using System;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using System.Linq;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.SystemSetUp;

namespace CoreWeb.WebSite.Views
{
    public class ClientSettingsPresenter : Presenter<IClientSettingsView>
    {

        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {
            if (SecurityManager.DefaultTenantID == View.TenantId)
            {
                View.ShowClientDropDown = true;
                View.lstTenant = ComplianceDataManager.getClientTenant();
            }
        }

        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public Boolean AddClientSetting(Boolean ifSettingValueIsString = false)
        {
            ClientSetting clientSetting = new ClientSetting();
            clientSetting.CS_TenantID = View.ClientId;
            clientSetting.CS_SettingID = View.SettingId;
            if (ifSettingValueIsString)
            {
                clientSetting.CS_SettingValue = View.StringSettingValue;
            }
            else
            {
                clientSetting.CS_SettingValue = View.SettingValue.ToString();
            }
            clientSetting.CS_IsDeleted = false;
            clientSetting.CS_CreatedOn = DateTime.Now.Date;
            clientSetting.CS_CreatedByID = View.TenantId;

            if (ComplianceDataManager.AddClientSetting(View.ClientId, clientSetting))
                return true;
            else
                return false;
        }

        public Boolean UpdateClientSetting(ClientSetting clientSetting, Boolean ifSettingValueIsString = false)
        {
            clientSetting.CS_TenantID = View.ClientId;
            clientSetting.CS_SettingID = View.SettingId;
            if (ifSettingValueIsString)
            {
                clientSetting.CS_SettingValue = View.StringSettingValue;
            }
            else
            {
                clientSetting.CS_SettingValue = View.SettingValue.ToString();
            }
            clientSetting.CS_IsDeleted = false;
            clientSetting.CS_ModifiedOn = DateTime.Now.Date;
            clientSetting.CS_ModifiedByID = View.TenantId;

            if (ComplianceDataManager.UpdateClientSetting(View.ClientId))
                return true;
            else
                return false;
        }
        /// <summary>
        /// TO get payment option label for particular tenant
        /// (GetPaymentOptions is a complex type for corresponding SP i.e. usp_GetPaymentOptions)
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns>List of payment options in security and tenant</returns>
        public List<GetPaymentOptions> GetPaymentOptions(Int32 tenantId)
        {
            return ComplianceDataManager.GetPaymentOptions(tenantId);
        }

        public Boolean UpdatePaymentOptions(Int32 tenantId, List<GetPaymentOptions> newOption)
        {
            return ComplianceDataManager.UpdatePaymentOptions(newOption, tenantId, View.CurrentLoggedInUserId);
        }

        // customattr UAT-2494
        public List<ClientSettingCustomAttributeContract> GetCustomattributes(Int32 tenantId)
        {
            return ComplianceDataManager.GetCustomAttributesWithClientSettingmapping(tenantId);
        }

        public void AddUpdateCustomAttributeClientSetting(List<ClientSettingCustomAttributeContract> lstClientSettingCustomAttributeContract, Int32 TenantId)
        {
            ComplianceDataManager.AddUpdateCustomAttributeClientSetting(TenantId, lstClientSettingCustomAttributeContract, View.CurrentLoggedInUserId);
        }
        public Boolean IsExistClientPieChartTColorSetting(Int32 tenantId, Int32 Settingid, String ColorCode)
        {
            return ComplianceDataManager.IsExistClientPieChartTColorSetting(tenantId, Settingid, ColorCode);
        }

        public void ExecuteOptionalCategoryRule(Int32 tenantId, Int32 nodeID)
        {
            ComplianceDataManager.ExecuteOptionalCategoryRule(tenantId, View.CurrentLoggedInUserId, nodeID);
        }
        #region UAT-3601
        public void IsLocationServiceTenant(Int32 TenantID)
        {
            View.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(TenantID);
        }
        #endregion
    }
}




