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
    public class aportacionObligatoriaEspecial : Aportacion
    {
        Motivo motivo_ = new Motivo();
        public String Motivo
        {
            get { return motivo_.Nombre; }
            set { motivo_.Nombre = value; }
        }

        String fechaPlazo_;
        public String fechaPlazo
        {
            get { return this.fechaPlazo_; }
            set { this.fechaPlazo_ = value; }
        }
    }
}
