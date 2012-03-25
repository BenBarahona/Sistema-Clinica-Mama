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
    public partial class MainPage : UserControl
    {

        MyWebReference.clinicaPatologiaWebServiceSoapClient myWebReference = new MyWebReference.clinicaPatologiaWebServiceSoapClient();

        public MainPage()
        {
            InitializeComponent();
            myWebReference.ConectarCompleted += new EventHandler<MyWebReference.ConectarCompletedEventArgs>(WebS_ConectarCompleted);
            myWebReference.ConectarAsync();

            textBox1.IsEnabled = false;
            passwordBox1.IsEnabled = false;
            button1.IsEnabled = false;

        }//End public MainPage()

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            myWebReference.esUsuarioValidoCompleted +=
                new EventHandler<MyWebReference.esUsuarioValidoCompletedEventArgs>(WebS_esUsuarioValidoCompleted);
            myWebReference.esUsuarioValidoAsync(textBox1.Text, passwordBox1.Password);

            button1.IsEnabled = false;
            textBox1.IsEnabled = false;
            passwordBox1.IsEnabled = false;

        }//End private void button1_Click(object sender, RoutedEventArgs e)

        void WebS_ConectarCompleted(object sender, MyWebReference.ConectarCompletedEventArgs e)
        {

            if (e.Result)
            {
                estado.Content = "Conexión Exitosa!";
                textBox1.IsEnabled = true;
                passwordBox1.IsEnabled = true;
                button1.IsEnabled = true;
            }
            else
                estado.Content = "No se puede Conectar a la Base de Datos... Revise su conexión a Internet o Contacte al Administrador...";

        }//End void WebS_getMedicoCompleted(object sender, MyWebReference.HelloWorldCompletedEventArgs e)

        void WebS_esUsuarioValidoCompleted(object sender, MyWebReference.esUsuarioValidoCompletedEventArgs e)
        {
            if (e.Result)
            {
                this.Content = new mainPage(myWebReference, textBox1.Text);
            }
            else
            {
                estado.Content = "Usuario o Contraseña Incorrecta";
                textBox1.Text = "";
                passwordBox1.Password = "";
                button1.IsEnabled = true;
                textBox1.IsEnabled = true;
                passwordBox1.IsEnabled = true;
            }

        }//End void WebS_BorrarEmpleadoCompleted(object sender, MyWebReference.BorrarEmpleadoCompletedEventArgs  e)

        private void passwordBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                myWebReference.esUsuarioValidoCompleted +=
                new EventHandler<MyWebReference.esUsuarioValidoCompletedEventArgs>(WebS_esUsuarioValidoCompleted);
                myWebReference.esUsuarioValidoAsync(textBox1.Text, passwordBox1.Password);

                button1.IsEnabled = false;
                textBox1.IsEnabled = false;
                passwordBox1.IsEnabled = false;
            }
        }

    }//End public partial class MainPage : UserControl

}//End namespace BaseDeDatosClinicaPatologica
