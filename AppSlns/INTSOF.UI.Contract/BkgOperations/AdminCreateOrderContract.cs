using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
   public class AdminCreateOrderContract
    {
        public Int32 OrderID { get; set; }
        public String OrderNumber { get; set; }
        public String LastName { get; set; }
        public String FirstName { get; set; }
        public String SSN { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? EntryDate { get; set; }
        public String OrderHierarchy { get; set; }
        public String IsReadyToTransmit { get; set; }
        public Int32 TotalCount { get; set; }
        public Int32 BkgPackageHierarchyMappingID { get; set; }
        public String UserID { get; set; }
        public Int32 HierarchyNodeId { get; set; }
        public Int32 SelectedNodeID { get; set; }
        public String NodeLabel { get; set; }
        public Boolean AttestFCRAPrevisions { get; set; }
        public Int32 BKgOrderID { get; set; }
        public String AdminOrderStatusType { get; set; }
    }
}
