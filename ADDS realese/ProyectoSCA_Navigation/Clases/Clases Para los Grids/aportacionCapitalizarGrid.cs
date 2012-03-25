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
    public class aportacionCapitalizarGrid
    {
        public String No_Certificado { get; set; }
        public String Descripcion_Aportacion { get; set; }
        public String Monto { get; set; }
        public String Fecha_Plazo { get; set; }
        public String Fecha_Realizacion { get; set; }
        public String Motivo { get; set; }
        public String Nombre_Interes { get; set; }
        public String Tasa_Interes { get; set; }
        public String Monto_Generado { get; set; }
    }
}