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
using System.Collections.Generic;

namespace ProyectoSCA_Navigation.Clases
{
    public class Persona
    {
        int ID_;
        String primerNombre_, segundoNombre_, primerApellido_, segundoApellido_, estadoCivil_, direccion_, fechaNacimiento_, identidad_, genero_;
        List<String> tels = new List<string>();
        List<String> cels = new List<string>();

        public void actualizarDatos()
        {
            //actualizar datos de una persona
        }

        public List<String> telefonoPersonal
        {

            get { return tels; }
            set { this.tels = value; }
        }

        public int ID
        {
            get { return this.ID_; }
            set { this.ID_ = value; }
        }

        public String estadoCivil
        {
            get { return this.estadoCivil_; }
            set { this.estadoCivil_ = value; }
        }

        public String identidad
        {
            get { return this.identidad_; }
            set { this.identidad_ = value; }
        }

        public String primerNombre
        {
            get { return this.primerNombre_; }
            set { this.primerNombre_ = value; }
        }

        public String segundoNombre
        {
            get { return this.segundoNombre_; }
            set { this.segundoNombre_ = value; }
        }

        public String primerApellido
        {
            get { return this.primerApellido_; }
            set { this.primerApellido_ = value; }
        }

        public String segundoApellido
        {
            get { return this.segundoApellido_; }
            set { this.segundoApellido_ = value; }
        }

        public List<String> celular
        {
            get { return this.cels; }
            set { this.cels = value; }
        }

        public String genero
        {
            get { return this.genero_; }
            set { this.genero_ = value; }
        }

        public String direccion
        {
            get { return this.direccion_; }
            set { this.direccion_ = value; }
        }

        public String fechaNacimiento
        {
            get { return this.fechaNacimiento_; }
            set { this.fechaNacimiento_ = value; }
        }
    }
}
