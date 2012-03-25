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
    public class Rol
    {
        int IDUsuario_;
        public int IDUsuario
        {
            get { return IDUsuario_; }
            set { IDUsuario_ = value; }
        }

        int IDRol_;
        public int IDRol
        {
            get { return this.IDRol_; }
            set { this.IDRol_ = value; }
        }

        String[] permisos_;
        public String[] permisos
        {
            get { return this.permisos_; }
            set { this.permisos_ = value; }
        }
        
        String nombre_;
        public String nombre
        {
            get { return this.nombre_; }
            set { this.nombre_ = value; }
        }

    }
}
