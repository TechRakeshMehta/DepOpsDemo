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
using INTSOF.UI.Contract.ComplianceOperation;


namespace CoreWeb.ComplianceOperations.Views
{
    public class SearchControlPresenter : Presenter<ISearchControlView>
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

        public Boolean IsAdminLoggedIn()
        {
            //View.SelectedTenant = GetTenantId();

            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
                return true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
                return false;
            }
        }

        public void GetTenants()
        {
            if (IsThirdPartyTenant)
            {
                View.lstTenant = ComplianceDataManager.getParentTenant(View.TenantId);
            }
            else
            {
                View.lstTenant = ComplianceDataManager.getClientTenant();
                //GetArchiveStateList();
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
            //UAT-397 As an admin, I should not see packages where my permission is "No Access" in package dropdowns
            Int32? orgUserId = null;
            if (!IsDefaultTenant)
                orgUserId = View.CurrentLoggedInUserId;

            if (ClientId == 0)
            {
                View.lstCompliancePackage = new List<ComplaincePackageDetails>();
                View.lstCompliancePackage2 = new List<Entity.ClientEntity.CompliancePackage>();
            }
            else
            {
                //UAT-397 As an admin, I should not see packages where my permission is "No Access" in package dropdowns
                //View.lstCompliancePackage = ComplianceSetupManager.GetCompliancePackage(ClientId, false).ToList();
                View.lstCompliancePackage = ComplianceSetupManager.GetPermittedPackagesByUserID(ClientId, orgUserId)
                                                                       .OrderBy(x => x.PackageName).ToList();//UAT sort dropdowns by Name;

                //UAT-4056 Category Search filtering updates
                View.lstCompliancePackage2 = ComplianceSetupManager.GetCompliancePackagesByPermission(View.DPM_Ids, View.CurrentLoggedInUserId, View.SelectedTenantId, View.IsAdminLoggedIn);
            }

        }

        public void GetComplianceCategory()
        {
            List<Entity.ClientEntity.ComplianceCategory> tempCategoryList = new List<Entity.ClientEntity.ComplianceCategory>();
            if (ClientId == 0)
                View.lstComplianceCategory = new List<ComplianceCategory>();
            else
            {
                //UAT-3519
                //List<CompliancePackageCategory> temp = ComplianceSetupManager.GetcomplianceCategoriesByPackage(View.SelectedPackageId, ClientId, false).OrderBy(x => x.CPC_DisplayOrder).ToList();
                //View.lstComplianceCategory = temp.Select(x => x.ComplianceCategory).ToList();
                // View.lstComplianceCategory = ComplianceSetupManager.GetcomplianceCategoriesByPackage(View.SelectedPackageId, ClientId, false).OrderBy(x => x.CPC_DisplayOrder).Select(x => x.ComplianceCategory).ToList();// UAT sort dropdowns by Name;

                //UAT-4136
                List<Int32> SelectedPackageIds = new List<Int32>();
                if (!View.SelectedPkgIds.IsNullOrEmpty() && View.SelectedPkgIds.Count() > AppConsts.NONE)
                {
                    foreach (string x in View.SelectedPkgIds.Split(','))
                    {
                        Int32 pkgId = 0;

                        if (!x.IsNullOrEmpty() && x != "")
                            pkgId = Convert.ToInt32(x);

                        if (!SelectedPackageIds.Contains(pkgId))
                            SelectedPackageIds.Add(pkgId);
                    }
                    View.SelectedPackageIds = SelectedPackageIds;
                }
                //END UAT-4136
                //tempCategoryList = ComplianceSetupManager.GetcomplianceCategoriesByPackageIds(View.SelectedPackageIds, ClientId, false).OrderBy(x => x.CPC_DisplayOrder).Select(x => x.ComplianceCategory).ToList();
                //View.lstComplianceCategory = tempCategoryList.DistinctBy(t => t.ComplianceCategoryID).OrderBy(x => x.CategoryName).ToList();
                View.lstComplianceCategory = ComplianceSetupManager.GetComplianceCategoriesByPermission(View.SelectedPackageIds, View.DPM_Ids, View.CurrentLoggedInUserId, View.SelectedTenantId,View.IsAdminLoggedIn);
            }
        }

        public void GetItemComplianceStatus()
        {
            if (ClientId == 0)
                View.lstItemComplianceStatus = new List<lkpItemComplianceStatu>();
            else
            {
                List<String> statusCode = new List<string>();
                statusCode.Add(ApplicantItemComplianceStatus.Pending_Review.GetStringValue());
                statusCode.Add(ApplicantItemComplianceStatus.Approved.GetStringValue());
                statusCode.Add(ApplicantItemComplianceStatus.Not_Approved.GetStringValue());
                statusCode.Add(ApplicantItemComplianceStatus.Expired.GetStringValue());
                //UAT-242: WB: Item Data Search: Add "Approved With Exception" and "Incomplete" checkbox to Status search criteria
                statusCode.Add(ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue());
                statusCode.Add(ApplicantItemComplianceStatus.Incomplete.GetStringValue());
                statusCode.Add(ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue());

                View.lstItemComplianceStatus = ComplianceDataManager.GetItemComplianceStatus(ClientId).Where(x => statusCode.Contains(x.Code)).ToList();
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
        private List<Int32> GetSelectedItemComplianceStatusIds()
        {
            List<Int32> statusIds = View.SelectedItemComplianceStatusId;
            List<String> statusCode = new List<string>();
            statusCode.Add(ApplicantItemComplianceStatus.Pending_Review.GetStringValue());
            statusCode.Add(ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue());
            statusCode.Add(ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue());
            statusCode.Add(ApplicantItemComplianceStatus.Approved.GetStringValue());
            statusCode.Add(ApplicantItemComplianceStatus.Not_Approved.GetStringValue());
            statusCode.Add(ApplicantItemComplianceStatus.Expired.GetStringValue());
            //UAT-242: WB: Item Data Search: Add "Approved With Exception" and "Incomplete" checkbox to Status search criteria
            statusCode.Add(ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue());
            statusCode.Add(ApplicantItemComplianceStatus.Incomplete.GetStringValue());
            statusCode.Add(ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue());

            List<lkpItemComplianceStatu> lstStatus = ComplianceDataManager.GetItemComplianceStatus(ClientId).Where(x => statusCode.Contains(x.Code)).ToList();
            if (lstStatus != null)
            {

                lkpItemComplianceStatu pendingReviewStatus = lstStatus.FirstOrDefault(x => x.Code.Equals(ApplicantItemComplianceStatus.Pending_Review.GetStringValue()));
                lkpItemComplianceStatu pendingReviewForClient = lstStatus.FirstOrDefault(x => x.Code.Equals(ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue()));
                lkpItemComplianceStatu pendingReviewForThirdParty = lstStatus.FirstOrDefault(x => x.Code.Equals(ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue()));

                if (View.SelectedItemComplianceStatusId.Count == 0)
                    return ComplianceDataManager.GetItemComplianceStatus(ClientId).Where(x => statusCode.Contains(x.Code)).Select(x => x.ItemComplianceStatusID).ToList();
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

        public void PerformSearch()
        {
            String _originalApplicantFirstName = string.Empty;
            String _originalApplicantLastName = string.Empty;
            if (ClientId == 0)
                View.ItemData = new List<ItemDataSearchContract>();

            else if (View.ActionType == ViewMode.Search.ToString())
            {
                INTSOF.UI.Contract.ComplianceManagement.SearchItemDataContract searchDataContract = GetSearchContract();

                _originalApplicantFirstName = View.ApplicantFirstName;
                _originalApplicantLastName = View.ApplicantLastName;

                View.ItemDataGridCustomPaging.DefaultSortExpression = "PackageID";
                View.ItemDataGridCustomPaging.SecondarySortExpression = QueueConstants.DEFAULT_SORTING_FIELDS_ASSIGNMENT;

                View.ItemData = ComplianceDataManager.GetItemDataSearchData(searchDataContract, View.ItemDataGridCustomPaging, View.DPM_Ids);//UAt-1055 //View.DPM_Id);
                //else
                //{
                //    var ItemDataQueueRestricted = ComplianceDataManager.PerformSearch<vwComplianceItemDataQueueRestricted>(searchDataContract, View.ItemDataGridCustomPaging).ToList();
                //    View.ItemData = ItemDataQueueRestricted.Select(x => new vwComplainceItemDataDetail
                //    {
                //        CategoryID = x.CategoryID,
                //        PackageSubscriptionID = x.PackageSubscriptionID,
                //        ApplicantName = x.ApplicantName,
                //        ItemName = x.ItemName,
                //        CategoryName = x.CategoryName,
                //        PackageName = x.PackageName,
                //        SubmissionDate = x.SubmissionDate,
                //        VerificationStatus = x.VerificationStatus,
                //        SystemStatus = x.SystemStatus,
                //        AssignedUserName = x.AssignedUserName,
                //        CustomAttributes=x.CustomAttributes

                //    }).ToList();
                //}

                View.ViewStateSearchData = searchDataContract;
                View.ViewStateSearchData.ApplicantFirstName = _originalApplicantFirstName;
                View.ViewStateSearchData.ApplicantLastName = _originalApplicantLastName;
                View.VirtualPageCount = View.ItemDataGridCustomPaging.VirtualPageCount;
                View.CurrentPageIndex = View.ItemDataGridCustomPaging.CurrentPageIndex;
            }
            else
            {
                View.ItemDataGridCustomPaging.DefaultSortExpression = "PackageID";
                View.ItemDataGridCustomPaging.SecondarySortExpression = QueueConstants.DEFAULT_SORTING_FIELDS_ASSIGNMENT;
                if (View.TenantId != SecurityManager.DefaultTenantID)
                {
                    View.ViewStateSearchData.IsRestricted = true;
                    //View.ItemData = ComplianceDataManager.PerformSearch<vwComplainceItemDataDetail>(searchDataContract, View.ItemDataGridCustomPaging).ToList();
                }
                View.ItemData = ComplianceDataManager.GetItemDataSearchData(View.ViewStateSearchData, View.ItemDataGridCustomPaging, View.DPM_Ids);//UAt-1055 //View.DPM_Id);
                //if (View.TenantId == SecurityManager.DefaultTenantID)
                //{
                //    View.ItemData = ComplianceDataManager.PerformSearch<vwComplainceItemDataDetail>(View.ViewStateSearchData, View.ItemDataGridCustomPaging).ToList();
                //}
                //else
                //{
                //    var ItemDataQueueRestricted = ComplianceDataManager.PerformSearch<vwComplianceItemDataQueueRestricted>(View.ViewStateSearchData, View.ItemDataGridCustomPaging).ToList();
                //    View.ItemData = ItemDataQueueRestricted.Select(x => new vwComplainceItemDataDetail
                //    {
                //        CategoryID = x.CategoryID,
                //        PackageSubscriptionID = x.PackageSubscriptionID,
                //        ApplicantName = x.ApplicantName,
                //        ItemName = x.ItemName,
                //        CategoryName = x.CategoryName,
                //        PackageName = x.PackageName,
                //        SubmissionDate = x.SubmissionDate,
                //        VerificationStatus = x.VerificationStatus,
                //        SystemStatus = x.SystemStatus,
                //        AssignedUserName = x.AssignedUserName,
                //        CustomAttributes=x.CustomAttributes

                //    }).ToList();
                //}
                View.VirtualPageCount = View.ItemDataGridCustomPaging.VirtualPageCount;
                View.CurrentPageIndex = View.ItemDataGridCustomPaging.CurrentPageIndex;
            }
        }

        public void GetAllOrganisationUserIds()
        {
            SearchItemDataContract searchDataContract = null;

            searchDataContract = GetSearchContract();

            View.ItemDataGridCustomPaging.DefaultSortExpression = "PackageID";
            View.ItemDataGridCustomPaging.SecondarySortExpression = QueueConstants.DEFAULT_SORTING_FIELDS_ASSIGNMENT;

            View.ItemDataGridCustomPaging.PageSize = View.VirtualPageCount;

            List<ItemDataSearchContract> lstItemDataSearchContract = ComplianceDataManager.GetItemDataSearchData(searchDataContract, View.ItemDataGridCustomPaging, View.DPM_Ids);

            if (!lstItemDataSearchContract.IsNullOrEmpty())
            {
                View.AssignOrganizationUserIds = new Dictionary<int, string>();

                foreach (ItemDataSearchContract item in lstItemDataSearchContract)
                {
                    if (!View.AssignOrganizationUserIds.ContainsKey(item.ApplicantId))
                        View.AssignOrganizationUserIds.Add(item.ApplicantId, item.ApplicantName);
                }
            }
        }

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
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
        // TODO: Handle other view events and set state in the view

        /// <summary>
        /// Gets the list of Archive State.
        /// </summary>
        public void GetArchiveStateList()
        {
            View.lstArchiveState = ComplianceDataManager.GetArchiveStateList(View.SelectedTenantId);
        }

        private String GetArchiveStateId()
        {
            return ComplianceDataManager.GetArchiveStateList(View.SelectedTenantId).FirstOrDefault(x => x.AS_Code.Equals(View.SelectedArchiveStateCode.FirstOrDefault())).AS_ID.ToString();
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

        private SearchItemDataContract GetSearchContract()
        {
            INTSOF.UI.Contract.ComplianceManagement.SearchItemDataContract searchDataContract = new INTSOF.UI.Contract.ComplianceManagement.SearchItemDataContract();
            searchDataContract.ClientID = ClientId;
            searchDataContract.PackageID = String.IsNullOrEmpty(View.SelectedPkgIds) ? null : View.SelectedPkgIds; //UAT-4136
            // searchDataContract.PackageID = View.SelectedPackageId; //Commented for UAT-4136
            searchDataContract.CategoryIDs = View.SelectedCategoryIds;

            searchDataContract.StatusID = GetSelectedItemComplianceStatusIds();
            searchDataContract.StatusIDForSearch = GetSelectedItemComplianceStatusIds().Select(x => new StatusIDClass
            {
                statusID = x
            }).ToList();

            searchDataContract.ApplicantFirstName = View.ApplicantFirstName;
            searchDataContract.ApplicantLastName = View.ApplicantLastName;
            searchDataContract.ProgramID = View.SelectedProgramStudyId;
            searchDataContract.DateOfBirth = View.DateOfBirth;
            searchDataContract.ItemLabel = View.ItemLabel;
            searchDataContract.AssignedToUserID = View.AssignedToUserId;
            searchDataContract.LoggedInUserId = View.CurrentLoggedInUserId;
            searchDataContract.SelectedUserGroupIDs = View.SelectedUserGroupIDs;
            if (View.TenantId != SecurityManager.DefaultTenantID)
            {
                searchDataContract.IsRestricted = true;
                //ComplianceDataManager.PerformSearch<vwComplainceItemDataDetail>(searchDataContract, View.ItemDataGridCustomPaging).ToList();
            }
            searchDataContract.CustomFields = View.CustomDataXML;

            #region UAT-422
            if (View.SelectedArchiveStateCode.IsNotNull())
            {
                searchDataContract.LstArchiveState = View.SelectedArchiveStateCode;
                searchDataContract.ArchieveStateIDForItemSearch = GetArchiveStateId();
            }
            #endregion

            #region UAT-3518
            if (View.SelectedExpiryStateCode.IsNotNull())
            {
                searchDataContract.SelectedExpiryStateCode = View.SelectedExpiryStateCode;        
            }
            #endregion

            searchDataContract.DisallowApostropheConversion = true;
            searchDataContract.IsClientAdminLoggedIn = (IsDefaultTenant || IsThirdPartyTenant) ? false : true; //UAT 2834 

            return searchDataContract;
        }

        /// <summary>
        /// To get Admin Program Study UAT-1681 and 1686
        /// </summary>
        public void GetAllUserGroups()
        {
            if (ClientId == 0)
                View.lstUserGroup = new List<UserGroup>();
            else
            {
                //UAT-2284: User Group permisson/access and availability by node
                Int32? currentUserId = View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID ? (Int32?)null : View.CurrentLoggedInUserId;
                View.lstUserGroup = ComplianceSetupManager.GetAllUserGroupWithPermission(ClientId, currentUserId).OrderBy(ex => ex.UG_Name).ToList();
            }
        }
    }
}




