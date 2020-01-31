using INTSOF.UI.Contract.BkgSetup;
using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IModifyShippingInfoView
    {
        Int32 LocationId { get; set; }
        Int32 SelectedTenantId
        {
            get;
            set;
        }
        Int32 CurrentLoggedInUserId
        {
            get;
        }
        Int32 OrderId
        {
            get;
            set;
        }

        String ParentControl
        {
            get;
            set;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Entity.OrganizationUser OrganizationUser { get; set; }

        Boolean IsLocationServiceTenant { get; set; }

        List<ServiceFeeItemRecordContract> lstMailingOptionsWithPrice
        {
            set;
            get;
        }
    }
}

