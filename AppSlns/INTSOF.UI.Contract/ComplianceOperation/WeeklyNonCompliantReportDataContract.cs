using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class WeeklyNonCompliantReportDataContract
    {
        public Int32 OrganizationID { get; set; }
        public Int32 OrgUserID { get; set; }
        public String EmailAddress { get; set; }
        public String UserFullName { get; set; }
        public String HeirarchyNodeIds { get; set; }
    }
}
