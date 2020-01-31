using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using Entity;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ApplicantDataAuditHistoryPresenter : Presenter<IApplicantDataAuditHistoryView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public ApplicantDataAuditHistoryPresenter([CreateNew] IComplianceOperationsController controller)
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
        }

        #region Properties
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

        public Boolean IsClientAdminReset { get; set; } // UAT-4371

        #endregion

        #region Methods

        #region Private Methods
        #endregion

        #region Public Methods

        /// <summary>
        /// To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            List<Entity.Tenant> lstTemp = SecurityManager.GetTenants(SortByName, false, clientCode);
            lstTemp.Insert(0, new Entity.Tenant { TenantID = 0, TenantName = "--Select--" });
            View.lstTenant = lstTemp;
        }

        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }
        /// <summary>
        /// To get Admin Program Study
        /// </summary>
        public void GetAllUserGroups()
        {
            List<Entity.ClientEntity.UserGroup> tempList = new List<Entity.ClientEntity.UserGroup>();
            if (ClientId != AppConsts.NONE)
            {
                tempList = ComplianceSetupManager.GetAllUserGroup(ClientId).OrderBy(x => x.UG_Name).ToList(); //UAT- sort dropdowns by Name
                tempList.Insert(0, new Entity.ClientEntity.UserGroup { UG_ID = 0, UG_Name = "--Select--" });
                View.lstUserGroup = tempList;
            }
            else
            {
                tempList.Insert(0, new Entity.ClientEntity.UserGroup { UG_ID = 0, UG_Name = "--Select--" });
                View.lstUserGroup = tempList;
            }
        }

        public void GetCompliancePackage(Int32? orgUserId = null)
        {
            List<ComplaincePackageDetails> tempPackageList = new List<ComplaincePackageDetails>();
            if (ClientId != AppConsts.NONE)
            {
                //UAT-397 As an admin, I should not see packages where my permission is "No Access" in package dropdowns
                //tempPackageList = ComplianceSetupManager.GetCompliancePackage(ClientId, false).ToList();
                tempPackageList = ComplianceSetupManager.GetPermittedPackagesByUserID(ClientId, orgUserId).OrderBy(x => x.PackageName).ToList(); //UAT- sort dropdowns by Name
                                                                                                                                                 // tempPackageList.Insert(0, new ComplaincePackageDetails { PackageName = "--Select--", CompliancePackageID = 0 });
                View.lstCompliancePackage = tempPackageList;

            }
            else
            {
                // tempPackageList.Insert(0, new ComplaincePackageDetails { PackageName = "--Select--", CompliancePackageID = 0 });
                View.lstCompliancePackage = tempPackageList;
            }
        }


        public void GetComplianceCategory()
        {
            List<Entity.ClientEntity.ComplianceCategory> tempCategoryList = new List<Entity.ClientEntity.ComplianceCategory>();
            if (ClientId != AppConsts.NONE)
            {
                //UAT-2069 - Passing multiple packageIds .
                tempCategoryList = ComplianceSetupManager.GetcomplianceCategoriesByPackageIds(View.SelectedPackageIds, ClientId, false).OrderBy(x => x.CPC_DisplayOrder).Select(x => x.ComplianceCategory).ToList();
                // tempCategoryList.Insert(0, new Entity.ClientEntity.ComplianceCategory { CategoryName = "--Select--", ComplianceCategoryID = 0 });
                View.lstComplianceCategory = tempCategoryList.DistinctBy(t => t.ComplianceCategoryID).OrderBy(x => x.CategoryName).ToList();
            }
            else
            {
                // tempCategoryList.Insert(0, new Entity.ClientEntity.ComplianceCategory { CategoryName = "--Select--", ComplianceCategoryID = 0 });
                View.lstComplianceCategory = tempCategoryList;
            }
        }

        /// <summary>
        /// Get the Applicant Data audit History
        /// </summary>
        public void GetApplicantDataAuditHistory()
        {
            try
            {
                if (ClientId != 0 && ClientId.IsNotNull() && !IsClientAdminReset)
                {
                    SearchItemDataContract searchDataContract = new SearchItemDataContract();

                    searchDataContract.DisallowApostropheConversion = true;
                    searchDataContract.RoleNames = View.selectedFltrRoleNames.IsNullOrEmpty() ? null : View.selectedFltrRoleNames; // UAT - 4107
                    searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.FirstName) ? null : View.FirstName;
                    searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.LastName) ? null : View.LastName;

                    #region UAT-950
                    searchDataContract.AdminFirstName = String.IsNullOrEmpty(View.AdminFirstName) ? null : View.AdminFirstName;
                    searchDataContract.AdminLastName = String.IsNullOrEmpty(View.AdminLastName) ? null : View.AdminLastName;
                    #endregion

                    if (!IsDefaultTenant && View.CurrentLoggedInUserId != AppConsts.NONE)
                    {
                        searchDataContract.LoggedInUserId = View.CurrentLoggedInUserId;
                    }
                    searchDataContract.LoggedInUserTenantId = View.TenantId;
                    if (View.SelectedUserGroupId != AppConsts.NONE && View.SelectedUserGroupId.IsNotNull())
                    {
                        searchDataContract.FilterUserGroupID = View.SelectedUserGroupId;
                    }
                    //UAT-2069 - Passing more than one package or category id to search filter
                    if (!View.SelectedCategoryIds.IsNullOrEmpty())
                    {
                        searchDataContract.CategoryIDs = String.Join(",", View.SelectedCategoryIds);
                    }

                    if (!View.SelectedPackageIds.IsNullOrEmpty())
                    {
                        searchDataContract.PackageIDs = String.Join(",", View.SelectedPackageIds);
                    }
                    searchDataContract.ItemID = View.SelectedItemID;
                    if (!View.TimeStampFromDate.IsNullOrEmpty() && View.TimeStampFromDate != DateTime.MinValue)
                    {
                        searchDataContract.FromDate = View.TimeStampFromDate;
                    }
                    else
                    {
                        searchDataContract.FromDate = null;
                    }
                    if (!View.TimeStampToDate.IsNullOrEmpty() && View.TimeStampToDate != DateTime.MinValue)
                    {
                        searchDataContract.ToDate = View.TimeStampToDate;
                    }
                    else
                    {
                        searchDataContract.ToDate = null;
                    }
                    View.ApplicantDataAuditHistoryList = ComplianceSetupManager.GetApplicantDataAuditHistory(ClientId, View.GridCustomPaging, searchDataContract);
                    if (View.ApplicantDataAuditHistoryList.IsNotNull() && View.ApplicantDataAuditHistoryList.Count > 0)
                    {
                        if (View.ApplicantDataAuditHistoryList[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.ApplicantDataAuditHistoryList[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualRecordCount = 0;
                        View.CurrentPageIndex = 1;
                    }
                }
                else
                {
                    View.ApplicantDataAuditHistoryList = new List<ApplicantDataAuditHistory>();
                }
            }
            catch (Exception e)
            {
                View.ApplicantDataAuditHistoryList = new List<ApplicantDataAuditHistory>();
                throw e;
            }
        }

        #endregion

        #endregion

        public void GetComplianceItem()
        {
            List<Entity.ClientEntity.ComplianceItem> tempItemList = new List<Entity.ClientEntity.ComplianceItem>();
            if (ClientId != AppConsts.NONE && !View.SelectedCategoryIds.IsNullOrEmpty())
            {
                SystemEventSetting _systemEventSetting;
                tempItemList = ComplianceSetupManager.GetComplianceItemsByCategoryIds(View.SelectedCategoryIds, View.SelectedTenantId, out _systemEventSetting, 0).OrderBy(x => x.DisplayOrder).ToList(); //.Select(x => x.ComplianceCategory).ToList();
                tempItemList.Insert(0, new Entity.ClientEntity.ComplianceItem { Name = "--Select--", ComplianceItemID = 0 });
                View.lstComplianceItems = tempItemList;
            }
            else
            {
                tempItemList.Insert(0, new Entity.ClientEntity.ComplianceItem { Name = "--Select--", ComplianceItemID = 0 });
                View.lstComplianceItems = tempItemList;
            }
        }
    }
}




