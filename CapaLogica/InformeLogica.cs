using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;

namespace CapaLogica
{
    public class InformeLogica
    {
        private readonly EntidadesContainer _context;

        public InformeLogica()
        {
            _context = new EntidadesContainer();
        }

        public IQueryable<Venta> ObtenerVentasPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            return _context.Ventas.Where(v => v.fecha >= fechaInicio && v.fecha <= fechaFin);
        }

        public IQueryable<Producto> ObtenerProductosConStockBajo(int stockMinimo)
        {
            return _context.Productos.Where(p => p.stock < stockMinimo);
        }
    }
}
