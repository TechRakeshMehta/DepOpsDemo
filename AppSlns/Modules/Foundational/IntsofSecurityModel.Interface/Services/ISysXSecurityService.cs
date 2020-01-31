#region Header Comment Block

// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ISysXSecurityService.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;

#endregion

#region Application Specific

using Entity;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Interface.Services
{
    /// <summary>
    /// This interface handles the operations related to security services.
    /// </summary>
    public interface ISysXSecurityService
    {
        #region Variables

        #endregion

        #region Properties

        #endregion

        #region Events

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves a list of all blocks by user id.
        /// </summary>
        /// <param name="userId">user's id.</param>
        /// <returns></returns>
        List<Int32> GetUserBlocksIds(String userId);
        
        /// <summary>
        /// Retrieves function key.
        /// </summary>
        /// <returns></returns>
        String GetSysXSystemFunctionKey();
        
        /// <summary>
        /// Retrieves a list of all admin user's id.
        /// </summary>
        /// <returns></returns>
        List<Int32> GetSysXAdminUserIds();
        
        /// <summary>
        /// Checks if the feature is available for user or not.
        /// </summary>
        /// <param name="userName">user name value.</param>
        /// <param name="featureId">feature's id</param>
        /// <param name="blockId">block's id</param>
        /// <returns></returns>
        Boolean IsFeatureAvailableToUser(String userName, Int32 featureId, Int32 blockId);
        
        /// <summary>
        /// Retrieves the permission for the current user.
        /// </summary>
        /// <param name="userId">user's id.</param>
        /// <param name="featureId">feature's id.</param>
        /// <param name="blockId">block's id.</param>
        /// <returns></returns>
        Permission GetMyPermission(String userId, List<Int32> lstFeatureId, Int32 blockId);

        /// <summary>
        /// Gets the sysX configuration value.
        /// </summary>
        /// <param name="sysXKey">The sys X key.</param>
        /// <returns>
        /// The system x coordinate configuration value.
        /// </returns>        
        String GetSysXConfigValue(String sysXKey);

        /// <summary>
        /// Marker flag for refreshing the menus.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool IsMenuRefreshRequired{get;set;}
        #endregion
    }
}