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
    public class AnnouncementPresenter : Presenter<IAnnouncementView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {

        }

        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantID);
        }

        /// <summary>
        /// Get Announcement details
        /// </summary>
        public void GetAnnouncementDetail()
        {
            View.AnnouncementDetails = SecurityManager.GetAnnouncementDetail();
        }

        /// <summary>
        /// Save Update Announcement
        /// </summary>
        /// <returns></returns>
        public bool SaveUpdateAnnouncement()
        {
            return SecurityManager.SaveUpdateAnnouncement(View.ViewContract, View.CurrentUserId);
        }

        /// <summary>
        /// Delete Announcement
        /// </summary>
        public void DeleteAnnouncement()
        {
            SecurityManager.DeleteAnnouncement(View.ViewContract.AnnouncementID, View.CurrentUserId);
        }
    }
}
