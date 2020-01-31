using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.Messaging.Views
{
    public interface IMessagingView
    {
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




