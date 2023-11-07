using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixels_Software
{
    class ListaEstructura
    {
        public ListaEstructura(string cantidad, string descripcion, string precio, string imagen)
        {
            this.cantidad = cantidad;
            this.descripcion = descripcion;
            this.precio = precio;
            this.imagen = imagen;
        }

        public ListaEstructura()
        {

        }

        public string cantidad { get; set; }
        public string descripcion { get; set; }
        public string precio { get; set; }
        public string imagen { get; set; }
    }
}
