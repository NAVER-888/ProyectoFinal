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
    public partial class FrmVentas : Form
    {
        private ClienteLogica clienteLogica;
        private ProductoLogica productoLogica;
        private List<DetalleVenta> detalles = new List<DetalleVenta>();
        private List<object> detallesView = new List<object>();
        private VentaLogica ventaLogica;
        public FrmVentas()
        {
            InitializeComponent();
            clienteLogica = new ClienteLogica();
            productoLogica = new ProductoLogica();
            ventaLogica = new VentaLogica();
            detalles = new List<DetalleVenta>();
            ConsultarVentas();
        }

        private void FrmVentas_Load(object sender, EventArgs e)
        {
            CargarProductos();
            CargarClientes();
            var detalles = ventaLogica.ObtenerDetalleVentas();  
            dgvVentas.DataSource = detalles;
        }
        
        private void InicializarColumnas()
        {
           
            dgvVentas.Columns.Clear();

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductoId",
                HeaderText = "Producto ID",  
                DataPropertyName = "ProductoId", 
                Visible = false, 
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "VentaId",
                HeaderText = "ID Venta",
                DataPropertyName = "VentaId", 
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Cliente",
                HeaderText = "Cliente",   
                DataPropertyName = "Cliente",  
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Producto",
                HeaderText = "Producto",
                DataPropertyName = "Producto",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Descripcion",
                HeaderText = "Descripcion",
                DataPropertyName = "descripcion", 
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Precio",
                HeaderText = "Precio",
                DataPropertyName = "precio", 
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }, 
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Cantidad",
                HeaderText = "Cantidad",
                DataPropertyName = "cantidad", 
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Subtotal",
                HeaderText = "Subtotal",
                DataPropertyName = "subtotal", 
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }, 
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }
        private void ConsultarVentas()
        {
            try
            {
                InicializarColumnas();
                var ventas = ventaLogica.ObtenerDetalleVentas();

                dgvVentas.AutoGenerateColumns = false;
                dgvVentas.DataSource = ventas;
                dgvVentas.ClearSelection();
                ActualizarTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al consultar las ventas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarControles()
        {
            try
            {
                cboCliente.SelectedItem = null; 

                dtpFecha.Value = DateTime.Now; 

                txtTotal.Text = string.Empty;

                dgvVentas.DataSource = null; 
                dgvVentas.ClearSelection(); 

                dgvVentas.ClearSelection();
                detallesView.Clear();

                ConsultarVentas();
                ActualizarTotal();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al limpiar los controles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarClientes()
        {
            try
            {
                var clientes = clienteLogica.ObtenerTodosLosClientes();

                if (clientes != null && clientes.Count > 0)
                {
                    cboCliente.DataSource = clientes;
                    cboCliente.DisplayMember = "nombre"; 
                    cboCliente.ValueMember = "Id";     
                }
                else
                {
                    MessageBox.Show("No se encontraron clientes registrados.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboCliente.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cargar los clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarProductos()
        {
            try
            {
                var productos = productoLogica.ObtenerTodosLosProductos();

                if (productos != null && productos.Count > 0)
                {
                    cboProducto.DataSource = productos;
                    cboProducto.DisplayMember = "nombre"; 
                    cboProducto.ValueMember = "Id";     
                }
                else
                {
                    MessageBox.Show("No se encontraron productos disponibles.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboProducto.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cargar los productos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cboProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboProducto.SelectedItem is Producto productoSeleccionado)
                {
                    txtPrecio.Text = productoSeleccionado.precio.ToString("0.00");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cargar el precio del producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ActualizarSubtotal()
        {
            try
            {
                if (decimal.TryParse(txtPrecio.Text.Trim(), out decimal precio) &&
                    int.TryParse(txtUnidades.Text.Trim(), out int cantidad) && cantidad >= 0)
                {
                    decimal subtotal = precio * cantidad;
                    txtSubtotal.Text = subtotal.ToString("0.00");
                }
                else
                {
                    txtSubtotal.Text = "0.00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al calcular el subtotal: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {
            ActualizarSubtotal();
        }
        private void txtUnidades_TextChanged(object sender, EventArgs e)
        {
            ActualizarSubtotal();
        }
        private void ActualizarTotal()
        {
            try
            {
                decimal total = 0;

                foreach (DataGridViewRow row in dgvVentas.Rows)
                {
                    if (row.Cells["Subtotal"].Value != null)
                    {
                        total += Convert.ToDecimal(row.Cells["Subtotal"].Value); 
                    }
                }

                decimal igv = total * 0.18m;
                decimal neto = total + igv;

                txtTotal.Text = total.ToString("F2");
                txtIGV.Text = igv.ToString("F2");  
                txtNeto.Text = neto.ToString("F2"); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar los totales: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CalcularTotales()
        {
            try
            {
                decimal total = 0;

                foreach (DataGridViewRow row in dgvVentas.Rows)
                {
                    if (row.Cells["Subtotal"].Value != null)
                    {
                        if (decimal.TryParse(row.Cells["Subtotal"].Value.ToString(), out decimal subtotal))
                        {
                            total += subtotal;
                        }
                    }
                }

                decimal igv = total * 0.18m;

                decimal neto = total + igv;

                txtTotal.Text = total.ToString("0.00");
                txtIGV.Text = igv.ToString("0.00");
                txtNeto.Text = neto.ToString("0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al calcular los totales: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
     
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboProducto.SelectedItem == null || string.IsNullOrWhiteSpace(txtUnidades.Text))
                {
                    MessageBox.Show("Debe seleccionar un producto y especificar las unidades.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtUnidades.Text.Trim(), out int unidades) || unidades <= 0)
                {
                    MessageBox.Show("Las unidades deben ser un número entero positivo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Producto productoSeleccionado = (Producto)cboProducto.SelectedItem;
                decimal precio = productoSeleccionado.precio;
                Cliente clienteSeleccionado = (Cliente)cboCliente.SelectedItem;

                DetalleVenta detalle = new DetalleVenta
                {
                    ProductoId = productoSeleccionado.Id,
                    cantidad = unidades,
                    subtotal = precio * unidades
                };

                detalles.Add(detalle);

                var detalleAnonimo = new
                {
                    ProductoId = productoSeleccionado.Id,
                    Producto = productoSeleccionado.nombre,
                    Descripcion = productoSeleccionado.descripcion,
                    Precio = precio,
                    Cantidad = unidades,
                    Subtotal = precio * (decimal)unidades,
                    Cliente = clienteSeleccionado?.nombre ?? "Sin Cliente"
                    
            };

                detallesView.Add(detalleAnonimo);

                dgvVentas.DataSource = null;  
                dgvVentas.DataSource = detallesView;  

                if (dgvVentas.Columns["VentaId"] != null)
                {
                    dgvVentas.Columns["VentaId"].Visible = false;
                }

                CalcularTotales();
                ActualizarTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al agregar el ítem: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvVentas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un item para quitar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (DataGridViewRow row in dgvVentas.SelectedRows)
            {
                var productoId = (int)row.Cells[0].Value; 

                var detalleAEliminar = detalles.FirstOrDefault(d => d.ProductoId == productoId);

                if (detalleAEliminar != null)
                {
                    detalles.Remove(detalleAEliminar);
                }

                detallesView.RemoveAt(row.Index);
            }

            dgvVentas.DataSource = null;
            dgvVentas.DataSource = detallesView;

            CalcularTotales();
            ActualizarTotal();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            FrmReportVentas reporte = new FrmReportVentas();
            reporte.ShowDialog();
        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (cboCliente.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar un cliente.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (detalles.Count == 0)
                {
                    MessageBox.Show("Debe agregar al menos un producto al detalle.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Venta nuevaVenta = new Venta
                {
                    ClienteId = ((Cliente)cboCliente.SelectedItem).Id,
                    fecha = dtpFecha.Value,
                    total = detalles.Sum(d => d.subtotal)
                };

                bool ventaGuardada = ventaLogica.GuardarVentaConDetalles(nuevaVenta, detalles);

                if (ventaGuardada)
                {
                    MessageBox.Show("Venta registrada correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    detallesView.Clear();
                    btnNuevo.PerformClick();
                }
                else
                {
                    MessageBox.Show("Error al guardar la venta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al guardar la venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvVentas.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar un ítem para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var filaSeleccionada = dgvVentas.SelectedRows[0];
                int productoId = (int)filaSeleccionada.Cells["ProductoId"].Value;
                int cantidad = (int)filaSeleccionada.Cells["Cantidad"].Value; 
                string ventaIdStr = filaSeleccionada.Cells["VentaId"].Value?.ToString();

                if (ventaIdStr == "N/A" || string.IsNullOrEmpty(ventaIdStr))
                {
                    MessageBox.Show("El ítem no pertenece a una venta guardada. Use el botón Quitar si aún no se ha guardado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int ventaId = int.Parse(ventaIdStr); 

                VentaLogica logica = new VentaLogica();
                bool eliminado = logica.EliminarDetalleVenta(ventaId, productoId, cantidad);

                if (eliminado)
                {
                    var detalleAEliminar = detalles.FirstOrDefault(d => d.VentaId == ventaId && d.ProductoId == productoId);
                    if (detalleAEliminar != null)
                    {
                        detalles.Remove(detalleAEliminar); 
                    }

                    decimal totalActualizado = detalles
                        .Where(d => d.VentaId == ventaId)
                        .Sum(d => d.subtotal); 

                    if (totalActualizado == 0)
                    {
                        bool ventaEliminada = logica.EliminarVenta(ventaId); 

                        if (ventaEliminada)
                        {
                            detalles.RemoveAll(d => d.VentaId == ventaId); 
                            MessageBox.Show("La venta fue eliminada completamente ya que no tiene más detalles.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Ocurrió un error al intentar eliminar la venta con total 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        bool actualizado = logica.ActualizarTotalVenta(ventaId, totalActualizado);

                        if (!actualizado)
                        {
                            MessageBox.Show("Ocurrió un error al actualizar el total de la venta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    dgvVentas.DataSource = null;  
                    dgvVentas.DataSource = detalles;  

                    CalcularTotales();
                    btnNuevo.PerformClick();
                }
                else
                {
                    MessageBox.Show("Ocurrió un error al eliminar el ítem.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al eliminar el ítem: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                string input = Interaction.InputBox("Ingrese el ID de la venta:", "Buscar Venta", "", -1, -1);

                if (string.IsNullOrEmpty(input))
                {
                    MessageBox.Show("Por favor, ingrese un ID para buscar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (int.TryParse(input, out int id))
                {
                    var ventaLogica = new VentaLogica();
                    var detalles = ventaLogica.ConsultarVentas(id);

                    if (detalles.Any())
                    {
                        dgvVentas.DataSource = detalles;
                        dgvVentas.ClearSelection();
                        InicializarColumnas();

                        dgvVentas.Columns.Remove("ProductoId");
                        ActualizarTotal();

                        MessageBox.Show("Venta consultada correctamente.");
                    }
                    else
                    {
                        dgvVentas.DataSource = null;
                        MessageBox.Show("No se encontró ninguna venta con ese ID.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("El ID debe ser un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al consultar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        
    }
}
