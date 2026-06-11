using Microsoft.Data.SqlClient;
using Modelos;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAO
{
    public class PermisosDAO
    {
        public DataTable ListarUsuarios(out string pError)
        {
            pError = string.Empty;
            DataTable tabla = new DataTable();
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                using SqlCommand cmd = new SqlCommand("SEGURIDAD.SpListarUsuariosPermisos", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;

                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }
            catch (Exception ex)
            {
                pError = ex.Message;
            }
            finally
            {
                conn.CerrarConexion(out string errorCerrar);
                if (string.IsNullOrEmpty(pError))
                    pError = errorCerrar;
            }

            return tabla;
        }

        public List<PermisoModulo> ObtenerPermisosUsuario(int usuarioID, out string pError)
        {
            pError = string.Empty;
            List<PermisoModulo> permisos = new List<PermisoModulo>();
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                using SqlCommand cmd = new SqlCommand("SEGURIDAD.SpObtenerPermisosUsuario", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);

                using SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    permisos.Add(new PermisoModulo
                    {
                        ModuloID = Convert.ToInt32(dr["ModuloID"]),
                        Codigo = dr["Codigo"]?.ToString() ?? string.Empty,
                        NombreModulo = dr["NombreModulo"]?.ToString() ?? string.Empty,
                        Permitido = Convert.ToBoolean(dr["Permitido"])
                    });
                }
            }
            catch (Exception ex)
            {
                pError = ex.Message;
            }
            finally
            {
                conn.CerrarConexion(out string errorCerrar);
                if (string.IsNullOrEmpty(pError))
                    pError = errorCerrar;
            }

            return permisos;
        }

        public HashSet<string> ObtenerCodigosPermitidos(int usuarioID, out string pError)
        {
            pError = string.Empty;
            HashSet<string> permisos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                using SqlCommand cmd = new SqlCommand("SEGURIDAD.SpObtenerPermisosDashboard", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);

                using SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    bool permitido = Convert.ToBoolean(dr["Permitido"]);
                    string codigo = dr["Codigo"]?.ToString() ?? string.Empty;

                    if (permitido && !string.IsNullOrWhiteSpace(codigo))
                    {
                        permisos.Add(codigo);
                    }
                }
            }
            catch (Exception ex)
            {
                pError = ex.Message;
            }
            finally
            {
                conn.CerrarConexion(out string errorCerrar);
                if (string.IsNullOrEmpty(pError))
                    pError = errorCerrar;
            }

            return permisos;
        }

        public void GuardarPermisosUsuario(int usuarioID, IEnumerable<PermisoModulo> permisos, int usuarioAuditoriaID, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            SqlTransaction? transaccion = null;

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                transaccion = conn.conn.BeginTransaction();

                foreach (PermisoModulo permiso in permisos)
                {
                    using SqlCommand cmd = new SqlCommand("SEGURIDAD.SpGuardarPermisoUsuario", conn.conn, transaccion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);
                    cmd.Parameters.AddWithValue("@CodigoModulo", permiso.Codigo);
                    cmd.Parameters.AddWithValue("@Permitido", permiso.Permitido);
                    cmd.Parameters.AddWithValue("@UsuarioAuditoriaID", usuarioAuditoriaID);

                    SqlParameter mensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 200)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(mensaje);

                    cmd.ExecuteNonQuery();
                }

                transaccion.Commit();
            }
            catch (Exception ex)
            {
                pError = ex.Message;

                try
                {
                    transaccion?.Rollback();
                }
                catch
                {
                }
            }
            finally
            {
                conn.CerrarConexion(out string errorCerrar);
                if (string.IsNullOrEmpty(pError))
                    pError = errorCerrar;
            }
        }
    }
}
