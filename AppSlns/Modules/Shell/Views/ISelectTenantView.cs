using INTSOF.Utils;
using System;

namespace CoreWeb.Shell.Views
{
    /// <summary>
    /// Interface for the 'Select Tenant' page for Applicant
    /// </summary>
    public interface ISelectTenantView
    {
        /// <summary>
        /// Property to represent the Institute Selected by the applicant
        /// </summary>
        Int32 SelectedTenantId { get; }

        /// <summary>
        /// Property to represent the Id of the current logged in user
        /// </summary>
        Int32 OrganizationUserId { get; set; }

        /// <summary>
        /// Gets the current user Session id.
        /// </summary>
        /// <remarks></remarks>
        String CurrentSessionId
        {
            get;
        }

        IPersistViewState ViewStateProvider { get; }
    }
}
