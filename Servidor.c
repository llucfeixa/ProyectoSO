#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <ctype.h>
#include <mysql.h>
#include <pthread.h>

#define MAXPartidas 100

//Variables globales
MYSQL *conn;
int err;
MYSQL_RES *resultado;
MYSQL_ROW row;

pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;

int i;
int sockets[100];

//Estructuras
typedef struct{
	char nombre[20];
	int socket;
}Conectado;

typedef struct{
	Conectado conectados[100];
	int num;
}ListaConectados;

typedef struct{
	int libre;
	Conectado jugadores[4];
	int numInvitados;
	int numJugadores;
	int contestar;
	int vidaInicial[4];
	int vida[4];
	int ataque;
	int defensa;
	int numJugadoresVivos;
	int idCartaAtacante;
	int idCartaDefensora;
	int cartas;
	int numForm[4];
	int partidaBBDD;
	
	
}Partida;

typedef Partida TablaPartidas[MAXPartidas];

//Variables globales (listas y tablas)
ListaConectados listaConectados;
TablaPartidas tabla;

//Funciones y procedimientos
int Pon(ListaConectados *lista, char nombre[20], int socket)
//Añade nuevo conectado en la lista que recibe a partir del nombre y el socket.
//Retorna 0 si OK y -1 si la lista esta llena y no se ha podido añadir.
{
	if (lista->num==100)
		return -1;
	else{
		strcpy(lista->conectados[lista->num].nombre, nombre);
		lista->conectados[lista->num].socket = socket;
		lista->num++;
		return 0;
	}
}

int DamePosicion(ListaConectados *lista, char nombre[20])
//Devuelve la posicion del usuario a partir de su nombre o -1 si no está en la lista
{
	int i = 0;
	int encontrado = 0;
	while ((i < lista->num) && !encontrado)
	{
		if (strcmp(lista->conectados[i].nombre, nombre) == 0)
			encontrado = 1;
		if (!encontrado)
			i++;
	}
	if (encontrado)
		return i;
	else
		return -1;
}

int DameSocket(ListaConectados *lista, char nombre[20])
//Devuelve el socket del usuario a parir de su nombre o -1 si no está en la lista
{
	int i = 0;
	int encontrado = 0;
	while ((i < lista->num) && !encontrado)
	{
		if (strcmp(lista->conectados[i].nombre, nombre) == 0)
			encontrado = 1;
		if (!encontrado)
			i++;
	}
	if (encontrado)
		return lista->conectados[i].socket;
	else
		return -1;
}

int DameNombre(ListaConectados *lista, int sock, char nombre[20])
//Añade en el parámetro nombre el nombre del usuario cuyo socket recibe como parámetro. Devuelve 0 si OK o -1 si no está en la lista
{
	int i = 0;
	int encontrado = 0;
	while ((i < lista->num) && !encontrado)
	{
		if (lista->conectados[i].socket == sock)
			encontrado = 1;
		if (!encontrado)
			i++;
	}
	if (encontrado)
	{
		strcpy(nombre, lista->conectados[i].nombre);
		return 0;
	}
	else
		return -1;
}

int Elimina(ListaConectados *lista, char nombre[20])
//Retorna 0 si elimina al usuario cuyo nombre recibe como parámetro y -1 si ese usuario no esta en la lista
{
	int pos = DamePosicion(lista, nombre);
	if (pos == -1)
		return -1;
	else{
		int i;
		for(i=pos;i<lista->num-1;i++)
		{
			lista->conectados[i] = lista->conectados[i+1];
			strcpy(lista->conectados[i].nombre,lista->conectados[i+1].nombre);
			lista->conectados[i].socket=lista->conectados[i+1].socket;
		}
		lista->num--;
		return 0;
	}
}

void DameConectados(ListaConectados *lista, char conectados[300])
//Pone en conectados los nombres de todos los conectados separados por /
//Primero pone el número de conectados. Ejemplo: "3/Juan/Maria/Pedro"
{
	sprintf(conectados, "%d", lista->num);
	int i;
	for(i=0;i<lista->num;i++)
		sprintf(conectados, "%s/%s", conectados, lista->conectados[i].nombre);
}

void InicializarTabla(TablaPartidas tabla)
//Pone todas las partidas de la tabla de partidas con el campo libre, numJugadores y otros campos a 0, para poder ser usadas
{
	while(i<MAXPartidas)
	{
		tabla[i].libre=0;
		tabla[i].numJugadores=0;
		tabla[i].numInvitados=0;
		tabla[i].contestar=0;
		tabla[i].ataque=0;
		tabla[i].defensa=1;
		tabla[i].numJugadoresVivos=0;
		tabla[i].cartas=0;
		tabla[i].partidaBBDD=0;
		i=i+1;
	}
}

void EliminarPartida(TablaPartidas tabla, int id)
//Elimina una partida de la tabla a partir de su id, recibida como parámetro
{
	tabla[id].libre=0;
	tabla[id].numJugadores=0;
	tabla[id].numInvitados=0;
	tabla[id].contestar=0;
	tabla[id].ataque=0;
	tabla[id].defensa=1;
	tabla[id].numJugadoresVivos=0;
	tabla[id].cartas=0;
	tabla[i].partidaBBDD=0;
}

int PonPartidaBBDD()
//Añade una partida a la Base de Datos y retorna la id con la que la ha añadido
//Retorna -1 si ha habido algun error con la Base
{
	int id;
	char consulta[200];
	strcpy(consulta, "SELECT MAX(id) FROM Partida");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row[0] == NULL){
		id=1;
	}
	else
	{
		id = atoi(row[0])+1;
	}
	char consulta2[200];
	char fecha[20];
	int duracion=0;
	char ganador[20];
	sprintf(consulta2, "INSERT INTO Partida VALUES(%d,'%s',%d,'%s')", id, fecha, duracion, ganador);
	err=mysql_query (conn, consulta2);
	if (err!=0) {
		return -1;
		exit (1);
	}
	else
	{
		return id;
	}
}

int PonPartida(TablaPartidas tabla, ListaConectados *lista, char nombre[20], char invitados[100])
//Añade en una partida a los usuarios que van a jugar. Para ello, tiene el nombre de quien invita
//y los usuarios invitados separados por "/". Retorna la posición de la partida en la tabla o -1
//si no habia ninguna libre. Devuelve -2 si ha habido algun error con la Base de Datos cuando
//intentaba saber qué posición ocuparía en la base la partida
{
	int i=0;
	int encontrado=0;
	int partidaBBDD=-1;
	while(i<MAXPartidas && !encontrado)
	{
		if(tabla[i].libre == 0)
		{
			partidaBBDD=PonPartidaBBDD();
			if (partidaBBDD!=-1)
			{
				strcpy(tabla[i].jugadores[0].nombre, nombre);
				tabla[i].jugadores[0].socket=DameSocket(lista, nombre);
				tabla[i].numJugadores++;
				tabla[i].numJugadoresVivos++;
				tabla[i].partidaBBDD=partidaBBDD;
				char copia[100];
				strcpy(copia, invitados);
				char *p;
				p = strtok(copia, "/");
				int j = 1;
				while(p!=NULL)
				{
					strcpy(tabla[i].jugadores[j].nombre, p);
					tabla[i].jugadores[j].socket=DameSocket(lista, p);
					tabla[i].numJugadores++;
					tabla[i].numInvitados++;
					tabla[i].numJugadoresVivos++;
					p = strtok(NULL, "/");
					j++;
				}
				tabla[i].libre = 1;
			}
			encontrado = 1;
		}
		if (!encontrado)
		{
			i++;
		}
	}
	if (encontrado && partidaBBDD!=-1)
		return i;
	else if (!encontrado)
		return -1;
	else if (encontrado && partidaBBDD==-1)
	{
		return -2;
	}
}

int DamePosicionJugador(TablaPartidas tabla, char nombre[20], int id)
//Retorna la posicion de un jugador en la partida con id recibido como parámetro
//y su nombre o -1 si no lo ha encontrado
{
	int i = 0;
	int encontrado = 0;
	while ((i < tabla[id].numJugadores) && !encontrado)
	{
		if (strcmp(tabla[id].jugadores[i].nombre, nombre) == 0)
			encontrado = 1;
		if (!encontrado)
			i++;
	}
	if (encontrado)
		return i;
	else
		return -1;
}

int ContestarInvitacion(TablaPartidas tabla, int id)
//Retorna el número de personas que han contestado a la invitación de una partida
{
	tabla[id].contestar++;
	return tabla[id].contestar;
}

void CambioTurno(TablaPartidas tabla, int id)
//Cambia el turno de una partida a partir de la tabla de partidas y de la id de la partida
{
	if (tabla[id].numJugadoresVivos==2)
	{
		if (tabla[id].ataque==0)
		{
			tabla[id].ataque=1;
			tabla[id].defensa=0;
		}
		else
		{
			tabla[id].ataque=0;
			tabla[id].defensa=1;	
		}
	}	
	else if (tabla[id].numJugadoresVivos==3)
	{
		if (tabla[id].ataque==0)
		{
			tabla[id].ataque=1;
			tabla[id].defensa=2;
		}
		else if (tabla[id].ataque==1)
		{
			tabla[id].ataque=2;
			tabla[id].defensa=0;	
		}
		else
		{
			tabla[id].ataque=0;
			tabla[id].defensa=1;	
		}
	}
	else if (tabla[id].numJugadoresVivos==4)
	{
		if (tabla[id].ataque==0)
		{
			tabla[id].ataque=1;
			tabla[id].defensa=2;
		}
		else if (tabla[id].ataque==1)
		{
			tabla[id].ataque=2;
			tabla[id].defensa=3;	
		}
		else if (tabla[id].ataque==2)
		{
			tabla[id].ataque=3;
			tabla[id].defensa=0;	
		}
		else
		{
			tabla[id].ataque=0;
			tabla[id].defensa=1;
		}
	}
	tabla[id].cartas=0;
}

int AbrirBaseDatos()
//Retorna 0 si OK o -1 y -2 si no se ha abierto correctamente
{
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1);
		return -1;
	}
	conn = mysql_real_connect (conn, "shiva2.upc.es","root", "mysql", "M3Juego",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1);
		return -2;
	}
	return 0;
}

void CerrarBaseDatos()
//Cierra la base de datos
{
	mysql_close (conn);
	exit(0);
}

int LogIn(char usuario[20], char password[20])
//Retorna 0 si el usuario se ha logueado correctamente a partir de su usuario y su contraseña, -1 si ha
//habido algún error al consultar la base de datos o -2 si ese usuario no existe o su contraseña es incorrecta
//Retorna -3 si ese usuario ya estaba conectado
{
	char consulta[200];
	sprintf(consulta, "SELECT Jugador.conectado FROM Jugador WHERE Jugador.nombre='%s' AND Jugador.contraseña='%s'", usuario, password);
	err=mysql_query (conn, consulta);
	if (err!=0) {
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		return -2;
	}
	else
	{
		if (atoi(row[0])==0)
		{
			char consulta2[200];
			strcpy(consulta2,"UPDATE Jugador SET Jugador.conectado=1 WHERE Jugador.nombre = '");
			strcat (consulta2, usuario);
			strcat(consulta2,"'");
			err=mysql_query(conn, consulta2);
			if (err!=0) {
				return -1;
				exit (1);
			}
			else
			{
				return 0;
			}
		}
		else
		{
			return -3;
		}
	}
}

int Desconectarse(char usuario[20])
//Desconexion de un jugador a partir de su nombre pasado como parametro en la base de datos. Retorna -1 si ha habido algun error o 0 si OK
{
	char consulta[200];
	strcpy(consulta,"UPDATE Jugador SET Jugador.conectado=0 WHERE Jugador.nombre = '");
	strcat (consulta, usuario);
	strcat(consulta,"'");
	err=mysql_query(conn, consulta);
	if (err!=0) {
		return -1;
		exit (1);
	}	
	else
	{
		return 0;
	}
}

int DesconectarJugadores()
//Desconecta a todos los jugadores de la base de datos. Retorna -1 si ha habido algun error o 0 si OK
{
	char consulta[200];
	strcpy(consulta,"UPDATE Jugador SET Jugador.conectado=0");
	err=mysql_query(conn, consulta);
	if (err!=0) {
		return -1;
		exit (1);
	}	
	else
	{
		return 0;
	}
}

int Register(char usuario[20], char password[20])
//Retorna 0 si el usuario se ha registrado correctamente a partir de un usuario y una contraseña, -1 si ha
//habido algún error al consultar la base de datos o -2 si ha habido un error al insertarlo en la base
{
	char consulta[200];
	sprintf(consulta, "SELECT * FROM Jugador WHERE Jugador.nombre='%s'", usuario);
	err=mysql_query (conn, consulta);
	if (err!=0) {
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		int id;
		char consulta2[200];
		strcpy(consulta2, "SELECT MAX(Jugador.id) FROM Jugador");
		err=mysql_query (conn, consulta2);
		if (err!=0) {
			return -1;
			exit (1);
		}
		resultado = mysql_store_result (conn);
		row = mysql_fetch_row (resultado);
		if (row[0] == NULL){
			id=1;
		}
		else
		{
			id = atoi(row[0])+1;
		}
		int nivel = 1;	
		int experiencia = 0;
		char consulta3[200];
		int conectado=0;
		sprintf(consulta3, "INSERT INTO Jugador VALUES(%d,'%s','%s',%d,%d,%d)", id, usuario, password, nivel, experiencia,conectado);
		err=mysql_query (conn, consulta3);
		if (err!=0) {
			return -1;
			exit (1);
		}
		else
		{
			return 0;
		}
	}
	else
	{
		return -2;
	}
}

int Historial(char nombre[20], char respuesta[1000])
//Inserta en el parámetro respuesta el historial de las últimas 20 partidas de un jugador cuyo nombre se recibe como parámetro
//Retorna 0 si OK, -1 si ha habido algun error o -2 si no ha jugado ninguna partida
//El formato de la respuesta es con el codigo 3, seguido por el numero de partidas, la fecha, la duracion y
//el ganador de cada partida, todo separado por /
{
	memset(respuesta, 0, strlen(respuesta));
	char consulta[200];
	strcpy (consulta,"SELECT Partida.fecha,Partida.duracion,Partida.ganador FROM Jugador,Partida,Historial WHERE Jugador.nombre = '");
	strcat (consulta, nombre);
	strcat (consulta,"'AND Jugador.id = Historial.idJ AND Historial.idP = Partida.id ORDER BY Partida.id DESC");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		strcpy(respuesta, "3/-1/Error al consultar datos de la base.");
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		strcpy(respuesta, "3/-2/No se han obtenido datos en la consulta.");
		return -2;
	}
	else
	{
		char historial[1000];
		memset(historial, 0, strlen(historial));
		int cont=0;
		while (row!=NULL && cont<20) {
			sprintf(historial, "%s/%s/%d/%s", historial,row[0],atoi(row[1]),row[2]);
			row = mysql_fetch_row (resultado);
			cont++;
		}
		sprintf(respuesta, "3/%d%s",cont,historial);
		return 0;
	}
}

int PartidasJugadas(char nombre[20])
//Retorna el numero de partidas jugadas por el jugador cuyo nombre se recibe como parámetro,
//-1 si ha habido algun error o -2 si no ha jugado ninguna partida
{
	char consulta[200];
	strcpy (consulta,"SELECT COUNT(Historial.idJ) FROM Jugador,Historial WHERE Jugador.nombre = '");
	strcat (consulta, nombre);
	strcat (consulta,"'AND Jugador.id = Historial.idJ");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		return -2;
	}
	else
	{
		return atoi(row[0]);
	}
}

int PartidasGanadas(char nombre[20])
//Retorna el numero de partidas ganadas por el jugador cuyo nombre se recibe como parámetro,
//-1 si ha habido algun error o -2 si no ha jugado ninguna partida
{
	char consulta[200];
	strcpy (consulta,"SELECT COUNT(Partida.ganador) FROM Partida WHERE Partida.ganador = '");
	strcat (consulta, nombre);
	strcat (consulta,"'");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		return -2;
	}
	else
	{
		return atoi(row[0]);
	}
}

int JugadorMasExperiencia(char respuesta[512])
//Inserta el respuesta con el codigo 23 cuantos jugadores distintos son los que tienen más experiencia, junto con su nivel,
//su experiencia y sus nombres separados por barras. Retrona 0 si OK, -1 si ha habido algun error en la consulta
//o -2 si no se han obtenido datos en la consulta
{
	memset(respuesta, 0, strlen(respuesta));
	char consulta[200];
	strcpy (consulta,"SELECT Jugador.nombre,Jugador.nivel,Jugador.experiencia FROM Jugador WHERE Jugador.experiencia=(SELECT MAX(Jugador.experiencia) FROM Jugador)");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		strcpy(respuesta, "23/-1/Error al consultar datos de la base.");
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		strcpy(respuesta, "23/-2/No se han obtenido datos en la consulta.");
		return -2;
	}
	else
	{
		char jugadores[512];
		memset(jugadores, 0, strlen(jugadores));
		int cont=0;
		int nivel=atoi(row[1]);
		int experiencia=atoi(row[2]);
		while (row!=NULL) {
			sprintf(jugadores, "%s/%s", jugadores,row[0]);
			row = mysql_fetch_row (resultado);
			cont++;
		}
		sprintf(respuesta, "23/%d/%d/%d%s",cont,nivel,experiencia,jugadores);
		return 0;
	}
}

int JugadorMasGanadas(char respuesta[512])
//Inserta el respuesta con el codigo 24 cuantos jugadores distintos son los que tienen más partidas ganadas, junto con el numero de ganadas,
//sus nombres separados por barras. Retrona 0 si OK, -1 si ha habido algun error en la consulta o -2 si no se han obtenido datos en la consulta
{
	memset(respuesta, 0, strlen(respuesta));
	char consulta[200];
	strcpy (consulta,"SELECT Partida.ganador,COUNT(Partida.ganador) AS total FROM Partida GROUP BY Partida.ganador ORDER BY total DESC");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		strcpy(respuesta, "24/-1/Error al consultar datos de la base.");
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		strcpy(respuesta, "24/-2/No se han obtenido datos en la consulta.");
		return -2;
	}
	else
	{
		char jugadores[512];
		memset(jugadores, 0, strlen(jugadores));
		int cont=0;
		int ganadas=atoi(row[1]);
		while (row!=NULL) {
			if (ganadas==atoi(row[1]))
			{
				sprintf(jugadores, "%s/%s", jugadores,row[0]);
				cont++;
			}
			row = mysql_fetch_row (resultado);
		}
		sprintf(respuesta, "24/%d/%d%s",cont,ganadas,jugadores);
		return 0;
	}
}

int DameExperiencia(char nombre[20])
//Retorna la experiencia del jugador cuyo nombre recibe como parametro, -1 si ha habido algun error o -2 si no ha obtenido datos en la consulta
{
	char consulta[200];
	strcpy (consulta,"SELECT Jugador.experiencia FROM Jugador WHERE Jugador.nombre='");
	strcat (consulta,nombre);
	strcat (consulta,"'");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		return -2;
	}
	else
	{
		return atoi(row[0]);
	}
}

int ExperienciaPorPosicion(int pos)
//Retorna la experiencia que recibe un jugador por su posicion en la partida, la cual se recibe como parametro
{
	int experiencia=0;
	if (pos==1)
		experiencia=75;
	else if (pos==2)
		experiencia=50;
	else if (pos==3)
		experiencia=30;
	else if (pos==4)
		experiencia=20;
	return experiencia;
}

int SubirExperiencia(char nombre[20], int pos)
//Actualiza la experiencia de un jugador cuyo nombre se recibe como parámetro a partir de su posición (parámetro) en una partida
//Retorna la experiencia del jugador o -1 si ha habido algun error (puede retornar -2 si es lo que obtiene de la funcion DameExperiencia)
{
	int experiencia=ExperienciaPorPosicion(pos);
	char consulta[200];
	strcpy(consulta,"UPDATE Jugador SET Jugador.experiencia=Jugador.experiencia+");
	sprintf(consulta,"%s%d",consulta,experiencia);
	strcat(consulta," WHERE Jugador.nombre = '");
	strcat (consulta, nombre);
	strcat(consulta,"'");
	err=mysql_query(conn, consulta);
	if (err!=0) {
		return -1;
		exit (1);
	}
	else
	{
		return DameExperiencia(nombre);
	}
}

int DameNivel(char nombre[20])
//Retorna el nivel del jugador cuyo nombre recibe como parametro, -1 si ha habido algun error o -2 si no ha obtenido datos en la consulta
{
	char consulta[200];
	strcpy (consulta,"SELECT Jugador.nivel FROM Jugador WHERE Jugador.nombre='");
	strcat (consulta,nombre);
	strcat (consulta,"'");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row[0] == NULL){
		return -2;
	}
	else
	{
		return atoi(row[0]);
	}
}

int NivelPorExperiencia(int experiencia)
//Retorna el nivel del jugador a partir de su experiencia, la cual se recibe como parámetro
{
	int nivel;
	if (0<=experiencia && experiencia<100)
		nivel=1;
	else if (100<=experiencia && experiencia<300)
		nivel=2;
	else if (300<=experiencia && experiencia<600)
		nivel=3;
	else if (600<=experiencia && experiencia<1000)
		nivel=4;
	else if (1000<=experiencia && experiencia<1500)
		nivel=5;
	else if (1500<=experiencia && experiencia<2500)
		nivel=6;
	else if (2500<=experiencia && experiencia<5000)
		nivel=7;
	else if (5000<=experiencia && experiencia<7500)
		nivel=8;
	else if (7500<=experiencia && experiencia<10000)
		nivel=9;
	else
		nivel=10;
	return nivel;
}

int SubirNivel(char nombre[20])
//Actualiza el nivel de un jugador cuyo nombre se recibe como parámetro a partir de su experiencia que obtiene de la base de datos
//Retorna el nivel del jugador o -1 si ha habido algun error (puede retornar -1 o -2 si es lo que obtiene de la funcion DameExperiencia)
{
	int experiencia = DameExperiencia(nombre);
	if (experiencia!=-1 && experiencia!=-2)
	{
		int experiencia=atoi(row[0]);
		int nivel=NivelPorExperiencia(experiencia);
		char consulta2[200];
		strcpy(consulta2,"UPDATE Jugador SET Jugador.nivel=");
		sprintf(consulta2,"%s%d",consulta2, nivel);
		strcat(consulta2," WHERE Jugador.nombre = '");
		strcat (consulta2, nombre);
		strcat(consulta2,"'");
		err=mysql_query(conn, consulta2);
		if (err!=0) {
			return -1;
			exit (1);
		}
		else
		{
			return nivel;
		}
	}
	else
		return experiencia;
}

int ExperienciaEnNivel(int experiencia, int nivel)
//Retorna la experiencia dentro de un nivel a partir de la experiencia total y el nivel, ambos recibidos como parámetros
{
	int experienciaEnNivel;
	if (nivel==1)
		experienciaEnNivel=experiencia;
	else if (nivel==2)
		experienciaEnNivel=experiencia-100;
	else if (nivel==3)
		experienciaEnNivel=experiencia-300;
	else if (nivel==4)
		experienciaEnNivel=experiencia-600;
	else if (nivel==5)
		experienciaEnNivel=experiencia-1000;
	else if (nivel==6)
		experienciaEnNivel=experiencia-1500;
	else if (nivel==7)
		experienciaEnNivel=experiencia-2500;
	else if (nivel==8)
		experienciaEnNivel=experiencia-5000;
	else if (nivel==9)
		experienciaEnNivel=experiencia-7500;
	else
		experienciaEnNivel=2500;
	return experienciaEnNivel;		
}

int ExperienciaMaxima(int nivel)
//Retorna la experiencia máxima dentro de un nivel, recibiendo este último como parámetro
{
	int experienciaMaxima;
	if (nivel==1)
		experienciaMaxima=100;
	else if (nivel==2)
		experienciaMaxima=200;
	else if (nivel==3)
		experienciaMaxima=300;
	else if (nivel==4)
		experienciaMaxima=400;
	else if (nivel==5)
		experienciaMaxima=500;
	else if (nivel==6)
		experienciaMaxima=1000;
	else if (nivel==7)
		experienciaMaxima=2500;
	else if (nivel==8)
		experienciaMaxima=2500;
	else if (nivel==9)
		experienciaMaxima=2500;
	else
		experienciaMaxima=2500;
	return experienciaMaxima;
}

int IdJugador(char nombre[20])
//Retorna la id de un jugador cuyo nombre se recibe como parametro, -1 si ha habido algun error o -2 si no se han obtenido datos en la consulta
{	
	char consulta[200];
	sprintf(consulta, "SELECT Jugador.id FROM Jugador WHERE Jugador.nombre='%s'",nombre);
	err=mysql_query (conn, consulta);
	if (err!=0) {
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row[0] == NULL){
		return -2;
	}
	else
	{
		return atoi(row[0]);
	}
}

int AnadirCarta(char nombre[20],int idCarta)
//Añade la carta cuya id se recibe como parametro (idCarta) en Mazo asignado a un jugador cuyo nombre se recibe como parametro
//Retorna 0 si OK o -1 si ha habido algun error (puede retornar -1 o -2 si es lo que se obtiene en la funcion IdJugador)
{
	int idJugador=IdJugador(nombre);
	if (idJugador!=-1 && idJugador!=-2)
	{
		char consulta[200];
		sprintf(consulta,"INSERT INTO Mazo VALUES(%d,%d)",idJugador,idCarta);
		err=mysql_query(conn, consulta);
		if (err!=0)
		{
			return -1;
			exit(1);
		}
		else
		{
			return 0;	
		}
	}
	else
		return -2;
}

int AceptarMazo(char nombre[20], char mazo[20])
//Añade las cartas recibidas por / en el parámetro mazo a la tabla de la base de datos Mazo,
//asignando estas cartas al nombre recibido como parametro. Retorna el resultado de AñadirCartas,
//es decir, 0 si OK y -1 o -2 si ha habido algún problema
{
	int res=0;
	char copia[20];
	strcpy(copia,mazo);
	char *p = strtok(copia, "/");
	while (p!=NULL)
	{
		int idCarta=atoi(p);
		res=AnadirCarta(nombre, idCarta);
		if (res!=0)
		{
			return res;
		}
		p=strtok(NULL,"/");
	}
	return res;
}

int EliminarMazo(char nombre[20])
//Elimina el mazo asociado al jugador cuyo nombre se recibe como parámetro. Retorna 0 si OK y -1 si
//ha habido algún error (puede retronar -1 o -2 si es lo que se obtiene en la función IdJugador)
{
	int idJugador=IdJugador(nombre);
	if (idJugador!=-1 && idJugador!=-2)
	{
		char consulta[200];
		sprintf(consulta,"DELETE FROM Mazo WHERE Mazo.idJ=%d",idJugador);
		err=mysql_query(conn, consulta);
		if (err!=0)
		{
			return -1;
			exit(1);
		}
		else
		{
			return 0;	
		}
	}
	else
		return -2;
}

int VidaTotal(char nombre[20])
//Retorna la vida total del mazo activo del jugador cuyo nombre recibe como parámetro, -1 si ha habido algún error
//o -2 si no se ha obtenido nada en la consulta
{
	char consulta[200];
	strcpy (consulta,"SELECT SUM(Carta.vida) FROM Jugador,Carta,Mazo WHERE Jugador.nombre='");
	strcat (consulta,nombre);
	strcat (consulta,"'AND Jugador.id=Mazo.idJ AND Mazo.idC=Carta.id");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row[0] == NULL){
		return -2;
	}
	else
	{
		return atoi(row[0]);
	}
}
int PonerVida(TablaPartidas tabla, int idPartida)
//Añade la vida de los jugadores en la partida cuya id se recibe como párametro dentro de la tabla de partidas (parámetro)
//Retorna 0 si OK y -1 o -2 si ha habido algún problema al añadirla
{
	int vida=0;
	int i=0;
	int encontrado=0;
	while(i<tabla[idPartida].numJugadores && !encontrado)
	{
		char nombre[20];
		strcpy(nombre, tabla[idPartida].jugadores[i].nombre);
		vida=VidaTotal(nombre);
		if (vida!=-1 && vida!=-2)
		{
			tabla[idPartida].vidaInicial[i]=vida;
			tabla[idPartida].vida[i]=vida;
			i++;
		}
		else
		{
			encontrado=1;
		}
	}
	if (i==tabla[idPartida].numJugadores)
	{
		return 0;
	}
	else
	{
		return vida;
	}
}

void NombresVidaJugadores(TablaPartidas tabla, int id, char nombresVidaJugadores[100])
//Procedimiento para obtener los nombres de los jugadores que estan vivos en la partida
//con esa id. Los nombres, la vida y la vida inicial se guardan en el parámetro nombresVidaJugadores separados por /
{
	int i = 0;
	while(i<tabla[id].numJugadoresVivos)
	{
		char nombre[20];
		strcpy(nombre,tabla[id].jugadores[i].nombre);
		int vidaInicial=tabla[id].vidaInicial[i];
		int vida=tabla[id].vida[i];
		sprintf(nombresVidaJugadores,"%s%s/%d/%d/",nombresVidaJugadores,nombre,vida,vidaInicial);
		i++;
	}
	nombresVidaJugadores[strlen(nombresVidaJugadores)-1]='\0';
}

int AtacarDefender(int idCartaAtacante, int idCartaDefensora)
//A partir de la id de la carta que ataca y la que defiende, comprueba cuánta vida la quita la carta que ataca a la que defiende
//y devuelve ese valor siempre que sea mayor o igual que 0. En caso contrario (cuando es menor que 0), devuelve 0
//Retorna -1 si ha habido algun error o -2 si no encuentra esa carta
{
	int ataque;
	int defensa;
	char consulta[200];
	sprintf(consulta, "SELECT Carta.ataque FROM Carta WHERE Carta.id=%d",idCartaAtacante); 
	err=mysql_query (conn, consulta);
	if (err!=0) {
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		return -2;
	}
	else
	{
		ataque=atoi(row[0]);
	}
	char consulta2[200];
	sprintf(consulta2, "SELECT Carta.defensa FROM Carta WHERE Carta.id=%d",idCartaDefensora); 
	err=mysql_query (conn, consulta2);
	if (err!=0) {
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		return -2;
	}
	else
	{
		defensa=atoi(row[0]);
	}
	int vidaRestada=ataque-defensa;
	if (vidaRestada<0)
		return 0;
	else
		return vidaRestada;
}

void EliminarJugadorMuerto(TablaPartidas tabla, int idJugador, int idPartida)
//Elimina a un jugador cuyo id se recibe como parámetro (idJugador) de una partida con idPartida también siendo un parámetro
//Lo añade al final del vector de jugadores de la tabla, así como todos sus atributos, para que no participe en la partida
//activamente, pero pueda seguir recibiendo la información de la partida hasta que desee salir
{
	tabla[idPartida].numJugadoresVivos--;
	int num=tabla[idPartida].numJugadoresVivos;
	int i = idJugador;
	Conectado auxiliar=tabla[idPartida].jugadores[i];
	int vidaInicialAuxiliar=tabla[idPartida].vidaInicial[i];
	int vidaAuxiliar=tabla[idPartida].vida[i];
	int numFormAuxiliar=tabla[idPartida].numForm[i];
	while (i<num)
	{
		tabla[idPartida].jugadores[i]=tabla[idPartida].jugadores[i+1];
		tabla[idPartida].vidaInicial[i]=tabla[idPartida].vidaInicial[i+1];
		tabla[idPartida].vida[i]=tabla[idPartida].vida[i+1];
		tabla[idPartida].numForm[i]=tabla[idPartida].numForm[i+1];
		i++;
	}
	tabla[idPartida].jugadores[num]=auxiliar;
	tabla[idPartida].vidaInicial[num]=vidaInicialAuxiliar;
	tabla[idPartida].vida[num]=vidaAuxiliar;
	tabla[idPartida].numForm[num]=numFormAuxiliar;
}

int EliminarJugadorTabla(TablaPartidas tabla, char nombre[20], int id)
//Elimina a un jugador de la tabla después de que haya rechazado la invitacion a una partida o haya abandonado esa partida
//a partir de su nombre y la id de la partida, es decir, la posición en la tabla. Retorna 0
//si OK o -1 si no ha encontrado ese jugador en la partida
//Elimina la partida de la tabla si es el ultimo jugador
{
	int pos = DamePosicionJugador(tabla, nombre, id);
	if(pos == -1)
		return -1;
	else
	{
		int i;
		for(i=pos;i<tabla[id].numJugadores-1;i++)
		{
			tabla[id].jugadores[i] = tabla[id].jugadores[i+1];
			tabla[id].vidaInicial[i]=tabla[id].vidaInicial[i+1];
			tabla[id].vida[i]=tabla[id].vida[i+1];
			tabla[id].numForm[i] = tabla[id].numForm[i+1];
		}
		tabla[id].numJugadores--;
		tabla[id].numJugadoresVivos--;
		if (tabla[id].numJugadores==0)
		{
			EliminarPartida(tabla,id);
		}
		return 0;	
	}
}

int ActualizarVida(TablaPartidas tabla, int idJugador, int idPartida, int idCartaAtacante, int idCartaDefensora)
//Actualiza el atributo vida del jugador defensor a partir de su id (idJugador) recibido como parámetro junto con la idPartida
//y las id de las cartas atacante y defensora. Retorn la vida del jugador, teniendo en cuenta que si es menor que 0 devuelve 0
{
	int vidaRestada=AtacarDefender(idCartaAtacante, idCartaDefensora);
	tabla[idPartida].vida[idJugador]=tabla[idPartida].vida[idJugador]-vidaRestada;
	if (tabla[idPartida].vida[idJugador]<0)
	{
		tabla[idPartida].vida[idJugador]=0;
	}
	return tabla[idPartida].vida[idJugador];
}

int Mazo(char nombre[20], char mazo[20])
//Añade en el parámetro mazo las id de las cartas que forman el mazo del jugador cuyo nombre se recibe como parámetro
//Retorna 0 si OK, -1 si ha habido algun error o -2 si no ha obtenido datos en la consulta
{
	char consulta[200];
	strcpy(consulta, "SELECT Mazo.idC FROM Jugador,Mazo WHERE Jugador.nombre='");
	strcat(consulta,nombre);
	strcat(consulta,"' AND Jugador.id=Mazo.idJ");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		strcpy(mazo,"-1");
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		strcpy(mazo,"-2");
		return -2;
	}
	else
	{
		char cartas[20];
		memset(cartas,0,sizeof(cartas));
		int cont=0;
		while (row!=NULL) 
		{
			int idCarta=atoi(row[0]);
			sprintf(cartas, "%s%d/", cartas,idCarta);
			row = mysql_fetch_row (resultado);
			cont++;
		}
		sprintf(mazo,"%d/%s",cont,cartas);
		mazo[strlen(mazo)-1] = '\0';
		return 0;
	}
}

int CartaSeleccionada(int idCarta, char respuesta[512])
//Añade en respuesta todos los datos de la carta cuya id se recibe como parámetro después del codigo 17
//Los parámetros son el nombre, la vida, el ataque, la defensa y la descripción
//Retorna 0 si OK, -1 si ha habido algun error o -2 si no ha obtenido datos en la consulta
{
	char consulta[200];
	sprintf(consulta, "SELECT * FROM Carta WHERE Carta.id=%d",idCarta); 
	err=mysql_query (conn, consulta);
	if (err!=0) {
		strcpy(respuesta,"17/-1");
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		strcpy(respuesta,"17/-2");
		return -2;
	}
	else
	{
		char nombre[20];
		strcpy(nombre, row[1]);
		int vida=atoi(row[2]);
		int ataque=atoi(row[3]);
		int defensa=atoi(row[4]);
		char descripcion[500];
		strcpy(descripcion, row[5]);
		sprintf(respuesta,"17/%d/%s/%d/%d/%d/%s",idCarta,nombre,vida,ataque,defensa,descripcion);
		return 0;
	}
}

int CartaSeleccionadaTablero(int idCarta, char respuesta[512])
//Añade en respuesta algunos de los datos de la carta cuya id se recibe como parámetro
//Los parámetros son el nombre, el ataque y la defensa (también añade la id de la carta)
//Retorna 0 si OK, -1 si ha habido algun error o -2 si no ha obtenido datos en la consulta
{
	char consulta[200];
	sprintf(consulta, "SELECT Carta.nombre,Carta.ataque,Carta.defensa FROM Carta WHERE Carta.id=%d",idCarta); 
	err=mysql_query (conn, consulta);
	if (err!=0) {
		strcpy(respuesta,"-1");
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		strcpy(respuesta,"-2");
		return -2;
	}
	else
	{
		char nombre[20];
		strcpy(nombre, row[0]);
		int ataque=atoi(row[1]);
		int defensa=atoi(row[2]);
		sprintf(respuesta,"%d/%s/%d/%d",idCarta,nombre,ataque,defensa);
		return 0;
	}
}

int AgregarHistorial(char nombre[20],int idPartida,int pos)
//Añade en la tabla Historial de la base de datos la posicion (parámetro pos) del jugador cuyo nombre se recibe como parametro
//en una partida cuya id es idPartida. Retorna 0 si OK y -1 si no se ha podido añadir
{
	int idJ=IdJugador(nombre);
	char consulta[200];
	sprintf(consulta,"INSERT INTO Historial VALUES(%d,%d,%d)",idJ,idPartida,pos);
	err=mysql_query(conn, consulta);
	if (err!=0) {
		return -1;
		exit (1);
	}
	else
	{
		return 0;
	}
}

int ActualizarPartidaBBDD(int idPartida,char fecha[30],int duracion,char nombre[20])
//Actualiza los resultados de una partida una vez a finalizado a partir de su fecha, la duracion y el nombre del ganador
//conociendo la id de la partida, siendo todo recibido como parámetros. Retorna 0 si OK y -1 si no se ha podido actualizar
{
	char consulta[200];
	strcpy(consulta,"UPDATE Partida SET Partida.fecha='");
	strcat(consulta,fecha);
	strcat(consulta,"',Partida.duracion=");
	sprintf(consulta,"%s%d",consulta,duracion);
	strcat(consulta,",Partida.ganador='");
	strcat (consulta, nombre);
	strcat(consulta,"' WHERE Partida.id=");
	sprintf(consulta,"%s%d",consulta,idPartida);
	err=mysql_query(conn, consulta);
	if (err!=0) {
		return -1;
		exit (1);
	}
	else
	{
		return 0;
	}
}

int DarBaja(char nombre[20], char password[20])
//Borra los datos del jugador cuyo nombre y contraseña se reciben como parámetros
//Retorna 0 si OK o -1 si ha habido algun error (puede retornar -1, -2 o -3 si es el resultado de la función LogIn)
{
	int res=LogIn(nombre,password);
	if (res==0)
	{
		int id=IdJugador(nombre);
		char consulta[200];
		sprintf(consulta, "DELETE FROM Historial WHERE Historial.idJ=%d",id);
		err=mysql_query (conn, consulta);
		if (err!=0) {
			return -1;
			exit (1);
		}
		else
		{
			char consulta2[200];
			sprintf(consulta2, "DELETE FROM Mazo WHERE Mazo.idJ=%d",id);
			err=mysql_query (conn, consulta2);
			if (err!=0) {
				return -1;
				exit (1);
			}
			else
			{
				char consulta3[200];
				sprintf(consulta3, "DELETE FROM Jugador WHERE Jugador.id=%d",id);
				err=mysql_query (conn, consulta3);
				if (err!=0) {
					return -1;
					exit (1);
				}
				else
				{
					char consulta4[200];
					sprintf(consulta4, "UPDATE Partida SET Partida.ganador='Usuario eliminado' WHERE Partida.ganador='%s'",nombre);
					err=mysql_query (conn, consulta4);
					if (err!=0) {
						return -1;
						exit (1);
					}
					else
					{
						return 0;
					}
				}
			}
		}
	}
	else
	{
		return res;
	}
}
	
int Adversarios(char nombre[20], char respuesta[512])
//Añade en respuesta los usuarios (separados por / despues del codigo 28 y el contador de cuántos rivales hay) contra los que se ha enfrentado
//el jugador cuyo nombre se recibe como parámetro. Retorna 0 si OK, -1 si ha habido algun error o -2 si no ha obtenido datos en la consulta
{
	char consulta[300];
	strcpy (consulta,"SELECT DISTINCT(Jugador.nombre) FROM Jugador,Historial WHERE Jugador.nombre!='");
	strcat (consulta, nombre);
	strcat (consulta,"'AND Jugador.id=Historial.idJ AND Historial.idP IN (SELECT Historial.idP FROM Jugador,Historial WHERE Jugador.nombre='");
	strcat (consulta, nombre);
	strcat (consulta,"' AND Jugador.id=Historial.idJ)");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		strcpy(respuesta,"28/-1/Error al consultar datos de la base.");
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		strcpy(respuesta,"28/-2/No se han obtenido datos en la consulta.");
		return -2;
	}
	else
	{
		int cont = 0;
		char adversarios[512];
		memset(adversarios, 0, strlen(adversarios));
		while (row !=NULL) {
			sprintf(adversarios,"%s/%s",adversarios,row[0]);
			cont = cont + 1;
			row = mysql_fetch_row (resultado);
		}
		sprintf(respuesta,"28/%d%s",cont,adversarios);
		return 0;
	}
	
}

int Ayuda(int max, char nombre[20], char jugador[20], char auxiliar[512])
//Añade en auxiliar un máximo de max partidas en las que dos jugadores recibidos como parámetro se han enfrentado.Añade la fecha de la partida, su duración
//y el ganador. Retorna el numero de partidas añadidas si OK, -1 si ha habido algun error o -2 si no ha obtenido datos en la consulta 
{
	char consulta[512];
	strcpy (consulta,"SELECT Partida.fecha,Partida.duracion,Partida.ganador FROM Jugador,Partida,Historial WHERE Jugador.nombre = '");
	strcat (consulta, jugador);
	strcat (consulta,"'AND Jugador.id=Historial.idJ AND Historial.idP IN (SELECT Historial.idP FROM Jugador,Historial WHERE Jugador.nombre='");
	strcat (consulta, nombre);
	strcat (consulta,"' AND Jugador.id=Historial.idJ) AND Historial.idP = Partida.id ORDER BY Partida.id DESC");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		return -2;
	}
	else
	{
		int cont=0;
		while (row!=NULL && cont<max) {
			sprintf(auxiliar, "%s/%s/%d/%s", auxiliar,row[0],atoi(row[1]),row[2]);
			cont++;
			row = mysql_fetch_row (resultado);
		}
		return cont;
	}
}
	
int ResultadosJugadores(char nombre[20],char adversarios[512],char respuesta[1000])
//A partir del nombre del jugador y diferentes rivales dentro del vector de adversarios separados por /, 
//añade en el vector respuesta un máximo de 20 partidas en las que el jugador se ha enfrentado a ellos
//Retorna 0 si OK, -1 si ha habido algún error o -2 si no se han obtenido datos en la consulta 
{
	memset(respuesta, 0, strlen(respuesta));
	char historial[1000];
	memset(historial, 0, strlen(historial));
	char copia[512];
	strcpy(copia,adversarios);
	char *p = strtok(copia, "/");
	int num=atoi(p);
	int max=20/num;
	int i=0;
	int numObtenidos=0;
	int resultado;
	while (i<num)
	{
		p=strtok(NULL,"/");
		char jugador[20];
		memset(jugador, 0, strlen(jugador));
		strcpy(jugador,p);
		char auxiliar[512];
		memset(auxiliar, 0, strlen(auxiliar));
		resultado=Ayuda(max,nombre,jugador,auxiliar);
		if (resultado>=0)
		{
			sprintf(historial,"%s%s",historial,auxiliar);
			numObtenidos=numObtenidos+resultado;
		}
		else if (resultado==-1)
		{
			strcpy(respuesta,"3/-1/Error al consultar datos de la base");
			return -1;
			exit(1);
		}
		i++;
	}
	if (numObtenidos==0)
	{
		strcpy(respuesta,"3/-2/No se han obtenido datos en la consulta");
		return -2;
	}
	else
	{
		sprintf(respuesta,"3/%d%s",numObtenidos,historial);
		return 0;
	}
}
	
int PartidasJugadasTiempo(char nombre[20],char fecha[20],char respuesta[1000])
//Añade en respuesta las 20 partidas más recientes que están entre una fecha y otra, obtenidas a partir del parámetro fecha, las cuales
//ha jugado un jugador cuyo nombre se recibe como parámetro. Añade la fecha de esa partida, su duracion y el ganador, a parte del numero total de partidas
//Retorna 0 si OK, -1 si ha habido algún error o -2 si no se han obtenido datos en la consulta 
{
	memset(respuesta, 0, strlen(respuesta));
	char copia[512];
	strcpy(copia,fecha);
	char *p = strtok(copia, "/");
	int diaInicial=atoi(p);
	p=strtok(NULL,"/");
	int mesInicial=atoi(p);
	p=strtok(NULL,"/");
	int anoInicial=atoi(p);
	p=strtok(NULL,"/");
	int horaInicial=atoi(p);
	p=strtok(NULL,"/");
	int minutosInicial=atoi(p);
	p=strtok(NULL,"/");
	int segundosInicial=atoi(p);
	int tiempoFechaInicial=anoInicial*365+mesInicial*30+diaInicial; 
	int tiempoDiaInicial=horaInicial*3600+minutosInicial*60+segundosInicial;
	p = strtok(NULL, "/");
	int diaFinal=atoi(p);
	p=strtok(NULL,"/");
	int mesFinal=atoi(p);
	p=strtok(NULL,"/");
	int anoFinal=atoi(p);
	p=strtok(NULL,"/");
	int horaFinal=atoi(p);
	p=strtok(NULL,"/");
	int minutosFinal=atoi(p);
	p=strtok(NULL,"/");
	int segundosFinal=atoi(p);
	int tiempoFechaFinal=anoFinal*365+mesFinal*30+diaFinal; 
	int tiempoDiaFinal=horaFinal*3600+minutosFinal*60+segundosFinal;
	char consulta[200];
	strcpy (consulta,"SELECT Partida.fecha,Partida.duracion,Partida.ganador FROM Jugador,Partida,Historial WHERE Jugador.nombre = '");
	strcat (consulta, nombre);
	strcat (consulta,"'AND Jugador.id = Historial.idJ AND Historial.idP = Partida.id ORDER BY Partida.id DESC");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		strcpy(respuesta, "3/-1/Error al consultar datos de la base.");
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		strcpy(respuesta, "3/-2/No se han obtenido datos en la consulta.");
		return -2;
	}
	else
	{
		char fecha[20];
		char tiempo[20];
		int dia;
		int mes;
		int ano;
		int hora;
		int minutos;
		int segundos;
		char historial[1000];
		memset(historial, 0, strlen(historial));
		int cont=0;
		while (row!=NULL && cont<20)
		{
			strcpy(copia,row[0]);
			p=strtok(copia," ");
			strcpy(fecha,p);
			p=strtok(NULL," ");
			strcpy(tiempo,p);
			p=strtok(fecha,"-");
			dia=atoi(p);
			p=strtok(NULL,"-");
			mes=atoi(p);
			p=strtok(NULL,"-");
			ano=atoi(p);
			p=strtok(tiempo,":");
			hora=atoi(p);
			p=strtok(NULL,":");
			minutos=atoi(p);
			p=strtok(NULL,":");
			segundos=atoi(p);
			int tiempoFecha=ano*365+mes*30+dia; 
			int tiempoDia=hora*3600+minutos*60+segundos;			
			if (tiempoFechaInicial<tiempoFecha && tiempoFecha<tiempoFechaFinal)
			{
				sprintf(historial, "%s/%s/%d/%s", historial,row[0],atoi(row[1]),row[2]);
				cont++;
			}
			else if (tiempoFechaInicial==tiempoFecha && tiempoFecha==tiempoFechaFinal)
			{
				if (tiempoDiaInicial<=tiempoDia && tiempoDia<=tiempoDiaFinal)
				{
					sprintf(historial, "%s/%s/%d/%s", historial,row[0],atoi(row[1]),row[2]);
					cont++;
				}
			}
			else if (tiempoFechaInicial==tiempoFecha)
			{
				if (tiempoDiaInicial<=tiempoDia)
				{
					sprintf(historial, "%s/%s/%d/%s", historial,row[0],atoi(row[1]),row[2]);
					cont++;
				}
			}
			else if (tiempoFechaFinal==tiempoFecha)
			{
				if (tiempoDia<=tiempoDiaFinal)
				{
					sprintf(historial, "%s/%s/%d/%s", historial,row[0],atoi(row[1]),row[2]);
					cont++;
				}
			}
			row = mysql_fetch_row (resultado);
		}
		if (cont==0)
		{
			strcpy(respuesta, "3/-2/No se han obtenido datos en la consulta.");
			return -2;			
		}
		else
		{
		sprintf(respuesta, "3/%d%s",cont,historial);
		return 0;
		}
	}
}

void *AtenderCliente(void *socket)
//Procedimiento que sirve para atender todas las peticiones de un cliente cuyo
//socket se recibe como parámetro
{
	int sock_conn;
	int *s;
	s = (int *) socket;
	sock_conn = *s;
	
	char peticion[512];
	char respuesta[512];
	char notificacion[512];
	char invitacion[512];
	int ret;
	int res;
	char nombre[20];
	
	
	int terminar = 0;
	while (terminar == 0)
	{
		memset(respuesta, 0, strlen(respuesta));
		// Ahora recibimos su nombre, que dejamos en buff
		ret=read(sock_conn,peticion, sizeof(peticion));
		printf ("Recibido\n");
		// Tenemos que a?adirle la marca de fin de string 
		// para que no escriba lo que hay despues en el buffer
		peticion[ret]='\0';
		
		//Escribimos el nombre en la consola
		
		printf ("Peticion: %s\n",peticion);
		char *p = strtok(peticion, "/");
		int codigo =  atoi (p);
		char password[20];
		char jugador1[20];
		char jugador2[20];
		int experiencia;
		int nivel;
		int experienciaEnNivel;
		int experienciaMaxima;
		char mazo[20];
		int idPartida;
		char invitados[100];
		int k;
		char respuesta_invitacion [10];
		char nombresVidaJugadores[100];
		int numForm;
		int idJugador;
		char mensaje [200];
		char conectados[512];
		int idCarta;
		
		if (codigo ==0) //Desconexion
		{
			terminar=1;
			pthread_mutex_lock(&mutex);
			Elimina(&listaConectados, nombre);
			Desconectarse(nombre);
			pthread_mutex_unlock(&mutex);
		}
		else if (codigo ==1) //Logueo
		{
			p = strtok( NULL, "/");
			strcpy (nombre, p);
			p = strtok( NULL, "/");
			strcpy (password, p);
			res = LogIn(nombre, password);
			if (res ==0)
			{
				pthread_mutex_lock(&mutex);
				Pon(&listaConectados, nombre, sock_conn);
				pthread_mutex_unlock(&mutex);
				experiencia=DameExperiencia(nombre);
				nivel=DameNivel(nombre);
				experienciaEnNivel=ExperienciaEnNivel(experiencia,nivel);
				experienciaMaxima=ExperienciaMaxima(nivel);
				Mazo(nombre,mazo);
				sprintf(respuesta, "1/%d/%d/%d/%d/%s", res,nivel,experienciaEnNivel,experienciaMaxima,mazo);
			}
			else
				sprintf(respuesta, "1/%d", res);
		}
		else if (codigo ==2) //Registro
		{
			p = strtok( NULL, "/");
			strcpy (nombre, p);
			p = strtok( NULL, "/");
			strcpy (password, p);
			pthread_mutex_lock(&mutex);
			res = Register(nombre, password);
			pthread_mutex_unlock(&mutex);
			sprintf(respuesta, "2/%d", res);
		}
		else if (codigo ==3) //Consulta historial
		{
			char historial[1000];
			Historial(nombre, historial);
			printf("Historial: %s\n",historial);
			write(sock_conn,historial,strlen(historial));
		}
		else if (codigo ==4) //Consulta partidas jugadas
		{
			res=PartidasJugadas(nombre);
			sprintf(respuesta, "4/%d", res);
		}
		else if (codigo ==5) //Consulta partidas ganadas
		{
			res=PartidasGanadas(nombre);
			sprintf(respuesta, "5/%d", res);
		}
		else if (codigo ==6) //Invitación partida
		{
			p = strtok(NULL, ",");
			strcpy(invitados, p);
			pthread_mutex_lock(&mutex);
			idPartida = PonPartida(tabla, &listaConectados, nombre, invitados);
			pthread_mutex_unlock(&mutex);
			if(idPartida==-1 || idPartida==-2)
			{
				sprintf(respuesta,"7/%d",idPartida);
				write(sock_conn,respuesta,strlen(respuesta));
			}
			else
			{				
				sprintf(notificacion, "8/%d/%s", idPartida, nombre);
				printf("Notificacion: %s\n", notificacion);
				k = 0;
				while(k<tabla[idPartida].numInvitados)
				{
					write(tabla[idPartida].jugadores[k+1].socket, notificacion, strlen(notificacion));
					k= k + 1;
				}
			}
		}
		else if (codigo ==7) //Respuesta a invitacion
		{
			p = strtok(NULL, "/");
			idPartida = atoi(p);
			p = strtok(NULL, "/");
			strcpy(respuesta_invitacion, p);
			if (strcmp(respuesta_invitacion, "SI")==0)
			{
				sprintf(invitacion, "9/%d/%s/%s", idPartida, nombre, respuesta_invitacion);			
			}
			else if (strcmp(respuesta_invitacion, "NO")==0)
			{
				p = strtok(NULL, "/");
				pthread_mutex_lock(&mutex);
				EliminarJugadorTabla(tabla, nombre, idPartida);
				pthread_mutex_unlock(&mutex);
				sprintf(invitacion, "9/%d/%s/%s/%d", idPartida, nombre, respuesta_invitacion,atoi(p));
			}
			printf("Respuesta a invitacion: %s\n", invitacion);
			write(tabla[idPartida].jugadores[0].socket, invitacion, strlen(invitacion));
			pthread_mutex_lock(&mutex);
			int numContestar = ContestarInvitacion(tabla, idPartida);
			pthread_mutex_unlock(&mutex);
			if (numContestar == tabla[idPartida].numInvitados)
			//El anfitrion de la partida no tiene que contestar
			{
				if (numContestar == tabla[idPartida].numJugadores-1)
				{
					res = PonerVida(tabla, idPartida);
					if (res!=-1 && res!=-2)
					{
						memset(nombresVidaJugadores,0,sizeof(nombresVidaJugadores));
						NombresVidaJugadores(tabla, idPartida, nombresVidaJugadores);
						k = 0;
						while(k<tabla[idPartida].numJugadores)
						{
							sprintf(notificacion, "10/%d/%d/%d/%s", idPartida, k, tabla[idPartida].numJugadores, nombresVidaJugadores);
							write(tabla[idPartida].jugadores[k].socket, notificacion, strlen(notificacion));
							k = k + 1;
						}
					}
				}
				else if (tabla[idPartida].numJugadores==1)
				{
					strcpy(notificacion, "11/");
					write(tabla[idPartida].jugadores[0].socket, notificacion, strlen(notificacion));
					pthread_mutex_lock(&mutex);
					EliminarPartida(tabla, idPartida);
					pthread_mutex_unlock(&mutex);
				}
				else
				{
					sprintf(notificacion, "12/%d/%d", idPartida, tabla[idPartida].numJugadores);
					write(tabla[idPartida].jugadores[0].socket, notificacion, strlen(notificacion));
				}
				printf("Notificacion: %s\n", notificacion);
			}
		}
		else if (codigo == 8) //Respuesta a empezar partida con menos jugadores de los invitados
		{
			p = strtok(NULL, "/");
			idPartida = atoi(p);
			p = strtok(NULL, "/");
			strcpy(respuesta_invitacion, p);
			if (strcmp(respuesta_invitacion, "SI")==0)
			{
				res = PonerVida(tabla, idPartida);
				if (res!=-1 && res!=-2)
				{
					memset(nombresVidaJugadores,0,sizeof(nombresVidaJugadores));
					NombresVidaJugadores(tabla, idPartida, nombresVidaJugadores);
					k = 0;
					while(k<tabla[idPartida].numJugadores)
					{
						sprintf(invitacion, "10/%d/%d/%d/%s", idPartida, k, tabla[idPartida].numJugadores, nombresVidaJugadores);
						write(tabla[idPartida].jugadores[k].socket, invitacion, strlen(invitacion));
						k = k + 1;
					}
				}
			}
			else if (strcmp(respuesta_invitacion, "NO")==0)
			{
				sprintf(invitacion, "13/");
				k = 1;
				while(k<tabla[idPartida].numJugadores)
				{
					write(tabla[idPartida].jugadores[k].socket, invitacion, strlen(invitacion));
					k = k + 1;
				}
				pthread_mutex_lock(&mutex);
				EliminarPartida(tabla,idPartida);
				pthread_mutex_unlock(&mutex);
			}
			printf("Respuesta a empezar partida: %s", invitacion);
		}
		else if (codigo == 9) //Chat
		{
			p = strtok(NULL, "/");
			idPartida = atoi(p);
			p = strtok(NULL, "/");
			strcpy(mensaje, p);
			k = 0;
			while(k<tabla[idPartida].numJugadores)
			{
				numForm=tabla[idPartida].numForm[k];
				sprintf(notificacion, "14/%d/%d/1/%s/%s", idPartida, numForm, nombre, mensaje);
				write(tabla[idPartida].jugadores[k].socket, notificacion, strlen(notificacion));
				k = k + 1;
			}
			printf("Chat: %s\n", notificacion);
			
		}
		else if (codigo==10) //Resultado partida o jugador
		{
			p = strtok(NULL, "/");
			idPartida = atoi(p);
			p = strtok(NULL, "/");
			int pos = atoi(p);
			AgregarHistorial(nombre,tabla[idPartida].partidaBBDD,pos);
			if (pos==1)
			{
				p=strtok(NULL,"/");
				char fecha[30];
				strcpy(fecha,p);
				p=strtok(NULL,"/");
				int duracion=atoi(p);
				ActualizarPartidaBBDD(tabla[idPartida].partidaBBDD,fecha,duracion,nombre);
			}
			pthread_mutex_lock(&mutex);
			experiencia=SubirExperiencia(nombre, pos);
			nivel=SubirNivel(nombre);
			pthread_mutex_unlock(&mutex);
			experienciaEnNivel=ExperienciaEnNivel(experiencia,nivel);
			experienciaMaxima=ExperienciaMaxima(nivel);
			sprintf(respuesta,"15/%d/%d/%d",nivel,experienciaEnNivel,experienciaMaxima);
			pthread_mutex_lock(&mutex);
			EliminarJugadorTabla(tabla,nombre,idPartida);
			pthread_mutex_unlock(&mutex);
			strcpy(mensaje," ha ABANDONADO la PARTIDA!");
			k = 0;
			while(k<tabla[idPartida].numJugadores)
			{
				numForm=tabla[idPartida].numForm[k];
				sprintf(notificacion, "14/%d/%d/2/%s/%s", idPartida, numForm, nombre, mensaje);
				write(tabla[idPartida].jugadores[k].socket, notificacion, strlen(notificacion));
				k = k + 1;
			}
			printf("Notificacion: %s\n",notificacion);
		}
		else if (codigo==11) //Guardar número formulario
		{
			p = strtok(NULL, "/");
			idPartida = atoi(p);
			p = strtok(NULL, "/");
			numForm = atoi(p);
			idJugador=DamePosicionJugador(tabla,nombre,idPartida);
			pthread_mutex_lock(&mutex);
			tabla[idPartida].numForm[idJugador]=numForm;
			pthread_mutex_unlock(&mutex);
		}
		else if (codigo==12) //Carta seleccionada
		{
			p = strtok(NULL, "/");
			idCarta = atoi(p);
			CartaSeleccionada(idCarta, respuesta);
		}
		else if (codigo==13) //Confirmar mazo
		{
			p=strtok(NULL,"");
			strcpy(mazo,p);
			pthread_mutex_lock(&mutex);
			res=EliminarMazo(nombre);
			if (res==0)
			{
				res=AceptarMazo(nombre,mazo);
			}
			pthread_mutex_unlock(&mutex);
			sprintf(respuesta,"18/%d",res);
		}
		else if (codigo==14) //Eliminar mazo
		{
			pthread_mutex_lock(&mutex);
			res=EliminarMazo(nombre);
			pthread_mutex_unlock(&mutex);
			sprintf(respuesta,"19/%d",res);
		}
		else if (codigo==15) //Enviar mazo
		{
			Mazo(nombre, mazo);
			sprintf(respuesta,"16/%s",mazo);			
		}
		else if (codigo==16) //Jugada partida
		{
			p=strtok(NULL,"/");
			idPartida=atoi(p);
			idJugador=DamePosicionJugador(tabla,nombre,idPartida);
			if (tabla[idPartida].ataque==idJugador)
			{
				p=strtok(NULL,"/");
				pthread_mutex_lock(&mutex);
				tabla[idPartida].idCartaAtacante=atoi(p);
				tabla[idPartida].cartas++;
				pthread_mutex_unlock(&mutex);
			}
			else if (tabla[idPartida].defensa==idJugador)
			{
				p=strtok(NULL,"/");
				pthread_mutex_lock(&mutex);
				tabla[idPartida].idCartaDefensora=atoi(p);
				tabla[idPartida].cartas++;
				pthread_mutex_unlock(&mutex);
			}
			if (tabla[idPartida].cartas==2)
			{
				int idDefensor=tabla[idPartida].defensa;
				pthread_mutex_lock(&mutex);
				int vidaRestante=ActualizarVida(tabla,idDefensor,idPartida,tabla[idPartida].idCartaAtacante,tabla[idPartida].idCartaDefensora);
				if (vidaRestante!=0)
				{
					CambioTurno(tabla,idPartida);
				}
				pthread_mutex_unlock(&mutex);
				int idAtacante=tabla[idPartida].ataque;
				idDefensor=tabla[idPartida].defensa;
				k = 0;
				while(k<tabla[idPartida].numJugadores)
				{
					numForm=tabla[idPartida].numForm[k];
					sprintf(notificacion,"20/%d/%d/%d/%d/%d/%d/%d",idPartida,numForm,tabla[idPartida].idCartaAtacante,tabla[idPartida].idCartaDefensora,vidaRestante,idAtacante,idDefensor);
					write(tabla[idPartida].jugadores[k].socket, notificacion, strlen(notificacion));
					k = k + 1;
				}
				printf("Jugada: %s\n",notificacion);
			}
		}
		else if (codigo==17) //Cerrar partida/morir
		{
			p = strtok(NULL, "/");
			idPartida = atoi(p);
			p = strtok(NULL, "/");
			int desconectado = atoi(p);
			idJugador=DamePosicionJugador(tabla,nombre,idPartida);
			pthread_mutex_lock(&mutex);
			tabla[idPartida].cartas=0;
			if (desconectado==-1)
			{
				EliminarJugadorTabla(tabla,nombre,idPartida);
				printf("Cerrar partida en la mitad: 22/\n");
				write(sock_conn,"22/",strlen("22/"));
			}
			if (desconectado==0)
			{
				EliminarJugadorMuerto(tabla,idJugador,idPartida);
			}
			if (tabla[idPartida].ataque==idJugador)
			{
				if (tabla[idPartida].numJugadoresVivos==3)
				{
					if (idJugador==3)
					{
						tabla[idPartida].ataque=0;
						tabla[idPartida].defensa=1;
					}
					else if (idJugador==2)
					{
						tabla[idPartida].ataque=2;
						tabla[idPartida].defensa=0;
					}
					else if (idJugador==1)
					{						
						tabla[idPartida].ataque=1;
						tabla[idPartida].defensa=2;
					}
					else if (idJugador==0)
					{
						tabla[idPartida].ataque=0;
						tabla[idPartida].defensa=1;
					}
				}
				else if (tabla[idPartida].numJugadoresVivos==2)
				{
					if (idJugador==2)
					{
						tabla[idPartida].ataque=0;
						tabla[idPartida].defensa=1;
					}
					else if (idJugador==1)
					{						
						tabla[idPartida].ataque=1;
						tabla[idPartida].defensa=0;
					}
					else if (idJugador==0)
					{
						tabla[idPartida].ataque=0;
						tabla[idPartida].defensa=1;
					}
				}
			}
			else if (tabla[idPartida].defensa==idJugador)
			{
				if (tabla[idPartida].numJugadoresVivos==3)
				{
					if (idJugador==3)
					{
						tabla[idPartida].ataque=2;
						tabla[idPartida].defensa=0;
					}
					else if (idJugador==2)
					{
						tabla[idPartida].ataque=1;
						tabla[idPartida].defensa=2;
					}
					else if (idJugador==1)
					{						
						tabla[idPartida].ataque=0;
						tabla[idPartida].defensa=1;
					}
					else if (idJugador==0)
					{
						tabla[idPartida].ataque=2;
						tabla[idPartida].defensa=0;
					}
				}
				else if (tabla[idPartida].numJugadoresVivos==2)
				{
					if (idJugador==2)
					{
						tabla[idPartida].ataque=1;
						tabla[idPartida].defensa=0;
					}
					else if (idJugador==1)
					{						
						tabla[idPartida].ataque=0;
						tabla[idPartida].defensa=1;
					}
					else if (idJugador==0)
					{
						tabla[idPartida].ataque=1;
						tabla[idPartida].defensa=0;
					}
				}
			}
			else
			{
				if (tabla[idPartida].numJugadoresVivos==3)
				{
					if (tabla[idPartida].ataque>2)
					{
						tabla[idPartida].ataque=2;
						tabla[idPartida].defensa=0;
					}
					else if (tabla[idPartida].defensa>2)
					{
						tabla[idPartida].ataque=1;
						tabla[idPartida].defensa=2;
					}
				}
				else if (tabla[idPartida].numJugadoresVivos==2)
				{
					if (tabla[idPartida].ataque>1)
					{
						tabla[idPartida].ataque=1;
						tabla[idPartida].defensa=0;
					}
					else if (tabla[idPartida].defensa>1)
					{
						tabla[idPartida].ataque=0;
						tabla[idPartida].defensa=1;
					}
				}
			}
			pthread_mutex_unlock(&mutex);
			memset(nombresVidaJugadores,0,sizeof(nombresVidaJugadores));
			NombresVidaJugadores(tabla, idPartida, nombresVidaJugadores);
			k = 0;
			while(k<tabla[idPartida].numJugadores)
			{
				numForm=tabla[idPartida].numForm[k];
				sprintf(notificacion, "21/%d/%d/%d/%s/%d/%d/%d/%s", idPartida, numForm, desconectado, nombre, tabla[idPartida].ataque, tabla[idPartida].defensa, tabla[idPartida].numJugadoresVivos, nombresVidaJugadores);
				write(tabla[idPartida].jugadores[k].socket, notificacion, strlen(notificacion));
				k = k + 1;
			}
			printf("Notificacion: %s\n",notificacion);
		}
		else if (codigo==18) //Consulta jugador mas experiencia
		{
			JugadorMasExperiencia(respuesta);
		}
		else if (codigo==19) //Consulta jugador mas partidas ganadas
		{
			JugadorMasGanadas(respuesta);
		}
		else if (codigo==20) //Consulta experiencia
		{
			experiencia=DameExperiencia(nombre);
			sprintf(respuesta,"25/%d",experiencia);
		}
		else if (codigo==21) //Consulta carta seleccionada en tablero
		{
			p = strtok(NULL, "/");
			idPartida = atoi(p);
			p = strtok(NULL, "/");
			idCarta = atoi(p);
			idJugador=DamePosicionJugador(tabla,nombre,idPartida);
			numForm=tabla[idPartida].numForm[idJugador];
			CartaSeleccionadaTablero(idCarta, mensaje);
			sprintf(respuesta,"26/%d/%d/%s",idPartida,numForm,mensaje);
		}
		else if (codigo==22) //Darse de baja
		{
			p = strtok( NULL, "/");
			strcpy (nombre, p);
			p = strtok( NULL, "/");
			strcpy (password, p);
			pthread_mutex_lock(&mutex);
			res=DarBaja(nombre,password);
			pthread_mutex_unlock(&mutex);
			sprintf(respuesta,"27/%d",res);
		}
		else if (codigo==23) //Consulta nombres adversarios
		{
			Adversarios(nombre,respuesta);
		}
		else if (codigo==24) //Consulta partidas contra adversarios
		{
			char adversarios[512];
			p=strtok(NULL," ");
			strcpy(adversarios,p);
			ResultadosJugadores(nombre,adversarios,respuesta);
		}
		else if (codigo==25) //Consulta partidas en un periodo de tiempo
		{
			p=strtok(NULL,"");
			PartidasJugadasTiempo(nombre,p,respuesta);
		}
		if (codigo != 0 && codigo!=3 && codigo != 6 && codigo !=7 && codigo !=8 && codigo !=9 && codigo!=16 && codigo!=17)
		{
			printf ("Respuesta: %s\n", respuesta);
			write(sock_conn,respuesta, strlen(respuesta));
		}
		if ((codigo ==1 && res ==0) || codigo ==0)
		{
			pthread_mutex_lock(&mutex);
			DameConectados(&listaConectados, conectados);
			pthread_mutex_unlock(&mutex);
			sprintf(notificacion,"6/%s",conectados);
			printf("Notificacion: %s\n",notificacion);
			for(k=0;k<listaConectados.num;k++)
				write(listaConectados.conectados[k].socket,notificacion,strlen(notificacion));
		}
	}
	// Se acabo el servicio para este cliente
	close(sock_conn);
}

int main(int argc, char *argv[])
//Programa principal en el que se crean los threads de todos los usuarios que se conectan correctamente al servidor
{
	int sock_conn, sock_listen;
	struct sockaddr_in serv_adr;
	// INICIALITZACIONS
	// Obrim el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket");
	// Fem el bind al port
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	
	// asocia el socket a cualquiera de las IP de la maquina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// escucharemos en el port 50008
	serv_adr.sin_port = htons(50008);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	//La cola de peticiones pendientes no podra ser superior a 4
	if (listen(sock_listen, 3) < 0)
		printf("Error en el Listen");
	
	int er = AbrirBaseDatos();
	InicializarTabla(tabla);
	DesconectarJugadores();
	
	pthread_t thread[100];
	
	for(;;){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexion\n");
		
		sockets[i] = sock_conn;
		//sock_conn es el socket que usaremos para este cliente
		
		// bucle de atencion al cliente
		pthread_create(&thread[i], NULL, AtenderCliente, &sockets[i]);
		i = i + 1;
	}
	CerrarBaseDatos();
}
