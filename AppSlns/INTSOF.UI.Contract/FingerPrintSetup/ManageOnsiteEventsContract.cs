using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using INTSOF.Utils;


namespace INTSOF.UI.Contract.FingerPrintSetup
{
    [Serializable]
    [DataContract]
    public class ManageOnsiteEventsContract
    {
        [DataMember]
        public int LocationEventId { get; set; }
        [DataMember]
        public int LocationId { get; set; }
        [DataMember]
        public string LocationEventName { get; set; }
        [DataMember]
        public string LocationEventDesc { get; set; }
        [DataMember]
        public DateTime LocationEventFromDate { get; set; }
        [DataMember]
        public DateTime LocationEventToDate { get; set; }
        [DataMember]
        public bool LocationEventIsPublished { get; set; }
        [DataMember]
        public DateTime? LocationEventPublishedDate { get; set; }
        [DataMember]
        public Int32 TotalCount { get; set; }
        [DataMember]
        public Int32 CurrentPageIndex { get; set; }
        [DataMember]
        public Int32 CurrentPageSize { get; set; }
        [DataMember]
        CustomPagingArgsContract GridCustomPaging { get; set; }
    }
}
