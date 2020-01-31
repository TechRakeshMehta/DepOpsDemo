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
    public class SetUpServiceForServicegroupPresenter : Presenter<ISetUpServiceForServicegroupView>
    {
        public void GetServicessForGridData()
        {
            View.lstServices = BackgroundSetupManager.GetServicesForGridData(View.TenantId, View.ServiceGroupId, View.PackageId);
        }

        public BkgPackageSvcGroup GetServiceGroupDetail()
        {
            return BackgroundSetupManager.GetServicesGroupForEdit(View.TenantId, View.ServiceGroupId);
        }

        public BkgPackageSvc GetCurrentBkgPkgService(Int32 serviceId)
        {
            return BackgroundSetupManager.GetCurrentBkgPkgService(View.TenantId, View.ServiceGroupId, View.PackageId, serviceId);
        }

        public void MapServiceWithServiceGroup(String _dataXML)
        {
            View.ErrorMessage = BackgroundSetupManager.MapServiceWithServiceGroup(View.TenantId, View.ServiceId, View.ServiceGroupId, View.PackageId, false,
                View.DisplayName, View.Notes, View.PkgCount, View.MinOccurrences, View.MaxOccurrences, View.ResidenceDuration,
                View.SendDocsToStudent, View.IsSupplemental, View.IgnoreRHOnSupplement, View.IsReportable, _dataXML); //UAT 1423
        }

        public void DeleteServiceMapping(Int32 serviceId)
        {
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.IfServiceMappingCanBeDeleted(serviceId, View.ServiceGroupId, View.PackageId, View.TenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                View.ErrorMessage = response.UIMessage;
            }
            else
            {
                View.ErrorMessage = BackgroundSetupManager.MapServiceWithServiceGroup(View.TenantId, serviceId, View.ServiceGroupId, View.PackageId,
                    true, null, null, null, null, null, null, false, false, false, true); //UAT 1423
            }
        }

        public List<BackgroundService> GetServicesForDropDown()
        {
            List<Int32> lstTobeRemoved = BackgroundSetupManager.GetServicesByPackageId(View.TenantId, View.PackageId).Distinct().ToList();
            return BackgroundSetupManager.GetServicesForDropDown(View.TenantId, lstTobeRemoved).OrderBy(col => col.BSE_Name).ToList();
        }

        public Entity.ApplicableServiceSetting GetServiceSettings()
        {
            return BackgroundSetupManager.GetServiceSetting(View.TenantId, View.ServiceId);
        }

        #region UAT-844 - ORDER REVIEW ENHANCEMENT
        //UPDATE PACKAGE SERVICE GROUP
        public void UpdatePackageServiceGroup(BkgPackageSvcGroup bkgPackageSvcGroup)
        {
            if (BackgroundSetupManager.UpdatePackageServiceGroup(View.TenantId, bkgPackageSvcGroup, View.PackageId, View.ServiceGroupId))
            {
                View.ErrorMessage = String.Empty;
            }
            else
            {
                View.ErrorMessage = "Service Group can not be updated. Please try again.";
            }
        }

        //GET PACKAGE SERVICE GROUP DETAILS BY BKG PACKAGEID AND SERVICEGROUPID
        public BkgPackageSvcGroup GetPkgServiceGroupDetail()
        {
            return BackgroundSetupManager.GetPkgServiceGroupDetail(View.TenantId, View.ServiceGroupId, View.PackageId);
        }
        #endregion
    }
}
