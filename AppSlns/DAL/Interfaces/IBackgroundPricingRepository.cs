using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Interfaces
{
    public interface IBackgroundPricingRepository
    {
        #region Manage Fee Record

        /// <summary>
        /// Get Service Item fee record by service item Fee record id.
        /// </summary>
        /// <param name="pkgSvcFeeItemId">svcItemFeeRecordId</param>
        /// <returns></returns>
        Entity.ServiceItemFeeRecord GetServiceItemFeeRecordByID(Int32 svcItemFeeRecordId);

        /// <summary>
        /// Save Service item fee record object.
        /// </summary>
        /// <param name="pkgSvcItemFeeObject">svcItemFeeRecordObject</param>
        /// <returns>Boolean</returns>
        Boolean SaveServiceItemFeeRecord(Entity.ServiceItemFeeRecord svcItemFeeRecordObject);

        /// <summary>
        /// Check that Fee Item Exist or not.
        /// </summary>
        /// <param name="feeItemName">feeItemName</param>
        /// <param name="feeItemId">feeItemId</param>
        /// <returns>Boolean</returns>
        Boolean IsFeeItemRecordExist(Int32 feeItemId, String fieldValue);

        /// <summary>
        /// Get all county list.
        /// </summary>
        /// <returns></returns>
        List<Entity.County> GetCountyListByStateId(Int32 stateId);

        Entity.County GetCountyByCountyId(Int32 countyId);

        DataTable GetServiceFeeItemRecordContract(Int32 feeItemId);

        DataTable GetAditionalServiceItemFeeRecordContract(Int32 feeItemId);

        /// <summary>
        /// Get global PackageServiceItemFee List
        /// </summary>
        /// <returns></returns>
        List<Entity.PackageServiceItemFee> GetPackageServiceFeeItemGlobal();
        Boolean UpdateSecurityChanges();
        #endregion

        #region Manage Fee Item
        /// <summary>
        /// Get PackageServiceItemFee List
        /// </summary>
        /// <returns></returns>
        List<Entity.PackageServiceItemFee> GetPackageServiceFeeItemList();

        /// <summary>
        /// Get package service item fee record by ItemFeeId
        /// </summary>
        /// <param name="pkgSvcFeeItemId">pkgSvcFeeItemId</param>
        /// <returns></returns>
        Entity.PackageServiceItemFee GetPackageServiceItemFeeByID(Int32 pkgSvcFeeItemId);

        /// <summary>
        /// Save package service fee item record.
        /// </summary>
        /// <param name="pkgSvcItemFeeObject">pkgSvcItemFeeObject</param>
        /// <returns>Boolean</returns>
        Boolean SavePackageServiceItemFeeRecord(Entity.PackageServiceItemFee pkgSvcItemFeeObject);

        /// <summary>
        /// Check that Fee Item Exist or not.
        /// </summary>
        /// <param name="feeItemName">feeItemName</param>
        /// <param name="feeItemId">feeItemId</param>
        /// <returns>Boolean</returns>
        Boolean IsFeeItemNameExist(String feeItemName, Int32? feeItemId);

        /// <summary>
        /// Check that Fee Item Exist or not.
        /// </summary>
        /// <param name="feeItemName">feeItemName</param>
        /// <param name="feeItemId">feeItemId</param>
        /// <returns>Boolean</returns>
        Boolean IsFeeItemExistForFeeItemType(Int32 feeItemTypeId, Int32? feeItemId);

        /// <summary>
        /// Check that Fee Item mapped or not.
        /// </summary>
        /// <param name="feeItemId">feeItemId</param>
        /// <returns>Boolean</returns>
        Boolean IsFeeItemMapped(Int32 feeItemId);

        #endregion

        #region Manage Service Item
        /// <summary>
        /// Get Attribute group list corresponding to bkgPackageSvcId.
        /// </summary>
        /// <param name="bkgPackageSvcId">bkgPackageSvcId</param>
        /// <returns></returns>
        List<BkgSvcAttributeGroup> GetBkgSvcAttributeGroupById(Int32 bkgPackageSvcId);
        
        /// <summary>
        /// Get Parent Package service items of selected service.
        /// </summary>
        /// <param name="bkgPackageSvcId">bkgPackageSvcId</param>
        /// <returns></returns>
        List<PackageServiceItem> GetParentPackageServiceItemList(Int32 bkgPackageSvcId, Int32 BkgPackageHierarchyMappingId);
        
        /// <summary>
        /// Get list of service items of service.
        /// </summary>
        /// <param name="bkgPackageSvcId">bkgPackageSvcId</param>
        /// <returns></returns>
        List<PackageServiceItem> GetPackageServiceItemList(Int32 bkgPackageSvcId, Int32 BkgPackageHierarchyMappingId);
        
        /// <summary>
        /// Get package service item corresponding to package service item id.
        /// </summary>
        /// <param name="packageServiceItemId"></param>
        /// <returns></returns>
        PackageServiceItem GetPackageServiceItemData(Int32 packageServiceItemId);
        
        /// <summary>
        /// Method to save the changes of client context.
        /// </summary>
        /// <returns></returns>
        Boolean SaveClientChanges();
        
        /// <summary>
        /// Method to save the package service item object.
        /// </summary>
        /// <param name="packageServiceItemObject"></param>
        /// <returns></returns>
        Boolean SavePackageServiceItemData(PackageServiceItem packageServiceItemObject,Int32 quantityGrpId);

        /// <summary>
        /// Check that service item of same name Exist for service or not.
        /// </summary>
        /// <param name="feeItemName">serviceItemName</param>
        /// <param name="feeItemId">PSI_Id</param>
        /// <param name="bkgPackageSvcId">bkgPackageSvcId</param>
        /// <returns>Boolean</returns>
        Boolean IsServiceItemNameExist(String serviceItemName, Int32? PSI_Id, Int32 bkgPackageSvcId);
        
        /// <summary>
        /// Method to chedk is Hierarchy mapped with background order or not.
        /// </summary>
        /// <param name="PSI_BkgPackageHierarchyMappingId">PSI_BkgPackageHierarchyMappingId</param>
        /// <returns></returns>
        Boolean IsPackageNodeMapped(Int32 PSI_BkgPackageHierarchyMappingId);

        /// <summary>
        /// Method to save the package service fee mapping.
        /// </summary>
        /// <param name="PackageServiceFeeMapping">packageServiceFeeMappingNewRecord</param>
        /// <returns></returns>
        Boolean SavePackageServiceItemFeeMapping(PackageServiceFeeMapping packageServiceFeeMappingNewRecord);
        Boolean SavePackageServiceItemPrice(PackageServiceItemPrice packageServiceItemPrice);

        DataTable GetServiceItemListAssociatedWithPackage(Int32 bphmid, Int32 attributeGrpId, Int32 currentSrvItmId);
       
         /// <summary>
        /// Method to create automatic rule
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="packageSrvcItemId"></param>
        /// <param name="serviceItemName"></param>
        /// <param name="serviceItemTypeId"></param>
        /// <returns></returns>
        String CreateAutomaticSrchRule(Int32 tenantId, Int32 currentLoggedInUserId, Int32 packageSrvcItemId, String serviceItemName, Int32 serviceItemTypeId);
        #endregion

        #region Manage Service Item's Fee item

        List<PackageServiceItemFee> GetPackageServiceItemFeeItemList(Int32 packageServiceItemID);
        Boolean IfServiceItemFeeItemExists(String feeItemName, Int32? feeItemId, Int32 packageServiceItemID);
        PackageServiceItemFee GetPackageServiceItemFeeItemByID(Int32 pkgSvcFeeItemId);
        Boolean SetPackageServiceItemFeeItem(PackageServiceItemFee pkgSvcItemFeeObject, Int32 packageServiceItemID, Decimal? fixedFeeAmount);
        Boolean UpdateTenantChanges();

        Boolean SaveLocalServiceItemFeeRecord(ServiceItemFeeRecord feeRecord);
        List<ServiceItemFeeRecord> GetServiceItemFeeRecordList(Int32 packageServiceItemFeeItemID);
        Boolean IfFieldValueStateOrCounty(Int32 packageServiceItemFeeItemID);
        ServiceItemFeeRecord GetFeeRecordByFeeRecordID(Int32 serviceItemFeeRecordId);
        List<LocalFeeRecordsInfo> GetLocalServiceItemFeeRecordsBasedOnGlobal(Int32 packageServiceItemFeeItemID);
        String GetGlobalFeeAmount(Int32 packageServiceItemFeeItemID, String fieldValue, String fieldValueState);
        Int32 IsGlobalFeeItemsMapped(Int32 packageServiceItemFeeItemID, String fieldValue, out Boolean IsAllCountyOverride);
        List<LocalFeeItemsInfo> GetFeeItemBasedOnServiceItemID(Int32 packageServiceItemID);
        #endregion
    }
}
