using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface ICIMAccountSelectionView
    {
        Int32 CurrentLoggedInUserId { get; }
        Int32 TenantId { get; set; }
        String InvoiceNumber { get; set; }
        List<Int32> OrderIds { get; set; }
        OnlinePaymentTransaction OnlinePaymentTransactionDetails { get; set; }
        //UAT 4537
        List<OnlinePaymentTransaction> LstOnlinePaymentTransactionDetails { get; set; }
        ICIMAccountSelectionView CurrentViewContext { get; }

        /// <summary>
        /// Get and set next page path.
        /// </summary>
        String NextPagePath
        {
            get;
            set;
        }

        Entity.OrganizationUser OrganizationUserData { get; set; }

        #region BILLING INFORMATION PROPERTIES
        String OrganizationUserID { get; set; }
        String FirstName { get; set; }
        String LastName { get; set; }
        String Company { get; set; }
        String Address { get; set; }
        String City { get; set; }
        String State { get; set; }
        String Zip { get; set; }
        String Country { get; set; }
        String Email { get; set; }
        String Phone { get; set; }
        String Fax { get; set; }
        #endregion

        /// <summary>
        /// Represents the OPDId for which the status is to be changed
        /// </summary>
        Int32 OPDId
        {
            get;
            set;
        }

        Int32 OrgUsrID { get; }

        Boolean IsItemPayment
        {
            get;
            set;
        }
        long DefaultpaymentProfileId { get; set; }

        Boolean IsInstructorPreceptorPackage
        {
            get;
            set;
        }
    }
}
