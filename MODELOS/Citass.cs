using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODELOS
{
    public class Citass
    {
        private int numerodecita;
        private string paciente = string.Empty;
        private string dentista = string.Empty;
        private DateTime fechayhora;
        private string motivo = string.Empty;
        private string estadodelacita = string.Empty;
        private string notasdelacita = string.Empty;
        private DateTime fecha;
        private int pacienteId;
        private int dentistaID;
        private int estadoID;

        public int Numerodecita { get => numerodecita; set => numerodecita = value; }
        public string Paciente { get => paciente; set => paciente = value; }
        public string Dentista { get => dentista; set => dentista = value; }
        public DateTime Fechayhora { get => fechayhora; set => fechayhora = value; }
        public string Motivo { get => motivo; set => motivo = value; }
        public string Estadodelacita1 { get => estadodelacita; set => estadodelacita = value; }
        public string Notasdelacita { get => notasdelacita; set => notasdelacita = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
        public int PacienteId { get => pacienteId; set => pacienteId = value; }
        public int DentistaID { get => dentistaID; set => dentistaID = value; }
        public int EstadoID { get => estadoID; set => estadoID = value; }
    }
}
