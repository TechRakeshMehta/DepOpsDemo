using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.SearchUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.SearchUI.Views
{
    public interface ISupportPortalNotesView
    {
        Int32 CurrentLoggedInUserId { get;  }
        Int32 ApplicantOrganizationUserID { get; set; }
        Int32 SelectedTenantID { get; set; }
        List<BkgOrderQueueNotesContract> lstBkgOrderNotes { get; set; }
        String CurrentLoggedInUserName { get; set; }
       
        Entity.OrganizationUser OrganizationUser { get; set; }
        List<Int32> lstOrderIds { get; set; }
        List<String> lstOrderNumber { get; set; }
        List<SupportPortalOrderDetailContract> lstOrder { get; set; }

    }
}
