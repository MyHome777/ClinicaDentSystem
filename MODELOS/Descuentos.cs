namespace MODELOS
{
    public class Descuentos
    {
        public int DescuentoID { get; set; }
        public int? ServicioID { get; set; }
        public string NombreServicio { get; set; } = string.Empty;
        public int? ProductoID { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public DateTime FechaIn { get; set; }
        public DateTime FechaFn { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
