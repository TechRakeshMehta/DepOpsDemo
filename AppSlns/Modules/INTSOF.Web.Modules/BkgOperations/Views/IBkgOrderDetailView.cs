using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IBkgOrderDetailView
    {
        Int32 SelectedTenantId { get;}
        Int32 CurrentLoggedInUserId { get; }
        Int32 OrderID { get; }
        Int32 OrderPkgSvcGroupID { get; set; }
        String ParentScreenName { get; set; }
        String OrderNumber { get; }
        Int32 SupplementAutomationStatusID { get; set; }
        #region UAT-2117:"Continue" button behavior
        Int32 MenuID { get; set; }
        #endregion
    }
}
