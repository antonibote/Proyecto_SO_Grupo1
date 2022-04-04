using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
<<<<<<< HEAD
=======
using System.Threading;
>>>>>>> 15be607 (Version 2)

namespace Cliente
{
    public partial class Form1 : Form
    {
<<<<<<< HEAD
=======

        public bool conectado = false;
        public int ID_J;
>>>>>>> 15be607 (Version 2)
        Socket server;

        public Form1()
        {
            InitializeComponent();
        }


        private void btnDesconectar_Click(object sender, EventArgs e)
        {
<<<<<<< HEAD
            //Mensaje de desconexión
            string mensaje = "0/";

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
=======
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
            btn_lista_conectados.Visible = false;
            btnConectar.Enabled = true;
            numConsultas.Visible = false;
            contConsultas.Visible = false;
            //Mensaje de desconexión
            string mensaje = "0/";

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Nos desconectamos
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            
>>>>>>> 15be607 (Version 2)
        }


        private void btnConectar_Click(object sender, EventArgs e)
        {
<<<<<<< HEAD
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.101");
            IPEndPoint ipep = new IPEndPoint(direc, 9050);
=======
            username.Visible = true;
            password.Visible = true;
            username.Enabled = true;
            password.Enabled = true;
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
            IPAddress direc = IPAddress.Parse("192.168.56.101");
            IPEndPoint ipep = new IPEndPoint(direc, 9080);
>>>>>>> 15be607 (Version 2)


            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");

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
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();

        }

<<<<<<< HEAD
        private void ConsultarBD_Click(object sender, EventArgs e)
        {
            Form consultas = new Form2();
            consultas.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((username.Text != "") && (password.Text != ""))
            {
                string mensaje = "7/" + username.Text + "/" + password.Text;
=======
        private void EnviarBtn_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                string mensaje = "1/" + dia.Text;
                // Enviamos al servidor el dia tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                MessageBox.Show("El nombre del ganador y la duración de la partida es: " + mensaje);

            }
            else if (radioButton2.Checked)
            {
                string mensaje = "10/" + nombre.Text;
>>>>>>> 15be607 (Version 2)
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
<<<<<<< HEAD

                if (mensaje != "NO")
                {
                   
                    MessageBox.Show("Pues quieres jugar eeee....UwU");
                }
                else
                {
                    MessageBox.Show("Usuario mal escritooooo, escriba bien el usuario y la contraseña, o " +
                         "Registrate para poder jugar a este maravilloso juego");
                }

            }
            else
            {
                MessageBox.Show("Los campos de login están vacios");
            }
        }

        private void Registrar_Click(object sender, EventArgs e)
=======
                MessageBox.Show("Las partidas ganadas de este jugador son: " + mensaje);
            }
            else if (radioButton3.Checked)
            {
                // Enviamos el nombre
                string mensaje = "11/" + nombre.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                MessageBox.Show("Este jugador ha jugado el siguiente número de partidas: " + mensaje);
            }
            else if (radioButton4.Checked)
            {
                string mensaje = "12/" + username.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                MessageBox.Show("La ID del jugador es: " + mensaje);

            }
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
>>>>>>> 15be607 (Version 2)
        {
            if (((username.Text.Length > 1 && password.Text.Length > 1)) && ((username.Text != "") && (password.Text != "")))
            {
                string mensaje = "8/" + username.Text + "/" + password.Text;
                // // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                if (mensaje == "SI")
<<<<<<< HEAD
                    MessageBox.Show("Este jugador se ha registrado");
=======
                {
                    BtnRegistrar.Enabled = false;              
                    MessageBox.Show("Jugador registrado correctamente, ya puedes iniciar sesión");
                }                   
>>>>>>> 15be607 (Version 2)
                else
                    MessageBox.Show("Este jugador ya está registrado");
            }
            else
            {
<<<<<<< HEAD
                MessageBox.Show("¡¡El nombre o la contraseña debe tener más de un carácter!!");

            }
        }
=======
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

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                    if (mensaje != "NO")
                    {
                    BtnLogin.Enabled = false;
                    peticion.Visible = true;                    
                    btn_lista_conectados.Visible = true;
                    BtnRegistrar.Enabled = false;
                    numConsultas.Visible = true;
                    contConsultas.Visible = true;
                    ID_J = Convert.ToInt32(mensaje);
                        MessageBox.Show("Sesión iniciada correctamente");
                    }
                    else
                    {
                        MessageBox.Show("Usuario mal escrito, escriba bien el usuario y la contraseña, o " +
                             "registrate para poder jugar a este maravilloso juego");
                    }

                }
                else
                {
                    MessageBox.Show("El campo de usuario o contraseña está vacio");
                }
        }

        private void btn_lista_conectados_Click(object sender, EventArgs e)
        {

                dataGridView1.Visible = true;
                dataGridView1.Rows.Clear();

                //Queremos saber la lista de conectados
                string mensaje = "13/";
                //Enviamos en el servidor el nombre como un bytes
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                //Convertir los bytes en ASCII (proceso inverso)
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                if (mensaje != null && mensaje != "")
                {
                    string[] porcadausername = mensaje.Split(',');
                    int num = Convert.ToInt32(porcadausername[0]);
                    int i = 0;
                    while (i < num)
                    {
                        int x = dataGridView1.Rows.Add();
                        dataGridView1.Rows[x].Cells[0].Value = porcadausername[i+1];
                        i++;
                    }
                }
                else
                {
                    MessageBox.Show("Ha habido algo raro");
                }
           
        }

        private void MostrarPass_CheckedChanged(object sender, EventArgs e)
        {
            if (MostrarPass.Checked == true)
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
>>>>>>> 15be607 (Version 2)
    }
}
