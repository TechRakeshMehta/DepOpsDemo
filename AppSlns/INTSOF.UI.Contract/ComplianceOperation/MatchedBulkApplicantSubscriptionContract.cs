using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class BulkMatchedApplicantSubscriptionContract
    {
        public Int32 PackageSubscriptionID { get; set; }
        public Int32 ApplicantID { get; set; }
        public String ApplicantFirstName { get; set; }
        public String ApplicantLastName { get; set; }
        public String PackageName { get; set; }
        public String InstitutionHierarchy { get; set; }
        public DateTime ApplicantDOB { get; set; }
    }

}
