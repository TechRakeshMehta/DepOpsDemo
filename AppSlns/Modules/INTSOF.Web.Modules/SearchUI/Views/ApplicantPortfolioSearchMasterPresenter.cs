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
    public class ApplicantPortfolioSearchMasterPresenter : Presenter<IApplicantPortfolioSearchMasterView>
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
        public List<Entity.Tenant> GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            return SecurityManager.GetTenants(SortByName, false, clientCode);
        }

        /// <summary>
        /// To get Admin Program Study
        /// </summary>
        public List<UserGroup> GetAllUserGroups(Int32 clientId)
        {
            if (SecurityManager.DefaultTenantID == clientId || clientId == -1 || clientId == 0)
                return new List<UserGroup>();
            else
                return ComplianceSetupManager.GetAllUserGroup(clientId).ToList();
        }

        public Dictionary<Int32, String> GetSearchModeList()
        {
            Dictionary<Int32, String> searchModeList = new Dictionary<Int32, String>();
            searchModeList[Convert.ToInt32(MasterSearchMode.Online)] = MasterSearchMode.Online.ToString();
            searchModeList[Convert.ToInt32(MasterSearchMode.Offline)] = MasterSearchMode.Offline.ToString();
            return searchModeList;
        }

        /// <summary>
        /// To get Tenant Id
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }
        // TODO: Handle other view events and set state in the view

        /// <summary>
        /// To get the list of offline search results
        /// </summary>
        /// <returns></returns>
        public DataTable GetOfflineSearchResultList()
        {
            Int16[] lstSearchScope = new Int16[] {
            SearchScope.SingleTenantAsynch,
            SearchScope.TenantsListAsynch,
            SearchScope.AllTenantsAsynch };
            DataSet searchDataSet = ExternalServiceProxy.ComplianceRuleServiceProxy.GetSearchResultList(View.CurrentLoggedInUserId, SearchTypeCode.AppilicantPortfolioSearch, lstSearchScope);

            DataTable SearchResultInstance = null;
            if (searchDataSet.IsNotNull() && searchDataSet.Tables.Count > 0)
            {
                SearchResultInstance = GetSearchInstanceTable(searchDataSet, SearchResultInstance);
            }
            return SearchResultInstance;
        }

        private DataTable GetSearchInstanceTable(DataSet searchDataSet, DataTable SearchResultInstance)
        {
            if (searchDataSet.Tables[SearchDataTable.SearchResult] != null && searchDataSet.Tables[SearchDataTable.SearchResult].Rows.Count > 0)
            {
                SearchResultInstance = searchDataSet.Tables[SearchDataTable.SearchResult];
            }
            return SearchResultInstance;
        }

        public Int32 GetSelectedTenantId(Int32 tenantId)
        {
            if(tenantId == SecurityManager.DefaultTenantID)
            {
                return -1;
            }
            return tenantId;
        }
    }
}




