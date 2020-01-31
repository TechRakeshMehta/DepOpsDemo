using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.SearchUI
{
    [Serializable]
    public class ApplicantData
    {
        public Int32 OrganizationUserID { get; set; }
        public String UserID { get; set; }
        public String InstitutionName { get; set; }
        public String IsInMultipleInstitutions { get; set; }
        public String ApplicantFirstName { get; set; }
        public String ApplicantUserName { get; set; }
        public String ApplicantLastName { get; set; }
        public String SSN { get; set; }
        public DateTime? DOB { get; set; }
        public Int32 TenantID { get; set; }
        public Int32 TotalCount { get; set; }
        public String EmailAddress { get; set; }
        public String UserType { get; set; } //UAT-4020
        public Boolean ApplicantAccountActivated { get; set; }
        public Int32? ClientContactID { get; set; }
    }
}
