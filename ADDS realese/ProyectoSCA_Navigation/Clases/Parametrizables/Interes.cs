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
    public class Interes
    {
        String nombre_;
        Int64 tasa_, monton_voluntaria_;
        Boolean aplica_obligatoria_, aplica_voluntaria_, aplica_obligatoria_especial_, aplica_voluntaria_arriba_, valido_;

        public Boolean Valido
        {
            get { return valido_; }
            set { valido_ = value; }
        }

        public String Nombre
        {
            get { return nombre_; }
            set { nombre_ = value; }
        }

        public Int64 Tasa
        {
            get { return tasa_; }
            set { tasa_ = value; }
        }

        public Boolean Aplica_obligatoria
        {
            get { return aplica_obligatoria_; }
            set { aplica_obligatoria_ = value; }
        }

        public Boolean Aplica_voluntaria
        {
            get { return aplica_voluntaria_; }
            set { aplica_voluntaria_ = value; }
        }

        public Boolean Aplica_obligatoria_especial
        {
            get { return aplica_obligatoria_especial_; }
            set { aplica_obligatoria_especial_ = value; }
        }

        public Boolean Aplica_voluntaria_arriba
        {
            get { return aplica_voluntaria_arriba_; }
            set { aplica_voluntaria_arriba_ = value; }
        }

        public Int64 Monto_voluntaria
        {
            get { return monton_voluntaria_; }
            set { monton_voluntaria_ = value; }
        }
    }
}
