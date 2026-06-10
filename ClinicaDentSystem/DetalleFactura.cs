using System.Data;
using System.Globalization;
using DAO;

namespace ClinicaDentSystem
{
    public partial class DetalleFactura : Form
    {
        private readonly FacturacionDAO _facturacionDAO = new FacturacionDAO();
        private DataTable _detalle = new DataTable();
        private int _facturaID;
        private string _estado = string.Empty;
        private string _tituloDetalle = "Detalle de factura";

        public DetalleFactura()
        {
            InitializeComponent();
            ConfigurarGrid();
            guna2Button2.Click += btnMarcarComoPagada_Click;
        }

        public DetalleFactura(int facturaID, DataTable detalle) : this()
        {
            CargarDetalle(facturaID, detalle);
        }

        public DetalleFactura(int facturaID, DataTable detalle, string estado) : this()
        {
            CargarDetalle(facturaID, detalle, estado);
        }

        public DetalleFactura(int facturaID, DataTable detalle, string estado, string tituloDetalle) : this()
        {
            _tituloDetalle = string.IsNullOrWhiteSpace(tituloDetalle) ? _tituloDetalle : tituloDetalle;
            CargarDetalle(facturaID, detalle, estado);
        }

        public void CargarDetalle(int facturaID, DataTable detalle)
        {
            CargarDetalle(facturaID, detalle, _estado);
        }

        public void CargarDetalle(int facturaID, DataTable detalle, string estado)
        {
            _facturaID = facturaID;
            _detalle = detalle;
            _estado = estado ?? string.Empty;
            lblTitulo.Text = $"{_tituloDetalle} #{facturaID}";
            dgvDetalle.DataSource = _detalle;
            lblTotal.Text = $"Total detalle: {FormatearMoneda(CalcularTotalDetalle(_detalle))}";
            ActualizarBotonPago();
        }

        private void ConfigurarGrid()
        {
            dgvDetalle.AutoGenerateColumns = false;
            dgvDetalle.Columns.Clear();

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "DetalleFID",
                DataPropertyName = "DetalleFID",
                Width = 90
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tipo",
                DataPropertyName = "Tipo",
                Width = 100
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Descripcion",
                DataPropertyName = "Descripcion",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Cantidad",
                DataPropertyName = "Cantidad",
                Width = 85,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "PrecioAplicado",
                DataPropertyName = "PrecioAplicado",
                Width = 110,
                DefaultCellStyle = { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Subtotal",
                DataPropertyName = "Subtotal",
                Width = 100,
                DefaultCellStyle = { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Desc. %",
                DataPropertyName = "DescuentoPorcentaje",
                Width = 85,
                DefaultCellStyle = { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Total",
                DataPropertyName = "TotalLinea",
                Width = 100,
                DefaultCellStyle = { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
        }

        private static decimal CalcularTotalDetalle(DataTable detalle)
        {
            if (detalle.Columns.Contains("TotalLinea"))
                return detalle.AsEnumerable().Sum(fila => Convert.ToDecimal(fila["TotalLinea"]));

            if (!detalle.Columns.Contains("Subtotal"))
                return 0m;

            return detalle.AsEnumerable().Sum(fila => Convert.ToDecimal(fila["Subtotal"]));
        }

        private static string FormatearMoneda(decimal valor)
        {
            return "$" + valor.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private void ActualizarBotonPago()
        {
            bool puedeMarcarPagada = _estado.Equals("EMITIDA", StringComparison.OrdinalIgnoreCase);
            guna2Button2.Visible = puedeMarcarPagada;
            guna2Button2.Enabled = puedeMarcarPagada;
        }

        private void btnMarcarComoPagada_Click(object? sender, EventArgs e)
        {
            if (!_estado.Equals("EMITIDA", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Solo una factura emitida se puede marcar como pagada.",
                                "Detalle de factura",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            int usuarioID = Program.UsuarioActivo?.UsuarioId ?? 0;
            if (usuarioID <= 0)
            {
                MessageBox.Show("No se encontro el usuario activo para registrar el pago.",
                                "Detalle de factura",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmacion = MessageBox.Show($"Deseas marcar la factura #{_facturaID} como pagada?",
                                                        "Confirmar pago",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question);
            if (confirmacion != DialogResult.Yes)
                return;

            bool pagada = _facturacionDAO.MarcarFacturaComoPagada(_facturaID, usuarioID, out string mensaje);
            MessageBox.Show(mensaje,
                            pagada ? "Detalle de factura" : "Error",
                            MessageBoxButtons.OK,
                            pagada ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (!pagada)
                return;

            _estado = "PAGADA";
            ActualizarBotonPago();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }
    }
}
