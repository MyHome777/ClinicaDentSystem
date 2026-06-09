using DAO;
using System.Data;
using System.Globalization;
using System.Text;

namespace ClinicaDentSystem
{
    public partial class facEmitidas : Form
    {
        private readonly FacturacionDAO _facturacionDAO = new FacturacionDAO();
        private DataTable _facturas = new DataTable();
        private DataTable _detalleActual = new DataTable();
        private bool _cargando;

        public facEmitidas()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConfigurarEventos();
        }

        private void guna2Button3_Click(object? sender, EventArgs e)
        {
            VerDetalleFactura();
        }

        private void guna2Button4_Click(object? sender, EventArgs e)
        {
            AnularFacturaSeleccionada();
        }

        private void ConfigurarFormulario()
        {
            StartPosition = FormStartPosition.CenterParent;
            guna2DateTimePicker1.Value = DateTime.Today.AddMonths(-1);
            guna2DateTimePicker2.Value = DateTime.Today;

            guna2ComboBox4.Items.Clear();
            guna2ComboBox4.Items.Add("Todas");
            guna2ComboBox4.Items.Add("Emitida");
            guna2ComboBox4.Items.Add("Pagada");
            guna2ComboBox4.Items.Add("Anulada");
            guna2ComboBox4.Items.Add("Pendiente");
            guna2ComboBox4.SelectedIndex = 0;

            guna2ImageRadioButton1.Enabled = false;
            guna2ImageRadioButton2.Enabled = false;
            guna2ImageRadioButton3.Enabled = false;
            guna2ImageRadioButton4.Enabled = false;
            guna2ImageRadioButton5.Enabled = false;

            ConfigurarGridFacturas();
            CargarFacturas();
        }

        private void ConfigurarEventos()
        {
            btnPacientes.Click += (sender, e) => CargarFacturas();
            guna2Button4.Click += guna2Button4_Click;
            guna2Button2.Click += (sender, e) => ExportarFacturas();
            guna2Button1.Click += (sender, e) => ImprimirFacturaSeleccionada();
            dataGridView1.CellDoubleClick += (sender, e) => VerDetalleFactura();
            dataGridView1.SelectionChanged += (sender, e) => CargarDetalleActual();
            guna2TextBox1.TextChanged += (sender, e) => CargarFacturasSiListo();
            guna2ComboBox4.SelectedIndexChanged += (sender, e) => CargarFacturasSiListo();
            guna2DateTimePicker1.ValueChanged += (sender, e) => CargarFacturasSiListo();
            guna2DateTimePicker2.ValueChanged += (sender, e) => CargarFacturasSiListo();
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
                HeaderText = "Paciente",
                DataPropertyName = "Paciente",
                Width = 170
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Facturador",
                DataPropertyName = "Facturador",
                Width = 130
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Subtotal",
                DataPropertyName = "Subtotal",
                DefaultCellStyle = { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight },
                Width = 95
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Desc. %",
                DataPropertyName = "DescuentoPorcentaje",
                DefaultCellStyle = { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight },
                Width = 85
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Total",
                DataPropertyName = "Total",
                DefaultCellStyle = { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight },
                Width = 95
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
                                    "Facturas Emitidas",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                string estado = guna2ComboBox4.SelectedItem?.ToString() ?? "Todas";
                if (estado == "Todas")
                    estado = string.Empty;

                _facturas = _facturacionDAO.ObtenerFacturasEmitidas(
                    guna2TextBox1.Text,
                    estado,
                    guna2DateTimePicker1.Value,
                    guna2DateTimePicker2.Value,
                    out string error);

                if (!string.IsNullOrEmpty(error))
                {
                    MostrarError(error);
                    return;
                }

                dataGridView1.DataSource = _facturas;
                CargarDetalleActual();
            }
            finally
            {
                _cargando = false;
            }
        }

        private void CargarDetalleActual()
        {
            if (!TryGetFacturaSeleccionada(out int facturaID))
            {
                _detalleActual = new DataTable();
                return;
            }

            _detalleActual = _facturacionDAO.ObtenerDetalleFactura(facturaID, out string error);
            if (!string.IsNullOrEmpty(error))
                _detalleActual = new DataTable();
        }

        private void VerDetalleFactura()
        {
            if (!TryGetFacturaSeleccionada(out int facturaID))
            {
                MessageBox.Show("Debe seleccionar una factura.",
                                "Facturas Emitidas",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            DataTable detalle = _facturacionDAO.ObtenerDetalleFactura(facturaID, out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MostrarError(error);
                return;
            }

            _detalleActual = detalle;
            string estado = ObtenerEstadoSeleccionado();
            using DetalleFactura frmDetalle = new DetalleFactura(facturaID, detalle, estado);
            if (frmDetalle.ShowDialog(this) == DialogResult.OK)
                CargarFacturas();
        }

        private void AnularFacturaSeleccionada()
        {
            if (!TryGetFacturaSeleccionada(out int facturaID))
            {
                MessageBox.Show("Debe seleccionar una factura para anular.",
                                "Facturas Emitidas",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            string estado = ObtenerEstadoSeleccionado();
            if (estado.Equals("ANULADA", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("La factura seleccionada ya esta anulada.",
                                "Facturas Emitidas",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            if (estado.Equals("PAGADA", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Una factura pagada no se puede anular desde este control.",
                                "Facturas Emitidas",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmacion = MessageBox.Show($"Deseas anular la factura #{facturaID}?",
                                                        "Confirmar anulacion",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question);
            if (confirmacion != DialogResult.Yes)
                return;

            int usuarioID = Program.UsuarioActivo?.UsuarioId ?? 0;
            if (usuarioID <= 0)
            {
                MessageBox.Show("No se encontro el usuario activo para registrar la anulacion.",
                                "Facturas Emitidas",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            bool anulada = _facturacionDAO.AnularFactura(facturaID, usuarioID, out string mensaje);
            MessageBox.Show(mensaje,
                            anulada ? "Facturas Emitidas" : "Error",
                            MessageBoxButtons.OK,
                            anulada ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (anulada)
                CargarFacturas();
        }

        private void ExportarFacturas()
        {
            if (_facturas.Rows.Count == 0)
            {
                MessageBox.Show("No hay facturas para exportar.",
                                "Facturas Emitidas",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog guardar = new SaveFileDialog
            {
                Filter = "Archivo CSV (*.csv)|*.csv",
                FileName = $"facturas_emitidas_{DateTime.Now:yyyyMMdd_HHmm}.csv"
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
            MessageBox.Show("Facturas exportadas correctamente.",
                            "Facturas Emitidas",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private void ImprimirFacturaSeleccionada()
        {
            if (!TryGetFacturaSeleccionada(out int facturaID))
            {
                MessageBox.Show("Debe seleccionar una factura para imprimir.",
                                "Facturas Emitidas",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            DataRowView? factura = dataGridView1.CurrentRow?.DataBoundItem as DataRowView;
            if (factura == null)
                return;

            DataTable detalle = _facturacionDAO.ObtenerDetalleFactura(facturaID, out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MostrarError(error);
                return;
            }

            using FacturaImpresionPreview vistaPrevia = new FacturaImpresionPreview(factura.Row, detalle);
            vistaPrevia.ShowDialog(this);
        }

        private bool TryGetFacturaSeleccionada(out int facturaID)
        {
            facturaID = 0;
            if (dataGridView1.CurrentRow?.DataBoundItem is not DataRowView fila)
                return false;

            return int.TryParse(fila["FacturaID"].ToString(), out facturaID);
        }

        private string ObtenerEstadoSeleccionado()
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is DataRowView fila)
                return fila["Estado"].ToString() ?? string.Empty;

            return string.Empty;
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
