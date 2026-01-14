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
    /// Interaction logic for Nhapmatkhaumoi.xaml
    /// </summary>
    public partial class Nhapmatkhaumoi : Window
    {
        private bool isNewPasswordVisible = false;
        private bool isConfirmPasswordVisible = false;

        public Nhapmatkhaumoi()
        {
            InitializeComponent();
            
            // Add event handlers for placeholder text
            txtNewPassword.GotFocus += TxtNewPassword_GotFocus;
            txtNewPassword.LostFocus += TxtNewPassword_LostFocus;
            txtNewPasswordVisible.GotFocus += TxtNewPassword_GotFocus;
            txtNewPasswordVisible.LostFocus += TxtNewPasswordVisible_LostFocus;
            txtConfirmPassword.GotFocus += TxtConfirmPassword_GotFocus;
            txtConfirmPassword.LostFocus += TxtConfirmPassword_LostFocus;
            txtConfirmPasswordVisible.GotFocus += TxtConfirmPassword_GotFocus;
            txtConfirmPasswordVisible.LostFocus += TxtConfirmPasswordVisible_LostFocus;
        }

        private void TxtNewPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtNewPasswordPlaceholder != null)
                txtNewPasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void TxtNewPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtNewPasswordPlaceholder != null && string.IsNullOrEmpty(txtNewPassword.Password))
                txtNewPasswordPlaceholder.Visibility = Visibility.Visible;
        }

        private void TxtNewPasswordVisible_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtNewPasswordPlaceholder != null && string.IsNullOrEmpty(txtNewPasswordVisible.Text))
                txtNewPasswordPlaceholder.Visibility = Visibility.Visible;
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

        private void TxtConfirmPasswordVisible_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtConfirmPasswordPlaceholder != null && string.IsNullOrEmpty(txtConfirmPasswordVisible.Text))
                txtConfirmPasswordPlaceholder.Visibility = Visibility.Visible;
        }

        private void BtnToggleNewPassword_Click(object sender, RoutedEventArgs e)
        {
            isNewPasswordVisible = !isNewPasswordVisible;

            if (isNewPasswordVisible)
            {
                txtNewPasswordVisible.Text = txtNewPassword.Password;
                txtNewPasswordVisible.Visibility = Visibility.Visible;
                txtNewPassword.Visibility = Visibility.Collapsed;
                btnToggleNewPassword.Content = "🙈";
            }
            else
            {
                txtNewPassword.Password = txtNewPasswordVisible.Text;
                txtNewPassword.Visibility = Visibility.Visible;
                txtNewPasswordVisible.Visibility = Visibility.Collapsed;
                btnToggleNewPassword.Content = "👁";
            }
        }

        private void BtnToggleConfirmPassword_Click(object sender, RoutedEventArgs e)
        {
            isConfirmPasswordVisible = !isConfirmPasswordVisible;

            if (isConfirmPasswordVisible)
            {
                txtConfirmPasswordVisible.Text = txtConfirmPassword.Password;
                txtConfirmPasswordVisible.Visibility = Visibility.Visible;
                txtConfirmPassword.Visibility = Visibility.Collapsed;
                btnToggleConfirmPassword.Content = "🙈";
            }
            else
            {
                txtConfirmPassword.Password = txtConfirmPasswordVisible.Text;
                txtConfirmPassword.Visibility = Visibility.Visible;
                txtConfirmPasswordVisible.Visibility = Visibility.Collapsed;
                btnToggleConfirmPassword.Content = "👁";
            }
        }

        private void TxtNewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewPassword.Password))
                txtNewPasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void TxtNewPasswordVisible_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewPasswordVisible.Text))
                txtNewPasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void TxtConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtConfirmPassword.Password))
                txtConfirmPasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void TxtConfirmPasswordVisible_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtConfirmPasswordVisible.Text))
                txtConfirmPasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            // Get the actual password value from whichever control is visible
            string newPassword = isNewPasswordVisible ? txtNewPasswordVisible.Text : txtNewPassword.Password;
            string confirmPassword = isConfirmPasswordVisible ? txtConfirmPasswordVisible.Text : txtConfirmPassword.Password;

            // Placeholder: Logic to update password would go here
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Mật khẩu không khớp, vui lòng kiểm tra lại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            
            // Navigate back to Login
            DangNhap loginWindow = new DangNhap();
            loginWindow.Show();
            this.Close();
        }
    }
}
