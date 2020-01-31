using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

namespace INTSOF.ServiceUtil
{
    /// <summary>
    /// Service context class, used to store context specification information for non HttpApplication applications, in parallel to HttpContext
    /// </summary>
    public class ServiceContext
    {
        /// <summary>
        /// The applications which use service context, have to set this property true on application start.
        /// </summary> 
        private static bool serviceContextInitialized = false;

        /// <summary>
        /// Stores thread specific context key.
        /// </summary>
        [ThreadStaticAttribute] //Use Thread Local Storage Concept: ThreadStaticAttribute or Thread.SetData, Thread.GetData
        public static String currentContextKey;

        private static Dictionary<string, ServiceContextData> contextItemsData = new Dictionary<string, ServiceContextData>();

        private static Object lockObject = new Object();

        /// <summary>
        /// for getting key for current thread
        /// </summary>
        public static String currentThreadContextKeyString
        {
            get { return currentContextKey; }
        }

        /// <summary>
        /// The applications which use service context, have to call this function on application start.
        /// </summary> 
        public static void init()
        {
            serviceContextInitialized = true;
        }

        /// <summary>
        /// Provides thread specific service context
        /// </summary>
        public static ServiceContextData Current
        {
            get
            {
                if (serviceContextInitialized == true)
                {
                    if (currentContextKey == null)
                    {
                        currentContextKey = Convert.ToString(Guid.NewGuid());
                    }
                }
                else
                {
                    return null;
                }
                lock (lockObject)
                {
                    ServiceContextData serviceContextDataObj;
                    if (!contextItemsData.TryGetValue(currentContextKey, out serviceContextDataObj))
                    {
                        serviceContextDataObj = new ServiceContextData();
                        contextItemsData.Add(currentContextKey, serviceContextDataObj);
                    }
                    ServiceContextData serviceContextDataObject;
                    contextItemsData.TryGetValue(currentContextKey, out serviceContextDataObject);
                    return serviceContextDataObject;
                }
            }
        }

        /// <summary>
        /// Clear all context items specific to current thread.
        /// </summary>
        public static void ReleaseContextItems()
        {
            if (serviceContextInitialized)
            {
                if (Current == null)
                {
                    return;
                }
                else
                {
                    Dictionary<String, ObjectContext> dicDBConext = Current.DBContexts;
                    foreach (var context in dicDBConext)
                    {
                        context.Value.Dispose();
                    }
                    lock (lockObject)
                    {
                        if (Current.DataDict != null)
                        {
                            //foreach (var dic in Current.DataDict)
                            //{
                            //    Current.DataDict.Remove(dic.Key);
                            //}
                            Current.DataDict.Clear();
                        }

                        contextItemsData.Remove(currentContextKey);
                        currentContextKey = null;
                    }
                }
            }
        }
        /// <summary>
        /// Clear database context items specific to current thread.
        /// </summary>
        public static void ReleaseDBContextItems()
        {
            if (serviceContextInitialized)
            {
                if (Current == null)
                {
                    return;
                }
                else
                {
                    Dictionary<String, ObjectContext> dicDBConext = Current.DBContexts;
                    foreach (var context in dicDBConext)
                    {
                        context.Value.Dispose();
                    }
                    lock (lockObject)
                    {
                        if (Current.DataDict != null)
                        {
                            //foreach (var dic in Current.DataDict)
                            //{
                            //    Current.DataDict.Remove(dic.Key);
                            //}
                            Current.DataDict.Clear();
                        }

                        Current.DBContexts.Clear();
                    }
                }
            }
        }
    }
}
