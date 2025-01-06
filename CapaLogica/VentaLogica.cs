using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
                    }

                    _context.SaveChanges();

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public void AgregarVenta(Venta venta)
        {
            _context.Ventas.Add(venta);
            _context.SaveChanges();
        }

        public bool EliminarVenta(int ventaId)
        {
            try
            {
                using (var context = new EntidadesContainer()) 
                {
                    var venta = context.Ventas.FirstOrDefault(v => v.Id == ventaId);
                    if (venta != null)
                    {
                        context.Ventas.Remove(venta); 
                        context.SaveChanges(); 
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar la venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void ModificarVenta(Venta venta)
        {
            _context.Entry(venta).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
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

    }
}
