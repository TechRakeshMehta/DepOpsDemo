using System;
using System.Collections.Generic;
using System.Text;
using Entity;

namespace CoreWeb.Messaging.Views
{
    public interface ISubscriptionSettingView
    {

        /// <summary>
        /// 
        /// </summary>
        Int32 OrganizationUserId { get; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<lkpCommunicationEvent> NotificationCommunicationEvents { set; }

        

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<lkpCommunicationEvent> ReminderCommunicationEvents { set; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<Int32> SelectedNotificationCommunicationEvents { get; set; }

       

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<Int32> SelectedReminderCommunicationEvents { get; set; }
    }
}




