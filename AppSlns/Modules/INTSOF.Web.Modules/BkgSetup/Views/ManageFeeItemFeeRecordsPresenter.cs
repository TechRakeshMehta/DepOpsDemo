using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;

namespace CoreWeb.BkgSetup.Views
{
    public class ManageFeeItemFeeRecordsPresenter : Presenter<IManageFeeItemFeeRecordView>
    {

        public override void OnViewInitialized()
        {
        }

        public void GetServiceItemFeeRecordList()
        {
            //if (BackgroundPricingManager.IsGlobalFeeItemsMapped(View.TenantId, View.SelectedFeeItemId) > 0)
            //{

            View.ListServiceItemFeeRecord = BackgroundPricingManager.GetLocalServiceItemFeeRecordsBasedOnGlobal(View.TenantId, View.SelectedFeeItemId);
            //}
            //else
            //{
            //    List<LocalFeeRecordsInfo> lstFeeRecords = new List<LocalFeeRecordsInfo>();
            //    List<ServiceItemFeeRecord> lstServiceItemFeeRecords = BackgroundPricingManager.GetServiceItemFeeRecordList(View.TenantId, View.SelectedFeeItemId);
            //    lstFeeRecords = lstServiceItemFeeRecords.AsEnumerable().Select(col =>
            //          new LocalFeeRecordsInfo
            //          {
            //              LocalSFRID = col.SIFR_ID,
            //              LocalSFRFeeeItemId = col.SIFR_FeeeItemId.Value,
            //              LocalSFRFieldValue = col.SIFR_FieldValue,
            //              LocalSFRAmount = col.SIFR_Amount,
            //              GlobalSIFR_ID = -1,
            //              GlobalSIFR_FeeeItemId = null,
            //              GlobalFeeAmount = null,
            //              ISGLobal = false
            //          }).ToList();
            //    View.ListServiceItemFeeRecord = lstFeeRecords;

            //}
        }

        /// <summary>
        /// Get state list.
        /// </summary>
        public void GetAllState(Boolean filtered)
        {
            List<Int32> lstFieldValue = new List<Int32>();
            if (filtered && IfFieldValueStateOrCounty())
            {
                lstFieldValue = BackgroundPricingManager.GetServiceItemFeeRecordList(View.TenantId, View.SelectedFeeItemId)
                                            .Select(obj => Convert.ToInt32(obj.SIFR_FieldValue)).ToList();
            }
            View.ListAllState = BackgroundSetupManager.GetAllStates().Where(obj => !lstFieldValue.Contains(obj.StateID)).ToList();
        }



        /// <summary>
        /// Get county list by state id.
        /// </summary>
        /// <param name="stateId">stateId</param>
        public void GetCountyByStateId(Int32 stateId)
        {
            List<Entity.County> tempListCounty = BackgroundPricingManager.GetCountyListByStateId(SecurityManager.DefaultTenantID, stateId);
            List<Int32> lstFieldValue = BackgroundPricingManager.GetServiceItemFeeRecordList(View.TenantId, View.SelectedFeeItemId)
                .Select(obj => Convert.ToInt32(obj.SIFR_FieldValue)).ToList();
            if (lstFieldValue.Count > 0)
            {
                tempListCounty = tempListCounty.Where(obj => !lstFieldValue.Contains(obj.CountyID)).ToList();
            }
            tempListCounty.Insert(0, new Entity.County { CountyID = -1, CountyName = "--SELECT--" });
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

        /// <summary>
        /// Checks If Field Value is State Or County value for specific Fee Record
        /// </summary>
        /// <returns>True if State Or False if County</returns>
        public Boolean IfFieldValueStateOrCounty()
        {
            return BackgroundPricingManager.IfFieldValueStateOrCounty(View.TenantId, View.SelectedFeeItemId);
        }


        public void DeleteServiceItemFeeRecord(int serviceItemFeeRecordID)
        {

            ServiceItemFeeRecord serviceItemFeeRecordInDB = BackgroundPricingManager.GetFeeRecordByFeeRecordID(View.TenantId, serviceItemFeeRecordID);
            if (serviceItemFeeRecordInDB != null)
            {
                serviceItemFeeRecordInDB.SIFR_IsDeleted = true;
                serviceItemFeeRecordInDB.SIFR_ModifiedByID = View.currentLoggedInUserId;
                serviceItemFeeRecordInDB.SIFR_ModifiedOn = DateTime.Now;
                if (BackgroundPricingManager.UpdateTenantChanges(View.TenantId))
                {
                    View.SuccessMessage = "Fee Record Deleted successfully.";
                }
                else
                {
                    View.ErrorMessage = "Some error has occurred.Please contact administrator.";
                }
            }
            else
            {
                View.InfoMessage = "Fee Record Does Not Exist.";
            }
        }

        public void SaveServiceItemFeeRecord()
        {
            ServiceItemFeeRecord feeRecord = new ServiceItemFeeRecord()
            {
                SIFR_FeeeItemId = View.SelectedFeeItemId,
                SIFR_Amount = View.ViewContract.Amount,
                SIFR_IsDeleted = false,
                SIFR_CreatedByID = View.currentLoggedInUserId,
                SIFR_CreatedOn = DateTime.Now,
                SIFR_FieldValue = View.ViewContract.FieldValue
            };
            if (BackgroundPricingManager.SaveLocalServiceItemFeeRecord(View.TenantId, feeRecord))
            {
                View.SuccessMessage = "Fee Record updated successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error has occurred.Please contact administrator.";
            }
        }

        public ServiceItemFeeRecord GetServiceItemFeeRecord(Int32 serviceItemFeeRecordID)
        {
            return BackgroundPricingManager.GetFeeRecordByFeeRecordID(View.TenantId, serviceItemFeeRecordID);
        }

        public String GetGlobalFeeAmount(String fieldValue, String fieldValueState = null)
        {
            return BackgroundPricingManager.GetGlobalFeeAmount(View.TenantId, View.SelectedFeeItemId, fieldValue, fieldValueState);
        }

        public String GetCurrentlkpServiceItemFeeTypeSIFTCode(Int32 serviceItemFeeRecordID, Boolean isGlobal)
        {
            if (isGlobal)
            {
                Entity.ServiceItemFeeRecord serviceItemFeeRecord = BackgroundPricingManager.GetServiceItemFeeRecordByID(View.TenantId, serviceItemFeeRecordID);
                return serviceItemFeeRecord.PackageServiceItemFee.lkpServiceItemFeeType.SIFT_Code;
            }
            else
            {
                ServiceItemFeeRecord serviceItemFeeRecord = GetServiceItemFeeRecord(serviceItemFeeRecordID);
                return serviceItemFeeRecord.PackageServiceItemFee.lkpServiceItemFeeType.SIFT_Code;
            }
        }

        public Boolean CheckIfFeeItemIsFixedType()
        {
            String feeItemTypeName = BackgroundPricingManager.GetPackageServiceItemFeeItemByID(View.TenantId, View.SelectedFeeItemId).lkpServiceItemFeeType.SIFT_Name;
            if (feeItemTypeName.ToLower().Equals("fixed fee"))
                return true;
            return false;
        }

    }
}
