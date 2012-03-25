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

namespace Sistema_BD_Clinica_Patologica.Views
{
    public partial class Creditos : Page
    {
        public Creditos()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void image1_GotFocus(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Holi");
        }

    }
}
