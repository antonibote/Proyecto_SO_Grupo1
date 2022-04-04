using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace Cliente
{
    public partial class Form2 : Form
    {
        int nForm;
        Socket server;

        public Form2()
        {
        }

        public Form2(int nForm, Socket server)
        {
            InitializeComponent();
            this.nForm = nForm;
            this.server = server;
        }

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
                string mensaje = "2/" + nombre.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                MessageBox.Show("Las partidas ganadas de este jugador son: " + mensaje);
            }
            else if (radioButton3.Checked)
            {
                // Enviamos el nombre
                string mensaje = "3/" + nombre.Text;
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
                string mensaje = "4/" + nombre.Text;
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

   
    }
}
