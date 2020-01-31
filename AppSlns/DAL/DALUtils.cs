#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  DALUtils.cs
// Purpose: Database Utilities
//

#endregion

#region Namespaces

#region System Defined
using System.Web;

#endregion

#region Application Specific

using CoreWeb.IntsofLoggerModel.Interface;
using INTSOF.Utils;
using INTSOF.ServiceUtil;
using INTSOF.Contracts;
using CoreWeb.IntsofSecurityModel.Interface.Services;

#endregion

#endregion

namespace DAL
{
    /// <summary>
    /// This class handles the operations for DAL Utilities.
    /// </summary>
    /// <remarks></remarks>
    public static class DALUtils
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static ISysXLoggerService _sysXLoggerService;
        private static ISysXSessionService _sysXSessionService;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Get instance of ISysXLoggerService
        /// </summary>
        public static ISysXLoggerService LoggerService
        {
            get
            {
                if (_sysXLoggerService.IsNull())
                {
                    if (HttpContext.Current.IsNotNull())
                    {

                        if (HttpContext.Current.ApplicationInstance is IWebApplication)
                        {
                            _sysXLoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                        }
                        else
                            if (HttpContext.Current.ApplicationInstance is INTSOF.ServiceModelInterface.IIntsofService)
                            {
                                _sysXLoggerService = (HttpContext.Current.ApplicationInstance as INTSOF.ServiceModelInterface.IIntsofService).LoggerService;
                            }
                    }
                    else if (ParallelTaskContext.Current.IsNotNull())
                    {
                        _sysXLoggerService = ParallelTaskContext.LoggerService();
                    }
                }

                return _sysXLoggerService;
            }
        }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        //public static Int32 GetLookUpIDbyCode<TEntity>(Func<TEntity, bool> predicate) where TEntity : EntityObject
        //{
        //    TEntity Enity = SysXCacheUtils.GetAddCacheLookup<TEntity>().SingleOrDefault<TEntity>(predicate);
        //    return Convert.ToInt32(Enity.EntityKey.EntityKeyValues.FirstOrDefault().Value);
        //}

        //public static String GetLookUpCodebyID<TEntity>(Func<TEntity, bool> predicate, Func<TEntity, String> selector) where TEntity : EntityObject
        //{
        //    return SysXCacheUtils.GetAddCacheLookup<TEntity>().Where(predicate).Select(selector).FirstOrDefault();
        //    //return Convert.ToInt32(Enity.EntityKey.EntityKeyValues.FirstOrDefault().Value);
        //}        

        #endregion

        #region Private Methods

        #endregion

        #endregion
        #region AMS

        public static ISysXSessionService SessionService
        {
            get
            {
                if (_sysXSessionService.IsNull())
                {
                    if (!HttpContext.Current.IsNull())
                    {
                        _sysXSessionService = (HttpContext.Current.ApplicationInstance as IWebApplication).SysXSessionService;
                    }

                }

                return _sysXSessionService;
            }
        }

        #endregion
    }
}