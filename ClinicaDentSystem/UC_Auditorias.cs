using DAO;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ClinicaDentSystem
{
    public partial class UC_Auditorias : UserControl
    {
        public UC_Auditorias()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
            ConfigurarGrid();
            CargarAuditorias();
        }

        private void ConfigurarGrid()
        {
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 111, 217);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(30, 111, 217);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
        }

        private void CargarAuditorias()
        {
            AuditoriaDAO dao = new AuditoriaDAO();

            dataGridView1.DataSource = dao.Listar();
            ConfigurarColumnas();
        }

        private void ConfigurarColumnas()
        {
            if (dataGridView1.Columns.Count == 0)
            {
                return;
            }

            OcultarColumna("historialId");
            OcultarColumna("tabla");

            if (dataGridView1.Columns.Contains("fecha"))
            {
                dataGridView1.Columns["fecha"].HeaderText = "Fecha";
                dataGridView1.Columns["fecha"].FillWeight = 18;
                dataGridView1.Columns["fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }

            if (dataGridView1.Columns.Contains("evento"))
            {
                dataGridView1.Columns["evento"].HeaderText = "Evento";
                dataGridView1.Columns["evento"].FillWeight = 64;
            }

            if (dataGridView1.Columns.Contains("NombreUsuario"))
            {
                dataGridView1.Columns["NombreUsuario"].HeaderText = "Usuario";
                dataGridView1.Columns["NombreUsuario"].FillWeight = 18;
            }
        }

        private void OcultarColumna(string nombre)
        {
            if (dataGridView1.Columns.Contains(nombre))
            {
                dataGridView1.Columns[nombre].Visible = false;
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource is not DataTable dt)
            {
                return;
            }

            string texto = EscaparFiltro(guna2TextBox1.Text);
            if (string.IsNullOrWhiteSpace(texto))
            {
                dt.DefaultView.RowFilter = string.Empty;
                return;
            }

            dt.DefaultView.RowFilter =
                $"Convert(evento, 'System.String') LIKE '%{texto}%' OR Convert(NombreUsuario, 'System.String') LIKE '%{texto}%'";
        }

        private static string EscaparFiltro(string texto)
        {
            return texto
                .Trim()
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("*", "[*]");
        }
    }
}
