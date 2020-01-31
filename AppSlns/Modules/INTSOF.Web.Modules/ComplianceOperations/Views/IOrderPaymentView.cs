#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IOrderPaymentView
    {
        #region Properties

        /// <summary>
        /// List of the Payment Options. Will contain the Package level Options.
        /// If not available, then Node level options
        /// </summary>
        List<PkgList> lstPaymentOptions { get; set; }

        Int32 DPPSId {get;set;}
    
        Int32 TenantId { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        Boolean ShowRushOrderForInvioce { get; set; }

        Boolean ShowRushOrder { get; set; }
        Int32 PaymentMode_InvoiceId { get; set; }
        String PaymentModeCode { get; set; }

        Int32 PaymentMode_InvoiceWdoutApprvlId { get; set; }

        /// <summary>
        /// Id for the Credit Card Payment Mode
        /// </summary>
        Int32 PaymentMode_CreditCardId { get; set; }

        /// <summary>
        /// Get and set next page path.
        /// </summary>
        String NextPagePath { get; set; }


        /// <summary>
        /// Used to identify the current Order Request Type i.e. New order, Change subscription etc.
        /// </summary>
        String OrderType
        {
            get;
            set;
        }

        //UAT-3268
        /// <summary>
        /// Represents the list of background packages which are to get qualify for rotation.
        /// </summary>
        List<BackgroundPackagesContract> lstRotationQualifyingBkgPkgs { get; set; }

        #region UAT-3601
        String PackageNameLabel { get; set; }
        #endregion

        String LanguageCode { get; }
        #endregion

        //UAT-3958
        //Int32 SelectedPaymentOptionId { get; set; }
        Boolean IsAnyOptionsApprovalReq { get; set; }

        Boolean IsAllPkgsPaymentOptionSame { get; set; }
    }
}
