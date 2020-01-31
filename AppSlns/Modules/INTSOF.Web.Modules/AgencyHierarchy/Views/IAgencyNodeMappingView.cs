using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IAgencyNodeMappingView
    {
        IAgencyNodeMappingView CurrentViewContext { get; }
        List<AgencyNodeMappingContract> lstAgencies { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        Int32 AgencyHierarchyID { get; set; }
        Boolean IsAgencyHierarchyLeafNode { get; set; }
        AgencyNodeMappingContract AgencyNodeMappingContract { get; set; }
        List<AgencyNodeMappingContract> lstAgencyHierarchyAgencies { get; set; }
        List<RequirementApprovalNotificationDocumentContract> ToSaveUploadedDocuments { get; set; } //UAT 2821
        AgencyHierarchySettingContract AgencyHierarchySettingContract { get; set; } //UAT 2821
    }
}
