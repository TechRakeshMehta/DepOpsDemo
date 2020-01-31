using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.UI.Contract.ProfileSharing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.Search.Views
{
    public interface IApplicantPortfolioDetailsView
    {
        IApplicantPortfolioDetailsView CurrentViewContext
        {
            get;
        }

        Int32 CurrentLoggedInUserId { get; }

        Int32 OrganizationUserId { get; }

        Int32 TenantId { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        /// <remarks></remarks>
        String Password
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default password.
        /// </summary>
        /// <value>The default password.</value>
        /// <remarks></remarks>
        String DefaultPassword
        {
            get;
            set;
        }

        String EmailAddress
        {
            get;
            set;
        }

        String FirstName
        {
            get;
            set;
        }

        String LastName
        {
            get;
            set;
        }

        Boolean IsLocked
        {
            get;
            set;
        }

        Boolean IsActive
        {
            get;
            set;
        }

        Entity.OrganizationUser OrganizationUser
        {
            get;
            set;
        }

        /// <summary>
        /// List of Sharing Packages. Includes both Complianvce and Background packages.
        /// </summary>
        List<InvitationDataContract> lstInvitationsSent
        {
            set;
            get;
        }

        /// <summary>
        /// Whether the invitation is sent successfully.
        /// </summary>
        Boolean IsInvitationSent
        {
            get;
            set;
        }
        //UAT 2467
        List<AttestationDocumentContract> LstInvitationDocumentContract { get; set; }

        Int32 SelectedTenantId { get; set; }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }
        String UserId { get; set; }

        #region UAT-2930
        //UAT-3068 (data type has been changes acc to requirement)
        //Boolean IsTwoFactorAuthenticationVerified { get; set; }
        //Boolean IsTwoFactorAuthentication
        //{
        //    get;
        //    set;
        //}
        String IsUserTwoFactorAuthenticatedPrevious
        {
            get;
            set;
        } 
        #endregion

        #region UAT-3068
        String SelectedAuthenticationType
        {
            get;
            set;
        }
        #endregion
       
    }
}




