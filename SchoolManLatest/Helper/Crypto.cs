using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TrackTap.Helper
{
    public class Crypto
    {
        //Algorithm types
        public enum Algorithm : int { SHA1 = 0, SHA256 = 1, SHA384 = 2, MD5 = 9, SHA512 = 12 }

        //Encoding Types
        public enum EncodingType : int { HEX = 0, BASE_64 = 1 }

        //Error messages
        private const string ERR_NO_CONTENT = "No content was provided";
        private const string ERR_INVALID_PROVIDER = "An invalid cryptographic provider was specified for this method";

        /// <summary>
        /// Crypto constructor
        /// </summary>
        public Crypto() { }

        /// <summary>
        /// Generates hash string for given value and algorith with encoding type
        /// Supported Algorithm: SHA1, SHA256, SHA384, MD5 and SHA512
        /// Supported Encoding Types: HEX and BASE_64
        /// </summary>
        /// <param name="Content">Content for hash</param>
        /// <param name="algorithm">Supported Algorithm: SHA1, SHA256, SHA384, MD5 and SHA512</param>
        /// <param name="et">Supported Encoding Types: HEX and BASE_64</param>
        /// <returns>Generated hash</returns>
        public static string GenerateHashString(string Content, Algorithm algorithm, EncodingType et)
        {
            string _content;
            if (Content == null || Content.Equals(string.Empty))
            {
                throw new CryptographicException(ERR_NO_CONTENT);
            }

            HashAlgorithm hashAlgorithm = null;

            switch (algorithm)
            {
                case Algorithm.SHA1:
                    hashAlgorithm = new SHA1CryptoServiceProvider();
                    break;
                case Algorithm.SHA256:
                    hashAlgorithm = new SHA256Managed();
                    break;
                case Algorithm.SHA384:
                    hashAlgorithm = new SHA384Managed();
                    break;
                case Algorithm.SHA512:
                    hashAlgorithm = new SHA512Managed();
                    break;
                case Algorithm.MD5:
                    hashAlgorithm = new MD5CryptoServiceProvider();
                    break;
                default:
                    throw new CryptographicException(ERR_INVALID_PROVIDER);
            }

            try
            {
                byte[] hash = ComputeHash(hashAlgorithm, Content);
                if (et == EncodingType.HEX)
                {
                    _content = BytesToHex(hash);
                }
                else
                {
                    _content = System.Convert.ToBase64String(hash);
                }
                hashAlgorithm.Clear();
                return _content;
            }
            catch (CryptographicException cge)
            {
                throw cge;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                hashAlgorithm.Clear();
            }
        }

        #region "Utility Functions"
        /// <summary>
        /// Compute Hash for given algorithm and content
        /// </summary>
        /// <param name="Provider">Hash algorithm</param>
        /// <param name="plainText">Content</param>
        /// <returns>Computed hash</returns>
        private static byte[] ComputeHash(HashAlgorithm Provider, string plainText)
        {
            byte[] hash = Provider.ComputeHash(System.Text.Encoding.UTF8.GetBytes(plainText));
            Provider.Clear();
            return hash;
        }

        /// <summary>
        /// Converts a byte array to a hex-encoded string
        /// </summary>
        /// <param name="bytes">Array of bytes</param>
        /// <returns>Converted hex-encoded string</returns>
        private static string BytesToHex(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder();
            for (int n = 0; n <= bytes.Length - 1; n++)
            {
                hex.AppendFormat("{0:X2}", bytes[n]);
            }
            return hex.ToString();
        }

        /// <summary>
        /// Converts a hex-encoded string to bytes array
        /// </summary>
        /// <param name="hexString">Hex-encoded string</param>
        /// <returns>Array of bytes</returns>
        private static byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] HexAsBytes = new byte[hexString.Length / 2];
            for (int index = 0; index < HexAsBytes.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return HexAsBytes;
        }
        #endregion
    }
}