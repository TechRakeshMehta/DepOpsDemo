using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IRuleListView
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

        IRuleListView CurrentViewContext { get; }

        //List<RuleMapping> lstRuleMapping { get; set; }
        List<RuleMappingContract> lstRuleMapping { get; set; }

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
        /// Gets and sets the IsVersionUpdate
        /// </summary>
        Boolean IsVersionUpdate
        {
            get;
            set;
        }

        Int32? FirstVersionRuleId
        {
            get;
            set;

        }

        Boolean IsAllSelected
        {
            get;
            set;
        }

        Boolean IsNewSelected
        {
            get;
            set;
        }

        Boolean IsExistingSelected
        {
            get;
            set;
        }

        List<UserGroupContract> UserGroups
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

        List<CompliancePackage> PackageListForSharingRuleInstance
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
        //UAT-2147 Auto-fill in dropdowns on rule mapping on tracking side
        Int32 CurrentCategoryID { get; set; }
        Int32 CurrentItemID { get; set; }
        Int32 PackageId { get; set; }
        Int32 ObjectId { get; set; }
        String ObjectType { get; set; }
        List<RuleSetData> lstRuleSetAssociationData { get; set; }
    }
}




