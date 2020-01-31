using System;
using System.Collections.Generic;
using System.Data;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace DAL.Interfaces
{
    public interface IRequirementVerificationRepository
    {
        #region Requirement Verification Queue

        List<RequirementVerificationQueueContract> GetRequirementVerificationQueueSearch(RequirementVerificationQueueContract searchDataContract, CustomPagingArgsContract customPagingArgsContract);

        #endregion

        #region Requirement Verification Details

        /// <summary>
        /// Get the Requirement Verification Details Screen data, including the data entered by Applicant.
        /// </summary>
        /// <param name="reqPkgSubId"></param>
        /// <returns></returns>
        List<RequirementVerificationDetailContract> GetVerificationDetailData(Int32 reqPkgSubId);

        /// <summary>
        /// Save/Update the data of the Verification Details screen.
        /// </summary>
        /// <param name="reqVerificationDataToSave"></param>
        /// <param name="lkpReqCatStatus"></param>
        /// <param name="lkpReqItemStatus"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Dictionary<Int32, String> SaveVerificationData(RequirementVerificationData reqVerificationDataToSave, List<lkpRequirementCategoryStatu> lkpReqCatStatus,
                                     List<lkpRequirementItemStatu> lkpReqItemStatus, Int32 currentUserId, List<lkpObjectType> lkpObjectType, ref Boolean isNewPackage);


        Dictionary<Boolean, String> ValidateDocumentRules(RequirementVerificationCategoryData applicantReqCatData, List<lkpObjectType> lstObjectTypes
                                                          , Int32 packageSubscriptionId);
        /// <summary>
        /// UAT 1626- Get the Requirement Verification Details by Category, including the data entered by Applicant.
        /// </summary>
        /// <param name="reqPkgSubId"></param>
        /// <returns></returns>
        List<RequirementVerificationDetailContract> GetRequirementItemsByCategoryId(Int32 reqPkgSubId, List<Int32> reqCatId, Int32 rotationId);

        List<ApplicantFieldDocumentMappingContract> GetRequirementApplicantDocumentsByCategoryId(Int32 reqPkgSubId, Int32 reqCatId);

        #endregion

        #region UAT-1626 : update rotation package verification to mimic tracking verification
        /// <summary>
        /// Get Package and Category Details for Compliance Verification detail screen.
        /// </summary>
        /// <param name="ReqPkgSubscriptionID"></param>
        /// <returns></returns>
        List<RequirementVerificationDetailContract> GetRequirementPackageCategoryData(Int32 ReqPkgSubscriptionID, Int32 rotationId);
        List<ReqPkgSubscriptionIDList> GetReqPkgSubscriptionIdListForRotationVerification(RequirementVerificationQueueContract requirementVerificationQueueContract, Int32 CurrentReqPkgSubscriptionID, Int32 ApplicantRequirementItemID);
        #endregion

        Tuple<List<Int32>,String, Dictionary<Boolean, String>> RotationSubscriptionApproveAllPendingItems(Int32 reqPkgSubsId, Int32 currentLoogedInUserId,Boolean isAdmin, ref Int32 affectedItemsCount);

        SystemEntityUserPermission GetSystemEntityUserPermission(Int32 clientOrganisationUserID);

        Boolean IsNewRequirementPackage(Int32 reqPkgId);

        #region UAT-2975: ADB Admin All Client Rotation Assignment and User work Queues.
        Boolean SyncRequirementVerificationToFlatData(String packageSubscriptionIds, Int32 currentUserId);
        #endregion

        List<RequirementItemRejectionContract> GetRequirementRejectedItemDetailsForMail(String rejectedItemIds);
    }
}
