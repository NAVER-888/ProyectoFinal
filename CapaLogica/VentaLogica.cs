using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;

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

        public void AgregarVenta(Venta venta)
        {
            _context.Ventas.Add(venta);
            _context.SaveChanges();
        }

        public void EliminarVenta(int id)
        {
            var venta = _context.Ventas.FirstOrDefault(v => v.Id == id);
            if (venta != null)
            {
                _context.Ventas.Remove(venta);
                _context.SaveChanges();
            }
        }

        public void ModificarVenta(Venta venta)
        {
            _context.Entry(venta).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
