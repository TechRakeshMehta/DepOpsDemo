using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.SearchUI;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using INTSOF.UI.Contract.PackageBundleManagement;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using System.Web.UI.WebControls;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.UI.Contract.SystemSetUp;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.Templates;
using INTSOF.UI.Contract.MobileAPI;
using INTSOF.UI.Contract.RecounciliationQueue;
using INTSOF.UI.Contract.FingerPrintSetup;

namespace DAL.Interfaces
{
    public interface IComplianceDataRepository
    {
        ComplianceSaveResponse RemoveExceptionData(Int32 applicantComplianceItem, Int32 organizationUserId);

        ComplianceSaveResponse UpdateExceptionData(ApplicantComplianceItemData applicantComplianceItemData, List<ExceptionDocumentMapping> exceptionMapping,
            List<int> savedApplicantDocumentIds, string reviewerTypeCode, string itemstatusCodeName
                                                    , String categoryStatusCode, String categoryExceptionStatusCode);

        ComplianceSaveResponse SaveExceptionData(ApplicantComplianceCategoryData applicantComplianceCategoryData,
                                ApplicantComplianceItemData applicantComplianceItemData,
                                string statusCodeName, string categoryStatusCode, string reviewerTypeCode,
            List<ExceptionDocumentMapping> exceptionDocumentMappings, List<int> savedApplicantDocumentIds, Boolean isCategory, String categoryExceptionStatusCode, Int32 orgnaizationUserID);

        PackageSubscription GetPackageSubscriptionByPackageID(Int32 compliancePackageID, Int32 organizationUserID);

        DataTable GetPackageTreeForServiceMapping(String packageIds);

        PackageSubscription GetPackageSubscriptionByID(Int32 tenantID, Int32 packageSubscriptionID);
        void AddDataInXMLForModifyShipping(Order applicantOrder, ApplicantOrderDataContract applicantOrderDataContract, Int32 orgUserID, Boolean isLocationServiceTenant ,PreviousAddressContract mailingAddress , FingerPrintAppointmentContract FingerPrintData);

        List<ApplicantComplianceItemData> DeleteOverideRuleStatus(Int32 currentLoggedInUserID, Int32 complianceCategoryID, Int32 packageSubscriptionID, List<lkpCategoryComplianceStatu> categoryComplianceStatus);
        String GetNodeHiearchy(Int32 packageSubscriptionID);

        /// <summary>
        /// Get the compliance item & its attributes details for which the dynamic applicant form is to be generated.
        /// </summary>
        /// <param name="itemId">Id of the selected item.</param>
        /// <returns>Details of the Item and its attributes for which dynamic form is to be generated.</returns>
        ComplianceItem GetDataEntryComplianceItem(Int32 itemId);

        /// <summary>
        /// Gets the list of Items for which applicant can enter data, under the selected category
        /// </summary>
        /// <param name="packageId">Id of the package selected</param>
        /// <param name="categoryId">Category for which applicant wants to enter data.</param>
        /// <param name="currentUserId">Organization User id of the applicant.</param>
        /// <returns>List of Items for which applicant can enter data, under the selected category.</returns>
        List<ComplianceItem> GetAvailableDataEntryItems(Int32 packageId, Int32 categoryId, Int32 currentUserId, Int32 currentSelectedItem = 0);

        /// <summary>
        /// Get already submitted data by applicant
        /// </summary>
        /// <param name="packageSubscriptionId"></param>
        /// <param name="complianceCategoryId"></param>
        /// <param name="complianceItemId"></param>
        /// <returns></returns>
        ApplicantComplianceItemData GetApplicantData(Int32 packageId, Int32 complianceCategoryId, Int32 complianceItemId, Int32 organizationUserId);


        ComplianceSaveResponse DeleteApplicantItemAttributeData(Int32 applicantComplianceItemId, Int32 currentUserId, String DeletedReasonCode, Int32 AppOrgUserID);

        String ValidateUIInput(Int32 organizationUserId, Int32 compliancePackageId, List<ApplicantComplianceAttributeData> lstApplicantData, Int32 complianceItemId, Int32 complianceCategoryId, Int32 packageSubscriptionId, Boolean isDataEntry, List<lkpObjectType> lstObjectTypes, List<ApplicantComplianceAttributeData> lstCompleteApplicantData = null);

        Dictionary<Int32, String> ValidateUIRulesVerificationDetail(List<ApplicantComplianceItemData> lstItemData, List<ApplicantComplianceAttributeData> lstAttributeData, Int32 packageSubscriptionId, Int32 compliancePackageId, Int32 complianceCategoryId, Int32 organizationUserId
            , List<lkpObjectType> lstObjectTypes, Int32 tenantId);

        String ValidateUIDocuments(Int32 organizationUserId, Int32 compliancePackageId, List<ApplicantComplianceAttributeData> lstApplicantData, ApplicantComplianceItemData applicantItemData, Int32 complianceCategoryId, Int32 packageSubscriptionId,
            List<lkpObjectType> lstObjectTypes);

        List<ApplicantDocuments> GetApplicantDocumentsData(Int32 organizationUserID);
        Int32 GetApplicantIdForSubscription(Int32 subscriptionId);

        List<ApplicantDocument> GetApplicantDocuments(Int32 organizationUserID);

        ApplicantDocument GetApplicantDocument(Int32 applicantDocumentId);
        ApplicantDocument GetFailedUnifiedApplicantDocument(Int32 applicantDocumentId);

        /// <summary>
        /// Get the category details for selected category in the applicant dynamic form, with explanaotry Notes.
        /// </summary>
        /// <param name="complianceCategoryId">Id of the selected category.</param>
        /// <returns>Details of the selected category.</returns>
        ComplianceCategory GetComplianceCategoryDetails(Int32 complianceCategoryId);

        /// <summary>
        /// To get cat updated by latest Info
        /// </summary>
        /// <param name="categoryID"></param>
        /// <param name="packageSubscriptionID"></param>
        /// <returns></returns>
        List<CatUpdatedByLatestInfo> GetCatUpdatedByLatestInfo(Int32 categoryID, Int32 packageSubscriptionID);

        List<INTSOF.Utils.CommonPocoClasses.ComplianceCategoryPocoClass> GetApplicantComplianceCategoryData(Int32 PackageSubscriptionID);

        Boolean CheckIfApplicantHasPlacedOrder(Int32 currentLoggedInUserId);

        /// <summary>
        /// Get the applicant orders to check if applicant has placed any order and 
        /// use it further to check any payment due.
        /// </summary>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        List<Order> GetApplicantOrders(Int32 currentLoggedInUserId);

        /// <summary>
        /// To check if cuurent Applicant have any order with Payment due.
        /// </summary>
        /// <param name="currentLoggedInUserId">currentLoggedInUserId</param>
        /// <returns>True/False</returns>
        Boolean CheckIfApplicantHasPaymentDue(Int32 currentLoggedInUserId);

        /// <summary>
        /// To check if cuurent Applicant have any order with Payment due.
        /// </summary>
        /// <param name="currentLoggedInUserId">currentLoggedInUserId</param>
        /// <returns>True/False</returns>
        Boolean CheckApplicantPaymentDue(Int32 currentLoggedInUserId, List<Order> lstOrders);

        /// <summary>
        /// Get Recent ApplicantHierarchyMapping of an applicant.
        /// </summary>
        /// <param name="applicantID">applicantID</param>
        /// <returns>ApplicantHierarchyMapping</returns>
        ApplicantHierarchyMapping GetRecentApplicantHierarchyMappingForApplicant(Int32 applicantID);

        #region Subscribe Default Package

        Int32 AddPackageSubscriptions(PackageSubscription packageSubscription);

        void CreateOptionalCategoryEntry(String packageSubscriptionIdsXML, Int32 currentUserId);

        #endregion

        #region Uploaded Documents

        ApplicantDocument GetApplicantUploadedDocument(Int32 applicantUploadedDocumentID);
        Boolean AddApplicantUploadedDocuments(List<ApplicantDocument> lstApplicantUploadedDocument);
        ApplicantDocument UpdateApplicantUploadedDocument(ApplicantDocument applicantUploadedDocument);
        ApplicantDocument DeleteApplicantUploadedDocument(Int32 applicantUploadedDocumentID, Int32 currentUserID, Int32? applicantID = null);

        Int32 AddApplicantDocument(ApplicantDocument applicantDocument);

        /// <summary>
        /// Check if the document with the same name is already uploaded by applicant, in data entry
        /// </summary>
        /// <param name="documentName"></param>
        /// <param name="documentSize"></param>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        Boolean IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, Int32 organizationUserId, List<lkpDocumentType> docType, Boolean isPersonalDoc = false);

        //UAT-2128
        List<UploadDocumentContract> GetsubscriptionItems(Int32 organizationUserID);

        ComplianceItemAttribute ItemHasFileAttribute(Int32 ComplianceItemId);

        #endregion

        #region ApplicantComplianceItemData for Verification

        /// <summary>
        /// To get Compliance Item data submitted by applicant
        /// </summary>
        /// <param name="applicantComplianceItemID"></param>
        /// <returns></returns>
        ApplicantComplianceItemData GetApplicantComplianceItemData(Int32 applicantComplianceItemID);

        List<VerificationQueueData> GetVerificationQueueData(VerificationQueueContract verificationQueueContract);


        /// <summary>
        /// Gets the active items for the assignment and user work queue based on the given parameters.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="statusCode">Status code</param>
        /// <param name="assignToUserId">Assign to User Id</param>
        /// <param name="reviewerType">Reviewer Type Code</param>
        /// <param name="showIncompleteItems">To Show incomplete Items</param>
        /// <returns>Query for showing active items</returns>
        IQueryable<vwComplianceItemDataQueue> GetApplicantComplianceItemData(Int32 tenantId, List<String> lstStatusCode, Int32 assignToUserId, String reviewerType, Boolean showIncompleteItems, Int32 reviewerId);

        /// <summary>
        /// Gets the active items for the assignment and user work queue based on the given parameters for client users.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="statusCode">Status code</param>
        /// <param name="assignToUserId">Assign to User Id</param>
        /// <param name="reviewerType">Reviewer Type Code</param>
        /// <param name="showIncompleteItems">To Show incomplete Items</param>
        /// <returns>Query for showing active items</returns>
        IQueryable<vwComplianceItemDataQueueRestricted> GetApplicantComplianceItemDataForClientUsers(Int32 tenantId, List<String> lstStatusCode, Int32 assignToUserId, String reviewerType, Boolean showIncompleteItems, Int32 reviewerId);
        IQueryable<vwComplianceItemDataQueueUG> GetApplicantComplianceItemDataUG(Int32 tenantId, List<String> lstStatusCode, Int32 assignToUserId, String reviewerType, Boolean showIncompleteItems, Int32 reviewerId);
        IQueryable<vwComplianceItemDataQueueRestrictedUG> GetApplicantComplianceItemDataForClientUsersUG(Int32 tenantId, List<String> lstStatusCode, Int32 assignToUserId, String reviewerType, Boolean showIncompleteItems, Int32 reviewerId);

        DataTable GetApplicantComplianceItemDataTable(ItemVerificationQueueData verificationQueueData, CustomPagingArgsContract verificationGridCustomPaging, String customHTML, String DPMID);

        vwApplicantComplianceItemData GetApplicantVerificationDetails(Int32 subcriptionId);

        //List<Int32?> GetSubscriptionIdList(Int32? TenantId, Int32? PackageId, Int32? CategoryId, Int32? UserGroupId, Boolean? IncludeIncompleteItems, Boolean? ShowOnlyRushOrder, String StatusCodes, Int32? ReviewerId, String ReviewerType, Int32? AssignToUserId, Int32? SubscriptionId, Int32? orgUserId, out Int32 PageIndex, out Int32 TotalPage, Boolean isDefaultThirdParty);
        DataTable GetSubscriptionIdList(Int32? TenantId, Int32? PackageId, Int32? CategoryId, Int32? UserGroupId, Boolean? IncludeIncompleteItems,
            Boolean? ShowOnlyRushOrder, String StatusCodes, Int32? ReviewerId, String ReviewerType, Int32? AssignToUserId, Int32? SubscriptionId, Int32 ApplicantComplianceItemID,
            Int32? orgUserId, Boolean isDefaultThirdParty, Boolean IsEscalationRecords, Int32 currentLoggedInUser);

        /// <summary>
        /// Assigns the items corresponding to the given list of Compliance Item Ids to the selected user. 
        /// </summary>
        /// <param name="complianceItemIds">List of Compliance Item Ids</param>
        /// <param name="userId">Selected user Id</param>
        /// <param name="currentLoggedInUserId">Current Logged In User Id</param>
        /// <returns>True if items assigned successfully</returns>
        Boolean AssignItemsToUser(List<Int32> complianceItemIds, Int32 userId, Int32 currentLoggedInUserId);

        DataTable AssignItemsToUserNew(Int32 tenantId, String xml, Int32 currentLoggedInUserId, Int32 assignToUserId, Boolean IsMutipleTimesAssignmentAllowed);

        List<ApplicantItemVerificationData> GetApplicantDataForVerification(Int32 complianceCategoryId, Int32 packageSubscriptionId);


        Boolean UpdateDocumentPath(String newFileName, Int32 documentId);

        /// <summary>
        /// CategoryNotes updated by the admins when all items are having exceptions applied.
        /// </summary>
        /// <param name="applicantComplianceCategoryId"></param>
        /// <param name="notes"></param>
        /// <param name="currentUserId"></param>
        void UpdateApplicantCategoryNotes(Int32 applicantComplianceCategoryId, String notes, Int32 currentUserId);

        Boolean GetDocumentTypeAttributeData(Int32 applicantItemDataId, out Int32 attributeId, out Int32 applicantAttributeDataId, out Int32 documentCount);

        /// <summary>
        ///  Gets the Document Type attribute data for all items except Incomplete items, for UI rule validation in the Verification details screen.
        /// </summary>
        /// <param name="lstItemData"></param>
        /// <returns></returns>
        List<ApplicantComplianceAttributeData> GetAllDocumentTypeAttributData(List<Int32> lstApplicantItemDataIds);

        #endregion

        #region ApplicationComplianceItemData for Exception
        /// <summary>
        /// Get Applicant Compliance Item by id
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        ApplicantComplianceItemData GetApplicantComplianceItemDataByID(Int32 applicantComlianceCategoryID, Int32 itemID);

        ApplicantComplianceItemData GetApplicantComplianceItemDataByID(Int32 applicantComplianceItemID);

        /// <summary>
        /// To update Item Data Status
        /// </summary>
        /// <param name="ItemDataId"></param>
        /// <param name="currentStatusId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        Boolean UpdateItemData(Int32 itemDataId, String comments, Int32 currentStatusId, Int32 currentLoggedInUserId, String currentLoggedInUserName, List<Int32> ListOfIdToRemoveDocument, Int32 itemId, List<Int32> ListOfIdToAddDocument);

        Boolean UpdateExceptionVerificationItemData(Int32 itemDataId, Int32 itemId, String comments, Int32 statusId, Int32 currentLoggedInUserId, String currentLoggedInUserName, List<Int32> lstDocumentsToRemove, List<ListItemAssignmentProperties> lstAssignmentProperties, String recordActionType, String currentLoggedInUserInitials, String statusCode, DateTime? itemExpirationDate);

        /// <summary>
        /// Save the Applicant uploaded documents and map with ApplicantComplianceItemData
        /// </summary>
        /// <param name="applicantDocument"></param>
        /// <param name="applicantComplianceItemID"></param>
        /// <returns></returns>
        Boolean SaveApplicantUploadedDocuments(List<ApplicantDocument> applicantDocumentList, Int32 applicantComplianceItemId);
        int SaveApplicantDocument(ApplicantDocument applicantDocument);

        /// <summary>
        /// Save the Applicant uploaded documents and map with ApplicantComplianceItemData and Add in ApplicantComplianceItemData
        /// </summary>
        /// <param name="applicantDocument"></param>
        /// <param name="applicantComplianceItemID"></param>
        /// <returns></returns>
        Int32 SaveInApplicantItemDataAndMapTable(ApplicantDocument applicantDocument, ApplicantComplianceItemData applicantComplianceItemData);

        void RemoveExceptionDocumentMapping(Int32 mappingId, Int32 currentUserId);

        #endregion

        /// <summary>
        /// Get the UserDetails by OrganizationUserId
        /// </summary>
        /// <param name="orgUserId"></param>
        /// <returns></returns>
        usp_GetUserDetails_Result GetUserData(Int32 orgUserId);

        /// <summary>
        /// Save/Update Applicant data from Data Entry screen
        /// </summary>
        /// <param name="applicantCategoryData">Data of the category for which applicant is entering/updating the data.</param>
        /// <param name="applicantItemData">Data of the item for which applicant is entering/updating the data.</param>
        /// <param name="lstApplicantData">Actual data of the attributes, entered by the applicant, in dynamic form.</param>
        /// <param name="createdModifiedById">Id of the applicant, adding/updating the data.</param>
        /// <param name="attributeDocuments"></param>
        /// <param name="categoryComplianceStatus">Status of the Category compliance.</param>
        /// <param name="itemComplianceStatus">Status of item compliance</param>
        /// <param name="packageSubscriptionId">Id of the subcription</param>
        ComplianceSaveResponse SaveApplicantData(ApplicantComplianceCategoryData applicantCategoryData, ApplicantComplianceItemData applicantItemData, List<ApplicantComplianceAttributeData> lstApplicantData, Int32 createdModifiedById, Dictionary<Int32, Int32> attributeDocuments, String categoryComplianceStatus, Int32 compliancePackageId, Boolean isUIValidationApplicable, AssignmentProperty assignmentProperty, Int32 packageSubscriptionId,
            List<lkpItemMovementType> lstItemMovementTypes, List<lkpObjectType> lstObjectTypes, Boolean isDataEntryForm, Dictionary<Int32, Int32> viewAttributeDocuments, Int32 orgUsrID, Int32 tenantID);

        /// <summary>
        /// Save/Update Applicant data from Verification details screen and Update item status
        /// </summary>
        ApplicantComplianceItemData SaveApplicanteDataVerificationDetails(VerificationDetailsContract verificationDetailsContract, String recordActionType, Int32 categoryComplianceStatusId,
            List<lkpItemMovementType> lstMovementTypes, Int32 tenantId, Boolean IsReconciliationDataSaved);

        //void SaveApplicanteDataVerificationDetails(ApplicantComplianceCategoryData applicantCategoryData, ApplicantComplianceItemData applicantItemData, List<ApplicantComplianceAttributeData> lstApplicantData,
        // Int32 createdModifiedById, String adminComments, Int32 _newStatus, Int16 reviewerTypeId, Int32? reviewerTenantId, Int32 thirdPartyReviewerUserId, Int32 applicantId, Boolean IsAdminReviewRequired, String newItemStatusCode, String currentTenantTypeCode, Int32 packageId);

        Boolean UpdateApplicantComplianceDocumentMaps(List<ApplicantComplianceDocumentMap> toAddDocumentMap, List<Int32> toDeleteApplicantComplianceDocumentMapIDs, Int32 currentUserId);

        /// <summary>
        /// Insert/Update Documents related data for all items 
        /// 1. EXCEPT incomplete
        /// 2. Data for other attributes has been already added but FileType attribute is being added for the first time
        /// </summary>
        /// <param name="toAddDocumentMap"></param>
        /// <param name="ToAddDocumentMapException"></param>
        /// <returns></returns>
        Boolean AddUpdateApplicantComplianceDocumentMappingData(ApplicantComplianceDocumentMap toAddDocumentMap, ExceptionDocumentMapping ToAddDocumentMapException, Int32 applicantComplianceItemDataId, Int32 complianceAttributeId);

        /// <summary>
        /// Add Category, Item, Attribute & Document mapping for Incomplete Items
        /// </summary>
        /// <param name="toAddDocumentMap"></param>
        /// <param name="categoryData"></param>
        /// <param name="itemData"></param>
        /// <param name="attributeData"></param>
        /// <returns></returns>
        void AddIncompleteDocumentMapping(ApplicantComplianceDocumentMap toAddDocumentMap, ApplicantComplianceCategoryData categoryData,
                 ApplicantComplianceItemData itemData, ApplicantComplianceAttributeData attributeData, Int32 packageSubscriptionId, Int32 applicantId, out Int32 itemDataId);

        Boolean RemoveMapping(Int32 applicantMappingId, Int32 curentUserId, Boolean Isexception);

        Boolean AssignUnAssignItemDocuments(List<ApplicantComplianceDocumentMap> toAddDocumentMap, List<ExceptionDocumentMapping> ToAddDocumentMapException, List<Int32> ToDeleteApplicantComplianceDocumentMapIDs, Boolean IsException, Int32 currentUserId, Int32 applicantComplianceItemId);

        void AssignUnAssignIncompleteItemDocuments(List<ApplicantComplianceDocumentMap> toAddDocumentMap, List<Int32> toDeleteDocumentMap, ApplicantComplianceCategoryData categoryData,
        ApplicantComplianceItemData itemData, ApplicantComplianceAttributeData attributeData, Int32 packageSubscriptionId, Int32 applicantId, Int32 currentUserId, out Int32 itemDataId, List<ListItemAssignmentProperties> lstAssignmentProperties);

        OrganizationUser AddOrganizationUser(OrganizationUser organizationUser);

        OrganizationUserDepartment AddOrganizationUserDept(OrganizationUserDepartment orguserDept);

        OrganizationUserProfile AddOrganizationUserProfile(OrganizationUserProfile orgUserProfile);

        Boolean AddAddress(Address address);

        Boolean AddAddressHandle(AddressHandle addressHandle);

        OrganizationUser GetOrganisationUser(Entity.OrganizationUser organizationUser);

        OrganizationUserProfile GetOrganizationUserProfile(Entity.OrganizationUser organizationUser);

        Boolean UpdateOrganizationData(OrganizationUser organizationUser);
        Boolean IsItemStatusApproved(Int32 itemId);

        #region Search

        //TODO: Will remove this method when custom paging is implemented in applicant search. 
        IQueryable<T> PerformSearch<T>(Dictionary<String, String> searchOptions, String orderByFieldName);

        IQueryable<T> PerformSearch<T>(Dictionary<String, String> searchOptions, CustomPagingArgsContract customPagingArgsContract);

        DataTable GetApplicantListDataValues(SearchItemDataContract searchItemDataContract, CustomPagingArgsContract customPagingArgsContract);
        DataTable GetItemDataSearchData(SearchItemDataContract searchItemDataContract, CustomPagingArgsContract customPagingArgsContract, String DPMids);//UAT-1055 //Int32? DPMid)

        List<ComplianceRecord> GetComplianceRecordsSearch(SearchItemDataContract searchItemDataContract, CustomPagingArgsContract customPagingArgsContract);

        // Changes done for "Use SP for Compliance Item Data Search"
        ObjectResult<ComplianceItemData> GetComplianceItemDataSearch(SearchItemDataContract searchItemDataContract, CustomPagingArgsContract customPagingArgsContract);

        #endregion

        #region Client Relation
        Boolean CopyTenantToClient(List<ClientRelation> clientChildRelationListToCopy);

        List<Int32> checkIfTenantExist(List<Int32> relatedTenantIds);
        List<Tenant> getTenantsToBeCopied(List<Int32> listTenantIdToCopy);
        Boolean CopyTenantToClient(List<Tenant> TenantsToBeCopied);
        Boolean DeleteSubTenant(Int32 tenantId, Int32 relatedTenantId, Int32 currentUserId);

        /// <summary>
        /// Retrieve a list of institution Type tenant.
        /// </summary>
        /// <returns>list of institution Type tenant</returns>
        List<Tenant> getClientTenant(Int32 defaultTenantId);

        /// <summary>
        /// Retrieve a list of institution Type and super admin tenants.
        /// </summary>
        /// <param name="tenantId">Super admin tenant id</param>
        /// <returns>list of institution Type and super admin tenants</returns>
        List<Tenant> GetMasterAndInstitutionTypeTenants(Int32 tenantId);

        /// <summary>
        /// Retrieve a list of Parent tenant.
        /// </summary>
        /// <returns>list of parent tenant</returns>
        List<Tenant> getParentTenant(Int32 subTenantId);
        #endregion

        #region Order Queue And Order Detail Screen

        /// <summary>
        /// Gets the list Of Active Orders filtered on the bases of statuses.
        /// </summary>
        /// <param name="lstStatusCode">List Of Status Code</param>
        /// <returns>Query for fetching active orders</returns>
        IQueryable<vwOrderDetail> GetOrderDetailList(List<String> lstStatusCode, Int32 CurrentuserId);

        /// <summary>
        /// Returns the list of the Orders, for which Rush Order can be placed
        /// </summary>
        /// <returns></returns>
        List<Int32> GetPossibleRushOrderIds(List<vwOrderDetail> lstOrderDetails);

        /// <summary>
        /// Gets the details for the given Order Id.
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns></returns>
        OrderPaymentDetail GetOrderDetailById(Int32 orderId);

        /// <summary>
        /// Gets the successfull Online Payment Transaction details, for an Order
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns></returns>
        OnlinePaymentTransaction GetSuccessfullOrderPaymentDetails(Int32 orderId);

        /// <summary>
        /// Gets the Online payment transaction record by invoice number
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns></returns>
        OnlinePaymentTransaction GetOnlinePayTransactionByInvNum(String invoiceNumber);

        Boolean CheckIsInvoiceOnly(Int32 hierarchyId);
        /// <summary>
        /// Gets the package subscription details for the given Order Id.
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns></returns>
        PackageSubscription GetPackageSubscriptionDetailByOrderId(Int32 orderId);
        /// <summary>
        /// Saves the reference number in th order table and changes th status from Pending paymnt Approved to Paid..
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <param name="orderStatusCode">StatusCode to be updated</param>
        /// <param name="currentLoggedInUserId">CurrentLoggedInUserId</param>
        /// <param name="referenceNumber">Reference Number</param>
        /// <returns>True, if status is updated. Else false.</returns>
        Boolean UpdateOrderStatus(Int32 orderId, String orderStatusCode, Int32 currentLoggedInUserId, String referenceNumber,
            List<lkpEventHistory> lstEvents, List<lkpOrderStatusType> lstOrderStatusType, Int32 tenantId, Int32 orderPaymentDetailId = 0);

        /// <summary>
        /// Cancel the order with the staus Cancellation requested and updates the status to Cancelled.
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <param name="orderStatusCode">StatusCode to be updated</param>
        /// <param name="currentLoggedInUserId">CurrentLoggedInUserId</param>
        /// <returns>True or false</returns>
        Boolean CancelPlacedOrder(Int32 orderId, String orderStatusCode, Int32 currentLoggedInUserId, String rejectionReason, Boolean isCancelledByApplicant, List<lkpEventHistory> lstEventHistory = null, Int32? orderPaymentDetailID = null, Boolean? IsCompliancePackageInclude = null, Boolean isInstantCancellation = false, Boolean isLocationTenant = false);

        /// <summary>
        ///  Request to Cancel the order with the staus Pending to Approve and updates the status to Cancellation requested.
        /// </summary>
        /// <param name="order">order Entity object</param>
        /// <param name="orderStatusCode">StatusCode to be updated</param>
        /// <returns></returns>
        Order UpdateOrderByOrderID(Order order, String orderStatusCode, Boolean isNewOrder = false, List<lkpEventHistory> lstOrderEvents = null,
            List<lkpOrderStatusType> lstBkgOrderStatusTypes = null, Int32 orderPaymentDetailId = 0, Boolean isRushOrderForExistingOrder = false);

        /// <summary>
        /// Rejects the cancellation request and updates the order its previous status.
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <param name="currentLoggedInUserId">currentLoggedInUserId</param>
        /// <param name="rejectionReason">Rejection Reason</param>
        /// <returns>True if order is updated</returns>
        Boolean RejectCancellationRequest(Int32 orderId, Int32 currentLoggedInUserId, String rejectionReason);

        /// <summary>
        /// check Package Subscription Details for the given orderids.
        /// </summary>
        /// <param name="orderId">Order Id List</param>
        /// <returns></returns>
        Boolean IsPackageSubscribedForOrderIds(List<Int32> orderIds);

        Boolean SaveScheduleTask(List<ScheduledTask> lstScheduleTask);
        ///// <summary>
        ///// Adds the Event history for the Order, when Approved/Rejected by admin
        ///// Also called when online order is being approved
        ///// Currently only for Order Creation, Approved & Rejected (No other case)
        ///// </summary>
        ///// <param name="lstEvents"></param>
        ///// <param name="_orderId"></param>
        ///// <param name="currentLoggedInUserId"></param>
        ///// <param name="orderEventMessage"></param>
        ///// <param name="orderEventCode"></param>
        //void AddBkgOrderEventHistory(List<lkpEventHistory> lstEvents, Int32 _orderId, Int32 currentLoggedInUserId, String orderEventMessage, String orderEventCode);

        /// <summary>
        /// Generates an entry for the Refund request in Order Details screen
        /// </summary>
        /// <param name="refundHistory"></param>
        void AddRefundHistory(RefundHistory refundHistory);

        /// <summary>
        /// Returns the list of Refunds associated with the current order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        List<RefundHistory> GetRefundHistory(Int32 orderId);

        #endregion

        #region Order selection & Payment


        /// <summary>
        /// Get Department ProgramPackageSubscriptionDetail
        /// </summary>
        /// <param name="programPackageSubscriptionId">DPPSID</param>
        /// <returns></returns>
        DeptProgramPackageSubscription GetDeptProgramPackageSubscriptionForPaymentOption(Int32 DPPSID);

        List<DeptProgramPaymentOption> GetDeptProgramPaymentOptionsByDepProgramMappingId(Int32 deptProgramMappingId);




        /// <summary>
        /// Get the details of the applicant selected package, when applicant is purchasing a package
        /// </summary>
        /// <param name="dppsId"></param>
        /// <returns></returns>
        DeptProgramPackageSubscription GetApplicantPackageDetails(Int32 dppsId);

        /// <summary>
        /// Get the institute Hierarchy Label, when Order review screen is opened
        /// </summary>
        /// <param name="dpmId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        String GetInstituteHierarchyLabel(Int32 dpmId);

        /// <summary>
        /// Generate New Compliance and Background orders 
        /// </summary>
        /// <param name="organizationUserProfile"></param>
        /// <param name="addressId"></param>
        /// <param name="addressHandleId"></param>
        /// <param name="userOrder"></param>
        /// <param name="programPackageSubscriptionId"></param>
        /// <param name="selectedPaymentModeId"></param>
        /// <param name="tenantId"></param>
        /// <param name="lstAttributeValues"></param>
        /// <param name="lastNodeDPMId"></param>
        /// <param name="lstBackgroundPackages"></param>
        /// <param name="paymentModeCode"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        //String SaveApplicantOrderProcessClient(OrganizationUserProfile organizationUserProfile, Int32 addressId, Guid addressHandleId, Order order, Int32 programPackageSubscriptionId, Int32 selectedPaymentModeId, Int32 tenantId, List<TypeCustomAttributes> lstAttributeValues, Int32 lastNodeDPMId, List<BackgroundPackagesContract> lstBackgroundPackages, out String paymentModeCode, out Int32 orderId, List<Entity.ResidentialHistoryProfile> lstResidentialHistoryProfile, List<Entity.PersonAliasProfile> lstPersonAliasProfile, Int32 orderCreatedStatusId);
        Dictionary<String, String> SaveApplicantOrderProcessClient(Order applicantOrder, ApplicantOrderDataContract applicantOrderDataContract, Int16 svcLineItemDispatchStatusId, out String paymentModeCode, out Int32 orderId, Int32 orgUserID, Int32? orderRequestNewOrderTypeId = null, List<OrderCartCompliancePackage> compliancePackages = null, Boolean isLocationServiceTenant = false, FingerPrintAppointmentContract FingerPrintData = null, PreviousAddressContract mailingAddress = null);

        void UpdateApplicanDetailsClient(OrganizationUser organizationUser, Dictionary<String, Object> dicAddressData, Int32 addressIdMaster, List<Entity.ResidentialHistory> lstResendentialHistory, List<Entity.PersonAlia> lstPersonAlia, PreviousAddressContract mailingAddress = null);

        String GetPaymentOptionCodeById(Int32 paymentOptionId);

        void UpdateMailingAddress(PreviousAddressContract mailingAddress, Boolean isLocationServiceTenant, Int32 orgUserID);

        void UpdateOrderPayment(ApplicantOrderCart applicantOrderCart, Int32 orgUserID);

        OnlinePaymentTransaction GetPaymentTransactionDetails(String invoiceNumber, Boolean requiredOrder = false);

        OnlinePaymentTransaction UpdateOnlineTransactionResults(String invoiceNumber, NameValueCollection transactionDetails, Int32 modifiedUserID);

        Boolean SaveIPNResponse(String invoiceNumber, String transactionID, String ipnTransactionStatus, String ipnPostData);

        Int32 GetOrderStatusCode(String status);

        Int32 GetOrderPaymentCount(Int32 orderID);

        String GenerateInvoiceNumber(Int32 orderID, Int32 tenantId, Boolean isRushOrder, List<Int32> childOrderIds = null);

        /// <summary>
        /// Attaches the OnlinePaymentTransaction instance for the current Order,
        /// which further attaches the OrderPaymentDetails and OrderPkgPaymentDetails
        /// </summary>
        /// <param name="applicantOrder"></param>
        /// <param name="creationDateTime"></param>
        /// <param name="invoiceNumber"></param>
        /// <param name="totalAmount"></param>
        /// <param name="paymentModeId"></param>
        /// <param name="orderStatusId"></param>
        /// <returns></returns>
        OrderPaymentDetail AddOnlinePaymentTransaction(Order applicantOrder, DateTime creationDateTime, String invoiceNumber,
                                                 Decimal totalAmount, Int32 paymentModeId, Int32 orderStatusId, Int32 currentUserId, decimal adjustedAmount = 0);

        DeptProgramPackageSubscription GetDeptProgramPackageSubscriptionDetail(Int32 programPackageSubscriptionId);

        String UpdateOrderByID(Order order, String orderStatusCode, Int32 ordPayDetailId, Int32 paymentModeId, out Int32 insertedOrdPayDetailId, Int32 tenantId, Boolean IsRevertToMoneyOrder);

        /// <summary>
        /// Update the Status of the OPD for the Credit Card, to the status specified and payment profile Id
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="opdId"></param>
        /// <param name="paymentProfileId"></param>
        void UpdateOPDStatus(Int32 statusId, Int32 currentUserId, Int32 opdId, long paymentProfileId);

        Int32 GetPaymentApprovalRequiredSetting(String orderIDs);
        void UpdateOPDStatusAndPaymentProfileId(Int32 statusId, Int32 currentUserId, List<Int32> ccOPDList, long paymentProfileId);

        #endregion

        #region Rush Order Review

        /// <summary>
        /// To get DeptProgramPackageSubscription object
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        DeptProgramPackageSubscription GetDeptProgramPackageSubscription(Int32 orderId, Int32 subscriptionId);

        /// <summary>
        /// To update rush order detail
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderStatusCode"></param>
        /// <param name="paymentModeId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        String UpdateRushOrderByOrderID(Order order, String orderStatusCode, Int32 paymentModeId, Int32 tenantId,
            out OrderPaymentDetail ordPayDetail, Int32 optTypeId);

        /// <summary>
        /// To update rush order detail
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderStatusCode"></param>
        /// <returns></returns>
        Boolean UpdateRushOrderExistByID(Order order, String orderStatusCode);


        #endregion

        #region Create Order

        /// <summary>
        /// A copy of this method is - GetCompliancePackages
        /// ANY CHANGES TO THIS SHOULD ALSO BE DONE IN GetCompliancePackages METHOD
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="deptProgramMappingIds"></param>
        /// <param name="NoUserMode"></param>
        /// <returns></returns>
        List<DeptProgramPackage> GetDeptProgramPackage(Int32 organizationUserId, List<Int32> depProgramMappingIds, Boolean NoUserMode = false);

        /// <summary>
        /// Gets the list of Compliance Packages available for purchase, for applicant, in Pending order screen 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="deptProgramMappingIds"></param>
        /// <param name="NoUserMode"></param>
        /// <returns></returns>
        Dictionary<String, List<DeptProgramPackage>> GetCompliancePackages(Int32 organizationUserId, Dictionary<Int32, Int32> deptProgramMappingIds
                                                                          , Int32? previousPkgId, Int32? previousNodeId, Boolean NoUserMode = false);

        //UAT-3259
        List<Int32> GetAlreadyExpiredComplPackageIds(Int32 organizationUserId);

        DeptProgramPackage GetDeptProgramPackageById(Int32 deptProgramPackageId);

        List<MobilityNodePackages> GetMobilityNodePackages(Int32 selectedNodeId, Int32 selectedNodeDPPId);

        List<DeptProgramMapping> GetHierarchyNode(Int32 nodeId, Boolean isParent, out Int32 possibleNodeId, Int32 changeSubscriptionSourceNodeId = 0, Int32 changeSubscriptionSourceNodeDPPId = 0, String languageCode = default(String));

        List<DeptProgramPackageSubscription> GetDeptProgramPackageSubscription(Int32 deptProgramPackageId);
        Boolean SaveOrganizationUserDepartment(OrganizationUserDepartment organizationUserDepartment);
        OrganizationUserDepartment GetOrganizationUserDepartment(Int32 organizationUserId);

        Boolean UpdateChanges();

        Int32 GetDefaultNodeId();
        Int32 GetInstitutionDPMID();

        Int32 GetLastNodeInstitutionId(Int32 lastNodeDPMId);

        /// <summary>
        /// Get the DeptProgramPackage of the change subscription 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        DeptProgramPackage GetChangeSubscriptionSourceNode(Int32 orderId);

        #endregion

        #region Subscription

        IQueryable<vwSubscription> GetSubscriptionList(Int32 currentUserId);

        #endregion

        #region Client Setings

        List<ClientSetting> GetClientSetting(Int32 tenantId);

        Boolean AddClientSetting(ClientSetting clientSetting);

        Boolean UpdateClientSetting();

        Boolean GetReviewPackageMappingEveryTransitionValue(Int32 tenantId, String code);

        Boolean GetAutoApprovalTransition(Int32 tenantId, String code);

        Int32 GetPendingPackageFrequencyDays(Int32 tenantId, String code);

        Int32 GetSubscriptionNotificationBeforeExpiryDays(Int32 tenantId, String code);

        Int32 GetSubscriptionNotificationAfterExpiryDays(Int32 tenantId, String code);

        List<SubscriptionFrequency> GetSubscriptionNotificationFrequencyDays();

        Int32 GetComplianceNotificationBeforeExpiryDays(Int32 tenantId, String code);

        Int32 GetComplianceNotificationFrequencyDays(Int32 tenantId, String code);

        Boolean GetRushOrderValue(Int32 tenantId, String code);

        Boolean GetRushOrderForInvoiceValue(Int32 tenantId, String code);

        Int32 GetMobilityInstanceLeadDays(Int32 tenantId, String code);

        Int32 GetMobilityTansitionLeadDays(Int32 tenantId, String code);

        List<GetPaymentOptions> GetPaymentOptions();
        Boolean UpdatePaymentOptions(List<GetPaymentOptions> newPaymentOption, Int32 loggedInUser);
        #endregion

        #region Package Subscription

        List<ReminderContract> GetExpiryPackageSubscriptions(String entitySetName, Int32 chunkSize);
        List<ReminderContract> GetExpiredPackageSubscriptions(String entitySetName, Int32 chunkSize, Int32? archieveStateId);
        List<ReminderContract> GetPendingPackageSubscriptions(Int32 pendingFrequency, String entitySetName, DateTime today, Int32 chunkSize, Int32 tenantID);

        #endregion

        #region Notification Delivery

        Boolean AddNotificationDelivery(NotificationDelivery notificationDelivery);

        Boolean AddNotificationDeliveryList(List<NotificationDelivery> lstNotificationDelivery);

        #endregion

        #region Renewal Order
        /// <summary>
        /// Gets the order details for the given Order Id.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="orderId">Order Id</param>
        /// <returns></returns>
        Order GetOrderById(Int32 orderId);

        Order GetOrderDetailsByOrderId(Int32 orderId);

        /// <summary>
        /// Get Package Subscription by orderId
        /// </summary>
        /// <param name="orderId">orderId</param>
        /// <returns>PackageSubscription</returns>
        PackageSubscription GetPackageSubscriptionByOrderId(Int32 orderId);

        /// <summary>
        /// Save changes for Package Subscription
        /// </summary>
        /// <returns>Boolean</returns>
        Boolean UpdatePackageSubscription();

        /// <summary>
        /// Get the list of Order of previous order
        /// </summary>
        /// <param name="orderDetail"> order</param>
        /// <returns>List of Order</returns>
        List<Order> GetOrderListOfPreviousOrder(Order orderDetail);

        List<Order> GetChangeSubscriptionOrderList(Int32 orderID);

        Boolean GetRenewSubscriptionOrder(Int32 orderID);

        Int32 GetParentOrderByOrderId(Int32 orderId);
        #endregion

        #region Mail and Message Content

        Dictionary<String, object> GetOrderCreationMoneyOrderMailData(Entity.ClientEntity.Order applicantOrder, Entity.ClientEntity.OrganizationUserProfile orgUserProfile, Int32 tenantId, String _paymentModeCode);

        Dictionary<String, object> GetOrderCreationInvoiceMailData(Entity.ClientEntity.Order applicantOrder, Entity.ClientEntity.OrganizationUserProfile orgUserProfile, int tenantId, String _paymentModeCode);

        Dictionary<String, object> GetOrderApprovalMailData(OrderPaymentDetail orderPaymentDetail, Int32 tenantID, String orderPackageTypeCode);

        Dictionary<String, object> GetCCOrderApprovalMailData(OrderPaymentDetail orderPaymentDetail, Int32 tenantID, String orderPackageTypeCode);

        Dictionary<String, object> GetOrderCancellationMailData(OrderPaymentDetail orderPaymentDetail, int tenantId);

        Dictionary<String, object> GetOrderCancellationApprovedMailData(OrderPaymentDetail orderPaymentDetail, int tenantId);

        Dictionary<String, object> GetOrderCancellationRejectedMailData(OrderPaymentDetail orderPaymentDetai, Int32 tenantId);

        Dictionary<String, object> GetRushOrderConfirmationMailData(PackageSubscription packageSubscription, int tenantId, int onlinePaymentDetailID);

        Dictionary<String, object> GetCCRushOrderConfirmationMailData(PackageSubscription packageSubscription, int tenantId, int onlinePaymentDetailID);

        Dictionary<String, object> GetOrderRejectionMailData(OrderPaymentDetail orderPaymentDetail, Int32 tenantId, Boolean isCompliancePackageInclude);

        #endregion

        #region Manage Institution Node

        /// <summary>
        /// Method to return all Nodes.
        /// </summary>
        /// <returns>IQueryable</returns>
        IQueryable<InstitutionNode> GetInstitutionNodeList();

        /// <summary>
        /// Get the Node by NodeId
        /// </summary>
        /// <param name="priceAdjustmentId">NodeId</param>
        /// <returns>InstitutionNode</returns>
        InstitutionNode GetNodeByNodeId(Int32 NodeId);

        /// <summary>
        /// Save Institution Node
        /// </summary>
        /// <param name="nodeDetail">nodeDetail</param>
        /// <param name="lstCustomAttributeMapping">lstCustomAttributeMapping</param>
        /// <returns></returns>
        Boolean SaveNodeDetail(InstitutionNode nodeDetail, List<CustomAttributeMapping> lstCustomAttributeMapping);

        /// <summary>
        /// Update Institution Node
        /// </summary>
        /// <param name="nodeId">nodeId</param>
        /// <param name="lstCustomAttributeMapping">lstCustomAttributeMapping</param>
        /// <param name="currentUserId">currentUserId</param>
        /// <returns></returns>
        Boolean UpdateNodeDetail(Int32 nodeId, List<CustomAttributeMapping> lstCustomAttributeMapping, Int32 currentUserId);

        /// <summary>
        /// Method to return all Node Types.
        /// </summary>
        /// <returns>IQueryable</returns>
        IQueryable<InstitutionNodeType> GetInstitutionNodeTypeList();

        /// <summary>
        /// Check institution Node Mapping
        /// </summary>
        /// <param name="priceAdjustmentId">NodeId</param>
        /// <returns>Boolean</returns>
        Boolean IsNodeMapped(Int32 NodeId);

        /// <summary>
        /// Check Get Last Code From Institution Node
        /// </summary>
        /// <returns>String</returns>
        String GetLastCodeFromInstitutionNode();

        /// <summary>
        /// Method to return all Node Types of program.
        /// </summary>
        /// <returns>IQueryable</returns>
        IQueryable<InstitutionNode> GetAllInstituteNodePrograms(String code);

        #region Custom Attribute

        /// <summary>
        /// Get mapping list of custom attribute with node 
        /// </summary>
        /// <param name="customAttributeNodeId">customAttributeNodeId</param>
        /// <returns>IQueryable</returns>
        IQueryable<CustomAttributeMapping> GetNodeMappedCustomAttributeList(Int32 customAttributeNodeId);

        /// <summary>
        /// Get Custom Attribute on the basis of Use Type Code.
        /// </summary>
        /// <param name="useTypeCode">useTypeCode</param>
        /// <returns>IQueryable</returns>
        IQueryable<CustomAttribute> GetCustomAttributeListByType(String useTypeCode);

        /// <summary>
        /// get list of ids mapped with custom attribute value table.
        /// </summary>
        /// <param name="customAttributeMappingIds">customAttributeMappingIds</param>
        /// <returns>List</returns>
        List<Int32> GetListOfIdMappedWithCustomAttrValue(List<Int32> customAttributeMappingIds);

        #endregion

        #region Custom Attributes

        List<TypeCustomAttributes> GetCustomAttributes(Int32 mappingRecordId, Int32 valueRecordId, String useTypeCode, Int32 organizationUserId);

        /// <summary>
        /// Gets the Custom Attributes for the Last selected node in the hierarchy
        /// </summary>
        /// <param name="useTypeCode"></param>
        /// <param name="selectedDPMId"></param>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        List<TypeCustomAttributes> GetCustomAttributesByNodes(String useTypeCode, Int32? selectedDPMId, Int32 organizationUserId);

        List<TypeCustomAttributesSearch> GetCustomAttributesSearch(Int32 mappingRecordId, String useTypeCode);

        /// <summary>
        /// Get the Custom Attributes for multiple Nodes selected - UAT 1055
        /// </summary>
        /// <param name="mappingRecordId"></param>
        /// <param name="useTypeCode"></param>
        /// <returns></returns>
        List<TypeCustomAttributesSearch> GetCustomAttributesNodeSearch(String dpmIds, String useTypeCode);

        void SaveCustomAttributeData(List<CustomAttributeValue> lstCAValues, Int32 currentLoggedInUserId);

        #endregion

        #endregion

        #region Manage Custom Attribute

        /// <summary>
        /// Get All Client Custom Attributes
        /// </summary>
        /// <returns>List of Custom Attributes</returns>
        IQueryable<CustomAttribute> GetCustomAttributes();

        /// <summary>
        /// Add Custom Attribute
        /// </summary>
        /// <returns>Boolean</returns>
        Boolean AddCustomAttribute(CustomAttribute customAttribute);

        /// <summary>
        /// Update Custom Attribute
        /// </summary>
        /// <returns>Boolean</returns>
        Boolean UpdateCustomAttribute(CustomAttribute customAttribute);

        /// <summary>
        /// Delete Custom Attribute
        /// </summary>
        /// <returns>Boolean</returns>
        Boolean DeleteCustomAttribute(Int32 customAttributeId, Int32 userId);

        /// <summary>
        /// Checks Custom attribute is mapped or not.
        /// </summary>
        /// <returns>Boolean</returns>
        Boolean IsAttributeMapped(Int32 customAttributeId, String useTypeCode);

        #endregion

        #region Reports

        DataSet Get(int tenantId, string name, Dictionary<string, string> parameters);

        #endregion

        #region Applicant Portfolio Search && Applicant User Group Mapping

        List<ApplicantDataList> GetApplicantPortfolioSearch(SearchItemDataContract searchDataContract, CustomPagingArgsContract gridCustomPaging);

        #endregion

        #region ApplicantPortfolioCustomAttribute
        List<Int32> GetDepartmentProgramMappingId(Int32 organizationUserId);
        List<DeptProgramMapping> GetDepartmentProgramMappingRecord(List<Int32> departmentProgramMappingIds);
        #endregion

        #region User Group Mapping

        List<Int32> GetUsersMappedUserGroup(Int32 userGroupId);
        Boolean AssignUserGroupToUsers(Dictionary<Int32, Boolean> selectedItems, Int32 userGroupId, Int32 currentUserId);

        List<UserGroup> GetUserGroupsByOrgUserIDs(List<Int32> OrgUserIDs);
        Boolean AssignUserGroupsToUsers(List<Int32> userGroupIds, List<Int32> applicantUserIds, Int32 currentUserId);
        Boolean UnassignUserGroups(List<Int32> userGroupIds, List<Int32> applicantUserIds, Int32 currentUserId);

        #endregion

        List<ExpiringItemList> GetItemsAboutToExpire(Int32 tenantID, Int32 subEventId, Int32 packageSubscriptionId);

        List<CompliancePackageCategory> getCompliancePackageCategoryByDisplayOrder(Int32 packageId);
        List<ComplianceCategoryItem> getComplianceCategoryItemByDisplayOrder(Int32 categoryId);
        List<ComplianceItemAttribute> getComplianceItemAttributeByDisplayOrder(Int32 itemId);
        Boolean UpdateCategoryDisplayOrder(Int32 packageId, Int32 categoryId, Int32 displayOrder, Int32 userId);
        Boolean UpdateItemDisplayOrder(Int32 categoryId, Int32 itemId, Int32 displayOrder, Int32 userId);
        Boolean UpdateAttributeDisplayOrder(Int32 itemId, Int32 attributeId, Int32 displayOrder, Int32 userId);
        Boolean IsAnySubscriptionExist(Int32 packageId);

        void UpdateItemVerificationSummary(Int32 tenantId, Int32 backgroundProcessUserId);

        void UpdateOrderSummary(Int32 tenantId, Int32 backgroundProcessUserId);

        List<AssignmentUsers> GetUsersForAssignment(Int32? currentLoggedInUserId, Int32 currentLoggedInUserTenantId);

        List<UserNodePermissions> GetUserNodePermissions(Int32 currentLoggedInUserId, Int32 currentLoggedInUserTenantId);
        List<UserNodePermissions> GetUserNodePermissionBasedOnHierarchyPermissionType(Int32 currentLoggedInUserId, Int32 currentLoggedInUserTenantId, String hierarchyPermissionType);
        List<UserNodeOrderPermission> GetUserNodeOrderPermissions(Int32 currentLoggedInUserId);

        Entity.UtilityFeatureUsage GetUtilityFeatureUsageByUserID(Int32 orgUserId, Int16 utilityFeatureId);

        ApplicantDocumentMerging GetDataRelatedToUnifiedDocument(Int32 documentId);

        Boolean SaveUpdateUtilityFeatureUsage(Entity.UtilityFeatureUsage utilityFeatureUsage, Int32 loggedInUserId);
        Boolean UpdateUtilityFeatureUsage();
        Boolean SaveApplicantEsignatureDocument(Int32 ApplicantDisclaimerDocumentId, List<Int32?> ApplicantDisclosureDocumentId, Int32 orderId, Int32 orgUserProfileId, Int32 CurrentLoggedInUserId, String orderNumber);
        DataTable GetNagMailData(Int32 subEventId, Int32 chunkSize);

        #region NotificationForDeadline
        List<GetUserBeforeExpiringDeadline> GetAllUserBeforeExpiringDeadline(Int32 chunkSize);
        #endregion

        #region Background Packages

        /// <summary>
        /// Get the list of All the background packages, which have not been purchased by Applicant, on a given node
        /// </summary>
        /// <param name="dpmId"></param>
        /// <param name="orgainizatuionUserId"></param>
        /// <returns></returns>
        DataTable GetBackgroundPackages(String xmlDPMIds, Int32 orgainizatuionUserId, Boolean IsLocationServiceTenant);

        DataTable GetPreviousOrderHistory(Int32 tenantId, Int32 orgainizatuionUserId);
        #endregion


        DataTable GetBkgOrders(Int32 bkgOrderId, Int32 masterOrderId);

        Boolean SaveDbContext(Int32 orderId);

        /// <summary>
        /// Get HierarchyNodeID for Order By Department Program Package ID or Bkg Package Hierarchy Mapping ID
        /// </summary>
        /// <param name="dppID"></param>
        /// <param name="bphmID"></param>
        /// <returns></returns>
        Int32 GetHierarchyNodeID(Int32? dppID, Int32? bphmID);

        /// <summary>
        /// Get PackageSubscriptionID by OrderID
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns>PackageSubscriptionID</returns>
        Int32 GetPackageSubscriptionID(Int32 orderID);

        /// <summary>
        /// Copy AMS/Background Package Data in Compliance Package
        /// </summary>
        /// <param name="packageSubscriptionID"></param>
        DataTable GetPackageDocumentDataPoints(Int32 packageSubscriptionID, Int32? bkgOrderID);

        /// <summary>
        /// Call Package Data Copy SP
        /// </summary>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="docXml"></param>
        /// <param name="packageSubscriptionID"></param>
        List<Int32> PackageDataCopy(Int32 packageSubscriptionID, Int32 currentLoggedInUserId, String docXml, Int32 tenantId);

        /// <summary>
        /// Check whether Order is fresh and has Compliance Package
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="tenantId"></param>
        /// <returns>Boolean</returns>
        Boolean IsComplianceAndFreshOrder(Int32 orderID);

        /// <summary>
        /// Returns the list of the child institute node ids for corresponding hierarchynode id
        /// </summary>
        /// <returns></returns>
        List<Int32> GetChildInstituteNodeIDs(String selectedhierarchyMappingNodeIds);

        /// <summary>
        /// Save Order Result Document Mapping
        /// </summary>
        /// <param name="lstOrdResDocMap"></param>
        /// <returns>true/false</returns>
        Boolean SaveOrderResultDocMap(List<OrderResultDocMap> lstOrdResDocMap);

        /// <summary>
        /// Get OrderResultDocMap
        /// </summary>
        /// <param name="organizationUserID"></param>
        /// <param name="masterOrderID"></param>
        /// <param name="serviceGroupID"></param>
        /// <param name="bkgDataPointTypeID"></param>
        /// <returns>OrderResultDocMap</returns>
        List<OrderResultDocMap> GetOrderResultDocMapping(Int32 organizationUserID, Int32 masterOrderID, Int32? serviceGroupID, Int32 bkgDataPointTypeID);

        /// <summary>
        /// Update Order Result Document Mapping
        /// </summary>
        /// <param name="lstOrdResDocMap"></param>
        /// <returns>true/false</returns>
        Boolean UpdateOrderResultDocMap(List<OrderResultDocMap> lstOrdResDocMap, Int32 currentLoggedInUserId);

        #region GET INSTRUCTION TEXT
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        String GetInstructionTextByID(Int32 packageId, Int32 categoryId, Int32 itemId, Int32 attributeId, Int32 CIA_Id);
        #endregion
        //UAT-738
        #region GET ASSIGNED ITEM LIST BY CATEGORY

        List<Int32> GetCategoryListForAssignedItem(Int32 PackageSubscriptionID, Int32 currentLoggedInUserId);
        #endregion

        List<CustomComplianceContract> FetchSelectedSubscriptionIDs(Dictionary<int, string> OrganisationUser);

        List<PackageSubscription> GetPackageSubscription(string subscriptionIDs);

        //UAT-613 Explanatory State
        #region UAT-613
        Boolean SaveUpdateExplanatoryState(Entity.aspnet_PersonalizationPerUser explanationObjToSave);
        Entity.aspnet_PersonalizationPerUser GetExplanatoryState(Guid userId);
        #endregion

        List<ApplicantDocumentDetails> GetApplicantDocumentDetails(Int32 organizationUserID);

        #region UAT:536 Save and Update Applicant Custom Attribute
        Boolean SaveUpdateApplicantCustomAttribute(List<ApplicantCustomAttributeContract> applicantCustomAttributeContract, Int32 loggedInUserId, Int32 orgUsrID);
        #endregion

        #region UAT-523 WB: Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.

        /// <summary>
        /// To Get Whole Category Item ID
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="wholeCatGUID"></param>
        /// <param name="complianceCategoryId"></param>
        /// <returns></returns>
        ComplianceItem GetWholeCategoryItemID(Int32 tenantID, Int32 currentLoggedInUserId, Guid wholeCatGUID, Int32 complianceCategoryId);

        #endregion



        #region UAT:719 Check Exceptions turned off for a Category/Item
        Boolean IsAllowExceptionOnCategory(Int32 packageId, Int32 categoryId, List<lkpObjectType> lstlkpObjectType);
        #endregion


        #region UAT-523 Category Level Exception
        Boolean UpdateCategoryLevelExceptionData(Int32 categoryDataId, DateTime? expirationDate, Int32 catStatusId, Int16? catExceptionStatusId, Int32 itemDataId, Int32 itemId, String comments, Int32 statusId, Int32 currentLoggedInUserId, String currentLoggedInUserName, List<Int32> lstDocumentsToRemove, List<ListItemAssignmentProperties> lstAssignmentProperties, String recordActionType, String currentLoggedInUserInitials);
        #endregion


        /// <summary>
        /// UPDATE Applicant Compliance Category data. Set [CategoryExceptionStatusID] and [ExpirationDate] to null.
        /// </summary>
        /// <param name="currentLoggedInUserID"></param>
        /// <param name="complianceCategoryID"></param>
        /// <param name="packageSubscriptionID"></param>
        /// <returns></returns>
        ApplicantComplianceItemData UpdateApplicantCmpCatData(Int32 currentLoggedInUserID, Int32 complianceCategoryID, Int32 packageSubscriptionID, Int32 catStatusApprovedId);



        #region UAT - 685 : Approving Multiple Orders

        List<ScheduledTask> GetScheduledTasks(String scheduleTaskTypeCode, String taskStatusTypeCode);
        Boolean UpdateScheduleTaskStatus(Dictionary<Int32, String> dictUpdatedTaskStatus, Int32 modifiedById);
        List<ScheduledTask> GetScheduledTasksForInvoiceOrder(Int32 chunkSize = 0);

        #endregion

        /// <summary>
        /// Check wheteher exception rejected is for category
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        Boolean IsCategoryException(Int32 itemID);



        List<ApplicantDataList> GetApplicantComprehensivePortfolioSearch(SearchItemDataContract searchDataContract, CustomPagingArgsContract gridCustomPaging, String tenantIdList, Boolean IsAllTenantSelected);
        #region UAT-718 WB: As an admin, I should be able to assign items across multiple institutions.
        Boolean UpdateAssignToUserForMultipleTenant(List<Int32> itemGlobalids, Int32 currentLoggedInUserId, Int32 assignToUserId);
        #endregion

        #region UAT-573 WB: Automatic renewal for Invoice clients

        List<ScheduledTask> GetScheduledTasksForAutoRenewExpiredInvoiceSubscription();
        List<Order> GetInvoiceOrdersForAutoRenew(short archieveStateID);

        #endregion
        #region UAT-775: Creation of Service Form reminder and notification emails.

        /// <summary>
        /// To Get Is Service Form Diaptched Noification Enabled.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="code"></param>
        /// <returns>True/False</returns>        
        Boolean GetSvcFrmDisNotification(Int32 tenantId, String code);

        /// <summary>
        /// To Get Service Form Dispatched Notification Data 
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <returns>DataTable</returns>
        DataTable GetServiceFormDispatchedNotificationData(Int32 chunkSize);

        #endregion

        Boolean GetSSNSetting(Int32 tenantId, String code);

        #region UAT-845 Creation of admin override function (verification details).
        Boolean UpdateCategoryOverrideData(Int32 categoryDataId, DateTime? expirationDate, Int32 catStatusId, Int16? catOverrideStatusId,
                                                         Int32 currentLoggedInUserId, Int32 complianceCatId, Int32 packageSubscriptionId, String CategoryOverrideNotes);
        #endregion

        String ResetAutoRenewalStatus(Int32 orderID, Int32 currentUserID);

        List<PackageSubscription> GetSubscribedPackagesForUser(int tenantID, int userID, int OrderCancelled, int PartialOrderCancelStatus);

        //UAT-4067
        List<PackageSubscription> GetSelectedNodeIDByOrderID(int tenantID, int userID);
        List<PackageSubscription> GetSelectedNodeIDBySubscriptionID(Int32 selectedtenantID, Int32 packageSubscriptionID);

        NotificationDelivery GetExistingNotificationDeliveryForToday(Int32 orgUserId, Int32 entityId, Int32 subEventId, String entitySetName);

        DataTable GetScheduleTasksToProcess(String taskTypeCode, Int32 chunkSize);

        Boolean UpdateBackgroundServiceExecutionHistory(BackgroundServiceExecutionHistory backgroundServiceExecutionHistory, Int32 serviceExecutionHistoryId);

        Boolean SaveBackgroundServiceExecutionHistory(BackgroundServiceExecutionHistory backgroundServiceExecutionHistory);

        List<Int32> GetUsersToMarkApplicantDocumentsComplete(DateTime fromDate, DateTime toDate, Int32 chunkSize, Int32 lastFetchedOrgUserID);

        Boolean UpdateStatusForApplicantDocuments(List<Int32> lstUsers, Int32 currentloggedInUserId, Int16 docStatusId);

        #region Sales Force


        /// <summary>
        /// Add the object for Compliance Data upload to third party like Sales Force
        /// </summary>
        /// <param name="_tpUploadData"></param>
        void AddThirdPartyDataUpload(ThirdPartyComplianceDataUpload _tpUploadData);

        /// <summary>
        /// Updates IsDeleted = 1 for all the Applicant Subscriptions in 'ThirdPartyComplianceDataUpload' table 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="currentUserId"></param>
        void UpdateApplicantSubcriptions(Int32 organizationUserId, Int32 currentUserId);

        /// <summary>
        /// Gets the App Configuration for the tenant, based on the Key
        /// </summary>
        /// <returns>
        /// </returns>
        Entity.ClientEntity.AppConfiguration GetAppConfiguration(String key);

        #endregion

        #region UAT-749:WB: Addition of "User Groups" to left panel of Verification Details screen
        List<Entity.ClientEntity.UserGroup> GetUserGroupsForUser(Int32 organizationUserId);
        #endregion

        #region Compliance Document Search [UAT-846:WB: As a client admin, I should be able to pull specific compliance documents for a group of students.]
        DataTable GetComplianceDocumentSearch(SearchItemDataContract searchDataContract, CustomPagingArgsContract gridCustomPaging);
        #endregion

        #region UAT-968:As an ADB admin, I should be able to create/view/edit "notes" in a student's profile search details.
        DataTable GetApplicantProfileNotesList(Int32 organizationUserID, Boolean IsClientAdmin); //IsClientAdmin- UAT-5052
        DataTable GetAllowedFileExtensionsByNodeIDs(String selectedNodeIDs);//UAT-4067
        Boolean SaveApplicantProfileNotes(ApplicantProfileNote applicantProfileNoteObj);
        ApplicantProfileNote GetApplicantProfileNotesByNoteID(Int32 applicantProfileNoteID);
        Boolean UpdateApplicantProfileNote();
        Boolean SaveUpdateApplicantProfileNotes(List<ApplicantProfileNotesContract> applicantProfileNoteList);

        #endregion

        #region UAT-966:As an admin, I should be able to cancel individual parts of an order

        DataTable GetCancelledBkgOrderData(Int32 orderId);

        Boolean SavePartialOrderCancellation(String partialOrderCancellationXML, Int32 orderID, String partialOrderCancellationTypeCode, Int32 currentUserID);

        #endregion

        Dictionary<String, object> GetPartialOrderCancellationMailData(OrderPaymentDetail orderPaymentDetail, int tenantId, String packageNames);
        #region UAT-916 :WB: As an application admin, I should be able to define payment options at the package level in addition to the node level
        List<OrderPaymentDetail> GetAllPaymentDetailsOfOrderByOrderID(Int32 orderID, Boolean IncludeDeletedRecords);

        List<PaymentDetailContract> GetOrderPaymentInvoiceItemByOrderID(Int32 orderID);
        //For UAT 2379,include "payment due" status in bulk approve
        List<OrderPaymentDetail> GetOrdrPaymentDetailOfOrderByPaymentOpt(Int32 orderID, Int32 paymentOptionCodeId, Int32 invoiceWdoutAprvlPaymentOption, Int32 paymentDueOrderStatusId, Int32 creditCardWithApprovalId);
        List<OrderPkgPaymentDetail> GetOrderPkgPaymentDetailsByOrderID(Int32 orderID, String orderPackageTypeCode = null);
        Boolean CheckIsInvoiceOnlyOrderPayment(Int32 orderID);
        List<BkgOrderPackage> GetBkgOrderPackageListByBphmIds(List<Int32> listBPHM_ID);
        List<OrderPkgPaymentDetail> GetOrderPkgPaymentDetailByOPDID(Int32 orderPaymentDetailID);
        Boolean IsOrderPaymentIncludeEDSService(Int32 orderPaymentDetailID);
        OrderPaymentDetail GetOrdrPaymentDetailByID(Int32 orderPaymentDetailId);
        #endregion

        //List<Order> GetOrdersToBeArchived();
        void AutomaticallyArchiveExpiredSubscriptions(Int32 currentUserId);
        String GetApplicantAndTheirOrdersFromHierarchyIds(String hierarchyIDs, String dateFrom, String dateTo);

        #region UAT-1683 Add the Archive button and Manage Un-Archive to the Screening side
        //UAT-1683
        List<UnArchivalRequestDetails> GetUnArchivalRequestData(String SelectedSubscriptionType, String SelectedPackageType);
        List<CompliancePackageSubscriptionArchiveHistory> ApproveUnArchivalRequests(List<Int32> lstSelectedunArchivalRequestIds, Int32 currentLoggedInUserId, short changeTypeID, short dataAuditChangeTypeID);
        //UAT-1683
        List<BkgOrderArchiveHistory> ApproveBkgUnArchivalRequests(List<Int32> lstSelectedunArchivalRequestIds, Int32 currentLoggedInUserId, short changeTypeID);
        Boolean RejectUnArchivalRequests(List<Int32> lstSelectedunArchivalRequestIds, Int32 currentLoggedInUserId);
        //UAT-1683
        Boolean RejectBkgUnArchivalRequests(List<Int32> lstSelectedunArchivalRequestIds, Int32 currentLoggedInUserId);

        Boolean SaveBkgOrderArchiveHistoryData(BkgOrderArchiveHistory objectToSave, Int32 orderID);

        Boolean IsActiveUnArchiveRequestForBkgOrderId(Int32 BKgOrderId, Int16 changeTypeId);

        BkgOrder GetBkgOrderDetailByID(Int32 orderId);

        #endregion


        #region Maintain PackageSubscription Archive History [UAT-977: Additional Works for Archive access]
        Boolean SaveCompSubscriptionArchiveHistoryData(CompliancePackageSubscriptionArchiveHistory objectToSave);
        Boolean IsActiveUnArchiveRequestForPkgSubscriptionId(Int32 packageSubscriptionId, Int16 changeTypeId);

        #endregion

        #region [UAT-977:Additional work towards archive ability]
        /// <summary>
        /// Method to return the repurchased order by previous order id.
        /// </summary>
        /// <param name="prevOrderId">previous order id</param>
        /// <param name="orderRequestId">order request id</param>
        /// <returns></returns>
        Order GetRepuchasedOrderByPreviousOrderID(Int32 prevOrderId, Int32 orderRequestId);
        #endregion

        List<PackageSubscription> GetSubscriptionsToBeArchived(List<Int32> subscriptionIds, Int32 archieveStatusId);

        #region UAT-977 Manual Archival
        String ArchieveSubscriptionsManually(short archieveStatusId, Int32 currentUserId, List<CompliancePackageSubscriptionArchiveHistory> lstArchiveHistory, List<Int32> requiredSubscriptionIDs, List<ApplicantDataAudit> lstApplicantDataAudit);
        #endregion

        #region UAT-977 Get Subscription list for Manual Archival
        Dictionary<String, List<Int32>> GetSubscriptionsListForArchival(Dictionary<Int32, String> AssignOrganizationUserIds);
        #endregion

        #region UAT-977 Get Data for multiple subscription popup
        DataTable GetMultipleSubscriptionDataForPopup(String packageSubscriptionIDs, Int32 currentLoggedInUserID);
        #endregion

        #region Get Package Subscription For Data Entry [UAT-1049:Admin Data Entry]
        DataTable GetPackageSubscriptionForDataEntry(Int32 organizationUserID);
        #endregion


        #region UAT-1033 Add link to download E Drug authorization form (Electronic Service Form) to screening tab.
        DataTable GetEDSStatusForOrders(string commaDelemittedOrderIDs);
        #endregion

        #region UAT-1049 :Admin Data Entry

        Boolean SubmitAdminDataEntry(AdminDataEntrySaveContract dataEntryContract, Int32 currentUserId, List<lkpItemComplianceStatu> itemComplianceStatusList
                                     , List<lkpReviewerType> reviewerTypeList, Int32 tenantId);
        Boolean UpdateDoccumentStatusAfterDataEntry(Int32 applicantDoccumentId, short documentStatusId, Int32 currentUserId);
        Boolean UpdateDoccumentStatusInFlatTableAfterDataEntry(Int32 fdeqId, short documentStatusId, Int32 currentUserId);
        #endregion

        #region UAT-1722

        Boolean SaveAdminDataEntry(AdminDataEntrySaveContract dataEntryContract, Int32 currentUserId,
                                    List<lkpItemComplianceStatu> itemComplianceStatusList,
                                    List<lkpReviewerType> reviewerTypeList, Int32 tenantId);

        #endregion


        #region UAT-1039 : Add contextual video popups on applicant side of complio.

        String ShowDocumentVideo(Int32 currentUserId, List<Int32> lstCompliancePkgIds);

        String ShowDataEnteredVideo(int currentUserId);

        #endregion


        List<Order> GetListofOrdersForOrderID(List<Int32> orderIds);
        #region Data Entry Tracking
        void DataEntryTimeTracking(DataEntryTrackingContract dataEntryTrackingContract);
        #endregion

        #region UAT-1176 - Node Employment
        Boolean CheckEmploymentNodePermission(Int32 userID);
        #endregion

        #region UAT-1025: Update Deadline Date email notification to include category list
        List<PackageSubscription> GetPackagesDetailForDeadLineNotification(Int32 selectedNodeId, Int32 organizationUserId);
        #endregion

        #region UAT-1214:Changes to "Required" and "Optional" labels in order flow
        List<ClientSetting> GetBkgOrdFlowLabelSetting(Int32 tenantId, String requiredLabelCode, String optionalLabelCode);
        #endregion

        #region UAT-1545

        /// <summary>
        /// Get the list of Client settings by Codes.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="lstCodes"></param>
        /// <returns></returns>
        List<ClientSetting> GetClientSettingsByCodes(Int32 tenantId, List<String> lstCodes, String languageCode);

        #endregion

        DataTable GetOrderApprovalQueueData(OrderApprovalQueueContract searchItemDataContract, CustomPagingArgsContract customPagingArgsContract, Boolean isBkgScreen);

        DataTable GetOrderDetails(String OrderID);

        #region UAT-1189:WB: Bulk Archive capability
        DataTable GetApplicantSubscriptionDataBulkArchive(String applicantXmlData, Int32? curentLoggedInUserID);
        DataTable GetUnMatchedApplicantDetails(String applicantXmlData);

        #endregion

        #region UAT-1234: WB: spreadsheet upload to see if applicants have created accounts or ordered
        DataTable GetUploadedDocumentApplicantOrders(String applicantXmlData, String orderPkgTypes, Int32? curentLoggedInUserID, CustomPagingArgsContract customPagingArgsContract);

        #endregion

        #region [UAT-1245:If payment method is the same, both tracking and screening are getting cancelled when the applicant attempts to cancel the tracking order.]
        /// <summary>
        /// Method to return the order by previous order id.
        /// </summary>
        /// <param name="prevOrderId">previous order id</param>
        /// <param name="orderRequestId">order request type code</param>
        /// <returns></returns>
        Order GetOrderByPreviousOrderID(Int32 prevOrderId, List<Int32> orderRequestTypeIDList);

        #endregion

        #region UAT 1230:WB: As an admin, I should be able to invite a person (or group of people) to create an applicant account

        Boolean IsOrganisationUserExistByEmail(string email);
        List<OrganizationUser> GetOrganisationUsersByEmail(List<String> lstEmail);

        #endregion

        #region  UAT 1304 Instructor/Preceptor screens and functionality
        ClientSystemDocument GetClientSystemDocument(Int32 clientSystemDocID);
        #endregion

        #region UAT 1080 Addition of notification for orders that are sent for online payment

        List<usp_GetIncompleteOnlineOrders_Result> GetIncompleteOnlineOrders(Int32 chunkSize, Int32 maxRetryCount, Int32 retryTimeLag);
        void SaveUpdateOrderNotifications(Int32? orderNotificationID, Int32 backgroundProcessUserId, Int32 orderID, Int32? systemCommunicationId, Int16 businessChannelTypeId, Int32 orderNotificationTypeID, string notificationDetail);

        #endregion

        #region UAT 1438: Enhancement to allow students to select a User Group.
        List<ApplicantUserGroupMapping> GetApplicantUserGroupMappingForUser(Int32 LoggedInUserID);

        Boolean SaveUpdateApplicantUserGroupCustomAttribute(List<ApplicantUserGroupMapping> lstApplicantUserGroupMapping_Added, Int32 loggedInUserID);

        Boolean IsUserGroupCustomAttributeExist(List<Int32> lstHierarchyNodeIds, Int32 loggedInUserID);
        #endregion

        Dictionary<Int32, String> GetCategoryListFilterForReport(Int32 tenantID, String nodeIds);

        Dictionary<Int32, String> GetItemListFilterForReport(Int32 tenantID, String selectedCategoryIds);

        Dictionary<Int32, String> GetHierarchyListFilterForReport(Int32 tenantID, Int32 userID);

        Dictionary<Int32, String> GetUserGroupListFilterForReport(Int32 tenantID);

        Boolean GetOptionalCategorySettingForCompliancePackage(Int32 HierarchyNodeID, Int32 SubscriptionID);

        #region bulletin

        /// <summary>
        /// To get data for binding the grid based on institution Id's.
        /// </summary>
        /// <param name="selectedInstitutionIds"></param>
        /// <param name="selectedHieararchyIds"></param>
        /// <returns></returns>
        List<BulletinContract> GetBulletin(String selectedInstitutionIds, String selectedHieararchyIds);

        /// <summary>
        /// Add and update institution hierarchy id's.
        /// </summary>
        /// <param name="bulletinId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="lstDpmIds"></param>
        void AddUpdateBulletinNodeMapping(Int32 bulletinId, Int32 currentUserId, List<Int32> lstDpmIds);


        /// <summary>
        /// Method to delete the institution hierarchy corresponding to any Bulletin Id.
        /// </summary>
        /// <param name="bulletinId"></param>
        /// <param name="currentUserId"></param>
        void DeleteBulletinNodeMapping(Int32 bulletinId, Int32 currentUserId);

        #endregion

        #region Bulletins Popup
        List<Int32> GetBulletins(Int32 tenantId, Int32 currentUserId);

        #endregion

        #region UAT-1254
        /// <summary>
        /// Get AMS/Background Package Data points exis or not.
        /// </summary>
        /// <param name="packageSubscriptionID"></param>
        /// <returns>DataTable</returns>
        DataTable GetBkgCompDataPointMappingExistOrNot(Int32? packageSubscriptionID, Int32? bkgOrderID);
        #endregion

        #region UAT-1538:Unified Document/ single document option and updates to document exports
        Entity.UtilityFeatureUsage GetDocumentViewTypeSettingByUserID(Int32 orgUserId, List<Int16> utilityFeatureIds);
        #endregion

        //UAT-1558
        Int32 UpdateCompPkgGraduationStatus(Int32 orderId, Int16 graduatedId, Int32 currentUserId, short dataAuditChangeTypeID, String graduatedCode);
        Boolean UpdateIsGraduatedBkgPkg(Int32 BkgOrderId, Int16 graduatedId, Int32 currentUserId);
        List<PackageSubscription> GetActiveSubscriptionListForUser(Int32 currentUserID);

        Entity.ClientEntity.ClientSystemDocument GetClientSystemDocumentByComplianceAttributeID(int complianceAttributeID);

        ApplicantDocument GetApplicantDocumentByApplAttrDataID(int applAttributeDataId);

        #region UAT-1607:Student Data Entry Screen changes
        /// <summary>
        /// Method return all item series for complience category.
        /// </summary>
        /// <param name="compCategoryId">Compliance Category ID </param>
        /// <returns>List of Item Series</returns>
        List<ItemSery> GetItemSeriesForCategory(Int32 compCategoryId);

        /// <summary>
        /// Get Item Series by id
        /// </summary>
        /// <param name="itemSeriesId">itemSeriesId</param>
        /// <returns>Item series</returns>
        ItemSery GetItemSeriesByID(Int32 itemSeriesId);
        List<ItemSeriesItemContract> GetItemSeriesItemForCategories(List<Int32> categoryIds);
        String SaveSeriesAttributeData(Int32 packageSubscriptionID, Int32 itemSeriesID, Int32 currentLoggedInUserID, String seriesAttributeXML,
                                                    String documentsXML, String calledFrom, Int32 orgUserId, String notes);

        #endregion

        #region UAT-1597
        List<PackageSubscription> GetPackageSubscriptionListByIDs(List<int> subscriptionIds, Boolean isResetContext);
        DataTable GetPackageSubscriptionDataForNotification(String pkgSubscriptionIDs);
        #endregion

        #region UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
        /// <summary>
        /// Method to get Address lookup data on the basis of AddressHandleID
        /// </summary>
        /// <param name="addressHandleID">addressHandleID</param>
        /// <returns>vw_AddressLookUp</returns>
        vw_AddressLookUp GetAddressLookupByHandlerId(Guid addressHandleID);


        /// <summary>
        /// Method to update the already existing order for applicant Completing Order Process.
        /// </summary>
        /// <param name="applicantOrder">Existing Order</param>
        /// <param name="applicantOrderDataContract"> applicant Order Data Contract </param>
        /// <param name="orgUserID">Organization USER ID</param>
        /// <param name="compliancePackages">Compliance Package list</param>
        /// <returns></returns>
        Dictionary<String, String> UpdateApplicantCompletingOrderProcess(Order applicantOrder, ApplicantOrderDataContract applicantOrderDataContract,
                                                                                out String paymentModeCode, Int32 orgUserID, out List<Int32> newlyAddedOPDIds,
                                                                                List<OrderCartCompliancePackage> compliancePackages = null, Boolean isLocationTenant = false);

        /// <summary>
        /// Method to update the already existing order for applicant Modify Shipping Process.
        /// </summary>
        /// <param name="applicantOrder">Existing Order</param>
        /// <param name="applicantOrderDataContract"> applicant Order Data Contract </param>
        /// <param name="applicantOrderCart">applicantOrderCart</param>
        /// <param name="orgUserID">Organization USER ID</param>
        /// <param name="compliancePackages">Compliance Package list</param>
        /// <returns></returns>
        Dictionary<String, String> UpdateApplicantModifyShippingProcess(Order applicantOrder, ApplicantOrderDataContract applicantOrderDataContract, ApplicantOrderCart applicantOrderCart,
                                                                                 out String paymentModeCode, Int32 orgUserID, out List<Int32> newlyAddedOPDIds,
                                                                                 List<OrderCartCompliancePackage> compliancePackages = null, Boolean IsModifyShipping = false);

        /// <summary>
        /// Get all package ids purchased by applicant those are "sent for online payment".
        /// </summary>
        /// <param name="organizationUserId">organizationUserId</param>
        /// <returns>List of Compliance Package IDs</returns>
        List<Int32> GetAppCompPackageSentForOnlinePayment(Int32 organizationUserId);


        #endregion

        Boolean GetBkgOrderNoteSetting(Int32 tenantId, String code);
       //UAT:4522

        List<GranularPermission> GeNewtGranularPermission(Int32 CurrentLogedInUser);

        #region UAT-1581:Everett Enhancement Request: Turn off non-data sync doc going to data entry by tenant
        ClientSetting GetClientSetting(Int32 tenantId, String settingCode, String _languageCod);
        #endregion

        BackgroundServiceExecutionHistory GetLastSuccessfullExecutionHistory(int tenantID, string serviceName);

        #region UAT-1560 : We should be able to add documents that need to be signed to the order process
        DataTable GetAdditionalDocumentSearch(SearchItemDataContract searchDataContract, CustomPagingArgsContract gridCustomPaging);

        //added Dictionary<Int32, Int32> dicAppDocWithSysDoc in UAT-4558
        List<ApplicantDocument> UpdateApplicantAdditionalEsignatureDocument(List<Int32?> applicantAdditionalDocumentId, Dictionary<Int32, Int32> dicAppDocWithSysDoc, Int32 orderId, Int32 orgUserProfileId,
                                                                            Int32 CurrentLoggedInUserId, Boolean needToSaveMapping, Int16 recordTypeId
                                                                            , Int16 dataEntryDocCompletedStatusID, List<Int32?> additionalDocumentSendToStudent, List<SystemDocBkgSvcMapping> lstSystemDocBkgSvcMapping = null);

        List<ApplicantDocument> UpdateAdditionalDocumentStatusForApproveOrder(Int32 orderId, Int32 currentloggedInUserId, String docTypeCode, Int16 dataEntryDocNewStatusID,
                                                                    String recordTypeCode, Int32 orgUserId);

        #endregion

        #region Invoice Group

        List<DeptProgramMapping> GetDeptProgramMappingList();

        #endregion

        #region UAT 1711: Auto Multi-Review
        Tuple<List<ReconciliationDetailsDataContract>, List<ApplicantItemVerificationData>> GetApplicantReconciliationDataForVerification(Int32 complianceItemId, Int32 complianceCategoryId, Int32 packageSubscriptionId);
        #endregion

        List<INTSOF.UI.Contract.QueueManagement.ItemReconciliationAvailiblityContract> GetItemReconciliationAvailiblityStatus(Int32 tenantId, String itemIDs, Int32 subscriptionId);

        #region UAT-1758
        String GetOvralCompStatusFromClientSetting(Int32 ClientID, Int32 SettingId);
        #endregion

        #region UAT-963: WB: As an ADB admin, I should be able to search one, many, or all institutions on the admin data audit history search
        /// <summary>
        /// Method to return last Synchronised Applicant data audit record id.
        /// </summary>
        /// <returns></returns>
        ApplicantDataAuditSyncHistory GetLastSynchedAuditDataRecord();
        /// <summary>
        /// Method to sync applicant audit data to multi tenant table of shared data base
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="lastSynchedRecordId">lastSynchedRecordId</param>
        /// <param name="systemUserId">systemUserId</param>
        void SyncApplicantAuditDataForTenant(Int32 tenantId, Int32? lastSynchedRecordId, Int32 systemUserId, Int32 chunkSize);

        #endregion

        void SyncReconcillationQueueData(Int32 tenantId, Int32 systemUserId);

        #region UAT 1843: Phase 2 5: Combining User group mapping, archive and rotation assignment screens
        List<StudentBucketAssignmentContract> GetStudentBucketAssignmentSearch(SearchItemDataContract searchDataContract, CustomPagingArgsContract customPagingArgsContract);
        List<Int32> GetComPkgSubArchiveHistoryIds(List<Int32> orgUserIds, Int32 UnArchiveId);
        List<Int32> GetbkgOrderArchiveHistoryIds(List<Int32> orgUserIds, Int32 UnArchiveId);
        #endregion

        #region UAT-1833: NYU Migration 1 of 3: Batch Upload Admin Ordering

        List<BulkOrderUploadContract> UploadBulkOrdersData(String applicantXmlData, String filePath, Int32 curentLoggedInUserID, String orderDataSourceCode);

        #endregion

        #region UAT-2697
        List<BulkOrderUploadContract> UploadBulkRepeatedOrdersData(String applicantXmlData, String filePath, Int32 curentLoggedInUserID, String orderDataSourceCode);
        #endregion

        #region UAT-1812:Creation of an Approval/rejection summary for applicant logins
        /// <summary>
        /// Method to Save Series item rejected status in ApplComplianceItemStatusHistory.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        void SaveSeriesRejectedItemStatusHistory(List<ApplicantComplianceItemData> lstItemDataId, Int32 loggedInUserID, Int32 itemRejectedStatusID);
        #endregion

        //UAT-1852 : If a Service Group is complete, but other service groups are not complete within an order, the system should send the applicant an email.
        Boolean DeleteNotificationDeliveryListIfExist(NotificationDelivery notificationDelivery, Int32 currentLoggedInID);

        List<CustomAttribute> GetCustomAttributesByTenantID(int tenantID);

        // UAT-1834:NYU Migration 2 of 3: Applicant Complete Order Process      
        BulkOrderUpload GetBulkOrderForApplicant(int applicantOrgUserID);

        CustomAttribute GetCustomAttribute(int customAttributeID);

        List<BkgOrderPackage> GetBkgOrderPackagesByBkgOrderId(int bkgOrderId);

        //UAT-1835, NYU Migration 3 of 3: Automatic Interval Searching.
        OrganizationUserProfile GetOrganizationUserProfileByID(Int32 currentOrgUserId);

        List<BulkOrderUpload> GetBulkOrderDataForIntervalSearch(Int32 _recordChunkSize);

        #region UAT-2028:Expired items should also show in the Enter Requirements item selection dropdown on the student screen

        /// <summary>
        /// Return A tuple with all available items and mapped items
        /// </summary>
        /// <param name="packageId">packageId</param>
        /// <param name="categoryId">categoryId</param>
        /// <param name="currentUserId">currentUserId</param>
        /// <param name="currentSelectedItem">currentSelectedItem</param>
        /// <param name="needToGetExpireAllItems">needToGetExpireAllItems</param>
        /// <param name="expiringItem"></param>
        /// <returns></returns>
        Tuple<List<ComplianceItem>, List<Int32>, List<Int32>> GetAllItemsForDataEntry(Int32 packageId, Int32 categoryId, Int32 currentUserId, Int32 itemStatusId_Expired, Int32 currentSelectedItem = 0, Boolean needToGetAllItems = false, List<Int32> expiringItem = null);
        #endregion

        void UpdateScheduleTask(Int32 ScheduleTaskID); //UAT-4364
        List<CompliancePackageCategory> GetPackageListByCategoryId(Int32 categoryId);

        PackageSubscription GetSubscriptionDetail(Int32 packageId, Int32 orgUserId, Int32 subscriptionMobilityStatusID, Int32 archieveStateId);

        List<ApplicantDocument> GetAdditionalDocNeedToSendToStudent(Int32 orderId, String docTypeCode, String recordTypeCode);

        Boolean CheckIfDocumentsAlreadySent(Int32 orderId, Int32 notificationTypeId);

        #region UAT-1831, Addition of details on cancellations and packages listed on the portfolio detail page
        List<vwOrderDetail> GetOrderDetailListByOrgUserID(int currentOrgUserId);

        List<CancelledBkgCompliancePackageContract> GetCancelledPackageByOrderID(int orderID);
        #endregion

        #region UAT-2218, Creation of a "Required Documents" tab on the left side of the student dashboard.
        List<ApplicantRequiredDocumentsContract> GetRequirementDocumentsDetails(Int32 orgUserId);
        #endregion
        #region UAT-3161
        List<ApplicantRequiredDocumentsContract> GetRotReqDocumentsDetails(Int32 orgUserId);
        #endregion

        List<ApplicantDocumentDetails> GetApplicantPersonalDocumentDetails(Int32 organizationUserID);

        List<CustomAttribute> GetProfileCustomAttributesByTenantID(Int32 tenantID, Int32 dataTypeId);

        Boolean AddUpdateProfileCustomAttributes(List<TypeCustomAttributes> customAttributeListToUpdate, Int32 applicantId, Int32 currentUserId);

        #region UAT-2456:Admin Data Entry: "Discard Documents" should have to go through the process twice
        Boolean UpdateDocumentDiscardCount(Int32 fdeqId, Int32 currentUserId, Boolean isDocumentFirstTimeDiscarded);
        #endregion

        #region UAT-2495
        List<Entity.SharedDataEntity.ClientDataUploadConfiguration> GetClientDataUploadConfiguration(Int32 clientDataUploadId);
        DataSet GetTenantNodeMappingData(Int32 clientDataUploadId);
        Boolean CreateClientDataUploadServiceHistory(Entity.SharedDataEntity.ClientDataUploadServiceHistory clientDataUploadServHistory);
        Dictionary<String, String> GetDataToUpload(Int32 Chunksize, String NodeIds, String SPName);
        /// <summary>
        /// Method to update wait until of Client Upload service
        /// </summary>
        /// <param name="clientDataUploadId">clientDataUploadId</param>
        /// <param name="frequency">frequency in minutes</param>
        /// <param name="tenantId">tenantId</param>
        /// <returns></returns>
        Boolean UpdateClientDataUploadService(Int32 clientDataUploadId, Int32 frequency, DateTime processStartdate);
        #endregion

        //UAT-2494
        List<ClientSettingCustomAttributeContract> GetCustomAttributesWithClientSettingmapping();

        void AddUpdateCustomAttributeClientSetting(List<ClientSettingCustomAttributeContract> lstClientSettingCustomAttributeContract, Int32 loggedInUser);

        List<ClientSettingCustomAttributeContract> GetClientSettingCustomAttribute();

        #region Production Issue: Data Entry[26/12/2016]
        Boolean IsDiscardDocumentEmailNeedToSend(Int32 DocumentId, Int32 DiscardReasonId, Int32 applicantID, Int32 currentLoggedInUserId);

        #endregion

        #region UAT-2460
        List<Int32> GetNextAndPrevPackageSubscriptionIds(Int32 CurrentPackageSubscriptionID, String SelectedArchiveStateCode, Int32? OrganizationUserID = null);
        #endregion

        List<ApplicantDocument> GetApplicantDocuments(List<Int32> lstApplicantDocumentIds);
        #region UAT-2610
        Boolean RemvPndgItmExpRequestAftrCatApproval(Int32 ApplicantComCatId, Int32 CurrentUserID);
        #endregion

        #region Get Tenant Ids for Client Data Upload
        List<Int32> GetTenantForClientDataUpload();
        //UAT:4473
        List<Int32> GetTenantDataBasedClientDataUpload(Int32 clientDataUploadId);
        #endregion
        //UAT 2680
        Boolean SaveComplianceSearchNote(ComplianceSearchNotesContract complianceSearchNotesContract);
        ComplianceSearchNotesContract GetComplianceSearchNote(ComplianceSearchNotesContract complianceSearchNotesContract);

        #region UAT-2618:If a document has ever been associated with any item within a tracking subscription, the refund functionality on the order detail screen should be grayed out
        Boolean IsGrayedOutRefundFunctionality(Int32 orderId, Int32 OPD_ID);
        Boolean UpdateIsDocAssociated(Int32 packageSubscriptionID, Boolean isDocAssociated, Int32 currentLoggedInuserID);

        #endregion

        List<UpcomingExpirationContract> GetUpcomingExpiration(String hierarchyIDs, String categoryIDs, String itemIDs, DateTime? dateFrom, DateTime? dateTo, String userGroupIDs, CustomPagingArgsContract customPagingArgsContract, Boolean IsClientAdminLoggedIn, Int32 CurrentLoggedInUserId);


        String GetApplicantNameByApplicantId(Int32 applicantId);
        //UAT 2727
        Boolean IsExistClientPieChartTColorSetting(Int32 tenantId, Int32 SettingId, String ColorCode);

        //UAT-2842
        List<Order> GetOrdersByIds(List<Int32> orderIds);


        Boolean InsertAutomaticInvitationLog(Int32 tenantId, Int32 orderID, Int32 currentLoggedInID, Int32 paidStatusID);//UAT-2388

        #region "UAT - 2802"
        Boolean IsExistingNodeSelected(Int32 tenantId, Int32 currentLoggedInID, Int32 currentSelectedNodeId, Int32 OrderStatusId);//UAT-2802

        Boolean IsClientOrderFlowMessageSetting(Int32 tenantId, String SettingCode);//UAT-2802
        #endregion

        #region UAT-2971

        List<ApplicantData> GetSupportPortalSearchData(SearchItemDataContract searchContract, String selectedTenantIds, CustomPagingArgsContract gridCustomPaging, Int32 currentLoggedInUserId);
        List<BkgOrderQueueNotesContract> GetSupportPortalBkgOrderNotes(Int32 applicantOrganizationUserID);
        Boolean SaveSupportPortalBkgOrderNotes(BkgOrderQueueNote supportPortalBkgOrderNotesToSave);
        BkgOrderQueueNote GetSupportPortalBkgOrderNotesByNoteID(Int32 supportPortalNoteId);
        Boolean UpdateSupportPortalBkgOrderNotes();

        List<SupportPortalOrderDetailContract> GetSupportPortalOrderDetail(Int32 OrganizationUserId);
        List<Int32> GetBkgOrderArchiveHistoryIds(Int32 bkgOrderId, Int32 changeType);
        List<Int32> GetPkgSubArchiveHistoryIds(Int32 packageSubscriptionId, Int32 changeType);
        List<Int32> GetPkgSubArchiveHistoryIds(List<Int32> packageSubscriptionIds, Int32 changeType);
        Entity.ClientEntity.OrganizationUser GetOrganizationUserByUserID(String userId, Boolean isApplicant);
        #endregion

        #region UAT-2697:New NYU Bulk Upload Feature
        List<BulkOrderUpload> GetBulkOrderDataForRepeatedSearchOrder(Int32 _recordChunkSize);
        List<Int32> CreateBulkOrderForRepeatedSearch(String bulkOrderUploadIDs, Int32 currentLoggedInUserID, Int32 tenantID);
        Boolean IsApplicantGraduated(Int32 applicantId, Int32 archiveStateGraduatedId, Int32 archiveStateArchivedAndGraduatedId);
        #endregion

        // Boolean AutomaticAssigningItemsToUsers(Int32 tenantId, Int32 currentLoggedInUserId, List<AutoAssignItemsToUserContract> adminUsersBucketList); //UAT-2310
        List<AutoAssignItemsToUserContract> GetAdminUsersBucketDetails(String SelectedTenantXML); //UAT-2310
        List<AutoAssignItemsToUserContract> AutomaticAssigningItemsToUsers(Int32 currentLoggedInUserId, String adminUsersBucketListxml, String verificationQueueDataXmlData, CustomPagingArgsContract verificationGridCustomPaging, String customHTML, String DPMID); //UAT-2310
        List<AutoAssignItemsToUserContract> AutomaticAssigningItemsToUsersFromAllClientAssignmentQueue(Int32 currentLoggedInUserId, String adminUsersBucketListxml, AutoAssignItemsToUserListContract AutoAssignContract); //UAT-2310


        DataSet GetApplicantAndAgencyUserListForFallOutOfCompNotification(Int32? reqSubsId, Int32? compSubsID);

        #region UAT-3009
        List<Int32> GetApplicantComplianceItemIdList(Int32 packageSubscriptionId, Int32 complianceCategoryId);
        #endregion

        #region UAT-3075
        List<CompliancePriorityObjectContract> GetCompObjMappings();
        List<CompliancePriorityObjectContract> GetCategoryItems();
        Boolean SaveCompObjMapping(CompliancePriorityObjectContract compObjMapping, Int32 currentLoggedInUserId);
        Boolean DeleteCompObjMapping(Int32 compObjMappingID, Int32 currentLoggedInUserId);
        #endregion
        #region [UAT-3077: (1 of ?) Initial Analysis and begin Dev: Pay per submission item type (CC only) for Tracking and Rotation]
        ItemPaymentContract CreateItemPaymentOrder(ItemPaymentContract itemPaymentContract, List<lkpObjectType> lstObjectType);
        OrganizationUserProfile GetOrganizationUserProfileByUserProfileID(Int32 OrganizationUserProfileID);
        List<ItemPaymentContract> GetItemPaymentDetailBySubscriptionId(Int32 subscriptionID, Boolean isRequirementPackage);
        String GetOrderNumberByOrderID(Int32 orderID);
        #endregion

        #region UAT-3084
        List<RejectedItemListContract> GetRejectedItemListForReSubmission(Int32 orgUserID);
        List<ApplicantComplianceItemData> GetApplicantComplianceItemDataByIDs(List<Int32> lstApplicantComplianceItemDataIds);
        //Boolean UpdateApplicantComplianceItemDataList(List<ApplicantComplianceItemData> lstApplicantComplianceItemData);
        Tuple<Boolean, Int32> CheckItemPayment(Int32 entityID, Int32 paidOrderStatusID, Int32 itemId, Boolean isRequirement);

        ComplianceSaveResponse SaveApplicantDataForReSubmission(ApplicantComplianceCategoryData categoryData, ApplicantComplianceItemData itemData, List<ApplicantComplianceAttributeData> applicantData,
            Int32 currentUserId, String categoryComplianceStatus, Int32 CompliancePackageID, AssignmentProperty assignmentProperty, Int32 packageSubscriptionId,
            List<lkpItemMovementType> lstItemMovementTypes, List<lkpObjectType> lstObjectTypes, Boolean isDataEntryForm, Int32 OrgUsrID, Int32 tenantId);

        #endregion

        #region UAT 3075
        /*List<AutoAssignItemsToUserContract> CheckADBUserTenantMappingExistForAutoAssignment(List<AutoAssignItemsToUserContract> adminUsersBucketList, Int32 tenantId);*/
        /*List<AutoAssignItemsToUserContract> GetAdminBucketOrderByNoOfMappedTenant(List<AutoAssignItemsToUserContract> adminUsersBucketList, String inputXML, List<Int32> tenantIds);*/
        /*List<Int32> GetMultipleTenantOrderByNoOfMappedUsers(List<AutoAssignItemsToUserContract> adminUsersBucketList, List<Int32> tenantIds);*/


        #endregion

        List<BadgeFormNotificationDataContract> SaveBadgeFormNotificationData(String applicantComplianceItemDataIDs, String applicantRequirementItemDataIDs, String PSI_IDs, Int32 currentOrgUserID);

        List<Int32> FilterComplianceDataItemsByStatusCode(string dataIds, string statusCode);

        Int32 GetPackageTypeIDByCode(string code);

        Int32 GetBagdeFormFieldTypeIDByCode(string code);

        #region UAT-3083
        Dictionary<String, String> GetSubscriptionIDByOrderIdForItmPaymt(Int32 OrderID);
        #endregion

        #region UAT-3097:Add SMS notification for Student out of compliance for rotation
        OrganizationUser GetOrganizationUserByOranizationID(Int32 organizationUserId);
        #endregion

        #region UAT-3112

        List<BadgeFormNotificationDataContract> GetBadgeFormNotificationData(Int32 chunkSize, Int32 currentLoggedInUser);
        Boolean UpdateBadgeFormNotificationData(List<BadgeFormNotificationDataContract> lstBadgeFormNotificationDataContract, Int32 currentLoggedInUser);
        Boolean UpdateBadgeFormNotificationDataComments(List<BadgeFormNotificationDataContract> lstBadgeFormNotificationDataContract, Int32 currentLoggedInUser);
        //List<Int32> GetSystemDocumentsMapped(Int32 ItemID, String ItemCode);
        List<Int32> GetSystemDocumentsMapped(Int32 ItemID);
        //List<SystemDocument> GetSystemDocumentsMapped(Int32 ItemID, String ItemCode);
        //List<BadgeFormSystemDocField> GetSystemDocFieldsMapped(Int32 SystemDocId);
        Boolean SaveBadgeFormApplicantDocument(Int32 currentLoggedInUserId, List<BadgeFormNotificationDataContract> lstBadingFromNotificationData);
        List<BadgeFormDocumentDataContract> GetBadgeFormDocumentData(String badgeFormNotificationIds);
        void ResetClientEntity();
        #endregion

        void ExecuteOptionalCategoryRule(Int32 currentUserId, Int32 nodeID);  //UAT 3106 //UAT 3683 - added 1 parameter - nodeID

        List<BkgCopyPackageDataContract> GetBkgCopyPackageData(Int32 chunkSize, Int32 currentUserID);

        void UpdateSubscriptionIDInBkgCopyPackageData(Int32 packageSubscriptionID, Boolean IsRecordProcessed, Boolean IsNeedToUpdateRetryCount, String errorMessage, Int32 currentUserID);
        Boolean InsertRecordInBkgCopyPackageData(Int32 packageSubscriptionID, String docXml, Int32 currentLoggedInUserId);
        RequirementPackageSubscription GetRequirementPackageSubscriptionForInstructorPreceptor(Int32 RotationID, Int32 InstructorPreceptorOrgID);//UAT-3338

        DataTable GetDataEntryQueueData(CustomPagingArgsContract gridCustomPaging, Int32? CurrentLoggedInUserID, String institutionHierarchyIds); //UAT-3354

        //UAT-3466
        String GetNotCompliantRequirementCategoryNames(Int32? SubsID, Boolean IsCompPkgSub);

        #region UAT-3348
        List<CommunicationSettingsSubEventsContract> GetCommunicationCopySubEventSetting(Int32 communicationCopySettingID);
        #endregion

        //UAT-3528
        List<ReqPkgSubscriptionIDList> GetReqPkgSubscriptionIdList(RequirementVerificationQueueContract searchDataContract, Int32 CurrentReqPkgSubscriptionID, Int32 ApplicantRequirementItemID);
        //uat-4461
        List<ReqPkgSubscriptionIDList> GetApplicantDataByRPSid(Int32 CurrentReqPkgSubscriptionID, Int32 ClinicalRotationId);
        #region UAT-3563
        void UniversalAttributeMapping(Int32 complianceAttributeID, Int32 currentLoggedInUserId, Int32 universalMappingTypeID);
        #endregion



        #region UAT-3593
        Boolean IsReqDocumentAlreadyUploaded(String documentName, Int32 documentSize, Int32 organizationUserId);//, List<lkpDocumentType> docType);
        List<ApplicantDocumentDetails> GetInstructorRequirementDocumentData(Int32 organizationUserID);
        #endregion

        #region UAT-3365
        Order GetExistingOrderList(Int32 packageId, Int32 orgUserId, List<Int32> IgnoredOrderStatusList);
        #endregion

        //UAT-3805
        List<RequirementPackageSubscription> GetRequirementPackageSubscriptionBySubscriptionIds(List<Int32> lstPackageSubscriptionIds);
        //UAT-3805
        List<RequirementPackageSubscription> GetRequirementPackageSubscriptionByApplicantComplianceItemIds(List<Int32> lstApplicantComplianceItemIds);
        //String GetEntityTypeTranslatedText(Int32 entityId, String entityTypeCode);

        #region Mobile Web API

        Dictionary<String, String> SaveOrderPaymentDetail(Int32 tenantID, PkgPaymentGrouping pkgPaymentGrouping, Order order, List<lkpOrderPackageType> lstOrderPackageTypes);
        Int32 GetOrderIDByOrderNumber(String orderNumber);
        INTSOF.UI.Contract.MobileAPI.ApplicantOrderContract GetOrderNumberDetails(string orderNumber);

        #endregion

        #region UAT-3632
        Dictionary<String, String> GetItemPaymentOrderData(Int32 orderID);
        #endregion
        List<SubscriptionOption> GetDeptProgramPackageSubscriptionOptions(Int32 dppID);
        Boolean UpdateOrderStatusForInvoiceWithoutApproval(Int32 orderId, String orderStatusCode, Int32 currentLoggedInUserId, List<lkpOrderStatusType> lstOrderStatusType, Int32 tenantId, List<lkpEventHistory> lstEventHistory, Int32 orderPaymentDetailId = 0);
        List<OrderDetailsContract> GetOrderHistory(int organizationUserID); //MObile Web

        void SaveCABSServiceOrderDetails(List<BackgroundPackagesContract> lstPackages, Order applicantOrder, Int32 orgUserID, PreviousAddressContract mailingAddress = null, Boolean isLocationServiceTenant = false, Boolean isConsent = false);
        //void AddDataInXMLForModifyShipping(Order applicantOrder, ApplicantOrderDataContract applicantOrderDataContract, Int32 orgUserID, Boolean isLocationServiceTenant = false);
        string GetCreditCardPaymentModeApprovalCode(Int32 dpmId);

        #region UAT-3954
        List<String> IsOrderExistForCurrentYear(String orderIds);
        #endregion
        #region UAT-3795
        DataTable GetCCUsersForWeeklyNonComplaintReport(String communicationSubEventCode, Int32 tenantId);

        DataTable GetWeeklyNonCompliantReportData(String hierarchyId, Int32 organizationID);
        #endregion

        #region UAT-3951
        Boolean SaveRejectionReasonAuditHistory(List<Int32> lstRejectionReasonIds, Int32 itemDataId, Int32 currentLoggedInUserId);
        #endregion

        Int32? GetAgencyRootNodeUsingRotationID(Int32? rotationID); //UAT-3704

        #region UAT-4161
        Int32 GetLastRecordToBeSyncAuditData();
        List<ApplicantDataAudit> GetApplicantDataAuditRecords(Int32 chunkSize, Int32 lastSyncId);
        #endregion
        #region UAT-4114
        String SaveReconciliationQueueConfiguration(Int32 queueConfigurationID, String description, Decimal percentage, Int32 reviews, String InstutionHierarchyID, Int32 currentLoggedInID);
        Boolean DeleteReconciliationQueueConfiguration(Int32 CurrentAssignmentConfigurationId, Int32 CurrentLoggedInUserId);
        Boolean IsHierarchyNodeSettingAlreadyExists(Int32 CurrentAssignmentConfigurationId, Int32 InstitutionHierarchyID);
        #endregion

        ApplicantComplianceAttributeData GetApplicantComplianceAttributeData(Int32 applicantItemDataId, Int32 complianceAttributeID);
        ApplicantDocument GetApplicantDocumentByApplicantAttrDataID(Int32 applAttributeDataId);

        List<LookupContract> FetchFingerprintOrderKeyData(List<BackgroundOrderData> lstBkgOrderData, List<string> attributesToFetch);

        bool CheckDataEntryForRequirementPackages(Int32 reqPkgSubscriptionId, Int32 clinicalRotationId, Int32 currentUserId);
        void InsertDummyLineItemResultCopy(String lineItemIds, Int32 currentLoggedInUserId); //UAT-4498
        Int32 CheckIsDummyLineItemPkgPaid(Int32 orderId); //UAT-4498

        DateTime? GetReconciliationLastDate(); //UAT-4110
        List<RecounciliationProductivityData> GetRecounciliationProductivityData(DateTime startDT); //UAT-4110

        List<GenericDocumentMapping> GetGenericDocMapping(Int32 recordId, String applicantDocTypeCode, String recordTypeCode);//UAT-4558
        //Int32 GetReviewStatusByCode(String code);
        Boolean SaveApplicantComplianceData(List<ApplicantComplianceCategoryData> lstApplicantComplianceCategoryDatas);
        Boolean SaveContextInDb();

        #region UAT-5031
        Boolean SaveOrderPaymentInvoice(Int32 orderID, Int32 currentLoggedInUserId, Boolean modifyShipping);
        #endregion
    }
}
