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
using System.Data;
using ExternalServiceProxy;
using System.Xml.Linq;

namespace CoreWeb.Search.Views
{
    public class ApplicantPortFolioSearch_CopyPresenter : Presenter<IApplicantPortFolioSearch_CopyView>
    {
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
            View.TenantDropdownDataSource = SecurityManager.GetTenants(SortByName, false, clientCode);
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
        public void GetAllUserGroups(Int32 clientId)
        {
            View.UserGroupListDataSource = ComplianceSetupManager.GetAllUserGroup(clientId).ToList();
        }

        /// <summary>
        /// To perform search
        /// </summary>
        public void PerformSearch()
        {
            String errorMsg = String.Empty;
            if (View.SelectedTenantId > AppConsts.DEFAULT_SELECTED_TENANTID && ((!View.IsOnlineUserControl && View.SearchInstanceId > AppConsts.DEFAULT_SEARCH_INSTANCEID) || View.IsOnlineUserControl))
            {
                SearchItemDataContract searchDataContract = new INTSOF.UI.Contract.ComplianceManagement.SearchItemDataContract();
                searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
                searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
                if (View.OrganizationUserID > SysXDBConsts.NONE)
                {
                    searchDataContract.OrganizationUserId = View.OrganizationUserID;
                }
                searchDataContract.EmailAddress = String.IsNullOrEmpty(View.EmailAddress) ? null : View.EmailAddress;
                searchDataContract.ApplicantSSN = String.IsNullOrEmpty(View.SSN) ? null : View.SSN;
                searchDataContract.DateOfBirth = View.DateOfBirth;
                if (View.DPM_ID > SysXDBConsts.NONE)
                {
                    searchDataContract.DPM_Id = View.DPM_ID;
                }
                searchDataContract.CustomFields = String.IsNullOrEmpty(View.CustomFields) ? null : View.CustomFields;
                if (View.MatchUserGroupId > SysXDBConsts.NONE)
                {
                    searchDataContract.MatchUserGroupID = View.MatchUserGroupId;
                    searchDataContract.FilterUserGroupID = View.FilterUserGroupId;
                }

                try
                {
                    List<Int32> listTenantId = new List<Int32>();
                    if (ClientId != AppConsts.NONE)
                    {
                        searchDataContract.ClientID = View.SelectedTenantId;
                        listTenantId.Add(View.SelectedTenantId);
                    }
                    String sortDirection = "Asc";
                    if (View.GridCustomPaging.SortDirectionDescending)
                        sortDirection = "Desc";
                    String searchParameter = BuildSearchParameterXML(searchDataContract);

                    DataSet searchDataSet = ExternalServiceProxy.ComplianceRuleServiceProxy.Search(View.CurrentLoggedInUserId, SearchTypeCode.AppilicantPortfolioSearch, searchParameter, listTenantId, View.GetSearchScopeType, View.SearchInstanceId,
                        View.GridCustomPaging.CurrentPageIndex, View.GridCustomPaging.PageSize, View.GridCustomPaging.SortExpression, sortDirection);

                    DataTable MasterResultTable = null;
                    if (searchDataSet.IsNotNull() && searchDataSet.Tables.Count > AppConsts.NONE)
                    {
                        MasterResultTable = GetMasterResultTable(searchDataSet, MasterResultTable);
                        GetSearchInstanceId(searchDataSet);
                        GetErrorMessage(searchDataSet);
                        if (!View.ErrorMessage.IsNullOrEmpty())
                            return;
                    }

                    if (MasterResultTable.IsNotNull() && MasterResultTable.Rows.Count > AppConsts.NONE)
                    {
                        View.VirtualRecordCount = Convert.ToInt32(MasterResultTable.Rows[0]["TotalCount"]);
                    }
                    else
                    {
                        View.VirtualRecordCount = 0;
                        View.CurrentPageIndex = 1;
                    }
                    View.SetApplicantSearchData = MasterResultTable;
                }
                catch (Exception e)
                {
                    View.SetApplicantSearchData = null;
                    throw e;
                }
            }
            else
            {
                View.SetApplicantSearchData = null;
            }
        }

        private void GetErrorMessage(DataSet searchDataSet)
        {
            if (searchDataSet.Tables[SearchDataTable.Error] != null && searchDataSet.Tables[SearchDataTable.Error].Rows.Count > 0)
            {
                View.ErrorMessage = "Some Error occurred while performing search operation. Please try again later or contact technical support team.";
                View.SetApplicantSearchData = null;
                View.VirtualRecordCount = 0;
                View.CurrentPageIndex = 1;
            }
        }

        private void GetSearchInstanceId(DataSet searchDataSet)
        {
            if (searchDataSet.Tables[SearchDataTable.SearchResultInstance] != null && searchDataSet.Tables[SearchDataTable.SearchResultInstance].Rows.Count > 0)
            {
                View.SearchInstanceId = Convert.ToInt32(searchDataSet.Tables[SearchDataTable.SearchResultInstance].Rows[0]["SRI_ID"]);
            }
        }

        private DataTable GetMasterResultTable(DataSet searchDataSet, DataTable MasterResultTable)
        {
            if (searchDataSet.Tables[SearchDataTable.SearchResult] != null && searchDataSet.Tables[SearchDataTable.SearchResult].Rows.Count > 0)
            {
                MasterResultTable = searchDataSet.Tables[SearchDataTable.SearchResult];
            }
            return MasterResultTable;
        }

        private String BuildSearchParameterXML(SearchItemDataContract searchDataContract)
        {
            XElement root = new XElement("root");
            XElement row = null;

            if (searchDataContract.ApplicantFirstName.IsNotNull())
            {
                row = new XElement("FirstName");
                row.Value = searchDataContract.ApplicantFirstName;
                root.Add(row);
            }
            if (searchDataContract.ApplicantLastName.IsNotNull())
            {
                row = new XElement("LastName");
                row.Value = searchDataContract.ApplicantLastName;
                root.Add(row);
            }
            if (searchDataContract.OrganizationUserId.IsNotNull())
            {
                row = new XElement("OrganizationUserID");
                row.Value = searchDataContract.OrganizationUserId.Value.ToString();
                root.Add(row);

            }
            if (searchDataContract.EmailAddress.IsNotNull())
            {
                row = new XElement("EmailAddress");
                row.Value = searchDataContract.EmailAddress;
                root.Add(row);
            }
            if (searchDataContract.ApplicantSSN.IsNotNull())
            {
                row = new XElement("SSN");
                row.Value = searchDataContract.ApplicantSSN;
                root.Add(row);
            }
            if (searchDataContract.DateOfBirth.IsNotNull())
            {
                row = new XElement("DOB");
                row.Value = searchDataContract.DateOfBirth.Value.ToString();
                root.Add(row);
            }
            if (searchDataContract.DPM_Id.IsNotNull())
            {
                row = new XElement("NodeID");
                row.Value = searchDataContract.DPM_Id.Value.ToString();
                root.Add(row);

            }
            if (searchDataContract.CustomFields.IsNotNull())
            {
                row = new XElement("CustomFields");
                row.Value = searchDataContract.CustomFields;
                root.Add(row);
            }
            if (searchDataContract.MatchUserGroupID.IsNotNull())
            {
                row = new XElement("MatchUserGroupID");
                row.Value = searchDataContract.MatchUserGroupID.Value.ToString();
                root.Add(row);
            }
            if (searchDataContract.FilterUserGroupID.IsNotNull())
            {
                row = new XElement("FilterUserGroupID");
                row.Value = searchDataContract.FilterUserGroupID.Value.ToString();
                root.Add(row);
            }

            row = new XElement("TenantID");
            row.Value = searchDataContract.ClientID.ToString();
            root.Add(row);

            return root.ToString();
        }

        /// <summary>
        /// Getting Formatted SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public string GetFormattedSSN(String unformattedSSN)
        {
            return ApplicationDataManager.GetFormattedSSN(unformattedSSN);
        }
    }
}




