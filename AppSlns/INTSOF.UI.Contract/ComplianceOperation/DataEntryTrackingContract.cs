using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class DataEntryTrackingContract
    {
        public Int32 DocumentId { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
        public Int32 ItemImpacted { get; set; }
        public short StatusId { get; set; }
        public Int32 QueueProcessUserId { get; set; }
        public Boolean PostBackOnSamePage { get; set; }

        public List<Int32> AffectedItemIds { get; set; }
        //Production issue Changes [26/12/2016]
        public Int32? DiscardReasonId { get; set; }
        public String DiscardReason { get; set; }
        public String StatusNotes { get; set; }
    }
}
