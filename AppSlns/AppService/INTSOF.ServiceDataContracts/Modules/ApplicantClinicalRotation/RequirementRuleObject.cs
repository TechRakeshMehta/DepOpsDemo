using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation
{
   public class RequirementRuleObject
    {
        public String RuleObjectTypeId { get; set; }
        public String RuleObjectTypeCode { get; set; }
        public String RuleObjectId { get; set; }
        public String RuleObjectParentId { get; set; }
    }
}
