using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.SharedDataEntity;
using DAL.Interfaces;
using INTSOF.Utils;
using System.Web.Configuration;

namespace DAL.Repository
{
    public class SharedRequirementRuleRepository : BaseRepository, ISharedRequirementRuleRepository
    {
        #region Variables

        #region public Variables

        #endregion

        #region Private Variables

        private ADB_SharedDataEntities _sharedDataDBContext;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor to initialize the context
        /// </summary>
        public SharedRequirementRuleRepository()
        {
            _sharedDataDBContext = base.SharedDataDBContext;
        }

        #endregion

        #region Methods
        #region Manage RuleTemplate

        /// <summary>
        /// GetRuleTemplates
        /// </summary>
        /// <returns></returns>
        List<RequirementRuleTemplate> ISharedRequirementRuleRepository.GetlstRuleTemplate()
        {
            return SharedDataDBContext.RequirementRuleTemplates.Include("lkpRuleResultType").Where(r => r.RLT_IsDeleted == false).ToList();
        }

        /// <summary>
        /// GetRuleTemplate
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        RequirementRuleTemplate ISharedRequirementRuleRepository.GetRuleTemplate(Int32 ruleId)
        {
            RequirementRuleTemplate rule = SharedDataDBContext.RequirementRuleTemplates.Include("RequirementRuleTemplateExpressions.RequirementExpression").Where(r => r.RLT_ID == ruleId).FirstOrDefault();
            return rule;
        }
        

        /// <summary>
        /// UpdateRuleTemplate
        /// </summary>
        /// <param name="rule"></param>
        void ISharedRequirementRuleRepository.UpdateRuleTemplate(Entity.ComplianceRuleTemplate complianceRuleTemplate, List<Int32> expressionIds)
        {
            RequirementRuleTemplate ruleTemplate = SharedDataDBContext.RequirementRuleTemplates.Include("RequirementRuleTemplateExpressions.RequirementExpression").Where(rr => rr.RLT_ID == complianceRuleTemplate.RLT_ID && rr.RLT_IsActive == true && rr.RLT_IsDeleted == false).FirstOrDefault();
            String delimiterKey = WebConfigurationManager.AppSettings[AppConsts.DELIMITER];
            String delimiters = "|||";
            delimiterKey = delimiterKey.IsNull() || delimiterKey == "" ? delimiters : delimiterKey;

            ruleTemplate.RLT_Code = ruleTemplate.RLT_Code.Equals(Guid.Empty) ? Guid.NewGuid() : ruleTemplate.RLT_Code;
            ruleTemplate.RLT_UIExpression = complianceRuleTemplate.RuleGroupExpression;
            ruleTemplate.RLT_Name = complianceRuleTemplate.RLT_Name;
            ruleTemplate.RLT_Description = complianceRuleTemplate.RLT_Description;
            ruleTemplate.RLT_ResultType = complianceRuleTemplate.RLT_ResultType;
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
                    ruleTemplate.RequirementRuleTemplateExpressions.Add
                    (
                        new RequirementRuleTemplateExpression()
                        {
                            RLE_CreatedByID = complianceRuleTemplate.RLT_CreatedByID,
                            RLE_CreatedOn = complianceRuleTemplate.RLT_CreatedOn,
                            RLE_IsActive = true,
                            RLE_ExpressionOrder = rExp.ExpressionOrder,
                            RequirementExpression = new RequirementExpression()
                            {
                                REX_ID = rExp.EX_ID,
                                REX_Name = rExp.EX_Name,
                                REX_Description = rExp.EX_Description,
                                REX_ResultType = complianceRuleTemplate.RLT_ResultType,
                                REX_RequiremntExpression = expression,
                                REX_IsActive = true,
                                REX_IsDeleted = false,
                                REX_CreatedByID = complianceRuleTemplate.RLT_CreatedByID,
                                REX_CreatedOn = complianceRuleTemplate.RLT_CreatedOn,
                            }
                        });
                }
                else
                {
                    RequirementRuleTemplateExpression rex = ruleTemplate.RequirementRuleTemplateExpressions.FirstOrDefault(fx => fx.RLE_ExpressionID == rExp.EX_ID
                                                 && fx.RLE_IsActive == true && fx.RLE_IsDeleted == false);
                    if (rex.IsNotNull())
                    {
                        rex.RLE_ModifiedByID = complianceRuleTemplate.RLT_ModifiedByID;
                        rex.RLE_ModifiedOn = complianceRuleTemplate.RLT_ModifiedOn;
                        rex.RLE_IsActive = true;
                        rex.RLE_ExpressionOrder = rExp.ExpressionOrder;

                        rex.RequirementExpression.REX_ID = rExp.EX_ID;
                        rex.RequirementExpression.REX_Name = rExp.EX_Name;
                        rex.RequirementExpression.REX_Description = rExp.EX_Description;
                        rex.RequirementExpression.REX_ResultType = complianceRuleTemplate.RLT_ResultType;
                        rex.RequirementExpression.REX_RequiremntExpression = expression;
                        rex.RequirementExpression.REX_IsActive = true;
                        rex.RequirementExpression.REX_IsDeleted = false;
                        rex.RequirementExpression.REX_ModifiedByID = complianceRuleTemplate.RLT_ModifiedByID;
                        rex.RequirementExpression.REX_ModifiedOn = complianceRuleTemplate.RLT_ModifiedOn;
                    }
                }
            }

            //Delete old records from saved records
            RequirementRuleTemplateExpression ruleTemplateExpression = null;
            if (expressionIds.IsNotNull())
            {
                foreach (var exId in expressionIds)
                {
                    ruleTemplateExpression = ruleTemplate.RequirementRuleTemplateExpressions.FirstOrDefault(x => x.RLE_ExpressionID == exId);
                    if (ruleTemplateExpression.IsNotNull())
                    {
                        //Update RuleTemplateExpression table
                        ruleTemplateExpression.RLE_IsDeleted = true;
                        ruleTemplateExpression.RLE_ModifiedByID = ruleTemplate.RLT_ModifiedByID;
                        ruleTemplateExpression.RLE_ModifiedOn = ruleTemplate.RLT_ModifiedOn;

                        //Update Expression table
                        if (ruleTemplateExpression.RequirementExpression.IsNotNull())
                        {
                            ruleTemplateExpression.RequirementExpression.REX_IsDeleted = true;
                            ruleTemplateExpression.RequirementExpression.REX_ModifiedByID = ruleTemplate.RLT_ModifiedByID;
                            ruleTemplateExpression.RequirementExpression.REX_ModifiedOn = ruleTemplate.RLT_ModifiedOn;
                        }
                    }
                }
            }

            SharedDataDBContext.SaveChanges();
        }

        /// <summary>
        /// AddRuleTemplate
        /// </summary>
        /// <param name="ruleTemplate"></param>
        void ISharedRequirementRuleRepository.AddRuleTemplate(Entity.ComplianceRuleTemplate complianceRuleTemplate)
        {
            String delimiterKey = WebConfigurationManager.AppSettings[AppConsts.DELIMITER];
            String delimiters = "|||";
            delimiterKey = delimiterKey.IsNull() || delimiterKey == "" ? delimiters : delimiterKey;
            RequirementRuleTemplate ruleTemplate = new RequirementRuleTemplate();

            ruleTemplate.RLT_Code = ruleTemplate.RLT_Code.Equals(Guid.Empty) ? Guid.NewGuid() : ruleTemplate.RLT_Code;
            ruleTemplate.RLT_UIExpression = complianceRuleTemplate.RuleGroupExpression;
            ruleTemplate.RLT_Name = complianceRuleTemplate.RLT_Name;
            ruleTemplate.RLT_Description = complianceRuleTemplate.RLT_Description;
            ruleTemplate.RLT_ResultType = complianceRuleTemplate.RLT_ResultType;
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

                ruleTemplate.RequirementRuleTemplateExpressions.Add
                (
                    new RequirementRuleTemplateExpression()
                    {
                        RLE_CreatedByID = complianceRuleTemplate.RLT_CreatedByID,
                        RLE_CreatedOn = DateTime.Now,
                        RLE_IsActive = complianceRuleTemplate.RLT_IsActive,
                        RLE_ExpressionOrder = rExp.ExpressionOrder,
                        RequirementExpression = new RequirementExpression()
                        {
                            REX_ID = rExp.EX_ID,
                            REX_Name = rExp.EX_Name,
                            REX_Description = rExp.EX_Description,
                            REX_ResultType = complianceRuleTemplate.RLT_ResultType,
                            REX_RequiremntExpression = expression,
                            REX_IsActive = complianceRuleTemplate.RLT_IsActive,
                            REX_IsDeleted = rExp.EX_IsDeleted,
                            REX_CreatedByID = complianceRuleTemplate.RLT_CreatedByID,
                            REX_CreatedOn = DateTime.Now
                        }
                    }
                );
            }
            SharedDataDBContext.RequirementRuleTemplates.AddObject(ruleTemplate);
            SharedDataDBContext.SaveChanges();
        }

        /// <summary>
        /// DeleteRuleTemplate
        /// </summary>
        /// <param name="ruleId"></param>
        /// <param name="currentUserId"></param>
        void ISharedRequirementRuleRepository.DeleteRuleTemplate(Int32 ruleId, Int32 currentUserId)
        {
            RequirementRuleTemplate ruleTemplate = SharedDataDBContext.RequirementRuleTemplates.Where(r => r.RLT_ID == ruleId).FirstOrDefault();
            ruleTemplate.RLT_IsDeleted = true;
            ruleTemplate.RLT_ModifiedByID = currentUserId;
            ruleTemplate.RLT_ModifiedOn = DateTime.Now;
            SharedDataDBContext.SaveChanges();

        }

        /// <summary>
        /// GetExpressionOperators
        /// </summary>
        /// <returns></returns>
        //List<lkpExpressionOperator> ISharedRequirementRuleRepository.GetExpressionOperators()
        //{
        //    return SharedDataDBContext.lkpExpressionOperators.Include("lkpExpressionOperatorType").AsQueryable().ToList();
        //}

        /// <summary>
        /// ValidateRuleTemplate
        /// </summary>
        /// <param name="ruleTemplateXML"></param>
        /// <returns></returns>
        String ISharedRequirementRuleRepository.ValidateRuleTemplate(string ruleTemplateXML)
        {
         return base.SharedDataDBContext.usp_ValidateRuleTemplate(ruleTemplateXML).FirstOrDefault(); 
        }

        #endregion

        #endregion
    }
}
