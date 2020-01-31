using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace INTSOF.RuleEngine.Utility
{
    internal class Helper
    {
        public static string ToDescriptionString(Enum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static T FromXml<T>(String xml)
        {
            T returnedXmlClass = default(T);
            UTF8Encoding utf = new UTF8Encoding();
            using (MemoryStream memoryStream = new MemoryStream(utf.GetBytes(xml)))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                using (StreamReader xmlStreamReader = new StreamReader(memoryStream, Encoding.UTF8))
                {
                    returnedXmlClass= (T)xmlSerializer.Deserialize(xmlStreamReader);
                }
            }
            return returnedXmlClass;
        }

        public static string ToXml<T>(T obj)
        {
            MemoryStream stream = null;
            TextWriter writer = null;
            try
            {

                stream = new MemoryStream(); // read xml in memory
                writer = new StreamWriter(stream, Encoding.Unicode);
                // get serialise object
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, obj); // read object
                int count = (int)stream.Length; // saves object in memory stream
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                // copy stream contents in byte array
                stream.Read(arr, 0, count);
                UnicodeEncoding utf = new UnicodeEncoding(); // convert byte array to string
                return utf.GetString(arr).Trim();
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (stream != null) stream.Close();
                if (writer != null) writer.Close();
            }
        }

    }
}
