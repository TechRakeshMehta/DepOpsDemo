using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.SearchUI
{
    [Serializable]
    public class ManageMultipleSubscriptionContract
    {
         public Int32 PackageSubscriptionID { get; set; }
         public Int32 OrganizationUserID { get; set; }
         public String FirstName { get; set; }
         public String LastName { get; set; }
         public String UserName { get; set; }
         public String UserGroup { get; set; }
         public String PackageName { get; set; }
         public String InstitutionHierarchy { get; set; }
         public Int32 TotalCount { get; set; }
    }
}
