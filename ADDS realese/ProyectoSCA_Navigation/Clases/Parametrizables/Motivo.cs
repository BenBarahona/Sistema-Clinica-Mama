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
    public class Motivo
    {
        String nombre;
        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        
        String descripcion;
        public String Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        aportacionObligatoriaEspecial aportacion;
        public aportacionObligatoriaEspecial Aportacion
        {
            get { return aportacion; }
            set { aportacion = value; }
        }
    }
}
