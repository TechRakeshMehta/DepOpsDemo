using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.Mobility;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

namespace DAL.Interfaces
{
    public interface IClientMobilityRepository
    {
        ObjectResult<GetRuleSetTree> GetRuleSetTreeForPackage(String packageID);

        Boolean UpdateOrderSkippedMapping(Entity.PkgMappingMaster pkgMappingMaster);

        String GetNodesDetails(Int32 nodeID);

        #region Institution Change Request Queue
        

        ObjectResult<GetPreviousValuesForSubscription> GetPreviousDataForSubscription(Int32 subscriptionnId);
       // ObjectResult<GetTargetMobilityNodes> GetTargetMobilityNodes(Int32 subscriptionnId);
        ObjectResult<GetSourceNodeDeatils> GetSourceNodeDeatils(Int32 sourceNodeId, Int32 sourcePackageId);
        String GetTargetNodeHierarchyLabel(Int32 departmentProgramMappingId);
        List<PackageSubscription> GetSourceSubscriptionDetails(List<Int32> sourceSubscriptionList);
       // List<usp_SubscriptionChange_Result> CreateNewSubscriptionForMobilityNode(String xml);
        #endregion

        

        #region Institute Hierarchy Mobility

        Boolean CreateMobilityInstance(Int32 backgroundProcessUserId, Int32 chunkSize);
        MobilityInstance GetNodeMobilityInstance(Int32 dpmID);
        InstHierarchyMobility GetInstHierarchyMobility(Int32 deptProgramMappingID);
        Boolean SaveMobilityData(Int32 deptProgramMappingID, DateTime firstStartDate, Int16 durationTypeID, Int32 duration, Int32? instanceInterval, Int32? successorNodeID, 
                                 List<MobilityPackageRelation> listMobilityPackageRelation, Int32 currentUserId);
        Boolean DeleteMobilityData(Int32 deptProgramMappingID, Int32 currentUserId);
        Boolean InsertNodeTranistionQueue(Int32 backgroundProcessUserId, Int32 daysDueBeforeTransition);

        #endregion

        #region Applicant Node Transition Status

        List<ApplicantTransitionStatus> GetApplicantNodeTransitionStatus(CustomPagingArgsContract gridCustomPaging, MobilitySearchDataContract mobilitySearchData);
        Boolean UpdateNodeTransitionStatus(List<Int32> mobilityNodeTransitionIds, Int32 currentLoggedInUserId, Int16 approvalStatusID);
        
        
        #endregion

        #region Institute Change Request Detail

        /// <summary>
        /// Delete the applicant orders placed in the current institution.
        /// </summary>
        /// <param name="organisationUserId">Organisation User Id</param>
        /// <param name="currentloggedInUserId">Current Logged In User Id</param>
        /// <returns>OrganizationUser Object</returns>
        Address DeleteAppicantOrders(Int32 organisationUserId, Int32 currentloggedInUserId);

        /// <summary>
        /// Creates applicant account in the new institution.
        /// </summary>
        /// <param name="organizationUser">OrganizationUser Entity</param>
        /// <param name="currentloggedInUserId">Current Logged In User Id</param>
        void CreateApplicantAccount(Entity.OrganizationUser organizationUser, Address address, Int32 currentloggedInUserId);

        PackageSubscription GetPackageSubscriptionById(Int32 packageSubscriptionId, Boolean checkDeletedSubscriptions = true);

        DeptProgramMapping GetDeptProgramMappingById(Int32 deptProgramMappingId);
        #endregion

        #region Applicant Balance Payment

        /// <summary>
        /// Get the Order for an Applicant whose Balance is due.
        /// </summary>
        /// <param name="applicantID">ID of applicant.</param>
        /// <returns>Order</returns>
        Order GetApplicantBalanceDueOrder(Int32 applicantID);  
        
        Order GetApplicantBalanceDuePreviousOrder(Int32 applicantID, Int32? orderID);

        Boolean IsOrderPaymtDueAndChangeByAdmin(Int32 orderID);
        #endregion

       
        #region "Copy Package Data"
       /// <summary>
        /// This stored procedure will get the list of Source and Target Subscription  IDs, and 
        /// </summary>
        /// <param name="subscriptionID"></param>
        /// <returns>List of Successfully transferred IDs(Target Subscription IDs)</returns>
        List<PackageSubscriptionList> CopyPackageData(List<SourceTargetSubscriptionList> subscriptionID, Int32 currentLoggedInUserID);
        #endregion

        #region "Rollback Subscriptions"
        Int32 RollbackSubscriptions(Int32 LoginUserID, List<Int32> subscriptionID);
        #endregion

        #region update mapping instance  package subscription
        void UpdateMappingInstanceforPendingSubscription(Int32 mappingMasterId, Int32 mappingInstanceId,Int32 currentLoggedInUserId);
        
        #endregion

        #region "Get List of Active Subscriptions"
        List<AdminChangeSubscription> GetAdminChangeSubscriptionList(SearchItemDataContract searchDataContract, CustomPagingArgsContract gridCustomPaging);
        #endregion

        List<ActiveSubscriptionsForRollback> GetActiveSubscriptionsForRollback(string _applicantFirstName, string _applicantLastName, Int32? _userGroupID, Int32? _sourceNodeID, Int32? _targetNodeID, DateTime? _fromDate, DateTime? _toDate, CustomPagingArgsContract gridCustomPaging);

        List<SourceTargetSubscriptionList> GetSourceTargetSubscriptionList(Int32 chunkSize);
        #region Pkg Mapping Queue
        List<Entity.ClientEntity.CompliancePkgMappingDependency> GetPkgMappingDependencyList(CustomPagingArgsContract gridCustomPaging, Int32 packageMappingMasterId);

        Boolean IsSubscriptionExist(Int32 pkgMappingMasterId);
        #endregion

        List<ApplicantsNodeTransitions> GetApplicantsNodeTransitionsDue(Int32 chunkSize);

        List<AutomaticChangedSubscriptions> AutomaticChangeSubscription(String sourceXML);

        #region UAT-1395:Change Subscription/Data sync bugs found by QA
        //Insert Subscription Detail in Data Sunc History
        void SaveDataSyncHistory(String subscriptionXml, Int32 currentLoggedInUSerID, Int32 tenantId);
        List<Int32> GetPackageSubscriptionIDForChangeSub(List<Int32> pkgSubscriptionIds, Int32 ordChangeReqTypeID);
        
        #endregion

        #region UAT-1476:WB: When a tracking package is ordered and there was already a previous package with entered data,
        //then there would be data movement as if there were a subscription change.
        CompliancePackageCopyDataMapping GetCompliancePackageCopyDataMapping(Int32 tenantId,Int32 targetPackageId,Int32 currentLoggedInUserId,Int32 SelectedNodeId);
        
        #endregion

        #region UAT-2387
        List<usp_SubscriptionChange_Result> ChangePackageAndSubscription(String xml,Boolean isOnlyPackageChange);
        #endregion
    }
}
