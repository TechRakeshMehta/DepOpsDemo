using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ComplianceRuleSetContract
    {
        public Int32 RuleSetId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Version { get; set; }
        public DateTime? StartDate { get; set; }
        public Int32 PrevRuleSetID { get; set; }
        public Int32 RuleSetTypeID { get; set; }
        public Boolean IsActive { get; set; }
        public Int32? TenantID { get; set; }
    }
}
