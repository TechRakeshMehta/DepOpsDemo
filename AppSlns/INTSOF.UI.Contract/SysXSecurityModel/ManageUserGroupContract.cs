#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageUserGroupContract.cs
// Purpose:   Define all the properties which are directly not related to view/ui.
//

#endregion

#region Namespaces

#region System Defined

using System;

#endregion

#region Application Specific
using INTSOF.Utils;

#endregion

#endregion

namespace INTSOF.UI.Contract.IntsofSecurityModel
{
    public class ManageUserGroupContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        /// <summary>
        /// GroupID
        /// </summary>
        public Int32 UserGoupId
        {
            get;
            set;
        }

        /// <summary>
        /// UserGroupName
        /// </summary>
        public String UserGoupName
        {
            get;
            set;
        }

        /// <summary>
        /// UserGroupDescription
        /// </summary>
        public String UserGroupDescription
        {
            get;
            set;
        }

        /// <summary>
        /// UserId
        /// </summary>
        public Int32 UserId
        {
            get;
            set;
        }

        /// <summary>
        /// AspNet_userId
        /// </summary>
        public Guid Aspnet_UserId
        {
            get;
            set;
        }

        /// <summary>
        /// UsersInUserGroupId
        /// </summary>
        public Int32 UsersInUserGroupID
        {
            get;
            set;
        }

        /// <summary>
        /// IsAdmin
        /// </summary>
        public Boolean IsAdmin
        {
            get;
            set;
        }

        /// <summary>
        /// CreatedById
        /// </summary>
        public Int32 CreatedById
        {
            get;
            set;
        }

        /// <summary>
        /// Tenant Id
        /// </summary>
        public Int32 TenantId
        {
            get;
            set;
        }

        #endregion


        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion

        #endregion

        #region Events

        #endregion
    }
}