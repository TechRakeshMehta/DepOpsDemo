using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
    [DataContract]
    public class AdminOrderSearchContract
    {
        [DataMember]
        public Int32 ClientId { get; set; }
        [DataMember]
        public Int32 OrderID { get; set; }
        [DataMember]
        public String OrderNumber { get; set; }
        [DataMember]
        public String LastName { get; set; }
        [DataMember]
        public String FirstName { get; set; }
        [DataMember]
        public String SSN { get; set; }
        [DataMember]
        public DateTime? DOB { get; set; }
        [DataMember]
        public String OrderHierarchy { get; set; }
        [DataMember]
        public String ReadyToTransmit { get; set; }
        [DataMember]
        public CustomPagingArgsContract gridCustomPaging { get; set; }
        [DataMember]
        public Dictionary<Int32, Boolean> DicSelectedOrders { get; set; }
        [DataMember]
        public String HierarchyLable { get; set; }
    }
}
