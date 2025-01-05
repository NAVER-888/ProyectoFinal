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
        private int _intentosFallidos = 0;
        private UsuarioLogica usuarioLogica;
        public FrmAcceso()
        {
            InitializeComponent();
            usuarioLogica = new UsuarioLogica();
        }

        private void btnAcceder_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string clave = txtClave.Text.Trim();

            // Verificar si los campos están vacíos
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(clave))
            {
                MessageBox.Show("Debe ingresar tanto el nombre de usuario como la clave.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var usuario = usuarioLogica.ValidarUsuario(nombre, clave);

            if (usuario != null)
            {
                if (usuario.rol == "Administrador")
                {
                    // Si es administrador, abrir el formulario principal
                    MessageBox.Show($"Bienvenido, {usuario.nombre}. Rol: Administrador", "Acceso permitido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FrmPrincipal frmPrincipal = new FrmPrincipal(usuario);
                    frmPrincipal.Show();
                    this.Hide();
                }
                else if (usuario.rol == "Operador")
                {
                    // Si es operador, abrir el formulario principal con acceso restringido
                    MessageBox.Show($"Bienvenido, {usuario.nombre}. Rol: Operador", "Acceso permitido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FrmPrincipal frmPrincipal = new FrmPrincipal(usuario);
                    frmPrincipal.Show();
                    this.Hide();
                }
            }
            else
            {
                // Si el usuario no es válido, aumentar el contador de intentos fallidos
                _intentosFallidos++;

                // Mostrar mensaje de error
                MessageBox.Show("Usuario o clave incorrectos. Intente nuevamente.", "Error de acceso", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Verificar si se han excedido los intentos
                if (_intentosFallidos >= 3)
                {
                    MessageBox.Show("Ha excedido el número de intentos permitidos. El sistema se cerrará.", "Acceso bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit(); // Cerrar la aplicación después de 3 intentos fallidos
                }
            }
        }
    }
}
