using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class AuditoriaDAO
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
                       WHERE h.Tabla <> 'CLINICO.CITA'
                       ORDER BY h.Fecha DESC";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);
                HistorialEventoFormatter.AplicarNombres(dt, con);

                conexion.CerrarConexion(out error);
            }

            return dt;
        }
    }
}
