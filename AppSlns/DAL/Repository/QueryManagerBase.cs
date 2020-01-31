#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename:  QueryManagerBase.cs
// Purpose: 
//

#endregion

#region Using Directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

#endregion


namespace DAL.Repository
{
    /// <summary>
    /// The query manager.
    /// </summary>
    /// <remarks></remarks>
   public abstract class QueryManagerBase
   {
       #region Constants and Fields

       /// <summary>
       ///   The _query type cache.
       /// </summary>
       private readonly ConcurrentDictionary<Type, ConcurrentDictionary<object, object>> _queryTypeCache = new ConcurrentDictionary<Type, ConcurrentDictionary<object, object>>();

       #endregion

       #region Public Properties

       /// <summary>Instances the specified context.</summary>
       /// <param name="context">The context.</param>
       /// <returns>A ConcurrentDictionary&lt;System.Object,System.Object&gt;</returns>
       public ConcurrentDictionary<object, object> Instance(object context)
       {
           return this._queryTypeCache.GetOrAdd(context.GetType(), new ConcurrentDictionary<object, object>());
       }

       #endregion

       /// <summary>Adds to cache.</summary>
       /// <typeparam name="T">Entity Type</typeparam>
       /// <param name="cache">The cache.</param>
       /// <param name="cacheKey">The cache key.</param>
       /// <param name="result">The result.</param>
       protected void AddToCache<T>(IDictionary<object, object> cache, object cacheKey, T result)
       {
           cache[cacheKey] = result;
       }

   }
}