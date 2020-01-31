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
using INTSOF.UI.Contract.SearchUI;
using INTSOF.Utils.Consts;

namespace CoreWeb.Search.Views
{
    public class ComplianceDocumentSearchPresenter : Presenter<IComplianceDocumentSearchView>
    {

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetTenants();
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
        /// To get User Groups
        /// </summary>
        public void GetAllUserGroups()
        {
            if (ClientId == 0)
                View.lstUserGroup = new List<UserGroup>();
            else
            {
                //UAT-2284: User Group permisson/access and availability by node
                Int32? currentUserId = View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID ? (Int32?)null : View.CurrentLoggedInUserId;
                View.lstUserGroup = ComplianceSetupManager.GetAllUserGroupWithPermission(ClientId,currentUserId).OrderBy(ex => ex.UG_Name).ToList();
            }
        }

        /// <summary>
        /// To get Compliance items
        /// </summary>
        public void GetComplianceItems()
        {
            if (ClientId == 0)
                View.lstComplianceItem = new List<ComplianceItem>();
            else
            {
                //View.lstComplianceItem = ComplianceSetupManager.GetComplianceItems(ClientId, false).Where(cond => cond.IsActive == true).ToList();
                View.lstComplianceItem = GetComplianceItemList().OrderBy(x => x.Name).ToList();//UAT sort dropdowns by Name;
            }
        }

        /// <summary>
        /// To perform search
        /// </summary>
        public void PerformSearch()
        {
            if (ClientId == 0)
            {
                View.ComplianceDocumentList = new List<ComplianceDocumentSearchContract>();
            }
            else
            {
                SearchItemDataContract searchDataContract = new INTSOF.UI.Contract.ComplianceManagement.SearchItemDataContract();
                searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
                searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
                searchDataContract.FromDate = View.DocumentFromDate.IsNullOrEmpty() ? (DateTime?)null : View.DocumentFromDate; //UAT 2566
                searchDataContract.ToDate = View.DocumentToDate.IsNullOrEmpty() ? (DateTime?)null : View.DocumentToDate; //UAT 2566

                //if (View.DPM_ID > SysXDBConsts.NONE)
                //{
                //    searchDataContract.DPM_Id = View.DPM_ID;
                //}
                if (!View.DPM_IDs.IsNullOrEmpty())
                {
                    searchDataContract.SelectedDPMIds = View.DPM_IDs;
                }
                //Commented below code for UAT-1175:Update Category and Document dropdowns to only display one per unique value (from name or label whichever is used)
                //searchDataContract.SelectedItemIDList = View.SelectedComplianceItemIds.Count == 0 ? null : View.SelectedComplianceItemIds;
                //searchDataContract.SelectedItemIDs = GetXMLString(searchDataContract.SelectedItemIDList);

                //UAT-1175:Update Category and Document dropdowns to only display one per unique value (from name or label whichever is used)
                searchDataContract.SelectedItemList = View.SelectedComplianceItemNames.Count == 0 ? null : View.SelectedComplianceItemNames;
                searchDataContract.SelectedItemNames = GetXMLString(searchDataContract.SelectedItemList);

                searchDataContract.SelectedUserGroupIDList = View.SelectedUserGroupIds.Count == 0 ? null : View.SelectedUserGroupIds;
                searchDataContract.SelectedUserGroupIDs = GetXMLString(searchDataContract.SelectedUserGroupIDList);

                if (View.TenantId != SecurityManager.DefaultTenantID)
                {
                    searchDataContract.LoggedInUserId = View.CurrentLoggedInUserId;
                }

                searchDataContract.LoggedInUserTenantId = View.TenantId;

                try
                {
                    View.ComplianceDocumentList = ComplianceDataManager.GetComplianceDocumentSearch(ClientId, searchDataContract, View.GridCustomPaging);
                    if (View.ComplianceDocumentList.IsNotNull() && View.ComplianceDocumentList.Count > 0)
                    {
                        if (View.ComplianceDocumentList[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.ComplianceDocumentList[0].TotalCount;
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
                    View.ComplianceDocumentList = new List<ComplianceDocumentSearchContract>();
                    throw e;
                }
            }
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

        private String GetXMLString(List<String> listOfItemNames)
        {
            if (listOfItemNames.IsNotNull() && listOfItemNames.Count > 0)
            {
                StringBuilder IdString = new StringBuilder();
                foreach (String id in listOfItemNames)
                {
                    IdString.Append("<Root><Value>" + id.ToString() + "</Value></Root>");
                }

                return IdString.ToString();
            }
            return null;
        }

        private List<ComplianceItem> GetComplianceItemList()
        {
            return ComplianceSetupManager.GetComplianceItems(ClientId, false).Where(cond => cond.IsActive == true).Select(slct =>
                new ComplianceItem
                {
                    ComplianceItemID = slct.ComplianceItemID,
                    Name = slct.ItemLabel.IsNullOrEmpty() ? slct.Name : slct.ItemLabel,
                    Description = slct.Description,
                    ItemLabel = slct.ItemLabel
                }).OrderBy(x => x.Name).DistinctBy(dist => dist.Name).ToList();
        }
    }
}
