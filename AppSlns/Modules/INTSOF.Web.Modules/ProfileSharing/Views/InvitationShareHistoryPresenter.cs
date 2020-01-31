using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ProfileSharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ProfileSharing.Views
{
    public class InvitationShareHistoryPresenter : Presenter<IInvitationShareHistory>
    {
        /// <summary>
        /// Method To Get Data to bind Profile Sharing History Grid
        /// </summary>
        public void GetProfileSharingData()
        {
            List<ProfileSharingDataContract> lstProfileSharingData = ProfileSharingManager.GetProfileSharingDataByInvitationId(View.TenantID, View.CurrentInvitationId);
            if (lstProfileSharingData != null && lstProfileSharingData.Count > 0)
                View.LstProfileSharingData = lstProfileSharingData;
            else
                View.LstProfileSharingData = new List<ProfileSharingDataContract>();
        }
    }
}
