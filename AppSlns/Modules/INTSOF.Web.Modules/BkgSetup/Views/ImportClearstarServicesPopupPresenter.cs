using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;

namespace CoreWeb.BkgSetup.Views
{
    public class ImportClearstarServicesPopupPresenter : Presenter<IImportClearstarServicesPopupView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void FetchClearstarServices()
        {
            View.ClearStarServices = BackgroundSetupManager.FetchClearstarServices(View.DefaultTenantId);
        }

        public Boolean ImportClearStarServices()
        {
            return BackgroundSetupManager.ImportClearStarServices(View.DefaultTenantId, View.SelectedCssIds, View.VendorID, View.CurrentLoggedInUserID);
        }

        public void FetchAllClearstarServices()
        {
            View.AllClearstarServices = BackgroundSetupManager.FetchAllClearstarServices(View.DefaultTenantId);
        }

        public void SaveClearStarSevices(List<Entity.ClearStarService> lstClearStarService)
        {            
            if (BackgroundSetupManager.SaveClearStarSevices(lstClearStarService, View.DefaultTenantId))
            {
                //StringBuilder sb = new StringBuilder();

                //sb.Append("Following Services are added Successfully:");
                //sb.Append(Environment.NewLine);
                //lstClearStarService.ForEach(cond => {
                //    sb.Append(cond.CSS_Number + " - " + cond.CSS_Name);
                //    sb.Append(Environment.NewLine);
                //});

                //View.SuccessMessage = sb.ToString();
                return;
            }

            View.ErrorMessage = "Some Error occurred while saving External Service.";
            return;
        }
    }
}
