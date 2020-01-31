using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IItemDataEditModeView
    {
        List<ApplicantComplianceAttributeData> lstApplicantComplianceAttributeData { get; set; }
        IItemDataEditModeView CurrentViewContext { get; }
        List<ApplicantItemVerificationData> VerificationData { get; set; }

        Int32 ApplicantItemDataId { get; set; }
        Int32 ApplicantCategoryDataId { get; set; }
        List<ApplicantDocuments> lstApplicantDocument { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        //String StatusCode { get; set; }
        String Comments { get; set; }
        String ItemComplianceStatusCode { get; set; }
        //Entity.Tenant Tenant { get; }

        Int32 PackageId { get; set; }
        Int32 ComplianceItemId { get; set; }
        //Int32 ComplianceCategoryId { get; set; }
        Int32 SelectedCompliancePackageId_Global { get; set; }
        Int32 SelectedComplianceCategoryId_Global { get; set; }

        String UIInputException { get; set; }
        Boolean IsUIValidationApplicable { get; }

        String AttemptedItemStatus { get; set; }
        Int32 AttemptedItemStatusId { get; }

        Int32 TPReviewerUserId { get; set; }
        Int32? ReviewerTenantId { get; set; }
        Int16 ReviewerTypeId { get; set; }

        Boolean IsAdminReviewRequired { get; set; }

        /// <summary>
        /// Status of the Item from one mode to another
        /// </summary>
        String ItemMovementStatusCode { get; set; }

        Int32 CurrentTenantId_Global { get; set; }

        Int32 SelectedTenantId_Global { get; set; }
        Int32 SelectedApplicantId_Global { get; set; }
        String CurrentLoggedInUserName_Global { get; set; }
        Int32 CurrentPackageSubscriptionId { get; set; }
        List<ListItemAssignmentProperties> lstAssignmentProperties { get; set; }
        Int32? IncompleteItemNewStatusId
        {
            get;
            set;
        }

        String IncompleteItemNewStatusCode
        {
            get;
            set;
        }

        String LoggedInUserInitials_Global
        {
            get;
            set;
        }

        String StatusComments
        {
            get;
            set;
        }

        //UAT-1056
        Boolean IsItemAsisgnedToCurrentUser
        {
            get;
            set;
        }

        Int32? ReconciliationReviewCount
        {
            get;
            set;
        }

        //UAT-2807
        String VerificationCommentsWithInitials
        {
            get;
            set;
        }
        #region UAT-3951:Rejection Reason
        List<Entity.RejectionReason> ListRejectionReasons { get; set; }
        List<Int32> SelectedRejectionReasonIds { get; }
       
        #endregion
    }

}




