using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Nhapotp.xaml
    /// </summary>
    public partial class Nhapotp : Window
    {
        public Nhapotp()
        {
            InitializeComponent();
            SetupOtpInputNavigation();
        }

        private void SetupOtpInputNavigation()
        {
            var textBoxes = new[] { txtOtp1, txtOtp2, txtOtp3, txtOtp4, txtOtp5, txtOtp6 };
            for (int i = 0; i < textBoxes.Length; i++)
            {
                int index = i;
                textBoxes[i].TextChanged += (s, e) =>
                {
                    if (textBoxes[index].Text.Length == 1 && index < textBoxes.Length - 1)
                    {
                        textBoxes[index + 1].Focus();
                    }
                };
                
                textBoxes[i].PreviewKeyDown += (s, e) =>
                {
                    if (e.Key == Key.Back && textBoxes[index].Text.Length == 0 && index > 0)
                    {
                        textBoxes[index - 1].Focus();
                    }
                };
            }
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            // Verify OTP Logic here
            string otp = $"{txtOtp1.Text}{txtOtp2.Text}{txtOtp3.Text}{txtOtp4.Text}{txtOtp5.Text}{txtOtp6.Text}";
            MessageBox.Show($"OTP Entered: {otp}", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnResend_Click(object sender, RoutedEventArgs e)
        {
            // Resend OTP Logic here
            MessageBox.Show("Mã OTP mới đã được gửi!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Otp_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            DangNhap login = new DangNhap();
            login.Show();
            this.Close();
        }
    }
}
