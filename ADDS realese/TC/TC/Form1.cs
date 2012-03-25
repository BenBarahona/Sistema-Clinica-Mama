using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TC
{
    public partial class Form1 : Form
    {
        TC tc = new TC();

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_afiliado_Click(object sender, EventArgs e)
        {
            afiliado afiliado = new afiliado();

            List<string> telefono = new List<string>();telefono.Add( "222-22222");telefono.Add("213-123123");
            List<string> celular = new List<string>();celular.Add( "99-9999");celular.Add( "9913-1223" );

            afiliado.primerNombre = "dany"; afiliado.primerApellido = "acosta"; afiliado.segundoNombre = "eduardo"; afiliado.segundoApellido = "carbajal"; 
            afiliado.identidad = "0801-0900-1231"; afiliado.genero = "masc"; afiliado.estadoCivil = "casado"; afiliado.direccion = "ksa"; afiliado.fechaNacimiento = "14/12/2001";
            afiliado.telefonoPersonal = telefono; afiliado.celular = celular; afiliado.CorreoElectronico = "dany@sca.com"; afiliado.NombreEmpresa = "mi kasa";afiliado.lugarDeNacimiento="ksa";
            afiliado.TelefonoEmpresa = "8765-23123"; afiliado.DireccionEmpresa = "esquina"; afiliado.fechaIngresoCooperativa = "02/12/2100"; afiliado.DepartamentoEmpresa = "IT";
            afiliado.Ocupacion = "Estudiante"; afiliado.Password = "afiliado";

            afiliado.BeneficiarioCont.primerNombre = "dany2"; afiliado.BeneficiarioCont.primerApellido = "acosta2";
            afiliado.BeneficiarioCont.segundoNombre = "eduardo2"; afiliado.BeneficiarioCont.segundoApellido = "carbajal2";
            afiliado.BeneficiarioCont.identidad = "0801-0920-1231"; afiliado.BeneficiarioCont.genero = "masculino";
            afiliado.BeneficiarioCont.estadoCivil = "casado"; afiliado.BeneficiarioCont.fechaNacimiento = "14/02/2001";
            afiliado.BeneficiarioCont.direccion = "llegando aki"; afiliado.BeneficiarioCont.parentesco = "padre";

            BeneficiarioNormal normal = new BeneficiarioNormal();
            normal.primerNombre = "dany3"; normal.primerApellido = "acosta3"; normal.parentesco = "primo"; normal.porcentajeAportaciones = 59;
            normal.segundoNombre = "eduardo3"; normal.segundoApellido = "carbajal3"; normal.porcentajeSeguros = 60;
            normal.identidad = "0801-0920-0931"; normal.genero = "masculino";
            normal.estadoCivil = "casado"; normal.fechaNacimiento = "14/05/2001"; normal.direccion = "mi ksa";

            BeneficiarioNormal [] normal2= new BeneficiarioNormal[1];

            normal2[0] = normal;
            afiliado.bensNormales = normal2;

            tc.AgregarPeticion(peticion.ingresar_afiliado, afiliado);
        }

        private void btn_empleado_Click(object sender, EventArgs e)
        {
            Empleado empleado = new Empleado();

            string[] telefono = { "222-22222", "213-123123" };
            string[] celular = { "99-9999", "9913-123123" };

            empleado.primerNombre = "dany"; empleado.primerApellido = "acosta"; empleado.segundoNombre = "eduardo"; empleado.segundoApellido = "carbajal";
            empleado.identidad = "0801-0900-1231"; empleado.genero = "masc"; empleado.estadoCivil = "casado"; empleado.direccion = "ksa"; empleado.fechaNacimiento = "14/12/2001";
            //empleado.telefonoPersonal = telefono; empleado.celular = celular;
            //empleado.PuestoEmpresaLabora = "Estudiante"; empleado.CorreoElectronico = "dany@sca.com"; 

            tc.AgregarPeticion(peticion.ingresar_empleado, empleado);
        }

        private void btn_interes_Click(object sender, EventArgs e)
        {
            Interes inter = new Interes();

            inter.Nombre = "ISV"; inter.Tasa = Convert.ToInt64(this.txtb_ingresar.Text); inter.Aplica_obligatoria = false; inter.Aplica_obligatoria_especial = false;
            inter.Aplica_voluntaria = false; inter.Aplica_voluntaria_arriba = true; inter.Monto_voluntaria = 20000;

            tc.AgregarPeticion(peticion.ingresar_interes, inter);
        }

        private void btn_ocupacion_Click(object sender, EventArgs e)
        {
            tc.AgregarPeticion(peticion.ingresar_ocupacion, this.txtb_ingresar.Text);
        }

        private void btn_motivo_Click(object sender, EventArgs e)
        {
            tc.AgregarPeticion(peticion.ingresar_motivo, this.txtb_ingresar.Text);
        }

        private void btn_monto_Click(object sender, EventArgs e)
        {
            tc.AgregarPeticion(peticion.ingresar_monto_AO, this.txtb_ingresar.Text);
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            string [] valores = new string [2];
            valores[0] = "adds.cosecol@gmail.com"; valores[1] = "kouta07";
            tc.AgregarPeticion(peticion.login, valores);
        }
    }
}
