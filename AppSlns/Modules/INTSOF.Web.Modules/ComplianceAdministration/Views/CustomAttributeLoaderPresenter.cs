using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using System.Linq;
using INTSOF.Utils;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class CustomAttributeLoaderPresenter : Presenter<ICustomAttributeLoaderView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceAdministrationController _controller;
        // public CustomAttributeLoaderPresenter([CreateNew] IComplianceAdministrationController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {

        }

        public void GetCustomAttributes(Int32 mappingRecordId, Int32 valueRecordId, String useTypeCode, Int32 tenantId)
        {
            View.lstTypeCustomAttributes = ComplianceDataManager.GetCustomAttributes(mappingRecordId, valueRecordId, useTypeCode, View.CurrentLoggedInUserId, tenantId);
            #region UAT-2494
            List<Int32> lstIdsToFilter = new List<Int32>();
            if (!View.LstProfileCustomAttributeOverride.IsNullOrEmpty())
            {
                Dictionary<Int32, String> map = View.LstProfileCustomAttributeOverride.Where(cond => cond.SettingValue).ToDictionary(x => x.CustomAttributeID, x => (x.SettingOverrideText.IsNullOrEmpty() ? x.SettingName : x.SettingOverrideText));
                foreach (TypeCustomAttributes item in View.lstTypeCustomAttributes)
                {
                    if (map.ContainsKey(item.CAId))
                    {
                        item.CALabel = map[item.CAId];
                    }
                    else
                    {
                        lstIdsToFilter.Add(item.CAId);
                    }
                }
                View.lstTypeCustomAttributes.RemoveAll(x => lstIdsToFilter.Contains(x.CAId));
            }
            #endregion
        }

        /// <summary>
        /// Gets the Custom Attributes for the Last selected node in the hierarchy
        /// </summary>
        /// <param name="useTypeCode"></param>
        /// <param name="selectedDPMId"></param>
        /// <param name="tenantId"></param>
        public void GetCustomAttributesByNodes(String useTypeCode, Int32? selectedDPMId, Int32 tenantId)
        {
            View.lstTypeCustomAttributes = ComplianceDataManager.GetCustomAttributesByNodes(useTypeCode, selectedDPMId, View.CurrentLoggedInUserId, tenantId);
        }

        #region UAT 1438: Enhancement to allow students to select a User Group.
        public void GetAllUserGroup()
        {
            if (View.TenantId > 0)
            {
                View.lstUserGroups = ComplianceSetupManager.GetAllUserGroup(View.TenantId).OrderBy(ex => ex.UG_Name);
            }

        }

        public void GetUserGroupsForUser()
        {
            if (View.TenantId > 0 && View.CurrentLoggedInUserId > 0)
            {
                View.lstUserGroupsForUser = ComplianceDataManager.GetUserGroupsForUser(View.TenantId, View.CurrentLoggedInUserId);
            }
        }
        #endregion

        //UAT-2792
        public Int32 GetCustomAttributeIDFromAppConfiguration(Int32 tenantId, String key)
        {
            if (tenantId != AppConsts.NONE)
            {
                Entity.ClientEntity.AppConfiguration appConfig = ComplianceDataManager.GetAppConfiguration(key, tenantId);
                if (!appConfig.IsNullOrEmpty())
                {
                    return Convert.ToInt32(appConfig.AC_Value);
                }
            }
            return AppConsts.NONE;
        }
    }
}




