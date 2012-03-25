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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using ProyectoSCA_Navigation.Clases;

namespace ProyectoSCA_Navigation
{
    public partial class MiPerfil : Page
    {
        string usuario = App.Correo;

        //Flags, true, puede disparar el evento, false no
        bool[] flags = new bool[10];
        /***    0 - Abrir Inserts
         ***    1 - Abrir Gets, luego Llenar campos de datos
         ***    2 - Error Windows
         ***/
        bool esAfiliado = false;
        bool esEmpleado = false;
        bool esAdministrador = false;
        bool[] camposValidos = new bool[20];
        bool[] camposValidosBeneficiario = new bool[7];
        float porcentajeAportacion = 0;
        float porcentajeSeguro = 0;
        bool modificarCampos = false;

        Afiliado afiliado = new Afiliado();
        Empleado empleado = new Empleado();

        Afiliado afiliadoViejo = new Afiliado();
        Empleado empleadoViejo = new Empleado();

        List<BeneficiarioGrid> listaBeneficiarios = new List<BeneficiarioGrid>();
        List<BeneficiarioNormal> beneficiarioNormal = new List<BeneficiarioNormal>();
        BeneficiarioContingencia beneficiarioContingencia = new BeneficiarioContingencia();
        int beneficiarioContingenciaCount = 1;
        DateTime fecha = DateTime.Now;

        //Expresiones Regulares para las validaciones
        Regex nombres = new Regex(@"^[A-Za-z][A-Za-zñ,\s-]+$");
        Regex numeros = new Regex("^[0-9]*$");
        Regex alphanumerico = new Regex(@"^[\w\s]+$");
        Regex email = new Regex(@"^[a-z0-9_.]+@[a-z0-9-]+\.[a-z]{2,4}$");
        Regex numeroIdentidad = new Regex("^[0-9]{4}-[0-9]{4}-[0-9]{5}$");
        Regex numeroTelefono = new Regex("^([(][0-9]{3}[)][0-9]{8}|[0-9]{8})$");
        Regex porcentaje = new Regex(@"^([0-9]|[1-9]\d|100)$");

        /********************************************************************************************************************************
        **************************************************    Inicializadores   *********************************************************
        *********************************************************************************************************************************/

        private System.ServiceModel.BasicHttpBinding bind;
        private System.ServiceModel.EndpointAddress endpoint;
        private ProyectoSCA_Navigation.ServiceReference.WSCosecolSoapClient Wrapper;
        private string m_EndPoint = "http://localhost:1809/WSCosecol.asmx";

        public MiPerfil()
        {
            InitializeComponent();
            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReference.WSCosecolSoapClient(bind, endpoint);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            genero_txt.Items.Add("Masculino");
            genero_txt.Items.Add("Femenino");
            estadoCivil_txt.Items.Add("Soltero");
            estadoCivil_txt.Items.Add("Casado");
            estadoCivil_txt.Items.Add("Divorciado");
            estadoCivil_txt.Items.Add("Viudo");
            estadoCivil_txt.Items.Add("Union Libre");
            estadoAfiliado_txt.Items.Add("Activo");
            estadoAfiliado_txt.Items.Add("Inactivo");
            estadoAfiliado_txt.Items.Add("Retirado");
            beneficiarioGenero_txt.Items.Add("Masculino");
            beneficiarioGenero_txt.Items.Add("Femenino");
            tipoBeneficiario_txt.Items.Add("Normal");
            tipoBeneficiario_txt.Items.Add("De Contingencia");

            genero_txt.SelectedIndex = 0;
            beneficiarioGenero_txt.SelectedIndex = 0;
            estadoCivil_txt.SelectedIndex = 0;
            estadoAfiliado_txt.SelectedIndex = 0;
            tipoBeneficiario_txt.SelectedIndex = 0;
            for (int i = 0; i < flags.Length; i++)
            {
                flags[i] = false;
            }
            for (int i = 0; i < camposValidos.Length; i++)
                camposValidos[i] = true;

            for (int i = 0; i < camposValidosBeneficiario.Length; i++)
                camposValidosBeneficiario[i] = true;

            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            flags[3] = true;
            camposIsReadOnly(true);
            habilitarPantalla(false);
            switch (App.Rol)
            {
                case "Afiliado":
                    Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                    Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getOcupacion, "");
                    break;

                case "Empleado":
                    Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                    Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getEmpleado, usuario);
                    break;

                case "Administrador":
                    Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                    Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getEmpleado, usuario);
                    break;
            }
        }

        /********************************************************************************************************************************
        *****************************************************    Metodo PRINCIPAL  ******************************************************
        *********************************************************************************************************************************/

        //es el metodo que corre cuando se dispara el evento de recibir respuesta, aqui van todas las respuestas
        //con los codigos que le pertenecen, consultar TABLA DE PETICIONES.XLSX

        private void Wrapper_AgregarPeticionCompleted(object sender, ServiceReference.AgregarPeticionCompletedEventArgs e)
        {
            habilitarPantalla(true);
            camposIsReadOnly(true);
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
                        camposIsReadOnly(true);
                        habilitarPantalla(true);
                    }
                }
                else
                {
                    switch (temp)
                    {
                        case "p08": //Solicitud Modificar Perfil
                            if (flags[0])
                            {
                                flags[0] = false;
                                MessageBox.Show("Se ha enviado una solicitud para aprobar los cambios");
                                mensaje_txt.Text = "Se ha enviado una solicitud para aprobar los cambios";
                            }
                            break;

                        case "p10"://Get Ocupaciones, refrescamos el monto actual
                            if (flags[0])
                            {
                                flags[0] = false;
                                List<string[]> lista = new List<string[]>();

                                lista = MainPage.tc.getParentesco(e.Result.Substring(3));
                                if (lista != null)
                                    for (int i = 0; i < lista.Count; i++)
                                    {
                                        if ((lista[i])[1] == "True")
                                            profesion_txt.Items.Add((lista[i])[0]);
                                    }
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getParentesco, "");
                            }
                            break;

                        case "p12"://Get Parentescos
                            if (flags[1])
                            {
                                flags[1] = false;
                                id_txt.IsEnabled = true;
                                List<string[]> lista = new List<string[]>();
                                lista = MainPage.tc.getParentesco(e.Result.Substring(3));
                                if (lista != null)
                                    for (int i = 0; i < lista.Count; i++)
                                    {
                                        if ((lista[i])[1] == "True")
                                            parentescoBeneficiario_txt.Items.Add((lista[i])[0]);
                                    }
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getAfiliado, usuario);
                            }
                            break;

                        case "p21"://Get datos Afiliado
                            if (flags[3])
                            {
                                flags[3] = false;
                                if (recievedResponce != "")
                                {
                                    habilitarPantalla(true);
                                    camposIsReadOnly(true);
                                    mensaje_txt.Text = "Datos Obtenidos!";
                                    esAfiliado = true;
                                    esEmpleado = false;
                                    afiliado = MainPage.tc.getAfiliado(recievedResponce);
                                    afiliadoViejo = MainPage.tc.getAfiliado(recievedResponce);

                                    numeroCertificado_txt.Text = afiliado.certificadoCuenta.ToString();
                                    pNombre_txt.Text = afiliado.primerNombre;
                                    sNombre_txt.Text = afiliado.segundoNombre;
                                    pApellido_txt.Text = afiliado.primerApellido;
                                    sApellido_txt.Text = afiliado.segundoApellido;
                                    direccion_txt.Text = afiliado.direccion;
                                    id_txt.Text = afiliado.identidad;
                                    genero_txt.SelectedItem = afiliado.genero;
                                    telefono_txt.Text = afiliado.telefonoPersonal[0];
                                    try
                                    {
                                        celular_txt.Text = afiliado.celular[0];
                                        telefono2_txt.Text = afiliado.telefonoPersonal[1];
                                        celular2_txt.Text = afiliado.celular[1];
                                    }
                                    catch { }
                                    profesion_txt.SelectedItem = afiliado.Ocupacion;
                                    email_txt.Text = afiliado.CorreoElectronico;
                                    lugarNacimiento_txt.Text = afiliado.lugarDeNacimiento;
                                    fechaNacimiento_txt.Text = afiliado.fechaNacimiento;
                                    estadoCivil_txt.SelectedItem = afiliado.estadoCivil;
                                    estadoAfiliado_txt.SelectedItem = afiliado.EstadoAfiliado;
                                    contrasena_txt.Password = afiliado.Password;
                                    contrasena2_txt.Password = afiliado.Password;

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
                                else
                                {
                                    MessageBox.Show("No se ha encontrado el registro!");
                                    camposIsReadOnly(true);
                                    habilitarPantalla(true);
                                }
                            }
                            break;

                        case "p22"://Get datos Empleados
                            if (flags[1])
                            {
                                flags[1] = false;
                                if (recievedResponce != "")
                                {
                                    habilitarPantalla(true);
                                    camposIsReadOnly(true);
                                    mensaje_txt.Text = "Datos Obtenidos!";
                                    datosLaborales_tab.IsEnabled = false;
                                    beneficiarios_tab.IsEnabled = false;
                                    esEmpleado = true;
                                    esAfiliado = false;
                                    empleado = MainPage.tc.getEmpleado(recievedResponce);
                                    empleadoViejo = MainPage.tc.getEmpleado(recievedResponce);

                                    label4.Content = "";
                                    numeroCertificado_txt.Text = "";
                                    pNombre_txt.Text = empleado.primerNombre;
                                    sNombre_txt.Text = empleado.segundoNombre;
                                    pApellido_txt.Text = empleado.primerApellido;
                                    sApellido_txt.Text = empleado.segundoApellido;
                                    direccion_txt.Text = empleado.direccion;
                                    id_txt.Text = empleado.identidad;
                                    genero_txt.SelectedItem = empleado.genero;
                                    telefono_txt.Text = empleado.telefonoPersonal[0];
                                    celular_txt.Text = empleado.celular[0];
                                    profesion_txt.SelectedIndex = -1;
                                    puesto_txt.Text = empleado.Puesto;
                                    email_txt.Text = empleado.correoElectronico;
                                    lugarNacimiento_txt.Text = "";
                                    fechaNacimiento_txt.Text = empleado.fechaNacimiento;
                                    estadoCivil_txt.SelectedItem = empleado.estadoCivil;
                                    contrasena_txt.Password = empleado.Password;
                                    contrasena2_txt.Password = empleado.Password;

                                    switch (App.Rol)
                                    {
                                        case "Administrador":
                                            esUsuarioAdmin_chk.IsChecked = true;
                                            break;
                                        case "Empleado":
                                            esUsuarioNorm_chk.IsChecked = true;
                                            break;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No se ha encontrado el registro!");
                                    camposIsReadOnly(true);
                                    habilitarPantalla(true);
                                }

                            }
                            break;

                        default:
                            if (flags[2])
                            {
                                flags[2] = false;
                                MessageBox.Show("No se entiende la peticion. (No esta definida)");
                                camposIsReadOnly(true);
                                habilitarPantalla(true);
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
                    MessageBox.Show("Ha ocurrido un error.  Intente su consulta de nuevo.\nSi el problema persiste, refresque la pagina e intente de nuevo.\nPuede ser que su perfil esté pendiente de confirmacion de administrador");
                }
            }
            camposIsReadOnly(true);
            habilitarPantalla(true);
        }


        /********************************************************************************************************************************
        **************************************************    Metodos de Botones    *****************************************************
        *********************************************************************************************************************************/

        private void agregarBeneficiario_btn_Click(object sender, RoutedEventArgs e)
        {
            if (beneficiarioNombre1_txt.Text == "" || beneficiarioApellido1_txt.Text == "" || beneficiarioId_txt.Text == ""
                || porcentajeAportacion_txt.Text == "" || porcentajeSeguro_txt.Text == "")
            {
                MessageBox.Show("Ingrese todos los valores marcados con *");
                return;
            }
            if (beneficiarioNacimiento_txt.Text == "")
            {
                MessageBox.Show("Ingrese una fecha valida!");
                return;
            }
            if (parentescoBeneficiario_txt.SelectedIndex == -1)
            {
                MessageBox.Show("Ingrese un parentesco!");
                return;
            }
            for (int i = 0; i < camposValidosBeneficiario.Length; i++)
            {
                if (!camposValidosBeneficiario[i])
                {
                    MessageBox.Show("Ingrese un dato valido para los campos marcados con 'X'");
                    return;
                }
            }
            if (beneficiarioContingenciaCount >= 1 && tipoBeneficiario_txt.SelectedIndex == 1)
            {
                MessageBox.Show("Solo puede ingresar 1 Beneficiario de Contingencia!");
                return;
            }

            BeneficiarioGrid beneficiario = new BeneficiarioGrid();
            beneficiario.PrimerNombre = beneficiarioNombre1_txt.Text;
            beneficiario.SegundoNombre = beneficiarioNombre2_txt.Text;
            beneficiario.PrimerApellido = beneficiarioApellido1_txt.Text;
            beneficiario.SegundoApellido = beneficiarioApellido2_txt.Text;
            beneficiario.NumeroIdentidad = beneficiarioId_txt.Text;
            beneficiario.Genero = beneficiarioGenero_txt.SelectedItem.ToString();
            beneficiario.Fecha_Nacimiento = beneficiarioNacimiento_txt.Text;
            beneficiario.Parentesco = parentescoBeneficiario_txt.SelectedItem.ToString();
            beneficiario.PorcentajeAportaciones = porcentajeAportacion;
            beneficiario.PorcentajeSeguros = porcentajeSeguro;
            beneficiario.TipoBeneficiario = tipoBeneficiario_txt.SelectedItem.ToString();

            if (tipoBeneficiario_txt.SelectedIndex == 0)
            {
                //Beneficiario Normal
                Clases.BeneficiarioNormal beneficiarioN = new Clases.BeneficiarioNormal();
                beneficiarioN.primerNombre = beneficiarioNombre1_txt.Text;
                beneficiarioN.segundoNombre = beneficiarioNombre2_txt.Text;
                beneficiarioN.primerApellido = beneficiarioApellido1_txt.Text;
                beneficiarioN.segundoApellido = beneficiarioApellido2_txt.Text;
                beneficiarioN.identidad = beneficiarioId_txt.Text;
                beneficiarioN.genero = beneficiarioGenero_txt.SelectedItem.ToString();
                beneficiarioN.fechaNacimiento = beneficiarioNacimiento_txt.Text;
                beneficiarioN.Parentesco = parentescoBeneficiario_txt.SelectedItem.ToString();
                beneficiarioN.porcentajeAportaciones = porcentajeAportacion;
                beneficiarioN.porcentajeSeguros = porcentajeSeguro;

                listaBeneficiarios.Add(beneficiario);
                beneficiarioNormal.Add(beneficiarioN);
            }
            else if (tipoBeneficiario_txt.SelectedIndex == 1)
            {
                //Beneficiario Contingencia
                beneficiarioContingenciaCount++;

                beneficiarioContingencia.primerNombre = beneficiarioNombre1_txt.Text;
                beneficiarioContingencia.segundoNombre = beneficiarioNombre2_txt.Text;
                beneficiarioContingencia.primerApellido = beneficiarioApellido1_txt.Text;
                beneficiarioContingencia.segundoApellido = beneficiarioApellido2_txt.Text;
                beneficiarioContingencia.identidad = beneficiarioId_txt.Text;
                beneficiarioContingencia.genero = beneficiarioGenero_txt.SelectedItem.ToString();
                beneficiarioContingencia.fechaNacimiento = beneficiarioNacimiento_txt.Text;
                beneficiarioContingencia.Parentesco = parentescoBeneficiario_txt.SelectedItem.ToString();

                listaBeneficiarios.Add(beneficiario);
            }

            gridBeneficiarios.ItemsSource = null;
            gridBeneficiarios.ItemsSource = listaBeneficiarios;
            resetBeneficiarios();
        }

        private void eliminarBeneficiario_btn_Click(object sender, RoutedEventArgs e)
        {
            if (gridBeneficiarios.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un Beneficiario para eliminar");
                return;
            }

            BeneficiarioGrid beneficiario = new BeneficiarioGrid();
            beneficiario = (BeneficiarioGrid)gridBeneficiarios.SelectedItem;
            if (beneficiario.TipoBeneficiario == "De Contingencia")
                beneficiarioContingenciaCount--;

            if (beneficiario.TipoBeneficiario == "Normal")
                beneficiarioNormal.RemoveAt(gridBeneficiarios.SelectedIndex);


            listaBeneficiarios.RemoveAt(gridBeneficiarios.SelectedIndex);

            gridBeneficiarios.ItemsSource = null;
            gridBeneficiarios.ItemsSource = listaBeneficiarios;
        }

        private void registrar_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!modificarCampos)
            {
                MessageBox.Show("Debe habilitar los campos clickeando 'Modificar Campos'");
                return;
            }
            if (pNombre_txt.Text == "" || pApellido_txt.Text == "" || id_txt.Text == "" || telefono_txt.Text == ""
                    || direccion_txt.Text == "" || email_txt.Text == "" || contrasena_txt.Password == "" || contrasena2_txt.Password == "")
            {
                MessageBox.Show("Ingrese todos los valores marcados con *");
                return;
            }

            if (esAfiliado && profesion_txt.SelectedIndex == -1)
            {
                MessageBox.Show("Ingrese todos los valores marcados con *");
                return;
            }

            if (fechaNacimiento_txt.Text == "")
            {
                MessageBox.Show("Ingrese una fecha de nacimiento valida!");
                return;
            }
            if (fechaIngresoEmpresa_txt.Text == "")
            {
                MessageBox.Show("Ingrese una fecha de datos laborales valida!");
                return;
            }
            if (esAfiliado)
            {
                if (listaBeneficiarios.Count == 0)
                {
                    MessageBox.Show("Debe ingresar por lo menos 1 Beneficiario Normal!");
                    return;
                }
                if (beneficiarioContingenciaCount == 0)
                {
                    MessageBox.Show("Debe ingresar 1 Beneficiario de Contingencia");
                    return;
                }
            }
            for (int i = 0; i < camposValidos.Length; i++)
            {
                if (!camposValidos[i])
                {
                    MessageBox.Show("Ingrese un dato valido para los campos marcados con 'X'");
                    return;
                }
            }

            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            habilitarPantalla(false);
            registrar_btn.IsEnabled = false;

            if (esAfiliado)
            {
                afiliado.telefonoPersonal.Clear();
                afiliado.celular.Clear();

                afiliado.primerNombre = pNombre_txt.Text;
                afiliado.segundoNombre = sNombre_txt.Text;
                afiliado.primerApellido = pApellido_txt.Text;
                afiliado.segundoApellido = sApellido_txt.Text;
                afiliado.direccion = direccion_txt.Text;
                afiliado.identidad = id_txt.Text;
                afiliado.genero = genero_txt.SelectedItem.ToString();
                afiliado.telefonoPersonal.Add(telefono_txt.Text);
                afiliado.telefonoPersonal.Add(telefono2_txt.Text);
                afiliado.celular.Add(celular_txt.Text);
                afiliado.celular.Add(celular2_txt.Text);
                afiliado.CorreoElectronico = email_txt.Text;
                afiliado.Ocupacion = profesion_txt.SelectedItem.ToString();
                afiliado.estadoCivil = estadoCivil_txt.SelectedItem.ToString();
                afiliado.fechaNacimiento = fechaNacimiento_txt.Text;
                afiliado.lugarDeNacimiento = lugarNacimiento_txt.Text;
                afiliado.Password = contrasena_txt.Password;
                afiliado.EstadoAfiliado = estadoAfiliado_txt.SelectedItem.ToString();

                afiliado.NombreEmpresa = empresa_txt.Text;
                afiliado.fechaIngresoCooperativa = fechaIngresoEmpresa_txt.Text;
                afiliado.TelefonoEmpresa = telefonoEmpresa_txt.Text;
                afiliado.DepartamentoEmpresa = departamentoEmpresa_txt.Text;
                afiliado.DireccionEmpresa = direccionEmpresa_txt.Text;

                afiliado.BeneficiarioCont = beneficiarioContingencia;
                afiliado.bensNormales = beneficiarioNormal;

                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.solicitudModificarPerfil, MainPage.tc.SolicitudModificarAfiliado(afiliado, usuario, afiliadoViejo.identidad));
            }

            if (esEmpleado)
            {
                empleado.primerNombre = pNombre_txt.Text;
                empleado.segundoNombre = sNombre_txt.Text;
                empleado.primerApellido = pApellido_txt.Text;
                empleado.segundoApellido = sApellido_txt.Text;
                empleado.direccion = direccion_txt.Text;
                empleado.identidad = id_txt.Text;
                empleado.genero = genero_txt.SelectedItem.ToString();
                empleado.telefonoPersonal.Add(telefono_txt.Text);
                empleado.celular.Add(celular_txt.Text);
                empleado.correoElectronico = email_txt.Text;
                empleado.Puesto = puesto_txt.Text;
                empleado.estadoCivil = estadoCivil_txt.SelectedItem.ToString();
                empleado.fechaNacimiento = fechaNacimiento_txt.Text;
                empleado.Password = contrasena_txt.Password;

                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.solicitudModificarPerfil, MainPage.tc.SolicitudModificarEmpleado(empleado, usuario, empleadoViejo.identidad, esAdministrador));
            }
        }


        /********************************************************************************************************************************
        **************************************************    Validaciones!!   **********************************************************
        *********************************************************************************************************************************/

        //Validaciones de valores ingresados
        private void pNombre_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            pNombre_txt.Text.Trim();
            if (nombres.IsMatch(pNombre_txt.Text))
            {
                camposValidos[0] = true;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                pNombre_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[0] = false;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                pNombre_valid.Source = new BitmapImage(imgURI);
            }
            if (pNombre_txt.Text == "")
            {
                camposValidos[0] = true;
                registrar_btn.IsEnabled = true;
                pNombre_valid.Source = MainPage.bmpClear;
            }
        }

        private void sNombre_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            sNombre_txt.Text.Trim();
            if (nombres.IsMatch(sNombre_txt.Text))
            {
                camposValidos[1] = true;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                sNombre_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[1] = false;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                sNombre_valid.Source = new BitmapImage(imgURI);
            }
            if (sNombre_txt.Text == "")
            {
                camposValidos[1] = true;
                registrar_btn.IsEnabled = true;
                sNombre_valid.Source = MainPage.bmpClear;
            }
        }

        private void pApellido_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            pApellido_txt.Text.Trim();
            if (nombres.IsMatch(pApellido_txt.Text))
            {
                camposValidos[2] = true;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                pApellido_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[2] = false;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                pApellido_valid.Source = new BitmapImage(imgURI);
            }
            if (pApellido_txt.Text == "")
            {
                camposValidos[2] = true;
                registrar_btn.IsEnabled = true;
                pApellido_valid.Source = MainPage.bmpClear;
            }
        }

        private void sApellido_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            sApellido_txt.Text.Trim();
            if (nombres.IsMatch(sApellido_txt.Text))
            {
                camposValidos[3] = true;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                sApellido_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[3] = false;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                sApellido_valid.Source = new BitmapImage(imgURI);
            }
            if (sApellido_txt.Text == "")
            {
                camposValidos[3] = true;
                registrar_btn.IsEnabled = true;
                sApellido_valid.Source = MainPage.bmpClear;
            }
        }

        private void id_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            id_txt.Text.Trim();
            if (numeroIdentidad.IsMatch(id_txt.Text))
            {
                camposValidos[4] = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[4] = false;
                mensaje_txt.Text = "Formato valido de Identidad: 0000-0000-00000";
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                id_valid.Source = new BitmapImage(imgURI);
            }
            if (id_txt.Text == "")
            {
                camposValidos[4] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                id_valid.Source = MainPage.bmpClear;
            }
        }

        private void telefono_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            telefono_txt.Text.Trim();
            if (numeroTelefono.IsMatch(telefono_txt.Text))
            {
                camposValidos[5] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                telefono_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[5] = false;
                registrar_btn.IsEnabled = false;
                mensaje_txt.Text = "Formato Valido: (504)0000000, ó 00000000";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                telefono_valid.Source = new BitmapImage(imgURI);
            }
            if (telefono_txt.Text == "")
            {
                camposValidos[5] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                telefono_valid.Source = MainPage.bmpClear;
            }
        }

        private void telefono2_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            telefono2_txt.Text.Trim();
            if (numeroTelefono.IsMatch(telefono2_txt.Text))
            {
                camposValidos[6] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                telefono2_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[6] = false;
                registrar_btn.IsEnabled = false;
                mensaje_txt.Text = "Formato Valido: (504)0000000, ó 00000000";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                telefono2_valid.Source = new BitmapImage(imgURI);
            }
            if (telefono2_txt.Text == "")
            {
                camposValidos[6] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                telefono2_valid.Source = MainPage.bmpClear;
            }
        }

        private void celular_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            celular_txt.Text.Trim();
            if (numeroTelefono.IsMatch(celular_txt.Text))
            {
                camposValidos[7] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                celular_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[7] = false;
                registrar_btn.IsEnabled = false;
                mensaje_txt.Text = "Formato Valido: (504)0000000, ó 00000000";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                celular_valid.Source = new BitmapImage(imgURI);
            }
            if (celular_txt.Text == "")
            {
                camposValidos[7] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                celular_valid.Source = MainPage.bmpClear;
            }
        }

        private void celular2_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            celular2_txt.Text.Trim();
            if (numeroTelefono.IsMatch(celular2_txt.Text))
            {
                camposValidos[8] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                celular2_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[8] = false;
                registrar_btn.IsEnabled = false;
                mensaje_txt.Text = "Formato Valido: (504)0000000, ó 00000000";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                celular2_valid.Source = new BitmapImage(imgURI);
            }
            if (celular2_txt.Text == "")
            {
                camposValidos[8] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                celular2_valid.Source = MainPage.bmpClear;
            }
        }

        private void email_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            email_txt.Text.Trim();
            if (email.IsMatch(email_txt.Text))
            {
                camposValidos[9] = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                correo_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[9] = false;
                registrar_btn.IsEnabled = false;
                mensaje_txt.Text = "Formato Valido de Correo: ejemplo@dominio.com";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                correo_valid.Source = new BitmapImage(imgURI);
            }
            if (email_txt.Text == "")
            {
                camposValidos[9] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                correo_valid.Source = MainPage.bmpClear;
            }
        }

        private void lugarNacimiento_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            lugarNacimiento_txt.Text.Trim();
            if (nombres.IsMatch(lugarNacimiento_txt.Text))
            {
                camposValidos[10] = true;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                lugarNacimiento_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[10] = false;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                lugarNacimiento_valid.Source = new BitmapImage(imgURI);
            }
            if (lugarNacimiento_txt.Text == "")
            {
                camposValidos[10] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                lugarNacimiento_valid.Source = MainPage.bmpClear;
            }
        }

        private void empresa_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            empresa_txt.Text.Trim();
            if (nombres.IsMatch(empresa_txt.Text))
            {
                camposValidos[12] = true;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                empresa_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[12] = false;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                empresa_valid.Source = new BitmapImage(imgURI);
            }
            if (empresa_txt.Text == "")
            {
                camposValidos[12] = true;
                registrar_btn.IsEnabled = true;
                empresa_valid.Source = MainPage.bmpClear;
            }
        }

        private void telefonoEmpresa_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            telefonoEmpresa_txt.Text.Trim();
            if (numeroTelefono.IsMatch(telefonoEmpresa_txt.Text))
            {
                camposValidos[13] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                telefonoEmpresa_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[13] = false;
                registrar_btn.IsEnabled = false;
                mensaje_txt.Text = "Formato Valido: (504)0000000, ó 00000000";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                telefonoEmpresa_valid.Source = new BitmapImage(imgURI);
            }
            if (telefonoEmpresa_txt.Text == "")
            {
                camposValidos[13] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                telefonoEmpresa_valid.Source = MainPage.bmpClear;
            }
        }

        private void departamentoEmpresa_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            departamentoEmpresa_txt.Text.Trim();
            if (alphanumerico.IsMatch(departamentoEmpresa_txt.Text))
            {
                camposValidos[14] = true;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                departamentoEmpresa_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[14] = false;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                departamentoEmpresa_valid.Source = new BitmapImage(imgURI);
            }
            if (departamentoEmpresa_txt.Text == "")
            {
                camposValidos[14] = true;
                registrar_btn.IsEnabled = true;
                departamentoEmpresa_valid.Source = MainPage.bmpClear;
            }
        }

        private void beneficiarioNombre1_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            beneficiarioNombre1_txt.Text.Trim();
            if (nombres.IsMatch(beneficiarioNombre1_txt.Text))
            {
                camposValidosBeneficiario[0] = true;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                beneficiarioPNombre_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidosBeneficiario[0] = false;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                beneficiarioPNombre_valid.Source = new BitmapImage(imgURI);
            }
            if (beneficiarioNombre1_txt.Text == "")
            {
                camposValidosBeneficiario[0] = true;
                registrar_btn.IsEnabled = true;
                beneficiarioPNombre_valid.Source = MainPage.bmpClear;
            }
        }

        private void beneficiarioNombre2_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            beneficiarioNombre2_txt.Text.Trim();
            if (nombres.IsMatch(beneficiarioNombre2_txt.Text))
            {
                camposValidosBeneficiario[1] = true;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                beneficiarioSNombre_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidosBeneficiario[1] = false;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                beneficiarioSNombre_valid.Source = new BitmapImage(imgURI);
            }
            if (beneficiarioNombre2_txt.Text == "")
            {
                camposValidosBeneficiario[1] = true;
                registrar_btn.IsEnabled = true;
                beneficiarioSNombre_valid.Source = MainPage.bmpClear;
            }
        }

        private void beneficiarioApellido1_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            beneficiarioApellido1_txt.Text.Trim();
            if (nombres.IsMatch(beneficiarioApellido1_txt.Text))
            {
                camposValidosBeneficiario[2] = true;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                beneficiarioPApellido_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidosBeneficiario[2] = false;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                beneficiarioPApellido_valid.Source = new BitmapImage(imgURI);
            }
            if (beneficiarioApellido1_txt.Text == "")
            {
                camposValidosBeneficiario[2] = true;
                registrar_btn.IsEnabled = true;
                beneficiarioPApellido_valid.Source = MainPage.bmpClear;
            }
        }

        private void beneficiarioApellido2_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            beneficiarioApellido2_txt.Text.Trim();
            if (nombres.IsMatch(beneficiarioApellido2_txt.Text))
            {
                camposValidosBeneficiario[3] = true;
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                beneficiarioSApellido_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidosBeneficiario[3] = false;
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                beneficiarioSApellido_valid.Source = new BitmapImage(imgURI);
            }
            if (beneficiarioApellido2_txt.Text == "")
            {
                camposValidosBeneficiario[3] = true;
                registrar_btn.IsEnabled = true;
                beneficiarioSApellido_valid.Source = MainPage.bmpClear;
            }
        }

        private void beneficiarioId_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            beneficiarioId_txt.Text.Trim();
            if (numeroIdentidad.IsMatch(beneficiarioId_txt.Text))
            {
                camposValidosBeneficiario[4] = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                beneficiarioId_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidosBeneficiario[4] = false;
                mensaje_txt.Text = "Formato valido de Identidad: 0000-0000-00000";
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                beneficiarioId_valid.Source = new BitmapImage(imgURI);
            }
            if (beneficiarioId_txt.Text == "")
            {
                camposValidosBeneficiario[4] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                beneficiarioId_valid.Source = MainPage.bmpClear;
            }
        }

        private void porcentajeAportacion_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            porcentajeAportacion_txt.Text.Trim();
            try
            {
                porcentajeAportacion = Convert.ToInt32(porcentajeAportacion_txt.Text);
            }
            catch { }

            if (porcentaje.IsMatch(porcentajeAportacion_txt.Text))
            {
                camposValidosBeneficiario[5] = true;
                mensaje_txt.Text = "*Campos Obligarios";
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                porcentajeAportacion_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidosBeneficiario[5] = false;
                mensaje_txt.Text = "Valor debe ser entre 0 y 100\nLa suma de los % debe ser menor o igual a 100";
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                porcentajeAportacion_valid.Source = new BitmapImage(imgURI);
            }
            if (porcentajeAportacion_txt.Text == "")
            {
                camposValidosBeneficiario[5] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligarios";
                porcentajeAportacion_valid.Source = MainPage.bmpClear;
            }
        }

        private void porcentajeSeguro_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            porcentajeSeguro_txt.Text.Trim();
            try
            {
                porcentajeSeguro = Convert.ToInt32(porcentajeSeguro_txt.Text);
            }
            catch { }

            if (porcentaje.IsMatch(porcentajeSeguro_txt.Text))
            {

                camposValidosBeneficiario[6] = true;
                mensaje_txt.Text = "*Campos Obligarios";
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                porcentajeSeguro_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidosBeneficiario[6] = false;
                mensaje_txt.Text = "Valor debe ser entre 0 y 100\nLa suma de los % debe ser menor o igual a 100";
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                porcentajeSeguro_valid.Source = new BitmapImage(imgURI);
            }
            if (porcentajeSeguro_txt.Text == "")
            {
                camposValidosBeneficiario[6] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligarios";
                porcentajeSeguro_valid.Source = MainPage.bmpClear;
            }
        }

        private void contrasena_txt_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (contrasena_txt.Password == contrasena2_txt.Password)
            {
                camposValidos[11] = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                registrar_btn.IsEnabled = true;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                password_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                camposValidos[11] = false;
                mensaje_txt.Text = "Las contraseñas no coinciden!";
                registrar_btn.IsEnabled = false;
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                password_valid.Source = new BitmapImage(imgURI);
            }
            if (contrasena2_txt.Password == "" && contrasena_txt.Password == "")
            {
                camposValidos[11] = true;
                registrar_btn.IsEnabled = true;
                mensaje_txt.Text = "*Campos Obligatorios";
                password_valid.Source = MainPage.bmpClear;
            }
        }


        /********************************************************************************************************************************
        **************************************************    Metodos Auxiliares    *****************************************************
        *********************************************************************************************************************************/

        public void camposIsReadOnly(bool valor)
        {
            pNombre_txt.IsReadOnly = valor;
            sNombre_txt.IsReadOnly = valor;
            pApellido_txt.IsReadOnly = valor;
            sApellido_txt.IsReadOnly = valor;
            direccion_txt.IsReadOnly = valor;
            genero_txt.IsEnabled = !valor;
            telefono_txt.IsReadOnly = valor;
            celular_txt.IsReadOnly = valor;
            email_txt.IsReadOnly = valor;
            fechaNacimiento_txt.IsEnabled = !valor;
            estadoCivil_txt.IsEnabled = !valor;
            estadoAfiliado_txt.IsEnabled = !valor;
            empresa_txt.IsReadOnly = valor;
            fechaIngresoEmpresa_txt.IsEnabled = !valor;
            telefonoEmpresa_txt.IsReadOnly = valor;
            departamentoEmpresa_txt.IsReadOnly = valor;
            direccionEmpresa_txt.IsReadOnly = valor;
            id_txt.IsReadOnly = valor;
            beneficiarioApellido1_txt.IsReadOnly = valor;
            beneficiarioApellido2_txt.IsReadOnly = valor;
            beneficiarioNombre1_txt.IsReadOnly = valor;
            beneficiarioNombre2_txt.IsReadOnly = valor;
            beneficiarioId_txt.IsReadOnly = valor;
            beneficiarioGenero_txt.IsEnabled = !valor;
            fechaIngresoEmpresa_txt.IsEnabled = !valor;
            fechaNacimiento_txt.IsEnabled = !valor;
            parentescoBeneficiario_txt.IsEnabled = !valor;
            porcentajeSeguro_txt.IsReadOnly = valor;
            porcentajeAportacion_txt.IsReadOnly = valor;
            tipoBeneficiario_txt.IsEnabled = !valor;
            agregarBeneficiario_btn.IsEnabled = !valor;
            eliminarBeneficiario_btn.IsEnabled = !valor;
            telefono2_txt.IsReadOnly = valor;
            celular2_txt.IsReadOnly = valor;
            profesion_txt.IsEnabled = !valor;
            lugarNacimiento_txt.IsReadOnly = valor;
            puesto_txt.IsReadOnly = valor;
            contrasena_txt.IsEnabled = !valor;
            contrasena2_txt.IsEnabled = !valor;
            estadoAfiliado_txt.IsEnabled = !valor;
            esUsuarioAdmin_chk.IsEnabled = !valor;
            esUsuarioNorm_chk.IsEnabled = !valor;

            if (esAfiliado)
            {
                puesto_txt.IsReadOnly = true;
                esUsuarioAdmin_chk.IsEnabled = false;
                esUsuarioNorm_chk.IsEnabled = false;
            }
            else if (esEmpleado)
            {
                telefono2_txt.IsReadOnly = true;
                celular2_txt.IsReadOnly = true;
                profesion_txt.IsEnabled = false;
                lugarNacimiento_txt.IsReadOnly = true;
                estadoAfiliado_txt.IsEnabled = false;
            }
        }

        public void habilitarPantalla(bool valor)
        {
            tabControl1.IsEnabled = valor;
            registrar_btn.IsEnabled = valor;
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            camposIsReadOnly(false);
            modificarCampos = true;
            registrar_btn.IsEnabled = true;
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            camposIsReadOnly(true);
            modificarCampos = false;
            registrar_btn.IsEnabled = false;
        }

        private void esUsuarioAdmin_chk_Checked(object sender, RoutedEventArgs e)
        {
            esAdministrador = true;
        }

        private void esUsuarioNorm_chk_Checked(object sender, RoutedEventArgs e)
        {
            esAdministrador = false;
        }

        private void resetBeneficiarios()
        {
            beneficiarioNombre1_txt.Text = "";
            beneficiarioNombre2_txt.Text = "";
            beneficiarioApellido1_txt.Text = "";
            beneficiarioApellido2_txt.Text = "";
            beneficiarioId_txt.Text = "";
            beneficiarioGenero_txt.SelectedIndex = 0;
            beneficiarioNacimiento_txt.Text = "";
            parentescoBeneficiario_txt.SelectedIndex = 0;
            porcentajeSeguro_txt.Text = "";
            porcentajeAportacion_txt.Text = "";
            tipoBeneficiario_txt.SelectedIndex = 0;
        }

        private void tipoBeneficiario_txt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tipoBeneficiario_txt.SelectedIndex == 1)
            {
                porcentajeSeguro_txt.Text = "100";
                porcentajeAportacion_txt.Text = "100";
                porcentajeSeguro_txt.IsReadOnly = true;
                porcentajeAportacion_txt.IsReadOnly = true;
            }
            else
            {
                porcentajeSeguro_txt.Text = "0";
                porcentajeAportacion_txt.Text = "0";
                porcentajeSeguro_txt.IsReadOnly = false;
                porcentajeAportacion_txt.IsReadOnly = false;
            }
        }

    }
}