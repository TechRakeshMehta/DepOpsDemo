using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public class ManageMasterStateSearchPresenter : Presenter<IManageMasterStateSearchView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetStateList()
        {
            List<Entity.State> objStateList = SecurityManager.GetStates().Where(x => x.Country.CountryID == AppConsts.COUNTRY_USA_ID && !x.StateName.Equals(AppConsts.COMBOBOX_ITEM_SELECT)).ToList();
            if (objStateList.IsNotNull() && objStateList.Count > 0)
            {
                View.lstState = objStateList;
            }
        }

        public void SaveMasterStateSearchCriteria()
        {
            if (BackgroundSetupManager.SaveMasterStateSearchCriteria(View.DefaultTenantId, View.lstStateSearchContract, View.CurrentLoggedInUserId))
            {
                View.ErrorMessage = String.Empty;
            }
            else
            {
                View.ErrorMessage = "Some error has ocured while saving the state search criteria. Please try again.";
            }
        }

        public void GetMasterStateSearchCriteria()
        {
            View.lstMasterStateSearch = BackgroundSetupManager.GetMasterStateSearchCriteria(View.DefaultTenantId);
        }
    }
}
