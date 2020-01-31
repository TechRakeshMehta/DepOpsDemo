using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
using System.Linq;

namespace CoreWeb.Messaging.Views
{
    public class SubscriptionSettingPresenter : Presenter<ISubscriptionSettingView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IMessagingController _controller;
        // public SubscriptionSettingPresenter([CreateNew] IMessagingController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads         
            BindCommunicationEvents();
            BindUserCommunicationEvents();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveUserCommunicationSubscriptionSettings()
        {
            CommunicationManager.AddUserCommunicationSubscriptionSettings(
            View.OrganizationUserId,
            View.SelectedNotificationCommunicationEvents.ToList(),
            View.SelectedReminderCommunicationEvents.ToList(),
            View.OrganizationUserId);

        }

        #region Private Methods

        private Int32 NotificationCommunicationTypeId
        {
            get
            {
                return MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.NOTIFICATION.GetStringValue()).CommunicationTypeID;
            }
        }

        private Int32 AlertCommunicationTypeId
        {
            get
            {
                return MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.ALERTS.GetStringValue()).CommunicationTypeID;
            }
        }

        private Int32 ReminderCommunicationTypeId
        {
            get
            {
                return MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.REMINDERS.GetStringValue()).CommunicationTypeID;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void BindCommunicationEvents()
        {
            View.NotificationCommunicationEvents = CommunicationManager.GetCommunicationEvents(NotificationCommunicationTypeId);
            View.ReminderCommunicationEvents = CommunicationManager.GetCommunicationEvents(ReminderCommunicationTypeId);
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindUserCommunicationEvents()
        {
            View.SelectedNotificationCommunicationEvents =
                CommunicationManager.GetUserCommunicationSubscriptionSettings(View.OrganizationUserId, NotificationCommunicationTypeId)
                .Where(userCommunicationSubscriptionSetting => userCommunicationSubscriptionSetting.IsSubscribedToUser)
                .Select(userCommunicationSubscriptionSetting => userCommunicationSubscriptionSetting.CommunicationEventID);


            View.SelectedReminderCommunicationEvents =
                CommunicationManager.GetUserCommunicationSubscriptionSettings(View.OrganizationUserId, ReminderCommunicationTypeId)
                .Where(userCommunicationSubscriptionSetting => userCommunicationSubscriptionSetting.IsSubscribedToUser)
                .Select(userCommunicationSubscriptionSetting => userCommunicationSubscriptionSetting.CommunicationEventID);
        }

        #endregion
    }
}




