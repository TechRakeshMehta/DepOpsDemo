using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IRulesetListView
    {
        IRulesetListView CurrentViewContext
        {
            get;
        }

        ComplianceRuleSetContract ViewContract
        {
            get;
        }

        List<RuleSet> RuleSetList
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 ObjectId
        {
            get;
            set;
        }

        Int32 ObjectTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Error Message
        /// </summary>
        String ErrorMessage
        {
            get;
            set;
        }

        List<lkpRuleType> RuleSetType
        {
            get;
            set;
        }

        List<RuleSet> complianceRuleSets
        {
            get;
            set;

        }

        Int32 selectedRuleSetId
        {
            get;
            set;
        }

        RuleSet ComplianceRuleSet
        {
            get;
            set;
        }

        Int32 parentPackageId
        {
            get;
            set;
        }

        Int32 parentCategoryId
        {
            get;
            set;
        }

        Int32 parentItemId
        {
            get;
            set;
        }

        String ObjectTypeCode
        {
            get;

        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }
    }
}




