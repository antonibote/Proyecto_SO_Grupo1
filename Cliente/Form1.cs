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

        public int ID_J;
        Socket server;
        Thread atender_mensajes_servidor;
        delegate void DelegadoParaEscribir(string mensaje);
        delegate void DelegadoParaDataGridView(DataGridView mensaje);
        int jugadoresPartida; //
        int cont; //Contador de jugadores que hemos invitado
        int cont2 = 0; //Contador para contar respuestas de los que he invitado
        bool Jugamos = true; //Bool para mirar si al final se juega o no
        int idpartida;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            username.Enabled = true;
            password.Enabled = true;
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
            numConsultas.Visible = false;
            contConsultas.Visible = false;

            //Mensaje de desconexión
            string mensaje = "0/"+ username.Text;

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos y thread tambien lo desactivamos
            atender_mensajes_servidor.Abort();
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            
        }


        private void btnConectar_Click(object sender, EventArgs e)
        {
            username.Visible = true;
            password.Visible = true;
            BtnRegistrar.Enabled = true;
            BtnLogin.Enabled = true;
            label1.Visible = true;
            label2.Visible = true;
            MostrarPass.Visible = true;
            BtnRegistrar.Visible = true;
            BtnLogin.Visible = true;
            btnConectar.Enabled = false;
            btnDesconectar.Visible = true;
            label5.Visible = true;
            label6.Visible = true;

            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.101"); //192.168.56.102
            IPEndPoint ipep = new IPEndPoint(direc, 9080);

           // IPAddress direc = IPAddress.Parse("147.83.117.22");
           // IPEndPoint ipep = new IPEndPoint(direc, 50002);


            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                MessageBox.Show("Conectado");

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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Mensaje de desconexión
            string mensaje = "0/";

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos
            atender_mensajes_servidor.Abort();
            username.Clear();
            password.Clear();
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }

        //ESTO SE TIENE QUE MIRAR 
        public void PonerDataGridView (string mensaje) 
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

        public void PonerVisibleDataGridView(DataGridView name)
        {
            name.Visible = true;
        }

        public void PonerNOVisibleDataGridView(DataGridView name)
        {
            name.Visible = false;
        }


        private void Atender_Peticiones_Servidor()
        {
            while (true) 
            {
                //Recibimos un mensaje del servidor
                //byte[] msg2 = new byte[80];
                //server.Receive(msg2);
                //string[] trozos = Encoding.ASCII.GetString(msg2).Split('/');
                //int codigo = Convert.ToInt32(trozos[0]);
                //string mensaje = trozos[1].Split('\0')[0];

                //byte[] msg2 = new byte[80];
                //server.Receive(msg2);
                //string mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                //string[] trozos = mensaje.Split('/');
                //int codigo = Convert.ToInt32(trozos[0]);

                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string[] trozos = Encoding.ASCII.GetString(msg2).Split('\0');
                string[] trozos1 = trozos[0].Split('/');
                int codigo = Convert.ToInt32(trozos1[0]);
                string mensaje = trozos1[1];
         

                switch (codigo)
                {
                    case 10:  //Respuesta de la consulta partidas ganadas por username
                        if (mensaje == "mal") //cambiar a palabra ya lo veremos.
                        {
                            MessageBox.Show(nombre.Text + " no existe o no ha ganado ninguna partida");
                        }
                        else 
                        {
                            MessageBox.Show(nombre.Text + " ha ganado " + mensaje + " partidas");
                        }
                        break;

                    case 11: //Respuesta de la consulta cantidad de partidas jugadas por username
                        if (mensaje == "mal")
                        {
                            MessageBox.Show(nombre.Text + " no existe o no ha jugado ninguna partida");
                        }
                        else 
                        {
                            MessageBox.Show(nombre.Text + " ha jugado " + mensaje + " partidas");
                        }
                        break;

                    case 12: //Respuesta de la consulta dame el ID de tal username 
                        if (mensaje == "mal") 
                        {
                            MessageBox.Show(nombre.Text + " no existe o no ha iniciado sesión");
                        }
                        else 
                        {
                            MessageBox.Show("El ID de " + nombre.Text + " es: " + mensaje);
                        }
                        break;

                    case 8: //Respuesta a registrarse
                        if (mensaje == "SI") 
                        {
                            MessageBox.Show("Registrado");
                        }
                        else
                        {
                            MessageBox.Show("Usuario ya esta registrado, ponga otro usuario");
                        }
                        break;

                    case 7: //Respuesta al iniciar sesión
                        if (mensaje != "NO")
                        {
                            ID_J = Convert.ToInt32(mensaje);
                            MessageBox.Show("Te estabámos esperando");
                            DelegadoParaDataGridView delegado112 = new DelegadoParaDataGridView(PonerVisibleDataGridView);
                            dataGridView1.Invoke(delegado112, new object[] { dataGridView1 });
                            Invoke(new Action(() =>
                            {
                                dataGridView1.Refresh();
                                peticion.Enabled = true;
                                peticion.Visible = true;
                                BtnLogin.Enabled = false;
                                BtnRegistrar.Enabled = false;
                            }));
                           
                            //INVOKE... PETICIONES/CONSULTAS
                        }
                        else 
                        {
                            MessageBox.Show("Usuario no encontrado, escriba bien el usuario y contraseña. Sino registrar que es gratis");
                        }
                        break;

                    case 13: //Respuesta de lista de conectados

                        DelegadoParaEscribir delegado13 = new DelegadoParaEscribir(PonerDataGridView);
                        dataGridView1.Invoke(delegado13, new object[] { mensaje });
                        break;

                    case 14:
                        //Invitación a jugar. Recibimos 14/nombreanfitrion/id partida
                        DialogResult aceptaorechaza = MessageBox.Show($" {mensaje} quiere jugar contigo, ¿Aceptas?", "Invitación", MessageBoxButtons.YesNo);
                        if (aceptaorechaza == DialogResult.Yes)
                        {
                            string mensaje2 = "15/SI/"+trozos1[2];
                            byte[] msg = Encoding.ASCII.GetBytes(mensaje2);
                            server.Send(msg);
                        }
                        else if (aceptaorechaza == DialogResult.No)
                        {
                            string mensaje2 = "15/NO/"+trozos1[2];
                            byte[] msg = Encoding.ASCII.GetBytes(mensaje2);
                            server.Send(msg);
                        }
                        break;

                    case 15:
                        //Respuesta a la invitacíon, 15/SI/idpartida o 15/NO/idpartida
                        cont2++;
                        if (mensaje == "SI")
                        {
                            MessageBox.Show("Invitación aceptada, a jugar!", username.Text);
                        }
                        else
                        {
                            MessageBox.Show("Invitación rechazada");
                            Jugamos = false;
                        }
                        if (cont2 == cont)
                        {
                            if (Jugamos == true)
                            {
                                //Si jugamos
                                string mensaje2 = $"16/SI/{trozos1[2]}";
                                byte[] msg = Encoding.ASCII.GetBytes(mensaje2);
                                server.Send(msg);
                            }
                            else
                            {
                                //No jugamos
                                string mensaje2 = $"16/NO/{trozos1[2]}";
                                byte[] msg = Encoding.ASCII.GetBytes(mensaje2);
                                server.Send(msg);
                            }
                        }
                        break;
                    case 17: //Jugamos partida o no

                        if (trozos[1] == "SI")
                        {
                            idpartida = Convert.ToInt32(trozos1[2]);
                            MessageBox.Show("Jugamos", username.Text);
                            Invoke(new Action(() =>
                            {
                                chat.Visible = true;
                            }));
                        }
                        else
                        {
                            MessageBox.Show("No Jugamos", username.Text);
                        }
                        break;

                    case 16: //16/mensaje/usuario/idpartida
                        Invoke(new Action(() =>
                        {
                            listBox1.Items.Add($"{trozos1[2]}: {trozos1[1]}");

                        }));

                        break;
                }
            }

        }

        private void EnviarBtn_Click(object sender, EventArgs e)
        {
            if (nombre.Text.Length > 1)
            {
                if (radioButton1.Checked)
                {
                    string mensaje = "9/" + dia.Text;
                    // Enviamos al servidor el dia tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else if (radioButton2.Checked)
                {
                    string mensaje = "10/" + nombre.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else if (radioButton3.Checked)
                {
                    // Enviamos el nombre
                    string mensaje = "11/" + nombre.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else if (radioButton4.Checked)
                {
                    string mensaje = "12/" + nombre.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
            }
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
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

        private void BtnLogin_Click(object sender, EventArgs e)
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


        private void MostrarPass_CheckedChanged(object sender, EventArgs e)
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

        private void numConsultas_Click(object sender, EventArgs e)
        {
            // Pedir numero de servicios realizados
            string mensaje = "20/";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
            contConsultas.Text = mensaje;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            peticion.Visible = false;
            dataGridView1.Visible = false;
        }

        private void BtnInvitar_Click(object sender, EventArgs e)
        {
            string mensaje = "14/" + dataGridView1.CurrentCell.Value.ToString();
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mensaje_clean = textBox1.Text.Replace("/", "\\");
            string mensaje = $"16/{mensaje_clean}/{username.Text}/{idpartida}";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

        }
    }
}
