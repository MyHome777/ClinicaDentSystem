using MODELOS;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DAO;

namespace DAO
{
    public class DentistaDAO
    {
        Conexion conexion = new Conexion();
        public void Insertar(Dentista d)
        {
            string error = string.Empty;
            SqlConnection con = conexion.AbrirConexion(out error);

            if (con != null)
            {
                // Asegúrate de que estos nombres de columnas coincidan exactamente con tu BD
                string sql = @"INSERT INTO CLINICO.DENTISTA (TipoDocID, NumDocumento, Nombre, Apellido, Telefono, Email, LicenciaMedica, EstadoID) 
                       VALUES (@td, @nd, @n, @a, @t, @e, @lm, @es)";

                SqlCommand cmd = new SqlCommand(sql, con);

                // AQUÍ ESTABA EL ERROR: Te faltaban parámetros y los nombres no coincidían con la consulta
                cmd.Parameters.AddWithValue("@td", d.TipoDocID);
                cmd.Parameters.AddWithValue("@nd", d.NumDocumento);
                cmd.Parameters.AddWithValue("@n", d.Nombre);
                cmd.Parameters.AddWithValue("@a", d.Apellido);
                cmd.Parameters.AddWithValue("@t", d.Telefono);
                cmd.Parameters.AddWithValue("@e", d.Email);
                cmd.Parameters.AddWithValue("@lm", d.LicenciaMedica);
                cmd.Parameters.AddWithValue("@es", d.EstadoID);

                cmd.ExecuteNonQuery();
                conexion.CerrarConexion(out error);
            }
            else
            {
                throw new Exception("Error al conectar: " + error);
            }
        }
        public void Actualizar(Dentista d)
        {
            string error = string.Empty;
            SqlConnection con = conexion.AbrirConexion(out error);

            if (con != null)
            {
                string sql = @"UPDATE CLINICO.DENTISTA SET 
                        TipoDocID = @td, 
                        NumDocumento = @nd, 
                        Nombre = @n, 
                        Apellido = @a, 
                        Telefono = @t, 
                        Email = @e, 
                        LicenciaMedica = @lm, 
                        EstadoID = @es 
                      WHERE DentistaID = @id";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@td", d.TipoDocID);
                cmd.Parameters.AddWithValue("@nd", d.NumDocumento);
                cmd.Parameters.AddWithValue("@n", d.Nombre);
                cmd.Parameters.AddWithValue("@a", d.Apellido);
                cmd.Parameters.AddWithValue("@t", d.Telefono);
                cmd.Parameters.AddWithValue("@e", d.Email);
                cmd.Parameters.AddWithValue("@lm", d.LicenciaMedica);
                cmd.Parameters.AddWithValue("@es", d.EstadoID);
                cmd.Parameters.AddWithValue("@id", d.DentistaID);

                cmd.ExecuteNonQuery();

                conexion.CerrarConexion(out error);
            }
            else
            {
                throw new Exception("Error al conectar: " + error);
            }
        }
        public void Eliminar(int idDentista)
        {
            string error = "";
            SqlConnection con = conexion.AbrirConexion(out error);
            if (con != null)
            {
                string sql = "DELETE FROM CLINICO.DENTISTA WHERE DentistaID = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idDentista);
                cmd.ExecuteNonQuery();
                conexion.CerrarConexion(out error);
            }
        }
        public DataTable Listar()
        {
            string error = string.Empty;
            SqlConnection con = conexion.AbrirConexion(out error);
            DataTable dt = new DataTable();

            if (con != null)
            {
                string sql = @"SELECT 
                    Nombre, 
                    Apellido, 
                    CASE WHEN TipoDocID = 1 THEN 'DUI' ELSE 'Pasaporte' END AS TipoDoc,
                    NumDocumento,
                    LicenciaMedica,
                    Telefono, 
                    Email,
                    CASE WHEN EstadoID = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado,
                    DentistaID, -- Lo ponemos al final para ocultarlo fácilmente
                    TipoDocID,
                    EstadoID
                   FROM CLINICO.DENTISTA";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conexion.CerrarConexion(out error);
            }
            else
            {
                throw new Exception("Error al conectar: " + error);
            }

            return dt;
        }
    }
}
