using Microsoft.Data.SqlClient;
using MODELOS;
using System.Data;

namespace DAO
{
    public class DescuentosDAO : AbstractDAO<Descuentos>
    {
        public override void GuardarRegistro(Descuentos reg, out string pError)
        {
            EjecutarComandoDescuento("dbo.spInsertDescuento", reg, out pError);
        }

        public override void ActualizarRegistro(Descuentos reg, out string pError)
        {
            EjecutarComandoDescuento("dbo.spUpdateDescuento", reg, out pError, incluirId: true);
        }

        public override List<Descuentos> ObtenerTodos(out string pError)
        {
            pError = string.Empty;
            List<Descuentos> descuentos = new List<Descuentos>();
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                using SqlCommand cmd = new SqlCommand("dbo.spSelectDescuentos", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter msgParam = CrearParametroMensaje();
                cmd.Parameters.Add(msgParam);

                using SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Descuentos descuento = new Descuentos
                    {
                        DescuentoID = dr.GetInt32(0),
                        ServicioID = dr.IsDBNull(1) ? null : dr.GetInt32(1),
                        NombreServicio = dr.IsDBNull(2) ? string.Empty : dr.GetString(2),
                        ProductoID = dr.IsDBNull(3) ? null : dr.GetInt32(3),
                        NombreProducto = dr.IsDBNull(4) ? string.Empty : dr.GetString(4),
                        FechaIn = dr.GetDateTime(5),
                        FechaFn = dr.GetDateTime(6),
                        Porcentaje = dr.IsDBNull(7) ? 0m : dr.GetDecimal(7)
                    };

                    descuentos.Add(descuento);
                }

                if (!dr.IsClosed)
                    dr.Close();

                pError = msgParam.Value?.ToString() ?? string.Empty;
                if (pError.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
                    throw new Exception(pError);

                pError = string.Empty;
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

            return descuentos;
        }

        public DataTable ObtenerServicios(out string pError)
        {
            return EjecutarTablaSP("dbo.spDescuentoServicios", out pError);
        }

        public DataTable ObtenerProductos(out string pError)
        {
            return EjecutarTablaSP("dbo.spDescuentoProductos", out pError);
        }

        public void EliminarDescuento(int descuentoID, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                using SqlCommand cmd = new SqlCommand("dbo.spDeleteDescuento", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DescuentoID", SqlDbType.Int).Value = descuentoID;

                SqlParameter msgParam = CrearParametroMensaje();
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                pError = msgParam.Value?.ToString() ?? string.Empty;
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
        }

        public override Descuentos ObtenerPorId(int id, out string pError)
        {
            throw new NotImplementedException();
        }

        public override Descuentos ObtenerPorId(string id, out string pError)
        {
            throw new NotImplementedException();
        }

        private static void EjecutarComandoDescuento(string procedimiento, Descuentos reg, out string pError, bool incluirId = false)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                using SqlCommand cmd = new SqlCommand(procedimiento, conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (incluirId)
                    cmd.Parameters.Add("@DescuentoID", SqlDbType.Int).Value = reg.DescuentoID;

                cmd.Parameters.Add("@ServicioID", SqlDbType.Int).Value = reg.ServicioID.HasValue ? reg.ServicioID.Value : DBNull.Value;
                cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = reg.ProductoID.HasValue ? reg.ProductoID.Value : DBNull.Value;
                cmd.Parameters.Add("@FechaIn", SqlDbType.DateTime).Value = reg.FechaIn;
                cmd.Parameters.Add("@FechaFn", SqlDbType.DateTime).Value = reg.FechaFn;
                cmd.Parameters.Add("@Porcentaje", SqlDbType.Decimal).Value = reg.Porcentaje;

                SqlParameter msgParam = CrearParametroMensaje();
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                pError = msgParam.Value?.ToString() ?? string.Empty;
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
        }

        private static DataTable EjecutarTablaSP(string procedimiento, out string pError)
        {
            pError = string.Empty;
            DataTable tabla = new DataTable();
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                using SqlCommand cmd = new SqlCommand(procedimiento, conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter msgParam = CrearParametroMensaje();
                cmd.Parameters.Add(msgParam);

                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);

                pError = msgParam.Value?.ToString() ?? string.Empty;
                if (pError.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
                    throw new Exception(pError);

                pError = string.Empty;
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

        private static SqlParameter CrearParametroMensaje()
        {
            return new SqlParameter("@Mensaje", SqlDbType.VarChar, 100)
            {
                Direction = ParameterDirection.Output
            };
        }
    }
}
