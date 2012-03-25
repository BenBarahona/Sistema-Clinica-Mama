using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace ProyectoSCA_Navigation.Clases
{
    public class Afiliado : Persona
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
            get { return this.IDAfiliado_; }
            set { this.IDAfiliado_ = value; }
        }

        String lugarDeNacimiento_;
        public String lugarDeNacimiento
        {
            get { return this.lugarDeNacimiento_; }
            set { this.lugarDeNacimiento_ = value; }
        }

        String fechaIngresoCooperativa_;
        public String fechaIngresoCooperativa
        {
            get { return this.fechaIngresoCooperativa_; }
            set { this.fechaIngresoCooperativa_ = value; }
        }

        String estadoAfiliado_;
        public String EstadoAfiliado
        {
            get { return this.estadoAfiliado_;}
            set { this.estadoAfiliado_ = value; }
        }

        Usuario usuario_ = new Usuario();
        public String Password
        {
            get { return this.usuario_.Password; }
            set { this.usuario_.Password = value; }
        }
        public String CorreoElectronico
        {
            get { return usuario_.correoElectronico; }
            set { this.usuario_.correoElectronico = value; }
        }

        Cuenta c = new Cuenta();
        public int certificadoCuenta
        {
            get { return this.c.num_certificado; }
            set { this.c.num_certificado = value; }
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

        Ocupacion ocupacion_ = new Ocupacion();
        public String Ocupacion
        {
            get { return ocupacion_.ocupacion; }
            set { ocupacion_.ocupacion = value; }
        }

        List<BeneficiarioNormal> beneficiariosNormales_;
        public List<BeneficiarioNormal> bensNormales
        {
            get { return beneficiariosNormales_; }
            set { beneficiariosNormales_ = value; }
        }

        Beneficiario beneficiarioCont_ = new BeneficiarioContingencia();
        public Beneficiario BeneficiarioCont
        {
            get { return beneficiarioCont_; }
            set { beneficiarioCont_ = value; }
        }

        public void verAportacionesObligatorias()
        {
            //en este metodo el afiliado puede ver que aportaciones ha pagado
            //y cuales le quedan por pagar
        }
    }
}
