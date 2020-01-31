using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Collections.Generic;

namespace INTSOF.UI.Contract.MobileAPI
{
    public class LinkAccountContract
    {
        public UserContract User { get; set; }
        public LookupContract LookupContract { get; set; }
        public string VerificationPassword { get; set; }        
    }

    public class VerificationAccountContract
    {
        public UserContract User { get; set; }
        public List<AttributesForCustomFormContract> lstCustomAttributes { get; set; }
    }
}
