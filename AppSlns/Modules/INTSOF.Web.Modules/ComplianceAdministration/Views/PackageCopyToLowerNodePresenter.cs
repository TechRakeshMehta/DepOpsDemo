using Business.RepoManagers;
using INTSOF.SharedObjects;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class PackageCopyToLowerNodePresenter : Presenter<IPackageCopyToLowerNode>
    {
        public void GetNodeList()
        {
            View.lstDepartmentProgramMapping = ComplianceSetupManager.GetChildNodesByNodeID(View.NodeID, View.TenantId);
        }

        public void CopyPackageStructure()
        {

            if (ComplianceSetupManager.CheckIfPackageNameAlreadyExist(View.CompliancePackageName, View.TenantId))
            {
                View.ErrorMessage = "Package Name already exists.";
                return;
            }
            ComplianceSetupManager.CopyPackageStructure(View.CompliancePackageID, View.CompliancePackageName, View.CurrentLoggedInUserId, View.TenantId, true, View.NodeID, View.SelectedNodeID);
        }
    }
}
