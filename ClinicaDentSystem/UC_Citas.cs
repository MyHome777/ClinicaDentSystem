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
    public partial class UC_Citas : UserControl
    {
        public UC_Citas()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
            CargarCitas();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            AbrirFormularioCita(false);
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            AbrirFormularioEditarCita();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            AbrirFormularioEditarCita();
        }

        private void guna2ImageButton5_Click(object sender, EventArgs e)
        {
            AbrirFormularioCita(false);
        }

        private CitasDAO _citasDAO = new CitasDAO();

        private void AbrirFormularioCita(bool esEdicion, int citaID = 0)
        {
            using Citas citas = new Citas(esEdicion, citaID);

            if (citas.ShowDialog(this) == DialogResult.OK)
            {
                CargarCitas();
            }
        }

        private void AbrirFormularioEditarCita()
        {
            Citass? citaSeleccionada = ObtenerCitaSeleccionada();

            if (citaSeleccionada is null)
            {
                return;
            }

            AbrirFormularioCita(true, citaSeleccionada.Numerodecita);
        }

        private Citass? ObtenerCitaSeleccionada()
        {
            if (dgv_Citas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una cita primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            if (dgv_Citas.SelectedRows[0].DataBoundItem is Citass cita)
            {
                return cita;
            }

            MessageBox.Show("No se pudo obtener la cita seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }

        private void CargarCitas()
        {
            dgv_Citas.AutoGenerateColumns = false;
            string pError = string.Empty;
            List<Citass> citas = _citasDAO.ObtenerTodos(out pError);

            if (!string.IsNullOrEmpty(pError))
            {
                MessageBox.Show(pError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgv_Citas.DataSource = citas;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string pError = string.Empty;

            if (string.IsNullOrEmpty(txt_Buscador.Text))
            {
                CargarCitas();
            }
            else
            {
                var resultado = _citasDAO.BuscarCitas(txt_Buscador.Text, out pError);

                if (!string.IsNullOrEmpty(pError))
                {
                    MessageBox.Show(pError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                dgv_Citas.DataSource = resultado;
            }
        }

        private void dtp_BuscarFCita_ValueChanged(object sender, EventArgs e)
        {
            string pError = string.Empty;
            string fecha = dtp_BuscarFCita.Value.ToString("dd/MM/yyyy");
            var resultado = _citasDAO.BuscarCitas(fecha, out pError);

            if (!string.IsNullOrEmpty(pError))
            {
                MessageBox.Show(pError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgv_Citas.DataSource = resultado;
        }

        private void guna2ImageButton6_Click(object sender, EventArgs e)
        {
            txt_Buscador.Text = string.Empty;
            dtp_BuscarFCita.Value = DateTime.Today;
            CargarCitas();
        }

        private void btn_CancelarCita_Click(object sender, EventArgs e)
        {
            Citass? citaSeleccionada = ObtenerCitaSeleccionada();

            if (citaSeleccionada is null)
            {
                return;
            }

            DialogResult confirmacion = MessageBox.Show("¿Estás seguro de cancelar esta cita?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                string pError = string.Empty;
                _citasDAO.CancelarCita(citaSeleccionada.Numerodecita, out pError);

                if (string.Equals(pError?.Trim(), "Cita cancelada", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(pError, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarCitas();
                }
                else
                {
                    MessageBox.Show(pError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            Citass? citaSeleccionada = ObtenerCitaSeleccionada();

            if (citaSeleccionada is null)
            {
                return;
            }

            DialogResult confirmacion = MessageBox.Show("¿Estás seguro de cancelar esta cita?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                string pError = string.Empty;
                _citasDAO.CancelarCita(citaSeleccionada.Numerodecita, out pError);

                if (string.Equals(pError?.Trim(), "Cita cancelada", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(pError, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarCitas();
                }
                else
                {
                    MessageBox.Show(pError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
