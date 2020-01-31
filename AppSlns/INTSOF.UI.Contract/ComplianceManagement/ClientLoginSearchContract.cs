using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    [Serializable]
    public class ClientLoginSearchContract
    {
        public List<Int32> SelectedTenantIDs { get; set; }
        public List<Entity.Tenant> lstTenant { get; set; }
        public Int32 CurrentLoggedInUserID { get; set; }
        public Int32 OrganizationUserID { get; set; }
        public String ClientFirstName { get; set; }
        public String ClientLastName { get; set; }
        public String ClientUserName { get; set; }
        public String EmailAddress { get; set; }
        public String Phone { get; set; }
        public Int32? ClientTenantID { get; set; }
        public Int32 TotalCount { get; set; }
        public String TenantName { get; set; }
        public String SearchType { get; set; }
        public String UserID { get; set; }
        public Boolean IsActive { get; set; }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridCustomPagingArguments
        {
            get;
            set;
        }
        public String CreateXml()
        {
            var serializer = new XmlSerializer(typeof(ClientLoginSearchContract));
            var sb = new StringBuilder();

            ClientLoginSearchContract xmlData = this;
            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, xmlData);
            }
            return sb.ToString();
        }

    }
}
