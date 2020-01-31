using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    public class ApplicantRequirementDataAuditContract
    {
        public int ApplicantRequirementDataAuditID { get; set; }

        public String ApplicantName { get; set; }

        //UAT-3117
        public String ComplioId { get; set; }

        public String PackageName { get; set; }

        public String CategoryName { get; set; }

        public String ItemName { get; set; }

        public String ChangeValue { get; set; }

        public String ChangeBY { get; set; }

        public String ChangeBYInitials { get; set; }

        public int TotalCount { get; set; }

        public DateTime TimeStampValue { get; set; }

    }
}
