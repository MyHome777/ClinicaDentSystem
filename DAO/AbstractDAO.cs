using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public abstract class AbstractDAO<T>
    {
        public abstract List<T> ObtenerTodos(out string pError);

        public abstract T ObtenerPorId(int id, out string pError);

        public abstract T ObtenerPorId(string id, out string pError);

        public abstract void GuardarRegistro(T reg, out string pError);

        public abstract void ActualizarRegistro(T reg, out string pError);

    }
}
