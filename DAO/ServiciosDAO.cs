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
    public class ServiciosDAO
    {
        Conexion conexion = new Conexion();

        public DataTable Listar()
        {
            string error = string.Empty;
            SqlConnection con = conexion.AbrirConexion(out error);
            DataTable dt = new DataTable();

            if (con != null)
            {
                SqlCommand cmd = new SqlCommand("dbo.SpReadServicio", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conexion.CerrarConexion(out error);
            }
            return dt;
        }

        public void Insertar(Servicios s)
        {
            string error = string.Empty;
            SqlConnection con = conexion.AbrirConexion(out error);

            if (con != null)
            {
                string sql = @"INSERT INTO CLINICO.SERVICIOS (NombreServicio, Descripcion, precio, EstadoId) 
                       VALUES (@n, @d, @p, @es)";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@n", s.NombreServicio);
                cmd.Parameters.AddWithValue("@d", s.Descripcion);
                cmd.Parameters.AddWithValue("@p", s.Precio);
                cmd.Parameters.AddWithValue("@es", s.EstadoID);
                cmd.ExecuteNonQuery();

                conexion.CerrarConexion(out error);
            }
            else
            {
                throw new Exception("No se pudo conectar a la base de datos: " + error);
            }
        }

        public void Actualizar(Servicios s)
        {
            string error = string.Empty;
            SqlConnection con = conexion.AbrirConexion(out error);

            if (con != null)
            {
                try
                {
                    string sql = @"UPDATE CLINICO.SERVICIOS SET 
                           NombreServicio = @n, Descripcion = @d, Precio = @p, EstadoId = @es 
                           WHERE ServicioID = @id";

                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@n", s.NombreServicio);
                    cmd.Parameters.AddWithValue("@d", s.Descripcion);
                    cmd.Parameters.AddWithValue("@p", s.Precio);
                    cmd.Parameters.AddWithValue("@es", s.EstadoID);
                    cmd.Parameters.AddWithValue("@id", s.ServicioID);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar: " + ex.Message);
                }
                finally
                {
                    conexion.CerrarConexion(out error);
                }
            }
            else
            {
                throw new Exception("No se pudo conectar a la BD: " + error);
            }
        }
        public void Eliminar(int id)
        {
            string error = string.Empty;
            SqlConnection con = conexion.AbrirConexion(out error);

            if (con != null)
            {
                SqlCommand cmd = new SqlCommand("dbo.SpDeleteServicio", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServicioID", id);
                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                string mensaje = msgParam.Value?.ToString() ?? string.Empty;
                conexion.CerrarConexion(out error);

                if (mensaje.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception(mensaje);
                }
            }
        }
        public DataTable Buscar(string texto)
        {
            string error = string.Empty;
            SqlConnection con = conexion.AbrirConexion(out error);
            DataTable dt = new DataTable();

            if (con != null)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.SpSearchServicio", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Texto", texto);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                finally
                {
                    conexion.CerrarConexion(out error);
                }
            }
            return dt;
        }
    }
}
