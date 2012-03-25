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
    public class Empleado : Persona
    {
        String puesto_;
        public String Puesto
        {
            get { return puesto_; }
            set { puesto_ = value; }
        }

        String Administrador_;
        public String Administrador
        {
            get { return this.Administrador_;}
            set { this.Administrador_ = value; }
        }

        Usuario usuario_ = new Usuario();
        public String correoElectronico
        {
            get { return usuario_.correoElectronico; }
            set { usuario_.correoElectronico = value; }
        }
        public String rol
        {
            get { return usuario_.Rol_s; }
            set { usuario_.Rol_s = value; }
        }
        public String Password
        {
            get { return this.usuario_.Password; }
            set { this.usuario_.Password = value; }
        }

        int IDEmpleado_;
        public int IDEmpleado
        {
            get { return this.IDEmpleado_; }
            set { this.IDEmpleado_ = value; }
        }

        String fechaInicioLabores_;
        public String fechaInicioLabores
        {
            get { return this.fechaInicioLabores_; } 
            set { this.fechaInicioLabores_ = value; }
        }
    }
}
