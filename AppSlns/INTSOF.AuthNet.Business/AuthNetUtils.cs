using CoreWeb.IntsofSecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Business.RepoManagers;

namespace INTSOF.AuthNet.Business
{
    public class AuthNetUtils
    {
        private static CoreWeb.IntsofSecurityModel.Interface.Services.ISysXSessionService _sysXSessionService;

        private static CoreWeb.IntsofSecurityModel.Interface.Services.ISysXSessionService SessionService
        {
            get
            {
                if (_sysXSessionService != null) return _sysXSessionService;
                if (HttpContext.Current != null && (HttpContext.Current.ApplicationInstance as INTSOF.Contracts.IWebApplication) != null)
                {
                    _sysXSessionService = (HttpContext.Current.ApplicationInstance as INTSOF.Contracts.IWebApplication).SysXSessionService;
                   
                }
                return _sysXSessionService;
            }
        }

        public static Int32 LoggedInUserTenantID
        {
            get
            {
                if (SessionService == null)
                {
                    return SecurityManager.DefaultTenantID;
                }
                SysXMembershipUser user = (SysXMembershipUser)SessionService.SysXMembershipUser;
                if (user == null)
                {
                    return SecurityManager.DefaultTenantID;

                }
                return user.TenantId.HasValue ? user.TenantId.Value : SecurityManager.DefaultTenantID;
            }
        }

        public static Int32 DefaultTenantID
        {
            get
            {
                return SecurityManager.DefaultTenantID;
            }
        }

        [ThreadStaticAttribute]
        public static Int32 OverrideTenantID;

    }
}
