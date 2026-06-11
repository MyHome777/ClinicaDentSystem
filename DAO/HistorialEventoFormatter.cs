using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace DAO
{
    internal static class HistorialEventoFormatter
    {
        public static void AplicarNombres(DataTable tabla, SqlConnection conexion)
        {
            DataColumn? columnaEvento = tabla.Columns["evento"];
            if (columnaEvento == null)
            {
                return;
            }

            columnaEvento.MaxLength = -1;

            CatalogosHistorial catalogos = CargarCatalogos(conexion);

            foreach (DataRow fila in tabla.Rows)
            {
                string evento = fila["evento"]?.ToString() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(evento))
                {
                    continue;
                }

                evento = ReemplazarReferencia(evento, "productoId", "producto", catalogos.Productos);
                evento = ReemplazarReferencia(evento, "servicioId", "servicio", catalogos.Servicios);
                evento = ReemplazarReferencia(evento, "pacienteId", "paciente", catalogos.Pacientes);
                evento = ReemplazarReferencia(evento, "dentistaId", "dentista", catalogos.Dentistas);
                evento = ReemplazarReferencia(evento, "rolId", "rol", catalogos.Roles);
                evento = ReemplazarReferencia(evento, "facturaId", "factura", catalogos.Facturas);
                evento = ReemplazarReferencia(evento, "compraId", "compra", catalogos.Compras);
                evento = ReemplazarReferencia(evento, "citaId", "cita", catalogos.Citas);
                evento = ReemplazarReferencia(evento, "usuarioId", "usuario", catalogos.Usuarios);

                fila["evento"] = evento;
            }
        }

        private static string ReemplazarReferencia(
            string evento,
            string etiquetaOriginal,
            string etiquetaNueva,
            Dictionary<int, string> catalogo)
        {
            string patron = @"\b" + Regex.Escape(etiquetaOriginal) + @"\s*:\s*(\d+)";

            return Regex.Replace(
                evento,
                patron,
                match =>
                {
                    int id = Convert.ToInt32(match.Groups[1].Value);
                    string valor = catalogo.TryGetValue(id, out string? nombre) && !string.IsNullOrWhiteSpace(nombre)
                        ? nombre
                        : "#" + id;

                    return etiquetaNueva + ": " + valor;
                },
                RegexOptions.IgnoreCase);
        }

        private static CatalogosHistorial CargarCatalogos(SqlConnection conexion)
        {
            return new CatalogosHistorial
            {
                Productos = CargarCatalogo(conexion, "SELECT ProductoID, NombreProducto FROM INVENTARIO.PRODUCTO;"),
                Servicios = CargarCatalogo(conexion, "SELECT ServicioID, NombreServicio FROM CLINICO.SERVICIOS;"),
                Pacientes = CargarCatalogo(conexion, "SELECT PacienteId, CONCAT(Nombre, ' ', Apellido) FROM CLINICO.PACIENTE;"),
                Dentistas = CargarCatalogo(conexion, "SELECT DentistaID, CONCAT(Nombre, ' ', Apellido) FROM CLINICO.DENTISTA;"),
                Roles = CargarCatalogo(conexion, "SELECT RolID, NombreRol FROM SEGURIDAD.ROL;"),
                Usuarios = CargarCatalogo(conexion, "SELECT UsuarioID, NombreUsuario FROM SEGURIDAD.USUARIO;"),
                Citas = CargarCatalogo(
                    conexion,
                    @"SELECT
                        c.CitaID,
                        CONCAT('Cita #', c.CitaID, ' - ', p.Nombre, ' ', p.Apellido, ' con ', d.Nombre, ' ', d.Apellido)
                      FROM CLINICO.CITA c
                      INNER JOIN CLINICO.PACIENTE p ON c.PacienteId = p.PacienteId
                      INNER JOIN CLINICO.DENTISTA d ON c.DentistaID = d.DentistaID;"),
                Facturas = CargarCatalogo(
                    conexion,
                    @"SELECT
                        f.FacturaID,
                        CONCAT('Factura #', f.FacturaID, ' - ', p.Nombre, ' ', p.Apellido)
                      FROM FACTURACION.FACTURA f
                      INNER JOIN CLINICO.CITA c ON f.CitaID = c.CitaID
                      INNER JOIN CLINICO.PACIENTE p ON c.PacienteId = p.PacienteId;"),
                Compras = CargarCatalogo(
                    conexion,
                    @"SELECT
                        c.CompraID,
                        CONCAT('Compra #', c.CompraID, ' - ', p.NombreProveedor)
                      FROM INVENTARIO.COMPRA c
                      INNER JOIN INVENTARIO.PROVEEDOR p ON c.ProveedorID = p.ProveedorID;")
            };
        }

        private static Dictionary<int, string> CargarCatalogo(SqlConnection conexion, string sql)
        {
            Dictionary<int, string> datos = new Dictionary<int, string>();

            try
            {
                using SqlCommand cmd = new SqlCommand(sql, conexion);
                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.IsDBNull(0))
                    {
                        continue;
                    }

                    int id = Convert.ToInt32(reader.GetValue(0));
                    string nombre = reader.IsDBNull(1) ? string.Empty : reader.GetValue(1).ToString() ?? string.Empty;

                    if (!datos.ContainsKey(id))
                    {
                        datos.Add(id, nombre);
                    }
                }
            }
            catch
            {
                return datos;
            }

            return datos;
        }

        private sealed class CatalogosHistorial
        {
            public Dictionary<int, string> Productos { get; set; } = new Dictionary<int, string>();
            public Dictionary<int, string> Servicios { get; set; } = new Dictionary<int, string>();
            public Dictionary<int, string> Pacientes { get; set; } = new Dictionary<int, string>();
            public Dictionary<int, string> Dentistas { get; set; } = new Dictionary<int, string>();
            public Dictionary<int, string> Roles { get; set; } = new Dictionary<int, string>();
            public Dictionary<int, string> Usuarios { get; set; } = new Dictionary<int, string>();
            public Dictionary<int, string> Citas { get; set; } = new Dictionary<int, string>();
            public Dictionary<int, string> Facturas { get; set; } = new Dictionary<int, string>();
            public Dictionary<int, string> Compras { get; set; } = new Dictionary<int, string>();
        }
    }
}
