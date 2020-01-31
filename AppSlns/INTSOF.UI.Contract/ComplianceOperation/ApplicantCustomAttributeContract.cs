using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class ApplicantCustomAttributeContract
    {       
        public Int32 CAM_ID { get; set; }
        public Int32 CAV_ID { get; set; }
        public String CAV_AttributeValue { get; set; }
        public Int32 HierarchyNodeID { get; set; }       
    }
}
