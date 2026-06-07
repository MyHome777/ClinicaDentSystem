using Microsoft.Data.SqlClient;
using MODELOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace DAO
{
    public class CitasDAO : AbstractDAO<Citass>
    {
        public override void ActualizarRegistro(Citass reg, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                cmd = new SqlCommand("SpUpdateCita", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CitaID", reg.Numerodecita);
                cmd.Parameters.AddWithValue("@PacienteId", reg.PacienteId);
                cmd.Parameters.AddWithValue("@DentistaID", reg.DentistaID);
                cmd.Parameters.AddWithValue("@FechaHora", reg.Fechayhora);
                cmd.Parameters.AddWithValue("@Motivo", reg.Motivo);
                cmd.Parameters.AddWithValue("@EstadoID", reg.EstadoID);
                cmd.Parameters.AddWithValue("@NotasCita", reg.Notasdelacita ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Fecha", reg.Fecha);

                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
                msgParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                pError = msgParam.Value?.ToString() ?? string.Empty;

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
       

        public override void GuardarRegistro(Citass reg, out string pError)
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

                cmd = new SqlCommand("SpInsertCita", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PacienteId", reg.PacienteId);
                cmd.Parameters.AddWithValue("@DentistaID", reg.DentistaID);
                cmd.Parameters.AddWithValue("@FechaHora", reg.Fechayhora);
                cmd.Parameters.AddWithValue("@Motivo", reg.Motivo);
                cmd.Parameters.AddWithValue("@EstadoID", reg.EstadoID);
                cmd.Parameters.AddWithValue("@NotasCita", reg.Notasdelacita ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Fecha", reg.Fecha);

                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
                msgParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();

                pError = msgParam.Value?.ToString() ?? string.Empty;

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

        public void CancelarCita(int citaID, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                    throw new Exception(pError);

                cmd = new SqlCommand("SpDeleteCita", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CitaID", citaID);

                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
                msgParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                pError = msgParam.Value?.ToString() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(pError))
                {
                    pError = "No se recibio respuesta del procedimiento SpDeleteCita.";
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

        public override Citass ObtenerPorId(int id, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            Citass cita = new Citass();
            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                {
                    throw new Exception(pError);
                }

                cmd = new SqlCommand(
                    @"SELECT CitaID, PacienteID, DentistaID, FechaHora, Motivo, EstadoID, NotasCita, Fecha
                      FROM CLINICO.CITA
                      WHERE CitaID = @CitaID",
                    conn.conn);
                cmd.Parameters.AddWithValue("@CitaID", id);
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    cita.Numerodecita = dr.GetInt32(0);
                    cita.PacienteId = dr.GetInt32(1);
                    cita.DentistaID = dr.GetInt32(2);
                    cita.Fechayhora = dr.GetDateTime(3);
                    cita.Motivo = dr.GetString(4);
                    cita.EstadoID = dr.GetInt32(5);
                    cita.Notasdelacita = dr.IsDBNull(6) ? string.Empty : dr.GetString(6);
                    cita.Fecha = dr.GetDateTime(7);
                }
                else
                {
                    pError = "No se encontro la cita seleccionada.";
                }

                if (!dr.IsClosed)
                    dr.Close();

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
            return cita;
        }

        public List<Citass> BuscarCitas(string campo, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            List<Citass> bCita = new List<Citass>();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError))
                {
                    throw new Exception(pError);
                }
                cmd = new SqlCommand("SpSearchCitas", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Campo", campo);
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Citass cita = new Citass();
                    cita.Numerodecita = dr.GetInt32(0);
                    cita.Paciente = dr.GetString(1);
                    cita.Dentista = dr.GetString(2);
                    cita.Fechayhora = dr.GetDateTime(3);
                    cita.Motivo = dr.GetString(4);
                    cita.Estadodelacita1 = dr.GetString(5);
                    cita.Notasdelacita = dr.GetString(6);
                    cita.Fecha = dr.GetDateTime(7);

                    bCita.Add(cita);
                }
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                conn.CerrarConexion(out pError);

                if (!string.IsNullOrEmpty(pError)) //problema al cerrar conexion
                {
                    throw new Exception(pError);
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
                {
                    pError = ex.Message;
                }

                Console.WriteLine(pError);
            }

            return bCita;
        }

        public override Citass ObtenerPorId(string id, out string pError)
        {
            throw new NotImplementedException();
        }

        public override List<Citass> ObtenerTodos(out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            List<Citass> citas = new List<Citass>();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) 
                {
                    throw new Exception(pError);
                }
                cmd = new SqlCommand("SpSelectAllCitas", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                 dr= cmd.ExecuteReader();
                while(dr.Read()) 
                {
                   Citass cita = new Citass();
                    cita.Numerodecita = dr.GetInt32(0);
                    cita.Paciente = dr.GetString(1);
                    cita.Dentista = dr.GetString(2);
                    cita.Fechayhora = dr.GetDateTime(3);
                    cita.Motivo = dr.GetString(4);
                    cita.Estadodelacita1 = dr.GetString(5);
                    cita.Notasdelacita = dr.GetString(6);
                    cita.Fecha = dr.GetDateTime(7);

                    citas.Add(cita);
                }
                if (!dr.IsClosed)
                {
                    dr.Close();
                }

                conn.CerrarConexion(out pError);

                if (!string.IsNullOrEmpty(pError)) //problema al cerrar conexion
                {
                    throw new Exception(pError);
                }
            }
            catch(SqlException ex) 
            {
                pError = ex.Message;
                Console.WriteLine(pError);
            }
            catch (Exception ex)
            {

                if (string.IsNullOrEmpty(pError))
                {
                    pError = ex.Message;
                }

                Console.WriteLine(pError);
            }

            return citas;
        }
    }
}
