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

namespace CoreWeb.SearchUI.Views
{
    public class ClientLoginSearchPresenter : Presenter<IClientLoginSearchView>
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

        #region PRIVATE METHODS
        #endregion

        #region PUBLIC METHODS
        public void PerformSearch()
        {
            String tenantIDList = "";
            if (View.SelectedTenantIDs.IsNotNull() && View.SelectedTenantIDs.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var item in View.SelectedTenantIDs)
                {
                    builder.Append("<TenantID>" + item.ToString() + "</TenantID>");
                }
                tenantIDList = "<TenantIdList>" + builder + "</TenantIdList>";
            }

            //Resolved issue: Pagination doesn’t reset, when admin search the record and click on “Reset” button 
            if (tenantIDList.IsNullOrEmpty())
            {
                View.ClientSearchData = new List<ClientLoginSearchContract>();
                View.VirtualPageCount = AppConsts.NONE;
            }
            else
            {
                ClientLoginSearchContract clientLoginSearchContract = new ClientLoginSearchContract();
                clientLoginSearchContract.ClientFirstName = View.ClientFirstName.IsNullOrEmpty() ? null : View.ClientFirstName;
                clientLoginSearchContract.ClientLastName = View.ClientLastName.IsNullOrEmpty() ? null : View.ClientLastName;
                clientLoginSearchContract.ClientUserName = View.ClientUserName.IsNullOrEmpty() ? null : View.ClientUserName;
                clientLoginSearchContract.EmailAddress = View.EmailAddress.IsNullOrEmpty() ? null : View.EmailAddress;
                if (IsDefaultTenant)
                {
                    clientLoginSearchContract.CurrentLoggedInUserID = View.CurrentLoggedInUserID;
                }
                else
                {
                    clientLoginSearchContract.CurrentLoggedInUserID = View.TenantID;
                }
                View.ClientSearchData = SecurityManager.GetClientLoginSearchData(tenantIDList, clientLoginSearchContract, View.GridCustomPaging);
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
        public Boolean IsAdminLoggedIn()
        {
            return (SecurityManager.DefaultTenantID == View.TenantID);
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


        public Boolean AddImpersonationHistory(Int32 clientAdminUserID, Int32 CurrentLoggedInUserID)
        {
            return SecurityManager.AddImpersonationHistory(clientAdminUserID, CurrentLoggedInUserID);         
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


    }
}
