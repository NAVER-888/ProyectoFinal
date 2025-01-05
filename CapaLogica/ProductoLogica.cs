using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;

namespace CapaLogica
{
    public class ProductoLogica
    {
        private readonly EntidadesContainer _context;

        public ProductoLogica()
        {
            _context = new EntidadesContainer();
        }

        public Producto ObtenerProductoPorId(int id)
        {
            return _context.Productos.FirstOrDefault(p => p.Id == id);
        }

        public void AgregarProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            _context.SaveChanges();
        }

        public void EliminarProducto(int id)
        {
            var producto = _context.Productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                _context.SaveChanges();
            }
        }

        public void ModificarProducto(Producto producto)
        {
            _context.Entry(producto).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
