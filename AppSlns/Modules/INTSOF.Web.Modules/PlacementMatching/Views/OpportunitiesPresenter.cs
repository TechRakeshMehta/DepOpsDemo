using Business.RepoManagers;
using Entity;
using Entity.SharedDataEntity;
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
    public class OpportunitiesPresenter : Presenter<IOpportunitiesView>
    {
        private ADB_SharedDataEntities _sharedDataDBContext = new ADB_SharedDataEntities();

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
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

        /// <summary>
        /// On View Initialized Event
        /// </summary>
        public override void OnViewInitialized()
        {
            View.lstTenants = new List<TenantDetailContract>();
            View.lstTenants = GetTenants();
            var tenantID = View.lstTenants.FirstOrDefault().TenantID;
            GetWeekDays(tenantID);
            GetSpecialties();
            GetStudentTypes();
            GetDepartments();
            GetAllShifts();
        }

        public void GetWeekDays(Int32 tenantID)
        {
            View.WeekDayList = ClinicalRotationManager.GetWeekDayList(tenantID);
        }

        /// <summary>
        /// Method To get all Tenants
        /// </summary>
        public List<TenantDetailContract> GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            List<TenantDetailContract> lstTenants = ClinicalRotationManager.GetTenants(SortByName, clientCode);
            return lstTenants;
        }

        public void GetOpportunitySearch()
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
            if (!View.CustomDataXML.IsNullOrEmpty())
            {
                searchContract.SharedCustomAttributes = View.CustomDataXML;
            }
            View.lstPlacementMatching = PlacementMatchingSetupManager.GetOpportunitySearch(searchContract);
        }

        public String GetAgencyNames(String agencyIds)
        {
            List<Int32> lstAgencyIds = (agencyIds.Split(',').ToList()).Select(int.Parse).ToList();
            List<String> lstAgencyName = new List<String>();
            List<Agency> lstAgency = _sharedDataDBContext.Agencies.Where(cond => lstAgencyIds.Contains(cond.AG_ID) && !cond.AG_IsDeleted).ToList();
            foreach (Agency agency in lstAgency)
            {
                String agencyName = String.Empty;
                agencyName = agency.AG_Name.IsNullOrEmpty() ? agency.AG_Label : agency.AG_Name;
                lstAgencyName.Add(agencyName);
            }
            return String.Join(",", lstAgencyName);
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
            View.lstShifts = new List<ShiftDetails>();
            List<ShiftDetails> lstShifts = new List<ShiftDetails>();
            lstShifts = PlacementMatchingSetupManager.GetAllShifts();
            if (!lstShifts.IsNullOrEmpty())
                View.lstShifts = lstShifts.DistinctBy(dst => dst.ClinicalInventoryShiftID).DistinctBy(dst => dst.Shift).ToList();
        }

        public void GetSharedCustomAttributeList()
        {
            Int32? recordId = null;
            String recordTypeCode = CustomAttributeValueRecordType.ClinicalInventory.GetStringValue();
            View.lstSharedCustomAttribute = PlacementMatchingSetupManager.GetSharedCustomAttributeList(View.SelectedAgencyRootNodeID, SharedCustomAttributeUseType.ClinicalInventory.GetStringValue(), recordId, recordTypeCode);
        }
    }
}
