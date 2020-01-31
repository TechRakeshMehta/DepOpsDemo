using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IComplianceSearchNotesView
    {
         Int32 PackageSubscriptionID { get; set; }
         Int32 CompliancePackageID { get; set; }
         Int32 OrderID { get; set; }
         String Notes { get; set; }
         Int32 CurrentLoggedInUserOrgId { get;}
         Int32 OrganizationUserId { get; set; }
         Int32 SelectedTenantId { get; set; }
    }
}
