using System;
using System.Collections.Generic;
using Entity.ClientEntity;

namespace CoreWeb.BkgSetup.Views
{
    public interface IMasterReviewCriteriaView
    {
        /// <summary>
        /// Logged in user ID
        /// </summary>
        Int32 currentLoggedInUserId
        {
            get;
        }

        Int32 TenantID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        List<Tenant> ListTenants
        {
            set;
            get;
        }

        /// <summary>
        /// Selected Tenant ID
        /// </summary>
        Int32 SelectedTenantID
        {
            get;
            set;
        }

        List<BkgReviewCriteria> ListBkgReviewCriteria
        {
            get;
            set;
        }

        String ErrorMessage { get; set; }
    }
}
