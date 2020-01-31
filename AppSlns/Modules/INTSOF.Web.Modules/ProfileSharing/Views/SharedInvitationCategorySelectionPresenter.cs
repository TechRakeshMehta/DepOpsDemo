using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ProfileSharing.Views
{
    public class SharedInvitationCategorySelectionPresenter : Presenter<ISharedInvitationCategorySelectionView>
    {

        #region Methods
        public void GetSharedCategoryList()
        {
            if (View.InvitationGroupTypeCode != ProfileSharingInvitationGroupTypes.ROTATION_SHARING_TYPE.GetStringValue())
            {
                List<ApplicantComplianceCategoryData> tempCategoryDataList = new List<ApplicantComplianceCategoryData>();
                tempCategoryDataList = ProfileSharingManager.GetSharedCategoryList(View.SelectedTenantID, View.PackageSubscriptionId, View.ListSharedCategoryId, View.SnapshotId);

                View.LstCategoryData = tempCategoryDataList.Select(slct => new SharedCategory
                                      {
                                          AppCategoryDataID = slct.ApplicantComplianceCategoryID,
                                          CategoryLabel = slct.ComplianceCategory.CategoryLabel,
                                          CategoryName = slct.ComplianceCategory.CategoryName,
                                          ComplianceCategoryId = slct.ComplianceCategoryID
                                      }).ToList();
            }
            else
            {
                List<ApplicantRequirementCategoryData> tempCategoryDataList = new List<ApplicantRequirementCategoryData>();
                tempCategoryDataList = ProfileSharingManager.GetSharedRequirementCategoryList(View.SelectedTenantID, View.PackageSubscriptionId, View.ListSharedCategoryId, View.SnapshotId, View.IsInstructorPreceptorData);

                View.LstCategoryData = tempCategoryDataList.Select(slct => new SharedCategory
                {
                    AppCategoryDataID = slct.ARCD_ID,
                    CategoryLabel = slct.RequirementCategory.RC_CategoryLabel.IsNull() ? String.Empty : slct.RequirementCategory.RC_CategoryLabel,
                    CategoryName = slct.RequirementCategory.RC_CategoryName,
                    ComplianceCategoryId = slct.ARCD_RequirementCategoryID
                }).ToList();
            }
        }

        //public List<InvitationDocumentContract> GetSharedCategoryDocuments()
        //{
        //    return ProfileSharingManager.GetSharedCategoryDocuments(View.SelectedTenantID, View.PackageSubscriptionId, View.SelectedSharedCategoryIds);
        //}
        #endregion

    }
}
