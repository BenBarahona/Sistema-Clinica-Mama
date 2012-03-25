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
    public class BeneficiarioGrid
    {
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string NumeroIdentidad { get; set; }
        public string TipoBeneficiario { get; set; }
        public float PorcentajeAportaciones { get; set; }
        public float PorcentajeSeguros { get; set; }
        public string Genero { get; set; }
        public string Parentesco { get; set; }
        public string Direccion { get; set; }
        public string Fecha_Nacimiento { get; set; }
    }
}
