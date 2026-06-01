using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class Conexion
    {
        private string connString = string.Empty;
        public SqlConnection conn;
        public Conexion()
        {
            conn = new SqlConnection();
            try
            {
                connString = ConfigurationManager.ConnectionStrings["connSQLServer"].ConnectionString;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public SqlConnection AbrirConexion(out string pError)
        {
            pError = string.Empty;
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.ConnectionString = connString;
                    conn.Open();
                }
                return conn;
            }
            catch (SqlException ex)
            {
                pError = ex.Message;
                return null;
            }
        }

        public void CerrarConexion(out string pError)//SqlConnection pConn) 
        {
            pError = string.Empty;
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                Console.WriteLine("Conexion cerrada");
            }
            catch (SqlException ex)
            {
                pError = ex.Message;
            }
        }
    }

}
