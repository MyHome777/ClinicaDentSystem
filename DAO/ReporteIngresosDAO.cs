using Microsoft.Data.SqlClient;
using System.Data;

namespace DAO
{
    public class ReporteIngresosDAO
    {
        public DataTable ObtenerResumen(DateTime fechaDesde, DateTime fechaHasta, string? estado, out string pError)
        {
            return EjecutarTablaSP(
                "dbo.spReporteIngresosResumen",
                out pError,
                CrearParametroFecha("@FechaDesde", fechaDesde),
                CrearParametroFecha("@FechaHasta", fechaHasta),
                CrearParametroEstado(estado));
        }

        public DataTable ObtenerServicios(DateTime fechaDesde, DateTime fechaHasta, string? estado, out string pError)
        {
            return EjecutarTablaSP(
                "dbo.spReporteIngresosServicios",
                out pError,
                CrearParametroFecha("@FechaDesde", fechaDesde),
                CrearParametroFecha("@FechaHasta", fechaHasta),
                CrearParametroEstado(estado));
        }

        public DataTable ObtenerProductos(DateTime fechaDesde, DateTime fechaHasta, string? estado, out string pError)
        {
            return EjecutarTablaSP(
                "dbo.spReporteIngresosProductos",
                out pError,
                CrearParametroFecha("@FechaDesde", fechaDesde),
                CrearParametroFecha("@FechaHasta", fechaHasta),
                CrearParametroEstado(estado));
        }

        private static DataTable EjecutarTablaSP(string procedimiento, out string pError, params SqlParameter[] parametros)
        {
            pError = string.Empty;
            DataTable tabla = new DataTable();
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                using SqlCommand cmd = new SqlCommand(procedimiento, conn.conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (parametros.Length > 0)
                    cmd.Parameters.AddRange(parametros);

                SqlParameter msgParam = CrearParametroMensaje();
                cmd.Parameters.Add(msgParam);

                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);

                string mensaje = msgParam.Value?.ToString() ?? string.Empty;
                if (mensaje.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
                    throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                pError = ex.Message;
            }
            finally
            {
                conn.CerrarConexion(out string errorCerrar);
                if (!string.IsNullOrEmpty(errorCerrar) && string.IsNullOrEmpty(pError))
                    pError = errorCerrar;
            }

            return tabla;
        }

        private static SqlParameter CrearParametroFecha(string nombre, DateTime fecha)
        {
            return new SqlParameter(nombre, SqlDbType.Date)
            {
                Value = fecha.Date
            };
        }

        private static SqlParameter CrearParametroEstado(string? estado)
        {
            return new SqlParameter("@Estado", SqlDbType.VarChar, 20)
            {
                Value = string.IsNullOrWhiteSpace(estado) ? DBNull.Value : estado.Trim()
            };
        }

        private static SqlParameter CrearParametroMensaje()
        {
            return new SqlParameter("@Mensaje", SqlDbType.VarChar, 100)
            {
                Direction = ParameterDirection.Output
            };
        }
    }
}
