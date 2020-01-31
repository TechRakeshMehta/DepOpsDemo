using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IExpressionObjectView
    {
        Int32 TenantId { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        Int32 PackageId { get; set; }

        IExpressionObjectView CurrentViewContext { get; }

        List<CompliancePackageCategory> lstCompliancePackageCategories { get; set; }

        List<ComplianceCategoryItem> lstComplianceCategoryItems { get; set; }

        List<ComplianceItemAttribute> lstComplianceItemAttributes { get; set; }

        List<lkpRuleObjectMappingType> lstRuleObjectMappingType { get; set; }

        List<lkpObjectType> lstObjectType { get; set; }

        String SelectedMappingTypeCode { get; set; }

        List<lkpConstantType> lstConstantType { get; set; }

        List<lkpConstantType> lstConstantGroup { get; set; }

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




