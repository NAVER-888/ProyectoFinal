﻿using System;
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
    public partial class FrmReportVentas : Form
    {
        public FrmReportVentas()
        {
            InitializeComponent();
        }

        private void FrmReportVentas_Load(object sender, EventArgs e)
        {
            
            this.detalleVentasTableAdapter.Fill(this.bDProyectoFinalVentas.DetalleVentas);
            
            
            

            this.reportViewer1.RefreshReport();
        }

        private void ventaBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
