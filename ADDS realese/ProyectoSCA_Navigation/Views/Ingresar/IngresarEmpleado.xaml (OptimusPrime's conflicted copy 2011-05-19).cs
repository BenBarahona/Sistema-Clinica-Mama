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
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace ProyectoSCA_Navigation.Views
{
    public partial class IngresarEmpleado : Page
    {
        string usuario = "Usuario";

        //Flags, true, puede disparar el evento, false no
        bool[] flags = new bool[10];
        /***    0 - Abrir Inserts
         ***    1 - Abrir Gets, luego Llenar campos de datos
         ***/
        int camposValidos = 0;
        
        //Expresiones Regulares para las validaciones
        Regex nombres = new Regex(@"^[A-Za-z][A-Za-zñ\s-]+$");
        Regex numeros = new Regex("^[0-9]*$");
        Regex alphanumerico = new Regex(@"^\w+$");
        Regex email = new Regex(@"^[a-z0-9_.]+@[a-z0-9-]+\.[a-z]{2,4}$");
        Regex numeroIdentidad = new Regex("^[0-9]{4}-[0-9]{4}-[0-9]{5}$");
        Regex numeroTelefono = new Regex("^([(][0-9]{3}[)][0-9]{8}|[0-9]{8})$");


        /********************************************************************************************************************************
        **************************************************    Inicializadores   *********************************************************
        *********************************************************************************************************************************/

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private ProyectoSCA_Navigation.ServiceReference.WSCosecolSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:1809/WSCosecol.asmx";

        Clases.TC tc = new Clases.TC();

        public IngresarEmpleado()
        {
            InitializeComponent();
            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReference.WSCosecolSoapClient(bind, endpoint);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DateTime fecha = DateTime.Today;
            fechaRegistro_txt.Content = fecha;
            genero_txt.Items.Add("Masculino");
            genero_txt.Items.Add("Femenino");
            estadoCivil_txt.Items.Add("Soltero");
            estadoCivil_txt.Items.Add("Casado");
            estadoCivil_txt.Items.Add("Divorciado");
            estadoCivil_txt.Items.Add("Viudo");
            estadoCivil_txt.Items.Add("Union Libre");
            
            genero_txt.SelectedIndex = 0;
            estadoCivil_txt.SelectedIndex = 0;
            for (int i = 0; i < flags.Length; i++)
            {
                flags[i] = false;
            }
            registrar_btn.IsEnabled = false;
            
        }

        /********************************************************************************************************************************
        *****************************************************    Metodo PRINCIPAL  ******************************************************
        *********************************************************************************************************************************/
        
        //es el metodo que corre cuando se dispara el evento de recibir respuesta, aqui van todas las respuestas
        //con los codigos que le pertenecen, consultar TABLA DE PETICIONES.XLSX

        private void Wrapper_AgregarPeticionCompleted(object sender, ServiceReference.AgregarPeticionCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string temp = e.Result.Substring(0, 3);

                switch (temp)
                {
                    case "p00"://Ingresar Empleado
                        if (flags[0])
                        {
                            MessageBox.Show(e.Result);

                            flags[0] = false;
                            habilitarCampos(true);
                        }
                        break;

                    default:
                        MessageBox.Show(e.Result.ToString());
                        habilitarCampos(true);
                        break;
                }
            }
            else
            {
                MessageBox.Show(e.Error.ToString());
                habilitarCampos(true);
            }
        }


        /********************************************************************************************************************************
        **************************************************    Metodos de Botones    *****************************************************
        *********************************************************************************************************************************/
        private void registrar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (camposValidos < 9)
            {
                MessageBox.Show("Ingrese todos los valores marcados con *");
                return;
            }

            flags[0] = true;
            flags[1] = true;

            Clases.Empleado empleado = new Clases.Empleado();
            List<string> telefono = new List<string>();
            telefono.Add(telefono_txt.Text);
            List<string> celular = new List<string>();
            telefono.Add(celular_txt.Text);

            empleado.primerNombre = pNombre_txt.Text;
            empleado.segundoNombre = sNombre_txt.Text;
            empleado.primerApellido = pApellido_txt.Text;
            empleado.segundoApellido = sApellido_txt.Text;
            empleado.direccion = direccion_txt.Text;
            empleado.identidad = id_txt.Text;
            empleado.genero = genero_txt.SelectedItem.ToString();
            empleado.telefonoPersonal = telefono;
            empleado.celular = celular;
            empleado.Puesto = puesto_txt.Text;
            empleado.fechaInicioLabores = fechaEmpresa_txt.Text;
            empleado.correoElectronico = correo_txt.Text;
            empleado.fechaNacimiento = fechaNacimiento_txt.Text;
            empleado.estadoCivil = estadoCivil_txt.SelectedItem.ToString();
            empleado.Password = contrasena_txt.Text;
            habilitarCampos(false);

            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.ingresar_empleado, tc.InsertarEmpleado(empleado, usuario));

        }


        /********************************************************************************************************************************
        **************************************************    Validaciones!!   **********************************************************
        *********************************************************************************************************************************/
        private void pNombre_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            pNombre_txt.Text.Trim();
            if (nombres.IsMatch(pNombre_txt.Text))
            {
                camposValidos++;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                pNombre_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos--;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                pNombre_valid.Source = new BitmapImage(imgURI);
            }
        }

        private void sNombre_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            camposValidos++;
            sNombre_txt.Text.Trim();
            if (nombres.IsMatch(sNombre_txt.Text))
            {
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                sNombre_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos--;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                sNombre_valid.Source = new BitmapImage(imgURI);
            }
        }

        private void pApellido_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            pApellido_txt.Text.Trim();
            if (nombres.IsMatch(pApellido_txt.Text))
            {
                camposValidos++;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                pApellido_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos--;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                pApellido_valid.Source = new BitmapImage(imgURI);
            }
        }

        private void sApellido_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            camposValidos++;
            sApellido_txt.Text.Trim();
            if (nombres.IsMatch(sApellido_txt.Text))
            {
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                sApellido_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos--;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                sApellido_valid.Source = new BitmapImage(imgURI);
            }
        }

        private void telefono_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            camposValidos++;
            telefono_txt.Text.Trim();
            if (numeroTelefono.IsMatch(telefono_txt.Text))
            {
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                telefono_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos--;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                telefono_valid.Source = new BitmapImage(imgURI);
            }
        }

        private void celular_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            camposValidos++;
            celular_txt.Text.Trim();
            if (numeroTelefono.IsMatch(celular_txt.Text))
            {
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                celular_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos--;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                celular_valid.Source = new BitmapImage(imgURI);
            }
        }

        private void correo_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            correo_txt.Text.Trim();
            if (email.IsMatch(correo_txt.Text))
            {
                camposValidos++;
                mensaje_txt.Text = "*Campos Obligatorios";
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                correo_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos--;
                registrar_btn.IsEnabled = false;
                mensaje_txt.Text = "Formato Valido de Correo: ejemplo@dominio.com";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                correo_valid.Source = new BitmapImage(imgURI);
            }
        }

        private void puesto_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            puesto_txt.Text.Trim();
            if (nombres.IsMatch(puesto_txt.Text))
            {
                camposValidos++;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                puesto_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos--;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                puesto_valid.Source = new BitmapImage(imgURI);
            }
        }

        private void contrasena2_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (contrasena2_txt.Text == contrasena_txt.Text)
            {
                camposValidos++;
                mensaje_txt.Text = "*Campos Obligatorios";
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                puesto_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos--;
                mensaje_txt.Text = "Las contraseñas no coinciden!";
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                puesto_valid.Source = new BitmapImage(imgURI);
            }
        }

        private void id1_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            id_txt.Text.Trim();
            if (numeroIdentidad.IsMatch(id_txt.Text))
            {
                camposValidos++;
                mensaje_txt.Text = "*Campos Obligatorios";
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos--;
                mensaje_txt.Text = "Formato valido de Identidad: 0000-0000-00000";
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_valid.Source = new BitmapImage(imgURI);
            }
        }

        /********************************************************************************************************************************
        **************************************************    Metodos Auxiliares    *****************************************************
        *********************************************************************************************************************************/

        private void habilitarCampos(bool valor)
        {
            insertarEmpleado_control.IsEnabled = valor;
        }

        private void pNombre_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (nombres.IsMatch(pNombre_txt.Text))
            {
                camposValidos++;
            }
        }

        private void sNombre_txt_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void pApellido_txt_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void sApellido_txt_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void id_txt_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void telefono_txt_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void celular_txt_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void puesto_txt_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void correo_txt_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void contrasena2_txt_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
