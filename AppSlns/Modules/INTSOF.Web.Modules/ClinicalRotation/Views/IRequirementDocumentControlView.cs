using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRequirementDocumentControlView
    {
        Int32 SelectedTenantId { get; set; }
        
        Int32 OrganizationUserId { get; set; }
        
        Int32 CurrentLoggedInUserId { get; }

        Int32 ItemDataId { get; set; }
        
        List<ApplicantDocuments> lstApplicantDocument { get; set; }

        Int32 CurrentApplicantId { get; set; }

        Int32 ClinicalRotationId { get; set; }
    }
}
