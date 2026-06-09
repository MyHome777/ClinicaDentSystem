namespace ClinicaDentSystem
{
    partial class UC_Dentistas
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_Dentistas));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label1 = new Label();
            dataGridView1 = new DataGridView();
            txtBuscar = new Guna.UI2.WinForms.Guna2TextBox();
            guna2ImageButton6 = new Guna.UI2.WinForms.Guna2ImageButton();
            guna2ImageButton3 = new Guna.UI2.WinForms.Guna2ImageButton();
            guna2ImageButton1 = new Guna.UI2.WinForms.Guna2ImageButton();
            guna2ImageButton2 = new Guna.UI2.WinForms.Guna2ImageButton();
            guna2ImageButton5 = new Guna.UI2.WinForms.Guna2ImageButton();
            guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
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
            label1.Size = new Size(148, 32);
            label1.TabIndex = 2;
            label1.Text = "DENTISTAS";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(47, 226);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(1437, 543);
            dataGridView1.TabIndex = 6;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // txtBuscar
            // 
            txtBuscar.BorderColor = Color.FromArgb(30, 111, 217);
            txtBuscar.BorderRadius = 15;
            txtBuscar.CustomizableEdges = customizableEdges1;
            txtBuscar.DefaultText = "";
            txtBuscar.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtBuscar.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtBuscar.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtBuscar.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtBuscar.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtBuscar.Font = new Font("Segoe UI", 9F);
            txtBuscar.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtBuscar.Location = new Point(479, 23);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderForeColor = Color.Gray;
            txtBuscar.PlaceholderText = "Buscar Pacientes";
            txtBuscar.SelectedText = "";
            txtBuscar.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtBuscar.Size = new Size(592, 36);
            txtBuscar.TabIndex = 7;
            txtBuscar.KeyDown += txtBuscar_KeyDown;
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
            guna2ImageButton6.Location = new Point(1068, 18);
            guna2ImageButton6.Name = "guna2ImageButton6";
            guna2ImageButton6.PressedState.ImageSize = new Size(64, 64);
            guna2ImageButton6.ShadowDecoration.CustomizableEdges = customizableEdges3;
            guna2ImageButton6.Size = new Size(52, 46);
            guna2ImageButton6.TabIndex = 11;
            guna2ImageButton6.UseTransparentBackground = true;
            guna2ImageButton6.Click += guna2ImageButton6_Click;
            // 
            // guna2ImageButton3
            // 
            guna2ImageButton3.CheckedState.ImageSize = new Size(64, 64);
            guna2ImageButton3.HoverState.ImageSize = new Size(35, 35);
            guna2ImageButton3.Image = (Image)resources.GetObject("guna2ImageButton3.Image");
            guna2ImageButton3.ImageOffset = new Point(0, 0);
            guna2ImageButton3.ImageRotate = 0F;
            guna2ImageButton3.ImageSize = new Size(32, 32);
            guna2ImageButton3.Location = new Point(1346, 13);
            guna2ImageButton3.Name = "guna2ImageButton3";
            guna2ImageButton3.PressedState.ImageSize = new Size(64, 64);
            guna2ImageButton3.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2ImageButton3.Size = new Size(52, 46);
            guna2ImageButton3.TabIndex = 12;
            guna2ImageButton3.Click += guna2ImageButton3_Click;
            // 
            // guna2ImageButton1
            // 
            guna2ImageButton1.CheckedState.ImageSize = new Size(64, 64);
            guna2ImageButton1.HoverState.ImageSize = new Size(35, 35);
            guna2ImageButton1.Image = (Image)resources.GetObject("guna2ImageButton1.Image");
            guna2ImageButton1.ImageOffset = new Point(0, 0);
            guna2ImageButton1.ImageRotate = 0F;
            guna2ImageButton1.ImageSize = new Size(32, 32);
            guna2ImageButton1.Location = new Point(1404, 13);
            guna2ImageButton1.Name = "guna2ImageButton1";
            guna2ImageButton1.PressedState.ImageSize = new Size(64, 64);
            guna2ImageButton1.ShadowDecoration.CustomizableEdges = customizableEdges5;
            guna2ImageButton1.Size = new Size(52, 46);
            guna2ImageButton1.TabIndex = 13;
            guna2ImageButton1.Click += guna2ImageButton1_Click;
            // 
            // guna2ImageButton2
            // 
            guna2ImageButton2.CheckedState.ImageSize = new Size(64, 64);
            guna2ImageButton2.HoverState.ImageSize = new Size(35, 35);
            guna2ImageButton2.Image = (Image)resources.GetObject("guna2ImageButton2.Image");
            guna2ImageButton2.ImageOffset = new Point(0, 0);
            guna2ImageButton2.ImageRotate = 0F;
            guna2ImageButton2.ImageSize = new Size(32, 32);
            guna2ImageButton2.Location = new Point(1462, 13);
            guna2ImageButton2.Name = "guna2ImageButton2";
            guna2ImageButton2.PressedState.ImageSize = new Size(64, 64);
            guna2ImageButton2.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2ImageButton2.Size = new Size(52, 46);
            guna2ImageButton2.TabIndex = 14;
            guna2ImageButton2.Click += guna2ImageButton2_Click;
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
            guna2ImageButton5.Location = new Point(189, 175);
            guna2ImageButton5.Name = "guna2ImageButton5";
            guna2ImageButton5.PressedState.ImageSize = new Size(64, 64);
            guna2ImageButton5.ShadowDecoration.CustomizableEdges = customizableEdges7;
            guna2ImageButton5.Size = new Size(42, 42);
            guna2ImageButton5.TabIndex = 17;
            guna2ImageButton5.UseTransparentBackground = true;
            // 
            // guna2Button2
            // 
            guna2Button2.BorderColor = Color.FromArgb(30, 111, 217);
            guna2Button2.BorderRadius = 20;
            guna2Button2.BorderThickness = 2;
            guna2Button2.CustomizableEdges = customizableEdges8;
            guna2Button2.DisabledState.BorderColor = Color.DarkGray;
            guna2Button2.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button2.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button2.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button2.FillColor = SystemColors.ButtonHighlight;
            guna2Button2.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button2.ForeColor = Color.FromArgb(30, 111, 217);
            guna2Button2.Location = new Point(63, 175);
            guna2Button2.Name = "guna2Button2";
            guna2Button2.ShadowDecoration.CustomizableEdges = customizableEdges9;
            guna2Button2.Size = new Size(180, 45);
            guna2Button2.TabIndex = 16;
            guna2Button2.Text = "  VER MAS DATOS";
            guna2Button2.TextAlign = HorizontalAlignment.Left;
            // 
            // UC_Dentistas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            Controls.Add(guna2ImageButton5);
            Controls.Add(guna2Button2);
            Controls.Add(guna2ImageButton2);
            Controls.Add(guna2ImageButton1);
            Controls.Add(guna2ImageButton3);
            Controls.Add(guna2ImageButton6);
            Controls.Add(txtBuscar);
            Controls.Add(dataGridView1);
            Controls.Add(label1);
            Name = "UC_Dentistas";
            Size = new Size(1527, 772);
            Load += UC_Dentistas_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private DataGridView dataGridView1;
        private Guna.UI2.WinForms.Guna2TextBox txtBuscar;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton6;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton3;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton1;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton2;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton5;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
    }
}
