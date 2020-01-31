using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.AgencyJobBoard;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.DashBoard.Views
{
    public class ApplicantDashboardMainPresenter : Presenter<IApplicantDashboardMainView>
    {
        public OrganizationUser GetUserData()
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.ApplicantId);
            return organizationUser;
        }

        #region UAT-2302-As a student, I should see a popup on login if i have logged in using a non-preferred browser.
        /// <summary>
        ///  On the basis of Applicant-ID and Non-Preferred enum code we are checking the user has opted the option of non-prferred browser
        /// </summary>
        /// <returns></returns>
        public Boolean CheckNonPrefferedBrowserOption()
        {
            Boolean chkNonPrefferedBrowserOption = SecurityManager.CheckUserOptedNonPrefferedBrowserOption(View.ApplicantId, UtilityFeatures.NonPrefferedBrowser.GetStringValue());
            View.IsDisplayNonPreferredOption = chkNonPrefferedBrowserOption;
            return chkNonPrefferedBrowserOption;
        }

        #endregion

        /// <summary>
        /// Method to update the Profile Photo.
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean SaveProfilePhoto()
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.ApplicantId);
            //organizationUser.ModifiedByID = View.ApplicantId;
            organizationUser.ModifiedByID = View.OrgUsrID;
            organizationUser.ModifiedOn = DateTime.Now;
            if (View.FilePath != null)
            {
                organizationUser.PhotoName = View.FilePath;
            }
            if (View.OriginalFileName != null)
            {
                organizationUser.OriginalPhotoName = View.OriginalFileName;
            }
            if (SecurityManager.SyncUsersProfilePictureInAllTenant(organizationUser))
            {
                return true;
            }
            return false;
        }

        public void GetUserSubscribePackages()
        {
            List<Entity.ClientEntity.PackageSubscription> lstPackageSubscriptions = ComplianceDataManager.GetSubscribedPackagesForUser(View.ApplicantTenantId, View.ApplicantId);
            View.SubscribedPackagesAll = lstPackageSubscriptions;
        }

        //UAT-4067
        public void GetSelectedNodeIDByOrderID()
        {
            List<Entity.ClientEntity.PackageSubscription> lstSelectedNodeIDForOrders = ComplianceDataManager.GetSelectedNodeIDByOrderID(View.ApplicantTenantId, View.ApplicantId);
            View.selectedNodeIDs = lstSelectedNodeIDForOrders.Where(x => !x.IsDeleted).DistinctBy(x => x.Order.SelectedNodeID).Select(x => x.Order.SelectedNodeID ?? 0).ToList();
        }
        public void GetAllowedFileExtensions()
        {
            String selectedNodeIDs = String.Join(",", View.selectedNodeIDs);
            var lstAllowedFileExtensions = ComplianceDataManager.GetAllowedFileExtensionsByNodeIDs(View.ApplicantTenantId, selectedNodeIDs);
            View.allowedFileExtensions = lstAllowedFileExtensions.Select(x => x.Name).ToList();
        }

        public void GetSelectedNodeIDBySubscriptionID(Int32 selectedtenantID, Int32 packageSubscriptionID)
        {
            var lstSelectedNodeIDForOrders = ComplianceDataManager.GetSelectedNodeIDBySubscriptionID(selectedtenantID, packageSubscriptionID);
            View.selectedNodeIDs = lstSelectedNodeIDForOrders.Where(x => !x.IsDeleted).DistinctBy(x => x.Order.SelectedNodeID).Select(x => x.Order.SelectedNodeID ?? 0).ToList();
        }

        public void GetUserSubscribeBkgPackages()
        {
            List<Entity.ClientEntity.vwOrderDetail> lstAllBkgOrder = ComplianceDataManager.GetOrderDetailList(View.ApplicantTenantId, View.ApplicantId, new List<String>())
                .Where(x => !x.OrderPackageTypeCode.Contains("AAAA") && !x.OrderPackageTypeCode.Contains("AAAE") && !x.OrderPackageTypeCode.Contains("AAAF")).ToList();
            //if (lstAllBkgOrder != null && lstAllBkgOrder.Count() > 0)
            var SubmittedToCBI = FingerPrintSetUpManager.GetAllAppointmentStatus().ToList().Where(t => t.AS_Code == FingerPrintAppointmentStatus.SUBMITTED_TO_CBI.GetStringValue()).FirstOrDefault().AS_Name;
            foreach (var item in lstAllBkgOrder)
            {
                if (item.AppointmentStatusCode == FingerPrintAppointmentStatus.TECHNICAL_REVIEW.GetStringValue())
                {
                    item.AppointmentStatus = SubmittedToCBI;
                }
            }

            if (!lstAllBkgOrder.IsNullOrEmpty())
            {
                View.LstAllBkgOrderDetail = lstAllBkgOrder;                             
            }
            else
            {
                View.LstAllBkgOrderDetail = new List<Entity.ClientEntity.vwOrderDetail>();
            }
        }

        public void SetVideoPopupSetting()
        {
            var _lstCompliancePkgIds = View.SubscribedPackagesAll.Select(x => x.CompliancePackageID).ToList();
            //Check that document is uploaded or not
            View.ShowDocumentVideo = ComplianceDataManager.ShowDocumentVideo(View.ApplicantTenantId, View.ApplicantId, _lstCompliancePkgIds);
            if (View.ShowDocumentVideo == AppConsts.ALREADY_UPLOADED_DOC)
            {
                //if doc is uploaded then check for data entered or not.
                View.ShowDataEnteredVideo = ComplianceDataManager.ShowDataEnteredVideo(View.ApplicantTenantId, View.ApplicantId);
            }
        }

        //public void GetClientSettingDetails()
        //{
        //    if (SecurityManager.DefaultTenantID != View.ApplicantTenantId)
        //    {
        //        List<Entity.ClientEntity.ClientSetting> clientSettingDetails = ComplianceDataManager.GetClientSetting(View.ApplicantTenantId);
        //        if (clientSettingDetails.Count > 0)
        //        {
        //            String clientSettingBeforeExpiry = String.Empty;
        //            if (clientSettingDetails.Any(cond => cond.lkpSetting.Code == Setting.Reminder_Before_Expiry.GetStringValue()))
        //            {
        //                clientSettingBeforeExpiry = clientSettingDetails.Where(cond => cond.lkpSetting.Code == Setting.Reminder_Before_Expiry.GetStringValue()).FirstOrDefault().CS_SettingValue;
        //            }
        //            View.ClientSettingBeforeExpiry = clientSettingBeforeExpiry.IsNullOrEmpty() ? 0 : Convert.ToInt32(clientSettingBeforeExpiry);
        //        }
        //    }
        //}
        public void GetSubscriptionFrequency()
        {
            if(SecurityManager.DefaultTenantID!=View.ApplicantTenantId)
            {
                View.lstSubscriptionFrequency = ComplianceDataManager.GetSubscriptionNotificationFrequencyDays(View.ApplicantTenantId);
            }
        }

        /// <summary>
        /// Method to check for valid subscription renewal
        /// </summary>
        /// <param name="orderDetail">orderDetail</param>
        /// <returns>Boolean</returns>
        public Boolean IsSubscriptionRenewalValid(Entity.ClientEntity.Order orderDetail)
        {
            List<Entity.ClientEntity.Order> orderList = ComplianceDataManager.GetOrderListOfPreviousOrder(View.ApplicantTenantId, orderDetail);
            if (orderList.Count > 0)
            {
                Int32 totalSubscriptionYear = Convert.ToInt32(orderList.Where(cnd => cnd.SubscriptionYear != null).Sum(x => x.SubscriptionYear));
                Int32 totalSubscriptionMonth = Convert.ToInt32(orderList.Where(cnd => cnd.SubscriptionMonth != null).Sum(x => x.SubscriptionMonth));
                Int32 subscriptionDuration = (totalSubscriptionYear * 12) + totalSubscriptionMonth;
                Int32 remainingProgDuration = Convert.ToInt32(orderDetail.ProgramDuration - subscriptionDuration);
                if (subscriptionDuration <= remainingProgDuration)
                    return true;
            }
            return false;
        }

        public Boolean IsPackageChangeSubscription(Int32 orderID)
        {
            List<Entity.ClientEntity.Order> orderList = ComplianceDataManager.GetChangeSubscriptionOrderList(View.ApplicantTenantId, orderID);
            if (orderList.IsNotNull() && orderList.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public void GetCurrentSubscribePackageDetail()
        {
            List<Entity.ClientEntity.PackageSubscription> lstPackageSubscriptionOfUser = ComplianceDataManager.GetSubscribedPackagesForUser(View.ApplicantTenantId, View.ApplicantId);
            var currentPackageSubscription = lstPackageSubscriptionOfUser.FirstOrDefault(cond => cond.PackageSubscriptionID == View.SelectedPkgSubscriptionId);
            List<Entity.ClientEntity.PackageSubscription> lstPackageSubscriptions = new List<Entity.ClientEntity.PackageSubscription>();
            lstPackageSubscriptions.Add(currentPackageSubscription);
            View.SubscribedPackagesAll = lstPackageSubscriptions;
            View.IfUserhasImmunizationPkg = lstPackageSubscriptionOfUser.Any(cond => cond.CompliancePackage.CompliancePackageTypeID == ImmunizationPkgTypeId);
            View.IfUserHasAdministrativePkg = lstPackageSubscriptionOfUser.Any(cond => cond.CompliancePackage.CompliancePackageTypeID == AdministrativePkgTypeId);
        }

        public Int32 ImmunizationPkgTypeId
        {
            get
            {
                return ComplianceDataManager.GetCompliancePkgTypeId(CompliancePackageTypes.IMMUNIZATION_COMPLIANCE_PACKAGE.GetStringValue(), View.SelectedTenantId);
            }
        }
        public Int32 AdministrativePkgTypeId
        {
            get
            {
                return ComplianceDataManager.GetCompliancePkgTypeId(CompliancePackageTypes.ADMINISTRATIVE_COMPLIANCE_PACKAGE.GetStringValue(), View.SelectedTenantId);
            }
        }

        /// <summary>
        /// UAT-1278: Student-side updates for Clinical Roatations: Update left tab and the resulting grid screen
        /// </summary>
        public void GetApplicantClinicalRotationMember()
        {
            View.IsApplicantClinicalRotationMember = ApplicantClinicalRotationManager.IsApplicantClinicalRotationMember(View.ApplicantTenantId, View.ApplicantId);
        }

        /// <summary>
        /// Get Bulletins List
        /// </summary>
        public void GetBulletins()
        {
            View.lstBulletinID = ComplianceDataManager.GetBulletins(View.ApplicantTenantId, View.ApplicantId);
        }

        #region UAT-1812
        /// <summary>
        /// Get Bulletins List
        /// </summary>
        public Boolean IsSameUserLoggedIn()
        {
            Entity.UserLoginHistory userLoginHistory = SecurityManager.GetApplicantLastLoginDetail(View.ApplicantId, String.Empty).Last();
            if (!userLoginHistory.IsNullOrEmpty() && String.Compare(View.CurrentSessionId, userLoginHistory.ULH_SessionID, true) == 0)
            {
                return true;
            }
            return false;
        }

        public Boolean IsAnyPackageExistForUser()
        {
            Boolean isAnyPackageExistForUser = false;
            var tupleData = ComplianceSetupManager.GetAppSummaryDataAfterLastLogin(View.ApplicantId, View.ApplicantTenantId, GetApplicantLastLoginTime());
            if (View.SubscribedPackagesAll.Any() || View.IfUserhasImmunizationPkg || View.IfUserHasAdministrativePkg || (!tupleData.IsNullOrEmpty() && !tupleData.Item2.IsNullOrEmpty()))
            {
                if (!tupleData.IsNullOrEmpty() && (!tupleData.Item2.IsNullOrEmpty() || !tupleData.Item1.IsNullOrEmpty()))
                {
                    isAnyPackageExistForUser = true;
                }
            }
            return isAnyPackageExistForUser;
        }

        public Boolean IsApprovedRotationExistsForUser()
        {
            Boolean isApprovedRotationExistsForUser = false;
            DateTime? lastLoginDate = GetApplicantLastLoginTime();
            var lstApprovedRotations = ProfileSharingManager.GetApprovedRotationsAfterSinceLastLogin(View.ApplicantId, View.ApplicantTenantId, lastLoginDate.HasValue ? lastLoginDate.Value : new DateTime(1990, 1, 1));

            if (!lstApprovedRotations.IsNullOrEmpty() && lstApprovedRotations.Count > 0)
                isApprovedRotationExistsForUser = true;

            return isApprovedRotationExistsForUser;
        }

        /// <summary>
        /// Get ApplicantLast login detail
        /// </summary>
        public DateTime? GetApplicantLastLoginTime()
        {
            DateTime? UserLastLoginTime = null;
            List<Entity.UserLoginHistory> userLoginHistorydataList = SecurityManager.GetApplicantLastLoginDetail(View.ApplicantId, String.Empty);
            if (!userLoginHistorydataList.IsNullOrEmpty())
            {
                Entity.UserLoginHistory userLoginHistorydata = userLoginHistorydataList.OrderByDescending(ord => ord.ULH_ID).Skip(1).Take(1).FirstOrDefault();
                if (!userLoginHistorydata.IsNullOrEmpty())
                {
                    UserLastLoginTime = userLoginHistorydata.ULH_LogoutTime.IsNullOrEmpty() ? userLoginHistorydata.ULH_LoginTime : userLoginHistorydata.ULH_LogoutTime;
                    UserLastLoginTime = DateTime.SpecifyKind(UserLastLoginTime.Value, DateTimeKind.Utc).ToLocalTime();
                }
            }
            return UserLastLoginTime;
        }
        #endregion

        /// <summary>
        /// //UAT-1834:NYU Migration 2 of 3: Applicant Complete Order Process
        /// </summary>
        public void GetBulkOrderForApplicant()
        {
            View.BulkOrderData = ComplianceDataManager.GetBulkOrderForApplicant(View.ApplicantTenantId, View.ApplicantId);
        }

        #region UAT-2003: Add ability to extend/renew when clicking "place order"

        public void GetSubscriptionList()
        {
            if (SecurityManager.DefaultTenantID != View.ApplicantTenantId)
            {
                View.ListSubscription = ComplianceDataManager.GetSubscriptionList(View.ApplicantTenantId, View.ApplicantId).ToList();
            }
        }

        /// <summary>
        /// Method to check the subscription custom for total price null or 0
        /// </summary>
        /// <param name="orderId">orderId</param>
        /// <returns></returns>
        public Boolean IsCustomPriceSetAndSubsRenewalValid(Int32 orderId)
        {
            Guid subscriptionOptionCode = new Guid(SubscriptionOptions.CustomMonthly.GetStringValue());
            Entity.ClientEntity.Order orderDetail = ComplianceDataManager.GetOrderById(View.ApplicantTenantId, orderId);
            //&& cond.DPPS_TotalPrice != 0 include packages with 0 price :[UAT-360]:- WB: Compliance Order: Applicant receives error message when attempting to purchase a subscription with the price of $0.00
            if (orderDetail.DeptProgramPackage.IsNotNull() && orderDetail.DeptProgramPackage.DeptProgramPackageSubscriptions.IsNotNull())
            {
                Entity.ClientEntity.DeptProgramPackageSubscription depProgramPackageSubscription = orderDetail.DeptProgramPackage.DeptProgramPackageSubscriptions.Where(cond => cond.SubscriptionOption.Code == subscriptionOptionCode && cond.SubscriptionOption.IsSystem && !cond.DPPS_IsDeleted && (cond.DPPS_TotalPrice != null)).FirstOrDefault();
                if (depProgramPackageSubscription != null && depProgramPackageSubscription.DPPS_TotalPrice != null && ((orderDetail != null && (orderDetail.ProgramDuration ?? 0) <= 0) || IsSubscriptionRenewalValid(orderDetail)))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        /// <summary>
        /// UAT-2218, Creation of a "Required Documents" tab on the left side of the student dashboard. 
        /// </summary>
        public void GetRequirementDocumentsDetails()
        {
            View.IsRequiredTabVisible = (ComplianceDataManager.GetRequirementDocumentsDetails(View.ApplicantTenantId, View.ApplicantId).IsNullOrEmpty() 
                                         && ComplianceDataManager.GetRotReqDocumentsDetails(View.ApplicantTenantId,View.ApplicantId).IsNullOrEmpty()) ? false : true;  //Changed condition relative to UAT-3161
        }

        public void GetClientSettingForPersonalDocTab()
        {
            List<String> lstCodes = new List<String>();
            lstCodes.Add(Setting.APPLICANT_PERSONAL_DOCUMENT_TAB.GetStringValue());
            List<Entity.ClientEntity.ClientSetting> lstClientSetting = ComplianceDataManager.GetClientSettingsByCodes(View.ApplicantTenantId, lstCodes);
            if (lstClientSetting.IsNullOrEmpty() || lstClientSetting.FirstOrDefault().CS_SettingValue == "0")
            {
                View.ShowSeparateTabForApplicantPersonalDocs = false;
            }
            else
            {
                View.ShowSeparateTabForApplicantPersonalDocs = true;
            }
        }

        //UAT-2251 : Ability to turn on/off the initial student videos individually and to replace the video link by institution
        public void GetClientSetting(String settingCode)
        {
            var clientsetting = ComplianceDataManager.GetClientSetting(View.ApplicantTenantId, settingCode);
            if (!clientsetting.IsNullOrEmpty())
            {
                View.ClientSettingForDisplayInitialVideo = clientsetting.CS_SettingValue == "1" ? true : false;
            }
            else
            {
                View.ClientSettingForDisplayInitialVideo = true;
            }
        }
        public String GetClientSettingValue(String settingCode)
        {
            var setting = ComplianceDataManager.GetClientSetting(View.ApplicantTenantId, settingCode, View.LanguageCode);
            if(!setting.IsNullOrEmpty())
            {
                //return setting.CS_SettingValue;
                return setting.CS_SettingValueLangugaeSpecific;
            }
            return null;
        }
        //UAT-2251 : Ability to turn on/off the initial student videos individually and to replace the video link by institution
        public List<Entity.ClientEntity.ClientSetting> GetClientSettingsByCodes()
        {
            return ComplianceDataManager.GetClientSetting(View.ApplicantTenantId);
        }

        #region UAT-2427

        private AgencyHierarchyProxy _agencyHierarchyProxy
        {
            get
            {
                return new AgencyHierarchyProxy();
            }
        }

        public void GetAgencyJobPosting()
        {
            JobSearchContract searchContract = GetJobSearchContract();
            CustomPagingArgsContract customContract = new CustomPagingArgsContract();
            customContract.PageSize = AppConsts.ONE;
            customContract.CurrentPageIndex = AppConsts.ONE;
            ServiceRequest<JobSearchContract, CustomPagingArgsContract> serviceRequest = new ServiceRequest<JobSearchContract, CustomPagingArgsContract>();

            serviceRequest.Parameter1 = searchContract;
            serviceRequest.Parameter2 = customContract;
            var lstData = _agencyHierarchyProxy.GetViewAgencyJobPosting(serviceRequest).Result;
            if (!lstData.IsNullOrEmpty())
                View.IsJobBoardMenuVisible = true;
            else
                View.IsJobBoardMenuVisible = false;
        }

        private JobSearchContract GetJobSearchContract()
        {
            JobSearchContract contract = new JobSearchContract();
            contract.JobTitle = string.Empty;
            contract.JobTypeCode = string.Empty;
            contract.OrganizationUserId = View.OrgUsrID;
            contract.TenantId = Convert.ToString(View.ApplicantTenantId);
            return contract;
        }

        #endregion

        //UAT-2930
        public Boolean DeleteTwofactorAuthenticationForUserID()
        {
            return SecurityManager.DeleteTwofactorAuthenticationForUserID(View.UserId, View.CurrentLoggedInUserID);
        }
        //UA-2960
        public Boolean IsNeedToShowAlumniPopUp()
        {
            if (IsNeedToShowOntheBasisofClientSettings())
            {
                String statusDueCode = lkpAlumniStatus.Due.GetStringValue();
                return AlumniManager.AlumniAccessStatus(View.OrgUsrID, View.IsAdminView ?View.ApplicantTenantId:View.SelectedTenantId, statusDueCode);
            }
            return IsNeedToShowOntheBasisofClientSettings();
        }
        public Boolean IsNeedToShowProfileSharingNReportTab()
        {
            String AlumniTenantCode = AlumniSettings.AlumniTenantID.GetStringValue();
            String targetTenantID = AlumniManager.GetAlumniSettingByCode(AlumniTenantCode);
            String AlumniPackageCode = AlumniSettings.DefaultAlumniPackageID.GetStringValue();
            String targetPackageID = AlumniManager.GetAlumniSettingByCode(AlumniPackageCode);

            if (!String.IsNullOrEmpty(targetTenantID) && !String.IsNullOrEmpty(targetPackageID))
            {
                Int32 AlumniTenantID = Convert.ToInt32(targetTenantID);
                Int32 DefaultAlumniPackageID = Convert.ToInt32(targetPackageID);
                if (AlumniTenantID == View.SelectedTenantId && DefaultAlumniPackageID > AppConsts.NONE)
                {
                    if (View.ImmunizationTrackingPackages.Where(s => s.CompliancePackageID != DefaultAlumniPackageID).Any())
                        return true;
                    else
                        return false;
                }
            }
            return true;
        }

        private Boolean IsNeedToShowOntheBasisofClientSettings()
        {
            List<String> lstCodes = new List<String>();
            lstCodes.Add(Setting.ALUMNI_ACCESS_POPUP.GetStringValue());
            List<Entity.ClientEntity.ClientSetting> lstClientSetting = ComplianceDataManager.GetClientSettingsByCodes(View.IsAdminView ? View.ApplicantTenantId : View.SelectedTenantId, lstCodes);
            var _setting = lstClientSetting.FirstOrDefault(cs => cs.lkpSetting.Code == Setting.ALUMNI_ACCESS_POPUP.GetStringValue());
            if (!_setting.IsNullOrEmpty())
            {
                return Convert.ToBoolean(Convert.ToInt32(_setting.CS_SettingValue));
            }
            else
            {
                return false;
            }
        }

        public AddressContract ApplicantAddress()
        {
          
            var UserData = ComplianceDataManager.GetOrganizationUserDetailByOrganizationUserId(View.ApplicantId);
          return  StoredProcedureManagers.GetAddressByAddressHandleId(UserData.AddressHandleID.Value, UserData.Organization.TenantID.Value);
        

        }
        
        public bool IsLocationServiceTenant()
        {
            return SecurityManager.IsLocationServiceTenant(View.ApplicantTenantId);
        }

        //UAT- 3876

        public bool CheckDataEntryForRequirementPackages(Int32 reqPkgSubscriptionId, Int32 clinicalRotationId)
        {
            bool result = ComplianceDataManager.CheckDataEntryForRequirementPackages(reqPkgSubscriptionId, clinicalRotationId, View.ApplicantTenantId, View.ApplicantId);
            return result;
        }
        #region Globalization Methods

        //public String GetLanguageCulture()
        //{
        //    Guid userId = new Guid(View.UserId);
        //    return SecurityManager.GetLanguageCulture(userId);
        //}
        #endregion
    }
}
