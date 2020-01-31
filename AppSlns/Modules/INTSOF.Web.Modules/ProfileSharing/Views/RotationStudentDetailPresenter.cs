using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using Business.RepoManagers;
using System.Web;
using INTSOF.ServiceUtil;
using INTSOF.Contracts;
using Entity.ClientEntity;
using System.Web.Configuration;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using System.IO;

namespace CoreWeb.ProfileSharing.Views
{
    public class RotationStudentDetailPresenter : Presenter<IRotationStudentDetailView>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        public void GetRotationStudents()
        {
            ServiceRequest<RotationStudentDetailContract, CustomPagingArgsContract> serviceRequest = new ServiceRequest<RotationStudentDetailContract, CustomPagingArgsContract>();
            serviceRequest.Parameter1 = View.RotationStudentDetailContract;
            //UAT-1548
            if (View.IsInstructor)
            {
                View.StudentGridCustomPaging.SortExpression = "ApplicantLastName";
            }
            serviceRequest.Parameter2 = View.StudentGridCustomPaging;
            ServiceResponse<List<ApplicantDataListContract>> _serviceResponse = _clinicalRotationProxy.GetRotationStudents(serviceRequest);
            View.RotationStudentData = _serviceResponse.Result;
            if (!View.RotationStudentData.IsNullOrEmpty())
            {
                View.VirtualRecordCount = View.RotationStudentData[0].TotalCount;
                View.CurrentPageIndex = View.StudentGridCustomPaging.CurrentPageIndex;
            }
            else
            {
                View.VirtualRecordCount = AppConsts.NONE;
            }

            //View.RotationStudentData = new List<ApplicantDataListContract>();
        }

        /// <summary>
        /// Getting Masked SSN
        /// </summary>
        /// <param name="unMaskedSSN"></param>
        /// <returns></returns>
        public String GetMaskedSSN(String unMaskedSSN)
        {
            ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
            serviceRequest.Parameter = unMaskedSSN;
            var _serviceResponse = _clinicalRotationProxy.GetMaskedSSN(serviceRequest);
            return _serviceResponse.Result;
        }

        /// <summary>
        /// Getting Formatted SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetFormattedSSN(String unFormattedSSN)
        {
            ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
            serviceRequest.Parameter = unFormattedSSN;
            var _serviceResponse = _clinicalRotationProxy.GetFormattedSSN(serviceRequest);
            return _serviceResponse.Result;
        }

        /// <summary>
        /// Getting Formatted Phone Number
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetFormattedPhoneNumber(String unFormattedSSN)
        {
            ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
            serviceRequest.Parameter = unFormattedSSN;
            var _serviceResponse = _clinicalRotationProxy.GetFormattedPhoneNumber(serviceRequest);
            return _serviceResponse.Result;
        }

        /// <summary>
        /// Save Invitation Expiration Request
        /// </summary>
        public void SaveInvExpirationRequest()
        {
            List<ApplicantDataListContract> selectedStudentList = View.SelectedStudentList;
            ServiceRequest<List<ApplicantDataListContract>, Int32?> serviceRequest = new ServiceRequest<List<ApplicantDataListContract>, Int32?>();
            serviceRequest.Parameter1 = selectedStudentList;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            var _serviceResponse = _clinicalRotationProxy.UpdateInvitationExpirationRequested(serviceRequest);
        }

        #region UAT-1844:Phase 2 8: Agency User> Rotation tab and Other Tab
        public Boolean SaveUpdateInvitationReviewStatus(String reviewStatusCode, String notes, String screenType, Int32 currentLoggedInUserId)
        {
            List<Int32> invitationIDs = View.SelectedStudentList.Select(slct => slct.ProfileSharingInvID).ToList();

            List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData = new List<Tuple<Int32, Int32, Int32, List<Int32>>>();
            lstRotationData.Add(new Tuple<int, int, int, List<int>>(View.ClinicalRotationId, View.SelectedTenantID, View.AgencyID, invitationIDs));


            Dictionary<String, String> dicStatusCodeAndNotes = new Dictionary<String, String>();
            dicStatusCodeAndNotes.Add(AppConsts.DIC_KEY_REVIEW_STATUS, reviewStatusCode);
            dicStatusCodeAndNotes.Add(AppConsts.DIC_KEY_NOTES, notes);
            dicStatusCodeAndNotes.Add(AppConsts.DIC_KEY_SCREEN_TYPE, screenType);
            dicStatusCodeAndNotes.Add(AppConsts.CURRENT_USER_ID_QUERY_STRING, Convert.ToString(currentLoggedInUserId));

            ServiceRequest<Dictionary<String, String>, List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean> serviceRequest = new ServiceRequest<Dictionary<String, String>, List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean>();
            serviceRequest.Parameter1 = dicStatusCodeAndNotes;
            serviceRequest.Parameter2 = new List<int>(); //selected invitations belong to rotation, so need to maintain dictionary.
            serviceRequest.Parameter3 = lstRotationData;// View.ClinicalRotationIds;
            serviceRequest.Parameter4 = true;// View.ClinicalRotationIds;
            serviceRequest.SelectedTenantId = View.SelectedTenantID;
            var _serviceResponse = _clinicalRotationProxy.UpdateRotAndInvitationReviewStatus(serviceRequest);
            Boolean Result = _serviceResponse.Result;
            if (Result)
            {
                //UAT-2538
                CallParallelTaskForMail(lstRotationData, reviewStatusCode, currentLoggedInUserId);
            }
            return Result;

        }

        public void GetRotationReviewStatus()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.SelectedTenantID;
            var _serviceResponse = _clinicalRotationProxy.GetReviewStatusList(serviceRequest);
            String sharedUserInvDroppedReviewStatusCode = SharedUserInvitationReviewStatus.Dropped.GetStringValue(); //UAT-4460
            View.lstRotationReviewStatus = _serviceResponse.Result.Where(cond => cond.Code != sharedUserInvDroppedReviewStatusCode).ToList(); //UAT-4460
        }

        public Boolean IsNotApprovedReviewStatusSelected(Int32 reviewStatusId)
        {
            Boolean IsNotApprovedReviewStatusSelected = false;
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.SelectedTenantID;
            var _serviceResponse = _clinicalRotationProxy.GetReviewStatusList(serviceRequest);

            List<SharedUserRotationReviewStatusContract> lstSharedUserInvitationReviewStatus = _serviceResponse.Result;
            if (!lstSharedUserInvitationReviewStatus.IsNullOrEmpty())
            {
                SharedUserRotationReviewStatusContract sharedUserInvReviewStatus_NotApproved = lstSharedUserInvitationReviewStatus.FirstOrDefault(x => x.RotationReviewStatusID == reviewStatusId);
                if (!sharedUserInvReviewStatus_NotApproved.IsNullOrEmpty()
                    && String.Compare(sharedUserInvReviewStatus_NotApproved.Code, SharedUserInvitationReviewStatus.NOT_APPROVED.GetStringValue(), true) == 0)
                {
                    IsNotApprovedReviewStatusSelected = true;
                }
            }
            return IsNotApprovedReviewStatusSelected;
        }
        #endregion

        #region UAT-2538
        public void CallParallelTaskForMail(List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData, String InvitataionStatusCode, Int32 CurrentLoggedInUserID)
        {
            String CurrentUserName = View.CurrentUserFirstName + " " + View.CurrentUserLastName;

            Dictionary<String, Object> Data = new Dictionary<String, Object>();
            Data.Add("lstRotationData", lstRotationData);
            Data.Add("InvitataionStatusCode", InvitataionStatusCode);
            Data.Add("CurrentLoggedInUserID", CurrentLoggedInUserID);
            Data.Add("CurrentUserName", CurrentUserName);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            ParallelTaskContext.PerformParallelTask(SendMailForRotInvAppRej, Data, LoggerService, ExceptiomService);
            //   SendMailForRotInvAppRej(Data);
        }



        public void SendMailForRotInvAppRej(Dictionary<String, Object> data)
        {
            List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData = (List<Tuple<Int32, Int32, Int32, List<Int32>>>)(data.GetValue("lstRotationData"));
            String reviewStatusCode = Convert.ToString(data.GetValue("InvitataionStatusCode"));
            Int32 CurrentLoggedInUserID = Convert.ToInt32(data.GetValue("CurrentLoggedInUserID"));
            String CurrentUserName = Convert.ToString(data.GetValue("CurrentUserName"));

            if (!lstRotationData.IsNullOrEmpty())
            {

                foreach (var item in lstRotationData)
                {
                    //Int32 RotationID = item.Item1;
                    //Int32 TenantID = item.Item2;
                    //List<Int32> lstRotationInvitations = item.Item3;
                    // 75_104
                    //Int32 RotationID = Convert.ToInt32(item.Substring(0, item.IndexOf('_')));
                    //Int32 TenantID = Convert.ToInt32(item.Substring(item.IndexOf('_') + 1));

                    ProfileSharingManager.SendMailForRotInvAppRej(lstRotationData, reviewStatusCode, CurrentLoggedInUserID, CurrentUserName);
                    ProfileSharingManager.SendNotificationToApplicantOnRequirementApproved(lstRotationData, reviewStatusCode, CurrentLoggedInUserID, CurrentUserName);
                    // ProfileSharingManager.SendMailForRotInvAppRej(RotationID, TenantID, InvitataionStatusCode, CurrentLoggedInUserID, CurrentUserName, lstRotationInvitations);
                }
            }
        }
        #endregion

        /// <summary>
        /// UAT-2705 Returns Tenant Name.
        /// </summary>
        public void GetTenantName()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            serviceRequest.Parameter1 = SortByName;
            serviceRequest.Parameter2 = clientCode;
            var _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);
            View.TenantName = _serviceResponse.Result.Where(cond => cond.TenantID == View.SelectedTenantID).Select(col => col.TenantName).FirstOrDefault();
        }

        public void GetClientSetting()
        {
            View.ClientSetting = ComplianceDataManager.GetClientSetting(View.SelectedTenantID);
            View.Settings = LookupManager.GetLookUpData<Entity.ClientEntity.lkpSetting>(View.SelectedTenantID).Where(cnd => cnd.IsDeleted == false).ToList();
        }

        public void GetAgencyUserSSN_Setting()
        {
            ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
            serviceRequest.Parameter = View.UserID;
            var _serviceResponse = _clinicalRotationProxy.GetAgencyUserSSN_Permission(serviceRequest);
            View.SSNPermissionCode = _serviceResponse.Result ? EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue() : EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue();
        }

        public List<String> GetScreenColumnsToHide(String GrdCode, Int32 CurrentLoggedInUserId)
        {
            return SecurityManager.GetScreenColumnsToHide(GrdCode, CurrentLoggedInUserId);
        }

        #region UAT-3315
        public void GetCheckedBadgeDocumentsToExport(List<Int32> lstOrganizationUserID)
        {
            String OrganizationUserIds = String.Join(",", lstOrganizationUserID);
            ServiceRequest<String, Int32, Int32> serviceRequest = new ServiceRequest<String, Int32, Int32>();
            serviceRequest.Parameter1 = OrganizationUserIds;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            serviceRequest.Parameter3 = View.SelectedTenantID;
            var _serviceResponse = _clinicalRotationProxy.GetSelectedBadgeDocumentsToExport(serviceRequest);
            View.LstBadgeDocumentContract = _serviceResponse.Result;
        }
        #endregion

        #region UAT-4399
        public byte[] GetRotationDetailSummaryReport(String InvitationId, String TenantId, String ReportName, String tempFilePath, String rotationID, String agencyID)
        {
            byte[] reportContent = ProfileSharingManager.GetRotationDetailSummaryReport(InvitationId, TenantId, ReportName, tempFilePath, rotationID, agencyID);
            return reportContent;
        }
        #endregion
    }
}

