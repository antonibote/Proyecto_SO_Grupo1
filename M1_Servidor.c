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
#include <my_global.h>
#define MAX_JUGADORES 4

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

typedef struct {	//Definimos estructura de Partida
	JugadorConectado jugadores [10];
	int IDPartida;
	int libre; //0 si esta libre, 1 si esta ocupado
	char fecha[20]; // Fecha en la que se juega.
	int njugadores; //Numero de jugadores. 
	char ganador[20]; // Nombre del ganador.
	char resultados[100]; // [Andrei-127.5/Toni-100/Miguel-80/Erik-20]
	
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
	strcat (consulta, nombre); 		// Arrancamos el nombre
	strcat (consulta,"'");
	strcat (consulta," AND jugador.Pass='");
	strcat (consulta, contra); 		// Arrancamos la contraseña
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
		sprintf(respuesta,"7/NO"); // Ha habido algun error
	else{
		sprintf(respuesta,"7/%s",row[0]); //Todo correcto
	}
}
	
	
	//Funcion para Registrar un nuevo usuario que no existe	
	
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
	
	// Funcion para darse de baja de la base de datos
	
	int DarDeBaja (char respuesta[200], char username[20],char password[20],MYSQL *conn) 
	{
		char consulta[200];
		MYSQL_RES *resultado;
		MYSQL_ROW row;
		
		strcpy (consulta, "SELECT jugador.Username FROM (jugador) WHERE jugador.Username='");
		strcat (consulta, username); 
		strcat (consulta, "'");
		strcat (consulta, " AND jugador.Pass='"); 
		strcat (consulta, password); 
		strcat (consulta, "';");
		
		printf("consulta = %s\n", consulta);
		
		int err = mysql_query(conn, consulta);
		if (err!=0)
		{
			printf ("El Username y el Password no coinciden %u %s\n", mysql_errno(conn), mysql_error(conn));
			exit (1);
		}
		resultado = mysql_store_result (conn);
		row = mysql_fetch_row (resultado);
		
		if (row == NULL)
		{
			printf ("El Username y el Password no coinciden\n");
			strcpy(respuesta,"6/El usuario NO existe, revise si el Usuario y el Password coinciden.");
			return -1;
		}
		
		else
			while (row !=NULL) 
		{
				
				strcpy (consulta, "DELETE FROM jugador WHERE jugador.Username='");
				strcat (consulta, username); 
				strcat (consulta, "';");
				printf("consulta = %s\n", consulta);
				
				strcpy(respuesta,"6/El usuario ha sido eliminado de la base de datos correctamente ");
				
				
				err = mysql_query(conn, consulta);
				if (err!=0)
				{
					printf ("Error al introducir datos la base %u %s\n", mysql_errno(conn), mysql_error(conn));
					strcpy(respuesta,"6/El usuario NO se ha podido eliminar de la base de datos ");
					return -1;
					exit (1);
				}
				printf("\n");
				printf("Despues de dar baja al jugador deseado la BBDD queda de la siguiente forma:\n");
				err=mysql_query (conn, "SELECT * FROM jugador");
				if (err!=0) 
				{
					printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
					exit (1);
				}
				
				resultado = mysql_store_result (conn);
				row = mysql_fetch_row (resultado);
				
				if (row == NULL)
				{
					printf ("No se han obtenido datos en la consulta\n");
				}
				else
					while (row !=NULL) 
				{
						printf ("Username: %s , Password: %s\n", row[1], row[2]);
						row = mysql_fetch_row (resultado);							
				}
					return 0;
		}
	}
	
	
	// Funcion para anadir un jugador que se acaba de conectar a la lista de conectados
	
	void PonerJugadorEnListaConectados(char nombre[100], int socket) 
	{
		strcpy(miLista.Lista[miLista.num].nombre,nombre);
		miLista.Lista[miLista.num].socket=socket;
		miLista.num++;
	}
	
	//Funcion para eliminar un jugador que estaba en la lista de conectados
	
	void EliminarJugadorQueEstabaListaConectados(char *nombre)
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
	
	
	//Funcion que actualiza la lista de conectados
	
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
	
	
	//Funcion que inicializa la tabla de partidas
	
	void JugarPartida () 
	{
		
		int i;
		for (i=0; i<100; i++)
		{	
			miTabla[i].libre=0;
			miTabla[i].IDPartida=i;
		}	
	}
	
	// Funcion para crear nueva partida
	
	int NuevaPartida(char listajugadores[20], int socket) 
	{
		int s=0;
		int encontrado=0; 
		while(s<100 && encontrado==0) //Recorre tabla de partidas.
		{
			if(miTabla[s].libre==0)
			{
				encontrado=1; //Ha encontrado partida libre.
				miTabla[s].libre=1;
			}
			else
			   s++;
		}
		if (encontrado==0)
		{
			return -1; //Máximo de partidas alcanzado, devuelve -1.
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
			strcpy(miTabla[s].jugadores[0].nombre,miLista.Lista[j].nombre);
			miTabla[s].jugadores[0].socket=socket;
			printf("socket anfitrion: %d\n", miTabla[s].jugadores[0].socket);
			char *p = strtok(listajugadores, "/");
			int cont=1; //cont=0 es el anfitrion. Empezamos a partir de 1.
			while(p!=NULL)
			{
				strcpy(miTabla[s].jugadores[cont].nombre,p); //pasamos listajugadores
				encontrado=0;
				j=0;
				while(j<miLista.num && encontrado==0) 
				{
					if (strcmp(miLista.Lista[j].nombre,p)==0)
					{
						encontrado=1;
					}
					else j++;
				}
				miTabla[s].jugadores[cont].socket=miLista.Lista[j].socket; //pasamos socket
				cont++;
				p = strtok( NULL, "/");
			}
			return miTabla[s].IDPartida;
		}
	}
	
	// Funcion para borrar una partida en caso que no se juegue
	
	void BorraPartida (int id) 
	{
		int c = 0;
		while (c < 10)
		{
			strcpy(miTabla[id].jugadores[c].nombre, ""); 
			miTabla[id].jugadores[c].socket = -1;
			c++;
		}
	}
	
	// Funcion que te da posicion de una partida dentro de su lista partidas que hemos creado
	// Ejemplos INPUT: (miTabla, TPartidaMultijugador).
	// Ejemplos OUTPUT: (-1: Andrei no esta conectado), (X: posicion).
	int DamePosicionPartida(int id)
	{
		int i=0;
		int encontrado;
		while (i<100)
		{
			if (miTabla[i].IDPartida==id)
			{
				encontrado=1;
				break;
			}
			i=i+1;
		}
		if (encontrado)
		{
			return i;
		}
		else
			return -1;
	}
	
	// Funcion para poner un usuario a la lista de conectados que hemos definido
	// Ejemplos INPUT: (miLista, "Erik").
	// Ejemplos OUTPUT: (-1: Erik no esta conectado), (X: socket).
	int DameSocket (ListaJugadoresConectados *lista, char nombre[20])
	{
		int i=0;
		int encontrado=0;
		while ((i< lista->num) && !encontrado)
		{
			if (strcmp(lista->Lista[i].nombre,nombre)==0)
				encontrado =1;
			if(!encontrado)
				i=i+1;
		}
		if (encontrado)
			return lista->Lista[i].socket;
		else
			return -1;
	}
	
	
	//-------------------------------- AtenderCliente ----------------------------------------
	
	
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
			
			
			//Peticion de desconexion
			
			if (codigo == 0){
				if(nombre!=NULL){
					pthread_mutex_lock(&mutex);
					EliminarJugadorQueEstabaListaConectados(nombre);
					pthread_mutex_unlock(&mutex);
					NumeroDeConectadosYUsernames(notificacion);
					notificador = 1;
				}
				terminar=1;
			}
			
			//Eliminar usuario de la base de datos (DarDeBaja)
			
			else if (codigo == 6 ){ 
				p = strtok( NULL, "/");
				strcpy (nombre, p);
				
				p = strtok( NULL, "/");
				strcpy (contrasena, p);
				
				
				int result = DarDeBaja(respuesta,nombre,contrasena,conn);
				if(result == 0)
				{
					write (sock_conn,respuesta, strlen(respuesta));
				}
				else
				   write (sock_conn,respuesta, strlen(respuesta));
			}
			
			//Login: loguea un jugador
			
			else if (codigo == 7 ){ 
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
			
			//Registrar: resgitra un jugador 
			
			else if (codigo == 8 ){ 
				char consulta[150];
				p = strtok( NULL, "/");
				strcpy(nombre,p);
				p=strtok(NULL,"/");
				strcpy(contrasena,p);
				Registrar(contrasena,nombre,respuesta);
			}
			
			//CONSULTA 1:  Dame las partidas ganadas de tal username
			
			else if (codigo == 10 ){ 
				//Construimos la consulta SQL
				p = strtok( NULL, "/");
				strcpy(nombre,p);
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
			
			//Consulta 2: Dame cantidad de partidas jugadas de tal username
			
			else if (codigo == 11 ){ 
				
				//Construimos la consulta SQL
				p = strtok( NULL, "/");
				strcpy(nombre,p);
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
			
			//Consulta 3: Dame ID de tal username
			
			else if (codigo == 12 ){ 
				
				//Construimos la consulta SQL
				p = strtok( NULL, "/");
				strcpy(nombre,p);
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
			
			// Invitacion para jugar juntos una partida
			
			else if (codigo == 14) 
			{
				char invitados[300];
				p = strtok( NULL, "/");
				sprintf(invitados, "%s", p);
				p = strtok(NULL, "/");
				while(p!=NULL)
				{
					sprintf(invitados, "%s/%s", invitados,p);
					p = strtok( NULL, "/");	
				}
				int id = NuevaPartida(invitados,sock_conn);
				//enviamos 14/invitado/idpartida
				sprintf(respuesta,"14/%s/%d",miTabla[id].jugadores[0].nombre, id);
				printf("socket invitado: %d\n", miTabla[id].jugadores[1].socket);
				int cont=1;
				while(strcmp(miTabla[id].jugadores[cont].nombre,"")!=0)
				{
					write (miTabla[id].jugadores[1].socket,respuesta, strlen(respuesta));
					cont++;
				}
			}
			
			// Un jugador que ha sido invitado puede Aceptar o Rechazar jugar la partida
			
			else if (codigo == 15) // Aceptar o rechazar jugar la partida
			{
				p = strtok( NULL, "/");
				char aceptaorechaza[10];
				strcpy(aceptaorechaza,p);
				p = strtok( NULL, "/");
				int cont=0;
				int id = atoi(p);
				if(strcmp(aceptaorechaza,"SI")==0) //Partida aceptada
				{
					sprintf(respuesta, "15/SI/%d", id);
					while(strcmp(miTabla[id].jugadores[cont].nombre,"")!=0)
					{
						write (miTabla[id].jugadores[cont].socket,respuesta, strlen(respuesta)); //Enviamos a todos que al final si jugamos la partida
						cont++;
					}
				}
				else //Partida rechazada
				{
					miTabla[id].libre=0;
					sprintf(respuesta, "15/NO/%d", id);
					while(strcmp(miTabla[id].jugadores[cont].nombre,"")!=0)
					{
						write (miTabla[id].jugadores[cont].socket,respuesta, strlen(respuesta));
						cont++;
					}
					BorraPartida(id);
				}
			}
			
			//Chat que se activa solo si se juega la partida
			
			else if (codigo == 16) 
			{ //Codigo para chat.
				p = strtok( NULL, "/");
				char mensaje[500];
				strcpy(mensaje,p);
				char usuario[20];
				p = strtok( NULL, "/");
				strcpy(usuario,p);
				int id = atoi(p);
				int g = 0;
				sprintf (respuesta, "16/%s/%s/%d", mensaje, usuario, id);
				while(strcmp(miTabla[id].jugadores[g].nombre,"")!=0) //Enviamos a todos
				{
					//if(strcmp(miTabla[id].jugadores[g].socket, sock_conn)!= 0)
					//{
					write (miTabla[id].jugadores[g].socket,respuesta, strlen(respuesta));
					//}
					g++;
				}
			}
			
			
			// CODIGO 52: SE PIDE QUE SE ENVIE UNA NOTIFICACION A TODOS LOS DEMAS JUGADORES DE LA PARTIDA.
			// Se recibe con el formato: '52/Andrei-1/[Jugador 1: Andrei] -> Ha pasado por meta. Gana 200 Euros.'
			// Se envia con el formato: '52/[Jugador 1: Andrei] -> Ha pasado por meta. Gana 200 Euros.'
			else if (codigo == 52)
			{
				pthread_mutex_lock( &mutex);
				
				char userID_P[100];// "user-ID_P"
				p = strtok(NULL, "/");
				strcpy(userID_P, p);
				char missatge[500];
				p = strtok(NULL, "/"); // "[Jugador 1: Andrei] -> Ha pasado por meta. Gana 200 Euros."
				sprintf(missatge, "52/%s", p); // Enviar a los clientes el mensaje: "52/[Jugador 1: Andrei] -> Ha pasado por meta. Gana 200 Euros.
				printf ("Respuesta: %s\n",missatge); 
				
				// * * * * * MECANISMO DE REENVIO A LOS JUGADORES DE LA MISMA PARTIDA * * * * *
				char quienEnvia[20];
				char *q = strtok(userID_P, "-");
				strcpy(quienEnvia,q);
				printf("QUIEN ENVIA: %s\n", quienEnvia);
				q = strtok(NULL, "-");
				int idMiPartida = atoi(q);
				printf("PARTIDA DEL QUE ENVIA: %d\n", idMiPartida);
				int PosMiPartida = DamePosicionPartida(idMiPartida);
				printf("POSICION DE LA PARTIDA DEL QUE ENVIA: %d\n", PosMiPartida);
				int otrosSockets[MAX_JUGADORES];
				for (int i=0;i<MAX_JUGADORES;i++)
				{
					char nombreJugador[20];
					strcpy(nombreJugador, miTabla[PosMiPartida].jugadores[i].nombre);
					if (strcmp(nombreJugador,quienEnvia)==0) {
						otrosSockets[i]=-1;
					}
					else {
						otrosSockets[i]=DameSocket(&miLista,nombreJugador);
					}
					printf("SE ANADE EL SOCKET A REENVIAR %d\n", otrosSockets[i]);
				}
				for (int j=0; j<4; j++) { // SE RESPONDE A LOS OTROS DE LA PARTIDA	
					if (otrosSockets[j]!=-1) // Si no es el socket del que ha hecho la peticion ...
					{
						write (otrosSockets[j], missatge, strlen(missatge));
						printf ("ENVIA '%s' al socket '%d'.\n",missatge, otrosSockets[j]);
					}
				}
				// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
				pthread_mutex_unlock( &mutex);
			}
			
			// CODIGO 53: SE AVISA DEL CAMBIO DE TURNO A TODOS LOS JUGADORES DE LA PARTIDA:
			// Se recibe con el formato: '53/Erik-1/turno'
			// Se envia con el formato: '53/turno'
			
			else if (codigo == 53)
			{
				pthread_mutex_lock( &mutex);
				
				char userID_P[100];// "user-IDP"
				p = strtok(NULL, "/");
				strcpy(userID_P, p);
				
				char mensaje[500];
				p = strtok(NULL, "/"); // "turno"
				sprintf(mensaje, "53/%s", p); // Enviara a los clientes del juego el mensaje: "53/turno"
				printf ("<--- Respuesta: %s\n",mensaje); 
				// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
				// * * * * * MECANISMO DE REENVIO A TODOS LOS JUGADORES DE LA MISMA PARTIDA * * * * *
				// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
				char quienEnvia[20];
				char *q = strtok(userID_P, "-");
				strcpy(quienEnvia,q);
				printf(">>> QUIEN ENVIA: %s\n", quienEnvia);
				q = strtok(NULL, "-");
				int idMiPartida = atoi(q);
				printf(">>> PARTIDA DEL QUE ENVIA: %d\n", idMiPartida);
				int PosMiPartida = DamePosicionPartida(idMiPartida);
				printf(">>> POSICION DE LA PARTIDA DEL QUE ENVIA: %d\n", PosMiPartida);
				int otrosSockets[MAX_JUGADORES];
				for (int i=0;i<MAX_JUGADORES;i++)
				{
					// EN ESTE CASO, QUEREMOS QUE LO REENVIE A TODOS LOS DE LA PARTIDA, INCLUIDO EL QUE LO PIDE:
					char nombreJugador[20];
					strcpy(nombreJugador, miTabla[PosMiPartida].jugadores[i].nombre);
					otrosSockets[i]=DameSocket(&miLista,nombreJugador);
					printf(">>> SE ANADE EL SOCKET A REENVIAR %d\n", otrosSockets[i]);
				}
				
				for (int j=0; j<4; j++) { // SE RESPONDE A LOS OTROS DE LA PARTIDA	
					write (otrosSockets[j], mensaje, strlen(mensaje));
					printf ("ENVIA '%s' al socket '%d'.\n", mensaje, otrosSockets[j]);
				}
				pthread_mutex_unlock( &mutex);
			}
			
			// CODIGO 54: SE PIDE QUE SE AVISE A LOS DEMAS JUGADORES DE LA PARTIDA DEL MOVIMIENTO DE UNA FICHA.
			// Se recibe con el formato: '54/Toni-1/numFicha|PosX|PosY'
			// Se envia con el formato: '54/numFicha|PosX|PosY'
			else if (codigo == 54)
			{
				pthread_mutex_lock( &mutex);
				char userID_P[100];// "user-IDP"
				p = strtok(NULL, "/");
				strcpy(userID_P, p);
				
				char mensaje[500];
				p = strtok(NULL, "/"); // "numFicha|PosX|PosY"
				sprintf(mensaje, "54/%s", p); // Enviara a los clientes el mensaje: "54/numFicha|PosX|PosY"
				printf ("<--- Respuesta: %s\n",mensaje); 
				// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
				// * * * * * MECANISMO DE REENVIO A LOS JUGADORES DE LA MISMA PARTIDA * * * * *
				// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
				char quienEnvia[20];
				char *q = strtok(userID_P, "-");
				strcpy(quienEnvia,q);
				printf(">>> QUIEN ENVIA: %s\n", quienEnvia);
				q = strtok(NULL, "-");
				int idMiPartida = atoi(q);
				printf(">>> PARTIDA DEL QUE ENVIA: %d\n", idMiPartida);
				int PosMiPartida = DamePosicionPartida(idMiPartida);
				printf(">>> POSICION DE LA PARTIDA DEL QUE ENVIA: %d\n", PosMiPartida);
				int otrosSockets[MAX_JUGADORES];
				for (int i=0;i<MAX_JUGADORES;i++)
				{
					char nombreJugador[20];
					strcpy(nombreJugador, miTabla[PosMiPartida].jugadores[i].nombre);
					if (strcmp(nombreJugador,quienEnvia)==0) {
						otrosSockets[i]=-1;
					}
					else {
						otrosSockets[i]=DameSocket(&miLista,nombreJugador);
					}
					printf(">>> SE ANADE EL SOCKET A REENVIAR %d\n", otrosSockets[i]);
				}
				for (int j=0; j<4; j++) { // SE RESPONDE A LOS OTROS DE LA PARTIDA	
					if (otrosSockets[j]!=-1) // Si no es el socket del que ha hecho la peticion ...
					{
						write (otrosSockets[j], mensaje, strlen(mensaje));
						printf ("ENVIA '%s' al socket '%d'.\n",mensaje, otrosSockets[j]);
					}
				}
				pthread_mutex_unlock( &mutex);
			}
			
			// CODIGO: 58. SE ACTUALIZA EL DINERO DE LOS DEMAS JUGADORES DE LA PARTIDA.
			// Se recibe con el formato: '58/Erik-1/numFicha|dinero'
			// Se envia con el formato: '58/numFicha|dinero'
			else if (codigo == 58)
			{
				pthread_mutex_lock( &mutex);
				
				char userID_P[100];// "user-ID_P"
				p = strtok(NULL, "/");
				strcpy(userID_P, p);
				
				char mensaje[500];
				p = strtok(NULL, "/"); // "numFicha|dinero"
				sprintf(mensaje, "58/%s", p);
				printf ("<--- Respuesta: %s\n",mensaje); 
				// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
				// * * * * * MECANISMO DE REENVIO A LOS JUGADORES DE LA MISMA PARTIDA * * * * *
				// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
				char quienEnvia[20];
				char *q = strtok(userID_P, "-");
				strcpy(quienEnvia,q);
				printf(">>> QUIEN ENVIA: %s\n", quienEnvia);
				q = strtok(NULL, "-");
				int idMiPartida = atoi(q);
				printf(">>> PARTIDA DEL QUE ENVIA: %d\n", idMiPartida);
				int PosMiPartida = DamePosicionPartida(idMiPartida);
				printf(">>> POSICION DE LA PARTIDA DEL QUE ENVIA: %d\n", PosMiPartida);
				int otrosSockets[MAX_JUGADORES];
				for (int i=0;i<MAX_JUGADORES;i++)
				{
					char nombreJugador[20];
					strcpy(nombreJugador, miTabla[PosMiPartida].jugadores[i].nombre);
					if (strcmp(nombreJugador,quienEnvia)==0) {
						otrosSockets[i]=-1;
					}
					else {
						otrosSockets[i]=DameSocket(&miLista,nombreJugador);
					}
					printf(">>> SE ANADE EL SOCKET A REENVIAR %d\n", otrosSockets[i]);
				}
				for (int j=0; j<4; j++) { // SE RESPONDE A LOS OTROS DE LA PARTIDA	
					if (otrosSockets[j]!=-1) // Si no es el socket del que ha hecho la peticion ...
					{
						write (otrosSockets[j], mensaje, strlen(mensaje));
						printf ("ENVIA '%s' al socket '%d'.\n", mensaje, otrosSockets[j]);
					}
				}
				pthread_mutex_unlock( &mutex);
			}
			
			// CODIGO 59: SE ACTUALIZAN LOS CREDITOS DE LOS DEMAS JUGADORES DE LA PARTIDA.
			// Se recibe con el formato: '59/Toni-1/numFicha|creditos'
			// Se envia con el formato: '59/numFicha/creditos'
			
			else if (codigo == 59)
			{
				pthread_mutex_lock( &mutex);
				
				char userID_P[100];// "user-IDP"
				p = strtok(NULL, "/");
				strcpy(userID_P, p);
				char mensaje[500];
				p = strtok(NULL, "/"); // "numFicha|creditos"
				sprintf(mensaje, "59/%s", p);
				printf ("<--- Respuesta: %s\n",mensaje); 
				// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
				// * * * * * MECANISMO DE REENVIO A LOS JUGADORES DE LA MISMA PARTIDA * * * * *
				// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
				char quienEnvia[20];
				char *q = strtok(userID_P, "-");
				strcpy(quienEnvia,q);
				printf(">>> QUIEN ENVIA: %s\n", quienEnvia);
				q = strtok(NULL, "-");
				int idMiPartida = atoi(q);
				printf(">>> PARTIDA DEL QUE ENVIA: %d\n", idMiPartida);
				int PosMiPartida = DamePosicionPartida(idMiPartida);
				printf(">>> POSICION DE LA PARTIDA DEL QUE ENVIA: %d\n", PosMiPartida);
				int otrosSockets[MAX_JUGADORES];
				for (int i=0;i<MAX_JUGADORES;i++)
				{
					char nombreJugador[20];
					strcpy(nombreJugador, miTabla[PosMiPartida].jugadores[i].nombre);
					if (strcmp(nombreJugador,quienEnvia)==0) {
						otrosSockets[i]=-1;
					}
					else {
						otrosSockets[i]=DameSocket(&miLista,nombreJugador);
					}
					printf(">>> SE ANADE EL SOCKET A REENVIAR %d\n", otrosSockets[i]);
				}
				for (int j=0; j<4; j++) { // SE RESPONDE A LOS OTROS DE LA PARTIDA	
					if (otrosSockets[j]!=-1) // Si no es el socket del que ha hecho la peticiÃ³n ...
					{
						write (otrosSockets[j], mensaje, strlen(mensaje));
						printf ("ENVIA '%s' al socket '%d'.\n",mensaje, otrosSockets[j]);
					}
				}
				pthread_mutex_unlock( &mutex);
			}
			
			// CODIGO 61: SE ACTUALIZAN LOS PROPIETARIOS DE LAS CASILLAS.
			// Se recibe con el formato: '61/user-idJuego/user|idPosPropietario'
			// Se envia con el formato: '61/user|idPosPropietario'
			
			else if (codigo == 61)
			{
				pthread_mutex_lock( &mutex);
				
				char userID_P[100];// "user-IDP"
				p = strtok(NULL, "/");
				strcpy(userID_P, p);
				
				char mensaje[500];
				p = strtok(NULL, "/"); // "user|idPosPropietario"
				sprintf(mensaje, "61/%s", p);
				printf ("<--- Respuesta: %s\n",mensaje);
				
				// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
				// * * * * * MECANISMO DE REENVIO A LOS JUGADORES DE LA MISMA PARTIDA * * * * *
				// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
				char quienEnvia[20];
				char *q = strtok(userID_P, "-");
				strcpy(quienEnvia,q);
				printf(">>> QUIEN ENVIA: %s\n", quienEnvia);
				q = strtok(NULL, "-");
				int idMiPartida = atoi(q);
				printf(">>> PARTIDA DEL QUE ENVIA: %d\n", idMiPartida);
				int PosMiPartida = DamePosicionPartida(idMiPartida);
				printf(">>> POSICION DE LA PARTIDA DEL QUE ENVIA: %d\n", PosMiPartida);
				int otrosSockets[MAX_JUGADORES];
				for (int i=0;i<MAX_JUGADORES;i++)
				{
					char nombreJugador[20];
					strcpy(nombreJugador, miTabla[PosMiPartida].jugadores[i].nombre);
					if (strcmp(nombreJugador,quienEnvia)==0) {
						otrosSockets[i]=-1;
					}
					else {
						otrosSockets[i]=DameSocket(&miLista,nombreJugador);
					}
					printf(">>> SE ANADE EL SOCKET A REENVIAR %d\n", otrosSockets[i]);
				}
				for (int j=0; j<4; j++) { // SE RESPONDE A LOS OTROS DE LA PARTIDA	
					if (otrosSockets[j]!=-1) // Si no es el socket del que ha hecho la PETICION ...
					{
						write (otrosSockets[j], mensaje, strlen(mensaje));
						printf ("ENVIA '%s' al socket '%d'.\n", mensaje, otrosSockets[j]);
					}
				}
				pthread_mutex_unlock( &mutex);
			}
			
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
		}
		// Se acabo el servicio para este cliente
		close(sock_conn); 
	}
	
	
	
	//--------------------------------------------- MAIN ------------------------------
	
	
	
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
		serv_adr.sin_port = htons(50001); //9070 o 50000-50003
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
		//conn = mysql_real_connect (conn,"localhost", "root", "mysql", "M1_BBDD",0, NULL, 0);
		conn = mysql_real_connect (conn,"shiva2.upc.es", "root", "mysql", "M1_BBDD",0, NULL, 0);
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
