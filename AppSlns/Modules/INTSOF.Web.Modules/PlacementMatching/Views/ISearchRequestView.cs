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
    public interface ISearchRequestView
    {
        Int32 CurrentLoggedInUserID { get; }
        List<RequestDetailContract> lstPlacementMaching { get; set; }
        List<WeekDayContract> WeekDayList { set; }
        List<TenantDetailContract> lstTenants { get; set; }
        Int32 TenantID { get; set; }
        Int32 SelectedTenantID { get; set; }
        Boolean IsAdminLoggedIn { get; set; }
        List<StudentTypeContract> lstStudentTypes { set; }
        List<ShiftDetails> lstShifts { set; }
        List<DepartmentContract> lstDepartments { set; }
        List<RequestStatusContract> lstRequestStatuses { set; }
        List<SpecialtyContract> lstSpecialties { set; }
        List<AgencyLocationDepartmentContract> lstLocations { get; set; }
        String CustomDataXML { get; }
        List<CustomAttribteContract> CustomAttributeList
        {
            get;
            set;
        }

        #region Search Properties

        Int32? selectedDepartmentId { get; }
        Int32? selectedLocationId { get; }
        Int32? selectedSpecialtyId { get; }
        String selectedShift { get; }
        DateTime? StartDate { get; }
        DateTime? EndDate { get; }
        String Max { get; }
        String selectedStudentTypeIds { get; }
        String Days { get; }
        String selectedStatusIds { get; }
        Int32 CurrentAgencyRootID { get; set; }
        #endregion



    }
}
