using Microsoft.Data.SqlClient;
using MODELOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class MovimientoStockDAO : AbstractDAO<MovimientoStock>
    {
        // ── REGISTRAR AJUSTE MANUAL ───────────────────────────────────────────
        // SP: spInsertMovimiento — valida stock si TipoMovimiento = "SALIDA"
        public override void GuardarRegistro(MovimientoStock reg, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                SqlCommand cmd = new SqlCommand("spInsertMovimiento", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoID",     reg.ProductoID);
                cmd.Parameters.AddWithValue("@TipoMovimiento", reg.TipoMovimiento);
                cmd.Parameters.AddWithValue("@Cantidad",        reg.Cantidad);
                cmd.Parameters.AddWithValue("@Descripcion",     reg.Descripcion);
                cmd.Parameters.AddWithValue("@UsuarioID",       reg.UsuarioID);

                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
                msgParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                pError = msgParam.Value?.ToString() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(pError))
                    pError = "No se recibio respuesta del procedimiento spInsertMovimiento.";

                conn.CerrarConexion(out string errCerrar);
                if (!string.IsNullOrEmpty(errCerrar)) pError = errCerrar;
            }
            catch (SqlException ex) { pError = ex.Message; Console.WriteLine(pError); }
            catch (Exception ex)    { if (string.IsNullOrEmpty(pError)) pError = ex.Message; Console.WriteLine(pError); }
        }

        // ── OBTENER HISTORIAL COMPLETO ────────────────────────────────────────
        // SP: spSelectMovimientos
        public override List<MovimientoStock> ObtenerTodos(out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            List<MovimientoStock> lista = new List<MovimientoStock>();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                SqlCommand cmd = new SqlCommand("spSelectMovimientos", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
                msgParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgParam);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    MovimientoStock m = new MovimientoStock();
                    m.MovimientoID    = dr.GetInt32(0);
                    m.ProductoID      = dr.GetInt32(1);
                    m.NombreProducto  = dr.IsDBNull(2) ? string.Empty : dr.GetString(2);
                    m.TipoMovimiento  = dr.GetString(3);
                    m.Cantidad        = dr.GetInt32(4);
                    m.FechaMovimiento = dr.GetDateTime(5);
                    m.Descripcion     = dr.IsDBNull(6) ? string.Empty : dr.GetString(6);
                    m.UsuarioID       = dr.GetInt32(7);
                    lista.Add(m);
                }

                if (!dr.IsClosed) dr.Close();
                conn.CerrarConexion(out string errCerrar);
                if (!string.IsNullOrEmpty(errCerrar)) pError = errCerrar;
            }
            catch (SqlException ex) { pError = ex.Message; Console.WriteLine(pError); }
            catch (Exception ex)    { if (string.IsNullOrEmpty(pError)) pError = ex.Message; Console.WriteLine(pError); }

            return lista;
        }

        public override void ActualizarRegistro(MovimientoStock reg, out string pError)
            => throw new NotImplementedException();
        public override MovimientoStock ObtenerPorId(int id, out string pError)
            => throw new NotImplementedException();
        public override MovimientoStock ObtenerPorId(string id, out string pError)
            => throw new NotImplementedException();
    }
}
