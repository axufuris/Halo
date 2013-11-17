using System;
using System.Security.Cryptography;
using System.Text;

namespace Halo.Utilities
{
    public class Encryption
    {
        /// <summary>
        /// The Initialization Vector for the DES encryption routine
        /// </summary>
        /// <createdate>11-20-2012</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        private static readonly byte[] IV =
            new byte[8] { 240, 3, 45, 29, 0, 76, 173, 59 };

        /// <summary>
        /// Encrypts the specified String to encrypt.
        /// </summary>
        /// <param name="stringToEncrypt">The String to encrypt.</param>
        /// <param name="key">The encryption/decryption key.</param>
        /// <returns>
        /// Encrypted Strings
        /// </returns>
        /// <createdate>11-20-2012</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static string Encrypt(string stringToEncrypt, string key)
        {
            try
            {
                if (stringToEncrypt == null || stringToEncrypt.Length == 0)
                {
                    return string.Empty;
                }

                byte[] buffer = Encoding.ASCII.GetBytes(stringToEncrypt);

                TripleDESCryptoServiceProvider des =
                    new TripleDESCryptoServiceProvider();

                MD5CryptoServiceProvider md5 =
                    new MD5CryptoServiceProvider();

                des.Key = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));

                des.IV = IV;
                return Convert.ToBase64String(
                    des.CreateEncryptor().TransformFinalBlock(
                        buffer, 0, buffer.Length));
            }
            catch (Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
        }

        /// <summary>
        /// Decrypt the given string using the specified key.
        /// </summary>
        /// <param name="stringToDecrypt">The string to decrypt.</param>
        /// <param name="key">The decryption key.</param>
        /// <returns>
        /// The decrypted string.
        /// </returns>
        /// <createdate>11-20-2012</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static string Decrypt(string stringToDecrypt, string key)
        {
            try
            {
                if (stringToDecrypt == null || stringToDecrypt.Length == 0)
                {
                    return string.Empty;
                }

                byte[] buffer = Convert.FromBase64String(stringToDecrypt);

                TripleDESCryptoServiceProvider des =
                    new TripleDESCryptoServiceProvider();

                MD5CryptoServiceProvider md5 =
                    new MD5CryptoServiceProvider();

                des.Key = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));

                des.IV = IV;

                return Encoding.ASCII.GetString(
                    des.CreateDecryptor().TransformFinalBlock(
                    buffer, 0, buffer.Length));
            }
            catch (Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
        }
    }  /// End of Class
}
