using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    public class UpcomingCategoryExpirationContract
    {
        public String Category { get; set; }
        public DateTime? CategoryComplianceExpiryDate { get; set; }

        //UAT:3575
        public String InstitutionHierarchyLabel { get; set; }
    }
}
