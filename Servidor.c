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
}Partida;

typedef Partida TablaPartidas[MAXPartidas];

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
	sprintf(conectados, "6/%d", lista->num);
	int i;
	for(i=0;i<lista->num;i++)
		sprintf(conectados, "%s/%s", conectados, lista->conectados[i].nombre);
}

void InicializarTabla(TablaPartidas tabla)
//Pone todas las partidas de la tabla de partidas con el campo libre, numJugadores y confirmar a 0, para poder ser usadas
{
	while(i<MAXPartidas)
	{
		tabla[i].libre=0;
		tabla[i].numJugadores=0;
		tabla[i].numInvitados=0;
		tabla[i].contestar=0;
		i=i+1;
	}
}

int PonPartida(TablaPartidas tabla, ListaConectados *lista, char nombre[20], char invitados[100])
//Añade en una partida a los usuarios que van a jugar. Para ello, tiene el nombre de quien invita
//y los usuarios invitados separados por "/". Retorna la posición de la partida en la tabla o -1
//si no habia ninguna libre
{
	int i=0;
	int encontrado=0;
	while(i<MAXPartidas && !encontrado)
	{
		if(tabla[i].libre == 0)
		{
			strcpy(tabla[i].jugadores[0].nombre, nombre);
			tabla[i].jugadores[0].socket=DameSocket(lista, nombre);
			tabla[i].numJugadores++;
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
				p = strtok(NULL, "/");
				j++;
			}
			tabla[i].libre = 1;
			encontrado = 1;
		}
		if (!encontrado)
		{
			i++;
		}
	}
	if (encontrado)
		return i;
	if (!encontrado)
		return -1;
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

int EliminarJugadorTabla(TablaPartidas tabla, char nombre[20], int id)
//Elimina a un jugador de la tabla después de que haya rechazado la invitacion a una partida
//a partir de su nombre y la id de la partida, es decir, la posición en la tabla. Retorna 0
//si OK o -1 si no ha encontrado ese jugador en la partida
{
	int pos = DamePosicionJugador(tabla, nombre, id);
	if(pos == -1)
		return -1;
	else
	{
		int i;
		for(i=pos;i<tabla[id].numJugadores-1;i++)
			tabla[id].jugadores[i] = tabla[id].jugadores[i+1];
		tabla[id].numJugadores--;
		return 0;	
	}
}

int ContestarInvitacion(TablaPartidas tabla, int id)
//Retorna el número de personas que han contestado a la invitación de una partida
{
	tabla[id].contestar++;
	return tabla[id].contestar;
}

void EliminarPartida(TablaPartidas tabla, int id)
//Elimina una partida de la tabla a partir de su id, recibida como parámetro
{
	tabla[id].libre=0;
	tabla[id].numJugadores=0;
	tabla[id].numInvitados=0;
	tabla[id].contestar=0;
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
{
	char consulta[200];
	sprintf(consulta, "SELECT * FROM Jugador WHERE nombre='%s' AND contraseña='%s'", usuario, password);
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
		return 0;
	}
}

int Register(char usuario[20], char password[20])
//Retorna 0 si el usuario se ha registrado correctamente a partir de un usuario y una contraseña, -1 si ha
//habido algún error al consultar la base de datos o -2 si ha habido un error al insertarlo en la base
{
	char consulta[200];
	strcpy(consulta, "SELECT MAX(id) FROM Jugador");
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
		int id = atoi(row[0])+1;
		char consulta2[200];
		sprintf(consulta2, "INSERT INTO Jugador VALUES(%d,'%s','%s')", id, usuario, password);
		err=mysql_query (conn, consulta2);
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

int Enfrentamientos(char jugador1[20], char jugador2[20])
//Retorna las veces que dos jugadores cuyos nombres recibe como parámetro se han enfrentado, -1 si ha
//habido algún error al consultar la base de datos o -2 si no ha obtenido ningún resultado de la base
{
	char consulta[300];
	strcpy (consulta,"SELECT Historial.idP FROM Jugador,Historial WHERE Jugador.nombre='");
	strcat (consulta, jugador1);
	strcat (consulta,"'AND Jugador.id=Historial.idJ AND Historial.idP IN (SELECT Historial.idP FROM Jugador,Historial WHERE Jugador.nombre='");
	strcat (consulta, jugador2);
	strcat (consulta,"' AND Jugador.id=Historial.idJ)");
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
		int cont = 0;
		while (row !=NULL) {
			cont = cont + 1;
			row = mysql_fetch_row (resultado);
		}
		return cont;
	}
}

int PuntosObtenidos(char nombre[20])
//Retorna el número total de puntos que ha obtenido un jugador cuyo nombre recibe como parámetro,
//-1 si ha habido algún error al consultar la base de datos o -2 si no ha obtenido ningún resultado de la base
{
	char consulta[200];
	strcpy (consulta,"SELECT SUM(Historial.Puntos) FROM Jugador,Historial WHERE Jugador.nombre='");
	strcat (consulta,nombre);
	strcat (consulta,"'AND Jugador.id=Historial.idJ");
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

int GanarNombre(char nombre[20], char respuesta[512])
//Añade en respuesta los nombres de los jugadores que han ganado una partida en la que estaba un determinado jugador cuyo nombre se recibe
//como parámetro. Retorna 0 si OK, -1 si ha habido algún error al consultar la base de datos o -2 si no ha obtenido ningún resultado de la base
{
	memset(respuesta, 0, strlen(respuesta));
	char consulta[200];
	strcpy (consulta,"SELECT DISTINCT Partida.ganador FROM Jugador,Partida,Historial WHERE Jugador.nombre = '");
	strcat (consulta, nombre);
	strcat (consulta,"'AND Jugador.id = Historial.idJ AND Historial.idP = Partida.id AND Jugador.nombre!=Partida.ganador");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		strcpy(respuesta, "5/Error al consultar datos de la base.");
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		strcpy(respuesta, "5/No se han obtenido datos en la consulta.");
		return -2;
	}
	else
	{
		strcpy(respuesta, "5/");
		while (row !=NULL) {
			printf("%s\n",row[0]);
			sprintf(respuesta, "%s%s,", respuesta,row[0]);
			row = mysql_fetch_row (resultado);
		}
		respuesta[strlen(respuesta)-1] = '\0';
		return 0;
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
	int ret;
	int res;
	
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
		char jugador1[20];
		char jugador2[20];
		char password[20];
		int numForm;
		
		if (codigo != 0 && codigo != 6 && codigo != 7 && codigo != 8 && codigo != 9)
		{
			p = strtok( NULL, "/");
			strcpy (jugador1, p);
		}
		
		if (codigo ==0)
		{
			terminar =1;
			pthread_mutex_lock(&mutex);
			Elimina(&listaConectados, jugador1);
			pthread_mutex_unlock(&mutex);
		}
		else if (codigo ==1)
		{
			p = strtok( NULL, "/");
			strcpy (password, p);
			printf ("Codigo: %d, jugador1: %s, password: %s\n", codigo, jugador1, password);
			res = LogIn(jugador1, password);
			sprintf(respuesta, "1/%d", res);
			if (res ==0)
			{
				pthread_mutex_lock(&mutex);
				Pon(&listaConectados, jugador1, sock_conn);
				pthread_mutex_unlock(&mutex);
			}
		}
		else if (codigo ==2)
		{
			p = strtok( NULL, "/");
			strcpy (password, p);
			printf ("Codigo: %d, jugador1: %s, password: %s\n", codigo, jugador1, password);
			res = Register(jugador1, password);
			sprintf(respuesta, "2/%d", res);
		}
		else if (codigo ==3)
		{
			p = strtok( NULL, "/");
			strcpy (jugador2, p);
			printf ("Codigo: %d, jugador1: %s, jugador2: %s\n", codigo, jugador1, jugador2);
			res = Enfrentamientos(jugador1, jugador2);
			sprintf(respuesta, "3/%d", res);
		}
		else if (codigo ==4)
		{
			printf ("Codigo: %d, jugador1: %s\n", codigo, jugador1);
			int res = PuntosObtenidos(jugador1);
			sprintf(respuesta, "4/%d", res);
		}
		else if (codigo ==5)
		{
			printf ("Codigo: %d, jugador1: %s\n", codigo, jugador1);
			GanarNombre(jugador1, respuesta);
		}
		else if (codigo ==6)
		{
			p = strtok(NULL, ",");
			char invitados[100];
			strcpy(invitados, p);
			printf("%s\n",invitados);
			char nombre[20];
			DameNombre(&listaConectados, sock_conn, nombre);
			pthread_mutex_lock(&mutex);
			int pos = PonPartida(tabla, &listaConectados, nombre, invitados);
			pthread_mutex_unlock(&mutex);
			if(pos == -1)
			{
				printf("7/\n");
				write(sock_conn, "7/", strlen("7/"));
			}
			else
			{
				char invitacion[100];
				sprintf(invitacion, "8/%d/%s", pos, nombre);
				printf("%s\n", invitacion);
				int k = 0;
				while(k<tabla[pos].numInvitados)
				{
					write(tabla[pos].jugadores[k+1].socket, invitacion, strlen(invitacion));
					k = k + 1;
				}	
			}
		}
		else if (codigo ==7)
		{
			p = strtok(NULL, "/");
			int pos = atoi(p);
			p = strtok(NULL, "/");
			char respuesta_invitacion [10];
			strcpy(respuesta_invitacion, p);
			char invitacion[100];
			char nombre[20];
			DameNombre(&listaConectados, sock_conn, nombre);
			if (strcmp(respuesta_invitacion, "SI")==0)
			{
				sprintf(invitacion, "9/%d/%s/%s", pos, nombre, respuesta_invitacion);
			}
			else if (strcmp(respuesta_invitacion, "NO")==0)
			{
				EliminarJugadorTabla(tabla, nombre, pos);
				sprintf(invitacion, "9/%d/%s/%s", pos, nombre, respuesta_invitacion);
			}
			printf("%s\n", invitacion);
			write(tabla[pos].jugadores[0].socket, invitacion, strlen(invitacion));
			char notificacion[100];
			int numContestar = ContestarInvitacion(tabla, pos);
			if (numContestar == tabla[pos].numInvitados)
			//El anfitrion de la partida no tiene que contestar
			{
				if (numContestar == tabla[pos].numJugadores-1)
				{
					sprintf(notificacion, "10/%d", pos);
					int k = 0;
					while(k<tabla[pos].numJugadores)
					{
						write(tabla[pos].jugadores[k].socket, notificacion, strlen(notificacion));
						k = k + 1;
					}
				}
				else if (tabla[pos].numJugadores==1)
				{
					strcpy(notificacion, "11/");
					write(tabla[pos].jugadores[0].socket, notificacion, strlen(notificacion));
					EliminarPartida(tabla, pos);
				}
				else
				{
					sprintf(notificacion, "12/%d/%d", pos, tabla[pos].numJugadores);
					write(tabla[pos].jugadores[0].socket, notificacion, strlen(notificacion));
				}
				printf("%s\n", notificacion);
			}
		}
		else if (codigo == 8)
		{
			p = strtok(NULL, "/");
			int pos = atoi(p);
			p = strtok(NULL, "/");
			char respuesta_invitacion [10];
			strcpy(respuesta_invitacion, p);
			char invitacion[100];
			char nombre[20];
			DameNombre(&listaConectados, sock_conn, nombre);
			if (strcmp(respuesta_invitacion, "SI")==0)
			{
				sprintf(invitacion, "10/%d", pos);
				int k = 0;
				while(k<tabla[pos].numJugadores)
				{
					write(tabla[pos].jugadores[k].socket, invitacion, strlen(invitacion));
					k = k + 1;
				}
			}
			else if (strcmp(respuesta_invitacion, "NO")==0)
			{
				sprintf(invitacion, "13/");
				int k = 1;
				while(k<tabla[pos].numJugadores)
				{
					write(tabla[pos].jugadores[k].socket, invitacion, strlen(invitacion));
					k = k + 1;
				}
			}
			printf("%s", invitacion);
		}
		else if (codigo == 9)
		{
			p = strtok(NULL, "/");
			int pos = atoi(p);
			p = strtok(NULL, "/");
			numForm = atoi(p);
			p = strtok(NULL, "/");
			char mensaje [200];
			strcpy(mensaje, p);
			char chat[200];
			char nombre[20];
			DameNombre(&listaConectados, sock_conn, nombre);
			sprintf(chat, "14/%d/%d/%s/%s", pos, numForm, nombre, mensaje);
			int k = 0;
			while(k<tabla[pos].numJugadores)
			{
				write(tabla[pos].jugadores[k].socket, chat, strlen(chat));
				k = k + 1;
			}
			printf("%s", chat);
			
		}
		if (codigo != 0 && codigo != 6 && codigo !=7 && codigo !=8 && codigo !=9)
		{
			printf ("%s\n", respuesta);
			// Y lo enviamos
			write(sock_conn,respuesta, strlen(respuesta));
		}
		if ((codigo ==1 && res ==0) || codigo ==0)
		{	
			char notificacion[100];		
			pthread_mutex_lock(&mutex);
			DameConectados(&listaConectados, notificacion);
			pthread_mutex_unlock(&mutex);
			printf("%s\n",notificacion);
			int j;
			for(j=0;j<listaConectados.num;j++)
				write(listaConectados.conectados[j].socket,notificacion,strlen(notificacion));
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
