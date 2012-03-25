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

namespace ProyectoSCA_Navigation.Views
{
    public partial class ModificarPerfil : Page
    {
        string usuario = App.Correo;
        string correo = "";

        //Flags, true, puede disparar el evento, false no
        bool[] flags = new bool[10];
        /***    0 - Abrir Inserts
         ***    1 - Abrir Gets, luego Llenar campos de datos
         ***    2 - Error Windows
         ***/

        Afiliado afiliado = new Afiliado();
        Empleado empleado = new Empleado();
        List<String> correos = new List<String>();
        List<BeneficiarioGrid> listaBeneficiarios = new List<BeneficiarioGrid>();
        List<BeneficiarioNormal> beneficiarioNormal = new List<BeneficiarioNormal>();
        BeneficiarioContingencia beneficiarioContingencia = new BeneficiarioContingencia();
        DateTime fecha = DateTime.Now;

        /********************************************************************************************************************************
        **************************************************    Inicializadores   *********************************************************
        *********************************************************************************************************************************/

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private ProyectoSCA_Navigation.ServiceReference.WSCosecolSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:1809/WSCosecol.asmx";
        public ModificarPerfil()
        {
            InitializeComponent();
            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReference.WSCosecolSoapClient(bind, endpoint);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            rechazar_btn.IsEnabled = false;
            aceptarCambios_btn.IsEnabled = false;
            for (int i = 0; i < flags.Length; i++)
            {
                flags[i] = false;
            }

            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getPersonaTemporal, "");
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
                        case "p09"://Modificar Perfil
                            if (flags[0])
                            {
                                flags[0] = false;
                                MessageBox.Show("Se ha modificado exitosamente el perfil del afiliado: " + correo);
                                NavigationService.Refresh();
                            }
                            break;

                        case "p41"://Get Perfiles Temporales
                            if (flags[0])
                            {
                                flags[0] = false;
                                correos = MainPage.tc.getPersonaTemprales(recievedResponce);
                                afiliadosPendientes_txt.ItemsSource = correos;
                            }
                            break;

                        case "p44"://Get Perfil Nuevo
                            if (flags[0])
                            {
                                flags[0] = false;
                                string[] tipo = recievedResponce.Split('&');
                                if (tipo[0] == "Afiliado")
                                {
                                    datosLaborales_tab.IsEnabled = true;
                                    beneficiarios_tab.IsEnabled = true;
                                    getDatosAfiliado(tipo[1]);
                                }
                                else if (tipo[0] == "Empleado")
                                {
                                    datosLaborales_tab.IsEnabled = false;
                                    beneficiarios_tab.IsEnabled = false;
                                    getDatosEmpleado(tipo[1]);
                                }
                            }
                            break;

                        case "p45"://Rechazar Perfil
                            if (flags[0])
                            {
                                flags[0] = false;
                                MessageBox.Show("Se ha rechazado el perfil modificado de: " + correo);
                                NavigationService.Refresh();
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
        private void aceptarCambios_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dlgResult = MessageBox.Show("Esta accion modificara el perfil del usuario.  Desea continuar?", "Continue?", MessageBoxButton.OKCancel);
            if (dlgResult == MessageBoxResult.OK)
            {
                flags[0] = true;
                flags[1] = true;
                flags[2] = true;
                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.modificarPerfil, correo);
            }
            else if (dlgResult == MessageBoxResult.Cancel)
            {
                // No, stop
            }
        }

        private void rechazar_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dlgResult = MessageBox.Show("Esta accion rechazará el perfil del usuario.  Desea continuar?", "Continue?", MessageBoxButton.OKCancel);
            if (dlgResult == MessageBoxResult.OK)
            {
                flags[0] = true;
                flags[1] = true;
                flags[2] = true;
                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.rechazarPerfil, correo);
            }
            else if (dlgResult == MessageBoxResult.Cancel)
            {
                // No, stop
            }
        }

        private void buscar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (afiliadosPendientes_txt.SelectedIndex == -1)
            {
                MessageBox.Show("Escoga un correo de afiliado");
                return;
            }

            correo = afiliadosPendientes_txt.SelectedItem.ToString();
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            habilitarCampos(false);

            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getPerfilNuevo, correo);
        }

        /********************************************************************************************************************************
        **************************************************    Metodos Auxiliares    *****************************************************
        *********************************************************************************************************************************/
        public void habilitarCampos(bool valor)
        {
            afiliadosPendientes_txt.IsEnabled = valor;
            buscar_btn.IsEnabled = valor;
            tabControl1.IsEnabled = valor;
            aceptarCambios_btn.IsEnabled = valor;
            rechazar_btn.IsEnabled = valor;
        }

        public void getDatosAfiliado(string recievedResponce)
        {
            afiliado = MainPage.tc.getAfiliado(recievedResponce);

            pNombre_txt.Text = afiliado.primerNombre;
            sNombre_txt.Text = afiliado.segundoNombre;
            pApellido_txt.Text = afiliado.primerApellido;
            sApellido_txt.Text = afiliado.segundoApellido;
            direccion_txt.Text = afiliado.direccion;
            id_txt.Text = afiliado.identidad;
            genero_txt.Text = afiliado.genero;
            telefono_txt.Text = afiliado.telefonoPersonal[0];
            try
            {
                celular_txt.Text = afiliado.celular[0];
                telefono2_txt.Text = afiliado.telefonoPersonal[1];
                celular2_txt.Text = afiliado.celular[1];
            }
            catch { }
            profesion_txt.Text = afiliado.Ocupacion;
            email_txt.Text = afiliado.CorreoElectronico;
            lugarNacimiento_txt.Text = afiliado.lugarDeNacimiento;
            fechaNacimiento_txt.Text = afiliado.fechaNacimiento;
            estadoCivil_txt.Text = afiliado.estadoCivil;
            estadoAfiliado_txt.Text = afiliado.EstadoAfiliado;

            empresa_txt.Text = afiliado.NombreEmpresa;
            fechaIngresoEmpresa_txt.Text = afiliado.fechaIngresoCooperativa;
            telefonoEmpresa_txt.Text = afiliado.TelefonoEmpresa;
            departamentoEmpresa_txt.Text = afiliado.DepartamentoEmpresa;
            direccionEmpresa_txt.Text = afiliado.DireccionEmpresa;

            beneficiarioNormal = afiliado.bensNormales;
            beneficiarioContingencia = (BeneficiarioContingencia)afiliado.BeneficiarioCont;

            foreach (BeneficiarioNormal item in beneficiarioNormal)
            {
                listaBeneficiarios.Add(new BeneficiarioGrid()
                {
                    PrimerNombre = item.primerNombre,
                    SegundoNombre = item.segundoNombre,
                    PrimerApellido = item.primerApellido,
                    SegundoApellido = item.segundoApellido,
                    NumeroIdentidad = item.identidad,
                    Fecha_Nacimiento = item.fechaNacimiento,
                    Genero = item.genero,
                    Direccion = item.direccion,
                    Parentesco = item.Parentesco,
                    PorcentajeSeguros = item.porcentajeSeguros,
                    PorcentajeAportaciones = item.porcentajeAportaciones,
                    TipoBeneficiario = "Normal",
                });
            }

            //Este es el Beneficiario de Contingencia
            listaBeneficiarios.Add(new BeneficiarioGrid()
            {
                PrimerNombre = beneficiarioContingencia.primerNombre,
                SegundoNombre = beneficiarioContingencia.segundoNombre,
                PrimerApellido = beneficiarioContingencia.primerApellido,
                SegundoApellido = beneficiarioContingencia.segundoApellido,
                NumeroIdentidad = beneficiarioContingencia.identidad,
                Fecha_Nacimiento = beneficiarioContingencia.fechaNacimiento,
                Genero = beneficiarioContingencia.genero,
                Direccion = beneficiarioContingencia.direccion,
                Parentesco = beneficiarioContingencia.Parentesco,
                TipoBeneficiario = "De Contingencia",
                PorcentajeAportaciones = 100,
                PorcentajeSeguros = 100
            });

            gridBeneficiarios.ItemsSource = listaBeneficiarios;
        }

        public void getDatosEmpleado(string recievedResponce)
        {
            empleado = MainPage.tc.getEmpleado(recievedResponce);

            pNombre_txt.Text = empleado.primerNombre;
            sNombre_txt.Text = empleado.segundoNombre;
            pApellido_txt.Text = empleado.primerApellido;
            sApellido_txt.Text = empleado.segundoApellido;
            direccion_txt.Text = empleado.direccion;
            id_txt.Text = empleado.identidad;
            genero_txt.Text = empleado.genero;
            telefono_txt.Text = empleado.telefonoPersonal[0];
            celular_txt.Text = empleado.celular[0];
            profesion_txt.Text = "";
            puesto_txt.Text = empleado.Puesto;
            email_txt.Text = empleado.correoElectronico;
            lugarNacimiento_txt.Text = "";
            fechaNacimiento_txt.Text = empleado.fechaNacimiento;
            estadoCivil_txt.Text = empleado.estadoCivil;
            if (empleado.Administrador == "True")
            {
                esUsuarioAdmin_chk.IsChecked = true;
            }
            else if (empleado.Administrador == "False")
            {
                esUsuarioNorm_chk.IsChecked = true;
            }
        }
    }
}
