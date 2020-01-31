using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class AlertMailForBlockedOrderContract
    {
        public Int32 MaxImpactCount { get; set; }
        public String TenantName { get; set; }
        public Int32 MaxImpactForTenant { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
