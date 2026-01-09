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
    /// Interaction logic for DangNhap.xaml
    /// </summary>
    public partial class DangNhap : Window
    {
        public DangNhap()
        {
            InitializeComponent();
            
            // Add event handlers for placeholder text
            txtEmail.GotFocus += TxtEmail_GotFocus;
            txtEmail.LostFocus += TxtEmail_LostFocus;
            txtPassword.GotFocus += TxtPassword_GotFocus;
            txtPassword.LostFocus += TxtPassword_LostFocus;
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

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            DangKy dangKyWindow = new DangKy();
            dangKyWindow.Show();
            this.Close();
        }

        private void BtnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            Quenmatkhau quenMatKhauWindow = new Quenmatkhau();
            quenMatKhauWindow.Show();
            this.Close();
        }
    }
}
