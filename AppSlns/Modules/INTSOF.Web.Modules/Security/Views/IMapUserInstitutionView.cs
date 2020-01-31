#region Namespace

#region System Defined

using System;
using System.Linq;
using System.Collections.Generic;

#endregion

#region Application Specific

using Entity;
using INTSOF.UI.Contract.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.Security.Views
{
   public interface IMapUserInstitutionView
    {
        Int32 TenantId
        {
            get;
            set;
        }

        String TenantName
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;

        }

        Int32 OrganizationUserId
        {
            get;
            set;
        }

        String UserId
        {
            get;
            set;
        }

        //MapServiceAttributeToGroupContract ViewContract
        //{
        //    get;

        //}

        String ErrorMessage
        {
            get;
            set;
        }

        List<Tenant> MappedTenantList
        {
            get;
            set;
        }

        List<Tenant> UnmappedTenantList
        {
            get;
            set;
        }

        Int32 DefaultTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the selected user.
        /// </summary>
        OrganizationUser SelectedUser
        {
            get;
            set;
        }

    }
}
