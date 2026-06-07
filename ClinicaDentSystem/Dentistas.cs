using DAO;
using MODELOS;
using System;
using System.Windows.Forms;

namespace ClinicaDentSystem
{
    public partial class Dentistas : Form
    {
        public int idDentistaEditar = 0;

        public Dentistas()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Dentista d = new Dentista();

                d.NumDocumento = txtNumDocumento.Text;
                d.Nombre = txtNombre.Text;
                d.Apellido = txtApellido.Text;
                d.Telefono = txtTelefono.Text;
                d.Email = txtEmail.Text;
                d.LicenciaMedica = txtLicenciaMedica.Text;
                d.TipoDocID = (cmbTipoDocumento.Text == "DUI") ? 1 : 2;
                d.EstadoID = (cmbEstado.Text == "Activo") ? 1 : 2;

                DentistaDAO dao = new DentistaDAO();

                if (idDentistaEditar > 0)
                {
                    d.DentistaID = idDentistaEditar;
                    dao.Actualizar(d);
                    MessageBox.Show("¡Dentista actualizado correctamente!");
                }
                else
                {
                    dao.Insertar(d);
                    MessageBox.Show("¡Dentista registrado correctamente!");
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }
    }
}