using DAL.Interfaces;
using Entity.SharedDataEntity;
using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using INTSOF.Utils;
using INTSOF.UI.Contract.ClinicalRotation;




namespace DAL.Repository
{
    public class UniversalMappingDataRepository : ClientBaseRepository, IUniversalMappingDataRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _dbContext;

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public UniversalMappingDataRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }

        List<UniversalCategory> IUniversalMappingDataRepository.GetUniversalCategories()
        {
            return base.SharedDataDBContext.UniversalCategories.Where(cond => !cond.UC_IsDeleted).ToList();
        }

        List<UniversalCategoryItemMapping> IUniversalMappingDataRepository.GetUniversalItemsByCategoryID(Int32 universalCatgeoryId)
        {
            return base.SharedDataDBContext.UniversalCategoryItemMappings.Where(cond => cond.UCIM_UniversalCategoryID == universalCatgeoryId && !cond.UCIM_IsDeleted).ToList();
        }

        List<UniversalItemAttributeMapping> IUniversalMappingDataRepository.GetUniversalAttributesByItemID(Int32 universalItemId)
        {
            return base.SharedDataDBContext.UniversalItemAttributeMappings.Where(cond => cond.UIAM_UniversalItemID == universalItemId && !cond.UIAM_IsDeleted).ToList();
        }

        List<UniversalField> IUniversalMappingDataRepository.GetUniversalFieldByAttributeDataTypeID(Int32 attributeDataTypeIID)
        {
            return base.SharedDataDBContext.UniversalFields.Where(cond => cond.UF_AttributeDataTypeID == attributeDataTypeIID && !cond.UF_IsDeleted).ToList();
        }


        #region Universal Mapping Setup Screen
        DataTable IUniversalMappingDataRepository.GetUniversalMappingTreeData()
        {
            EntityConnection connection = base.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[dbo].[usp_GetUniversalMappingDataTree]", con);
                command.CommandType = CommandType.StoredProcedure;
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

        Boolean IUniversalMappingDataRepository.SaveUpdateUniversalcategoryData(UniversalCategory uniCatData)
        {
            if (uniCatData.UC_ID == AppConsts.NONE)
            {
                base.SharedDataDBContext.UniversalCategories.AddObject(uniCatData);
            }
            if (base.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IUniversalMappingDataRepository.DeleteUniversalCategorydata(Int32 UniCatID, Int32 currentLoggedInUserID)
        {
            UniversalCategory uniCatData = base.SharedDataDBContext.UniversalCategories.Where(cond => !cond.UC_IsDeleted && cond.UC_ID == UniCatID).FirstOrDefault();

            if (!uniCatData.IsNullOrEmpty())
            {
                uniCatData.UC_IsDeleted = true;
                uniCatData.UC_ModifiedOn = DateTime.Now;
                uniCatData.UC_ModifiedBy = currentLoggedInUserID;
            }

            if (base.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        UniversalCategory IUniversalMappingDataRepository.GetUniversalCategoryByID(Int32 CategoryID)
        {
            return base.SharedDataDBContext.UniversalCategories.Where(cond => cond.UC_ID == CategoryID && !cond.UC_IsDeleted).FirstOrDefault();
        }

        Boolean IUniversalMappingDataRepository.DeleteUniversalItemByID(Int32 uniItemID, Int32 uniCatID, Int32 currentLoggedInUserID)
        {
            UniversalCategoryItemMapping UCIM = base.SharedDataDBContext.UniversalCategoryItemMappings.Where(cond => !cond.UCIM_IsDeleted && !cond.UniversalItem.UI_IsDeleted
                                                               && cond.UCIM_UniversalItemID == uniItemID && cond.UCIM_UniversalCategoryID == uniCatID).FirstOrDefault();

            if (!UCIM.IsNullOrEmpty())
            {
                UCIM.UCIM_IsDeleted = true;
                UCIM.UCIM_ModifiedBy = currentLoggedInUserID;
                UCIM.UCIM_ModifiedOn = DateTime.Now;
            }

            UniversalItem UI = base.SharedDataDBContext.UniversalItems.Where(cond => !cond.UI_IsDeleted && cond.UI_ID == uniItemID).FirstOrDefault();

            if (!UI.IsNullOrEmpty())
            {
                UI.UI_IsDeleted = true;
                UI.UI_ModifiedBy = currentLoggedInUserID;
                UI.UI_ModifiedOn = DateTime.Now;
            }

            if (base.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        Boolean IUniversalMappingDataRepository.SaveUpdateUniverItem(UniversalItem uniItemData)
        {
            if (uniItemData.UI_ID == AppConsts.NONE)
            {
                base.SharedDataDBContext.UniversalItems.AddObject(uniItemData);
            }

            if (base.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IUniversalMappingDataRepository.DeleteUniversalAttributeByID(Int32 uniItemID, Int32 uniAttributeID, Int32 cuurentLoggedInUserID)
        {
            UniversalItemAttributeMapping UIAM = base.SharedDataDBContext.UniversalItemAttributeMappings.Where(cond => !cond.UIAM_IsDeleted && !cond.UniversalAttribute.UA_IsDeleted
                                                                             && cond.UIAM_UniversalAttributeID == uniAttributeID && cond.UIAM_UniversalItemID == uniItemID).FirstOrDefault();

            if (!UIAM.IsNullOrEmpty())
            {
                UIAM.UIAM_IsDeleted = true;
                UIAM.UIAM_ModifiedBy = cuurentLoggedInUserID;
                UIAM.UIAM_ModifiedOn = DateTime.Now;
            }

            UniversalAttribute UA = base.SharedDataDBContext.UniversalAttributes.Where(cond => !cond.UA_IsDeleted && cond.UA_ID == uniAttributeID).FirstOrDefault();

            if (!UA.IsNullOrEmpty())
            {
                UA.UA_IsDeleted = true;
                UA.UA_ModifiedBy = cuurentLoggedInUserID;
                UA.UA_ModifiedOn = DateTime.Now;
            }

            if (base.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        UniversalItem IUniversalMappingDataRepository.GetUniversalItemDetailByID(Int32 uniCatID, Int32 uniItemID)
        {
            return base.SharedDataDBContext.UniversalCategoryItemMappings.Where(cond => !cond.UCIM_IsDeleted && !cond.UniversalItem.UI_IsDeleted
                                            && cond.UCIM_UniversalItemID == uniItemID && cond.UCIM_UniversalCategoryID == uniCatID)
                                            .Select(sel => sel.UniversalItem).FirstOrDefault();
        }

        Boolean IUniversalMappingDataRepository.SaveUpdateAttributeDetails(UniversalAttribute uniAttributeDetails)
        {
            if (uniAttributeDetails.UA_ID == AppConsts.NONE)
            {
                base.SharedDataDBContext.UniversalAttributes.AddObject(uniAttributeDetails);
            }
            if (base.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        UniversalAttribute IUniversalMappingDataRepository.GetUniversalAttributeDataByID(Int32 uniItemID, Int32 uniAttributeID)
        {
            return base.SharedDataDBContext.UniversalItemAttributeMappings.Where(cond => cond.UIAM_UniversalAttributeID == uniAttributeID
                                                                    && cond.UIAM_UniversalItemID == uniItemID && !cond.UIAM_IsDeleted
                                                                    && !cond.UniversalAttribute.UA_IsDeleted).Select(sel => sel.UniversalAttribute).FirstOrDefault();
        }

        Boolean IUniversalMappingDataRepository.IsValidAttributeName(String AttributeName)
        {
            UniversalAttribute UA = base.SharedDataDBContext.UniversalAttributes.Where(cond => !cond.UA_IsDeleted && cond.UA_Name == AttributeName).FirstOrDefault();
            if (!UA.IsNullOrEmpty())
                return true;
            return false;
        }

        Boolean IUniversalMappingDataRepository.IsValidCategoryName(String CategoryName)
        {
            UniversalCategory UC = base.SharedDataDBContext.UniversalCategories.Where(cond => !cond.UC_IsDeleted && cond.UC_Name == CategoryName).FirstOrDefault();
            if (!UC.IsNullOrEmpty())
                return true;
            return false;
        }

        Boolean IUniversalMappingDataRepository.IsValidItemName(String ItemName)
        {
            UniversalItem UI = base.SharedDataDBContext.UniversalItems.Where(cond => !cond.UI_IsDeleted && cond.UI_Name == ItemName).FirstOrDefault();
            if (!UI.IsNullOrEmpty())
                return true;
            return false;
        }

        #endregion

        #region compliance mapping with universal mapping regarding UAT-2305

        Boolean IUniversalMappingDataRepository.SaveUpdateCategoryMappingWithUniversalCat(UniversalCategoryMapping ucmData)
        {
            if (!ucmData.IsNullOrEmpty() && ucmData.UCM_ID > AppConsts.NONE)
            {
                UniversalCategoryMapping ucmDataInDB = _dbContext.UniversalCategoryMappings.FirstOrDefault(cond => cond.UCM_ID == ucmData.UCM_ID && !cond.UCM_IsDeleted);
                if (!ucmDataInDB.IsNullOrEmpty())
                {
                    ucmDataInDB.UCM_CategoryID = ucmData.UCM_CategoryID;
                    ucmDataInDB.UCM_UniversalCategoryID = ucmData.UCM_UniversalCategoryID;
                    ucmDataInDB.UCM_ModifiedBy = ucmData.UCM_ModifiedBy;
                    ucmDataInDB.UCM_ModifiedOn = ucmData.UCM_ModifiedOn;
                    ucmDataInDB.UCM_UniversalMappingTypeID = ucmData.UCM_UniversalMappingTypeID;
                }
            }
            else
            {
                _dbContext.UniversalCategoryMappings.AddObject(ucmData);
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        UniversalCategoryMapping IUniversalMappingDataRepository.GetMappedUniversalCategoryDataByID(Int32 categoryId, Int32 mappingTypeId)
        {
            return _dbContext.UniversalCategoryMappings.FirstOrDefault(cond => cond.UCM_CategoryID == categoryId && !cond.UCM_IsDeleted && cond.UCM_UniversalMappingTypeID == mappingTypeId);
        }

        Boolean IUniversalMappingDataRepository.DeleteCategoryMappingWithUniversalCategory(Int32 categoryMappingID, Int32 mappingTypeID, Int32 currentLoggedInuserId)
        {
            UniversalCategoryMapping uniCategoryMapping = _dbContext.UniversalCategoryMappings.FirstOrDefault(x => x.UCM_ID == categoryMappingID
                                                                        && x.UCM_UniversalMappingTypeID == mappingTypeID && !x.UCM_IsDeleted);

            if (!uniCategoryMapping.IsNullOrEmpty())
            {
                uniCategoryMapping.UCM_IsDeleted = true;
                uniCategoryMapping.UCM_ModifiedBy = currentLoggedInuserId;
                uniCategoryMapping.UCM_ModifiedOn = DateTime.Now;
                DeleteItemMappingWithUniversalItem(uniCategoryMapping.UCM_ID, currentLoggedInuserId);
            }

            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        private void DeleteItemMappingWithUniversalItem(Int32 universalCategoryMappingID, Int32 currentLoggedInuserId)
        {
            List<UniversalItemMapping> lstItemMapping = _dbContext.UniversalItemMappings.Where(cnd => cnd.UIM_UniversalCategoryMappingID == universalCategoryMappingID
                                                        && !cnd.UIM_IsDeleted).ToList();

            if (!lstItemMapping.IsNullOrEmpty())
            {
                List<Int32> lstMappedItemIDs = new List<Int32>();
                lstMappedItemIDs = lstItemMapping.Select(x => x.UIM_ID).ToList();
                DeleteAttributeMappingWithUniversalAttribute(lstMappedItemIDs, currentLoggedInuserId);
                lstItemMapping.ForEach(attr =>
                {
                    attr.UIM_IsDeleted = true;
                    attr.UIM_ModifiedBy = currentLoggedInuserId;
                    attr.UIM_ModifiedOn = DateTime.Now;
                });
            }
        }

        private void DeleteAttributeMappingWithUniversalAttribute(List<Int32> universalItemMappingIDs, Int32 currentLoggedInuserId)
        {
            List<UniversalAttributeMapping> lstAttributeMapping = _dbContext.UniversalAttributeMappings.Where(cnd => universalItemMappingIDs.Contains(cnd.UAM_UniversalItemMappingID)
                                                                                               && !cnd.UAM_IsDeleted).ToList();

            if (!lstAttributeMapping.IsNullOrEmpty())
            {
                List<Int32> lstMappedAttributeIDs = new List<Int32>();
                lstMappedAttributeIDs = lstAttributeMapping.Select(x => x.UAM_ID).ToList();
                lstAttributeMapping.ForEach(attr =>
                {
                    attr.UAM_IsDeleted = true;
                    attr.UAM_ModifiedOn = DateTime.Now;
                    attr.UAM_ModifiedBy = currentLoggedInuserId;
                });
                DeleteAttributeInputMapping(lstMappedAttributeIDs, currentLoggedInuserId);
                //UAT-2402:
                DeleteAttributeOptionMapping(lstMappedAttributeIDs, currentLoggedInuserId);
            }
        }

        Boolean IUniversalMappingDataRepository.SaveUpdateItemMappingWithUniversalItem(UniversalItemMapping uimData)
        {
            if (!uimData.IsNullOrEmpty() && uimData.UIM_ID > AppConsts.NONE)
            {
                UniversalItemMapping uimDataInDB = _dbContext.UniversalItemMappings.FirstOrDefault(cond => cond.UIM_ID == uimData.UIM_ID && !cond.UIM_IsDeleted);
                if (!uimDataInDB.IsNullOrEmpty())
                {
                    uimDataInDB.UIM_CategoryItemMappingID = uimData.UIM_CategoryItemMappingID;
                    uimDataInDB.UIM_UniversalCategoryItemMappingID = uimData.UIM_UniversalCategoryItemMappingID;
                    uimDataInDB.UIM_UniversalCategoryMappingID = uimData.UIM_UniversalCategoryMappingID;
                    uimDataInDB.UIM_ModifiedBy = uimData.UIM_ModifiedBy;
                    uimDataInDB.UIM_ModifiedOn = uimData.UIM_ModifiedOn;
                }
            }
            else
            {
                _dbContext.UniversalItemMappings.AddObject(uimData);
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        UniversalItemMapping IUniversalMappingDataRepository.GetMappedUniversalItemDataByID(Int32 uniCateMapId, Int32 catItemMappingId)
        {
            return _dbContext.UniversalItemMappings.FirstOrDefault(cond => cond.UIM_CategoryItemMappingID == catItemMappingId && cond.UIM_UniversalCategoryMappingID == uniCateMapId
                                                                   && !cond.UIM_IsDeleted);
        }

        Boolean IUniversalMappingDataRepository.DeleteItemMappingWithUniversalItem(Int32 itemMappingID, Int32 currentLoggedInuserId)
        {
            UniversalItemMapping itemMappingToDelete = _dbContext.UniversalItemMappings.FirstOrDefault(cnd => cnd.UIM_ID == itemMappingID && !cnd.UIM_IsDeleted);

            if (!itemMappingToDelete.IsNullOrEmpty())
            {
                List<Int32> lstMappedItemIDs = new List<Int32>();
                lstMappedItemIDs.Add(itemMappingToDelete.UIM_ID);
                itemMappingToDelete.UIM_IsDeleted = true;
                itemMappingToDelete.UIM_ModifiedBy = currentLoggedInuserId;
                itemMappingToDelete.UIM_ModifiedOn = DateTime.Now;
                DeleteAttributeMappingWithUniversalAttribute(lstMappedItemIDs, currentLoggedInuserId);
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IUniversalMappingDataRepository.SaveUpdateAttributeMappingWithUniversalAttribute(UniversalAttributeMapping uamData)
        {
            if (!uamData.IsNullOrEmpty() && uamData.UAM_ID > AppConsts.NONE)
            {
                UniversalAttributeMapping uamDataInDB = _dbContext.UniversalAttributeMappings.FirstOrDefault(cond => cond.UAM_ID == uamData.UAM_ID && !cond.UAM_IsDeleted);
                if (!uamDataInDB.IsNullOrEmpty())
                {
                    uamDataInDB.UAM_ItemAttributeMappingID = uamData.UAM_ItemAttributeMappingID;
                    uamDataInDB.UAM_UniversalItemAttributeMappingID = uamData.UAM_UniversalItemAttributeMappingID;
                    uamDataInDB.UAM_UniversalItemMappingID = uamData.UAM_UniversalItemMappingID;
                    uamDataInDB.UAM_ModifiedBy = uamData.UAM_ModifiedBy;
                    uamDataInDB.UAM_ModifiedOn = uamData.UAM_ModifiedOn;
                }
            }
            else
            {
                _dbContext.UniversalAttributeMappings.AddObject(uamData);
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        UniversalAttributeMapping IUniversalMappingDataRepository.GetMappedUniversalAttributeDataByID(Int32 uniItemMapId, Int32 itemAttrMappingId)
        {
            return _dbContext.UniversalAttributeMappings.FirstOrDefault(cond => cond.UAM_UniversalItemMappingID == uniItemMapId && cond.UAM_ItemAttributeMappingID == itemAttrMappingId
                                                                   && !cond.UAM_IsDeleted);
        }

        Boolean IUniversalMappingDataRepository.DeleteAttributeMappingWithUniversalAttribute(Int32 attrMappingID, Int32 currentLoggedInuserId)
        {
            UniversalAttributeMapping attributeMapping = _dbContext.UniversalAttributeMappings.FirstOrDefault(cnd => cnd.UAM_ID == attrMappingID && !cnd.UAM_IsDeleted);

            if (!attributeMapping.IsNullOrEmpty())
            {
                attributeMapping.UAM_IsDeleted = true;
                attributeMapping.UAM_ModifiedOn = DateTime.Now;
                attributeMapping.UAM_ModifiedBy = currentLoggedInuserId;
                List<Int32> lstMappedAttributeIDs = new List<Int32>();
                lstMappedAttributeIDs.Add(attributeMapping.UAM_ID);
                DeleteAttributeInputMapping(lstMappedAttributeIDs, currentLoggedInuserId);
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        UniversalCategoryItemMapping IUniversalMappingDataRepository.GetUniversalCatItemMappingData(Int32 UCIM_Id)
        {
            return base.SharedDataDBContext.UniversalCategoryItemMappings.FirstOrDefault(cond => cond.UCIM_ID == UCIM_Id && !cond.UCIM_IsDeleted);
        }

        List<UniversalAttributeInputTypeMapping> IUniversalMappingDataRepository.GetMappedUniversalInputAttributeDataByID(Int32 uniAttributeMapId)
        {
            return _dbContext.UniversalAttributeInputTypeMappings.Where(cond => cond.UAITM_UniversalAttributeMappingID == uniAttributeMapId && !cond.UAITM_IsDeleted).ToList();
        }

        Boolean IUniversalMappingDataRepository.UpdateAttributeInputMapping(Int32 uniAttributeMapId, List<UniversalAttributeInputTypeMapping> lstInputMapping, Int32 currentLoggedInUserId)
        {
            List<UniversalAttributeInputTypeMapping> lstAttributeInputMappingInDB = _dbContext.UniversalAttributeInputTypeMappings.Where(x =>
                                                                                      x.UAITM_UniversalAttributeMappingID == uniAttributeMapId && !x.UAITM_IsDeleted).ToList();
            if (!lstAttributeInputMappingInDB.IsNullOrEmpty())
            {
                List<Int32> inputMappingIDToMap = lstInputMapping.Select(slct => slct.UAITM_UniversalItemAttributeMappingID).ToList();
                var mappingToDelete = lstAttributeInputMappingInDB.Where(cnd => !inputMappingIDToMap.Contains(cnd.UAITM_UniversalItemAttributeMappingID)).ToList();
                lstInputMapping.ForEach(inputAttrType =>
                {
                    var existInDB = lstAttributeInputMappingInDB.FirstOrDefault(cond => cond.UAITM_UniversalItemAttributeMappingID == inputAttrType.UAITM_UniversalItemAttributeMappingID);
                    if (!existInDB.IsNullOrEmpty())
                    {
                        existInDB.UAITM_InputPriority = inputAttrType.UAITM_InputPriority;
                        existInDB.UAITM_ModifiedBy = currentLoggedInUserId;
                        existInDB.UAITM_ModifiedOn = DateTime.Now;
                    }
                    else
                    {
                        inputAttrType.UAITM_UniversalAttributeMappingID = uniAttributeMapId;
                        _dbContext.UniversalAttributeInputTypeMappings.AddObject(inputAttrType);
                    }
                });

                mappingToDelete.ForEach(dlt =>
                {
                    dlt.UAITM_IsDeleted = true;
                    dlt.UAITM_ModifiedBy = currentLoggedInUserId;
                    dlt.UAITM_ModifiedOn = DateTime.Now;
                });
            }
            else
            {
                lstInputMapping.ForEach(inputAttr =>
                {
                    inputAttr.UAITM_UniversalAttributeMappingID = uniAttributeMapId;
                    _dbContext.UniversalAttributeInputTypeMappings.AddObject(inputAttr);
                });
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        private void DeleteAttributeInputMapping(List<Int32> universalAttributeMappingIDs, Int32 currentLoggedInuserId)
        {
            List<UniversalAttributeInputTypeMapping> lstAttributeInputMapping = _dbContext.UniversalAttributeInputTypeMappings.Where(cnd =>
                                                                                      universalAttributeMappingIDs.Contains(cnd.UAITM_UniversalAttributeMappingID)
                                                                                               && !cnd.UAITM_IsDeleted).ToList();

            if (!lstAttributeInputMapping.IsNullOrEmpty())
            {
                lstAttributeInputMapping.ForEach(attr =>
                {
                    attr.UAITM_IsDeleted = true;
                    attr.UAITM_ModifiedOn = DateTime.Now;
                    attr.UAITM_ModifiedBy = currentLoggedInuserId;
                });
            }
        }
        #endregion


        #region Universal Rotation Mapping View
        List<UniversalRotationMappingViewContract> IUniversalMappingDataRepository.GetUniversalRotationMappingView(Int32 requirementPackageID)
        {
            List<UniversalRotationMappingViewContract> lstUniversalRotationMappingViewContract = new List<UniversalRotationMappingViewContract>();
            EntityConnection connection = base.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetUniversalRotationMappingView", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RequirementPackageID", requirementPackageID);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                lstUniversalRotationMappingViewContract = ds.Tables[0].AsEnumerable().Select(col =>
                     new UniversalRotationMappingViewContract
                     {
                         RequirementPackageID = Convert.ToInt32(col["RequirementPackageID"]),
                         RequirementPackageName = col["RequirementPackageName"] == DBNull.Value ? String.Empty : Convert.ToString(col["RequirementPackageName"]),
                         RequirementCategoryID = Convert.ToInt32(col["RequirementCategoryID"]),
                         RequirementCategoryName = col["RequirementCategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(col["RequirementCategoryName"]),
                         RequirementCategoryItemID = col["RequirementCategoryItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RequirementCategoryItemID"]),
                         RequirementItemID = col["RequirementItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RequirementItemID"]),
                         RequirementItemName = col["RequirementItemName"] == DBNull.Value ? String.Empty : Convert.ToString(col["RequirementItemName"]),
                         RequirementItemFieldID = col["RequirementItemFieldID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RequirementItemFieldID"]),
                         RequirementFieldID = col["RequirementFieldID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RequirementFieldID"]),
                         RequirementFieldName = col["RequirementFieldName"] == DBNull.Value ? String.Empty : Convert.ToString(col["RequirementFieldName"]),

                         RequirmentFieldOptionID = col["RequirmentFieldOptionID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RequirmentFieldOptionID"]),
                         RequirmentFieldOptionText = col["RequirmentFieldOptionText"] == DBNull.Value ? String.Empty : Convert.ToString(col["RequirmentFieldOptionText"]),
                         UniversalFieldID = col["UniversalFieldID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalFieldID"]),
                         UniversalFieldName = col["UniversalFieldName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UniversalFieldName"]),
                         UniversalFieldMappingID = col["UniversalFieldMappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalFieldMappingID"]),
                         //UniversalCategoryID = col["UniversalCategoryID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalCategoryID"]),
                         //UniversalCategoryName = col["UniversalCategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UniversalCategoryName"]),
                         //UniversalItemID = col["UniversalItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalItemID"]),
                         //UniversalItemName = col["UniversalItemName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UniversalItemName"]),
                         //UniversalFieldID = col["UniversalFieldID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalFieldID"]),
                         //UniversalFieldName = col["UniversalFieldName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UniversalFieldName"]),
                         //UniversalReqCatMappingID = col["UniversalReqCatMappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalReqCatMappingID"]),
                         //UniversalReqItemMappingID = col["UniversalReqItemMappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalReqItemMappingID"]),
                         //UniversalReqAttrMappingID = col["UniversalReqAttrMappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalReqAttrMappingID"]),
                         //UniversalCatItemMappingID = col["UniversalCatItemMappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalCatItemMappingID"]),
                         //UniversalItemAttrMappingID = col["UniversalItemAttrMappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalItemAttrMappingID"]),

                         MappedUniversalAttrOptionID = col["MappedUniversalAttrOptionID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["MappedUniversalAttrOptionID"]),

                         IsPackageDisabled = col["IsPackageDisabled"] == DBNull.Value ? false : Convert.ToBoolean(col["IsPackageDisabled"]),
                         RequirmentFieldDataTypeCode = col["RequirmentFieldDataTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(col["RequirmentFieldDataTypeCode"]),
                         MappedUniversalFieldID= col["MappedUniversalFieldID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["MappedUniversalFieldID"]),
                         MappedUniversalFieldName = col["MappedUniversalFieldName"] == DBNull.Value ? String.Empty : Convert.ToString(col["MappedUniversalFieldName"]),
                         UniversalFieldMappingDate = col["UniversalFieldMappingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["UniversalFieldMappingDate"]),

                     }).ToList();
            }

            return lstUniversalRotationMappingViewContract;
        }

        Boolean IUniversalMappingDataRepository.SaveUniversalRequirmentCategoryMappingData(UniversalRotationMappingViewContract updateContract, Int32 loggedInUserID)
        {
            UniversalRequirementCategoryMapping prevUniversalReqCatMapping = base.SharedDataDBContext.UniversalRequirementCategoryMappings
                                                                                 .Where(x => x.URCM_ID == updateContract.UniversalReqCatMappingID &&
                                                                                            !x.URCM_IsDeleted).FirstOrDefault();
            if (prevUniversalReqCatMapping.IsNotNull())
            {
                if (updateContract.IsNotNull() && updateContract.UniversalCategoryID == AppConsts.NONE)
                {
                    //CURRENT MAPPING IS DELETED
                    prevUniversalReqCatMapping.URCM_IsDeleted = true;
                    prevUniversalReqCatMapping.URCM_ModifiedBy = loggedInUserID;
                    prevUniversalReqCatMapping.URCM_ModifiedOn = DateTime.Now;
                }
                else
                {
                    //UPDATE RECORD WITH NEW MAPPING
                    prevUniversalReqCatMapping.URCM_UniversalCategoryID = updateContract.UniversalCategoryID;
                    prevUniversalReqCatMapping.URCM_IsDeleted = false;
                    prevUniversalReqCatMapping.URCM_ModifiedBy = loggedInUserID;
                    prevUniversalReqCatMapping.URCM_ModifiedOn = DateTime.Now;
                }


                //DELETE NAVIGATION MAPPINGS
                List<UniversalRequirementItemMapping> lstUniReqItemMapping = prevUniversalReqCatMapping.UniversalRequirementItemMappings.ToList();
                foreach (UniversalRequirementItemMapping item in lstUniReqItemMapping)
                {
                    item.URIM_IsDeleted = true;
                    item.URIM_ModifiedBy = loggedInUserID;
                    item.URIM_ModifiedOn = DateTime.Now;
                    List<UniversalRequirementAttributeMapping> lstUniReqAttrMapping = item.UniversalRequirementAttributeMappings.ToList();
                    foreach (UniversalRequirementAttributeMapping attr in lstUniReqAttrMapping)
                    {
                        attr.URAM_IsDeleted = true;
                        attr.URAM_ModifiedBy = loggedInUserID;
                        attr.URAM_ModifiedOn = DateTime.Now;

                        List<UniversalRequirementAttributeInputTypeMapping> lstUniReqInputTypeMapping = attr.UniversalRequirementAttributeInputTypeMappings.ToList();
                        foreach (UniversalRequirementAttributeInputTypeMapping inputAttrMap in lstUniReqInputTypeMapping)
                        {
                            //DELETE INPUT ATTRIBUTES MAPPINGS
                            inputAttrMap.URAITM_IsDeleted = true;
                            inputAttrMap.URAITM_ModifiedBy = loggedInUserID;
                            inputAttrMap.URAITM_ModifiedOn = DateTime.Now;
                        }

                        List<UniversalRequirementAttributeOptionMapping> lstUniversalRequirementAttributeOptionMapping = attr.UniversalRequirementAttributeOptionMappings.ToList();
                        foreach (UniversalRequirementAttributeOptionMapping attrOption in lstUniversalRequirementAttributeOptionMapping)
                        {
                            //DELETE REQUIREMENT ATTRIBUTE OPTION MAPPINGS
                            attrOption.URAOM_IsDeleted = true;
                            attrOption.URAOM_ModifiedBy = loggedInUserID;
                            attrOption.URAOM_ModifiedOn = DateTime.Now;
                        }
                    }
                }

            }
            else
            {
                //INSERT NEW MAPPING
                UniversalRequirementCategoryMapping newUniversalRequirementCategoryMapping = new UniversalRequirementCategoryMapping();
                newUniversalRequirementCategoryMapping.URCM_RequirementCategoryID = updateContract.RequirementCategoryID;
                newUniversalRequirementCategoryMapping.URCM_UniversalCategoryID = updateContract.UniversalCategoryID;
                newUniversalRequirementCategoryMapping.URCM_IsDeleted = false;
                newUniversalRequirementCategoryMapping.URCM_CreatedBy = loggedInUserID;
                newUniversalRequirementCategoryMapping.URCM_CreatedOn = DateTime.Now;

                base.SharedDataDBContext.AddToUniversalRequirementCategoryMappings(newUniversalRequirementCategoryMapping);
            }
            base.SharedDataDBContext.SaveChanges();
            return true;
        }

        Boolean IUniversalMappingDataRepository.SaveUniversalRequirmentItemMappingData(UniversalRotationMappingViewContract updateContract, Int32 loggedInUserID)
        {
            UniversalRequirementItemMapping prevUniversalReqItemMapping = base.SharedDataDBContext.UniversalRequirementItemMappings
                                                                              .Where(x => x.URIM_ID == updateContract.UniversalReqItemMappingID &&
                                                                                         !x.URIM_IsDeleted).FirstOrDefault();
            if (prevUniversalReqItemMapping.IsNotNull())
            {
                if (updateContract.IsNotNull() && updateContract.UniversalCatItemMappingID == AppConsts.NONE)
                {
                    //CURRENT MAPPING IS DELETED
                    prevUniversalReqItemMapping.URIM_IsDeleted = true;
                    prevUniversalReqItemMapping.URIM_ModifiedBy = loggedInUserID;
                    prevUniversalReqItemMapping.URIM_ModifiedOn = DateTime.Now;
                }
                else
                {
                    //UPDATE RECORD WITH NEW MAPPING
                    prevUniversalReqItemMapping.URIM_UniversalCategoryItemMappingID = updateContract.UniversalCatItemMappingID;
                    prevUniversalReqItemMapping.URIM_IsDeleted = false;
                    prevUniversalReqItemMapping.URIM_ModifiedBy = loggedInUserID;
                    prevUniversalReqItemMapping.URIM_ModifiedOn = DateTime.Now;
                }

                //DELETE NAVIGATION MAPPINGS
                List<UniversalRequirementAttributeMapping> lstUniReqAttrMapping = prevUniversalReqItemMapping.UniversalRequirementAttributeMappings.ToList();
                foreach (UniversalRequirementAttributeMapping attr in lstUniReqAttrMapping)
                {
                    attr.URAM_IsDeleted = true;
                    attr.URAM_ModifiedBy = loggedInUserID;
                    attr.URAM_ModifiedOn = DateTime.Now;

                    List<UniversalRequirementAttributeInputTypeMapping> lstUniReqInputTypeMapping = attr.UniversalRequirementAttributeInputTypeMappings.ToList();
                    foreach (UniversalRequirementAttributeInputTypeMapping inputAttrMap in lstUniReqInputTypeMapping)
                    {
                        //DELETE INPUT ATTRIBUTES MAPPINGS
                        inputAttrMap.URAITM_IsDeleted = true;
                        inputAttrMap.URAITM_ModifiedBy = loggedInUserID;
                        inputAttrMap.URAITM_ModifiedOn = DateTime.Now;
                    }

                    List<UniversalRequirementAttributeOptionMapping> lstUniversalRequirementAttributeOptionMapping = attr.UniversalRequirementAttributeOptionMappings.ToList();
                    foreach (UniversalRequirementAttributeOptionMapping attrOption in lstUniversalRequirementAttributeOptionMapping)
                    {
                        //DELETE REQUIREMENT ATTRIBUTE OPTION MAPPINGS
                        attrOption.URAOM_IsDeleted = true;
                        attrOption.URAOM_ModifiedBy = loggedInUserID;
                        attrOption.URAOM_ModifiedOn = DateTime.Now;
                    }
                }
            }
            else
            {
                //INSERT NEW MAPPING
                UniversalRequirementItemMapping newUniversalRequirementItemMapping = new UniversalRequirementItemMapping();
                newUniversalRequirementItemMapping.URIM_UniversalReqCategoryMappingID = updateContract.UniversalReqCatMappingID;
                newUniversalRequirementItemMapping.URIM_RequirementCategoryItemMappingID = updateContract.RequirementCategoryItemID;
                newUniversalRequirementItemMapping.URIM_UniversalCategoryItemMappingID = updateContract.UniversalCatItemMappingID;
                newUniversalRequirementItemMapping.URIM_IsDeleted = false;
                newUniversalRequirementItemMapping.URIM_CreatedBy = loggedInUserID;
                newUniversalRequirementItemMapping.URIM_CreatedOn = DateTime.Now;

                base.SharedDataDBContext.AddToUniversalRequirementItemMappings(newUniversalRequirementItemMapping);
            }
            base.SharedDataDBContext.SaveChanges();
            return true;
        }

        Boolean IUniversalMappingDataRepository.SaveUniversalRequirmentAttributeMappingData(UniversalRotationMappingViewContract updateContract, Int32 loggedInUserID)
        {

            Entity.SharedDataEntity.UniversalFieldMapping prevUniversalRequirementAttributeMapping = base.SharedDataDBContext.UniversalFieldMappings
                                                                                                                     .Where(x => x.UFM_ID == updateContract.UniversalFieldMappingID &&
                                                                                                                     !x.UFM_IsDeleted).FirstOrDefault();
            if (prevUniversalRequirementAttributeMapping.IsNotNull())
            {
                if (updateContract.IsNotNull() && updateContract.UniversalFieldID > AppConsts.NONE && prevUniversalRequirementAttributeMapping.UFM_UniversalFieldID != updateContract.UniversalFieldID)
                {
                    prevUniversalRequirementAttributeMapping.UFM_UniversalFieldID = updateContract.UniversalFieldID;
                    prevUniversalRequirementAttributeMapping.UFM_ModifiedBy = loggedInUserID;
                    prevUniversalRequirementAttributeMapping.UFM_ModifiedOn = DateTime.Now;

                    List<Entity.SharedDataEntity.UniversalFieldInputTypeMapping> lstUniReqInputTypeMapping = prevUniversalRequirementAttributeMapping.UniversalFieldInputTypeMappings.ToList();

                    foreach (Entity.SharedDataEntity.UniversalFieldInputTypeMapping attr in lstUniReqInputTypeMapping)
                    {
                        //DELETE INPUT ATTRIBUTES MAPPINGS
                        attr.UFITM_IsDeleted = true;
                        attr.UFITM_ModifiedBy = loggedInUserID;
                        attr.UFITM_ModifiedOn = DateTime.Now;
                    }

                    List<Entity.SharedDataEntity.UniversalFieldOptionMapping> lstUniversalRequirementAttributeOptionMapping = prevUniversalRequirementAttributeMapping.UniversalFieldOptionMappings.ToList();
                    foreach (Entity.SharedDataEntity.UniversalFieldOptionMapping attrOption in lstUniversalRequirementAttributeOptionMapping)
                    {
                        //DELETE REQUIREMENT ATTRIBUTE OPTION MAPPINGS
                        attrOption.UFOM_IsDeleted = true;
                        attrOption.UFOM_ModifiedBy = loggedInUserID;
                        attrOption.UFOM_ModifiedOn = DateTime.Now;
                    }
                }
                else if (updateContract.IsNotNull() && updateContract.UniversalFieldID == AppConsts.NONE)
                {
                    prevUniversalRequirementAttributeMapping.UFM_IsDeleted = true;
                    prevUniversalRequirementAttributeMapping.UFM_ModifiedBy = loggedInUserID;
                    prevUniversalRequirementAttributeMapping.UFM_ModifiedOn = DateTime.Now;
                }

                #region INPUT TYPE MAPPING
                List<Int32> prevItmAttrMappingIDs = new List<Int32>();
                Dictionary<Int32, Int32?> dicInputMapping = new Dictionary<Int32, Int32?>();

                List<Entity.SharedDataEntity.UniversalFieldInputTypeMapping> prevListUniReqInputTypeMapping = base.SharedDataDBContext.UniversalFieldInputTypeMappings.Where(x => x.UFITM_UniversalFieldMappingID == updateContract.UniversalFieldMappingID &&
                                                                                                                !x.UFITM_IsDeleted).ToList();

                if (!updateContract.lstUniReqAttrInputMapping.IsNullOrEmpty())
                {

                    dicInputMapping = updateContract.lstUniReqAttrInputMapping.ToDictionary(x => x.UFITM_UniversalFieldID, x => x.UFITM_InputPriority);
                }

                if (!prevListUniReqInputTypeMapping.IsNullOrEmpty())
                {
                    foreach (Entity.SharedDataEntity.UniversalFieldInputTypeMapping prevInpTypeMapping in prevListUniReqInputTypeMapping)
                    {

                        if (dicInputMapping.ContainsKey(prevInpTypeMapping.UFITM_UniversalFieldID))
                        {
                            //UPDATE EXISTING INPUT ATTRIBUTE MAPPINGS
                            prevInpTypeMapping.UFITM_InputPriority = dicInputMapping[prevInpTypeMapping.UFITM_UniversalFieldID]; //Update new value
                            prevInpTypeMapping.UFITM_IsDeleted = false;
                            prevInpTypeMapping.UFITM_ModifiedBy = loggedInUserID;
                            prevInpTypeMapping.UFITM_ModifiedOn = DateTime.Now;

                            prevItmAttrMappingIDs.Add(prevInpTypeMapping.UFITM_UniversalFieldID);
                        }
                        else
                        {
                            prevInpTypeMapping.UFITM_IsDeleted = true;
                            prevInpTypeMapping.UFITM_ModifiedBy = loggedInUserID;
                            prevInpTypeMapping.UFITM_ModifiedOn = DateTime.Now;
                        }
                    }
                }

                //ADD NEW ENTRIES
                if (!updateContract.lstUniReqAttrInputMapping.IsNullOrEmpty())
                {
                    InsertUniversalInputAttributeMapping(loggedInUserID, prevUniversalRequirementAttributeMapping, updateContract.lstUniReqAttrInputMapping.Where(x => !prevItmAttrMappingIDs.Contains(x.UFITM_UniversalFieldID)).ToList());
                }
                #endregion

                #region  UAT:2402 REQUIRMENT ATTRIBUTE OPTIONS
                if (!updateContract.lstUniversalRequirementAttributeOptionMapping.IsNullOrEmpty())
                {
                    List<Tuple<Int32, Int32>> prevAttrOptnMappingIDs = new List<Tuple<Int32, Int32>>();


                    var AttrOptionMappingList = updateContract.lstUniversalRequirementAttributeOptionMapping.Select(x => new { x.UFOM_AttributeOptionID, x.UFOM_UniversalFieldOptionID }).ToList();
                    List<Int32> requirmentFieldOptionIDs = updateContract.lstUniversalRequirementAttributeOptionMapping.Select(x => x.UFOM_UniversalFieldOptionID).ToList();



                    List<Entity.SharedDataEntity.UniversalFieldOptionMapping> prevLstUniversalRequirementAttributeOptionMapping = base.SharedDataDBContext
                         .UniversalFieldOptionMappings.Where(x => x.UFOM_UniversalFieldMappingID == updateContract.UniversalFieldMappingID && !x.UFOM_IsDeleted).ToList();

                    foreach (Entity.SharedDataEntity.UniversalFieldOptionMapping reqAttrOptn in prevLstUniversalRequirementAttributeOptionMapping)
                    {
                        var existCheck = AttrOptionMappingList.Where(s => s.UFOM_AttributeOptionID == reqAttrOptn.UFOM_AttributeOptionID && s.UFOM_UniversalFieldOptionID == reqAttrOptn.UFOM_UniversalFieldOptionID).FirstOrDefault();

                        if (!existCheck.IsNullOrEmpty())
                        {
                            //UPDATE EXISTING INPUT ATTRIBUTE MAPPINGS
                            reqAttrOptn.UFOM_UniversalFieldOptionID = existCheck.UFOM_UniversalFieldOptionID; // dicAttrOptionMapping[reqAttrOptn.URAOM_RequirementFieldOptionID.Value]; //Update new value
                            reqAttrOptn.UFOM_IsDeleted = false;
                            reqAttrOptn.UFOM_ModifiedBy = loggedInUserID;
                            reqAttrOptn.UFOM_ModifiedOn = DateTime.Now;

                            prevAttrOptnMappingIDs.Add(new Tuple<Int32, Int32>(reqAttrOptn.UFOM_AttributeOptionID.Value, reqAttrOptn.UFOM_UniversalFieldOptionID));
                        }
                        else
                        {
                            reqAttrOptn.UFOM_IsDeleted = true;
                            reqAttrOptn.UFOM_ModifiedBy = loggedInUserID;
                            reqAttrOptn.UFOM_ModifiedOn = DateTime.Now;
                            prevAttrOptnMappingIDs.Add(new Tuple<Int32, Int32>(reqAttrOptn.UFOM_AttributeOptionID.Value, reqAttrOptn.UFOM_UniversalFieldOptionID));
                        }
                    }
                    //ADD NEW ENTRIES
                    if (!updateContract.lstUniversalRequirementAttributeOptionMapping.IsNullOrEmpty())
                    {
                        InsertUniversalRequirementAttributeOptionMapping(loggedInUserID, prevUniversalRequirementAttributeMapping, updateContract.lstUniversalRequirementAttributeOptionMapping.Where(x => !prevAttrOptnMappingIDs.Any(cond => cond.Item1 == x.UFOM_AttributeOptionID && cond.Item2 == x.UFOM_UniversalFieldOptionID)).ToList());
                    }
                }
                else
                {

                    prevUniversalRequirementAttributeMapping.UniversalFieldOptionMappings.Where(cond => !cond.UFOM_IsDeleted).ForEach(reqAttrOptn =>
                    {
                        reqAttrOptn.UFOM_IsDeleted = true;
                        reqAttrOptn.UFOM_ModifiedBy = loggedInUserID;
                        reqAttrOptn.UFOM_ModifiedOn = DateTime.Now;
                    });
                }
                #endregion

            }

            else
            {

                Int32 categoryItemMappingID;
                if (updateContract.RequirementCategoryID > AppConsts.NONE)
                {
                    categoryItemMappingID = base.SharedDataDBContext.RequirementCategoryItems.Where(cond => cond.RCI_RequirementCategoryID == updateContract.RequirementCategoryID && cond.RCI_RequirementItemID == updateContract.RequirementItemID && !cond.RCI_IsDeleted).FirstOrDefault().RCI_ID;
                }
                else
                {
                    List<Int32> categoriesList = base.SharedDataDBContext.RequirementPackageCategories.Where(cond => cond.RPC_RequirementPackageID == updateContract.RequirementPackageID && !cond.RPC_IsDeleted).Select(x => x.RPC_RequirementCategoryID).ToList();
                    categoryItemMappingID = base.SharedDataDBContext.RequirementCategoryItems.Where(cond => categoriesList.Contains(cond.RCI_RequirementCategoryID) && cond.RCI_RequirementItemID == updateContract.RequirementItemID && !cond.RCI_IsDeleted).FirstOrDefault().RCI_ID;
                }

                Entity.SharedDataEntity.UniversalFieldMapping newUniversalRequirementAttributeMapping = new Entity.SharedDataEntity.UniversalFieldMapping();
                newUniversalRequirementAttributeMapping.UFM_UniversalFieldID = updateContract.UniversalFieldID;
                newUniversalRequirementAttributeMapping.UFM_CategoryItemMappingID = categoryItemMappingID;
                newUniversalRequirementAttributeMapping.UFM_ItemAttributeMappingID = updateContract.RequirementItemFieldID;
                newUniversalRequirementAttributeMapping.UFM_IsDeleted = false;
                newUniversalRequirementAttributeMapping.UFM_CreatedBy = loggedInUserID;
                newUniversalRequirementAttributeMapping.UFM_CreatedOn = DateTime.Now;

                if (!updateContract.lstUniReqAttrInputMapping.IsNullOrEmpty())
                {
                    InsertUniversalInputAttributeMapping(loggedInUserID, newUniversalRequirementAttributeMapping, updateContract.lstUniReqAttrInputMapping);
                }
                //UAT:2402
                if (!updateContract.lstUniversalRequirementAttributeOptionMapping.IsNullOrEmpty())
                {
                    InsertUniversalRequirementAttributeOptionMapping(loggedInUserID, newUniversalRequirementAttributeMapping, updateContract.lstUniversalRequirementAttributeOptionMapping);
                }

                base.SharedDataDBContext.AddToUniversalFieldMappings(newUniversalRequirementAttributeMapping);
            }

            if (base.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;

            return false;
        }


        private static void InsertUniversalInputAttributeMapping(Int32 loggedInUserID, Entity.SharedDataEntity.UniversalFieldMapping prevUniversalRequirementAttributeMapping, List<Entity.SharedDataEntity.UniversalFieldInputTypeMapping> mappingToInsert)
        {
            foreach (Entity.SharedDataEntity.UniversalFieldInputTypeMapping item in mappingToInsert)
            {
                Entity.SharedDataEntity.UniversalFieldInputTypeMapping obj = new Entity.SharedDataEntity.UniversalFieldInputTypeMapping();
                obj.UFITM_InputPriority = item.UFITM_InputPriority;
                obj.UFITM_UniversalFieldID = item.UFITM_UniversalFieldID;
                obj.UFITM_UniversalFieldMappingID = item.UFITM_UniversalFieldMappingID;
                obj.UFITM_IsDeleted = false;
                obj.UFITM_CreatedBy = loggedInUserID;
                obj.UFITM_CreatedOn = DateTime.Now;
                prevUniversalRequirementAttributeMapping.UniversalFieldInputTypeMappings.Add(obj);
            }
        }

        private static void InsertUniversalInputAttributeClientMapping(Int32 loggedInUserID, Entity.ClientEntity.UniversalFieldMapping prevUniversalRequirementAttributeMapping, List<Entity.ClientEntity.UniversalFieldInputTypeMapping> mappingToInsert)
        {
            foreach (Entity.ClientEntity.UniversalFieldInputTypeMapping item in mappingToInsert)
            {
                Entity.ClientEntity.UniversalFieldInputTypeMapping obj = new Entity.ClientEntity.UniversalFieldInputTypeMapping();
                obj.UFITM_InputPriority = item.UFITM_InputPriority;
                obj.UFITM_UniversalFieldID = item.UFITM_UniversalFieldID;
                obj.UFITM_UniversalFieldMappingID = item.UFITM_UniversalFieldMappingID;
                obj.UFITM_IsDeleted = false;
                obj.UFITM_CreatedBy = loggedInUserID;
                obj.UFITM_CreatedOn = DateTime.Now;
                prevUniversalRequirementAttributeMapping.UniversalFieldInputTypeMappings.Add(obj);
            }
        }
        //private static void InsertUniversalRequirementAttributeOptionMapping(Int32 loggedInUserID, UniversalRequirementAttributeMapping universalRequirementAttributeMapping, List<UniversalRequirementAttributeOptionMapping> mappingToInsert)
        //{
        //    foreach (UniversalRequirementAttributeOptionMapping item in mappingToInsert)
        //    {
        //        if (item.URAOM_UniversalAttributeOptionID > AppConsts.NONE)
        //        {
        //            UniversalRequirementAttributeOptionMapping obj = new UniversalRequirementAttributeOptionMapping();
        //            obj.URAOM_UniversalAttributeOptionID = item.URAOM_UniversalAttributeOptionID;
        //            obj.URAOM_RequirementFieldOptionID = item.URAOM_RequirementFieldOptionID;
        //            obj.URAOM_UniversalReqAttributeMappingID = item.URAOM_UniversalReqAttributeMappingID;
        //            obj.URAOM_IsDeleted = false;
        //            obj.URAOM_CreatedBy = loggedInUserID;
        //            obj.URAOM_CreatedOn = DateTime.Now;
        //            universalRequirementAttributeMapping.UniversalRequirementAttributeOptionMappings.Add(obj);
        //        }
        //    }
        //}
        private static void InsertUniversalRequirementAttributeOptionMapping(Int32 loggedInUserID, Entity.SharedDataEntity.UniversalFieldMapping universalRequirementAttributeMapping, List<Entity.SharedDataEntity.UniversalFieldOptionMapping> mappingToInsert)
        {
            foreach (Entity.SharedDataEntity.UniversalFieldOptionMapping item in mappingToInsert)
            {
                if (item.UFOM_AttributeOptionID > AppConsts.NONE)
                {
                    Entity.SharedDataEntity.UniversalFieldOptionMapping obj = new Entity.SharedDataEntity.UniversalFieldOptionMapping();
                    obj.UFOM_AttributeOptionID = item.UFOM_AttributeOptionID;
                    obj.UFOM_UniversalFieldMappingID = item.UFOM_UniversalFieldMappingID;
                    obj.UFOM_UniversalFieldOptionID = item.UFOM_UniversalFieldOptionID;
                    obj.UFOM_IsDeleted = false;
                    obj.UFOM_CreatedBy = loggedInUserID;
                    obj.UFOM_CreatedOn = DateTime.Now;
                    universalRequirementAttributeMapping.UniversalFieldOptionMappings.Add(obj);
                }
            }
        }
        private static void InsertUniversalRequirementAttributeOptionClientMapping(Int32 loggedInUserID, Entity.ClientEntity.UniversalFieldMapping universalRequirementAttributeMapping, List<Entity.ClientEntity.UniversalFieldOptionMapping> mappingToInsert)
        {
            foreach (Entity.ClientEntity.UniversalFieldOptionMapping item in mappingToInsert)
            {
                if (item.UFOM_AttributeOptionID > AppConsts.NONE)
                {
                    Entity.ClientEntity.UniversalFieldOptionMapping obj = new Entity.ClientEntity.UniversalFieldOptionMapping();
                    obj.UFOM_AttributeOptionID = item.UFOM_AttributeOptionID;
                    obj.UFOM_UniversalFieldMappingID = item.UFOM_UniversalFieldMappingID;
                    obj.UFOM_UniversalFieldOptionID = item.UFOM_UniversalFieldOptionID;
                    obj.UFOM_IsDeleted = false;
                    obj.UFOM_CreatedBy = loggedInUserID;
                    obj.UFOM_CreatedOn = DateTime.Now;
                    universalRequirementAttributeMapping.UniversalFieldOptionMappings.Add(obj);
                }
            }
        }
        Boolean IUniversalMappingDataRepository.CopySharedToTenantRequirementUniversalMapping(Int32 sharedRotationPackageId, Int32 tenantRotPackageId
                                                                                              , Int32 currentLoggedInUserId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@CurrentLoggedInUserId", currentLoggedInUserId),
                              new SqlParameter("@TenantCopiedRotPackageID", tenantRotPackageId),
                              new SqlParameter("@SharedRotationPackageId", sharedRotationPackageId)
                        };

                base.OpenSQLDataReaderConnection(con);

                base.ExecuteSQLDataReader(con, "usp_CopySharedToTenantRequirementUniversalMapping", sqlParameterCollection);

                base.CloseSQLDataReaderConnection(con);
            }
            return true;
        }

        Boolean IUniversalMappingDataRepository.CopyTenantToSharedRequirementUniversalMapping(Int32 sharedCopiedRotationPackageId, Int32 tenantRotPackageId
                                                                                              , Int32 currentLoggedInUserId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@CurrentLoggedInUserId", currentLoggedInUserId),
                              new SqlParameter("@TenantRotPackageID", tenantRotPackageId),
                              new SqlParameter("@SharedCopiedRotationPackageId", sharedCopiedRotationPackageId)
                        };

                base.OpenSQLDataReaderConnection(con);

                //base.ExecuteSQLDataReader(con, "usp_CopyTenantToSharedRequirementUniversalMapping", sqlParameterCollection);

                base.CloseSQLDataReaderConnection(con);
            }
            return true;
        }

        Boolean IUniversalMappingDataRepository.CopySharedToSharedReqUniversalMappingForPkg(Int32 sharedRotationPackageId, Int32 sharedCopiedRotationPackageId
                                                                                              , Int32 currentLoggedInUserId)
        {
            EntityConnection connection = base.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@CurrentLoggedInUserId", currentLoggedInUserId),
                              new SqlParameter("@SharedRotationPackageID", sharedRotationPackageId),
                              new SqlParameter("@SharedCopiedRotationPackageId", sharedCopiedRotationPackageId)
                        };

                base.OpenSQLDataReaderConnection(con);

                base.ExecuteSQLDataReader(con, "usp_CopySharedToSharedReqUniversalMappingForPkg", sqlParameterCollection);

                base.CloseSQLDataReaderConnection(con);
            }
            return true;
        }


        List<UniversalRequirementAttributeInputTypeMapping> IUniversalMappingDataRepository.GetUniversalRequirementAttributeInputTypeMapping(Int32 universalReqAttrMappingID)
        {
            return base.SharedDataDBContext.UniversalRequirementAttributeInputTypeMappings.Where(x => x.URAITM_UniversalReqAttributeMappingID == universalReqAttrMappingID && !x.URAITM_IsDeleted).ToList();
        }

        List<Entity.SharedDataEntity.UniversalFieldInputTypeMapping> IUniversalMappingDataRepository.GetUniversalFieldInputTypeMappings(Int32 universalFieldMappingID)
        {
            return base.SharedDataDBContext.UniversalFieldInputTypeMappings.Where(x => x.UFITM_UniversalFieldMappingID == universalFieldMappingID && !x.UFITM_IsDeleted).ToList();
        }
        #endregion

        #region Master Rotation Package
        UniversalRequirementCategoryMapping IUniversalMappingDataRepository.GetUniversalCategoryByReqCatID(Int32 ReqCatID)
        {
            return base.SharedDataDBContext.UniversalRequirementCategoryMappings.Where(cond => cond.URCM_RequirementCategoryID == ReqCatID
                            && !cond.URCM_IsDeleted && !cond.UniversalCategory.UC_IsDeleted).FirstOrDefault();
        }
        UniversalRequirementItemMapping IUniversalMappingDataRepository.GetUniversalItemsByUniReqCatItmID(Int32 uniReqCatID, Int32 ReqItmID, Int32 ReqCatID)
        {
            Int32 ReqCatItmMappingID = base.SharedDataDBContext.RequirementCategoryItems.Where(cond => cond.RCI_RequirementItemID == ReqItmID && cond.RCI_RequirementCategoryID == ReqCatID
                            && !cond.RCI_IsDeleted).FirstOrDefault().RCI_ID;
            return base.SharedDataDBContext.UniversalRequirementItemMappings.Where(cond => cond.URIM_UniversalReqCategoryMappingID == uniReqCatID
                                                            && !cond.URIM_IsDeleted && cond.URIM_RequirementCategoryItemMappingID == ReqCatItmMappingID).FirstOrDefault();
        }
        UniversalItem IUniversalMappingDataRepository.GetUniversalItemDetailsByReqItemID(Int32 ReqItmID, Int32 ReqCatID = 0)
        {
            Int32 ReqCatItmMappingID = AppConsts.NONE;
            if (ReqCatID > AppConsts.NONE)
            {
                ReqCatItmMappingID = base.SharedDataDBContext.RequirementCategoryItems.Where(cond => cond.RCI_RequirementItemID == ReqItmID
                                            && !cond.RCI_IsDeleted && cond.RCI_RequirementCategoryID == ReqCatID).FirstOrDefault().RCI_ID;
            }
            else
            {
                ReqCatItmMappingID = base.SharedDataDBContext.RequirementCategoryItems.Where(cond => cond.RCI_RequirementItemID == ReqItmID
                                            && !cond.RCI_IsDeleted).FirstOrDefault().RCI_ID;
            }
            if (ReqCatItmMappingID > AppConsts.NONE)
            {
                UniversalRequirementItemMapping URIM = base.SharedDataDBContext.UniversalRequirementItemMappings.Where(cond => !cond.URIM_IsDeleted
                                              && cond.URIM_RequirementCategoryItemMappingID == ReqCatItmMappingID).FirstOrDefault();
                if (!URIM.IsNullOrEmpty() && !URIM.UniversalCategoryItemMapping.IsNullOrEmpty())
                    return URIM.UniversalCategoryItemMapping.UniversalItem;
            }
            return null;
        }


        UniversalRequirementAttributeMapping IUniversalMappingDataRepository.GetUniversalattributeDetailsByItmFieldMappingID(Int32 UniReqItemMappingID, Int32 ReqItmFieldID)
        {
            return base.SharedDataDBContext.UniversalRequirementAttributeMappings.Where(cond => !cond.URAM_IsDeleted && cond.URAM_RequirementItemFieldMappingID == ReqItmFieldID
                && cond.URAM_UniversalReqItemMappingID == UniReqItemMappingID && !cond.UniversalItemAttributeMapping.UIAM_IsDeleted).FirstOrDefault();
        }
        List<UniversalRequirementAttributeInputTypeMapping> IUniversalMappingDataRepository.GetAtrInputPriorityByID(Int32 uniReqAtrMappingID)
        {
            return base.SharedDataDBContext.UniversalRequirementAttributeInputTypeMappings.Where(cond => !cond.URAITM_IsDeleted
                                    && cond.URAITM_UniversalReqAttributeMappingID == uniReqAtrMappingID).ToList();
        }

        List<Entity.SharedDataEntity.UniversalFieldInputTypeMapping> IUniversalMappingDataRepository.GetUniversalAtrInputPriorityByID(Int32 uniFieldMappingID)
        {
            return base.SharedDataDBContext.UniversalFieldInputTypeMappings.Where(cond => !cond.UFITM_IsDeleted
                                    && cond.UFITM_UniversalFieldMappingID == uniFieldMappingID && !cond.UniversalField.UF_IsDeleted).ToList();
        }

        List<Entity.SharedDataEntity.UniversalFieldInputTypeMapping> IUniversalMappingDataRepository.GetUniversalFieldAtrInputPriorityByID(Int32 uniFieldID)
        {
            return base.SharedDataDBContext.UniversalFieldInputTypeMappings.Where(cond => !cond.UFITM_IsDeleted
                                    && cond.UFITM_UniversalFieldMappingID == uniFieldID && !cond.UniversalField.UF_IsDeleted).ToList();
        }

        Int32 IUniversalMappingDataRepository.GetUniversalReqAtrMappingID(Int32 uniReqItmID, Int32 uniItmAtrID)
        {
            return base.SharedDataDBContext.UniversalRequirementAttributeMappings.Where(cond => !cond.URAM_IsDeleted && cond.URAM_UniversalItemAttributeMappingID == uniItmAtrID
                && cond.URAM_UniversalReqItemMappingID == uniReqItmID).FirstOrDefault().URAM_ID;
        }
        Boolean IUniversalMappingDataRepository.SaveChangesAttributeInputPriorty(List<UniversalRequirementAttributeInputTypeMapping> DataToSave)
        {
            DataToSave.ForEach(x =>
            {
                base.SharedDataDBContext.UniversalRequirementAttributeInputTypeMappings.AddObject(x);
            });
            if (base.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        Int32 IUniversalMappingDataRepository.GetRequirementItmFieldIDByReqFldID(Int32 ReqFieldID, Int32 ReqItmID)
        {
            return base.SharedDataDBContext.RequirementItemFields.Where(cond => !cond.RIF_IsDeleted
                && cond.RIF_RequirementFieldID == ReqFieldID && cond.RIF_RequirementItemID == ReqItmID).FirstOrDefault().RIF_ID;
        }
        #endregion

        #region Universal Compliance Mapping View

        List<UniversalComplianceMappingViewContract> IUniversalMappingDataRepository.GetUniversalComplianceMappingView(Int32 compliancePackageID)
        {
            List<UniversalComplianceMappingViewContract> lstUniversalComplianceMappingViewContract = new List<UniversalComplianceMappingViewContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetUniversalComplianceMappingView", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CompliancePackageID", compliancePackageID);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                lstUniversalComplianceMappingViewContract = ds.Tables[0].AsEnumerable().Select(col =>
                     new UniversalComplianceMappingViewContract
                     {
                         CompliancePackageID = Convert.ToInt32(col["CompliancePackageID"]),
                         CompliancePackageName = col["CompliancePackageName"] == DBNull.Value ? String.Empty : Convert.ToString(col["CompliancePackageName"]),
                         ComplianceCategoryID = Convert.ToInt32(col["ComplianceCategoryID"]),
                         ComplianceCategoryName = col["ComplianceCategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ComplianceCategoryName"]),
                         ComplianceCategoryItemID = col["ComplianceCategoryItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ComplianceCategoryItemID"]),
                         ComplianceItemID = col["ComplianceItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ComplianceItemID"]),
                         ComplianceItemName = col["ComplianceItemName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ComplianceItemName"]),
                         ComplianceItemAttributeID = col["ComplianceItemAttributeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ComplianceItemAttributeID"]),
                         ComplianceAttributeID = col["ComplianceAttributeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ComplianceAttributeID"]),
                         ComplianceAttributeName = col["ComplianceAttributeName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ComplianceAttributeName"]),
                         //UniversalCategoryID = col["UniversalCategoryID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalCategoryID"]),
                         //UniversalCategoryName = col["UniversalCategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UniversalCategoryName"]),
                         //UniversalItemID = col["UniversalItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalItemID"]),
                         //UniversalItemName = col["UniversalItemName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UniversalItemName"]),
                         UniversalFieldID = col["UniversalFieldID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalFieldID"]),
                         UniversalFieldName = col["UniversalFieldName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UniversalFieldName"]),
                         //UniversalCatMappingID = col["UniversalCatMappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalCatMappingID"]),
                         //UniversalItemMappingID = col["UniversalItemMappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalItemMappingID"]),
                         //UniversalAttrMappingID = col["UniversalAttrMappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalAttrMappingID"]),
                         //UniversalCatItemMappingID = col["UniversalCatItemMappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalCatItemMappingID"]),
                         UniversalItemAttrMappingID = col["UniversalItemAttrMappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalItemAttrMappingID"]),
                         ComplianceAttrDataTypeCode = col["ComplianceAttrDataTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(col["ComplianceAttrDataTypeCode"]),
                         ComplianceAttributeOptionID = col["ComplianceAttributeOptionID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ComplianceAttributeOptionID"]),
                         ComplianceAttributeOptionText = col["ComplianceAttributeOptionText"] == DBNull.Value ? String.Empty : Convert.ToString(col["ComplianceAttributeOptionText"]),
                         MappedUniversalAttrOptionID = col["MappedUniversalAttrOptionID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["MappedUniversalAttrOptionID"]),
                         UniversalFieldMappingID = col["UniversalFieldMappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["UniversalFieldMappingID"]),
                         UniversalFieldMappingDate = col["UniversalFieldMappingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["UniversalFieldMappingDate"])


                     }).ToList();
            }

            return lstUniversalComplianceMappingViewContract;
        }

        List<Entity.ClientEntity.UniversalFieldInputTypeMapping> IUniversalMappingDataRepository.GetUniversalAttributeInputTypeMapping(Int32 UniversalFieldMappingID)
        {
            return _dbContext.UniversalFieldInputTypeMappings.Where(x => x.UFITM_UniversalFieldMappingID == UniversalFieldMappingID && !x.UFITM_IsDeleted).ToList();
        }

        Boolean IUniversalMappingDataRepository.SaveUpdateAttributeMappingWithUniversalAttributeFromView(Entity.ClientEntity.UniversalFieldMapping uamData, Int32 loggedInUserID)
        {
            Entity.ClientEntity.UniversalFieldMapping prevUniversalRequirementAttributeMapping = _dbContext.UniversalFieldMappings
                                                                                                                     .Where(x => x.UFM_ID == uamData.UFM_ID &&
                                                                                                                     !x.UFM_IsDeleted).FirstOrDefault();
            if (prevUniversalRequirementAttributeMapping.IsNotNull())
            {
                if (uamData.IsNotNull() && prevUniversalRequirementAttributeMapping.UFM_UniversalFieldID != uamData.UFM_UniversalFieldID)
                {
                    prevUniversalRequirementAttributeMapping.UFM_UniversalFieldID = uamData.UFM_UniversalFieldID;
                    prevUniversalRequirementAttributeMapping.UFM_ModifiedBy = loggedInUserID;
                    prevUniversalRequirementAttributeMapping.UFM_ModifiedOn = DateTime.Now;

                    List<Entity.ClientEntity.UniversalFieldInputTypeMapping> lstUniReqInputTypeMapping = prevUniversalRequirementAttributeMapping.UniversalFieldInputTypeMappings.ToList();

                    foreach (Entity.ClientEntity.UniversalFieldInputTypeMapping attr in lstUniReqInputTypeMapping)
                    {
                        //DELETE INPUT ATTRIBUTES MAPPINGS
                        attr.UFITM_IsDeleted = true;
                        attr.UFITM_ModifiedBy = loggedInUserID;
                        attr.UFITM_ModifiedOn = DateTime.Now;
                    }

                    List<Entity.ClientEntity.UniversalFieldOptionMapping> lstUniversalRequirementAttributeOptionMapping = prevUniversalRequirementAttributeMapping.UniversalFieldOptionMappings.ToList();
                    foreach (Entity.ClientEntity.UniversalFieldOptionMapping attrOption in lstUniversalRequirementAttributeOptionMapping)
                    {
                        //DELETE REQUIREMENT ATTRIBUTE OPTION MAPPINGS
                        attrOption.UFOM_IsDeleted = true;
                        attrOption.UFOM_ModifiedBy = loggedInUserID;
                        attrOption.UFOM_ModifiedOn = DateTime.Now;
                    }
                }

                #region INPUT TYPE MAPPING
                List<Int32> prevItmAttrMappingIDs = new List<Int32>();
                Dictionary<Int32, Int32?> dicInputMapping = new Dictionary<Int32, Int32?>();

                List<Entity.ClientEntity.UniversalFieldInputTypeMapping> prevListUniReqInputTypeMapping = _dbContext.UniversalFieldInputTypeMappings.Where(x => x.UFITM_UniversalFieldMappingID == uamData.UFM_ID &&
                                                                                                                !x.UFITM_IsDeleted).ToList();

                if (!uamData.UniversalFieldInputTypeMappings.IsNullOrEmpty())
                {

                    dicInputMapping = uamData.UniversalFieldInputTypeMappings.ToDictionary(x => x.UFITM_UniversalFieldID, x => x.UFITM_InputPriority);
                }

                if (!prevListUniReqInputTypeMapping.IsNullOrEmpty())
                {
                    foreach (Entity.ClientEntity.UniversalFieldInputTypeMapping prevInpTypeMapping in prevListUniReqInputTypeMapping)
                    {

                        if (dicInputMapping.ContainsKey(prevInpTypeMapping.UFITM_UniversalFieldID))
                        {
                            //UPDATE EXISTING INPUT ATTRIBUTE MAPPINGS
                            prevInpTypeMapping.UFITM_InputPriority = dicInputMapping[prevInpTypeMapping.UFITM_UniversalFieldID]; //Update new value
                            prevInpTypeMapping.UFITM_IsDeleted = false;
                            prevInpTypeMapping.UFITM_ModifiedBy = loggedInUserID;
                            prevInpTypeMapping.UFITM_ModifiedOn = DateTime.Now;

                            prevItmAttrMappingIDs.Add(prevInpTypeMapping.UFITM_UniversalFieldID);
                        }
                        else
                        {
                            prevInpTypeMapping.UFITM_IsDeleted = true;
                            prevInpTypeMapping.UFITM_ModifiedBy = loggedInUserID;
                            prevInpTypeMapping.UFITM_ModifiedOn = DateTime.Now;
                        }
                    }
                }

                //ADD NEW ENTRIES
                if (!uamData.UniversalFieldInputTypeMappings.IsNullOrEmpty())
                {
                    InsertUniversalInputAttributeClientMapping(loggedInUserID, prevUniversalRequirementAttributeMapping, uamData.UniversalFieldInputTypeMappings.Where(x => !prevItmAttrMappingIDs.Contains(x.UFITM_UniversalFieldID)).ToList());
                }
                #endregion

                #region  UAT:2402 REQUIRMENT ATTRIBUTE OPTIONS
                if (!uamData.UniversalFieldOptionMappings.IsNullOrEmpty())
                {
                    List<Tuple<Int32, Int32>> prevAttrOptnMappingIDs = new List<Tuple<Int32, Int32>>();


                    var AttrOptionMappingList = uamData.UniversalFieldOptionMappings.Select(x => new { x.UFOM_AttributeOptionID, x.UFOM_UniversalFieldOptionID }).ToList();
                    List<Int32> requirmentFieldOptionIDs = uamData.UniversalFieldOptionMappings.Select(x => x.UFOM_UniversalFieldOptionID).ToList();



                    List<Entity.ClientEntity.UniversalFieldOptionMapping> prevLstUniversalRequirementAttributeOptionMapping = _dbContext
                         .UniversalFieldOptionMappings.Where(x => x.UFOM_UniversalFieldMappingID == uamData.UFM_ID && !x.UFOM_IsDeleted).ToList();

                    foreach (Entity.ClientEntity.UniversalFieldOptionMapping reqAttrOptn in prevLstUniversalRequirementAttributeOptionMapping)
                    {
                        var existCheck = AttrOptionMappingList.Where(s => s.UFOM_AttributeOptionID == reqAttrOptn.UFOM_AttributeOptionID && s.UFOM_UniversalFieldOptionID == reqAttrOptn.UFOM_UniversalFieldOptionID).FirstOrDefault();

                        if (!existCheck.IsNullOrEmpty())
                        {
                            //UPDATE EXISTING INPUT ATTRIBUTE MAPPINGS
                            reqAttrOptn.UFOM_UniversalFieldOptionID = existCheck.UFOM_UniversalFieldOptionID; // dicAttrOptionMapping[reqAttrOptn.URAOM_RequirementFieldOptionID.Value]; //Update new value
                            reqAttrOptn.UFOM_IsDeleted = false;
                            reqAttrOptn.UFOM_ModifiedBy = loggedInUserID;
                            reqAttrOptn.UFOM_ModifiedOn = DateTime.Now;

                            prevAttrOptnMappingIDs.Add(new Tuple<Int32, Int32>(reqAttrOptn.UFOM_AttributeOptionID.Value, reqAttrOptn.UFOM_UniversalFieldOptionID));
                        }
                        else
                        {
                            reqAttrOptn.UFOM_IsDeleted = true;
                            reqAttrOptn.UFOM_ModifiedBy = loggedInUserID;
                            reqAttrOptn.UFOM_ModifiedOn = DateTime.Now;
                            prevAttrOptnMappingIDs.Add(new Tuple<Int32, Int32>(reqAttrOptn.UFOM_AttributeOptionID.Value, reqAttrOptn.UFOM_UniversalFieldOptionID));
                        }
                    }
                    //ADD NEW ENTRIES
                    if (!uamData.UniversalFieldOptionMappings.IsNullOrEmpty())
                    {
                        InsertUniversalRequirementAttributeOptionClientMapping(loggedInUserID, prevUniversalRequirementAttributeMapping, uamData.UniversalFieldOptionMappings.Where(x => !prevAttrOptnMappingIDs.Any(cond => cond.Item1 == x.UFOM_AttributeOptionID && cond.Item2 == x.UFOM_UniversalFieldOptionID)).ToList());
                    }
                }
                else
                {

                    prevUniversalRequirementAttributeMapping.UniversalFieldOptionMappings.Where(cond => !cond.UFOM_IsDeleted).ForEach(reqAttrOptn =>
                    {
                        reqAttrOptn.UFOM_IsDeleted = true;
                        reqAttrOptn.UFOM_ModifiedBy = loggedInUserID;
                        reqAttrOptn.UFOM_ModifiedOn = DateTime.Now;
                    });
                }
                #endregion

            }

            else
            {

                Entity.ClientEntity.UniversalFieldMapping newUniversalRequirementAttributeMapping = new Entity.ClientEntity.UniversalFieldMapping();
                newUniversalRequirementAttributeMapping.UFM_UniversalFieldID = uamData.UFM_UniversalFieldID;
                newUniversalRequirementAttributeMapping.UFM_CategoryItemMappingID = uamData.UFM_CategoryItemMappingID;
                newUniversalRequirementAttributeMapping.UFM_ItemAttributeMappingID = uamData.UFM_ItemAttributeMappingID;
                newUniversalRequirementAttributeMapping.UFM_UniversalMappingTypeID = uamData.UFM_UniversalMappingTypeID;
                newUniversalRequirementAttributeMapping.UFM_IsDeleted = false;
                newUniversalRequirementAttributeMapping.UFM_CreatedBy = loggedInUserID;
                newUniversalRequirementAttributeMapping.UFM_CreatedOn = DateTime.Now;

                if (!uamData.UniversalFieldInputTypeMappings.IsNullOrEmpty())
                {
                    InsertUniversalInputAttributeClientMapping(loggedInUserID, newUniversalRequirementAttributeMapping, uamData.UniversalFieldInputTypeMappings.ToList());
                }
                //UAT:2402
                if (!uamData.UniversalFieldOptionMappings.IsNullOrEmpty())
                {
                    InsertUniversalRequirementAttributeOptionClientMapping(loggedInUserID, newUniversalRequirementAttributeMapping, uamData.UniversalFieldOptionMappings.ToList());
                }

                _dbContext.AddToUniversalFieldMappings(newUniversalRequirementAttributeMapping);
            }

            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;

            return false;
        }
        //Boolean IUniversalMappingDataRepository.SaveUpdateAttributeMappingWithUniversalAttributeFromView(UniversalAttributeMapping uamData, Int32 loggedInUserID)
        //{
        //    if (!uamData.IsNullOrEmpty() && uamData.UAM_ID > AppConsts.NONE)
        //    {
        //        UniversalAttributeMapping uamDataInDB = _dbContext.UniversalAttributeMappings.FirstOrDefault(cond => cond.UAM_ID == uamData.UAM_ID && !cond.UAM_IsDeleted);
        //        if (!uamDataInDB.IsNullOrEmpty())
        //        {
        //            if (uamData.IsNotNull() && uamData.UAM_ItemAttributeMappingID == AppConsts.NONE)
        //            {
        //                CURRENT MAPPING IS DELETED
        //                uamDataInDB.UAM_IsDeleted = true;
        //                uamDataInDB.UAM_ModifiedBy = loggedInUserID;
        //                uamDataInDB.UAM_ModifiedOn = DateTime.Now;

        //                List<UniversalAttributeInputTypeMapping> lstUniInputTypeMapping = uamDataInDB.UniversalAttributeInputTypeMappings.ToList();
        //                foreach (UniversalAttributeInputTypeMapping attr in lstUniInputTypeMapping)
        //                {
        //                    DELETE INPUT ATTRIBUTES MAPPINGS
        //                    attr.UAITM_IsDeleted = true;
        //                    attr.UAITM_ModifiedBy = loggedInUserID;
        //                    attr.UAITM_ModifiedOn = DateTime.Now;
        //                }

        //                List<UniversalAttributeOptionMapping> lstUniversalAttributeOptionMapping = uamDataInDB.UniversalAttributeOptionMappings.ToList();
        //                foreach (UniversalAttributeOptionMapping attrOption in lstUniversalAttributeOptionMapping)
        //                {
        //                    DELETE ATTRIBUTE OPTION MAPPINGS
        //                    attrOption.UAOM_IsDeleted = true;
        //                    attrOption.UAOM_ModifiedBy = loggedInUserID;
        //                    attrOption.UAOM_ModifiedOn = DateTime.Now;
        //                }
        //            }
        //            else
        //            {
        //                uamDataInDB.UAM_ItemAttributeMappingID = uamData.UAM_ItemAttributeMappingID;
        //                uamDataInDB.UAM_UniversalItemAttributeMappingID = uamData.UAM_UniversalItemAttributeMappingID;
        //                uamDataInDB.UAM_UniversalItemMappingID = uamData.UAM_UniversalItemMappingID;
        //                uamDataInDB.UAM_ModifiedBy = uamData.UAM_ModifiedBy;
        //                uamDataInDB.UAM_ModifiedOn = DateTime.Now;

        //                #region ATTRIBUTE INPUT TYPE MAPPING
        //                List<UniversalAttributeInputTypeMapping> lstPrevAttrInputTypeMappings = uamDataInDB.UniversalAttributeInputTypeMappings.Where(x => !x.UAITM_IsDeleted).ToList();

        //                List<Int32> prevItmAttrMappingIDs = new List<Int32>();
        //                Dictionary<Int32, Int32?> dicAttrInputMapping = uamData.UniversalAttributeInputTypeMappings.ToDictionary(x => x.UAITM_UniversalItemAttributeMappingID, x => x.UAITM_InputPriority);

        //                if (!lstPrevAttrInputTypeMappings.IsNullOrEmpty())
        //                {
        //                    List<Int32> attrInputIDsToRemove = prevItmAttrMappingIDs.Except(uamData.UniversalAttributeInputTypeMappings.Select(x => x.UAITM_UniversalItemAttributeMappingID)).ToList();
        //                    foreach (UniversalAttributeInputTypeMapping prevInpTypeMapping in lstPrevAttrInputTypeMappings)
        //                    {
        //                        if (dicAttrInputMapping.ContainsKey(prevInpTypeMapping.UAITM_UniversalItemAttributeMappingID))
        //                        {
        //                            UPDATE EXISTING INPUT ATTRIBUTE MAPPINGS
        //                            prevInpTypeMapping.UAITM_InputPriority = dicAttrInputMapping[prevInpTypeMapping.UAITM_UniversalItemAttributeMappingID]; //Update new value
        //                            prevInpTypeMapping.UAITM_IsDeleted = false;
        //                            prevInpTypeMapping.UAITM_ModifiedBy = loggedInUserID;
        //                            prevInpTypeMapping.UAITM_ModifiedOn = DateTime.Now;

        //                            prevItmAttrMappingIDs.Add(prevInpTypeMapping.UAITM_UniversalItemAttributeMappingID);
        //                        }
        //                        else
        //                        {
        //                            prevInpTypeMapping.UAITM_IsDeleted = true;
        //                            prevInpTypeMapping.UAITM_ModifiedBy = loggedInUserID;
        //                            prevInpTypeMapping.UAITM_ModifiedOn = DateTime.Now;
        //                        }
        //                    }
        //                }


        //                List<UniversalAttributeInputTypeMapping> lstNewAttrInputMapping = uamData.UniversalAttributeInputTypeMappings.Where(x => !prevItmAttrMappingIDs.Contains(x.UAITM_UniversalItemAttributeMappingID) && !x.UAITM_IsDeleted).ToList();
        //                foreach (UniversalAttributeInputTypeMapping item in lstNewAttrInputMapping)
        //                {
        //                    UniversalAttributeInputTypeMapping obj = new UniversalAttributeInputTypeMapping();
        //                    obj.UAITM_InputPriority = item.UAITM_InputPriority;
        //                    obj.UAITM_UniversalItemAttributeMappingID = item.UAITM_UniversalItemAttributeMappingID;
        //                    obj.UAITM_UniversalAttributeMappingID = item.UAITM_UniversalAttributeMappingID;
        //                    obj.UAITM_IsDeleted = false;
        //                    obj.UAITM_CreatedBy = loggedInUserID;
        //                    obj.UAITM_CreatedOn = DateTime.Now;
        //                    uamDataInDB.UniversalAttributeInputTypeMappings.Add(obj);
        //                }
        //                #endregion

        //                #region UAT:2402 COMPLIANCE ATTRIBUTE OPTION
        //                if (!uamData.UniversalAttributeOptionMappings.IsNullOrEmpty())
        //                {
        //                    List<UniversalAttributeOptionMapping> lstPrevAttrOptionMappings = uamDataInDB.UniversalAttributeOptionMappings.Where(x => !x.UAOM_IsDeleted).ToList();

        //                    List<Int32> prevOptionMappingIDs = new List<Int32>();
        //                    Dictionary<Int32?, Int32> dicAttrOptionMapping = uamData.UniversalAttributeOptionMappings.ToDictionary(x => x.UAOM_AttributeOptionID, x => x.UAOM_UniversalAttributeOptionID);

        //                    if (!lstPrevAttrOptionMappings.IsNullOrEmpty())
        //                    {
        //                        foreach (UniversalAttributeOptionMapping prevAttrOptionMapping in lstPrevAttrOptionMappings)
        //                        {
        //                            if (dicAttrOptionMapping.ContainsKey(prevAttrOptionMapping.UAOM_AttributeOptionID))
        //                            {
        //                                UPDATE EXISTING INPUT ATTRIBUTE MAPPINGS
        //                                prevAttrOptionMapping.UAOM_UniversalAttributeOptionID = dicAttrOptionMapping[prevAttrOptionMapping.UAOM_AttributeOptionID]; //Update new value
        //                                prevAttrOptionMapping.UAOM_IsDeleted = false;
        //                                prevAttrOptionMapping.UAOM_ModifiedBy = loggedInUserID;
        //                                prevAttrOptionMapping.UAOM_ModifiedOn = DateTime.Now;

        //                                prevOptionMappingIDs.Add(prevAttrOptionMapping.UAOM_AttributeOptionID.Value);
        //                            }
        //                            else
        //                            {
        //                                prevAttrOptionMapping.UAOM_IsDeleted = true;
        //                                prevAttrOptionMapping.UAOM_ModifiedBy = loggedInUserID;
        //                                prevAttrOptionMapping.UAOM_ModifiedOn = DateTime.Now;
        //                            }
        //                        }
        //                    }


        //                    List<UniversalAttributeOptionMapping> lstNewAttrOptionMapping = uamData.UniversalAttributeOptionMappings.Where(x => !prevOptionMappingIDs.Contains(x.UAOM_AttributeOptionID.Value) && !x.UAOM_IsDeleted).ToList();
        //                    foreach (UniversalAttributeOptionMapping attrOptnMapping in lstNewAttrOptionMapping)
        //                    {
        //                        UniversalAttributeOptionMapping obj = new UniversalAttributeOptionMapping();
        //                        obj.UAOM_UniversalAttributeOptionID = attrOptnMapping.UAOM_UniversalAttributeOptionID;
        //                        obj.UAOM_AttributeOptionID = attrOptnMapping.UAOM_AttributeOptionID;
        //                        obj.UAOM_UniversalAttributeMappingID = attrOptnMapping.UAOM_UniversalAttributeMappingID;
        //                        obj.UAOM_IsDeleted = false;
        //                        obj.UAOM_CreatedBy = loggedInUserID;
        //                        obj.UAOM_CreatedOn = DateTime.Now;
        //                        uamDataInDB.UniversalAttributeOptionMappings.Add(obj);
        //                    }
        //                }
        //                else
        //                {
        //                    uamDataInDB.UniversalAttributeOptionMappings.Where(cond => !cond.UAOM_IsDeleted).ForEach(attrOptn =>
        //                    {
        //                        attrOptn.UAOM_IsDeleted = true;
        //                        attrOptn.UAOM_ModifiedBy = loggedInUserID;
        //                        attrOptn.UAOM_ModifiedOn = DateTime.Now;
        //                    });
        //                }
        //                #endregion
        //            }
        //        }
        //    }
        //    else
        //    {
        //        uamData.UAM_CreatedBy = loggedInUserID;
        //        uamData.UAM_CreatedOn = DateTime.Now;

        //        List<UniversalAttributeInputTypeMapping> lstAttrInputTypeMappings = uamData.UniversalAttributeInputTypeMappings.ToList();
        //        foreach (UniversalAttributeInputTypeMapping attrInpTypeMapping in lstAttrInputTypeMappings)
        //        {
        //            attrInpTypeMapping.UAITM_CreatedBy = loggedInUserID;
        //            attrInpTypeMapping.UAITM_CreatedOn = DateTime.Now;
        //        }

        //        List<UniversalAttributeOptionMapping> lstAttrOptionMappings = uamData.UniversalAttributeOptionMappings.ToList();
        //        foreach (UniversalAttributeOptionMapping attrAttrOptnMapping in lstAttrOptionMappings)
        //        {
        //            attrAttrOptnMapping.UAOM_CreatedBy = loggedInUserID;
        //            attrAttrOptnMapping.UAOM_CreatedOn = DateTime.Now;
        //        }

        //        _dbContext.UniversalAttributeMappings.AddObject(uamData);
        //    }
        //    if (_dbContext.SaveChanges() > AppConsts.NONE)
        //        return true;
        //    return false;
        //}

        #endregion

        #region UAT-2402
        List<UniversalAttributeOption> IUniversalMappingDataRepository.GetUniversalAtrOptionData(Int32 uniItmAtrMappingID)
        {
            return base.SharedDataDBContext.UniversalAttributeOptions.Where(cond => cond.UAO_UniversalItemAttributeMappingID == uniItmAtrMappingID && !cond.UAO_IsDeleted).ToList();
        }

        List<UniversalFieldOption> IUniversalMappingDataRepository.GetUniversalFieldAtrOptionData(Int32 universalFieldID)
        {
            return base.SharedDataDBContext.UniversalFieldOptions.Where(cond => cond.UFO_UniversalFieldID == universalFieldID && !cond.UFO_IsDeleted && !cond.UniversalField.UF_IsDeleted).ToList();
        }
        List<UniversalRequirementAttributeOptionMapping> IUniversalMappingDataRepository.GetUniversalAtrOptionSelected(Int32 reqFieldOptionID, Int32 uniReqAtrMappingID)
        {
            return base.SharedDataDBContext.UniversalRequirementAttributeOptionMappings.Where(cond => cond.URAOM_RequirementFieldOptionID == reqFieldOptionID
                                                && cond.URAOM_UniversalReqAttributeMappingID == uniReqAtrMappingID
                                                && !cond.URAOM_IsDeleted).ToList();
        }

        List<Entity.SharedDataEntity.UniversalFieldOptionMapping> IUniversalMappingDataRepository.GetUniversalFieldAtrOptionSelected(Int32 uniFieldOptionID, Int32 uniFieldMappingID)
        {
            return base.SharedDataDBContext.UniversalFieldOptionMappings.Where(cond => cond.UFOM_AttributeOptionID == uniFieldOptionID
                                                && cond.UFOM_UniversalFieldMappingID == uniFieldMappingID
                                                && !cond.UFOM_IsDeleted).ToList();
        }

        List<UniversalAttributeOptionMapping> IUniversalMappingDataRepository.GetUniversalAttributeOptionMapping(Int32 uniAttributeMappingId)
        {
            return _dbContext.UniversalAttributeOptionMappings.Where(x => x.UAOM_UniversalAttributeMappingID == uniAttributeMappingId && !x.UAOM_IsDeleted).ToList();
        }
        #endregion

        #region UAT-2402
        List<UniversalAttributeOption> IUniversalMappingDataRepository.GetUniversalAttributeOptionsByID(int universalItemAttrMappingID)
        {
            return base.SharedDataDBContext.UniversalAttributeOptions.Where(x => x.UAO_UniversalItemAttributeMappingID == universalItemAttrMappingID
                                                                                && !x.UAO_IsDeleted).ToList();
        }

        List<Entity.SharedDataEntity.UniversalFieldOption> IUniversalMappingDataRepository.GetUniversalFieldeOptionsByID(int universalFieldID)
        {
            return base.SharedDataDBContext.UniversalFieldOptions.Where(x => x.UFO_UniversalFieldID == universalFieldID
                                                                                && !x.UFO_IsDeleted).ToList();
        }
        Boolean IUniversalMappingDataRepository.SaveUniverAttrOptionMapping(List<UniversalAttributeOptionMapping> lstUniAttrOptMapping, Int32 uniAttributeMapId, Int32 currentLoggedInUserId)
        {
            List<UniversalAttributeOptionMapping> lstAttributeOptionMappingInDB = _dbContext.UniversalAttributeOptionMappings.Where(x =>
                                                                                      x.UAOM_UniversalAttributeMappingID == uniAttributeMapId && !x.UAOM_IsDeleted).ToList();
            if (!lstAttributeOptionMappingInDB.IsNullOrEmpty())
            {
                //List<Int32> attrOptMappingIDToMap = lstUniAttrOptMapping.Select(slct => slct.UAOM_UniversalAttributeOptionID).ToList();
                //var mappingToDelete = lstAttributeOptionMappingInDB.Where(cnd => !attrOptMappingIDToMap.Contains(cnd.UAOM_UniversalAttributeOptionID)).ToList();
                //lstUniAttrOptMapping.ForEach(AttrOption =>
                //{
                //    var existInDB = lstAttributeOptionMappingInDB.FirstOrDefault(cond => cond.UAOM_UniversalAttributeOptionID == AttrOption.UAOM_UniversalAttributeOptionID 
                //                                                                 && cond.UAOM_AttributeOptionID == AttrOption.UAOM_AttributeOptionID);
                //    if (!existInDB.IsNullOrEmpty())
                //    {
                //        existInDB.UAOM_AttributeOptionID = AttrOption.UAOM_AttributeOptionID;
                //        existInDB.UAOM_ModifiedBy = currentLoggedInUserId;
                //        existInDB.UAOM_ModifiedOn = DateTime.Now;
                //    }
                //    else
                //    {
                //        _dbContext.UniversalAttributeOptionMappings.AddObject(AttrOption);
                //    }
                //});

                lstAttributeOptionMappingInDB.ForEach(dlt =>
                {
                    dlt.UAOM_IsDeleted = true;
                    dlt.UAOM_ModifiedBy = currentLoggedInUserId;
                    dlt.UAOM_ModifiedOn = DateTime.Now;
                });
            }

            lstUniAttrOptMapping.ForEach(attrOpt =>
            {
                _dbContext.UniversalAttributeOptionMappings.AddObject(attrOpt);
            });

            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        private void DeleteAttributeOptionMapping(List<Int32> universalAttributeMappingIDs, Int32 currentLoggedInuserId)
        {
            List<UniversalAttributeOptionMapping> lstAttributeOptionMapping = _dbContext.UniversalAttributeOptionMappings.Where(cnd =>
                                                                                      universalAttributeMappingIDs.Contains(cnd.UAOM_UniversalAttributeMappingID)
                                                                                               && !cnd.UAOM_IsDeleted).ToList();

            if (!lstAttributeOptionMapping.IsNullOrEmpty())
            {
                lstAttributeOptionMapping.ForEach(attr =>
                {
                    attr.UAOM_IsDeleted = true;
                    attr.UAOM_ModifiedOn = DateTime.Now;
                    attr.UAOM_ModifiedBy = currentLoggedInuserId;
                });
            }
        }

        Boolean IUniversalMappingDataRepository.AddApprovedItemsToCopyDataQueue(Int32 complianceItemId, Int32 complianceCategoryId, Int32 currentLoggedInUserId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@CurrentLoggedInUserId", currentLoggedInUserId),
                              new SqlParameter("@ComplianceItemId", complianceItemId),
                              new SqlParameter("@ComplianceCategoryId", complianceCategoryId)
                        };

                base.OpenSQLDataReaderConnection(con);

                base.ExecuteSQLDataReader(con, "usp_CopyApprovedItemsInCopyDataQueue", sqlParameterCollection);

                base.CloseSQLDataReaderConnection(con);
            }
            return true;
        }


        //UAT-3716
        Boolean IUniversalMappingDataRepository.AddApprovedPkgsToCopyDataQueue(Int32 CurrentPackageId, Int32 currentLoggedInUserId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@CurrentLoggedInID", currentLoggedInUserId),
                              new SqlParameter("@CompliancePackageID", CurrentPackageId)
                        };

                base.OpenSQLDataReaderConnection(con);

                base.ExecuteSQLDataReader(con, "usp_CopyTrackingPackageDataToRequirement", sqlParameterCollection);

                base.CloseSQLDataReaderConnection(con);
            }
            return true;
        }
        //END UAT-3716


        List<UniversalAttributeMapping> IUniversalMappingDataRepository.GetMappedUniversalAttributesByItemMappingID(Int32 uniItemMapId)
        {
            return _dbContext.UniversalAttributeMappings.Where(cond => cond.UAM_UniversalItemMappingID == uniItemMapId && !cond.UAM_IsDeleted).ToList();
        }
        #endregion

        #region Çategory Copy
        Boolean IUniversalMappingDataRepository.CopyUniversalDataByCategoryIds(Int32 currentAddedCategoryID, Int32 requirementCatyegoryID, Int32 currentLoggedInUserID)
        {
            EntityConnection connection = base.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                con.Open();

                SqlCommand command = new SqlCommand("usp_CopyUniversalDataByCategoryIds", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CurrentLoggedInUserId", currentLoggedInUserID);
                command.Parameters.AddWithValue("@CurrentCatoryID", currentAddedCategoryID);
                command.Parameters.AddWithValue("@OldCategoryID", requirementCatyegoryID);
                //Commented code related to UAT-2985
                //command.Parameters.AddWithValue("@UniversalCategoryID", universalCategoryID);

                command.ExecuteNonQuery();

                con.Close();
            }
            return true;
        }
        #endregion

        List<UniversalField> IUniversalMappingDataRepository.GetUniversalAttributeField()
        {
            List<UniversalField> universalFields = base.SharedDataDBContext.UniversalFields.Where(cond => !cond.UF_IsDeleted).ToList();
            if (!universalFields.IsNullOrEmpty())
                return universalFields;
            return new List<UniversalField>();
        }

        Boolean IUniversalMappingDataRepository.DeleteUniversalFieldByID(Int32 uniFieldID, Int32 cuurentLoggedInUserID)
        {

            UniversalField universalField = base.SharedDataDBContext.UniversalFields.Where(cond => !cond.UF_IsDeleted
                                                                               && cond.UF_ID == uniFieldID).FirstOrDefault();
            if (!universalField.IsNullOrEmpty())
            {
                universalField.UF_IsDeleted = true;
                universalField.UF_ModifiedBy = cuurentLoggedInUserID;
                universalField.UF_ModifiedOn = DateTime.Now;

                List<Entity.SharedDataEntity.UniversalFieldMapping> UFM = base.SharedDataDBContext.UniversalFieldMappings.Where(cond => !cond.UFM_IsDeleted && cond.UFM_UniversalFieldID == uniFieldID).ToList();

                if (!UFM.IsNullOrEmpty() && UFM.Count > 0)
                {
                    foreach (Entity.SharedDataEntity.UniversalFieldMapping item in UFM)
                    {
                        item.UFM_IsDeleted = true;
                        item.UFM_ModifiedBy = cuurentLoggedInUserID;
                        item.UFM_ModifiedOn = DateTime.Now;
                    }
                }
            }

            if (base.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IUniversalMappingDataRepository.SaveUpdateUniversalField(UniversalField universalField, Int32 cuurentLoggedInUserID)
        {
            //if (!universalField.IsNullOrEmpty() && universalField.UF_ID > AppConsts.NONE)
            //{
            //    UniversalField ucmDataInDB = base.SharedDataDBContext.UniversalFields.FirstOrDefault(cond => cond.UF_ID == universalField.UF_ID && !cond.UF_IsDeleted);
            //    if (!ucmDataInDB.IsNullOrEmpty())
            //    {
            //        ucmDataInDB.UF_Name = universalField.UF_Name;
            //        ucmDataInDB.UF_AttributeDataTypeID = universalField.UF_AttributeDataTypeID;
            //        ucmDataInDB.UF_ModifiedBy = universalField.UF_ModifiedBy;
            //        ucmDataInDB.UF_ModifiedOn = universalField.UF_ModifiedOn;
            //        ucmDataInDB.UF_IsDeleted = false;
            //    }
            //}

            //else
            //{
            //    universalField.UF_CreatedOn = DateTime.Now;
            //    universalField.UF_CreatedBy = cuurentLoggedInUserID;
            //    universalField.UF_IsDeleted = false;
            //    base.SharedDataDBContext.UniversalFields.AddObject(universalField);
            //}
            //if (base.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
            //    return true;
            //return false;

            if (!universalField.IsNullOrEmpty() && universalField.UF_ID == AppConsts.NONE)
            {
                base.SharedDataDBContext.UniversalFields.AddObject(universalField);
            }

            if (base.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        UniversalField IUniversalMappingDataRepository.GetUniversalFieldById(Int32 uniFieldId)
        {
            return base.SharedDataDBContext.UniversalFields.Where(cond => cond.UF_ID == uniFieldId && !cond.UF_IsDeleted).FirstOrDefault();
        }

        Entity.SharedDataEntity.UniversalFieldMapping IUniversalMappingDataRepository.GetUniversalAttributeMappingByReqCatItemFieldID(Int32 ReqItmID, Int32 ReqFieldId, Int32 ReqCatID = 0)
        {

            Int32 ReqCatItmMappingID = AppConsts.NONE;
            Int32 ReqItmFieldMappingID = AppConsts.NONE;
            if (ReqCatID > AppConsts.NONE)
            {
                ReqCatItmMappingID = base.SharedDataDBContext.RequirementCategoryItems.Where(cond => cond.RCI_RequirementItemID == ReqItmID
                                            && !cond.RCI_IsDeleted && cond.RCI_RequirementCategoryID == ReqCatID).FirstOrDefault().RCI_ID;
            }
            else
            {
                ReqCatItmMappingID = base.SharedDataDBContext.RequirementCategoryItems.Where(cond => cond.RCI_RequirementItemID == ReqItmID
                                            && !cond.RCI_IsDeleted).FirstOrDefault().RCI_ID;
            }

            if (ReqFieldId > AppConsts.NONE && ReqItmID > AppConsts.NONE)
            {
                ReqItmFieldMappingID = base.SharedDataDBContext.RequirementItemFields.Where(cond => cond.RIF_RequirementItemID == ReqItmID
                                            && !cond.RIF_IsDeleted && cond.RIF_RequirementFieldID == ReqFieldId).FirstOrDefault().RIF_ID;
            }

            if (ReqCatItmMappingID > AppConsts.NONE && ReqItmFieldMappingID > AppConsts.NONE)
            {
                Entity.SharedDataEntity.UniversalFieldMapping URIM = base.SharedDataDBContext.UniversalFieldMappings.Where(cond => !cond.UFM_IsDeleted
                                              && cond.UFM_CategoryItemMappingID == ReqCatItmMappingID && cond.UFM_ItemAttributeMappingID == ReqItmFieldMappingID).FirstOrDefault();
                return URIM;
            }

            return new Entity.SharedDataEntity.UniversalFieldMapping();
        }

        List<UniversalFieldOption> IUniversalMappingDataRepository.GetUniversalAttributeOptionData(Int32 universalFieldMappingID)
        {
            //return base.SharedDataDBContext.UniversalAttributeOptions.Where(cond => cond.UAO_UniversalItemAttributeMappingID == uniItmAtrMappingID && !cond.UAO_IsDeleted).ToList();

            List<Int32?> universalFieldOptionMapping = base.SharedDataDBContext.UniversalFieldOptionMappings.Where(cond => cond.UFOM_UniversalFieldMappingID == universalFieldMappingID && !cond.UFOM_IsDeleted).Select(x => x.UFOM_AttributeOptionID).ToList();

            if (universalFieldOptionMapping.IsNotNull())
            {
                return base.SharedDataDBContext.UniversalFieldOptions.Where(cond => universalFieldOptionMapping.Contains(cond.UFO_ID) && !cond.UFO_IsDeleted).ToList();
            }

            return new List<UniversalFieldOption>();
        }

        List<UniversalFieldOption> IUniversalMappingDataRepository.GetUniversalFieldOptionData(Int32 universalFieldID)
        {
            return base.SharedDataDBContext.UniversalFieldOptions.Where(cond => cond.UFO_UniversalFieldID == universalFieldID && !cond.UFO_IsDeleted).ToList();
        }

        Entity.ClientEntity.UniversalFieldMapping IUniversalMappingDataRepository.GetComplianceTypeUniversalFieldMapping(Int32 complianceCategoryItemID, Int32 complianceItemAttributeID, Int32 complianceMappingTypeID)
        {
            if (complianceCategoryItemID > 0 && complianceItemAttributeID > 0)
            {
                return _dbContext.UniversalFieldMappings.Where(cond => cond.UFM_CategoryItemMappingID == complianceCategoryItemID
                                                                    && cond.UFM_ItemAttributeMappingID == complianceItemAttributeID
                                                                    && !cond.UFM_IsDeleted
                                                                    && cond.UFM_UniversalMappingTypeID == complianceMappingTypeID
                                                                ).FirstOrDefault();

            }

            return null;
        }

        Boolean IUniversalMappingDataRepository.SaveUpdateUniversalFieldMapping(Entity.ClientEntity.UniversalFieldMapping ufmData)
        {
            if (!ufmData.IsNullOrEmpty() && ufmData.UFM_ID > AppConsts.NONE)
            {
                Entity.ClientEntity.UniversalFieldMapping ufmDataInDB = _dbContext.UniversalFieldMappings.FirstOrDefault(cond => cond.UFM_ID == ufmData.UFM_ID && !cond.UFM_IsDeleted);

                if (!ufmDataInDB.IsNullOrEmpty())
                {
                    ufmDataInDB.UFM_CategoryItemMappingID = ufmData.UFM_CategoryItemMappingID;
                    ufmDataInDB.UFM_ItemAttributeMappingID = ufmData.UFM_ItemAttributeMappingID;
                    ufmDataInDB.UFM_UniversalFieldID = ufmData.UFM_UniversalFieldID;
                    ufmDataInDB.UFM_ModifiedBy = ufmData.UFM_ModifiedBy;
                    ufmDataInDB.UFM_ModifiedOn = ufmData.UFM_ModifiedOn;
                }
            }
            else
            {
                ufmData.UFM_ModifiedBy = null;
                ufmData.UFM_ModifiedOn = null;
                _dbContext.UniversalFieldMappings.AddObject(ufmData);
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IUniversalMappingDataRepository.DeleteUniversalFieldMapping(Int32 universalFieldMappingID, Int32 currentLoggedInuserId)
        {
            Entity.ClientEntity.UniversalFieldMapping fieldMapping = _dbContext.UniversalFieldMappings.FirstOrDefault(cnd => cnd.UFM_ID == universalFieldMappingID && !cnd.UFM_IsDeleted);

            if (!fieldMapping.IsNullOrEmpty())
            {
                fieldMapping.UFM_IsDeleted = true;
                fieldMapping.UFM_ModifiedOn = DateTime.Now;
                fieldMapping.UFM_ModifiedBy = currentLoggedInuserId;
                List<Int32> lstMappedFieldIDs = new List<Int32>();
                lstMappedFieldIDs.Add(fieldMapping.UFM_ID);
                DeleteFieldInputMapping(lstMappedFieldIDs, currentLoggedInuserId);
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        private void DeleteFieldInputMapping(List<Int32> lstMappedFieldIDs, Int32 currentLoggedInuserId)
        {
            List<Entity.ClientEntity.UniversalFieldInputTypeMapping> lstFieldInputMapping = _dbContext.UniversalFieldInputTypeMappings.Where(cnd =>
                                                                                      lstMappedFieldIDs.Contains(cnd.UFITM_UniversalFieldMappingID)
                                                                                               && !cnd.UFITM_IsDeleted).ToList();

            if (!lstFieldInputMapping.IsNullOrEmpty())
            {
                lstFieldInputMapping.ForEach(attr =>
                {
                    attr.UFITM_IsDeleted = true;
                    attr.UFITM_ModifiedOn = DateTime.Now;
                    attr.UFITM_ModifiedBy = currentLoggedInuserId;
                });
            }
        }

        Boolean IUniversalMappingDataRepository.SaveUniversalFieldOptionMapping(List<Entity.ClientEntity.UniversalFieldOptionMapping> lstUniFieldOptMapping, Int32 uniFieldMappingId, Int32 currentLoggedInUserId)
        {
            List<Entity.ClientEntity.UniversalFieldOptionMapping> lstFieldOptionMappingInDB = _dbContext.UniversalFieldOptionMappings.Where(x =>
                                                                                      x.UFOM_UniversalFieldMappingID == uniFieldMappingId && !x.UFOM_IsDeleted).ToList();
            if (!lstFieldOptionMappingInDB.IsNullOrEmpty())
            {
                lstFieldOptionMappingInDB.ForEach(dlt =>
                {
                    dlt.UFOM_IsDeleted = true;
                    dlt.UFOM_ModifiedBy = currentLoggedInUserId;
                    dlt.UFOM_ModifiedOn = DateTime.Now;
                });
            }

            lstUniFieldOptMapping.ForEach(attrOpt =>
            {
                _dbContext.UniversalFieldOptionMappings.AddObject(attrOpt);
            });

            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IUniversalMappingDataRepository.UpdateFieldInputMapping(Int32 uniFieldMappingId, List<Entity.ClientEntity.UniversalFieldInputTypeMapping> lstInputMapping, Int32 currentLoggedInUserId)
        {
            List<Entity.ClientEntity.UniversalFieldInputTypeMapping> lstFieldInputMappingInDB = _dbContext.UniversalFieldInputTypeMappings.Where(x =>
                                                                                      x.UFITM_UniversalFieldMappingID == uniFieldMappingId && !x.UFITM_IsDeleted).ToList();
            if (!lstFieldInputMappingInDB.IsNullOrEmpty())
            {
                List<Int32> inputMappingIDToMap = lstInputMapping.Select(slct => slct.UFITM_UniversalFieldID).ToList();
                var mappingToDelete = lstFieldInputMappingInDB.Where(cnd => !inputMappingIDToMap.Contains(cnd.UFITM_UniversalFieldID)).ToList();

                lstInputMapping.ForEach(inputAttrType =>
                {
                    var existInDB = lstFieldInputMappingInDB.FirstOrDefault(cond => cond.UFITM_UniversalFieldID == inputAttrType.UFITM_UniversalFieldID);
                    if (!existInDB.IsNullOrEmpty())
                    {
                        existInDB.UFITM_InputPriority = inputAttrType.UFITM_InputPriority;
                        existInDB.UFITM_ModifiedBy = currentLoggedInUserId;
                        existInDB.UFITM_ModifiedOn = DateTime.Now;
                    }
                    else
                    {
                        inputAttrType.UFITM_UniversalFieldMappingID = uniFieldMappingId;
                        _dbContext.UniversalFieldInputTypeMappings.AddObject(inputAttrType);
                    }
                });

                mappingToDelete.ForEach(dlt =>
                {
                    dlt.UFITM_IsDeleted = true;
                    dlt.UFITM_ModifiedBy = currentLoggedInUserId;
                    dlt.UFITM_ModifiedOn = DateTime.Now;
                });
            }
            else
            {
                lstInputMapping.ForEach(inputAttr =>
                {
                    inputAttr.UFITM_UniversalFieldMappingID = uniFieldMappingId;
                    _dbContext.UniversalFieldInputTypeMappings.AddObject(inputAttr);
                });
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IUniversalMappingDataRepository.IsAnyAttributeMappingExists(Int32 categoryItemMappingID)
        {
            return _dbContext.UniversalFieldMappings.Any(cond => cond.UFM_CategoryItemMappingID == categoryItemMappingID && !cond.UFM_IsDeleted);
        }

        Boolean IUniversalMappingDataRepository.IsUniversalFieldNameExists(String universalFieldName)
        {
            UniversalField universalField = base.SharedDataDBContext.UniversalFields
                                                .FirstOrDefault(item => !item.UF_IsDeleted
                                                                            && item.UF_Name.Trim().ToLower().Equals(universalFieldName.Trim().ToLower()));
            if (!universalField.IsNullOrEmpty())
                return true;
            else
                return false;

        }





    }
}
