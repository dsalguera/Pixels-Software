using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixels_Software
{
    class Materiales
    {
        public Materiales(string nombre, double precio)
        {
            this.nombre = nombre;
            this.precio = precio;
        }

        public Materiales()
        {

        }

        public string nombre { get; set; }
        public double precio { get; set; }
    }
}
