using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class DisclosureDocument
    {
        public String PackageName { get; set; }
        public String DocumentName { get; set; }
        public Int32  InstitutionWebPageID { get; set; }
        public Int32 TotalCount { get; set; }
    }
}
