using DAO;
using MODELOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicaDentSystem
{
    public partial class FrmDialogcs : Form
    {
        public int idPacienteEditar = 0;
        public FrmDialogcs()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
        }

        private void FrmDialogcs_Load(object sender, EventArgs e)
        {
            cmbTipoDocumento.Items.Clear();
            cmbTipoDocumento.Items.Add("DUI");
            cmbTipoDocumento.Items.Add("Pasaporte");
            cmbTipoDocumento.SelectedIndex = 0;
            cmbEstado.Items.Clear();
            cmbEstado.Items.Add("Activo");
            cmbEstado.Items.Add("Inactivo");
            cmbEstado.SelectedIndex = 0;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Paciente p = new Paciente();
            p.NumDocumento = txtNumDocumento.Text;
            p.Nombre = txtNombre.Text;
            p.Apellido = txtApellido.Text;
            p.FechaNacimiento = dtpFechaNacimiento.Value;
            p.Telefono = txtTelefono.Text;
            p.Direccion = txtDireccion.Text;
            p.Email = txtEmail.Text;
            p.ContactoEmergencia = txtContactoEmergencia.Text;
            p.TelefonoEmergencia = txtTelefonoEmergencia.Text;
            p.Alergias = txtAlergias.Text;
            p.NotasMedicas = txtNotasMedicas.Text;
            p.TipoDocID = (cmbTipoDocumento.Text == "DUI") ? 1 : 2;
            p.EstadoID = (cmbEstado.Text == "Activo") ? 1 : 2;

            PacienteDAO dao = new PacienteDAO();

            if (idPacienteEditar == 0)
            {
                dao.Insertar(p);
                MessageBox.Show("Paciente registrado correctamente.");
            }
            else
            {
                p.PacienteID = idPacienteEditar;
                dao.Actualizar(p);
                MessageBox.Show("Paciente actualizado correctamente.");
            }

            this.Close();
        }
    }
}
