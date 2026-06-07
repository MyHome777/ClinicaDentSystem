using DAO;
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
    public partial class UC_Dentistas : UserControl
    {
        public UC_Dentistas()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
        }

        private void UC_Dentistas_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            DentistaDAO dao = new DentistaDAO();
            dataGridView1.DataSource = dao.Listar();

            // 1. Ocultar las columnas técnicas que no deben verse
            if (dataGridView1.Columns["DentistaID"] != null) dataGridView1.Columns["DentistaID"].Visible = false;
            if (dataGridView1.Columns["TipoDocID"] != null) dataGridView1.Columns["TipoDocID"].Visible = false;
            if (dataGridView1.Columns["EstadoID"] != null) dataGridView1.Columns["EstadoID"].Visible = false;

            // 2. Opcional: Cambiar los nombres de los encabezados para que se vean bonitos
            dataGridView1.Columns["TipoDoc"].HeaderText = "Tipo Doc.";
            dataGridView1.Columns["NumDocumento"].HeaderText = "No. Documento";
            dataGridView1.Columns["LicenciaMedica"].HeaderText = "Licencia";
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            Dentistas dentistas = new Dentistas();
            dentistas.ShowDialog();

            CargarDatos();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Dentistas formEdicion = new Dentistas();

                formEdicion.txtNombre.Text = dataGridView1.SelectedRows[0].Cells["Nombre"].Value.ToString();
                formEdicion.txtApellido.Text = dataGridView1.SelectedRows[0].Cells["Apellido"].Value.ToString();
                formEdicion.txtNumDocumento.Text = dataGridView1.SelectedRows[0].Cells["NumDocumento"].Value.ToString();

                formEdicion.ShowDialog();

                CargarDatos();
            }
            else
            {
                MessageBox.Show("Selecciona un dentista para editar.");
            }
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int idSeleccionado = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["DentistaID"].Value);

                DialogResult result = MessageBox.Show("¿Estás seguro de eliminar este dentista?", "Confirmar", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    DentistaDAO dao = new DentistaDAO();
                    dao.Eliminar(idSeleccionado);

                    CargarDatos();
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila del dentista a eliminar.");
            }
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Dentistas formEdicion = new Dentistas();

                // Obtenemos los valores por NOMBRE de columna (más seguro que por índice)
                formEdicion.idDentistaEditar = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["DentistaID"].Value);
                formEdicion.txtNombre.Text = dataGridView1.SelectedRows[0].Cells["Nombre"].Value.ToString();
                formEdicion.txtApellido.Text = dataGridView1.SelectedRows[0].Cells["Apellido"].Value.ToString();
                formEdicion.txtNumDocumento.Text = dataGridView1.SelectedRows[0].Cells["NumDocumento"].Value.ToString();
                formEdicion.txtTelefono.Text = dataGridView1.SelectedRows[0].Cells["Telefono"].Value.ToString();
                formEdicion.txtEmail.Text = dataGridView1.SelectedRows[0].Cells["Email"].Value.ToString();
                formEdicion.txtLicenciaMedica.Text = dataGridView1.SelectedRows[0].Cells["LicenciaMedica"].Value.ToString();

                formEdicion.ShowDialog();
                CargarDatos();
            }
        }

        private void guna2ImageButton6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource is DataTable dt)
            {
                string textoBusqueda = txtBuscar.Text.Trim();

                if (string.IsNullOrEmpty(textoBusqueda))
                {
                    dt.DefaultView.RowFilter = string.Empty;
                }
                else
                {
                    string filtro = string.Format("Nombre LIKE '%{0}%' OR Apellido LIKE '%{0}%' OR NumDocumento LIKE '%{0}%'", textoBusqueda);

                    dt.DefaultView.RowFilter = filtro;
                }
            }
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                guna2ImageButton6_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
