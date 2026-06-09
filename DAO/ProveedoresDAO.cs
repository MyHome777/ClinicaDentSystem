using Microsoft.Data.SqlClient;
using MODELOS;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAO
{
    public class ProveedoresDAO : AbstractDAO<Proveedores>
    {
        public override void ActualizarRegistro(Proveedores reg, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            SqlCommand cmd = new SqlCommand();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                cmd = new SqlCommand("INVENTARIO.sp_actualizar_proveedor", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProveedorID", reg.ProveedorID);
                cmd.Parameters.AddWithValue("@NombreProveedor", reg.NombreProveedor);
                cmd.Parameters.AddWithValue("@Contacto", reg.Contacto);
                cmd.Parameters.AddWithValue("@Telefono", reg.Telefono);
                cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(reg.Email) ? DBNull.Value : reg.Email);
                cmd.Parameters.AddWithValue("@EstadoID", reg.EstadoId);

                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                pError = msgParam.Value?.ToString() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(pError))
                {
                    pError = "No se recibio respuesta del procedimiento INVENTARIO.sp_actualizar_proveedor.";
                }

                conn.CerrarConexion(out string errorCerrar);
                if (!string.IsNullOrEmpty(errorCerrar)) pError = errorCerrar;
            }
            catch (SqlException ex)
            {
                pError = ex.Message;
                Console.WriteLine(pError);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(pError)) pError = ex.Message;
                Console.WriteLine(pError);
            }
        }

        public override void GuardarRegistro(Proveedores reg, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            SqlCommand cmd = new SqlCommand();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                cmd = new SqlCommand("SpInsertProveedor", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreProveedor", reg.NombreProveedor);
                cmd.Parameters.AddWithValue("@Contacto", reg.Contacto);
                cmd.Parameters.AddWithValue("@Telefono", reg.Telefono);
                cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(reg.Email) ? DBNull.Value : reg.Email);
                cmd.Parameters.AddWithValue("@EstadoID", reg.EstadoId);

                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                pError = msgParam.Value?.ToString() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(pError))
                {
                    pError = "No se recibio respuesta del procedimiento SpInsertProveedor.";
                }

                conn.CerrarConexion(out string errorCerrar);
                if (!string.IsNullOrEmpty(errorCerrar)) pError = errorCerrar;
            }
            catch (SqlException ex)
            {
                pError = ex.Message;
                Console.WriteLine(pError);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(pError)) pError = ex.Message;
                Console.WriteLine(pError);
            }
        }

        public override Proveedores ObtenerPorId(int id, out string pError)
        {
            throw new NotImplementedException();
        }

        public override Proveedores ObtenerPorId(string id, out string pError)
        {
            throw new NotImplementedException();
        }

        public override List<Proveedores> ObtenerTodos(out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            List<Proveedores> proveedores = new List<Proveedores>();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                cmd = new SqlCommand("SpSelectAllProveedores", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    // 🌟 OBTENER EL ÍNDICE REAL DE LAS COLUMNAS (Evita errores de Case Sensitive o nombres exactos)
                    int idxProveedorID = dr.GetOrdinal("ProveedorID");
                    int idxNombre = dr.GetOrdinal("NombreProveedor");
                    int idxContacto = dr.GetOrdinal("Contacto");
                    int idxTelefono = dr.GetOrdinal("Telefono");
                    int idxEmail = dr.GetOrdinal("Email");

                    // Buscamos la columna del Estado de forma segura
                    int idxEstadoId = -1;
                    try { idxEstadoId = dr.GetOrdinal("IdEstado"); }
                    catch { try { idxEstadoId = dr.GetOrdinal("EstadoID"); } catch { } }

                    int idxEstadoTexto = -1;
                    try { idxEstadoTexto = dr.GetOrdinal("Estado"); } catch { }

                    Proveedores proveedor = new Proveedores
                    {
                        ProveedorID = !dr.IsDBNull(idxProveedorID) ? dr.GetInt32(idxProveedorID) : 0,
                        NombreProveedor = !dr.IsDBNull(idxNombre) ? dr.GetString(idxNombre) : string.Empty,
                        Contacto = !dr.IsDBNull(idxContacto) ? dr.GetString(idxContacto) : string.Empty,
                        Telefono = !dr.IsDBNull(idxTelefono) ? dr.GetString(idxTelefono) : string.Empty,
                        Email = !dr.IsDBNull(idxEmail) ? dr.GetString(idxEmail) : string.Empty,

                        // Asignamos usando los índices numéricos seguros obtenidos arriba
                        EstadoId = (idxEstadoId != -1 && !dr.IsDBNull(idxEstadoId)) ? Convert.ToInt32(dr.GetValue(idxEstadoId)) : 0,
                        Estado = (idxEstadoTexto != -1 && !dr.IsDBNull(idxEstadoTexto)) ? dr.GetString(idxEstadoTexto) : string.Empty
                    };

                    proveedores.Add(proveedor);
                }

                if (!dr.IsClosed) dr.Close();

                conn.CerrarConexion(out string errorCerrar);
                if (!string.IsNullOrEmpty(errorCerrar)) pError = errorCerrar;
            }
            catch (SqlException ex)
            {
                pError = ex.Message;
                Console.WriteLine(pError);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(pError)) pError = ex.Message;
                Console.WriteLine(pError);
            }

            return proveedores;
        }
        public DataTable ObtenerEstados(out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            DataTable estados = new DataTable();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                cmd = new SqlCommand("SELECT EstadoID, Estado FROM INVENTARIO.ESTADO ORDER BY EstadoID", conn.conn);
                dr = cmd.ExecuteReader();
                estados.Load(dr);

                if (!dr.IsClosed) dr.Close();

                conn.CerrarConexion(out string errorCerrar);
                if (!string.IsNullOrEmpty(errorCerrar)) pError = errorCerrar;
            }
            catch (SqlException ex)
            {
                pError = ex.Message;
                Console.WriteLine(pError);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(pError)) pError = ex.Message;
                Console.WriteLine(pError);
            }

            return estados;
        }
    }
}