#region Namespaces

#region SystemDefined

using System;
using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.Utils;
using Entity.ClientEntity;
using System.Collections.Generic;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public class PackageSubscriptionPresenter : Presenter<IPackageSubscriptionView>
    {
        #region Variables

        #region Private Variables
        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties
        #endregion

        #region Public Properties
        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetSubscriptionList()
        {
            if (SecurityManager.DefaultTenantID != View.CurrentUserTenantId)
            {
                View.ListSubscription = ComplianceDataManager.GetSubscriptionList(View.CurrentUserTenantId, View.CurrentUserId).ToList();
            }
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentUserId).Organization.TenantID.Value;
        }

        //public void GetClientSettingDetails()
        //{
        //    if (SecurityManager.DefaultTenantID != View.CurrentUserTenantId)
        //    {
        //        List<ClientSetting> clientSettingDetails = ComplianceDataManager.GetClientSetting(View.CurrentUserTenantId);
        //        if (clientSettingDetails.Count > 0)
        //        {
        //            String clientSettingAfterExpiry = String.Empty;
        //            if (clientSettingDetails.Any(cond => cond.lkpSetting.Code == Setting.Reminder_After_Expiry.GetStringValue()))
        //            {
        //                clientSettingAfterExpiry = clientSettingDetails.Where(cond => cond.lkpSetting.Code == Setting.Reminder_After_Expiry.GetStringValue()).FirstOrDefault()
        //                                            .CS_SettingValue;
        //            }
        //            String clientSettingBeforeExpiry = String.Empty;
        //            if (clientSettingDetails.Any(cond => cond.lkpSetting.Code == Setting.Reminder_Before_Expiry.GetStringValue()))
        //            {
        //                clientSettingBeforeExpiry = clientSettingDetails.Where(cond => cond.lkpSetting.Code == Setting.Reminder_Before_Expiry.GetStringValue()).FirstOrDefault().CS_SettingValue;
        //            }

        //            View.ClientSettingAfterExpiry = clientSettingAfterExpiry.IsNullOrEmpty() ? 0 : Convert.ToInt32(clientSettingAfterExpiry);
        //            View.ClientSettingBeforeExpiry = clientSettingBeforeExpiry.IsNullOrEmpty() ? 0 : Convert.ToInt32(clientSettingBeforeExpiry);
        //        }
        //    }
        //}

        public void GetSubscriptionFrequencies()
        {
            if (SecurityManager.DefaultTenantID != View.CurrentUserTenantId)
            {
                View.lstSubscriptionFrequencies = ComplianceDataManager.GetSubscriptionNotificationFrequencyDays(View.CurrentUserTenantId);
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
            Order orderDetail = ComplianceDataManager.GetOrderById(View.CurrentUserTenantId, orderId);
            //&& cond.DPPS_TotalPrice != 0 include packages with 0 price :[UAT-360]:- WB: Compliance Order: Applicant receives error message when attempting to purchase a subscription with the price of $0.00
            if (orderDetail.DeptProgramPackage.IsNotNull() && orderDetail.DeptProgramPackage.DeptProgramPackageSubscriptions.IsNotNull())
            {
                DeptProgramPackageSubscription depProgramPackageSubscription = orderDetail.DeptProgramPackage.DeptProgramPackageSubscriptions.Where(cond => cond.SubscriptionOption.Code == subscriptionOptionCode && cond.SubscriptionOption.IsSystem && !cond.DPPS_IsDeleted && (cond.DPPS_TotalPrice != null)).FirstOrDefault();
                if (depProgramPackageSubscription != null && depProgramPackageSubscription.DPPS_TotalPrice != null && ((orderDetail != null && (orderDetail.ProgramDuration ?? 0) <= 0) || IsSubscriptionRenewalValid(orderDetail)))
                {
                    return true;
                }
            }
            return false;
        }


        public Boolean IsCustomPriceSet(Int32 orderId)
        {
            Guid subscriptionOptionCode = new Guid(SubscriptionOptions.CustomMonthly.GetStringValue());
            Order orderDetail = ComplianceDataManager.GetOrderById(View.CurrentUserTenantId, orderId);
            //&& cond.DPPS_TotalPrice != 0 include packages with 0 price :[UAT-360]:- WB: Compliance Order: Applicant receives error message when attempting to purchase a subscription with the price of $0.00
            DeptProgramPackageSubscription depProgramPackageSubscription = orderDetail.DeptProgramPackage.DeptProgramPackageSubscriptions.Where(cond => cond.SubscriptionOption.Code == subscriptionOptionCode && cond.SubscriptionOption.IsSystem && !cond.DPPS_IsDeleted && (cond.DPPS_TotalPrice != null)).FirstOrDefault();
            if (depProgramPackageSubscription != null && depProgramPackageSubscription.DPPS_TotalPrice != null)
            {
                return true;
            }
            return false;
        }

        public Dictionary<String, String> GetOrderDetails(int orderID)
        {
            Dictionary<String, String> dic = new Dictionary<String, String>();

            Guid subscriptionOptionCode = new Guid(SubscriptionOptions.CustomMonthly.GetStringValue());
            Order orderDetail = ComplianceDataManager.GetOrderById(View.CurrentUserTenantId, orderID);
            if (orderDetail.IsNotNull() && orderDetail.PackageSubscriptions.IsNotNull())
            {
                //DeptProgramPackageSubscription depProgramPackageSubscription = orderDetail.DeptProgramPackage.DeptProgramPackageSubscriptions.Where(cond =>
                //                                                               cond.SubscriptionOption.Code == subscriptionOptionCode
                //                                                               && cond.SubscriptionOption.IsSystem
                //                                                               && !cond.DPPS_IsDeleted && (cond.DPPS_TotalPrice != null && cond.DPPS_TotalPrice != 0))
                //                                                               .FirstOrDefault();

                //UAT-339 Change to Previous Package Settlement formula.
                //First Order of Current Package, if RO then get first order else get current Order
                //Order fstOrderDetail = GetRecursiveOrder(View.CurrentUserTenantId, orderDetail);

                //DeptProgramPackageSubscription deptPrgmPackageSubscription = orderDetail.DeptProgramPackage.DeptProgramPackageSubscriptions.
                //                                        Where(cond => cond.SubscriptionOption.Label == fstOrderDetail.SubscriptionLabel
                //                                         && !cond.SubscriptionOption.IsSystem
                //                                        ).FirstOrDefault();

                dic.Add("ExpiryDate", orderDetail.PackageSubscriptions.LastOrDefault().ExpiryDate.ToString());

                //UAT-339 Change to Previous Package Settlement formula.

                //dic.Add("PackagePrice", Convert.ToString(deptPrgmPackageSubscription.DPPS_TotalPrice ?? 0));
                dic.Add("PackagePrice", Convert.ToString(orderDetail.TotalPrice ?? 0));

                dic.Add("TotalMonthsInPackage", Convert.ToString(GetTotalNumOfMonths(orderDetail.SubscriptionYear,
                                                orderDetail.SubscriptionMonth)));

                //if (depProgramPackageSubscription.IsNotNull())
                //{
                //    dic.Add("MonthlyPrice", Convert.ToString(depProgramPackageSubscription.DPPS_TotalPrice ?? 0));
                //}
            }
            return dic;
        }

        public Boolean IsPackageChangeSubscription(Int32 orderID)
        {
            List<Order> orderList = ComplianceDataManager.GetChangeSubscriptionOrderList(View.CurrentUserTenantId, orderID);
            if (orderList.IsNotNull() && orderList.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public void GetRecentPackagesIDList()
        {
            Int32 deptProgramMappingId = ComplianceDataManager.GetRecentHierachyNodeIDForApplicant(View.CurrentUserId, View.CurrentUserTenantId);
            List<Int32> deptProgramMappingIds = new List<Int32>() { deptProgramMappingId };
            List<DeptProgramPackage> deptProgramPackageList = ComplianceDataManager.GetDeptProgramPackage(View.CurrentUserId, View.CurrentUserTenantId, deptProgramMappingIds);

            if (deptProgramPackageList.IsNotNull())
            {
                View.RecentPackagesIDList = deptProgramPackageList.Select(x => x.DPP_CompliancePackageID).ToList();
            }
        }

        #region [UAT-977: Additional Works for Archive access]
        /// <summary>
        /// Method return true if already an active request is send for the same package subscription id.
        /// </summary>
        /// <param name="packageSubscriptionId">packageSubscriptionId</param>
        /// <returns></returns>
        public Boolean IsActiveUnArchiveRequestForPkgSubscriptionId(Int32 packageSubscriptionId)
        {
            return ComplianceDataManager.IsActiveUnArchiveRequestForPkgSubscriptionId(View.CurrentUserTenantId, packageSubscriptionId, ComplianceSubscriptionArchiveChangeType.UN_ARCHIVE_REQUESTED.GetStringValue());
        }

        public Boolean SaveCompSubscriptionUnArchiveRequest(Int32 packageSubscriptionId)
        {
            Int16 unArchiveChangeTypeId = ComplianceDataManager.GetComplianceSubsArchiveChangeTypeIdByCode(View.CurrentUserTenantId, ComplianceSubscriptionArchiveChangeType.UN_ARCHIVE_REQUESTED.GetStringValue());
            CompliancePackageSubscriptionArchiveHistory objectToSaveUnArchiveRequest = new CompliancePackageSubscriptionArchiveHistory();
            objectToSaveUnArchiveRequest.CPSAH_IsActive = true;
            objectToSaveUnArchiveRequest.CPSAH_IsDeleted = false;
            objectToSaveUnArchiveRequest.CPSAH_PackageSubscriptionID = packageSubscriptionId;
            objectToSaveUnArchiveRequest.CPSAH_SubscriptionChangeDetail = null;
            objectToSaveUnArchiveRequest.CPSAH_ChangeTypeID = unArchiveChangeTypeId;
            //objectToSaveUnArchiveRequest.CPSAH_CreatedBy = View.CurrentUserId;
            objectToSaveUnArchiveRequest.CPSAH_CreatedBy = View.OrgUsrID;
            objectToSaveUnArchiveRequest.CPSAH_CreatedOn = DateTime.Now;
            return ComplianceDataManager.SaveCompSubscriptionArchiveHistoryData(View.CurrentUserTenantId, objectToSaveUnArchiveRequest);
        }
        #endregion

        #endregion

        #region Private Method
        /// <summary>
        /// Method to check for valid subscription renewal
        /// </summary>
        /// <param name="orderDetail">orderDetail</param>
        /// <returns>Boolean</returns>
        private Boolean IsSubscriptionRenewalValid(Order orderDetail)
        {
            List<Order> orderList = ComplianceDataManager.GetOrderListOfPreviousOrder(View.CurrentUserTenantId, orderDetail);
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

        /// <summary>
        /// Get total months 
        /// </summary>
        /// <param name="NoOfYears"></param>
        /// <param name="NoOfMonths"></param>
        /// <returns></returns>
        private Int32 GetTotalNumOfMonths(Int32? NoOfYears, Int32? NoOfMonths)
        {
            Int32 totalNumOfMonths = AppConsts.NONE;
            if (NoOfYears.IsNotNull())
            {
                totalNumOfMonths = NoOfYears.Value * 12;
            }
            if (NoOfMonths.IsNotNull())
            {
                totalNumOfMonths = totalNumOfMonths + NoOfMonths.Value;
            }
            return totalNumOfMonths;
        }

        /// <summary>
        /// Get recursive orders based on the Order Request Type.
        /// </summary>
        /// <param name="CurrentUserTenantId"></param>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
        private Order GetRecursiveOrder(Int32 CurrentUserTenantId, Order orderDetail)
        {
            if (orderDetail.lkpOrderRequestType.ORT_Code == OrderRequestType.RenewalOrder.GetStringValue())
            {
                Order ordDetail = ComplianceDataManager.GetOrderById(View.CurrentUserTenantId, orderDetail.PreviousOrderID.Value);
                return GetRecursiveOrder(View.CurrentUserTenantId, ordDetail);
            }
            else
            {
                return orderDetail;
            }
            //Order orderDetail = ComplianceDataManager.GetOrderById(View.CurrentUserTenantId, orderID);
        }


        #endregion

        #endregion
    }
}




