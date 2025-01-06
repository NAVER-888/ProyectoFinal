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
        public List<Producto> ObtenerTodosLosProductos()
        {
            try
            {
                return _context.Productos.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los productos.", ex);
            }
        }

        public Producto BuscarProductoPorId(int id)
        {
            try
            {
                return _context.Productos.FirstOrDefault(p => p.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el producto por ID.", ex);
            }
        }

        public bool CrearProducto(Producto producto)
        {
            try
            {
                if (_context.Productos.Any(p => p.nombre == producto.nombre))
                {
                    return false; 
                }

                _context.Productos.Add(producto);
                _context.SaveChanges();
                return true; 
            }
            catch (Exception)
            {
                return false; 
            }
        }
        public bool ActualizarProducto(Producto producto)
        {
            try
            {
                var productoExistente = _context.Productos.FirstOrDefault(p => p.Id == producto.Id);
                if (productoExistente == null)
                {
                    return false; 
                }

                productoExistente.nombre = producto.nombre;
                productoExistente.descripcion = producto.descripcion;
                productoExistente.precio = producto.precio;
                productoExistente.stock = producto.stock;

                _context.SaveChanges();
                return true; 
            }
            catch (Exception)
            {
                return false; 
            }
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
