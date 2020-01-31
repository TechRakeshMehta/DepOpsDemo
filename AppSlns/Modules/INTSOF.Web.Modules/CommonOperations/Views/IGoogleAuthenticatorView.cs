using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.CommonOperations.Views
{
    public interface IGoogleAuthenticatorView
    {
        String UserId { get; }
        String AuthenticationCode { get; set; }
        String UserFullName { get; }
        String UserPrimaryEmailAddress { get; }
        Int32 OrgUserId { get; }
        Int32 TenantId { get; }
    }
}
