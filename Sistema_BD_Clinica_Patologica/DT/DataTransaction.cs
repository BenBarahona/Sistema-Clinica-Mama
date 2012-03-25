using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;


namespace DT
{
    public class DataTransaction
    {
        /*Esta es la clase DT (data transaction), esta clase se encarga totalmente de los queries a la base de datos
         *debe ser el unico elemento que tenga acceso a la base de datos, no se puede aceder a la base de datos sin pasar
         *por el DT, el DT proporciona todo los métodos necesarios para la inserción, eliminación, actualización y
         *recuperación de los datos en la base de datos COSECOL. El DT se comunica por medio dle protocolo DK*/

        /*VARIABLES DEL DT*/
        /*Esta es la variable donde se ingresan los Queries para ser ejecutados en la BD*/
        public static SqlCommand Query;
        /*Esta es la variable que se encarga de leer datos de la BD*/
        public static SqlDataReader DataReader;
        /*Esta es la variable que se encaraga de la conexión a la BD*/
        public static SqlConnection Connection;
        /*Esta variable almacena la información necesaria para conectarse a la BD, debe ser completamente privada!!*/

        public static string ConnectionString = "Data Source=OPTIMUSPRIME;Initial Catalog=OdessaPatLab_DB;Integrated Security=True",
            /*Esta es la variable donde escribimos todos los queries*/
        QueryString = "";

        /*por problemas del protocolo HTTP que envia paquetes a cada rato, la referencia a la conexion siempre se perdera
         *por lo tanto en cada metodo debemos de crear una conexion nueva y desconectarnos para ello simplemente debemos
         *de llamar antes de cada query al metodo CONECTAR, El standard para enviar datos es enviarlos separados por , 
         *pero para resibir y regresar objetos mas grandes como un afiliado o un empleado se utiliza lo que es el formato DK
         *que por motivos de segurdad no estan especificados en esta libreria, para utilización de estos metodos se requiere
         *conocer el protocolo DK.*/



        /********************************************************************************************************************************
        *********************************************** LOGIN ***************************************************************************
        *********************************************************************************************************************************/
        public static String loginAlSistema(string usuario, string contrasena)
        {
            QueryString = "SELECT Password FROM Empleado WHERE " + "Nombre_Usuario=" + "'" + usuario + "'";

            string password = null;
            if (EjecutarComando())
                if (DataReader.Read())
                    password = DataReader.GetValue(0).ToString();

            if (contrasena == password)
            {
                return getLoginDatos(usuario);
            }
            else
            {
                return "False";
            }

        }//End public bool esUsuarioValido(string usuario, string contrasena)

        public static String getLoginDatos(string Usuario)
        {
            string Nombres = "";
            string Permisos = "";

            QueryString = "SELECT P_Nombre, S_Nombre, P_Apellido FROM Empleado WHERE Nombre_Usuario = '" + Usuario + "'";
            if (EjecutarComando())
                while (DataReader.Read())
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        Nombres += DataReader.GetValue(i).ToString() + ",";
            if (Nombres.Length > 0)
                Nombres = Nombres.Substring(0, Nombres.Length - 1);

            QueryString = "SELECT Nombre_Modulo FROM Acceso WHERE Nombre_Usuario = '" + Usuario + "'";
            if (EjecutarComando())
                while (DataReader.Read())
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        Permisos += DataReader.GetValue(i).ToString() + ",";
            if (Permisos.Length > 0)
                Permisos = Permisos.Substring(0, Permisos.Length - 1);

            FinalizarComandoCorrectamente();
            return Nombres + ";" + Permisos;
        }

        /********************************************************************************************************************************
        *********************************************************************************************************************************
        *********************************************    METODOS GETS   *****************************************************************
        *********************************************************************************************************************************
        *********************************************************************************************************************************/

        public static String getMedico(int id_medico)
        {
            QueryString = "SELECT * FROM Medico WHERE Medico.ID_Medico = " + id_medico + ";";
            String medico = "";

            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        medico += DataReader.GetValue(i).ToString() + ";";
                }
            }//End if (seEjecutoComando())

            FinalizarComandoCorrectamente();
            return medico;
        }//End public String getMedico(int id_medico)

        public static int getIDdeMedico(String nombre_Medico)
        {
            QueryString = "SELECT ID_Medico FROM Medico WHERE " + " Medico.Nombre = '" + nombre_Medico + "';";
            String resultado = "";

            if (EjecutarComando())
                while (DataReader.Read())
                    resultado = "" + DataReader.GetValue(0);
            if (resultado.CompareTo("") == 0)
                return -1;
            FinalizarComandoCorrectamente();
            return Convert.ToInt32(resultado);
        }// End public int getIDdeMedico

        public static int getIDdePaciente(String nombre_Paciente, String S_nombre_Paciente, String apellido_paciente, String S_apellido_Paciente)
        {
            QueryString = "SELECT ID_paciente FROM Paciente WHERE " +
                    " Paciente.P_Nombre = '" + nombre_Paciente + "' AND "
                    + "Paciente.P_Apellido = '" + apellido_paciente + "' AND Paciente.S_Nombre = '" + S_nombre_Paciente + "' "
                    + " AND Paciente.S_Apellido = '" + S_apellido_Paciente + "';";

            String resultado = "";

            if (EjecutarComando())
                while (DataReader.Read())
                    resultado = "" + DataReader.GetValue(0);
            if (resultado.CompareTo("") == 0)
                return -1;

            FinalizarComandoCorrectamente();
            return Convert.ToInt32(resultado);

        }// End public int getIDdePaciente


        public static ArrayList getMedicos_Nombres()
        {
            ArrayList medicos = new ArrayList();
            QueryString = "SELECT  Medico.Nombre FROM Medico ORDER BY Nombre;";

            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    String thisrow = "";
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        thisrow += DataReader.GetValue(i).ToString() + ";";
                    medicos.Add(thisrow.Substring(0, thisrow.Length));
                }
            }//End if (seEjecutoComando())

            FinalizarComandoCorrectamente();
            return medicos;
        }

        public static String getMedicosTodos()
        {
            QueryString = "SELECT * FROM Medico";

            String datos = "";
            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString() + ";";
                }
            }
            FinalizarComandoCorrectamente();
            return datos;
        }

        public static String getMaterialEnviadoBiopsiaImprimir(String idMuestra, String tabla)
        {
            QueryString = "SELECT Material_Enviado " +
                "FROM (SELECT * FROM Examen WHERE Examen.Codigo_Examen = '" + idMuestra + "') E " +
                "INNER JOIN " + tabla + " ON " + tabla + ".Codigo_Examen = E.Codigo_Examen ";

            String datos = "";

            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString() + ";";
                }
            }
            FinalizarComandoCorrectamente();
            return datos;
        }

        public static string getMaterialEnviado_CitologiaNoGinecologica()
        {
            QueryString = "SELECT DISTINCT Material_Enviado FROM Material ORDER BY Material_Enviado";

            String datos = "";
            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString() + ";";
                }
            }
            FinalizarComandoCorrectamente();
            return datos;

        }

        /*******************************************METODOS GET TODOS LOS ATRIBUTOS PARA IMPRIMIR************************************************************/
        public static String getMuestraGinecologica(String idMuestra)
        {
            QueryString = "SELECT E.Codigo_Examen, E.Fecha_Examen, Paciente.P_Nombre, Paciente.S_Nombre, Paciente.P_Apellido, " +
                    "Paciente.S_Apellido, Paciente.Edad, Medico.Nombre, G.Origen_muestra, Paciente.FUR, " +
                    "Paciente.DIU, G.Anticonceptivos_Orales, E.Diagnostico_Medico, G.Calidad_del_Frotis_Adecuado, " +
                    "G.Calidad_del_Frotis_Causa, G.Evaluacion_Hormonal_Basales, G.Evaluacion_Hormonal_Intermedias, " +
                    "G.Evaluacion_Hormonal_Superficiales, G.Inflamacion, G.Candida_SP, " +
                    "G.Agentes_Infecciosos_Gardnerella, G.Agentes_Infecciosos_Herpes, " +
                    "G.Agentes_Infecciosos_Vaginosis, G.Agentes_Infecciosos_Tricomonas, " +
                    "G.Agentes_Infecciosos_Otro, G.Diagnostico_Negativo_por_malignidad, G.Celula_Atipica, " +
                    "G.Diagnostico_Escamosa, G.Diagnostico_Glandular, G.LesionEscamosa_BajoGrado, " +
                    "G.Diagnostico_NIC_I, G.Diagnostico_Infecciones_VPH, G.LesionEscamosa_AltoGrado, " +
                    "G.Diagnostico_NIC_II, G.Diagnostico_NIC_III, G.Diagnostico_Carcinoma_Celula, " +
                    "G.Diagnostico_Adenocarcinoma, G.Recomendaciones_Repetir, G.Recomendaciones_Colposcopia, " +
                    "G.Recomendaciones_Biopsia, G.Recomendaciones__tratamiento, G.Recomendaciones_Otra, " +
                    "C.Comentario, E.Diagnostico, E.Fecha_Informe " +
            " FROM (SELECT * FROM Examen WHERE Examen.Codigo_Examen = '" + idMuestra + "') AS E, Paciente, Medico,  " +
                  "(SELECT * FROM Citologia WHERE Citologia.Codigo_Examen = '" + idMuestra + "') AS C, " +
                  "(SELECT * FROM Ginecologica WHERE Ginecologica.Codigo_Examen = '" + idMuestra + "') AS G " +
            " WHERE E.ID_Paciente = Paciente.ID_paciente AND E.ID_Medico = Medico.ID_Medico AND C.Codigo_Examen = E.Codigo_Examen;";

            String datos = "";
            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString() + "$";
                }
            }
            FinalizarComandoCorrectamente();
            return datos;

        }//End public String getMuestraGinecologica

        public static String getMuestraNo_Ginecologica(String idMuestra)
        {
            QueryString = "SELECT E.Codigo_Examen, E.Fecha_Examen, Paciente.P_Nombre, Paciente.S_Nombre, " +
                    "Paciente.P_Apellido, Paciente.S_Apellido, Paciente.Edad, E.Diagnostico_Medico, " +
                    "Medico.Nombre, NG.Descripcion_Macroscopica, " +
                    "NG.Descripcion_Microscopica, E.Diagnostico, E.Fecha_Informe, E.Fecha_Recibido " +
                 " FROM (SELECT * FROM Examen WHERE Examen.Codigo_Examen = '" + idMuestra + "') AS E, Paciente, Medico, " +
                       "(SELECT * FROM No_Ginecologica WHERE No_Ginecologica.Codigo_Examen = '" + idMuestra + "') AS NG " +
                 " WHERE E.ID_Paciente = Paciente.ID_paciente AND E.ID_Medico = Medico.ID_Medico AND NG.Codigo_Examen = E.Codigo_Examen;";


            String datos = "";
            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString() + "$";
                }
            }
            FinalizarComandoCorrectamente();
            return datos;

        }//End public String getMuestraNo_Ginecologica

        public static String getMuestraBiopsia(String idMuestra)
        {
            QueryString = "SELECT E.Codigo_Examen, E.Fecha_Examen, Paciente.P_Nombre, Paciente.S_Nombre, " +
                    "Paciente.P_Apellido, Paciente.S_Apellido, Paciente.Edad, E.Diagnostico_Medico, " +
                    "Medico.Nombre, B.Codificacion, B.Descripcion_Macroscopica, B.Descripcion_Microscopica, E.Diagnostico, E.Fecha_Informe, E.Fecha_Recibido " +
                " FROM (SELECT * FROM Examen WHERE Examen.Codigo_Examen = '" + idMuestra + "') AS E, Paciente, Medico, " +
                       "(SELECT * FROM Biopsia WHERE Biopsia.Codigo_Examen = '" + idMuestra + "') AS B " +
                 " WHERE E.ID_Paciente = Paciente.ID_paciente AND E.ID_Medico = Medico.ID_Medico AND B.Codigo_Examen = E.Codigo_Examen;";

            String datos = "";
            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString() + "$";
                }
            }
            FinalizarComandoCorrectamente();
            return datos;

        }

        public static String getIdExamenes()
        {
            QueryString = "SELECT Codigo_Examen FROM Examen WHERE ID_Medico <> ''";

            String datos = "";
            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString() + ";";
                }
            }
            FinalizarComandoCorrectamente();

            return datos;
        }

        /************************************************************************************************************************
         ************************************************************************************************************************
         ****************************************** GETS PARA LAS CONSULTAS *****************************************************
         ************************************************************************************************************************
         ************************************************************************************************************************/

        public static int getCantidadDeExamenes(String tabla, String filtadoporedad, String fechainicial, String fechafinal, String filtroCategoria, String doctor)
        {
            if (tabla.Equals("Todas"))
                QueryString = "SELECT COUNT(*) FROM (Paciente JOIN Examen ON Paciente.ID_paciente = Examen.ID_Paciente) ";
            else
                QueryString = "SELECT COUNT(*) FROM (Paciente JOIN (Examen JOIN " + tabla + " ON Examen.Codigo_Examen = " + tabla + ".Codigo_Examen) ON Paciente.ID_paciente = Examen.ID_Paciente) ";

            if (doctor.CompareTo("") != 0)
            {
                QueryString += ", (SELECT * FROM Medico WHERE Nombre = '" + doctor + "') As M WHERE M.ID_Medico = Examen.ID_Medico";
            }
            else
            {
                QueryString += " WHERE Examen.ID_Medico <> ''";
            }

            if (filtadoporedad.CompareTo("Ninguna") != 0)
            {
                if (filtadoporedad.CompareTo("Sin Edad") == 0)
                    QueryString += " AND Paciente.Edad = 0";
                if (filtadoporedad.CompareTo("< 20 Años") == 0)
                    QueryString += " AND Paciente.Edad < 20 AND Paciente.Edad <> 0";
                if (filtadoporedad.CompareTo("20 - 30 Años") == 0)
                    QueryString += " AND Paciente.Edad BETWEEN 20 AND 30";
                if (filtadoporedad.CompareTo("30 - 40 Años") == 0)
                    QueryString += " AND Paciente.Edad BETWEEN 30 AND 40";
                if (filtadoporedad.CompareTo("40 - 50 Años") == 0)
                    QueryString += " AND Paciente.Edad BETWEEN 40 AND 50";
                if (filtadoporedad.CompareTo("> 60 Años") == 0)
                    QueryString += " AND Paciente.Edad > 60 ";
            }

            if (filtroCategoria.CompareTo("Ninguna") != 0)
            {
                if (filtroCategoria.CompareTo("Adecuados") == 0)
                    QueryString += " AND Calidad_del_Frotis_Adecuado = 1";
                if (filtroCategoria.CompareTo("Inadecuados") == 0)
                    QueryString += " AND Calidad_del_Frotis_Adecuado = 0";
                if (filtroCategoria.CompareTo("Negativo por Malignidad") == 0)
                    QueryString += " AND Diagnostico_Negativo_por_malignidad = 1";
                if (filtroCategoria.CompareTo("Lesion de Alto Grado") == 0)
                    QueryString += " AND LesionEscamosa_AltoGrado = 1";
                if (filtroCategoria.CompareTo("Lesion de Bajo Grado") == 0)
                    QueryString += " AND LesionEscamosa_BajoGrado = 1";
                if (filtroCategoria.CompareTo("Cancer") == 0)
                    QueryString += " AND Diagnostico_Carcinoma_Celula = 1";

            }

            if (fechainicial.CompareTo("") != 0)
                QueryString += " AND Examen.Fecha > '" + fechainicial + "' ";
            if (fechafinal.CompareTo("") != 0)
                QueryString += " AND Examen.Fecha < '" + fechafinal + "' ";

            if (doctor.CompareTo("") != 0)
                QueryString += " AND M.Nombre = '" + doctor + "' ";

            String datos = "";
            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString();
                }
            }
            FinalizarComandoCorrectamente();
            return Convert.ToInt32(datos);
        }

        public static ArrayList getExamenesFiltrados(String tabla, String filtadoporedad, String fechainicial, String fechafinal, String filtroCategoria, String doctor)
        {

            QueryString = "";

            if (tabla.CompareTo("Biopsia") == 0)
                QueryString += "SELECT  Examen.Codigo_Examen, P_Nombre, S_nombre, P_Apellido, S_Apellido, Edad, Examen.Diagnostico, Medico.Nombre, Precio, Expediente, " +
                    "Fecha_Examen, Fecha_Informe, Registrado_por_empleado " +
                    "FROM (Medico JOIN (Paciente JOIN  (Examen JOIN Biopsia ON Examen.Codigo_Examen = Biopsia.Codigo_Examen)  ON Paciente.ID_paciente = Examen.ID_Paciente) " +
                    "ON Medico.ID_Medico = Examen.ID_Medico) WHERE Medico.ID_Medico <> ''";
            else if (tabla.CompareTo("Todas") == 0)
                QueryString += "SELECT  Examen.Codigo_Examen, P_Nombre, S_nombre, P_Apellido, S_Apellido, Edad, Examen.Diagnostico, Medico.Nombre, Precio, Expediente, " +
                    "Fecha_Examen, Fecha_Informe, Registrado_por_empleado " +
                    "FROM (Medico JOIN (Paciente JOIN  Examen ON Paciente.ID_paciente = Examen.ID_Paciente) " +
                    "ON Medico.ID_Medico = Examen.ID_Medico) WHERE Medico.ID_Medico <> ''";
            else
                QueryString += "SELECT  Examen.Codigo_Examen, P_Nombre, S_nombre, P_Apellido, S_Apellido, Edad, Examen.Diagnostico, Citologia.Comentario, Medico.Nombre, " +
                    "Precio, Expediente, Fecha_Examen, Fecha_Informe, Registrado_por_empleado" +
                    " FROM (Citologia JOIN (Medico JOIN  (Paciente JOIN  " +
                    "(Examen JOIN " + tabla + " ON Examen.Codigo_Examen = " + tabla + ".Codigo_Examen) ON Paciente.ID_paciente = Examen.ID_Paciente) " +
                    "ON Medico.ID_Medico = Examen.ID_Medico) ON Citologia.Codigo_Examen = Examen.Codigo_Examen) WHERE Medico.ID_Medico <> ''";

            if (filtadoporedad.CompareTo("Ninguna") != 0)
            {
                if (filtadoporedad.CompareTo("Sin Edad") == 0)
                    QueryString += " AND Paciente.Edad = 0";
                if (filtadoporedad.CompareTo("< 20 Años") == 0)
                    QueryString += " AND Paciente.Edad < 20 AND Paciente.Edad <> 0";
                if (filtadoporedad.CompareTo("20 - 30 Años") == 0)
                    QueryString += " AND Paciente.Edad BETWEEN 20 AND 30";
                if (filtadoporedad.CompareTo("30 - 40 Años") == 0)
                    QueryString += " AND Paciente.Edad BETWEEN 30 AND 40";
                if (filtadoporedad.CompareTo("40 - 50 Años") == 0)
                    QueryString += " AND Paciente.Edad BETWEEN 40 AND 50";
                if (filtadoporedad.CompareTo("> 60 Años") == 0)
                    QueryString += " AND Paciente.Edad > 60 ";
            }

            if (filtroCategoria.CompareTo("Ninguna") != 0)
            {
                if (filtroCategoria.CompareTo("Adecuados") == 0)
                    QueryString += " AND Calidad_del_Frotis_Adecuado = 1";
                if (filtroCategoria.CompareTo("Inadecuados") == 0)
                    QueryString += " AND Calidad_del_Frotis_Adecuado = 0";
                if (filtroCategoria.CompareTo("Negativo por Malignidad") == 0)
                    QueryString += " AND Diagnostico_Negativo_por_malignidad = 1";
                if (filtroCategoria.CompareTo("Lesion de Alto Grado") == 0)
                    QueryString += " AND LesionEscamosa_AltoGrado = 1";
                if (filtroCategoria.CompareTo("Lesion de Bajo Grado") == 0)
                    QueryString += " AND LesionEscamosa_BajoGrado = 1";
                if (filtroCategoria.CompareTo("Cancer") == 0)
                    QueryString += " AND Diagnostico_Carcinoma_Celula = 1";

            }

            if (fechainicial.CompareTo("") != 0)
                QueryString += " AND Examen.Fecha > '" + fechainicial + "' ";
            if (fechafinal.CompareTo("") != 0)
                QueryString += " AND Examen.Fecha < '" + fechafinal + "' ";

            if (doctor.CompareTo("") != 0)
                QueryString += " AND Medico.Nombre = '" + doctor + "' ";

            ArrayList datos = new ArrayList();
            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    String thisrow = "";
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        thisrow += DataReader.GetValue(i).ToString() + ";";
                    datos.Add(thisrow);
                }
            }
            FinalizarComandoCorrectamente();
            return datos;
        }

        public static string consultaMedicoBiopsia(int anio, int mes, String doctor)
        {
            QueryString = "";

            QueryString += "SELECT Medico.Nombre, SUM(Examen.Precio), COUNT(*) " +
                           "FROM Medico JOIN (Examen JOIN Biopsia ON Examen.Codigo_Examen = Biopsia.Codigo_Examen) ON Examen.ID_Medico = Medico.ID_Medico " +
                            "WHERE Examen.ID_Medico = Medico.ID_Medico AND Examen.Codigo_Examen = Biopsia.Codigo_Examen ";

            if (anio != 0)
                QueryString += " AND Anio_contable = " + anio.ToString() + " ";
            if (mes != 0)
                QueryString += " AND Mes_contable = " + mes.ToString() + " ";
            if (doctor.CompareTo("") != 0)
                QueryString += " AND Medico.Nombre = '" + doctor + "' ";

            QueryString += " GROUP BY Medico.Nombre;";


            String datos = "";
            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString() + ";";
                }
            }
            FinalizarComandoCorrectamente();
            return datos;
        }

        public static string consultaMedicoCitologia(int anio, int mes, String doctor)
        {
            QueryString = "";

            QueryString += "SELECT Medico.Nombre, SUM(Examen.Precio), COUNT(*) " +
                           "FROM Medico JOIN (Examen JOIN Citologia ON Examen.Codigo_Examen = Citologia.Codigo_Examen) ON Examen.ID_Medico = Medico.ID_Medico " +
                            "WHERE Examen.ID_Medico = Medico.ID_Medico AND Examen.Codigo_Examen = Citologia.Codigo_Examen ";

            if (anio != 0)
                QueryString += "AND Anio_contable = " + anio.ToString() + " ";
            if (mes != 0)
                QueryString += "AND Mes_contable = " + mes.ToString() + " ";
            if (doctor.CompareTo("") != 0)
                QueryString += " AND Medico.Nombre = '" + doctor + "' ";

            QueryString += " GROUP BY Medico.Nombre;";


            String datos = "";
            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString() + ";";
                }
            }
            FinalizarComandoCorrectamente();
            return datos;
        }

        /**************************************************************************************************************************
        ***************************************************************************************************************************
        *********************************************    OTROS GETS SELECTS   *****************************************************
        ***************************************************************************************************************************
        ***************************************************************************************************************************/
        /*
        public static ArrayList getNombresPacientes()
        {
            QueryString = "SELECT DISTINCT P_Nombre, S_Nombre, P_Apellido, S_Apellido FROM Paciente";

            ArrayList datos = new ArrayList();

            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    String thisrow = "";
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        thisrow += DataReader.GetValue(i).ToString() + " ";
                    datos.Add(thisrow + ";");
                }
            }
            FinalizarComandoCorrectamente();
            return datos;
        }
        */
        public static ArrayList getNombresPacientes()
        {
            QueryString = "SELECT P_Nombre, S_Nombre, P_Apellido, S_Apellido FROM Paciente";

            ArrayList datos = new ArrayList();

            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    NombreCompleto thisrow = new NombreCompleto();
                    for (int i = 0; i < DataReader.FieldCount; i++)
                    {
                        switch (i % 4)
                        {
                            case 0:
                                thisrow.PrimerNombre = DataReader.GetValue(i).ToString();
                                break;
                            case 1:
                                thisrow.SegundoNombre = DataReader.GetValue(i).ToString();
                                break;
                            case 2:
                                thisrow.PrimerApellido = DataReader.GetValue(i).ToString();
                                break;
                            case 3:
                                thisrow.SegundoApellido = DataReader.GetValue(i).ToString();
                                break;
                        }
                    }
                    datos.Add(thisrow);
                }
            }
            FinalizarComandoCorrectamente();
            return datos;
        }


        public static String getUsuarios()
        {

            QueryString = "SELECT Nombre_Usuario, P_Nombre, S_Nombre, P_Apellido, S_Apellido, Tipo_Empleado, Correo FROM Empleado";


            String datos = "";
            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString() + ";";
                }
            }
            FinalizarComandoCorrectamente();
            if (datos.Equals(""))
                return datos;
            else
                return datos.Substring(0, datos.Length - 1);
        }

        public static String getDatosEmpleado(string usuario)
        {
            QueryString = "SELECT * FROM Empleado WHERE " +
                    " Empleado.Nombre_Usuario = '" + usuario + "';";

            String datos = "";
            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString() + ";";
                }
            }
            FinalizarComandoCorrectamente();
            return datos;
        }

        public static String getAccesosDeUsuario(string usuario)
        {
            QueryString = "SELECT Nombre_Modulo FROM " +
                    "Acceso WHERE Nombre_Usuario = '" + usuario + "';";

            String datos = "";
            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString() + ";";
                }
            }
            FinalizarComandoCorrectamente();
            return datos;
        }

        /**************************************************************************************************************************
        ***************************************************************************************************************************
        *********************************************    METODOS INSERT   *********************************************************
        ***************************************************************************************************************************
        ***************************************************************************************************************************/


        public static bool InsertarMedico(String Nombre, String telefono, String celular, String direccion, String compania, String numeroCol)
        {
            Nombre = Traductor.traductorNombres(Nombre);
            direccion = Traductor.traductorNombres(direccion);
            compania = Traductor.traductorNombres(compania);

            /*QueryString = "INSERT INTO Medico VALUES ('" + Nombre +
                            "', '" + telefono + "' ," + "'" + celular + "','" + direccion + "','" +
                            compania + "','" + numeroCol + "');";
             */
            QueryString = "";
            for (int i = 0; i <= 3000; i++)
            {
                QueryString += " INSERT INTO Paciente VALUES('', '', '', 'Lourdes', 'Isabel', 'Bermudez', 'Flores', 20, '', ''); ";
            }
            if (EjecutarComando())
            {
                FinalizarComandoCorrectamente();
                return true;
            }
            else
                return false;
        }

        public static bool InsertarMaterial(String material_enviado)
        {
            material_enviado = Traductor.traductorNombres(material_enviado);

            QueryString = "INSERT INTO Material VALUES('" + material_enviado + "');";
            if (EjecutarComando())
            {
                FinalizarComandoCorrectamente();
                return true;
            }
            else
                return false;
        }

        public static bool InsertarPaciente(bool DIU, String fur, String fup, String pnombre,
            String snombre, String papellido, String sapellido, int edad, bool anticonceptivos, String Expediente)
        {
            pnombre = Traductor.traductorNombres(pnombre);
            snombre = Traductor.traductorNombres(snombre);
            papellido = Traductor.traductorNombres(papellido);
            sapellido = Traductor.traductorNombres(sapellido);

            QueryString = "INSERT INTO Paciente VALUES ( '" +
                DIU.ToString() + "' , '" + fur + "','" + fup +
                "' , '" + pnombre + "' , '" + snombre + "' ,  '" + papellido + "', '" + sapellido + "' , "
                + edad.ToString() + ", '" + Expediente + "' , '" + anticonceptivos.ToString() + "');";

            if (EjecutarComando())
            {
                FinalizarComandoCorrectamente();
                return true;
            }
            else
                return false;
        }

        public static bool InsertarBiopsia(String cod_examen, String fecha, int precio, String Diagnostico,
            String diagnostico_medico, int Id_paciente, int id_medico, String Nombre_Contable,
            int anio_contable, int mes_contable, String usuario_empleado, String descripcion_macros,
            String descripcion_micros, String codificacion, String fechaInforme, String fechaIngresada, String fechaMuestra)
        {
            QueryString = "INSERT INTO Examen VALUES( '" + cod_examen + "', '" + fecha + "', " + precio.ToString()
                + ",'" + Diagnostico + "', '" + diagnostico_medico + "','" + usuario_empleado + "', " + Id_paciente.ToString()
                + "," + id_medico.ToString() + ",'" + Nombre_Contable + "' , " + anio_contable.ToString()
                + " , " + mes_contable + " , '" + fechaInforme + "' , '" + fechaIngresada + "' , '" + fechaMuestra + "');";


            if (EjecutarComando())
            {
                FinalizarComandoCorrectamente();
                QueryString = "INSERT INTO Biopsia VALUES( '" + cod_examen + "','"
                    + descripcion_macros + "' , '" + descripcion_micros + "','" + codificacion + "');";
                if (EjecutarComando())
                {
                    FinalizarComandoCorrectamente();
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public static bool InsertarCitologiaNoGinecologica(String cod_examen, String fecha, int precio, String Diagnostico,
            String diagnostico_medico, int Id_paciente, int id_medico, String Nombre_Contable,
            int anio_contable, int mes_contable, String usuario_empleado, String descripcion_macros,
            String descripcion_micros, String comentario, String fechaInforme, String fechaIngresada, String fechaMuestra)
        {
            QueryString = "INSERT INTO Examen VALUES( '" + cod_examen + "', '" + fecha + "', " + precio.ToString()
                + ",'" + Diagnostico + "', '" + diagnostico_medico + "','" + usuario_empleado + "', " + Id_paciente.ToString()
                + "," + id_medico.ToString() + ",'" + Nombre_Contable + "' , " + anio_contable.ToString()
                + " , " + mes_contable + " , '" + fechaInforme + "' , '" + fechaIngresada + "' , '" + fechaMuestra + "' );";

            if (!EjecutarComando())
                return false;
            else
            {
                FinalizarComandoCorrectamente();
                QueryString = "INSERT INTO Citologia VALUES( '" + cod_examen + "','"
                    + comentario + "');";

                if (!EjecutarComando())
                    return false;
                else
                {
                    FinalizarComandoCorrectamente();
                    QueryString = "INSERT INTO No_Ginecologica VALUES( '" + cod_examen + "','"
                        + descripcion_macros + "' , '" + descripcion_micros + "');";

                    if (EjecutarComando())
                    {
                        FinalizarComandoCorrectamente();
                        return true;
                    }
                    else
                        return false;
                }
            }
        }

        public static bool InsertarCitologiaGinecologica(String cod_examen, String fecha, int precio, String Diagnostico,
            String diagnostico_medico, int Id_paciente, int id_medico, String Nombre_Contable,
            int anio_contable, int mes_contable, String usuario_empleado, String comentario,
            String inflamacion, String calidadFrotis_causa, bool calidadFrotisAdecuado, bool anticonceptivos,
            bool candida_sp, bool gardnerela, bool vaginosis, bool herpes, bool tricomonas,
            String otroAgenteInfeccioso, int evaluacionHormonal_basales,
            int evaluacionhormonal_intermedias, int evaluacionhormonal_superficiales, bool Colposcopia,
            int repetir, String recomendaciones_otra, bool recomendaciones_biopsia, bool recomendaciones_tratamientos,
            bool NIC_I, bool NIC_II, bool NIC_III, String origen_muestra, bool negativo, bool VPH,
            bool glandular, bool escamoza, bool adenocarcinomana, bool carcinomana_celula, bool celula_atipica,
            bool lesion_escamosa_AltoGrado, bool lesion_escamosa_BajoGrado, String fechaInforme, String fechaIngresada)
        {
            QueryString = "INSERT INTO Examen VALUES( '" + cod_examen + "', '" + fecha + "', " + precio.ToString()
                + ",'" + Diagnostico + "', '" + diagnostico_medico + "','" + usuario_empleado + "' , " + Id_paciente.ToString()
                + "," + id_medico.ToString() + ",'" + Nombre_Contable + "' , " + anio_contable.ToString()
                + " , " + mes_contable + " , '" + fechaInforme + "' , '" + fechaIngresada + "' , '" + fechaInforme + "' );";

            QueryString += " INSERT INTO Citologia VALUES( '" + cod_examen + "','" + comentario + "');";

            QueryString += " INSERT INTO Ginecologica "
            + "VALUES( '" + cod_examen + "','" + inflamacion + "','" + calidadFrotis_causa + "','" +
            calidadFrotisAdecuado.ToString() + "','" + anticonceptivos.ToString() + "','" +
            candida_sp.ToString() + "','" + gardnerela.ToString() + "','" + vaginosis.ToString()
            + "','" + herpes.ToString() + "','" + tricomonas.ToString() + "','" + otroAgenteInfeccioso
            + "','" + evaluacionHormonal_basales.ToString() + "','" + evaluacionhormonal_intermedias.ToString()
            + "','" + evaluacionhormonal_superficiales.ToString() + "','" + Colposcopia + "','" +
            repetir.ToString() + "','" + recomendaciones_otra + "','" + recomendaciones_biopsia.ToString() +
            "','" + recomendaciones_tratamientos + "','" + NIC_I.ToString() +
            "','" + NIC_II.ToString() + "','" + NIC_III.ToString() + "','" + celula_atipica.ToString()
            + "' , '" + lesion_escamosa_BajoGrado.ToString() + "' , '" + lesion_escamosa_AltoGrado.ToString()
            + "','" + origen_muestra + "','" + negativo.ToString() + "','" + VPH.ToString() + "','" + glandular.ToString() + "','" + escamoza.ToString() +
            "','" + adenocarcinomana.ToString() + "','" + carcinomana_celula.ToString() + "');";

            if (EjecutarComando())
            {
                FinalizarComandoCorrectamente();
                return true;
            }
            else
                return false;
        }

        public static bool InsertarMaterialEnviado(String cod_examen, String material_enviado, String tabla)
        {
            QueryString = "";

            String[] mats = material_enviado.Split(';');

            for (int i = 0; i < mats.Length - 1; i++)
            {
                QueryString += "INSERT INTO " + tabla + " " 
                + "VALUES('" + cod_examen + "','" + mats[i] + "'); ";
            }

            if (EjecutarComando())
            {
                FinalizarComandoCorrectamente();
                return true;
            }
            else
                return false;
        }

        public static bool InsertarEmpleado(String usuario, String tipoEmpleado, String p_nombre, String s_nombre, String p_apellido, String s_apellido,
            String contrasena, String correo, String accesos)
        {
            QueryString = "INSERT INTO Empleado VALUES( '" + usuario + "', '" + tipoEmpleado + "', '" + p_nombre + "', '" + s_nombre + "', '" +
                p_apellido + "', '" + s_apellido + "', '" + contrasena + "', '" + correo + "'); ";

            if (accesos.CompareTo("") != 0)
            {
                String[] temp = accesos.Split('&');
                for (int i = 0; i < temp.Length; i++)
			    {
                    QueryString += "INSERT INTO Acceso VALUES('" + temp[i] + "', '" + usuario + "'); ";
                }

            }
            if (EjecutarComando())
            {
                FinalizarComandoCorrectamente();
                return true;
            }
            else
                return false;
        }




        /**************************************************************************************************************************
        ***************************************************************************************************************************
        *********************************************    METODOS BORRAR   *********************************************************
        ***************************************************************************************************************************
        ***************************************************************************************************************************/

        public static bool BorrarMedico(String nombre)
        {
            QueryString = "DELETE FROM Medico " +
                "WHERE Medico.Nombre = '" + nombre + "'";

            if (EjecutarComando())
            {
                if (DataReader.RecordsAffected == 0)
                {
                    FinalizarComandoCorrectamente();
                    return false;
                }
                else
                {
                    FinalizarComandoCorrectamente();
                    return true;
                }
            }
            else
                return false;
        }

        public static bool BorrarMuestra(String codigo)
        {
            QueryString = "DELETE FROM Examen " +
                "WHERE Codigo_Examen = '" + codigo + "'";

            if (EjecutarComando())
            {
                if (DataReader.RecordsAffected == 0)
                {
                    FinalizarComandoCorrectamente();
                    return false;
                }
                else
                {
                    FinalizarComandoCorrectamente();
                    return true;
                }
            }
            else
                return false;
        }

        public static bool BorrarMaterial(String material)
        {
            QueryString = "DELETE FROM Material " +
                "WHERE Material.Material_Enviado = '" + material + "'";

            if (EjecutarComando())
            {
                if (DataReader.RecordsAffected == 0)
                {
                    FinalizarComandoCorrectamente();
                    return false;
                }
                else
                {
                    FinalizarComandoCorrectamente();
                    return true;
                }
            }
            else
                return false;
        }

        public static bool BorrarEmpleado(String usuario)
        {
            QueryString = "DELETE FROM Empleado " +
                "WHERE Nombre_Usuario = '" + usuario + "'";

            if (EjecutarComando())
            {
                if (DataReader.RecordsAffected == 0)
                {
                    FinalizarComandoCorrectamente();
                    return false;
                }
                else
                {
                    FinalizarComandoCorrectamente();
                    return true;
                }
            }
            else
                return false;
        }

        /**************************************************************************************************************************
        ***************************************************************************************************************************
        *********************************************    METODOS UPDATE   *********************************************************
        ***************************************************************************************************************************
        ***************************************************************************************************************************/

        public static bool ActualizarCitologia(String cod_examen, String Diagnostico, String comentario,
            String inflamacion, String calidadFrotis_causa, bool calidadFrotisAdecuado,
            bool candida_sp, bool gardnerela, bool vaginosis, bool herpes, bool tricomonas,
            String otroAgenteInfeccioso, int evaluacionHormonal_basales,
            int evaluacionhormonal_intermedias, int evaluacionhormonal_superficiales, bool Colposcopia,
            int repetir, String recomendaciones_otra, bool recomendaciones_biopsia, bool recomendaciones_tratamientos,
            bool NIC_I, bool NIC_II, bool NIC_III, String origen_muestra, bool negativo, bool VPH,
            bool glandular, bool escamosa, bool adenocarcinomana, bool carcinomana_celula, bool celula_atipica,
            bool lesion_escamosa_AltoGrado, bool lesion_escamosa_BajoGrado, bool anticonceptivos_orales,
            String fecha_examen, String diagnostico_medico, bool diu, String fur, String fup, String p_nombre,
            String s_nombre, String p_apellido, String s_apellido, int edad, String expediente, int precio)
        {
            String idPaciente = "";

            QueryString = "UPDATE Ginecologica " +
            "SET Calidad_del_Frotis_Adecuado = '" + calidadFrotisAdecuado + "', Calidad_del_Frotis_Causa = '" + calidadFrotis_causa + "', " +
            "Evaluacion_Hormonal_Basales = " + evaluacionHormonal_basales + ", Evaluacion_Hormonal_Intermedias = " + evaluacionhormonal_intermedias +
            ", Evaluacion_Hormonal_Superficiales = " + evaluacionhormonal_superficiales + ", Inflamacion = '" + inflamacion + "', " +
            "Candida_SP = '" + candida_sp + "', Agentes_Infecciosos_Gardnerella = '" + gardnerela + "', Agentes_Infecciosos_Vaginosis = '" + vaginosis + "'," +
            "Agentes_Infecciosos_Herpes = '" + herpes + "', Agentes_Infecciosos_Tricomonas = '" + tricomonas +
            "', Agentes_Infecciosos_Otro = '" + otroAgenteInfeccioso + "', Diagnostico_Negativo_por_malignidad = '" + negativo +
            "', Celula_Atipica = '" + celula_atipica + "', Diagnostico_Escamosa = '" + escamosa + "', Diagnostico_Glandular = '" + glandular + "'," +
            " LesionEscamosa_BajoGrado = '" + lesion_escamosa_BajoGrado + "', Diagnostico_NIC_I = '" + NIC_I + "', Diagnostico_Infecciones_VPH = '" + VPH + "'," +
            " LesionEscamosa_AltoGrado = '" + lesion_escamosa_AltoGrado + "', Diagnostico_NIC_II = '" + NIC_II + "', Diagnostico_NIC_III = '" + NIC_III + "'," +
            "Diagnostico_Carcinoma_Celula = '" + carcinomana_celula + "', Diagnostico_Adenocarcinoma = '" + adenocarcinomana + "', Recomendaciones_Repetir = " + repetir +
            ", Recomendaciones_Colposcopia = '" + Colposcopia + "', Recomendaciones_Biopsia = '" + recomendaciones_biopsia +
            "', Recomendaciones__tratamiento = '" + recomendaciones_tratamientos + "', Recomendaciones_Otra = '" + recomendaciones_otra +
            "', Anticonceptivos_Orales =  '" + anticonceptivos_orales + "', Origen_muestra = '" + origen_muestra + "' " +
            "WHERE Ginecologica.Codigo_Examen = '" + cod_examen + "'; ";

            QueryString += "UPDATE Examen " +
            "SET Fecha_Examen = '" + fecha_examen + "', Diagnostico = '" + Diagnostico + "', Diagnostico_Medico = '" + diagnostico_medico + "', Precio = " + precio +
            " WHERE Examen.Codigo_Examen = '" + cod_examen + "'; ";

            QueryString += "UPDATE Citologia " +
            "SET Comentario = '" + comentario + "' " +
            "WHERE Citologia.Codigo_Examen = '" + cod_examen + "'; ";

            if (EjecutarComando())
            {
                FinalizarComandoCorrectamente();
                QueryString = "SELECT ID_Paciente FROM Examen WHERE Codigo_Examen = '" + cod_examen + "'";

                if (EjecutarComando())
                {
                    while (DataReader.Read())
                    {
                        for (int i = 0; i < DataReader.FieldCount; i++)
                            idPaciente = "" + DataReader.GetValue(i).ToString();
                    }
                }
                FinalizarComandoCorrectamente();
                ActualizarPaciente(Convert.ToInt32(idPaciente), diu, fur, fup, p_nombre, s_nombre, p_apellido, s_apellido, edad, anticonceptivos_orales, expediente);
                return true;
            }
            else
                return false;
        }


        public static bool ActualizarBiopsia(String cod_examen, String macroscopica, String microscopica, String codificacion, String diagnostico,
            String fecha_examen, String diagnostico_medico, bool diu, String fur, String fup, String p_nombre,
            String s_nombre, String p_apellido, String s_apellido, int edad, String expediente, String material_enviado, int precio, String fechaMuestra)
        {
            String idPaciente = "";

            QueryString = "UPDATE Biopsia " +
                "SET Descripcion_Macroscopica = '" + macroscopica + "', Descripcion_Microscopica = '" + microscopica + "', " +
                "Codificacion = '" + codificacion + "' " +
                "WHERE Codigo_Examen = '" + cod_examen + "'; ";

            QueryString += "UPDATE Examen " +
                "SET Diagnostico = '" + diagnostico + "', Fecha_Examen = '" + fecha_examen + "', Diagnostico_Medico = '" + diagnostico_medico + "', Precio = " + precio +
                ", Fecha_Recibido = '" + fechaMuestra + "' WHERE Codigo_Examen = '" + cod_examen + "'; ";

            QueryString += "DELETE FROM MaterialEnviadoBiopsia WHERE Codigo_Examen = '" + cod_examen + "'; ";

            if (EjecutarComando())
            {
                FinalizarComandoCorrectamente();
                InsertarMaterialEnviado(cod_examen, material_enviado, "MaterialEnviadoBiopsia");
                QueryString = "SELECT ID_Paciente FROM Examen WHERE Codigo_Examen = '" + cod_examen + "'";

                if (EjecutarComando())
                {
                    while (DataReader.Read())
                    {
                        for (int i = 0; i < DataReader.FieldCount; i++)
                            idPaciente = "" + DataReader.GetValue(i).ToString();
                    }
                }
                FinalizarComandoCorrectamente();
                ActualizarPaciente(Convert.ToInt32(idPaciente), diu, fur, fup, p_nombre, s_nombre, p_apellido, s_apellido, edad, false, expediente);
                return true;
            }
            else
                return false;
        }

        public static bool ActualizarCitologiaLiquidos(String cod_examen, String macroscopica, String microscopica, String diagnostico,
            String fecha_examen, String diagnostico_medico, bool diu, String fur, String fup, String p_nombre,
            String s_nombre, String p_apellido, String s_apellido, int edad, String expediente, int precio, String material_enviado, String fechaMuestra)
        {
            String idPaciente = "";

            QueryString = "UPDATE No_Ginecologica " +
                "SET Descripcion_Macroscopica = '" + macroscopica + "', Descripcion_Microscopica = '" + microscopica + "' " +
                "WHERE Codigo_Examen = '" + cod_examen + "';";

            QueryString += "UPDATE Examen " +
                "SET Diagnostico = '" + diagnostico + "', Fecha_Examen = '" + fecha_examen + "', Diagnostico_Medico = '" + diagnostico_medico +
                "', Precio = " + precio + " , Fecha_Recibido = '" + fechaMuestra + "' " + 
                " WHERE Codigo_Examen = '" + cod_examen + "'; ";

            QueryString += "DELETE FROM MaterialEnviadoCitologia WHERE Codigo_Examen = '" + cod_examen + "'; ";

            if (EjecutarComando())
            {
                FinalizarComandoCorrectamente();
                InsertarMaterialEnviado(cod_examen, material_enviado, "MaterialEnviadoCitologia");
                QueryString = "SELECT ID_Paciente FROM Examen WHERE Codigo_Examen = '" + cod_examen + "'";

                if (EjecutarComando())
                {
                    while (DataReader.Read())
                    {
                        for (int i = 0; i < DataReader.FieldCount; i++)
                            idPaciente = "" + DataReader.GetValue(i).ToString();
                    }
                }
                FinalizarComandoCorrectamente();
                ActualizarPaciente(Convert.ToInt32(idPaciente), diu, fur, fup, p_nombre, s_nombre, p_apellido, s_apellido, edad, false, expediente);
                return true;
            }
            else
                return false;
        }

        public static bool ActualizarMedico(String nombre, String telefono, String celular, String direccion, String compania, String numeroCol)
        {

            QueryString = "UPDATE Medico " +
            "SET telefono = '" + telefono + "', celular = '" + celular + "', direccion = '" + direccion +
            "', compania = '" + compania + "', numeroColegiatura = '" + numeroCol + "' " +
            "WHERE Nombre = '" + nombre + "';";

            if (EjecutarComando())
            {
                FinalizarComandoCorrectamente();
                return true;
            }
            else
                return false;
        }

        public static bool ActualizarPaciente(int id, bool DIU, String fur, String fup, String pnombre,
            String snombre, String papellido, String sapellido, int edad, bool anticonceptivos, String Expediente)
        {
            QueryString = "UPDATE Paciente SET DIU = '" + DIU.ToString() + "', FUR = '" + fur + "', FUP = '" + fup +
                "', P_Nombre = '" + pnombre + "', S_Nombre = '" + snombre + "', P_Apellido = '" + papellido +
                "', S_Apellido = '" + sapellido + "', Edad = " + edad + " , Expediente = '" + Expediente +
                "', Anticonceptivos_Orales = '" + anticonceptivos + "' " +
                " WHERE ID_paciente = " + id + ";";

            if (EjecutarComando())
            {
                FinalizarComandoCorrectamente();
                return true;
            }
            else
                return false;
        }

        public static bool ActualizarUsuario(String usuario, String tipoEmpleado, String p_nombre, String s_nombre, String p_apellido, String s_apellido,
            String contrasena, String correo, String accesos)
        {
            QueryString = "UPDATE Empleado SET Tipo_Empleado = '" + tipoEmpleado + "', P_Nombre = '" + p_nombre + "', S_Nombre = '" + s_nombre +
                            "',  P_Apellido = '" + p_apellido + "', S_Apellido = '" + s_apellido + "', Password = '" + contrasena + "' , Correo = '" + correo +
                            "' WHERE Nombre_Usuario = '" + usuario + "';";

            QueryString += "DELETE FROM Acceso WHERE Nombre_Usuario = '" + usuario + "';";

            if (accesos.CompareTo("") != 0)
            {
                String[] temp = accesos.Split('&');
                for (int i = 0; i < temp.Length; i++)
                {
                    QueryString += "INSERT INTO Acceso VALUES('" + temp[i] + "', '" + usuario + "'); ";
                }

            }

            if (EjecutarComando())
            {
                FinalizarComandoCorrectamente();
                return true;
            }
            else
                return false;
        }


        /**************************************************************************************************************************
        ***************************************************************************************************************************
        *********************************************    METODOS BUSCAR   *********************************************************
        ***************************************************************************************************************************
        ***************************************************************************************************************************/
        public static String buscarBiopsia(String idMuestra)
        {
            QueryString = "SELECT E.Codigo_Examen, E.Fecha_Examen, Paciente.P_Nombre, Paciente.S_Nombre, " +
                "Paciente.P_Apellido, Paciente.S_Apellido, Paciente.Edad, Medico.Nombre, E.Precio, Paciente.Expediente, E.Diagnostico_Medico, " +
                "B.Codificacion, B.Descripcion_Macroscopica, B.Descripcion_Microscopica, E.Diagnostico, E.Fecha_Recibido " +
            " FROM (SELECT * FROM Examen WHERE Examen.Codigo_Examen = '" + idMuestra + "') AS E, Paciente, Medico, " +
                   "(SELECT * FROM Biopsia WHERE Biopsia.Codigo_Examen = '" + idMuestra + "') AS B " +
             " WHERE E.ID_Paciente = Paciente.ID_paciente AND E.ID_Medico = Medico.ID_Medico AND B.Codigo_Examen = E.Codigo_Examen;";

            String datos = "";
            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString() + "$";
                }
            }
            FinalizarComandoCorrectamente();
            return datos;

        }//End public String buscarBiopsia

        public static String buscarCitologia(String idMuestra)
        {
            QueryString = "SELECT E.Codigo_Examen, E.Fecha_Examen, Paciente.P_Nombre, Paciente.S_Nombre, Paciente.P_Apellido, " +
                       "Paciente.S_Apellido, Medico.Nombre, Paciente.Edad, G.Origen_muestra, Paciente.FUP, Paciente.FUR, E.Precio, " +
                       "Paciente.DIU, G.Anticonceptivos_Orales, Paciente.Expediente, E.Diagnostico_Medico, G.Calidad_del_Frotis_Adecuado, " +
                       "G.Calidad_del_Frotis_Causa, G.Evaluacion_Hormonal_Basales, G.Evaluacion_Hormonal_Intermedias, " +
                       "G.Evaluacion_Hormonal_Superficiales, G.Inflamacion, G.Candida_SP, " +
                       "G.Agentes_Infecciosos_Gardnerella, G.Agentes_Infecciosos_Herpes, " +
                       "G.Agentes_Infecciosos_Vaginosis, G.Agentes_Infecciosos_Tricomonas, " +
                       "G.Agentes_Infecciosos_Otro, G.Diagnostico_Negativo_por_malignidad, G.Celula_Atipica, " +
                       "G.Diagnostico_Escamosa, G.Diagnostico_Glandular, G.LesionEscamosa_BajoGrado, " +
                       "G.Diagnostico_NIC_I, G.Diagnostico_Infecciones_VPH, G.LesionEscamosa_AltoGrado, " +
                       "G.Diagnostico_NIC_II, G.Diagnostico_NIC_III, G.Diagnostico_Carcinoma_Celula, " +
                       "G.Diagnostico_Adenocarcinoma, G.Recomendaciones_Repetir, G.Recomendaciones_Colposcopia, " +
                       "G.Recomendaciones_Biopsia, G.Recomendaciones__tratamiento, G.Recomendaciones_Otra, " +
                       "C.Comentario, E.Diagnostico" +
               " FROM (SELECT * FROM Examen WHERE Examen.Codigo_Examen = '" + idMuestra + "') AS E, Paciente, Medico,  " +
                     "(SELECT * FROM Citologia WHERE Citologia.Codigo_Examen = '" + idMuestra + "') AS C, " +
                     "(SELECT * FROM Ginecologica WHERE Ginecologica.Codigo_Examen = '" + idMuestra + "') AS G " +
               " WHERE E.ID_Paciente = Paciente.ID_paciente AND E.ID_Medico = Medico.ID_Medico AND C.Codigo_Examen = E.Codigo_Examen;";

            String datos = "";
            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString() + "$";
                }
            }
            FinalizarComandoCorrectamente();
            return datos;
        }

        public static String buscarCitologiaLiquidos(String idMuestra)
        {
            QueryString = "SELECT E.Codigo_Examen, E.Fecha_Examen, Paciente.P_Nombre, Paciente.S_Nombre, " +
                    "Paciente.P_Apellido, Paciente.S_Apellido, Paciente.Edad, Medico.Nombre, E.Precio, " +
                    "Paciente.Expediente, E.Diagnostico_Medico, NG.Descripcion_Macroscopica, " +
                    "NG.Descripcion_Microscopica, E.Diagnostico, E.Fecha_Recibido " +
                 " FROM (SELECT * FROM Examen WHERE Examen.Codigo_Examen = '" + idMuestra + "') AS E, Paciente, Medico, " +
                       "(SELECT * FROM No_Ginecologica WHERE No_Ginecologica.Codigo_Examen = '" + idMuestra + "') AS NG " +
                 " WHERE E.ID_Paciente = Paciente.ID_paciente AND E.ID_Medico = Medico.ID_Medico AND NG.Codigo_Examen = E.Codigo_Examen;";

            String datos = "";

            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString() + "$";
                }
            }
            FinalizarComandoCorrectamente();
            return datos;
        }

        public static String buscarMuestraPorPaciente(String pNombre, String sNombre, String pApellido, String sApellido)
        {
            /*
            QueryString = "SELECT  Examen.Codigo_Examen, P_Nombre, S_nombre, P_Apellido, S_Apellido, Edad, Examen.Diagnostico, " +
                "Medico.Nombre, Precio, Expediente, Fecha_Examen, Fecha_Informe, Registrado_por_empleado " +
                "FROM ((SELECT * FROM Paciente WHERE P_Nombre = '" + nombres[0] + "' AND S_Nombre = '" + nombres[1] +
                "' AND P_Apellido = '" + nombres[2] + "' AND S_Apellido = '" + nombres[3] + "') " +
                " AS _Paciente JOIN Examen ON Examen.ID_Paciente = _Paciente.ID_paciente ) JOIN Medico ON Medico.ID_Medico = Examen.ID_Medico";
            */
            QueryString = "SELECT  Examen.Codigo_Examen, P_Nombre, S_nombre, P_Apellido, S_Apellido, Edad, Examen.Diagnostico, " +
                "Medico.Nombre, Precio, Expediente, Fecha_Examen, Fecha_Informe, Registrado_por_empleado " +
                "FROM ((SELECT * FROM Paciente WHERE P_Nombre = '" + pNombre + "'";

            if (!sNombre.Equals(""))
                QueryString += " AND S_Nombre = '" + sNombre + "'";

            if(!pApellido.Equals(""))
                QueryString += " AND P_Apellido = '" + pApellido + "'";

            if (!sNombre.Equals(""))
                QueryString += " AND S_Apellido = '" + sApellido + "'";

            QueryString += ") AS _Paciente JOIN Examen ON Examen.ID_Paciente = _Paciente.ID_paciente ) JOIN Medico ON Medico.ID_Medico = Examen.ID_Medico";

            String datos = "";

            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos += DataReader.GetValue(i).ToString() + ";";
                }
            }
            FinalizarComandoCorrectamente();
            return datos;
        }

        /******************************************************************************
         * *****************METODOS ESTANDARD DEL DT***********************************
         * ****************************************************************************/
        /*El metodo que debe retornar todo método para saber si todo se inserto en la base de datos, o para saber
         *si un select o update tubo efecto etc.*/
        public static bool EjecutarComando()
        {
            try
            {
                DataTransaction.Connection = new SqlConnection(DataTransaction.ConnectionString);
                DataTransaction.Connection.Open();
                SqlTransaction transaccion = Connection.BeginTransaction();
                DataTransaction.Query = Connection.CreateCommand();
                DataTransaction.Query.CommandType = System.Data.CommandType.Text;
                DataTransaction.Query.Transaction = transaccion;
                DataTransaction.Query.CommandText = QueryString;
                DataTransaction.DataReader = DataTransaction.Query.ExecuteReader();
                return true;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                try
                {
                    DataTransaction.Query.Transaction.Rollback();
                    DataTransaction.Connection.Close();
                    DataTransaction.Connection.Dispose();
                    Console.WriteLine("ERROR: TRANSACTION ROLLED BACK\n" + ex.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: TRANSACTION NOT ROLLED BACK\n" + e.Message);
                }

                DataTransaction.DataReader = null;
                DataTransaction.Connection = null;
                DataTransaction.Query = null;
                DataTransaction.QueryString = "";
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: NO SE QUE PASO...\n" + ex.Message);
                return false;
            }
        }
        /*Comete y desconecta de la base de datos*/
        public static void FinalizarComandoCorrectamente()
        {
            DataTransaction.DataReader.Dispose();
            DataTransaction.DataReader.Close();
            DataTransaction.Query.Transaction.Commit();
            DataTransaction.DataReader.Dispose();
            DataTransaction.DataReader.Close();
            DataTransaction.Query.Connection.Dispose();
            DataTransaction.Query.Dispose();
            DataTransaction.Query.Connection.Close();
            DataTransaction.Connection.Dispose();
            DataTransaction.Connection.Close();
            DataTransaction.DataReader = null;
            DataTransaction.Connection = null;
            DataTransaction.Query = null;
            DataTransaction.QueryString = "";
        }
        /*Convierte la fecha del sistema a formato de BD con el formato: AÑO-MES-DIA*/
        public static string convertirFechaBD(string fecha)
        {
            string[] fecha_corregida = fecha.Split('/');
            fecha = "";

            fecha += fecha_corregida[2].Split(' ')[0] + "-";
            fecha += fecha_corregida[0] + "-";
            fecha += fecha_corregida[1];

            return fecha;
        }
        /*regresa el ultimo dia del mes*/
        public static String getDiaFinal(int Mes)
        {
            switch (Mes)
            {
                case 2:
                    return "28";
                case 4:
                    return "30";
                case 6:
                    return "30";
                case 9:
                    return "30";
                case 11:
                    return "30";
                default:
                    return "31";
            }
        }
    }
}
