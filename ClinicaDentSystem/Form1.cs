using DAO;
using Modelos;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ClinicaDentSystem
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (credencialesValidas())
            {
                Dashboard dashboard = new Dashboard(this);
                dashboard.Show();
                this.Hide();
            }

        }

        internal void PrepararNuevaSesion()
        {
            txt_pass.Text = string.Empty;
            txt_user.Focus();
        }
        private void linkRecuperarUsuario_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            using RecuperarUsuario recuperarUsuario = new RecuperarUsuario();
            recuperarUsuario.ShowDialog(this);
        }

        private bool credencialesValidas()
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();

            if (string.IsNullOrEmpty(txt_user.Text))
            {
                MessageBox.Show("Debe ingresar el usuario",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txt_pass.Text))
            {
                MessageBox.Show("Debe ingresar la clave",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }

            Usuario usuario = usuarioDAO.Login(txt_user.Text, txt_pass.Text, out string msjError);

            if (usuario == null || !string.IsNullOrEmpty(msjError))
            {
                MessageBox.Show("Credenciales no válidas", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            Program.UsuarioActivo = usuario;
            CargarPermisos(usuario);
            return true;
        }

        private void CargarPermisos(Usuario usuario)
        {
            PermisosDAO permisosDAO = new PermisosDAO();
            usuario.PermisosModulos = permisosDAO.ObtenerCodigosPermitidos(usuario.UsuarioId, out string msjError);

            if (usuario.EsAdministrador)
            {
                usuario.PermisosModulos = new HashSet<string>(ModulosSistema.Todos, StringComparer.OrdinalIgnoreCase);
                return;
            }

            if (!string.IsNullOrEmpty(msjError))
            {
                MessageBox.Show("No se pudieron cargar los permisos del usuario. Ejecute primero el script de permisos.",
                                "Permisos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }

}
