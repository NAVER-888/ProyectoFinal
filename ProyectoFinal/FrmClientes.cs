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
    public partial class FrmClientes : Form
    {
        private ClienteLogica clienteLogica;
        public FrmClientes()
        {
            InitializeComponent();
            clienteLogica = new ClienteLogica();
            ConsultarClientes();
        }
        private void ConsultarClientes()
        {
            dgvClientes.DataSource = clienteLogica.ObtenerTodosLosClientes();
        }
        private void SeleccionarFilaCliente(int clienteId)
        {
            foreach (DataGridViewRow row in dgvClientes.Rows)
            {
                if (row.Cells["Id"].Value != null && (int)row.Cells["Id"].Value == clienteId)
                {
                    row.Selected = true;
                    break;
                }
            }
        }
        private void MostrarCliente(Cliente cliente)
        {
            txtNombre.Text = cliente.nombre;
            txtTelefono.Text = cliente.telefono;
            txtDireccion.Text = cliente.direccion;
            txtEmail.Text = cliente.correo;
        }
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            txtEmail.Clear();
            txtNombre.Focus();
            ConsultarClientes();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                MessageBox.Show("Debe completar los campos Nombre y Teléfono.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cliente = new Cliente
            {
                nombre = txtNombre.Text.Trim(),
                telefono = txtTelefono.Text.Trim(),
                direccion = txtDireccion.Text.Trim(),
                correo = txtEmail.Text.Trim()
            };

            bool resultado = clienteLogica.CrearCliente(cliente);

            if (resultado)
            {
                MessageBox.Show("Cliente creado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ConsultarClientes();

                btnNuevo.PerformClick();
            }
            else
            {
                MessageBox.Show("Error al crear el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                string input = Interaction.InputBox("Ingrese el ID del cliente:", "Buscar Cliente", "", -1, -1);

                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (int.TryParse(input.Trim(), out int id))
                    {
                        var cliente = clienteLogica.ObtenerClientePorId(id);

                        if (cliente != null)
                        {
                            MostrarCliente(cliente);

                            dgvClientes.DataSource = new List<Cliente> { cliente };
                            dgvClientes.ClearSelection();
                            dgvClientes.Rows[0].Selected = true;
                            dgvClientes.CurrentCell = dgvClientes.Rows[0].Cells[0];

                            SeleccionarFilaCliente(cliente.Id);
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún cliente con ese ID.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ConsultarClientes();
                        }
                    }
                    else
                    {
                        MessageBox.Show("El ID debe ser un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    ConsultarClientes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al consultar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvClientes.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar un cliente para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int clienteId = (int)dgvClientes.SelectedRows[0].Cells["Id"].Value;

                var confirmResult = MessageBox.Show(
                    "¿Está seguro de que desea eliminar este cliente?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    clienteLogica.EliminarCliente(clienteId);

                    MessageBox.Show("Cliente eliminado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    ConsultarClientes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al eliminar el cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un cliente para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                MessageBox.Show("Debe completar los campos Nombre y Teléfono.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int clienteId = (int)dgvClientes.SelectedRows[0].Cells["Id"].Value;

            var cliente = new Cliente
            {
                Id = clienteId,
                nombre = txtNombre.Text.Trim(),
                telefono = txtTelefono.Text.Trim(),
                direccion = txtDireccion.Text.Trim(),
                correo = txtEmail.Text.Trim()
            };

            bool resultado = clienteLogica.ActualizarCliente(cliente);

            if (resultado)
            {
                MessageBox.Show("Cliente modificado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dgvClientes.ClearSelection();
                ConsultarClientes();
                btnNuevo.PerformClick();
            }
            else
            {
                MessageBox.Show("Error al modificar el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnReporteCliente_Click(object sender, EventArgs e)
        {
            FrmReportCliente reporte = new FrmReportCliente();
            reporte.ShowDialog();
        }
    }
}
