using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODELOS
{
    public class Servicios
    {
        public int ServicioID { get; set; }
        public string NombreServicio { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int EstadoID { get; set; }
    }
}
