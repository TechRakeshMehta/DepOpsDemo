using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;

namespace CoreWeb.ProfileSharing.Views
{
    public class SetInvitationExpiryPresenter : Presenter<ISetInvitationExpiry>
    {
        public void SaveUpdateProfileExpirationCriteria()
        {
            View.Success = ProfileSharingManager.SaveUpdateProfileExpirationCriteria(View.ExpirationCriteriaDetail, View.SelectedInvitationIds);
        }
    }
}
