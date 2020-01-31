using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Entity.ClientEntity;
using Business.RepoManagers;

namespace CoreWeb.ComplianceOperations.Views
{
    public class MobilitiyNodePackagesPresenter : Presenter<IMobilitiyNodePackagesView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public MobilitiyNodePackagesPresenter([CreateNew] IComplianceOperationsController controller)
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

        public List<MobilityNodePackages> GetMobilityPackages(Int32 nodeId, Int32 nodeDppId, Int32 tenantId)
        {
            return ComplianceDataManager.GetMobilityNodePackages(nodeId, nodeDppId, tenantId);
        }
    }
}




