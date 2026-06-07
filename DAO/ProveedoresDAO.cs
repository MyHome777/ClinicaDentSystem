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
                if (!string.IsNullOrEmpty(pError))
                {
                    throw new Exception(pError);
                }

                cmd = new SqlCommand("INVENTARIO.sp_actualizar_proveedor", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProveedorID", reg.ProveedorID);
                cmd.Parameters.AddWithValue("@NombreProveedor", reg.NombreProveedor);
                cmd.Parameters.AddWithValue("@Contacto", reg.Contacto);
                cmd.Parameters.AddWithValue("@Telefono", reg.Telefono);
                cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(reg.Email) ? DBNull.Value : reg.Email);
                cmd.Parameters.AddWithValue("@EstadoId", reg.EstadoId);

                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
                msgParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                pError = msgParam.Value?.ToString() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(pError))
                {
                    pError = "No se recibio respuesta del procedimiento INVENTARIO.sp_actualizar_proveedor.";
                }

                conn.CerrarConexion(out string errorCerrar);
                if (!string.IsNullOrEmpty(errorCerrar))
                {
                    pError = errorCerrar;
                }
            }
            catch (SqlException ex)
            {
                pError = ex.Message;
                Console.WriteLine(pError);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(pError))
                    pError = ex.Message;
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
                if (!string.IsNullOrEmpty(pError))
                {
                    throw new Exception(pError);
                }

                cmd = new SqlCommand("SpInsertProveedor", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreProveedor", reg.NombreProveedor);
                cmd.Parameters.AddWithValue("@Contacto", reg.Contacto);
                cmd.Parameters.AddWithValue("@Telefono", reg.Telefono);
                cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(reg.Email) ? DBNull.Value : reg.Email);
                cmd.Parameters.AddWithValue("@EstadoId", reg.EstadoId);

                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
                msgParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                pError = msgParam.Value?.ToString() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(pError))
                {
                    pError = "No se recibio respuesta del procedimiento SpInsertProveedor.";
                }

                conn.CerrarConexion(out string errorCerrar);
                if (!string.IsNullOrEmpty(errorCerrar))
                {
                    pError = errorCerrar;
                }
            }
            catch (SqlException ex)
            {
                pError = ex.Message;
                Console.WriteLine(pError);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(pError))
                    pError = ex.Message;
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
                if (!string.IsNullOrEmpty(pError))
                {
                    throw new Exception(pError);
                }

                cmd = new SqlCommand("SpSelectAllProveedores", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Proveedores proveedor = new Proveedores();
                    proveedor.ProveedorID = dr.GetInt32(0);
                    proveedor.NombreProveedor = dr.GetString(1);
                    proveedor.Contacto = dr.GetString(2);
                    proveedor.Telefono = dr.GetString(3);
                    proveedor.Email = dr.IsDBNull(4) ? string.Empty : dr.GetString(4);
                    proveedor.EstadoId = dr.GetInt32(5);
                    proveedor.Estado = dr.GetString(6);
                    proveedores.Add(proveedor);
                }

                if (!dr.IsClosed)
                {
                    dr.Close();
                }

                conn.CerrarConexion(out string errorCerrar);
                if (!string.IsNullOrEmpty(errorCerrar))
                {
                    pError = errorCerrar;
                }
            }
            catch (SqlException ex)
            {
                pError = ex.Message;
                Console.WriteLine(pError);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(pError))
                    pError = ex.Message;
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
                if (!string.IsNullOrEmpty(pError))
                {
                    throw new Exception(pError);
                }

                cmd = new SqlCommand("SELECT EstadoId, Estado FROM INVENTARIO.ESTADO ORDER BY EstadoId", conn.conn);
                dr = cmd.ExecuteReader();
                estados.Load(dr);

                if (!dr.IsClosed)
                {
                    dr.Close();
                }

                conn.CerrarConexion(out string errorCerrar);
                if (!string.IsNullOrEmpty(errorCerrar))
                {
                    pError = errorCerrar;
                }
            }
            catch (SqlException ex)
            {
                pError = ex.Message;
                Console.WriteLine(pError);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(pError))
                    pError = ex.Message;
                Console.WriteLine(pError);
            }

            return estados;
        }
    }
}
