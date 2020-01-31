#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  BaseManager.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;

#endregion

#region Application Specific

using INTSOF.Utils;
using INTSOF.Logger;
using Entity;
using Entity.ClientEntity;

#endregion

#endregion

namespace Business.RepoManagers
{   
    /// <summary>
    /// class Base Manager
    /// </summary>
    public class BaseManager
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ILogger _logger = null;
        private String _classModule;

        #endregion

        #endregion

        #region Constructor

        public BaseManager()
        {            
            Init();
        }

        #endregion        

        #region Properties

        #region Public Properties

        /// <summary>
        /// Get or Set Class Module.
        /// </summary>
        public String ClassModule
        {
            get
            {
                return _classModule;
            }
            set
            {
                _classModule = value;
            }
        } 

        #endregion

        #region Private Properties

        private ILogger Logger
        {
            get
            {
                if (_logger.IsNull())
                {
                    _logger = BALUtils.LoggerService.GetLogger();
                }
                return _logger;
            }
        }
        

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        private void Init()
        {
            try
            {
                _classModule = GetType().FullName + ".";
            }
            catch (Exception ex)
            {               
                throw (new SysXException(this.ClassModule + SysXException.ShowTrace() + Environment.NewLine + "[" + ex.Message + "]"));
            }
        }

        #endregion

        #region Protected Methods
        /// <summary>
        /// Used to Log the Error Message
        /// </summary>
        /// <param name="infoMessage"></param>
        /// <param name="ex">Exception</param>
        protected void LogError(String infoMessage, Exception ex)
        {
            Logger.Info(infoMessage, ex);
        }

        public static void ClearDBContexts()
        {
            SysXAppDBEntities.DisposeDbContext();
            ADB_LibertyUniversity_ReviewEntities.DisposeDbContext();
            ADBMessageDB_DevEntities.DisposeDbContext();
        }
      
        #endregion

        #endregion
    }
}
