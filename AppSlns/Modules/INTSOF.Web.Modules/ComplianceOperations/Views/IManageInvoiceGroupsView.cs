#region Namespaces

#region SystemDefined

using System.Collections.Generic;
using System.Linq;
using System;

#endregion

#region UserDefined

using Entity;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManageInvoiceGroupsView
    {
        #region Properties

        #region Public Properties

        Int32 TenantId { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        IManageInvoiceGroupsView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        String SuccessMessage { get; set; }
        List<InvoiceGroupContract> lstInvoiceGroups { get; set; }
        List<Tenant> lstTenants { get; set; }
        List<Entity.ClientEntity.DeptProgramMapping> lstNodes { get; set; }
        List<lkpReportColumn> lstReportColumns { get; set; }
        Int32 InvoiceGroupID { get; set; }
        List<InvoiceGroupNodes> LstInvoiceGroupNodes { get; set; }

        #endregion

        #endregion
    }
}




