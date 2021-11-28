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
    public partial class Form2 : Form
    {
        int numForm;
        Socket server;
        int id;
        delegate void DelegadoParaMensajeChat(string jugador, string mensaje);

        public Form2(int numForm, Socket server, int id)
        {
            InitializeComponent();
            this.numForm = numForm;
            this.server = server;
            this.id = id;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            numFormlbl.Text = Convert.ToString(numForm);
        }

        private void ChatBtn_Click(object sender, EventArgs e)
        {
            if (ChatTextBox.Text != "")
            {
                string mensaje = "9/" + id + "/" + numForm + "/" + ChatTextBox.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            ChatTextBox.Clear();
        }

        public void TomaMensajeChat(string jugador, string mensaje)
        {
            ChatBox.Items.Add(jugador + ": " + mensaje);
            ChatBox.TopIndex = ChatBox.Items.Count - 1;
        }

        public int Partida()
        {
            return id;
        }
    }
}
