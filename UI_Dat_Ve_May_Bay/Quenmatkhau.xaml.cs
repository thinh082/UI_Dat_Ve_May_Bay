using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI_Dat_Ve_May_Bay
{
    /// <summary>
    /// Interaction logic for Quenmatkhau.xaml
    /// </summary>
    public partial class Quenmatkhau : Window
    {
        public Quenmatkhau()
        {
            InitializeComponent();

            // Add event handlers for placeholder text
            if (txtEmail != null)
            {
                txtEmail.GotFocus += TxtEmail_GotFocus;
                txtEmail.LostFocus += TxtEmail_LostFocus;
            }
        }

        private void TxtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmailPlaceholder != null)
                txtEmailPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void TxtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmailPlaceholder != null && string.IsNullOrEmpty(txtEmail.Text))
                txtEmailPlaceholder.Visibility = Visibility.Visible;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            DangNhap loginWindow = new DangNhap();
            loginWindow.Show();
            this.Close();
        }
    }
}
