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
using System.Windows.Printing;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace Sistema_BD_Clinica_Patologica.Views
{
    public partial class Formulario : Page
    {
        /******************************************
         * ****************************************
         *          PRINTER VARIABLES             *
         * ****************************************
         * ****************************************/
        PrintPreview cw;
        PrintPreview2 cw2;
        PrintPreview3 cw3;

        DispatcherTimer timer;

        PrintDocument page1;
        PrintDocument page2;
        PrintDocument page3;
        PrintDocument page4;

        Rectangle firstBottomRect;

        //Este es el rectangulo para cubrir el texto de arriba del margen en la segunda pagina en adelante
        Rectangle rect;
        //Y este es para cubrir el texto en el margen de abajo de la pagina 2 en adelante...
        Rectangle rectBottom;

        //Este es el grid principal para imprimir 2 paginas en adelante
        Grid grid;
        TextBlock idExamen;
        TextBlock nombrePaciente;
        TextBlock fechaBio;
        RichTextBox rtb;

        //Variables de control
        double formH;

        int counter = 0;
        int pagesToPrint = 0;
        double diagBoxHeight = 0;
        bool isMorePages = false; //Flag si es de 2 paginas
        bool isMorePages2 = false;// Si es de 3 paginas
        bool isMorePages3 = false;// Si es de 4 paginas....no puede haber formularios mas largos que esto   

        /******************************************
         * ****************************************
         *          END PRINTER VARIABLES         *
         * ****************************************
         * ****************************************/

        String idMuestra;
        String Usuario = App.Correo;

        //flags
        bool mainFlag = false;
        bool getIdExamenes = false;
        bool getCitologia = false;
        bool getBiopsia = false;
        bool getCitologiaLiquidos = false;
        bool materiales = false;

        string imagenCitologia = "/Sistema_BD_Clinica_Patologica;component/Images/Citologia.jpg";
        string imagenBiopsia = "/Sistema_BD_Clinica_Patologica;component/Images/Biopsia.jpg";
        Uri biopsiaURI = null;
        Uri citologiaURI = null;

        /********************************************************************************************************************************
        **************************************************    Inicializadores   *********************************************************
        *********************************************************************************************************************************/

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private Sistema_BD_Clinica_Patologica.ServiceReferenceClinica.WSClinicaSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:5633/WSClinica.asmx";

        public Formulario()
        {
            InitializeComponent();
            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReferenceClinica.WSClinicaSoapClient(bind, endpoint);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            /*****************
            /*BEGIN PRINTER VARS
            ******************/
            timer = new DispatcherTimer();

            cw = new PrintPreview(); //For page 2 confirmation
            cw2 = new PrintPreview2(); // For page 3
            cw3 = new PrintPreview3(); // Page 4

            cw.Closed += new EventHandler(userConfirmsPrintPage2);
            cw2.Closed += new EventHandler(userConfirmsPrintPage3);
            cw3.Closed += new EventHandler(userConfirmsPrintPage4);

            page1 = new PrintDocument();
            page2 = new PrintDocument();
            page3 = new PrintDocument();
            page4 = new PrintDocument();

            page1.PrintPage += new EventHandler<PrintPageEventArgs>(page1_PrintPage);
            page1.EndPrint += new EventHandler<EndPrintEventArgs>(page1_EndPrint);
            page2.PrintPage += new EventHandler<PrintPageEventArgs>(page2_PrintPage);
            page2.EndPrint += new EventHandler<EndPrintEventArgs>(page2_EndPrint);
            page3.PrintPage += new EventHandler<PrintPageEventArgs>(page3_PrintPage);
            page3.EndPrint += new EventHandler<EndPrintEventArgs>(page3_EndPrint);
            page4.PrintPage += new EventHandler<PrintPageEventArgs>(page4_PrintPage);
            page4.EndPrint += new EventHandler<EndPrintEventArgs>(page4_EndPrint);

            firstBottomRect = new Rectangle();
            //White rectangle to cover unwanted text jaja
            rect = new Rectangle();
            rectBottom = new Rectangle();
            grid = new Grid();
            idExamen = new TextBlock();
            nombrePaciente = new TextBlock();
            fechaBio = new TextBlock();
            rtb = new RichTextBox();

            formH = espacioFormulario.ActualHeight;

            grid.Children.Add(rtb);
            grid.Children.Add(rect);
            grid.Children.Add(rectBottom);
            grid.Children.Add(nombrePaciente);
            grid.Children.Add(idExamen);
            grid.Children.Add(fechaBio);

            /*****************
            /*END PRINTER VARS
            ******************/

            idMuestra = NavigationContext.QueryString["ID"];
            getIdExamenes = false;
            enableButtons(false);

            biopsiaURI = new Uri(imagenBiopsia, UriKind.Relative);
            citologiaURI = new Uri(imagenCitologia, UriKind.Relative);

            resetearValoresCitologia();

            diagnosticoBiopsia.Opacity = 0;

            Wrapper.getIdExamenesCompleted += new EventHandler<ServiceReferenceClinica.getIdExamenesCompletedEventArgs>(myWebReference_getIdExamenesCompleted);
            Wrapper.getIdExamenesAsync();
        }


        /********************************************************************************************************************************
        *********************************************************************************************************************************
        *********************************************                           *********************************************************
        *********************************************     PRINTING HANDLING EVENTS    *********************************************************
        *********************************************                           *********************************************************
        *********************************************************************************************************************************
        *********************************************************************************************************************************/

        /**************
         *   PAGE 1   *
         **************/
        void page1_PrintPage(object sender, PrintPageEventArgs e)
        {
            switch (pagesToPrint)
            {
                case 1:
                    isMorePages = false;
                    isMorePages2 = false;
                    isMorePages3 = false;
                    break;
                case 2:
                    isMorePages = true;
                    isMorePages2 = false;
                    isMorePages3 = false;
                    break;
                case 3:
                    isMorePages = true;
                    isMorePages2 = true;
                    isMorePages3 = false;
                    break;
                case 4:
                    isMorePages = true;
                    isMorePages2 = true;
                    isMorePages3 = true;
                    break;
                default:
                    isMorePages = true;
                    isMorePages2 = true;
                    isMorePages3 = true;
                    break;
            }

            e.PageVisual = espacioFormulario;
        }

        void page1_EndPrint(object sender, EndPrintEventArgs e)
        {
            if (isMorePages)// && counter == 0)
            {
                counter += 1;
                //cw.Show();
                cw2.Show();
            }
        }

        /**************
         *   PAGE 2   *
         **************/
        void userConfirmsPrintPage2(object sender, EventArgs e)
        {
            if (cw.DialogResult.Value)
            {
                //page2.Print("Formulario pagina 2");
                page3.Print("Formulario pagina 3");

                //cw.DialogResult = true;
                //cw.printFlag = false;
                //cw.Close();
            }
        }

        void page2_PrintPage(object sender, PrintPageEventArgs e)
        {
            rect.Height = 450;
            rect.Width = diagnosticoBiopsia.Width + 100;
            rect.Fill = new SolidColorBrush(Colors.White);
            rect.VerticalAlignment = VerticalAlignment.Top;

            rectBottom.Height = 80;
            rectBottom.Width = diagnosticoBiopsia.Width + 100;
            rectBottom.Fill = new SolidColorBrush(Colors.White);
            rectBottom.VerticalAlignment = VerticalAlignment.Bottom;

            idExamen.Text = idMuestra;
            idExamen.Margin = new Thickness(idMuestra_txt.Margin.Left - 80, -640, 0, 0);
            idExamen.FontSize = 18;
            idExamen.FontWeight = idMuestra_txt.FontWeight;
            idExamen.FontFamily = nombre.FontFamily;
            idExamen.Width = idMuestra_txt.Width;
            idExamen.Height = idMuestra_txt.Height;

            nombrePaciente.Text = nombre.Text;
            nombrePaciente.Margin = new Thickness(nombre.Margin.Left, -520, 0, 0);
            nombrePaciente.FontSize = 13;
            nombrePaciente.FontFamily = nombre.FontFamily;
            nombrePaciente.Width = nombre.Width;
            nombrePaciente.Height = nombre.Height;

            fechaBio.Text = fecha.Text;
            fechaBio.Margin = new Thickness(fecha.Margin.Left - 100, -520, 0, 0);
            fechaBio.FontSize = 13;
            fechaBio.FontFamily = nombre.FontFamily;
            fechaBio.Width = fecha.Width;
            fechaBio.Height = fecha.Height;

            rtb.Xaml = diagnosticoBiopsia.Xaml;
            rtb.VerticalAlignment = VerticalAlignment.Top;
            rtb.Height = diagBoxHeight;
            rtb.Width = diagnosticoBiopsia.Width;
            rtb.Margin = new Thickness(20, -30, 0, 0);
            //rtb.Margin = new Thickness(20, -85, 0, 0);
            rtb.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            rtb.BorderBrush = diagnosticoBiopsia.BorderBrush;
            e.PageVisual = grid;
        }

        void page2_EndPrint(object sender, EndPrintEventArgs e)
        {
            //Showing the page 4 printing confirm dialogue
            if (isMorePages2)
            {
                cw.Show();
            }
            else
            {
                NavigationService.Refresh();
            }
        }

        /**************
         *   PAGE 3   *
         **************/
        void userConfirmsPrintPage3(object sender, EventArgs e)
        {
            //PRINTING PAGE 2
            if (cw2.DialogResult.Value)
            {
                //page3.Print("Formulario pagina 3");
                page2.Print("Formulario pagina 2");
            }
        }

        void page3_PrintPage(object sender, PrintPageEventArgs e)
        {
            idExamen.Text = idMuestra;
            nombrePaciente.Text = nombre.Text;
            fechaBio.Text = fecha.Text;
            rtb.Xaml = diagnosticoBiopsia.Xaml;
            rtb.Margin = new Thickness(20, -510, 0, 0);
            //rtb.Margin = new Thickness(20, -50, 0, 0);
            e.PageVisual = grid;
        }

        void page3_EndPrint(object sender, EndPrintEventArgs e)
        {
            if (isMorePages3)
            {
                cw3.Show();
            }
            else
            {
                NavigationService.Refresh();
            }
        }

        /**************
         *   PAGE 4   *
         **************/
        void userConfirmsPrintPage4(object sender, EventArgs e)
        {
            //PRINTING PAGE 4
            if (cw2.DialogResult.Value)
            {
                //page3.Print("Formulario pagina 3");
                page4.Print("Formulario pagina 4");
            }
        }

        void page4_PrintPage(object sender, PrintPageEventArgs e)
        {
            idExamen.Text = idMuestra;
            nombrePaciente.Text = nombre.Text;
            fechaBio.Text = fecha.Text;
            rtb.Xaml = diagnosticoBiopsia.Xaml;
            rtb.Margin = new Thickness(20, -1030, 0, 0);
            e.PageVisual = grid;
        }

        void page4_EndPrint(object sender, EndPrintEventArgs e)
        {
            NavigationService.Refresh();
        }

        /********************************************************************************************************************************
        *********************************************************************************************************************************
        *********************************************                           *********************************************************
        *********************************************     MAIN BUTTON EVENTS    *********************************************************
        *********************************************                           *********************************************************
        *********************************************************************************************************************************
        *********************************************************************************************************************************/

        private void buscar_btn_Click(object sender, RoutedEventArgs e)
        {
            diagnosticoBiopsia.Opacity = 0;
            mainFlag = false;
            getIdExamenes = false;
            getCitologia = false;
            materiales = false;
            getBiopsia = false;
            getCitologiaLiquidos = false;

            idMuestra = "";

            if (idMuestrasCombo.SelectedItem != null)
            {
                idMuestra = idMuestrasCombo.SelectedItem.ToString();
            }
            else if (!selectedIDMuestra_txt.Text.Equals(""))
            {
                idMuestra = selectedIDMuestra_txt.Text;
            }

            resetearValoresCitologia();
            estado.Text = "Buscando Datos...";
            enableButtons(false);

            Wrapper.getMuestraGinecologicaCompleted += new EventHandler<ServiceReferenceClinica.getMuestraGinecologicaCompletedEventArgs>(myWebReference_getMuestraGinecologicaCompleted);
            Wrapper.getMuestraGinecologicaAsync(idMuestra);
        }

        //PRINT BUTTON!!
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Print the document
            isMorePages = false;
            isMorePages2 = false;

            counter = 0;
            //Variables de control
            formH = espacioFormulario.ActualHeight;
            diagBoxHeight = diagnosticoBiopsia.ActualHeight;
            pagesToPrint = (int)Math.Ceiling(diagBoxHeight / 580);

            //990 pixeles es el tamano de la pagina maximo
            //if (espacioFormulario.ActualHeight >= 985)
            if (diagnosticoBiopsia.ActualHeight > 580)
            {
                firstBottomRect.VerticalAlignment = VerticalAlignment.Top;
                firstBottomRect.Height = 90;
                firstBottomRect.Width = diagnosticoBiopsia.Width + 100;
                firstBottomRect.Fill = new SolidColorBrush(Colors.White);
                firstBottomRect.Margin = new Thickness(0, 973, 0, 0);
                try
                {
                    espacioFormulario.Children.Add(firstBottomRect);
                }
                catch (Exception) { }
            }

            page1.Print("Formulario Citologia");
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string imagenCitologia = "/Sistema_BD_Clinica_Patologica;component/Images/Citologia.jpg";
            Uri citologiaURI = new Uri(imagenCitologia, UriKind.Relative);
            fotoFormulario.Source = new BitmapImage(citologiaURI);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            string imagenBiopsia = "/Sistema_BD_Clinica_Patologica;component/Images/Biopsia.jpg";
            Uri biopsiaURI = new Uri(imagenBiopsia, UriKind.Relative);
            fotoFormulario.Source = new BitmapImage(biopsiaURI);
        }

        /********************************************************************************************************************************
        *********************************************************************************************************************************
        *********************************************                           *********************************************************
        *********************************************   METHOD RESPONCE EVENTS  *********************************************************
        *********************************************                           *********************************************************
        *********************************************************************************************************************************
        *********************************************************************************************************************************/

        void myWebReference_getIdExamenesCompleted(object sender, ServiceReferenceClinica.getIdExamenesCompletedEventArgs e)
        {
            if (!getIdExamenes)
            {
                getIdExamenes = true;
                estado.Text = "Datos Obtenidos!";

                for (int i = idMuestrasCombo.Items.Count - 1; i > -1; i--)
                    idMuestrasCombo.Items.RemoveAt(i);

                String[] resp = e.Result.ToString().Split(';');
                for (int i = 0; i < resp.Length; i++)
                {
                    idMuestrasCombo.Items.Add(resp[i]);
                }

                if (!idMuestra.Equals(""))
                {
                    estado.Text = "Cargando datos de la muestra No. :" + idMuestra;

                    if (!getCitologia)
                    {
                        getCitologia = true;
                        Wrapper.getMuestraGinecologicaCompleted += new EventHandler<ServiceReferenceClinica.getMuestraGinecologicaCompletedEventArgs>(myWebReference_getMuestraGinecologicaCompleted);
                        Wrapper.getMuestraGinecologicaAsync(idMuestra);
                    }
                }
                else
                {
                    enableButtons(true);
                    estado.Text = "Busque por No. de muestra para obtener los datos!";
                }
            }
        }


        void myWebReference_getMuestraGinecologicaCompleted(object sender, ServiceReferenceClinica.getMuestraGinecologicaCompletedEventArgs e)
        {
            if (e.Result.ToString() == "")
            {
                diagnosticoBiopsia.Opacity = 1;
                if (!getBiopsia)
                {
                    getBiopsia = true;
                    Wrapper.getMuestraBiopsiaCompleted += new EventHandler<ServiceReferenceClinica.getMuestraBiopsiaCompletedEventArgs>(myWebReference_getMuestraBiopsiaCompleted);
                    Wrapper.getMuestraBiopsiaAsync(idMuestra);
                }
            }
            else
            {
                enableButtons(true);
                fotoFormulario.Source = new BitmapImage(citologiaURI);
                estado.Text = "Datos Obtenidos!";
                String[] resultado;
                resultado = e.Result.ToString().Split('$');

                //Resetea los campos de Biopsias para q no salgan

                idBiopsia.Text = "";
                diagnosticoMedicoB.Text = "";
                medicoB.Text = "";
                material1.Text = "";
                material2.Text = "";
                material3.Text = "";
                material4.Text = "";
                material5.Text = "";
                fechaRecibida_txt.Text = "";

                //Empieza a llenar los campos
                idMuestra_txt.Text = resultado[0];
                //Esto es pq por alguna razon, la fecha llega con el tiempo, asi: 12/31/2011 12:00 AM entoncs le quitamos eso XD
                fecha.Text = convertDateToSpanishDate(resultado[1].Split(' ')[0]);
                nombre.Text = resultado[2] + " " + resultado[3] + " " + resultado[4] + " " + resultado[5];
                if (resultado[6].CompareTo("0") != 0)
                    edad.Text = resultado[6];
                medico.Text = resultado[7];
                origenMuestra.Text = resultado[8];
                fur.Text = resultado[9];
                if (resultado[10] != "False")
                    diu.Text = "Si";
                //Revisa para anticonceptivos
                if (resultado[11] == "True")
                    anticonceptivos.Text = "Si";
                diagnosticoAnterior.Text = resultado[12];
                //Revisa calidad del frotis
                if (resultado[13] == "True")
                    adecuado.Text = "X";
                else if (resultado[13] == "False")
                    inadecuado.Text = "X";
                causa.Text = resultado[14];
                if (resultado[15].CompareTo("0") != 0)
                    basales.Text = resultado[15];
                if (resultado[16].CompareTo("0") != 0)
                    intermedias.Text = resultado[16];
                if (resultado[17].CompareTo("0") != 0)
                    superficiales.Text = resultado[17];

                //Inflamacion
                if (resultado[18] == "No")
                    inflamacion_no.Text = "X";
                else if (resultado[18] == "Leve")
                    inflamacion_leve.Text = "X";
                else if (resultado[18] == "Moderada")
                    inflamacion_moderada.Text = "X";
                else if (resultado[18] == "Severa")
                    inflamacion_severa.Text = "X";

                //Agentes Infecciosos
                if (resultado[19] == "True")
                    candidaSP.Text = "X";
                if (resultado[20] == "True")
                    gardnerella.Text = "X";
                if (resultado[21] == "True")
                    herpes.Text = "X";
                if (resultado[22] == "True")
                    vaginosis.Text = "X";
                if (resultado[23] == "True")
                    tricomonas.Text = "X";
                otroAgenteInfeccioso.Text = resultado[24];

                //Diagnostico Descriptivo
                if (resultado[25] == "True")
                    negativo.Text = "X";
                if (resultado[26] == "True")
                    atipica.Text = "X";
                if (resultado[27] == "True")
                    escamosa.Text = "X";
                if (resultado[28] == "True")
                    glandular.Text = "X";
                if (resultado[29] == "True")
                    lesionBajoGrado.Text = "X";
                if (resultado[30] == "True")
                    nic1.Text = "X";
                if (resultado[31] == "True")
                    infeccionVPH.Text = "X";
                if (resultado[32] == "True")
                    lesionAltoGrado.Text = "X";
                if (resultado[33] == "True")
                    nic2.Text = "X";
                if (resultado[34] == "True")
                    nic3.Text = "X";
                if (resultado[35] == "True")
                    cancer.Text = "X";
                if (resultado[36] == "True")
                    adenocarcinoma.Text = "X";

                //Recomendaciones
                if (resultado[37] != "0")
                    repetir.Text = resultado[37];
                if (resultado[38] == "True")
                    colposcopia.Text = "X";
                if (resultado[39] == "True")
                    biopsia.Text = "X";
                if (resultado[40] == "True")
                    despuesTratamiento.Text = "X";
                otraRecomendacion.Text = resultado[41];
                if (resultado[42].StartsWith("<"))
                    comentario.Xaml = resultado[42];
                else
                    comentario.Xaml = createXmalString(resultado[42]);

                if (resultado[43].StartsWith("<"))
                    diagnostico_txtR.Xaml = resultado[43];
                else
                {
                    String[] diagnosticos = resultado[43].Split(',');
                    try
                    {
                        diagnostico.Text = diagnosticos[0];
                        diagnostico2.Text = diagnosticos[1];
                        diagnostico3.Text = diagnosticos[2];
                        diagnostico4.Text = diagnosticos[3];
                    }
                    catch (Exception exp) { }
                }
                fechaInforme.Text = resultado[44];
            }

        }
        void myWebReference_getMuestraBiopsiaCompleted(object sender, ServiceReferenceClinica.getMuestraBiopsiaCompletedEventArgs e)
        {
            if (e.Result.ToString() == "")
            {
                if (!getCitologiaLiquidos)
                {
                    getCitologiaLiquidos = true;
                    Wrapper.getMuestraNo_GinecologicaCompleted += new EventHandler<ServiceReferenceClinica.getMuestraNo_GinecologicaCompletedEventArgs>(myWebReference_getMuestraNo_GinecologicaCompleted);
                    Wrapper.getMuestraNo_GinecologicaAsync(idMuestra);
                }
            }
            else
            {
                enableButtons(true);
                fotoFormulario.Source = new BitmapImage(biopsiaURI);
                //resetea los campos
                resetearValoresCitologia();

                estado.Text = "Datos Obtenidos!";
                String[] resultado;
                resultado = e.Result.ToString().Split('$');

                idBiopsia.Text = resultado[0];
                fecha.Text = convertDateToSpanishDate(resultado[1].Split(' ')[0]);
                nombre.Text = resultado[2] + " " + resultado[3] + " " + resultado[4] + " " + resultado[5];
                if (resultado[6].CompareTo("0") != 0)
                    edad.Text = resultado[6];
                diagnosticoMedicoB.Text = resultado[7];
                medicoB.Text = resultado[8];

                if (resultado[10].StartsWith("<"))
                {
                    diagnosticoBiopsia.Xaml = createXmalStringFromXaml(resultado[10], resultado[11], resultado[12], resultado[13]);
                }
                else
                    diagnosticoBiopsia.Xaml = createXmalString(resultado[10], resultado[11], resultado[12], resultado[13]);


                fechaRecibida_txt.Text = "Muestra recibida el: " + convertirFecha(resultado[14]);

                //Obtiene los datos para las muestras enviadas de la biopsia
                estado.Text = "Obteniendo Material Enviado...";
                if (!materiales)
                {
                    materiales = true;
                    Wrapper.getMaterialEnviadoBiopsiaImprimirCompleted += new EventHandler<ServiceReferenceClinica.getMaterialEnviadoBiopsiaImprimirCompletedEventArgs>(myWebReference_getMaterialEnviadoBiopsiaImprimirCompleted);
                    Wrapper.getMaterialEnviadoBiopsiaImprimirAsync(idMuestra, "MaterialEnviadoBiopsia");
                }
            }
        }

        void myWebReference_getMuestraNo_GinecologicaCompleted(object sender, ServiceReferenceClinica.getMuestraNo_GinecologicaCompletedEventArgs e)
        {
            enableButtons(true);
            resetearValoresCitologia();

            if (e.Result.Equals(""))
            {
                estado.Text = "No se encontro una muestra con el id " + idMuestra;
                return;
            }

            fotoFormulario.Source = new BitmapImage(biopsiaURI);
            estado.Text = "Datos Obtenidos!";
            String[] resultado = e.Result.ToString().Split('$');

            idBiopsia.Text = resultado[0];
            fecha.Text = convertDateToSpanishDate(resultado[1].Split(' ')[0]);
            nombre.Text = resultado[2] + " " + resultado[3] + " " + resultado[4] + " " + resultado[5];
            if (resultado[6].CompareTo("0") != 0)
                edad.Text = resultado[6];
            diagnosticoMedicoB.Text = resultado[7];
            medicoB.Text = resultado[8];

            if (resultado[9].StartsWith("<"))
            {
                diagnosticoBiopsia.Xaml = createXmalStringFromXaml(resultado[9], resultado[10], resultado[11], resultado[12]);
            }
            else
                diagnosticoBiopsia.Xaml = createXmalString(resultado[9], resultado[10], resultado[11], resultado[12]);

            fechaRecibida_txt.Text = "Muestra recibida el: " + convertirFecha(resultado[13]);

            //Obtiene los datos para las muestras enviadas de la biopsia
            estado.Text = "Obteniendo Material Enviado...";
            if (!materiales)
            {
                materiales = true;
                Wrapper.getMaterialEnviadoBiopsiaImprimirCompleted += new EventHandler<ServiceReferenceClinica.getMaterialEnviadoBiopsiaImprimirCompletedEventArgs>(myWebReference_getMaterialEnviadoBiopsiaImprimirCompleted);
                Wrapper.getMaterialEnviadoBiopsiaImprimirAsync(idMuestra, "MaterialEnviadoCitologia");
            }
        }

        void myWebReference_getMaterialEnviadoBiopsiaImprimirCompleted(object sender, ServiceReferenceClinica.getMaterialEnviadoBiopsiaImprimirCompletedEventArgs e)
        {
            enableButtons(true);
            estado.Text = "Datos Obtenidos!";
            if (e.Result.ToString() != "")
            {
                String[] resultado;
                resultado = e.Result.ToString().Split(';');
                try
                {
                    material1.Text = resultado[0];
                    material2.Text = resultado[1];
                    material3.Text = resultado[2];
                    material4.Text = resultado[3];
                    material5.Text = resultado[4];
                }
                catch (Exception exp)
                {

                }
            }
        }

        /********************************************************************************************************************************
        *********************************************************************************************************************************
        *********************************************                           *********************************************************
        *********************************************     UTILITY FUNCTIONS     *********************************************************
        *********************************************                           *********************************************************
        *********************************************************************************************************************************
        *********************************************************************************************************************************/

        private void enableButtons(bool value)
        {
            selectedIDMuestra_txt.IsReadOnly = !value;
            idMuestrasCombo.IsEnabled = value;
            button1.IsEnabled = value;
            buscar_btn.IsEnabled = value;

            if (value)
            {
                image1.Opacity = 1;
                image2.Opacity = 1;
            }
            else
            {
                image1.Opacity = 0.5;
                image2.Opacity = 0.5;
            }
        }

        private String convertDateToSpanishDate(String oldDate)
        {
            //Old date comes in format MM/DD/YYYY (Month/Day/Year)
            //New date returns spanish date format, e.g. DD/MM/YYYY (Day/Month/Year)
            String newDate = "";
            String[] dateArray = oldDate.Split('/');
            newDate = dateArray[1] + "/" + dateArray[0] + "/" + dateArray[2];
            return newDate;
        }

        //This is for texts in xaml form, to get the Paragraph tags
        private String createXmalStringFromXaml(String macro, String micro, String _diagnostico, String fecha)
        {
            String xaml = "";
            macro = macro.Substring(137, macro.Length - 147);
            micro = micro.Substring(137, micro.Length - 147);
            _diagnostico = _diagnostico.Substring(137, _diagnostico.Length - 147);

            if (macro.StartsWith("able User Interface"))
                macro = "<Paragraph FontSize=\"13\" FontFamily=\"Port" + macro;
            if (micro.StartsWith("able User Interface"))
                micro = "<Paragraph FontSize=\"13\" FontFamily=\"Port" + micro;
            if (_diagnostico.StartsWith("able User Interface"))
                _diagnostico = "<Paragraph FontSize=\"13\" FontFamily=\"Port" + _diagnostico;

            xaml += "<Section xml:space=\"preserve\" HasTrailingParagraphBreakOnPaste=\"False\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">";

            xaml += "<Paragraph FontSize=\"11\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"><Bold FontSize=\"16\" FontWeight=\"Bold\"><Run Text=\"Descripción Macroscópica\" /></Bold></Paragraph>";
            xaml += macro;
            xaml += "<Paragraph FontSize=\"24\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"> <Run Text=\" \" /></Paragraph>";

            xaml += "<Paragraph FontSize=\"11\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"><Bold FontSize=\"16\" FontWeight=\"Bold\"><Run Text=\"Descripción Microscópica\" /></Bold></Paragraph>";
            xaml += micro;
            xaml += "<Paragraph FontSize=\"24\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"> <Run Text=\" \" /></Paragraph>";


            xaml += "<Paragraph FontSize=\"11\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"><Bold FontSize=\"16\" FontWeight=\"Bold\"><Run Text=\"Diagnóstico\" /></Bold></Paragraph>";
            xaml += _diagnostico;
            xaml += "<Paragraph FontSize=\"42\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"> <Run Text=\" \" /></Paragraph>";

            xaml += "<Paragraph FontSize=\"11\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\">" +
                "<Bold FontSize=\"13\" FontWeight=\"Bold\"><Run Text=\"Fecha de Informe:  \" /></Bold><Run FontSize=\"13\" Text=\"";
            xaml += fecha;

            xaml += "\" /><Run Text=\"\n\" /><Run Text=\"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\" /><Run Text=\"DRA. ODESSA HENRIQUEZ RIVAS\" /><Run Text=\"\n\n\" /></Paragraph>" +
                "<Paragraph FontSize=\"24\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"> <Run Text=\" \" /></Paragraph></Section>";
            return xaml;
        }
        private String createXmalString(String text)
        {
            String xaml = "<Section xml:space=\"preserve\" HasTrailingParagraphBreakOnPaste=\"False\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"";
            xaml += text;
            xaml += "\" /></Paragraph></Section>";
            return xaml;
        }

        private String createXmalString(String macro, String micro, String _diagnostico, String fecha)
        {
            String xaml = "";

            xaml += "<Section xml:space=\"preserve\" HasTrailingParagraphBreakOnPaste=\"False\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">";

            xaml += "<Paragraph FontSize=\"11\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"><Bold FontSize=\"16\" FontWeight=\"Bold\"><Run Text=\"Descripción Macroscópica\" /></Bold></Paragraph>";
            xaml += "<Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"";
            xaml += macro + "\" /></Paragraph>";
            xaml += "<Paragraph FontSize=\"24\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"> <Run Text=\" \" /></Paragraph>";

            xaml += "<Paragraph FontSize=\"11\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"><Bold FontSize=\"16\" FontWeight=\"Bold\"><Run Text=\"Descripción Microscópica\" /></Bold></Paragraph>";
            xaml += "<Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"";
            xaml += micro + "\" /></Paragraph>";
            xaml += "<Paragraph FontSize=\"24\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"> <Run Text=\" \" /></Paragraph>";


            xaml += "<Paragraph FontSize=\"11\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"><Bold FontSize=\"16\" FontWeight=\"Bold\"><Run Text=\"Diagnóstico\" /></Bold></Paragraph>";
            xaml += "<Paragraph FontSize=\"13\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"><Run Text=\"";
            xaml += _diagnostico + "\" /></Paragraph>";
            xaml += "<Paragraph FontSize=\"42\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"> <Run Text=\" \" /></Paragraph>";

            xaml += "<Paragraph FontSize=\"11\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" FontStretch=\"Normal\" TextAlignment=\"Left\">" +
                "<Bold FontSize=\"13\" FontWeight=\"Bold\"><Run Text=\"Fecha de Informe:  \" /></Bold><Run FontSize=\"13\" Text=\"";
            xaml += fecha;
            xaml += "\" /><Run Text=\"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\" /><Run Text=\"DRA. ODESSA HENRIQUEZ RIVAS\" /></Paragraph>" +
                "<Paragraph FontSize=\"24\" FontFamily=\"Portable User Interface\" Foreground=\"#FF000000\" FontWeight=\"Normal\" FontStyle=\"Normal\" " +
                "FontStretch=\"Normal\" TextAlignment=\"Left\"> <Run Text=\" \" /></Paragraph></Section>";

            return xaml;
        }

        private void idMuestrasCombo_LostFocus(object sender, RoutedEventArgs e)
        {
            selectedIDMuestra_txt.Text = "";
        }

        private void selectedIDMuestra_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            idMuestrasCombo.SelectedIndex = -1;
        }

        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.PageVisual = espacioFormulario;
        }

        public String convertirFecha(String fechaNumeros)
        {
            String fechaPalabras = "";

            //Validando por si esta vacio
            if (fechaNumeros.CompareTo("") == 0)
                return "";

            //Las fechas vienen de la base de datos asi: Mes/Dia/Ano            
            string[] fechaSplitted = fechaNumeros.Split('/');

            fechaPalabras += fechaSplitted[1] + " de ";

            switch (fechaSplitted[0])
            {
                case "1":
                    fechaPalabras += "Enero";
                    break;
                case "2":
                    fechaPalabras += "Febrero";
                    break;
                case "3":
                    fechaPalabras += "Marzo";
                    break;
                case "4":
                    fechaPalabras += "Abril";
                    break;
                case "5":
                    fechaPalabras += "Mayo";
                    break;
                case "6":
                    fechaPalabras += "Junio";
                    break;
                case "7":
                    fechaPalabras += "Julio";
                    break;
                case "8":
                    fechaPalabras += "Agosto";
                    break;
                case "9":
                    fechaPalabras += "Septiembre";
                    break;
                case "10":
                    fechaPalabras += "Octubre";
                    break;
                case "11":
                    fechaPalabras += "Noviembre";
                    break;
                case "12":
                    fechaPalabras += "Diciembre";
                    break;
            }

            fechaPalabras += " de " + fechaSplitted[2];

            return fechaPalabras;
        }

        void resetearValoresCitologia()
        {
            //DATOS GENERALES
            idMuestra_txt.Text = "";
            nombre.Text = "";
            edad.Text = "";
            fecha.Text = "";
            medico.Text = "";
            origenMuestra.Text = "";
            fur.Text = "";
            diu.Text = "";
            anticonceptivos.Text = "";
            diagnosticoAnterior.Text = "";
            fechaInforme.Text = "";

            //CITOLOGIA
            adecuado.Text = "";
            inadecuado.Text = "";
            causa.Text = "";
            basales.Text = "";
            intermedias.Text = "";
            superficiales.Text = "";
            inflamacion_no.Text = "";
            inflamacion_leve.Text = "";
            inflamacion_moderada.Text = "";
            inflamacion_severa.Text = "";
            candidaSP.Text = "";
            gardnerella.Text = "";
            herpes.Text = "";
            vaginosis.Text = "";
            tricomonas.Text = "";
            otroAgenteInfeccioso.Text = "";
            negativo.Text = "";
            atipica.Text = "";
            escamosa.Text = "";
            glandular.Text = "";
            lesionBajoGrado.Text = "";
            nic1.Text = "";
            infeccionVPH.Text = "";
            lesionAltoGrado.Text = "";
            nic2.Text = "";
            nic3.Text = "";
            cancer.Text = "";
            adenocarcinoma.Text = "";
            repetir.Text = "";
            colposcopia.Text = "";
            biopsia.Text = "";
            despuesTratamiento.Text = "";
            otraRecomendacion.Text = "";
            comentario.SelectAll();
            comentario.Selection.Text = "";

            diagnostico_txtR.SelectAll();
            diagnostico_txtR.Selection.Text = "";

            diagnostico.Text = "";
            diagnostico2.Text = "";
            diagnostico3.Text = "";
            diagnostico4.Text = "";

            //BIOPSIA
            idBiopsia.Text = "";
            diagnosticoMedicoB.Text = "";
            medicoB.Text = "";
            fechaRecibida_txt.Text = "";

            material1.Text = "";
            material2.Text = "";
            material3.Text = "";
            material4.Text = "";
            material5.Text = "";
        }
    }

}
