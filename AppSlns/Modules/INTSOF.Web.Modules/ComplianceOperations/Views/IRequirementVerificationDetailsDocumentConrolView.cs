using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRequirementVerificationDetailsDocumentConrolView
    {
        IRequirementVerificationDetailsDocumentConrolView CurrentViewContext { get; }
        Int32 RequirementItemDataId { get; set; }
        Int32 RequirementItemId { get; set; }
        Int32 ApplicantId { get; set; }
        Int32 SelectedTenantId_Global { get; set; }
        Int32 OrganiztionUserID { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        List<ApplicantFieldDocumentMappingContract> lstApplicantRequirementDocuments { get; set; }
        List<ApplicantDocumentContract> ToSaveApplicantUploadedDocuments { get; set; }
        List<ApplicantRequirementFieldData> lstApplicantRequirementFieldData { get; set; }
        ApplicantRequirementItemData ApplicantRequirementItemData { get; set; }
        List<ApplicantFieldDocumentMappingContract> lstApplicantRequirementDocumentMaps { get; set; }
        List<ApplicantDocumentContract> lstApplicantDocument { get; set; }
        Boolean IsFileUploadApplicable { get; set; }
        Boolean IsFieldRequired { get; set; }
        Int32 ViewApplDocId { get; set; }

        Int32 RequirementPackageSubscriptionId { get; set; }
        Int32 RequirementCategoryId { get; set; }
        Int32 RequirementFieldId { get; set; }

        /// <summary>
        /// Property to check if the document grid can be edited by the admin
        /// </summary>
        Boolean IsReadOnly { get; set; }

        /// <summary>
        /// Set from the Edit Mode screen to differentiate normal item with Incomplete Item
        /// </summary>
        Boolean IsIncompleteItem { get; set; }
        String ErrorMessage
        {
            get;
            set;
        }

        Int32 CurrentTenantId_Global { get; }
        //AUT
        String EntityPermissionName { get; set; }
        Boolean IsFileUploadControlExist { get; set; }

        #region UAT-4368
        Boolean IsClientAdminLoggedIn { get; set; }
        Boolean IsAdminLoggedIn { get; set; }
        Boolean IsFieldEditableByAdmin { get; set; }
        Boolean IsFieldEditableByApplicant { get; set; }
        Boolean IsFieldEditableByClientAdmin { get; set; }
        #endregion
    }
}




