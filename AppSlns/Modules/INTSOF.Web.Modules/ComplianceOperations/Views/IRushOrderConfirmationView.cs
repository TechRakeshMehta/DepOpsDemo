using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IRushOrderConfirmationView
    {
        Int32 TenantId { get; set; }
        Int32 OrderId { get; }
        IRushOrderConfirmationView CurrentViewContext { get; }
        Int32 CurrentLoggedInUserId { get; }
        DeptProgramPackageSubscription SelectedPackageDetails { get; set; }
        Entity.OrganizationUser OrganizationUser { get; set; }
        Int32 DeptProgramPackageSubscriptionId { get; set; }
        Int32 SubscriptionId { get; }
        String ClientMachineIP { get; }

        /// <summary>
        /// Get and set next page path.
        /// </summary>
        String NextPagePath
        {
            get;
            set;
        }

        Int32 OrderPaymentTypeId
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




