using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAO;

namespace ClinicaDentSystem
{
    public partial class UC_Auditorias : UserControl
    {
        public UC_Auditorias()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);

            CargarAuditorias();
        }

        private void CargarAuditorias()
        {
            AuditoriaDAO dao = new AuditoriaDAO();

            dataGridView1.DataSource = dao.Listar();

            dataGridView1.Columns["historialId"].Visible = false;
            dataGridView1.Columns["tabla"].Visible = false;

            dataGridView1.Columns["fecha"].HeaderText = "Fecha";

            dataGridView1.Columns["evento"].HeaderText = "Evento";
            dataGridView1.Columns["NombreUsuario"].HeaderText = "Usuario";

            dataGridView1.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            if (dt != null)
            {
                dt.DefaultView.RowFilter =
                    $"evento LIKE '%{guna2TextBox1.Text.Replace("'", "''")}%'";
            }
        }
    }
}