using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRequirementVerificationDocViewerView
    {
        Int32 SelectedTenantId { get; set; }

        Int32 OrganizationUserId { get; set; }

        Int32 CurrentDocumentId { get; set; }

        ApplicantDocument ApplicantDocument { get; set; }

    }
}
