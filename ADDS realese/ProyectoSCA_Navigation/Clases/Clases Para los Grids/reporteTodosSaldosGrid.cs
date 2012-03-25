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
    public class reporteTodosSaldosGrid
    {
        public int Num_Certificado { get; set; }
        public String Nombre { get; set; }
        public float Total_AportacionesO { get; set; }
        public float Total_AportacionesV { get; set; }
        public float Total_Intereses { get; set; }
        public float Saldo_Actual { get; set; }
    }
}