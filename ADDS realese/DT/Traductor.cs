using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DT
{
    public static class traductor
    {
        public static String DK_ocupacion(String ocupacion_DK)
        {
            String traduccion = "";
            String[] datos = ocupacion_DK.Split('&');
            datos[0] = datos[0].Substring(0, 1).ToUpper() + datos[0].Substring(1, datos[0].Length - 1).ToLower();
            for (int i = 0; i < datos.Length; i++)
                traduccion += datos[i] + '&';
            return traduccion.Substring(0, traduccion.Length - 1);

        }



        public static String DK_Parentesco(String parentesco_DK)
        {
            String traduccion = "";
            String[] datos = parentesco_DK.Split('&');
            datos[0] = datos[0].Substring(0, 1).ToUpper() + datos[0].Substring(1, datos[0].Length - 1).ToLower();
            for (int i = 0; i < datos.Length; i++)
                traduccion += datos[i] + '&';
            return traduccion.Substring(0, traduccion.Length - 1);
        }

        public static String DK_Motivo(String motivo_DK)
        {
            String traduccion = "";
            String[] datos = motivo_DK.Split('&');
            datos[0] = datos[0].Substring(0, 1).ToUpper() + datos[0].Substring(1, datos[0].Length - 1).ToLower();
            for (int i = 0; i < datos.Length; i++)
                traduccion += datos[i] + '&';
            return traduccion.Substring(0, traduccion.Length - 1);
        }

        public static String DK_Afiliado(String afiliado_DK)
        {
            String traduccion = "";
            //Quito el van persie, porque esa parte no me sirve
            String[] separador = afiliado_DK.Split('&');
            //ahora lo separo en todos los tipos de datos que pueden haber, tengo mis datos separados en grupitos de comas...
            String[] datos = separador[0].Split(';');
            //todos los datos personales
            String[] datosPersonales = datos[0].Split(',');
            if (datosPersonales[1].Length > 0)
                datosPersonales[1] = datosPersonales[1].Substring(0, 1).ToUpper() + datosPersonales[1].Substring(1, datosPersonales[1].Length - 1).ToLower();
            if (datosPersonales[2].Length > 0)
                datosPersonales[2] = datosPersonales[2].Substring(0, 1).ToUpper() + datosPersonales[2].Substring(1, datosPersonales[2].Length - 1).ToLower();
            if (datosPersonales[3].Length > 0)
                datosPersonales[3] = datosPersonales[3].Substring(0, 1).ToUpper() + datosPersonales[3].Substring(1, datosPersonales[3].Length - 1).ToLower();
            if (datosPersonales[4].Length > 0)
                datosPersonales[4] = datosPersonales[4].Substring(0, 1).ToUpper() + datosPersonales[4].Substring(1, datosPersonales[4].Length - 1).ToLower();
            //Los datos personales se agragan al String con esto
            traduccion = datosPersonales[0] + "," + datosPersonales[1] + "," + datosPersonales[2] + "," + datosPersonales[3] + "," + datosPersonales[4] + "," + datosPersonales[5] + "," + datosPersonales[6] + "," + datosPersonales[7] + "," + datosPersonales[8] + ";" + datos[1] + ";" + datos[2] + ";" + datos[3] + ";" + datos[4] + ";" + datos[5] + ";";
            //datos[5] = profesion u oficio, hasta este punto tengo en el String los datos de profesion, vamos con los beneficiarios.
            //En los beneficiarios, los nombres siempre son desde las posiciones 1 hasta la 4, los beneficiarios van desde 6 hasta n...
            String[] datosBeneficiario;
            for (int n = 6; n < datos.Length; n++)
            {
                if (datos[n] == "")
                    break;
                datosBeneficiario = datos[n].Split(',');
                //Valido que la cadena no este vacia, si esta vacia me jodi
                if (datosBeneficiario[1].Length > 0)
                    datosBeneficiario[1] = datosBeneficiario[1].Substring(0, 1).ToUpper() + datosBeneficiario[1].Substring(1, datosBeneficiario[1].Length - 1).ToLower();
                if (datosBeneficiario[2].Length > 0)
                    datosBeneficiario[2] = datosBeneficiario[2].Substring(0, 1).ToUpper() + datosBeneficiario[2].Substring(1, datosBeneficiario[2].Length - 1).ToLower();
                if (datosBeneficiario[3].Length > 0)
                    datosBeneficiario[3] = datosBeneficiario[3].Substring(0, 1).ToUpper() + datosBeneficiario[3].Substring(1, datosBeneficiario[3].Length - 1).ToLower();
                if (datosBeneficiario[4].Length > 0)
                    datosBeneficiario[4] = datosBeneficiario[4].Substring(0, 1).ToUpper() + datosBeneficiario[4].Substring(1, datosBeneficiario[4].Length - 1).ToLower();
                //los datos del beneficiario estan listos, ahora los pego a mi String traduccion
                for (int i = 0; i < datosBeneficiario.Length; i++)
                    traduccion += datosBeneficiario[i] + ",";

                //Ahora debo quitar la ultima coma y ponerle un punto y coma...
                traduccion = traduccion.Substring(0, traduccion.Length - 1) + ";";

            }
            //Ahora elimino el ultimo punto y coma que no es necesario y agregamos el van persie
            traduccion = traduccion.Substring(0, traduccion.Length - 1) + "&" + separador[1];
            //Y listo

            return traduccion;

        }

        public static String DK_Empleado(String empleado_DK)
        {
            String traduccion = "";
            String[] datos = empleado_DK.Split(';');
            String[] datosPersonales = datos[0].Split(',');
            String telPersonal = datos[1];
            String celular = datos[2];
            String usuario = datos[3];
            //Otra vez, las validaciones para no joderme
            if (datosPersonales[1].Length > 0)
                datosPersonales[1] = datosPersonales[1].Substring(0, 1).ToUpper() + datosPersonales[1].Substring(1, datosPersonales[1].Length - 1).ToLower();
            if (datosPersonales[2].Length > 0)
                datosPersonales[2] = datosPersonales[2].Substring(0, 1).ToUpper() + datosPersonales[2].Substring(1, datosPersonales[2].Length - 1).ToLower();
            if (datosPersonales[3].Length > 0)
                datosPersonales[3] = datosPersonales[3].Substring(0, 1).ToUpper() + datosPersonales[3].Substring(1, datosPersonales[3].Length - 1).ToLower();
            if (datosPersonales[4].Length > 0)
                datosPersonales[4] = datosPersonales[4].Substring(0, 1).ToUpper() + datosPersonales[4].Substring(1, datosPersonales[4].Length - 1).ToLower();
            traduccion += datosPersonales[0] + "," + datosPersonales[1] + "," + datosPersonales[2] + "," + datosPersonales[3] + "," + datosPersonales[4]
            + "," + datosPersonales[5] + "," + datosPersonales[6] + "," + datosPersonales[7] + "," + datosPersonales[8] + ";" + datos[1] + ";" + datos[2] + ";" + datos[3];
            return traduccion;

        }

    }
}
