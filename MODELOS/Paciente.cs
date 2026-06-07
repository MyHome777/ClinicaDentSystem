using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODELOS
{
    public class Paciente
    {
        public int PacienteID { get; set; }
        public string NumDocumento { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string ContactoEmergencia { get; set; }
        public string TelefonoEmergencia { get; set; }
        public string Alergias { get; set; }
        public string NotasMedicas { get; set; }
        public int TipoDocID { get; set; }
        public int EstadoID { get; set; }
    }
}
