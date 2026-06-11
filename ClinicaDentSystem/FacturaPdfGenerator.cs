using System.Data;
using System.Globalization;
using System.Text;

namespace ClinicaDentSystem
{
    internal static class FacturaPdfGenerator
    {
        public static void Generar(DataRow factura, DataTable detalle, string rutaArchivo)
        {
            List<string> lineas = ConstruirLineas(factura, detalle);
            string contenido = ConstruirContenidoPdf(lineas);
            EscribirPdf(rutaArchivo, contenido);
        }

        private static List<string> ConstruirLineas(DataRow factura, DataTable detalle)
        {
            List<string> lineas = new List<string>
            {
                "Clinica Dental",
                "Factura emitida",
                $"Factura #: {ObtenerValor(factura, "FacturaID")}",
                $"Fecha: {ObtenerFecha(factura, "FechaEmision")}",
                $"Paciente: {ObtenerValor(factura, "Paciente")}",
                $"Facturador: {ObtenerValor(factura, "Facturador")}",
                $"Estado: {ObtenerValor(factura, "Estado")}",
                "",
                "Detalle"
            };

            foreach (DataRow item in detalle.Rows)
            {
                string linea = $"{ObtenerValor(item, "Tipo")} - {ObtenerValor(item, "Descripcion")} | Cant: {ObtenerValor(item, "Cantidad")} | Precio: {FormatearMoneda(item, "PrecioAplicado")} | Subtotal: {FormatearMoneda(item, "Subtotal")}";
                lineas.AddRange(PartirLinea(linea, 98));
            }

            lineas.Add("");
            lineas.Add($"Subtotal: {FormatearMoneda(factura, "Subtotal")}");
            lineas.Add($"Descuento: {ObtenerDecimal(factura, "DescuentoPorcentaje"):0.00}%");
            lineas.Add($"Total: {FormatearMoneda(factura, "Total")}");

            return lineas;
        }

        private static string ConstruirContenidoPdf(List<string> lineas)
        {
            StringBuilder contenido = new StringBuilder();
            float y = 750;

            foreach (string linea in lineas)
            {
                int tamano = linea == "Clinica Dental" ? 18 : linea == "Factura emitida" || linea == "Detalle" ? 13 : 10;
                contenido.Append("BT /F1 ");
                contenido.Append(tamano.ToString(CultureInfo.InvariantCulture));
                contenido.Append(" Tf 50 ");
                contenido.Append(y.ToString("0.##", CultureInfo.InvariantCulture));
                contenido.Append(" Td (");
                contenido.Append(EscaparTexto(linea));
                contenido.AppendLine(") Tj ET");
                y -= tamano + 8;

                if (y < 50)
                    break;
            }

            return contenido.ToString();
        }

        private static void EscribirPdf(string rutaArchivo, string contenido)
        {
            Encoding encoding = Encoding.ASCII;
            List<string> objetos = new List<string>
            {
                "<< /Type /Catalog /Pages 2 0 R >>",
                "<< /Type /Pages /Kids [3 0 R] /Count 1 >>",
                "<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Resources << /Font << /F1 5 0 R >> >> /Contents 4 0 R >>",
                $"<< /Length {encoding.GetByteCount(contenido)} >>\nstream\n{contenido}endstream",
                "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>"
            };

            using FileStream fs = new FileStream(rutaArchivo, FileMode.Create, FileAccess.Write);
            using BinaryWriter writer = new BinaryWriter(fs, encoding);
            List<long> posiciones = new List<long> { 0 };

            writer.Write(encoding.GetBytes("%PDF-1.4\n"));

            for (int i = 0; i < objetos.Count; i++)
            {
                posiciones.Add(fs.Position);
                writer.Write(encoding.GetBytes($"{i + 1} 0 obj\n{objetos[i]}\nendobj\n"));
            }

            long inicioXref = fs.Position;
            writer.Write(encoding.GetBytes($"xref\n0 {objetos.Count + 1}\n"));
            writer.Write(encoding.GetBytes("0000000000 65535 f \n"));

            for (int i = 1; i < posiciones.Count; i++)
                writer.Write(encoding.GetBytes($"{posiciones[i]:0000000000} 00000 n \n"));

            writer.Write(encoding.GetBytes($"trailer\n<< /Size {objetos.Count + 1} /Root 1 0 R >>\nstartxref\n{inicioXref}\n%%EOF"));
        }

        private static IEnumerable<string> PartirLinea(string texto, int maximo)
        {
            texto = NormalizarTexto(texto);
            while (texto.Length > maximo)
            {
                int corte = texto.LastIndexOf(' ', Math.Min(maximo, texto.Length - 1));
                if (corte <= 0)
                    corte = maximo;

                yield return texto.Substring(0, corte);
                texto = texto.Substring(corte).TrimStart();
            }

            yield return texto;
        }

        private static string EscaparTexto(string texto)
        {
            return NormalizarTexto(texto)
                .Replace("\\", "\\\\")
                .Replace("(", "\\(")
                .Replace(")", "\\)");
        }

        private static string NormalizarTexto(string texto)
        {
            StringBuilder sb = new StringBuilder(texto.Length);

            foreach (char c in texto)
            {
                if (c >= 32 && c <= 126)
                    sb.Append(c);
                else
                    sb.Append(RemoverAcento(c));
            }

            return sb.ToString();
        }

        private static char RemoverAcento(char c)
        {
            return c switch
            {
                'á' or 'Á' => 'a',
                'é' or 'É' => 'e',
                'í' or 'Í' => 'i',
                'ó' or 'Ó' => 'o',
                'ú' or 'Ú' => 'u',
                'ñ' or 'Ñ' => 'n',
                _ => '?'
            };
        }

        private static string ObtenerValor(DataRow fila, string columna)
        {
            if (!fila.Table.Columns.Contains(columna) || fila[columna] == DBNull.Value)
                return string.Empty;

            return fila[columna]?.ToString() ?? string.Empty;
        }

        private static string ObtenerFecha(DataRow fila, string columna)
        {
            if (!fila.Table.Columns.Contains(columna) || fila[columna] == DBNull.Value)
                return string.Empty;

            return Convert.ToDateTime(fila[columna], CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        private static string FormatearMoneda(DataRow fila, string columna)
        {
            return "$" + ObtenerDecimal(fila, columna).ToString("0.00", CultureInfo.InvariantCulture);
        }

        private static decimal ObtenerDecimal(DataRow fila, string columna)
        {
            if (!fila.Table.Columns.Contains(columna) || fila[columna] == DBNull.Value)
                return 0m;

            return Convert.ToDecimal(fila[columna], CultureInfo.InvariantCulture);
        }
    }
}
