using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IItemDataExceptionModeView
    {
        IItemDataExceptionModeView CurrentViewContext { get; }
        List<ApplicantItemVerificationData> VerificationData { get; set; }
         
        Int32 ApplicantItemDataId { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        String StatusCode { get; set; }
        String Comments { get; set; }
        Int32 MappingId { get; set; }
        Int32 CurrentItemId { get; set; }
        Int32 ExceptionItemIdUpdated { get;set; }
        List<ApplicantDocuments> lstApplicantDocument { get; set; }
        Int32 CurrentTenantId_Global { get; set; }
        Int32 SelectedTenantId_Global { get; set; }
        Int32 SelectedCompliancePackageId_Global { get; set; }
        Int32 SelectedComplianceCategoryId_Global { get; set; }
        Int32 SelectedApplicantId_Global { get; set; }
        Int32 CurrentPackageSubscriptionId { get; set; }
        List<ListItemAssignmentProperties> lstAssignmentProperties { get; set; }
        DateTime? ItemExpirationDate { get; set; }
        //UAT-1056
        Boolean IsItemAsisgnedToCurrentUser
        {
            get;
            set;
        }

        Boolean IsAdminLoggedIn { get; set; } //UAT 2807

        #region UAT-3951:Rejection Reason
        List<Entity.RejectionReason> ListRejectionReasons { get; set; }
        List<Int32> SelectedRejectionReasonIds { get; }
        
        #endregion
    }
}




