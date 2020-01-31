using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity;
using INTSOF.Utils;
using System.Linq;
namespace CoreWeb.Messaging.Views
{
    public class SubscriptionSettingByAdminPresenter : Presenter<ISubscriptionSettingByAdminView>
    {
        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private ICommunicationController _controller;
        // public SubscriptionSettingByAdminPresenter([CreateNew] ICommunicationController controller)
        // {
        // 		_controller = controller;
        // }

        #region Private Properties


        public Int32 AlertCommunicationTypeId
        {
            get
            {
                return MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.ALERTS.GetStringValue()).CommunicationTypeID;
            }
        }

        public Int32 NotificationCommunicationTypeId
        {
            get
            {
                return MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.NOTIFICATION.GetStringValue()).CommunicationTypeID;
            }
        }

        public Int32 ReminderCommunicationTypeId
        {
            get
            {
                return MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.REMINDERS.GetStringValue()).CommunicationTypeID;
            }
        }

        #endregion

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
            View.NotificationCommunicationEvents = CommunicationManager.GetCommunicationEvents(NotificationCommunicationTypeId);
            View.ReminderCommunicationEvents = CommunicationManager.GetCommunicationEvents(ReminderCommunicationTypeId);
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads

        }

        #region Methods


        public IEnumerable<UserCommunicationSubscriptionSetting> GetNotificationUserCommunicationSubscriptionSetting(Int32 organizationUserId)
        {
            return CommunicationManager.GetUserCommunicationSubscriptionSettings(organizationUserId, NotificationCommunicationTypeId)
                .Where(x => x.IsSubscribedToAdmin);
        }

        public IEnumerable<UserCommunicationSubscriptionSetting> GetReminderUserCommunicationSubscriptionSetting(Int32 organizationUserId)
        {
            return CommunicationManager.GetUserCommunicationSubscriptionSettings(organizationUserId, ReminderCommunicationTypeId)
                .Where(x => x.IsSubscribedToAdmin);
        }

        public void AddUserCommunicationSubscriptionSettings()
        {
            CommunicationManager.AddUserCommunicationSubscriptionSettings(View.OrganizationUserId, View.SelectedUserCommunicationSubscriptionSettings, View.UnSelectedUserCommunicationSubscriptionSettings);
        }


        public void GetApplicantUsers()
        {
            Entity.CustomPagingArgs customPagingArgs = new Entity.CustomPagingArgs();
            View.GridCustomPaging.DefaultSortExpression = "OrganizationUserID";
            View.GridCustomPaging.SecondarySortExpression = ", FirstName";

            IQueryable<vw_ApplicantUser> applicantUser = CommunicationManager.GetApplicantUsers();
            View.ApplicantUsers = customPagingArgs.ApplyFilterOrSort(applicantUser, View.GridCustomPaging);

            View.VirtualPageCount = View.GridCustomPaging.VirtualPageCount;
            View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
        }
        #endregion

        // TODO: Handle other view events and set state in the view
    }
}




