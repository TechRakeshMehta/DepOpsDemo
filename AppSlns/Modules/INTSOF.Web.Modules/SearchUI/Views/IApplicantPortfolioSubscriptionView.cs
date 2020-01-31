#region Namespaces

#region SystemDefined

using System.Collections.Generic;
using System.Linq;
using System;

#endregion

#region UserDefined

using Entity.ClientEntity;

#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public interface IApplicantPortfolioSubscriptionView
    {
        IApplicantPortfolioSubscriptionView CurrentViewContext { get; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 OrganizationUserId { get; }
        Int32 TenantId { get; }
        List<GetPortfolioSubscriptionTree> TreeListDetail { get; set; }
    }
}




