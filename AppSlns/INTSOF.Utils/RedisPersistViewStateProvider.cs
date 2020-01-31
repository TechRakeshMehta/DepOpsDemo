#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXPersistViewStateProvider.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using Intsof.RedisOutputCacheProvider;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils
{
    public class RedisPersistViewStateProvider : IPersistViewState
    {
        #region Variables

        #region Public Variables
        #endregion

        #region Private Variables
        private RedisCacheProvider _cache = null;
        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        private Int32 ViewStateTimeOutMinutes
        {
            get
            {
                if (ConfigurationManager.AppSettings[SysXCachingConst.CUSTOMVIEWSTATETIMEOUTKEY].IsNotNull())
                {
                    Int32 timeoutMinutes = Convert.ToInt32(ConfigurationManager
                                            .AppSettings[SysXCachingConst
                                            .CUSTOMVIEWSTATETIMEOUTKEY]);
                }

                return 30;
            }
        }

        // As discussed we have to use static return of 30 for now. It means we have to use the old method (ViewStateTimeOutMinutes) for now.
        //private Int32 ViewStateTimeOutMinutes
        //{
        //    get
        //    {
        //        Int32 timeoutMinutes = 30;
        //        if (ConfigurationManager.AppSettings[SysXCachingConst.CUSTOMVIEWSTATETIMEOUTKEY].IsNotNull())
        //        {
        //            timeoutMinutes = Convert.ToInt32(ConfigurationManager
        //                                    .AppSettings[SysXCachingConst
        //                                    .CUSTOMVIEWSTATETIMEOUTKEY]);
        //        }

        //        return timeoutMinutes;
        //    }
        //}

        #endregion

        #endregion

        #region Events
        public RedisPersistViewStateProvider()
        {
            _cache = new RedisCacheProvider();
            _cache.Initialize(SysXCachingConst.CUSTOMVIEWSTATESECTIONNAME, (NameValueCollection)ConfigurationManager.GetSection(SysXCachingConst.CUSTOMVIEWSTATESECTIONNAME));
        }
        #endregion

        #region Methods

        #region public Methods
        /// <summary>
        /// Loads the View state for a page.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="pageUrl">The page URL.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public String Load(String sessionId, String pageUrl)
        {
            try
            {
                string viewData = string.Empty;

                if (!(sessionId.IsNullOrEmpty() || pageUrl.IsNullOrEmpty()))
                {
                    String key = sessionId + SysXCachingConst.DELIMETER_SESSIONPAGEURL + pageUrl;
                    var data = _cache.Get(key);

                    viewData = data.IsNotNull() ? data.ToString() : String.Empty;
                }
                else
                {
                    throw new InvalidOperationException(SysXCachingConst.NULLID_NOTIFICATION);
                }
                return viewData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves the view sate for a page.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="content">The content.</param>
        /// <remarks></remarks>
        public void Save(String sessionId, String pageUrl, String content, Boolean isOverWritable)
        {
            try
            {
                if (!(sessionId.IsNullOrEmpty() || pageUrl.IsNullOrEmpty() || content.IsNullOrEmpty()))
                {
                    String key = sessionId + SysXCachingConst.DELIMETER_SESSIONPAGEURL + pageUrl;
                    if (!isOverWritable)
                        sessionId = string.Empty;
                    if (isOverWritable)
                    {
                        _cache.Set(key, content, DateTime.UtcNow.AddMinutes(ViewStateTimeOutMinutes));
                    }
                    else
                    {
                        _cache.Set(key, content, DateTime.UtcNow.AddMinutes(4 * 60));
                    }
                }
                else
                {
                    throw new InvalidOperationException(SysXCachingConst.NULLID_NOTIFICATION);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deletes the view sate for a page.
        /// </summary>`
        /// <param name="sessionId">The session id.</param>        
        /// <remarks></remarks>
        public void Delete(String sessionId)
        {
            try
            {
                if (!sessionId.IsNullOrEmpty())
                {
                    String wildCardPattern = "*" + sessionId + "*";
                    _cache.RemoveAll(wildCardPattern);
                }
                else
                {
                    throw new InvalidOperationException(SysXCachingConst.NULLID_NOTIFICATION);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion
    }
}