using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class RequirementCategoryContract
    {
        [DataMember]
        public Int32 RequirementPackageID { get; set; }

        [DataMember]
        public Int32 RequirementCategoryID { get; set; }

        [DataMember]
        public Int32 RequirementPackageCategoryID { get; set; }

        [DataMember]
        public String RequirementCategoryName { get; set; }

        [DataMember]
        public Int32? CategoryDisplayOrder { get; set; }

        //No Description and no label field for any except attribute (Package, Category, Item). Duplicate names should be allowed. 
        //[DataMember]
        //public String RequirementCategoryLabel { get; set; }
        //[DataMember]
        //public String RequirementCategoryDescription { get; set; }

        [DataMember]
        public String ExplanatoryNotes { get; set; }

        /// <summary>
        /// Used to determine the category in which items are to be added currently(if multiple categories are being added).
        /// It will be true if items are being added in this category currently
        /// </summary>
        [DataMember]
        public Boolean IsCurrentCategory { get; set; }

        /// <summary>
        /// Used to depict : "All Items"/"Any Item"
        /// </summary>
        [DataMember]
        public String CategoryRuleTypeCode { get; set; }

        [DataMember]
        public Int32 CategoryRuleTypeID { get; set; }

        [DataMember]
        public Boolean IsUpdated { get; set; }

        [DataMember]
        public Boolean IsNewCategory { get; set; }

        [DataMember]
        public Boolean IsDeleted { get; set; }

        [DataMember]
        public Guid RequirementCategoryCode { get; set; }

        [DataMember]
        public List<RequirementItemContract> LstRequirementItem { get; set; }

        [DataMember]
        public Int32 CategoryObjectTreeID { get; set; }

        //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes).
        [DataMember]
        public String RequirementDocumentLink { get; set; }

        [DataMember]
        public String RuleUIExpression { get; set; }

        [DataMember]
        public String RuleSqlExpression { get; set; }

        #region UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)

        [DataMember]
        public Boolean IsComplianceRequired { get; set; }

        [DataMember]
        public DateTime? ComplianceReqStartDate { get; set; }

        [DataMember]
        public DateTime? ComplianceReqEndDate { get; set; }

        #endregion

        #region MyRegion
        [DataMember]
        public Int32 UniversalCategoryID { get; set; }

        [DataMember]
        public Int32 UniversalReqCatMappingID { get; set; }

        #endregion

        #region 2213
        [DataMember]
        public String RequirementCategoryLabel { get; set; }

        [DataMember]
        public String lstSelectedAgencyIds { get; set; }

        [DataMember]
        public String AgencyNames { get; set; }

        [DataMember]
        public Int32 TotalCount { get; set; }

        [DataMember]
        public Int32 CurrentLoggedInUserID { get; set; }
        [DataMember]
        public String CategoryLabel { get; set; }

        [DataMember]
        public Int32 lstAgency { get; set; }

        [DataMember]
        public Boolean IsCategoryMappedWithPkg { get; set; }

        [DataMember]
        public CustomPagingArgsContract GridCustomPagingArguments { get; set; }
        #endregion

        #region [UAT-2603]

        [DataMember]
        public Boolean AllowDataMovement { get; set; }

        #endregion

        //UAT-3161
        [DataMember]
        public String RequirementDocumentLinkLabel { get; set; }

        [DataMember]
        public Boolean SendItemDoconApproval { get; set; } //UAT-3805
        //UAT-4121

        [DataMember]
        public String RItemURLSampleDocURL { get; set; }


        [DataMember]
        public String RItemURLLabel { get; set; }

        [DataMember]

        public Dictionary<String, Boolean> SelectedlstEditableBy { get; set; }

        [DataMember]
        public RequirementObjectPropertiesContract RequirementObjectProperties { get; set; }
        [DataMember]
        public Boolean? IsEditableByApplicant { get; set; }

        //UAT-4259
        [DataMember]
        public Boolean TriggerOtherCategoryRules { get; set; }

        //Added In UAT-4254 in release 181
        [DataMember]
        public List<RequirementCategoryDocUrl> lstReqCatDocUrls { get; set; }

        //UAT-4705
        [DataMember]
        public String RequirementCategoryIDs { get; set; }
    }

    //Below class is added in UAT-4254
    [Serializable]
    [DataContract]
    public class RequirementCategoryDocUrl
    {
        [DataMember]
        public Int32 RequirementCatDocLinkID { get; set; }
        [DataMember]
        public Int32 RequirementCatID { get; set; }
        [DataMember]
        public String RequirementCatDocUrlLabel { get; set; }
        [DataMember]
        public String RequirementCatDocUrl { get; set; }
    }
}


