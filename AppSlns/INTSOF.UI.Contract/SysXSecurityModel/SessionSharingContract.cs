using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.SysXSecurityModel
{
    [Serializable]
    public class SessionSharingContract
    {
        public Int32 OrganizationUserId { get; set; }
        public String UserId { get; set; }
        public Int32 SysXBlockId { get; set; }
        public String SysXBlockName { get; set; }
        public GoogleAuthenticationStatus UserGoogleAuthenticated { get; set; }
        public Boolean IsSysXAdmin { get; set; }
        public BusinessChannelTypeMappingData BusinessChannelType { get; set; }
        public Int32? TenantId { get; set; }
        public Int32 WebsiteId { get; set; }
    }
}
