using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaLogica;

namespace ProyectoFinal
{
    public partial class FrmAcceso : Form
    {
        private int intentosFallidos = 0;
        private UsuarioLogica usuarioLogica;
        public FrmAcceso()
        {
            InitializeComponent();
            ConfigurarTextos();
            usuarioLogica = new UsuarioLogica();
            this.KeyPreview = true;
        }
        private void ConfigurarTextos()
        {
            Font fuentePersonalizada = new Font("Arial", 12, FontStyle.Bold);

            txtNombre.Font = fuentePersonalizada;
            txtClave.Font = fuentePersonalizada;
        }
        private void chkMostrarClave_CheckedChanged(object sender, EventArgs e)
        {
            txtClave.UseSystemPasswordChar = !chkMostrar.Checked;
        }

        private void btnAcceder_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string clave = txtClave.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(clave))
            {
                MessageBox.Show("Debe ingresar tanto el nombre de usuario como la clave.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Debe ingresar el nombre de usuario.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (string.IsNullOrWhiteSpace(clave))
            {
                MessageBox.Show("Debe ingresar la clave del usuario.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var usuario = usuarioLogica.ValidarUsuario(nombre, clave);

            if (usuario != null)
            {
                if (usuario.rol == "Administrador")
                {
                    MessageBox.Show($"Bienvenido, {usuario.nombre}. Rol: Administrador", "Acceso permitido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FrmPrincipal frmPrincipal = new FrmPrincipal(usuario);
                    frmPrincipal.Show();
                    this.Hide();
                }
                else if (usuario.rol == "Operador")
                {
                    MessageBox.Show($"Bienvenido, {usuario.nombre}. Rol: Operador", "Acceso permitido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FrmPrincipal frmPrincipal = new FrmPrincipal(usuario);
                    frmPrincipal.Show();
                    this.Hide();
                }
            }
            else
            {
                intentosFallidos++;

                MessageBox.Show("Usuario o clave incorrectos. Intente nuevamente.", "Error de acceso", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (intentosFallidos >= 3)
                {
                    MessageBox.Show("Ha excedido el número de intentos permitidos. El sistema se cerrará.", "Acceso bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
        }

        private void FrmAcceso_Load(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void FormAcceso_KeyDown(object sender, KeyEventArgs e) 
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnAcceder.PerformClick(); 
            }
        }
        private void txtNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAcceder.PerformClick();
            }
        }

        private void txtClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAcceder.PerformClick();
            }
        }
    }
}
