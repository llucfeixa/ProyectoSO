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
        delegate void DelegadoParaHacerVisible();
        delegate void DelegadoParaMensajeChat(string jugador, string mensaje);
        List<Form2> formularios = new List<Form2>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DesconectarButton.Visible = false;
            groupBox1.Visible = false;
            password.UseSystemPasswordChar = true;            
            this.WindowState = FormWindowState.Maximized;
            TablaConectados.ReadOnly = true;
        }

        private void PonerEnMarchaFormulario(int id)
        {
            int cont = formularios.Count();
            Form2 f = new Form2(cont, server, id);
            formularios.Add(f);
            f.ShowDialog();
        }

        private void PonDataGridView(string[] trozos)
        {
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
            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Check";
            dgvCmb.HeaderText = "Seleccionar";
            TablaConectados.Columns.Add(dgvCmb);
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

        private void HacerVisible()
        {
            this.BackColor = Color.Green;
            groupBox1.Visible = true;
            DesconectarButton.Visible = true;
            groupBox2.Visible = false;
        }

        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                string[] trozos = mensaje.Split('/');
                int codigo = Convert.ToInt32(trozos[0]);
                int res;
                int numForm;

                switch (codigo)
                {
                    case 1: //Respuesta a logueo
                        res = Convert.ToInt32(trozos[1]);
                        if (res == 0)
                        {
                            MessageBox.Show("Logueado correctamente.");
                            Invoke(new DelegadoParaHacerVisible(HacerVisible));
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
                        res = Convert.ToInt32(trozos[1]);
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
                        res = Convert.ToInt32(trozos[1]);
                        if (res == -1)
                            MessageBox.Show("Error");
                        else if (res == -2)
                            MessageBox.Show("Error");
                        else
                            MessageBox.Show("El número de veces totales que se han enfrentado " + contrincante1.Text + " y " + contrincante2.Text + " es de " + res + " veces.");
                        break;
                    case 4: //Número de puntos
                        res = Convert.ToInt32(trozos[1]);
                        if (res == -1)
                            MessageBox.Show("Error");
                        else if (res == -2)
                            MessageBox.Show("Error");
                        else
                            MessageBox.Show("El número de puntos totales obtenidos por " + jugadorP.Text + " es de " + res + ".");
                        break;
                    case 5: //Lista de los que han ganado a uno
                        MessageBox.Show(trozos[1]);
                        break;
                    case 6: //Lista conectados
                        TablaConectados.Invoke(new DelegadoParaDataGridView(PonDataGridView), new Object[] { trozos });
                        break;
                    case 7:
                        MessageBox.Show("No hay partidas disponibles.");
                        break;
                    case 8:
                        res = Convert.ToInt32(trozos[1]);
                        DialogResult dialogResult = MessageBox.Show(trozos[2] + " te ha invitado a una partida. Quieres jugar?", "Invitación recibida", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            // Enviamos respuesta a la invitación
                            string mensaje2 = "7/" + res + "/SI";
                            // Enviamos al servidor la respuesta
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje2);
                            server.Send(msg);
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            // Enviamos respuesta a la invitación
                            string mensaje2 = "7/" + res + "/NO";
                            // Enviamos al servidor la respuesta
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje2);
                            server.Send(msg);
                        }
                        break;
                    case 9:
                        if (trozos[3] == "SI")
                        {
                            MessageBox.Show(trozos[2] + " ha aceptado jugar la partida.");
                        }
                        else if (trozos[3] == "NO")
                        {
                            MessageBox.Show(trozos[2] + " ha rechazado jugar la partida.");
                        }
                        break;
                    case 10:
                        //Invoke para abrir formulario
                        ThreadStart ts = delegate { PonerEnMarchaFormulario(Convert.ToInt32(trozos[1])); };
                        Thread T = new Thread(ts);
                        T.Start();
                        break;
                    case 11:
                        MessageBox.Show("Todos los usuarios invitados han rechazado jugar la partida.");
                        break;
                    case 12:
                        DialogResult dialogResult2 = MessageBox.Show((Convert.ToInt32(trozos[2]) - 1) + " personas han aceptado la invitación a la partida. Todavía quieres jugar?", "Invitación recibida", MessageBoxButtons.YesNo);
                        if (dialogResult2 == DialogResult.Yes)
                        {
                            // Enviamos respuesta a la invitación
                            string mensaje2 = "8/" + trozos[1] + "/SI";
                            // Enviamos al servidor la respuesta
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje2);
                            server.Send(msg);
                        }
                        else if (dialogResult2 == DialogResult.No)
                        {
                            // Enviamos respuesta a la invitación
                            string mensaje2 = "8/" + trozos[1] + "/NO";
                            // Enviamos al servidor la respuesta
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje2);
                            server.Send(msg);
                        }
                        break;
                    case 13:
                        MessageBox.Show("El anfitrión ha decidido no jugar la partida porque no todos los invitados han aceptado.");
                        break;
                    case 14:
                        int pos = Convert.ToInt32(trozos[1]);
                        numForm = Convert.ToInt32(trozos[2]);
                        string jugador = trozos[3];
                        string chat = trozos[4];
                        int i = 0;
                        bool encontrado = false;
                        while (i<formularios.Count() && encontrado == false)
                        {
                            if (formularios[i].Partida() == pos)
                            {
                                encontrado = true;
                            }
                            if (encontrado == false)
                            {
                                i = i + 1;
                            }
                        }
                        formularios[i].Invoke(new DelegadoParaMensajeChat(formularios[i].TomaMensajeChat), new object[] { jugador, chat });
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
            else if (Invitar.Checked)
            {
                List<string> invitados = new List<string>();
                int i = 0;
                while(i<TablaConectados.RowCount)
                {
                    if (Convert.ToBoolean(TablaConectados.Rows[i].Cells[1].Value))
                    {
                        invitados.Add(Convert.ToString(TablaConectados.Rows[i].Cells[0].Value));
                    }
                    i = i + 1;
                }
                //Quiere invitar a algunos jugadores
                if(invitados.Count()==0)
                {
                    MessageBox.Show("Tienes que invitar a alguien.");
                }
                else if(invitados.Count>3)
                {
                    MessageBox.Show("El número máximo de personas invitadas es de 3.");
                }
                else
                {
                    string mensaje = "6";
                    i = 0;
                    while (i < invitados.Count)
                    {
                        mensaje = mensaje + "/" + invitados[i];
                        i = i + 1;
                    }
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    MessageBox.Show("Invitaciones enviadas.");
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

        private void TablaConectados_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            int fila = e.RowIndex;
            if (Convert.ToString(TablaConectados.Rows[fila].Cells[0].Value) != usuario.Text)
            {
                if (Convert.ToBoolean(TablaConectados.Rows[fila].Cells[1].Value))
                {
                    TablaConectados.Rows[fila].Cells[1].Value = false;
                }
                else
                {
                    TablaConectados.Rows[fila].Cells[1].Value = true;
                }
            }
        }
    }
}