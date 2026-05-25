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
            string userIngresado = guna2TextBox1.Text;
            string passIngresado = guna2TextBox2.Text;

            if (DatosLogin(userIngresado, passIngresado))
            {
                Dashboard formDashboard = new Dashboard();
                formDashboard.Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos. Intente de nuevo.", "Error de Inicio de Sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public bool DatosLogin(string username, string password)
        {

            string usuarioCorrecto = "Cristian";
            string contraseñaCorrecta = "1234";

            if (username == usuarioCorrecto && password == contraseñaCorrecta)
            {
                return true;
            }
            else
            {

                return false;
            }
        }


    }

}
