using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace INTSOF.UI.Contract.SysXSecurityModel
{
    [Serializable]
    public class ExternalLoginDataContract
    {
        public String TenantName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime DOB { get; set; }
        public String SSN { get; set; }
        public String UserName { get; set; }
        public String Email1 { get; set; }
        public String Email2 { get; set; }
        public String PhoneNumber { get; set; }
        public Int32 OrganizationID { get; set; }
        public Int32 OrganizationUserID { get; set; }
        public Int32 TenantID { get; set; }
        public Int32 IntegrationClientID { get; set; }

        //UAT-2792
        public Boolean IsFirstLogin { get; set; }

        public String CreateXml()
        {
            var serializer = new XmlSerializer(typeof(ExternalLoginDataContract));
            var sb = new StringBuilder();

            ExternalLoginDataContract xmlData = this;
            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, xmlData);
            }
            return sb.ToString();
        }
    }
    public class ExternalDataFromTokenDataContract
    {
        public String Name { get; set; }
        public String Value { get; set; }
    }
}
