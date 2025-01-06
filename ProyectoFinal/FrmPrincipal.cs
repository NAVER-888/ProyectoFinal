using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using CapaDatos;
namespace ProyectoFinal
{
    public partial class FrmPrincipal : Form
    {
        private Usuario _usuario;
        public FrmPrincipal(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
            this.Load += FrmPrincipal_Load;
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg,int wparam, int lparam);

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            lblUsuario.Text = $"Bienvenido, {_usuario.nombre}";  
            lblRol.Text = $"Rol: {_usuario.rol}";
        }
        private void btnSlide_Click(object sender, EventArgs e)
        {
            if (MenuVertical.Width == 260)
            {
                MenuVertical.Width = 70;
            }
            else
                MenuVertical.Width = 260;
        }

        private void iconcerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconmaximizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            iconrestaurar.Visible = false;
            iconmaximizar.Visible = true;
        }

        private void iconrestaurar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            iconrestaurar.Visible = false;
            iconmaximizar.Visible = true;
        }

        private void iconminimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        private void AbrirFromInPanel(object Formhijo)
        {
            if (this.PanelContenedor.Controls.Count > 0)
                this.PanelContenedor.Controls.RemoveAt(0);
            Form fh = Formhijo as Form;
            fh.TopLevel = false;
            fh.Dock = DockStyle.Fill;
            this.PanelContenedor.Controls.Add(fh);
            this.PanelContenedor.Tag = fh;
            fh.Show();
        }
        private void btnClientes_Click(object sender, EventArgs e)
        {
            AbrirFromInPanel(new FrmClientes());
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            AbrirFromInPanel(new FrmProductos());
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            AbrirFromInPanel(new FrmVentas());
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            AbrirFromInPanel(new FrmConsultas());
        }

        private void btnUsuario_Click(object sender, EventArgs e)
        {
            AbrirFromInPanel(new FrmUsuarios());
        }

        private void MenuVertical_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
