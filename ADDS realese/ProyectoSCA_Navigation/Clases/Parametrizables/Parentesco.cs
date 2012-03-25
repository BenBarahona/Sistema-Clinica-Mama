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
    public class Parentesco
    {
        String parentesco_;
        public String parentesco
        {
            get { return this.parentesco_; }
            set { this.parentesco_ = value; }
        }
        
        int IDParentesco_;
        public int IDParentesco
        {
            get { return this.IDParentesco_; }
            set { this.IDParentesco_ = value; }
        }
    }
}
