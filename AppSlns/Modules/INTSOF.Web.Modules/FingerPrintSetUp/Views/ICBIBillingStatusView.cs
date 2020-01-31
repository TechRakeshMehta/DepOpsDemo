using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.FingerPrintSetup;
using Entity;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface ICBIBillingStatusView
    {
        #region for Page 
        Int32 TenantId { get; set; }
        Int32 SelectedTenantID { get; set; }
        List<Tenant> lstTenant { get; set; }
         string CBIUniqueID { get; set; }
         string BillingCode { get; set; }
        //Boolean IsEnabled { get; set; }
         string AccountAddress { get; set; }
         string AccountCity { get; set; }
         string AccountName { get; set; }
         string AccountState { get; set; }
         string AccountZIP { get; set; }
         Int32 CurrentLoggedInUserID { get; }
         List<CBIBillingStatusContract> listCBIBillingStatusContract { get; set; }
         CBIBillingStatusContract CbiBillingStatusContract { get; set; } 
        Boolean IsSearch { get; set; }
        #endregion


        #region Custom Args
        Int32 VirtualRecordCount { get; set; }
        Int32 PageSize { get; set; }
        Int32 CurrentPageIndex { get; set; }
        CustomPagingArgsContract GridCustomPaging { get; set; }
        #endregion

    }
}
