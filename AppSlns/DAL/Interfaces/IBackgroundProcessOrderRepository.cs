using CoreWeb.IntsofExceptionModel.Interface;
using CoreWeb.IntsofLoggerModel.Interface;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Interfaces
{
    public interface IBackgroundProcessOrderRepository
    {
        #region Backround Order Search
        List<Entity.ClientEntity.BackroundOrderSearch> GetBackroundOrderSearchDetail(CustomPagingArgsContract gridCustomPaging, BkgOrderSearchContract bkgOrderSearchContract);
        Entity.ClientEntity.ExternalVendorBkgOrderDetail GetExternalVendorBkgOrderDetail(Int32 bkgOrderId);
        List<Entity.ClientEntity.InstitutionOrderFlag> GetInstitutionStatusColor(Int32 tenantId);
        List<Entity.ClientEntity.BkgSvcGroup> GetBackroundServiceGroup();
        List<Entity.ClientEntity.BkgOrderClientStatu> GetBkgOrderClientStatus(Int32 tenantId);
        InstitutionOrderFlag GetOrderInstitutionStatusColor(Int32 institutionStatusColorId);
        Int32 GetClientStatusByOrderId(Int32 orderId);
        Boolean UpdateOrderClientStatus(Int32 orderId, Int32 ClinetStatusId, Int32 currentLoggedInUserId);
        Boolean SaveOrderClientStatusHistory(Int32 orderId, String notes, Int32 currentLoggedInUserId);
        List<Entity.ClientEntity.BkgOrderClientStatusHistory> GetClientOrderStatusHistory(Int32 orderId);
        BkgOrder GetBkgOrderDetail(Int32 masterOrderId);
        Boolean UpdateBkgOrderArchiveStatus(Int32 masterOrderId, Boolean archiveStatus, String eventDetailNotes, Int32 loggedInUserId);
        #endregion

        #region Order Detail

        OrderDetailMain GetOrderDetailMenuItem(Int32 orderID);
        ApplicantOrderDetail GetApplicantOrderDetail(Int32 orderID, Int32 tenantID);
        Boolean UpdateOrderStatus(Int32 selectedOrderColorStatusId, Int32 orderID, Int32 selectedOrderStatusTypeId, Int32 loggedInUserId, Int32 orderPkgSvcGroupID, BkgOrderPackageSvcGroup bkgOrderPackageSvcGroup);
        List<PackageDetailsContract> GetPackageByOrderId(Int32 orderID);
        DataTable GetNotesByOrderId(Int32 orderID);
        Boolean AddNote(OrderNote orderNote);

        /// <summary>
        /// Gets the list of custom attributes for the given form id and order id along with the list of data value.
        /// </summary>
        /// <param name="masterOrderId">Master Order ID</param>
        /// <param name="customFormId">Custom Form ID</param>
        /// <returns>Contract containing list of custom Attributes along the list of data value.</returns>
        BkgOrderDetailCustomFormDataContract GetBkgOrderCustomFormAttributesData(Int32 masterOrderId, Int32 customFormId);
        OrganizationUserProfile GetOrganisationUserProfileByOrderId(Int32 masterOrderId);
        OrderServiceLineItemPriceInfo GetBackroundOrderServiceLinePriceByOrderID(Int32 orderID, List<Int32> Bkg_PkgIDs);
        List<ExternalVendorServiceContract> GetExternalVendorServicesByOrderId(Int32 orderID);
        OrderLineDetailsContract GetBkgOrderLineItemDetails(Int32 PSLI_ID);
        Boolean UpdateRecordToADBCopy(BkgOrderLineItemResultCopy bkgOrderLineItemResultCopy, Int32 currentLoggedInUserId);
        #endregion

        #region Order Notification

        /// <summary>
        /// To Get Background Order Notification Data
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <returns>DataTable</returns>
        DataTable GetBkgOrderNotificationData(Int32 chunkSize, String orderIds = null);




        /// <summary>
        /// To Get Background Order Result Completed Notification Data
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <returns>DataTable</returns>
        DataTable GetBkgOrderResultCompletedNotificationData(Int32 chunkSize);

        #region UAT-2438

        String GetBGPkgPDFAttachementStatus(Int32 hierarchyNodeID);

        #endregion

        /// <summary>
        /// To Get Service Group Result Completed Notification Data
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <returns>DataTable</returns>
        DataTable GetSvcGrpResultCompletedNotificationData(Int32 chunkSize);

        /// <summary>
        /// To Get Flagged Service Group Result Completed Notification Data
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        DataTable GetFlaggedSvcGrpResultCompletedNotificationData(Int32 chunkSize);

        /// <summary>
        /// Create Background Order Notification
        /// </summary>
        /// <param name="orderNotification"></param>
        /// <returns>OrderNotificationID</returns>
        Int32 CreateOrderNotification(OrderNotification orderNotification);

        /// <summary>
        /// Create Background Order Service Form
        /// </summary>
        /// <param name="bkgOrderServiceForm"></param>
        /// <returns>BkgOrderServiceFormID</returns>
        Int32 CreateBkgOrderServiceForm(BkgOrderServiceForm bkgOrderServiceForm);

        /// <summary>
        /// Update Background Order Notify Status
        /// </summary>
        /// <param name="bkgOrder"></param>
        /// <returns>True/False</returns>
        Boolean UpdateBkgOrderNotifyStatus(BkgOrder bkgOrd);

        #endregion

        #region Order Details for Client Admin

        List<OrderEventHistoryContract> GetOrderEventHistory(Int32 orderID);
        OrderDetailClientAdmin GetOrderDetailsInfo(Int32 orderID, Int32 tenantID);
        List<OrderServiceGroupDetails> GetServiceGroupDetails(Int32 orderID);
        OrderDetailHeaderInfo GetOrderDetailHeaderInfo(Int32 orderID);
        #endregion

        #region Custom Forms

        List<CustomFormDataContract> GetCustomFormsForThePackage(String packageId);

        List<AttributesForCustomFormContract> GetAttributesForTheCustomForm(String packageId, Int32 customFormId, string _languageCode);

        List<BkgSvcAttributeOption> GetOptionValues(Int32 attributeId);

        #endregion

        #region Assign Flag to completed

        Boolean AssignFlagToCompletedOrders(Int32 backgroundProcessUserId);

        #endregion

        #region Order Notification History

        List<OrderNotificationDetail> GetOrderNotificationHistory(Int32 orderId);
        List<OrderNotificationDetail> GetApplicantSpecificOrderNotificationHistory(Int32 orgUserID);
        List<LookupContract> GetHistoryByOrderNotificationId(Int32 orderNotificationId);
        Boolean UpdateBkgOrderServiceFormStatus(OrderNotification newOrderNotification);
        OrderNotification GetOrderNotificationById(Int32 orderNotificationId);
        #endregion

        #region E Drug Screening Form
        String GetClearStarServiceIdAndExtVendorId(List<Int32> backgroundPackageServiceId, String serviceTypeCode);
        Int32 GetDPM_IDForEDSPackage(List<Int32> backgroundPackageServiceId, String serviceTypeCode);

        String GetVendorAccountNumber(Int32 extVendorId, Int32 DPM_ID);
        List<BkgAttributeGroupMapping> GetAttributeListByGroupId(Int32 attributeGroupId);
        Entity.CustomFormAttributeGroup GetEDrugScreeningHtml(Int32 customFormId, Int32 attributeGroupid);
        Entity.ZipCode GetZipCodeObjByZipCode(String zipCode);
        String GetEDrugAttributeGroupId(String eDrugSvcAttributeGrpCode);
        List<BkgOrderPackage> GetBackgroundPackageIdListByBkgOrderId(Int32 bkgOrderId);
        CustomFormDataGroup GetCustomFormDataGroupForEDSData(Int32 bkgOrderId, String eDrugAttributegroupName);
        List<BkgAttributeGroupMapping> GetListBkgAttributeGroupMappingForEDrug(String eDrugAttributeGroupName);
        Boolean SaveCustomFormOrderDataForEDrug(List<CustomFormOrderData> lstCustomFormOrderDataObj);
        Boolean UpdateBkgOrderSvcLineItem(Int32 vendorId, Int32 bkgOrderId, String svcLineItemDisStatusCode, Int32 CurrentLoggedInUserId);
        String GetRegistrationIdAttributeName(Guid registrationIdAttributeCode, Int32 attributeGroupId);
        BkgOrder GetBkgOrderByOrderID(Int32 masterOrderId);
        Boolean UpdateChanges();
        Boolean SaveWebCCFPDFDocument(ApplicantDocument webCCfPdfDocument);
        String GetStateNameByAbbreviation(String stateAbbreviation);
        void RunParallelTaskSaveCCFDataAndPDF(INTSOF.ServiceUtil.ParallelTaskContext.ParallelTask operation, Dictionary<String, Object> dicParam, ISysXLoggerService loggerService, ISysXExceptionService exceptionService);

        /// <summary>
        /// Gets the BOPId of the EDS Package, from all the BKGPackages
        /// </summary>
        /// <param name="lstBOPIds"></param>
        /// <returns></returns>
        Int32 GetEDSBkgOrderPkgId(List<Int32> lstBOPIds);

        #endregion

        #region D & R Document Entity Mapping

        List<DRDocsMappingContract> GetDRDocumentEntityMappingList(DRDocsMappingObjectIds docsEntityMappingFilters);

        #endregion

        #region MVR Fields in Personal Information Page
        List<AttributeFieldsOfSelectedPackages> GetMVRAttriGrpID(String packageIds);
        Int32 GetCustomFormIDBYCode(String customFormCode);
        #endregion

        #region Supplement Services

        /// <summary>
        /// Gets the Supplemental Service for a particular OrderPackageServiceGroup
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderPkgSvcGroupId"></param>
        /// <returns></returns>
        List<SupplementServicesInformation> GetSupplementServices(Int32 orderId);//, Int32 orderPkgSvcGroupId);

        List<SupplementServiceItemInformation> GetSupplementServiceItem(Int32 orderId, Int32 serviceId);

        List<SupplementServiceItemCustomForm> GetListOfCustomFormsForSelectedItem(String serviceItemId);

        List<SupplementServiceItemCustomForm> GetListOfCustomFormsForSelectedServices(String packageServiceIds);

        List<AttributesForCustomFormContract> GetListOfAttributesForSelectedItem(Int32 customFormId, Int32 serviceItemId);

        /// <summary>
        /// for checking whether order contain ssn and natinal crimnal search or not.
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="IfSSNServiceExist"></param>
        /// <param name="IfNationalCriminalServiceExist"></param>
        SourceServiceDetailForSupplement CheckSourceServicesForSupplement(Int32 orderId);
        #endregion

        #region Supplement Order
        Int32 GetPaymentTypeIdForOrder(Int32 orderId);

        /// <summary>
        /// Generate Supplment order line items for a Service Group
        /// </summary>
        /// <param name="supplementOrderData"></param>
        /// <param name="paymentTypeIsInvoice"></param>
        void GenerateSupplementOrder(SupplementOrderContract supplementOrderData, Boolean paymentTypeIsInvoice, List<Int32> lstPackageServiceIds);

        #endregion

        #region Send Order Complete Mail

        OrganizationUser GetOrganisationUserByOrderId(Int32 masterOrderId);
        Int32 CheckIfOrderCompleteNotificationExistsByOrderID(Int32 masterOrderId, String notificationType, Int32? packageServiceGroupID);

        #endregion

        #region Disclosure And Release Form
        List<Entity.ClientEntity.ApplicantDocument> GetDisclosureReleaseDoc(Int32 masterOrderId);
        #endregion

        #region Show EDS Document
        Boolean IsEdsServiceExitForOrder(Int32 orderId, String serviceTypeCode);
        ApplicantDocument GetApplicantDocumentForEds(Int32 orderId, String documentTypeCode, String recordTypeCode);
        Int32 GetSvcAttributeGroupIdByCode(String eDrugSvcAttributeGrpCode);
        #endregion

        #region Show Residentail History Check

        /// <summary>
        /// Get the information for Residential History section (Attribute Group).
        /// Residential History section should not come at personal information page if no service in the selected packages required that.
        /// Returns Boolean Value. True: If residentaial History Attribute Group is mapped with Background Package Service Group, Show Residential History Section.
        /// Else returns false, Hide Residential History Section.
        /// </summary>
        /// <param name="backgorundPackagesID"></param>
        /// <returns></returns>
        List<PackageGroupContract> CheckShowResidentialHistory(List<Int32> backgorundPackagesID);

        List<Int32> GetBackGroundPackagesForOrderId(Int32 orderId);

        #endregion

        #region Get Min and Max Ocuurance Residential History

        /// <summary>
        /// Get Max Min Occurances based on the Background Packages ID. Returns a Dictionary containing Maximum of 'Min Occurance' 
        /// and Maximum of 'Max Occurances'.
        /// </summary>
        /// <param name="backgorundPackagesID"></param>
        /// <param name="attributeGrpCode"> Guid code for that Attrobute group</param>
        /// <returns></returns>
        Dictionary<String, Int32?> GetMinMaxOccurancesForAttributeGroup(List<Int32> backgorundPackagesID, Guid attributeGrpCode);

        #endregion

        #region package level instruction text for Residential History

        Dictionary<Guid, String> ShowInstructionTextForResiHistory(List<Int32> backgorundPackagesID);

        #endregion

        /// <summary>
        /// Get All Client Admins having node permission
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="hierarchyNodeID"></param>
        /// <returns>DataTable</returns>
        DataTable GetClientAdminWithNodePermission(Int32 tenantID, Int32 hierarchyNodeID);

        #region UAT-586 WB: AMS: When adding supplemental services to an order, need to be able to see what the applicant has already entered (location, alias, etc.)
        List<AttributesForCustomFormContract> GetAttributeDataListForPreExistingSupplement(Int32 groupId, Int32 masterOrderId, Int32 serviceItemId, Int32 serviceId);
        #endregion

        #region UAT-777: As an applicant, I should be able to access the service form PDFs from my complio account

        /// <summary>
        /// To Get Automatic Services Forms for an Order.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>DataTable</returns>
        DataTable GetAutomaticServiceFormForOrder(Int32 orderId);

        #endregion

        #region UAT-807: Addition of a flagged report only notification

        /// <summary>
        /// To Get Background Flagged Order Result Completed Notification Data
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <returns>DataTable</returns>
        DataTable GetBkgFlaggedOrderResultCompletedNotificationData(Int32 chunkSize);

        #endregion


        #region UAT-844 Bkg Order Review details Screen changes
        BkgOrderPackageSvcGroup GetOrderPackageServiceGroupData(Int32 orderPkgSvcGroupID);
        #endregion
        DataTable GetBkgOrderReviewQueueData(BkgOrderReviewQueueContract searchDataContract, CustomPagingArgsContract gridCustomPaging);
        List<BkgReviewCriteria> GetAllReviewCriterias();
        Boolean AreServiceGroupsCompleted(Int32 orderID, Int32? serviceGroupCompletedID);

        List<BkgAttributeGroupMapping> GetAllBkgAttributeGroupMapping();

        #region UAT- 1159 WB: Add link to all Electronic service forms where the e-drug link is on the student dashboard.

        DataTable GetAutomaticServiceFormForListOfOrders(string commaDelemittedOrderIDs);
        #endregion

        #region UAT-1177:System Updates for 613[Notification for Employment flag order]

        /// <summary>
        /// To Get Background Flagged Order Completed Employment Notification Data
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderID"></param>
        /// <returns>List of BkgOrderNotificationDataContract</returns>
        DataTable GetBkgFlaggedOrderEmploymentNotificationData(Int32 chunkSize);
        #endregion

        List<ServiceLevelDetailsForOrderContract> GetServiceLevelDetailsForOrder(Int32 orderID);

        #region UAT-1358: Complio Notification to applicant for PrintScan
        Boolean IsBkgServiceExistInOrder(Int32 orderPaymentDetailId, String bkgServiceTypeCode);
        #endregion

        #region  #region UAT-1455:Flag override should re-trigger data sync for the service group
        void RemoveDataSyncHistoryToRetriggerDataSync(Int32 PSLI_ID, Int32 currentLoggedInUserID, Boolean isPackageFlaggedOverride);
        Boolean GetBackGroundPackageFlaggedStatus(Int32 PSLI_ID);
        #endregion

        DataTable GetFlaggedEmploymentServiceGroupCompletedNotificationData(Int32 chunkSize);

        #region UAT-1648:As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
        /// <summary>
        /// Get BkgOrderPackage List for BOPIDs
        /// </summary>
        /// <param name="BOPIds">BOPIds</param>
        /// <returns>List of BkgOrderPackage</returns>
        List<BkgOrderPackage> GetBackgroundOrderPackageListById(List<Int32> BOPIds);

        /// <summary>
        /// Get Bkg Order selected service details xml
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        string GetBkgOrderServiceDetails(Int32 orderId);

        /// <summary>
        /// Gets the list of custom attributes for the given form id and order id along with the list of data value.
        /// </summary>
        /// <param name="masterOrderId">Master Order ID</param>
        /// <param name="customFormId">Custom Form ID</param>
        /// <returns>Contract containing list of custom Attributes along the list of data value.</returns>
        BkgOrderDetailCustomFormDataContract GetBkgORDCustomFormAttrDataForCompletingOrder(Int32 masterOrderId, String bopIds, Boolean isIncludeMvrData);
        #endregion


        #region UAT 1659 Notes section added on the background screening side.
        Boolean SaveBkgOrderNote(Int32 orderId, String notes, Int32 loggedInUserId);

        List<BkgOrderQueueNotesContract> GetBkgOrderNotes(Int32 orderId);
        #endregion

        List<AttributeFieldsOfSelectedPackages> GetAttributeFieldsOfSelectedPackages(string packageIds);

        #region UAT-1795 : Add D&A download button on Background Order Queue search.
        List<BkgOrderSearchQueueContract> GetAllDnADocument(List<Int32> masterOrderId);
        #endregion

        //UAT-1852 : If a Service Group is complete, but other service groups are not complete within an order, the system should send the applicant an email..
        DataTable SendingMailForBkgSvcGrpCompletion();

        #region UAT-1996:Setting to allow Client admins the ability to edit color flags
        /// <summary>
        /// To update Order color flag Status
        /// </summary>
        /// <param name="selectedOrderColorStatusId"></param>
        /// <param name="orderID"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        Boolean UpdateOrderColorFlag(Int32 selectedOrderColorStatusId, Int32 orderID, Int32 loggedInUserId);
        #endregion

        List<Entity.ClientEntity.ApplicantDocument> GetOrderAndBackgroundProfileRelatedDocuments(Int32 masterOrderId);

        Boolean UpdateOrderIntervalSearchRefOrderId(Int32 newOrderId, Int32 refOrderId, Int32 currentLoggedInUserId);

        Int32 SaveAutoRecurringOrderHistory(AutoRecurringOrderHistory autoRecurringOrderHistory);

        Boolean UpdateAutoRecurringOrderHistory(Int32 autoRecurringOrderHistoryId, DateTime? orderCompletionDate, Int32? newOrderId, String notes, Int32 currentUserId);

        #region UAT-2062:System to determine and add additional searches in supplement (SSN Trace)
        DataTable GetMatchedAdditionalSearchData(String inputAdditionalSearchXML, Int32 masterOrderId);
        #endregion

        #region UAT-2117:"Continue" button behavior
        Dictionary<String, String> CheckBackgroundOrderToUpdate(Int32 tenantId, Int32 masterOrderID, Int32 ordPackageSvcGrpID);
        #endregion

        #region UAT-2304: Random review of auto completed supplements

        Dictionary<String, String> CheckBackgroundOrderForAllSvcGroupsToUpdate(Int32 masterOrderID);
        Boolean SaveSupplementAutomationStatusAndHistory(Int32 tenantID, Int32 orderId, List<lkpSupplementAutomationStatu> supplementAutomationStatusList, Int32 bkgSvcGrpStatusTypeCompletedID, Int32 bkgSvcGrpReviewStatusTypeFirstReviewID, Int32 loggedInUserId);
        Boolean UpdateSupplementAutomationStatus(Int32 orderPkgSvcGroupID, Int32 supplementAutomationReviewedStatusID, Int32 loggedInUserId);
        Boolean RollbackSupplementAutomation(Int32 orderID, Int32 orderPkgSvcGroupID, Int32 loggedInUserId);

        #endregion

        #region UAT-2399:If there are no red lines in SSN trace results and additional searches are added by the system, add the searches automatically without need for review.
        Dictionary<Int32, Boolean> CheckSupplementAutoSvcGrpForRandonReview(Int32 tenantID, String bkgOrderPackageSvcGroupIDs);
        #endregion

        #region UAT-2319

        /// <summary>
        /// Copy AMS/Background Package Data in Compliance Package
        /// </summary>
        /// <param name="packageSubscriptionID"></param>
        DataTable GetPackageDocumentDataPoints(Int32 currentLoggedInUserId);

        /// <summary>
        ///  Call Package Data Copy SP
        /// </summary>
        /// <param name="packageSubscriptionID"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="docXml"></param>
        /// <param name="tenantId"></param>
        /// <param name="LstBkgComplianceMappingID"></param>
        /// <param name="LstItemID"></param>
        List<Int32> SyncDataForNewMapping(Int32 packageSubscriptionID, Int32 currentLoggedInUserId, String docXml, Int32 tenantId, String BkgCompliancePackageMappingIDs, String ItemIDs, String MasterOrderIDs);

        #endregion
        //UAT-2370 : Supplement SSN Processing updates
        List<BkgSSNExceptionNotificationDataContract> SendEmailWhenExceptionInSSNResult(Int32 BkgOrderID, String vendorBkgOrderLineItemDetailIDs);

        #region UAT-2842-- Admin Create Screening Order
        List<AdminCreateOrderContract> GetAdminCreateOrderSearchData(AdminOrderSearchContract searchContract, CustomPagingArgsContract gridCustomePaging);
        #endregion

        #region UAT-2842
        OrganizationUser GetOrganisationUserByUserID(Guid UserID);
        DataTable GetBkgPackageDetailsForAdminOrder(String DPMIds);
        List<Int32> GetBkgSvcGroupDetailsByBkgPkgId(Int32 bkgPackageId);
        Order SaveAdminOrderDetails(Order orderDetails);
        List<Int32> GetBackgroundServiceIdsBysvcGrpId(Int32 SvcGrpId, Int32 bkgPackageId);
        DataSet GetAdminOrderDetailsByOrderId(Int32 OrderId);
        Order GetAdminOrderDataByOrderId(Int32 OrderId);
        Boolean IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, Int32 ApplicantOrgUserID, Int32 ApplicantDocTypeId);
        Boolean IsDandAAlreadyUploaded(Int32 ApplicantOrgUserID, Int32 ApplicantDocTypeId);
        Boolean SaveApplicantDocumentDetails(List<ApplicantDocument> lstApplicantDocument);
        Boolean SaveUpdateGenericDocumentmapping(Int16 recordTypeId, List<Int32> lstApplicantDocIds, Int32 currentLoggedInUserId, Int32 organizationUserProfileId);
        List<ApplicantDocumentContract> GetApplicantDocuments(Int32 organizationUserId);
        Boolean DeleteApplicantDocuments(Int32 organizationUserID, Int32 currentLoggedInUserId);
        Boolean SaveCustomFormDetails(List<BackgroundOrderData> lstBkgOrderData, Int32 BkgOrderID, Int32 currentLoggedInUserId);
        Boolean DeleteAdminOrderDetails(Int32 currentLoggedInUserId, Int32 orderId);
        Boolean DeleteOldOrganizationUserProfileData(OrganizationUser OrgUserDetails);
        Boolean DeleteCustomFormData(Int32 BkgOrderID, Int32 currentLoggedInUserId, String packageIDs);
        #endregion

        #region UAT-2842:(Transmit Order Functionality)
        Boolean TransmmitAdminOrders(Int32 tenantId, Int32 currentLoggedInUserId, List<Int32> orderId);
        List<AdminOrderDetailReadyToTransmitContract> AdminOrderIsReadyToTransmit(Int32 tenantId, Int32 currentLoggedInUserId, String orderIds);
        Boolean IsAdminCreatedOrder(Int32 tenantId, Int32 orderId);
        DataTable CheckOrderAvailabilityForTrasmit(String OrderIds);
        #endregion

        //UAT-2154
        BkgOrder GetBkgOrderByBkgOrderId(Int32 BkgOrderID);

        //UAT-2587
        List<BackroundServicesContract> AcknowledgeMessagePopUpContent(String bkgPackageIds, Int32 selectedNodeId);

        #region UAT-3268:- Manage Additional Fee for Background package needed to Qualify for rotation.

        List<PkgAdditionalPaymentInfo> GetAdditionalPriceData(List<Int32> lstBkgHierarchyPkgId);
        List<OrderPaymentDetail> GetAdditionalPaymentModes(List<Int32> lstOpdIds);
        #endregion

        //UAT-3453
        Boolean IsBkgOrderFlagged(Int32 masterOrderId);

        //UAT 3521
        Dictionary<String, List<String>> GetDataForCascadingDropDown(String searchId, Int32 AtrributeGroupId, Int32 AttributeID);
        //UAT 3521
        List<String> GetDataForBindingCascadingDropDown(Int32 attributeGroupID, Int32 attributeId, String searchID);
        //UAT 3573 CBI
        String ValidatePageData(System.Text.StringBuilder xmlStringData, Boolean IsCustomFormScreen, string languageCode);
        //UAT-3669
        DataTable GetAlertMailDataForWebCCFError(Int32 CurrentLogedInUserId, String BlockedOrderReasonCode);

        List<CustomFormAutoFillDataContract> GetConditionsforAttributes(System.Text.StringBuilder xmlStringData, String languageCode);

        List<LookupContract> GetCustomAttributeOptionsData(String attributeName);

        DataTable GetDataForReceivedFromStudentServiceFormStatus(String TenantIDs, Int32 serviceFormStatusLimit); //UAT-3820

        List<Entity.ClientEntity.BkgPackageSvcGroup> GetBkgSvcGroupByBkgPkgId(Int32 bkgPackageId);

        List<SystemDocBkgSvcMapping> GetApplicantDocsMappedWithSvc(Int32 orderId); //UAT-3745
        List<VendorProfileSvcLineItemContract> GetLineItemsDataforOrderID(Int32 orderID);//UAT-4004
        Boolean SaveUpdateSvcLineItemMapping(Int32 currentLoggedInUserId, VendorProfileSvcLineItemContract vendorProfileSvcLineItemData); //UAT-4004
        List<VendorProfileSvcLineItemContract> GetSvcLineItemsCreated(Int32 orderID); //UAT-4004
        List<VendorProfileSvcLineItemContract> GetDSOrderToGetCSData(Int32 chunkSize, Int32 maxRetryCount, Int32 retryTimeLag);//UAT-4162
        Boolean UpdateRetryCountForDsOrders(Dictionary<Int32, Int32> dicbkgSvcLineItems, Int32 LoggedInUserId); //UAT-4162

        bool SaveCustomFormApplicantData(string xmlStringData, int applicantOrganisationId, int currentLoggedInUserId);
        DataTable GetBkgOrderServiceFormNotificationDataForAdminEntry(Int32 orderId, String serviceIds);

        DataTable GetDataForInProcessAgencyFromApplicantServiceFormStatus(Int32 serviceFormStatusLimit);
        bool CheckIfOrderIsAdminEntryOrder(Int32 bkgOrderId);
    }
}
