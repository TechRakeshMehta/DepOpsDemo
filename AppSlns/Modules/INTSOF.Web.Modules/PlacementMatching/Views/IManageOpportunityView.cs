using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.SharedDataEntity;
using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Core;

namespace CoreWeb.PlacementMatching.Views
{
    public interface IManageOpportunityView
    {
        Int32 CurrentLoggedInUserID { get; }
        List<lkpInventoryAvailabilityType> lstInstitutionAvailability { get; set; }
        List<AgencyLocationDepartmentContract> lstLocations { get; set; }
        Guid UserId { get; }
        Int32 AgencyRootNodeID { get; set; }
        List<DepartmentContract> lstDepartment { get; set; }
        List<StudentTypeContract> lstStudentType { get; set; }
        List<SpecialtyContract> lstSpecialties { get; set; }
        List<PlacementMatchingContract> lstPlacementMatchingContract { get; set; }
        PlacementMatchingContract SearchContract { get; set; }
        Dictionary<Int32, String> SelectedOpportunityIDs { get; set; }
        Boolean IsAdvanceSearchPanelDisplay { get; set; }
        List<TenantDetailContract> lstTenants { get; set; }
        List<String> SharedUserTypeCodes { get; }
        List<WeekDayContract> WeekDayList { set; }
        Boolean IsSavedSuccessfully { get; set; }
        List<CustomAttribteContract> lstSharedCustomAttribute { get; set; }
        String CustomDataXML { get; }
    }
}
