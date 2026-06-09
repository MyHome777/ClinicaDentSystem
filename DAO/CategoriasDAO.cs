using Microsoft.Data.SqlClient;
using MODELOS;
using System;
using System.Collections.Generic;
using System.Data;
                                                                                                                                                                                                                                                                                                                                                                                                                            
namespace DAO
{
    public class CategoriasDAO : AbstractDAO<Categorias>
    {
        public override void ActualizarRegistro(Categorias reg, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            SqlCommand cmd = new SqlCommand();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                cmd = new SqlCommand("INVENTARIO.sp_actualizar_categoria", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoriaID", reg.CategoriaID);
                cmd.Parameters.AddWithValue("@Nombre", reg.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", reg.Descripcion);
                cmd.Parameters.AddWithValue("@EstadoID", reg.EstadoId);

                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                pError = msgParam.Value?.ToString() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(pError))
                {
                    pError = "No se recibió respuesta del procedimiento INVENTARIO.sp_actualizar_categoria.";
                }

                conn.CerrarConexion(out string errorCerrar);
                if (!string.IsNullOrEmpty(errorCerrar)) pError = errorCerrar;
            }
            catch (SqlException ex)
            {
                pError = ex.Message;
                Console.WriteLine(pError);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(pError)) pError = ex.Message;
                Console.WriteLine(pError);
            }
        }

        public override void GuardarRegistro(Categorias reg, out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            SqlCommand cmd = new SqlCommand();

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                cmd = new SqlCommand("INVENTARIO.sp_crear_categoria", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", reg.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", reg.Descripcion);
                cmd.Parameters.AddWithValue("@EstadoID", reg.EstadoId);

                SqlParameter msgParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(msgParam);

                cmd.ExecuteNonQuery();
                pError = msgParam.Value?.ToString() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(pError))
                {
                    pError = "No se recibió respuesta del procedimiento INVENTARIO.sp_crear_categoria.";
                }

                conn.CerrarConexion(out string errorCerrar);
                if (!string.IsNullOrEmpty(errorCerrar)) pError = errorCerrar;
            }
            catch (SqlException ex)
            {
                pError = ex.Message;
                Console.WriteLine(pError);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(pError)) pError = ex.Message;
                Console.WriteLine(pError);
            }
        }

        public override Categorias ObtenerPorId(int id, out string pError)
        {
            throw new NotImplementedException();
        }

        public override Categorias ObtenerPorId(string id, out string pError)
        {
            throw new NotImplementedException();
        }

        public override List<Categorias> ObtenerTodos(out string pError)
        {
            pError = string.Empty;
            Conexion conn = new Conexion();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            List<Categorias> categorias = new List<Categorias>(); // 🌟 Declarada como 'categorias'

            try
            {
                conn.AbrirConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);

                cmd = new SqlCommand("INVENTARIO.sp_listar_categorias", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Categorias categoria = new Categorias
                    {
                        CategoriaID = dr["CategoriaID"] != DBNull.Value ? Convert.ToInt32(dr["CategoriaID"]) : 0,
                        Nombre = dr["Nombre"] != DBNull.Value ? dr["Nombre"].ToString() : string.Empty,
                        Descripcion = dr["Descripcion"] != DBNull.Value ? dr["Descripcion"].ToString() : string.Empty,
                        EstadoId = dr["EstadoID"] != DBNull.Value ? Convert.ToInt32(dr["EstadoID"]) : 0,
                        Estado = dr["Estado"] != DBNull.Value ? dr["Estado"].ToString() : string.Empty
                    };
                    categorias.Add(categoria); // 🌟 ¡CORREGIDO! Ahora usa 'categorias.Add' en lugar de 'lista.Add'
                }

                if (!dr.IsClosed) dr.Close();

                conn.CerrarConexion(out pError);
                if (!string.IsNullOrEmpty(pError)) throw new Exception(pError);
            }
            catch (SqlException ex)
            {
                pError = ex.Message;
                Console.WriteLine(pError);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(pError)) pError = ex.Message;
                Console.WriteLine(pError);
            }
            return categorias; // 🌟 Retorna 'categorias' correctamente
        }
    }
}