using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    /// <summary>
    /// Contract to manage the data of the Verification Details screen.
    /// </summary> 
    [DataContract]
    public class RequirementVerificationDetailContract
    {
        [DataMember]
        public Int32 PkgId { get; set; }
        [DataMember]
        public Boolean IsItemDataEntryExist { get; set; } //UAT-4461
        [DataMember]
        public String PkgName { get; set; }

        [DataMember]
        public String PkgLabel { get; set; }

        [DataMember]
        public String PkgStatusCode { get; set; }
        [DataMember]
        public Boolean IsCategoryDataMovementAllowed { get; set; } //UAT-4253
        [DataMember]
        public Boolean IsItemDataMovementAllowed { get; set; } //UAT-4253
        [DataMember]
        public String PkgStatusName { get; set; }

        [DataMember]
        public String CatName { get; set; }

        /// <summary>
        /// PK of the RequirementCategory Table i.e. 'RC_ID'
        /// </summary>
        [DataMember]
        public Int32 CatId { get; set; }

        /// <summary>
        /// PK of the ApplicantRequirementCategoryData Table i.e. 'ARCD_ID'
        /// </summary>
        [DataMember]
        public Int32 ApplReqCatDataId { get; set; }

        [DataMember]
        public String CatStatusCode { get; set; }

        [DataMember]
        public String CatStatusName { get; set; }

        [DataMember]
        public String ItemName { get; set; }

        /// <summary>
        /// PK of the RequirementItem Table i.e. 'RI_ID'
        /// </summary>
        [DataMember]
        public Int32 ItemId { get; set; }

        /// <summary>
        /// PK of the ApplicantRequirementItemData Table i.e. 'ARID_ID'
        /// </summary>
        [DataMember]
        public Int32 ApplReqItemDataId { get; set; }

        [DataMember]
        public String ItemStatusCode { get; set; }

        [DataMember]
        public String ItemStatusName { get; set; }

        [DataMember]
        public String FieldName { get; set; }

        /// <summary>
        /// PK of the RequirementField Table i.e. 'RF_ID'
        /// </summary>
        [DataMember]
        public Int32 FieldId { get; set; }

        [DataMember]
        public String FieldDataTypeCode { get; set; }

        [DataMember]
        public String FieldAttributeTypeCode { get; set; }

        /// <summary>
        /// PK of the ApplicantRequirementFieldData Table i.e. 'ARFD_ID'
        /// </summary>
        [DataMember]
        public Int32 ApplReqFieldDataId { get; set; }

        [DataMember]
        public String FieldDataValue { get; set; }

        [DataMember]
        public String OptionText { get; set; }

        [DataMember]
        public String OptionValue { get; set; }

        /// <summary>
        /// PK of the ApplicantDocument table i.e. 'ApplicantDocumentID'
        /// </summary>
        [DataMember]
        public Int32 ApplDocId { get; set; }

        /// <summary>
        /// FileName from the ApplicantDocument table
        /// </summary>
        [DataMember]
        public String FieldDocName { get; set; }

        /// <summary>
        /// DocumentPath from the ApplicantDocument table
        /// </summary>
        [DataMember]
        public String FieldDocPath { get; set; }

        /// <summary>
        /// Item Rejection reason.
        /// </summary>
        [DataMember]
        public String RejectionReason { get; set; }

        /// <summary>
        /// Last Rank for Upload Document type so that they are generated as last attribute in an item
        /// </summary>
        [DataMember]
        public Int32 FieldRank { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean IsFieldRequired { get; set; }

        //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes)
        [DataMember]
        public String CategoryExplanatoryNotes
        { get; set; }

        [DataMember]
        public String RequirementDocumentLink
        { get; set; }

        [DataMember]
        public String RequirementDocumentLinkLabel
        { get; set; }

        //UAT-1626 : Update rotation package verification to mimic tracking verification.
        [DataMember]
        public String RotationMemberCount
        { get; set; }

        //UAT-2165: Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
        [DataMember]
        public Boolean IsComplianceRequired
        { get; set; }

        [DataMember]
        public String CategoryDescription
        { get; set; }

        [DataMember]
        public String ItemDescription
        { get; set; }

        [DataMember]
        public String ItemExplanatoryNotes
        { get; set; }

        [DataMember]
        public DateTime? ItemSubmissionDate
        { get; set; }

        [DataMember]
        public Nullable<Int32> FieldMaxLength { get; set; } // UAT-2701


        [DataMember]
        public Nullable<Int32> RequirementItemDisplayOrder { get; set; } // UAT-3078

        [DataMember]
        public Nullable<Int32> RequirementItemFieldDisplayOrder { get; set; } // UAT-3078
        #region UAT-3077
        [DataMember]
        public Boolean IsPaymentTypeItem { get; set; }

        [DataMember]
        public Boolean IsItemPaymentPaid { get; set; }

        [DataMember]
        public Decimal? ItemAmount { get; set; }

        [DataMember]
        public String ItemPaymentStatus { get; set; }

        [DataMember]
        public String ItemPaymentStatusCode { get; set; }

        [DataMember]
        public Decimal? PaidItemAmount { get; set; }

        #endregion
        //UAT 3106
        [DataMember]
        public String CategoryRuleStatusID { get; set; }

        [DataMember]
        public Nullable<Int32> RequirementAttributeGroupID { get; set; } // UAT-3176

        [DataMember]
        public List<RequirementItemURLContract> ListRequirementItemURLContract { get; set; } // UAT-3176




        [DataMember]
        public String ReqItemSampleDocFormURL { get; set; } // UAT-3309

        [DataMember]
        public Int32 AssignToUserID { get; set; }
        [DataMember]
        public DateTime? ComplianceRqdStartDate { get; set; }
        [DataMember]
        public DateTime? ComplianceRqdEndDate { get; set; }
        [DataMember]
        public Boolean isActualComplianceRequired { get; set; }
        [DataMember]
        public Boolean IsEditableByAdmin { get; set; }
        [DataMember]
        public Boolean IsEditableByApplicant { get; set; }
        [DataMember]
        public Boolean IsEditableByClientAdmin { get; set; }

        #region UAT 4380
        [DataMember]
        public Boolean IsFieldEditableByAdmin { get; set; }
        [DataMember]
        public Boolean IsFieldEditableByApplicant { get; set; }
        [DataMember]
        public Boolean IsFieldEditableByClientAdmin { get; set; }
        #endregion

        #region UAT-4254
        public List<RequirementCategoryDocUrl> lstReqCatDocUrls { get; set; }

        #endregion
    }
}
