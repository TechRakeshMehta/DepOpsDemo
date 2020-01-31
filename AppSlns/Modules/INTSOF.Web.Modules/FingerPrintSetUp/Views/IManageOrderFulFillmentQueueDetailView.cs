using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IManageOrderFulFillmentQueueDetailView
    {
        Int32 SelectedTenantId { get; set; }
        Int32 ApplicantAppointmentID { get; set; }
        Boolean IsAdminScreen { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        Boolean IsAdminLoggedIn { get; set; }
        Int32 TenantId { get; set; }
        List<ServiceStatusContract> ServiceStatusList { get; set; }        

        AppointmentOrderScheduleContract AppointmentDetailContract { get; set; }

        PreviousAddressContract MailingAddressData
        {
            get;
            set;
        }

        List<OrderPaymentDetail> OrderPaymentDetailList
        {
            get;
            set;
        }


        List<OrderPkgPaymentDetail> OrderPkgPaymentDetailList
        { get; set; }

        String PartialOrderCancellationTypeCode
        {
            get;
            set;
        }

        Boolean ShowApproveRejectButtons { get; set; }

        /// <summary>
        /// Used to get the CustomerProfileId, for Refund process
        /// </summary>
        Guid UserId
        {
            get;
            set;
        }
        Int32 PageSize { get; set; }
        Int32 CurrentPageIndex { get; set; }
        Int32 VirtualRecordCount { get; set; }
        CustomPagingArgsContract GridCustomPaging { get; set; }

        OnlinePaymentTransaction OnlinePaymentTransaction { get; set; }
        Order OrderDetail { get; set; }
        Decimal OnlinePaymentAmount { get; set; }
        Boolean IsFileSentToCBI { get; set; }
        //UAT-3850
        Int32 SelectedOPDID { get; set; }
        Boolean IsHrAdminScreen { get; set; }
        ApplicantFingerPrintFileImageContract ApplicantFingerPrintImagesData { get; set; }
        //List<OrderDetailContract> OrderDetailContractLst { get; set; }
        List<OrderDetailContract> lstOrderDetail { get; set; }
        Boolean IsFingerPrintSvcSelected { get; set; }
        Boolean IsPassportPhotoSvcSelected { get; set; }
    }
}
