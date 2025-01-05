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

namespace ProyectoFinal
{
    public partial class FrmUsuarios : Form
    {
        private UsuarioLogica usuarioLogica;
        public FrmUsuarios()
        {
            InitializeComponent();
            usuarioLogica = new UsuarioLogica();
            CargarRoles();
            ConsultarUsuarios();
        }

        private void CargarRoles()
        {
            cboRol.Items.Add("Seleccionar");
            cboRol.Items.Add("Administrador");
            cboRol.Items.Add("Operador");
            cboRol.SelectedIndex = 0; 
        }
        private void ConsultarUsuarios()
        {
            var usuarios = usuarioLogica.ObtenerUsuarios();
            dgvUsuarios.DataSource = usuarios;
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                var fila = dgvUsuarios.SelectedRows[0];
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtClave.Text = fila.Cells["Clave"].Value.ToString();
                cboRol.SelectedItem = fila.Cells["Rol"].Value.ToString();
                chkEstado.Checked = (bool)fila.Cells["Estado"].Value;
            }
        }
        private void Limpiar()
        {
            txtNombre.Clear();
            txtClave.Clear();
            cboRol.SelectedIndex = 0;
            chkEstado.Checked = true;
            txtNombre.Focus();
            ConsultarUsuarios();
        }
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtClave.Text))
            {
                MessageBox.Show("Debe completar los campos Nombre y Clave.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var usuario = new Usuario
            {
                nombre = txtNombre.Text.Trim(),
                clave = txtClave.Text.Trim(),
                rol = cboRol.SelectedItem.ToString(),
                estado = chkEstado.Checked
            };

            bool resultado;
            if (dgvUsuarios.SelectedRows.Count > 0) 
            {
                usuario.Id = (int)dgvUsuarios.SelectedRows[0].Cells["Id"].Value;
                resultado = usuarioLogica.ActualizarUsuario(usuario);
            }
            else 
            {
                resultado = usuarioLogica.CrearUsuario(usuario);
            }

            if (resultado)
            {
                MessageBox.Show("Usuario guardado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ConsultarUsuarios();
                btnNuevo.PerformClick();
            }
            else
            {
                MessageBox.Show("Error al guardar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un usuario.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var idUsuario = (int)dgvUsuarios.SelectedRows[0].Cells["Id"].Value;
            

            if (usuarioLogica.EliminarUsuario(idUsuario))
            {
                MessageBox.Show("Usuario eliminado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Limpiar();
            }
            else
            {
                MessageBox.Show("Error al eliminar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreBusqueda = txtNombre.Text.Trim();

                if (string.IsNullOrEmpty(nombreBusqueda))
                {
                    MessageBox.Show("Por favor, ingrese un nombre para buscar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var usuarioLogica = new UsuarioLogica();
                var resultados = usuarioLogica.BuscarUsuariosPorNombre(nombreBusqueda);

                if (resultados.Count > 0)
                {
                    dgvUsuarios.DataSource = resultados;

                    dgvUsuarios.ClearSelection();
                    dgvUsuarios.Rows[0].Selected = true;
                    dgvUsuarios.CurrentCell = dgvUsuarios.Rows[0].Cells[0];

                    var usuarioSeleccionado = (Usuario)dgvUsuarios.Rows[0].DataBoundItem;
                    txtNombre.Text = usuarioSeleccionado.nombre;
                    txtClave.Text = usuarioSeleccionado.clave;
                    cboRol.SelectedItem = usuarioSeleccionado.rol;
                    chkEstado.Checked = usuarioSeleccionado.estado;
                }
                else
                {
                    dgvUsuarios.DataSource = null; 
                    MessageBox.Show("No se encontraron usuarios con ese nombre.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al consultar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un usuario para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtClave.Text))
            {
                MessageBox.Show("Debe completar los campos Nombre y Clave.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int usuarioId = (int)dgvUsuarios.SelectedRows[0].Cells["Id"].Value;

            var usuario = new Usuario
            {
                Id = usuarioId,
                nombre = txtNombre.Text.Trim(),
                clave = txtClave.Text.Trim(),
                rol = cboRol.SelectedItem.ToString(),
                estado = chkEstado.Checked
            };

            bool resultado = usuarioLogica.ActualizarUsuario(usuario);

            if (resultado)
            {
                MessageBox.Show("Usuario modificado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dgvUsuarios.ClearSelection();
                ConsultarUsuarios(); 
                btnNuevo.PerformClick(); 
            }
            else
            {
                MessageBox.Show("Error al modificar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
