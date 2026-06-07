namespace ClinicaDentSystem
{
    partial class UC_Pacientes1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_Pacientes1));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label1 = new Label();
            txtBuscar = new Guna.UI2.WinForms.Guna2TextBox();
            btnEditar = new Guna.UI2.WinForms.Guna2ImageButton();
            btnEliminar = new Guna.UI2.WinForms.Guna2ImageButton();
            btnAgregar = new Guna.UI2.WinForms.Guna2ImageButton();
            dataGridView1 = new DataGridView();
            guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            guna2ImageButton5 = new Guna.UI2.WinForms.Guna2ImageButton();
            btnBuscar = new Guna.UI2.WinForms.Guna2ImageButton();
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
            label1.Size = new Size(145, 32);
            label1.TabIndex = 0;
            label1.Text = "PACIENTES";
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
            txtBuscar.TabIndex = 1;
            txtBuscar.KeyDown += txtBuscar_KeyDown;
            // 
            // btnEditar
            // 
            btnEditar.CheckedState.ImageSize = new Size(64, 64);
            btnEditar.HoverState.ImageSize = new Size(35, 35);
            btnEditar.Image = (Image)resources.GetObject("btnEditar.Image");
            btnEditar.ImageOffset = new Point(0, 0);
            btnEditar.ImageRotate = 0F;
            btnEditar.ImageSize = new Size(32, 32);
            btnEditar.Location = new Point(1404, 13);
            btnEditar.Name = "btnEditar";
            btnEditar.PressedState.ImageSize = new Size(64, 64);
            btnEditar.ShadowDecoration.CustomizableEdges = customizableEdges3;
            btnEditar.Size = new Size(52, 46);
            btnEditar.TabIndex = 2;
            btnEditar.Click += guna2ImageButton1_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.CheckedState.ImageSize = new Size(64, 64);
            btnEliminar.HoverState.ImageSize = new Size(35, 35);
            btnEliminar.Image = (Image)resources.GetObject("btnEliminar.Image");
            btnEliminar.ImageOffset = new Point(0, 0);
            btnEliminar.ImageRotate = 0F;
            btnEliminar.ImageSize = new Size(32, 32);
            btnEliminar.Location = new Point(1462, 13);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.PressedState.ImageSize = new Size(64, 64);
            btnEliminar.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnEliminar.Size = new Size(52, 46);
            btnEliminar.TabIndex = 3;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnAgregar
            // 
            btnAgregar.CheckedState.ImageSize = new Size(64, 64);
            btnAgregar.HoverState.ImageSize = new Size(35, 35);
            btnAgregar.Image = (Image)resources.GetObject("btnAgregar.Image");
            btnAgregar.ImageOffset = new Point(0, 0);
            btnAgregar.ImageRotate = 0F;
            btnAgregar.ImageSize = new Size(32, 32);
            btnAgregar.Location = new Point(1346, 13);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.PressedState.ImageSize = new Size(64, 64);
            btnAgregar.ShadowDecoration.CustomizableEdges = customizableEdges5;
            btnAgregar.Size = new Size(52, 46);
            btnAgregar.TabIndex = 4;
            btnAgregar.Click += guna2ImageButton3_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(47, 226);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1427, 620);
            dataGridView1.TabIndex = 5;
            // 
            // guna2Button2
            // 
            guna2Button2.BorderColor = Color.FromArgb(30, 111, 217);
            guna2Button2.BorderRadius = 12;
            guna2Button2.BorderThickness = 2;
            guna2Button2.CustomizableEdges = customizableEdges6;
            guna2Button2.DisabledState.BorderColor = Color.DarkGray;
            guna2Button2.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button2.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button2.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button2.FillColor = SystemColors.ButtonHighlight;
            guna2Button2.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button2.ForeColor = Color.FromArgb(30, 111, 217);
            guna2Button2.Location = new Point(63, 175);
            guna2Button2.Name = "guna2Button2";
            guna2Button2.ShadowDecoration.CustomizableEdges = customizableEdges7;
            guna2Button2.Size = new Size(180, 45);
            guna2Button2.TabIndex = 7;
            guna2Button2.Text = "  VER EXPEDIENTE";
            guna2Button2.TextAlign = HorizontalAlignment.Left;
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
            guna2ImageButton5.Location = new Point(185, 175);
            guna2ImageButton5.Name = "guna2ImageButton5";
            guna2ImageButton5.PressedState.ImageSize = new Size(64, 64);
            guna2ImageButton5.ShadowDecoration.CustomizableEdges = customizableEdges8;
            guna2ImageButton5.Size = new Size(42, 42);
            guna2ImageButton5.TabIndex = 9;
            guna2ImageButton5.UseTransparentBackground = true;
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = Color.Transparent;
            btnBuscar.CheckedState.ImageSize = new Size(64, 64);
            btnBuscar.HoverState.ImageSize = new Size(35, 35);
            btnBuscar.Image = (Image)resources.GetObject("btnBuscar.Image");
            btnBuscar.ImageOffset = new Point(0, 0);
            btnBuscar.ImageRotate = 0F;
            btnBuscar.ImageSize = new Size(32, 32);
            btnBuscar.Location = new Point(1068, 18);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.PressedState.ImageSize = new Size(64, 64);
            btnBuscar.ShadowDecoration.CustomizableEdges = customizableEdges9;
            btnBuscar.Size = new Size(52, 46);
            btnBuscar.TabIndex = 10;
            btnBuscar.UseTransparentBackground = true;
            btnBuscar.Click += guna2ImageButton6_Click;
            // 
            // UC_Pacientes1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            Controls.Add(btnBuscar);
            Controls.Add(guna2ImageButton5);
            Controls.Add(guna2Button2);
            Controls.Add(dataGridView1);
            Controls.Add(btnAgregar);
            Controls.Add(btnEliminar);
            Controls.Add(btnEditar);
            Controls.Add(txtBuscar);
            Controls.Add(label1);
            Name = "UC_Pacientes1";
            Size = new Size(1525, 849);
            Load += UC_Pacientes1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Guna.UI2.WinForms.Guna2TextBox txtBuscar;
        private Guna.UI2.WinForms.Guna2ImageButton btnEditar;
        private Guna.UI2.WinForms.Guna2ImageButton btnEliminar;
        private Guna.UI2.WinForms.Guna2ImageButton btnAgregar;
        private DataGridView dataGridView1;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton5;
        private Guna.UI2.WinForms.Guna2ImageButton btnBuscar;
    }
}
