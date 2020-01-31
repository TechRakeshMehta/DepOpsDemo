using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public class ManageServiceDetailPresenter : Presenter<IManageServiceDetailView>
    {
        public Int32 getObjectTypeIdByCode()
        {
            string serviceObjectTypeCode=BkgObjectType.SERVICE.GetStringValue();
            return BackgroungRuleManager.getObjectTypeIdByCode(serviceObjectTypeCode, View.SelectedTenantId);
        }
    }
}
