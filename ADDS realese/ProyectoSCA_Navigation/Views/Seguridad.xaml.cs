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

namespace ProyectoSCA_Navigation.Views
{
    public partial class Seguridad : Page
    {
        string usuario = App.Correo;

        //Flags, true, puede disparar el evento, false no
        bool[] flags = new bool[10];
        /***    0 - Abrir Inserts
         ***    1 - Abrir Gets, luego Llenar campos de datos
         ***    2 - Error Windows
         ***/
        List<Boolean> permisosAfiliado = new List<Boolean>();
        List<Boolean> permisosEmpleado = new List<Boolean>();
        List<Boolean> permisosAdministrador = new List<Boolean>();

        /********************************************************************************************************************************
        **************************************************    Inicializadores   *********************************************************
        *********************************************************************************************************************************/

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private ProyectoSCA_Navigation.ServiceReference.WSCosecolSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:1809/WSCosecol.asmx";


        public Seguridad()
        {
            InitializeComponent();
            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReference.WSCosecolSoapClient(bind, endpoint);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mensaje_txt.Text = "*Si su tipo de usuario esta siendo modificado, debe\nreiniciar su sesion para ver los cambios";
            rol_txt.Items.Add("Afiliado");
            rol_txt.Items.Add("Empleado");
            rol_txt.Items.Add("Administrador");
            for (int i = 0; i < flags.Length; i++)
            {
                flags[i] = true;
            }
            habilitarCampos(false);
            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getPermisosSeguridad, "");
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
                        habilitarCampos(true);
                    }
                }
                else
                {
                    switch (temp)
                    {
                        case "p34":
                            if (flags[1])
                            {
                                flags[1] = false;
                                habilitarCampos(true);
                                List<List<Boolean>> listaDeLista = new List<List<Boolean>>();

                                listaDeLista = MainPage.tc.getTodosLosPermisos(recievedResponce);
                                permisosAdministrador = listaDeLista[0];
                                permisosEmpleado = listaDeLista[1];
                                permisosAfiliado = listaDeLista[2];
                            }
                            break;

                        case "p35":
                            if (flags[0])
                            {
                                flags[0] = false;
                                habilitarCampos(true);
                                MessageBox.Show("Permisos modificados exitosamente!");
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getPermisosSeguridad, "");
                            }
                            break;

                        default:
                            if (flags[2])
                            {
                                flags[2] = false;
                                MessageBox.Show("Ha ocurrido un error.  Intente su consulta de nuevo.\nSi el problema persiste, refresque la pagina e intente de nuevo.");
                                habilitarCampos(true);
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
                    habilitarCampos(true);
                }
            }
        }


        /********************************************************************************************************************************
        **************************************************    Metodos de Botones    *****************************************************
        *********************************************************************************************************************************/
        private void guardar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (rol_txt.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un Rol!");
                return;
            }

            MessageBoxResult dlgResult = MessageBox.Show("Esta seguro que desea modificar los permisos para " + rol_txt.SelectedItem.ToString() + "?", "Seguro?", MessageBoxButton.OKCancel);
            if (dlgResult == MessageBoxResult.OK)
            {
                bool? registrarPersona = null;
                bool? controlDePago = null;
                bool? capitalizacion = null;
                bool? consultas = null;
                bool? parametrizacion = null;
                bool? seguridad = null;

                if (!registrarPersona.HasValue)
                    registrarPersona = registrarPersona_chk.IsChecked;

                if (!controlDePago.HasValue)
                    controlDePago = controlDePago_chk.IsChecked;

                if (!capitalizacion.HasValue)
                    capitalizacion = capitalizacion_chk.IsChecked;

                if (!consultas.HasValue)
                    consultas = consultas_chk.IsChecked;

                if (!parametrizacion.HasValue)
                    parametrizacion = parametrizacion_chk.IsChecked;

                if (!seguridad.HasValue)
                    seguridad = seguridad_chk.IsChecked;

                switch (rol_txt.SelectedItem.ToString())
                {
                    case "Administrador":
                        permisosAdministrador[0] = (bool)registrarPersona;
                        permisosAdministrador[1] = (bool)controlDePago;
                        permisosAdministrador[2] = (bool)capitalizacion;
                        permisosAdministrador[3] = (bool)consultas;
                        permisosAdministrador[4] = (bool)parametrizacion;
                        permisosAdministrador[5] = (bool)seguridad;
                        break;

                    case "Empleado":
                        permisosEmpleado[0] = (bool)registrarPersona;
                        permisosEmpleado[1] = (bool)controlDePago;
                        permisosEmpleado[2] = (bool)capitalizacion;
                        permisosEmpleado[3] = (bool)consultas;
                        permisosEmpleado[4] = (bool)parametrizacion;
                        permisosEmpleado[5] = (bool)seguridad;
                        break;

                    case "Afiliado":
                        permisosAfiliado[0] = (bool)registrarPersona;
                        permisosAfiliado[1] = (bool)controlDePago;
                        permisosAfiliado[2] = (bool)capitalizacion;
                        permisosAfiliado[3] = (bool)consultas;
                        permisosAfiliado[4] = (bool)parametrizacion;
                        permisosAfiliado[5] = (bool)seguridad;
                        break;
                }

                habilitarCampos(false);
                flags[0] = true;
                flags[1] = true;
                flags[2] = true;
                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.modificarPermisosSeguridad, MainPage.tc.ModificarPermisos(permisosAdministrador, permisosEmpleado, permisosAfiliado, usuario));
            }
            else if (dlgResult == MessageBoxResult.Cancel)
            {
                return;
            }
        }

        /********************************************************************************************************************************
        **************************************************    Validaciones!!   **********************************************************
        *********************************************************************************************************************************/

        /********************************************************************************************************************************
        **************************************************    Metodos Auxiliares    *****************************************************
        *********************************************************************************************************************************/

        private void rol_txt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            switch (rol_txt.SelectedItem.ToString())
            {
                case "Administrador":
                    registrarPersona_chk.IsChecked = permisosAdministrador[0];
                    controlDePago_chk.IsChecked = permisosAdministrador[1];
                    capitalizacion_chk.IsChecked = permisosAdministrador[2];
                    consultas_chk.IsChecked = permisosAdministrador[3];
                    parametrizacion_chk.IsChecked = permisosAdministrador[4];
                    seguridad_chk.IsChecked = permisosAdministrador[5];
                    seguridad_chk.IsEnabled = false;
                    break;

                case "Empleado":
                    registrarPersona_chk.IsChecked = permisosEmpleado[0];
                    controlDePago_chk.IsChecked = permisosEmpleado[1];
                    capitalizacion_chk.IsChecked = permisosEmpleado[2];
                    consultas_chk.IsChecked = permisosEmpleado[3];
                    parametrizacion_chk.IsChecked = permisosEmpleado[4];
                    seguridad_chk.IsChecked = permisosEmpleado[5];
                    seguridad_chk.IsEnabled = true;
                    break;

                case "Afiliado":
                    registrarPersona_chk.IsChecked = permisosAfiliado[0];
                    controlDePago_chk.IsChecked = permisosAfiliado[1];
                    capitalizacion_chk.IsChecked = permisosAfiliado[2];
                    consultas_chk.IsChecked = permisosAfiliado[3];
                    parametrizacion_chk.IsChecked = permisosAfiliado[4];
                    seguridad_chk.IsChecked = permisosAfiliado[5];
                    seguridad_chk.IsEnabled = true;
                    break;
            }
        }

        private void habilitarCampos(bool valor)
        {
            rol_txt.IsEnabled = valor;
            registrarPersona_chk.IsEnabled = valor;
            controlDePago_chk.IsEnabled = valor;
            capitalizacion_chk.IsEnabled = valor;
            consultas_chk.IsEnabled = valor;
            parametrizacion_chk.IsEnabled = valor;
            seguridad_chk.IsEnabled = valor;
            guardar_btn.IsEnabled = valor;
        }

    }
}
