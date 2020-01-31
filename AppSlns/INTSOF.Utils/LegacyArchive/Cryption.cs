using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.Utils.LegacyArchive
{

    // 
    /// <summary>
    /// dont want to confuse anyone by modifying the Cryptography class
    /// </summary>
    public class Cryption : EncryptedQueryString
    {
        public new string Encrypt(string text)
        {

            return base.Encrypt(text);
        }

        public new string Decrypt(string text)
        {

            return base.Decrypt(text);
        }
    }
}
