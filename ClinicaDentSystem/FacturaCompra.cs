using DAO;
using System.Data;
using System.Globalization;
using System.Text;

namespace ClinicaDentSystem
{
    public partial class FacturaCompra : Form
    {
        private readonly FacturaCompraDAO _facturaCompraDAO = new FacturaCompraDAO();
        private DataTable _facturas = new DataTable();
        private bool _cargando;

        public FacturaCompra()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConfigurarEventos();
            CargarFacturas();
        }

        private void ConfigurarFormulario()
        {
            StartPosition = FormStartPosition.CenterParent;
            guna2DateTimePicker1.Value = DateTime.Today.AddMonths(-1);
            guna2DateTimePicker2.Value = DateTime.Today;
            ConfigurarGridFacturas();
        }

        private void ConfigurarEventos()
        {
            guna2Button1.Click += (_, __) => VerDetalle();
            guna2Button2.Click += (_, __) => ExportarFacturas();
            guna2Button3.Click += (_, __) => ImprimirFactura();
            guna2TextBox1.TextChanged += (_, __) => CargarFacturasSiListo();
            guna2DateTimePicker1.ValueChanged += (_, __) => CargarFacturasSiListo();
            guna2DateTimePicker2.ValueChanged += (_, __) => CargarFacturasSiListo();
            dataGridView1.CellDoubleClick += (_, __) => VerDetalle();
        }

        private void ConfigurarGridFacturas()
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
                HeaderText = "Factura",
                DataPropertyName = "FacturaID",
                Width = 75
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Fecha",
                DataPropertyName = "FechaEmision",
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" },
                Width = 125
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Proveedor",
                DataPropertyName = "Proveedor",
                Width = 220
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Usuario",
                DataPropertyName = "UsuarioRegistro",
                Width = 140
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Total",
                DataPropertyName = "Total",
                DefaultCellStyle = { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight },
                Width = 110
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Estado",
                DataPropertyName = "Estado",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        private void CargarFacturasSiListo()
        {
            if (!_cargando)
                CargarFacturas();
        }

        private void CargarFacturas()
        {
            _cargando = true;

            try
            {
                if (guna2DateTimePicker1.Value.Date > guna2DateTimePicker2.Value.Date)
                {
                    MessageBox.Show("La fecha desde no puede ser mayor que la fecha hasta.",
                                    "Facturas de compra",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                _facturas = _facturaCompraDAO.ObtenerFacturasCompra(
                    guna2TextBox1.Text,
                    guna2DateTimePicker1.Value,
                    guna2DateTimePicker2.Value,
                    out string error);

                if (!string.IsNullOrWhiteSpace(error))
                {
                    MostrarError(error);
                    return;
                }

                dataGridView1.DataSource = _facturas;
            }
            finally
            {
                _cargando = false;
            }
        }

        private void VerDetalle()
        {
            if (!TryGetFacturaSeleccionada(out int compraID))
            {
                MessageBox.Show("Debe seleccionar una factura de compra.",
                                "Facturas de compra",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            DataTable detalle = _facturaCompraDAO.ObtenerDetalleFacturaCompra(compraID, out string error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                MostrarError(error);
                return;
            }

            using DetalleFactura frmDetalle = new DetalleFactura(compraID, detalle, "REGISTRADA", "Detalle de factura de compra");
            frmDetalle.ShowDialog(this);
        }

        private void ImprimirFactura()
        {
            if (!TryGetFacturaSeleccionada(out int compraID))
            {
                MessageBox.Show("Debe seleccionar una factura de compra para imprimir.",
                                "Facturas de compra",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (dataGridView1.CurrentRow?.DataBoundItem is not DataRowView factura)
                return;

            DataTable detalle = _facturaCompraDAO.ObtenerDetalleFacturaCompra(compraID, out string error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                MostrarError(error);
                return;
            }

            using FacturaImpresionPreview vistaPrevia = new FacturaImpresionPreview(
                factura.Row,
                detalle,
                "Factura de compra",
                "Vista previa de factura de compra",
                "Proveedor",
                "Proveedor",
                "Usuario",
                "UsuarioRegistro",
                false);

            vistaPrevia.ShowDialog(this);
        }

        private void ExportarFacturas()
        {
            if (_facturas.Rows.Count == 0)
            {
                MessageBox.Show("No hay facturas de compra para exportar.",
                                "Facturas de compra",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog guardar = new SaveFileDialog
            {
                Filter = "Archivo CSV (*.csv)|*.csv",
                FileName = $"facturas_compra_{DateTime.Now:yyyyMMdd_HHmm}.csv"
            };

            if (guardar.ShowDialog(this) != DialogResult.OK)
                return;

            StringBuilder csv = new StringBuilder();
            string[] columnas = _facturas.Columns.Cast<DataColumn>().Select(c => EscapeCsv(c.ColumnName)).ToArray();
            csv.AppendLine(string.Join(",", columnas));

            foreach (DataRow fila in _facturas.Rows)
            {
                string[] valores = _facturas.Columns.Cast<DataColumn>()
                    .Select(c => EscapeCsv(Convert.ToString(fila[c], CultureInfo.InvariantCulture) ?? string.Empty))
                    .ToArray();

                csv.AppendLine(string.Join(",", valores));
            }

            File.WriteAllText(guardar.FileName, csv.ToString(), Encoding.UTF8);
            MessageBox.Show("Facturas de compra exportadas correctamente.",
                            "Facturas de compra",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private bool TryGetFacturaSeleccionada(out int compraID)
        {
            compraID = 0;
            if (dataGridView1.CurrentRow?.DataBoundItem is not DataRowView fila)
                return false;

            return int.TryParse(fila["FacturaID"].ToString(), out compraID);
        }

        private static string EscapeCsv(string valor)
        {
            if (valor.Contains('"') || valor.Contains(',') || valor.Contains('\n') || valor.Contains('\r'))
                return "\"" + valor.Replace("\"", "\"\"") + "\"";

            return valor;
        }

        private static void MostrarError(string error)
        {
            MessageBox.Show(error,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
    }
}
