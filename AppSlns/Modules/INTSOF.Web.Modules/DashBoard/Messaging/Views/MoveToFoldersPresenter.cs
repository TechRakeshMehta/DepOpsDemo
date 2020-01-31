using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;

namespace CoreWeb.Messaging.Views
{
    public class MoveToFoldersPresenter : Presenter<IMoveToFoldersView>
    {


        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetFolders()
        {
            View.FolderList = MessageManager.GetFolders(View.CurrentUserID, View.UserGroupID);
        }

        /// <summary>
        /// Invoked to set the TenantType as per userid
        /// </summary>
        public void GetQueueType()
        {
            View.QueueType = MessageManager.GetTenantType(View.CurrentUserID);
        }
    }
}




