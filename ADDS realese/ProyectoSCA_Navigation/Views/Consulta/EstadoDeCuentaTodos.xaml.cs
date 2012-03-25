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
using ProyectoSCA_Navigation.Clases;
using Lite.ExcelLibrary.CompoundDocumentFormat;
using Lite.ExcelLibrary.BinaryFileFormat;
using Lite.ExcelLibrary.SpreadSheet;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Collections;
using System.Reflection;

namespace ProyectoSCA_Navigation.Views.Consulta
{
    public partial class EstadoDeCuentaTodos : Page
    {
        string usuario = App.Correo;

        //Flags, true, puede disparar el evento, false no
        bool[] flags = new bool[10];
        /***    0 - Abrir Inserts
         ***    1 - Abrir Gets, luego Llenar campos de datos
         ***    2 - Error Windows
         ***/
        string afiliadosSaldos = "";

        List<String> certificados = new List<String>();
        List<reporteEstadoCuentaTodosGrid> reporteAportaciones = new List<reporteEstadoCuentaTodosGrid>();
        List<reporteTodosSaldosGrid> reporteSaldos = new List<reporteTodosSaldosGrid>();

        Afiliado afiliado = new Afiliado();
        Empleado empleado = new Empleado();

        /********************************************************************************************************************************
        **************************************************    Inicializadores   *********************************************************
        *********************************************************************************************************************************/

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private ProyectoSCA_Navigation.ServiceReference.WSCosecolSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:1809/WSCosecol.asmx";
        public EstadoDeCuentaTodos()
        {
            InitializeComponent();
            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReference.WSCosecolSoapClient(bind, endpoint);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            tipoAportacion_txt.Items.Add("Todas Las Aportaciones");
            tipoAportacion_txt.Items.Add("Aportaciones Obligatorias Normales");
            tipoAportacion_txt.Items.Add("Aportaciones Obligatorias Especiales");
            tipoAportacion_txt.Items.Add("Aportaciones Obligatorias");
            tipoAportacion_txt.Items.Add("Aportaciones Voluntarias");
            tipoAportacion_txt.SelectedIndex = 0;

            afiliadosSaldos = NavigationContext.QueryString["IDS"];
            string[] afiliadoID = NavigationContext.QueryString["IDS"].Split(',');

            certificados = afiliadoID.ToList<String>();

            for (int i = 0; i < flags.Length; i++)
            {
                flags[i] = false;
            }
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
                    }
                }
                else
                {
                    switch (temp)
                    {
                        case "p42"://Reporte Aportaciones
                            if (flags[0])
                            {
                                flags[0] = false;
                                if (recievedResponce == "")
                                {
                                    MessageBox.Show("No existen aportaciones pagadas en ese rango de fechas");
                                    return;
                                }

                                reporteAportaciones = MainPage.tc.getReporteEstadoCuentaTodos(recievedResponce);
                                gridResultado.ItemsSource = reporteAportaciones;
                            }
                            break;

                        case "p43"://Reporte Saldos
                            if (flags[0])
                            {
                                flags[0] = false;
                                if (recievedResponce == "")
                                {
                                    MessageBox.Show("No existen afiliados en el sistema para mostrar saldos");
                                    return;
                                }

                                reporteSaldos = MainPage.tc.getReporteTodosLosSaldos(recievedResponce);
                                gridResultado.ItemsSource = reporteSaldos;
                            }
                            break;

                        default:
                            if (flags[2])
                            {
                                flags[2] = false;
                                MessageBox.Show("No se entiende la peticion. (No esta definida)");
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
                }
            }
            habilitarCampos(true);
        }


        /********************************************************************************************************************************
        **************************************************    Metodos de Botones    *****************************************************
        *********************************************************************************************************************************/
        private void aplicar_btn_Click(object sender, RoutedEventArgs e)
        {
            bool? aportaciones = null;
            bool? saldos = null;

            if (!aportaciones.HasValue)
            {
                aportaciones = todasAportaciones_btn.IsChecked;
            }
            if (!saldos.HasValue)
            {
                saldos = saldosTotales_btn.IsChecked;
            }

            if ((fechaDesde_txt.Text == "" || fechaHasta_txt.Text == "") && !(bool)saldos)
            {
                MessageBox.Show("Ingrese un rango de fecha! Formato: Mes/Dia/Año");
                return;
            }

            string tipoAportacion = tipoAportacion_txt.SelectedItem.ToString();

            habilitarCampos(false);
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;

            if ((bool)aportaciones)
            {
                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.reporteAportaciones, MainPage.tc.getFiltrosEstadoCuentaTodos(certificados,
                tipoAportacion, fechaDesde_txt.Text, fechaHasta_txt.Text));
            }
            else if ((bool)saldos)
            {
                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.reporteSaldos, afiliadosSaldos);
            }
        }

        private void exportar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (gridResultado.ItemsSource == null)
            {
                MessageBox.Show("No hay datos para exportar");
                return;
            }
            // open file dialog to select an export file.   
            SaveFileDialog sDialog = new SaveFileDialog();
            sDialog.Filter = "Excel Files(*.xls)|*.xls";

            if (sDialog.ShowDialog() == true)
            {

                // create an instance of excel workbook
                Workbook workbook = new Workbook();
                // create a worksheet object
                Worksheet worksheet = new Worksheet("Estado de Cuenta");

                Int16 ColumnCount = 0;
                Int16 RowCount = 0;

                //Writing Column Names 
                foreach (DataGridColumn dgcol in gridResultado.Columns)
                {
                    worksheet.Cells[0, ColumnCount] = new Cell(dgcol.Header.ToString());
                    ColumnCount++;
                }

                //Extracting values from grid and writing to excell sheet
                //
                foreach (object data in gridResultado.ItemsSource)
                {
                    ColumnCount = 0;
                    RowCount++;
                    foreach (DataGridColumn col in gridResultado.Columns)
                    {

                        string strValue = "";
                        Binding objBinding = null;
                        if (col is DataGridBoundColumn)
                            objBinding = (col as DataGridBoundColumn).Binding;
                        if (col is DataGridTemplateColumn)
                        {
                            //This is a template column... let us see the underlying dependency object
                            DependencyObject objDO = (col as DataGridTemplateColumn).CellTemplate.LoadContent();
                            FrameworkElement oFE = (FrameworkElement)objDO;
                            FieldInfo oFI = oFE.GetType().GetField("TextProperty");
                            if (oFI != null)
                            {
                                if (oFI.GetValue(null) != null)
                                {
                                    if (oFE.GetBindingExpression((DependencyProperty)oFI.GetValue(null)) != null)
                                        objBinding = oFE.GetBindingExpression((DependencyProperty)oFI.GetValue(null)).ParentBinding;
                                }
                            }
                        }
                        if (objBinding != null)
                        {
                            if (objBinding.Path.Path != "")
                            {
                                PropertyInfo pi = data.GetType().GetProperty(objBinding.Path.Path);
                                if (pi != null) strValue = Convert.ToString(pi.GetValue(data, null));
                            }
                            if (objBinding.Converter != null)
                            {
                                if (strValue != "")
                                    strValue = objBinding.Converter.Convert(strValue, typeof(string), objBinding.ConverterParameter, objBinding.ConverterCulture).ToString();
                                //else
                                //    strValue = objBinding.Converter.Convert(data, typeof(string), objBinding.ConverterParameter, objBinding.ConverterCulture).ToString();
                            }
                        }
                        // writing extracted value in excell cell
                        worksheet.Cells[RowCount, ColumnCount] = new Cell(strValue);
                        ColumnCount++;
                    }
                }

                //add worksheet to workbook
                workbook.Worksheets.Add(worksheet);
                // get the selected file's stream
                Stream sFile = sDialog.OpenFile();
                workbook.Save(sFile);
            }
        }

        /********************************************************************************************************************************
        **************************************************    Metodos Auxiliares    *****************************************************
        *********************************************************************************************************************************/

        public void habilitarCampos(bool valor)
        {
            todasAportaciones_btn.IsEnabled = valor;
            saldosTotales_btn.IsEnabled = valor;
            tipoAportacion_txt.IsEnabled = valor;
            if (!(bool)saldosTotales_btn.IsChecked)
            {
                fechaDesde_txt.IsEnabled = valor;
                fechaHasta_txt.IsEnabled = valor;
            }
            aplicar_btn.IsEnabled = valor;
            exportar_btn.IsEnabled = valor;
        }

        private void saldosTotales_btn_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                fechaDesde_txt.Text = "";
                fechaHasta_txt.Text = "";
                fechaDesde_txt.IsEnabled = false;
                fechaHasta_txt.IsEnabled = false;
            }
            catch { }
        }

        private void todasAportaciones_btn_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                fechaDesde_txt.IsEnabled = true;
                fechaHasta_txt.IsEnabled = true;
            }
            catch { }
        }
    }
}
