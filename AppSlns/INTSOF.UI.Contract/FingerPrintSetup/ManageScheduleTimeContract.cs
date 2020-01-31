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
  public  class     ManageScheduleTimeContract
    {

        [DataMember]
        public Int32 FingureLocationTimeOffId { get; set; }
        [DataMember]
        public Int32 LocationId { get; set; }
        [DataMember]
        public String LocationName { get; set; }
        [DataMember]
        public String OffReason { get; set; }
        [DataMember]
        public bool Published { get; set; }
        [DataMember]
        public DateTime ScheduleDate { get; set; }     
        [DataMember]
        public DateTime StartDateTime { get; set; }
        [DataMember]
        public DateTime EndDateTime { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public Int32 TotalCount { get; set; }

    }
}
