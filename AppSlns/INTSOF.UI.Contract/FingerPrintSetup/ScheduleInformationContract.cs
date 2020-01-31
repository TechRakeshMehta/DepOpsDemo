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
    public class ScheduleInformationContract
    {
        [DataMember]
        public Int32 LocationID { get; set; }
        [DataMember]
        public int ScheduleID { get; set; }
        [DataMember]
        public Boolean IsPendingChanges { get; set; }
        [DataMember]
        public DateTime? LastBookedAppointmentDate { get; set; }
    }
}
