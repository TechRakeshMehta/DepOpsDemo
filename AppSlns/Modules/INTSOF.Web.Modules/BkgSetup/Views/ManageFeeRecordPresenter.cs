#region Namespace
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
using INTSOF.Utils;
using INTSOF.UI.Contract.BkgSetup;
using Business.RepoManagers;
#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public class ManageFeeRecordPresenter : Presenter<IManageFeeRecordView>
    {
        public override void OnViewInitialized()
        {
        }

        public void GetTenants()
        {
            //View.ListTenants = //ComplianceDataManager.getClientTenant();
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
            List<PackageServiceItemFee> tempServiceItemFeeRevordList = new List<PackageServiceItemFee>();
            if (SecurityManager.DefaultTenantID > 0)
                tempServiceItemFeeRevordList = BackgroundPricingManager.GetPackageServiceFeeItemGlobal(SecurityManager.DefaultTenantID);
            tempServiceItemFeeRevordList.Insert(0, new PackageServiceItemFee { PSIF_Name = "--SELECT--", PSIF_ID = -1 });
            View.ListPackageServiceItemFee = tempServiceItemFeeRevordList;
        }

        /// <summary>
        /// Get the List of ServiceItemFeeType list.
        /// </summary>
        public void GetServiceItemFeeRecordListContract()
        {
            if (SecurityManager.DefaultTenantID > 0)
            {
                View.ListServiceItemFeeRecordContract = BackgroundPricingManager.GetServiceItemFeeRecordContract(SecurityManager.DefaultTenantID, View.SelectedFeeItemId);
            }
            else
                View.ListServiceItemFeeRecordContract = new List<ServiceFeeItemRecordContract>();
        }

        /// <summary>
        /// Save the ServiceItemFeeRecord Data.
        /// </summary>
        public void SaveServiceItemFeeRecordData()
        {
            if (!IsFeeItemRecordExist(View.FieldValue))
            {
                ServiceItemFeeRecord svcItemFeeRecordObject = new ServiceItemFeeRecord();
                svcItemFeeRecordObject.SIFR_FeeeItemId = View.SelectedFeeItemId;
                svcItemFeeRecordObject.SIFR_Amount = View.Amount;
                svcItemFeeRecordObject.SIFR_IsDeleted = false;
                if (!View.FieldValue.IsNullOrEmpty())
                {
                    svcItemFeeRecordObject.SIFR_FieldValue = View.FieldValue;
                }
                else
                {
                    svcItemFeeRecordObject.SIFR_FieldValue = null;
                }

                svcItemFeeRecordObject.SIFR_CreatedByID = View.currentLoggedInUserId;
                svcItemFeeRecordObject.SIFR_CreatedOn = DateTime.Now;
                if (BackgroundPricingManager.SaveServiceItemFeeRecord(SecurityManager.DefaultTenantID, svcItemFeeRecordObject))
                {
                    View.SuccessMessage = "Fee Record saved successfully.";
                }
                else
                {
                    View.ErrorMessage = "Some error has occured.Please contact administrator.";
                }
            }
            else
            {
                View.InfoMessage = "Fee Record  already exist.";
            }
        }

        /// <summary>
        /// Update the PackageServiceItemFee Data.
        /// </summary>
        public void UpdateServiceItemFeeRecordData(Int32 svcItemFeeRecordId)
        {

            ServiceItemFeeRecord svcItemFeeRecordObject = BackgroundPricingManager.GetServiceItemFeeRecordByID(SecurityManager.DefaultTenantID, svcItemFeeRecordId);
            svcItemFeeRecordObject.SIFR_FeeeItemId = View.SelectedFeeItemId;
            svcItemFeeRecordObject.SIFR_Amount = View.Amount;
            if (svcItemFeeRecordObject.SIFR_FieldValue != View.FieldValue)
            {
                if (!IsFeeItemRecordExist(View.FieldValue))
                {
                    if (!View.FieldValue.IsNullOrEmpty())
                    {
                        svcItemFeeRecordObject.SIFR_FieldValue = View.FieldValue;
                    }
                    else
                    {
                        svcItemFeeRecordObject.SIFR_FieldValue = null;
                    }
                }
                else
                {
                    View.InfoMessage = "Fee Record already exist.";
                    return;
                }
            }

            svcItemFeeRecordObject.SIFR_ModifiedByID = View.currentLoggedInUserId;
            svcItemFeeRecordObject.SIFR_ModifiedOn = DateTime.Now;
            if (BackgroundPricingManager.UpdateSecurityChanges(SecurityManager.DefaultTenantID))
            {
                View.SuccessMessage = "Fee Record updated successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error has occured.Please contact administrator.";
            }
        }

        /// <summary>
        /// Delete the PackageServiceItemFee Data.
        /// </summary>
        public void DeleteServiceItemFeeRecordData(Int32 svcItemFeeRecordId)
        {
            ServiceItemFeeRecord svcItemFeeRecordObjectInDB = BackgroundPricingManager.GetServiceItemFeeRecordByID(SecurityManager.DefaultTenantID, svcItemFeeRecordId);
            svcItemFeeRecordObjectInDB.SIFR_IsDeleted = true;
            svcItemFeeRecordObjectInDB.SIFR_ModifiedByID = View.currentLoggedInUserId;
            svcItemFeeRecordObjectInDB.SIFR_ModifiedOn = DateTime.Now;
            if (BackgroundPricingManager.UpdateSecurityChanges(SecurityManager.DefaultTenantID))
            {
                View.SuccessMessage = "Fee Record Deleted successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error has occured.Please contact administrator.";
            }
        }

        /// <summary>
        /// Method to check fee Item record exist or not.
        /// </summary>
        /// <param name="feeItemId">fieldValue</param>
        /// <returns></returns>
        public Boolean IsFeeItemRecordExist(String fieldValue)
        {
            return BackgroundPricingManager.IsFeeItemRecordExist(SecurityManager.DefaultTenantID, View.SelectedFeeItemId, fieldValue);
        }

        /// <summary>
        /// Get satate list.
        /// </summary>
        public void GetAllState()
        {
            View.ListAllState = BackgroundSetupManager.GetAllStates();
        }

        /// <summary>
        /// Gets List of All Countries 
        /// </summary>
        public void GetCountries()
        {
            View.lstCountries =  SecurityManager.GetCountries();
        }

        /// <summary>
        /// Get county list by state id.
        /// </summary>
        /// <param name="stateId">stateId</param>
        public void GetCountyByStateId(Int32 stateId)
        {
            List<Entity.County> tempListCounty = BackgroundPricingManager.GetCountyListByStateId(SecurityManager.DefaultTenantID, stateId);
            tempListCounty.Insert(0, new Entity.County { CountyID = -1, CountyName = "--Select--" });
            View.ListCountyByStateId = tempListCounty;
        }

        /// <summary>
        /// Get County By CountyId.
        /// </summary>
        /// <param name="countyId">countyId</param>
        /// <returns></returns>
        public Entity.County GetCountyByCountyId(Int32 countyId)
        {
            return BackgroundPricingManager.GetCountyByCountyId(SecurityManager.DefaultTenantID, countyId);
        }

        public void GetMailingOption()
        {
            View.lstMailingOption = SecurityManager.GetMailingOption();
        }

        public void GetAdditionalServiceFeeOption()
        {
            View.lstAdditionalServiceFeeOption = SecurityManager.GetAdditionalServiceFeeOption();
        }
    }
}
