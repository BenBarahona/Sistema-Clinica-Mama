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
    public class Permisos
    {
        int IDPermiso_;
        public int IDPermiso
        {
            get { return IDPermiso_; }
            set { IDPermiso_ = value; }
        }

        String nombrePermiso_;
        public String NombrePermiso
        {
            get { return nombrePermiso_; }
            set { nombrePermiso_ = value; }
        }
        
        int idRol_;
        public int IdRol
        {
            get { return idRol_; }
            set { idRol_ = value; }
        }
    }
}
