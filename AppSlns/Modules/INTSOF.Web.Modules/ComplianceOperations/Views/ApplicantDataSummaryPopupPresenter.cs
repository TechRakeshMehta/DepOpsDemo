using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ApplicantDataSummaryPopupPresenter : Presenter<IApplicantDataSummaryPopupView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            GetApplicantLastLoginTime();
        }

        /// <summary>
        /// Get ApplicantLast login detail
        /// </summary>
        public void GetApplicantLastLoginTime()
        {
            List<Entity.UserLoginHistory> userLoginHistorydataList = SecurityManager.GetApplicantLastLoginDetail(View.CurrentLoggedInUserID, String.Empty);
            if (!userLoginHistorydataList.IsNullOrEmpty())
            {
                Entity.UserLoginHistory userLoginHistorydata = userLoginHistorydataList.OrderByDescending(ord => ord.ULH_ID).Skip(1).Take(1).FirstOrDefault();
                if (!userLoginHistorydata.IsNullOrEmpty())
                {
                    View.UserLastLoginTime = userLoginHistorydata.ULH_LogoutTime.IsNullOrEmpty() ? userLoginHistorydata.ULH_LoginTime : userLoginHistorydata.ULH_LogoutTime;
                    View.UserLastLoginTime = DateTime.SpecifyKind(View.UserLastLoginTime.Value, DateTimeKind.Utc).ToLocalTime();
                }
            }
        }

        /// <summary>
        /// Get Applicant Data summary detail
        /// </summary>
        public void GetApplicantSummaryData()
        {
            var tupleData = ComplianceSetupManager.GetAppSummaryDataAfterLastLogin(View.CurrentLoggedInUserID, View.ApplicantTenantId, View.UserLastLoginTime);
            View.lstApplicantSummary = tupleData.Item1;
            View.lstApplicantBackgroundSummary = tupleData.Item2;
        }

        public void GetApplicantApprovedRotations()
        {
            View.lstApprovedRotations = ProfileSharingManager.GetApprovedRotationsAfterSinceLastLogin(View.CurrentLoggedInUserID, View.ApplicantTenantId, View.UserLastLoginTime.HasValue ? View.UserLastLoginTime.Value : new DateTime(1990, 1, 1));
        }

        //UAT-2924: Add upcoming expirations to Since You Been Gone popup as part of the not compliant categories
        public void GetUpcomingExpirationcategoryByLoginId()
        {
            View.lstUpcomingCategoryExpiration = ComplianceSetupManager.GetUpcomingExpirationcategoryByLoginId(View.CurrentLoggedInUserID, View.ApplicantTenantId);
        }

        public void GetSubscriptionList()
        {
            if (SecurityManager.DefaultTenantID != View.ApplicantTenantId)
            {
                View.ListSubscription = ComplianceDataManager.GetSubscriptionList(View.ApplicantTenantId, View.CurrentLoggedInUserID).ToList();
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
        public void GetSubscriptionFrequencies()
        {
            if (SecurityManager.DefaultTenantID != View.ApplicantTenantId)
            {
                View.lstSubscriptionFrequencies = ComplianceDataManager.GetSubscriptionNotificationFrequencyDays(View.ApplicantTenantId);
            }
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
    }
}
