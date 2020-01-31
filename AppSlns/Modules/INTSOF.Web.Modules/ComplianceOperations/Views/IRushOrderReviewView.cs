using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IRushOrderReviewView
    {
        Int32 TenantId { get; set; }
        Int32 OrderId { get; set; }
        IRushOrderReviewView CurrentViewContext { get; }
        Int32 CurrentLoggedInUserId { get; }
        String PaymentModeCode { get; set; }
        Int32 DeptProgramPackageSubscriptionId { get; set; }
        Int32 SubscriptionId { get; set; }
        DeptProgramPackageSubscription SelectedPackageDetails { get; set; }
        List<lkpPaymentOption> lstPaymentOptions { get; set; }
        Entity.OrganizationUser OrganizationUser { get; set; }

        /// <summary>
        /// Get and set next page path.
        /// </summary>
        String NextPagePath
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the hierarhcy which was selected by applicant, durint actual order placement
        /// i.e. hierarchy by SelectedNodeId
        /// </summary>
        String SelectedNodeHierarchy
        {
            set;
        }
    }
}




