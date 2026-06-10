using Microsoft.Data.SqlClient;
using System.Data;

namespace DAO
{
    public class FacturaCompraDAO
    {
        public DataTable ObtenerFacturasCompra(string buscar, DateTime fechaDesde, DateTime fechaHasta, out string pError)
        {
            return EjecutarTablaSP(
                "dbo.spSelectFacturasCompra",
                out pError,
                new SqlParameter("@Buscar", SqlDbType.VarChar, 100) { Value = string.IsNullOrWhiteSpace(buscar) ? DBNull.Value : buscar.Trim() },
                new SqlParameter("@FechaDesde", SqlDbType.Date) { Value = fechaDesde.Date },
                new SqlParameter("@FechaHasta", SqlDbType.Date) { Value = fechaHasta.Date });
        }

        public DataTable ObtenerDetalleFacturaCompra(int compraID, out string pError)
        {
            return EjecutarTablaSP(
                "dbo.spSelectDetalleFacturaCompra",
                out pError,
                new SqlParameter("@CompraID", SqlDbType.Int) { Value = compraID });
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

                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
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
    }
}
