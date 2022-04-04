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

namespace Cliente
{
    public partial class Form1 : Form
    {
        Socket server;

        public Form1()
        {
            InitializeComponent();
        }


        private void btnDesconectar_Click(object sender, EventArgs e)
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


        private void btnConectar_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.101");
            IPEndPoint ipep = new IPEndPoint(direc, 9050);


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
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

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
                    MessageBox.Show("Este jugador se ha registrado");
                else
                    MessageBox.Show("Este jugador ya está registrado");
            }
            else
            {
                MessageBox.Show("¡¡El nombre o la contraseña debe tener más de un carácter!!");

            }
        }
    }
}
