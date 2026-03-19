using System.Windows.Controls;
using UI_Dat_Ve_May_Bay.ViewModels;

namespace UI_Dat_Ve_May_Bay.Views
{
    public partial class AuthView : UserControl
    {
        public AuthView()
        {
            InitializeComponent();
        }

        private AuthViewModel? VM => DataContext as AuthViewModel;

        private void LoginPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (VM != null) VM.LoginMatKhau = ((PasswordBox)sender).Password;
        }

        private void RegPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (VM != null) VM.RegMatKhau = ((PasswordBox)sender).Password;
        }

        private void RegConfirmPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (VM != null) VM.RegXacNhanMatKhau = ((PasswordBox)sender).Password;
        }

        private void FpNewPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (VM != null) VM.FpNewPassword = ((PasswordBox)sender).Password;
        }

        private void FpConfirmPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (VM != null) VM.FpConfirmPassword = ((PasswordBox)sender).Password;
        }
    }
}
