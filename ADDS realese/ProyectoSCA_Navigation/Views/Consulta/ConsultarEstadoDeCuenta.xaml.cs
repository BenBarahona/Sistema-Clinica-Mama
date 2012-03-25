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
using ProyectoSCA_Navigation.Clases.Clases_Para_los_Grids;

namespace ProyectoSCA_Navigation.Views
{
    public partial class ConsultarEstadoDeCuenta : Page
    {
        string usuario = App.Correo;

        //Flags, true, puede disparar el evento, false no
        bool[] flags = new bool[10];
        /***    0 - Abrir Inserts
         ***    1 - Abrir Gets, luego Llenar campos de datos
         ***    2 - Error Windows
         ***/
        Afiliado afiliado = new Afiliado();
        List<estadoDeCuentaGrid> aportaciones = new List<estadoDeCuentaGrid>();
        DateTime fecha = DateTime.Now;

        /********************************************************************************************************************************
        **************************************************    Inicializadores   *********************************************************
        *********************************************************************************************************************************/

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private ProyectoSCA_Navigation.ServiceReference.WSCosecolSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:1809/WSCosecol.asmx";

        public ConsultarEstadoDeCuenta()
        {
            InitializeComponent();
            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReference.WSCosecolSoapClient(bind, endpoint);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string afiliadoID = NavigationContext.QueryString["ID"];
            string[] afiliadoCorreo = afiliadoID.Split('$');

            for (int i = 0; i < flags.Length; i++)
            {
                flags[i] = false;
            }
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            habilitarCampos(false);
            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getAfiliado, afiliadoCorreo[1]);
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
                        case "p21"://Get datos Afiliado
                            if (flags[0])
                            {
                                flags[0] = false;
                                if (recievedResponce != "")
                                {
                                    afiliado = MainPage.tc.getAfiliado(recievedResponce);

                                    numeroCertificado_txt.Text = afiliado.certificadoCuenta.ToString();
                                    nombre_txt.Text = afiliado.primerNombre + " " + afiliado.segundoNombre + " " +
                                        afiliado.primerApellido + " " + afiliado.segundoApellido;
                                    id_txt.Text = afiliado.identidad;
                                }
                                else
                                {
                                    MessageBox.Show("No se ha encontrado el registro!");
                                }
                            }
                            break;

                        case "p32"://Get Estado de Cuenta
                            if (flags[0])
                            {
                                flags[0] = false;
                                if (recievedResponce == "")
                                {
                                    MessageBox.Show("El afiliado no tiene aportaciones pagadas en ese rango de fechas");
                                    return;
                                }

                                aportaciones = MainPage.tc.getEstadoCuenta(recievedResponce);
                                gridEstadoDeCuenta.ItemsSource = aportaciones;
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
            if (fechaDesde_txt.Text == "" || fechaHasta_txt.Text == "")
            {
                MessageBox.Show("Ingrese un rango de fecha! Formato: Mes/Dia/Año");
                return;
            }

            habilitarCampos(false);
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.consultarEstadoDeCuenta, MainPage.tc.enviarAportacionesConsulas(numeroCertificado_txt.Text,
                    fechaDesde_txt.Text, fechaHasta_txt.Text));
        }

        private void exportarReporte_btn_Click(object sender, RoutedEventArgs e)
        {
            if (gridEstadoDeCuenta.ItemsSource == null)
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
                foreach (DataGridColumn dgcol in gridEstadoDeCuenta.Columns)
                {
                    worksheet.Cells[0, ColumnCount] = new Cell(dgcol.Header.ToString());
                    ColumnCount++;
                }

                //Extracting values from grid and writing to excell sheet
                //
                foreach (object data in gridEstadoDeCuenta.ItemsSource)
                {
                    ColumnCount = 0;
                    RowCount++;
                    foreach (DataGridColumn col in gridEstadoDeCuenta.Columns)
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
            fechaDesde_txt.IsEnabled = valor;
            fechaHasta_txt.IsEnabled = valor;
            aplicar_btn.IsEnabled = valor;
            exportarReporte_btn.IsEnabled = valor;
        }
    }
}
