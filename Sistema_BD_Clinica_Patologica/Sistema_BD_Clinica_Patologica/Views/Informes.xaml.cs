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
using System.Collections;
using System.Windows.Media.Imaging;

namespace Sistema_BD_Clinica_Patologica.Views
{
    public partial class Informes : Page
    {
        int anio = 0;
        int mes = 0;
        String doctor = "";

        int totalCitologias = 0;
        int totalBiopsias = 0;
        String Usuario = App.Correo;

        //Datos
        String tabla = "";
        String categoria = "";
        String rangoEdad = "";

        //Flags
        bool mainFlag = true;
        bool flag2 = true;

        //DATA VARIABLES    
        nombreCompletoPacientes[] lista;
        //String[] nameList;
        List<string> nameList;

        /********************************************************************************************************************************
        **************************************************    Inicializadores   *********************************************************
        *********************************************************************************************************************************/

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private Sistema_BD_Clinica_Patologica.ServiceReferenceClinica.WSClinicaSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:5633/WSClinica.asmx";

        public Informes()
        {
            InitializeComponent();
            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReferenceClinica.WSClinicaSoapClient(bind, endpoint);

            for (int i = 2010; i <= 2050; i++)
            {
                medicoAno.Items.Add(i);
            }
            for (int i = 0; i < 13; i++)
            {
                if (i == 0)
                    medicoMes.Items.Add("N\\A");
                else
                    medicoMes.Items.Add(i);
            }

            medicoMes.SelectedIndex = 0;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mainFlag = true;
            flag2 = true;

            Wrapper.getNombresPacientesCompleted += new EventHandler<ServiceReferenceClinica.getNombresPacientesCompletedEventArgs>(getNombresPacientes);
            Wrapper.getNombresPacientesAsync();
        }


        /********************************************************************************************************************************
        ***********************************************    MAIN BUTTON EVENTS   *********************************************************
        *********************************************************************************************************************************/

        private void buscar_btn_Click(object sender, RoutedEventArgs e)
        {
            String filtroText = "";
            String filtroFecha = "";
            mainFlag = true;
            flag2 = true;
            enableButtons(false);
            estado.Text = "Esperando Respuesta...";

            dataGrid1.ItemsSource = null;
            tabla = ((ComboBoxItem)(muestra_combo.SelectedItem)).Content.ToString();
            rangoEdad = ((ComboBoxItem)(rangoEdadCombo.SelectedItem)).Content.ToString();
            categoria = ((ComboBoxItem)(comboBox1.SelectedItem)).Content.ToString();

            filtroText = ((ComboBoxItem)(muestra_combo.SelectedItem)).Content.ToString();

            if (filtroText.Equals("Todas"))
                filtroText += " las muestras ";

            if (tabla.Equals("Citologías Ginecológicas"))
                tabla = "Ginecologica";
            else if (tabla.Equals("Citologías No Ginecológicas"))
                tabla = "No_Ginecologica";
            else if (tabla.Equals("Biopsias"))
                tabla = "Biopsia";

            if (!categoria.Equals("Ninguna"))
                filtroText += " de la Categoria: " + categoria;

            if (!rangoEdad.Equals("Ninguna"))
                filtroText += " con edades entre: " + rangoEdad;

            filtro_txt.Text = filtroText;


            if (fechaMuestra.Text.CompareTo("") != 0)
                filtroFecha += "Ingresados desde: " + fechaMuestra.Text;
            else if (datePicker1.Text.CompareTo("") != 0)
                filtroFecha += "Ingresados hasta: " + datePicker1.Text;
            else
                filtroFecha = "";

            if (fechaMuestra.Text.CompareTo("") != 0 && datePicker1.Text.CompareTo("") != 0)
                filtroFecha += " hasta: " + datePicker1.Text;


            fechaIngresados_txt.Text = filtroFecha;

            Wrapper.getCantidadDeExamenesCompleted += new EventHandler<ServiceReferenceClinica.getCantidadDeExamenesCompletedEventArgs>(WebS_CantidadExamenes);
            Wrapper.getCantidadDeExamenesAsync(tabla, rangoEdad, getFechaEnFormatoMysql(fechaMuestra.Text), getFechaEnFormatoMysql(datePicker1.Text), categoria, autoDoctor_txt.Text);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (medicoAno.SelectedIndex == -1)
            {
                estado.Text = "Ingrese el Año y/o Mes";
                string sURL = "/Sistema_BD_Clinica_Patologica;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_error.Source = new BitmapImage(imgURI);
                return;
            }

            mainFlag = true;
            flag2 = true;
            totalBiopsias = 0;
            totalCitologias = 0;
            totalCitologias_txt.Text = "";
            totalBiopsias_txt.Text = "";
            estado.Text = "Obteniendo datos...";
            enableButtons(false);

            if (medicoMes.SelectedIndex != 0)
                mes = Convert.ToInt32(medicoMes.SelectedItem.ToString());
            else
                mes = 0;

            anio = Convert.ToInt32(medicoAno.SelectedItem.ToString());
            doctor = autoDoctor2_txt.Text;

            Wrapper.consultaMedicoBiopsiasCompleted += new EventHandler<ServiceReferenceClinica.consultaMedicoBiopsiasCompletedEventArgs>(WebS_consultaMedicoBiopsia);
            Wrapper.consultaMedicoBiopsiasAsync(anio, mes, doctor);

        }

        private void buscarNombre_btn_Click(object sender, RoutedEventArgs e)
        {
            mainFlag = true;
            int index;
            nombreCompletoPacientes pacienteSeleccionado = new nombreCompletoPacientes();

            if (nombreAuto_txt.SelectedItem != null)
            {
                index = nameList.IndexOf(nombreAuto_txt.SelectedItem.ToString());
                pacienteSeleccionado = lista[index];
            }
            Wrapper.buscarMuestraPorPacienteCompleted += new EventHandler<ServiceReferenceClinica.buscarMuestraPorPacienteCompletedEventArgs>(Wrapper_buscarMuestraPorPacienteCompleted);
            Wrapper.buscarMuestraPorPacienteAsync(pacienteSeleccionado.primerNombre, pacienteSeleccionado.segundoNombre, pacienteSeleccionado.primerApellido, pacienteSeleccionado.segundoApellido);

        }

        /********************************************************************************************************************************
        *********************************************************************************************************************************
        *********************************************                           *********************************************************
        *********************************************   METHOD RESPONCE EVENTS  *********************************************************
        *********************************************                           *********************************************************
        *********************************************************************************************************************************
        *********************************************************************************************************************************/

        void Wrapper_buscarMuestraPorPacienteCompleted(object sender, ServiceReferenceClinica.buscarMuestraPorPacienteCompletedEventArgs e)
        {
            if (mainFlag)
            {
                mainFlag = false;
                if (e.Result.Equals(""))
                {
                    MessageBox.Show("No se encontraron resultados!");
                }
                String[] result = e.Result.Split(';');

                List<object> valores = new List<object>();
                valores.Capacity = result.Length;
                for (int i = 0; i < result.Length - 1; i++)
                {
                    if (i % 13 == 0)
                    {
                        valores.Add(new dataExamenB()
                        {
                            Muestra = result[i],
                            Nombre = result[i + 1] + " " + result[i + 2] + " " + result[i + 3] + " " + result[i + 4],
                            Edad = result[i + 5],
                            Diagnostico = result[i + 6],
                            Medico = result[i + 7],
                            Precio = result[i + 8],
                            Expediente = result[i + 9],
                            Fecha_Examen = result[i + 10].Split(' ')[0],
                            Fecha_Ingresado = result[i + 11],
                            Registrado_Por = result[i + 12],
                        });
                    }
                }

                dataGrid1.ItemsSource = valores;
            }
        }

        void getNombresPacientes(object sender, ServiceReferenceClinica.getNombresPacientesCompletedEventArgs e)
        {
            if (mainFlag)
            {
                mainFlag = false;
                System.Collections.ObjectModel.ObservableCollection<Sistema_BD_Clinica_Patologica.ServiceReferenceClinica.NombreCompleto> result = e.Result;

                lista = new nombreCompletoPacientes[e.Result.Count];
                //nameList = new String[e.Result.Count];
                nameList = new List<string>();

                if (result.Count == 0)//Quiere decir que no existen pacientes registrados
                {
                    estado.Text = "Advertencia!  No existen pacientes registrados, por lo que no podrá buscar por Nombre";
                    nombreAuto_txt.Text = "No existen pacientes en la base de datos";
                    return;
                }

                for (int i = 0; i < result.Count; i++)
                {
                    nombreCompletoPacientes item = new nombreCompletoPacientes();
                    item.primerNombre = e.Result[i].PrimerNombre;
                    item.segundoNombre = e.Result[i].SegundoNombre;
                    item.primerApellido = e.Result[i].PrimerApellido;
                    item.segundoApellido = e.Result[i].SegundoApellido;

                    lista[i] = item;

                    string name = e.Result[i].PrimerNombre + " " + e.Result[i].SegundoNombre + " " + e.Result[i].PrimerApellido + " " + e.Result[i].SegundoApellido;

                    nameList.Add(name);
                }


                nombreAuto_txt.ItemsSource = nameList;

                Wrapper.getMedicos_NombresCompleted += new EventHandler<ServiceReferenceClinica.getMedicos_NombresCompletedEventArgs>(getMedicosNombres);
                Wrapper.getMedicos_NombresAsync();
            }
        }

        void getMedicosNombres(object sender, ServiceReferenceClinica.getMedicos_NombresCompletedEventArgs e)
        {
            if (flag2)
            {
                flag2 = false;

                if (e.Result.Length != 0)
                {
                    String[] resp = e.Result.ToString().Split(';');
                    autoDoctor_txt.ItemsSource = resp;
                    autoDoctor2_txt.ItemsSource = resp;
                }
                else
                {
                    autoDoctor_txt.Text = "No existen doctores en la base de datos";
                    autoDoctor2_txt.Text = "No existen doctores en la base de datos";
                }
            }
        }

        void WebS_CantidadExamenes(object sender, ServiceReferenceClinica.getCantidadDeExamenesCompletedEventArgs e)
        {
            if (mainFlag)
            {
                mainFlag = false;

                if (e.Result.ToString().CompareTo("-1") != 0)
                {
                    textBox5.Text = e.Result.ToString();
                    Wrapper.getExamenesFiltradosCompleted += new EventHandler<ServiceReferenceClinica.getExamenesFiltradosCompletedEventArgs>(myWebReference_getExamenesFiltradosCompleted);
                    Wrapper.getExamenesFiltradosAsync(tabla, rangoEdad, getFechaEnFormatoMysql(fechaMuestra.Text), getFechaEnFormatoMysql(datePicker1.Text), categoria, autoDoctor_txt.Text);
                }
                else
                {
                    enableButtons(true);
                    estado.Text = "No se encontraron datos";
                    filtro_txt.Text = "Especifique el citerio de busqueda en la viñeta \"Consulta de Muestras\"";
                    fechaIngresados_txt.Text = "";
                }
            }
        }

        void WebS_consultaMedicoBiopsia(object sender, ServiceReferenceClinica.consultaMedicoBiopsiasCompletedEventArgs e)
        {
            if (mainFlag)
            {
                mainFlag = false;
                string[] content;
                content = e.Result.Split(';');
                List<estadisticaMedico> valores = new List<estadisticaMedico>();
                valores.Capacity = content.Length;
                for (int i = 0; i < content.Length - 1; i++)
                {
                    if (i % 3 == 0)
                    {
                        valores.Add(new estadisticaMedico()
                        {
                            Medico = content[i],
                            Muestras = content[i + 2]
                        });
                        totalBiopsias += Convert.ToInt32(content[i + 2]);
                    }
                }

                listadoMedicosBiopsia.ItemsSource = valores;
                totalBiopsias_txt.Text = totalBiopsias.ToString();
                totalBiopsias = 0;

                Wrapper.consultaMedicoCitologiaCompleted += new EventHandler<ServiceReferenceClinica.consultaMedicoCitologiaCompletedEventArgs>(WebS_consultaMedicoCitologia);
                Wrapper.consultaMedicoCitologiaAsync(anio, mes, doctor);
            }
        }

        void WebS_consultaMedicoCitologia(object sender, ServiceReferenceClinica.consultaMedicoCitologiaCompletedEventArgs e)
        {
            if (flag2)
            {
                flag2 = false;
                enableButtons(true);

                estado.Text = "Busqueda Finalizada";
                string[] content;
                content = e.Result.Split(';');
                List<estadisticaMedico> valores = new List<estadisticaMedico>();
                valores.Capacity = content.Length;
                for (int i = 0; i < content.Length - 1; i++)
                {
                    if (i % 3 == 0)
                    {
                        valores.Add(new estadisticaMedico()
                        {
                            Medico = content[i],
                            Muestras = content[i + 2]
                        });
                        totalCitologias += Convert.ToInt32(content[i + 2]);
                    }
                }

                listadoMedicosCitologia.ItemsSource = valores;
                totalCitologias_txt.Text = totalCitologias.ToString();
            }
        }

        void myWebReference_getExamenesFiltradosCompleted(object sender, ServiceReferenceClinica.getExamenesFiltradosCompletedEventArgs e)
        {
            if (flag2)
            {
                flag2 = false;
                enableButtons(true);

                if (e.Result.ToString().CompareTo("") != 0)
                {
                    estado.Text = "Datos recibidos!";

                    string[] content;
                    content = e.Result.Split(';');
                    List<object> valores = new List<object>();
                    valores.Capacity = content.Length;
                    for (int i = 0; i < content.Length - 1; i++)
                    {
                        if (tabla.Equals("Biopsia") || tabla.Equals("Todas"))
                        {
                            if (i % 13 == 0)
                            {
                                valores.Add(new dataExamenB()
                                {
                                    Muestra = content[i],
                                    Nombre = content[i + 1] + " " + content[i + 2] + " " + content[i + 3] + " " + content[i + 4],
                                    Edad = content[i + 5],
                                    Diagnostico = content[i + 6],
                                    Medico = content[i + 7],
                                    Precio = content[i + 8],
                                    Expediente = content[i + 9],
                                    Fecha_Examen = content[i + 10].Split(' ')[0],
                                    Fecha_Ingresado = content[i + 11],
                                    Registrado_Por = content[i + 12],
                                });
                            }
                        }
                        else
                        {
                            if (i % 14 == 0)
                            {
                                valores.Add(new dataExamen()
                                {
                                    Muestra = content[i],
                                    Nombre = content[i + 1] + " " + content[i + 2] + " " + content[i + 3] + " " + content[i + 4],
                                    Edad = content[i + 5],
                                    Diagnostico = content[i + 6],
                                    Comentario = content[i + 7],
                                    Medico = content[i + 8],
                                    Precio = content[i + 9],
                                    Expediente = content[i + 10],
                                    Fecha_Examen = content[i + 11].Split(' ')[0],
                                    Fecha_Ingresado = content[i + 12],
                                    Registrado_Por = content[i + 13],
                                });
                            }
                        }
                    }

                    dataGrid1.ItemsSource = valores;
                }
                else
                {
                    estado.Text = "No se encontraron muestras";
                    dataGrid1.ItemsSource = null;
                }
            }
        }

        /************************************************************************************
        ************************************************************************************
        *                                                                                  *
        *                      VALIDACIONES Y UTILIDADES                                   *
        *                                                                                  *  
        ************************************************************************************
        ************************************************************************************/
        private void enableButtons(bool value)
        {
            buscarNombre_btn.IsEnabled = value;
            buscar_btn.IsEnabled = value;
            button1.IsEnabled = value;

            if (value)
            {
                buscarNombre_icon.Opacity = 1;
                buscar_icon.Opacity = 1;
                medicoBtn_icon.Opacity = 1;
            }
            else
            {
                buscarNombre_icon.Opacity = 0.5;
                buscar_icon.Opacity = 0.5;
                medicoBtn_icon.Opacity = 0.5;
            }
        }

        private string getFechaEnFormatoMysql(String fecha)
        {
            if (fecha.CompareTo("") == 0)
                return "";
            String[] split = fecha.Split('/');
            return split[2] + "-" + split[0] + "-" + split[1];
        }

        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (muestra_combo.SelectedIndex == 1)
                {
                    comboBox1.IsEnabled = true;
                }
                else
                {
                    comboBox1.SelectedIndex = 0;
                    comboBox1.IsEnabled = false;
                }
            }
            catch (Exception exp) { }
        }

        private void medicoAno_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            id_error.Source = MainPage.bmpClear;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem == null)
                MessageBox.Show("No ha seleccionado un informe a imprimir!");
            else
            {
                dataExamenB selectedData = (dataExamenB)dataGrid1.SelectedItem;

                NavigationService.Navigate(new Uri("/Formulario" + "?ID=" + selectedData.Muestra, UriKind.Relative));
            }
        }

    }
}
