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
    public class AportacionObligatoria : Aportacion
    {
        int idMonto_;
        public int IdMonto
        {
            get { return idMonto_; }
            set { idMonto_ = value; }
        }
        
        montoAportacionObligatoria monto;
        public montoAportacionObligatoria MontoAportacionObligatoria
        {
            get { return monto; }
            set { monto = value; }
        }

        String fechaPlazo_;
        public String FechaPlazo
        {
            get { return fechaPlazo_; }
            set { fechaPlazo_ = value; }
        }
    }
}
