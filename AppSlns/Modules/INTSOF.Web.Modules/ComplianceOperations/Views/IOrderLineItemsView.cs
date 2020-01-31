using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IOrderLineItemsView
    {
       
        Int32 TenantId { get; set; }
        Dictionary<string, DeptProgramPackageSubscription> SelectedPackageDetails { get; set; }
        IOrderLineItemsView CurrentViewContext { get; }
        Int32 CurrentLoggedInUserId
        {
            get;
        }
      

        List<OrderLineItem> lstOrderLineItems
        {
            get;
            set;
        }
        Boolean IsLocationServiceTenant { get; set; }
       

        String LanguageCode { get; }

        #region Admin Entry Portal
        Boolean IsAdminEntryTenant { get; set; }
        #endregion
    }
}




