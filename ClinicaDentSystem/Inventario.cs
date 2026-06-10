using DAO;
using Guna.UI2.WinForms;
using MODELOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicaDentSystem
{
    public partial class Inventario : Form
    {
        private readonly CategoriasDAO _catDAO = new CategoriasDAO();
        private readonly ProveedoresDAO _provDAO = new ProveedoresDAO();
        private readonly CompraDAO _compraDAO = new CompraDAO();
        private readonly DetalleCompraDAO _detDAO = new DetalleCompraDAO();
        private readonly ProductoDAO _productoDAO = new ProductoDAO();
        public event EventHandler? InventarioGuardado;
        private bool _modoEdicion;
        private int _productoIdActual;

        public Inventario()
        {
            InitializeComponent();
            guna2TextBox5.TextChanged += guna2TextBox5_TextChanged;
            guna2TextBox8.TextChanged += guna2TextBox8_TextChanged;
        }

        public void CargarDatosEdicion(Producto producto)
        {
            _modoEdicion = true;
            _productoIdActual = producto.ProductoID;

            guna2ComboBox3.SelectedValue = producto.CategoriaID;
            guna2TextBox6.Text = producto.NombreProducto;
            guna2TextBox1.Text = producto.Descripcion;
            guna2TextBox2.Text = producto.UnidadMedida;
            guna2TextBox3.Text = producto.StockActual.ToString();
            guna2TextBox4.Text = producto.StockMinimo.ToString();
            guna2TextBox5.Text = producto.PrecioUnitario.ToString("0.00", CultureInfo.CurrentCulture);
            guna2DateTimePicker1.Value = producto.FechaVencimiento;
            guna2ComboBox1.SelectedValue = producto.EstadoID;

            guna2Button2.Text = "  ACTUALIZAR";
        }

        // ── LOAD ─────────────────────────────────────────────────────────────
        private void Inventario_Load(object sender, EventArgs e)
        {
            CargarCategorias();
            CargarEstado();
            CargarProveedores();
            CargarUsuario();

            guna2DateTimePicker1.Value = DateTime.Now; // Vencimiento
            guna2DateTimePicker2.Value = DateTime.Now; // Fech. Compra

            guna2TextBox7.ReadOnly = true; // Total Compra — lo confirma la BD
            guna2TextBox9.ReadOnly = true; // PRECIO TOTAL — estimado visual
            ActualizarTotalesVisuales();
        }

        // ── CARGAR COMBOS ─────────────────────────────────────────────────────

        private void CargarCategorias()
        {
            string pError;
            List<Categorias> lista = _catDAO.ObtenerTodos(out pError);

            if (!string.IsNullOrEmpty(pError))
            {
                MessageBox.Show("Error al cargar categorías: " + pError, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            guna2ComboBox3.DisplayMember = "Nombre";       // 1. Primero defines el texto visual
            guna2ComboBox3.ValueMember = "CategoriaID";    // 2. Segundo defines el ID interno
            guna2ComboBox3.DataSource = lista;             // 3. Al final asignas los datos

            guna2ComboBox3.SelectedIndex = -1; // Deja el combo limpio al iniciar
        }

        private void CargarEstado()
        {
            string pError;
            DataTable estados = _provDAO.ObtenerEstados(out pError);

            if (estados != null && estados.Rows.Count > 0)
            {
                guna2ComboBox1.DataSource = estados;
                guna2ComboBox1.DisplayMember = "Estado";
                guna2ComboBox1.ValueMember = "EstadoID";
                guna2ComboBox1.SelectedIndex = 0;
            }
            else
            {
                DataTable tablaEstados = new DataTable();
                tablaEstados.Columns.Add("EstadoID", typeof(int));
                tablaEstados.Columns.Add("Estado", typeof(string));
                tablaEstados.Rows.Add(1, "ACTIVO");
                tablaEstados.Rows.Add(2, "INACTIVO");
                guna2ComboBox1.DataSource = tablaEstados;
                guna2ComboBox1.DisplayMember = "Estado";
                guna2ComboBox1.ValueMember = "EstadoID";
                guna2ComboBox1.SelectedIndex = 0;
            }
        }

        private void CargarProveedores()
        {
            string pError;
            List<Proveedores> lista = _provDAO.ObtenerTodos(out pError);

            if (!string.IsNullOrEmpty(pError))
            {
                MessageBox.Show("Error al cargar proveedores: " + pError, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            guna2ComboBox2.DataSource = lista;
            guna2ComboBox2.DisplayMember = "NombreProveedor";
            guna2ComboBox2.ValueMember = "ProveedorID";
            guna2ComboBox2.SelectedIndex = -1;
        }

        private void CargarUsuario()
        {
            if (Program.UsuarioActivo != null)
            {
                guna2ComboBox4.Items.Clear();
                guna2ComboBox4.Items.Add(Program.UsuarioActivo.NombreUsuario);
                guna2ComboBox4.SelectedIndex = 0;
                guna2ComboBox4.Enabled = false;
            }
        }

        // ── CÁLCULO VISUAL (estimado) ─────────────────────────────────────────
        private void guna2TextBox8_TextChanged(object? sender, EventArgs e) // Cantidad
        {
            ActualizarTotalesVisuales();
        }

        private void guna2TextBox5_TextChanged(object? sender, EventArgs e) // Precio Unitario
        {
            ActualizarTotalesVisuales();
        }

        private void ActualizarTotalesVisuales()
        {
            decimal precio = 0m;
            int cantidad = 0;

            decimal.TryParse(guna2TextBox5.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out precio);
            if (precio == 0m)
            {
                decimal.TryParse(guna2TextBox5.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out precio);
            }

            int.TryParse(guna2TextBox8.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out cantidad);

            decimal total = precio * cantidad;
            guna2TextBox9.Text = "$" + total.ToString("F2", CultureInfo.CurrentCulture);
            guna2TextBox7.Text = total.ToString("F2", CultureInfo.CurrentCulture);
        }

        // ── BOTÓN GUARDAR (guna2Button2) ──────────────────────────────────────
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (!CamposValidos()) return;

            int categoriaID = Convert.ToInt32(guna2ComboBox3.SelectedValue);
            string nombreProducto = guna2TextBox6.Text.Trim();
            string descripcion = guna2TextBox1.Text.Trim();
            string unidadMedida = guna2TextBox2.Text.Trim();
            int stockActual = 0;
            int stockMinimo = 0;
            decimal precioUnitario = 0m;
            int.TryParse(guna2TextBox3.Text, out stockActual);
            int.TryParse(guna2TextBox4.Text, out stockMinimo);
            if (!decimal.TryParse(guna2TextBox5.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out precioUnitario) &&
                !decimal.TryParse(guna2TextBox5.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out precioUnitario))
            {
                MessageBox.Show("Ingresa un precio unitario válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DateTime vencimiento = guna2DateTimePicker1.Value;
            int estadoID = Convert.ToInt32(guna2ComboBox1.SelectedValue);
            int proveedorID = Convert.ToInt32(guna2ComboBox2.SelectedValue);
            int usuarioID = Program.UsuarioActivo?.UsuarioId ?? Program.UsuarioActivo?.IdEmpleado ?? 0;
            if (usuarioID <= 0)
            {
                MessageBox.Show("No se pudo obtener el usuario activo. Vuelve a iniciar sesión.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int cantidad = 0;
            int.TryParse(guna2TextBox8.Text, out cantidad);
            decimal totalCompra = precioUnitario * cantidad;

            string pError;

            if (_modoEdicion)
            {
                Producto productoEditado = new Producto
                {
                    ProductoID = _productoIdActual,
                    CategoriaID = categoriaID,
                    NombreProducto = nombreProducto,
                    Descripcion = descripcion,
                    UnidadMedida = unidadMedida,
                    StockActual = stockActual,
                    StockMinimo = stockMinimo,
                    PrecioUnitario = precioUnitario,
                    FechaVencimiento = vencimiento,
                    EstadoID = estadoID
                };

                _productoDAO.ActualizarRegistro(productoEditado, out pError);

                if (!string.IsNullOrWhiteSpace(pError) && !pError.ToLower().Contains("correctamente"))
                {
                    MessageBox.Show("Error al actualizar producto: " + pError, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Producto actualizado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                InventarioGuardado?.Invoke(this, EventArgs.Empty);
                LimpiarFormulario();
                _modoEdicion = false;
                _productoIdActual = 0;
                guna2Button2.Text = "  GUARDAR";
                return;
            }

            // ── 1. Insertar Producto ──────────────────────────────────────────
            Producto producto = new Producto();
            producto.CategoriaID = categoriaID;
            producto.NombreProducto = nombreProducto;
            producto.Descripcion = descripcion;
            producto.UnidadMedida = unidadMedida;
            producto.StockActual = stockActual;
            producto.StockMinimo = stockMinimo;
            producto.PrecioUnitario = precioUnitario;
            producto.FechaVencimiento = vencimiento;
            producto.EstadoID = estadoID;

            int productoID = _productoDAO.GuardarRegistroConId(producto, out pError);

            if (productoID <= 0)
            {
                MessageBox.Show("Error al crear producto: " + pError, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ── 2. Insertar encabezado Compra ──────────────────────────────────
            Compra compra = new Compra();
            compra.ProveedorID = proveedorID;
            compra.UsuarioID = usuarioID;
            compra.FechaCompra = guna2DateTimePicker2.Value;
            compra.TotalCompra = totalCompra;

            int compraID = _compraDAO.GuardarRegistroConId(compra, out pError);

            if (compraID <= 0)
            {
                MessageBox.Show("Error al registrar compra: " + pError, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ── 3. Insertar Detalle ───────────────────────────────────────────
            DetalleCompra detalle = new DetalleCompra();
            detalle.CompraID = compraID;
            detalle.ProductoID = productoID;
            detalle.Cantidad = cantidad;
            detalle.PrecioUnitarioCompra = precioUnitario;

            _detDAO.GuardarRegistro(detalle, out pError);

            if (!string.IsNullOrEmpty(pError) && pError.ToLower().Contains("error"))
            {
                MessageBox.Show("Error en detalle: " + pError, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ── 4. Consultar total real calculado por la BD ───────────────────
            decimal totalReal = _compraDAO.ObtenerTotalReal(compraID);
            guna2TextBox7.Text = totalReal.ToString("F2");
            guna2TextBox9.Text = "$" + totalReal.ToString("F2");

            MessageBox.Show(
                $"Compra #{compraID} registrada correctamente.\nTotal confirmado por BD: ${totalReal:F2}",
                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            InventarioGuardado?.Invoke(this, EventArgs.Empty);

            LimpiarFormulario();
        }

        // ── LIMPIAR (guna2ImageButton2) ───────────────────────────────────────
        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        // ── VALIDACIONES ──────────────────────────────────────────────────────
        private bool CamposValidos()
        {
            if (string.IsNullOrWhiteSpace(guna2TextBox6.Text))
            { MessageBox.Show("Ingresa el nombre del producto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            if (guna2ComboBox3.SelectedValue == null)
            { MessageBox.Show("Selecciona una categoría.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            if (string.IsNullOrWhiteSpace(guna2TextBox2.Text))
            { MessageBox.Show("Ingresa la unidad de medida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            if (string.IsNullOrWhiteSpace(guna2TextBox8.Text) || !int.TryParse(guna2TextBox8.Text, out _))
            { MessageBox.Show("Ingresa una cantidad válida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            if (string.IsNullOrWhiteSpace(guna2TextBox5.Text) ||
                (!decimal.TryParse(guna2TextBox5.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out _) &&
                 !decimal.TryParse(guna2TextBox5.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out _)))
            { MessageBox.Show("Ingresa un precio unitario válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            if (_modoEdicion)
            {
                return true;
            }

            if (guna2ComboBox2.SelectedValue == null)
            { MessageBox.Show("Selecciona un proveedor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            return true;
        }

        // ── LIMPIAR FORMULARIO ────────────────────────────────────────────────
        private void LimpiarFormulario()
        {
            guna2TextBox6.Clear(); // Nombre Producto
            guna2TextBox1.Clear(); // Descripción
            guna2TextBox2.Clear(); // Unidad Medida
            guna2TextBox3.Clear(); // Stock Actual
            guna2TextBox4.Clear(); // Stock Mínimo
            guna2TextBox5.Clear(); // Precio Unitario
            guna2TextBox8.Clear(); // Cantidad
            guna2TextBox7.Clear(); // Total Compra
            guna2TextBox9.Text = "$0.00";
            guna2ComboBox3.SelectedIndex = -1;
            guna2ComboBox2.SelectedIndex = -1;
            guna2ComboBox1.SelectedIndex = 0;
            guna2DateTimePicker1.Value = DateTime.Now;
            guna2DateTimePicker2.Value = DateTime.Now;
            _modoEdicion = false;
            _productoIdActual = 0;
            guna2Button2.Text = "  GUARDAR";
        }

        private void guna2TextBox9_TextChanged(object? sender, EventArgs e)
        {

        }

        private void guna2ImageButton2_Click_1(object? sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void guna2Button2_Click_1(object? sender, EventArgs e)
        {
            guna2Button2_Click(this, e);
        }

        private void guna2ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
