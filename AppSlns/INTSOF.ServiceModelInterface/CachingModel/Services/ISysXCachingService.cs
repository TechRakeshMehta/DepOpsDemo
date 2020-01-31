#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ISysXCachingService.cs
// Purpose:   Interface  Caching Service
//

#endregion

#region Namespace

#region System Defined

using System;
using System.Web.Caching;

#endregion

#region Application Defined
#endregion

#endregion

namespace CoreWeb.IntsofCachingModel.Interface.Services
{
    /// <summary>
    /// ISysXCachingService for SysXCachingService 
    /// </summary>
    public interface ISysXCachingService
    {

        /// <summary>
        /// Adds an object to MyCache Service
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        void Add(String Key,Object Value);

        /// <summary>
        /// Adds an object to MyCache Service along with a FileDependency
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>        
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="priority"></param>        
        void Add(String Key, Object Value, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority);

        /// <summary>
        /// Adds an object to MyCache Service along with a FileDependency
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>        
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="priority"></param>        
        void Add(String Key, Object Value, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback);

        /// <summary>
        /// Tries to obtain a lock for the specified Key on MyCache service
        /// within the CacheItemRemovedCallBack method
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        Boolean SetLock(String Key);

        /// <summary>
        /// Releases the current lock for the specified Key on MyCache service. Only to be used
        /// within the CacheItemRemovedCallBack method
        /// </summary>
        /// <param name="Key"></param>
        void ReleaseLock(String Key);

        /// <summary>
        /// Removes the corresponding object from MyCache service for the specified Key
        /// </summary>
        /// <param name="Key"></param>
        void Remove(String Key);

        /// <summary>
        /// Get objet from MyCache service for the specified Key
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        Object Get(String Key);

        /// <summary>
        /// Checks whether the corresponding object exists on MyCache service for the Key
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        Boolean Exists(String Key);

    }
}
