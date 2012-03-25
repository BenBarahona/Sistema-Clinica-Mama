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

namespace ProyectoSCA_Navigation
{
    public partial class Home : Page
    {
        Login login = new Login();
        bool[] flags = new bool[3];
        List<string> loginDatos = new List<string>();
        List<string> Permisos = new List<string>();
        String username = "";

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private ProyectoSCA_Navigation.ServiceReference.WSCosecolSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:1809/WSCosecol.asmx";

        public Home()
        {
            InitializeComponent();

            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReference.WSCosecolSoapClient(bind, endpoint);

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
                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.login, MainPage.tc.verificarLogin(login.Usuario, login.Password));
            }
        }

        private void Wrapper_AgregarPeticionCompleted(object sender, ServiceReference.AgregarPeticionCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string temp = e.Result.Substring(0, 3);
                string recievedResponce = e.Result.Substring(3);
                if (recievedResponce == "False" || recievedResponce == "")
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
                    switch (temp)
                    {
                        case "p23"://Login
                            if (flags[0])
                            {
                                flags[0] = false;
                                loginDatos = MainPage.tc.getLoginDatos(e.Result.Substring(3));
                                App.Username = loginDatos[0] + " " + loginDatos[1] + " " + loginDatos[2] + " " + loginDatos[3];
                                App.Correo = login.Usuario;
                                for (int i = 4; i < loginDatos.Count - 1; i++)
                                    Permisos.Add(loginDatos[i]);
                                App.Permisos = Permisos;
                                App.UserIsAuthenticated = true;
                                App.Rol = loginDatos[loginDatos.Count - 1];
                                AppEvents.Instance.UpdateMain(sender);
                                MessageBox.Show("Login Exitoso!");
                                NavigationService.Refresh();
                            }
                            break;

                        default:
                            if (flags[2])
                            {
                                flags[2] = false;
                                MessageBox.Show("Usuario o Password Incorrecta.  Intente de nuevo");
                                App.UserIsAuthenticated = false;
                                
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
}