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

namespace Sistema_BD_Clinica_Patologica.Views
{
    public partial class Citologia : Page
    {
        int edadPaciente = 0;
        double precioCitologia = 0;
        int basales = 0;
        int intermedias = 0;
        int superficiales = 0;
        int repetirMeses = 0;
        bool diu = false;
        bool anticonceptivos = false;
        int idPaciente = 0;
        int idMedico = 0;
        bool candidaSp = false;
        bool gardnerella = false;
        bool herpes = false;
        bool vaginosis = false;
        bool triconomas = false;
        bool negativo = false;
        bool celulasAtipicas = false;
        bool escamosa = false;
        bool glandular = false;
        bool lesionBajoGrado = false;
        bool nic1 = false;
        bool infeccionVPH = false;
        bool lesionAltoGrado = false;
        bool nic2 = false;
        bool nic3 = false;
        bool carcinoma = false;
        bool adenocarcinoma = false;
        bool colposcopia = false;
        bool repetirBiopsia = false;
        bool dspsTratamiento = false;
        string primerNombre = "";
        string segundoNombre = "";
        string primerApellido = "";
        string segundoApellido = "";
        string nombreMedicoString = "";
        string inflamacion = "";
        string origenMuestra = "";
        bool calidadFrotis = false;
        string[] fecha;
        string[] fechaBiopsia;
        string material;
        String[] nombre = new String[2];
        String[] apellido = new String[2];

        DateTime fechaInforme = DateTime.Now;
        String[] fechaInformeArray = new String[4];
        String fechaInformeIngresado = "";
        String fechaInformeString = "";

        String anioActual = "";
        //Control Flags
        bool insertarPacienteFlag = true;
        bool getIDPacienteFlag = true;
        bool getIDMedicoFlag = true;
        bool insertarExamenFlag = true;
        bool errorCitologia = true;
        bool insertarMaterialFlag = true;
        bool flagMedico = true;
        bool masterFlag = true;

        String[] diagnosticoDescriptivo = new String[12];
        String Usuario = App.Correo;

        //Expresiones Regulares para las validaciones de los campos
        Regex numeros = new Regex("^[0-9]+$");

        /********************************************************************************************************************************
        **************************************************    Inicializadores   *********************************************************
        *********************************************************************************************************************************/

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private Sistema_BD_Clinica_Patologica.ServiceReferenceClinica.WSClinicaSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:5633/WSClinica.asmx";

        public Citologia()
        {
            InitializeComponent();
            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReferenceClinica.WSClinicaSoapClient(bind, endpoint);

            //FORMATO DE FECHA QUE DEVUELTE:
            //Ej. Friday, December 17, 2010
            fechaInformeArray = fechaInforme.ToLongDateString().Split();
            fechaInformeIngresado = getFechaEnFormatoMysql(fechaInforme.Date.ToShortDateString());

            //Al traducirla, sale el arreglo asi: Viernes Diciembre 17 2010
            traducirFecha(fechaInformeArray);
            //Dia actual en string junto y asignacion del año para el ID de las muestras
            fechaInformeString = fechaInformeArray[0] + " " + fechaInformeArray[1] + " " + fechaInformeArray[2] + ", " + fechaInformeArray[3];
            anioActual = "-" + fechaInformeArray[3].Substring(2);

            nombreMedico.IsEnabled = false;
            enableButtons(false);

            Wrapper.getMedicos_NombresCompleted += new EventHandler<ServiceReferenceClinica.getMedicos_NombresCompletedEventArgs>(WebS_getMedicos);
            Wrapper.getMedicos_NombresAsync();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /********************************************************************************************************************************
        *********************************************************************************************************************************
        *********************************************                           *********************************************************
        *********************************************     MAIN BUTTON EVENTS    *********************************************************
        *********************************************                           *********************************************************
        *********************************************************************************************************************************
        *********************************************************************************************************************************/

        private void imprimir_btn_Click(object sender, RoutedEventArgs e)
        {
            String idFinal = idMuestra_txt.Text;
            string[] test = idMuestra_txt.Text.Split('-');

            if (!idFinal.Equals(""))
            {
                int idInt;
                try
                {
                    if (test[0].StartsWith("C"))
                        idInt = Convert.ToInt32(test[0].Substring(1));
                    else
                        idInt = Convert.ToInt32(test[0]);
                }
                catch (Exception exp)
                {
                    idInt = 100;
                }


                if (test[0].StartsWith("C"))
                {
                    idFinal = test[0];
                }
                else
                {
                    if (idInt < 10)
                        test[0] = "00" + test[0];
                    else if (idInt < 100)
                        test[0] = "0" + test[0];
                    idFinal = "C" + test[0];
                }

            }
            try
            {
                if (!test[1].Equals(""))
                {

                    NavigationService.Navigate(new Uri("/Formulario" + "?ID=" + idFinal + "-" + test[1], UriKind.Relative));
                }
                else
                    NavigationService.Navigate(new Uri("/Formulario" + "?ID=" + idFinal + anioActual, UriKind.Relative));
            }
            catch (Exception exp)
            {
                if (idMuestra_txt.Text.Equals(""))
                    NavigationService.Navigate(new Uri("/Formulario" + "?ID=", UriKind.Relative));
                else
                    NavigationService.Navigate(new Uri("/Formulario" + "?ID=" + idFinal + anioActual, UriKind.Relative));
            }
        }

        private void guardarBtn_Click(object sender, RoutedEventArgs e)
        {
            //MASTER FLAGS
            insertarPacienteFlag = true;
            getIDPacienteFlag = true;
            getIDMedicoFlag = true;
            insertarExamenFlag = true;
            errorCitologia = true;
            masterFlag = true;

            if (idMuestra_txt.Text.Length == 0)
            {
                estado.Text = "Escriba un ID de Muestra";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (primerNombrePaciente_txt.Text.Length == 0)
            {
                estado.Text = "Escriba el Primer Nombre del paciente";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                nombre_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (primerApellidoPaciente_txt.Text.Length == 0)
            {
                estado.Text = "Escriba el Primer Apellido del paciente";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                apellido_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (nombreMedico.SelectedItem == null)
            {
                estado.Text = "Seleccione un Médico";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                medico_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (fechaMuestra.Text.Length == 0)
            {
                estado.Text = "Ingrese la Fecha de la Muestra";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                fecha_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (valor_txt.Text.Length == 0)
            {
                estado.Text = "Ingrese el precio de la Muestra!";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                valor_error.Source = new BitmapImage(imgURI);
                return;
            }
            estado.Text = "Guardando Datos...";
            enableButtons(false);

            string[] test = idMuestra_txt.Text.Split('-');

            Wrapper.InsertarPacienteCompleted += new EventHandler<ServiceReferenceClinica.InsertarPacienteCompletedEventArgs>(WebS_InsertarPacienteCompleted);
            Wrapper.InsertarPacienteAsync(diu, FUR_txt.Text, FUP_txt.Text, primerNombrePaciente_txt.Text, segundoNombrePaciente_txt.Text,
                primerApellidoPaciente_txt.Text, segundoApellidoPaciente_txt.Text, edadPaciente, anticonceptivos, expediente_txt.Text);
        }

        //EN REALIDAD ES ACTUALIZAR.... >__>
        private void guardar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (idMuestra_txt.Text.Length == 0)
            {
                estado.Text = "Escriba un ID de Muestra";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (primerNombrePaciente_txt.Text.Length == 0)
            {
                estado.Text = "Escriba el Primer Nombre del paciente";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                nombre_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (primerApellidoPaciente_txt.Text.Length == 0)
            {
                estado.Text = "Escriba el Primer Apellido del paciente";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                apellido_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (nombreMedico.SelectedItem == null)
            {
                estado.Text = "Seleccione un Médico";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                medico_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (fechaMuestra.Text.Length == 0)
            {
                estado.Text = "Ingrese la Fecha de la Muestra";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                fecha_error.Source = new BitmapImage(imgURI);
                return;
            }

            if (valor_txt.Text.Length == 0)
            {
                estado.Text = "Ingrese el precio de la Muestra!";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                valor_error.Source = new BitmapImage(imgURI);
                return;
            }


            if (basales_txt.Text.Length == 0)
                basales_txt.Text = "0";
            if (superficiales_txt.Text.Length == 0)
                superficiales_txt.Text = "0";
            if (intermedias_txt.Text.Length == 0)
                intermedias_txt.Text = "0";
            if (repetirMeses_txt.Text.Length == 0)
                repetirMeses_txt.Text = "0";

            string fechaFormated = fecha[2] + "-" + fecha[0] + "-" + fecha[1];

            estado.Text = "Guardando Datos...";
            enableButtons(false);
            masterFlag = true;

            Wrapper.ActualizarCitologiaCompleted += new EventHandler<ServiceReferenceClinica.ActualizarCitologiaCompletedEventArgs>(myWebReference_ActualizarCitologiaCompleted);
            Wrapper.ActualizarCitologiaAsync(corregirIdMuestra(idMuestra_txt.Text), diagnostico_richtext.Xaml, comentario_txtR.Xaml, inflamacion,
                cfCausa_txt.Text, calidadFrotis, candidaSp, gardnerella, vaginosis, herpes, triconomas, otro_txt.Text, Convert.ToInt32(basales_txt.Text.ToString()),
                Convert.ToInt32(intermedias_txt.Text.ToString()), Convert.ToInt32(superficiales_txt.Text.ToString()), colposcopia,
                Convert.ToInt32(repetirMeses_txt.Text.ToString()), otraRecomendacion_txt.Text.ToString(), repetirBiopsia, dspsTratamiento,
                nic1, nic2, nic3, origenMuestra, negativo, infeccionVPH, glandular, escamosa, adenocarcinoma, carcinoma, celulasAtipicas, lesionAltoGrado, lesionBajoGrado,
                anticonceptivos, fechaFormated, diagnosticoClinico_txt.Text, diu, FUR_txt.Text, FUR_txt.Text, primerNombrePaciente_txt.Text, segundoNombrePaciente_txt.Text,
                primerApellidoPaciente_txt.Text, segundoApellidoPaciente_txt.Text, Convert.ToInt32(edad_txt.Text), expediente_txt.Text, Convert.ToInt32(valor_txt.Text));
        }

        private void buscar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (idMuestra_txt.Text.Length == 0)
            {
                estado.Text = "Escriba el No. de Muestra a buscar!";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_error.Source = new BitmapImage(imgURI);
                return;
            }

            estado.Text = "Buscando Datos...";
            enableButtons(false);
            masterFlag = true;


            Wrapper.buscarCitologiaCompleted += new EventHandler<ServiceReferenceClinica.buscarCitologiaCompletedEventArgs>(myWebReference_buscarCitologiaCompleted);
            Wrapper.buscarCitologiaAsync(corregirIdMuestra(idMuestra_txt.Text));
        }

        private void borrarBtn_Click(object sender, RoutedEventArgs e)
        {
            if (idMuestra_txt.Text.CompareTo("") == 0)
            {
                estado.Text = "El número de la muestra no puede estar vacio...";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_error.Source = new BitmapImage(imgURI);
                return;
            }

            MessageBoxResult msgResult = MessageBox.Show("Esta a punto de borrar una Citologia, \nEsta seguro que quiere continuar?", "Borrando Citologia", System.Windows.MessageBoxButton.OKCancel);
            if (msgResult == MessageBoxResult.Cancel)
                return;

            estado.Text = "Borrando Citología...";
            enableButtons(false);
            masterFlag = true;


            Wrapper.BorrarMuestraCompleted += new EventHandler<ServiceReferenceClinica.BorrarMuestraCompletedEventArgs>(myWebReference_BorrarMuestraCompleted);
            Wrapper.BorrarMuestraAsync(corregirIdMuestra(idMuestra_txt.Text));
        }

        private void normal_btn_Click(object sender, RoutedEventArgs e)
        {
            String comentario = "Se observan células de exo y endocervix sin alteraciones";

            calidadFrotis_combo.SelectedItem = calidadFrotis_combo.Items[0];
            inflamacion_combo.SelectedItem = inflamacion_combo.Items[0];

            celulasAtipicas_check.IsChecked = false;
            celulasAtipicas_check.IsEnabled = false;

            lesionAltoGrado_check.IsChecked = false;
            lesionAltoGrado_check.IsEnabled = false;

            lesionBajoGrado_check.IsChecked = false;
            lesionBajoGrado_check.IsEnabled = false;

            carcinoma_check.IsChecked = false;
            carcinoma_check.IsEnabled = false;
            adenocarcinoma_check.IsChecked = false;
            adenocarcinoma_check.IsEnabled = false;

            negativo_check.IsChecked = true;
            negativo_check.IsEnabled = true;

            comentario_txtR.Xaml = crearXamlString(comentario);
            negativo = true;
        }


        /********************************************************************************************************************************
        *********************************************************************************************************************************
        ****************************************                                     ****************************************************
        ****************************************     NON FUNCTIONAL BUTTON EVENTS    ****************************************************
        ****************************************                                     ****************************************************
        *********************************************************************************************************************************
        *********************************************************************************************************************************/

        private void increaseSize_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void decreaseSize_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bold_btn_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (comentarioTxt != null)
            {
                if (comentarioTxt.Selection.GetPropertyValue(Run.FontWeightProperty) is FontWeight &&
                ((FontWeight)comentarioTxt.Selection.GetPropertyValue(Run.FontWeightProperty)) ==
                FontWeights.Normal)
                    comentarioTxt.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Bold);
                else
                    comentarioTxt.Selection.ApplyPropertyValue(Run.FontWeightProperty,
                        FontWeights.Normal);
            }

            if (diagnostico_richtext != null)
            {
                if (diagnostico_richtext.Selection.GetPropertyValue(Run.FontWeightProperty) is FontWeight &&
                ((FontWeight)diagnostico_richtext.Selection.GetPropertyValue(Run.FontWeightProperty)) ==
                FontWeights.Normal)
                    diagnostico_richtext.Selection.ApplyPropertyValue(Run.FontWeightProperty,
                        FontWeights.Bold);
                else
                    diagnostico_richtext.Selection.ApplyPropertyValue(Run.FontWeightProperty,
                        FontWeights.Normal);
            }
         */
        }

        private void underline_btn_Click(object sender, RoutedEventArgs e)
        {
            /* 
               if (comentarioTxt != null)
               {
                   if (comentarioTxt.Selection.GetPropertyValue(Run.TextDecorationsProperty) == null)
                       comentarioTxt.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Underline);
                   else
                       comentarioTxt.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, null);
               }

               if (diagnostico_richtext != null)
               {
                   if (diagnostico_richtext.Selection.GetPropertyValue(Run.TextDecorationsProperty) == null)
                       diagnostico_richtext.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Underline);
                   else
                       diagnostico_richtext.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, null);
               }
             */
        }

        private void color_btn_Click(object sender, RoutedEventArgs e)
        {
            /*
              SolidColorBrush black = new SolidColorBrush(Colors.Black);
              SolidColorBrush red = new SolidColorBrush(Colors.Red);

              if (comentarioTxt != null)
              {
                  if (comentarioTxt.Selection.GetPropertyValue(Run.ForegroundProperty) == black)
                      comentarioTxt.Selection.ApplyPropertyValue(Run.ForegroundProperty, red);
                  else
                      comentarioTxt.Selection.ApplyPropertyValue(Run.ForegroundProperty, black);
              }

              if (diagnostico_richtext != null)
              {
                  if (diagnostico_richtext.Selection.GetPropertyValue(Run.ForegroundProperty) == black)
                      diagnostico_richtext.Selection.ApplyPropertyValue(Run.ForegroundProperty, red);
                  else
                      diagnostico_richtext.Selection.ApplyPropertyValue(Run.ForegroundProperty, black);
              }
           * */
        }

        /********************************************************************************************************************************
        *********************************************************************************************************************************
        *********************************************                           *********************************************************
        *********************************************   METHOD RESPONCE EVENTS  *********************************************************
        *********************************************                           *********************************************************
        *********************************************************************************************************************************
        *********************************************************************************************************************************/

        void WebS_getMedicos(object sender, ServiceReferenceClinica.getMedicos_NombresCompletedEventArgs e)
        {
            if (flagMedico)
            {
                flagMedico = false;
                for (int i = nombreMedico.Items.Count - 1; i > -1; i--)
                    nombreMedico.Items.RemoveAt(i);

                String[] resp = e.Result.ToString().Split(';');
                for (int i = 0; i < resp.Length; i++)
                {
                    if (!resp.Equals(""))
                        nombreMedico.Items.Add(resp[i]);
                }

                nombreMedico.IsEnabled = true;
                enableButtons(true);
                estado.Text = "Datos Obtenidos!";
            }
        }

        void WebS_getIDdePacienteCompleted(object sender, ServiceReferenceClinica.getIDdePacienteCompletedEventArgs e)
        {
            int Response;
            Response = e.Result;
            estado.Text = "Obteniendo ID del paciente...";
            if (Response == -1)
            {
                estado.Text = "Error al obtener el ID del Paciente";
                enableButtons(true);
                return;
            }
            else
            {
                idPaciente = e.Result;
                if (getIDMedicoFlag)
                {
                    getIDMedicoFlag = false;
                    Wrapper.getIDdeMedicoCompleted += new EventHandler<ServiceReferenceClinica.getIDdeMedicoCompletedEventArgs>(WebS_getIDdeMedicoCompleted);
                    Wrapper.getIDdeMedicoAsync(nombreMedico.SelectedItem.ToString());
                }
            }
        }

        void WebS_InsertarPacienteCompleted(object sender, ServiceReferenceClinica.InsertarPacienteCompletedEventArgs e)
        {
            if (insertarPacienteFlag)
            {
                insertarPacienteFlag = false;
                bool Response;
                Response = e.Result;
                if (Response == false)
                {
                    estado.Text = "Error al insertar Paciente";
                    enableButtons(true);
                    return;
                }
                else
                {
                    estado.Text = "Paciente insertado con exito!";
                    //Get ID del paciente para Insertar Citologia
                    if (getIDPacienteFlag)
                    {
                        getIDPacienteFlag = false;
                        Wrapper.getIDdePacienteCompleted += new EventHandler<ServiceReferenceClinica.getIDdePacienteCompletedEventArgs>(WebS_getIDdePacienteCompleted);
                        Wrapper.getIDdePacienteAsync(primerNombrePaciente_txt.Text, segundoNombrePaciente_txt.Text, primerApellidoPaciente_txt.Text, segundoApellidoPaciente_txt.Text);
                    }
                }
            }
        }

        void actualizarPacienteCompletedInsertar(object sender, ServiceReferenceClinica.ActualizarPacienteCompletedEventArgs e)
        {
            bool Response = e.Result;
            if (Response)
            {
                if (getIDMedicoFlag)
                {
                    getIDMedicoFlag = false;
                    Wrapper.getIDdeMedicoCompleted += new EventHandler<ServiceReferenceClinica.getIDdeMedicoCompletedEventArgs>(WebS_getIDdeMedicoCompleted);
                    Wrapper.getIDdeMedicoAsync(nombreMedico.SelectedItem.ToString());
                }
            }
            else
            {
                estado.Text = "Error al actualizar el paciente";
                enableButtons(true);
                return;
            }
        }

        void WebS_getIDdeMedicoCompleted(object sender, ServiceReferenceClinica.getIDdeMedicoCompletedEventArgs e)
        {
            int Response;
            Response = e.Result;

            if (Response == -1)
            {
                enableButtons(true);
                estado.Text = "Error al leer el ID del Medico";
            }
            else
            {
                if (valor_txt.Text == "")
                    valor_txt.Text = "0";
                if (basales_txt.Text == "")
                    basales_txt.Text = "0";
                if (intermedias_txt.Text == "")
                    intermedias_txt.Text = "0";
                if (superficiales_txt.Text == "")
                    superficiales_txt.Text = "0";
                if (repetirMeses_txt.Text == "")
                    repetirMeses_txt.Text = "0";

                string fechaFormated = fecha[2] + "-" + fecha[0] + "-" + fecha[1];

                idMedico = e.Result;
                estado.Text = "Insertando Citologia...";

                if (insertarExamenFlag)
                {
                    insertarExamenFlag = false;

                    Wrapper.InsertarCitologiaGinecologicaCompleted += new EventHandler<ServiceReferenceClinica.InsertarCitologiaGinecologicaCompletedEventArgs>(WebS_InsertarCitologiaGinecologicaCompleted);
                    Wrapper.InsertarCitologiaGinecologicaAsync(corregirIdMuestra(idMuestra_txt.Text), fechaFormated, Convert.ToInt32(valor_txt.Text.ToString()), diagnostico_richtext.Xaml,
                        diagnosticoClinico_txt.Text, idPaciente, idMedico, "Ingresos por Examen", Convert.ToInt32(fechaInformeArray[3]), Convert.ToInt32(fechaInforme.Month.ToString()), Usuario, comentario_txtR.Xaml, inflamacion, cfCausa_txt.Text, calidadFrotis, anticonceptivos,
                        candidaSp, gardnerella, vaginosis, herpes, triconomas, otro_txt.Text, Convert.ToInt32(basales_txt.Text.ToString()), Convert.ToInt32(intermedias_txt.Text.ToString()),
                        Convert.ToInt32(superficiales_txt.Text.ToString()), colposcopia, Convert.ToInt32(repetirMeses_txt.Text.ToString()), otraRecomendacion_txt.Text.ToString(), repetirBiopsia, dspsTratamiento,
                        nic1, nic2, nic3, origenMuestra, negativo, infeccionVPH, glandular, escamosa, adenocarcinoma, carcinoma, celulasAtipicas, lesionAltoGrado, lesionBajoGrado, fechaInformeString, fechaInformeIngresado);

                }

                /*                
                public bool InsertarCitologiaGinecologica(String cod_examen, String fecha, int precio, String Diagnostico,
                String diagnostico_medico, int Id_paciente, int id_medico, String Nombre_Contable,
                int anio_contable, int mes_contable, String usuario_empleado, String comentario,
                String inflamacion, String calidadFrotis_causa, bool calidadFrotisAdecuado, bool anticonceptivos,
                bool candida_sp, bool gardnerela, bool vaginosis, bool herpes, bool tricomonas,
                String otroAgenteInfeccioso, int evaluacionHormonal_basales,
                int evaluacionhormonal_intermedias, int evaluacionhormonal_superficiales, bool Colposcopia,
                int repetir, String recomendaciones_otra, bool recomendaciones_biopsia, bool recomendaciones_tratamientos,
                bool NIC_I, bool NIC_II, bool NIC_III, String origen_muestra, bool negativo, bool VPH,
                bool glandular, bool escamoza, bool adenocarcinomana, bool carcinomana_celula, bool celula_atipica ,
                bool lesion_escamosa_AltoGrado, bool lesion_escamosa_BajoGrado, string fechaInforme )
                */
            }
        }

        void WebS_InsertarCitologiaGinecologicaCompleted(object sender, ServiceReferenceClinica.InsertarCitologiaGinecologicaCompletedEventArgs e)
        {
            bool Response;
            Response = e.Result;

            enableButtons(true);
            if (Response == false)
            {
                if (errorCitologia)
                {
                    errorCitologia = false;
                    MessageBox.Show("Se detectóun error!  Ya existe una muestra con el mismo numero");
                }
                estado.Text = "Error al insertar Citologia";
            }
            else
            {
                resetCampos();
                estado.Text = "Citología insertada con exito!";
            }
        }

        void myWebReference_buscarCitologiaCompleted(object sender, ServiceReferenceClinica.buscarCitologiaCompletedEventArgs e)
        {
            if (masterFlag)
            {
                masterFlag = false;
                enableButtons(true);
                if (e.Result.ToString() == "")
                {
                    estado.Text = "No se encontró ninguna muestra con el ID " + idMuestra_txt.Text;
                }
                else
                {
                    resetCampos();
                    estado.Text = "Datos Obtenidos!";
                    String[] resultado;
                    resultado = e.Result.ToString().Split('$');

                    //Empieza a llenar los campos
                    idMuestra_txt.Text = resultado[0];
                    fechaMuestra.Text = resultado[1];
                    primerNombrePaciente_txt.Text = resultado[2];
                    segundoNombrePaciente_txt.Text = resultado[3];
                    primerApellidoPaciente_txt.Text = resultado[4];
                    segundoApellidoPaciente_txt.Text = resultado[5];
                    nombreMedico.SelectedItem = resultado[6];
                    edad_txt.Text = resultado[7];
                    switch (resultado[8])
                    {
                        case "Cervix":
                            origenMuestraComboBox.SelectedIndex = 0;
                            break;
                        case "Vagina":
                            origenMuestraComboBox.SelectedIndex = 1;
                            break;
                        case "Cupula":
                            origenMuestraComboBox.SelectedIndex = 2;
                            break;
                    }
                    FUP_txt.Text = resultado[9];
                    FUR_txt.Text = resultado[10];
                    valor_txt.Text = resultado[11];
                    if (resultado[12] == "True")
                        DIU.IsChecked = true;
                    if (resultado[13] == "True")
                        Anticonceptivos.IsChecked = true;
                    expediente_txt.Text = resultado[14];
                    diagnosticoClinico_txt.Text = resultado[15];

                    //Revisa calidad del frotis
                    if (resultado[16] == "True")
                        calidadFrotis_combo.SelectedIndex = 0;
                    else
                        calidadFrotis_combo.SelectedIndex = 1;
                    cfCausa_txt.Text = resultado[17];
                    basales_txt.Text = resultado[18];
                    intermedias_txt.Text = resultado[19];
                    superficiales_txt.Text = resultado[20];
                    switch (resultado[21])
                    {
                        case "No":
                            inflamacion_combo.SelectedIndex = 0;
                            break;
                        case "Leve":
                            inflamacion_combo.SelectedIndex = 1;
                            break;
                        case "Moderada":
                            inflamacion_combo.SelectedIndex = 2;
                            break;
                        case "Severa":
                            inflamacion_combo.SelectedIndex = 3;
                            break;
                    }
                    //Agentes Infecciosos
                    if (resultado[22] == "True")
                        candidaSP_check.IsChecked = true;
                    if (resultado[23] == "True")
                        gardnerella_check.IsChecked = true;
                    if (resultado[24] == "True")
                        herpes_check.IsChecked = true;
                    if (resultado[25] == "True")
                        vaginosis_check.IsChecked = true;
                    if (resultado[26] == "True")
                        tricomonas_txt.IsChecked = true;
                    otro_txt.Text = resultado[27];

                    //Diagnostico Descriptivo
                    if (resultado[28] == "True")
                        negativo_check.IsChecked = true;
                    if (resultado[29] == "True")
                        celulasAtipicas_check.IsChecked = true;
                    if (resultado[30] == "True")
                        Escamosa_check.IsChecked = true;
                    if (resultado[31] == "True")
                        glandular_check.IsChecked = true;
                    if (resultado[32] == "True")
                        lesionBajoGrado_check.IsChecked = true;
                    if (resultado[33] == "True")
                        nic1_check.IsChecked = true;
                    if (resultado[34] == "True")
                        infeccionVph_check.IsChecked = true;
                    if (resultado[35] == "True")
                        lesionAltoGrado_check.IsChecked = true;
                    if (resultado[36] == "True")
                        nic2_check.IsChecked = true;
                    if (resultado[37] == "True")
                        nic3_check.IsChecked = true;
                    if (resultado[38] == "True")
                        carcinoma_check.IsChecked = true;
                    if (resultado[39] == "True")
                        adenocarcinoma_check.IsChecked = true;

                    //Recomendaciones
                    repetirMeses_txt.Text = resultado[40];
                    if (resultado[41] == "True")
                        colposcopia_check.IsChecked = true;
                    if (resultado[42] == "True")
                        biopsia_check.IsChecked = true;
                    if (resultado[43] == "True")
                        dspsTratamiento_check.IsChecked = true;
                    otraRecomendacion_txt.Text = resultado[44];

                    if (resultado[45].StartsWith("<"))
                        comentario_txtR.Xaml = resultado[45];
                    else
                        comentario_txtR.Xaml = crearXamlString(resultado[45]);

                    if (resultado[46].StartsWith("<"))
                        diagnostico_richtext.Xaml = resultado[46];
                    else
                        diagnostico_richtext.Xaml = crearXamlString(resultado[46]);
                }
            }
        }

        void myWebReference_BorrarMuestraCompleted(object sender, ServiceReferenceClinica.BorrarMuestraCompletedEventArgs e)
        {
            if (masterFlag)
            {
                masterFlag = false;
                enableButtons(true);
                string idCit = idMuestra_txt.Text;

                resetCampos();
                if (e.Result)
                {
                    idMuestra_txt.Text = "";
                    estado.Text = "Se ha borrado la Citología exitosamente!";
                }
                else
                    estado.Text = "Error al borrar la Citologia, no existe la muestra " + idCit + "en la base de datos";
            }
        }

        public void myWebReference_ActualizarCitologiaCompleted(object sender, ServiceReferenceClinica.ActualizarCitologiaCompletedEventArgs e)
        {
            if (masterFlag)
            {
                masterFlag = false;
                bool Response;
                Response = e.Result;

                enableButtons(true);

                if (Response == false)
                {
                    estado.Text = "Error al actualizar Citologia";
                }
                else
                {
                    estado.Text = "Citología actualizada con exito!";
                }
            }
        }

        /********************************************************************************************************************************
        *********************************************************************************************************************************
        *********************************************                           *********************************************************
        *********************************************    OTHER ITEM EVENTS      *********************************************************
        *********************************************                           *********************************************************
        *********************************************************************************************************************************
        *********************************************************************************************************************************/

        /****************
         *  LOST FOCUS  *
         ****************/
        private void idMuestra_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            idMuestra_txt2.Text = idMuestra_txt.Text;
            idMuestra_txt3.Text = idMuestra_txt.Text;
            idMuestra_txt4.Text = idMuestra_txt.Text;
        }

        private void edad_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            edad_txt.Text.Trim();
            if (numeros.IsMatch(edad_txt.Text))
            {
                edadPaciente = int.Parse(edad_txt.Text);
                enableButtons(true);
            }
            else
            {
                enableButtons(false);
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                edad_error.Source = new BitmapImage(imgURI);
            }
        }

        private void precio_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            valor_txt.Text.Trim();
            if (numeros.IsMatch(valor_txt.Text))
            {
                precioCitologia = int.Parse(valor_txt.Text);
            }
            else
            {
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                valor_error.Source = new BitmapImage(imgURI);
                valor_txt.Text = "";
            }
        }

        private void basales_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            basales_txt.Text.Trim();
            if (numeros.IsMatch(basales_txt.Text))
            {
                basales = int.Parse(basales_txt.Text);
            }
            else
            {
                basales_txt.Text = "";
            }
        }

        private void intermedias_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            intermedias_txt.Text.Trim();
            if (numeros.IsMatch(intermedias_txt.Text))
            {
                intermedias = int.Parse(intermedias_txt.Text);
            }
            else
            {
                intermedias_txt.Text = "";
            }
        }

        private void superficiales_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            superficiales_txt.Text.Trim();
            if (numeros.IsMatch(superficiales_txt.Text))
            {
                superficiales = int.Parse(superficiales_txt.Text);
            }
            else
            {
                superficiales_txt.Text = "";
            }
        }

        private void primerNombrePaciente_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            primerNombre = primerNombrePaciente_txt.Text;
            updateNombrePaciente(primerNombre, segundoNombre, primerApellido, segundoApellido);
        }

        private void segundoNombrePaciente_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            segundoNombre = segundoNombrePaciente_txt.Text;
            updateNombrePaciente(primerNombre, segundoNombre, primerApellido, segundoApellido);
        }

        private void primerApellidoPaciente_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            primerApellido = primerApellidoPaciente_txt.Text;
            updateNombrePaciente(primerNombre, segundoNombre, primerApellido, segundoApellido);
        }

        /***********************
         *  SELECTION CHANGED  *
         ***********************/

        private void calidadFrotis_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calidadFrotis_combo.SelectedIndex == 0)
                calidadFrotis = true;
            else if (calidadFrotis_combo.SelectedIndex == 1)
                calidadFrotis = false;
        }

        private void inflamacion_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (inflamacion_combo.SelectedIndex)
            {
                case 0:
                    inflamacion = "No";
                    break;
                case 1:
                    inflamacion = "Leve";
                    break;
                case 2:
                    inflamacion = "Moderada";
                    break;
                case 3:
                    inflamacion = "Severa";
                    break;
            }

        }

        private void origenMuestraComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (origenMuestraComboBox.SelectedIndex)
            {
                case 0:
                    origenMuestra = "Cervix";
                    break;
                case 1:
                    origenMuestra = "Vagina";
                    break;
                case 2:
                    origenMuestra = "Cupula";
                    break;
            }
        }

        private void nombreMedico_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            medico_error.Source = MainPage.bmpClear;
            try
            {
                nombreMedicoString = nombreMedico.SelectedItem.ToString();
                nombreMedico_txt2.Text = nombreMedicoString;
                nombreMedico_txt3.Text = nombreMedicoString;
                nombreMedico_txt4.Text = nombreMedicoString;
            }
            catch (Exception exception)
            {

            }
        }


        private void repetirMeses_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            repetirMeses_txt.Text.Trim();
            if (numeros.IsMatch(repetirMeses_txt.Text))
            {
                repetirMeses = int.Parse(repetirMeses_txt.Text);
            }
            else
            {
                repetirMeses_txt.Text = "";
            }
        }

        private void fechaMuestra_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            fecha_error.Source = MainPage.bmpClear;
            fecha = fechaMuestra.Text.Split('/');
        }

        private void segundoApellidoPaciente_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            segundoApellido = segundoApellidoPaciente_txt.Text;
            updateNombrePaciente(primerNombre, segundoNombre, primerApellido, segundoApellido);
        }

        /********************************************************************************************************************************
        *********************************************************************************************************************************
        *********************************************                           *********************************************************
        *********************************************     CHECKBOX  EVENTS      *********************************************************
        *********************************************                           *********************************************************
        *********************************************************************************************************************************
        *********************************************************************************************************************************/

        private void DIU_Checked(object sender, RoutedEventArgs e)
        {
            diu = true;
        }
        private void DIU_Unchecked(object sender, RoutedEventArgs e)
        {
            diu = false;
        }

        private void Anticonceptivos_Checked(object sender, RoutedEventArgs e)
        {
            anticonceptivos = true;
        }
        private void Anticonceptivos_Unchecked(object sender, RoutedEventArgs e)
        {
            anticonceptivos = false;
        }

        private void candidaSP_check_Checked(object sender, RoutedEventArgs e)
        {
            candidaSp = true;
        }
        private void candidaSP_check_Unchecked(object sender, RoutedEventArgs e)
        {
            candidaSp = false;
        }

        private void gardnerella_check_Checked(object sender, RoutedEventArgs e)
        {
            gardnerella = true;
        }
        private void gardnerella_check_Unchecked(object sender, RoutedEventArgs e)
        {
            gardnerella = false;
        }

        private void herpes_check_Checked(object sender, RoutedEventArgs e)
        {
            herpes = true;
        }
        private void herpes_check_Unchecked(object sender, RoutedEventArgs e)
        {
            herpes = false;
        }

        private void vaginosis_check_Checked(object sender, RoutedEventArgs e)
        {
            vaginosis = true;
        }
        private void vaginosis_check_Unchecked(object sender, RoutedEventArgs e)
        {
            vaginosis = false;
        }

        private void tricomonas_txt_Checked(object sender, RoutedEventArgs e)
        {
            triconomas = true;
        }
        private void tricomonas_txt_Unchecked(object sender, RoutedEventArgs e)
        {
            triconomas = false;
        }

        /****************************************************************************************
         * Diagnostico Descriptivo
         * **************************************************************************************/

        private void Escamosa_check_Checked(object sender, RoutedEventArgs e)
        {
            escamosa = true;
            diagnosticoDescriptivo[2] = "<Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" " +
                    "FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"ESCAMOSA (ASCUS)\" /></Paragraph>";
            escribirDiagnostico();
        }
        private void Escamosa_check_Unchecked(object sender, RoutedEventArgs e)
        {
            escamosa = false;
            diagnosticoDescriptivo[2] = " ";
            escribirDiagnostico();
        }

        private void glandular_check_Checked(object sender, RoutedEventArgs e)
        {
            glandular = true;
            diagnosticoDescriptivo[3] = "<Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" " +
                    "FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"GLANDULAR (AGUS)\" /></Paragraph>";
            escribirDiagnostico();
        }
        private void glandular_check_Unchecked(object sender, RoutedEventArgs e)
        {
            glandular = false;
            diagnosticoDescriptivo[3] = " ";
            escribirDiagnostico();
        }

        private void nic1_check_Checked(object sender, RoutedEventArgs e)
        {
            nic1 = true;
            diagnosticoDescriptivo[5] = "<Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" " +
                    "FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"NIC I (DISPLASLA LEVE)\" /></Paragraph>";
            escribirDiagnostico();
        }
        private void nic1_check_Unchecked(object sender, RoutedEventArgs e)
        {
            nic1 = false;
            diagnosticoDescriptivo[5] = " ";
            escribirDiagnostico();
        }

        private void infeccionVph_check_Checked(object sender, RoutedEventArgs e)
        {
            infeccionVPH = true;
            diagnosticoDescriptivo[6] = "<Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" " +
                    "FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"INFECCION VPH\" /></Paragraph>";
            negativo_check.IsEnabled = false;
            celulasAtipicas_check.IsEnabled = false;
            carcinoma_check.IsEnabled = false;
            adenocarcinoma_check.IsEnabled = false;
            escribirDiagnostico();

        }
        private void infeccionVph_check_Unchecked(object sender, RoutedEventArgs e)
        {
            infeccionVPH = false;
            diagnosticoDescriptivo[6] = "";

            if (!lesionAltoGrado && !lesionBajoGrado)
            {
                negativo_check.IsEnabled = true;
                celulasAtipicas_check.IsEnabled = true;
                carcinoma_check.IsEnabled = true;
                adenocarcinoma_check.IsEnabled = true;
                escribirDiagnostico();
            }
        }

        private void nic2_check_Checked(object sender, RoutedEventArgs e)
        {
            nic2 = true;
            diagnosticoDescriptivo[8] = "<Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" " +
                    "FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"NIC II\" /></Paragraph>";
            escribirDiagnostico();
        }
        private void nic2_check_Unchecked(object sender, RoutedEventArgs e)
        {
            nic2 = false;
            diagnosticoDescriptivo[8] = "";
            escribirDiagnostico();
        }

        private void nic3_check_Checked(object sender, RoutedEventArgs e)
        {
            nic3 = true;
            diagnosticoDescriptivo[9] = "<Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" " +
                    "FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"NIC III (CARCINOMA IN SITU)\" /></Paragraph>";            
            escribirDiagnostico();
        }
        private void nic3_check_Unchecked(object sender, RoutedEventArgs e)
        {
            nic3 = false;
            diagnosticoDescriptivo[9] = "";
            escribirDiagnostico();
        }

        private void negativo_check_Checked(object sender, RoutedEventArgs e)
        {
            negativo = true;
            diagnosticoDescriptivo[0] = "<Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" " +
                    "FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"NEGATIVO POR MALIGNIDAD\" /></Paragraph>";
            celulasAtipicas_check.IsEnabled = false;
            lesionAltoGrado_check.IsEnabled = false;
            lesionBajoGrado_check.IsEnabled = false;
            carcinoma_check.IsEnabled = false;
            adenocarcinoma_check.IsEnabled = false;
            infeccionVph_check.IsEnabled = false;

            escribirDiagnostico();
        }
        private void negativo_check_Unchecked(object sender, RoutedEventArgs e)
        {
            negativo = false;
            diagnosticoDescriptivo[0] = "";

            celulasAtipicas_check.IsEnabled = true;
            lesionAltoGrado_check.IsEnabled = true;
            lesionBajoGrado_check.IsEnabled = true;
            carcinoma_check.IsEnabled = true;
            adenocarcinoma_check.IsEnabled = true;
            infeccionVph_check.IsEnabled = true;
            escribirDiagnostico();
        }

        private void celulasAtipicas_check_Checked(object sender, RoutedEventArgs e)
        {
            celulasAtipicas = true;
            negativo_check.IsEnabled = false;
            diagnosticoDescriptivo[1] = "<Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" " +
                    "FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"CELULAS ATIPICAS DE SIGNIFICADO INDETERMINADO\" /></Paragraph>";

            lesionAltoGrado_check.IsEnabled = false;
            lesionBajoGrado_check.IsEnabled = false;
            Escamosa_check.IsEnabled = true;
            glandular_check.IsEnabled = true;
            carcinoma_check.IsEnabled = false;
            adenocarcinoma_check.IsEnabled = false;
            infeccionVph_check.IsEnabled = false;
            escribirDiagnostico();
        }
        private void celulasAtipicas_check_Unchecked(object sender, RoutedEventArgs e)
        {
            celulasAtipicas = false;
            escamosa = false;
            glandular = false;
            diagnosticoDescriptivo[1] = "";

            negativo_check.IsEnabled = true;
            lesionAltoGrado_check.IsEnabled = true;
            lesionBajoGrado_check.IsEnabled = true;
            Escamosa_check.IsEnabled = false;
            glandular_check.IsEnabled = false;
            carcinoma_check.IsEnabled = true;
            adenocarcinoma_check.IsEnabled = true;
            Escamosa_check.IsChecked = false;
            glandular_check.IsChecked = false;
            infeccionVph_check.IsEnabled = true;
            escribirDiagnostico();
        }

        private void lesionBajoGrado_check_Checked(object sender, RoutedEventArgs e)
        {
            diagnosticoDescriptivo[4] = "<Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" " +
                    "FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"LESION ESCAMOSA INTRAEPITELIAL DE BAJO GRADO\" /></Paragraph>";
            lesionBajoGrado = true;
            nic1_check.IsEnabled = true;
            negativo_check.IsEnabled = false;
            celulasAtipicas_check.IsEnabled = false;
            lesionAltoGrado_check.IsEnabled = false;
            carcinoma_check.IsEnabled = false;
            adenocarcinoma_check.IsEnabled = false;
            nic1_check.IsChecked = false;
            escribirDiagnostico();
        }
        private void lesionBajoGrado_check_Unchecked(object sender, RoutedEventArgs e)
        {
            diagnosticoDescriptivo[4] = "";
            lesionBajoGrado = false;
            nic1 = false;
            nic1_check.IsChecked = false;
            nic1_check.IsEnabled = false;

            infeccionVph_check.IsChecked = false;
            negativo_check.IsEnabled = true;
            celulasAtipicas_check.IsEnabled = true;
            lesionAltoGrado_check.IsEnabled = true;
            carcinoma_check.IsEnabled = true;
            adenocarcinoma_check.IsEnabled = true;
            escribirDiagnostico();
        }

        private void lesionAltoGrado_check_Checked(object sender, RoutedEventArgs e)
        {
            diagnosticoDescriptivo[7] = "<Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" " +
                    "FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"LESION ESCAMOSA INTRAEPITELIAL DE ALTO GRADO\" /></Paragraph>";
            lesionAltoGrado = true;

            negativo_check.IsEnabled = false;
            celulasAtipicas_check.IsEnabled = false;
            lesionBajoGrado_check.IsEnabled = false;
            nic2_check.IsEnabled = true;
            nic3_check.IsEnabled = true;
            carcinoma_check.IsEnabled = false;
            adenocarcinoma_check.IsEnabled = false;
            escribirDiagnostico();
        }
        private void lesionAltoGrado_check_Unchecked(object sender, RoutedEventArgs e)
        {
            diagnosticoDescriptivo[7] = "";
            lesionAltoGrado = false;
            nic2 = false;
            nic3 = false;

            infeccionVph_check.IsChecked = false;
            nic2_check.IsChecked = false;
            nic3_check.IsChecked = false;
            negativo_check.IsEnabled = true;
            celulasAtipicas_check.IsEnabled = true;
            lesionBajoGrado_check.IsEnabled = true;
            nic2_check.IsEnabled = false;
            nic3_check.IsEnabled = false;
            carcinoma_check.IsEnabled = true;
            adenocarcinoma_check.IsEnabled = true;
            escribirDiagnostico();
        }

        private void carcinoma_check_Checked(object sender, RoutedEventArgs e)
        {
            diagnosticoDescriptivo[10] = "<Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" " +
                    "FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"CARCINOMA DE CELULAS ESCAMOSAS INVASOR\" /></Paragraph>";
            carcinoma = true;
            negativo_check.IsEnabled = false;
            celulasAtipicas_check.IsEnabled = false;
            lesionAltoGrado_check.IsEnabled = false;
            lesionBajoGrado_check.IsEnabled = false;
            infeccionVph_check.IsEnabled = false;
            adenocarcinoma_check.IsEnabled = false;
            escribirDiagnostico();
        }
        private void carcinoma_check_Unchecked(object sender, RoutedEventArgs e)
        {
            diagnosticoDescriptivo[10] = "";
            carcinoma = false;
            negativo_check.IsEnabled = true;
            celulasAtipicas_check.IsEnabled = true;
            lesionAltoGrado_check.IsEnabled = true;
            lesionBajoGrado_check.IsEnabled = true;
            adenocarcinoma_check.IsEnabled = true;
            infeccionVph_check.IsEnabled = true;
            escribirDiagnostico();
        }

        private void adenocarcinoma_check_Checked(object sender, RoutedEventArgs e)
        {
            diagnosticoDescriptivo[11] = "<Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" " +
                    "FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"ADENOCARCINOMA\" /></Paragraph>";
            adenocarcinoma = true;
            negativo_check.IsEnabled = false;
            celulasAtipicas_check.IsEnabled = false;
            lesionAltoGrado_check.IsEnabled = false;
            lesionBajoGrado_check.IsEnabled = false;
            carcinoma_check.IsEnabled = false;
            infeccionVph_check.IsEnabled = false;
            escribirDiagnostico();
        }
        private void adenocarcinoma_check_Unchecked(object sender, RoutedEventArgs e)
        {
            diagnosticoDescriptivo[11] = "";
            adenocarcinoma = false;
            negativo_check.IsEnabled = true;
            celulasAtipicas_check.IsEnabled = true;
            lesionAltoGrado_check.IsEnabled = true;
            lesionBajoGrado_check.IsEnabled = true;
            carcinoma_check.IsEnabled = true;
            infeccionVph_check.IsEnabled = true;
            escribirDiagnostico();
        }

        /**********************************************************************************
         * DIAGNOSTICO
         * *******************************************************************************/

        private void colposcopia_check_Checked(object sender, RoutedEventArgs e)
        {
            colposcopia = true;
        }
        private void colposcopia_check_Unchecked(object sender, RoutedEventArgs e)
        {
            colposcopia = false;
        }

        private void biopsia_check_Checked(object sender, RoutedEventArgs e)
        {
            repetirBiopsia = true;
        }
        private void biopsia_check_Unchecked(object sender, RoutedEventArgs e)
        {
            repetirBiopsia = false;

        }

        private void dspsTratamiento_check_Checked(object sender, RoutedEventArgs e)
        {
            dspsTratamiento = true;
        }
        private void dspsTratamiento_check_Unchecked(object sender, RoutedEventArgs e)
        {
            dspsTratamiento = false;
        }

        /********************************************************************************************************************************
        *********************************************************************************************************************************
        *********************************************                           *********************************************************
        *********************************************     UTILITY FUNCTIONS     *********************************************************
        *********************************************                           *********************************************************
        *********************************************************************************************************************************
        *********************************************************************************************************************************/

        private string getFechaEnFormatoMysql(String fecha)
        {
            if (fecha.CompareTo("") == 0)
                return "";
            String[] split = fecha.Split('/');
            return split[2] + "-" + split[0] + "-" + split[1];
        }

        void traducirFecha(String[] fecha)
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

        private String crearXamlString(String text)
        {
            String xaml = "<Section xml:space=\"preserve\" HasTrailingParagraphBreakOnPaste=\"False\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"";
            xaml += text;
            xaml += "\" /></Paragraph></Section>";
            return xaml;
        }

        private void resetCampos()
        {
            primerNombrePaciente_txt.Text = "";
            segundoNombrePaciente_txt.Text = "";
            primerApellidoPaciente_txt.Text = "";
            segundoApellidoPaciente_txt.Text = "";
            fechaMuestra.Text = "";
            nombreMedico.SelectedIndex = -1;
            FUP_txt.Text = "";
            FUR_txt.Text = "";
            edad_txt.Text = "";
            origenMuestraComboBox.SelectedIndex = -1;
            valor_txt.Text = "";
            DIU.IsChecked = false;
            Anticonceptivos.IsChecked = false;
            diagnosticoClinico_txt.Text = "";
            calidadFrotis_combo.SelectedIndex = -1;
            cfCausa_txt.Text = "";
            basales_txt.Text = "";
            intermedias_txt.Text = "";
            superficiales_txt.Text = "";
            inflamacion_combo.SelectedIndex = -1;
            candidaSP_check.IsChecked = false;
            gardnerella_check.IsChecked = false;
            herpes_check.IsChecked = false;
            vaginosis_check.IsChecked = false;
            tricomonas_txt.IsChecked = false;
            otro_txt.Text = "";
            negativo_check.IsChecked = false;
            celulasAtipicas_check.IsChecked = false;
            lesionAltoGrado_check.IsChecked = false;
            lesionBajoGrado_check.IsChecked = false;
            carcinoma_check.IsChecked = false;
            adenocarcinoma_check.IsChecked = false;
            Escamosa_check.IsChecked = false;
            glandular_check.IsChecked = false;
            nic1_check.IsChecked = false;
            infeccionVph_check.IsChecked = false;
            nic2_check.IsChecked = false;
            nic3_check.IsChecked = false;
            repetirMeses_txt.Text = "";
            colposcopia_check.IsChecked = false;
            biopsia_check.IsChecked = false;
            dspsTratamiento_check.IsChecked = false;
            otraRecomendacion_txt.Text = "";

            comentario_txtR.SelectAll();
            comentario_txtR.Selection.Text = "";
            diagnostico_richtext.SelectAll();
            diagnostico_richtext.Selection.Text = "";
        }

        private void enableButtons(bool value)
        {
            imprimir_btn.IsEnabled = value;
            guardar_btn.IsEnabled = value;
            buscarBtn.IsEnabled = value;
            guardarBtn.IsEnabled = value;
            borrarBtn.IsEnabled = value;
            resetFields_btn.IsEnabled = value;

            if (value)
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

        void updateNombrePaciente(string pNombre, string sNombre, string pApellido, string sApellido)
        {
            nombrePaciente_txt2.Text = pNombre + " " + sNombre + " " + pApellido + " " + sApellido;
            nombrePaciente_txt3.Text = pNombre + " " + sNombre + " " + pApellido + " " + sApellido;
            nombrePaciente_txt4.Text = pNombre + " " + sNombre + " " + pApellido + " " + sApellido;
        }

        private void idMuestra_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            id_error.Source = MainPage.bmpClear;
        }

        private void valor_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            valor_error.Source = MainPage.bmpClear;
        }

        private void primerNombrePaciente_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            nombre_error.Source = MainPage.bmpClear;
        }

        private void primerApellidoPaciente_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            apellido_error.Source = MainPage.bmpClear;
        }

        private void edad_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            edad_error.Source = MainPage.bmpClear;
            enableButtons(true);
        }

        private void escribirDiagnostico()
        {
            String diagnosticoFinal = "<Section xml:space=\"preserve\" HasTrailingParagraphBreakOnPaste=\"False\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">";
            for (int i = 0; i < this.diagnosticoDescriptivo.Length; i++)
            {
                diagnosticoFinal += this.diagnosticoDescriptivo[i];
            }
            diagnosticoFinal += "</Section>";

            if (!diagnosticoFinal.Equals(""))
                diagnostico_richtext.Xaml = diagnosticoFinal;
            else
            {
                diagnostico_richtext.SelectAll();
                diagnostico_richtext.Selection.Text = "";
            }
        }

        private String corregirIdMuestra(String id)
        {
            String idFinal = id;
            string[] test = id.Split('-');

            if (!idFinal.Equals(""))
            {
                int idInt;
                try
                {
                    if (test[0].StartsWith("C"))
                        idInt = Convert.ToInt32(test[0].Substring(1));
                    else
                        idInt = Convert.ToInt32(test[0]);
                }
                catch (Exception exp)
                {
                    idInt = 100;
                }


                if (test[0].StartsWith("C"))
                {
                    idFinal = test[0];
                }
                else
                {
                    if (idInt < 10)
                        test[0] = "00" + test[0];
                    else if (idInt < 100)
                        test[0] = "0" + test[0];
                    idFinal = "C" + test[0];
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

        private void resetFieldValues()
        {
            primerNombre = "";
            primerApellido = "";
            segundoNombre = "";
            segundoApellido = "";

        }

        private void resetFields_btn_Click(object sender, RoutedEventArgs e)
        {
            idMuestra_txt.Text = "";
            resetCampos();
        }

        private void ReturnFocus(RichTextBox richtext)
        {
            if (richtext != null)
                richtext.Focus();
        }


        #region Bold, Italics & Underline
        //Set Bold formatting to the selected content 
        private void btnBold_Click(object sender, RoutedEventArgs e)
        {
            if (diagnostico_richtext != null && diagnostico_richtext.Selection.Text.Length > 0)
            {
                if (diagnostico_richtext.Selection.GetPropertyValue(Run.FontWeightProperty) is FontWeight && ((FontWeight)diagnostico_richtext.Selection.GetPropertyValue(Run.FontWeightProperty)) == FontWeights.Normal)
                    diagnostico_richtext.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Bold);
                else
                    diagnostico_richtext.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Normal);
                ReturnFocus(diagnostico_richtext);
            }
            else if (comentario_txtR != null && comentario_txtR.Selection.Text.Length > 0)
            {
                if (comentario_txtR.Selection.GetPropertyValue(Run.FontWeightProperty) is FontWeight && ((FontWeight)comentario_txtR.Selection.GetPropertyValue(Run.FontWeightProperty)) == FontWeights.Normal)
                    comentario_txtR.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Bold);
                else
                    comentario_txtR.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Normal);
                ReturnFocus(comentario_txtR);
            }
        }

        //Set Italic formatting to the selected content 
        private void btnItalic_Click(object sender, RoutedEventArgs e)
        {
            if (diagnostico_richtext != null && diagnostico_richtext.Selection.Text.Length > 0)
            {
                if (diagnostico_richtext.Selection.GetPropertyValue(Run.FontStyleProperty) is FontStyle && ((FontStyle)diagnostico_richtext.Selection.GetPropertyValue(Run.FontStyleProperty)) == FontStyles.Normal)
                    diagnostico_richtext.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Italic);
                else
                    diagnostico_richtext.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Normal);
                ReturnFocus(diagnostico_richtext);
            }
            else if (comentario_txtR != null && comentario_txtR.Selection.Text.Length > 0)
            {
                if (comentario_txtR.Selection.GetPropertyValue(Run.FontStyleProperty) is FontStyle && ((FontStyle)comentario_txtR.Selection.GetPropertyValue(Run.FontStyleProperty)) == FontStyles.Normal)
                    comentario_txtR.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Italic);
                else
                    comentario_txtR.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Normal);
                ReturnFocus(comentario_txtR);
            }
        }

        //Set Underline formatting to the selected content 
        private void btnUnderline_Click(object sender, RoutedEventArgs e)
        {
            if (diagnostico_richtext != null && diagnostico_richtext.Selection.Text.Length > 0)
            {
                if (diagnostico_richtext.Selection.GetPropertyValue(Run.TextDecorationsProperty) == null)
                    diagnostico_richtext.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Underline);
                else
                    diagnostico_richtext.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, null);
                ReturnFocus(diagnostico_richtext);
            }
            else if (comentario_txtR != null && comentario_txtR.Selection.Text.Length > 0)
            {
                if (comentario_txtR.Selection.GetPropertyValue(Run.TextDecorationsProperty) == null)
                    comentario_txtR.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Underline);
                else
                    comentario_txtR.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, null);
                ReturnFocus(comentario_txtR);
            }
        }
        #endregion

        #region Font Type, Color & size

        //Set font type to selected content
        private void cmbFonts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (diagnostico_richtext != null && diagnostico_richtext.Selection.Text.Length > 0)
            {
                diagnostico_richtext.Selection.ApplyPropertyValue(Run.FontFamilyProperty, new FontFamily((cmbFonts.SelectedItem as ComboBoxItem).Tag.ToString()));
                ReturnFocus(diagnostico_richtext);
            }
            else if (comentario_txtR != null && comentario_txtR.Selection.Text.Length > 0)
            {
                comentario_txtR.Selection.ApplyPropertyValue(Run.FontFamilyProperty, new FontFamily((cmbFonts.SelectedItem as ComboBoxItem).Tag.ToString()));
                ReturnFocus(comentario_txtR);
            }
        }

        //Set font size to selected content
        private void cmbFontSizes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (diagnostico_richtext != null && diagnostico_richtext.Selection.Text.Length > 0)
            {
                diagnostico_richtext.Selection.ApplyPropertyValue(Run.FontSizeProperty, double.Parse((cmbFontSizes.SelectedItem as ComboBoxItem).Tag.ToString()));
                ReturnFocus(diagnostico_richtext);
            }
            else if (comentario_txtR != null && comentario_txtR.Selection.Text.Length > 0)
            {
                comentario_txtR.Selection.ApplyPropertyValue(Run.FontSizeProperty, double.Parse((cmbFontSizes.SelectedItem as ComboBoxItem).Tag.ToString()));
                ReturnFocus(comentario_txtR);
            }
        }

        //Set font color to selected content
        private void cmbFontColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (diagnostico_richtext != null && diagnostico_richtext.Selection.Text.Length > 0)
            {
                string color = (cmbFontColors.SelectedItem as ComboBoxItem).Tag.ToString();

                SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(
                    byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)));

                diagnostico_richtext.Selection.ApplyPropertyValue(Run.ForegroundProperty, brush);
                ReturnFocus(diagnostico_richtext);
            }
            else if (comentario_txtR != null && comentario_txtR.Selection.Text.Length > 0)
            {
                string color = (cmbFontColors.SelectedItem as ComboBoxItem).Tag.ToString();

                SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(
                    byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)));

                comentario_txtR.Selection.ApplyPropertyValue(Run.ForegroundProperty, brush);
                ReturnFocus(comentario_txtR);
            }
        }

        #endregion
    }
}
