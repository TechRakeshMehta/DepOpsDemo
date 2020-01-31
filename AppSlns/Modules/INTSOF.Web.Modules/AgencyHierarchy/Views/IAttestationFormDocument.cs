using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IAttestationFormDocument
    {
        Int32 AgencyHierarchyID { get; set; }

        IAttestationFormDocument CurrentViewContext { get; }

        Int32 CurrentUserId { get; }

        List<RequirementApprovalNotificationDocumentContract> ToSaveUploadedDocuments { get; set; }

        RequirementApprovalNotificationDocumentContract AttestationFormDocument { get; set; }

        AgencyHierarchySettingContract AgencyHierarchySettingContract { get; set; }

        Int32 SelectedRootNodeID { get; set; }
    }
}
