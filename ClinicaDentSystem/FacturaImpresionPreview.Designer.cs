namespace ClinicaDentSystem
{
    partial class FacturaImpresionPreview
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (disposing)
            {
                _documento.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            panelSuperior = new Panel();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            lblTitulo = new Label();
            printPreviewControl1 = new PrintPreviewControl();
            panelSuperior.SuspendLayout();
            SuspendLayout();
            // 
            // panelSuperior
            // 
            panelSuperior.Controls.Add(guna2Button1);
            panelSuperior.Controls.Add(guna2Button2);
            panelSuperior.Controls.Add(lblTitulo);
            panelSuperior.Dock = DockStyle.Top;
            panelSuperior.Location = new Point(0, 0);
            panelSuperior.Name = "panelSuperior";
            panelSuperior.Size = new Size(984, 58);
            panelSuperior.TabIndex = 0;
            // 
            // guna2Button1
            // 
            guna2Button1.BorderColor = Color.FromArgb(30, 111, 217);
            guna2Button1.BorderRadius = 20;
            guna2Button1.BorderThickness = 2;
            guna2Button1.CustomizableEdges = customizableEdges1;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = SystemColors.ButtonHighlight;
            guna2Button1.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button1.ForeColor = Color.FromArgb(30, 111, 217);
            guna2Button1.Location = new Point(885, 7);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Button1.Size = new Size(87, 45);
            guna2Button1.TabIndex = 46;
            guna2Button1.Text = "  CERRAR";
            guna2Button1.TextAlign = HorizontalAlignment.Left;
            guna2Button1.Click += guna2Button1_Click;
            // 
            // guna2Button2
            // 
            guna2Button2.BorderColor = Color.FromArgb(30, 111, 217);
            guna2Button2.BorderRadius = 20;
            guna2Button2.BorderThickness = 2;
            guna2Button2.CustomizableEdges = customizableEdges3;
            guna2Button2.DisabledState.BorderColor = Color.DarkGray;
            guna2Button2.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button2.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button2.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button2.FillColor = SystemColors.ButtonHighlight;
            guna2Button2.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button2.ForeColor = Color.FromArgb(30, 111, 217);
            guna2Button2.Location = new Point(780, 7);
            guna2Button2.Name = "guna2Button2";
            guna2Button2.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Button2.Size = new Size(99, 45);
            guna2Button2.TabIndex = 45;
            guna2Button2.Text = "  IMPRIMIR";
            guna2Button2.TextAlign = HorizontalAlignment.Left;
            guna2Button2.Click += guna2Button2_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(30, 111, 217);
            lblTitulo.Location = new Point(16, 8);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(724, 42);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "VISTA PREVIA DE LA FACTURA";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            lblTitulo.Click += lblTitulo_Click;
            // 
            // printPreviewControl1
            // 
            printPreviewControl1.AutoZoom = false;
            printPreviewControl1.Dock = DockStyle.Fill;
            printPreviewControl1.Location = new Point(0, 58);
            printPreviewControl1.Name = "printPreviewControl1";
            printPreviewControl1.Size = new Size(984, 603);
            printPreviewControl1.TabIndex = 1;
            printPreviewControl1.Zoom = 1D;
            // 
            // FacturaImpresionPreview
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(984, 661);
            Controls.Add(printPreviewControl1);
            Controls.Add(panelSuperior);
            MinimizeBox = false;
            Name = "FacturaImpresionPreview";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Vista previa de factura";
            panelSuperior.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelSuperior;
        private Label lblTitulo;
        private PrintPreviewControl printPreviewControl1;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
    }
}
