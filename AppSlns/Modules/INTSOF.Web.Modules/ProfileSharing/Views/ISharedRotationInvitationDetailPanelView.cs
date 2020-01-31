using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ProfileSharing.Views
{
    public interface ISharedRotationInvitationDetailPanelView
    {
        Int32 CurrentLoggedInUserId { get; }

        Int32 SelectedTenantID
        {
            get;
            set;
        }

        #region Tree Properties
        RequirementPackage RequirementPackage
        {
            get;
            set;
        }

        RequirementPackageSubscription Subscription
        {
            get;
            set;
        }

        Int32 RequirementPackageSubscriptionId
        {
            get;
            set;
        }

        List<Int32> SharedRequirementCategoryIdList
        {
            get;
            set;
        }

        List<SharedRequirementSubscriptionCategory> SharedRequirementSubscriptionCategoryList
        {
            get;
            set;
        }

        #endregion

        String RequirementPackageName { set; }

        //UAT-2164
        Int32 RequirementPackageID { get; set; }
        List<BackgroundDocumentPermissionContract> LstIsBackgroundDocumentContract { get; set; }

        String InvitationSharedInfoType
        {
            get;
            set;
        }

        #region SnapShot Data
        SharedRotationInvitationDetailContarct SharedInvitationRequirementSnapShotDetail { get; set; }
        Int32 SnapShotID { get; set; }
        String SnapShotcategoryIds { get; set; }

        #endregion
        //UAt-1572: WB: Kill the "Save Category View" button, should save checked/unchecked automatically. Duplicate other buttons at the top.
        Int32 ProfileSharingInvitationID
        {
            get;
            set;
        }

        /// <summary>
        /// UAT-2414, Create a snapshot on Rotation End Date 
        /// </summary>
        Int32 ClinicalRotationID { get; set; }

        Boolean ShowLiveRotationData { get; set; }
        Boolean IsInstructorPreceptorPackage { get; set; }//UAT-3338
        //UAT-3977
        Boolean IsLiveDataForInstructorPreceptor
        {
            get;
            set;
        }

    }
}
