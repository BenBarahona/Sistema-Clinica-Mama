using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;
using DT;

namespace Sistema_BD_Clinica_Patologica.Web
{
    /// <summary>
    /// Summary description for WSClinica
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSClinica : System.Web.Services.WebService
    {
        public enum peticion
        {
            Login, getLoginDatos
        }

        [WebMethod]
        public string esUsuarioValido(string usuario, string password)
        {
            return DataTransaction.loginAlSistema(usuario, password);
        }
        
        /*******************************************************************************
         ****************************** METODOS GETS ***********************************
         *******************************************************************************/
        [WebMethod]
        public int getIDdeMedico(String nombre_Medico)
        {
            return DataTransaction.getIDdeMedico(nombre_Medico);
        }
        

        [WebMethod]
        public int getIDdePaciente(String nombre_Paciente, String s_nombre_Paciente, String apellido_paciente, String s_apellido_paciente)
        {
            return DataTransaction.getIDdePaciente(nombre_Paciente, s_nombre_Paciente, apellido_paciente, s_apellido_paciente);
        }

        [WebMethod]
        public String getMedicos_Nombres()
        {
            String resultado = "";
            ArrayList resp = DataTransaction.getMedicos_Nombres();
            for (int i = 0; i < resp.Count; i++)
                resultado += resp[i].ToString();
            return resultado;
        }

        [WebMethod]
        public String getMedicosTodos()
        {
            return DataTransaction.getMedicosTodos();
        }

        [WebMethod]
        public string getMaterialEnviadoBiopsiaImprimir(String idMuestra, String tabla)
        {
            return DataTransaction.getMaterialEnviadoBiopsiaImprimir(idMuestra, tabla);
        }

        [WebMethod]
        public string getMaterialEnviado_CitologiaNoGinecologica()
        {

            return DataTransaction.getMaterialEnviado_CitologiaNoGinecologica();

        }

        [WebMethod]
        public String getMedico(int id_medico)
        {
            return DataTransaction.getMedico(id_medico);
        }//End public String getMedico(int id_medico)

        /*************************************METODOS PARA OBTENER DATOS PARA IMPRIMIR****************************************/
        [WebMethod]
        public string getMuestraGinecologica(String idMuestra)
        {
            return DataTransaction.getMuestraGinecologica(idMuestra);
        }

        [WebMethod]
        public string getMuestraNo_Ginecologica(String idMuestra)
        {
            return DataTransaction.getMuestraNo_Ginecologica(idMuestra);
        }

        [WebMethod]
        public string getMuestraBiopsia(String idMuestra)
        {
            return DataTransaction.getMuestraBiopsia(idMuestra);
        }

        [WebMethod]
        public string getIdExamenes()
        {
            return DataTransaction.getIdExamenes();
        }//End getIDExamenes()

        /****************************************METODOS PARA LAS CONSULTAS*****************************************************/

        [WebMethod]
        public int getCantidadDeExamenes(String tabla, String filtroedad, String fechainicial, String fechafinal, String filtroCategoria, String doctor)
        {
            return DataTransaction.getCantidadDeExamenes(tabla, filtroedad, fechainicial, fechafinal, filtroCategoria, doctor);
        }

        [WebMethod]
        public string getExamenesFiltrados(String tabla, String filtroedad, String fechainicial, String fechafinal, String filtroCategoria, String doctor)
        {
            String resultado = "";
            ArrayList resp = DataTransaction.getExamenesFiltrados(tabla, filtroedad, fechainicial, fechafinal, filtroCategoria, doctor);
            for (int i = 0; i < resp.Count; i++)
                resultado += resp[i].ToString();
            return resultado;
        }

        [WebMethod]
        public string consultaMedicoBiopsias(int anio, int mes, String doctor)
        {
            return DataTransaction.consultaMedicoBiopsia(anio, mes, doctor);

        }
        
        [WebMethod]
        public string consultaMedicoCitologia(int anio, int mes, String doctor)
        {
            return DataTransaction.consultaMedicoCitologia(anio, mes, doctor);
        }

        /*
        [WebMethod]
        public String getNombresPacientes()
        {
            String resultado = "";
            ArrayList resp = DataTransaction.getNombresPacientes();
            for (int i = 0; i < resp.Count; i++)
                resultado += resp[i].ToString();
            return resultado;
        }*/

        [WebMethod]
        public List<NombreCompleto> getNombresPacientes()
        {
            List<NombreCompleto> nombres = new List<NombreCompleto>();
            ArrayList resp = DataTransaction.getNombresPacientes();
            for (int i = 0; i < resp.Count; i++)
                nombres.Add((NombreCompleto)resp[i]);
            return nombres;
        }

        [WebMethod]
        public String getUsuarios()
        {
            return DataTransaction.getUsuarios();
        }

        [WebMethod]
        public String getDatosEmpleado(String usuario)
        {
            return DataTransaction.getDatosEmpleado(usuario);
        }

        [WebMethod]
        public String getAccesosDeUsuario(String usuario)
        {
            return DataTransaction.getAccesosDeUsuario(usuario);
        }

        /*******************************************************************************
         ******************************* METODOS INSERTS *******************************
         *******************************************************************************/
        [WebMethod]
        public bool InsertarMedico(String Nombre, String telefono, String celular, String direccion, String compania, String numeroCol)
        {
            return DataTransaction.InsertarMedico(Nombre, telefono, celular, direccion, compania, numeroCol);
        }//End public void InsertarMedico(int ID_Medico, String Nombre, String telefono, String celular)

        [WebMethod]
        public bool InsertarMaterial(String material_enviado)
        {
            return DataTransaction.InsertarMaterial(material_enviado);
        }//END public void InsertarMaterial

        [WebMethod]
        public bool InsertarPaciente(bool DIU, String fur, String fup, String pnombre,
            String snombre, String papellido, String sapellido, int edad, bool anticonceptivos, String expediente)
        {
            return DataTransaction.InsertarPaciente(DIU, fur, fup, pnombre, snombre, papellido, sapellido, edad, anticonceptivos, expediente);
        }

        [WebMethod]
        public bool InsertarBiopsia(String cod_examen, String fecha, int precio, String Diagnostico,
            String diagnostico_medico, int Id_paciente, int id_medico, String Nombre_Contable,
            int anio_contable, int mes_contable, String usuario_empleado, String descripcion_macros,
            String descripcion_micros, String codificacion, String fechaHoy, String fechaHoySQL, String fechaRecibido)
        {
            return DataTransaction.InsertarBiopsia(cod_examen, fecha, precio, Diagnostico,
                                        diagnostico_medico, Id_paciente, id_medico, Nombre_Contable,
                                        anio_contable, mes_contable, usuario_empleado, descripcion_macros,
                                        descripcion_micros, codificacion, fechaHoy, fechaHoySQL, fechaRecibido);
        }

        [WebMethod]
        public bool InsertarCitologiaNoGinecologica(String cod_examen, String fecha, int precio, String Diagnostico,
            String diagnostico_medico, int Id_paciente, int id_medico, String Nombre_Contable,
            int anio_contable, int mes_contable, String usuario_empleado, String descripcion_macros,
            String descripcion_micros, String comentario, String fechaHoy, String fechaHoySQL, String fechaRecibido)
        {
            return DataTransaction.InsertarCitologiaNoGinecologica(cod_examen, fecha, precio, Diagnostico,
                                                        diagnostico_medico, Id_paciente, id_medico, Nombre_Contable,
                                                        anio_contable, mes_contable, usuario_empleado, descripcion_macros,
                                                        descripcion_micros, comentario, fechaHoy, fechaHoySQL, fechaRecibido);
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
            return DataTransaction.InsertarCitologiaGinecologica(cod_examen, fecha, precio, Diagnostico,
                                                      diagnostico_medico, Id_paciente, id_medico, Nombre_Contable,
                                                      anio_contable, mes_contable, usuario_empleado, comentario,
                                                      inflamacion, calidadFrotis_causa, calidadFrotisAdecuado, anticonceptivos,
                                                      candida_sp, gardnerela, vaginosis, herpes, tricomonas,
                                                      otroAgenteInfeccioso, evaluacionHormonal_basales,
                                                      evaluacionhormonal_intermedias, evaluacionhormonal_superficiales, Colposcopia,
                                                      repetir, recomendaciones_otra, recomendaciones_biopsia, recomendaciones_tratamientos,
                                                      NIC_I, NIC_II, NIC_III, origen_muestra, negativo, VPH,
                                                      glandular, escamoza, adenocarcinomana, carcinomana_celula, celula_atipica,
                                                      lesion_escamosa_AltoGrado, lesion_escamosa_BajoGrado, fechaHoy, fechaHoySQL);
        }

        [WebMethod]
        public bool InsertarMaterialEnviado(String cod_examen, String material_enviado, String tabla)
        {
            return DataTransaction.InsertarMaterialEnviado(cod_examen, material_enviado, tabla);
        }

        [WebMethod]
        public bool InsertarEmpleado(String usuario, String tipoEmpleado, String p_nombre, String s_nombre, String p_apellido, String s_apellido,
            String contrasena, String correo, String accesos)
        {
            return DataTransaction.InsertarEmpleado(usuario, tipoEmpleado, p_nombre, s_nombre, p_apellido, s_apellido, contrasena, correo, accesos);
        }


        /*******************************************************************************
        ********************************* METODOS BUSCAR *******************************
        *******************************************************************************/
        [WebMethod]
        public string buscarCitologia(String idMuestra)
        {
            return DataTransaction.buscarCitologia(idMuestra);
        }//End buscarCitologia()

        [WebMethod]
        public string buscarCitologiaLiquidos(String idMuestra)
        {
            return DataTransaction.buscarCitologiaLiquidos(idMuestra);
        }//End buscarCitologiaLiquidos()

        [WebMethod]
        public string buscarBiopsia(String idMuestra)
        {
            return DataTransaction.buscarBiopsia(idMuestra);
        }//End buscarBiopsia()

        [WebMethod]
        public string buscarMuestraPorPaciente(String pNombre, String sNombre, String pApellido, String sApellido)
        {
            return DataTransaction.buscarMuestraPorPaciente(pNombre, sNombre, pApellido, sApellido);
        }
        
        /******************************************************************************
        ********************************* METODOS BORRAR ******************************
        *******************************************************************************/
        [WebMethod]
        public bool BorrarMedico(String nombre)
        {
            return DataTransaction.BorrarMedico(nombre);
        }

        [WebMethod]
        public bool BorrarMuestra(String codigo)
        {
            return DataTransaction.BorrarMuestra(codigo);
        }

        [WebMethod]
        public bool BorrarMaterial(String material)
        {

            return DataTransaction.BorrarMaterial(material);
        }

        [WebMethod]
        public bool BorrarEmpleado(String usuario)
        {
            return DataTransaction.BorrarEmpleado(usuario);
        }

        /*******************************************************************************
        ******************************* METODOS UPDATE *********************************
        ********************************************************************************/

        [WebMethod]
        public bool ActualizarCitologia(String cod_examen, String Diagnostico, String comentario,
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
            return DataTransaction.ActualizarCitologia(cod_examen, Diagnostico, comentario, inflamacion, calidadFrotis_causa, calidadFrotisAdecuado,
                candida_sp, gardnerela, vaginosis, herpes, tricomonas, otroAgenteInfeccioso, evaluacionHormonal_basales,
                evaluacionhormonal_intermedias, evaluacionhormonal_superficiales, Colposcopia, repetir, recomendaciones_otra,
                recomendaciones_biopsia, recomendaciones_tratamientos, NIC_I, NIC_II, NIC_III, origen_muestra, negativo, VPH,
                glandular, escamosa, adenocarcinomana, carcinomana_celula, celula_atipica, lesion_escamosa_AltoGrado, lesion_escamosa_BajoGrado, anticonceptivos_orales,
                fecha_examen, diagnostico_medico, diu, fur, fup, p_nombre, s_nombre, p_apellido, s_apellido, edad, expediente, precio);
        }

        [WebMethod]
        public bool ActualizarBiopsia(String cod_examen, String macroscopica, String microscopica, String codificacion, String diagnostico, 
            String fecha_examen, String diagnostico_medico, bool diu, String fur, String fup, String p_nombre,
            String s_nombre, String p_apellido, String s_apellido, int edad, String expediente, String materialEnviado, int precio, String fechaRecibido)
        {
            return DataTransaction.ActualizarBiopsia(cod_examen, macroscopica, microscopica, codificacion, diagnostico, fecha_examen, diagnostico_medico, 
                diu, fur, fup, p_nombre, s_nombre, p_apellido, s_apellido, edad, expediente, materialEnviado, precio, fechaRecibido);
        }

        [WebMethod]
        public bool ActualizarCitologiaLiquidos(String cod_examen, String macroscopica, String microscopica, String diagnostico,
            String fecha_examen, String diagnostico_medico, bool diu, String fur, String fup, String p_nombre,
            String s_nombre, String p_apellido, String s_apellido, int edad, String expediente, int precio, String material_enviado, String fechaRecibido)
        {
            return DataTransaction.ActualizarCitologiaLiquidos(cod_examen, macroscopica, microscopica, diagnostico, fecha_examen, diagnostico_medico,
                diu, fur, fup, p_nombre, p_apellido, s_nombre, s_apellido, edad, expediente, precio, material_enviado, fechaRecibido);
        }

        [WebMethod]
        public bool ActualizarMedico(String nombre, String telefono, String celular, String direccion, String compania, String numeroCol)
        {
            return DataTransaction.ActualizarMedico(nombre, telefono, celular, direccion, compania, numeroCol);
        }

        [WebMethod]
        public bool ActualizarPaciente(int id, bool DIU, String fur, String fup, String pnombre,
            String snombre, String papellido, String sapellido, int edad, bool anticonceptivos, String Expediente)
        {
            return DataTransaction.ActualizarPaciente(id, DIU, fur, fup, pnombre, snombre, papellido, sapellido, edad, anticonceptivos, Expediente);
        }

        [WebMethod]
        public bool ActualizarUsuario(String usuario, String tipoEmpleado, String p_nombre, String s_nombre, String p_apellido, String s_apellido,
            String contrasena, String correo, String accesos)
        {
            return DataTransaction.ActualizarUsuario(usuario, tipoEmpleado, p_nombre, s_nombre, p_apellido, s_apellido, contrasena, correo, accesos);
        }
    }
}
