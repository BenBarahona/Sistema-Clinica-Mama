using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DT
{
    public class Interes
    {
        public String nombre;
        public Boolean Obligatoria,Obligatoria_Especial,Voluntaria,Aplica_VoluntariaArribaDe,Valido;
        public int VoluntariaArribaDe,porcentaje;

        public Interes(String InteresDK)
        {
            String[] datos = InteresDK.Split(',');
            this.nombre = datos[0];
            this.porcentaje = Convert.ToInt32(datos[1]);
            this.Obligatoria = Convert.ToBoolean(datos[2]);
            this.Voluntaria = Convert.ToBoolean(datos[3]);
            this.Obligatoria_Especial = Convert.ToBoolean(datos[4]);
            this.Aplica_VoluntariaArribaDe = Convert.ToBoolean(datos[5]);
            this.VoluntariaArribaDe = Convert.ToInt32(datos[6]);
            this.Valido = Convert.ToBoolean(datos[7]);
        }

        public float Ganancia(float Base)
        {
            return (float)(Base * ((float)this.porcentaje / 100));
        }

        public String Ganancia(String base_s)
        {
            float Base = (float)Convert.ToDouble(base_s);
            return Convert.ToString( Base * (float)(this.porcentaje / 100) );
        }
    }
}
