namespace Modelos
{
    public class ConfiguracionCorreo
    {
        public int ConfiguracionCorreoID { get; set; }
        public string CorreoRemitente { get; set; } = string.Empty;
        public string NombreRemitente { get; set; } = string.Empty;
        public string ServidorSmtp { get; set; } = string.Empty;
        public int Puerto { get; set; } = 587;
        public bool UsaSsl { get; set; } = true;
        public string ClaveCorreo { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;

        public bool EstaCompleta =>
            !string.IsNullOrWhiteSpace(CorreoRemitente)
            && !string.IsNullOrWhiteSpace(ServidorSmtp)
            && Puerto > 0
            && !string.IsNullOrWhiteSpace(ClaveCorreo)
            && Activo;
    }
}
