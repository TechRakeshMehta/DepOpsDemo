using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Entity;
using INTSOF.Utils;
using System.Collections;

namespace CoreWeb.Messaging.Views
{
    public interface IAddressLookupView
    {
        IQueryable<OrganizationUser> OrganizationUsers
        {
            set;
        }

        IQueryable<vw_ListOfUsers> MessagingGroups
        {
            set;
        }

        Int32 SelectedProgramId
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }
        IAddressLookupView CurrentViewContext
        {
            get;
        }

        /// <summary>
        /// CurrentPageIndex
        /// </summary>
        /// <value> Gets or sets the value for CurrentPageIndex.</value>
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// PageSize
        /// </summary>
        /// <value> Gets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
        }

        /// <summary>
        /// VirtualPageCount
        /// </summary>
        /// <value> Sets the value for VirtualPageCount.</value>
        Int32 VirtualPageCount
        {
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridUsersCustomPaging
        {
            get;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridMessagingGroupCustomPaging
        {
            get;
        }

        List<String> FilterColumns
        {
            get;
            set;
        }

        List<String> FilterOperators
        {
            get;
            set;
        }

        List<String> FilterTypes
        {
            get;
            set;
        }

        ArrayList FilterValues
        {
            get;
            set;
        }

        List<Int32> SelectedOrganizationUserIds
        {
            get;
            set;
        }
    }
}




