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
using System.Windows.Media.Imaging;

namespace Sistema_BD_Clinica_Patologica
{
    public partial class MainPage : UserControl
    {
        public static Clases.TC tc = new Clases.TC();
        public static BitmapImage bmpClear = new BitmapImage();
        public List<string> permisos = new List<string>();

        public MainPage()
        {
            InitializeComponent();
            username.Text = "Usuario Desconocido";
            AppEvents.Instance.OnUpdateMain += new AppEvents.UpdateMainHandler(Instance_OnUpdateMain);
            
            home.IsEnabled = true;
            citologia.IsEnabled = false;
            biopsia.IsEnabled = false;
            medico.IsEnabled = false;
            contabilidad.IsEnabled = false;
            consultas.IsEnabled = false;
            controlUsuarios.IsEnabled = false;
            cerrarSesion.IsEnabled = false;
        }

        // After the Frame navigates, ensure the HyperlinkButton representing the current page is selected
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            foreach (UIElement child in LinksStackPanel.Children)
            {
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()))
                    {
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    }
                    else
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ChildWindow errorWin = new ErrorWindow(e.Uri);
            errorWin.Show();
        }

        void Instance_OnUpdateMain(object someObject)
        {
            username.Text = App.Username;
            permisos = App.Permisos;
            cerrarSesion.IsEnabled = true;

            //Hago esto para resetear los permisos que dejo el usuario anterior
            citologia.IsEnabled = false;
            biopsia.IsEnabled = false;
            medico.IsEnabled = false;
            contabilidad.IsEnabled = false;
            consultas.IsEnabled = false;
            controlUsuarios.IsEnabled = false;
            foreach (string item in permisos)
            {
                switch (item)
                {
                    case "Examenes":
                        citologia.IsEnabled = true;
                        biopsia.IsEnabled = true;
                        medico.IsEnabled = true;
                        break;
                    case "Ingresos":
                    case "Egresos":
                        contabilidad.IsEnabled = true;
                        break;
                    case "Consultas":
                        consultas.IsEnabled = true;
                        break;
                    case "Control De Usuarios":
                        controlUsuarios.IsEnabled = true;
                        break;
                }
            }
        }

        private void cerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            App.UserIsAuthenticated = false;
            App.Correo = "";
            App.Username = "Usuario Desconocido";
            while (App.Permisos.Count != 0)
                App.Permisos.RemoveAt(0);
            Instance_OnUpdateMain(sender);
        }
    }
}