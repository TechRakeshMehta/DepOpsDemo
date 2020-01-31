using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class RuleMappingContract
    {
        public Int32 RLM_ID { get; set; }
        public Int32 RLM_RuleSetID { get; set; }
        public String RLM_Name { get; set; }
        public String RLT_Description { get; set; }
        public String RSL_Description { get; set; }
        public String ACT_Description { get; set; }
        public Boolean RLM_IsActive { get; set; }
        public Boolean RLM_IsCurrent { get; set; }
        public Boolean IsRuleAssociationExists { get; set; }
    }
}
