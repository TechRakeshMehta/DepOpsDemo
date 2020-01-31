using Business.RepoManagers;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.PlacementMatching.Views
{
    public class ManageRequestPresenter : Presenter<IManageRequestView>
    {
        /// <summary>
        /// On View Initialized Event
        /// </summary>
        public override void OnViewInitialized()
        {
            List<TenantDetailContract> tenants = GetTenants();
            List<Int32> sharedUserTenantIDs = new List<Int32>();

            sharedUserTenantIDs = GetSharedUserTenantIds();
            var tenantList = tenants.Where(x => sharedUserTenantIDs.Contains(x.TenantID)).ToList();
            View.lstTenants = tenantList;
            var tenantID = tenants.FirstOrDefault().TenantID;
            GetWeekDays(tenantID);
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

        public List<Int32> GetSharedUserTenantIds()
        {
            List<Int32> lstTenantids = SharedUserClinicalRotationManager.GetSharedUserTenantIDs(View.UserID, View.SharedUserTypeCodes);
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

        public void GetRequests()
        {
            View.lstRequests = new List<RequestDetailContract>();
            View.lstRequests = PlacementMatchingSetupManager.GetRequests(View.SearchRequestContract);
            foreach (RequestDetailContract item in View.lstRequests)
            {
                if (item.StatusCode == RequestStatusCodes.Requested.GetStringValue())
                {
                    item.RequestStatus = "Pending Review";
                }
            }
        }

        public void GetRequestStatuses()
        {
            View.lstRequestStatus = PlacementMatchingSetupManager.GetRequestStatuses();
        }

        public void GetSharedCustomAttributeList()
        {
            Int32? recordId = null;
            String recordTypeCode = CustomAttributeValueRecordType.ClinicalInventoryRequest.GetStringValue();
            View.lstSharedCustomAttribute = PlacementMatchingSetupManager.GetSharedCustomAttributeList(View.AgencyRootNodeID, SharedCustomAttributeUseType.ClinicalInventoryRequest.GetStringValue(), recordId, recordTypeCode);
        }
    }
}
