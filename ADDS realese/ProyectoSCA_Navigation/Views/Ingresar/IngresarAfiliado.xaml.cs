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


namespace ProyectoSCA_Navigation.Views
{
    public partial class IngresarAfiliado : Page
    {
        string usuario = App.Correo;

        //Flags, true, puede disparar el evento, false no
        bool[] flags = new bool[10];
        /***    0 - Abrir Inserts
         ***    1 - Abrir Gets, luego Llenar campos de datos
         ***    2 - Error Windows
         ***/
        bool[] camposValidos = new bool[20];
        bool[] camposValidosBeneficiario = new bool[7];
        float porcentajeAportacion = 0;
        float porcentajeSeguro = 0;
        List<Object> listaBeneficiarios = new List<Object>();
        List<Clases.BeneficiarioNormal> beneficiarioNormal = new List<Clases.BeneficiarioNormal>();
        Clases.BeneficiarioContingencia beneficiarioContingencia = new Clases.BeneficiarioContingencia();
        int beneficiarioContingenciaCount = 0;
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

        public IngresarAfiliado()
        {
            InitializeComponent();
            bind = new System.ServiceModel.BasicHttpBinding();
            endpoint = new System.ServiceModel.EndpointAddress(m_EndPoint);
            Wrapper = new ServiceReference.WSCosecolSoapClient(bind, endpoint);

            fechaRegistro_txt.Content = fecha;
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

            registrar_btn.IsEnabled = false;
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            flags[3] = true;
            habilitarCampos(false);
            id_txt.IsEnabled = false;

            Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
            Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getProximoNumeroCertificado, "");
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
                        case "p01"://Ingresar Afiliado
                            if (flags[0])
                            {
                                MessageBox.Show("Se ha ingresado el afiliado exitosamente!");
                                flags[0] = false;
                                NavigationService.Refresh();
                            }
                            break;

                        case "p10"://Get Ocupaciones, refrescamos el monto actual
                            if (flags[0])
                            {
                                flags[0] = false;
                                List<string[]> lista = new List<string[]>();
                                
                                lista = MainPage.tc.getParentesco(e.Result.Substring(3));
                                if(lista != null)
                                    for (int i = 0; i < lista.Count; i++)
                                    {
                                        if((lista[i])[1] == "True")
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
                                habilitarCampos(true);
                                id_txt.IsEnabled = true;
                                List<string[]> lista = new List<string[]>();
                                lista = MainPage.tc.getParentesco(e.Result.Substring(3));
                                if (lista != null)
                                    for (int i = 0; i < lista.Count; i++)
                                    {
                                        if((lista[i])[1] == "True")
                                            parentescoBeneficiario_txt.Items.Add((lista[i])[0]);
                                    }
                            }
                            break;

                        case "p33"://Get Proximo Numero Certificado
                            if (flags[3])
                            {
                                flags[3] = false;
                                numeroCertificado_txt.Text = recievedResponce;
                                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.getOcupacion, "");
                            }
                            break;


                        default:
                            if (flags[2])
                            {
                                flags[2] = false;
                                MessageBox.Show("No se entiende la peticion. (No esta definida)");
                                habilitarCampos(true);
                                id_txt.IsEnabled = true;
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
                id_txt.IsEnabled = true;
                habilitarCampos(true);
                habilitarPantalla(true);
            }
        }


        /********************************************************************************************************************************
        **************************************************    Metodos de Botones    *****************************************************
        *********************************************************************************************************************************/
        private void registrar_btn_Click(object sender, RoutedEventArgs e)
        {
                if (pNombre_txt.Text == "" || pApellido_txt.Text == "" || id_txt.Text == "" || telefono_txt.Text == ""
                    || profesion_txt.SelectedIndex == -1 || direccion_txt.Text == "" || empresa_txt.Text == "" ||
                    email_txt.Text == "" || contrasena_txt.Password == "" || contrasena2_txt.Password == "")
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
                Clases.Afiliado afiliado = new Clases.Afiliado();
                List<string> telefono = new List<string>();
                List<string> celular = new List<string>();

                telefono.Add(telefono_txt.Text);
                telefono.Add(telefono2_txt.Text);
                celular.Add(celular_txt.Text);
                celular.Add(celular2_txt.Text);

                afiliado.primerNombre = pNombre_txt.Text;
                afiliado.segundoNombre = sNombre_txt.Text;
                afiliado.primerApellido = pApellido_txt.Text;
                afiliado.segundoApellido = sApellido_txt.Text;
                afiliado.direccion = direccion_txt.Text;
                afiliado.identidad = id_txt.Text;
                afiliado.genero = genero_txt.SelectedItem.ToString();
                afiliado.telefonoPersonal = telefono;
                afiliado.celular = celular;
                afiliado.Ocupacion = profesion_txt.SelectedItem.ToString();
                afiliado.CorreoElectronico = email_txt.Text;
                afiliado.lugarDeNacimiento = lugarNacimiento_txt.Text;
                afiliado.fechaNacimiento = fechaNacimiento_txt.Text;
                afiliado.estadoCivil = estadoCivil_txt.SelectedItem.ToString();

                afiliado.NombreEmpresa = empresa_txt.Text;
                afiliado.fechaIngresoCooperativa = fechaIngresoEmpresa_txt.Text;
                afiliado.TelefonoEmpresa = telefonoEmpresa_txt.Text;
                afiliado.DepartamentoEmpresa = departamentoEmpresa_txt.Text;
                afiliado.DireccionEmpresa = direccionEmpresa_txt.Text;

                afiliado.BeneficiarioCont = beneficiarioContingencia;
                afiliado.bensNormales = beneficiarioNormal;
                afiliado.Password = contrasena_txt.Password;


                Wrapper.AgregarPeticionCompleted += new EventHandler<ServiceReference.AgregarPeticionCompletedEventArgs>(Wrapper_AgregarPeticionCompleted);
                Wrapper.AgregarPeticionAsync(ServiceReference.peticion.ingresar_afiliado, MainPage.tc.InsertarAfiliado(afiliado, usuario));
        }


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

            

            Beneficiarios beneficiario = new Beneficiarios();
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
            else if(tipoBeneficiario_txt.SelectedIndex == 1)
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

            Beneficiarios beneficiario = new Beneficiarios();
            beneficiario = (Beneficiarios)gridBeneficiarios.SelectedItem;
            if (beneficiario.TipoBeneficiario == "De Contingencia")
                beneficiarioContingenciaCount--;

            if (beneficiario.TipoBeneficiario == "Normal")
                beneficiarioNormal.RemoveAt(gridBeneficiarios.SelectedIndex);

            listaBeneficiarios.Remove(gridBeneficiarios.SelectedItem);

            gridBeneficiarios.ItemsSource = null;
            gridBeneficiarios.ItemsSource = listaBeneficiarios;
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
                mensaje_txt.Text = "*Campos Obligatorios";
                lugarNacimiento_valid.Source = MainPage.bmpClear;
            }
        }

        private void contrasena2_txt_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (contrasena2_txt.Password == contrasena_txt.Password)
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
                mensaje_txt.Text = "*Campos Obligatorios";
                password_valid.Source = MainPage.bmpClear;
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


        /********************************************************************************************************************************
        **************************************************    Metodos Auxiliares    *****************************************************
        *********************************************************************************************************************************/
        public void habilitarCampos(bool valor)
        {
            try
            {
                registrar_btn.IsEnabled = valor;

                pNombre_txt.IsEnabled = valor;
                sNombre_txt.IsEnabled = valor;
                pApellido_txt.IsEnabled = valor;
                sApellido_txt.IsEnabled = valor;
                direccion_txt.IsEnabled = valor;
                genero_txt.IsEnabled = valor;
                telefono_txt.IsEnabled = valor;
                telefono2_txt.IsEnabled = valor;
                celular_txt.IsEnabled = valor;
                celular2_txt.IsEnabled = valor;
                profesion_txt.IsEnabled = valor;
                email_txt.IsEnabled = valor;
                lugarNacimiento_txt.IsEnabled = valor;
                fechaNacimiento_txt.IsEnabled = valor;
                contrasena_txt.IsEnabled = valor;
                contrasena2_txt.IsEnabled = valor;
                estadoCivil_txt.IsEnabled = valor;
                empresa_txt.IsEnabled = valor;
                fechaIngresoEmpresa_txt.IsEnabled = valor;
                telefonoEmpresa_txt.IsEnabled = valor;
                departamentoEmpresa_txt.IsEnabled = valor;
                direccionEmpresa_txt.IsEnabled = valor;

                beneficiarioApellido1_txt.IsEnabled = valor;
                beneficiarioApellido2_txt.IsEnabled = valor;
                beneficiarioNombre1_txt.IsEnabled = valor;
                beneficiarioNombre2_txt.IsEnabled = valor;
                beneficiarioId_txt.IsEnabled = valor;
                beneficiarioGenero_txt.IsEnabled = valor;
                fechaIngresoEmpresa_txt.IsEnabled = valor;
                fechaNacimiento_txt.IsEnabled = valor;
                parentescoBeneficiario_txt.IsEnabled = valor;
                porcentajeSeguro_txt.IsEnabled = valor;
                porcentajeAportacion_txt.IsEnabled = valor;
                tipoBeneficiario_txt.IsEnabled = valor;
                agregarBeneficiario_btn.IsEnabled = valor;
            }
            catch
            {

            }
        }

        public void habilitarPantalla(bool valor)
        {
            tabControl1.IsEnabled = valor;
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
                porcentajeSeguro_txt.IsReadOnly = false;
                porcentajeAportacion_txt.IsReadOnly = false;
            }
        }

    }

    public class Beneficiarios
    {
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string NumeroIdentidad { get; set; }
        public string TipoBeneficiario { get; set; }
        public float PorcentajeAportaciones { get; set; }
        public float PorcentajeSeguros { get; set; }
        public string Genero { get; set; }
        public string Parentesco { get; set; }
        public string Direccion { get; set; }
        public string Fecha_Nacimiento { get; set; }
    }
}
