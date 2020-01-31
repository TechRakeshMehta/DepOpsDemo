using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IRequirementApprovalNotificationDocument
    {
        Int32 AgencyHierarchyID { get; set; }

        IRequirementApprovalNotificationDocument CurrentViewContext { get; }

        Int32 CurrentUserId { get; }

        List<RequirementApprovalNotificationDocumentContract> ToSaveUploadedDocuments { get; set; }

        RequirementApprovalNotificationDocumentContract RequirementApprovalNotificationDocument { get; set; }
    }
}
