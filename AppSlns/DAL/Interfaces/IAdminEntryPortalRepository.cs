using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAdminEntryPortalRepository
    {
        /// <summary>
        /// Retrieves a list of all Organization User based on OrganizationUserID.
        /// </summary>
        /// <param name="organizationUserId">The value for organization user's id.</param>
        /// <returns>
        /// OrganizationUser.
        /// </returns>
        OrganizationUser GetOrganizationUser(Int32 OrderId, Int32 organizationUserId);

        ///<summary>
        ///Retrieve Applicant Order Cart Data on the basis of OrderId
        ///</summary>
        ///<param name="OrderId">
        ///</param>
        ///<returns>
        /// Object of ApplicantOrderCart
        ///</returns>
        ApplicantOrderCart GetApplicantCartData(Int32 OrderId);
        /// <summary>
        /// Method to update the already existing order for applicant Completing Order Process.
        /// </summary>
        /// <param name="applicantOrder">Existing Order</param>
        /// <param name="applicantOrderDataContract"> applicant Order Data Contract </param>
        /// <param name="orgUserID">Organization USER ID</param>
        /// <param name="compliancePackages">Compliance Package list</param>
        /// <returns></returns>
        Dictionary<String, String> UpdateApplicantCompletingOrderProcess(Order applicantOrder, ApplicantOrderDataContract applicantOrderDataContract, out String paymentModeCode, Int32 orgUserID, out List<Int32> newlyAddedOPDIds, List<OrderCartCompliancePackage> compliancePackages = null);

        /// <summary>
        /// Saves the reference number in th order table and changes th status from Pending paymnt Approved to Paid..
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <param name="orderStatusCode">StatusCode to be updated</param>
        /// <param name="currentLoggedInUserId">CurrentLoggedInUserId</param>
        /// <param name="referenceNumber">Reference Number</param>
        /// <returns>True, if status is updated. Else false.</returns>
        Boolean UpdateOrderStatus(Int32 orderId, String orderStatusCode, Int32 currentLoggedInUserId, String referenceNumber, List<lkpEventHistory> lstEvents, List<lkpOrderStatusType> lstOrderStatusType, Int32 tenantId, Int32 orderPaymentDetailId = 0);
        /// <summary>
        /// Save changes for Package Subscription
        /// </summary>
        /// <returns>Boolean</returns>
        Boolean UpdatePackageSubscription();
        Boolean SaveDbContext(Int32 orderId);
        void CreateOptionalCategoryEntry(String packageSubscriptionIdsXML, Int32 currentUserId);
        //Int32 AddPackageSubscriptions(PackageSubscription packageSubscription);
        /// <summary>
        /// Updates IsDeleted = 1 for all the Applicant Subscriptions in 'ThirdPartyComplianceDataUpload' table 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="currentUserId"></param>
        void UpdateApplicantSubcriptions(Int32 organizationUserId, Int32 currentUserId);

        /// <summary>
        /// Add the object for Compliance Data upload to third party like Sales Force
        /// </summary>
        /// <param name="_tpUploadData"></param>
        void AddThirdPartyDataUpload(ThirdPartyComplianceDataUpload _tpUploadData);
        Boolean UpdateChanges();
        /// <summary>
        /// This stored procedure will get the list of Source and Target Subscription  IDs, and 
        /// </summary>
        /// <param name="subscriptionID"></param>
        /// <returns>List of Successfully transferred IDs(Target Subscription IDs)</returns>
        List<PackageSubscriptionList> CopyPackageData(List<SourceTargetSubscriptionList> subscriptionID, Int32 currentLoggedInUserID);
        void SaveDataSyncHistory(String subscriptionXml, Int32 currentLoggedInUSerID, Int32 tenantId);
        /// <summary>
        /// Save Order Result Document Mapping
        /// </summary>
        /// <param name="lstOrdResDocMap"></param>
        /// <returns>true/false</returns>
        Boolean SaveOrderResultDocMap(List<OrderResultDocMap> lstOrdResDocMap);
        Int32 AddApplicantDocument(ApplicantDocument applicantDocument);

        /// <summary>
        /// Update Order Result Document Mapping
        /// </summary>
        /// <param name="lstOrdResDocMap"></param>
        /// <returns>true/false</returns>
        Boolean UpdateOrderResultDocMap(List<OrderResultDocMap> lstOrdResDocMap, Int32 currentLoggedInUserId);
        Boolean UpdateDocumentPath(String newFileName, Int32 documentId);
        Boolean UpdateIsDocAssociated(Int32 packageSubscriptionID, Boolean isDocAssociated, Int32 currentLoggedInuserID);
        void UpdateApplicanDetailsClient(OrganizationUser organizationUser, Dictionary<String, Object> dicAddressData, Int32 addressIdMaster, List<Entity.ResidentialHistory> lstResendentialHistory, List<Entity.PersonAlia> lstPersonAlias);
        Dictionary<String, String> SaveApplicantOrderProcessClient(Order applicantOrder, ApplicantOrderDataContract applicantOrderDataContract, Int16 svcLineItemDispatchStatusId, out String paymentModeCode, out Int32 orderId, Int32 orgUserID, Int32? orderRequestNewOrderTypeId = null, List<OrderCartCompliancePackage> compliancePackages = null);
        Boolean SaveUpdateApplicantUserGroupCustomAttribute(List<ApplicantUserGroupMapping> lstApplicantUserGroupMapping_Added, Int32 loggedInUserID);
        Boolean SaveApplicantEsignatureDocument(Int32 ApplicantDisclaimerDocumentId, List<Int32?> ApplicantDisclosureDocumentId, Int32 orderId, Int32 orgUserProfileId, Int32 CurrentLoggedInUserId, String orderNumber);
        List<ApplicantDocument> UpdateApplicantAdditionalEsignatureDocument(List<Int32?> applicantAdditionalDocumentId, Int32 orderId, Int32 orgUserProfileId, Int32 CurrentLoggedInUserId, Boolean needToSaveMapping, Int16 recordTypeId
                                                                            , Int16 dataEntryDocCompletedStatusID, List<Int32?> additionalDocumentSendToStudent, List<SystemDocBkgSvcMapping> lstSystemDocBkgSvcMapping = null);
        List<ApplicantDocument> UpdateAdditionalDocumentStatusForApproveOrder(Int32 orderId, Int32 currentloggedInUserId, String docTypeCode, Int16 dataEntryDocNewStatusID, String recordTypeCode, Int32 orgUserId);
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
        void AddAddressHandle(Guid addressHandleId);
        void AddAddress(Dictionary<String, Object> dicAddressData, Guid addressHandleId, Int32 currentUserId, Int32 addressIdMaster, AddressExt addressExtn);
        void AddUpdateAdminEntryUserData(Entity.OrganizationUser organizationUser, OrganizationUserProfile UserProfile);

        DataTable GetDraftOrders(Int32 chunkSize, Int32 daysOld, Int32 subEventId);

        DataTable GetInvitationPendingStatusOrderForApplicant(Int32 chunkSize, Int32 daysOld, Int32 subEventId);

        bool DeleteDraftOrders(Int32 DaysOld, Int32 backgroundProcessUserId);

        bool ChangeBkgOrdersStatusCompletedToArchived(Int32 DaysOld, Int32 bkgAdminEntryOrderID, Int32 backgroundProcessUserId, List<lkpAdminEntryOrderStatu> lstlkpAdminEntryOrderStatus
                                                                                 , List<lkpEventHistory> lstlkpEventHistory);

        DataTable GetAutoArchiveTimeLineDays(Int32 chunkSize);

        DataTable GetRecentCompletedOrders(Int32 chunkSize, string EntityName, Int32 subEventId);
        String GetApplicantInviteContent(Int32 orderId);



    }
}
