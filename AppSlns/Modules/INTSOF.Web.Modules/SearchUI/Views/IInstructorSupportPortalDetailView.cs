using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.UI.Contract.ProfileSharing;
using System;
using System.Collections.Generic;
using System.Text;
using Entity;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace CoreWeb.SearchUI.Views
{
   public interface IInstructorSupportPortalDetailView
    {
        IInstructorSupportPortalDetailView CurrentViewContext
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
        String MiddleName
        {
            get;
            set;
        }

        
        Entity.OrganizationUser OrganizationUser
        {
            get;
            set;
        }       

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
        List<ClinicalRotationDetailContract> ClinicalRotationData
        {
            get;
            set;
        }
        OrganizationUserContract OrganisationUser { get; set; }
        Int32 LoggedInUserTenantId { get; }
        #region UAT-4313
        List<ClientContactNotesContract> ClientContactNotes
        {
            get;
            set;
           
        }
        Int32 ClientContactId { get; set; }
        #endregion
    }
}
