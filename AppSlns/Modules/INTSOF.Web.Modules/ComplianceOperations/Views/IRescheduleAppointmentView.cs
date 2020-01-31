using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
  public  interface IRescheduleAppointmentView
    {
        Int32 SelectedSlotID { get; set; }
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// returns the Current Logged In User Id.
        /// </summary>
        Int32 CurrentLoggedInUserId
        {
            get;
        }
        Int32 OrderId
        {
            get;
            set;
        }

        String ParentControl
        {
            get;
            set;
        }

        AppointmentSlotContract AppointSlotContract
        {
            get;
            set;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Boolean IsFingerPrintSvcAvailable
        {
            get;
            set;
        }
        Boolean IsPassportPhotoSvcAvailable
        {
            get;
            set;
        }
        Int32 SubscriptionOptionID
        {
            get;
            set;
        }
        List<Int32> BopIds { get; set; }
    }
}
