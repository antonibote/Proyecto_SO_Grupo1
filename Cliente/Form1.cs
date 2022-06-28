using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Cliente
{
    public partial class Form1 : Form
    {
        // ------------------- VARIABLES GLOBALES --------------------

        public int ID_J;
        Socket server;
        Thread atender_mensajes_servidor;
        delegate void DelegadoParaEscribir(string mensaje);
        delegate void DelegadoParaDataGridView(DataGridView mensaje);

       // Estructura necesaria para reproducir el audio de bienvenida
       // private string ruta = "C:/Users/erikf/Desktop/bienvenido wey.mp3";

        // Todos los delegados que se usan para el juego
        delegate void DelegadoNotificacion(string mensaje);
        delegate void DelegadoTurno(int torn);
        delegate void DelegadoFichas(int IDF, string CX, string CY);
        delegate void DelegadoDinero(int IDF, double dinero);
        delegate void DelegadoCreditos(int IDF, double creditos);
        delegate void DelegadoOwners(string USUARI, int IDPOS);
        int cont = 0; // Contador numero de jugadores invitados
        int cont2 = 0; // Contador respuesta de los invitados que han aceptado la partida
        bool Jugamos = true; // Bool para ver si jugamos
        int idpartida; 

        // Asignaciones de variables que tienen que ver con el juego y usaremos en más de una función      
        int turno;
        int contjuego;
        int JugadorID;
        int idFichaUser;      
        int idCartaComunidad;
        int idCartaSuerte;
        string user;
        string usuario1;
        string usuario2;
        string usuario3;
        string usuario4;

        // Definimos variables para asignar los nombres de los jugadores
        string nombre1;
        string nombre2;
        string nombre3;
        string nombre4;

        // Definimos variables para asignar casillas de los jugadores

        int casilla1;
        int casilla2;
        int casilla3;
        int casilla4;

        // Definimos variables para asignar el dinero de cada jugador 

        double dinero1;
        double dinero2;
        double dinero3;
        double dinero4;

        // Definimos variables para asignar los creditos de cada jugador 

        double creditos1;
        double creditos2;
        double creditos3;
        double creditos4;

        // Definimos variables para asignar el dinero inicial a todos los jugadores

        int DineroInicial = 1800; // (€). Consideramos: 1 CREDITO = 50€.

        // Definimos variables para asignar los resultados de la partida

        string nP1; double cP1;
        string nP2; double cP2;
        string nP3; double cP3;
        string nP4; double cP4;

        // Definimos la variable para asignar la id de la partida actual

        int idGame;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // ------------------- FUNCIONES --------------------

        private void btnDesconectar_Click(object sender, EventArgs e) // Funcion para desconectarse del servidor
        {
            MessageBox.Show("Desconectado correctamente, vuelve pronto, " + username.Text);
            // Ponemos no visible todos los botones, labels, testbox, tablero...
            username.Visible = false;
            password.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            MostrarPass.Visible = false;
            BtnRegistrar.Visible = false;
            BtnLogin.Visible = false;         
            btnDesconectar.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            peticion.Visible = false;
            dataGridView1.Visible = false;
            btnConectar.Enabled = true;
            BtnDarBaja.Visible = false;
            BtnInvitar.Visible = false;
            chat.Visible = false;
            PicDado1.Visible = false;
            PicDado2.Visible = false;
            BtnTirarDados.Visible = false;
            NumDados.Visible = false;
            ficha1.Visible = true;
            Jugador.Visible = false;
            TurnoLbl.Visible = false;
            turno1.Visible = false;
            turno2.Visible = false;
            turno3.Visible = false;
            turno4.Visible = false;
            JugadoresLbl.Visible = false;
            user1.Visible = false;
            user2.Visible = false;
            user3.Visible = false;
            user4.Visible = false;
            DineroLbl.Visible = false;
            money1.Visible = false;
            money2.Visible = false;
            money3.Visible = false;
            money4.Visible = false;
            CreditosLbl.Visible = false;
            credito1.Visible = false;
            credito2.Visible = false;
            credito3.Visible = false;
            credito4.Visible = false;
            MiMatriculaLbl.Visible = false;
            MiMatricula.Visible = false;
            notificacion.Visible = false;
            ComunidadPanel.Visible = false;
            SuertePanel.Visible = false;

            TurnoLbl.Visible = false;
            CoordenadasLbl.Visible = false;
            TableroJuego.Visible = false;

            // Limpiamos el campo username, password y el testbox de las consultas donde ponemos el nombre
            username.Clear();
            password.Clear();
            nombre.Clear();

            // Mensaje de desconexión que enviamos al servidor
            string mensaje = "0/"+ username.Text;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos y thread tambien lo desactivamos
            atender_mensajes_servidor.Abort();
            server.Shutdown(SocketShutdown.Both);
            server.Close();           
        }


        private void btnConectar_Click(object sender, EventArgs e) // Funcion para conectarnos al servidor
        {
            // Ponemos visible botones, labels, testbox...
            username.Visible = true;
            password.Visible = true;
            username.Enabled = true;
            password.Enabled = true;
            BtnRegistrar.Enabled = true;
            BtnLogin.Enabled = true;
            MostrarPass.Visible = true;
            BtnRegistrar.Visible = true;
            BtnLogin.Visible = true;
            btnConectar.Enabled = false;
            btnDesconectar.Visible = true;
            label1.Visible = true;
            label2.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            MostrarPass.Enabled = true;
            BtnDarBaja.Enabled = true;

            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos

            //Entorno de desarrollo (Localhost)
            //IPAddress direc = IPAddress.Parse("192.168.56.101"); //Para Toni: 192.168.56.101
            //IPEndPoint ipep = new IPEndPoint(direc, 9070);

            // Entorno de produccion (Shiva)
            IPAddress direc = IPAddress.Parse("147.83.117.22");
            IPEndPoint ipep = new IPEndPoint(direc, 50002); // De 50000 a 50002


            //Creamos el socket que hara posible la conexion entre 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                MessageBox.Show("Conectado correctamente");

                //Poner en marcha el thread que utilizamos para atender los mensajes del servidor
                ThreadStart ts = delegate { Atender_Peticiones_Servidor(); };
                atender_mensajes_servidor = new Thread(ts);
                atender_mensajes_servidor.Start();

            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) // Funcion que se activa si se cierra el formulario
        {
            // Mensaje de desconexión
            string mensaje = "0/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos del servidor
            atender_mensajes_servidor.Abort();
            username.Clear();
            password.Clear();
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }
 
        public void PonerDataGridView (string mensaje) // Funcion para construir la lista de usuarios conectados
        {
            if (mensaje != null && mensaje != "") 
            {
                dataGridView1.Rows.Clear();
                string[] parts = mensaje.Split(',');

                int num = Convert.ToInt32(parts[0]);
                int i = 0;

                while(i < num)
                {
                    int x = dataGridView1.Rows.Add();
                    dataGridView1.Rows[x].Cells[0].Value = parts[i + 1];
                    i++;
                }
            }
            else 
            {
                MessageBox.Show("Ha ocurrido algun tipo de problema");
            }
        }

        public void PonerVisibleDataGridView(DataGridView name) // Funcion para poner visible la lista de conectados
        {
            name.Visible = true;
        }

        public void PonerNOVisibleDataGridView(DataGridView name) //Funcion para poner NO visible la lista de conectados
        {
            name.Visible = false;
        }

        private void Atender_Peticiones_Servidor() // Funcion para atender las peticiones del servidor
        {
            while (true) 
            {
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string mensajeinicial = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                string[] trozos = mensajeinicial.Split('/');  // Trozeamos el mensaje para ir cogiendo los trozos que nos interesan
                int codigo = Convert.ToInt32(trozos[0]);           //Al tirar los dados peta aqui
                string mensaje = trozos[1]; // Asignamos mensaje = trozos[1], para que sea mas facil trabajar con lo que nos interesa

                switch (codigo)
                {
                    case 6: // Eliminar usuario de la Base de datos
                        MessageBox.Show(mensaje); // Recibimos el mensaje del servidor conforme el usuario se ha eliminado correctamente
                        Invoke(new Action(() =>    // Desactivamos todo, ya que el usuario se ha dado de baja
                        {
                            chat.Visible = false;
                            PicDado1.Visible = false;
                            PicDado2.Visible = false;
                            BtnTirarDados.Visible = false;
                            dataGridView1.Visible = false;
                            BtnInvitar.Visible = false;
                            peticion.Visible = false;
                            username.Enabled = false;
                            password.Enabled = false;
                            MostrarPass.Enabled = false;
                            BtnDarBaja.Enabled = false;
                            TurnoLbl.Visible = false;
                            CoordenadasLbl.Visible = false;
                            NumDados.Visible = false;
                            TableroJuego.Visible = false;
                            ficha1.Visible = true;
                        }));
                        break;

                    case 7: // Iniciamos sesion con nuestro username y password
                        if (mensaje != "NO") // Sesion iniciada correctamente
                        {
                            ID_J = Convert.ToInt32(mensaje);
                            MessageBox.Show("Bienvenido de nuevo, " + username.Text + " , te hemos echado de menos");
                            DelegadoParaDataGridView delegado112 = new DelegadoParaDataGridView(PonerVisibleDataGridView); //Ponemos visible la lista de conectados
                            dataGridView1.Invoke(delegado112, new object[] { dataGridView1 });                          
                            Invoke(new Action(() =>  // Activamos las peticiones, boton dar de baja, lista conectados...
                            {
                                dataGridView1.Refresh();
                                peticion.Visible = true;
                                BtnLogin.Enabled = false;
                                BtnRegistrar.Enabled = false;
                                BtnDarBaja.Visible = true;
                                BtnInvitar.Visible = true;
                                username.Enabled = false;
                                password.Enabled = false;

                                //Activamos el audio que nos da la bienvenida al iniciar sesión                               
                                //  axWindowsMediaPlayer1.URL = ruta;
                                // axWindowsMediaPlayer1.Ctlcontrols.play();
                            }));

                        }
                        else
                        {
                            MessageBox.Show("Usuario no encontrado, escribe bien el usuario y la contraseña. O registrate, que es gratis");
                        }
                        break;

                    case 8: // Nos registramos en el servidor, recibimos "SI" si todo ha ido bien, "NO" si ese usuario ya existe
                        if (mensaje == "SI")
                        {
                            MessageBox.Show("Registrado correctamente, ya puedes iniciar sesión");
                        }
                        else
                        {
                            MessageBox.Show("Usuario ya registrado, ponga otro nombre de usuario");
                        }
                        break;

                    case 10:  // Respuesta de la consulta partidas ganadas por username
                        if (mensaje == "mal")
                        {
                            MessageBox.Show(nombre.Text + " no existe o no ha ganado ninguna partida");
                        }
                        else 
                        {
                            MessageBox.Show(nombre.Text + " ha ganado " + mensaje + " partidas");
                        }
                        break;

                    case 11: // Respuesta de la consulta cantidad de partidas jugadas por username
                        if (mensaje == "mal")
                        {
                            MessageBox.Show(nombre.Text + " no existe o no ha jugado ninguna partida");
                        }
                        else 
                        {
                            MessageBox.Show(nombre.Text + " ha jugado " + mensaje + " partidas");
                        }
                        break;

                    case 12: // Respuesta de la consulta dame el ID de tal username 
                        if (mensaje == "mal") 
                        {
                            MessageBox.Show(nombre.Text + " no existe o no ha iniciado sesión");
                        }
                        else 
                        {
                            MessageBox.Show("El ID de " + nombre.Text + " es: " + mensaje);
                        }
                        break;

                    case 13: // Actualizamos la lista de conectados automaticamente

                        DelegadoParaEscribir delegado13 = new DelegadoParaEscribir(PonerDataGridView);
                        dataGridView1.Invoke(delegado13, new object[] { mensaje });
                        break;

                    case 14:
                        // Invitación para jugar. Recibimos 14/nombreanfitrion/idpartida
                        DialogResult aceptaorechaza = MessageBox.Show($" {mensaje} te ha invitado a jugar, ¿Aceptas?", "Invitación", MessageBoxButtons.YesNo);
                        if (aceptaorechaza == DialogResult.Yes) // Se acepta la invitacion
                        {
                            string mensaje2 = "15/SI/"+trozos[2];
                            byte[] msg = Encoding.ASCII.GetBytes(mensaje2);
                            server.Send(msg);
                            idpartida = Convert.ToInt32(trozos[2]);
                        }
                        else if (aceptaorechaza == DialogResult.No) // Se rechaza la invitacion
                        {
                            string mensaje2 = "15/NO/"+trozos[2];
                            byte[] msg = Encoding.ASCII.GetBytes(mensaje2);
                            server.Send(msg);
                        }
                        break;

                    case 53: // Nos llega informacion de un turno, se recibe: "53/turno"
                        int turno = Convert.ToInt32(mensaje);
                        DelegadoTurno delegated3 = new DelegadoTurno(UseGestorDeTurnos); // Usamos el delegado para gestionar los turnos
                        Invoke(delegated3, new object[] { turno });
                        break;

                    case 15:
                        // El usuario que ha sido invitado ya ha respondido a la invitacion, el que ha invitado recibe 15/SI/idpartida o 15/NO/idpartida
                        cont2++;
                        if (mensaje == "SI")
                        {
                            MessageBox.Show("Invitación aceptada, a jugar!", username.Text);
                            Invoke(new Action(() =>
                            {
                                // Ponemos visible botones del juego, testbox, label, dados, tablero...
                                chat.Visible = true;
                                PicDado1.Visible = true;
                                PicDado2.Visible = true;
                                BtnTirarDados.Visible = true;
                                NumDados.Visible = true;
                                TurnoLbl.Visible = true;
                                CoordenadasLbl.Visible = true;
                                TableroJuego.Visible = true;
                                TurnoLbl.Visible = true;
                                CoordenadasLbl.Visible = true;
                                ficha1.Visible = true;
                                Jugador.Visible = true;
                                TurnoLbl.Visible = true;
                                turno1.Visible = true;
                                turno2.Visible = true;
                                turno3.Visible = true;
                                turno4.Visible = true;
                                JugadoresLbl.Visible = true;
                                user1.Visible = true;
                                user2.Visible = true;
                                user3.Visible = true;
                                user4.Visible = true;
                                DineroLbl.Visible = true;
                                money1.Visible = true;
                                money2.Visible = true;
                                money3.Visible = true;
                                money4.Visible = true;
                                CreditosLbl.Visible = true;
                                credito1.Visible = true;
                                credito2.Visible = true;
                                credito3.Visible = true;
                                credito4.Visible = true;
                                MiMatriculaLbl.Visible = true;
                                MiMatricula.Visible = true;
                                notificacion.Visible = true;
                                ComunidadPanel.Visible = true;
                                SuertePanel.Visible = true;
                                
                                // Asignamos nombre a los jugadores
                                if (username.Text == "Erik")
                                {
                                    user1.Text = "Erik";
                                    usuario1 = username.Text;
                                    JugadorID = 1;
                                    nombre1 = username.Text;
                                }

                                if (username.Text == "Toni")
                                {
                                    user2.Text = "Toni";
                                    usuario2 = username.Text;
                                    JugadorID = 2;
                                    nombre2 = username.Text;
                                }

                                if (username.Text == "Andrei")
                                {
                                    user3.Text = "Andrei";
                                    usuario3 = username.Text;
                                    JugadorID = 3;
                                    nombre3 = username.Text;
                                }

                                // Identificamos que jugador es cada uno y asignamos id de ficha
                                if (usuario1 == user)
                                {
                                    idFichaUser = 1;
                                }
                                else if (usuario2 == user)
                                {
                                    idFichaUser = 2;
                                }
                                else if (usuario3 == user)
                                {
                                    idFichaUser = 3;
                                }
                                else if (usuario4 == user)
                                {
                                    idFichaUser = 4;
                                }

                                // Ponemos todas las fichas en la posicion de salida
                                casilla1 = 0;
                                casilla2 = 0;
                                casilla3 = 0;
                                casilla4 = 0;

                                // Ponemos los creditos iniciales de cada jugador
                                creditos1 = 0;
                                creditos2 = 0;
                                creditos3 = 0;
                                creditos4 = 0;

                                // Asignamos la cantidad de dinero inicial a cada jugador
                                dinero1 = DineroInicial;
                                dinero2 = DineroInicial;
                                dinero3 = DineroInicial;
                                dinero4 = DineroInicial;

                                // Ponemos en el form el dinero de cada jugador
                                money1.Text = Convert.ToString(dinero1);
                                money2.Text = Convert.ToString(dinero2);
                                money3.Text = Convert.ToString(dinero3);
                                money4.Text = Convert.ToString(dinero4);

                                // Ponemos en el form los creditos de cada jugador
                                credito1.Text = Convert.ToString(creditos1);
                                credito2.Text = Convert.ToString(creditos2);
                                credito3.Text = Convert.ToString(creditos3);
                                credito4.Text = Convert.ToString(creditos4);                               

                                // Declaramos el turno 1 para que el primer jugador haga su turno
                                turno = 1;
                                turno1.Visible = true;
                                turno2.Visible = false;
                                turno3.Visible = false;
                                turno4.Visible = false;
                                turno1.Text = "Te toca";

                                // Asignamos el nombre de cada jugador con el usuario y el numero que le toca
                                Jugador.Text = user;
                                nombre1 = usuario1;
                                nombre2 = usuario2;
                                nombre3 = usuario3;
                                nombre4 = usuario4;
                                
                                // El id de usuario sera igual al id de la ficha
                                idUsuari.Text = Convert.ToString(idFichaUser);
                                idGame = idpartida;

                                // El username con ID=1 será el primero en tirar los dados, así que activamos los dados solo para ese jugador
                                if (JugadorID == 1)
                                {
                                    BtnTirarDados.Enabled = true;
                                }
                                else
                                {
                                    BtnTirarDados.Enabled = false;
                                }

                                // Abrimos el data grid view donde salen las asignaturas
                                ActualizaGridMaterias();
                            }));
                        }

                        else // Rechazamos la invitacion, ponemos el bool jugamos a false
                        {
                            MessageBox.Show("Invitación rechazada, lo sentimos");
                            Jugamos = false;
                        }

                        if (cont2 == cont) // Si el numero de personas que aceptan es igual al numero de personas invitadas, jugamos la partida
                        {
                            if (Jugamos == true)
                            {
                                //Avisamos que jugamos
                                string mensaje2 = $"16/SI/{trozos[2]}";
                                byte[] msg = Encoding.ASCII.GetBytes(mensaje2);
                                server.Send(msg);
                            }
                            else // El numero de personas que aceptan NO es igual al numero de personas invitadas, NO jugamos la partida
                            {
                                //Avisamos que no jugamos
                                string mensaje2 = $"16/NO/{trozos[2]}";
                                byte[] msg = Encoding.ASCII.GetBytes(mensaje2);
                                server.Send(msg);
                            }
                        }
                        break;

                    case 16: // Chat que se activa solo si jugamos la partida (Recibimos del servidor: 11/mensaje/usuario/idPartida)
                        Invoke(new Action(() =>
                        {
                            listBox1.Items.Add($"{trozos[2]}: {trozos[1]}");

                        }));

                        break;

                    case 52: // Llega la notificacion de que alguien ha pasado por la casilla de salida y gana 200€
                        // Recibimos: "52/[JUGADOR 1: Erik] -> Ha pasado por meta. Gana 200€."
                        DelegadoNotificacion delegated2 = new DelegadoNotificacion(UseEscribirNotificacion);
                        Invoke(delegated2, new object[] { mensaje });
                        break;

                    // Caso 53 esta mas arriba por temas de asignar variables

                    case 54: // Un jugador ha movido su ficha, recibimos: "54/numFicha|PosX|PosY"
                        char separador = '|';
                        string[] parte1 = mensaje.Split(separador);
                        int idFicha = Convert.ToInt32(parte1[0]); // Copiamos la parte1[0] en idFicha
                        string PositionX = parte1[1]; // Obtenemos posicion X
                        string PositionY = parte1[2]; // Obtenemos posicion Y
                        DelegadoFichas delegated4 = new DelegadoFichas(UsoGestorFichas); // Usamos el delegado para gestionar las fichas
                        Invoke(delegated4, new object[] { idFicha, PositionX, PositionY });
                        break;

                    case 58: // Un usuario ha ganado o perdido dinero, hay que actualizarlo, recibimos: "idficha|dinero"         
                        char separador2 = '|';
                        string[] parte2 = mensaje.Split(separador2);
                        int IDFichaA = Convert.ToInt32(parte2[0]); // Copiamos la parte2[0] en idFichaA
                        double dinero = Convert.ToDouble(parte2[1]);

                        DelegadoDinero delegate5 = new DelegadoDinero(UsoGestorDinero); // Usamos el delegado para actualizar el dinero
                        Invoke(delegate5, new object[] { IDFichaA, dinero });
                        break;

                    case 59: // Un usuario ha comprado una materia, hay que actualizar sus creditos. Recibimos: Nos llega: "ficha|creditos"
                        char separador3 = '|';
                        string[] parte3 = mensaje.Split(separador3);
                        int IDFichaB = Convert.ToInt32(parte3[0]); // Copiamos la parte3[0] en idFichaB
                        double creditos = Convert.ToDouble(parte3[1]);

                        DelegadoCreditos delegate6 = new DelegadoCreditos(UsoGestorCreditos); // Usamos el delegado para gestionar los creditos
                        Invoke(delegate6, new object[] { IDFichaB, creditos });
                        break;

                    case 61: // Un usuario ha suspendido una materia, nos llega: "user|idPosOwners"
                        char separador4 = '|';
                        string[] parte4 = mensaje.Split(separador4);
                        string usuari = parte4[0];                          // Copiamos la parte4[0] en usuari
                        int idPositionOwners = Convert.ToInt32(parte4[1]);

                        DelegadoOwners delegate7 = new DelegadoOwners(UsoGestorOwners); // Usamos el delegado para gestionar los dueños de las materias
                        Invoke(delegate7, new object[] { usuari, idPositionOwners });
                        break;


                }
            }
        }

        private void EnviarBtn_Click(object sender, EventArgs e)  // Funcion para preguntar las consultas escogidas
        {
            if (nombre.Text.Length > 1)
            {
                if (radioButton2.Checked) // Consulta partidas ganadas de tal username
                {
                    string mensaje = "10/" + nombre.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else if (radioButton3.Checked) // Consulta partidas jugadas de tal username
                {
                    // Enviamos el nombre
                    string mensaje = "11/" + nombre.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else if (radioButton4.Checked) // Consulta dame ID de tal username
                {
                    string mensaje = "12/" + nombre.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
            }
        }

        private void BtnRegistrar_Click(object sender, EventArgs e) // Boton para registrarse
        {
            if (((username.Text.Length > 1 && password.Text.Length > 1)) && ((username.Text != "") && (password.Text != "")))
            {
                string mensaje = "8/" + username.Text + "/" + password.Text;
                //Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else
            {
                MessageBox.Show("El nombre y la contraseña deben tener más de un carácter");

            }
        }

        private void BtnLogin_Click(object sender, EventArgs e) // Boton para loguearse
        {        
                if ((username.Text != "") && (password.Text != ""))
                {
                    string mensaje = "7/" + username.Text + "/" + password.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                {
                    MessageBox.Show("El campo de usuario o contraseña está vacio");
                }
        }

        private void MostrarPass_CheckedChanged(object sender, EventArgs e) // Funcion para poder ocultar/mostrar la contraseña
        {
            if (MostrarPass.Checked == false)
            {
                password.UseSystemPasswordChar = false;

            }
            else
            {
                password.UseSystemPasswordChar = true;
            }
        }                 

        private void BtnInvitar_Click(object sender, EventArgs e) // Boton para invitar a jugar a otros jugadores
        {
            string mensaje = "14/" + dataGridView1.CurrentCell.Value.ToString();
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private void button1_Click(object sender, EventArgs e) // Boton para enviar el mensaje que ha escrito el usuario en el chat
        {
            string mensaje_clean = textBox1.Text.Replace("/", "\\"); 
            string mensaje = $"16/{mensaje_clean}/{username.Text}/{idpartida}";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

        }

        private void BtnDarBaja_Click(object sender, EventArgs e) // Boton para darse de baja en la base de datos
        {
            string mensaje = "6/" + username.Text + "/" + password.Text;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private void TableroJuego_MouseMove(object sender, MouseEventArgs e) // Funcion saber coordenadas exactas del tablero
        {
            CoordenadasLbl.Text = e.X.ToString() + "," + e.Y.ToString();
        }

        private void BtnTirarDados_Click(object sender, EventArgs e) // Boton que permite tirar los dos dados y salgan dos numeros aleatorios
        {
            int TiroDado1;
            int TiroDado2;
            int numerodado1 = 0;
            int numerodado2 = 0;
            int SumaDados;   

            Random aleatorio = new Random(); // Generamos un numero random a los dados
            TiroDado1 = aleatorio.Next(1, 7); // dado1 tiene 6 números, va del 1 al 7
            TiroDado2 = aleatorio.Next(1, 7); // dado2 tiene 6 números, va del 1 al 7

            if (TiroDado1 == 1)
            {
                PicDado1.Image = Image.FromFile("Cara1.png"); //Cargamos imagen de la cara 1 del dado1
                numerodado1 = 1;
            }

            if (TiroDado1 == 2)
            {
                PicDado1.Image = Image.FromFile("Cara2.png"); //Cargamos imagen de la cara 2 del dado1
                numerodado1 = 2;
            }

            if (TiroDado1 == 3)
            {
                PicDado1.Image = Image.FromFile("Cara3.png"); //Cargamos imagen de la cara 3 del dado1
                numerodado1 = 3;
            }

            if (TiroDado1 == 4)
            {
                PicDado1.Image = Image.FromFile("Cara4.png"); //Cargamos imagen de la cara 4 del dado1
                numerodado1 = 4;
            }

            if (TiroDado1 == 5)
            {
                PicDado1.Image = Image.FromFile("Cara5.png"); //Cargamos imagen de la cara 5 del dado1
                numerodado1 = 5;
            }

            if (TiroDado1 == 6)
            {
                PicDado1.Image = Image.FromFile("Cara6.png"); //Cargamos imagen de la cara 6 del dado1
                numerodado1 = 6;
            }

            if (TiroDado2 == 1)
            {
                PicDado2.Image = Image.FromFile("Cara1.png"); //Cargamos imagen de la cara 1 del dado2
                numerodado2 = 1;
            }

            if (TiroDado2 == 2)
            {
                PicDado2.Image = Image.FromFile("Cara2.png"); //Cargamos imagen de la cara 2 del dado2
                numerodado2 = 2;
            }

            if (TiroDado2 == 3)
            {
                PicDado2.Image = Image.FromFile("Cara3.png"); //Cargamos imagen de la cara 3 del dado2
                numerodado2 = 3;
            }

            if (TiroDado2 == 4)
            {
                PicDado2.Image = Image.FromFile("Cara4.png"); //Cargamos imagen de la cara 4 del dado2
                numerodado2 = 4;
            }

            if (TiroDado2 == 5)
            {
                PicDado2.Image = Image.FromFile("Cara5.png"); //Cargamos imagen de la cara 5 del dado2
                numerodado2 = 5;
            }

            if (TiroDado2 == 6)
            {
                PicDado2.Image = Image.FromFile("Cara6.png"); //Cargamos imagen de la cara 6 del dado2
                numerodado2 = 6;
            }

            SumaDados = numerodado1 + numerodado2; // Sumamos los dos dados para obtener el numero total
            NumDados.Text = SumaDados.ToString(); // Pasamos la suma de dados a string para poder ponerlo en el Label

            if (SumaDados == 2)
            {
                moverFicha(2);
            }
            else if (SumaDados == 3)
            {
                moverFicha(3);
            }
            else if (SumaDados == 4)
            {
                moverFicha(4);
            }
            else if (SumaDados == 5)
            {
                moverFicha(5);
            }
            else if (SumaDados == 6)
            {
                moverFicha(6);
            }
            else if (SumaDados == 7)
            {
                moverFicha(7);
            }
            else if (SumaDados == 8)
            {
                moverFicha(8);
            }
            else if (SumaDados == 9)
            {
                moverFicha(9);
            }
            else if (SumaDados == 10)
            {
                moverFicha(10);
            }
            else if (SumaDados == 11)
            {
                moverFicha(11);
            }
            else if (SumaDados == 12)
            {
                moverFicha(12);
            }

            //BtnTirarDados.Enabled = false; //Desactivamos tirada hasta que nos vuelva a tocar
        }


        // ---------------------- VARIABLES GLOBALES DEL JUEGO ---------------------------

        // PRECIOS de las asignaturas (0 a 39, 40 en total): (Recordamos que cada credito son 50€)
        double[] Precios = { 0, 4 * 50, 0, 4 * 50, 0, 5 * 50, 4 * 50, 0, 4 * 50, 4 * 50, 0, 5 * 50, 0 , 5 * 50, 5 * 50, 5 * 50, 6 * 50, 0, 6 * 50, 6 * 50, 0, 7 * 50, 0, 7 * 50, 7 * 50, 5 * 50, 8 * 50, 8 * 50, 0, 8 * 50, 0, 9 * 50, 9 * 50, 0, 9 * 50, 5 * 50, 0, 10 * 50, 0, 12 * 50 };

        // CREDITOS de cada asignatura (0 a 39, 40 en total)
        double[] Creditos = { 0, 4, 0, 4, 0, 0, 4, 0, 4, 4, 0, 5, 0, 5, 5, 0, 6, 0, 6, 6, 0, 7, 0, 7, 7, 5, 8, 8, 0, 8, 0, 9, 9, 0, 9, 0, 0, 10, 0, 12 };
        // SALIDA, CÁRCEL, COBRAS, PAGAS, SEGURATA, COMUNIDAD Y SUERTE NO TIENEN CREDITOS:

        // PROPIETARIOS (0 a 39, 40 en total), al empezar estan vacíos
        string[] Owners = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        // SALIDA, CÁRCEL, SEGURATA, COBRAS, PAGAS, COMUNIDAD Y SUERTE NO PUEDEN TENER PROPIETARIOS:

        // NOMBRES DE LAS CASILLAS
        string[] Asuntos = { "Salida", "CAL", "Comunidad", "FIS", "Pagas200", "Profe", "IO", "Suerte", "ETS", "ET", "Cárcel", "PP", "Cobras100", "CSL", "FT", "Profe", "PDS", "Comunidad", "CSD", "FC", "Cobras500", "ER", "Suerte", "SO", "API", "Profe", "PES", "ERF", "Cobras150", "CET", "Segurata", "SRF", "CA", "Comunidad", "ESR", "Profe", "Suerte", "TIQ", "Pagas100", "TFG" };

        // VECTOR QUE NOS DICE SI UNA CASILLA ES UNA MATERIA O NO
        bool[] Subject = { false, true, false, true, false, false, true, false, true, true, false, true, false, true, true, false, true, false, true, true, false, true, false, true, true, false, true, true, false, true, false, true, true, false, true, false, false, true, false, true };

        // VECTOR QUE NOS DICE SI UNA CASILLA ES DE SUERTE
        bool[] CasillasSuerte = { false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false };

        // VECTOR QUE NOS DICE SI UNA CASILLA ES DE COMUNIDAD
        bool[] CasillasComunidad = { false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false };

        // VECTOR QUE NOS DICE SI UNA CASILLA ES DE PROFESOR
        bool[] CasillasProfe = { false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, true, false, false, false, false };

        // VECTOR QUE NOS DICE SI LA CASILLA ES REPETIR TIQ (PAGAS 200€)
        bool[] CasillasPagas200 = { false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };

        // VECTOR QUE NOS DICE SI UNA CASILLA ES REPETIR TIQ (PAGAS 100€)
        bool[] CasillasPagas100 = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false };

        // VECTOR QUE NOS DICE SI LA CASILLA ES OBTIENES UNA BECA (COBRAS 500€)
        bool[] CasillasCobras500 = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };

        // VECTOR QUE NOS DICE SI LA CASILLA ES APRUEBAS PP (COBRAS 100€)
        bool[] CasillasCobras100 = { false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };

        // VECTOR QUE NOS DICE SI LA CASILLA ES VALERO TE DONA DINERO (COBRAS 150€)
        bool[] CasillasCobras150 = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false };


        // ----------------------- DELEGADOS DEL JUEGO ---------------------------

        // Delegado para gestionar el movimiento de las fichas de todos los formularios
        delegate void GestionarFichas(int idFicha, string PosX, string PosY);

        // Delegado para gestionar los turnos
        delegate void GestionarTurnos(int turno);

        // Delegado para actualizar el dinero de todos los formularios
        delegate void GestionarDinero(int idFicha, double diners);

        // Delegado para actualizar los creditos de todos los formularios
        delegate void GestionarCreditos(int idFicha, double credits);

        // Delegado para actualizar los propietarios y la lista de asignaturas aprobadas de todos los formularios
        delegate void GestionarOwners(string usuari, int idPositionOwners);

        // Delegado para escribir las notificaciones
        delegate void EscribirNotificaciones(string mensaje);


        // ------------------ FUNCIONES QUE USAN LOS DELEGADOS -------------------

        // Actualizar la lista de Owners y el data grid de Asignaturas Aprobadas
        public void UsoGestorOwners(string usuari, int idPositionOwners)
        {
            GestionarOwners delegado4 = new GestionarOwners(GestorOwners);
            Invoke(delegado4, new object[] { usuari, idPositionOwners });
        }

        public void GestorOwners(string usuari, int idPositionOwners)
        {
            // Ponemos el nombre del propietario en la lista de propietarios
            if (usuari != "-1")
            {
                Owners[idPositionOwners] = usuari; // Ponemos el usuario
            }
            else
            {
                Owners[idPositionOwners] = ""; // Borramos el usuario
            }

            // Actualizamos el data grid view de asignaturas aprobadas
            ActualizaGridMaterias();

            // Si todas las materias estan ocupadas, avisamos al servidor de que se ha acabado la partida
            int contador = 0;
            for (int i = 0; i < 40; i++)
            {
                if (Owners[i] != "")
                {
                    contador = contador + 1;
                }
            }
        }

        // Actualizamos el dinero
        public void UsoGestorDinero(int idFicha, double diners)
        {
            GestionarDinero delegado4 = new GestionarDinero(GestorDinero);
            Invoke(delegado4, new object[] { idFicha, diners });
        }

        public void GestorDinero(int idFicha, double diners)
        {
            if (idFicha == 1)
            {
                this.dinero1 = diners;
                this.money1.Text = Convert.ToString(diners);
            }
            else if (idFicha == 2)
            {
                this.dinero2 = diners;
                this.money2.Text = Convert.ToString(diners);
            }
            else if (idFicha == 3)
            {
                this.dinero3 = diners;
                this.money3.Text = Convert.ToString(diners);
            }
            else if (idFicha == 4)
            {
                this.dinero4 = diners;
                this.money4.Text = Convert.ToString(diners);
            }
        }

        // Actualizar los creditos

        public void UsoGestorCreditos(int idFicha, double credits)
        {
            GestionarCreditos delegado4 = new GestionarCreditos(GestorCreditos);
            Invoke(delegado4, new object[] { idFicha, credits });
        }

        public void GestorCreditos(int idFicha, double credits)
        {
            if (idFicha == 1)
            {
                this.creditos1 = credits;
                this.credito1.Text = Convert.ToString(credits);
            }
            else if (idFicha == 2)
            {
                this.creditos2 = credits;
                this.credito2.Text = Convert.ToString(credits);
            }
            else if (idFicha == 3)
            {
                this.creditos3 = credits;
                this.credito3.Text = Convert.ToString(credits);
            }
            else if (idFicha == 4)
            {
                this.creditos4 = credits;
                this.credito4.Text = Convert.ToString(credits);
            }
        }

        // Actualizar la posicion de las fichas de todos los jugadores   
        public void UsoGestorFichas(int idFicha, string PosX, string PosY)
        {
            GestionarFichas delegado4 = new GestionarFichas(GestorFichas);
            Invoke(delegado4, new object[] { idFicha, PosX, PosY });
        }

        public void GestorFichas(int idFicha, string PosX, string PosY)
        {
            int posX = getCoordinate(PosX);
            int posY = getCoordinate(PosY);
            if (idFicha == 1)
            {
                ficha1.Location = new Point(posX, posY);
            }
            else if (idFicha == 2)
            {
                ficha2.Location = new Point(posX, posY);
            }
            else if (idFicha == 3)
            {
                ficha3.Location = new Point(posX, posY);
            }
            else if (idFicha == 4)
            {
                ficha4.Location = new Point(posX, posY);
            }
        }

        // Gestionar turnos
        public void UseGestorDeTurnos(int turno)
        {
            GestionarTurnos delegado3 = new GestionarTurnos(GestorDeTurnos);
            Invoke(delegado3, new object[] { turno });
        }

        public void GestorDeTurnos(int turno) // turno es un int del JugadorID que le toca mover ficha
        {
            if (turno == JugadorID) // Si es nuestro turno
            {
                // Como estamos en un nuevo turno, quitamos las fotos que teníamos del turno de antes de Comunidad y Suerte:
                ComunidadPanel.BackgroundImage = null;
                SuertePanel.BackgroundImage = null;
                // Tambien quitamos la notificacion de antes
                notificacion.Text = null;
                // Activamos el dado
                BtnTirarDados.Enabled = true;

                if (JugadorID == 1)
                {
                    turno1.Visible = true; turno2.Visible = false; turno3.Visible = false; turno4.Visible = false;
                    user = "Erik";
                }

                if (JugadorID == 2)
                {
                    turno1.Visible = false; turno2.Visible = true; turno3.Visible = false; turno4.Visible = false;
                    user = "Toni";
                }

                if (JugadorID == 3)
                {
                    turno1.Visible = false; turno2.Visible = false; turno3.Visible = true; turno4.Visible = false;
                    user = "Andrei";
                }

                if (JugadorID == 4)
                {
                    turno1.Visible = false; turno2.Visible = false; turno3.Visible = false; turno4.Visible = true;
                }
            }

            else // Si no es nuestro turno
            {
                // Como estamos en un nuevo turno, quitamos las fotos que teníamos del turno de antes de Comunidad y Suerte
                ComunidadPanel.BackgroundImage = null;
                SuertePanel.BackgroundImage = null;
                // Tambien quitamos la notificacion de antes
                notificacion.Text = null;
                // Desactivamos el dado
                BtnTirarDados.Enabled = false;

                if (turno == 1)
                {
                    turno1.Visible = true; turno2.Visible = false; turno3.Visible = false; turno4.Visible = false;
                }

                if (turno == 2)
                {
                    turno1.Visible = false; turno2.Visible = true; turno3.Visible = false; turno4.Visible = false;
                }

                if (turno == 3)
                {
                    turno1.Visible = false; turno2.Visible = false; turno3.Visible = true; turno4.Visible = false;
                }

                if (turno == 4)
                {
                    turno1.Visible = false; turno2.Visible = false; turno3.Visible = false; turno4.Visible = true;
                }

            }
        }
        int idCartaComunity;
        int idCartaLuck;
        public double randomComunidad()
        {
            // Dinero que va a descontar del jugador:
            double tasa = 0;

            // Genera un número aleatorio del 1 al 5 para importar una carta de comunidad diferente.
            Random myObject = new Random();
            int Num = myObject.Next(1, 5);
            idCartaComunity = Num; // Guardamos la carta para comunicarla a los demás.
            if (Num == 1)
            {
                ComunidadPanel.BackgroundImage = Properties.Resources.Community1;
                tasa = -30;
            }
            else if (Num == 2)
            {
                ComunidadPanel.BackgroundImage = Properties.Resources.Community2;
                tasa = -20;
            }
            else if (Num == 3)
            {
                ComunidadPanel.BackgroundImage = Properties.Resources.Community3;
                tasa = -15;
            }
            else if (Num == 4)
            {
                ComunidadPanel.BackgroundImage = Properties.Resources.Community4;
                tasa = -10;
            }
            else if (Num == 5)
            {
                ComunidadPanel.BackgroundImage = Properties.Resources.Community5;
                tasa = -25;
            }
            return tasa;
        }

        public double randomSuerte()
        {
            // Dinero que va a descontar del jugador:
            double tasa = 0;

            // Genera un número aleatorio del 1 al 3 para importar una carta de suerte diferente.
            Random myObject = new Random();
            int Num = myObject.Next(1, 3);
            idCartaLuck = Num; // Guardamos la carta para comunicarla a los demás.
            if (Num == 1)
            {
                SuertePanel.BackgroundImage = Properties.Resources.SuerteCSD;
                tasa = -50;
            }
            else if (Num == 2)
            {
                SuertePanel.BackgroundImage = Properties.Resources.SuerteFC;
                tasa = -150;
            }
            else if (Num == 3)
            {
                SuertePanel.BackgroundImage = Properties.Resources.SuerteSO;
                tasa = -100;
            }
            return tasa;
        }


        // Escribir notificaciones
        public void UseEscribirNotificacion(string missatge)
        {
            EscribirNotificaciones delegado2 = new EscribirNotificaciones(EscribirNotificacion);
            Invoke(delegado2, new object[] { missatge });
        }
        public void EscribirNotificacion(string missatge)
        {
            notificacion.Text = missatge;
        }

        // ------------------- FUNCIÓN PARA CADA CASILLA ----------------------

        public void CasillaNueva(int nC, int idUser) // nC: numCasilla.
        {
            double precioComprar = Precios[nC];
            double creditos = Creditos[nC];
            string propietari = Owners[nC];
            string asignatura = Asuntos[nC];

            string jugador; // Nombre del jugador

            if (idUser == 1)
            {
                jugador = nombre1;
            }
            else if (idUser == 2)
            {
                jugador = nombre2;
            }
            else if (idUser == 3)
            {
                jugador = nombre3;
            }
            else if (idUser == 4)
            {
                jugador = nombre4;
            }

            // CASILLA COBRAS 100:
            if (CasillasCobras100[nC] == true)
            {
                if (idUser == 1)
                    dinero1 = dinero1 + 100;
                if (idUser == 2)
                    dinero1 = dinero2 + 100;
                if (idUser == 3)
                    dinero1 = dinero3 + 100;
                if (idUser == 4)
                    dinero1 = dinero4 + 100;
            }

            // CASILLA COBRAS 150 :
            if (CasillasCobras150[nC] == true)
            {
                if (idUser == 1)
                    dinero1 = dinero1 + 150;
                if (idUser == 2)
                    dinero1 = dinero2 + 150;
                if (idUser == 3)
                    dinero1 = dinero3 + 150;
                if (idUser == 4)
                    dinero1 = dinero4 + 150;
            }

            // CASILLA COBRAS 500:
            if (CasillasCobras500[nC] == true)
            {
                if (idUser == 1)
                    dinero1 = dinero1 + 500;
                if (idUser == 2)
                    dinero1 = dinero2 + 500;
                if (idUser == 3)
                    dinero1 = dinero3 + 500;
                if (idUser == 4)
                    dinero1 = dinero4 + 500;
            }

            // CASILLA PAGAS 100:
            if (CasillasPagas100[nC] == true)
            {
                if (idUser == 1)
                    dinero1 = dinero1 - 100;
                if (idUser == 2)
                    dinero1 = dinero2 - 100;
                if (idUser == 3)
                    dinero1 = dinero3 - 100;
                if (idUser == 4)
                    dinero1 = dinero4 - 100;
            }

            // CASILLA PAGAS 200:
            if (CasillasPagas200[nC] == true)
            {
                if (idUser == 1)
                    dinero1 = dinero1 - 200;
                if (idUser == 2)
                    dinero1 = dinero2 - 200;
                if (idUser == 3)
                    dinero1 = dinero3 - 200;
                if (idUser == 4)
                    dinero1 = dinero4 - 200;
            }


            // CASILLA SUERTE:
            if (CasillasSuerte[nC] == true)
            {
                double tasas = randomSuerte();
                if (idUser == 1)
                {
                    dinero1 = dinero1 + tasas;
                    money1.Text = Convert.ToString(dinero1);
                    string mensaje = "58/" + user + "-" + idGame + "/" + idUser + "|" + dinero1;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    Thread.Sleep(200);
                    server.Send(msg);
                }
                else if (idUser == 2)
                {
                    dinero2 = dinero2 + tasas;
                    money2.Text = Convert.ToString(dinero2);
                    string mensaje = "58/" + user + "-" + idGame + "/" + idUser + "|" + dinero2;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    Thread.Sleep(200);
                    server.Send(msg);
                }
                else if (idUser == 3)
                {
                    dinero3 = dinero3 + tasas;
                    money3.Text = Convert.ToString(dinero3);
                    string mensaje = "58/" + user + "-" + idGame + "/" + idUser + "|" + dinero3;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    Thread.Sleep(200);
                    server.Send(msg);
                }
                else if (idUser == 4)
                {
                    dinero4 = dinero4 + tasas;
                    money4.Text = Convert.ToString(dinero4);
                    string mensaje = "58/" + user + "-" + idGame + "/" + idUser + "|" + dinero4;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    Thread.Sleep(200);
                    server.Send(msg);
                }

                if (tasas < 0)
                {
                    notificacion.Text = "[Casilla Comunidad] ---> Se te han descontado correctamente los " + tasas * (-1) + " €!";

                    // NOTIFICACIÓN:
                    // ---> Avisamos a los demás de que  le han quitado dinero.
                    string mensaje = "52/" + user + "-" + idGame + "/" + "[JUGADOR " + JugadorID + ": " + user + "] -> Le han descontado " + tasas + " Euros.";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    Thread.Sleep(200);
                    server.Send(msg);
                }
            }

            // CASILLA COMUNIDAD:
            if (CasillasComunidad[nC] == true)
            {
                double tasas = randomComunidad();
                if (idUser == 1)
                {
                    dinero1 = dinero1 + tasas;
                    money1.Text = Convert.ToString(dinero1);
                    string mensaje = "58/" + user + "-" + idGame + "/" + idUser + "|" + dinero1;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    Thread.Sleep(200);
                    server.Send(msg);
                }
                else if (idUser == 2)
                {
                    dinero2 = dinero2 + tasas;
                    money2.Text = Convert.ToString(dinero2);
                    string mensaje = "58/" + user + "-" + idGame + "/" + idUser + "|" + dinero2;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    Thread.Sleep(200);
                    server.Send(msg);
                }
                else if (idUser == 3)
                {
                    dinero3 = dinero3 + tasas;
                    money3.Text = Convert.ToString(dinero3);
                    string mensaje = "58/" + user + "-" + idGame + "/" + idUser + "|" + dinero3;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    Thread.Sleep(200);
                    server.Send(msg);
                }
                else if (idUser == 4)
                {
                    dinero4 = dinero4 + tasas;
                    money4.Text = Convert.ToString(dinero4);
                    string mensaje = "58/" + user + "-" + idGame + "/" + idUser + "|" + dinero4;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    Thread.Sleep(200);
                    server.Send(msg);
                }

                if (tasas < 0)
                {
                    notificacion.Text = "[Casilla Comunidad] ---> Se te han descontado los " + tasas * (-1) + " €!";

                    // NOTIFICACIÓN:
                    // ---> Avisamos a los demás de que le han quitado dinero.
                    string mensaje = "52/" + user + "-" + idGame + "/" + "[JUGADOR " + JugadorID + ": " + user + "] -> Le han descontado " + tasas + " Euros.";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    Thread.Sleep(200);
                    server.Send(msg);
                }
            }

            // CASILLA DEL TIPO PROFE:
            if (CasillasProfe[nC] == true)
            {
                if (idUser == 1)
                    dinero1 = dinero1 + 250;
                if (idUser == 2)
                    dinero1 = dinero2 + 250;
                if (idUser == 3)
                    dinero1 = dinero3 + 250;
                if (idUser == 4)
                    dinero1 = dinero4 + 250;
            }

            // CASILLA DEL TIPO ASIGNATURA
            if ((nC == 1) || (nC == 3) || (nC == 6) || (nC == 8) || (nC == 9) || (nC == 11) || (nC == 13) || (nC == 14) || (nC == 16) || (nC == 18) || (nC == 19) || (nC == 21) || (nC == 23) || (nC == 24) || (nC == 26) || (nC == 27) || (nC == 29) || (nC == 31) || (nC == 32) || (nC == 34) || (nC == 37) || (nC == 39))
            {
                // La casilla no tiene propietario:
                if (propietari == "")
                {
                    if (idUser == 1) // La ficha que ha caido en esa casilla es del jugador 1:
                    {
                        if (precioComprar <= dinero1) // El usuario tiene suficiente dinero para comprar la casilla:
                        {
                            // Preguntamos al jugador si quiere comprar (aprobar) la asignatura:
                            DialogResult dr = MessageBox.Show("Quieres matricular " + Asuntos[nC] + "? [Precio: " + Precios[nC] + "] [Créditos: " + Creditos[nC] + "]", "Matrícula EETAC", MessageBoxButtons.YesNo);
                            switch (dr)
                            {
                                case DialogResult.Yes:

                                    // Compramos la asignatura
                                    dinero1 = dinero1 - precioComprar; // Se le quita el dinero al usuario
                                    money1.Text = Convert.ToString(dinero1); // Actualizamos el dinero que tiene este usuario
                                    creditos1 = creditos1 + creditos; // Se le suman los creditos al usuario
                                    credito1.Text = Convert.ToString(creditos1); // Actualizamos los creditos que tiene este usuario
                                    Owners[nC] = nombre1; // La casilla pasa a tener propietario

                                    // Avisamos a los demás clientes de cual es el nuevo propietario de la asignatura
                                    // Formato: "61/user-idGame/user|idPosOwners"
                                    int idPosOwners = nC;
                                    string aviso = "61/" + user + "-" + idGame + "/" + user + "|" + idPosOwners;
                                    byte[] mstge = System.Text.Encoding.ASCII.GetBytes(aviso);
                                    Thread.Sleep(200);
                                    server.Send(mstge);
                                    // Mensaje para actualizar el dinero
                                    // Formato: "58/user-idGame/ficha|dinero"
                                    string missatge = "58/" + user + "-" + idGame + "/" + idUser + "|" + dinero1;
                                    byte[] msge = System.Text.Encoding.ASCII.GetBytes(missatge);
                                    Thread.Sleep(200);
                                    server.Send(msge);

                                    // Mensaje para actualizar los creditos:
                                    // Formato: "59/user-idGame/ficha|creditos"
                                    string MNSJ = "59/" + user + "-" + idGame + "/" + idUser + "|" + creditos1;
                                    byte[] mnsj = System.Text.Encoding.ASCII.GetBytes(MNSJ);
                                    Thread.Sleep(200);
                                    server.Send(mnsj);

                                    // Nos avisamos a nosotros mismos para confirmar:
                                    notificacion.Text = "[Casilla " + Asuntos[nC] + "]---> Has comprado por " + precioComprar + " Euros) " + Asuntos[nC] + ". Se te han sumado " + creditos + " Creditos.";
                                    // NOTIFICACIÓN:
                                    // ---> Avisamos a los demás de que has comprado (aprobado) una asignatura
                                    // Formato: "52/Erik-1/[JUGADOR 1: Erik] -> Ha comprado por +precioComprar+ Euros) asignatura. Se le han sumado +creditos+ Creditos"
                                    string mensaje = "52/" + user + "-" + idGame + "/" + "[JUGADOR " + JugadorID + ": " + user + "] -> Ha aprobado comprado por " + precioComprar + " Euros " + Asuntos[nC] + ". Se le han sumado " + creditos + " Creditos.";
                                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                                    Thread.Sleep(200);
                                    server.Send(msg);

                                    // Actualizar el data grid view de las materias
                                    ActualizaGridMaterias();

                                    // Si todas las materias estan compradas, y somos el jugador 1, se acaba la partida
                                    int contador = 0;
                                    for (int i = 0; i < 40; i++)
                                    {
                                        if (Owners[i] != "")
                                        {
                                            contador = contador + 1;
                                        }
                                    }
                                    break;

                                case DialogResult.No:
                                    // No hace nada
                                    break;

                            }
                        }
                    }
                    // LO MISMO PARA USER 2:

                    else if (idUser == 2) // La ficha que ha caido en esa casilla es del jugador 1:
                    {
                        if (precioComprar <= dinero2) // El usuario tiene suficiente dinero para comprar la casilla:
                        {
                            // Preguntamos al jugador si quiere comprar (aprobar) la asignatura:
                            DialogResult dr = MessageBox.Show("Quieres matricular " + Asuntos[nC] + "? [Precio: " + Precios[nC] + "] [Créditos: " + Creditos[nC] + "]", "Matrícula EETAC", MessageBoxButtons.YesNo);
                            switch (dr)
                            {
                                case DialogResult.Yes:

                                    // Compramos la asignatura:
                                    dinero2 = dinero2 - precioComprar; // Se le quita el dinero al usuario.
                                    money2.Text = Convert.ToString(dinero2); // Actualizamos el dinero que tiene este usuario.
                                    creditos2 = creditos2 + creditos; // Se le suman los creditos al usuario.
                                    credito2.Text = Convert.ToString(creditos2); // Actualizamos los creditos que tiene este usuario.
                                    Owners[nC] = nombre2; // La casilla pasa a tener propietario.

                                    // Avisamos a los demás clientes de cual es el nuevo propietario de la asignatura:
                                    // Formato: '61/user-idGame/user|idPosOwners'
                                    int idPosOwners = nC;
                                    string aviso = "61/" + user + "-" + idGame + "/" + user + "|" + idPosOwners;
                                    byte[] mstge = System.Text.Encoding.ASCII.GetBytes(aviso);
                                    Thread.Sleep(200);
                                    server.Send(mstge);

                                    // Mensaje para actualizar el dinero:
                                    // Formato: '58/user-idGame/ficha|dinero'
                                    string missatge = "58/" + user + "-" + idGame + "/" + idUser + "|" + dinero2;
                                    byte[] msge = System.Text.Encoding.ASCII.GetBytes(missatge);
                                    Thread.Sleep(200);
                                    server.Send(msge);

                                    // Mensaje para actualizar los creditos:
                                    // Formato: '59/user-idGame/ficha|creditos'
                                    string MNSJ = "59/" + user + "-" + idGame + "/" + idUser + "|" + creditos2;
                                    byte[] mnsj = System.Text.Encoding.ASCII.GetBytes(MNSJ);
                                    Thread.Sleep(200);
                                    server.Send(mnsj);

                                    // Nos avisamos a nosotros mismos para confirmar:
                                    notificacion.Text = "[Casilla " + Asuntos[nC] + "]---> Has comprado por " + precioComprar + " Euros " + Asuntos[nC] + ". Se te han sumado " + creditos + " Creditos.";

                                    // NOTIFICACIÓN:
                                    // ---> Avisamos a los demás de que has comprado (aprobado) una asignatura.
                                    // Formato: '52/Erik-1/[JUGADOR 1: Erik] -> Ha aprobado comprado por +precioComprar+ Euros asignatura. Se le han sumado +creditos+ Creditos.'
                                    string mensaje = "52/" + user + "-" + idGame + "/" + "[JUGADOR " + JugadorID + ": " + user + "] -> Ha aprobado comprado por " + precioComprar + " Euros " + Asuntos[nC] + ". Se le han sumado " + creditos + " Creditos.";
                                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                                    Thread.Sleep(200);
                                    server.Send(msg);

                                    // Actualizar el data grid view de las materias
                                    ActualizaGridMaterias();
                                    break;

                                case DialogResult.No:
                                    // No hace nada
                                    break;
                            }
                        }
                    }

                    // LO MISMO PARA USER 3:

                    else if (idUser == 3) // La ficha que ha caido en esa casilla es del jugador 1:
                    {
                        if (precioComprar <= dinero1) // El usuario tiene suficiente dinero para comprar la casilla:
                        {
                            // Preguntamos al jugador si quiere comprar (aprobar) la asignatura:
                            DialogResult dr = MessageBox.Show("Quieres matricular " + Asuntos[nC] + "? [Precio: " + Precios[nC] + "] [Créditos: " + Creditos[nC] + "]", "Matrícula EETAC", MessageBoxButtons.YesNo);
                            switch (dr)
                            {
                                case DialogResult.Yes:

                                    // Compramos la asignatura:
                                    dinero3 = dinero3 - precioComprar; // Se le quita el dinero al usuario.
                                    money3.Text = Convert.ToString(dinero3); // Actualizamos el dinero que tiene este usuario.
                                    creditos3 = creditos3 + creditos; // Se le suman los creditos al usuario.
                                    credito3.Text = Convert.ToString(creditos3); // Actualizamos los creditos que tiene este usuario.
                                    Owners[nC] = nombre3; // La casilla pasa a tener propietario.

                                    // Avisamos a los demás clientes de cual es el nuevo propietario de la asignatura:
                                    // Formato: '61/user-idGame/user|idPosOwners'
                                    int idPosOwners = nC;
                                    string aviso = "61/" + user + "-" + idGame + "/" + user + "|" + idPosOwners;
                                    byte[] mstge = System.Text.Encoding.ASCII.GetBytes(aviso);
                                    Thread.Sleep(200);
                                    server.Send(mstge);

                                    // Mensaje para actualizar el dinero:
                                    // Formato: '58/user-idGame/ficha|dinero'
                                    string missatge = "58/" + user + "-" + idGame + "/" + idUser + "|" + dinero3;
                                    byte[] msge = System.Text.Encoding.ASCII.GetBytes(missatge);
                                    Thread.Sleep(200);
                                    server.Send(msge);

                                    // Mensaje para actualizar los creditos:
                                    // Formato: '59/user-idGame/ficha|creditos'
                                    string MNSJ = "59/" + user + "-" + idGame + "/" + idUser + "|" + creditos3;
                                    byte[] mnsj = System.Text.Encoding.ASCII.GetBytes(MNSJ);
                                    Thread.Sleep(200);
                                    server.Send(mnsj);

                                    // Nos avisamos a nosotros mismos para confirmar:
                                    notificacion.Text = "[Casilla " + Asuntos[nC] + "]---> Has comprado por " + precioComprar + " Euros) " + Asuntos[nC] + ". Se te han sumado " + creditos + " Creditos.";

                                    // NOTIFICACIÓN:
                                    // ---> Avisamos a los demás de que has comprado (aprobado) una asignatura.
                                    // Formato: '52/Erik-1/[JUGADOR 1: Erik] -> Ha aprobado comprado por +precioComprar + Euros asignatura. Se le han sumado +creditos+ Creditos.'
                                    string mensaje = "52/" + user + "-" + idGame + "/" + "[JUGADOR " + JugadorID + ": " + user + "] -> Ha comprado por " + precioComprar + " Euros) " + Asuntos[nC] + ". Se le han sumado " + creditos + " Creditos.";
                                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                                    Thread.Sleep(200);
                                    server.Send(msg);

                                    // Actualizar el data grid view de las materias
                                    ActualizaGridMaterias();
                                    break;

                                case DialogResult.No:
                                    // No hace nada
                                    break;
                            }
                        }
                    }

                    // LO MISMO PARA USER 4:

                    else if (idUser == 4) // La ficha que ha caido en esa casilla es del jugador 1
                    {
                        if (precioComprar <= dinero4) // El usuario tiene suficiente dinero para comprar la casilla
                        {
                            // Preguntamos al jugador si quiere comprar (aprobar) la asignatura
                            DialogResult dr = MessageBox.Show("Quieres matricular " + Asuntos[nC] + "? [Precio: " + Precios[nC] + "] [Créditos: " + Creditos[nC] + "]", "Matrícula EETAC", MessageBoxButtons.YesNo);
                            switch (dr)
                            {
                                case DialogResult.Yes:

                                    // Compramos la asignatura
                                    dinero4 = dinero4 - precioComprar; // Se le quita el dinero al usuario.
                                    money4.Text = Convert.ToString(dinero4); // Actualizamos el dinero que tiene este usuario.
                                    creditos4 = creditos4 + creditos; // Se le suman los creditos al usuario.
                                    credito4.Text = Convert.ToString(creditos4); // Actualizamos los creditos que tiene este usuario.
                                    Owners[nC] = nombre4; // La casilla pasa a tener propietario.

                                    // Avisamos a los demás clientes de cual es el nuevo propietario de la asignatura:
                                    // Formato: "61/user-idGame/user|idPosOwners"
                                    int idPosOwners = nC;
                                    string aviso = "61/" + user + "-" + idGame + "/" + user + "|" + idPosOwners;
                                    byte[] mstge = System.Text.Encoding.ASCII.GetBytes(aviso);
                                    Thread.Sleep(200);
                                    server.Send(mstge);

                                    // Mensaje para actualizar el dinero:
                                    // Formato: "58/user-idGame/ficha|dinero"
                                    string missatge = "58/" + user + "-" + idGame + "/" + idUser + "|" + dinero4;
                                    byte[] msge = System.Text.Encoding.ASCII.GetBytes(missatge);
                                    Thread.Sleep(200);
                                    server.Send(msge);

                                    // Mensaje para actualizar los creditos
                                    // Formato: "59/user-idGame/ficha|creditos"
                                    string MNSJ = "59/" + user + "-" + idGame + "/" + idUser + "|" + creditos4;
                                    byte[] mnsj = System.Text.Encoding.ASCII.GetBytes(MNSJ);
                                    Thread.Sleep(200);
                                    server.Send(mnsj);

                                    // Nos avisamos a nosotros mismos para confirmar:
                                    notificacion.Text = "[Casilla " + Asuntos[nC] + "]---> Has aprobado (ehem, comprado por " + precioComprar + " Euros) " + Asuntos[nC] + ". Se te han sumado " + creditos + " Creditos.";

                                    // NOTIFICACIÓN
                                    // ---> Avisamos a los demás de que has comprado (aprobado) una asignatura
                                    // Formato: "52/Erik-1/[JUGADOR 1: Erik] -> Ha comprado por +precioComprar+ Euros asignatura. Se le han sumado +creditos+ Creditos"
                                    string mensaje = "52/" + user + "-" + idGame + "/" + "[JUGADOR " + JugadorID + ": " + user + "] -> Ha aprobado (ehem, comprado por " + precioComprar + " Euros) " + Asuntos[nC] + ". Se le han sumado " + creditos + " Creditos.";
                                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                                    Thread.Sleep(200);
                                    server.Send(msg);

                                    // Actualizar el data grid view de las materias
                                    ActualizaGridMaterias();
                                    break;

                                case DialogResult.No:
                                    // No hace nada
                                    break;
                            }
                        }
                    }
                }
            }
        }   

        //  Localizacion de cada casilla 
        int getCoordinate(string name)
        {
            // Vector con todas las coordenadas encontradas una a una
            int[] listaCoordenadas = { 884, 886, 948, 886, 884, 943, 948, 943, 792, 897, 828, 896, 793, 943, 827, 943, 712, 884, 745, 884, 713, 943, 746, 943, 629, 897, 666, 898, 631, 943, 665, 943, 557, 884, 584, 884, 557, 930, 581, 930, 472, 875, 506, 875, 468, 930, 504, 930, 388, 897, 419, 901, 387, 943, 424, 943, 306, 886, 339, 886, 303, 930, 340, 930, 220, 897, 256, 897, 220, 943, 259, 943, 143, 900, 174, 899, 142, 943, 174, 943, 20, 886, 80, 886, 22, 930, 83, 930, 69, 795, 68, 828, 17, 795, 15, 829, 87, 714, 88, 734, 27, 709, 23, 734, 73, 631, 71, 663, 21, 631, 23, 665, 64, 554, 65, 580, 19, 554, 16, 580, 85, 466, 86, 497, 23, 466, 22, 497, 72, 387, 69, 418, 19, 387, 14, 421, 84, 304, 84, 333, 25, 314, 26, 339, 69, 225, 69, 252, 21, 224, 20, 252, 71, 141, 66, 178, 18, 140, 16, 178, 76, 17, 76, 78, 23, 27, 24, 78, 183, 67, 147, 67, 174, 17, 146, 17, 259, 78, 229, 67, 263, 17, 229, 17, 341, 67, 306, 67, 340, 17, 303, 17, 422, 67, 388, 67, 418, 27, 389, 27, 504, 87, 469, 87, 501, 32, 468, 36, 584, 67, 557, 67, 588, 27, 557, 27, 666, 67, 634, 67, 667, 27, 663, 27, 750, 87, 717, 93, 746, 32, 714, 27, 830, 67, 792, 67, 830, 27, 797, 27, 948, 67, 884, 67, 948, 27, 884, 27, 897, 172, 899, 140, 948, 169, 948, 140, 903, 252, 903, 226, 948, 252, 948, 222, 897, 329, 897, 304, 948, 329, 948, 304, 902, 421, 902, 389, 948, 409, 948, 387, 884, 497, 884, 474, 928, 497, 927, 466, 884, 580, 884, 556, 948, 589, 948, 556, 903, 663, 903, 636, 948, 655, 948, 638, 884, 743, 884, 709, 939, 734, 939, 714, 898, 820, 899, 795, 948, 816, 948, 795 };
            // Vector con los nombres de las coordenadas.
            string[] listaNombres = { "j1c0_X", "j1c0_Y", "j2c0_X", "j2c0_Y", "j3c0_X", "j3c0_Y", "j4c0_X", "j4c0_Y", "j1c1_X", "j1c1_Y", "j2c1_X", "j2c1_Y", "j3c1_X", "j3c1_Y", "j4c1_X", "j4c1_Y", "j1c2_X", "j1c2_Y", "j2c2_X", "j2c2_Y", "j3c2_X", "j3c2_Y", "j4c2_X", "j4c2_Y", "j1c3_X", "j1c3_Y", "j2c3_X", "j2c3_Y", "j3c3_X", "j3c3_Y", "j4c3_X", "j4c3_Y", "j1c4_X", "j1c4_Y", "j2c4_X", "j2c4_Y", "j3c4_X", "j3c4_Y", "j4c4_X", "j4c4_Y", "j1c5_X", "j1c5_Y", "j2c5_X", "j2c5_Y", "j3c5_X", "j3c5_Y", "j4c5_X", "j4c5_Y", "j1c6_X", "j1c6_Y", "j2c6_X", "j2c6_Y", "j3c6_X", "j3c6_Y", "j4c6_X", "j4c6_Y", "j1c7_X", "j1c7_Y", "j2c7_X", "j2c7_Y", "j3c7_X", "j3c7_Y", "j4c7_X", "j4c7_Y", "j1c8_X", "j1c8_Y", "j2c8_X", "j2c8_Y", "j3c8_X", "j3c8_Y", "j4c8_X", "j4c8_Y", "j1c9_X", "j1c9_Y", "j2c9_X", "j2c9_Y", "j3c9_X", "j3c9_Y", "j4c9_X", "j4c9_Y", "j1c10_X", "j1c10_Y", "j2c10_X", "j2c10_Y", "j3c10_X", "j3c10_Y", "j4c10_X", "j4c10_Y", "j1c11_X", "j1c11_Y", "j2c11_X", "j2c11_Y", "j3c11_X", "j3c11_Y", "j4c11_X", "j4c11_Y", "j1c12_X", "j1c12_Y", "j2c12_X", "j2c12_Y", "j3c12_X", "j3c12_Y", "j4c12_X", "j4c12_Y", "j1c13_X", "j1c13_Y", "j2c13_X", "j2c13_Y", "j3c13_X", "j3c13_Y", "j4c13_X", "j4c13_Y", "j1c14_X", "j1c14_Y", "j2c14_X", "j2c14_Y", "j3c14_X", "j3c14_Y", "j4c14_X", "j4c14_Y", "j1c15_X", "j1c15_Y", "j2c15_X", "j2c15_Y", "j3c15_X", "j3c15_Y", "j4c15_X", "j4c15_Y", "j1c16_X", "j1c16_Y", "j2c16_X", "j2c16_Y", "j3c16_X", "j3c16_Y", "j4c16_X", "j4c16_Y", "j1c17_X", "j1c17_Y", "j2c17_X", "j2c17_Y", "j3c17_X", "j3c17_Y", "j4c17_X", "j4c17_Y", "j1c18_X", "j1c18_Y", "j2c18_X", "j2c18_Y", "j3c18_X", "j3c18_Y", "j4c18_X", "j4c18_Y", "j1c19_X", "j1c19_Y", "j2c19_X", "j2c19_Y", "j3c19_X", "j3c19_Y", "j4c19_X", "j4c19_Y", "j1c20_X", "j1c20_Y", "j2c20_X", "j2c20_Y", "j3c20_X", "j3c20_Y", "j4c20_X", "j4c20_Y", "j1c21_X", "j1c21_Y", "j2c21_X", "j2c21_Y", "j3c21_X", "j3c21_Y", "j4c21_X", "j4c21_Y", "j1c22_X", "j1c22_Y", "j2c22_X", "j2c22_Y", "j3c22_X", "j3c22_Y", "j4c22_X", "j4c22_Y", "j1c23_X", "j1c23_Y", "j2c23_X", "j2c23_Y", "j3c23_X", "j3c23_Y", "j4c23_X", "j4c23_Y", "j1c24_X", "j1c24_Y", "j2c24_X", "j2c24_Y", "j3c24_X", "j3c24_Y", "j4c24_X", "j4c24_Y", "j1c25_X", "j1c25_Y", "j2c25_X", "j2c25_Y", "j3c25_X", "j3c25_Y", "j4c25_X", "j4c25_Y", "j1c26_X", "j1c26_Y", "j2c26_X", "j2c26_Y", "j3c26_X", "j3c26_Y", "j4c26_X", "j4c26_Y", "j1c27_X", "j1c27_Y", "j2c27_X", "j2c27_Y", "j3c27_X", "j3c27_Y", "j4c27_X", "j4c27_Y", "j1c28_X", "j1c28_Y", "j2c28_X", "j2c28_Y", "j3c28_X", "j3c28_Y", "j4c28_X", "j4c28_Y", "j1c29_X", "j1c29_Y", "j2c29_X", "j2c29_Y", "j3c29_X", "j3c29_Y", "j4c29_X", "j4c29_Y", "j1c30_X", "j1c30_Y", "j2c30_X", "j2c30_Y", "j3c30_X", "j3c30_Y", "j4c30_X", "j4c30_Y", "j1c31_X", "j1c31_Y", "j2c31_X", "j2c31_Y", "j3c31_X", "j3c31_Y", "j4c31_X", "j4c31_Y", "j1c32_X", "j1c32_Y", "j2c32_X", "j2c32_Y", "j3c32_X", "j3c32_Y", "j4c32_X", "j4c32_Y", "j1c33_X", "j1c33_Y", "j2c33_X", "j2c33_Y", "j3c33_X", "j3c33_Y", "j4c33_X", "j4c33_Y", "j1c34_X", "j1c34_Y", "j2c34_X", "j2c34_Y", "j3c34_X", "j3c34_Y", "j4c34_X", "j4c34_Y", "j1c35_X", "j1c35_Y", "j2c35_X", "j2c35_Y", "j3c35_X", "j3c35_Y", "j4c35_X", "j4c35_Y", "j1c36_X", "j1c36_Y", "j2c36_X", "j2c36_Y", "j3c36_X", "j3c36_Y", "j4c36_X", "j4c36_Y", "j1c37_X", "j1c37_Y", "j2c37_X", "j2c37_Y", "j3c37_X", "j3c37_Y", "j4c37_X", "j4c37_Y", "j1c38_X", "j1c38_Y", "j2c38_X", "j2c38_Y", "j3c38_X", "j3c38_Y", "j4c38_X", "j4c38_Y", "j1c39_X", "j1c39_Y", "j2c39_X", "j2c39_Y", "j3c39_X", "j3c39_Y", "j4c39_X", "j4c39_Y" };

            int coordinate = 0;

            for (int i = 0; i < listaNombres.Length; i++)
            {
                if (name == listaNombres[i])
                {
                    coordinate = listaCoordenadas[i];
                    break;
                }
            }
            return coordinate;
        }

        // Funcion que pone los propietarios de cada materia
        public void ActualizaGridMaterias()
            {
                MiMatricula.Columns.Clear();
                MiMatricula.ColumnCount = 2;
                MiMatricula.Columns[0].HeaderText = "POS.";
                MiMatricula.Columns[1].HeaderText = "OWNER";
                MiMatricula.EnableHeadersVisualStyles = false;
                string nombreCasilla = "";
                string propietario = "";
                for (int i = 0; i < Precios.Length; i++)
                {
                    nombreCasilla = Asuntos[i];
                    propietario = Owners[i];
                    if (Subject[i] == true) // Comprobamos que sea una materia 
                    {
                        MiMatricula.Rows.Add(nombreCasilla, propietario); // Añadimos la materia
                    }
                }
                MiMatricula.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                MiMatricula.ClearSelection();
            }      

        // Funcion para mover una ficha
        public void moverFicha(int desplazamiento)
        {
            int numMov = desplazamiento; // Indica cuantas casillas se desplaza
            string PosX = ""; // Por ejemplo, 'j1c12_X'.
            string PosY = ""; // Por ejemplo, 'j1c12_Y'.

            if (JugadorID == 1)
            {
                user = "Erik";
                casilla1 = casilla1 + numMov;
                if (casilla1 >= 40) // Cuando llegas o pasas por salida
                {
                    casilla1 = casilla1 - 40;
                    // Bonificamos al jugador por el paso por meta
                    dinero1 = dinero1 + 200;
                    money1.Text = Convert.ToString(dinero1);
                    // NOTIFICACIÓN:
                    // Avisamos a los demás del paso por casilla
                    // Formato: '52/Erik-1/[JUGADOR 1: Erik] -> Ha pasado por meta. Gana 200 Euros.'
                    string mensaje = "52/" + user + "-" + idGame + "/" + "[JUGADOR " + JugadorID + ": " + user + "] -> Ha pasado por meta. Gana 200 Euros.";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);                 
                    server.Send(msg);
                }
                else if (casilla1 == 30) // Si caes en el segurata
                {
                    // Te manda a la cárcel:
                    casilla1 = 10;
                    // NOTIFICACIÓN:
                    // ---> Avisamos a los demás de que has sido enviado a la cárcel
                    // Formato: "52/Erik-1/[JUGADOR 1: Erik] -> Ha suspendido todas las materias, el segurata lo ha mandado a la carcel"
                    string mensaje = "52/" + user + "-" + idGame + "/" + "[JUGADOR " + JugadorID + ": " + user + "] -> Ha suspendido todas las materias, el segurata lo ha mandado a la carcel";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                PosX = $"j{JugadorID}c{casilla1}_X";
                PosY = $"j{JugadorID}c{casilla1}_Y";
            }

            else if (JugadorID == 2)
            {
                user = "Toni";
                casilla2 = casilla2 + numMov;
                if (casilla2 >= 40) // Cuando llegas o pasas por salida
                {
                    casilla2 = casilla2 - 40;
                    // Bonificamos al jugador por el paso por meta
                    dinero2 = dinero2 + 200;
                    money2.Text = Convert.ToString(dinero2);
                    // NOTIFICACIÓN:
                    // ---> Avisamos a los demás del paso por casilla.
                    // Formato: '52/Erik-1/[JUGADOR 1: Erik] -> Ha pasado por meta. Gana 200 Euros.'
                    string mensaje = "52/" + user + "-" + idGame + "/" + "[JUGADOR " + JugadorID + ": " + user + "] -> Ha pasado por meta. Gana 200 Euros.";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else if (casilla2 == 30) // Si caes en el segurata
                {
                    // Te manda a la cárcel:
                    casilla2 = 10;
                    // NOTIFICACIÓN:
                    // ---> Avisamos a los demás de que has sido enviado a la cárcel.
                    // Formato: "52/Erik-1/[JUGADOR 1: Erik] -> Ha suspendido todas las materias, el segurata lo ha mandado a la carcel"
                    string mensaje = "52/" + user + "-" + idGame + "/" + "[JUGADOR " + JugadorID + ": " + user + "] -> Ha suspendido todas las materias, el segurata lo ha mandado a la carcel";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                PosX = $"j{JugadorID}c{casilla2}_X";
                PosY = $"j{JugadorID}c{casilla2}_Y";
            }

            else if (JugadorID == 3)
            {
                user = "Andrei";
                casilla3 = casilla3 + numMov;
                if (casilla3 >= 40) // Cuando llegas o pasas por salida
                {
                    casilla3 = casilla3 - 40;
                    // Bonificamos al jugador por el paso por meta:
                    dinero3 = dinero3 + 200;
                    money3.Text = Convert.ToString(dinero3);
                    // NOTIFICACIÓN:
                    // ---> Avisamos a los demás del paso por casilla.
                    // Formato: '52/Erik-1/[JUGADOR 1: Erik] -> Ha pasado por meta. Gana 200 Euros.'
                    string mensaje = "52/" + user + "-" + idGame + "/" + "[JUGADOR " + JugadorID + ": " + user + "] -> Ha pasado por meta. Gana 200 Euros.";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else if (casilla3 == 30) // Si caes en el segurata
                {
                    // Te manda a la cárcel:
                    casilla3 = 10;
                    // NOTIFICACIÓN:
                    // ---> Avisamos a los demás de que has sido enviado a la cárcel.
                    // Formato: "52/Erik-1/[JUGADOR 1: Erik] -> Ha suspendido todas las materias, el segurata lo ha mandado a la carcel"
                    string mensaje = "52/" + user + "-" + idGame + "/" + "[JUGADOR " + JugadorID + ": " + user + "] -> Ha suspendido todas las materias, el segurata lo ha mandado a la carcel";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                PosX = $"j{JugadorID}c{casilla3}_X";
                PosY = $"j{JugadorID}c{casilla3}_Y";
            }

            else if (JugadorID == 4)
            {
                casilla4 = casilla4 + numMov;
                if (casilla4 >= 40) // Cuando llegas o pasas por salida
                {
                    casilla4 = casilla4 - 40;
                    // Bonificamos al jugador por el paso por meta:
                    dinero4 = dinero4 + 200;
                    money4.Text = Convert.ToString(dinero4);
                    // NOTIFICACIÓN:
                    // ---> Avisamos a los demás del paso por casilla.
                    // Formato: "52/Marc-1/[JUGADOR 1: Erik] -> Ha pasado por meta. Gana 200 Euros."
                    string mensaje = "52/" + user + "-" + idGame + "/" + "[JUGADOR " + JugadorID + ": " + user + "] -> Ha pasado por meta. Gana 200 Euros.";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else if (casilla4 == 30) // Si caes en el segurata
                {
                    // Te manda a la cárcel:
                    casilla4 = 10;
                    // NOTIFICACIÓN:
                    // ---> Avisamos a los demás de que has sido enviado a la cárcel
                    // Formato: "52/Erik-1/[JUGADOR 1: Erik] -> Ha suspendido todas las materias, el segurata lo ha mandado a la carcel"
                    string mensaje = "52/" + user + "-" + idGame + "/" + "[JUGADOR " + JugadorID + ": " + user + "] -> Ha suspendido todas las materias, el segurata lo ha mandado a la carcel";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                PosX = $"j{JugadorID}c{casilla4}_X";
                PosY = $"j{JugadorID}c{casilla4}_Y";
            }

            // Enviamos la nueva posicion de la ficha a los demas
            // 'PosX' es un string con formato 'j1c38_X'.
            // 'PosY' es un string con formato 'j1c38_Y'.
            // Formato del mensaje: '54/Erik-1/numFicha|PosX|PosY'.
            string aviso = "54/" + user + "-" + idGame + "/" + JugadorID + "|" + PosX + "|" + PosY;
            byte[] msge = System.Text.Encoding.ASCII.GetBytes(aviso);
            server.Send(msge);

            int posX = getCoordinate(PosX); // Obtenemos la coodenada X
            int posY = getCoordinate(PosY); // Obtenemos la coordenada Y

            if (JugadorID == 1)
            {
                user = "Erik";
                ficha1.Location = new Point(posX, posY);
                CasillaNueva(casilla1, Convert.ToInt32(JugadorID));
            }
            else if (JugadorID == 2)
            {
                user = "Toni";
                ficha2.Location = new Point(posX, posY);
                CasillaNueva(casilla2, Convert.ToInt32(JugadorID));
            }
            else if (JugadorID == 3)
            {
                user = "Andrei";
                ficha3.Location = new Point(posX, posY);
                CasillaNueva(casilla3, Convert.ToInt32(JugadorID));
            }
            else if (JugadorID == 4)
            {
                ficha4.Location = new Point(posX, posY);
                CasillaNueva(casilla4, Convert.ToInt32(JugadorID));
            }
        }

    }
}