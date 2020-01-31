using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public class OverridePackageServiceFormDispatchTypePresenter : Presenter<IOverridePackageServiceFormDispatchType>
    {
        public List<ServiceFormsDispatchTypesContract> GetServiceFormDispatchType()
        {
            return BackgroundSetupManager.GetServiceFormDispatchType(View.PackageId, View.BkgSvcId, View.BPSId, View.SelectedTenantId);
        } 
    }
}
