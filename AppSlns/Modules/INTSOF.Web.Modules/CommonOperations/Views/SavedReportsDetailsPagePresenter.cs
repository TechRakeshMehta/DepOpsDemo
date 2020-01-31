using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.SystemSetUp;

namespace CoreWeb.CommonOperations.Views
{
    public class SavedReportsDetailsPagePresenter : Presenter<ISavedReportsDetailsPageView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
        }

        public void GetSelectedFavParamDetails()
        {
            View.SelectedFavouriteParameter = ReportManager.GetReportFavouriteParameterByID(View.SelectedFavParamID);
        }

        public List<Tenant> GetTenants()
        {
            return ComplianceDataManager.getClientTenant();
        }

        public List<Entity.ClientEntity.lkpPackageComplianceStatu> GetPackageComplianceStatus(Int32 tenantID)
        {
            if (tenantID > AppConsts.NONE)
            {
                return ComplianceDataManager.GetOverAllComplianceStatus(tenantID);
            }
            return new List<lkpPackageComplianceStatu>();
        }

        public List<Entity.ClientEntity.lkpCategoryComplianceStatu> GetCategoryComplianceStatus(Int32 tenantID)
        {
            if (tenantID > AppConsts.NONE)
            {
                return ComplianceDataManager.GetCategoryComplianceStatus(tenantID);
            }
            return new List<lkpCategoryComplianceStatu>();
        }

        public List<Entity.ClientEntity.lkpArchiveState> GetArchiveStateList(Int32 tenantID)
        {
            if (tenantID > AppConsts.NONE)
            {
                return ComplianceDataManager.GetArchiveStateList(tenantID);
            }
            return new List<Entity.ClientEntity.lkpArchiveState>();
        }

        public List<Entity.ClientEntity.lkpArchiveState> GetArchiveStateListForNonCompliantReport(Int32 tenantID)
        {
            if (tenantID > AppConsts.NONE)
            {
                return ComplianceDataManager.GetArchiveStateListForNonCompliantReport(tenantID);
            }
            return new List<Entity.ClientEntity.lkpArchiveState>();
        }

        public Dictionary<Int32, String> GetUserGroupListFilterForReport(Int32 tenantID)
        {
            if (tenantID > AppConsts.NONE)
            {
                return ComplianceDataManager.GetUserGroupListFilterForReport(tenantID);
            }
            return new Dictionary<Int32, String>();
        }

        public Dictionary<Int32, String> GetCategoryListFilterForReport(Int32 tenantID, String nodeIds)
        {
            if (tenantID > AppConsts.NONE)
            {
                return ComplianceDataManager.GetCategoryListFilterForReport(tenantID, nodeIds);
            }
            return new Dictionary<Int32, String>();
        }

        public Dictionary<Int32, String> GetItemListFilterForReport(Int32 tenantID, String selectedCategoryIds)
        {
            if (tenantID > AppConsts.NONE)
            {
                return ComplianceDataManager.GetItemListFilterForReport(tenantID, selectedCategoryIds);
            }
            return new Dictionary<Int32, String>();
        }

        public Dictionary<Int32, String> GetHierarchyListFilterForReport(Int32 tenantID, Int32 userID)
        {
            if (tenantID > AppConsts.NONE)
            {
                return ComplianceDataManager.GetHierarchyListFilterForReport(tenantID, userID);
            }
            return new Dictionary<Int32, String>();
        }

        public Boolean UpdateFavParamReportParamMapping(Dictionary<Int32, String> dicUpdatedParameters, Entity.ReportFavouriteParameter favParam)
        {
            return ReportManager.UpdateFavParamReportParamMapping(dicUpdatedParameters, favParam);
        }

        #region UAT-3052
        public List<Entity.SharedDataEntity.Agency> GetAgencyHierarchy(String loggedInEmailID)
        {
            return ComplianceDataManager.GetAgencyHierarchy(loggedInEmailID);
        }

        public List<Entity.SharedDataEntity.Agency> GetAgencyUsers(String loggedInEmailID)
        {
            return ComplianceDataManager.GetAgencyUsers(loggedInEmailID);
        }

        public Dictionary<String, String> GetItemListForReportsByTenantIdLoggedInEmail(String tenantIDs, String loggedInEmailId)
        {
            if (!tenantIDs.IsNullOrEmpty())
            {
                return ComplianceDataManager.GetItemListForReportsByTenantIdLoggedInEmail(tenantIDs, loggedInEmailId);
            }
            return new Dictionary<String, String>();
        }

        public Dictionary<String, String> GetAllItemListForReportsByTenantIdLoggedInEmail(String tenantIDs, String loggedInEmailId)
        {
            if (!tenantIDs.IsNullOrEmpty())
            {
                return ComplianceDataManager.GetAllItemListForReportsByTenantIdLoggedInEmail(tenantIDs, loggedInEmailId);
            }
            return new Dictionary<String, String>();
        }

        public List<Entity.ClientEntity.Tenant> GetTenantsByTenantId(String tenantIDs)
        {

            if (!tenantIDs.IsNullOrEmpty())
            {
                return ComplianceDataManager.GetTenantsByTenantId(tenantIDs);
            }
            return new List<Entity.ClientEntity.Tenant>();
        }
        #endregion

        #region UAT 3143

        public Dictionary<String, String> GetCategoryListFilterForLoggedInAgencyUserReports(String selectedTenantIDs, String loggedInUserEmailId)
        {
            if (!selectedTenantIDs.IsEmpty())
            {
                return ComplianceDataManager.GetCategoryListFilterForLoggedInAgencyUserReports(selectedTenantIDs, loggedInUserEmailId);
            }
            return new Dictionary<String, String>();
        }

        #endregion

        #region [UAT-4509]
        public Dictionary<String, String> GetAllCategoryListFilterForLoggedInAgencyUserReports(String selectedTenantIDs, String loggedInUserEmailId)
        {
            if (!selectedTenantIDs.IsEmpty())
            {
                return ComplianceDataManager.GetAllCategoryListFilterForLoggedInAgencyUserReports(selectedTenantIDs, loggedInUserEmailId);
            }
            return new Dictionary<String, String>();
        }
        #endregion

        /// <summary>
        /// UAT-3146
        /// Get the Rotation List for Agency user Reports
        /// </summary>
        /// <param name="SelectedTenantIDs"></param>
        /// <param name="loggedInUserEmailId"></param>
        /// <returns></returns>
        public Dictionary<String, String> GetRotationListFilterForLoggedInAgencyUserReports(String SelectedTenantIDs, String loggedInUserEmailId)
        {
            if (!SelectedTenantIDs.IsEmpty())
            {
                return ComplianceDataManager.GetRotationListFilterForLoggedInAgencyUserReports(SelectedTenantIDs, loggedInUserEmailId);
            }
            return new Dictionary<String, String>();
        }

        //UAT 3214
        public Dictionary<String, String> GetWeekDaysList()
        {
            List<Entity.SharedDataEntity.lkpWeekDay> lstWeekDays = ComplianceDataManager.GetWeekDaysList();
            Dictionary<String, String> dicWeekDays = new Dictionary<String, String>();

            foreach (Entity.SharedDataEntity.lkpWeekDay item in lstWeekDays)
            {
                dicWeekDays.Add(item.WD_Code, item.WD_Name);
            }

            return dicWeekDays; 
        }

        public Boolean DeleteFavParamReportParamMapping(string RPF_ids)
        {
            return ReportManager.DeleteFavParamReportParamMapping(RPF_ids, View.CurrentLoggedInUserId);

        }

        public Dictionary<String, String> GetUserTypesForReports()
        {
            return ComplianceDataManager.GetUserTypesForReports();
        }

        public Dictionary<String, String> GetInvitationReviewStatus()
        {
            return ComplianceDataManager.GetInvitationReviewStatus();
        }
    }
}


