namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.usuario = new System.Windows.Forms.TextBox();
            this.EnviarButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.jugadorG = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.jugadorP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.contrincante2 = new System.Windows.Forms.TextBox();
            this.contrincante1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Ganadores = new System.Windows.Forms.RadioButton();
            this.Puntos = new System.Windows.Forms.RadioButton();
            this.password = new System.Windows.Forms.TextBox();
            this.Enfrentamiento = new System.Windows.Forms.RadioButton();
            this.Register = new System.Windows.Forms.RadioButton();
            this.Login = new System.Windows.Forms.RadioButton();
            this.ConectarButton = new System.Windows.Forms.Button();
            this.DesconectarButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 43);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 37);
            this.label2.TabIndex = 1;
            this.label2.Text = "Usuario";
            // 
            // usuario
            // 
            this.usuario.Location = new System.Drawing.Point(282, 53);
            this.usuario.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.usuario.Name = "usuario";
            this.usuario.Size = new System.Drawing.Size(244, 26);
            this.usuario.TabIndex = 3;
            // 
            // EnviarButton
            // 
            this.EnviarButton.Location = new System.Drawing.Point(495, 372);
            this.EnviarButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.EnviarButton.Name = "EnviarButton";
            this.EnviarButton.Size = new System.Drawing.Size(125, 52);
            this.EnviarButton.TabIndex = 5;
            this.EnviarButton.Text = "Enviar";
            this.EnviarButton.UseVisualStyleBackColor = true;
            this.EnviarButton.Click += new System.EventHandler(this.EnviarButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.groupBox1.Controls.Add(this.jugadorG);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.jugadorP);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.contrincante2);
            this.groupBox1.Controls.Add(this.contrincante1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Ganadores);
            this.groupBox1.Controls.Add(this.Puntos);
            this.groupBox1.Controls.Add(this.password);
            this.groupBox1.Controls.Add(this.Enfrentamiento);
            this.groupBox1.Controls.Add(this.Register);
            this.groupBox1.Controls.Add(this.Login);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.EnviarButton);
            this.groupBox1.Controls.Add(this.usuario);
            this.groupBox1.Location = new System.Drawing.Point(13, 107);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(991, 434);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Peticion";
            // 
            // jugadorG
            // 
            this.jugadorG.Location = new System.Drawing.Point(282, 315);
            this.jugadorG.Name = "jugadorG";
            this.jugadorG.Size = new System.Drawing.Size(244, 26);
            this.jugadorG.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(23, 305);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 37);
            this.label5.TabIndex = 20;
            this.label5.Text = "Nombre";
            // 
            // jugadorP
            // 
            this.jugadorP.Location = new System.Drawing.Point(282, 247);
            this.jugadorP.Name = "jugadorP";
            this.jugadorP.Size = new System.Drawing.Size(244, 26);
            this.jugadorP.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(23, 237);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 37);
            this.label4.TabIndex = 18;
            this.label4.Text = "Nombre";
            // 
            // contrincante2
            // 
            this.contrincante2.Location = new System.Drawing.Point(414, 180);
            this.contrincante2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.contrincante2.Name = "contrincante2";
            this.contrincante2.Size = new System.Drawing.Size(136, 26);
            this.contrincante2.TabIndex = 17;
            // 
            // contrincante1
            // 
            this.contrincante1.Location = new System.Drawing.Point(246, 180);
            this.contrincante1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.contrincante1.Name = "contrincante1";
            this.contrincante1.Size = new System.Drawing.Size(136, 26);
            this.contrincante1.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 170);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(215, 37);
            this.label3.TabIndex = 15;
            this.label3.Text = "Contrincantes";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 91);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 37);
            this.label1.TabIndex = 14;
            this.label1.Text = "Contraseña";
            // 
            // Ganadores
            // 
            this.Ganadores.AutoSize = true;
            this.Ganadores.Location = new System.Drawing.Point(597, 316);
            this.Ganadores.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Ganadores.Name = "Ganadores";
            this.Ganadores.Size = new System.Drawing.Size(295, 24);
            this.Ganadores.TabIndex = 13;
            this.Ganadores.TabStop = true;
            this.Ganadores.Text = "Personas que ganaron a ese jugador";
            this.Ganadores.UseVisualStyleBackColor = true;
            // 
            // Puntos
            // 
            this.Puntos.AutoSize = true;
            this.Puntos.Location = new System.Drawing.Point(597, 248);
            this.Puntos.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Puntos.Name = "Puntos";
            this.Puntos.Size = new System.Drawing.Size(272, 24);
            this.Puntos.TabIndex = 12;
            this.Puntos.TabStop = true;
            this.Puntos.Text = "Puntos obtenidos por ese jugador";
            this.Puntos.UseVisualStyleBackColor = true;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(282, 101);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(244, 26);
            this.password.TabIndex = 10;
            // 
            // Enfrentamiento
            // 
            this.Enfrentamiento.AutoSize = true;
            this.Enfrentamiento.Location = new System.Drawing.Point(597, 181);
            this.Enfrentamiento.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Enfrentamiento.Name = "Enfrentamiento";
            this.Enfrentamiento.Size = new System.Drawing.Size(368, 24);
            this.Enfrentamiento.TabIndex = 9;
            this.Enfrentamiento.TabStop = true;
            this.Enfrentamiento.Text = "Veces que los contrincantes se han enfrentado";
            this.Enfrentamiento.UseVisualStyleBackColor = true;
            // 
            // Register
            // 
            this.Register.AutoSize = true;
            this.Register.Location = new System.Drawing.Point(714, 76);
            this.Register.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Register.Name = "Register";
            this.Register.Size = new System.Drawing.Size(94, 24);
            this.Register.TabIndex = 7;
            this.Register.TabStop = true;
            this.Register.Text = "Register";
            this.Register.UseVisualStyleBackColor = true;
            // 
            // Login
            // 
            this.Login.AutoSize = true;
            this.Login.Location = new System.Drawing.Point(582, 76);
            this.Login.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(78, 24);
            this.Login.TabIndex = 8;
            this.Login.TabStop = true;
            this.Login.Text = "Log-in";
            this.Login.UseVisualStyleBackColor = true;
            // 
            // ConectarButton
            // 
            this.ConectarButton.Location = new System.Drawing.Point(43, 12);
            this.ConectarButton.Name = "ConectarButton";
            this.ConectarButton.Size = new System.Drawing.Size(200, 75);
            this.ConectarButton.TabIndex = 7;
            this.ConectarButton.Text = "Conectar";
            this.ConectarButton.UseVisualStyleBackColor = true;
            this.ConectarButton.Click += new System.EventHandler(this.ConectarButton_Click);
            // 
            // DesconectarButton
            // 
            this.DesconectarButton.Location = new System.Drawing.Point(43, 565);
            this.DesconectarButton.Name = "DesconectarButton";
            this.DesconectarButton.Size = new System.Drawing.Size(248, 100);
            this.DesconectarButton.TabIndex = 8;
            this.DesconectarButton.Text = "Desconectar";
            this.DesconectarButton.UseVisualStyleBackColor = true;
            this.DesconectarButton.Click += new System.EventHandler(this.DesconectarButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1112, 865);
            this.Controls.Add(this.DesconectarButton);
            this.Controls.Add(this.ConectarButton);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox usuario;
        private System.Windows.Forms.Button EnviarButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton Register;
        private System.Windows.Forms.RadioButton Login;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.RadioButton Enfrentamiento;
        private System.Windows.Forms.Button ConectarButton;
        private System.Windows.Forms.Button DesconectarButton;
        private System.Windows.Forms.RadioButton Puntos;
        private System.Windows.Forms.RadioButton Ganadores;
        private System.Windows.Forms.TextBox jugadorG;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox jugadorP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox contrincante2;
        private System.Windows.Forms.TextBox contrincante1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
    }
}

