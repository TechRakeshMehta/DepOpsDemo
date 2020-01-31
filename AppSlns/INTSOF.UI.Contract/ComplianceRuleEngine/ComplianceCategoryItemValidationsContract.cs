using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceRuleEngine
{
    public class ComplianceCategoryItemValidationsContract
    {
        public Int32 ComplianceItemValidationId { get; set; }
        public Int32 ComplianceCategoryId { get; set; }
        public Int32 ComplianceCategoryComplianceItemId { get; set; }
        public String ValidationMessage { get; set; }
        public String ValidationElements { get; set; }
        public Boolean IsDeleted { get; set; }
    }
}
