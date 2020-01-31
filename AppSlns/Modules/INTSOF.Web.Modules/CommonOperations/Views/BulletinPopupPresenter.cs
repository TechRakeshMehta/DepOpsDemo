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
    public class BulletinPopupPresenter : Presenter<IBulletinPopupView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
        }

        /// <summary>
        /// Get Bulletin detail
        /// </summary>
        public void GetBulletinDetail()
        {
            View.ViewContract = SecurityManager.GetBulletinPopupDetail(View.BulletinID);
        }

        /// <summary>
        /// Save Bulletin Mapping
        /// </summary>
        /// <returns></returns>
        public Boolean SaveBulletinMapping()
        {
            return SecurityManager.SaveBulletinMapping(View.BulletinID, View.LoggedInUserID);
        }
    }
}
