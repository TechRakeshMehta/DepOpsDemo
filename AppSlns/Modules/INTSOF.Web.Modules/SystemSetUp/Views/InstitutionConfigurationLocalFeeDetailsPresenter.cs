using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.SystemSetUp;

namespace CoreWeb.SystemSetUp.Views
{
    public class InstitutionConfigurationLocalFeeDetailsPresenter : Presenter<IInstitutionConfigurationLocalFeeDetailsView>
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

        public void GetServiceItemFeeRecordList()
        {
            View.ListServiceItemFeeRecord = BackgroundPricingManager.GetLocalServiceItemFeeRecordsBasedOnGlobal(View.SelectedTenantId, View.PackageServiceItemFeeID);
        }
    }
}


