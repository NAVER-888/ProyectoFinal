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
        private VentaLogica ventaLogica;
        public FrmVentas()
        {
            InitializeComponent();
            clienteLogica = new ClienteLogica();
            productoLogica = new ProductoLogica();
            ventaLogica = new VentaLogica();
        }

        private void FrmVentas_Load(object sender, EventArgs e)
        {
            CargarProductos();
            CargarClientes();
            detalles = new List<DetalleVenta>();
        }
        private void InicializarColumnas(bool esListaGeneral)
        {
            dgvVentas.Columns.Clear(); 

            if (esListaGeneral)
            {
                // Configurar columnas para la lista general
                dgvVentas.Columns.Add("Id", "Id");
                dgvVentas.Columns.Add("Cliente", "Cliente");
                dgvVentas.Columns.Add("Fecha", "Fecha");
                dgvVentas.Columns.Add("Total", "Total");
            }
            else
            {
                // Configurar columnas para agregar/editar ítem
                dgvVentas.Columns.Add("Producto", "Producto");
                dgvVentas.Columns.Add("Precio", "Precio");
                dgvVentas.Columns.Add("Cantidad", "Cantidad");
                dgvVentas.Columns.Add("Subtotal", "Subtotal");

                // Ajustar el formato de las columnas de Precio y Subtotal a un formato numérico
                dgvVentas.Columns["Precio"].DefaultCellStyle.Format = "0.00";
                dgvVentas.Columns["Subtotal"].DefaultCellStyle.Format = "0.00";
            }
        }
        private void ConsultarVentas()
        {
            try
            {
                List<Venta> ventas = ventaLogica.ObtenerTodasLasVentas();
                InicializarColumnas(true);

                dgvVentas.DataSource = ventas;  

                dgvVentas.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al consultar las ventas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void MostrarVenta(Venta venta)
        {
            try
            {
                if (venta == null)
                {
                    MessageBox.Show("La venta no existe o los datos no se pudieron cargar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<Cliente> clientes = clienteLogica.ObtenerTodosLosClientes(); // Obtener la lista de clientes

                Cliente clienteSeleccionado = clientes.FirstOrDefault(c => c.Id == venta.ClienteId);
                if (clienteSeleccionado != null)
                {
                    cboCliente.SelectedItem = clienteSeleccionado; 
                }
                else
                {
                    MessageBox.Show("No se encontró el cliente asociado a esta venta.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                dtpFecha.Value = venta.fecha;

                txtTotal.Text = venta.total.ToString("C"); 

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al mostrar los datos de la venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void txtUnidades_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (decimal.TryParse(txtPrecio.Text.Trim(), out decimal precio) &&
                    int.TryParse(txtUnidades.Text.Trim(), out int cantidad))
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
        private void ActualizarDataGridView()
        {
            try
            {
                dgvVentas.Rows.Clear();

                foreach (var detalle in detalles)
                {
                    Producto productoSeleccionado = productoLogica.BuscarProductoPorId(detalle.ProductoId);

                    dgvVentas.Rows.Add(
                        productoSeleccionado.nombre, 
                        productoSeleccionado.precio.ToString("0.00"), 
                        detalle.cantidad, 
                        detalle.subtotal.ToString("0.00") 
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al actualizar el DataGridView: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarControles();
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

                DetalleVenta detalle = new DetalleVenta
                {
                    ProductoId = productoSeleccionado.Id,
                    cantidad = unidades,
                    subtotal = precio * unidades
                };

                detalles.Add(detalle);
                InicializarColumnas(false);
                ActualizarDataGridView();
                CalcularTotales();
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
                dgvVentas.Rows.Remove(row);
            }

            CalcularTotales();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
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

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                string input = Interaction.InputBox("Ingrese el ID de la venta:", "Buscar Venta", "", -1, -1);

                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (int.TryParse(input.Trim(), out int ventaId))
                    {
                        var venta = ventaLogica.ObtenerVentaPorId(ventaId);

                        if (venta != null)
                        {
                            List<Cliente> clientes = clienteLogica.ObtenerTodosLosClientes(); 
                            Cliente clienteSeleccionado = clientes.FirstOrDefault(c => c.Id == venta.ClienteId);
                            if (clienteSeleccionado != null)
                            {
                                cboCliente.SelectedItem = clienteSeleccionado;  
                            }
                            else
                            {
                                MessageBox.Show("No se encontró el cliente asociado a esta venta.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            dtpFecha.Value = venta.fecha;

                            txtTotal.Text = venta.total.ToString("C"); 
     
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ninguna venta con ese ID.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("El ID debe ser un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Debe ingresar un ID de venta.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al buscar la venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvVentas.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar una venta para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int ventaId = (int)dgvVentas.SelectedRows[0].Cells["Id"].Value;

                DialogResult dialogResult = MessageBox.Show("¿Está seguro de que desea eliminar esta venta?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    bool ventaEliminada = ventaLogica.EliminarVenta(ventaId);

                    if (ventaEliminada)
                    {
                        MessageBox.Show("Venta eliminada correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimpiarControles(); 

                        ConsultarVentas(); 
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar la venta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al eliminar la venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    var venta = ventaLogica.ObtenerVentaPorId(id); 

                    if (venta != null)
                    {
                        dgvVentas.DataSource = new List<Venta> { venta };
                        dgvVentas.ClearSelection();
                        dgvVentas.Rows[0].Selected = true;
                        dgvVentas.CurrentCell = dgvVentas.Rows[0].Cells[0];

                        dtpFecha.Text = venta.fecha.ToString("dd/MM/yyyy");
                        txtTotal.Text = venta.total.ToString("C"); 
                        var cliente = clienteLogica.ObtenerClientePorId(venta.ClienteId);
                        cboCliente.SelectedItem = cliente; 

                        
                        var detallesVenta = ventaLogica.ObtenerDetallesPorVenta(id).ToList(); 
                        dgvVentas.DataSource = detallesVenta; 
                        dgvVentas.ClearSelection();
                    }
                    else
                    {
                    
                        dgvVentas.DataSource = null;
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
    }
}
