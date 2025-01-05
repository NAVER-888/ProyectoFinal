using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFinal
{
    public partial class FrmSplash : Form
    {
        public FrmSplash()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void FrmSplash_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            progressBar1.Increment(2);
            if (progressBar1.Value == 100)
            {
                timer1.Enabled = false;
                FrmAcceso acceso = new FrmAcceso();
                acceso.Show();
                this.Hide();
            }
        }
    }
}
