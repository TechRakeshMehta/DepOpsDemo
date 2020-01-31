using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IReconciliationItemDataLoaderView
    {
        IReconciliationItemDataLoaderView CurrentViewContext { get; }

        List<ApplicantItemVerificationData> lst { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Boolean IsDefaultTenant { get; set; }
        List<ReconciliationDetailsDataContract> lstReconciliationDetailsData { get; set; }
        List<ApplicantDocuments> lstApplicantDocument { get; set; }
        Int32 CurrentTenantId_Global { get; set; }
        Int32 SelectedTenantId_Global { get; set; }
        //Int32 SelectedPackageSubscriptionId_Global { get; set; }
        Int32 SelectedCompliancePackageId_Global { get; set; }
        Int32 SelectedComplianceCategoryId_Global { get; set; }
        Int32 SelectedApplicantId_Global { get; set; }
        String CurrentLoggedInUserName_Global { get; set; }
        Int32 NextComplianceCategoryId_Global { get; set; }
        Int32 PreviousComplianceCategoryId_Global { get; set; }

        Boolean IncludeIncompleteItems_Global { get; set; }
        Boolean ShowOnlyRushOrders { get; set; }
        Int32 ItemDataId_Global { get; set; }
        Int32 AssignedToVerUser { get; set; }
        Int32 CurrentPackageSubscriptionID_Global
        {
            get;
            set;
        }

        Int32 PackageId_Global
        {
            get;
            set;
        }

        Int32 UserGroupId
        {
            get;
            set;
        }

        Int32 CategoryId_Global { get; set; }
        String viewType { get; set; }
        Boolean IsException { get; set; }

        Boolean IsUIValidationApplicable
        {
            get;
        }

        String LoggedInUserInitials_Global
        {
            get;
            set;
        }

        //UAT-3805
        Entity.ClientEntity.PackageSubscription PackageSubscriptionBeforeSaving { get; set; }

        #region UAT-3951:Addition of option to use preset ADB Admin rejection notes
        List<Entity.RejectionReason> ListRejectionReasons { get; set; }
        #endregion
    }
}
