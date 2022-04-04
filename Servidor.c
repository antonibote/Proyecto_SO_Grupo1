#include <mysql.h>
#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
MYSQL *conn;

void Login(char contra[100], char nombre[100], char respuesta[100],int socket){
	
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[100];
	
	strcpy (consulta,"Select jugador.ID FROM (jugador) WHERE jugador.Username= '"); 
	strcat (consulta, nombre);
	strcat (consulta,"'");
	strcat (consulta," AND jugador.Pass='");
	strcat (consulta, contra);
	strcat (consulta,"'");
	// hacemos la consulta 
	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn); 
	row = mysql_fetch_row (resultado);
	if (row == NULL)
		sprintf(respuesta,"NO");
	else{
		sprintf(respuesta,"%s",row[0]);
	}
}


//Funcion para Resgistrar un nuevo usuario que no existe

void Registrar(char contra[100], char nombre[100], char respuesta[100])
{
	
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[100];
	
	strcpy (consulta,"Select jugador.Username,jugador.Pass FROM (jugador) WHERE jugador.Username= '"); 
	strcat (consulta, nombre);
	strcat (consulta,"'");
	
	
	// Hacemos la consulta 
	err = mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	//Recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn); 
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		char select[100];
		strcpy(select,"Select MAX(jugador.ID) FROM (jugador)");
		err=mysql_query (conn, select); 
		if (err!=0) {
			printf ("Error al consultar datos de la base %u %s\n",
					mysql_errno(conn), mysql_error(conn));
			exit (1);
		}
		//recogemos el resultado de la consulta 
		resultado = mysql_store_result (conn); 
		row = mysql_fetch_row (resultado);
		int ID_jugador = atoi(row[0])+1;
		char insert[100];
		strcpy (insert,"Insert INTO jugador(ID,Username,Pass) values(");
		sprintf(insert,"%s%d",insert,ID_jugador);
		strcat (insert,",'");
		strcat (insert, nombre);
		strcat (insert,"','");
		strcat (insert, contra);
		strcat (insert,"')");
	
		err=mysql_query (conn, insert); 
		if (err!=0) {
			printf ("Error al insertar datos de la base %u %s\n",
					mysql_errno(conn), mysql_error(conn));
			exit (1);
			sprintf(respuesta,"NO");
		}
		else 
			sprintf(respuesta,"SI");
		
	}
	else{
		sprintf(respuesta,"NO");
	}
}	

int main(int argc, char *argv[])
{
	
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char peticion[512];
	char respuesta[512];
	int err;
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
	// establecemos el puerto de escucha
	serv_adr.sin_port = htons(9050);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	
	if (listen(sock_listen, 5) < 0)
		printf("Error en el Listen");
	
	int i;
	// Bucle infinito
	for (i=0;i<100;i++){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexion\n");
		//sock_conn es el socket que usaremos para este cliente
		MYSQL *conn;
		int err;
		conn = mysql_init(NULL);
		int terminar =0;
		// Entramos en un bucle para atender todas las peticiones de este cliente
		//hasta que se desconecte
		while (terminar ==0)
		{
			// Ahora recibimos la petici?n
			ret=read(sock_conn,peticion, sizeof(peticion));
			printf ("Recibido\n");
			
			// Tenemos que anadirle la marca de fin de string 
			// para que no escriba lo que hay despues en el buffer
			peticion[ret]='\0';
			
			//Escribimos el nombre en la consola
			printf ("Peticion: %s\n",peticion);
			
			// vamos a ver que quieren
			char *p = strtok( peticion, "/");
			int codigo =  atoi (p);
			p = strtok(NULL,"/");
			char nombre[20];
			strcpy (nombre, p);
			printf ("Codigo: %d, Nombre: %s\n", codigo, nombre);
			// Ya tenemos el codigo de la peticion
			
			if (codigo !=0)
			{
				p = strtok( NULL, "/");
				strcpy (nombre, p);
				
				// Ya tenemos el nombre
				printf ("Codigo: %d, Nombre: %s\n", codigo, nombre);
			}
			
			if (codigo ==0) //peticion de desconexion
				terminar=1;
			
			else if (codigo ==1) //piden username del ganador y duracion partida jugada tal dia
			{
				p = strtok (NULL, "/");
				char fecha[20];
				// Pregunto los nombres de los dos jugadores
				printf ("Dame la fecha a consultar en formato: dd-mm-aaaa \n"); 
				scanf ("%s", fecha);
				
				char consulta [80];
				strcpy (consulta,"SELECT jugador.Username, partida.Duracion FROM jugador,partida,relacion WHERE partida.Fecha = '");
				strcat (consulta, fecha);
				strcat (consulta,"'AND partida.ID = relacion.ID_P AND relacion.ID_J = jugador.ID");								//=%s", fecha);?
				
				err=mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al consultar datos de la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
			}
			
			
			else if (codigo ==2)  // piden partidas ganadas de tal username
			{
				// Pregunto los nombre de los dos jugadores
				printf ("Dame el nombre de un jugador\n"); 
				scanf ("%s", nombre);
				
				char consulta [80];
				strcpy (consulta,"SELECT relacion.Victorias FROM jugador,partida,relacion WHERE jugador.Username = %s'");
				strcat (consulta, nombre);
				strcat (consulta,"'AND jugador.ID = Relacion.ID_J AND relacion.ID_P = partida.ID");
				
				err=mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al consultar datos de la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
			}
			
			
			else if (codigo ==3) //piden cantidad partidas jugadas de tal username
				{
				// Pregunto los nombre de los dos jugadores
				printf ("Dame el nombre de un jugador\n"); 
				scanf ("%s", nombre);
				
				char consulta [80];
				strcpy (consulta,"SELECT relacion.Cantidad FROM jugador, partida, relacion WHERE jugador.Username = '");
				strcat (consulta, nombre);
				strcat (consulta,"partida.ID = relacion.ID_P AND relacion.ID_J = jugador.ID");
				
				err=mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al consultar datos de la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
					}
				}
			else if (codigo ==4) //piden id de tal username
				{
				// Pregunto los nombre de los dos jugadores
				printf ("Dame el nombre de un jugador\n"); 
				scanf ("%s", nombre);
				
				char consulta [80];
				strcpy (consulta,"SELECT jugador.ID FROM jugador WHERE jugador.Username = '");
				strcat (consulta, nombre);
				strcat (consulta,"'AND partida.ID = relacion.ID_P AND relacion.ID_J = jugador.ID");
				
				err=mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al consultar datos de la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
					}
				}
				
				
				
				if (codigo !=0)
				{
					printf ("Respuesta: %s\n", respuesta);
					// Enviamos respuesta
					write (sock_conn,respuesta, strlen(respuesta));
				}
		}
		// Se acabo el servicio para este cliente
		close(sock_conn); 
	}
}
