using INTSOF.SharedObjects;
using Business.RepoManagers;

namespace CoreWeb.Shell.Views
{
    public class CategoriesItemsNodesPresenter : Presenter<ICategoriesItemsNodes>
    {
        public void GetListofNodes()
        {
            View.ListofNodes = ComplianceSetupManager.GetListofNodes(View.ComplianceCategoryId, View.ComplianceItemId, View.SelectedTenantId);
        }
    }
}
