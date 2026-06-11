using System.Collections.Generic;

namespace Modelos
{
    public static class ModulosSistema
    {
        public const string Inicio = "INICIO";
        public const string Pacientes = "PACIENTES";
        public const string Citas = "CITAS";
        public const string Dentistas = "DENTISTAS";
        public const string Inventario = "INVENTARIO";
        public const string Facturacion = "FACTURACION";
        public const string Historial = "HISTORIAL";
        public const string Auditorias = "AUDITORIAS";
        public const string Usuarios = "USUARIOS";
        public const string Configuracion = "CONFIGURACION";

        public static IReadOnlyList<string> Todos { get; } = new[]
        {
            Inicio,
            Pacientes,
            Citas,
            Dentistas,
            Inventario,
            Facturacion,
            Historial,
            Auditorias,
            Usuarios,
            Configuracion
        };
    }
}
