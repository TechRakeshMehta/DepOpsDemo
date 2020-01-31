using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using BESTX.WEB.IntsofLoggerModel.Interface;
using BESTX.WEB.IntsofCachingModel.Interface.Services;

namespace INTSOF.Contracts
{
    public interface IMVCApplication
    {
        ISysXLoggerService LoggerService{get;}
        ISysXCachingDependencyService CacheDependencyService { get; }
        ISysXAppFabricCacheService AppFabricCacheService { get; }
    }
}
