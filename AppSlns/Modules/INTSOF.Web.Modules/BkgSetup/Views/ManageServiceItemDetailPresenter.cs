using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using Business.RepoManagers;
using Entity.ClientEntity;

namespace CoreWeb.BkgSetup.Views
{
    public class ManageServiceItemDetailPresenter : Presenter<IManageServiceItemDetailView>
    {
        public Int32 getObjectTypeIdByCode()
        {
            string serviceObjectTypeCode = BkgObjectType.SERVICE_ITEM.GetStringValue();
            return BackgroungRuleManager.getObjectTypeIdByCode(serviceObjectTypeCode, View.SelectedTenantId);
        }

        public PackageServiceItem GetPackageServiceItemData()
        {
            return BackgroundPricingManager.GetPackageServiceItemData(View.SelectedTenantId, View.PackageServiceItemId);
        }
            
    }
}
