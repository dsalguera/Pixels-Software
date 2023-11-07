using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixels_Software
{
    class Clientes
    {

        public Clientes(string nombre, string correo, string telefono,
            string cedula, string direccion, string descripcion, string monto_total, string archivo)
        {
            this.nombre = nombre;
            this.correo = correo;
            this.telefono = telefono;
            this.cedula = cedula;
            this.direccion = direccion;
            this.descripcion = descripcion;
            this.monto_total = monto_total;
            this.archivo = archivo;

        }

        public Clientes()
        {

        }

        public string nombre { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string cedula { get; set; }
        public string direccion { get; set; }
        public string descripcion { get; set; }
        public string monto_total { get; set; }
        public string archivo { get; set; }

    }
}
