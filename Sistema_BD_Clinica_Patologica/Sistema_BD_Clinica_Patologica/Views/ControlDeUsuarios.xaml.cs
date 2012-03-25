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
using System.Windows.Media.Imaging;

namespace Sistema_BD_Clinica_Patologica.Views
{
    public partial class ControlDeUsuarios : Page
    {

        String nombreUsuario = "";
        String[] Accesos = {"", "", "", "", ""};
        //Flags
        bool mainFlag = true;
        bool flag2 = true;

        /********************************************************************************************************************************
        **************************************************    Inicializadores   *********************************************************
        *********************************************************************************************************************************/

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private Sistema_BD_Clinica_Patologica.ServiceReferenceClinica.WSClinicaSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:5633/WSClinica.asmx";

        public ControlDeUsuarios()
        {
            InitializeComponent();
            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReferenceClinica.WSClinicaSoapClient(bind, endpoint);

            enableButtons(false);
            estado.Text = "Obteniendo Datos...";
            mainFlag = true;
            Wrapper.getUsuariosCompleted += new EventHandler<ServiceReferenceClinica.getUsuariosCompletedEventArgs>(getUsuarios_Completed);
            Wrapper.getUsuariosAsync();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /********************************************************************************************************************************
        ***********************************************    MAIN BUTTON EVENTS   *********************************************************
        *********************************************************************************************************************************/
        private void buscar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioNombre_auto.Text.CompareTo("") == 0)
            {
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                nombreAuto_error.Source = new BitmapImage(imgURI);
                estado.Text = "Escriba el nombre de un usuario";
                return;
            }

            nombreUsuario = usuarioNombre_auto.Text;
            estado.Text = "Obteniendo Datos de los Usuarios...";
            enableButtons(false);
            mainFlag = true;
            flag2 = true;

            Wrapper.getDatosEmpleadoCompleted += new EventHandler<ServiceReferenceClinica.getDatosEmpleadoCompletedEventArgs>(WebS_getEmpleado);
            Wrapper.getDatosEmpleadoAsync(nombreUsuario);
        }

        private void guardar_btn5_Click(object sender, RoutedEventArgs e)
        {
            if (usuario_txt.Text.Length == 0)
            {
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_error.Source = new BitmapImage(imgURI);
                estado.Text = "Escriba el nombre de un usuario";
                return;
            }

            if (passwordBox1.Password.Length == 0)
            {
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                password_error.Source = new BitmapImage(imgURI);
                estado.Text = "Ingrese una contraseña...";
                return;
            }

            if (passwordBox2.Password.Length == 0)
            {
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                password2_error.Source = new BitmapImage(imgURI);
                estado.Text = "Confirme su contraseña...";
                return;
            }


            if (passwordBox2.Password.CompareTo(passwordBox1.Password) != 0)
            {
                estado.Text = "Las contraseñas no concuerdan...";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                password_error.Source = new BitmapImage(imgURI);
                password2_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (nombre_txt.Text.Length == 0)
            {
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                nombre_error.Source = new BitmapImage(imgURI);
                estado.Text = "Ingrese el nombre del usuario...";
                return;
            }

            if (apellido_txt.Text.Length == 0)
            {
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                apellido_error.Source = new BitmapImage(imgURI);
                estado.Text = "Ingrese el apellido del usuario...";
                return;
            }

            String accesosFinal = "";
            for (int i = 0; i < Accesos.Length; i++)
            {
                if (Accesos[i].CompareTo("") != 0)
                    accesosFinal += Accesos[i] + "&";
            }
            if (accesosFinal.Length == 0)
            {
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                acceso_error.Source = new BitmapImage(imgURI);
                estado.Text = "El usuario debe tener almenos 1 Acceso";
                return;
            }

            accesosFinal = accesosFinal.Substring(0, accesosFinal.Length - 1);


            enableButtons(false);
            mainFlag = true;
            flag2 = true;

            estado.Text = "Guardando Usuario...";

            Wrapper.InsertarEmpleadoCompleted += new EventHandler<ServiceReferenceClinica.InsertarEmpleadoCompletedEventArgs>(WebS_EmpleadoInsertado);
            Wrapper.InsertarEmpleadoAsync(usuario_txt.Text, tipoEmpleado_txt.Text, nombre_txt.Text, "", apellido_txt.Text, "", passwordBox1.Password, correo_txt.Text, accesosFinal);
        }

        private void actualizar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (usuario_txt.Text.Length == 0)
            {
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_error.Source = new BitmapImage(imgURI);
                estado.Text = "Escriba el nombre de un usuario";
                return;
            }

            if (passwordBox1.Password.Length == 0)
            {
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                password_error.Source = new BitmapImage(imgURI);
                estado.Text = "Ingrese una contraseña...";
                return;
            }

            if (passwordBox2.Password.Length == 0)
            {
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                password2_error.Source = new BitmapImage(imgURI);
                estado.Text = "Confirme su contraseña...";
                return;
            }


            if (passwordBox2.Password.CompareTo(passwordBox1.Password) != 0)
            {
                estado.Text = "Las contraseñas no concuerdan...";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                password_error.Source = new BitmapImage(imgURI);
                password2_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (nombre_txt.Text.Length == 0)
            {
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                nombre_error.Source = new BitmapImage(imgURI);
                estado.Text = "Ingrese el nombre del usuario...";
                return;
            }

            if (apellido_txt.Text.Length == 0)
            {
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                apellido_error.Source = new BitmapImage(imgURI);
                estado.Text = "Ingrese el apellido del usuario...";
                return;
            }

            String accesosFinal = "";
            for (int i = 0; i < Accesos.Length; i++)
            {
                if (Accesos[i].CompareTo("") != 0)
                    accesosFinal += Accesos[i] + "&";
            }
            if (accesosFinal.Length == 0)
            {
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                acceso_error.Source = new BitmapImage(imgURI);
                estado.Text = "El usuario debe tener almenos 1 Acceso";
                return;
            }

            accesosFinal = accesosFinal.Substring(0, accesosFinal.Length - 1);


            enableButtons(false);
            mainFlag = true;
            flag2 = true;

            estado.Text = "Actualizando Usuario...";

            Wrapper.ActualizarUsuarioCompleted += new EventHandler<ServiceReferenceClinica.ActualizarUsuarioCompletedEventArgs>(ActualizarEmpleadoCompleted);
            Wrapper.ActualizarUsuarioAsync(usuario_txt.Text, tipoEmpleado_txt.Text, nombre_txt.Text, "", apellido_txt.Text, "", passwordBox1.Password, correo_txt.Text, accesosFinal);
        }

        private void borrar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (usuario_txt.Text.CompareTo("") == 0)
            {
                estado.Text = "Escriba el nombre del usuario que desea borrar!";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_error.Source = new BitmapImage(imgURI);
                return;
            }

            MessageBoxResult msgResult = MessageBox.Show("Esta seguro que desea Eliminar este usuario?  Esta acción no se puede revertir.",
                "Borrando Usuario", System.Windows.MessageBoxButton.OKCancel);
            if (msgResult == MessageBoxResult.Cancel)
                return;

            mainFlag = true;
            flag2 = true;

            enableButtons(false);
            estado.Text = "Eliminando Empleado";
            Wrapper.BorrarEmpleadoCompleted += new EventHandler<ServiceReferenceClinica.BorrarEmpleadoCompletedEventArgs>(WebS_empleadoEliminado);
            Wrapper.BorrarEmpleadoAsync(usuario_txt.Text);
        }

        private void resetFields_Click(object sender, RoutedEventArgs e)
        {
            resetFields();
        }


        /********************************************************************************************************************************
        *********************************************************************************************************************************
        *********************************************                           *********************************************************
        *********************************************   METHOD RESPONCE EVENTS  *********************************************************
        *********************************************                           *********************************************************
        *********************************************************************************************************************************
        *********************************************************************************************************************************/
        void getUsuarios_Completed(object sender, ServiceReferenceClinica.getUsuariosCompletedEventArgs e)
        {
            if (mainFlag)
            {
                mainFlag = false;
                enableButtons(true);
                if (!e.Result.ToString().Equals(""))
                {
                    List<datosUsuario> valoresGrid = new List<datosUsuario>();
                    List<String> empleados = new List<String>();
                    String[] resultado = e.Result.ToString().Split(';');

                    for (int i = 0; i < resultado.Length; i++)
                    {
                        if (i % 7 == 0)
                        {
                            empleados.Add(resultado[i]);
                            valoresGrid.Add(new datosUsuario()
                            {
                                Usuario = resultado[i],
                                Nombre = resultado[i + 1] + " " + resultado[i + 2] + " " + resultado[i + 3] + " " + resultado[i + 4],
                                Rol = resultado[i + 5],
                                Correo = resultado[i + 6],
                            });
                        }
                    }
                    usuarioNombre_auto.ItemsSource = empleados;

                    usuarios_grid.ItemsSource = valoresGrid;
                    estado.Text = "Usuarios Actualizados!";
                }
                else
                {
                    estado.Text = "No existen usuarios registrados";
                }
            }
        }

        void WebS_getEmpleado(object sender, ServiceReferenceClinica.getDatosEmpleadoCompletedEventArgs e)
        {
            enableButtons(true);
            if (mainFlag)
            {
                mainFlag = false;
                if (e.Result.ToString().CompareTo("") != 0)
                {
                    String[] resultado = e.Result.ToString().Split(';');
                    resetFields();

                    usuario_txt.Text = resultado[0];
                    tipoEmpleado_txt.Text = resultado[1];
                    nombre_txt.Text = resultado[2] + " " + resultado[3];
                    apellido_txt.Text = resultado[4] + " " + resultado[5];
                    passwordBox1.Password = resultado[6];
                    passwordBox2.Password = resultado[6];
                    correo_txt.Text = resultado[7];
                    

                    Wrapper.getAccesosDeUsuarioCompleted += new EventHandler<ServiceReferenceClinica.getAccesosDeUsuarioCompletedEventArgs>(WebS_getAccesos);
                    Wrapper.getAccesosDeUsuarioAsync(nombreUsuario);
                }
                else
                {
                    estado.Text = "No se encontraron datos";
                }
            }
        }

        void WebS_getAccesos(object sender, ServiceReferenceClinica.getAccesosDeUsuarioCompletedEventArgs e)
        {
            if (flag2)
            {
                flag2 = false;
                String[] modulos = e.Result.ToString().Split(';');

                for (int i = 0; i < modulos.Length; i++)
                {
                    switch (modulos[i].ToString())
                    {
                        case "Control De Usuarios":
                            controlUsuario_check.IsChecked = true;
                            break;
                        case "Consultas":
                            consultas_check.IsChecked = true;
                            break;
                        case "Ingresos":
                            ingresos_check.IsChecked = true;
                            break;
                        case "Egresos":
                            egresos_check.IsChecked = true;
                            break;
                        case "Examenes":
                            examenes_check.IsChecked = true;
                            break;
                    }
                }
                estado.Text = "Datos Recibidos";
            }
        }

        void WebS_EmpleadoInsertado(object sender, ServiceReferenceClinica.InsertarEmpleadoCompletedEventArgs e)
        {
            enableButtons(true);
            if (flag2)
            {
                flag2 = false;
                if (e.Result)
                {
                    resetFields();
                    estado.Text = "El empleado fue Insertado";
                    Wrapper.getUsuariosCompleted += new EventHandler<ServiceReferenceClinica.getUsuariosCompletedEventArgs>(getUsuarios_Completed);
                    Wrapper.getUsuariosAsync();

                }
                else
                {
                    estado.Text = "El Empleado no se pudo insertar...";
                }
            }
        }

        void WebS_empleadoEliminado(object sender, ServiceReferenceClinica.BorrarEmpleadoCompletedEventArgs e)
        {
            enableButtons(true);
            if (e.Result)
            {
                estado.Text = "Empleado Eliminado... Actualizando Empleados";
                resetFields();

                Wrapper.getUsuariosCompleted += new EventHandler<ServiceReferenceClinica.getUsuariosCompletedEventArgs>(getUsuarios_Completed);
                Wrapper.getUsuariosAsync();

            }
            else
                estado.Text = "No existe un empleado con el nombre ingresado.";

        }

        void ActualizarEmpleadoCompleted(object sender, ServiceReferenceClinica.ActualizarUsuarioCompletedEventArgs e)
        {
            enableButtons(true);
            if (flag2)
            {
                flag2 = false;
                if (e.Result)
                {
                    resetFields();
                    estado.Text = "El empleado fue Actualizado";
                    Wrapper.getUsuariosCompleted += new EventHandler<ServiceReferenceClinica.getUsuariosCompletedEventArgs>(getUsuarios_Completed);
                    Wrapper.getUsuariosAsync();

                }
                else
                {
                    estado.Text = "El Empleado no se pudo actualizar...";
                }
            }
        }

        /************************************************************************************
        ************************************************************************************
        *                                                                                  *
        *                      VALIDACIONES Y UTILIDADES                                   *
        *                                                                                  *  
        ************************************************************************************
        ************************************************************************************/

        public void enableButtons(bool value)
        {
            buscar_btn.IsEnabled = value;
            guardar_btn.IsEnabled = value;
            actualizar_btn.IsEnabled = value;
            borrar_btn.IsEnabled = value;
            resetFields_btn.IsEnabled = value;

            if (value)
            {
                buscar_icon.Opacity = 1;
                guardar_icon.Opacity = 1;
                actualizar_icon.Opacity = 1;
                borrar_icon.Opacity = 1;
                reset_icon.Opacity = 1;
            }
            else
            {
                buscar_icon.Opacity = 0.5;
                guardar_icon.Opacity = 0.5;
                actualizar_icon.Opacity = 0.5;
                borrar_icon.Opacity = 0.5;
                reset_icon.Opacity = 0.5;
            }
        }

        public void resetFields()
        {
            usuarioNombre_auto.Text = "";
            usuario_txt.Text = "";
            passwordBox1.Password = "";
            passwordBox2.Password = "";
            nombre_txt.Text = "";
            apellido_txt.Text = "";
            correo_txt.Text = "";
            tipoEmpleado_txt.Text = "";

            controlUsuario_check.IsChecked = false;
            ingresos_check.IsChecked = false;
            egresos_check.IsChecked = false;
            consultas_check.IsChecked = false;
        }

        private void usuarioNombre_auto_TextChanged(object sender, RoutedEventArgs e)
        {
            nombreAuto_error.Source = MainPage.bmpClear;
        }

        private void controlUsuario_check_Checked(object sender, RoutedEventArgs e)
        {
            Accesos[0] = "Control De Usuarios";
            acceso_error.Source = MainPage.bmpClear;
        }

        private void controlUsuario_check_Unchecked(object sender, RoutedEventArgs e)
        {
            Accesos[0] = "";
            acceso_error.Source = MainPage.bmpClear;
        }

        private void consultas_check_Checked(object sender, RoutedEventArgs e)
        {
            acceso_error.Source = MainPage.bmpClear;
            Accesos[1] = "Consultas";
        }

        private void consultas_check_Unchecked(object sender, RoutedEventArgs e)
        {
            Accesos[1] = "";
            acceso_error.Source = MainPage.bmpClear;
        }

        private void examenes_check_Checked(object sender, RoutedEventArgs e)
        {
            Accesos[2] = "Examenes";
            acceso_error.Source = MainPage.bmpClear;
        }

        private void examenes_check_Unchecked(object sender, RoutedEventArgs e)
        {
            Accesos[2] = "";
            acceso_error.Source = MainPage.bmpClear;
        }

        private void ingresos_check_Checked(object sender, RoutedEventArgs e)
        {
            Accesos[3] = "Ingresos";
            acceso_error.Source = MainPage.bmpClear;
        }

        private void ingresos_check_Unchecked(object sender, RoutedEventArgs e)
        {
            Accesos[3] = "";
            acceso_error.Source = MainPage.bmpClear;
        }

        private void egresos_check_Checked(object sender, RoutedEventArgs e)
        {
            Accesos[4] = "Egresos";
            acceso_error.Source = MainPage.bmpClear;
        }

        private void egresos_check_Unchecked(object sender, RoutedEventArgs e)
        {
            Accesos[4] = "";
            acceso_error.Source = MainPage.bmpClear;
        }

        private void passwordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            password_error.Source = MainPage.bmpClear;
            password2_error.Source = MainPage.bmpClear;
        }

        private void passwordBox2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            password_error.Source = MainPage.bmpClear;
            password2_error.Source = MainPage.bmpClear;
        }

        private void nombre_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            nombre_error.Source = MainPage.bmpClear;
        }

        private void apellido_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            apellido_error.Source = MainPage.bmpClear;
        }



    }
}
