using DAO;
using MODELOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace ClinicaDentSystem
{
    public partial class EditInventario : Form
    {
        private readonly CategoriasDAO _catDAO = new CategoriasDAO();
        private readonly ProveedoresDAO _provDAO = new ProveedoresDAO();
        private readonly ProductoDAO _productoDAO = new ProductoDAO();

        private Producto? _productoEdicion;
        private int _productoIdActual;

        public event EventHandler? InventarioGuardado;

        public EditInventario()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
            guna2Button2.Click += guna2Button2_Click;
            guna2ImageButton2.Click += guna2ImageButton2_Click;
            
        }

        public void CargarDatosEdicion(Producto producto)
        {
            _productoEdicion = producto;
            _productoIdActual = producto.ProductoID;

            if (IsHandleCreated)
            {
                CargarProductoEnFormulario();
            }
        }

        private void EditInventario_Load(object sender, EventArgs e)
        {
            CargarCategorias();
            CargarEstado();

            guna2TextBox2.ReadOnly = true;
            guna2Button2.Text = "  ACTUALIZAR";

            if (_productoEdicion != null)
            {
                CargarProductoEnFormulario();
            }
        }

        private void CargarCategorias()
        {
            List<Categorias> categorias = _catDAO.ObtenerTodos(out string pError);

            if (!string.IsNullOrWhiteSpace(pError))
            {
                MessageBox.Show("Error al cargar categorias: " + pError, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            guna2ComboBox3.DisplayMember = "Nombre";
            guna2ComboBox3.ValueMember = "CategoriaID";
            guna2ComboBox3.DataSource = categorias;
            guna2ComboBox3.SelectedIndex = -1;
        }

        private void CargarEstado()
        {
            DataTable estados = _provDAO.ObtenerEstados(out string pError);

            if (!string.IsNullOrWhiteSpace(pError))
            {
                MessageBox.Show("Error al cargar estados: " + pError, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            guna2ComboBox1.DisplayMember = "Estado";
            guna2ComboBox1.ValueMember = "EstadoID";
            guna2ComboBox1.DataSource = estados;
            guna2ComboBox1.SelectedIndex = estados.Rows.Count > 0 ? 0 : -1;
        }

        private void CargarProductoEnFormulario()
        {
            if (_productoEdicion == null)
            {
                return;
            }

            guna2TextBox2.Text = _productoEdicion.ProductoID.ToString();
            guna2TextBox7.Text = _productoEdicion.NombreProducto;
            guna2TextBox6.Text = _productoEdicion.Descripcion;
            guna2TextBox1.Text = _productoEdicion.UnidadMedida;
            guna2TextBox3.Text = _productoEdicion.StockActual.ToString();
            guna2TextBox4.Text = _productoEdicion.StockMinimo.ToString();
            guna2TextBox5.Text = _productoEdicion.PrecioUnitario.ToString("0.00", CultureInfo.CurrentCulture);
            guna2DateTimePicker1.Value = _productoEdicion.FechaVencimiento;

            if (guna2ComboBox3.DataSource != null)
            {
                guna2ComboBox3.SelectedValue = _productoEdicion.CategoriaID;
            }

            if (guna2ComboBox1.DataSource != null)
            {
                guna2ComboBox1.SelectedValue = _productoEdicion.EstadoID;
            }
        }

        private void guna2Button2_Click(object? sender, EventArgs e)
        {
            if (!CamposValidos())
            {
                return;
            }

            Producto producto = new Producto
            {
                ProductoID = _productoIdActual,
                CategoriaID = Convert.ToInt32(guna2ComboBox3.SelectedValue),
                NombreProducto = guna2TextBox7.Text.Trim(),
                Descripcion = guna2TextBox6.Text.Trim(),
                UnidadMedida = guna2TextBox1.Text.Trim(),
                StockActual = Convert.ToInt32(guna2TextBox3.Text),
                StockMinimo = Convert.ToInt32(guna2TextBox4.Text),
                PrecioUnitario = LeerDecimal(guna2TextBox5.Text),
                FechaVencimiento = guna2DateTimePicker1.Value,
                EstadoID = Convert.ToInt32(guna2ComboBox1.SelectedValue)
            };

            _productoDAO.ActualizarRegistro(producto, out string pError);

            if (!string.IsNullOrWhiteSpace(pError) && !pError.ToLowerInvariant().Contains("correctamente"))
            {
                MessageBox.Show("Error al actualizar producto: " + pError, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Producto actualizado correctamente.", "Exito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            InventarioGuardado?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void guna2ImageButton2_Click(object? sender, EventArgs e)
        {
            CargarProductoEnFormulario();
        }

        private void guna2ImageButton6_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(guna2TextBox2.Text, out int productoID))
            {
                MessageBox.Show("Ingresa un ID de producto valido.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<Producto> productos = _productoDAO.ObtenerTodos(out string pError);
            if (!string.IsNullOrWhiteSpace(pError))
            {
                MessageBox.Show("Error al buscar producto: " + pError, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Producto? producto = productos.FirstOrDefault(p => p.ProductoID == productoID);
            if (producto == null)
            {
                MessageBox.Show("No se encontro el producto indicado.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _productoEdicion = producto;
            _productoIdActual = producto.ProductoID;
            CargarProductoEnFormulario();
        }

        private bool CamposValidos()
        {
            if (_productoIdActual <= 0)
            {
                MessageBox.Show("Selecciona un producto valido para actualizar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (guna2ComboBox3.SelectedValue == null)
            {
                MessageBox.Show("Selecciona una categoria.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(guna2TextBox7.Text))
            {
                MessageBox.Show("Ingresa el nombre del producto.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(guna2TextBox1.Text))
            {
                MessageBox.Show("Ingresa la unidad de medida.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(guna2TextBox3.Text, out _))
            {
                MessageBox.Show("Ingresa un stock actual valido.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(guna2TextBox4.Text, out _))
            {
                MessageBox.Show("Ingresa un stock minimo valido.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!DecimalValido(guna2TextBox5.Text))
            {
                MessageBox.Show("Ingresa un precio unitario valido.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (guna2ComboBox1.SelectedValue == null)
            {
                MessageBox.Show("Selecciona un estado.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private static bool DecimalValido(string texto)
        {
            return decimal.TryParse(texto, NumberStyles.Any, CultureInfo.CurrentCulture, out _)
                || decimal.TryParse(texto, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        }

        private static decimal LeerDecimal(string texto)
        {
            if (decimal.TryParse(texto, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal valor))
            {
                return valor;
            }

            decimal.TryParse(texto, NumberStyles.Any, CultureInfo.InvariantCulture, out valor);
            return valor;
        }
    }
}
