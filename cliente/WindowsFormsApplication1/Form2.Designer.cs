
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
            this.numFormlbl = new System.Windows.Forms.Label();
            this.ChatBox = new System.Windows.Forms.ListBox();
            this.ChatTextBox = new System.Windows.Forms.TextBox();
            this.ChatBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // numFormlbl
            // 
            this.numFormlbl.AutoSize = true;
            this.numFormlbl.Location = new System.Drawing.Point(63, 57);
            this.numFormlbl.Name = "numFormlbl";
            this.numFormlbl.Size = new System.Drawing.Size(0, 20);
            this.numFormlbl.TabIndex = 0;
            // 
            // ChatBox
            // 
            this.ChatBox.FormattingEnabled = true;
            this.ChatBox.ItemHeight = 20;
            this.ChatBox.Location = new System.Drawing.Point(184, 57);
            this.ChatBox.Name = "ChatBox";
            this.ChatBox.Size = new System.Drawing.Size(378, 224);
            this.ChatBox.TabIndex = 1;
            // 
            // ChatTextBox
            // 
            this.ChatTextBox.Location = new System.Drawing.Point(184, 304);
            this.ChatTextBox.Name = "ChatTextBox";
            this.ChatTextBox.Size = new System.Drawing.Size(253, 26);
            this.ChatTextBox.TabIndex = 2;
            // 
            // ChatBtn
            // 
            this.ChatBtn.Location = new System.Drawing.Point(455, 296);
            this.ChatBtn.Name = "ChatBtn";
            this.ChatBtn.Size = new System.Drawing.Size(107, 42);
            this.ChatBtn.TabIndex = 3;
            this.ChatBtn.Text = "Enviar";
            this.ChatBtn.UseVisualStyleBackColor = true;
            this.ChatBtn.Click += new System.EventHandler(this.ChatBtn_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ChatBtn);
            this.Controls.Add(this.ChatTextBox);
            this.Controls.Add(this.ChatBox);
            this.Controls.Add(this.numFormlbl);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label numFormlbl;
        private System.Windows.Forms.ListBox ChatBox;
        private System.Windows.Forms.TextBox ChatTextBox;
        private System.Windows.Forms.Button ChatBtn;
    }
}