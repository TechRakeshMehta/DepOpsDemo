using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace INTSOF.UI.Contract.SearchUI
{
    public class SharedUserSearchContract
    {
        public Int32 SharedUserID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String UserName { get; set; }
        public String EmailAddress { get; set; }
        public Int32 TotalCount { get; set; }
        public String CreateXml()
        {
            var serializer = new XmlSerializer(typeof(SharedUserSearchContract));
            var sb = new StringBuilder();

            SharedUserSearchContract xmlData = this;

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, xmlData);
            }
            return sb.ToString();
        }
    }
}
