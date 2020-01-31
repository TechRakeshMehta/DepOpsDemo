using System;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Core;
using INTSOF.Utils.Enums;
using INTSOF.Utils;
using INTSOF.ServiceInterface.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using System.Collections.Generic;
using System.Data;

namespace INTSOF.ServiceProxy.Modules.ApplicantOperations
{
    public class ApplicantClinicalRotationProxy : BaseServiceProxy<IApplicantClinicalRotation>
    {
        IApplicantClinicalRotation _applicantClinicalRotationServiceChannel;

        public ApplicantClinicalRotationProxy()
            : base(ServiceUrlEnum.ApplicantClinicalRotationSvcUrl.GetStringValue())
        {
            _applicantClinicalRotationServiceChannel = base.ServiceChannel;
        }

        public ServiceResponse<RequirementPackageSubscriptionContract, RequirementPackageContract> GetRequirementPackageSubscriptionData(ServiceRequest<Int32, Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.GetRequirementPackageSubscriptionData(data);
        }

        public ServiceResponse<List<ClinicalRotationDetailContract>> GetApplicantRotaions(ServiceRequest<ClinicalRotationDetailContract> data)
        {
            return _applicantClinicalRotationServiceChannel.GetApplicantRotaions(data);
        }

        #region UAT-1316
        public ServiceResponse<RequirementItemContract> GetDataEntryRequirementItem(ServiceRequest<Int32, Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.GetDataEntryRequirementItem(data);
        }

        public ServiceResponse<List<ApplicantDocumentContract>> GetApplicantDocument(ServiceRequest<Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.GetApplicantDocument(data);
        }

        public ServiceResponse<ApplicantRequirementItemDataContract> GetApplicantRequirementItemData(ServiceRequest<ApplicantRequirementParameterContract, Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.GetApplicantRequirementItemData(data);
        }

        public ServiceResponse<Dictionary<Boolean, String>> SaveApplicantRequirementData(ServiceRequest<ApplicantRequirementParameterContract, Int32, Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.SaveApplicantRequirementData(data);
        }

        public ServiceResponse<List<RequirementPackageSubscriptionStatusContract>> GetPackageSubscriptionCategoryStatus(ServiceRequest<Int32, String> data)
        {
            return _applicantClinicalRotationServiceChannel.GetPackageSubscriptionCategoryStatus(data);
        }

        public ServiceResponse<List<ApplicantDocumentContract>> SaveApplicantUploadDocument(ServiceRequest<List<ApplicantDocumentContract>, Int32, Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.SaveApplicantUploadDocument(data);
        }
        public ServiceResponse<Boolean> DeleteAppRequirementItemFieldData(ServiceRequest<Int32, Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.DeleteAppRequirementItemFieldData(data);
        }

        public ServiceResponse<Boolean> IsDocumentAlreadyUploaded(ServiceRequest<String, Int32, Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.IsDocumentAlreadyUploaded(data);
        }

        public ServiceResponse<RequirementFieldVideoData> GetRequirementFieldVideoData(ServiceRequest<Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.GetRequirementFieldVideoData(data);
        }

        public ServiceResponse<RequirementObjectTreeContract> GetObjectTreeProperty(ServiceRequest<Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.GetObjectTreeProperty(data);
        }
        public ServiceResponse<List<RequirementObjectTreeContract>> GetAttributeObjectTreeProperties(ServiceRequest<ApplicantRequirementParameterContract, Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.GetAttributeObjectTreeProperties(data);
        }

        public ServiceResponse<ApplicantDocumentContract> GetClientSystemDocument(ServiceRequest<Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.GetClientSystemDocument(data);
        }

        public ServiceResponse<ObjectAttributeContract> GetObjectTreeProperties(ServiceRequest<Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.GetObjectTreeProperties(data);
        }

        public ServiceResponse<ViewDocumentContract> GetViewDocumentData(ServiceRequest<Int32, Int32, Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.GetViewDocumentData(data);
        }

        #endregion

        #region Rule's Execution.
        public ServiceResponse<Boolean> ExecuteRequirementObjectBuisnessRules(ServiceRequest<List<RequirementRuleObject>, Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.ExecuteRequirementObjectBuisnessRules(data);
        }

        public ServiceResponse<Boolean> EvaluateRequirementPostSubmitRules(ServiceRequest<List<RequirementRuleObject>, Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.EvaluateRequirementPostSubmitRules(data);
        }
        #endregion

        #region GET EXPLANATORY NOTES
        public ServiceResponse<String> GetExplanatoryNotes(ServiceRequest<Int32, String, String> data)
        {
            return _applicantClinicalRotationServiceChannel.GetExplanatoryNotes(data);
        }
        #endregion

        //UAT-1523 Addition a notes box for each rotation for the student to input information
        public ServiceResponse<Boolean> UpdateRequirementPackageSubscriptionNotes(ServiceRequest<String, Int32, Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.UpdateRequirementPackageSubscriptionNotes(data);
        }

        //UAT-2544 
        public ServiceResponse<Boolean> IsApplicantDropped(ServiceRequest<Int32, Int32, Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.IsApplicantDropped(data);
        }

        #region UAT-2975:
        public ServiceResponse<Boolean> SyncRequirementVerificationToFlatData(ServiceRequest<Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.SyncRequirementVerificationToFlatData(data);
        }
        #endregion

        #region UAT-3273

        public ServiceResponse<DataTable> GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(ServiceRequest<Int32, string> data)
        {
            return _applicantClinicalRotationServiceChannel.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(data);
        }
        #endregion

        public ServiceResponse<Int32> GetApplicantRequirementFieldData(ServiceRequest<Int32, Int32, String> data)
        {
            return _applicantClinicalRotationServiceChannel.GetApplicantRequirementFieldData(data);
        }

        #region UAT-4254


        public ServiceResponse<List<RequirementCategoryDocUrl>> GetRequirementCatDocUrls(ServiceRequest<Int32, Int32> data)
        {
            return _applicantClinicalRotationServiceChannel.GetRequirementCatDocUrls(data);
        }
        #endregion
    }
}
