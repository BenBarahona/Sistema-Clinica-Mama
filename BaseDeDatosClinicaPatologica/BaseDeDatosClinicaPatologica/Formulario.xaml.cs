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

namespace BaseDeDatosClinicaPatologica
{
    public partial class Formulario : UserControl
    {
        PrintDocument pd;
        String idMuestra;
        MyWebReference.clinicaPatologiaWebServiceSoapClient myWebReference;
        String Usuario;

        //flags
        bool getIdExamenes = false;
        bool getCitologia = false;
        bool getBiopsia = false;
        bool getCitologiaLiquidos = false;
        bool materiales = false;

        public Formulario(String idMuestra, MyWebReference.clinicaPatologiaWebServiceSoapClient myWebReference, String Usuario)
        {
            this.myWebReference = myWebReference;
            this.Usuario = Usuario;
            this.idMuestra = idMuestra;
            InitializeComponent();
            idMuestrasCombo.IsEnabled = false;
            buscar_btn.IsEnabled = false;
            imprimir_btn.IsEnabled = false;
            estado.Content = "Obteniendo Datos...";
            scrollViewer1.Height = Application.Current.Host.Content.ActualHeight - 10;
            pd = new PrintDocument();
            pd.PrintPage += new EventHandler<PrintPageEventArgs>(pd_PrintPage);

            diagnostico_label.Content = "";
            microscopica_label.Content = "";
            macroscopica_label.Content = "";
            resetearValoresCitologia();
            myWebReference.getIdExamenesCompleted += new EventHandler<MyWebReference.getIdExamenesCompletedEventArgs>(myWebReference_getIdExamenesCompleted);
            myWebReference.getIdExamenesAsync();
        }

        void myWebReference_getIdExamenesCompleted(object sender, MyWebReference.getIdExamenesCompletedEventArgs e)
        {
            estado.Content = "Datos Obtenidos!";
            idMuestrasCombo.IsEnabled = true;
            buscar_btn.IsEnabled = true;
            imprimir_btn.IsEnabled = true;
            for (int i = idMuestrasCombo.Items.Count - 1; i > -1; i--)
                idMuestrasCombo.Items.RemoveAt(i);

            String[] resp = e.Result.ToString().Split(';');
            for (int i = 0; i < resp.Length; i++)
            {
                idMuestrasCombo.Items.Add(resp[i]);
            }
            idMuestrasCombo.SelectedItem = this.idMuestra;
            if (idMuestra.Substring(0, 1) == "-")
                return;

            try
            {
                String test = "";
                estado.Content = "Cargando datos de la muestra " + idMuestra;
                test = idMuestrasCombo.SelectedItem.ToString();
            }
            catch (Exception exp)
            {
                estado.Content = "Ocurrio un error. Intente de nuevo ";
                return;
            }

            if (!getCitologia)
            {
                getCitologia = true;
                myWebReference.getMuestraGinecologicaCompleted += new EventHandler<MyWebReference.getMuestraGinecologicaCompletedEventArgs>(myWebReference_getMuestraGinecologicaCompleted);
                myWebReference.getMuestraGinecologicaAsync(idMuestrasCombo.SelectedItem.ToString());
            }
        }

        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.PageVisual = espacioFormulario;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            pd.Print("Formulario Citologia");
        }

        private void paginaPrincipal_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new mainPage(this.myWebReference, this.Usuario);
        }

        private void buscar_btn_Click(object sender, RoutedEventArgs e)
        {
            getIdExamenes = false;
            getCitologia = false;
            materiales = false;
            getBiopsia = false;
            getCitologiaLiquidos = false;

            if (idMuestrasCombo.SelectedItem == null)
            {
                estado.Content = "Seleccione el ID de la muestra";
                return;
            }

            estado.Content = "Cargando Datos...";
            buscar_btn.IsEnabled = false;
            idMuestrasCombo.IsEnabled = false;
            selectedIDMuestra_txt.IsEnabled = false;
            button3.IsEnabled = false;
            idMuestra = idMuestrasCombo.SelectedItem.ToString();

            myWebReference.getMuestraGinecologicaCompleted += new EventHandler<MyWebReference.getMuestraGinecologicaCompletedEventArgs>(myWebReference_getMuestraGinecologicaCompleted);
            myWebReference.getMuestraGinecologicaAsync(idMuestrasCombo.SelectedItem.ToString());
        }

        void myWebReference_getMuestraGinecologicaCompleted(object sender, MyWebReference.getMuestraGinecologicaCompletedEventArgs e)
        {
            if (e.Result.ToString() == "")
            {
                if (!getBiopsia)
                {
                    getBiopsia = true;
                    myWebReference.getMuestraBiopsiaCompleted += new EventHandler<MyWebReference.getMuestraBiopsiaCompletedEventArgs>(myWebReference_getMuestraBiopsiaCompleted);
                    myWebReference.getMuestraBiopsiaAsync(idMuestra);
                }
            }
            else
            {
                diagnostico_label.Content = "";
                microscopica_label.Content = "";
                macroscopica_label.Content = "";
                buscar_btn.IsEnabled = true;
                idMuestrasCombo.IsEnabled = true;
                selectedIDMuestra_txt.IsEnabled = true;
                button3.IsEnabled = true;
                estado.Content = "Datos Obtenidos!";
                String[] resultado;
                resultado = e.Result.ToString().Split(';');

                //Resetea los campos de Biopsias para q no salgan
                idBiopsia.Content = "";
                diagnosticoMedicoB.Content = "";
                medicoB.Content = "";
                descripcionMacro.Text = "";
                descripcionMicro.Text = "";
                diagnosticoB.Text = "";
                material1.Content = "";
                material2.Content = "";
                material3.Content = "";
                material4.Content = "";
                material5.Content = "";

                //Empieza a llenar los campos
                idMuestra_txt.Content = resultado[0];
                fecha.Content = resultado[1];
                nombre.Content = resultado[2] + " " + resultado[3] + " " + resultado[4] + " " + resultado[5];
                edad.Content = resultado[6];
                medico.Content = resultado[7];
                origenMuestra.Content = resultado[8];
                if (resultado[9] != "0/0/0000")
                    fur.Content = resultado[9];
                if (resultado[10] != "False")
                    diu.Content = "Si";
                //Revisa para anticonceptivos
                if (resultado[11] == "True")
                    anticonceptivos.Content = "Si";
                diagnosticoAnterior.Content = resultado[12];
                //Revisa calidad del frotis
                if (resultado[13] == "True")
                    adecuado.Content = "X";
                else if (resultado[13] == "False")
                    inadecuado.Content = "X";
                causa.Content = resultado[14];
                if(resultado[15] != "0")
                    basales.Content = resultado[15];
                if (resultado[16] != "0")
                    intermedias.Content = resultado[16];
                if (resultado[17] != "0")
                    superficiales.Content = resultado[17];

                //Inflamacion
                if (resultado[18] == "No")
                    inflamacion_no.Content = "X";
                else if (resultado[18] == "Leve")
                    inflamacion_leve.Content = "X";
                else if (resultado[18] == "Moderada")
                    inflamacion_moderada.Content = "X";
                else if (resultado[18] == "Severa")
                    inflamacion_severa.Content = "X";

                //Agentes Infecciosos
                if (resultado[19] == "True")
                    candidaSP.Content = "X";
                if (resultado[20] == "True")
                    gardnerella.Content = "X";
                if (resultado[21] == "True")
                    herpes.Content = "X";
                if (resultado[22] == "True")
                    vaginosis.Content = "X";
                if (resultado[23] == "True")
                    tricomonas.Content = "X";
                otroAgenteInfeccioso.Content = resultado[24];

                //Diagnostico Descriptivo
                if (resultado[25] == "True")
                    negativo.Content = "X";
                if (resultado[26] == "True")
                    atipica.Content = "X";
                if (resultado[27] == "True")
                    escamosa.Content = "X";
                if (resultado[28] == "True")
                    glandular.Content = "X";
                if (resultado[29] == "True")
                    lesionBajoGrado.Content = "X";
                if (resultado[30] == "True")
                    nic1.Content = "X";
                if (resultado[31] == "True")
                    infeccionVPH.Content = "X";
                if (resultado[32] == "True")
                    lesionAltoGrado.Content = "X";
                if (resultado[33] == "True")
                    nic2.Content = "X";
                if (resultado[34] == "True")
                    nic3.Content = "X";
                if (resultado[35] == "True")
                    cancer.Content = "X";
                if (resultado[36] == "True")
                    adenocarcinoma.Content = "X";

                //Recomendaciones
                if (resultado[37] != "0")
                    repetir.Content = resultado[37];
                if (resultado[38] == "True")
                    colposcopia.Content = "X";
                if (resultado[39] == "True")
                    biopsia.Content = "X";
                if (resultado[40] == "True")
                    despuesTratamiento.Content = "X";
                otraRecomendacion.Content = resultado[41];
                comentario.Text = resultado[42];
                diagnostico.Text = resultado[43];
                fechaInforme.Content = resultado[44];
            }
        }

        void myWebReference_getMuestraBiopsiaCompleted(object sender, MyWebReference.getMuestraBiopsiaCompletedEventArgs e)
        {
            if (e.Result.ToString() == "")
            {
                if (!getCitologiaLiquidos)
                {
                    getCitologiaLiquidos = true;
                    myWebReference.getMuestraNo_GinecologicaCompleted += new EventHandler<MyWebReference.getMuestraNo_GinecologicaCompletedEventArgs>(myWebReference_getMuestraNo_GinecologicaCompleted);
                    myWebReference.getMuestraNo_GinecologicaAsync(idMuestra);
                }
            }
            else
            {
                diagnostico_label.Content = "Diagnostico";
                microscopica_label.Content = "Descripción Macroscópica";
                macroscopica_label.Content = "Descripción Microscópica";
                buscar_btn.IsEnabled = true;
                idMuestrasCombo.IsEnabled = true;
                selectedIDMuestra_txt.IsEnabled = true;
                button3.IsEnabled = true;
                estado.Content = "Datos Obtenidos!";
                String[] resultado;
                resultado = e.Result.ToString().Split(';');
                //resetea los campos
                resetearValoresCitologia();

                idBiopsia.Content = resultado[0];
                fecha.Content = resultado[1];
                nombre.Content = resultado[2] + " " + resultado[3] + " " + resultado[4] + " " + resultado[5];
                edad.Content = resultado[6];
                diagnosticoMedicoB.Content = resultado[7];
                medicoB.Content = resultado[8];
                descripcionMacro.Text = resultado[10];
                descripcionMicro.Text = resultado[11];
                diagnosticoB.Text = resultado[12];
                fechaInforme.Content = resultado[13];

                //Obtiene los datos para las muestras enviadas de la biopsia
                estado.Content = "Obteniendo Material Enviado...";
                if (!materiales)
                {
                    materiales = true;
                    myWebReference.getMaterialEnviadoBiopsiaImprimirCompleted += new EventHandler<MyWebReference.getMaterialEnviadoBiopsiaImprimirCompletedEventArgs>(myWebReference_getMaterialEnviadoBiopsiaImprimirCompleted);
                    myWebReference.getMaterialEnviadoBiopsiaImprimirAsync(idMuestra);
                }
            }
        }

        void myWebReference_getMuestraNo_GinecologicaCompleted(object sender, MyWebReference.getMuestraNo_GinecologicaCompletedEventArgs e)
        {
            diagnostico_label.Content = "Diagnostico";
            microscopica_label.Content = "Descripción Macroscópica";
            macroscopica_label.Content = "Descripción Microscópica";
            buscar_btn.IsEnabled = true;
            idMuestrasCombo.IsEnabled = true;
            selectedIDMuestra_txt.IsEnabled = true;
            button3.IsEnabled = true;
            estado.Content = "Datos Obtenidos!";
            String[] resultado;
            resultado = e.Result.ToString().Split(';');
            //Resetea los valores anteriores
            material1.Content = "";
            material2.Content = "";
            material3.Content = "";
            material4.Content = "";
            material5.Content = "";
            resetearValoresCitologia();

            idBiopsia.Content = resultado[0];
            fecha.Content = resultado[1];
            nombre.Content = resultado[2] + " " + resultado[3] + " " + resultado[4] + " " + resultado[5];
            edad.Content = resultado[6];
            diagnosticoMedicoB.Content = resultado[7];
            medicoB.Content = resultado[8];
            descripcionMacro.Text = resultado[9];
            descripcionMicro.Text = resultado[10];
            diagnosticoB.Text = resultado[11];
            fechaInforme.Content = resultado[12];
        }

        void myWebReference_getMaterialEnviadoBiopsiaImprimirCompleted(object sender, MyWebReference.getMaterialEnviadoBiopsiaImprimirCompletedEventArgs e)
        {
            if (e.Result.ToString() != "")
            {
                buscar_btn.IsEnabled = true;
                idMuestrasCombo.IsEnabled = true;
                selectedIDMuestra_txt.IsEnabled = true;
                button3.IsEnabled = true;
                estado.Content = "Datos Obtenidos!";
                String[] resultado;
                resultado = e.Result.ToString().Split(';');
                try
                {
                    material1.Content = resultado[0];
                    material2.Content = resultado[1];
                    material3.Content = resultado[2];
                    material4.Content = resultado[3];
                    material5.Content = resultado[4];
                }
                catch (Exception exp)
                {

                }
            }
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            string sURL = "/BaseDeDatosClinicaPatologica;component/Images/Citologia.jpg";
            Uri imgURI = new Uri(sURL, UriKind.Relative);
            fotoFormulario.Source = new BitmapImage(imgURI);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string sURL = "/BaseDeDatosClinicaPatologica;component/Images/Biopsia.jpg";
            Uri imgURI = new Uri(sURL, UriKind.Relative);
            fotoFormulario.Source = new BitmapImage(imgURI);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (selectedIDMuestra_txt.Text == "")
            {
                estado.Content = "Seleccione el ID de la muestra";
                return;
            }


            getIdExamenes = false;
            getCitologia = false;
            materiales = false;
            getBiopsia = false;
            getCitologiaLiquidos = false;
            estado.Content = "Cargando Datos...";
            buscar_btn.IsEnabled = false;
            idMuestrasCombo.IsEnabled = false;
            selectedIDMuestra_txt.IsEnabled = false;
            button3.IsEnabled = false;
            idMuestra = selectedIDMuestra_txt.Text;

            if (!getCitologia)
            {
                getCitologia = true;
                myWebReference.getMuestraGinecologicaCompleted += new EventHandler<MyWebReference.getMuestraGinecologicaCompletedEventArgs>(myWebReference_getMuestraGinecologicaCompleted);
                myWebReference.getMuestraGinecologicaAsync(selectedIDMuestra_txt.Text);
            }
        }

        void resetearValoresCitologia()
        {
            idMuestra_txt.Content = "";
            medico.Content = "";
            origenMuestra.Content = "";
            fur.Content = "";
            diu.Content = "";
            anticonceptivos.Content = "";
            diagnosticoAnterior.Content = "";
            adecuado.Content = "";
            inadecuado.Content = "";
            causa.Content = "";
            basales.Content = "";
            intermedias.Content = "";
            superficiales.Content = "";
            inflamacion_no.Content = "";
            inflamacion_leve.Content = "";
            inflamacion_moderada.Content = "";
            inflamacion_severa.Content = "";
            candidaSP.Content = "";
            gardnerella.Content = "";
            herpes.Content = "";
            vaginosis.Content = "";
            tricomonas.Content = "";
            otroAgenteInfeccioso.Content = "";
            negativo.Content = "";
            atipica.Content = "";
            escamosa.Content = "";
            glandular.Content = "";
            lesionBajoGrado.Content = "";
            nic1.Content = "";
            infeccionVPH.Content = "";
            lesionAltoGrado.Content = "";
            nic2.Content = "";
            nic3.Content = "";
            cancer.Content = "";
            adenocarcinoma.Content = "";
            repetir.Content = "";
            colposcopia.Content = "";
            biopsia.Content = "";
            despuesTratamiento.Content = "";
            otraRecomendacion.Content = "";
            comentario.Text = "";
            diagnostico.Text = "";
            //BIOPSIA
            idBiopsia.Content = "";
            diagnosticoMedicoB.Content = "";
            medicoB.Content = "";
            descripcionMacro.Text = "";
            descripcionMicro.Text = "";
            diagnosticoB.Text = "";
            material1.Content = "";
            material2.Content = "";
            material3.Content = "";
            material4.Content = "";
            material5.Content = "";
        }
    }
}
