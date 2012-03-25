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

namespace ProyectoSCA_Navigation.Clases
{
    enum Estado { retirado, activo, inactivo }

    public class Cuenta
    {        
        bool estaActiva_;
        public bool estaActiva
        {
            get { return this.estaActiva_; }
            set { this.estaActiva_ = value; }
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

        float saldo_;
        public float saldo
        {
            get { return this.saldo_; }
            set { this.saldo_ = value; }
        }

        int num_certificado_;
        public int num_certificado
        {
            get { return this.num_certificado_; }
            set { this.num_certificado_ = value; }
        }

        String fecha_;
        public String fecha
        {
            get { return this.fecha_; }
            set { this.fecha_ = value; }
        }
    }
}
