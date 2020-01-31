using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;

namespace CoreWeb.ProfileSharing.Views
{
    public class RotationWidgetPresenter : Presenter<IRotationWidgetView>
    {
        public override void OnViewInitialized()
        {
        }

        public override void OnViewLoaded()
        {

        }

        public void GetSharedStudentsPerInstitution()
        {
            View.LstInstitutionProfileContract = ProfileSharingManager.GetSharedStudentsPerInstitution(View.InviteeOrgUserID, View.FromDate, View.ToDate);
        }

        public void GetRotationDashboardData()
        {
            View.lstDashboardRotations = ProfileSharingManager.GetDashBoardRotationData(View.UserId, View.CurrentUserId);
            //View.SharedUserDetails = ProfileSharingManager.GetSharedUserDashboardDetails(View.UserId, true);

            if (View.SharedUserDetails.IsNullOrEmpty() || View.ReloadAllData)
            {
                View.SharedUserDetails = ProfileSharingManager.GetAgencyUserDashboardDetails(View.UserId);
            }
        }

        public void IsInstructorPreceptor()
        {
            List<OrganizationUserTypeMapping> listOrganizationUserTypeMapping = SecurityManager.GetOrganizationUserTypeMapping(View.UserId);
            View.IsInstructorPreceptor = listOrganizationUserTypeMapping.Any(outm => (outm.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Instructor.GetStringValue()) || (outm.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Preceptor.GetStringValue()));
        }

        public List<Entity.SharedDataEntity.lkpDurationOption> GetDurationOptions()
        {
            return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpDurationOption>().Where(cond => !cond.DO_IsDeleted).ToList();
        }
        #region UAT-3220
        public Boolean HideRequirementSharesDetailLink(Guid userID)
        {
            return ClinicalRotationManager.HideRequirementSharesDetailLink(userID);
        }
        #endregion

        #region UAT-3321
        public void GetUsrGuideDocument()
        {
            Entity.SystemDocument userGuideDoc = SecurityManager.GetUserGuideForAgencyUser();
            if (!userGuideDoc.IsNullOrEmpty())
            {
                View.DocumentPath_UserGuide = userGuideDoc.DocumentPath;
                View.FileName_UserGuide = userGuideDoc.FileName;
            }
        }
        #endregion
    }
}
