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

namespace BaseDeDatosClinicaPatologica
{
    public partial class mainPage : UserControl
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
        bool insertarBiopsia = true;
        bool insertarCitologiaLiquidos = false;
        string[] fecha;
        string[] fup;
        string[] fur;
        string[] fechaAnterior;
        string[] fechaBiopsia;
        string material;
        String[] nombre = new String[2];
        String[] apellido = new String[2];

        DateTime fechaInforme = DateTime.Now;
        String[] fechaInformeArray = new String[4];
        String fechaInformeIngresado = "";
        String fechaInformeString = "";
        //Control Flags
        bool insertarPacienteFlag = true;
        bool getIDPacienteFlag = true;
        bool getIDMedicoFlag = true;
        bool insertarExamenFlag = true;
        bool errorCitologia = true;
        bool errorBiopsia = true;
        bool errorMaterial = true;
        bool insertarMaterialFlag = true;

        bool esCitologia = false;
        bool esBiopsia = false;

        String anioActual = "";

        MyWebReference.clinicaPatologiaWebServiceSoapClient myWebReference = new MyWebReference.clinicaPatologiaWebServiceSoapClient();
        String Usuario = "";


        public mainPage(MyWebReference.clinicaPatologiaWebServiceSoapClient myWebReference, String usuario)
        {
            InitializeComponent();
            this.myWebReference = myWebReference;
            this.Usuario = usuario;

            //FORMATO DE FECHA QUE DEVUELTE:
            //Ej. Friday, December 17, 2010
            fechaInformeArray = fechaInforme.ToLongDateString().Split();
            fechaInformeIngresado = getFechaEnFormatoMysql(fechaInforme.Date.ToShortDateString());

            //Al traducirla, sale el arreglo asi: Viernes Diciembre 17 2010
            traducirFecha(fechaInformeArray);
            //Dia actual en string junto y asignacion del año para el ID de las muestras
            fechaInformeString = fechaInformeArray[0] + " " + fechaInformeArray[1] + " " + fechaInformeArray[2] + ", " + fechaInformeArray[3];
            anioActual = "-" + fechaInformeArray[3].Substring(2);

            VerificarAccesosDeUsuarios();
            scrollViewer1.Height = Application.Current.Host.Content.ActualHeight - 15;

            medico_combo.IsEnabled = false;
            detalle.IsEnabled = false;
            borrar_btn5.IsEnabled = false;
            guardar_btn.IsEnabled = false;
            nombreMedico.IsEnabled = false;
            nombreMedico2.IsEnabled = false;

            myWebReference.getMedicos_NombresCompleted += new EventHandler<MyWebReference.getMedicos_NombresCompletedEventArgs>(WebS_getMedicos);
            myWebReference.getMedicos_NombresAsync();
        }

        /************************************************************************************
         ************************************************************************************
         *                                                                                  *
         *                      VALIDACIONES Y UTILIDADES                                   *
         *                                                                                  *  
         ************************************************************************************
         ************************************************************************************/
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

        void Main_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {

        }

        private void negativo_check_Checked(object sender, RoutedEventArgs e)
        {
            negativo = true;
            celulasAtipicas_check.IsEnabled = false;
            lesionAltoGrado_check.IsEnabled = false;
            lesionBajoGrado_check.IsEnabled = false;
            carcinoma_check.IsEnabled = false;
            adenocarcinoma_check.IsEnabled = false;
        }

        private void negativo_check_Unchecked(object sender, RoutedEventArgs e)
        {
            negativo = false;
            celulasAtipicas_check.IsEnabled = true;
            lesionAltoGrado_check.IsEnabled = true;
            lesionBajoGrado_check.IsEnabled = true;
            carcinoma_check.IsEnabled = true;
            adenocarcinoma_check.IsEnabled = true;
        }


        private void celulasAtipicas_check_Checked(object sender, RoutedEventArgs e)
        {
            celulasAtipicas = true;
            negativo_check.IsEnabled = false;
            lesionAltoGrado_check.IsEnabled = false;
            lesionBajoGrado_check.IsEnabled = false;
            Escamosa_check.IsEnabled = true;
            glandular_check.IsEnabled = true;
            carcinoma_check.IsEnabled = false;
            adenocarcinoma_check.IsEnabled = false;
        }
        private void celulasAtipicas_check_Unchecked(object sender, RoutedEventArgs e)
        {
            celulasAtipicas = false;
            escamosa = false;
            glandular = false;
            negativo_check.IsEnabled = true;
            lesionAltoGrado_check.IsEnabled = true;
            lesionBajoGrado_check.IsEnabled = true;
            Escamosa_check.IsEnabled = false;
            glandular_check.IsEnabled = false;
            carcinoma_check.IsEnabled = true;
            adenocarcinoma_check.IsEnabled = true;
            Escamosa_check.IsChecked = false;
            glandular_check.IsChecked = false;
        }

        private void lesionBajoGrado_check_Checked(object sender, RoutedEventArgs e)
        {
            lesionBajoGrado = true;
            nic1_check.IsEnabled = true;
            infeccionVph_check.IsEnabled = true;
            negativo_check.IsEnabled = false;
            celulasAtipicas_check.IsEnabled = false;
            lesionAltoGrado_check.IsEnabled = false;
            carcinoma_check.IsEnabled = false;
            adenocarcinoma_check.IsEnabled = false;
            nic1_check.IsChecked = false;
            infeccionVph_check.IsChecked = false;
        }
        private void lesionBajoGrado_check_Unchecked(object sender, RoutedEventArgs e)
        {
            lesionBajoGrado = false;
            nic1 = false;
            infeccionVPH = false;
            nic1_check.IsChecked = false;
            infeccionVph_check.IsChecked = false;
            negativo_check.IsEnabled = true;
            celulasAtipicas_check.IsEnabled = true;
            nic1_check.IsEnabled = false;
            infeccionVph_check.IsEnabled = false;
            lesionAltoGrado_check.IsEnabled = true;
            carcinoma_check.IsEnabled = true;
            adenocarcinoma_check.IsEnabled = true;
            nic2_check.IsChecked = false;
            nic3_check.IsChecked = false;
        }

        private void lesionAltoGrado_check_Checked(object sender, RoutedEventArgs e)
        {
            lesionAltoGrado = true;
            negativo_check.IsEnabled = false;
            celulasAtipicas_check.IsEnabled = false;
            lesionBajoGrado_check.IsEnabled = false;
            nic2_check.IsEnabled = true;
            nic3_check.IsEnabled = true;
            carcinoma_check.IsEnabled = false;
            adenocarcinoma_check.IsEnabled = false;
        }
        private void lesionAltoGrado_check_Unchecked(object sender, RoutedEventArgs e)
        {
            lesionAltoGrado = false;
            nic2 = false;
            nic3 = false;
            nic2_check.IsChecked = false;
            nic3_check.IsChecked = false;
            negativo_check.IsEnabled = true;
            celulasAtipicas_check.IsEnabled = true;
            lesionBajoGrado_check.IsEnabled = true;
            nic2_check.IsEnabled = false;
            nic3_check.IsEnabled = false;
            carcinoma_check.IsEnabled = true;
            adenocarcinoma_check.IsEnabled = true;
        }

        private void carcinoma_check_Checked(object sender, RoutedEventArgs e)
        {
            carcinoma = true;
            negativo_check.IsEnabled = false;
            celulasAtipicas_check.IsEnabled = false;
            lesionAltoGrado_check.IsEnabled = false;
            lesionBajoGrado_check.IsEnabled = false;
            infeccionVph_check.IsEnabled = false;
            adenocarcinoma_check.IsEnabled = false;
        }
        private void carcinoma_check_Unchecked(object sender, RoutedEventArgs e)
        {
            carcinoma = false;
            negativo_check.IsEnabled = true;
            celulasAtipicas_check.IsEnabled = true;
            lesionAltoGrado_check.IsEnabled = true;
            lesionBajoGrado_check.IsEnabled = true;
            adenocarcinoma_check.IsEnabled = true;
        }

        private void adenocarcinoma_check_Checked(object sender, RoutedEventArgs e)
        {
            adenocarcinoma = true;
            negativo_check.IsEnabled = false;
            celulasAtipicas_check.IsEnabled = false;
            lesionAltoGrado_check.IsEnabled = false;
            lesionBajoGrado_check.IsEnabled = false;
            carcinoma_check.IsEnabled = false;
        }
        private void adenocarcinoma_check_Unchecked(object sender, RoutedEventArgs e)
        {
            adenocarcinoma = false;
            negativo_check.IsEnabled = true;
            celulasAtipicas_check.IsEnabled = true;
            lesionAltoGrado_check.IsEnabled = true;
            lesionBajoGrado_check.IsEnabled = true;
            carcinoma_check.IsEnabled = true;
        }

        private void edad_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            String text = this.edad_txt.Text;
            try
            {
                edadPaciente = int.Parse(text);
            }
            catch (Exception exception)
            {
                this.edad_txt.Text = "";
                MessageBox.Show("Valor Invalido");
            }
        }

        private void precio_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            String text = this.valor_txt.Text;
            try
            {
                precioCitologia = double.Parse(text);
            }
            catch (Exception exception)
            {
                this.valor_txt.Text = "";
                MessageBox.Show("Valor invalido");
            }
        }

        private void basales_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            String text = this.basales_txt.Text;
            try
            {
                basales = int.Parse(text);
            }
            catch (Exception exception)
            {
                this.basales_txt.Text = "";
                MessageBox.Show("Valor invalido");
            }
        }

        private void intermedias_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            String text = this.intermedias_txt.Text;
            try
            {
                intermedias = int.Parse(text);
            }
            catch (Exception exception)
            {
                this.intermedias_txt.Text = "";
                MessageBox.Show("Valor invalido");
            }
        }

        private void superficiales_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            String text = this.superficiales_txt.Text;
            try
            {
                superficiales = int.Parse(text);
            }
            catch (Exception exception)
            {
                this.superficiales_txt.Text = "";
                MessageBox.Show("Valor invalido");
            }
        }

        private void normal_btn_Click(object sender, RoutedEventArgs e)
        {
            String comentario = "Se observan células de exo y endocervix sin alteraciones";
            String diagnostico = "Negativo por malignidad";

            calidadFrotis_combo.SelectedItem = calidadFrotis_combo.Items[0];
            inflamacion_combo.SelectedItem = inflamacion_combo.Items[0];
            negativo_check.IsChecked = true;
            negativo_check.IsEnabled = true;

            celulasAtipicas_check.IsChecked = false;
            celulasAtipicas_check.IsEnabled = false;
            /*
            Escamosa_check.IsChecked = false;
            Escamosa_check.IsEnabled = false;
            glandular_check.IsEnabled = false;
            glandular_check.IsChecked = false;
             */
            lesionAltoGrado_check.IsChecked = false;
            lesionAltoGrado_check.IsEnabled = false;
            /*
            nic1_check.IsChecked = false;
            nic1_check.IsChecked = false;
            infeccionVph_check.IsEnabled = false;
            infeccionVph_check.IsChecked = false;
             */
            lesionBajoGrado_check.IsChecked = false;
            lesionBajoGrado_check.IsEnabled = false;
            /*
            nic2_check.IsEnabled = false;
            nic3_check.IsEnabled = false;
             */
            carcinoma_check.IsChecked = false;
            carcinoma_check.IsEnabled = false;
            adenocarcinoma_check.IsChecked = false;
            adenocarcinoma_check.IsEnabled = false;
            /**/
            comentario_txt.Text = comentario;
            diagnosticoCitologia_txt.Text = diagnostico;
            negativo = true;
        }


        private void repetirMeses_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            String text = this.repetirMeses_txt.Text;
            try
            {
                repetirMeses = int.Parse(text);
            }
            catch (Exception exception)
            {
                this.repetirMeses_txt.Text = "";
                MessageBox.Show("Valor invalido");
            }
        }

        private void increaseSize_btn2_Click(object sender, RoutedEventArgs e)
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

            if (diagnosticoCitologia_txt != null)
            {
                if (diagnosticoCitologia_txt.Selection.GetPropertyValue(Run.FontWeightProperty) is FontWeight &&
                ((FontWeight)diagnosticoCitologia_txt.Selection.GetPropertyValue(Run.FontWeightProperty)) ==
                FontWeights.Normal)
                    diagnosticoCitologia_txt.Selection.ApplyPropertyValue(Run.FontWeightProperty,
                        FontWeights.Bold);
                else
                    diagnosticoCitologia_txt.Selection.ApplyPropertyValue(Run.FontWeightProperty,
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

               if (diagnosticoCitologia_txt != null)
               {
                   if (diagnosticoCitologia_txt.Selection.GetPropertyValue(Run.TextDecorationsProperty) == null)
                       diagnosticoCitologia_txt.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Underline);
                   else
                       diagnosticoCitologia_txt.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, null);
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

              if (diagnosticoCitologia_txt != null)
              {
                  if (diagnosticoCitologia_txt.Selection.GetPropertyValue(Run.ForegroundProperty) == black)
                      diagnosticoCitologia_txt.Selection.ApplyPropertyValue(Run.ForegroundProperty, red);
                  else
                      diagnosticoCitologia_txt.Selection.ApplyPropertyValue(Run.ForegroundProperty, black);
              }
           * */
        }

        private void buscar_btn5_Click(object sender, RoutedEventArgs e)
        {
            myWebReference.getMedicoCompleted += new EventHandler<MyWebReference.getMedicoCompletedEventArgs>(WebS_getMedicoCompleted);
            myWebReference.getMedicoAsync(Convert.ToInt16(nombreMedicoMain_txt.Text));

        }

        void WebS_getMedicoCompleted(object sender, MyWebReference.getMedicoCompletedEventArgs e)
        {

            string Response;
            Response = e.Result;

            if (Response == "")
            {
                MessageBox.Show("Error al leer el Médico");
                return;
            }
            else
                nombreMedicoMain_txt.Text = Response.ToString();

        }

        private void informes_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Informes(myWebReference, Usuario);
        }

        private void borrarBtn_Click(object sender, RoutedEventArgs e)
        {
            if (idMuestra_txt.Text.CompareTo("") == 0)
            {
                estado_medico.Content = "El número de la muestra no puede estar vacio...";
                return;
            }

            MessageBoxResult msgResult = MessageBox.Show("Esta a punto de borrar una Citologia, \nEsta seguro que quiere continuar?", "Borrando Citologia", System.Windows.MessageBoxButton.OKCancel);
            if (msgResult == MessageBoxResult.Cancel)
                return;

            estado.Content = "Borrando Citología...";
            guardarBtn.IsEnabled = false;
            borrarBtn.IsEnabled = false;
            buscarBtn.IsEnabled = false;
            myWebReference.BorrarMuestraCompleted += new EventHandler<MyWebReference.BorrarMuestraCompletedEventArgs>(myWebReference_BorrarMuestraCompleted);
            myWebReference.BorrarMuestraAsync(idMuestra_txt.Text);
        }

        void myWebReference_BorrarMuestraCompleted(object sender, MyWebReference.BorrarMuestraCompletedEventArgs e)
        {
            borrarBtn.IsEnabled = true;
            guardarBtn.IsEnabled = true;
            buscarBtn.IsEnabled = true;
            if (e.Result)
            {
                idMuestra_txt.Text = "";
                estado.Content = "Se ha borrado la Citología exitosamente!";
            }
            else
                estado.Content = "No se pudo Borrar la Citología...";

        }

        private void increaseSize_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void decreaseSize_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void usuarios_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new ControlDeUsuario(myWebReference, Usuario);
        }//End void WebS_getMedicoCompleted(object sender, MyWebReference.HelloWorldCompletedEventArgs e)

        private void DIU_Checked(object sender, RoutedEventArgs e)
        {
            diu = true;
        }

        private void Anticonceptivos_Checked(object sender, RoutedEventArgs e)
        {
            anticonceptivos = true;
        }

        private void DIU_Unchecked(object sender, RoutedEventArgs e)
        {
            diu = false;
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

        private void Escamosa_check_Checked(object sender, RoutedEventArgs e)
        {
            escamosa = true;
            Escamosa_check.IsChecked = true;
        }

        private void Escamosa_check_Unchecked(object sender, RoutedEventArgs e)
        {
            escamosa = false;
            Escamosa_check.IsChecked = false;
        }

        private void glandular_check_Checked(object sender, RoutedEventArgs e)
        {
            glandular = true;
            glandular_check.IsChecked = true;
        }

        private void glandular_check_Unchecked(object sender, RoutedEventArgs e)
        {
            glandular = false;
            glandular_check.IsChecked = false;
        }

        private void nic1_check_Checked(object sender, RoutedEventArgs e)
        {
            nic1 = true;
            nic1_check.IsChecked = true;
        }

        private void nic1_check_Unchecked(object sender, RoutedEventArgs e)
        {
            nic1 = false;
            nic1_check.IsChecked = false;
        }

        private void infeccionVph_check_Checked(object sender, RoutedEventArgs e)
        {
            infeccionVPH = true;
            infeccionVph_check.IsChecked = true;
        }

        private void infeccionVph_check_Unchecked(object sender, RoutedEventArgs e)
        {
            infeccionVPH = false;
            infeccionVph_check.IsChecked = false;
        }

        private void nic2_check_Checked(object sender, RoutedEventArgs e)
        {
            nic2 = true;
            nic2_check.IsChecked = true;
        }

        private void nic2_check_Unchecked(object sender, RoutedEventArgs e)
        {
            nic2 = false;
            nic2_check.IsChecked = false;
        }

        private void nic3_check_Checked(object sender, RoutedEventArgs e)
        {
            nic3 = true;
            nic3_check.IsChecked = true;
        }

        private void nic3_check_Unchecked(object sender, RoutedEventArgs e)
        {
            nic3 = false;
            nic3_check.IsChecked = false;
        }

        private void idMuestra_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            idMuestra_txt2.Text = idMuestra_txt.Text;
            idMuestra_txt3.Text = idMuestra_txt.Text;
            idMuestra_txt4.Text = idMuestra_txt.Text;
            idMuestra_txt6.Text = idMuestra_txt.Text;
            idMuestra_txt2.IsEnabled = false;
            idMuestra_txt3.IsEnabled = false;
            idMuestra_txt4.IsEnabled = false;
        }//End void WebS_getPacienteCompleted(object sender, MyWebReference.HelloWorldCompletedEventArgs e)

        private void calidadFrotis_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calidadFrotis_combo.SelectedIndex == 0)
                calidadFrotis = true;
            else if (calidadFrotis_combo.SelectedIndex == 1)
                calidadFrotis = false;
        }

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

        private void btn_logout_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new MainPage();
        }

        private void btn_contabilidad_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Contabilidad(this.myWebReference, this.Usuario);
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

        private void fechaMuestra_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            fecha = fechaMuestra.Text.Split('/');
        }

        private void FUP_txt_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            fup = FUP_txt.Text.Split('/');
        }

        private void FUR_txt_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            fur = FUR_txt.Text.Split('/');
        }

        private void fechaCitologiaAnterior_txt_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            fechaAnterior = fechaCitologiaAnterior_txt.Text.Split('/');
        }

        private void fechaBiopsia_txt_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            fechaBiopsia = fechaBiopsia_txt.Text.Split('/');
        }

        private void creditos_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Credits(this.myWebReference, this.Usuario);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            string[] test = idMuestra_txt.Text.Split('-');
            try
            {
                if (test[1] == "")
                    this.Content = new Formulario(idMuestra_txt.Text + anioActual, this.myWebReference, this.Usuario);
                else
                    this.Content = new Formulario(idMuestra_txt.Text, this.myWebReference, this.Usuario);
            }
            catch (Exception exp)
            {
                this.Content = new Formulario(idMuestra_txt.Text + anioActual, this.myWebReference, this.Usuario);
            }
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
            resultadoCitologiaAnterior_txt.Text = "";
            fechaCitologiaAnterior_txt.Text = "";
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
            comentario_txt.Text = "";
            diagnosticoCitologia_txt.Text = "";

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
            macroscopica_txt.Text = "";
            micro_txt.Text = "";
            diagnosticoBiopsia_txt.Text = "";
        }

        /************************************************************************************
         ************************************************************************************
         ************************************************************************************
         *                   METODOS INSERT, TODO LLAMADO AL SERVIDOR                       *
         ************************************************************************************
         ************************************************************************************
         ************************************************************************************/

        private void guardarBtn_Click(object sender, RoutedEventArgs e)
        {
            //MASTER FLAG
            esCitologia = true;
            esBiopsia = false;
            //OTHER FLAGS
            insertarPacienteFlag = false;
            getIDPacienteFlag = false;
            getIDMedicoFlag = false;
            insertarExamenFlag = false;
            errorCitologia = false;

            if (idMuestra_txt.Text.Length == 0)
            {
                estado.Content = "Escriba un ID de Muestra";
                return;
            }

            if (primerNombrePaciente_txt.Text.Length == 0)
            {
                estado.Content = "Escriba el Primer Nombre del paciente";
                return;
            }

            if (primerApellidoPaciente_txt.Text.Length == 0)
            {
                estado.Content = "Escriba el Primer Apellido del paciente";
                return;
            }

            if (nombreMedico.SelectedItem == null)
            {
                estado.Content = "Seleccione un Médico";
                return;
            }

            if (fechaMuestra.Text.Length == 0)
            {
                estado.Content = "Ingrese la Fecha de la Muestra";
                return;
            }

            if (valor_txt.Text.Length == 0)
            {
                estado.Content = "Ingrese el precio de la Muestra!";
                return;
            }
            estado.Content = "Guardando Datos...";
            guardarBtn.IsEnabled = false;
            borrarBtn.IsEnabled = false;
            buscarBtn.IsEnabled = false;

            myWebReference.getIDdePacienteCompleted += new EventHandler<MyWebReference.getIDdePacienteCompletedEventArgs>(WebS_getIDdePacienteCompleted);
            myWebReference.getIDdePacienteAsync(primerNombrePaciente_txt.Text, segundoNombrePaciente_txt.Text, primerApellidoPaciente_txt.Text, segundoApellidoPaciente_txt.Text);
        }

        /****************************************************EVENTOS INSERT********************************************************/
        void WebS_InsertarPacienteCompleted(object sender, MyWebReference.InsertarPacienteCompletedEventArgs e)
        {
            if (esCitologia)
            {
                bool Response;
                Response = e.Result;
                if (Response == false)
                {
                    if (!errorCitologia)
                    {
                        errorCitologia = true;
                        MessageBox.Show("Se detectó un error!  Refresque la página o intente de nuevo");
                    }
                    estado.Content = "Error al insertar Paciente";
                    guardarBtn.IsEnabled = true;
                    borrarBtn.IsEnabled = true;
                    buscarBtn.IsEnabled = true;
                    return;
                }
                else
                {
                    estado.Content = "Paciente insertado con exito!";
                    //Get ID del paciente para Insertar Citologia
                    if (!getIDPacienteFlag)
                    {
                        getIDPacienteFlag = true;
                        myWebReference.getIDdePacienteCompleted += new EventHandler<MyWebReference.getIDdePacienteCompletedEventArgs>(WebS_getIDdePacienteCompleted);
                        myWebReference.getIDdePacienteAsync(primerNombrePaciente_txt.Text, segundoNombrePaciente_txt.Text, primerApellidoPaciente_txt.Text, segundoApellidoPaciente_txt.Text);
                    }
                }
            }

            else if (esBiopsia)
            {
                bool Response;
                Response = e.Result;

                if (Response == false)
                    estadoBiopsia.Content = "Error al insertar Paciente";
                else
                {
                    estadoBiopsia.Content = "Paciente insertado con exito !";
                    //Get ID del paciente para Insertar Biopsia
                    if (!getIDPacienteFlag)
                    {
                        getIDPacienteFlag = true;
                        myWebReference.getIDdePacienteCompleted += new EventHandler<MyWebReference.getIDdePacienteCompletedEventArgs>(WebS_getIDdePacienteCompleted);
                        myWebReference.getIDdePacienteAsync(nombre[0], nombre[1], apellido[0], apellido[1]);
                    }
                }
            }
        }

        void WebS_InsertarMedicoCompleted(object sender, MyWebReference.InsertarMedicoCompletedEventArgs e)
        {

            bool Response;
            Response = e.Result;

            if (Response == false)
            {
                guardarBtn.IsEnabled = true;
                borrarBtn.IsEnabled = true;
                buscarBtn.IsEnabled = true;
                if (!errorCitologia)
                {
                    errorCitologia = true;
                    MessageBox.Show("Se detectó un error!  Refresque la página o intente de nuevo");
                }
                estado.Content = "Error al insertar Medico";
            }
            else
                estado.Content = "Médico insertado con exito!";

        }

        void WebS_getIDdePacienteCompleted(object sender, MyWebReference.getIDdePacienteCompletedEventArgs e)
        {
            if (esCitologia)
            {
                int Response;
                Response = e.Result;
                estado.Content = "Obteniendo Datos para insertar muestra...";
                if (Response == -1)
                {
                    string formatedFUR = "";
                    string formatedFUP = "";

                    try
                    {
                        if (fur != null)
                            formatedFUR = fur[2] + "-" + fur[0] + "-" + fur[1];
                        if (fup != null)
                            formatedFUP = fup[2] + "-" + fup[0] + "-" + fup[1];
                    }
                    catch (Exception exp)
                    {

                    }
                    //Inserta el nuevo paciente
                    if (!insertarPacienteFlag)
                    {
                        insertarPacienteFlag = true;

                        myWebReference.InsertarPacienteCompleted += new EventHandler<MyWebReference.InsertarPacienteCompletedEventArgs>(WebS_InsertarPacienteCompleted);
                        myWebReference.InsertarPacienteAsync(diu, formatedFUR, formatedFUP, primerNombre.ToString(), segundoNombre.ToString(),
                            primerApellido.ToString(), segundoApellido.ToString(), edadPaciente, anticonceptivos, expediente_txt.Text);
                    }
                }
                else
                {
                    idPaciente = e.Result;

                    if (!getIDMedicoFlag)
                    {
                        getIDMedicoFlag = true;
                        myWebReference.getIDdeMedicoCompleted += new EventHandler<MyWebReference.getIDdeMedicoCompletedEventArgs>(WebS_getIDdeMedicoCompleted);
                        myWebReference.getIDdeMedicoAsync(nombreMedico.SelectedItem.ToString());
                    }
                }
            }

            else if (esBiopsia)
            {
                int Response;
                Response = e.Result;
                estadoBiopsia.Content = "Obteniendo datos para insertar muestra...";
                if (Response == -1)
                {
                    if (!insertarPacienteFlag)
                    {
                        insertarPacienteFlag = true;
                        myWebReference.InsertarPacienteCompleted += new EventHandler<MyWebReference.InsertarPacienteCompletedEventArgs>(WebS_InsertarPacienteCompleted);
                        myWebReference.InsertarPacienteAsync(diu, FUR_txt.Text,
                            FUP_txt.Text, nombre[0], nombre[1], apellido[0], apellido[1], Convert.ToInt16(edadBiopsia_txt.Text), anticonceptivos, expedienteBiopsia_txt.Text);
                    }
                }
                else
                {
                    idPaciente = e.Result;
                    if (!getIDMedicoFlag)
                    {
                        getIDMedicoFlag = true;
                        myWebReference.getIDdeMedicoCompleted += new EventHandler<MyWebReference.getIDdeMedicoCompletedEventArgs>(WebS_getIDdeMedicoCompleted);
                        myWebReference.getIDdeMedicoAsync(nombreMedico2.SelectedItem.ToString());
                    }
                }
            }

        }//End void WebS_getAccesoCompleted(object sender, MyWebReference.HelloWorldCompletedEventArgs e)


        void WebS_getIDdeMedicoCompleted(object sender, MyWebReference.getIDdeMedicoCompletedEventArgs e)
        {
            if (esCitologia)
            {
                int Response;
                Response = e.Result;

                if (Response == -1)
                {
                    guardarBtn.IsEnabled = true;
                    borrarBtn.IsEnabled = true;
                    buscarBtn.IsEnabled = true;
                    if (!errorCitologia)
                    {
                        errorCitologia = true;
                        MessageBox.Show("Se detectó un error!  Refresque la página o intente de nuevo");
                    }
                    estado.Content = "Error al leer el ID del paciente";
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
                    estado.Content = "Insertando Citologia...";

                    if (!insertarExamenFlag)
                    {
                        insertarExamenFlag = true;

                        myWebReference.InsertarCitologiaGinecologicaCompleted += new EventHandler<MyWebReference.InsertarCitologiaGinecologicaCompletedEventArgs>(WebS_InsertarCitologiaGinecologicaCompleted);
                        myWebReference.InsertarCitologiaGinecologicaAsync(idMuestra_txt.Text + anioActual, fechaFormated, Convert.ToInt16(valor_txt.Text.ToString()), diagnosticoCitologia_txt.Text,
                            diagnosticoClinico_txt.Text, idPaciente, idMedico, "Ingresos por Examen", Convert.ToInt16(fechaInformeArray[3]), Convert.ToInt16(fechaInforme.Month.ToString()), Usuario, comentario_txt.Text, inflamacion, cfCausa_txt.Text, calidadFrotis, anticonceptivos,
                            candidaSp, gardnerella, vaginosis, herpes, triconomas, otro_txt.Text, Convert.ToInt16(basales_txt.Text.ToString()), Convert.ToInt16(intermedias_txt.Text.ToString()),
                            Convert.ToInt16(superficiales_txt.Text.ToString()), colposcopia, Convert.ToInt16(repetirMeses_txt.Text.ToString()), otraRecomendacion_txt.Text.ToString(), repetirBiopsia, dspsTratamiento,
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

            else if (esBiopsia)
            {
                int Response;
                Response = e.Result;

                if (Response == -1)
                {
                    estadoBiopsia.Content = "Error al leer ID del Medico, intente denuevo";
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
                        registrarBiopsia_btn.IsEnabled = false;
                        borrarBiopsia.IsEnabled = false;
                        buscarBiopsia_btn.IsEnabled = false;
                        if (!insertarExamenFlag)
                        {
                            insertarExamenFlag = true;
                            myWebReference.InsertarBiopsiaCompleted += new EventHandler<MyWebReference.InsertarBiopsiaCompletedEventArgs>(WebS_InsertarBiopsiaCompleted);
                            myWebReference.InsertarBiopsiaAsync(idMuestra_txt6.Text + anioActual, fechaFormated, Convert.ToInt16(valorBiopsia_txt.Text), diagnosticoBiopsia_txt.Text.ToString(),
                                diagnosticoClinicoBiopsia_txt.Text.ToString(), idPaciente, idMedico, "Ingresos por Examen", Convert.ToInt16(fechaInformeArray[3]),
                                Convert.ToInt16(fechaInforme.Month.ToString()), Usuario, macroscopica_txt.Text.ToString(), micro_txt.Text.ToString(), codificacion_txt.Text.ToString(), fechaInformeString, fechaInformeIngresado);
                        }
                    }
                    else if (insertarCitologiaLiquidos)
                    {
                        registrarBiopsia_btn.IsEnabled = false;
                        borrarBiopsia.IsEnabled = false;
                        buscarBiopsia_btn.IsEnabled = false;
                        if (!insertarExamenFlag)
                        {
                            insertarExamenFlag = true;
                            myWebReference.InsertarCitologiaNoGinecologicaCompleted += new EventHandler<MyWebReference.InsertarCitologiaNoGinecologicaCompletedEventArgs>(WebS_InsertarCitologiaNoGinecologica);
                            myWebReference.InsertarCitologiaNoGinecologicaAsync(idMuestra_txt6.Text + anioActual, fechaFormated, Convert.ToInt16(valorBiopsia_txt.Text), diagnosticoBiopsia_txt.Text,
                                diagnosticoClinicoBiopsia_txt.Text, idPaciente, idMedico, "Ingresos por Examen", Convert.ToInt16(fechaInformeArray[3]),
                                Convert.ToInt16(fechaInforme.Month.ToString()), Usuario, macroscopica_txt.Text, micro_txt.Text, comentario_txt.Text, fechaInformeString, fechaInformeIngresado);
                        }
                    }

                }
            }

        }

        void WebS_InsertarCitologiaGinecologicaCompleted(object sender, MyWebReference.InsertarCitologiaGinecologicaCompletedEventArgs e)
        {
            insertarPacienteFlag = true;
            getIDPacienteFlag = true;
            getIDMedicoFlag = true;
            insertarExamenFlag = true;

            bool Response;
            Response = e.Result;

            guardarBtn.IsEnabled = true;
            borrarBtn.IsEnabled = true;
            buscarBtn.IsEnabled = true;
            if (Response == false)
            {
                if (!errorCitologia)
                {
                    errorCitologia = true;
                    MessageBox.Show("Se detectóun error!  Ya existe una muestra con el mismo numero");
                }
                estado.Content = "Error al insertar Citologia";
            }
            else
            {
                resetCampos();
                estado.Content = "Citología insertada con exito!";
            }
        }


        void WebS_getMaterialEnviado_CitologiaNoGinecologica(object sender, MyWebReference.getMaterialEnviado_CitologiaNoGinecologicaCompletedEventArgs e)
        {
            agregarMaterialEnviado.IsEnabled = true;
            borrarMaterial_btn.IsEnabled = true;
            materialEnviado_txt.IsEnabled = true;
            matEnviadoStatus.Content = "Materiales Obtenidos!";
            string[] content = new string[e.Result.Length];
            content = e.Result.Split(';');
            List<dataMaterial> valores = new List<dataMaterial>();
            valores.Capacity = content.Length;
            for (int i = 0; i < content.Length - 1; i++)
            {
                valores.Add(new dataMaterial()
                {
                    Nombre = content[i + 1]
                });
            }

            materialEnviadoTodos.ItemsSource = content;

        }

        void WebS_getMedicos(object sender, MyWebReference.getMedicos_NombresCompletedEventArgs e)
        {
            for (int i = medico_combo.Items.Count - 1; i > -1; i--)
                medico_combo.Items.RemoveAt(i);

            for (int i = nombreMedico.Items.Count - 1; i > -1; i--)
                nombreMedico.Items.RemoveAt(i);

            String[] resp = e.Result.ToString().Split(';');
            for (int i = 0; i < resp.Length; i++)
            {
                medico_combo.Items.Add(resp[i]);
                nombreMedico.Items.Add(resp[i]);
                nombreMedico2.Items.Add(resp[i]);
            }

            myWebReference.getMaterialEnviado_CitologiaNoGinecologicaCompleted += new EventHandler<MyWebReference.getMaterialEnviado_CitologiaNoGinecologicaCompletedEventArgs>(WebS_getMaterialEnviado_CitologiaNoGinecologica);
            myWebReference.getMaterialEnviado_CitologiaNoGinecologicaAsync();

            medico_combo.IsEnabled = true;
            nombreMedico.IsEnabled = true;
            nombreMedico2.IsEnabled = true;
            guardar_btn.IsEnabled = true;
            detalle.IsEnabled = true;
            borrar_btn5.IsEnabled = true;

            estado_medico.Content = "Médicos Atualizados!";
            estado.Content = "Datos Obtenidos!";
            estadoBiopsia.Content = "Datos Obtenidos!";
        }

        private void detalle_Click(object sender, RoutedEventArgs e)
        {
            estado_medico.Content = "Obteniendo Informacion del Médico...";
            detalle.IsEnabled = false;
            myWebReference.getIDdeMedicoCompleted +=
                new EventHandler<MyWebReference.getIDdeMedicoCompletedEventArgs>(WebS_getIDMedico);
            myWebReference.getIDdeMedicoAsync(medico_combo.SelectedItem.ToString());
        }

        void WebS_getIDMedico(object sender, MyWebReference.getIDdeMedicoCompletedEventArgs e)
        {

            estado_medico.Content = "Obteniendo Datos del Médico...";

            myWebReference.getMedicoCompleted +=
                new EventHandler<MyWebReference.getMedicoCompletedEventArgs>(WebS_getDatos_Medico);
            myWebReference.getMedicoAsync(e.Result);
        }

        void WebS_getDatos_Medico(object sender, MyWebReference.getMedicoCompletedEventArgs e)
        {
            estado_medico.Content = "Datos recibidos!";
            detalle.IsEnabled = true;
            String[] resultado = e.Result.ToString().Split(';');
            nombreMedicoMain_txt.Text = resultado[1];
            telefonoMedico_txt.Text = resultado[2];
            celularMedico_txt.Text = resultado[3];
        }

        private void guardar_btn5_Click(object sender, RoutedEventArgs e)
        {
            if (nombreMedicoMain_txt.Text.CompareTo("") == 0)
            {
                estado_medico.Content = "El nombre del médico no puede estar vacio...";
                return;
            }

            guardar_btn5.IsEnabled = false;
            borrar_btn5.IsEnabled = false;
            estado_medico.Content = "Guardando Médico...";
            myWebReference.InsertarMedicoCompleted +=
                new EventHandler<MyWebReference.InsertarMedicoCompletedEventArgs>(WebS_insertarMedico);
            myWebReference.InsertarMedicoAsync(nombreMedicoMain_txt.Text, telefonoMedico_txt.Text, celularMedico_txt.Text);
        }

        void WebS_insertarMedico(object sender, MyWebReference.InsertarMedicoCompletedEventArgs e)
        {
            guardar_btn5.IsEnabled = true;
            if (e.Result)
            {
                estado_medico.Content = "Médico Guardado Exitosamente!... \nActualizando Médicos...";
                medico_combo.IsEnabled = false;
                detalle.IsEnabled = false;
                borrar_btn5.IsEnabled = false;

                myWebReference.getMedicos_NombresCompleted += new EventHandler<MyWebReference.getMedicos_NombresCompletedEventArgs>(WebS_getMedicos);
                myWebReference.getMedicos_NombresAsync();
            }
            else
                estado_medico.Content = "Hubo Problemas al guardar el Médico...";
        }

        private void borrar_btn5_Click(object sender, RoutedEventArgs e)
        {
            if (nombreMedicoMain_txt.Text.CompareTo("") == 0)
            {
                estado_medico.Content = "El nombre del Médico no puede estar vacio...";
                return;
            }

            estado_medico.Content = "Borrando Médico...";
            guardar_btn5.IsEnabled = false;
            borrar_btn5.IsEnabled = false;
            myWebReference.BorrarMedicoCompleted +=
                new EventHandler<MyWebReference.BorrarMedicoCompletedEventArgs>(WebS_borarrmedico);
            myWebReference.BorrarMedicoAsync(nombreMedicoMain_txt.Text);
        }

        void WebS_borarrmedico(object sender, MyWebReference.BorrarMedicoCompletedEventArgs e)
        {
            if (e.Result)
            {
                nombreMedicoMain_txt.Text = "";
                telefonoMedico_txt.Text = "";
                celularMedico_txt.Text = "";

                medico_combo.IsEnabled = false;
                detalle.IsEnabled = false;

                myWebReference.getMedicos_NombresCompleted += new EventHandler<MyWebReference.getMedicos_NombresCompletedEventArgs>(WebS_getMedicos);
                myWebReference.getMedicos_NombresAsync();
            }
            else
                estado_medico.Content = "No se pudo Borrar el Médico...";
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

        private void segundoApellidoPaciente_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            segundoApellido = segundoApellidoPaciente_txt.Text;
            updateNombrePaciente(primerNombre, segundoNombre, primerApellido, segundoApellido);
        }

        void updateNombrePaciente(string pNombre, string sNombre, string pApellido, string sApellido)
        {
            nombrePaciente_txt2.Text = pNombre + " " + sNombre + " " + pApellido + " " + sApellido;
            nombrePaciente_txt3.Text = pNombre + " " + sNombre + " " + pApellido + " " + sApellido;
            nombrePaciente_txt4.Text = pNombre + " " + sNombre + " " + pApellido + " " + sApellido;
        }

        private void nombreMedico_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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

        private void registrarBiopsia_btn_Click(object sender, RoutedEventArgs e)
        {
            //MASTER FLAG
            esCitologia = false;
            esBiopsia = true;
            //OTHER FLAGS
            insertarMaterialFlag = false;
            insertarPacienteFlag = false;
            getIDPacienteFlag = false;
            getIDMedicoFlag = false;
            insertarExamenFlag = false;
            errorBiopsia = false;

            if (idMuestra_txt6.Text.Length == 0)
            {
                estadoBiopsia.Content = "Escriba un ID de Muestra";
                return;
            }

            if (nombrePacienteB.Text.Length == 0)
            {
                estadoBiopsia.Content = "Escriba el Primer Nombre del paciente";
                return;
            }

            if (apellidoPacienteB.Text.Length == 0)
            {
                estadoBiopsia.Content = "Escriba el Primer Apellido del paciente";
                return;
            }

            if (nombreMedico2.SelectedItem == null)
            {
                estado.Content = "Seleccione un Médico";
                return;
            }

            if (fechaBiopsia_txt.Text.Length == 0)
            {
                estadoBiopsia.Content = "Escriba la fecha de la muestra";
                return;
            }

            if (valorBiopsia_txt.Text == "")
            {
                estadoBiopsia.Content = "Escriba el valor de la Biopsia";
                return;
            }

            estadoBiopsia.Content = "Guardando Datos...";
            registrarBiopsia_btn.IsEnabled = false;
            buscarBiopsia_btn.IsEnabled = false;
            borrarBiopsia.IsEnabled = false;
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            agregarMaterialEnviado.IsEnabled = false;
            borrarMaterial_btn.IsEnabled = false;
            //Inserta el nuevo paciente

            nombre = (nombrePacienteB.Text + " ").Split(' ');
            apellido = (apellidoPacienteB.Text + " ").Split(' ');

            if (edadBiopsia_txt.Text == "")
                edadBiopsia_txt.Text = "0";

            myWebReference.getIDdePacienteCompleted += new EventHandler<MyWebReference.getIDdePacienteCompletedEventArgs>(WebS_getIDdePacienteCompleted);
            myWebReference.getIDdePacienteAsync(nombre[0], nombre[1], apellido[0], apellido[1]);
        }

        void WebS_InsertarBiopsiaCompleted(object sender, MyWebReference.InsertarBiopsiaCompletedEventArgs e)
        {
            insertarPacienteFlag = true;
            getIDPacienteFlag = true;
            getIDMedicoFlag = true;
            insertarExamenFlag = true;

            if (e.Result == false)
            {
                estadoBiopsia.Content = "Ha ocurrido un error...intente denuevo";
                if (!errorBiopsia)
                {
                    errorBiopsia = true;
                    MessageBox.Show("Se detectó un error!  Ya existe una muestra con el mismo numero");
                }
                registrarBiopsia_btn.IsEnabled = true;
                buscarBiopsia_btn.IsEnabled = true;
                borrarBiopsia.IsEnabled = true;
                button1.IsEnabled = true;
                button2.IsEnabled = true;
                agregarMaterialEnviado.IsEnabled = true;
                borrarMaterial_btn.IsEnabled = true;
            }
            else
            {
                if (materialEnviadoSelect.Items.Count > 0)
                {
                    if (!insertarMaterialFlag)
                    {
                        insertarMaterialFlag = true;
                        for (int i = 0; i < materialEnviadoSelect.Items.Count; i++)
                        {
                            myWebReference.InsertarMaterialEnviadoCompleted += new EventHandler<MyWebReference.InsertarMaterialEnviadoCompletedEventArgs>(myWebReference_InsertarMaterialEnviadoCompleted);
                            myWebReference.InsertarMaterialEnviadoAsync(idMuestra_txt6.Text + anioActual, materialEnviadoSelect.Items.ElementAt(i).ToString());
                        }
                    }
                }
                else
                {
                    resetCampos();
                    registrarBiopsia_btn.IsEnabled = true;
                    buscarBiopsia_btn.IsEnabled = true;
                    borrarBiopsia.IsEnabled = true;
                    estadoBiopsia.Content = "Biopsia Insertada con exito!";
                    button1.IsEnabled = true;
                    button2.IsEnabled = true;
                    agregarMaterialEnviado.IsEnabled = true;
                    borrarMaterial_btn.IsEnabled = true;
                }
            }
        }

        void myWebReference_InsertarMaterialEnviadoCompleted(object sender, MyWebReference.InsertarMaterialEnviadoCompletedEventArgs e)
        {
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
                registrarBiopsia_btn.IsEnabled = true;
                buscarBiopsia_btn.IsEnabled = true;
                borrarBiopsia.IsEnabled = true;
                estadoBiopsia.Content = "Biopsia Insertada con exito!";
                button1.IsEnabled = true;
                button2.IsEnabled = true;
                agregarMaterialEnviado.IsEnabled = true;
                borrarMaterial_btn.IsEnabled = true;
            }
        }

        void WebS_InsertarCitologiaNoGinecologica(object sender, MyWebReference.InsertarCitologiaNoGinecologicaCompletedEventArgs e)
        {
            insertarPacienteFlag = true;
            getIDPacienteFlag = true;
            getIDMedicoFlag = true;
            insertarExamenFlag = true;

            if (e.Result == false)
            {
                registrarBiopsia_btn.IsEnabled = true;
                buscarBiopsia_btn.IsEnabled = true;
                borrarBiopsia.IsEnabled = true;
                estadoBiopsia.Content = "Ha ocurrido un error...intente denuevo";
                if (!errorBiopsia)
                {
                    errorBiopsia = true;
                    MessageBox.Show("Se detectó un error!  Ya existe una muestra con el mismo numero");
                }
                button1.IsEnabled = true;
                button2.IsEnabled = true;
                agregarMaterialEnviado.IsEnabled = true;
                borrarMaterial_btn.IsEnabled = true;
            }
            else
            {
                resetCampos();
                registrarBiopsia_btn.IsEnabled = true;
                buscarBiopsia_btn.IsEnabled = true;
                borrarBiopsia.IsEnabled = true;
                estadoBiopsia.Content = "Biopsia Insertada con exito!";
            }
        }

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
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            materialEnviadoSelect.Items.Clear();
        }

        private void citologiaLiquido_radioBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            insertarCitologiaLiquidos = false;
            button1.IsEnabled = true;
            button2.IsEnabled = true;
        }

        private void borrarBiopsia_Click(object sender, RoutedEventArgs e)
        {
            if (idMuestra_txt6.Text.Length == 0)
            {
                estadoBiopsia.Content = "Escriba un ID de Muestra";
                return;
            }

            MessageBoxResult msgResult = MessageBox.Show("Esta a punto de borrar una Biopsia, \nEsta seguro que quiere continuar?", "Borrando Biopsia", System.Windows.MessageBoxButton.OKCancel);
            if (msgResult == MessageBoxResult.Cancel)
                return;

            estadoBiopsia.Content = "Borrando Datos...";
            if (insertarBiopsia)
            {
                registrarBiopsia_btn.IsEnabled = false;
                borrarBiopsia.IsEnabled = false;
                buscarBiopsia_btn.IsEnabled = false;
                myWebReference.BorrarMuestraCompleted += new EventHandler<MyWebReference.BorrarMuestraCompletedEventArgs>(myWebReference_BorrarMuestraCompleted2);
                myWebReference.BorrarMuestraAsync(idMuestra_txt6.Text);
            }
            else if (insertarCitologiaLiquidos)
            {
                registrarBiopsia_btn.IsEnabled = false;
                borrarBiopsia.IsEnabled = false;
                buscarBiopsia_btn.IsEnabled = false;
                myWebReference.BorrarMuestraCompleted += new EventHandler<MyWebReference.BorrarMuestraCompletedEventArgs>(myWebReference_BorrarMuestraCompleted2);
                myWebReference.BorrarMuestraAsync(idMuestra_txt6.Text);
            }
        }

        void myWebReference_BorrarMuestraCompleted2(object sender, MyWebReference.BorrarMuestraCompletedEventArgs e)
        {
            if (e.Result == false)
                MessageBox.Show("Error al borrar ");
            else
            {
                registrarBiopsia_btn.IsEnabled = true;
                borrarBiopsia.IsEnabled = true;
                buscarBiopsia_btn.IsEnabled = true;
                estadoBiopsia.Content = "Biopsia Borrada con exito!";
            }
        }

        void WebS_myWebReference_InsertarMaterialCompleted(object sender, MyWebReference.InsertarMaterialCompletedEventArgs e)
        {
            if (e.Result == false)
                MessageBox.Show("Error al insertar material");
            else
            {
                materialEnviado_txt.IsEnabled = true;
                agregarMaterialEnviado.IsEnabled = true;
                borrarMaterial_btn.IsEnabled = true;
                matEnviadoStatus.Content = "Material insertado exitosamente!\nActualizando lista de materiales...";
                myWebReference.getMaterialEnviado_CitologiaNoGinecologicaCompleted += new EventHandler<MyWebReference.getMaterialEnviado_CitologiaNoGinecologicaCompletedEventArgs>(WebS_getMaterialEnviado_CitologiaNoGinecologica);
                myWebReference.getMaterialEnviado_CitologiaNoGinecologicaAsync();
            }
        }


        /******************METODO DE ACCESOS************************************/

        private void VerificarAccesosDeUsuarios()
        {
            myWebReference.getAccesosCompleted
                += new EventHandler<MyWebReference.getAccesosCompletedEventArgs>(WebS_getAccesos);
            myWebReference.getAccesosAsync(this.Usuario);

            myWebReference.VerificaryActualizarContabilidadCompleted
                += new EventHandler<MyWebReference.VerificaryActualizarContabilidadCompletedEventArgs>(WebS_ContabilidadCreada);
            myWebReference.VerificaryActualizarContabilidadAsync();
        }

        void WebS_ContabilidadCreada(Object s, MyWebReference.VerificaryActualizarContabilidadCompletedEventArgs e)
        {
        }

        void WebS_getAccesos(Object Sender, MyWebReference.getAccesosCompletedEventArgs e)
        {
            String[] accesos_del_usuario = e.Result.Split(';');

            if (TieneAccesoA(accesos_del_usuario, "Control de Usuarios"))
                usuarios_btn.Visibility = System.Windows.Visibility.Visible;
            if (TieneAccesoA(accesos_del_usuario, "Informes"))
                informes_btn.Visibility = System.Windows.Visibility.Visible;
            if (TieneAccesoA(accesos_del_usuario, "Contabilidad"))
                btn_contabilidad.Visibility = System.Windows.Visibility.Visible;

            btn_logout.Visibility = System.Windows.Visibility.Visible;
            estado_acceso.Content = "";

        }

        private bool TieneAccesoA(String[] todos_los_accesos, String elAcceso)
        {
            for (int i = 0; i < todos_los_accesos.Length; i++)
                if (todos_los_accesos[i].CompareTo(elAcceso) == 0)
                    return true;
            return false;
        }


        /******************FIN METODO DE ACCESOS************************************/

        public class dataMaterial
        {
            public string Nombre { get; set; }
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
                matEnviadoStatus.Content = "Escriba el nombre del material";
                return;
            }

            matEnviadoStatus.Content = "Guardando Dato...";
            materialEnviado_txt.IsEnabled = false;
            agregarMaterialEnviado.IsEnabled = false;
            borrarMaterial_btn.IsEnabled = false;
            //Inserta el nuevo material
            myWebReference.InsertarMaterialCompleted += new EventHandler<MyWebReference.InsertarMaterialCompletedEventArgs>(WebS_myWebReference_InsertarMaterialCompleted);
            myWebReference.InsertarMaterialAsync(materialEnviado_txt.Text);
        }

        private void borrarMaterial_btn_Click(object sender, RoutedEventArgs e)
        {
            if (materialEnviado_txt.Text == "")
            {
                matEnviadoStatus.Content = "Escriba el nombre del material";
                return;
            }

            MessageBoxResult msgResult = MessageBox.Show("Esta a punto de borrar un Material, \nEsta seguro que quiere continuar?", "Borrando Materiales", System.Windows.MessageBoxButton.OKCancel);
            if (msgResult == MessageBoxResult.Cancel)
                return;

            matEnviadoStatus.Content = "Borrando Material";
            materialEnviado_txt.IsEnabled = false;
            agregarMaterialEnviado.IsEnabled = false;
            borrarMaterial_btn.IsEnabled = false;
            //Inserta el nuevo material
            myWebReference.BorrarMaterialCompleted += new EventHandler<MyWebReference.BorrarMaterialCompletedEventArgs>(WebS_borarrMaterial);
            myWebReference.BorrarMaterialAsync(materialEnviado_txt.Text);
        }

        void WebS_borarrMaterial(object sender, MyWebReference.BorrarMaterialCompletedEventArgs e)
        {
            if (e.Result)
            {
                materialEnviado_txt.Text = "";
                matEnviadoStatus.Content = "Material borrado exitosamente!\nActualizando lista de materiales...";
                myWebReference.getMaterialEnviado_CitologiaNoGinecologicaCompleted += new EventHandler<MyWebReference.getMaterialEnviado_CitologiaNoGinecologicaCompletedEventArgs>(WebS_getMaterialEnviado_CitologiaNoGinecologica);
                myWebReference.getMaterialEnviado_CitologiaNoGinecologicaAsync();
            }
            else
                estado_medico.Content = "No se pudo Borrar el Material...";
        }

        private void idMuestra_txt6_LostFocus(object sender, RoutedEventArgs e)
        {
            idMuestra_txt.Text = idMuestra_txt6.Text;
            idMuestra_txt2.Text = idMuestra_txt6.Text;
            idMuestra_txt3.Text = idMuestra_txt6.Text;
            idMuestra_txt4.Text = idMuestra_txt6.Text;
            idMuestra_txt2.IsEnabled = false;
            idMuestra_txt3.IsEnabled = false;
            idMuestra_txt4.IsEnabled = false;
        }

        private void buscar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (idMuestra_txt.Text.Length == 0)
            {
                estado.Content = "Escriba el No. de Muestra a buscar!";
                return;
            }
            estado.Content = "Buscando Datos...";
            guardarBtn.IsEnabled = false;
            borrarBtn.IsEnabled = false;
            buscarBtn.IsEnabled = false;
            string[] test = idMuestra_txt.Text.Split('-');
            try
            {
                if (test[1] != "")
                {
                    myWebReference.buscarCitologiaCompleted += new EventHandler<MyWebReference.buscarCitologiaCompletedEventArgs>(myWebReference_buscarCitologiaCompleted);
                    myWebReference.buscarCitologiaAsync(idMuestra_txt.Text);
                }
            }
            catch (Exception exp)
            {
                myWebReference.buscarCitologiaCompleted += new EventHandler<MyWebReference.buscarCitologiaCompletedEventArgs>(myWebReference_buscarCitologiaCompleted);
                myWebReference.buscarCitologiaAsync(idMuestra_txt.Text + anioActual);
            }
        }

        void myWebReference_buscarCitologiaCompleted(object sender, MyWebReference.buscarCitologiaCompletedEventArgs e)
        {
            if (e.Result.ToString() == "")
            {
                estado.Content = "No se encontró ninguna muestra";
                guardarBtn.IsEnabled = true;
                borrarBtn.IsEnabled = true;
                buscarBtn.IsEnabled = true;
            }
            else
            {
                resetCampos();
                guardarBtn.IsEnabled = true;
                borrarBtn.IsEnabled = true;
                buscarBtn.IsEnabled = true;
                estado.Content = "Datos Obtenidos!";
                String[] resultado;
                resultado = e.Result.ToString().Split(';');

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
                comentario_txt.Text = resultado[45];
                diagnosticoCitologia_txt.Text = resultado[46];
            }
        }

        private void buscarBiopsia_btn_Click(object sender, RoutedEventArgs e)
        {
            if (idMuestra_txt6.Text.Length == 0)
            {
                estadoBiopsia.Content = "Escriba el No. de Muestra a buscar!";
                return;
            }
            estadoBiopsia.Content = "Buscando Datos...";
            registrarBiopsia_btn.IsEnabled = false;
            buscarBiopsia_btn.IsEnabled = false;
            borrarBiopsia.IsEnabled = false;
            string[] test = idMuestra_txt6.Text.Split('-');
            try
            {
                if (test[1] != "")
                {
                    if (insertarBiopsia)
                    {
                        myWebReference.buscarBiopsiaCompleted += new EventHandler<MyWebReference.buscarBiopsiaCompletedEventArgs>(myWebReference_buscarBiopsiaCompleted);
                        myWebReference.buscarBiopsiaAsync(idMuestra_txt6.Text);
                    }
                    else if (insertarCitologiaLiquidos)
                    {
                        myWebReference.buscarCitologiaLiquidosCompleted += new EventHandler<MyWebReference.buscarCitologiaLiquidosCompletedEventArgs>(myWebReference_buscarCitologiaLiquidosCompleted);
                        myWebReference.buscarCitologiaLiquidosAsync(idMuestra_txt6.Text);
                    }
                }
            }
            catch (Exception exp)
            {
                if (insertarBiopsia)
                {
                    myWebReference.buscarBiopsiaCompleted += new EventHandler<MyWebReference.buscarBiopsiaCompletedEventArgs>(myWebReference_buscarBiopsiaCompleted);
                    myWebReference.buscarBiopsiaAsync(idMuestra_txt6.Text + anioActual);
                }
                else if (insertarCitologiaLiquidos)
                {
                    myWebReference.buscarCitologiaLiquidosCompleted += new EventHandler<MyWebReference.buscarCitologiaLiquidosCompletedEventArgs>(myWebReference_buscarCitologiaLiquidosCompleted);
                    myWebReference.buscarCitologiaLiquidosAsync(idMuestra_txt6.Text + anioActual);
                }   
            }
        }

        void myWebReference_buscarBiopsiaCompleted(object sender, MyWebReference.buscarBiopsiaCompletedEventArgs e)
        {
            if (e.Result.ToString() == "")
            {
                registrarBiopsia_btn.IsEnabled = true;
                borrarBiopsia.IsEnabled = true;
                buscarBiopsia_btn.IsEnabled = true;
                estadoBiopsia.Content = "No se encontró ninguna muestra";
            }
            else
            {
                resetCampos();
                registrarBiopsia_btn.IsEnabled = true;
                borrarBiopsia.IsEnabled = true;
                buscarBiopsia_btn.IsEnabled = true;
                estadoBiopsia.Content = "Datos Obtenidos!";
                String[] resultado;
                resultado = e.Result.ToString().Split(';');

                //Empieza a llenar los campos
                idMuestra_txt6.Text = resultado[0];
                fechaBiopsia_txt.Text = resultado[1];
                nombrePacienteB.Text = resultado[2] + resultado[3];
                apellidoPacienteB.Text = resultado[4] + resultado[5];
                edadBiopsia_txt.Text = resultado[6];
                nombreMedico2.SelectedItem = resultado[7];
                valorBiopsia_txt.Text = resultado[8];
                expedienteBiopsia_txt.Text = resultado[9];
                diagnosticoClinicoBiopsia_txt.Text = resultado[10];
                codificacion_txt.Text = resultado[11];
                macroscopica_txt.Text = resultado[12];
                micro_txt.Text = resultado[13];
                diagnosticoBiopsia_txt.Text = resultado[14];

                //Obtiene los datos para las muestras enviadas de la biopsia
                estado.Content = "Obteniendo Material Enviado...";
                myWebReference.getMaterialEnviadoBiopsiaImprimirCompleted += new EventHandler<MyWebReference.getMaterialEnviadoBiopsiaImprimirCompletedEventArgs>(myWebReference_getMaterialEnviadoBiopsiaImprimirCompleted);
                myWebReference.getMaterialEnviadoBiopsiaImprimirAsync(idMuestra_txt6.Text);
            }
        }//End myWebReference_buscarBiopsiaCompleted

        void myWebReference_getMaterialEnviadoBiopsiaImprimirCompleted(object sender, MyWebReference.getMaterialEnviadoBiopsiaImprimirCompletedEventArgs e)
        {
            if (e.Result.ToString() == "")
            {
                return;
            }
            else
            {
                estado.Content = "Datos Obtenidos!";
                String[] resultado;
                resultado = e.Result.ToString().Split(';');
                for (int i = 0; i < resultado.Length; i++)
                    materialEnviadoSelect.Items.Add(resultado[i]);
            }
        }

        void myWebReference_buscarCitologiaLiquidosCompleted(object sender, MyWebReference.buscarCitologiaLiquidosCompletedEventArgs e)
        {
            if (e.Result.ToString() == "")
            {
                registrarBiopsia_btn.IsEnabled = true;
                borrarBiopsia.IsEnabled = true;
                buscarBiopsia_btn.IsEnabled = true;
                estadoBiopsia.Content = "No se encontró ninguna muestra con ese No. de muestra";
            }
            else
            {
                resetCampos();
                registrarBiopsia_btn.IsEnabled = true;
                borrarBiopsia.IsEnabled = true;
                buscarBiopsia_btn.IsEnabled = true;
                estadoBiopsia.Content = "Datos Obtenidos!";
                String[] resultado;
                resultado = e.Result.ToString().Split(';');

                //Empieza a llenar los campos
                idMuestra_txt6.Text = resultado[0];
                fechaBiopsia_txt.Text = resultado[1];
                nombrePacienteB.Text = resultado[2] + resultado[3];
                apellidoPacienteB.Text = resultado[4] + resultado[5];
                edadBiopsia_txt.Text = resultado[6];
                nombreMedico2.SelectedItem = resultado[7];
                valorBiopsia_txt.Text = resultado[8];
                expedienteBiopsia_txt.Text = resultado[9];
                diagnosticoClinicoBiopsia_txt.Text = resultado[10];
                macroscopica_txt.Text = resultado[11];
                micro_txt.Text = resultado[12];
                diagnosticoBiopsia_txt.Text = resultado[13];
            }
        }

        private void guardar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (idMuestra_txt.Text.Length == 0)
            {
                estadoActualizar.Content = "Escriba un ID de Muestra";
                estadoActualizar2.Content = "Escriba un ID de Muestra";
                estadoActualizar3.Content = "Escriba un ID de Muestra";
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

            estadoActualizar.Content = "Guardando Datos...";
            estadoActualizar2.Content = "Guardando Datos...";
            estadoActualizar3.Content = "Guardando Datos...";
            guardarBtn.IsEnabled = false;
            borrarBtn.IsEnabled = false;
            buscarBtn.IsEnabled = false;
            guardar_btn.IsEnabled = false;
            guardar_btn2.IsEnabled = false;
            guardar_btn3.IsEnabled = false;
            normal_btn.IsEnabled = false;
            
            myWebReference.ActualizarCitologiaCompleted += new EventHandler<MyWebReference.ActualizarCitologiaCompletedEventArgs>(myWebReference_ActualizarCitologiaCompleted);
            myWebReference.ActualizarCitologiaAsync(idMuestra_txt.Text, diagnosticoCitologia_txt.Text, comentario_txt.Text, inflamacion,
                cfCausa_txt.Text, calidadFrotis, candidaSp, gardnerella, vaginosis, herpes, triconomas, otro_txt.Text, Convert.ToInt16(basales_txt.Text.ToString()),
                Convert.ToInt16(intermedias_txt.Text.ToString()), Convert.ToInt16(superficiales_txt.Text.ToString()), colposcopia,
                Convert.ToInt16(repetirMeses_txt.Text.ToString()), otraRecomendacion_txt.Text.ToString(), repetirBiopsia, dspsTratamiento,
                nic1, nic2, nic3, origenMuestra, negativo, infeccionVPH, glandular, escamosa, adenocarcinoma, carcinoma, celulasAtipicas, lesionAltoGrado, lesionBajoGrado);
        }//End myWebReference_buscarCitologiaLiquidosCompleted

        public void myWebReference_ActualizarCitologiaCompleted(object sender, MyWebReference.ActualizarCitologiaCompletedEventArgs e)
        {
            bool Response;
            Response = e.Result;

            guardarBtn.IsEnabled = true;
            borrarBtn.IsEnabled = true;
            buscarBtn.IsEnabled = true;
            guardar_btn.IsEnabled = true;
            guardar_btn2.IsEnabled = true;
            guardar_btn3.IsEnabled = true;
            normal_btn.IsEnabled = true;

            if (Response == false)
            {
                estadoActualizar.Content = "Error al actualizar Citologia";
                estadoActualizar2.Content = "Error al actualizar Citologia";
                estadoActualizar3.Content = "Error al actualizar Citologia";
            }
            else
            {
                resetCampos();
                estadoActualizar.Content = "Citología actualizada con exito!";
                estadoActualizar2.Content = "Citología actualizada con exito!";
                estadoActualizar3.Content = "Citología actualizada con exito!";
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (nombreMedicoMain_txt.Text.Length == 0)
            {
                estado_medico.Content = "Escriba el nombre del medico a actualizar";
                return;
            }

            guardar_btn5.IsEnabled = false;
            borrar_btn5.IsEnabled = false;
            button4.IsEnabled = false;
            detalle.IsEnabled = false;
            estado_medico.Content = "Actualizando Médico...";
            myWebReference.ActualizarMedicoCompleted += new EventHandler<MyWebReference.ActualizarMedicoCompletedEventArgs>(myWebReference_ActualizarMedicoCompleted);
            myWebReference.ActualizarMedicoAsync(nombreMedicoMain_txt.Text, telefonoMedico_txt.Text, celularMedico_txt.Text);
        }

        private void myWebReference_ActualizarMedicoCompleted(object sender, MyWebReference.ActualizarMedicoCompletedEventArgs e)
        {
            bool Response;
            Response = e.Result;
            guardar_btn5.IsEnabled = true;
            borrar_btn5.IsEnabled = true;
            button4.IsEnabled = true;
            detalle.IsEnabled = true;

            if (!Response)
            {
                estado_medico.Content = "No se pudo actualizar el medico.  Intente de nuevo";
            }
            else
            {
                estado_medico.Content = "Medico actualizado exitosamente!";
            }
        }

        private void actualizarBiopsia_Click(object sender, RoutedEventArgs e)
        {
            if (idMuestra_txt6.Text.Length == 0)
            {
                estadoBiopsia.Content = "Escriba un ID de Muestra";
                return;
            }

            estadoBiopsia.Content = "Guardando Datos...";
            registrarBiopsia_btn.IsEnabled = false;
            buscarBiopsia_btn.IsEnabled = false;
            borrarBiopsia.IsEnabled = false;
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            agregarMaterialEnviado.IsEnabled = false;
            borrarMaterial_btn.IsEnabled = false;

            if (insertarBiopsia)
            {
                myWebReference.ActualizarBiopsiaCompleted += new EventHandler<MyWebReference.ActualizarBiopsiaCompletedEventArgs>(myWebReference_ActualizarBiopsiaCompleted);
                myWebReference.ActualizarBiopsiaAsync(idMuestra_txt6.Text, macroscopica_txt.Text, micro_txt.Text, codificacion_txt.Text, diagnosticoBiopsia_txt.Text);
            }
            else if (insertarCitologiaLiquidos)
            {
                myWebReference.ActualizarCitologiaLiquidosCompleted += new EventHandler<MyWebReference.ActualizarCitologiaLiquidosCompletedEventArgs>(myWebReference_ActualizarCitologiaLiquidosCompleted);
                myWebReference.ActualizarCitologiaLiquidosAsync(idMuestra_txt6.Text, macroscopica_txt.Text, micro_txt.Text, diagnosticoBiopsia_txt.Text);
            }
        }

        private void myWebReference_ActualizarBiopsiaCompleted(object sender, MyWebReference.ActualizarBiopsiaCompletedEventArgs e)
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
                estadoBiopsia.Content = "Error al actualizar Biopsia";
            }
            else
            {
                resetCampos();
                estadoBiopsia.Content = "Biopsia actualizada con exito!";
            }
        }

        private void myWebReference_ActualizarCitologiaLiquidosCompleted(object sender, MyWebReference.ActualizarCitologiaLiquidosCompletedEventArgs e)
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
                estadoBiopsia.Content = "Error al actualizar Biopsia";
            }
            else
            {
                resetCampos();
                estadoBiopsia.Content = "Biopsia actualizada con exito!";
            }
        }

    }//FIN DE CLASE


    /************************************************************************************
     ************************************************************************************
     *                                                                                  *
     *                      CLASSES PARA VALORES DE DATAGRIDS                            *
     *                                                                                  *  
     ************************************************************************************
     ************************************************************************************/


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
        public string Apellido { get; set; }
        public string Edad { get; set; }
        public string Expediente { get; set; }
        public string Medico { get; set; }
        public string Precio { get; set; }
        public string FechaCalendario { get; set; }
        public string Fecha { get; set; }
        public string Registrado_Por { get; set; }
    }
}