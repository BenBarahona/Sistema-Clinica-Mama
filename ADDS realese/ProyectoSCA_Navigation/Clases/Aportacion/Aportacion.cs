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
    public class Aportacion
    {
        float[] interes_;
        public float[] Interes_
        {
            get { return interes_; }
            set { interes_ = value; }
        }
        
        String descripcion_;
        public String Descripcion
        {
            get { return descripcion_; }
            set { descripcion_ = value; }
        }
        
        float monto_;
        public float Monto
        {
            get { return monto_; }
            set { monto_ = value; }
        }

        int IDAportacion_;
        public int IDAportacion
        {
            get { return IDAportacion_; }
            set { IDAportacion_ = value; }
        }

        String fechaRealizacion_;
        public String FechaRealizacion
        {
            get { return fechaRealizacion_; }
            set { fechaRealizacion_ = value; }
        }

        int IDAfiliado_;
        public int IDAfiliado
        {
            get { return IDAfiliado_; }
            set { IDAfiliado_ = value; }
        }
    }
}
