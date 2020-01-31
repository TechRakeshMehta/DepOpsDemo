using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IVerificationDetailsUnassignedDocumentConrolView
    {
        Int32 ItemDataId { get; set; }
        Int32 ComplianceItemId { get; set; }
        Int32 ApplicantId { get; set; }
        Int32 SelectedTenantId_Global { get; set; }
        Int32 OrganiztionUserID { get; set; }
        Int32 CurrentLoggedUserID { get; }

        Int32 CurrentLoggedInUserId { get; }
        List<ApplicantDocument> ToSaveApplicantUploadedDocuments { get; set; }
        List<ApplicantComplianceAttributeData> lstApplicantComplianceAttributeData { get; set; }
        ApplicantComplianceItemData ApplicantComplianceItemData { get; set; }
        Boolean IsException { get; set; }
        //Entity.OrganizationUser OrganizationUserData { get; set; }
        List<ApplicantComplianceDocumentMap> lstApplicantComplianceDocumentMaps { get; set; }
        List<ExceptionDocumentMapping> lstExceptionDocumentDocumentMaps { get; set; }
        List<ApplicantDocuments> lstApplicantDocument { get; set; }
        Int32 PackageSubscriptionId { get; set; }
        Boolean IsFileUploadApplicable { get; set; }

        Int32 CompliancePackageId { get; set; }
        Int32 ComplianceCategoryId { get; set; }

        /// <summary>
        /// Can be NULL
        /// </summary>
        Int32 ComplianceAttributeId { get; set; }

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

        /// <summary>
        /// Contains the Assignment Properties of the current item
        /// </summary>
        List<ListItemAssignmentProperties> lstAssignmentProperties
        {
            get;
            set;
        }

        #region UAT-1049:Admin Data Entry
        Int16 DataEntryDocNewStatusId { get; set; }
        #endregion

        /// <summary>
        /// UAT1738 - Remove the Documents from Document panel, attached to "Screening Document Type" attribute.
        /// </summary>
        Int32 ScreeningDocTypeId
        {
            get;
            set;
        }

        Int32 CurrentTenantId_Global { get; }
    }
}




