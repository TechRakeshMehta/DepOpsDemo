using System;
using System.Collections.Generic;
using System.ServiceModel;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System.Data;

namespace INTSOF.ServiceInterface.Modules.ApplicantClinicalRotation
{
    [ServiceContract]
    public interface IApplicantClinicalRotation
    {
        [OperationContract]
        ServiceResponse<RequirementPackageSubscriptionContract, RequirementPackageContract> GetRequirementPackageSubscriptionData(ServiceRequest<Int32, Int32> data);

        [OperationContract]
        ServiceResponse<List<ClinicalRotationDetailContract>> GetApplicantRotaions(ServiceRequest<ClinicalRotationDetailContract> data);

        #region UAT-1316
        [OperationContract]
        ServiceResponse<RequirementItemContract> GetDataEntryRequirementItem(ServiceRequest<Int32, Int32> data);

        [OperationContract]
        ServiceResponse<List<ApplicantDocumentContract>> GetApplicantDocument(ServiceRequest<Int32> data);

        [OperationContract]
        ServiceResponse<ApplicantRequirementItemDataContract> GetApplicantRequirementItemData(ServiceRequest<ApplicantRequirementParameterContract, Int32> data);

        [OperationContract]
        ServiceResponse<Dictionary<Boolean, String>> SaveApplicantRequirementData(ServiceRequest<ApplicantRequirementParameterContract, Int32, Int32> data);

        [OperationContract]

        ServiceResponse<List<RequirementPackageSubscriptionStatusContract>> GetPackageSubscriptionCategoryStatus(ServiceRequest<Int32, String> data);

        [OperationContract]
        ServiceResponse<List<ApplicantDocumentContract>> SaveApplicantUploadDocument(ServiceRequest<List<ApplicantDocumentContract>, Int32, Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> DeleteAppRequirementItemFieldData(ServiceRequest<Int32, Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> IsDocumentAlreadyUploaded(ServiceRequest<String, Int32, Int32> data);

        [OperationContract]
        ServiceResponse<RequirementFieldVideoData> GetRequirementFieldVideoData(ServiceRequest<Int32> data);

        [OperationContract]
        ServiceResponse<RequirementObjectTreeContract> GetObjectTreeProperty(ServiceRequest<Int32> data);

        [OperationContract]
        ServiceResponse<List<RequirementObjectTreeContract>> GetAttributeObjectTreeProperties(ServiceRequest<ApplicantRequirementParameterContract, Int32> data);

        [OperationContract]
        ServiceResponse<ApplicantDocumentContract> GetClientSystemDocument(ServiceRequest<Int32> data);

        [OperationContract]
        ServiceResponse<ObjectAttributeContract> GetObjectTreeProperties(ServiceRequest<Int32> data);

        [OperationContract]
        ServiceResponse<ViewDocumentContract> GetViewDocumentData(ServiceRequest<Int32, Int32, Int32> data);

        #endregion

        #region Rule's Execution.
        [OperationContract]
        ServiceResponse<Boolean> ExecuteRequirementObjectBuisnessRules(ServiceRequest<List<RequirementRuleObject>, Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> EvaluateRequirementPostSubmitRules(ServiceRequest<List<RequirementRuleObject>, Int32> data);
        #endregion

        #region GET EXPLANATORY NOTES
        [OperationContract]
        ServiceResponse<String> GetExplanatoryNotes(ServiceRequest<Int32, String, String> data);
        #endregion

        //UAT-1523 Addition a notes box for each rotation for the student to input information
        [OperationContract]
        ServiceResponse<Boolean> UpdateRequirementPackageSubscriptionNotes(ServiceRequest<String, Int32, Int32> data);

        //UAT-2544 
        [OperationContract]
        ServiceResponse<Boolean> IsApplicantDropped(ServiceRequest<Int32, Int32, Int32> data);

        #region UAT-2975:
        [OperationContract]
        ServiceResponse<Boolean> SyncRequirementVerificationToFlatData(ServiceRequest<Int32> data);

        #endregion

        //UAT-3273
        [OperationContract]
        ServiceResponse<DataTable> GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(ServiceRequest<Int32, string> data);
        //UAT-4300
        [OperationContract]
        ServiceResponse<Int32> GetApplicantRequirementFieldData(ServiceRequest<Int32, Int32, String> data);

        //UAT-4254
        [OperationContract]
        ServiceResponse<List<RequirementCategoryDocUrl>> GetRequirementCatDocUrls(ServiceRequest<Int32, Int32> data);
    }
}
