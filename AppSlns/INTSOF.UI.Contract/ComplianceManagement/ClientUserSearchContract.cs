using System;
using System.Collections;
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
    public class ClientUserSearchContract
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
        public List<Int32> SelectedAgencyIDs { get; set; }

        public String SelectedAgecnyHierarchyIds { get; set; } //UAT-4257
        
        public List<Int32> lstSelectedAgencyHierarchyIDs { get; set; } //UAT-4257
        public String HierarchyNode { get; set; }
        public String HierarchyNodeLabel { get; set; }

        public Int32? AgencyUserID { get; set; }
        public Int32 TotalCount { get; set; }
        public String UserType { get; set; }
        public String TenantName { get; set; }
        public String AgencyName { get; set; }
        public String SearchType { get; set; }
        public String AssignedRoles { get; set; }
        public String UserID { get; set; }
        public DateTime? LastLoginDateTime { get; set; }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridCustomPagingArguments
        {
            get;
            set;
        }

        //public List<String> FilterColumns
        //{
        //    get;
        //    set;
        //}

        //public List<String> FilterOperators
        //{
        //    get;
        //    set;
        //}

        //public List<String> FilterTypes
        //{
        //    get;
        //    set;
        //}

        //public ArrayList FilterValues
        //{
        //    get;
        //    set;
        //}

        //public Boolean IsRestricted
        //{
        //    get;
        //    set;
        //}

        public String CreateXml()
        {
            var serializer = new XmlSerializer(typeof(ClientUserSearchContract));
            var sb = new StringBuilder();

            ClientUserSearchContract xmlData = this;
            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, xmlData);
            }
            return sb.ToString();
        }

    }
}
