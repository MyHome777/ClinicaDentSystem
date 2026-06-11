using Microsoft.Data.SqlClient;
using Modelos;
using System;
using System.Data;

namespace DAO
{
    public class ConfiguracionCorreoDAO
    {
        public ConfiguracionCorreo ObtenerConfiguracion(out string pError)
        {
            pError = string.Empty;
            ConfiguracionCorreo configuracion = new ConfiguracionCorreo();
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                using SqlCommand cmd = new SqlCommand("SEGURIDAD.SpObtenerConfiguracionCorreoFactura", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter mensaje = CrearParametroMensaje();
                cmd.Parameters.Add(mensaje);

                using SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    configuracion.ConfiguracionCorreoID = Convert.ToInt32(dr["ConfiguracionCorreoID"]);
                    configuracion.CorreoRemitente = dr["CorreoRemitente"]?.ToString() ?? string.Empty;
                    configuracion.NombreRemitente = dr["NombreRemitente"]?.ToString() ?? string.Empty;
                    configuracion.ServidorSmtp = dr["ServidorSmtp"]?.ToString() ?? string.Empty;
                    configuracion.Puerto = Convert.ToInt32(dr["Puerto"]);
                    configuracion.UsaSsl = Convert.ToBoolean(dr["UsaSsl"]);
                    configuracion.ClaveCorreo = dr["ClaveCorreo"]?.ToString() ?? string.Empty;
                    configuracion.Activo = Convert.ToBoolean(dr["Activo"]);
                }

                dr.Close();
                ValidarMensaje(mensaje.Value?.ToString());
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

            return configuracion;
        }

        public bool GuardarConfiguracion(ConfiguracionCorreo configuracion, int usuarioID, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                using SqlCommand cmd = new SqlCommand("SEGURIDAD.SpGuardarConfiguracionCorreoFactura", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CorreoRemitente", SqlDbType.VarChar, 150).Value = configuracion.CorreoRemitente.Trim();
                cmd.Parameters.Add("@NombreRemitente", SqlDbType.VarChar, 100).Value = string.IsNullOrWhiteSpace(configuracion.NombreRemitente) ? DBNull.Value : configuracion.NombreRemitente.Trim();
                cmd.Parameters.Add("@ServidorSmtp", SqlDbType.VarChar, 150).Value = configuracion.ServidorSmtp.Trim();
                cmd.Parameters.Add("@Puerto", SqlDbType.Int).Value = configuracion.Puerto;
                cmd.Parameters.Add("@UsaSsl", SqlDbType.Bit).Value = configuracion.UsaSsl;
                cmd.Parameters.Add("@ClaveCorreo", SqlDbType.VarChar, 300).Value = configuracion.ClaveCorreo;
                cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = configuracion.Activo;
                cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = usuarioID > 0 ? usuarioID : DBNull.Value;

                SqlParameter mensaje = CrearParametroMensaje();
                cmd.Parameters.Add(mensaje);

                cmd.ExecuteNonQuery();
                pError = mensaje.Value?.ToString() ?? string.Empty;

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
                if (string.IsNullOrEmpty(pError))
                    pError = errorCerrar;
            }
        }

        private static SqlParameter CrearParametroMensaje()
        {
            return new SqlParameter("@Mensaje", SqlDbType.VarChar, 200)
            {
                Direction = ParameterDirection.Output
            };
        }

        private static void ValidarMensaje(string? mensaje)
        {
            if (string.IsNullOrWhiteSpace(mensaje))
                return;

            if (mensaje.StartsWith("Error", StringComparison.OrdinalIgnoreCase) ||
                mensaje.Contains("no existe", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception(mensaje);
            }
        }
    }
}
