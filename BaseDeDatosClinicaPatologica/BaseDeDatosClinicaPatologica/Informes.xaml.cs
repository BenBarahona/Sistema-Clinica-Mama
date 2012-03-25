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


    public partial class Informes : UserControl
    {
        MyWebReference.clinicaPatologiaWebServiceSoapClient myWebReference;
        String usuario;
        int anio = 0;
        int mes = 0;
        int totalCitologias = 0;
        int totalBiopsias = 0;

        public Informes(MyWebReference.clinicaPatologiaWebServiceSoapClient myWebReference, String usuario)
        {
            this.myWebReference = myWebReference;
            this.usuario = usuario;
            InitializeComponent();

            for (int i = 2010; i < 2101; i++)
            {
                medicoAno.Items.Add(i);
            }
            for (int i = 0; i < 13; i++)
            {
                medicoMes.Items.Add(i);
            }
            medicoMes.SelectedIndex = 0;
            scrollViewer1.Height = Application.Current.Host.Content.ActualHeight - 15;
        }

        private void paginaPrincipal_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new mainPage(myWebReference, usuario);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                totalBiopsias = 0;
                totalCitologias = 0;
                totalCitologias_txt.Text = "";
                totalBiopsias_txt.Text = "";
                busquedaStatus.Content = "Obteniendo datos...";
                button1.IsEnabled = false;
                if (medicoAno.SelectedItem.ToString().CompareTo("") != 0)
                {
                    anio = Convert.ToInt16(medicoAno.SelectedItem.ToString());
                }
                mes = Convert.ToInt16(medicoMes.SelectedItem.ToString());
                myWebReference.consultaMedicoBiopsiasCompleted
                    += new EventHandler<MyWebReference.consultaMedicoBiopsiasCompletedEventArgs>(WebS_consultaMedicoBiopsia);

                myWebReference.consultaMedicoBiopsiasAsync(anio, mes);
            }
            catch (Exception exp)
            {
                busquedaStatus.Content = "Ingrese el Año y/o Mes";
                button1.IsEnabled = true;
            }
        }

        void WebS_consultaMedicoBiopsia(object sender, MyWebReference.consultaMedicoBiopsiasCompletedEventArgs e)
        {
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
                    totalBiopsias += Convert.ToInt16(content[i + 2]);
                }
            }

            listadoMedicosBiopsia.ItemsSource = valores;
            totalBiopsias_txt.Text = totalBiopsias.ToString();
            totalBiopsias = 0;

            myWebReference.consultaMedicoCitologiaCompleted += new EventHandler<MyWebReference.consultaMedicoCitologiaCompletedEventArgs>(WebS_consultaMedicoCitologia);
            myWebReference.consultaMedicoCitologiaAsync(anio, mes);
        }

        void WebS_consultaMedicoCitologia(object sender, MyWebReference.consultaMedicoCitologiaCompletedEventArgs e)
        {
            button1.IsEnabled = true;
            busquedaStatus.Content = "Busqueda Finalizada";
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
                    totalCitologias += Convert.ToInt16(content[i + 2]);
                }
            }

            listadoMedicosCitologia.ItemsSource = valores;
            totalCitologias_txt.Text = totalCitologias.ToString();
            totalCitologias = 0;
        }

        private void buscar_btn_Click(object sender, RoutedEventArgs e)
        {
            buscar_btn.IsEnabled = false;
            dataGrid1.ItemsSource = null;
            String tabla = "Examen";
            if (radioButton1.IsChecked == true)
                tabla = "Citologia";
            if (radioButton2.IsChecked == true)
                tabla = "Biopsia";

            muestra_txt.Content = tabla;
            categoria_txt.Content = ((ComboBoxItem)(comboBox1.SelectedItem)).Content.ToString();
            rangoEdad_txt.Content = ((ComboBoxItem)(rangoEdadCombo.SelectedItem)).Content.ToString();

            myWebReference.getCantidadDeExamenesCompleted +=
                new EventHandler<MyWebReference.getCantidadDeExamenesCompletedEventArgs>(WebS_CantidadExamenes);
            myWebReference.getCantidadDeExamenesAsync(tabla, ((ComboBoxItem)(rangoEdadCombo.SelectedItem)).Content.ToString(),
                getFechaEnFormatoMysql(fechaMuestra.Text), getFechaEnFormatoMysql(datePicker1.Text), ((ComboBoxItem)(comboBox1.SelectedItem)).Content.ToString());
            estado.Content = "Esperando Respuesta...";
        }

        void WebS_CantidadExamenes(object sender, MyWebReference.getCantidadDeExamenesCompletedEventArgs e)
        {

            buscar_btn.IsEnabled = true;
            estado.Content = "Datos Recibidos!";
            String tabla = "Examen";
            if (radioButton1.IsChecked == true)
                tabla = "Citologia";
            if (radioButton2.IsChecked == true)
                tabla = "Biopsia";
            if (e.Result.ToString().CompareTo("-1") != 0)
            {
                textBox5.Text = e.Result.ToString();
                estadoMuestras.Content = "Cargando Datos...";
                myWebReference.getExamenesFiltradosCompleted += new EventHandler<MyWebReference.getExamenesFiltradosCompletedEventArgs>(myWebReference_getExamenesFiltradosCompleted);
                myWebReference.getExamenesFiltradosAsync(tabla, ((ComboBoxItem)(rangoEdadCombo.SelectedItem)).Content.ToString(),
                    getFechaEnFormatoMysql(fechaMuestra.Text), getFechaEnFormatoMysql(datePicker1.Text), ((ComboBoxItem)(comboBox1.SelectedItem)).Content.ToString());
            }
            else
                estado.Content = "No se recibio Respuesta...";
        }

        void myWebReference_getExamenesFiltradosCompleted(object sender, MyWebReference.getExamenesFiltradosCompletedEventArgs e)
        {
            if (e.Result.ToString().CompareTo("") != 0)
            {
                estadoMuestras.Content = "Datos recibidos!";
                string[] content;
                content = e.Result.Split(';');
                List<dataExamen> valores = new List<dataExamen>();
                valores.Capacity = content.Length;
                for (int i = 0; i < content.Length - 1; i++)
                {
                    if (i % 12 == 0)
                    {
                        valores.Add(new dataExamen()
                        {
                            Muestra = content[i],
                            Nombre = content[i + 1] + " " + content[i + 2],
                            Apellido = content[i + 3] + " " + content[i + 4],
                            Edad = content[i + 5],
                            Expediente = content[i + 6],
                            Medico = content[i + 7],
                            Precio = content[i + 8],
                            FechaCalendario = content[i + 9],
                            Fecha = content[i + 10],
                            Registrado_Por = content[i + 11],
                        });
                    }
                }

                dataGrid1.ItemsSource = valores;
            }
            else
            {
                estadoMuestras.Content = "No se encontraron muestras";
                dataGrid1.ItemsSource = null;
            }
        }

        private string getFechaEnFormatoMysql(String fecha)
        {
            if (fecha.CompareTo("") == 0)
                return "";
            String[] split = fecha.Split('/');
            return split[2] + "-" + split[0] + "-" + split[1];
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            //comboBox1 = new ComboBox();
            comboBox1.SelectedIndex = 0;
            comboBox1.IsEnabled = false;
        }

        private void radioButton2_Unchecked(object sender, RoutedEventArgs e)
        {
            comboBox1.IsEnabled = true;
        }
    }

}
