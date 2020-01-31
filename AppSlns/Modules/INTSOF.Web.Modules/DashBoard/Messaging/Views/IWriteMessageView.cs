using System;
using System.Collections.Generic;
using System.Text;
using Entity;
using System.Linq;
using INTSOF.Utils;

namespace CoreWeb.Messaging.Views
{
    public interface IWriteMessageView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties
        #region Public Properties

        /// <summary>
        /// Receiver type
        /// </summary>
        String ReceiverType
        {
            get;
            set;
        }

        /// <summary>
        /// Receiver type id
        /// </summary>
        Int32 ReceiverTypeId
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        Boolean IsDraftMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Get Message Id
        /// </summary>
        Guid MessageId
        {
            get;
            set;
        }
        /// <summary>
        /// Property to get the Action
        /// </summary>
        MessagingAction Action
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the ToList
        /// </summary>
        String ToList
        {
            get;
            set;
        }
        /// <summary>
        /// Property to get and set the CCList
        /// </summary>
        String CCList
        {
            get;
            set;
        }
        /// <summary>
        /// Property to get and set the Subject
        /// </summary>
        String Subject
        {
            get;
            set;
        }
        /// <summary>
        /// Property to get and set the Content
        /// </summary>
        String Content
        {
            get;
            set;
        }
        /// <summary>
        /// Property to get the QueueType
        /// </summary>
        Int32 QueueType
        {
            get;
        }
        /// <summary>
        /// Property to get the CurrentUserId
        /// </summary>
        Int32 CurrentUserId
        {
            get;
        }
        /// <summary>
        /// Property to get and set the TenantTypes
        /// </summary>
        Dictionary<Int32, String> TenantTypes
        {
            get;
            set;
        }
        ///// <summary>
        ///// Property to set the ClientTemplates
        ///// </summary>
        //IQueryable<ClientMessage> ClientTemplates
        //{
        //    set;
        //}
        /// <summary>
        /// Property to set the CompanyTemplates
        /// </summary>
        IQueryable<ADBMessage> CompanyTemplates
        {
            set;
        }
        ///// <summary>
        ///// Property to set the SupplierTemplates
        ///// </summary>
        //IQueryable<SupplierMessage> SupplierTemplates
        //{
        //    set;
        //}

        MessagingContract   ViewContract
        {
            get;
        }

        /// <summary>
        /// Get and set MessageType
        /// </summary>
        Int32 MessageType
        {
            get;
            set;
        }

        /// <summary>
        /// Get and set MessageType
        /// </summary>
        Int32 MessageTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Set MessageType.
        /// </summary>
        IQueryable<lkpMessageType> BindMessageType
        {
            set;
        }


        /// <summary>
        /// Property to get and set ToListIds
        /// </summary>
        String ToListIds
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set CcListIds
        /// </summary>
        String CcListIds
        {
            get;
            set;
        }

        String CcListOfUserForApplicant
        {
            get;
            set;
        }


        
        /// <summary>
        /// Property to get and set CcListIds
        /// </summary>
        String BccListIds
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        String ToListGroupIds { get; set; }


        /// <summary>
        /// 
        /// </summary>
        String ToListUsersForApplicant { get; set; }

        /// <summary>
        /// 
        /// </summary>
        String CcListGroupIds
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool IsApplicant { get; }

        byte[] DocumentFile
        {
            get;
            set;
        }

        String DocumentID
        {
            get;
            set;
        }

        Dictionary<String, String> AttachedFiles
        {
            get;
            set;
        }

        String EVaultDocumentID
        {
            get;
            set;
        }

        String DocumentName
        {
            get;
            set;
        }
        String OriginalDocumentName
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get the CommunicationType
        /// </summary>
        String CommunicationType
        {
            get;
        }

        String FileSize
        {
            set;
        }

        /// <summary>
        /// Gets/Sets the high important
        /// </summary>
        bool IsHighImportant
        {
            get;
            set;
        }
        /// <summary>
        /// UAT-4179
        /// </summary>
        bool IsCopyOfMailToSender
        {
            get;
        }

        /// <summary>
        /// UAT-4179
        /// </summary>
        bool IsNededToShowCopyMeInMailCheckBox
        {
            get;
            set;
        }
         Boolean IsSendMessageSuccess { get; set; }

        #endregion
        #endregion

        #region Methods
        #region Public Methods

        #endregion
        #region Protected Methods

        #endregion
        #region Private Methods

        #endregion

        #endregion
    }
}




