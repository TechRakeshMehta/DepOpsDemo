using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IManageAppointmentOrderDetailView
    {
        Int32 SelectedTenantId { get; set; }
        Int32 ApplicantAppointmentID { get; set; }
        Boolean IsAdminScreen { get; set; }
        Int32 CurrentLoggedInUserID { get;}
        Boolean IsAdminLoggedIn { get; set; }
        Int32 TenantId { get; set; }
     
        AppointmentOrderScheduleContract AppointmentDetailContract { get; set; }

        List<OrderPaymentDetail> OrderPaymentDetailList
        {
            get;
            set;
        }

        List<PaymentDetailContract> PaymentDetailContactList
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

        OnlinePaymentTransaction OnlinePaymentTransaction { get; set; }
        Order OrderDetail { get; set; }
        Decimal OnlinePaymentAmount { get; set; }
        Boolean IsFileSentToCBI { get; set; }
        //UAT-3850
        Int32 SelectedOPDID { get; set; }
        Boolean IsHrAdminScreen { get; set; }
        ApplicantFingerPrintFileImageContract ApplicantFingerPrintImagesData { get; set; }

        Boolean IsFingerPrintSvcSelected { get; set; }
        Boolean IsPassportPhotoSvcSelected { get; set; }

    }
}
