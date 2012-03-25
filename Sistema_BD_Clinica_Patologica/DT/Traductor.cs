using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DT
{
    class Traductor
    {
        public static String traductorNombres(String datos)
        {
            datos = datos.Trim();
            if (!datos.Equals(""))
            {
                String[] datosPartidos = datos.Split(' ');
                String resultado = "";
                for (int i = 0; i < datosPartidos.Length; i++)
                {
                    if (!datosPartidos[i].Equals(""))
                        resultado += datosPartidos[i].Substring(0, 1).ToUpper() +
                            datosPartidos[i].Substring(1, datosPartidos[i].Length - 1).ToLower() + " ";
                }

                resultado = resultado.Trim();
                return resultado;
            }
            else
                return datos;
        }
    }
}
