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
    public class controlDePagoGrid
    {
        public int ID { get; set; }
        public String Descripcion { get; set; }
        public float Monto { get; set; }
        public String Fecha_A_Pagar { get; set; }
        public String Motivo { get; set; }
        public String TipoAportacion { get; set; }
    }
}
