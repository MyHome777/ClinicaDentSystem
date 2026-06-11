using Microsoft.Data.SqlClient;
using Modelos;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAO
{
    public class UsuarioDAO : AbstractDAO<Usuario>
    {
        public override void ActualizarRegistro(Usuario reg, out string pError)
        {
            EjecutarGuardarActualizar("SEGURIDAD.SpActualizarUsuario", reg, out pError);
        }

        public override void GuardarRegistro(Usuario reg, out string pError)
        {
            EjecutarGuardarActualizar("SEGURIDAD.SpInsertarUsuario", reg, out pError);
        }

        public override Usuario ObtenerPorId(string id, out string pError)
        {
            if (!int.TryParse(id, out int usuarioID))
            {
                pError = "El ID del usuario no es valido.";
                return new Usuario();
            }

            return ObtenerPorId(usuarioID, out pError);
        }

        public Usuario Login(string nombreUsuario, string clave, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            Usuario usuario = null;

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                using SqlCommand cmd = new SqlCommand("SEGURIDAD.SpLogin", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@NombreUsuario", SqlDbType.VarChar, 50).Value = nombreUsuario.Trim();
                cmd.Parameters.Add("@Clave", SqlDbType.VarChar, 25).Value = clave.Trim();

                using SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    usuario = new Usuario
                    {
                        UsuarioId = Convert.ToInt32(dr["UsuarioId"]),
                        NombreUsuario = dr["NombreUsuario"].ToString() ?? string.Empty,
                        Clave = dr["Clave"].ToString() ?? string.Empty,
                        NombreRol = dr["NombreRol"]?.ToString() ?? string.Empty
                    };
                }

                if (usuario == null)
                    pError = "Usuario o clave incorrectos";
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
            finally
            {
                conn.CerrarConexion(out string errorCerrar);
                if (string.IsNullOrEmpty(pError) && !string.IsNullOrEmpty(errorCerrar))
                    pError = errorCerrar;
            }

            return usuario;
        }

        public override List<Usuario> ObtenerTodos(out string pError)
        {
            DataTable tabla = ListarUsuarios(out pError);
            List<Usuario> usuarios = new List<Usuario>();

            foreach (DataRow row in tabla.Rows)
            {
                usuarios.Add(MapearUsuario(row));
            }

            return usuarios;
        }

        public override Usuario ObtenerPorId(int id, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            Usuario usuario = new Usuario();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                const string sql = @"
SELECT TOP 1
    u.UsuarioID,
    u.NombreUsuario,
    u.NombreEmpleado,
    u.Clave,
    u.Email,
    u.RolID,
    r.NombreRol,
    u.EstadoID,
    e.Estado
FROM SEGURIDAD.USUARIO u
INNER JOIN SEGURIDAD.ROL r ON u.RolID = r.RolID
INNER JOIN SEGURIDAD.ESTADO e ON u.EstadoID = e.EstadoId
WHERE u.UsuarioID = @UsuarioID;";

                using SqlCommand cmd = new SqlCommand(sql, conn.conn);
                cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = id;

                using SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    usuario = new Usuario
                    {
                        UsuarioId = Convert.ToInt32(dr["UsuarioID"]),
                        NombreUsuario = dr["NombreUsuario"]?.ToString() ?? string.Empty,
                        NombreEmpleado = dr["NombreEmpleado"]?.ToString() ?? string.Empty,
                        Clave = dr["Clave"]?.ToString() ?? string.Empty,
                        Email = dr["Email"]?.ToString() ?? string.Empty,
                        RolID = Convert.ToInt32(dr["RolID"]),
                        NombreRol = dr["NombreRol"]?.ToString() ?? string.Empty,
                        EstadoID = Convert.ToInt32(dr["EstadoID"]),
                        Estado = dr["Estado"]?.ToString() ?? string.Empty
                    };
                }
                else
                {
                    pError = "No se encontro el usuario indicado.";
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
            finally
            {
                conn.CerrarConexion(out string errorCerrar);
                if (string.IsNullOrEmpty(pError) && !string.IsNullOrEmpty(errorCerrar))
                    pError = errorCerrar;
            }

            return usuario;
        }

        public DataTable ListarUsuarios(out string pError)
        {
            return EjecutarTablaSP("SEGURIDAD.SpListarUsuarios", out pError);
        }

        public DataTable BuscarUsuarios(string texto, out string pError)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return ListarUsuarios(out pError);
            }

            return EjecutarTablaSP(
                "SEGURIDAD.SpBuscarUsuarios",
                out pError,
                new SqlParameter("@TextoBusqueda", SqlDbType.VarChar, 100) { Value = texto.Trim() });
        }

        public DataTable ObtenerRoles(out string pError)
        {
            return EjecutarTablaSql(
                "SELECT RolID, NombreRol FROM SEGURIDAD.ROL ORDER BY NombreRol;",
                out pError);
        }

        public DataTable ObtenerEstados(out string pError)
        {
            return EjecutarTablaSql(
                "SELECT EstadoId, Estado FROM SEGURIDAD.ESTADO ORDER BY Estado;",
                out pError);
        }

        public void DesactivarUsuario(int usuarioID, out string pError)
        {
            Usuario usuario = ObtenerPorId(usuarioID, out pError);
            if (!string.IsNullOrEmpty(pError))
            {
                return;
            }

            DataTable estadoInactivo = EjecutarTablaSql(
                "SELECT TOP 1 EstadoId, Estado FROM SEGURIDAD.ESTADO WHERE UPPER(Estado) = 'INACTIVO';",
                out pError);

            if (!string.IsNullOrEmpty(pError))
            {
                return;
            }

            if (estadoInactivo.Rows.Count == 0)
            {
                pError = "No existe el estado INACTIVO en SEGURIDAD.ESTADO.";
                return;
            }

            usuario.EstadoID = Convert.ToInt32(estadoInactivo.Rows[0]["EstadoId"]);
            usuario.Estado = estadoInactivo.Rows[0]["Estado"]?.ToString() ?? "INACTIVO";

            ActualizarRegistro(usuario, out pError);
        }

        private void EjecutarGuardarActualizar(string sp, Usuario usuario, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                using SqlCommand cmd = new SqlCommand(sp, conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (usuario.UsuarioId > 0)
                {
                    cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = usuario.UsuarioId;
                }

                cmd.Parameters.Add("@NombreUsuario", SqlDbType.VarChar, 50).Value = usuario.NombreUsuario.Trim();
                cmd.Parameters.Add("@NombreEmpleado", SqlDbType.VarChar, 60).Value = usuario.NombreEmpleado.Trim();
                cmd.Parameters.Add("@Clave", SqlDbType.VarChar, 25).Value = usuario.Clave.Trim();
                cmd.Parameters.Add("@RolID", SqlDbType.Int).Value = usuario.RolID;
                cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = usuario.EstadoID;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = usuario.Email.Trim();
                cmd.Parameters.Add("@Estado", SqlDbType.VarChar, 15).Value = usuario.Estado.Trim();

                cmd.ExecuteNonQuery();
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
            finally
            {
                conn.CerrarConexion(out string errorCerrar);
                if (string.IsNullOrEmpty(pError) && !string.IsNullOrEmpty(errorCerrar))
                    pError = errorCerrar;
            }
        }

        private DataTable EjecutarTablaSP(string sp, out string pError, params SqlParameter[] parametros)
        {
            pError = string.Empty;
            DataTable tabla = new DataTable();
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                using SqlCommand cmd = new SqlCommand(sp, conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (parametros.Length > 0)
                {
                    cmd.Parameters.AddRange(parametros);
                }

                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
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
            finally
            {
                conn.CerrarConexion(out string errorCerrar);
                if (string.IsNullOrEmpty(pError) && !string.IsNullOrEmpty(errorCerrar))
                    pError = errorCerrar;
            }

            return tabla;
        }

        private DataTable EjecutarTablaSql(string sql, out string pError)
        {
            pError = string.Empty;
            DataTable tabla = new DataTable();
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                using SqlCommand cmd = new SqlCommand(sql, conn.conn);
                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
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
            finally
            {
                conn.CerrarConexion(out string errorCerrar);
                if (string.IsNullOrEmpty(pError) && !string.IsNullOrEmpty(errorCerrar))
                    pError = errorCerrar;
            }

            return tabla;
        }

        private static Usuario MapearUsuario(DataRow row)
        {
            return new Usuario
            {
                UsuarioId = ObtenerEntero(row, "UsuarioID"),
                NombreUsuario = ObtenerTexto(row, "NombreUsuario"),
                NombreEmpleado = ObtenerTexto(row, "NombreEmpleado"),
                Email = ObtenerTexto(row, "Email"),
                RolID = ObtenerEntero(row, "RolID"),
                NombreRol = ObtenerTexto(row, "NombreRol"),
                EstadoID = ObtenerEntero(row, "EstadoID"),
                Estado = ObtenerTexto(row, "Estado")
            };
        }

        private static int ObtenerEntero(DataRow row, string columna)
        {
            if (!row.Table.Columns.Contains(columna) || row[columna] == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(row[columna]);
        }

        private static string ObtenerTexto(DataRow row, string columna)
        {
            if (!row.Table.Columns.Contains(columna) || row[columna] == DBNull.Value)
            {
                return string.Empty;
            }

            return row[columna]?.ToString() ?? string.Empty;
        }
    }
}
