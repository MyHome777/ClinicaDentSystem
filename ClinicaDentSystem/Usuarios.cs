using DAO;
using Modelos;
using System;
using System.Data;
using System.Net.Mail;
using System.Windows.Forms;

namespace ClinicaDentSystem
{
    public partial class Usuarios : Form
    {
        private readonly UsuarioDAO usuarioDAO = new UsuarioDAO();

        public event EventHandler? UsuarioGuardado;
        public int UsuarioID = 0;

        public Usuarios()
        {
            InitializeComponent();
        }

        private void Usuarios_Load(object sender, EventArgs e)
        {
            CargarComboRoles();
            CargarComboEstados();

            if (UsuarioID > 0)
            {
                CargarDatosParaEditar();
            }
        }

        private void CargarComboRoles()
        {
            DataTable roles = usuarioDAO.ObtenerRoles(out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show("Error roles: " + error, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cmbRol.DataSource = roles;
            cmbRol.DisplayMember = "NombreRol";
            cmbRol.ValueMember = "RolID";
        }

        private void CargarComboEstados()
        {
            DataTable estados = usuarioDAO.ObtenerEstados(out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show("Error estados: " + error, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cmbEstado.DataSource = estados;
            cmbEstado.DisplayMember = "Estado";
            cmbEstado.ValueMember = "EstadoId";
            SeleccionarEstadoActivo();
        }

        private void SeleccionarEstadoActivo()
        {
            foreach (DataRowView item in cmbEstado.Items)
            {
                string estado = item["Estado"]?.ToString() ?? string.Empty;
                if (string.Equals(estado, "ACTIVO", StringComparison.OrdinalIgnoreCase))
                {
                    cmbEstado.SelectedValue = item["EstadoId"];
                    return;
                }
            }
        }

        private void CargarDatosParaEditar()
        {
            Usuario usuario = usuarioDAO.ObtenerPorId(UsuarioID, out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show("Error al cargar datos: " + error, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            txtNombreUsuario.Text = usuario.NombreUsuario;
            txtNombreEmpleado.Text = usuario.NombreEmpleado;
            txtClave.Text = usuario.Clave;
            txtEmail.Text = usuario.Email;
            cmbRol.SelectedValue = usuario.RolID;
            cmbEstado.SelectedValue = usuario.EstadoID;
        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            if (!ValidarFormulario())
            {
                return;
            }

            Usuario usuario = CrearUsuarioDesdeFormulario();

            if (UsuarioID == 0)
            {
                usuarioDAO.GuardarRegistro(usuario, out string errorGuardar);
                if (!string.IsNullOrEmpty(errorGuardar))
                {
                    MessageBox.Show("Error: " + errorGuardar, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                usuarioDAO.ActualizarRegistro(usuario, out string errorActualizar);
                if (!string.IsNullOrEmpty(errorActualizar))
                {
                    MessageBox.Show("Error: " + errorActualizar, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            MessageBox.Show("Guardado con exito.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UsuarioGuardado?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private Usuario CrearUsuarioDesdeFormulario()
        {
            return new Usuario
            {
                UsuarioId = UsuarioID,
                NombreUsuario = txtNombreUsuario.Text.Trim(),
                NombreEmpleado = txtNombreEmpleado.Text.Trim(),
                Clave = txtClave.Text.Trim(),
                RolID = Convert.ToInt32(cmbRol.SelectedValue),
                EstadoID = Convert.ToInt32(cmbEstado.SelectedValue),
                Email = txtEmail.Text.Trim(),
                Estado = cmbEstado.Text.Trim()
            };
        }

        private bool ValidarFormulario()
        {
            txtNombreUsuario.Text = txtNombreUsuario.Text.Trim();
            txtNombreEmpleado.Text = txtNombreEmpleado.Text.Trim();
            txtClave.Text = txtClave.Text.Trim();
            txtEmail.Text = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text))
            {
                return MostrarValidacion("Ingresa el nombre de usuario.", txtNombreUsuario);
            }

            if (txtNombreUsuario.Text.Length > 50)
            {
                return MostrarValidacion("El nombre de usuario no puede superar 50 caracteres.", txtNombreUsuario);
            }

            if (string.IsNullOrWhiteSpace(txtNombreEmpleado.Text))
            {
                return MostrarValidacion("Ingresa el nombre del empleado.", txtNombreEmpleado);
            }

            if (txtNombreEmpleado.Text.Length > 60)
            {
                return MostrarValidacion("El nombre del empleado no puede superar 60 caracteres.", txtNombreEmpleado);
            }

            if (string.IsNullOrWhiteSpace(txtClave.Text))
            {
                return MostrarValidacion("Ingresa la clave.", txtClave);
            }

            if (txtClave.Text.Length > 25)
            {
                return MostrarValidacion("La clave no puede superar 25 caracteres.", txtClave);
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                return MostrarValidacion("Ingresa el correo del usuario.", txtEmail);
            }

            if (txtEmail.Text.Length > 100)
            {
                return MostrarValidacion("El correo no puede superar 100 caracteres.", txtEmail);
            }

            if (!EsCorreoValido(txtEmail.Text))
            {
                return MostrarValidacion("Ingresa un correo valido.", txtEmail);
            }

            if (cmbRol.SelectedValue == null)
            {
                MessageBox.Show("Selecciona un rol.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbRol.Focus();
                return false;
            }

            if (cmbEstado.SelectedValue == null)
            {
                MessageBox.Show("Selecciona un estado.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbEstado.Focus();
                return false;
            }

            return true;
        }

        private static bool EsCorreoValido(string correo)
        {
            try
            {
                MailAddress mail = new MailAddress(correo);
                return string.Equals(mail.Address, correo, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private static bool MostrarValidacion(string mensaje, Control control)
        {
            MessageBox.Show(mensaje, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
            return false;
        }
    }
}
