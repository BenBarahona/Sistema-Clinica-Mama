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
    public class Limite
    {
        int IDLimite_, cantidad_;
        String fecha_;

        public int IDLimite
        {
            get { return this.IDLimite_; }
            set { this.IDLimite_ = value; }
        }
        
        public int cantidad
        {
            get { return this.cantidad_; }
            set { this.cantidad_ = value; }
        }
        
        public String fecha
        {
            get { return this.fecha_; }
            set { this.fecha_ = value; }
        }
    }
}
