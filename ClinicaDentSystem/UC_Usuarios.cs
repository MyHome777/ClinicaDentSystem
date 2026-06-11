using DAO;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ClinicaDentSystem
{
    public partial class UC_Usuarios : UserControl
    {
        private readonly UsuarioDAO usuarioDAO = new UsuarioDAO();

        public UC_Usuarios()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
            ConfigurarGridUsuarios();
            ListarUsuarios();
        }

        public void ListarUsuarios()
        {
            DataTable usuarios = usuarioDAO.ListarUsuarios(out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show("Error al listar usuarios: " + error, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgvUsuarios.DataSource = usuarios;
            ConfigurarColumnasUsuarios();
        }

        private void ConfigurarGridUsuarios()
        {
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AllowUserToResizeRows = false;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.BackgroundColor = SystemColors.ButtonHighlight;
            dgvUsuarios.BorderStyle = BorderStyle.FixedSingle;
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 111, 217);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvUsuarios.DefaultCellStyle.SelectionBackColor = Color.FromArgb(30, 111, 217);
            dgvUsuarios.DefaultCellStyle.SelectionForeColor = Color.White;
        }

        private void ConfigurarColumnasUsuarios()
        {
            if (dgvUsuarios.Columns.Count == 0)
            {
                return;
            }

            OcultarColumna("UsuarioID");
            OcultarColumna("RolID");
            OcultarColumna("EstadoID");

            ConfigurarColumna("NombreUsuario", "Usuario", 22);
            ConfigurarColumna("NombreEmpleado", "Empleado", 28);
            ConfigurarColumna("Email", "Correo", 28);
            ConfigurarColumna("NombreRol", "Rol", 14);
            ConfigurarColumna("Estado", "Estado", 8);

            foreach (DataGridViewColumn columna in dgvUsuarios.Columns)
            {
                columna.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void ConfigurarColumna(string nombre, string encabezado, float peso)
        {
            if (!dgvUsuarios.Columns.Contains(nombre))
            {
                return;
            }

            DataGridViewColumn columna = dgvUsuarios.Columns[nombre];
            columna.HeaderText = encabezado;
            columna.FillWeight = peso;
        }

        private void OcultarColumna(string nombre)
        {
            if (dgvUsuarios.Columns.Contains(nombre))
            {
                dgvUsuarios.Columns[nombre].Visible = false;
            }
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            using Usuarios usuarios = new Usuarios();
            usuarios.UsuarioGuardado += (s, args) => ListarUsuarios();
            usuarios.ShowDialog();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            int usuarioID = ObtenerUsuarioSeleccionadoId();
            if (usuarioID == 0)
            {
                return;
            }

            using Usuarios usuarios = new Usuarios();
            usuarios.UsuarioID = usuarioID;
            usuarios.UsuarioGuardado += (s, args) => ListarUsuarios();
            usuarios.ShowDialog();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int usuarioID = ObtenerUsuarioSeleccionadoId();
            if (usuarioID == 0)
            {
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                "Esta seguro de desactivar a este usuario?",
                "Confirmar desactivacion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
            {
                return;
            }

            usuarioDAO.DesactivarUsuario(usuarioID, out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show("Error al desactivar: " + error, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Usuario desactivado correctamente.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ListarUsuarios();
        }

        private int ObtenerUsuarioSeleccionadoId()
        {
            if (dgvUsuarios.CurrentRow == null || dgvUsuarios.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Por favor, selecciona un usuario de la lista.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }

            if (!dgvUsuarios.Columns.Contains("UsuarioID"))
            {
                MessageBox.Show("No se encontro la columna UsuarioID.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }

            object valor = dgvUsuarios.CurrentRow.Cells["UsuarioID"].Value;
            if (valor == null || valor == DBNull.Value || !int.TryParse(valor.ToString(), out int usuarioID))
            {
                MessageBox.Show("No se pudo obtener el usuario seleccionado.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }

            return usuarioID;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            FiltrarUsuarios(txtBuscar.Text);
        }

        private void FiltrarUsuarios(string texto)
        {
            DataTable usuarios = usuarioDAO.BuscarUsuarios(texto, out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show("Error al buscar: " + error, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgvUsuarios.DataSource = usuarios;
            ConfigurarColumnasUsuarios();
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
