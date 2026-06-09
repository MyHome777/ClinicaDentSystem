using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class Usuario
    {
        int _usuarioId;
        string _nombreusuario = string.Empty;
        string _clave = string.Empty;
        string _nombreRol = string.Empty;

        public int UsuarioId { get => _usuarioId; set => _usuarioId = value; }
        public string NombreUsuario { get => _nombreusuario; set => _nombreusuario = value; }
        public int IdEmpleado { get => _usuarioId; set => _usuarioId = value; }
        public string Clave { get => _clave; set => _clave = value; }
        public string NombreRol { get => _nombreRol; set => _nombreRol = value; }
    }
}
