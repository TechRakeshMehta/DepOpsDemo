using System;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using System.Data;
using INTSOF.UI.Contract.SysXSecurityModel;
using System.Linq;
using INTSOF.Utils;


namespace CoreWeb.Dashboard.Views
{
    public class DefaultViewPresenter : Presenter<IDefaultView>
    {


        public DefaultViewPresenter()
        {

        }

        public Dictionary<string, string> GetDashboardMarkup(Guid userid, int tenantID, short businessChannelTypeId)
        {

            if (SecurityManager.IsTenantThirdPartyType(tenantID, View.thirdPartyCode))
            {
                Int32 orgId = WebSiteManager.GetOrganisationIDByURL(View.SiteUrl);
                tenantID = SecurityManager.GetTenantIdFromOrganizationId(orgId);
            }
            View.LoggedInUserTenantId = tenantID;
            Dictionary<string, string> dashboard = SecurityManager.GetDashboardMarkup(userid, tenantID, businessChannelTypeId);
            return dashboard;
        }

        public void SaveWidgetStates(Guid userid, Dictionary<string, string> dashboard, short businessChannelTypeId)
        {
            SecurityManager.SavePersonalizedPreference(userid, dashboard, businessChannelTypeId);
        }

        public void ClearWidgetStates(Guid userid, short businessChannelTypeId)
        {
            SecurityManager.ClearDashboardStates(userid, businessChannelTypeId);
        }

        public Boolean? CheckIfUserIsApplicant()
        {
            Boolean? isapplicant = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).IsApplicant;
            return isapplicant;
        }

        public Int32 GetBuisnessChannelTypeByTenantId(Int32 tenantId)
        {

            List<ManageBuisnessChannelTypeContract> buisnessChannelList = SecurityManager.GetBuisnessChannelTypeByTenantId(tenantId);

            if (buisnessChannelList != null)
            {
                var complioBuisnessChanel = buisnessChannelList.FirstOrDefault(x => x.BuisnessChannelTypeCode == BusinessChannelType.COMPLIO.GetStringValue());
                var amsBuisnessChanel = buisnessChannelList.FirstOrDefault(x => x.BuisnessChannelTypeCode == BusinessChannelType.AMS.GetStringValue());
                if (complioBuisnessChanel != null)
                    return complioBuisnessChanel.BuisnessChannelTypeId;
                else
                    return amsBuisnessChanel.BuisnessChannelTypeId;
            }
            return AppConsts.NONE;
        }
    }
}