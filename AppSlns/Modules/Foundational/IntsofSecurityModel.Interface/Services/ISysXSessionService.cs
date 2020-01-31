#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ISysXSessionService.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web.Security;
using System.Web.UI;

#endregion

#region Application Specific

using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Interface.Services
{
    /// <summary>
    /// This interface handles session services.
    /// </summary>
    /// <remarks></remarks>
    public interface ISysXSessionService
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Get current User Id from Session
        /// </summary>
        /// <remarks></remarks>
        String UserId
        {
            get;
        }

        /// <summary>
        /// Gets the organization user id.
        /// </summary>
        /// <remarks></remarks>
        Int32 OrganizationUserId
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is sys X admin.
        /// </summary>
        /// <remarks></remarks>
        Boolean IsSysXAdmin
        {
            get;
        }

        /// <summary>
        /// Get SysXMembershipUser from Session
        /// </summary>
        /// <remarks></remarks>
        MembershipUser SysXMembershipUser
        {
            get;
        }

        /// <summary>
        /// Get current SysXBlockId from Session
        /// </summary>
        /// <remarks></remarks>
        Int32 SysXBlockId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the sys X block.
        /// </summary>
        /// <remarks></remarks>
        String SysXBlockName
        {
            get;
        }

        #endregion

        #region Events

        #endregion

        #region Methods

        /// <summary>
        /// Set current user id in Session.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <remarks></remarks>
        void SetUserId(String userId);

        /// <summary>
        /// Set SysXMembershipUser object in Session
        /// </summary>
        /// <param name="sysXMembershipUser">SysXMembershipUser</param>
        /// <remarks></remarks>
        void SetSysXMembershipUser(MembershipUser sysXMembershipUser);

        /// <summary>
        /// Set Current SysXBlockId in Session
        /// </summary>
        /// <param name="sysXBlockId">SysXBlockId</param>
        /// <remarks></remarks>
        void SetSysXBlockId(Int32 sysXBlockId);

        /// <summary>
        /// Sets the name of the sys X block.
        /// </summary>
        /// <param name="blockName">Name of the block.</param>
        /// <remarks></remarks>
        void SetSysXBlockName(String blockName);

        /// <summary>
        /// Set custom data in Session
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <remarks></remarks>
        void SetCustomData(String key, Object data);

        /// <summary>
        /// Get custom data from Session
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        Object GetCustomData(String key);

        /// <summary>
        /// Clears the data in session.
        /// </summary>
        /// <param name="doAbandon">if set to <c>true</c> [do abandon].</param>
        /// <remarks></remarks>
        void ClearSession(Boolean doAbandon);

        /// <summary>
        /// Gets the session view state persister.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        PageStatePersister GetSessionViewStatePersister(Page page);

        /// <summary>
        /// Compresses the state of the view.
        /// </summary>
        /// <param name="uncompData">The uncomp data.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        Byte[] CompressViewState(Byte[] uncompData);

        /// <summary>
        /// Decompresses the state of the view.
        /// </summary>
        /// <param name="compData">The comp data.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        Byte[] DecompressViewState(Byte[] compData);

        #endregion

        BusinessChannelTypeMappingData BusinessChannelType { get; set; }

        //UAT-2930
        GoogleAuthenticationStatus UserGoogleAuthenticated{ get; set; }
    }
}