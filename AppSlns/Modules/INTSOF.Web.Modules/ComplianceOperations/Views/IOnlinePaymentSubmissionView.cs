using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IOnlinePaymentSubmissionView
    {
        Int32 CurrentLoggedInUserId { get; }
        Int32 TenantId { get; set; }
        String InvoiceNumber { get; set; }
        OnlinePaymentTransaction OnlinePaymentTransactionDetails { get; set; } 
        IOnlinePaymentSubmissionView CurrentViewContext { get; }

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
        String OrganizationUserID{get;set;}
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

        
    }
}




