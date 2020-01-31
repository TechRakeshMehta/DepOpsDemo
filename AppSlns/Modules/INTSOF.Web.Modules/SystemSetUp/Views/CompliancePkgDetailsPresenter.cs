using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.SystemSetUp.Views
{
    public class CompliancePkgDetailsPresenter : Presenter<ICompliancePkgDetailsView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetCompliancePkgDetails()
        {
            View.CompliancePkgDetails = ComplianceSetupManager.GetCompliancePkgDetails(View.DeptProgramMappingID, View.PackageID, View.SelectedTenantId, View.PackageHierarchyID);
            if (!View.CompliancePkgDetails.IsNullOrEmpty())
            {
                //UAT:2411
                if (!string.IsNullOrEmpty(View.ParentScreenName))
                    View.NodeLabel = "Node: " + View.CompliancePkgDetails.NodeLabel + " > " + View.BundleName + " > " + View.CompliancePkgDetails.PackageName;
                else
                    View.NodeLabel = "Node: " + View.CompliancePkgDetails.NodeLabel + " > " + View.CompliancePkgDetails.PackageName;
            }
        }
    }
}
