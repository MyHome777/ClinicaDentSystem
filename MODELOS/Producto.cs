using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODELOS
{
    public class Producto
    {
        private int      productoID;
        private int      categoriaID;
        private int      compraID;
        private int      ventaID;
        private string   categoria       = string.Empty;
        private string   nombreProducto   = string.Empty;
        private string   descripcion      = string.Empty;
        private string   unidadMedida     = string.Empty;
        private int      stockActual;
        private int      stockMinimo;
        private decimal  precioUnitario;
        private DateTime fechaVencimiento;
        private int      estadoID;
        private string   estado           = string.Empty;

        public int      ProductoID        { get => productoID;       set => productoID = value; }
        public int      CategoriaID       { get => categoriaID;      set => categoriaID = value; }
        public int      CompraID          { get => compraID;         set => compraID = value; }
        public int      VentaID           { get => ventaID;          set => ventaID = value; }
        public string   Categoria         { get => categoria;        set => categoria = value; }
        public string   NombreProducto    { get => nombreProducto;   set => nombreProducto = value; }
        public string   Descripcion       { get => descripcion;      set => descripcion = value; }
        public string   UnidadMedida      { get => unidadMedida;     set => unidadMedida = value; }
        public int      StockActual       { get => stockActual;      set => stockActual = value; }
        public int      StockMinimo       { get => stockMinimo;      set => stockMinimo = value; }
        public decimal  PrecioUnitario    { get => precioUnitario;   set => precioUnitario = value; }
        public DateTime FechaVencimiento  { get => fechaVencimiento; set => fechaVencimiento = value; }
        public int      EstadoID          { get => estadoID;         set => estadoID = value; }
        public string   Estado            { get => estado;           set => estado = value; }
    }
}
