using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

using INTSOF.Utils;
using Entity.SharedDataEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Xml;
using System.Xml.Linq;



namespace Business.RepoManagers
{
    public class SharedRequirementRuleManager
    {

        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static SharedRequirementRuleManager()
        {
            BALUtils.ClassModule = "Shared Requirement Rule Manager";
        }

        #endregion

        public static List<RequirementRuleTemplate> GetlstRuleTemplate()
        {
            try
            {
                return BALUtils.GetSharedRequirementRuleRepoInstance().GetlstRuleTemplate();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static RequirementRuleTemplate GetRuleTemplateDetails(Int32 ruleTemplateId)
        {
            try
            {
                return BALUtils.GetSharedRequirementRuleRepoInstance().GetRuleTemplate(ruleTemplateId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// To delete Rule Template record
        /// </summary>
        /// <param name="ruleId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        public static void DeleteRuleTemplate(Int32 ruleId, Int32 currentUserId)
        {
            BALUtils.GetSharedRequirementRuleRepoInstance().DeleteRuleTemplate(ruleId, currentUserId);
        }

        /// <summary>
        /// Gets the list of all Rule Types.
        /// </summary>
        /// <returns>
        /// IQueryable List of lkpRuleType Objects.
        /// </returns>
        public static List<lkpRuleType> GetRuleTypes()
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<lkpRuleType>().AsQueryable().ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Gets the list of all Rule Result Types.
        /// </summary>
        /// <returns>
        /// IQueryable List of lkpRuleResultType Objects.
        /// </returns>
        public static List<lkpRuleResultType> GetRuleResultTypes()
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<lkpRuleResultType>().ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Gets the list of all Rule Action Types.
        /// </summary>
        /// <returns>
        /// IQueryable List of lkpRuleActionType Objects.
        /// </returns>
        public static List<lkpRuleActionType> GetRuleActionTypes()
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<lkpRuleActionType>().ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// GetExpressionOperators
        /// </summary>
        /// <returns></returns>
        public static List<lkpExpressionOperator> GetExpressionOperators()
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<lkpExpressionOperator>().ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// GetExpressionOperators
        /// </summary>
        /// <returns></returns>
        public static List<lkpExpressionOperator> GetExpressionOperators(int noOfObjects)
        {
            try
            {
                //List<lkpExpressionOperator> lkpOperators = null;
                List<lkpExpressionOperator> lkpOperators = LookupManager.GetSharedDBLookUpData<lkpExpressionOperator>().ToList();
                Int32 operatorCount = lkpOperators.Count;

                lkpOperators.Insert(operatorCount, new lkpExpressionOperator() { EO_ID = 100 + operatorCount, EO_Name = "(", EO_UILabel = "(", EO_SQL = "(" });
                operatorCount += 1;
                lkpOperators.Insert(operatorCount, new lkpExpressionOperator() { EO_ID = 100 + operatorCount, EO_Name = ")", EO_UILabel = ")", EO_SQL = ")" });

                for (int index = 1; index <= noOfObjects; index++)
                {
                    lkpOperators.Insert(index - 1, new lkpExpressionOperator() { EO_ID = 1000 + index, EO_Name = "[Object" + index + "]", EO_UILabel = "[Object" + index + "]", EO_SQL = "[Object" + index + "]" });
                }
                return lkpOperators;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// To get Rule Template
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static Entity.ComplianceRuleTemplate GetRuleTemplate(Int32 ruleId)
        {
            try
            {
                String delimiterKey = WebConfigurationManager.AppSettings[AppConsts.DELIMITER];
                String delimiters = "|||";
                delimiterKey = delimiterKey.IsNull() || delimiterKey == "" ? delimiters : delimiterKey;
                RequirementRuleTemplate rule = BALUtils.GetSharedRequirementRuleRepoInstance().GetRuleTemplate(ruleId);
                Entity.ComplianceRuleTemplate complianceRuleTemplate = new Entity.ComplianceRuleTemplate();

                complianceRuleTemplate.RuleGroupExpression = rule.RLT_UIExpression;
                complianceRuleTemplate.RLT_ID = rule.RLT_ID;
                complianceRuleTemplate.RLT_Name = rule.RLT_Name;
                complianceRuleTemplate.RLT_Description = rule.RLT_Description;
                complianceRuleTemplate.RLT_ResultType = rule.RLT_ResultType;
                complianceRuleTemplate.RLT_Code = rule.RLT_Code;
                complianceRuleTemplate.RLT_ObjectCount = rule.RLT_ObjectCount;
                complianceRuleTemplate.RLT_Notes = rule.RLT_Notes;
                complianceRuleTemplate.RLT_IsActive = rule.RLT_IsActive;
                complianceRuleTemplate.RLT_IsDeleted = rule.RLT_IsDeleted;
                complianceRuleTemplate.RLT_CreatedByID = rule.RLT_CreatedByID;
                complianceRuleTemplate.RLT_CreatedOn = rule.RLT_CreatedOn;
                complianceRuleTemplate.RLT_ModifiedByID = rule.RLT_ModifiedByID;
                complianceRuleTemplate.RLT_ModifiedOn = rule.RLT_ModifiedOn;
                complianceRuleTemplate.RLT_IsCopied = rule.RLT_IsCopied;
                //Set IsRuleTemplateAssociatedWithRule = true if RuleTemplate is associated with any Rule (to restricts the user to save any change).
                if (rule.RequirementObjectRules.IsNotNull() && rule.RequirementObjectRules.Count(x => x.ROR_IsDeleted == false) > 0)
                {
                    complianceRuleTemplate.IsRuleTemplateAssociatedWithRule = true;
                }
                List<RequirementRuleTemplateExpression> expressions = rule.RequirementRuleTemplateExpressions.Where(x => x.RLE_IsActive && x.RLE_IsDeleted == false).OrderBy(x => x.RLE_ExpressionOrder).ToList();
                foreach (RequirementRuleTemplateExpression rExp in expressions)
                {
                    Entity.ComplianceRuleExpressionTemplate complianceRuleExpressionTemplate = new Entity.ComplianceRuleExpressionTemplate();

                    complianceRuleExpressionTemplate.EX_ID = rExp.RequirementExpression.REX_ID;
                    complianceRuleExpressionTemplate.EX_Name = rExp.RequirementExpression.REX_Name;
                    complianceRuleExpressionTemplate.EX_Description = rExp.RequirementExpression.REX_Description;
                    complianceRuleExpressionTemplate.EX_Expression = rExp.RequirementExpression.REX_RequiremntExpression;
                    complianceRuleExpressionTemplate.EX_IsActive = rExp.RequirementExpression.REX_IsActive;
                    complianceRuleExpressionTemplate.EX_IsDeleted = rExp.RequirementExpression.REX_IsDeleted;
                    complianceRuleExpressionTemplate.EX_CreatedByID = rExp.RequirementExpression.REX_CreatedByID;
                    complianceRuleExpressionTemplate.EX_CreatedOn = rExp. RequirementExpression.REX_CreatedOn;
                    complianceRuleExpressionTemplate.EX_ModifiedByID = rExp.RequirementExpression.REX_ModifiedByID;
                    complianceRuleExpressionTemplate.EX_ModifiedOn = rExp.RequirementExpression.REX_ModifiedOn;
                    complianceRuleExpressionTemplate.EX_Code = rExp.RequirementExpression.REX_Code;
                    complianceRuleExpressionTemplate.ExpressionOrder = rExp.RLE_ExpressionOrder;

                    // Set Expression Elements
                    String[] expElements = complianceRuleExpressionTemplate.EX_Expression.Split(new String[] { delimiterKey }, StringSplitOptions.None);

                    foreach (String expElement in expElements)
                    {
                        if (expElement != String.Empty)
                        {
                            Entity.ComplianceRuleExpressionElementType ruleExpressionElementType = Entity.ComplianceRuleExpressionElementType.Operator;

                            String equalOperator = "EQUAL";

                            //if (expElement.StartsWith("O", StringComparison.OrdinalIgnoreCase))
                            if (expElement.StartsWith("[", StringComparison.OrdinalIgnoreCase))
                            {
                                ruleExpressionElementType = Entity.ComplianceRuleExpressionElementType.Object;
                            }
                            else if (expElement.StartsWith("E", StringComparison.OrdinalIgnoreCase) && !(equalOperator.Equals(expElement.ToUpper())))
                            {
                                ruleExpressionElementType = Entity.ComplianceRuleExpressionElementType.Expression;
                            }

                            complianceRuleExpressionTemplate.RuleExpressionElements.Add(new Entity.ComplianceRuleExpressionElement
                            {
                                ElementValue = expElement,
                                ExpressionElementType = ruleExpressionElementType
                            });
                        }
                    }

                    complianceRuleTemplate.ComplianceRuleExpressionTemplates.Add(complianceRuleExpressionTemplate);
                }
                return complianceRuleTemplate;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        

        /// <summary>
        /// UpdateRuleTemplate
        /// </summary>
        public static void UpdateRuleTemplate(Entity.ComplianceRuleTemplate complianceRuleTemplate, List<Int32> expressionIds)
        {
            try
            {
                BALUtils.GetSharedRequirementRuleRepoInstance().UpdateRuleTemplate(complianceRuleTemplate, expressionIds);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// AddRuleTemplate
        /// </summary>
        public static void AddRuleTemplate(Entity.ComplianceRuleTemplate complianceRuleTemplate)
        {
            try
            {
                BALUtils.GetSharedRequirementRuleRepoInstance().AddRuleTemplate(complianceRuleTemplate);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
       

        /// <summary>
        /// ValidateRuleTemplate
        /// </summary>
        /// <param name="ruleExpressionTemplates"></param>
        /// <param name="resultTypeCode"></param>
        /// <returns></returns>
        public static RuleProcessingResult ValidateRuleTemplate(List<Entity.ComplianceRuleExpressionTemplate> ruleExpressionTemplates, String resultTypeCode)
        {
            try
            {
                if (!(ruleExpressionTemplates.Count > 0))
                {
                    return new RuleProcessingResult
                    {
                        Status = 1,
                        ErrorMessage = "No expression defined."
                    };
                }
                String xmlResult = String.Empty;
                XmlDocument doc = new XmlDocument();
                XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("Rule"));
                el.AppendChild(doc.CreateElement("ResultType")).InnerText = resultTypeCode;
                XmlNode expGroup = el.AppendChild(doc.CreateElement("Expressions"));

                String delimiterKey = WebConfigurationManager.AppSettings[AppConsts.DELIMITER];
                String delimiters = "$$$";
                delimiterKey = delimiterKey.IsNull() || delimiterKey == "" ? delimiters : delimiterKey;

                foreach (Entity.ComplianceRuleExpressionTemplate ruleExpressionTemplate in ruleExpressionTemplates)
                {
                    XmlNode exp = expGroup.AppendChild(doc.CreateElement("Expression"));

                    String expressionName = ruleExpressionTemplate.EX_Name == "(Group)" ? "GE" : ruleExpressionTemplate.EX_Name;
                    exp.AppendChild(doc.CreateElement("Name")).InnerText = expressionName;

                    String definition = String.Empty;
                    for (int index = 0; index < ruleExpressionTemplate.RuleExpressionElements.Count; index++)
                    {
                        //definition +=GetExpressionOperator(ruleExpressionTemplate.RuleExpressionElements[index].ElementValue.ToUpper());  

                        String equalOperator = "EQUAL";
                        if (ruleExpressionTemplate.RuleExpressionElements[index].ElementValue.ToUpper().StartsWith("E", StringComparison.OrdinalIgnoreCase) && !(equalOperator.Equals(ruleExpressionTemplate.RuleExpressionElements[index].ElementValue.ToUpper())))
                        {
                            if (definition == String.Empty)
                            {
                                definition += ruleExpressionTemplate.RuleExpressionElements[index].ElementValue.ToUpper();
                            }
                            else
                            {
                                definition += delimiterKey + ruleExpressionTemplate.RuleExpressionElements[index].ElementValue.ToUpper();
                            }
                        }
                        else
                        {
                            if (definition == String.Empty)
                            {
                                definition += ruleExpressionTemplate.RuleExpressionElements[index].ElementOperator.ToUpper();
                            }
                            else
                            {
                                definition += delimiterKey + ruleExpressionTemplate.RuleExpressionElements[index].ElementOperator.ToUpper();
                            }
                        }
                    }
                    definition = definition.Trim();

                    //Implement CDATA object 
                    exp.AppendChild(doc.CreateElement("Definition")).InnerText = "<![CDATA[" + definition + "]]>";
                }
                xmlResult = BALUtils.GetSharedRequirementRuleRepoInstance().ValidateRuleTemplate(doc.OuterXml.ToString().Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&"));
                return ParseValidationXml(xmlResult);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static RuleProcessingResult ParseValidationXml(String outPutResult)
        {
            try
            {
                var doc = XDocument.Parse(outPutResult);
                RuleProcessingResult rProcessdResult = new RuleProcessingResult();
                rProcessdResult.Status = Convert.ToInt32(doc.Descendants().FirstOrDefault(x => x.Name == "Status").Value);
                rProcessdResult.Action = Convert.ToString(doc.Descendants().FirstOrDefault(x => x.Name == "Action").Value);
                rProcessdResult.Result = Convert.ToString(doc.Descendants().FirstOrDefault(x => x.Name == "Result").Value);
                rProcessdResult.UIExpressionLabel = Convert.ToString(doc.Descendants().FirstOrDefault(x => x.Name == "UIExpressionLabel").Value);
                rProcessdResult.SuccessMessage = Convert.ToString(doc.Descendants().FirstOrDefault(x => x.Name == "SuccessMessage").Value);
                rProcessdResult.ErrorMessage = Convert.ToString(doc.Descendants().FirstOrDefault(x => x.Name == "ErrorMessage").Value);
                return rProcessdResult;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                RuleProcessingResult rProcessdResult = new RuleProcessingResult();
                rProcessdResult.Status = AppConsts.NONE;
                rProcessdResult.ErrorMessage = "Some Internal error has occured while parsing the Xml";
                return rProcessdResult;
            }

        }

        /// <summary>
        /// To get Mathmatical Expression Operator
        /// </summary>
        /// <param name="expressionOperatorName"></param>
        /// <returns></returns>
        private static String GetExpressionOperator(String expressionOperatorName)
        {
            String expressionOperator = String.Empty;
            switch (expressionOperatorName)
            {
                case "PLUS":
                    expressionOperator = "+";
                    break;
                case "MINUS":
                    expressionOperator = "-";
                    break;
                case "MULTIPLY":
                    expressionOperator = "*";
                    break;
                case "DIVIDE":
                    expressionOperator = "/";
                    break;
                case "EQUAL":
                    expressionOperator = "=";
                    break;
                case "NOT EQUAL":
                    expressionOperator = "!=";
                    break;
                case "GREATER THAN":
                    expressionOperator = ">";
                    break;
                case "LESS THAN":
                    expressionOperator = "<";
                    break;
                case "GREATER THAN EQUAL TO":
                    expressionOperator = ">=";
                    break;
                case "LESS THAN EQUAL TO":
                    expressionOperator = "<=";
                    break;
                case "AND":
                    expressionOperator = "&&";
                    break;
                case "OR":
                    expressionOperator = "||";
                    break;
                case "NOT":
                    expressionOperator = "!";
                    break;
                default:
                    return expressionOperatorName + " ";

            }
            return "<![CDATA[" + expressionOperator + "]]>" + " ";
        }

    }
}