using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public class DisclosureAndReleaseFormPresenter : Presenter<IDisclosureAndReleaseFormView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {

        }

        public void GetDisclosureReleaseDoc()
        {
            if (View.SelectedTenantID > 0 && View.MasterOrderID > 0)
                View.SetTenantID = View.SelectedTenantID.ToString();
                View.lstDnRDocuments = BackgroundProcessOrderManager.GetDisclosureReleaseDoc(View.SelectedTenantID, View.MasterOrderID);        
        }

        #region UAT-3745
        
        public void GetApplicantAdditionalDoc()
        {
            View.lstApplicantDocs = BackgroundProcessOrderManager.GetApplicantDocsMappedWithSvc(View.SelectedTenantID, View.MasterOrderID);
        }

        #endregion
    }
}
