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
        public Nhapmatkhaumoi()
        {
            InitializeComponent();
            
            // Add event handlers for placeholder text
            txtNewPassword.GotFocus += TxtNewPassword_GotFocus;
            txtNewPassword.LostFocus += TxtNewPassword_LostFocus;
            txtConfirmPassword.GotFocus += TxtConfirmPassword_GotFocus;
            txtConfirmPassword.LostFocus += TxtConfirmPassword_LostFocus;
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

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder: Logic to update password would go here
            if (string.IsNullOrEmpty(txtNewPassword.Password) || string.IsNullOrEmpty(txtConfirmPassword.Password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txtNewPassword.Password != txtConfirmPassword.Password)
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
