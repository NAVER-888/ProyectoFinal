using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;

namespace CapaLogica
{
    public class ClienteLogica
    {
        private readonly EntidadesContainer _context;

        public ClienteLogica()
        {
            _context = new EntidadesContainer();
        }
        public List<Cliente> ObtenerTodosLosClientes()
        {
            try
            {
                return _context.Clientes.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los clientes.", ex);
            }
        }

        public Cliente ObtenerClientePorId(int id)
        {
            return _context.Clientes.FirstOrDefault(c => c.Id == id);
        }

        public bool CrearCliente(Cliente cliente)
        {
            try
            {
                if (_context.Clientes.Any(c => c.nombre == cliente.nombre))
                {
                    return false; 
                }

                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                return true; 
            }
            catch (Exception)
            {
                return false; 
            }
        }

        public void EliminarCliente(int id)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == id);
            if (cliente != null)
            {
                var ventas = _context.Ventas.Where(v => v.ClienteId == id).ToList();
                foreach (var venta in ventas)
                {
                    var detalles = _context.DetalleVentas.Where(d => d.VentaId == venta.Id).ToList();
                    _context.DetalleVentas.RemoveRange(detalles); 
                    _context.Ventas.Remove(venta);              
                }

                _context.Clientes.Remove(cliente);
                _context.SaveChanges();
            }
        }

        public bool ActualizarCliente(Cliente cliente)
        {
            try
            {
                var clienteExistente = _context.Clientes.FirstOrDefault(c => c.Id == cliente.Id);
                if (clienteExistente == null)
                {
                    return false; 
                }

                clienteExistente.nombre = cliente.nombre;
                clienteExistente.direccion = cliente.direccion;
                clienteExistente.telefono = cliente.telefono;
                clienteExistente.correo = cliente.correo;

                _context.SaveChanges();
                return true; 
            }
            catch (Exception)
            {
                return false; 
            }
        }
    }
}
