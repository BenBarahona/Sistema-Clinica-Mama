using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DT
{
    public class NombreCompleto
    {
        String primerNombre;
        public String PrimerNombre
        {
            get { return primerNombre; }
            set { primerNombre = value; }
        }

        String segundoNombre;
        public String SegundoNombre
        {
            get { return segundoNombre; }
            set { segundoNombre = value; }
        }

        String primerApellido;
        public String PrimerApellido
        {
            get { return primerApellido; }
            set { primerApellido = value; }
        }

        String segundoApellido;
        public String SegundoApellido
        {
            get { return segundoApellido; }
            set { segundoApellido = value; }
        }
    }
}
