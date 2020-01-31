using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManageUploadDocumentView
    {
        List<ApplicantDocumentDetails> ApplicantUploadedDocuments { get; set; }

        ApplicantDocument ToUpdateUploadedDocument { get; set; }

        Int32 CurrentUserID { get; }

        String AllowedFileExtension { get; set; }

        Int32 TenantID { get; set; }

        Int32 ApplicantUploadedDocumentID { get; set; }

        Int32 FromAdminApplicantID { get; set; }

        Int32 FromAdminTenantID { get; set; }

        Boolean IsAdminScreen { get; set; }

        String PageType { get; set; }

        Int32 PackageSubscriptionId { get; set; }

        String WorkQueue
        {
            get;
            set;
        }
        String ErrorMessage
        {
            get;
            set;
        }

        String IsUploadOrViewDocument
        {
            get;
            set;
        }
        #region UAT-977: Additional work towards archive ability
        Boolean IsActiveSubscription { get; set; }
        #endregion

        #region UAT-1544
        List<Int32> DocumentIdsToPrint { get; set; }
        #endregion

        Int32 OrgUsrID { get; }

        #region UAT-2296
        List<UploadDocumentContract> lstSubcribedItems { get; set; }

        List<lkpItemDocMappingType> lstItemDocMappingType { get; set; }

        List<lkpDataEntryDocumentStatu> lkpDataEntryDocumentStatus { get; set; }
        #endregion

        Boolean IsRequirementFieldUploadDocument { get; set; }

		String SelectedComplianceCategoryId_Global { get; set; }

		String SelectedPackageId { get; set; }
		String SelectedItemDataId { get; set; }
		String SelectedComplianceItemReconciliationDataID { get; set; }

        #region UAT-4687
        Boolean IsApplicantClinicalRotationMember
        {
            get;
            set;
        }
        #endregion
    }
}




