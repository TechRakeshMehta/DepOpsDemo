#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXGlobalExceptionModule.cs
// Purpose:   Global Exception Module
//

#endregion

#region Namespace

#region System Defined

using System;
using System.Web;


#endregion

#region Application Specific

using CoreWeb.IntsofExceptionModel.Interface;
using INTSOF.Utils;
using INTSOF.Contracts;


#endregion

#endregion

namespace CoreWeb.IntsofExceptionModel
{
    /// <summary>
    /// Global Exception Module
    /// </summary>
    /// <remarks></remarks>
    public class SysXGlobalExceptionModule : IHttpModule
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static ISysXExceptionService _sysXExceptionService;
        private Object _lockThis = new Object();

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// For Initializes
        /// </summary>
        /// <param name="application">HttpApplication</param>
        public virtual void Init(HttpApplication application)
        {
            application.Error += OnError;

        }

        /// <summary>
        /// ExceptionService
        /// </summary>
        /// [ServiceDependency]
        public static ISysXExceptionService ExceptionService
        {
            get
            {
                if (_sysXExceptionService.IsNull())
                {
                    if (HttpContext.Current.ApplicationInstance is IWebApplication)
                    {
                        _sysXExceptionService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                    }
                   
                }
                return _sysXExceptionService;
            }
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
        }

        #endregion

        #region Private Methods

        #endregion

        #region Protected Methods

        /// <summary>
        /// For unhandled Exception, OnError will be fired 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">EventArgs</param>
        protected virtual void OnError(Object sender, EventArgs args)
        {
            lock (_lockThis)
            {
                ExceptionService.HandleError(String.Empty, HttpContext.Current.ApplicationInstance.Server.GetLastError().GetBaseException());
            }
        }

        #endregion

        #endregion
    }
}