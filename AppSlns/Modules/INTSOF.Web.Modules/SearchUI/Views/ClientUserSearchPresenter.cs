using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.SysXSecurityModel;

namespace CoreWeb.Search.Views
{
    public class ClientUserSearchPresenter : Presenter<IClientUserSearchView>
    {
        #region VARIABLES

        #endregion

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantID;
            }
        }

        #region EVENTS

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetTenants();
        }

        private void GetTenants()
        {
            Boolean sortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            List<Entity.Tenant> lstTenants = SecurityManager.GetTenants(sortByName, false, clientCode);
            if (lstTenants.Count > 0)
            {
                View.lstTenant = lstTenants;
            }
        }

        #endregion

        #region METHODS

        #region PRIVATE METHODS
        #endregion

        #region PUBLIC METHODS
        public void PerformSearch()
        {
            String tenantIDList = "";
            //String agencyIDList = ""; //commented for UAT-4257
            String agencyRootNodeIDList = "";
            if (View.SelectedTenantIDs.IsNotNull() && View.SelectedTenantIDs.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var item in View.SelectedTenantIDs)
                {
                    builder.Append("<TenantID>" + item.ToString() + "</TenantID>");
                }
                tenantIDList = "<TenantIdList>" + builder + "</TenantIdList>";
            }

            //Commented for UAT-4257

            //if (View.SelectedAgencyIDs.IsNotNull() && View.SelectedAgencyIDs.Count > 0)
            //{
            //    StringBuilder builder = new StringBuilder();
            //    foreach (var item in View.SelectedAgencyIDs)
            //    {
            //        builder.Append("<AgencyID>" + item.ToString() + "</AgencyID>");
            //    }
            //    agencyIDList = "<AgencyIdList>" + builder + "</AgencyIdList>";
            //}

            //UAT-4257
            if (View.lstSelectedAgencyHierarchyIDs.IsNotNull() && View.lstSelectedAgencyHierarchyIDs.Count > 0)
            {
                //StringBuilder builder = new StringBuilder();
                //foreach (var item in View.lstSelectedAgencyHierarchyIDs)
                //{
                //    builder.Append("<AgencyRootNodeID>" + item.ToString() + "</AgencyRootNodeID>");
                //}
                // agencyRootNodeIDList = "<AgencyRootNodeIdList>" + builder + "</AgencyRootNodeIdList>";
                agencyRootNodeIDList = String.Join(",", View.lstSelectedAgencyHierarchyIDs);
            }

            //Resolved issue: Pagination doesn’t reset, when admin search the record and click on “Reset” button 
            if (tenantIDList.IsNullOrEmpty())
            {
                View.ClientSearchData = new List<ClientUserSearchContract>();
                View.VirtualPageCount = AppConsts.NONE;
            }
            else
            {
                ClientUserSearchContract clientUserSearchContract = new ClientUserSearchContract();
                clientUserSearchContract.ClientFirstName = View.ClientFirstName.IsNullOrEmpty() ? null : View.ClientFirstName;
                clientUserSearchContract.ClientLastName = View.ClientLastName.IsNullOrEmpty() ? null : View.ClientLastName;
                clientUserSearchContract.ClientUserName = View.ClientUserName.IsNullOrEmpty() ? null : View.ClientUserName;
                clientUserSearchContract.EmailAddress = View.EmailAddress.IsNullOrEmpty() ? null : View.EmailAddress;
                if (IsDefaultTenant)
                {
                    clientUserSearchContract.CurrentLoggedInUserID = View.CurrentLoggedInUserID;
                }
                else
                {
                    clientUserSearchContract.CurrentLoggedInUserID = View.TenantID;
                }

                //Commented For UAT-4257
                //View.ClientSearchData = SecurityManager.GetClientUserSearchData(View.SearchType, tenantIDList, agencyIDList, clientUserSearchContract, View.GridCustomPaging);
                View.VirtualPageCount = AppConsts.NONE;
                View.ClientSearchData = SecurityManager.GetClientUserSearchData(View.SearchType, tenantIDList, View.HierarchyNode, agencyRootNodeIDList, View.SelectedAgecnyHierarchyIds, clientUserSearchContract, View.GridCustomPaging);
                if (View.ClientSearchData.IsNotNull() && View.ClientSearchData.Count > 0)
                {
                    if (View.ClientSearchData[0].TotalCount > 0)
                    {
                        View.VirtualPageCount = View.ClientSearchData[0].TotalCount;
                    }
                }
            }
        }

        public String GetFormattedPhoneNumber(String unformattedPhoneNumber)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(unformattedPhoneNumber);
        }

        #endregion

        #endregion

        public void GetAgenciesByInstitionIDs()
        {
            //UAT-1881
            if (IsAdminLoggedIn())
                View.lstAgencies = ProfileSharingManager.GetAgenciesByInstitionIDs(View.SelectedTenantIDs).OrderBy(x => x.AG_Name).ToList();
            else
                View.lstAgencies = ProfileSharingManager.GetAllAgencyForOrgUser(View.TenantID, View.CurrentLoggedInUserID).ToList();
        }
        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsAdminLoggedIn()
        {
            return (SecurityManager.DefaultTenantID == View.TenantID);
        }

        #region UAT-2511

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
        /// Add Web Agency Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddWebAgencyUserData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.AddWebApplicationData(key, serializedData, 300);
        }


        /// <summary>
        /// Update Web Agency Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdateWebAgencyUserData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.UpdateWebApplicationData(key, serializedData);
        }


        #endregion

        #region UAT-4257
        public void GetAgencyRootNodes()
        {
            View.lstAgencyHierarchyRootNodes = new List<INTSOF.ServiceDataContracts.Modules.AgencyHierarchy.AgencyHierarchyContract>();
            List<Int32> tenantIDs = View.SelectedTenantIDs;
            View.lstAgencyHierarchyRootNodes = AgencyHierarchyManager.GetAgencyHierarchyRootNodesByTenantIDs(tenantIDs);
        }
        #endregion
    }
}
