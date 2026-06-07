using DAO;
using MODELOS;
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
    public partial class Proveedor : Form
    {
        private readonly ProveedoresDAO _dao = new ProveedoresDAO();
        private int _proveedorIdSeleccionado;

        public Proveedor()
        {
            InitializeComponent();
            ConfigurarGridProveedores();
            CargarEstadosProveedor();
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            guna2Button2.Click += guna2Button2_Click;
            guna2ImageButton2.Click += guna2ImageButton2_Click;
            guna2Button1.Click += guna2Button1_Click;
            guna2ImageButton1.Click += guna2ImageButton1_Click;
            CargarProveedores();
            ActivarModoCreacion();
        }

        private void guna2vSeparator1_Click(object sender, EventArgs e)
        {

        }

        private void ConfigurarGridProveedores()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProveedorID",
                HeaderText = "Código",
                Name = "ProveedorID",
                ReadOnly = true,
                Width = 80
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreProveedor",
                HeaderText = "Proveedor",
                Name = "NombreProveedor",
                ReadOnly = true,
                Width = 160
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Contacto",
                HeaderText = "Contacto",
                Name = "Contacto",
                ReadOnly = true,
                Width = 130
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Telefono",
                HeaderText = "Teléfono",
                Name = "Telefono",
                ReadOnly = true,
                Width = 100
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Email",
                HeaderText = "Email",
                Name = "Email",
                ReadOnly = true,
                Width = 110
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Estado",
                HeaderText = "Estado",
                Name = "Estado",
                ReadOnly = true,
                Width = 90
            });
        }

        private void CargarEstadosProveedor()
        {
            string pError = string.Empty;
            DataTable estados = _dao.ObtenerEstados(out pError);

            if (!string.IsNullOrEmpty(pError))
            {
                MessageBox.Show(pError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            guna2ComboBox3.DisplayMember = "Estado";
            guna2ComboBox3.ValueMember = "EstadoId";
            guna2ComboBox3.DataSource = estados;
            guna2ComboBox3.Enabled = true;
        }

        private void CargarProveedores()
        {
            string pError = string.Empty;
            dataGridView1.DataSource = _dao.ObtenerTodos(out pError);

            if (!string.IsNullOrEmpty(pError))
            {
                MessageBox.Show(pError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button2_Click(object? sender, EventArgs e)
        {
            GuardarProveedor();
        }

        private void guna2ImageButton2_Click(object? sender, EventArgs e)
        {
            GuardarProveedor();
        }

        private void guna2Button1_Click(object? sender, EventArgs e)
        {
            ActualizarProveedorSeleccionado();
        }

        private void guna2ImageButton1_Click(object? sender, EventArgs e)
        {
            ActualizarProveedorSeleccionado();
        }

        private void GuardarProveedor()
        {
            if (!ValidarFormulario())
            {
                return;
            }

            string pError = string.Empty;
            Proveedores proveedor = ObtenerProveedorFormulario();
            _dao.GuardarRegistro(proveedor, out pError);

            if (string.Equals(pError?.Trim(), "Proveedor insertado correctamente", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(pError, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarProveedores();
                ActivarModoCreacion();
            }
            else
            {
                MessageBox.Show(pError, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ActualizarProveedorSeleccionado()
        {
            if (_proveedorIdSeleccionado <= 0)
            {
                MessageBox.Show("Selecciona un proveedor para actualizar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarFormulario())
            {
                return;
            }

            string pError = string.Empty;
            Proveedores proveedor = ObtenerProveedorFormulario();
            proveedor.ProveedorID = _proveedorIdSeleccionado;
            _dao.ActualizarRegistro(proveedor, out pError);

            if (string.Equals(pError?.Trim(), "Proveedor actualizado correctamente", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(pError, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarProveedores();
                ActivarModoCreacion();
            }
            else
            {
                MessageBox.Show(pError, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView1_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is not Proveedores proveedor)
            {
                MessageBox.Show("No se pudo obtener el proveedor seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _proveedorIdSeleccionado = proveedor.ProveedorID;
            guna2TextBox2.Text = proveedor.NombreProveedor;
            guna2TextBox3.Text = proveedor.Contacto;
            guna2TextBox4.Text = proveedor.Telefono;
            guna2TextBox1.Text = proveedor.Email;
            guna2ComboBox3.SelectedValue = proveedor.EstadoId;

            guna2Button2.Enabled = false;
            guna2ImageButton2.Enabled = false;
            guna2Button1.Enabled = true;
            guna2ImageButton1.Enabled = true;
        }

        private bool ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(guna2TextBox2.Text))
            {
                MessageBox.Show("Ingresa el nombre del proveedor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(guna2TextBox3.Text))
            {
                MessageBox.Show("Ingresa el contacto del proveedor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(guna2TextBox4.Text))
            {
                MessageBox.Show("Ingresa el teléfono del proveedor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (guna2ComboBox3.SelectedValue is null)
            {
                MessageBox.Show("Selecciona un estado para el proveedor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private Proveedores ObtenerProveedorFormulario()
        {
            dataGridView1.AutoGenerateColumns = false;
            return new Proveedores
            {
                
                NombreProveedor = guna2TextBox2.Text.Trim(),
                Contacto = guna2TextBox3.Text.Trim(),
                Telefono = guna2TextBox4.Text.Trim(),
                Email = guna2TextBox1.Text.Trim(),
                EstadoId = Convert.ToInt32(guna2ComboBox3.SelectedValue)
            };
        }

        private void ActivarModoCreacion()
        {
            _proveedorIdSeleccionado = 0;
            guna2TextBox2.Text = string.Empty;
            guna2TextBox3.Text = string.Empty;
            guna2TextBox4.Text = string.Empty;
            guna2TextBox1.Text = string.Empty;

            if (guna2ComboBox3.Items.Count > 0)
            {
                guna2ComboBox3.SelectedIndex = 0;
            }

            guna2Button2.Enabled = true;
            guna2ImageButton2.Enabled = true;
            guna2Button1.Enabled = false;
            guna2ImageButton1.Enabled = false;
        }

        private void Proveedor_Load(object sender, EventArgs e)
        {

        }
    }
}
