namespace ClinicaDentSystem
{
    partial class UC_Usuarios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_Usuarios));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label1 = new Label();
            txtBuscar = new Guna.UI2.WinForms.Guna2TextBox();
            btnBuscar = new Guna.UI2.WinForms.Guna2ImageButton();
            btnAgregar = new Guna.UI2.WinForms.Guna2ImageButton();
            btnEditar = new Guna.UI2.WinForms.Guna2ImageButton();
            btnEliminar = new Guna.UI2.WinForms.Guna2ImageButton();
            dgvUsuarios = new DataGridView();
            guna2ImageButton4 = new Guna.UI2.WinForms.Guna2ImageButton();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
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
            label1.Size = new Size(138, 32);
            label1.TabIndex = 4;
            label1.Text = "USUARIOS";
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
            txtBuscar.PlaceholderText = "Buscar Usuarios";
            txtBuscar.SelectedText = "";
            txtBuscar.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtBuscar.Size = new Size(592, 36);
            txtBuscar.TabIndex = 5;
            txtBuscar.KeyDown += txtBuscar_KeyDown;
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
            btnBuscar.ShadowDecoration.CustomizableEdges = customizableEdges3;
            btnBuscar.Size = new Size(52, 46);
            btnBuscar.TabIndex = 11;
            btnBuscar.UseTransparentBackground = true;
            btnBuscar.Click += btnBuscar_Click;
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
            btnAgregar.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnAgregar.Size = new Size(52, 46);
            btnAgregar.TabIndex = 12;
            btnAgregar.Click += guna2ImageButton3_Click;
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
            btnEditar.ShadowDecoration.CustomizableEdges = customizableEdges5;
            btnEditar.Size = new Size(52, 46);
            btnEditar.TabIndex = 13;
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
            btnEliminar.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnEliminar.Size = new Size(52, 46);
            btnEliminar.TabIndex = 14;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.BackgroundColor = SystemColors.Control;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Location = new Point(47, 226);
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.Size = new Size(1440, 543);
            dgvUsuarios.TabIndex = 15;
            // 
            // guna2ImageButton4
            // 
            guna2ImageButton4.BackColor = Color.Transparent;
            guna2ImageButton4.CheckedState.ImageSize = new Size(64, 64);
            guna2ImageButton4.HoverState.ImageSize = new Size(25, 25);
            guna2ImageButton4.Image = (Image)resources.GetObject("guna2ImageButton4.Image");
            guna2ImageButton4.ImageOffset = new Point(0, 0);
            guna2ImageButton4.ImageRotate = 0F;
            guna2ImageButton4.ImageSize = new Size(24, 24);
            guna2ImageButton4.Location = new Point(248, 175);
            guna2ImageButton4.Name = "guna2ImageButton4";
            guna2ImageButton4.PressedState.ImageSize = new Size(64, 64);
            guna2ImageButton4.ShadowDecoration.CustomizableEdges = customizableEdges7;
            guna2ImageButton4.Size = new Size(46, 42);
            guna2ImageButton4.TabIndex = 17;
            guna2ImageButton4.UseTransparentBackground = true;
            // 
            // guna2Button1
            // 
            guna2Button1.BorderColor = Color.FromArgb(30, 111, 217);
            guna2Button1.BorderRadius = 20;
            guna2Button1.BorderThickness = 2;
            guna2Button1.CustomizableEdges = customizableEdges8;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = SystemColors.ButtonHighlight;
            guna2Button1.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button1.ForeColor = Color.FromArgb(30, 111, 217);
            guna2Button1.Location = new Point(63, 175);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges9;
            guna2Button1.Size = new Size(241, 45);
            guna2Button1.TabIndex = 16;
            guna2Button1.Text = " VER DETALLES DEL USUARIO";
            guna2Button1.TextAlign = HorizontalAlignment.Left;
            // 
            // UC_Usuarios
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            Controls.Add(guna2ImageButton4);
            Controls.Add(guna2Button1);
            Controls.Add(dgvUsuarios);
            Controls.Add(btnEliminar);
            Controls.Add(btnEditar);
            Controls.Add(btnAgregar);
            Controls.Add(btnBuscar);
            Controls.Add(txtBuscar);
            Controls.Add(label1);
            Name = "UC_Usuarios";
            Size = new Size(1527, 772);
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Guna.UI2.WinForms.Guna2TextBox txtBuscar;
        private Guna.UI2.WinForms.Guna2ImageButton btnBuscar;
        private Guna.UI2.WinForms.Guna2ImageButton btnAgregar;
        private Guna.UI2.WinForms.Guna2ImageButton btnEditar;
        private Guna.UI2.WinForms.Guna2ImageButton btnEliminar;
        private DataGridView dgvUsuarios;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton4;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
    }
}
