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
    public class AportacionVoluntaria : Aportacion
    {
        Limite idLimite_;
        public int Limite
        {
            get { return idLimite_.cantidad; }
            set { idLimite_.cantidad = value; }
        }

        bool aceptada_;
        public bool estaAceptada
        {
            get { return this.aceptada_; }
            set { this.aceptada_ = value; }
        }
    }
}