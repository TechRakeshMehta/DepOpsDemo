using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace INTSOF.Utils
{
    public class DataSerializer<T> where T:class
    {
        private DataContractSerializer xmlSerializer = null;

        public DataContractSerializer XmlSerializer
        {
            get 
            {
                if (xmlSerializer == null)
                {
                    xmlSerializer = new DataContractSerializer(typeof(T));
                }
                return xmlSerializer; 
            }            
        }

        public string Serialize(T obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                XmlSerializer.WriteObject(memoryStream, obj);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }            
        }
     
        public T Deserialize(String xml)
        {            
            using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
            {
                return (XmlSerializer.ReadObject(reader) as T);
            }
        }       

    }
}
