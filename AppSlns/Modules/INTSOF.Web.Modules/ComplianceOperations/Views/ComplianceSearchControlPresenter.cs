using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Xml;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils.Consts;
using INTSOF.UI.Contract.ComplianceOperation;


namespace CoreWeb.ComplianceOperations.Views
{
    public class ComplianceSearchControlPresenter : Presenter<IComplianceSearchControlView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public SearchControlPresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            GetTenants();
            GetGranularPermissionForDOBandSSN();
            GetArchiveStateList();
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        public Boolean IsThirdPartyTenant
        {
            get
            {
                return SecurityManager.IsTenantThirdPartyType(View.TenantId, TenantType.Compliance_Reviewer.GetStringValue());
            }
        }

        public void GetTenants()
        {
            if (IsDefaultTenant)
            {

                View.lstTenant = ComplianceDataManager.getClientTenant();
            }
            else if (IsThirdPartyTenant)
            {
                View.lstTenant = ComplianceDataManager.getParentTenant(View.TenantId);
            }
            else
            {
                List<Tenant> lstTnt = new List<Tenant>();
                Entity.Tenant tnt = SecurityManager.GetTenant(View.TenantId);
                lstTnt.Add(new Tenant { TenantID = tnt.TenantID, TenantName = tnt.TenantName });
                View.lstTenant = lstTnt;
                View.SelectedTenantId = tnt.TenantID;
                GetArchiveStateList();
            }

        }

        public Int32 ClientId
        {
            get
            {
                if (IsDefaultTenant || IsThirdPartyTenant)
                    return View.SelectedTenantId;
                return View.TenantId;
            }
        }

        public void GetCompliancePackage()
        {
            if (ClientId == 0)
                View.lstCompliancePackage = new List<CompliancePackage>();
            else
                View.lstCompliancePackage = ComplianceSetupManager.GetCompliancePackage(ClientId, false).ToList();
        }

        //public void GetComplianceCategory()
        //{
        //    if (ClientId == 0)
        //        View.lstComplianceCategory = new List<ComplianceCategory>();
        //    else
        //        View.lstComplianceCategory = ComplianceSetupManager.GetcomplianceCategoriesByPackage(View.SelectedPackageId, ClientId).Select(x => x.ComplianceCategory).ToList();
        //}

        public void GetItemComplianceStatus()
        {
            if (ClientId == 0)
                View.lstItemComplianceStatus = new List<lkpItemComplianceStatu>();
            else
            {
                List<String> statusCode = new List<string>();
                //statusCode.Add(ApplicantItemComplianceStatus.Pending_Review.GetStringValue());
                //statusCode.Add(ApplicantItemComplianceStatus.Approved.GetStringValue());
                //statusCode.Add(ApplicantItemComplianceStatus.Not_Approved.GetStringValue());
                //statusCode.Add(ApplicantItemComplianceStatus.Expired.GetStringValue());

                View.lstItemComplianceStatus = ComplianceDataManager.GetItemComplianceStatus(ClientId).DistinctBy(x => x.Name).ToList();
            }
        }

        public void GetOverAllComplianceStatus()
        {
            if (ClientId == 0)
                View.lstOverAllComplianceStatus = new List<lkpPackageComplianceStatu>();
            else
            {
                View.lstOverAllComplianceStatus = ComplianceDataManager.GetOverAllComplianceStatus(ClientId).ToList();
            }
        }

        public void GetCategoryComplianceStatus()
        {
            if (ClientId == 0)
                View.lstCategoryComplianceStatus = new List<lkpCategoryComplianceStatu>();
            else
            {
                View.lstCategoryComplianceStatus = ComplianceDataManager.GetCategoryComplianceStatus(ClientId).ToList();
            }
        }

        public void GetAdminProgramStudy()
        {
            //Entity.Organization organization = SecurityManager.GetOrganizationForTenant(ClientId);
            //if (organization == null)
            //    View.lstAdminProgramStudy = new List<Entity.AdminProgramStudy>();
            //else
            //    View.lstAdminProgramStudy = SecurityManager.GetAllPrograms(organization.OrganizationID).ToList();

            //View.lstAdminProgramStudy = SecurityManager.GetAllProgramsForTenantID(ClientId).ToList();
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

        private List<Int32> GetSelectedOverAllComplianceStatusIds()
        {
            if (View.SelectedOverAllComplianceStatusId.Count == 0)
            {
                return null;
            }
            else
            {
                return View.SelectedOverAllComplianceStatusId;
            }
        }
        private List<Int32> GetSelectedCategoryComplianceStatusIds()
        {
            if (View.SelectedCategoryComplianceStatusId.Count == 0)
            {
                return null;
            }
            else
            {
                return View.SelectedCategoryComplianceStatusId;
            }
        }

        private List<Int32> GetArchiveStateId()
        {
            return new List<Int32>{
            ComplianceDataManager.GetArchiveStateList(View.SelectedTenantId).FirstOrDefault(x => x.AS_Code.Equals(View.SelectedArchiveStateCode.FirstOrDefault())).AS_ID};

        }

        private List<Int32> GetSelectedItemComplianceStatusIds()
        {
            List<Int32> statusIds = View.SelectedItemComplianceStatusId;
            List<lkpItemComplianceStatu> lstStatus = ComplianceDataManager.GetItemComplianceStatus(ClientId).ToList();
            if (lstStatus != null)
            {

                lkpItemComplianceStatu pendingReviewStatus = lstStatus.FirstOrDefault(x => x.Code.Equals(ApplicantItemComplianceStatus.Pending_Review.GetStringValue()));
                lkpItemComplianceStatu pendingReviewForClient = lstStatus.FirstOrDefault(x => x.Code.Equals(ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue()));
                lkpItemComplianceStatu pendingReviewForThirdParty = lstStatus.FirstOrDefault(x => x.Code.Equals(ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue()));

                if (View.SelectedItemComplianceStatusId.Count == 0)
                {
                    return null;
                }
                else if (View.SelectedItemComplianceStatusId.Contains(pendingReviewStatus.ItemComplianceStatusID))
                {
                    if (pendingReviewForClient != null)
                        statusIds.Add(pendingReviewForClient.ItemComplianceStatusID);

                    if (pendingReviewForThirdParty != null)
                        statusIds.Add(pendingReviewForThirdParty.ItemComplianceStatusID);
                }
            }
            return statusIds;

        }

        /// <summary>
        /// Gets the list of Archive State.
        /// </summary>
        public void GetArchiveStateList()
        {
            //UAT-4356 :  To show Subscription Archive State before page loads                
            if (View.SelectedTenantId ==AppConsts.NONE)
            {
                List<lkpArchiveState> lkpArchiveSatetList = new List<lkpArchiveState>();                
                lkpArchiveSatetList.Add(new lkpArchiveState() { AS_Name = ArchiveState.Active.ToString(), AS_Code = ArchiveState.Active.GetStringValue() });
                lkpArchiveSatetList.Add(new lkpArchiveState() { AS_Name = ArchiveState.Archived.ToString(), AS_Code = ArchiveState.Archived.GetStringValue() });
                lkpArchiveSatetList.Add(new lkpArchiveState() { AS_Name = ArchiveState.All.ToString(), AS_Code = ArchiveState.All.GetStringValue() });
                View.lstArchiveState = lkpArchiveSatetList;
            }
            //UAT-4356
            else
            {
                View.lstArchiveState = ComplianceDataManager.GetArchiveStateList(View.SelectedTenantId);
            }            
        }



        public void PerformSearch()
        {
            SearchItemDataContract searchDataContract = null;
            if (ClientId == 0)
                View.ItemData = new List<ComplianceRecord>();

            //else if (View.ActionType == ViewMode.Search.ToString())
            else
            {
                searchDataContract = GetSearchContract();
                //View.ItemDataGridCustomPaging.SortDirectionDescending = false;
                View.ItemData = ComplianceDataManager.GetComplianceRecordsSearch(searchDataContract, View.ItemDataGridCustomPaging);
                if (View.ItemData.Count > 0)
                {
                    View.ItemDataGridCustomPaging.VirtualPageCount = View.ItemData.FirstOrDefault().TotalCount;
                }
                else
                {
                    View.ItemDataGridCustomPaging.VirtualPageCount = 0;
                }
                View.VirtualPageCount = View.ItemDataGridCustomPaging.VirtualPageCount;
                View.CurrentPageIndex = View.ItemDataGridCustomPaging.CurrentPageIndex;
                searchDataContract.GridCustomPagingArguments = View.ItemDataGridCustomPaging;
                View.ViewStateSearchData = searchDataContract;
            }
            //Commented for UAT-1456 as user was getting exception while changing page size.
            //else
            //{
            //    if (View.ViewStateSearchData.IsNotNull())
            //    {
            //        searchDataContract = View.ViewStateSearchData;
            //    }
            //    else
            //    {
            //        searchDataContract = new SearchItemDataContract();
            //    }
            //    View.ItemData = ComplianceDataManager.GetComplianceRecordsSearch(View.ViewStateSearchData, View.ItemDataGridCustomPaging).ToList();

            //    if (View.ItemData.Count > 0)
            //    {
            //        View.ItemDataGridCustomPaging.VirtualPageCount = View.ItemData.FirstOrDefault().TotalCount;
            //    }
            //    else
            //    {
            //        View.ItemDataGridCustomPaging.VirtualPageCount = 0;
            //    }
            //    View.VirtualPageCount = View.ItemDataGridCustomPaging.VirtualPageCount;
            //    View.CurrentPageIndex = View.ItemDataGridCustomPaging.CurrentPageIndex;
            //    searchDataContract.GridCustomPagingArguments = View.ItemDataGridCustomPaging;
            //    //if (View.ViewStateSearchData.IsNotNull())
            //    //{
            //    //    View.ViewStateSearchData.GridCustomPagingArguments = searchDataContract.GridCustomPagingArguments;
            //    //}
            //    //else
            //    //{
            //        View.ViewStateSearchData = searchDataContract;
            //    //}
            //}

        }

        private SearchItemDataContract GetSearchContract()
        {
            SearchItemDataContract searchDataContract = new SearchItemDataContract();
            searchDataContract.ClientID = ClientId;
            if (View.TenantId != SecurityManager.DefaultTenantID)
            {
                searchDataContract.OrganizationUserId = View.CurrentLoggedInUserId;
            }
            //searchDataContract.PackageID = View.SelectedPackageId;
            //searchDataContract.CategoryID = View.SelectedCategoryId;

           // searchDataContract.DisallowApostropheConversion = true;

            searchDataContract.StatusID = GetSelectedItemComplianceStatusIds();
            searchDataContract.ItemStatusID = GetXMLString(searchDataContract.StatusID);
            searchDataContract.CategoryIDList = GetSelectedCategoryComplianceStatusIds();
            searchDataContract.CategoryStatusID = GetXMLString(searchDataContract.CategoryIDList);
            searchDataContract.OverAllIDList = GetSelectedOverAllComplianceStatusIds();
            searchDataContract.OverAllStatusID = GetXMLString(searchDataContract.OverAllIDList);
            if (!View.ApplicantFirstName.Equals(String.Empty))
            {
                searchDataContract.ApplicantFirstName = View.ApplicantFirstName;
            }
            if (!View.ApplicantLastName.Equals(String.Empty))
            {
                searchDataContract.ApplicantLastName = View.ApplicantLastName;
            }
            //if (!View.SSNnumber.Equals(String.Empty))
            //{
            //    searchDataContract.ApplicantSSN = View.SSNnumber;
            //}

            searchDataContract.ApplicantSSN = ApplicationDataManager.GetSSNForFilters(View.SSNnumber);

            if (!View.OrderID.Equals(0))
            {
                searchDataContract.OrderID = View.OrderID;
            }

            if (!View.OrderNumber.IsNullOrEmpty())
            {
                searchDataContract.OrderNumber = View.OrderNumber;
            }
            if (View.SelectedArchiveStateCode.IsNotNull())
            {
                searchDataContract.LstArchiveState = View.SelectedArchiveStateCode;
                searchDataContract.ArchieveStateId = GetXMLString(GetArchiveStateId());
            }

            #region UAT-3518
            if (View.SelectedExpiryStateCode.IsNotNull())
            {
                searchDataContract.SelectedExpiryStateCode = View.SelectedExpiryStateCode;
            }
            #endregion

            if (View.MatchUserGroupId > SysXDBConsts.NONE)
            {
                searchDataContract.MatchUserGroupID = View.MatchUserGroupId;
            }
            if (!View.UserGroupIds.IsNullOrEmpty())
            {
                searchDataContract.UserGroupIds = View.UserGroupIds;
            }
            searchDataContract.CustomFields = View.CustomDataXML;
            searchDataContract.NodeLabel = View.NodeLable;
            searchDataContract.DateOfBirth = View.DateOfBirth;
            //if (!View.DPM_Id.Equals(0))
            //{
            //    searchDataContract.DPM_Id = View.DPM_Id;
            //}

            searchDataContract.SelectedDPMIds = View.DPM_Ids;

            //if (!View.NodeId.Equals(0))
            //{
            searchDataContract.NodeIds = View.NodeIds;
            //}

            //UAT 2834
            if (!IsDefaultTenant && !IsThirdPartyTenant)
            {
                searchDataContract.IsClientAdminLoggedIn = true;
            }
            return searchDataContract;
        }

        public void GetAllOrganisationUserIds()
        {
            SearchItemDataContract searchDataContract = null;

            searchDataContract = GetSearchContract();
            View.ItemDataGridCustomPaging.SortDirectionDescending = false;
            View.ItemDataGridCustomPaging.PageSize = View.ItemDataGridCustomPaging.VirtualPageCount;
            List<ComplianceRecord> lstComplianceRecord = ComplianceDataManager.GetComplianceRecordsSearch(searchDataContract, View.ItemDataGridCustomPaging);

            if (!lstComplianceRecord.IsNullOrEmpty())
            {
                View.AssignOrganizationUserIds = new Dictionary<int, string>();
                View.ListSubscriptionIds = new Dictionary<int, int>();

                foreach (ComplianceRecord item in lstComplianceRecord)
                {
                    if (!View.AssignOrganizationUserIds.ContainsKey(item.ApplicantId))
                        View.AssignOrganizationUserIds.Add(item.ApplicantId, item.ApplicantFirstName);

                    if (!View.ListSubscriptionIds.ContainsKey(item.PackageSubscriptionID.HasValue ? item.PackageSubscriptionID.Value : 0))
                        View.ListSubscriptionIds.Add(item.PackageSubscriptionID.HasValue ? item.PackageSubscriptionID.Value : 0, item.ApplicantId);
                }
            }
        }

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
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
                Int32? currentUserId = GetTenantId() == Business.RepoManagers.SecurityManager.DefaultTenantID ? (Int32?)null : View.CurrentLoggedInUserId;
                View.lstUserGroup = ComplianceSetupManager.GetAllUserGroupWithPermission(ClientId, currentUserId).OrderBy(ex => ex.UG_Name).ToList();
            }
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
            List<Int32> lstsubscriptionIds = new List<Int32>();
            foreach (var x in View.ListSubscriptionIds)
            {
                lstsubscriptionIds.Add(x.Key);
            }
            return ComplianceDataManager.ArchieveSubscriptionsManually(lstsubscriptionIds, View.SelectedTenantId, View.CurrentLoggedInUserId);

        }

        public Boolean UnArchiveSubscription()
        {
            List<Int32> lstsubscriptionIds = new List<Int32>();
            foreach (var x in View.ListSubscriptionIds)
            {
                lstsubscriptionIds.Add(x.Key);
            }
            List<Int32> pkgSubArchiveHistoryIds = ComplianceDataManager.GetPkgSubArchiveHistoryIds(lstsubscriptionIds, View.SelectedTenantId);
            return ComplianceDataManager.ApproveUnArchivalRequests(View.SelectedTenantId, pkgSubArchiveHistoryIds, View.CurrentLoggedInUserId, SubscriptionType.ARCHIVED_SUBSCRIPTIONS.GetStringValue(), ArchivePackageType.Tracking.GetStringValue());
        }
        public Boolean GetUserVerificationPermission(Int32 packageSubscriptionId)
        {
            Boolean IsFullPermissionForVerification = true;
            if (View.TenantId != SecurityManager.DefaultTenantID)
            {
                PackageSubscription packageSubscription = ComplianceDataManager.GetPackageSubscriptionByID(View.SelectedTenantId, packageSubscriptionId);
                if (packageSubscription.IsNotNull() && packageSubscription.Order.IsNotNull())
                {
                    List<UserNodePermissionsContract> lstUserNodePermission = ComplianceSetupManager.GetUserNodePermissionForVerificationAndProfile(View.TenantId, View.CurrentLoggedInUserId).ToList();
                    UserNodePermissionsContract userNodePermission = lstUserNodePermission.FirstOrDefault(cond => cond.DPM_ID == packageSubscription.Order.SelectedNodeID);
                    if (userNodePermission.IsNotNull() && userNodePermission.VerificationPermissionCode == LkpPermission.ReadOnly.GetStringValue())
                    {
                        IsFullPermissionForVerification = false;
                    }
                }
            }
            return IsFullPermissionForVerification;
        }

        #region UAT-2422
        public void SetQueueImaging()
        {
            Dictionary<String, Object> dataDictionary = new Dictionary<String, Object>();
            dataDictionary.Add("TenantID", View.SelectedTenantId);
            QueueImagingManager.AsignQueueImagingRepoInstance(dataDictionary);
        }
        #endregion

        #region UAT-2675
        public List<String> GetScreenColumnsToHide(String GrdCode, Int32 CurrentLoggedInUserId)
        {
            return SecurityManager.GetScreenColumnsToHide(GrdCode, CurrentLoggedInUserId);
        }
        #endregion

        #region UAT-4067
        public void GetSelectedNodeIDBySubscriptionID(Int32 selectedtenantID, Int32 packageSubscriptionID)
        {
            var lstSelectedNodeIDForOrders = ComplianceDataManager.GetSelectedNodeIDBySubscriptionID(selectedtenantID, packageSubscriptionID);
            View.selectedNodeIDs = lstSelectedNodeIDForOrders.Where(x => !x.IsDeleted).DistinctBy(x => x.Order.SelectedNodeID).Select(x => x.Order.SelectedNodeID ?? 0).ToList();
        }
        public void GetAllowedFileExtensions()
        {
            String selectedNodeIDs = String.Join(",", View.selectedNodeIDs);
            var lstAllowedFileExtensions = ComplianceDataManager.GetAllowedFileExtensionsByNodeIDs(View.SelectedTenantId, selectedNodeIDs);
            View.allowedFileExtensions = lstAllowedFileExtensions.Select(x => x.Name).ToList();
        }
        #endregion
        
    }
}




