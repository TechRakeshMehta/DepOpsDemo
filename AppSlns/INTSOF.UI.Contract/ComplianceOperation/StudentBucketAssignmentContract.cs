using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class StudentBucketAssignmentContract
    {
        public Int32 OrganizationUserID { get; set; }

        public String ApplicantFirstName { get; set; }

        public String ApplicantLastName { get; set; }

        public String EmailAddress { get; set; }

        public String SSN { get; set; }

        public String InstitutionHierarchy { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public String UserGroups { get; set; }

        public String RotationNameAssigned { get; set; }

        public String ArchiveStatus { get; set; }
        
        public String RotationNotification { get; set; }
        
        public String BackgroundServiceGroupStatus { get; set; }
        
        public String ImmunizationComplianceStatus { get; set; }
        
        public String RotationComplianceStatus { get; set; }

        public String InstituteName { get; set; }

        public Int32 TotalCount { get; set; } 
    }
}
