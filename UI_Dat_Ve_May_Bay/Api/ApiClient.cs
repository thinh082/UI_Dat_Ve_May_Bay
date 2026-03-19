// Api/ApiClient.cs
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace UI_Dat_Ve_May_Bay.Api
{
    public class ApiClient
    {
        public HttpClient Http { get; }

        // ✅ cho phép set (đỡ lỗi "BaseUrl read only")
        public string BaseUrl { get; set; }

        public string? Token { get; set; }

        // ✅ ctor rỗng để XAML/VM new ApiClient() không lỗi
        public ApiClient() : this("http://localhost:5231") { }

        // ✅ ctor theo baseUrl (đỡ lỗi "required parameter baseUrl")
        public ApiClient(string baseUrl)
        {
            BaseUrl = baseUrl.TrimEnd('/');
            Http = new HttpClient();
        }

        // ✅ tương thích code cũ gọi ApplyBaseUrl()
        public void ApplyBaseUrl(string baseUrl)
        {
            BaseUrl = (baseUrl ?? "").TrimEnd('/');
        }

        // ✅ overload không tham số (giữ tương thích code cũ đã gọi ApplyBaseUrl();)
        public void ApplyBaseUrl()
        {
            BaseUrl = (BaseUrl ?? "").TrimEnd('/');
        }

        // ✅ thêm attachAuth để khỏi lỗi CS1739 (named parameter 'attachAuth')
        public HttpRequestMessage CreateRequest(HttpMethod method, string path, bool attachAuth = true)
        {
            if (string.IsNullOrWhiteSpace(path)) path = "/";
            if (!path.StartsWith("/")) path = "/" + path;

            var req = new HttpRequestMessage(method, BaseUrl + path);

            if (attachAuth && !string.IsNullOrWhiteSpace(Token))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            return req;
        }

        public HttpRequestMessage CreateJsonRequest<T>(HttpMethod method, string path, T body, bool attachAuth = true)
        {
            var req = CreateRequest(method, path, attachAuth);
            var json = JsonSerializer.Serialize(body);
            req.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return req;
        }
    }
}