using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace CapaLogica
{
    public class VentaLogica
    {
        private readonly EntidadesContainer _context;

        public VentaLogica()
        {
            _context = new EntidadesContainer();
        }

        public Venta ObtenerVentaPorId(int id)
        {
            return _context.Ventas.FirstOrDefault(v => v.Id == id);
        }
        public List<object> ConsultarVentas(int idVenta)
        {
            var ventas = (from detalle in _context.DetalleVentas
                          join venta in _context.Ventas on detalle.VentaId equals venta.Id
                          join producto in _context.Productos on detalle.ProductoId equals producto.Id
                          join cliente in _context.Clientes on venta.ClienteId equals cliente.Id
                          where venta.Id == idVenta  
                          select new
                          {
                              VentaId = venta.Id,
                              Producto = producto.nombre,
                              Descripcion = producto.descripcion,
                              Precio = producto.precio,
                              Cantidad = detalle.cantidad,
                              Subtotal = detalle.subtotal,
                              Cliente = cliente.nombre,
                              ProductoId = producto.Id
                          }).ToList();

            return ventas.Cast<object>().ToList(); 
        }
        public bool GuardarVentaConDetalles(Venta venta, List<DetalleVenta> detalles)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    
                    _context.Ventas.Add(venta);
                    _context.SaveChanges();

                    
                    foreach (var detalle in detalles)
                    {
                        detalle.VentaId = venta.Id;
                        _context.DetalleVentas.Add(detalle);

                        
                        var producto = _context.Productos.FirstOrDefault(p => p.Id == detalle.ProductoId);
                        if (producto != null)
                        {
                            producto.stock -= detalle.cantidad; 
                            if (producto.stock < 0)
                            {
                                throw new Exception($"El stock del producto '{producto.nombre}' no puede ser negativo.");
                            }
                        }
                        else
                        {
                            throw new Exception($"No se encontró el producto con ID {detalle.ProductoId}.");
                        }
                    }

                    
                    _context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void AgregarVenta(Venta venta)
        {
            _context.Ventas.Add(venta);
            _context.SaveChanges();
        }

        public bool EliminarDetalleVenta(int ventaId, int productoId, int cantidad)
        {
            try
            {
                using (var context = new EntidadesContainer())
                {
                    
                    var detalleEliminar = context.DetalleVentas
                        .FirstOrDefault(d => d.VentaId == ventaId && d.ProductoId == productoId);

                    if (detalleEliminar == null)
                    {
                        throw new Exception("No se encontró el detalle de venta en la base de datos.");
                    }

                    var producto = context.Productos.FirstOrDefault(p => p.Id == productoId);
                    if (producto != null)
                    {
                        producto.stock += cantidad;
                    }

                    context.DetalleVentas.Remove(detalleEliminar);

                 
                    context.SaveChanges();

                    return true; 
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el detalle de venta: {ex.Message}");
            }
        }

        public void ModificarVentaDetalles(List<dynamic> detalles)
        {
            try
            {
               
                foreach (var detalle in detalles)
                {
                    int productoId = detalle.ProductoId;
                    int ventaId = detalle.VentaId;
                    int cantidad = detalle.Cantidad;
                    decimal precio = detalle.Precio;

                    var detalleVenta = _context.DetalleVentas
                                               .FirstOrDefault(d => d.ProductoId == productoId && d.VentaId == ventaId);

                    if (detalleVenta != null)
                    {
                        
                        detalleVenta.cantidad = cantidad;
                        detalleVenta.subtotal = precio * cantidad;

                       
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar los detalles de la venta: " + ex.Message);
            }
        }
        public IQueryable<DetalleVenta> ObtenerDetallesPorVenta(int ventaId)
        {
            return _context.DetalleVentas.Where(dv => dv.VentaId == ventaId);
        }
        public List<Venta> ObtenerTodasLasVentas()
        {
            try
            {
                using (var context = new EntidadesContainer())  
                {
                    return context.Ventas.ToList(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener las ventas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Venta>(); 
            }
        }
        public List<object> ObtenerDetalleVentas()
        {
            var ventas = (from detalle in _context.DetalleVentas
                          join venta in _context.Ventas on detalle.VentaId equals venta.Id
                          join producto in _context.Productos on detalle.ProductoId equals producto.Id
                          join cliente in _context.Clientes on venta.ClienteId equals cliente.Id
                          select new
                          {
                              VentaId = venta.Id,
                              Producto = producto.nombre,
                              Descripcion = producto.descripcion,
                              Precio = producto.precio,
                              Cantidad = detalle.cantidad,
                              Subtotal = detalle.subtotal,
                              Cliente = cliente.nombre,
                              ProductoId = producto.Id
                          }).ToList();

            return ventas.Cast<object>().ToList(); 
        }
        public bool ActualizarTotalVenta(int ventaId, decimal nuevoTotal)
        {
            try
            {
                using (var context = new EntidadesContainer()) 
                {
                    var venta = context.Ventas.FirstOrDefault(v => v.Id == ventaId);
                    if (venta != null)
                    {
                        venta.total = nuevoTotal; 
                        context.SaveChanges(); 
                        return true;
                    }
                    return false; 
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error al actualizar el total de la venta: {ex.Message}");
                return false;
            }
        }
        public bool EliminarVenta(int ventaId)
        {
            try
            {
                using (var context = new EntidadesContainer())
                {
                    var venta = context.Ventas.SingleOrDefault(v => v.Id == ventaId);

                    if (venta != null)
                    {
                        context.Ventas.Remove(venta);
                        context.SaveChanges();
                        return true;
                    }
                    return false; 
                }
            }
            catch (Exception)
            {
                return false; 
            }
        }
        public (Venta venta, List<DetalleVenta> detalles, Cliente cliente) ConsultarVentaConDetalles(int idVenta)
        {
            try
            {
                var venta = _context.Ventas
                    .FirstOrDefault(v => v.Id == idVenta);

                if (venta == null)
                {
                    return (null, null, null); 
                }

                var detalles = (from detalle in _context.DetalleVentas
                                join producto in _context.Productos on detalle.ProductoId equals producto.Id
                                where detalle.VentaId == idVenta
                                select detalle).ToList();

  
                var cliente = _context.Clientes
                    .FirstOrDefault(c => c.Id == venta.ClienteId);

                return (venta, detalles, cliente);
            }
            catch (Exception ex)
            {
  
                throw new Exception("Error al consultar la venta: " + ex.Message);
            }
        }
        
        public List<object> ObtenerDetalleDeVenta(int ventaId)
        {
            var detalles = (from d in _context.DetalleVentas
                            join p in _context.Productos on d.ProductoId equals p.Id
                            join v in _context.Ventas on d.VentaId equals v.Id
                            join c in _context.Clientes on v.ClienteId equals c.Id
                            where d.VentaId == ventaId
                            select new
                            {
                                ventaId = d.VentaId,
                                ProductoId = d.ProductoId,
                                Producto = p.nombre,
                                Descripcion = p.descripcion,
                                Precio = p.precio,
                                Cantidad = d.cantidad,
                                Subtotal = d.subtotal,
                                Cliente = c.nombre,       
                            }).ToList();

            return detalles.Cast<object>().ToList(); 
        }

    }
}
