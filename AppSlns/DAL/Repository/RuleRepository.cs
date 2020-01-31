#region References

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Core.Metadata.Edm;
using System.Web.Configuration;

#endregion

#region Application Specific

using INTSOF.Utils;
using DAL.Interfaces;
using Entity.ClientEntity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;

#endregion

#endregion

namespace DAL.Repository
{
    /// <summary>
    /// RuleRepository
    /// </summary>
    public class RuleRepository : ClientBaseRepository, IRuleRepository
    {

        #region Variables

        #region Private Variables

        private ObjectContext _RuleEntityContext;


        #endregion

        #endregion

        #region Methods

        #region Public Methods

        #region Constructor
        private ADB_LibertyUniversity_ReviewEntities _ClientDBContext;

        ///// <summary>
        ///// Default constructor to initialize class level variables.
        ///// </summary>
        public RuleRepository(Int32 tenantId)
            : base(tenantId)
        {
            _ClientDBContext = base.ClientDBContext;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// AddObjectEntity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override T AddObjectEntity<T>(T entity)
        {
            var entityTypeAttr = (EdmEntityTypeAttribute)entity.GetType().GetCustomAttributes(typeof(EdmEntityTypeAttribute), false).First();
            String entitySetName =
                _RuleEntityContext.MetadataWorkspace.GetEntityContainer(_RuleEntityContext.DefaultContainerName, DataSpace.CSpace).BaseEntitySets
                                                          .Where(bes => bes.ElementType.Name == typeof(T).Name).FirstOrDefault().Name;

            _RuleEntityContext.AddObject(entitySetName, entity);

            _RuleEntityContext.SaveChanges();
            return entity;
        }

        /// <summary>
        /// Generic method to add an entity in DB in a single transaction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddObjectEntityInTransaction<T>(T entity) where T : EntityObject
        {
            try
            {
                var entityTypeAttr = (EdmEntityTypeAttribute)entity.GetType().GetCustomAttributes(typeof(EdmEntityTypeAttribute), false).First();
                String entitySetName =
                    _RuleEntityContext.MetadataWorkspace.GetEntityContainer(_RuleEntityContext.DefaultContainerName, DataSpace.CSpace).BaseEntitySets
                                                              .Where(bes => bes.ElementType.Name == typeof(T).Name).FirstOrDefault().Name;

                _RuleEntityContext.AddObject(entitySetName, entity);

                return entity;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected Int32 SaveChanges()
        {
            return _RuleEntityContext.SaveChanges();
        }

        #endregion

        #region IRuleRepository Member

        /// <summary>
        /// GetRuleTemplate
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        RuleTemplate IRuleRepository.GetRuleTemplate(Int32 ruleId)
        {
            RuleTemplate rule = ClientDBContext.RuleTemplates.Include("RuleTemplateExpressions.Expression").Where(r => r.RLT_ID == ruleId).FirstOrDefault();
            return rule;
        }

        /// <summary>
        /// GetRuleTemplates
        /// </summary>
        /// <returns></returns>
        List<RuleTemplate> IRuleRepository.GetRuleTemplates(Boolean? IsSeriesDataRqd)
        {
            if (IsSeriesDataRqd.IsNull() || IsSeriesDataRqd == true)
                return ClientDBContext.RuleTemplates.Include("lkpRuleType").Include("lkpRuleActionType").Include("lkpRuleResultType").Where(r => r.RLT_IsDeleted == false).ToList();
            else
                return ClientDBContext.RuleTemplates.Include("lkpRuleType").Include("lkpRuleActionType").Include("lkpRuleResultType").Where(r => r.RLT_IsDeleted == false
                                                                                                                                            && (r.lkpRuleActionType.ACT_Code != "RSSR" && r.lkpRuleActionType.ACT_Code != "VSRI")).ToList();
            //else
            //    return ClientDBContext.RuleTemplates.Include("lkpRuleType").Include("lkpRuleActionType").Include("lkpRuleResultType").Where(r => r.RLT_IsDeleted == false
            //                                                                                                                                    && (r.lkpRuleActionType.ACT_Code == "RSSR" || r.lkpRuleActionType.ACT_Code == "VSRI")).ToList();
        }

        /// <summary>
        /// UpdateRuleTemplate
        /// </summary>
        /// <param name="rule"></param>
        void IRuleRepository.UpdateRuleTemplate(Entity.ComplianceRuleTemplate complianceRuleTemplate, List<Int32> expressionIds)
        {
            RuleTemplate ruleTemplate = ClientDBContext.RuleTemplates.Include("RuleTemplateExpressions.Expression").Where(rr => rr.RLT_ID == complianceRuleTemplate.RLT_ID && rr.RLT_IsActive == true && rr.RLT_IsDeleted == false).FirstOrDefault();
            String delimiterKey = WebConfigurationManager.AppSettings[AppConsts.DELIMITER];
            String delimiters = "|||";
            delimiterKey = delimiterKey.IsNull() || delimiterKey == "" ? delimiters : delimiterKey;

            ruleTemplate.RLT_Code = ruleTemplate.RLT_Code.Equals(Guid.Empty) ? Guid.NewGuid() : ruleTemplate.RLT_Code;
            //ruleTemplate.RLT_UserExpression = complianceRuleTemplate.ComplianceRuleExpressionTemplates.LastOrDefault().EX_Expression;
            ruleTemplate.RLT_UIExpression = complianceRuleTemplate.RuleGroupExpression;
            ruleTemplate.RLT_Name = complianceRuleTemplate.RLT_Name;
            ruleTemplate.RLT_Description = complianceRuleTemplate.RLT_Description;
            ruleTemplate.RLT_ResultType = complianceRuleTemplate.RLT_ResultType;
            ruleTemplate.RLT_ActionType = complianceRuleTemplate.RLT_ActionType;
            ruleTemplate.RLT_Type = complianceRuleTemplate.RLT_Type;
            ruleTemplate.RLT_ObjectCount = complianceRuleTemplate.RLT_ObjectCount;
            ruleTemplate.RLT_Notes = complianceRuleTemplate.RLT_Notes;
            ruleTemplate.RLT_IsActive = complianceRuleTemplate.RLT_IsActive;
            ruleTemplate.RLT_IsDeleted = complianceRuleTemplate.RLT_IsDeleted;
            ruleTemplate.RLT_ModifiedByID = complianceRuleTemplate.RLT_ModifiedByID;
            ruleTemplate.RLT_ModifiedOn = complianceRuleTemplate.RLT_ModifiedOn;


            foreach (Entity.ComplianceRuleExpressionTemplate rExp in complianceRuleTemplate.ComplianceRuleExpressionTemplates)
            {
                string expression = string.Empty;
                foreach (Entity.ComplianceRuleExpressionElement ruleExpressionElement in rExp.RuleExpressionElements)
                {
                    //expression += ruleExpressionElement.ElementValue + " ";
                    String equalOperator = "EQUAL";

                    if (ruleExpressionElement.ElementValue.ToUpper().StartsWith("E", StringComparison.OrdinalIgnoreCase) && !(equalOperator.Equals(ruleExpressionElement.ElementValue.ToUpper())))
                    {
                        if (expression == String.Empty)
                        {
                            expression += ruleExpressionElement.ElementValue;
                        }
                        else
                        {
                            expression += delimiterKey + ruleExpressionElement.ElementValue;
                        }
                    }
                    else
                    {
                        if (expression == String.Empty)
                        {
                            expression += ruleExpressionElement.ElementOperator;
                        }
                        else
                        {
                            expression += delimiterKey + ruleExpressionElement.ElementOperator;
                        }
                    }
                }
                expression = expression.Trim();

                if (rExp.EX_ID == 0)
                {
                    ruleTemplate.RuleTemplateExpressions.Add
                    (
                        new RuleTemplateExpression()
                        {
                            RLE_CreatedByID = complianceRuleTemplate.RLT_CreatedByID,
                            RLE_CreatedOn = complianceRuleTemplate.RLT_CreatedOn,
                            RLE_IsActive = true,
                            RLE_ExpressionOrder = rExp.ExpressionOrder,
                            Expression = new Expression()
                            {
                                EX_ID = rExp.EX_ID,
                                EX_Name = rExp.EX_Name,
                                EX_Description = rExp.EX_Description,
                                EX_ResultType = complianceRuleTemplate.RLT_ResultType,
                                EX_Expression = expression,
                                EX_IsActive = true,
                                EX_IsDeleted = false,
                                EX_CreatedByID = complianceRuleTemplate.RLT_CreatedByID,
                                EX_CreatedOn = complianceRuleTemplate.RLT_CreatedOn,
                            }
                        });
                }
                else
                {
                    RuleTemplateExpression rex = ruleTemplate.RuleTemplateExpressions.FirstOrDefault(fx => fx.RLE_ExpressionID == rExp.EX_ID
                                                 && fx.RLE_IsActive == true && fx.RLE_IsDeleted == false);
                    if (rex.IsNotNull())
                    {
                        rex.RLE_ModifiedByID = complianceRuleTemplate.RLT_ModifiedByID;
                        rex.RLE_ModifiedOn = complianceRuleTemplate.RLT_ModifiedOn;
                        rex.RLE_IsActive = true;
                        rex.RLE_ExpressionOrder = rExp.ExpressionOrder;

                        rex.Expression.EX_ID = rExp.EX_ID;
                        rex.Expression.EX_Name = rExp.EX_Name;
                        rex.Expression.EX_Description = rExp.EX_Description;
                        rex.Expression.EX_ResultType = complianceRuleTemplate.RLT_ResultType;
                        rex.Expression.EX_Expression = expression;
                        rex.Expression.EX_IsActive = true;
                        rex.Expression.EX_IsDeleted = false;
                        rex.Expression.EX_ModifiedByID = complianceRuleTemplate.RLT_ModifiedByID;
                        rex.Expression.EX_ModifiedOn = complianceRuleTemplate.RLT_ModifiedOn;
                    }
                }
            }

            //Delete old records from saved records
            RuleTemplateExpression ruleTemplateExpression = null;
            if (expressionIds.IsNotNull())
            {
                foreach (var exId in expressionIds)
                {
                    ruleTemplateExpression = ruleTemplate.RuleTemplateExpressions.FirstOrDefault(x => x.RLE_ExpressionID == exId);
                    if (ruleTemplateExpression.IsNotNull())
                    {
                        //Update RuleTemplateExpression table
                        ruleTemplateExpression.RLE_IsDeleted = true;
                        ruleTemplateExpression.RLE_ModifiedByID = ruleTemplate.RLT_ModifiedByID;
                        ruleTemplateExpression.RLE_ModifiedOn = ruleTemplate.RLT_ModifiedOn;

                        //Update Expression table
                        if (ruleTemplateExpression.Expression.IsNotNull())
                        {
                            ruleTemplateExpression.Expression.EX_IsDeleted = true;
                            ruleTemplateExpression.Expression.EX_ModifiedByID = ruleTemplate.RLT_ModifiedByID;
                            ruleTemplateExpression.Expression.EX_ModifiedOn = ruleTemplate.RLT_ModifiedOn;
                        }
                    }
                }
            }

            ClientDBContext.SaveChanges();
        }

        /// <summary>
        /// AddRuleTemplate
        /// </summary>
        /// <param name="ruleTemplate"></param>
        void IRuleRepository.AddRuleTemplate(Entity.ComplianceRuleTemplate complianceRuleTemplate)
        {
            String delimiterKey = WebConfigurationManager.AppSettings[AppConsts.DELIMITER];
            String delimiters = "|||";
            delimiterKey = delimiterKey.IsNull() || delimiterKey == "" ? delimiters : delimiterKey;
            RuleTemplate ruleTemplate = new RuleTemplate();

            ruleTemplate.RLT_Code = ruleTemplate.RLT_Code.Equals(Guid.Empty) ? Guid.NewGuid() : ruleTemplate.RLT_Code;
            //rule.RL_Expression = ruleTemplate.RuleGroupExpression;
            //ruleTemplate.RLT_UIExpression = complianceRuleTemplate.ComplianceRuleExpressionTemplates.LastOrDefault().EX_Expression;
            ruleTemplate.RLT_UIExpression = complianceRuleTemplate.RuleGroupExpression;
            ruleTemplate.RLT_Name = complianceRuleTemplate.RLT_Name;
            ruleTemplate.RLT_Description = complianceRuleTemplate.RLT_Description;
            ruleTemplate.RLT_ResultType = complianceRuleTemplate.RLT_ResultType;
            ruleTemplate.RLT_ActionType = complianceRuleTemplate.RLT_ActionType;
            ruleTemplate.RLT_Type = complianceRuleTemplate.RLT_Type;
            ruleTemplate.RLT_ObjectCount = complianceRuleTemplate.RLT_ObjectCount;
            ruleTemplate.RLT_Notes = complianceRuleTemplate.RLT_Notes;
            ruleTemplate.RLT_IsActive = complianceRuleTemplate.RLT_IsActive;
            ruleTemplate.RLT_IsDeleted = complianceRuleTemplate.RLT_IsDeleted;
            ruleTemplate.RLT_CreatedByID = complianceRuleTemplate.RLT_CreatedByID;
            ruleTemplate.RLT_CreatedOn = complianceRuleTemplate.RLT_CreatedOn;

            foreach (Entity.ComplianceRuleExpressionTemplate rExp in complianceRuleTemplate.ComplianceRuleExpressionTemplates)
            {
                String expression = String.Empty;
                foreach (Entity.ComplianceRuleExpressionElement complianceRuleExpressionElement in rExp.RuleExpressionElements)
                {
                    //expression += complianceRuleExpressionElement.ElementValue + " ";
                    String equalOperator = "EQUAL";

                    if (complianceRuleExpressionElement.ElementValue.ToUpper().StartsWith("E", StringComparison.OrdinalIgnoreCase) && !(equalOperator.Equals(complianceRuleExpressionElement.ElementValue.ToUpper())))
                    {
                        if (expression == String.Empty)
                        {
                            expression += complianceRuleExpressionElement.ElementValue;
                        }
                        else
                        {
                            expression += delimiterKey + complianceRuleExpressionElement.ElementValue;
                        }
                    }
                    else
                    {
                        if (expression == String.Empty)
                        {
                            expression += complianceRuleExpressionElement.ElementOperator;
                        }
                        else
                        {
                            expression += delimiterKey + complianceRuleExpressionElement.ElementOperator;
                        }
                    }
                }
                expression = expression.Trim();

                ruleTemplate.RuleTemplateExpressions.Add
                (
                    new RuleTemplateExpression()
                    {
                        RLE_CreatedByID = complianceRuleTemplate.RLT_CreatedByID,
                        RLE_CreatedOn = DateTime.Now,
                        RLE_IsActive = complianceRuleTemplate.RLT_IsActive,
                        RLE_ExpressionOrder = rExp.ExpressionOrder,
                        Expression = new Expression()
                        {
                            EX_ID = rExp.EX_ID,
                            EX_Name = rExp.EX_Name,
                            EX_Description = rExp.EX_Description,
                            EX_ResultType = complianceRuleTemplate.RLT_ResultType,
                            EX_Expression = expression,
                            EX_IsActive = complianceRuleTemplate.RLT_IsActive,
                            EX_IsDeleted = rExp.EX_IsDeleted,
                            EX_CreatedByID = complianceRuleTemplate.RLT_CreatedByID,
                            EX_CreatedOn = DateTime.Now
                        }
                    }
                );
            }
            ClientDBContext.RuleTemplates.AddObject(ruleTemplate);
            ClientDBContext.SaveChanges();
        }

        /// <summary>
        /// DeleteRuleTemplate
        /// </summary>
        /// <param name="ruleId"></param>
        /// <param name="currentUserId"></param>
        void IRuleRepository.DeleteRuleTemplate(Int32 ruleId, Int32 currentUserId)
        {
            RuleTemplate ruleTemplate = ClientDBContext.RuleTemplates.Where(r => r.RLT_ID == ruleId).FirstOrDefault();
            ruleTemplate.RLT_IsDeleted = true;
            ruleTemplate.RLT_ModifiedByID = currentUserId;
            ruleTemplate.RLT_ModifiedOn = DateTime.Now;
            ClientDBContext.SaveChanges();

        }

        /// <summary>
        /// GetExpressionOperators
        /// </summary>
        /// <returns></returns>
        List<lkpExpressionOperator> IRuleRepository.GetExpressionOperators()
        {
            return ClientDBContext.lkpExpressionOperators.Include("lkpExpressionOperatorType").AsQueryable().ToList();
        }

        /// <summary>
        /// ValidateRuleTemplate
        /// </summary>
        /// <param name="ruleTemplateXML"></param>
        /// <returns></returns>
        public string ValidateRuleTemplate(string ruleTemplateXML)
        {
            //return ClientDBContext.usp_ValidateRuleTemplate(ruleTemplateXML).FirstOrDefault();
            return ClientDBContext.ValidateRuleTemplate(ruleTemplateXML).FirstOrDefault();
            //return "Validated Data";  //Todo
        }

        #endregion

        #region Validate Rule
        public string ValidateExpression(String ruleTemplateXml, String ruleExpression)
        {
            return ClientDBContext.usp_ValidateRuleExpression(ruleTemplateXml, ruleExpression).FirstOrDefault();
        }

        public List<ComplianceAttribute> getAttributeDetail(List<Int32> objectIds)
        {
            return ClientDBContext.ComplianceAttributes
                                  .Include("lkpComplianceAttributeDatatype")
                                  .Where(obj => objectIds.Contains(obj.ComplianceAttributeID)).ToList();
        }

        public String RuleObjectMappingTypeCodebyId(Int32 rmtID)
        {
            return ClientDBContext.lkpRuleObjectMappingTypes.FirstOrDefault(x => x.RMT_ID == rmtID && !x.RMT_IsDeleted).RMT_Code;
        }

        #endregion

        #region Rule Mapping

        public Boolean AddRuleMapping(RuleMapping ruleMapping)
        {
            ClientDBContext.RuleMappings.AddObject(ruleMapping);
            ClientDBContext.SaveChanges();
            return true;
        }

        public RuleMapping GetRuleMapping(Int32 rlm_Id, Boolean getRuleMappingDetails)
        {
            if (getRuleMappingDetails)
            {
                return ClientDBContext.RuleMappings
                     .Include("RuleMappingDetails")
                     .Include("RuleMappingDetails.RuleMappingObjectTrees")
                     .Include("RuleMappingDetails.lkpObjectType")
                     .FirstOrDefault(x => x.RLM_ID == rlm_Id);
            }
            else
            {
                return ClientDBContext.RuleMappings.FirstOrDefault(x => x.RLM_ID == rlm_Id);
            }
        }

        public RuleTemplate GetRuleTemplateDetails(Int32 ruleTemplateId)
        {
            return ClientDBContext.RuleTemplates.Include("RuleTemplateExpressions.Expression").Where(r => r.RLT_ID == ruleTemplateId).FirstOrDefault();
        }

        // public List<RuleMapping> GetRuleMappings(Int32 ruleSetId)
        public List<RuleMappingContract> GetRuleMappings(Int32 ruleSetId)
        {
            // return ClientDBContext.RuleMappings.Where(rm => rm.RLM_RuleSetID == ruleSetId && !rm.RLM_IsDeleted).ToList();
            //Comment in optimization
            //Below added
            List<RuleMappingContract> lstRuleMappingContracts = new List<RuleMappingContract>();

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("dbo.usp_GetRuleMappings", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RuleSetId", ruleSetId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > AppConsts.NONE)
                    {
                        lstRuleMappingContracts = ds.Tables[0].AsEnumerable().Select(x => new RuleMappingContract
                        {

                            RLM_ID = x["RLM_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["RLM_ID"]),
                            RLM_RuleSetID = x["RLM_RuleSetID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["RLM_RuleSetID"]),
                            RLM_Name = x["RLM_Name"] == DBNull.Value ? String.Empty : Convert.ToString(x["RLM_Name"]),
                            RLT_Description = x["RLT_Description"] == DBNull.Value ? String.Empty : Convert.ToString(x["RLT_Description"]),
                            RSL_Description = x["RSL_Description"] == DBNull.Value ? String.Empty : Convert.ToString(x["RSL_Description"]),
                            ACT_Description = x["ACT_Description"] == DBNull.Value ? String.Empty : Convert.ToString(x["ACT_Description"]),
                            RLM_IsActive = x["RLM_IsActive"] == DBNull.Value ? false : Convert.ToBoolean(x["RLM_IsActive"]),
                            RLM_IsCurrent = x["RLM_IsCurrent"] == DBNull.Value ? false : Convert.ToBoolean(x["RLM_IsCurrent"]),
                            IsRuleAssociationExists = x["IsRuleAssociationExists"] == DBNull.Value ? false : Convert.ToBoolean(x["IsRuleAssociationExists"])
                        }).ToList();
                    }
                }
            }
            return lstRuleMappingContracts;
        }

        public void DeleteRuleMapping(Int32 ruleMappingId, Int32 currentUserId)
        {
            RuleMapping ruleMapping = ClientDBContext.RuleMappings.Where(rm => rm.RLM_ID == ruleMappingId).FirstOrDefault();
            ruleMapping.RLM_IsDeleted = true;
            ruleMapping.RLM_ModifiedOn = DateTime.Now;
            ruleMapping.RLM_ModifiedByID = currentUserId;
            ClientDBContext.SaveChanges();
        }

        Boolean IRuleRepository.DeleteRuleMappingAndRefireRules(Int32 ruleMappingId, Int32 currentUserId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_RemoveRuleMapping", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RuleId", ruleMappingId);
                command.Parameters.AddWithValue("@UserId", currentUserId);
                command.Parameters.Add("@isScheduleActionRecordInserted", SqlDbType.Bit);
                command.Parameters["@isScheduleActionRecordInserted"].Direction = ParameterDirection.Output;
                command.CommandTimeout = 120;
                if (con.State == ConnectionState.Closed)
                    con.Open();
                command.ExecuteNonQuery();
                con.Close();

                return Convert.ToBoolean(command.Parameters["@isScheduleActionRecordInserted"].Value);
            }
        }

        public List<lkpRuleObjectMappingType> GetRuleObjectMappingType()
        {
            return ClientDBContext.lkpRuleObjectMappingTypes.Where(rmt => !rmt.RMT_IsDeleted).ToList();
        }

        public List<lkpObjectType> GetObjectTypes(String ruleObjectMappingTypeCode)
        {
            //Int32 _ruleObjectMappingTypeId = ClientDBContext.lkpRuleObjectMappingTypes.
            //    Where(rmt => rmt.RMT_Code.ToLower() == ruleObjectMappingTypeCode.ToLower() && !rmt.RMT_IsDeleted).
            //    FirstOrDefault().RMT_ID;

            return ClientDBContext.lkpRuleObjectMappingTypeObjectTypes.
                 Where(rmtot => rmtot.lkpRuleObjectMappingType.RMT_Code == ruleObjectMappingTypeCode && !rmtot.RMTO_IsDeleted).
                 Select(rmtot => rmtot.lkpObjectType).ToList();

        }

        public List<RuleSetTree> GetRuleSetTreeData()
        {
            return ClientDBContext.RuleSetTrees.Where(x => !x.RST_IsDeleted).ToList();
        }

        public lkpObjectType GetObjectType(String ot_Code)
        {
            return ClientDBContext.lkpObjectTypes.FirstOrDefault(x => x.OT_Code == ot_Code && !x.OT_IsDeleted);
        }

        public List<lkpObjectType> GetObjectTypeList()
        {
            return ClientDBContext.lkpObjectTypes.ToList();
        }

        public lkpRuleObjectMappingType GetRuleObjectMappingType(String rmt_Code)
        {
            return ClientDBContext.lkpRuleObjectMappingTypes.FirstOrDefault(x => x.RMT_Code == rmt_Code && !x.RMT_IsDeleted);
        }

        public RuleMappingObjectTree GetRuleMapingObjectTree(Int32 ruleMappingDetailId, Int32 ruleSetTreeID)
        {
            return ClientDBContext.RuleMappingObjectTrees.FirstOrDefault(obj => obj.RMOT_RuleMappingDetailID == ruleMappingDetailId && obj.RMOT_RuleSetTreeID == ruleSetTreeID);
        }

        public lkpObjectType GetObjectTypeById(Int32 ot_ID)
        {
            return ClientDBContext.lkpObjectTypes.FirstOrDefault(x => x.OT_ID == ot_ID && !x.OT_IsDeleted);
        }

        public Boolean UpdateRuleMapping(RuleMapping ruleMapping)
        {
            RuleMapping ruleMappingInDb = GetRuleMapping(ruleMapping.RLM_ID, true);
            if (ruleMappingInDb != null)
            {
                ruleMappingInDb.RLM_RuleTemplateID = ruleMapping.RLM_RuleTemplateID;
                ruleMappingInDb.RLM_SuccessMessage = ruleMapping.RLM_SuccessMessage;
                ruleMappingInDb.RLM_ErrorMessage = ruleMapping.RLM_ErrorMessage;
                //ruleMappingInDb.RLM_RuleSetID = ruleMapping.RLM_RuleSetID;
                ruleMappingInDb.RLM_Name = ruleMapping.RLM_Name;
                ruleMappingInDb.RLM_ActionBlock = ruleMapping.RLM_ActionBlock;
                ruleMappingInDb.RLM_UIExpression = ruleMapping.RLM_UIExpression;
                ruleMappingInDb.RLM_IsActive = ruleMapping.RLM_IsActive;
                ruleMappingInDb.RLM_IsDeleted = ruleMapping.RLM_IsDeleted;
                ruleMappingInDb.RLM_ModifiedByID = ruleMapping.RLM_ModifiedByID;
                ruleMappingInDb.RLM_ModifiedOn = DateTime.Now;
                ruleMappingInDb.RLM_IsActive = ruleMapping.RLM_IsActive;
                ruleMappingInDb.RLM_IsCurrent = ruleMapping.RLM_IsCurrent;
                foreach (RuleMappingDetail mappingDetail in ruleMapping.RuleMappingDetails)
                {
                    RuleMappingDetail existingMapping = ruleMappingInDb.RuleMappingDetails.FirstOrDefault(obj => obj.RLMD_PlaceHolderName == mappingDetail.RLMD_PlaceHolderName);
                    if (existingMapping != null)
                    {
                        existingMapping.RLMD_PlaceHolderName = mappingDetail.RLMD_PlaceHolderName;
                        existingMapping.RLMD_ObjectID = mappingDetail.RLMD_ObjectID;
                        existingMapping.RLMD_ObjectTypeID = mappingDetail.RLMD_ObjectTypeID;
                        existingMapping.RLMD_RuleObjectMappingTypeID = mappingDetail.RLMD_RuleObjectMappingTypeID;
                        existingMapping.RLMD_ConstantType = mappingDetail.RLMD_ConstantType;
                        existingMapping.RLMD_ConstantValue = mappingDetail.RLMD_ConstantValue;
                        existingMapping.RLMD_ModifiedByID = mappingDetail.RLMD_ModifiedByID;
                        existingMapping.RLMD_ModifiedOn = DateTime.Now;
                        existingMapping.RLMD_IsDeleted = false;
                    }

                    var existingMappingObjectTreeList = existingMapping.RuleMappingObjectTrees.ToList();
                    foreach (RuleMappingObjectTree existingMappingObjectTree in existingMappingObjectTreeList)
                    {
                        //To Do delete existing record
                        ClientDBContext.RuleMappingObjectTrees.DeleteObject(existingMappingObjectTree);
                    }

                    foreach (RuleMappingObjectTree newMappingObjectTree in mappingDetail.RuleMappingObjectTrees)
                    {
                        if (newMappingObjectTree != null)
                        {
                            RuleMappingObjectTree ruleMappingObjectTree = new RuleMappingObjectTree();
                            ruleMappingObjectTree.RMOT_RuleMappingDetailID = newMappingObjectTree.RMOT_RuleMappingDetailID;
                            ruleMappingObjectTree.RMOT_RuleSetTreeID = newMappingObjectTree.RMOT_RuleSetTreeID;
                            ruleMappingObjectTree.RMOT_ObjectID = newMappingObjectTree.RMOT_ObjectID;
                            existingMapping.RuleMappingObjectTrees.Add(ruleMappingObjectTree);
                        }
                    }
                }
                ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        public Boolean IsRuleAlreadyInUse(Int32 ruleMappingId)
        {
            String code = LkpSubscriptionMobilityStatus.MobilitySwitched;
            Int32 subscriptionMobilityStatusID = ClientDBContext.lkpSubscriptionMobilityStatus.Where(item => !item.IsDeleted && item.Code.Equals(code)).FirstOrDefault().SubscriptionMobilityStatusID;
            Boolean IsRuleAlreadyInUse = ClientDBContext.PackageSubscriptionRules.Any(x => x.PSR_RuleMappingID == ruleMappingId
                                                                                     && !x.PSR_IsDeleted && !x.PackageSubscription.IsDeleted
                                                                                     && (x.PackageSubscription.SubscriptionMobilityStatusID == null || x.PackageSubscription.SubscriptionMobilityStatusID != subscriptionMobilityStatusID));
            Boolean ifRuleIsOldVersion = ClientDBContext.RuleMappings.Any(x => x.RLM_ID == ruleMappingId && x.RLM_IsCurrent == false && !x.RLM_IsDeleted);
            if (IsRuleAlreadyInUse || ifRuleIsOldVersion)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IQueryable<lkpConstantType> getConstantType()
        {
            return ClientDBContext.lkpConstantTypes.Where(obj => obj.IsDeleted == false);
        }

        public lkpConstantType getConstantTypeByCode(String code)
        {
            return ClientDBContext.lkpConstantTypes.Where(obj => obj.Code == code
                                                    && !obj.IsDeleted).FirstOrDefault();
        }

        public lkpConstantType getConstantTypeById(Int32 constantTypeId)
        {
            return ClientDBContext.lkpConstantTypes.Where(obj => obj.ID == constantTypeId
                                                    && !obj.IsDeleted).FirstOrDefault();
        }


        public void deActivatePreviousRules(Int32? prevVesrsionId)
        {
            List<RuleMapping> rulesToBeDeactivate = ClientDBContext.RuleMappings.Where(x => x.RLM_FirstVersionID == prevVesrsionId).ToList();
            if (rulesToBeDeactivate != null)
            {
                foreach (RuleMapping ruleMapping in rulesToBeDeactivate)
                {
                    //set iscurrent=false
                    ruleMapping.RLM_IsCurrent = false;
                }
                ClientDBContext.SaveChanges();
            }
        }

        public Boolean DeactivatePreviousRulesAndCreateNewRule(String parameters)
        {
            return ClientDBContext.usp_DeactivatePreviousRulesAndCreateNewRule(parameters).FirstOrDefault().Value;
        }

        public List<lkpRuleImpactGroup> getImpactedUserGroupType()
        {
            return _ClientDBContext.lkpRuleImpactGroups.Where(cond => cond.IsDeleted == false).ToList();
        }


        public List<RuleImpactGroupMapping> getPreviousImpactedGroupMappings(Int32 ruleMappingId)
        {
            return _ClientDBContext.RuleImpactGroupMappings.Where(cond => cond.RUGM_RuleMappingId == ruleMappingId && cond.RUGM_IsDeleted == false).ToList();
        }

        public void updateRuleImpactedGroupMappings(List<RuleImpactGroupMapping> impactedGroupMappings, Int32 ruleMappingId, Int32 loggedInUserId)
        {
            var previousImpactedGroupMappings = getPreviousImpactedGroupMappings(ruleMappingId);
            if (previousImpactedGroupMappings != null && previousImpactedGroupMappings.Count > AppConsts.NONE)
            {
                foreach (RuleImpactGroupMapping previousImpactedGroupMapping in previousImpactedGroupMappings)
                {
                    previousImpactedGroupMapping.RUGM_IsDeleted = true;
                    previousImpactedGroupMapping.RUGM_ModifiedByID = 1;
                    previousImpactedGroupMapping.RUGM_ModifiedOn = DateTime.Now;
                }
            }
            saveRuleImpactedGroupMapping(impactedGroupMappings, loggedInUserId);
        }

        public void saveRuleImpactedGroupMapping(List<RuleImpactGroupMapping> impactedGroupMappings, Int32 loggedInUserId)
        {
            if (impactedGroupMappings.Count > 0)
            {
                foreach (RuleImpactGroupMapping impactedGroup in impactedGroupMappings)
                {
                    impactedGroup.RUGM_IsDeleted = false;
                    impactedGroup.RUGM_CreatedById = loggedInUserId;
                    impactedGroup.RUGM_CreatedOn = DateTime.Now;
                    _ClientDBContext.RuleImpactGroupMappings.AddObject(impactedGroup);
                }
                _ClientDBContext.SaveChanges();
            }
        }

        public Int32 GetLargeObjectTypeIdByCode(String code)
        {
            return ClientDBContext.lkpObjectTypes.Where(obj => obj.OT_Code == code && obj.OT_IsDeleted == false).FirstOrDefault().OT_ID;
        }

        public DataTable GetListOfInstanceWichCanShareRule(Int32 ruleSetId, String HID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("dbo.usp_GetPackageListForSharingRuleInstance", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RuleSetId", ruleSetId);
                command.Parameters.AddWithValue("@HID", HID);
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

        public RuleSynchronisationData GetListOfInstanceWichCanShareRuleOnEdit(Int32 ruleSetId, Int32 ruleId)
        {
            RuleSynchronisationData ruleSynchronisationData = new RuleSynchronisationData();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("dbo.usp_GetPackageListForSharingRuleInstance", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RuleSetId", ruleSetId);
                command.Parameters.AddWithValue("@SourceRuleId", ruleId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > AppConsts.NONE)
                    {
                        ruleSynchronisationData.PkgListCanShareRuleInstance = ds.Tables[0].AsEnumerable().Select(x => new CompliancePackage
                        {
                            CompliancePackageID = x["CompliancePackageID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["CompliancePackageID"]),
                            PackageName = x["PackageName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["PackageName"])
                        }).ToList();
                    }

                    if (ds.Tables[1].Rows.Count > AppConsts.NONE)
                    {
                        ruleSynchronisationData.IfRuleIsAlreadyShared = Convert.ToBoolean(ds.Tables[1].Rows[0]["IfRuleIsAlreadyShared"]);
                    }
                }
            }
            return ruleSynchronisationData;
        }

        public Boolean ComplianceRuleSynchronisation(List<Int32> packageList, Int32 sourceRuleId, Int32 currentUserId, String settingParameters, Boolean sourceRuleHasSubscription)
        {
            DataTable dtPackageList = new DataTable();
            dtPackageList.Columns.Add("PackageId", typeof(Int32));

            foreach (Int32 pkgId in packageList)
            {
                dtPackageList.Rows.Add(new object[] { pkgId });
            }

            EntityConnection connection = ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_ComplianceRuleSynch", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageList", dtPackageList);
                command.Parameters.AddWithValue("@SourceRuleId", sourceRuleId);
                command.Parameters.AddWithValue("@UserId", currentUserId);
                command.Parameters.AddWithValue("@SettingParameters", settingParameters);
                command.Parameters.AddWithValue("@SourceRuleHasSubscription", sourceRuleHasSubscription);
                command.Parameters.Add("@isScheduleActionRecordInserted", SqlDbType.Bit);
                command.Parameters["@isScheduleActionRecordInserted"].Direction = ParameterDirection.Output;
                if (con.State == ConnectionState.Closed)
                    con.Open();
                command.ExecuteNonQuery();
                con.Close();

                return Convert.ToBoolean(command.Parameters["@isScheduleActionRecordInserted"].Value);
            }
        }

        public Boolean ComplianceRuleSynchronisationonRuleEdit(List<Int32> packageList, Int32 sourceRuleId, Int32 currentUserId, String settingParameters, Boolean isVersionUpdate, Int32? sourceRuleVersionId, Boolean? updateAllSelected)
        {
            DataTable dtPackageList = new DataTable();
            dtPackageList.Columns.Add("PackageId", typeof(Int32));

            foreach (Int32 pkgId in packageList)
            {
                dtPackageList.Rows.Add(new object[] { pkgId });
            }

            EntityConnection connection = ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_ComplianceRuleSynchOnRuleEdit", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageList", dtPackageList);
                command.Parameters.AddWithValue("@SourceRuleId", sourceRuleId);
                command.Parameters.AddWithValue("@SourceRuleVersionId", sourceRuleVersionId);
                command.Parameters.AddWithValue("@UserId", currentUserId);
                command.Parameters.AddWithValue("@IsVersionUpdate", isVersionUpdate);
                command.Parameters.AddWithValue("@UpdateAll", updateAllSelected);
                command.Parameters.AddWithValue("@SettingParameters", settingParameters);
                command.Parameters.Add("@isScheduleActionRecordInserted", SqlDbType.Bit);
                command.Parameters["@isScheduleActionRecordInserted"].Direction = ParameterDirection.Output;
                if (con.State == ConnectionState.Closed)
                    con.Open();
                command.ExecuteNonQuery();
                con.Close();

                return Convert.ToBoolean(command.Parameters["@isScheduleActionRecordInserted"].Value);
            }
        }
        #endregion


        #region Evaluate Post Submit Rules

        public void evaluatePostSubmitRules(Int32 applicantUserId, String ruleObjectXml, Int32 systemUserId)
        {
            ClientDBContext.usp_Rule_EvaluatePostSubmitRules(applicantUserId, ruleObjectXml, systemUserId);
        }

        #endregion

        #region post submit rule On expiary

        /// <summary>
        /// get a list of applicanr compliance item data which are expiring today or already expired
        /// </summary>
        /// <param name="currentDate">Today Date</param>
        /// <returns></returns>
        public List<ExpiredItemDataList> getExpiringItemData(Int32 chunkSize, Int32 userId)
        {
            return ClientDBContext.SetItemStatusToExpire(chunkSize, userId).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        private Int32 GetReviewStatusByCode(String statusCode)
        {
            return ClientDBContext.lkpItemComplianceStatus.Where(cmpStatus => cmpStatus.Code.ToLower() == statusCode.ToLower()
                                                                && !cmpStatus.IsDeleted).FirstOrDefault().ItemComplianceStatusID;
        }

        /// <summary>
        /// update the status of item data to expired.
        /// </summary>
        /// <param name="itemDataId"></param>
        public void UpdateItemDataStatusToExpire(Int32 itemDataId)
        {
            ApplicantComplianceItemData applicantComplianceItemData = ClientDBContext.ApplicantComplianceItemDatas.FirstOrDefault(x => x.ApplicantComplianceItemID == itemDataId && x.IsDeleted == false);
            if (applicantComplianceItemData != null)
            {
                Int32 expiredItemStatusId = GetReviewStatusByCode(ApplicantItemComplianceStatus.Expired.GetStringValue());
                applicantComplianceItemData.StatusID = expiredItemStatusId;
                ClientDBContext.SaveChanges();
            }
        }

        /// <summary>
        /// Get Expiring Compliance Items
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="beforeExpiry"></param>
        /// <param name="expiryFrequency"></param>
        /// <param name="entitySetName"></param>
        /// <param name="today"></param>
        /// <returns></returns>
        public List<GetExpiredItemDataList> GetExpiringComplianceItems(Int32 tenantId, String subEventCode, Int32 subEventId, String entitySetName, Int32 chunkSize)
        {
            try
            {
                List<GetExpiredItemDataList> lstExpiringItems = ClientDBContext.GetComplianceItemDataToExpire(tenantId, subEventCode, subEventId, entitySetName).Take(chunkSize).ToList();
                return lstExpiringItems;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// get a list of applicanr compliance item data whose next action is set to be execute category rules.
        /// </summary>
        /// <param name="currentDate">Today Date</param>
        /// <returns></returns>
        public List<GetActionScheduleExecuteCategoryRulesList> getActionActionExecuteCategoryRules(Int32 chunkSize)
        {
            return ClientDBContext.usp_GetActionScheduleExecuteCategoryRulesList(chunkSize).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduleActionId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Boolean inactiveProcessedScheduleAction(Int32 scheduleActionId, Int32 userId)
        {
            ScheduledAction scheduleAction = _ClientDBContext.ScheduledActions.FirstOrDefault(x => x.SA_ID == scheduleActionId &&
                                                                                                x.SA_IsActive
                                                                                                && !x.SA_IsDeleted);
            if (scheduleAction != null)
            {
                scheduleAction.SA_IsActive = false;
                scheduleAction.SA_ModifiedByID = userId;
                scheduleAction.SA_ModifiedOn = DateTime.Now;
                _ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// for getting count of total record for which category rules scheduled to be reoccur.
        /// </summary>
        /// <returns></returns>
        public Int32 getCountRecordsScheduleExecutecategoryrule()
        {
            DateTime currentTime = DateTime.Now;
            return _ClientDBContext.ScheduledActions.Where(obj => obj.SA_ScheduleDate <= currentTime && obj.SA_IsActive
                                     && obj.SA_ObjectTypeID == 2 && obj.lkpScheduledActionType.SAT_Code == "ECR").Count();
        }

        #region Upcoming Non Compliance Categories

        /// <summary>
        /// Send mail to all applicant for upcomming Non Compliance Categories
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="tenantName">tenantName</param>
        /// <param name="chunkSize">chunkSize</param>
        /// <returns></returns>

        public List<GetUpcomingNonComplianceCategories> GetUpcomingNonComplianceCategory(Int32 tenantId, Int32 chunkSize)
        {
            return ClientDBContext.usp_GetUpcomingNonComplianceCategoriesList(tenantId, chunkSize).ToList();
        }

        #endregion

        #endregion

        #endregion

        #endregion

        /// <summary>
        /// Get a list of applicant compliance category data which are expiring today or already expired
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetExpiringCategoryData(Int32 chunkSize, Int32 userId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("dbo.usp_SetCategoryDataStatusToExpire", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                command.Parameters.AddWithValue("@UserID", userId);
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

        List<ScheduledAction> IRuleRepository.GetObjectDeletionActiveScheduleActionList(Int32 chunkSize)
        {
            String DeletionObjectCode = ScheduleActionType.EXECUTE_RULES_AFTER_OBJECT_DELETION.GetStringValue();
            return ClientDBContext.ScheduledActions.Where(cond => cond.SA_IsActive && !cond.SA_IsDeleted && cond.lkpScheduledActionType.SAT_Code == DeletionObjectCode
                                                                    && cond.SA_ScheduleDate <= DateTime.Now)
                                                                    .Take(chunkSize).ToList();
        }

        void IRuleRepository.ExecuteRulesOnObjectDeletion(Int32 packageSubscriptionId, Int32 removedObjectTypeId, Int32 currentUserId, Int32 removedObjectId)
        {
            EntityConnection connection = ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_ExecuteBuisnessRules", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageSubscriptionId", packageSubscriptionId);
                command.Parameters.AddWithValue("@RemovedObjectTypeId", removedObjectTypeId);
                command.Parameters.AddWithValue("@RemovedObjectId", removedObjectId);
                command.Parameters.AddWithValue("@SystemUserID", currentUserId);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        #region UAT-1217:Notification to correspond with UAT-1209
        DataTable IRuleRepository.GetNonComplianceRequiredCategoryActionData(Int32 remBfrExp, Int32 remExpFrq, Int32 chunkSize, Int32 tenantId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("dbo.usp_GetUpcomingNonComplianceRequiredCategoryData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BeforeDays", remBfrExp);
                command.Parameters.AddWithValue("@ExpiryFrequency", remExpFrq);
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                command.Parameters.AddWithValue("@TenantID", tenantId);
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
        #endregion

        /// <summary>
        /// get the item list from ItemSeriesItem table on basis on series id
        /// </summary>
        /// <param name="seriesId">selected series id</param>
        /// <returns></returns>
        List<ItemSeriesItem> IRuleRepository.GetItemSeriesItemsBySeriesId(Int32 seriesId)
        {
            return _ClientDBContext.ItemSeriesItems.Where(isi => isi.ISI_ItemSeriesID == seriesId && !isi.ISI_IsDeleted).ToList();
        }


        /// <summary>
        /// get the item Attribute from ItemSeriesAttribute table on basis on series id
        /// </summary>
        /// <param name="seriesId">selected series id</param>
        /// <returns></returns>
        List<ItemSeriesAttribute> IRuleRepository.GetItemSeriesAttributeBySeriesId(Int32 seriesId)
        {
            return _ClientDBContext.ItemSeriesAttributes.Where(isa => isa.ISA_ItemSeriesID == seriesId && !isa.ISA_IsDeleted).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <returns></returns>
        List<RuleMapping> IRuleRepository.GetShotSeriesRuleMappings(Int32 seriesId, Int32 seriesObjectTypeId, Int32 seriesItemObjectTypeId)
        {

            List<ItemSeriesItem> lstSeriesItem = ClientDBContext.ItemSeriesItems.Where(cond => cond.ISI_ItemSeriesID == seriesId && !cond.ISI_IsDeleted).ToList();
            List<Int32> seriesItemIds = lstSeriesItem.Select(sel => sel.ISI_ID).ToList();
            List<RuleSetObject> lstRuleSetObject = ClientDBContext.RuleSetObjects.Where(cond =>
                                                                            (cond.RLSO_ObjectID == seriesId && cond.RLSO_ObjectTypeID == seriesObjectTypeId)
                                                                            || (seriesItemIds.Contains(cond.RLSO_ObjectID) && cond.RLSO_ObjectTypeID == seriesItemObjectTypeId))
                                                                            .ToList();
            List<Int32> ruleSetIds = new List<Int32>();
            if (!lstRuleSetObject.IsNullOrEmpty())
            {
                ruleSetIds = lstRuleSetObject.Select(cond => cond.RLSO_RuleSetID).Distinct().ToList();
            }

            return ClientDBContext.RuleMappings.Where(rm => ruleSetIds.Contains(rm.RLM_RuleSetID) && !rm.RLM_IsDeleted).ToList();
        }

        Boolean IRuleRepository.UpdateShotSeriesRuleMapping(RuleMapping ruleMapping, List<Int32> objectIds, Int32 objectTypeId, Int32 currentUserId)
        {
            RuleMapping ruleMappingInDb = GetRuleMapping(ruleMapping.RLM_ID, true);
            if (ruleMappingInDb != null)
            {

                List<RuleSetObject> existingRuleSetObjectList = ruleMappingInDb.RuleSet.RuleSetObjects.Where(cond => !cond.RLSO_IsDeleted).ToList();


                foreach (RuleSetObject existingRuleSetObject in existingRuleSetObjectList)
                {
                    if (!objectIds.Contains(existingRuleSetObject.RLSO_ObjectID))
                    {
                        existingRuleSetObject.RLSO_IsDeleted = true;
                        existingRuleSetObject.RLS_ModifiedById = currentUserId;
                        existingRuleSetObject.RLS_ModifiedOn = DateTime.Now;
                    }
                }


                List<Int32> objectIdsToBeInserted = objectIds.Except(existingRuleSetObjectList.Select(sel => sel.RLSO_ObjectID)).ToList();
                foreach (var objectId in objectIdsToBeInserted)
                {
                    RuleSetObject ruleSetObject = new RuleSetObject();
                    ruleSetObject.RLSO_ObjectID = objectId;
                    ruleSetObject.RLSO_ObjectTypeID = objectTypeId;
                    ruleSetObject.RLSO_IsActive = true;
                    ruleSetObject.RLS_CreatedById = currentUserId;
                    ruleSetObject.RLS_CreatedOn = DateTime.Now;
                    ruleMappingInDb.RuleSet.RuleSetObjects.Add(ruleSetObject);
                }

                ruleMappingInDb.RLM_RuleTemplateID = ruleMapping.RLM_RuleTemplateID;
                ruleMappingInDb.RLM_SuccessMessage = ruleMapping.RLM_SuccessMessage;
                ruleMappingInDb.RLM_ErrorMessage = ruleMapping.RLM_ErrorMessage;
                ruleMappingInDb.RLM_Name = ruleMapping.RLM_Name;
                ruleMappingInDb.RLM_ActionBlock = ruleMapping.RLM_ActionBlock;
                ruleMappingInDb.RLM_UIExpression = ruleMapping.RLM_UIExpression;
                ruleMappingInDb.RLM_IsActive = ruleMapping.RLM_IsActive;
                ruleMappingInDb.RLM_IsDeleted = ruleMapping.RLM_IsDeleted;
                ruleMappingInDb.RLM_ModifiedByID = ruleMapping.RLM_ModifiedByID;
                ruleMappingInDb.RLM_ModifiedOn = DateTime.Now;
                ruleMappingInDb.RLM_IsActive = ruleMapping.RLM_IsActive;
                ruleMappingInDb.RLM_IsCurrent = ruleMapping.RLM_IsCurrent;
                foreach (RuleMappingDetail mappingDetail in ruleMapping.RuleMappingDetails)
                {
                    RuleMappingDetail existingMapping = ruleMappingInDb.RuleMappingDetails.FirstOrDefault(obj => obj.RLMD_PlaceHolderName == mappingDetail.RLMD_PlaceHolderName);
                    if (existingMapping != null)
                    {
                        existingMapping.RLMD_PlaceHolderName = mappingDetail.RLMD_PlaceHolderName;
                        existingMapping.RLMD_ObjectID = mappingDetail.RLMD_ObjectID;
                        existingMapping.RLMD_ObjectTypeID = mappingDetail.RLMD_ObjectTypeID;
                        existingMapping.RLMD_RuleObjectMappingTypeID = mappingDetail.RLMD_RuleObjectMappingTypeID;
                        existingMapping.RLMD_ConstantType = mappingDetail.RLMD_ConstantType;
                        existingMapping.RLMD_ConstantValue = mappingDetail.RLMD_ConstantValue;
                        existingMapping.RLMD_ModifiedByID = mappingDetail.RLMD_ModifiedByID;
                        existingMapping.RLMD_ModifiedOn = DateTime.Now;
                        existingMapping.RLMD_IsDeleted = false;
                    }

                    var existingMappingObjectTreeList = existingMapping.RuleMappingObjectTrees.ToList();
                    foreach (RuleMappingObjectTree existingMappingObjectTree in existingMappingObjectTreeList)
                    {
                        //To Do delete existing record
                        ClientDBContext.RuleMappingObjectTrees.DeleteObject(existingMappingObjectTree);
                    }

                    foreach (RuleMappingObjectTree newMappingObjectTree in mappingDetail.RuleMappingObjectTrees)
                    {
                        if (newMappingObjectTree != null)
                        {
                            RuleMappingObjectTree ruleMappingObjectTree = new RuleMappingObjectTree();
                            ruleMappingObjectTree.RMOT_RuleMappingDetailID = newMappingObjectTree.RMOT_RuleMappingDetailID;
                            ruleMappingObjectTree.RMOT_RuleSetTreeID = newMappingObjectTree.RMOT_RuleSetTreeID;
                            ruleMappingObjectTree.RMOT_ObjectID = newMappingObjectTree.RMOT_ObjectID;
                            existingMapping.RuleMappingObjectTrees.Add(ruleMappingObjectTree);
                        }
                    }
                }
                ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        void IRuleRepository.DeleteShotSeriesRuleMapping(Int32 ruleMappingId, Int32 currentUserId)
        {
            RuleMapping ruleMappingToBeDeleted = ClientDBContext.RuleMappings.Where(rm => rm.RLM_ID == ruleMappingId).FirstOrDefault();
            ruleMappingToBeDeleted.RLM_IsDeleted = true;
            ruleMappingToBeDeleted.RLM_ModifiedOn = DateTime.Now;
            ruleMappingToBeDeleted.RLM_ModifiedByID = currentUserId;

            RuleSet ruleSetToBeDeleted = ruleMappingToBeDeleted.RuleSet;

            ruleSetToBeDeleted.RLS_IsDeleted = true;
            ruleSetToBeDeleted.RLS_ModifiedOn = DateTime.Now;
            ruleSetToBeDeleted.RLS_ModifiedByID = currentUserId;

            List<RuleSetObject> lstRuleSetObjectToBeDeleted = ruleSetToBeDeleted.RuleSetObjects.Where(cond => !cond.RLSO_IsDeleted).ToList();
            foreach (RuleSetObject ruleSetObjectToBeDeleted in lstRuleSetObjectToBeDeleted)
            {
                ruleSetObjectToBeDeleted.RLSO_IsDeleted = true;
                ruleSetObjectToBeDeleted.RLS_ModifiedOn = DateTime.Now;
                ruleSetObjectToBeDeleted.RLS_ModifiedById = currentUserId;
            }
            ClientDBContext.SaveChanges();
        }

        /// <summary>
        /// UAT-2044: To copy Rule Template from one tenant to another or within the same tenant
        /// </summary>
        /// <param name="ruleTemp"></param>
        /// <param name="templateName"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean IRuleRepository.CopyRuleTemplate(RuleTemplate ruleTemp, String templateName, Int32 currentUserId)
        {
            RuleTemplate newRuleTemplate = new RuleTemplate();

            newRuleTemplate.RLT_Name = templateName;
            newRuleTemplate.RLT_Code = Guid.NewGuid();
            newRuleTemplate.RLT_UIExpression = ruleTemp.RLT_UIExpression;
            newRuleTemplate.RLT_Description = ruleTemp.RLT_Description;
            newRuleTemplate.RLT_ResultType = ruleTemp.RLT_ResultType;
            newRuleTemplate.RLT_ActionType = ruleTemp.RLT_ActionType;
            newRuleTemplate.RLT_Type = ruleTemp.RLT_Type;
            newRuleTemplate.RLT_ObjectCount = ruleTemp.RLT_ObjectCount;
            newRuleTemplate.RLT_Notes = ruleTemp.RLT_Notes;
            newRuleTemplate.RLT_IsActive = ruleTemp.RLT_IsActive;
            newRuleTemplate.RLT_IsDeleted = ruleTemp.RLT_IsDeleted;
            newRuleTemplate.RLT_CreatedByID = currentUserId;
            newRuleTemplate.RLT_CreatedOn = DateTime.Now;

            foreach (RuleTemplateExpression ruleExpression in ruleTemp.RuleTemplateExpressions)
            {
                RuleTemplateExpression newRuleTempExp = new RuleTemplateExpression();
                newRuleTempExp.RLE_ExpressionOrder = ruleExpression.RLE_ExpressionOrder;
                newRuleTempExp.RLE_IsActive = ruleExpression.RLE_IsActive;
                newRuleTempExp.RLE_IsDeleted = ruleExpression.RLE_IsDeleted;
                newRuleTempExp.RLE_CreatedByID = currentUserId;
                newRuleTempExp.RLE_CreatedOn = DateTime.Now;

                Expression expressionToCopy = ruleExpression.Expression;
                Expression newExpression = new Expression();

                newExpression.EX_Name = expressionToCopy.EX_Name;
                newExpression.EX_Description = expressionToCopy.EX_Description;
                newExpression.EX_Expression = expressionToCopy.EX_Expression;
                newExpression.EX_ResultType = expressionToCopy.EX_ResultType;
                newExpression.EX_IsActive = expressionToCopy.EX_IsActive;
                newExpression.EX_IsDeleted = expressionToCopy.EX_IsDeleted;
                newExpression.EX_CreatedByID = currentUserId;
                newExpression.EX_CreatedOn = DateTime.Now;

                newRuleTempExp.Expression = newExpression;

                newRuleTemplate.RuleTemplateExpressions.Add(newRuleTempExp);
            }

            ClientDBContext.RuleTemplates.AddObject(newRuleTemplate);

            if (ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        String IRuleRepository.TestComplianceRule(String ruleInputData)
        {
            String resultXML = ClientDBContext.RuleEvaluatePreSubmitResults(ruleInputData).FirstOrDefault();
            return resultXML;
        }

        Tuple<Dictionary<Boolean, String>, Dictionary<Int32, Int32>> IRuleRepository.EvaluateDataEntryUIRules(Int32 packageSubscriptionID, String nonSeriesData, String seriesData)
        {
            Dictionary<Boolean, String> dicResponse = new Dictionary<Boolean, String>();

            Dictionary<Int32, Int32> lstItemRuleVoilated = new Dictionary<Int32, Int32>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_Rule_EvaluateUIRule_DataEntry", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageSubscriptionID", packageSubscriptionID);
                command.Parameters.AddWithValue("@NonSeriesData", nonSeriesData);
                command.Parameters.AddWithValue("@SeriesData", seriesData);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    String dtSaveResponse = Convert.ToString(ds.Tables[0].Rows[0]["Result"]);

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(dtSaveResponse);
                    XmlNode nodeStatus = null;
                    nodeStatus = xml.SelectSingleNode("Result/Status");
                    XmlNodeList nodeMessages = xml.SelectNodes("Result/Messages/Message");
                    Int32 statusCode = AppConsts.NONE;
                    String message = String.Empty;
                    if (nodeStatus.IsNullOrEmpty())
                    {
                        nodeStatus = xml.SelectSingleNode("Result");
                    }
                    List<String> lstMessage = new List<string>();
                    if (!nodeMessages.IsNullOrEmpty())
                    {
                        foreach (XmlNode xmlNode in nodeMessages)
                        {
                            if (xmlNode.IsNotNull())
                            {
                                lstMessage.Add(xmlNode.InnerText);
                            }
                        }
                    }
                    message = String.Join("<br/>", lstMessage);
                    statusCode = Convert.ToInt32(nodeStatus["Code"].InnerText);
                    dicResponse.Add(Convert.ToBoolean(statusCode), message);

                    XmlNodeList nodeRuleData = xml.SelectNodes("Result/RuleData");
                    if (!nodeRuleData.IsNullOrEmpty())
                    {
                        foreach (XmlNode xmlNode in nodeRuleData)
                        {
                            Int32 catId = Convert.ToInt32(xmlNode["CatId"].InnerXml);
                            Int32 itemId = Convert.ToInt32(xmlNode["ItemId"].InnerText);
                            lstItemRuleVoilated.Add(itemId, catId);
                        }
                    }

                }
            }
            return new Tuple<Dictionary<Boolean, String>, Dictionary<Int32, Int32>>(dicResponse, lstItemRuleVoilated);
        }

        void IRuleRepository.EvaluateRequirementPostSubmitRules(String ruleObjectXML, Int32 reqSubscriptionId, Int32 systemUserId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
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

        #region UAT-2725 Trigger Category Compliance check

        Boolean IRuleRepository.IsTriggerForOtherCategoryNeeded(Int32 categoryID)
        {
            return Convert.ToBoolean(_ClientDBContext.ComplianceCategories.Where(con => con.ComplianceCategoryID == categoryID && !con.IsDeleted && con.IsActive).Select(sel => sel.TriggerOtherCategoryRules).FirstOrDefault());
        }

        List<Int32> IRuleRepository.GetComplianceCategoryIdsByPackageID(Int32 packageId)
        {
            return _ClientDBContext.CompliancePackageCategories.Where(con => con.CPC_PackageID == packageId && !con.CPC_IsDeleted && !con.ComplianceCategory.IsDeleted).Select(sel => sel.ComplianceCategory.ComplianceCategoryID).ToList();
        }
        #endregion
        //UAT-2740
        String IRuleRepository.CalculateDueDate(String resultXml)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("dbo.usp_CalculateDueDate", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Result", resultXml);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return Convert.ToString(ds.Tables[0].Rows[0]["ResultXml"]);
                }
            }
            return String.Empty;
        }

        List<ComplianceExceptionExpiryData> IRuleRepository.GetComplianceExceptionAboutToExpire(Int32 tenantId, Int32 subEventId, Int32 chunkSize)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("dbo.usp_GetComplianceExceptionAboutToExpire", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantId", tenantId);
                command.Parameters.AddWithValue("@SubEventId", subEventId);
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                List<ComplianceExceptionExpiryData> lstUnArchivalRequestDetails = new List<ComplianceExceptionExpiryData>();
                if (ds.Tables.Count > AppConsts.NONE)
                {
                    lstUnArchivalRequestDetails = ds.Tables[0].AsEnumerable().Select(col =>
                          new ComplianceExceptionExpiryData
                          {
                              ApplicantComplianceCategoryID = col["ApplicantComplianceCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(col["ApplicantComplianceCategoryID"]),
                              ApplicantComplianceItemID = col["ApplicantComplianceItemID"] == DBNull.Value ? 0 : Convert.ToInt32(col["ApplicantComplianceItemID"]),
                              LastName = col["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["LastName"]),
                              FirstName = col["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FirstName"]),
                              ExpiryDate = Convert.ToDateTime(col["ExpiryDate"]),
                              ItemCategoryName = col["ItemCategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ItemCategoryName"]),
                              PrimaryEmailAddress = col["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["PrimaryEmailAddress"]),
                              HierarchyNodeID = Convert.ToInt32(col["HierarchyNodeID"]),
                              OrganizationUserID = Convert.ToInt32(col["OrganizationUserID"])
                          }).ToList();
                }
                return lstUnArchivalRequestDetails;

            }

        }

        List<RuleSetData> IRuleRepository.GetRuleSetDataByObjectId(Int32 ObjectId, Int32 ObjectTypeId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("dbo.usp_GetRuleSetDataByObjectId", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ObjectID", ObjectId);
                command.Parameters.AddWithValue("@ObjectTypeID", ObjectTypeId);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                List<RuleSetData> lstUnArchivalRequestDetails = new List<RuleSetData>();
                if (ds.Tables.Count > AppConsts.NONE)
                {
                    lstUnArchivalRequestDetails = ds.Tables[0].AsEnumerable().Select(col =>
                          new RuleSetData
                          {
                              RuleMappingId = col["RuleMappingId"] == DBNull.Value ? 0 : Convert.ToInt32(col["RuleMappingId"]),
                              RuleSetId = col["RuleSetId"] == DBNull.Value ? 0 : Convert.ToInt32(col["RuleSetId"]),
                              PackageId = col["PackageId"] == DBNull.Value ? 0 : Convert.ToInt32(col["PackageId"]),
                              CategoryId = col["CategoryId"] == DBNull.Value ? 0 : Convert.ToInt32(col["CategoryId"]),
                              AssignmentHierarchyID = col["AssignmentHierarchyID"] == DBNull.Value ? 0 : Convert.ToInt32(col["AssignmentHierarchyID"]),
                              RuleAssociationId = col["RuleAssociationId"] == DBNull.Value ? 0 : Convert.ToInt32(col["RuleAssociationId"])
                          }).ToList();
                }
                return lstUnArchivalRequestDetails;

            }

        }

        Boolean IRuleRepository.IsRuleAssociationExists(Int32 AffectedRuleId)
        {
            //EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            //using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            //{
            //    SqlCommand command = new SqlCommand("dbo.usp_IsRuleAssociationExists", con);
            //    command.CommandType = CommandType.StoredProcedure;
            //    command.Parameters.AddWithValue("@AffectedRuleId", AffectedRuleId);
            //    SqlDataAdapter adp = new SqlDataAdapter();
            //    adp.SelectCommand = command;
            //    DataSet ds = new DataSet();
            //    adp.Fill(ds);
            //    if (ds.Tables.Count > 0)
            //    {
            //        return Convert.ToBoolean(ds.Tables[0].Rows[0]["Exits"]);
            //    }
            //}
            return false;
        }
        CompliancePackage IRuleRepository.GetCompliancePackageByPackageId(Int32 packageId)
        {
            return _ClientDBContext.CompliancePackages.Where(c => c.IsActive == true && c.IsDeleted == false && c.CompliancePackageID == packageId).FirstOrDefault();
        }
    }
}
