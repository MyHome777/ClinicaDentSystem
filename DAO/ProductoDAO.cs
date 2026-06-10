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
    public class ProductoDAO : AbstractDAO<Producto>
    {
        // ── GUARDAR (sin retorno ID) ──────────────────────────────────────────
        public override void GuardarRegistro(Producto reg, out string pError)
        {
            GuardarRegistroConId(reg, out pError);
        }

        // ── GUARDAR con retorno de ProductoID ─────────────────────────────────
        // SP: INVENTARIO.sp_crear_producto
        public int GuardarRegistroConId(Producto reg, out string pError)
        {
            pError = string.Empty;
            int productoID = 0;
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                using SqlCommand cmd = new SqlCommand("INVENTARIO.sp_crear_producto", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoriaID", reg.CategoriaID);
                cmd.Parameters.AddWithValue("@NombreProducto", reg.NombreProducto);
                cmd.Parameters.AddWithValue("@Descripcion", string.IsNullOrWhiteSpace(reg.Descripcion) ? DBNull.Value : reg.Descripcion);
                cmd.Parameters.AddWithValue("@UnidadMedida", reg.UnidadMedida);
                cmd.Parameters.AddWithValue("@StockActual", reg.StockActual);
                cmd.Parameters.AddWithValue("@StockMinimo", reg.StockMinimo);
                cmd.Parameters.AddWithValue("@PrecioUnitario", reg.PrecioUnitario);
                cmd.Parameters.AddWithValue("@FechaVencimiento", reg.FechaVencimiento);
                cmd.Parameters.AddWithValue("@EstadoID", reg.EstadoID);

                SqlParameter outId = new SqlParameter("@ProductoID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outId);

                cmd.ExecuteNonQuery();

                if (outId.Value != null && outId.Value != DBNull.Value)
                {
                    productoID = Convert.ToInt32(outId.Value);
                    pError = "Producto creado correctamente";
                }
                else
                {
                    pError = "No se obtuvo el ID del producto.";
                }

                conn.CerrarConexion(out string errCerrar);
                if (!string.IsNullOrEmpty(errCerrar)) pError = errCerrar;
            }
            catch (SqlException ex) { pError = ex.Message; Console.WriteLine(pError); }
            catch (Exception ex)    { if (string.IsNullOrEmpty(pError)) pError = ex.Message; Console.WriteLine(pError); }

            return productoID;
        }

        // ── LISTAR TODOS ──────────────────────────────────────────────────────
        // SP: INVENTARIO.sp_listar_productos
        public override List<Producto> ObtenerTodos(out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            List<Producto> lista = new List<Producto>();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                SqlCommand cmd = new SqlCommand("INVENTARIO.sp_listar_productos", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Producto p = new Producto();
                    p.ProductoID = dr.GetInt32(dr.GetOrdinal("ProductoID"));
                    p.VentaID = ObtenerEnteroOpcional(dr, "VentaID");
                    p.Categoria = dr.IsDBNull(dr.GetOrdinal("Categoria")) ? string.Empty : dr.GetString(dr.GetOrdinal("Categoria"));
                    p.NombreProducto = dr.IsDBNull(dr.GetOrdinal("NombreProducto")) ? string.Empty : dr.GetString(dr.GetOrdinal("NombreProducto"));
                    p.Descripcion = dr.IsDBNull(dr.GetOrdinal("Descripcion")) ? string.Empty : dr.GetString(dr.GetOrdinal("Descripcion"));
                    p.UnidadMedida = dr.IsDBNull(dr.GetOrdinal("UnidadMedida")) ? string.Empty : dr.GetString(dr.GetOrdinal("UnidadMedida"));
                    p.StockActual = dr.GetInt32(dr.GetOrdinal("StockActual"));
                    p.StockMinimo = dr.GetInt32(dr.GetOrdinal("StockMinimo"));
                    p.PrecioUnitario = dr.GetDecimal(dr.GetOrdinal("PrecioUnitario"));
                    p.FechaVencimiento = dr.GetDateTime(dr.GetOrdinal("FechaVencimiento"));
                    p.Estado = dr.IsDBNull(dr.GetOrdinal("Estado")) ? string.Empty : dr.GetString(dr.GetOrdinal("Estado"));
                    lista.Add(p);
                }

                if (!dr.IsClosed) dr.Close();
                conn.CerrarConexion(out string errCerrar);
                if (!string.IsNullOrEmpty(errCerrar)) pError = errCerrar;
            }
            catch (SqlException ex) { pError = ex.Message; Console.WriteLine(pError); }
            catch (Exception ex)    { if (string.IsNullOrEmpty(pError)) pError = ex.Message; Console.WriteLine(pError); }

            return lista;
        }

        private static int ObtenerEnteroOpcional(SqlDataReader dr, string columna)
        {
            try
            {
                int ordinal = dr.GetOrdinal(columna);
                return dr.IsDBNull(ordinal) ? 0 : Convert.ToInt32(dr.GetValue(ordinal));
            }
            catch (IndexOutOfRangeException)
            {
                return 0;
            }
        }

        public override void ActualizarRegistro(Producto reg, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                using SqlCommand cmd = new SqlCommand(@"
UPDATE INVENTARIO.PRODUCTO
SET
    CategoriaID = @CategoriaID,
    NombreProducto = @NombreProducto,
    Descripcion = @Descripcion,
    UnidadMedida = @UnidadMedida,
    StockActual = @StockActual,
    StockMinimo = @StockMinimo,
    PrecioUnitario = @PrecioUnitario,
    FechaVencimiento = @FechaVencimiento,
    EstadoID = @EstadoID
WHERE ProductoID = @ProductoID;", conn.conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ProductoID", reg.ProductoID);
                cmd.Parameters.AddWithValue("@CategoriaID", reg.CategoriaID);
                cmd.Parameters.AddWithValue("@NombreProducto", reg.NombreProducto);
                cmd.Parameters.AddWithValue("@Descripcion", string.IsNullOrWhiteSpace(reg.Descripcion) ? DBNull.Value : reg.Descripcion);
                cmd.Parameters.AddWithValue("@UnidadMedida", reg.UnidadMedida);
                cmd.Parameters.AddWithValue("@StockActual", reg.StockActual);
                cmd.Parameters.AddWithValue("@StockMinimo", reg.StockMinimo);
                cmd.Parameters.AddWithValue("@PrecioUnitario", reg.PrecioUnitario);
                cmd.Parameters.AddWithValue("@FechaVencimiento", reg.FechaVencimiento);
                cmd.Parameters.AddWithValue("@EstadoID", reg.EstadoID);

                cmd.ExecuteNonQuery();
                pError = "Producto actualizado correctamente";

                conn.CerrarConexion(out string errCerrar);
                if (!string.IsNullOrEmpty(errCerrar)) pError = errCerrar;
            }
            catch (SqlException ex) { pError = ex.Message; Console.WriteLine(pError); }
            catch (Exception ex) { if (string.IsNullOrEmpty(pError)) pError = ex.Message; Console.WriteLine(pError); }
        }
        public override Producto ObtenerPorId(int id, out string pError)
            => throw new NotImplementedException();
        public override Producto ObtenerPorId(string id, out string pError)
            => throw new NotImplementedException();

        public void Eliminar(int productoID, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                using SqlCommand chk = new SqlCommand(@"
SELECT
    (SELECT COUNT(1) FROM INVENTARIO.DETALLECOMPRA WHERE ProductoID = @ProductoID) +
    (SELECT COUNT(1) FROM INVENTARIO.MOVIMIENTOSTOCK WHERE ProductoID = @ProductoID) +
    (SELECT COUNT(1) FROM FACTURACION.DETALLEFACTURA WHERE ProductoID = @ProductoID);", conn.conn);
                chk.Parameters.AddWithValue("@ProductoID", productoID);
                int referencias = Convert.ToInt32(chk.ExecuteScalar());
                if (referencias > 0)
                {
                    pError = "No se puede eliminar porque el producto tiene registros relacionados.";
                    conn.CerrarConexion(out _);
                    return;
                }

                using SqlCommand cmd = new SqlCommand("DELETE FROM INVENTARIO.PRODUCTO WHERE ProductoID = @ProductoID;", conn.conn);
                cmd.Parameters.AddWithValue("@ProductoID", productoID);
                cmd.ExecuteNonQuery();
                pError = "Producto eliminado correctamente";

                conn.CerrarConexion(out string errCerrar);
                if (!string.IsNullOrEmpty(errCerrar)) pError = errCerrar;
            }
            catch (SqlException ex) { pError = ex.Message; Console.WriteLine(pError); }
            catch (Exception ex) { if (string.IsNullOrEmpty(pError)) pError = ex.Message; Console.WriteLine(pError); }
        }
    }
}
