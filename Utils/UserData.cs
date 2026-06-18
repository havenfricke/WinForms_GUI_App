using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace WinForms_GUI_App.Utils
{
    internal class UserData
    {
        
        private static readonly string DirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "user_data");
        private static readonly string FilePath = Path.Combine(DirectoryPath, "user_data.json");

        private static readonly HttpClient httpClient = new HttpClient();

        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public static async Task<string> InitUserDataAsync()
        {
            Dictionary<string, string> data;

            if (Directory.Exists(DirectoryPath) && File.Exists(FilePath))
            {
                string json = await File.ReadAllTextAsync(FilePath, Encoding.UTF8);
                data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                if (data != null && data.TryGetValue("APP_ID", out string appId))
                {
                    Debug.WriteLine($"[APPLICATION ID]: {appId}");
                }
            }
            else
            {
                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }

                string ipRes = await GetPublicIpStandardAsync();

                // Construct the dictionary inline
                data = new Dictionary<string, string>
                {
                    { "APP_ID", GenerateSecureAppId() },
                    { "CURRENT_IP", ipRes }
                };
            }

            string outputJson = JsonSerializer.Serialize(data, jsonOptions);
            await File.WriteAllTextAsync(FilePath, outputJson, Encoding.UTF8);

            return DirectoryPath;
        }

        public static async Task<string> GetPublicIpStandardAsync()
        {
            try
            {
                return await httpClient.GetStringAsync("https://ident.me");
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Network Error: {ex.Message}");
                return "127.0.0.1";
            }
        }

        // ----- Helper functions -----

        private static Dictionary<string, string> ReadUserData()
        {
            if (Directory.Exists(DirectoryPath) && File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath, Encoding.UTF8);
                return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }
            return null;
        }

        public static string GetAppId()
        {
            var data = ReadUserData();
            return data != null && data.TryGetValue("APP_ID", out string appId) ? appId : null;
        }

        public static string GetAppIp()
        {
            var data = ReadUserData();
            return data != null && data.TryGetValue("CURRENT_IP", out string currentIp) ? currentIp : null;
        }

        public static async Task RefreshIpAsync()
        {
            if (Directory.Exists(DirectoryPath) && File.Exists(FilePath))
            {
                var data = ReadUserData();
                if (data != null)
                {
                    string currentIp = await GetPublicIpStandardAsync();

                    data.TryGetValue("CURRENT_IP", out string existingIp);

                    if (existingIp != currentIp)
                    {
                        data["CURRENT_IP"] = currentIp;

                        string outputJson = JsonSerializer.Serialize(data, jsonOptions);
                        await File.WriteAllTextAsync(FilePath, outputJson, Encoding.UTF8);
                    }
                }
            }
        }

        // --- Cryptography Helpers ---

        private static string GenerateSecureAppId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string[] blocks = new string[10];

            for (int i = 0; i < 10; i++)
            {
                blocks[i] = GenerateSecureString(chars, 5);
            }

            return string.Join("-", blocks);
        }

        private static string GenerateSecureString(string allowedChars, int length)
        {
            var stringBuilder = new StringBuilder(length);
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] uintBuffer = new byte[4];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    stringBuilder.Append(allowedChars[(int)(num % (uint)allowedChars.Length)]);
                }
            }
            return stringBuilder.ToString();
        }
    }
}