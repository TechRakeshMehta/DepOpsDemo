using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalVendors.Utility
{
    public static class ConversionHelper
    {
        private static readonly Encoding encoding = new System.Text.ASCIIEncoding();

        public static byte[] StrToByteArray(string text)
        {
            if (text == null) { throw new ArgumentNullException("text"); }
            return encoding.GetBytes(text);
        }

        public static string ByteArrayToStr(byte[] bytes)
        {
            if (bytes == null) { throw new ArgumentNullException("bytes"); }

            return encoding.GetString(bytes);
        }

    }
}
