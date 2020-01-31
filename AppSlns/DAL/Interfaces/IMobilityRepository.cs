using Entity;
using INTSOF.UI.Contract.Mobility;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DAL.Interfaces
{
    public interface IMobilityRepository
    {
        IQueryable<Entity.PkgMappingMaster> GetPkgMappingMasterData(Int32 pkgMappingMasterId);

        Boolean SaveComplianceItmMapping(List<Entity.ComplianceItmMappingDetail> cmplnceItemMappingDetailContractList);


        ObjectResult<Entity.GetComplianceItemMappingDetails_Result> GetMappedItemList(String complianceItemMappingXML);

        ObjectResult<Entity.GetComplianceItemMappingDetails_Result> GetSavedMappedItemDetails(Int32 packageMappingMasterID);

        Entity.ComplianceItmMappingDetail GetComplianceItemMappingDetails(Int32 tenantID, Int32 pkgMappingMasterID, Int32 itemID, Int32 attributeID);

        /// <summary>
        /// Saves the Institute Change Request for an applicant.
        /// </summary>
        /// <param name="institutionChangeRequest">Object of InstitutionChangeRequest including Organization User ID, Tenant ID, Request Note, Source Subscription ID, Source Node ID,Request Status ID, Is Deleted, Created On and Created By ID </param>
        /// <returns>True if Institute Change Request is saved</returns>
        void SaveInstituteChangeRequest(InstitutionChangeRequest institutionChangeRequest);

        #region Mapping Queue
        List<ApplicantTransitionMappingList> GetApplicantTransitionMappingList(CustomPagingArgsContract gridCustomPaging, String searchParameter);

        Boolean CheckIfMappingAlreadyExist(Int32 tenantID, Int32 SelectedSourcePackageId, Int32 SelectedTargetPackageId, Int32 SourceNodeId, Int32 TargetNodeId);

        Boolean SaveMapping(Entity.PkgMappingMaster pkgMappingMaster);
        Boolean HasNoReviewedInstance(Int32 pkgMappingMasterId);
        Boolean UpdateChanges(Int32 pkgMappingMasterId, Int32 currentLoggedInUserId);
        Entity.PkgMappingMaster GetPkgMappingDtail(Int32 pkgMappingMasterId);
        #endregion

        #region InstitutionChangeRequestQueue
        
        List<GetInstitutionChangeRequests> GetInstitutionChangeRequestData(MobilitySearchDataContract mobilitySerachItemDataContract, CustomPagingArgsContract customPagingArgsContract, String orderBy, String ordDirection);

        #endregion

        #region Institute Change Request Detail
 
        Entity.InstitutionChangeRequest GetInstitutionChangeRequestById(Int32 institutionChangeRequestId);

        Int16 GetInstitutionReqStatusIdByCode(String statusCode);

        Boolean SetInstnChangeReqStatus(Int32 institutionChangeRequestId, String statusCode, Int32 currentLoggedInUserId, Int32? newTenantId, String rejectnReason);
        #endregion

        #region Common Methods

        #region Add Record In Mapping Queue
        /// <summary>
        /// Method to add the record in package mapping table
        /// </summary>
        /// <param name="mappingData">XML String of mapping data</param>
        List<MappingRequest> AddInMappingQueue(String mappingData);

        /// <summary>
        /// Method to add the record in package mapping table
        /// </summary>
        /// <param name="mappingData">XML String of mapping data</param>
        List<MappingRequestData> AddRecordsInMappingQueue(String mappingData);
        #endregion

        #region Generate mapping instance and update in package subscription

        Int32? generateMappingInstance(Int32 mappingMasterId, Int32 currentLoggedInUserId);

        Int32 GetPossibleInstanceIdByCurrentInstanceId(Int32 currentMappingMasterId);

        #endregion

        /// <summary>
        /// get the the mapping record for the source and target packages
        /// </summary>
        /// <param name="sourcePackageId"></param>
        /// <param name="targetPackageId"></param>
        /// <param name="sourceTenantId"></param>
        /// <param name="targetTenantId"></param>
        /// <param name="sourceNodeId"></param>
        /// <param name="targetNodeId"></param>
        /// <returns></returns>
        PkgMappingMaster getMappingData(Int32 sourcePackageId, Int32 targetPackageId, Int32 sourceTenantId, Int32 targetTenantId, Int32 sourceNodeId, Int32? targetNodeId);

        Boolean checkIfMappingIsDefinedForAttribute(Int32 attributeId, Int32 tenantId);
        #endregion
    }
}
