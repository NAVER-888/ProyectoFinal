using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using System.Data.Entity;

namespace CapaLogica
{
    public class ConsultaLogica
    {
        private readonly EntidadesContainer _context;

        public ConsultaLogica(EntidadesContainer context)
        {
            _context = context;
        }

        public List<object> ConsultarVentasPorId(int idVenta)
        {
            var ventas = (from detalle in _context.DetalleVentas
                          join venta in _context.Ventas on detalle.VentaId equals venta.Id
                          join producto in _context.Productos on detalle.ProductoId equals producto.Id
                          join cliente in _context.Clientes on venta.ClienteId equals cliente.Id
                          where venta.Id == idVenta
                          select new
                          {
                              VentaId = venta.Id,
                              Fecha = venta.fecha,
                              Cliente = cliente.nombre,
                              Producto = producto.nombre,
                              Descripcion = producto.descripcion,
                              Precio = producto.precio,
                              Cantidad = detalle.cantidad,
                              Subtotal = detalle.subtotal,
                              Total = venta.total
                          }).ToList();

            return ventas.Cast<object>().ToList(); 
        }
        public List<object> ConsultarVentasPorFecha(DateTime fecha)
        {
            var ventas = (from detalle in _context.DetalleVentas
                          join venta in _context.Ventas on detalle.VentaId equals venta.Id
                          join producto in _context.Productos on detalle.ProductoId equals producto.Id
                          join cliente in _context.Clientes on venta.ClienteId equals cliente.Id
                          where DbFunctions.TruncateTime(venta.fecha) == DbFunctions.TruncateTime(fecha)
                          select new
                          {
                              VentaId = venta.Id,
                              Fecha = venta.fecha,
                              Cliente = cliente.nombre,
                              Producto = producto.nombre,
                              Descripcion = producto.descripcion,
                              Precio = producto.precio,
                              Cantidad = detalle.cantidad,
                              Subtotal = detalle.subtotal,
                              Total = venta.total
                          }).ToList();

            return ventas.Cast<object>().ToList(); 
        }
    }
}
