using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class CBIBillingStatusPresenter : Presenter<ICBIBillingStatusView>
    {
        public override void OnViewInitialized()
        {
            GetTenants();
        }
        private void GetTenants()
        {
            View.lstTenant = new List<Tenant>();
            View.lstTenant = SecurityManager.GetListOfTenantWithLocationService();
        }
        public void GetCBIBillingStatus()
        {
            try
            {
                View.listCBIBillingStatusContract = new List<CBIBillingStatusContract>();
                if (View.SelectedTenantID > 0)
                {
                    View.listCBIBillingStatusContract = CBIBillingStatusManagers.GetCBIBillingStatus(View.GridCustomPaging, View.CbiBillingStatusContract);
                    if (View.listCBIBillingStatusContract.Count > 0)
                    {
                        View.VirtualRecordCount = View.listCBIBillingStatusContract.Select(x => x.TotalCount).First();
                    }
                    else { View.VirtualRecordCount = 0; }
                }
                else
                    View.VirtualRecordCount = 0;
            }
            catch (Exception)
            {
                
                throw;
            }
           
           
        }
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        public bool SaveCBIBillingStatus(CBIBillingStatusContract cBIBillingStatusContract)
        {
            try
            {
                return CBIBillingStatusManagers.SaveCBIBillingStatus(cBIBillingStatusContract, View.CurrentLoggedInUserID);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

    }
}
