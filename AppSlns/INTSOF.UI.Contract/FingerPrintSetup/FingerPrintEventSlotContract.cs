using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace INTSOF.UI.Contract.FingerPrintSetup
{
    [Serializable]
    [DataContract]
    public class FingerPrintEventSlotContract
    {
        [DataMember]
        public int EventSlotId { get; set; }
        [DataMember]
        public DateTime EventSlot_FromTime { get; set; }
        [DataMember]
        public DateTime EventSlot_ToTime { get; set; }
        [DataMember]
        public string EventSlot_Description { get; set; }
        [DataMember]
        public string EventSlot_EventsCode { get; set; }
        [DataMember]
        public int EventId { get; set; }
        [DataMember]
        public Int32? Increment { get; set; }
        [DataMember]
        public Int32 TotalAppointment { get; set; }
        [DataMember]
        public Int32 TotalCount { get; set; }
    }
}
