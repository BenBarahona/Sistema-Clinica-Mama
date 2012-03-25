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
    public partial class Contabilidad : UserControl
    {
        MyWebReference.clinicaPatologiaWebServiceSoapClient myWebReference;
        String Usuario;
        int anio = 0;
        int mes = 0;
        int sumaEgresos = 0;
        int totalIngresosBiopsia = 0;
        int totalMuestrasBiopsia = 0;
        int totalIngresosCitologia = 0;
        int totalMuestrasCitologia = 0;

        //flags
        bool getMedicoBiopsia = true;
        bool getMedicoCitologia = true;

        public Contabilidad(MyWebReference.clinicaPatologiaWebServiceSoapClient myWebReference, String Usuario)
        {
            InitializeComponent();
            this.myWebReference = myWebReference;
            this.Usuario = Usuario;
            scrollViewer1.Height = Application.Current.Host.Content.ActualHeight - 15;

            for (int i = 2010; i < 2101; i++)
            {
                anoEgreso_combo.Items.Add(i);
                anoInsertarE_combo.Items.Add(i);
                medicoAno.Items.Add(i);
            }
            for (int i = 0; i < 13; i++)
            {
                mesEgreso_combo.Items.Add(i);
                mesInsertarE_combo.Items.Add(i);
                medicoMes.Items.Add(i);
            }
            medicoMes.SelectedIndex = 0;
            mesEgreso_combo.SelectedIndex = 0;
        }

        private void verCategorias_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (anoInsertarE_combo.SelectedItem.ToString().CompareTo("") == 0)
                {
                    estadoInsertarEgreso.Content = "Seleccione un Año";
                    return;
                }
                if (mesInsertarE_combo.SelectedItem.ToString().CompareTo("") == 0)
                {
                    estadoInsertarEgreso.Content = "Seleccione un Mes";
                    return;
                }
            }
            catch (Exception exp)
            {
                estadoInsertarEgreso.Content = "Seleccione un Año y Mes";
                return;
            }
            varCategorias_btn.IsEnabled = false;
            registrarCat_btn.IsEnabled = false;
            registrarGasto_btn.IsEnabled = false;
            categoriasExistentes_combo.IsEnabled = false;
            estadoInsertarEgreso.Content = "Obteniendo Categorías...";
            myWebReference.getEgresosPorMesCompleted
                += new EventHandler<MyWebReference.getEgresosPorMesCompletedEventArgs>(WebS_getEgresospormes);

            myWebReference.getEgresosPorMesAsync(Convert.ToInt16(anoInsertarE_combo.SelectedItem),
                Convert.ToInt16(mesInsertarE_combo.SelectedItem));
        }

        void WebS_getEgresospormes(Object s, MyWebReference.getEgresosPorMesCompletedEventArgs e)
        {
            varCategorias_btn.IsEnabled = true;
            registrarCat_btn.IsEnabled = true;
            registrarGasto_btn.IsEnabled = true;
            categoriasExistentes_combo.IsEnabled = true;
            nombreGasto_txt.IsEnabled = true;
            valorGasto_txt.IsEnabled = true;
            nombreCat_txt.IsEnabled = true;
            diaGasto_txt.IsEnabled = true;
            nombreCat_txt.IsEnabled = true;

            estadoInsertarEgreso.Content = "Datos Recibidos";
            String[] resultado = e.Result.ToString().Split(';');

            for (int i = categoriasExistentes_combo.Items.Count - 1; i > -1; i--)
                categoriasExistentes_combo.Items.RemoveAt(i);

            for (int i = 0; i < resultado.Length; i++)
                categoriasExistentes_combo.Items.Add(resultado[i]);

        }

        private void registrarCat_btn_Click(object sender, RoutedEventArgs e)
        {
            if (nombreCat_txt.Text.CompareTo("") == 0)
            {
                estadoInsertarEgreso.Content = "Ingrese un Nombre";
                return;
            }

            nombreCat_txt.IsEnabled = false;
            varCategorias_btn.IsEnabled = false;
            registrarCat_btn.IsEnabled = false;
            registrarGasto_btn.IsEnabled = false;
            categoriasExistentes_combo.IsEnabled = false;
            estadoInsertarEgreso.Content = "Registrando Categoría...";
            myWebReference.InsertarEgresoCompleted
                += new EventHandler<MyWebReference.InsertarEgresoCompletedEventArgs>(WebS_egresoinsertado);
            myWebReference.InsertarEgresoAsync(nombreCat_txt.Text, Convert.ToInt16(anoInsertarE_combo.SelectedItem),
                Convert.ToInt16(mesInsertarE_combo.SelectedItem), 0);

        }

        void WebS_egresoinsertado(Object s, MyWebReference.InsertarEgresoCompletedEventArgs e)
        {
            if (e.Result)
            {

                estadoInsertarEgreso.Content = "Actualizando Categorías...";
                myWebReference.getEgresosPorMesCompleted
                    += new EventHandler<MyWebReference.getEgresosPorMesCompletedEventArgs>(WebS_getEgresospormes);

                myWebReference.getEgresosPorMesAsync(Convert.ToInt16(anoInsertarE_combo.SelectedItem),
                    Convert.ToInt16(mesInsertarE_combo.SelectedItem));
            }
            else
            {
                estadoInsertarEgreso.Content = "Hubo un problema al registrar la Categoría...";
                nombreCat_txt.IsEnabled = true;
                varCategorias_btn.IsEnabled = true;
                registrarCat_btn.IsEnabled = true;
                registrarGasto_btn.IsEnabled = true;
                categoriasExistentes_combo.IsEnabled = true;
                nombreGasto_txt.IsEnabled = true;
                valorGasto_txt.IsEnabled = true;
                nombreCat_txt.IsEnabled = true;
                diaGasto_txt.IsEnabled = true;
            }
        }

        private void registrarGasto_btn_Click(object sender, RoutedEventArgs e)
        {
            if (categoriasExistentes_combo.SelectedItem.ToString().CompareTo("") == 0)
            {
                estadoInsertarEgreso.Content = "Seleccione una categoría";
                return;
            }

            if (nombreGasto_txt.Text.CompareTo("") == 0)
            {
                estadoInsertarEgreso.Content = "Ingrese un Nombre";
                return;
            }

            if (valorGasto_txt.Text.CompareTo("") == 0)
            {
                estadoInsertarEgreso.Content = "Ingrese un Valor";
                return;
            }

            if (diaGasto_txt.Text.CompareTo("") == 0)
            {
                estadoInsertarEgreso.Content = "Ingrese el Día";
                return;
            }

            try
            {
                int x = Convert.ToInt16(valorGasto_txt.Text);
            }
            catch (Exception ex)
            {
                estadoInsertarEgreso.Content = "Valor no valido...";
                return;
            }

            try
            {
                int x = Convert.ToInt16(diaGasto_txt.Text);
            }
            catch (Exception ex)
            {
                estadoInsertarEgreso.Content = "Día no valido...";
                return;
            }

            if (Convert.ToInt16(diaGasto_txt.Text) > 31 || Convert.ToInt16(diaGasto_txt.Text) < 1)
            {
                estadoInsertarEgreso.Content = "Día no valido...";
                return;
            }

            estadoInsertarEgreso.Content = "Registrando Gasto...";
            myWebReference.InsertarGastoenEgresoCompleted
                    += new EventHandler<MyWebReference.InsertarGastoenEgresoCompletedEventArgs>(WebS_gastoinsert);
            myWebReference.InsertarGastoenEgresoAsync(categoriasExistentes_combo.SelectedItem.ToString(), Convert.ToInt16(anoInsertarE_combo.SelectedItem),
                Convert.ToInt16(mesInsertarE_combo.SelectedItem), nombreGasto_txt.Text, Convert.ToInt16(diaGasto_txt.Text), Convert.ToInt16(valorGasto_txt.Text));
        }

        void WebS_gastoinsert(Object s, MyWebReference.InsertarGastoenEgresoCompletedEventArgs e)
        {
            if (e.Result)
                estadoInsertarEgreso.Content = "Gasto Registrado";
            else
                estadoInsertarEgreso.Content = "hubo un problema al registrar el gasto...";
        }

        private void paginaPrincipal_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new mainPage(this.myWebReference, this.Usuario);
        }

        private void consultaEgresos_btn_Click(object sender, RoutedEventArgs e)
        {
            sumaEgresos = 0;
            sumaEgresos_txt.Text = "";
            try
            {
                consultaEgresos_btn.IsEnabled = false;
                if (anoEgreso_combo.SelectedItem.ToString().CompareTo("") == 0)
                {
                    estado_egreso.Content = "Seleccione un Año";
                    consultaEgresos_btn.IsEnabled = true;
                    return;
                }
                if (mesEgreso_combo.SelectedItem.ToString().CompareTo("") == 0)
                {
                    estado_egreso.Content = "Seleccione un Mes";
                    consultaEgresos_btn.IsEnabled = true;
                    return;
                }


                estado_egreso.Content = "Obteniendo Datos...";
                myWebReference.getTodosLosGastosCompleted +=
                    new EventHandler<MyWebReference.getTodosLosGastosCompletedEventArgs>(WebS_getGastos);
                myWebReference.getTodosLosGastosAsync(Convert.ToInt16(anoEgreso_combo.SelectedItem),
                    Convert.ToInt16(mesEgreso_combo.SelectedItem), ((ComboBoxItem)(egresosCategoria_combo.SelectedItem)).Content.ToString());
            }
            catch (Exception exp)
            {
                estado_egreso.Content = "Seleccione un Mes y Año";
                consultaEgresos_btn.IsEnabled = true;
            }
        }

        void WebS_getGastos(Object s, MyWebReference.getTodosLosGastosCompletedEventArgs e)
        {
            consultaEgresos_btn.IsEnabled = true;
            estado_egreso.Content = "Datos Obtenidos!";
            string[] content;
            content = e.Result.Split(';');
            List<dataEgreso> valores = new List<dataEgreso>();
            valores.Capacity = content.Length;
            for (int i = 0; i < content.Length - 1; i++)
            {
                if (i % 3 == 0)
                {
                    valores.Add(new dataEgreso()
                    {
                        Categoria = content[i],
                        Nombre = content[i + 1],
                        Valor = content[i + 2]
                    });
                    sumaEgresos += Convert.ToInt16(content[i + 2]);
                }
            }

            tablaEgresos.ItemsSource = valores;
            sumaEgresos_txt.Text = sumaEgresos.ToString();
            sumaEgresos = 0;
        }

        private void consultaIngresos_btn_Click(object sender, RoutedEventArgs e)
        {
            if (fechaInicial.Text == "" || fechaFinal.Text == "")
            {
                busquedaStatus.Content = "Escoga el rango de fechas";
                return;
            }
            consultaIngresos_btn.IsEnabled = false;
            fechaFinal.IsEnabled = false;
            fechaInicial.IsEnabled = false;
            myWebReference.getIngresosCitologiaCompleted += new EventHandler<MyWebReference.getIngresosCitologiaCompletedEventArgs>(myWebReference_getIngresosCitologiaCompleted);
            myWebReference.getIngresosCitologiaAsync(getFechaEnFormatoMysql(fechaInicial.Text), getFechaEnFormatoMysql(fechaFinal.Text));

            busquedaStatus.Content = "Esperando Respuesta...";
        }

        void myWebReference_getIngresosCitologiaCompleted(object sender, MyWebReference.getIngresosCitologiaCompletedEventArgs e)
        {

            if (e.Result.ToString().CompareTo("-1") != 0)
                ingresosCitologia.Text = e.Result.ToString();
            else
                busquedaStatus.Content = "No se recibio Respuesta...";

            myWebReference.getIngresosBiopsiaCompleted += new EventHandler<MyWebReference.getIngresosBiopsiaCompletedEventArgs>(myWebReference_getIngresosBiopsiaCompleted);
            myWebReference.getIngresosBiopsiaAsync(getFechaEnFormatoMysql(fechaInicial.Text), getFechaEnFormatoMysql(fechaFinal.Text));
        }

        void myWebReference_getIngresosBiopsiaCompleted(object sender, MyWebReference.getIngresosBiopsiaCompletedEventArgs e)
        {
            consultaIngresos_btn.IsEnabled = true;
            fechaFinal.IsEnabled = true;
            fechaInicial.IsEnabled = true;
            busquedaStatus.Content = "Datos Recibidos!";
            if (e.Result.ToString().CompareTo("-1") != 0)
                ingresoBiopsia.Text = e.Result.ToString();
            else
                busquedaStatus.Content = "No se recibio Respuesta...";
        }

        private string getFechaEnFormatoMysql(String fecha)
        {
            if (fecha.CompareTo("") == 0)
                return "";
            String[] split = fecha.Split('/');
            return split[2] + "-" + split[0] + "-" + split[1];
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            getMedicoBiopsia = false;
            getMedicoCitologia = false;
            totalIngresosBiopsia = 0;
            totalMuestrasBiopsia = 0;
            totalIngresosCitologia = 0;
            totalMuestrasCitologia = 0;
            totalCitologias_txt.Text = "";
            totalBiopsias_txt.Text = "";
            ingresosCitologia_txt.Text = "";
            ingresosBiopsias_txt.Text = "";

            try
            {
                estadoMedico.Content = "Obteniendo datos...";
                button1.IsEnabled = false;
                if (medicoAno.SelectedItem.ToString().CompareTo("") != 0)
                {
                    anio = Convert.ToInt16(medicoAno.SelectedItem.ToString());
                }
                mes = Convert.ToInt16(medicoMes.SelectedItem.ToString());
                if (!getMedicoBiopsia)
                {
                    getMedicoBiopsia = true;
                    myWebReference.consultaMedicoBiopsiasCompleted
                        += new EventHandler<MyWebReference.consultaMedicoBiopsiasCompletedEventArgs>(WebS_consultaMedicoBiopsia);
                    myWebReference.consultaMedicoBiopsiasAsync(anio, mes);
                }
            }
            catch (Exception exp)
            {
                estadoMedico.Content = "Ingrese el Mes y Año";
                button1.IsEnabled = true;
            }
        }

        void WebS_consultaMedicoBiopsia(object sender, MyWebReference.consultaMedicoBiopsiasCompletedEventArgs e)
        {
            string[] content;
            content = e.Result.Split(';');
            List<dataMedico> valores = new List<dataMedico>();
            valores.Capacity = content.Length;
            for (int i = 0; i < content.Length - 1; i++)
            {
                if (i % 3 == 0)
                {
                    valores.Add(new dataMedico()
                    {
                        Medico = content[i],
                        Ingresos = content[i + 1],
                        Muestras = content[i + 2]
                    });
                    totalIngresosBiopsia += Convert.ToInt16(content[i + 1]);
                    totalMuestrasBiopsia += Convert.ToInt16(content[i + 2]);
                }
            }

            listadoMedicosBiopsia.ItemsSource = valores;
            ingresosBiopsias_txt.Text = totalIngresosBiopsia.ToString();
            totalBiopsias_txt.Text = totalMuestrasBiopsia.ToString();
            totalMuestrasBiopsia = 0;
            totalIngresosBiopsia = 0;

            if (!getMedicoCitologia)
            {
                getMedicoCitologia = true;
                myWebReference.consultaMedicoCitologiaCompleted += new EventHandler<MyWebReference.consultaMedicoCitologiaCompletedEventArgs>(WebS_consultaMedicoCitologia);
                myWebReference.consultaMedicoCitologiaAsync(anio, mes);
            }
        }

        void WebS_consultaMedicoCitologia(object sender, MyWebReference.consultaMedicoCitologiaCompletedEventArgs e)
        {
            button1.IsEnabled = true;
            estadoMedico.Content = "Busqueda Finalizada";
            string[] content;
            content = e.Result.Split(';');
            List<dataMedico> valores = new List<dataMedico>();
            valores.Capacity = content.Length;
            for (int i = 0; i < content.Length - 1; i++)
            {
                if (i % 3 == 0)
                {
                    valores.Add(new dataMedico()
                    {
                        Medico = content[i],
                        Ingresos = content[i + 1],
                        Muestras = content[i + 2]
                    });
                    totalIngresosCitologia += Convert.ToInt16(content[i + 1]);
                    totalMuestrasCitologia += Convert.ToInt16(content[i + 2]);
                }
            }

            listadoMedicosCitologia.ItemsSource = valores;
            ingresosCitologia_txt.Text = totalIngresosCitologia.ToString();
            totalCitologias_txt.Text = totalMuestrasCitologia.ToString();
            totalMuestrasCitologia = 0;
            totalIngresosCitologia = 0;
        }
    }
}
