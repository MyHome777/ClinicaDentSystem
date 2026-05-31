using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClinicaDentSystem
{
    public partial class Dashboard : Form
    {
        private const int DashboardDesignWidth = 1390;
        private const int SidebarDesignWidth = 235;
        private const int SidebarMinWidth = 185;
        private const int SidebarMaxWidth = 320;

        public Dashboard()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = false;
            ResponsiveLayout.Configure(this);
            SizeChanged += Dashboard_SizeChanged;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            RebuildDashboardLayout();
            UpdateSidebarWidth();
            guna2Panel2.AutoScroll = true;
            guna2Panel3.AutoScroll = true;

            CargarVista(new UC_Inicio());
        }

        private void CargarVista(UserControl control)
        {
            guna2Panel3.Controls.Clear();
            control.Dock = DockStyle.Fill;
            guna2Panel3.Controls.Add(control);
        }

        private void Dashboard_SizeChanged(object? sender, EventArgs e)
        {
            UpdateSidebarWidth();
        }

        private void RebuildDashboardLayout()
        {
            guna2Panel1.SuspendLayout();
            SuspendLayout();

            guna2Panel1.Controls.Remove(guna2Panel2);
            guna2Panel1.Controls.Remove(guna2Panel3);

            guna2Panel3.Dock = DockStyle.Fill;
            guna2Panel2.Dock = DockStyle.Left;

            // El panel de contenido se agrega primero para que DockStyle.Fill
            // respete el ancho real del sidebar en lugar de quedar debajo.
            guna2Panel1.Controls.Add(guna2Panel3);
            guna2Panel1.Controls.Add(guna2Panel2);

            guna2Panel1.ResumeLayout(true);
            ResumeLayout(true);
        }

        private void UpdateSidebarWidth()
        {
            if (ClientSize.Width <= 0)
            {
                return;
            }

            var scale = Math.Clamp(ClientSize.Width / (float)DashboardDesignWidth, 0.78f, 1.25f);
            var sidebarWidth = (int)Math.Round(SidebarDesignWidth * scale, MidpointRounding.AwayFromZero);
            guna2Panel2.Width = Math.Clamp(sidebarWidth, SidebarMinWidth, SidebarMaxWidth);
        }

        // SIDEBAR BOTONES
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            CargarVista(new UC_Inicio());
        }

        private void btnPacientes_Click(object sender, EventArgs e)
        {
            CargarVista(new UC_Pacientes1());
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            CargarVista(new UC_Citas());
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            CargarVista(new UC_Dentistas());
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            CargarVista(new UC_Inventario());
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            CargarVista(new UC_Facturacion());
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            CargarVista(new UC_Historial());
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            CargarVista(new UC_Auditorias());
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            CargarVista(new UC_Usuarios());
        }

        // LOGOUT
        private void guna2ImageRadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2ImageRadioButton3_CheckedChanged_1(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            CargarVista(new UC_Pacientes1());
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            CargarVista(new UC_Citas());
        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            CargarVista(new UC_Pacientes1());
        }

        private void guna2Button3_Click_2(object sender, EventArgs e)
        {
            CargarVista(new UC_Citas());
        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            CargarVista(new UC_Dentistas());
        }

        private void guna2Button5_Click_1(object sender, EventArgs e)
        {
            CargarVista(new UC_Inventario());
        }

        private void guna2Button6_Click_1(object sender, EventArgs e)
        {
            CargarVista(new UC_Facturacion());
        }

        private void guna2Button7_Click_1(object sender, EventArgs e)
        {
            CargarVista(new UC_Historial());
        }

        private void guna2Button8_Click_1(object sender, EventArgs e)
        {
            CargarVista(new UC_Auditorias());
        }

        private void guna2ImageButton4_Click_1(object sender, EventArgs e)
        {
            CargarVista(new UC_Usuarios());
        }

        private void guna2ImageRadioButton3_CheckedChanged_2(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            CargarVista(new UC_Inicio());
        }
    }
}
