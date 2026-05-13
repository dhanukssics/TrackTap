using System.Security.Cryptography;
using System.Text;

namespace CCA.Util
{
    public class CCACrypto
    {
        public string Encrypt(string plainText, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] iv = Encoding.UTF8.GetBytes(key.Substring(0, 16));

            using Aes aes = Aes.Create();

            aes.Key = keyBytes;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor =
                aes.CreateEncryptor(aes.Key, aes.IV);

            byte[] plainBytes =
                Encoding.UTF8.GetBytes(plainText);

            byte[] encrypted =
                encryptor.TransformFinalBlock(
                    plainBytes,
                    0,
                    plainBytes.Length);

            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string cipherText, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] iv = Encoding.UTF8.GetBytes(key.Substring(0, 16));

            using Aes aes = Aes.Create();

            aes.Key = keyBytes;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor =
                aes.CreateDecryptor(aes.Key, aes.IV);

            byte[] cipherBytes =
                Convert.FromBase64String(cipherText);

            byte[] decrypted =
                decryptor.TransformFinalBlock(
                    cipherBytes,
                    0,
                    cipherBytes.Length);

            return Encoding.UTF8.GetString(decrypted);
        }
    }
}