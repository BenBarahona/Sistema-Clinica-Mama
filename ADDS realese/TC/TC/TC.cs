using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Security.Cryptography;

namespace TC
{
    /********************************************************************************************************************************
     *****************************************************************dAnY************************************************************
     *********************************************************************************************************************************/

    enum peticion { ingresar_afiliado, ingresar_empleado, modificar_afiliado, modificar_empleado, mostrar_perfil_afiliado, mostrar_perfil_empleado, reingresar_afiliado, 
        ingresar_aportacion, capitalizar_extra, login, ingresar_ocupacion, ingresar_AOS, ingresar_monto_AO, limite_AV, ingresar_motivo, ingresar_parentesco, ingresar_interes }

    class TC
    {

        public TC()
        {
        }
        /*
         * En este metodo se recibe un enum el cual s la opcion y un object q son los valores...
         * Este es el formato:
         *       enum          recibe   retorna
         * ingresar_afiliado, afiliado, void
         * ingresar_empleado, empleado, void
         * modificar_afiliado, afiliado, 
         * modificar_empleado, empleado, 
         * mostrar_perfil_afiliado, string, afiliado
         * mostrar_perfil_empleado, string, empleado
         * reingresar_afiliado, 
         * ingresar_aportacion, 
         * capitalizar_extra, 
         * login, [] string, void
         * ingresar_ocupacion, string ocupacion, void
         * ingresar_AOS, 
         * ingresar_monto_AO, string monto, void
         * limite_AV, string limite, void
         * ingresar_motivo, string motivo, void
         * ingresar_parentesco, string parentesco, void
         * ingresar_interes, interes, void
        */
        public void AgregarPeticion(peticion codigo_peticion, object valores)
        {
            switch (codigo_peticion)
            {
                case peticion.ingresar_afiliado:
                    this.InsertarAfiliado(valores as afiliado);
                    break;
                case peticion.ingresar_empleado:
                    this.InsertarEmpleado(valores as Empleado);
                    break;
                case peticion.login:
                    this.Login(valores);
                    break;
                case peticion.ingresar_monto_AO:
                    this.InsertarMonto(valores as string);
                    break;
                case peticion.ingresar_interes:
                    this.InsertarInteres(valores as Interes);
                    break;
                case peticion.ingresar_ocupacion:
                    this.InsertarOcupacion(valores as string);
                    break;
                case peticion.ingresar_motivo:
                    this.InsertarMotivo(valores as string);
                    break;
                case peticion.limite_AV:
                    this.InsertarLimite(valores as string);
                    break;
                case peticion.ingresar_parentesco:
                    this.InsertarParentesco(valores as string);
                    break;
                case peticion.modificar_afiliado:
                    this.ModificarAfiliado(valores as afiliado);
                    break;
                case peticion.modificar_empleado:
                    this.ModificarEmpleado(valores as Empleado);
                    break;
                default:
                    break;
            }
        }

        /*********************************************************************************************************************************
         *****************************************************************INSERTS*********************************************************
         *********************************************************************************************************************************/

        //Insertar una nuevo limite aportacion voluntaria
        private Boolean InsertarLimite(string limite)
        {
            string m_Enviar = limite;

            return true;
        }

        //Insertar una nuevo monto aportacion obligatoria
        private Boolean InsertarMonto(string monto)
        {
            string m_Enviar = monto;

            return true;
        }

        //Insertar una nuevo motivo
        private Boolean InsertarMotivo(string motivo)
        {
            string m_Enviar = motivo;

            return true;
        }

        //Insertar una nueva ocupacion
        private Boolean InsertarOcupacion(string ocupacion)
        {
            string m_Enviar=ocupacion;

            return true;
        }

        //Insertar un nuevo interes
        private Boolean InsertarInteres(Interes intereses)
        {
            string m_Enviar;
            char del1 = ',';

            /*
             *En la variable m_Enviar se guarda el string que se enviara al WS y lo que se recibe es una referencia hacia la clase interes, el formato es el siguiente:
             *nombre(string), tasa(Int64), aplica obligatoria(boolean), aplica voluntaria(boolean), aplica obligatoria especial(boolean), 
             *aplica voluntaria arriba(boolean), monto voluntaria(Int64)
            */
            m_Enviar = intereses.Nombre+del1+intereses.Tasa+del1+intereses.Aplica_obligatoria+del1+intereses.Aplica_voluntaria+del1
                +intereses.Aplica_obligatoria_especial+del1+intereses.Aplica_voluntaria_arriba+del1+intereses.Monto_voluntaria;

            return true;
        }

        //Insertar una nueva parentesco
        private Boolean InsertarParentesco(string parentesco)
        {
            string m_Enviar = parentesco;

            return true;
        }

        //Insertar un nuevo afiliado
        private Boolean InsertarAfiliado(afiliado afiliado)
        {
            string m_Enviar;
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
            m_Enviar = afiliado.identidad+del1+afiliado.primerNombre+del1+afiliado.segundoNombre+del1+afiliado.primerApellido+del1+afiliado.segundoApellido+del1
                +this.convertirFechaBD(afiliado.fechaNacimiento)+del1+afiliado.estadoCivil+del1+afiliado.genero+del1+afiliado.direccion+del2;

            /*
             * DATOS PERSONALES:
             * telefono personal(string);
             * aqui son varios asi que por eso aparte
            */
            for (int i = 0; i < afiliado.telefonoPersonal.Count; i++ )
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
            m_Enviar += del2+afiliado.CorreoElectronico+del1+this.md5(afiliado.Password)+del2+afiliado.NombreEmpresa+del1+afiliado.TelefonoEmpresa+del1+afiliado.DireccionEmpresa
                +del1+this.convertirFechaBD(afiliado.fechaIngresoCooperativa)+del1+afiliado.DepartamentoEmpresa+del1+afiliado.lugarDeNacimiento+del2
                +afiliado.Ocupacion+del2;

            /*
             * DATOS BENEFICIARIO DE CONTINGENCIA:
             * No. Identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), fecha_nac(string), estado_Civil(string), genero(string),
             * direccion(string), parentesco(string);
            */
            m_Enviar += afiliado.BeneficiarioCont.identidad+del1+afiliado.BeneficiarioCont.primerNombre+del1+afiliado.BeneficiarioCont.segundoNombre
                +del1+afiliado.BeneficiarioCont.primerApellido+del1+afiliado.BeneficiarioCont.segundoApellido+del1+this.convertirFechaBD(afiliado.BeneficiarioCont.fechaNacimiento)
                +del1+afiliado.BeneficiarioCont.estadoCivil+del1+afiliado.BeneficiarioCont.genero+del1+afiliado.BeneficiarioCont.direccion+del1+afiliado.BeneficiarioCont.parentesco;
            m_Enviar += del2;

            /*
             * DATOS BENEFICIARIOS NORMALES:
             * No. Identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), fecha_nac(string), estado_Civil(string), genero(string),
             * direccion(string), parentesco(string), porcentaje seguros(Int64), porcentaje aportacion(Int64)
            */
            foreach (BeneficiarioNormal item in afiliado.bensNormales)
            {
                m_Enviar += item.identidad+del1+item.primerNombre+del1+item.segundoNombre+del1+ item.primerApellido+del1+item.segundoApellido+del1+this.convertirFechaBD(item.fechaNacimiento)
                +del1+item.estadoCivil+del1+item.genero+del1+item.direccion+del1+item.parentesco+del1+item.porcentajeSeguros+del1+item.porcentajeAportaciones;
            }

            return true;
        }

        //Insertar un nuevo empleado
        private Boolean InsertarEmpleado(Empleado empleado)
        {
            string m_Enviar;
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
             * correo electronico(string), puesto(string)
            */
            m_Enviar += del2 + empleado.correoElectronico + del1 + empleado.Puesto;

            return true;
        }

        /*********************************************************************************************************************************
         *********************************************************FIN*****INSERTS*********************************************************
         *********************************************************************************************************************************/

        /*********************************************************************************************************************************
         ***********************************************************MODIFICAR*************************************************************
         *********************************************************************************************************************************/

        //Modificar un afiliado
        private Boolean ModificarAfiliado(afiliado afiliado)
        {
            string m_Enviar;
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
            m_Enviar += del2 + afiliado.CorreoElectronico + del1 + this.md5(afiliado.Password) + del2 + afiliado.NombreEmpresa + del1 + afiliado.TelefonoEmpresa + del1 + afiliado.DireccionEmpresa
                + del1 + this.convertirFechaBD(afiliado.fechaIngresoCooperativa) + del1 + afiliado.DepartamentoEmpresa + del1 + afiliado.lugarDeNacimiento + del2
                + afiliado.Ocupacion + del2;

            /*
             * DATOS BENEFICIARIO DE CONTINGENCIA:
             * No. Identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), fecha_nac(string), estado_Civil(string), genero(string),
             * direccion(string), parentesco(string);
            */
            m_Enviar += afiliado.BeneficiarioCont.identidad + del1 + afiliado.BeneficiarioCont.primerNombre + del1 + afiliado.BeneficiarioCont.segundoNombre
                + del1 + afiliado.BeneficiarioCont.primerApellido + del1 + afiliado.BeneficiarioCont.segundoApellido + del1 + this.convertirFechaBD(afiliado.BeneficiarioCont.fechaNacimiento)
                + del1 + afiliado.BeneficiarioCont.estadoCivil + del1 + afiliado.BeneficiarioCont.genero + del1 + afiliado.BeneficiarioCont.direccion + del1 + afiliado.BeneficiarioCont.parentesco;
            m_Enviar += del2;

            /*
             * DATOS BENEFICIARIOS NORMALES:
             * No. Identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), fecha_nac(string), estado_Civil(string), genero(string),
             * direccion(string), parentesco(string), porcentaje seguros(Int64), porcentaje aportacion(Int64)
            */
            foreach (BeneficiarioNormal item in afiliado.bensNormales)
            {
                m_Enviar += item.identidad + del1 + item.primerNombre + del1 + item.segundoNombre + del1 + item.primerApellido + del1 + item.segundoApellido + del1 + this.convertirFechaBD(item.fechaNacimiento)
                + del1 + item.estadoCivil + del1 + item.genero + del1 + item.direccion + del1 + item.parentesco + del1 + item.porcentajeSeguros + del1 + item.porcentajeAportaciones;
            }

            return true;
        }

        //Modificar un empleado
        private Boolean ModificarEmpleado(Empleado empleado)
        {
            string m_Enviar;
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
             * correo electronico(string), puesto(string)
            */
            m_Enviar += del2 + empleado.correoElectronico + del1 + empleado.Puesto;
            
            return true;
        }

        /*********************************************************************************************************************************
         **************************************************FIN******MODIFICAR*************************************************************
         *********************************************************************************************************************************/

        /*********************************************************************************************************************************
         *****************************************************************GETS************************************************************
         *********************************************************************************************************************************/

        //Obtiene string de DT y lo convierte a un afiliado
        private afiliado BuscarAfiliado(string valores)
        {
            afiliado afiliado = new afiliado();

            /*obtener datos de afiliado*/
            string[] datos = valores.Split(';');

            /*
             * DATOS PERSONALES:
             * identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), 
             * fecha nacimiento(string), estado civil(string), genero(string),direccion(string);
            */
            string[] datos_personales = datos[0].Split(',');
            afiliado.identidad=datos_personales[0]; afiliado.primerNombre=datos_personales[1]; afiliado.segundoNombre=datos_personales[2]; 
            afiliado.primerApellido=datos_personales[3];afiliado.segundoApellido=datos_personales[4];afiliado.fechaNacimiento=this.convertirFechaNormal(datos_personales[5]);
            afiliado.estadoCivil=datos_personales[6];afiliado.genero=datos_personales[7];afiliado.direccion=datos_personales[8];

            /*
             * DATOS PERSONALES:
             * telefono personal(string);
             * aqui son varios asi que por eso aparte
            */
            string[] telefonos = datos[1].Split(',');
            List<string> telefono = telefonos.OfType<string>().ToList();
            afiliado.telefonoPersonal= telefono;
            /*
             * DATOS PERSONALES:
             * telefono celular(string);
             * aqui son varios asi que por eso aparte
            */
            string[] celulares = datos[2].Split(',');
            List<string> celular = celulares.OfType<string>().ToList();
            afiliado.celular = celular;
            /*
             * DATOS LABORALES:
             * correo electronico(string), password(string 32 caracteres), nombre empresa(string), telefono empresa(string), direccion empresa(string),
             * fecha ingreso cooperativa(string), depto empresa(string), lugar nacimiento(string), ocupacion(stirng);
            */
            string[] datos_laborales = datos[4].Split(',');
            string[] login = datos[3].Split(',');
            afiliado.CorreoElectronico = login[0]; afiliado.NombreEmpresa = datos_laborales[0]; afiliado.TelefonoEmpresa = datos_laborales[1];
            afiliado.DireccionEmpresa = datos_laborales[2];
            afiliado.fechaIngresoCooperativa=this.convertirFechaNormal(datos_laborales[3]);afiliado.DepartamentoEmpresa=datos_laborales[4];
            afiliado.lugarDeNacimiento=datos_laborales[5];afiliado.Ocupacion=datos[5];

            /*
             * DATOS BENEFICIARIO DE CONTINGENCIA:
             * No. Identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), fecha_nac(string), estado_Civil(string), genero(string),
             * direccion(string), parentesco(string);
            */
            string[] beneficiario_cont = datos[6].Split(',');
            afiliado.BeneficiarioCont.identidad=beneficiario_cont[0];afiliado.BeneficiarioCont.primerNombre=beneficiario_cont[1];
            afiliado.BeneficiarioCont.segundoNombre=beneficiario_cont[2];afiliado.BeneficiarioCont.primerApellido=beneficiario_cont[3];
            afiliado.BeneficiarioCont.segundoApellido=beneficiario_cont[4];afiliado.BeneficiarioCont.fechaNacimiento=beneficiario_cont[5];
            afiliado.BeneficiarioCont.estadoCivil=beneficiario_cont[6];afiliado.BeneficiarioCont.genero=beneficiario_cont[7];
            afiliado.BeneficiarioCont.direccion=beneficiario_cont[8];afiliado.BeneficiarioCont.parentesco=beneficiario_cont[9];

            /*
             * DATOS BENEFICIARIOS NORMALES:
             * No. Identidad(string), pNombre(string), sNombre(string), pApellido(string), sApellido(string), fecha_nac(string), estado_Civil(string), genero(string),
             * direccion(string), parentesco(string), porcentaje seguros(Int64), porcentaje aportacion(Int64);
            */

            for (int i = 7; i < datos.Length; i++)
			{
                string[] normal = datos[i].Split(',');
            }
            return afiliado;
        }
        
        //Obtiene string de DT y convierte a varias ocupaciones
        private void BuscarOcupacion(string valores)
        {
        }

        //Login
        private Boolean Login(object valores)
        {
            string[] tmp = valores as string[];
            string m_Enviar;
            char del1 = ',';

            m_Enviar = tmp[0] + del1 + this.md5(tmp[1]);

            return true;
        }

        /*********************************************************************************************************************************
         **********************************************************FIN****GETS************************************************************
         *********************************************************************************************************************************/

        //Convierte la fecha del sistema a formato de BD con el formato: AÑO-MES-DIA
        private string convertirFechaBD(string fecha)
        {
            string [] fecha_corregida = fecha.Split('/');
            fecha = "";

            fecha += fecha_corregida[2] + "-";
            fecha += fecha_corregida[1] + "-";
            fecha += fecha_corregida[0];

            return fecha;
        }

        //Convierte la fecha de la a BD a formato de BD con el formato: DIA/MES/AÑO
        private string convertirFechaNormal(string fecha)
        {
            string[] fecha_corregida = fecha.Split('-');
            fecha = "";

            fecha += fecha_corregida[0] + "/";
            fecha += fecha_corregida[1] + "/";
            fecha += fecha_corregida[2];

            return fecha;
        }

        //Encriptar a md5
        private string md5(string valor)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();

            stream = md5.ComputeHash(encoding.GetBytes(valor));

            for (int i = 0; i < stream.Length; i++)
                sb.AppendFormat("{0:x2}", stream[i]);

            return sb.ToString();
        }
    }

    class Correo
    {
        SmtpClient server;
        MailMessage mensage;
        Attachment adjunto;
        string correo, password, host;
        int puerto;

        public Correo()
        {
            server = new SmtpClient();
            mensage = new MailMessage();
        }//Fin Constructor

        //Establece el host, puerto e inicia session
        public void Login(string correo, string password)
        {
            this.correo = correo; this.password = password;

            puerto = 25;
            server.EnableSsl = true;

            if (correo.Contains("@hotmail.com"))
                host = "smtp.live.com";
            else if (correo.Contains("@yahoo.com"))
            {
                host = "out.izymail.com";
                server.EnableSsl = false;
            }
            else if (correo.Contains("@gmail.com") || correo.Contains("@unitec.edu"))
                host = "smtp.gmail.com";

            server.Host = this.host;
            server.Port = this.puerto;
            server.Credentials = new System.Net.NetworkCredential(correo, password);
            mensage.From = new System.Net.Mail.MailAddress(this.correo);
            mensage.To.Add("adds.cosecol@gmail.com");
            mensage.Subject = "";
            mensage.Body = "login";
            server.Send(mensage);
        }

        //Envia correo solo con el mensaje
        public void Enviar_Correo(string correo_a_enviar, string mensaje)
        {
            Array a;
            a = correo_a_enviar.Split(';');
            string tmp;

            for (int i = 0; i < a.Length - 1; i++)
            {
                tmp = a.GetValue(i).ToString() as string;
                if (!tmp.Equals(""))
                {
                    mensage.From = new System.Net.Mail.MailAddress(this.correo);
                    mensage.To.Add(tmp);
                    mensage.Subject = "";
                    mensage.Body = mensaje;
                }
            }
            server.Send(mensage);

            System.Windows.Forms.MessageBox.Show("Correo Enviado Exitosamente");
        }

        //Envia correo solo con el mensaje y tema
        public void Enviar_Correo(string correo_a_enviar, string tema, string mensaje)
        {
            Array a;
            a = correo_a_enviar.Split(';');
            string tmp;

            for (int i = 0; i < a.Length - 1; i++)
            {
                tmp = a.GetValue(i).ToString() as string;
                if (!tmp.Equals(""))
                {
                    mensage.From = new System.Net.Mail.MailAddress(correo);
                    mensage.To.Add(tmp);
                    mensage.Subject = tema;
                    mensage.Body = mensaje;
                }
            }
            server.Send(mensage);

            System.Windows.Forms.MessageBox.Show("Correo Enviado Exitosamente");
        }
        
        //Envia correo solo con el mensaje y ruta del attachment
        public void Enviar_Correo_Ruta(string correo_a_enviar, string mensaje, string ruta)
        {
            Array a;
            a = correo_a_enviar.Split(';');
            string tmp;

            for (int i = 0; i < a.Length - 1; i++)
            {
                tmp = a.GetValue(i).ToString() as string;
                if (!tmp.Equals(""))
                {
                    adjunto = new System.Net.Mail.Attachment(ruta);
                    mensage.From = new System.Net.Mail.MailAddress(correo);
                    mensage.To.Add(tmp);
                    mensage.Subject = "";
                    mensage.Body = mensaje;
                    mensage.Attachments.Add(adjunto);
                }
            }
            server.Send(mensage);

            System.Windows.Forms.MessageBox.Show("Correo Enviado Exitosamente");
        }

        //Envia correo solo con el mensaje, tema y ruta del attachment
        public void Enviar_Correo(string correo_a_enviar, string tema, string mensaje, string ruta)
        {
            Array a;
            a = correo_a_enviar.Split(';');
            string tmp;

            for (int i = 0; i < a.Length - 1; i++)
            {
                tmp = a.GetValue(i).ToString() as string;
                if (!tmp.Equals(""))
                {
                    adjunto = new System.Net.Mail.Attachment(ruta);
                    mensage.From = new System.Net.Mail.MailAddress(correo);
                    mensage.To.Add(tmp);
                    mensage.Subject = tema;
                    mensage.Body = mensaje;
                    mensage.Attachments.Add(adjunto);
                }
            }
            server.Send(mensage);

            System.Windows.Forms.MessageBox.Show("Correo Enviado Exitosamente");
        }
    }

    /*********************************************************************************************************************************
    ***********************************************************FIN***dAnY************************************************************
    *********************************************************************************************************************************/
}