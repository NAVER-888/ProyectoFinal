using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos;
using CapaLogica;
using Microsoft.VisualBasic;

namespace ProyectoFinal
{
    public partial class FrmConsultas : Form
    {
        private readonly ConsultaLogica _consultaLogica;
        public FrmConsultas()
        {
            InitializeComponent();
            _consultaLogica = new ConsultaLogica(new EntidadesContainer());
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult opcion = MessageBox.Show("¿Desea buscar por ID de Venta? (Presione 'No' para buscar por Fecha)",
                                                      "Consulta Ventas",
                                                      MessageBoxButtons.YesNoCancel,
                                                      MessageBoxIcon.Question);

                if (opcion == DialogResult.Yes) 
                {
                    string input = Interaction.InputBox("Ingrese el ID de la venta:", "Buscar Venta", "", -1, -1);

                    if (string.IsNullOrEmpty(input))
                    {
                        MessageBox.Show("Por favor, ingrese un ID para buscar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (int.TryParse(input, out int id))
                    {
                        var ventaLogica = new ConsultaLogica(new EntidadesContainer());
                        var detalles = ventaLogica.ConsultarVentasPorId(id);

                        if (detalles.Any()) 
                        {
                            dgvConsultas.DataSource = detalles; 
                            dgvConsultas.ClearSelection();
                            if (dgvConsultas.Columns["Subtotal"] != null)
                            {
                                dgvConsultas.Columns["Subtotal"].Visible = false;
                            }
                            MessageBox.Show("Venta consultada correctamente.");
                        }
                        else
                        {
                            dgvConsultas.DataSource = null;
                            MessageBox.Show("No se encontró ninguna venta con ese ID.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("El ID debe ser un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (opcion == DialogResult.No) 
                {
                    DateTime fechaSeleccionada = dtpFecha.Value.Date;
                    var ventaLogica = new ConsultaLogica(new EntidadesContainer());
                    var ventas = ventaLogica.ConsultarVentasPorFecha(fechaSeleccionada);

                    if (ventas.Any())
                    {
  
                        dgvConsultas.DataSource = ventas;
                        dgvConsultas.ClearSelection();
                        MessageBox.Show("Ventas consultadas correctamente.");
                    }
                    else
                    {
                        dgvConsultas.DataSource = null;
                        MessageBox.Show("No se encontraron ventas para la fecha seleccionada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al consultar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
