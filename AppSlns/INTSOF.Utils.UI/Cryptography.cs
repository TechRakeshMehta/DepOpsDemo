#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  Cryptography.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;


#endregion

#region Application Specific

using INTSOF.Utils;

#endregion

#endregion

namespace INTSOF.Utils.UI
{
   public class EncryptedQueryString : Dictionary<String, String>
   {
      // Change the following keys to ensure uniqueness
      // Must be 8 bytes
      protected Byte[] _keyBytes = { 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18 };

      // Must be at least 8 characters
      protected String _keyString = "ABC12345";

      // Name for checksum value (unlikely to be used as arguments by user)
      protected String _checksumKey = "__$$";

      /// <summary>
      /// Creates an empty dictionary
      /// </summary>
      public EncryptedQueryString()
      {
         // TODO: method needs to be implemented here.
      }

      public EncryptedQueryString(IDictionary<String, String> dictionary)
         : base(dictionary)
      {
         // To initialize the base class constructor.
      }

      /// <summary>
      /// Creates a dictionary from the given, encrypted String
      /// </summary>
      /// <param name="encryptedData"></param>
      public EncryptedQueryString(String encryptedData)
      {
         // Decrypt String
         String data = Decrypt(encryptedData);

         // Parse out key/value pairs and add to dictionary
         String checksum = null;
         String[] args = data.Split('&');

         foreach (String arg in args)
         {
            Int32 counter = arg.IndexOf('=');

            if (!counter.Equals(-AppConsts.ONE))
            {
               String key = arg.Substring(AppConsts.NONE, counter);
               String value = arg.Substring(counter + AppConsts.ONE);

               if (key.Equals(_checksumKey))
               {
                  checksum = value;
               }
               else
               {
                  base.Add(HttpUtility.UrlDecode(key), HttpUtility.UrlDecode(value));
               }
            }
         }

         // Clear contents if valid checksum not found
         if (checksum == null || !checksum.Equals(ComputeChecksum()))
         {
            base.Clear();
         }
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
            if (content.Length > AppConsts.NONE)
            {
               content.Append('&');
            }

            content.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key),
              HttpUtility.UrlEncode(base[key]));
         }

         // Add checksum
         if (content.Length > AppConsts.NONE)
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
         Int32 checksum = AppConsts.NONE;

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
            Byte[] keyData = Encoding.UTF8.GetBytes(_keyString.Substring(AppConsts.NONE, AppConsts.EIGHT));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] textData = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms,
              des.CreateEncryptor(keyData, _keyBytes), CryptoStreamMode.Write);
            cs.Write(textData, AppConsts.NONE, textData.Length);
            cs.FlushFinalBlock();

            return GetString(ms.ToArray());
         }
         catch (Exception)
         {
            return String.Empty;
         }
      }

      /// <summary>
      /// Decrypts the given encrypted text
      /// </summary>
      /// <param name="text">Text to be decrypted</param>
      /// <returns></returns>
      protected String Decrypt(String text)
      {
         try
         {
            Byte[] keyData = Encoding.UTF8.GetBytes(_keyString.Substring(AppConsts.NONE, AppConsts.EIGHT));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] textData = GetBytes(text);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms,
              des.CreateDecryptor(keyData, _keyBytes), CryptoStreamMode.Write);
            cs.Write(textData, AppConsts.NONE, textData.Length);
            cs.FlushFinalBlock();

            return Encoding.UTF8.GetString(ms.ToArray());
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

      /// <summary>
      /// Converts a String of hex characters to a Byte array
      /// </summary>
      /// <param name="data"></param>
      /// <returns></returns>
      protected Byte[] GetBytes(String data)
      {
         // GetString() encodes the hex-numbers with two digits
         Byte[] results = new Byte[data.Length / AppConsts.TWO];

         for (Int32 count = AppConsts.NONE; count < data.Length; count += AppConsts.TWO)
            results[count / AppConsts.TWO] = Convert.ToByte(data.Substring(count, AppConsts.TWO), AppConsts.SIXTEEN);

         return results;
      }
   }
}
