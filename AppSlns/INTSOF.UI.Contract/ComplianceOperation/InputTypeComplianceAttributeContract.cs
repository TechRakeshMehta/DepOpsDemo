using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class InputTypeComplianceAttributeContract
    {
        public String Name { get; set; }
        public Int32 ID { get; set; }
        public Int32 InputPriority { get; set; }
        public Boolean Enabled { get; set; }
    }

    [Serializable]
    public class ComplianceAttributeOptionMappingContract
    {
        public String OptionText { get; set; }
        public String OptionValue { get; set; }
        public Int32 OptionId { get; set; }
        public Int32 MappedUniversalOptionID { get; set; }
        public Boolean Enabled { get; set; }
    }
}
