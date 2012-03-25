using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DT;

namespace ProyectoSCA_Navigation.Web
{
    /// <summary>
    /// Summary description for WSCosecol
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSCosecol : System.Web.Services.WebService
    {
        public enum peticion
        {
            ingresar_afiliado, ingresar_empleado, solicitudModificarPerfil, mostrar_perfil_afiliado, mostrar_perfil_empleado, reingresar_afiliado,
            ingresar_aportacion, capitalizar_extra, login, ingresar_ocupacion, ingresar_AOS, ingresar_monto_AO, limite_AV, ingresar_motivo, ingresar_parentesco, ingresar_interes,
            getOcupacion, getParentesco, getAfiliado, getEmpleado, getIntereses, getMotivos, getMontoActual, getLimiteActual, eliminarOcupacion, eliminarMotivo, eliminarParentesco,
            eliminarInteres, getAfiliadosPagar, getControlPagos, modificarOcupacion, modificarMotivo, modificarParentesco, modificarInteres, getPermisosSeguridad, modificarPermisosSeguridad,
            getPermisosPersona, getProximoNumeroCertificado, insertarAportacionOE, pagarAportaciones, getAportacion_OE, getAportacionesACapitalizar, getSaldosACapitalizar, capitalizarExtraordinaria,
            buscarAfiliados, consultarEstadoDeCuenta, getPersonaTemporal, reporteAportaciones, reporteSaldos, modificarPerfil, getPerfilNuevo, rechazarPerfil
        }

        public enum peticion2 { enviar_Correo, encriptar }  

        [WebMethod]
        public String AgregarPeticion(peticion codigo_peticion, String String_DK)
        {
            //DT.DataTransaction dt = new DT.DataTransaction();
            switch (codigo_peticion)
            {
                case peticion.ingresar_empleado:
                    return "p00" + DataTransaction.InsertarEmpleado(String_DK);

                case peticion.ingresar_afiliado:
                    return "p01" + DataTransaction.InsertarAfiliado(String_DK).ToString(); 

                case peticion.ingresar_ocupacion:
                    return "p02" + DataTransaction.InsertarOcupacion(String_DK);

                case peticion.ingresar_motivo:
                    return "p03" + DataTransaction.InsertarMotivo(String_DK);

                case peticion.ingresar_parentesco:
                    return "p04" + DataTransaction.InsertarParentesco(String_DK);

                case peticion.ingresar_monto_AO:
                    return "p05" + DataTransaction.InsertarNuevoMonto(String_DK);

                case peticion.limite_AV:
                    return "p06" + DataTransaction.InsertarNuevoLimite(String_DK);

                case peticion.ingresar_interes:
                    return "p07" + DataTransaction.InsertarInteres(String_DK);

                case peticion.solicitudModificarPerfil:
                    SD.Correo correo = new SD.Correo();
                    correo.enviarCorreoModificar(String_DK);
                    return "p08" + DataTransaction.SolicitarModificacionDePerfil(String_DK);

                case peticion.modificarPerfil:
                    return "p09" + DataTransaction.ModificarPerfil(String_DK);

                case peticion.getOcupacion:
                    return "p10" + DataTransaction.getTodasOcupaciones();

                case peticion.getMotivos:
                    return "p11" + DataTransaction.getTodosMotivos();

                case peticion.getParentesco:
                    return "p12" + DataTransaction.getTodosParentescos();

                case peticion.getMontoActual:
                    return "p13" + DataTransaction.getMontoActual();

                case peticion.getLimiteActual:
                    return "p14" + DataTransaction.getLimiteActual();

                case peticion.getIntereses:
                    return "p15" + DataTransaction.getTodosIntereses();

                case peticion.eliminarOcupacion:
                    return "p17" + DataTransaction.DeshabilitarOcupacion(String_DK);

                case peticion.eliminarMotivo:
                    return "p18" + DataTransaction.DeshabilitarMotivo(String_DK);
                    
                case peticion.eliminarParentesco:
                    return "p19" + DataTransaction.DeshabilitarParentesco(String_DK);

                case peticion.eliminarInteres:
                    return "p20" + DataTransaction.DeshabilitarInteres(String_DK);

                case peticion.getAfiliado:
                    return "p21" + DataTransaction.getPerfilAfiliado(String_DK);

                case peticion.getEmpleado:
                    return "p22" + DataTransaction.getPerfilEmpleado(String_DK);

                case peticion.login:
                    return "p23" + DataTransaction.getLogin(String_DK);

                case peticion.modificarOcupacion:
                    return "p24" + DataTransaction.ModificarOcupacion(String_DK);

                case peticion.modificarParentesco:
                    return "p25" + DataTransaction.ModificarParentesco(String_DK);

                case peticion.modificarMotivo:
                    return "p26" + DataTransaction.ModificarMotivo(String_DK);

                case peticion.modificarInteres:
                    return "p27" + DataTransaction.ModificarInteres(String_DK);

                case peticion.getAfiliadosPagar:
                    return "p28" + DataTransaction.getAfiliadosPAGAR();

                case peticion.getControlPagos:
                    return "p29" + DataTransaction.getControlDePago(String_DK);

                case peticion.pagarAportaciones:
                    return "p30" + DataTransaction.PagarAportaciones(String_DK);

                case peticion.buscarAfiliados:
                    return "p31" + DataTransaction.BuscarAfiliadoConsultas(String_DK);

                case peticion.consultarEstadoDeCuenta:
                    return "p32" + DataTransaction.ConsultarEstadoDeCuenta(String_DK);

                case peticion.getProximoNumeroCertificado:
                    return "p33" + DataTransaction.getSiguienteNumeroDeCertificado();

                case peticion.getPermisosSeguridad:
                    return "p34" + DataTransaction.getTodosLosPermisos();

                case peticion.modificarPermisosSeguridad:
                    return "p35" + DataTransaction.ModificarPermisos(String_DK);

                case peticion.insertarAportacionOE:
                    return "p36" + DataTransaction.AsignarAportacionOE(String_DK);

                case peticion.getAportacion_OE:
                    return "p37" + DataTransaction.getAOE();

                case peticion.getAportacionesACapitalizar:
                    return "p38" + DataTransaction.getAportacionesACapitalizar();

                case peticion.getSaldosACapitalizar:
                    return "p39" + DataTransaction.getSaldosACapitalizar();

                case peticion.capitalizarExtraordinaria:
                    return "p40" + DataTransaction.CapitalizarExtraordinariamente(String_DK);

                case peticion.getPersonaTemporal:
                    return "p41" + DataTransaction.getPersonaTemporales();

                case peticion.reporteAportaciones:
                    return "p42" + DataTransaction.ReporteTodasAportaciones(String_DK);

                case peticion.reporteSaldos:
                    return "p43" + DataTransaction.ReporteTodosSaldos(String_DK);

                case peticion.getPerfilNuevo:
                    return "p44" + DataTransaction.getPerfilNuevo(String_DK);

                case peticion.rechazarPerfil:
                    return "p45" + DataTransaction.RechazarPerfil(String_DK);

                default: return "Tenga Cuidado con eso! (No se ha creado la peticion en el WS)";

            }
        }

       /* [WebMethod]
        public String UsarSD(peticion2 pt, String valor)
        {
            switch (pt)
            {
                case peticion2.enviar_Correo:
                    SD.Correo correo = new SD.Correo();
                    return correo.Enviar_Correo(valor, valor);
                case peticion2.encriptar:
                    SD.SD sd = new SD.SD();
                    return sd.md5(valor);
                default:
                    break;
            }
            return null;
        }*/
    }
}
