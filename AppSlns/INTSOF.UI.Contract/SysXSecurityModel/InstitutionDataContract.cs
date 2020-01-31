using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.SysXSecurityModel
{
    [Serializable]
    public class InstitutionDataContract
    {
        public String InstitutionName { get; set; }
        public String InstitutionTypeCode { get; set; }
        public String InstitutionURL { get; set; }
    }
}
