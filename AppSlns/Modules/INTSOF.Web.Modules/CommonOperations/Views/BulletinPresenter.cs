using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Entity.ClientEntity;
using INTSOF.UI.Contract.PackageBundleManagement;
namespace CoreWeb.CommonOperations.Views
{
    public class BulletinPresenter : Presenter<IBulletinView>
    {
        public override void OnViewLoaded()
        {
            View.ListTenants = SecurityManager.GetTenantList();
        }

        public override void OnViewInitialized()
        {
        }

        public void GetBulletin()
        {
            string institutionIds = String.Join(",", View.SelectedTenantID);
            if (View.IsADBAdmin)
            {
                View.BulletinDetails = SecurityManager.GetBulletin(institutionIds);
            }
            else
            {
                string hierarchyIds = String.Join(",", View.SelectedHierarchyIds);
                View.BulletinDetails = ComplianceDataManager.GetBulletin(View.TenantId, institutionIds, hierarchyIds);
            }
        }

        public bool SaveUpdateBulletin()
        {
            return SecurityManager.SaveUpdateBulletin(View.ViewContract, View.CurrentUserId);
        }

        public bool DeleteBulletin()
        {
            return SecurityManager.DeleteBulletin(View.ViewContract.BulletinID, View.TenantId, View.CurrentUserId, View.IsADBAdmin);
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
            //Int32 currentUserTenantId = GetTenantId();
            //Checked if logged user is admin or not.
            if (View.TenantId == SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}
