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

    }
}
