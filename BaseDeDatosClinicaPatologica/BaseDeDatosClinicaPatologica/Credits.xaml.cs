using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BaseDeDatosClinicaPatologica
{
    public partial class Credits : UserControl
    {
        MyWebReference.clinicaPatologiaWebServiceSoapClient myWebReference;
        String usuario;

        public Credits(MyWebReference.clinicaPatologiaWebServiceSoapClient myWebReference, String usuario)
        {
            this.myWebReference = myWebReference;
            this.usuario = usuario;
            InitializeComponent();
            scrollViewer1.Height = Application.Current.Host.Content.ActualHeight - 15;
        }

        private void paginaPrincipal_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new mainPage(myWebReference, usuario);
        }
    }
}
