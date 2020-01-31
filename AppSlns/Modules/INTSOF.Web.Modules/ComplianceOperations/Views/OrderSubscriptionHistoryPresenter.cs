using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public class OrderSubscriptionHistoryPresenter : Presenter<IOrderSubscriptionHistoryView>
    {
        public void GetBuisnessChannelTypeByTenantId(Int32 tenantId)
        {
            List<ManageBuisnessChannelTypeContract> buisnessChannelList = SecurityManager.GetBuisnessChannelTypeByTenantId(tenantId);
            if (buisnessChannelList != null)
            {
                var complioBuisnessChanel = buisnessChannelList.FirstOrDefault(x => x.BuisnessChannelTypeCode == BusinessChannelType.COMPLIO.GetStringValue());
                if (complioBuisnessChanel != null)
                    View.IsComplioBuisnessChannelTypeAvlbl = true;
                else
                    View.IsComplioBuisnessChannelTypeAvlbl = false;
            }
        }

        #region UAT-3521 || CBI || CABS

        public void IsLocationServiceTenant(Int32 TenantId)
        {
            if (TenantId > AppConsts.NONE)
                View.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(TenantId);
        }

        #endregion
    }
}
