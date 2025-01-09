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
    public partial class FrmProductos : Form
    {
        private ProductoLogica productoLogica;
        public FrmProductos()
        {
            InitializeComponent();
            ConfigurarDgv();
            ConfigurarTextos();
            productoLogica = new ProductoLogica();
            ConsultarProductos();
        }
        private void ConfigurarDgv()
        {
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.DefaultCellStyle.Font = new Font("Verdana", 10, FontStyle.Regular);
            dgvProductos.DefaultCellStyle.ForeColor = Color.Black;
            dgvProductos.DefaultCellStyle.BackColor = Color.White;
            dgvProductos.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvProductos.DefaultCellStyle.SelectionBackColor = Color.Blue;

            dgvProductos.ColumnHeadersDefaultCellStyle.Font = new Font("Verdana", 12, FontStyle.Bold);
            dgvProductos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProductos.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue;
            dgvProductos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvProductos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProductos.GridColor = Color.LightGray;

            dgvProductos.ColumnHeadersHeight = 35;
            dgvProductos.RowTemplate.Height = 30;
        }
        private void ConfigurarTextos()
        {
            Font fuentePersonalizada = new Font("Arial", 12, FontStyle.Bold);

            txtNombre.Font = fuentePersonalizada;
            txtDescripcion.Font = fuentePersonalizada;
            txtPrecio.Font = fuentePersonalizada;
            txtStock.Font = fuentePersonalizada;
        }
        private void ConsultarProductos()
        {
            dgvProductos.DataSource = productoLogica.ObtenerTodosLosProductos();
            dgvProductos.Columns["DetalleVenta"].Visible = false;
        }
        private void SeleccionarFilaProducto(int productoId)
        {
            foreach (DataGridViewRow row in dgvProductos.Rows)
            {
                if (row.Cells["Id"].Value != null && (int)row.Cells["Id"].Value == productoId)
                {
                    row.Selected = true;
                    break;
                }
            }
        }
        private void MostrarProducto(Producto producto)
        {
            txtNombre.Text = producto.nombre;
            txtDescripcion.Text = producto.descripcion;
            txtPrecio.Text = producto.precio.ToString("0.00");
            txtStock.Text = producto.stock.ToString();
        }
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            txtNombre.Focus();
            ConsultarProductos();
        }
        
        
        private void btnReporteCliente_Click(object sender, EventArgs e)
        {
            FrmReportProductos reporte = new FrmReportProductos();
            reporte.ShowDialog();
        }

        private void btnNuevo_Click_1(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text) || string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("Debe completar los campos Nombre, Precio y Stock.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text.Trim(), out decimal precio) || precio < 0)
            {
                MessageBox.Show("El precio debe ser un valor numérico mayor o igual a 0.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtStock.Text.Trim(), out int stock) || stock < 0)
            {
                MessageBox.Show("El stock debe ser un número entero mayor o igual a 0.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var producto = new Producto
            {
                nombre = txtNombre.Text.Trim(),
                descripcion = txtDescripcion.Text.Trim(),
                precio = precio,
                stock = stock
            };

            bool resultado = productoLogica.CrearProducto(producto);

            if (resultado)
            {
                MessageBox.Show("Producto creado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ConsultarProductos();

                btnNuevo.PerformClick();
            }
            else
            {
                MessageBox.Show("Error al crear el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar un producto para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int productoId = (int)dgvProductos.SelectedRows[0].Cells["Id"].Value;

                var confirmResult = MessageBox.Show(
                    "¿Está seguro de que desea eliminar este producto?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    productoLogica.EliminarProducto(productoId);

                    MessageBox.Show("Producto eliminado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    ConsultarProductos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al eliminar el producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConsultar_Click_1(object sender, EventArgs e)
        {
            try
            {
                string input = Interaction.InputBox("Ingrese el ID del producto:", "Buscar Producto", "", -1, -1);

                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (int.TryParse(input.Trim(), out int id))
                    {
                        var producto = productoLogica.BuscarProductoPorId(id);

                        if (producto != null)
                        {
                            MostrarProducto(producto);

                            dgvProductos.DataSource = new List<Producto> { producto };
                            dgvProductos.ClearSelection();
                            dgvProductos.Rows[0].Selected = true;
                            dgvProductos.CurrentCell = dgvProductos.Rows[0].Cells[0];

                            SeleccionarFilaProducto(producto.Id);
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún producto con ese ID.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ConsultarProductos();
                        }
                    }
                    else
                    {
                        MessageBox.Show("El ID debe ser un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    ConsultarProductos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al consultar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click_1(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un producto para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text) || string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("Debe completar los campos Nombre, Precio y Stock.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text.Trim(), out decimal precio) || !int.TryParse(txtStock.Text.Trim(), out int stock))
            {
                MessageBox.Show("El Precio debe ser un valor numérico y el Stock debe ser un número entero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int productoId = (int)dgvProductos.SelectedRows[0].Cells["Id"].Value;

            var producto = new Producto
            {
                Id = productoId,
                nombre = txtNombre.Text.Trim(),
                descripcion = txtDescripcion.Text.Trim(),
                precio = precio,
                stock = stock
            };

            bool resultado = productoLogica.ActualizarProducto(producto);

            if (resultado)
            {
                MessageBox.Show("Producto modificado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dgvProductos.ClearSelection();
                ConsultarProductos();
                btnNuevo.PerformClick();
            }
            else
            {
                MessageBox.Show("Error al modificar el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
