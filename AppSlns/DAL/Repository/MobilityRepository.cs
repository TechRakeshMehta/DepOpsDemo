#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  MobilityRepository.cs
// Purpose:
//

#endregion

using DAL.Interfaces;
using Entity;
using INTSOF.UI.Contract.Mobility;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
namespace DAL.Repository
{
    public class MobilityRepository : BaseRepository, IMobilityRepository
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables
        private SysXAppDBEntities _dbNavigation;
        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public MobilityRepository()
        {
            _dbNavigation = base.Context;
        }

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        IQueryable<Entity.PkgMappingMaster> IMobilityRepository.GetPkgMappingMasterData(Int32 pkgMappingMasterID)
        {
            return _dbNavigation.PkgMappingMasters.Where(cond => cond.PMM_ID == pkgMappingMasterID);
        }

        Boolean IMobilityRepository.SaveComplianceItmMapping(List<Entity.ComplianceItmMappingDetail> cmplnceItmMapngList)
        {
            foreach (var cmplnceItmMapng in cmplnceItmMapngList)
            {
                if (cmplnceItmMapng.CIMD_ID == AppConsts.NONE)
                {
                    base.AddObjectEntityInTransaction(cmplnceItmMapng);
                }
            }
            _dbNavigation.SaveChanges();
            return true;
        }

        

        ObjectResult<Entity.GetComplianceItemMappingDetails_Result> IMobilityRepository.GetMappedItemList(String complianceItemMappingXML)
        {
            return _dbNavigation.usp_GetComplianceItemMappingDetails(complianceItemMappingXML);
        }

        ObjectResult<Entity.GetComplianceItemMappingDetails_Result> IMobilityRepository.GetSavedMappedItemDetails(Int32 packageMappingMasterID)
        {
            return _dbNavigation.usp_GetSavedComplianceItemMappingDetails(packageMappingMasterID);
        }




        /// <summary>
        /// Saves the Institute Change Request for an applicant.
        /// </summary>
        /// <param name="institutionChangeRequest">Object of InstitutionChangeRequest including Organization User ID, Tenant ID, Request Note, Source Subscription ID, Source Node ID,Request Status ID, Is Deleted, Created On and Created By ID </param>
        /// <returns>True if Institute Change Request is saved</returns>
        void IMobilityRepository.SaveInstituteChangeRequest(InstitutionChangeRequest institutionChangeRequest)
        {
            _dbNavigation.AddToInstitutionChangeRequests(institutionChangeRequest);
            _dbNavigation.SaveChanges();
        }

        Entity.ComplianceItmMappingDetail IMobilityRepository.GetComplianceItemMappingDetails(Int32 tenantID, Int32 pkgMappingMasterID, Int32 itemID, Int32 attributeID)
        {
            return _dbNavigation.ComplianceItmMappingDetails.Where(cond => cond.CIMD_TenantID == tenantID
                                          && cond.CIMD_ItemID == itemID && cond.CIMD_AttributeID == attributeID
                                          && cond.CIMD_MappingMasterID == pkgMappingMasterID).FirstOrDefault();
        }

        #region Mobility Mapping Queue

        List<ApplicantTransitionMappingList> IMobilityRepository.GetApplicantTransitionMappingList(CustomPagingArgsContract gridCustomPaging, String searchParameter)
        {
            string orderBy = null;
            string ordDirection = null;
            ordDirection = gridCustomPaging.SortDirectionDescending == false ? null : "desc";
            List<ApplicantTransitionMappingList> lstApplicantTransitionMappingList = _dbNavigation.GetApplicantTransitionMappingList(orderBy, ordDirection, gridCustomPaging.CurrentPageIndex, gridCustomPaging.PageSize, searchParameter).ToList();
            return lstApplicantTransitionMappingList;
        }

        /// <summary>
        /// To check if mapping already exist
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="SelectedSourcePackageId"></param>
        /// <param name="SelectedTargetPackageId"></param>
        /// <param name="SourceNodeId"></param>
        /// <param name="TargetNodeId"></param>
        /// <returns></returns>
        Boolean IMobilityRepository.CheckIfMappingAlreadyExist(Int32 tenantID, Int32 SelectedSourcePackageId, Int32 SelectedTargetPackageId, Int32 SourceNodeId, Int32 TargetNodeId)
        {
            //Added tenant id check to resolve issue related to Mapping Duplicate Check
            return _dbNavigation.PkgMappingMasters.Any(cond => cond.PMM_FromPackageID == SelectedSourcePackageId && cond.PMM_ToPackageID == SelectedTargetPackageId
                     /*UAT-2738:Update Package to package mapping to display all possible tracking packages 
                      * (look up and show all on parent nodes) to allow for when new package is added to child node.
                      * Commented below code.
                      * && cond.PMM_FromNodeID == SourceNodeId && cond.PMM_ToNodeID == TargetNodeId*/
                    && cond.PMM_FromTenantID == tenantID && cond.PMM_IsDeleted == false);
        }

        Boolean IMobilityRepository.SaveMapping(Entity.PkgMappingMaster pkgMappingMaster)
        {
            _dbNavigation.PkgMappingMasters.AddObject(pkgMappingMaster);
            if (_dbNavigation.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        Boolean IMobilityRepository.HasNoReviewedInstance(Int32 pkgMappingMasterId)
        {
            return !_dbNavigation.PkgMappingMasters.Where(x => x.PMM_ParentID == pkgMappingMasterId && !x.PMM_IsDeleted).Any();
        }

        Boolean IMobilityRepository.UpdateChanges(Int32 pkgMappingMasterId, Int32 currentLoggedInUserId)
        {
            Entity.PkgMappingMaster pkgMasterDetails = _dbNavigation.PkgMappingMasters.Where(x => x.PMM_ID == pkgMappingMasterId).FirstOrDefault();
            pkgMasterDetails.PMM_IsDeleted = true;
            pkgMasterDetails.PMM_ModifiedByID = currentLoggedInUserId;
            pkgMasterDetails.PMM_ModifiedOn = DateTime.Now;
            return (_dbNavigation.SaveChanges() > AppConsts.NONE);
        }
        Entity.PkgMappingMaster IMobilityRepository.GetPkgMappingDtail(Int32 pkgMappingMasterId)
        {
            return _dbNavigation.PkgMappingMasters.Where(x => x.PMM_ID == pkgMappingMasterId).FirstOrDefault();

        }

        #endregion

        #region Institution Change request



        List<GetInstitutionChangeRequests> IMobilityRepository.GetInstitutionChangeRequestData(MobilitySearchDataContract mobilitySerachItemDataContract, CustomPagingArgsContract customPagingArgsContract, String orderBy, String ordDirection)
        {

            return _dbNavigation.GetInstitutionChangeRequests(mobilitySerachItemDataContract.ApplicantFirstName, mobilitySerachItemDataContract.ApplicantLastName, mobilitySerachItemDataContract.SourceTenantId, mobilitySerachItemDataContract.TargetTenantId
                                                              , mobilitySerachItemDataContract.Status, orderBy, ordDirection, customPagingArgsContract.CurrentPageIndex, customPagingArgsContract.PageSize).ToList();
        }

        #endregion

        #region Institute Change Request Detail

        public Entity.InstitutionChangeRequest GetInstitutionChangeRequestById(Int32 institutionChangeRequestId)
        {
            return _dbNavigation.InstitutionChangeRequests.FirstOrDefault(icr => icr.ICR_ID == institutionChangeRequestId && icr.ICR_IsDeleted == false);
        }

        public Int16 GetInstitutionReqStatusIdByCode(String statusCode)
        {
            return _dbNavigation.lkpInstChangeRequestStatus.FirstOrDefault(obj => obj.Code.Equals(statusCode) && obj.IsDeleted == false).InstChangeRequestStatusID;
        }

        public Boolean SetInstnChangeReqStatus(Int32 institutionChangeRequestId, String statusCode, Int32 currentLoggedInUserId, Int32? newTenantId, String rejectnReason)
        {
            Entity.InstitutionChangeRequest institutionChangeRequest = GetInstitutionChangeRequestById(institutionChangeRequestId);
            institutionChangeRequest.ICR_RejectionReason = rejectnReason;
            institutionChangeRequest.ICR_RequestStatusID = GetInstitutionReqStatusIdByCode(statusCode);
            institutionChangeRequest.ICR_NextTenantID = newTenantId;
            institutionChangeRequest.ICR_ModifiedByID = currentLoggedInUserId;
            institutionChangeRequest.ICR_ModifiedOn = DateTime.Now;
            _dbNavigation.SaveChanges();
            return true;
        }

        #endregion

        #region Common Methods
        #region Add Record In Mapping Queue

        /// <summary>
        /// Method to add the record in package mapping table and return mapping id
        /// </summary>
        /// <param name="mappingData">XML String of mapping data</param>
        /// <returns>MappingId</returns>
        public List<MappingRequest> AddInMappingQueue(String mappingData)
        {
            return _dbNavigation.Insert_In_MappingQueue(mappingData).ToList();
        }

        /// <summary>
        /// Method to add the record in package mapping table and return mapping id
        /// </summary>
        /// <param name="mappingData">XML String of mapping data</param>
        /// <returns>MappingId</returns>
        public List<MappingRequestData> AddRecordsInMappingQueue(String mappingData)
        {
            return _dbNavigation.usp_InsertInMappingQueue(mappingData).ToList();
        }

        #endregion

        #region Generate mapping instance and update in package subscription
        public Int32? generateMappingInstance(Int32 mappingMasterId, Int32 currentLoggedInUserId)
        {
            return _dbNavigation.usp_CreateCmplanceItmMappingInstance(mappingMasterId, currentLoggedInUserId).FirstOrDefault();
        }

        public Int32 GetPossibleInstanceIdByCurrentInstanceId(Int32 currentMappingMasterId)
        {
            List<PkgMappingMaster> lstAllInstances = _dbNavigation.PkgMappingMasters.Where(
                        pmm => (pmm.PMM_ParentID == currentMappingMasterId || pmm.PMM_ID == currentMappingMasterId)
                        && pmm.PMM_IsDeleted == false).ToList();

            PkgMappingMaster _masterInstance = lstAllInstances.Where(pmm => pmm.PMM_ID == currentMappingMasterId).FirstOrDefault();

            PkgMappingMaster _childInstance = lstAllInstances.Where(
                        pmm => pmm.PMM_ParentID == currentMappingMasterId
                        && pmm.PMM_IsDeleted == false
                        && pmm.lkpPkgMappingStatu.PMS_Code == PkgMappingStatus.Reviewed_instance.GetStringValue()
                        && pmm.PMM_ReviewDate >= _masterInstance.PMM_ReviewDate)       
                        .FirstOrDefault();

            if (_childInstance.IsNullOrEmpty())
                return AppConsts.NONE;

            return _childInstance.PMM_ID;
        }

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
        public PkgMappingMaster getMappingData(Int32 sourcePackageId, Int32 targetPackageId, Int32 sourceTenantId, Int32 targetTenantId, Int32 sourceNodeId, Int32? targetNodeId)
        {
            PkgMappingMaster _pkgMappingMaster = _dbNavigation.PkgMappingMasters.Where
                 (pmm => pmm.PMM_FromPackageID == sourcePackageId && pmm.PMM_ToPackageID == targetPackageId
                     && pmm.PMM_FromTenantID == sourceTenantId && pmm.PMM_ToTenantID == targetTenantId
                     /*UAT-2738:Update Package to package mapping to display all possible tracking packages 
                      * (look up and show all on parent nodes) to allow for when new package is added to child node.
                      * Commented below code.
                      * && pmm.PMM_FromNodeID == sourceNodeId && pmm.PMM_ToNodeID == targetNodeId
                      */
                     && pmm.PMM_IsDeleted == false).FirstOrDefault();
            return _pkgMappingMaster;

        }

        public Boolean checkIfMappingIsDefinedForAttribute(Int32 attributeId, Int32 tenantId)
        {
            return _dbNavigation.ComplianceItmMappingDetails.Any(x => x.CIMD_AttributeID == attributeId && x.CIMD_TenantID == tenantId
                                                         && x.PkgMappingMaster.PMM_IsDeleted == false && x.CIMD_IsDeleted == false);
        }

        #endregion
        #endregion


        #region Private Methods

        #endregion

        #endregion

        #region Compiled Queries

        #endregion
    }
}
