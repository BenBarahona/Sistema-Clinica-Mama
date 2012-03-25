using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DT
{
    public static class Generador
    {
        public static String[] PERMISOS = { "Registrar Persona", "Control de Pago", "Capitalizacion", "Consultas", "Parametrizacion", "Seguridad" };

        public static float getTotalInteres(List<String> aportaciones)
        {
            float total = 0.0f;
            foreach (String aportacion in aportaciones)
            {
                String[] tupla = aportacion.Split(',');
                total += ((float)Convert.ToDouble(tupla[8]));
            }
            return total;
        }

        public static float getTotalAportaciones(List<String> todas)
        {
            float result = 0.0f;
            foreach (String aportacion in todas)
                result += (float)Convert.ToDouble(aportacion.Split(',')[1]);
            return result;
        }

        public static List<String> filtrarAportaciones(String[] todas, String num_certificado)
        {
            List<String> resultado = new List<String>();
            foreach (String tupla in todas)
                if (tupla.Split(',')[0] == num_certificado)
                    resultado.Add(tupla);
            return resultado;
        }

        public static List<String> filtrarAportaciones(List<String> todas, String num_certificado)
        {
            List<String> resultado = new List<String>();
            foreach (String tupla in todas)
                if (tupla.Split(',')[0] == num_certificado)
                    resultado.Add(tupla);
            return resultado;
        }

        public static List<String> filtrarAportacionesMorosas(String[] todas)
        {
            List<String> morosas = new List<String>();
            foreach (String aportacion in todas)
            {
                if (DataTransaction.EsMorosa(aportacion.Split(',')[1]))
                    morosas.Add(aportacion);
            }
            return morosas;
        }

        public static String UpdateParametrizable(int tabla, String valor_viejo, String valor_nuevo , String responsable)
        {
            String Query = " UPDATE ";
            switch (tabla)
            {
                case 0:/*Ocupacion*/
                    Query += " Ocupacion SET Ocupacion.Ocupacion = '" + valor_nuevo +
                        "', Fecha_Modificacion = GETDATE() , Usuario_Responsable = '" + responsable + 
                        "' WHERE Ocupacion.Ocupacion = '" + valor_viejo + "'";
                    break;
                case 1:/*Parentesco*/
                    Query += " Parentesco SET Parentesco.Parentesco = '" + valor_nuevo +
                        "', Fecha_Modificacion = GETDATE() , Usuario_Responsable = '" + responsable +
                        "' WHERE Parentesco.Parentesco = '" + valor_viejo + "'";
                    break;
                case 2:/*Motivo*/
                    Query += " Motivo SET Motivo.Motivo = '" + valor_nuevo +
                        "', Fecha_Modificacion = GETDATE() , Usuario_Responsable = '" + responsable +
                        "' WHERE Motivo.Motivo = '" + valor_viejo + "'";
                    break;
                default: return "";
            }
            return Query;

        }

        public static String INSERT( String tabla , String[] valores )
        {
            return "";
        }

        public static String InsertarAportacionVoluntaria(String AportacionV_DK)
        {
            String tempDK = AportacionV_DK;
            String responsable = AportacionV_DK.Split('&')[1];
            String[] datos = AportacionV_DK.Split('&')[0].Split(',');
            String id = DataTransaction.getIDAfiliado(datos[0]);
            int limite_actual = Convert.ToInt32(DataTransaction.getLimiteActual());
            String id_limite = DataTransaction.getIDLimitectual();
            String QueryString_Temp = " INSERT INTO Aportacion VALUES( " + id + ", 'TRUE' ," + datos[1] + ",'" + datos[2] +
                "' , GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "' , GETDATE() ) ";

            if (Convert.ToInt32(datos[1]) <= limite_actual)
            {
                QueryString_Temp += " INSERT INTO Aportacion_Voluntaria VALUES( (SELECT MAX(Aportacion.ID_Aportacion) FROM "
                           + "Aportacion ) , 'TRUE' , " + id_limite + " , GETDATE() , GETDATE() , '" + responsable + "' , '" +
                           responsable + "' ) UPDATE Cuenta SET Saldo = (Cuenta.Saldo + " + datos[1] + "), " +
                           " Fecha_Modificacion = GETDATE(), Usuario_Responsable = '" + responsable + 
                           "' WHERE Cuenta.Num_Certificado =  (SELECT Afiliado.Num_Certificado FROM Afiliado,Persona " +
                           "WHERe Afiliado.ID_Persona = Persona.ID_Persona AND Persona.No_Identidad = '" + datos[0] + "') ";
            }
            else
            {
                QueryString_Temp += " INSERT INTO Aportacion_Voluntaria VALUES( (SELECT MAX(Aportacion.ID_Aportacion) FROM "
                           + "Aportacion ) , 'FALSE' , " + id_limite + " , GETDATE() , GETDATE() , '" + responsable + "' , '" +
                           responsable + "' ) ";
            }
            QueryString_Temp += Log.Insercion(tempDK, "Aportacion_Voluntaria");
            return QueryString_Temp;
        }

        public static String PermisosToString(bool[,] permisos)
        {
            String result = "";
            for (int i = 0; i < permisos.GetLength(0); i++)
            {
                for (int j = 0; j < permisos.GetLength(1); j++ )
                    result += permisos[i, j].ToString() + ",";
                result = result.Substring(0,result.Length-1) + ";";
            }
            return result.Substring(0,result.Length-1);
        }

        public static int getTipoDK(String str)
        {
            string[] datos = str.Split(';');
            if (datos.Length == 4)
                return 2;
            if (datos.Length >= 6)
                return 1;
            return 0;
        }

        public static String SumarACuenta(String numCertificado, float valor, String responsable)
        {
            return " UPDATE Cuenta SET Saldo = ( Saldo + " + valor.ToString() +
                "), Fecha_Modificacion = GETDATE(), Usuario_Responsable = '" + responsable + "'WHERE " +
                "Cuenta.Num_Certificado = " + numCertificado;
        }

        public static String getQueryPermisoRol(String rol, String[] permisos, String responsable)
        {
            String query = "";
            for (int i = 0; i < permisos.Length; i++)
                if (Convert.ToBoolean(permisos[i]))
                    query += getQueryPermiso(rol, Generador.PERMISOS[i], responsable);
            return query;
        }

        private static String getQueryPermiso(String rol, String permiso, String responsable)
        {
            return " INSERT INTO Tiene VALUES( (SELECT ID_Permiso FROM Permiso WHERE Permiso.Nombre = '" +
                permiso + "') , (SELECT Rol.ID_Rol  FROM Rol  WHERE Rol.Nombre = '" + rol + "' ) , GETDATE() ," +
                " GETDATE() , '" + responsable + "' , '" + responsable + "') ";
        }

        public static String getTuplaTodasLasAportaciones(String[] datos,Interes interes)
        {
            String sdatos = "";
            foreach (String dato in datos)
                sdatos += dato + ',';
            if (Convert.ToInt32(datos[2]) == 0)
                return "";
            return  sdatos + interes.nombre + ',' + interes.porcentaje + ',' 
                + interes.Ganancia(Convert.ToInt32(datos[2])) + ';';
        }

        public static String AumentarAportacion(String monto,String id_aportacion)
        {
            return " UPDATE Aportacion SET Monto = Monto + " + monto + " WHERE Aportacion.ID_Aportacion = " +
                id_aportacion + " ";
        }

        public static Boolean[,] AsignarPermiso(int i, String Permiso, Boolean[,] permisos)
        {
            for (int j = 0; j < Generador.PERMISOS.Length; j++)
                if (Permiso == Generador.PERMISOS[j])
                    permisos[i, j] = true;
            return permisos;
        }
    }
}
