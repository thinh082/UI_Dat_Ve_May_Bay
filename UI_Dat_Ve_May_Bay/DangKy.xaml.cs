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
    /// Interaction logic for DangKy.xaml
    /// </summary>
    public partial class DangKy : Window
    {
        public DangKy()
        {
            InitializeComponent();
            
            // Add event handlers for placeholder text
            if (txtEmail != null)
            {
                txtEmail.GotFocus += TxtEmail_GotFocus;
                txtEmail.LostFocus += TxtEmail_LostFocus;
            }

            if (txtPhone != null)
            {
                txtPhone.GotFocus += TxtPhone_GotFocus;
                txtPhone.LostFocus += TxtPhone_LostFocus;
            }

            if (txtPassword != null)
            {
                txtPassword.GotFocus += TxtPassword_GotFocus;
                txtPassword.LostFocus += TxtPassword_LostFocus;
            }

            if (txtConfirmPassword != null)
            {
                txtConfirmPassword.GotFocus += TxtConfirmPassword_GotFocus;
                txtConfirmPassword.LostFocus += TxtConfirmPassword_LostFocus;
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

        private void TxtPhone_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtPhonePlaceholder != null)
                txtPhonePlaceholder.Visibility = Visibility.Collapsed;
        }

        private void TxtPhone_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPhonePlaceholder != null && string.IsNullOrEmpty(txtPhone.Text))
                txtPhonePlaceholder.Visibility = Visibility.Visible;
        }

        private void TxtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtPasswordPlaceholder != null)
                txtPasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void TxtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPasswordPlaceholder != null && string.IsNullOrEmpty(txtPassword.Password))
                txtPasswordPlaceholder.Visibility = Visibility.Visible;
        }

        private void TxtConfirmPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtConfirmPasswordPlaceholder != null)
                txtConfirmPasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void TxtConfirmPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtConfirmPasswordPlaceholder != null && string.IsNullOrEmpty(txtConfirmPassword.Password))
                txtConfirmPasswordPlaceholder.Visibility = Visibility.Visible;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DangNhap dangNhapWindow = new DangNhap();
            dangNhapWindow.Show();
            this.Close();
        }
    }
}
