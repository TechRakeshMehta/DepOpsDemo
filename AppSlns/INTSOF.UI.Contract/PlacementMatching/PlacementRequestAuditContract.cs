using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.PlacementMatching
{
    [Serializable]
    [DataContract]
    public class PlacementRequestAuditContract
    {
        [DataMember]
        public Int32 RequestId { get; set; }
        [DataMember]
        public String OldValue { get; set; }
        [DataMember]
        public String NewValue { get; set; }
        [DataMember]
        public String ColumnName { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public Int32 CreatedByID { get; set; }
    }
}
