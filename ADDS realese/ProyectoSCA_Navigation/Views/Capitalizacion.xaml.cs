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
using ProyectoSCA_Navigation.Clases;

namespace ProyectoSCA_Navigation.Views
{
    public partial class Capitalizacion : Page
    {
        string usuario = App.Correo;

        //Flags, true, puede disparar el evento, false no
        bool[] flags = new bool[10];
        /***    0 - Abrir Inserts
         ***    1 - Abrir Gets, luego Llenar campos de datos
         ***    2 - Error Windows
         ***/
        bool saldosFlag = true;

        /********************************************************************************************************************************
        **************************************************    Inicializadores   *********************************************************
        *********************************************************************************************************************************/

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private ProyectoSCA_Navigation.ServiceReference.WSCosecolSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:1809/WSCosecol.asmx";

        public Capitalizacion()
        {
            InitializeComponent();
            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReference.WSCosecolSoapClient(bind, endpoint);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getAportacionesACapitalizar, "");
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
                        case "p38"://Get Aportaciones a Capitalizar
                            if (flags[0])
                            {
                                flags[0] = false;
                                List<aportacionCapitalizarGrid> listaAportaciones = new List<aportacionCapitalizarGrid>();
                                listaAportaciones = MainPage.tc.getAportacionesACapitalizar(recievedResponce);
                                if (listaAportaciones == null)
                                {
                                    mensaje_txt.Text = "No hay aportaciones a capitalizar";
                                    MessageBox.Show("No hay aportaciones a capitalizar");
                                    return;
                                }
                                gridAportaciones.ItemsSource = listaAportaciones;
                            }
                            break;

                        case "p39"://Get Saldos de Afiliado
                            if (flags[0])
                            {
                                flags[0] = false;
                                List<saldoCapitalizarGrid> listaSaldos = new List<saldoCapitalizarGrid>();
                                listaSaldos = MainPage.tc.getSaldosACapitalizar(recievedResponce);
                                if (listaSaldos == null)
                                {
                                    mensaje_txt.Text = "No hay saldos porque no existen afiliados";
                                    MessageBox.Show("No hay saldos porque no existen afiliados");
                                    return;
                                }
                                gridSaldos.ItemsSource = listaSaldos;
                            }
                            break;

                        case "p40"://Capitalizacion Extraordinaria
                            if (flags[0])
                            {
                                flags[0] = false;
                                habilitarCampos(true);
                                mensaje_txt.Text = "Las aportaciones han sido capitalizadas exitosamente!";
                                MessageBox.Show("Las aportaciones han sido capitalizadas exitosamente!");
                                NavigationService.Refresh();
                            }
                            break;

                        default:
                            if (flags[2])
                            {
                                flags[2] = false;
                                MessageBox.Show("No se entiende la peticion. (No esta definida)");
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
                }
            }
            habilitarCampos(true);
        }

        /********************************************************************************************************************************
        **************************************************    Metodos de Botones    *****************************************************
        *********************************************************************************************************************************/

        private void capitalizar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dlgResult = MessageBox.Show("Esta seguro que desea capitalizar las aportaciones?", "Continue?", MessageBoxButton.OKCancel);
            if (dlgResult == MessageBoxResult.OK)
            {
                flags[0] = true;
                flags[2] = true;
                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.capitalizarExtraordinaria, usuario);
            }
            else if (dlgResult == MessageBoxResult.Cancel)
            {
                // No, stop
            }
        }

        private void ajustar_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/IngresarAportacion", UriKind.Relative));
        }

        /********************************************************************************************************************************
        **************************************************    Metodos Auxiliares    *****************************************************
        *********************************************************************************************************************************/
        public void habilitarCampos(bool valor)
        {
            tabControl1.IsEnabled = valor;
            capitalizar_btn.IsEnabled = valor;
            ajustar_btn.IsEnabled = valor;
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (tabControl1.SelectedIndex == 1)
                {
                    habilitarCampos(false);
                    flags[0] = true;
                    flags[1] = true;
                    flags[2] = true;
                    Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                    Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getSaldosACapitalizar, usuario);
                }
            }
            catch
            {
                
            }
        }
    }
}
