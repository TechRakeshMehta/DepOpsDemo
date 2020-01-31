using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceRuleEngine
{
    public class ClientComplianceItemValidationsContract
    {
        public Int32 ClientComplianceItemValidationId { get; set; }
        public Int32 ClientComplianceItemId { get; set; }
        public Int32 ClientComplianceCategoryId { get; set; }
        public String ValidationMessage { get; set; }
        public String ValidationElements { get; set; }
        public Boolean IsDeleted { get; set; }
    }
}
