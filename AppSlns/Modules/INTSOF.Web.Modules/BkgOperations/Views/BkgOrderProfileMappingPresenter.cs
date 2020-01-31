using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public class BkgOrderProfileMappingPresenter : Presenter<IBkgOrderProfileMappingView>
    {
        /// <summary>
        /// This method is to get the service line items and other details of an order.
        /// </summary>
        public void GetLineItemsDataforOrderID()
        {
            View.lstLineItemsData = BackgroundProcessOrderManager.GetLineItemsDataforOrderID(View.TenantID, View.OrderID);
        }

        /// <summary>
        /// To get the line itme data and other details of order for the selected line item.
        /// </summary>
        public void GetLineItemData()
        {
            if (!View.lstLineItemsData.IsNullOrEmpty())
            {
                View.LineItemProfileData = View.lstLineItemsData.Where(con => con.PackageSvcLineItemID == View.PackageServiceLineItemID).FirstOrDefault();
            }
        }

        /// <summary>
        /// To Get all External Vendors in the system.
        /// </summary>
        public void GetExternalVendor()
        {
            View.lstExtVendors = new List<Entity.ExternalVendor>();
            View.lstExtVendors = BackgroundSetupManager.GetVendors();
        }

        /// <summary>
        /// Get external Vendor Account
        /// </summary>
        public Entity.ExternalVendorAccount GetExternalVendorAccount()
        {
            Entity.ExternalVendorAccount VendorAccount = new Entity.ExternalVendorAccount();
            VendorAccount = BackgroundSetupManager.GetExternalVendorAccount(View.SelectedVendorID, View.TenantID, View.OrderID);
            return VendorAccount;
        }

        /// <summary>
        /// To get all the packages in the order.
        /// </summary>
        public void GetBkgOrderPackage()
        {
            View.dicBkgPackages = new Dictionary<Int32, String>();

            if (!View.lstLineItemsData.IsNullOrEmpty())
            {
                //List<VendorProfileSvcLineItemContract> lstPackages = View.lstLineItemsData.Where(con => con.ExtVendorID == View.SelectedVendorID && con.OrderID == View.OrderID).DistinctBy(d => d.BackgroundPackageID).ToList();
                List<VendorProfileSvcLineItemContract> lstPackages = View.lstLineItemsData.Where(con => con.OrderID == View.OrderID).DistinctBy(d => d.BackgroundPackageID).ToList();
                if (!lstPackages.IsNullOrEmpty())
                {
                    IEnumerable<KeyValuePair<Int32, String>> bkgPackages = lstPackages.Select(x => new KeyValuePair<Int32, String>(x.BackgroundPackageID, x.BackgroundPackageName));
                    View.dicBkgPackages.AddRange(bkgPackages);
                }
            }
        }

        /// <summary>
        /// To get all the service groups in an order and for selected package.
        /// </summary>
        public void GetPakageServiceGroup()
        {
            View.dicBkgSvcGroups = new Dictionary<Int32, String>();
            if (!View.lstLineItemsData.IsNullOrEmpty())
            {
                //List<VendorProfileSvcLineItemContract> lstSvcGroups = View.lstLineItemsData.Where(con => con.ExtVendorID == View.SelectedVendorID && con.OrderID == View.OrderID && con.BackgroundPackageID == View.SelectedPackageID).DistinctBy(d => d.ServiceGroupID).ToList();
                List<VendorProfileSvcLineItemContract> lstSvcGroups = View.lstLineItemsData.Where(con => con.OrderID == View.OrderID && con.BackgroundPackageID == View.SelectedPackageID).DistinctBy(d => d.ServiceGroupID).ToList();
                if (!lstSvcGroups.IsNullOrEmpty())
                {
                    IEnumerable<KeyValuePair<Int32, String>> bkgSvcGroups = lstSvcGroups.Select(x => new KeyValuePair<Int32, String>(x.ServiceGroupID, x.ServiceGroupName));
                    View.dicBkgSvcGroups.AddRange(bkgSvcGroups);
                }
            }
        }

        /// <summary>
        /// To get all the services in an order for selected package and selected service group.
        /// </summary>
        public void GetServices()
        {
            View.dicBkgServices = new Dictionary<Int32, String>();
            if (!View.lstLineItemsData.IsNullOrEmpty())
            {
                //List<VendorProfileSvcLineItemContract> lstServices = View.lstLineItemsData.Where(con => con.ExtVendorID == View.SelectedVendorID && con.OrderID == View.OrderID && con.BackgroundPackageID == View.SelectedPackageID && con.ServiceGroupID == View.SelectedSvcGroupID).DistinctBy(d => d.ServiceID).ToList();
                List<VendorProfileSvcLineItemContract> lstServices = View.lstLineItemsData.Where(con => con.OrderID == View.OrderID && con.BackgroundPackageID == View.SelectedPackageID && con.ServiceGroupID == View.SelectedSvcGroupID).DistinctBy(d => d.ServiceID).ToList();
                if (!lstServices.IsNullOrEmpty())
                {
                    IEnumerable<KeyValuePair<Int32, String>> bkgServices = lstServices.Select(x => new KeyValuePair<Int32, String>(x.ServiceID, x.ServiceName));
                    View.dicBkgServices.AddRange(bkgServices);
                }
            }
        }

        public void GetLineItemStatus()
        {
            View.lstLineItemStatus = new List<Entity.ClientEntity.lkpOrderLineItemResultStatu>();
            View.lstLineItemStatus = BackgroundProcessOrderManager.GetlkpOrderLineItemResultStatus(View.TenantID);
        }

        public Boolean SaveProfileMapping()
        {
            //Create a new contract here using properties.//

            Int32? packageServiceLineItemID = null;
            if (View.IsLinkProfile)
                packageServiceLineItemID = View.PackageServiceLineItemID;


            VendorProfileSvcLineItemContract VendorProfileSvcLineItemData = new VendorProfileSvcLineItemContract()
            {
                OrderID = View.OrderID,
                BackgroundPackageID = View.SelectedPackageID,
                ServiceID = View.SelectedServiceID,
                ServiceGroupID = View.SelectedSvcGroupID,
                ExtVendorID = View.SelectedVendorID,
                SvcLineItemStatusID = View.SelectedLineItemStatusID,
                VendorProfileID = View.VendorProfileID,
                VendorLineItemOrderID = View.VendorLineItemOrderID,
                IsLinkProfile = View.IsLinkProfile,
                PackageSvcLineItemID = Convert.ToInt32(packageServiceLineItemID)
            };

            if (!VendorProfileSvcLineItemData.IsNullOrEmpty())
                return BackgroundProcessOrderManager.SaveUpdateSvcLineItemMapping(View.TenantID, View.CurrentLoggedInUserId, VendorProfileSvcLineItemData);
            return false;
        }

        public void GetSvcLineItemsCreated()
        {
            View.lstCreatedVendorProfileSvcLineItem = new List<VendorProfileSvcLineItemContract>();
            View.lstCreatedVendorProfileSvcLineItem = BackgroundProcessOrderManager.GetSvcLineItemsCreated(View.TenantID, View.OrderID);
        }

        //public String IsProfileAndOrderExistsOnClearStar()
        //{

        //}
    }
}
