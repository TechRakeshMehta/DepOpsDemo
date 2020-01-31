using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
namespace CoreWeb.ComplianceOperations.Views
{
    public class OrderLineItemsPresenter : Presenter<IOrderLineItemsView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public PackageDetailsPresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            GetTenantId();
            View.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(View.TenantId);
        }



        public void GetTenantId()
        {
            View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }
        public override void OnViewInitialized()
        {

            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Get Bkg Order Service Details xml
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public string GetBkgOrderServiceDetails(int orderID)
        {
            return BackgroundProcessOrderManager.GetBkgOrderServiceDetails(Convert.ToInt32(View.TenantId), orderID);
        }

        public void GetOrderLineItems(ApplicantOrderCart _applicantOrderCart)
        {
            if (!_applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                BkgDataStore bkgDataStore = new BkgDataStore();
                String personalDataXML = bkgDataStore.ConvertApplicantDataIntoXML(_applicantOrderCart, View.TenantId, false, false, true);
                Boolean _isXMLGenerated;
                String _pricingInputXML = StoredProcedureManagers.GetPricingDataInputXML(personalDataXML, View.TenantId, _applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData,
                    _applicantOrderCart.lstApplicantOrder[0].lstPackages, _applicantOrderCart.MailingAddress, out _isXMLGenerated);
                View.lstOrderLineItems = StoredProcedureManagers.GetOrderLineItems(View.TenantId, _pricingInputXML);
            }

        }

        public void GetSavedOrderLineItems(Int32 OrderID)
        {
            View.lstOrderLineItems = StoredProcedureManagers.GetSavedOrderLineItems(View.TenantId, OrderID);
        }

        // TODO: Handle other view events and set state in the view
    }
}




