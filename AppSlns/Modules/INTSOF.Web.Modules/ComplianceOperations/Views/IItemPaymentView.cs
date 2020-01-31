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
    public interface IItemPaymentView
    {
        #region Properties
        /// <summary>
        /// List of the Payment Options. Will contain the Package level Options.
        /// If not available, then Node level options
        /// </summary>
        List<PkgPaymentOptions> lstPaymentOptions { get; set; }
        String PaymentModeCode { get; set; }
        Int32 PaymentModeId { get; set; }

        Int32 TenantId { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 OrganizationUserID { get; }


        ItemPaymentContract itemPaymentContract { get; set; }

        Boolean IsInstructorPreceptorPackage { get; set; }
        #endregion

    }
}
