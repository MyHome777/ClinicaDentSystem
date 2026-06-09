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
    public class DetalleCompraDAO : AbstractDAO<DetalleCompra>
    {
        // ── INSERTAR DETALLE ──────────────────────────────────────────────────
        // SP: spinsertDetalleCompra
        // El SP internamente:
        //   ① Calcula PrecioTotal = Cantidad × PrecioUnitarioCompra
        //   ② Actualiza StockActual del producto
        //   ③ Registra ENTRADA en INVENTARIO.MOVIMIENTOSTOCK
        //   Trigger → registra en SEGURIDAD.HISTORIAL
        public override void GuardarRegistro(DetalleCompra reg, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                SqlCommand cmd = new SqlCommand("spinsertDetalleCompra", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompraID",             reg.CompraID);
                cmd.Parameters.AddWithValue("@ProductoID",           reg.ProductoID);
                cmd.Parameters.AddWithValue("@Cantidad",             reg.Cantidad);
                cmd.Parameters.AddWithValue("@PrecioUnitarioCompra", reg.PrecioUnitarioCompra);

                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
                msgParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                pError = msgParam.Value?.ToString() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(pError))
                    pError = "No se recibio respuesta del procedimiento spinsertDetalleCompra.";

                conn.CerrarConexion(out string errCerrar);
                if (!string.IsNullOrEmpty(errCerrar)) pError = errCerrar;
            }
            catch (SqlException ex) { pError = ex.Message; Console.WriteLine(pError); }
            catch (Exception ex)    { if (string.IsNullOrEmpty(pError)) pError = ex.Message; Console.WriteLine(pError); }
        }

        // ── OBTENER DETALLE POR COMPRA ────────────────────────────────────────
        // SP: spSelectConsultaCompra
        public List<DetalleCompra> ObtenerPorCompra(int compraID, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            List<DetalleCompra> lista = new List<DetalleCompra>();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                SqlCommand cmd = new SqlCommand("spSelectConsultaCompra", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompraID", compraID);

                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
                msgParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgParam);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    DetalleCompra d = new DetalleCompra();
                    d.DetalleCompraID      = dr.GetInt32(0);
                    d.CompraID             = dr.GetInt32(1);
                    d.ProductoID           = dr.GetInt32(2);
                    d.NombreProducto       = dr.IsDBNull(3) ? string.Empty : dr.GetString(3);
                    d.Cantidad             = dr.GetInt32(4);
                    d.PrecioUnitarioCompra = dr.GetDecimal(5);
                    d.PrecioTotal          = dr.GetDecimal(6);
                    lista.Add(d);
                }

                if (!dr.IsClosed) dr.Close();
                conn.CerrarConexion(out string errCerrar);
                if (!string.IsNullOrEmpty(errCerrar)) pError = errCerrar;
            }
            catch (SqlException ex) { pError = ex.Message; Console.WriteLine(pError); }
            catch (Exception ex)    { if (string.IsNullOrEmpty(pError)) pError = ex.Message; Console.WriteLine(pError); }

            return lista;
        }

        public override void ActualizarRegistro(DetalleCompra reg, out string pError)
            => throw new NotImplementedException();
        public override DetalleCompra ObtenerPorId(int id, out string pError)
            => throw new NotImplementedException();
        public override DetalleCompra ObtenerPorId(string id, out string pError)
            => throw new NotImplementedException();
        public override List<DetalleCompra> ObtenerTodos(out string pError)
            => throw new NotImplementedException();
    }
}
