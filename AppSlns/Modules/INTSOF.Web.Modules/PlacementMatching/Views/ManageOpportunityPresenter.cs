using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using Entity.SharedDataEntity;
using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace CoreWeb.PlacementMatching.Views
{
    public class ManageOpportunityPresenter : Presenter<IManageOpportunityView>
    {
        public override void OnViewInitialized()
        {
            GetTenants();
            GetAssociatedTenants();
            Int32 tenantID = View.lstTenants.FirstOrDefault().TenantID;
            GetWeekDays(tenantID);
        }

        public void GetAssociatedTenants()
        {
            List<Int32> sharedUserTenantIDs = new List<Int32>();
            sharedUserTenantIDs = GetSharedUserTenantIds();
            if (!View.lstTenants.IsNullOrEmpty() && View.lstTenants.Count > AppConsts.NONE)
                View.lstTenants = View.lstTenants.Where(x => sharedUserTenantIDs.Contains(x.TenantID)).ToList();
        }

        /// <summary>
        /// Method To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenants = new List<TenantDetailContract>();
            View.lstTenants = ClinicalRotationManager.GetTenants(SortByName, clientCode);
        }

        public List<Int32> GetSharedUserTenantIds()
        {
            List<Int32> lstTenantids = SharedUserClinicalRotationManager.GetSharedUserTenantIDs(View.UserId.ToString(), View.SharedUserTypeCodes);
            return lstTenantids;
        }
        /// <summary>
        /// Method to get week days of an institution
        /// </summary>
        /// <param name="tenantID"></param>
        public void GetWeekDays(Int32 tenantID)
        {
            View.WeekDayList = ClinicalRotationManager.GetWeekDayList(tenantID);
        }

        public void GetInstitutionAvailability()
        {
            View.lstInstitutionAvailability = new List<lkpInventoryAvailabilityType>();
            View.lstInstitutionAvailability = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpInventoryAvailabilityType>().Where(cond => !cond.IAT_IsDeleted).ToList();
            View.lstInstitutionAvailability = PlacementMatchingSetupManager.GetInstitutionAvailability();
        }

        public void GetAgencyRootNode()
        {
            Dictionary<Int32, String> dicAgencyRootNode = new Dictionary<Int32, String>();
            dicAgencyRootNode = PlacementMatchingSetupManager.GetAgencyRootNode(View.UserId);
            if (!dicAgencyRootNode.IsNullOrEmpty())
            {
                View.AgencyRootNodeID = dicAgencyRootNode.Keys.FirstOrDefault();
            }
        }

        public void GetLocations()
        {
            View.lstLocations = new List<AgencyLocationDepartmentContract>();
            View.lstLocations = PlacementMatchingSetupManager.GetAgencyLocations(View.AgencyRootNodeID);
        }

        public void GetDepartments()
        {
            View.lstDepartment = new List<DepartmentContract>();
            View.lstDepartment = PlacementMatchingSetupManager.GetPlacementDepartments();
        }

        public void GetStudentTypes()
        {
            View.lstStudentType = new List<StudentTypeContract>();
            View.lstStudentType = PlacementMatchingSetupManager.GetPlacementStudentTypes();
        }

        public void GetSpecialty()
        {
            View.lstSpecialties = new List<SpecialtyContract>();
            View.lstSpecialties = PlacementMatchingSetupManager.GetPlacementSpecialties();
        }

        public void GetOpportunities()
        {
            View.lstPlacementMatchingContract = new List<PlacementMatchingContract>();
            View.lstPlacementMatchingContract = PlacementMatchingSetupManager.GetOpportunities(View.SearchContract);  //Root node id and search contract needed to pass 
        }

        public Boolean DeleteOpportunity()
        {
            if (!View.SelectedOpportunityIDs.IsNullOrEmpty())
                return PlacementMatchingSetupManager.DeleteOpportunity(View.CurrentLoggedInUserID, View.SelectedOpportunityIDs.Keys.ToList());
            return false;
        }

        public Boolean ArchiveOpportunities()
        {
            if (!View.SelectedOpportunityIDs.IsNullOrEmpty())
                return PlacementMatchingSetupManager.ArchiveOpportunities(View.CurrentLoggedInUserID, View.SelectedOpportunityIDs.Keys.ToList());
            return false;
        }

        public Boolean UnArchiveOpportunities()
        {
            if (!View.SelectedOpportunityIDs.IsNullOrEmpty())
                return PlacementMatchingSetupManager.UnArchiveOpportunities(View.CurrentLoggedInUserID, View.SelectedOpportunityIDs.Keys.ToList());
            return false;
        }

        public void GetSharedCustomAttributeList()
        {
            Int32? recordId = null;
            String recordTypeCode = CustomAttributeValueRecordType.ClinicalInventory.GetStringValue();
            View.lstSharedCustomAttribute = PlacementMatchingSetupManager.GetSharedCustomAttributeList(View.AgencyRootNodeID, SharedCustomAttributeUseType.ClinicalInventory.GetStringValue(), recordId, recordTypeCode);
        }
        
    }
}
