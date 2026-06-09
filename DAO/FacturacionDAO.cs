using Microsoft.Data.SqlClient;
using MODELOS;
using System.Data;

namespace DAO
{
    public class FacturacionDAO
    {
        public DataTable ObtenerCitas(out string pError)
        {
            return EjecutarTablaSP("dbo.spFacturacionCitasDisponibles", out pError);
        }

        public DataTable ObtenerEstadosFactura(out string pError)
        {
            return EjecutarTablaSP("dbo.spFacturacionEstadosEmision", out pError);
        }

        public DataTable ObtenerUsuarios(out string pError)
        {
            return EjecutarTablaSP("dbo.spFacturacionUsuarios", out pError);
        }

        public DataTable ObtenerDescuentos(out string pError)
        {
            return EjecutarTablaSP("dbo.spFacturacionDescuentosVigentes", out pError);
        }

        public DataTable ObtenerItemsFacturables(string tipo, out string pError)
        {
            return EjecutarTablaSP(
                "dbo.spFacturacionItemsFacturables",
                out pError,
                new SqlParameter("@Tipo", SqlDbType.VarChar, 20) { Value = tipo });
        }

        public bool EmitirFactura(FacturaEmision factura, out int facturaID, out string pError)
        {
            pError = string.Empty;
            facturaID = 0;
            Conexion conn = new Conexion();
            SqlTransaction? transaccion = null;

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                if (factura.Detalles.Count == 0)
                    throw new Exception("Debe agregar al menos un detalle a la factura.");

                transaccion = conn.conn.BeginTransaction();

                int estadoSolicitadoID = factura.EstadoId;
                bool marcarComoPagada = EstadoFacturaEs(conn.conn, transaccion, estadoSolicitadoID, "PAGADA");
                if (marcarComoPagada)
                    factura.EstadoId = ObtenerEstadoFacturaID(conn.conn, transaccion, "EMITIDA");

                ValidarStockProductos(conn.conn, transaccion, factura.Detalles);
                DescuentoFacturacion descuento = ObtenerDescuentoFacturacion(conn.conn, transaccion, factura.DescuentoID);

                decimal subtotal = factura.Detalles.Sum(d => d.Subtotal);
                decimal totalDescuento = factura.Detalles
                    .Where(descuento.AplicaA)
                    .Sum(d => d.Subtotal * descuento.Porcentaje / 100m);
                decimal total = subtotal - totalDescuento;
                int? descuentoFacturaID = factura.Detalles.Any(descuento.AplicaA) ? descuento.DescuentoID : null;

                facturaID = InsertarFactura(conn.conn, transaccion, factura, subtotal, total, descuentoFacturaID);

                foreach (FacturaDetalle detalle in factura.Detalles)
                {
                    int? descuentoDetalleID = descuento.AplicaA(detalle) ? descuento.DescuentoID : null;
                    InsertarDetalleFactura(conn.conn, transaccion, facturaID, detalle, descuentoDetalleID);
                }

                if (marcarComoPagada)
                    ActualizarEstadoFactura(conn.conn, transaccion, facturaID, estadoSolicitadoID);

                transaccion.Commit();
                return true;
            }
            catch (Exception ex)
            {
                string errorOriginal = ex.Message;

                try
                {
                    transaccion?.Rollback();
                }
                catch (Exception rollbackEx)
                {
                    if (string.IsNullOrEmpty(errorOriginal))
                        errorOriginal = rollbackEx.Message;
                }

                pError = errorOriginal;

                return false;
            }
            finally
            {
                conn.CerrarConexion(out string errorCerrar);
                if (!string.IsNullOrEmpty(errorCerrar) && string.IsNullOrEmpty(pError))
                    pError = errorCerrar;
            }
        }

        public DataTable ObtenerFacturasEmitidas(string buscar, string? estado, DateTime fechaDesde, DateTime fechaHasta, out string pError)
        {
            return EjecutarTablaSP(
                "dbo.spSelectFacturas",
                out pError,
                new SqlParameter("@Buscar", SqlDbType.VarChar, 100) { Value = string.IsNullOrWhiteSpace(buscar) ? DBNull.Value : buscar.Trim() },
                new SqlParameter("@Estado", SqlDbType.VarChar, 20) { Value = string.IsNullOrWhiteSpace(estado) ? DBNull.Value : estado },
                new SqlParameter("@FechaDesde", SqlDbType.Date) { Value = fechaDesde.Date },
                new SqlParameter("@FechaHasta", SqlDbType.Date) { Value = fechaHasta.Date });
        }

        public DataTable ObtenerDetalleFactura(int facturaID, out string pError)
        {
            return EjecutarTablaSP(
                "dbo.spSelectDetalleFactura",
                out pError,
                new SqlParameter("@FacturaID", SqlDbType.Int) { Value = facturaID });
        }

        public bool AnularFactura(int facturaID, int usuarioID, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                using SqlCommand cmd = CrearComandoSP("dbo.spAnularFactura", conn.conn);
                cmd.Parameters.Add("@FacturaID", SqlDbType.Int).Value = facturaID;
                cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = usuarioID;

                SqlParameter msgParam = CrearParametroMensaje();
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                pError = msgParam.Value?.ToString() ?? string.Empty;

                return pError.Contains("correctamente", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                pError = ex.Message;
                return false;
            }
            finally
            {
                conn.CerrarConexion(out string errorCerrar);
                if (!string.IsNullOrEmpty(errorCerrar) && string.IsNullOrEmpty(pError))
                    pError = errorCerrar;
            }
        }

        public bool MarcarFacturaComoPagada(int facturaID, int usuarioID, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                using SqlCommand cmd = CrearComandoSP("dbo.spMarcarFacturaPagada", conn.conn);
                cmd.Parameters.Add("@FacturaID", SqlDbType.Int).Value = facturaID;
                cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = usuarioID;

                SqlParameter msgParam = CrearParametroMensaje();
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                pError = msgParam.Value?.ToString() ?? string.Empty;

                return pError.Contains("correctamente", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                pError = ex.Message;
                return false;
            }
            finally
            {
                conn.CerrarConexion(out string errorCerrar);
                if (!string.IsNullOrEmpty(errorCerrar) && string.IsNullOrEmpty(pError))
                    pError = errorCerrar;
            }
        }

        private static int InsertarFactura(SqlConnection conn, SqlTransaction transaccion, FacturaEmision factura, decimal subtotal, decimal total, int? descuentoID)
        {
            using SqlCommand cmd = CrearComandoSP("dbo.spInsertFactura", conn, transaccion);
            cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = factura.UsuarioID;
            cmd.Parameters.Add("@CitaID", SqlDbType.Int).Value = factura.CitaID;
            cmd.Parameters.Add("@FechaEmision", SqlDbType.DateTime).Value = factura.FechaEmision;
            cmd.Parameters.Add("@Subtotal", SqlDbType.Decimal).Value = subtotal;
            cmd.Parameters.Add("@DescuentoID", SqlDbType.Int).Value = descuentoID.HasValue ? descuentoID.Value : DBNull.Value;
            cmd.Parameters.Add("@Total", SqlDbType.Decimal).Value = total;
            cmd.Parameters.Add("@EstadoId", SqlDbType.Int).Value = factura.EstadoId;

            SqlParameter facturaIdParam = new SqlParameter("@FacturaID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(facturaIdParam);

            SqlParameter msgParam = CrearParametroMensaje();
            cmd.Parameters.Add(msgParam);

            cmd.ExecuteNonQuery();
            ValidarMensajeSP(msgParam.Value?.ToString());

            return Convert.ToInt32(facturaIdParam.Value);
        }

        private static void InsertarDetalleFactura(SqlConnection conn, SqlTransaction transaccion, int facturaID, FacturaDetalle detalle, int? descuentoID)
        {
            using SqlCommand cmd = CrearComandoSP("dbo.sp_InsertDetalleFactura", conn, transaccion);
            cmd.Parameters.Add("@FacturaID", SqlDbType.Int).Value = facturaID;
            cmd.Parameters.Add("@ServicioID", SqlDbType.Int).Value = detalle.ServicioID.HasValue ? detalle.ServicioID.Value : DBNull.Value;
            cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = detalle.ProductoID.HasValue ? detalle.ProductoID.Value : DBNull.Value;
            cmd.Parameters.Add("@Cantidad", SqlDbType.Int).Value = detalle.Cantidad;
            cmd.Parameters.Add("@DescuentoID", SqlDbType.Int).Value = descuentoID.HasValue ? descuentoID.Value : DBNull.Value;
            cmd.Parameters.Add("@PrecioAplicado", SqlDbType.Decimal).Value = detalle.PrecioAplicado;

            SqlParameter msgParam = CrearParametroMensaje();
            cmd.Parameters.Add(msgParam);

            cmd.ExecuteNonQuery();
            ValidarMensajeSP(msgParam.Value?.ToString());
        }

        private static bool EstadoFacturaEs(SqlConnection conn, SqlTransaction transaccion, int estadoID, string estado)
        {
            using SqlCommand cmd = new SqlCommand(
                "SELECT Estado FROM FACTURACION.ESTADOFACTURA WHERE EstadoId = @EstadoId",
                conn,
                transaccion);
            cmd.Parameters.Add("@EstadoId", SqlDbType.Int).Value = estadoID;

            object? resultado = cmd.ExecuteScalar();
            if (resultado == null || resultado == DBNull.Value)
                throw new Exception("Error: El estado de factura no existe");

            return string.Equals(resultado.ToString(), estado, StringComparison.OrdinalIgnoreCase);
        }

        private static int ObtenerEstadoFacturaID(SqlConnection conn, SqlTransaction transaccion, string estado)
        {
            using SqlCommand cmd = new SqlCommand(
                "SELECT EstadoId FROM FACTURACION.ESTADOFACTURA WHERE UPPER(Estado) = @Estado",
                conn,
                transaccion);
            cmd.Parameters.Add("@Estado", SqlDbType.VarChar, 20).Value = estado.ToUpperInvariant();

            object? resultado = cmd.ExecuteScalar();
            if (resultado == null || resultado == DBNull.Value)
                throw new Exception($"Error: No existe el estado {estado} en FACTURACION.ESTADOFACTURA");

            return Convert.ToInt32(resultado);
        }

        private static void ActualizarEstadoFactura(SqlConnection conn, SqlTransaction transaccion, int facturaID, int estadoID)
        {
            using SqlCommand cmd = new SqlCommand(
                "UPDATE FACTURACION.FACTURA SET EstadoId = @EstadoId WHERE FacturaID = @FacturaID",
                conn,
                transaccion);
            cmd.Parameters.Add("@EstadoId", SqlDbType.Int).Value = estadoID;
            cmd.Parameters.Add("@FacturaID", SqlDbType.Int).Value = facturaID;
            cmd.ExecuteNonQuery();
        }

        private static DescuentoFacturacion ObtenerDescuentoFacturacion(SqlConnection conn, SqlTransaction transaccion, int? descuentoID)
        {
            DescuentoFacturacion descuento = new DescuentoFacturacion();

            if (!descuentoID.HasValue)
                return descuento;

            using SqlCommand cmd = CrearComandoSP("dbo.spFacturacionObtenerPorcentajeDescuento", conn, transaccion);
            cmd.Parameters.Add("@DescuentoID", SqlDbType.Int).Value = descuentoID.Value;

            SqlParameter porcentajeParam = new SqlParameter("@Porcentaje", SqlDbType.Decimal)
            {
                Direction = ParameterDirection.Output,
                Precision = 10,
                Scale = 2
            };
            cmd.Parameters.Add(porcentajeParam);

            SqlParameter msgParam = CrearParametroMensaje();
            cmd.Parameters.Add(msgParam);

            SqlParameter servicioParam = new SqlParameter("@ServicioID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(servicioParam);

            SqlParameter productoParam = new SqlParameter("@ProductoID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(productoParam);

            cmd.ExecuteNonQuery();
            ValidarMensajeSP(msgParam.Value?.ToString());

            descuento.DescuentoID = descuentoID.Value;
            descuento.Porcentaje = porcentajeParam.Value == DBNull.Value ? 0m : Convert.ToDecimal(porcentajeParam.Value);
            descuento.ServicioID = servicioParam.Value == DBNull.Value ? null : Convert.ToInt32(servicioParam.Value);
            descuento.ProductoID = productoParam.Value == DBNull.Value ? null : Convert.ToInt32(productoParam.Value);

            return descuento;
        }

        private static void ValidarStockProductos(SqlConnection conn, SqlTransaction transaccion, List<FacturaDetalle> detalles)
        {
            var productos = detalles
                .Where(d => d.ProductoID.HasValue)
                .GroupBy(d => d.ProductoID!.Value)
                .Select(g => new { ProductoID = g.Key, Cantidad = g.Sum(d => d.Cantidad) });

            foreach (var producto in productos)
            {
                using SqlCommand cmd = CrearComandoSP("dbo.spFacturacionValidarStockProducto", conn, transaccion);
                cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = producto.ProductoID;
                cmd.Parameters.Add("@Cantidad", SqlDbType.Int).Value = producto.Cantidad;

                SqlParameter msgParam = CrearParametroMensaje();
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                ValidarMensajeSP(msgParam.Value?.ToString());
            }
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

                using SqlCommand cmd = CrearComandoSP(procedimiento, conn.conn);
                if (parametros.Length > 0)
                    cmd.Parameters.AddRange(parametros);

                SqlParameter msgParam = CrearParametroMensaje();
                cmd.Parameters.Add(msgParam);

                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
                ValidarMensajeSP(msgParam.Value?.ToString());
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

        private static SqlCommand CrearComandoSP(string procedimiento, SqlConnection conn, SqlTransaction? transaccion = null)
        {
            return new SqlCommand(procedimiento, conn, transaccion)
            {
                CommandType = CommandType.StoredProcedure
            };
        }

        private static SqlParameter CrearParametroMensaje()
        {
            return new SqlParameter("@Mensaje", SqlDbType.VarChar, 100)
            {
                Direction = ParameterDirection.Output
            };
        }

        private static void ValidarMensajeSP(string? mensaje)
        {
            if (string.IsNullOrWhiteSpace(mensaje))
                return;

            if (mensaje.StartsWith("Error", StringComparison.OrdinalIgnoreCase) ||
                mensaje.StartsWith("ERROR", StringComparison.OrdinalIgnoreCase) ||
                mensaje.StartsWith("Stock insuficiente", StringComparison.OrdinalIgnoreCase) ||
                mensaje.Contains("no existe", StringComparison.OrdinalIgnoreCase) ||
                mensaje.Contains("no esta vigente", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception(mensaje);
            }
        }

        private sealed class DescuentoFacturacion
        {
            public int? DescuentoID { get; set; }
            public int? ServicioID { get; set; }
            public int? ProductoID { get; set; }
            public decimal Porcentaje { get; set; }

            public bool AplicaA(FacturaDetalle detalle)
            {
                if (!DescuentoID.HasValue || Porcentaje <= 0)
                    return false;

                if (ServicioID.HasValue)
                    return detalle.ServicioID == ServicioID.Value;

                if (ProductoID.HasValue)
                    return detalle.ProductoID == ProductoID.Value;

                return false;
            }
        }
    }
}
