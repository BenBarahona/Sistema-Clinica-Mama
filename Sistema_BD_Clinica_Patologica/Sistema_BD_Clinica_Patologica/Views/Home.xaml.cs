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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sistema_BD_Clinica_Patologica
{
    public partial class Home : Page
    {
        Login login = new Login();
        bool[] flags = new bool[3];
        string[] loginDatos;
        string[] Nombre;
        string[] Permisos;
        List<string> Permiso_List = new List<string>();
        String username = "";

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private Sistema_BD_Clinica_Patologica.ServiceReferenceClinica.WSClinicaSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:5633/WSClinica.asmx";

        public Home()
        {
            InitializeComponent();

            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReferenceClinica.WSClinicaSoapClient(bind, endpoint);

            for (int i = 0; i < flags.Length; i++)
            {
                flags[i] = false;
            }


            login.Closed += childWindow_Closed;
            if (!App.UserIsAuthenticated)
                login.Show();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void childWindow_Closed(object sender, EventArgs e)
        {
            
            Login login = (Login)sender;
            if (login.DialogResult == true)
            {
                flags[0] = true;
                flags[1] = true;
                flags[2] = true;
                username = login.Usuario;
                
                Wrapper.esUsuarioValidoCompleted += new EventHandler<ServiceReferenceClinica.esUsuarioValidoCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                Wrapper.esUsuarioValidoAsync(login.Usuario, login.Password);
            }
        }

        private void Wrapper_AgregarPeticionCompleted(object sender, ServiceReferenceClinica.esUsuarioValidoCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string recievedResponce = e.Result.ToString();

                if (recievedResponce.Equals("False"))
                {
                    if (flags[2])
                    {
                        flags[2] = false;
                        MessageBox.Show("Usuario o Password Incorrecta.  Intente de nuevo");
                        App.UserIsAuthenticated = false;
                        NavigationService.Refresh();
                    }
                }
                else
                {
                    if (flags[0])
                    {
                                
                        flags[0] = false;
                        loginDatos = recievedResponce.Split(';');
                        Nombre = loginDatos[0].Split(',');
                        Permisos = loginDatos[1].Split(',');

                        App.Username = "";
                        for(int i = 0; i < Nombre.Length; i++)
                            App.Username += Nombre[i] + " ";

                        App.Correo = login.Usuario;
                        for (int i = 0; i < Permisos.Length; i++)
                            Permiso_List.Add(Permisos[i]);
                        App.Permisos = Permiso_List;
                        App.UserIsAuthenticated = true;
                        AppEvents.Instance.UpdateMain(sender);
                        NavigationService.Refresh();
                                
                    }
                }
            }
            else
            {
                if (flags[2])
                {
                    flags[2] = false;
                    MessageBox.Show("Usuario o Password Incorrecta.  Intente de nuevo");
                    App.UserIsAuthenticated = false;
                }
            }
        }
    }

    public class AppEvents
    {
        public static readonly AppEvents Instance = new AppEvents();

        public delegate void UpdateMainHandler(object someObject);
        public event UpdateMainHandler OnUpdateMain;

        public void UpdateMain(Object someobject)
        {
            if (OnUpdateMain != null)
            {
                OnUpdateMain(someobject);
            }
        }
    }

    /************************************************************************************
 ************************************************************************************
 *                                                                                  *
 *                      CLASSES PARA VALORES DE DATAGRIDS                            *
 *                                                                                  *  
 ************************************************************************************
 ************************************************************************************/
    public class datosUsuario
    {
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }     
        public string Correo { get; set; }
    }
    public class medicos
    {
        public string ID { get; set; }
        public string Nombre { get; set; }
        public string Numero_Colegiación { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Direccion { get; set; }
        public string Compañía { get; set; }
    }

    public class dataMedico
    {
        public string Medico { get; set; }
        public string Ingresos { get; set; }
        public string Muestras { get; set; }
    }

    public class estadisticaMedico
    {
        public string Medico { get; set; }
        public string Muestras { get; set; }
    }

    public class dataEgreso
    {
        public string Categoria { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }

    public class dataExamen
    {
        public string Muestra { get; set; }
        public string Nombre { get; set; }
        public string Edad { get; set; }
        public string Diagnostico { get; set; }
        public string Comentario { get; set; }
        public string Medico { get; set; }
        public string Precio { get; set; }
        public string Expediente { get; set; }
        public string Fecha_Examen { get; set; }
        public string Fecha_Ingresado { get; set; }
        public string Registrado_Por { get; set; }
    }

    public class dataExamenB
    {
        public string Muestra { get; set; }
        public string Nombre { get; set; }
        public string Edad { get; set; }
        public string Diagnostico { get; set; }
        public string Medico { get; set; }
        public string Precio { get; set; }
        public string Expediente { get; set; }
        public string Fecha_Examen { get; set; }
        public string Fecha_Ingresado { get; set; }
        public string Registrado_Por { get; set; }
    }

    public class dataMaterial
    {
        public string Nombre { get; set; }
    }

    public class nombreCompletoPacientes
    {
        public string primerNombre { get; set; }
        public string segundoNombre { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
    }
}