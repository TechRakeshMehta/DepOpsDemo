using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;

namespace CoreWeb.Shell.Views
{
    public class TempFileViewerPresenter : Presenter<ITempFileViewerView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IShellController _controller;
        // public TempFileViewerPresenter([CreateNew] IShellController controller)
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
        public void GetFilePath(Guid id)
        {

            List<TempFile> tempRecords = SecurityManager.GetFilePath(id);

            if (tempRecords != null)
            {
                View.TempFileRecord = tempRecords[0];
                View.FilePath = tempRecords[0].TF_Path;
                View.TotalMinutes = (DateTime.Now - tempRecords[0].TF_CreatedOn).TotalMinutes;
            }
        }
        // TODO: Handle other view events and set state in the view
    }
}




