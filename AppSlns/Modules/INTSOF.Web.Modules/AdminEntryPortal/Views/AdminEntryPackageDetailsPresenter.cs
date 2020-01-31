using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
using Entity.ClientEntity;

namespace CoreWeb.AdminEntryPortal.Views
{
    public class AdminEntryPackageDetailsPresenter : Presenter<IAdminEntryPackageDetailsView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public PackageDetailsPresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
           // GetTenantId();

            if (View.DPPSIds != null)
            {
                foreach (string cptype in View.DPPSIds.Keys)
                {
                    int dppsid = View.DPPSIds[cptype];
                    if (View.SelectedPackageDetails == null)
                        View.SelectedPackageDetails = new Dictionary<string, Entity.ClientEntity.DeptProgramPackageSubscription>();

                    View.SelectedPackageDetails.Add(cptype, ComplianceDataManager.GetApplicantPackageDetails(dppsid, View.TenantId));
                }
            }

            View.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(View.TenantId);
            // UAT-3601
            GetOrderReviewLabels();
        }

        /// <summary>
        /// Get the institute Hierarchy Label, when Order review screen is opened
        /// </summary>
        /// <param name="dpmId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public String GetInstituteHierarchyLabel(Int32 dpmId)
        {
            return ComplianceDataManager.GetInstituteHierarchyLabel(dpmId, View.TenantId);
        }

        //public void GetTenantId()
        //{
        //    View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        //}
        public override void OnViewInitialized()
        {

            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetOrderBkgPackageDetails(List<Int32> bkgHierarchyMappingIds)
        {
            View.lstExternalPackages = BackgroundSetupManager.GetOrderBkgPackageDetails(View.TenantId, bkgHierarchyMappingIds);
        }
        #region UAT-3601
        ///<summary>
        ///Get the labels to be displayed for Package Detail And Change package selection
        ///</summary>
        private void GetOrderReviewLabels()
        {
            List<String> lstCodes = new List<String>();
            lstCodes.Add(Setting.ORDERREVIEW_CHANGE_PACKAGE_SELECTION.GetStringValue());
            lstCodes.Add(Setting.ORDERREVIEW_PACKAGE_DETAIL_HEADER.GetStringValue());
            List<ClientSetting> lstClientSetting = ComplianceDataManager.GetClientSettingsByCodes(View.TenantId, lstCodes,View.LanguageCode);
            var _setting = lstClientSetting.FirstOrDefault(x => x.lkpSetting.Code == Setting.ORDERREVIEW_CHANGE_PACKAGE_SELECTION.GetStringValue());
            //if (_setting.IsNullOrEmpty())
            //{
            //    View.ChangePackageSelectionButtonLabel = AppConsts.ORDER_REVIEW_CHANGE_PACKAGE_SELECTION_DEFAULT_LABEL;
            //}
            //else
            //{
            //    View.ChangePackageSelectionButtonLabel = _setting.CS_SettingValueLangugaeSpecific;
            //}
            _setting = lstClientSetting.FirstOrDefault(x => x.lkpSetting.Code == Setting.ORDERREVIEW_PACKAGE_DETAIL_HEADER.GetStringValue());
            if (_setting.IsNullOrEmpty())
            {
                View.PackageDetailHeaderLabel = AppConsts.ORDER_REVIEW_PACKAGE_DETAIL_HEADER_DEFAULT_LABEL;
            }
            else
            {
                View.PackageDetailHeaderLabel = _setting.CS_SettingValueLangugaeSpecific;
            }
        }
        #endregion
        // TODO: Handle other view events and set state in the view
    }
}




