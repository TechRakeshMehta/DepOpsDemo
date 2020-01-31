using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.UI.Contract.RotationPackages;
using System.Data;
using INTSOF.UI.Contract.ComplianceOperation;

namespace DAL.Interfaces
{
    public interface IApplicantRequirementRepository
    {
        RequirementPackageSubscription GetRequirementPackageSubscription(Int32 requirementPackageSubscriptionID);
        RequirementPackage GetRequirementPackageDetail(Int32 rotationRequirementPackageID);

        #region UAT-1316
        RequirementItem GetDataEntryRequirementItem(Int32 requirementItemId);
        List<ApplicantDocument> GetApplicantDocument(Int32 currentLoggedInUserId, List<Int32?> lstDocumentType);
        ApplicantRequirementItemData GetApplicantRequirementItemData(Int32 reqPkgSubscriptionId, Int32 reqItemId, Int32 reqCategoryId, Int32 currentLoggedInUserId);
        RequirementFieldVideo GetRequirementFieldVideoData(Int32 rfVideoId);
        RequirementObjectTreeProperty GetObjectTreeProperty(Int32 rotId, Int32 lkpAttributeId);
        ClientSystemDocument GetClientSystemDocument(Int32 clientSysDocId);
        List<RequirementObjectTreeProperty> GetObjectTreeProperties(Int32 rotId);
        ViewDocumentContract GetViewDocumentData(Int32 applicantDocId, Int32 clientSysDocId, Int32 organizationUserId, Int32 reqFieldId, string signCode);
        #region Save Applicant Requirement Data
        Dictionary<Boolean, String> SaveApplicantRequirementData(ApplicantRequirementCategoryData applicantReqCategoryData, ApplicantRequirementItemData applicantReqItemData,
                                            List<ApplicantRequirementFieldData> lstApplicantFieldData, Int32 createdModifiedById, Dictionary<Int32, Int32> fieldDocuments,
                                            Int32 compliancePackageId, Int32 packageSubscriptionId, List<lkpObjectType> lstObjectTypes
                                            , Dictionary<Int32, ApplicantDocument> signedAppDocuments, Int32 orgUsrID, Boolean isNewPackage, Boolean IsUploadDocUpdated);

        DataTable GetPackageSubscriptionCategoryStatus(String requirementPackageSubscriptionIDs);

        List<ApplicantDocument> SaveApplicantUploadDocument(List<ApplicantDocument> appUploadedDocumentList);

        Boolean DeleteAppRequirementItemFieldData(Int32 applicantReqItemDataId, Int32 currentUserId);

        Boolean IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, Int32 organizationUserId, List<lkpDocumentType> docType);
        #endregion
        List<RequirementObjectTreeContract> GetAttributeObjectTreeProperties(Int32 reqPackageId, Int32 reqItemId, Int32 reqCategoryId, Int32 currentLoggedInUserId);
        #endregion

        #region Rule's Execution.
        void ExecuteRequirementObjectBuisnessRules(String ruleObjectXML, Int32 reqSubscriptionId, Int32 systemUserId);

        #endregion

        #region GET EXPLANATORY NOTES
        String GetExplanatoryNotes(Int32 objectId, Int32 objectTypeId, Int32 contentTypeId);
        #endregion

        //UAT-1523: Addition a notes box for each rotation for the student to input information
        Boolean SaveChanges();

        #region UAT-2224: Admin access to upload/associate documents on rotation package items.

        ApplicantRequirementItemData GetApplicantRequirementItemDataByID(Int32 applicantRequirementItemDataID);
        Boolean AddUpdateApplicantRequirementDocumentMappingData(List<ApplicantDocumentContract> applicantUploadedDocuments, Int32 applicantRequirementItemDataId, Int32 requirementFieldId, Int32 currentUserId);
        Boolean AddIncompleteApplicantRequirementDocumentMappingData(List<ApplicantDocumentContract> applicantUploadedDocuments, ApplicantRequirementCategoryData categoryData,
            ApplicantRequirementItemData itemData, ApplicantRequirementFieldData fieldData, Int32 requirementPackageSubscriptionId, Int32 applicantId, Int32 currentUserId, out Int32 itemDataId);
        Boolean AssignUnAssignRequirementItemDocuments(List<ApplicantDocumentContract> toAddDocumentMapList, List<Int32> toDeleteApplicantRequirementDocumentMapIDs, Int32 requirementItemDataId, Int32 currentUserId);
        Boolean AssignUnAssignIncompleteRequirementItemDocuments(List<ApplicantDocumentContract> toAddDocumentMapList, List<Int32> toDeleteApplicantRequirementDocumentMapIDs, ApplicantRequirementCategoryData categoryData,
            ApplicantRequirementItemData itemData, ApplicantRequirementFieldData fieldData, Int32 requirementPackageSubscriptionId, Int32 applicantId, Int32 currentUserId, out Int32 itemDataId);
        Boolean RemoveMapping(Int32 applicantRequirementDocumentMapId, Int32 currentUserId);

        #endregion

        Boolean CanDeleteRqmtFieldUploadDoc(Int32 applicantUploadedDocumentID);

        //UAT-2905
        DataTable GetMailDataForItemSubmitted(String rpsIds);

        List<RequirementItemField> GetRequirementFieldList(Int32 reqItemId);

        Dictionary<Boolean, String> ValidateDynamicUiRules(Int32 organizationUserId, Int32 reqPackageId, List<ApplicantRequirementFieldData> lstApplicantData
                                                            , Int32 complianceItemId, Int32 complianceCategoryId, Int32 packageSubscriptionId
                                                            , Boolean isDataEntryForm, List<lkpObjectType> lstObjectTypes);

        List<Int32> FilterRequirementDataItemsByStatusCode(string dataIds, string statusCode);

        Boolean SaveBadgeRequestFormData(List<BadgeFormNotificationDataContract> lstBadgeFormNotificationDataContract, Int32 compliancePackageTypeID, Int32 requirementPackageTypeID, string compItemCode, string reqItemCode, Int32 itemApprovalDateBagdeFormFieldTypeID, Int32 currentOrgUserID);


        //List<RequirementPackageSubscriptionApprovedContract> 
        DataTable GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(String requirementPackageSubscriptionIDs);

        String GetRequirementPackageSubscriptionIdsByPackageID(String requirementPackageId);

        //UAt-4015
        List<RequirementPackageSubscriptionStatusContract> GetInstPrecepRPSData(Int32 rpsIds);

        Int32 GetApplicantRequirementFieldData(Int32 RequirementItemDataID, Int32 RequirementFieldID);

        //UAT-4254
        List<RequirementCategoryDocLink> GetRequirementCatDocUrls(Int32 requirementCategoryId);
    }
}
