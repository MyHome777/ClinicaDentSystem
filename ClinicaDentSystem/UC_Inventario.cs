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
    public partial class UC_Inventario : UserControl
    {
        public UC_Inventario()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
        }

        private void guna2ImageButton7_Click(object sender, EventArgs e)
        {
            Servicio servicio = new Servicio();
            servicio.Show();
        }

        private void guna2ImageButton8_Click(object sender, EventArgs e)
        {
            Servicio servicio = new Servicio();
            servicio.Show();
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            Inventario inventario = new Inventario();
            inventario.Show();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            EditInventario editInventario = new EditInventario();
            editInventario.Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Proveedor proveedor = new Proveedor();
            proveedor.Show();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Categoria categoria = new Categoria();
            categoria.Show();
        }

        private void guna2ImageButton5_Click(object sender, EventArgs e)
        {
            Categoria categoria = new Categoria();
            categoria.Show();
        }

        private void guna2ImageButton10_Click(object sender, EventArgs e)
        {
            Proveedor proveedor = new Proveedor();
            proveedor.Show();
        }
    }
}
