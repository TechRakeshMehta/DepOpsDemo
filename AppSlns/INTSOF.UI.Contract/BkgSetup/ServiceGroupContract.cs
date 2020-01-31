using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    public class ServiceGroupContract
    {
        public Int32 ServiceGroupID { get; set; }
        public String ServiceGroupName { get; set; }
        public String ServiceGroupDesc { get; set; }
        public Boolean Active { get; set; }
        public Int32 TenantID { get; set; }
    }
}
