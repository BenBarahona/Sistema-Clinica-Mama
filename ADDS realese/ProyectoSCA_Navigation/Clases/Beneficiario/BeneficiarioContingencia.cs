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
    public class BeneficiarioContingencia : Beneficiario
    {
        bool activo_;
        public bool activo
        {
            get { return this.activo_; }
            set { this.activo_ = value; }
        }
    }
}
