using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;

namespace CoreWeb.SearchUI.Views
{
    public class SharedUserSearchDetailsPresenter : Presenter<ISharedUserSearchDetails>
    {
        /// <summary>
        /// Method to get Shared User Invitation Details based on Shared UserID
        /// </summary>
        public void GetSharedUserInvitationDetails()
        {
            View.SharedUserInvitationDetails = ProfileSharingManager.GetSharedUserInvitationDetails(View.SharedUserID);
            if (View.SharedUserInvitationDetails.IsNullOrEmpty())
            {
                View.SharedUserInvitationDetails = new List<INTSOF.UI.Contract.SearchUI.SharedUserSearchInvitationDetailsContract>();
            }
        }

        /// <summary>
        /// Method to get Shared category ids based on invitationid and packagesubscriptionid
        /// </summary>
        /// <param name="packageSubscriptionID"></param>
        public void GetSharedCategoryList(Int32 packageSubscriptionID)
        {
            List<Int32> lstSharedCategoryID = ProfileSharingManager.GetSharedCategoryList(View.TenantID,View.CurrentInvitationId, packageSubscriptionID);
            View.SharedCategoryIDs = String.Empty;
            if (!lstSharedCategoryID.IsNullOrEmpty())
            {
                View.SharedCategoryIDs = String.Join(",", lstSharedCategoryID.Select(x => x.ToString()).ToArray());
            }
        }
    }
}
