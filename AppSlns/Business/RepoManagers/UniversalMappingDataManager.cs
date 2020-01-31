using Entity.ClientEntity;
using Entity.SharedDataEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ClinicalRotation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.UI.Contract.ComplianceOperation;


namespace Business.RepoManagers
{
    public class UniversalMappingDataManager
    {
        public static List<UniversalCategory> GetUniversalCategories(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).GetUniversalCategories();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static List<UniversalCategoryItemMapping> GetUniversalItemsByCategoryID(Int32 tenantId, Int32 universalCategoryID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).GetUniversalItemsByCategoryID(universalCategoryID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static List<UniversalItemAttributeMapping> GetUniversalAttributesByItemID(Int32 tenantId, Int32 universalItemID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).GetUniversalAttributesByItemID(universalItemID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static List<UniversalField> GetUniversalFieldByAttributeDataTypeID(Int32 tenantId, Int32 universalItemID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).GetUniversalFieldByAttributeDataTypeID(universalItemID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }


        #region Univarsal Mapping Setup Screen
        public static List<UniversalMappingContract> GetUniversalMappingTreeData()
        {
            try
            {
                IEnumerable<DataRow> rows = BALUtils.GetUniversalMappingDataRepoInstance(0).GetUniversalMappingTreeData().AsEnumerable();
                return ConvertToUniversalMappingContract(rows);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        private static List<UniversalMappingContract> ConvertToUniversalMappingContract(IEnumerable<DataRow> rows)
        {
            return rows.Select(x => new UniversalMappingContract
            {
                TreeNodeTypeID = x["TreeNodeTypeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["TreeNodeTypeID"]),
                NodeID = x["NodeID"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["NodeID"]),
                ParentNodeID = x["ParentNodeID"].GetType().Name == "DBNull" ? null : Convert.ToString(x["ParentNodeID"]),
                Level = x["Level"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["Level"]),
                DataID = x["DataID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["DataID"]),
                ParentDataID = x["ParentDataID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["ParentDataID"]),
                UICode = x["UICode"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["UICode"]),
                Value = x["Value"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["Value"]),
                Description = x["Description"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["Description"])
            }).ToList();
        }
        public static Boolean SaveUpdateUniversalcategoryData(UniversalCategory UniCatData)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(0).SaveUpdateUniversalcategoryData(UniCatData);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static Boolean DeleteUniversalCategorydata(Int32 UniCatID, Int32 cuurentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(0).DeleteUniversalCategorydata(UniCatID, cuurentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static UniversalCategory GetUniversalCategoryByID(Int32 CategoryID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(0).GetUniversalCategoryByID(CategoryID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static Boolean DeleteUniversalItemByID(Int32 uniItemID, Int32 uniCatID, Int32 currentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(0).DeleteUniversalItemByID(uniItemID, uniCatID, currentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static Boolean SaveUpdateUniverItem(UniversalItem uniItemData)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(0).SaveUpdateUniverItem(uniItemData);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static Boolean DeleteUniversalAttributeByID(Int32 uniItemID, Int32 uniAttributeID, Int32 cuurentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(0).DeleteUniversalAttributeByID(uniItemID, uniAttributeID, cuurentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static UniversalItem GetUniversalItemDetailByID(Int32 uniCatID, Int32 uniItemID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(0).GetUniversalItemDetailByID(uniCatID, uniItemID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static List<lkpUniversalAttributeDataType> GetAttributeDataType()
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<lkpUniversalAttributeDataType>().Where(cond => !cond.LUADT_IsDeleted).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean SaveUpdateAttributeDetails(UniversalAttribute uniAttributeDetails)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(0).SaveUpdateAttributeDetails(uniAttributeDetails);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static UniversalAttribute GetUniversalAttributeDataByID(Int32 uniItemID, Int32 uniAttributeID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(0).GetUniversalAttributeDataByID(uniItemID, uniAttributeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static Boolean IsValidAttributeName(String AttributeName)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(0).IsValidAttributeName(AttributeName);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static Boolean IsValidItemName(String ItemName)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(0).IsValidItemName(ItemName);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static Boolean IsValidCategoryName(String CategoryName)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(0).IsValidCategoryName(CategoryName);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        #endregion

        #region compliance mapping with universal mapping regarding UAT-2305
        public static Boolean SaveUpdateCategoryMappingWithUniversalCat(Int32 tenantId, UniversalCategoryMapping ucmData, String mappingTypeCode)
        {
            try
            {
                Int32 universalMappingTypeID = LookupManager.GetLookUpData<lkpUniversalMappingType>(tenantId).FirstOrDefault(x => x.LUMT_Code == mappingTypeCode).LUMT_ID;
                ucmData.UCM_UniversalMappingTypeID = universalMappingTypeID;
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).SaveUpdateCategoryMappingWithUniversalCat(ucmData);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static UniversalCategoryMapping GetMappedUniversalCategoryDataByID(Int32 tenantId, Int32 categoryID, String mappingTypeCode)
        {
            try
            {
                Int32 universalMappingTypeID = LookupManager.GetLookUpData<lkpUniversalMappingType>(tenantId).FirstOrDefault(x => x.LUMT_Code == mappingTypeCode).LUMT_ID;
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).GetMappedUniversalCategoryDataByID(categoryID, universalMappingTypeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean DeleteCategoryMappingWithUniversalCategory(Int32 tenantId, Int32 categoryMappingID, String mappingTypeCode, Int32 currentLoggedInuserId)
        {
            try
            {
                Int32 universalMappingTypeID = LookupManager.GetLookUpData<lkpUniversalMappingType>(tenantId).FirstOrDefault(x => x.LUMT_Code == mappingTypeCode).LUMT_ID;
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).DeleteCategoryMappingWithUniversalCategory(categoryMappingID, universalMappingTypeID, currentLoggedInuserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean SaveUpdateItemMappingWithUniversalItem(Int32 tenantId, UniversalItemMapping uimData)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).SaveUpdateItemMappingWithUniversalItem(uimData);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static UniversalItemMapping GetMappedUniversalItemDataByID(Int32 tenantId, Int32 uniCateMapId, Int32 catItemMappingId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).GetMappedUniversalItemDataByID(uniCateMapId, catItemMappingId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean DeleteItemMappingWithUniversalItem(Int32 tenantId, Int32 itemMappedID, Int32 currentLoggedInuserId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).DeleteItemMappingWithUniversalItem(itemMappedID, currentLoggedInuserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean SaveUpdateAttributeMappingWithUniversalAttribute(Int32 tenantId, UniversalAttributeMapping uamData)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).SaveUpdateAttributeMappingWithUniversalAttribute(uamData);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static UniversalAttributeMapping GetMappedUniversalAttributeDataByID(Int32 tenantId, Int32 uniItemMapId, Int32 itemAttrMappingId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).GetMappedUniversalAttributeDataByID(uniItemMapId, itemAttrMappingId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean DeleteAttributeMappingWithUniversalAttribute(Int32 tenantId, Int32 attrMappingID, Int32 currentLoggedInuserId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).DeleteAttributeMappingWithUniversalAttribute(attrMappingID, currentLoggedInuserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static UniversalCategoryItemMapping GetUniversalCatItemMappingData(Int32 tenantId, Int32 UCIM_Id)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).GetUniversalCatItemMappingData(UCIM_Id);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static List<UniversalAttributeInputTypeMapping> GetMappedUniversalInputAttributeDataByID(Int32 tenantId, Int32 uniAttributeMapId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).GetMappedUniversalInputAttributeDataByID(uniAttributeMapId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean UpdateAttributeInputMapping(Int32 tenantId, Int32 uniAttributeMapId, List<UniversalAttributeInputTypeMapping> lstInputMapping, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).UpdateAttributeInputMapping(uniAttributeMapId, lstInputMapping, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #endregion

        #region Universal Rotation Mapping View
        public static List<UniversalRotationMappingViewContract> GetUniversalRotationMappingView(Int32 requirementPackageID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.ONE).GetUniversalRotationMappingView(requirementPackageID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean SaveUniversalRequirmentCategoryMappingData(UniversalRotationMappingViewContract updateContract, Int32 loggedInUserID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.ONE).SaveUniversalRequirmentCategoryMappingData(updateContract, loggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean SaveUniversalRequirmentItemMappingData(UniversalRotationMappingViewContract updateContract, int loggedInUserID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.ONE).SaveUniversalRequirmentItemMappingData(updateContract, loggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean SaveUniversalRequirmentAttributeMappingData(UniversalRotationMappingViewContract updateContract, int loggedInUserID)
        {
            try
            {
                Int32 requirementFieldID = updateContract.RequirementFieldID;
                Boolean resultStatus = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.ONE).SaveUniversalRequirmentAttributeMappingData(updateContract, loggedInUserID);

                if (resultStatus)
                {
                    #region Rot Pkg Object Sync Data
                    List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();
                    RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                    objectData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.EDITED.GetStringValue();
                    objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_FIELD.GetStringValue();
                    objectData.NewObjectId = requirementFieldID;
                    lstPackageObjectSynchingData.Add(objectData);
                    String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                    SharedRequirementPackageManager.SaveRequirementPackageObjectForSync(requestXML, loggedInUserID);
                    #endregion
                }
                return resultStatus;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Int32 GetLkpAttributeDataTypeIDByCode(String uniAttrDataTypeCode)
        {
            try
            {
                List<lkpUniversalAttributeDataType> lstUniAttrDataType = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpUniversalAttributeDataType>();
                return lstUniAttrDataType.Where(cond => cond.LUADT_Code == uniAttrDataTypeCode).FirstOrDefault().LUADT_ID;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }


        public static Boolean CopySharedToTenantRequirementUniversalMapping(Int32 tenantId, Int32 sharedRotationPackageId, Int32 tenantRotPackageId, Int32 currentLoggedInuSerId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).CopySharedToTenantRequirementUniversalMapping(sharedRotationPackageId, tenantRotPackageId
                                                                                                                            , currentLoggedInuSerId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean CopyTenantToSharedRequirementUniversalMapping(Int32 tenantId, Int32 sharedCopiedRotationPackageId, Int32 tenantRotPackageId, Int32 currentLoggedInuSerId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).CopyTenantToSharedRequirementUniversalMapping(sharedCopiedRotationPackageId, tenantRotPackageId
                                                                                                                            , currentLoggedInuSerId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean CopySharedToSharedReqUniversalMappingForPkg(Int32 tenantId, Int32 sharedRotationPackageId, Int32 sharedCopiedRotationPackageId, Int32 currentLoggedInuSerId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).CopySharedToSharedReqUniversalMappingForPkg(sharedRotationPackageId, sharedCopiedRotationPackageId
                                                                                                                            , currentLoggedInuSerId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static List<InputTypeRotationAttributeContract> GetUniversalRequirementAttributeInputTypeMapping(int universalReqAttrMappingID)
        {
            try
            {
                List<InputTypeRotationAttributeContract> lstInputMappings = new List<InputTypeRotationAttributeContract>();
                List<UniversalRequirementAttributeInputTypeMapping> lstTmpInputMappings = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.ONE).GetUniversalRequirementAttributeInputTypeMapping(universalReqAttrMappingID);

                foreach (UniversalRequirementAttributeInputTypeMapping item in lstTmpInputMappings)
                {
                    InputTypeRotationAttributeContract inputRotAttrMapping = new InputTypeRotationAttributeContract();
                    //inputRotAttrMapping.ID = item.URAITM_UniversalItemAttributeMappingID.Value;
                    inputRotAttrMapping.ID = item.URAITM_UniversalItemAttributeMappingID;
                    inputRotAttrMapping.InputPriority = item.URAITM_InputPriority;
                    lstInputMappings.Add(inputRotAttrMapping);
                }

                return lstInputMappings;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static List<InputTypeRotationAttributeContract> GetUniversalFieldInputTypeMappings(Int32 universalFieldMappingID)
        {
            try
            {
                List<InputTypeRotationAttributeContract> lstInputMappings = new List<InputTypeRotationAttributeContract>();
                List<Entity.SharedDataEntity.UniversalFieldInputTypeMapping> lstTmpInputMappings = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.ONE).GetUniversalFieldInputTypeMappings(universalFieldMappingID);

                foreach (Entity.SharedDataEntity.UniversalFieldInputTypeMapping item in lstTmpInputMappings)
                {
                    InputTypeRotationAttributeContract inputRotAttrMapping = new InputTypeRotationAttributeContract();
                    //inputRotAttrMapping.ID = item.URAITM_UniversalItemAttributeMappingID.Value;
                    inputRotAttrMapping.ID = item.UFITM_UniversalFieldID;
                    inputRotAttrMapping.InputPriority = item.UFITM_InputPriority;
                    lstInputMappings.Add(inputRotAttrMapping);
                }

                return lstInputMappings;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #endregion

        #region Master Rotation Package

        public static List<UniversalCategoryContract> GetUniversalCategorys()
        {
            try
            {
                List<UniversalCategoryContract> result = new List<UniversalCategoryContract>();
                List<UniversalCategory> uniCatDetails = BALUtils.GetUniversalMappingDataRepoInstance(0).GetUniversalCategories();
                uniCatDetails.ForEach(x =>
                {
                    UniversalCategoryContract uniCatContract = new UniversalCategoryContract();
                    uniCatContract.UniversalCategoryName = x.UC_Name;
                    uniCatContract.UniversalCategoryID = x.UC_ID;
                    result.Add(uniCatContract);
                });
                return result;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static UniversalCategoryContract GetUniversalCategoryByReqCatID(Int32 ReqCatID)
        {
            try
            {
                UniversalCategoryContract result = new UniversalCategoryContract();
                UniversalRequirementCategoryMapping URCM = BALUtils.GetUniversalMappingDataRepoInstance(0).GetUniversalCategoryByReqCatID(ReqCatID);
                if (!URCM.IsNullOrEmpty())
                {
                    result.UniversalCategoryID = URCM.URCM_UniversalCategoryID;
                    result.UniversalCategoryName = URCM.UniversalCategory.UC_Name;
                    result.UniCatReqCatMappingID = URCM.URCM_ID;
                }
                return result;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean DeleteUnversalCategoryMappings(Int32 UniversalReqCatMappingID, Int32 currentLoggedInUserID)
        {
            try
            {
                UniversalRotationMappingViewContract uniRotationContrcat = new UniversalRotationMappingViewContract();

                if (UniversalReqCatMappingID > AppConsts.NONE)
                {
                    uniRotationContrcat.UniversalReqCatMappingID = UniversalReqCatMappingID;
                    return BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).SaveUniversalRequirmentCategoryMappingData(uniRotationContrcat, currentLoggedInUserID);
                }
                return true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static List<UniversalItemContract> GetUniversalItemsByUniReqCatID(Int32 ReqCatID)
        {
            try
            {
                List<UniversalItemContract> result = new List<UniversalItemContract>();
                UniversalRequirementCategoryMapping URCM = BALUtils.GetUniversalMappingDataRepoInstance(0).GetUniversalCategoryByReqCatID(ReqCatID);
                if (!URCM.IsNullOrEmpty())
                {
                    List<UniversalCategoryItemMapping> uniItemDetails = BALUtils.GetUniversalMappingDataRepoInstance(0).GetUniversalItemsByCategoryID(URCM.URCM_UniversalCategoryID);
                    uniItemDetails.ForEach(x =>
                    {
                        UniversalItemContract uniItmContract = new UniversalItemContract();
                        uniItmContract.UniversalItemName = x.UniversalItem.UI_Name;
                        uniItmContract.UniversalItemID = x.UCIM_UniversalItemID;
                        uniItmContract.UniReqCatMappingID = URCM.URCM_ID;
                        uniItmContract.UniCatItmMappingID = x.UCIM_ID;
                        uniItmContract.UniCatItmMappingID = x.UCIM_ID;
                        result.Add(uniItmContract);
                    });
                }
                return result;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static UniversalItemContract GetUniversalItemsByUniReqCatItmID(Int32 ReqCatID, Int32 ReqItmID)
        {
            try
            {
                UniversalItemContract result = new UniversalItemContract();
                UniversalRequirementCategoryMapping URCM = BALUtils.GetUniversalMappingDataRepoInstance(0).GetUniversalCategoryByReqCatID(ReqCatID);
                if (!URCM.IsNullOrEmpty())
                {
                    UniversalRequirementItemMapping URIM = BALUtils.GetUniversalMappingDataRepoInstance(0).GetUniversalItemsByUniReqCatItmID(URCM.URCM_ID, ReqItmID, ReqCatID);
                    if (!URIM.IsNullOrEmpty())
                    {
                        result.UniversalItemID = URIM.UniversalCategoryItemMapping.UniversalItem.UI_ID;
                        result.UniCatItmMappingID = URIM.URIM_UniversalCategoryItemMappingID;
                        result.ReqCatIteMappingmID = URIM.URIM_RequirementCategoryItemMappingID;
                        result.UniReqItmMappingID = URIM.URIM_ID;
                    }
                    result.UniReqCatMappingID = URCM.URCM_ID;
                }
                return result;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static Boolean DeleteUniversalReqItmMapping(Int32 UniversalReqItmMappingID, Int32 currentLoggedInUserID)
        {
            try
            {
                UniversalRotationMappingViewContract uniRotationContrcat = new UniversalRotationMappingViewContract();

                if (UniversalReqItmMappingID > AppConsts.NONE)
                {
                    uniRotationContrcat.UniversalReqItemMappingID = UniversalReqItmMappingID;
                    return BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).SaveUniversalRequirmentItemMappingData(uniRotationContrcat, currentLoggedInUserID);
                }
                return true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static List<UniversalAttributeContract> GetUniversalAttributes(Int32 ReqItmID, Int32 ReqCatID = 0)
        {
            try
            {
                List<UniversalAttributeContract> lstUniversalAttrContract = new List<UniversalAttributeContract>();
                List<UniversalField> UI = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).GetUniversalAttributeField();

                if (!UI.IsNullOrEmpty())
                {
                    //List<UniversalItemAttributeMapping> lstUA = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).GetUniversalAttributesByItemID(UI.UI_ID);
                    UI.ForEach(x =>
                    {
                        UniversalAttributeContract UAC = new UniversalAttributeContract();
                        UAC.UniversalAttributeID = x.UF_ID;
                        UAC.UniversalAttrDataTypeCode = x.lkpUniversalAttributeDataType.LUADT_Code;
                        UAC.UniversalAttributeName = x.UF_Name;
                        //UAC.UniItmAttrMappingID = x.UIAM_ID;
                        //UAC.UniReqItemMappingID = UI.UniversalCategoryItemMappings.FirstOrDefault().UniversalRequirementItemMappings.FirstOrDefault().URIM_ID;
                        lstUniversalAttrContract.Add(UAC);
                    });
                }
                return lstUniversalAttrContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        //public static List<UniversalAttributeContract> GetUniversalAttributes(Int32 ReqItmID, Int32 ReqCatID = 0)
        //{
        //    try
        //    {
        //        List<UniversalAttributeContract> lstUniversalAttrContract = new List<UniversalAttributeContract>();
        //        UniversalItem UI = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).GetUniversalItemDetailsByReqItemID(ReqItmID, ReqCatID);
        //        if (!UI.IsNullOrEmpty())
        //        {
        //            List<UniversalItemAttributeMapping> lstUA = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).GetUniversalAttributesByItemID(UI.UI_ID);
        //            lstUA.ForEach(x =>
        //            {
        //                UniversalAttributeContract UAC = new UniversalAttributeContract();
        //                UAC.UniversalAttributeID = x.UniversalAttribute.UA_ID;
        //                UAC.UniversalAttrDataTypeCode = x.UniversalAttribute.lkpUniversalAttributeDataType.LUADT_Code;
        //                UAC.UniversalAttributeName = x.UniversalAttribute.UA_Name;
        //                UAC.UniItmAttrMappingID = x.UIAM_ID;
        //                UAC.UniReqItemMappingID = UI.UniversalCategoryItemMappings.FirstOrDefault().UniversalRequirementItemMappings.FirstOrDefault().URIM_ID;
        //                lstUniversalAttrContract.Add(UAC);
        //            });
        //        }
        //        return lstUniversalAttrContract;
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //}
        public static UniversalAttributeContract GetUniversalFieldAttributeDetails(Int32 ReqItmID, Int32 ReqFieldID, Int32 ReqCatID = 0)
        {
            try
            {
                UniversalAttributeContract UAC = new UniversalAttributeContract();
                Entity.SharedDataEntity.UniversalFieldMapping uniFieldmapping = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).GetUniversalAttributeMappingByReqCatItemFieldID(ReqItmID, ReqFieldID, ReqCatID);

                if (!uniFieldmapping.IsNullOrEmpty())
                {
                    UAC.UniversalFieldMappingID = uniFieldmapping.UFM_ID;
                    UAC.UniversalFieldID = uniFieldmapping.UFM_UniversalFieldID;
                }
                return UAC;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static UniversalAttributeContract GetUniversalattributeDetails(Int32 ReqItmID, Int32 ReqFieldID, Int32 ReqCatID = 0)
        {
            try
            {

                UniversalAttributeContract UAC = new UniversalAttributeContract();
                UniversalItem UI = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).GetUniversalItemDetailsByReqItemID(ReqItmID, ReqCatID);
                if (!UI.IsNullOrEmpty())
                {
                    Int32 UniReqItemMappingID = UI.UniversalCategoryItemMappings.FirstOrDefault().UniversalRequirementItemMappings.FirstOrDefault().URIM_ID;
                    if (ReqFieldID > AppConsts.NONE)
                    {
                        Int32 ReqItmFieldID = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).GetRequirementItmFieldIDByReqFldID(ReqFieldID, ReqItmID);
                        UniversalRequirementAttributeMapping URAM = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).GetUniversalattributeDetailsByItmFieldMappingID(UniReqItemMappingID, ReqItmFieldID);
                        if (!URAM.IsNullOrEmpty())
                        {
                            UAC.UniversalAttributeID = URAM.UniversalItemAttributeMapping.UniversalAttribute.UA_ID;
                            UAC.UniversalAttrDataTypeCode = URAM.UniversalItemAttributeMapping.UniversalAttribute.lkpUniversalAttributeDataType.LUADT_Code;
                            UAC.UniversalAttributeName = URAM.UniversalItemAttributeMapping.UniversalAttribute.UA_Name;
                            UAC.UniItmAttrMappingID = URAM.UniversalItemAttributeMapping.UIAM_ID;
                            UAC.UniReqAttrMappingID = URAM.URAM_ID;
                            UAC.ReqItmFldMappingID = URAM.URAM_RequirementItemFieldMappingID;
                        }
                    }
                    UAC.UniReqItemMappingID = UniReqItemMappingID;
                }
                return UAC;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static List<InputTypeComplianceAttributeServiceContract> GetAtrInputPriorityByID(Int32 uniReqAtrMappingID)
        {
            try
            {
                List<InputTypeComplianceAttributeServiceContract> result = new List<InputTypeComplianceAttributeServiceContract>();
                List<UniversalRequirementAttributeInputTypeMapping> URAIP = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).GetAtrInputPriorityByID(uniReqAtrMappingID);
                URAIP.ForEach(x =>
                {
                    InputTypeComplianceAttributeServiceContract InputTypeDat = new InputTypeComplianceAttributeServiceContract();
                    InputTypeDat.InputPriority = x.URAITM_InputPriority.HasValue ? x.URAITM_InputPriority.Value : 1;
                    //InputTypeDat.ID = x.URAITM_UniversalItemAttributeMappingID.HasValue ? x.URAITM_UniversalItemAttributeMappingID.Value : 0;
                    InputTypeDat.ID = x.URAITM_UniversalItemAttributeMappingID;
                    InputTypeDat.Name = x.UniversalItemAttributeMapping.UniversalAttribute.UA_Name;
                    result.Add(InputTypeDat);
                });
                return result;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static List<InputTypeComplianceAttributeServiceContract> GetUniversalFieldAtrInputPriorityByID(Int32 uniFieldMappingID)
        {
            try
            {
                List<InputTypeComplianceAttributeServiceContract> result = new List<InputTypeComplianceAttributeServiceContract>();
                List<Entity.SharedDataEntity.UniversalFieldInputTypeMapping> URAIP = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).GetUniversalAtrInputPriorityByID(uniFieldMappingID);
                URAIP.ForEach(x =>
                {
                    InputTypeComplianceAttributeServiceContract InputTypeDat = new InputTypeComplianceAttributeServiceContract();
                    InputTypeDat.InputPriority = x.UFITM_InputPriority.HasValue ? x.UFITM_InputPriority.Value : 1;
                    //InputTypeDat.ID = x.URAITM_UniversalItemAttributeMappingID.HasValue ? x.URAITM_UniversalItemAttributeMappingID.Value : 0;
                    InputTypeDat.ID = x.UFITM_UniversalFieldID;
                    InputTypeDat.Name = x.UniversalField.UF_Name;
                    result.Add(InputTypeDat);
                });
                return result;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static Boolean SaveUpdateAttributeInputPriority(UniversalAttributeContract UniAtrContract, Int32 currentLoggedInUser)
        {
            try
            {
                List<UniversalRequirementAttributeInputTypeMapping> DataToSave = new List<UniversalRequirementAttributeInputTypeMapping>();
                List<InputTypeComplianceAttributeServiceContract> NewData = UniAtrContract.lstAttributeInputData;
                if (!NewData.IsNullOrEmpty())
                {
                    Int32 uniReqAtrMappingID = 0;
                    if (UniAtrContract.UniReqAttrMappingID > AppConsts.NONE)
                    {
                        uniReqAtrMappingID = UniAtrContract.UniReqAttrMappingID;
                    }
                    else
                    {
                        uniReqAtrMappingID = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE)
                                                .GetUniversalReqAtrMappingID(UniAtrContract.UniReqItemMappingID, UniAtrContract.UniItmAttrMappingID);
                    }

                    List<UniversalRequirementAttributeInputTypeMapping> lstUniReqAtrInput = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE)
                                                                                                            .GetAtrInputPriorityByID(uniReqAtrMappingID);
                    if (!lstUniReqAtrInput.IsNullOrEmpty())
                    {
                        lstUniReqAtrInput.ForEach(x =>
                        {
                            x.URAITM_IsDeleted = true;
                            x.URAITM_ModifiedBy = currentLoggedInUser;
                            x.URAITM_ModifiedOn = DateTime.Now;
                        });
                    }

                    NewData.ForEach(x =>
                    {
                        UniversalRequirementAttributeInputTypeMapping URAIP = new UniversalRequirementAttributeInputTypeMapping();
                        URAIP.URAITM_IsDeleted = false;
                        URAIP.URAITM_CreatedBy = currentLoggedInUser;
                        URAIP.URAITM_CreatedOn = DateTime.Now;
                        URAIP.URAITM_InputPriority = x.InputPriority;
                        URAIP.URAITM_UniversalItemAttributeMappingID = x.ID;
                        URAIP.URAITM_UniversalReqAttributeMappingID = uniReqAtrMappingID;
                        DataToSave.Add(URAIP);
                    });

                    BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).SaveChangesAttributeInputPriorty(DataToSave);
                }
                return true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        #endregion

        #region Universal Compliance Mapping View
        public static List<UniversalComplianceMappingViewContract> GetUniversalComplianceMappingView(Int32 tenantID, Int32 compliancePkgID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantID).GetUniversalComplianceMappingView(compliancePkgID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        //public static List<InputTypeComplianceAttributeContract> GetUniversalComplianceAttributeInputTypeMapping(Int32 tenantID, Int32 universalAttrMappingID)
        //{
        //    try
        //    {
        //        List<InputTypeComplianceAttributeContract> lstInputMappings = new List<InputTypeComplianceAttributeContract>();
        //        List<UniversalAttributeInputTypeMapping> lstTmpInputMappings = BALUtils.GetUniversalMappingDataRepoInstance(tenantID).GetUniversalAttributeInputTypeMapping(universalAttrMappingID);

        //        foreach (UniversalAttributeInputTypeMapping item in lstTmpInputMappings)
        //        {
        //            InputTypeComplianceAttributeContract inputRotAttrMapping = new InputTypeComplianceAttributeContract();
        //            inputRotAttrMapping.ID = item.UAITM_UniversalItemAttributeMappingID;
        //            inputRotAttrMapping.InputPriority = item.UAITM_InputPriority.HasValue ? item.UAITM_InputPriority.Value : AppConsts.NONE;
        //            lstInputMappings.Add(inputRotAttrMapping);
        //        }

        //        return lstInputMappings;
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //}
        public static List<InputTypeComplianceAttributeContract> GetUniversalComplianceAttributeInputTypeMapping(Int32 tenantID, Int32 UniversalFieldMappingID)
        {
            try
            {
                List<InputTypeComplianceAttributeContract> lstInputMappings = new List<InputTypeComplianceAttributeContract>();
                List<Entity.ClientEntity.UniversalFieldInputTypeMapping> lstTmpInputMappings = BALUtils.GetUniversalMappingDataRepoInstance(tenantID).GetUniversalAttributeInputTypeMapping(UniversalFieldMappingID);

                foreach (Entity.ClientEntity.UniversalFieldInputTypeMapping item in lstTmpInputMappings)
                {
                    InputTypeComplianceAttributeContract inputRotAttrMapping = new InputTypeComplianceAttributeContract();
                    //inputRotAttrMapping.ID = item.URAITM_UniversalItemAttributeMappingID.Value;
                    inputRotAttrMapping.ID = item.UFITM_UniversalFieldID;
                    inputRotAttrMapping.InputPriority = item.UFITM_InputPriority.HasValue ? item.UFITM_InputPriority.Value : AppConsts.ONE;
                    lstInputMappings.Add(inputRotAttrMapping);
                }

                return lstInputMappings;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        #endregion


        public static Boolean SaveUpdateAttributeMappingWithUniversalAttributeFromView(int tenantId, Entity.ClientEntity.UniversalFieldMapping uamData, Int32 loggedInUserID)
        {
            try
            {
                Int32 universalMappingTypeID = LookupManager.GetLookUpData<lkpUniversalMappingType>(tenantId).FirstOrDefault(x => x.LUMT_Code == UniversalMappingTypeEnum.COMPLIANCE_TYPE.GetStringValue()).LUMT_ID;
                uamData.UFM_UniversalMappingTypeID = universalMappingTypeID;
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).SaveUpdateAttributeMappingWithUniversalAttributeFromView(uamData, loggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #region UAT-2402
        public static Dictionary<Int32, String> GetUniversalAtrOptionData(Int32 uniItmAtrMappingID)
        {
            try
            {
                Dictionary<Int32, String> optionData = new Dictionary<Int32, String>();
                List<UniversalAttributeOption> lstUniAtrOption = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).GetUniversalAtrOptionData(uniItmAtrMappingID);
                lstUniAtrOption.ForEach(x =>
                {
                    optionData.Add(x.UAO_ID, x.UAO_OptionText);
                });
                return optionData;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Dictionary<Int32, String> GetUniversalFieldAtrOptionData(Int32 uniItmAtrMappingID)
        {
            try
            {
                Dictionary<Int32, String> optionData = new Dictionary<Int32, String>();
                List<UniversalFieldOption> lstUniAtrOption = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).GetUniversalFieldAtrOptionData(uniItmAtrMappingID);
                lstUniAtrOption.ForEach(x =>
                {
                    optionData.Add(x.UFO_ID, x.UFO_OptionText);
                });
                return optionData;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static List<Int32> GetUniversalAtrOptionSelected(Int32 reqFieldOptionID, Int32 uniReqAtrMappingID)
        {
            try
            {
                List<UniversalRequirementAttributeOptionMapping> uniReqAtrOptionMapping = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).GetUniversalAtrOptionSelected(reqFieldOptionID, uniReqAtrMappingID);
                if (!uniReqAtrOptionMapping.IsNullOrEmpty())
                {
                    return uniReqAtrOptionMapping.Select(sel => sel.URAOM_UniversalAttributeOptionID).ToList();
                }
                return new List<Int32> { AppConsts.NONE };
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static List<Int32> GetUniversalFieldAtrOptionSelected(Int32 uniFieldOptionID, Int32 uniFieldMappingID)
        {
            try
            {
                List<Entity.SharedDataEntity.UniversalFieldOptionMapping> uniReqAtrOptionMapping = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).GetUniversalFieldAtrOptionSelected(uniFieldOptionID, uniFieldMappingID);
                if (!uniReqAtrOptionMapping.IsNullOrEmpty())
                {
                    return uniReqAtrOptionMapping.Select(sel => sel.UniversalFieldOption.UFO_ID).ToList();
                }
                return new List<Int32> { AppConsts.NONE };
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static List<UniversalAttributeOptionMapping> GetUniversalAttributeOptionMapping(Int32 tenantId, Int32 uniAttributeMappingId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).GetUniversalAttributeOptionMapping(uniAttributeMappingId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        #endregion

        #region UAT-2402

        public static List<UniversalAttributeOption> GetUniversalAttributeOptionsByID(Int32 universalItemAttrMappingID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.ONE).GetUniversalAttributeOptionsByID(universalItemAttrMappingID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static List<Entity.SharedDataEntity.UniversalFieldOption> GetUniversalFieldeOptionsByID(Int32 universalFieldID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.ONE).GetUniversalFieldeOptionsByID(universalFieldID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean SaveUniverAttrOptionMapping(Int32 tenantId, List<UniversalAttributeOptionMapping> lstUniAttrOptMapping, Int32 uniAttributeMapId
                                                          , Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).SaveUniverAttrOptionMapping(lstUniAttrOptMapping, uniAttributeMapId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean AddApprovedItemsToCopyDataQueue(Int32 tenantId, Int32 complianceItemId, Int32 complianceCategoryId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).AddApprovedItemsToCopyDataQueue(complianceItemId, complianceCategoryId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        //UAT-3716
        public static Boolean AddApprovedPkgsToCopyDataQueue(Int32 tenantId, Int32 CurrentPackageId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).AddApprovedPkgsToCopyDataQueue(CurrentPackageId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        //END UAT-3716

        public static List<UniversalAttributeMapping> GetMappedUniversalAttributesByItemMappingID(Int32 tenantId, Int32 uniItemMapId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).GetMappedUniversalAttributesByItemMappingID(uniItemMapId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        #endregion

        #region Category Copy
        public static Boolean CopyUniversalDataByCategoryIds(Int32 currentAddedCategoryID, Int32 requirementCatyegoryID, Int32 currentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).CopyUniversalDataByCategoryIds(currentAddedCategoryID, requirementCatyegoryID, currentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        #endregion

        #region UAT 2985
        public static List<UniversalField> GetUniversalAttributeField(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).GetUniversalAttributeField();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static Boolean DeleteUniversalFieldByID(Int32 tenantId, Int32 uniFieldID, Int32 cuurentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).DeleteUniversalFieldByID(uniFieldID, cuurentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static Boolean SaveUpdateUniversalField(Int32 tenantId, UniversalField uamData, Int32 cuurentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).SaveUpdateUniversalField(uamData, cuurentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static UniversalField GetUniversalFieldById(Int32 tenantId, Int32 uniFieldId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).GetUniversalFieldById(uniFieldId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        #endregion

        public static Boolean UpdateFieldInputMapping(Int32 tenantId, Int32 uniFieldMappingId, List<Entity.ClientEntity.UniversalFieldInputTypeMapping> lstInputMapping, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).UpdateFieldInputMapping(uniFieldMappingId, lstInputMapping, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Entity.ClientEntity.UniversalFieldMapping GetComplianceTypeUniversalFieldMapping(Int32 tenantId, Int32 complianceCategoryItemID, Int32 complianceItemAttributeID, String mappingTypeCode)
        {
            try
            {
                Int32 universalMappingTypeID = LookupManager.GetLookUpData<lkpUniversalMappingType>(tenantId).FirstOrDefault(x => x.LUMT_Code == mappingTypeCode).LUMT_ID;
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).GetComplianceTypeUniversalFieldMapping(complianceCategoryItemID, complianceItemAttributeID, universalMappingTypeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Dictionary<Int32, String> GetUniversalFieldOptionData(Int32 universalFieldID)
        {
            try
            {
                Dictionary<Int32, String> optionData = new Dictionary<Int32, String>();
                List<UniversalFieldOption> lstUniFieldOption = BALUtils.GetUniversalMappingDataRepoInstance(AppConsts.NONE).GetUniversalFieldOptionData(universalFieldID);
                lstUniFieldOption.ForEach(x =>
                {
                    optionData.Add(x.UFO_ID, x.UFO_OptionText);
                });
                return optionData;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean SaveUpdateUniversalFieldMapping(Int32 tenantId, Entity.ClientEntity.UniversalFieldMapping ufmData, string mappingTypeCode)
        {
            try
            {
                Int32 universalMappingTypeID = LookupManager.GetLookUpData<lkpUniversalMappingType>(tenantId).FirstOrDefault(x => x.LUMT_Code == mappingTypeCode).LUMT_ID;
                ufmData.UFM_UniversalMappingTypeID = universalMappingTypeID;
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).SaveUpdateUniversalFieldMapping(ufmData);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean DeleteUniversalFieldMapping(Int32 tenantId, Int32 universalFieldMappingID, Int32 currentLoggedInuserId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).DeleteUniversalFieldMapping(universalFieldMappingID, currentLoggedInuserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean SaveUniversalFieldOptionMapping(Int32 tenantId, List<Entity.ClientEntity.UniversalFieldOptionMapping> lstUniFieldOptMapping, Int32 uniFieldMappingId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).SaveUniversalFieldOptionMapping(lstUniFieldOptMapping, uniFieldMappingId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean IsAnyAttributeMappingExists(Int32 tenantId, Int32 categoryItemMappingID)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).IsAnyAttributeMappingExists(categoryItemMappingID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        //UAT-2991
        private static String ConvertPackageObjectTypeInXML(List<RequirementPackageObjectSynchingContract> lstObjects)
        {
            StringBuilder sb = new StringBuilder();
            foreach (RequirementPackageObjectSynchingContract itm in lstObjects)
            {
                sb.Append("<RequestData>");
                sb.Append("<ObjectId>" + itm.NewObjectId + "</ObjectId>");
                sb.Append("<ObjectTypeCode>" + itm.ObjectTypeCode + "</ObjectTypeCode>");
                sb.Append("<OldObjectId>" + itm.OldObjectId + "</OldObjectId>");
                sb.Append("<ActionTypeCode>" + itm.ActionTypeCode + "</ActionTypeCode>");
                sb.Append("</RequestData>");
            }
            return sb.ToString();
        }

        public static Boolean IsUniversalFieldNameExists(Int32 tenantId, String universalFieldName)
        {
            try
            {
                return BALUtils.GetUniversalMappingDataRepoInstance(tenantId).IsUniversalFieldNameExists(universalFieldName);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
    }
}
