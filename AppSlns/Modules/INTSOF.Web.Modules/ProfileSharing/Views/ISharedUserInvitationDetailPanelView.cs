using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ProfileSharing.Views
{
    public interface ISharedUserInvitationDetailPanelView
    {
        Int32 CurrentLoggedInUserId { get; }
        #region Tree Properties
        Entity.ClientEntity.CompliancePackage CompliancePackage
        {
            get;
            set;
        }

        Entity.ClientEntity.PackageSubscription Subscription
        {
            get;
            set;
        }

        Int32 PackageSubscriptionId
        {
            get;
            set;
        }

        List<Int32> SharedComplianceCategoryIdList
        {
            get;
            set;
        }

        Int32 SelectedTenantID
        {
            get;
            set;
        }

        List<SharedSubscriptionCategory> SharedComplianceCategoryList { get; set; }

        #endregion

        String PackageName { set; }

        String InvitationSharedInfoType
        {
            get;
            set;
        }
        #region SnapShot Data
        SharedUserInvitationDetailContarct SharedInvitationComplianceSnapShotDetail { get; set; }
        Int32 SnapShotID { get; set; }
        String SnapShotcategoryIds { get; set; }

        #endregion

        //UAT-1572:  WB: Kill the "Save Category View" button, should save checked/unchecked automatically. Duplicate other buttons at the top.
        Int32 ProfileSharingInvitationID
        {
            get;
            set;
        }

        //UAT-1610: Add "Institution Hierarchy" column to the student grid in Rotation Details
        String InstitutionHierarchy { set; }
    }
}
