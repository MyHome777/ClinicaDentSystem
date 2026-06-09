using DAO;
using Guna.UI2.WinForms;
using MODELOS;
using System.Data;
using System.Globalization;

namespace ClinicaDentSystem
{
    public partial class Descuento : Form
    {
        private readonly DescuentosDAO _dao = new DescuentosDAO();
        private int _descuentoIdSeleccionado;
        private bool _actualizandoCombos;

        public Descuento()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConfigurarEventos();
            CargarDatos();
        }

        private void ConfigurarFormulario()
        {
            StartPosition = FormStartPosition.CenterParent;
            guna2DateTimePicker1.Value = DateTime.Today;
            guna2DateTimePicker2.Value = DateTime.Today.AddMonths(1);
            ConfigurarGrid();
        }

        private void ConfigurarEventos()
        {
            guna2Button2.Click += (sender, e) => GuardarOActualizar();
            guna2ImageButton5.Click += (sender, e) => GuardarOActualizar();
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            guna2ComboBox1.SelectedIndexChanged += guna2ComboBox1_SelectedIndexChanged;
            guna2ComboBox2.SelectedIndexChanged += guna2ComboBox2_SelectedIndexChanged;
        }

        private void ConfigurarGrid()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = nameof(Descuentos.DescuentoID),
                Width = 50
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Servicio",
                DataPropertyName = nameof(Descuentos.NombreServicio),
                Width = 140
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Producto",
                DataPropertyName = nameof(Descuentos.NombreProducto),
                Width = 140
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Inicio",
                DataPropertyName = nameof(Descuentos.FechaIn),
                DefaultCellStyle = { Format = "dd/MM/yyyy" },
                Width = 90
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Final",
                DataPropertyName = nameof(Descuentos.FechaFn),
                DefaultCellStyle = { Format = "dd/MM/yyyy" },
                Width = 90
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "%",
                DataPropertyName = nameof(Descuentos.Porcentaje),
                DefaultCellStyle = { Format = "N2" },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        private void CargarDatos()
        {
            CargarServicios();
            CargarProductos();
            CargarDescuentos();
            ActivarModoCreacion();
        }

        private void CargarServicios()
        {
            DataTable servicios = _dao.ObtenerServicios(out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MostrarError(error);
                return;
            }

            InsertarOpcionVacia(servicios, "ServicioID", "NombreServicio", "Seleccione servicio");

            guna2ComboBox1.DataSource = servicios;
            guna2ComboBox1.DisplayMember = "NombreServicio";
            guna2ComboBox1.ValueMember = "ServicioID";
            guna2ComboBox1.SelectedIndex = 0;
        }

        private void CargarProductos()
        {
            DataTable productos = _dao.ObtenerProductos(out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MostrarError(error);
                return;
            }

            InsertarOpcionVacia(productos, "ProductoID", "NombreProducto", "Seleccione producto");

            guna2ComboBox2.DataSource = productos;
            guna2ComboBox2.DisplayMember = "NombreProducto";
            guna2ComboBox2.ValueMember = "ProductoID";
            guna2ComboBox2.SelectedIndex = 0;
        }

        private void CargarDescuentos()
        {
            dataGridView1.DataSource = _dao.ObtenerTodos(out string error);
            if (!string.IsNullOrEmpty(error))
                MostrarError(error);
        }

        private void GuardarOActualizar()
        {
            if (!ValidarFormulario())
                return;

            Descuentos descuento = ObtenerDescuentoFormulario();
            string error;

            if (_descuentoIdSeleccionado > 0)
            {
                descuento.DescuentoID = _descuentoIdSeleccionado;
                _dao.ActualizarRegistro(descuento, out error);
            }
            else
            {
                _dao.GuardarRegistro(descuento, out error);
            }

            bool exito = error.Contains("correctamente", StringComparison.OrdinalIgnoreCase);
            MessageBox.Show(error,
                            exito ? "Descuentos" : "Aviso",
                            MessageBoxButtons.OK,
                            exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (exito)
            {
                CargarDescuentos();
                ActivarModoCreacion();
            }
        }

        private void dataGridView1_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is not Descuentos descuento)
            {
                MessageBox.Show("No se pudo obtener el descuento seleccionado.",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            _descuentoIdSeleccionado = descuento.DescuentoID;
            _actualizandoCombos = true;
            SeleccionarValorNullable(guna2ComboBox1, descuento.ServicioID);
            SeleccionarValorNullable(guna2ComboBox2, descuento.ProductoID);
            _actualizandoCombos = false;
            guna2DateTimePicker1.Value = descuento.FechaIn;
            guna2DateTimePicker2.Value = descuento.FechaFn;
            guna2TextBox2.Text = descuento.Porcentaje.ToString("0.##", CultureInfo.InvariantCulture);
            ActivarModoEdicion();
        }

        private void guna2ComboBox1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_actualizandoCombos)
                return;

            if (TryGetSelectedInt(guna2ComboBox1.SelectedValue, out _))
                LimpiarSeleccion(guna2ComboBox2);
        }

        private void guna2ComboBox2_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_actualizandoCombos)
                return;

            if (TryGetSelectedInt(guna2ComboBox2.SelectedValue, out _))
                LimpiarSeleccion(guna2ComboBox1);
        }

        private bool ValidarFormulario()
        {
            bool tieneServicio = TryGetSelectedInt(guna2ComboBox1.SelectedValue, out _);
            bool tieneProducto = TryGetSelectedInt(guna2ComboBox2.SelectedValue, out _);

            if (!tieneServicio && !tieneProducto)
            {
                MessageBox.Show("Debe seleccionar un servicio o un producto.",
                                "Descuentos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return false;
            }

            if (tieneServicio && tieneProducto)
            {
                MessageBox.Show("Seleccione solo un servicio o un producto, no ambos.",
                                "Descuentos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return false;
            }

            if (!TryParseDecimal(guna2TextBox2.Text, out decimal porcentaje))
            {
                MessageBox.Show("Ingrese un porcentaje valido.",
                                "Descuentos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return false;
            }

            if (porcentaje <= 0 || porcentaje > 100)
            {
                MessageBox.Show("El porcentaje debe ser mayor que 0 y menor o igual a 100.",
                                "Descuentos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return false;
            }

            if (guna2DateTimePicker2.Value.Date < guna2DateTimePicker1.Value.Date)
            {
                MessageBox.Show("La fecha final no puede ser menor que la fecha de inicio.",
                                "Descuentos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private Descuentos ObtenerDescuentoFormulario()
        {
            TryParseDecimal(guna2TextBox2.Text, out decimal porcentaje);
            bool tieneServicio = TryGetSelectedInt(guna2ComboBox1.SelectedValue, out int servicioId);
            bool tieneProducto = TryGetSelectedInt(guna2ComboBox2.SelectedValue, out int productoId);

            return new Descuentos
            {
                ServicioID = tieneServicio ? servicioId : null,
                ProductoID = tieneProducto ? productoId : null,
                FechaIn = guna2DateTimePicker1.Value.Date,
                FechaFn = guna2DateTimePicker2.Value.Date,
                Porcentaje = porcentaje
            };
        }

        private void ActivarModoCreacion()
        {
            _descuentoIdSeleccionado = 0;
            guna2Button2.Text = "AGREGAR";
            guna2Button2.TextAlign = HorizontalAlignment.Left;
            guna2ImageButton5.Visible = true;
            guna2TextBox2.Text = string.Empty;
            guna2DateTimePicker1.Value = DateTime.Today;
            guna2DateTimePicker2.Value = DateTime.Today.AddMonths(1);

            _actualizandoCombos = true;
            LimpiarSeleccion(guna2ComboBox1);
            LimpiarSeleccion(guna2ComboBox2);
            _actualizandoCombos = false;
        }

        private void ActivarModoEdicion()
        {
            guna2Button2.Text = "ACTUALIZAR";
            guna2Button2.TextAlign = HorizontalAlignment.Center;
            guna2ImageButton5.Visible = false;
        }

        private static bool TryGetSelectedInt(object? selectedValue, out int value)
        {
            value = 0;
            if (selectedValue == null || selectedValue == DBNull.Value || selectedValue is DataRowView)
                return false;

            return int.TryParse(selectedValue.ToString(), out value);
        }

        private static void InsertarOpcionVacia(DataTable tabla, string valueColumn, string displayColumn, string texto)
        {
            DataColumn? columnaValor = tabla.Columns[valueColumn];
            DataColumn? columnaTexto = tabla.Columns[displayColumn];

            if (columnaValor == null || columnaTexto == null)
                return;

            columnaValor.AllowDBNull = true;
            DataRow row = tabla.NewRow();
            row[columnaValor] = DBNull.Value;
            row[columnaTexto] = texto;
            tabla.Rows.InsertAt(row, 0);
        }

        private static void LimpiarSeleccion(Guna2ComboBox combo)
        {
            combo.SelectedIndex = combo.Items.Count > 0 ? 0 : -1;
        }

        private static void SeleccionarValorNullable(Guna2ComboBox combo, int? valor)
        {
            if (valor.HasValue)
            {
                combo.SelectedValue = valor.Value;
                return;
            }

            LimpiarSeleccion(combo);
        }

        private static bool TryParseDecimal(string texto, out decimal valor)
        {
            texto = texto.Trim().Replace(",", ".");
            return decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out valor);
        }

        private static void MostrarError(string error)
        {
            MessageBox.Show(error,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }

        private void Descuento_Load(object sender, EventArgs e)
        {

        }
    }
}
