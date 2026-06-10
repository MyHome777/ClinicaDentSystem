using System.Data;
using System.Drawing.Printing;
using System.Globalization;

namespace ClinicaDentSystem
{
    public partial class FacturaImpresionPreview : Form
    {
        private readonly PrintDocument _documento = new PrintDocument();
        private DataRow? _factura;
        private DataTable _detalle = new DataTable();
        private string _tituloDocumento = "Factura emitida";
        private string _tituloVentana = "Vista previa de factura";
        private string _terceroEtiqueta = "Paciente";
        private string _terceroColumna = "Paciente";
        private string _responsableEtiqueta = "Facturador";
        private string _responsableColumna = "Facturador";
        private bool _mostrarDescuento = true;

        public FacturaImpresionPreview()
        {
            InitializeComponent();
            _documento.PrintPage += Documento_PrintPage;
            printPreviewControl1.Document = _documento;
        }

        public FacturaImpresionPreview(DataRow factura, DataTable detalle) : this()
        {
            CargarFactura(factura, detalle);
        }

        public FacturaImpresionPreview(
            DataRow factura,
            DataTable detalle,
            string tituloDocumento,
            string tituloVentana,
            string terceroEtiqueta,
            string terceroColumna,
            string responsableEtiqueta,
            string responsableColumna,
            bool mostrarDescuento) : this()
        {
            _tituloDocumento = tituloDocumento;
            _tituloVentana = tituloVentana;
            _terceroEtiqueta = terceroEtiqueta;
            _terceroColumna = terceroColumna;
            _responsableEtiqueta = responsableEtiqueta;
            _responsableColumna = responsableColumna;
            _mostrarDescuento = mostrarDescuento;
            CargarFactura(factura, detalle);
        }

        public void CargarFactura(DataRow factura, DataTable detalle)
        {
            _factura = factura;
            _detalle = detalle;
            Text = $"{_tituloVentana} - Factura #{ObtenerValor(factura, "FacturaID")}";
            lblTitulo.Text = $"{_tituloVentana} #{ObtenerValor(factura, "FacturaID")}";
            printPreviewControl1.InvalidatePreview();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            using PrintDialog dialogo = new PrintDialog
            {
                Document = _documento,
                UseEXDialog = true
            };

            if (dialogo.ShowDialog(this) == DialogResult.OK)
                _documento.Print();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Documento_PrintPage(object? sender, PrintPageEventArgs e)
        {
            if (_factura == null || e.Graphics == null)
                return;

            DibujarFactura(e.Graphics, _factura, _detalle);
        }

        private void DibujarFactura(Graphics g, DataRow factura, DataTable detalle)
        {
            float y = 60;
            float x = 60;

            using Font titulo = new Font("Segoe UI", 18, FontStyle.Bold);
            using Font subtitulo = new Font("Segoe UI", 11, FontStyle.Bold);
            using Font normal = new Font("Segoe UI", 10);

            g.DrawString(_tituloDocumento, titulo, Brushes.Black, x, y);
            y += 42;
            g.DrawString($"Factura #: {ObtenerValor(factura, "FacturaID")}", normal, Brushes.Black, x, y);
            y += 24;
            g.DrawString($"Fecha: {Convert.ToDateTime(factura["FechaEmision"]):dd/MM/yyyy HH:mm}", normal, Brushes.Black, x, y);
            y += 24;
            g.DrawString($"{_terceroEtiqueta}: {ObtenerValor(factura, _terceroColumna)}", normal, Brushes.Black, x, y);
            y += 24;
            g.DrawString($"{_responsableEtiqueta}: {ObtenerValor(factura, _responsableColumna)}", normal, Brushes.Black, x, y);
            y += 24;
            g.DrawString($"Estado: {ObtenerValor(factura, "Estado")}", normal, Brushes.Black, x, y);
            y += 38;

            g.DrawString("Detalle", subtitulo, Brushes.Black, x, y);
            y += 28;

            foreach (DataRow item in detalle.Rows)
            {
                string linea = $"{item["Tipo"]} - {item["Descripcion"]} | Cant: {item["Cantidad"]} | Precio: {FormatearMoneda(Convert.ToDecimal(item["PrecioAplicado"]))} | Subtotal: {FormatearMoneda(Convert.ToDecimal(item["Subtotal"]))}";
                g.DrawString(linea, normal, Brushes.Black, x, y);
                y += 24;
            }

            y += 28;
            g.DrawString($"Subtotal: {FormatearMoneda(Convert.ToDecimal(factura["Subtotal"]))}", normal, Brushes.Black, x, y);
            if (_mostrarDescuento)
            {
                y += 24;
                g.DrawString($"Descuento: {Convert.ToDecimal(factura["DescuentoPorcentaje"]):0.00}%", normal, Brushes.Black, x, y);
            }
            y += 24;
            g.DrawString($"Total: {FormatearMoneda(Convert.ToDecimal(factura["Total"]))}", subtitulo, Brushes.Black, x, y);
        }

        private static string FormatearMoneda(decimal valor)
        {
            return "$" + valor.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private static string ObtenerValor(DataRow fila, string columna)
        {
            if (!fila.Table.Columns.Contains(columna) || fila[columna] == DBNull.Value)
                return string.Empty;

            return fila[columna]?.ToString() ?? string.Empty;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            using PrintDialog dialogo = new PrintDialog
            {
                Document = _documento,
                UseEXDialog = true
            };

            if (dialogo.ShowDialog(this) == DialogResult.OK)
                _documento.Print();

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
