
namespace WindowsFormsApplication1
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.ChatTextBox = new System.Windows.Forms.TextBox();
            this.ChatBtn = new System.Windows.Forms.Button();
            this.JugadorIzquierda = new System.Windows.Forms.Label();
            this.JugadorDerecha = new System.Windows.Forms.Label();
            this.JugadorArriba = new System.Windows.Forms.Label();
            this.JugadorAbajo = new System.Windows.Forms.Label();
            this.numFormlbl = new System.Windows.Forms.Label();
            this.vidaLblJA = new System.Windows.Forms.Label();
            this.vidaLblJI = new System.Windows.Forms.Label();
            this.vidaLblJB = new System.Windows.Forms.Label();
            this.vidaLblJD = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBarJB = new System.Windows.Forms.ProgressBar();
            this.progressBarJI = new System.Windows.Forms.ProgressBar();
            this.progressBarJD = new System.Windows.Forms.ProgressBar();
            this.progressBarJA = new System.Windows.Forms.ProgressBar();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.ChatBox = new System.Windows.Forms.TextBox();
            this.salirBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.atacaLbl = new System.Windows.Forms.Label();
            this.defiendeLbl = new System.Windows.Forms.Label();
            this.notificaciónLbl = new System.Windows.Forms.Label();
            this.defensaLbl = new System.Windows.Forms.Label();
            this.ataqueLbl = new System.Windows.Forms.Label();
            this.lanzarBtn = new System.Windows.Forms.Button();
            this.nombreLbl = new System.Windows.Forms.Label();
            this.notificaciónULbl = new System.Windows.Forms.Label();
            this.DPicBox = new FontAwesome.Sharp.IconPictureBox();
            this.APicBox = new FontAwesome.Sharp.IconPictureBox();
            this.JBPicBox6 = new FontAwesome.Sharp.IconPictureBox();
            this.JBPicBox5 = new FontAwesome.Sharp.IconPictureBox();
            this.JBPicBox4 = new FontAwesome.Sharp.IconPictureBox();
            this.JBPicBox3 = new FontAwesome.Sharp.IconPictureBox();
            this.JBPicBox2 = new FontAwesome.Sharp.IconPictureBox();
            this.JBPicBox1 = new FontAwesome.Sharp.IconPictureBox();
            this.defensaPicBox = new FontAwesome.Sharp.IconPictureBox();
            this.ataquePicBox = new FontAwesome.Sharp.IconPictureBox();
            this.JDPicBox = new FontAwesome.Sharp.IconPictureBox();
            this.JAPicBox = new FontAwesome.Sharp.IconPictureBox();
            this.JIPicBox = new FontAwesome.Sharp.IconPictureBox();
            this.vidaPicBoxJD = new FontAwesome.Sharp.IconPictureBox();
            this.vidaPicBoxJB = new FontAwesome.Sharp.IconPictureBox();
            this.vidaPicBoxJI = new FontAwesome.Sharp.IconPictureBox();
            this.vidaPicBoxJA = new FontAwesome.Sharp.IconPictureBox();
            this.timerAcción = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.APicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JBPicBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JBPicBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JBPicBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JBPicBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JBPicBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JBPicBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.defensaPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ataquePicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JDPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JAPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JIPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vidaPicBoxJD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vidaPicBoxJB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vidaPicBoxJI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vidaPicBoxJA)).BeginInit();
            this.SuspendLayout();
            // 
            // ChatTextBox
            // 
            this.ChatTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChatTextBox.Location = new System.Drawing.Point(1484, 250);
            this.ChatTextBox.Name = "ChatTextBox";
            this.ChatTextBox.Size = new System.Drawing.Size(281, 35);
            this.ChatTextBox.TabIndex = 2;
            // 
            // ChatBtn
            // 
            this.ChatBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChatBtn.Location = new System.Drawing.Point(1771, 238);
            this.ChatBtn.Name = "ChatBtn";
            this.ChatBtn.Size = new System.Drawing.Size(141, 60);
            this.ChatBtn.TabIndex = 3;
            this.ChatBtn.Text = "Enviar";
            this.ChatBtn.UseVisualStyleBackColor = true;
            this.ChatBtn.Click += new System.EventHandler(this.ChatBtn_Click);
            // 
            // JugadorIzquierda
            // 
            this.JugadorIzquierda.AutoSize = true;
            this.JugadorIzquierda.BackColor = System.Drawing.Color.Transparent;
            this.JugadorIzquierda.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JugadorIzquierda.ForeColor = System.Drawing.Color.White;
            this.JugadorIzquierda.Location = new System.Drawing.Point(176, 327);
            this.JugadorIzquierda.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.JugadorIzquierda.Name = "JugadorIzquierda";
            this.JugadorIzquierda.Size = new System.Drawing.Size(186, 25);
            this.JugadorIzquierda.TabIndex = 19;
            this.JugadorIzquierda.Text = "JugadorIzquierda";
            // 
            // JugadorDerecha
            // 
            this.JugadorDerecha.AutoSize = true;
            this.JugadorDerecha.BackColor = System.Drawing.Color.Transparent;
            this.JugadorDerecha.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JugadorDerecha.ForeColor = System.Drawing.Color.White;
            this.JugadorDerecha.Location = new System.Drawing.Point(1361, 327);
            this.JugadorDerecha.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.JugadorDerecha.Name = "JugadorDerecha";
            this.JugadorDerecha.Size = new System.Drawing.Size(174, 25);
            this.JugadorDerecha.TabIndex = 23;
            this.JugadorDerecha.Text = "JugadorDerecha";
            // 
            // JugadorArriba
            // 
            this.JugadorArriba.AutoSize = true;
            this.JugadorArriba.BackColor = System.Drawing.Color.Transparent;
            this.JugadorArriba.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JugadorArriba.ForeColor = System.Drawing.Color.White;
            this.JugadorArriba.Location = new System.Drawing.Point(526, 107);
            this.JugadorArriba.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.JugadorArriba.Name = "JugadorArriba";
            this.JugadorArriba.Size = new System.Drawing.Size(153, 25);
            this.JugadorArriba.TabIndex = 27;
            this.JugadorArriba.Text = "JugadorArriba";
            // 
            // JugadorAbajo
            // 
            this.JugadorAbajo.AutoSize = true;
            this.JugadorAbajo.BackColor = System.Drawing.Color.Transparent;
            this.JugadorAbajo.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JugadorAbajo.ForeColor = System.Drawing.Color.White;
            this.JugadorAbajo.Location = new System.Drawing.Point(628, 724);
            this.JugadorAbajo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.JugadorAbajo.Name = "JugadorAbajo";
            this.JugadorAbajo.Size = new System.Drawing.Size(148, 25);
            this.JugadorAbajo.TabIndex = 31;
            this.JugadorAbajo.Text = "JugadorAbajo";
            // 
            // numFormlbl
            // 
            this.numFormlbl.AutoSize = true;
            this.numFormlbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numFormlbl.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.numFormlbl.Location = new System.Drawing.Point(12, 9);
            this.numFormlbl.Name = "numFormlbl";
            this.numFormlbl.Size = new System.Drawing.Size(107, 82);
            this.numFormlbl.TabIndex = 0;
            this.numFormlbl.Text = "ID";
            // 
            // vidaLblJA
            // 
            this.vidaLblJA.AutoSize = true;
            this.vidaLblJA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vidaLblJA.ForeColor = System.Drawing.Color.White;
            this.vidaLblJA.Location = new System.Drawing.Point(1352, 107);
            this.vidaLblJA.Name = "vidaLblJA";
            this.vidaLblJA.Size = new System.Drawing.Size(88, 29);
            this.vidaLblJA.TabIndex = 34;
            this.vidaLblJA.Text = "VidaJA";
            // 
            // vidaLblJI
            // 
            this.vidaLblJI.AutoSize = true;
            this.vidaLblJI.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vidaLblJI.ForeColor = System.Drawing.Color.White;
            this.vidaLblJI.Location = new System.Drawing.Point(464, 585);
            this.vidaLblJI.Name = "vidaLblJI";
            this.vidaLblJI.Size = new System.Drawing.Size(79, 29);
            this.vidaLblJI.TabIndex = 36;
            this.vidaLblJI.Text = "VidaJI";
            // 
            // vidaLblJB
            // 
            this.vidaLblJB.AutoSize = true;
            this.vidaLblJB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vidaLblJB.ForeColor = System.Drawing.Color.White;
            this.vidaLblJB.Location = new System.Drawing.Point(1242, 720);
            this.vidaLblJB.Name = "vidaLblJB";
            this.vidaLblJB.Size = new System.Drawing.Size(89, 29);
            this.vidaLblJB.TabIndex = 38;
            this.vidaLblJB.Text = "VidaJB";
            // 
            // vidaLblJD
            // 
            this.vidaLblJD.AutoSize = true;
            this.vidaLblJD.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vidaLblJD.ForeColor = System.Drawing.Color.White;
            this.vidaLblJD.Location = new System.Drawing.Point(1689, 588);
            this.vidaLblJD.Name = "vidaLblJD";
            this.vidaLblJD.Size = new System.Drawing.Size(90, 29);
            this.vidaLblJD.TabIndex = 40;
            this.vidaLblJD.Text = "VidaJD";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(701, 272);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 41);
            this.label1.TabIndex = 43;
            this.label1.Text = "Ataca";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBarJB
            // 
            this.progressBarJB.Location = new System.Drawing.Point(960, 724);
            this.progressBarJB.Name = "progressBarJB";
            this.progressBarJB.Size = new System.Drawing.Size(255, 32);
            this.progressBarJB.TabIndex = 51;
            // 
            // progressBarJI
            // 
            this.progressBarJI.Location = new System.Drawing.Point(181, 588);
            this.progressBarJI.Name = "progressBarJI";
            this.progressBarJI.Size = new System.Drawing.Size(255, 32);
            this.progressBarJI.TabIndex = 52;
            // 
            // progressBarJD
            // 
            this.progressBarJD.Location = new System.Drawing.Point(1407, 588);
            this.progressBarJD.Name = "progressBarJD";
            this.progressBarJD.Size = new System.Drawing.Size(255, 32);
            this.progressBarJD.TabIndex = 53;
            // 
            // progressBarJA
            // 
            this.progressBarJA.Location = new System.Drawing.Point(1076, 107);
            this.progressBarJA.Name = "progressBarJA";
            this.progressBarJA.Size = new System.Drawing.Size(255, 32);
            this.progressBarJA.TabIndex = 54;
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // ChatBox
            // 
            this.ChatBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChatBox.Location = new System.Drawing.Point(1484, 14);
            this.ChatBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ChatBox.Multiline = true;
            this.ChatBox.Name = "ChatBox";
            this.ChatBox.ReadOnly = true;
            this.ChatBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChatBox.Size = new System.Drawing.Size(428, 207);
            this.ChatBox.TabIndex = 66;
            // 
            // salirBtn
            // 
            this.salirBtn.Location = new System.Drawing.Point(1596, 959);
            this.salirBtn.Name = "salirBtn";
            this.salirBtn.Size = new System.Drawing.Size(239, 77);
            this.salirBtn.TabIndex = 67;
            this.salirBtn.Text = "Salir";
            this.salirBtn.UseVisualStyleBackColor = true;
            this.salirBtn.Click += new System.EventHandler(this.salirBtn_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(873, 272);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(230, 41);
            this.label2.TabIndex = 68;
            this.label2.Text = "Defiende";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // atacaLbl
            // 
            this.atacaLbl.BackColor = System.Drawing.Color.Transparent;
            this.atacaLbl.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.atacaLbl.ForeColor = System.Drawing.Color.White;
            this.atacaLbl.Location = new System.Drawing.Point(696, 321);
            this.atacaLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.atacaLbl.Name = "atacaLbl";
            this.atacaLbl.Size = new System.Drawing.Size(174, 44);
            this.atacaLbl.TabIndex = 69;
            this.atacaLbl.Text = "Ataca";
            this.atacaLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // defiendeLbl
            // 
            this.defiendeLbl.BackColor = System.Drawing.Color.Transparent;
            this.defiendeLbl.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defiendeLbl.ForeColor = System.Drawing.Color.White;
            this.defiendeLbl.Location = new System.Drawing.Point(878, 321);
            this.defiendeLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.defiendeLbl.Name = "defiendeLbl";
            this.defiendeLbl.Size = new System.Drawing.Size(225, 44);
            this.defiendeLbl.TabIndex = 70;
            this.defiendeLbl.Text = "Defiende";
            this.defiendeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // notificaciónLbl
            // 
            this.notificaciónLbl.BackColor = System.Drawing.Color.Transparent;
            this.notificaciónLbl.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificaciónLbl.ForeColor = System.Drawing.Color.White;
            this.notificaciónLbl.Location = new System.Drawing.Point(561, 613);
            this.notificaciónLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.notificaciónLbl.Name = "notificaciónLbl";
            this.notificaciónLbl.Size = new System.Drawing.Size(654, 89);
            this.notificaciónLbl.TabIndex = 71;
            this.notificaciónLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // defensaLbl
            // 
            this.defensaLbl.AutoSize = true;
            this.defensaLbl.Font = new System.Drawing.Font("Verdana", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defensaLbl.ForeColor = System.Drawing.Color.White;
            this.defensaLbl.Location = new System.Drawing.Point(89, 887);
            this.defensaLbl.Name = "defensaLbl";
            this.defensaLbl.Size = new System.Drawing.Size(163, 38);
            this.defensaLbl.TabIndex = 75;
            this.defensaLbl.Text = "Defensa";
            // 
            // ataqueLbl
            // 
            this.ataqueLbl.AutoSize = true;
            this.ataqueLbl.Font = new System.Drawing.Font("Verdana", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ataqueLbl.ForeColor = System.Drawing.Color.White;
            this.ataqueLbl.Location = new System.Drawing.Point(89, 811);
            this.ataqueLbl.Name = "ataqueLbl";
            this.ataqueLbl.Size = new System.Drawing.Size(144, 38);
            this.ataqueLbl.TabIndex = 74;
            this.ataqueLbl.Text = "Ataque";
            // 
            // lanzarBtn
            // 
            this.lanzarBtn.Location = new System.Drawing.Point(12, 974);
            this.lanzarBtn.Name = "lanzarBtn";
            this.lanzarBtn.Size = new System.Drawing.Size(256, 64);
            this.lanzarBtn.TabIndex = 76;
            this.lanzarBtn.Text = "Lanzar Carta";
            this.lanzarBtn.UseVisualStyleBackColor = true;
            this.lanzarBtn.Click += new System.EventHandler(this.lanzarBtn_Click);
            // 
            // nombreLbl
            // 
            this.nombreLbl.Font = new System.Drawing.Font("Verdana", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nombreLbl.ForeColor = System.Drawing.Color.White;
            this.nombreLbl.Location = new System.Drawing.Point(12, 683);
            this.nombreLbl.Name = "nombreLbl";
            this.nombreLbl.Size = new System.Drawing.Size(562, 110);
            this.nombreLbl.TabIndex = 77;
            this.nombreLbl.Text = "Nombre";
            this.nombreLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // notificaciónULbl
            // 
            this.notificaciónULbl.BackColor = System.Drawing.Color.Transparent;
            this.notificaciónULbl.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificaciónULbl.ForeColor = System.Drawing.Color.White;
            this.notificaciónULbl.Location = new System.Drawing.Point(1484, 737);
            this.notificaciónULbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.notificaciónULbl.Name = "notificaciónULbl";
            this.notificaciónULbl.Size = new System.Drawing.Size(428, 194);
            this.notificaciónULbl.TabIndex = 78;
            this.notificaciónULbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DPicBox
            // 
            this.DPicBox.BackColor = System.Drawing.Color.DarkBlue;
            this.DPicBox.ForeColor = System.Drawing.Color.RoyalBlue;
            this.DPicBox.IconChar = FontAwesome.Sharp.IconChar.ShieldAlt;
            this.DPicBox.IconColor = System.Drawing.Color.RoyalBlue;
            this.DPicBox.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.DPicBox.IconSize = 70;
            this.DPicBox.Location = new System.Drawing.Point(12, 887);
            this.DPicBox.Name = "DPicBox";
            this.DPicBox.Size = new System.Drawing.Size(70, 70);
            this.DPicBox.TabIndex = 73;
            this.DPicBox.TabStop = false;
            // 
            // APicBox
            // 
            this.APicBox.BackColor = System.Drawing.Color.DarkBlue;
            this.APicBox.ForeColor = System.Drawing.Color.DimGray;
            this.APicBox.IconChar = FontAwesome.Sharp.IconChar.FistRaised;
            this.APicBox.IconColor = System.Drawing.Color.DimGray;
            this.APicBox.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.APicBox.IconSize = 70;
            this.APicBox.Location = new System.Drawing.Point(12, 811);
            this.APicBox.Name = "APicBox";
            this.APicBox.Size = new System.Drawing.Size(70, 70);
            this.APicBox.TabIndex = 72;
            this.APicBox.TabStop = false;
            // 
            // JBPicBox6
            // 
            this.JBPicBox6.BackColor = System.Drawing.Color.DarkBlue;
            this.JBPicBox6.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("JBPicBox6.BackgroundImage")));
            this.JBPicBox6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.JBPicBox6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.JBPicBox6.IconChar = FontAwesome.Sharp.IconChar.None;
            this.JBPicBox6.IconColor = System.Drawing.SystemColors.ControlText;
            this.JBPicBox6.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.JBPicBox6.IconSize = 150;
            this.JBPicBox6.Location = new System.Drawing.Point(1315, 811);
            this.JBPicBox6.Name = "JBPicBox6";
            this.JBPicBox6.Size = new System.Drawing.Size(150, 225);
            this.JBPicBox6.TabIndex = 65;
            this.JBPicBox6.TabStop = false;
            this.JBPicBox6.Click += new System.EventHandler(this.JBPicBox6_Click);
            // 
            // JBPicBox5
            // 
            this.JBPicBox5.BackColor = System.Drawing.Color.DarkBlue;
            this.JBPicBox5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("JBPicBox5.BackgroundImage")));
            this.JBPicBox5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.JBPicBox5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.JBPicBox5.IconChar = FontAwesome.Sharp.IconChar.None;
            this.JBPicBox5.IconColor = System.Drawing.SystemColors.ControlText;
            this.JBPicBox5.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.JBPicBox5.IconSize = 150;
            this.JBPicBox5.Location = new System.Drawing.Point(1115, 811);
            this.JBPicBox5.Name = "JBPicBox5";
            this.JBPicBox5.Size = new System.Drawing.Size(150, 225);
            this.JBPicBox5.TabIndex = 64;
            this.JBPicBox5.TabStop = false;
            this.JBPicBox5.Click += new System.EventHandler(this.JBPicBox5_Click);
            // 
            // JBPicBox4
            // 
            this.JBPicBox4.BackColor = System.Drawing.Color.DarkBlue;
            this.JBPicBox4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("JBPicBox4.BackgroundImage")));
            this.JBPicBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.JBPicBox4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.JBPicBox4.IconChar = FontAwesome.Sharp.IconChar.None;
            this.JBPicBox4.IconColor = System.Drawing.SystemColors.ControlText;
            this.JBPicBox4.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.JBPicBox4.IconSize = 150;
            this.JBPicBox4.Location = new System.Drawing.Point(910, 811);
            this.JBPicBox4.Name = "JBPicBox4";
            this.JBPicBox4.Size = new System.Drawing.Size(150, 225);
            this.JBPicBox4.TabIndex = 63;
            this.JBPicBox4.TabStop = false;
            this.JBPicBox4.Click += new System.EventHandler(this.JBPicBox4_Click);
            // 
            // JBPicBox3
            // 
            this.JBPicBox3.BackColor = System.Drawing.Color.DarkBlue;
            this.JBPicBox3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("JBPicBox3.BackgroundImage")));
            this.JBPicBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.JBPicBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.JBPicBox3.IconChar = FontAwesome.Sharp.IconChar.None;
            this.JBPicBox3.IconColor = System.Drawing.SystemColors.ControlText;
            this.JBPicBox3.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.JBPicBox3.IconSize = 150;
            this.JBPicBox3.Location = new System.Drawing.Point(707, 811);
            this.JBPicBox3.Name = "JBPicBox3";
            this.JBPicBox3.Size = new System.Drawing.Size(150, 225);
            this.JBPicBox3.TabIndex = 62;
            this.JBPicBox3.TabStop = false;
            this.JBPicBox3.Click += new System.EventHandler(this.JBPicBox3_Click);
            // 
            // JBPicBox2
            // 
            this.JBPicBox2.BackColor = System.Drawing.Color.DarkBlue;
            this.JBPicBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("JBPicBox2.BackgroundImage")));
            this.JBPicBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.JBPicBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.JBPicBox2.IconChar = FontAwesome.Sharp.IconChar.None;
            this.JBPicBox2.IconColor = System.Drawing.SystemColors.ControlText;
            this.JBPicBox2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.JBPicBox2.IconSize = 150;
            this.JBPicBox2.Location = new System.Drawing.Point(506, 811);
            this.JBPicBox2.Name = "JBPicBox2";
            this.JBPicBox2.Size = new System.Drawing.Size(150, 225);
            this.JBPicBox2.TabIndex = 61;
            this.JBPicBox2.TabStop = false;
            this.JBPicBox2.Click += new System.EventHandler(this.JBPicBox2_Click);
            // 
            // JBPicBox1
            // 
            this.JBPicBox1.BackColor = System.Drawing.Color.DarkBlue;
            this.JBPicBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("JBPicBox1.BackgroundImage")));
            this.JBPicBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.JBPicBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.JBPicBox1.IconChar = FontAwesome.Sharp.IconChar.None;
            this.JBPicBox1.IconColor = System.Drawing.SystemColors.ControlText;
            this.JBPicBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.JBPicBox1.IconSize = 150;
            this.JBPicBox1.Location = new System.Drawing.Point(301, 811);
            this.JBPicBox1.Name = "JBPicBox1";
            this.JBPicBox1.Size = new System.Drawing.Size(150, 225);
            this.JBPicBox1.TabIndex = 60;
            this.JBPicBox1.TabStop = false;
            this.JBPicBox1.Click += new System.EventHandler(this.JBPicBox1_Click);
            // 
            // defensaPicBox
            // 
            this.defensaPicBox.BackColor = System.Drawing.SystemColors.HighlightText;
            this.defensaPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.defensaPicBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.defensaPicBox.IconChar = FontAwesome.Sharp.IconChar.None;
            this.defensaPicBox.IconColor = System.Drawing.SystemColors.ControlText;
            this.defensaPicBox.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.defensaPicBox.IconSize = 150;
            this.defensaPicBox.Location = new System.Drawing.Point(910, 368);
            this.defensaPicBox.Name = "defensaPicBox";
            this.defensaPicBox.Size = new System.Drawing.Size(150, 225);
            this.defensaPicBox.TabIndex = 59;
            this.defensaPicBox.TabStop = false;
            // 
            // ataquePicBox
            // 
            this.ataquePicBox.BackColor = System.Drawing.SystemColors.HighlightText;
            this.ataquePicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ataquePicBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ataquePicBox.IconChar = FontAwesome.Sharp.IconChar.None;
            this.ataquePicBox.IconColor = System.Drawing.SystemColors.ControlText;
            this.ataquePicBox.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.ataquePicBox.IconSize = 150;
            this.ataquePicBox.Location = new System.Drawing.Point(707, 368);
            this.ataquePicBox.Name = "ataquePicBox";
            this.ataquePicBox.Size = new System.Drawing.Size(150, 225);
            this.ataquePicBox.TabIndex = 58;
            this.ataquePicBox.TabStop = false;
            // 
            // JDPicBox
            // 
            this.JDPicBox.BackColor = System.Drawing.Color.DarkBlue;
            this.JDPicBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("JDPicBox.BackgroundImage")));
            this.JDPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.JDPicBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.JDPicBox.IconChar = FontAwesome.Sharp.IconChar.None;
            this.JDPicBox.IconColor = System.Drawing.SystemColors.ControlText;
            this.JDPicBox.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.JDPicBox.IconSize = 150;
            this.JDPicBox.Location = new System.Drawing.Point(1366, 399);
            this.JDPicBox.Name = "JDPicBox";
            this.JDPicBox.Size = new System.Drawing.Size(225, 150);
            this.JDPicBox.TabIndex = 57;
            this.JDPicBox.TabStop = false;
            // 
            // JAPicBox
            // 
            this.JAPicBox.BackColor = System.Drawing.Color.DarkBlue;
            this.JAPicBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("JAPicBox.BackgroundImage")));
            this.JAPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.JAPicBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.JAPicBox.IconChar = FontAwesome.Sharp.IconChar.None;
            this.JAPicBox.IconColor = System.Drawing.SystemColors.ControlText;
            this.JAPicBox.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.JAPicBox.IconSize = 150;
            this.JAPicBox.Location = new System.Drawing.Point(806, 24);
            this.JAPicBox.Name = "JAPicBox";
            this.JAPicBox.Size = new System.Drawing.Size(150, 225);
            this.JAPicBox.TabIndex = 56;
            this.JAPicBox.TabStop = false;
            // 
            // JIPicBox
            // 
            this.JIPicBox.BackColor = System.Drawing.Color.DarkBlue;
            this.JIPicBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("JIPicBox.BackgroundImage")));
            this.JIPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.JIPicBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.JIPicBox.IconChar = FontAwesome.Sharp.IconChar.None;
            this.JIPicBox.IconColor = System.Drawing.SystemColors.ControlText;
            this.JIPicBox.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.JIPicBox.IconSize = 150;
            this.JIPicBox.Location = new System.Drawing.Point(181, 399);
            this.JIPicBox.Name = "JIPicBox";
            this.JIPicBox.Size = new System.Drawing.Size(225, 150);
            this.JIPicBox.TabIndex = 55;
            this.JIPicBox.TabStop = false;
            // 
            // vidaPicBoxJD
            // 
            this.vidaPicBoxJD.BackColor = System.Drawing.Color.DarkBlue;
            this.vidaPicBoxJD.ForeColor = System.Drawing.Color.Red;
            this.vidaPicBoxJD.IconChar = FontAwesome.Sharp.IconChar.Heart;
            this.vidaPicBoxJD.IconColor = System.Drawing.Color.Red;
            this.vidaPicBoxJD.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.vidaPicBoxJD.IconSize = 69;
            this.vidaPicBoxJD.Location = new System.Drawing.Point(1315, 585);
            this.vidaPicBoxJD.Name = "vidaPicBoxJD";
            this.vidaPicBoxJD.Size = new System.Drawing.Size(72, 69);
            this.vidaPicBoxJD.TabIndex = 39;
            this.vidaPicBoxJD.TabStop = false;
            // 
            // vidaPicBoxJB
            // 
            this.vidaPicBoxJB.BackColor = System.Drawing.Color.DarkBlue;
            this.vidaPicBoxJB.ForeColor = System.Drawing.Color.Red;
            this.vidaPicBoxJB.IconChar = FontAwesome.Sharp.IconChar.Heart;
            this.vidaPicBoxJB.IconColor = System.Drawing.Color.Red;
            this.vidaPicBoxJB.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.vidaPicBoxJB.IconSize = 69;
            this.vidaPicBoxJB.Location = new System.Drawing.Point(856, 724);
            this.vidaPicBoxJB.Name = "vidaPicBoxJB";
            this.vidaPicBoxJB.Size = new System.Drawing.Size(72, 69);
            this.vidaPicBoxJB.TabIndex = 37;
            this.vidaPicBoxJB.TabStop = false;
            // 
            // vidaPicBoxJI
            // 
            this.vidaPicBoxJI.BackColor = System.Drawing.Color.DarkBlue;
            this.vidaPicBoxJI.ForeColor = System.Drawing.Color.Red;
            this.vidaPicBoxJI.IconChar = FontAwesome.Sharp.IconChar.Heart;
            this.vidaPicBoxJI.IconColor = System.Drawing.Color.Red;
            this.vidaPicBoxJI.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.vidaPicBoxJI.IconSize = 69;
            this.vidaPicBoxJI.Location = new System.Drawing.Point(77, 585);
            this.vidaPicBoxJI.Name = "vidaPicBoxJI";
            this.vidaPicBoxJI.Size = new System.Drawing.Size(72, 69);
            this.vidaPicBoxJI.TabIndex = 35;
            this.vidaPicBoxJI.TabStop = false;
            // 
            // vidaPicBoxJA
            // 
            this.vidaPicBoxJA.BackColor = System.Drawing.Color.DarkBlue;
            this.vidaPicBoxJA.ForeColor = System.Drawing.Color.Red;
            this.vidaPicBoxJA.IconChar = FontAwesome.Sharp.IconChar.Heart;
            this.vidaPicBoxJA.IconColor = System.Drawing.Color.Red;
            this.vidaPicBoxJA.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.vidaPicBoxJA.IconSize = 69;
            this.vidaPicBoxJA.Location = new System.Drawing.Point(988, 97);
            this.vidaPicBoxJA.Name = "vidaPicBoxJA";
            this.vidaPicBoxJA.Size = new System.Drawing.Size(72, 69);
            this.vidaPicBoxJA.TabIndex = 33;
            this.vidaPicBoxJA.TabStop = false;
            // 
            // timerAcción
            // 
            this.timerAcción.Interval = 3000;
            this.timerAcción.Tick += new System.EventHandler(this.timerAcción_Tick);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkBlue;
            this.ClientSize = new System.Drawing.Size(1924, 1050);
            this.Controls.Add(this.notificaciónULbl);
            this.Controls.Add(this.nombreLbl);
            this.Controls.Add(this.lanzarBtn);
            this.Controls.Add(this.defensaLbl);
            this.Controls.Add(this.ataqueLbl);
            this.Controls.Add(this.DPicBox);
            this.Controls.Add(this.APicBox);
            this.Controls.Add(this.notificaciónLbl);
            this.Controls.Add(this.defiendeLbl);
            this.Controls.Add(this.atacaLbl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.salirBtn);
            this.Controls.Add(this.ChatBox);
            this.Controls.Add(this.JBPicBox6);
            this.Controls.Add(this.JBPicBox5);
            this.Controls.Add(this.JBPicBox4);
            this.Controls.Add(this.JBPicBox3);
            this.Controls.Add(this.JBPicBox2);
            this.Controls.Add(this.JBPicBox1);
            this.Controls.Add(this.defensaPicBox);
            this.Controls.Add(this.ataquePicBox);
            this.Controls.Add(this.JDPicBox);
            this.Controls.Add(this.JAPicBox);
            this.Controls.Add(this.JIPicBox);
            this.Controls.Add(this.progressBarJA);
            this.Controls.Add(this.progressBarJD);
            this.Controls.Add(this.progressBarJI);
            this.Controls.Add(this.progressBarJB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.vidaLblJD);
            this.Controls.Add(this.vidaPicBoxJD);
            this.Controls.Add(this.vidaLblJB);
            this.Controls.Add(this.vidaPicBoxJB);
            this.Controls.Add(this.vidaLblJI);
            this.Controls.Add(this.vidaPicBoxJI);
            this.Controls.Add(this.vidaLblJA);
            this.Controls.Add(this.vidaPicBoxJA);
            this.Controls.Add(this.JugadorAbajo);
            this.Controls.Add(this.JugadorArriba);
            this.Controls.Add(this.JugadorDerecha);
            this.Controls.Add(this.JugadorIzquierda);
            this.Controls.Add(this.ChatBtn);
            this.Controls.Add(this.ChatTextBox);
            this.Controls.Add(this.numFormlbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.APicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JBPicBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JBPicBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JBPicBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JBPicBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JBPicBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JBPicBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.defensaPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ataquePicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JDPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JAPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JIPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vidaPicBoxJD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vidaPicBoxJB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vidaPicBoxJI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vidaPicBoxJA)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox ChatTextBox;
        private System.Windows.Forms.Button ChatBtn;
        private System.Windows.Forms.Label JugadorIzquierda;
        private System.Windows.Forms.Label JugadorDerecha;
        private System.Windows.Forms.Label JugadorArriba;
        private System.Windows.Forms.Label JugadorAbajo;
        private System.Windows.Forms.Label numFormlbl;
        private FontAwesome.Sharp.IconPictureBox vidaPicBoxJA;
        private System.Windows.Forms.Label vidaLblJA;
        private System.Windows.Forms.Label vidaLblJI;
        private FontAwesome.Sharp.IconPictureBox vidaPicBoxJI;
        private System.Windows.Forms.Label vidaLblJB;
        private FontAwesome.Sharp.IconPictureBox vidaPicBoxJB;
        private System.Windows.Forms.Label vidaLblJD;
        private FontAwesome.Sharp.IconPictureBox vidaPicBoxJD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBarJB;
        private System.Windows.Forms.ProgressBar progressBarJI;
        private System.Windows.Forms.ProgressBar progressBarJD;
        private System.Windows.Forms.ProgressBar progressBarJA;
        private FontAwesome.Sharp.IconPictureBox JIPicBox;
        private FontAwesome.Sharp.IconPictureBox JAPicBox;
        private FontAwesome.Sharp.IconPictureBox JDPicBox;
        private FontAwesome.Sharp.IconPictureBox ataquePicBox;
        private FontAwesome.Sharp.IconPictureBox defensaPicBox;
        private FontAwesome.Sharp.IconPictureBox JBPicBox1;
        private FontAwesome.Sharp.IconPictureBox JBPicBox2;
        private FontAwesome.Sharp.IconPictureBox JBPicBox3;
        private FontAwesome.Sharp.IconPictureBox JBPicBox4;
        private FontAwesome.Sharp.IconPictureBox JBPicBox5;
        private FontAwesome.Sharp.IconPictureBox JBPicBox6;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.TextBox ChatBox;
        private System.Windows.Forms.Button salirBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label atacaLbl;
        private System.Windows.Forms.Label defiendeLbl;
        private System.Windows.Forms.Label notificaciónLbl;
        private System.Windows.Forms.Label defensaLbl;
        private System.Windows.Forms.Label ataqueLbl;
        private FontAwesome.Sharp.IconPictureBox DPicBox;
        private FontAwesome.Sharp.IconPictureBox APicBox;
        private System.Windows.Forms.Button lanzarBtn;
        private System.Windows.Forms.Label nombreLbl;
        private System.Windows.Forms.Label notificaciónULbl;
        private System.Windows.Forms.Timer timerAcción;
    }
}