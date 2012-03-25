using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;

namespace BaseDeDatosClinicaPatologica.Web
{
    /// <summary>
    /// Summary description for clinicaPatologiaWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class clinicaPatologiaWebService : System.Web.Services.WebService
    {

        //Instancia de la clase Data Transaccion
        DT.DataTransaction myDt = new DT.DataTransaction();

        [WebMethod]
        public string HelloWorld(string Nombre, int MedicoID)
        {
            //DT myDt;
            DT.DataTransaction MyDt = new DT.DataTransaction();
            //ArrayList Aray;
            MyDt.Conectar();
            //Aray = MyDt.getMedicos();

            return "Hello World " + Nombre;
        }//End public string HelloWorld(string Nombre, int MedicoID)

        /***************UTILIDADES**************************************************************/
        [WebMethod]
        public bool seEjecutoComando()
        {

            bool respuesta = myDt.seEjecutoComando();
            return respuesta;

        }//End public bool seEjecutoComando()

        [WebMethod]
        public bool Conectar()
        {
            return myDt.Conectar();
        }//End public void Conectar()

        [WebMethod]
        public String Querry(String querry)
        {

            return myDt.Querry(querry);

        }//End public String Querry(String querry)

        /**************************METODOS DE INSERTADO*********************************************/
        [WebMethod]
        public bool InsertarMedico(String Nombre, String telefono, String celular)
        {
            return myDt.InsertarMedico(Nombre, telefono, celular);
        }//End public void InsertarMedico(int ID_Medico, String Nombre, String telefono, String celular)

        [WebMethod]
        public bool InsertarEmpleado(String nombre_usuario, String tipo, String P_nombre,
            String S_nombre, String P_apellido, String S_apellido, String Password)
        {

            return myDt.InsertarEmpleado(nombre_usuario, tipo, P_nombre,
                                         S_nombre, P_apellido, S_apellido, Password);
        }//END public void InsertarEmpleado(String nombre_usuario, String tipo,...

        [WebMethod]
        public bool InsertarModulo(String nombre_modulo, String descripcion)
        {
            return myDt.InsertarModulo(nombre_modulo, descripcion);
        }//END public void InsertarModulo

        [WebMethod]
        public bool InsertarAcceso(String nombre_modulo, String usuario)
        {
            return myDt.InsertarAcceso(nombre_modulo, usuario);
        }//END public void InsertarAcceso

        [WebMethod]
        public bool InsertarContabilidad(String nombre_contable, int anio, int mes)
        {
            return myDt.InsertarContabilidad(nombre_contable, anio, mes);
        }//END public void InsertarContabilidad

        [WebMethod]
        public bool InsertarEgreso(String nombre_contable, int anio, int mes, int totalEgreso)
        {
            return myDt.InsertarEgreso(nombre_contable, anio, mes, totalEgreso);
        }//END public void InsertarEgreso

        [WebMethod]
        public bool InsertarGasto(String nombre_contable, int anio, int mes, int totalEgreso,
            String descripcion, String fechaexacta, int valor)
        {
            return myDt.InsertarGasto(nombre_contable, anio, mes, totalEgreso,
                                      descripcion, fechaexacta, valor);
        }//END public void Insertargasto

        [WebMethod]
        public bool InsertarMaterial(String material_enviado)
        {
            return myDt.InsertarMaterial(material_enviado);
        }//END public void InsertarMaterial

        [WebMethod]
        public bool InsertarMaterialEnviado(String cod_examen,String material_enviado)
        {
            return myDt.InsertarMaterialEnviado(cod_examen, material_enviado);
        }//END public void InsertarMaterialEnviado
        
        [WebMethod]
        public bool InsertarPaciente(bool DIU, String fur, String fup, String pnombre,
            String snombre, String papellido, String sapellido, int edad, bool anticonceptivos, String expediente)
        {
            return myDt.InsertarPaciente(DIU, fur, fup, pnombre, snombre, papellido, sapellido, edad, anticonceptivos, expediente);
        }//END public void InsertarPaciente

        [WebMethod]
        public bool InsertarBiopsia(String cod_examen, String fecha, int precio, String Diagnostico,
            String diagnostico_medico, int Id_paciente, int id_medico, String Nombre_Contable,
            int anio_contable, int mes_contable, String usuario_empleado, String descripcion_macros,
            String descripcion_micros, String codificacion, String fechaHoy, String fechaHoySQL)
        {
            return myDt.InsertarBiopsia(cod_examen, fecha, precio, Diagnostico,
                                        diagnostico_medico, Id_paciente, id_medico, Nombre_Contable,
                                        anio_contable, mes_contable, usuario_empleado, descripcion_macros,
                                        descripcion_micros, codificacion, fechaHoy, fechaHoySQL);
        }//END public void InsertarBiopsia

        [WebMethod]
        public bool InsertarCitologiaNoGinecologica(String cod_examen, String fecha, int precio, String Diagnostico,
            String diagnostico_medico, int Id_paciente, int id_medico, String Nombre_Contable,
            int anio_contable, int mes_contable, String usuario_empleado, String descripcion_macros,
            String descripcion_micros, String comentario, String fechaHoy, String fechaHoySQL)
        {
            return myDt.InsertarCitologiaNoGinecologica(cod_examen, fecha, precio, Diagnostico,
                                                        diagnostico_medico, Id_paciente, id_medico, Nombre_Contable,
                                                        anio_contable, mes_contable, usuario_empleado, descripcion_macros,
                                                        descripcion_micros, comentario, fechaHoy, fechaHoySQL);
        }//END public void InsertarNo_Ginecologica

        [WebMethod]
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
            bool lesion_escamosa_AltoGrado, bool lesion_escamosa_BajoGrado, String fechaHoy, String fechaHoySQL)
        {
            return myDt.InsertarCitologiaGinecologica(cod_examen, fecha, precio, Diagnostico,
                                                      diagnostico_medico, Id_paciente, id_medico, Nombre_Contable,
                                                      anio_contable, mes_contable, usuario_empleado, comentario,
                                                      inflamacion, calidadFrotis_causa, calidadFrotisAdecuado, anticonceptivos,
                                                      candida_sp, gardnerela, vaginosis, herpes, tricomonas,
                                                      otroAgenteInfeccioso, evaluacionHormonal_basales,
                                                      evaluacionhormonal_intermedias, evaluacionhormonal_superficiales, Colposcopia,
                                                      repetir, recomendaciones_otra, recomendaciones_biopsia, recomendaciones_tratamientos,
                                                      NIC_I, NIC_II, NIC_III, origen_muestra, negativo, VPH,
                                                      glandular, escamoza, adenocarcinomana, carcinomana_celula , celula_atipica ,
                                                      lesion_escamosa_AltoGrado, lesion_escamosa_BajoGrado, fechaHoy, fechaHoySQL);
        }//END public void InsertarGinecologica

        /***********METODOS GETS***************************************************************/
        [WebMethod]
        public int getIDdeMedico(String nombre_Medico)
        {
            return myDt.getIDdeMedico(nombre_Medico);
        }// End public int getIDdeMedico(String nombre_Medico)

        [WebMethod]
        public int getIDdePaciente(String nombre_Paciente, String s_nombre_Paciente, String apellido_paciente, String s_apellido_paciente)
        {
            return myDt.getIDdePaciente(nombre_Paciente, s_nombre_Paciente, apellido_paciente, s_apellido_paciente);
        }// End public int getIDdeMedico(String nombre_Medico)

        [WebMethod]
        public String getPaciente(int Id_paciente)
        {
            return myDt.getPaciente(Id_paciente);
        }//End public String getPaciente(int Id_paciente)

        [WebMethod]
        public String getExamen(String codigo_Examen)
        {
            return myDt.getExamen(codigo_Examen);
        }//End public String getExamen(String codigo_Examen)

        [WebMethod]
        public String getMedico(int id_medico)
        {
            int i = 0;
            return myDt.getMedico(id_medico);
        }//End public String getMedico(int id_medico)

        [WebMethod]
        public String getEmpleado(String usuario)
        {
            return myDt.getEmpleado(usuario);
        }//End public String getEmpleado(String usuario)

        [WebMethod]
        public String getEmpleados()
        {
            String resultado = "";
            ArrayList resp = myDt.getEmpleados();

            for (int i = 0; i < resp.Count; i++)
                resultado += resp[i];
            return resultado;
        }//End public String getMedico(int id_medico)


        /*****METODOS ACTUALIZAR**********************************************************************/
        [WebMethod]
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
            return myDt.ActualizarCitologia(cod_examen, Diagnostico, comentario, inflamacion, calidadFrotis_causa, calidadFrotisAdecuado,
                candida_sp, gardnerela, vaginosis, herpes, tricomonas, otroAgenteInfeccioso, evaluacionHormonal_basales,
                evaluacionhormonal_intermedias, evaluacionhormonal_superficiales, Colposcopia, repetir, recomendaciones_otra,
                recomendaciones_biopsia, recomendaciones_tratamientos, NIC_I, NIC_II, NIC_III, origen_muestra, negativo, VPH,
                glandular, escamosa, adenocarcinomana, carcinomana_celula, celula_atipica, lesion_escamosa_AltoGrado, lesion_escamosa_BajoGrado);
        }

        [WebMethod]
        public bool ActualizarBiopsia(String cod_examen, String macroscopica, String microscopica, String codificacion, String diagnostico)
        {
            return myDt.ActualizarBiopsia(cod_examen, macroscopica, microscopica, codificacion, diagnostico);
        }

        [WebMethod]
        public bool ActualizarCitologiaLiquidos(String cod_examen, String macroscopica, String microscopica, String diagnostico)
        {
            return myDt.ActualizarCitologiaLiquidos(cod_examen, macroscopica, microscopica, diagnostico);
        }

        [WebMethod]
        public bool ActualizarMedico(String nombre, String telefono, String celular)
        {
            return myDt.ActualizarMedico(nombre, telefono, celular);
        }

        /*****METODOS PARA BORRAR**********************************************************************/
        [WebMethod]
        public bool BorrarEmpleado(String usuario)
        {

            return myDt.BorrarEmpleado(usuario);

        }//End public bool BorrarEmpleado(String usuario)

        [WebMethod]
        public bool BorrarMaterial(String material)
        {

            return myDt.BorrarMaterial(material);

        }//End public bool BorrarMaterial(String material)

        /***********************COMPROBRACION DE LOGIN*****************************************************/
        [WebMethod]
        public bool esUsuarioValido(string usuario, string contrasena)
        {

            return myDt.esUsuarioValido(usuario, contrasena);
        }//End public bool esUsuarioValido(string usuario, string contrasena)

        /***********************INGRESOS POR MES*****************************************************/
        [WebMethod]
        public ArrayList getIngresosPorMes(int Año, int Mes)
        {

            return myDt.getIngresosPorMes(Año, Mes);

        }//End getIngresosPorMes(Año,Mes)

        /***********************EGRESOS POR MES*****************************************************/
        [WebMethod]
        public String getEgresosPorMes(int Anio, int Mes)
        {

            return myDt.getEgresosPorMes(Anio,Mes);

        }//End getEgresosPorMes(Año,Mes)

        /***********************CITOLOGIAS GINECOLOGICAS*****************************************************/
        [WebMethod]
        public ArrayList getCitologiasGinecologicas(int Año)
        {

            return myDt.getCitologiasGinecologicas(Año);

        }//End public ArrayList getCitologiasGinecologicas()

        /***********************GET BIOPSIAS*****************************************************/
        [WebMethod]
        public ArrayList getBiopsias(int Año)
        {


            return myDt.getBiopsias(Año);

        }//End getBiopsias()

        /***********************GET PACIENTES*****************************************************/
        [WebMethod]
        public ArrayList getPacientes()
        {

            return myDt.getPacientes();

        }//End getPacientes()

        /***********************GET MEDICOS*****************************************************/
        [WebMethod]
        public ArrayList getMedicos()
        {

            return myDt.getMedicos();

        }//End getMedicos()



        /***********************GET MATERIAL ENVIADO BIOPSIA*****************************************************/
        [WebMethod]
        public ArrayList getMaterialEnviado_Biopsia()
        {

            return myDt.getMaterialEnviado_Biopsia();

        }//End getMaterialEnviado_Biopsia()

        /***********************GET MATERIAL ENVIADO CITOLOGIA NO GINECOLOGICA*****************************************************/
        [WebMethod]
        public string getMaterialEnviado_CitologiaNoGinecologica()
        {

            return myDt.getMaterialEnviado_CitologiaNoGinecologica();

        }//End getMaterialEnviado_CitologiaNoGinecologica()


        /*****************************CONSULTA MEDICO POR BIOPSIAS*****************************************************/
        [WebMethod]
        public string consultaMedicoBiopsias(int anio, int mes)
        {

            String t = myDt.consultaMedicoBiopsia(anio, mes);
            return t;

        }//End public string pruebaWebService()

        /*****************************CONSULTA MEDICO POR CITOLOGIAS*****************************************************/
        [WebMethod]
        public string consultaMedicoCitologia(int anio, int mes)
        {

            String t = myDt.consultaMedicoCitologia(anio, mes);
            return t;

        }//End public string pruebaWebService()

        /*********************METDOOS AGREGADOS*******************************/
        [WebMethod]
        public string getAccesos(String usuario)
        {
            return myDt.getAccesos(usuario);
           
        }

        [WebMethod]
        public int getCantidadDeExamenes(String tabla, String filtroedad, String fechainicial, String fechafinal, String filtroCategoria)
        {
            return myDt.getCantidadDeExamenes(tabla, filtroedad, fechainicial, fechafinal, filtroCategoria);
        }

        [WebMethod]
        public string getExamenesFiltrados(String tabla, String filtroedad, String fechainicial, String fechafinal, String filtroCategoria)
        {
            return myDt.getExamenesFiltrados(tabla, filtroedad, fechainicial, fechafinal, filtroCategoria);
        }

        [WebMethod]
        public String getMedicos_Nombres()
        {
            String resultado = "";
            ArrayList resp = myDt.getMedicos_Nombres();
            for (int i = 0; i < resp.Count; i++)
                resultado += resp[i].ToString();
            return resultado;
        }

        [WebMethod]
        public bool BorrarMedico(String nombre)
        {
            return myDt.BorrarMedico(nombre);
        }

        /***********************************NUEVOS METODOS**************************************/
        [WebMethod]
        public bool BorrarMuestra(String codigo)
        {
            return myDt.BorrarMuestra(codigo);
        }
/*************************************NUEVOS METODOS PARA CONTABILIDAD*********************************************/

        [WebMethod]
        public bool VerificaryActualizarContabilidad()
        {
            return myDt.VerificaryActualizarContabilidad();
        }

        [WebMethod]
        public bool InsertarGastoenEgreso(String NombreContable, int anio_contable, int mes_contable,
            String descripcion,int dia, int valor)
        {
            return myDt.InsertarGastoenEgreso(NombreContable,anio_contable,mes_contable,descripcion,dia,valor);
        }

        [WebMethod]
        public string getTodosLosGastos(int anio, int mes, String categoria)
        {
            return myDt.getTodosLosGastos(anio,mes, categoria);
        }

        [WebMethod]
        public string getIngresosCitologia(String fechainicial, String fechafinal)
        {
            return myDt.getIngresosCitologia(fechainicial, fechafinal);
        }

        [WebMethod]
        public string getIngresosBiopsia(String fechainicial, String fechafinal)
        {
            return myDt.getIngresosBiopsia(fechainicial, fechafinal);
        }

/*************************************FIN NUEVOS METODOS PARA CONTABILIDAD*********************************************/

/*************************************METODOS PARA OBTENER DATOS PARA IMPRIMIR****************************************/
        [WebMethod]
        public string getMuestraGinecologica(String idMuestra)
        {
            return myDt.getMuestraGinecologica(idMuestra);
        }

        [WebMethod]
        public string getMuestraNo_Ginecologica(String idMuestra)
        {
            return myDt.getMuestraNo_Ginecologica(idMuestra);
        }

        [WebMethod]
        public string getMuestraBiopsia(String idMuestra)
        {
            return myDt.getMuestraBiopsia(idMuestra);
        }

        [WebMethod]
        public string getMaterialEnviadoBiopsiaImprimir(String idMuestra)
        {
            return myDt.getMaterialEnviadoBiopsiaImprimir(idMuestra);
        }

        [WebMethod]
        public string getIdExamenes()
        {
            return myDt.getIdExamenes();
        }//End getIDExamenes()


/*********************************************BUSCAR*************************************************************/
        [WebMethod]
        public string buscarCitologia(String idMuestra)
        {
            return myDt.buscarCitologia(idMuestra);
        }//End buscarCitologia()

        [WebMethod]
        public string buscarCitologiaLiquidos(String idMuestra)
        {
            return myDt.buscarCitologiaLiquidos(idMuestra);
        }//End buscarCitologiaLiquidos()

        [WebMethod]
        public string buscarBiopsia(String idMuestra)
        {
            return myDt.buscarBiopsia(idMuestra);
        }//End buscarBiopsia()

    }//End public class clinicaPatologiaWebService : System.Web.Services.WebService
}//End 
