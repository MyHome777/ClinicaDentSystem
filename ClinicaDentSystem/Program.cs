using Modelos;

namespace ClinicaDentSystem
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {

            ApplicationConfiguration.Initialize();
            Application.Run(new Login());
            
    }
        public static Usuario UsuarioActivo { get; set; }
    }
}
