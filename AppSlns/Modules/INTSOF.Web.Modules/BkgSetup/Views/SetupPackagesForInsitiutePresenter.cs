using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public class SetupPackagesForInsitiutePresenter : Presenter<ISetupPackagesForInsitiuteView>
    {
        public void GetPackageData()
        {
            View.lstPackages = BackgroundSetupManager.GetPackageData(View.tenantId);
        }

        public void SavePackagedetail(BackgroundPackage backgroundPackage)
        {
            List<Int32> targetPackageIds = new List<Int32>();
            View.ErrorMessage = BackgroundSetupManager.SaveEditPackagedetail(View.tenantId, backgroundPackage, AppConsts.NONE, targetPackageIds, AppConsts.NONE,false);
        }

        public Boolean DeletePackageMapping(Int32 packageID)
        {
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.IfPackageMappingCanBeDeleted(packageID, View.tenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                View.ErrorMessage = response.UIMessage;
                return false;
            }
            else
            {
                return BackgroundSetupManager.DeletePackageMapping(View.tenantId, packageID);
            }
        }

        public void GetPackageNotesPosition(String selectedPositionCode)
        {
            View.NotesPositionId = BackgroundSetupManager.GetPackageNotesPosition(View.tenantId, selectedPositionCode).PNP_ID;
        }
        #region UAT-3525
        public List<BkgPackageType> GetBkgPackageType()
        {
            return BackgroundSetupManager.GetAllBkgPackageTypes(View.tenantId).ToList();
        }
        #endregion
    }
}
