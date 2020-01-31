using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class NodesContract
    {
        public Int32 ComplianceCategoryId { get; set; }
        public Int32 ComplianceItemId { get; set; }
        public Int32 DPM_ID { get; set; }
        public String DPM_Label { get; set; }
        public String PackageName { get; set; }
    }
}
