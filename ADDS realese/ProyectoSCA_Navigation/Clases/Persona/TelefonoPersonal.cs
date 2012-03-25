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
    public class TelefonoPersonal
    {
        String numero_;
        public String numero
        {
            get { return this.numero_; }
            set { this.numero_ = value; }
        }
        
        int IDpersona_;
        public int IDpersona
        {
            get{ return this.IDpersona_; }
            set { this.IDpersona_ = value; }
        }
    }
}
