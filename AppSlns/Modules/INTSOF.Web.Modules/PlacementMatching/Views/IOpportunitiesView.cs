using Entity;
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


    public interface IOpportunitiesView
    {
        List<TenantDetailContract> lstTenants { get; set; }
        Int32 SelectedTenantID { get; set; }
        List<PlacementMatchingContract> lstPlacementMatching { get; set; }
        List<WeekDayContract> WeekDayList { set; }
        Int32 TenantID { get; set; }
        Boolean IsAdminLoggedIn { get; set; }
        List<StudentTypeContract> lstStudentTypes { set; }
        List<ShiftDetails> lstShifts { set; }
        List<DepartmentContract> lstDepartments { set; }
        List<SpecialtyContract> lstSpecialties { set; }
        List<AgencyLocationDepartmentContract> lstLocations { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 OrganizationUserId { get; }
        Int32 SelectedAgencyRootNodeID { get; set; }
        List<CustomAttribteContract> lstSharedCustomAttribute { get; set; }
        #region Search Properties

        Int32? selectedDepartmentId { get; }
        Int32? selectedLocationId { get; }
        Int32? selectedSpecialtyId { get; }
        String  selectedShift { get; }
        DateTime? StartDate { get; }
        DateTime? EndDate { get; }
        String Max { get; }
        String selectedStudentTypeIds { get; }
        String Days { get; }
        String CustomDataXML { get; }
        #endregion

   
    }
}
