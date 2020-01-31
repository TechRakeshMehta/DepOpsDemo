using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.FingerPrintSetup
{
    [Serializable]
    [DataContract]
    public class ReserveSlotContract
    {
        [DataMember]
        public Int32 ReservedSlotID {get;set;}
        [DataMember]
        public Int32 TenantID {get;set;}
        [DataMember]
        public Int32 AppOrgUserID {get;set;}
        [DataMember]
        public Int32 OrderID {get;set;}
        [DataMember]
        public Int32 LocationID{get;set;}
        [DataMember]
        public Int32 ServiceLineItemID {get;set;}
        [DataMember]
        public Int32? SlotID {get;set;}
        [DataMember]
        public Boolean IsConfirmed{get;set;}
        [DataMember]
        public String ErrorMsg{get;set;}
        [DataMember]
        public String SuccessMsg{get;set;}
        [DataMember]
        public Int32 ApplicantAppointmentID { get; set; }
        [DataMember]
        public Boolean IsAvailable { get; set; }

        [DataMember]
        public Boolean IsEventTypeCode { get; set; }
        [DataMember]
        public Boolean IsLocationUpdate { get; set; }
        [DataMember]
        public bool IsOutOfState { get; set; }
        [DataMember]
        public bool IsRejectedReschedule { get; set; }
        [DataMember]
        public String BillingCode { get; set; }
        [DataMember]
        public String CbiUniqueId { get; set; }
        [DataMember]
        public Int32? MailingOptionID { get; set; }
        [DataMember]
        public decimal? MailingOptionPrice { get; set; }
        [DataMember]
        public int? MailingAddressId { get; set; }
       
        [DataMember]
        public List<BkgPackagesData> bkgPackagesData { get; set; }
        [DataMember]
        public bool IsFingerPrintAndPassPhotoService { get; set; }

    }
    public class BkgPackagesData
    {
        [DataMember]
        public Int32 NumberOfCopies { get; set; }
        [DataMember]
        public decimal BasePrice { get; set; }
        [DataMember]
        public string ServiceType { get; set; }

    }
}
