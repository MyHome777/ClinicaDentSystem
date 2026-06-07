using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODELOS
{
    public class Categorias
    {

        private int categoriaID;
        private string nombre = string.Empty;
        private string descripcion = string.Empty;
        private int estadoId;
        private string estado= string.Empty;
        public int CategoriaID { get => categoriaID; set => categoriaID = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public int EstadoId { get => estadoId; set => estadoId = value; }
        public string Estado { get => estado; set => estado = value; }
    }
}
