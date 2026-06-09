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

        public void CargarFactura(DataRow factura, DataTable detalle)
        {
            _factura = factura;
            _detalle = detalle;
            Text = $"Vista previa - Factura #{factura["FacturaID"]}";
            lblTitulo.Text = $"Vista previa de factura #{factura["FacturaID"]}";
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

        private static void DibujarFactura(Graphics g, DataRow factura, DataTable detalle)
        {
            float y = 60;
            float x = 60;

            using Font titulo = new Font("Segoe UI", 18, FontStyle.Bold);
            using Font subtitulo = new Font("Segoe UI", 11, FontStyle.Bold);
            using Font normal = new Font("Segoe UI", 10);

            g.DrawString("Factura emitida", titulo, Brushes.Black, x, y);
            y += 42;
            g.DrawString($"Factura #: {factura["FacturaID"]}", normal, Brushes.Black, x, y);
            y += 24;
            g.DrawString($"Fecha: {Convert.ToDateTime(factura["FechaEmision"]):dd/MM/yyyy HH:mm}", normal, Brushes.Black, x, y);
            y += 24;
            g.DrawString($"Paciente: {factura["Paciente"]}", normal, Brushes.Black, x, y);
            y += 24;
            g.DrawString($"Facturador: {factura["Facturador"]}", normal, Brushes.Black, x, y);
            y += 24;
            g.DrawString($"Estado: {factura["Estado"]}", normal, Brushes.Black, x, y);
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
            y += 24;
            g.DrawString($"Descuento: {Convert.ToDecimal(factura["DescuentoPorcentaje"]):0.00}%", normal, Brushes.Black, x, y);
            y += 24;
            g.DrawString($"Total: {FormatearMoneda(Convert.ToDecimal(factura["Total"]))}", subtitulo, Brushes.Black, x, y);
        }

        private static string FormatearMoneda(decimal valor)
        {
            return "$" + valor.ToString("0.00", CultureInfo.InvariantCulture);
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
