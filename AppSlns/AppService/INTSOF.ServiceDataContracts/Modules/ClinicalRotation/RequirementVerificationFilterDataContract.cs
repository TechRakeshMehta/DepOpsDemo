using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    public class RequirementVerificationFilterDataContract
    {
        public List<String> FilterColumns { get; set; }
        public List<String> FilterOperators { get; set; }
        public ArrayList FilterValues { get; set; }
        public List<String> FilterTypes { get; set; }

    }
}
