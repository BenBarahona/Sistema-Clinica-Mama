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
    public class reporteEstadoCuentaTodosGrid
    {
        public int Num_Certificado { get; set; }
        public float Monto { get; set; }
        public String Descripcion { get; set; }
        public String fechaPlazo { get; set; }
        public String fechaRealizacion { get; set; }
        public String Motivo { get; set; }
        public String Cancelado { get; set; }
    }
}
