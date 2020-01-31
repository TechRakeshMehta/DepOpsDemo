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
    public class CreateUpdateOpportunityPopupPresenter : Presenter<ICreateUpdateOpportunityPopupView>
    {
        /// <summary>
        /// On View Initialized Event
        /// </summary>
        public override void OnViewInitialized()
        {
            GetTenants();
            GetAssociatedTenants();
            Int32 tenantID = View.lstTenants.FirstOrDefault().TenantID;
            GetWeekDays(tenantID);
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

        public void GetAssociatedTenants()
        {
            List<Int32> sharedUserTenantIDs = new List<Int32>();
            sharedUserTenantIDs = GetSharedUserTenantIds();
            if (!View.lstTenants.IsNullOrEmpty() && View.lstTenants.Count > AppConsts.NONE)
                View.lstTenants = View.lstTenants.Where(x => sharedUserTenantIDs.Contains(x.TenantID)).ToList();
        }

        public List<Int32> GetSharedUserTenantIds()
        {
            List<Int32> lstTenantids = SharedUserClinicalRotationManager.GetSharedUserTenantIDs(View.UserId, View.SharedUserTypeCodes);
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
            Guid currentUserID = new Guid(View.UserId);
            dicAgencyRootNode = PlacementMatchingSetupManager.GetAgencyRootNode(currentUserID);
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

        public void GetLocationDepartments()
        {
            View.lstDepartment = new List<DepartmentContract>();
            View.lstDepartment = PlacementMatchingSetupManager.GetLocationDepartments(View.SelectedAgencyLocationID);
        }

        public void GetAgencyDepartmentStudentTypes()
        {
            View.lstStudentType = new List<StudentTypeContract>();
            View.lstStudentType = PlacementMatchingSetupManager.GetAgencyDepartmentStudentTypes(View.SelectedDepartmentID,View.SelectedAgencyLocationID);
        }

        public void GetSpecialty()
        {
            View.lstSpecialties = new List<SpecialtyContract>();
            View.lstSpecialties = PlacementMatchingSetupManager.GetPlacementSpecialties();
        }

        public void GetOpportunityDetailByID()
        {
            View.PlacementMatchingContract = new PlacementMatchingContract();
            if (!View.OpportunityID.IsNullOrEmpty() && View.OpportunityID > AppConsts.NONE)
            {
                View.PlacementMatchingContract = PlacementMatchingSetupManager.GetOpportunityDetailByID(View.OpportunityID);
            }
        }

        public Boolean PublishOpportunity()
        {
            if (!View.PlacementMatchingContract.IsNullOrEmpty())
            {
                return PlacementMatchingSetupManager.PublishOpportunity(View.CurrentLoggedInUserID, View.PlacementMatchingContract, View.SaveCustomAttributeList);
            }
            return false;
        }

        public Boolean SaveOpportunity()
        {
            if (!View.PlacementMatchingContract.IsNullOrEmpty())
            {
                return PlacementMatchingSetupManager.SaveOpportunity(View.CurrentLoggedInUserID, View.PlacementMatchingContract, View.SaveCustomAttributeList);
            }
            return false;
        }

        public void GetShiftsForOpportunity()
        {
            View.lstShiftDetails = new List<ShiftDetails>();
            View.lstShiftDetails = PlacementMatchingSetupManager.GetShiftsForOpportunity(View.OpportunityID);

            foreach (ShiftDetails shiftDetails in View.lstShiftDetails)
            {
                shiftDetails.Days = String.Join(",", View.WeekDayList.Where(cond => shiftDetails.lstDaysId.Contains(cond.WeekDayID)).Select(Sel => Sel.Name).ToList());
                shiftDetails.IsEditClick = false;
            }

            if (!View.PlacementMatchingContract.IsNullOrEmpty() && !View.lstShiftDetails.IsNullOrEmpty())
                View.PlacementMatchingContract.lstShift = View.lstShiftDetails;

        }

        public Boolean SaveShiftDetails()
        {
            if (!View.ShiftDetail.IsNullOrEmpty())
            {
                if (View.lstShiftDetails.IsNullOrEmpty())
                    View.lstShiftDetails = new List<ShiftDetails>();
                View.lstShiftDetails.Add(View.ShiftDetail);

                return true;
                //return PlacementMatchingSetupManager.SaveShiftDetails(View.CurrentLoggedInUserID, View.ShiftDetail);
            }
            return false;
        }

        public Boolean DeleteShiftDetail()
        {
            if (!View.ShiftDetail.IsNullOrEmpty())
            {
                if (!View.lstShiftDetails.IsNullOrEmpty())
                {
                    View.lstShiftDetails.Remove(View.lstShiftDetails.Where(cond => cond.ClinicalInventoryShiftID == View.ShiftDetail.ClinicalInventoryShiftID).FirstOrDefault());
                    return true;
                }
                return false;
                //return PlacementMatchingSetupManager.DeleteShiftDetail(View.CurrentLoggedInUserID, View.ShiftDetail);
            }
            return false;
        }

        public void GetSharedCustomAttributeList(Int32? recordId)
        {
            String recordTypeCode = CustomAttributeValueRecordType.ClinicalInventory.GetStringValue();
            View.lstSharedCustomAttribute = PlacementMatchingSetupManager.GetSharedCustomAttributeList(View.AgencyRootNodeID, SharedCustomAttributeUseType.ClinicalInventory.GetStringValue(), recordId, recordTypeCode);
        }


    }
}
