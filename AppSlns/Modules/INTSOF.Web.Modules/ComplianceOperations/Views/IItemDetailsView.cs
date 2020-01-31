using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IItemDetailsView
    {
        Int32 ItemId { get; set; }
        Int32 TenantId { get; set; }
        String ItemName { get; set; }
        Boolean ReadOnly { get; set; }
        Boolean SaveStatus { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 ComplianceCategoryId { get; set; }
        Int32 PackageSubscriptionId { get; set; }
        IItemDetailsView CurrentViewContext { get; }
        Int32 ApplicantComplianceItemIdGenerated { get; set; }
        Dictionary<Int32, Int32> AttributeDocuments { get; set; }
        Int32 ApplicantComplianceCategoryIdGenerated { get; set; }
        ApplicantComplianceItemDataContract ItemDataContract { get; set; }
        ApplicantComplianceAttributeDataContract ViewContract { get; }
        ApplicantComplianceCategoryDataContract CategoryDataContract { get; set; }
        List<ApplicantComplianceAttributeDataContract> lstAttributesData { get; set; }
        Entity.ClientEntities.ApplicantComplianceItemData ApplicantItemData { get; set; }
        List<Entity.ClientEntities.ClientComplianceItemAttribute> ClientItemAttributes { get; set; }
    }
}




