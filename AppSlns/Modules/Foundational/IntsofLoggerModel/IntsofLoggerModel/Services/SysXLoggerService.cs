#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXLoggerService.cs
// Purpose:   SysXLoggerService class
//

#endregion

#region Namespace

#region System Defined

using System;
using System.Configuration;


#endregion

#region Application Specific

using INTSOF.Logger;
using INTSOF.Logger.consts;
using INTSOF.Logger.factory;
using CoreWeb.IntsofLoggerModel.Interface;

#endregion

#endregion

namespace CoreWeb.IntsofLoggerModel.Services
{
    /// <summary>
    /// Logger Service class
    /// </summary>
    public class SysXLoggerService : ISysXLoggerService
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        SysXLoggerFactory sysXLoggerFactory;

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="SysXLoggerService"/> class.
        /// </summary>
        /// <remarks></remarks>
        public SysXLoggerService()
        {
            sysXLoggerFactory = SysXLoggerFactory.GetInstance();
        }

        /// <summary>
        /// Return the instance of Logger Object (eg: EventLogger,FlatFileLogger,DataBase Logger)
        /// </summary>
        /// <returns>ILogger</returns>
        public ILogger GetLogger()
        {
            return sysXLoggerFactory.GetLogger();
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}