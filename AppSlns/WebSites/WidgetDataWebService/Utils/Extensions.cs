using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WidgetDataWebService.Utils
{
    public static class Extensions
    {
        #region Encrypt and Decrypt Query String

        /// <summary>
        /// This is an extension method for EncryptedQueryString which returns the encrypted string for string type dictionary.
        /// </summary>
        /// <param name="sourceDictionary">Source dictionary value.</param>
        /// <returns></returns>
        public static String ToEncryptedQueryString(this Dictionary<String, String> sourceDictionary)
        {
            EncryptedQueryString encryptedQueryString = new EncryptedQueryString(sourceDictionary);
            return encryptedQueryString.ToString();
        }

        #endregion
    }

    public class EncryptedQueryString : Dictionary<String, String>
    {
        // Change the following keys to ensure uniqueness
        // Must be 8 bytes
        protected Byte[] _keyBytes = { 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18 };

        // Must be at least 8 characters
        protected String _keyString = "ABC12345";// Name for checksum value (unlikely to be used as arguments by user)

        protected String _checksumKey = "__$$";

        public EncryptedQueryString(IDictionary<String, String> dictionary)
            : base(dictionary)
        {
            // To initialize the base class constructor.
        }

        /// <summary>
        /// Returns an encrypted String that contains the current dictionary
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            // Build query String from current contents
            StringBuilder content = new StringBuilder();

            foreach (String key in base.Keys)
            {
                if (content.Length > WidgetDataConsts.NONE)
                {
                    content.Append('&');
                }

                content.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key),
                  HttpUtility.UrlEncode(base[key]));
            }

            // Add checksum
            if (content.Length > WidgetDataConsts.NONE)
            {
                content.Append('&');
            }

            content.AppendFormat("{0}={1}", _checksumKey, ComputeChecksum());
            return Encrypt(content.ToString());
        }

        /// <summary>
        /// Returns a simple checksum for all keys and values in the collection
        /// </summary>
        /// <returns></returns>
        protected String ComputeChecksum()
        {
            Int32 checksum = WidgetDataConsts.NONE;

            foreach (KeyValuePair<String, String> pair in this)
            {
                checksum += pair.Key.Sum(c => c - '0');
                checksum += pair.Value.Sum(c => c - '0');
            }

            return checksum.ToString("X");
        }

        /// <summary>
        /// Encrypts the given text
        /// </summary>
        /// <param name="text">Text to be encrypted</param>
        /// <returns></returns>
        protected String Encrypt(String text)
        {
            try
            {
                Byte[] keyData = Encoding.UTF8.GetBytes(_keyString.Substring(WidgetDataConsts.NONE, WidgetDataConsts.EIGHT));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] textData = Encoding.UTF8.GetBytes(text);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms,
                  des.CreateEncryptor(keyData, _keyBytes), CryptoStreamMode.Write);
                cs.Write(textData, WidgetDataConsts.NONE, textData.Length);
                cs.FlushFinalBlock();

                return GetString(ms.ToArray());
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Converts a Byte array to a String of hex characters
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected String GetString(Byte[] data)
        {
            StringBuilder results = new StringBuilder();

            foreach (Byte b in data)
            {
                results.Append(b.ToString("X2"));
            }

            return results.ToString();
        }
    }

}