#region NameSpace

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Project Specific
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity;
#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public class ManageFeeItemPresenter : Presenter<IManageFeeItemView>
    {
        public override void OnViewInitialized()
        {
        }

        public void GetTenants()
        {
            //View.ListTenants = ComplianceDataManager.getClientTenant();
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }

        /// <summary>
        /// Get the List of Fee Items.
        /// </summary>
        public void GetPackageSvcItemFeeList()
        {
            if (SecurityManager.DefaultTenantID > 0)
            {
                View.ListPackageServiceItemFee = BackgroundPricingManager.GetPackageServiceFeeItemList(SecurityManager.DefaultTenantID);
            }
            else
                View.ListPackageServiceItemFee = new List<PackageServiceItemFee>();
        }

        /// <summary>
        /// Get the List of ServiceItemFeeType list.
        /// </summary>
        public void GetServiceItemFeeTypeList()
        {
            List<lkpServiceItemFeeType> tempServiceItemFeeList = new List<lkpServiceItemFeeType>();
            if (SecurityManager.DefaultTenantID > 0)
                tempServiceItemFeeList = BackgroundPricingManager.GetServiceItemFeeTypeList(SecurityManager.DefaultTenantID);
            tempServiceItemFeeList.Insert(0, new lkpServiceItemFeeType { SIFT_Name = "--Select--", SIFT_ID = 0 });
            View.ListServiceItemFeeType = tempServiceItemFeeList;
        }

        /// <summary>
        /// Save the PackageServiceItemFee Data.
        /// </summary>
        public void SavePackageServiceItemFeeRecord()
        {
            if (!IsFeeItemNameExist(null))
            {
                if (BackgroundPricingManager.IsFeeItemExistForFeeItemType(SecurityManager.DefaultTenantID, View.SelectedServiceItemFeeTypeId, null))
                {
                    View.InfoMessage = "Fee Item already exist for selected fee item type.";
                }
                else
                {
                    PackageServiceItemFee pkgSvcItemFeeObject = new PackageServiceItemFee();
                    pkgSvcItemFeeObject.PSIF_Description = View.ItemFeeDescription;
                    pkgSvcItemFeeObject.PSIF_Name = View.ItemFeeName;
                    //pkgSvcItemFeeObject.PSIF_Label = View.ItemFeeLabel;
                    pkgSvcItemFeeObject.PSIF_ServiceItemFeeType = View.SelectedServiceItemFeeTypeId;
                    pkgSvcItemFeeObject.PSIF_IsGlobal = View.IsGlobal;
                    pkgSvcItemFeeObject.PSIF_CreatedByID = View.currentLoggedInUserId;
                    pkgSvcItemFeeObject.PSIF_CreadtedOn = DateTime.Now;
                    if (BackgroundPricingManager.SavePackageServiceItemFeeRecord(SecurityManager.DefaultTenantID, pkgSvcItemFeeObject))
                    {
                        View.SuccessMessage = "Fee Item saved successfully.";
                    }
                    else
                    {
                        View.ErrorMessage = "Some error has occured.Please contact administrator.";
                    }
                }
            }
            else
            {
                View.InfoMessage = "Fee Item name already exist.";
            }
        }

        /// <summary>
        /// Update the PackageServiceItemFee Data.
        /// </summary>
        public void UpdatePackageServiceItemFeeRecord(Int32 pkgSvcItemFeeId)
        {
            if (!IsFeeItemNameExist(pkgSvcItemFeeId))
            {
                if (BackgroundPricingManager.IsFeeItemExistForFeeItemType(SecurityManager.DefaultTenantID, View.SelectedServiceItemFeeTypeId, pkgSvcItemFeeId))
                {
                    View.InfoMessage = "Fee Item already exist for selected fee item type.";
                }
                else
                {
                    PackageServiceItemFee pkgSvcItemFeeObjectInDB = BackgroundPricingManager.GetPackageServiceItemFeeByID(SecurityManager.DefaultTenantID, pkgSvcItemFeeId);
                    pkgSvcItemFeeObjectInDB.PSIF_Description = View.ItemFeeDescription;
                    pkgSvcItemFeeObjectInDB.PSIF_Name = View.ItemFeeName;
                    //pkgSvcItemFeeObjectInDB.PSIF_Label = View.ItemFeeLabel;
                    pkgSvcItemFeeObjectInDB.PSIF_ServiceItemFeeType = View.SelectedServiceItemFeeTypeId;
                    pkgSvcItemFeeObjectInDB.PSIF_IsGlobal = View.IsGlobal;
                    pkgSvcItemFeeObjectInDB.PSIF_ModifiedID = View.currentLoggedInUserId;
                    pkgSvcItemFeeObjectInDB.PSIF_ModifiedOn = DateTime.Now;
                    if (BackgroundPricingManager.UpdateSecurityChanges(SecurityManager.DefaultTenantID))
                    {
                        View.SuccessMessage = "Fee Item updated successfully.";
                    }
                    else
                    {
                        View.ErrorMessage = "Some error has occured.Please contact administrator.";
                    }
                }
            }
            else
            {
                View.InfoMessage = "Fee Item name already exist.";
            }
        }

        /// <summary>
        /// Delete the PackageServiceItemFee Data.
        /// </summary>
        public void DeletePackageServiceItemFeeData(Int32 pkgSvcItemFeeId)
        {
            if (BackgroundPricingManager.IsFeeItemMapped(SecurityManager.DefaultTenantID, pkgSvcItemFeeId))
            {
                View.InfoMessage = "Fee Item can not be deleted because it is in used.";
            }
            else
            {
                PackageServiceItemFee pkgSvcItemFeeObjectInDB = BackgroundPricingManager.GetPackageServiceItemFeeByID(SecurityManager.DefaultTenantID, pkgSvcItemFeeId);
                pkgSvcItemFeeObjectInDB.PSIF_IsDeleted = true;
                pkgSvcItemFeeObjectInDB.PSIF_ModifiedID = View.currentLoggedInUserId;
                pkgSvcItemFeeObjectInDB.PSIF_ModifiedOn = DateTime.Now;
                if (BackgroundPricingManager.UpdateSecurityChanges(SecurityManager.DefaultTenantID))
                {
                    View.SuccessMessage = "Fee Item Deleted successfully.";
                }
                else
                {
                    View.ErrorMessage = "Some error has occured.Please contact administrator.";
                }
            }
        }

        /// <summary>
        /// Method to check fee item name exist or not.
        /// </summary>
        /// <param name="feeItemId"></param>
        /// <returns></returns>
        public Boolean IsFeeItemNameExist(Int32? feeItemId)
        {
            return BackgroundPricingManager.IsFeeItemNameExist(SecurityManager.DefaultTenantID, View.ItemFeeName, feeItemId);
        }
    }
}
