using System;
using System.Collections.Generic;

namespace Modelos
{
    public class Usuario
    {
        string _nombreusuario = string.Empty;
        string _clave = string.Empty;
        int _usuarioId;
        int _idEmpleado;
        int _rolId;
        int _estadoId;
        string _nombreRol = string.Empty;
        string _nombreEmpleado = string.Empty;
        string _email = string.Empty;
        string _estado = string.Empty;

        public int UsuarioId { get => _usuarioId; set => _usuarioId = value; }
        public string NombreUsuario { get => _nombreusuario; set => _nombreusuario = value; }
        public int IdEmpleado { get => _idEmpleado; set => _idEmpleado = value; }
        public string Clave { get => _clave; set => _clave = value; }
        public int RolID { get => _rolId; set => _rolId = value; }
        public int EstadoID { get => _estadoId; set => _estadoId = value; }
        public string NombreRol { get => _nombreRol; set => _nombreRol = value; }
        public string NombreEmpleado { get => _nombreEmpleado; set => _nombreEmpleado = value; }
        public string Email { get => _email; set => _email = value; }
        public string Estado { get => _estado; set => _estado = value; }
        public HashSet<string> PermisosModulos { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        public bool EsAdministrador
        {
            get
            {
                string rol = NombreRol?.Trim() ?? string.Empty;
                return string.Equals(rol, "Administrador", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(rol, "Admin", StringComparison.OrdinalIgnoreCase);
            }
        }

        public bool TienePermiso(string codigoModulo)
        {
            if (string.IsNullOrWhiteSpace(codigoModulo))
            {
                return false;
            }

            return EsAdministrador || PermisosModulos.Contains(codigoModulo);
        }
    }
}
