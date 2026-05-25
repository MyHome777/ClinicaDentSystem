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
    public partial class UC_Citas : UserControl
    {
        public UC_Citas()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Citas citas = new Citas();
            citas.Show();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            Citas citas = new Citas();
            citas.Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Citas citas = new Citas();
            citas.Show();
        }

        private void guna2ImageButton5_Click(object sender, EventArgs e)
        {
            Citas citas = new Citas();
            citas.Show();
        }
    }
}
