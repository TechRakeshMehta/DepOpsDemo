using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public interface IRotationRequirementDataEntryView
    {
        Int32 ClinicalRotationID
        {
            get;
            set;
        }
        //UAT-2040
        String ClinicalRotationIDs
        {
            get;
            set;
        }

        Int32 RequirementPackageSubscriptionID
        {
            get;
            set;
        }

        ClinicalRotationDetailContract ClinicalRotationDetails
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }

        RequirementPackageContract RotationPackageDetail
        {
            get;
            set;
        }

        RequirementPackageSubscriptionContract RotationSubscriptionDetail
        {
            get;
            set;
        }

        Int32 RequirementCategoryDataId
        {
            get;
            set;
        }

        Int32 RequirementCategoryId
        {
            get;
            set;
        }


        Int32 RequirementItemDataId
        {
            get;
            set;
        }

        Int32 RequirementItemId
        {
            get;
            set;
        }

        Int32 RequirementPackageId
        {
            get;
            set;
        }

        List<RequirementItemContract> lstAvailableItems
        {
            get;
            set;
        }
        Boolean IsUIValidationApplicable { get; set; }
        Dictionary<Int32, String> SavedApplicantDocuments { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Boolean IsClinicalRotationExpired { get; set; }
        Int32 OrganiztionUserID { get; set; }
        String RequirementCategoryName { get; set; }
        //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes).
        String RequirementCategoryURL { get; set; }
        //UAT-3161:-Make "More information" category field label editable by category.
        String RequirementCategoryURLLabel { get; set; }
        Int32 OrgUsrID { get; }

        #region Data Contract Properties
        ApplicantRequirementItemDataContract RequirementItemDataContract { get; set; }
        ApplicantRequirementCategoryDataContract RequirementCategoryDataContract { get; set; }
        List<ApplicantRequirementFieldDataContract> ApplicantFieldDataContractList { get; set; }
        Dictionary<Int32, Int32> FieldDocuments { get; set; }
        String UIValidationErrors { get; set; }

        List<ApplicantDocumentContract> ToSaveApplicantUploadedDocuments { get; set; }

        Boolean IsAppDataSavedSuccessfully { get; set; }

        Dictionary<Int32, ApplicantDocumentContract> AppSignedDocumentDic { get; set; }
        #endregion

        //UAT-2040
        List<ClinicalRotationDetailContract> lstclinicalRotationDetailContract
        {
            get;
            set;
        }
        Boolean IsDisplayMultipleRotationDetails
        {
            get;
            set;
        }

        #region UAT-2165:Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
        Dictionary<Int32, Boolean> LstComplianceRqdCategoryMapping
        {
            get;
            set;
        }
        #endregion

        //Changes related to bud ID:15048
        String ItemPreviousStatsCode { get; set; }

        Int32 ApplicantID
        {
            get;
            set;
        }

        Boolean IsOptionalCategoryClientSettingEnabled { get; set; }  //UAT 3106

        String QuizConfigSetting { get; set; } //UAT 3299 
        List<RequirementExpiringItemListContract> lstRequirementExpiringItem { get; set; }
        List<Int32> ExpiringReqItemList { get; set; }
        List<Int32> ViewDocumentFieldDocumentList { get; set; }
        //UAT-3737
        Boolean IsIntructorPreceptorPkg { get; set; }
        List<RequirementItemPaymentContract> ItemPaymentList { get; set; }

        RequirementItemContract SelectedItemDetails { get; set; }
        Boolean IsAutoSubmit { get; set; }

        List<RequirementCategoryDocUrl> lstReqCatDocUrls { get; set; }//UAt-4254

        //Start UAT-5062
        Boolean IsUploadDocUpdated { get; set; }
        //End UAT-5062
    }
}
