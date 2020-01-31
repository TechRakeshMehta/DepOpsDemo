#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXSecurityService.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Collections.Generic;

#endregion

#region Application Specific

using Entity;
using INTSOF.Utils.Consts;
using Business.RepoManagers;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Services
{
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public class SysXSecurityService : ISysXSecurityService
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables
        private bool _isMenuRefreshRequired = false;
        #endregion

        #endregion

        #region Properties

        #region Public Properties
        /// <summary>
        /// Retrieves the marker value for menu refresh
        /// </summary>
        /// <returns>true if menu refresh is needed else false</returns>
        public bool IsMenuRefreshRequired
        {
            get
            {
                return _isMenuRefreshRequired;
            }
            set
            {
                _isMenuRefreshRequired = value;
            }
        }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region Class Initialization

        #endregion

        #region Public Methods

        #region ISysXSecurityService Methods

        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        /// <param name="userId">user id of the current user.</param>
        /// <returns></returns>
        public List<Int32> GetUserBlocksIds(String userId)
        {
            try
            {
                return SecurityManager.GetUserLineOfBusinessesIds(userId).IsNull() ? null : SecurityManager.GetUserLineOfBusinessesIds(userId).ToList();
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        /// <returns></returns>
        public String GetSysXSystemFunctionKey()
        {
            try
            {
                return SecurityManager.GetSysXConfigValue(SysXSecurityConst.SYSX_SYS_FUN_MENU_KEY_NAME);
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Retrieves the user id of sysx admin.
        /// </summary>
        /// <returns></returns>
        public List<Int32> GetSysXAdminUserIds()
        {
            try
            {
                return SecurityManager.GetSysXAdminUserIds().ToList();
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Checks if the feature is available for user?
        /// </summary>
        /// <param name="userName">User name of the current user.</param>
        /// <param name="featureId">Feature id of the current user.</param>
        /// <param name="blockId">Block id of the current user.</param>
        /// <returns></returns>
        public Boolean IsFeatureAvailableToUser(String userName, Int32 featureId, Int32 blockId)
        {
            return SecurityManager.IsFeatureAvailableToUser(userName, featureId, blockId);
        }

        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        /// <param name="userId">User id of the current user.</param>
        /// <param name="featureId">Feature's id.</param>
        /// <param name="blockId">Block's id.</param>
        /// <returns></returns>
        public Permission GetMyPermission(String userId, List<Int32> lstFeatureID, Int32 blockId)
        {
            return SecurityManager.GetMyPermission(userId, lstFeatureID, blockId);
        }

        /// <summary>
        /// Retrieves the application configuration value based on SysXKey.
        /// </summary>
        /// <param name="sysXKey">The sys X key.</param>
        public String GetSysXConfigValue(String sysXKey)
        {
            return SecurityManager.GetSysXConfigValue(sysXKey);
        }

        #endregion

        #endregion

        #region Private Methods

        #endregion

        #endregion

        #region Events

        #endregion

    }
}