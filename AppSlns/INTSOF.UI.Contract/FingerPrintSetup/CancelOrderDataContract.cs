using Entity;
using Entity.ClientEntity;
using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace INTSOF.UI.Contract.FingerPrintSetup
{
    [Serializable]
    [DataContract]
    public class CancelOrderDataContract
    {
        public string MaxLocScheduleAllowedDays { get; set; }
        public List<OrderPaymentDetail> orderPaymentDetailList { get; set; }
        public AppointmentSlotContract AppointSlotContract { get; set; }
        public AuthNetCustomerProfile customerProfile { get; set; }
        public String Description { get; set; }

    }
}