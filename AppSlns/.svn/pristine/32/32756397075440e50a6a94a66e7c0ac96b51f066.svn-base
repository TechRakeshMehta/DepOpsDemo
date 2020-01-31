using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BESTX.WEB.IntsofLoggerModel.Interface;
using BESTX.WEB.IntsofLoggerModel.Services;
using BESTX.WEB.IntsofCachingModel.Interface.Services;
using BESTX.WEB.IntsofCachingModel.Services;
using BESTX.WEB.IntsofExceptionModel.Interface;

namespace INTSOF.Services
{
    public class SingletonServices
    {
        private volatile static ISysXLoggerService singleTonObjectISysXLoggerService;
        private volatile static ISysXAppFabricCacheService singleTonObjectISysXAppFabricCacheService;
        private volatile static ISysXCachingDependencyService singleTonObjectISysXCachingDependencyService;
        private volatile static ISysXExceptionService singleTonObjectISysXExceptionService;
        private static object lockingObject = new object();

        private SingletonServices()
        { }
        //public static ISysXExceptionService GetSingletonSysXExceptionService()
        //{
        //    object lockingObject = new object();
        //    if (singleTonObjectISysXExceptionService == null)
        //    {
        //        lock (lockingObject)
        //        {
        //            if (singleTonObjectISysXExceptionService == null)
        //            {
        //                singleTonObjectISysXExceptionService = new SysXExceptionService();
        //            }
        //        }
        //    }
        //    return singleTonObjectISysXExceptionService;
        //}

        public static ISysXLoggerService GetSingletonSysXLoggerService()
        {
          
           if (singleTonObjectISysXLoggerService == null)
            {
                lock (lockingObject)
                {
                    if (singleTonObjectISysXLoggerService == null)
                    {
                        singleTonObjectISysXLoggerService = new SysXLoggerService();
                    }
                }
            }
           return singleTonObjectISysXLoggerService;
        }
        public static ISysXAppFabricCacheService GetSingletonSysXAppFabricCacheService()
        {

            if (singleTonObjectISysXAppFabricCacheService == null)
            {
                lock (lockingObject)
                {
                    if (singleTonObjectISysXAppFabricCacheService == null)
                    {
                        singleTonObjectISysXAppFabricCacheService = new SysXAppFabricCacheService();
                    }
                }
            }
            return singleTonObjectISysXAppFabricCacheService;
        }

        public static ISysXCachingDependencyService GetSingletonSysXCachingDependencyService()
        {

            if (singleTonObjectISysXCachingDependencyService == null)
            {
                lock (lockingObject)
                {
                    if (singleTonObjectISysXCachingDependencyService == null)
                    {
                        singleTonObjectISysXCachingDependencyService = new SysXCachingDependencyService();
                    }
                }
            }
            return singleTonObjectISysXCachingDependencyService;
        }

    }
}
