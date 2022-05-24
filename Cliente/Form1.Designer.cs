namespace Cliente
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
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
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.btnConectar = new System.Windows.Forms.Button();
            this.btnDesconectar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.EnviarBtn = new System.Windows.Forms.Button();
            this.peticion = new System.Windows.Forms.GroupBox();
            this.dia = new System.Windows.Forms.TextBox();
            this.nombre = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.BtnRegistrar = new System.Windows.Forms.Button();
            this.BtnLogin = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MostrarPass = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numConsultas = new System.Windows.Forms.Button();
            this.contConsultas = new System.Windows.Forms.Label();
            this.BtnInvitar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chat = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.peticion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.chat.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(58, 278);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 52);
            this.label1.TabIndex = 0;
            this.label1.Text = "USUARIO";
            this.label1.Visible = false;
            // 
            // username
            // 
            this.username.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username.Location = new System.Drawing.Point(54, 333);
            this.username.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(414, 56);
            this.username.TabIndex = 1;
            this.username.Visible = false;
            // 
            // btnConectar
            // 
            this.btnConectar.BackColor = System.Drawing.Color.Lime;
            this.btnConectar.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConectar.Location = new System.Drawing.Point(18, 630);
            this.btnConectar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(328, 86);
            this.btnConectar.TabIndex = 2;
            this.btnConectar.Text = "CONECTAR";
            this.btnConectar.UseVisualStyleBackColor = false;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // btnDesconectar
            // 
            this.btnDesconectar.BackColor = System.Drawing.Color.DarkOrange;
            this.btnDesconectar.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDesconectar.Location = new System.Drawing.Point(0, 733);
            this.btnDesconectar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDesconectar.Name = "btnDesconectar";
            this.btnDesconectar.Size = new System.Drawing.Size(374, 89);
            this.btnDesconectar.TabIndex = 3;
            this.btnDesconectar.Text = "DESCONECTAR";
            this.btnDesconectar.UseVisualStyleBackColor = false;
            this.btnDesconectar.Visible = false;
            this.btnDesconectar.Click += new System.EventHandler(this.btnDesconectar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(58, 447);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(338, 52);
            this.label2.TabIndex = 4;
            this.label2.Text = "CONTRASEÑA";
            this.label2.Visible = false;
            // 
            // password
            // 
            this.password.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password.Location = new System.Drawing.Point(54, 506);
            this.password.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(414, 56);
            this.password.TabIndex = 5;
            this.password.Visible = false;
            // 
            // EnviarBtn
            // 
            this.EnviarBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnviarBtn.Location = new System.Drawing.Point(428, 477);
            this.EnviarBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.EnviarBtn.Name = "EnviarBtn";
            this.EnviarBtn.Size = new System.Drawing.Size(208, 62);
            this.EnviarBtn.TabIndex = 8;
            this.EnviarBtn.Text = "ENVIAR";
            this.EnviarBtn.UseVisualStyleBackColor = true;
            this.EnviarBtn.Click += new System.EventHandler(this.EnviarBtn_Click);
            // 
            // peticion
            // 
            this.peticion.BackColor = System.Drawing.Color.Gray;
            this.peticion.Controls.Add(this.EnviarBtn);
            this.peticion.Controls.Add(this.dia);
            this.peticion.Controls.Add(this.nombre);
            this.peticion.Controls.Add(this.label3);
            this.peticion.Controls.Add(this.label4);
            this.peticion.Controls.Add(this.radioButton4);
            this.peticion.Controls.Add(this.radioButton3);
            this.peticion.Controls.Add(this.radioButton2);
            this.peticion.Controls.Add(this.radioButton1);
            this.peticion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.peticion.Location = new System.Drawing.Point(1234, 14);
            this.peticion.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.peticion.Name = "peticion";
            this.peticion.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.peticion.Size = new System.Drawing.Size(706, 564);
            this.peticion.TabIndex = 7;
            this.peticion.TabStop = false;
            this.peticion.Text = "Peticion de consultas";
            this.peticion.Visible = false;
            // 
            // dia
            // 
            this.dia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dia.Location = new System.Drawing.Point(342, 147);
            this.dia.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dia.Name = "dia";
            this.dia.Size = new System.Drawing.Size(337, 44);
            this.dia.TabIndex = 7;
            // 
            // nombre
            // 
            this.nombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nombre.Location = new System.Drawing.Point(278, 69);
            this.nombre.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nombre.Name = "nombre";
            this.nombre.Size = new System.Drawing.Size(402, 44);
            this.nombre.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(100, 70);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 37);
            this.label3.TabIndex = 5;
            this.label3.Text = "NOMBRE";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(24, 152);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(299, 37);
            this.label4.TabIndex = 4;
            this.label4.Text = "DIA (dd-mm-aaaa)";
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(34, 436);
            this.radioButton4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(236, 36);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "ID del jugador";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(34, 377);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(640, 36);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Cantidad de partidas jugadas de tal jugador";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(34, 316);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(649, 36);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Cantidad de partidas ganadas de tal jugador";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(34, 252);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(674, 36);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Nombre del ganador y duración partida del dia";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // BtnRegistrar
            // 
            this.BtnRegistrar.BackColor = System.Drawing.Color.Aquamarine;
            this.BtnRegistrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnRegistrar.Location = new System.Drawing.Point(472, 802);
            this.BtnRegistrar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BtnRegistrar.Name = "BtnRegistrar";
            this.BtnRegistrar.Size = new System.Drawing.Size(330, 86);
            this.BtnRegistrar.TabIndex = 8;
            this.BtnRegistrar.Text = "REGISTRARSE";
            this.BtnRegistrar.UseVisualStyleBackColor = false;
            this.BtnRegistrar.Visible = false;
            this.BtnRegistrar.Click += new System.EventHandler(this.BtnRegistrar_Click);
            // 
            // BtnLogin
            // 
            this.BtnLogin.BackColor = System.Drawing.Color.Aquamarine;
            this.BtnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLogin.Location = new System.Drawing.Point(472, 645);
            this.BtnLogin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.Size = new System.Drawing.Size(330, 86);
            this.BtnLogin.TabIndex = 9;
            this.BtnLogin.Text = "LOGIN";
            this.BtnLogin.UseVisualStyleBackColor = false;
            this.BtnLogin.Visible = false;
            this.BtnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dataGridView1.Location = new System.Drawing.Point(909, 19);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(268, 453);
            this.dataGridView1.TabIndex = 11;
            this.dataGridView1.Visible = false;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Usuarios Conectados";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.Width = 125;
            // 
            // MostrarPass
            // 
            this.MostrarPass.AutoSize = true;
            this.MostrarPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MostrarPass.Location = new System.Drawing.Point(494, 533);
            this.MostrarPass.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MostrarPass.Name = "MostrarPass";
            this.MostrarPass.Size = new System.Drawing.Size(122, 33);
            this.MostrarPass.TabIndex = 12;
            this.MostrarPass.Text = "Ocultar";
            this.MostrarPass.UseVisualStyleBackColor = true;
            this.MostrarPass.Visible = false;
            this.MostrarPass.CheckedChanged += new System.EventHandler(this.MostrarPass_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(466, 609);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(380, 32);
            this.label5.TabIndex = 13;
            this.label5.Text = "Escribe usuario y contraseña";
            this.label5.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(454, 770);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(440, 32);
            this.label6.TabIndex = 14;
            this.label6.Text = "Crea un usuario y una contraseña";
            this.label6.Visible = false;
            // 
            // numConsultas
            // 
            this.numConsultas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numConsultas.Location = new System.Drawing.Point(104, 66);
            this.numConsultas.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numConsultas.Name = "numConsultas";
            this.numConsultas.Size = new System.Drawing.Size(386, 127);
            this.numConsultas.TabIndex = 15;
            this.numConsultas.Text = "Ver número de servicios realizados";
            this.numConsultas.UseVisualStyleBackColor = true;
            this.numConsultas.Visible = false;
            this.numConsultas.Click += new System.EventHandler(this.numConsultas_Click);
            // 
            // contConsultas
            // 
            this.contConsultas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contConsultas.Location = new System.Drawing.Point(498, 66);
            this.contConsultas.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.contConsultas.Name = "contConsultas";
            this.contConsultas.Size = new System.Drawing.Size(150, 105);
            this.contConsultas.TabIndex = 16;
            this.contConsultas.Visible = false;
            // 
            // BtnInvitar
            // 
            this.BtnInvitar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnInvitar.Location = new System.Drawing.Point(860, 478);
            this.BtnInvitar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BtnInvitar.Name = "BtnInvitar";
            this.BtnInvitar.Size = new System.Drawing.Size(366, 89);
            this.BtnInvitar.TabIndex = 17;
            this.BtnInvitar.Text = "Invitar Jugador";
            this.BtnInvitar.UseVisualStyleBackColor = true;
            this.BtnInvitar.Click += new System.EventHandler(this.BtnInvitar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(-43, -88);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // chat
            // 
            this.chat.Controls.Add(this.button1);
            this.chat.Controls.Add(this.textBox1);
            this.chat.Controls.Add(this.listBox1);
            this.chat.Location = new System.Drawing.Point(1163, 630);
            this.chat.Name = "chat";
            this.chat.Size = new System.Drawing.Size(616, 370);
            this.chat.TabIndex = 19;
            this.chat.TabStop = false;
            this.chat.Text = "Chat";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 25;
            this.listBox1.Location = new System.Drawing.Point(28, 44);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(558, 254);
            this.listBox1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(28, 311);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(416, 31);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(471, 304);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 49);
            this.button1.TabIndex = 2;
            this.button1.Text = "Enviar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1992, 1333);
            this.Controls.Add(this.chat);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnInvitar);
            this.Controls.Add(this.contConsultas);
            this.Controls.Add(this.numConsultas);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.MostrarPass);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.BtnLogin);
            this.Controls.Add(this.BtnRegistrar);
            this.Controls.Add(this.peticion);
            this.Controls.Add(this.password);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDesconectar);
            this.Controls.Add(this.btnConectar);
            this.Controls.Add(this.username);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inicio";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.peticion.ResumeLayout(false);
            this.peticion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.chat.ResumeLayout(false);
            this.chat.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.Button btnDesconectar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Button EnviarBtn;
        private System.Windows.Forms.GroupBox peticion;
        private System.Windows.Forms.TextBox dia;
        private System.Windows.Forms.TextBox nombre;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button BtnRegistrar;
        private System.Windows.Forms.Button BtnLogin;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.CheckBox MostrarPass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button numConsultas;
        private System.Windows.Forms.Label contConsultas;
        private System.Windows.Forms.Button BtnInvitar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox chat;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox listBox1;
    }
}

