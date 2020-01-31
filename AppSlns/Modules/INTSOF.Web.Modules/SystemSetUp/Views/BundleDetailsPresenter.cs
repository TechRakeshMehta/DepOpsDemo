using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.SystemSetUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace CoreWeb.SystemSetUp.Views
{
    public class BundleDetailsPresenter : Presenter<IBundleDetailsView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetInstitutionConfigurationBundlePackageDetails()
        {
            View.InstitutionConfigurationPackageDetailsList = ComplianceSetupManager.GetInstitutionConfigurationBundlePackageDetailsList(View.BundlePackageID, View.SelectedTenantId, View.DeptProgramMappingID);
            if (!View.InstitutionConfigurationPackageDetailsList.IsNullOrEmpty())
            {
                View.InstitutionConfigurationPackageDetailsList = View.InstitutionConfigurationPackageDetailsList;
            }
            else
            {
                View.InstitutionConfigurationPackageDetailsList = new List<InstitutionConfigurationPackageDetails>();
            }
           
                View.NodeLabel = !string.IsNullOrEmpty(View.NodeLabel)?View.NodeLabel: View.MasterNodeLabel + " > " + View.BundleName;
        }
    }
}
