using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ComplianceOperation;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using System.Data;

namespace Business.RepoManagers
{
    public class FingerPrintDataManager
    {
        public static Dictionary<Int32, Int32> GetLocationHierarchy(Int32 tenantID, Int32 locationID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantID).GetLocationHierarchy(locationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static string GetPackageNameForCompleteOrder(Int32 tenantId, Int32 orderId, string serviceType, bool isIdRequired)
        {
            return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetPackageNameForCompleteOrder(orderId, serviceType,  isIdRequired);
        }
        public static string GetShippingLineItemName(Int32 tenantId,  string serviceType)
        {
            return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetShippingLineItemName( serviceType);
        }
        public static PreviousAddressContract GetAddressThroughAddressHandleID(Int32 tenantId, string MailingAddressHandleId)
        {
            return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetAddressThroughAddressHandleID(MailingAddressHandleId);
        }

        /// <summary>
        /// To save the  services  service status.
        /// </summary>
        /// <returns></returns>
        public static void SaveServiceStatus(Int32 tenantId, Int32 detailExtID, Int32 serviceStatusID, bool IsServiceStatusRejected, Int32 CurrentLoggedInUserID)
        {
            try
            {
                BALUtils.GetFingerPrintClientRepoInstance(tenantId).SaveServiceStatus(detailExtID, serviceStatusID, IsServiceStatusRejected, CurrentLoggedInUserID);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static string GetOrderAppointmentStatus(Int32 tenantId, Int32 OrderID, Int32 CurrentLoggedInUserID)
        {
            try
            {
              return  BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetOrderAppointmentStatus(OrderID, CurrentLoggedInUserID);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static bool GetIsOrderRescheduled(Int32 tenantId, Int32 OrderID, Int32 CurrentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetIsOrderRescheduled(OrderID, CurrentLoggedInUserID);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static bool CheckShipmntPriorAppointment(Int32 tenantId, Int32 OrderID, Int32 CurrentLoggedInUserID, int AppointmentDetailExtId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).CheckShipmntPriorAppointment(OrderID, CurrentLoggedInUserID, AppointmentDetailExtId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static DateTime? GetAdditionalServiceShipmentDate(Int32 tenantId, Int32 OrderID, Int32 CurrentLoggedInUserID, Int32 AppointmentDetailExtId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetAdditionalServiceShipmentDate(OrderID, CurrentLoggedInUserID, AppointmentDetailExtId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<String> GetServiceStatus(Int32 tenantId, Int32 orderId, Int32 orgUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetServiceStatus(orderId, orgUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// To save the  tracking number.
        /// </summary>
        /// <returns></returns>
        public static void SaveTrackingNumber(Int32 tenantId, Int32 detailExtID, string trackingnum)
        {
            try
            {
                BALUtils.GetFingerPrintClientRepoInstance(tenantId).SaveTrackingNumber(detailExtID, trackingnum);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static ReserveSlotContract SubmitApplicantAppointment(ReserveSlotContract reserveSlotContract, Int32 currentLoggedInUserId, Boolean isCompleteYourOrderClick = false)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(reserveSlotContract.TenantID).SubmitApplicantAppointment(reserveSlotContract, currentLoggedInUserId, isCompleteYourOrderClick);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SubmitOrderBillingCodeMapping(Int32 tenantID, Int32 orderID, String billingCode, Int32 currentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantID).SubmitOrderBillingCodeMapping(orderID, billingCode, currentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveUpdateApplicantAppointmentHistory(Int32 tenantId, AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).SaveUpdateApplicantAppointmentHistory(scheduleAppointmentContract, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static Boolean RescheduleApplicantAppointmentHistory(Int32 tenantId, AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).RescheduleApplicantAppointmentHistory(scheduleAppointmentContract, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static decimal GetOrderPriceTotal(Int32 TenantID, Int32 orgUserID, Int32 OrderID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantID).GetOrderPriceTotal(OrderID, orgUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static string GetOrderNumber(Int32 TenantID, Int32 orgUserID, Int32 OrderID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantID).GetOrderNumber(OrderID, orgUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Address GetMailingAddressDetails(Int32 TenantID, Int32 OrderID, Int32 orgUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantID).GetMailingAddressDetails(OrderID, orgUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static SelectedMailingData GetSelectedMailingOptionPrice(Int32 TenantID, Int32 OrderID, Int32 orgUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantID).GetSelectedMailingOptionPrice(OrderID, orgUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static decimal GetSentForOnlinePaymentAmount(Int32 TenantID, Int32 OrderID, Int32 orgUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantID).GetSentForOnlinePaymentAmount(OrderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static PreviousAddressContract GetAddressData(Int32 TenantID, Int32 OrderID, Int32 orgUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantID).GetAddressData(OrderID, orgUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static PreviousAddressContract GetShippingAddressData(Int32 TenantID, Int32 OrderID, Int32 orgUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantID).GetShippingAddressData(OrderID, orgUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }



        public static void UpdateMailingDetailXML(Int32 tenantID, Int32 orderID, Guid mailingAddressHandleId, String mailingoptionID, String mailingOptionPrice)
        {
            try
            {
                 BALUtils.GetFingerPrintClientRepoInstance(tenantID).UpdateMailingDetailXML(orderID, mailingAddressHandleId, mailingoptionID,mailingOptionPrice);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }




        public static Int32 GetOrderHeirarchyNodeId(Int32 TenantID, Int32 OrderID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantID).GetOrderHeirarchyNodeId(OrderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Order GetOrderByOrderId(Int32 TenantID, Int32 OrderID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantID).GetOrderByOrderId(OrderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean ResetApplicantAppointmenBkgOrderStatus(Int32 tenantId, AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserId)
        {
            try
            {
                if (scheduleAppointmentContract.IsRejectedReschedule)
                {
                    return BALUtils.GetFingerPrintClientRepoInstance(tenantId).ResetApplicantAppointmenBkgOrderStatus(scheduleAppointmentContract, currentLoggedInUserId);
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveTenantLocationMapping(Int32 selectedLocationID, Int32 tenantId, Int32 selectedDpmId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).SaveTenantLocationMapping(selectedLocationID, tenantId, selectedDpmId, currentLoggedInUserId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static Boolean HideReschedule(AppointmentSlotContract scheduleAppointmentContract, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).HideReschedule(scheduleAppointmentContract);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean HideCancel(int OrderId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).HideCancel(OrderId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static int? GetRevertToMoneyDetails(List<int> PaymentDetails, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetRevertToMoneyDetails(PaymentDetails);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }




        public static Boolean HideRescheduleForAdmin(int OrderId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).HideRescheduleForAdmin(OrderId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean HideABIReviewForFulfilment(int OrderId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).HideABIReviewForFulfilment(OrderId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        


        public static List<Int32> GetDPMLocations(Int32 tenantId, Int32 selectedDpmId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetDPMLocationIDs(tenantId, selectedDpmId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteTenantLocationMapping(Int32 tenantId, Int32 selectedDPMId, Int32 selectedLocationId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).DeleteTenantLocationMapping(tenantId, selectedDPMId, selectedLocationId, currentLoggedInUserId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static bool IsLocationMapped(Int32 TenantId, Int32 selectedDpmId)
        {
            try
            {
                List<Int32> lstLocations = GetDPMLocations(TenantId, selectedDpmId);
                if (lstLocations.Count > AppConsts.NONE)
                {

                    return BALUtils.GetFingerPrintSetupRepoInstance().IsLocationInUse(TenantId, lstLocations);
                }
                return false;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }
        public static Boolean DeleteTenantNodeLocationMapping(Int32 tenantId, Int32 selectedDPMId, Int32 currentLoggedInUserId)
        {
            try
            {
                List<Int32> lstLocationIds = GetDPMLocations(tenantId, selectedDPMId);
                if (lstLocationIds.Count > AppConsts.NONE)
                {
                    return BALUtils.GetFingerPrintClientRepoInstance(tenantId).DeleteTenantNodeLocationMapping(tenantId, selectedDPMId, currentLoggedInUserId);
                }
                return true;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// To fetch the specfic order detail.
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public static List<OrderDetailContract> OrderSerivceDetail(Int32 tenantId, Int32 orderId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).OrderSerivceDetail(orderId, currentLoggedInUserId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }




        /// <summary>
        /// To Get the CABS service statues list.
        /// </summary>
        /// <returns></returns>
        public static List<ServiceStatusContract> GetServiceStatues(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetServiceStatues();
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }



        public static AppointmentOrderScheduleContract GetAppointmentFulFillmentQueueOrderDetailData(Int32 userID, Boolean isAdmin, Int32 tenantId, Int32 applicantAppointmentId)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().GetAppointmentFulFillmentQueueOrderDetailData(userID, isAdmin, tenantId.ToString(), applicantAppointmentId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }




        //public static Boolean SaveRescheduledAppointment(Int32 tenantId, Int32 currentLoggedInUserId, AppointmentSlotContract appointSlotContract)
        //{
        //    try
        //    {
        //        return BALUtils.GetFingerPrintClientRepoInstance(tenantId).SaveRescheduledAppointment(tenantId, currentLoggedInUserId, appointSlotContract);
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        public static Boolean CancelBkgOrder(Int32 tenantId, Int32 orderId, String orderStatusCode, Int32 currentLoggedInUserId, Boolean isCancelledByApplicant, Int32 applicantAppointmentId, Decimal? RefundAmount = null, Int32? OrganizationUserId = null)
        {
            try
            {
                var result = ComplianceDataManager.CancelPlacedOrder(tenantId, orderId, orderStatusCode, currentLoggedInUserId, isCancelledByApplicant);
                if (result)
                {
                    // Mail Code Here  ---  Cancel confirmed mail
                    result = BALUtils.GetFingerPrintClientRepoInstance(tenantId).DeleteApplicantAppointment(tenantId, orderId, currentLoggedInUserId, applicantAppointmentId, RefundAmount);
                    SendCancelOrderMail(tenantId, orderId, OrganizationUserId);
                    return result;
                }
                return false;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean ApprovePayment(Int32 tenantId, Int32 orderId, Int32 orderPaymentDetailId, Int32 currentLoggedInUserId)
        {
            try
            {
                Int32 orderStatusPaidId = LookupManager.GetLookUpData<lkpOrderStatu>(tenantId).FirstOrDefault(type => type.Code == ApplicantOrderStatus.Paid.GetStringValue()
                                                        && !type.IsDeleted).OrderStatusID;
                Int32 newOrderStatusTypeID = LookupManager.GetLookUpData<lkpOrderStatusType>(tenantId).Where(type => type.Code == BackgroundOrderStatus.NEW.GetStringValue()).FirstOrDefault().OrderStatusTypeID;

                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).ApprovePayment(tenantId, orderId, orderPaymentDetailId, orderStatusPaidId, currentLoggedInUserId, newOrderStatusTypeID);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean GetIfApprovePaymentReqd(Int32 tenantId, Int32 orderId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetIfApprovePaymentReqd(tenantId, orderId, currentLoggedInUserId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean GetIsRevertToMoneyOrder(Int32 tenantId, Int32 orderId, Int32 currentLoggedInUserId, int orderPaymentDetailId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetIsRevertToMoneyOrder(tenantId, orderId, currentLoggedInUserId, orderPaymentDetailId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        

        public static Boolean IsOrderPaymentStatusPending(Int32 tenantId, Int32 orderId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).IsOrderPaymentStatusPending(tenantId, orderId,  currentLoggedInUserId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }







        public static AppointmentSlotContract GetBkgOrderWithAppointmentData(Int32 tenantId, Int32 OrderId, Int32 ApplicantOrgUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetBkgOrderWithAppointmentData(tenantId, OrderId, ApplicantOrgUserID);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<ValidateRegexDataContract> GetPersonalInformationExpressions(Int32 tenantId, Int32 BkgPkgID, String languageCode = default(String))
        {
            try
            {
                if (languageCode.IsNullOrEmpty())
                {
                    languageCode = Languages.ENGLISH.GetStringValue();
                }
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetPersonalInformationExpressions(BkgPkgID, languageCode);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }




        #region SFTP File Integration for Finger Print Document




        public static Boolean SaveFingerPrintDocumentsStaging(Int32 tenantId, List<String> lstFileName, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).SaveFingerPrintDocumentsStaging(lstFileName, currentLoggedInUserId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }



        public static List<String> GetFingerPrintDocStaging(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetFingerPrintDocStaging();
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<FingerPrintOrderContract> GetInProgressFingerPrintOrders(Int32 tenantId, Int32 completedOrderStatusTypeId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetInProgressFingerPrintOrders(completedOrderStatusTypeId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean InsertDataCBIResultFile(FingerPrintOrderContract fingerPrintOrderContract, Int32 bkgProcessUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().InsertDataCBIResultFile(fingerPrintOrderContract, bkgProcessUserID);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdateDataCBIResultFile(string PCNNumber, Int32 bkgProcessUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintSetupRepoInstance().UpdateDataCBIResultFile(PCNNumber, bkgProcessUserID);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveFingerprintApplicantDocument(Int32 tenantId, List<FingerPrintOrderContract> lstFingerPrintOrderContract, Int32 bkgProcessUserID)
        {
            try
            {
                var result = BALUtils.GetFingerPrintClientRepoInstance(tenantId).SaveFingerprintApplicantDocument(lstFingerPrintOrderContract, bkgProcessUserID);
                if (result)
                {
                    SaveFingerprintApplicantImages(tenantId, lstFingerPrintOrderContract, bkgProcessUserID);
                }
                return result;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteFingerPrintDocStaging(Int32 tenantId, List<String> lstFingerPrintDocStagingToDelete, Int32 bkgProcessUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).DeleteFingerPrintDocStaging(lstFingerPrintDocStagingToDelete, bkgProcessUserID);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean AddDocInApplicantAppointmentDetail(Int32 tenantId, List<FingerPrintOrderContract> lstFingerPrintOrderContract, Int32 bkgProcessUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).AddDocInApplicantAppointmentDetail(lstFingerPrintOrderContract, bkgProcessUserID);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        #endregion

        public static Boolean CanRevertToMoneyOrder(Int32 tenantId, Int32 orderId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).CanRevertToMoneyOrder(orderId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static Boolean UpdateAppointmentHistory(Int32 orderid, String orderStatusCode, Int32 paymentModeId, Int32 tenantId, Int32 currentLoggedInUserId, Int32 applicantAppointmentId, decimal refundAmount)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).UpdateAppointmentHistory(orderid, orderStatusCode, paymentModeId, currentLoggedInUserId, tenantId, applicantAppointmentId, refundAmount);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean SaveUpdateAppointmentAudit(AppointmentOrderScheduleContract AppOrdSchdContract, AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserId, Int32 oldStatusID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(Convert.ToInt32(AppOrdSchdContract.TenantID)).SaveUpdateAppointmentAudit(AppOrdSchdContract, scheduleAppointmentContract, currentLoggedInUserId, oldStatusID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean SaveUpdateApointmentRefundAudit(RefundHistory refundData, Int32 TenantID, Int32 ApplicantOrgUserId, Int32 CurrentUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantID).SaveUpdateApointmentRefundAudit(refundData, ApplicantOrgUserId, CurrentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }



        public static PreviousAddressContract GetMailingDetail(Int32 OrderID, Int32 TenantID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantID).GetMailingDetail(OrderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static string GetServiceDescription(Int32 TenantID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantID).GetServiceDescription();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Dictionary<String, String> ValidateCBIUniqueID(Int32 TenantID, String CBIUniqueID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantID).ValidateCBIUniqueID(CBIUniqueID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean SavePaymentTypeAuditChange(String PaymentModeCode, Int32 OrderId, Int32 TenantId, Int32 CurrentLoggedInUserId, Int32 oldOrderPaymentDetailId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantId).SavePaymentTypeAuditChange(PaymentModeCode, OrderId, CurrentLoggedInUserId, oldOrderPaymentDetailId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<FileResultStatusUpdateContract> GetAllUpdatedFileResults(Int32 tenantId, Int32 ChunkSize, Int32? ApplicantAppointmentId = null)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetAllUpdatedFileResults(ChunkSize, ApplicantAppointmentId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static void UpdateStatusForFileResult(FileResultStatusUpdateContract fileResult, Int32 CurrentLoggedInUserId, Int32 tenantId, Boolean IsSubmittedToCBI, Boolean IsContactAgency)
        {
            try
            {
                BALUtils.GetFingerPrintClientRepoInstance(tenantId).UpdateStatusForFileResult(fileResult, CurrentLoggedInUserId, IsSubmittedToCBI, IsContactAgency);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        // UAT-4271
        public static List<LookupContract> GetCBIUniqueIdByAcctNameOrNumber(Int32 tenantId, String acctNameOrNumber)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetCBIUniqueIdByAcctNameOrNumber(acctNameOrNumber);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region CDR Import Data
        public static void ImportDataToTable(string exportedData, CDRFileDetailContract fileDetailContract, int? backgroundProcessUserId)
        {
            try
            {
                BALUtils.GetFingerPrintClientRepoInstance(0).ImportDataToTable(exportedData, fileDetailContract, backgroundProcessUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int64 GetLastRecordInsertedId()
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(0).GetLastRecordInsertedId();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        public static List<FingerPrintRecieptContract> GetUserRicieptFileData(Int32 tenantId, Int32 ChunkSize)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetUserRicieptFileData(ChunkSize);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean UpdateFileRecieptDispatched(Int32 ApplicantAppointmentId, Int32 tenantId, Int32 UserId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).UpdateFileRecieptDispatched(ApplicantAppointmentId, UserId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean IsFileSentToCbi(Int32 TenantId, Int32 OrderId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantId).IsFileSentToCbi(OrderId);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static CustomFormDataContract GetCustomAttributes(Int32 tenantID, Int32 packageID, String cBIUniqueID, String langCode)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantID).GetCustomAttributes(packageID, cBIUniqueID, langCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-3850

        public static CBIBillingStatu GetCBIBillingStatusData(Int32 tenantID, String cbiUniqueId, String billingCode)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantID).GetCBIBillingStatusData(cbiUniqueId, billingCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Entity.ClientEntity.OrderBillingCodeMapping GetOrderBillingCode(Int32 tenantID, Int32 orderId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantID).GetOrderBillingCode(orderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region SendCbiCancelOrderMail

        public static void SendCancelOrderMail(Int32 tenantId, Int32 OrderId, Int32? OrgUserId)
        {
            if (OrgUserId.HasValue)
            {
                Order OrderDetail = ComplianceDataManager.GetOrderById(tenantId, OrderId);
                var userDetail = SecurityManager.GetOrganizationUser(OrgUserId.Value);
                if (!OrderDetail.IsNullOrEmpty() && !userDetail.IsNullOrEmpty())
                {
                    String institutionName = String.Empty;
                    BackgroundPackage packageData = new BackgroundPackage();
                    if (!OrderDetail.BkgOrders.IsNullOrEmpty() && !OrderDetail.BkgOrders.FirstOrDefault().BkgOrderPackages.IsNullOrEmpty() && !OrderDetail.BkgOrders.FirstOrDefault().BkgOrderPackages.FirstOrDefault().BkgPackageHierarchyMapping.IsNullOrEmpty())
                        packageData = OrderDetail.BkgOrders.FirstOrDefault().BkgOrderPackages.FirstOrDefault().BkgPackageHierarchyMapping.BackgroundPackage;

                    Entity.Tenant tenant = SecurityManager.GetTenant(Convert.ToInt32(tenantId));
                    if (!tenant.IsNullOrEmpty())
                        institutionName = tenant.TenantName.IsNullOrEmpty() ? String.Empty : tenant.TenantName;

                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                    dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, userDetail.FirstName + " " + userDetail.LastName);
                    dictMailData.Add(EmailFieldConstants.ORDER_NO, OrderDetail.OrderNumber);
                    dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId);
                    dictMailData.Add(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID, Convert.ToString(OrgUserId.Value));
                    dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, packageData.IsNullOrEmpty() ? String.Empty : (packageData.BPA_Label.IsNullOrEmpty() ? packageData.BPA_Name : packageData.BPA_Label));
                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, institutionName);
                    dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, OrderDetail.DeptProgramMapping.DPM_Label);

                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                    mockData.UserName = userDetail.FirstName + " " + userDetail.LastName;
                    mockData.EmailID = userDetail.PrimaryEmailAddress;
                    mockData.ReceiverOrganizationUserID = OrgUserId.Value;
                    CommunicationSubEvents ObjCommunicationSubEvents = new CommunicationSubEvents();
                    ObjCommunicationSubEvents = CommunicationSubEvents.NOTIFICATION_ORDER_CANCELLATION_APPROVED;

                    //// send mail/message notification
                    var HierarchyNodeid = OrderDetail.HierarchyNodeID.HasValue ? OrderDetail.HierarchyNodeID.Value : 0;
                    CommunicationManager.SentMailMessageNotification(ObjCommunicationSubEvents, mockData, dictMailData, OrgUserId.Value, tenantId, HierarchyNodeid);
                }
            }

        }

        #endregion

        #region UAT - 4088
        public static List<FileResultStatusUpdateContract> GetAllManuallyRejectedOrders(Int32 tenantId, Int32 AppointmentID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetAllManuallyRejectedOrders(AppointmentID);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT- 4088
        public static Boolean RejectOutOfStateOrderByCBI(Int32 ApplicantAppointmentId, Int32 CurrentLoggedInUserId, Int32 TenantID)
        {
            try
            {
                if (BALUtils.GetFingerPrintClientRepoInstance(TenantID).RejectOutOfStateOrderByCBI(ApplicantAppointmentId, CurrentLoggedInUserId))
                {
                    RescheduleManuallyRejectedOutOfStateOrder(TenantID, ApplicantAppointmentId, CurrentLoggedInUserId);
                    return true;
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        private static Boolean RescheduleManuallyRejectedOutOfStateOrder(Int32 TenantId, Int32 ApplicantAppointmentId, Int32 CurrentLoggedInUserId)
        {
            try
            {

                var tenant = SecurityManager.GetTenant(Convert.ToInt32(TenantId));
                String institutionName = tenant.IsNull() || tenant.TenantName.IsNullOrEmpty() ? String.Empty : tenant.TenantName;
                ///// Getting FileResultStatusUpdateContract Data
                List<FileResultStatusUpdateContract> fileResultStatusUpdateContract = FingerPrintDataManager.GetAllManuallyRejectedOrders(TenantId, ApplicantAppointmentId);

                foreach (var fileResult in fileResultStatusUpdateContract)
                {
                    ////Create Dictionary for Mail And Message Data
                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                    dictMailData.Add(EmailFieldConstants.AGENCY_NAME, institutionName);
                    dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, fileResult.ApplicantName);
                    dictMailData.Add(EmailFieldConstants.Order_Number, fileResult.OrderNumber);
                    dictMailData.Add(EmailFieldConstants.CBI_SUCCESS_STATUS, "Manually Rejected");
                    dictMailData.Add(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID, fileResult.UserId);
                    dictMailData.Add(EmailFieldConstants.TENANT_ID, TenantId);

                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                    mockData.UserName = string.Concat(fileResult.ApplicantName);
                    mockData.EmailID = fileResult.UserEmailId;
                    mockData.ReceiverOrganizationUserID = fileResult.UserId;

                    CommunicationSubEvents ObjCommunicationSubEvents = CommunicationSubEvents.NOTIFICATION_FOR_FINGERPRINT_OUT_OF_STATE_MANUAL_REJECTION;

                    AppointmentSlotContract reserveSlotContract = new AppointmentSlotContract();
                    reserveSlotContract.OrderNumber = fileResult.OrderNumber;
                    reserveSlotContract.IsOutOfStateAppointment = true;
                    reserveSlotContract.IsRejectedReschedule = true;
                    reserveSlotContract.ApplicantOrgUserId = fileResult.UserId;
                    if (FingerPrintSetUpManager.ResetOutOfStateApplicantAppointment(reserveSlotContract, CurrentLoggedInUserId, TenantId))
                    {
                        //// send mail/message notification                                
                        CommunicationManager.SentMailMessageNotification(ObjCommunicationSubEvents, mockData, dictMailData, fileResult.UserId, TenantId, fileResult.HierarchyNodeId);
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                return false;

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT -4270
        public static Boolean SaveManualFingerPrintFile(ApplicantDocument appDocument, Int32 FingerprintAppointmentId, Int32 TenantId, Int32 CurrentLoggedinUserId, Boolean IsAbiReviewUpload)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantId).SaveManualFingerPrintFile(appDocument, FingerprintAppointmentId, CurrentLoggedinUserId, IsAbiReviewUpload);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion
        #region ABI Review
        public static bool UpdateAppointmentStatus(string appointmentStatusCode, int fingerPrintApplicantAppointmentID, int TenantId, int CurrentLoggedinUserId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantId).UpdateAppointmentStatus(appointmentStatusCode, fingerPrintApplicantAppointmentID, CurrentLoggedinUserId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        public static ApplicantFingerPrintFileImageContract GetFingerPrintImageData(Int32 TenantId, Int32 ApplicantAppointmentDetailID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantId).GetFingerPrintImageData(ApplicantAppointmentDetailID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveFingerprintApplicantImages(Int32 tenantId, List<FingerPrintOrderContract> lstFingerPrintOrderContract, Int32 bkgProcessUserID)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(tenantId).SaveFingerprintApplicantImages(lstFingerPrintOrderContract, bkgProcessUserID);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return false;
                //throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean ChangeSendToCBIAppointmentStatus(List<AppointmentOrderScheduleContract> lstAppointmentOrderScheduleContract, Int32 CurrentLoggedInUserID)
        {
            try
            {
                var tenantIds = lstAppointmentOrderScheduleContract.Select(s => Convert.ToInt32(s.TenantID)).Distinct();
                foreach (var tenantId in tenantIds)
                {
                    return BALUtils.GetFingerPrintClientRepoInstance(tenantId).ChangeSendToCBIAppointmentStatus(lstAppointmentOrderScheduleContract, CurrentLoggedInUserID);
                }
                return true;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return false;
            }
        }

        public static List<LocationServiceAppointmentAuditContract> GetOrderHistoryList(Int32 TenantId, Int32 OrderID, bool IsCABSAppointment)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantId).GetOrderHistoryList(OrderID, IsCABSAppointment);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }





        #region Archive order
        public static List<FingerPrintArchivedOrderContract> GetBkgPkgPrevOrderDetails(Int32 orgainizatuionUserId, Int32 tenantId)
        {
            try
            {
                DataTable table = BALUtils.GetFingerPrintClientRepoInstance(tenantId).GetBkgPkgPrevOrderDetails(tenantId, orgainizatuionUserId);
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new FingerPrintArchivedOrderContract
                {
                    OrderId = x["OrderID"] is DBNull ? 0 : Convert.ToInt32(x["OrderID"]),
                    OrderNumber = x["OrderNumber"] is DBNull ? "" : Convert.ToString(x["OrderNumber"]),
                    OrderDate = x["OrderDate"] is DBNull ? DateTime.Now : Convert.ToDateTime(x["OrderDate"]),
                    FirstName = x["FirstName"] is DBNull ? "" : Convert.ToString(x["FirstName"]),
                    LastName = x["LastName"] is DBNull ? "" : Convert.ToString(x["LastName"]),
                    PrimaryEmail = x["PrimaryEmail"] is DBNull ? "" : Convert.ToString(x["PrimaryEmail"]),
                    DowntownLocationId = x["DowntownLocationId"] is DBNull ? 0 : Convert.ToInt32(x["DowntownLocationId"]),
                    DowntownLocationName = x["DowntownLocationName"] is DBNull ? "" : Convert.ToString(x["DowntownLocationName"]),
                    DowntownLocationAddress = x["DowntownLocationAddress"] is DBNull ? "" : Convert.ToString(x["DowntownLocationAddress"]),
                    IsPrinterAvailable = x["IsPrinterAvailable"] is DBNull ? false : Convert.ToBoolean(x["IsPrinterAvailable"]),
                    DowntownLocationDescription = x["DowntownLocationAddress"] is DBNull ? "" : Convert.ToString(x["DowntownLocationDescription"]),
                    IsPassportPhotoService = x["IsPassportPhotoService"] is DBNull ? false : Convert.ToBoolean(x["IsPassportPhotoService"]),
                    ArchivedOrgUserId = x["ArchivedOrganizationUserID"] is DBNull ? 0 : Convert.ToInt32(x["ArchivedOrganizationUserID"]),
                    ArchivedOrgUserProfileId = x["ArchivedOrganizationUserProfileID"] is DBNull ? 0 : Convert.ToInt32(x["ArchivedOrganizationUserProfileID"]),
                }).ToList();

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion
        
        public static List<BackgroundServiceContract> GetOrderBackgroundServices(Int32 OrderID, Int32 TenantId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantId).GetOrderBackgroundServices(OrderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static bool AdditionalServicesNotshipped(Int32 OrderID, Int32 TenantId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantId).AdditionalServicesNotshipped(OrderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static bool AdditionalServicesExist(Int32 OrderID, Int32 TenantId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantId).AdditionalServicesExist(OrderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        

        public static LocationContract GetLocationByOrderId(Int32 OrderID, Int32 TenantId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantId).GetLocationByOrderId(OrderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static LocationContract GetLocationByLocationid(Int32 LocationId, Int32 TenantId)
        {
            try
            {
                return BALUtils.GetFingerPrintClientRepoInstance(TenantId).GetLocationByOrderId(LocationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

    }
}
