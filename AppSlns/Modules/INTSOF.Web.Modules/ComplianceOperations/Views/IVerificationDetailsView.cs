using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IVerificationDetailsView
    {
        IVerificationDetailsView CurrentViewContext { get; }
        Int32 CurrentLoggedInUserId { get; }

        String UIInputException { get; set; }

        Int32 TenantId_Global { get; set; }
        /// <summary>
        /// set package id for return back to queue.
        /// </summary>
        Int32 PackageId { get; set; }

        /// <summary>
        /// set Category id for return back to queue.
        /// </summary>
        Int32 CategoryId { get; set; }
        //String Notes { get; set; }

        Int32 ItemDataId_Global { get; set; }

        /// <summary>
        ///get and  set package id .
        /// </summary>
        Int32 CurrentCompliancePackageId_Global { get; set; }

        /// <summary>
        ///get and  set Applicant Id 
        /// </summary>
        Int32 CurrentApplicantId_Global { get; set; }

        /// <summary>
        ///get and  set package id .
        /// </summary>
        Int32 SelectedPackageSubscriptionID_Global { get; set; }

        /// <summary>
        ///get and  set Category id 
        /// </summary>
        Int32 SelectedComplianceCategoryId_Global { get; set; }
        Int32 PrevComplianceCategoryId_Global { get; set; }
        Int32 NextComplianceCategoryId_Global { get; set; }

        Int32 ReviewerUserId { get; set; }
        Boolean IncludeIncompleteItems { get; set; }
        WorkQueueType WorkQueue { get; set; }
        String WorkQueuePath { get; }

        String LoggedInUserName_Global { get; set; }

        Int32 CurrentTenantId_Global { get; set; }
        List<ApplicantDocuments> lstApplicantDocument { get; set; }
        String ApplicantFirstName { get; set; }
        String ApplicantLastName { get; set; }
        DateTime? DateOfBirth { get; set; }
        String ItemLabel { get; set; }
        Int32 ProgramId { get; set; }
        String SelectedItemComplianceStatusId { get; set; }
        Boolean IsException { get; set; }
        Entity.OrganizationUser OrganizationData { get; set; }
        String LoggedInUserInitials_Global { get; set; }

        //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
        Boolean IsFullPermissionForVerification
        { get; set; }
        //UAT-2971
        String UserID { get; set; }
        //UAT 4067
        //String AllowedFileExtensions_Global { get; set; }
    }
        
}




