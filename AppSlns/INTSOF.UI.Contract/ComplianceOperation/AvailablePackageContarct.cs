using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class AvailablePackageContarct
    {

        public Int32 PackageId { get; set; }
        public Int32? DPP_ID { get; set; }
        public Int32 DPM_ID { get; set; }
        public Int32? BPHM_ID { get; set; }
        public String PackageName { get; set; }
        public Boolean IsCompliancePackage { get; set; }
    }
}
