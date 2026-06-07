using DAO;
using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;
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
    public partial class Categoria : Form
    {
        private readonly CategoriasDAO _dao = new CategoriasDAO();
        private int _categoriaIdSeleccionada;

        public Categoria()
        {
            InitializeComponent();
            dgv_MostrarCategorias.CellDoubleClick += dgv_MostrarCategorias_CellDoubleClick;
        }

        private void Categoria_Load(object sender, EventArgs e)
        {
            CargarEstados();
            CargarCategorias();
            ActivarModoCreacion();
        }

        private void CargarEstados()
        {
            string pError = string.Empty;
            Conexion conn = new Conexion();
            try
            {
                conn.AbrirConexion(out pError);
                SqlCommand cmd = new SqlCommand("SELECT EstadoId, Estado FROM INVENTARIO.ESTADO", conn.conn);
                SqlDataReader dr = cmd.ExecuteReader();
                cbox_Estado.DisplayMember = "Estado";
                cbox_Estado.ValueMember = "EstadoId";
                DataTable dt = new DataTable();
                dt.Load(dr);
                cbox_Estado.DataSource = dt;
                conn.CerrarConexion(out pError);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_GuardarCategoria_Click_1(object sender, EventArgs e)
        {
            GuardarCategoria();
        }

        private void btn_Actualizar_Click_1(object? sender, EventArgs e)
        {
            ActualizarCategoriaSeleccionada();
        }

        private void GuardarCategoria()
        {
            if (!ValidarFormulario())
            {
                return;
            }

            string pError = string.Empty;
            Categorias cat = ObtenerCategoriaFormulario();
            _dao.GuardarRegistro(cat, out pError);

            if (string.Equals(pError?.Trim(), "Categoria creada correctamente", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(pError, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarCategorias();
                ActivarModoCreacion();
            }
            else
            {
                MessageBox.Show(pError, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ActualizarCategoriaSeleccionada()
        {
            if (_categoriaIdSeleccionada <= 0)
            {
                MessageBox.Show("Selecciona una categoria para actualizar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarFormulario())
            {
                return;
            }

            string pError = string.Empty;
            Categorias cat = ObtenerCategoriaFormulario();
            cat.CategoriaID = _categoriaIdSeleccionada;
            _dao.ActualizarRegistro(cat, out pError);

            if (string.Equals(pError?.Trim(), "Categoria actualizada correctamente", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(pError, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarCategorias();
                ActivarModoCreacion();
            }
            else
            {
                MessageBox.Show(pError, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgv_MostrarCategorias_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (dgv_MostrarCategorias.Rows[e.RowIndex].DataBoundItem is not Categorias cat)
            {
                MessageBox.Show("No se pudo obtener la categoria seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _categoriaIdSeleccionada = cat.CategoriaID;
            txt_Categoria.Text = cat.Nombre;
            txt_Descripcion.Text = cat.Descripcion;
            cbox_Estado.SelectedValue = cat.EstadoId;

            btn_GuardarCategoria.Enabled = false;
            guna2ImageButton2.Enabled = false;
            btn_Actualizar.Enabled = true;
            guna2ImageButton1.Enabled = true;
        }

        private bool ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txt_Categoria.Text))
            {
                MessageBox.Show("Ingresa el nombre de la categoria.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbox_Estado.SelectedValue is null)
            {
                MessageBox.Show("Selecciona un estado para la categoria.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private Categorias ObtenerCategoriaFormulario()
        {
            return new Categorias
            {
                Nombre = txt_Categoria.Text.Trim(),
                Descripcion = txt_Descripcion.Text.Trim(),
                EstadoId = Convert.ToInt32(cbox_Estado.SelectedValue)
            };
        }

        private void ActivarModoCreacion()
        {
            _categoriaIdSeleccionada = 0;
            txt_Categoria.Text = string.Empty;
            txt_Descripcion.Text = string.Empty;
            if (cbox_Estado.Items.Count > 0)
            {
                cbox_Estado.SelectedIndex = 0;
            }

            btn_GuardarCategoria.Enabled = true;
            guna2ImageButton2.Enabled = true;
            btn_Actualizar.Enabled = false;
            guna2ImageButton1.Enabled = false;
        }

        private void CargarCategorias()
        {
            dgv_MostrarCategorias.AutoGenerateColumns = false;
            string pError = string.Empty;
            dgv_MostrarCategorias.DataSource = _dao.ObtenerTodos(out pError);
            if (!string.IsNullOrEmpty(pError))
                MessageBox.Show(pError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            GuardarCategoria();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            ActualizarCategoriaSeleccionada();
        }

        
    }
}
