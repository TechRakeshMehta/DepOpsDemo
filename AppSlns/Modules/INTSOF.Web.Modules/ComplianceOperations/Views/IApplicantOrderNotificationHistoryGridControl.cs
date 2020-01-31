using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IApplicantOrderNotificationHistoryGridControl
    {
        OrderNotificationHistoryContract OrderNotificationHistoryContract { get; set; }
        Int32 SelectedTenantID { get; }
        Int32 loggedInUserId { get; }
        Int32 OrganizationUserId { get; set;  }
    }
}
