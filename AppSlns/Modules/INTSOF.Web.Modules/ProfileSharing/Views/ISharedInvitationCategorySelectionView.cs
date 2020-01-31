
using System;
using System.Collections.Generic;

namespace CoreWeb.ProfileSharing.Views
{
    public interface ISharedInvitationCategorySelectionView
    {

        Int32 SelectedTenantID { get; set; }

        Int32 PackageSubscriptionId { get; set; }

        List<SharedCategory> LstCategoryData { set; }
        String SharedCategoryIds { get; set; }
        List<Int32> ListSharedCategoryId { get; }
        Int32 SnapshotId { get; set; }
        //String SelectedSharedCategoryIds { get; set; }
        String InvitationGroupTypeCode
        {
            get;
            set;
        }
        Boolean IsInstructorPreceptorData { get; set; }//UAT-3338
    }
}


public class SharedCategory
{
    public String CategoryName { get; set; }
    public String CategoryLabel { get; set; }
    public Int32 ComplianceCategoryId { get; set; }
    public Int32 AppCategoryDataID { get; set; }
}
