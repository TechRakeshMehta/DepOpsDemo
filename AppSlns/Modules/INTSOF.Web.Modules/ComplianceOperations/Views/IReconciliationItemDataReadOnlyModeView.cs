using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;


namespace CoreWeb.ComplianceOperations.Views
{
    public interface IReconciliationItemDataReadOnlyModeView
    {
        IReconciliationItemDataReadOnlyModeView CurrentViewContext { get; }
        List<ApplicantItemVerificationData> VerificationData { get; set; }
        String FormMode { get; set; }
        Int32 SelectedTenantId_Global { get; set; }

        Int32 CurrentPackageSubscriptionId { get; set; }
        List<ApplicantDocuments> lstApplicantDocument { get; set; }


        /// <summary>
        /// Get the Mapped documents from the Loader control and pass on to the Document control
        /// </summary>
        List<ApplicantComplianceDocumentMap> lstApplicantComplianceDocumentMaps
        {
            get;
            set;
        }
        List<ListItemAssignmentProperties> lstAssignmentProperties { get; set; }
    }
}