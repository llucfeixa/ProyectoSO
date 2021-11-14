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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Socket server;
        Thread atender;
        int connected = 0;
        delegate void DelegadoParaDataGridView(string [] info);

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; //Necesario para que los elementos de los formularios puedan ser
            //accedidos desde threads diferentes a los que los crearon
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DesconectarButton.Visible = false;
            groupBox1.Visible = false;
            password.UseSystemPasswordChar = true;            
            this.WindowState = FormWindowState.Maximized;
        }

        private void PonDataGridView(string[] trozos)
        {
            trozos[trozos.Length - 1] = trozos[trozos.Length - 1].Split('\0')[0];
            TablaConectados.Rows.Clear();
            TablaConectados.RowHeadersVisible = false;
            TablaConectados.ColumnCount = 1;
            TablaConectados.RowCount = Convert.ToInt32(trozos[1]);
            TablaConectados.Columns[0].HeaderText = "Conectados";
            TablaConectados.ColumnHeadersDefaultCellStyle.Font = new Font(TablaConectados.Font, FontStyle.Bold);
            TablaConectados.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            TablaConectados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            TablaConectados.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            TablaConectados.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            int i = 0;
            while (i < (Convert.ToInt32(trozos[1])))
            {
                TablaConectados.Rows[i].Cells[0].Value = trozos[i + 2];
                TablaConectados.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (usuario.Text == trozos[i + 2])
                {
                    TablaConectados.Rows[i].DefaultCellStyle.ForeColor = Color.Gray;
                }
                i++;
            }
            TablaConectados.ClearSelection();
        }

        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string [] trozos = Encoding.ASCII.GetString(msg2).Split('/');
                int codigo = Convert.ToInt32(trozos[0]);
                string mensaje = trozos[1].Split('\0')[0];
                int res;

                switch (codigo)
                {
                    case 1: //Respuesta a logueo
                        res = Convert.ToInt32(mensaje);
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
                        break;
                    case 2:  //Respuesta a registro
                        res = Convert.ToInt32(mensaje);
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
                        break;
                    case 3: //Número de veces enfrentadas
                        res = Convert.ToInt32(mensaje);
                        if (res == -1)
                            MessageBox.Show("Error");
                        else if (res == -2)
                            MessageBox.Show("Error");
                        else
                            MessageBox.Show("El número de veces totales que se han enfrentado " + contrincante1.Text + " y " + contrincante2.Text + " es de " + res + " veces.");
                        break;
                    case 4: //Número de puntos
                        res = Convert.ToInt32(mensaje);
                        if (res == -1)
                            MessageBox.Show("Error");
                        else if (res == -2)
                            MessageBox.Show("Error");
                        else
                            MessageBox.Show("El número de puntos totales obtenidos por " + jugadorP.Text + " es de " + res + ".");
                        break;
                    case 5: //Lista de los que han ganado a uno
                        MessageBox.Show(mensaje);
                        break;
                    case 6: //Lista conectados
                        TablaConectados.Invoke(new DelegadoParaDataGridView(PonDataGridView), new Object[] { trozos });
                        break;
                }
            }
        }

        private void LogInButton_Click(object sender, EventArgs e)
        {
            if (usuario.Text != "" && password.Text != "" && (Login.Checked == true || Register.Checked == true))
            {
                {
                    //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
                    //al que deseamos conectarnos
                    IPAddress direc = IPAddress.Parse("147.83.117.22");
                    IPEndPoint ipep = new IPEndPoint(direc, 50008);
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
                        }
                        else if (Register.Checked)
                        {
                            // Quiere registrarse
                            string mensaje = "2/" + usuario.Text + "/" + password.Text;
                            // Enviamos al servidor el nombre tecleado
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                            server.Send(msg);
                        }
                        ThreadStart ts = delegate { AtenderServidor(); };
                        atender = new Thread(ts);
                        atender.Start();
                        connected = 1;
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
            }
            else if (Puntos.Checked)
            {
                // Quiere saber el número de puntos
                string mensaje = "4/" + jugadorP.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else if (Ganadores.Checked)
            {
                //Quiere saber quién ha ganado en las partidas donde ha jugado
                string mensaje = "5/" + jugadorG.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void DesconectarButton_Click(object sender, EventArgs e)
        {
            // Mensaje de desconexión
            string mensaje = "0/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos
            connected = 0;
            atender.Abort();
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            DesconectarButton.Visible = false;
            groupBox1.Visible = false;
            groupBox2.Visible = true;            
            TablaConectados.Columns.Clear();
            TablaConectados.Rows.Clear();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connected == 1)
            {
                // Mensaje de desconexión
                string mensaje = "0/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Nos desconectamos
                connected = 0;
                atender.Abort();
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            if (password.UseSystemPasswordChar == true)
            {
                password.UseSystemPasswordChar = false;
                label6.Text = "Ocultar contraseña";
            }
            else if (password.UseSystemPasswordChar == false)
            {
                password.UseSystemPasswordChar = true;
                label6.Text = "Mostrar contraseña";
            }
        }
    }
}