using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TC
{
    enum Estado { retirado, activo, inactivo }

    public class Cuenta
    {
        float saldo_;
        int num_certificado_;
        String fecha_;
        bool estaActiva_;

        public bool estaActiva
        {
            get
            {
                return this.estaActiva_;
            }
            set
            {
                this.estaActiva_ = value;
            }
        }

        public void retirarDeCuenta(float monto)
        {
            if (this.saldo_ >= monto)
                this.saldo_ -= monto;

        }

        public void depositarEnCuenta(float monto)
        {
            this.saldo_ += monto;
        }

        public float saldo
        {
            get
            {
                return this.saldo_;
            }
            set
            {
                this.saldo_ = value;
            }
        }

        public int num_certificado
        {
            get
            {
                return this.num_certificado_;
            }
            set
            {
                this.num_certificado_ = value;
            }
        }

        public String fecha
        {
            get
            {
                return this.fecha_;
            }
            set
            {
                this.fecha_ = value;
            }
        }
    }

    public class Persona
    {
        int ID_;
        String primerNombre_;
        String segundoNombre_;
        String primerApellido_;
        String segundoApellido_;
        String estadoCivil_;
        String direccion_;
        telefonoPersonal[] tel_p;
        List<String> tels = new List<string>();
        List<String> cels = new List<string>();

        String fechaNacimiento_;
        String identidad_;
        String genero_;
        celular[] telefonoCelular_;
        parentesco paren = new parentesco();
        celular[] celular_;

        public String parentesco
        {
            get { return paren.Parentesco; }
            set { this.paren.Parentesco = value; }
        }

        public void actualizarDatos()
        {
            //actualizar datos de una persona
        }

        public List<String> telefonoPersonal
        {

            get
            {
                return tels;
            }
            set
            {
                this.tels = value;
            }
        }

        public int ID
        {
            get
            {
                return this.ID_;
            }
            set
            {
                this.ID_ = value;
            }
        }

        public String estadoCivil
        {
            get
            {
                return this.estadoCivil_;
            }
            set
            {
                this.estadoCivil_ = value;
            }
        }

        public String identidad
        {
            get
            {
                return this.identidad_;
            }

            set
            {
                this.identidad_ = value;
            }
        }

        public String primerNombre
        {
            get
            {
                return this.primerNombre_;
            }
            set
            {
                this.primerNombre_ = value;
            }
        }

        public String segundoNombre
        {
            get
            {
                return this.segundoNombre_;
            }
            set
            {
                this.segundoNombre_ = value;
            }
        }

        public String primerApellido
        {
            get
            {
                return this.primerApellido_;
            }
            set
            {
                this.primerApellido_ = value;
            }
        }

        public String segundoApellido
        {
            get
            {
                return this.segundoApellido_;
            }
            set
            {
                this.segundoApellido_ = value;
            }
        }

        public List<String> celular
        {
            get
            {
                return this.cels;
            }
            set
            {
                this.cels = value;
            }
        }

        public String genero
        {
            get
            {
                return this.genero_;
            }
            set
            {
                this.genero_ = value;
            }
        }

        public String direccion
        {
            get
            {
                return this.direccion_;
            }
            set
            {
                this.direccion_ = value;
            }
        }

        public String fechaNacimiento
        {
            get
            {
                return this.fechaNacimiento_;
            }
            set
            {
                this.fechaNacimiento_ = value;
            }
        }
    }

    public class Beneficiario : Persona
    {

        int IDBeneficiario_;
        parentesco parentesco_ = new parentesco();

        public int IDBeneficiario
        {
            get { return IDBeneficiario_; }
            set { IDBeneficiario_ = value; }
        }


        public String Parentesco
        {
            get { return parentesco_.Parentesco; }
            set { parentesco_.Parentesco = value; }
        }

    }

    public class afiliado : Persona
    {
        String telefonoEmpresa_;
        public String TelefonoEmpresa
        {
            get { return telefonoEmpresa_; }
            set { telefonoEmpresa_ = value; }
        }

        String nombreEmpresa_;
        public String NombreEmpresa
        {
            get { return nombreEmpresa_; }
            set { nombreEmpresa_ = value; }
        }

        String direccionEmpresa_;
        public String DireccionEmpresa
        {
            get { return direccionEmpresa_; }
            set { direccionEmpresa_ = value; }
        }
        
        String departamentoEmpresa_;
        public String DepartamentoEmpresa
        {
            get { return departamentoEmpresa_; }
            set { departamentoEmpresa_ = value; }
        }
        
        String tiempoLaboracionEmpresa_;
        public String TiempoLaboracionEmpresa
        {
            get { return tiempoLaboracionEmpresa_; }
            set { tiempoLaboracionEmpresa_ = value; }
        }

        int IDAfiliado_;
        public int IDAfiliado
        {

            get
            {
                return this.IDAfiliado_;
            }
            set
            {
                this.IDAfiliado_ = value;
            }
        }

        String lugarDeNacimiento_;
        public String lugarDeNacimiento
        {
            get
            {
                return this.lugarDeNacimiento_;
            }
            set
            {
                this.lugarDeNacimiento_ = value;
            }
        }

        String fechaIngresoCooperativa_;
        public String fechaIngresoCooperativa
        {
            get
            {
                return this.fechaIngresoCooperativa_;
            }
            set
            {
                this.fechaIngresoCooperativa_ = value;
            }
        }
        Estado estadoAfiliado_;
        public void retirarse()
        {
            estadoAfiliado_ = Estado.retirado;
        }

        public String obtenerEstadoAfiliado()
        {
            return this.estadoAfiliado_.ToString();
        }

        Usuario usuario_ = new Usuario();
        public String Usuario
        {
            get
            {
                return this.usuario_.correoElectronico;
            }
            set
            {
                this.usuario_.correoElectronico = value;
            }
        }

        public String Password
        {
            get
            {
                return this.usuario_.Password;
            }
            set
            {
                this.usuario_.Password = value;
            }
        }

        Cuenta c = new Cuenta();
        public int certificadoCuenta
        {
            get { return this.c.num_certificado; }
            set { this.c.num_certificado = value; }
        }

        ocupacion ocupacion_ = new ocupacion();
        public String Ocupacion
        {
            get { return ocupacion_.Ocupacion; }
            set { ocupacion_.Ocupacion = value; }
        }

        BeneficiarioNormal[] beneficiariosNormales_;
        public BeneficiarioNormal[] bensNormales
        {
            get {  return beneficiariosNormales_; }
            set { beneficiariosNormales_ = value; }
        }

        Beneficiario beneficiarioCont_ = new BeneficiarioContingencia();
        public Beneficiario BeneficiarioCont
        {
            get { return beneficiarioCont_; }
            set { beneficiarioCont_ = value; }
        }

        public String CorreoElectronico
        {
            get { return usuario_.correoElectronico; }
            set { this.usuario_.correoElectronico = value; }
        }

        public void verAportacionesObligatorias()
        {
            //en este metodo el afiliado puede ver que aportaciones ha pagado
            //y cuales le quedan por pagar
        }

        public void retirar(float monto)
        {
            c.retirarDeCuenta(monto);
        }

        public void depositar(float monto)
        {
            c.depositarEnCuenta(monto);
        }

        public float consultarMontoDeCuenta()
        {
            return c.saldo;
        }
    }

    public class Empleado : Persona
    {
        //String lugarNacimiento_;
        //String nombreEmpresaLabora_;
        //String telefonoEmpresaLabora_;
        //String direccionEmpresaLabora_;
        //String puesto_2;
        public String Puesto
        {
            get { return puesto_; }
            set { puesto_ = value; }
        }
        //public String DireccionEmpresaLabora
        //{
        //    get { return direccionEmpresaLabora_; }
        //    set { direccionEmpresaLabora_ = value; }
        //}
        //public String TelefonoEmpresaLabora
        //{
        //    get { return telefonoEmpresaLabora_; }
        //    set { telefonoEmpresaLabora_ = value; }
        //}

        //public String NombreEmpresaLabora
        //{
        //    get { return nombreEmpresaLabora_; }
        //    set { nombreEmpresaLabora_ = value; }
        //}

        //public String LugarNacimiento
        //{
        //    get { return lugarNacimiento_; }
        //    set { lugarNacimiento_ = value; }
        //}
        //public int IDProfesion
        //{
        //    get { return IDProfesion_; }
        //    set { IDProfesion_ = value; }
        //}
        String fechaInicioLabores_;
        int IDEmpleado_;
        String puesto_;
        public String IDPuesto
        {
            get { return puesto_; }
            set { puesto_ = value; }
        }

        Usuario usuario_ = new Usuario();
        public String correoElectronico
        {
            get { return usuario_.correoElectronico; }
            set { usuario_.correoElectronico = value; }
        }

        public String rol
        {
            get { return usuario_.Rol_s; }
            set { usuario_.Rol_s = value; }
        }
        public int IDEmpleado
        {
            get
            {
                return this.IDEmpleado_;
            }
            set
            {
                this.IDEmpleado_ = value;
            }
        }
        public String fechaInicioLabores
        {
            get
            {
                return this.fechaInicioLabores_;
            }
            set
            {
                this.fechaInicioLabores_ = value;
            }
        }

        //public String puesto
        //{
        //    get
        //    {
        //        return this.puesto_;
        //    }
        //    set
        //    {
        //        this.puesto_ = value;
        //    }
        //}
    }

    public class rol
    {
        int IDRol_;
        String[] permisos_;
        String nombre_;
        int IDUsuario_;


        public int IDUsuario
        {
            get { return IDUsuario_; }
            set { IDUsuario_ = value; }
        }

        public int IDRol
        {
            get
            {
                return this.IDRol_;
            }
            set
            {
                this.IDRol_ = value;
            }
        }
        public String[] permisos
        {
            get
            {
                return this.permisos_;
            }
            set
            {
                this.permisos_ = value;
            }
        }
        public String nombre
        {
            get
            {
                return this.nombre_;
            }
            set
            {
                this.nombre_ = value;
            }
        }

    }

    public class BeneficiarioContingencia : Beneficiario
    {
        bool activo_;
        public bool activo
        {
            get
            {
                return this.activo_;
            }
            set
            {
                this.activo_ = value;
            }
        }


    }

    public class BeneficiarioNormal : Beneficiario
    {
        float porcentajeAportaciones_;
        float porcentajeSeguros_;

        public float porcentajeAportaciones
        {
            get
            {
                return this.porcentajeAportaciones_;
            }
            set
            {
                this.porcentajeAportaciones_ = value;
            }
        }

        public float porcentajeSeguros
        {
            get
            {
                return this.porcentajeSeguros_;
            }
            set
            {
                this.porcentajeSeguros_ = value;
            }
        }
    }

    public class Usuario
    {
        int IDUsuario_;
        String correoElectronico_;
        String password_;
        rol rol_;


        public String Rol_s
        {
            get { return rol_.nombre; }
            set { rol_.nombre = value; }
        }





        public int IDUsuario
        {
            get
            {
                return this.IDUsuario_;
            }
            set
            {
                this.IDUsuario_ = value;
            }
        }
        public String correoElectronico
        {
            get
            {
                return this.correoElectronico_;
            }
            set
            {
                this.correoElectronico_ = value;
            }
        }

        public String Password
        {
            get
            {
                return this.password_;
            }
            set
            {
                this.password_ = value;
            }
        }

    }

    public class montoAportacionObligatoria
    {
        //NO SE PORQUE NO ME DEJA HACER ESTO....

        Double cantidad_ = 0;

        public Double Cantidad
        {
            get { return cantidad_; }
            set { cantidad_ = value; }
        }
        Double IDmonto_ = 0;

        public Double IDmonto
        {
            get { return IDmonto_; }
            set { IDmonto_ = value; }
        }
        Double fecha_ = 0;

        public Double Fecha
        {
            get { return fecha_; }
            set { fecha_ = value; }
        }





    }

    public class aportacion
    {

        float[] interes_;

        public float[] Interes_
        {
            get { return interes_; }
            set { interes_ = value; }
        }
        String descripcion_;

        public String Descripcion
        {
            get { return descripcion_; }
            set { descripcion_ = value; }
        }
        float monto_;

        public float Monto
        {
            get { return monto_; }
            set { monto_ = value; }
        }

        int IDAportacion_;

        public int IDAportacion
        {
            get { return IDAportacion_; }
            set { IDAportacion_ = value; }
        }

        String fechaRealizacion_;

        public String FechaRealizacion
        {
            get { return fechaRealizacion_; }
            set { fechaRealizacion_ = value; }
        }

        int IDAfiliado_;

        public int IDAfiliado
        {
            get { return IDAfiliado_; }
            set { IDAfiliado_ = value; }
        }



    }

    public class aportacionObligatoria : aportacion
    {
        int idMonto_;

        public int IdMonto
        {
            get { return idMonto_; }
            set { idMonto_ = value; }
        }
        String fechaPlazo_;
        montoAportacionObligatoria monto;

        public montoAportacionObligatoria MontoAportacionObligatoria
        {
            get { return monto; }
            set { monto = value; }
        }


        public String FechaPlazo
        {
            get { return fechaPlazo_; }
            set { fechaPlazo_ = value; }
        }


    }

    public class aportacionObligatoriaEspecial : aportacion
    {
        //String[] motivo_;
        Motivo[] motivo_;

        public Motivo[] Motivo
        {
            get { return motivo_; }
            set { motivo_ = value; }
        }

        String fechaPlazo_;


        /*public String[] motivo
        {
            get
            {
                return this.motivo_;
            }
            set
            {
                this.motivo_ = value;
            }
        }*/

        public String fechaPlazo
        {
            get
            {
                return this.fechaPlazo_;
            }
            set
            {
                this.fechaPlazo_ = value;
            }
        }
    }

    public class Motivo
    {
        String nombre;

        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        String descripcion;

        public String Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
        aportacionObligatoriaEspecial aportacion = new aportacionObligatoriaEspecial();

        public aportacionObligatoriaEspecial Aportacion
        {
            get { return aportacion; }
            set { aportacion = value; }
        }

    }

    public class aportacionVoluntaria : aportacion
    {
        bool aceptada_;
        int idLimite_;

        public int IdLimite
        {
            get { return idLimite_; }
            set { idLimite_ = value; }
        }



        public bool estaAceptada
        {
            get
            {
                return this.aceptada_;
            }
            set
            {
                this.aceptada_ = value;
            }
        }
    }

    public class Limite
    {
        int IDLimite_;
        int cantidad_;
        String fecha_;

        public int IDLimite
        {
            get
            {
                return this.IDLimite_;
            }
            set
            {
                this.IDLimite_ = value;
            }
        }
        public int cantidad
        {
            get
            {
                return this.cantidad_;
            }
            set
            {
                this.cantidad_ = value;
            }
        }
        public String fecha
        {
            get
            {
                return this.fecha_;
            }
            set
            {
                this.fecha_ = value;
            }
        }
    }

    public class celular
    {
        String numero_;
        int IDpersona_;
        public String numero
        {
            get
            {
                return this.numero_;
            }
            set
            {
                this.numero_ = value;
            }
        }
        public int IDpersona
        {
            get
            {
                return this.IDpersona_;
            }
            set
            {
                this.IDpersona_ = value;
            }
        }
    }

    public class telefonoPersonal
    {
        String numero_;
        int IDpersona_;
        public String numero
        {
            get
            {
                return this.numero_;
            }
            set
            {
                this.numero_ = value;
            }
        }
        public int IDpersona
        {
            get
            {
                return this.IDpersona_;
            }
            set
            {
                this.IDpersona_ = value;
            }
        }
    }

    public class parentesco
    {
        //String relacion_;
        //int IDbeneficiario_;

        int IDParentesco_;
        String parentesco_;

        public String Parentesco
        {
            get
            {
                return this.parentesco_;
            }
            set
            {
                this.parentesco_ = value;
            }
        }
        public int IDParentesco
        {
            get
            {
                return this.IDParentesco_;
            }
            set
            {
                this.IDParentesco_ = value;
            }
        }

        //public String relacion
        //{
        //    get
        //    {
        //        return this.relacion_;
        //    }
        //    set
        //    {
        //        this.relacion_ = value;
        //    }
        //}
        //public int IDbeneficiario
        //{
        //    get
        //    {
        //        return this.IDbeneficiario_;
        //    }
        //    set
        //    {
        //        this.IDbeneficiario_ = value;
        //    }
        //}

    }

    public class ocupacion
    {
        String ocupacion_;
        int IDafiliado_;

        public int IDafiliado
        {
            get { return IDafiliado_; }
            set { IDafiliado_ = value; }
        }

        public String Ocupacion
        {
            get { return ocupacion_; }
            set { ocupacion_ = value; }
        }
        int IDocupacion_;

        public int IDocupacion
        {
            get { return IDocupacion_; }
            set { IDocupacion_ = value; }
        }
    }

    public class Permisos
    {
        int IDPermiso_;

        public int IDPermiso
        {
            get { return IDPermiso_; }
            set { IDPermiso_ = value; }
        }
        String nombrePermiso_;

        public String NombrePermiso
        {
            get { return nombrePermiso_; }
            set { nombrePermiso_ = value; }
        }
        int idRol_;

        public int IdRol
        {
            get { return idRol_; }
            set { idRol_ = value; }
        }
    }

    public class Interes
    {
        String nombre_;
        Int64 tasa_, monton_voluntaria_;
        Boolean aplica_obligatoria_, aplica_voluntaria_, aplica_obligatoria_especial_, aplica_voluntaria_arriba_;

        public String Nombre
        {
            get { return nombre_; }
            set { nombre_ = value; }
        }

        public Int64 Tasa
        {
            get { return tasa_; }
            set { tasa_ = value; }
        }

        public Int64 Monto_voluntaria
        {
            get { return monton_voluntaria_; }
            set { monton_voluntaria_ = value; }
        }

        public Boolean Aplica_obligatoria
        {
            get { return aplica_obligatoria_; }
            set { aplica_obligatoria_ = value; }
        }

        public Boolean Aplica_voluntaria
        {
            get { return aplica_voluntaria_; }
            set { aplica_voluntaria_ = value; }
        }

        public Boolean Aplica_obligatoria_especial
        {
            get { return aplica_obligatoria_especial_; }
            set { aplica_obligatoria_especial_ = value; }
        }

        public Boolean Aplica_voluntaria_arriba
        {
            get { return aplica_voluntaria_arriba_; }
            set { aplica_voluntaria_arriba_ = value; }
        }
    }
}