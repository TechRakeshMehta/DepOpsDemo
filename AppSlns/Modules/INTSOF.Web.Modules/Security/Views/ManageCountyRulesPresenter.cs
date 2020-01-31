#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IManageFeatureView.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.CompositeWeb;

#endregion

#region Application Specific

using INTSOF.Utils;
using BESTX.Entity;
using BESTX.Business.RepoManagers;
using  BESTX.WEB.IntsofSecurityModel.Providers;
using INTSOF.Utils.Consts;
using System.Collections.Generic;
#endregion

#endregion

namespace  BESTX.WEB.IntsofSecurityModel.Views
{
    public class ManageCountyRulesPresenter : Presenter<IManageCountyRulesView>
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        public override void OnViewInitialized()
        {
        }

        public Int32 GetStateID()
        {
            Int32 stateID = SecurityManager.GetStateID(View.ViewContract.CountyID);
            return stateID;
        }
        public void GetStateCategories(Int32 stateID)
        {
            Int32 tenantID = View.ViewContract.TenantID;
            View.Categories = SecurityManager.GetStateCategories(stateID, tenantID).ToList();
        }
        public void GetRules()
        {
            Int32 judgeID = View.ViewContract.JudgeID;
            Int32 countyID = View.ViewContract.CountyID;
            Int32 tenantID = View.ViewContract.TenantID;
            if (judgeID != 0)
            {
                List<Rule> judgeRules = SecurityManager.GetRulesForJudge(judgeID, tenantID).ToList();
                View.JudgeRules = judgeRules;
            }
            else
            {
                List<Rule> countyRules = SecurityManager.GetRulesForCounty(countyID, tenantID).ToList();
                View.CountyRules = countyRules;
            }
        }

        public Boolean UpdateRule()
        {
            Boolean IsSucess = false;
            Int32 categoryID = View.ViewContract.CategoryID;
            Int32 judgeID = View.ViewContract.JudgeID;
            Int32 countyID = View.ViewContract.CountyID;
            Int32 ruleID = View.ViewContract.RuleID;
            if (judgeID != 0)
            {
                JudgeRule judgeRule = SecurityManager.GetJudgeRuleCategory(judgeID, ruleID);
                if (judgeRule != null)
                {
                    judgeRule.JRU_RuleCategoryID = categoryID;
                    judgeRule.JRU_RuleID = ruleID;
                    judgeRule.JRU_ModifiedByID = 10;
                    judgeRule.JRU_ModifiedOn = DateTime.Now;
                    Rule rule = judgeRule.Rule;
                    rule.RUL_Name = View.ViewContract.RuleName;
                    rule.RUL_Description = View.ViewContract.RuleDescription;
                    rule.RUL_RuleCategoryID = View.ViewContract.CategoryID;
                    rule.RUL_ModifiedOn = DateTime.Now;
                    rule.RUL_ModifiedByID = 10;
                    IsSucess = SecurityManager.UpdateJudgeRule(judgeRule);
                }
            }
            else
            {
                CountyRule cRule = SecurityManager.GetCountyRuleCategory(countyID, ruleID);
                if (cRule != null)
                {
                    cRule.CRU_RuleCategoryID = categoryID;
                    cRule.CRU_RuleID = ruleID;
                    cRule.CRU_ModifiedByID = 10;
                    cRule.CRU_ModifiedOn = System.DateTime.Now;
                    Rule rule = cRule.Rule;
                    rule.RUL_Name = View.ViewContract.RuleName;
                    rule.RUL_Description = View.ViewContract.RuleDescription;
                    rule.RUL_RuleCategoryID = View.ViewContract.CategoryID;
                    rule.RUL_ModifiedOn = DateTime.Now;
                    rule.RUL_ModifiedByID = 10;
                    IsSucess = SecurityManager.UpdateCountyRule(cRule);
                }
            }
            View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.RULE) + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.UPDATED_SUCCESSFULLY);
            return IsSucess;
        }

        public Boolean SaveRule()
        {
            Int32 categoryID = View.ViewContract.CategoryID;
            Int32 judgeID = View.ViewContract.JudgeID;
            Int32 countyID = View.ViewContract.CountyID;
            Rule rule = new Rule();
            rule.RUL_Name = View.ViewContract.RuleName;
            rule.RUL_Description = View.ViewContract.RuleDescription;
            rule.RUL_RuleCategoryID = categoryID;
            rule.RUL_IsActive = true;
            rule.RUL_CreatedByID = 10;
            rule.RUL_CreatedOn = System.DateTime.Now;
            if (judgeID != 0)
            {
                JudgeRule jRule = new JudgeRule();
                jRule.JRU_JudgeID = judgeID;
                jRule.JRU_RuleCategoryID = categoryID;
                jRule.JRU_IsActive = true;
                jRule.JRU_CreatedByID = 10;
                jRule.JRU_CreatedOn = System.DateTime.Now;
                rule.JudgeRules.Add(jRule);
            }
            else
            {
                CountyRule cRule = new CountyRule();
                cRule.CRU_CountyID = countyID;
                cRule.CRU_RuleCategoryID = categoryID; //rule id
                cRule.CRU_IsActive = true;
                cRule.CRU_CreatedByID = 10;
                cRule.CRU_CreatedOn = System.DateTime.Now;
                rule.CountyRules.Add(cRule);
            }
            Int32 ruleID = SecurityManager.SaveRule(rule);
            View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.RULE) + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SAVED_SUCCESSFULLY);
            return true;
        }

        public Boolean DeleteRule()
        {
            Boolean IsSucess = false;
            Int32 categoryID = View.ViewContract.CategoryID;
            Int32 judgeID = View.ViewContract.JudgeID;
            Int32 countyID = View.ViewContract.CountyID;
            Int32 ruleID = View.ViewContract.RuleID;
            if (judgeID != 0)
            {
                JudgeRule judgeRule = SecurityManager.GetJudgeRuleCategory(judgeID, ruleID);
                if (judgeRule != null)
                {
                    judgeRule.JRU_IsActive = false;
                    judgeRule.JRU_ModifiedByID = 10;
                    judgeRule.JRU_ModifiedOn = DateTime.Now;
                    Rule rule = judgeRule.Rule;
                    rule.RUL_IsActive = false;
                    rule.RUL_ModifiedOn = DateTime.Now;
                    rule.RUL_ModifiedByID = 10;
                    IsSucess = SecurityManager.UpdateJudgeRule(judgeRule);
                }
            }
            else
            {
                CountyRule cRule = SecurityManager.GetCountyRuleCategory(countyID, ruleID);
                if (cRule != null)
                {
                    cRule.CRU_IsActive = false;
                    cRule.CRU_ModifiedByID = 10;
                    cRule.CRU_ModifiedOn = System.DateTime.Now;
                    Rule rule = cRule.Rule;
                    rule.RUL_IsActive = false;
                    rule.RUL_ModifiedOn = DateTime.Now;
                    rule.RUL_ModifiedByID = 10;
                    IsSucess = SecurityManager.UpdateCountyRule(cRule);
                }
            }
            View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.RULE) + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.DELETED_SUCCESSFULLY);
            return IsSucess;
        }
        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
