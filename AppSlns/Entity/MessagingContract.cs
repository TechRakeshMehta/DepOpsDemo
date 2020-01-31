#region Header Comment Block

// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ItemInfoContract.cs
// Purpose:   This Represents Item Info Contract.
//
// Revisions:
// Comment
// -------------------------------------------------
// Initial

#endregion

#region Namespaces

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#region Application Specific
using INTSOF.Utils;
#endregion

#endregion

namespace Entity
{
    /// <summary>
    /// Represents Messaging Contract.
    /// </summary>
    /// 
    [Serializable]
    public class MessagingContract
    {
        #region Properties

        #region Public Properties

        /// <summary>
        /// Get and Set Message Id
        /// </summary>
        public Guid MessageId
        {
            get;
            set;
        }

        /// <summary>
        /// Property to Get and Set Action
        /// </summary>
        public MessagingAction Action
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the MessageMode
        /// </summary>
        public String MessageMode
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the ToList
        /// </summary>
        public String ToList
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the CcList
        /// </summary>
        public String CcList
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the CcList
        /// </summary>
        public String BCcList
        {
            get;
            set;
        }

        public String toUserList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the email ids of cc users
        /// </summary>
        public String CcUserList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the email ids of Bcc users
        /// </summary>
        public String BccUserList
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the ToUserIds
        /// </summary>
        public String ToUserIds
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the CcUserIds
        /// </summary>
        public String CcUserIds
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the BccUserIds
        /// </summary>
        public String BccUserIds
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the CCUserGroupIds
        /// </summary>
        public String CCUserGroupIds
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the Subject
        /// </summary>
        public String Subject
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the Content
        /// </summary>
        public String Content
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set QueueType
        /// </summary>
        public Int32 QueueType
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set CurrentUserId
        /// </summary>
        public Int32 CurrentUserId
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the FolderId
        /// </summary>
        public Int32 FolderId
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set IsHighImportance
        /// </summary>
        public Boolean IsHighImportance
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the TenantTypes
        /// </summary>
        public Dictionary<Int32, String> TenantTypes
        {
            get;
            set;
        }

        ///// <summary>
        ///// Property to get and set ClientTemplates
        ///// </summary>
        //public IQueryable<ClientMessage> ClientTemplates
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Property to get and set CompanyTemplates
        /// </summary>
        public IQueryable<ADBMessage> CompanyTemplates
        {
            get;
            set;
        }

        ///// <summary>
        ///// Property to get and set SupplierTemplates
        ///// </summary>
        //public IQueryable<SupplierMessage> SupplierTemplates
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Property to get and set the UserGroupids
        /// </summary>
        public String ToUserGroupIds
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the from
        /// </summary>
        public String From
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set MessageType
        /// </summary>
        public Int32 MessageType
        {
            get;
            set;
        }

       

        public String DocumentID
        {
            get;
            set;
        }

        public String EVaultDocumentID
        {
            get;
            set;
        }

        public String DocumentName
        {
            get;
            set;
        }

        public String OriginalDocumentName
        {
            get;
            set;
        }

        public String CommunicationType
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set IsHighImportance
        /// </summary>
        public Boolean IsUnRead
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set ApplicationDatabase
        /// </summary>
        public String ApplicationDatabaseName
        {
            get;
            set;
        }
        #endregion

        #endregion


    }
}
