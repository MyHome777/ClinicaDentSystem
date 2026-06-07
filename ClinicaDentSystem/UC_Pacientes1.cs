using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAO;

namespace ClinicaDentSystem
{
    public partial class UC_Pacientes1 : UserControl
    {
        public UC_Pacientes1()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
        }

        private void UC_Pacientes1_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }
        private void CargarDatos()
        {
            try
            {
                PacienteDAO oPacienteDAO = new PacienteDAO();
                dataGridView1.DataSource = oPacienteDAO.Listar();

                if (dataGridView1.Columns["PacienteID"] != null)
                    dataGridView1.Columns["PacienteID"].Visible = false;

                if (dataGridView1.Columns.Contains("TipoDocID"))
                    dataGridView1.Columns["TipoDocID"].Visible = false;

                if (dataGridView1.Columns.Contains("EstadoID"))
                    dataGridView1.Columns["EstadoID"].Visible = false;

                dataGridView1.Columns["TipoDoc"].HeaderText = "Tipo Doc.";
                dataGridView1.Columns["NumDocumento"].HeaderText = "No. Documento";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar: " + ex.Message);
            }
        }
        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            FrmDialogcs frm = new FrmDialogcs();
            frm.ShowDialog();
            CargarDatos();
        }
        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                FrmDialogcs frm = new FrmDialogcs();
                frm.idPacienteEditar = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["PacienteID"].Value);
                frm.txtNumDocumento.Text = dataGridView1.SelectedRows[0].Cells["NumDocumento"].Value.ToString();
                frm.txtNombre.Text = dataGridView1.SelectedRows[0].Cells["Nombre"].Value.ToString();
                frm.txtApellido.Text = dataGridView1.SelectedRows[0].Cells["Apellido"].Value.ToString();
                frm.dtpFechaNacimiento.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["FechaNacimiento"].Value);
                frm.txtTelefono.Text = dataGridView1.SelectedRows[0].Cells["Telefono"].Value.ToString();
                frm.txtDireccion.Text = dataGridView1.SelectedRows[0].Cells["Direccion"].Value.ToString();
                frm.txtEmail.Text = dataGridView1.SelectedRows[0].Cells["Email"].Value.ToString();
                frm.txtContactoEmergencia.Text = dataGridView1.SelectedRows[0].Cells["ContactoEmergencia"].Value.ToString();
                frm.txtTelefonoEmergencia.Text = dataGridView1.SelectedRows[0].Cells["TelefonoEmergencia"].Value.ToString();
                frm.txtAlergias.Text = dataGridView1.SelectedRows[0].Cells["Alergias"].Value.ToString();
                frm.txtNotasMedicas.Text = dataGridView1.SelectedRows[0].Cells["NotasMedicas"].Value.ToString();
                frm.cmbTipoDocumento.Text = dataGridView1.SelectedRows[0].Cells["TipoDoc"].Value.ToString();
                frm.cmbEstado.Text = dataGridView1.SelectedRows[0].Cells["Estado"].Value.ToString();

                frm.ShowDialog();
                CargarDatos();
            }
            else
            {
                MessageBox.Show("Seleccione un paciente de la lista.");
            }
        }
        private void guna2ImageButton6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource is DataTable dt)
            {
                string filtro = string.Format("Nombre LIKE '%{0}%' OR Apellido LIKE '%{0}%'", txtBuscar.Text);
                dt.DefaultView.RowFilter = filtro;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("¿Está seguro de eliminar este paciente?",
                                                      "Confirmar eliminación",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["PacienteID"].Value);
                        PacienteDAO oPacienteDAO = new PacienteDAO();
                        oPacienteDAO.Eliminar(id);
                        MessageBox.Show("Paciente eliminado correctamente.");
                        CargarDatos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un paciente de la lista.");
            }
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView1.DataSource is DataTable dt)
                {
                    string filtro = string.Format("Nombre LIKE '%{0}%' OR Apellido LIKE '%{0}%'", txtBuscar.Text);
                    dt.DefaultView.RowFilter = filtro;
                }
                e.SuppressKeyPress = true;
            }
        }
    }
}