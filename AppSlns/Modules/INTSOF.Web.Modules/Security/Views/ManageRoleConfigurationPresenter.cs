using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.IntsofSecurityModel;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.Security.Views
{
    public class ManageRoleConfigurationPresenter : Presenter<IManageRoleConfigurationView>
    {
        public void GetRolesSetting()
        {
            // GetRoles();
            View.lstRoleConfig = new List<RoleConfigurationContract>();
            List<RoleConfigurationContract> lstRolesConfiguration = SecurityManager.GetRolesSetting();

            if (!lstRolesConfiguration.IsNullOrEmpty())
            {
                lstRolesConfiguration.ForEach(item =>
                {
                    item.RoleName = (!item.RoleName.IsNull()) ? item.RoleName.Split(new char[] { '_' }).FirstOrDefault() : String.Empty;
                    item._IsAllowPreferredTenantExp = item.RPTS_IsAllowPreferredTenant ? "Yes" : "No";
                    item._IsAllowDataEntryExp = item.RPTS_IsAllowDataEntry ? "Yes" : "No";
                    item._IsAllowComplianceVerficationExp = item.RPTS_IsAllowComplianceVerfication ? "Yes" : "No";
                    item._IsAllowRotationVerficationExp = item.RPTS_IsAllowRotationVerfication ? "Yes" : "No";
                    item._IsAllowLocationEnrollerExp = item.RPTS_IsAllowLocationEnroller ? "Yes" : "No";

                });

                View.lstRoleConfig = lstRolesConfiguration;
            }
            else
            {
                View.lstRoleConfig = new List<RoleConfigurationContract>();
            }
        }

        public Boolean SaveRolePreferredTenantSetting()
        {
            //To save/ Update role settings
            return SecurityManager.SaveRolePreferredTenantSetting(View.RolePreferredTenantSetting, View.CurrentLoggedInUserId);
        }

    }
}
