namespace ClinicaDentSystem
{
    partial class UC_Historial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_Historial));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label1 = new Label();
            guna2ImageButton6 = new Guna.UI2.WinForms.Guna2ImageButton();
            guna2TextBox1 = new Guna.UI2.WinForms.Guna2TextBox();
            dataGridView1 = new DataGridView();
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
            label1.Size = new Size(143, 32);
            label1.TabIndex = 2;
            label1.Text = "HISTORIAL";
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
            guna2ImageButton6.Location = new Point(1029, 18);
            guna2ImageButton6.Name = "guna2ImageButton6";
            guna2ImageButton6.PressedState.ImageSize = new Size(64, 64);
            guna2ImageButton6.ShadowDecoration.CustomizableEdges = customizableEdges1;
            guna2ImageButton6.Size = new Size(52, 46);
            guna2ImageButton6.TabIndex = 12;
            guna2ImageButton6.UseTransparentBackground = true;
            // 
            // guna2TextBox1
            // 
            guna2TextBox1.BorderColor = Color.FromArgb(30, 111, 217);
            guna2TextBox1.BorderRadius = 15;
            guna2TextBox1.CustomizableEdges = customizableEdges2;
            guna2TextBox1.DefaultText = "";
            guna2TextBox1.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            guna2TextBox1.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            guna2TextBox1.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            guna2TextBox1.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            guna2TextBox1.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2TextBox1.Font = new Font("Segoe UI", 9F);
            guna2TextBox1.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2TextBox1.Location = new Point(440, 23);
            guna2TextBox1.Name = "guna2TextBox1";
            guna2TextBox1.PlaceholderForeColor = Color.Gray;
            guna2TextBox1.PlaceholderText = "Buscar ";
            guna2TextBox1.SelectedText = "";
            guna2TextBox1.ShadowDecoration.CustomizableEdges = customizableEdges3;
            guna2TextBox1.Size = new Size(592, 36);
            guna2TextBox1.TabIndex = 11;
            guna2TextBox1.TextChanged += guna2TextBox1_TextChanged;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(47, 81);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1439, 764);
            dataGridView1.TabIndex = 13;
            // 
            // UC_Historial
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            Controls.Add(dataGridView1);
            Controls.Add(guna2ImageButton6);
            Controls.Add(guna2TextBox1);
            Controls.Add(label1);
            Name = "UC_Historial";
            Size = new Size(1527, 772);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton6;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox1;
        private DataGridView dataGridView1;
    }
}
