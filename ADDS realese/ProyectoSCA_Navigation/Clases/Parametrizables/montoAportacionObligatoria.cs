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
    public class montoAportacionObligatoria
    {
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

        String fecha_;
        public String Fecha
        {
            get { return fecha_; }
            set { fecha_ = value; }
        }
    }
}
