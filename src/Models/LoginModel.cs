using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using Login.WPF.Template.Helper;

namespace Login.WPF.Template.Models
{
    public interface ILoginService
    {
        Task<(ResponseType ResponseType, string Message)> LoginAsync(string username, string password);
        void SaveCredentials(string username, string password, bool rememberMe);
        (string Username, string Password, bool RememberMe) LoadCredentials();
    }

    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;
        private const string FilePath = "user_credentials.txt";

        public LoginService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<(ResponseType, string)> LoginAsync(string username, string password)
        {
            var loginUrl = "http://127.0.0.1:8000/login/"; // 替换为后端实际URL
            var loginData = new
            {
                username = username,
                password = password
            };

            // 将数据序列化为 JSON
            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            try
            {
                // 发送 POST 请求
                var response = await _httpClient.PostAsync(loginUrl, content);

                // 检查响应是否成功
                if (response.IsSuccessStatusCode)
                {
                    // 解析响应数据
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<LoginResponse>(responseString);

                    // 处理 access_token，后续操作可以存储到本地或内存中
                    var accessToken = responseData?.AccessToken;
                    // TODO: 将 access_token 存储或进一步处理

                    return (ResponseType.Successed, ""); // 登录成功
                }
                else
                {
                    // 登录失败，可能的原因是用户名或密码不正确
                    return (ResponseType.Failed, "");
                }
            }
            catch (Exception ex)
            {
                // 处理网络请求的异常，例如连接失败等
                Console.WriteLine($"登录请求失败: {ex.Message}");
                return (ResponseType.Error, ex.Message);
            }
        }

        // 保存账号和密码，并记录是否记住我
        public void SaveCredentials(string username, string password, bool rememberMe)
        {
            if (rememberMe)
            {
                var encryptedUsername = EncryptHelper.Encrypt(username);
                var encryptedPassword = EncryptHelper.Encrypt(password);
                var rememberMeString = EncryptHelper.BoolToString(rememberMe);

                File.WriteAllText(FilePath, $"{encryptedUsername}|{encryptedPassword}|{rememberMeString}");
            }
            else
            {
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
            }
        }

        // 加载保存的账号和密码，并返回记住我状态
        public (string, string, bool) LoadCredentials()
        {
            if (File.Exists(FilePath))
            {
                var credentials = File.ReadAllText(FilePath).Split('|');
                var username = EncryptHelper.Decrypt(credentials[0]);
                var password = EncryptHelper.Decrypt(credentials[1]);
                var rememberMe = EncryptHelper.StringToBool(credentials[2]);
                return (username, password, rememberMe);
            }
            return ("", "", false);
        }
    }

    // 用于解析后端返回的 JSON 响应
    public class LoginResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }

    public enum ResponseType
    {
        Successed,
        Failed,
        Error
    }
}
