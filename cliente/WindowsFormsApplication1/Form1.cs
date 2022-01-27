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
using FontAwesome.Sharp;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private IconButton currentBtn;
        private Panel leftBorderBtn;

        Socket server;
        Thread atender;
        int connected = 0;
        int logueado = 0;
        int cont = 0;
        int nivel;
        string nombre;
        int[] selección = new int[6];
        int[] mazo = new int[6];
        int[] cartasEnMazo = new int[36];
        int id = 0;
        int partidasActivas = 0;

        delegate void DelegadoParaDataGridView(string [] info);
        delegate void DelegadoParaHacerVisible();
        delegate void DelegadoParaNotificación(string mensaje);
        delegate void DelegadoParaMensajeChat(int caso, string jugador, string mensaje);
        delegate void DelegadoParaNivel(int level, int exp, int expMax);
        delegate void DelegadoParaImagen(int id, IconPictureBox iconPictureBox);
        delegate void DelegadoParaCarta(string nombre, string vida, string ataque, string defensa, string descripción);
        delegate void DelegadoParaTurno(int idCartaA, int idCartaD, int vidaTotal, int turnoA, int turnoD);
        delegate void DelegadoParaActualizar(int turnoAtaque, int turnoDefensa, string[] nombreJug, int[] vidaJug, int[] vidaInicialJug);
        delegate void DelegadoParaAtaqueDefensa(string nombre, int ataque, int defensa);
        delegate void DelegadoParaNotificacionesForm2(int caso, string jugador);

        List<Form2> formularios = new List<Form2>();

        public Form1()
        {
            InitializeComponent();
            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 92);
            panelMenu.Controls.Add(leftBorderBtn);
        }

        private struct RGBColors
        {
            public static Color color1 = Color.FromArgb(172, 126, 241);
            public static Color color2 = Color.FromArgb(249, 118, 176);
            public static Color color3 = Color.FromArgb(253, 138, 114);
            public static Color color4 = Color.FromArgb(95, 77, 221);
            public static Color color5 = Color.FromArgb(249, 88, 155);
            public static Color color6 = Color.FromArgb(24, 161, 251);
        }

        private void ActivarBoton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DeshabilitarBoton();
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.FromArgb(37, 36, 81);
                currentBtn.ForeColor = color;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;

                leftBorderBtn.BackColor = color;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();

                iconActual.IconChar = currentBtn.IconChar;
                iconActual.IconColor = color;

                TituloLbl.Text = currentBtn.Text;
            }
        }

        private void DeshabilitarBoton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.DarkBlue;
                currentBtn.ForeColor = Color.Gainsboro;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.Gainsboro;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DesconectarBtn.Visible = false;
            password.UseSystemPasswordChar = true;
            TablaConectados.ReadOnly = true;
            TablaConectados.Hide();
            tabControl1.Show();
            tabControl1.SelectedTab = tabControl1.TabPages["homePage"];
            axWindowsMediaPlayer1.uiMode = "none";
            axWindowsMediaPlayer1.URL = "Intro.mp4";
            nivelLbl.Visible = false;
            nivelProgressBar.Visible = false;
            progresoLbl.Visible = false;
        }

        private void PonerEnMarchaFormulario(int id, int j, string[] nombres, int[] cartas, int[] vida, int[] vidaInicial)
        {
            int forms = formularios.Count();
            string mensaje = "11/" + id + "/" + forms;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            Form2 f = new Form2(forms, server, id, j, nombres, cartas, vida, vidaInicial);
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
            dgvCmb.HeaderText = "Selección";
            TablaConectados.Columns.Add(dgvCmb);
            int i = 0;
            while (i < (Convert.ToInt32(trozos[1])))
            {
                TablaConectados.Rows[i].Cells[0].Value = trozos[i + 2];
                TablaConectados.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (nombre == trozos[i + 2])
                {
                    TablaConectados.Rows[i].DefaultCellStyle.ForeColor = Color.Gray;
                }
                i++;
            }
            TablaConectados.ClearSelection();
        }

        private void PonHistorialPartidas(string[] trozos)
        {
            historialGridView.Visible = true;
            historialGridView.Rows.Clear();
            historialGridView.RowHeadersVisible = false;
            historialGridView.ColumnCount = 3;
            historialGridView.RowCount = Convert.ToInt32(trozos[1]);
            historialGridView.Columns[0].HeaderText = "Fecha y hora";
            historialGridView.Columns[1].HeaderText = "Duración";
            historialGridView.Columns[2].HeaderText = "Ganador";
            historialGridView.ColumnHeadersDefaultCellStyle.Font = new Font(historialGridView.Font, FontStyle.Bold);
            historialGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            historialGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            historialGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            historialGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            int i = 0;
            int j = 0;
            while (i < (Convert.ToInt32(trozos[1])))
            {
                historialGridView.Rows[i].Cells[0].Value = trozos[j + 2];
                int segundos = Convert.ToInt32(trozos[j + 3]);
                int minutosH = segundos / 60;
                int Rsegundo = segundos % 60;
                int Rhora = minutosH / 60;
                int Rminutos = minutosH % 60;
                historialGridView.Rows[i].Cells[1].Value = Rhora + "h " + Rminutos + "min " + Rsegundo + "s";
                historialGridView.Rows[i].Cells[2].Value = trozos[j + 4];
                historialGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                historialGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                historialGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (nombre == trozos[j + 4])
                {
                    historialGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
                }
                i++;
                j = j + 3;
            }
            historialGridView.ClearSelection();
        }

        private void InicializaCartas()
        {
            carta1.BackgroundImage = Properties.Resources.Heroe1;
            carta2.BackgroundImage = Properties.Resources.Heroe2;
            carta3.BackgroundImage = Properties.Resources.Heroe3;
            carta4.BackgroundImage = Properties.Resources.Heroe4;
            carta5.BackgroundImage = Properties.Resources.Heroe5;
            carta6.BackgroundImage = Properties.Resources.Heroe6;
            carta7.BackgroundImage = Properties.Resources.Heroe7;
            carta8.BackgroundImage = Properties.Resources.Heroe8;
            carta9.BackgroundImage = Properties.Resources.Heroe9;
            carta10.BackgroundImage = Properties.Resources.Heroe10;
            carta11.BackgroundImage = Properties.Resources.Heroe11;
            carta12.BackgroundImage = Properties.Resources.Heroe12;
        }

        private void ActualizaCartas()
        {
            carta13.BackgroundImage = Properties.Resources.Candado_Nivel_2;
            carta14.BackgroundImage = Properties.Resources.Candado_Nivel_2;
            carta15.BackgroundImage = Properties.Resources.Candado_Nivel_2;
            carta16.BackgroundImage = Properties.Resources.Candado_Nivel_2;
            carta17.BackgroundImage = Properties.Resources.Candado_Nivel_3;
            carta18.BackgroundImage = Properties.Resources.Candado_Nivel_3;
            carta19.BackgroundImage = Properties.Resources.Candado_Nivel_3;
            carta20.BackgroundImage = Properties.Resources.Candado_Nivel_3;
            carta21.BackgroundImage = Properties.Resources.Candado_Nivel_4;
            carta22.BackgroundImage = Properties.Resources.Candado_Nivel_4;
            carta23.BackgroundImage = Properties.Resources.Candado_Nivel_4;
            carta24.BackgroundImage = Properties.Resources.Candado_Nivel_4;
            carta25.BackgroundImage = Properties.Resources.Candado_Nivel_5;
            carta26.BackgroundImage = Properties.Resources.Candado_Nivel_5;
            carta27.BackgroundImage = Properties.Resources.Candado_Nivel_5;
            carta28.BackgroundImage = Properties.Resources.Candado_Nivel_6;
            carta29.BackgroundImage = Properties.Resources.Candado_Nivel_6;
            carta30.BackgroundImage = Properties.Resources.Candado_Nivel_6;
            carta31.BackgroundImage = Properties.Resources.Candado_Nivel_7;
            carta32.BackgroundImage = Properties.Resources.Candado_Nivel_7;
            carta33.BackgroundImage = Properties.Resources.Candado_Nivel_8;
            carta34.BackgroundImage = Properties.Resources.Candado_Nivel_8;
            carta35.BackgroundImage = Properties.Resources.Candado_Nivel_9;
            carta36.BackgroundImage = Properties.Resources.Candado_Nivel_Máx;
            if (nivel >= 2)
            {
                carta13.BackgroundImage = Properties.Resources.Heroe13;
                carta14.BackgroundImage = Properties.Resources.Heroe14;
                carta15.BackgroundImage = Properties.Resources.Heroe15;
                carta16.BackgroundImage = Properties.Resources.Heroe16;
            }
            if (nivel >= 3)
            {
                carta17.BackgroundImage = Properties.Resources.Heroe17;
                carta18.BackgroundImage = Properties.Resources.Heroe18;
                carta19.BackgroundImage = Properties.Resources.Heroe19;
                carta20.BackgroundImage = Properties.Resources.Heroe20;
            }
            if (nivel >= 4)
            {
                carta21.BackgroundImage = Properties.Resources.Heroe21;
                carta22.BackgroundImage = Properties.Resources.Heroe22;
                carta23.BackgroundImage = Properties.Resources.Heroe23;
                carta24.BackgroundImage = Properties.Resources.Heroe24;
            }
            if (nivel >= 5)
            {
                carta25.BackgroundImage = Properties.Resources.Heroe25;
                carta26.BackgroundImage = Properties.Resources.Heroe26;
                carta27.BackgroundImage = Properties.Resources.Heroe27;
            }
            if (nivel >= 6)
            {
                carta28.BackgroundImage = Properties.Resources.Heroe28;
                carta29.BackgroundImage = Properties.Resources.Heroe29;
                carta30.BackgroundImage = Properties.Resources.Heroe30;
            }
            if (nivel >= 7)
            {
                carta31.BackgroundImage = Properties.Resources.Heroe31;
                carta32.BackgroundImage = Properties.Resources.Heroe32;
            }
            if (nivel >= 8)
            {
                carta33.BackgroundImage = Properties.Resources.Heroe33;
                carta34.BackgroundImage = Properties.Resources.Heroe34;
            }
            if (nivel >= 9)
            {
                carta35.BackgroundImage = Properties.Resources.Heroe35;
            }
            if (nivel >= 10)
            {
                carta36.BackgroundImage = Properties.Resources.Heroe36;
            }
        }

        private void ActualizaNivel(int nivelJugador, int experiencia, int experienciaMax)
        {
            nivel = nivelJugador;
            if (nivel == 10)
            {
                nivelLbl.Text = "Nivel Máx.";           
            }
            else
            {
                nivelLbl.Text = "Nivel " + nivel;
            }
            progresoLbl.Text = experiencia + "/" + experienciaMax;
            int progreso = Convert.ToInt32(Convert.ToDouble(experiencia) / Convert.ToDouble(experienciaMax) * 100);
            nivelProgressBar.Value = progreso;
            ActualizaCartas();
        }

        private void InformaciónCarta(string nombre, string vida, string ataque, string defensa, string descripción)
        {
            nombreLbl.Text = nombre;
            vidaLbl.Text = vida;
            ataqueLbl.Text = ataque;
            defensaLbl.Text = defensa;
            descripciónLbl.Text = descripción;
            tabControl1.Show();
            tabControl1.SelectedTab = tabControl1.TabPages["CartaPage"];
            AñadirImagen(id, cartaPicBox);
        }

        private void HacerVisible()
        {
            nivelLbl.Visible = true;
            nivelProgressBar.Visible = true;
            progresoLbl.Visible = true;
            DesconectarBtn.Visible = true;
            TablaConectados.Show();
            nombre = usuario.Text;
        }

        private void Logueo()
        {
            tabControl1.Show();
            tabControl1.SelectedTab = tabControl1.TabPages["logueadoPage"];
        }

        private void Notificación(string mensaje)
        {
            perfilLbl.Text = mensaje;
        }

        private void Notificaciones(string mensaje)
        {
            notificaciónLbl.Text = mensaje;
        }

        private void NotificaciónJugar(string mensaje)
        {
            notificaciónJLbl.Text = mensaje;
        }

        private void DarseDeBaja()
        {
            usuario.Text = null;
            password.Text = null;
            DarseBaja.Checked = false;
        }

        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos la respuesta del servidor
                byte[] msg = new byte[600];
                server.Receive(msg);
                string mensaje = Encoding.ASCII.GetString(msg).Split('\0')[0];
                string[] trozos = mensaje.Split('/');
                int codigo = Convert.ToInt32(trozos[0]);
                int res;
                int numForm;
                int idCarta;
                int idPartida;
                int idCartaA;
                int idCartaD;
                int vidaTotal;
                int turnoA;
                int turnoD;
                int numJugadores;
                string[] nombres;
                int[] vida;
                int[] vidaInicial;
                int i;
                int j;
                string petición;
                byte[] pet;

                switch (codigo)
                {
                    case 1: //Respuesta a logueo
                        res = Convert.ToInt32(trozos[1]);
                        if (res == 0)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "Te has logueado correctamente." });
                            logueado = 1;
                            Invoke(new DelegadoParaNivel(ActualizaNivel), new Object[] { Convert.ToInt32(trozos[2]), Convert.ToInt32(trozos[3]), Convert.ToInt32(trozos[4]) });
                            Invoke(new DelegadoParaHacerVisible(Logueo));
                            Invoke(new DelegadoParaHacerVisible(HacerVisible));
                            if (Convert.ToInt32(trozos[5]) == 6)
                            {
                                cont = 6;
                                idCarta = Convert.ToInt32(trozos[6]);
                                mazo[0] = idCarta;
                                idCarta = Convert.ToInt32(trozos[7]);
                                mazo[1] = idCarta;
                                idCarta = Convert.ToInt32(trozos[8]);
                                mazo[2] = idCarta;
                                idCarta = Convert.ToInt32(trozos[9]);
                                mazo[3] = idCarta;
                                idCarta = Convert.ToInt32(trozos[10]);
                                mazo[4] = idCarta;
                                idCarta = Convert.ToInt32(trozos[11]);
                                mazo[5] = idCarta;
                            }
                        }
                        else if (res == -1)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "Error al consultar la base de datos." });
                        }
                        else if (res == -2)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "No es posible loguearse. Inténtelo de nuevo." });
                        }
                        else if (res == -3)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "Ya hay un usuario usando esta cuenta." });
                        }
                        break;
                    case 2:  //Respuesta a registro
                        res = Convert.ToInt32(trozos[1]);
                        if (res == 0)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "Te has registrado correctamente." });
                        }
                        else if (res == -1)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "No es posible registrarse. Inténtelo de nuevo." });
                        }
                        else
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "No es posible registrarse. Pruebe otro nombre de usuario." });
                        }
                        break;
                    case 3: //Respuesta a historial de partidas
                        res = Convert.ToInt32(trozos[1]);
                        if (res == -1)
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { trozos[2] });
                        else if (res == -2)
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { trozos[2] });
                        else
                        {
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { "Mostrando historial de las partidas en las que has participado." });
                            Invoke(new DelegadoParaDataGridView(PonHistorialPartidas), new Object[] { trozos });
                        }
                        break;
                    case 4: //Respuesta a partidas jugadas
                        res = Convert.ToInt32(trozos[1]);
                        if (res == -1)
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { "Error al consultar datos de la base." });
                        else if (res == -2)
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { "No se han obtenido datos en la consulta." });
                        else
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { "El número total de partidas que has jugado es " + res + "." });
                        break;
                    case 5: //Respuesta a partidas ganadas
                        res = Convert.ToInt32(trozos[1]);
                        if (res == -1)
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { "Error al consultar datos de la base." });
                        else if (res == -2)
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { "No se han obtenido datos en la consulta." });
                        else
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { "El número total de partidas que has ganado es " + res + "." });
                        break;
                    case 6: //Lista conectados
                        TablaConectados.Invoke(new DelegadoParaDataGridView(PonDataGridView), new Object[] { trozos });
                        break;
                    case 7: //Respuesta al no poder crear la partida
                        res = Convert.ToInt32(trozos[1]);
                        if (res == -1)
                        {
                            Invoke(new DelegadoParaNotificación(NotificaciónJugar), new Object[] { "No hay partidas disponibles." });
                        }
                        else if (res == -2)
                        {
                            Invoke(new DelegadoParaNotificación(NotificaciónJugar), new Object[] { "Error al crear la partida." });
                        }
                        break;
                    case 8: //Respuesta a invitación recibida
                        res = Convert.ToInt32(trozos[1]);
                        petición = "7/" + res;
                        if (cont == 6)
                        {
                            DialogResult dialogResult = MessageBox.Show(trozos[2] + " te ha invitado a una partida. Quieres jugar?", "Invitación recibida", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                petición = petición + "/SI";
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                petición = petición + "/NO/1";
                            }
                        }
                        else
                        {
                            petición = petición + "/NO/2";
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { trozos[2] + " te ha invitado a una partida, pero aún no tienes ningún mazo." });
                        }
                        pet = System.Text.Encoding.ASCII.GetBytes(petición);
                        server.Send(pet);
                        break;
                    case 9: //Respuesta a quién ha aceptado o no
                        if (trozos[3] == "SI")
                        {
                            Invoke(new DelegadoParaNotificación(NotificaciónJugar), new Object[] { trozos[2] + " ha aceptado jugar la partida." });
                        }
                        else if (trozos[3] == "NO")
                        {
                            if (Convert.ToInt32(trozos[4]) == 1)
                            {
                                Invoke(new DelegadoParaNotificación(NotificaciónJugar), new Object[] { trozos[2] + " ha rechazado jugar la partida." });
                            }
                            else if (Convert.ToInt32(trozos[4]) == 2)
                            {
                                Invoke(new DelegadoParaNotificación(NotificaciónJugar), new Object[] { trozos[2] + " aún no tiene ningún mazo creado." });
                            }
                        }
                        break;
                    case 10: //Respuesta a inicio partida (Invoke para abrir formulario)
                        MessageBox.Show("La partida va a empezar...");
                        nombres = new string[Convert.ToInt32(trozos[3])];
                        vida = new int[Convert.ToInt32(trozos[3])];
                        vidaInicial = new int[Convert.ToInt32(trozos[3])];
                        j = 0;
                        i = 0;
                        while (i < Convert.ToInt32(trozos[3]))
                        {
                            nombres[i] = trozos[j + 4];
                            vida[i] = Convert.ToInt32(trozos[j + 5]);
                            vidaInicial[i] = Convert.ToInt32(trozos[j + 6]);
                            j = j + 3;
                            i++;
                        }
                        ThreadStart ts = delegate { PonerEnMarchaFormulario(Convert.ToInt32(trozos[1]), Convert.ToInt32(trozos[2]), nombres, mazo, vida, vidaInicial); };
                        Thread T = new Thread(ts);
                        T.Start();
                        partidasActivas++;
                        break;
                    case 11: //Respuesta a todos han rechazado
                        Invoke(new DelegadoParaNotificación(NotificaciónJugar), new Object[] { "Todos los usuarios invitados han rechazado jugar la partida." });
                        break;
                    case 12: //Respuesta a si todavía sigue la partida
                        DialogResult dialogResult2 = MessageBox.Show((Convert.ToInt32(trozos[2]) - 1) + " personas han aceptado la invitación a la partida. Todavía quieres jugar?", "Invitación recibida", MessageBoxButtons.YesNo);
                        petición = "8/" + trozos[1];
                        if (dialogResult2 == DialogResult.Yes)
                        {
                            petición = petición + "/SI";
                        }
                        else if (dialogResult2 == DialogResult.No)
                        {
                            petición = petición + "/NO";
                        }
                        pet = System.Text.Encoding.ASCII.GetBytes(petición);
                        server.Send(pet);
                        break;
                    case 13: //Respuesta a partida no sigue
                        Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "El anfitrión ha cancelado la partida porque no todos han aceptado." });
                        break;
                    case 14: //Respuesta a mensaje chat
                        idPartida = Convert.ToInt32(trozos[1]);
                        numForm = Convert.ToInt32(trozos[2]);
                        formularios[numForm].Invoke(new DelegadoParaMensajeChat(formularios[numForm].TomaMensajeChat), new object[] { Convert.ToInt32(trozos[3]), trozos[4], trozos[5] });
                        break;
                    case 15: //Respuesta a actualizar nivel
                        partidasActivas--;
                        Invoke(new DelegadoParaNivel(ActualizaNivel), new Object[] { Convert.ToInt32(trozos[1]), Convert.ToInt32(trozos[2]), Convert.ToInt32(trozos[3]) });
                        Invoke(new DelegadoParaHacerVisible(BorrarImagen));
                        break;
                    case 16: //Respuesta a cargar mazo
                        Invoke(new DelegadoParaHacerVisible(InicializaCartas));
                        Invoke(new DelegadoParaHacerVisible(ActualizaCartas));
                        res = Convert.ToInt32(trozos[1]);
                        if (res == -1)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "No tienes ningún mazo." });
                        }
                        if (res == 6)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "Mazo cargado correctamente." });

                            i = 0;
                            while (i<cartasEnMazo.Length)
                            {
                                cartasEnMazo[i] = 0;
                                i++;
                            }
                            cont = 6;
                            idCarta = Convert.ToInt32(trozos[2]);
                            Invoke(new DelegadoParaImagen(AñadirImagen), new Object[] { idCarta, mazo1 });
                            selección[0] = idCarta;
                            mazo[0] = idCarta;
                            cartasEnMazo[idCarta - 1] = 1;

                            idCarta = Convert.ToInt32(trozos[3]);
                            Invoke(new DelegadoParaImagen(AñadirImagen), new Object[] { idCarta, mazo2 });
                            selección[1] = idCarta;
                            mazo[1] = idCarta;
                            cartasEnMazo[idCarta - 1] = 1;

                            idCarta = Convert.ToInt32(trozos[4]);
                            Invoke(new DelegadoParaImagen(AñadirImagen), new Object[] { idCarta, mazo3 });
                            selección[2] = idCarta;
                            mazo[2] = idCarta;
                            cartasEnMazo[idCarta - 1] = 1;

                            idCarta = Convert.ToInt32(trozos[5]);
                            Invoke(new DelegadoParaImagen(AñadirImagen), new Object[] { idCarta, mazo4 });
                            selección[3] = idCarta;
                            mazo[3] = idCarta;
                            cartasEnMazo[idCarta - 1] = 1;

                            idCarta = Convert.ToInt32(trozos[6]);
                            Invoke(new DelegadoParaImagen(AñadirImagen), new Object[] { idCarta, mazo5 });
                            selección[4] = idCarta;
                            mazo[4] = idCarta;
                            cartasEnMazo[idCarta - 1] = 1;

                            idCarta = Convert.ToInt32(trozos[7]);
                            Invoke(new DelegadoParaImagen(AñadirImagen), new Object[] { idCarta, mazo6 });
                            selección[5] = idCarta;
                            mazo[5] = idCarta;
                            cartasEnMazo[idCarta - 1] = 1;

                            Invoke(new DelegadoParaHacerVisible(BorrarImagen));
                        }
                        else
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "Error al cargar el mazo." });
                        }
                        break;
                    case 17: //Respuesta a información de una carta
                        Invoke(new DelegadoParaCarta(InformaciónCarta), new Object[] { trozos[2], trozos[3], trozos[4], trozos[5], trozos[6] });
                        break;
                    case 18: //Respuesta a añadir mazo
                        res = Convert.ToInt32(trozos[1]);
                        if (res == -1)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "No se ha podido añadir el mazo." });
                        }
                        if (res == -2)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "No se ha podido añadir el mazo." });
                        }
                        if (res == 0)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "Mazo añadido correctamente." });
                        }
                        break;
                    case 19: //Respuesta al borrar mazo
                        res = Convert.ToInt32(trozos[1]);
                        if (res == -1)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "No se ha podido borrar el mazo." });
                        }
                        if (res == -2)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "No se ha podido borrar el mazo." });
                        }
                        if (res == 0)
                        {
                            Invoke(new DelegadoParaHacerVisible(InicializaCartas));
                            Invoke(new DelegadoParaHacerVisible(ActualizaCartas));
                            Invoke(new DelegadoParaHacerVisible(BorrarMazo));
                            Invoke(new DelegadoParaHacerVisible(BorrarImagen));
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "Mazo borrado correctamente." });
                        }
                        break;
                    case 20: //Respuesta a cambio de turno
                        idPartida = Convert.ToInt32(trozos[1]);
                        numForm = Convert.ToInt32(trozos[2]);
                        idCartaA = Convert.ToInt32(trozos[3]);
                        idCartaD = Convert.ToInt32(trozos[4]);
                        vidaTotal = Convert.ToInt32(trozos[5]);
                        turnoA = Convert.ToInt32(trozos[6]);
                        turnoD = Convert.ToInt32(trozos[7]);
                        formularios[numForm].Invoke(new DelegadoParaTurno(formularios[numForm].TomaAtaqueDefensa), new object[] { idCartaA, idCartaD, vidaTotal, turnoA, turnoD });
                        break;
                    case 21: //Respuesta a ha abandonado partida
                        idPartida = Convert.ToInt32(trozos[1]);
                        numForm = Convert.ToInt32(trozos[2]);
                        res = Convert.ToInt32(trozos[3]);
                        turnoA = Convert.ToInt32(trozos[5]);
                        turnoD = Convert.ToInt32(trozos[6]);
                        numJugadores = Convert.ToInt32(trozos[7]);
                        nombres = new string[numJugadores];
                        vida = new int[numJugadores];
                        vidaInicial = new int[numJugadores];
                        j = 0;
                        i = 0;
                        while (i < numJugadores)
                        {
                            nombres[i] = trozos[j + 8];
                            vida[i] = Convert.ToInt32(trozos[j + 9]);
                            vidaInicial[i] = Convert.ToInt32(trozos[j + 10]);
                            j = j + 3;
                            i++;
                        }
                        try
                        {
                            formularios[numForm].Invoke(new DelegadoParaActualizar(formularios[numForm].ActualizaForm), new object[] { turnoA, turnoD, nombres, vida, vidaInicial });
                            formularios[numForm].Invoke(new DelegadoParaNotificacionesForm2(formularios[numForm].TomaNotificaciones), new object[] { res, trozos[4] });
                        }
                        finally
                        {
                        }
                        break;
                    case 22: //Respuesta a has abandonado partida
                        partidasActivas--;
                        MessageBox.Show("Has abandonado la partida. No has recibido experiencia.");
                        break;
                    case 23: //Respuesta a jugador con más experiencia
                        res = Convert.ToInt32(trozos[1]);
                        if (res == -1)
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { trozos[2] });
                        else if (res == -2)
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { trozos[2] });
                        else if (res == 1)
                        {
                            if (Convert.ToInt32(trozos[2]) == 10)
                                Invoke(new DelegadoParaNotificación(Notificación), new Object[] { "El jugador con más experiencia es " + trozos[4] + ", que ha alcanzado el Nivel Máximo y tiene una experiencia total de " + trozos[3] + "." });
                            else
                                Invoke(new DelegadoParaNotificación(Notificación), new Object[] { "El jugador con más experiencia es " + trozos[4] + ", que tiene Nivel " + trozos[2] + " y una experiencia total de " + trozos[3] + "." });
                        }
                        else
                        {
                            mensaje = "Los jugadores con más experiencia son: " + trozos[4];
                            i = 1;
                            while (i < res)
                            {
                                mensaje = mensaje + ", " + trozos[i + 4];
                                i++;
                            }
                            if (Convert.ToInt32(trozos[2]) == 10)
                                Invoke(new DelegadoParaNotificación(Notificación), new Object[] { mensaje + ", que han alcanzado el Nivel Máximo y tienen una experiencia total de " + trozos[3] + "." });
                            else
                                Invoke(new DelegadoParaNotificación(Notificación), new Object[] { mensaje + ", que tienen Nivel " + trozos[2] + " y una experiencia total de " + trozos[3] + "." });
                        }
                        break;
                    case 24: //Respuesta a jugador con más partidas ganadas
                        res = Convert.ToInt32(trozos[1]);
                        if (res == -1)
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { trozos[2] });
                        else if (res == -2)
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { trozos[2] });
                        else if (res == 1)
                        {
                            mensaje = "El jugador con más partidas ganadas es " + trozos[3] + ", que ha ganado " + trozos[2];
                            if (Convert.ToInt32(trozos[2]) == 1)
                                mensaje = mensaje + " partida.";
                            else
                                mensaje = mensaje + " partidas.";
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { mensaje });
                        }
                        else
                        {
                            mensaje = "Los jugadores con más partidas ganadas son: " + trozos[3];
                            i = 1;
                            while (i < res)
                            {
                                mensaje = mensaje + ", " + trozos[i + 3];
                                i++;
                            }
                            if (Convert.ToInt32(trozos[2]) == 1)
                                mensaje = mensaje + ", que han ganado " + trozos[2] + " partida.";
                            else
                                mensaje = mensaje + ", que han ganado " + trozos[2] + " partidas.";
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { mensaje });
                        }
                        break;
                    case 25: //Respuesta a experiencia total ganada
                        res = Convert.ToInt32(trozos[1]);
                        if (res == -1)
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { "Error al consultar datos de la base." });
                        else if (res == -2)
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { "No se han obtenido datos en la consulta." });
                        else
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { "La experiencia total que has ganado es " + res + "." });
                        break;
                    case 26: //Respuesta a información carta durante la partida
                        idPartida = Convert.ToInt32(trozos[1]);
                        numForm = Convert.ToInt32(trozos[2]);
                        res = Convert.ToInt32(trozos[3]);
                        if (res == -1)
                        {
                            formularios[numForm].Invoke(new DelegadoParaNotificación(formularios[numForm].TomaNotificación), new object[] { "Error al consultar datos de la base." });
                        }
                        else if (res == -2)
                        {
                            formularios[numForm].Invoke(new DelegadoParaNotificación(formularios[numForm].TomaNotificación), new object[] { "No se han obtenido datos en la consulta." });
                        }
                        else
                        {
                            formularios[numForm].Invoke(new DelegadoParaAtaqueDefensa(formularios[numForm].AtaqueDefensaCarta), new object[] { trozos[4], Convert.ToInt32(trozos[5]), Convert.ToInt32(trozos[6]) });
                        }
                        break;
                    case 27: //Respuesta a darse de baja
                        res = Convert.ToInt32(trozos[1]);
                        if (res == 0)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "Te has dado de baja correctamente." });
                            Invoke(new DelegadoParaHacerVisible(DarseDeBaja));
                        }
                        else if (res == -1)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "No es posible darse de baja. Inténtelo de nuevo." });
                        }
                        else if (res == -2)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "No es posible darse de baja. Compruebe los datos introducidos." });
                        }
                        else if (res == -3)
                        {
                            Invoke(new DelegadoParaNotificación(Notificaciones), new Object[] { "No es posible darse de baja. Hay un usuario utilizando la cuenta." });
                        }
                        break;
                    case 28: //Respuesta a jugadores contra los que me he enfrentado
                        res = Convert.ToInt32(trozos[1]);
                        if (res == -1)
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { trozos[2] });
                        else if (res == -2)
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { trozos[2] });
                        else
                        {
                            mensaje = "Te has enfrentado contra: " + trozos[2];
                            i = 1;
                            while (i < res)
                            {
                                mensaje = mensaje + ", " + trozos[i + 2];
                                i++;
                            }
                            Invoke(new DelegadoParaNotificación(Notificación), new Object[] { mensaje });
                        }
                        break;
                }
            }
        }

        private void LogInButton_Click(object sender, EventArgs e)
        {
            if (usuario.Text != "" && password.Text != "" && (Login.Checked == true || Register.Checked == true || DarseBaja.Checked == true))
            {
                {
                    //Creamos un IPEndPoint con el ip del servidor y puerto del servidor al que deseamos conectarnos
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
                        else if (DarseBaja.Checked)
                        {
                            // Quiere darse de baja
                            string mensaje = "22/" + usuario.Text + "/" + password.Text;
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
            if (Login.Checked == false && Register.Checked == false && DarseBaja.Checked == false)
            {
                MessageBox.Show("Marque una de las dos opciones para continuar.");
            }
        }
        
        private void EnviarButton_Click(object sender, EventArgs e)
        {
            if (historial.Checked)
            {
                string mensaje = "3/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else if (partidasJugadas.Checked)
            {
                string mensaje = "4/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else if (partidasGanadas.Checked)
            {
                string mensaje = "5/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else if (experiencia.Checked)
            {
                string mensaje = "18/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else if (maxGanador.Checked)
            {
                string mensaje = "19/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else if (experienciaTotal.Checked)
            {
                string mensaje = "20/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else if (adversarios.Checked)
            {
                string mensaje = "23/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else if (partidasMarcados.Checked)
            {
                string[] trozos = marcadosBox.Text.Split('/');
                if (marcadosBox.Text == "")
                {
                    perfilLbl.Text = "Tienes que escribir algún nombre.";
                }
                else if (trozos.Length < 20)
                {
                    string mensaje = "24/" + trozos.Length + "/" + marcadosBox.Text;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                {
                    perfilLbl.Text = "El número máximo de nombres es 20.";
                }
            }
            else if (partidasTiempo.Checked)
            {
                string fecha_inicio = dateTimePicker1.Value.ToString("dd/MM/yyyy/HH/mm/ss");
                string fecha_final = dateTimePicker2.Value.ToString("dd/MM/yyyy/HH/mm/ss");
                string mensaje = "25/" + fecha_inicio + "/" + fecha_final;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (partidasActivas == 0 && connected == 1)
            {
                // Mensaje de desconexión
                string mensaje = "0/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Nos desconectamos
                atender.Abort();
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
            else if (partidasActivas > 0)
            {
                e.Cancel = (e.CloseReason == CloseReason.UserClosing);
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
            if (Convert.ToString(TablaConectados.Rows[fila].Cells[0].Value) != nombre)
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

        private void LogInBtn_Click(object sender, EventArgs e)
        {
            if (notificaciónLbl.Text == "Todavía no estás logueado.")
            {
                notificaciónLbl.Text = null;
            }
            ActivarBoton(sender, RGBColors.color1);
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            tabControl1.Show();
            if (logueado == 1)
            {
                tabControl1.SelectedTab = tabControl1.TabPages["logueadoPage"];
            }
            else
            {
                tabControl1.SelectedTab = tabControl1.TabPages["logueoPage"];
            }
        }

        private void PerfilBtn_Click(object sender, EventArgs e)
        {
            if (logueado == 1)
            {
                ActivarBoton(sender, RGBColors.color2);
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                historial.Checked = false;
                partidasJugadas.Checked = false;
                partidasGanadas.Checked = false;
                experiencia.Checked = false;
                maxGanador.Checked = false;
                experienciaTotal.Checked = false;
                adversarios.Checked = false;
                partidasMarcados.Checked = false;
                partidasTiempo.Checked = false;
                perfilLbl.Text = null;
                historialGridView.Visible = false;
                historialGridView.Columns.Clear();
                historialGridView.Rows.Clear();
                dateTimePicker1.Visible = false;
                dateTimePicker2.Visible = false;
                marcadosBox.Visible = false;
                marcadosBox.Text = null;
                tabControl1.Show();
                tabControl1.SelectedTab = tabControl1.TabPages["perfilPage"];
            }
            else
            {
                notificaciónLbl.Text="Todavía no estás logueado.";
            }
        }

        private void JugarBtn_Click(object sender, EventArgs e)
        {
            if (logueado == 1)
            {
                ActivarBoton(sender, RGBColors.color3);
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                tabControl1.Show();
                notificaciónJLbl.Text = null;
                tabControl1.SelectedTab = tabControl1.TabPages["invitarPage"];
            }
            else
            {
                notificaciónLbl.Text = "Todavía no estás logueado.";
            }
        }

        private void InstruccionesBtn_Click(object sender, EventArgs e)
        {
            if (notificaciónLbl.Text == "Todavía no estás logueado.")
            {
                notificaciónLbl.Text = null;
            }
            ActivarBoton(sender, RGBColors.color4);
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            tabControl1.Show();
            tabControl1.SelectedTab = tabControl1.TabPages["instruccionesPage"];
        }

        private void MazoBtn_Click(object sender, EventArgs e)
        {
            if (logueado == 1)
            {
                ActivarBoton(sender, RGBColors.color5);
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                tabControl1.Show();
                string mensaje = "15/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                tabControl1.SelectedTab = tabControl1.TabPages["mazoPage"];
            }
            else
            {
                notificaciónLbl.Text = "Todavía no estás logueado.";
            }
        }

        private void DesconectarBtn_Click(object sender, EventArgs e)
        {
            if (partidasActivas == 0)
            {
                ActivarBoton(sender, RGBColors.color6);
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                BorrarMazo();

                // Mensaje de desconexión
                string mensaje = "0/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Nos desconectamos
                logueado = 0;
                connected = 0;
                atender.Abort();
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                DesconectarBtn.Visible = false;
                nivelLbl.Visible = false;
                nivelProgressBar.Visible = false;
                progresoLbl.Visible = false;
                TablaConectados.Columns.Clear();
                TablaConectados.Rows.Clear();
                TablaConectados.Hide();
                Login.Checked = false;
                usuario.Text = null;
                password.Text = null;
                notificaciónLbl.Text = null;
                Resetear();
            }
            else
            {
                notificaciónLbl.Text = "Aún tienes partidas sin acabar.";
            }
        }

        private void InicioBtn_Click(object sender, EventArgs e)
        {
            Resetear();
        }

        private void Resetear()
        {
            DeshabilitarBoton();
            leftBorderBtn.Visible = false;

            iconActual.IconChar = IconChar.Home;
            iconActual.IconColor = Color.Violet;
            TituloLbl.Text = "Inicio";

            tabControl1.Show();
            tabControl1.SelectedTab = tabControl1.TabPages["homePage"];

            axWindowsMediaPlayer1.Ctlcontrols.play();

            notificaciónLbl.Text = null;
        }

        private void invitarBtn_Click(object sender, EventArgs e)
        {
            if (cont == 6)
            {
                List<string> invitados = new List<string>();
                int i = 0;
                while (i < TablaConectados.RowCount)
                {
                    if (Convert.ToBoolean(TablaConectados.Rows[i].Cells[1].Value))
                    {
                        invitados.Add(Convert.ToString(TablaConectados.Rows[i].Cells[0].Value));
                        TablaConectados.Rows[i].Cells[1].Value = false;
                    }
                    i = i + 1;
                }
                //Quiere invitar a algunos jugadores
                if (invitados.Count() == 0)
                {
                    notificaciónJLbl.Text = "Tienes que invitar a alguien.";
                }
                else if (invitados.Count > 3)
                {
                    notificaciónJLbl.Text = "El número máximo de personas invitadas es de 3.";
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
                    notificaciónJLbl.Text = "Invitaciones enviadas.";
                }
            }
            else
            {
                notificaciónJLbl.Text = "Aún no tienes ningún mazo seleccionado.";
            }
        }

        private void confirmarMazoBtn_Click(object sender, EventArgs e)
        {
            if (cont == 6)
            {
                int i = 0;
                string mensaje = "13";
                while (i < cont)
                {
                    if (selección[i] != 0)
                    {
                        mazo[i] = selección[i];
                        mensaje = mensaje + "/" + selección[i];
                    }
                    i++;
                }
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else
            {
                notificaciónLbl.Text = "Se necesitan 6 cartas en el mazo.";
            }
        }

        private void borrarMazoBtn_Click(object sender, EventArgs e)
        {
            if (cont != 0)
            {
                string mensaje = "14/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else
            {
                notificaciónLbl.Text = "No hay ninguna carta en el mazo.";
            }
        }

        private void salirBtn_Click(object sender, EventArgs e)
        {
            tabControl1.Show();
            tabControl1.SelectedTab = tabControl1.TabPages["mazoPage"];
            id = 0;
        }

        private void AEBtn_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[id - 1] == 1)
            {
                cont--;
                if (selección[0] == id)
                {
                    DevolverImagen(id);
                    cartasEnMazo[id - 1] = 0;
                    selección[0] = 0;
                    id = 0;
                    AñadirImagen(id, mazo1);
                }
                else if (selección[1] == id)
                {
                    DevolverImagen(id);
                    cartasEnMazo[id - 1] = 0;
                    selección[1] = 0;
                    id = 0;
                    AñadirImagen(id, mazo2);
                }
                else if (selección[2] == id)
                {
                    DevolverImagen(id);
                    cartasEnMazo[id - 1] = 0;
                    selección[2] = 0;
                    id = 0;
                    AñadirImagen(id, mazo3);
                }
                else if (selección[3] == id)
                {
                    DevolverImagen(id);
                    cartasEnMazo[id - 1] = 0;
                    selección[3] = 0;
                    id = 0;
                    AñadirImagen(id, mazo4);
                }
                else if (selección[4] == id)
                {
                    DevolverImagen(id);
                    cartasEnMazo[id - 1] = 0;
                    selección[4] = 0;
                    id = 0;
                    AñadirImagen(id, mazo5);
                }
                else if (selección[5] == id)
                {
                    DevolverImagen(id);
                    cartasEnMazo[id - 1] = 0;
                    selección[5] = 0;
                    id = 0;
                    AñadirImagen(id, mazo6);
                }
                tabControl1.Show();
                tabControl1.SelectedTab = tabControl1.TabPages["mazoPage"];
            }
            else if (cartasEnMazo[id - 1] == 0)
            {
                if (selección[0] == 0)
                {
                    cont++;
                    cartasEnMazo[id - 1] = 1;
                    selección[0] = id;
                    AñadirImagen(id, mazo1);
                    tabControl1.Show();
                    tabControl1.SelectedTab = tabControl1.TabPages["mazoPage"];
                }
                else if (selección[1] == 0)
                {
                    cont++;
                    cartasEnMazo[id - 1] = 1;
                    selección[1] = id;
                    AñadirImagen(id, mazo2);
                    tabControl1.Show();
                    tabControl1.SelectedTab = tabControl1.TabPages["mazoPage"];
                }
                else if (selección[2] == 0)
                {
                    cont++;
                    cartasEnMazo[id - 1] = 1;
                    selección[2] = id;
                    AñadirImagen(id, mazo3);
                    tabControl1.Show();
                    tabControl1.SelectedTab = tabControl1.TabPages["mazoPage"];
                }
                else if (selección[3] == 0)
                {
                    cont++;
                    cartasEnMazo[id - 1] = 1;
                    selección[3] = id;
                    AñadirImagen(id, mazo4);
                    tabControl1.Show();
                    tabControl1.SelectedTab = tabControl1.TabPages["mazoPage"];
                }
                else if (selección[4] == 0)
                {
                    cont++;
                    cartasEnMazo[id - 1] = 1;
                    selección[4] = id;
                    AñadirImagen(id, mazo5);
                    tabControl1.Show();
                    tabControl1.SelectedTab = tabControl1.TabPages["mazoPage"];
                }
                else if (selección[5] == 0)
                {
                    cont++;
                    cartasEnMazo[id - 1] = 1;
                    selección[5] = id;
                    AñadirImagen(id, mazo6);
                    tabControl1.Show();
                    tabControl1.SelectedTab = tabControl1.TabPages["mazoPage"];
                }
                else
                {
                    notificaciónLbl.Text = "Ya tienes 6 cartas en el mazo.";
                }
                BorrarImagen();
            }
        }
        
        private void mazo1_Click(object sender, EventArgs e)
        {
            if (selección[0] != 0)
            {
                AEBtn.Text = "Eliminar Carta";
                id = selección[0];
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void mazo2_Click(object sender, EventArgs e)
        {
            if (selección[1] != 0)
            {
                AEBtn.Text = "Eliminar Carta";
                id = selección[1];
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void mazo3_Click(object sender, EventArgs e)
        {
            if (selección[2] != 0)
            {
                AEBtn.Text = "Eliminar Carta";
                id = selección[2];
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void mazo4_Click(object sender, EventArgs e)
        {
            if (selección[3] != 0)
            {
                AEBtn.Text = "Eliminar Carta";
                id = selección[3];
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void mazo5_Click(object sender, EventArgs e)
        {
            if (selección[4] != 0)
            {
                AEBtn.Text = "Eliminar Carta";
                id = selección[4];
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void mazo6_Click(object sender, EventArgs e)
        {
            if (selección[5] != 0)
            {
                AEBtn.Text = "Eliminar Carta";
                id = selección[5];
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }
        
        private void carta1_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[0] == 0)
            {
                AEBtn.Text = "Añadir Carta";
                id = 1;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta2_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[1] == 0)
            {
                AEBtn.Text = "Añadir Carta";
                id = 2;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta3_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[2] == 0)
            {
                AEBtn.Text = "Añadir Carta";
                id = 3;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta4_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[3] == 0)
            {
                AEBtn.Text = "Añadir Carta";
                id = 4;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta5_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[4] == 0)
            {
                AEBtn.Text = "Añadir Carta";
                id = 5;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta6_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[5] == 0)
            {
                AEBtn.Text = "Añadir Carta";
                id = 6;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta7_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[6] == 0)
            {
                AEBtn.Text = "Añadir Carta";
                id = 7;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta8_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[7] == 0)
            {
                AEBtn.Text = "Añadir Carta";
                id = 8;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta9_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[8] == 0)
            {
                AEBtn.Text = "Añadir Carta";
                id = 9;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta10_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[9] == 0)
            {
                AEBtn.Text = "Añadir Carta";
                id = 10;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta11_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[10] == 0)
            {
                AEBtn.Text = "Añadir Carta";
                id = 11;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta12_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[11] == 0)
            {
                AEBtn.Text = "Añadir Carta";
                id = 12;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta13_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[12] == 0 && nivel >= 2)
            {
                AEBtn.Text = "Añadir Carta";
                id = 13;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta14_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[13] == 0 && nivel >= 2)
            {
                AEBtn.Text = "Añadir Carta";
                id = 14;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta15_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[14] == 0 && nivel >= 2)
            {
                AEBtn.Text = "Añadir Carta";
                id = 15;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta16_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[15] == 0 && nivel >= 2)
            {
                AEBtn.Text = "Añadir Carta";
                id = 16;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta17_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[16] == 0 && nivel >= 3)
            {
                AEBtn.Text = "Añadir Carta";
                id = 17;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta18_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[17] == 0 && nivel >= 3)
            {
                AEBtn.Text = "Añadir Carta";
                id = 18;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta19_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[18] == 0 && nivel >= 3)
            {
                AEBtn.Text = "Añadir Carta";
                id = 19;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta20_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[19] == 0 && nivel >= 3)
            {
                AEBtn.Text = "Añadir Carta";
                id = 20;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta21_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[20] == 0 && nivel >= 4)
            {
                AEBtn.Text = "Añadir Carta";
                id = 21;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta22_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[21] == 0 && nivel >= 4)
            {
                AEBtn.Text = "Añadir Carta";
                id = 22;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta23_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[22] == 0 && nivel >= 4)
            {
                AEBtn.Text = "Añadir Carta";
                id = 23;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta24_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[23] == 0 && nivel >= 4)
            {
                AEBtn.Text = "Añadir Carta";
                id = 24;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta25_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[24] == 0 && nivel >= 5)
            {
                AEBtn.Text = "Añadir Carta";
                id = 25;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta26_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[25] == 0 && nivel >= 5)
            {
                AEBtn.Text = "Añadir Carta";
                id = 26;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta27_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[26] == 0 && nivel >= 5)
            {
                AEBtn.Text = "Añadir Carta";
                id = 27;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta28_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[27] == 0 && nivel >= 6)
            {
                AEBtn.Text = "Añadir Carta";
                id = 28;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta29_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[28] == 0 && nivel >= 6)
            {
                AEBtn.Text = "Añadir Carta";
                id = 29;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta30_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[29] == 0 && nivel >= 6)
            {
                AEBtn.Text = "Añadir Carta";
                id = 30;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta31_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[30] == 0 && nivel >= 7)
            {
                AEBtn.Text = "Añadir Carta";
                id = 31;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta32_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[31] == 0 && nivel >= 7)
            {
                AEBtn.Text = "Añadir Carta";
                id = 32;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta33_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[32] == 0 && nivel >= 8)
            {
                AEBtn.Text = "Añadir Carta";
                id = 33;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta34_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[33] == 0 && nivel >= 8)
            {
                AEBtn.Text = "Añadir Carta";
                id = 34;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta35_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[34] == 0 && nivel >= 9)
            {
                AEBtn.Text = "Añadir Carta";
                id = 35;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void carta36_Click(object sender, EventArgs e)
        {
            if (cartasEnMazo[35] == 0 && nivel >= 10)
            {
                AEBtn.Text = "Añadir Carta";
                id = 36;
                string mensaje = "12/" + id;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void AñadirImagen(int id, IconPictureBox iconPictureBox)
        {
            if (id == 0)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.interrogante;
            }
            if (id == 1)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe1;
            }
            if (id == 2)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe2;
            }
            if (id == 3)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe3;
            }
            if (id == 4)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe4;
            }
            if (id == 5)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe5;
            }
            if (id == 6)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe6;
            }
            if (id == 7)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe7;
            }
            if (id == 8)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe8;
            }
            if (id == 9)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe9;
            }
            if (id == 10)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe10;
            }
            if (id == 11)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe11;
            }
            if (id == 12)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe12;
            }
            if (id == 13)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe13;
            }
            if (id == 14)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe14;
            }
            if (id == 15)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe15;
            }
            if (id == 16)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe16;
            }
            if (id == 17)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe17;
            }
            if (id == 18)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe18;
            }
            if (id == 19)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe19;
            }
            if (id == 20)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe20;
            }
            if (id == 21)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe21;
            }
            if (id == 22)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe22;
            }
            if (id == 23)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe23;
            }
            if (id == 24)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe24;
            }
            if (id == 25)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe25;
            }
            if (id == 26)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe26;
            }
            if (id == 27)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe27;
            }
            if (id == 28)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe28;
            }
            if (id == 29)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe29;
            }
            if (id == 30)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe30;
            }
            if (id == 31)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe31;
            }
            if (id == 32)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe32;
            }
            if (id == 33)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe33;
            }
            if (id == 34)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe34;
            }
            if (id == 35)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe35;
            }
            if (id == 36)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Heroe36;
            }
        }

        private void DevolverImagen(int id)
        {
            if (id == 1)
            {
                carta1.BackgroundImage = Properties.Resources.Heroe1;
            }
            if (id == 2)
            {
                carta2.BackgroundImage = Properties.Resources.Heroe2;
            }
            if (id == 3)
            {
                carta3.BackgroundImage = Properties.Resources.Heroe3;
            }
            if (id == 4)
            {
                carta4.BackgroundImage = Properties.Resources.Heroe4;
            }
            if (id == 5)
            {
                carta5.BackgroundImage = Properties.Resources.Heroe5;
            }
            if (id == 6)
            {
                carta6.BackgroundImage = Properties.Resources.Heroe6;
            }
            if (id == 7)
            {
                carta7.BackgroundImage = Properties.Resources.Heroe7;
            }
            if (id == 8)
            {
                carta8.BackgroundImage = Properties.Resources.Heroe8;
            }
            if (id == 9)
            {
                carta9.BackgroundImage = Properties.Resources.Heroe9;
            }
            if (id == 10)
            {
                carta10.BackgroundImage = Properties.Resources.Heroe10;
            }
            if (id == 11)
            {
                carta11.BackgroundImage = Properties.Resources.Heroe11;
            }
            if (id == 12)
            {
                carta12.BackgroundImage = Properties.Resources.Heroe12;
            }
            if (id == 13)
            {
                carta13.BackgroundImage = Properties.Resources.Heroe13;
            }
            if (id == 14)
            {
                carta14.BackgroundImage = Properties.Resources.Heroe14;
            }
            if (id == 15)
            {
                carta15.BackgroundImage = Properties.Resources.Heroe15;
            }
            if (id == 16)
            {
                carta16.BackgroundImage = Properties.Resources.Heroe16;
            }
            if (id == 17)
            {
                carta17.BackgroundImage = Properties.Resources.Heroe17;
            }
            if (id == 18)
            {
                carta18.BackgroundImage = Properties.Resources.Heroe18;
            }
            if (id == 19)
            {
                carta19.BackgroundImage = Properties.Resources.Heroe19;
            }
            if (id == 20)
            {
                carta20.BackgroundImage = Properties.Resources.Heroe20;
            }
            if (id == 21)
            {
                carta21.BackgroundImage = Properties.Resources.Heroe21;
            }
            if (id == 22)
            {
                carta22.BackgroundImage = Properties.Resources.Heroe22;
            }
            if (id == 23)
            {
                carta23.BackgroundImage = Properties.Resources.Heroe23;
            }
            if (id == 24)
            {
                carta24.BackgroundImage = Properties.Resources.Heroe24;
            }
            if (id == 25)
            {
                carta25.BackgroundImage = Properties.Resources.Heroe25;
            }
            if (id == 26)
            {
                carta26.BackgroundImage = Properties.Resources.Heroe26;
            }
            if (id == 27)
            {
                carta27.BackgroundImage = Properties.Resources.Heroe27;
            }
            if (id == 28)
            {
                carta28.BackgroundImage = Properties.Resources.Heroe28;
            }
            if (id == 29)
            {
                carta29.BackgroundImage = Properties.Resources.Heroe29;
            }
            if (id == 30)
            {
                carta30.BackgroundImage = Properties.Resources.Heroe30;
            }
            if (id == 31)
            {
                carta31.BackgroundImage = Properties.Resources.Heroe31;
            }
            if (id == 32)
            {
                carta32.BackgroundImage = Properties.Resources.Heroe32;
            }
            if (id == 33)
            {
                carta33.BackgroundImage = Properties.Resources.Heroe33;
            }
            if (id == 34)
            {
                carta34.BackgroundImage = Properties.Resources.Heroe34;
            }
            if (id == 35)
            {
                carta35.BackgroundImage = Properties.Resources.Heroe35;
            }
            if (id == 36)
            {
                carta36.BackgroundImage = Properties.Resources.Heroe36;
            }
        }

        private void BorrarImagen()
        {
            int i = 0;
            while (i < 6)
            {
                if (selección[i] == 1)
                {
                    carta1.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 2)
                {
                    carta2.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 3)
                {
                    carta3.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 4)
                {
                    carta4.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 5)
                {
                    carta5.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 6)
                {
                    carta6.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 7)
                {
                    carta7.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 8)
                {
                    carta8.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 9)
                {
                    carta9.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 10)
                {
                    carta10.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 11)
                {
                    carta11.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 12)
                {
                    carta12.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 13)
                {
                    carta13.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 14)
                {
                    carta14.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 15)
                {
                    carta15.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 16)
                {
                    carta16.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 17)
                {
                    carta17.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 18)
                {
                    carta18.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 19)
                {
                    carta19.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 20)
                {
                    carta20.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 21)
                {
                    carta21.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 22)
                {
                    carta22.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 23)
                {
                    carta23.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 24)
                {
                    carta24.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 25)
                {
                    carta25.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 26)
                {
                    carta26.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 27)
                {
                    carta27.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 28)
                {
                    carta28.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 29)
                {
                    carta29.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 30)
                {
                    carta30.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 31)
                {
                    carta31.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 32)
                {
                    carta32.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 33)
                {
                    carta33.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 34)
                {
                    carta34.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 35)
                {
                    carta35.BackgroundImage = Properties.Resources.interrogante;
                }
                if (selección[i] == 36)
                {
                    carta36.BackgroundImage = Properties.Resources.interrogante;
                }
                i++;
            }
        }

        private void BorrarMazo()
        {
            cont = 0;
            int i = 0;
            while (i < 6)
            {
                selección[i] = 0;
                i++;
            }
            int j = 0;
            while (j < 36)
            {
                cartasEnMazo[j] = 0;
                j++;
            }
            id = 0;
            mazo1.BackgroundImage = Properties.Resources.interrogante;
            mazo2.BackgroundImage = Properties.Resources.interrogante;
            mazo3.BackgroundImage = Properties.Resources.interrogante;
            mazo4.BackgroundImage = Properties.Resources.interrogante;
            mazo5.BackgroundImage = Properties.Resources.interrogante;
            mazo6.BackgroundImage = Properties.Resources.interrogante;
        }

        private void partidasTiempo_CheckedChanged(object sender, EventArgs e)
        {
            perfilLbl.Text = null;
            if (dateTimePicker1.Visible == true && dateTimePicker2.Visible == true)
            {
                dateTimePicker1.Visible = false;
                dateTimePicker2.Visible = false;
            }
            else if (dateTimePicker1.Visible == false && dateTimePicker2.Visible == false)
            {
                dateTimePicker1.Visible = true;
                dateTimePicker2.Visible = true;
            }
        }

        private void partidasMarcados_CheckedChanged(object sender, EventArgs e)
        {
            perfilLbl.Text = null;
            marcadosBox.Text = null;
            if (marcadosBox.Visible == true)
            {
                marcadosBox.Visible = false;
            }
            else if (marcadosBox.Visible == false)
            {
                marcadosBox.Visible = true;
            }
        }
    }
}