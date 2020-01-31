using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;

namespace CoreWeb.BkgSetup.Views
{

    public class ManageSvcItemFeeItemsPresenter : Presenter<IManageSvcItemFeeItemsView>
    {
        public override void OnViewInitialized()
        {
        }

        /// <summary>
        /// Get the List of Fee Items.
        /// </summary>
        public void GetPackageSvcItemFeeList()
        {
            View.lstPackageServiceItemFee = BackgroundPricingManager.GetFeeItemBasedOnServiceItemID(View.TenantId, View.PackageServiceItemID);
                //BackgroundPricingManager.GetPackageServiceItemFeeItemList(View.TenantId, View.PackageServiceItemID);
        }

        /// <summary>
        /// Get the List of ServiceItemFeeType list.
        /// </summary>
        public void GetServiceItemFeeTypeList()
        {
            List<lkpServiceItemFeeType> tempServiceItemFeeList = BackgroundPricingManager.GetLocalServiceItemFeeTypeList(View.TenantId).OrderBy(col=>col.SIFT_Name).ToList();
            tempServiceItemFeeList.Insert(0, new lkpServiceItemFeeType { SIFT_Name = "--Select--", SIFT_ID = 0 });
            View.lstServiceItemFeeTypes = tempServiceItemFeeList;
        }

        /// <summary>
        /// Save the PackageServiceItemFee Data.
        /// </summary>
        public void SavePackageServiceItemFeeRecord()
        {
            if (!BackgroundPricingManager.IfServiceItemFeeItemExists(View.TenantId, View.ViewContract.FeeItemName, null, View.PackageServiceItemID))
            {

                PackageServiceItemFee pkgSvcItemFeeObject = new PackageServiceItemFee();
                pkgSvcItemFeeObject.PSIF_Description = View.ViewContract.FeeItemDescription;
                pkgSvcItemFeeObject.PSIF_Name = View.ViewContract.FeeItemName;
                //pkgSvcItemFeeObject.PSIF_Label = View.ViewContract.FeeItemLabel;
                pkgSvcItemFeeObject.PSIF_ServiceItemFeeType = View.ViewContract.FeeItemTypeId;
                pkgSvcItemFeeObject.PSIF_IsDeleted = false;
                pkgSvcItemFeeObject.PSIF_IsGlobal = false;
                pkgSvcItemFeeObject.PSIF_CreatedByID = View.currentLoggedInUserId;
                pkgSvcItemFeeObject.PSIF_CreadtedOn = DateTime.Now;

                if (BackgroundPricingManager.SetPackageServiceItemFeeItem(View.TenantId, pkgSvcItemFeeObject, View.PackageServiceItemID, View.ViewContract.FixedFeeAmount))
                {
                    View.SuccessMessage = "Fee Item saved successfully.";
                }
                else
                {
                    View.ErrorMessage = "Some error has occurred.Please contact administrator.";
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
            if (BackgroundPricingManager.IfServiceItemFeeItemExists(View.TenantId, null, pkgSvcItemFeeId, View.PackageServiceItemID))
            {
                PackageServiceItemFee pkgSvcItemFeeObjectInDB = BackgroundPricingManager.GetPackageServiceItemFeeItemByID(View.TenantId, pkgSvcItemFeeId);
                pkgSvcItemFeeObjectInDB.PSIF_Description = View.ViewContract.FeeItemDescription;
                pkgSvcItemFeeObjectInDB.PSIF_Name = View.ViewContract.FeeItemName;
                //pkgSvcItemFeeObjectInDB.PSIF_Label = View.ViewContract.FeeItemLabel;
                pkgSvcItemFeeObjectInDB.PSIF_ServiceItemFeeType = View.ViewContract.FeeItemTypeId;
                pkgSvcItemFeeObjectInDB.PSIF_ModifiedID = View.currentLoggedInUserId;
                pkgSvcItemFeeObjectInDB.PSIF_ModifiedOn = DateTime.Now;
                if (pkgSvcItemFeeObjectInDB.lkpServiceItemFeeType.SIFT_Name.ToLower().Equals("fixed fee") && View.ViewContract.FixedFeeAmount.HasValue)
                {
                    if (pkgSvcItemFeeObjectInDB.ServiceItemFeeRecords.Count > 0)
                        pkgSvcItemFeeObjectInDB.ServiceItemFeeRecords.FirstOrDefault().SIFR_Amount = View.ViewContract.FixedFeeAmount.Value;
                }
                if (BackgroundPricingManager.UpdateTenantChanges(View.TenantId))
                {
                    if (View.ViewContract.FixedFeeAmount.HasValue && pkgSvcItemFeeObjectInDB.ServiceItemFeeRecords.Count == 0)
                    {
                        ServiceItemFeeRecord feeRecord = new ServiceItemFeeRecord()
                        {

                            SIFR_FeeeItemId = pkgSvcItemFeeObjectInDB.PSIF_ID,
                            SIFR_Amount = View.ViewContract.FixedFeeAmount.Value,
                            SIFR_IsDeleted = false,
                            SIFR_CreatedByID = View.currentLoggedInUserId,
                            SIFR_CreatedOn = DateTime.Now,
                            SIFR_FieldValue = null
                        };
                        if (BackgroundPricingManager.SaveLocalServiceItemFeeRecord(View.TenantId, feeRecord))
                        {
                            View.SuccessMessage = "Fee Item updated successfully.";
                        }
                        else
                        {
                            View.ErrorMessage = "Some error has occurred.Please contact administrator.";
                        }
                    }
                    else
                        View.SuccessMessage = "Fee Item updated successfully.";
                }
                else
                {
                    View.ErrorMessage = "Some error has occurred. Please contact administrator.";
                }

            }
            else
            {
                View.InfoMessage = "Fee Item does not exist.";
            }
        }

        /// <summary>
        /// Delete the PackageServiceItemFee Data.
        /// </summary>
        public void DeletePackageServiceItemFeeData(Int32 pkgSvcItemFeeId)
        {

            PackageServiceItemFee pkgSvcItemFeeObjectInDB = BackgroundPricingManager.GetPackageServiceItemFeeItemByID(View.TenantId, pkgSvcItemFeeId);
            pkgSvcItemFeeObjectInDB.PSIF_IsDeleted = true;
            pkgSvcItemFeeObjectInDB.PSIF_ModifiedID = View.currentLoggedInUserId;
            pkgSvcItemFeeObjectInDB.PSIF_ModifiedOn = DateTime.Now;
            if (BackgroundPricingManager.SetPackageServiceItemFeeItem(View.TenantId, pkgSvcItemFeeObjectInDB, View.PackageServiceItemID, null))
            {
                View.SuccessMessage = "Fee Item Deleted successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error has occurred.Please contact administrator.";
            }

        }

        public void GetFeeItemByID()
        {
            View.packageServiceItemFee =  BackgroundPricingManager.GetPackageServiceItemFeeItemByID(View.TenantId, View.FeeItemId);
        }

    }
}
