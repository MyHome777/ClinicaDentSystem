using Guna.UI2.WinForms;
using Modelos;
using System;
using System.Drawing;
using System.Windows.Forms;
using UI;

namespace ClinicaDentSystem
{
    public partial class Dashboard : Form
    {
        private const int DashboardDesignWidth = 1390;
        private const int SidebarDesignWidth = 235;
        private const int SidebarMinWidth = 185;
        private const int SidebarMaxWidth = 320;
        private readonly Login? _login;
        private bool _cerrandoSesion;

        public Dashboard() : this(null)
        {
        }

        public Dashboard(Login? login)
        {
            _login = login;
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = false;
            ResponsiveLayout.Configure(this);
            SizeChanged += Dashboard_SizeChanged;
            FormClosed += Dashboard_FormClosed;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            RebuildDashboardLayout();
            UpdateSidebarWidth();
            guna2Panel2.AutoScroll = true;
            guna2Panel3.AutoScroll = true;

            lblNombre.Text = Program.UsuarioActivo.NombreUsuario;
            lblRol.Text = Program.UsuarioActivo.NombreRol;

            AplicarPermisos();
            CargarVista(new UC_Inicio());
        }

        private void CargarVista(UserControl control)
        {
            guna2Panel3.Controls.Clear();
            control.Dock = DockStyle.Fill;
            guna2Panel3.Controls.Add(control);
        }

        private void CargarVistaSegura(string codigoModulo, Func<UserControl> crearControl)
        {
            if (!TieneAccesoModulo(codigoModulo))
            {
                return;
            }

            CargarVista(crearControl());
        }

        private bool TieneAccesoModulo(string codigoModulo)
        {
            if (codigoModulo == ModulosSistema.Inicio)
            {
                return true;
            }

            bool permitido = Program.UsuarioActivo?.TienePermiso(codigoModulo) == true;

            if (!permitido)
            {
                MessageBox.Show("No tiene permiso para acceder a este modulo.",
                                "Permisos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }

            return permitido;
        }

        private bool PuedeVerModulo(string codigoModulo)
        {
            return codigoModulo == ModulosSistema.Inicio || Program.UsuarioActivo?.TienePermiso(codigoModulo) == true;
        }

        private bool EsAdministrador()
        {
            return Program.UsuarioActivo?.EsAdministrador == true;
        }

        private void AplicarPermisos()
        {
            SetHabilitadoModulo(ModulosSistema.Inicio, guna2Button1, guna2ImageRadioButton4);
            SetHabilitadoModulo(ModulosSistema.Pacientes, guna2Button2, guna2ImageRadioButton5);
            SetHabilitadoModulo(ModulosSistema.Citas, guna2Button3, guna2ImageRadioButton6);
            SetHabilitadoModulo(ModulosSistema.Dentistas, guna2Button4, guna2ImageRadioButton9);
            SetHabilitadoModulo(ModulosSistema.Inventario, guna2Button5, guna2ImageRadioButton7);
            SetHabilitadoModulo(ModulosSistema.Facturacion, guna2Button6, guna2ImageRadioButton8);
            SetHabilitadoModulo(ModulosSistema.Historial, guna2Button7, guna2ImageRadioButton10);
            SetHabilitadoModulo(ModulosSistema.Auditorias, guna2Button8, guna2ImageRadioButton11);
            SetHabilitadoModulo(ModulosSistema.Usuarios, guna2ImageButton4);

            guna2ImageButton7.Enabled = EsAdministrador();
        }

        private void SetHabilitadoModulo(string codigoModulo, params Control[] controles)
        {
            bool habilitado = PuedeVerModulo(codigoModulo);

            foreach (Control control in controles)
            {
                control.Enabled = habilitado;
            }
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            CargarVista(new UC_Inicio());
        }

        private void btnPacientes_Click(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Pacientes, () => new UC_Pacientes1());
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Citas, () => new UC_Citas());
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Dentistas, () => new UC_Dentistas());
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Inventario, () => new UC_Inventario());
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Facturacion, () => new UC_Facturacion());
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Historial, () => new UC_Historial());
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Auditorias, () => new UC_Auditorias());
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Usuarios, () => new UC_Usuarios());
        }

        private void guna2ImageRadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            CerrarSesion();
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2ImageRadioButton3_CheckedChanged_1(object sender, EventArgs e)
        {
            CerrarSesion();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Pacientes, () => new UC_Pacientes1());
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Citas, () => new UC_Citas());
        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Pacientes, () => new UC_Pacientes1());
        }

        private void guna2Button3_Click_2(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Citas, () => new UC_Citas());
        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Dentistas, () => new UC_Dentistas());
        }

        private void guna2Button5_Click_1(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Inventario, () => new UC_Inventario());
        }

        private void guna2Button6_Click_1(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Facturacion, () => new UC_Facturacion());
        }

        private void guna2Button7_Click_1(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Historial, () => new UC_Historial());
        }

        private void guna2Button8_Click_1(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Auditorias, () => new UC_Auditorias());
        }

        private void guna2ImageButton4_Click_1(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Usuarios, () => new UC_Usuarios());
        }

        private void guna2ImageRadioButton3_CheckedChanged_2(object sender, EventArgs e)
        {
            CerrarSesion();
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            CargarVista(new UC_Inicio());
        }

        private void guna2ImageRadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            CargarVista(new UC_Inicio());
        }

        private void guna2ImageRadioButton5_CheckedChanged(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Pacientes, () => new UC_Pacientes1());
        }

        private void guna2ImageRadioButton6_CheckedChanged(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Citas, () => new UC_Citas());
        }

        private void guna2ImageRadioButton9_CheckedChanged(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Dentistas, () => new UC_Dentistas());
        }

        private void guna2ImageRadioButton7_CheckedChanged(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Inventario, () => new UC_Inventario());
        }

        private void guna2ImageRadioButton8_CheckedChanged(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Facturacion, () => new UC_Facturacion());
        }

        private void guna2ImageRadioButton10_CheckedChanged(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Historial, () => new UC_Historial());
        }

        private void guna2ImageRadioButton11_CheckedChanged(object sender, EventArgs e)
        {
            CargarVistaSegura(ModulosSistema.Auditorias, () => new UC_Auditorias());
        }

        private void lblNombre_Click(object sender, EventArgs e)
        {

        }

        private void CerrarSesion()
        {
            _cerrandoSesion = true;

            if (_login is not null)
            {
                _login.PrepararNuevaSesion();
                _login.Show();
            }
            else
            {
                Login login = new Login();
                login.Show();
            }

            Close();
        }

        private void Dashboard_FormClosed(object? sender, FormClosedEventArgs e)
        {
            if (!_cerrandoSesion)
            {
                Application.Exit();
            }
        }

        private void guna2ImageButton7_Click(object sender, EventArgs e)
        {
            if (!EsAdministrador())
            {
                MessageBox.Show("Solo el administrador puede acceder a configuracion.",
                                "Permisos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            CargarVista(new UC_Configuracion());
        }
    }
}
