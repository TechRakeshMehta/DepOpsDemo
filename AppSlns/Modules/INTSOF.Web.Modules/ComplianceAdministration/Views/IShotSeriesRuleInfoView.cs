using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IShotSeriesRuleInfoView
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

        IShotSeriesRuleInfoView CurrentViewContext { get; }

        RuleMapping RuleMapping { get; set; }

        Int32 RuleMappingId { get; set; }

        Int32 RuleSetId { get; set; }

        Int32 ObjectCount { get; set; }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the IsVersionUpdate
        /// </summary>
        Boolean IsVersionUpdate
        {
            get;
            set;
        }

        Int32 PreviousRuleMappingId
        {
            get;
            set;

        }

        Int32? FirstVersionRuleId
        {
            get;
            set;

        }

        String SettingXml
        {
            get;
            set;
        }

        Boolean IsScheduleActionRecordInserted
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




