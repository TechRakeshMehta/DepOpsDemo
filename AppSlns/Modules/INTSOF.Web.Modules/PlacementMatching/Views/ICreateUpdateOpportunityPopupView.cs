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
    public interface ICreateUpdateOpportunityPopupView
    {
        Int32 CurrentLoggedInUserID { get; }

        List<lkpInventoryAvailabilityType> lstInstitutionAvailability { get; set; }
        List<StudentTypeContract> lstStudentType { get; set; }
        String UserId { get; }
        Int32 AgencyRootNodeID { get; set; }
        List<AgencyLocationDepartmentContract> lstLocations { get; set; }
        List<DepartmentContract> lstDepartment { get; set; }
        List<SpecialtyContract> lstSpecialties { get; set; }

        Int32 OpportunityID { get; set; }
        Boolean IsPreviewClick { get; set; }
        Boolean IsEditClicked { get; set; }
        Boolean IsNewOpportunity { get; set; }
        Boolean IsExistingOpportunity { get; set; }
        Boolean IsShiftsLoadFirstTime { get; set; }
        String StatusCode { get; set; }
        List<WeekDayContract> WeekDayList { get; set; }
        List<String> SharedUserTypeCodes { get; }
        List<TenantDetailContract> lstTenants { get; set; }
        PlacementMatchingContract PlacementMatchingContract { get; set; }
        Int32 SelectedAgencyLocationID { get; set; }
        Int32 SelectedDepartmentID { get; set; }
        List<ShiftDetails> lstShiftDetails { get; set; }
        ShiftDetails ShiftDetail { get; set; }
        //Int32 LastAddedShiftIndex { get; set; }
        //Boolean IsShiftEdit { get; set; }
        List<CustomAttribteContract> lstSharedCustomAttribute { get; set; }

        List<CustomAttribteContract> SaveCustomAttributeList { get; set; }
    }
}
