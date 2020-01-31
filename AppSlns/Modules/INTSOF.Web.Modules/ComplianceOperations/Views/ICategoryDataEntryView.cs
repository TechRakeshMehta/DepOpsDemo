using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface ICategoryDataEntryView
    {
        Int32 TenantId { get; set; }
        Int32 PackageSubscriptionId { get; set; }
        ICategoryDataEntryView CurrentViewContext { get; }
        ClientComplianceItemsContract ClientComplianceItemsContract { get; }
        Entity.ClientEntities.ApplicantComplianceCategoryData ApplicantCategoryData { get; set; }
        List<Entity.ClientEntities.ClientComplianceItem> ClientComplianceItems { get; set; }
    }
}




