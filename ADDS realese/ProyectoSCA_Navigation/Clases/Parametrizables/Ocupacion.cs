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
    public class Ocupacion
    {        
        int IDafiliado_;
        public int IDafiliado
        {
            get { return IDafiliado_; }
            set { IDafiliado_ = value; }
        }

        String ocupacion_;
        public String ocupacion
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
}
