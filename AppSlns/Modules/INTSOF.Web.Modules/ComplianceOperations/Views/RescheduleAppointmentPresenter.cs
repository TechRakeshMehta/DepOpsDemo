using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
  public  class RescheduleAppointmentPresenter : Presenter<IRescheduleAppointmentView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads

        }

        public override void OnViewInitialized()
        {

        }

        public ReserveSlotContract ReserveSlot()
        {
            return FingerPrintSetUpManager.ReserveSlot(0, View.SelectedSlotID, View.CurrentLoggedInUserId);
        }
        public LocationContract GetLocationByOrderId(Int32 OrderID, Int32 TenantId)
        {
            return FingerPrintDataManager.GetLocationByOrderId(OrderID, TenantId);
        }

        public LocationContract GetLocationByLocationid(Int32 OrderID, Int32 TenantId)
        {
            return FingerPrintDataManager.GetLocationByOrderId(OrderID, TenantId);
        }

        public ReserveSlotContract SubmitApplicantAppointment(Boolean isLocationUpdate, Boolean IsOnsiteAppointment, Boolean IsRejectedReschedule)
        {
            ReserveSlotContract reserveSlotContract = new ReserveSlotContract();
            reserveSlotContract.SlotID = View.AppointSlotContract.SlotID;
            reserveSlotContract.TenantID = View.TenantId;
            reserveSlotContract.AppOrgUserID = View.AppointSlotContract.ApplicantOrgUserId;
            reserveSlotContract.OrderID = View.AppointSlotContract.OrderId;
            reserveSlotContract.LocationID = View.AppointSlotContract.LocationId;
            reserveSlotContract.ReservedSlotID = View.AppointSlotContract.ReservedSlotID;
            reserveSlotContract.IsEventTypeCode = IsOnsiteAppointment;
            reserveSlotContract.IsLocationUpdate = isLocationUpdate;
            reserveSlotContract.IsRejectedReschedule = IsRejectedReschedule;

            if (!reserveSlotContract.IsNullOrEmpty())
                return FingerPrintDataManager.SubmitApplicantAppointment(reserveSlotContract, View.CurrentLoggedInUserId);
            else
                return new ReserveSlotContract();
        }



        public void SendAppointmentRescheduleNotification(Boolean isLocationUpdateAllowed)
        {
            Boolean isAdmin = false;
            AppointmentOrderScheduleContract appOrdSchdContract = FingerPrintSetUpManager.GetAppointmentOrderDetailData(View.CurrentLoggedInUserId, isAdmin, View.TenantId.ToString(), View.AppointSlotContract.ApplicantAppointmentId);
            if (!appOrdSchdContract.IsNullOrEmpty() && !View.AppointSlotContract.IsNullOrEmpty())
            {
                AppointmentSlotContract appointmentSlotContract = new AppointmentSlotContract();
                appointmentSlotContract.IsLocationUpdate = isLocationUpdateAllowed;
                appointmentSlotContract.LocationId = appOrdSchdContract.LocationId;
                appointmentSlotContract.SlotDate = appOrdSchdContract.AppointmentDate;
                appointmentSlotContract.SlotStartTime = appOrdSchdContract.StartTime;
                appointmentSlotContract.SlotEndTime = appOrdSchdContract.EndTime;
                appointmentSlotContract.ApplicantOrgUserId = appOrdSchdContract.ApplicantOrgUserId;
                appointmentSlotContract.IsEventType = View.AppointSlotContract.IsEventType;
                appointmentSlotContract.EventName = View.AppointSlotContract.EventName;
                appointmentSlotContract.EventDescription = View.AppointSlotContract.EventDescription;
                var res = FingerPrintSetUpManager.SendFingerPrintAppointmentMailNotification(appOrdSchdContract, appointmentSlotContract, true);
            }
        }


        public void GetBkgOrderWithAppointmentData()
        {
            View.AppointSlotContract = new AppointmentSlotContract();
            if (View.TenantId > AppConsts.NONE && View.OrderId > AppConsts.NONE && View.CurrentLoggedInUserId > AppConsts.NONE)
                View.AppointSlotContract = FingerPrintDataManager.GetBkgOrderWithAppointmentData(View.TenantId, View.OrderId, View.CurrentLoggedInUserId);
        }

        public Order GetOrderByOrderId(Int32 orderId)
        {
            return ComplianceDataManager.GetOrderById(View.TenantId, orderId);
        }
        public string GetBkgOrderServiceDetails(int orderID)
        {
            return BackgroundProcessOrderManager.GetBkgOrderServiceDetails(View.TenantId, orderID);
        }
        public List<OrderPaymentDetail> GetOrderPaymentDetails(Order currentOrder)
        {
            List<OrderPaymentDetail> PaymentDetail = new List<OrderPaymentDetail>();
            if (!currentOrder.IsNullOrEmpty() && !currentOrder.OrderPaymentDetails.IsNullOrEmpty())
            {
                String ordrPkgTypeComplianceRushOrderCode = OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue();
                //var sentForOnlinePaymentDetailTemp = currentOrder.OrderPaymentDetails.Where(cnd => cnd.lkpOrderStatu != null
                //                                                                     && cnd.lkpOrderStatu.Code == sentForOnlinePaymentCode && !cnd.OPD_IsDeleted).ToList();

                var PaymentDetailTemp = currentOrder.OrderPaymentDetails.Where(cnd => cnd.lkpOrderStatu != null
                                                                     && !cnd.OPD_IsDeleted).ToList();

                foreach (var opd in PaymentDetailTemp)
                {
                    if (!opd.OrderPkgPaymentDetails.Any(OPPD => !OPPD.OPPD_IsDeleted && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeComplianceRushOrderCode))
                    {
                        PaymentDetail.Add(opd);
                    }
                }
            }

            return PaymentDetail;
        }
        public Boolean IsBackgroundPackageIncluded(List<OrderPaymentDetail> sentForOnlinePaymentDetailList)
        {
            String ordrPkgTypeBkgCode = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();
            Boolean isBackgroundPackageIncluded = false;
            if (!sentForOnlinePaymentDetailList.IsNullOrEmpty())
            {
                foreach (var opd in sentForOnlinePaymentDetailList)
                {
                    isBackgroundPackageIncluded = opd.OrderPkgPaymentDetails.Any(OPPD => OPPD.OPPD_BkgOrderPackageID != null && !OPPD.OPPD_IsDeleted
                                                                          && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeBkgCode);
                    if (isBackgroundPackageIncluded)
                    {
                        break;
                    }
                }
            }
            return isBackgroundPackageIncluded;
        }
        public List<BkgOrderPackage> GetBkgOrderPackageDetail(List<OrderPaymentDetail> sentForOnlinePaymentDetailList)
        {
            String ordrPkgTypeBkgCode = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();
            List<Int32> lstBopIDs = new List<Int32>();
            List<BkgOrderPackage> lstBkgOrderPackageList = new List<BkgOrderPackage>();
            foreach (var opd in sentForOnlinePaymentDetailList)
            {
                var bopIds = opd.OrderPkgPaymentDetails.Where(OPPD => OPPD.OPPD_BkgOrderPackageID != null && !OPPD.OPPD_IsDeleted
                                                               && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeBkgCode).Select(slct => slct.OPPD_BkgOrderPackageID.Value)
                                                               .ToList();
                lstBopIDs.AddRange(bopIds);
            }
            View.BopIds = lstBopIDs;
            lstBkgOrderPackageList = BackgroundProcessOrderManager.GetBackgroundOrderPackageListById(View.TenantId, lstBopIDs);
            return lstBkgOrderPackageList;

        }
        public String GetDecryptedSSN(Int32 orgUserID)
        {
            return ComplianceSetupManager.GetFormattedString(orgUserID, true, View.TenantId);
        }
        public bool IsUserGroupCustomAttributeExist(String useTypeCode, Int32 selectedDPMId)
        {
            return ComplianceDataManager.GetCustomAttributesByNodes(useTypeCode, selectedDPMId, View.CurrentLoggedInUserId, View.TenantId)
                .Any(x => x.CADataTypeCode.ToLower().Trim() == CustomAttributeDatatype.User_Group.GetStringValue().ToLower().Trim());
        }
        public vw_AddressLookUp GetAddressLookupByHandlerId(String addressHandleId)
        {
            Guid addHandleId = new Guid(addressHandleId);
            return ComplianceDataManager.GetAddressLookupByHandlerId(addHandleId, View.TenantId);
        }
    }
}
