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

        public Cliente ObtenerClientePorId(int id)
        {
            return _context.Clientes.FirstOrDefault(c => c.Id == id);
        }

        public void AgregarCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }

        public void EliminarCliente(int id)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                _context.SaveChanges();
            }
        }

        public void ModificarCliente(Cliente cliente)
        {
            _context.Entry(cliente).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
