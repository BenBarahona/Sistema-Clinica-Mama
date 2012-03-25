using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DT;

namespace DTPrueba
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        String afiliado = "0801-1991-09122,CaRLOs,JAmILCAR,SáncHEz,RosA,1991-04-07,Soltero,Masculino,col.loarque sur blqW casa " +
                "1016;226-8169,226-7522;97604843,90785634;kouta_revenge3@hotmail.com,d6288b8ae441ed58142bb70133276b72;" +
                "UNITEC,225-1234,alla cerca de los pinos," +
                "2008-06-01,Docencia,Tegucigalpa;Estudiante;0801-1991-00000,Luis,Carlos,Baquero,,1991-11-01,Soltero," +
                "Masculino,Res.Plaza,Amigo;0801-1991-00001,Daniel,Eduardo,ACOSTA,carbajal,1991-12-05,Soltero,Masculino," +
                "Res.Plaza,Alumno,50,0;0801-1990-90234,Angela,Sarahi,Velasquez,Lopez,1990-05-17,Soltero,Femenino,col. " +
                "Primavera,Pareja,50,100",

                afiliado_mod = "0801-1991-09122,CaRLOs,AmILCAR,SáncHEz,RosA,1991-04-07,Divorciado,Masculino,col.loarque sur blqW casa " +
                "1016;226-8169,226-7522;97604843,90785634;carlos.sanchez@squadventure.com,d628142bb7013327688b8ae441ed5b72;" +
                "UNITEC,225-1234,alla cerca de los pinos," +
                "2008-06-01,Docencia,Tegucigalpa;Puta;0801-1991-00000,Luis,Carlos,Baquero,,1991-11-01,Soltero," +
                "Masculino,Res.Plaza,Amigo;0801-1991-00001,Daniel,Eduardo,ACOSTA,carbajal,1991-12-05,Soltero,Masculino," +
                "Res.Plaza,Alumno,50,0;0801-1990-90234,Angela,Sarahi,Velasquez,Lopez,1990-05-17,Soltero,Femenino,col. " +
                "Primavera,Pareja,50,100",

                empleado = "0901-2011-09212,Mauricio,JAVIER,Funez,PERALTA,2010-12-31,Viudo," +
                "Masculino,zamorano;2290-9090;9999-9999,3333-3333;pedro_jose@gmail.com,d6288b8ae441ed58142bb70133276b72,Jefe IT",

                empleado_mod = "0901-2011-09213,Mauricio,JAVIER,Funez,PERALTA,2010-12-31,Casado," +
                "Masculino,zamorano;2290-9090;9999-9999,3333-3333;pedro_jose3@gmail.com,d6288b8ae441ed58142bb70133276b72,Jefe IT",

                empleado2 = "1911-2011-09212,Federico,,Andares,,2010-12-31,Viudo," +
                "Masculino,alla en la requinta pija;2290-9090;9999-9999,3333-3333;federico@gmail.com,d6288b8ae441ed58142bb70133276b72,Prostituta",

                afiliado2 = "0801-0900-1231," +
                "dany,eduardo,acosta,carbajal,2001-12-14,casado,masc,ksa;2222-2222,2133-1233;99-9999,9913-1123;dany@sca.com,d6288b8ae441ed58142bb70133276b72;mi" +
                " kasa,8765-2313,esquina,2100-12-01,IT,ksa;Estudiante;0801-0920-1231,dany2,eduardo2,acosta2,carbajal2,2001-02-14," +
                "casado,masculino,llegando aki,padre;0801-0920-0931,dany3,eduardo3,acosta3,carbajal3,2001-05-14,casado,masculino,mi" +
                " ksa,primo,60,59";

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.InsertarAfiliado(afiliado + '&' + textBox4.Text).ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.InsertarEmpleado(empleado + '&' + textBox4.Text).ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.getTodasOcupaciones());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.InsertarOcupacion(textBox1.Text + '&' + textBox4.Text).ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.DeshabilitarOcupacion(textBox1.Text + '&' + textBox4.Text).ToString());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.getTodosMotivos());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.InsertarMotivo(textBox1.Text + '&' + textBox4.Text).ToString());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.DeshabilitarMotivo(textBox1.Text + '&' + textBox4.Text).ToString());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.getTodosParentescos());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.InsertarParentesco(textBox1.Text + '&' + textBox4.Text).ToString());
        }

        private void button11_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.DeshabilitarParentesco(textBox1.Text + '&' + textBox4.Text).ToString());
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.InsertarNuevoMonto(textBox1.Text + '&' + textBox4.Text).ToString());
        }

        private void button13_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.getMontoActual().ToString());
        }

        private void button14_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.InsertarNuevoLimite(textBox1.Text + '&' + textBox4.Text).ToString());
        }

        private void button15_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.getLimiteActual().ToString());
        }

        private void button16_Click(object sender, EventArgs e)
        {
            String InteresDK = textBox1.Text + "," + textBox3.Text + "," + checkBox1.Checked.ToString() + "," + checkBox2.Checked.ToString()
                + "," + checkBox3.Checked.ToString() + "," + checkBox4.Checked.ToString() + "," + textBox2.Text + '&' + textBox4.Text;
            MessageBox.Show(DataTransaction.InsertarInteres(InteresDK).ToString());
        }

        private void button17_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.getTodosIntereses());
        }

        private void button18_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.DeshabilitarInteres(textBox1.Text + '&' + textBox4.Text).ToString());
        }

        private void button19_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.getPerfilAfiliado(textBox1.Text).ToString());
        }

        private void button20_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.getPerfilEmpleado(textBox1.Text).ToString());
        }

        private void button21_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.getInteres(textBox1.Text));
        }

        private void button24_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.SolicitarModificacionDePerfil(empleado_mod + "&"
                + textBox4.Text + "&0901-2011-09213").ToString());
        }

        private void button26_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.ModificarPerfil("0901-2011-09213").ToString());
        }

        private void button22_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataTransaction.getLogin("pedro_jose3@gmail.com,d6288b8ae441ed58142bb70133276b72").ToString());
        }



    }
}
