using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using Business.RepoManagers;
using INTSOF.UI.Contract.ProfileSharing;
using System.Web;
using INTSOF.Contracts;
using INTSOF.ServiceUtil;

namespace CoreWeb.ProfileSharing.Views
{
    public class AgencyUserNotesPopupPresenter : Presenter<IAgencyUserNotesPopupView>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        #region UAT-1844:Phase 2 8: Agency User> Rotation tab and Other Tab
        public void SaveUpdateInvitationReviewStatus(String reviewStatusCode, List<Int32> invitationIDs, String notes, List<Tuple<Int32, Int32, Int32>> rotations, List<Int32> lstRotationInvitations)
        {
            //Creating dictory containing rotation ids and corresponding invitation ids
            List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData = new List<Tuple<Int32, Int32, Int32, List<Int32>>>();
            Boolean isIndividualReview = true;

            if (!rotations.IsNullOrEmpty() && lstRotationInvitations.IsNullOrEmpty())
            {
                List<InvitationIDsDetailContract> tempRotationIdsDetailContract = rotations.Select(con => new InvitationIDsDetailContract { RotationID = con.Item1, TenantID = con.Item2, AgencyID = con.Item3 }).ToList();//Cast Tuple into Contract list as per the requirement of UAT:2475
                lstRotationData = ProfileSharingManager.GetRotationSharedInvitations(tempRotationIdsDetailContract, View.OrganisationUserID); //If user select multiple rotation(s) from requirement shares screen
                isIndividualReview = false;
            }
            else if (!rotations.IsNullOrEmpty() && rotations.Count == 1 && !lstRotationInvitations.IsNullOrEmpty())
                lstRotationData.Add(new Tuple<int, int, int, List<int>>(rotations[0].Item1, rotations[0].Item2, rotations[0].Item3, lstRotationInvitations));//If user select invitations from rotation details screen


            Dictionary<String, String> dicStatusCodeAndNotes = new Dictionary<String, String>();
            dicStatusCodeAndNotes.Add(AppConsts.DIC_KEY_REVIEW_STATUS, reviewStatusCode);
            dicStatusCodeAndNotes.Add(AppConsts.DIC_KEY_NOTES, notes);
            dicStatusCodeAndNotes.Add(AppConsts.DIC_KEY_SCREEN_TYPE, AppConsts.REQUIREMENT_SHARES_SCREEN_NAME);//UAT-2463
            dicStatusCodeAndNotes.Add(AppConsts.CURRENT_USER_ID_QUERY_STRING, Convert.ToString(View.CurrentLoggedInUserId));
            ServiceRequest<Dictionary<String, String>, List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean> serviceRequest = new ServiceRequest<Dictionary<String, String>, List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean>();
            serviceRequest.Parameter1 = dicStatusCodeAndNotes;
            serviceRequest.Parameter2 = invitationIDs;
            serviceRequest.Parameter3 = lstRotationData;
            serviceRequest.Parameter4 = isIndividualReview;
            serviceRequest.SelectedTenantId = View.SelectedTenantID;
            var _serviceResponse = _clinicalRotationProxy.UpdateRotAndInvitationReviewStatus(serviceRequest);
            if (!_serviceResponse.IsNullOrEmpty())
            {
                //UAT-2538
                CallParallelTaskForMail(lstRotationData, reviewStatusCode);
            }

        }

        #endregion

        #region UAT-2538
        public void CallParallelTaskForMail(List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData, String reviewStatusCode)
        {
            Int32 CurrentLoggedInUserID = View.CurrentLoggedInUserId;
            String CurrentUserName = View.CurrentUserFirstName + " " + View.CurrentUserLastName;

            Dictionary<String, Object> Data = new Dictionary<String, Object>();
            Data.Add("lstRotationData", lstRotationData);
            Data.Add("CurrentLoggedInUserID", CurrentLoggedInUserID);
            Data.Add("CurrentUserName", CurrentUserName);
            Data.Add("reviewStatusCode", reviewStatusCode);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            ParallelTaskContext.PerformParallelTask(SendMailForRotInvAppRej, Data, LoggerService, ExceptiomService);

            // SendMailForRotInvAppRej(Data);

        }
        public void SendMailForRotInvAppRej(Dictionary<String, Object> data)
        {
            List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData = (List<Tuple<Int32, Int32, Int32, List<Int32>>>)(data.GetValue("lstRotationData"));
            String reviewStatusCode = Convert.ToString(data.GetValue("reviewStatusCode"));
            Int32 CurrentLoggedInUserID = Convert.ToInt32(data.GetValue("CurrentLoggedInUserID"));
            String CurrentUserName = Convert.ToString(data.GetValue("CurrentUserName"));

            if (!lstRotationData.IsNullOrEmpty())
            {
                ProfileSharingManager.SendMailForRotInvAppRej(lstRotationData, reviewStatusCode, CurrentLoggedInUserID, CurrentUserName);
                //foreach (var item in lstRotationData)
                //{
                //    Int32 RotationID = item.Item1;
                //    Int32 TenantID = item.Item2;
                //    List<Int32> lstRotationInvitations = item.Item3;

                //    ProfileSharingManager.SendMailForRotInvAppRej(RotationID, TenantID, reviewStatusCode, CurrentLoggedInUserID, CurrentUserName, lstRotationInvitations);
                //}
            }
        }
        #endregion

    }
}
