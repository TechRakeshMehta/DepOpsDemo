using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils.Consts;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.Search.Views
{
    public class ApplicantPortFolioSearchPresenter : Presenter<IApplicantPortFolioSearchView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private ISearchController _controller;
        // public ApplicantUserGroupPresenter([CreateNew] ISearchController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetTenants();
            GetGranularPermissionForDOBandSSN();
            GetUserNodePermission();
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        /// <summary>
        /// To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = SecurityManager.GetTenants(SortByName, false, clientCode);
            //GetArchiveStateList();//UAT-977
        }

        /// <summary>
        /// To get Tenant Id
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// To get Client Id
        /// </summary>
        private Int32 ClientId
        {
            get
            {
                if (IsDefaultTenant)
                    return View.SelectedTenantId;
                return View.TenantId;
            }
        }

        /// <summary>
        /// To get Admin Program Study
        /// </summary>
        public void GetAllUserGroups()
        {
            if (ClientId == 0)
                View.lstUserGroup = new List<UserGroup>();
            else
            {
                //UAT-2284: User Group permisson/access and availability by node
                Int32? currentUserId = IsDefaultTenant ? (Int32?)null : View.CurrentLoggedInUserId;
                View.lstUserGroup = ComplianceSetupManager.GetAllUserGroupWithPermission(ClientId,currentUserId).OrderBy(ex => ex.UG_Name).ToList();
            }
        }

        /// <summary>
        /// To perform search
        /// </summary>
        public void PerformSearch()
        {
            if (ClientId == 0)
                View.ApplicantSearchData = new List<ApplicantDataList>();

            else
            {
                SearchItemDataContract searchDataContract = GetSearchContract();
                try
                {
                    View.ApplicantSearchData = ComplianceDataManager.GetApplicantPortfolioSearch(ClientId, searchDataContract, View.GridCustomPaging);
                    if (View.ApplicantSearchData.IsNotNull() && View.ApplicantSearchData.Count > 0)
                    {
                        if (View.ApplicantSearchData[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.ApplicantSearchData[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualRecordCount = 0;
                        View.CurrentPageIndex = 1;
                    }
                }
                catch (Exception e)
                {
                    View.ApplicantSearchData = null;
                    throw e;
                }
            }
        }

        public void GetAllOrganisationUserIds()
        {
            SearchItemDataContract searchDataContract = GetSearchContract();

            View.GridCustomPaging.PageSize = View.VirtualRecordCount;
            List<ApplicantDataList> lstApplicantDataList = ComplianceDataManager.GetApplicantPortfolioSearch(ClientId, searchDataContract, View.GridCustomPaging);

            if (!lstApplicantDataList.IsNullOrEmpty())
            {
                View.AssignOrganizationUserIds = new Dictionary<int, string>();

                foreach (ApplicantDataList item in lstApplicantDataList)
                {
                    if (!View.AssignOrganizationUserIds.ContainsKey(item.OrganizationUserId))
                    {
                        View.AssignOrganizationUserIds.Add(item.OrganizationUserId, item.ApplicantFirstName);
                        View.AssignOrganizationUsers.Add(new CustomComplianceContract { OrganizationUserID = item.OrganizationUserId, ApplicantName = item.ApplicantFirstName + " " + item.ApplicantLastName }); //UAT:4218
                    }
                }
            }
        }

        /// <summary>
        /// Getting Formatted SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetFormattedSSN(String unformattedSSN)
        {
            return ApplicationDataManager.GetFormattedSSN(unformattedSSN);
        }
        public void FetchSelectedSubscriptionIDs()
        {
            View.lstPackageSubscription = ComplianceDataManager.FetchSelectedSubscriptionIDs(ClientId, View.AssignOrganizationUserIds);
            View.ListSubscriptionIds = View.lstPackageSubscription.Select(x => x.PackageSubscriptionID).ToList();
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        public void GetGranularPermissionForDOBandSSN()
        {
            View.IsDOBDisable = false;
            View.SSNPermissionCode = EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue();
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (SecurityManager.GetUserGranularPermission(View.CurrentLoggedInUserId, out dicPermissions))
            {
                if (dicPermissions.ContainsKey(EnumSystemEntity.DOB.GetStringValue()) && dicPermissions[EnumSystemEntity.DOB.GetStringValue()].ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                {
                    View.IsDOBDisable = true;
                }
                if (dicPermissions.ContainsKey(EnumSystemEntity.SSN.GetStringValue()))
                {
                    View.SSNPermissionCode = dicPermissions[EnumSystemEntity.SSN.GetStringValue()];
                }

                //UAT-3010:-  Granular Permission for Client Admin Users to Archive.
                if (dicPermissions.ContainsKey(EnumSystemEntity.ARCHIVE_ABILITY.GetStringValue()))
                {
                    View.ArchivePermissionCode = dicPermissions[EnumSystemEntity.ARCHIVE_ABILITY.GetStringValue()];
                }
            }
        }

        /// <summary>
        /// Getting Masked SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetMaskedSSN(String unMaskedSSN)
        {
            return ApplicationDataManager.GetMaskedSSN(unMaskedSSN);
        }

        #endregion

        public String ArchieveSubscriptions()
        {
            return ComplianceDataManager.ArchieveSubscriptionsManually(View.ListSubscriptionIds, View.SelectedTenantId, View.CurrentLoggedInUserId);
        }
        #region UAT-977
        private List<Int32> GetArchiveStateId()
        {
            return new List<Int32>{
            ComplianceDataManager.GetArchiveStateList(View.SelectedTenantId).FirstOrDefault(x => x.AS_Code.Equals(View.SelectedArchiveStateCode.FirstOrDefault())).AS_ID};

        }

        /// <summary>
        /// Gets the list of Archive State.
        /// </summary>
        public void GetArchiveStateList()
        {
            View.lstArchiveState = ComplianceDataManager.GetArchiveStateList(View.SelectedTenantId);
        }

        private String GetXMLString(List<Int32> listOfIds)
        {
            if (listOfIds.IsNotNull() && listOfIds.Count > 0)
            {
                StringBuilder IdString = new StringBuilder();
                foreach (Int32 id in listOfIds)
                {
                    IdString.Append("<Root><Value>" + id.ToString() + "</Value></Root>");
                }

                return IdString.ToString();
            }
            return null;
        }

        #endregion


        public void GetSubscriptionsListForArchival()
        {
            //Dictionary<String, List<Int32>> dicSubscriptions = new Dictionary<String, List<Int32>>();
            View.DicSubscriptionIDs = ComplianceDataManager.GetSubscriptionsListForArchival(View.SelectedTenantId, View.AssignOrganizationUserIds);
        }

        private SearchItemDataContract GetSearchContract()
        {
            SearchItemDataContract searchDataContract = new INTSOF.UI.Contract.ComplianceManagement.SearchItemDataContract();
            searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
            searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
            if (View.OrganizationUserID > SysXDBConsts.NONE)
            {
                searchDataContract.OrganizationUserId = View.OrganizationUserID;
            }
            searchDataContract.EmailAddress = String.IsNullOrEmpty(View.EmailAddress) ? null : View.EmailAddress;
            //searchDataContract.ApplicantSSN = String.IsNullOrEmpty(View.SSN) ? null : View.SSN;
            searchDataContract.ApplicantSSN = ApplicationDataManager.GetSSNForFilters(View.SSN);
            searchDataContract.DateOfBirth = View.DateOfBirth;
            //UAT-1055: WB: As an admin, I should be able to search multiple nodes at a time.
            //if (View.DPM_ID > SysXDBConsts.NONE)
            //{
            //    searchDataContract.DPM_Id = View.DPM_ID;
            //}
            //if (View.NodeId > SysXDBConsts.NONE)
            //{
            //    searchDataContract.NodeId = View.NodeId;
            //}
            if (!View.DPM_IDs.IsNullOrEmpty())
            {
                searchDataContract.SelectedDPMIds = View.DPM_IDs;
            }
            searchDataContract.CustomFields = String.IsNullOrEmpty(View.CustomFields) ? null : View.CustomFields;
            if (View.MatchUserGroupId > SysXDBConsts.NONE)
            {
                searchDataContract.MatchUserGroupID = View.MatchUserGroupId;
                searchDataContract.FilterUserGroupID = View.FilterUserGroupId;
            }

            if (View.TenantId != SecurityManager.DefaultTenantID)
            {
                searchDataContract.LoggedInUserId = View.CurrentLoggedInUserId;
            }

            searchDataContract.LoggedInUserTenantId = View.TenantId;
            if (View.SelectedArchiveStateCode.IsNotNull())
            {
                searchDataContract.LstArchiveState = View.SelectedArchiveStateCode;
                searchDataContract.ArchieveStateId = GetXMLString(GetArchiveStateId());
            }
            if (View.ShowActiveOrdersOnly.IsNotNull())  //UAT-4273
            {
                searchDataContract.ShowActiveOrdersOnly = View.ShowActiveOrdersOnly;                
            }

            return searchDataContract;
        }

        #region UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.

        /// <summary>
        /// Add Web Application Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddWebApplicationData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.AddWebApplicationData(key, serializedData, 300);
        }

        /// <summary>
        /// Update Web Application Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdateWebApplicationData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.UpdateWebApplicationData(key, serializedData);
        }

        /// <summary>
        /// Get Data by Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<String, ApplicantInsituteDataContract> GetDataByKey(String key)
        {
            Object applicationData = ApplicationDataManager.GetObjectDataByKey(key);
            return ApplicationDataManager.DeserializeDictionaryValues<ApplicantInsituteDataContract>(applicationData);
        }

        /// <summary>
        /// Get Switching Target Url
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public String GetSwitchingTargetUrl(Int32 tenantID)
        {
            return WebSiteManager.GetInstitutionUrl(tenantID);
        }

        #endregion

        public void GetUserNodePermission()
        {
            if (View.SelectedTenantId > 0)
                View.lstUserNodePermissionsContract = ComplianceSetupManager.GetUserNodePermissionForVerificationAndProfile(View.SelectedTenantId, View.CurrentLoggedInUserId);
        }

        public void GetApplicantInstitutionHierarchyMapping()
        {
            if (View.SelectedTenantId > 0)
                View.lstApplicantInstitutionHierarchyMapping = StoredProcedureManagers.GetApplicantInstitutionHierarchyMapping(View.SelectedTenantId, View.OrganisationUserIds);
        }

        #region UAT-2422
        public void SetQueueImaging()
        {
            Dictionary<String, Object> dataDictionary = new Dictionary<String, Object>();
            dataDictionary.Add("TenantID", View.SelectedTenantId);
            QueueImagingManager.AsignQueueImagingRepoInstance(dataDictionary);
        }
        #endregion
    }
}




