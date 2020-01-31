using System;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class RequirementPackageHierarchicalDetailsContract
    {
        [DataMember]
        public Int32 RequirementPackageID { get; set; }
        [DataMember]
        public Guid RequirementPackageCode { get; set; }
        [DataMember]
        public String RequirementPackageName { get; set; }
        [DataMember]
        public Int32 PackageObjectTreeID { get; set; }
        [DataMember]
        public String RequirementPackageLabel { get; set; }
        //[DataMember]
        //public String RequirementPackageDescription { get; set; }
        [DataMember]
        public String RequirementPackageRuleTypeCode { get; set; }
        [DataMember]
        public Int32 RequirementPackageCategoryID { get; set; }
        [DataMember]
        public Int32 CategoryDisplayOrder { get; set; }
        [DataMember]
        public Int32 RequirementCategoryID { get; set; }
        [DataMember]
        public Guid RequirementCategoryCode { get; set; }
        [DataMember]
        public String RequirementCategoryName { get; set; }

        //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes)
        [DataMember]
        public String RequirementDocumentLink { get; set; }

        //UAT-3161
        [DataMember]
        public String RequirementDocumentLinkLabel { get; set; }

        //UAT:4121
        [DataMember]
        public String RItemURLSampleDocURL { get; set; }
        [DataMember]
        public String RItemURLLabel { get; set; }

        [DataMember]
        public String RequirementCategoryLabel { get; set; }
        [DataMember]
        public String RequirementCategoryDescription { get; set; }
        [DataMember]
        public String CategoryExplanatoryNotes { get; set; }
        [DataMember]
        public String RequirementCategoryRuleTypeCode { get; set; }
        [DataMember]
        public Int32 CategoryObjectTreeID { get; set; }
        [DataMember]
        public Int32 RequirementCategoryItemID { get; set; }
        [DataMember]
        public Int32 RequirementItemID { get; set; }
        [DataMember]
        public Guid RequirementItemCode { get; set; }
        [DataMember]
        public String RequirementItemName { get; set; }
        [DataMember]
        public String RequirementItemLabel { get; set; }
        //UAT-3077
        [DataMember]
        public Boolean IsPaymentTypeItem { get; set; }
        [DataMember]
        public Decimal? ItemAmount { get; set; }

        [DataMember]
        public String RequirementItemDescription { get; set; }
        [DataMember]
        public Int32 ItemObjectTreeID { get; set; }
        [DataMember]
        public String RequirementItemRuleTypeCode { get; set; }
        [DataMember]
        public Guid DateTypeRequirementFieldCodeForExpiration { get; set; }
        [DataMember]
        public Int32 DateTypeRequirementFieldIDForExpiration { get; set; }
        [DataMember]
        public String ExpirationValueTypeCode { get; set; }
        [DataMember]
        public Int32 ExpirationValueTypeID { get; set; }
        [DataMember]
        public String ExpirationValue { get; set; }
        [DataMember]
        public Int32 RequirementItemFieldID { get; set; }
        [DataMember]
        public Int32 RequirementFieldID { get; set; }
        [DataMember]
        public Guid RequirementFieldCode { get; set; }
        [DataMember]
        public String RequirementFieldName { get; set; }
        [DataMember]
        public String RequirementFieldLabel { get; set; }
        [DataMember]
        public Int32? RequirementFieldAttributeTypeID { get; set; }
        [DataMember]
        public String RequirementFieldDescription { get; set; }
        [DataMember]
        public String RequirementFieldRuleTypeCode { get; set; }
        [DataMember]
        public String RequirementFieldRuleValue { get; set; }
        [DataMember]
        public Int32 FieldObjectTreeID { get; set; }
        [DataMember]
        public Int32 RequirementFieldDataTypeID { get; set; }
        [DataMember]
        public String RequirementFieldDataTypeName { get; set; }
        [DataMember]
        public String RequirementFieldDataTypeCode { get; set; }
        [DataMember]
        public Int32 RequirementFieldVideoID { get; set; }
        [DataMember]
        public String RequirementFieldVideoName { get; set; }
        [DataMember]
        public String FieldVideoURL { get; set; }
        //[DataMember]
        //public Boolean IsVideoOpenRequired { get; set; }
        //[DataMember]
        //public Int32 VideoOpenTimeDuration { get; set; }
        [DataMember]
        public Int32 RequirementFieldOptionsID { get; set; }
        [DataMember]
        public String OptionText { get; set; }
        [DataMember]
        public String OptionValue { get; set; }
        [DataMember]
        public Int32 RequirementFieldDocumentID { get; set; }
        [DataMember]
        public Int32 ClientSystemDocumentID { get; set; }
        [DataMember]
        public String ClientSystemDocumentFileName { get; set; }
        [DataMember]
        public Int32 ClientSystemDocumentSize { get; set; }
        [DataMember]
        public String ClientSystemDocumentPath { get; set; }
        [DataMember]
        public String ClientSystemDocumentDescription { get; set; }
        [DataMember]
        public Int32 DocumentTypeID { get; set; }
        [DataMember]
        public String DocumentTypeCode { get; set; }
        [DataMember]
        public Int32 RequirementDocumentAcroFieldID { get; set; }
        [DataMember]
        public String DocumentAcroFieldTypeID { get; set; }
        [DataMember]
        public String DocumentAcroFieldTypeCode { get; set; }
        [DataMember]
        public String DocumentAcroFieldTypeName { get; set; }
        [DataMember]
        public Int32 RequirementPackageAgencyID { get; set; }
        [DataMember]
        public Int32 AgencyID { get; set; }
        [DataMember]
        public String AgencyName { get; set; }
        //UAT 1352:As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use
        [DataMember]
        public Int32 RequirementPkgTypeID { get; set; }
        [DataMember]
        public String RequirementPkgTypeCode { get; set; }
        /// <summary>
        /// This property will be true if package is created by Agency User and to be created in Shared DB
        /// </summary>
        [DataMember]
        public Boolean IsSharedUserPackage { get; set; }
        [DataMember]
        public Int32 RequirementPackageInstitutionID { get; set; }
        [DataMember]
        public Int32 MappedTenantID { get; set; }
        [DataMember]
        public String MappedTenantName { get; set; }
        [DataMember]
        public Boolean IsUsed { get; set; }
        [DataMember]
        public Boolean IsCopied { get; set; }


        [DataMember]
        public String RequirementCategorySQLExpression { get; set; }

        [DataMember]
        public String RequirementCategoryUIExpression { get; set; }

        #region UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)

        [DataMember]
        public Boolean IsComplianceRequired { get; set; }

        [DataMember]
        public DateTime? ComplianceReqStartDate { get; set; }

        [DataMember]
        public DateTime? ComplianceReqEndDate { get; set; }

        [DataMember]
        public DateTime? ExpirationCondStartDate { get; set; }

        [DataMember]
        public DateTime? ExpirationCondEndDate { get; set; }
        #endregion

        //UAT-2164
        [DataMember]
        public Boolean IsBackgroundDocument { get; set; }

        [DataMember]
        public String ErrorMessage { get; set; }

        //UAT-2366
        [DataMember]
        public Int32? UiRequirementItemID { get; set; }

        [DataMember]
        public Int32? UiRequirementFieldID { get; set; }

        [DataMember]
        public String RequirementFieldFixedRuleTypeCode { get; set; }

        [DataMember]
        public String UiRuleErrorMessage { get; set; }

        [DataMember]
        public string DefinedRequirementDescription { get; set; }

        [DataMember]
        public int? DefinedRequirementID { get; set; }

        #region UAT-2514 Copy Package

        [DataMember]
        public DateTime? RequirementPackageEffectiveStartDate { get; set; }

        [DataMember]
        public DateTime? RequirementPackageEffectiveEndDate { get; set; }

        [DataMember]
        public Boolean IsNewRequirementPackage { get; set; }

        public DateTime? RequirementCategoryCompReqdStartDate { get; set; }

        [DataMember]
        public DateTime? RequirementCategoryCompReqdEndDate { get; set; }

        [DataMember]
        public Boolean IsNewRequirementCategory { get; set; }

        [DataMember]
        public Boolean IsCategoryComplianceRequired { get; set; }

        [DataMember]
        public Boolean IsPackageArchived { get; set; }

        #endregion

        #region [UAT-2203]

        [DataMember]
        public Boolean ReqCatAllowDataMovement { get; set; }

        [DataMember]
        public Boolean ReqItmAllowDataMovement { get; set; }

        #endregion

        [DataMember]
        public Nullable<Int32> ReqFieldMaxLength { get; set; }

        #region UAT-3078

        [DataMember]
        public Int32 RequirementCategoryItemDisplayOrder { get; set; }

        [DataMember]
        public Int32 RequirementItemFieldDisplayOrder { get; set; }

        #endregion

        #region UAT-3176
        [DataMember]
        public int RequirementAttributeGroupID { get; set; }
        #endregion

        [DataMember]
        public int? ReqReviewByID { get; set; }

        [DataMember]
        public string ReqReviewByDesc { get; set; }

        #region UAT-3309
        public String RequirementItemSampleDocumentFormURL { get; set; }
        #endregion

        //UAT-3805
        [DataMember]
        public Boolean SendItemDocOnApproval { get; set; }

        [DataMember]
        public Boolean AllowItemDataEntry { get; set; }

        #region UAT-4165
        [DataMember]
        public Int32 RequirementObjPropCategoryID { get; set; }

        [DataMember]
        public Boolean? RequirementCategoryPropIsCustomSettings { get; set; }
        [DataMember]
        public Boolean? RequirementCategoryPropIsEditableByAdmin { get; set; }

        [DataMember]
        public Boolean? RequirementCategoryPropIsEditableByApplicant { get; set; }
        [DataMember]
        public Boolean? RequirementCategoryPropIsEditableByClientAdmin { get; set; }

        [DataMember]
        public Int32 RequirementItemPropCategoryItemID { get; set; }
        [DataMember]
        public Int32 RequirementItemPropCategoryID { get; set; }

        [DataMember]
        public Boolean? RequirementItemPropIsCustomSettings { get; set; }
        [DataMember]
        public Boolean? RequirementItemPropIsEditableByAdmin { get; set; }

        [DataMember]
        public Boolean? RequirementItemPropIsEditableByApplicant { get; set; }
        [DataMember]
        public Boolean? RequirementItemPropIsEditableByClientAdmin { get; set; }

        //UAT 4380
        [DataMember]
        public Int32 RequirementItemFieldPropCategoryID { get; set; }
        [DataMember]
        public Int32 RequirementItemFieldPropCategoryItemID { get; set; }

        [DataMember]
        public Int32 RequirementItemFieldPropItemFieldID { get; set; }
        [DataMember]
        public Boolean? RequirementItemFieldPropIsCustomSettings { get; set; }
        [DataMember]
        public Boolean? RequirementItemFieldPropIsEditableByAdmin { get; set; }

        [DataMember]
        public Boolean? RequirementItemFieldPropIsEditableByApplicant { get; set; }
        [DataMember]
        public Boolean? RequirementItemFieldPropIsEditableByClientAdmin { get; set; }


        #endregion

        [DataMember]
        public Guid? ParentPackageCode { get; set; }
    }
}



