using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.Utils;
using INTSOF.UI.Contract.RotationPackages;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using CoreWeb.IntsofLoggerModel.Interface;
using System.Data.Entity;

namespace DAL.Repository
{
    public class RequirementRuleRepository : ClientBaseRepository, IRequirementRuleRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _dbContext;

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public RequirementRuleRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }

        #region Requirement Package Rules Execution

        /// <summary>
        /// Execute rules after saving Verification detail screen data.
        /// </summary>
        /// <param name="ruleObjectXML"></param>
        /// <param name="reqSubscriptionId"></param>
        /// <param name="systemUserId"></param>
        void IRequirementRuleRepository.ExecuteRequirementObjectBuisnessRules(String ruleObjectXML, Int32 reqSubscriptionId, Int32 systemUserId)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand command = new SqlCommand("usp_Rule_EvaluateRequirementFixedBusinessRules", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RequirementSubscriptionID", reqSubscriptionId);
                command.Parameters.AddWithValue("@Objects", ruleObjectXML);
                command.Parameters.AddWithValue("@SystemUserID", systemUserId);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        void IRequirementRuleRepository.EvaluateRequirementPostSubmitRules(String ruleObjectXML, Int32 reqSubscriptionId, Int32 systemUserId)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand command = new SqlCommand("usp_Rule_EvaluateRequirementPostSubmitRules", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RequirementSubscriptionID", reqSubscriptionId);
                command.Parameters.AddWithValue("@Objects", ruleObjectXML);
                command.Parameters.AddWithValue("@SystemUserID", systemUserId);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        #endregion

        List<RequirementExpiryContract> IRequirementRuleRepository.SetRequirementItemExpiry(Int32 chunkSize, Int32 systemUserId)
        {
            var lstRequirementExpiryContract = new List<RequirementExpiryContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@chunkSize", chunkSize), 
                           new SqlParameter("@userId", systemUserId)
                        };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_SetRequirementItemExpiry", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var requirementExpiryContract = new RequirementExpiryContract();
                            requirementExpiryContract.RequirementSubId = dr["RequirementSubId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementSubId"]);
                            requirementExpiryContract.RequirementPkgId = dr["RequirementPkgId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementPkgId"]);
                            requirementExpiryContract.RequirementCatId = dr["RequirementCatId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementCatId"]);
                            requirementExpiryContract.RequirementItemId = dr["RequirementItemId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementItemId"]);
                            requirementExpiryContract.RequirementItemDataId = dr["ItemDataId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ItemDataId"]);
                            requirementExpiryContract.RequirementPkgSubStatusCode = dr["CurrentPkgSubStatusCode"] == DBNull.Value ? string.Empty : Convert.ToString(dr["CurrentPkgSubStatusCode"]);
                            requirementExpiryContract.IsNewPackage = Convert.ToBoolean(dr["IsNewPackage"]);
                            lstRequirementExpiryContract.Add(requirementExpiryContract);
                        }
                    }
                }
            }
            return lstRequirementExpiryContract;
        }

        #region Private Methods

        /// <summary>
        /// Get conenction string from the Context
        /// </summary>
        /// <returns></returns>
        private String GetConnectionString()
        {
            return (_dbContext.Connection as System.Data.Entity.Core.EntityClient.EntityConnection).StoreConnection.ConnectionString;
        }

        #endregion

        List<RequirementScheduledAction> IRequirementRuleRepository.GetActiveScheduleActionList(Int32 chunkSize, String actionType)
        {
            return ClientDBContext.RequirementScheduledActions.Where(cond => cond.RSA_IsActive && !cond.RSA_IsDeleted
                                                                    && cond.RSA_ScheduleDate <= DateTime.Now
                                                                    && cond.lkpRotScheduledActionType.RSAT_Code == actionType)
                                                                    .Take(chunkSize).ToList();
        }

        /// <summary>
        /// Execute rules for Schedule action.
        /// </summary>
        /// <param name="ruleObjectXML"></param>
        /// <param name="reqSubscriptionId"></param>
        /// <param name="systemUserId"></param>
        List<RequirementPackageSubscriptionContract> IRequirementRuleRepository.ExecuteRequirementCategoryScheduleAction(Int32 systemUserId, Int32 chunkSize, String ScheduleActionTypeCode)
        {
            List<RequirementPackageSubscriptionContract> lstRequirementPackageSubscription = new List<RequirementPackageSubscriptionContract>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                #region Old Code
                //SqlCommand command = new SqlCommand("Usp_ExecuteRequirementCategoryScheduleAction", con);
                //command.CommandType = CommandType.StoredProcedure;

                //command.Parameters.AddWithValue("@chunkSize", chunkSize);
                //command.Parameters.AddWithValue("@SystemUserID", systemUserId);
                //command.Parameters.AddWithValue("@ScheduleActionTypeCode", ScheduleActionTypeCode);
                //if (con.State == ConnectionState.Closed)
                //{
                //    con.Open();
                //}
                //command.ExecuteNonQuery();
                //con.Close();
                #endregion

                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@chunkSize", chunkSize), 
                           new SqlParameter("@SystemUserID", systemUserId),
                           new SqlParameter("@ScheduleActionTypeCode", ScheduleActionTypeCode)
                        };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "Usp_ExecuteRequirementCategoryScheduleAction", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var requirementPackageSubscription = new RequirementPackageSubscriptionContract();
                            requirementPackageSubscription.RequirementPackageSubscriptionID = dr["ReqPackageSubscriptionID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ReqPackageSubscriptionID"]);
                            requirementPackageSubscription.RequirementPackageSubscriptionStatusId = dr["PerviousReqPkgSubStatusID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["PerviousReqPkgSubStatusID"]);
                            requirementPackageSubscription.RequirementPackageSubscriptionStatusCode = dr["PerviousReqPkgSubStatusCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PerviousReqPkgSubStatusCode"]);
                            lstRequirementPackageSubscription.Add(requirementPackageSubscription);
                        }
                    }
                }
            }
            return lstRequirementPackageSubscription;
        }

        /// <summary>
        /// Execute rules for Schedule action.
        /// </summary>
        /// <param name="ruleObjectXML"></param>
        /// <param name="reqSubscriptionId"></param>
        /// <param name="systemUserId"></param>
        List<RequirementPackageSubscriptionContract> IRequirementRuleRepository.ExecuteRequirementPackageScheduleAction(Int32 systemUserId, Int32 chunkSize)
        {
            List<RequirementPackageSubscriptionContract> lstRequirementPackageSubscription = new List<RequirementPackageSubscriptionContract>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                #region OLD CODE
                //SqlCommand command = new SqlCommand("Usp_ExecuteRequirementPackageScheduleAction", con);
                //command.CommandType = CommandType.StoredProcedure;

                //command.Parameters.AddWithValue("@chunkSize", chunkSize);
                //command.Parameters.AddWithValue("@SystemUserID", systemUserId);
                //if (con.State == ConnectionState.Closed)
                //{
                //    con.Open();
                //}
                //command.ExecuteNonQuery();
                //con.Close();
                #endregion

                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@chunkSize", chunkSize), 
                           new SqlParameter("@SystemUserID", systemUserId)
                        };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "Usp_ExecuteRequirementPackageScheduleAction", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var requirementPackageSubscription = new RequirementPackageSubscriptionContract();
                            requirementPackageSubscription.RequirementPackageSubscriptionID = dr["ReqPackageSubscriptionID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ReqPackageSubscriptionID"]);
                            requirementPackageSubscription.RequirementPackageSubscriptionStatusId = dr["PerviousReqPkgSubStatusID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["PerviousReqPkgSubStatusID"]);
                            requirementPackageSubscription.RequirementPackageSubscriptionStatusCode = dr["PerviousReqPkgSubStatusCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PerviousReqPkgSubStatusCode"]);
                            lstRequirementPackageSubscription.Add(requirementPackageSubscription);
                        }
                    }
                }
            }
            return lstRequirementPackageSubscription;
        }
        #region Rule template

        Boolean IRequirementRuleRepository.CopyRuleTemplate(Entity.SharedDataEntity.RequirementRuleTemplate ruleTemp, String RuleTemplateName, Int32 CurrentLoggedInUserId)
        {
            Entity.SharedDataEntity.RequirementRuleTemplate newRuleTemplate = new Entity.SharedDataEntity.RequirementRuleTemplate();

            newRuleTemplate.RLT_Name = RuleTemplateName;
            newRuleTemplate.RLT_Code = Guid.NewGuid();
            newRuleTemplate.RLT_UIExpression = ruleTemp.RLT_UIExpression;
            newRuleTemplate.RLT_Description = ruleTemp.RLT_Description;
            newRuleTemplate.RLT_ResultType = ruleTemp.RLT_ResultType;
            newRuleTemplate.RLT_ObjectCount = ruleTemp.RLT_ObjectCount;
            newRuleTemplate.RLT_Notes = ruleTemp.RLT_Notes;
            newRuleTemplate.RLT_IsActive = ruleTemp.RLT_IsActive;
            newRuleTemplate.RLT_IsDeleted = ruleTemp.RLT_IsDeleted;
            newRuleTemplate.RLT_CreatedByID = CurrentLoggedInUserId;
            newRuleTemplate.RLT_CreatedOn = DateTime.Now;
            newRuleTemplate.RLT_IsCopied = false;

            foreach (Entity.SharedDataEntity.RequirementRuleTemplateExpression ruleExpression in ruleTemp.RequirementRuleTemplateExpressions)
            {
                Entity.SharedDataEntity.RequirementRuleTemplateExpression newRuleTempExp = new Entity.SharedDataEntity.RequirementRuleTemplateExpression();
                newRuleTempExp.RLE_ExpressionOrder = ruleExpression.RLE_ExpressionOrder;
                newRuleTempExp.RLE_IsActive = ruleExpression.RLE_IsActive;
                newRuleTempExp.RLE_IsDeleted = ruleExpression.RLE_IsDeleted;
                newRuleTempExp.RLE_CreatedByID = CurrentLoggedInUserId;
                newRuleTempExp.RLE_CreatedOn = DateTime.Now;

                Entity.SharedDataEntity.RequirementExpression expressionToCopy = ruleExpression.RequirementExpression;
                Entity.SharedDataEntity.RequirementExpression newExpression = new Entity.SharedDataEntity.RequirementExpression();

                newExpression.REX_Name = expressionToCopy.REX_Name;
                newExpression.REX_Description = expressionToCopy.REX_Description;
                newExpression.REX_RequiremntExpression = expressionToCopy.REX_RequiremntExpression;
                newExpression.REX_ResultType = expressionToCopy.REX_ResultType;
                newExpression.REX_IsActive = expressionToCopy.REX_IsActive;
                newExpression.REX_IsDeleted = expressionToCopy.REX_IsDeleted;
                newExpression.REX_CreatedByID = CurrentLoggedInUserId;
                newExpression.REX_CreatedOn = DateTime.Now;

                newRuleTempExp.RequirementExpression = newExpression;

                newRuleTemplate.RequirementRuleTemplateExpressions.Add(newRuleTempExp);
            }

            base.SharedDataDBContext.RequirementRuleTemplates.AddObject(newRuleTemplate);

            if (base.SharedDataDBContext.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
        #region Rule Mapping

        Boolean IRequirementRuleRepository.AddRuleMapping(Entity.SharedDataEntity.RequirementObjectRule ruleMapping)
        {
            base.SharedDataDBContext.RequirementObjectRules.AddObject(ruleMapping);
            base.SharedDataDBContext.SaveChanges();

            #region UAT-2514
            return SaveRequirementPkgSyncLog(ruleMapping.ROR_CreatedByID, ruleMapping.ROR_ID);
            #endregion
        }

        Boolean IRequirementRuleRepository.UpdateRequirementRuleMapping(Entity.SharedDataEntity.RequirementObjectRule requirementObjectRule)
        {
            Entity.SharedDataEntity.RequirementObjectRule requirementRuleInDB = GetRequirementRuleMapping(requirementObjectRule.ROR_ID);
            if (!requirementRuleInDB.IsNullOrEmpty())
            {
                requirementRuleInDB.ROR_Name = requirementObjectRule.ROR_Name;
                requirementRuleInDB.ROR_RuleType = requirementObjectRule.ROR_RuleType;
                requirementRuleInDB.ROR_ActionType = requirementObjectRule.ROR_ActionType;
                requirementRuleInDB.ROR_IsActive = requirementObjectRule.ROR_IsActive;
                requirementRuleInDB.ROR_IsDeleted = requirementObjectRule.ROR_IsDeleted;
                requirementRuleInDB.ROR_ModifiedByID = requirementObjectRule.ROR_ModifiedByID;
                requirementRuleInDB.ROR_ModifiedOn = requirementObjectRule.ROR_ModifiedOn;
                requirementRuleInDB.ROR_SucessMessage = requirementObjectRule.ROR_SucessMessage;
                requirementRuleInDB.ROR_ErroMessage = requirementObjectRule.ROR_ErroMessage;
                requirementRuleInDB.ROR_UIExpression = requirementObjectRule.ROR_UIExpression;
                foreach (Entity.SharedDataEntity.RequirementObjectRuleDetail item in requirementObjectRule.RequirementObjectRuleDetails)
                {
                    Entity.SharedDataEntity.RequirementObjectRuleDetail ruleDetail = requirementRuleInDB.RequirementObjectRuleDetails.FirstOrDefault(obj => obj.RORD_PlaceHolderName == item.RORD_PlaceHolderName);
                    ruleDetail.RORD_PlaceHolderName = item.RORD_PlaceHolderName;
                    ruleDetail.RORD_RuleObjectMappingTypeID = item.RORD_RuleObjectMappingTypeID;
                    ruleDetail.RORD_ObjectTreeID = item.RORD_ObjectTreeID;
                    ruleDetail.RORD_ConstantType = item.RORD_ConstantType;
                    ruleDetail.RORD_ConstantValue = item.RORD_ConstantValue;
                    ruleDetail.RORD_IsDeleted = false;
                    ruleDetail.RORD_ModifiedByID = requirementObjectRule.ROR_ModifiedByID;
                    ruleDetail.RORD_ModifiedOn = DateTime.Now;
                }

                base.SharedDataDBContext.SaveChanges();

                #region UAT-2514
                return SaveRequirementPkgSyncLog(requirementRuleInDB.ROR_CreatedByID, requirementRuleInDB.ROR_ID);
                #endregion
            }
            return false;
        }

        void IRequirementRuleRepository.DeleteRequirementRuleMapping(Int32 ruleMappingId, Int32 currentUserId)
        {
            Entity.SharedDataEntity.RequirementObjectRule ruleMapping = base.SharedDataDBContext.RequirementObjectRules.Where(ror => ror.ROR_ID == ruleMappingId).FirstOrDefault();
            ruleMapping.ROR_IsDeleted = true;
            ruleMapping.ROR_ModifiedOn = DateTime.Now;
            ruleMapping.ROR_ModifiedByID = currentUserId;
            base.SharedDataDBContext.SaveChanges();
            #region UAT-2514
            SaveRequirementPkgSyncLog(ruleMapping.ROR_CreatedByID, ruleMapping.ROR_ID);
            #endregion
        }

        List<Entity.SharedDataEntity.RequirementRuleTemplate> IRequirementRuleRepository.GetRuleTemplates()
        {
            return base.SharedDataDBContext.RequirementRuleTemplates.Include("lkpRuleResultType").Where(r => r.RLT_IsDeleted == false).ToList();
        }

        Entity.SharedDataEntity.RequirementRuleTemplate IRequirementRuleRepository.GetRuleTemplateDetails(Int32 ruleTemplateId)
        {
            return base.SharedDataDBContext.RequirementRuleTemplates.Include("RequirementRuleTemplateExpressions.RequirementExpression").Where(r => r.RLT_ID == ruleTemplateId).FirstOrDefault();
        }

        Entity.SharedDataEntity.RequirementObjectTree IRequirementRuleRepository.GetRequirementObjectTree(Int32 ObjectTreeID, Int32 objectTypeID, String objectTypeCode = "")
        {
            if (!objectTypeID.IsNullOrEmpty() && objectTypeID != AppConsts.NONE)
            {
                return base.SharedDataDBContext.RequirementObjectTrees.Where(con => con.ROT_ID == ObjectTreeID && con.ROT_ObjectTypeID == objectTypeID).FirstOrDefault();
            }
            else
            {
                return base.SharedDataDBContext.RequirementObjectTrees.Where(con => con.ROT_ID == ObjectTreeID && con.lkpObjectType.OT_Code == objectTypeCode).FirstOrDefault();
            }
        }

        List<Entity.SharedDataEntity.RequirementObjectRule> IRequirementRuleRepository.GetRuleMappings(Int32 ParentObjectTreeID)
        {
            return base.SharedDataDBContext.RequirementObjectRules.Where(cond => cond.ROR_ObjectTreeId == ParentObjectTreeID && !cond.ROR_IsDeleted).ToList();

        }

        public Entity.SharedDataEntity.RequirementObjectRule GetRequirementRuleMapping(Int32 ObjectRuleID)
        {
            return base.SharedDataDBContext.RequirementObjectRules.Include("RequirementObjectRuleDetails")
                                                                  .Include("RequirementObjectRuleDetails.RequirementObjectTree")
                                                                  .FirstOrDefault(con => con.ROR_ID == ObjectRuleID);
        }

        List<RequirementExpressionData> IRequirementRuleRepository.GetAttributeDetail(List<Int32> lstFieldObjectTreeIDs)
        {
            List<RequirementExpressionData> ExpressionData = new List<RequirementExpressionData>();
            List<Entity.SharedDataEntity.RequirementObjectTree> lstRequirementObjectTree = base.SharedDataDBContext.RequirementObjectTrees.Where(con => lstFieldObjectTreeIDs.Contains(con.ROT_ID)).ToList();
            if (!lstRequirementObjectTree.IsNullOrEmpty())
            {
                List<Int32> lstFieldID = lstRequirementObjectTree.Select(con => Convert.ToInt32(con.ROT_ObjectID)).Distinct().ToList();
                List<Entity.SharedDataEntity.RequirementField> lstRequirementFields = base.SharedDataDBContext.RequirementItemFields
                                        .Where(con => lstFieldID.Contains(con.RIF_RequirementFieldID) && !con.RIF_IsDeleted && !con.RequirementField.RF_IsDeleted).Select(con => con.RequirementField).Distinct().ToList();

                foreach (Entity.SharedDataEntity.RequirementField item in lstRequirementFields)
                {
                    ExpressionData.Add(new RequirementExpressionData
                       {
                           AttributeID = item.RF_ID,
                           AttributeDataTypeName = item.lkpRequirementFieldDataType.RFDT_Name,
                           AttributeName = item.RF_FieldLabel.IsNullOrEmpty() ? item.RF_FieldName : item.RF_FieldLabel,
                           // AttributeName = item.RF_FieldName,
                           RequirementObjectAttributeID = lstRequirementObjectTree.Where(con => con.ROT_ObjectID == item.RF_ID).Select(x => x.ROT_ID).FirstOrDefault()
                       });
                }

                //foreach (Entity.SharedDataEntity.RequirementObjectTree item in lstRequirementObjectTree)
                //{
                //    ExpressionData.Add(new RequirementExpressionData
                //       {
                //           AttributeID = Convert.ToInt32(item.ROT_ObjectID),
                //           AttributeDataTypeName = lstRequirementFields.Where(con => con.RF_ID == item.ROT_ObjectID).Select(con => con.lkpRequirementFieldDataType.RFDT_Name).FirstOrDefault(),
                //           AttributeName = lstRequirementFields.Where(con => con.RF_ID == item.ROT_ObjectID).Select(con => con.RF_FieldLabel.IsNullOrEmpty() ? con.RF_FieldName : con.RF_FieldLabel).FirstOrDefault(),
                //           RequirementObjectAttributeID = item.ROT_ID,
                //       });
                //}
            }
            return ExpressionData;
        }

        String IRequirementRuleRepository.ValidateExpression(String ruleTemplateXml, String ruleExpressionXml)
        {
            return base.SharedDataDBContext.usp_ValidateRuleExpression(ruleTemplateXml, ruleExpressionXml).FirstOrDefault();
        }
        #region Category-Item Mapping

        /// <summary>
        /// Gets the list of Category related to a categoryID
        /// </summary>
        /// <param name="categoryObjectTreeID">Id of the selected category</param>
        /// <returns>List of the items of that category</returns>
        List<RequirementExpressionData> IRequirementRuleRepository.GetRequirementCategoryItems(Int32 categoryObjectTreeID)
        {
            List<RequirementExpressionData> expressionData = new List<RequirementExpressionData>();

            List<Entity.SharedDataEntity.RequirementObjectTree> lstItemsobjectTree = base.SharedDataDBContext.RequirementObjectTrees.Where(con => con.ROT_ParentID == categoryObjectTreeID && !con.ROT_IsDeleted).ToList();
            if (!lstItemsobjectTree.IsNullOrEmpty())
            {
                Entity.SharedDataEntity.RequirementObjectTree CateogryobjectTree = base.SharedDataDBContext.RequirementObjectTrees.Where(con => con.ROT_ID == categoryObjectTreeID && !con.ROT_IsDeleted).FirstOrDefault();
                List<Entity.SharedDataEntity.RequirementItem> lstrequirementItems = base.SharedDataDBContext.RequirementCategoryItems.Where(con => con.RCI_RequirementCategoryID == CateogryobjectTree.ROT_ObjectID
                        && !con.RCI_IsDeleted && !con.RequirementItem.RI_IsDeleted).Select(sel => sel.RequirementItem).Distinct().ToList();
                if (!lstrequirementItems.IsNullOrEmpty())
                {
                    foreach (Entity.SharedDataEntity.RequirementItem item in lstrequirementItems)
                    {
                        expressionData.Add(new RequirementExpressionData
                        {
                            ItemID = Convert.ToInt32(item.RI_ID),
                            RequirementObjectItemID = lstItemsobjectTree.Where(con => con.ROT_ObjectID == item.RI_ID).Select(con => con.ROT_ID).FirstOrDefault(),
                            //ItemName = item.RI_ItemLabel.IsNullOrEmpty() ? item.RI_ItemName : item.RI_ItemLabel
                            ItemName = item.RI_ItemName.IsNullOrEmpty() ? item.RI_ItemLabel : item.RI_ItemName
                        });
                    }
                }
            }



            //foreach (Entity.SharedDataEntity.RequirementObjectTree item in lstItemsobjectTree.Where(con => lstItemIds.Contains(Convert.ToInt32(con.ROT_ObjectID))).ToList())
            //{
            //    expressionData.Add(new RequirementExpressionData
            //    {
            //        ItemID = Convert.ToInt32(item.ROT_ObjectID),
            //        RequirementObjectItemID = item.ROT_ID,
            //        ItemName = lstrequirementItems.Where(con => con.RI_ID == item.ROT_ObjectID).Select(con => con.RI_ItemLabel.IsNullOrEmpty() ? con.RI_ItemName : con.RI_ItemLabel).FirstOrDefault()
            //    });
            //}

            return expressionData;
        }

        /// <summary>
        /// Gets the list of Category related to a categoryID
        /// </summary>
        /// <param name="categoryId">Id of the selected category</param>
        /// <returns>List of the items of that category</returns>
        List<RequirementExpressionData> IRequirementRuleRepository.GetRequirementSubmissionItemsByCategoryID(Int32 categoryId)
        {
            List<RequirementExpressionData> expressionData = new List<RequirementExpressionData>();

            Entity.SharedDataEntity.RequirementObjectTree CategoryobjectTree = base.SharedDataDBContext.RequirementObjectTrees.Where(con => con.ROT_ObjectID == categoryId && con.ROT_ParentID == null && !con.ROT_IsDeleted).FirstOrDefault();
            if (!CategoryobjectTree.IsNullOrEmpty())
            {
                List<Entity.SharedDataEntity.RequirementObjectTree> lstItemobjectTree = base.SharedDataDBContext.RequirementObjectTrees.Where(con => con.ROT_ParentID == CategoryobjectTree.ROT_ID && !con.ROT_IsDeleted).ToList();
                List<Entity.SharedDataEntity.RequirementItem> lstrequirementItems = base.SharedDataDBContext.RequirementCategoryItems
                    .Where(con => con.RCI_RequirementCategoryID == categoryId && !con.RCI_IsDeleted && !con.RequirementItem.RI_IsDeleted).Select(sel => sel.RequirementItem).Distinct().ToList();
                if (!lstrequirementItems.IsNullOrEmpty())
                {
                    foreach (Entity.SharedDataEntity.RequirementItem item in lstrequirementItems)
                    {
                        expressionData.Add(new RequirementExpressionData
                        {
                            ItemID = Convert.ToInt32(item.RI_ID),
                            RequirementObjectItemID = lstItemobjectTree.Where(con => con.ROT_ObjectID == item.RI_ID).Select(con => con.ROT_ID).FirstOrDefault(),
                            //ItemName = item.RI_ItemLabel.IsNullOrEmpty() ? item.RI_ItemName : item.RI_ItemLabel
                            ItemName = item.RI_ItemName.IsNullOrEmpty() ? item.RI_ItemLabel : item.RI_ItemName
                        });
                    }
                }
            }
            //foreach (Entity.SharedDataEntity.RequirementObjectTree item in lstItemobjectTree.Where(con => lstItemIds.Contains(Convert.ToInt32(con.ROT_ObjectID))).ToList())
            //{
            //    expressionData.Add(new RequirementExpressionData
            //    {
            //        ItemID = Convert.ToInt32(item.ROT_ObjectID),
            //        RequirementObjectItemID = item.ROT_ID,
            //        ItemName = lstrequirementItems.Where(con => con.RI_ID == item.ROT_ObjectID).Select(con => con.RI_ItemLabel.IsNullOrEmpty() ? con.RI_ItemName : con.RI_ItemLabel).FirstOrDefault()
            //    });
            //}

            return expressionData;
        }

        ///// <summary>
        ///// Gets the list of Category related to a categoryID
        ///// </summary>
        ///// <param name="categoryId">Id of the selected category</param>
        ///// <returns>List of the items of that category</returns>
        //List<Entity.SharedDataEntity.RequirementCategoryItem> IRequirementRuleRepository.GetRequirementCategoryItems(Int32 categoryId)
        //{
        //    List<Entity.SharedDataEntity.RequirementCategoryItem> requirementCategoryItems = base.SharedDataDBContext.RequirementCategoryItems
        //                                                           .Include("RequirementItem")
        //                                                           .Where(rci => rci.RCI_RequirementCategoryID == categoryId && rci.RCI_IsDeleted == false &&
        //                                                            rci.RequirementItem.RI_IsDeleted == false).ToList();
        //    return requirementCategoryItems;
        //}

        /// <summary>
        /// Gets the list of Items related to a category
        /// </summary>
        /// <param name="categoryId">Id of the selected category</param>
        /// <returns>List of the items of that category</returns>
        List<RequirementExpressionData> IRequirementRuleRepository.GetRequirementCategoryByCategoryID(Int32 categoryId, String ObjectTypeCode)
        {
            List<RequirementExpressionData> expressionData = new List<RequirementExpressionData>();

            Entity.SharedDataEntity.RequirementObjectTree objectTree = base.SharedDataDBContext.RequirementObjectTrees.Where(con => con.lkpObjectType.OT_Code == ObjectTypeCode
                                                                                              && con.ROT_ObjectID == categoryId && !con.ROT_IsDeleted).FirstOrDefault();

            Entity.SharedDataEntity.RequirementCategory category = base.SharedDataDBContext.RequirementCategories.Where(con => con.RC_ID == categoryId && !con.RC_IsDeleted).FirstOrDefault();

            if (objectTree.IsNotNull() && category.IsNotNull())
            {
                expressionData.Add(new RequirementExpressionData
                {
                    CategoryID = categoryId,
                    RequirementObjectCategoryID = objectTree.ROT_ID,
                    //CateogryName = category.RC_CategoryLabel.IsNullOrEmpty() ? category.RC_CategoryName : category.RC_CategoryLabel
                    CateogryName = category.RC_CategoryName.IsNullOrEmpty() ? category.RC_CategoryLabel : category.RC_CategoryName
                });
            }

            return expressionData;
        }

        #endregion

        #region Item-Attributes Mapping

        List<RequirementExpressionData> IRequirementRuleRepository.GetRequirementItemAttribute(Int32 itemObjectTreeID)
        {
            List<RequirementExpressionData> expressionData = new List<RequirementExpressionData>();

            Entity.SharedDataEntity.RequirementObjectTree ItemobjectTree = base.SharedDataDBContext.RequirementObjectTrees.Where(con => con.ROT_ID == itemObjectTreeID && !con.ROT_IsDeleted).FirstOrDefault();
            if (!ItemobjectTree.IsNullOrEmpty())
            {

                List<Entity.SharedDataEntity.RequirementObjectTree> lstItemobjectTree = base.SharedDataDBContext.RequirementObjectTrees.Where(con => con.ROT_ParentID == itemObjectTreeID && !con.ROT_IsDeleted).ToList();
                List<Entity.SharedDataEntity.RequirementField> requirementAttributes = base.SharedDataDBContext.RequirementItemFields
                                                                        .Where(x => !x.RIF_IsDeleted && !x.RequirementField.RF_IsDeleted && ItemobjectTree.ROT_ObjectID == x.RIF_RequirementItemID
                                                                             ).Select(con => con.RequirementField).Distinct().ToList();
                if (!requirementAttributes.IsNullOrEmpty())
                {
                    foreach (Entity.SharedDataEntity.RequirementField item in requirementAttributes)
                    {
                        expressionData.Add(new RequirementExpressionData
                         {
                             AttributeID = Convert.ToInt32(item.RF_ID),
                             RequirementObjectAttributeID = lstItemobjectTree.Where(con => con.ROT_ObjectID == item.RF_ID).Select(con => con.ROT_ID).FirstOrDefault(),
                             //AttributeName = item.RF_FieldLabel.IsNullOrEmpty() ? item.RF_FieldName : item.RF_FieldLabel
                             AttributeName = item.RF_FieldName
                         });
                    }


                }

            }
            //foreach (Entity.SharedDataEntity.RequirementObjectTree item in lstobjectTree)
            //{
            //    expressionData.Add(new RequirementExpressionData
            //    {
            //        AttributeID = Convert.ToInt32(item.ROT_ObjectID),
            //        RequirementObjectAttributeID = item.ROT_ID,
            //        AttributeName = requirementAttributes.Where(con => con.RF_ID == item.ROT_ObjectID).Select(con => con.RF_FieldLabel.IsNullOrEmpty() ? con.RF_FieldName : con.RF_FieldLabel).FirstOrDefault()
            //    });
            //}

            return expressionData;
        }
        #endregion

        #endregion

        #region UAT-2514

        public Boolean SaveRequirementPkgSyncLog(Int32 currentLoggedInUserId, Int32 requirementObjectRuleId)
        {
            #region Get ObjectTypeCode
            String ObjectTypeCode = String.Empty;
            String ActionTypeCode = RequirementPackageObjectActionTypeEnum.RULECHANGED.GetStringValue();
            //RequirementPackageObjectTypeEnum.REQUIREMENT_RULE.GetStringValue()
            var ObjectDetails = base.SharedDataDBContext.RequirementObjectRules.Where(cond => cond.ROR_ID == requirementObjectRuleId).Select(q => new { NewObjectId = q.RequirementObjectTree.ROT_ObjectID, ObjectTypeCode = q.RequirementObjectTree.lkpObjectType.OT_Code }).FirstOrDefault();
            Int32 NewObjectId = Convert.ToInt32(ObjectDetails.NewObjectId);

            switch (ObjectDetails.ObjectTypeCode)
            {
                case "CAT":
                    ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_CATEGORY.GetStringValue();
                    break;
                case "ITM":
                    ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_ITEM.GetStringValue();
                    break;
                case "ATR":
                    ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_FIELD.GetStringValue();
                    break;

            }
            #endregion

            List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();
            RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
            objectData.ActionTypeCode = ActionTypeCode;
            objectData.ObjectTypeCode = ObjectTypeCode;
            objectData.NewObjectId = NewObjectId;
            lstPackageObjectSynchingData.Add(objectData);

            String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
            SaveRequirementPackageObjectForSync(requestXML, currentLoggedInUserId);
            return true;
        }

        public Boolean SaveRequirementPackageObjectForSync(String requestDataXML, Int32 currentLoggedInUserId)
        {
            List<Int32> lstSyncReqPkgObjectIds = new List<Int32>();
            EntityConnection connection = base.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_AddRequirementPackageObjectsForSynching", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestXML", requestDataXML);
                cmd.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                //cmd.ExecuteScalar();
                //UAT-3230
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables.Count > AppConsts.NONE && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (var row in ds.Tables[0].AsEnumerable())
                        {
                            Int32 syncReqPkgObjectID = row["SyncRequirementPkgObjectId"] == DBNull.Value ? 0 : Convert.ToInt32(row["SyncRequirementPkgObjectId"]);
                            lstSyncReqPkgObjectIds.Add(syncReqPkgObjectID);
                        }
                    }
                }
                if (!lstSyncReqPkgObjectIds.IsNullOrEmpty())
                {
                    Dictionary<String, Object> dicData = new Dictionary<String, Object>();
                    dicData.Add("syncReqPkgObjectIds", String.Join(",", lstSyncReqPkgObjectIds));
                    dicData.Add("currentLoggedInUserId", currentLoggedInUserId);
                    dicData.Add("LoggerService", DALUtils.LoggerService);
                    var LoggerService = DALUtils.LoggerService;
                    INTSOF.ServiceUtil.ParallelTaskContext.PerformParallelTask(SaveUpdateRequirementPkgSyncDetails, dicData, LoggerService, null);
                }
                return true;
            }
        }


        public String ConvertPackageObjectTypeInXML(List<RequirementPackageObjectSynchingContract> lstObjects)
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
        #endregion

        #region UAT-2514 Copy Package
        /// <summary>
        /// Copy Rules From Shared To Tenant
        /// </summary>
        /// <param name="oldRequirementCategoryID"></param>
        /// <param name="newRequirementCategoryID"></param>
        /// <param name="CurrentLoggedInOrgUserID"></param>
        public void CopyRulesFromSharedToTenant(Int32 oldRequirementCategoryID, Int32 newRequirementCategoryID, Int32 CurrentLoggedInOrgUserID)
        {
            EntityConnection connection = base.ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_CopyRulesFromSharedToTenant", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequirementCategoryID", oldRequirementCategoryID);
                cmd.Parameters.AddWithValue("@NewlyAddedReqCategoryID", newRequirementCategoryID);
                cmd.Parameters.AddWithValue("@CurrentOrgUserID", CurrentLoggedInOrgUserID);
                cmd.ExecuteScalar();

            }
        }
        #endregion

        #region Requirement Rules Service Execution

        /// <summary>
        /// UAT 3080 : Get the list of schedule action to be executed corresponding to Schedule Category compliance check rule
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        List<RequirementScheduleActionCategoryRulesContract> IRequirementRuleRepository.GetScheduleActionExecuteCategoryRulesList(Int32 chunkSize)
        {
            List<RequirementScheduleActionCategoryRulesContract> lstScheduleActionCategoryRule = new List<RequirementScheduleActionCategoryRulesContract>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@chunkSize", chunkSize)
                        };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRequirementScheduleActionExecuteCategoryRulesList", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var scheduleActionCategoryRule = new RequirementScheduleActionCategoryRulesContract();
                            scheduleActionCategoryRule.RequirementPackageSubscriptionID = dr["PackageSubscriptionId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["PackageSubscriptionId"]);
                            scheduleActionCategoryRule.ApplicantOrgUserID = dr["ApplicnatID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ApplicnatID"]);
                            scheduleActionCategoryRule.ApplicantName = dr["ApplicantFullName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ApplicantFullName"]);
                            scheduleActionCategoryRule.PrimaryEmailAddress = dr["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PrimaryEmailAddress"]);
                            scheduleActionCategoryRule.RequirementPackageID = dr["RequirementPackageID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementPackageID"]);
                            scheduleActionCategoryRule.RequirementCategoryID = dr["RequirementCategoryID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementCategoryID"]);
                            scheduleActionCategoryRule.RequirementItemID = dr["RequirementItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementItemID"]);
                            scheduleActionCategoryRule.RequirementFieldID = dr["RequirementFieldID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementFieldID"]);
                            scheduleActionCategoryRule.RequirementPackageSubscriptionStatusID = dr["RequirementStatusId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementStatusId"]);
                            scheduleActionCategoryRule.ScheduleActionId = dr["ScheduleActionId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ScheduleActionId"]);
                            scheduleActionCategoryRule.RequirementPackageSubscriptionStatusCode = dr["RequirementPackageSubscriptionStatusCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageSubscriptionStatusCode"]);
                            scheduleActionCategoryRule.RequirementStatus = dr["RequirementStatus"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementStatus"]);
                            lstScheduleActionCategoryRule.Add(scheduleActionCategoryRule);
                        }
                    }
                }
            }
            return lstScheduleActionCategoryRule;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduleActionId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Boolean IRequirementRuleRepository.inactiveProcessedScheduleAction(Int32 scheduleActionId, Int32 systemUserId)
        {
            RequirementScheduledAction scheduleAction = _dbContext.RequirementScheduledActions.FirstOrDefault(x => x.RSA_ID == scheduleActionId &&
                                                                                                x.RSA_IsActive && !x.RSA_IsDeleted);
            if (scheduleAction != null)
            {
                scheduleAction.RSA_IsActive = false;
                scheduleAction.RSA_ModifiedByID = systemUserId;
                scheduleAction.RSA_ModifiedOn = DateTime.Now;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }



        #endregion

        #region UAT-3230
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstSyncReqPkgObjectIds"></param>
        /// <param name="currentUserId"></param>
        private void SaveUpdateRequirementPkgSyncDetails(Dictionary<String, Object> dicData)
        {
            ISysXLoggerService LoggerService = (dicData["LoggerService"] as ISysXLoggerService);
            Int32 currentUserId = Convert.ToInt32(dicData["currentLoggedInUserId"]);
            String syncReqPkgObjectIds = Convert.ToString(dicData["syncReqPkgObjectIds"]);
            try
            {
                EntityConnection connection = base.SharedDataDBContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("usp_InsertRequirementPkgSyncObjectDetails", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SyncReqPkgObjectIds", syncReqPkgObjectIds);
                    command.Parameters.AddWithValue("@UserId", currentUserId);
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }

                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("usp_UpdateSyncRequirementPackageObjectsCount", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SyncReqPkgObjectIds", syncReqPkgObjectIds);
                    command.Parameters.AddWithValue("@UserId", currentUserId);
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }

                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("usp_RemoveSyncRequirementPackageObjects", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SyncReqPkgObjectIds", syncReqPkgObjectIds);
                    command.Parameters.AddWithValue("@UserId", currentUserId);
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                LoggerService.GetLogger().Error("SyncReqPkgObjectIds : " + syncReqPkgObjectIds, ex);
            }
        }
        #endregion

        #region UAT-4228
        List<RequirementObjectTree> IRequirementRuleRepository.GetReqObjectTreeList(List<Int32> lstObjectIds, Int32 objectTypeId)
        {
            return _dbContext.RequirementObjectTrees.Where(con => !con.ROT_IsDeleted && con.ROT_ObjectID.HasValue
                              && lstObjectIds.Contains(con.ROT_ObjectID.Value) && con.ROT_ObjectTypeID == objectTypeId).ToList();
        }

        List<lkpRotScheduledActionType> IRequirementRuleRepository.GetRotScheduledActionTypes()
        {
            return _dbContext.lkpRotScheduledActionTypes.Where(cond => !cond.RSAT_IsDeleted).ToList();
        }

        Boolean IRequirementRuleRepository.SaveScheduledActions(List<RequirementScheduledAction> lstRequirementScheduledActions)
        {
            DateTime currentDate = DateTime.Now.Date;
            List<RequirementScheduledAction> lstAlreadyAddedReqScheduledAction = _dbContext.RequirementScheduledActions.Where(c => c.RSA_IsActive && !c.RSA_IsDeleted
                                                                          && c.lkpRotScheduledActionType.RSAT_Code == "AAAB"
                                                                          && c.lkpObjectType.OT_Code == "CAT"
                                                                          && DbFunctions.TruncateTime(c.RSA_ScheduleDate).Value <= currentDate).ToList();


            foreach (RequirementScheduledAction item in lstRequirementScheduledActions)
            {
                if (!lstAlreadyAddedReqScheduledAction.Any(x => x.RSA_ObjectID == item.RSA_ObjectID && x.RSA_PackageSubscriptionID == item.RSA_PackageSubscriptionID))
                    _dbContext.RequirementScheduledActions.AddObject(item);
            }

            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        #endregion
    }
}
