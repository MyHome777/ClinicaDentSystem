namespace Modelos
{
    public class PermisoModulo
    {
        public int ModuloID { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string NombreModulo { get; set; } = string.Empty;
        public bool Permitido { get; set; }

        public override string ToString()
        {
            return NombreModulo;
        }
    }
}
