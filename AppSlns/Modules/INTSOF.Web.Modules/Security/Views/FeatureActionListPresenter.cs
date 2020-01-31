using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Entity;
using Business.RepoManagers;
using INTSOF.UI.Contract.SysXSecurityModel;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public class FeatureActionListPresenter : Presenter<IFeatureActionListView>
    {
        public List<FeatureAction> GetFeatureAction(Int32 productFeatureID, Int32 rolePermissionID)
        {
            return SecurityManager.GetFeatureAction(productFeatureID, rolePermissionID);
        }
        public List<FeatureActionContract> GetFeatureActionT(Int32 productFeatureID)
        {
            return SecurityManager.GetFeatureActionT(productFeatureID);
        }

        public List<Permission> GetListOFPermissions()
        {
            return SecurityManager.GetListOFPermissions();
        }
    }
}
