using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ComplianceSearchNotesPresenter : Presenter<IComplianceSearchNotesView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
        }

        public void GetComplianceSearchNote()
        {
            ComplianceSearchNotesContract objComplianceSearchNotesContract = new ComplianceSearchNotesContract();
            objComplianceSearchNotesContract.CurrentLoggedInUserOrgId = View.CurrentLoggedInUserOrgId;
            objComplianceSearchNotesContract.PackageSubscriptionID = View.PackageSubscriptionID;
            objComplianceSearchNotesContract.TenantId = View.SelectedTenantId;
            objComplianceSearchNotesContract = ComplianceDataManager.GetComplianceSearchNote(objComplianceSearchNotesContract);
            View.Notes = objComplianceSearchNotesContract.Notes;
        }

        public Boolean SaveComplianceSearchNote()
        {
            ComplianceSearchNotesContract objComplianceSearchNotesContract = new ComplianceSearchNotesContract();
            objComplianceSearchNotesContract.Notes = View.Notes;
            objComplianceSearchNotesContract.CurrentLoggedInUserOrgId = View.CurrentLoggedInUserOrgId;
            objComplianceSearchNotesContract.PackageSubscriptionID = View.PackageSubscriptionID;
            objComplianceSearchNotesContract.TenantId = View.SelectedTenantId;

            return ComplianceDataManager.SaveComplianceSearchNote(objComplianceSearchNotesContract);
        }
    }
}
