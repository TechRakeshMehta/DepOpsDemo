using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.FingerPrintSetup
{
    [DataContract]
    [Serializable]
    public class AppointmentSlotContract
    {
        [DataMember]
        public Int32 SlotID { get; set; }
        [DataMember]
        public DateTime? SlotDate { get; set; }
        [DataMember]
        public String SlotStartTime { get; set; }
        [DataMember]
        public String SlotEndTime { get; set; }
        [DataMember]
        public Int32 SlotAppointment { get; set; }
        [DataMember]
        public Int32 LocationId { get; set; }
        [DataMember]
        public Boolean IsAvailable { get; set; }
        [DataMember]
        public Int32 ApplicantAppointmentId { get; set; }
        [DataMember]
        public Int32 ApplicantOrgUserId { get; set; }
        [DataMember]
        public Int32 OrderId { get; set; }
        [DataMember]
        public DateTime? StartDateTime
        {
            get
            {
                if (this.SlotStartTime != null)
                {
                    TimeSpan time = new TimeSpan();
                    TimeSpan.TryParse(this.SlotStartTime, out time);
                    return this.SlotDate.HasValue ? this.SlotDate.Value.Add(time) : (DateTime?)null;
                }
                return this.SlotDate.HasValue ? this.SlotDate.Value : (DateTime?)null;
            }
        }
        [DataMember]
        public DateTime? EndDateTime
        {
            get
            {
                if (this.SlotEndTime != null)
                {
                    TimeSpan time = new TimeSpan();
                    TimeSpan.TryParse(this.SlotEndTime, out time);
                    return this.SlotDate.HasValue ? this.SlotDate.Value.Add(time) : (DateTime?)null;
                }
                return this.SlotDate.HasValue ? this.SlotDate.Value : (DateTime?)null;
            }
        }

        [DataMember]
        public String LocationName { get; set; }
        [DataMember]
        public String LocationAddress { get; set; }
        [DataMember]
        public TimeSpan SlotStartTimeTimeSpanFormat { get; set; }
        [DataMember]
        public TimeSpan SlotEndTimeTimeSpanFormat { get; set; }
        [DataMember]
        public DateTime OrderDate { get; set; }
        [DataMember]
        public Int32 ReservedSlotID { get; set; }
        [DataMember]
        public Boolean IsExpired
        {
            get
            {
                return StartDateTime.HasValue && StartDateTime.Value < DateTime.Now;
            }
        }
        [DataMember]
        public String LocDescription { get; set; }

        [DataMember]
        public String OrderStatusCode { get; set; }

        [DataMember]
        public Int32 FingerPrintDocumentId { get; set; }
        [DataMember]
        public string AppointmentStatus { get; set; }
        [DataMember]
        public Boolean? IsOnsiteAppointment { get; set; }
        [DataMember]
        public bool IsEventType { get; set; }
        [DataMember]
        public String AppointmentStatusCode { get; set; }
        [DataMember]
        public Boolean IsLocationUpdate { get; set; }
        [DataMember]
        public String EventName { get; set; }
        [DataMember]
        public String EventDescription { get; set; }
        [DataMember]
        public Boolean IsOutOfStateAppointment { get; set; }
        [DataMember]
        public Boolean IsRejectedReschedule { get; set; }
        [DataMember]
        public String OrderNumber { get; set; }
    }
}
