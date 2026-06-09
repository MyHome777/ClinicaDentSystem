using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODELOS
{
    public class FacturaDetalle
    {
        public string Tipo { get; set; } = string.Empty;
        public int ItemID { get; set; }
        public int? ServicioID { get; set; }
        public int? ProductoID { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioAplicado { get; set; }
        public int? DescuentoID { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public decimal Subtotal => Cantidad * PrecioAplicado;
        public decimal Descuento => Subtotal * PorcentajeDescuento / 100m;
        public decimal Total => Subtotal - Descuento;
    }
}
