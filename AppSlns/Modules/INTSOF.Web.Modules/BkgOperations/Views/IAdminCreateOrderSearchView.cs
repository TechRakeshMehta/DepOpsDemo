using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IAdminCreateOrderSearchView
    {
        //IAdminCreateOrderSearchView CurrentViewContext
        //{
        //    get;
        //}

        Int32 CurrentLoggedInUserId { get; }

        Boolean IsReset { get; set; }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        List<Tenant> lstTenant
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }

        String TargetHierarchyNodeIds
        {
            get;
            set;
        }

        List<AdminCreateOrderContract> lstAdminOrders
        {
            get;
            set;
        }

        AdminOrderSearchContract searchContract
        {
            get;
            set;
        }

        Int32 OrderID
        {
            get;
            set;
        }

        String OrderNumber
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

        String SSN
        {
            get;
            set;
        }
        DateTime DOB
        {
            get;
            set;
        }

        String ReadyToTransmit
        {
            get;
            set;
        }

        List<Int32> SelectedOrderIds
        {
            get;
            set;
        }

        Dictionary<Int32, Boolean> DicOfSelectedOrders { get; set; }
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
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }

        Int32 VirtualRecordCount
        {
            get;
            set;
        }

        #endregion
    }
}
