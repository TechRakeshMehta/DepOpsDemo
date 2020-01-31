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
    public class AnnouncementPopupPresenter : Presenter<IAnnouncementPopupView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
        }

        /// <summary>
        /// Get Announcement detail
        /// </summary>
        public void GetAnnouncementDetail()
        {
            View.ViewContract = SecurityManager.GetAnnouncementPopupDetail(View.AnnouncementID);
        }

        /// <summary>
        /// Save Announcement Mapping
        /// </summary>
        /// <returns></returns>
        public Boolean SaveAnnouncementMapping()
        {
            return SecurityManager.SaveAnnouncementMapping(View.AnnouncementID, View.LoggedInUserID);
        }
    }
}
