using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiReg
{
    class Cliente
    {
        
        public Cliente(string nombre, string cedula, string desc, string monto, string archivo, string correo)
        {
            this.nombre = nombre;
            this.cedula = cedula;
            this.desc = desc;
            this.monto = monto;
            this.archivo = archivo;
            this.correo = correo;
        }

        public Cliente()
        {

        }

        public string nombre { get; set; }
        public string cedula { get; set; }
        public string desc { get; set; }
        public string monto { get; set; }
        public string archivo { get; set; }
        public string correo { get; set; }


    }
}
