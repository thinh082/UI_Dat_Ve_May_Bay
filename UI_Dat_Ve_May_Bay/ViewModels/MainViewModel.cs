using System.Windows;
using UI_Dat_Ve_May_Bay.Api;
using UI_Dat_Ve_May_Bay.Core;
using UI_Dat_Ve_May_Bay.Services;

namespace UI_Dat_Ve_May_Bay.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public MainViewModel()
            : this(new ApiClient("https://localhost:7242"), new TokenStore())
        { }

        private readonly ApiClient _apiClient;
        private readonly TokenStore _tokenStore;

        private HomeViewModel? _homeVM;
        private NotificationViewModel? _notiVM;
        private VoucherViewModel? _voucherVM;
        private AuthViewModel? _authVM;
        private ProfileViewModel? _profileVM;

        private FlightViewModel? _flightVM;
        private BookingViewModel? _bookingVM;

        private object _currentViewModel = new object();
        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (SetProperty(ref _currentViewModel, value))
                    OnPropertyChanged(nameof(IsAppScreen));
            }
        }

        private string _currentTabName = "Home";
        public string CurrentTabName { get => _currentTabName; set => SetProperty(ref _currentTabName, value); }

        public bool IsAppScreen => _authVM != null && CurrentViewModel != _authVM;

        public RelayCommand GoHomeCommand { get; }
        public RelayCommand GoNotificationCommand { get; }
        public RelayCommand GoVoucherCommand { get; }
        public RelayCommand GoFlightCommand { get; }
        public RelayCommand GoProfileCommand { get; }

        public RelayCommand LogoutCommand { get; }
        public RelayCommand ClearTokenCommand { get; }
        public RelayCommand ShowTokenPathCommand { get; }

        public MainViewModel(ApiClient apiClient, TokenStore tokenStore)
        {
            _apiClient = apiClient;
            _tokenStore = tokenStore;

            CurrentTabName = "Đăng nhập";
            CurrentViewModel = new object();

            try
            {
                ReloadTokenToApiClient();

                // ✅ FIX: HomeViewModel cần ApiClient để gọi Huỷ vé / Check-in
                _homeVM = new HomeViewModel(_apiClient);

                // IMPORTANT: AuthViewModel phải nhận đúng ApiClient đang dùng trong app.
                // Nếu dùng nhầm overload không truyền ApiClient, các Command sẽ không được init => bấm đăng nhập không chạy.
                _authVM = new AuthViewModel(new AuthApi(_apiClient), _tokenStore, _apiClient, onLoginSuccess: NavigateHome);

                if (!string.IsNullOrWhiteSpace(_apiClient.Token))
                {
                    CurrentTabName = "Home";
                    CurrentViewModel = _homeVM;
                }
                else
                {
                    CurrentTabName = "Đăng nhập";
                    CurrentViewModel = _authVM;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khởi tạo UI: {ex.Message}\n\nHệ thống sẽ chuyển về màn hình đăng nhập.",
                    "Init error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                _authVM ??= new AuthViewModel(new AuthApi(_apiClient), _tokenStore, _apiClient, onLoginSuccess: NavigateHome);
                CurrentTabName = "Đăng nhập";
                CurrentViewModel = _authVM;
            }

            GoHomeCommand = new RelayCommand(NavigateHome);

            GoNotificationCommand = new RelayCommand(() =>
            {
                if (!EnsureLoggedIn()) return;

                _notiVM ??= new NotificationViewModel(new NotificationApi(_apiClient));
                CurrentTabName = "Thông báo";
                CurrentViewModel = _notiVM;
            });

            GoVoucherCommand = new RelayCommand(() =>
            {
                if (!EnsureLoggedIn()) return;
                CurrentTabName = "Voucher";

                _voucherVM ??= new VoucherViewModel(new VoucherApi(_apiClient), autoLoad: true);
                CurrentViewModel = _voucherVM;
            });

            GoFlightCommand = new RelayCommand(() =>
            {
                if (!EnsureLoggedIn()) return;

                _flightVM ??= new FlightViewModel(_apiClient, GoToBooking);
                CurrentTabName = "Chuyến bay";
                CurrentViewModel = _flightVM;
            });

            GoProfileCommand = new RelayCommand(() =>
            {
                if (!EnsureLoggedIn()) return;

                _profileVM ??= new ProfileViewModel(_apiClient);
                CurrentTabName = "Hồ sơ";
                CurrentViewModel = _profileVM;
            });

            LogoutCommand = new RelayCommand(() =>
            {
                _tokenStore.Clear();
                _apiClient.Token = null;
                _notiVM = null;
                _voucherVM = null;
                _flightVM = null;
                _bookingVM = null;
                _profileVM = null;

                CurrentTabName = "Đăng nhập";
                CurrentViewModel = _authVM ?? new object();

                MessageBox.Show("Đã đăng xuất thành công.", "Đăng xuất",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            });

            ClearTokenCommand = new RelayCommand(() =>
            {
                _tokenStore.Clear();
                _apiClient.Token = null;
                _notiVM = null;
                _voucherVM = null;
                _flightVM = null;
                _bookingVM = null;
                _profileVM = null;

                MessageBox.Show("Đã xóa token. App sẽ quay về đăng nhập.", "Token cleared",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                CurrentTabName = "Đăng nhập";
                CurrentViewModel = _authVM ?? new object();
            });

            ShowTokenPathCommand = new RelayCommand(() =>
            {
                MessageBox.Show(_tokenStore.GetFilePath(), "Token file path",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private void NavigateFlight()
        {
            if (!EnsureLoggedIn()) return;

            _flightVM ??= new FlightViewModel(_apiClient, GoToBooking);
            CurrentTabName = "Chuyến bay";
            CurrentViewModel = _flightVM;
        }

        private void GoToBooking(FlightViewModel.LichBayItemVm? selected)
        {
            if (selected is null) return;

            // ✅ REUSE: Nếu đang có booking cho đúng chuyến bay này thì không tạo mới
            if (_bookingVM != null && _bookingVM.SelectedSchedule.Id == selected.Id)
            {
                CurrentTabName = "Đặt vé";
                CurrentViewModel = _bookingVM;
                return;
            }

            _bookingVM = new BookingViewModel(
                _apiClient,
                selected,
                () => {
                    _bookingVM = null; // Xoá session khi nhấn Quay lại
                    NavigateFlight();
                }
            );

            CurrentTabName = "Đặt vé";
            CurrentViewModel = _bookingVM;

            if (_bookingVM.RefreshSeatsCommand.CanExecute(null))
                _bookingVM.RefreshSeatsCommand.Execute(null);
        }

        private void ReloadTokenToApiClient()
        {
            var token = _tokenStore.Load();
            _apiClient.Token = string.IsNullOrWhiteSpace(token) ? null : token.Trim();
        }

        private void NavigateHome()
        {
            // Reset all sub-VMs before re-init for new session
            _flightVM = null;
            _bookingVM = null;
            _notiVM = null;
            _voucherVM = null;
            _profileVM = null;

            ReloadTokenToApiClient();

            if (!EnsureLoggedIn(showMessage: false))
            {
                CurrentTabName = "Đăng nhập";
                CurrentViewModel = _authVM ?? new object();
                return;
            }

            CurrentTabName = "Home";
            CurrentViewModel = _homeVM ?? new object();
        }

        private bool EnsureLoggedIn(bool showMessage = true)
        {
            // phòng trường hợp token đã được lưu ở TokenStore nhưng _apiClient.Token chưa được set
            if (string.IsNullOrWhiteSpace(_apiClient.Token))
                ReloadTokenToApiClient();

            if (!string.IsNullOrWhiteSpace(_apiClient.Token))
                return true;

            if (showMessage)
            {
                MessageBox.Show(
                    "Bạn chưa đăng nhập (chưa có token). Hệ thống sẽ chuyển về đăng nhập.",
                    "Thiếu token",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }

            CurrentTabName = "Đăng nhập";
            CurrentViewModel = _authVM ?? new object();
            return false;
        }
    }
}