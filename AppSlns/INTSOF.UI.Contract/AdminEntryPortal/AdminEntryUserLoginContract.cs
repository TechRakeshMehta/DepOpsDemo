using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.AdminEntryPortal
{
    [Serializable]
    public class AdminEntryUserLoginContract
    {
        public OrganizationUser organizationUser { get; set; }
        public Int32 OrderId { get; set; }
        public Int32 TenantId { get; set; }
    }
}
