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
using System.Windows.Media.Imaging;

namespace Sistema_BD_Clinica_Patologica.Views
{
    public partial class PrintPreview : ChildWindow
    {
        public bool printFlag;

        public PrintPreview()
        {
            InitializeComponent();
        }
        /*
        public void ShowPreview(Grid space)
        {
                
        }
        */
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

