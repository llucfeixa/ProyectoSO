#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <ctype.h>
#include <mysql.h>

MYSQL *conn;
int err;
MYSQL_RES *resultado;
MYSQL_ROW row;

int AbrirBaseDatos()
{
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1);
		return -1;
	}
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "Juego",0, NULL, 0);
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

int LogIn(char usuario[20], char password[20], char respuesta[512])
{
	char consulta[200];
	sprintf(consulta, "SELECT * FROM Jugador WHERE nombre='%s' AND contraseña='%s'", usuario, password);
	err=mysql_query (conn, consulta);
	if (err!=0) {
		strcpy(respuesta, "Error al consultar datos de la base.");
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		strcpy(respuesta, "No es posible loguearse. Intentelo de nuevo.");
		return -2;
	}
	else
	{
		strcpy(respuesta, "Logueado correctamente.");
		return 0;
	}
}

int Register(char usuario[20], char password[20], char respuesta[512])
{
	char consulta[200];
	strcpy(consulta, "SELECT MAX(id) FROM Jugador");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		strcpy(respuesta, "Error al consultar datos de la base.");
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		strcpy(respuesta, "Error al insertar datos en la base.");
		return -2;
	}
	else
	{
		int id = atoi(row[0])+1;
		char consulta2[200];
		sprintf(consulta2, "INSERT INTO Jugador VALUES(%d,'%s','%s')", id, usuario, password);
		err=mysql_query (conn, consulta2);
		if (err!=0) {
			strcpy(respuesta, "Error al insertar datos en la base.");
			return -1;
			exit (1);
		}
		else
		{
			strcpy(respuesta, "Registrado correctamente.");
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
	if (row == NULL){
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
		strcpy(respuesta, "Error al consultar datos de la base.");
		return -1;
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		strcpy(respuesta, "No se han obtenido datos en la consulta.");
		return -2;
	}
	else
	{
		while (row !=NULL) {
			printf("%s\n",row[0]);
			sprintf(respuesta, "%s%s,", respuesta,row[0]);
			row = mysql_fetch_row (resultado);
		}
		respuesta[strlen(respuesta)-1] = '\0';
		return 0;
	}
}

int main(int argc, char *argv[])
{
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char peticion[512];
	char respuesta[512];
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
	serv_adr.sin_port = htons(9060);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 3) < 0)
		printf("Error en el Listen");
	
	for(;;){
		printf ("Escuchando\n");
		
		int er = AbrirBaseDatos();
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexion\n");
		//sock_conn es el socket que usaremos para este cliente
		
		// bucle de atencion al cliente
		int terminar = 0;
		while (terminar == 0)
		{
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
				printf ("Codigo: %d, jugador1: %s\n", codigo, jugador1);
			}
			
			if (codigo ==0)
				terminar =1;
			else if (codigo ==1)
			{
				p = strtok( NULL, "/");
				strcpy (password, p);
				LogIn(jugador1, password, respuesta);
			}
			else if (codigo ==2)
			{
				p = strtok( NULL, "/");
				strcpy (password, p);
				Register(jugador1, password, respuesta);
			}
			else if (codigo ==3)
			{
				p = strtok( NULL, "/");
				strcpy (jugador2, p);
				int veces = Enfrentamientos(jugador1, jugador2);
				sprintf(respuesta, "%d", veces);
			}
			else if (codigo ==4)
			{
				int puntos = PuntosObtenidos(jugador1);
				sprintf(respuesta, "%d", puntos);
			}
			else
			{
				GanarNombre(jugador1,respuesta);
			}
			if (codigo != 0)
			{
				printf ("%s\n", respuesta);
				// Y lo enviamos
				write (sock_conn,respuesta, strlen(respuesta));
			}
		}
		// Se acabo el servicio para este cliente
		close(sock_conn); 
	}
	CerrarBaseDatos();
}
