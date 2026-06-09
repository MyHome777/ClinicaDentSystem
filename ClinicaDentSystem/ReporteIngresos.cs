using DAO;
using System.Data;
using System.Globalization;

namespace UI
{
    public partial class ReporteIngresos : Form
    {
        private readonly ReporteIngresosDAO _dao = new ReporteIngresosDAO();
        private bool _cargando;

        public ReporteIngresos()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConfigurarEventos();
            CargarReporte();
        }

        private void ConfigurarFormulario()
        {
            StartPosition = FormStartPosition.CenterParent;

            _cargando = true;
            guna2ComboBox5.Items.Clear();
            guna2ComboBox5.Items.Add("Hoy");
            guna2ComboBox5.Items.Add("Este mes");
            guna2ComboBox5.Items.Add("Este año");
            guna2ComboBox5.Items.Add("Por fechas");
            guna2ComboBox5.SelectedIndex = 1;
            AplicarPeriodoSeleccionado();
            _cargando = false;

            ConfigurarGrid(dataGridView1, "Servicio");
            ConfigurarGrid(dataGridView2, "Producto");
        }

        private void ConfigurarEventos()
        {
            guna2ComboBox5.SelectedIndexChanged += (_, _) =>
            {
                AplicarPeriodoSeleccionado();
                CargarReporte();
            };
            guna2DateTimePicker2.ValueChanged += (_, _) => CargarReporte();
            guna2DateTimePicker1.ValueChanged += (_, _) => CargarReporte();
        }

        private static void ConfigurarGrid(DataGridView grid, string nombreColumna)
        {
            grid.AutoGenerateColumns = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.Columns.Clear();

            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = nombreColumna,
                DataPropertyName = "Nombre",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Cantidad",
                DataPropertyName = "Cantidad",
                Width = 100,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Total",
                DataPropertyName = "TotalGenerado",
                Width = 130,
                DefaultCellStyle =
                {
                    Format = "C2",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });
        }

        private void AplicarPeriodoSeleccionado()
        {
            string periodo = guna2ComboBox5.SelectedItem?.ToString() ?? "Este mes";
            bool esPersonalizado = periodo == "Por fechas";

            DateTime desde = DateTime.Today;
            DateTime hasta = DateTime.Today;

            if (periodo == "Este mes")
            {
                desde = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                hasta = desde.AddMonths(1).AddDays(-1);
            }
            else if (periodo == "Este año")
            {
                desde = new DateTime(DateTime.Today.Year, 1, 1);
                hasta = new DateTime(DateTime.Today.Year, 12, 31);
            }

            _cargando = true;
            guna2DateTimePicker2.Enabled = esPersonalizado;
            guna2DateTimePicker1.Enabled = esPersonalizado;

            if (!esPersonalizado)
            {
                guna2DateTimePicker2.Value = desde;
                guna2DateTimePicker1.Value = hasta;
            }

            _cargando = false;
        }

        private void CargarReporte()
        {
            if (_cargando)
                return;

            DateTime fechaDesde = guna2DateTimePicker2.Value.Date;
            DateTime fechaHasta = guna2DateTimePicker1.Value.Date;

            if (fechaHasta < fechaDesde)
            {
                MessageBox.Show("La fecha hasta no puede ser menor que la fecha desde.",
                                "Reporte de ingresos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            DataTable resumen = _dao.ObtenerResumen(fechaDesde, fechaHasta, null, out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MostrarError(error);
                return;
            }

            DataTable servicios = _dao.ObtenerServicios(fechaDesde, fechaHasta, null, out error);
            if (!string.IsNullOrEmpty(error))
            {
                MostrarError(error);
                return;
            }

            DataTable productos = _dao.ObtenerProductos(fechaDesde, fechaHasta, null, out error);
            if (!string.IsNullOrEmpty(error))
            {
                MostrarError(error);
                return;
            }

            PintarResumen(resumen);
            dataGridView1.DataSource = servicios;
            dataGridView2.DataSource = productos;
        }

        private void PintarResumen(DataTable resumen)
        {
            if (resumen.Rows.Count == 0)
            {
                label2.Text = FormatearMoneda(0);
                label5.Text = "0";
                label7.Text = "0";
                label10.Text = FormatearMoneda(0);
                return;
            }

            DataRow row = resumen.Rows[0];
            label2.Text = FormatearMoneda(ObtenerDecimal(row, "TotalIngresos"));
            label5.Text = ObtenerEntero(row, "FacturasReportadas").ToString(CultureInfo.InvariantCulture);
            label7.Text = ObtenerEntero(row, "DescuentosAplicados").ToString(CultureInfo.InvariantCulture);
            label10.Text = FormatearMoneda(ObtenerDecimal(row, "PromedioFactura"));
        }

        private static decimal ObtenerDecimal(DataRow row, string columna)
        {
            if (!row.Table.Columns.Contains(columna))
                return 0m;

            return row[columna] == DBNull.Value ? 0m : Convert.ToDecimal(row[columna], CultureInfo.InvariantCulture);
        }

        private static int ObtenerEntero(DataRow row, string columna)
        {
            if (!row.Table.Columns.Contains(columna))
                return 0;

            return row[columna] == DBNull.Value ? 0 : Convert.ToInt32(row[columna], CultureInfo.InvariantCulture);
        }

        private static string FormatearMoneda(decimal valor)
        {
            return "$" + valor.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private static void MostrarError(string error)
        {
            MessageBox.Show(error,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
