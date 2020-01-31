using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.PlacementMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.PlacementMatching.Views
{
    public interface IManageRequestView
    {
        Int32 CurrentLoggedInUserID { get; }
        String UserID { get; }
        List<TenantDetailContract> lstTenants { get; set; }
        List<WeekDayContract> WeekDayList { set; }
        List<String> SharedUserTypeCodes { get; }
        List<RequestDetailContract> lstRequests { get; set; }
        RequestDetailContract SearchRequestContract { get; set; }
        Boolean IsAdvanceSearchPanelDisplay { get; set; }

        List<lkpInventoryAvailabilityType> lstInstitutionAvailability { get; set; }
        List<AgencyLocationDepartmentContract> lstLocations { get; set; }
        Guid UserId { get; }
        Int32 AgencyRootNodeID { get; set; }
        List<DepartmentContract> lstDepartment { get; set; }
        List<StudentTypeContract> lstStudentType { get; set; }
        List<SpecialtyContract> lstSpecialties { get; set; }
        List<RequestStatusContract> lstRequestStatus { get; set; }
        List<CustomAttribteContract> lstSharedCustomAttribute { get; set; }
        String CustomDataXML { get; }
    }
}
