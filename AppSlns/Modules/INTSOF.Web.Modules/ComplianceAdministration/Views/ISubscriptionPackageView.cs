using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Linq;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ISubscriptionPackageView
    {
        Int32 TenantId { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        List<Int32> SelectedSubscriptionOptions { get; set; }
        Int32 Priority { get; set; }
        Int32 SelectedPriceModel { get; set; }
        Int32 SavedPriceModelId { get; set; }
        List<Entity.ClientEntity.SubscriptionOption> ListSubscriptionOption { get; set; }
        List<Entity.ClientEntity.lkpPriceModel> ListPriceModel { get; set; }
        Int32 PackageId { get; set; }
        Int32 DeptProgramPackageID { get; set; }
        String ErrorMessage { get; set; }
        String PermissionCode { get; set; }
        Boolean IsAutoRenewInvoiceOrder { get; set; }

        /// <summary>
        /// List of the selected PaymentOptions at the Package Level
        /// </summary>
        List<Int32> lstSelectedOptions { get; set; }

        //UAT-2073
        Int32 PaymentApprovalID
        {
            get;
            set;
        }
    }
}




