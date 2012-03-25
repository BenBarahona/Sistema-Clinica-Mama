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
        string usuario = App.Correo;
        //string usuario = "Usuario";
        //Flags, true, puede disparar el evento, false no
        bool[] flags = new bool[10];
        /***    0 - Abrir Inserts
         ***    1 - Abrir Gets, luego Llenar campos de datos
         ***    2 - Error Windows
         ***/
        bool[] camposValidos = new bool[10];
        
        /*
         * Esta variable es para saber si el usuario ingresado es uno normal o un usuario administrativo
         * FALSE: Usuario Normal
         * TRUE: Usuario Administrativo
        */  
        bool tipoUsuario = false;

        //Expresiones Regulares para las validaciones
        Regex nombres = new Regex(@"^[A-Za-z][A-Za-zñ\s-]+$");
        Regex numeros = new Regex("^[0-9]*$");
        Regex porcentaje = new Regex(@"^([0-9]|[1-9]\d|100)$");
        Regex alphanumerico = new Regex(@"^[\w\s]+$");
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
            DateTime fecha = DateTime.Now;
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
            for (int i = 0; i < camposValidos.Length; i++)
            {
                camposValidos[i] = true;
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
                string recievedResponce = e.Result.Substring(3);
                if (recievedResponce == "False")
                {
                    if (flags[2])
                    {
                        flags[2] = false;
                        MessageBox.Show("Ha ocurrido un error.  Intente su consulta de nuevo.\nSi el problema persiste, refresque la pagina e intente de nuevo.");
                        habilitarCampos(true);
                    }
                }
                else
                {
                    switch (temp)
                    {
                        case "p00"://Ingresar Empleado
                            if (flags[0])
                            {
                                MessageBox.Show("Se ha ingresado el empleado exitosamente!");
                                flags[0] = false;
                                NavigationService.Refresh();
                            }
                            break;

                        default:
                            if (flags[2])
                            {
                                flags[2] = false;
                                MessageBox.Show("Ha ocurrido un error.  Intente su consulta de nuevo.\nSi el problema persiste, refresque la pagina e intente de nuevo.");
                                habilitarCampos(true);
                            }
                            break;
                    }
                }
            }
            else
            {
                if (flags[2])
                {
                    flags[2] = false;
                    MessageBox.Show("Ha ocurrido un error.  Intente su consulta de nuevo.\nSi el problema persiste, refresque la pagina e intente de nuevo.");
                    habilitarCampos(true);
                }
            }
        }


        /********************************************************************************************************************************
        **************************************************    Metodos de Botones    *****************************************************
        *********************************************************************************************************************************/
        private void registrar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (pNombre_txt.Text == "" || pApellido_txt.Text == "" || id_txt.Text == "" || telefono_txt.Text == ""
                || celular_txt.Text == "" || puesto_txt.Text == "" || direccion_txt.Text == "" ||
                 correo_txt.Text == "" || contrasena_txt.Password == "" || contrasena2_txt.Password == "")
            {
                MessageBox.Show("Ingrese todos los valores marcados con *");
                return;
            }
            if (fechaNacimiento_txt.Text == "" || fechaEmpresa_txt.Text == "")
            {
                MessageBox.Show("Ingrese una fecha valida!");
                return;
            }

            for(int i = 0; i < camposValidos.Length; i++)
            {
                if (!camposValidos[i])
                {
                    MessageBox.Show("Ingrese un dato valido para los campos marcados");
                    return;
                }
            }
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;

            Clases.Empleado empleado = new Clases.Empleado();
            List<string> telefono = new List<string>();
            telefono.Add(telefono_txt.Text);
            List<string> celular = new List<string>();
            celular.Add(celular_txt.Text);

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
            empleado.Password = contrasena_txt.Password;
            habilitarCampos(false);
            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.ingresar_empleado, MainPage.tc.InsertarEmpleado(empleado, usuario, tipoUsuario));

        }


        /********************************************************************************************************************************
        **************************************************    Validaciones!!   **********************************************************
        *********************************************************************************************************************************/
        private void pNombre_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            pNombre_txt.Text.Trim();
            if (nombres.IsMatch(pNombre_txt.Text))
            {
                camposValidos[0] = true;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                pNombre_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[0] = false;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                pNombre_valid.Source = new BitmapImage(imgURI);
            }
            if (pNombre_txt.Text == "")
            {
                camposValidos[0] = true;
                pNombre_valid.Source = MainPage.bmpClear;
            }
        }

        private void sNombre_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            sNombre_txt.Text.Trim();
            if (nombres.IsMatch(sNombre_txt.Text))
            {
                camposValidos[8] = true;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                sNombre_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[8] = false;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                sNombre_valid.Source = new BitmapImage(imgURI);
            }
            if (sNombre_txt.Text == "")
            {
                camposValidos[8] = true;
                sNombre_valid.Source = MainPage.bmpClear;
            }
        }

        private void pApellido_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            pApellido_txt.Text.Trim();
            if (nombres.IsMatch(pApellido_txt.Text))
            {
                camposValidos[1] = true;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                pApellido_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[1] = false;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                pApellido_valid.Source = new BitmapImage(imgURI);
            }
            if (pApellido_txt.Text == "")
            {
                camposValidos[1] = true;
                pApellido_valid.Source = MainPage.bmpClear;
            }
        }

        private void sApellido_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            sApellido_txt.Text.Trim();
            if (nombres.IsMatch(sApellido_txt.Text))
            {
                camposValidos[9] = true;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                sApellido_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[9] = false;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                sApellido_valid.Source = new BitmapImage(imgURI);
            }
            if (sApellido_txt.Text == "")
            {
                camposValidos[9] = true;
                sApellido_valid.Source = MainPage.bmpClear;
            }
        }

        private void telefono_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            telefono_txt.Text.Trim();
            if (numeroTelefono.IsMatch(telefono_txt.Text))
            {
                camposValidos[2] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                telefono_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[2] = false;
                registrar_btn.IsEnabled = false;
                mensaje_txt.Text = "Formato Valido: (504)0000000, ó 00000000";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                telefono_valid.Source = new BitmapImage(imgURI);
            }
            if (telefono_txt.Text == "")
            {
                camposValidos[2] = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                telefono_valid.Source = MainPage.bmpClear;
            }
        }

        private void celular_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            celular_txt.Text.Trim();
            if (numeroTelefono.IsMatch(celular_txt.Text))
            {
                camposValidos[3] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                celular_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[3] = true;
                registrar_btn.IsEnabled = false;
                mensaje_txt.Text = "Formato Valido: (504)0000000, ó 00000000";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                celular_valid.Source = new BitmapImage(imgURI);
            }
            if (celular_txt.Text == "")
            {
                camposValidos[3] = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                celular_valid.Source = MainPage.bmpClear;
            }
        }

        private void correo_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            correo_txt.Text.Trim();
            if (email.IsMatch(correo_txt.Text))
            {
                camposValidos[4] = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                correo_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[4] = true;
                registrar_btn.IsEnabled = false;
                mensaje_txt.Text = "Formato Valido de Correo: ejemplo@dominio.com";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                correo_valid.Source = new BitmapImage(imgURI);
            }
            if (correo_txt.Text == "")
            {
                camposValidos[4] = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                correo_valid.Source = MainPage.bmpClear;
            }
        }

        private void puesto_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            puesto_txt.Text.Trim();
            if (nombres.IsMatch(puesto_txt.Text))
            {
                camposValidos[5] = true;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                puesto_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[5] = false;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                puesto_valid.Source = new BitmapImage(imgURI);
            }
            if (puesto_txt.Text == "")
            {
                camposValidos[5] = true;
                puesto_valid.Source = MainPage.bmpClear;
            }
        }

        private void contrasena2_txt_TextChanged(object sender, RoutedEventArgs e)
        {
            if (contrasena2_txt.Password == contrasena_txt.Password)
            {
                camposValidos[6] = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                password1_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[6] = false;
                mensaje_txt.Text = "Las contraseñas no coinciden!";
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                password1_valid.Source = new BitmapImage(imgURI);
            }
            if (contrasena2_txt.Password == "")
            {
                camposValidos[6] = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                password1_valid.Source = MainPage.bmpClear;
            }
        }

        private void id1_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            id_txt.Text.Trim();
            if (numeroIdentidad.IsMatch(id_txt.Text))
            {
                camposValidos[7] = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[7] = false;
                mensaje_txt.Text = "Formato valido de Identidad: 0000-0000-00000";
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_valid.Source = new BitmapImage(imgURI);
            }
            if (id_txt.Text == "")
            {
                camposValidos[7] = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                id_valid.Source = MainPage.bmpClear;
            }
        }

        /********************************************************************************************************************************
        **************************************************    Metodos Auxiliares    *****************************************************
        *********************************************************************************************************************************/

        private void habilitarCampos(bool valor)
        {
            insertarEmpleado_control.IsEnabled = valor;
        }

        private void usuarioNormal_Checked(object sender, RoutedEventArgs e)
        {
            tipoUsuario = false;
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            tipoUsuario = true;
        }

    }
}
