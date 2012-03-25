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
    public class BeneficiarioNormal : Beneficiario
    {
        float porcentajeAportaciones_;
        public float porcentajeAportaciones
        {
            get { return this.porcentajeAportaciones_; }
            set { this.porcentajeAportaciones_ = value; }
        }

        float porcentajeSeguros_;
        public float porcentajeSeguros
        {
            get { return this.porcentajeSeguros_; }
            set { this.porcentajeSeguros_ = value; }
        }
    }
}
