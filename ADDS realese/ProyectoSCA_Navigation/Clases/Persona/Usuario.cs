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
    public class Usuario
    {      
        Rol rol_;
        public String Rol_s
        {
            get { return rol_.nombre; }
            set { rol_.nombre = value; }
        }

        int IDUsuario_;
        public int IDUsuario
        {
            get { return this.IDUsuario_; }
            set { this.IDUsuario_ = value; }
        }

        String correoElectronico_;
        public String correoElectronico
        {
            get { return this.correoElectronico_; }
            set { this.correoElectronico_ = value; }
        }

        String password_;
        public String Password
        {
            get { return this.password_; }
            set { this.password_ = value; }
        }

    }
}
