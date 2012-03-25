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
using System.Text;
using System.Collections;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;

namespace Sistema_BD_Clinica_Patologica.Views
{
    public partial class Biopsia : Page
    {

        int edadPaciente = 0;
        int idPaciente = 0;
        int idMedico = 0;
        string[] fechaBiopsia;
        string material;
        String[] nombre = new String[2];
        String[] apellido = new String[2];
        String segundoNombre = "";
        String segundoApellido = "";

        DateTime fechaInforme = DateTime.Now;
        String[] fechaInformeArray = new String[4];
        String fechaInformeIngresado = "";
        String fechaInformeString = "";

        List<String> materialEnviado = new List<String>();

        //Control Flags
        bool mainFlag = true;
        bool mainFlag2 = true;
        bool getIDPacienteFlag = true;
        bool getIDMedicoFlag = true;
        bool insertarExamenFlag = true;
        bool errorBiopsia = true;
        bool errorMaterial = true;
        bool insertarMaterialFlag = true;
        bool insertarBiopsia = true;
        bool insertarCitologiaLiquidos = false;
        bool insertarMaterial = true;
        bool buscarB = true;
        bool borrarB = true;
        bool actualizarB = true;

        String anioActual = "";
        String Usuario = App.Correo;

        //Expresiones Regulares para las validaciones de los campos
        Regex numeros = new Regex("^[0-9]+$");

        string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
        Uri imgURI = null;
        /********************************************************************************************************************************
        **************************************************    Inicializadores   *********************************************************
        *********************************************************************************************************************************/

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private Sistema_BD_Clinica_Patologica.ServiceReferenceClinica.WSClinicaSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:5633/WSClinica.asmx";


        public Biopsia()
        {
            InitializeComponent();
            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReferenceClinica.WSClinicaSoapClient(bind, endpoint);

            imgURI = new Uri(sURL, UriKind.Relative);

            //FORMATO DE FECHA QUE DEVUELTE:
            //Ej. Friday, December 17, 2010
            fechaInformeArray = fechaInforme.ToLongDateString().Split();
            fechaInformeIngresado = getFechaEnFormatoMysql(fechaInforme.Date.ToShortDateString());

            //Al traducirla, sale el arreglo asi: Viernes Diciembre 17 2010
            traducirFecha(fechaInformeArray);
            //Dia actual en string junto y asignacion del año para el ID de las muestras
            fechaInformeString = fechaInformeArray[0] + " " + fechaInformeArray[1] + " " + fechaInformeArray[2] + ", " + fechaInformeArray[3];
            anioActual = "-" + fechaInformeArray[3].Substring(2);

            enableMainButtons(false);
            enableMatButtons(false);

            mainFlag = true;
            Wrapper.getMedicos_NombresCompleted += new EventHandler<ServiceReferenceClinica.getMedicos_NombresCompletedEventArgs>(getMedicosNombres);
            Wrapper.getMedicos_NombresAsync();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }


        /********************************************************************************************************************************
        ***********************************************    MAIN BUTTON EVENTS   *********************************************************
        *********************************************************************************************************************************/

        private void registrarBiopsia_btn_Click(object sender, RoutedEventArgs e)
        {
            //OTHER FLAGS
            insertarMaterialFlag = false;
            mainFlag = false;
            getIDPacienteFlag = false;
            getIDMedicoFlag = false;
            insertarExamenFlag = false;
            errorBiopsia = false;
            segundoApellido = "";
            segundoNombre = "";

            if (idMuestra_txt6.Text.Length == 0)
            {
                estadoBiopsia.Text = "Escriba un ID de Muestra";
                id_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (nombrePacienteB.Text.Length == 0)
            {
                estadoBiopsia.Text = "Escriba el Primer Nombre del paciente";
                nombre_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (apellidoPacienteB.Text.Length == 0)
            {
                estadoBiopsia.Text = "Escriba el Primer Apellido del paciente";
                nombre_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (nombreMedico2.SelectedItem == null)
            {
                estadoBiopsia.Text = "Seleccione un Médico";
                medico_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (fechaBiopsia_txt.Text.Length == 0)
            {
                estadoBiopsia.Text = "Escriba la fecha de la muestra";
                fecha_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (valorBiopsia_txt.Text == "")
            {
                estadoBiopsia.Text = "Escriba el valor de la Biopsia";
                valor_error.Source = new BitmapImage(imgURI);
                return;
            }

            estadoBiopsia.Text = "Guardando Datos...";
            enableMainButtons(false);
            enableMatButtons(false);
            //Inserta el nuevo paciente

            nombre = (nombrePacienteB.Text + " ").Split(' ');
            apellido = (apellidoPacienteB.Text + " ").Split(' ');

            for (int i = 1; i < nombre.Length; i++)
            {
                if (nombre[i].CompareTo("") != 0)
                {
                    segundoNombre += nombre[i] + " ";
                }
            }

            for (int i = 1; i < apellido.Length; i++)
            {
                if (apellido[i].CompareTo("") != 0)
                {
                    segundoApellido += apellido[i] + " ";
                }
            }
            segundoNombre = segundoNombre.Trim();
            segundoApellido = segundoApellido.Trim();

            if (edadBiopsia_txt.Text == "")
                edadBiopsia_txt.Text = "0";

            Wrapper.InsertarPacienteCompleted += new EventHandler<ServiceReferenceClinica.InsertarPacienteCompletedEventArgs>(InsertarPacienteCompleted);
            Wrapper.InsertarPacienteAsync(false, "", "", nombre[0], segundoNombre, apellido[0], segundoApellido, Convert.ToInt32(edadBiopsia_txt.Text), false, expedienteBiopsia_txt.Text);
        }

        private void buscarBiopsia_btn_Click(object sender, RoutedEventArgs e)
        {
            if (idMuestra_txt6.Text.Length == 0)
            {
                estadoBiopsia.Text = "Escriba el No. de Muestra a buscar!";
                id_error.Source = new BitmapImage(imgURI);
                return;
            }

            buscarB = true;

            estadoBiopsia.Text = "Buscando Datos...";
            enableMainButtons(false);
            enableMatButtons(false);
            String idmuestra = idMuestra_txt6.Text;
            string[] test = idMuestra_txt6.Text.Split('-');

            resetCampos();
            try
            {
                if (test[1] != "")
                {
                    if (insertarBiopsia)
                    {
                        Wrapper.buscarBiopsiaCompleted += new EventHandler<ServiceReferenceClinica.buscarBiopsiaCompletedEventArgs>(buscarBiopsia);
                        Wrapper.buscarBiopsiaAsync(corregirIdMuestra(idmuestra));
                    }
                    else if (insertarCitologiaLiquidos)
                    {
                        Wrapper.buscarCitologiaLiquidosCompleted += new EventHandler<ServiceReferenceClinica.buscarCitologiaLiquidosCompletedEventArgs>(buscarCitologiaLiquidos);
                        Wrapper.buscarCitologiaLiquidosAsync(corregirIdMuestra(idmuestra));
                    }
                }
                else
                {
                    estadoBiopsia.Text = "No existe una biopsia con ese Numero!";
                    enableMainButtons(true);
                    enableMatButtons(true);
                }
            }
            catch (Exception exp)
            {
                if (insertarBiopsia)
                {
                    Wrapper.buscarBiopsiaCompleted += new EventHandler<ServiceReferenceClinica.buscarBiopsiaCompletedEventArgs>(buscarBiopsia);
                    Wrapper.buscarBiopsiaAsync(corregirIdMuestra(idmuestra));
                }
                else if (insertarCitologiaLiquidos)
                {
                    Wrapper.buscarCitologiaLiquidosCompleted += new EventHandler<ServiceReferenceClinica.buscarCitologiaLiquidosCompletedEventArgs>(buscarCitologiaLiquidos);
                    Wrapper.buscarCitologiaLiquidosAsync(corregirIdMuestra(idmuestra));
                }
            }
        }

        private void borrarBiopsia_Click(object sender, RoutedEventArgs e)
        {
            if (idMuestra_txt6.Text.Length == 0)
            {
                estadoBiopsia.Text = "Escriba un ID de Muestra";
                id_error.Source = new BitmapImage(imgURI);
                return;
            }

            MessageBoxResult msgResult = MessageBox.Show("Esta a punto de borrar una Biopsia, \nEsta seguro que quiere continuar?", "Borrando Biopsia", System.Windows.MessageBoxButton.OKCancel);
            if (msgResult == MessageBoxResult.Cancel)
                return;

            borrarB = true;
            enableMainButtons(false);
            estadoBiopsia.Text = "Borrando Datos...";
            String idmuestra = idMuestra_txt6.Text;
            string[] test = idMuestra_txt6.Text.Split('-');

            resetCampos();
            try
            {
                if (test[1] != "")
                {
                    Wrapper.BorrarMuestraCompleted += new EventHandler<ServiceReferenceClinica.BorrarMuestraCompletedEventArgs>(BorrarMuestra);
                    Wrapper.BorrarMuestraAsync(corregirIdMuestra(idmuestra));

                }
                else
                {
                    estadoBiopsia.Text = "No existe una biopsia con ese Numero";
                    enableMainButtons(true);
                }
            }
            catch (Exception exp)
            {
                Wrapper.BorrarMuestraCompleted += new EventHandler<ServiceReferenceClinica.BorrarMuestraCompletedEventArgs>(BorrarMuestra);
                Wrapper.BorrarMuestraAsync(corregirIdMuestra(idmuestra));
            }
        }

        private void actualizarBiopsia_Click(object sender, RoutedEventArgs e)
        {
            if (idMuestra_txt6.Text.Length == 0)
            {
                estadoBiopsia.Text = "Escriba un ID de Muestra";
                id_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (nombrePacienteB.Text.Length == 0)
            {
                estadoBiopsia.Text = "Escriba el Primer Nombre del paciente";
                nombre_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (apellidoPacienteB.Text.Length == 0)
            {
                estadoBiopsia.Text = "Escriba el Primer Apellido del paciente";
                nombre_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (fechaBiopsia_txt.Text.Length == 0)
            {
                estadoBiopsia.Text = "Escriba la fecha de la muestra";
                fecha_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (valorBiopsia_txt.Text == "")
            {
                estadoBiopsia.Text = "Escriba el valor de la Biopsia";
                valor_error.Source = new BitmapImage(imgURI);
                return;
            }


            nombre = (nombrePacienteB.Text + " ").Split(' ');
            apellido = (apellidoPacienteB.Text + " ").Split(' ');

            if (edadBiopsia_txt.Text == "")
                edadBiopsia_txt.Text = "0";

            actualizarB = true;
            estadoBiopsia.Text = "Guardando Datos...";
            enableMainButtons(false);
            enableMatButtons(false);

            string fechaFormated = fechaBiopsia[2] + "-" + fechaBiopsia[0] + "-" + fechaBiopsia[1];
            String materialEnviado = "";

            for (int i = 0; i < materialEnviadoSelect.Items.Count; i++)
            {
                materialEnviado += materialEnviadoSelect.Items.ElementAt(i).ToString() + ";";
            }

            if (insertarBiopsia)
            {
                Wrapper.ActualizarBiopsiaCompleted += new EventHandler<ServiceReferenceClinica.ActualizarBiopsiaCompletedEventArgs>(ActualizarBiopsia);
                Wrapper.ActualizarBiopsiaAsync(corregirIdMuestra(idMuestra_txt6.Text), macroscopica_txtR.Xaml, micro_txtR.Xaml, codificacion_txt.Text, diagnosticoBiopsia_txtR.Xaml,
                        fechaFormated, diagnosticoClinicoBiopsia_txt.Text, false, "", "", nombre[0], nombre[1], apellido[0], apellido[1], Convert.ToInt32(edadBiopsia_txt.Text), expedienteBiopsia_txt.Text, materialEnviado, Convert.ToInt32(valorBiopsia_txt.Text), fechaRecibida_txt.Text);
            }
            else if (insertarCitologiaLiquidos)
            {
                Wrapper.ActualizarCitologiaLiquidosCompleted += new EventHandler<ServiceReferenceClinica.ActualizarCitologiaLiquidosCompletedEventArgs>(ActualizarCitologiaLiquidos);
                Wrapper.ActualizarCitologiaLiquidosAsync(corregirIdMuestra(idMuestra_txt6.Text), macroscopica_txtR.Xaml, micro_txtR.Xaml, diagnosticoBiopsia_txtR.Xaml,
                        fechaFormated, diagnosticoClinicoBiopsia_txt.Text, false, "", "", nombre[0], nombre[1], apellido[0], apellido[1], Convert.ToInt32(edadBiopsia_txt.Text), expedienteBiopsia_txt.Text, Convert.ToInt32(valorBiopsia_txt.Text), materialEnviado, fechaRecibida_txt.Text);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                material = materialEnviadoTodos.SelectedItem.ToString();
                materialEnviadoSelect.Items.Add(material);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Seleccione un elemento de la lista");
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            int indice = materialEnviadoSelect.SelectedIndex;
            try
            {
                materialEnviadoSelect.Items.RemoveAt(indice);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Seleccione un material de la lista!");
            }
        }

        private void agregarMaterialEnviado_Click(object sender, RoutedEventArgs e)
        {

            if (materialEnviado_txt.Text == "")
            {
                matEnviadoStatus.Text = "Escriba el nombre del material";
                material_error.Source = new BitmapImage(imgURI);
                return;
            }

            matEnviadoStatus.Text = "Guardando...";
            insertarMaterial = true;
            mainFlag2 = true;
            enableMainButtons(false);
            enableMatButtons(false);

            //Inserta el nuevo material
            Wrapper.InsertarMaterialCompleted += new EventHandler<ServiceReferenceClinica.InsertarMaterialCompletedEventArgs>(InsertarMaterial);
            Wrapper.InsertarMaterialAsync(materialEnviado_txt.Text);
        }

        private void borrarMaterial_btn_Click(object sender, RoutedEventArgs e)
        {
            insertarMaterial = true;

            if (materialEnviado_txt.Text == "")
            {
                matEnviadoStatus.Text = "Escriba el nombre del material";
                material_error.Source = new BitmapImage(imgURI);
                return;
            }

            MessageBoxResult msgResult = MessageBox.Show("Esta a punto de borrar un Material, \nEsta seguro que quiere continuar?", "Borrando Materiales", System.Windows.MessageBoxButton.OKCancel);
            if (msgResult == MessageBoxResult.Cancel)
                return;

            matEnviadoStatus.Text = "Borrando Material";
            enableMainButtons(false);
            enableMatButtons(false);
            //Inserta el nuevo material
            Wrapper.BorrarMaterialCompleted += new EventHandler<ServiceReferenceClinica.BorrarMaterialCompletedEventArgs>(borarrMaterial);
            Wrapper.BorrarMaterialAsync(materialEnviado_txt.Text);
        }

        private void imprimir_btn_Click(object sender, RoutedEventArgs e)
        {
            if (idMuestra_txt6.Text.CompareTo("") != 0)
                NavigationService.Navigate(new Uri("/Formulario" + "?ID=" + corregirIdMuestra(idMuestra_txt6.Text), UriKind.Relative));
            else
                NavigationService.Navigate(new Uri("/Formulario?ID=", UriKind.Relative));
        }

        /********************************************************************************************************************************
        *********************************************    FIN   BUTTON EVENTS    *********************************************************
        *********************************************************************************************************************************/

        /********************************************************************************************************************************
        *********************************************    RADIO BUTTONS EVENTS   *********************************************************
        *********************************************************************************************************************************/
        private void biopsia_radioBtn_Checked(object sender, RoutedEventArgs e)
        {
            insertarBiopsia = true;
            insertarCitologiaLiquidos = false;

        }

        private void biopsia_radioBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            insertarBiopsia = false;
        }

        private void citologiaLiquido_radioBtn_Checked(object sender, RoutedEventArgs e)
        {
            insertarCitologiaLiquidos = true;
            insertarBiopsia = false;
        }

        private void citologiaLiquido_radioBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            insertarCitologiaLiquidos = false;
        }

        /********************************************************************
        **********************    OTHER EVENTS   ****************************
        *********************************************************************/
        private void fechaBiopsia_txt_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            fecha_error.Source = MainPage.bmpClear;
            fechaBiopsia = fechaBiopsia_txt.Text.Split('/');
        }

        /********************************************************************************************************************************
        **************************************************    FIN       *****************************************************************
        *********************************************************************************************************************************/


        /********************************************************************************************************************************
        *********************************************************************************************************************************
        *********************************************                           *********************************************************
        *********************************************   METHOD RESPONCE EVENTS  *********************************************************
        *********************************************                           *********************************************************
        *********************************************************************************************************************************
        *********************************************************************************************************************************/

        void buscarBiopsia(object sender, ServiceReferenceClinica.buscarBiopsiaCompletedEventArgs e)
        {
            if (buscarB)
            {
                buscarB = false;
                enableMainButtons(true);
                enableMatButtons(true);

                if (e.Result.ToString() == "")
                {
                    estadoBiopsia.Text = "No se encontró ninguna muestra con el ID " + corregirIdMuestra(idMuestra_txt6.Text) + " \nTalvez buscaba una Citologia de Liquidos?";
                }
                else
                {
                    resetCampos();
                    estadoBiopsia.Text = "Datos Obtenidos!";
                    String[] resultado;
                    resultado = e.Result.ToString().Split('$');

                    //Empieza a llenar los campos
                    idMuestra_txt6.Text = resultado[0];
                    fechaBiopsia_txt.Text = resultado[1];
                    nombrePacienteB.Text = resultado[2] + " " + resultado[3];
                    apellidoPacienteB.Text = resultado[4] + " " + resultado[5];
                    edadBiopsia_txt.Text = resultado[6];
                    nombreMedico2.SelectedItem = resultado[7];
                    valorBiopsia_txt.Text = resultado[8];
                    expedienteBiopsia_txt.Text = resultado[9];
                    diagnosticoClinicoBiopsia_txt.Text = resultado[10];
                    codificacion_txt.Text = resultado[11];

                    if (resultado[12].StartsWith("<"))
                        macroscopica_txtR.Xaml = resultado[12];
                    else
                        macroscopica_txtR.Xaml = crearXamlString(resultado[12]);
                        
                    if (resultado[13].StartsWith("<"))
                        micro_txtR.Xaml = resultado[13];
                    else
                        micro_txtR.Xaml = crearXamlString(resultado[13]);

                    if (resultado[14].StartsWith("<"))
                        diagnosticoBiopsia_txtR.Xaml = resultado[14];
                    else
                        diagnosticoBiopsia_txtR.Xaml = crearXamlString(resultado[14]);

                    fechaRecibida_txt.Text = resultado[15];

                    //Obtiene los datos para las muestras enviadas de la biopsia
                    matEnviadoStatus.Text = "Obteniendo Material Enviado...";
                    Wrapper.getMaterialEnviadoBiopsiaImprimirCompleted += new EventHandler<ServiceReferenceClinica.getMaterialEnviadoBiopsiaImprimirCompletedEventArgs>(getMaterialEnviadoBiopsiaImprimir);
                    Wrapper.getMaterialEnviadoBiopsiaImprimirAsync(idMuestra_txt6.Text, "MaterialEnviadoBiopsia");
                }
            }
        }

        private void ActualizarBiopsia(object sender, ServiceReferenceClinica.ActualizarBiopsiaCompletedEventArgs e)
        {
            if (actualizarB)
            {
                actualizarB = false;
                bool Response = e.Result;

                enableMainButtons(true);
                enableMatButtons(true);

                if (Response == false)
                {
                    estadoBiopsia.Text = "Error al actualizar Biopsia";
                }
                else
                {
                    estadoBiopsia.Text = "Biopsia actualizada con exito!";
                }
            }
        }

        private void ActualizarCitologiaLiquidos(object sender, ServiceReferenceClinica.ActualizarCitologiaLiquidosCompletedEventArgs e)
        {
            if (actualizarB)
            {
                actualizarB = false;
                bool Response = e.Result;

                enableMainButtons(true);
                enableMatButtons(true);

                if (Response == false)
                {
                    estadoBiopsia.Text = "Error al actualizar Citologia de Liquidos";
                }
                else
                {
                    estadoBiopsia.Text = "Citologia de Liquidos actualizada con exito!";
                }
            }
        }

        void getMedicosNombres(object sender, ServiceReferenceClinica.getMedicos_NombresCompletedEventArgs e)
        {
            if (mainFlag)
            {
                mainFlag = false;
                for (int i = nombreMedico2.Items.Count - 1; i > -1; i--)
                    nombreMedico2.Items.RemoveAt(i);

                if (e.Result.Length != 0)
                {
                    String[] resp = e.Result.ToString().Split(';');
                    for (int i = 0; i < resp.Length; i++)
                    {
                        if (!resp.Equals(""))
                            nombreMedico2.Items.Add(resp[i]);
                    }
                }
                Wrapper.getMaterialEnviado_CitologiaNoGinecologicaCompleted += new EventHandler<ServiceReferenceClinica.getMaterialEnviado_CitologiaNoGinecologicaCompletedEventArgs>(getMaterialEnviado_CitologiaNoGinecologica);
                Wrapper.getMaterialEnviado_CitologiaNoGinecologicaAsync();

                enableMainButtons(true);

                estadoBiopsia.Text = "Datos Obtenidos!";
            }
        }

        void getMaterialEnviado_CitologiaNoGinecologica(object sender, ServiceReferenceClinica.getMaterialEnviado_CitologiaNoGinecologicaCompletedEventArgs e)
        {
            if (mainFlag2)
            {
                mainFlag2 = false;
                enableMatButtons(true);
                matEnviadoStatus.Text = "Materiales Obtenidos!";
                if (e.Result.Length != 0)
                {
                    string[] content = e.Result.Split(';');
                    List<String> valores = new List<String>();
                    //List<dataMaterial> valores = new List<dataMaterial>();
                    for (int i = 0; i < content.Length - 1; i++)
                    {
                        if (!content[i].Equals(""))
                        {
                            valores.Add(content[i]);
                        }
                    }

                    materialEnviadoTodos.ItemsSource = valores;
                }
            }
        }

        void getIDdePaciente(object sender, ServiceReferenceClinica.getIDdePacienteCompletedEventArgs e)
        {
            int Response;
            Response = e.Result;
            estadoBiopsia.Text = "Obteniendo datos para insertar muestra...";
            if (Response == -1)
            {
                enableMainButtons(true);
                enableMatButtons(true);
                estadoBiopsia.Text = "Error al obtener el ID del paciente";
                return;
            }
            else
            {
                idPaciente = e.Result;
                if (!getIDMedicoFlag)
                {
                    getIDMedicoFlag = true;
                    Wrapper.getIDdeMedicoCompleted += new EventHandler<ServiceReferenceClinica.getIDdeMedicoCompletedEventArgs>(getIDdeMedico);
                    Wrapper.getIDdeMedicoAsync(nombreMedico2.SelectedItem.ToString());
                }
            }

        }//End void WebS_getAccesoCompleted(object sender, MyWebReference.HelloWorldCompletedEventArgs e)


        void getIDdeMedico(object sender, ServiceReferenceClinica.getIDdeMedicoCompletedEventArgs e)
        {
            int Response;
            Response = e.Result;

            if (Response == -1)
            {
                enableMainButtons(true);
                enableMatButtons(true);
                estadoBiopsia.Text = "Error al leer ID del Medico, intente denuevo";
                if (!errorBiopsia)
                {
                    errorBiopsia = true;
                    MessageBox.Show("Error al leer el ID del medico");
                }
            }
            else
            {
                idMedico = e.Result;
                string fechaFormated = fechaBiopsia[2] + "-" + fechaBiopsia[0] + "-" + fechaBiopsia[1];
                if (insertarBiopsia)
                {
                    if (!insertarExamenFlag)
                    {
                        insertarExamenFlag = true;

                        Wrapper.InsertarBiopsiaCompleted += new EventHandler<ServiceReferenceClinica.InsertarBiopsiaCompletedEventArgs>(InsertarBiopsia);
                        Wrapper.InsertarBiopsiaAsync(corregirIdMuestra(idMuestra_txt6.Text), fechaFormated, Convert.ToInt32(valorBiopsia_txt.Text), diagnosticoBiopsia_txtR.Xaml.ToString(),
                            diagnosticoClinicoBiopsia_txt.Text.ToString(), idPaciente, idMedico, "Ingresos por Examen", Convert.ToInt32(fechaInformeArray[3]),
                            Convert.ToInt32(fechaInforme.Month.ToString()), Usuario, macroscopica_txtR.Xaml, micro_txtR.Xaml, codificacion_txt.Text.ToString(), fechaInformeString, fechaInformeIngresado, fechaRecibida_txt.Text);
                    }
                }
                else if (insertarCitologiaLiquidos)
                {
                    if (!insertarExamenFlag)
                    {
                        insertarExamenFlag = true;
                        Wrapper.InsertarCitologiaNoGinecologicaCompleted += new EventHandler<ServiceReferenceClinica.InsertarCitologiaNoGinecologicaCompletedEventArgs>(InsertarCitologiaNoGinecologica);
                        Wrapper.InsertarCitologiaNoGinecologicaAsync(corregirIdMuestra(idMuestra_txt6.Text), fechaFormated, Convert.ToInt32(valorBiopsia_txt.Text), diagnosticoBiopsia_txtR.Xaml,
                            diagnosticoClinicoBiopsia_txt.Text, idPaciente, idMedico, "Ingresos por Examen", Convert.ToInt32(fechaInformeArray[3]),
                            Convert.ToInt32(fechaInforme.Month.ToString()), Usuario, macroscopica_txtR.Xaml, micro_txtR.Xaml, "", fechaInformeString, fechaInformeIngresado, fechaRecibida_txt.Text);
                    }
                }
            }
        }

        void InsertarCitologiaNoGinecologica(object sender, ServiceReferenceClinica.InsertarCitologiaNoGinecologicaCompletedEventArgs e)
        {
            mainFlag = true;
            getIDPacienteFlag = true;
            getIDMedicoFlag = true;
            insertarExamenFlag = true;

            enableMainButtons(true);
            if (e.Result == false)
            {
                estadoBiopsia.Text = "Ha ocurrido un error...intente denuevo";
                if (!errorBiopsia)
                {
                    errorBiopsia = true;
                    MessageBox.Show("Se detectó un error!  Ya existe una muestra con el mismo numero");
                }
            }
            else
            {
                if (materialEnviadoSelect.Items.Count > 0)
                {
                    if (!insertarMaterialFlag)
                    {
                        insertarMaterialFlag = true;
                        String materialEnviado = "";

                        for (int i = 0; i < materialEnviadoSelect.Items.Count; i++)
                        {
                            materialEnviado += materialEnviadoSelect.Items.ElementAt(i).ToString() + ";";
                        }

                        Wrapper.InsertarMaterialEnviadoCompleted += new EventHandler<ServiceReferenceClinica.InsertarMaterialEnviadoCompletedEventArgs>(InsertarMaterialEnviado);
                        Wrapper.InsertarMaterialEnviadoAsync(corregirIdMuestra(idMuestra_txt6.Text), materialEnviado, "MaterialEnviadoCitologia");
                    }
                }
                else
                {
                    enableMainButtons(true);
                    enableMatButtons(true);
                    resetCampos();
                    estadoBiopsia.Text = "Citologia de Liquidos insertada con exito!";
                }
            }
        }

        void InsertarBiopsia(object sender, ServiceReferenceClinica.InsertarBiopsiaCompletedEventArgs e)
        {
            mainFlag = true;
            getIDPacienteFlag = true;
            getIDMedicoFlag = true;
            insertarExamenFlag = true;

            if (e.Result == false)
            {
                enableMainButtons(true);
                enableMatButtons(true);
                estadoBiopsia.Text = "Ha ocurrido un error...intente denuevo";
                if (!errorBiopsia)
                {
                    errorBiopsia = true;
                    MessageBox.Show("Se detectó un error!  Ya existe una muestra con el mismo numero");
                }
            }
            else
            {
                if (materialEnviadoSelect.Items.Count > 0)
                {
                    if (!insertarMaterialFlag)
                    {
                        insertarMaterialFlag = true;
                        String materialEnviado = "";

                        for (int i = 0; i < materialEnviadoSelect.Items.Count; i++)
                        {
                            materialEnviado += materialEnviadoSelect.Items.ElementAt(i).ToString() + ";";
                        }

                        Wrapper.InsertarMaterialEnviadoCompleted += new EventHandler<ServiceReferenceClinica.InsertarMaterialEnviadoCompletedEventArgs>(InsertarMaterialEnviado);
                        Wrapper.InsertarMaterialEnviadoAsync(corregirIdMuestra(idMuestra_txt6.Text), materialEnviado, "MaterialEnviadoBiopsia");
                    }
                }
                else
                {
                    enableMainButtons(true);
                    enableMatButtons(true);
                    resetCampos();
                    estadoBiopsia.Text = "Biopsia Insertada con exito!";
                }
            }
        }


        void buscarCitologiaLiquidos(object sender, ServiceReferenceClinica.buscarCitologiaLiquidosCompletedEventArgs e)
        {
            if (buscarB)
            {
                buscarB = false;
                enableMainButtons(true);
                enableMatButtons(true);

                if (e.Result.ToString() == "")
                {
                    estadoBiopsia.Text = "No se encontró ninguna muestra con el ID " + corregirIdMuestra(idMuestra_txt6.Text) + " \nTalvez buscaba una biopsia?";
                }
                else
                {
                    resetCampos();
                    estadoBiopsia.Text = "Datos Obtenidos!";
                    String[] resultado;
                    resultado = e.Result.ToString().Split('$');

                    //Empieza a llenar los campos
                    idMuestra_txt6.Text = resultado[0];
                    fechaBiopsia_txt.Text = resultado[1];
                    nombrePacienteB.Text = resultado[2] + " " + resultado[3];
                    apellidoPacienteB.Text = resultado[4] + " " + resultado[5];
                    edadBiopsia_txt.Text = resultado[6];
                    nombreMedico2.SelectedItem = resultado[7];
                    valorBiopsia_txt.Text = resultado[8];
                    expedienteBiopsia_txt.Text = resultado[9];
                    diagnosticoClinicoBiopsia_txt.Text = resultado[10];

                    if (resultado[11].StartsWith("<"))
                        macroscopica_txtR.Xaml = resultado[11];
                    else
                        diagnosticoBiopsia_txtR.Xaml = crearXamlString(resultado[11]);

                    if (resultado[12].StartsWith("<"))
                        micro_txtR.Xaml = resultado[12];
                    else
                        diagnosticoBiopsia_txtR.Xaml = crearXamlString(resultado[12]);

                    if (resultado[13].StartsWith("<"))
                        diagnosticoBiopsia_txtR.Xaml = resultado[13];
                    else
                        diagnosticoBiopsia_txtR.Xaml = crearXamlString(resultado[13]);

                    fechaRecibida_txt.Text = resultado[14];

                    //Obtiene los datos para las muestras enviadas de la biopsia
                    matEnviadoStatus.Text = "Obteniendo Material Enviado...";
                    Wrapper.getMaterialEnviadoBiopsiaImprimirCompleted += new EventHandler<ServiceReferenceClinica.getMaterialEnviadoBiopsiaImprimirCompletedEventArgs>(getMaterialEnviadoBiopsiaImprimir);
                    Wrapper.getMaterialEnviadoBiopsiaImprimirAsync(idMuestra_txt6.Text, "MaterialEnviadoCitologia");
                }
            }
        }

        void BorrarMuestra(object sender, ServiceReferenceClinica.BorrarMuestraCompletedEventArgs e)
        {
            if (borrarB)
            {
                borrarB = false;
                enableMainButtons(true);
                if (e.Result == false)
                    estadoBiopsia.Text = "Error al borrar, no existe una muestra con el id " + idMuestra_txt6.Text;
                else
                {
                    estadoBiopsia.Text = "Biopsia Borrada con exito!";
                }
            }
        }

        void InsertarMaterial(object sender, ServiceReferenceClinica.InsertarMaterialCompletedEventArgs e)
        {
            materialEnviado_txt.Text = "";
            if (insertarMaterial)
            {
                insertarMaterial = false;
                enableMainButtons(true);
                enableMatButtons(true);
                if (e.Result == false)
                {
                    matEnviadoStatus.Text = "Error!  Ya existe un material con el mismo nombre";
                }
                else
                {
                    matEnviadoStatus.Text = "Material insertado exitosamente!\nActualizando lista de materiales...";
                    Wrapper.getMaterialEnviado_CitologiaNoGinecologicaCompleted += new EventHandler<ServiceReferenceClinica.getMaterialEnviado_CitologiaNoGinecologicaCompletedEventArgs>(getMaterialEnviado_CitologiaNoGinecologica);
                    Wrapper.getMaterialEnviado_CitologiaNoGinecologicaAsync();
                }
            }
        }

        void borarrMaterial(object sender, ServiceReferenceClinica.BorrarMaterialCompletedEventArgs e)
        {
            enableMainButtons(true);
            enableMatButtons(true);
            materialEnviado_txt.Text = "";
            if (insertarMaterial)
            {
                insertarMaterial = false;
                if (e.Result)
                {
                    materialEnviado_txt.Text = "";
                    matEnviadoStatus.Text = "Material borrado exitosamente!\nActualizando lista de materiales...";
                    Wrapper.getMaterialEnviado_CitologiaNoGinecologicaCompleted += new EventHandler<ServiceReferenceClinica.getMaterialEnviado_CitologiaNoGinecologicaCompletedEventArgs>(getMaterialEnviado_CitologiaNoGinecologica);
                    Wrapper.getMaterialEnviado_CitologiaNoGinecologicaAsync();
                }
                else
                    matEnviadoStatus.Text = "Error al borrar material, no existe uno con el nombre " + materialEnviado_txt.Text;
            }
        }

        void getMaterialEnviadoBiopsiaImprimir(object sender, ServiceReferenceClinica.getMaterialEnviadoBiopsiaImprimirCompletedEventArgs e)
        {
            if (e.Result.ToString() == "")
            {
                return;
            }
            else
            {
                matEnviadoStatus.Text = "Datos Obtenidos!";
                String[] resultado;
                materialEnviadoSelect.Items.Clear();
                resultado = e.Result.ToString().Split(';');
                for (int i = 0; i < resultado.Length; i++)
                    if (!resultado[i].Equals(""))
                        materialEnviadoSelect.Items.Add(resultado[i]);
            }
        }

        void InsertarPacienteCompleted(object sender, ServiceReferenceClinica.InsertarPacienteCompletedEventArgs e)
        {
            if (!mainFlag)
            {
                mainFlag = true;
                bool Response;
                Response = e.Result;

                if (Response == false)
                {
                    enableMainButtons(true);
                    enableMatButtons(true);
                    estadoBiopsia.Text = "Error al insertar Paciente";
                }
                else
                {
                    estadoBiopsia.Text = "Paciente insertado con exito !";
                    //Get ID del paciente para Insertar Biopsia
                    if (!getIDPacienteFlag)
                    {
                        getIDPacienteFlag = true;
                        Wrapper.getIDdePacienteCompleted += new EventHandler<ServiceReferenceClinica.getIDdePacienteCompletedEventArgs>(getIDdePaciente);
                        Wrapper.getIDdePacienteAsync(nombre[0], segundoNombre, apellido[0], segundoApellido);
                    }
                }
            }
        }

        void InsertarMaterialEnviado(object sender, ServiceReferenceClinica.InsertarMaterialEnviadoCompletedEventArgs e)
        {
            enableMainButtons(true);
            enableMatButtons(true);
            if (e.Result == false)
            {
                if (!errorMaterial)
                {
                    errorMaterial = true;
                    MessageBox.Show("Error al insertar material enviado");
                }
            }
            else
            {
                resetCampos();
                if (insertarBiopsia)
                    estadoBiopsia.Text = "Biopsia Insertada con exito!";
                else
                    estadoBiopsia.Text = "Citologia de Liquidos Insertada con exito!";
            }
        }

        private void myWebReference_ActualizarBiopsiaCompleted(object sender, ServiceReferenceClinica.ActualizarBiopsiaCompletedEventArgs e)
        {
            bool Response;
            Response = e.Result;

            registrarBiopsia_btn.IsEnabled = true;
            buscarBiopsia_btn.IsEnabled = true;
            borrarBiopsia.IsEnabled = true;
            button1.IsEnabled = true;
            button2.IsEnabled = true;
            agregarMaterialEnviado.IsEnabled = true;
            borrarMaterial_btn.IsEnabled = true;

            if (Response == false)
            {
                estadoBiopsia.Text = "Error al actualizar Biopsia";
            }
            else
            {
                resetCampos();
                estadoBiopsia.Text = "Biopsia actualizada con exito!";
            }
        }

        private void myWebReference_ActualizarCitologiaLiquidosCompleted(object sender, ServiceReferenceClinica.ActualizarCitologiaLiquidosCompletedEventArgs e)
        {
            bool Response;
            Response = e.Result;

            registrarBiopsia_btn.IsEnabled = true;
            buscarBiopsia_btn.IsEnabled = true;
            borrarBiopsia.IsEnabled = true;
            button1.IsEnabled = true;
            button2.IsEnabled = true;
            agregarMaterialEnviado.IsEnabled = true;
            borrarMaterial_btn.IsEnabled = true;

            if (Response == false)
            {
                estadoBiopsia.Text = "Error al actualizar Biopsia";
            }
            else
            {
                resetCampos();
                estadoBiopsia.Text = "Biopsia actualizada con exito!";
            }
        }


        /************************************************************************************
        ************************************************************************************
        *                                                                                  *
        *                      VALIDACIONES Y UTILIDADES                                   *
        *                                                                                  *  
        ************************************************************************************
        ************************************************************************************/
        public string getFechaEnFormatoMysql(String fecha)
        {
            if (fecha.CompareTo("") == 0)
                return "";
            String[] split = fecha.Split('/');
            return split[2] + "-" + split[0] + "-" + split[1];
        }

        private void enableMainButtons(bool valor)
        {
            imprimir_btn.IsEnabled = valor;
            registrarBiopsia_btn.IsEnabled = valor;
            buscarBiopsia_btn.IsEnabled = valor;
            actualizarBiopsia.IsEnabled = valor;
            borrarBiopsia.IsEnabled = valor;
            agregarMaterialEnviado.IsEnabled = valor;
            borrarMaterial_btn.IsEnabled = valor;
            resetFields_btn.IsEnabled = valor;

            if (valor)
            {
                reset_icon.Opacity = 1;
                print_icon.Opacity = 1;
                registrar_icon.Opacity = 1;
                buscar_icon.Opacity = 1;
                actualizar_icon.Opacity = 1;
                borrar_icon.Opacity = 1;
            }
            else
            {
                reset_icon.Opacity = 0.5;
                print_icon.Opacity = 0.5;
                registrar_icon.Opacity = 0.5;
                buscar_icon.Opacity = 0.5;
                actualizar_icon.Opacity = 0.5;
                borrar_icon.Opacity = 0.5;
            }
        }

        private void enableMatButtons(bool valor)
        {
            button1.IsEnabled = valor;
            button2.IsEnabled = valor;

            if (valor)
            {
                left1.Opacity = 1;
                left2.Opacity = 1;
                right1.Opacity = 1;
                right2.Opacity = 1;
            }
            else
            {
                left1.Opacity = 0.5;
                left2.Opacity = 0.5;
                right1.Opacity = 0.5;
                right2.Opacity = 0.5;
            }
        }

        private void resetCampos()
        {
            //BIOPSIA
            nombrePacienteB.Text = "";
            apellidoPacienteB.Text = "";
            nombreMedico2.SelectedIndex = -1;
            fechaBiopsia_txt.Text = "";
            edadBiopsia_txt.Text = "";
            valorBiopsia_txt.Text = "";
            diagnosticoClinicoBiopsia_txt.Text = "";
            materialEnviadoSelect.Items.Clear();
            codificacion_txt.Text = "";
            
            macroscopica_txtR.SelectAll();
            macroscopica_txtR.Selection.Text = "";
            micro_txtR.SelectAll();
            micro_txtR.Selection.Text = "";
            diagnosticoBiopsia_txtR.SelectAll();
            diagnosticoBiopsia_txtR.Selection.Text = "";

            

            fechaRecibida_txt.Text = "";
        }

        public void traducirFecha(String[] fecha)
        {
            char[] charsToTrim = { ',', '.', ' ' };
            for (int i = 0; i < fecha.Length; i++)
                fecha[i] = fecha[i].TrimEnd(charsToTrim);

            foreach (string word in fecha)
            {
                switch (word)
                {
                    //DIAS
                    case "Monday":
                        fechaInformeArray[0] = "Lunes";
                        break;
                    case "Tuesday":
                        fechaInformeArray[0] = "Martes";
                        break;
                    case "Wednesday":
                        fechaInformeArray[0] = "Miercoles";
                        break;
                    case "Thursday":
                        fechaInformeArray[0] = "Jueves";
                        break;
                    case "Friday":
                        fechaInformeArray[0] = "Viernes";
                        break;
                    case "Saturday":
                        fechaInformeArray[0] = "Sabado";
                        break;
                    case "Sunday":
                        fechaInformeArray[0] = "Domingo";
                        break;

                    //MESES
                    case "January":
                        fechaInformeArray[1] = "Enero";
                        break;
                    case "February":
                        fechaInformeArray[1] = "Febrero";
                        break;
                    case "March":
                        fechaInformeArray[1] = "Marzo";
                        break;
                    case "April":
                        fechaInformeArray[1] = "Abril";
                        break;
                    case "May":
                        fechaInformeArray[1] = "Mayo";
                        break;
                    case "June":
                        fechaInformeArray[1] = "Junio";
                        break;
                    case "July":
                        fechaInformeArray[1] = "Julio";
                        break;
                    case "August":
                        fechaInformeArray[1] = "Agosto";
                        break;
                    case "September":
                        fechaInformeArray[1] = "Septiembre";
                        break;
                    case "October":
                        fechaInformeArray[1] = "Octubre";
                        break;
                    case "November":
                        fechaInformeArray[1] = "Noviembre";
                        break;
                    case "December":
                        fechaInformeArray[1] = "Diciembre";
                        break;
                }
            }
        }

        private void edadBiopsia_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            //edadBiopsia_txt.Text.Trim();
            if (numeros.IsMatch(edadBiopsia_txt.Text))
            {
                edadPaciente = int.Parse(edadBiopsia_txt.Text);
                enableMainButtons(true);
            }
            else
            {
                enableMainButtons(false);
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                edad_error.Source = new BitmapImage(imgURI);
            }
        }

        private void idMuestra_txt6_TextChanged(object sender, TextChangedEventArgs e)
        {
            id_error.Source = MainPage.bmpClear;
        }

        private void nombrePacienteB_TextChanged(object sender, TextChangedEventArgs e)
        {
            nombre_error.Source = MainPage.bmpClear;
        }

        private void nombreMedico2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            medico_error.Source = MainPage.bmpClear;
        }

        private void edadBiopsia_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            enableMainButtons(true);
        }

        private void valorBiopsia_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            valorBiopsia_txt.Text.Trim();
            if (numeros.IsMatch(valorBiopsia_txt.Text))
            {
                int valor = int.Parse(valorBiopsia_txt.Text);
                valor_error.Source = MainPage.bmpClear;
            }
            else
            {
                valorBiopsia_txt.Text = "";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                valor_error.Source = new BitmapImage(imgURI);
            }
        }

        private void materialEnviado_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            material_error.Source = MainPage.bmpClear;
        }

        private String corregirIdMuestra(String id)
        {
            String idFinal = id;
            string[] test = id.Split('-');
            String tipo = "";

            if (insertarBiopsia)
                tipo = "B";
            else if (insertarCitologiaLiquidos)
                tipo = "CL";

            if (!idFinal.Equals(""))
            {
                int idInt;
                try
                {
                    if (test[0].StartsWith(tipo))
                        idInt = Convert.ToInt32(test[0].Substring(1));
                    else
                        idInt = Convert.ToInt32(test[0]);
                }
                catch (Exception exp)
                {
                    idInt = 100;
                }


                if (test[0].StartsWith(tipo))
                {
                    idFinal = test[0];
                }
                else
                {
                    if (idInt < 10)
                        test[0] = "00" + test[0];
                    else if (idInt < 100)
                        test[0] = "0" + test[0];
                    idFinal = tipo + test[0];
                }

            }
            try
            {
                if (!test[1].Equals(""))
                    idFinal = idFinal + "-" + test[1];
            }
            catch (Exception exp)
            {
                idFinal = idFinal + anioActual;
            }

            return idFinal;
        }

        private String crearXamlString(String text)
        {
            String xaml = "<Section xml:space=\"preserve\" HasTrailingParagraphBreakOnPaste=\"False\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"";
            xaml += text;
            xaml += "\" /></Paragraph></Section>";
            return xaml;
        }

        private void resetFields_btn_Click(object sender, RoutedEventArgs e)
        {
            idMuestra_txt6.Text = "";
            resetCampos();
        }

        private void ReturnFocus(RichTextBox textBox)
        {
            if (textBox != null)
                textBox.Focus();
        }


        #region Bold, Italics & Underline
        //Set Bold formatting to the selected content 
        private void btnBold_Click(object sender, RoutedEventArgs e)
        {
            if (macroscopica_txtR != null && macroscopica_txtR.Selection.Text.Length > 0)
            {
                if (macroscopica_txtR.Selection.GetPropertyValue(Run.FontWeightProperty) is FontWeight && ((FontWeight)macroscopica_txtR.Selection.GetPropertyValue(Run.FontWeightProperty)) == FontWeights.Normal)
                    macroscopica_txtR.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Bold);
                else
                    macroscopica_txtR.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Normal);
                ReturnFocus(macroscopica_txtR);
            }
            else if (micro_txtR != null && micro_txtR.Selection.Text.Length > 0)
            {
                if (micro_txtR.Selection.GetPropertyValue(Run.FontWeightProperty) is FontWeight && ((FontWeight)micro_txtR.Selection.GetPropertyValue(Run.FontWeightProperty)) == FontWeights.Normal)
                    micro_txtR.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Bold);
                else
                    micro_txtR.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Normal);
                ReturnFocus(micro_txtR);
            }
            else if (diagnosticoBiopsia_txtR != null && diagnosticoBiopsia_txtR.Selection.Text.Length > 0)
            {
                if (diagnosticoBiopsia_txtR.Selection.GetPropertyValue(Run.FontWeightProperty) is FontWeight && ((FontWeight)diagnosticoBiopsia_txtR.Selection.GetPropertyValue(Run.FontWeightProperty)) == FontWeights.Normal)
                    diagnosticoBiopsia_txtR.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Bold);
                else
                    diagnosticoBiopsia_txtR.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Normal);
                ReturnFocus(diagnosticoBiopsia_txtR);
            }
        }

        //Set Italic formatting to the selected content 
        private void btnItalic_Click(object sender, RoutedEventArgs e)
        {
            if (macroscopica_txtR != null && macroscopica_txtR.Selection.Text.Length > 0)
            {
                if (macroscopica_txtR.Selection.GetPropertyValue(Run.FontStyleProperty) is FontStyle && ((FontStyle)macroscopica_txtR.Selection.GetPropertyValue(Run.FontStyleProperty)) == FontStyles.Normal)
                    macroscopica_txtR.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Italic);
                else
                    macroscopica_txtR.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Normal);
                ReturnFocus(macroscopica_txtR);
            }
            else if (micro_txtR != null && micro_txtR.Selection.Text.Length > 0)
            {
                if (micro_txtR.Selection.GetPropertyValue(Run.FontStyleProperty) is FontStyle && ((FontStyle)micro_txtR.Selection.GetPropertyValue(Run.FontStyleProperty)) == FontStyles.Normal)
                    micro_txtR.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Italic);
                else
                    micro_txtR.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Normal);
                ReturnFocus(micro_txtR);
            }
            else if (diagnosticoBiopsia_txtR != null && diagnosticoBiopsia_txtR.Selection.Text.Length > 0)
            {
                if (diagnosticoBiopsia_txtR.Selection.GetPropertyValue(Run.FontStyleProperty) is FontStyle && ((FontStyle)diagnosticoBiopsia_txtR.Selection.GetPropertyValue(Run.FontStyleProperty)) == FontStyles.Normal)
                    diagnosticoBiopsia_txtR.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Italic);
                else
                    diagnosticoBiopsia_txtR.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Normal);
                ReturnFocus(diagnosticoBiopsia_txtR);
            }
        }

        //Set Underline formatting to the selected content 
        private void btnUnderline_Click(object sender, RoutedEventArgs e)
        {
            if (macroscopica_txtR != null && macroscopica_txtR.Selection.Text.Length > 0)
            {
                if (macroscopica_txtR.Selection.GetPropertyValue(Run.TextDecorationsProperty) == null)
                    macroscopica_txtR.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Underline);
                else
                    macroscopica_txtR.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, null);
                ReturnFocus(macroscopica_txtR);
            }
            else if (micro_txtR != null && micro_txtR.Selection.Text.Length > 0)
            {
                if (micro_txtR.Selection.GetPropertyValue(Run.TextDecorationsProperty) == null)
                    micro_txtR.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Underline);
                else
                    micro_txtR.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, null);
                ReturnFocus(micro_txtR);
            }
            else if (diagnosticoBiopsia_txtR != null && diagnosticoBiopsia_txtR.Selection.Text.Length > 0)
            {
                if (diagnosticoBiopsia_txtR.Selection.GetPropertyValue(Run.TextDecorationsProperty) == null)
                    diagnosticoBiopsia_txtR.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Underline);
                else
                    diagnosticoBiopsia_txtR.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, null);
                ReturnFocus(diagnosticoBiopsia_txtR);
            }
        }
        #endregion

        #region Font Type, Color & size

        //Set font type to selected content
        private void cmbFonts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (macroscopica_txtR != null && macroscopica_txtR.Selection.Text.Length > 0)
            {
                macroscopica_txtR.Selection.ApplyPropertyValue(Run.FontFamilyProperty, new FontFamily((cmbFonts.SelectedItem as ComboBoxItem).Tag.ToString()));
                ReturnFocus(macroscopica_txtR);
            }
            else if (micro_txtR != null && micro_txtR.Selection.Text.Length > 0)
            {
                micro_txtR.Selection.ApplyPropertyValue(Run.FontFamilyProperty, new FontFamily((cmbFonts.SelectedItem as ComboBoxItem).Tag.ToString()));
                ReturnFocus(micro_txtR);
            }
            else if (diagnosticoBiopsia_txtR != null && diagnosticoBiopsia_txtR.Selection.Text.Length > 0)
            {
                diagnosticoBiopsia_txtR.Selection.ApplyPropertyValue(Run.FontFamilyProperty, new FontFamily((cmbFonts.SelectedItem as ComboBoxItem).Tag.ToString()));
                ReturnFocus(diagnosticoBiopsia_txtR);
            }
        }

        //Set font size to selected content
        private void cmbFontSizes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (macroscopica_txtR != null && macroscopica_txtR.Selection.Text.Length > 0)
            {
                macroscopica_txtR.Selection.ApplyPropertyValue(Run.FontSizeProperty, double.Parse((cmbFontSizes.SelectedItem as ComboBoxItem).Tag.ToString()));
                ReturnFocus(macroscopica_txtR);
            }
            else if (micro_txtR != null && micro_txtR.Selection.Text.Length > 0)
            {
                micro_txtR.Selection.ApplyPropertyValue(Run.FontSizeProperty, double.Parse((cmbFontSizes.SelectedItem as ComboBoxItem).Tag.ToString()));
                ReturnFocus(micro_txtR);
            }
            else if (diagnosticoBiopsia_txtR != null && diagnosticoBiopsia_txtR.Selection.Text.Length > 0)
            {
                diagnosticoBiopsia_txtR.Selection.ApplyPropertyValue(Run.FontSizeProperty, double.Parse((cmbFontSizes.SelectedItem as ComboBoxItem).Tag.ToString()));
                ReturnFocus(diagnosticoBiopsia_txtR);
            }
        }

        //Set font color to selected content
        private void cmbFontColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (macroscopica_txtR != null && macroscopica_txtR.Selection.Text.Length > 0)
            {
                string color = (cmbFontColors.SelectedItem as ComboBoxItem).Tag.ToString();

                SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(
                    byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)));

                macroscopica_txtR.Selection.ApplyPropertyValue(Run.ForegroundProperty, brush);
                ReturnFocus(macroscopica_txtR);
            }
            else if (micro_txtR != null && micro_txtR.Selection.Text.Length > 0)
            {
                string color = (cmbFontColors.SelectedItem as ComboBoxItem).Tag.ToString();

                SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(
                    byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)));

                micro_txtR.Selection.ApplyPropertyValue(Run.ForegroundProperty, brush);
                ReturnFocus(micro_txtR);
            }
            else if (diagnosticoBiopsia_txtR != null && diagnosticoBiopsia_txtR.Selection.Text.Length > 0)
            {
                string color = (cmbFontColors.SelectedItem as ComboBoxItem).Tag.ToString();

                SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(
                    byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)));

                diagnosticoBiopsia_txtR.Selection.ApplyPropertyValue(Run.ForegroundProperty, brush);
                ReturnFocus(diagnosticoBiopsia_txtR);
            }
        }

        #endregion
    }
}
