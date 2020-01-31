using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.SharedDataEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;

namespace CoreWeb.SearchUI.Views
{
    public interface IClientLoginSearchView
    {
        Int32 TenantID { get; set; }
        List<Int32> SelectedTenantIDs { get; set; }
        List<Entity.Tenant> lstTenant { get; set; }
        IClientLoginSearchView CurrentViewContext { get; }
        Int32 CurrentLoggedInUserID { get; }
        Int32 OrganizationUserID { get; set; }
        String ClientFirstName { get; set; }
        String ClientLastName { get; set; }
        String ClientUserName { get; set; }
        String EmailAddress { get; set; }
        List<ClientLoginSearchContract> ClientSearchData { get; set; }
       // String SearchType { get; }
        #region Custom Paging Parameters

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        Int32 VirtualPageCount
        {
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
        }


        #endregion
    }
}
