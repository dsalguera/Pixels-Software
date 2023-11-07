using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixels_Software
{
    class Papeleria
    {
        public Papeleria(String nombre, double basex, double altura, double precio, String medida)
        {
            this.nombre = nombre;
            this.basex = basex;
            this.altura = altura;
            this.precio = precio;
            this.medida = medida;
        }

        public Papeleria()
        {

        }

        public string nombre { get; set; }

        public double basex { get; set; }

        public double altura { get; set; }

        public double precio { get; set; }

        public string medida { get; set; }


    }
}
