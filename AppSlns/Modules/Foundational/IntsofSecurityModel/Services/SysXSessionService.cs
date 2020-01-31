#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXSessionService.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Web.Security;
using System.IO.Compression;

#endregion

#region Application Specific

using INTSOF.Utils;
using INTSOF.Utils.Consts;
using Business.RepoManagers;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using Entity;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Services
{
    /// <summary>
    /// Handles the operations related to session services.
    /// </summary>
    public class SysXSessionService : ISysXSessionService
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private String _sysXAdminRoleName;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Checks if the user is admin or not?
        /// </summary>
        public Boolean IsSysXAdmin
        {
            get
            {
                if (!SysXMembershipUser.IsNull())
                {
                    String[] roles = Roles.GetRolesForUser(SysXMembershipUser.UserName);
                    return roles.Contains(_sysXAdminRoleName);
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the value of SysX membership user.
        /// </summary>
        public MembershipUser SysXMembershipUser
        {
            get
            {
                return (SysXMembershipUser)HttpContext.Current.Session[AppConsts.SESSION_SYSX_USER_KEY];
            }
        }

        /// <summary>
        /// Gets the value for SysX Block's id.
        /// </summary>
        public Int32 SysXBlockId
        {
            get
            {
                Object sysXBlockId = HttpContext.Current.Session[AppConsts.SESSION_SYSX_BLOCKID_KEY];

                if (sysXBlockId.IsNull())
                {
                    return -AppConsts.ONE;
                }
                return (Int32)sysXBlockId;
            }
            set
            {
                HttpContext.Current.Session[AppConsts.SESSION_SYSX_BLOCKID_KEY] = value;
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

        /// <summary>
        /// Handles the operation for session services.
        /// </summary>
        public SysXSessionService()
        {
            _sysXAdminRoleName = SecurityManager.GetSysXConfigValue(SysXSecurityConst.SYSX_ADMIN_ROLE_KEY_NAME);
        }

        /// <summary>
        /// Sets the value for user id.
        /// </summary>
        /// <param name="userId">user id value.</param>
        public void SetUserId(String userId)
        {
            HttpContext.Current.Session.Add(AppConsts.SESSION_ORG_USERID_KEY, userId);
        }

        /// <summary>
        /// Set the membership user information.
        /// </summary>
        /// <param name="sysXMembershipUser">sysX Membership User value.</param>
        public void SetSysXMembershipUser(MembershipUser sysXMembershipUser)
        {
            HttpContext.Current.Session.Add(AppConsts.SESSION_SYSX_USER_KEY, sysXMembershipUser);
        }

        /// <summary>
        /// Sets the value for sysx block's id.
        /// </summary>
        /// <param name="sysXBlockId">sysX Block's Id.</param>
        public void SetSysXBlockId(Int32 sysXBlockId)
        {
            HttpContext.Current.Session.Add(AppConsts.SESSION_SYSX_BLOCKID_KEY, sysXBlockId);
        }

        /// <summary>
        /// Sets the value for sysx block's name.
        /// </summary>
        /// <param name="blockName">value for block's name.</param>
        public void SetSysXBlockName(String blockName)
        {
            HttpContext.Current.Session.Add(AppConsts.SESSION_SYSX_BLOCK_NAME_KEY, blockName);
        }

        /// <summary>
        /// Set the custom data.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="data">Data value.</param>
        public void SetCustomData(String key, Object data)
        {
            HttpContext.Current.Session.Add(key, data);
        }

        /// <summary>
        /// Retrieves the custom data.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <returns></returns>
        public Object GetCustomData(String key)
        {
            return HttpContext.Current.Session[key];
        }

        /// <summary>
        /// Gets the value of current user's id.
        /// </summary>
        public String UserId
        {
            get
            {
                string uID = string.Empty;
                if (!HttpContext.Current.Session[AppConsts.SESSION_SYSX_USER_KEY].IsNullOrEmpty())
                {
                    uID = ((SysXMembershipUser)HttpContext.Current.Session[AppConsts.SESSION_SYSX_USER_KEY]).UserId.ToString();
                }
                return uID;
            }
        }

        /// <summary>
        /// Gets the value of current organization user's id.
        /// </summary>
        public Int32 OrganizationUserId
        {
            get
            {
                if (HttpContext.Current.Session[AppConsts.SESSION_SYSX_USER_KEY] != null)
                {
                    return ((SysXMembershipUser)HttpContext.Current.Session[AppConsts.SESSION_SYSX_USER_KEY]).OrganizationUserId;
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Clears the session value.
        /// </summary>
        /// <param name="doAbandon">value for doAbandon.</param>
        public void ClearSession(Boolean doAbandon)
        {
            if (doAbandon)
            {
                HttpContext.Current.Session.Abandon();
            }
            HttpContext.Current.Session.Clear();
        }

        /// <summary>
        /// Gets the sysX block's name.
        /// </summary>
        public String SysXBlockName
        {
            get
            {
                return (String)HttpContext.Current.Session[AppConsts.SESSION_SYSX_BLOCK_NAME_KEY];
            }
        }

        /// <summary>
        /// Gets the value for Session ViewState Persister.
        /// </summary>
        /// <param name="page">page value.</param>
        /// <returns></returns>
        public PageStatePersister GetSessionViewStatePersister(Page page)
        {
            return new SessionPageStatePersister(page);
        }

        /// <summary>
        /// Compress the ViewState.
        /// </summary>
        /// <param name="uncompData">value for uncompData.</param>
        /// <returns></returns>
        public Byte[] CompressViewState(Byte[] uncompData)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                CompressionMode mode = CompressionMode.Compress;

                // Use the newly created memory stream for the compressed data.
                using (GZipStream gzip = new GZipStream(mem, mode, true))
                {
                    //Writes compressed byte to the underlying
                    //stream from the specified byte array.
                    gzip.Write(uncompData, AppConsts.NONE, uncompData.Length);
                }

                return mem.ToArray();
            }
        }

        /// <summary>
        /// Decompress the ViewState.
        /// </summary>
        /// <param name="compData">value for compData.</param>
        /// <returns></returns>
        public Byte[] DecompressViewState(Byte[] compData)
        {
            using (MemoryStream inputMem = new MemoryStream())
            {
                inputMem.Write(compData, AppConsts.NONE, compData.Length);

                // Reset the memory stream position to begin decompression.
                inputMem.Position = AppConsts.NONE;
                CompressionMode mode = CompressionMode.Decompress;
                GZipStream gzip = new GZipStream(inputMem, mode, true);

                using (MemoryStream outputMem = new MemoryStream())
                {
                    // Read 1024 bytes at a time
                    Byte[] buf = new Byte[1024];
                    Int32 byteRead = -AppConsts.ONE;
                    byteRead = gzip.Read(buf, AppConsts.NONE, buf.Length);

                    while (byteRead > AppConsts.NONE)
                    {
                        //write to memory stream
                        outputMem.Write(buf, AppConsts.NONE, byteRead);
                        byteRead = gzip.Read(buf, AppConsts.NONE, buf.Length);
                    }

                    gzip.Close();
                    return outputMem.ToArray();
                }
            }
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion



        public BusinessChannelTypeMappingData BusinessChannelType
        {

            get
            {
                if (!HttpContext.Current.Session[AppConsts.SESSION_BUSINESS_CHANNEL_TYPE].IsNullOrEmpty())
                    return (BusinessChannelTypeMappingData)HttpContext.Current.Session[AppConsts.SESSION_BUSINESS_CHANNEL_TYPE];
                else
                    return null;

            }
            set
            {
                HttpContext.Current.Session[AppConsts.SESSION_BUSINESS_CHANNEL_TYPE] = value;
            }

        }

        //UAT-2930
        public GoogleAuthenticationStatus UserGoogleAuthenticated { get; set; }
    }
}