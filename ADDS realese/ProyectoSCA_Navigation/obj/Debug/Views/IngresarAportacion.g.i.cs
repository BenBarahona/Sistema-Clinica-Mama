﻿#pragma checksum "C:\Users\Ben\Dropbox\ADDS realese\ProyectoSCA_Navigation\Views\IngresarAportacion.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9B7F55198EBCEAD4A074D0C8AEF827C9"
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


namespace ProyectoSCA_Navigation.Views {
    
    
    public partial class IngresarAportacion : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ScrollViewer PageScrollViewer;
        
        internal System.Windows.Controls.Grid grid2;
        
        internal System.Windows.Controls.Label label3;
        
        internal System.Windows.Controls.DataGrid gridAportaciones;
        
        internal System.Windows.Controls.Button registrar_btn;
        
        internal System.Windows.Controls.Border border1;
        
        internal System.Windows.Controls.Grid grid3;
        
        internal System.Windows.Controls.ComboBox numeroCertificado_txt;
        
        internal System.Windows.Controls.Label label4;
        
        internal System.Windows.Controls.Label label5;
        
        internal System.Windows.Controls.Label id;
        
        internal System.Windows.Controls.AutoCompleteBox nombreAuto_txt;
        
        internal System.Windows.Controls.Button buscar_btn;
        
        internal System.Windows.Controls.TextBox id_txt;
        
        internal System.Windows.Controls.TextBlock textBlock1;
        
        internal System.Windows.Controls.Image id_valid;
        
        internal System.Windows.Controls.TextBlock mensaje_txt;
        
        internal System.Windows.Controls.TabControl tabControl1;
        
        internal System.Windows.Controls.TabItem tabItem1;
        
        internal System.Windows.Controls.Label label10;
        
        internal System.Windows.Controls.DatePicker fechaAO_txt;
        
        internal System.Windows.Controls.DataGrid gridMesesPorPagar;
        
        internal System.Windows.Controls.Label label11;
        
        internal System.Windows.Controls.Button agregar_btn;
        
        internal System.Windows.Controls.TabItem tabItem2;
        
        internal System.Windows.Controls.Label label6;
        
        internal System.Windows.Controls.TextBox descripcionAV_txt;
        
        internal System.Windows.Controls.Label label9;
        
        internal System.Windows.Controls.DatePicker fechaAV_txt;
        
        internal System.Windows.Controls.Label label12;
        
        internal System.Windows.Controls.Button agregarAV_btn;
        
        internal System.Windows.Controls.Image monto_valid;
        
        internal System.Windows.Controls.TextBox montoAV_txt;
        
        internal System.Windows.Controls.Button eliminar_btn;
        
        internal System.Windows.Controls.Button limpiar_btn;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ProyectoSCA_Navigation;component/Views/IngresarAportacion.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.PageScrollViewer = ((System.Windows.Controls.ScrollViewer)(this.FindName("PageScrollViewer")));
            this.grid2 = ((System.Windows.Controls.Grid)(this.FindName("grid2")));
            this.label3 = ((System.Windows.Controls.Label)(this.FindName("label3")));
            this.gridAportaciones = ((System.Windows.Controls.DataGrid)(this.FindName("gridAportaciones")));
            this.registrar_btn = ((System.Windows.Controls.Button)(this.FindName("registrar_btn")));
            this.border1 = ((System.Windows.Controls.Border)(this.FindName("border1")));
            this.grid3 = ((System.Windows.Controls.Grid)(this.FindName("grid3")));
            this.numeroCertificado_txt = ((System.Windows.Controls.ComboBox)(this.FindName("numeroCertificado_txt")));
            this.label4 = ((System.Windows.Controls.Label)(this.FindName("label4")));
            this.label5 = ((System.Windows.Controls.Label)(this.FindName("label5")));
            this.id = ((System.Windows.Controls.Label)(this.FindName("id")));
            this.nombreAuto_txt = ((System.Windows.Controls.AutoCompleteBox)(this.FindName("nombreAuto_txt")));
            this.buscar_btn = ((System.Windows.Controls.Button)(this.FindName("buscar_btn")));
            this.id_txt = ((System.Windows.Controls.TextBox)(this.FindName("id_txt")));
            this.textBlock1 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock1")));
            this.id_valid = ((System.Windows.Controls.Image)(this.FindName("id_valid")));
            this.mensaje_txt = ((System.Windows.Controls.TextBlock)(this.FindName("mensaje_txt")));
            this.tabControl1 = ((System.Windows.Controls.TabControl)(this.FindName("tabControl1")));
            this.tabItem1 = ((System.Windows.Controls.TabItem)(this.FindName("tabItem1")));
            this.label10 = ((System.Windows.Controls.Label)(this.FindName("label10")));
            this.fechaAO_txt = ((System.Windows.Controls.DatePicker)(this.FindName("fechaAO_txt")));
            this.gridMesesPorPagar = ((System.Windows.Controls.DataGrid)(this.FindName("gridMesesPorPagar")));
            this.label11 = ((System.Windows.Controls.Label)(this.FindName("label11")));
            this.agregar_btn = ((System.Windows.Controls.Button)(this.FindName("agregar_btn")));
            this.tabItem2 = ((System.Windows.Controls.TabItem)(this.FindName("tabItem2")));
            this.label6 = ((System.Windows.Controls.Label)(this.FindName("label6")));
            this.descripcionAV_txt = ((System.Windows.Controls.TextBox)(this.FindName("descripcionAV_txt")));
            this.label9 = ((System.Windows.Controls.Label)(this.FindName("label9")));
            this.fechaAV_txt = ((System.Windows.Controls.DatePicker)(this.FindName("fechaAV_txt")));
            this.label12 = ((System.Windows.Controls.Label)(this.FindName("label12")));
            this.agregarAV_btn = ((System.Windows.Controls.Button)(this.FindName("agregarAV_btn")));
            this.monto_valid = ((System.Windows.Controls.Image)(this.FindName("monto_valid")));
            this.montoAV_txt = ((System.Windows.Controls.TextBox)(this.FindName("montoAV_txt")));
            this.eliminar_btn = ((System.Windows.Controls.Button)(this.FindName("eliminar_btn")));
            this.limpiar_btn = ((System.Windows.Controls.Button)(this.FindName("limpiar_btn")));
        }
    }
}

