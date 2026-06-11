using System.Data;
using Microsoft.Data.SqlClient;

namespace DAO
{
    public class HistorialDAO
    {
        Conexion conexion = new Conexion();

        public DataTable Listar()
        {
            DataTable dt = new DataTable();
            string error = string.Empty;

            SqlConnection con = conexion.AbrirConexion(out error);

            if (con != null)
            {
                string sql = @"SELECT
                        h.historialId,
                        h.fecha,
                        h.tabla,
                        h.evento,
                        u.NombreUsuario
                       FROM SEGURIDAD.HISTORIAL h
                       INNER JOIN SEGURIDAD.USUARIO u
                            ON h.usuarioId = u.UsuarioId
                       WHERE h.Tabla = 'CLINICO.CITA'
                       ORDER BY h.Fecha DESC";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                conexion.CerrarConexion(out error);
            }

            return dt;
        }
    }
}