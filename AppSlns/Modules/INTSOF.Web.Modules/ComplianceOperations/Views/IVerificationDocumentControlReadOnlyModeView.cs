using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IVerificationDocumentControlReadOnlyModeView
    {
        Int32 ItemDataId { get; set; }
        Int32 SelectedTenantId_Global { get; set; }
        Boolean IsException { get; set; }
        Entity.OrganizationUser OrganizationUserData { get; set; }
        Int32 PackageSubscriptionId { get; set; }
        List<ApplicantDocuments> lstApplicantDocument { get; set; }

        /// <summary>
        /// Screening Document Type Attribute Data Type Id
        /// </summary>
        Int32 ScreeningDocTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Mapped documents from the Read Only control
        /// </summary>
        List<ApplicantComplianceDocumentMap> lstApplicantComplianceDocumentMaps
        {
            get;
            set;
        }
    }
}




