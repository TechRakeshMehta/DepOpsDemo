using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace CoreWeb.PlacementMatching.Views
{
    public class SearchRequestPresenter : Presenter<ISearchRequestView>
    {
        public override void OnViewInitialized()
        {
            View.lstTenants = new List<TenantDetailContract>();
            View.lstTenants = GetTenants();
            var tenantID = View.lstTenants.FirstOrDefault().TenantID;
            GetWeekDays(tenantID);
            GetStudentTypes();
            GetSpecialties();
            GetDepartments();
            GetAllLocations();
            GetRequestStatuses();
            GetAllShifts();
        }
        public void GetSearchRequestData()
        {
            PlacementSearchContract searchContract = new PlacementSearchContract();
            searchContract.TenantId = View.SelectedTenantID;
            searchContract.StudentTypeIds = View.selectedStudentTypeIds.IsNullOrEmpty() ? String.Empty : View.selectedStudentTypeIds;
            searchContract.DepartmentId = View.selectedDepartmentId.HasValue ? View.selectedDepartmentId.Value.ToString() : String.Empty;
            searchContract.LocationId = View.selectedLocationId.HasValue ? View.selectedLocationId.Value.ToString() : String.Empty;
            searchContract.SpecialtyId = View.selectedSpecialtyId.HasValue ? View.selectedSpecialtyId.Value.ToString() : String.Empty;
            searchContract.Max = View.Max.IsNullOrEmpty() ? String.Empty : View.Max;
            searchContract.Shift = View.selectedShift.IsNullOrEmpty() ? String.Empty : View.selectedShift;
            searchContract.StartDate = View.StartDate.HasValue ? View.StartDate.Value : (DateTime?)null;
            searchContract.EndDate = View.EndDate.HasValue ? View.EndDate.Value : (DateTime?)null;
            searchContract.StatusIds = View.selectedStatusIds.IsNullOrEmpty() ? String.Empty : View.selectedStatusIds;
            if (!View.CustomDataXML.IsNullOrEmpty())
            {
                searchContract.SharedCustomAttributes = View.CustomDataXML;
            }
            View.lstPlacementMaching = PlacementMatchingSetupManager.GetPlacementRequests(searchContract);

        }
        public List<TenantDetailContract> GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            List<TenantDetailContract> lstTenants = ClinicalRotationManager.GetTenants(SortByName, clientCode);

            return lstTenants;
        }

        public void GetWeekDays(Int32 tenantID)
        {
            View.WeekDayList = ClinicalRotationManager.GetWeekDayList(tenantID);
        }
        public void IsAdminLoggedIn()
        {
            if (View.TenantID == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }

        public void GetSpecialties()
        {
            View.lstSpecialties = PlacementMatchingSetupManager.GetPlacementSpecialties();
        }
        public void GetStudentTypes()
        {
            View.lstStudentTypes = PlacementMatchingSetupManager.GetPlacementStudentTypes();
        }
        public void GetDepartments()
        {
            View.lstDepartments = PlacementMatchingSetupManager.GetPlacementDepartments();
        }

        public void GetAllLocations()
        {
            View.lstLocations = PlacementMatchingSetupManager.GetAllLocations();
        }

        public void GetAllShifts()
        {
            View.lstShifts = PlacementMatchingSetupManager.GetAllShifts();
        }
        public List<String> GetScreenColumnsToHide(String GrdCode, Int32 CurrentLoggedInUserId)
        {
            return SecurityManager.GetScreenColumnsToHide(GrdCode, CurrentLoggedInUserId);
        }
        public void GetRequestStatuses()
        {
            View.lstRequestStatuses = PlacementMatchingSetupManager.GetRequestStatuses();
        }
        public void GetSharedCustomAttributeList()
        {
            View.CustomAttributeList = new List<CustomAttribteContract>();
            View.CustomAttributeList = PlacementMatchingSetupManager.GetSharedCustomAttributeList(View.CurrentAgencyRootID, SharedCustomAttributeUseType.ClinicalInventoryRequest.GetStringValue(), null, null);
        }

        public Int32 GetAgencyHierarchyId(Int32 locationID)
        {
            return PlacementMatchingSetupManager.GetAllLocations().Where(cond => cond.AgencyLocationID == locationID).FirstOrDefault().AgencyHierarchyID;
        }
    }
}
