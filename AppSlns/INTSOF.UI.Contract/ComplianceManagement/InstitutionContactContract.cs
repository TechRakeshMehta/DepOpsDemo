using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class InstitutionContactContract
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Title { get; set; }
        public String EmailAddress { get; set; }
        public String PrimaryPhone { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public Int32? ZipCodeID { get; set; }
        
    }
}
