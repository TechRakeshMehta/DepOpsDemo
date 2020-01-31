using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public class SetUpAttributeGroupForServicesPresenter : Presenter<ISetUpAttributeGroupForServicesView>
    {
        #region Methods
        /// <summary>
        /// Method to get Attribute group list mapped with services.
        /// </summary>
        public void GetMappedAttributeGrpWithService()
        {
            View.MappedAttributeGroupList = BackgroundSetupManager.GetMappedAttributeGroupList(View.TenantId, View.BackgroundServiceId, View.BackgroundServiceGroupId, View.BackgroundPackageId);
            //UAT-3109
            if (View.BackgroundServiceId > AppConsts.NONE)
            {
                View.AMERNumber = BackgroundSetupManager.GetCurrentBkgPkgServiceAMERDetail(View.BackgroundServiceId);
            }
            else
            {
                View.AMERNumber = String.Empty;
            }
        }

        public void GetCurrentBkgPackageSvc()
        {
            View.CurrentBkgPackageSvc = BackgroundSetupManager.GetCurrentBkgPkgService(View.TenantId, View.BackgroundServiceGroupId, View.BackgroundPackageId, View.BackgroundServiceId);
        }

        public Entity.ApplicableServiceSetting GetServiceSettings()
        {
            return BackgroundSetupManager.GetServiceSetting(View.TenantId, View.BackgroundServiceId);
        }

        public void UpdateBkgPackageSvc(List<BkgPackageSvcFormOverride> lstUpdateServiceFormsOverride)
        {
            if (View.CurrentBkgPackageSvc == null)
                GetCurrentBkgPackageSvc();
            BkgPackageSvc currentBkgPackageSvc = View.CurrentBkgPackageSvc;
            currentBkgPackageSvc.BPS_DisplayName = View.DisplayName;
            currentBkgPackageSvc.BPS_Notes = View.Notes;
            currentBkgPackageSvc.BPS_IncludeInPackageCount = View.PkgCount;
            currentBkgPackageSvc.BPS_MaxOccurrences = View.MaxOccurrences;
            currentBkgPackageSvc.BPS_MinOccurrences = View.MinOccurrences;
            currentBkgPackageSvc.BPS_NumberOfYearsOfResidence = View.ResidenceDuration;
            currentBkgPackageSvc.BPS_SendDocumentsToStudent = View.SendDocsToStudent;
            currentBkgPackageSvc.BPS_IsSupplemental = View.IsSupplemental;
            currentBkgPackageSvc.BPS_IgnoreResidentialHistoryOnSupplement = View.IgnoreRHOnSupplement;
            //UAT 1423
            currentBkgPackageSvc.BPS_IsReportable = View.IsReportable;
            
            if (!lstUpdateServiceFormsOverride.IsNullOrEmpty())
            {
                var _bpsId = currentBkgPackageSvc.BPS_ID;
                BackgroundSetupManager.UpdateBkgPackageSvcFormOverride(lstUpdateServiceFormsOverride, _bpsId, View.TenantId);
            }

            if (BackgroundSetupManager.UpdateTenantChanges(View.TenantId))
            {
                View.SuccessMessage = "Package Service Updated successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error has occured.Please contact administrator.";
            }

        }
        #region UAT-583 WB: AMS: Ability to delete attributes and attribute groups from the package setup screen (even after the attribute or attribute group is active)
        /// <summary>
        /// Method to delete mapping of attribute group with service and attribute group.
        /// </summary>
        /// <param name="attributeGroupId"></param>
        /// <param name="bkgPackageSvcId"></param>
        /// <returns></returns>
        public Boolean DeletedBkgSvcAttributeGroupMapping(Int32 attributeGroupId, Int32 bkgPackageSvcId)
        {
            IntegrityCheckResponse responseOrderPlaced = BackgroundServiceIntegrityManager.IfAttributeMappingCanBeDeleted(View.BackgroundPackageId, View.TenantId);
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.IfAttributeGroupAssociatedWithExtSVC(View.TenantId, attributeGroupId, View.BackgroundServiceId, bkgPackageSvcId);
            if (response.CheckStatus == CheckStatus.True || responseOrderPlaced.CheckStatus == CheckStatus.True)
            {
                if (responseOrderPlaced.CheckStatus == CheckStatus.True)
                {
                    responseOrderPlaced.UIMessage = "You cannot delete this attribute group as it is mapped with other objects.";
                }
                if (responseOrderPlaced.CheckStatus == CheckStatus.True)
                    View.ErrorMessage = responseOrderPlaced.UIMessage;
                else if (response.CheckStatus == CheckStatus.True && responseOrderPlaced.CheckStatus == CheckStatus.True)
                    View.ErrorMessage = responseOrderPlaced.UIMessage;
                else if (response.CheckStatus == CheckStatus.True)
                    View.ErrorMessage = response.UIMessage;
                return false;
            }
            else
            {
                return BackgroundSetupManager.DeletedBkgSvcAttributeGroupMapping(View.TenantId, attributeGroupId, bkgPackageSvcId, View.CurrentLoggedInUserId);
            }
        }
        #endregion
        #endregion
    }
}
