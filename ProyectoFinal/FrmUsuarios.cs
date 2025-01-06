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

            bool resultado = usuarioLogica.CrearUsuario(usuario);

            if (resultado)
            {
                MessageBox.Show("Usuario creado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ConsultarUsuarios();
                btnNuevo.PerformClick();
            }
            else
            {
                MessageBox.Show("Error al crear el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsuarios.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar un usuario.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var idUsuario = (int)dgvUsuarios.SelectedRows[0].Cells["Id"].Value;
                var confirmResult = MessageBox.Show(
                        "¿Está seguro de que desea eliminar este producto?",
                        "Confirmar eliminación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    usuarioLogica.EliminarUsuario(idUsuario);
                
                    MessageBox.Show("Usuario eliminado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al eliminar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                string input = Interaction.InputBox("Ingrese el ID del Usuario:", "Buscar Usuario", "", -1, -1);

                if (string.IsNullOrEmpty(input))
                {
                    MessageBox.Show("Por favor, ingrese un ID para buscar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (int.TryParse(input, out int id))
                {
                    var usuarioLogica = new UsuarioLogica();
                    var usuario = usuarioLogica.ObtenerUsuarioPorId(id);

                    if (usuario != null)
                    {
                        dgvUsuarios.DataSource = new List<Usuario> { usuario }; 
                        dgvUsuarios.ClearSelection();
                        dgvUsuarios.Rows[0].Selected = true;
                        dgvUsuarios.CurrentCell = dgvUsuarios.Rows[0].Cells[0];

                        txtNombre.Text = usuario.nombre;
                        txtClave.Text = usuario.clave;
                        cboRol.SelectedItem = usuario.rol;
                        chkEstado.Checked = usuario.estado;
                    }
                    else
                    {
                        dgvUsuarios.DataSource = null;
                        MessageBox.Show("No se encontró ningún usuario con ese ID.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {

        }
    }
}
