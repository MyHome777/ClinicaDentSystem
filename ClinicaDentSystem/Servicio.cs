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
    public partial class Servicio : Form
    {
        public int idServicioEditar = 0;
        public Servicio()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
            cmbEstado.Items.Clear();
            cmbEstado.Items.Add("Activo");
            cmbEstado.Items.Add("Inactivo");
            cmbEstado.SelectedIndex = 0;
        }
        public void CargarDatosEdicion(int id, string nombre, string desc, decimal precio, int estado)
        {
            idServicioEditar = id;

            txtNombreServicio.Text = nombre;
            txtDescripcion.Text = desc;
            txtPrecio.Text = precio.ToString();
            cmbEstado.SelectedIndex = (estado == 1) ? 0 : 1;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                MODELOS.Servicios s = new MODELOS.Servicios();
                s.NombreServicio = txtNombreServicio.Text;
                s.Descripcion = txtDescripcion.Text;
                s.Precio = decimal.Parse(txtPrecio.Text);
                s.EstadoID = (cmbEstado.SelectedItem.ToString() == "Activo") ? 1 : 2;

                DAO.ServiciosDAO dao = new DAO.ServiciosDAO();

                if (idServicioEditar > 0)
                {
                    s.ServicioID = idServicioEditar;
                    dao.Actualizar(s);
                    MessageBox.Show("Actualizado correctamente.");
                }
                else
                {
                    dao.Insertar(s);
                    MessageBox.Show("Guardado correctamente.");
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
