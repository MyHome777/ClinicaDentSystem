using DAO;
using Modelos;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace ClinicaDentSystem
{
    internal class CorreoFacturaService
    {
        private readonly ConfiguracionCorreoDAO _configuracionCorreoDAO = new ConfiguracionCorreoDAO();
        private readonly FacturacionDAO _facturacionDAO = new FacturacionDAO();

        public bool EnviarFactura(int facturaID, out string mensaje)
        {
            mensaje = string.Empty;
            string rutaPdf = string.Empty;

            try
            {
                ConfiguracionCorreo configuracion = _configuracionCorreoDAO.ObtenerConfiguracion(out string errorConfig);
                if (!string.IsNullOrEmpty(errorConfig))
                {
                    mensaje = errorConfig;
                    return false;
                }

                if (!configuracion.EstaCompleta)
                {
                    mensaje = "La configuracion de correo no esta completa.";
                    return false;
                }

                DataTable factura = _facturacionDAO.ObtenerFacturaParaCorreo(facturaID, out string errorFactura);
                if (!string.IsNullOrEmpty(errorFactura))
                {
                    mensaje = errorFactura;
                    return false;
                }

                if (factura.Rows.Count == 0)
                {
                    mensaje = "No se encontro la factura para enviar por correo.";
                    return false;
                }

                DataRow filaFactura = factura.Rows[0];
                string correoPaciente = ObtenerValor(filaFactura, "PacienteEmail");
                if (string.IsNullOrWhiteSpace(correoPaciente))
                {
                    mensaje = "El paciente no tiene correo registrado.";
                    return false;
                }

                DataTable detalle = _facturacionDAO.ObtenerDetalleFactura(facturaID, out string errorDetalle);
                if (!string.IsNullOrEmpty(errorDetalle))
                {
                    mensaje = errorDetalle;
                    return false;
                }

                rutaPdf = CrearRutaTemporal(facturaID);
                FacturaPdfGenerator.Generar(filaFactura, detalle, rutaPdf);
                EnviarCorreo(configuracion, correoPaciente, facturaID, rutaPdf);

                mensaje = $"Correo enviado a {correoPaciente}.";
                return true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(rutaPdf) && File.Exists(rutaPdf))
                {
                    try
                    {
                        File.Delete(rutaPdf);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static void EnviarCorreo(ConfiguracionCorreo configuracion, string correoPaciente, int facturaID, string rutaPdf)
        {
            using MailMessage correo = new MailMessage();
            correo.From = new MailAddress(
                configuracion.CorreoRemitente,
                string.IsNullOrWhiteSpace(configuracion.NombreRemitente) ? configuracion.CorreoRemitente : configuracion.NombreRemitente);
            correo.To.Add(correoPaciente);
            correo.Subject = $"Factura #{facturaID} - Clinica Dental";
            correo.Body = $"Estimado paciente,\n\nAdjuntamos su factura #{facturaID} en formato PDF.\n\nGracias por su visita.";
            correo.Attachments.Add(new Attachment(rutaPdf, MediaTypeNames.Application.Pdf));

            using SmtpClient smtp = new SmtpClient(configuracion.ServidorSmtp, configuracion.Puerto);
            smtp.EnableSsl = configuracion.UsaSsl;
            smtp.Credentials = new NetworkCredential(configuracion.CorreoRemitente, configuracion.ClaveCorreo);
            smtp.Send(correo);
        }

        private static string CrearRutaTemporal(int facturaID)
        {
            string carpeta = Path.Combine(Path.GetTempPath(), "ClinicaDentSystem", "Facturas");
            Directory.CreateDirectory(carpeta);
            return Path.Combine(carpeta, $"Factura_{facturaID}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }

        private static string ObtenerValor(DataRow fila, string columna)
        {
            if (!fila.Table.Columns.Contains(columna) || fila[columna] == DBNull.Value)
                return string.Empty;

            return fila[columna]?.ToString() ?? string.Empty;
        }
    }
}
