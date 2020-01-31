using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;

namespace CoreWeb.SystemSetUp.Views
{
    public class InstitutionConfigurationGlobalFeeDetailsPresenter : Presenter<IInstitutionConfigurationGlobalFeeDetailsView>
    {
        public override void OnViewInitialized()
        {
        }

        /// <summary>
        /// Get the List of ServiceItemFeeType list.
        /// </summary>
        public void GetServiceItemFeeRecordListContract()
        {
            View.ListServiceItemFeeRecordContract = BackgroundPricingManager.GetServiceItemFeeRecordContract(SecurityManager.DefaultTenantID, View.SelectedFeeItemId);
        }
    }
}
