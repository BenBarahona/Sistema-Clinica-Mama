using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DT
{
    public class Fecha
    {
        public int Anio, Mes, Dia, Hora, Minuto, Segundo;
        String AM;

        public Fecha(String fecha)
        {
            this.Anio = Convert.ToInt32(fecha.Split('/')[2].Split(' ')[0]);
            this.Mes = Convert.ToInt32(fecha.Split('/')[0]);
            this.Dia = Convert.ToInt32(fecha.Split('/')[1]);
            this.Hora = Convert.ToInt32((fecha.Split(' ')[1]).Split(':')[0]);
            this.Minuto = Convert.ToInt32((fecha.Split(' ')[1]).Split(':')[1]);
            this.Segundo = Convert.ToInt32((fecha.Split(' ')[1]).Split(':')[2]);
            this.AM = fecha.Split(' ')[2];
        }
    }
}
