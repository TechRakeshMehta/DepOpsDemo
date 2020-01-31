using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public class RowControlPresenter : Presenter<IRowControlView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public RowControlPresenter([CreateNew] IComplianceOperationsController controller)
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
        public String GetInstructionText( Int32 attributeId, Int32 CIA_Id)
        {
            return ComplianceDataManager.GetInstructionTextByID(View.TenantId, View.PackageId, View.CategoryId, View.ItemId, attributeId, CIA_Id);
        }
        // TODO: Handle other view events and set state in the view

        //UAT-3806
        public List<ListItemEditableBies> GetEditableBiesByCategoryId()
        {
            return ComplianceSetupManager.GetEditableBiesByCategoryId(View.PackageId, View.CategoryId, View.TenantId);
        }


    }
}




