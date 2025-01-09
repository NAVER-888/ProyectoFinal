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
            ConfigurarDgv();
            _consultaLogica = new ConsultaLogica(new EntidadesContainer());
        }
        private void ConfigurarDgv()
        {
            dgvConsultas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvConsultas.DefaultCellStyle.Font = new Font("Verdana", 10, FontStyle.Regular);
            dgvConsultas.DefaultCellStyle.ForeColor = Color.Black;
            dgvConsultas.DefaultCellStyle.BackColor = Color.White;
            dgvConsultas.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvConsultas.DefaultCellStyle.SelectionBackColor = Color.Blue;

            dgvConsultas.ColumnHeadersDefaultCellStyle.Font = new Font("Verdana", 12, FontStyle.Bold);
            dgvConsultas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvConsultas.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue;
            dgvConsultas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvConsultas.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvConsultas.GridColor = Color.LightGray;

            dgvConsultas.ColumnHeadersHeight = 35;
            dgvConsultas.RowTemplate.Height = 30;
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
                            if (dgvConsultas.Columns["Descripcion"] != null)
                            {
                                dgvConsultas.Columns["Descripcion"].HeaderText = "Categoria";
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
                    DateTime? fechaSeleccionada = MostrarSeleccionFecha();
                    if (fechaSeleccionada.HasValue)
                    {
                        var ventaLogica = new ConsultaLogica(new EntidadesContainer());
                        var ventas = ventaLogica.ConsultarVentasPorFecha(fechaSeleccionada.Value);

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
                    else
                    {
                        MessageBox.Show("No se seleccionó ninguna fecha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al consultar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private DateTime? MostrarSeleccionFecha()
        {
            Form formFecha = new Form
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Seleccionar Fecha",
                StartPosition = FormStartPosition.CenterScreen
            };

            DateTimePicker dtp = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Location = new Point(50, 20),
                Width = 200
            };

            Button btnAceptar = new Button
            {
                Text = "Aceptar",
                DialogResult = DialogResult.OK,
                Location = new Point(50, 60),
                Width = 100
            };

            formFecha.Controls.Add(dtp);
            formFecha.Controls.Add(btnAceptar);
            formFecha.AcceptButton = btnAceptar;

            if (formFecha.ShowDialog() == DialogResult.OK)
            {
                return dtp.Value.Date; 
            }

            return null; 
        }
    }
}
