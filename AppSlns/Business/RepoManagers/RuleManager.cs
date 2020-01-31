#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  RuleManager.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Web.Configuration;

#endregion

#region Application Specific

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceRuleEngine;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Data;

#endregion

#endregion

namespace Business.RepoManagers
{
    /// <summary>
    /// This is a business class for compliance rule, which handles the operations at business layer.
    /// </summary>
    /// <remarks></remarks>
    public static class RuleManager
    {
        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static RuleManager()
        {
            BALUtils.ClassModule = "RuleManager";
        }

        #endregion

        #region Private Variables


        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the list of all Rule Types.
        /// </summary>
        /// <returns>
        /// IQueryable List of lkpRuleType Objects.
        /// </returns>
        public static List<lkpRuleType> GetRuleTypes(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpRuleType>(tenantId).AsQueryable().ToList();
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
        public static List<lkpRuleResultType> GetRuleResultTypes(Int32? tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpRuleResultType>(tenantId).ToList();
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
        public static List<lkpRuleActionType> GetRuleActionTypes(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpRuleActionType>(tenantId).ToList();
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
        public static Entity.ComplianceRuleTemplate GetRuleTemplate(Int32 ruleId, Int32 tenantId)
        {
            try
            {
                String delimiterKey = WebConfigurationManager.AppSettings[AppConsts.DELIMITER];
                String delimiters = "|||";
                delimiterKey = delimiterKey.IsNull() || delimiterKey == "" ? delimiters : delimiterKey;
                RuleTemplate rule = BALUtils.GetRuleRepoInstance(tenantId).GetRuleTemplate(ruleId);
                Entity.ComplianceRuleTemplate complianceRuleTemplate = new Entity.ComplianceRuleTemplate();

                complianceRuleTemplate.RuleGroupExpression = rule.RLT_UIExpression;
                complianceRuleTemplate.RLT_ID = rule.RLT_ID;
                complianceRuleTemplate.RLT_Name = rule.RLT_Name;
                complianceRuleTemplate.RLT_Description = rule.RLT_Description;
                complianceRuleTemplate.RLT_ResultType = rule.RLT_ResultType;
                complianceRuleTemplate.RLT_ActionType = rule.RLT_ActionType;
                complianceRuleTemplate.RLT_Type = rule.RLT_Type;
                complianceRuleTemplate.RLT_Code = rule.RLT_Code;
                complianceRuleTemplate.RLT_ObjectCount = rule.RLT_ObjectCount;
                complianceRuleTemplate.RLT_Notes = rule.RLT_Notes;
                complianceRuleTemplate.RLT_IsActive = rule.RLT_IsActive;
                complianceRuleTemplate.RLT_IsDeleted = rule.RLT_IsDeleted;
                complianceRuleTemplate.RLT_CreatedByID = rule.RLT_CreatedByID;
                complianceRuleTemplate.RLT_CreatedOn = rule.RLT_CreatedOn;
                complianceRuleTemplate.RLT_ModifiedByID = rule.RLT_ModifiedByID;
                complianceRuleTemplate.RLT_ModifiedOn = rule.RLT_ModifiedOn;

                //Set IsRuleTemplateAssociatedWithRule = true if RuleTemplate is associated with any Rule (to restricts the user to save any change).
                if (rule.RuleMappings.IsNotNull() && rule.RuleMappings.Count(x => x.RLM_IsDeleted == false) > 0)
                {
                    complianceRuleTemplate.IsRuleTemplateAssociatedWithRule = true;
                }
                List<RuleTemplateExpression> expressions = rule.RuleTemplateExpressions.Where(x => x.RLE_IsActive && x.RLE_IsDeleted == false).OrderBy(x => x.RLE_ExpressionOrder).ToList();
                foreach (RuleTemplateExpression rExp in expressions)
                {
                    Entity.ComplianceRuleExpressionTemplate complianceRuleExpressionTemplate = new Entity.ComplianceRuleExpressionTemplate();

                    complianceRuleExpressionTemplate.EX_ID = rExp.Expression.EX_ID;
                    complianceRuleExpressionTemplate.EX_Name = rExp.Expression.EX_Name;
                    complianceRuleExpressionTemplate.EX_Description = rExp.Expression.EX_Description;
                    complianceRuleExpressionTemplate.EX_Expression = rExp.Expression.EX_Expression;
                    complianceRuleExpressionTemplate.EX_IsActive = rExp.Expression.EX_IsActive;
                    complianceRuleExpressionTemplate.EX_IsDeleted = rExp.Expression.EX_IsDeleted;
                    complianceRuleExpressionTemplate.EX_CreatedByID = rExp.Expression.EX_CreatedByID;
                    complianceRuleExpressionTemplate.EX_CreatedOn = rExp.Expression.EX_CreatedOn;
                    complianceRuleExpressionTemplate.EX_ModifiedByID = rExp.Expression.EX_ModifiedByID;
                    complianceRuleExpressionTemplate.EX_ModifiedOn = rExp.Expression.EX_ModifiedOn;
                    complianceRuleExpressionTemplate.EX_Code = rExp.Expression.EX_Code;
                    complianceRuleExpressionTemplate.ExpressionOrder = rExp.RLE_ExpressionOrder;

                    // Set Expression Elements
                    String[] expElements = complianceRuleExpressionTemplate.EX_Expression.Split(new String[] { delimiterKey }, StringSplitOptions.None);

                    //String firstObject = expElements[0];

                    //String thirdObject = String.Empty;

                    //if (expElements.Length != 1)
                    //{
                    //    thirdObject = expElements[expElements.Length - 1];
                    //}

                    //Int32 i = 1;
                    //String finalExpression = String.Empty;
                    //while (i < expElements.Length - 1)
                    //{
                    //    finalExpression += expElements[i] + " ";
                    //    i++;
                    //}

                    //expElements = new String[] { firstObject.Trim(), finalExpression.Trim(), thirdObject.Trim() };

                    //if (firstObject.Trim().StartsWith("E", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    expElements = complianceRuleExpressionTemplate.EX_Expression.Split(' ');
                    //}

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
        /// GetExpressionOperators
        /// </summary>
        /// <returns></returns>
        public static List<lkpExpressionOperator> GetExpressionOperators(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).GetExpressionOperators().ToList();
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
        public static List<lkpExpressionOperator> GetExpressionOperators(int noOfObjects, Int32 tenantId)
        {
            try
            {
                //List<lkpExpressionOperator> lkpOperators = null;
                List<lkpExpressionOperator> lkpOperators = LookupManager.GetLookUpData<lkpExpressionOperator>(tenantId).ToList();
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
        /// UpdateRuleTemplate
        /// </summary>
        public static void UpdateRuleTemplate(Entity.ComplianceRuleTemplate complianceRuleTemplate, List<Int32> expressionIds, Int32 tenantId)
        {
            try
            {
                BALUtils.GetRuleRepoInstance(tenantId).UpdateRuleTemplate(complianceRuleTemplate, expressionIds);

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
        public static void AddRuleTemplate(Entity.ComplianceRuleTemplate complianceRuleTemplate, Int32 tenantId)
        {
            try
            {

                BALUtils.GetRuleRepoInstance(tenantId).AddRuleTemplate(complianceRuleTemplate);

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
        public static void DeleteRuleTemplate(Int32 ruleId, Int32 currentUserId, Int32 tenantId)
        {
            BALUtils.GetRuleRepoInstance(tenantId).DeleteRuleTemplate(ruleId, currentUserId);
        }

        /// <summary>
        /// ValidateRuleTemplate
        /// </summary>
        /// <param name="ruleExpressionTemplates"></param>
        /// <param name="resultTypeCode"></param>
        /// <returns></returns>
        public static RuleProcessingResult ValidateRuleTemplate(List<Entity.ComplianceRuleExpressionTemplate> ruleExpressionTemplates, String resultTypeCode, Int32 tenantId)
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
                    //exp.AppendChild(doc.CreateElement("Definition")).InnerText = SysXUtils.GetXmlDecodedString(definition.Trim());
                    exp.AppendChild(doc.CreateElement("Definition")).InnerText = "<![CDATA[" + definition + "]]>";
                }

                /* Implement Encoding 
                //Create an XML declaration. 
                XmlDeclaration xmldecl;
                xmldecl = doc.CreateXmlDeclaration("1.0", "utf-8", "yes");

                //Add the new node to the document.
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmldecl, root);
                */

                xmlResult = BALUtils.GetRuleRepoInstance(tenantId).ValidateRuleTemplate(doc.OuterXml.ToString().Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&"));
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

        #region Validate Rule

        public static RuleProcessingResult ValidateExpression(String ruleTemplateXml, String ruleExpressionXml, Int32 tenantId)
        {
            try
            {
                //String ruleTemplateXml = "<Rule><ResultType>BOOL</ResultType><Expressions><Expression><Name>E1</Name><Definition>[Object1] <![CDATA[<=]]> [Object2] </Definition></Expression><Expression><Name>E2</Name><Definition>[Object3] <![CDATA[>=]]> [Object4] </Definition></Expression><Expression><Name>GE</Name><Definition>E1 <![CDATA[&&]]> [Object4] </Definition></Expression></Expressions></Rule>";
                //String ruleExpressionXml = "<ObjectMapping><Mappings><Mapping><Key>[Object1]</Key><DataType>Numeric</DataType></Mapping><Mapping><Key>[Object2]</Key><DataType>Numeric</DataType></Mapping><Mapping><Key>[Object3]</Key><DataType>Numeric</DataType></Mapping><Mapping><Key>[Object4]</Key><DataType>Boolean</DataType></Mapping></Mappings></ObjectMapping>";
                String outPutResult = BALUtils.GetRuleRepoInstance(tenantId).ValidateExpression(ruleTemplateXml, ruleExpressionXml);
                RuleProcessingResult processingResult = ParseValidationXml(outPutResult);
                return processingResult;
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

        public static RuleProcessingResult ParseValidationXml(String outPutResult)
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

        public static List<ComplianceAttribute> getAttributeDetail(List<Int32> objectIds, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).getAttributeDetail(objectIds);
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

        public static String RuleObjectMappingTypeCodebyId(Int32 rmtID, Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpRuleObjectMappingType>(tenantId).FirstOrDefault(x => x.RMT_ID == rmtID && !x.RMT_IsDeleted).RMT_Code;
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

        #endregion

        #region Rule Mapping

        public static Boolean AddRuleMapping(RuleMapping ruleMapping, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).AddRuleMapping(ruleMapping);
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

        public static RuleMapping GetRuleMapping(Int32 rlm_Id, Int32 tenantId, Boolean getRuleMappingDetails = false)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).GetRuleMapping(rlm_Id, getRuleMappingDetails);
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

        public static List<RuleTemplate> GetRuleTemplates(Int32 tenantId, Boolean? IsSeriesDataRqd = null)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).GetRuleTemplates(IsSeriesDataRqd);
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

        public static RuleTemplate GetRuleTemplateDetails(Int32 ruleTemplateId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).GetRuleTemplateDetails(ruleTemplateId);
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

        //public static List<RuleMapping> GetRuleMappings(Int32 ruleSetId, Int32 tenantId)
        public static List<RuleMappingContract> GetRuleMappings(Int32 ruleSetId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).GetRuleMappings(ruleSetId);
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

        public static void DeleteRuleMapping(Int32 ruleMappingId, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                if (tenantId == SecurityManager.DefaultTenantID)
                    BALUtils.GetRuleRepoInstance(tenantId).DeleteRuleMapping(ruleMappingId, currentUserId);
                else
                {
                    Boolean isScheduleActionRecordInserted = BALUtils.GetRuleRepoInstance(tenantId).DeleteRuleMappingAndRefireRules(ruleMappingId, currentUserId);
                    if (isScheduleActionRecordInserted)
                    {
                        ComplianceSetupManager.InsertSystemSeriveTrigger(currentUserId, tenantId, LkpSystemService.EXECUTE_MULTI_RULES_SERVICE);
                    }
                }
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

        public static List<lkpRuleObjectMappingType> GetRuleObjectMappingType(Int32 tenantId, Boolean isSeriesDataRqd)
        {
            try
            {
                if (isSeriesDataRqd)
                    return LookupManager.GetLookUpData<Entity.ClientEntity.lkpRuleObjectMappingType>(tenantId).Where(rmt => !rmt.RMT_IsDeleted).ToList();
                else
                {
                    String seriesItemStatus = ObjectMappingType.Series_Item_Status.GetStringValue();
                    String seriesItemAttribute = ObjectMappingType.Series_Item_Attribute.GetStringValue();
                    return LookupManager.GetLookUpData<Entity.ClientEntity.lkpRuleObjectMappingType>(tenantId).Where(rmt => !rmt.RMT_IsDeleted &&
                                                                                                                            rmt.RMT_Code != seriesItemStatus && rmt.RMT_Code != seriesItemAttribute).ToList();

                }
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

        public static List<lkpObjectType> GetObjectTypes(String ruleObjectMappingTypeCode, Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpRuleObjectMappingTypeObjectType>(tenantId).Where(rmtot => rmtot.lkpRuleObjectMappingType.RMT_Code == ruleObjectMappingTypeCode && !rmtot.RMTO_IsDeleted).
                Select(rmtot => rmtot.lkpObjectType).ToList();
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


        public static List<RuleSetTree> GetRuleSetTreeData(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).GetRuleSetTreeData();
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

        public static lkpObjectType GetObjectType(String ot_Code, Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantId).FirstOrDefault(x => x.OT_Code == ot_Code && !x.OT_IsDeleted);
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

        public static List<lkpObjectType> GetObjectTypeList(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantId);
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

        public static lkpRuleObjectMappingType GetRuleObjectMappingType(String rmt_Code, Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpRuleObjectMappingType>(tenantId).FirstOrDefault(x => x.RMT_Code == rmt_Code && !x.RMT_IsDeleted);
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

        public static RuleMappingObjectTree GetRuleMapingObjectTree(Int32 ruleMappingDetailId, Int32 ruleSetTreeID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).GetRuleMapingObjectTree(ruleMappingDetailId, ruleSetTreeID);
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


        public static lkpObjectType GetObjectTypeById(Int32 ot_ID, Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantId).FirstOrDefault(x => x.OT_ID == ot_ID && !x.OT_IsDeleted);
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

        public static Boolean UpdateRuleMapping(RuleMapping ruleMapping, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).UpdateRuleMapping(ruleMapping);
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

        public static Boolean IsRuleAlreadyInUse(Int32 ruleMappingId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).IsRuleAlreadyInUse(ruleMappingId);
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

        public static IQueryable<lkpConstantType> getConstantType(Int32 tenantId, Boolean isSeriesDataRqd)
        {
            try
            {
                String BothUseTypeCode = "AAAC";
                Int32 BothUseTypeID = LookupManager.GetSharedLookUpIDbyCode<Entity.SharedDataEntity.lkpUseType>(con => con.UT_Code.Trim().Contains(BothUseTypeCode));
                if (isSeriesDataRqd)
                {
                    String itemStatusConstantType = ConstantType.ItemComplianceStatus.GetStringValue();
                    String itemSubmissionDateConstanttype = ConstantType.ItemSubmissionDate.GetStringValue();
                    return LookupManager.GetLookUpData<Entity.ClientEntity.lkpConstantType>(tenantId).Where(obj => obj.IsDeleted == false
                                                                                                                   && obj.Code != itemStatusConstantType && obj.Code != itemSubmissionDateConstanttype
                                                                                                                   && obj.UseType == BothUseTypeID).AsQueryable();
                }
                else
                {
                    return LookupManager.GetLookUpData<Entity.ClientEntity.lkpConstantType>(tenantId).Where(obj => obj.IsDeleted == false
                                                                                                            && obj.UseType == BothUseTypeID).AsQueryable();
                }
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

        public static lkpConstantType getConstantTypeByCode(String code, Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpConstantType>(tenantId).Where(obj => obj.Code == code
                                                    && !obj.IsDeleted).FirstOrDefault();
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

        public static lkpConstantType getConstantTypeById(Int32 constantTypeId, Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpConstantType>(tenantId).Where(obj => obj.ID == constantTypeId
                                                    && !obj.IsDeleted).FirstOrDefault();
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

        public static void deActivatePreviousRules(Int32? prevVesrsionId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetRuleRepoInstance(tenantId).deActivatePreviousRules(prevVesrsionId);
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

        public static Boolean DeactivatePreviousRulesAndCreateNewRule(String parameters, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).DeactivatePreviousRulesAndCreateNewRule(parameters);
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

        public static void InsertSystemSeriveTrigger(Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                Entity.lkpSystemService reOccurRuleService = SecurityManager.GetSystemServiceByCode(SystemService.REOCCUR_RULES_SERVICE.GetStringValue());
                Entity.SystemServiceTrigger systemServiceTrigger = new Entity.SystemServiceTrigger();
                if (reOccurRuleService != null)
                    systemServiceTrigger.SST_SystemServiceID = reOccurRuleService.SS_ID;
                systemServiceTrigger.SST_TenantID = tenantId;
                systemServiceTrigger.SST_IsActive = true;
                systemServiceTrigger.SST_CreatedByID = currentUserId;
                systemServiceTrigger.SST_CreatedOn = DateTime.Now;
                SecurityManager.AddSystemServiceTrigger(systemServiceTrigger);
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

        public static List<lkpRuleImpactGroup> getImpactedUserGroupType(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpRuleImpactGroup>(tenantId).Where(cond => cond.IsDeleted == false).ToList();
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

        public static List<RuleImpactGroupMapping> getPreviousImpactedGroupMappings(Int32 ruleMappingId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).getPreviousImpactedGroupMappings(ruleMappingId);
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

        public static void updateRuleImpactedGroupMappings(List<RuleImpactGroupMapping> impactedGroupMappings, Int32 ruleMappingId, Int32 loggedInUserId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetRuleRepoInstance(tenantId).updateRuleImpactedGroupMappings(impactedGroupMappings, ruleMappingId, loggedInUserId);
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


        public static List<CompliancePackage> GetListOfInstanceWichCanShareRule(Int32 ruleSetId, Int32 tenantId, String HID)
        {
            try
            {
                DataTable table = BALUtils.GetRuleRepoInstance(tenantId).GetListOfInstanceWichCanShareRule(ruleSetId, HID);

                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new CompliancePackage
                {
                    CompliancePackageID = x["CompliancePackageID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["CompliancePackageID"]),
                    PackageName = x["PackageName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["PackageName"])
                }).ToList();
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

        public static RuleSynchronisationData GetListOfInstanceWichCanShareRuleOnEdit(Int32 ruleSetId, Int32 ruleId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).GetListOfInstanceWichCanShareRuleOnEdit(ruleSetId, ruleId);
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

        public static String ComplianceRuleSynchronisation(List<Int32> packageList, Int32 sourceRuleId, Int32 currentUserId, String settingParameters, Int32 tenantId, Boolean sourceRuleHasSubscription, out Boolean isScheduleActionRecordInserted)
        {
            String errorMsg = String.Empty;
            try
            {
                isScheduleActionRecordInserted = BALUtils.GetRuleRepoInstance(tenantId).ComplianceRuleSynchronisation(packageList, sourceRuleId, currentUserId, settingParameters, sourceRuleHasSubscription);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                errorMsg = "Rule was updated, however, changes could not be propagated to other packages. Please try again.";
                isScheduleActionRecordInserted = false;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
            return errorMsg;
        }

        public static String ComplianceRuleSynchronisationonRuleEdit(Int32 tenantId, List<Int32> packageList, Int32 sourceRuleId, Int32 currentUserId, String settingParameters, Boolean isVersionUpdate, Boolean? updateAllSelected, Int32? sourceRuleVersionId, out Boolean isScheduleActionRecordInserted)
        {
            String errorMsg = String.Empty;
            try
            {
                isScheduleActionRecordInserted = BALUtils.GetRuleRepoInstance(tenantId).ComplianceRuleSynchronisationonRuleEdit(packageList, sourceRuleId, currentUserId, settingParameters, isVersionUpdate, sourceRuleVersionId, updateAllSelected);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                errorMsg = "Rule was updated, however, changes could not be propagated to other packages. Please try again.";
                isScheduleActionRecordInserted = false;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
            return errorMsg;
        }

        #endregion

        #region Evaluate Post Submit Rules

        /// <summary>
        /// For evaluating post submit rules
        /// </summary>
        /// <param name="ruleObjectMapping"> object of rule mapping</param>
        /// <param name="applicantId"> applicant Id</param>
        /// <param name="systemUserId">current loggedIn user Id</param>
        /// <param name="tenantId"> Tenant Id</param>
        public static void evaluatePostSubmitRules(List<RuleObjectMapping> ruleObjectMapping, Int32 applicantId, Int32 systemUserId, Int32 tenantId)
        {
            try
            {
                Boolean executeBuisnessRule = Convert.ToBoolean(WebConfigurationManager.AppSettings["ExecuteBuisnessRule"]);
                if (!executeBuisnessRule || executeBuisnessRule.IsNullOrEmpty())
                {
                    return;
                }
                else
                {
                    String generateRuleObjectXml = genarateRuleObjectXml(ruleObjectMapping);
                    BALUtils.GetRuleRepoInstance(tenantId).evaluatePostSubmitRules(applicantId, generateRuleObjectXml, systemUserId);
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                // throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method for generating object mapping Xml
        /// </summary>
        /// <param name="ruleObjectMappingList">object list of RuleObjectMapping </param>
        /// <returns></returns>
        public static String genarateRuleObjectXml(List<RuleObjectMapping> ruleObjectMappingList)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("RuleObjects"));
            foreach (RuleObjectMapping ruleObjectMapping in ruleObjectMappingList)
            {
                XmlNode exp = el.AppendChild(doc.CreateElement("RuleObject"));
                exp.AppendChild(doc.CreateElement("TypeId")).InnerText = ruleObjectMapping.RuleObjectTypeId;
                exp.AppendChild(doc.CreateElement("Id")).InnerText = ruleObjectMapping.RuleObjectId;
                exp.AppendChild(doc.CreateElement("ParentId")).InnerText = ruleObjectMapping.RuleObjectParentId;
            }

            return doc.OuterXml.ToString();
        }

        /// <summary>
        /// method for evaluating buisness rule on item expiary.
        /// </summary>
        /// <param name="applicantId">Applicant Id</param>
        /// <param name="systemUserId">LoggedIn User Id</param>
        /// <param name="tenantId">tenant id</param>
        public static Boolean ProcessItemExpiry(Int32 tenantId, String applicationUrl, String entitySetName, Int32 subEventId, String tenantName, Int32 chunkSize)//public static List<GetExpiredItemDataList> ProcessItemExpiry(Int32 tenantId)
        {
            try
            {
                DateTime todayDate = DateTime.Now;
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 systemUserId = AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                if (appConfiguration.IsNotNull())
                {
                    systemUserId = Convert.ToInt32(appConfiguration.AC_Value);
                }
                Int32 previousPckgSubsId = 0;
                List<ExpiredItemDataList> expiredItemList = new List<ExpiredItemDataList>();
                expiredItemList = BALUtils.GetRuleRepoInstance(tenantId).getExpiringItemData(chunkSize, systemUserId);
                if (expiredItemList.Count > 0)
                {
                    String code = QueueMetaDataType.Verification_Queue_For_Admin.GetStringValue();
                    QueueMetaData queueMetaData = QueueManagementManager.GetQueueMetaDataByCode(tenantId, code);
                    Int32 businessProcessID = (queueMetaData.IsNotNull() && queueMetaData.QMD_BusinessProcessID.HasValue) ? queueMetaData.QMD_BusinessProcessID.Value : 0;
                    foreach (ExpiredItemDataList expiredItem in expiredItemList)
                    {
                        if (expiredItem != null)
                        {
                            try
                            {

                                Int32 itemId = expiredItem.ComplianceItemID;
                                Int32 categoryId = expiredItem.ComplianceCategoryId;
                                Int32 packageId = expiredItem.CompliancePackageId;
                                Int32 applicantId = expiredItem.OrgUserId;
                                String complianceStatus = expiredItem.ComplianceStatus;
                                Int32 complianceStatusId = expiredItem.ComplianceStatusId;
                                Int32 packageSubscriptionId = expiredItem.PackageSubscriptionID;
                                List<RuleObjectMapping> ruleObjectMappingList = getRuleObjectMapping(tenantId, itemId, categoryId, packageId);
                                RuleManager.evaluatePostSubmitRules(ruleObjectMappingList, applicantId, systemUserId, tenantId);
                                //BALUtils.GetRuleRepoInstance(tenantId).UpdateItemDataStatusToExpire(expiredItem.ComplianceItemId);
                                if (packageSubscriptionId != previousPckgSubsId)
                                {
                                    //Send Mail
                                    ComplianceDataManager.SendMailOnComplianceStatusChange(tenantId, tenantName, complianceStatus, complianceStatusId, packageSubscriptionId, expiredItem.HierarchyNodeID);
                                }

                                ////Send Notification On Item Status Changed To Review Status[UAT-1597]
                                //ComplianceDataManager.SendNotificationOnItemStatusChangedToReviewStatus(false, tenantId, packageSubscriptionId,
                                //                                                                        packageId, categoryId,itemId,systemUserId, applicantId,
                                //                                                                        ApplicantItemComplianceStatus.Expired.GetStringValue());

                                previousPckgSubsId = packageSubscriptionId;
                                PackageSubscription pkgSub = ComplianceDataManager.GetPackageSubscriptionByID(tenantId, packageSubscriptionId);

                                //Update the new status
                                expiredItem.Code = pkgSub.ApplicantComplianceCategoryDatas.Where(con => con.ComplianceCategoryID == expiredItem.ComplianceCategoryId && !con.IsDeleted)
                                            .Select(sel => sel.lkpCategoryComplianceStatu.Code).FirstOrDefault();
                                
                                if ((expiredItem.Code != ApplicantCategoryComplianceStatus.Approved.GetStringValue()
                                    && expiredItem.Code != ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue()) ||
                                  ((expiredItem.Code == ApplicantCategoryComplianceStatus.Approved.GetStringValue()
                                  || expiredItem.Code == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue())
                                  && (expiredItem.ExpirationDate.HasValue && expiredItem.CategoryComplianceExpiryDate.HasValue ?
                                  expiredItem.ExpirationDate.Value.Date == expiredItem.CategoryComplianceExpiryDate.Value.Date :
                                  false)
                                  )
                                  )
                                {
                                    //UAT-3654
                                    PackageSubscription ps = ComplianceDataManager.GetPackageSubscriptionByID(tenantId, previousPckgSubsId);
                                    if (!ps.IsNullOrEmpty() && ps.ExpiryDate > DateTime.Now)
                                    {

                                        //Create Dictionary
                                        Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                        dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(expiredItem.UserFirstName, " ", expiredItem.UserLastName));
                                        dictMailData.Add(EmailFieldConstants.COMPLIANCE_ITEM_EXPIRY_DATE, expiredItem.ExpirationDate.HasValue ? expiredItem.ExpirationDate.Value.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy"));
                                        dictMailData.Add(EmailFieldConstants.COMPLIANCE_ITEM_NAME, expiredItem.ItemName);
                                        dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                                        dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, expiredItem.NodeHierarchy);
                                        dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                        //dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, ComplianceSetupManager.GetCurrentPackageInfo(expiredItem.CompliancePackageId, tenantId).PackageName);
                                        CompliancePackage cmpPackage = ComplianceSetupManager.GetCurrentPackageInfo(expiredItem.CompliancePackageId, tenantId);
                                        if (!cmpPackage.IsNullOrEmpty())
                                        {
                                            dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, cmpPackage.PackageLabel.IsNullOrEmpty() ? cmpPackage.PackageName : cmpPackage.PackageLabel);
                                        }
                                        dictMailData.Add(EmailFieldConstants.COMPLIANCE_ITEM_ID, expiredItem.ComplianceItemID.ToString());
                                        dictMailData.Add(EmailFieldConstants.COMPLIANCE_CATEGORY_ID, expiredItem.ComplianceCategoryId.ToString());
                                        dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId.ToString());

                                        Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                        mockData.UserName = string.Concat(expiredItem.UserFirstName, " ", expiredItem.UserLastName);
                                        mockData.EmailID = expiredItem.PrimaryEmailaddress;
                                        mockData.ReceiverOrganizationUserID = expiredItem.OrgUserId;

                                        //Send mail
                                        CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.COMPLIANCE_ITEM_EXPIRED, dictMailData, mockData, tenantId, expiredItem.HierarchyNodeID);

                                        //Send Message
                                        CommunicationManager.SaveMessageContent(CommunicationSubEvents.COMPLIANCE_ITEM_EXPIRED, dictMailData, expiredItem.OrgUserId, tenantId);

                                        //Save Notification Delivery 
                                        Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                        notificationDelivery.ND_OrganizationUserID = expiredItem.OrgUserId;
                                        notificationDelivery.ND_SubEventTypeID = subEventId;
                                        notificationDelivery.ND_EntityId = expiredItem.ApplicantComplianceItemID;
                                        notificationDelivery.ND_EntityName = entitySetName;
                                        notificationDelivery.ND_IsDeleted = false;
                                        notificationDelivery.ND_CreatedOn = DateTime.Now;
                                        notificationDelivery.ND_CreatedBy = systemUserId;
                                        ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);
                                    }
                                }
                                //Reset Business process by businessProcessID and recordID.
                                QueueManagementManager.ResetBusinessProcess(tenantId, businessProcessID, expiredItem.ApplicantComplianceItemID);

                            }
                            catch (SysXException ex)
                            {
                                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace()
                                    + Environment.NewLine + "Tenant Id:" + tenantId
                                    + Environment.NewLine + "Item Data Id:" + expiredItem.ApplicantComplianceItemID
                                    + Environment.NewLine + ex.Message
                                    + Environment.NewLine + ex.StackTrace, ex);
                                ex.Data["TenantId"] = tenantId;
                                ex.Data["ItemDataId"] = expiredItem.ApplicantComplianceItemID;
                                throw ex;
                            }
                            catch (Exception ex)
                            {
                                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace()
                                    + Environment.NewLine + "Tenant Id:" + tenantId
                                    + Environment.NewLine + "Item Data Id:" + expiredItem.ComplianceItemID
                                    + Environment.NewLine + ex.Message
                                    + Environment.NewLine + ex.StackTrace, ex);
                                ex.Data["TenantId"] = tenantId;
                                ex.Data["ItemDataId"] = expiredItem.ApplicantComplianceItemID;
                                throw (new SysXException(ex.Message, ex));
                            }
                        }
                    }
                    return true;
                }
                return false;
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
            //return new List<GetExpiredItemDataList>();
        }

        /// <summary>
        /// get the object mapping for firing rule.
        /// </summary>
        /// <param name="tenantId">tenant Id</param>
        /// <param name="itemId">Item Id</param>
        /// <param name="categoryId">Category Id</param>
        /// <param name="packageId">Package Id</param>
        /// <returns></returns>
        private static List<RuleObjectMapping> getRuleObjectMapping(Int32 tenantId, Int32 itemId, Int32 categoryId, Int32 packageId, Int32 attributeId = 0)
        {
            List<RuleObjectMapping> ruleObjectMappingList = new List<RuleObjectMapping>();
            RuleObjectMapping ruleObjectMappingForPackage = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Package.GetStringValue(), tenantId).OT_ID),
                RuleObjectId = Convert.ToString(packageId),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RuleObjectMapping ruleObjectMappingForCategory = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Category.GetStringValue(), tenantId).OT_ID),
                RuleObjectId = Convert.ToString(categoryId),
                RuleObjectParentId = Convert.ToString(packageId)
            };
            RuleObjectMapping ruleObjectMappingForItem = null;
            if (itemId != null && itemId != 0)
            {
                ruleObjectMappingForItem = new RuleObjectMapping();
                ruleObjectMappingForItem.RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Item.GetStringValue(), tenantId).OT_ID);
                ruleObjectMappingForItem.RuleObjectId = Convert.ToString(itemId);
                ruleObjectMappingForItem.RuleObjectParentId = Convert.ToString(categoryId);
            }
            RuleObjectMapping ruleObjectMappingForAttribute = null;
            if (attributeId != null && attributeId != 0)
            {
                ruleObjectMappingForAttribute = new RuleObjectMapping();
                ruleObjectMappingForAttribute.RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_ATR.GetStringValue(), tenantId).OT_ID);
                ruleObjectMappingForAttribute.RuleObjectId = Convert.ToString(attributeId);
                ruleObjectMappingForAttribute.RuleObjectParentId = Convert.ToString(itemId);
            }
            ruleObjectMappingList.Add(ruleObjectMappingForPackage);
            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
            if (ruleObjectMappingForItem != null)
            {
                ruleObjectMappingList.Add(ruleObjectMappingForItem);
            }
            if (ruleObjectMappingForAttribute != null)
            {
                ruleObjectMappingList.Add(ruleObjectMappingForAttribute);
            }
            return ruleObjectMappingList;
        }

        /// <summary>
        /// Get Expiring Compliance Items
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="beforeExpiry"></param>
        /// <param name="expiryFrequency"></param>
        /// <param name="entitySetName"></param>
        /// <param name="today"></param>
        public static List<GetExpiredItemDataList> GetExpiringComplianceItems(Int32 tenantId, String subEventCode, Int32 subEventId, String entitySetName, Int32 chunkSize)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).GetExpiringComplianceItems(tenantId, subEventCode, subEventId, entitySetName, chunkSize);
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
        /// method for evaluating buisness rule on item whose next action is schedule to Execute Category Rules.
        /// </summary>
        /// <param name="applicantId">Applicant Id</param>
        /// <param name="systemUserId">LoggedIn User Id</param>
        /// <param name="tenantId">tenant id</param>
        ///<returns>Bool whether some records were processed or not</returns>
        public static Boolean ProcessActionExecuteCategoryRules(Int32 tenantId, String tenantName, Int32 chunkSize)
        {
            try
            {
                DateTime todayDate = DateTime.Now;
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 systemUserId = AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                if (appConfiguration.IsNotNull())
                {
                    systemUserId = Convert.ToInt32(appConfiguration.AC_Value);
                }
                List<GetActionScheduleExecuteCategoryRulesList> scheduleExecuteCategoryRulesList = new List<GetActionScheduleExecuteCategoryRulesList>();
                scheduleExecuteCategoryRulesList = BALUtils.GetRuleRepoInstance(tenantId).getActionActionExecuteCategoryRules(chunkSize);
                if (scheduleExecuteCategoryRulesList.Count > 0)
                {
                    foreach (GetActionScheduleExecuteCategoryRulesList scheduledcategory in scheduleExecuteCategoryRulesList)
                    {
                        if (scheduledcategory != null)
                        {
                            try
                            {
                                Int32 attributeId = scheduledcategory.ComplianceAttributeId.HasValue ? scheduledcategory.ComplianceAttributeId.Value : AppConsts.NONE;
                                Int32 itemId = scheduledcategory.ComplianceItemId.HasValue ? scheduledcategory.ComplianceItemId.Value : AppConsts.NONE;
                                Int32 categoryId = scheduledcategory.ComplianceCategoryId;
                                Int32 packageId = scheduledcategory.CompliancePackageId;
                                Int32 applicantId = scheduledcategory.OrgUserId.Value;
                                String complianceStatus = scheduledcategory.ComplianceStatus;
                                Int32 complianceStatusId = scheduledcategory.ComplianceStatusId.Value;
                                Int32 packageSubscriptionId = scheduledcategory.PackageSubscriptionId;

                                /*
                                //[SG]: UAT-1031 - (Remove System notifications: Payment Rejected and Rule changed.)
                                if (scheduledcategory.ScheduleActionTypeCode == ScheduleActionType.Execute_Category_Rules_After_Rule_Edit.GetStringValue())
                                {
                                    Entity.lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_FOR_RULE_CHANGE.GetStringValue());
                                    Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                                    //Checks added corresponding to UAT - 863 : WB: As a student, I should only receive one "Rules Changed" email per day
                                    NotificationDelivery notificationDelivery = ComplianceDataManager.GetExistingNotificationDeliveryForToday(tenantId, scheduledcategory.OrgUserId.HasValue ? scheduledcategory.OrgUserId.Value : 0, scheduledcategory.PackageSubscriptionId, subEventId, "ComplianceRuleChange");
                                    if (notificationDelivery.IsNullOrEmpty())
                                    {
                                        //SendMailOnComplianceRuleChange(tenantId, tenantName, scheduledcategory.PackageName, scheduledcategory.UserFullName, scheduledcategory.EmailAddress, applicantId, scheduledcategory.HierarchyLabel, scheduledcategory.HierarchyNodeID);
                                        SendMailOnComplianceRuleChange(tenantId, tenantName, scheduledcategory, subEventId, systemUserId);
                                    }
                                }
                                */


                                List<RuleObjectMapping> ruleObjectMappingList = getRuleObjectMapping(tenantId, itemId, categoryId, packageId, attributeId);

                                ////UAT-2725 Not in Use.
                                //Boolean _isOtherCategoryComplianceCheckNeeded = BALUtils.GetRuleRepoInstance(tenantId).IsTriggerForOtherCategoryNeeded(categoryId);
                                //List<Int32> lstCategoryIDs = new List<Int32>();
                                //if (_isOtherCategoryComplianceCheckNeeded)
                                //{
                                //    lstCategoryIDs = BALUtils.GetRuleRepoInstance(tenantId).GetComplianceCategoryIdsByPackageID(packageId);
                                //    foreach (Int32 ID in lstCategoryIDs.Where(value => value != categoryId))
                                //    {
                                //        RuleObjectMapping ruleObjectMappingForCategory = new RuleObjectMapping
                                //        {
                                //            RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Category.GetStringValue(), tenantId).OT_ID),
                                //            RuleObjectId = Convert.ToString(ID),
                                //            RuleObjectParentId = Convert.ToString(packageId)
                                //        };
                                //        ruleObjectMappingList.Add(ruleObjectMappingForCategory);
                                //    }
                                //}

                                //UAT-3805
                                List<Int32> approvedCategoryIDs = ProfileSharingManager.GetApprovedCategorIDs(tenantId, packageSubscriptionId
                                                                                          , new List<Int32>(), lkpUseTypeEnum.COMPLIANCE.GetStringValue());

                                RuleManager.evaluatePostSubmitRules(ruleObjectMappingList, applicantId, systemUserId, tenantId);
                                BALUtils.GetRuleRepoInstance(tenantId).inactiveProcessedScheduleAction(scheduledcategory.ScheduleActionId.Value, systemUserId);
                                ComplianceDataManager.SendMailOnComplianceStatusChange(tenantId, tenantName, complianceStatus, complianceStatusId, packageSubscriptionId, scheduledcategory.HierarchyNodeID);


                                //UAT-3805
                                String approvedCategoryIds = approvedCategoryIDs.IsNullOrEmpty() ? String.Empty : String.Join(",", approvedCategoryIDs);
                                ProfileSharingManager.SendItemDocNotificationOnCategoryApproval(tenantId, Convert.ToString(categoryId), applicantId
                                                                                                , approvedCategoryIds, lkpUseTypeEnum.COMPLIANCE.GetStringValue()
                                                                                                , packageSubscriptionId, null, systemUserId);

                            }
                            catch (SysXException ex)
                            {
                                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace()
                                    + Environment.NewLine + "Tenant Id:" + tenantId
                                    + Environment.NewLine + "Package Subscription Id:" + scheduledcategory.PackageSubscriptionId
                                    + Environment.NewLine + ex.Message
                                    + Environment.NewLine + ex.StackTrace, ex);
                                ex.Data["TenantId"] = tenantId;
                                ex.Data["PackageSubscriptionId"] = scheduledcategory.PackageSubscriptionId;
                                throw ex;
                            }
                            catch (Exception ex)
                            {

                                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace()
                                    + Environment.NewLine + "Tenant Id:" + tenantId
                                    + Environment.NewLine + "Package Subscription Id:" + scheduledcategory.PackageSubscriptionId
                                    + Environment.NewLine + ex.Message
                                    + Environment.NewLine + ex.StackTrace, ex);
                                ex.Data["TenantId"] = tenantId;
                                ex.Data["PackageSubscriptionId"] = scheduledcategory.PackageSubscriptionId;
                                throw (new SysXException(ex.Message, ex));
                            }
                        }
                    }
                    return true;//Returns true if some records were found for procesing.
                }
                return false;//Returns false if no record was processed.
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
        /// for getting count of total record for which category rules scheduled to be reoccur.
        /// </summary>
        /// <returns></returns>
        public static Int32 getCountRecordsScheduleExecutecategoryrule(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).getCountRecordsScheduleExecutecategoryrule();
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
        /// for executung buisness rule for a list of package subscription id's
        /// </summary>
        /// <param name="packageSubscriptionIds"></param>
        /// <param name="tenantId"></param>
        /// <param name="systemUserId"></param>
        public static void ExecuteBusinessRules(List<Int32> packageSubscriptionIds, Int32 tenantId, Int32 systemUserId, List<Int32> lstCatDataAffected = null)
        {
            foreach (Int32 packageSubscriptionId in packageSubscriptionIds)
            {
                PackageSubscription currentSubscription = ComplianceDataManager.GetPackageSubscriptionByID(tenantId, packageSubscriptionId);
                if (currentSubscription != null)
                {
                    List<RuleObjectMapping> ruleObjectMappingList = new List<RuleObjectMapping>();
                    RuleObjectMapping ruleObjectMappingForPackage = new RuleObjectMapping
                    {
                        RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Package.GetStringValue(), tenantId).OT_ID),
                        RuleObjectId = Convert.ToString(currentSubscription.CompliancePackageID),
                        RuleObjectParentId = Convert.ToString(AppConsts.NONE)
                    };
                    ruleObjectMappingList.Add(ruleObjectMappingForPackage);
                    List<ApplicantComplianceCategoryData> lstCategoryData = new List<ApplicantComplianceCategoryData>();
                    if (lstCatDataAffected.IsNullOrEmpty())
                    {
                        lstCategoryData = currentSubscription.ApplicantComplianceCategoryDatas.Where(x => x.IsDeleted == false).ToList();
                    }
                    else
                    {
                        lstCategoryData = currentSubscription.ApplicantComplianceCategoryDatas.Where(x => x.IsDeleted == false && lstCatDataAffected.Contains(x.ApplicantComplianceCategoryID)).ToList();
                    }
                    foreach (ApplicantComplianceCategoryData categoryData in lstCategoryData)
                    {
                        if (categoryData != null)
                        {
                            RuleObjectMapping ruleObjectMappingForCategory = new RuleObjectMapping
                            {
                                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Category.GetStringValue(), tenantId).OT_ID),
                                RuleObjectId = Convert.ToString(categoryData.ComplianceCategoryID),
                                RuleObjectParentId = Convert.ToString(currentSubscription.CompliancePackageID)
                            };
                            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
                            foreach (ApplicantComplianceItemData itemData in categoryData.ApplicantComplianceItemDatas.Where(x => x.IsDeleted == false))
                            {
                                if (itemData != null)
                                {
                                    RuleObjectMapping ruleObjectMappingForItem = new RuleObjectMapping();
                                    ruleObjectMappingForItem.RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Item.GetStringValue(), tenantId).OT_ID);
                                    ruleObjectMappingForItem.RuleObjectId = Convert.ToString(itemData.ComplianceItemID);
                                    ruleObjectMappingForItem.RuleObjectParentId = Convert.ToString(categoryData.ComplianceCategoryID);
                                    ruleObjectMappingList.Add(ruleObjectMappingForItem);
                                }

                                foreach (ApplicantComplianceAttributeData attributeData in itemData.ApplicantComplianceAttributeDatas.Where(x => x.IsDeleted == false))
                                {
                                    if (attributeData != null)
                                    {
                                        RuleObjectMapping ruleObjectMappingForAttribte = new RuleObjectMapping();
                                        ruleObjectMappingForAttribte.RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_ATR.GetStringValue(), tenantId).OT_ID);
                                        ruleObjectMappingForAttribte.RuleObjectId = Convert.ToString(attributeData.ComplianceAttributeID);
                                        ruleObjectMappingForAttribte.RuleObjectParentId = Convert.ToString(itemData.ComplianceItemID);
                                        ruleObjectMappingList.Add(ruleObjectMappingForAttribte);
                                    }
                                }
                            }
                        }
                    }
                    RuleManager.evaluatePostSubmitRules(ruleObjectMappingList, currentSubscription.OrganizationUserID.Value, systemUserId, tenantId);
                }
            }
        }

        /// <summary>
        /// To execute Category level Business Rules
        /// </summary>
        /// <param name="compliancePackageId"></param>
        /// <param name="complianceCategoryId"></param>
        /// <param name="systemUserId"></param>
        /// <param name="applicantId"></param>
        /// <param name="tenantId"></param>
        public static void ExecuteCategoryLevelBusinessRules(Int32 compliancePackageId, Int32 complianceCategoryId, Int32 systemUserId, Int32 applicantId, Int32 tenantId)
        {
            List<RuleObjectMapping> ruleObjectMappingList = new List<RuleObjectMapping>();

            RuleObjectMapping ruleObjectMappingForPackage = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Package.GetStringValue(), tenantId).OT_ID),
                RuleObjectId = Convert.ToString(compliancePackageId),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RuleObjectMapping ruleObjectMappingForCategory = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Category.GetStringValue(), tenantId).OT_ID),
                RuleObjectId = Convert.ToString(complianceCategoryId),
                RuleObjectParentId = Convert.ToString(compliancePackageId)
            };

            ruleObjectMappingList.Add(ruleObjectMappingForPackage);
            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
            RuleManager.evaluatePostSubmitRules(ruleObjectMappingList, applicantId, systemUserId, tenantId);
        }

        /// <summary>
        /// Send mail to all applicant for upcomming Non Compliance Categories
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="tenantName">tenantName</param>
        /// <param name="chunkSize">chunkSize</param>
        /// <returns></returns>
        public static Boolean ProcessNonComplianceCategories(Int32 tenantId, String tenantName, Int32 chunkSize)
        {
            try
            {
                DateTime todayDate = DateTime.Now;

                Entity.lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_NON_COMPLIANCE_SCHEDULE_CATEGORY.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                String entitySetName = "ApplicantComplianceCategoryData";

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                var categoryRecordObject = CommunicationManager.GetlkpRecordObjectType().Where(cond => cond.OT_Code == LCObjectType.ComplianceCategory.GetStringValue()).FirstOrDefault();
                Int32 categoryObjectTypeID = 0;

                if (categoryRecordObject.IsNotNull())
                {
                    categoryObjectTypeID = categoryRecordObject.OT_ID;
                }

                List<GetUpcomingNonComplianceCategories> upcomingNonComplianceCategoryList = new List<GetUpcomingNonComplianceCategories>();
                upcomingNonComplianceCategoryList = BALUtils.GetRuleRepoInstance(tenantId).GetUpcomingNonComplianceCategory(tenantId, chunkSize);
                if (upcomingNonComplianceCategoryList != null && upcomingNonComplianceCategoryList.Count > 0)
                {
                    foreach (GetUpcomingNonComplianceCategories nonComplianceCategory in upcomingNonComplianceCategoryList)
                    {
                        if (nonComplianceCategory != null)
                        {
                            try
                            {
                                //Create Dictionary
                                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, nonComplianceCategory.UserFullName);
                                dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, nonComplianceCategory.PackageName);
                                dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, applicationUrl);
                                dictMailData.Add(EmailFieldConstants.CATEGORY_NAME, nonComplianceCategory.CategoryName);
                                dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, nonComplianceCategory.NodeHierarchy);
                                dictMailData.Add(EmailFieldConstants.DUE_DATE, nonComplianceCategory.DueDate.ToString("MM-dd-yyyy"));

                                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                mockData.UserName = nonComplianceCategory.UserFullName;
                                mockData.EmailID = nonComplianceCategory.PrimaryEmailAddress;
                                mockData.ReceiverOrganizationUserID = nonComplianceCategory.ApplicantID;

                                //Send mail
                                //[UAT-1072]
                                //CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_NON_COMPLIANCE_SCHEDULE_CATEGORY, dictMailData, mockData, tenantId, nonComplianceCategory.HierarchyNodeID);
                                CommunicationManager.SendPackageNotificationMailForCCUserSettings(CommunicationSubEvents.NOTIFICATION_NON_COMPLIANCE_SCHEDULE_CATEGORY, dictMailData, mockData, tenantId, nonComplianceCategory.HierarchyNodeID, categoryObjectTypeID, nonComplianceCategory.ComplianceCategoryID);

                                //Send Message
                                //[UAT-1072]
                                //CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_NON_COMPLIANCE_SCHEDULE_CATEGORY, dictMailData, nonComplianceCategory.ApplicantID, tenantId);
                                CommunicationManager.SaveMessageContentForCCUserSettings(CommunicationSubEvents.NOTIFICATION_NON_COMPLIANCE_SCHEDULE_CATEGORY, dictMailData, nonComplianceCategory.ApplicantID, tenantId, categoryObjectTypeID, nonComplianceCategory.ApplicantComplianceCategoryID);

                                //Save Notification Delivery
                                Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                notificationDelivery.ND_OrganizationUserID = nonComplianceCategory.ApplicantID;
                                notificationDelivery.ND_SubEventTypeID = subEventId;
                                notificationDelivery.ND_EntityId = nonComplianceCategory.ApplicantComplianceCategoryID;
                                notificationDelivery.ND_EntityName = entitySetName;
                                notificationDelivery.ND_IsDeleted = false;
                                notificationDelivery.ND_CreatedOn = DateTime.Now;
                                notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                            }
                            catch (SysXException ex)
                            {
                                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace()
                                    + Environment.NewLine + "Tenant Id:" + tenantId
                                    + Environment.NewLine + "Applicant Compliance CategoryID:" + nonComplianceCategory.ApplicantComplianceCategoryID
                                    + Environment.NewLine + ex.Message
                                    + Environment.NewLine + ex.StackTrace, ex);
                                ex.Data["TenantId"] = tenantId;
                                ex.Data["ApplicantComplianceCategoryID"] = nonComplianceCategory.ApplicantComplianceCategoryID;
                                throw ex;
                            }
                            catch (Exception ex)
                            {

                                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace()
                                    + Environment.NewLine + "Tenant Id:" + tenantId
                                    + Environment.NewLine + "Applicant Compliance CategoryID:" + nonComplianceCategory.ApplicantComplianceCategoryID
                                    + Environment.NewLine + ex.Message
                                    + Environment.NewLine + ex.StackTrace, ex);
                                ex.Data["TenantId"] = tenantId;
                                ex.Data["ApplicantComplianceCategoryID"] = nonComplianceCategory.ApplicantComplianceCategoryID;
                                throw (new SysXException(ex.Message, ex));
                            }
                        }
                    }
                    return true;//Returns true if some records were found for procesing.
                }
                return false;//Returns false if no record was processed.
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

        //public static void SendMailOnComplianceRuleChange(Int32 tenantId, String tenantName, String packageName, String userName, String primaryEmailAddress, Int32 organizationUserID, String hierarchyLabel, Int32 hierarchyNodeID)
        public static void SendMailOnComplianceRuleChange(Int32 tenantId, String tenantName, GetActionScheduleExecuteCategoryRulesList scheduledcategory, Int32 subEventId, Int32 systemUserId)
        {
            try
            {
                String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);
                //Create Dictionary
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, scheduledcategory.UserFullName);
                dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, scheduledcategory.PackageName);
                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, scheduledcategory.HierarchyLabel);
                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                mockData.UserName = scheduledcategory.UserFullName;
                mockData.EmailID = scheduledcategory.EmailAddress;
                mockData.ReceiverOrganizationUserID = scheduledcategory.OrgUserId.Value;

                //Send mail
                Int32? systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_RULE_CHANGE, dictMailData, mockData, tenantId, scheduledcategory.HierarchyNodeID);

                //Send Message
                CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_RULE_CHANGE, dictMailData, scheduledcategory.OrgUserId.Value, tenantId);

                if (systemCommunicationID.IsNotNull() && systemCommunicationID > 0)
                {
                    Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                    notificationDelivery.ND_OrganizationUserID = scheduledcategory.OrgUserId.Value;
                    notificationDelivery.ND_SubEventTypeID = subEventId;
                    notificationDelivery.ND_EntityId = scheduledcategory.PackageSubscriptionId;
                    notificationDelivery.ND_EntityName = "ComplianceRuleChange";
                    notificationDelivery.ND_IsDeleted = false;
                    notificationDelivery.ND_CreatedOn = DateTime.Now;
                    notificationDelivery.ND_CreatedBy = systemUserId;
                    ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);
                }
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

        /// <summary>
        /// Method for evaluating buisness rule on category expiry.
        /// </summary>
        /// <param name="tenantId">tenant id</param>
        /// <param name="applicationUrl">applicationUrl</param>
        /// <param name="chunkSize">chunkSize</param>
        /// <param name="tenantName">tenantName</param>
        public static Boolean ProcessCategoryExpiry(Int32 tenantId, String applicationUrl, String tenantName, Int32 chunkSize)
        {
            try
            {
                Guid whlCatGuid = new Guid(AppConsts.WHOLE_CATEGORY_GUID);
                DateTime todayDate = DateTime.Now;
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 systemUserId = AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                if (appConfiguration.IsNotNull())
                {
                    systemUserId = Convert.ToInt32(appConfiguration.AC_Value);
                }
                Int32 previousPckgSubsId = 0, prevCatID = 0, prevApplicantComplianceCategoryID = 0;
                List<ExpiredCategoryDataListContract> expiredCategoryList = new List<ExpiredCategoryDataListContract>();
                expiredCategoryList = AssignExpiredCategoryToDataModel(BALUtils.GetRuleRepoInstance(tenantId).GetExpiringCategoryData(chunkSize, systemUserId));
                if (expiredCategoryList.Count > 0)
                {
                    String code = QueueMetaDataType.Verification_Queue_For_Admin.GetStringValue();
                    QueueMetaData queueMetaData = QueueManagementManager.GetQueueMetaDataByCode(tenantId, code);
                    Int32 businessProcessID = (queueMetaData.IsNotNull() && queueMetaData.QMD_BusinessProcessID.HasValue) ? queueMetaData.QMD_BusinessProcessID.Value : 0;
                    foreach (ExpiredCategoryDataListContract expiredCategory in expiredCategoryList)
                    {
                        if (expiredCategory != null)
                        {
                            try
                            {
                                Int32 itemId = expiredCategory.ComplianceItemID;
                                Int32 categoryId = expiredCategory.ComplianceCategoryId;
                                Int32 packageId = expiredCategory.CompliancePackageId;
                                Int32 applicantId = expiredCategory.OrgUserId;
                                String complianceStatus = expiredCategory.ComplianceStatus;
                                Int32 complianceStatusId = expiredCategory.ComplianceStatusId;
                                Int32 packageSubscriptionId = expiredCategory.PackageSubscriptionID;
                                List<RuleObjectMapping> ruleObjectMappingList = getRuleObjectMapping(tenantId, itemId, categoryId, packageId);
                                RuleManager.evaluatePostSubmitRules(ruleObjectMappingList, applicantId, systemUserId, tenantId);

                                if (packageSubscriptionId != previousPckgSubsId && !expiredCategory.IsApproveByOverrideDisableStatus)
                                {
                                    //Send Mail
                                    ComplianceDataManager.SendMailOnComplianceStatusChange(tenantId, tenantName, complianceStatus, complianceStatusId, packageSubscriptionId, expiredCategory.HierarchyNodeID);
                                }

                                previousPckgSubsId = packageSubscriptionId;

                                PackageSubscription ps = ComplianceDataManager.GetPackageSubscriptionByID(tenantId, packageSubscriptionId);
                                //UAT-3654
                                if (!ps.IsNullOrEmpty() && ps.ExpiryDate > DateTime.Now)
                                {
                                    if (prevCatID != categoryId && prevApplicantComplianceCategoryID != expiredCategory.ApplicantComplianceCategoryID && !expiredCategory.IsApproveByOverrideDisableStatus)
                                    {
                                        //Create Dictionary
                                        Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                        dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(expiredCategory.UserFirstName, " ", expiredCategory.UserLastName));
                                        dictMailData.Add(EmailFieldConstants.COMPLIANCE_CATEGORY_EXPIRY_DATE, expiredCategory.ExpiryDate.ToString("MM/dd/yyyy"));
                                        dictMailData.Add(EmailFieldConstants.COMPLIANCE_CATEGORY_NAME, expiredCategory.CategoryName);
                                        dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                                        dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, expiredCategory.NodeHierarchy);
                                        dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);

                                        Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                        mockData.UserName = string.Concat(expiredCategory.UserFirstName, " ", expiredCategory.UserLastName);
                                        mockData.EmailID = expiredCategory.PrimaryEmailaddress;
                                        mockData.ReceiverOrganizationUserID = expiredCategory.OrgUserId;

                                        //Send mail
                                        CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_COMPLIANCE_CATEGORY_EXPIRED, dictMailData, mockData, tenantId, expiredCategory.HierarchyNodeID);

                                        //Send Message
                                        CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_COMPLIANCE_CATEGORY_EXPIRED, dictMailData, expiredCategory.OrgUserId, tenantId);
                                    }
                                }

                                prevCatID = categoryId;
                                prevApplicantComplianceCategoryID = expiredCategory.ApplicantComplianceCategoryID;

                                //Reset Business process by businessProcessID and recordID.                                
                                if (!String.IsNullOrEmpty(expiredCategory.ComplianceItemCode))
                                {
                                    Guid itemCode = new Guid(expiredCategory.ComplianceItemCode);
                                    if (itemCode == whlCatGuid)
                                    {
                                        QueueManagementManager.ResetBusinessProcess(tenantId, businessProcessID, expiredCategory.ApplicantComplianceItemID);
                                    }
                                }
                            }
                            catch (SysXException ex)
                            {
                                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace()
                                    + Environment.NewLine + "Tenant Id:" + tenantId
                                    + Environment.NewLine + "Item Data Id:" + expiredCategory.ApplicantComplianceItemID
                                    + Environment.NewLine + "Category Data Id:" + expiredCategory.ApplicantComplianceCategoryID
                                    + Environment.NewLine + ex.Message
                                    + Environment.NewLine + ex.StackTrace, ex);
                                ex.Data["TenantId"] = tenantId;
                                ex.Data["ItemDataId"] = expiredCategory.ApplicantComplianceItemID;
                                ex.Data["CategoryDataID"] = expiredCategory.ApplicantComplianceCategoryID;
                                throw ex;
                            }
                            catch (Exception ex)
                            {
                                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace()
                                    + Environment.NewLine + "Tenant Id:" + tenantId
                                    + Environment.NewLine + "Item Data Id:" + expiredCategory.ApplicantComplianceItemID
                                    + Environment.NewLine + "Category Data Id:" + expiredCategory.ApplicantComplianceCategoryID
                                    + Environment.NewLine + ex.Message
                                    + Environment.NewLine + ex.StackTrace, ex);
                                ex.Data["TenantId"] = tenantId;
                                ex.Data["ItemDataId"] = expiredCategory.ApplicantComplianceItemID;
                                ex.Data["CategoryDataID"] = expiredCategory.ApplicantComplianceCategoryID;
                                throw (new SysXException(ex.Message, ex));
                            }
                        }
                    }
                    return true;
                }
                return false;
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
        /// Assign the datatable record in ExpiredCategoryDataListContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List<QueueAuditRecordContract></returns>
        private static List<ExpiredCategoryDataListContract> AssignExpiredCategoryToDataModel(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new ExpiredCategoryDataListContract
                {
                    ApplicantComplianceCategoryID = x["ApplicantComplianceCategoryID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["ApplicantComplianceCategoryID"]),
                    ApplicantComplianceItemID = x["ApplicantComplianceItemID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["ApplicantComplianceItemID"]),
                    ComplianceItemID = x["ComplianceItemID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["ComplianceItemID"]),
                    CategoryName = Convert.ToString(x["CategoryName"]),
                    ExpiryDate = Convert.ToDateTime(x["ExpiryDate"]),
                    ComplianceCategoryId = x["ComplianceCategoryId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["ComplianceCategoryId"]),
                    CompliancePackageId = x["CompliancePackageId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["CompliancePackageId"]),
                    OrgUserId = x["OrgUserId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["OrgUserId"]),
                    PrimaryEmailaddress = Convert.ToString(x["PrimaryEmailaddress"]),
                    UserFirstName = Convert.ToString(x["UserFirstName"]),
                    UserLastName = Convert.ToString(x["UserLastName"]),
                    NodeHierarchy = Convert.ToString(x["NodeHierarchy"]),
                    ComplianceStatusId = x["ComplianceStatusId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["ComplianceStatusId"]),
                    ComplianceStatus = Convert.ToString(x["UserFirstName"]),
                    PackageSubscriptionID = x["PackageSubscriptionID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["PackageSubscriptionID"]),
                    HierarchyNodeID = x["HierarchyNodeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["HierarchyNodeID"]),
                    ComplianceItemCode = x["ComplianceItemCode"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["ComplianceItemCode"]),
                    IsApproveByOverrideDisableStatus = x["IsApproveByOverrideDisableStatus"].GetType().Name == "DBNULL" ? false : Convert.ToBoolean(x["IsApproveByOverrideDisableStatus"]), //UAT 3106
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
        /// method for evaluating buisness rule on item whose next action is schedule to Execute Category Rules.
        /// </summary>
        /// <param name="applicantId">Applicant Id</param>
        /// <param name="systemUserId">LoggedIn User Id</param>
        /// <param name="tenantId">tenant id</param>
        ///<returns>Bool whether some records were processed or not</returns>
        public static Boolean ProcessActionExecuteRulesOnObjectDeletion(Int32 tenantId, String tenantName, Int32 chunkSize)
        {
            try
            {
                DateTime todayDate = DateTime.Now;
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 systemUserId = AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                String ObjectDltnActionTypeCode = ScheduleActionType.EXECUTE_RULES_AFTER_OBJECT_DELETION.GetStringValue();
                lkpScheduledActionType scheduledActionType = LookupManager.GetLookUpData<lkpScheduledActionType>(tenantId).FirstOrDefault(cond => cond.SAT_Code == ObjectDltnActionTypeCode);
                Int32 ObjectDltnActionTypeId = scheduledActionType.IsNotNull() ? scheduledActionType.SAT_ID : AppConsts.NONE;
                if (appConfiguration.IsNotNull())
                {
                    systemUserId = Convert.ToInt32(appConfiguration.AC_Value);
                }
                List<ScheduledAction> scheduleActionList = new List<ScheduledAction>();
                scheduleActionList = BALUtils.GetRuleRepoInstance(tenantId).GetObjectDeletionActiveScheduleActionList(chunkSize);
                if (scheduleActionList.Count > 0)
                {
                    foreach (ScheduledAction scheduledActionToBeExecuted in scheduleActionList)
                    {
                        if (scheduledActionToBeExecuted != null)
                        {
                            try
                            {
                                BALUtils.GetRuleRepoInstance(tenantId).ExecuteRulesOnObjectDeletion(scheduledActionToBeExecuted.SA_PackageSubscriptionID, scheduledActionToBeExecuted.SA_ObjectTypeID, scheduledActionToBeExecuted.SA_CreatedByID, scheduledActionToBeExecuted.SA_ObjectID);
                                BALUtils.GetRuleRepoInstance(tenantId).inactiveProcessedScheduleAction(scheduledActionToBeExecuted.SA_ID, systemUserId);
                            }
                            catch (SysXException ex)
                            {
                                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace()
                                    + Environment.NewLine + "Tenant Id:" + tenantId
                                    + Environment.NewLine + "Package Subscription Id:" + scheduledActionToBeExecuted.SA_PackageSubscriptionID
                                     + Environment.NewLine + "ScheduleActionId:" + scheduledActionToBeExecuted.SA_ID
                                    + Environment.NewLine + ex.Message
                                    + Environment.NewLine + ex.StackTrace, ex);
                                ex.Data["TenantId"] = tenantId;
                                ex.Data["PackageSubscriptionId"] = scheduledActionToBeExecuted.SA_PackageSubscriptionID;
                                throw ex;
                            }
                            catch (Exception ex)
                            {
                                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace()
                                    + Environment.NewLine + "Tenant Id:" + tenantId
                                    + Environment.NewLine + "Package Subscription Id:" + scheduledActionToBeExecuted.SA_PackageSubscriptionID
                                     + Environment.NewLine + "ScheduleActionId:" + scheduledActionToBeExecuted.SA_ID
                                    + Environment.NewLine + ex.Message
                                    + Environment.NewLine + ex.StackTrace, ex);
                                ex.Data["TenantId"] = tenantId;
                                ex.Data["PackageSubscriptionId"] = scheduledActionToBeExecuted.SA_PackageSubscriptionID;
                                throw (new SysXException(ex.Message, ex));
                            }
                        }
                    }
                    return true;//Returns true if some records were found for procesing.
                }
                return false;//Returns false if no record was processed.
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

        #region UAT-1217:Notification to correspond with UAT-1209
        /// <summary>
        /// Get all applicant for categories that are going to be non compliance from compliance by compliance required category action.
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="tenantName">tenantName</param>
        /// <param name="remBfrExp">remBfrExp</param>
        /// <param name="remExpFrq">remExpFrq</param>
        /// <param name="chunkSize">chunkSize</param>
        /// <returns></returns>
        public static List<GetUpcomingNonComplianceCategories> GetNonComplianceRequiredCategoryActionData(Int32 tenantId, Int32 remBfrExp, Int32 remExpFrq, Int32 chunkSize)
        {
            try
            {
                List<GetUpcomingNonComplianceCategories> nonComplianceRequiredCategoryList = new List<GetUpcomingNonComplianceCategories>();
                nonComplianceRequiredCategoryList = AssignNonComplianceRequiredCategoryToDataModel(BALUtils.GetRuleRepoInstance(tenantId)
                                                                                                  .GetNonComplianceRequiredCategoryActionData(remBfrExp, remExpFrq, chunkSize, tenantId));
                return nonComplianceRequiredCategoryList;
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

        private static List<GetUpcomingNonComplianceCategories> AssignNonComplianceRequiredCategoryToDataModel(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new GetUpcomingNonComplianceCategories
                {
                    ApplicantComplianceCategoryID = x["ApplicantComplianceCategoryID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["ApplicantComplianceCategoryID"]),
                    ApplicantID = x["ApplicantID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["ApplicantID"]),
                    UserFullName = Convert.ToString(x["UserFullName"]),
                    CategoryName = Convert.ToString(x["CategoryName"]),
                    PackageName = Convert.ToString(x["PackageName"]),
                    PrimaryEmailAddress = Convert.ToString(x["PrimaryEmailAddress"]),
                    SecondaryEmailAddress = Convert.ToString(x["SecondaryEmailAddress"]),
                    NodeHierarchy = Convert.ToString(x["NodeHierarchy"]),
                    HierarchyNodeID = x["HierarchyNodeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["HierarchyNodeID"]),
                    DueDate = Convert.ToDateTime(x["CategoryRequiredDueDate"]),
                    ComplianceCategoryID = x["ComplianceCategoryID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["ComplianceCategoryID"]),
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
        #endregion


        public static List<ItemSeriesItem> GetSeriesItemList(Int32 seriesId, Int32 selectedTenantId)
        {
            try
            {
                List<ItemSeriesItem> lstItemSeriesItem = BALUtils.GetRuleRepoInstance(selectedTenantId).GetItemSeriesItemsBySeriesId(seriesId);
                foreach (var itemSeriesItem in lstItemSeriesItem)
                {
                    itemSeriesItem.ISI_ItemName = itemSeriesItem.ComplianceItem.ItemLabel.IsNullOrEmpty() ?
                                              itemSeriesItem.ComplianceItem.Name : itemSeriesItem.ComplianceItem.ItemLabel;
                }
                return lstItemSeriesItem;
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

        public static List<ItemSeriesAttribute> GetItemSeriesAttributeBySeriesId(Int32 seriesId, Int32 selectedTenantId)
        {
            try
            {
                List<ItemSeriesAttribute> lstItemSeriesAttribute = BALUtils.GetRuleRepoInstance(selectedTenantId).GetItemSeriesAttributeBySeriesId(seriesId);

                foreach (var itemSeriesAttribute in lstItemSeriesAttribute)
                {
                    itemSeriesAttribute.ISA_AttributeName = itemSeriesAttribute.ComplianceAttribute.AttributeLabel.IsNullOrEmpty() ?
                                              itemSeriesAttribute.ComplianceAttribute.Name : itemSeriesAttribute.ComplianceAttribute.AttributeLabel;
                }
                return lstItemSeriesAttribute;
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


        public static Boolean AddShotSeriesRuleMapping(RuleMapping ruleMapping, Int32 tenantId, String SelectedObjectTypeCode, List<Int32> selectedObjectIds, Int32 currentUserId, Boolean isCreatedByAdmin)
        {
            try
            {
                String ruleSetName = "RuleSet_" + SelectedObjectTypeCode;
                RuleSet newRuleSet = new RuleSet
                {
                    RLS_Name = "",
                    RLS_IsActive = true,
                    RLS_Description = "",
                    RLS_TenantID = tenantId,
                    RLS_IsCreatedByAdmin = isCreatedByAdmin,
                    RLS_AssignmentHierarchyID = null,
                    RLS_Code = Guid.NewGuid(),
                    RLS_IsDeleted = false,
                    RLS_CreatedByID = currentUserId,
                    RLS_CreatedOn = DateTime.Now
                };

                Int32 objectTypeId = GetObjectType(SelectedObjectTypeCode, tenantId).OT_ID;
                foreach (var id in selectedObjectIds)
                {
                    RuleSetObject ruleSetObject = new RuleSetObject();
                    ruleSetObject.RLSO_ObjectID = id;
                    ruleSetObject.RLSO_ObjectTypeID = objectTypeId;
                    ruleSetObject.RLSO_IsActive = true;
                    ruleSetObject.RLS_CreatedById = currentUserId;
                    ruleSetObject.RLS_CreatedOn = DateTime.Now;
                    newRuleSet.RuleSetObjects.Add(ruleSetObject);
                }

                ruleMapping.RuleSet = newRuleSet;
                return BALUtils.GetRuleRepoInstance(tenantId).AddRuleMapping(ruleMapping);
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

        public static List<RuleMapping> GetShotSeriesRuleMappings(Int32 seriesId, Int32 tenantId)
        {
            try
            {
                Int32 seriesObjectTypeId = GetObjectType(ObjectType.Series.GetStringValue(), tenantId).OT_ID;
                Int32 seriesItemObjectTypeId = GetObjectType(ObjectType.Series_Item.GetStringValue(), tenantId).OT_ID;
                return BALUtils.GetRuleRepoInstance(tenantId).GetShotSeriesRuleMappings(seriesId, seriesObjectTypeId, seriesItemObjectTypeId);
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

        public static Boolean UpdateShotSeriesRuleMapping(RuleMapping ruleMapping, List<Int32> objectIds, String selectedObjectTypeCode, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                Int32 objectTypeId = GetObjectType(selectedObjectTypeCode, tenantId).OT_ID;
                return BALUtils.GetRuleRepoInstance(tenantId).UpdateShotSeriesRuleMapping(ruleMapping, objectIds, objectTypeId, currentUserId);
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

        public static void DeleteShotSeriesRuleMapping(Int32 ruleMappingId, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetRuleRepoInstance(tenantId).DeleteShotSeriesRuleMapping(ruleMappingId, currentUserId);
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
        /// UAT-2044: To copy Rule Template from one tenant to another or within the same tenant
        /// </summary>
        /// <param name="fromTenantId"></param>
        /// <param name="toTenantID"></param>
        /// <param name="ruleTemplateName"></param>
        /// <param name="orgUserId"></param>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static Boolean CopyRuleTemplate(Int32 fromTenantId, Int32 toTenantID, String ruleTemplateName, Int32 orgUserId, Int32 ruleId)
        {
            try
            {
                RuleTemplate rule = BALUtils.GetRuleRepoInstance(fromTenantId).GetRuleTemplate(ruleId);
                return BALUtils.GetRuleRepoInstance(toTenantID).CopyRuleTemplate(rule, ruleTemplateName, orgUserId);
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


        public static String TestComplianceRule(Int32 tenantId, String inputXml)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).TestComplianceRule(inputXml);
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

        public static Tuple<Dictionary<Boolean, String>, Dictionary<Int32, Int32>> EvaluateDataEntryUIRules(Int32 tenantId,
                                                                                    Int32 packageSubscriptionID, String nonSeriesData, String seriesData)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantId).EvaluateDataEntryUIRules(packageSubscriptionID, nonSeriesData, seriesData);
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


        public static void EvaluateRequirementPostSubmitRules(string ruleObjectXml, Int32 reqSubscriptionId, Int32 systemUserId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetRuleRepoInstance(tenantId).EvaluateRequirementPostSubmitRules(ruleObjectXml, reqSubscriptionId, systemUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
            }
        }
        //UAT-2740
        public static bool IsActionTypeDueDate(Int32 tenantID, Int32 RuleActionTypeID)
        {
            try
            {
                Int32 DueDateRuleActionID = LookupManager.GetLookUpData<lkpRuleActionType>(tenantID).Where(con => con.ACT_Code.Trim() == "STDD").Select(sel => sel.ACT_ID).FirstOrDefault();

                if (DueDateRuleActionID == RuleActionTypeID)
                    return true;
                else
                    return false;
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
        //UAT-2740
        public static string CalculateDueDate(Int32 tenantID, String resultXml)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantID).CalculateDueDate(resultXml);
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

        public static List<ComplianceExceptionExpiryData> GetComplianceExceptionAboutToExpire(Int32 tenantID, Int32 subEventId, Int32 chunkSize)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantID).GetComplianceExceptionAboutToExpire(tenantID, subEventId, chunkSize);
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

        public static List<RuleSetData> GetRuleSetDataByObjectId(Int32 tenantID, Int32 objectId, String objectTypeCode)
        {
            try
            {
                Int32 objectTypeId = GetObjectType(objectTypeCode, tenantID).OT_ID;
                return BALUtils.GetRuleRepoInstance(tenantID).GetRuleSetDataByObjectId(objectId, objectTypeId);
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
        public static Boolean IsRuleAssociationExists(Int32 tenantID, Int32 AffectedRuleId)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantID).IsRuleAssociationExists(AffectedRuleId);
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
        public static CompliancePackage GetCompliancePackageByPackageId(Int32 tenantID, Int32 packageId)
        {
            try
            {
                return BALUtils.GetRuleRepoInstance(tenantID).GetCompliancePackageByPackageId(packageId);
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
    }
}
