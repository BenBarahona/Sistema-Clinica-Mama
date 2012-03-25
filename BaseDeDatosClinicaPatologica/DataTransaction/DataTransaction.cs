using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DT
{


    public class DataTransaction
    {

        //Campos de la clase Data Transaction

        //Base de Datos FINAL
        private string MyConString = "Server=odessapatlab.db.6949030.hostedresource.com; Port=3306; Database=odessapatlab; Uid=odessapatlab; Pwd=Odessa58; Allow Zero Datetime=true";

        //Base de Datos de PRUEBA
        //private string MyConString = "Server=patlabprueba.db.6949030.hostedresource.com; Port=3306; Database=patlabprueba; Uid=patlabprueba; Pwd=Odessa58; Allow Zero Datetime=true";

        MySqlConnection Connection;
        MySqlCommand Command;
        MySqlDataReader Reader;

        /***************UTILIDADES**************************************************************/
        public bool seEjecutoComando()
        {
            try
            {
                Reader = Command.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }//End public bool seEjecutoComando()        

        public bool seEjecutoComando1(MySqlDataReader newReader, MySqlCommand newCommand)
        {
            try
            {
                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }//End public bool seEjecutoComando()

        public bool Conectar()
        {

            MySqlConnection mConnection = new MySqlConnection(MyConString);

            try
            {
                mConnection.Open();
            }
            catch (Exception ex)
            {
                return false;
            }

            if (mConnection.Ping())
            {

                //Command = Connection.CreateCommand();
                return true;

            }//End if (Connection.Ping())

            return false;

        }//End public void Conectar()

        public MySqlConnection Reconectar()
        {

            MySqlConnection myConnection = new MySqlConnection();

            return myConnection;

        }//End public MySqlConnection Conectar()

        public void Desconectar(MySqlConnection connectionDesc)
        {

            connectionDesc.Close();

        }//End public void Desconectar()

        public String Querry(String querry)
        {
            Connection.Close();
            Connection.Open();
            Command.CommandText = querry;
            if (seEjecutoComando())
            {
                String consulta = "";
                while (Reader.Read())
                {

                    for (int i = 0; i < Reader.FieldCount; i++)
                        consulta += Reader.GetValue(i).ToString() + ",";
                    consulta += "\n";
                }
                return consulta;
            }
            else
                return null;

        }//End Querry

        /**************************METODOS DE INSERTADO*********************************************/
        public bool InsertarMedico(String Nombre, String telefono,
            String celular)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("INSERT INTO Medico( `Nombre` , " +
                                "`telefono`,`celular` ) VALUES ('" + Nombre +
                                "', '" + telefono + "' ," + "'" + celular + "' );");


                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try


        }//End public void InsertarMedico(int ID_Medico, String Nombre, String telefono, String celular)

        public bool InsertarEmpleado(String nombre_usuario, String tipo, String P_nombre,
            String S_nombre, String P_apellido, String S_apellido, String Password)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("INSERT INTO Empleado( `Nombre_Usuario`,`Tipo_Empleado`,`P_Nombre`," +
                    "`S_Nombre`,`P_Apellido`,`S_Apellido`,`Password`)VALUES( '" +
                    nombre_usuario + "','" + tipo + "','" + P_nombre + "','" + S_nombre + "','" + P_apellido
                    + "','" + S_apellido + "','" + Password + "');");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try
        }//END public void InsertarEmpleado(String nombre_usuario, String tipo,...

        public bool InsertarModulo(String nombre_modulo, String descripcion)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("INSERT INTO Modulo(`Nombre_Modulo`,"
                + "`Descripcion`)VALUES( '"
                + nombre_modulo + "','" + descripcion + "' )");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try


        }//END public void InsertarModulo

        public bool InsertarAcceso(String nombre_modulo, String usuario)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("INSERT INTO Acceso(`Nombre_Modulo`,"
                + "`Nombre_Usuario`)VALUES('" + nombre_modulo + "','" + usuario + "');");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try


        }//END public void InsertarAcceso

        public bool InsertarContabilidad(String nombre_contable, int anio, int mes)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("INSERT INTO Contabilidad( `Nombre_contable`"
                + " ,`Anio_contable` ,`Mes_contable` )VALUES ( '" + nombre_contable + "' , " + anio.ToString() +
                " , " + mes.ToString() + ");");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try


        }//END public void InsertarContabilidad

        public bool InsertarEgreso(String nombre_contable, int anio, int mes, int totalEgreso)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("INSERT INTO Egreso(`Nombre_contable`,"
                + "`Anio_contable`,`Mes_contable`,`Total_Egreso`)VALUES( "
                + "'" + nombre_contable + "' , " + anio.ToString() + " , " + mes.ToString() + " , " + totalEgreso.ToString() + ");");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try


        }//END public void InsertarEgreso

        public bool InsertarGasto(String nombre_contable, int anio, int mes, int totalEgreso,
            String descripcion, String fechaexacta, int valor)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("INSERT INTO Gasto(`Nombre_contable`,"
                + "`Anio_contable`,`Mes_contable`,`Descripcion`,`Fecha_Exacta`,`Valor`)VALUES('"
                + nombre_contable + "' ," + anio.ToString() + " , " + mes.ToString() + " ," +
                " '" + descripcion + "' , '" + fechaexacta + "' , " + valor.ToString() + ");");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try


        }//END public void Insertargasto

        public bool InsertarMaterial(String material_enviado)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("INSERT INTO MaterialEnviadoCitologia_No_Ginecologica(`Material_Enviado`)"
                + "VALUES('" + material_enviado + "');");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try


        }//END public void InsertarMaterial

        public bool InsertarMaterialEnviado(String cod_examen, String material_enviado)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("INSERT INTO MaterialEnviadoBiopsia(`Codigo_Examen`,`Material_Enviado`)"
                + "VALUES('" + cod_examen + "','" + material_enviado + "');");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try


        }//END public void InsertarMaterial


        public bool InsertarPaciente(bool DIU, String fur, String fup, String pnombre,
            String snombre, String papellido, String sapellido, int edad, bool anticonceptivos, String Expediente)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("INSERT INTO Paciente( `DIU` ," +
                " `FUR` ,`FUP`, `P_Nombre`, `S_Nombre` , `P_Apellido` , `S_Apellido` , `Edad` , " +
                " `Anticonceptivos_Orales`,`Expediente` ) VALUES ( " +
                DIU.ToString() + " , '" + fur + "','" + fup +
                "' , '" + pnombre + "' , '" + snombre + "' ,  '" + papellido + "', '" + sapellido + "' , "
                + edad.ToString() + ", '" + anticonceptivos.ToString() + "' , '" + Expediente + "');");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();

                newConnection.Close();
                newReader.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try

        }//END public void InsertarPaciente

        public bool InsertarBiopsia(String cod_examen, String fecha, int precio, String Diagnostico,
            String diagnostico_medico, int Id_paciente, int id_medico, String Nombre_Contable,
            int anio_contable, int mes_contable, String usuario_empleado, String descripcion_macros,
            String descripcion_micros, String codificacion, String fechaInforme, String fechaIngresada)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlDataReader newReader = null;
                MySqlCommand newCommand = new MySqlCommand("INSERT INTO Examen(`Codigo_Examen`, "
                + "`Fecha_Examen`, `Precio` ,`Diagnostico`, `Diagnostico_Medico`, `ID_Paciente`,"
                + "`ID_Medico`,`Nombre_contable`,`Anio_contable`,`Mes_contable` ,"
                + " `Registrado_por_empleado`,`Fecha_Informe`,`Fecha` )VALUES( '" + cod_examen + "', '" + fecha + "', " + precio.ToString()
                + ",'" + Diagnostico + "', '" + diagnostico_medico + "'," + Id_paciente.ToString()
                + "," + id_medico.ToString() + ",'" + Nombre_Contable + "' , " + anio_contable.ToString()
                + " , " + mes_contable + " , '" + usuario_empleado + "' , '" + fechaInforme + "' , '" + fechaIngresada + "');");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                newReader = null;
                newReader = newCommand.ExecuteReader();
                newCommand = null;
                newConnection.Close();

                newConnection = Reconectar();
                newCommand = new MySqlCommand("INSERT INTO Biopsia(`Codigo_Examen`,"
                    + "`Descripcion_Macroscopica`,`Descripcion_Microscopica`,`Codificacion`) VALUES( '" + cod_examen + "','"
                    + descripcion_macros + "' , '" + descripcion_micros + "','" + codificacion + "');");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                newReader = null;

                newReader = newCommand.ExecuteReader();
                newConnection.Close();
                newReader.Close();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }//End try

        }//END public void InsertarBiopsia

        public bool InsertarCitologiaNoGinecologica(String cod_examen, String fecha, int precio, String Diagnostico,
            String diagnostico_medico, int Id_paciente, int id_medico, String Nombre_Contable,
            int anio_contable, int mes_contable, String usuario_empleado, String descripcion_macros,
            String descripcion_micros, String comentario, String fechaInforme, String fechaIngresada)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("INSERT INTO Examen (`Codigo_Examen`, "
                + "`Fecha_Examen`, `Precio` ,`Diagnostico`, `Diagnostico_Medico`, `ID_Paciente`,"
                + "`ID_Medico`,`Nombre_contable`,`Anio_contable`,`Mes_contable` ,"
                + " `Registrado_por_empleado`,`Fecha_Informe`,`Fecha` )VALUES( '" + cod_examen + "', '" + fecha + "', " + precio.ToString()
                + ",'" + Diagnostico + "', '" + diagnostico_medico + "'," + Id_paciente.ToString()
                + "," + id_medico.ToString() + ",'" + Nombre_Contable + "' , " + anio_contable.ToString()
                + " , " + mes_contable + " , '" + usuario_empleado + "' , '" + fechaInforme + "' , '" + fechaIngresada + "');");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                newConnection.Close();
                newCommand = null;
                newConnection = Reconectar();
                newCommand = new MySqlCommand("INSERT INTO Citologia(`Codigo_Examen`,"
                    + "`Comentario`) VALUES( '" + cod_examen + "','"
                    + comentario + "');");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;

                try
                {
                    newReader = newCommand.ExecuteReader();
                    newConnection.Close();
                    newCommand = null;
                    newConnection = Reconectar();
                    newCommand = new MySqlCommand("INSERT INTO No_Ginecologica(`Codigo_Examen`,"
                        + "`Descripcion_Macroscopica`,`Descripcion_Microscopica`) VALUES( '" + cod_examen + "','"
                        + descripcion_macros + "' , '" + descripcion_micros + "');");
                    newCommand.Connection = newConnection;
                    newConnection.ConnectionString = MyConString;
                    newConnection.Open();
                    newCommand.Connection = newConnection;
                    newReader = null;
                    newReader = newCommand.ExecuteReader();
                    newConnection.Close();
                    newCommand = null;
                    newReader.Close();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }//End try

            }
            catch (Exception ex)
            {
                return false;
            }//End try


        }//END public void InsertarNo_Ginecologica

        public bool InsertarCitologiaGinecologica(String cod_examen, String fecha, int precio, String Diagnostico,
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
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlDataReader newReader;
                MySqlCommand newCommand = new MySqlCommand("INSERT INTO Examen (`Codigo_Examen`, "
                + "`Fecha_Examen`, `Precio` ,`Diagnostico`, `Diagnostico_Medico`, `ID_Paciente`,"
                + "`ID_Medico`,`Nombre_contable`,`Anio_contable`,`Mes_contable` ,"
                + " `Registrado_por_empleado`,`Fecha_Informe`,`Fecha` )VALUES( '" + cod_examen + "', '" + fecha + "', " + precio.ToString()
                + ",'" + Diagnostico + "', '" + diagnostico_medico + "'," + Id_paciente.ToString()
                + "," + id_medico.ToString() + ",'" + Nombre_Contable + "' , " + anio_contable.ToString()
                + " , " + mes_contable + " , '" + usuario_empleado + "' , '" + fechaInforme + "' , '" + fechaIngresada + "');");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;

                newReader = newCommand.ExecuteReader();
                //newConnection.Close();
                newCommand = null;
                newConnection.Close();
                newReader = null;

                newConnection = Reconectar();

                newCommand = new MySqlCommand("INSERT INTO Citologia(`Codigo_Examen`,"
                                              + "`Comentario`) VALUES( '" + cod_examen + "','"
                                              + comentario + "');");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                newReader = null;

                newReader = newCommand.ExecuteReader();
                //newConnection.Close();
                //newCommand = null;
                newConnection.Close();
                //newReader.Close();

                newConnection = Reconectar();

                newCommand = new MySqlCommand("INSERT INTO Ginecologica(`Codigo_Examen`," +
                "`Inflamacion` , `Calidad_del_Frotis_Causa` , `Calidad_del_Frotis_Adecuado` ,"
                + "`Anticonceptivos_Orales`, `Candida_SP` , `Agentes_Infecciosos_Gardnerella`,"
                + "`Agentes_Infecciosos_Vaginosis_Bacteriana` , `Agentes_Infecciosos_Herpes` ,"
                + "`Agentes_Infecciosos_Tricomonas`, `Agentes_Infecciosos_Otro` ,"
                + "`Evaluacion_Hormonal_Basales` , `Evaluacion_Hormonal_Intermedias` ,"
                + "`Evaluacion_Hormonal_Superficiales`,`Recomendaciones_Colposcopia`,"
                + "`Recomendaciones_Repetir`,`Recomendaciones_Otra`,`Recomendaciones_Biopsia`,"
                + "`Recomendaciones__tratamiento`, `Diagnostico_NIC_I` , `Diagnostico_NIC_II`,"
                + "`Diagnostico_NIC_III`,`Origen_muestra`,`Diagnostico_Negativo_por_malignidad`,"
                + "`Diagnostico_Infecciones_VPH`,`Diagnostico_Glandular`, `Diagnostico_Escamosa`,"
                + "`Diagnostico_Adenocarcinoma`,`Diagnostico_Carcinoma_Celula`, `Celula_Atipica` , "
                + "`LesionEscamosa_BajoGrado` , `LesionEscamosa_AltoGrado`) "
                + "VALUES( '" + cod_examen + "','" + inflamacion + "','" + calidadFrotis_causa + "'," +
                calidadFrotisAdecuado.ToString() + "," + anticonceptivos.ToString() + "," +
                candida_sp.ToString() + "," + gardnerela.ToString() + "," + vaginosis.ToString()
                + "," + herpes.ToString() + "," + tricomonas.ToString() + ",'" + otroAgenteInfeccioso
                + "'," + evaluacionHormonal_basales.ToString() + "," + evaluacionhormonal_intermedias.ToString()
                + "," + evaluacionhormonal_superficiales.ToString() + "," + Colposcopia + "," +
                repetir.ToString() + ",'" + recomendaciones_otra + "'," + recomendaciones_biopsia.ToString() +
                "," + recomendaciones_tratamientos + "," + NIC_I.ToString() +
                "," + NIC_II.ToString() + "," + NIC_III.ToString() + ",'" + origen_muestra + "'," +
                negativo.ToString() + "," + VPH.ToString() + "," + glandular.ToString() + "," + escamoza.ToString() +
                "," + adenocarcinomana.ToString() + "," + carcinomana_celula.ToString() + "," + celula_atipica.ToString()
                + " , " + lesion_escamosa_AltoGrado.ToString() + " , " + lesion_escamosa_BajoGrado.ToString() + ");");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                newReader = null;

                newReader = newCommand.ExecuteReader();

                newConnection.Close();
                newReader.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try

        }//END public void InsertarGinecologica


        /***********METODOS GETS***************************************************************/


        public int getIDdeMedico(String nombre_Medico)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT ID_Medico FROM Medico WHERE " +
                    " Medico.Nombre = '" + nombre_Medico + "';");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String id = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        id += newReader.GetValue(i).ToString();
                }
                newConnection.Close();
                newReader.Close();
                return Convert.ToInt16(id);

            }
            catch (Exception ex)
            {
                return -1;
            }//End try

        }// End public int getIDdeMedico(String nombre_Medico)

        public int getIDdePaciente(String nombre_Paciente, String S_nombre_Paciente, String apellido_paciente, String S_apellido_Paciente)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT ID_paciente FROM Paciente WHERE " +
                    " Paciente.P_Nombre = '" + nombre_Paciente + "' AND "
                    + "Paciente.P_Apellido = '" + apellido_paciente + "' AND Paciente.S_Nombre = '" + S_nombre_Paciente + "' "
                    + " AND Paciente.S_Apellido = '" + S_apellido_Paciente + "';");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String id = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    if (newReader.FieldCount != 0)
                        id += newReader.GetValue(0).ToString();
                }
                newConnection.Close();
                newReader.Close();
                return Convert.ToInt16(id);

            }
            catch (Exception ex)
            {
                return -1;
            }//End try

        }// End public int getIDdeMedico(String nombre_Medico)

        public String getPaciente(int Id_paciente)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT * FROM Paciente WHERE " +
                    " Paciente.ID_paciente = " + Id_paciente + ";");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String paciente = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        paciente += newReader.GetValue(i).ToString();
                }
                return paciente;

            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }//End try
        }//End public String getPaciente(int Id_paciente)

        public String getExamen(String codigo_Examen)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT * FROM Examen WHERE " +
                    " Examen.codigo_Examen = '" + codigo_Examen + "';");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String examen = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        examen += newReader.GetValue(i).ToString();
                }
                return examen;

            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }//End try        
        }//End public String getExamen(String codigo_Examen)

        public String getMedico(int id_medico)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT * FROM Medico WHERE " +
                    " Medico.ID_Medico = " + id_medico + ";");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String medico = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        medico += newReader.GetValue(i).ToString() + ";";
                }
                return medico;

            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }//End try        

        }//End public String getMedico(int id_medico)

        /******************************GET EMPLEADO**************************/
        public String getEmpleado(string usuario)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT * FROM Empleado WHERE " +
                    " Empleado.Nombre_Usuario = '" + usuario + "';");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String medico = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        medico += newReader.GetValue(i).ToString() + ";";
                }
                return medico;

            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }//End try        

        }//End public String getMedico(int id_medico)


        /**********GET todos los empleados**********/


        public ArrayList getEmpleados()
        {
            try
            {
                ArrayList medicos = new ArrayList();
                Connection = new MySqlConnection(MyConString);
                Connection.Open();
                Command = new MySqlCommand("SELECT Nombre_Usuario FROM Empleado;");
                Command.Connection = Connection;
                //Command.CommandText = "SELECT * FROM clinica_patologica.Medico";

                if (seEjecutoComando())
                {

                    while (Reader.Read())
                    {

                        String thisrow = "";
                        for (int i = 0; i < Reader.FieldCount; i++)
                            thisrow += Reader.GetValue(i).ToString() + ";";

                        medicos.Add(thisrow);
                    }//End while (Reader.Read())

                    Connection.Close();
                    return medicos;

                }//End if (seEjecutoComando())
            }
            catch (Exception ex)
            {
                return null;
            }//End try
            return null;
        }

        /*************************************METODOS UPDATE*********************************************/
        public bool ActualizarCitologia(String cod_examen, String Diagnostico, String comentario,
            String inflamacion, String calidadFrotis_causa, bool calidadFrotisAdecuado,
            bool candida_sp, bool gardnerela, bool vaginosis, bool herpes, bool tricomonas,
            String otroAgenteInfeccioso, int evaluacionHormonal_basales,
            int evaluacionhormonal_intermedias, int evaluacionhormonal_superficiales, bool Colposcopia,
            int repetir, String recomendaciones_otra, bool recomendaciones_biopsia, bool recomendaciones_tratamientos,
            bool NIC_I, bool NIC_II, bool NIC_III, String origen_muestra, bool negativo, bool VPH,
            bool glandular, bool escamosa, bool adenocarcinomana, bool carcinomana_celula, bool celula_atipica,
            bool lesion_escamosa_AltoGrado, bool lesion_escamosa_BajoGrado)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("UPDATE Ginecologica, Examen, Citologia " +
                "SET Calidad_del_Frotis_Adecuado = " + calidadFrotisAdecuado + ", Calidad_del_Frotis_Causa = '" + calidadFrotis_causa +"', " + 
                "Evaluacion_Hormonal_Basales = " + evaluacionHormonal_basales + ", Evaluacion_Hormonal_Intermedias = " + evaluacionhormonal_intermedias + 
                ", Evaluacion_Hormonal_Superficiales = " + evaluacionhormonal_superficiales + ", Inflamacion = '" + inflamacion + "', " +
                "Candida_SP = " + candida_sp + ", Agentes_Infecciosos_Gardnerella = " + gardnerela + ", Agentes_Infecciosos_Vaginosis_Bacteriana = " + vaginosis + "," +
                "Agentes_Infecciosos_Herpes = " + herpes + ", Agentes_Infecciosos_Tricomonas = " + tricomonas + 
                ", Agentes_Infecciosos_Otro = '" + otroAgenteInfeccioso + "', Diagnostico_Negativo_por_malignidad = " + negativo + 
                ", Celula_Atipica = " + celula_atipica + ", Diagnostico_Escamosa = " + escamosa + ", Diagnostico_Glandular = " + glandular + "," +
                " LesionEscamosa_BajoGrado = " + lesion_escamosa_BajoGrado + ", Diagnostico_NIC_I = " + NIC_I + ", Diagnostico_Infecciones_VPH = " + VPH + "," +
                " LesionEscamosa_AltoGrado = " + lesion_escamosa_AltoGrado + ", Diagnostico_NIC_II = " + NIC_II + ", Diagnostico_NIC_III = " + NIC_III +"," +
                "Diagnostico_Carcinoma_Celula = " + carcinomana_celula + ", Diagnostico_Adenocarcinoma = " + adenocarcinomana + ", Recomendaciones_Repetir = " + repetir +
                ", Recomendaciones_Colposcopia = " + Colposcopia + ", Recomendaciones_Biopsia = " + recomendaciones_biopsia + 
                ", Recomendaciones__tratamiento = " + recomendaciones_tratamientos + ", Recomendaciones_Otra = '" + recomendaciones_otra + "', " + 
                "Diagnostico = '" + Diagnostico + "', Comentario = '" + comentario + "' " + 
                "WHERE Ginecologica.Codigo_Examen = '" + cod_examen + "' AND Examen.Codigo_Examen = '" + cod_examen + 
                "' AND Citologia.Codigo_Examen = '" + cod_examen + "';");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try        

        }//End public bool ActualizarCitologia


        public bool ActualizarBiopsia(String cod_examen, String macroscopica, String microscopica, String codificacion, String diagnostico)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("UPDATE Biopsia, Examen " +
                "SET Descripcion_Macroscopica = '" + macroscopica + "', Descripcion_Microscopica = '" + microscopica + "', " +
                "Codificacion = '" + codificacion + "', Diagnostico = '" + diagnostico + "' " +
                "WHERE Biopsia.Codigo_Examen = '" + cod_examen + "' AND Examen.Codigo_Examen = '" + cod_examen + "';");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try        

        }//End public bool ActualizarBiopsia

        public bool ActualizarCitologiaLiquidos(String cod_examen, String macroscopica, String microscopica, String diagnostico)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("UPDATE No_Ginecologica, Examen " +
                "SET Descripcion_Macroscopica = '" + macroscopica + "', Descripcion_Microscopica = '" + microscopica + "', " +
                "Diagnostico = '" + diagnostico + "' " +
                "WHERE No_Ginecologica.Codigo_Examen = '" + cod_examen + "' AND Examen.Codigo_Examen = '" + cod_examen + "';");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try        

        }//End public bool ActualizarCitologiaLiquidos

        public bool ActualizarMedico(String nombre, String telefono, String celular)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("UPDATE Medico " +
                "SET telefono = '" + telefono + "', celular = '" + celular + "' " +
                "WHERE Nombre = '" + nombre + "';");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try        

        }//End public bool ActualizarMedico


        /*****METODOS PARA BORRAR**********************************************************************/
        public bool BorrarEmpleado(String usuario)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("DELETE FROM Empleado " +
                    "WHERE Empleado.Nombre_Usuario = '" + usuario + "'; " +
                  "DELETE FROM Acceso WHERE Nombre_Usuario = '" + usuario + "';");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try        

        }//End public bool BorrarEmpleado(String usuario)

        public bool BorrarMaterial(String material)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("DELETE FROM MaterialEnviadoCitologia_No_Ginecologica " +
                    "WHERE MaterialEnviadoCitologia_No_Ginecologica.Material_Enviado = '" + material + "'");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try        

        }//End public bool BorrarMaterial

        /***********************COMPROBRACION DE LOGIN*****************************************************/
        public bool esUsuarioValido(string usuario, string contrasena)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT Password FROM Empleado WHERE "
                + "Nombre_Usuario=" + "'" + usuario + "'");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                string password = null;

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        password += newReader.GetValue(i).ToString();

                }//End while (Reader.Read())

                return (contrasena == password);
            }
            catch (Exception ex)
            {
                return false;
            }//End try        

        }//End public bool esUsuarioValido(string usuario, string contrasena)

        /***********************INGRESOS POR MES*****************************************************/
        public ArrayList getIngresosPorMes(int Año, int Mes)
        {
            try
            {
                ArrayList ingresos = new ArrayList();

                Connection.Close();
                Connection.Open();
                Command.CommandText = "SELECT * FROM clinica_patologica.Ingreso WHERE Anio_contable="
                + Año + " AND " + "Mes_contable=" + Mes;

                if (seEjecutoComando())
                {

                    while (Reader.Read())
                    {

                        String thisrow = "";
                        for (int i = 0; i < Reader.FieldCount; i++)
                            thisrow += Reader.GetValue(i).ToString() + ";";

                        ingresos.Add(thisrow);

                    }

                    Connection.Close();
                    return ingresos;

                }//End if (seEjecutoComando())
            }
            catch (Exception ex)
            {
                return null;
            }//End try
            return null;

        }//End getIngresosPorMes(Año,Mes)

        /***********************EGRESOS POR MES*****************************************************/
        public string getEgresosPorMes(int Anio, int Mes)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT Nombre_contable FROM Egreso WHERE Anio_contable="
                + Anio + " AND " + "Mes_contable=" + Mes + ";");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String resp = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        resp += newReader.GetValue(i).ToString() + ";";
                }

                return resp;
            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }//End try 


        }//End getEgresosPorMes(Año,Mes)

        /***********************CITOLOGIAS GINECOLOGICAS*****************************************************/
        public ArrayList getCitologiasGinecologicas(int Año)
        {
            ArrayList ingresos = new ArrayList();

            Connection.Close();
            Connection.Open();
            Command.CommandText = "SELECT  clinica_patologica.Examen.Codigo_Examen, Inflamacion, Calidad_del_Frotis_Causa, " +
                                  "Calidad_del_Frotis_Adecuado, Anticonceptivos_Orales, Candida_SP, " +
                                  "Agentes_Infecciosos_Gardnerella, Agentes_Infecciosos_Vaginosis_Bacteriana, " +
                                  "Agentes_Infecciosos_Herpes, Agentes_Infecciosos_Tricomonas, " +
                                  "Agentes_Infecciosos_Otro, Evaluacion_Hormonal_Basales, " +
                                  "Evaluacion_Hormonal_Intermedias, Evaluacion_Hormonal_Superficiales, " +
                                  "Recomendaciones_Colposcopia, Recomendaciones_Repetir, " +
                                  "Recomendaciones_Otra, Recomendaciones_Biopsia, " +
                                  "Recomendaciones__tratamiento, Diagnostico_NIC_I, " +
                                  "Diagnostico_NIC_II, Diagnostico_NIC_III, " +
                                  "Origen_muestra, Diagnostico_Negativo_por_malignidad, " +
                                  "Diagnostico_Infecciones_VPH, Diagnostico_Glandular, " +
                                  "Diagnostico_Escamosa, Diagnostico_Adenocarcinoma, " +
                                  "Diagnostico_Carcinoma_Celula, Comentario " +
                                  "FROM clinica_patologica.Ginecologica, " +
                                  "clinica_patologica.Examen, clinica_patologica.Citologia " +
                                  "WHERE clinica_patologica.Examen." +
                                  "Codigo_Examen=clinica_patologica.Citologia.Codigo_Examen AND " +
                                  "clinica_patologica.Citologia.Codigo_Examen=" +
                                  "clinica_patologica.Ginecologica.Codigo_Examen AND " +
                                  "clinica_patologica.Examen.Anio_contable=" + Año;
            if (seEjecutoComando())
            {

                while (Reader.Read())
                {

                    string thisrow = "";
                    for (int i = 0; i < Reader.FieldCount; i++)
                        thisrow += Reader.GetValue(i).ToString() + ";";

                    ingresos.Add(thisrow);

                }//End while (Reader.Read())

                Connection.Close();
                return ingresos;

            }//End if (seEjecutoComando())

            return null;

        }//End public ArrayList getCitologiasGinecologicas()

        /***********************GET BIOPSIAS*****************************************************/
        public ArrayList getBiopsias(int Año)
        {

            ArrayList biopsias = new ArrayList();

            Connection.Close();
            Connection.Open();
            Command.CommandText = "SELECT * FROM clinica_patologica.Biopsia, " +
                                  "clinica_patologica.Examen WHERE clinica_patologica.Examen." +
                                  "Codigo_Examen=clinica_patologica.Biopsia.Codigo_Examen AND " +
                                  "Anio_contable=" + Año;

            if (seEjecutoComando())
            {

                while (Reader.Read())
                {

                    String thisrow = "";
                    for (int i = 0; i < Reader.FieldCount; i++)
                        thisrow += Reader.GetValue(i).ToString() + ";";

                    biopsias.Add(thisrow);

                }//End while (Reader.Read())

                Connection.Close();
                return biopsias;

            }//End if (seEjecutoComando())

            return null;

        }//End getBiopsias()

        /***********************GET PACIENTES*****************************************************/
        public ArrayList getPacientes()
        {

            ArrayList pacientes = new ArrayList();

            Connection.Close();
            Connection.Open();
            Command.CommandText = "SELECT * FROM clinica_patologica.Paciente";

            if (seEjecutoComando())
            {

                while (Reader.Read())
                {

                    String thisrow = "";
                    for (int i = 0; i < Reader.FieldCount; i++)
                        thisrow += Reader.GetValue(i).ToString() + ";";

                    pacientes.Add(thisrow);

                }//End while (Reader.Read())

                Connection.Close();
                return pacientes;

            }//End if (seEjecutoComando())

            return null;

        }//End getPacientes()

        /***********************GET MEDICOS*****************************************************/
        public ArrayList getMedicos()
        {

            ArrayList medicos = new ArrayList();
            Connection = new MySqlConnection(MyConString);
            Connection.Open();
            Command = new MySqlCommand("SELECT * FROM clinica_patologica.Medico");
            Command.Connection = Connection;
            //Command.CommandText = "SELECT * FROM clinica_patologica.Medico";

            if (seEjecutoComando())
            {

                while (Reader.Read())
                {

                    String thisrow = "";
                    for (int i = 0; i < Reader.FieldCount; i++)
                        thisrow += Reader.GetValue(i).ToString() + ";";

                    medicos.Add(thisrow);
                }//End while (Reader.Read())

                Connection.Close();
                return medicos;

            }//End if (seEjecutoComando())

            return null;

        }//End getMedicos()


        /***********************GET MATERIAL ENVIADO BIOPSIA*****************************************************/
        public ArrayList getMaterialEnviado_Biopsia()
        {

            ArrayList materialEnviado = new ArrayList();

            Connection.Close();
            Connection.Open();
            Command.CommandText = "SELECT * FROM clinica_patologica.MaterialEnviadoBiopsia";

            if (seEjecutoComando())
            {

                while (Reader.Read())
                {

                    String thisrow = "";
                    for (int i = 0; i < Reader.FieldCount; i++)
                        thisrow += Reader.GetValue(i).ToString() + ";";

                    materialEnviado.Add(thisrow);
                }//End while (Reader.Read())

                Connection.Close();
                return materialEnviado;

            }//End if (seEjecutoComando())

            return null;

        }//End getMaterialEnviado_Biopsia()

        /***********************GET MATERIAL ENVIADO CITOLOGIA NO GINECOLOGICA*****************************************************/
        public string getMaterialEnviado_CitologiaNoGinecologica()
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT DISTINCT Material_Enviado FROM MaterialEnviadoCitologia_No_Ginecologica ORDER BY Material_Enviado");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String materiales = "";
                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        materiales += newReader.GetValue(i).ToString() + ";";
                }
                return materiales;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }//End try     

        }//End getMaterialEnviado_CitologiaNoGinecologica()

        public string consultaMedicoBiopsia(int anio, int mes)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                String comando = "";

                comando += "SELECT Medico.Nombre, SUM(Examen.Precio), COUNT(*) FROM Examen,Medico,Biopsia " +
                "WHERE Examen.ID_Medico = Medico.ID_Medico AND Examen.Codigo_Examen = Biopsia.Codigo_Examen ";

                if (anio != 0)
                    comando += "AND Anio_contable = " + anio.ToString() + " ";
                if (mes != 0)
                    comando += "AND Mes_contable = " + mes.ToString() + " ";

                comando += " GROUP BY Medico.Nombre;";

                MySqlCommand newCommand = new MySqlCommand(comando);

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String medico = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        medico += newReader.GetValue(i).ToString() + ";";
                }
                return medico;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }//End try     
        }

        public string consultaMedicoCitologia(int anio, int mes)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                String comando = "";

                comando += "SELECT Medico.Nombre, SUM(Examen.Precio), COUNT(*) FROM Examen,Medico, Citologia " +
                "WHERE Examen.ID_Medico = Medico.ID_Medico AND Examen.Codigo_Examen = Citologia.Codigo_Examen ";
                if (anio != 0)
                    comando += "AND Anio_contable = " + anio.ToString() + " ";
                if (mes != 0)
                    comando += "AND Mes_contable = " + mes.ToString() + " ";

                comando += " GROUP BY Medico.Nombre;";

                MySqlCommand newCommand = new MySqlCommand(comando);

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String medico = "";
                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        medico += newReader.GetValue(i).ToString() + ";";
                }
                return medico;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }//End try     
        }


        /********************METODOS AGREGADOs**********************************/

        public string getAccesos(string Usuario)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT Nombre_Modulo FROM " +
                    "Acceso WHERE Nombre_Usuario = '" + Usuario + "';");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String medico = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        medico += newReader.GetValue(i).ToString() + ";";
                }
                return medico;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }//End try     
        }

        public int getCantidadDeExamenes(String tabla, String filtadoporedad, String fechainicial, String fechafinal, String filtroCategoria)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();

                String comando = "";

                if (tabla.CompareTo("Biopsia") == 0)
                    comando += "SELECT  COUNT(*) FROM Examen , Paciente, " + tabla + " WHERE " +
                        " Examen.ID_Paciente = Paciente.ID_paciente AND " + tabla +
                        ".Codigo_Examen = Examen.Codigo_Examen ";
                else
                    comando += "SELECT  COUNT(*) FROM Examen , Paciente, " + tabla + ", Ginecologica WHERE " +
                        " Examen.ID_Paciente = Paciente.ID_paciente AND " + tabla +
                        ".Codigo_Examen = Examen.Codigo_Examen AND " +
                        "Ginecologica.Codigo_Examen = Examen.Codigo_Examen";

                if (filtadoporedad.CompareTo("Ninguna") != 0)
                {
                    if (filtadoporedad.CompareTo("< 20 Años") == 0)
                        comando += " AND Paciente.Edad < 20 ";
                    if (filtadoporedad.CompareTo("20 - 30 Años") == 0)
                        comando += " AND Paciente.Edad BETWEEN 20 AND 30 = 1 ";
                    if (filtadoporedad.CompareTo("30 - 40 Años") == 0)
                        comando += " AND Paciente.Edad BETWEEN 30 AND 40 = 1 ";
                    if (filtadoporedad.CompareTo("40 - 50 Años") == 0)
                        comando += " AND Paciente.Edad BETWEEN 40 AND 50 = 1 ";
                    if (filtadoporedad.CompareTo("> 60 Años") == 0)
                        comando += " AND Paciente.Edad > 60 ";
                }

                if (filtroCategoria.CompareTo("Ninguna") != 0)
                {
                    if (filtroCategoria.CompareTo("Adecuados") == 0)
                        comando += " AND Calidad_del_Frotis_Adecuado = 1";
                    if (filtroCategoria.CompareTo("Inadecuados") == 0)
                        comando += " AND Calidad_del_Frotis_Adecuado = 0";
                    if (filtroCategoria.CompareTo("Negativo por Malignidad") == 0)
                        comando += " AND Diagnostico_Negativo_por_malignidad = 1";
                    if (filtroCategoria.CompareTo("Lesion de Alto Grado") == 0)
                        comando += " AND LesionEscamosa_AltoGrado = 1";
                    if (filtroCategoria.CompareTo("Lesion de Bajo Grado") == 0)
                        comando += " AND LesionEscamosa_BajoGrado = 1";
                    if (filtroCategoria.CompareTo("Cancer") == 0)
                        comando += " AND Diagnostico_Carcinoma_Celula = 1";

                }

                if (fechainicial.CompareTo("") != 0)
                    comando += " AND Examen.Fecha_Examen > '" + fechainicial + "' ";
                if (fechafinal.CompareTo("") != 0)
                    comando += " AND Examen.Fecha_Examen < '" + fechafinal + "' ";


                MySqlCommand newCommand = new MySqlCommand(comando);

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String medico = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        medico += newReader.GetValue(i).ToString();
                }
                return Convert.ToInt16(medico);

            }
            catch (Exception ex)
            {
                return -1;
            }//End try     
        }

        public string getExamenesFiltrados(String tabla, String filtadoporedad, String fechainicial, String fechafinal, String filtroCategoria)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();

                String comando = "";
        
                if (tabla.CompareTo("Biopsia") == 0)
                    comando += "SELECT  Examen.Codigo_Examen, P_Nombre, S_nombre, P_Apellido, S_Apellido, Edad, Expediente, Medico.Nombre, Precio, Fecha_Informe, Fecha_Examen, Registrado_por_empleado" +
                        " FROM Examen, Paciente, Medico, " + tabla + 
                        " WHERE Examen.ID_Paciente = Paciente.ID_paciente AND Medico.ID_Medico = Examen.ID_Medico AND " + tabla + ".Codigo_Examen = Examen.Codigo_Examen ";
                else
                    comando += "SELECT  Examen.Codigo_Examen, P_Nombre, S_nombre, P_Apellido, S_Apellido, Edad, Expediente, Medico.Nombre, Precio, Fecha_Informe, Fecha_Examen, Registrado_por_empleado" +
                        " FROM Examen, Paciente, " + tabla + ", Ginecologica, Medico " +
                        " WHERE Examen.ID_Paciente = Paciente.ID_paciente AND Medico.ID_Medico = Examen.ID_Medico AND  " + tabla +
                        ".Codigo_Examen = Examen.Codigo_Examen AND " +
                        "Ginecologica.Codigo_Examen = Examen.Codigo_Examen";

                if (filtadoporedad.CompareTo("Ninguna") != 0)
                {
                    if (filtadoporedad.CompareTo("< 20 Años") == 0)
                        comando += " AND Paciente.Edad < 20 ";
                    if (filtadoporedad.CompareTo("20 - 30 Años") == 0)
                        comando += " AND Paciente.Edad BETWEEN 20 AND 30 = 1 ";
                    if (filtadoporedad.CompareTo("30 - 40 Años") == 0)
                        comando += " AND Paciente.Edad BETWEEN 30 AND 40 = 1 ";
                    if (filtadoporedad.CompareTo("40 - 50 Años") == 0)
                        comando += " AND Paciente.Edad BETWEEN 40 AND 50 = 1 ";
                    if (filtadoporedad.CompareTo("> 60 Años") == 0)
                        comando += " AND Paciente.Edad > 60 ";
                }

                if (filtroCategoria.CompareTo("Ninguna") != 0)
                {
                    if (filtroCategoria.CompareTo("Adecuados") == 0)
                        comando += " AND Calidad_del_Frotis_Adecuado = 1";
                    if (filtroCategoria.CompareTo("Inadecuados") == 0)
                        comando += " AND Calidad_del_Frotis_Adecuado = 0";
                    if (filtroCategoria.CompareTo("Negativo por Malignidad") == 0)
                        comando += " AND Diagnostico_Negativo_por_malignidad = 1";
                    if (filtroCategoria.CompareTo("Lesion de Alto Grado") == 0)
                        comando += " AND LesionEscamosa_AltoGrado = 1";
                    if (filtroCategoria.CompareTo("Lesion de Bajo Grado") == 0)
                        comando += " AND LesionEscamosa_BajoGrado = 1";
                    if (filtroCategoria.CompareTo("Cancer") == 0)
                        comando += " AND Diagnostico_Carcinoma_Celula = 1";

                }

                if (fechainicial.CompareTo("") != 0)
                    comando += " AND Examen.Fecha_Examen > '" + fechainicial + "' ";
                if (fechafinal.CompareTo("") != 0)
                    comando += " AND Examen.Fecha_Examen < '" + fechafinal + "' ";


                MySqlCommand newCommand = new MySqlCommand(comando);

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String resultado = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        resultado += newReader.GetValue(i).ToString() + ";";
                }
                return resultado;

            }
            catch (Exception ex)
            {
                return "";
            }//End try     
        }



        public ArrayList getMedicos_Nombres()
        {
            try
            {
                ArrayList medicos = new ArrayList();
                Connection = new MySqlConnection(MyConString);
                Connection.Open();
                Command = new MySqlCommand("SELECT  Medico.Nombre FROM Medico ORDER BY Nombre;");
                Command.Connection = Connection;


                if (seEjecutoComando())
                {

                    while (Reader.Read())
                    {

                        String thisrow = "";
                        for (int i = 0; i < Reader.FieldCount; i++)
                            thisrow += Reader.GetValue(i).ToString() + ";";

                        medicos.Add(thisrow);
                    }//End while (Reader.Read())

                    Connection.Close();
                    return medicos;

                }//End if (seEjecutoComando())

            }
            catch (Exception ex)
            {
                return null;
            }//End try     
            return null;

        }

        /*************************************METODOS BORRAR*********************************************/
        public bool BorrarMedico(String nombre)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("DELETE FROM Medico " +
                    "WHERE Medico.Nombre = '" + nombre + "'");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try        

        }//End public bool BorrarEmpleado(String usuario)

        public bool BorrarMuestra(String codigo)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("DELETE FROM Examen " +
                    "WHERE Codigo_Examen = '" + codigo + "' ; DELETE FROM Citologia " +
                    "WHERE Codigo_Examen = '" + codigo + "' ; DELETE FROM Ginecologica " +
                    "WHERE Codigo_Examen = '" + codigo + "' ; DELETE FROM No_Ginecologica " +
                    "WHERE Codigo_Examen = '" + codigo + "' ; DELETE FROM Biopsia " +
                    "WHERE Codigo_Examen = '" + codigo + "' ; DELETE FROM MaterialEnviadoBiopsia " + 
                    "WHERE Codigo_Examen = '" + codigo + "' ;");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try        

        }//End public bool BorrarMuestra(String codigo)


        /*************************************NUEVOS METODOS PARA CONTABILIDAD*********************************************/

        public bool VerificaryActualizarContabilidad()
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT * FROM Contabilidad WHERE Anio_contable = "
                + "YEAR( CURRENT_DATE() ) AND Mes_contable = MONTH( CURRENT_DATE() );");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String resp = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        resp += newReader.GetValue(i).ToString() + ";";
                }

                if (resp.CompareTo("") == 0)
                {
                    return CrearContabilidad();
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                return false;
            }//End try 
        }

        private bool CrearContabilidad()
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("INSERT INTO Contabilidad( Nombre_contable , Anio_contable , Mes_contable ) " +
                                "VALUES( 'Ingresos por Examen' , YEAR( CURRENT_DATE() ) , MONTH( CURRENT_DATE() ) );" +
                                "INSERT INTO Ingreso( Nombre_contable , Anio_contable , Mes_contable , Total_Ingreso)" +
                                "VALUES( 'Ingresos por Examen' , YEAR( CURRENT_DATE() ) , MONTH( CURRENT_DATE() ) , 0 );");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String resp = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        resp += newReader.GetValue(i).ToString() + ";";
                }

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }//End try 
        }

        public bool InsertarGastoenEgreso(String NombreContable, int anio_contable, int mes_contable, String descripcion, int dia, int valor)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("INSERT INTO Gasto( Nombre_contable , Anio_contable, " +
                    "Mes_contable , Descripcion , Fecha_Exacta , Valor ) VALUES( '" + NombreContable + "' , " +
                    anio_contable.ToString() + " , " + mes_contable.ToString() + "  , '" + descripcion +
                    "' , '" + anio_contable.ToString() + "-" + mes_contable.ToString() + "-" + dia.ToString() + "', " +
                    valor.ToString() + " );");


                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;

                newReader = newCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }//End try


        }//End public void InsertarMedico(int ID_Medico, String Nombre, String telefono, String celular)

        /**************************************************************************
 **************************************************************************
 **************************BUSCAR (SELECTS)********************************
 **************************************************************************
 **************************************************************************/

        public String getTodosLosGastos(int anio, int mes, String categoria)
        {

            try
            {
                MySqlConnection newConnection = Reconectar();
                String comando = "";

                comando += "SELECT Nombre_contable, Descripcion , Valor FROM Gasto " +
                           "WHERE Anio_contable = " + anio.ToString() + " ";
                if (mes != 0)
                    comando += "AND Mes_contable = " + mes.ToString() + " ";
                if (categoria.CompareTo("Ninguna") != 0)
                {
                    comando += " AND Nombre_contable = '" + categoria.ToString() + "'";
                }

                comando += " ORDER BY Nombre_contable;";

                MySqlCommand newCommand = new MySqlCommand(comando);

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String thisRow = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        thisRow += newReader.GetValue(i).ToString() + ";";
                }
                return thisRow;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }//End try 

        }

        /*************************************FIN NUEVOS METODOS PARA CONTABILIDAD*********************************************/

        //Ingresos
        public String getIngresosCitologia(String fechainicial, String fechafinal)
        {
            try
            {
                Connection = new MySqlConnection(MyConString);
                Connection.Open();
                Command = new MySqlCommand("SELECT SUM(Precio) FROM Examen, Citologia WHERE Examen.Codigo_Examen = Citologia.Codigo_Examen " +
                "AND Examen.Fecha > " + "'" + fechainicial + "'" + " AND Examen.Fecha < " + "'" + fechafinal + "';");

                Command.Connection = Connection;

                string resp = "";
                if (seEjecutoComando())
                {

                    while (Reader.Read())
                    {

                        String thisrow = "";
                        for (int i = 0; i < Reader.FieldCount; i++)
                            thisrow += Reader.GetValue(i).ToString();

                        resp += thisrow;
                    }//End while (Reader.Read())

                    Connection.Close();
                    return resp;

                }//End if (seEjecutoComando())

            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }//End try     
            return "";

        }

        public String getIngresosBiopsia(String fechainicial, String fechafinal)
        {
            try
            {
                Connection = new MySqlConnection(MyConString);
                Connection.Open();
                Command = new MySqlCommand("SELECT SUM(Precio) FROM Examen, Biopsia WHERE Examen.Codigo_Examen = Biopsia.Codigo_Examen " +
                "AND Examen.Fecha > " + "'" + fechainicial + "'" + " AND Examen.Fecha < " + "'" + fechafinal + "';");

                Command.Connection = Connection;

                string resp = "";
                if (seEjecutoComando())
                {

                    while (Reader.Read())
                    {

                        String thisrow = "";
                        for (int i = 0; i < Reader.FieldCount; i++)
                            thisrow += Reader.GetValue(i).ToString();

                        resp += thisrow;
                    }//End while (Reader.Read())

                    Connection.Close();
                    return resp;

                }//End if (seEjecutoComando())

            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }//End try     
            return "";
        }//End getIngresosBiopsia


        /*******************************************METODOS GET TODOS LOS ATRIBUTOS PARA IMPRIMIR************************************************************/
        public String getMuestraGinecologica(String idMuestra)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT E.Codigo_Examen, E.Fecha_Examen, Paciente.P_Nombre, Paciente.S_Nombre, Paciente.P_Apellido, " +
                    "Paciente.S_Apellido, Paciente.Edad, Medico.Nombre, G.Origen_muestra, Paciente.FUR, " +
                    "Paciente.DIU, G.Anticonceptivos_Orales, E.Diagnostico_Medico, G.Calidad_del_Frotis_Adecuado, " +
                    "G.Calidad_del_Frotis_Causa, G.Evaluacion_Hormonal_Basales, G.Evaluacion_Hormonal_Intermedias, " +
                    "G.Evaluacion_Hormonal_Superficiales, G.Inflamacion, G.Candida_SP, " +
                    "G.Agentes_Infecciosos_Gardnerella, G.Agentes_Infecciosos_Herpes, " +
                    "G.Agentes_Infecciosos_Vaginosis_Bacteriana, G.Agentes_Infecciosos_Tricomonas, " +
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
            " WHERE E.ID_Paciente = Paciente.ID_paciente AND E.ID_Medico = Medico.ID_Medico AND C.Codigo_Examen = E.Codigo_Examen;");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String datos = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        datos += newReader.GetValue(i).ToString() + ";";
                }
                return datos;

            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }//End try        

        }//End public String getMuestraGinecologica

        public String getMuestraNo_Ginecologica(String idMuestra)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT E.Codigo_Examen, E.Fecha_Examen, Paciente.P_Nombre, Paciente.S_Nombre, " +
                    "Paciente.P_Apellido, Paciente.S_Apellido, Paciente.Edad, E.Diagnostico_Medico, " +
                    "Medico.Nombre, NG.Descripcion_Macroscopica, " +
                    "NG.Descripcion_Microscopica, E.Diagnostico, E.Fecha_Informe " +
                 " FROM (SELECT * FROM Examen WHERE Examen.Codigo_Examen = '" + idMuestra + "') AS E, Paciente, Medico, " +
                       "(SELECT * FROM No_Ginecologica WHERE No_Ginecologica.Codigo_Examen = '" + idMuestra + "') AS NG " +
                 " WHERE E.ID_Paciente = Paciente.ID_paciente AND E.ID_Medico = Medico.ID_Medico AND NG.Codigo_Examen = E.Codigo_Examen;");


                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String datos = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        datos += newReader.GetValue(i).ToString() + ";";
                }
                return datos;

            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }//End try        

        }//End public String getMuestraNo_Ginecologica

        public String getMuestraBiopsia(String idMuestra)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT E.Codigo_Examen, E.Fecha_Examen, Paciente.P_Nombre, Paciente.S_Nombre, " +
                    "Paciente.P_Apellido, Paciente.S_Apellido, Paciente.Edad, E.Diagnostico_Medico, " +
                    "Medico.Nombre, B.Codificacion, B.Descripcion_Macroscopica, B.Descripcion_Microscopica, E.Diagnostico, E.Fecha_Informe " +
                " FROM (SELECT * FROM Examen WHERE Examen.Codigo_Examen = '" + idMuestra + "') AS E, Paciente, Medico, " +
                       "(SELECT * FROM Biopsia WHERE Biopsia.Codigo_Examen = '" + idMuestra + "') AS B " +
                 " WHERE E.ID_Paciente = Paciente.ID_paciente AND E.ID_Medico = Medico.ID_Medico AND B.Codigo_Examen = E.Codigo_Examen;");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String datos = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        datos += newReader.GetValue(i).ToString() + ";";
                }
                return datos;

            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }//End try        

        }//End public String getMuestraBiopsia

        public String getMaterialEnviadoBiopsiaImprimir(String idMuestra)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT Material_Enviado " +
                    "FROM (SELECT * FROM Examen WHERE Examen.Codigo_Examen = '" + idMuestra + "') E " +
                    "INNER JOIN MaterialEnviadoBiopsia ON MaterialEnviadoBiopsia.Codigo_Examen = E.Codigo_Examen ");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String datos = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        datos += newReader.GetValue(i).ToString() + ";";
                }
                return datos;

            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }//End try        

        }//End public String getMaterialEnviadoBiopsiaImprimir

        public String getIdExamenes()
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT Codigo_Examen FROM Examen");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String examenes = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        examenes += newReader.GetValue(i).ToString() + ";";
                }
                return examenes;

            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }//End try        

        }//End public String getIdExamenes()


        public String buscarCitologia(String idMuestra)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT E.Codigo_Examen, E.Fecha_Examen, Paciente.P_Nombre, Paciente.S_Nombre, Paciente.P_Apellido, " +
                    "Paciente.S_Apellido, Medico.Nombre, Paciente.Edad, G.Origen_muestra, Paciente.FUP, Paciente.FUR, E.Precio, " +
                    "Paciente.DIU, G.Anticonceptivos_Orales, Paciente.Expediente, E.Diagnostico_Medico, G.Calidad_del_Frotis_Adecuado, " +
                    "G.Calidad_del_Frotis_Causa, G.Evaluacion_Hormonal_Basales, G.Evaluacion_Hormonal_Intermedias, " +
                    "G.Evaluacion_Hormonal_Superficiales, G.Inflamacion, G.Candida_SP, " +
                    "G.Agentes_Infecciosos_Gardnerella, G.Agentes_Infecciosos_Herpes, " +
                    "G.Agentes_Infecciosos_Vaginosis_Bacteriana, G.Agentes_Infecciosos_Tricomonas, " +
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
            " WHERE E.ID_Paciente = Paciente.ID_paciente AND E.ID_Medico = Medico.ID_Medico AND C.Codigo_Examen = E.Codigo_Examen;");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String datos = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        datos += newReader.GetValue(i).ToString() + ";";
                }
                return datos;

            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }//End try        

        }//End public String buscarCitologia

        public String buscarCitologiaLiquidos(String idMuestra)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT E.Codigo_Examen, E.Fecha_Examen, Paciente.P_Nombre, Paciente.S_Nombre, " +
                    "Paciente.P_Apellido, Paciente.S_Apellido, Paciente.Edad, Medico.Nombre, E.Precio, " +
                    "Paciente.Expediente, E.Diagnostico_Medico, NG.Descripcion_Macroscopica, " +
                    "NG.Descripcion_Microscopica, E.Diagnostico " +
                 " FROM (SELECT * FROM Examen WHERE Examen.Codigo_Examen = '" + idMuestra + "') AS E, Paciente, Medico, " +
                       "(SELECT * FROM No_Ginecologica WHERE No_Ginecologica.Codigo_Examen = '" + idMuestra + "') AS NG " +
                 " WHERE E.ID_Paciente = Paciente.ID_paciente AND E.ID_Medico = Medico.ID_Medico AND NG.Codigo_Examen = E.Codigo_Examen;");


                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String datos = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        datos += newReader.GetValue(i).ToString() + ";";
                }
                return datos;

            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }//End try        

        }//End public String buscarCitologiaLiquidos

        public String buscarBiopsia(String idMuestra)
        {
            try
            {
                MySqlConnection newConnection = Reconectar();
                MySqlCommand newCommand = new MySqlCommand("SELECT E.Codigo_Examen, E.Fecha_Examen, Paciente.P_Nombre, Paciente.S_Nombre, " +
                    "Paciente.P_Apellido, Paciente.S_Apellido, Paciente.Edad, Medico.Nombre, E.Precio, Paciente.Expediente, E.Diagnostico_Medico, " +
                    "B.Codificacion, B.Descripcion_Macroscopica, B.Descripcion_Microscopica, E.Diagnostico " +
                " FROM (SELECT * FROM Examen WHERE Examen.Codigo_Examen = '" + idMuestra + "') AS E, Paciente, Medico, " +
                       "(SELECT * FROM Biopsia WHERE Biopsia.Codigo_Examen = '" + idMuestra + "') AS B " +
                 " WHERE E.ID_Paciente = Paciente.ID_paciente AND E.ID_Medico = Medico.ID_Medico AND B.Codigo_Examen = E.Codigo_Examen;");

                newCommand.Connection = newConnection;
                newConnection.ConnectionString = MyConString;
                newConnection.Open();
                newCommand.Connection = newConnection;
                MySqlDataReader newReader;
                String datos = "";

                newReader = newCommand.ExecuteReader();
                while (newReader.Read())
                {

                    for (int i = 0; i < newReader.FieldCount; i++)
                        datos += newReader.GetValue(i).ToString() + ";";
                }
                return datos;

            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }//End try        

        }//End public String buscarBiopsia

    }//End public class DataTransaction

}//End namespace DT
