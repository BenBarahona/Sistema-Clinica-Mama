using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using ProyectoSCA_Navigation.Clases;
using ProyectoSCA_Navigation.Clases.Clases_Para_los_Grids;

namespace ProyectoSCA_Navigation.Clases
{
    /********************************************************************************************************************************
     *****************************************************************dAnY************************************************************
     *********************************************************************************************************************************/
    public class TC
    {
        String m_Enviar;

        /*********************************************************************************************************************************
         *****************************************************************INSERTS*********************************************************
         *********************************************************************************************************************************/

        //Insertar una nuevo limite aportacion voluntaria
        public String InsertarLimite(String limite, String correo)
        {
            if (limite.Equals("") || correo.Equals(""))
                return null;

            m_Enviar = limite + "&" + correo;

            return m_Enviar;
        }

        //Insertar una nuevo monto aportacion obligatoria
        public String InsertarMonto(String monto, String correo)
        {
            if (monto.Equals("") || correo.Equals(""))
                return null;

            m_Enviar = monto + "&" + correo;

            return m_Enviar;
        }

        //Insertar una nuevo motivo
        public String InsertarMotivo(String motivo, String correo)
        {
            if (motivo.Equals("") || correo.Equals(""))
                return null;

            string m_Enviar = motivo + "&" + correo;

            return m_Enviar;
        }

        //Insertar una nueva ocupacion
        public String InsertarOcupacion(String ocupacion, String correo)
        {
            if (ocupacion.Equals("") || correo.Equals(""))
                return null;

            string m_Enviar = ocupacion + "&" + correo;

            return m_Enviar;
        }

        //Insertar un nuevo interes
        public String InsertarInteres(Interes intereses, String correo)
        {
            if (intereses == null || correo.Equals(""))
                return null;

            char del1 = ',';

            /*
             *En la variable m_Enviar se guarda el string que se enviara al WS y lo que se recibe es una referencia hacia la clase interes, el formato es el siguiente:
             *nombre(string), tasa(Int64), aplica obligatoria(boolean), aplica voluntaria(boolean), aplica obligatoria especial(boolean), 
             *aplica voluntaria arriba(boolean), monto voluntaria(Int64)
            */
            m_Enviar = intereses.Nombre + del1 + intereses.Tasa + del1 + intereses.Aplica_obligatoria + del1 + intereses.Aplica_voluntaria + del1
                + intereses.Aplica_obligatoria_especial + del1 + intereses.Aplica_voluntaria_arriba + del1 + intereses.Monto_voluntaria + "&" + correo;

            return m_Enviar;
        }

        //Insertar una nueva parentesco
        public String InsertarParentesco(String parentesco, String correo)
        {
            if (parentesco.Equals("") || correo.Equals(""))
                return null;

            string m_Enviar = parentesco + "&" + correo;

            return m_Enviar;
        }

        //Insertar un nueva aportacion obligatoria especial
        public String InsertarAportacionObligatoriaEspecial(aportacionObligatoriaEspecial aportacion, String correo)
        {
            if (aportacion == null || correo.Equals(""))
                return null;

            char del1 = ',';

            /*
             *En la variable m_Enviar se guarda el string que se enviara al WS y lo que se recibe es una referencia hacia la clase aportacion obligatoria especial
             *y el formato es el siguiente:
             *Monto(int), fecha realizacion, motivo(string), descripcion(string) & correo
            */
            String[] fecha = this.convertirFechaBD(aportacion.FechaRealizacion).Split('-');
            m_Enviar = aportacion.Monto + "," + fecha[0] + del1 + fecha[1] + del1 + aportacion.Motivo + del1 + aportacion.Descripcion + "&" + correo;

            return m_Enviar;
        }

        //Insertar un nuevo afiliado
        public String InsertarAfiliado(Afiliado afiliado, String correo)
        {
            if (afiliado == null || correo.Equals(""))
                return null;

            MD5 md5 = new MD5();
            char del1 = ',', del2 = ';';

            /*
             *En la variable m_Enviar se guarda el string que se enviara al WS y lo que se recibe es una referencia hacia la clase afiliado
             *el formato es el siguiente: datos personales, datos laborales y beneficiarios
            */
            /*
             * DATOS PERSONALES:
             * identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), 
             * fecha nacimiento(string), estado civil(string), genero(string),direccion(string);
            */
            m_Enviar = afiliado.identidad + del1 + afiliado.primerNombre + del1 + afiliado.segundoNombre + del1 + afiliado.primerApellido + del1 + afiliado.segundoApellido + del1
                + this.convertirFechaBD(afiliado.fechaNacimiento) + del1 + afiliado.estadoCivil + del1 + afiliado.genero + del1 + afiliado.direccion + del2;

            /*
             * DATOS PERSONALES:
             * telefono personal(string);
             * aqui son varios asi que por eso aparte
            */
            for (int i = 0; i < afiliado.telefonoPersonal.Count; i++)
            {
                if (i < afiliado.telefonoPersonal.Count - 1)
                    m_Enviar += afiliado.telefonoPersonal[i] + del1;
                else if (i < afiliado.telefonoPersonal.Count)
                    m_Enviar += afiliado.telefonoPersonal[i];
            }
            m_Enviar += del2;

            /*
             * DATOS PERSONALES:
             * telefono celular(string);
             * aqui son varios asi que por eso aparte
            */
            for (int i = 0; i < afiliado.celular.Count; i++)
            {
                if (i < afiliado.celular.Count - 1)
                    m_Enviar += afiliado.celular[i] + del1;
                else if (i < afiliado.celular.Count)
                    m_Enviar += afiliado.celular[i];
            }

            /*
             * DATOS LABORALES:
             * correo electronico(string), password(string 32 caracter), nombre empresa(string), telefono empresa(string), direccion empresa(string),
             * fecha ingreso cooperativa(string), depto empresa(string), lugar nacimiento(string), ocupacion(stirng);
            */
            md5.Value = afiliado.Password;
            m_Enviar += del2 + afiliado.CorreoElectronico + del1 + md5.FingerPrint + del2 + afiliado.NombreEmpresa + del1 + afiliado.TelefonoEmpresa + del1 + afiliado.DireccionEmpresa
                + del1 + this.convertirFechaBD(afiliado.fechaIngresoCooperativa) + del1 + afiliado.DepartamentoEmpresa + del1 + afiliado.lugarDeNacimiento + del2
                + afiliado.Ocupacion + del2;

            /*
             * DATOS BENEFICIARIO DE CONTINGENCIA:
             * No. Identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), fecha_nac(string), estado_Civil(string), genero(string),
             * direccion(string), parentesco(string);
            */
            m_Enviar += afiliado.BeneficiarioCont.identidad + del1 + afiliado.BeneficiarioCont.primerNombre + del1 + afiliado.BeneficiarioCont.segundoNombre
                + del1 + afiliado.BeneficiarioCont.primerApellido + del1 + afiliado.BeneficiarioCont.segundoApellido + del1 + this.convertirFechaBD(afiliado.BeneficiarioCont.fechaNacimiento)
                + del1 + afiliado.BeneficiarioCont.estadoCivil + del1 + afiliado.BeneficiarioCont.genero + del1 + afiliado.BeneficiarioCont.direccion + del1 + afiliado.BeneficiarioCont.Parentesco;
            m_Enviar += del2;

            /*
             * DATOS BENEFICIARIOS NORMALES:
             * No. Identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), fecha_nac(string), estado_Civil(string), genero(string),
             * direccion(string), parentesco(string), porcentaje seguros(Int64), porcentaje aportacion(Int64)
            */
            for (int i = 0; i < afiliado.bensNormales.Count; i++)
            {
                BeneficiarioNormal item = afiliado.bensNormales[i];
                if (i < afiliado.celular.Count - 1)
                {
                    m_Enviar += item.identidad + del1 + item.primerNombre + del1 + item.segundoNombre + del1 + item.primerApellido + del1 + item.segundoApellido + del1 + this.convertirFechaBD(item.fechaNacimiento)
                + del1 + item.estadoCivil + del1 + item.genero + del1 + item.direccion + del1 + item.Parentesco + del1 + item.porcentajeSeguros + del1 + item.porcentajeAportaciones;
                    m_Enviar += del2;
                }
                else if (i < afiliado.celular.Count)
                {
                    m_Enviar += item.identidad + del1 + item.primerNombre + del1 + item.segundoNombre + del1 + item.primerApellido + del1 + item.segundoApellido + del1 + this.convertirFechaBD(item.fechaNacimiento)
                + del1 + item.estadoCivil + del1 + item.genero + del1 + item.direccion + del1 + item.Parentesco + del1 + item.porcentajeSeguros + del1 + item.porcentajeAportaciones;
                }
            }

            m_Enviar += "&" + correo;

            return m_Enviar;
        }

        //Insertar un nuevo empleado
        public String InsertarEmpleado(Empleado empleado, String correo, Boolean administrador)
        {
            if (empleado == null || correo.Equals(""))
                return null;

            MD5 md5 = new MD5();
            char del1 = ',', del2 = ';';

            /*
             *En la variable m_Enviar se guarda el string que se enviara al WS y lo que se recibe es una referencia hacia la clase afiliado
             *el formato es el siguiente: datos personales, datos laborales y beneficiarios
            */
            /*
             * DATOS PERSONALES:
             * identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), 
             * fecha nacimiento(string), estado civil(string), genero(string),direccion(string);
            */
            m_Enviar = empleado.identidad + del1 + empleado.primerNombre + del1 + empleado.segundoNombre + del1 + empleado.primerApellido + del1 + empleado.segundoApellido + del1
                + this.convertirFechaBD(empleado.fechaNacimiento) + del1 + empleado.estadoCivil + del1 + empleado.genero + del1 + empleado.direccion + del2;

            /*
             * DATOS PERSONALES:
             * telefono personal(string);
             * aqui son varios asi que por eso aparte
            */
            for (int i = 0; i < empleado.telefonoPersonal.Count; i++)
            {
                if (i < empleado.telefonoPersonal.Count - 1)
                    m_Enviar += empleado.telefonoPersonal[i] + del1;
                else if (i < empleado.telefonoPersonal.Count)
                    m_Enviar += empleado.telefonoPersonal[i];
            }
            m_Enviar += del2;
            /*
             * DATOS PERSONALES:
             * telefono celular(string);
             * aqui son varios asi que por eso aparte
            */
            for (int i = 0; i < empleado.celular.Count; i++)
            {
                if (i < empleado.celular.Count - 1)
                    m_Enviar += empleado.celular[i] + del1;
                else if (i < empleado.celular.Count)
                    m_Enviar += empleado.celular[i];
            }

            /*
             * DATOS LABORALES:
             * correo electronico(string), password(string), puesto(string)
            */
            md5.Value = empleado.Password;
            m_Enviar += del2 + empleado.correoElectronico + del1 + md5.FingerPrint + del1 + empleado.Puesto + "&" + correo + "&" + administrador.ToString();

            return m_Enviar;
        }

        /*********************************************************************************************************************************
         *********************************************************FIN*****INSERTS*********************************************************
         *********************************************************************************************************************************/

        /*********************************************************************************************************************************
         *****************************************************************DESHABILITAR****************************************************
         *********************************************************************************************************************************/

        //Deshabilitar una motivo
        public String DeshabilitarMotivo(String motivo, String correo)
        {
            if (motivo.Equals("") || correo.Equals(""))
                return null;

            string m_Enviar = motivo + "&" + correo;

            return m_Enviar;
        }

        //Deshabilitar una ocupacion
        public String DeshabilitarOcupacion(String ocupacion, String correo)
        {
            if (ocupacion.Equals("") || correo.Equals(""))
                return null;

            string m_Enviar = ocupacion + "&" + correo;

            return m_Enviar;
        }

        //Deshabilitar un interes
        public String DeshabilitarInteres(String nombreInteres, String correo)
        {
            if (nombreInteres.Equals("") || correo.Equals(""))
                return null;

            m_Enviar = nombreInteres + "&" + correo;

            return m_Enviar;
        }

        //Deshabilitar una parentesco
        public String DeshabilitarParentesco(String parentesco, String correo)
        {
            if (parentesco.Equals("") || correo.Equals(""))
                return null;

            string m_Enviar = parentesco + "&" + correo;

            return m_Enviar;
        }

        /*********************************************************************************************************************************
         ************************************************************FIN**DESHABILITAR****************************************************
         *********************************************************************************************************************************/

        /*********************************************************************************************************************************
         ***********************************************************MODIFICAR*************************************************************
         *********************************************************************************************************************************/

        //Solicitud de modificar un afiliado
        public String SolicitudModificarAfiliado(Afiliado afiliado, String correo, String NoIdentidadViejo)
        {
            if (afiliado == null || correo.Equals(""))
                return null;

            MD5 md5 = new MD5();
            char del1 = ',', del2 = ';';

            /*
             *En la variable m_Enviar se guarda el string que se enviara al WS y lo que se recibe es una referencia hacia la clase afiliado
             *el formato es el siguiente: datos personales, datos laborales y beneficiarios
            */
            /*
             * DATOS PERSONALES:
             * identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), 
             * fecha nacimiento(string), estado civil(string), genero(string),direccion(string);
            */
            m_Enviar = afiliado.identidad + del1 + afiliado.primerNombre + del1 + afiliado.segundoNombre + del1 + afiliado.primerApellido + del1 + afiliado.segundoApellido + del1
                + this.convertirFechaBD(afiliado.fechaNacimiento) + del1 + afiliado.estadoCivil + del1 + afiliado.genero + del1 + afiliado.direccion + del2;

            /*
             * DATOS PERSONALES:
             * telefono personal(string);
             * aqui son varios asi que por eso aparte
            */
            for (int i = 0; i < afiliado.telefonoPersonal.Count; i++)
            {
                if (i < afiliado.telefonoPersonal.Count - 1)
                    m_Enviar += afiliado.telefonoPersonal[i] + del1;
                else if (i < afiliado.telefonoPersonal.Count)
                    m_Enviar += afiliado.telefonoPersonal[i];
            }
            m_Enviar += del2;
            /*
             * DATOS PERSONALES:
             * telefono celular(string);
             * aqui son varios asi que por eso aparte
            */
            for (int i = 0; i < afiliado.celular.Count; i++)
            {
                if (i < afiliado.celular.Count - 1)
                    m_Enviar += afiliado.celular[i] + del1;
                else if (i < afiliado.celular.Count)
                    m_Enviar += afiliado.celular[i];
            }

            /*
             * DATOS LABORALES:
             * correo electronico(string), password(string 32 caracter), nombre empresa(string), telefono empresa(string), direccion empresa(string),
             * fecha ingreso cooperativa(string), depto empresa(string), lugar nacimiento(string), ocupacion(stirng);
            */
            if (afiliado.Password.Length < 32)
            {
                md5.Value = afiliado.Password;
                m_Enviar += del2 + afiliado.CorreoElectronico + del1 + md5.FingerPrint + del2 + afiliado.NombreEmpresa + del1 + afiliado.TelefonoEmpresa + del1 + afiliado.DireccionEmpresa
                + del1 + this.convertirFechaBD(afiliado.fechaIngresoCooperativa) + del1 + afiliado.DepartamentoEmpresa + del1 + afiliado.lugarDeNacimiento + del2
                + afiliado.Ocupacion + del2;
            }
            else
                m_Enviar += del2 + afiliado.CorreoElectronico + del1 + afiliado.Password + del2 + afiliado.NombreEmpresa + del1 + afiliado.TelefonoEmpresa + del1 + afiliado.DireccionEmpresa
                + del1 + this.convertirFechaBD(afiliado.fechaIngresoCooperativa) + del1 + afiliado.DepartamentoEmpresa + del1 + afiliado.lugarDeNacimiento + del2
                + afiliado.Ocupacion + del2;

            /*
             * DATOS BENEFICIARIO DE CONTINGENCIA:
             * No. Identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), fecha_nac(string), estado_Civil(string), genero(string),
             * direccion(string), parentesco(string);
            */
            m_Enviar += afiliado.BeneficiarioCont.identidad + del1 + afiliado.BeneficiarioCont.primerNombre + del1 + afiliado.BeneficiarioCont.segundoNombre
                + del1 + afiliado.BeneficiarioCont.primerApellido + del1 + afiliado.BeneficiarioCont.segundoApellido + del1 + this.convertirFechaBD(afiliado.BeneficiarioCont.fechaNacimiento)
                + del1 + afiliado.BeneficiarioCont.estadoCivil + del1 + afiliado.BeneficiarioCont.genero + del1 + afiliado.BeneficiarioCont.direccion + del1 + afiliado.BeneficiarioCont.Parentesco;
            m_Enviar += del2;

            /*
             * DATOS BENEFICIARIOS NORMALES:
             * No. Identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), fecha_nac(string), estado_Civil(string), genero(string),
             * direccion(string), parentesco(string), porcentaje seguros(Int64), porcentaje aportacion(Int64)
            */
            for (int i = 0; i < afiliado.bensNormales.Count; i++)
            {
                BeneficiarioNormal item = afiliado.bensNormales[i];
                if (i < afiliado.celular.Count - 1)
                {
                    m_Enviar += item.identidad + del1 + item.primerNombre + del1 + item.segundoNombre + del1 + item.primerApellido + del1 + item.segundoApellido + del1 + this.convertirFechaBD(item.fechaNacimiento)
                + del1 + item.estadoCivil + del1 + item.genero + del1 + item.direccion + del1 + item.Parentesco + del1 + item.porcentajeSeguros + del1 + item.porcentajeAportaciones;
                    m_Enviar += del2;
                }
                else if (i < afiliado.celular.Count)
                {
                    m_Enviar += item.identidad + del1 + item.primerNombre + del1 + item.segundoNombre + del1 + item.primerApellido + del1 + item.segundoApellido + del1 + this.convertirFechaBD(item.fechaNacimiento)
                + del1 + item.estadoCivil + del1 + item.genero + del1 + item.direccion + del1 + item.Parentesco + del1 + item.porcentajeSeguros + del1 + item.porcentajeAportaciones;
                }
            }

            m_Enviar += "&" + correo + "&" + NoIdentidadViejo + "~" + afiliado.EstadoAfiliado;

            return m_Enviar;
        }

        //solicitud de modificar un empleado
        public String SolicitudModificarEmpleado(Empleado empleado, String correo, String NoIdentidadViejo, Boolean Administrador)
        {
            if (empleado == null || correo.Equals(""))
                return null;

            MD5 md5 = new MD5();
            char del1 = ',', del2 = ';';

            /*
             *En la variable m_Enviar se guarda el string que se enviara al WS y lo que se recibe es una referencia hacia la clase afiliado
             *el formato es el siguiente: datos personales, datos laborales y beneficiarios
            */
            /*
             * DATOS PERSONALES:
             * identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), 
             * fecha nacimiento(string), estado civil(string), genero(string),direccion(string);
            */
            m_Enviar = empleado.identidad + del1 + empleado.primerNombre + del1 + empleado.segundoNombre + del1 + empleado.primerApellido + del1 + empleado.segundoApellido + del1
                + this.convertirFechaBD(empleado.fechaNacimiento) + del1 + empleado.estadoCivil + del1 + empleado.genero + del1 + empleado.direccion + del2;

            /*
             * DATOS PERSONALES:
             * telefono personal(string);
             * aqui son varios asi que por eso aparte
            */
            for (int i = 0; i < empleado.telefonoPersonal.Count; i++)
            {
                if (i < empleado.telefonoPersonal.Count - 1)
                    m_Enviar += empleado.telefonoPersonal[i] + del1;
                else if (i < empleado.telefonoPersonal.Count)
                    m_Enviar += empleado.telefonoPersonal[i];
            }
            m_Enviar += del2;
            /*
             * DATOS PERSONALES:
             * telefono celular(string);
             * aqui son varios asi que por eso aparte
            */
            for (int i = 0; i < empleado.celular.Count; i++)
            {
                if (i < empleado.celular.Count - 1)
                    m_Enviar += empleado.celular[i] + del1;
                else if (i < empleado.celular.Count)
                    m_Enviar += empleado.celular[i];
            }

            /*
             * DATOS LABORALES:
             * correo electronico(string), password(string 32 caracteres), puesto(string)
            */
            if (empleado.Password.Length < 32)
            {
                md5.Value = empleado.Password;
                m_Enviar += del2 + empleado.correoElectronico + del1 + md5.FingerPrint + del1 + empleado.Puesto + "&" + correo + "&" + NoIdentidadViejo + "&" + Administrador.ToString();
            }
            else
                m_Enviar += del2 + empleado.correoElectronico + del1 + empleado.Password + del1 + empleado.Puesto + "&" + correo + "&" + NoIdentidadViejo + "&" + Administrador.ToString();

            return m_Enviar;
        }

        //Modificar el perfil de un usuario accediendo a la tabla temporal
        public String ModificarPerfilUsuario(String NoIdentidad)
        {
            m_Enviar = NoIdentidad;

            return m_Enviar;
        }

        //Modificar una motivo
        public String ModificarMotivo(String motivo, String motivo_v, String correo)
        {
            if (motivo.Equals("") || correo.Equals("") || motivo_v.Equals(""))
                return null;

            string m_Enviar = motivo_v + "," + motivo + "&" + correo;

            return m_Enviar;
        }

        //Modificar una ocupacion
        public String ModificarOcupacion(String ocupacion, String ocupacion_v, String correo)
        {
            if (ocupacion.Equals("") || correo.Equals("") || ocupacion_v.Equals(""))
                return null;

            string m_Enviar = ocupacion_v + "," + ocupacion + "&" + correo;

            return m_Enviar;
        }

        //Modificar un interes
        public String ModificarInteres(Interes intereses, String interes_v, String correo)
        {
            if (interes_v.Equals("") || correo.Equals("") || intereses == null)
                return null;

            char del1 = ',';

            m_Enviar = interes_v + "," + intereses.Nombre + del1 + intereses.Tasa + del1 + intereses.Aplica_obligatoria + del1 + intereses.Aplica_voluntaria + del1
                + intereses.Aplica_obligatoria_especial + del1 + intereses.Aplica_voluntaria_arriba + del1 + intereses.Monto_voluntaria + "&" + correo;

            return m_Enviar;
        }

        //Modificar una parentesco
        public String ModificarParentesco(String parentesco, String parentesco_v, String correo)
        {
            if (parentesco.Equals("") || correo.Equals("") || parentesco_v.Equals(""))
                return null;

            string m_Enviar = parentesco_v + "," + parentesco + "&" + correo;

            return m_Enviar;
        }

        //Modificar todos los permisos
        public String ModificarPermisos(List<Boolean> admin, List<Boolean> empleado, List<Boolean> afiliado, String correo)
        {
            if (admin == null || correo.Equals("") || empleado == null || afiliado == null)
                return null;

            char del1 = ',', del2 = ';';

            m_Enviar = "";

            //Ingresa los permisos de administrador
            for (int i = 0; i < admin.Count; i++)
            {
                if (i < admin.Count - 1)
                    m_Enviar += admin[i].ToString() + del1;
                else if (i < admin.Count)
                    m_Enviar += admin[i].ToString() + del2;
            }

            //Ingresa los permisos de empleado
            for (int i = 0; i < empleado.Count; i++)
            {
                if (i < empleado.Count - 1)
                    m_Enviar += empleado[i].ToString() + del1;
                else if (i < empleado.Count)
                    m_Enviar += empleado[i].ToString() + del2;
            }

            //Ingresa los permisos de afiliado
            for (int i = 0; i < afiliado.Count; i++)
            {
                if (i < afiliado.Count - 1)
                    m_Enviar += afiliado[i].ToString() + del1;
                else if (i < afiliado.Count)
                    m_Enviar += afiliado[i].ToString() + del2;
            }

            m_Enviar += "&" + correo;

            return m_Enviar;
        }

        /*********************************************************************************************************************************
         **************************************************FIN******MODIFICAR*************************************************************
         *********************************************************************************************************************************/

        /*********************************************************************************************************************************
         *****************************************************************GETS************************************************************
         *********************************************************************************************************************************/

        //Obtiene string de DT y lo convierte a un afiliado
        public Afiliado getAfiliado(String valores)
        {
            if (valores.Equals(""))
                return null;

            Afiliado afiliado = new Afiliado();

            String [] tmp = valores.Split('~');
            afiliado.EstadoAfiliado = tmp[1];

            valores = tmp[0];

            afiliado.certificadoCuenta = Convert.ToInt32(valores.Split(',')[0]);

            int posicion = 0;

            for (int i = 0; i < valores.Length; i++)
            {
                if (valores[i] == ',')
                {
                    posicion = i;
                    i = valores.Length;
                }
            }

            valores = valores.Substring(posicion + 1);

            /*obtener datos de afiliado*/
            string[] datos = valores.Split(';');

            /*
             * DATOS PERSONALES:
             * identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), 
             * fecha nacimiento(string), estado civil(string), genero(string),direccion(string);
            */
            string[] datos_personales = datos[0].Split(',');
            afiliado.identidad = datos_personales[0]; afiliado.primerNombre = datos_personales[1]; afiliado.segundoNombre = datos_personales[2];
            afiliado.primerApellido = datos_personales[3]; afiliado.segundoApellido = datos_personales[4]; afiliado.fechaNacimiento = this.convertirFechaNormal(datos_personales[5]);
            afiliado.estadoCivil = datos_personales[6]; afiliado.genero = datos_personales[7]; afiliado.direccion = datos_personales[8];

            /*
             * DATOS PERSONALES:
             * telefono personal(string);
             * aqui son varios asi que por eso aparte
            */
            string[] telefonos = datos[1].Split(',');
            List<string> telefono = new List<string>();

            for (int i = 0; i < telefonos.Length; i++)
                telefono.Add(telefonos[i]);

            afiliado.telefonoPersonal = telefono;

            /*
             * DATOS PERSONALES:
             * telefono celular(string);
             * aqui son varios asi que por eso aparte
            */
            string[] celulares = datos[2].Split(',');
            List<string> celular = new List<string>();

            for (int i = 0; i < celulares.Length; i++)
                celular.Add(celulares[i]);

            afiliado.celular = celular;

            /*
             * DATOS LABORALES:
             * correo electronico(string), password(string 32 caracteres), nombre empresa(string), telefono empresa(string), direccion empresa(string),
             * fecha ingreso cooperativa(string), depto empresa(string), lugar nacimiento(string), ocupacion(stirng);
            */
            string[] datos_laborales = datos[4].Split(',');
            string[] login = datos[3].Split(',');
            afiliado.CorreoElectronico = login[0]; afiliado.NombreEmpresa = datos_laborales[0]; afiliado.TelefonoEmpresa = datos_laborales[1];
            afiliado.DireccionEmpresa = datos_laborales[2]; afiliado.Password = login[1];
            afiliado.fechaIngresoCooperativa = this.convertirFechaNormal(datos_laborales[3]); afiliado.DepartamentoEmpresa = datos_laborales[4];
            afiliado.lugarDeNacimiento = datos_laborales[5]; afiliado.Ocupacion = datos[5];

            /*
             * DATOS BENEFICIARIO DE CONTINGENCIA:
             * No. Identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), fecha_nac(string), estado_Civil(string), genero(string),
             * direccion(string), parentesco(string);
            */
            string[] beneficiario_cont = datos[6].Split(',');
            afiliado.BeneficiarioCont.identidad = beneficiario_cont[0]; afiliado.BeneficiarioCont.primerNombre = beneficiario_cont[1];
            afiliado.BeneficiarioCont.segundoNombre = beneficiario_cont[2]; afiliado.BeneficiarioCont.primerApellido = beneficiario_cont[3];
            afiliado.BeneficiarioCont.segundoApellido = beneficiario_cont[4]; afiliado.BeneficiarioCont.fechaNacimiento = this.convertirFechaNormal(beneficiario_cont[5]);
            afiliado.BeneficiarioCont.estadoCivil = beneficiario_cont[6]; afiliado.BeneficiarioCont.genero = beneficiario_cont[7];
            afiliado.BeneficiarioCont.direccion = beneficiario_cont[8]; afiliado.BeneficiarioCont.Parentesco = beneficiario_cont[9];

            /*
             * DATOS BENEFICIARIOS NORMALES:
             * No. Identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), fecha_nac(string), estado_Civil(string), genero(string),
             * direccion(string), parentesco(string), porcentaje seguros(Int64), porcentaje aportacion(Int64);
            */

            List<BeneficiarioNormal> beneficiarios = new List<BeneficiarioNormal>();

            for (int i = 7; i < datos.Length; i++)
            {
                BeneficiarioNormal temp = new BeneficiarioNormal();
                string[] normal = datos[i].Split(',');
                if (datos[i] == "")
                    break;
                temp.identidad = normal[0]; temp.primerNombre = normal[1]; temp.segundoNombre = normal[2]; temp.primerApellido = normal[3];
                temp.segundoApellido = normal[4]; temp.fechaNacimiento = this.convertirFechaNormal(normal[5]);
                temp.estadoCivil = normal[6]; temp.genero = normal[7]; temp.direccion = normal[8]; temp.Parentesco = normal[9];
                temp.porcentajeSeguros = (float)Convert.ToDouble(normal[10]); temp.porcentajeAportaciones = (float)Convert.ToDouble(normal[11]);

                beneficiarios.Add(temp);
            }
            afiliado.bensNormales = beneficiarios;

            return afiliado;
        }

        //Obtiene string de DT y lo convierte a un empleado
        public Empleado getEmpleado(String valores)
        {
            if (valores.Equals(""))
                return null;

            Empleado empleado = new Empleado();

            /*obtener datos de empleado*/
            string[] datos2 = valores.Split('~'), datos = datos2[0].Split(';');

            empleado.Administrador = datos2[1];
            /*
             * DATOS PERSONALES:
             * identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), 
             * fecha nacimiento(string), estado civil(string), genero(string),direccion(string);
            */
            string[] datos_personales = datos[0].Split(',');
            empleado.identidad = datos_personales[0]; empleado.primerNombre = datos_personales[1]; empleado.segundoNombre = datos_personales[2];
            empleado.primerApellido = datos_personales[3]; empleado.segundoApellido = datos_personales[4]; empleado.fechaNacimiento = this.convertirFechaNormal(datos_personales[5]);
            empleado.estadoCivil = datos_personales[6]; empleado.genero = datos_personales[7]; empleado.direccion = datos_personales[8];

            /*
             * DATOS PERSONALES:
             * telefono personal(string);
             * aqui son varios asi que por eso aparte
            */
            string[] telefonos = datos[1].Split(',');
            List<string> telefono = new List<string>();

            for (int i = 0; i < telefonos.Length; i++)
                telefono.Add(telefonos[i]);

            empleado.telefonoPersonal = telefono;

            /*
             * DATOS PERSONALES:
             * telefono celular(string);
             * aqui son varios asi que por eso aparte
            */
            string[] celulares = datos[2].Split(',');
            List<string> celular = new List<string>();

            for (int i = 0; i < celulares.Length; i++)
                celular.Add(celulares[i]);

            empleado.celular = celular;

            /*
             * DATOS LABORALES:
             * correo electronico(string)
            */
            string[] login = datos[3].Split(',');
            empleado.correoElectronico = login[0]; empleado.Password = login[1];
            empleado.Puesto = login[2];

            return empleado;
        }

        //Obtiene string de DT y devuelve lista de ocupacion
        public List<String[]> getOcupacion(String valores)
        {
            if (valores.Equals(""))
                return null;

            /*obtener datos de ocupacion*/
            String[] datos = valores.Split(';'), datos2;

            List<String[]> lista = new List<String[]>();

            for (int i = 0; i < datos.Length; i++)
            {
                datos2 = datos[i].Split(',');
                lista.Add(datos2);
            }

            return lista;
        }

        //Obtiene string de DT y devuelve lista de parentesco
        public List<String[]> getParentesco(String valores)
        {
            if (valores.Equals(""))
                return null;

            /*obtener datos de parentesco*/
            String[] datos = valores.Split(';'), datos2;

            List<String[]> lista = new List<String[]>();

            for (int i = 0; i < datos.Length; i++)
            {
                datos2 = datos[i].Split(',');
                lista.Add(datos2);
            }

            return lista;
        }

        //Obtiene string de DT y devuelve lista de intereses
        public List<Interes> getIntereses(String valores)
        {
            if (valores.Equals(""))
                return null;

            /*obtener datos de intereses*/
            string[] datos = valores.Split(';');

            List<Interes> lista = new List<Interes>();
            for (int i = 0; i < datos.Length; i++)
            {
                string[] datos2 = datos[i].Split(',');
                Interes tmp = new Interes();

                tmp.Aplica_obligatoria = Convert.ToBoolean(datos2[2]); tmp.Aplica_voluntaria = Convert.ToBoolean(datos2[3]);
                tmp.Aplica_obligatoria_especial = Convert.ToBoolean(datos2[4]); tmp.Aplica_voluntaria_arriba = Convert.ToBoolean(datos2[5]);
                tmp.Nombre = datos2[0]; tmp.Tasa = Convert.ToInt64(datos2[1]); tmp.Monto_voluntaria = Convert.ToInt64(datos2[6]);
                tmp.Valido = Convert.ToBoolean(datos2[7]);

                lista.Add(tmp);
            }

            return lista;
        }

        //Obtiene string de DT y devuelve lista de motivos
        public List<String[]> getMotivos(String valores)
        {
            if (valores.Equals(""))
                return null;

            /*obtener datos de motivos*/
            String[] datos = valores.Split(';'), datos2;

            List<String[]> lista = new List<String[]>();

            for (int i = 0; i < datos.Length; i++)
            {
                datos2 = datos[i].Split(',');
                lista.Add(datos2);
            }

            return lista;
        }

        //Obtiene string de DT y devuelve monto actual
        public String getMontoActual(String valores)
        {
            if (valores.Equals(""))
                return null;

            return valores;
        }

        //Obtiene string de DT y devuelve limite actual
        public String getLimiteActual(String valores)
        {
            if (valores.Equals(""))
                return null;

            return valores;
        }

        //verificar si se puede login
        public String verificarLogin(String correo, String pwd)
        {
            if (correo.Equals("") || pwd.Equals(""))
                return null;

            MD5 md5 = new MD5();
            char del1 = ','; md5.Value = pwd;

            m_Enviar = correo + del1 + md5.FingerPrint;

            return m_Enviar;
        }

        //Obtiene nombre completo y permisos de un usuario  
        public List<String> getLoginDatos(String valores)
        {
            if (valores.Equals(""))
                return null;

            /*obtener datos de permisos*/
            string[] datos = valores.Split(';'), datos2;

            List<string> lista = new List<string>();

            for (int i = 0; i < datos.Length; i++)
            {
                datos2 = datos[i].Split(',');
                for (int j = 0; j < datos2.Length; j++)
                    lista.Add(datos2[j]);
            }

            return lista;
        }

        //Obtiene No. Certificado, PNombre, SNombre, PApellido, SApellido, No. Identidad afiliado y aportaciones para ingresar aportaciones
        public List<Object> getAfiliadoAportaciones(String valores)
        {
            if (valores.Equals(""))
                return null;

            List<Object> lista = new List<Object>();
            aportacionObligatoriaEspecial aportacion;

            /*obtener datos de las aportaciones*/
            string[] datos = valores.Split('&'), datos2 = datos[0].Split(';'), datos3 = datos2[1].Split(',');

            /*Obtiene: No. Certificado, PNombre, SNombre, PApellido, SApellido, No. Identidad*/
            lista.Add(datos2[0]);
            for (int i = 0; i < datos3.Length; i++)
                lista.Add(datos3[i]);
            lista.Add(datos2[2]);

            if (datos[1].Equals(""))
            {
                lista.Add("NULL");
                return lista;
            }
            //Obtiene datos de las aportaicones: ID_Aportacion, fecha a pagar, monto, motivo ("")
            datos2 = datos[1].Split(';');
            for (int i = 0; i < datos2.Length; i++)
            {
                datos3 = datos2[i].Split(',');
                aportacion = new aportacionObligatoriaEspecial();

                aportacion.IDAportacion = Convert.ToInt32(datos3[0]); aportacion.fechaPlazo = datos3[1]; aportacion.Monto = Convert.ToInt64(datos3[2]);
                if (datos3[3].Equals(""))
                    aportacion.Motivo = "N/A";
                aportacion.Motivo = datos3[3];

                lista.Add(aportacion);
            }

            return lista;
        }

        //Obtiene string de DT y devuelve lista Aportacion Obligatoria Especial
        public List<AportacionOE> getAportacionObligatoriaEspecial(String valores)
        {
            if (valores.Equals(""))
                return null;

            List<AportacionOE> lista = new List<AportacionOE>();
            AportacionOE aportacion;

            /*obtener datos de las aportaciones*/
            string[] datos = valores.Split(';'), datos2;

            //Obtiene datos de las aportaicones: ID_Aportacion, fecha a pagar, monto, motivo (""), descripcion
            for (int i = 0; i < datos.Length; i++)
            {
                datos2 = datos[i].Split(',');
                aportacion = new AportacionOE();

                aportacion.Descripcion = datos2[4]; aportacion.Fecha = datos2[2] + "/" + datos2[1]; aportacion.Monto = Convert.ToInt64(datos2[0]);
                if (datos2[3].Equals(""))
                    aportacion.Motivo = "N/A";
                aportacion.Motivo = datos2[3];

                lista.Add(aportacion);
            }

            return lista;
        }

        //Obtiene el afiliado apartir de un nombre, numero certificado o identidad
        public String getAfiliadoAportacion(String Certificado, String PNombre, String SNombre, String PApellido, String SApellido, String Identidad)
        {
            char del1 = ',', del2 = ';';

            m_Enviar = Certificado + del2 + PNombre + del1 + SNombre + del1 + PApellido + del1 + SApellido + del2 + Identidad;

            return m_Enviar;
        }

        //Ingresar las aportaciones seleccionadas por usuario
        public String Pagar(String NoIdentidad, List<controlDePagoGrid> aportaciones, String correo)
        {
            if (correo.Equals("") || NoIdentidad.Equals("") || aportaciones == null)
                return null;

            char del1 = ',', del2 = ';';

            //Agrega el numero de identidad
            m_Enviar = NoIdentidad + del2;

            List<String> ids = new List<String>();
            List<AportacionVoluntaria> voluntarias = new List<AportacionVoluntaria>();
            AportacionVoluntaria tmp;

            for (int i = 0; i < aportaciones.Count; i++)
            {
                if (aportaciones[i].TipoAportacion.Equals("Obligatoria"))
                    ids.Add("" + aportaciones[i].ID);
                else
                {
                    tmp = new AportacionVoluntaria();
                    tmp.Monto = aportaciones[i].Monto; tmp.Descripcion = aportaciones[i].Descripcion;

                    voluntarias.Add(tmp);
                }
            }

            //Agrega los IDs de las aportaciones obligatorias
            for (int i = 0; i < ids.Count; i++)
            {
                if (i < ids.Count - 1)
                    m_Enviar += ids[i] + del1;
                else if (i < ids.Count)
                    m_Enviar += ids[i];
            }
            m_Enviar += del2;

            //Agrega el monto y la descripcion de las aportaciones voluntarias
            for (int i = 0; i < voluntarias.Count; i++)
            {
                if (i < voluntarias.Count - 1)
                    m_Enviar += voluntarias[i].Monto + "," + voluntarias[i].Descripcion + ";";
                else if (i < voluntarias.Count)
                    m_Enviar += voluntarias[i].Monto + "," + voluntarias[i].Descripcion;
            }
            m_Enviar += "&" + correo;

            return m_Enviar;
        }

        //Obtener listas de No. Certificado y Nombres de afiliados
        public List<Object> getListasPagos(String valores)
        {
            if (valores.Equals(""))
                return null;

            List<Object> lista = new List<Object>();
            List<String> nombres = new List<String>(), No_Certificado = new List<String>();
            String tmp;

            /*obtener datos de las aportaciones*/
            string[] datos = valores.Split(';'), datos2;

            /*Obtiene: No. Certificado, PNombre, SNombre, PApellido, SApellido*/
            for (int i = 0; i < datos.Length; i++)
            {
                tmp = "";
                datos2 = datos[i].Split(',');
                for (int j = 0; j < datos2.Length; j++)
                {
                    if (j == 0)
                        No_Certificado.Add(datos2[j]);
                    if (j > 0 && j < datos2.Length - 1)
                        tmp += datos2[j] + " ";
                    else if (j > 0 && j < datos2.Length)
                        tmp += datos2[j];
                }
                nombres.Add(tmp);
            }

            lista.Add(No_Certificado); lista.Add(nombres);

            return lista;
        }

        //Filtros de busqueda de un afiliado
        public String buscarAfiliadosConsultas(String Certificado, String PNombre, String SNombre, String PApellido, String SApellido, String Identidad,
            String Telefono, String Celular, String Ocupacion, String Genero, String Correo, String Estado_civil, String Estado_afiliado, String Empresa)
        {
            char del1 = ',';

            //Agrega el no certificado, PNombre, SNombre, PApellido, SApellido, No Identidad, Telefono, 
            //Celular, Ocupacion, Genero, Correo, Estado Civil, Estado afiliado, Empresa
            m_Enviar = Certificado + del1 + PNombre + del1 + SNombre + del1 + PApellido + del1 + SApellido + del1 + Identidad + del1 + Telefono + del1 + Celular + del1 + Ocupacion + del1 + Genero + del1
                + Correo + del1 + Estado_civil + del1 + Estado_afiliado + del1 + Empresa;

            return m_Enviar;
        }

        //Devuelve una lista de los Afiliados filtrados
        public List<filtrosAfiliadoGrid> getListaAfiliadosFiltrados(String valores)
        {
            if (valores.Equals(""))
                return null;

            List<filtrosAfiliadoGrid> lista = new List<filtrosAfiliadoGrid>();
            List<String> tmp;

            /*obtener datos de las aportaciones*/
            string[] datos = valores.Split(';'), datos2;

            /*Obtiene: No. Certificado, PNombre, SNombre, PApellido, SApellido, No. Identidad, Correo*/
            for (int i = 0; i < datos.Length; i++)
            {
                datos2 = datos[i].Split(',');
                tmp = new List<String>();
                for (int j = 0; j < datos2.Length; j++)
                    tmp.Add(datos2[j]);
                lista.Add(new filtrosAfiliadoGrid
                {
                    No_Certificado = tmp[0],
                    Nombre = tmp[1] + " " + tmp[2] + " " + tmp[3] + " " + tmp[4],
                    No_Identidad = tmp[5],
                    Correo = tmp[6]
                });
            }

            return lista;
        }

        //Obtiene filtro de busqueda en consultar estado de cuenta
        public String enviarAportacionesConsulas(String Certificado, String fecha_desde, String fecha_hasta)
        {
            char del1 = ',';

            //Agrega el no certificado, fecha desde, fecha hasta
            m_Enviar = Certificado + del1 + this.convertirFechaBD(fecha_desde) + del1 + this.convertirFechaBD(fecha_hasta);

            return m_Enviar;
        }

        //Obtiene todas las aportaciones del afiliado
        public List<estadoDeCuentaGrid> getEstadoCuenta(String valores)
        {
            if (valores.Equals(""))
                return null;

            List<estadoDeCuentaGrid> lista = new List<estadoDeCuentaGrid>(); ;
            estadoDeCuentaGrid aportacion;

            /*obtener datos de las aportaciones*/
            string[] datos = valores.Split(';'), datos2;

            /*Obtiene: Monto, Descripcion, Fecha Plazo, Fecha Realizacion, Motivo ("")*/
            for (int i = 0; i < datos.Length; i++)
            {
                datos2 = datos[i].Split(',');
                aportacion = new estadoDeCuentaGrid();

                aportacion.Monto = Convert.ToInt32(datos2[0]); aportacion.Descripcion = datos2[1]; aportacion.fechaPlazo = datos2[2];
                if (datos2[3].Equals(""))
                    aportacion.fechaRealizacion = "No pagada.";
                aportacion.fechaRealizacion = datos2[3];
                if (datos2[4].Equals(""))
                    aportacion.Motivo = "N/A";
                aportacion.Motivo = datos2[4];

                lista.Add(aportacion);
            }

            return lista;
        }

        //Obtener todos los permisos de roles
        public List<List<Boolean>> getTodosLosPermisos(String valores)
        {
            if (valores.Equals(""))
                return null;

            List<List<Boolean>> lista = new List<List<Boolean>>();
            List<Boolean> admin = new List<Boolean>(), empleado = new List<Boolean>(), afiliado = new List<Boolean>();

            /*obtener datos de los permisos*/
            string[] datos = valores.Split(';'), datos2;

            /*Obtiene: permisos Admin, Empleado, Afiliado*/
            for (int i = 0; i < datos.Length; i++)
            {
                datos2 = datos[i].Split(',');
                for (int j = 0; j < datos2.Length; j++)
                {
                    if (i == 0)
                        admin.Add(Convert.ToBoolean(datos2[j]));
                    else if (i == 1)
                        empleado.Add(Convert.ToBoolean(datos2[j]));
                    else if (i == 2)
                        afiliado.Add(Convert.ToBoolean(datos2[j]));
                }
            }

            lista.Add(admin); lista.Add(empleado); lista.Add(afiliado);

            return lista;
        }

        //Obtener aportaciones a capitalizar
        public List<aportacionCapitalizarGrid> getAportacionesACapitalizar(String valores)
        {
            if (valores.Equals(""))
                return null;

            List<aportacionCapitalizarGrid> lista = new List<aportacionCapitalizarGrid>();
            aportacionCapitalizarGrid reporte;

            /*obtener datos de las aportaciones*/
            string[] datos = valores.Split(';'), datos2;

            /*Obtiene: No. Certificado, Descripcion, Monto, Fecha Plazo, Fecha Realizacion, Motivo, Nombre Interes, Tasa Interes, Monto generado*/
            for (int i = 0; i < datos.Length; i++)
            {
                datos2 = datos[i].Split(',');
                reporte = new aportacionCapitalizarGrid();

                String[] datos3 = datos2[3].Split(' '), datos4 = datos2[4].Split(' ');

                reporte.No_Certificado = datos2[0]; reporte.Descripcion_Aportacion = datos2[1]; reporte.Monto = datos2[2];
                reporte.Fecha_Plazo = datos3[0]; reporte.Fecha_Realizacion = datos4[0];
                reporte.Motivo = datos2[5]; reporte.Nombre_Interes = datos2[6]; reporte.Tasa_Interes = datos2[7]; reporte.Monto_Generado = datos2[8];

                lista.Add(reporte);
            }

            return lista;
        }

        //Obtener saldos a capitalizar
        public List<saldoCapitalizarGrid> getSaldosACapitalizar(String valores)
        {
            if (valores.Equals(""))
                return null;

            List<saldoCapitalizarGrid> lista = new List<saldoCapitalizarGrid>();
            saldoCapitalizarGrid reporte;

            /*obtener datos de los saldos*/
            string[] datos = valores.Split(';'), datos2;

            /*Obtiene: No. Certificado, Saldo Actual, Total Interes, Nuevo Saldo*/
            for (int i = 0; i < datos.Length; i++)
            {
                datos2 = datos[i].Split(',');
                reporte = new saldoCapitalizarGrid();

                reporte.No_Certificado = datos2[0]; reporte.Saldo_Actual = datos2[1]; reporte.Total_Interes = datos2[2];
                reporte.Nuevo_Saldo = datos2[3];

                lista.Add(reporte);
            }

            return lista;
        }

        //para enviar a DT los afiliados filtrados, tipo de aportacion, fecha desde, fecha hasta
        public String getFiltrosEstadoCuentaTodos(List<String> certificados, String tipo_aportacion, String fecha_desde, String fecha_hasta)
        {
            if (tipo_aportacion.Equals("") || certificados == null || fecha_desde.Equals("") || fecha_hasta.Equals(""))
                return null;

            char del1 = ',', del2 = ';';

            m_Enviar = "";
            for (int i = 0; i < certificados.Count; i++)
            {
                if (i < certificados.Count - 1)
                    m_Enviar += certificados[i] + del1;
                else if (i < certificados.Count)
                    m_Enviar += certificados[i];
            }

            char t_aportacion = ' ';

            switch (tipo_aportacion)
            {
                case "Todas Las Aportaciones":
                    t_aportacion = 'T';
                    break;
                case "Aportaciones Obligatorias Normales":
                    t_aportacion = 'O';
                    break;
                case "Aportaciones Obligatorias Especiales":
                    t_aportacion = 'E';
                    break;
                case "Aportaciones Obligatorias":
                    t_aportacion = 'Y';
                    break;
                case "Aportaciones Voluntarias":
                    t_aportacion = 'V';
                    break;
            }

            m_Enviar += del2 + "" + t_aportacion + "" + del2 + this.convertirFechaBD(fecha_desde) + del1 + this.convertirFechaBD(fecha_hasta);

            return m_Enviar;
        }

        //Obtener lista del reporte de los saldos de todos los afiliados
        public List<reporteTodosSaldosGrid> getReporteTodosLosSaldos(String valores)
        {
            if (valores.Equals(""))
                return null;

            List<reporteTodosSaldosGrid> lista = new List<reporteTodosSaldosGrid>();
            reporteTodosSaldosGrid reporte;

            /*obtener datos de los saldos*/
            string[] datos = valores.Split(';'), datos2;

            /*Obtiene: No. Certificado, PNombre, SNombre, PApellido, SApellido, Total Aportaciones O, Total Aportaciones V, Saldo Actual*/
            for (int i = 0; i < datos.Length; i++)
            {
                datos2 = datos[i].Split(',');
                reporte = new reporteTodosSaldosGrid();

                reporte.Num_Certificado = Convert.ToInt32(datos2[0]); reporte.Nombre = datos2[1] + " " + datos2[2] + " " + datos2[3] + " " + datos2[4];
                reporte.Total_AportacionesO = (float)Convert.ToDouble(datos2[5]);
                reporte.Total_AportacionesV = (float)Convert.ToDouble(datos2[6]);
                reporte.Total_Intereses = (float)Convert.ToDouble(datos2[7]);
                reporte.Saldo_Actual = (float)Convert.ToDouble(datos2[8]);

                lista.Add(reporte);
            }

            return lista;
        }

        //Obtener lista del reporte de los estado de cuenta de todos los afiliados
        public List<reporteEstadoCuentaTodosGrid> getReporteEstadoCuentaTodos(String valores)
        {
            if (valores.Equals(""))
                return null;

            List<reporteEstadoCuentaTodosGrid> lista = new List<reporteEstadoCuentaTodosGrid>();
            reporteEstadoCuentaTodosGrid reporte;

            /*obtener datos de los saldos*/
            string[] datos = valores.Split(';'), datos2;

            /*Obtiene: No. Certificado, Monto, Descripcion, fecha de plazo, fecha de realizacion, Motivo, Cancelado*/
            for (int i = 0; i < datos.Length; i++)
            {
                datos2 = datos[i].Split(',');
                reporte = new reporteEstadoCuentaTodosGrid();

                reporte.Num_Certificado = Convert.ToInt32(datos2[0]); reporte.Monto = (float)Convert.ToDouble(datos2[1]); reporte.Descripcion = datos2[2];
                reporte.fechaPlazo = datos2[3]; reporte.fechaRealizacion = datos2[4]; reporte.Motivo = datos2[5]; reporte.Cancelado = datos2[6];

                lista.Add(reporte);
            }

            return lista;
        }

        //Obtener correos de persona temporal para llenar combobox
        public List<String> getPersonaTemprales(String valores)
        {
            if (valores.Equals(""))
                return null;

            /*obtener datos de correos*/
            string[] datos = valores.Split(',');

            List<String> lista = new List<String>();

            for (int i = 0; i < datos.Length; i++)
                lista.Add(datos[i]);

            return lista;
        }

        /*********************************************************************************************************************************
         **********************************************************FIN****GETS************************************************************
         *********************************************************************************************************************************/

        //Convierte la fecha del sistema a formato de BD con el formato: AÑO-MES-DIA
        private String convertirFechaBD(String fecha)
        {
            string[] fecha_corregida = fecha.Split('/');
            fecha = "";

            fecha += fecha_corregida[2] + "-";
            fecha += fecha_corregida[0] + "-";
            fecha += fecha_corregida[1];

            return fecha;
        }

        //Convierte la fecha de la a BD a formato de BD con el formato: MES/DIA/AÑO
        private String convertirFechaNormal(String fecha)
        {
            string[] fecha_corregida = fecha.Split('-');
            fecha = "";

            fecha += fecha_corregida[1] + "/";
            fecha += fecha_corregida[0] + "/";
            fecha += fecha_corregida[2];

            return fecha;
        }

        /*********************************************************************************************************************************
         ***********************************************************MD5****dAnY************************************************************
         *********************************************************************************************************************************/

        private class MD5
        {
            /// <summary>
            /// helper class providing suporting function
            /// </summary>
            sealed private class MD5Helper
            {
                private MD5Helper() { }

                /// <summary>
                /// Left rotates the input word
                /// </summary>
                /// <param name="uiNumber">a value to be rotated</param>
                /// <param name="shift">no of bits to be rotated</param>
                /// <returns>the rotated value</returns>
                public static uint RotateLeft(uint uiNumber, ushort shift)
                {
                    return ((uiNumber >> 32 - shift) | (uiNumber << shift));
                }

                /// <summary>
                /// perform a ByteReversal on a number
                /// </summary>
                /// <param name="uiNumber">value to be reversed</param>
                /// <returns>reversed value</returns>
                public static uint ReverseByte(uint uiNumber)
                {
                    return (((uiNumber & 0x000000ff) << 24) |
                                (uiNumber >> 24) |
                            ((uiNumber & 0x00ff0000) >> 8) |
                            ((uiNumber & 0x0000ff00) << 8));
                }
            }

            /// <summary>
            /// class for changing event args
            /// </summary>
            public class MD5ChangingEventArgs : EventArgs
            {
                public readonly byte[] NewData;

                public MD5ChangingEventArgs(byte[] data)
                {
                    byte[] NewData = new byte[data.Length];
                    for (int i = 0; i < data.Length; i++)
                        NewData[i] = data[i];
                }

                public MD5ChangingEventArgs(string data)
                {
                    byte[] NewData = new byte[data.Length];
                    for (int i = 0; i < data.Length; i++)
                        NewData[i] = (byte)data[i];
                }

            }

            /// <summary>
            /// class for cahnged event args
            /// </summary>
            public class MD5ChangedEventArgs : EventArgs
            {
                public readonly byte[] NewData;
                public readonly string FingerPrint;

                public MD5ChangedEventArgs(byte[] data, string HashedValue)
                {
                    byte[] NewData = new byte[data.Length];
                    for (int i = 0; i < data.Length; i++)
                        NewData[i] = data[i];
                    FingerPrint = HashedValue;
                }

                public MD5ChangedEventArgs(string data, string HashedValue)
                {
                    byte[] NewData = new byte[data.Length];
                    for (int i = 0; i < data.Length; i++)
                        NewData[i] = (byte)data[i];

                    FingerPrint = HashedValue;
                }
            }

            /// <summary>
            /// constants for md5
            /// </summary>
            public enum MD5InitializerConstant : uint
            {
                A = 0x67452301,
                B = 0xEFCDAB89,
                C = 0x98BADCFE,
                D = 0X10325476
            }

            /// <summary>
            /// Represent digest with ABCD
            /// </summary>
            sealed public class Digest
            {
                public uint A;
                public uint B;
                public uint C;
                public uint D;

                public Digest()
                {
                    A = (uint)MD5InitializerConstant.A;
                    B = (uint)MD5InitializerConstant.B;
                    C = (uint)MD5InitializerConstant.C;
                    D = (uint)MD5InitializerConstant.D;
                }

                public override string ToString()
                {
                    string st;
                    st = MD5Helper.ReverseByte(A).ToString("X8") +
                        MD5Helper.ReverseByte(B).ToString("X8") +
                        MD5Helper.ReverseByte(C).ToString("X8") +
                        MD5Helper.ReverseByte(D).ToString("X8");
                    return st;

                }
            }

            /***********************Statics**************************************/
            /// <summary>
            /// lookup table 4294967296*sin(i)
            /// </summary>
            protected readonly static uint[] T = new uint[64] 
			{	0xd76aa478,0xe8c7b756,0x242070db,0xc1bdceee,
				0xf57c0faf,0x4787c62a,0xa8304613,0xfd469501,
                0x698098d8,0x8b44f7af,0xffff5bb1,0x895cd7be,
                0x6b901122,0xfd987193,0xa679438e,0x49b40821,
				0xf61e2562,0xc040b340,0x265e5a51,0xe9b6c7aa,
                0xd62f105d,0x2441453,0xd8a1e681,0xe7d3fbc8,
                0x21e1cde6,0xc33707d6,0xf4d50d87,0x455a14ed,
				0xa9e3e905,0xfcefa3f8,0x676f02d9,0x8d2a4c8a,
                0xfffa3942,0x8771f681,0x6d9d6122,0xfde5380c,
                0xa4beea44,0x4bdecfa9,0xf6bb4b60,0xbebfbc70,
                0x289b7ec6,0xeaa127fa,0xd4ef3085,0x4881d05,
				0xd9d4d039,0xe6db99e5,0x1fa27cf8,0xc4ac5665,
                0xf4292244,0x432aff97,0xab9423a7,0xfc93a039,
                0x655b59c3,0x8f0ccc92,0xffeff47d,0x85845dd1,
                0x6fa87e4f,0xfe2ce6e0,0xa3014314,0x4e0811a1,
				0xf7537e82,0xbd3af235,0x2ad7d2bb,0xeb86d391
            };

            /*****instance variables**************/
            /// <summary>
            /// X used to proces data in 
            ///	512 bits chunks as 16 32 bit word
            /// </summary>
            protected uint[] X = new uint[16];

            /// <summary>
            /// the finger print obtained. 
            /// </summary>
            protected Digest dgFingerPrint;

            /// <summary>
            /// the input bytes
            /// </summary>
            protected byte[] m_byteInput;
            /**********************EVENTS AND DELEGATES*******************************************/

            public delegate void ValueChanging(object sender, MD5ChangingEventArgs Changing);
            public delegate void ValueChanged(object sender, MD5ChangedEventArgs Changed);

            public event ValueChanging OnValueChanging;
            public event ValueChanged OnValueChanged;

            /********************************************************************/
            /***********************PROPERTIES ***********************/
            /// <summary>
            ///gets or sets as string
            /// </summary>
            public string Value
            {
                get
                {
                    string st;
                    char[] tempCharArray = new Char[m_byteInput.Length];

                    for (int i = 0; i < m_byteInput.Length; i++)
                        tempCharArray[i] = (char)m_byteInput[i];

                    st = new String(tempCharArray);
                    return st;
                }
                set
                {
                    /// raise the event to notify the changing 
                    if (this.OnValueChanging != null)
                        this.OnValueChanging(this, new MD5ChangingEventArgs(value));


                    m_byteInput = new byte[value.Length];
                    for (int i = 0; i < value.Length; i++)
                        m_byteInput[i] = (byte)value[i];
                    dgFingerPrint = CalculateMD5Value();

                    /// raise the event to notify the change
                    if (this.OnValueChanged != null)
                        this.OnValueChanged(this, new MD5ChangedEventArgs(value, dgFingerPrint.ToString()));

                }
            }

            /// <summary>
            /// get/sets as  byte array 
            /// </summary>
            public byte[] ValueAsByte
            {
                get
                {
                    byte[] bt = new byte[m_byteInput.Length];
                    for (int i = 0; i < m_byteInput.Length; i++)
                        bt[i] = m_byteInput[i];
                    return bt;
                }
                set
                {
                    /// raise the event to notify the changing
                    if (this.OnValueChanging != null)
                        this.OnValueChanging(this, new MD5ChangingEventArgs(value));

                    m_byteInput = new byte[value.Length];
                    for (int i = 0; i < value.Length; i++)
                        m_byteInput[i] = value[i];
                    dgFingerPrint = CalculateMD5Value();


                    /// notify the changed  value
                    if (this.OnValueChanged != null)
                        this.OnValueChanged(this, new MD5ChangedEventArgs(value, dgFingerPrint.ToString()));
                }
            }

            //gets the signature/figner print as string
            public string FingerPrint
            {
                get { return dgFingerPrint.ToString(); }
            }

            /*************************************************************************/
            /// <summary>
            /// Constructor
            /// </summary>
            public MD5()
            {
                Value = "";
            }

            /******************************************************************************/
            /*********************METHODS**************************/

            /// <summary>
            /// calculat md5 signature of the string in Input
            /// </summary>
            /// <returns> Digest: the finger print of msg</returns>
            protected Digest CalculateMD5Value()
            {
                /***********vairable declaration**************/
                byte[] bMsg;	//buffer to hold bits
                uint N;			//N is the size of msg as  word (32 bit) 
                Digest dg = new Digest();			//  the value to be returned

                // create a buffer with bits padded and length is alos padded
                bMsg = CreatePaddedBuffer();

                N = (uint)(bMsg.Length * 8) / 32;		//no of 32 bit blocks

                for (uint i = 0; i < N / 16; i++)
                {
                    CopyBlock(bMsg, i);
                    PerformTransformation(ref dg.A, ref dg.B, ref dg.C, ref dg.D);
                }
                return dg;
            }

            /********************************************************
             * TRANSFORMATIONS :  FF , GG , HH , II  acc to RFC 1321
             * where each Each letter represnets the aux function used
             *********************************************************/

            /// <summary>
            /// perform transformatio using f(((b&c) | (~(b)&d))
            /// </summary>
            protected void TransF(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
            {
                a = b + MD5Helper.RotateLeft((a + ((b & c) | (~(b) & d)) + X[k] + T[i - 1]), s);
            }

            /// <summary>
            /// perform transformatio using g((b&d) | (c & ~d) )
            /// </summary>
            protected void TransG(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
            {
                a = b + MD5Helper.RotateLeft((a + ((b & d) | (c & ~d)) + X[k] + T[i - 1]), s);
            }

            /// <summary>
            /// perform transformatio using h(b^c^d)
            /// </summary>
            protected void TransH(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
            {
                a = b + MD5Helper.RotateLeft((a + (b ^ c ^ d) + X[k] + T[i - 1]), s);
            }

            /// <summary>
            /// perform transformatio using i (c^(b|~d))
            /// </summary>
            protected void TransI(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
            {
                a = b + MD5Helper.RotateLeft((a + (c ^ (b | ~d)) + X[k] + T[i - 1]), s);
            }

            /// <summary>
            /// Perform All the transformation on the data
            /// </summary>
            /// <param name="A">A</param>
            /// <param name="B">B </param>
            /// <param name="C">C</param>
            /// <param name="D">D</param>
            protected void PerformTransformation(ref uint A, ref uint B, ref uint C, ref uint D)
            {
                //// saving  ABCD  to be used in end of loop
                uint AA, BB, CC, DD;

                AA = A;
                BB = B;
                CC = C;
                DD = D;
                /* Round 1 
                    * [ABCD  0  7  1]  [DABC  1 12  2]  [CDAB  2 17  3]  [BCDA  3 22  4]
                    * [ABCD  4  7  5]  [DABC  5 12  6]  [CDAB  6 17  7]  [BCDA  7 22  8]
                    * [ABCD  8  7  9]  [DABC  9 12 10]  [CDAB 10 17 11]  [BCDA 11 22 12]
                    * [ABCD 12  7 13]  [DABC 13 12 14]  [CDAB 14 17 15]  [BCDA 15 22 16]
                    *  * */
                TransF(ref A, B, C, D, 0, 7, 1); TransF(ref D, A, B, C, 1, 12, 2); TransF(ref C, D, A, B, 2, 17, 3); TransF(ref B, C, D, A, 3, 22, 4);
                TransF(ref A, B, C, D, 4, 7, 5); TransF(ref D, A, B, C, 5, 12, 6); TransF(ref C, D, A, B, 6, 17, 7); TransF(ref B, C, D, A, 7, 22, 8);
                TransF(ref A, B, C, D, 8, 7, 9); TransF(ref D, A, B, C, 9, 12, 10); TransF(ref C, D, A, B, 10, 17, 11); TransF(ref B, C, D, A, 11, 22, 12);
                TransF(ref A, B, C, D, 12, 7, 13); TransF(ref D, A, B, C, 13, 12, 14); TransF(ref C, D, A, B, 14, 17, 15); TransF(ref B, C, D, A, 15, 22, 16);
                /** rOUND 2
                    **[ABCD  1  5 17]  [DABC  6  9 18]  [CDAB 11 14 19]  [BCDA  0 20 20]
                    *[ABCD  5  5 21]  [DABC 10  9 22]  [CDAB 15 14 23]  [BCDA  4 20 24]
                    *[ABCD  9  5 25]  [DABC 14  9 26]  [CDAB  3 14 27]  [BCDA  8 20 28]
                    *[ABCD 13  5 29]  [DABC  2  9 30]  [CDAB  7 14 31]  [BCDA 12 20 32]
                */
                TransG(ref A, B, C, D, 1, 5, 17); TransG(ref D, A, B, C, 6, 9, 18); TransG(ref C, D, A, B, 11, 14, 19); TransG(ref B, C, D, A, 0, 20, 20);
                TransG(ref A, B, C, D, 5, 5, 21); TransG(ref D, A, B, C, 10, 9, 22); TransG(ref C, D, A, B, 15, 14, 23); TransG(ref B, C, D, A, 4, 20, 24);
                TransG(ref A, B, C, D, 9, 5, 25); TransG(ref D, A, B, C, 14, 9, 26); TransG(ref C, D, A, B, 3, 14, 27); TransG(ref B, C, D, A, 8, 20, 28);
                TransG(ref A, B, C, D, 13, 5, 29); TransG(ref D, A, B, C, 2, 9, 30); TransG(ref C, D, A, B, 7, 14, 31); TransG(ref B, C, D, A, 12, 20, 32);
                /*  rOUND 3
                    * [ABCD  5  4 33]  [DABC  8 11 34]  [CDAB 11 16 35]  [BCDA 14 23 36]
                    * [ABCD  1  4 37]  [DABC  4 11 38]  [CDAB  7 16 39]  [BCDA 10 23 40]
                    * [ABCD 13  4 41]  [DABC  0 11 42]  [CDAB  3 16 43]  [BCDA  6 23 44]
                    * [ABCD  9  4 45]  [DABC 12 11 46]  [CDAB 15 16 47]  [BCDA  2 23 48]
                 * */
                TransH(ref A, B, C, D, 5, 4, 33); TransH(ref D, A, B, C, 8, 11, 34); TransH(ref C, D, A, B, 11, 16, 35); TransH(ref B, C, D, A, 14, 23, 36);
                TransH(ref A, B, C, D, 1, 4, 37); TransH(ref D, A, B, C, 4, 11, 38); TransH(ref C, D, A, B, 7, 16, 39); TransH(ref B, C, D, A, 10, 23, 40);
                TransH(ref A, B, C, D, 13, 4, 41); TransH(ref D, A, B, C, 0, 11, 42); TransH(ref C, D, A, B, 3, 16, 43); TransH(ref B, C, D, A, 6, 23, 44);
                TransH(ref A, B, C, D, 9, 4, 45); TransH(ref D, A, B, C, 12, 11, 46); TransH(ref C, D, A, B, 15, 16, 47); TransH(ref B, C, D, A, 2, 23, 48);
                /*ORUNF  4
                    *[ABCD  0  6 49]  [DABC  7 10 50]  [CDAB 14 15 51]  [BCDA  5 21 52]
                    *[ABCD 12  6 53]  [DABC  3 10 54]  [CDAB 10 15 55]  [BCDA  1 21 56]
                    *[ABCD  8  6 57]  [DABC 15 10 58]  [CDAB  6 15 59]  [BCDA 13 21 60]
                    *[ABCD  4  6 61]  [DABC 11 10 62]  [CDAB  2 15 63]  [BCDA  9 21 64]
                             * */
                TransI(ref A, B, C, D, 0, 6, 49); TransI(ref D, A, B, C, 7, 10, 50); TransI(ref C, D, A, B, 14, 15, 51); TransI(ref B, C, D, A, 5, 21, 52);
                TransI(ref A, B, C, D, 12, 6, 53); TransI(ref D, A, B, C, 3, 10, 54); TransI(ref C, D, A, B, 10, 15, 55); TransI(ref B, C, D, A, 1, 21, 56);
                TransI(ref A, B, C, D, 8, 6, 57); TransI(ref D, A, B, C, 15, 10, 58); TransI(ref C, D, A, B, 6, 15, 59); TransI(ref B, C, D, A, 13, 21, 60);
                TransI(ref A, B, C, D, 4, 6, 61); TransI(ref D, A, B, C, 11, 10, 62); TransI(ref C, D, A, B, 2, 15, 63); TransI(ref B, C, D, A, 9, 21, 64);

                A = A + AA;
                B = B + BB;
                C = C + CC;
                D = D + DD;
            }

            /// <summary>
            /// Create Padded buffer for processing , buffer is padded with 0 along 
            /// with the size in the end
            /// </summary>
            /// <returns>the padded buffer as byte array</returns>
            protected byte[] CreatePaddedBuffer()
            {
                uint pad;		//no of padding bits for 448 mod 512 
                byte[] bMsg;	//buffer to hold bits
                ulong sizeMsg;		//64 bit size pad
                uint sizeMsgBuff;	//buffer size in multiple of bytes
                int temp = (448 - ((m_byteInput.Length * 8) % 512)); //temporary 


                pad = (uint)((temp + 512) % 512);		//getting no of bits to  be pad
                if (pad == 0)				///pad is in bits
                    pad = 512;			//at least 1 or max 512 can be added

                sizeMsgBuff = (uint)((m_byteInput.Length) + (pad / 8) + 8);
                sizeMsg = (ulong)m_byteInput.Length * 8;
                bMsg = new byte[sizeMsgBuff];	///no need to pad with 0 coz new bytes 
                // are already initialize to 0 :)




                ////copying string to buffer 
                for (int i = 0; i < m_byteInput.Length; i++)
                    bMsg[i] = m_byteInput[i];

                bMsg[m_byteInput.Length] |= 0x80;		///making first bit of padding 1,

                //wrting the size value
                for (int i = 8; i > 0; i--)
                    bMsg[sizeMsgBuff - i] = (byte)(sizeMsg >> ((8 - i) * 8) & 0x00000000000000ff);

                return bMsg;
            }

            /// <summary>
            /// Copies a 512 bit block into X as 16 32 bit words
            /// </summary>
            /// <param name="bMsg"> source buffer</param>
            /// <param name="block">no of block to copy starting from 0</param>
            protected void CopyBlock(byte[] bMsg, uint block)
            {

                block = block << 6;
                for (uint j = 0; j < 61; j += 4)
                {
                    X[j >> 2] = (((uint)bMsg[block + (j + 3)]) << 24) |
                            (((uint)bMsg[block + (j + 2)]) << 16) |
                            (((uint)bMsg[block + (j + 1)]) << 8) |
                            (((uint)bMsg[block + (j)]));

                }
            }
        }

        /*********************************************************************************************************************************
        ****************************************************FIN****MD5****dAnY************************************************************
        *********************************************************************************************************************************/
    }
    /*********************************************************************************************************************************
    ***********************************************************FIN***dAnY************************************************************
    *********************************************************************************************************************************/
}