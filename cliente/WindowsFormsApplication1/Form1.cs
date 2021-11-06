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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Socket server;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            DesconectarButton.Visible = false;
            groupBox1.Visible = false;
            this.WindowState = FormWindowState.Maximized;
        }

        private void LogInButton_Click(object sender, EventArgs e)
        {
            if (usuario.Text != "" && password.Text != "" && (Login.Checked == true || Register.Checked == true))
            {
                {
                    //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
                    //al que deseamos conectarnos
                    IPAddress direc = IPAddress.Parse("192.168.56.102");
                    IPEndPoint ipep = new IPEndPoint(direc, 9050);
                    //Creamos el socket 
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {

                        server.Connect(ipep);//Intentamos conectar el socket
                        if (Login.Checked)
                        {
                            // Quiere loguearse
                            string mensaje = "1/" + usuario.Text + "/" + password.Text;
                            // Enviamos al servidor el nombre tecleado
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                            server.Send(msg);

                            //Recibimos la respuesta del servidor
                            byte[] msg2 = new byte[80];
                            server.Receive(msg2);
                            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                            int res = Convert.ToInt32(mensaje);
                            if (res == 0)
                            {
                                MessageBox.Show("Logueado correctamente.");
                                this.BackColor = Color.Green;
                                groupBox1.Visible = true;
                                DesconectarButton.Visible = true;
                                groupBox2.Visible = false;
                            }
                            else if (res == -1)
                            {
                                MessageBox.Show("Error al consultar la base de datos.");
                            }
                            else
                            {
                                MessageBox.Show("No es posible loguearse. Inténtelo de nuevo.");
                            }
                        }
                        else if (Register.Checked)
                        {
                            // Quiere registrarse
                            string mensaje = "2/" + usuario.Text + "/" + password.Text;
                            // Enviamos al servidor el nombre tecleado
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                            server.Send(msg);

                            //Recibimos la respuesta del servidor
                            byte[] msg2 = new byte[80];
                            server.Receive(msg2);
                            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                            int res = Convert.ToInt32(mensaje);
                            if (res == 0)
                            {
                                MessageBox.Show("Registrado correctamente.");
                            }
                            else if (res == -1)
                            {
                                MessageBox.Show("Error al consultar la base de datos.");
                            }
                            else
                            {
                                MessageBox.Show("No es posible registrarse. Inténtelo de nuevo.");
                            }
                        }
                    }
                    catch (SocketException)
                    {
                        //Si hay excepcion imprimimos error y salimos del programa con return 
                        MessageBox.Show("No he podido conectar con el servidor.");
                        return;
                    }
                }
            }
            if (usuario.Text == "" || password.Text == "")
            {
                MessageBox.Show("Introduzca todos los datos en los campos usuario y contraseña.");
            }
            if (Login.Checked == false && Register.Checked == false)
            {
                MessageBox.Show("Marque una de las dos opciones para continuar.");
            }
        }

        private void EnviarButton_Click(object sender, EventArgs e)
        {
            if (Enfrentamiento.Checked)
            {
                // Enviamos nombre de los contrincantes
                string mensaje = "3/" + contrincante1.Text + "/" + contrincante2.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                int veces = Convert.ToInt32(mensaje);
                if (veces == -1)
                    MessageBox.Show("Error");
                else if (veces == -2)
                    MessageBox.Show("Error");
                else
                    MessageBox.Show("El número de veces totales que se han enfrentado " + contrincante1.Text + " y " + contrincante2.Text + " es de " + veces + " veces.");
            }
            else if (Puntos.Checked)
            {
                // Quiere saber el número de puntos
                string mensaje = "4/" + jugadorP.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                int puntos = Convert.ToInt32(mensaje);
                if (puntos == -1)
                    MessageBox.Show("Error");
                else if (puntos == -2)
                    MessageBox.Show("Error");
                else
                    MessageBox.Show("El número de puntos totales obtenidos por " + jugadorP.Text + " es de " + puntos + ".");
            }
            else if (Ganadores.Checked)
            {
                //Quiere saber quién ha ganado en las partidas donde ha jugado
                string mensaje = "5/" + jugadorG.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                MessageBox.Show(mensaje);
            }
            else if (Conectados.Checked)
            {
                // Quiere quién está conectado
                string mensaje = "6/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                string[] p = mensaje.Split('/');
                TablaConectados.ColumnCount = 1;
                TablaConectados.RowCount = Convert.ToInt32(p[0]);
                TablaConectados.Columns[0].HeaderText = "Conectados";
                TablaConectados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                TablaConectados.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                TablaConectados.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                int i = 0;
                while (i<Convert.ToInt32(p[0]))
                {
                    TablaConectados.Rows[i].HeaderCell.Value = Convert.ToString(i + 1);
                    TablaConectados.Rows[i].Cells[0].Value = p[i + 1];
                    i++;
                }
            }
        }

        private void DesconectarButton_Click(object sender, EventArgs e)
        {
            // Mensaje de desconexión
            string mensaje = "0/";

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            DesconectarButton.Visible = false;
            groupBox1.Visible = false;
            groupBox2.Visible = true;
            TablaConectados.Columns.Clear();
            TablaConectados.Rows.Clear();
        }
    }
}
