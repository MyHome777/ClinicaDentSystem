using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODELOS
{
    public class MovimientoStock
    {
        private int      movimientoID;
        private int      productoID;
        private string   nombreProducto  = string.Empty;
        private string   tipoMovimiento  = string.Empty; // "ENTRADA" o "SALIDA"
        private int      cantidad;
        private string   descripcion     = string.Empty;
        private DateTime fechaMovimiento;
        private int      usuarioID;
        private string   nombreUsuario   = string.Empty;

        public int      MovimientoID    { get => movimientoID;    set => movimientoID = value; }
        public int      ProductoID      { get => productoID;      set => productoID = value; }
        public string   NombreProducto  { get => nombreProducto;  set => nombreProducto = value; }
        public string   TipoMovimiento  { get => tipoMovimiento;  set => tipoMovimiento = value; }
        public int      Cantidad        { get => cantidad;        set => cantidad = value; }
        public string   Descripcion     { get => descripcion;     set => descripcion = value; }
        public DateTime FechaMovimiento { get => fechaMovimiento; set => fechaMovimiento = value; }
        public int      UsuarioID       { get => usuarioID;       set => usuarioID = value; }
        public string   NombreUsuario   { get => nombreUsuario;   set => nombreUsuario = value; }
    }
}
