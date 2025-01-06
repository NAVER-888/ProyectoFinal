using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;

namespace CapaLogica
{
    public class DetalleVentaLogica
    {
        private readonly EntidadesContainer _context;

        public DetalleVentaLogica()
        {
            _context = new EntidadesContainer();
        }

        

        public void AgregarDetalleVenta(DetalleVenta detalle)
        {
            _context.DetalleVentas.Add(detalle);
            _context.SaveChanges();
        }

        public void EliminarDetalleVenta(int detalleId)
        {
            var detalle = _context.DetalleVentas.FirstOrDefault(dv => dv.Id == detalleId);
            if (detalle != null)
            {
                _context.DetalleVentas.Remove(detalle);
                _context.SaveChanges();
            }
        }
    }
}
