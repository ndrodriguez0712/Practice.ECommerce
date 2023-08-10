using System.Text;
using System.Security.Cryptography;
using Identity.Service.EventHandlers.Helpers.Interfaces;
using Microsoft.Extensions.Options;
using Identity.Service.Queries.DTOs;

namespace Identity.Service.EventHandlers.Helpers
{
    public class EncryptionManager : IEncryptionManager
    {
        #region Variables
        private readonly EncryptionOptionsDto _encryptionOptions;
        #endregion

        #region Constructor
        public EncryptionManager(IOptions<EncryptionOptionsDto> encryptionOptions)
        {
            _encryptionOptions = encryptionOptions.Value;
        }
        #endregion

        public string Hash(string password)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                SHA256 sha256 = SHA256.Create();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] stream = null;
                StringBuilder sb = new StringBuilder();
                stream = sha256.ComputeHash(encoding.GetBytes(password));
                for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                var hashed = sb.ToString();
                return hashed;
            }
        }

        public string Encrypt(string value, string key)
        {
            return Encrypt<AesManaged>(value, key);
        }

        public string Encrypt<T>(string value, string key)
                where T : SymmetricAlgorithm, new()
        {
            byte[] vectorBytes = Encoding.ASCII.GetBytes(_encryptionOptions.Vector);
            byte[] saltBytes = Encoding.ASCII.GetBytes(_encryptionOptions.Salt);
            byte[] valueBytes = Encoding.UTF8.GetBytes(value);

            byte[] encrypted;
            using (T cipher = new T())
            {
                PasswordDeriveBytes _passwordBytes =
                    new PasswordDeriveBytes(key, saltBytes, _encryptionOptions.Hash, _encryptionOptions.Iterations);
                byte[] keyBytes = _passwordBytes.GetBytes(_encryptionOptions.KeySize / 8);

                cipher.Mode = CipherMode.CBC;

                using (ICryptoTransform encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes))
                {
                    using (MemoryStream to = new MemoryStream())
                    {
                        using (CryptoStream writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(valueBytes, 0, valueBytes.Length);
                            writer.FlushFinalBlock();
                            encrypted = to.ToArray();
                        }
                    }
                }
                cipher.Clear();
            }
            return Convert.ToBase64String(encrypted);
        }

        public string Unscrypt(string value, string key)
        {
            return Unscrypt<AesManaged>(value, key);
        }

        public string Unscrypt<T>(string value, string key) where T : SymmetricAlgorithm, new()
        {
            byte[] vectorBytes = Encoding.ASCII.GetBytes(_encryptionOptions.Vector);
            byte[] saltBytes = Encoding.ASCII.GetBytes(_encryptionOptions.Salt);
            byte[] valueBytes = Convert.FromBase64String(value);

            byte[] decrypted;
            int decryptedByteCount = 0;

            using (T cipher = new T())
            {
                PasswordDeriveBytes _passwordBytes = new PasswordDeriveBytes(key, saltBytes, _encryptionOptions.Hash, _encryptionOptions.Iterations);
                byte[] keyBytes = _passwordBytes.GetBytes(_encryptionOptions.KeySize / 8);

                cipher.Mode = CipherMode.CBC;

                try
                {
                    using (ICryptoTransform decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
                    {
                        using (MemoryStream from = new MemoryStream(valueBytes))
                        {
                            using (CryptoStream reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read))
                            {
                                decrypted = new byte[valueBytes.Length];
                                decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }

                cipher.Clear();
            }
            return Encoding.UTF8.GetString(decrypted, 0, decryptedByteCount);
        }
    }
}
