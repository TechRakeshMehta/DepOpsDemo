using CoreWeb.IntsofCachingModel.Interface.Services;
using CoreWeb.IntsofExceptionModel.Interface;
using CoreWeb.IntsofLoggerModel.Interface;

namespace INTSOF.ServiceModelInterface
{
    public interface IIntsofService
    {
        ISysXLoggerService LoggerService { get; }
        ISysXCachingDependencyService CacheDependencyService { get; }
        ISysXAppFabricCacheService AppFabricCacheService { get; }
        ISysXExceptionService ExceptionService { get; }

    }
}
