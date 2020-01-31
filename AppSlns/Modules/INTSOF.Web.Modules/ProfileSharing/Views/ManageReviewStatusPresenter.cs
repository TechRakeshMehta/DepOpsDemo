using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Contracts;
using System.Web;
using INTSOF.Utils.CommonPocoClasses;
using Entity.SharedDataEntity;
using INTSOF.ServiceUtil;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;

namespace CoreWeb.ProfileSharing.Views
{
    public class ManageReviewStatusPresenter : Presenter<IManageReviewStatusView>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        public Boolean IsNotApprovedReviewStatusSelected(Int32 reviewStatusId)
        {
            Boolean IsNotApprovedReviewStatusSelected = false;
            List<lkpSharedUserInvitationReviewStatu> lstSharedUserInvitationReviewStatus = LookupManager.GetSharedDBLookUpData<lkpSharedUserInvitationReviewStatu>().ToList();
            if (!lstSharedUserInvitationReviewStatus.IsNullOrEmpty())
            {
                lkpSharedUserInvitationReviewStatu sharedUserInvReviewStatus_NotApproved = lstSharedUserInvitationReviewStatus.FirstOrDefault(x => x.SUIRS_ID == reviewStatusId && !x.SUIRS_IsDeleted);
                if (!sharedUserInvReviewStatus_NotApproved.IsNullOrEmpty()
                    && String.Compare(sharedUserInvReviewStatus_NotApproved.SUIRS_Code, SharedUserInvitationReviewStatus.NOT_APPROVED.GetStringValue(), true) == 0)
                {
                    IsNotApprovedReviewStatusSelected = true;
                }
            }
            return IsNotApprovedReviewStatusSelected;
        }

        public override void OnViewInitialized()
        {
            GetInvitationReviewStatus();
        }
        public bool SaveUpdateReviewStatus()
        {
            lkpSharedUserInvitationReviewStatu lstSharedUserInvitationReviewStatus = LookupManager.GetSharedDBLookUpData<lkpSharedUserInvitationReviewStatu>().Where(cond => !cond.SUIRS_IsDeleted && cond.SUIRS_ID == View.SelectedReviewStatusID).FirstOrDefault();

            List<Int32> lstSelectedInvitationIds = new List<Int32>();


            //Creating dictory containing rotation ids and corresponding invitation ids
            List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData = new List<Tuple<Int32, Int32, Int32, List<Int32>>>();
            Boolean isIndividualReview = true;

            if (!View.RotationID.IsNullOrEmpty())
            {
                List<Int32> InvId = new List<int>();
                InvId.Add(View.ProfileSharingInvitationID);
                List<InvitationIDsDetailContract> lstRotationIdsDetailContract = new List<InvitationIDsDetailContract>();
                lstRotationIdsDetailContract.Add(new InvitationIDsDetailContract { RotationID = Convert.ToInt32(View.RotationID), AgencyID = View.AgencyID, TenantID = View.SelectedTenantID });
                var result = ProfileSharingManager.GetRotationSharedInvitations(lstRotationIdsDetailContract, View.CurrentLoggedInUserId); //If user select multiple rotation(s) from requirement shares screen
                lstRotationData.Add(new Tuple<int, int, int, List<int>>(result[0].Item1, result[0].Item2, result[0].Item3, InvId));
                isIndividualReview = false;
            }
            else
            {
                lstSelectedInvitationIds.Add(View.ProfileSharingInvitationID);
            }

            int tenantId = 0;
            if (!View.SelectedTenantID.IsNullOrEmpty())
                tenantId = View.SelectedTenantID;

            Dictionary<String, String> dicStatusCodeAndNotes = new Dictionary<String, String>();
            dicStatusCodeAndNotes.Add(AppConsts.DIC_KEY_REVIEW_STATUS, !lstSharedUserInvitationReviewStatus.IsNullOrEmpty() ? lstSharedUserInvitationReviewStatus.SUIRS_Code : string.Empty);
            dicStatusCodeAndNotes.Add(AppConsts.DIC_KEY_NOTES, string.Empty);
            dicStatusCodeAndNotes.Add(AppConsts.DIC_KEY_SCREEN_TYPE, AppConsts.REQUIREMENT_SHARES_SCREEN_NAME);//UAT-2463
            dicStatusCodeAndNotes.Add(AppConsts.CURRENT_USER_ID_QUERY_STRING, Convert.ToString(View.CurrentLoggedInUserId));


            ServiceRequest<Dictionary<String, String>, List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean> serviceRequest = new ServiceRequest<Dictionary<String, String>, List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean>();
            serviceRequest.Parameter1 = dicStatusCodeAndNotes;
            serviceRequest.Parameter2 = lstSelectedInvitationIds;
            serviceRequest.Parameter3 = lstRotationData;
            serviceRequest.Parameter4 = isIndividualReview;
            serviceRequest.SelectedTenantId = tenantId;
            var _serviceResponse = _clinicalRotationProxy.UpdateRotAndInvitationReviewStatus(serviceRequest);
            Boolean Result = _serviceResponse.Result;

            if (Result)
            {
                //UAt-2538
                CallParallelTaskForMail(lstRotationData, lstSharedUserInvitationReviewStatus.SUIRS_Code, lstSelectedInvitationIds); // lstSelectedInvitationIds added as per UAT-2942
            }
            return Result;


        }

        public void CallParallelTaskForMail(List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData, String reviewStatusCode, List<Int32> lstSelectedInvitationIds) // lstSelectedInvitationIds added as per UAT-2942
        {
            Int32 CurrentLoggedInUserID = View.CurrentLoggedInUserId;
            String CurrentUserName = View.CurrentUserFirstName + " " + View.CurrentUserLastName;

            Dictionary<String, Object> Data = new Dictionary<String, Object>();
            Data.Add("lstRotationData", lstRotationData);
            Data.Add("CurrentLoggedInUserID", CurrentLoggedInUserID);
            Data.Add("CurrentUserName", CurrentUserName);
            Data.Add("reviewStatusCode", reviewStatusCode);
            Data.Add("lstSelectedInvitationIds", lstSelectedInvitationIds);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            ParallelTaskContext.PerformParallelTask(SendMailForRotInvAppRej, Data, LoggerService, ExceptiomService);
        }
       
        public void SendMailForRotInvAppRej(Dictionary<String, Object> data)
        {
            List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData = (List<Tuple<Int32, Int32, Int32, List<Int32>>>)(data.GetValue("lstRotationData"));
            String reviewStatusCode = Convert.ToString(data.GetValue("reviewStatusCode"));
            Int32 CurrentLoggedInUserID = Convert.ToInt32(data.GetValue("CurrentLoggedInUserID"));
            String CurrentUserName = Convert.ToString(data.GetValue("CurrentUserName"));
            List<Int32> lstSelectedInvitationIds = (List<Int32>)(data.GetValue("lstSelectedInvitationIds"));
            ProfileSharingManager.SendMailForRotInvAppRej(lstRotationData, reviewStatusCode, CurrentLoggedInUserID, CurrentUserName);
            ProfileSharingManager.SendNotificationToApplicantOnRequirementApproved(lstRotationData, reviewStatusCode, CurrentLoggedInUserID, CurrentUserName);

            #region UAT-2942
            if (!lstSelectedInvitationIds.IsNullOrEmpty() && lstSelectedInvitationIds.Count > AppConsts.NONE)
            {
                ProfileSharingManager.SendMailForApprovedApplicantProfileInvitation(lstSelectedInvitationIds, reviewStatusCode, CurrentLoggedInUserID, CurrentUserName);
            }
			#endregion

			#region UAT-3606
			//Notification for Applicant who used profile share with agency dropdown when agency user has marked the share approved
			if (lstRotationData.IsNullOrEmpty())
			{
				ProfileSharingManager.SendNotificationToApplicantOnNonRequirementApproved(lstSelectedInvitationIds, reviewStatusCode, CurrentLoggedInUserID, CurrentUserName);
			}
			#endregion
		}

        //public Boolean SaveInvitationNotes()
        //{
        //    return ProfileSharingManager.UpdateInvitationNotes(View.ProfileSharingInvitationID, View.CurrentLoggedInUserId, View.InvitationNotes);
        //} 
      

        ////UAT-1402:Student name and rotation details should display on the invitation details screen.
        //public void GetOrganizationUserDetail(Int32 orgUserId)
        //{
        //    View.OrganizationUserDetail = SecurityManager.GetOrganizationUser(orgUserId);
        //}

        public void GetInvitationReviewStatus()
        {
            List<lkpSharedUserInvitationReviewStatu> lstSharedUserInvitationReviewStatus = LookupManager.GetSharedDBLookUpData<lkpSharedUserInvitationReviewStatu>().Where(cond => !cond.SUIRS_IsDeleted).ToList();
            String sharedUserInvDroppedReviewStatus = SharedUserInvitationReviewStatus.Dropped.GetStringValue(); //UAT-4460
            View.lstInvitationReviewStatus = lstSharedUserInvitationReviewStatus.Where(cond=>cond.SUIRS_Code != sharedUserInvDroppedReviewStatus).Select(col => new SharedUserInvitationReviewStatusContract
            {
                Code = col.SUIRS_Code,
                Description = col.SUIRS_Description,
                IsDeleted = col.SUIRS_IsDeleted,
                Name = col.SUIRS_Name,
                ReviewStatusID = col.SUIRS_ID
            }).ToList();
        }

        public void GetReviewStatusIDByProfileSharingInvitationID()
        {
            View.SelectedReviewStatusID = ProfileSharingManager.GetReviewStatusIDByProfileSharingInvitationID(View.ProfileSharingInvitationID);
        }

    }
}
