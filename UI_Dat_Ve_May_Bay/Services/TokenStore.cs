using System;
using System.IO;
using System.Text.RegularExpressions;

namespace UI_Dat_Ve_May_Bay.Services
{
    public class TokenStore
    {
        private readonly string _filePath;

        public TokenStore(string appName = "UI_Dat_Ve_May_Bay")
        {
            var dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                appName
            );
            Directory.CreateDirectory(dir);
            _filePath = Path.Combine(dir, "jwt.txt");
        }

        public string? Load()
        {
            // ✅ Nếu chưa có file thì tạo file rỗng luôn để user paste vào
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "");
                return null;
            }

            var token = File.ReadAllText(_filePath);
            token = token.Replace("\uFEFF", "").Trim(); // bỏ BOM
            token = token.Trim('"');                   // bỏ " nếu lỡ dán
            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                token = token.Substring("Bearer ".Length).Trim();

            return string.IsNullOrWhiteSpace(token) ? null : token;
        }

        public void Save(string token)
        {
            File.WriteAllText(_filePath, token?.Trim() ?? "");
        }

        public void Clear()
        {
            if (File.Exists(_filePath)) File.Delete(_filePath);
        }

        public string GetFilePath() => _filePath;
    }
}
