using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceManagement
{
   public class NagEmailData
    {
       public String Userfullname { get; set; }
       public String NodeHierarchy { get; set; }
       public String Email { get; set; }
       public Int32 OrganizationUserID { get; set; }
       public String Packagename { get; set; }
       public String CategoryList { get; set; }
       public Int32 PackageSubscriptionId { get; set; }
       public Int32 MainNodeId { get; set; }
       public Int32 HierarchyNodeID { get; set; }
    }
}
