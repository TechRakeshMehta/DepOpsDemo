using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class UpcomingExpirationContract
    {
        public Int32 StudentID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Category { get; set; }
        public String Item { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? NonComplianceDate { get; set; }
        public String ItemStatus { get; set; }
        public String UserGroups { get; set; }
        public String EmailAddress { get; set; }
        public String CustomAttributes { get; set; }
        public Int32 TotalCount { get; set; }
        public String OrderId { get; set; }


    }
}
