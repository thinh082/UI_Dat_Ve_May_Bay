using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using UI_Dat_Ve_May_Bay.Api;
using UI_Dat_Ve_May_Bay.Core;
using UI_Dat_Ve_May_Bay.Models.Vouchers;

namespace UI_Dat_Ve_May_Bay.ViewModels
{
    public class VoucherViewModel : ObservableObject
    {
        private readonly VoucherApi _voucherApi;

        public ObservableCollection<VoucherDto> Vouchers { get; } = new();

        private VoucherDto? _selectedVoucher;
        public VoucherDto? SelectedVoucher
        {
            get => _selectedVoucher;
            set => SetProperty(ref _selectedVoucher, value);
        }

        private string _searchCode = "";
        public string SearchCode
        {
            get => _searchCode;
            set => SetProperty(ref _searchCode, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private string _status = "";
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public AsyncRelayCommand LoadMyVouchersCommand { get; }
        public AsyncRelayCommand LoadAllVouchersCommand { get; }   // ✅ phải init
        public AsyncRelayCommand SearchCommand { get; }
        public AsyncRelayCommand ApplySelectedCommand { get; }
        public AsyncRelayCommand LoadDetailsCommand { get; }

        public VoucherViewModel(VoucherApi voucherApi, bool autoLoad = false)
        {
            _voucherApi = voucherApi;

            LoadMyVouchersCommand = new AsyncRelayCommand(LoadMyVouchersAsync);
            LoadAllVouchersCommand = new AsyncRelayCommand(LoadAllVouchersAsync); // ✅ FIX: gán command
            SearchCommand = new AsyncRelayCommand(SearchAsync);
            ApplySelectedCommand = new AsyncRelayCommand(ApplySelectedAsync);
            LoadDetailsCommand = new AsyncRelayCommand(LoadDetailsAsync);

            // ✅ chỉ auto-load khi thật sự điều hướng vào tab Voucher
            if (autoLoad)
                _ = LoadMyVouchersAsync();
        }

        private async Task LoadMyVouchersAsync()
        {
            await RunSafe(async () =>
            {
                Status = "Đang tải voucher của tôi...";
                Vouchers.Clear();

                var list = await _voucherApi.LayToanBoPhieuGiamGiaAsync();
                foreach (var v in list) Vouchers.Add(v);

                Status = Vouchers.Count == 0 ? "Không có voucher." : $"Đã tải {Vouchers.Count} voucher.";
            });
        }

        private async Task LoadAllVouchersAsync()
        {
            await RunSafe(async () =>
            {
                Status = "Đang tải tất cả voucher...";
                Vouchers.Clear();

                var list = await _voucherApi.GetDanhSachPhieuGiamGiaAsync(); // ✅ API ALL
                foreach (var v in list) Vouchers.Add(v);

                Status = Vouchers.Count == 0 ? "Không có voucher." : $"Đã tải {Vouchers.Count} voucher.";
            });
        }

        private async Task SearchAsync()
        {
            await RunSafe(async () =>
            {
                if (string.IsNullOrWhiteSpace(SearchCode))
                {
                    MessageBox.Show("Nhập mã giảm giá để tìm.", "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Status = "Đang tìm mã giảm giá...";
                Vouchers.Clear();

                var list = await _voucherApi.TimKiemMaGiamGiaAsync(SearchCode.Trim());
                foreach (var v in list) Vouchers.Add(v);

                Status = Vouchers.Count == 0 ? "Không tìm thấy voucher." : $"Tìm thấy {Vouchers.Count} voucher.";
            });
        }

        private async Task ApplySelectedAsync()
        {
            await RunSafe(async () =>
            {
                if (SelectedVoucher == null)
                {
                    MessageBox.Show("Chọn 1 voucher trước.", "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var id = SelectedVoucher.Id; // ✅ dùng Id theo JSON
                if (id <= 0)
                {
                    MessageBox.Show("Voucher không có Id hợp lệ.", "Lỗi dữ liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Status = "Đang áp dụng voucher...";
                var res = await _voucherApi.ApplyVoucherAsync(id);

                MessageBox.Show(res.Message ?? "Apply voucher OK", "Kết quả", MessageBoxButton.OK, MessageBoxImage.Information);

                // Sau khi apply, thường hợp lý là load lại voucher của tôi
                await LoadMyVouchersAsync();
            });
        }

        private async Task LoadDetailsAsync()
        {
            await RunSafe(async () =>
            {
                Status = "Đang tải chi tiết voucher...";
                Vouchers.Clear();

                var list = await _voucherApi.LayDanhSachChiTietPhieuGiamGiaAsync();
                foreach (var v in list) Vouchers.Add(v);

                Status = Vouchers.Count == 0 ? "Không có chi tiết voucher." : $"Đã tải {Vouchers.Count} dòng chi tiết.";
            });
        }

        private async Task RunSafe(Func<Task> action)
        {
            try
            {
                IsLoading = true;
                await action();
            }
            catch (Exception ex)
            {
                Status = "Lỗi.";
                MessageBox.Show(ex.Message, "Lỗi Voucher", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}