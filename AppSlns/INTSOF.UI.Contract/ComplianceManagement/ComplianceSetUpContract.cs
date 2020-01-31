using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ComplianceSetUpContract
    {
        public List<CompliancePackage> CompliancePackageList
        {
            get;
            set;
        }

        public List<ComplianceCategory> ComplianceCategoryList
        {
            get;
            set;
        }

        public List<ComplianceItem> ComplianceItemList
        {
            get;
            set;
        }

        public List<ComplianceAttribute> ComplianceAttributeList
        {
            get;
            set;
        }

        public List<LargeContent> LargeContentList
        {
            get;
            set;
        }

        public List<RuleMapping> RuleMappingList
        {
            get;
            set;
        }

        public List<RuleMappingDetail> RuleMappingDetailList
        {
            get;
            set;
        }

        public List<RuleTemplateExpression> RuleTemplateExpressionList
        {
            get;
            set;
        }

        public List<RuleMappingObjectTree> RuleMappingObjectTreeList
        {
            get;
            set;
        }

        public List<lkpObjectType> LkpObjectTypeList
        {
            get;
            set;
        }

        public List<lkpRuleObjectMappingType> LkpRuleObjectMappingTypeList
        {
            get;
            set;
        }

        public List<RuleSetTree> RuleSetTreeList
        {
            get;
            set;
        }

        public List<RuleSet> RuleSetList
        {
            get;
            set;
        }

        public List<AssignmentHierarchy> AssignmentHierarchyList
        {
            get;
            set;
        }

        public List<RuleSet> RuleSetToBeCopied
        {
            get;
            set;
        }

        public List<CompliancePackageCategory> CompliancePackageCategoryList
        {
            get;
            set;
        }

        public List<Entity.InstitutionWebPage> InstitutionWebPageList
        {
            get;
            set;
        }

        public List<AttributeInstruction> AttributeInstructionList
        {
            get;
            set;
        }

        public List<UniversalCategoryMapping> lstUniversalCategorymapping
        {
            get;
            set;
        }

        public List<UniversalItemMapping> lstUniversalItemMapping
        {
            get;
            set;
        }

        public List<UniversalFieldMapping> lstUniversalAttrMapping
        {
            get;
            set;
        }

        public List<UniversalFieldOptionMapping> lstUniversalAttrOptionMapping
        {
            get;
            set;
        }
        public List<TrackingPackageRequiredDocURL> lstTrackingPackageRequiredDocURL
        {
            get;
            set;
        }
        public List<TrackingPackageRequiredDocURLMapping> lstTrackingPackageRequiredDocURLMapping
        {
            get;
            set;
        }


    }
}
