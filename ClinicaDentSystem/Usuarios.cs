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
    public partial class Usuarios : Form
    {
        public event EventHandler UsuarioGuardado;
        public int UsuarioID = 0;

        string cadena = @"Data Source=.\SQLEXPRESS;Initial Catalog=CLINICADENTAL;Integrated Security=True;TrustServerCertificate=True";

        public Usuarios()
        {
            InitializeComponent();
        }

        private void Usuarios_Load(object sender, EventArgs e)
        {
            CargarComboRoles();
            CargarComboEstados();
            if (UsuarioID > 0)
            {
                CargarDatosParaEditar();
            }
        }

        private void CargarComboRoles()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT RolID, NombreRol FROM SEGURIDAD.ROL", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmbRol.DataSource = dt;
                    cmbRol.DisplayMember = "NombreRol";
                    cmbRol.ValueMember = "RolID";
                }
            }
            catch (Exception ex) { MessageBox.Show("Error roles: " + ex.Message); }
        }

        private void CargarComboEstados()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT EstadoId, Estado FROM SEGURIDAD.ESTADO", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmbEstado.DataSource = dt;
                    cmbEstado.DisplayMember = "Estado";
                    cmbEstado.ValueMember = "EstadoId";
                }
            }
            catch (Exception ex) { MessageBox.Show("Error estados: " + ex.Message); }
        }

        private void CargarDatosParaEditar()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM SEGURIDAD.USUARIO WHERE UsuarioID = @ID", con);
                    cmd.Parameters.AddWithValue("@ID", UsuarioID);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        txtNombreUsuario.Text = dr["NombreUsuario"].ToString();
                        txtNombreEmpleado.Text = dr["NombreEmpleado"].ToString();
                        txtClave.Text = dr["Clave"].ToString();
                        txtEmail.Text = dr["Email"].ToString();
                        cmbRol.SelectedValue = dr["RolID"];
                        cmbEstado.SelectedValue = dr["EstadoID"];
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error al cargar datos: " + ex.Message); }
        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    string sp = (UsuarioID == 0) ? "SEGURIDAD.SpInsertarUsuario" : "SEGURIDAD.SpActualizarUsuario";
                    SqlCommand cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (UsuarioID > 0) cmd.Parameters.AddWithValue("@UsuarioID", UsuarioID);

                    cmd.Parameters.AddWithValue("@NombreUsuario", txtNombreUsuario.Text);
                    cmd.Parameters.AddWithValue("@NombreEmpleado", txtNombreEmpleado.Text);
                    cmd.Parameters.AddWithValue("@Clave", txtClave.Text);
                    cmd.Parameters.AddWithValue("@RolID", cmbRol.SelectedValue);
                    cmd.Parameters.AddWithValue("@EstadoID", cmbEstado.SelectedValue);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Estado", cmbEstado.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Guardado con éxito.");

                    UsuarioGuardado?.Invoke(this, EventArgs.Empty);
                    this.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }
    }
}