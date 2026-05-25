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
    public partial class UC_Facturacion : UserControl
    {
        public UC_Facturacion()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void UC_Facturacion_Load(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            Descuento descuento = new Descuento();
            descuento.Show();
        }
    }
}
