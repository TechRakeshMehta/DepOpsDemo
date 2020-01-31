using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IShotSeriesRuleListView
    {
        Int32 TenantId
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId { get; }

        /// <summary>
        /// Id of the rule template selected for the use 
        /// </summary>
        Int32 SelectedRuleTemplateId { get; set; }

        List<RuleTemplate> lstRuleTemplates { get; set; }

        RuleTemplate RuleTemplateDetails { get; set; }

        IShotSeriesRuleListView CurrentViewContext { get; }

        List<RuleMapping> lstRuleMapping { get; set; }

        Int32 RuleMappingId { get; set; }

        Int32 RuleSetId { get; set; }

        String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 DefaultTenantId
        {
            get;
            set;
        }


        String ErrMsg
        {
            get;
            set;
        }

        Int32 SeriesId
        {
            get;
            set;
        }

        String SelectedObjectTypeCode
        {
            get;
            set;
        }

        List<Int32> SelectedObjectIds
        {
            get;
            set;
        }
    }
}




