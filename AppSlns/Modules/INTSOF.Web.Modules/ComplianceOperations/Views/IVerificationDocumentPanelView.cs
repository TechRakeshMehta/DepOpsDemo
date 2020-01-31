using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IVerificationDocumentPanelView
    {
        Int32 SelectedTenantId { get; set; }
        Int32 PackageSubscriptionId { get; set; }
        Int32 SelectedComplianceCategoryId_Global { get; set; }
        //String DocumentType { get;set; }
        Int32 ItemDataId { get; set; }
        List<ApplicantDocuments> lstApplicantDocument { get; set; }
        Int32 OrganizationUserId { get; set; }
        String WorkQueue
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        /// <summary>
        /// Document TypeID of Screening document synched by data synching process.
        /// </summary>
        Int32 ScreeningDocumentTypeId
        {
            get;
            set;
        }
		String SelectedPackageId { get; set; }
		String SelectedItemDataId { get; set; }
		String SelectedComplianceItemReconciliationDataID { get; set; }

	}
}




