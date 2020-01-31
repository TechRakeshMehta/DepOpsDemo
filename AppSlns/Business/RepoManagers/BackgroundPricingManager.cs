using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;

namespace Business.RepoManagers
{
    public class BackgroundPricingManager
    {
        private static String ConvertToAttributeXML(DataTable attributeTable)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("BkgSvcAttributeGroupList"));

            IEnumerable<DataRow> attributeRecords = attributeTable.AsEnumerable();
            Int32 attributeGroupID = attributeRecords.Select(col => Convert.ToInt32(col["AttributeGroupID"])).FirstOrDefault();

            attributeRecords.Select(col => col["InstanceID"].ToString()).Distinct().ForEach(instance =>
                {
                    XmlNode exp = el.AppendChild(doc.CreateElement("BkgSvcAttributeDataGroup"));
                    exp.AppendChild(doc.CreateElement("AttributeGroupID")).InnerText = attributeGroupID.ToString();
                    exp.AppendChild(doc.CreateElement("InstanceId")).InnerText = instance;
                    attributeRecords.Where(cond => cond["InstanceID"].ToString() == instance).ForEach(attribute =>
                        {
                            XmlNode expChild = exp.AppendChild(doc.CreateElement("BkgSvcAttributeData"));
                            expChild.AppendChild(doc.CreateElement("BkgAttributeGroupMappingID")).InnerText = attribute["AttributeGroupMappingID"].ToString();
                            expChild.AppendChild(doc.CreateElement("Value")).InnerText = attribute["AttributeValue"].ToString();
                        });
                });

            return doc.OuterXml.ToString();

        }

        #region Manage Fee Record

        public static Boolean SaveServiceItemFeeRecord(Int32 tenantId, Entity.ServiceItemFeeRecord svcItemFeeRecordData)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).SaveServiceItemFeeRecord(svcItemFeeRecordData);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Entity.ServiceItemFeeRecord GetServiceItemFeeRecordByID(Int32 tenantId, Int32 svcItemFeeRecordId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetServiceItemFeeRecordByID(svcItemFeeRecordId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Entity.County> GetCountyListByStateId(Int32 tenantId, Int32 stateId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetCountyListByStateId(stateId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Check that Fee Item Exist or not.
        /// </summary>
        /// <param name="feeItemName">feeItemName</param>
        /// <param name="feeItemId">feeItemId</param>
        /// <returns>Boolean</returns>
        public static Boolean IsFeeItemRecordExist(Int32 tenantId, Int32 feeItemId, String fieldValue)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).IsFeeItemRecordExist(feeItemId, fieldValue);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Entity.County GetCountyByCountyId(Int32 tenantId, Int32 countyId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetCountyByCountyId(countyId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Assign the datatable record in ServiceFeeItemRecordContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List<QueueAuditRecordContract></returns>
        public static List<ServiceFeeItemRecordContract> GetServiceItemFeeRecordContract(Int32 tenantId, Int32 feeItemId)
        {
            try
            {
                IEnumerable<DataRow> rows = BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetServiceFeeItemRecordContract(feeItemId).AsEnumerable();
                return rows.Select(x => new ServiceFeeItemRecordContract
                {
                    SIFR_ID = x["SIFR_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["SIFR_ID"]),
                    SIFR_FeeItemID = x["SIFR_FeeeItemId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["SIFR_FeeeItemId"]),
                    PSIF_Name = x["FeeItemName"].ToString(),
                    FieldValue = x["FieldValue"].ToString(),
                    FeeItemTypeCode = x["FeeItemTypeCode"].ToString(),
                    FieldID = x["FieldID"].ToString(),
                    SIFR_Amount = x["SIFR_Amount"].GetType().Name == "DBNull" ? 0 : Convert.ToDecimal(x["SIFR_Amount"]),
                    State = x["StateName"].IsNullOrEmpty() ? String.Empty : x["StateName"].ToString(),
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<ServiceFeeItemRecordContract> GetAditionalServiceItemFeeRecordContract(Int32 tenantId, Int32 feeItemId)
        {
            try
            {
                IEnumerable<DataRow> rows = BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetAditionalServiceItemFeeRecordContract(feeItemId).AsEnumerable();
                return rows.Select(x => new ServiceFeeItemRecordContract
                {
                    SIFR_ID = x["SIFR_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["SIFR_ID"]),
                    SIFR_FeeItemID = x["SIFR_FeeeItemId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["SIFR_FeeeItemId"]),
                    PSIF_Name = x["FeeItemName"].ToString(),
                    FieldValue = x["FieldCode"].ToString(),
                    FeeItemTypeCode = x["FeeItemTypeCode"].ToString(),
                    FieldID = x["FieldID"].ToString(),
                    SIFR_Amount = x["SIFR_Amount"].GetType().Name == "DBNull" ? 0 : Convert.ToDecimal(x["SIFR_Amount"]),
                    State = x["StateName"].IsNullOrEmpty() ? String.Empty : x["StateName"].ToString(),
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Entity.PackageServiceItemFee> GetPackageServiceFeeItemGlobal(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetPackageServiceFeeItemGlobal();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdateSecurityChanges(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).UpdateSecurityChanges();

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Manage Fee Item
        public static List<Entity.lkpServiceItemFeeType> GetServiceItemFeeTypeList(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpServiceItemFeeType>().ToList();

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SavePackageServiceItemFeeRecord(Int32 tenantId, Entity.PackageServiceItemFee pkgSvcItemFeeObject)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).SavePackageServiceItemFeeRecord(pkgSvcItemFeeObject);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Entity.PackageServiceItemFee GetPackageServiceItemFeeByID(Int32 tenantId, Int32 pkgSvcFeeItemId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetPackageServiceItemFeeByID(pkgSvcFeeItemId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Entity.PackageServiceItemFee> GetPackageServiceFeeItemList(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetPackageServiceFeeItemList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Check that Fee Item Exist or not.
        /// </summary>
        /// <param name="feeItemName">feeItemName</param>
        /// <param name="feeItemId">feeItemId</param>
        /// <returns>Boolean</returns>
        public static Boolean IsFeeItemNameExist(Int32 tenantId, String feeItemName, Int32? feeItemId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).IsFeeItemNameExist(feeItemName, feeItemId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Check that Fee Item Exist or not.
        /// </summary>
        /// <param name="feeItemName">feeItemName</param>
        /// <param name="feeItemId">feeItemId</param>
        /// <returns>Boolean</returns>
        public static Boolean IsFeeItemExistForFeeItemType(Int32 tenantId, Int32 feeItemTypeId, Int32? feeItemId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).IsFeeItemExistForFeeItemType(feeItemTypeId, feeItemId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Check that Fee Item mapped ord not.
        /// </summary>
        /// <param name="feeItemId">feeItemId</param>
        /// <returns>Boolean</returns>
        public static Boolean IsFeeItemMapped(Int32 tenantId, Int32 feeItemId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).IsFeeItemMapped(feeItemId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Manage Service Item
        /// <summary>
        /// Get Attribute group list corresponding to bkgPackageSvcId.
        /// </summary>
        /// <param name="bkgPackageSvcId">bkgPackageSvcId</param>
        /// <returns></returns>
        public static List<BkgSvcAttributeGroup> GetBkgSvcAttributeGroupById(Int32 tenantId, Int32 bkgPackageSvcId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetBkgSvcAttributeGroupById(bkgPackageSvcId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get Parent Package service items of selected service.
        /// </summary>
        /// <param name="bkgPackageSvcId">bkgPackageSvcId</param>
        /// <returns></returns>
        public static List<PackageServiceItem> GetParentPackageServiceItemList(Int32 tenantId, Int32 bkgPackageSvcId, Int32 BkgPackageHierarchyMappingId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetParentPackageServiceItemList(bkgPackageSvcId, BkgPackageHierarchyMappingId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get list of service items of service.
        /// </summary>
        /// <param name="bkgPackageSvcId">bkgPackageSvcId</param>
        /// <returns></returns>
        public static List<PackageServiceItem> GetPackageServiceItemList(Int32 tenantId, Int32 bkgPackageSvcId, Int32 BkgPackageHierarchyMappingId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetPackageServiceItemList(bkgPackageSvcId, BkgPackageHierarchyMappingId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get package service item corresponding to package service item id.
        /// </summary>
        /// <param name="packageServiceItemId"></param>
        /// <returns></returns>
        public static PackageServiceItem GetPackageServiceItemData(Int32 tenantId, Int32 packageServiceItemId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetPackageServiceItemData(packageServiceItemId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to save the changes of client context.
        /// </summary>
        /// <returns></returns>
        public static Boolean SaveClientChanges(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).SaveClientChanges();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to save the package service item object.
        /// </summary>
        /// <param name="packageServiceItemObject"></param>
        /// <returns></returns>
        public static Boolean SavePackageServiceItemData(Int32 tenantId, PackageServiceItem packageServiceItemObject,Int32 quantityGrpId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).SavePackageServiceItemData(packageServiceItemObject,quantityGrpId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<lkpServiceItemType> GetServiceItemTypeList(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpServiceItemType>(tenantId).ToList();

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static lkpServiceItemPriceType GetServiceItemPriceTypeByCode(Int32 tenantId, String code)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpServiceItemPriceType>(tenantId).FirstOrDefault(cond => cond.SIPT_Code == code);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Check that service item of same name Exist for service or not.
        /// </summary>
        /// <param name="feeItemName">serviceItemName</param>
        /// <param name="feeItemId">PSI_Id</param>
        /// <param name="bkgPackageSvcId">bkgPackageSvcId</param>
        /// <returns>Boolean</returns>
        public static Boolean IsServiceItemNameExist(Int32 tenantId, String serviceItemName, Int32? PSI_Id, Int32 bkgPackageSvcId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).IsServiceItemNameExist(serviceItemName, PSI_Id, bkgPackageSvcId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to chedk is Hierarchy mapped with background order or not.
        /// </summary>
        /// <param name="PSI_BkgPackageHierarchyMappingId">PSI_BkgPackageHierarchyMappingId</param>
        /// <returns></returns>
        public static Boolean IsPackageNodeMapped(Int32 tenantId, Int32 PSI_BkgPackageHierarchyMappingId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).IsPackageNodeMapped(PSI_BkgPackageHierarchyMappingId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to save the package service fee mapping.
        /// </summary>
        /// <param name="PackageServiceFeeMapping">packageServiceFeeMappingNewRecord</param>
        /// <returns></returns>
        public static Boolean SavePackageServiceItemFeeMapping(Int32 tenantId, PackageServiceFeeMapping packageServiceFeeMappingNewRecord)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).SavePackageServiceItemFeeMapping(packageServiceFeeMappingNewRecord);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Method to save the package service Price 
        /// </summary>
        /// <param name="PackageServiceFeeMapping">packageServiceFeeMappingNewRecord</param>
        /// <returns></returns>
        public static Boolean SavePackageServiceItemPrice(Int32 tenantId, PackageServiceItemPrice packageServiceItemPrice)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).SavePackageServiceItemPrice(packageServiceItemPrice);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// used for getting serviceitem list in a package
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="bphmid"></param>
        /// <param name="attribteGrpId"></param>
        /// <returns></returns>
        public static List<PackageServiceItem> GetServiceItemListAssociatedWithPackage(Int32 tenantId, Int32 bphmId, Int32 attribteGrpId,Int32 currentSrvItmId=0)
        {
            try
            {
                IEnumerable<DataRow> rows = BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetServiceItemListAssociatedWithPackage(bphmId, attribteGrpId, currentSrvItmId).AsEnumerable();
                return rows.Select(x => new PackageServiceItem
                {
                    PSI_ID = x["PSI_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["PSI_ID"]),
                    PSI_PackageServiceID = x["PSI_PackageServiceID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["PSI_PackageServiceID"]),
                    PSI_ServiceItemName = x["PSI_ServiceItemName"].ToString(),
                    PSI_ServiceItemType = x["PSI_ServiceItemType"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["PSI_ServiceItemType"]),
                    PSI_IsDeleted = Convert.ToBoolean(x["PSI_IsDeleted"]),
                    PSI_CreatedByID = x["PSI_CreatedByID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["PSI_CreatedByID"]),
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
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
        public static String CreateAutomaticSrchRule(Int32 tenantId,Int32 currentLoggedInUserId,Int32 packageSrvcItemId,String serviceItemName,Int32 serviceItemTypeId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).CreateAutomaticSrchRule(tenantId,currentLoggedInUserId,packageSrvcItemId,serviceItemName,serviceItemTypeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Manage Service Item's Fee item

        /// <summary>
        /// Get lkpServiceItemFeeType List for specific tenant
        /// </summary>
        /// <returns></returns>
        public static List<lkpServiceItemFeeType> GetLocalServiceItemFeeTypeList(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpServiceItemFeeType>(tenantId).ToList();

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get PackageServiceItemFee List for specific Package Service Item
        /// </summary>
        /// <returns></returns>
        public static List<PackageServiceItemFee> GetPackageServiceItemFeeItemList(Int32 tenantId, Int32 packageServiceItemID)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetPackageServiceItemFeeItemList(packageServiceItemID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Check that Service Item Fee Item Exist or not.
        /// </summary>
        /// <param name="feeItemName">feeItemName</param>
        /// <param name="feeItemId">feeItemId</param>
        /// <returns>Boolean</returns>
        public static Boolean IfServiceItemFeeItemExists(Int32 tenantId, String feeItemName, Int32? feeItemId, Int32 packageServiceItemID)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).IfServiceItemFeeItemExists(feeItemName, feeItemId, packageServiceItemID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Get package service item fee record by FeeItemId
        /// </summary>
        /// <param name="pkgSvcFeeItemId">pkgSvcFeeItemId</param>
        /// <returns></returns>

        public static PackageServiceItemFee GetPackageServiceItemFeeItemByID(Int32 tenantId, Int32 pkgSvcFeeItemId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetPackageServiceItemFeeItemByID(pkgSvcFeeItemId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Save package service item fee item record.
        /// </summary>
        /// <param name="pkgSvcItemFeeObject">pkgSvcItemFeeObject</param>
        /// <returns>Boolean</returns>
        public static Boolean SetPackageServiceItemFeeItem(Int32 tenantId, PackageServiceItemFee pkgSvcItemFeeObject, Int32 packageServiceItemID, Decimal? fixedFeeAmount)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).SetPackageServiceItemFeeItem(pkgSvcItemFeeObject, packageServiceItemID, fixedFeeAmount);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Update Tenant after modifying record.
        /// </summary>
        /// <returns>Boolean</returns>
        public static Boolean UpdateTenantChanges(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).UpdateTenantChanges();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Save Service Item Fee Record.
        /// </summary>
        /// <param name="feeRecord">ServiceItemFeeRecord</param>
        /// <returns>Boolean</returns>
        public static Boolean SaveLocalServiceItemFeeRecord(Int32 tenantId, ServiceItemFeeRecord feeRecord)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).SaveLocalServiceItemFeeRecord(feeRecord);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get ServiceItemFeeRecord List for specific Fee Item
        /// </summary>
        /// <returns></returns>
        public static List<ServiceItemFeeRecord> GetServiceItemFeeRecordList(Int32 tenantId, Int32 packageServiceItemFeeItemID)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetServiceItemFeeRecordList(packageServiceItemFeeItemID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Checks If Field Value is State Or County value for specific Fee Record
        /// </summary>
        /// <returns>True if State Or False if County</returns>        
        public static Boolean IfFieldValueStateOrCounty(Int32 tenantId, Int32 packageServiceItemFeeItemID)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).IfFieldValueStateOrCounty(packageServiceItemFeeItemID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static ServiceItemFeeRecord GetFeeRecordByFeeRecordID(Int32 tenantId, Int32 serviceItemFeeRecordId)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetFeeRecordByFeeRecordID(serviceItemFeeRecordId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get ServiceItemFeeRecord record  Based On Global
        /// </summary>
        /// <param name="pkgSvcFeeItemId">serviceItemFeeRecordId</param>
        /// <param name="FeeItemTypeID">FeeItemTypeID</param>
        /// <returns></returns>
        public static List<LocalFeeRecordsInfo> GetLocalServiceItemFeeRecordsBasedOnGlobal(Int32 tenantId, Int32 packageServiceItemFeeItemID)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetLocalServiceItemFeeRecordsBasedOnGlobal(packageServiceItemFeeItemID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get Global Fee Amount for Selected State/ County
        /// </summary>
        /// <param name="pkgSvcFeeItemId">serviceItemFeeRecordId</param>
        /// <param name="fieldValue">fieldValue</param>
        /// <returns></returns>
        public static String GetGlobalFeeAmount(Int32 tenantId, Int32 packageServiceItemFeeItemID, String fieldValue, String fieldValueState = null)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetGlobalFeeAmount(packageServiceItemFeeItemID,fieldValue, fieldValueState);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int32 IsGlobalFeeItemsMapped(Int32 tenantId, Int32 packageServiceItemFeeItemID, String fieldValue, out Boolean IsAllCountyOverride)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).IsGlobalFeeItemsMapped(packageServiceItemFeeItemID,fieldValue, out IsAllCountyOverride);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get PackageServiceItemFee record  Based On Service Item ID
        /// </summary>
        /// <param name="packageServiceItemID">packageServiceItemID</param>
        /// <returns></returns>
        public static List<LocalFeeItemsInfo> GetFeeItemBasedOnServiceItemID(Int32 tenantId, Int32 packageServiceItemID)
        {
            try
            {
                return BALUtils.GetBackgroundPricingRepoInstance(tenantId).GetFeeItemBasedOnServiceItemID(packageServiceItemID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion
    }
}
