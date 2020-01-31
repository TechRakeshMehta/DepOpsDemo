using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.CommonControls.Views
{
    public class ProfileExpirationCriteriaPresenter : Presenter<IProfileExpirationCriteriaView>
    {

        /// <summary>
        /// Gets the Lookup Expiration Types i.e. lkpInvitationExpirationType
        /// </summary> 
        public void BindExpirationTypes()
        {
            var _lstExpirationTypes = ProfileSharingManager.GetExpirationTypes();
            View.lstExpirationTypes = _lstExpirationTypes.Where(et => et.Code != InvitationExpirationTypes.NO_EXPIRATION_CRITERIA.GetStringValue()).ToList();
        }


    }
}
