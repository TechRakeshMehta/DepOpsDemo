using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity;

namespace CoreWeb.Messaging.Views
{
    public class CommunicationNotificationPreviewPresenter : Presenter<ICommunicationNotificationPreviewView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IMessagingController _controller;
        // public CommunicationNotificationPreviewPresenter([CreateNew] IMessagingController controller)
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


        public void GetSystemNotificationDetails()
        {
            SystemCommunication systemCommunication = CommunicationManager.GetSystemNotificationDetails(View.SystemCommunicationId);
            if (systemCommunication != null)
            {
                View.DetailedContent = systemCommunication.Content;
                View.Subject = systemCommunication.Subject;
            }
            else {
                systemCommunication = CommunicationManager.GetSystemNotificationDetailsArchive(View.SystemCommunicationId);
                View.DetailedContent = systemCommunication.Content;
                View.Subject = systemCommunication.Subject;
            }
        }

        // TODO: Handle other view events and set state in the view
    }
}




