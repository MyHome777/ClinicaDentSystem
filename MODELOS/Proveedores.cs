using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class Proveedores
    {
        private int proveedorID;
        private string nombreProveedor = string.Empty;
        private string contacto = string.Empty;
        private string telefono = string.Empty;
        private string email = string.Empty;
        private int estadoId;
        private string estado = string.Empty;

        public int ProveedorID { get => proveedorID; set => proveedorID = value; }
        public string NombreProveedor { get => nombreProveedor; set => nombreProveedor = value; }
        public string Contacto { get => contacto; set => contacto = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Email { get => email; set => email = value; }
        public int EstadoId { get => estadoId; set => estadoId = value; }
        public string Estado { get => estado; set => estado = value; }
    }