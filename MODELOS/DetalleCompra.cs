using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODELOS
{
    public class DetalleCompra
    {
        private int     detalleCompraID;
        private int     compraID;
        private int     productoID;
        private string  nombreProducto       = string.Empty;
        private int     cantidad;
        private decimal precioUnitarioCompra;
        private decimal precioTotal;

        public int     DetalleCompraID       { get => detalleCompraID;       set => detalleCompraID = value; }
        public int     CompraID              { get => compraID;              set => compraID = value; }
        public int     ProductoID            { get => productoID;            set => productoID = value; }
        public string  NombreProducto        { get => nombreProducto;        set => nombreProducto = value; }
        public int     Cantidad              { get => cantidad;              set => cantidad = value; }
        public decimal PrecioUnitarioCompra  { get => precioUnitarioCompra;  set => precioUnitarioCompra = value; }
        public decimal PrecioTotal           { get => precioTotal;           set => precioTotal = value; }
        // Calculado local — igual al PrecioTotal que guarda el SP
        public decimal Subtotal => cantidad * precioUnitarioCompra;
    }
}
