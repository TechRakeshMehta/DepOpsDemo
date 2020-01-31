#region Namespaces

#region SystemDefined

using System.Collections.Generic;
using System;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public interface IApplicantPortfolioOrderHistoryView
    {
        #region Properties

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 OrganizationUserId
        {
            get;
        }

        Int32 TenantID
        {
            get;
        }

        List<vwOrderDetail> ListOrderDetail
        {
            get;
            set;
        }

        String SourceScreen
        {
            get;
            set;
        }

        String CancelledPackages
        {
            get;
            set;
        }

        Int32 OrderID
        {
            get;
            set;
        }

        #endregion
    }
}




