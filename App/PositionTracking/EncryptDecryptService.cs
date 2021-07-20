using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Microsoft.Extensions.Logging;
using PositionTracking.Data;

namespace PositionTracking
{
    public class EncryptDecryptService
    {
        private readonly string _signupPassword;
        private readonly ILogger _logger;

        public EncryptDecryptService(IConfiguration config,ILogger<EncryptDecryptService> logger)
        {
            _signupPassword = config.GetValue<string>("Settings:GetSignupPassword");
            _logger = logger;

        }
#if DEBUG
        public EncryptDecryptService()
        {
            _signupPassword = "";
            
        }
#endif
        private static byte[] HashPassword(string password)
        {
            using (SHA256 mysHA256 = SHA256.Create())
               return mysHA256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public string EncryptString(string plainText,string salt)
        {
            try
            {
                byte[] iv = new byte[16];
                byte[] array;

                using(Aes aes = Aes.Create())
                {
                    aes.Key = HashPassword( _signupPassword + salt);
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using(MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream,encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            {
                                streamWriter.Write(plainText);
                            }
                            array = memoryStream.ToArray();
                        }
                    }
                }
                return Convert.ToBase64String(array);
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Encryption failed.", ex);
                throw;
            }
            
        }

        public string DecryptString(string cipherText,string salt)
        {
            try
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = HashPassword(_signupPassword + salt);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Decryption failed.", ex);
                throw;
            }
        }
    }
}
