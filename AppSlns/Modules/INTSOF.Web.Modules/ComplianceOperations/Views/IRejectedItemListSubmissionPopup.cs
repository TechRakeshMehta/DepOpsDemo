using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IRejectedItemListSubmissionPopup
    {
        Int32 OrgUserId { get; set; }
        Int32 TenantId { get; set; }
        Int32 CurrenLoggedInUserId { get;  }
        List<RejectedItemListContract> lstRejectedItemListContract { get; set; }
    }
}
