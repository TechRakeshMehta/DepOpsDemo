using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.UI.Contract.CommonControls;

namespace CoreWeb.CommonControls.Views
{
    public class ColumnsConfigurationPresenter : Presenter<IColumnsConfigurationView>
    {
        public override void OnViewInitialized()
        {
            base.OnViewInitialized();
        }

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
        }

        public void GetColumns()
        {
            View.ColumnsConfigurationData = SecurityManager.GetScreenColumns(View.lstGridCode, View.OrganisationUserID);
        }


        public Boolean SaveUserScreenColumnMapping(Dictionary<Int32,Boolean> columnVisibility)
        {
          return  SecurityManager.SaveUserScreenColumnMapping(columnVisibility, View.CurrentLoggedInUserID,View.OrganisationUserID);
        }
    }
}
