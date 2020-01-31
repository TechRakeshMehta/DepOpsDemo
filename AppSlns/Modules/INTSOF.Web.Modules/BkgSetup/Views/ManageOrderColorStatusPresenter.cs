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
    public class ManageOrderColorStatusPresenter : Presenter<IManageOrderColorStatusView>
    {
        public override void OnViewInitialized()
        {           
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }

        public void GetTenants()
        {
            View.lstTenants = ComplianceDataManager.getClientTenant();
        }

        public void GetAllOrderFlags(int? orderFlagID = null)
        {
            List<Int32> alreadySelectedFlags = BackgroundSetupManager.GetInstituteOrderFlags(View.SelectedTenantId).Select(obj=> obj.IOF_OrderFlagID).ToList();

            //Allow Current Flag in list
            if (orderFlagID.HasValue)
                alreadySelectedFlags = alreadySelectedFlags.Where(obj => obj != orderFlagID.Value).ToList();

            List<lkpOrderFlag> lstOrderFlags = BackgroundSetupManager.GetAllOrderFlags(View.SelectedTenantId);
            if (alreadySelectedFlags.IsNotNull() && alreadySelectedFlags.Count > 0)
                lstOrderFlags = lstOrderFlags.Where(obj => !alreadySelectedFlags.Contains(obj.OFL_ID)).ToList();

            View.lstOrderFlags = lstOrderFlags;
        }

        public void GetInstituteOrderFlags()
        {
            View.lstInstitutionOrderFlags = BackgroundSetupManager.GetInstituteOrderFlags(View.SelectedTenantId);
        }

        public void SaveInstitutionOrderFlagDetail()
        {
            InstitutionOrderFlag instituteOrderFlag = new InstitutionOrderFlag();
            instituteOrderFlag.IOF_OrderFlagID = View.ViewContract.SelectedOrderFlagId;
            instituteOrderFlag.IOF_IsSuccessIndicator = View.ViewContract.IsSuccessIndicator;
            instituteOrderFlag.IOF_Description = View.ViewContract.Description;
            instituteOrderFlag.IOF_TenantID = View.SelectedTenantId;
            BackgroundSetupManager.SaveInstitutionOrderFlagDetail(View.SelectedTenantId, instituteOrderFlag, View.CurrentLoggedInUserId);
        }

        public void UpdateInstitutionOrderFlagDetail()
        {
            InstitutionOrderFlag instituteOrderFlag = new InstitutionOrderFlag();
            instituteOrderFlag.IOF_ID = View.SelectedInstitutionOrderFlagId;
            instituteOrderFlag.IOF_OrderFlagID = View.ViewContract.SelectedOrderFlagId;
            instituteOrderFlag.IOF_IsSuccessIndicator = View.ViewContract.IsSuccessIndicator;
            instituteOrderFlag.IOF_Description = View.ViewContract.Description;
            BackgroundSetupManager.UpdateInstitutionOrderFlagDetail(View.SelectedTenantId, instituteOrderFlag, View.CurrentLoggedInUserId);
        }

        public Boolean DeleteInstitutionOrderFlag()
        {

            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.CheckIfInstitutionOrderFlagBkgOrderMappingExist(View.SelectedTenantId, View.SelectedInstitutionOrderFlagId);
            if (response.CheckStatus == CheckStatus.True)
            {
                InstitutionOrderFlag institutionOrderFlag = BackgroundSetupManager.GetCurrentInstitutionOrderFlag(View.SelectedTenantId, View.SelectedInstitutionOrderFlagId);
                View.ErrorMessage = String.Format(response.UIMessage, institutionOrderFlag.lkpOrderFlag.OFL_Name);
                return false;
            }
            else
            {
                Boolean result = BackgroundSetupManager.DeleteInstitutionOrderFlag(View.SelectedTenantId, View.SelectedInstitutionOrderFlagId, View.CurrentLoggedInUserId); ;
                if (!result)
                    View.ErrorMessage = "Order Status Color/Flag can not be removed. Try again!";
                return result;
            }
        }

    }
}
