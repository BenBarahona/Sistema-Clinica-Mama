﻿#pragma checksum "C:\Users\Ben\Dropbox\ADDS realese\ProyectoSCA_Navigation\Login.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A58640494198E5ED32C06946CB9AF0AF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace ProyectoSCA_Navigation {
    
    
    public partial class Login : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Button OKButton;
        
        internal System.Windows.Controls.Label lbl_correo;
        
        internal System.Windows.Controls.TextBox txt_correo;
        
        internal System.Windows.Controls.Label lbl_pwd;
        
        internal System.Windows.Controls.PasswordBox pwd_box;
        
        internal System.Windows.Controls.Label lbl_login;
        
        internal System.Windows.Controls.TextBlock txt_msj;
        
        internal System.Windows.Controls.Image correo_valid;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/ProyectoSCA_Navigation;component/Login.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
            this.lbl_correo = ((System.Windows.Controls.Label)(this.FindName("lbl_correo")));
            this.txt_correo = ((System.Windows.Controls.TextBox)(this.FindName("txt_correo")));
            this.lbl_pwd = ((System.Windows.Controls.Label)(this.FindName("lbl_pwd")));
            this.pwd_box = ((System.Windows.Controls.PasswordBox)(this.FindName("pwd_box")));
            this.lbl_login = ((System.Windows.Controls.Label)(this.FindName("lbl_login")));
            this.txt_msj = ((System.Windows.Controls.TextBlock)(this.FindName("txt_msj")));
            this.correo_valid = ((System.Windows.Controls.Image)(this.FindName("correo_valid")));
        }
    }
}

