using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public class ManageServiceAttributePresenter : Presenter<IManageServiceAttributeView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetTenants()
        {
            View.ListTenants = ComplianceDataManager.GetMasterAndInstitutionTypeTenants(View.DefaultTenantId);
        }

        public List<BkgSvcAttribute> GetServiceAttributes()
        {
            return BackgroundSetupManager.GetServiceAttributes(View.SelectedTenantId);
        }

        public List<lkpSvcAttributeDataType> GetServiceAttributeDataType()
        {
            List<lkpSvcAttributeDataType> lstServiceAttributeDataTypes = BackgroundSetupManager.GetServiceAttributeDatatype(View.SelectedTenantId);
            lstServiceAttributeDataTypes.Insert(0, new lkpSvcAttributeDataType { SADT_Name = "--Select--", SADT_ID = 0 });
            return lstServiceAttributeDataTypes;
        }

        public Boolean AddServiceAttribute(ServiceAttributeContract ServiceAttributeContract)
        {
            Entity.BkgSvcAttribute serviceAttribute = ServiceAttributeContract.TranslateToMasterEntity();
            BackgroundSetupManager.AddServiceAttribute(serviceAttribute, View.SelectedTenantId);
            return true;
        }

        public Boolean UpdateServiceAttribute(ServiceAttributeContract ServiceAttributeContract)
        {
            Entity.BkgSvcAttribute serviceAttribute = ServiceAttributeContract.TranslateToMasterEntity();
            BackgroundSetupManager.UpdateServiceAttribute(serviceAttribute, View.SelectedTenantId);
            return true;
        }

        public Boolean DeleteServiceAttribute(Int32 serviceAttributeID, Int32 currentUserID)
        {
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.IfServiceAttributeCanBeDeleted(serviceAttributeID, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                BkgSvcAttribute svcAttribute = BackgroundSetupManager.GetServiceAttributeBasedOnAttributeID(serviceAttributeID, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, svcAttribute.BSA_Name);
                return false;
            }
            else
            {
                return BackgroundSetupManager.DeleteServiceAttribute(serviceAttributeID, currentUserID, View.SelectedTenantId);
            }
        }

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.currentLoggedInUserId).Organization.TenantID.Value;
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

        public Boolean IfServiceAttributeCanBeUpdated(Int32 serviceeAttributeID)
        {
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.IfServiceAttributeCanBeUpdated(serviceeAttributeID, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                BkgSvcAttribute svcAttribute = BackgroundSetupManager.GetServiceAttributeBasedOnAttributeID(serviceeAttributeID, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, svcAttribute.BSA_Name);
                return false;
            }
            return true;
        }
    }
}
