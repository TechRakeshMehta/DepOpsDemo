using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Entity;
using Entity.ClientEntity;
using Entity.ExternalVendorContracts;


namespace ExternalVendors.Interface
{
    public interface IVendorServiceAdapter
    {
        /// <summary>
        /// This Adapter method is used to dispatch the AMS Orders to Vendors
        /// </summary>
        /// <param name="evOrderContract"></param>
        /// <param name="tenantID"></param>
        /// <returns>EvCreateOrderContract</returns>
        EvCreateOrderContract DispatchOrderItemsToVendor(EvCreateOrderContract evOrderContract, Int32 tenantID, Boolean isTestModeON);

        /// <summary>
        /// This method is used to Update the AMS Order with Vendor data.
        /// </summary>
        /// <param name="evUpdateOrderContract"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        //EvUpdateOrderContract UpdateVendorBkgOrder(EvUpdateOrderContract evUpdateOrderContract, IEnumerable<usp_GetOrdersToBeUpdatedVendorData_Result> processedOrderItemList, 
        //                                           Int32 tenantID);
        EvUpdateOrderContract UpdateVendorBkgOrder(EvUpdateOrderContract evUpdateOrderContract, Int32 tenantID);

        /// <summary>
        /// This method is used to revert back all the vendor changes i.e. Delete Vendor Profile and Cancel Vendor Profile.
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="boid"></param>
        /// <param name="customerID"></param>
        /// <param name="vendorProfileNumber"></param>
        /// <param name="tenantID"></param>
        /// <param name="bkgOrderID"></param>
        void RevertExternalVendorChanges(String loginName, String password, Int32 boid, String customerID, String vendorProfileNumber, Int32 tenantID, Int32 bkgOrderID, Int32 bkgOrderPackageSvcGroupID);

        List<State> States
        {
            get;
            set;
        }
    }
}
