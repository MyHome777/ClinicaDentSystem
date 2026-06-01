using Microsoft.Data.SqlClient;
using System;
using DAO;
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
    public partial class UC_Inicio : UserControl
    {
        public UC_Inicio()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);

            CargarContadores();
            CargarCitasHoy();
            CargarStockBajo();
        }

        private void CargarContadores()
        {
            string pError = string.Empty;
            Conexion conn = new Conexion();
            try
            {
                conn.AbrirConexion(out pError);

                // Pacientes activos
                SqlCommand cmd = new SqlCommand("CLINICO.SpContarPacientesActivos", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                lblPacientes.Text = cmd.ExecuteScalar().ToString();

                // Citas hoy
                cmd = new SqlCommand("CLINICO.SpCitasHoy", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                lblCitasHoy.Text = cmd.ExecuteScalar().ToString();

                // Stock bajo
                cmd = new SqlCommand("INVENTARIO.SpProductosStockBajo", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                int count = 0;
                while (dr.Read()) count++;
                dr.Close();
                lblStockBajo.Text = count.ToString();

                conn.CerrarConexion(out pError);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CargarCitasHoy()
        {
            string pError = string.Empty;
            Conexion conn = new Conexion();
            try
            {
                conn.AbrirConexion(out pError);
                SqlCommand cmd = new SqlCommand("CLINICO.SpCitasHoy", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                //  para la tarjeta de citas
                lblCitasHoy.Text = ds.Tables[0].Rows[0]["Total"].ToString();
                
                // para el dgv de citas
                dgvCitasHoy.DataSource = ds.Tables[1];

                conn.CerrarConexion(out pError);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CargarStockBajo()
        {
            string pError = string.Empty;
            Conexion conn = new Conexion();
            try
            {
                conn.AbrirConexion(out pError);
                SqlCommand cmd = new SqlCommand("INVENTARIO.SpProductosStockBajo", conn.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                // para la tarjeta de stock
                lblStockBajo.Text = ds.Tables[0].Rows[0]["Total"].ToString();

                // para la tabla de stock
                dgvStockBajo.DataSource = ds.Tables[1];

                conn.CerrarConexion(out pError);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void guna2Separator2_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dgvStockBajo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvCitasHoy_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
