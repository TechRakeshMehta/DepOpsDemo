using INTSOF.ServiceDataContracts.Modules.AgencyJobBoard;
using INTSOF.ServiceDataContracts.Modules.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyJobBoard.Views
{
    public interface IManageAgencyJobsView
    {
        IManageAgencyJobsView CurrentViewContext { get; }

        List<AgencyJobContract> LstAgencyJobTemplate { get; set; }

       
        List<DefinedRequirementContract> LstJobFieldType { get; set; }

        List<AgencyJobContract> LstAgencyJobPosting { get; set; }

        Boolean IsJobTemplate { get; }

        Int32 CurrentLoggedInUserId { get; }

        Int32 OrganisationUserID { get; }

        AgencyJobContract AgencyJob { get; set; }

        Int32 MappedAgencyHierarchyRootNodeId { get; set; }

        AgencyLogoContract AgencyLogo { get; set; }

        String OriginalFileName { get; set; }

        String FilePath { get; set; }

        List<Int32> SelectedAgencyPostIDs { get; set; }
    }
}
