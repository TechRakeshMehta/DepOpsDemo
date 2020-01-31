using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

namespace DAL.Interfaces
{
    public interface IStoredProcedures
    {
        /// <summary>
        /// Get Document Mappings for all the item types i.e. Exception and Normal Items
        /// </summary>
        /// <param name="applicantComplianceAttributeIdsXML"></param>
        /// <param name="applicantComplianceItemIdsXML"></param>
        /// <returns></returns>
        DataSet GetDocumentMappings(String applicantComplianceAttributeIdsXML, String applicantComplianceItemIdsXML);

        /// <summary>
        /// Get Updated Document Mappings for a particular Item, whether Normal or Exception type
        /// </summary>
        /// <param name="applicantComplianceAttributeIdsXML"></param>
        /// <param name="applicantComplianceItemIdsXML"></param>
        /// <returns></returns>
        DataSet GetUpdatedDocumentMappings(Int32 applicantComplianceItemId, Boolean isExceptionType);


        DataTable GetNodeTemplatesByQuery(String query);

        #region Stored Procedures - Applicant Order Flow

        /// <summary>
        /// Common method, used to convert the 
        /// 1. OrganizationUserProfile 
        /// 2. List Of Aliases 
        /// 3. List of Residential History related data into XML's
        /// </summary>
        /// <param name="inputXML"></param>
        /// <param name="organizationUserProfileId"></param>
        /// <param name="storedProcedureName"></param>
        /// <returns></returns>
        DataTable ConvertXMLToAttribute(String inputXML, String storedProcedureName);

        /// <summary>
        /// Execute the Pricing stored procedure
        /// </summary>
        /// <param name="xml"></param>
        String GetPricingData(String inputXML);

        /// <summary>
        /// To get the Order Line Items
        /// </summary>
        /// <param name="inputXM"></param>
        /// <returns></returns>

        DataSet GetOrderLineItems(String inputXM);

        DataSet GetSavedOrderLineItems(Int32 OrderId);

        /// <summary>
        /// Updates the External Service and Vendor Id for the linte items, after order is successfully placed.
        /// </summary>
        /// <param name="masterOrderId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        void UpdateExtServiceVendorforLineItems(Int32 masterOrderId, Int32 tenantId);

        /// <summary>
        /// Get the Payment Options for the PAckages, at Package Level 
        /// and Also at the Node Level
        /// </summary>
        /// <param name="dppId"></param>
        /// <param name="dppsId"></param>
        /// <param name="bphmIds"></param>
        /// <param name="dpmId"></param>
        /// <returns></returns>
        DataSet GetPaymentOptions(Int32 dppId, String bphmIds, Int32 dpmId);
        DataSet GetPaymentOptions(string dppIds, String bphmIds, Int32 dpmId);

        #endregion

        DataSet GetBackroundOrderSearchDetail(CustomPagingArgsContract gridCustomPaging, BkgOrderSearchContract bkgOrderSearchContract);

        #region UAT-1683
        String ArchieveBkgOrder(short archieveStateID, Int32 currentUserId, List<Entity.ClientEntity.BkgOrder> lstBkgOrderToBeArchieved, List<Entity.ClientEntity.BkgOrderArchiveHistory> lstsubscriptionArchiveHistoryData);
        List<Entity.ClientEntity.BkgOrder> GetBkgOrderToBeArchived(List<Int32> orderIDs, List<Int32> archieveStatusIds);

        #region UAT-4085
        String UnArchieveBkgOrder(short archieveStateID, Int32 currentUserId, List<Entity.ClientEntity.BkgOrder> lstBkgOrderToBeUnArchived, List<Entity.ClientEntity.BkgOrderArchiveHistory> lstsubscriptionArchiveHistoryData);
        List<Entity.ClientEntity.BkgOrder> GetBkgOrderToBeUnArchived(List<Int32> orderIDs, List<Int32> archieveStatusIds);
        #endregion
        #endregion
        #region Supplement Order

        /// <summary>
        /// Execute the Supplement Order Pricing stored procedure
        /// </summary>
        /// <param name="xml"></param>
        String GetSupplementOrderPricingData(String inputXML);

        /// <summary>
        /// Gets Applicant details, related to the Current Background Order, for SupplementOrder flow
        /// </summary>
        DataTable GetApplicantData(Int32 masterOrderId);

        /// <summary>
        /// Gets Applicant Residential Histories & Personal Alias to display in Supplement Order, added during normal order
        /// </summary>
        DataSet GetApplicantBkgOrderDeta(Int32 masterOrderId);

        #endregion

        #region Manual Service Forms

        DataSet GetManualServiceFormsSearchDetail(CustomPagingArgsContract gridCustomPaging, ManualServiceFormsSearchContract manualServiceFormsSearchContract);

        #endregion

        List<ApplicantInstitutionHierarchyMapping> GetApplicantInstitutionHierarchyMapping(String OrganizationUserIDs);

        #region Evaluate Post Submit Rules For Multi
        void EvaluatePostSubmitRulesForMulti(String ruleXml, Int32 userID);
        #endregion

        #region Stored Procedures - Sales Force Service

        /// <summary>
        ///  Gets the list of Admins/Client admins and the Nodes on which they have permissions
        /// </summary>
        /// <param name="xml"></param>
        DataTable GetPermissionsSubscriptionSettings(String subEventCode, String serviceType, Int32 tenantId);

        /// <summary>
        ///  Gets the ComplianceData to be uploaded to Sales Force
        /// </summary>
        /// <param name="chunkSize"></param>
        DataSet GetComplianceDataToUpload(Int32 chunkSize);

        /// <summary>
        /// Save the history for the execution of the service to uplaod the data to Sales Force
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        void SaveComplianceUploadServiceHistory(String request, String response);

        /// <summary>
        /// Update the status of the Records that have been either uploaded or Error occured in their upload,
        /// depending on the code passed
        /// </summary>
        /// <param name="tpcduIds">CSV of the TPCDU_ID's</param>
        /// <param name="statusCode"></param>
        void UpdateThirdPartyComplianceDataUploadStatus(String tpcduIds, String statusCode, Int32 backgroundProcessUserId);

        /// <summary>
        /// Get Third Party Data Upload Response
        /// </summary>
        /// <param name="clientDataUploadID">clientDataUploadID</param>
        /// <returns> List<ThirdPartyDataUploadResponse></returns>
        List<ThirdPartyDataUploadResponseTypeContract> GetThirdPartyDataUploadResponseRegex(Int32 clientDataUploadID);
        #endregion

        #region Stored Procedures - Compliance Administration

        /// <summary>
        /// Gets the Package level Payment Options, based on the package type i.e. Background or Compliance
        /// </summary>
        /// <param name="pkgNodeMappingId"></param>
        /// <param name="packageTypeCode"></param>
        /// <param name="offlineSettlementCode"></param>
        /// <returns></returns>
        DataTable GetPackagePaymentOptions(Int32 pkgNodeMappingId, String packageTypeCode, String offlineSettlementCode);

        /// <summary>
        /// Updates the Display order of the Nodes of the Hierarchy Tree
        /// </summary>
        /// <param name="lstDPMIds"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="currentUserId"></param>
        Boolean UpdateNodeDisplayOrder(List<Entity.ClientEntity.GetChildNodesWithPermission> lstDPMIds, Int32? destinationIndex, Int32 currentUserId);

        /// <summary>
        /// Get the Institution Nodes of the Selected Tenant, for the current user, based on the Permissions
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        DataTable GetInstitutionNodes(Int32? userId);

        #endregion

        #region Stored Procedures - Admin Data Entry

        /// <summary>
        /// Gets the All the Package subscription related meta-data and applicant data 
        /// for the selected package subscription in Admin Data entry details screen
        /// </summary>
        /// <param name="pkgSubscriptionId"></param>
        /// <returns></returns>
        DataSet GetAdminDataEntrySubscription(Int32 pkgSubscriptionId, Int32 documentId);

        /// <summary>
        /// Gets the Package details upto ApplicantComplianceItemData for the selected Package Subscription.
        /// </summary>
        /// <param name="pkgSubscriptionId"></param>
        /// <returns></returns>
        DataSet GetPackageDetailsBySubscriptionId(Int32 pkgSubscriptionId);

        #endregion

        #region UAT-1067:Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on.
        DataTable GetAvailableCompAndBkgPackages(Int32 nodeId, String orderPackageTypeCode, Boolean isLoadParentPackages = false);
        #endregion

        #region Profile Sharing

        /// <summary>
        /// Get the compliance and background packages that can be shared by the applicant
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        DataSet GetSharingPackages(Int32 organizationUserId);

        /// <summary>
        /// Get the Requirement packages that can be shared by the admins, for Rotation sharing
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="rotationId"></param> 
        /// <returns></returns>
        DataSet GetSharingRequirementPackages(Int32 organizationUserId, Int32 rotationId);

        /// <summary>
        /// Stored procedure to get the address of a User, by Address HandleId
        /// </summary>
        /// <param name="addressHandleId"></param>
        /// <returns></returns>
        DataTable GetAddressByAddressHandleId(Guid addressHandleId);

        /// <summary>
        /// Get the Compliance & Background Packages (and their related data) for the
        /// selected applicants, out of which admin can select which category/service group etc can be shared - UAT 1324
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        DataSet GetSharingPackageData(String organizationUserIds);

        #endregion

        #region Generic 'Execute' Stored Procedure

        /// <summary>
        ///  Geeneric method to execute any Stored Procedure 
        /// </summary>
        /// <param name="dicParameters"></param>
        /// <param name="procedureName"></param>
        void ExecuteProcedure(Dictionary<String, Object> dicParameters, String procedureName);

        #endregion

        #region UAT-1843
        List<Int32> GetBkgOrderIdByOrgUsers(List<Int32> orgUserIds, Int32 ArchiveId);
        #endregion

        #region UAT-3077
        Tuple<Int32, Int32, Int32> ApprovedPaymentItem(Int32 orderId, Int32 currentLoggedInUserId);
        #endregion
    }
}
