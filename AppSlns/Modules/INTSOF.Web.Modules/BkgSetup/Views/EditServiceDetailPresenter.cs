using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public class EditServiceDetailPresenter : Presenter<IEditServiceDetailView>
    {
        public void GetCurrentBkgPackageSvc()
        {
            View.CurrentBkgPackageSvc = BackgroundSetupManager.GetCurrentBkgPkgServiceDetail(View.TenantId, View.BkgPackageSrvcId);
        }

        public void GetExternalCode()
        {
            //UAT-3109
            if (!View.CurrentBkgPackageSvc.IsNullOrEmpty() && !View.CurrentBkgPackageSvc.BackgroundService.IsNullOrEmpty() && View.CurrentBkgPackageSvc.BackgroundService.BSE_ID > AppConsts.NONE)
            {
                View.AMERNumber = BackgroundSetupManager.GetCurrentBkgPkgServiceAMERDetail(View.CurrentBkgPackageSvc.BackgroundService.BSE_ID);
            }
            else
            {
                View.AMERNumber = String.Empty;
            }
        }

        public Entity.ApplicableServiceSetting GetServiceSettings()
        {
            return BackgroundSetupManager.GetServiceSetting(View.TenantId, View.BackgroundServiceId);
        }

        public void UpdateBkgPackageSvc()
        {
            if (View.CurrentBkgPackageSvc == null)
                GetCurrentBkgPackageSvc();
            BkgPackageSvc currentBkgPackageSvc = View.CurrentBkgPackageSvc;
            currentBkgPackageSvc.BPS_DisplayName = View.DisplayName;
            currentBkgPackageSvc.BPS_Notes = View.Notes;
            //currentBkgPackageSvc.BPS_IncludeInPackageCount = View.PkgCount;
            //currentBkgPackageSvc.BPS_MaxOccurrences = View.MaxOccurrences;
            //currentBkgPackageSvc.BPS_MinOccurrences = View.MinOccurrences;
            currentBkgPackageSvc.BPS_NumberOfYearsOfResidence = View.ResidenceDuration;
            currentBkgPackageSvc.BPS_SendDocumentsToStudent = View.SendDocsToStudent;
            currentBkgPackageSvc.BPS_IsSupplemental = View.IsSupplemental;
            currentBkgPackageSvc.BPS_IgnoreResidentialHistoryOnSupplement = View.IgnoreRHOnSupplement;
            if (BackgroundSetupManager.UpdateTenantChanges(View.TenantId))
            {
                View.SuccessMessage = "Package Service Updated successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error has occured.Please contact administrator.";
            }

        }
    }
}
