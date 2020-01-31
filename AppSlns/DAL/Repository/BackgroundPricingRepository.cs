using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Repository
{
    public class BackgroundPricingRepository : ClientBaseRepository, IBackgroundPricingRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _ClientDBContext;

        ///// <summary>
        ///// Default constructor to initialize class level variables.
        ///// </summary>
        public BackgroundPricingRepository(Int32 tenantId)
            : base(tenantId)
        {
            _ClientDBContext = base.ClientDBContext;
        }

        #region Manage Fee Record

        /// <summary>
        /// Get Service Item fee record by service item Fee record id.
        /// </summary>
        /// <param name="pkgSvcFeeItemId">svcItemFeeRecordId</param>
        /// <returns></returns>
        Entity.ServiceItemFeeRecord IBackgroundPricingRepository.GetServiceItemFeeRecordByID(Int32 svcItemFeeRecordId)
        {
            return base.SecurityContext.ServiceItemFeeRecords.Include("PackageServiceItemFee").FirstOrDefault(cond => cond.SIFR_ID == svcItemFeeRecordId && cond.SIFR_IsDeleted == false);
        }

        /// <summary>
        /// Save Service item fee record object.
        /// </summary>
        /// <param name="pkgSvcItemFeeObject">svcItemFeeRecordObject</param>
        /// <returns>Boolean</returns>
        Boolean IBackgroundPricingRepository.SaveServiceItemFeeRecord(Entity.ServiceItemFeeRecord svcItemFeeRecordObject)
        {
            base.SecurityContext.ServiceItemFeeRecords.AddObject(svcItemFeeRecordObject);
            if (base.SecurityContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Check that Fee Item Exist or not.
        /// </summary>
        /// <param name="feeItemName">feeItemName</param>
        /// <param name="feeItemId">feeItemId</param>
        /// <returns>Boolean</returns>
        Boolean IBackgroundPricingRepository.IsFeeItemRecordExist(Int32 feeItemId, String fieldValue)
        {
            if (fieldValue.IsNullOrEmpty())
            {
                if (base.SecurityContext.ServiceItemFeeRecords.Any(x => x.SIFR_FeeeItemId == feeItemId && x.SIFR_IsDeleted == false))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (base.SecurityContext.ServiceItemFeeRecords.Any(x => x.SIFR_FeeeItemId == feeItemId && x.SIFR_FieldValue == fieldValue && x.SIFR_IsDeleted == false))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Get all state list.
        /// </summary>
        /// <returns></returns>
        List<Entity.County> IBackgroundPricingRepository.GetCountyListByStateId(Int32 stateId)
        {
            return base.SecurityContext.Counties.Where(cond => cond.StateID == stateId).ToList();
        }

        public Entity.County GetCountyByCountyId(Int32 countyId)
        {
            return base.SecurityContext.Counties.FirstOrDefault(cond => cond.CountyID == countyId);
        }

        DataTable IBackgroundPricingRepository.GetServiceFeeItemRecordContract(Int32 feeItemId)
        {
            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetServiceItemFeeRecord", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@feeItemId", feeItemId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        DataTable IBackgroundPricingRepository.GetAditionalServiceItemFeeRecordContract(Int32 feeItemId)
        {
            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAdditionalServiceItemFeeRecord", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@feeItemId", feeItemId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        /// <summary>
        /// Get PackageServiceItemFee List
        /// </summary>
        /// <returns></returns>
        List<Entity.PackageServiceItemFee> IBackgroundPricingRepository.GetPackageServiceFeeItemGlobal()
        {
            return base.SecurityContext.PackageServiceItemFees.Include("lkpServiceItemFeeType").Where(cond => !cond.PSIF_IsDeleted && cond.PSIF_IsGlobal == true).ToList();
        }

        Boolean IBackgroundPricingRepository.UpdateSecurityChanges()
        {
            if (base.SecurityContext.SaveChanges() > 0)
                return true;
            return false;
        }
        #endregion

        #region Manage Fee Item
        /// <summary>
        /// Get PackageServiceItemFee List
        /// </summary>
        /// <returns></returns>
        List<Entity.PackageServiceItemFee> IBackgroundPricingRepository.GetPackageServiceFeeItemList()
        {
            return base.SecurityContext.PackageServiceItemFees.Include("lkpServiceItemFeeType").Where(cond => !cond.PSIF_IsDeleted).ToList();
        }

        /// <summary>
        /// Get package service item fee record by ItemFeeId
        /// </summary>
        /// <param name="pkgSvcFeeItemId">pkgSvcFeeItemId</param>
        /// <returns></returns>
        Entity.PackageServiceItemFee IBackgroundPricingRepository.GetPackageServiceItemFeeByID(Int32 pkgSvcFeeItemId)
        {
            return base.SecurityContext.PackageServiceItemFees.Include("lkpServiceItemFeeType").FirstOrDefault(cond => cond.PSIF_ID == pkgSvcFeeItemId && !cond.PSIF_IsDeleted);
        }

        /// <summary>
        /// Save package service fee item record.
        /// </summary>
        /// <param name="pkgSvcItemFeeObject">pkgSvcItemFeeObject</param>
        /// <returns>Boolean</returns>
        Boolean IBackgroundPricingRepository.SavePackageServiceItemFeeRecord(Entity.PackageServiceItemFee pkgSvcItemFeeObject)
        {
            base.SecurityContext.PackageServiceItemFees.AddObject(pkgSvcItemFeeObject);
            if (base.SecurityContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Check that Fee Item Exist or not.
        /// </summary>
        /// <param name="feeItemName">feeItemName</param>
        /// <param name="feeItemId">feeItemId</param>
        /// <returns>Boolean</returns>
        Boolean IBackgroundPricingRepository.IsFeeItemNameExist(String feeItemName, Int32? feeItemId)
        {
            if (feeItemId != null)
            {
                if (base.SecurityContext.PackageServiceItemFees.Any(x => x.PSIF_Name.ToLower() == feeItemName.ToLower() && x.PSIF_ID != feeItemId && !x.PSIF_IsDeleted))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (base.SecurityContext.PackageServiceItemFees.Any(x => x.PSIF_Name.ToLower() == feeItemName.ToLower() && !x.PSIF_IsDeleted))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Check that Fee Item Exist or not.
        /// </summary>
        /// <param name="feeItemName">feeItemName</param>
        /// <param name="feeItemId">feeItemId</param>
        /// <returns>Boolean</returns>
        Boolean IBackgroundPricingRepository.IsFeeItemExistForFeeItemType(Int32 feeItemTypeId, Int32? feeItemId)
        {
            if (feeItemId != null)
            {
                if (base.SecurityContext.PackageServiceItemFees.Any(x => x.PSIF_ServiceItemFeeType == feeItemTypeId && x.PSIF_ID != feeItemId && !x.PSIF_IsDeleted))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (base.SecurityContext.PackageServiceItemFees.Any(x => x.PSIF_ServiceItemFeeType == feeItemTypeId && !x.PSIF_IsDeleted))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Check that Fee Item mapped or not.
        /// </summary>
        /// <param name="feeItemId">feeItemId</param>
        /// <returns>Boolean</returns>
        Boolean IBackgroundPricingRepository.IsFeeItemMapped(Int32 feeItemId)
        {
            return base.SecurityContext.ServiceItemFeeRecords.Any(cond => cond.SIFR_FeeeItemId == feeItemId && cond.SIFR_IsDeleted == false);
        }
        #endregion

        #region Manage Service Item
        /// <summary>
        /// Get Attribute group list corresponding to bkgPackageSvcId.
        /// </summary>
        /// <param name="bkgPackageSvcId">bkgPackageSvcId</param>
        /// <returns></returns>
        List<BkgSvcAttributeGroup> IBackgroundPricingRepository.GetBkgSvcAttributeGroupById(Int32 bkgPackageSvcId)
        {
            return _ClientDBContext.BkgPackageSvcAttributes.Include("BkgAttributeGroupMapping").Include("BkgAttributeGroupMapping.BkgSvcAttributeGroup").Where(cond => cond.BPSA_BkgPackageSvcID == bkgPackageSvcId && cond.BPSA_IsDeleted == false).Select(slct => slct.BkgAttributeGroupMapping.BkgSvcAttributeGroup).Distinct().ToList();
        }

        /// <summary>
        /// Get Parent Package service items of selected service.
        /// </summary>
        /// <param name="bkgPackageSvcId">bkgPackageSvcId</param>
        /// <returns></returns>
        List<PackageServiceItem> IBackgroundPricingRepository.GetParentPackageServiceItemList(Int32 bkgPackageSvcId, Int32 BkgPackageHierarchyMappingId)
        {
            return _ClientDBContext.PackageServiceItems.Where(cond => cond.PSI_PackageServiceID == bkgPackageSvcId && cond.PSI_ParentServiceItemId == null && cond.PSI_BkgPackageHierarchyMappingId == BkgPackageHierarchyMappingId && cond.PSI_IsDeleted == false).ToList();
        }

        /// <summary>
        /// Get list of service items of service.
        /// </summary>
        /// <param name="bkgPackageSvcId">bkgPackageSvcId</param>
        /// <returns></returns>
        List<PackageServiceItem> IBackgroundPricingRepository.GetPackageServiceItemList(Int32 bkgPackageSvcId, Int32 BkgPackageHierarchyMappingId)
        {
            return _ClientDBContext.PackageServiceItems.Include("BkgSvcAttributeGroup").Include("lkpServiceItemType").Where(cond => cond.PSI_PackageServiceID == bkgPackageSvcId && cond.PSI_BkgPackageHierarchyMappingId == BkgPackageHierarchyMappingId && cond.PSI_IsDeleted == false).ToList();
        }

        /// <summary>
        /// Get package service item corresponding to package service item id.
        /// </summary>
        /// <param name="packageServiceItemId"></param>
        /// <returns></returns>
        PackageServiceItem IBackgroundPricingRepository.GetPackageServiceItemData(Int32 packageServiceItemId)
        {
            return _ClientDBContext.PackageServiceItems.FirstOrDefault(cond => cond.PSI_ID == packageServiceItemId && cond.PSI_IsDeleted == false);
        }

        /// <summary>
        /// Method to save the changes of client context.
        /// </summary>
        /// <returns></returns>
        Boolean IBackgroundPricingRepository.SaveClientChanges()
        {
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Method to save the package service item object.
        /// </summary>
        /// <param name="packageServiceItemObject"></param>
        /// <returns></returns>
        Boolean IBackgroundPricingRepository.SavePackageServiceItemData(PackageServiceItem packageServiceItemObject, Int32 quantityGrpId)
        {
            if (quantityGrpId > AppConsts.NONE)
                packageServiceItemObject.PSI_QuantityGroup = quantityGrpId;

            if (quantityGrpId == AppConsts.NONE)
                packageServiceItemObject.PSI_QuantityGroup = null;

            _ClientDBContext.PackageServiceItems.AddObject(packageServiceItemObject);
            if (_ClientDBContext.SaveChanges() > 0)
            {
                if (quantityGrpId == -1)
                    packageServiceItemObject.PackageServiceItem3= packageServiceItemObject;
                ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check that service item of same name Exist for service or not.
        /// </summary>
        /// <param name="feeItemName">serviceItemName</param>
        /// <param name="feeItemId">PSI_Id</param>
        /// <param name="bkgPackageSvcId">bkgPackageSvcId</param>
        /// <returns>Boolean</returns>
        Boolean IBackgroundPricingRepository.IsServiceItemNameExist(String serviceItemName, Int32? PSI_Id, Int32 bkgPackageSvcId)
        {
            if (PSI_Id != null)
            {
                if (_ClientDBContext.PackageServiceItems.Any(x => x.PSI_ServiceItemName.ToLower() == serviceItemName.ToLower() && x.PSI_ID != PSI_Id && !x.PSI_IsDeleted && x.PSI_PackageServiceID == bkgPackageSvcId))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (_ClientDBContext.PackageServiceItems.Any(x => x.PSI_ServiceItemName.ToLower() == serviceItemName.ToLower() && !x.PSI_IsDeleted && x.PSI_PackageServiceID == bkgPackageSvcId))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Method to chedk is Hierarchy mapped with background order or not.
        /// </summary>
        /// <param name="PSI_BkgPackageHierarchyMappingId">PSI_BkgPackageHierarchyMappingId</param>
        /// <returns></returns>
        Boolean IBackgroundPricingRepository.IsPackageNodeMapped(Int32 PSI_BkgPackageHierarchyMappingId)
        {
            return _ClientDBContext.BkgOrderPackages.Any(cond => cond.BOP_BkgPackageHierarchyMappingID == PSI_BkgPackageHierarchyMappingId && cond.BOP_IsDeleted == false);
        }

        /// <summary>
        /// Method to save the package service fee mapping.
        /// </summary>
        /// <param name="PackageServiceFeeMapping">packageServiceFeeMappingNewRecord</param>
        /// <returns></returns>
        Boolean IBackgroundPricingRepository.SavePackageServiceItemFeeMapping(PackageServiceFeeMapping packageServiceFeeMappingNewRecord)
        {
            _ClientDBContext.PackageServiceFeeMappings.AddObject(packageServiceFeeMappingNewRecord);
            return true;
        }

        Boolean IBackgroundPricingRepository.SavePackageServiceItemPrice(PackageServiceItemPrice packageServiceItemPrice)
        {
            _ClientDBContext.PackageServiceItemPrices.AddObject(packageServiceItemPrice);
            return true;
        }

        DataTable IBackgroundPricingRepository.GetServiceItemListAssociatedWithPackage(Int32 bphmId, Int32 attribteGrpId,Int32 currentSrvItmId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetServiceItemListAssociatedWithPackage", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BPHM_ID", bphmId);
                command.Parameters.AddWithValue("@attributeGrpId", attribteGrpId);
                command.Parameters.AddWithValue("@currentSrvcItmId", currentSrvItmId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

         /// <summary>
        /// Method to create automatic rule
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="packageSrvcItemId"></param>
        /// <param name="serviceItemName"></param>
        /// <param name="serviceItemTypeId"></param>
        /// <returns></returns>
       public String CreateAutomaticSrchRule(Int32 tenantId, Int32 currentLoggedInUserId, Int32 packageSrvcItemId, String serviceItemName, Int32 serviceItemTypeId)
        {
            EntityConnection connection = ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_CreateAutomaticSrchRuleForServiceItem", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageServiceItemId", packageSrvcItemId);
                command.Parameters.AddWithValue("@ServiceItemName", serviceItemName);
                command.Parameters.AddWithValue("@ServiceItemTypeId", serviceItemTypeId);
                command.Parameters.AddWithValue("@TenantId", tenantId);
                command.Parameters.AddWithValue("@UserId", currentLoggedInUserId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["Result"].ToString();
                }
            }
            return String.Empty;  
        }
        #endregion


        #region Manage Service Item's Fee item
        /// <summary>
        /// Get PackageServiceItemFee List for specific Package Service Item
        /// </summary>
        /// <returns></returns>
        public List<PackageServiceItemFee> GetPackageServiceItemFeeItemList(Int32 packageServiceItemID)
        {
            List<Int32> lstFeeItemId = _ClientDBContext.PackageServiceFeeMappings.Where(obj => obj.PSFM_PackageServiceItemID == packageServiceItemID
                && obj.PSFM_IsDeleted == false && obj.PSFM_IsGlobal == false).Select(obj => obj.PSFM_FeeItemID).ToList();
            return _ClientDBContext.PackageServiceItemFees.Include("lkpServiceItemFeeType").Include("ServiceItemFeeRecords").Where(cond => !cond.PSIF_IsDeleted && cond.PSIF_IsGlobal == false && lstFeeItemId.Contains(cond.PSIF_ID)).ToList();
        }

        /// <summary>
        /// Check that Service Item Fee Item Exist or not.
        /// </summary>
        /// <param name="feeItemName">feeItemName</param>
        /// <param name="feeItemId">feeItemId</param>
        /// <returns>Boolean</returns>
        Boolean IBackgroundPricingRepository.IfServiceItemFeeItemExists(String feeItemName, Int32? feeItemId, Int32 packageServiceItemID)
        {
            if (feeItemId.HasValue)
            {
                return _ClientDBContext.PackageServiceItemFees.Any(cond => cond.PSIF_ID == feeItemId && !cond.PSIF_IsDeleted);
            }
            else
            {
                return GetPackageServiceItemFeeItemList(packageServiceItemID).Any(obj => obj.PSIF_Name.Trim().ToLower() == feeItemName.Trim().ToLower());
            }
        }

        /// <summary>
        /// Get package service item fee item record by FeeItemId
        /// </summary>
        /// <param name="pkgSvcFeeItemId">pkgSvcFeeItemId</param>
        /// <returns></returns>
        public PackageServiceItemFee GetPackageServiceItemFeeItemByID(Int32 pkgSvcFeeItemId)
        {
            PackageServiceItemFee feeItem = _ClientDBContext.PackageServiceItemFees.Include("lkpServiceItemFeeType").Include("ServiceItemFeeRecords").FirstOrDefault(cond => cond.PSIF_ID == pkgSvcFeeItemId && !cond.PSIF_IsDeleted && cond.PSIF_IsGlobal == false);
            _ClientDBContext.Refresh(RefreshMode.StoreWins, feeItem);
            return feeItem;
        }

        /// <summary>
        /// Save package service item fee item record.
        /// </summary>
        /// <param name="pkgSvcItemFeeObject">pkgSvcItemFeeObject</param>
        /// <returns>Boolean</returns>
        Boolean IBackgroundPricingRepository.SetPackageServiceItemFeeItem(PackageServiceItemFee pkgSvcItemFeeObject, Int32 packageServiceItemID, Decimal? fixedFeeAmount)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_InsertPackageServiceItemFeeAndMapping", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@packageServiceItemID", packageServiceItemID);
                command.Parameters.AddWithValue("@feeItemTypeId", pkgSvcItemFeeObject.PSIF_ServiceItemFeeType);
                command.Parameters.AddWithValue("@currentLoggedInUserId", pkgSvcItemFeeObject.PSIF_CreatedByID);
                command.Parameters.AddWithValue("@feeItemId", (!pkgSvcItemFeeObject.PSIF_IsDeleted ? (Int32?)null : pkgSvcItemFeeObject.PSIF_ID));
                command.Parameters.AddWithValue("@isDelete", pkgSvcItemFeeObject.PSIF_IsDeleted);
                command.Parameters.AddWithValue("@feeItemName", pkgSvcItemFeeObject.PSIF_Name);
                command.Parameters.AddWithValue("@feeItemDescription", pkgSvcItemFeeObject.PSIF_Description);
                command.Parameters.AddWithValue("@fixedFeeAmount", fixedFeeAmount);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                return true;
            }           
        }

        /// <summary>
        /// Update Tenant after modifying record.
        /// </summary>
        /// <returns>Boolean</returns>
        Boolean IBackgroundPricingRepository.UpdateTenantChanges()
        {
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Save Service Item Fee Record.
        /// </summary>
        /// <param name="feeRecord">ServiceItemFeeRecord</param>
        /// <returns>Boolean</returns>
        Boolean IBackgroundPricingRepository.SaveLocalServiceItemFeeRecord(ServiceItemFeeRecord feeRecord)
        {
            _ClientDBContext.ServiceItemFeeRecords.AddObject(feeRecord);
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Get ServiceItemFeeRecord List for specific Fee Item
        /// </summary>
        /// <returns></returns>
        List<ServiceItemFeeRecord> IBackgroundPricingRepository.GetServiceItemFeeRecordList(Int32 packageServiceItemFeeItemID)
        {
            return _ClientDBContext.ServiceItemFeeRecords.Include("PackageServiceItemFee").Where(cond => cond.SIFR_IsDeleted == false && cond.SIFR_FeeeItemId == packageServiceItemFeeItemID).ToList();
        }

        /// <summary>
        /// Checks If Field Value is State Or County value for specific Fee Record
        /// </summary>
        /// <returns>True if State Or False if County</returns>
        Boolean IBackgroundPricingRepository.IfFieldValueStateOrCounty(Int32 packageServiceItemFeeItemID)
        {
            PackageServiceItemFee packageServiceItemFee = GetPackageServiceItemFeeItemByID(packageServiceItemFeeItemID);
            if (packageServiceItemFee.lkpServiceItemFeeType.SIFT_Code == ServiceItemFeeType.COUNTY_COURT_FEE.GetStringValue())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get ServiceItemFeeRecord record by ServiceItemFeeRecordId
        /// </summary>
        /// <param name="pkgSvcFeeItemId">serviceItemFeeRecordId</param>
        /// <returns></returns>
        public ServiceItemFeeRecord GetFeeRecordByFeeRecordID(Int32 serviceItemFeeRecordId)
        {
            return _ClientDBContext.ServiceItemFeeRecords.Include("PackageServiceItemFee").Where(cond => cond.SIFR_IsDeleted == false && cond.SIFR_ID == serviceItemFeeRecordId).FirstOrDefault();
        }

        /// <summary>
        /// Get ServiceItemFeeRecord record  Based On Global
        /// </summary>
        /// <param name="pkgSvcFeeItemId">serviceItemFeeRecordId</param>
        /// <param name="FeeItemTypeID">FeeItemTypeID</param>
        /// <returns></returns>
        public List<LocalFeeRecordsInfo> GetLocalServiceItemFeeRecordsBasedOnGlobal(Int32 packageServiceItemFeeItemID)
        {
            String feeItemTypeCode = GetPackageServiceItemFeeItemByID(packageServiceItemFeeItemID).lkpServiceItemFeeType.SIFT_Code;
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetLocalServiceItemFeeRecordsBasedOnGlobal", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LocalFeeItemID", packageServiceItemFeeItemID);
                command.Parameters.AddWithValue("@FeeItemTypeCode", feeItemTypeCode);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                List<LocalFeeRecordsInfo> lstLocalFeeRecordsInfo = new List<LocalFeeRecordsInfo>();


                lstLocalFeeRecordsInfo = ds.Tables[0].AsEnumerable().Select(col =>
                      new LocalFeeRecordsInfo
                      {
                          LocalSFRID = Convert.ToInt32(col["SIFR_ID"]),
                          LocalSFRFeeeItemId = col["SIFR_FeeeItemId"] == DBNull.Value ? -1 : Convert.ToInt32(col["SIFR_FeeeItemId"]),
                          LocalSFRFieldValue = Convert.ToString(col["SIFR_FieldValue"]),
                          LocalSFRAmount = col["SIFR_Amount"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(col["SIFR_Amount"]),
                          GlobalSIFR_ID = col["GlobalSIFR_ID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["GlobalSIFR_ID"]),
                          GlobalSIFR_FeeeItemId = col["GlobalSIFR_FeeeItemId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["GlobalSIFR_FeeeItemId"]),
                          GlobalFeeAmount = col["GlobalFeeAmount"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(col["GlobalFeeAmount"]),
                          ISGLobal = Convert.ToBoolean(col["ISGLobal"]),
                          StateName = Convert.ToString(col["StateName"]),
                          CountyName = Convert.ToString(col["CountyName"])
                      }).ToList();


                return lstLocalFeeRecordsInfo;
            }

        }

        /// <summary>
        /// Get Global Fee Amount for Selected State/ County
        /// </summary>
        /// <param name="pkgSvcFeeItemId">serviceItemFeeRecordId</param>
        /// <param name="fieldValue">fieldValue</param>
        /// <returns></returns>
        public String GetGlobalFeeAmount(Int32 packageServiceItemFeeItemID, String fieldValue, String fieldValueState)
        {
            Boolean IsAllCountyOverride;
            Int32 globalFeeItemID = IsGlobalFeeItemsMapped(packageServiceItemFeeItemID, fieldValue, out IsAllCountyOverride);
            fieldValue = IsAllCountyOverride ? fieldValueState : fieldValue;
            if (globalFeeItemID > 0)
            {
                base.SecurityContext.Refresh(RefreshMode.StoreWins, base.SecurityContext.ServiceItemFeeRecords);
                String globalFeeAmount = base.SecurityContext.ServiceItemFeeRecords.Where(cond => cond.SIFR_FeeeItemId == globalFeeItemID && cond.SIFR_FieldValue == fieldValue && (cond.SIFR_IsDeleted.HasValue && !cond.SIFR_IsDeleted.Value)).Select(obj => obj.SIFR_Amount).FirstOrDefault().ToString();
                return globalFeeAmount;
            }
            return null;
        }

        public Int32 IsGlobalFeeItemsMapped(Int32 packageServiceItemFeeItemID, String fieldValue, out Boolean IsAllCountyOverride)
        {
            String feeItemTypeCode = GetPackageServiceItemFeeItemByID(packageServiceItemFeeItemID).lkpServiceItemFeeType.SIFT_Code;
            base.SecurityContext.Refresh(RefreshMode.StoreWins, base.SecurityContext.PackageServiceItemFees);
            Int32 packageServiceItemID = GetPackageServiceItemID(packageServiceItemFeeItemID);
            Int32 globalFeeItemID = -1, globalFeeItemIDAllCounty = -1, globalFeeItemIDCounty = -1;
            IsAllCountyOverride = false;

            //Get "all county global fee" for "county fee" and override "county fee" - this is the case when local and global fee can be of different types
            if (feeItemTypeCode == ServiceItemFeeType.COUNTY_COURT_FEE.GetStringValue())
            {
                String FeeItemTypeAllCounty = ServiceItemFeeType.ALL_COUNTY_COURT_FEE.GetStringValue();
                //UAT-2935:------------------------------------
                String FeeItemTypeCounty = ServiceItemFeeType.COUNTY_COURT_FEE.GetStringValue();
                globalFeeItemIDCounty = base.SecurityContext.PackageServiceItemFees.Where(cond => cond.lkpServiceItemFeeType.SIFT_Code == FeeItemTypeCounty && !cond.PSIF_IsDeleted).Select(obj => obj.PSIF_ID).FirstOrDefault();
                //----------------------------------------------
                globalFeeItemIDAllCounty = base.SecurityContext.PackageServiceItemFees.Where(cond => cond.lkpServiceItemFeeType.SIFT_Code == FeeItemTypeAllCounty && !cond.PSIF_IsDeleted).Select(obj => obj.PSIF_ID).FirstOrDefault();
                _ClientDBContext.Refresh(RefreshMode.StoreWins, _ClientDBContext.PackageServiceFeeMappings);
                if (_ClientDBContext.PackageServiceFeeMappings.Any(x => x.PSFM_IsGlobal.HasValue && x.PSFM_IsGlobal.Value && (x.PSFM_FeeItemID == globalFeeItemIDAllCounty || x.PSFM_FeeItemID == globalFeeItemIDCounty) && x.PSFM_PackageServiceItemID == packageServiceItemID && (x.PSFM_IsDeleted.HasValue && !x.PSFM_IsDeleted.Value)))
                {
                    //UAT-2935:--------------------------------
                    if (base.SecurityContext.ServiceItemFeeRecords.Any(cond => cond.SIFR_FeeeItemId == globalFeeItemIDCounty && cond.SIFR_FieldValue == fieldValue && (cond.SIFR_IsDeleted.HasValue && !cond.SIFR_IsDeleted.Value)))
                    {
                        return globalFeeItemIDCounty;
                    }
                    //-----------------------------------------
                    IsAllCountyOverride = true;
                    return globalFeeItemIDAllCounty;
                }
            }

            globalFeeItemID = base.SecurityContext.PackageServiceItemFees.Where(cond => cond.lkpServiceItemFeeType.SIFT_Code == feeItemTypeCode && !cond.PSIF_IsDeleted).Select(obj => obj.PSIF_ID).FirstOrDefault();

            _ClientDBContext.Refresh(RefreshMode.StoreWins, _ClientDBContext.PackageServiceFeeMappings);
            if (_ClientDBContext.PackageServiceFeeMappings.Any(x => x.PSFM_IsGlobal.HasValue && x.PSFM_IsGlobal.Value && x.PSFM_FeeItemID == globalFeeItemID && x.PSFM_PackageServiceItemID == packageServiceItemID && (x.PSFM_IsDeleted.HasValue && !x.PSFM_IsDeleted.Value)))
                return globalFeeItemID;

            return -1;
        }

        public Int32 GetPackageServiceItemID(Int32 packageServiceItemFeeItemID)
        {
            return _ClientDBContext.PackageServiceFeeMappings.Where(x => x.PSFM_FeeItemID == packageServiceItemFeeItemID && x.PSFM_IsDeleted == false).Select(x => x.PSFM_PackageServiceItemID).FirstOrDefault();
        }


        /// <summary>
        /// Get PackageServiceItemFee record  Based On Service Item ID
        /// </summary>
        /// <param name="packageServiceItemID">packageServiceItemID</param>
        /// <returns></returns>
        public List<LocalFeeItemsInfo> GetFeeItemBasedOnServiceItemID(Int32 packageServiceItemID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetFeeItemBasedOnServiceItemID", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageServiceItemID", packageServiceItemID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                List<LocalFeeItemsInfo> lstLocalFeeItemsInfo = new List<LocalFeeItemsInfo>();


                lstLocalFeeItemsInfo = ds.Tables[0].AsEnumerable().Select(col =>
                      new LocalFeeItemsInfo
                      {
                          PSIF_ID = Convert.ToInt32(col["PSIF_ID"]),
                          FeeItemName = Convert.ToString(col["FeeItemName"]),
                          FeeItemDescription = Convert.ToString(col["FeeItemDescription"]),
                          FeeItemType = Convert.ToString(col["FeeItemType"]),
                          FeeItemAmount = col["FeeItemAmount"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(col["FeeItemAmount"]),
                      }).ToList();



                return lstLocalFeeItemsInfo;
            }

        }
        #endregion
    }
}
