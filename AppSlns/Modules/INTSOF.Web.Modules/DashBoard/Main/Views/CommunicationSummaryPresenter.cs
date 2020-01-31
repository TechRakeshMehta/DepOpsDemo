#region Namespaces

#region System Defined
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
#endregion

#region Project Defined
using Business.RepoManagers;
using INTSOF.Utils;
#endregion
#endregion

namespace CoreWeb.Main.Views
{
    public class CommunicationSummaryPresenter : Presenter<ICommunicationSummaryView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IMainController _controller;
        // public CommunicationSummaryPresenter([CreateNew] IMainController controller)
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
        }


        public void GetRecentMessages()
        {
            View.lstRecentMessages = MessageManager.GetRecentMessages(QueueConstants.MESSAGEQUEUE, lkpMessageFolderContext.INBOX.GetStringValue(), View.CurrentUserId);
        }

        public void UpdateReadStatus()
        {
            MessageManager.UpdateReadStatus(View.MessageId, View.CurrentUserId, AppConsts.NONE, false);
        }

        public void DeleteMessage()
        {
            MessageManager.DeleteMesssage(View.MessageId, View.CurrentUserId, lkpMessageFolderContext.INBOX.GetStringValue());
        }


        /// <summary>
        /// Invoked to set the TenantType as per userid
        /// </summary>
        public void GetQueueType()
        {
            View.QueueType = MessageManager.GetTenantType(View.CurrentUserId);
        }
    }
}




