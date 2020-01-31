using System;
using System.Collections.Generic;
using System.Text;
using Entity;

namespace CoreWeb.Messaging.Views
{
    public interface IMoveToFoldersView
    {
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
        /// Property to get the QueueType
        /// </summary>
        Int32 QueueType
        {
            set;
        }
    }
}




