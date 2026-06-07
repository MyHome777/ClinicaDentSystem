using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODELOS
{
    public class Dentista
    {
        public int DentistaID { get; set; } // PK
        public int TipoDocID { get; set; }
        public string NumDocumento { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string LicenciaMedica { get; set; }
        public int EstadoID { get; set; }
    }
}
