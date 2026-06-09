using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODELOS
{
    public class FacturaEmision
    {
        public int UsuarioID { get; set; }
        public int CitaID { get; set; }
        public DateTime FechaEmision { get; set; }
        public int EstadoId { get; set; }
        public int? DescuentoID { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public List<FacturaDetalle> Detalles { get; set; } = new List<FacturaDetalle>();
    }
}
