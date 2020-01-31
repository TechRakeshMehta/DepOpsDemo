using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IOrderNotificationHistoryView
    {
        List<LookupContract> lstNotificationHistory { get; set; }
        OrderNotificationHistoryContract OrderNotificationHistoryContract { get; set; }
        Int32 MasterOrderID { get; }
        Int32 SelectedTenantID { get; }
        Int32 loggedInUserId { get; }
        String OrderNumber { get; }
    }
}
