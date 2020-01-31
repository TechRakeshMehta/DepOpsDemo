using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IItemFormView
    {
        /// <summary>
        /// Id of the Master compliance Item (Client database), present in the ApplicantComplianceItem Entity. 
        /// </summary>
        Int32 ItemId { get; set; }

        Int32 TenantId { get; set; }
        String ItemName { get; set; }
        Boolean ReadOnly { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 ComplianceCategoryId { get; set; }
        Int32 PackageId { get; set; }
        Boolean IsItemSeries { get; set; }
        IItemFormView CurrentViewContext { get; }
        Int32 ApplicantComplianceItemId { get; set; }
        Dictionary<Int32, Int32> AttributeDocuments { get; set; }
        Int32 ApplicantComplianceCategoryId { get; set; }

        ApplicantComplianceAttributeDataContract ViewContract { get; }
        ApplicantComplianceItemDataContract ItemDataContract { get; set; }
        ApplicantComplianceCategoryDataContract CategoryDataContract { get; set; }
        ComplianceItem ClientComplianceItem { get; set; }
        List<ApplicantComplianceAttributeDataContract> lstAttributesData { get; set; }
        ApplicantComplianceItemData ApplicantItemData { get; set; }
        ApplicantComplianceCategoryData ApplicantCategoryData { get; set; }
        List<ComplianceItemAttribute> ClientItemAttributes { get; set; }
        //UAT-4067
        String SelectedNodeIds { get; set; }
        String AllowedExtensions { get; set; }
        Boolean IsAllowedFileExtensionEnable { get; set; }
    }
}




