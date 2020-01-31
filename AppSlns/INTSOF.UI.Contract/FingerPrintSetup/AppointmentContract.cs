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
    public class AppointmentContract
    {
        [DataMember]
        public Int32 ID { get; set; }
        [DataMember]
        public String Subject { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public DateTime Start { get; set; }
        [DataMember]
        public DateTime End { get; set; }
        [DataMember]
        public String RecurrenceRule { get; set; }
        [DataMember]
        public Int32? RecurrenceParentID { get; set; }
        [DataMember]
        public String RecurrenceState { get; set; }
        [DataMember]
        public Int32 AppointmentCreatedBy { get; set; }
        [DataMember]
        public Int32? AppointmentModifiedBy { get; set; }
        [DataMember]
        public Int32 Increment { get; set; }
        [DataMember]
        public Int32 TotalAppointment { get; set; }
        [DataMember]
        public Int32 OldScheduleMasterID { get; set; }
        [DataMember]
        public Int32 LocationID { get; set; }
        [DataMember]
        public Int32 NewScheduleMasterID { get; set; }
        [DataMember]
        public Boolean IsExceptionEdit { get; set; }
    }
}
