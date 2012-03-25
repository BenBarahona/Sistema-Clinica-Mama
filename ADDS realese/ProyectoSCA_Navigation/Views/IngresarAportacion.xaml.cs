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
using ProyectoSCA_Navigation.Clases;

namespace ProyectoSCA_Navigation.Views
{
    public partial class IngresarAportacion : Page
    {
        /********************************************************************************************************************************
        **************************************************    Inicializadores   *********************************************************
        *********************************************************************************************************************************/
        string usuario = App.Correo;

        //Flags, true, puede disparar el evento, false no
        bool[] flags = new bool[10];
        /***    0 - Abrir Inserts
         ***    1 - Abrir Gets, luego Llenar campos de datos
         ***    2 - Error Windows
         ***/

        int limiteAportacionVoluntaria = 0;

        List<int> listaIDAportaciones = new List<int>();
        List<Object> listaGrandota = new List<Object>();
        List<string> nombresAfiliados = new List<string>();
        List<string> numeroCertificados = new List<string>();

        List<Object> listaAportaciones = new List<Object>();

        //Esta es la lista que guarda todas las aportaciones que debe el afiliado        
        List<aportacionObligatoriaEspecial> listaAportacionesAfiliado = new List<aportacionObligatoriaEspecial>();

        //Esta lista es la que mostramos en el otro data grid que muestra todas las aportaciones a pagar
        List<controlDePagoGrid> controlDePagoGrid1 = new List<controlDePagoGrid>();
        List<controlDePagoGrid> controlDePagoGrid2 = new List<controlDePagoGrid>();

        //Listas finales, para mandar a los metodos de pagar
        List<controlDePagoGrid> listaAportacionesFinal = new List<controlDePagoGrid>();


        //Expresiones Regulares para las validaciones de los campos
        Regex numeros = new Regex("^[-][0-9]*$|^[0-9]*$");
        Regex numeroIdentidad = new Regex("^[0-9]{4}-[0-9]{4}-[0-9]{5}$");

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private ProyectoSCA_Navigation.ServiceReference.WSCosecolSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:1809/WSCosecol.asmx";

        public IngresarAportacion()
        {
            InitializeComponent();
            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReference.WSCosecolSoapClient(bind, endpoint);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Pone la fecha en los datepickers
            DateTime fechaNow = DateTime.Now;

            string[] fecha = fechaNow.ToString().Split(' ');
            fechaAO_txt.Text = fecha[0];
            fechaAV_txt.Text = fecha[0];

            habilitarAportaciones(false);
            habilitarSearch(false);
            habilitarBotones(false);
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getAfiliadosPagar, "");
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
                        habilitarSearch(true);
                        habilitarAportaciones(true);
                        habilitarBotones(true);
                    }
                }
                else
                {
                    switch (temp)
                    {
                        case "p14"://Get Limite Actual de la aportacion
                            if (flags[1])
                            {
                                descripcionAV_txt.IsEnabled = true;
                                montoAV_txt.IsEnabled = true;
                                agregarAV_btn.IsEnabled = true;

                                flags[1] = false;
                                limiteAportacionVoluntaria = Convert.ToInt32(recievedResponce);
                            }
                            break;

                        case "p28"://Get Afiliados (Para llenar el autocomplete y combobox del SEARCH)
                            if (flags[0])
                            {
                                flags[0] = false;
                                habilitarSearch(true);

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
                            }
                            break;

                        case "p29"://buscar Afiliado PAGAR
                            if (flags[0])
                            {
                                flags[0] = false;
                                habilitarSearch(true);
                                habilitarAportaciones(true);
                                habilitarBotones(true);
                                aportacionObligatoriaEspecial tempInstance = new aportacionObligatoriaEspecial();

                                listaAportaciones = MainPage.tc.getAfiliadoAportaciones(recievedResponce);

                                if (listaAportaciones[6] == "NULL")//Si no hay aportaciones
                                {
                                    MessageBox.Show("El usuario no tiene aportaciones obligatorias pendientes");
                                    return;
                                }

                                //Los primeros 6 posiciones traen datos del afiliado
                                id_txt.Text = listaAportaciones[5].ToString();
                                nombreAuto_txt.Text = listaAportaciones[1].ToString() + " " + listaAportaciones[2].ToString() + " " +
                                                      listaAportaciones[3].ToString() + " " + listaAportaciones[4].ToString();
                                numeroCertificado_txt.SelectedItem = listaAportaciones[0].ToString();

                                //Del indice 6 en adelante son de tipo aportaciones obligatorias especiales

                                for (int i = 6; i < listaAportaciones.Count; i++)
                                {
                                    listaAportacionesAfiliado.Add((aportacionObligatoriaEspecial)listaAportaciones[i]);
                                    tempInstance = (aportacionObligatoriaEspecial)listaAportaciones[i];
                                    controlDePagoGrid1.Add(new controlDePagoGrid()
                                    {
                                        ID = tempInstance.IDAportacion,
                                        Fecha_A_Pagar = tempInstance.fechaPlazo,
                                        Monto = tempInstance.Monto,
                                        Motivo = tempInstance.Motivo,
                                        Descripcion = tempInstance.Descripcion,
                                        TipoAportacion = "Obligatoria"
                                    });
                                    if (tempInstance.Motivo == "")
                                    {
                                        listaIDAportaciones.Add(tempInstance.IDAportacion);
                                    }
                                }


                                //Grid culero de las aportaciones obligatorias
                                gridMesesPorPagar.ItemsSource = controlDePagoGrid1;

                            }
                            break;

                        case "p30":
                            if (flags[0])
                            {
                                flags[0] = false;
                                habilitarSearch(true);
                                habilitarAportaciones(true);
                                habilitarBotones(true);

                                while (controlDePagoGrid2.Count != 0)
                                    controlDePagoGrid2.RemoveAt(0);

                                MessageBox.Show("Se han registrado las aportaciones exitosamente!");
                                NavigationService.Refresh();
                            }
                            break;
                        default:
                            if (flags[2])
                            {
                                flags[2] = false;
                                MessageBox.Show("Ha ocurrido un error.  Intente su consulta de nuevo.\nSi el problema persiste, refresque la pagina e intente de nuevo.");
                                habilitarSearch(true);
                                habilitarAportaciones(true);
                                habilitarBotones(true);
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
                    habilitarSearch(true);
                    habilitarAportaciones(true);
                    habilitarBotones(true);
                }
                //habilitarCampos(true);
            }
        }


        /********************************************************************************************************************************
        **************************************************    Metodos de Botones    *****************************************************
        *********************************************************************************************************************************/


        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (numeroCertificado_txt.SelectedIndex == -1 && nombreAuto_txt.Text == "" && id_txt.Text == "")
            {
                MessageBox.Show("No ha especificado algun criterio de busqueda");
                return;
            }

            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            habilitarSearch(false);

            controlDePagoGrid1.Clear();
            controlDePagoGrid2.Clear();
            gridMesesPorPagar.ItemsSource = null;
            gridAportaciones.ItemsSource = null;

            string numeroCertificado = "";
            String[] nombres = { "", "", "", "" };
            string numeroIdentidad = "";
            if (numeroCertificado_txt.SelectedIndex != -1)
                numeroCertificado = numeroCertificado_txt.SelectedItem.ToString();
            else if (nombreAuto_txt.Text != "")
                nombres = nombreAuto_txt.SelectedItem.ToString().Split(' ');
            else if (nombreAuto_txt.Text == "" && numeroCertificado_txt.SelectedIndex == -1)
                numeroIdentidad = id_txt.Text;

            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getControlPagos, MainPage.tc.getAfiliadoAportacion(numeroCertificado, nombres[0], nombres[1], nombres[2], nombres[3], numeroIdentidad));
        }

        private void agregar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (gridMesesPorPagar.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una aportacion por pagar!");
                return;
            }

            controlDePagoGrid verificador = (controlDePagoGrid)gridMesesPorPagar.SelectedItem;

            if ((listaIDAportaciones.Min() < verificador.ID) && verificador.Motivo == "")
            {
                MessageBox.Show("No puede pagar un mes sin pagar el mes anterior");
                return;
            }
            
            int temp = gridMesesPorPagar.SelectedIndex;
            controlDePagoGrid1.RemoveAt(temp);
            listaIDAportaciones.Remove(verificador.ID);

            controlDePagoGrid2.Add((controlDePagoGrid)gridMesesPorPagar.SelectedItem);

            gridMesesPorPagar.ItemsSource = null;
            gridMesesPorPagar.ItemsSource = controlDePagoGrid1;

            gridAportaciones.ItemsSource = null;
            gridAportaciones.ItemsSource = controlDePagoGrid2;
        }

        private void agregarAV_btn_Click(object sender, RoutedEventArgs e)
        {
            if (montoAV_txt.Text == "")
            {
                MessageBox.Show("Ingrese un monto para la aportacion");
                return;
            }

            int monto = Convert.ToInt32(montoAV_txt.Text);
            if (monto > limiteAportacionVoluntaria)
            {
                MessageBox.Show("El monto de la aportacion sobrepasa el limite establecido para una aportacion voluntaria!\nLimite Actual: " + limiteAportacionVoluntaria.ToString());
                return;
            }

            controlDePagoGrid2.Add(new controlDePagoGrid
                {
                    Descripcion = descripcionAV_txt.Text,
                    Monto = Convert.ToInt64(montoAV_txt.Text),
                    TipoAportacion = "Voluntaria"
                });

            gridAportaciones.ItemsSource = null;
            gridAportaciones.ItemsSource = controlDePagoGrid2;
        }

        private void limpiar_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dlgResult = MessageBox.Show("Esta seguro que desea limpiar la lista?", "Continue?", MessageBoxButton.OKCancel);
            if (dlgResult == MessageBoxResult.OK)
            {
                foreach (controlDePagoGrid item in controlDePagoGrid2)
                {
                    if (item.TipoAportacion == "Obligatoria")
                    {
                        listaIDAportaciones.Add(item.ID);
                        controlDePagoGrid1.Add(item);
                    }
                }

                while (controlDePagoGrid2.Count != 0)
                    controlDePagoGrid2.RemoveAt(0);

                gridAportaciones.ItemsSource = null;
                gridMesesPorPagar.ItemsSource = null;
                gridMesesPorPagar.ItemsSource = controlDePagoGrid1;
            }
            else if (dlgResult == MessageBoxResult.Cancel)
            {
                // No, stop
            }
        }

        private void eliminar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (gridAportaciones.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un elemento de la lista a borrar");
                return;
            }

            controlDePagoGrid aportacionSeleccionada = (controlDePagoGrid)gridAportaciones.SelectedItem;

            if (aportacionSeleccionada.TipoAportacion == "Obligatoria")
            {
                if (aportacionSeleccionada.Motivo == "")
                {
                    listaIDAportaciones.Add(aportacionSeleccionada.ID);
                }
                controlDePagoGrid1.Add(aportacionSeleccionada);
                gridMesesPorPagar.ItemsSource = null;
                gridMesesPorPagar.ItemsSource = controlDePagoGrid1;
            }

            controlDePagoGrid2.RemoveAt(gridAportaciones.SelectedIndex);

            gridAportaciones.ItemsSource = null;
            gridAportaciones.ItemsSource = controlDePagoGrid2;
        }

        private void registrar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (gridAportaciones.ItemsSource == null)
            {
                MessageBox.Show("No ha seleccionado alguna aportacion para registrar");
                return;
            }

            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            habilitarSearch(false);
            habilitarAportaciones(false);
            habilitarBotones(false);

            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.pagarAportaciones, MainPage.tc.Pagar(listaAportaciones[5].ToString(), controlDePagoGrid2, usuario));
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (tabControl1.SelectedIndex == 1)
                {
                    descripcionAV_txt.IsEnabled = false;
                    montoAV_txt.IsEnabled = false;
                    agregarAV_btn.IsEnabled = false;
                    flags[0] = true;
                    flags[1] = true;
                    flags[2] = true;
                    Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                    Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getLimiteActual, "");
                }
            }
            catch { }
        }

        /********************************************************************************************************************************
        **************************************************    Validaciones!!   **********************************************************
        *********************************************************************************************************************************/

        private void id_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            numeroCertificado_txt.SelectedIndex = -1;
            nombreAuto_txt.Text = "";
        }

        private void nombreAuto_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            numeroCertificado_txt.SelectedIndex = -1;
            id_txt.Text = "";
        }

        private void numeroCertificado_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            nombreAuto_txt.Text = "";
            id_txt.Text = "";
        }

        private void montoAV_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            montoAV_txt.Text.Trim();
            if (numeros.IsMatch(montoAV_txt.Text))
            {
                agregarAV_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                monto_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                agregarAV_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                monto_valid.Source = new BitmapImage(imgURI);
            }
            if (montoAV_txt.Text == "")
            {
                monto_valid.Source = MainPage.bmpClear;
            }
        }

        /********************************************************************************************************************************
        **************************************************    Metodos Auxiliares    *****************************************************
        *********************************************************************************************************************************/

        public void habilitarSearch(bool valor)
        {
            try
            {
                numeroCertificado_txt.IsEnabled = valor;
                nombreAuto_txt.IsEnabled = valor;
                id_txt.IsEnabled = valor;
                buscar_btn.IsEnabled = valor;
            }
            catch { }
        }

        public void habilitarAportaciones(bool valor)
        {
            tabControl1.IsEnabled = valor;
            gridAportaciones.IsEnabled = valor;
        }

        public void habilitarBotones(bool valor)
        {
            registrar_btn.IsEnabled = valor;
            eliminar_btn.IsEnabled = valor;
            limpiar_btn.IsEnabled = valor;
        }

        private void id_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            id_txt.Text.Trim();
            if (numeroIdentidad.IsMatch(id_txt.Text))
            {
                mensaje_txt.Text = "";
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                mensaje_txt.Text = "Formato valido de Identidad: 0000-0000-00000";
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_valid.Source = new BitmapImage(imgURI);
            }
            if (id_txt.Text == "")
            {
                mensaje_txt.Text = "";
                id_valid.Source = MainPage.bmpClear;
            }
        }
    }
}
