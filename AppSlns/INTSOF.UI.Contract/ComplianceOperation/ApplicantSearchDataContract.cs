using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class ApplicantSearchDataContract
    {
        public Int32 OrganizationUserId { get; set; }
        public String ApplicantFirstName { get; set; }
        public String ApplicantLastName { get; set; }
        public String EmailAddress { get; set; }
        public String TenantName { get; set; }
        public String ApplicantSSN { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Int32? ClientID { get; set; }
    }
}
