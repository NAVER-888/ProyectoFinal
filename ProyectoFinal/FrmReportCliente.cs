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
    public partial class FrmReportCliente : Form
    {
        public FrmReportCliente()
        {
            InitializeComponent();
        }

        private void FrmReportCliente_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'bDProyectoFinalCliente.Clientes' Puede moverla o quitarla según sea necesario.
            this.clientesTableAdapter.Fill(this.bDProyectoFinalCliente.Clientes);

            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }

        private void clienteBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
