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
    public partial class UC_Inventario : UserControl
    {
        public UC_Inventario()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                ServiciosDAO dao = new ServiciosDAO();
                DataTable dt = dao.Listar();

                dgvServicios.DataSource = dt;

                if (dgvServicios.Columns["ServicioID"] != null)
                    dgvServicios.Columns["ServicioID"].Visible = false;

                if (dgvServicios.Columns["EstadoID"] != null)
                    dgvServicios.Columns["EstadoID"].Visible = false;

                if (dgvServicios.Columns.Contains("NombreServicio"))
                    dgvServicios.Columns["NombreServicio"].HeaderText = "Nombre del Servicio";

                if (dgvServicios.Columns.Contains("Precio"))
                    dgvServicios.Columns["Precio"].HeaderText = "Precio ($)";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }
        private void guna2ImageButton7_Click(object sender, EventArgs e)
        {
            Servicio form = new Servicio();
            form.ShowDialog();
            CargarDatos();
        }

        private void guna2ImageButton8_Click(object sender, EventArgs e)
        {
            Servicio form = new Servicio();
            form.ShowDialog();
            CargarDatos();
        }
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvServicios.CurrentRow != null)
            {
                Servicio form = new Servicio();
                int id = Convert.ToInt32(dgvServicios.CurrentRow.Cells["ServicioID"].Value);
                string nombre = dgvServicios.CurrentRow.Cells["NombreServicio"].Value.ToString();
                string desc = dgvServicios.CurrentRow.Cells["Descripcion"].Value.ToString();
                decimal precio = Convert.ToDecimal(dgvServicios.CurrentRow.Cells["Precio"].Value);
                int estado = Convert.ToInt32(dgvServicios.CurrentRow.Cells["EstadoID"].Value);
                form.CargarDatosEdicion(id, nombre, desc, precio, estado);
                form.ShowDialog();
                CargarDatos();
            }
        }
        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            Inventario inventario = new Inventario();
            inventario.ShowDialog();
            CargarDatos();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            EditInventario editInventario = new EditInventario();
            editInventario.ShowDialog();
            CargarDatos();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Proveedor proveedor = new Proveedor();
            proveedor.ShowDialog();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Categoria categoria = new Categoria();
            categoria.ShowDialog();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvServicios.CurrentRow != null)
            {
                DialogResult confirmacion = MessageBox.Show("¿Está seguro de eliminar este servicio?",
                                                            "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmacion == DialogResult.Yes)
                {
                    try
                    {
                        int id = Convert.ToInt32(dgvServicios.CurrentRow.Cells["ServicioID"].Value);
                        DAO.ServiciosDAO dao = new DAO.ServiciosDAO();
                        dao.Eliminar(id);
                        CargarDatos();
                        MessageBox.Show("Servicio eliminado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fila para eliminar.");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string texto = txtBuscar.Text.Trim();

            if (string.IsNullOrEmpty(texto))
            {
                CargarDatos();
            }
            else
            {
                DAO.ServiciosDAO dao = new DAO.ServiciosDAO();
                DataTable dt = dao.Buscar(texto);

                dgvServicios.DataSource = dt;

                if (dgvServicios.Columns["ServicioID"] != null) dgvServicios.Columns["ServicioID"].Visible = false;
                if (dgvServicios.Columns["EstadoID"] != null) dgvServicios.Columns["EstadoID"].Visible = false;
            }
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnBuscar.PerformClick();
            }
        }

        private void txtBuscar_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnBuscar.PerformClick();
            }
        }
    }
}
