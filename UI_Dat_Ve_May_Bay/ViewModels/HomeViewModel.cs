using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UI_Dat_Ve_May_Bay.Api;
using UI_Dat_Ve_May_Bay.Core;

namespace UI_Dat_Ve_May_Bay.ViewModels
{
    public class HomeViewModel : ObservableObject
    {
        private readonly ApiClient _apiClient;

        // Vé (vòng đời)
        private const string EP_HUY_DAT_VE = "/api/ChuyenBay/HuyDatVe"; // POST (query: idDatVe, lyDoHuy)
        private const string EP_CHECKIN = "/api/ChuyenBay/CheckIn";    // POST (query: id)

        public string Greeting => "Hi, NGUYEN VAN TUNG";
        public string Question => "Dự định hôm nay của bạn là gì?";

        // Home chưa có BE: hardcode tạm để UI giống hình
        public string[] Categories { get; } = { "✈ Vé máy bay", "🏨 Khách sạn", "🚗 Xe" };

        // ===== Vé của tôi (Tra cứu / Huỷ / Check-in) =====
        private string _bookingIdText = "";
        public string BookingIdText { get => _bookingIdText; set => SetProperty(ref _bookingIdText, value); }

        private string _cancelReason = "";
        public string CancelReason { get => _cancelReason; set => SetProperty(ref _cancelReason, value); }

        private string _checkInIdText = "";
        public string CheckInIdText { get => _checkInIdText; set => SetProperty(ref _checkInIdText, value); }

        private bool _isBusy;
        public bool IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }

        private string _ticketInfo = "";
        public string TicketInfo { get => _ticketInfo; set => SetProperty(ref _ticketInfo, value); }

        private string _ticketError = "";
        public string TicketError { get => _ticketError; set => SetProperty(ref _ticketError, value); }

        public AsyncRelayCommand CancelBookingCommand { get; }
        public AsyncRelayCommand CheckInCommand { get; }

        public HomeViewModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
            CancelBookingCommand = new AsyncRelayCommand(CancelBookingAsync);
            CheckInCommand = new AsyncRelayCommand(CheckInAsync);
        }

        private async Task CancelBookingAsync()
        {
            try
            {
                TicketError = "";
                TicketInfo = "";
                IsBusy = true;

                if (!long.TryParse((BookingIdText ?? string.Empty).Trim(), out var idDatVe) || idDatVe <= 0)
                {
                    TicketInfo = "Nhập Mã đặt vé (idDatVe) hợp lệ trước.";
                    return;
                }

                var reason = (CancelReason ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(reason))
                    reason = "Huỷ theo yêu cầu";

                var url = $"{EP_HUY_DAT_VE}?idDatVe={idDatVe}&lyDoHuy={Uri.EscapeDataString(reason)}";
                var raw = await SendAndReadAsync(HttpMethod.Post, url, new { });

                var msg = TryExtractMessage(raw);
                TicketInfo = string.IsNullOrWhiteSpace(msg) ? "Đã gửi yêu cầu huỷ vé." : msg;
            }
            catch (Exception ex)
            {
                TicketError = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task CheckInAsync()
        {
            try
            {
                TicketError = "";
                TicketInfo = "";
                IsBusy = true;

                var id = (CheckInIdText ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(id))
                {
                    TicketInfo = "Nhập id để check-in trước.";
                    return;
                }

                var url = $"{EP_CHECKIN}?id={Uri.EscapeDataString(id)}";
                var raw = await SendAndReadAsync(HttpMethod.Post, url, new { });

                var msg = TryExtractMessage(raw);
                TicketInfo = string.IsNullOrWhiteSpace(msg) ? "Đã gửi yêu cầu check-in." : msg;
            }
            catch (Exception ex)
            {
                TicketError = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<string> SendAndReadAsync(HttpMethod method, string url, object? body)
        {
            var req = _apiClient.CreateRequest(method, url, true);

            if (body != null)
            {
                var json = JsonSerializer.Serialize(body);
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var resp = await _apiClient.Http.SendAsync(req);
            var content = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
            {
                var msg = TryExtractMessage(content);
                if (string.IsNullOrWhiteSpace(msg))
                    msg = $"{(int)resp.StatusCode} {resp.ReasonPhrase}";
                throw new Exception(msg);
            }

            return string.IsNullOrWhiteSpace(content) ? "{}" : content;
        }

        private static string? TryExtractMessage(string content)
        {
            try
            {
                using var doc = JsonDocument.Parse(content);
                if (doc.RootElement.ValueKind == JsonValueKind.Object &&
                    doc.RootElement.TryGetProperty("message", out var m) &&
                    m.ValueKind == JsonValueKind.String)
                    return m.GetString();
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}