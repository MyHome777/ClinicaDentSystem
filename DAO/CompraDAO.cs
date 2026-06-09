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
    public class CompraDAO : AbstractDAO<Compra>
    {
        // ── GUARDAR (sin retorno de ID) ───────────────────────────────────────
        public override void GuardarRegistro(Compra reg, out string pError)
        {
            GuardarRegistroConId(reg, out pError);
        }

        // ── GUARDAR con retorno de CompraID ───────────────────────────────────
        // SP: dbo.spInsertCompras
        public int GuardarRegistroConId(Compra reg, out string pError)
        {
            pError = string.Empty;
            int compraID = 0;
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                SqlCommand cmd = new SqlCommand("dbo.spInsertCompras", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProveedorID", reg.ProveedorID);
                cmd.Parameters.AddWithValue("@UsuarioID",   reg.UsuarioID);
                cmd.Parameters.AddWithValue("@FechaCompra", reg.FechaCompra);
                cmd.Parameters.AddWithValue("@TotalCompra", reg.TotalCompra);

                SqlParameter idParam = new SqlParameter("@CompraID", SqlDbType.Int);
                idParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(idParam);

                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
                msgParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                pError = msgParam.Value?.ToString() ?? string.Empty;

                if (idParam.Value != null && idParam.Value != DBNull.Value)
                    compraID = Convert.ToInt32(idParam.Value);

                conn.CerrarConexion(out string errCerrar);
                if (!string.IsNullOrEmpty(errCerrar)) pError = errCerrar;
            }
            catch (SqlException ex) { pError = ex.Message; Console.WriteLine(pError); }
            catch (Exception ex)    { if (string.IsNullOrEmpty(pError)) pError = ex.Message; Console.WriteLine(pError); }

            return compraID;
        }

        // ── OBTENER TODAS ─────────────────────────────────────────────────────
        // SP: spSelectCompras
        // Columnas: CompraID, ProveedorID, NombreProveedor, UsuarioID, FechaCompra, TotalCompra
        public override List<Compra> ObtenerTodos(out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            List<Compra> lista = new List<Compra>();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                SqlCommand cmd = new SqlCommand("spSelectCompras", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
                msgParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgParam);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Compra c = new Compra();
                    c.CompraID        = dr.GetInt32(0);
                    c.ProveedorID     = dr.GetInt32(1);
                    c.NombreProveedor = dr.IsDBNull(2) ? string.Empty : dr.GetString(2);
                    c.UsuarioID       = dr.GetInt32(3);
                    c.FechaCompra     = dr.GetDateTime(4);
                    c.TotalCompra     = dr.GetDecimal(5);
                    lista.Add(c);
                }

                if (!dr.IsClosed) dr.Close();
                conn.CerrarConexion(out string errCerrar);
                if (!string.IsNullOrEmpty(errCerrar)) pError = errCerrar;
            }
            catch (SqlException ex) { pError = ex.Message; Console.WriteLine(pError); }
            catch (Exception ex)    { if (string.IsNullOrEmpty(pError)) pError = ex.Message; Console.WriteLine(pError); }

            return lista;
        }

        // ── OBTENER TOTAL REAL (Opción B) ─────────────────────────────────────
        // Suma PrecioTotal de DETALLECOMPRA — ya calculado por el SP
        public decimal ObtenerTotalReal(int compraID)
        {
            decimal total = 0;
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out string pError);
                if (!string.IsNullOrEmpty(pError)) return 0;

                SqlCommand cmd = new SqlCommand(
                    "SELECT ISNULL(SUM(PrecioTotal),0) FROM INVENTARIO.DETALLECOMPRA WHERE CompraID=@CompraID",
                    conn.conn);
                cmd.Parameters.AddWithValue("@CompraID", compraID);

                object resultado = cmd.ExecuteScalar();
                if (resultado != null && resultado != DBNull.Value)
                    total = Convert.ToDecimal(resultado);

                conn.CerrarConexion(out _);
            }
            catch (SqlException ex) { Console.WriteLine("ObtenerTotalReal: " + ex.Message); }
            catch (Exception ex)    { Console.WriteLine("ObtenerTotalReal: " + ex.Message); }

            return total;
        }

        public override void ActualizarRegistro(Compra reg, out string pError)
            => throw new NotImplementedException();
        public override Compra ObtenerPorId(int id, out string pError)
            => throw new NotImplementedException();
        public override Compra ObtenerPorId(string id, out string pError)
            => throw new NotImplementedException();
    }
}
