using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos;

namespace CapaLogica
{
    public class UsuarioLogica
    {
        private readonly EntidadesContainer _context;

        public UsuarioLogica()
        {
            _context = new EntidadesContainer();
        }

        public Usuario ValidarUsuario(string nombre, string clave)
        {
            return _context.Usuarios
                           .Where(u => u.nombre == nombre && u.clave == clave && u.estado)
                           .SingleOrDefault();
        }

        public List<Usuario> ObtenerUsuarios()
        {
            return _context.Usuarios.ToList();
        }

        public bool CrearUsuario(Usuario usuario)
        {
            if (_context.Usuarios.Any(u => u.nombre.Equals(usuario.nombre, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Nombre de usuario ya existe. Intenta con otro.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            try
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ActualizarUsuario(Usuario usuario)
        {
            try
            {
                var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.Id == usuario.Id);
                if (usuarioExistente != null)
                {
                    usuarioExistente.nombre = usuario.nombre;
                    usuarioExistente.clave = usuario.clave;
                    usuarioExistente.rol = usuario.rol;
                    usuarioExistente.estado = usuario.estado;

                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool EliminarUsuario(int id)
        {
            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
                if (usuario != null)
                {
                    _context.Usuarios.Remove(usuario);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        
        public Usuario ObtenerUsuarioPorId(int id)
        {
            return _context.Usuarios.Find(id);
        }
    }
}
