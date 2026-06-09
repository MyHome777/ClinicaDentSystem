using Microsoft.Data.SqlClient;
using Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class UsuarioDAO : AbstractDAO<Usuario>
    {
        public UsuarioDAO()
        {
        }

        public override void ActualizarRegistro(Usuario reg, out string pError)
        {
            throw new NotImplementedException();
        }

        public override void GuardarRegistro(Usuario reg, out string pError)
        {
            throw new NotImplementedException();
        }

        public override Usuario ObtenerPorId(string id, out string pError)
        {
            throw new NotImplementedException();
        }

        public Usuario Login(string nombreUsuario, string clave, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            SqlCommand cmd;
            SqlDataReader dr;
            Usuario usuario = null;

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                // ← Solo estas líneas cambian vs el lic
                cmd = new SqlCommand("SEGURIDAD.SpLogin", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                cmd.Parameters.AddWithValue("@Clave", clave);

                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    usuario = new Usuario();
                    usuario.UsuarioId = Convert.ToInt32(dr["UsuarioId"]);
                    usuario.NombreUsuario = dr["NombreUsuario"].ToString() ?? string.Empty;
                    usuario.Clave = dr["Clave"].ToString() ?? string.Empty;
                    usuario.NombreRol = dr["NombreRol"]?.ToString() ?? string.Empty;
                }

                if (usuario == null)
                    pError = "Usuario o clave incorrectos";

                dr.Close();
                conn.CerrarConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);
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
            return usuario;
        }

        public override List<Usuario> ObtenerTodos(out string pError)
        {
            throw new NotImplementedException();
        }

        public override Usuario ObtenerPorId(int id, out string pError)
        {
            throw new NotImplementedException();
        }
    }
}
