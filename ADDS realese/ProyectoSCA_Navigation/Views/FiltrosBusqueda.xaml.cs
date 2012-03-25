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
    public partial class FiltrosBusqueda : Page
    {
        string usuario = App.Correo;

        //Flags, true, puede disparar el evento, false no
        bool[] flags = new bool[10];
        /***    0 - Abrir Inserts
         ***    1 - Abrir Gets, luego Llenar campos de datos
         ***    2 - Error Windows
         ***/
        List<Object> listaGrandota = new List<Object>();
        List<string> nombresAfiliados = new List<string>();
        List<string> numeroCertificados = new List<string>();

        List<filtrosAfiliadoGrid> listaAfiliadosFiltrados = new List<filtrosAfiliadoGrid>();

        /********************************************************************************************************************************
        **************************************************    Inicializadores   *********************************************************
        *********************************************************************************************************************************/

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private ProyectoSCA_Navigation.ServiceReference.WSCosecolSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:1809/WSCosecol.asmx";

        public FiltrosBusqueda()
        {
            InitializeComponent();
            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReference.WSCosecolSoapClient(bind, endpoint);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            genero_txt.Items.Add("Masculino");
            genero_txt.Items.Add("Femenino");
            estadoCivil_txt.Items.Add("Soltero");
            estadoCivil_txt.Items.Add("Casado");
            estadoCivil_txt.Items.Add("Divorciado");
            estadoCivil_txt.Items.Add("Viudo");
            estadoCivil_txt.Items.Add("Union Libre");
            estadoAfiliado_txt.Items.Add("Activo");
            estadoAfiliado_txt.Items.Add("Inactivo");
            estadoAfiliado_txt.Items.Add("Retirado");
            habilitarCampos(false);
            verPerfil_btn.IsEnabled = false;
            estadoCuenta_btn.IsEnabled = false;
            generarReporte_btn.IsEnabled = false;

            flags[0] = true;
            flags[1] = true;
            flags[2] = true;

            if (App.Rol == "Afiliado")
            {
                NavigationService.Navigate(new Uri("/Consulta/ConsultarEstadoDeCuenta" + "?ID=0" + "$" + App.Correo, UriKind.Relative));
            }
            else
            {
                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getAfiliadosPagar, "");
            }
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
                    }
                }
                else
                {
                    switch (temp)
                    {

                        case "p10"://Get Ocupaciones, refrescamos el monto actual
                            if (flags[1])
                            {
                                flags[1] = false;
                                List<string[]> lista = new List<string[]>();

                                lista = MainPage.tc.getParentesco(e.Result.Substring(3));
                                if (lista != null)
                                    for (int i = 0; i < lista.Count; i++)
                                    {
                                        if ((lista[i])[1] == "True")
                                            profesion_txt.Items.Add((lista[i])[0]);
                                    }

                            }
                            break;

                        case "p21"://Get datos Afiliado
                            if (flags[3])
                            {
                                flags[3] = false;
                                if (recievedResponce != "")
                                {
                                    Afiliado afiliado = new Afiliado();
                                    afiliado = MainPage.tc.getAfiliado(recievedResponce);
                                }
                            }
                            break;

                        case "p28"://Get Afiliados (Para llenar el autocomplete y combobox del SEARCH)
                            if (flags[0])
                            {
                                flags[0] = false;

                                if (recievedResponce == "")//Quiere decir que no existen afiliados registrados
                                {
                                    MessageBox.Show("No existen afiliados registrados");
                                    return;
                                }
                                listaGrandota = MainPage.tc.getListasPagos(recievedResponce);
                                numeroCertificados = (List<string>)listaGrandota[0];
                                nombresAfiliados = (List<string>)listaGrandota[1];

                                numeroCertificado_txt.ItemsSource = numeroCertificados;
                                nombreAuto_txt.ItemsSource = nombresAfiliados;

                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getOcupacion, "");
                            }
                            break;

                        case "p31"://Buscar afiliados (El boton de buscar jaja)
                            if (flags[0])
                            {
                                flags[0] = false;
                                resetCampos();

                                if (recievedResponce == "")//Quiere decir que no se encontro nada con esos filtros
                                {
                                    gridResultado.ItemsSource = null;
                                    MessageBox.Show("No se encontraron resultados.");
                                    return;
                                }

                                listaAfiliadosFiltrados = MainPage.tc.getListaAfiliadosFiltrados(recievedResponce);
                                gridResultado.ItemsSource = listaAfiliadosFiltrados;
                            }
                            break;

                        default:
                            if (flags[2])
                            {
                                flags[2] = false;
                                MessageBox.Show("Ha ocurrido un error.  Intente su consulta de nuevo.\nSi el problema persiste, refresque la pagina e intente de nuevo.");
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

        private void buscar_btn_Click(object sender, RoutedEventArgs e)
        {
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            habilitarCampos(false);

            string numeroCertificado = "";
            String[] nombres = { "", "", "", "" };
            string numeroIdentidad = "";
            string telefono = "";
            string celular = "";
            string profesion = "";
            string genero = "";
            string correo = "";
            string estadoCivil = "";
            string estadoAfiliado = "";
            string empresa = "";

            if (numeroCertificado_txt.SelectedIndex != -1)
                numeroCertificado = numeroCertificado_txt.SelectedItem.ToString();
            if (nombreAuto_txt.Text != "")
                nombres = nombreAuto_txt.SelectedItem.ToString().Split(' ');
            numeroIdentidad = id_txt.Text;
            telefono = telefono_txt.Text;
            celular = celular_txt.Text;
            if (profesion_txt.SelectedIndex != -1)
                profesion = profesion_txt.SelectedItem.ToString();
            if (genero_txt.SelectedIndex != -1)
                genero = genero_txt.SelectedItem.ToString();
            correo = correo_txt.Text;
            if (estadoCivil_txt.SelectedIndex != -1)
                estadoCivil = estadoCivil_txt.SelectedItem.ToString();
            if (estadoAfiliado_txt.SelectedIndex != -1)
                estadoAfiliado = estadoAfiliado_txt.SelectedItem.ToString();
            empresa = empresa_txt.Text;

            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.buscarAfiliados, MainPage.tc.buscarAfiliadosConsultas(numeroCertificado, nombres[0], nombres[1],
                nombres[2], nombres[3], numeroIdentidad, telefono, celular, profesion, genero, correo, estadoCivil, estadoAfiliado, empresa));
        }

        private void verPerfil_btn_Click(object sender, RoutedEventArgs e)
        {
            if (gridResultado.SelectedIndex == -1)
            {
                MessageBox.Show("Escoga un afiliado!");
                return;
            }
            filtrosAfiliadoGrid seleccionado = (filtrosAfiliadoGrid)gridResultado.SelectedItem;
            NavigationService.Navigate(new Uri("/Consulta/ConsultarPerfil" + "?User=" + seleccionado.Correo, UriKind.Relative));
        }

        private void estadoCuenta_btn_Click(object sender, RoutedEventArgs e)
        {
            if (gridResultado.SelectedIndex == -1)
            {
                MessageBox.Show("Escoga un afiliado!");
                return;
            }
            filtrosAfiliadoGrid seleccionado = (filtrosAfiliadoGrid)gridResultado.SelectedItem;
            NavigationService.Navigate(new Uri("/Consulta/ConsultarEstadoDeCuenta" + "?ID=" + seleccionado.No_Certificado + "$" + seleccionado.Correo, UriKind.Relative));
        }

        private void generarReporte_btn_Click(object sender, RoutedEventArgs e)
        {
            string IDAfiliados = "";
            filtrosAfiliadoGrid temp = new filtrosAfiliadoGrid();

            for (int i = 0; i < listaAfiliadosFiltrados.Count; i++)
            {
                temp = listaAfiliadosFiltrados[i];
                IDAfiliados += temp.No_Certificado.ToString();
                if (i != listaAfiliadosFiltrados.Count - 1)
                    IDAfiliados += ",";
            }

            NavigationService.Navigate(new Uri("/Consulta/EstadoDeCuentaTodos" + "?IDS=" + IDAfiliados, UriKind.Relative));
        }

        /********************************************************************************************************************************
       **************************************************    Metodos Auxiliares    *****************************************************
       *********************************************************************************************************************************/

        public void habilitarCampos(bool valor)
        {
            numeroCertificado_txt.IsEnabled = valor;
            nombreAuto_txt.IsEnabled = valor;
            id_txt.IsEnabled = valor;
            telefono_txt.IsEnabled = valor;
            celular_txt.IsEnabled = valor;
            profesion_txt.IsEnabled = valor;
            genero_txt.IsEnabled = valor;
            correo_txt.IsEnabled = valor;
            estadoCivil_txt.IsEnabled = valor;
            estadoAfiliado_txt.IsEnabled = valor;
            empresa_txt.IsEnabled = valor;
            buscar_btn.IsEnabled = valor;
            reset_btn.IsEnabled = valor;
            if (gridResultado.ItemsSource != null)
            {
                verPerfil_btn.IsEnabled = valor;
                estadoCuenta_btn.IsEnabled = valor;
                generarReporte_btn.IsEnabled = valor;
            }
        }

        public void resetCampos()
        {
            numeroCertificado_txt.SelectedIndex = -1;
            nombreAuto_txt.Text = "";
            id_txt.Text = "";
            telefono_txt.Text = "";
            celular_txt.Text = "";
            profesion_txt.SelectedIndex = -1;
            genero_txt.SelectedIndex = -1;
            correo_txt.Text = "";
            estadoCivil_txt.SelectedIndex = -1;
            estadoAfiliado_txt.SelectedIndex = -1;
            empresa_txt.Text = "";
        }

        private void reset_btn_Click(object sender, RoutedEventArgs e)
        {
            resetCampos();
        }
    }
}
