using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ProfileSharing;
using Entity.SharedDataEntity;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IManageReviewStatusView
    {
        Int32 CurrentLoggedInUserId { get; }
        Int32 ProfileSharingInvitationID
        {
            get;
            set;
        }

        Int32 SelectedTenantID
        {
            get;
            set;
        }

        //List<SharedComplianceSubscription> ListSharedPackageSubscription
        //{
        //    get;
        //    set;
        //}

        //List<SharedInvitationBackgroundDetail> ListSharedBackgroundPackage
        //{
        //    get;
        //    set;
        //}

        //ProfileSharingInvitation ProfileSharingInvitation
        //{
        //    get;
        //    set;
        //}

        //String InvitationNotes
        //{
        //    get;
        //    set;
        //}

        //String InvitationGroupTypeCode
        //{
        //    get;
        //    set;
        //}

        //SharedRequirementSubscription SharedRequirementSubscription
        //{
        //    get;
        //    set;
        //}

        String RotationID
        {
            get;
            set;
        }

        //String InvitationGroupID
        //{
        //    get;
        //    set;
        //}

     //   List<SharedInvitationSubscriptionContract> LstSharedInvitationSubscriptionContract { get; set; }

        //UAT-1402:Student name and rotation details should display on the invitation details screen.
     //   Entity.OrganizationUser OrganizationUserDetail { get; set; }

        Int32 AgencyID { get; set; }

        Int32 OrganisationUserID { get; }

        //UAT-2774
      //  List<SharedUserInvitationDocumentContract> lstSharedUserInvitationDocumentContract { get; set; }
      //  List<SharedUserInvitationDocumentMapping> lstSharedUserInvitationDocument { get; set; }
      //  Dictionary<Int32, Boolean> lstSelectedDocumentIds { get; set; }



        List<SharedUserInvitationReviewStatusContract> lstInvitationReviewStatus //UAT-2943
        {
            set;
        }
        Int32 SelectedReviewStatusID { get; set; }  //UAT-2943
        String CurrentUserFirstName//UAT-2943
        {
            get;
        }
        String CurrentUserLastName//UAT-2943
        {
            get;
        }
    }
}
