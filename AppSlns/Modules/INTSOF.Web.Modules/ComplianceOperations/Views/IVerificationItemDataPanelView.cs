using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IVerificationItemDataPanelView
    {
        IVerificationItemDataPanelView CurrentViewContext { get; }

        List<ApplicantItemVerificationData> lst { get; set; }

        Int32 CurrentTenantId_Global { get; set; }
        Int32 SelectedTenantId_Global { get; set; }
        Int32 SelectedComplianceCategoryId_Global { get; set; }
        Int32 SelectedApplicantId_Global { get; set; }
        String CurrentLoggedInUserName_Global { get; set; }
        Int32 SelectedCompliancePackageId_Global { get; set; }
        Int32 NextComplianceCategoryId_Global { get; set; }
        Int32 PreviousComplianceCategoryId_Global { get; set; }
        List<ApplicantDocuments> lstApplicantDocument { get; set; }
        Boolean IncludeIncompleteItems_Global { get; set; }
        Int32 ItemDataId_Global { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 AssignedToVerUser { get; set; }
        WorkQueueType WorkQueue { get; set; }
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
        Int32 CategoryId_Global
        {
            get;
            set;
        }
        Boolean ShowOnlyRushOrders { get; set; }

        Boolean IsException { get; set; }

        /// <summary>
        /// set user group id for return back to queue.
        /// </summary>
        Int32 UserGroupId
        {
            get;
            set;
        }

        String LoggedInUserInitials_Global
        {
            get;
            set;
        }

        Boolean IsEscalationRecords { get; set; }

        List<RuleSet> RuleSetList
        {
            get;
            set;
        }
        Boolean IsAdminLoggedIn { get; set; }  //UAT-3566
    }
}




