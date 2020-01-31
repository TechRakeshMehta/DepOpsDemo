using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IAgencyUserNotesPopupView
    {
        Int32 SelectedTenantID
        {
            get;
            set;
        }

        String InvitationIDs
        {
            get;
            set;
        }

        String ClinicalRotationIds
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        String ScreenType
        {
            get;
            set;
        }

        String RotationInvitationIds
        {
            get;
            set;
        }
        //UAT-2538
        String CurrentUserFirstName
        { get; }
        String CurrentUserLastName
        { get; }
        Int32 OrganisationUserID { get; }

    }
}
