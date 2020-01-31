using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class UniversalMappingContract
    {
        public Int32 TreeNodeTypeID { get; set; }
        public String NodeID { get; set; }
        public String ParentNodeID { get; set; }
        public Int32 Level { get; set; }
        public Int32 DataID { get; set; }
        public Int32? ParentDataID { get; set; }
        public String UICode { get; set; }
        public String Value { get; set; }
        public String Description { get; set; }
    }
}
