using DAO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicaDentSystem
{
    public partial class RecuperarUsuario : Form
    {
        string cadena = @"Data Source=.\SQLEXPRESS;Initial Catalog=CLINICADENTAL;Integrated Security=True;TrustServerCertificate=True";

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public RecuperarUsuario()
        {
            InitializeComponent();
            CargarRolesForzado();
        }

        private void CargarRolesForzado()
        {
            string cadena = @"Data Source=.\SQLEXPRESS;Initial Catalog=CLINICADENTAL;Integrated Security=True;TrustServerCertificate=True";
            if (cmbRol == null) return;

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT RolID, NombreRol FROM SEGURIDAD.ROL", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        cmbRol.DataSource = dt;
                        cmbRol.DisplayMember = "NombreRol";
                        cmbRol.ValueMember = "RolID";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los roles: " + ex.Message);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (txtNuevaContrasena.Text != txtConfirmarContrasena.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.SpRecuperarContrasena", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NombreUsuario", txtNombreUsuario.Text);
                    cmd.Parameters.AddWithValue("@NombreEmpleado", txtNombreEmpleado.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@RolID", cmbRol.SelectedValue);
                    cmd.Parameters.AddWithValue("@NuevaClave", txtNuevaContrasena.Text);

                    SqlParameter res = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
                    res.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(res);
                    con.Open();
                    cmd.ExecuteNonQuery();

                    string mensaje = res.Value.ToString();
                    MessageBox.Show(mensaje);

                    if (mensaje == "Contraseña actualizada correctamente")
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message);
            }
        }
    }
}
