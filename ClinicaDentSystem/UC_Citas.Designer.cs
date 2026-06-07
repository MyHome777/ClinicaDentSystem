namespace ClinicaDentSystem
{
    partial class UC_Citas
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

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_Citas));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label1 = new Label();
            dgv_Citas = new DataGridView();
            CitaID = new DataGridViewTextBoxColumn();
            Paciente = new DataGridViewTextBoxColumn();
            Dentista = new DataGridViewTextBoxColumn();
            FechaHora = new DataGridViewTextBoxColumn();
            Motivo = new DataGridViewTextBoxColumn();
            EstadoCita = new DataGridViewTextBoxColumn();
            NotasCita = new DataGridViewTextBoxColumn();
            Creada = new DataGridViewTextBoxColumn();
            guna2ImageButton6 = new Guna.UI2.WinForms.Guna2ImageButton();
            txt_Buscador = new Guna.UI2.WinForms.Guna2TextBox();
            guna2ImageButton5 = new Guna.UI2.WinForms.Guna2ImageButton();
            guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            guna2ImageButton2 = new Guna.UI2.WinForms.Guna2ImageButton();
            guna2ImageButton1 = new Guna.UI2.WinForms.Guna2ImageButton();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            btn_CancelarCita = new Guna.UI2.WinForms.Guna2Button();
            dtp_BuscarFCita = new Guna.UI2.WinForms.Guna2DateTimePicker();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgv_Citas).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.ButtonHighlight;
            label1.Font = new Font("Segoe UI Black", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(30, 111, 217);
            label1.Location = new Point(47, 27);
            label1.Name = "label1";
            label1.Size = new Size(84, 32);
            label1.TabIndex = 1;
            label1.Text = "CITAS";
            // 
            // dgv_Citas
            // 
            dgv_Citas.AllowUserToAddRows = false;
            dgv_Citas.AllowUserToDeleteRows = false;
            dgv_Citas.BackgroundColor = SystemColors.Control;
            dgv_Citas.BorderStyle = BorderStyle.None;
            dgv_Citas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_Citas.Columns.AddRange(new DataGridViewColumn[] { CitaID, Paciente, Dentista, FechaHora, Motivo, EstadoCita, NotasCita, Creada });
            dgv_Citas.Location = new Point(29, 226);
            dgv_Citas.Name = "dgv_Citas";
            dgv_Citas.ReadOnly = true;
            dgv_Citas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_Citas.Size = new Size(1469, 543);
            dgv_Citas.TabIndex = 6;
            dgv_Citas.CellContentClick += dataGridView1_CellContentClick;
            // 
            // CitaID
            // 
            CitaID.DataPropertyName = "Numerodecita";
            CitaID.HeaderText = "Numero de Cita";
            CitaID.MinimumWidth = 150;
            CitaID.Name = "CitaID";
            CitaID.ReadOnly = true;
            CitaID.Width = 150;
            // 
            // Paciente
            // 
            Paciente.DataPropertyName = "Paciente";
            Paciente.HeaderText = "Paciente";
            Paciente.MinimumWidth = 250;
            Paciente.Name = "Paciente";
            Paciente.ReadOnly = true;
            Paciente.Width = 250;
            // 
            // Dentista
            // 
            Dentista.DataPropertyName = "Dentista";
            Dentista.HeaderText = "Dentista";
            Dentista.MinimumWidth = 250;
            Dentista.Name = "Dentista";
            Dentista.ReadOnly = true;
            Dentista.Width = 250;
            // 
            // FechaHora
            // 
            FechaHora.DataPropertyName = "Fechayhora";
            FechaHora.HeaderText = "Fecha y Hora";
            FechaHora.MinimumWidth = 100;
            FechaHora.Name = "FechaHora";
            FechaHora.ReadOnly = true;
            // 
            // Motivo
            // 
            Motivo.DataPropertyName = "Motivo";
            Motivo.HeaderText = "Motivo";
            Motivo.MinimumWidth = 200;
            Motivo.Name = "Motivo";
            Motivo.ReadOnly = true;
            Motivo.Width = 200;
            // 
            // EstadoCita
            // 
            EstadoCita.DataPropertyName = "Estadodelacita1";
            EstadoCita.HeaderText = "Estado";
            EstadoCita.MinimumWidth = 80;
            EstadoCita.Name = "EstadoCita";
            EstadoCita.ReadOnly = true;
            EstadoCita.Width = 80;
            // 
            // NotasCita
            // 
            NotasCita.DataPropertyName = "Notasdelacita";
            NotasCita.HeaderText = "Notas de la Cita";
            NotasCita.MinimumWidth = 320;
            NotasCita.Name = "NotasCita";
            NotasCita.ReadOnly = true;
            NotasCita.Width = 320;
            // 
            // Creada
            // 
            Creada.DataPropertyName = "Fecha";
            Creada.HeaderText = "Creada";
            Creada.MinimumWidth = 90;
            Creada.Name = "Creada";
            Creada.ReadOnly = true;
            Creada.Width = 90;
            // 
            // guna2ImageButton6
            // 
            guna2ImageButton6.BackColor = Color.Transparent;
            guna2ImageButton6.CheckedState.ImageSize = new Size(64, 64);
            guna2ImageButton6.HoverState.ImageSize = new Size(35, 35);
            guna2ImageButton6.Image = (Image)resources.GetObject("guna2ImageButton6.Image");
            guna2ImageButton6.ImageOffset = new Point(0, 0);
            guna2ImageButton6.ImageRotate = 0F;
            guna2ImageButton6.ImageSize = new Size(32, 32);
            guna2ImageButton6.Location = new Point(1377, 18);
            guna2ImageButton6.Name = "guna2ImageButton6";
            guna2ImageButton6.PressedState.ImageSize = new Size(64, 64);
            guna2ImageButton6.ShadowDecoration.CustomizableEdges = customizableEdges1;
            guna2ImageButton6.Size = new Size(52, 46);
            guna2ImageButton6.TabIndex = 12;
            guna2ImageButton6.UseTransparentBackground = true;
            guna2ImageButton6.Click += guna2ImageButton6_Click;
            // 
            // txt_Buscador
            // 
            txt_Buscador.BorderColor = Color.FromArgb(30, 111, 217);
            txt_Buscador.BorderRadius = 15;
            txt_Buscador.CustomizableEdges = customizableEdges2;
            txt_Buscador.DefaultText = "";
            txt_Buscador.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txt_Buscador.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txt_Buscador.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txt_Buscador.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txt_Buscador.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txt_Buscador.Font = new Font("Segoe UI", 9F);
            txt_Buscador.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txt_Buscador.Location = new Point(284, 23);
            txt_Buscador.Name = "txt_Buscador";
            txt_Buscador.PlaceholderForeColor = Color.Gray;
            txt_Buscador.PlaceholderText = "Buscar Citas";
            txt_Buscador.SelectedText = "";
            txt_Buscador.ShadowDecoration.CustomizableEdges = customizableEdges3;
            txt_Buscador.Size = new Size(592, 36);
            txt_Buscador.TabIndex = 11;
            txt_Buscador.TextChanged += guna2TextBox1_TextChanged;
            // 
            // guna2ImageButton5
            // 
            guna2ImageButton5.BackColor = Color.Transparent;
            guna2ImageButton5.CheckedState.ImageSize = new Size(64, 64);
            guna2ImageButton5.HoverState.ImageSize = new Size(25, 25);
            guna2ImageButton5.Image = (Image)resources.GetObject("guna2ImageButton5.Image");
            guna2ImageButton5.ImageOffset = new Point(0, 0);
            guna2ImageButton5.ImageRotate = 0F;
            guna2ImageButton5.ImageSize = new Size(24, 24);
            guna2ImageButton5.Location = new Point(159, 175);
            guna2ImageButton5.Name = "guna2ImageButton5";
            guna2ImageButton5.PressedState.ImageSize = new Size(64, 64);
            guna2ImageButton5.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2ImageButton5.Size = new Size(42, 42);
            guna2ImageButton5.TabIndex = 14;
            guna2ImageButton5.UseTransparentBackground = true;
            guna2ImageButton5.Click += guna2ImageButton5_Click;
            // 
            // guna2Button2
            // 
            guna2Button2.BorderColor = Color.FromArgb(30, 111, 217);
            guna2Button2.BorderRadius = 20;
            guna2Button2.BorderThickness = 2;
            guna2Button2.CustomizableEdges = customizableEdges5;
            guna2Button2.DisabledState.BorderColor = Color.DarkGray;
            guna2Button2.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button2.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button2.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button2.FillColor = SystemColors.ButtonHighlight;
            guna2Button2.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button2.ForeColor = Color.FromArgb(30, 111, 217);
            guna2Button2.Location = new Point(63, 175);
            guna2Button2.Name = "guna2Button2";
            guna2Button2.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2Button2.Size = new Size(147, 45);
            guna2Button2.TabIndex = 13;
            guna2Button2.Text = "  NUEVA CITA";
            guna2Button2.TextAlign = HorizontalAlignment.Left;
            guna2Button2.Click += guna2Button2_Click;
            // 
            // guna2ImageButton2
            // 
            guna2ImageButton2.BackColor = Color.Transparent;
            guna2ImageButton2.CheckedState.ImageSize = new Size(64, 64);
            guna2ImageButton2.HoverState.ImageSize = new Size(35, 35);
            guna2ImageButton2.Image = (Image)resources.GetObject("guna2ImageButton2.Image");
            guna2ImageButton2.ImageOffset = new Point(0, 0);
            guna2ImageButton2.ImageRotate = 0F;
            guna2ImageButton2.ImageSize = new Size(24, 24);
            guna2ImageButton2.Location = new Point(1455, 174);
            guna2ImageButton2.Name = "guna2ImageButton2";
            guna2ImageButton2.PressedState.ImageSize = new Size(64, 64);
            guna2ImageButton2.ShadowDecoration.CustomizableEdges = customizableEdges7;
            guna2ImageButton2.Size = new Size(43, 43);
            guna2ImageButton2.TabIndex = 16;
            guna2ImageButton2.UseTransparentBackground = true;
            guna2ImageButton2.Click += guna2ImageButton2_Click;
            // 
            // guna2ImageButton1
            // 
            guna2ImageButton1.BackColor = Color.Transparent;
            guna2ImageButton1.CheckedState.ImageSize = new Size(64, 64);
            guna2ImageButton1.HoverState.ImageSize = new Size(35, 35);
            guna2ImageButton1.Image = (Image)resources.GetObject("guna2ImageButton1.Image");
            guna2ImageButton1.ImageOffset = new Point(0, 0);
            guna2ImageButton1.ImageRotate = 0F;
            guna2ImageButton1.ImageSize = new Size(24, 24);
            guna2ImageButton1.Location = new Point(284, 174);
            guna2ImageButton1.Name = "guna2ImageButton1";
            guna2ImageButton1.PressedState.ImageSize = new Size(64, 64);
            guna2ImageButton1.ShadowDecoration.CustomizableEdges = customizableEdges8;
            guna2ImageButton1.Size = new Size(52, 46);
            guna2ImageButton1.TabIndex = 15;
            guna2ImageButton1.UseTransparentBackground = true;
            guna2ImageButton1.Click += guna2ImageButton1_Click;
            // 
            // guna2Button1
            // 
            guna2Button1.BorderColor = Color.FromArgb(30, 111, 217);
            guna2Button1.BorderRadius = 20;
            guna2Button1.BorderThickness = 2;
            guna2Button1.CustomizableEdges = customizableEdges9;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = SystemColors.ButtonHighlight;
            guna2Button1.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button1.ForeColor = Color.FromArgb(30, 111, 217);
            guna2Button1.Location = new Point(216, 175);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges10;
            guna2Button1.Size = new Size(132, 45);
            guna2Button1.TabIndex = 17;
            guna2Button1.Text = "   EDITAR";
            guna2Button1.TextAlign = HorizontalAlignment.Left;
            guna2Button1.Click += guna2Button1_Click;
            // 
            // btn_CancelarCita
            // 
            btn_CancelarCita.BorderColor = Color.Red;
            btn_CancelarCita.BorderRadius = 20;
            btn_CancelarCita.BorderThickness = 2;
            btn_CancelarCita.CustomizableEdges = customizableEdges11;
            btn_CancelarCita.DisabledState.BorderColor = Color.DarkGray;
            btn_CancelarCita.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_CancelarCita.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_CancelarCita.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_CancelarCita.FillColor = SystemColors.ButtonHighlight;
            btn_CancelarCita.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_CancelarCita.ForeColor = Color.Red;
            btn_CancelarCita.Location = new Point(1377, 175);
            btn_CancelarCita.Name = "btn_CancelarCita";
            btn_CancelarCita.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btn_CancelarCita.Size = new Size(127, 45);
            btn_CancelarCita.TabIndex = 18;
            btn_CancelarCita.Text = " CANCELAR";
            btn_CancelarCita.TextAlign = HorizontalAlignment.Left;
            btn_CancelarCita.Click += btn_CancelarCita_Click;
            // 
            // dtp_BuscarFCita
            // 
            dtp_BuscarFCita.BorderRadius = 15;
            dtp_BuscarFCita.Checked = true;
            dtp_BuscarFCita.CustomizableEdges = customizableEdges13;
            dtp_BuscarFCita.FillColor = Color.FromArgb(30, 111, 217);
            dtp_BuscarFCita.Font = new Font("Segoe UI", 9F);
            dtp_BuscarFCita.ForeColor = SystemColors.Control;
            dtp_BuscarFCita.Format = DateTimePickerFormat.Long;
            dtp_BuscarFCita.Location = new Point(1137, 23);
            dtp_BuscarFCita.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtp_BuscarFCita.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtp_BuscarFCita.Name = "dtp_BuscarFCita";
            dtp_BuscarFCita.ShadowDecoration.CustomizableEdges = customizableEdges14;
            dtp_BuscarFCita.Size = new Size(234, 36);
            dtp_BuscarFCita.TabIndex = 19;
            dtp_BuscarFCita.Value = new DateTime(2026, 5, 1, 23, 15, 4, 278);
            dtp_BuscarFCita.ValueChanged += dtp_BuscarFCita_ValueChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Gray;
            label2.Location = new Point(989, 32);
            label2.Name = "label2";
            label2.Size = new Size(132, 20);
            label2.TabIndex = 20;
            label2.Text = "Buscar Por Fecha:";
            // 
            // UC_Citas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            Controls.Add(label2);
            Controls.Add(dtp_BuscarFCita);
            Controls.Add(guna2ImageButton2);
            Controls.Add(btn_CancelarCita);
            Controls.Add(guna2ImageButton1);
            Controls.Add(guna2Button1);
            Controls.Add(guna2ImageButton5);
            Controls.Add(guna2Button2);
            Controls.Add(guna2ImageButton6);
            Controls.Add(txt_Buscador);
            Controls.Add(dgv_Citas);
            Controls.Add(label1);
            Name = "UC_Citas";
            Size = new Size(1527, 772);
            ((System.ComponentModel.ISupportInitialize)dgv_Citas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private DataGridView dgv_Citas;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton6;
        private Guna.UI2.WinForms.Guna2TextBox txt_Buscador;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton5;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton2;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton1;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2Button btn_CancelarCita;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtp_BuscarFCita;
        private Label label2;
        private DataGridViewTextBoxColumn CitaID;
        private DataGridViewTextBoxColumn Paciente;
        private DataGridViewTextBoxColumn Dentista;
        private DataGridViewTextBoxColumn FechaHora;
        private DataGridViewTextBoxColumn Motivo;
        private DataGridViewTextBoxColumn EstadoCita;
        private DataGridViewTextBoxColumn NotasCita;
        private DataGridViewTextBoxColumn Creada;
    }
}
