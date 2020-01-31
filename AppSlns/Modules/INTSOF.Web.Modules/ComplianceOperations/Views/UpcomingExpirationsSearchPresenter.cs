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
    public class UpcomingExpirationsSearchPresenter : Presenter<IUpcomingExpirationsSearchView>
    {
        public override void OnViewLoaded()
        {
            GetTenants();
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantID;
            }
        }

        public Boolean IsThirdPartyTenant
        {
            get
            {
                return SecurityManager.IsTenantThirdPartyType(View.TenantID, TenantType.Compliance_Reviewer.GetStringValue());
            }
        }

        public Int32 ClientId
        {
            get
            {
                if (IsDefaultTenant || IsThirdPartyTenant)
                    return View.SelectedTenantId;
                return View.TenantID;
            }
        }

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }
        public void GetTenants()
        {
            if (IsDefaultTenant)
            {
                View.lstTenant = ComplianceDataManager.getClientTenant();
            }
            else if (IsThirdPartyTenant)
            {
                View.lstTenant = ComplianceDataManager.getParentTenant(View.TenantID);
            }
            else
            {
                List<Tenant> lstTnt = new List<Tenant>();
                Entity.Tenant tnt = SecurityManager.GetTenant(View.TenantID);
                lstTnt.Add(new Tenant { TenantID = tnt.TenantID, TenantName = tnt.TenantName });
                View.lstTenant = lstTnt;
            }
        }

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

        public void GetComplianceCategory()
        {
            if (ClientId == 0)
                View.lstComplianceCategory = new List<ComplianceCategory>();
            // [SS]:[14/07/2017]:Revert back changes of UAT-2834
            //else if (IsDefaultTenant || IsThirdPartyTenant)
            //{
            //    View.lstComplianceCategory = ComplianceSetupManager.GetPermittedCategoriesByUserID(View.SelectedTenantId,null); //UAT 2834
            //}
            else
            {
                //List<CompliancePackageCategory> temp = ComplianceSetupManager.GetcomplianceCategoriesByPackage(View.SelectedPackageId, ClientId, false).OrderBy(x => x.CPC_DisplayOrder).ToList();
                // View.lstComplianceCategory = temp.Select(x => x.ComplianceCategory).ToList();

                // [SS]:[14/07/2017]:Revert back changes of UAT-2834
                //View.lstComplianceCategory = ComplianceSetupManager.GetPermittedCategoriesByUserID(View.SelectedTenantId, View.CurrentLoggedInUserId); //UAT 2834

                View.lstComplianceCategory = ComplianceSetupManager.GetComplianceCategories(View.SelectedTenantId, false).OrderBy(x => x.CategoryName).ToList();// UAT sort dropdowns by Name;
            }
        }

        public void GetComplianceItem()
        {
            List<ComplianceItem> tempItemList = new List<ComplianceItem>();
            if (ClientId == 0)
            {
                tempItemList.Insert(0, new ComplianceItem { Name = "--Select--", ComplianceItemID = 0 });
                View.lstComplianceItem = tempItemList;
            }
            else
            {
                Entity.SystemEventSetting _systemEventSetting;
                tempItemList = ComplianceSetupManager.GetComplianceItemsByCategoryIds(View.SelectedCategoryIds, View.SelectedTenantId, out _systemEventSetting, 0).OrderBy(x => x.DisplayOrder).ToList(); //.Select(x => x.ComplianceCategory).ToList();
                tempItemList.Insert(0, new ComplianceItem { Name = "--Select--", ComplianceItemID = 0 });
                View.lstComplianceItem = tempItemList;
            }
        }

        public void GetUpcomingExpirations()
        {
            if (ClientId == 0)
                View.lstUpcomingExpirations = new List<UpcomingExpirationContract>();
            else
            {
                Boolean IsClientAdminLoggedIn = (IsDefaultTenant || IsThirdPartyTenant) ? false : true;
                View.lstUpcomingExpirations = ComplianceDataManager.GetUpcomingExpiration(View.SelectedTenantId, View.HierarchyIds, View.Categories, View.Items, View.DateFrom, View.DateTo, View.UserGroups, View.customPagingArgsContract, IsClientAdminLoggedIn, View.CurrentLoggedInUserId);
                if (View.lstUpcomingExpirations.Count > 0)
                {
                    View.customPagingArgsContract.VirtualPageCount = View.lstUpcomingExpirations.FirstOrDefault().TotalCount;
                }
                else
                {
                    View.customPagingArgsContract.VirtualPageCount = 0;
                }
                View.VirtualPageCount = View.customPagingArgsContract.VirtualPageCount;
                View.CurrentPageIndex = View.customPagingArgsContract.CurrentPageIndex;
            }

        }

    }
}
