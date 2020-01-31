using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.FingerPrintSetup
{
    [Serializable]
    [DataContract]
    public class AppointmentOrderScheduleContract
    {
        [DataMember]
        public String FirstName { get; set; }
        [DataMember]
        public String LastName { get; set; }
        [DataMember]
        public String LocationName { get; set; }

        [DataMember]
        public String LocationDescription { get; set; }
        [DataMember]
        public Int32 OrderId { get; set; }
        [DataMember]
        public String OrderNumber { get; set; }
        [DataMember]
        public Int32 LocationId { get; set; }
        [DataMember]
        public DateTime? AppointmentDateFrom { get; set; }
        [DataMember]
        public DateTime? AppointmentDateTo { get; set; }
        [DataMember]
        public DateTime? AppointmentDate { get; set; }

        public String StartTime { get; set; }
        [DataMember]
        public String EndTime { get; set; }
        [DataMember]
        public Int32 SlotId { get; set; }
        [DataMember]
        // filter properties
        public String AppFirstNameFilter { get; set; }
        [DataMember]
        public String AppLastNameFilter { get; set; }
        [DataMember]
        public String OrderIdFilter { get; set; }
        [DataMember]
        public DateTime? AppointmentDateFilter { get; set; }
        [DataMember]
        public Int32 ApplicantAppointmentId { get; set; }
        [DataMember]
        public Int32 ApplicantOrgUserId { get; set; }
        [DataMember]
        public String PermissionName { get; set; }
        [DataMember]
        public Int32 TotalCount { get; set; }
        [DataMember]
        public string TenantID { get; set; }
        [DataMember]
        public string PackageName { get; set; }
        [DataMember]
        public string LocationAddress { get; set; }
        [DataMember]
        public Int32 HeirarchyNodeId { get; set; }
        [DataMember]
        public String ApplicantEmail { get; set; }
        [DataMember]
        public Decimal TotalOrderPrice { get; set; }
        [DataMember]
        public DateTime OrderDate { get; set; }
        [DataMember]
        public String OrderStatus { get; set; }
        [DataMember]
        public String ShipmentStatus { get; set; }
        [DataMember]
        public String PaymentStatus { get; set; }
        [DataMember]
        public String PaymentType { get; set; }
        [DataMember]
        public DateTime? CompletionDate { get; set; }
        [DataMember]
        public String FingerPrintTech { get; set; }
        [DataMember]
        public String CBI_FBI_Status { get; set; }
        // session property, case of redirect
        [DataMember]
        public String TenantIds { get; set; }
        [DataMember]
        public Int32 OPD_ID { get; set; }
        [DataMember]
        public String FormattedStartTime
        {
            get
            {
                return this.StartTime.IsNullOrEmpty() ? null : DateTime.ParseExact(this.StartTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
            }
        }
        [DataMember]
        public String FormattedEndTime
        {
            get
            {
                return this.EndTime.IsNullOrEmpty() ? null : DateTime.ParseExact(this.EndTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
            }
        }
        //[DataMember]
        //public DateTime EndDateFilter { get; set; }
        [DataMember]
        public String PaymentStatusCode { get; set; }
        [DataMember]
        public Int32 FingerPrintDocumentId { get; set; }
        [DataMember]
        public Int32 CurrentPageIndex { get; set; }
        [DataMember]
        public Int32 CurrentPageSize { get; set; }

        public DateTime? OrderFromDate { get; set; }

        public DateTime? OrderToDate { get; set; }
        public Boolean IsContractLocation { get; set; }

        //UAT-3734
        [DataMember]
        public DateTime? StartDateTime
        {
            get
            {
                if (this.StartTime != null)
                {
                    TimeSpan time = new TimeSpan();
                    TimeSpan.TryParse(this.StartTime, out time);
                    return this.AppointmentDate.HasValue ? this.AppointmentDate.Value.Add(time) : (DateTime?)null;
                }
                return this.AppointmentDate.HasValue ? this.AppointmentDate.Value : (DateTime?)null;
            }
        }
        [DataMember]
        public DateTime? EndDateTime
        {
            get
            {
                if (this.EndTime != null)
                {
                    TimeSpan time = new TimeSpan();
                    TimeSpan.TryParse(this.EndTime, out time);
                    return this.AppointmentDate.HasValue ? this.AppointmentDate.Value.Add(time) : (DateTime?)null;
                }
                return this.AppointmentDate.HasValue ? this.AppointmentDate.Value : (DateTime?)null;
            }
        }

        [DataMember]
        public String AppointmentStatus { get; set; }
        [DataMember]
        public String LocationIds { get; set; }
        [DataMember]
        public string AppointmentStatusIds { get; set; }
        [DataMember]
        public string ShipmentStatusIds { get; set; }
        [DataMember]
        public String OrderStatusCode { get; set; }
        [DataMember]
        public Boolean IsOnsiteAppointment { get; set; }

        [DataMember]
        public Int32 FingerPrintingLocation { get; set; }

        [DataMember]
        public String FingerPrintingSite { get; set; }
        [DataMember]
        public String AppointmentStatusCode { get; set; }
        [DataMember]
        public Boolean IsOutOfStateAppointment { get; set; }
        [DataMember]
        public List<String> lstPaymentStatusCode
        {
            get
            {
                if (!PaymentStatusCode.IsNullOrEmpty())
                    return PaymentStatusCode.Split(',').ToList();
                return new List<String>();
            }
        }
        [DataMember]
        public String ResonedFingerprinting { get; set; }
        [DataMember]
        public String CBIPCNNumber { get; set; }
        [DataMember]
        public String ABIPCNNumber { get; set; }   
        [DataMember]
        public String ApplicantPhone { get; set; }
        #region UAT - 4242
        [DataMember]
        public String UserFullName { get; set; }
        #endregion

        #region UAT - 4205
        [DataMember]
        public String CbiUniqueId { get; set; } //// for display grid data
        
        [DataMember]
        public String CbiUniqueids { get; set; } //// for filter data 
        #endregion
        [DataMember]
        public Int32 ApplicantAppointmentDetailID { get; set; }
        public string FingerPrintCardFileName { get; set; }
        public string PassportPhotoFileName { get; set; }
        public string FingerPrintCardDocID { get; set; }
        public string PassportPhotoDocID { get; set; }
    }
}
