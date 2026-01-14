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
        private bool isPasswordVisible = false;

        public DangNhap()
        {
            InitializeComponent();
            
            // Add event handlers for placeholder text
            txtEmail.GotFocus += TxtEmail_GotFocus;
            txtEmail.LostFocus += TxtEmail_LostFocus;
            txtPassword.GotFocus += TxtPassword_GotFocus;
            txtPassword.LostFocus += TxtPassword_LostFocus;
            txtPasswordVisible.GotFocus += TxtPassword_GotFocus;
            txtPasswordVisible.LostFocus += TxtPasswordVisible_LostFocus;
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

        private void TxtPasswordVisible_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPasswordPlaceholder != null && string.IsNullOrEmpty(txtPasswordVisible.Text))
                txtPasswordPlaceholder.Visibility = Visibility.Visible;
        }

        private void BtnTogglePassword_Click(object sender, RoutedEventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                // Show password as text
                txtPasswordVisible.Text = txtPassword.Password;
                txtPasswordVisible.Visibility = Visibility.Visible;
                txtPassword.Visibility = Visibility.Collapsed;
                btnTogglePassword.Content = "🙈"; // Closed eye
            }
            else
            {
                // Hide password
                txtPassword.Password = txtPasswordVisible.Text;
                txtPassword.Visibility = Visibility.Visible;
                txtPasswordVisible.Visibility = Visibility.Collapsed;
                btnTogglePassword.Content = "👁"; // Open eye
            }
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Hide placeholder when typing
            if (!string.IsNullOrEmpty(txtPassword.Password))
                txtPasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void TxtPasswordVisible_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Hide placeholder when typing
            if (!string.IsNullOrEmpty(txtPasswordVisible.Text))
                txtPasswordPlaceholder.Visibility = Visibility.Collapsed;
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
