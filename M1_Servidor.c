#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <string.h>
#include <ctype.h>
#include <mysql.h>
#include <pthread.h>
//#include <my_global.h>

int i;
int sockets[100];
MYSQL *conn;
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;

typedef struct{
	char nombre[100];
	int socket;
} JugadorConectado;
typedef struct{
	JugadorConectado Lista[100];
	int num;
}ListaJugadoresConectados;

typedef struct {
	JugadorConectado jugadores [10];
	int id;
	int libre; //0 si está libre, 1 si está ocupado
}TPartidaMultijugador;

typedef TPartidaMultijugador TablaPartidas[100];

ListaJugadoresConectados miLista;
TablaPartidas miTabla;



//Funcion Login de un jugador que quiere hacer una partida

void Login(char contra[100], char nombre[100], char respuesta[512]){
	
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
		sprintf(respuesta,"7/NO");
	else{
		sprintf(respuesta,"7/%s",row[0]);
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
				sprintf(respuesta,"8/NO");
			}
			else 
				sprintf(respuesta,"8/SI");
		}
		else{
			sprintf(respuesta,"8/NO");
		}
	}	
		
	void PonerJugadorEnListaConectados(char nombre[100], int socket)
	{
		strcpy(miLista.Lista[miLista.num].nombre,nombre);
		miLista.Lista[miLista.num].socket=socket;
		miLista.num++;
	}
		//Funcion para eliminar un jugador que estaba en la lista de conectados
	void ElimimarJugadorQueEstabaListaConectados(char *nombre)
	{
		int encontrado = 0;
		int i=0;
		while((encontrado==0) && (i < miLista.num))
		{
			if(strcmp(nombre,miLista.Lista[i].nombre)==0)
			{
				encontrado=1;
				int j = i;
					
				for(j;j<miLista.num;j++)
				{
					strcpy(miLista.Lista[j].nombre,miLista.Lista[j+1].nombre);
					miLista.Lista[j].socket = miLista.Lista[j+1].socket;
				}
				miLista.num--;
			}
				i++;
		}
	}
		
	void NumeroDeConectadosYUsernames(char respuesta[512])
	{
		pthread_mutex_lock(&mutex);
		sprintf(respuesta,"%d,",miLista.num);
			
		for (int i = 0; i<miLista.num;i++)
		{
			sprintf(respuesta,"%s%s,",respuesta,miLista.Lista[i].nombre);
		}
			respuesta[strlen(respuesta)-1] ='\0';
			pthread_mutex_unlock(&mutex);
		}
		
		
	void JugarPartida () //funcion que inicializa la tabla de partidas
	{
		
		int i;
		for (i=0; i<100; i++)
		{	
				miTabla[i].libre=0;
				miTabla[i].id=i;
		}	
	}
	
	int NuevaPartida(char jugador[20], int socket)
	//devuelve -1 si el maximo de partidas ha sido alcanzado, id partida si no lo esta
	{
		int s=0;
		int encontrado=0; //0 no encuentra partida libre, 1 encuentra partida libre
		while(s<100 && encontrado==0)
		{
			if(miTabla[s].libre==0)
			{
				encontrado=1;
				miTabla[s].libre=1;
			}
			else
			   s++;
		}
		if (encontrado==0)
		{
			return -1;
		}
		else
		{
			int j = 0;
			encontrado=0;
			while(j<miLista.num && encontrado==0)
			{
				if (miLista.Lista[j].socket==socket)
				{
					encontrado=1;
				}
				else j++;
			}
			//int id=g+1;
			strcpy(miTabla[s].jugadores[0].nombre,miLista.Lista[j].nombre);
			miTabla[s].jugadores[0].socket=socket;
			printf("socket anfitrion: %d\n", miTabla[s].jugadores[0].socket);
			strcpy(miTabla[s].jugadores[1].nombre,jugador);
			encontrado=0;
			j=0;
			while(j<miLista.num && encontrado==0)
			{
				if (strcmp(miLista.Lista[j].nombre,jugador)==0)
				{
					encontrado=1;
				}
				else j++;
			}
			miTabla[s].jugadores[1].socket=miLista.Lista[j].socket;
			printf("socket jugador invitado: %d\n", miTabla[s].jugadores[1].socket);
			return miTabla[s].id;
		}
	}
		
	void *AtenderCliente (void *socket)
	{
			//Bucle para atención al cliente
			int terminar = 0;
			int sock_conn, ret;
			MYSQL_RES *resultado;
			MYSQL_ROW row;
			int err;
			int contservicios=0;
			
			int *s;
			s = (int *) socket;
			sock_conn = *s;
			
			char peticion[512];
			char respuesta[512];
			char notificacion[512];
			int notificador = 0;
			
			//Entramos en un bucle para atender todas la peticiones que quiere este cliente
			//Hasta que se desconecte
			
			while (terminar == 0){
				strcpy(respuesta,"");
				
				printf ("Escuchando\n");
				// Ahora recibimos la petici?n
				ret=read(sock_conn,peticion, sizeof(peticion));
				printf ("Recibido\n");
				
				// Tenemos que anadirle la marca de fin de string 
				// para que no escriba lo que hay despues en el buffer
				peticion[ret]='\0';
				
				printf ("Peticion: %s\n",peticion);
				
				// vamos a ver que quieren
				char *p = strtok( peticion, "/");
				int codigo =  atoi (p);
				// Ya tenemos el codigo de la peticion
				char nombre[20];
				char contrasena[100];
				char fecha[20];
				char id_j[100];
				char consulta[100];
				//p = strtok(NULL,"/");
				
					//if(p!=NULL){
					//strcpy (nombre, p);
					// Ya tenemos el nombre
					//printf ("Codigo: %d, Nombre: %s\n", codigo, nombre);
				//}
				
				
				if (codigo == 0){//peticion de desconexion
					if(nombre!=NULL){
						pthread_mutex_lock(&mutex);
						ElimimarJugadorQueEstabaListaConectados(nombre);
						pthread_mutex_unlock(&mutex);
						NumeroDeConectadosYUsernames(notificacion);
						notificador = 1;
					}
					terminar=1;
				}
				
				else if (codigo == 7 ){ //LOGIN
					p = strtok( NULL, "/");
					strcpy(nombre,p);
					p = strtok(NULL, "/");
					strcpy(contrasena,p);
					Login(contrasena,nombre,respuesta);
					if(strcmp(respuesta,"7/NO") != 0)
					{
						pthread_mutex_lock(&mutex);
						PonerJugadorEnListaConectados(nombre,sock_conn);
						pthread_mutex_unlock(&mutex);
						NumeroDeConectadosYUsernames(notificacion);
						notificador=1;
					}
				}
				else if (codigo == 8 ){ //Registrar
					char consulta[150];
					p = strtok( NULL, "/");
					strcpy(nombre,p);
					p=strtok(NULL,"/");
					strcpy(contrasena,p);
					Registrar(contrasena,nombre,respuesta);
				}
				else if (codigo == 9 ){ //Dime nombre ganador y duracion partida jugada tal dia
					//Construimos la consulta SQL
					sprintf(consulta,"SELECT jugador.Username, partida.Duracion FROM jugador, partida, relacion WHERE partida.Fecha = '%s' AND partida.ID = relacion.ID_P AND relacion.ID_J = jugador.ID",fecha);
					
					//Hacemos la consulta
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
						printf ("No se han obtenido datos en la consulta\n");
						sprintf(respuesta,"9/mal");
					}
					else
					{
						while (row != NULL) 
						{
							sprintf(respuesta,"9/ El ganador es: %s y la duracion es: %s",row[0],row[1]);
							row = mysql_fetch_row(resultado);
						}
					}
				}
				else if (codigo == 10 ){ ////Dame las partidas ganadas de tal username
					//Construimos la consulta SQL
					sprintf(consulta,"SELECT SUM(relacion.Victorias) FROM jugador,partida,relacion WHERE jugador.Username = '%s' AND jugador.ID = relacion.ID_J AND relacion.ID_P = partida.ID",nombre);
					
					// consulta SQL para obtener una tabla con todos los datos de la base
					err=mysql_query (conn, consulta);
					
					if (err!=0) {
						printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
						exit (1);
					}
					
					//recogemos el resultado de la consulta
					resultado = mysql_store_result (conn);
					row = mysql_fetch_row (resultado);
					
					if (row == NULL)
					{
						printf ("No se han obtenido datos en la consulta\n");
						sprintf(respuesta,"10/mal");
					}
					else
					{
						while (row !=NULL) {
							// la columna 0 contiene las victorias
							sprintf(respuesta,"10/%s",row[0]);
							row = mysql_fetch_row (resultado);
						}
					}
					
				}
				else if (codigo == 11 ){ //Dame cantidad de partidas jugadas de tal username
					
					//Construimos la consulta SQL
					sprintf(consulta,"SELECT SUM(relacion.Cantidad) FROM jugador, partida, relacion WHERE partida.ID = relacion.ID_P AND relacion.ID_J = jugador.ID AND jugador.Username='%s'",nombre);
					
					//Hacemos la consulta
					err=mysql_query (conn, consulta); 
					if (err!=0) {
						printf ("Error al consultar datos de la base %u %s\n",
								mysql_errno(conn), mysql_error(conn));
						exit (1);
					}
					
					//Recogemos el resultado de la consulta 
					resultado = mysql_store_result (conn); 
					row = mysql_fetch_row (resultado);			
					if(row == NULL){
						printf("No se han obtenido datos en la consulta\n");
						sprintf(respuesta,"11/mal");
					}
					else
					{
						while (row != NULL)
						{
							//int c = atoi(row[0]);
							sprintf(respuesta,"11/%s",row[0]);
							row = mysql_fetch_row(resultado);
						}
					}
				}
				else if (codigo == 12 ){ //Funcion dame ID de tal username
					
					//Construimos la consulta SQL
					sprintf(consulta,"SELECT jugador.ID FROM jugador WHERE jugador.Username = '%s'", nombre);
					
					//Hacemos la consulta
					err=mysql_query (conn, consulta); 
					if (err!=0) {
						printf ("Error al consultar datos de la base %u %s\n",
								mysql_errno(conn), mysql_error(conn));
						exit (1);
					}
					
					//Recogemos el resultado de la consulta
					resultado = mysql_store_result (conn);
					row = mysql_fetch_row (resultado);
					if (row == NULL){
						printf("No se han obtenido datos en la consulta\n");
						sprintf(respuesta,"12/mal");
					}
					else{
						while (row != NULL) 
						{
							sprintf(respuesta,"12/%s",row[0]);
							row = mysql_fetch_row(resultado);
						}
					}
				}
				else if (codigo == 14)
				{
					p = strtok( NULL, "/");
					int id = NuevaPartida(p,sock_conn);
					//ahora enviamos 14/invitado/idpartida
					sprintf(respuesta,"14/%s/%d",miTabla[id].jugadores[0].nombre, id);
					printf("socket invitado: %d\n", miTabla[id].jugadores[1].socket);
					write (miTabla[id].jugadores[1].socket,respuesta, strlen(respuesta));
				}
				
				else if (codigo == 15)
				{
					p = strtok( NULL, "/");
					char aceptaorechaza[10];
					strcpy(aceptaorechaza,p);
					p = strtok( NULL, "/");
					int id = atoi(p);
					if(strcmp(aceptaorechaza,"SI")==0)
					{
						sprintf(respuesta, "15/SI/%d", id);
						write (miTabla[id].jugadores[0].socket,respuesta, strlen(respuesta)); //Enviamos a todos que al final si jugamos la partida
					}
					else
					{
						miTabla[id].libre=0;
						sprintf(respuesta, "15/NO/%d", id);
						write (miTabla[id].jugadores[0].socket,respuesta, strlen(respuesta));
					}
				}
								
				else if (codigo == 20)  //Dime cuantos servicios llevo realizados
				{
					sprintf(respuesta,"%d",contservicios);
				}
				
				printf("Respuesta: %s\n",respuesta);
				printf("Notificacion: %s\n", notificacion);
				write (sock_conn,respuesta, strlen(respuesta));
				
				
				if(notificador == 1)
				{
					char codigolistaconectados[200] = "13/";
					strcat(codigolistaconectados,notificacion);
					printf("Notificacion:%s\n",codigolistaconectados);
					for(int i=0; i<miLista.num;i++)
					{
						write (miLista.Lista[i].socket,codigolistaconectados,strlen(codigolistaconectados));
					}
					
				}
				notificador=0;
				if (codigo != 0) // if (codigo !=0 && codigo != 8 && codigo != 9)
				{
					printf ("Respuesta: %s\n", respuesta);
					// Enviamos respuesta
					write (sock_conn,respuesta, strlen(respuesta));
				}
				if (codigo==9 || codigo==10|| codigo==11 ||codigo==12)
				{
					pthread_mutex_lock(&mutex); //No me interrumpas ahora
					contservicios=contservicios+1;
					pthread_mutex_unlock(&mutex); //Ya puedes interrumpirme
					//notificar a todos los clientes conectados numero de consultas hechas
					char notificacion[20];
					sprintf(notificacion,"20/%d",contservicios);
					int z;
					for (z=0;z<i;z++)
					{
						write (sockets[z],notificacion, strlen(notificacion));
						
					}
				}
			}
			// Se acabo el servicio para este cliente
			close(sock_conn); 
	}
		
	int main(int argc, char *argv[]) {
			
			int sock_conn, sock_listen, ret;
			struct sockaddr_in serv_adr;
			char peticion[512];
			char respuesta[512];
			char notificacion[512];
			miLista.num=0;
			JugarPartida();
			
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
			serv_adr.sin_port = htons(9080); //9070 o 50000-50003
			if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
				printf ("Error al bind\n");
			
			if (listen(sock_listen, 3) < 0)
				printf("Error en el Listen\n");
			
			int err;
			// Estructura especial para almacenar resultados de consultas 
			MYSQL_RES *resultado;
			MYSQL_ROW row;
			//Creamos una conexion al servidor MYSQL 
			conn = mysql_init(NULL);
			if (conn==NULL) {
				printf ("Error al crear la conexion: %u %s\n", 
						mysql_errno(conn), mysql_error(conn));
				exit (1);
			}
			//inicializar la conexion
			conn = mysql_real_connect (conn,"localhost", "root", "mysql", "M1_BBDD",0, NULL, 0);
			//conn = mysql_real_connect (conn,"shiva2.upc.es", "root", "mysql", "M1_BBDD",0, NULL, 0);
			if (conn==NULL) 
			{
				printf ("Error al inicializar la conexion: %u %s\n", 
						mysql_errno(conn), mysql_error(conn));
				exit (1);
			}
			
			int i=0;
			int socket[500];
			pthread_t thread[500];
			
			//Bucle infinito
			for(;;){
				printf ("Escuchando\n");
				
				sock_conn = accept(sock_listen, NULL, NULL);
				printf ("He recibido conexion\n");
				socket[i]=sock_conn;
				
				//La variable sock_conn es el socket que utilizamos para este cliente
				pthread_create(&thread[i], NULL, AtenderCliente, &socket[i]);
				i = i+1;		
			}
	}

