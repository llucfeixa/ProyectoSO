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
    public partial class Form2 : Form
    {
        int numForm;
        int segundos = 0;
        int seleccionada = 0;
        Socket server;
        int idPartida;
        int j;
        int arriba;
        int izquierda;
        int derecha;
        string[] nombres;
        int[] cartas;
        int[] vida;
        int[] vidaInicial;
        int turnoAtaque = 0;
        int turnoDefensa = 1;
        int[] usadas = new int[6];
        int cont = 0;
        int posición;
        int salir = 0;
        string fecha;
        int lanzada;

        public Form2(int numForm, Socket server, int idPartida, int k, string[] nombresJugadores, int[] selección, int[] vidaJugadores, int[] vidaInicialJugadores)
        {
            InitializeComponent();
            this.timer.Start();
            this.numForm = numForm;
            this.server = server;
            this.idPartida = idPartida;
            this.j = k;
            this.nombres = nombresJugadores;
            this.cartas = selección;
            this.vida = vidaJugadores;
            this.vidaInicial = vidaInicialJugadores;
            this.posición = nombres.Length;
            this.fecha = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            OrdenaJugadores();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            numFormlbl.Text = Convert.ToString(numForm);
            salirBtn.Visible = false;
            nombreLbl.Visible = false;
            APicBox.Visible = false;
            DPicBox.Visible = false;
            ataqueLbl.Visible = false;
            defensaLbl.Visible = false;
            lanzarBtn.Visible = false;
            atacaLbl.Text = nombres[turnoAtaque];
            defiendeLbl.Text = nombres[turnoDefensa];
            if (turnoAtaque == j)
            {
                notificaciónULbl.Text = "Te toca ATACAR!";
            }
            if (turnoDefensa == j)
            {
                notificaciónULbl.Text = "Te toca DEFENDER!";
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.segundos++;
        }

        private void timerAcción_Tick(object sender, EventArgs e)
        {
            if (nombres.Length > 1)
            {
                ataquePicBox.BackgroundImage = Properties.Resources.interrogante;
                defensaPicBox.BackgroundImage = Properties.Resources.interrogante;
                if (turnoAtaque == j)
                {
                    notificaciónULbl.Text = "Te toca ATACAR!";
                    lanzada = 0;
                }
                if (turnoDefensa == j)
                {
                    notificaciónULbl.Text = "Te toca DEFENDER!";
                    lanzada = 0;
                }
                atacaLbl.Text = nombres[turnoAtaque];
                defiendeLbl.Text = nombres[turnoDefensa];
                ResetearUsadas();
                notificaciónLbl.Text = null;
                if (j != -1)
                    if (vida[j] == 0)
                    {
                        string mensaje = "17/" + idPartida + "/0";
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                        notificaciónULbl.Text = null;
                    }
            }
            timerAcción.Stop();
        }

        private void ChatBtn_Click(object sender, EventArgs e)
        {
            if (ChatTextBox.Text != "")
            {
                string mensaje = "9/" + idPartida + "/" + ChatTextBox.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            ChatTextBox.Clear();
        }

        public void TomaMensajeChat(int caso, string jugador, string mensaje)
        {
            if (caso == 1)
            {
                ChatBox.AppendText(jugador + ": " + mensaje + Environment.NewLine);
            }
            else if (caso == 2)
            {
                notificaciónLbl.Text = jugador + mensaje;
            }
        }

        public void TomaNotificaciones(int caso, string jugador)
        {
            if (caso == -1)
            {
                notificaciónLbl.Text = jugador + " ha ABANDONADO en medio de la partida";
            }
            if (caso == 0)
            {
                notificaciónLbl.Text = jugador + " ha sido ELIMINADO al perder toda la VIDA";
            }
            if (nombres.Length == 1)
            {
                notificaciónLbl.Text += " y " + nombres[0] + " ha GANADO la partida.";
            }
            else
            {
                notificaciónLbl.Text += ".";
            }
        }

        public void TomaNotificación(string mensaje)
        {
            notificaciónLbl.Text = mensaje;
        }

        public void TomaAtaqueDefensa(int idAtaque, int idDefensa, int vidaTotal, int turnoA, int turnoD)
        {
            notificaciónULbl.Text = null;
            AñadirImagen(idAtaque, ataquePicBox);
            AñadirImagen(idDefensa, defensaPicBox);
            if (turnoAtaque == j || turnoDefensa == j)
            {
                usadas[seleccionada] = 1;
                cont++;
                if (seleccionada == 0)
                    AñadirImagen(0, JBPicBox1);
                if (seleccionada == 1)
                    AñadirImagen(0, JBPicBox2);
                if (seleccionada == 2)
                    AñadirImagen(0, JBPicBox3);
                if (seleccionada == 3)
                    AñadirImagen(0, JBPicBox4);
                if (seleccionada == 4)
                    AñadirImagen(0, JBPicBox5);
                if (seleccionada == 5)
                    AñadirImagen(0, JBPicBox6);
            }
            if (vidaTotal == vida[turnoDefensa])
            {
                notificaciónLbl.Text = nombres[turnoDefensa] + " se ha defendido PERFECTAMENTE del ataque de " + nombres[turnoAtaque] + ".";
            }
            else
            {
                int vidaRestada = vida[turnoDefensa] - vidaTotal;
                notificaciónLbl.Text = nombres[turnoAtaque] + " le ha bajado " + vidaRestada + " puntos de VIDA a " + nombres[turnoDefensa] + ".";
                vida[turnoDefensa] = vidaTotal;
            }
            int progreso = Convert.ToInt32((Convert.ToDouble(vidaTotal) / Convert.ToDouble(vidaInicial[turnoDefensa])) * 100);
            if (j != -1)
            {
                if (turnoDefensa == j)
                {
                    this.progressBarJB.Value = progreso;
                    this.vidaLblJB.Text = Convert.ToString(vidaTotal);
                }
            }
            else
            {
                if (turnoDefensa == 0)
                {
                    this.progressBarJB.Value = progreso;
                    this.vidaLblJB.Text = Convert.ToString(vidaTotal);
                }
            }
            if (turnoDefensa == arriba)
            {
                this.progressBarJA.Value = progreso;
                this.vidaLblJA.Text = Convert.ToString(vidaTotal);
            }
            if (turnoDefensa == izquierda)
            {
                this.progressBarJI.Value = progreso;
                this.vidaLblJI.Text = Convert.ToString(vidaTotal);
            }
            if (turnoDefensa == derecha)
            {
                this.progressBarJD.Value = progreso;
                this.vidaLblJD.Text = Convert.ToString(vidaTotal);
            }
            turnoAtaque = turnoA;
            turnoDefensa = turnoD;
            timerAcción.Start();
        }

        public void AtaqueDefensaCarta(string nombre, int ataque, int defensa)
        {
            nombreLbl.Visible = true;
            nombreLbl.Text = nombre;
            APicBox.Visible = true;
            DPicBox.Visible = true;
            ataqueLbl.Visible = true;
            ataqueLbl.Text = Convert.ToString(ataque);
            defensaLbl.Visible = true;
            defensaLbl.Text = Convert.ToString(defensa);
            lanzarBtn.Visible = true;
        }

        public int Partida()
        {
            return idPartida;
        }

        private void JBPicBox1_Click(object sender, EventArgs e)
        {
            if ((turnoAtaque == j || turnoDefensa == j) && j != -1 && lanzada == 0 && nombres.Length > 1)
            {
                if (usadas[0] == 0)
                {
                    seleccionada = 0;
                    string mensaje = "21/" + idPartida + "/" + cartas[0];
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                {
                    nombreLbl.Visible = false;
                    APicBox.Visible = false;
                    DPicBox.Visible = false;
                    ataqueLbl.Visible = false;
                    defensaLbl.Visible = false;
                    lanzarBtn.Visible = false;
                }
            }
        }

        private void JBPicBox2_Click(object sender, EventArgs e)
        {
            if ((turnoAtaque == j || turnoDefensa == j) && j != -1 && lanzada == 0 && nombres.Length > 1)
            {
                if (usadas[1] == 0)
                {
                    seleccionada = 1;
                    string mensaje = "21/" + idPartida + "/" + cartas[1];
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                {
                    nombreLbl.Visible = false;
                    APicBox.Visible = false;
                    DPicBox.Visible = false;
                    ataqueLbl.Visible = false;
                    defensaLbl.Visible = false;
                    lanzarBtn.Visible = false;
                }
            }
        }

        private void JBPicBox3_Click(object sender, EventArgs e)
        {
            if ((turnoAtaque == j || turnoDefensa == j) && j != -1 && lanzada == 0 && nombres.Length > 1)
            {
                if (usadas[2] == 0)
                {
                    seleccionada = 2;
                    string mensaje = "21/" + idPartida + "/" + cartas[2];
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                {
                    nombreLbl.Visible = false;
                    APicBox.Visible = false;
                    DPicBox.Visible = false;
                    ataqueLbl.Visible = false;
                    defensaLbl.Visible = false;
                    lanzarBtn.Visible = false;
                }
            }
        }

        private void JBPicBox4_Click(object sender, EventArgs e)
        {
            if ((turnoAtaque == j || turnoDefensa == j) && j != -1 && lanzada == 0 && nombres.Length > 1)
            {
                if (usadas[3] == 0)
                {
                    seleccionada = 3;
                    string mensaje = "21/" + idPartida + "/" + cartas[3];
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                {
                    nombreLbl.Visible = false;
                    APicBox.Visible = false;
                    DPicBox.Visible = false;
                    ataqueLbl.Visible = false;
                    defensaLbl.Visible = false;
                    lanzarBtn.Visible = false;
                }
            }
        }

        private void JBPicBox5_Click(object sender, EventArgs e)
        {
            if ((turnoAtaque == j || turnoDefensa == j) && j != -1 && lanzada == 0 && nombres.Length > 1)
            {
                if (usadas[4] == 0)
                {
                    seleccionada = 4;
                    string mensaje = "21/" + idPartida + "/" + cartas[4];
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                {
                    nombreLbl.Visible = false;
                    APicBox.Visible = false;
                    DPicBox.Visible = false;
                    ataqueLbl.Visible = false;
                    defensaLbl.Visible = false;
                    lanzarBtn.Visible = false;
                }
            }
        }

        private void JBPicBox6_Click(object sender, EventArgs e)
        {
            if ((turnoAtaque == j || turnoDefensa == j) && j != -1 && lanzada == 0 && nombres.Length > 1)
            {
                if (usadas[5] == 0)
                {
                    seleccionada = 5;
                    string mensaje = "21/" + idPartida + "/" + cartas[5];
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                {
                    nombreLbl.Visible = false;
                    APicBox.Visible = false;
                    DPicBox.Visible = false;
                    ataqueLbl.Visible = false;
                    defensaLbl.Visible = false;
                    lanzarBtn.Visible = false;
                }
            }
        }

        private void ResetearUsadas()
        {
            if (cont == 6)
            {
                int i = 0;
                while (i < cont)
                {
                    usadas[i] = 0;
                    i++;
                }
                cont = 0;
                AñadirImagen(cartas[0], JBPicBox1);
                AñadirImagen(cartas[1], JBPicBox2);
                AñadirImagen(cartas[2], JBPicBox3);
                AñadirImagen(cartas[3], JBPicBox4);
                AñadirImagen(cartas[4], JBPicBox5);
                AñadirImagen(cartas[5], JBPicBox6);
            }
        }

        private void AñadirImagen(int id, IconPictureBox iconPictureBox)
        {
            if (id == 0)
            {
                iconPictureBox.BackgroundImage = Properties.Resources.Abajo;
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

        public void ActualizaForm(int turnoA, int turnoD, string[] nombresActualizados, int[] vidaActualizada, int[] vidaInicialActualizada)
        {
            timerAcción.Stop();
            ataquePicBox.BackgroundImage = Properties.Resources.interrogante;
            defensaPicBox.BackgroundImage = Properties.Resources.interrogante;
            ResetearUsadas();
            notificaciónLbl.Text = null;
            int p = 0;
            while (p < nombresActualizados.Length)
            {
                if (nombres[turnoAtaque] != nombresActualizados[p] || nombres[turnoDefensa] != nombresActualizados[p])
                {
                    lanzada = 0;
                }
                p++;
            }
            int i = 0;
            bool encontrado = false;
            while (i < nombresActualizados.Length && !encontrado && j != -1)
            {
                if (nombresActualizados[i] == nombres[j])
                {
                    j = i;
                    encontrado = true;
                }
                if (!encontrado)
                {
                    i++;
                }
            }
            if (!encontrado)
            {
                j = -1;
            }
            nombres = nombresActualizados;
            vida = vidaActualizada;
            vidaInicial = vidaInicialActualizada;
            posición--;
            if (nombres.Length > 1)
            {
                turnoAtaque = turnoA;
                turnoDefensa = turnoD;
                atacaLbl.Text= nombres[turnoAtaque];
                defiendeLbl.Text = nombres[turnoDefensa];
                if (turnoAtaque == j)
                {
                    notificaciónULbl.Text = "Te toca ATACAR!";
                }
                if (turnoDefensa == j)
                {
                    notificaciónULbl.Text = "Te toca DEFENDER!";
                }
            }
            else
            {
                timer.Stop();
                notificaciónULbl.Text = null;
            }
            OrdenaJugadores();
        }

        private void OrdenaJugadores()
        {
            int progreso;
            if (j != -1)
            {
                this.JugadorAbajo.Text = nombres[j];
                progreso = Convert.ToInt32((Convert.ToDouble(vida[j]) / Convert.ToDouble(vidaInicial[j])) * 100);
                this.progressBarJB.Value = progreso;
                this.vidaLblJB.Text = Convert.ToString(vida[j]);
                if (usadas[0] == 0)
                    AñadirImagen(cartas[0], JBPicBox1);
                if (usadas[1] == 0)
                    AñadirImagen(cartas[1], JBPicBox2);
                if (usadas[2] == 0)
                    AñadirImagen(cartas[2], JBPicBox3);
                if (usadas[3] == 0)
                    AñadirImagen(cartas[3], JBPicBox4);
                if (usadas[4] == 0)
                    AñadirImagen(cartas[4], JBPicBox5);
                if (usadas[5] == 0)
                    AñadirImagen(cartas[5], JBPicBox6);
                if (nombres.Length == 1)
                {
                    this.JAPicBox.Visible = false;
                    this.JugadorArriba.Visible = false;
                    this.vidaPicBoxJA.Visible = false;
                    this.progressBarJA.Visible = false;
                    this.vidaLblJA.Visible = false;
                    this.JDPicBox.Visible = false;
                    this.JugadorDerecha.Visible = false;
                    this.vidaPicBoxJD.Visible = false;
                    this.progressBarJD.Visible = false;
                    this.vidaLblJD.Visible = false;
                    this.JIPicBox.Visible = false;
                    this.JugadorIzquierda.Visible = false;
                    this.vidaPicBoxJI.Visible = false;
                    this.progressBarJI.Visible = false;
                    this.vidaLblJI.Visible = false;
                    salirBtn.Visible = true;
                }
                if (nombres.Length == 2)
                {
                    this.JDPicBox.Visible = false;
                    this.JugadorDerecha.Visible = false;
                    this.vidaPicBoxJD.Visible = false;
                    this.progressBarJD.Visible = false;
                    this.vidaLblJD.Visible = false;
                    this.JIPicBox.Visible = false;
                    this.JugadorIzquierda.Visible = false;
                    this.vidaPicBoxJI.Visible = false;
                    this.progressBarJI.Visible = false;
                    this.vidaLblJI.Visible = false;
                    arriba = (j + 1) % 2;
                    this.JugadorArriba.Text = nombres[arriba];
                    progreso = Convert.ToInt32((Convert.ToDouble(vida[arriba]) / Convert.ToDouble(vidaInicial[arriba])) * 100);
                    this.progressBarJA.Value = progreso;
                    this.vidaLblJA.Text = Convert.ToString(vida[arriba]);
                }
                else if (nombres.Length == 3)
                {
                    this.JDPicBox.Visible = false;
                    this.JugadorDerecha.Visible = false;
                    this.vidaPicBoxJD.Visible = false;
                    this.progressBarJD.Visible = false;
                    this.vidaLblJD.Visible = false;
                    izquierda = (j + 1) % 3;
                    this.JugadorIzquierda.Text = nombres[izquierda];
                    progreso = Convert.ToInt32((Convert.ToDouble(vida[izquierda]) / Convert.ToDouble(vidaInicial[izquierda])) * 100);
                    this.progressBarJI.Value = progreso;
                    this.vidaLblJI.Text = Convert.ToString(vida[izquierda]);
                    arriba = (j + 2) % 3;
                    this.JugadorArriba.Text = nombres[arriba];
                    progreso = Convert.ToInt32((Convert.ToDouble(vida[arriba]) / Convert.ToDouble(vidaInicial[arriba])) * 100);
                    this.progressBarJA.Value = progreso;
                    this.vidaLblJA.Text = Convert.ToString(vida[arriba]);
                }
                else if (nombres.Length == 4)
                {
                    izquierda = (j + 1) % 4;
                    this.JugadorIzquierda.Text = nombres[izquierda];
                    progreso = Convert.ToInt32((Convert.ToDouble(vida[izquierda]) / Convert.ToDouble(vidaInicial[izquierda])) * 100);
                    this.progressBarJI.Value = progreso;
                    this.vidaLblJI.Text = Convert.ToString(vida[izquierda]);
                    arriba = (j + 2) % 4;
                    this.JugadorArriba.Text = nombres[arriba];
                    progreso = Convert.ToInt32((Convert.ToDouble(vida[arriba]) / Convert.ToDouble(vidaInicial[arriba])) * 100);
                    this.progressBarJA.Value = progreso;
                    this.vidaLblJA.Text = Convert.ToString(vida[arriba]);
                    derecha = (j + 3) % 4;
                    this.JugadorDerecha.Text = nombres[derecha];
                    progreso = Convert.ToInt32((Convert.ToDouble(vida[derecha]) / Convert.ToDouble(vidaInicial[derecha])) * 100);
                    this.progressBarJD.Value = progreso;
                    this.vidaLblJD.Text = Convert.ToString(vida[derecha]);
                }
            }
            else if (j == -1)
            {
                posición++;
                int p = 0;
                this.JugadorAbajo.Text = nombres[p];
                progreso = Convert.ToInt32((Convert.ToDouble(vida[p]) / Convert.ToDouble(vidaInicial[p])) * 100);
                this.progressBarJB.Value = progreso;
                this.vidaLblJB.Text = Convert.ToString(vida[p]);
                JBPicBox2.Visible = false;
                JBPicBox3.Visible = false;
                JBPicBox4.Visible = false;
                JBPicBox5.Visible = false;
                JBPicBox6.Visible = false;
                JBPicBox1.Location = new Point(JAPicBox.Location.X, JBPicBox1.Location.Y);
                AñadirImagen(0, JBPicBox1);
                salirBtn.Visible = true;
                if (nombres.Length == 1)
                {
                    this.JAPicBox.Visible = false;
                    this.JugadorArriba.Visible = false;
                    this.vidaPicBoxJA.Visible = false;
                    this.progressBarJA.Visible = false;
                    this.vidaLblJA.Visible = false;
                    this.JDPicBox.Visible = false;
                    this.JugadorDerecha.Visible = false;
                    this.vidaPicBoxJD.Visible = false;
                    this.progressBarJD.Visible = false;
                    this.vidaLblJD.Visible = false;
                    this.JIPicBox.Visible = false;
                    this.JugadorIzquierda.Visible = false;
                    this.vidaPicBoxJI.Visible = false;
                    this.progressBarJI.Visible = false;
                    this.vidaLblJI.Visible = false;
                }
                if (nombres.Length == 2)
                {
                    this.JDPicBox.Visible = false;
                    this.JugadorDerecha.Visible = false;
                    this.vidaPicBoxJD.Visible = false;
                    this.progressBarJD.Visible = false;
                    this.vidaLblJD.Visible = false;
                    this.JIPicBox.Visible = false;
                    this.JugadorIzquierda.Visible = false;
                    this.vidaPicBoxJI.Visible = false;
                    this.progressBarJI.Visible = false;
                    this.vidaLblJI.Visible = false;
                    arriba = (p + 1) % 2;
                    this.JugadorArriba.Text = nombres[arriba];
                    progreso = Convert.ToInt32((Convert.ToDouble(vida[arriba]) / Convert.ToDouble(vidaInicial[arriba])) * 100);
                    this.progressBarJA.Value = progreso;
                    this.vidaLblJA.Text = Convert.ToString(vida[arriba]);
                }
                else if (nombres.Length == 3)
                {
                    this.JDPicBox.Visible = false;
                    this.JugadorDerecha.Visible = false;
                    this.vidaPicBoxJD.Visible = false;
                    this.progressBarJD.Visible = false;
                    this.vidaLblJD.Visible = false;
                    izquierda = (p + 1) % 3;
                    this.JugadorIzquierda.Text = nombres[izquierda];
                    progreso = Convert.ToInt32((Convert.ToDouble(vida[izquierda]) / Convert.ToDouble(vidaInicial[izquierda])) * 100);
                    this.progressBarJI.Value = progreso;
                    this.vidaLblJI.Text = Convert.ToString(vida[izquierda]);
                    arriba = (p + 2) % 3;
                    this.JugadorArriba.Text = nombres[arriba];
                    progreso = Convert.ToInt32((Convert.ToDouble(vida[arriba]) / Convert.ToDouble(vidaInicial[arriba])) * 100);
                    this.progressBarJA.Value = progreso;
                    this.vidaLblJA.Text = Convert.ToString(vida[arriba]);
                }
            }
        }

        private void salirBtn_Click(object sender, EventArgs e)
        {
            salir = 1;
            string mensaje = "10/" + idPartida + "/" + posición;
            if (posición == 1)
                mensaje = mensaje + "/" + fecha + "/" + segundos;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            this.Close();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            string mensaje;
            if (salirBtn.Visible == true && salir == 0)
            {
                mensaje = "10/" + idPartida + "/" + posición;
                if (posición == 1)
                    mensaje = mensaje + "/" + fecha + "/" + segundos;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else if (salirBtn.Visible == false)
            {
                mensaje = "17/" + idPartida + "/-1";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        private void lanzarBtn_Click(object sender, EventArgs e)
        {
            lanzada = 1;
            string mensaje = "16/" + idPartida + "/" + cartas[seleccionada];
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            nombreLbl.Visible = false;
            APicBox.Visible = false;
            DPicBox.Visible = false;
            ataqueLbl.Visible = false;
            defensaLbl.Visible = false;
            lanzarBtn.Visible = false;
            notificaciónULbl.Text = "Carta lanzada, esperando al movimiento del rival.";
        }
    }
}