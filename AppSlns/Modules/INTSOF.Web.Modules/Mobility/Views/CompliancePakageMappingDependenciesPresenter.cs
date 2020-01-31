using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.Mobility;
using INTSOF.Utils.Consts;
using System.Xml.Linq;

namespace CoreWeb.Mobility.Views
{
    public class CompliancePakageMappingDependenciesPresenter : Presenter<ICompliancePakageMappingDependenciesView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IMobilityController _controller;
        // public CompliancePakageMappingDependenciesPresenter([CreateNew] IMobilityController controller)
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
        
        public void GetPkgMappingDependencyList()
        {

            View.ApplicantPkgMappingDependencyList = MobilityManager.GetPkgMappingDependencyList(View.FromTenantId, View.GridCustomPaging, View.PackageMappingMasterId);

                if (View.ApplicantPkgMappingDependencyList.IsNotNull() && View.ApplicantPkgMappingDependencyList.Count > 0)
                {
                    if (View.ApplicantPkgMappingDependencyList[0].TotalCount > 0)
                    {
                        View.VirtualRecordCount = View.ApplicantPkgMappingDependencyList[0].TotalCount;
                    }
                    View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                }
                else
                {
                    View.VirtualRecordCount = 0;
                    View.CurrentPageIndex = 1;
                }
       
        }
    }
}




