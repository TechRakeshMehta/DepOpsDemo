using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IReconciliationDocumentPanelView
    {
        Int32 SelectedTenantId { get; set; }
        Int32 PackageSubscriptionId { get; set; }
        Int32 SelectedComplianceCategoryId_Global { get; set; }
        //String DocumentType { get;set; }
        Int32 ItemDataId { get; set; }
        List<ApplicantDocuments> lstApplicantDocument { get; set; }
        Int32 OrganizationUserId { get; set; }
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
    }
}
