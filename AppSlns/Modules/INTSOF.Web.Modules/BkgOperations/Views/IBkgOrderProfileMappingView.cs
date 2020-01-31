using INTSOF.UI.Contract.BkgOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IBkgOrderProfileMappingView
    {
        /// <summary>
        /// stores Tenant Id of the processing order.
        /// </summary>
        Int32 TenantID { get; set; }

        /// <summary>
        /// Stores whether the link the profile with exisiting line item or not.
        /// </summary>
        Boolean IsLinkProfile { get; set; }

        /// <summary>
        /// Store Master OrderID for the process.
        /// </summary>
        Int32 OrderID { get; set; }

        /// <summary>
        /// To store the Package Service Line ID.
        /// </summary>
        Int32 PackageServiceLineItemID { get; set; }

        /// <summary>
        /// To get the list of svc line items of a order.
        /// </summary>
        List<VendorProfileSvcLineItemContract> lstLineItemsData { get; set; }

        /// <summary>
        /// To get the list of svc line items of a order.
        /// </summary>
        VendorProfileSvcLineItemContract LineItemProfileData { get; set; }

        /// <summary>
        /// TO Store all external vendors data.
        /// </summary>
        List<Entity.ExternalVendor> lstExtVendors { get; set; }

        /// <summary>
        /// To Store the selected vendor Id.
        /// </summary>
        //Int32 SelectedVendorID { get; set; }
        Int32 SelectedVendorID { get; }

        /// <summary>
        /// To Store all the background packages in an order.
        /// </summary>
        Dictionary<Int32, String> dicBkgPackages { get; set; }

        /// <summary>
        /// To Store the selected package Id.
        /// </summary>
        Int32 SelectedPackageID { get; set; }
        /// <summary>
        /// To store all the service groups in the package selected.
        /// </summary>
        Dictionary<Int32, String> dicBkgSvcGroups { get; set; }

        /// <summary>
        /// To Store the selected Svc Group Id.
        /// </summary>
        Int32 SelectedSvcGroupID { get; set; }

        /// <summary>
        /// To store all the services in the selected service group.
        /// </summary>
        Dictionary<Int32, String> dicBkgServices { get; set; }

        /// <summary>
        /// To Store the selected service Id.
        /// </summary>
        Int32 SelectedServiceID { get; set; }

        /// <summary>
        /// To get Vendor Profile ID
        /// </summary>
        String VendorProfileID { get; set; }

        /// <summary>
        /// TO get vendor line item order id.
        /// </summary>
        String VendorLineItemOrderID { get; set; }

        /// <summary>
        /// To store list of Line item status.
        /// </summary>
        List<Entity.ClientEntity.lkpOrderLineItemResultStatu> lstLineItemStatus { get; set; }

        /// <summary>
        /// To Store the selected line item status Id.
        /// </summary>
        Int32 SelectedLineItemStatusID { get; set; }

        /// <summary>
        /// Get the values of logged in user id from session.
        /// </summary>
        Int32 CurrentLoggedInUserId { get; }

        /// <summary>
        /// To store the list of line items mapped manually for an order. 
        /// </summary>
        List<VendorProfileSvcLineItemContract> lstCreatedVendorProfileSvcLineItem { get; set; }

        /// <summary>
        /// Stores whether Profile ID Exists on clearstar.
        /// </summary>
        Boolean IsProfileIDExists { get; set; }

        /// <summary>
        /// Stores whether Vendor OrderID Exists on clearstar.
        /// </summary>
        Boolean IsVendorOrderIDExists { get; set; }
    }
}
