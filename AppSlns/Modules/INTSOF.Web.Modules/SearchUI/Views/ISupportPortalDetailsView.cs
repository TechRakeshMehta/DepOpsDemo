using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.UI.Contract.ProfileSharing;
using System;
using System.Collections.Generic;
using System.Text;
using Entity;
namespace CoreWeb.SearchUI.Views
{
    public interface ISupportPortalDetailsView
    {
        ISupportPortalDetailsView CurrentViewContext
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

        List<Tenant> lstTenant
        {
            get;
            set;
        }

        String OrgUserId
        {
            get;
            set;
        }

        List<INTSOF.UI.Contract.SearchUI.SupportPortalOrderDetailContract> lstSuportPortalOrderData
        {
            get;
            set;
        }

       
    }
}
