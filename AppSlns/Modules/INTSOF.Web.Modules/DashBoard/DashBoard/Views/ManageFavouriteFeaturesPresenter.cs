using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.DashBoard.DashBoard.Views
{
    public class ManageFavouriteFeaturesPresenter : Presenter<IManageFavouriteFeaturesView>
    {
        public void GetAccessibleFeatureList()
        {
            View.lstAccessibleFeatures = SecurityManager.GetAccessibleFeatures(View.OrganizationUserId, View.TenantID, View.IsSuperAdmin ? View.BusinessChannelTypeID : View.SysXBlockId, View.IsSuperAdmin);
        }
    }
}
