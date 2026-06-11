using ClinicaDentSystem;
using DAO;
using Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class UC_Configuracion : UserControl
    {
        private readonly PermisosDAO permisosDAO = new PermisosDAO();
        private readonly ConfiguracionCorreoDAO configuracionCorreoDAO = new ConfiguracionCorreoDAO();
        private readonly BindingSource usuariosBindingSource = new BindingSource();
        private List<PermisoModulo> permisosActuales = new List<PermisoModulo>();
        private bool cargandoPermisos;

        public UC_Configuracion()
        {
            InitializeComponent();
        }

        private void UC_Configuracion_Load(object sender, EventArgs e)
        {
            if (Program.UsuarioActivo?.EsAdministrador != true)
            {
                BloquearPantalla();
                return;
            }

            CargarUsuarios();
            CargarConfiguracionCorreo();
        }

        private void BloquearPantalla()
        {
            dgvUsuarios.Enabled = false;
            checkedListBoxModulos.Enabled = false;
            txtBuscar.Enabled = false;
            btnGuardarPermisos.Enabled = false;
            btnMarcarTodos.Enabled = false;
            btnQuitarTodos.Enabled = false;
            btnRecargar.Enabled = false;
            txtCorreoRemitente.Enabled = false;
            txtNombreRemitente.Enabled = false;
            txtServidorSmtp.Enabled = false;
            txtPuertoSmtp.Enabled = false;
            txtClaveCorreo.Enabled = false;
            chkUsaSsl.Enabled = false;
            btnGuardarCorreo.Enabled = false;
            lblInfo.Text = "Solo el administrador puede asignar permisos.";
            lblCorreoInfo.Text = "Solo el administrador puede modificar la configuracion de correo.";
        }

        private void CargarUsuarios()
        {
            DataTable usuarios = permisosDAO.ListarUsuarios(out string error);

            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show("Error al cargar usuarios: " + error,
                                "Configuracion",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            cargandoPermisos = true;
            usuariosBindingSource.DataSource = usuarios;
            dgvUsuarios.DataSource = usuariosBindingSource;
            FormatearGridUsuarios();
            cargandoPermisos = false;

            if (dgvUsuarios.Rows.Count > 0)
            {
                dgvUsuarios.Rows[0].Selected = true;
                CargarPermisosUsuarioSeleccionado();
            }
        }

        private void FormatearGridUsuarios()
        {
            if (dgvUsuarios.Columns.Contains("UsuarioID"))
            {
                dgvUsuarios.Columns["UsuarioID"].HeaderText = "ID";
                dgvUsuarios.Columns["UsuarioID"].Width = 60;
            }

            if (dgvUsuarios.Columns.Contains("NombreUsuario"))
                dgvUsuarios.Columns["NombreUsuario"].HeaderText = "Usuario";

            if (dgvUsuarios.Columns.Contains("NombreEmpleado"))
                dgvUsuarios.Columns["NombreEmpleado"].HeaderText = "Empleado";

            if (dgvUsuarios.Columns.Contains("NombreRol"))
                dgvUsuarios.Columns["NombreRol"].HeaderText = "Rol";

            if (dgvUsuarios.Columns.Contains("Estado"))
                dgvUsuarios.Columns["Estado"].HeaderText = "Estado";

            if (dgvUsuarios.Columns.Contains("RolID"))
                dgvUsuarios.Columns["RolID"].Visible = false;

            if (dgvUsuarios.Columns.Contains("EstadoID"))
                dgvUsuarios.Columns["EstadoID"].Visible = false;
        }

        private void CargarPermisosUsuarioSeleccionado()
        {
            int usuarioID = ObtenerUsuarioSeleccionado();
            if (usuarioID <= 0)
            {
                return;
            }

            lblUsuarioSeleccionado.Text = "Usuario seleccionado: " + ObtenerTextoCelda("NombreUsuario");

            List<PermisoModulo> permisos = permisosDAO.ObtenerPermisosUsuario(usuarioID, out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show("Error al cargar permisos: " + error,
                                "Configuracion",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            permisosActuales = permisos
                .Where(p => !string.Equals(p.Codigo, ModulosSistema.Inicio, StringComparison.OrdinalIgnoreCase)
                         && !string.Equals(p.Codigo, ModulosSistema.Configuracion, StringComparison.OrdinalIgnoreCase))
                .ToList();

            cargandoPermisos = true;
            checkedListBoxModulos.Items.Clear();

            foreach (PermisoModulo permiso in permisosActuales)
            {
                checkedListBoxModulos.Items.Add(permiso, permiso.Permitido);
            }

            cargandoPermisos = false;
            btnGuardarPermisos.Enabled = true;
            lblInfo.Text = "Marque los modulos que este usuario puede ver en el Dashboard.";
        }

        private int ObtenerUsuarioSeleccionado()
        {
            if (dgvUsuarios.CurrentRow == null || dgvUsuarios.CurrentRow.IsNewRow || !dgvUsuarios.Columns.Contains("UsuarioID"))
            {
                return 0;
            }

            object valor = dgvUsuarios.CurrentRow.Cells["UsuarioID"].Value;
            return Convert.ToInt32(valor);
        }

        private string ObtenerTextoCelda(string columna)
        {
            if (dgvUsuarios.CurrentRow == null || !dgvUsuarios.Columns.Contains(columna))
            {
                return string.Empty;
            }

            return dgvUsuarios.CurrentRow.Cells[columna].Value?.ToString() ?? string.Empty;
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (!cargandoPermisos && Program.UsuarioActivo?.EsAdministrador == true)
            {
                CargarPermisosUsuarioSeleccionado();
            }
        }

        private void btnGuardarPermisos_Click(object sender, EventArgs e)
        {
            int usuarioID = ObtenerUsuarioSeleccionado();
            if (usuarioID <= 0)
            {
                MessageBox.Show("Seleccione un usuario.",
                                "Configuracion",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            for (int i = 0; i < checkedListBoxModulos.Items.Count; i++)
            {
                if (checkedListBoxModulos.Items[i] is PermisoModulo permiso)
                {
                    permiso.Permitido = checkedListBoxModulos.GetItemChecked(i);
                }
            }

            permisosDAO.GuardarPermisosUsuario(usuarioID,
                                               permisosActuales,
                                               Program.UsuarioActivo?.UsuarioId ?? 0,
                                               out string error);

            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show("Error al guardar permisos: " + error,
                                "Configuracion",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (Program.UsuarioActivo?.UsuarioId == usuarioID)
            {
                RefrescarPermisosUsuarioActivo();
            }

            MessageBox.Show("Permisos guardados correctamente.",
                            "Configuracion",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private void RefrescarPermisosUsuarioActivo()
        {
            if (Program.UsuarioActivo == null)
            {
                return;
            }

            if (Program.UsuarioActivo.EsAdministrador)
            {
                Program.UsuarioActivo.PermisosModulos = new HashSet<string>(ModulosSistema.Todos, StringComparer.OrdinalIgnoreCase);
                return;
            }

            Program.UsuarioActivo.PermisosModulos = permisosDAO.ObtenerCodigosPermitidos(Program.UsuarioActivo.UsuarioId, out _);
        }

        private void btnMarcarTodos_Click(object sender, EventArgs e)
        {
            MarcarPermisos(true);
        }

        private void btnQuitarTodos_Click(object sender, EventArgs e)
        {
            MarcarPermisos(false);
        }

        private void MarcarPermisos(bool marcado)
        {
            for (int i = 0; i < checkedListBoxModulos.Items.Count; i++)
            {
                checkedListBoxModulos.SetItemChecked(i, marcado);
            }
        }

        private void btnRecargar_Click(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (usuariosBindingSource.DataSource is not DataTable tabla)
            {
                return;
            }

            string texto = txtBuscar.Text.Trim().Replace("'", "''");
            if (string.IsNullOrWhiteSpace(texto))
            {
                usuariosBindingSource.RemoveFilter();
                return;
            }

            List<string> filtros = new List<string>();
            AgregarFiltro(tabla, filtros, "UsuarioID", texto, true);
            AgregarFiltro(tabla, filtros, "NombreUsuario", texto);
            AgregarFiltro(tabla, filtros, "NombreEmpleado", texto);
            AgregarFiltro(tabla, filtros, "NombreRol", texto);
            AgregarFiltro(tabla, filtros, "Estado", texto);

            usuariosBindingSource.Filter = string.Join(" OR ", filtros);
        }

        private static void AgregarFiltro(DataTable tabla, List<string> filtros, string columna, string texto, bool convertir = false)
        {
            if (!tabla.Columns.Contains(columna))
            {
                return;
            }

            if (convertir)
            {
                filtros.Add($"Convert({columna}, 'System.String') LIKE '%{texto}%'");
                return;
            }

            filtros.Add($"{columna} LIKE '%{texto}%'");
        }

        private void CargarConfiguracionCorreo()
        {
            ConfiguracionCorreo configuracion = configuracionCorreoDAO.ObtenerConfiguracion(out string error);

            if (!string.IsNullOrEmpty(error))
            {
                lblCorreoInfo.Text = "Ejecute el script de configuracion de correo antes de guardar.";
                return;
            }

            txtCorreoRemitente.Text = configuracion.CorreoRemitente;
            txtNombreRemitente.Text = configuracion.NombreRemitente;
            txtServidorSmtp.Text = configuracion.ServidorSmtp;
            txtPuertoSmtp.Text = configuracion.Puerto > 0 ? configuracion.Puerto.ToString() : "587";
            txtClaveCorreo.Text = configuracion.ClaveCorreo;
            chkUsaSsl.Checked = configuracion.UsaSsl;
            lblCorreoInfo.Text = "Configure el SMTP desde donde se enviaran las facturas.";
        }

        private void btnGuardarCorreo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCorreoRemitente.Text)
                || string.IsNullOrWhiteSpace(txtServidorSmtp.Text)
                || string.IsNullOrWhiteSpace(txtClaveCorreo.Text))
            {
                MessageBox.Show("Ingrese correo remitente, servidor SMTP y clave/app password.",
                                "Configuracion de correo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtPuertoSmtp.Text.Trim(), out int puerto) || puerto <= 0)
            {
                MessageBox.Show("Ingrese un puerto SMTP valido.",
                                "Configuracion de correo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            ConfiguracionCorreo configuracion = new ConfiguracionCorreo
            {
                CorreoRemitente = txtCorreoRemitente.Text.Trim(),
                NombreRemitente = txtNombreRemitente.Text.Trim(),
                ServidorSmtp = txtServidorSmtp.Text.Trim(),
                Puerto = puerto,
                UsaSsl = chkUsaSsl.Checked,
                ClaveCorreo = txtClaveCorreo.Text,
                Activo = true
            };

            bool guardado = configuracionCorreoDAO.GuardarConfiguracion(
                configuracion,
                Program.UsuarioActivo?.UsuarioId ?? 0,
                out string mensaje);

            MessageBox.Show(mensaje,
                            guardado ? "Configuracion de correo" : "Error",
                            MessageBoxButtons.OK,
                            guardado ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (guardado)
                CargarConfiguracionCorreo();
        }
    }
}
