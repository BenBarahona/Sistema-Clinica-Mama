using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DT
{
    public static class Log
    {

        /*Esta es una clase que se encarga de llevar un log de todo lo q pasa en la base de datos*/
        /*Log 0.1 Hecho por Cortez*/
        public static String Modificacion(String string_DK, String tabla, String string_DKAnterior)
        {
            String QueryString = " INSERT INTO LOG VALUES ('";
            String[] datos = string_DK.Split('&');
            QueryString += datos[1] + "', GetDate(), 'MODIFICACION', '" + tabla + "', '" + datos[0] + "', '" + string_DKAnterior + "') ";
            return QueryString;
        }

        public static String Capitalizacion(String usuario)
        {
            String QueryString = " INSERT INTO LOG VALUES ('" + usuario
            + "', GetDate(), 'CAPITALIZACION', 'NULL', 'NULL', 'NULL') ";
            return QueryString;
        }

        public static String Insercion(String string_DK, String tabla)
        {
            String QueryString = " INSERT INTO LOG VALUES ('";
            String[] datos = string_DK.Split('&');
            QueryString += datos[1] + "', GetDate(), 'INSERCION', '" + tabla + "', '" + datos[0] + "', 'NULL') ";
            return QueryString;
        }
    }
}
