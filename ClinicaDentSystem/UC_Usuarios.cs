using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ClinicaDentSystem
{
    public partial class UC_Usuarios : UserControl
    {
        string cadena = @"Data Source=.\SQLEXPRESS;Initial Catalog=CLINICADENTAL;Integrated Security=True;TrustServerCertificate=True";

        public UC_Usuarios()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
            ListarUsuarios();
        }

        public void ListarUsuarios()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.SpListarUsuarios", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvUsuarios.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar: " + ex.Message);
            }
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            Usuarios usuarios = new Usuarios();
            usuarios.UsuarioGuardado += (s, args) =>
            {
                ListarUsuarios();
            };
            usuarios.ShowDialog();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow != null)
            {
                Usuarios usuarios = new Usuarios();
                usuarios.UsuarioID = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["UsuarioID"].Value);

                usuarios.UsuarioGuardado += (s, args) =>
                {
                    ListarUsuarios();
                };

                usuarios.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un usuario de la lista.");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null) return;
            int idUsuario = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["UsuarioID"].Value);

            DialogResult confirmacion = MessageBox.Show("¿Está seguro de desactivar a este usuario?",
                                                        "Confirmar Desactivación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(cadena))
                    {
                        con.Open();
                        string sql = "UPDATE SEGURIDAD.USUARIO SET EstadoID = 2 WHERE UsuarioID = @ID";

                        SqlCommand cmd = new SqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@ID", idUsuario);

                        int filas = cmd.ExecuteNonQuery();

                        if (filas > 0)
                        {
                            MessageBox.Show("Usuario desactivado correctamente.");
                            ListarUsuarios();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al desactivar: " + ex.Message);
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            FiltrarUsuarios(txtBuscar.Text);
        }

        private void FiltrarUsuarios(string texto)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.SpBuscarUsuarios", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TextoBusqueda", texto);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvUsuarios.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show("Error al buscar: " + ex.Message); }
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FiltrarUsuarios(txtBuscar.Text);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}