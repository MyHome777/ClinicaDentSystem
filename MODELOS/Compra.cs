using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODELOS
{
    public class Compra
    {
        private int compraID;
        private int proveedorID;
        private string nombreProveedor = string.Empty;
        private int usuarioID;
        private string nombreUsuario   = string.Empty;
        private DateTime fechaCompra;
        private decimal totalCompra;

        public int      CompraID        { get => compraID;        set => compraID = value; }
        public int      ProveedorID     { get => proveedorID;     set => proveedorID = value; }
        public string   NombreProveedor { get => nombreProveedor; set => nombreProveedor = value; }
        public int      UsuarioID       { get => usuarioID;       set => usuarioID = value; }
        public string   NombreUsuario   { get => nombreUsuario;   set => nombreUsuario = value; }
        public DateTime FechaCompra     { get => fechaCompra;     set => fechaCompra = value; }
        public decimal  TotalCompra     { get => totalCompra;     set => totalCompra = value; }
    }
}
