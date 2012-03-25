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
    public class Beneficiario : Persona
    {

        int IDBeneficiario_;
        public int IDBeneficiario
        {
            get { return IDBeneficiario_; }
            set { IDBeneficiario_ = value; }
        }

        Parentesco parentesco_ = new Parentesco();
        public String Parentesco
        {
            get { return parentesco_.parentesco; }
            set { parentesco_.parentesco = value; }
        }
    }
}
