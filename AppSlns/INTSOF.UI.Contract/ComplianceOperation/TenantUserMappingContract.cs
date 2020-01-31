using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class TenantUserMappingContract
    {
        public Int32 TUM_ID {get;set;}
        public Int32 TenantID { get; set; }
        public String TenantName { get; set; }
        public Int32 OrganizationUserID { get; set; }
        public String UserName { get; set; }
    }
}
