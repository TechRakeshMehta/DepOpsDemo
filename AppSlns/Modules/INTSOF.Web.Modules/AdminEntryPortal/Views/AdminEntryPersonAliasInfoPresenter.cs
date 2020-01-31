using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using INTSOF.SharedObjects;
using Business.RepoManagers;

namespace CoreWeb.AdminEntryPortal.Views
{
    public class AdminEntryPersonAliasInfoPresenter : Presenter<IAdminEntryPersonAliasInfo>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }
        public void GetAdminEntrySuffixes()
        {
            View.lstAliasSuffixes = new List<Entity.lkpAdminEntrySuffix>();
            View.lstAliasSuffixes = SecurityManager.GetAdminEntrySuffixes();
        }
    }
}
