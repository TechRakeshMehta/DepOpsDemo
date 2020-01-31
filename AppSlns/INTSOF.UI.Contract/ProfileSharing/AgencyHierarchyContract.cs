using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class AgencyHierarchyContract
    {
        public Int32 TenantID { get; set; }
        public String TenantName { get; set; }
        public String Hierarchies { get; set; }
        public String HierarchyIDs { get; set; }
        public Dictionary<Int32, String> DicHierarchy { get; set; }
        public Boolean IsTenantRemoved { get; set; }
        public Boolean IsTenantNewlyMapped { get; set; }
        public String HierarchyValidationMessage { get; set; }
        public Boolean IsStudent { get; set; }
        public Boolean IsAdmin { get; set; }
        //UAT-2639:
        public Int32 AgencyHierarchyID { get; set; }
        public Int32 AgencyHierarchyProfileSharePermissionID { get; set; }
    }
}

