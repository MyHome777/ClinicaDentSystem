using DAO;
using MODELOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ClinicaDentSystem
{
    public partial class UC_Inventario : UserControl
    {
        private readonly ProductoDAO _productoDAO = new ProductoDAO();

        public UC_Inventario()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
            CargarDatos();
            CargarInventario();
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

        private Producto? ObtenerProductoSeleccionado()
        {
            if (dataGridView1.CurrentRow == null)
                return null;

            try
            {
                return new Producto
                {
                    ProductoID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ProductoID"].Value),
                    CategoriaID = dataGridView1.CurrentRow.Cells["CategoriaID"].Value != null ? Convert.ToInt32(dataGridView1.CurrentRow.Cells["CategoriaID"].Value) : 0,
                    Categoria = dataGridView1.CurrentRow.Cells["Categoria"].Value?.ToString() ?? string.Empty,
                    NombreProducto = dataGridView1.CurrentRow.Cells["NombreProducto"].Value?.ToString() ?? string.Empty,
                    Descripcion = dataGridView1.CurrentRow.Cells["Descripcion"].Value?.ToString() ?? string.Empty,
                    UnidadMedida = dataGridView1.CurrentRow.Cells["UnidadMedida"].Value?.ToString() ?? string.Empty,
                    StockActual = Convert.ToInt32(dataGridView1.CurrentRow.Cells["StockActual"].Value),
                    StockMinimo = Convert.ToInt32(dataGridView1.CurrentRow.Cells["StockMinimo"].Value),
                    PrecioUnitario = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["PrecioUnitario"].Value),
                    FechaVencimiento = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["FechaVencimiento"].Value),
                    EstadoID = dataGridView1.CurrentRow.Cells["EstadoID"].Value != null ? Convert.ToInt32(dataGridView1.CurrentRow.Cells["EstadoID"].Value) : 0,
                    Estado = dataGridView1.CurrentRow.Cells["Estado"].Value?.ToString() ?? string.Empty
                };
            }
            catch
            {
                return null;
            }
        }

        private void CargarInventario()
        {
            try
            {
                string pError;
                List<Producto> productos = _productoDAO.ObtenerTodos(out pError);

                if (!string.IsNullOrWhiteSpace(pError))
                {
                    MessageBox.Show("Error al cargar inventario: " + pError, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = productos;

                if (dataGridView1.Columns["ProductoID"] != null)
                    dataGridView1.Columns["ProductoID"].Visible = false;

                if (dataGridView1.Columns["CategoriaID"] != null)
                    dataGridView1.Columns["CategoriaID"].Visible = false;

                if (dataGridView1.Columns["EstadoID"] != null)
                    dataGridView1.Columns["EstadoID"].Visible = false;

                if (dataGridView1.Columns["CompraID"] != null)
                {
                    dataGridView1.Columns["CompraID"].Visible = true;
                    dataGridView1.Columns["CompraID"].DisplayIndex = 0;
                    dataGridView1.Columns["CompraID"].HeaderText = "Compra ID";
                }

                if (dataGridView1.Columns.Contains("NombreProducto"))
                    dataGridView1.Columns["NombreProducto"].HeaderText = "Nombre del Producto";

                if (dataGridView1.Columns.Contains("UnidadMedida"))
                    dataGridView1.Columns["UnidadMedida"].HeaderText = "Unidad";

                if (dataGridView1.Columns.Contains("PrecioUnitario"))
                    dataGridView1.Columns["PrecioUnitario"].HeaderText = "Precio Unitario";

                if (dataGridView1.Columns.Contains("FechaVencimiento"))
                    dataGridView1.Columns["FechaVencimiento"].HeaderText = "Vencimiento";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar inventario: " + ex.Message);
            }
        }

        private void BuscarInventario(string texto)
        {
            try
            {
                string pError;
                List<Producto> productos = _productoDAO.ObtenerTodos(out pError);

                if (!string.IsNullOrWhiteSpace(pError))
                {
                    MessageBox.Show("Error al cargar inventario: " + pError, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!string.IsNullOrWhiteSpace(texto))
                {
                    string criterio = texto.Trim().ToLowerInvariant();
                    productos = productos
                        .Where(p =>
                            p.ProductoID.ToString().Contains(criterio) ||
                            (p.CompraID > 0 && p.CompraID.ToString().Contains(criterio)) ||
                            (p.Categoria ?? string.Empty).ToLowerInvariant().Contains(criterio) ||
                            (p.NombreProducto ?? string.Empty).ToLowerInvariant().Contains(criterio) ||
                            (p.Descripcion ?? string.Empty).ToLowerInvariant().Contains(criterio) ||
                            (p.UnidadMedida ?? string.Empty).ToLowerInvariant().Contains(criterio) ||
                            (p.Estado ?? string.Empty).ToLowerInvariant().Contains(criterio)
                        )
                        .ToList();
                }

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = productos;

                if (dataGridView1.Columns["ProductoID"] != null)
                    dataGridView1.Columns["ProductoID"].Visible = false;

                if (dataGridView1.Columns["CategoriaID"] != null)
                    dataGridView1.Columns["CategoriaID"].Visible = false;

                if (dataGridView1.Columns["EstadoID"] != null)
                    dataGridView1.Columns["EstadoID"].Visible = false;

                if (dataGridView1.Columns["CompraID"] != null)
                {
                    dataGridView1.Columns["CompraID"].Visible = true;
                    dataGridView1.Columns["CompraID"].DisplayIndex = 0;
                    dataGridView1.Columns["CompraID"].HeaderText = "Compra ID";
                }

                if (dataGridView1.Columns.Contains("NombreProducto"))
                    dataGridView1.Columns["NombreProducto"].HeaderText = "Nombre del Producto";

                if (dataGridView1.Columns.Contains("UnidadMedida"))
                    dataGridView1.Columns["UnidadMedida"].HeaderText = "Unidad";

                if (dataGridView1.Columns.Contains("PrecioUnitario"))
                    dataGridView1.Columns["PrecioUnitario"].HeaderText = "Precio Unitario";

                if (dataGridView1.Columns.Contains("FechaVencimiento"))
                    dataGridView1.Columns["FechaVencimiento"].HeaderText = "Vencimiento";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar inventario: " + ex.Message);
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
                string nombre = dgvServicios.CurrentRow.Cells["NombreServicio"].Value?.ToString() ?? string.Empty;
                string desc = dgvServicios.CurrentRow.Cells["Descripcion"].Value?.ToString() ?? string.Empty;
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
            inventario.InventarioGuardado += (_, __) => CargarInventario();
            inventario.ShowDialog();
            CargarInventario();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            Producto? producto = ObtenerProductoSeleccionado();
            if (producto == null)
            {
                MessageBox.Show("Selecciona un producto del inventario para editarlo.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Inventario form = new Inventario();
            form.CargarDatosEdicion(producto);
            form.InventarioGuardado += (_, __) => CargarInventario();
            form.ShowDialog();
            CargarInventario();
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
                return;
            }

            DAO.ServiciosDAO dao = new DAO.ServiciosDAO();
            DataTable dt = dao.Buscar(texto);

            dgvServicios.DataSource = dt;

            if (dgvServicios.Columns["ServicioID"] != null) dgvServicios.Columns["ServicioID"].Visible = false;
            if (dgvServicios.Columns["EstadoID"] != null) dgvServicios.Columns["EstadoID"].Visible = false;
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

        private void guna2ImageButton6_Click(object sender, EventArgs e)
        {
            BuscarInventario(guna2TextBox1.Text);
        }

        private void guna2TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BuscarInventario(guna2TextBox1.Text);
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(guna2TextBox1.Text))
            {
                CargarInventario();
            }
        }

        private void guna2ImageButton5_Click(object sender, EventArgs e)
        {
            Categoria categoria = new Categoria();
            categoria.Show();
        }

        private void guna2ImageButton10_Click(object sender, EventArgs e)
        {
            Proveedor proveedor = new Proveedor();
            proveedor.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            Producto? producto = ObtenerProductoSeleccionado();
            if (producto != null)
            {
                DialogResult confirmacion = MessageBox.Show("¿Está seguro de eliminar esta compra?",
                                                            "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmacion == DialogResult.Yes)
                {
                    try
                    {
                        string pError;
                        _productoDAO.Eliminar(producto.ProductoID, out pError);

                        if (!string.IsNullOrWhiteSpace(pError) && !pError.ToLower().Contains("correctamente"))
                        {
                            MessageBox.Show(pError, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        CargarInventario();
                        MessageBox.Show("Producto eliminado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un producto para eliminar.");
            }
        }
    }
}
