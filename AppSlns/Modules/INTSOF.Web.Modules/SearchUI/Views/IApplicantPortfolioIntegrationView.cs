using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.Search.Views
{
    public interface IApplicantPortfolioIntegrationView
    {
        IApplicantPortfolioIntegrationView CurrentViewContext { get; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 OrganizationUserId { get; }
    }
}




