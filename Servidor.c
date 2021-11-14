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

pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;

int i;
int sockets[100];

typedef struct{
	char nombre[20];
	int socket;
}Conectado;

typedef struct{
	Conectado conectados[100];
	int num;
}ListaConectados;

int Pon(ListaConectados *lista, char nombre[20], int socket)
{
	//Añade nuevo conectado. Retorna 0 si OK y -1 si la lista esta llena y no se ha podido añadir.
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
{
	//Devuelve la posicion o -1 si no está en la lista
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

int Elimina(ListaConectados *lista, char nombre[20])
{
	//Retorna 0 si elimina y -1 si ese usuario no esta en la lista
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
{
	//Pone en conectados los nombres de todos los conectados separados por /
	//Primero pone el número de conectados. Ejemplo: "3/Juan/Maria/Pedro"	
	sprintf(conectados, "6/%d", lista->num);
	int i;
	for(i=0;i<lista->num;i++)
		sprintf(conectados, "%s/%s", conectados, lista->conectados[i].nombre);
}

MYSQL *conn;
int err;
MYSQL_RES *resultado;
MYSQL_ROW row;
ListaConectados listaConectados;

int AbrirBaseDatos()
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
{
	mysql_close (conn);
	exit(0);
}

int LogIn(char usuario[20], char password[20])
{
	char consulta[200];
	sprintf(consulta, "SELECT * FROM Jugador WHERE nombre='%s' AND contraseña='%s'", usuario, password);
	err=mysql_query (conn, consulta);
	if (err!=0) {
		//Error al consultar datos de la base.
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		//No es posible loguearse. Intentelo de nuevo.
		return -2;
	}
	else
	{
		//Logueado correctamente.
		return 0;
	}
}

int Register(char usuario[20], char password[20])
{
	char consulta[200];
	strcpy(consulta, "SELECT MAX(id) FROM Jugador");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		//Error al consultar datos de la base.
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		//Error al insertar datos en la base.
		return -2;
	}
	else
	{
		int id = atoi(row[0])+1;
		char consulta2[200];
		sprintf(consulta2, "INSERT INTO Jugador VALUES(%d,'%s','%s')", id, usuario, password);
		err=mysql_query (conn, consulta2);
		if (err!=0) {
			//Error al insertar datos en la base.
			return -1;
			exit (1);
		}
		else
		{
			//Registrado correctamente.
			return 0;
		}
	}
}

int Enfrentamientos(char jugador1[20], char jugador2[20])
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
		char *p = strtok( peticion, "/");
		int codigo =  atoi (p);
		char jugador1[20];
		char jugador2[20];
		char password[20];
		
		if (codigo != 0)
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
		if (codigo != 0)
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
	
	// asocia el socket a cualquiera de las IP de la m?quina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// escucharemos en el port 9050
	serv_adr.sin_port = htons(50008);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 3) < 0)
		printf("Error en el Listen");
	
	int er = AbrirBaseDatos();
	
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
