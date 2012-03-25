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
using System.Windows.Navigation;

namespace BaseDeDatosClinicaPatologica
{
    public partial class ControlDeUsuario : Page
    {
        MyWebReference.clinicaPatologiaWebServiceSoapClient myWebReference;
        String Usuario = "";

        public void VaciarTextBox()
        {
            textBox1.Text = "";
            passwordBox1.Password = "";
            passwordBox2.Password = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            checkBox1.IsChecked = false;
            checkBox2.IsChecked = false;
            checkBox3.IsChecked = false;

        }

        public ControlDeUsuario(MyWebReference.clinicaPatologiaWebServiceSoapClient myWebReference, String Usuario)
        {
            InitializeComponent();
            this.myWebReference = myWebReference;
            this.Usuario = Usuario;

            button3.IsEnabled = false;
            button4.IsEnabled = false;
            comboBox1.IsEnabled = false;

            myWebReference.getEmpleadosCompleted += new EventHandler<MyWebReference.getEmpleadosCompletedEventArgs>(WebS_getEmpleados);
            myWebReference.getEmpleadosAsync();

        }

        void WebS_getEmpleados(object sender, MyWebReference.getEmpleadosCompletedEventArgs e)
        {

            String[] empleados = e.Result.ToString().Split(';');

            for (int i = comboBox1.Items.Count - 1; i > -1; i--)
                comboBox1.Items.RemoveAt(i);

            for (int i = 0; i < empleados.Length; i++)
                comboBox1.Items.Add(empleados[i]);
            comboBox1.IsEnabled = true;
            button3.IsEnabled = true;
            button4.IsEnabled = true;

            estado.Content = "Empleados Actualizados!";
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new mainPage(myWebReference, Usuario);
        }

        private void image1_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            myWebReference.BorrarEmpleadoCompleted += new EventHandler<MyWebReference.BorrarEmpleadoCompletedEventArgs>(WebS_empleadoEliminado);
            myWebReference.BorrarEmpleadoAsync(comboBox1.SelectedItem.ToString());
        }

        void WebS_empleadoEliminado(object sender, MyWebReference.BorrarEmpleadoCompletedEventArgs e)
        {
            if (e.Result)
            {
                estado.Content = "Empleado Eliminado... Actualizando Empleados";
                button3.IsEnabled = true;
                button4.IsEnabled = true;
                comboBox1.IsEnabled = true;

                myWebReference.getEmpleadosCompleted += new EventHandler<MyWebReference.getEmpleadosCompletedEventArgs>(WebS_getEmpleados);
                myWebReference.getEmpleadosAsync();

            }
            else
                estado.Content = "hubo un problema al eliminar el empleado...";



        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            estado.Content = "Obteniendo Datos del Empleado...";
            button4.IsEnabled = false;
            myWebReference.getEmpleadoCompleted += new EventHandler<MyWebReference.getEmpleadoCompletedEventArgs>(WebS_getEmpleado);
            myWebReference.getEmpleadoAsync(comboBox1.SelectedItem.ToString());



        }

        void WebS_getAccesos(object sender, MyWebReference.getAccesosCompletedEventArgs e)
        {
            String[] modulos = e.Result.ToString().Split(';');
            checkBox1.IsChecked = false;
            checkBox2.IsChecked = false;
            checkBox3.IsChecked = false;
            for (int i = 0; i < modulos.Length; i++)
                if (modulos[i].CompareTo("Control de Usuarios") == 0)
                    checkBox1.IsChecked = true;
            for (int i = 0; i < modulos.Length; i++)
                if (modulos[i].CompareTo("Contabilidad") == 0)
                    checkBox2.IsChecked = true;
            for (int i = 0; i < modulos.Length; i++)
                if (modulos[i].CompareTo("Informes") == 0)
                    checkBox3.IsChecked = true;

            estado.Content = "Datos Recibidos";
        }

        void WebS_getEmpleado(object sender, MyWebReference.getEmpleadoCompletedEventArgs e)
        {
            button4.IsEnabled = true;
            VaciarTextBox();
            String[] resultado = e.Result.ToString().Split(';');
            textBox1.Text = resultado[0];
            textBox8.Text = resultado[1];
            textBox4.Text = resultado[2];
            textBox5.Text = resultado[3];
            textBox6.Text = resultado[4];
            textBox7.Text = resultado[5];

            myWebReference.getAccesosCompleted +=
                 new EventHandler<MyWebReference.getAccesosCompletedEventArgs>(WebS_getAccesos);
            myWebReference.getAccesosAsync(comboBox1.SelectedItem.ToString());



        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

            if (textBox1.Text.Length == 0)
            {
                estado.Content = "El usuario esta vacio...";
                return;
            }


            if (passwordBox1.Password.Length == 0)
            {
                estado.Content = "Ingrese una contraseña...";
                return;
            }

            if (passwordBox2.Password.Length == 0)
            {
                estado.Content = "Reescriba la Contraseña...";
                return;
            }


            if (passwordBox2.Password.CompareTo(passwordBox1.Password) != 0)
            {
                estado.Content = "La Contraseña no concuerda...";
                return;
            }

            if (textBox4.Text.Length == 0)
            {
                estado.Content = "Ingrese el Primer Nombre...";
                return;
            }

            button2.IsEnabled = false;
            estado.Content = "Guardando Usuario...";
            myWebReference.InsertarEmpleadoCompleted += new EventHandler<MyWebReference.InsertarEmpleadoCompletedEventArgs>(WebS_EmpleadoInsertado);
            myWebReference.InsertarEmpleadoAsync(textBox1.Text, textBox8.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, passwordBox1.Password);

            myWebReference.InsertarAccesoCompleted += new EventHandler<MyWebReference.InsertarAccesoCompletedEventArgs>(WebS_AccesoInsertado);
            myWebReference.InsertarAccesoAsync("Examenes", textBox1.Text);

            if (checkBox1.IsChecked == true)
            {
                myWebReference.InsertarAccesoCompleted += new EventHandler<MyWebReference.InsertarAccesoCompletedEventArgs>(WebS_AccesoInsertado);
                myWebReference.InsertarAccesoAsync("Control de Usuarios", textBox1.Text);
            }

            if (checkBox2.IsChecked == true)
            {
                myWebReference.InsertarAccesoCompleted += new EventHandler<MyWebReference.InsertarAccesoCompletedEventArgs>(WebS_AccesoInsertado);
                myWebReference.InsertarAccesoAsync("Contabilidad", textBox1.Text);
            }

            if (checkBox3.IsChecked == true)
            {
                myWebReference.InsertarAccesoCompleted += new EventHandler<MyWebReference.InsertarAccesoCompletedEventArgs>(WebS_AccesoInsertado);
                myWebReference.InsertarAccesoAsync("Informes", textBox1.Text);
            }

        }

        void WebS_EmpleadoInsertado(object sender, MyWebReference.InsertarEmpleadoCompletedEventArgs e)
        {
            if (e.Result)
            {

                VaciarTextBox();
                estado.Content = "El empleado fue Insertado";
                button2.IsEnabled = true;

                button3.IsEnabled = false;
                button4.IsEnabled = false;
                comboBox1.IsEnabled = false;


                myWebReference.getEmpleadosCompleted += new EventHandler<MyWebReference.getEmpleadosCompletedEventArgs>(WebS_getEmpleados);
                myWebReference.getEmpleadosAsync();



            }
            else
            {
                estado.Content = "El Empleado no se pudo insertar...";
            }
        }

        private void button3_Click_1(object sender, RoutedEventArgs e)
        {
            estado.Content = "Eliminando Empleado";
            myWebReference.BorrarEmpleadoCompleted += new EventHandler<MyWebReference.BorrarEmpleadoCompletedEventArgs>(WebS_empleadoEliminado);
            myWebReference.BorrarEmpleadoAsync(comboBox1.SelectedItem.ToString());
            button3.IsEnabled = false;
        }

        void WebS_AccesoInsertado(object sender, MyWebReference.InsertarAccesoCompletedEventArgs e)
        {
            if (e.Result)
                estado.Content = "Se le ha otorgado acceso al empleado a ciertos modulos";
            else
                estado.Content = "No se pudo otorgar Acceso al empleado a ciertos modulos";

        }

    }
}
