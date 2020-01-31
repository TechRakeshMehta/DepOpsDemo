using CoreWeb.IntsofCachingModel.Interface.Services;
using CoreWeb.IntsofExceptionModel.Interface;
using CoreWeb.IntsofLoggerModel.Interface;
using CoreWeb.IntsofSecurityModel.Interface.Services;

namespace INTSOF.Contracts
{
    public interface IWebApplication
    {
        ISysXLoggerService LoggerService { get; }
        ISysXCachingDependencyService CacheDependencyService { get; }
        ISysXAppFabricCacheService AppFabricCacheService { get; }
        ISysXExceptionService ExceptionService { get; }
        ISysXSessionService SysXSessionService { get; }
        ISysXSecurityService SysXSecurityService { get; }
        IAllClientSessionService AllClientSessionService { get; }
    }
}
