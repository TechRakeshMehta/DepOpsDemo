using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
   public class ComplianceSearchNotesContract
    {
       public Int32 PackageSubscriptionID { get; set; }
       public Int32 CompliancePackageID { get; set; }
       public Int32 OrderID { get; set; }
       public String Notes { get; set; }
       public Int32 CurrentLoggedInUserOrgId { get; set; }
       public Int32 OrganizationUserId { get; set; }
       public Int32 TenantId { get; set; }
    }
}
