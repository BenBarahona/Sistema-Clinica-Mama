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
using ProyectoSCA_Navigation.Clases;

namespace ProyectoSCA_Navigation.Views
{

    public partial class Parametrizacion : Page
    {
        BitmapImage bmpClear = new BitmapImage();
        //El usuario que esta logeado
        string usuario = App.Correo;

        //Flags, true, puede disparar el evento, false no
        bool[] flags = new bool[10];
        /***    0 - Abrir Inserts
         ***    1 - Abrir Gets, luego Llenar campos de datos
         ***    2 - Mensaje de Error
         ***/
        bool interesAportacionVoluntaria = false;

        List<string[]> ocupacion = new List<string[]>();
        List<string[]> parentesco = new List<string[]>();
        List<string[]> motivo = new List<string[]>();

        string interesViejo;

        //Expresiones Regulares para las validaciones
        Regex nombres = new Regex(@"^[A-Za-z][A-Za-zñ\s-]+$");
        Regex numeros = new Regex("^[0-9]+$");
        Regex alphanumerico = new Regex(@"^[\w\s]+$");
        Regex porcentaje = new Regex(@"^([1-9]|[1-9]\d|100)$");

        /********************************************************************************************************************************
        **************************************************    Inicializadores   *********************************************************
        *********************************************************************************************************************************/

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private ProyectoSCA_Navigation.ServiceReference.WSCosecolSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:1809/WSCosecol.asmx";

        //Clases.TC tc = new Clases.TC();


        public Parametrizacion()
        {
            InitializeComponent();

            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReference.WSCosecolSoapClient(bind, endpoint);

        }


        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DateTime fecha = DateTime.Now;
            aportacionMes_txt.Items.Add("Enero");
            aportacionMes_txt.Items.Add("Febrero");
            aportacionMes_txt.Items.Add("Marzo");
            aportacionMes_txt.Items.Add("Abril");
            aportacionMes_txt.Items.Add("Mayo");
            aportacionMes_txt.Items.Add("Junio");
            aportacionMes_txt.Items.Add("Julio");
            aportacionMes_txt.Items.Add("Agosto");
            aportacionMes_txt.Items.Add("Septiembre");
            aportacionMes_txt.Items.Add("Octubre");
            aportacionMes_txt.Items.Add("Noviembre");
            aportacionMes_txt.Items.Add("Diciembre");


            for (int i = fecha.Year; i <= 2030; i++)
            {
                aportacionAno_txt.Items.Add(i);
            }

            profesionAgregar_btn.IsEnabled = false;
            profesionEliminar_btn.IsEnabled = false;
            profesion_valid.Source = bmpClear;
            //los botones y campos estan deshabilitados al principio.  Se abilitan cuando la peticion de inicio (get lo q sea) se complete.
            habilitarCampos(false, "profesion");

            for (int i = 0; i < flags.Length; i++)
            {
                flags[i] = false;
            }

            flags[1] = true;
            flags[2] = true;
            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getOcupacion, "");
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
                        habilitarTodaPantalla();
                    }
                }
                else
                {
                    switch (temp)
                    {
                        case "p02"://Ingresar Ocupacion
                            if (flags[0])
                            {
                                //MessageBox.Show(e.Result);

                                flags[0] = false;
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getOcupacion, "");
                            }
                            break;

                        case "p03"://Insertar Motivo
                            if (flags[0])
                            {
                                //MessageBox.Show(e.Result);

                                flags[0] = false;
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getMotivos, "");
                            }
                            break;

                        case "p04"://Insertar Parentesco
                            if (flags[0])
                            {
                                //MessageBox.Show(e.Result);

                                flags[0] = false;
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getParentesco, "");
                            }
                            break;

                        case "p05": //Insertar Monto
                            if (flags[0])
                            {
                                //MessageBox.Show(e.Result);

                                flags[0] = false;
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getMontoActual, "");
                            }
                            break;

                        case "p06": //Insertar Nuevo Limite
                            if (flags[0])
                            {
                                //MessageBox.Show(e.Result);

                                flags[0] = false;
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getLimiteActual, "");
                            }
                            break;

                        case "p07"://Insertar Interes
                            if (flags[0])
                            {
                                //MessageBox.Show(e.Result);

                                flags[0] = false;
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getIntereses, "");
                            }
                            break;

                        case "p10"://Get Ocupaciones, refrescamos el monto actual
                            if (flags[1])
                            {
                                //MessageBox.Show(e.Result);
                                List<profesionGrid> listaProfesion = new List<profesionGrid>();
                                flags[1] = false;
                                profesion_txt.Text = "";
                                profesionModificar_txt.Text = "";
                                profesionNuevo_txt.Text = "";
                                habilitarCampos(true, "profesion");
                                ocupacion = MainPage.tc.getOcupacion(recievedResponce);
                                if (ocupacion != null)
                                    for (int i = 0; i < ocupacion.Count; i++)
                                    {
                                        listaProfesion.Add(new profesionGrid()
                                        {
                                            Nombre = (ocupacion[i])[0],
                                            Valido = Boolean.Parse((ocupacion[i])[1])
                                        });
                                    }
                                profesion_grid.ItemsSource = listaProfesion;
                            }
                            break;

                        case "p11"://Get Motivos de Aportacion
                            if (flags[1])
                            {
                                //MessageBox.Show(e.Result);
                                flags[1] = false;
                                if (tabsParametrizable.SelectedIndex == 1)
                                {
                                    aportacionMotivo_txt.Items.Clear();
                                    motivo = MainPage.tc.getOcupacion(recievedResponce);
                                    if (motivo != null)
                                        for (int i = 0; i < motivo.Count; i++)
                                        {
                                            if((motivo[i])[1] == "True")
                                                aportacionMotivo_txt.Items.Add((motivo[i])[0]);
                                        }
                                }
                                else
                                {
                                    motivo_txt.Text = "";
                                    motivoViejo_txt.Text = "";
                                    motivoNuevo_txt.Text = "";
                                    habilitarCampos(true, "motivo");
                                    List<profesionGrid> lista = new List<profesionGrid>();
                                    motivo = MainPage.tc.getOcupacion(recievedResponce);
                                    if (motivo != null)
                                        for (int i = 0; i < motivo.Count; i++)
                                        {
                                            lista.Add(new profesionGrid()
                                            {
                                                Nombre = (motivo[i])[0],
                                                Valido = Boolean.Parse((motivo[i])[1])
                                            });

                                        }
                                    motivo_grid.ItemsSource = lista;
                                }
                            }
                            break;

                        case "p12"://Get Parentescos
                            if (flags[1])
                            {
                                //MessageBox.Show(e.Result);

                                flags[1] = false;
                                parentesco_txt.Text = "";
                                parentescoNuevo_txt.Text = "";
                                parentescoViejo_txt.Text = "";
                                habilitarCampos(true, "parentesco");

                                List<profesionGrid> lista = new List<profesionGrid>();
                                parentesco = MainPage.tc.getOcupacion(recievedResponce);
                                if (parentesco != null)
                                    for (int i = 0; i < parentesco.Count; i++)
                                    {
                                        lista.Add(new profesionGrid()
                                        {
                                            Nombre = (parentesco[i])[0],
                                            Valido = Boolean.Parse((parentesco[i])[1])
                                        });
                                    }
                                parentesco_grid.ItemsSource = lista;
                            }
                            break;

                        case "p13"://Get Monto Actual (de la aportacion obligatoria)
                            if (flags[1])
                            {
                                //MessageBox.Show(e.Result);

                                flags[1] = false;
                                habilitarCampos(true, "monto");

                                nuevoMonto_txt.Text = "";
                                string respuesta = recievedResponce;
                                montoActual_txt.Text = respuesta;
                            }
                            break;

                        case "p14"://Get Limite Actual de la aportacion
                            if (flags[1])
                            {
                                //MessageBox.Show(e.Result);

                                flags[1] = false;
                                habilitarCampos(true, "limite");

                                nuevoLimite_txt.Text = "";
                                string respuesta = recievedResponce;
                                limiteActual_txt.Text = respuesta;
                            }
                            break;

                        case "p15"://Get Intereses
                            if (flags[1])
                            {
                                //MessageBox.Show(e.Result);

                                flags[1] = false;
                                habilitarCampos(true, "interes");

                                interesNombre_txt.Text = "";
                                interesPorcentaje_txt.Text = "";
                                interesAplica1_chk.IsChecked = false;
                                interesAplica2_chk.IsChecked = false;
                                interesAplica3_chk.IsChecked = false;
                                interesAplica4_chk.IsChecked = false;
                                interesAportacionAplica_txt.Text = "";

                                interes_grid.ItemsSource = MainPage.tc.getIntereses(recievedResponce);
                            }
                            break;

                        case "p17"://Eliminar Ocupacion
                            if (flags[0])
                            {
                                //MessageBox.Show(e.Result);

                                flags[0] = false;
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getOcupacion, "");
                            }
                            break;

                        case "p18"://Eliminar Motivo
                            if (flags[0])
                            {
                                //MessageBox.Show(e.Result);

                                flags[0] = false;
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getMotivos, "");
                            }
                            break;

                        case "p19"://Eliminar Parentesco
                            if (flags[0])
                            {
                                //MessageBox.Show(e.Result);

                                flags[0] = false;
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getParentesco, "");
                            }
                            break;

                        case "p20"://Eliminar Interes
                            if (flags[0])
                            {
                                //MessageBox.Show(e.Result);

                                flags[0] = false;
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getIntereses, "");
                            }
                            break;

                        case "p24": //Modificar Ocupacion
                            if (flags[0])
                            {
                                flags[0] = false;
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getOcupacion, "");
                            }
                            break;

                        case "p25": //Modificar Parentesco
                            if (flags[0])
                            {
                                flags[0] = false;
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getParentesco, "");
                            }
                            break;

                        case "p26": //Modificar Motivo
                            if (flags[0])
                            {
                                flags[0] = false;
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getMotivos, "");
                            }
                            break;

                        case "p27": //Modificar Interes
                            if (flags[0])
                            {
                                flags[0] = false;
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getIntereses, "");
                            }
                            break;

                        case "p36"://Insertar Aportacion OE
                            if (flags[0])
                            {
                                flags[0] = false;
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getAportacion_OE, "");
                            }
                            break;

                        case "p37"://Get Aportaciones OE
                            if (flags[3])
                            {
                                flags[3] = false;
                                habilitarCampos(true, "aportacion");

                                aportacionMonto_txt.Text = "";
                                aportacionMotivo_txt.SelectedIndex = -1;
                                aportacionDescripcion_txt.Text = "";

                                aportacionObligatoria_grid.ItemsSource = MainPage.tc.getAportacionObligatoriaEspecial(recievedResponce);

                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getMotivos, "");
                            }
                            break;
                        default:

                            //MessageBox.Show(e.Result.ToString());
                            habilitarTodaPantalla();
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
                habilitarTodaPantalla();
            }
        }



        /*********************************************************************************************************************************
         ********************************     LLamados a Getters de parametrizables desde los TABS    ************************************
         *********************************************************************************************************************************/

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (tabsParametrizable.SelectedIndex)
                {
                    case 0:
                        //Profesion
                        profesion_grid.ItemsSource = null;
                        habilitarCampos(false, "profesion");
                        flags[1] = true;
                        flags[2] = true;
                        Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                        Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getOcupacion, "");
                        break;
                    case 1:
                        //Aportacion Obligatoria Especial 
                        tabAportacion.SelectedIndex = 0;
                        aportacionObligatoria_grid.ItemsSource = null;
                        habilitarCampos(false, "aportacion");
                        flags[1] = true;
                        flags[2] = true;
                        flags[3] = true;
                        Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                        Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getAportacion_OE, "");
                        break;
                    case 2:
                        //Motivos de Aportacion
                        motivo_grid.ItemsSource = null;
                        habilitarCampos(false, "motivo");
                        flags[1] = true;
                        flags[2] = true;
                        Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                        Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getMotivos, "");
                        break;
                    case 3:
                        //Parentesco
                        parentesco_grid.ItemsSource = null;
                        habilitarCampos(false, "parentesco");
                        flags[1] = true;
                        flags[2] = true;
                        Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                        Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getParentesco, "");
                        break;
                    case 4:
                        //Interes
                        interes_grid.ItemsSource = null;
                        habilitarCampos(false, "interes");
                        flags[1] = true;
                        flags[2] = true;
                        Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                        Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getIntereses, "");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                /*
                 * 
                 * 
                 //Navega a la pagina de error (Ya lo hace)
                 *
                 * 
                 * 
                 */
            }
        }

        private void tabAportacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (tabAportacion.SelectedIndex)
                {
                    case 1:
                        //Monto de Aportacion Obligatoria
                        habilitarCampos(false, "monto");
                        flags[1] = true;
                        flags[2] = true;
                        Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                        Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getMontoActual, "");
                        break;
                    case 2:
                        //Limite Aportacion Voluntaria
                        habilitarCampos(false, "limite");
                        flags[1] = true;
                        flags[2] = true;
                        Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                        Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getLimiteActual, "");
                        break;
                }
            }
            catch (Exception)
            {

            }
        }



        /********************************************************************************************************************************
        **************************************************    Metodos de Botones    *****************************************************
        *********************************************************************************************************************************/

        //Ingresar ocupacion
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (profesion_txt.Text == "")
            {
                MessageBox.Show("Ingrese una profesion");
                return;
            }
            string profesion = profesion_txt.Text;
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            habilitarCampos(false, "profesion");
            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.ingresar_ocupacion, MainPage.tc.InsertarOcupacion(profesion, usuario));
        }

        //Eliminar Profesion
        private void profesionEliminar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (profesion_txt.Text == "")
            {
                MessageBox.Show("Elija una profesion");
                return;
            }
            string profesion = profesion_txt.Text;
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            habilitarCampos(false, "profesion");
            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.eliminarOcupacion, MainPage.tc.DeshabilitarOcupacion(profesion, usuario));
        }

        //Modificar Profesion
        private void profesionModificar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (profesionModificar_txt.Text == "")
            {
                MessageBox.Show("Elija una profesion");
                return;
            }
            if (profesionNuevo_txt.Text == "")
            {
                MessageBox.Show("Ingrese el nombre nuevo para la profesion");
                return;
            }
            string profesionV = profesionModificar_txt.Text;
            string profesionNuevo = profesionNuevo_txt.Text;
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            habilitarCampos(false, "profesion");
            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.modificarOcupacion, MainPage.tc.ModificarOcupacion(profesionNuevo, profesionV, usuario));
        }

        //Ajustar/Ingresar Monto
        private void ajustarMonto_btn_Click(object sender, RoutedEventArgs e)
        {
            if (nuevoMonto_txt.Text == "")
            {
                MessageBox.Show("Ingrese un numero");
                return;
            }
            string nuevoMonto = nuevoMonto_txt.Text;
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            habilitarCampos(false, "monto");
            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.ingresar_monto_AO, MainPage.tc.InsertarMonto(nuevoMonto, usuario));
        }

        //Ajustar/Ingresar Limite de Aportacion Voluntaria
        private void ajustarLimite_btn_Click(object sender, RoutedEventArgs e)
        {
            if (nuevoLimite_txt.Text == "")
            {
                MessageBox.Show("Ingrese un numero");
                return;
            }
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;

            string nuevoLimite = nuevoLimite_txt.Text;
            habilitarCampos(false, "limite");

            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.limite_AV, MainPage.tc.InsertarLimite(nuevoLimite, usuario));
        }

        //Ingresar Motivo
        private void motivoAgregar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (motivo_txt.Text == "")
            {
                MessageBox.Show("Ingrese un Motivo");
                return;
            }
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;

            string motivo = motivo_txt.Text;
            habilitarCampos(false, "motivo");

            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.ingresar_motivo, MainPage.tc.InsertarMotivo(motivo, usuario));
        }

        //Eliminar Motivo
        private void motivoEliminar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (motivo_txt.Text == "")
            {
                MessageBox.Show("Elija un Motivo");
                return;
            }
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;

            string motivo = motivo_txt.Text;
            habilitarCampos(false, "motivo");

            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.eliminarMotivo, MainPage.tc.DeshabilitarMotivo(motivo, usuario));
        }

        //Modificar Motivo
        private void modificarMotivo_btn_Click(object sender, RoutedEventArgs e)
        {
            if (motivoViejo_txt.Text == "")
            {
                MessageBox.Show("Elija un Motivo");
                return;
            }
            if (motivoNuevo_txt.Text == "")
            {
                MessageBox.Show("Ingrese el nombre nuevo para el Motivo");
                return;
            }
            string viejo = motivoViejo_txt.Text;
            string nuevo = motivoNuevo_txt.Text;
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            habilitarCampos(false, "motivo");
            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.modificarMotivo, MainPage.tc.ModificarMotivo(nuevo, viejo, usuario));
        }

        //Ingresar Parentesco
        private void parentescoAgregar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (parentesco_txt.Text == "")
            {
                MessageBox.Show("Ingrese un Parentesco");
                return;
            }
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;

            string parentesco = parentesco_txt.Text;
            habilitarCampos(false, "parentesco");

            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.ingresar_parentesco, MainPage.tc.InsertarParentesco(parentesco, usuario));
        }

        //Eliminar Parentesco
        private void parentescoEliminar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (parentesco_txt.Text == "")
            {
                MessageBox.Show("Elija un Parentesco");
                return;
            }
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;

            string parentesco = parentesco_txt.Text;
            habilitarCampos(false, "parentesco");

            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.eliminarParentesco, MainPage.tc.DeshabilitarParentesco(parentesco, usuario));
        }

        //Modificar Parentesco
        private void parentescoModificar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (parentescoViejo_txt.Text == "")
            {
                MessageBox.Show("Elija un Parentesco");
                return;
            }
            if (parentescoNuevo_txt.Text == "")
            {
                MessageBox.Show("Ingrese el nombre nuevo para el Parentesco");
                return;
            }
            string viejo = parentescoViejo_txt.Text;
            string nuevo = parentescoNuevo_txt.Text;
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            habilitarCampos(false, "motivo");
            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.modificarParentesco, MainPage.tc.ModificarParentesco(nuevo, viejo, usuario));
        }

        //Ingresar Interes
        private void interesAgregar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (interesNombre_txt.Text == "" || interesPorcentaje_txt.Text == "")
            {
                MessageBox.Show("Ingrese un nombre y porcentaje para el interes");
                return;
            }
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;

            bool? aplicaObligatoria = null;
            bool? aplicaObligatoriaE = null;
            bool? aplicaVoluntaria = null;
            bool? aplicaVoluntariaE = null;

            Interes interes = new Interes();
            interes.Nombre = interesNombre_txt.Text;
            interes.Tasa = Convert.ToInt64(interesPorcentaje_txt.Text);
            if (!aplicaObligatoria.HasValue)
            {
                aplicaObligatoria = interesAplica2_chk.IsChecked;
            }
            interes.Aplica_obligatoria = (bool)aplicaObligatoria;

            if (!aplicaObligatoriaE.HasValue)
            {
                aplicaObligatoriaE = interesAplica3_chk.IsChecked;
            }
            interes.Aplica_obligatoria_especial = (bool)aplicaObligatoriaE;

            if (!aplicaVoluntaria.HasValue)
            {
                aplicaVoluntaria = interesAplica1_chk.IsChecked;
            }
            interes.Aplica_voluntaria = (bool)aplicaVoluntaria;

            if (!aplicaVoluntariaE.HasValue)
            {
                aplicaVoluntariaE = interesAplica4_chk.IsChecked;
            }
            interes.Aplica_voluntaria_arriba = (bool)aplicaVoluntariaE;
            if (interes.Aplica_voluntaria_arriba && interesAportacionAplica_txt.Text == "")
            {
                /*
                 * 
                 * 
                 * Mensaje de Error (Validacion que ponga el numero que aplica)
                 * 
                 * 
                 */
            }
            else
            {
                if (interesAportacionAplica_txt.Text == "")
                {
                    interes.Monto_voluntaria = 0;
                }
                else
                {
                    interes.Monto_voluntaria = Convert.ToInt64(interesAportacionAplica_txt.Text);
                }
            }

            habilitarCampos(false, "interes");
            interesAportacionAplica_txt.IsEnabled = false;

            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.ingresar_interes, MainPage.tc.InsertarInteres(interes, usuario));
        }

        //Eliminar Interes
        private void interesEliminar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (interesNombre_txt.Text == "")
            {
                MessageBox.Show("Elija un nombre de interes");
                return;
            }
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;

            string interes = interesNombre_txt.Text;
            habilitarCampos(false, "interes");

            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.eliminarInteres, MainPage.tc.DeshabilitarInteres(interes, usuario));
        }

        //Modificar Interes
        private void interesModificar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (interesViejo_txt.Text == "")
            {
                MessageBox.Show("Elija un Interes");
                return;
            }
            if (interesModificarNombre_txt.Text == "")
            {
                MessageBox.Show("Ingrese el nombre nuevo para el Interes");
                return;
            }
            if (interesModificarPorc_txt.Text == "")
            {
                MessageBox.Show("Ingrese un Porcentaje de Interes");
                return;
            }

            string viejo = interesViejo_txt.Text;
            bool? aplicaObligatoria = null;
            bool? aplicaObligatoriaE = null;
            bool? aplicaVoluntaria = null;
            bool? aplicaVoluntariaE = null;

            if (!aplicaObligatoria.HasValue)
            {
                aplicaObligatoria = modificarInteresAplica2_chk.IsChecked;
            }

            if (!aplicaObligatoriaE.HasValue)
            {
                aplicaObligatoriaE = modificarInteresAplica3_chk.IsChecked;
            }

            if (!aplicaVoluntaria.HasValue)
            {
                aplicaVoluntaria = modificarInteresAplica1_chk.IsChecked;
            }

            if (!aplicaVoluntariaE.HasValue)
            {
                aplicaVoluntariaE = modificarInteresAplica4_chk.IsChecked;
            }

            Interes interesNuevo = new Interes()
            {
                Nombre = interesModificarNombre_txt.Text,
                Tasa = Convert.ToInt64(interesModificarPorc_txt.Text),
                Aplica_obligatoria = (bool)aplicaObligatoria,
                Aplica_obligatoria_especial = (bool)aplicaObligatoriaE,
                Aplica_voluntaria = (bool)aplicaVoluntaria,
                Aplica_voluntaria_arriba = (bool)aplicaVoluntariaE
            };

            if (interesNuevo.Aplica_voluntaria_arriba && interesAportacionAplica_txt.Text == "")
            {
                interesAportacionAplica_txt.Text = "0";
                interesNuevo.Monto_voluntaria = Convert.ToInt64(interesAportacionAplica_txt.Text);
            }
            else
            {
                interesNuevo.Monto_voluntaria = 0;
            }

            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            habilitarCampos(false, "interes");
            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.modificarInteres, MainPage.tc.ModificarInteres(interesNuevo, viejo, usuario));
        }

        //Ingresar Aportacion OE
        private void aportacionAgregar_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dlgResult = MessageBox.Show("Una vez ingresada, esta aportacion no se puede deshacer.\nEsta seguro que los campos estan correctos y desea continuar?", "Continue?", MessageBoxButton.OKCancel);
            if (dlgResult == MessageBoxResult.OK)
            {
                if (aportacionAno_txt.SelectedIndex == -1 || aportacionMes_txt.SelectedIndex == -1)
                {
                    MessageBox.Show("Debe especificar un año y mes");
                    return;
                }
                if (aportacionMotivo_txt.SelectedIndex == -1)
                {
                    MessageBox.Show("Ingrese un motivo");
                    return;
                }
                if (aportacionMonto_txt.Text == "")
                {
                    MessageBox.Show("Ingrese un monto");
                    return;
                }
                DateTime fecha = DateTime.Now;
                if (mesANumero(aportacionMes_txt.SelectedItem.ToString()) <= fecha.Month && Convert.ToInt32(aportacionAno_txt.SelectedItem.ToString()) == fecha.Year)
                {
                    MessageBox.Show("No puede ingresar una fecha en el pasado o del mes actual!");
                    return;
                }

                aportacionObligatoriaEspecial aportacionOE = new aportacionObligatoriaEspecial();
                aportacionOE.FechaRealizacion = mesANumero(aportacionMes_txt.SelectedItem.ToString()) + "/1/" + aportacionAno_txt.SelectedItem.ToString();
                aportacionOE.Monto = Convert.ToInt64(aportacionMonto_txt.Text);
                aportacionOE.Motivo = aportacionMotivo_txt.SelectedItem.ToString();
                aportacionOE.Descripcion = aportacionDescripcion_txt.Text;

                flags[0] = true;
                flags[1] = true;
                flags[2] = true;
                flags[3] = true;
                habilitarCampos(false, "aportacion");
                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.insertarAportacionOE, MainPage.tc.InsertarAportacionObligatoriaEspecial(aportacionOE, usuario));
            }
            else if (dlgResult == MessageBoxResult.Cancel)
            {
                // No, stop
            }
        }

        /********************************************************************************************************************************
        **************************************************    Validaciones!!   **********************************************************
        *********************************************************************************************************************************/
        private void profesion_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            profesion_txt.Text.Trim();
            if (nombres.IsMatch(profesion_txt.Text))
            {
                profesionAgregar_btn.IsEnabled = true;
                profesionEliminar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                profesion_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                profesionAgregar_btn.IsEnabled = false;
                profesionEliminar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                profesion_valid.Source = new BitmapImage(imgURI);
            }
            if (profesion_txt.Text == "")
            {
                profesion_valid.Source = MainPage.bmpClear;
            }
        }

        private void profesionNuevo_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            profesionNuevo_txt.Text.Trim();
            if (nombres.IsMatch(profesionNuevo_txt.Text))
            {
                profesionModificar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                profesionNuevo_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                profesionModificar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                profesionNuevo_valid.Source = new BitmapImage(imgURI);
            }
            if (profesionNuevo_txt.Text == "")
            {
                profesion_valid.Source = MainPage.bmpClear;
            }
        }


        private void nuevoLimite_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            nuevoLimite_txt.Text.Trim();
            if (numeros.IsMatch(nuevoLimite_txt.Text))
            {
                ajustarLimite_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                limite_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                ajustarLimite_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                limite_valid.Source = new BitmapImage(imgURI);
            }
            if (nuevoLimite_txt.Text == "")
            {
                limite_valid.Source = MainPage.bmpClear;
            }
        }

        private void aportacionMonto_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            aportacionMonto_txt.Text.Trim();
            if (numeros.IsMatch(aportacionMonto_txt.Text))
            {
                aportacionAgregar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                aportacionMonto_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                aportacionAgregar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                aportacionMonto_valid.Source = new BitmapImage(imgURI);
            }
            if (aportacionMonto_txt.Text == "")
            {
                aportacionMonto_valid.Source = MainPage.bmpClear;
            }
        }

        private void nuevoMonto_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            nuevoMonto_txt.Text.Trim();
            if (numeros.IsMatch(nuevoMonto_txt.Text))
            {
                ajustarMonto_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                monto_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                ajustarMonto_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                monto_valid.Source = new BitmapImage(imgURI);
            }
            if (nuevoMonto_txt.Text == "")
            {
                monto_valid.Source = MainPage.bmpClear;
            }
        }

        private void motivo_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            motivo_txt.Text.Trim();
            if (nombres.IsMatch(motivo_txt.Text))
            {
                motivoAgregar_btn.IsEnabled = true;
                motivoEliminar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                motivo_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                motivoAgregar_btn.IsEnabled = false;
                motivoEliminar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                motivo_valid.Source = new BitmapImage(imgURI);
            }
            if (motivo_txt.Text == "")
            {
                motivo_valid.Source = MainPage.bmpClear;
            }
        }

        private void motivoNuevo_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            motivoNuevo_txt.Text.Trim();
            if (nombres.IsMatch(motivoNuevo_txt.Text))
            {
                modificarMotivo_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                motivoNuevo_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                modificarMotivo_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                motivoNuevo_valid.Source = new BitmapImage(imgURI);
            }
            if (motivoNuevo_txt.Text == "")
            {
                motivoNuevo_valid.Source = MainPage.bmpClear;
            }
        }

        private void parentesco_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            parentesco_txt.Text.Trim();
            if (nombres.IsMatch(parentesco_txt.Text))
            {
                parentescoAgregar_btn.IsEnabled = true;
                parentescoEliminar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                parentesco_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                parentescoAgregar_btn.IsEnabled = false;
                parentescoEliminar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                parentesco_valid.Source = new BitmapImage(imgURI);
            }
            if (parentesco_txt.Text == "")
            {
                parentesco_valid.Source = MainPage.bmpClear;
            }
        }

        private void parentescoNuevo_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            parentescoNuevo_txt.Text.Trim();
            if (nombres.IsMatch(parentescoNuevo_txt.Text))
            {
                parentescoModificar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                parentescoNuevo_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                parentescoModificar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                parentescoNuevo_valid.Source = new BitmapImage(imgURI);
            }
            if (parentescoNuevo_txt.Text == "")
            {
                parentescoNuevo_valid.Source = MainPage.bmpClear;
            }
        }

        private void interesNombre_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            interesNombre_txt.Text.Trim();
            if (alphanumerico.IsMatch(interesNombre_txt.Text))
            {
                interesAgregar_btn.IsEnabled = true;
                interesEliminar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                interesNombre_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                interesAgregar_btn.IsEnabled = false;
                interesEliminar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                interesNombre_valid.Source = new BitmapImage(imgURI);
            }
            if (interesNombre_txt.Text == "")
            {
                interesNombre_valid.Source = MainPage.bmpClear;
            }
        }

        private void interesPorcentaje_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            interesPorcentaje_txt.Text.Trim();
            if (porcentaje.IsMatch(interesPorcentaje_txt.Text))
            {
                interesAgregar_btn.IsEnabled = true;
                interesEliminar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                interesPorcentaje_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                interesAgregar_btn.IsEnabled = false;
                interesEliminar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                interesPorcentaje_valid.Source = new BitmapImage(imgURI);
            }
            if (interesPorcentaje_txt.Text == "")
            {
                interesPorcentaje_valid.Source = MainPage.bmpClear;
            }
        }

        private void interesAportacionAplica_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (interesAportacionVoluntaria)
            {
                interesAportacionAplica_txt.Text.Trim();
                if (numeros.IsMatch(interesAportacionAplica_txt.Text))
                {
                    interesAgregar_btn.IsEnabled = true;
                    interesEliminar_btn.IsEnabled = true;
                    string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                    Uri imgURI = new Uri(sURL, UriKind.Relative);
                    aportacionVoluntaria_valid.Source = new BitmapImage(imgURI);
                }
                else
                {
                    interesAgregar_btn.IsEnabled = false;
                    interesEliminar_btn.IsEnabled = false;
                    string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                    Uri imgURI = new Uri(sURL, UriKind.Relative);
                    aportacionVoluntaria_valid.Source = new BitmapImage(imgURI);
                }
            }
            if (interesAportacionAplica_txt.Text == "")
            {
                aportacionVoluntaria_valid.Source = MainPage.bmpClear;
            }
        }

        private void interesModificarNombre_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            interesModificarNombre_txt.Text.Trim();
            if (alphanumerico.IsMatch(interesModificarNombre_txt.Text))
            {
                interesModificar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                interesModificarNombre_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                interesModificar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                interesModificarNombre_valid.Source = new BitmapImage(imgURI);
            }
            if (interesModificarNombre_txt.Text == "")
            {
                interesModificarNombre_valid.Source = MainPage.bmpClear;
            }
        }

        private void interesModificarPorc_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            interesModificarPorc_txt.Text.Trim();
            if (porcentaje.IsMatch(interesModificarPorc_txt.Text))
            {
                interesModificar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                interesModificarPorcentaje_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                interesModificar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                interesModificarPorcentaje_valid.Source = new BitmapImage(imgURI);
            }
            if (interesModificarPorc_txt.Text == "")
            {
                interesModificarPorcentaje_valid.Source = MainPage.bmpClear;
            }
        }

        private void modificarAplicaAV_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            modificarAplicaAV_txt.Text.Trim();
            if (numeros.IsMatch(modificarAplicaAV_txt.Text))
                {
                    interesModificar_btn.IsEnabled = true;
                    string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                    Uri imgURI = new Uri(sURL, UriKind.Relative);
                    interesModificarAplicaAV_valid.Source = new BitmapImage(imgURI);
                }
                else
                {
                    interesModificar_btn.IsEnabled = false;
                    string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                    Uri imgURI = new Uri(sURL, UriKind.Relative);
                    interesModificarAplicaAV_valid.Source = new BitmapImage(imgURI);
                }
            if (modificarAplicaAV_txt.Text == "")
            {
                interesModificarAplicaAV_valid.Source = MainPage.bmpClear;
                interesModificar_btn.IsEnabled = true;
            }
        }


        //Estos 4 son las checkboxes en la ultima tab, de interes
        private void checkBox4_Checked(object sender, RoutedEventArgs e)
        {
            interesAportacionAplica_txt.IsEnabled = true;
            interesAportacionVoluntaria = true;
        }

        private void checkBox4_Unchecked(object sender, RoutedEventArgs e)
        {
            interesAportacionAplica_txt.Text = "";
            interesAportacionAplica_txt.IsEnabled = false;
            interesAportacionVoluntaria = false;
            interesAgregar_btn.IsEnabled = true;
            interesEliminar_btn.IsEnabled = true;
        }

        private void modificarInteresAplica4_chk_Checked(object sender, RoutedEventArgs e)
        {
            modificarAplicaAV_txt.IsEnabled = true;
        }

        private void modificarInteresAplica4_chk_Unchecked(object sender, RoutedEventArgs e)
        {
            modificarAplicaAV_txt.IsEnabled = false;
            modificarAplicaAV_txt.Text = "";
            interesModificar_btn.IsEnabled = true;
        }
        

        /********************************************************************************************************************************
        **************************************************    Metodos Auxiliares    *****************************************************
        *********************************************************************************************************************************/

        private void habilitarCampos(bool valor, string tab)
        {
            switch (tab)
            {
                case "profesion":
                    profesionAgregar_btn.IsEnabled = valor;
                    profesionEliminar_btn.IsEnabled = valor;
                    profesion_txt.IsEnabled = valor;
                    tabProfesion.IsEnabled = valor;
                    //Tabs
                    aportacion_tab.IsEnabled = valor;
                    motivo_tab.IsEnabled = valor;
                    parentesco_tab.IsEnabled = valor;
                    interes_tab.IsEnabled = valor;
                    break;

                case "monto":
                    ajustarMonto_btn.IsEnabled = valor;
                    nuevoMonto_txt.IsEnabled = valor;
                    //Tabs
                    profesion_tab.IsEnabled = valor;
                    motivo_tab.IsEnabled = valor;
                    parentesco_tab.IsEnabled = valor;
                    interes_tab.IsEnabled = valor;
                    aportacionObligatoria_tab.IsEnabled = valor;
                    limite_tab.IsEnabled = valor;
                    break;

                case "limite":
                    nuevoLimite_txt.IsEnabled = valor;
                    ajustarLimite_btn.IsEnabled = valor;
                    //Tabs
                    profesion_tab.IsEnabled = valor;
                    motivo_tab.IsEnabled = valor;
                    parentesco_tab.IsEnabled = valor;
                    interes_tab.IsEnabled = valor;
                    aportacionObligatoria_tab.IsEnabled = valor;
                    monto_tab.IsEnabled = valor;
                    break;

                case "motivo":
                    motivo_txt.IsEnabled = valor;
                    motivoAgregar_btn.IsEnabled = valor;
                    motivoEliminar_btn.IsEnabled = valor;
                    tabMotivo.IsEnabled = valor;
                    //Tabs
                    profesion_tab.IsEnabled = valor;
                    aportacion_tab.IsEnabled = valor;
                    parentesco_tab.IsEnabled = valor;
                    interes_tab.IsEnabled = valor;
                    break;

                case "parentesco":
                    parentesco_txt.IsEnabled = valor;
                    parentescoAgregar_btn.IsEnabled = valor;
                    parentescoEliminar_btn.IsEnabled = valor;
                    tabParentesco.IsEnabled = valor;
                    //Tabs
                    profesion_tab.IsEnabled = valor;
                    aportacion_tab.IsEnabled = valor;
                    motivo_tab.IsEnabled = valor;
                    interes_tab.IsEnabled = valor;
                    break;

                case "interes":
                    interesNombre_txt.IsEnabled = valor;
                    interesPorcentaje_txt.IsEnabled = valor;
                    interesAplica1_chk.IsEnabled = valor;
                    interesAplica2_chk.IsEnabled = valor;
                    interesAplica3_chk.IsEnabled = valor;
                    interesAplica4_chk.IsEnabled = valor;
                    interesAgregar_btn.IsEnabled = valor;
                    interesEliminar_btn.IsEnabled = valor;
                    tabInteres.IsEnabled = valor;
                    //Tabs
                    profesion_tab.IsEnabled = valor;
                    aportacion_tab.IsEnabled = valor;
                    motivo_tab.IsEnabled = valor;
                    parentesco_tab.IsEnabled = valor;
                    break;

                case "aportacion":
                    aportacionAno_txt.IsEnabled = valor;
                    aportacionMes_txt.IsEnabled = valor;
                    aportacionMonto_txt.IsEnabled = valor;
                    aportacionMotivo_txt.IsEnabled = valor;
                    aportacionDescripcion_txt.IsEnabled = valor;
                    aportacionAgregar_btn.IsEnabled = valor;
                    //Tabs
                    monto_tab.IsEnabled = valor;
                    limite_tab.IsEnabled = valor;
                    profesion_tab.IsEnabled = valor;
                    motivo_tab.IsEnabled = valor;
                    parentesco_tab.IsEnabled = valor;
                    interes_tab.IsEnabled = valor;
                    break;
            }

        }

        private void habilitarTodaPantalla()
        {
            habilitarCampos(true, "motivo");
            habilitarCampos(true, "parentesco");
            habilitarCampos(true, "limite");
            habilitarCampos(true, "monto");
            habilitarCampos(true, "profesion");
            habilitarCampos(true, "interes");
            habilitarCampos(true, "aportacion");
        }

        private void profesion_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                profesionGrid selection = (profesionGrid)profesion_grid.SelectedItem;
                profesion_txt.Text = selection.Nombre;
                profesionModificar_txt.Text = selection.Nombre;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void motivo_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                profesionGrid selection = (profesionGrid)motivo_grid.SelectedItem;
                motivo_txt.Text = selection.Nombre;
                motivoViejo_txt.Text = selection.Nombre;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void parentesco_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                profesionGrid selection = (profesionGrid)parentesco_grid.SelectedItem;
                parentesco_txt.Text = selection.Nombre;
                parentescoViejo_txt.Text = selection.Nombre;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void interes_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Interes selection = (Interes)interes_grid.SelectedItem;
                interesNombre_txt.Text = selection.Nombre;
                interesPorcentaje_txt.Text = selection.Tasa.ToString();
                interesAplica1_chk.IsChecked = selection.Aplica_voluntaria;
                interesAplica2_chk.IsChecked = selection.Aplica_obligatoria;
                interesAplica3_chk.IsChecked = selection.Aplica_obligatoria_especial;
                interesAplica4_chk.IsChecked = selection.Aplica_voluntaria_arriba;
                interesAportacionAplica_txt.Text = selection.Monto_voluntaria.ToString();
                interesViejo_txt.Text = selection.Nombre;
                interesViejo = selection.Nombre;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void habilitarFlags()
        {
            for (int i = 0; i < flags.Length; i++)
            {
                flags[i] = true;
            }
        }

        private int mesANumero(string mes)
        {
            switch (mes)
            {
                case "Enero":
                    return 1;
                case "Febrero":
                    return 2;
                    
                case "Marzo":
                    return 3;
                    
                case "Abril":
                    return 4;
                    
                case "Mayo":
                    return 5;
                    
                case "Junio":
                    return 6;
                    
                case "Julio":
                    return 7;
                    
                case "Agosto":
                    return 8;
                    
                case "Septiembre":
                    return 9;
                    
                case "Octubre":
                    return 10;
                    
                case "Noviembre":
                    return 11;
                    
                case "Diciembre":
                    return 11;
                    
                default:
                    return 0;
                    
            }
        }

    }
}
