using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using MODELOS;

namespace DAO
{
    public class PacienteDAO
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
                        PacienteID, 
                        Nombre, 
                        Apellido, 
                        CASE WHEN TipoDocID = 1 THEN 'DUI' ELSE 'Pasaporte' END AS TipoDoc,
                        NumDocumento,
                        FechaNacimiento, 
                        Telefono, 
                        Direccion, 
                        Email, 
                        ContactoEmergencia, 
                        TelefonoEmergencia, 
                        Alergias, 
                        NotasMedicas,
                        CASE WHEN EstadoID = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
                       FROM CLINICO.PACIENTE";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conexion.CerrarConexion(out error);
            }
            return dt;
        }

        // 2. INSERTAR
        public void Insertar(Paciente p)
        {
            string error = string.Empty;
            SqlConnection con = conexion.AbrirConexion(out error);
            string sql = @"INSERT INTO CLINICO.PACIENTE 
                   (NumDocumento, Nombre, Apellido, FechaNacimiento, Telefono, Direccion, 
                    Email, ContactoEmergencia, TelefonoEmergencia, Alergias, NotasMedicas, 
                    TipoDocID, EstadoID, Fecha) 
                   VALUES 
                   (@nd, @n, @a, @fn, @t, @d, @e, @ce, @te, @al, @nm, @td, @es, @f)";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@nd", p.NumDocumento);
            cmd.Parameters.AddWithValue("@n", p.Nombre);
            cmd.Parameters.AddWithValue("@a", p.Apellido);
            cmd.Parameters.AddWithValue("@fn", p.FechaNacimiento);
            cmd.Parameters.AddWithValue("@t", p.Telefono);
            cmd.Parameters.AddWithValue("@d", p.Direccion);
            cmd.Parameters.AddWithValue("@e", p.Email);
            cmd.Parameters.AddWithValue("@ce", p.ContactoEmergencia);
            cmd.Parameters.AddWithValue("@te", p.TelefonoEmergencia);
            cmd.Parameters.AddWithValue("@al", (object)p.Alergias ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@nm", p.NotasMedicas);
            cmd.Parameters.AddWithValue("@td", p.TipoDocID);
            cmd.Parameters.AddWithValue("@es", p.EstadoID);
            cmd.Parameters.AddWithValue("@f", DateTime.Now);

            cmd.ExecuteNonQuery();
            conexion.CerrarConexion(out error);
        }

        // 3. MODIFICAR (Actualizar)
        public void Actualizar(Paciente p)
        {
            string error = string.Empty;
            SqlConnection con = conexion.AbrirConexion(out error);
            string sql = @"UPDATE CLINICO.PACIENTE SET NumDocumento=@nd, Nombre=@n, Apellido=@a, FechaNacimiento=@fn, 
                           Telefono=@t, Direccion=@d, Email=@e, ContactoEmergencia=@ce, TelefonoEmergencia=@te, 
                           Alergias=@al, NotasMedicas=@nm, TipoDocID=@td, EstadoID=@es 
                           WHERE PacienteID=@id";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", p.PacienteID);
            AgregarParametros(cmd, p);
            cmd.ExecuteNonQuery();
            conexion.CerrarConexion(out error);
        }

        // 4. ELIMINAR
        public void Eliminar(int id)
        {
            string error = string.Empty;
            SqlConnection con = conexion.AbrirConexion(out error);
            string sql = "DELETE FROM CLINICO.PACIENTE WHERE PacienteID = @id";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            conexion.CerrarConexion(out error);
        }
        private void AgregarParametros(SqlCommand cmd, Paciente p)
        {
            cmd.Parameters.AddWithValue("@nd", p.NumDocumento);
            cmd.Parameters.AddWithValue("@n", p.Nombre);
            cmd.Parameters.AddWithValue("@a", p.Apellido);
            cmd.Parameters.AddWithValue("@fn", p.FechaNacimiento);
            cmd.Parameters.AddWithValue("@t", p.Telefono);
            cmd.Parameters.AddWithValue("@d", p.Direccion);
            cmd.Parameters.AddWithValue("@e", p.Email);
            cmd.Parameters.AddWithValue("@ce", p.ContactoEmergencia);
            cmd.Parameters.AddWithValue("@te", p.TelefonoEmergencia);
            cmd.Parameters.AddWithValue("@al", p.Alergias);
            cmd.Parameters.AddWithValue("@nm", p.NotasMedicas);
            cmd.Parameters.AddWithValue("@td", p.TipoDocID);
            cmd.Parameters.AddWithValue("@es", p.EstadoID);
        }
    }
}