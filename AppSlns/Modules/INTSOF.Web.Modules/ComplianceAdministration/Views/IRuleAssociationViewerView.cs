using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IRuleAssociationViewerView
    {
        Int32 SelectedTenantId { get; set; }
        Int32 RuleSetId { get; set; }
        Int32 RuleMappingId { get; set; }
        List<CompliancePackage> PackageListForSharingRuleInstance { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 ObjectId { get; set; }
        String ObjectType { get; set; }
        Int32 PackageId { get; set; }
        Int32 CurrentCategoryID { get; set; }
        Int32 CurrentItemID { get; set; }
    }
}
