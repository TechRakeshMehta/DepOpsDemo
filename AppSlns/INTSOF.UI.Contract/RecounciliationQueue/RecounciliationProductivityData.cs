using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.RecounciliationQueue
{
    public class RecounciliationProductivityData
    {
        public int OrganizationUserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public int TotalCount { get; set; }
        public int TenantID { get; set; }
        public DateTime ProductivityDate   { get; set; }
        public int CreatedBy { get; set; }
    }
}
