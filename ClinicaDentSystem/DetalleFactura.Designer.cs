namespace ClinicaDentSystem
{
    partial class DetalleFactura
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lblTitulo = new Label();
            dgvDetalle = new DataGridView();
            panelInferior = new Panel();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            lblTotal = new Label();
            guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
            panelInferior.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(30, 111, 217);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Padding = new Padding(16, 0, 0, 0);
            lblTitulo.Size = new Size(784, 54);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Detalle de factura";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            lblTitulo.Click += lblTitulo_Click;
            // 
            // dgvDetalle
            // 
            dgvDetalle.AllowUserToAddRows = false;
            dgvDetalle.AllowUserToDeleteRows = false;
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalle.BackgroundColor = Color.White;
            dgvDetalle.BorderStyle = BorderStyle.None;
            dgvDetalle.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalle.Dock = DockStyle.Fill;
            dgvDetalle.Location = new Point(0, 54);
            dgvDetalle.MultiSelect = false;
            dgvDetalle.Name = "dgvDetalle";
            dgvDetalle.ReadOnly = true;
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalle.Size = new Size(784, 357);
            dgvDetalle.TabIndex = 1;
            // 
            // panelInferior
            // 
            panelInferior.Controls.Add(guna2Button2);
            panelInferior.Controls.Add(guna2Button1);
            panelInferior.Controls.Add(lblTotal);
            panelInferior.Dock = DockStyle.Bottom;
            panelInferior.Location = new Point(0, 411);
            panelInferior.Name = "panelInferior";
            panelInferior.Size = new Size(784, 50);
            panelInferior.TabIndex = 2;
            // 
            // guna2Button1
            // 
            guna2Button1.BorderColor = Color.FromArgb(30, 111, 217);
            guna2Button1.BorderRadius = 20;
            guna2Button1.BorderThickness = 2;
            guna2Button1.CustomizableEdges = customizableEdges3;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = SystemColors.ButtonHighlight;
            guna2Button1.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button1.ForeColor = Color.FromArgb(30, 111, 217);
            guna2Button1.Location = new Point(685, 3);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Button1.Size = new Size(87, 45);
            guna2Button1.TabIndex = 47;
            guna2Button1.Text = "CERRAR";
            guna2Button1.Click += guna2Button1_Click;
            // 
            // lblTotal
            // 
            lblTotal.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(30, 111, 217);
            lblTotal.Location = new Point(12, 13);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(637, 24);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "Total detalle: $0.00";
            lblTotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // guna2Button2
            // 
            guna2Button2.BorderColor = Color.Green;
            guna2Button2.BorderRadius = 20;
            guna2Button2.BorderThickness = 2;
            guna2Button2.CustomizableEdges = customizableEdges1;
            guna2Button2.DisabledState.BorderColor = Color.DarkGray;
            guna2Button2.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button2.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button2.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button2.FillColor = SystemColors.ButtonHighlight;
            guna2Button2.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button2.ForeColor = Color.Green;
            guna2Button2.Location = new Point(0, 3);
            guna2Button2.Name = "guna2Button2";
            guna2Button2.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Button2.Size = new Size(198, 45);
            guna2Button2.TabIndex = 48;
            guna2Button2.Text = "MARCAR COMO PAGADA";
            // 
            // DetalleFactura
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(784, 461);
            Controls.Add(dgvDetalle);
            Controls.Add(panelInferior);
            Controls.Add(lblTitulo);
            MinimizeBox = false;
            Name = "DetalleFactura";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Detalle de factura";
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
            panelInferior.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitulo;
        private DataGridView dgvDetalle;
        private Panel panelInferior;
        private Label lblTotal;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
    }
}
