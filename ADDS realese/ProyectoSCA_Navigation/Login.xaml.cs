﻿using System;
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
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace ProyectoSCA_Navigation
{
    public partial class Login : ChildWindow
    {
        Regex email = new Regex(@"^[a-z0-9_.]+@[a-z0-9-]+\.[a-z]{2,4}$");
        public String Usuario
        {
            get { return this.txt_correo.Text; }
        }

        public String Password
        {
            get { return this.pwd_box.Password; }
        }

        public Login()
        {
            InitializeComponent();
            txt_correo.Text = "sca@sca.com";
            pwd_box.Password = "kouta07";
        }

        /********************************************************************************************************************************
        **************************************************    Metodos de Botones    *****************************************************
        *********************************************************************************************************************************/

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.txt_correo.Text != "" && this.pwd_box.Password != "")
            {
                this.txt_correo.Text.Trim();
                if (!email.IsMatch(this.txt_correo.Text))
                    this.txt_msj.Text = "Formato Valido de Correo:\n ejemplo@dominio.com";
                else
                {
                    this.txt_msj.Text = "Formato Correcto";
                    this.enableButtons(true);
                    this.DialogResult = true;
                    App.UserIsAuthenticated = true;
                }
            }
            else
            {
                MessageBox.Show("Ingrese un usuario y contraseña");
            }
        }

        /********************************************************************************************************************************
        **************************************************    Metodos Auxiliares    *****************************************************
        *********************************************************************************************************************************/

        private void pwd_keyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OKButton_Click(sender, e);
            }
        }

        private void enableButtons(Boolean valor)
        {
            this.OKButton.IsEnabled = valor;
        }

        /********************************************************************************************************************************
        **************************************************    Validaciones!!   **********************************************************
        *********************************************************************************************************************************/  

        private void correo_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.txt_correo.Text.Trim();
            if (email.IsMatch(txt_correo.Text))
            {
                string sURL = "/ProyectoSCA_Navigation;component/Images/Good.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                correo_valid.Source = new BitmapImage(imgURI);
            }
            else
            {
                this.txt_msj.Text = "Formato Valido de Correo:\n ejemplo@dominio.com";
                string sURL = "/ProyectoSCA_Navigation;component/Images/Bad.png";
                Uri imgURI = new Uri(sURL, UriKind.Relative);
                correo_valid.Source = new BitmapImage(imgURI);
            }
            if (txt_correo.Text == "")
            {
                correo_valid.Source = MainPage.bmpClear;
            }
        }

        private void pwd_Changed(object sender, RoutedEventArgs e)
        {
            if (this.pwd_box.Password != "")
            {
                this.txt_msj.Text = "Formato Correcto";
                this.enableButtons(true);
            }
            else
            {
                this.txt_msj.Text = "Debe ingresar una contraseña.";
                this.enableButtons(false);
            }
        }
    }
}