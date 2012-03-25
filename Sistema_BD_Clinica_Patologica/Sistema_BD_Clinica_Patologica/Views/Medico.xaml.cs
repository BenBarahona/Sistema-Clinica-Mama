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
using System.Windows.Media.Imaging;

namespace Sistema_BD_Clinica_Patologica.Views
{
    public partial class Medico : Page
    {
        String idMedico = "";
        //FLAGS
        bool mainFlag = true;
        bool flag2 = true;
        bool flag3 = true;
        String Usuario = App.Correo;

        /********************************************************************************************************************************
        **************************************************    Inicializadores   *********************************************************
        *********************************************************************************************************************************/

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private Sistema_BD_Clinica_Patologica.ServiceReferenceClinica.WSClinicaSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:5633/WSClinica.asmx";


        public Medico()
        {
            InitializeComponent();
            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReferenceClinica.WSClinicaSoapClient(bind, endpoint);
            

            mainFlag = true;
            flag2 = true;
            Wrapper.getMedicos_NombresCompleted += new EventHandler<ServiceReferenceClinica.getMedicos_NombresCompletedEventArgs>(WebS_getMedicos);
            Wrapper.getMedicos_NombresAsync();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /********************************************************************************************************************************
        ***********************************************    MAIN BUTTON EVENTS   *********************************************************
        *********************************************************************************************************************************/


        private void guardar_btn5_Click(object sender, RoutedEventArgs e)
        {
            if (nombreMedicoMain_txt.Text.CompareTo("") == 0)
            {
                estado_medico.Text = "El nombre del médico no puede estar vacio...";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_error.Source = new BitmapImage(imgURI);
                return;
            }

            mainFlag = true;
            flag2 = true;
            flag3 = true;
            enableButtons(false);
            estado_medico.Text = "Guardando Médico...";
            Wrapper.InsertarMedicoCompleted += new EventHandler<ServiceReferenceClinica.InsertarMedicoCompletedEventArgs>(WebS_insertarMedico);
            Wrapper.InsertarMedicoAsync(nombreMedicoMain_txt.Text, telefonoMedico_txt.Text, celularMedico_txt.Text, direccion_txt.Text, compania_txt.Text, numeroColegiatura_txt.Text);
        }

        private void borrar_btn5_Click(object sender, RoutedEventArgs e)
        {
            if (nombreMedicoMain_txt.Text.CompareTo("") == 0)
            {
                estado_medico.Text = "El nombre del Médico no puede estar vacio...";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_error.Source = new BitmapImage(imgURI);
                return;
            }

            MessageBoxResult msgResult = MessageBox.Show("\t\t\tADVERTENCIA!!!\nAl borrar un medico, se eliminan TODOS los examenes que ese medico realizó!\n Seguro que quiere continuar? (Para revertir esto, llamar al Administrador)", 
                "Borrando Medico", System.Windows.MessageBoxButton.OKCancel);
            if (msgResult == MessageBoxResult.Cancel)
                return;

            estado_medico.Text = "Borrando Médico...";
            mainFlag = true;
            flag2 = true;
            flag3 = true;

            enableButtons(false);
            Wrapper.BorrarMedicoCompleted += new EventHandler<ServiceReferenceClinica.BorrarMedicoCompletedEventArgs>(WebS_borarrmedico);
            Wrapper.BorrarMedicoAsync(nombreMedicoMain_txt.Text);
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (nombreMedicoMain_txt.Text.Length == 0)
            {
                estado_medico.Text = "Escriba el nombre del medico a actualizar";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_error.Source = new BitmapImage(imgURI);
                return;
            }

            enableButtons(false);
            mainFlag = true;
            flag2 = true;
            estado_medico.Text  = "Actualizando Médico...";
            Wrapper.ActualizarMedicoCompleted += new EventHandler<ServiceReferenceClinica.ActualizarMedicoCompletedEventArgs>(myWebReference_ActualizarMedicoCompleted);
            Wrapper.ActualizarMedicoAsync(nombreMedicoMain_txt.Text, telefonoMedico_txt.Text, celularMedico_txt.Text, direccion_txt.Text, compania_txt.Text, numeroColegiatura_txt.Text);
        }

        private void detalle_Click(object sender, RoutedEventArgs e)
        {
            //if (medico_combo.SelectedItem == null)
            if (medicoNombre_auto.Text.Equals(""))
            {
                estado_medico.Text = "Seleccione un Médico";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                medico_combo_error.Source = new BitmapImage(imgURI);
                return;
            }

            estado_medico.Text = "Obteniendo Informacion del Médico...";
            enableButtons(false);
            mainFlag = true;
            flag2 = true;
            datosMedicos_grid.ItemsSource = null;
            Wrapper.getIDdeMedicoCompleted += new EventHandler<ServiceReferenceClinica.getIDdeMedicoCompletedEventArgs>(WebS_getIDMedico);
            //Wrapper.getIDdeMedicoAsync(medico_combo.SelectedItem.ToString());
            Wrapper.getIDdeMedicoAsync(medicoNombre_auto.Text);
        }

        /********************************************************************************************************************************
        *********************************************************************************************************************************
        *********************************************                           *********************************************************
        *********************************************   METHOD RESPONCE EVENTS  *********************************************************
        *********************************************                           *********************************************************
        *********************************************************************************************************************************
        *********************************************************************************************************************************/
       
        void WebS_insertarMedico(object sender, ServiceReferenceClinica.InsertarMedicoCompletedEventArgs e)
        {
            enableButtons(true);
            if (flag3)
            {
                flag3 = false;
                if (e.Result)
                {
                    estado_medico.Text = "Médico Guardado Exitosamente!... \nActualizando Médicos...";
                    resetFields();
                    Wrapper.getMedicos_NombresCompleted += new EventHandler<ServiceReferenceClinica.getMedicos_NombresCompletedEventArgs>(WebS_getMedicos);
                    Wrapper.getMedicos_NombresAsync();
                }
                else
                {
                    estado_medico.Text = "Hubo Problemas al guardar el Médico, intente de nuevo";
                }
            }
        }


        void WebS_borarrmedico(object sender, ServiceReferenceClinica.BorrarMedicoCompletedEventArgs e)
        {
            if (flag3)
            {
                flag3 = false;
                enableButtons(true);
                string idMedico = nombreMedicoMain_txt.Text;
                resetFields();
                if (e.Result)
                {
                    nombreMedicoMain_txt.Text = "";
                    telefonoMedico_txt.Text = "";
                    celularMedico_txt.Text = "";

                    Wrapper.getMedicos_NombresCompleted += new EventHandler<ServiceReferenceClinica.getMedicos_NombresCompletedEventArgs>(WebS_getMedicos);
                    Wrapper.getMedicos_NombresAsync();
                }
                else
                    estado_medico.Text = "Error al borrar el Medico, no existe un medico llamado " + idMedico + " en la base de datos";
            }
        }

        private void myWebReference_ActualizarMedicoCompleted(object sender, ServiceReferenceClinica.ActualizarMedicoCompletedEventArgs e)
        {
            if (mainFlag)
            {
                mainFlag = false;
                bool Response;
                Response = e.Result;
                enableButtons(true);

                if (!Response)
                {
                    estado_medico.Text = "No se pudo actualizar el medico.  Intente de nuevo";
                }
                else
                {
                    estado_medico.Text = "Medico actualizado exitosamente!";
                    Wrapper.getMedicosTodosCompleted += new EventHandler<ServiceReferenceClinica.getMedicosTodosCompletedEventArgs>(getDatosTodosMedicos);
                    Wrapper.getMedicosTodosAsync();
                }
            }
        }

        void WebS_getIDMedico(object sender, ServiceReferenceClinica.getIDdeMedicoCompletedEventArgs e)
        {
            if (flag2)
            {
                flag2 = false;
                if (e.Result == -1)
                {
                    enableButtons(true);
                    estado_medico.Text = "Hubo un error, intente denuevo";
                }
                else
                {
                    int idMedico = e.Result;
                    this.idMedico = e.Result.ToString();
                    estado_medico.Text = "Obteniendo Datos del Médico...";
                    Wrapper.getMedicoCompleted += new EventHandler<ServiceReferenceClinica.getMedicoCompletedEventArgs>(WebS_getDatos_Medico);
                    Wrapper.getMedicoAsync(idMedico);
                }
            }
        }

        void WebS_getMedicos(object sender, ServiceReferenceClinica.getMedicos_NombresCompletedEventArgs e)
        {
            if (mainFlag)
            {
                mainFlag = false;

                String[] resp = e.Result.ToString().Split(';');
                
                medicoNombre_auto.ItemsSource = null;
                medicoNombre_auto.ItemsSource = resp;

                Wrapper.getMedicosTodosCompleted += new EventHandler<ServiceReferenceClinica.getMedicosTodosCompletedEventArgs>(getDatosTodosMedicos);
                Wrapper.getMedicosTodosAsync();
                estado_medico.Text = "Buscando datos de medicos...";
            }
        }

        void getDatosTodosMedicos(object sender, ServiceReferenceClinica.getMedicosTodosCompletedEventArgs e)
        {
            if (flag2)
            {
                estado_medico.Text = "Médicos Actualizados!";

                flag2 = false;
                enableButtons(true);

                string[] content;
                content = e.Result.Split(';');
                List<medicos> valores = new List<medicos>();
                valores.Capacity = content.Length;
                for (int i = 0; i < content.Length - 1; i++)
                {
                    if (i % 7 == 0)
                    {
                        valores.Add(new medicos()
                        {
                            ID = content[i],
                            Nombre = content[i + 1],
                            Numero_Colegiación = content[i + 6],
                            Telefono = content[i + 2],
                            Celular = content[i + 3],
                            Direccion = content[i + 4],
                            Compañía = content[i + 5],
                        });
                    }
                }

                datosMedicos_grid.ItemsSource = valores;

            }
        }

        void WebS_getDatos_Medico(object sender, ServiceReferenceClinica.getMedicoCompletedEventArgs e)
        {
            if (mainFlag)
            {
                mainFlag = false;

                if (e.Result.ToString().CompareTo("") == 0)
                {
                    estado_medico.Text = "No se encontro ningun dato";
                    return;
                }
                estado_medico.Text = "Datos recibidos!";
                enableButtons(true);
                String[] resultado = e.Result.ToString().Split(';');
                List<medicos> valores = new List<medicos>();
                valores.Add(new medicos()
                {
                    ID = resultado[0],
                    Nombre = resultado[1],
                    Numero_Colegiación = resultado[6],
                    Telefono = resultado[2],
                    Celular = resultado[3],
                    Direccion = resultado[4],
                    Compañía = resultado[5]
                });

                datosMedicos_grid.ItemsSource = valores;
            }
        }

        /************************************************************************************
        ************************************************************************************
        *                                                                                  *
        *                      VALIDACIONES Y UTILIDADES                                   *
        *                                                                                  *  
        ************************************************************************************
        ************************************************************************************/

        public void enableButtons(bool valor)
        {
            guardar_btn5.IsEnabled = valor;
            button4.IsEnabled = valor;
            borrar_btn5.IsEnabled = valor;
            detalle.IsEnabled = valor;

            if (valor)
            {
                guardar_icon.Opacity = 1;
                buscar_icon.Opacity = 1;
                actualizar_icon.Opacity = 1;
                borrar_icon.Opacity = 1;
            }
            else
            {
                guardar_icon.Opacity = 0.5;
                buscar_icon.Opacity = 0.5;
                actualizar_icon.Opacity = 0.5;
                borrar_icon.Opacity = 0.5;
            }

        }

        public void resetFields()
        {
            nombreMedicoMain_txt.Text = "";
            numeroColegiatura_txt.Text = "";
            celularMedico_txt.Text = "";
            telefonoMedico_txt.Text = "";
            direccion_txt.Text = "";
            compania_txt.Text = "";
        }

        private void nombreMedicoMain_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            id_error.Source = MainPage.bmpClear;
        }

        private void medico_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            medico_combo_error.Source = MainPage.bmpClear;
        }

        private void datosMedicos_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                medicos datos = (medicos)datosMedicos_grid.SelectedItem;

                nombreMedicoMain_txt.Text = datos.Nombre;
                numeroColegiatura_txt.Text = datos.Numero_Colegiación;
                celularMedico_txt.Text = datos.Celular;
                telefonoMedico_txt.Text = datos.Telefono;
                direccion_txt.Text = datos.Direccion;
                compania_txt.Text = datos.Compañía;
            } catch(Exception exp) {}
        }

        private void resetFields_Click(object sender, RoutedEventArgs e)
        {
            resetFields();
        }
    }
}
