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
    // EditInventario: dos funciones
    //   ① Consultar compras registradas con su detalle
    //   ② Registrar ajuste manual de stock (ENTRADA o SALIDA)
    public partial class EditInventario : Form
    {
        private readonly CompraDAO _compraDAO = new CompraDAO();
        private readonly DetalleCompraDAO _detDAO = new DetalleCompraDAO();
        private readonly MovimientoStockDAO _movDAO = new MovimientoStockDAO();

        private List<Compra> _listaCompras = new List<Compra>();
        private List<MovimientoStock> _listaMovimientos = new List<MovimientoStock>();

        public EditInventario()
        {
            InitializeComponent();
        }

        // ── LOAD ─────────────────────────────────────────────────────────────
        private void EditInventario_Load(object sender, EventArgs e)
        {
            CargarFiltroTipo();
            CargarCompras();
            CargarMovimientos();
            guna2DateTimePicker1.Value = DateTime.Now;
        }

        // ── FILTRO TIPO MOVIMIENTO (guna2ComboBox1) ───────────────────────────
        private void CargarFiltroTipo()
        {
            // guna2ComboBox1 = filtro historial de movimientos
            guna2ComboBox1.Items.Clear();
            guna2ComboBox1.Items.Add("TODOS");
            guna2ComboBox1.Items.Add("ENTRADA");
            guna2ComboBox1.Items.Add("SALIDA");
            guna2ComboBox1.SelectedIndex = 0;

            // guna2ComboBox3 = tipo para ajuste manual (Categoría reutilizado)
            guna2ComboBox3.Items.Clear();
            guna2ComboBox3.Items.Add("ENTRADA");
            guna2ComboBox3.Items.Add("SALIDA");
            guna2ComboBox3.SelectedIndex = 0;
        }

        // ── CARGAR COMPRAS ────────────────────────────────────────────────────
        private void CargarCompras()
        {
            // SP: spSelectCompras
            string pError;
            _listaCompras = _compraDAO.ObtenerTodos(out pError);

            if (!string.IsNullOrEmpty(pError) && pError.ToLower().Contains("error"))
                MessageBox.Show("Error al cargar compras: " + pError, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // ── CARGAR MOVIMIENTOS ────────────────────────────────────────────────
        private void CargarMovimientos()
        {
            // SP: spSelectMovimientos
            string pError;
            _listaMovimientos = _movDAO.ObtenerTodos(out pError);

            if (!string.IsNullOrEmpty(pError) && pError.ToLower().Contains("error"))
                MessageBox.Show("Error al cargar movimientos: " + pError, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // ── BUSCAR compra (guna2ImageButton6) ─────────────────────────────────
        private void guna2ImageButton6_Click(object sender, EventArgs e)
        {
            string busqueda = guna2TextBox2.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(busqueda))
            {
                CargarCompras();
                return;
            }

            // Filtrar lista en memoria
            List<Compra> filtradas = _listaCompras.FindAll(c =>
                c.CompraID.ToString().Contains(busqueda) ||
                c.NombreProveedor.ToUpper().Contains(busqueda));

            // Si hay un DataGridView para compras en el designer asignar aqui:
            // dgvCompras.DataSource = filtradas;
        }

        // ── FILTRAR por tipo de movimiento (guna2ComboBox1) ───────────────────
        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedItem == null) return;
            string filtro = guna2ComboBox1.SelectedItem.ToString();

            List<MovimientoStock> filtrados = filtro == "TODOS"
                ? _listaMovimientos
                : _listaMovimientos.FindAll(m => m.TipoMovimiento == filtro);

            // Si hay DataGridView para movimientos en el designer asignar aqui:
            // dgvMovimientos.DataSource = filtrados;
        }

        // ── GUARDAR AJUSTE MANUAL (guna2Button2) ──────────────────────────────
        // guna2TextBox2  = ID Producto  (label3)
        // guna2TextBox3  = Cantidad     (label4 = Stock Actual reutilizado)
        // guna2TextBox6  = Descripción  (label2)
        // guna2ComboBox3 = ENTRADA/SALIDA
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (!CamposAjusteValidos()) return;

            MovimientoStock mov = new MovimientoStock();
            mov.ProductoID = Convert.ToInt32(guna2TextBox2.Text);
            mov.TipoMovimiento = guna2ComboBox3.SelectedItem?.ToString() ?? "ENTRADA";
            mov.Cantidad = Convert.ToInt32(guna2TextBox3.Text);
            mov.Descripcion = guna2TextBox6.Text.Trim();
            mov.UsuarioID = Program.UsuarioActivo?.UsuarioId ?? Program.UsuarioActivo?.IdEmpleado ?? 0;
            if (mov.UsuarioID <= 0)
            {
                MessageBox.Show("No se pudo obtener el usuario activo. Vuelve a iniciar sesión.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // SP: spInsertMovimiento — valida stock si es SALIDA
            string pError;
            _movDAO.GuardarRegistro(mov, out pError);

            if (pError.ToLower().Contains("correctamente") || string.IsNullOrEmpty(pError))
            {
                MessageBox.Show("Ajuste registrado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarAjuste();
                CargarMovimientos();
            }
            else
            {
                MessageBox.Show(pError, "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ── LIMPIAR (guna2ImageButton2) ───────────────────────────────────────
        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            LimpiarAjuste();
        }

        private void LimpiarAjuste()
        {
            guna2TextBox1.Clear();
            guna2TextBox2.Clear();
            guna2TextBox3.Clear();
            guna2TextBox4.Clear();
            guna2TextBox5.Clear();
            guna2TextBox6.Clear();
            guna2TextBox7.Clear();
            guna2ComboBox3.SelectedIndex = 0;
            guna2DateTimePicker1.Value = DateTime.Now;
        }

        // ── VALIDACIONES ──────────────────────────────────────────────────────
        private bool CamposAjusteValidos()
        {
            if (string.IsNullOrWhiteSpace(guna2TextBox2.Text) || !int.TryParse(guna2TextBox2.Text, out _))
            { MessageBox.Show("Ingresa el ID del producto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            if (string.IsNullOrWhiteSpace(guna2TextBox3.Text) || !int.TryParse(guna2TextBox3.Text, out _))
            { MessageBox.Show("Ingresa una cantidad válida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            if (string.IsNullOrWhiteSpace(guna2TextBox6.Text))
            { MessageBox.Show("Ingresa una descripción del ajuste.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            return true;
        }
    }
}
