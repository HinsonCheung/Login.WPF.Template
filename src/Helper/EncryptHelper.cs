using System.Security.Cryptography;
using System.Text;

namespace Login.WPF.Template.Helper
{
    public static class EncryptHelper
    {
        // 加密字符串（使用 DPAPI）
        public static string Encrypt(string plainText)
        {
            byte[] encryptedData = ProtectedData.Protect(Encoding.UTF8.GetBytes(plainText), null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        // 解密字符串（使用 DPAPI）
        public static string Decrypt(string encryptedText)
        {
            byte[] encryptedData = Convert.FromBase64String(encryptedText);
            byte[] decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decryptedData);
        }

        // 保存是否记住我
        public static string BoolToString(bool rememberMe)
        {
            return rememberMe ? "true" : "false";
        }

        // 读取记住我状态
        public static bool StringToBool(string value)
        {
            return value.Equals("true", StringComparison.OrdinalIgnoreCase);
        }
    }
}
