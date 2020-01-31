using System;
using System.Collections.Generic;
using System.Text;
using Entity;

namespace CoreWeb.Messaging.MasterPages
{
    public interface IMessagingMasterView
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
        /// <summary>
        /// Property to get and set the folderlist
        /// </summary>
        List<lkpMessageFolder> FolderList
        {
            set;
            get;
        }

        /// <summary>
        /// Property to get the CurrentUserID
        /// </summary>
        Int32 CurrentUserID
        {
            get;
        }

        /// <summary>
        /// Property to get the UserGroupID
        /// </summary>
        Int32 UserGroupID
        {
            get;
        }

        /// <summary>
        /// Property to get and set the Types of communication
        /// </summary>
        List<lkpCommunicationType> CommunicationTypeList
        {
            set;
            get;
        }


        /// <summary>
        /// Property to get and set the folderlist
        /// </summary>
        List<MessagingGroup> GroupFolderList
        {
            set;
            get;
        }

        String UserName
        {
            get;
        }

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




