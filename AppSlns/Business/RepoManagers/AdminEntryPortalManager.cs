//using Amazon;
//using Amazon.S3;
//using Amazon.S3.IO;
//using Amazon.S3.Transfer;
using Business.ReportExecutionService;
using Entity.ClientEntity;
using INTSOF.UI.Contract.AdminEntryPortal;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using INTSOF.Utils.CommonPocoClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Business.RepoManagers
{
    public static class AdminEntryPortalManager
    {

        public static OrganizationUser GetOrganizationUser(Int32 tenantId, Int32 OrderId, Int32 organizationUserId)
        {
            try
            {
                return BALUtils.GetAdminEntryPortalRepoInstance(tenantId).GetOrganizationUser(OrderId, organizationUserId);
            }
            catch (INTSOF.Utils.SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new INTSOF.Utils.SysXException(ex.Message, ex));
            }
        }
        public static ApplicantOrderCart GetApplicantCartData(Int32 TenantId, Int32 OrderId)
        {
            try
            {
                return BALUtils.GetAdminEntryPortalRepoInstance(TenantId).GetApplicantCartData(OrderId);
            }
            catch (INTSOF.Utils.SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new INTSOF.Utils.SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// UAT 1438: Enhancement to allow students to select a User Group. 
        /// <param name="lstUserGroupIDs"></param>
        public static List<ApplicantUserGroupMapping> AddCustomAttributeValuesForUserGroup(List<Int32> lstUserGroupIDs, Int32 loggedInUserId, Int32 orgUserId)
        {
            try
            {
                List<ApplicantUserGroupMapping> lstUserGroupCustomAttributeMapping = new List<ApplicantUserGroupMapping>();
                if (lstUserGroupIDs.IsNotNull())
                {

                    foreach (Int32 userGroupID in lstUserGroupIDs)
                    {
                        ApplicantUserGroupMapping applicantUserGroupMapping = new ApplicantUserGroupMapping();
                        applicantUserGroupMapping.AUGM_UserGroupID = userGroupID;
                        applicantUserGroupMapping.AUGM_OrganizationUserID = loggedInUserId;
                        applicantUserGroupMapping.AUGM_IsDeleted = false;
                        applicantUserGroupMapping.AUGM_CreatedByID = orgUserId;
                        applicantUserGroupMapping.AUGM_CreatedOn = DateTime.Now;

                        lstUserGroupCustomAttributeMapping.Add(applicantUserGroupMapping);
                    }
                }
                return lstUserGroupCustomAttributeMapping;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Saves the Applicant Order 
        /// </summary>
        /// <param name="userOrder"></param>
        /// <param name="applicantOrderDataContract"></param>
        /// <param name="updateMainProfile"></param>
        /// <param name="lstPrevAddress"></param>
        /// <param name="lstPersonAliasContract"></param>
        /// <param name="paymentModeCode"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static Dictionary<String, String> SubmitApplicantOrder(Order userOrder, ApplicantOrderDataContract applicantOrderDataContract, Boolean updateMainProfile, List<PreviousAddressContract> lstPrevAddress,
            List<PersonAliasContract> lstPersonAliasContract, out String paymentModeCode, out Int32 orderId, Int32 orgUserID, List<OrderCartCompliancePackage> compliancePackages = null, List<ApplicantOrder> lstApplicantOrder = null,
            Boolean IsAdminEntryUser = false)
        {
            try
            {
                orderId = userOrder.OrderID;
                List<lkpEventHistory> _lstLkpEventHistoryBkgOrders = LookupManager.GetLookUpData<lkpEventHistory>(applicantOrderDataContract.TenantId).Where(eh => !eh.EH_IsDeleted).ToList();
                #region Account Settings dependent data add/update AND Statuses data Add/Update

                int dpm_ID = userOrder.SelectedNodeID.IsNullOrEmpty() ? AppConsts.NONE : userOrder.SelectedNodeID.Value;
                List<DeptProgramAdminEntryAcctSetting> lstAccountSetting = ComplianceSetupManager.GetDeptProgramAdminEntryAcctSettings(applicantOrderDataContract.TenantId, dpm_ID);
                String onHoldSettingIsOn = "0"; // UAT-4775
                if (!lstAccountSetting.IsNullOrEmpty() && lstAccountSetting.Count > AppConsts.NONE)
                {

                    //String BkgAdminEntryOrderDetailDraftStatus = String.Empty;
                    //String BkgAdminEntryOrderDetailStatus = String.Empty;

                    /*Getting Setting Value for applicant Invite submit status type setting, either it will be one(Draft) or two(Transmit). If there is no setting save for this setting type then 
                    treat it as draft submit status.(one) */
                    Int32 applicantInviteStatusSubmitId = !lstAccountSetting.Where(c => c.lkpAdminEntryAccountSetting.AEAS_Code == AdminEntryAccountSettingEnum.ApplicantInvite_SubmitStatus.GetStringValue()).FirstOrDefault().IsNullOrEmpty()
                                                            ? Convert.ToInt32(lstAccountSetting.Where(c => c.lkpAdminEntryAccountSetting.AEAS_Code == AdminEntryAccountSettingEnum.ApplicantInvite_SubmitStatus.GetStringValue())
                                                                    .Select(sel => sel.DPAEAS_SettingValue).FirstOrDefault()) : AppConsts.ONE;

                    applicantOrderDataContract.ApplicantInviteSubmitStatusTypeCode = LookupManager.GetLookUpData<lkpApplicantInviteSubmitStatusType>(applicantOrderDataContract.TenantId).Where(x => x.AISST_IsDeleted == false && x.AISST_ID == applicantInviteStatusSubmitId).Select(sel => sel.AISST_Code).FirstOrDefault();

                    // UAT-4775
                    onHoldSettingIsOn = !lstAccountSetting.Where(c => c.lkpAdminEntryAccountSetting.AEAS_Code == AdminEntryAccountSettingEnum.OnHoldStatus.GetStringValue()).FirstOrDefault().IsNullOrEmpty()
                                                ? lstAccountSetting.Where(c => c.lkpAdminEntryAccountSetting.AEAS_Code == AdminEntryAccountSettingEnum.OnHoldStatus.GetStringValue())
                                                                    .Select(sel => sel.DPAEAS_SettingValue).FirstOrDefault() : "0";

                }

                //this is independent of Account Setting
                applicantOrderDataContract.BkgAdminEntryOrderDetailDraftStatusId = LookupManager.GetLookUpData<lkpAdminEntryOrderDraftStatu>(applicantOrderDataContract.TenantId)
                                                                            .Where(con => !con.AEODS_IsDeleted && con.AEODS_Code == AdminEntryOrderDraftStatus.INVITATION_COMPLETED.GetStringValue()).Select(sel => sel.AEODS_ID).FirstOrDefault();
                String adminEntryOrderLineItemStatusCode = String.Empty;
                if (!String.IsNullOrEmpty(applicantOrderDataContract.ApplicantInviteSubmitStatusTypeCode)
                         && applicantOrderDataContract.ApplicantInviteSubmitStatusTypeCode == ApplicantInviteSubmitStatusType.TRANSMIT.GetStringValue())
                {
                    adminEntryOrderLineItemStatusCode = AdminEntryOrderStatus.INPROGRESS.GetStringValue();
                    applicantOrderDataContract.AdminEntryLineItemStatusId = LookupManager.GetLookUpData<lkpAdminEntryOrderLineItemStatu>(applicantOrderDataContract.TenantId)
                                                                              .Where(con => !con.AEOLIS_IsDeleted && con.AEOLIS_Code == AdminEntryOrderLineItemStatus.INPROGRESS.GetStringValue()).Select(sel => sel.AEOLIS_ID).FirstOrDefault();
                    applicantOrderDataContract.BkgAdminEntryOrderDetail_TransmittDate = DateTime.Now;

                    if (onHoldSettingIsOn == "1")
                    {
                        applicantOrderDataContract.BkgAdminEntryOrderDetailHoldStatusId = LookupManager.GetLookUpData<lkpAdminEntryOrderHoldStatu>(applicantOrderDataContract.TenantId)
                                                                              .Where(con => !con.AEOHS_IsDeleted && con.AEOHS_Code == "AAAA").Select(sel => sel.AEOHS_ID).FirstOrDefault();

                        String _onHoldEventCode = BkgOrderEvents.ADMIN_ENTRY_ORDER_ON_HOLD.GetStringValue();
                        applicantOrderDataContract.AdminOrderOnHoldEventID = _lstLkpEventHistoryBkgOrders.Where(eh => eh.EH_Code == _onHoldEventCode).Select(Sel => Sel.EH_ID).FirstOrDefault();

                    }

                    String _inprogressCode = BkgOrderEvents.ADMIN_ENTRY_ORDER_INPROGRESS.GetStringValue();
                    Int32 _inprogressId = _lstLkpEventHistoryBkgOrders.Where(eh => eh.EH_Code == _inprogressCode).Select(Sel => Sel.EH_ID).FirstOrDefault();
                    applicantOrderDataContract.AdminOrderInprogressEventID = _inprogressId;

                }
                else
                {
                    adminEntryOrderLineItemStatusCode = AdminEntryOrderStatus.DRAFT.GetStringValue();
                    applicantOrderDataContract.AdminEntryLineItemStatusId = LookupManager.GetLookUpData<lkpAdminEntryOrderLineItemStatu>(applicantOrderDataContract.TenantId)
                                                                              .Where(con => !con.AEOLIS_IsDeleted && con.AEOLIS_Code == AdminEntryOrderLineItemStatus.DRAFT.GetStringValue()).Select(sel => sel.AEOLIS_ID).FirstOrDefault();
                }
                applicantOrderDataContract.BkgAdminEntryOrderDetailStatusId = LookupManager.GetLookUpData<lkpAdminEntryOrderStatu>(applicantOrderDataContract.TenantId)
                                                                              .Where(con => !con.AEOS_IsDeleted && con.AEOS_Code == adminEntryOrderLineItemStatusCode)
                                                                              .Select(sel => sel.AEOS_ID).FirstOrDefault();

                #endregion

                Dictionary<String, Object> dicAddressData = new Dictionary<String, Object>();
                if (applicantOrderDataContract.OrganizationUserProfile.AddressHandle.IsNotNull())
                {
                    Address addressNew = applicantOrderDataContract.OrganizationUserProfile.AddressHandle.Addresses.Where(add => add.AddressHandleID == applicantOrderDataContract.OrganizationUserProfile.AddressHandleID).FirstOrDefault();
                    if (addressNew.IsNotNull())
                    {
                        dicAddressData.Add("address1", addressNew.Address1);
                        dicAddressData.Add("address2", addressNew.Address2);
                        dicAddressData.Add("zipcodeid", addressNew.ZipCodeID);
                    }
                }

                //Int32 _profileIdMaster;
                //Int32 _addressIdMaster;
                //Guid _addressHandleIdMaster;

                //List<Entity.PersonAliasProfile> lstPersonAliasProfile = new List<Entity.PersonAliasProfile>();
                //Entity.OrganizationUserProfile organizationUserProfileMain = SecurityManager.GetProfile


                if (updateMainProfile)
                {
                    //#region UPDATE ORIGINAL DATA IN CLIENT AND MASTER DB's
                    //List<Entity.ResidentialHistory> lstResendentialHistory = new List<Entity.ResidentialHistory>();
                    //List<Entity.PersonAlia> lstPersonAlias = new List<Entity.PersonAlia>();

                    //foreach (var perAlias in lstPersonAliasContract)
                    //{
                    //    Entity.PersonAlia personAlia = new Entity.PersonAlia();
                    //    personAlia.PA_ID = perAlias.ID;
                    //    personAlia.PA_FirstName = perAlias.FirstName;
                    //    personAlia.PA_MiddleName = perAlias.MiddleName;
                    //    personAlia.PA_LastName = perAlias.LastName;

                    //    lstPersonAlias.Add(personAlia);
                    //}
                    //#endregion
                }

                #region SAVE ORDER AND PROFILES IN THE CLIENT AND MASTER DATABASES


                String _invitationCompletedCode = BkgOrderEvents.APPLICANT_INVITATION_COMPLETED.GetStringValue();
                Int32 __invitationCompletedId = _lstLkpEventHistoryBkgOrders.Where(eh => eh.EH_Code == _invitationCompletedCode).FirstOrDefault().EH_ID;

                applicantOrderDataContract.OrderCreatedStatusId = __invitationCompletedId;


                if (!applicantOrderDataContract.lstBackgroundPackages.IsNullOrEmpty())
                {
                    List<lkpOrderStatusType> _lstOrderStatusTypes = LookupManager.GetLookUpData<lkpOrderStatusType>(applicantOrderDataContract.TenantId);
                    applicantOrderDataContract.BkgOrderStatusTypeId = _lstOrderStatusTypes.Where(ot => ot.Code == BackgroundOrderStatus.IN_PROGRESS.GetStringValue()).FirstOrDefault().OrderStatusTypeID;

                    List<lkpOrderLineItemStatu> _lstOrderLineItemStatus = LookupManager.GetLookUpData<lkpOrderLineItemStatu>(applicantOrderDataContract.TenantId);
                    String _orderLineItemStatusType = OrderLineItemStatusType.NEW.GetStringValue();
                    applicantOrderDataContract.OrderLineItemStatusId = _lstOrderLineItemStatus.Where(olists => olists.OLIS_Code == _orderLineItemStatusType && !olists.OLIS_IsDeleted).FirstOrDefault().OLIS_ID;

                    applicantOrderDataContract.lstSvcAttributeGrps = BackgroundSetupManager.GetServiceAttributeGroupsByTenant(applicantOrderDataContract.TenantId);
                }

                //Get SvcLineItem Status Id from lookup
                String dispatchedExternalVendorCode = SvcLineItemDispatchStatus.NOT_DISPATCHED.GetStringValue();
                Int16 PSLI_DispatchedExternalVendor = LookupManager.GetLookUpData<Entity.ClientEntity.lkpSvcLineItemDispatchStatu>(applicantOrderDataContract.TenantId).FirstOrDefault(cnd => cnd.SLIDS_Code == dispatchedExternalVendorCode).SLIDS_ID;

                #region Get Order RequestTypeID  [UAT-977: Additional work towards archive ability]
                String orderRequestNewOrderTypeCode = OrderRequestType.NewOrder.GetStringValue();
                Int32 orderRequestNewOrderTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderRequestType>(applicantOrderDataContract.TenantId).FirstOrDefault(cnd => cnd.ORT_Code == orderRequestNewOrderTypeCode && cnd.ORT_Active == true).ORT_ID;

                applicantOrderDataContract.NewSvcGrpReviewStatusTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpBkgSvcGrpReviewStatusType>(applicantOrderDataContract.TenantId).First(sgrs => sgrs.BSGRS_ReviewCode == BkgSvcGrpReviewStatusType.NEW.GetStringValue() && !sgrs.BSGRS_IsDeleted).BSGRS_ID;

                applicantOrderDataContract.NewSvcGrpStatusTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpBkgSvcGrpStatusType>(applicantOrderDataContract.TenantId).First(sgs => sgs.BSGS_StatusCode == BkgSvcGrpStatusType.NEW.GetStringValue() && !sgs.BSGS_IsDeleted).BSGS_ID;

                #endregion

                // Save Applicant Order
                var _dicInvoiceNumbers = BALUtils.GetAdminEntryPortalRepoInstance(applicantOrderDataContract.TenantId).SaveApplicantOrderProcessClient(userOrder, applicantOrderDataContract, PSLI_DispatchedExternalVendor, out paymentModeCode, out orderId, orgUserID, orderRequestNewOrderTypeId, compliancePackages);


                //Save Custom Attributes for UserGroup
                //UAT 1438: Enhancement to allow students to select a User Group.
                if (applicantOrderDataContract.IsUserGroupCustomAttributeExist)
                {
                    SaveUpdateApplicantUserGroupCustomAttribute(applicantOrderDataContract.TenantId, applicantOrderDataContract.lstAttributeValuesForUserGroup, applicantOrderDataContract.OrganizationUserProfile.OrganizationUserID, orgUserID);
                }
                try
                {
                    // Update External Service and Vendor for LineItems generated
                    if (!applicantOrderDataContract.lstBackgroundPackages.IsNullOrEmpty())
                        StoredProcedureManagers.UpdateExtServiceVendorforLineItems(userOrder.OrderID, applicantOrderDataContract.TenantId);
                }
                catch (Exception ex)
                {
                    BALUtils.LogError("Exception in calling stored procedure ams.usp_UpdateBkgOrderPackageSvcLineItem for Order number : " + orderId, ex);
                    BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                }

                //UAT 264
                String _prevStatus = ApplicantOrderStatus.Paid.GetStringValue();
                Int32 orderStatusId = ComplianceDataManager.GetOrderStatusList(applicantOrderDataContract.TenantId).Where(orderSts => orderSts.Code.ToLower() == _prevStatus.ToLower() && !orderSts.IsDeleted).FirstOrDefault().OrderStatusID;

                var _lstOPDs = userOrder.OrderPaymentDetails.Where(opd => opd.OPD_IsDeleted == false).ToList();

                if (userOrder.OrderGroupOrderNavProp.IsNotNull() && userOrder.OrderGroupOrderNavProp.Count > 0)
                {
                    foreach (Order o in userOrder.OrderGroupOrderNavProp)
                        _lstOPDs.AddRange(o.OrderPaymentDetails.Where(opd => opd.OPD_IsDeleted == false).ToList());
                }

                foreach (var opd in _lstOPDs)
                {
                    var _paymentTypeCode = opd.lkpPaymentOption.Code;
                    var _orderStatusId = opd.OPD_OrderStatusID;
                    var _orderPaymentDetailId = opd.OPD_ID;
                    //var _packageId = AppConsts.NONE;


                    // Case when Order gets Paid with Grand Total = 0 or it is case of Invoice to Institution
                    //if (_orderStatusId == orderStatusId || (_paymentTypeCode == PaymentOptions.Credit_Card.GetStringValue() && opd.OPD_Amount == 0) || (_paymentTypeCode == PaymentOptions.InvoiceWithOutApproval.GetStringValue() && !ifRenewalOrderApprovalRequired))
                    //{
                    //    DateTime expirydate = DateTime.Now;
                    //    if (opd.Order.SubscriptionYear.HasValue)
                    //    {
                    //        expirydate = expirydate.AddYears(opd.Order.SubscriptionYear.Value);
                    //    }
                    //    if (opd.Order.SubscriptionMonth.HasValue)
                    //    {
                    //        expirydate = expirydate.AddMonths(opd.Order.SubscriptionMonth.Value);
                    //    }

                    //    // If the Order contained any Compliance Package and it belongs to Current OPDs' OrderPkgPaymentDetails 
                    //    //if (!opd.Order.DeptProgramPackage.IsNullOrEmpty() && opd.OrderPkgPaymentDetails.Any(oppd => oppd.OPPD_BkgOrderPackageID.IsNull() && oppd.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()))
                    //    //    _packageId = opd.Order.DeptProgramPackage.DPP_CompliancePackageID;

                    //    //Changes done for UAT-357
                    //    String referenceNumber = String.Empty;
                    //    if (_orderStatusId == orderStatusId) // Change Subscription with 0 Payment 
                    //        referenceNumber = String.Format("Change Subscriptions: Previous Order Id: {0}", opd.Order.PreviousOrderID);
                    //    else
                    //        referenceNumber = "N/A";

                    //    UpdateOrderStatus(applicantOrderDataContract.TenantId, opd.Order.OrderID, ApplicantOrderStatus.Paid.GetStringValue(), _packageId,
                    //                       orgUserID, applicantOrderDataContract.OrganizationUserProfile.OrganizationUserID, referenceNumber, expirydate, _orderPaymentDetailId);


                    //}
                }

                return _dicInvoiceNumbers;

                #endregion

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// This method will save the UserGroup custom attribute in ApplicantUserGroupMapping Table.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="lstApplicantUserGroupMapping"></param>
        /// <param name="loggedInUserID"></param>
        /// <returns></returns>
        public static bool SaveUpdateApplicantUserGroupCustomAttribute(Int32 tenantID, List<ApplicantUserGroupMapping> lstApplicantUserGroupMapping, Int32 loggedInUserID, Int32 orgUsrID)
        {
            try
            {
                List<ApplicantUserGroupMapping> lstApplicantUserGroupMappingDistinct = lstApplicantUserGroupMapping.DistinctBy(x => x.AUGM_UserGroupID).ToList();
                List<ApplicantUserGroupMapping> lstPrevUserGroupMapping = BALUtils.GetComplianceDataRepoInstance(tenantID).GetApplicantUserGroupMappingForUser(loggedInUserID);

                List<Int32> lstCurrentUserGroupMappingIDs = lstApplicantUserGroupMappingDistinct.Select(x => x.AUGM_UserGroupID).ToList();
                List<Int32> lstPrevUserGroupMappingIDs = lstPrevUserGroupMapping.Select(x => x.AUGM_UserGroupID).ToList();

                List<Int32> usergroupIdsToAdd = lstCurrentUserGroupMappingIDs.Except(lstPrevUserGroupMappingIDs).ToList();
                List<Int32> usergroupIdsToRemove = lstPrevUserGroupMappingIDs.Except(lstCurrentUserGroupMappingIDs).ToList();

                List<ApplicantUserGroupMapping> lstApplicantUserGroupMappingToAdd = lstApplicantUserGroupMappingDistinct.Where(x => usergroupIdsToAdd.Contains(x.AUGM_UserGroupID)).ToList();
                List<ApplicantUserGroupMapping> lstApplicantUserGroupMappingToRemove = lstPrevUserGroupMapping.Where(x => usergroupIdsToRemove.Contains(x.AUGM_UserGroupID)).ToList();

                //Remove the deleted records
                if (!lstApplicantUserGroupMappingToRemove.IsNullOrEmpty())
                {
                    foreach (ApplicantUserGroupMapping applicantUserGroupMapping in lstApplicantUserGroupMappingToRemove)
                    {
                        applicantUserGroupMapping.AUGM_IsDeleted = true;
                        applicantUserGroupMapping.AUGM_ModifiedByID = orgUsrID;
                        applicantUserGroupMapping.AUGM_ModifiedOn = DateTime.Now;
                    }
                }

                if (lstApplicantUserGroupMappingToRemove.IsNullOrEmpty() && lstApplicantUserGroupMappingToAdd.IsNullOrEmpty())
                {
                    return true; //No need to call save.
                }
                else
                {
                    return BALUtils.GetAdminEntryPortalRepoInstance(tenantID).SaveUpdateApplicantUserGroupCustomAttribute(lstApplicantUserGroupMappingToAdd, loggedInUserID);
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean SaveApplicantEsignatureDocument(Int32 tenantId, Int32 applicantDisclaimerDocumentId, List<Int32?> applicantDisclosureDocumentIds, Int32 orderId, Int32 orgUserProfileId, Int32 currentLoggedInUserId, String orderNumber)
        {
            try
            {
                return BALUtils.GetAdminEntryPortalRepoInstance(tenantId).SaveApplicantEsignatureDocument(applicantDisclaimerDocumentId, applicantDisclosureDocumentIds, orderId, orgUserProfileId, currentLoggedInUserId, orderNumber);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<ApplicantDocument> UpdateApplicantAdditionalEsignatureDocument(Int32 tenantId, List<Int32?> applicantAdditionalDocumentId, Int32 orderId, Int32 orgUserProfileId, Int32 orgUserId, Boolean needToSaveMapping
                                                                                     , List<Int32?> additionalDocumentSendToStudent, List<SystemDocBkgSvcMapping> lstSystemDocBkgSvcMapping = null)
        {
            try
            {
                List<ApplicantDocument> lstAppDocCurrentOrd = new List<ApplicantDocument>();
                List<ApplicantDocument> lstAppDocPreviousOrd = new List<ApplicantDocument>();

                String recordTypeCodeOrder = RecordType.Order.GetStringValue();
                String dataEntryDocCompletedStatusCode = DataEntryDocumentStatus.COMPLETE.GetStringValue();
                List<lkpDataEntryDocumentStatu> dataEntryDocStatus = LookupManager.GetLookUpData<lkpDataEntryDocumentStatu>(tenantId);
                List<lkpRecordType> recordTypeList = LookupManager.GetLookUpData<lkpRecordType>(tenantId);
                Int16 dataEntryDocCompletedStatusId = dataEntryDocStatus.FirstOrDefault(cnd => cnd.LDEDS_Code == dataEntryDocCompletedStatusCode && !cnd.LDEDS_IsDeleted).LDEDS_ID;
                Int16 recordTypeIdOrder = recordTypeList.FirstOrDefault(cond => cond.Code == recordTypeCodeOrder && !cond.IsDeleted).RecordTypeID;

                lstAppDocCurrentOrd = BALUtils.GetAdminEntryPortalRepoInstance(tenantId).UpdateApplicantAdditionalEsignatureDocument(applicantAdditionalDocumentId, orderId, orgUserProfileId, orgUserId, needToSaveMapping, recordTypeIdOrder,
                                                                                                                      dataEntryDocCompletedStatusId, additionalDocumentSendToStudent, lstSystemDocBkgSvcMapping);
                if (!needToSaveMapping)
                {
                    lstAppDocPreviousOrd = UpdateAdditionalDocumentStatusForApproveOrder(tenantId, orderId, orgUserId, orgUserId);
                    lstAppDocCurrentOrd.AddRange(lstAppDocPreviousOrd);
                }
                return lstAppDocCurrentOrd;

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<ApplicantDocument> UpdateAdditionalDocumentStatusForApproveOrder(Int32 tenantId, Int32 orderId, Int32 currentloggedInUserId, Int32 orgUserId)
        {
            try
            {
                String recordTypeCodeOrder = RecordType.Order.GetStringValue();
                String dataEntryDocNewStatusCode = DataEntryDocumentStatus.NEW.GetStringValue();
                String additionalDocTypeCode = DocumentType.ADDITIONAL_DOCUMENTS.GetStringValue();
                List<lkpDataEntryDocumentStatu> dataEntryDocStatus = LookupManager.GetLookUpData<lkpDataEntryDocumentStatu>(tenantId);
                Int16 dataEntryDocNewStatusID = dataEntryDocStatus.FirstOrDefault(cnd => cnd.LDEDS_Code == dataEntryDocNewStatusCode && !cnd.LDEDS_IsDeleted).LDEDS_ID;

                return BALUtils.GetAdminEntryPortalRepoInstance(tenantId).UpdateAdditionalDocumentStatusForApproveOrder(orderId, currentloggedInUserId, additionalDocTypeCode, dataEntryDocNewStatusID, recordTypeCodeOrder, orgUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean AddUpdateAdminEntryUserData(OrganizationUserProfile UserProfile, Int32 currentLoggedInUserId, List<PersonAliasContract> lstPersonAlias, Int32 tenantId
                                                          , List<PreviousAddressContract> lstPrevAddress)
        {
            Entity.OrganizationUser organizationUser = SecurityManager.GetAdminEntryOrganizationUserDetailByOrgUserId(UserProfile.OrganizationUserID);
            organizationUser.FirstName = UserProfile.FirstName;
            organizationUser.LastName = UserProfile.LastName;
            organizationUser.MiddleName = UserProfile.MiddleName;
            organizationUser.IsApplicant = true;
            organizationUser.ModifiedByID = currentLoggedInUserId;
            organizationUser.ModifiedOn = DateTime.Now;
            organizationUser.DOB = UserProfile.DOB;
            organizationUser.Gender = UserProfile.Gender;
            organizationUser.PhoneNumber = UserProfile.PhoneNumber;
            organizationUser.SecondaryPhone = UserProfile.SecondaryPhone;
            organizationUser.PrimaryEmailAddress = UserProfile.PrimaryEmailAddress;
            organizationUser.SecondaryEmailAddress = UserProfile.SecondaryEmailAddress;
            organizationUser.SSN = UserProfile.SSN;
            organizationUser.SSNL4 = UserProfile.SSNL4;
            organizationUser.UserTypeID = UserProfile.UserTypeID;

            #region Add/Update Person Alias data here //For Security//

            //if (!organizationUser.IsNullOrEmpty() && !lstPersonAlias.IsNullOrEmpty())
            //{
            //    List<Entity.PersonAlia> lstAlias = organizationUser.PersonAlias.Where(con => !con.PA_IsDeleted).ToList();

            //    foreach (var personAlias in lstPersonAlias)
            //    {
            //        //if entry already exists//Update 
            //        if (personAlias.ID > AppConsts.NONE)
            //        {
            //            Entity.PersonAlia currentPersonAlias = lstAlias.FirstOrDefault(x => x.PA_ID == personAlias.ID);

            //            if (!currentPersonAlias.IsNullOrEmpty())
            //            {
            //                currentPersonAlias.PA_FirstName = personAlias.FirstName;
            //                currentPersonAlias.PA_LastName = personAlias.LastName;
            //                currentPersonAlias.PA_MiddleName = personAlias.MiddleName;
            //                currentPersonAlias.PA_ModifiedBy = currentLoggedInUserId;
            //                currentPersonAlias.PA_ModifiedOn = DateTime.Now;

            //                #region PersonAliasSuffixMappings update
            //                // personalias update functionality
            //                // Get PersonAliasSuffixMappings on the basis of PersonAliasID --
            //                //If It Exists // then Update
            //                //Else Add It
            //                if (!currentPersonAlias.PersonAliasSuffixMappings.Where(con => !con.PASM_IsDeleted).ToList().IsNullOrEmpty())
            //                {
            //                    Entity.PersonAliasSuffixMapping currentPersonAliasSuffix = currentPersonAlias.PersonAliasSuffixMappings.FirstOrDefault(x => !x.PASM_IsDeleted);

            //                    currentPersonAliasSuffix.PASM_SuffixId = Convert.ToInt32(personAlias.SuffixID);
            //                    currentPersonAliasSuffix.PASM_ModifiedBy = currentLoggedInUserId;
            //                    currentPersonAliasSuffix.PASM_ModifiedOn = DateTime.Now;
            //                }
            //                else
            //                {
            //                    Entity.PersonAliasSuffixMapping newPersonAliasSuffix = new Entity.PersonAliasSuffixMapping();
            //                    newPersonAliasSuffix.PASM_CreatedBy = currentLoggedInUserId;
            //                    newPersonAliasSuffix.PASM_CreatedOn = DateTime.Now;
            //                    newPersonAliasSuffix.PASM_SuffixId = Convert.ToInt32(personAlias.SuffixID);
            //                }
            //                #endregion
            //            }
            //        }
            //        else
            //        {
            //            //add entries into table if doesnt exist
            //            Entity.PersonAlia newPersonAlias = new Entity.PersonAlia();
            //            newPersonAlias.PA_FirstName = personAlias.FirstName;
            //            newPersonAlias.PA_MiddleName = personAlias.MiddleName;
            //            newPersonAlias.PA_LastName = personAlias.LastName;
            //            newPersonAlias.PA_CreatedBy = currentLoggedInUserId;
            //            newPersonAlias.PA_CreatedOn = DateTime.Now;

            //            #region // Add PersonAliasSuffixMappings Here//
            //            Entity.PersonAliasSuffixMapping newPersonAliasSuffix = new Entity.PersonAliasSuffixMapping();
            //            newPersonAliasSuffix.PASM_CreatedBy = currentLoggedInUserId;
            //            newPersonAliasSuffix.PASM_CreatedOn = DateTime.Now;
            //            newPersonAliasSuffix.PASM_SuffixId = Convert.ToInt32(personAlias.SuffixID);

            //            newPersonAlias.PersonAliasSuffixMappings.Add(newPersonAliasSuffix);
            //            organizationUser.PersonAlias.Add(newPersonAlias);
            //        }
            //    }
            //}
            ////Delete Existing aliases from table
            //else
            //{
            //    List<Entity.PersonAlia> lstExistingPersonAliases = organizationUser.PersonAlias.Where(con => !con.PA_IsDeleted).ToList();
            //    foreach (var personAlias in lstExistingPersonAliases)
            //    {
            //        personAlias.PA_IsDeleted = true;
            //        personAlias.PA_ModifiedBy = currentLoggedInUserId;
            //        personAlias.PA_ModifiedOn = DateTime.Now;
            //    }
            //}

            //#endregion

            #endregion

            #region New Implementation- Person Alias data
            if (!organizationUser.IsNullOrEmpty())
            {
                List<Entity.PersonAlia> lstExistingPersonAliases = organizationUser.PersonAlias.Where(con => !con.PA_IsDeleted).ToList();
                foreach (var personAlias in lstExistingPersonAliases)
                {
                    personAlias.PA_IsDeleted = true;
                    personAlias.PA_ModifiedBy = currentLoggedInUserId;
                    personAlias.PA_ModifiedOn = DateTime.Now;
                }

                if (!lstPersonAlias.IsNullOrEmpty())
                {
                    foreach (var personAlias in lstPersonAlias)
                    {
                        //add entries into table if doesnt exist
                        Entity.PersonAlia newPersonAlias = new Entity.PersonAlia();
                        //newPersonAlias.PA_ID = personAlias.ID;
                        newPersonAlias.PA_FirstName = personAlias.FirstName;
                        newPersonAlias.PA_MiddleName = personAlias.MiddleName;
                        newPersonAlias.PA_LastName = personAlias.LastName;
                        newPersonAlias.PA_CreatedBy = currentLoggedInUserId;
                        newPersonAlias.PA_CreatedOn = DateTime.Now;

                        #region // Add PersonAliasSuffixMappings Here//
                        if (!personAlias.SuffixID.IsNullOrEmpty() && personAlias.SuffixID > AppConsts.NONE)
                        {
                            Entity.PersonAliasSuffixMapping newPersonAliasSuffix = new Entity.PersonAliasSuffixMapping();
                            newPersonAliasSuffix.PASM_CreatedBy = currentLoggedInUserId;
                            newPersonAliasSuffix.PASM_CreatedOn = DateTime.Now;
                            newPersonAliasSuffix.PASM_SuffixId = Convert.ToInt32(personAlias.SuffixID);

                            newPersonAlias.PersonAliasSuffixMappings.Add(newPersonAliasSuffix);
                        }
                        organizationUser.PersonAlias.Add(newPersonAlias);
                        #endregion
                    }
                }
            }

            #endregion

            #region ADD/Update User Address Data

            //if (!UserProfile.AddressHandle.IsNullOrEmpty() && !UserProfile.AddressHandle.Addresses.IsNullOrEmpty())
            //{
            //    if (organizationUser.AddressHandle.IsNullOrEmpty())
            //    {
            //        if (UserProfile.AddressHandle.AddressHandleID == Guid.Empty)
            //        {
            //            UserProfile.AddressHandle.AddressHandleID = Guid.NewGuid();
            //        }
            //        BALUtils.GetAdminEntryPortalRepoInstance(tenantId).AddAddressHandle(UserProfile.AddressHandle.AddressHandleID);
            //    }
            //    organizationUser.AddressHandleID = organizationUser.AddressHandle.AddressHandleID = UserProfile.AddressHandle.AddressHandleID;
            //    foreach (var address in UserProfile.AddressHandle.Addresses)
            //    {
            //        if (address.AddressID > AppConsts.NONE)
            //        {
            //            Entity.Address currAddress = organizationUser.AddressHandle.Addresses.FirstOrDefault(x => x.AddressID == address.AddressID);
            //            if (!currAddress.IsNullOrEmpty())
            //            {
            //                currAddress.Address1 = address.Address1;
            //                currAddress.Address2 = address.Address2;
            //                currAddress.AddressHandleID = UserProfile.AddressHandle.AddressHandleID;
            //                currAddress.ModifiedByID = currentLoggedInUserId;
            //                currAddress.ModifiedOn = DateTime.Now;
            //                if (!address.AddressExts.IsNullOrEmpty() && address.AddressExts.Count > AppConsts.NONE)
            //                {
            //                    foreach (var addressExt in address.AddressExts)
            //                    {
            //                        if (addressExt.AE_ID > AppConsts.NONE)
            //                        {
            //                            Entity.AddressExt currentAddressExt = currAddress.AddressExts.FirstOrDefault(x => x.AE_ID == addressExt.AE_ID);
            //                            if (!currentAddressExt.IsNullOrEmpty())
            //                            {
            //                                currentAddressExt.AE_CityName = addressExt.AE_CityName;
            //                                currentAddressExt.AE_CountryID = addressExt.AE_CountryID > AppConsts.NONE ? addressExt.AE_CountryID : AppConsts.ONE;
            //                                currentAddressExt.AE_County = addressExt.AE_County;
            //                                currentAddressExt.AE_StateName = addressExt.AE_StateName;
            //                                currentAddressExt.AE_ZipCode = addressExt.AE_ZipCode;
            //                            }
            //                        }
            //                        else
            //                        {
            //                            Entity.AddressExt newAddressExt = new Entity.AddressExt();
            //                            newAddressExt.AE_CityName = addressExt.AE_CityName;
            //                            newAddressExt.AE_CountryID = addressExt.AE_CountryID > AppConsts.NONE ? addressExt.AE_CountryID : AppConsts.ONE;
            //                            newAddressExt.AE_County = addressExt.AE_County;
            //                            newAddressExt.AE_StateName = addressExt.AE_StateName;
            //                            newAddressExt.AE_ZipCode = addressExt.AE_ZipCode;
            //                            currAddress.AddressExts.Add(newAddressExt);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            Entity.Address newAddress = new Entity.Address();
            //            newAddress.Address1 = address.Address1;
            //            newAddress.Address2 = address.Address2;
            //            newAddress.AddressHandleID = UserProfile.AddressHandle.AddressHandleID;
            //            newAddress.CreatedByID = currentLoggedInUserId;
            //            newAddress.CreatedOn = DateTime.Now;
            //            if (!address.AddressExts.IsNullOrEmpty() && address.AddressExts.Count > AppConsts.NONE)
            //            {
            //                foreach (var addressExt in address.AddressExts)
            //                {
            //                    Entity.AddressExt newAddressExt = new Entity.AddressExt();
            //                    newAddressExt.AE_CityName = addressExt.AE_CityName;
            //                    newAddressExt.AE_CountryID = addressExt.AE_CountryID > AppConsts.NONE ? addressExt.AE_CountryID : AppConsts.ONE;
            //                    newAddressExt.AE_County = addressExt.AE_County;
            //                    newAddressExt.AE_StateName = addressExt.AE_StateName;
            //                    newAddressExt.AE_ZipCode = addressExt.AE_ZipCode;
            //                    newAddress.AddressExts.Add(newAddressExt);
            //                }
            //            }
            //            organizationUser.AddressHandle.Addresses.Add(newAddress);
            //        }
            //    }
            //}
            #endregion

            #region New Implementation- User Address Data

            if (!UserProfile.AddressHandle.IsNullOrEmpty() && !UserProfile.AddressHandle.Addresses.IsNullOrEmpty())
            {
                if (!organizationUser.IsNullOrEmpty() && !organizationUser.AddressHandle.IsNullOrEmpty() && !organizationUser.AddressHandle.Addresses.IsNullOrEmpty())
                {
                    List<Entity.Address> lstExistingAddresses = organizationUser.AddressHandle.Addresses.Where(con => con.IsActive).ToList();
                    foreach (var addresses in lstExistingAddresses)
                    {
                        addresses.IsActive = false;
                        addresses.ModifiedByID = currentLoggedInUserId;
                        addresses.ModifiedOn = DateTime.Now;
                    }
                }

                //if (organizationUser.AddressHandle.IsNullOrEmpty())
                //{
                //    if (UserProfile.AddressHandle.AddressHandleID == Guid.Empty)
                //    {
                //        UserProfile.AddressHandle.AddressHandleID = Guid.NewGuid();
                //    }
                //    BALUtils.GetAdminEntryPortalRepoInstance(tenantId).AddAddressHandle(UserProfile.AddressHandle.AddressHandleID);
                //}
                //organizationUser.AddressHandleID = organizationUser.AddressHandle.AddressHandleID = UserProfile.AddressHandle.AddressHandleID;
                //foreach (var address in UserProfile.AddressHandle.Addresses)
                //{
                //    Entity.Address newAddress = new Entity.Address();
                //    newAddress.AddressID = address.AddressID;
                //    newAddress.Address1 = address.Address1;
                //    newAddress.Address2 = address.Address2;
                //    newAddress.AddressHandleID = UserProfile.AddressHandle.AddressHandleID;
                //    newAddress.CreatedByID = currentLoggedInUserId;
                //    newAddress.CreatedOn = DateTime.Now;
                //    newAddress.IsActive = true;

                //    //AddressExt ???????

                //    if (!address.AddressExts.IsNullOrEmpty() && address.AddressExts.Count > AppConsts.NONE)
                //    {
                //        foreach (var addressExt in address.AddressExts)
                //        {
                //            Entity.AddressExt newAddressExt = new Entity.AddressExt();
                //            newAddressExt.AE_ID = addressExt.AE_ID;
                //            newAddressExt.AE_CityName = addressExt.AE_CityName;
                //            newAddressExt.AE_CountryID = addressExt.AE_CountryID > AppConsts.NONE ? addressExt.AE_CountryID : AppConsts.ONE;
                //            newAddressExt.AE_County = addressExt.AE_County;
                //            newAddressExt.AE_StateName = addressExt.AE_StateName;
                //            newAddressExt.AE_ZipCode = addressExt.AE_ZipCode;
                //            newAddress.AddressExts.Add(newAddressExt);
                //        }
                //    }
                //    organizationUser.AddressHandle.Addresses.Add(newAddress);
                //}
            }
            #endregion

            #region Add/Update User Profile

            if (UserProfile.OrganizationUserProfileID > AppConsts.NONE)
            {
                if (!organizationUser.OrganizationUserProfiles.IsNullOrEmpty() && organizationUser.OrganizationUserProfiles.Count > AppConsts.NONE)
                {
                    Entity.OrganizationUserProfile organizationUserProfile = organizationUser.OrganizationUserProfiles.FirstOrDefault(x => x.OrganizationUserProfileID == UserProfile.OrganizationUserProfileID);
                    if (!organizationUserProfile.IsNullOrEmpty())
                    {
                        organizationUserProfile.OrganizationUserID = UserProfile.OrganizationUserID;
                        organizationUserProfile.UserTypeID = UserProfile.UserTypeID;
                        organizationUserProfile.AddressHandleID = UserProfile.AddressHandleID;
                        organizationUserProfile.FirstName = UserProfile.FirstName;
                        organizationUserProfile.LastName = UserProfile.LastName;
                        organizationUserProfile.VerificationCode = UserProfile.VerificationCode;
                        organizationUserProfile.OfficeReturnDateTime = UserProfile.OfficeReturnDateTime;
                        organizationUserProfile.IsDeleted = UserProfile.IsDeleted;
                        organizationUserProfile.IsActive = UserProfile.IsActive;
                        organizationUserProfile.ExpireDate = UserProfile.ExpireDate;
                        organizationUserProfile.CreatedByID = UserProfile.CreatedByID;
                        organizationUserProfile.CreatedOn = UserProfile.CreatedOn;
                        organizationUserProfile.ModifiedByID = UserProfile.ModifiedByID;
                        organizationUserProfile.ModifiedOn = UserProfile.ModifiedOn;
                        organizationUserProfile.PhotoName = UserProfile.PhotoName;
                        organizationUserProfile.OriginalPhotoName = UserProfile.OriginalPhotoName;
                        organizationUserProfile.DOB = UserProfile.DOB;
                        organizationUserProfile.SSN = UserProfile.SSN;
                        organizationUserProfile.Gender = UserProfile.Gender;
                        organizationUserProfile.PhoneNumber = UserProfile.PhoneNumber;
                        organizationUserProfile.MiddleName = UserProfile.MiddleName;
                        organizationUserProfile.Alias1 = UserProfile.Alias1;
                        organizationUserProfile.Alias2 = UserProfile.Alias2;
                        organizationUserProfile.Alias3 = UserProfile.Alias3;
                        organizationUserProfile.PrimaryEmailAddress = UserProfile.PrimaryEmailAddress;
                        organizationUserProfile.SecondaryEmailAddress = UserProfile.SecondaryEmailAddress;
                        organizationUserProfile.SecondaryPhone = UserProfile.SecondaryPhone;
                        //UAT-2447
                        organizationUserProfile.IsInternationalPhoneNumber = UserProfile.IsInternationalPhoneNumber;
                        organizationUserProfile.IsInternationalSecondaryPhone = UserProfile.IsInternationalSecondaryPhone;
                        organizationUserProfile.ModifiedByID = currentLoggedInUserId;
                        organizationUserProfile.ModifiedOn = DateTime.Now;

                        #region New Implementation- Person Alias Profile/ Profile Suffix Mapping
                        if (!organizationUserProfile.IsNullOrEmpty())
                        {
                            List<Entity.PersonAliasProfile> lstExistingPersonAliasProfiles = organizationUserProfile.PersonAliasProfiles.Where(con => !con.PAP_IsDeleted).ToList();
                            foreach (var personAliasProfiles in lstExistingPersonAliasProfiles)
                            {
                                personAliasProfiles.PAP_IsDeleted = true;
                                personAliasProfiles.PAP_ModifiedBy = currentLoggedInUserId;
                                personAliasProfiles.PAP_ModifiedOn = DateTime.Now;
                            }

                            foreach (var personAliasProfile in lstPersonAlias)
                            {
                                Entity.PersonAliasProfile newPersonAliasProfile = new Entity.PersonAliasProfile();
                                newPersonAliasProfile.PAP_FirstName = personAliasProfile.FirstName;
                                newPersonAliasProfile.PAP_MiddleName = personAliasProfile.MiddleName;
                                newPersonAliasProfile.PAP_LastName = personAliasProfile.LastName;
                                newPersonAliasProfile.PAP_CreatedBy = currentLoggedInUserId;
                                newPersonAliasProfile.PAP_CreatedOn = DateTime.Now;
                                newPersonAliasProfile.PAP_SequenceId = personAliasProfile.AliasSequenceId;

                                if (!personAliasProfile.SuffixID.IsNullOrEmpty() && personAliasProfile.SuffixID > AppConsts.NONE)
                                {
                                    Entity.PersonAliasProfileSuffixMapping newPersonAliasProfileSuffix = new Entity.PersonAliasProfileSuffixMapping();
                                    newPersonAliasProfileSuffix.PAPSM_CreatedBy = currentLoggedInUserId;
                                    newPersonAliasProfileSuffix.PAPSM_CreatedOn = DateTime.Now;
                                    newPersonAliasProfileSuffix.PAPSM_SuffixId = Convert.ToInt32(personAliasProfile.SuffixID);

                                    newPersonAliasProfile.PersonAliasProfileSuffixMappings.Add(newPersonAliasProfileSuffix);
                                }
                                organizationUserProfile.PersonAliasProfiles.Add(newPersonAliasProfile);
                            }
                        }

                        #endregion

                        #region OUP Suffix Mapping Update
                        if (!organizationUserProfile.UserTypeID.IsNullOrEmpty() && organizationUserProfile.UserTypeID > AppConsts.NONE)
                        {
                            Entity.OrganizationUserProfileSuffixMapping oupSuffixMapping = new Entity.OrganizationUserProfileSuffixMapping();
                            oupSuffixMapping.OUPSM_OrganizationUserProfileId = organizationUserProfile.OrganizationUserProfileID;
                            //oupSuffixMapping.OUPSM_SuffixId = Convert.ToInt32(oupMapping.SuffixID);
                            oupSuffixMapping.OUPSM_SuffixId = Convert.ToInt32(organizationUserProfile.UserTypeID);
                            oupSuffixMapping.OUPSM_CreatedOn = DateTime.Now;
                            oupSuffixMapping.OUPSM_CreatedBy = organizationUser.OrganizationUserID;

                            organizationUserProfile.OrganizationUserProfileSuffixMappings.Add(oupSuffixMapping);
                        }
                        #endregion


                        #region New Implementation- User Profile Residential History
                        if (lstPrevAddress.IsNotNull())
                        {
                            List<Entity.ResidentialHistoryProfile> lstExistingResidentialHistoryProfiles = organizationUserProfile.ResidentialHistoryProfiles.Where(con => !con.RHIP_IsDeleted).ToList();
                            foreach (var residentHistoryProfile in lstExistingResidentialHistoryProfiles)
                            {
                                residentHistoryProfile.RHIP_IsDeleted = true;
                                residentHistoryProfile.RHIP_ModifiedBy = currentLoggedInUserId;
                                residentHistoryProfile.RHIP_ModifiedOn = DateTime.Now;
                            }

                            //List<Entity.ClientEntity.ResidentialHistoryProfile> lstResidentialHistoryProfiles = UserProfile.ResidentialHistoryProfiles.Where(con => !con.RHIP_IsDeleted).ToList();
                            //foreach (var resHistory in lstResidentialHistoryProfiles)
                            //{
                            //    //if (resHistory.RHIP_IsDeleted == false)
                            //    //{
                            //    Entity.ResidentialHistoryProfile residentialHistoryProfile = new Entity.ResidentialHistoryProfile();
                            //    residentialHistoryProfile.RHIP_AddressId = resHistory.RHIP_AddressId;
                            //    residentialHistoryProfile.RHIP_IsCurrentAddress = resHistory.RHIP_IsCurrentAddress;
                            //    residentialHistoryProfile.RHIP_IsPrimaryResidence = resHistory.RHIP_IsPrimaryResidence;
                            //    residentialHistoryProfile.RHIP_ResidenceStartDate = resHistory.RHIP_ResidenceStartDate;
                            //    residentialHistoryProfile.RHIP_ResidenceEndDate = resHistory.RHIP_ResidenceEndDate;
                            //    residentialHistoryProfile.RHIP_IsDeleted = resHistory.RHIP_IsDeleted;
                            //    residentialHistoryProfile.RHIP_CreatedBy = organizationUser.OrganizationUserID;
                            //    residentialHistoryProfile.RHIP_CreatedOn = resHistory.RHIP_CreatedOn;
                            //    residentialHistoryProfile.RHIP_SequenceOrder = resHistory.RHIP_SequenceOrder;
                            //    organizationUserProfile.ResidentialHistoryProfiles.Add(residentialHistoryProfile);
                            //    //}
                            //}
                            //if (lstPrevAddress.IsNotNull())
                            //{
                            //lstPrevAddress = lstPrevAddress.Where(x => x.isDeleted != true
                            //    && x.isCurrent == false) //Is current check added as the current address is NOT removed from the list now (Comment above)
                            //    .ToList();
                            //if (lstPrevAddress.Count > 0)
                            //{
                            foreach (var prevAddress in lstPrevAddress)
                            {

                                Guid addressHandleId = Guid.NewGuid();
                                //Entity.AddressHandle addressHandleNew = new Entity.AddressHandle();
                                //addressHandleNew.AddressHandleID = addressHandleId;

                                SecurityManager.AddAddressHandle(addressHandleId);

                                Entity.Address addressNew = new Entity.Address();
                                addressNew.Address1 = prevAddress.Address1;
                                addressNew.Address2 = prevAddress.Address2;
                                addressNew.IsActive = true;
                                addressNew.ZipCodeID = prevAddress.ZipCodeID;
                                if (addressNew.ZipCodeID == 0)
                                {
                                    Entity.AddressExt addressExtNew = new Entity.AddressExt();
                                    addressExtNew.AE_CountryID = prevAddress.CountryId;
                                    addressExtNew.AE_StateName = prevAddress.StateName;
                                    addressExtNew.AE_CityName = prevAddress.CityName;
                                    addressExtNew.AE_ZipCode = prevAddress.Zipcode;
                                    addressNew.AddressExts.Add(addressExtNew);
                                }
                                addressNew.AddressHandleID = addressHandleId;
                                organizationUserProfile.AddressHandleID = addressHandleId;
                                addressNew.CreatedOn = DateTime.Now;
                                //addressNew.CreatedByID = orgProfileMaster.OrganizationUserID;
                                addressNew.CreatedByID = currentLoggedInUserId;

                                Entity.ResidentialHistoryProfile objResHistoryProfile = new Entity.ResidentialHistoryProfile();
                                objResHistoryProfile.RHIP_IsCurrentAddress = prevAddress.isCurrent;
                                objResHistoryProfile.RHIP_IsPrimaryResidence = false;
                                objResHistoryProfile.RHIP_ResidenceStartDate = prevAddress.ResidenceStartDate;
                                objResHistoryProfile.RHIP_ResidenceEndDate = prevAddress.ResidenceEndDate;
                                objResHistoryProfile.RHIP_IsDeleted = false;
                                //objResHistoryProfile.RHIP_CreatedBy = orgProfileMaster.OrganizationUserID;
                                objResHistoryProfile.RHIP_CreatedBy = currentLoggedInUserId;
                                objResHistoryProfile.RHIP_CreatedOn = DateTime.Now;
                                objResHistoryProfile.RHIP_OrganizationUserProfileID = organizationUserProfile.OrganizationUserProfileID;
                                //objResHistoryProfile.RHIP_SequenceId = prevAddress.ResHistorySeqOrdID;
                                objResHistoryProfile.RHIP_SequenceOrder = prevAddress.ResHistorySeqOrdID;
                                objResHistoryProfile.RHIP_MotherMaidenName = prevAddress.MotherName;
                                objResHistoryProfile.RHIP_IdentificationNumber = prevAddress.IdentificationNumber;
                                objResHistoryProfile.RHIP_DriverLicenseNumber = prevAddress.LicenseNumber;
                                //objResHistoryProfile.Address = new Entity.Address();
                                objResHistoryProfile.Address = addressNew;
                                organizationUserProfile.ResidentialHistoryProfiles.Add(objResHistoryProfile);
                            }
                            //}
                            // }

                        }
                        #endregion

                    }
                }
            }

            #endregion

            #region Update Organization User

            organizationUser.UserID = UserProfile.OrganizationUser.UserID;
            organizationUser.OrganizationID = UserProfile.OrganizationUser.OrganizationID;
            organizationUser.BillingAddressID = UserProfile.OrganizationUser.BillingAddressID;
            organizationUser.ContactID = UserProfile.OrganizationUser.ContactID;
            organizationUser.UserTypeID = UserProfile.OrganizationUser.UserTypeID;
            organizationUser.DepartmentID = UserProfile.OrganizationUser.DepartmentID;
            organizationUser.SysXBlockID = UserProfile.OrganizationUser.SysXBlockID;
            organizationUser.AddressHandleID = UserProfile.OrganizationUser.AddressHandleID;
            organizationUser.FirstName = UserProfile.OrganizationUser.FirstName;
            organizationUser.LastName = UserProfile.OrganizationUser.LastName;
            organizationUser.IsMessagingUser = UserProfile.OrganizationUser.IsMessagingUser;
            organizationUser.ModifiedByID = currentLoggedInUserId;
            organizationUser.ModifiedOn = DateTime.Now;
            organizationUser.PhotoName = UserProfile.OrganizationUser.PhotoName;
            organizationUser.OriginalPhotoName = UserProfile.OrganizationUser.OriginalPhotoName;
            organizationUser.DOB = UserProfile.OrganizationUser.DOB;
            organizationUser.SSN = UserProfile.OrganizationUser.SSN; ///Issue here

            organizationUser.Gender = UserProfile.OrganizationUser.Gender;
            organizationUser.PhoneNumber = UserProfile.OrganizationUser.PhoneNumber;
            organizationUser.MiddleName = UserProfile.OrganizationUser.MiddleName;
            organizationUser.Alias1 = UserProfile.OrganizationUser.Alias1;
            organizationUser.Alias2 = UserProfile.OrganizationUser.Alias2;
            organizationUser.Alias3 = UserProfile.OrganizationUser.Alias3;
            organizationUser.PrimaryEmailAddress = UserProfile.OrganizationUser.PrimaryEmailAddress;
            organizationUser.SecondaryEmailAddress = UserProfile.OrganizationUser.SecondaryEmailAddress;
            organizationUser.SecondaryPhone = UserProfile.OrganizationUser.SecondaryPhone;
            organizationUser.IsInternationalPhoneNumber = UserProfile.OrganizationUser.IsInternationalPhoneNumber;
            organizationUser.IsInternationalSecondaryPhone = UserProfile.OrganizationUser.IsInternationalSecondaryPhone;
            //if (UserProfile.OrganizationUser.ResidentialHistories.IsNotNull())
            //{
            #region Old Implementation -- Residential Histories
            //foreach (var prevAddress in UserProfile.OrganizationUser.ResidentialHistories)
            //{
            //    Entity.ResidentialHistory newResHisObj = organizationUser.ResidentialHistories.FirstOrDefault(x => x.RHI_ID == prevAddress.RHI_ID);
            //    //newResHisObj delete existing
            //    if (newResHisObj.IsNotNull())
            //    {
            //        if (newResHisObj.RHI_IsCurrentAddress == true && prevAddress.RHI_IsCurrentAddress == true)
            //        {
            //            newResHisObj.RHI_AddressId = prevAddress.RHI_AddressId;
            //            newResHisObj.RHI_ResidenceStartDate = prevAddress.RHI_ResidenceStartDate;
            //            newResHisObj.RHI_ModifiedByID = prevAddress.RHI_ModifiedByID;
            //            newResHisObj.RHI_ModifiedOn = prevAddress.RHI_ModifiedOn;
            //            newResHisObj.RHI_SequenceOrder = prevAddress.RHI_SequenceOrder;
            //        }
            //        else
            //        {
            //            if (!(newResHisObj.RHI_IsDeleted == true && prevAddress.RHI_IsDeleted == true))
            //            {
            //                if (prevAddress.RHI_IsDeleted == true)
            //                {
            //                    newResHisObj.RHI_IsDeleted = true;
            //                }
            //                else
            //                {
            //                    newResHisObj.RHI_AddressId = prevAddress.RHI_AddressId;
            //                    newResHisObj.RHI_ResidenceStartDate = prevAddress.RHI_ResidenceStartDate;
            //                    newResHisObj.RHI_ResidenceEndDate = prevAddress.RHI_ResidenceEndDate;
            //                    newResHisObj.RHI_IsCurrentAddress = prevAddress.RHI_IsCurrentAddress;
            //                }
            //                newResHisObj.RHI_ModifiedByID = prevAddress.RHI_ModifiedByID;
            //                newResHisObj.RHI_ModifiedOn = prevAddress.RHI_ModifiedOn;
            //                newResHisObj.RHI_SequenceOrder = prevAddress.RHI_SequenceOrder;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (prevAddress.RHI_IsDeleted == false)
            //        {
            //            newResHisObj = new Entity.ResidentialHistory();
            //            newResHisObj.RHI_ID = prevAddress.RHI_ID;
            //            newResHisObj.RHI_ResidenceStartDate = prevAddress.RHI_ResidenceStartDate;
            //            newResHisObj.RHI_ResidenceEndDate = prevAddress.RHI_ResidenceEndDate;
            //            newResHisObj.RHI_OrganizationUserID = prevAddress.RHI_OrganizationUserID;
            //            newResHisObj.RHI_AddressId = prevAddress.RHI_AddressId;
            //            newResHisObj.RHI_IsCurrentAddress = prevAddress.RHI_IsCurrentAddress;
            //            newResHisObj.RHI_IsPrimaryResidence = prevAddress.RHI_IsPrimaryResidence;
            //            newResHisObj.RHI_IsDeleted = prevAddress.RHI_IsDeleted;
            //            newResHisObj.RHI_CreatedByID = prevAddress.RHI_CreatedByID;
            //            newResHisObj.RHI_CreatedOn = prevAddress.RHI_CreatedOn;
            //            newResHisObj.RHI_SequenceOrder = prevAddress.RHI_SequenceOrder;
            //            organizationUser.ResidentialHistories.Add(newResHisObj);
            //        }
            //    }
            //}
            #endregion

            #region New Implementation --Residential Histories

            if (lstPrevAddress.IsNotNull())
            {
                List<Entity.ResidentialHistory> oldResHisObj = organizationUser.ResidentialHistories.Where(con => !Convert.ToBoolean(con.RHI_IsDeleted)).ToList();
                foreach (var residentialHistory in oldResHisObj)
                {
                    residentialHistory.RHI_IsDeleted = true;
                    residentialHistory.RHI_ModifiedByID = currentLoggedInUserId;
                    residentialHistory.RHI_ModifiedOn = DateTime.Now;
                }

                PreviousAddressContract currentAddress = lstPrevAddress.FirstOrDefault(x => x.isCurrent == true);


                //Address userAddress = orgUserToUpdate.AddressHandle.Addresses.Where(add => add.AddressHandleID == orgUserToUpdate.AddressHandleID).FirstOrDefault();
                //if (userAddress.IsNotNull() && currentAddress.IsNotNull())
                //{
                //    if (CheckIfAddressUpdated(userAddress, currentAddress))
                //    {
                //        if (currentAddress.ZipCodeID == 0)
                //        {
                //            addressExtNew = new AddressExt();
                //            addressExtNew.AE_StateName = currentAddress.StateName;
                //            addressExtNew.AE_CityName = currentAddress.CityName;
                //            addressExtNew.AE_ZipCode = currentAddress.Zipcode;
                //        }
                //        Guid addressHandleId = Guid.NewGuid();
                //        AddAddressHandle(addressHandleId);
                //        AddAddress(dicAddressData, addressHandleId, Convert.ToInt32(orgUser.ModifiedByID), addressNew, addressExtNew);
                //        orgUserToUpdate.AddressHandleID = addressHandleId;
                //    }
                //}

                if (currentAddress.IsNotNull())
                {

                    Guid addressHandleId = Guid.NewGuid();
                    //Entity.AddressHandle addressHandleNew = new Entity.AddressHandle();
                    //addressHandleNew.AddressHandleID = addressHandleId;

                    SecurityManager.AddAddressHandle(addressHandleId);

                    Entity.Address addressNew = new Entity.Address();
                    addressNew.Address1 = currentAddress.Address1;
                    addressNew.Address2 = currentAddress.Address2;
                    addressNew.IsActive = true;
                    addressNew.ZipCodeID = currentAddress.ZipCodeID;
                    if (addressNew.ZipCodeID == 0)
                    {
                        Entity.AddressExt addressExtNew = new Entity.AddressExt();
                        addressExtNew.AE_CountryID = currentAddress.CountryId;
                        addressExtNew.AE_StateName = currentAddress.StateName;
                        addressExtNew.AE_CityName = currentAddress.CityName;
                        addressExtNew.AE_ZipCode = currentAddress.Zipcode;
                        addressNew.AddressExts.Add(addressExtNew);
                    }
                    addressNew.AddressHandleID = addressHandleId;
                    organizationUser.AddressHandleID = addressHandleId;
                    addressNew.CreatedOn = DateTime.Now;
                    //addressNew.CreatedByID = orgProfileMaster.OrganizationUserID;
                    addressNew.CreatedByID = currentLoggedInUserId;

                    //if (prevAddress.RHI_IsDeleted == false)
                    //{
                    Entity.ResidentialHistory newResidentialHistory = new Entity.ResidentialHistory();
                    //newResidentialHistory.RHI_ID = currentAddress.RHI_ID;
                    newResidentialHistory.RHI_ResidenceStartDate = currentAddress.ResidenceStartDate.HasValue ? currentAddress.ResidenceStartDate : null;
                    newResidentialHistory.RHI_ResidenceEndDate = currentAddress.ResidenceEndDate.HasValue ? currentAddress.ResidenceEndDate : null;
                    newResidentialHistory.RHI_OrganizationUserID = UserProfile.OrganizationUser.OrganizationID;
                    //newResidentialHistory.RHI_AddressId = currentAddress.RHI_AddressId;
                    newResidentialHistory.RHI_IsCurrentAddress = true;
                    //newResidentialHistory.RHI_DriverLicenseNumber = 
                    newResidentialHistory.RHI_IsPrimaryResidence = false;
                    newResidentialHistory.RHI_IsDeleted = false;
                    newResidentialHistory.RHI_CreatedByID = organizationUser.OrganizationUserID;
                    newResidentialHistory.RHI_CreatedOn = DateTime.Now;
                    newResidentialHistory.RHI_SequenceOrder = AppConsts.ONE;
                    //newResidentialHistory.RHI_IdentificationNumber = currentAddress.IdentificationNumber;
                    //newResidentialHistory.RHI_MotherMaidenName = currentAddress.MotherName;
                    //newResidentialHistory.RHI_DriverLicenseNumber = currentAddress.LicenseNumber;
                    //newResidentialHistory.Address = new Entity.Address();
                    newResidentialHistory.Address = addressNew;
                    organizationUser.ResidentialHistories.Add(newResidentialHistory);
                    // }
                }
                #endregion

            }

            #endregion

            SecurityManager.UpdateChanges();
            BALUtils.GetAdminEntryPortalRepoInstance(tenantId).AddUpdateAdminEntryUserData(organizationUser, UserProfile);
            return false;
        }


        public static Boolean UpdateApplicatInviteToken(Int32 tenantId, Int32 orderId)
        {
            try
            {
                return SecurityManager.UpdateApplicatInviteToken(tenantId, orderId);
            }
            catch (INTSOF.Utils.SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new INTSOF.Utils.SysXException(ex.Message, ex));
            }
        }

        public static List<SendDraftOrderNotificationtoAdminContract> GetDraftOrderForNotification(Int32 tenantID, Int32 subEventId, Int32 chunkSize, Int32 daysOld)
        {
            try
            {
                DataTable dt = BALUtils.GetAdminEntryPortalRepoInstance(tenantID).GetDraftOrders(chunkSize, daysOld, subEventId);

                return SetDraftOrderData(dt);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        private static List<SendDraftOrderNotificationtoAdminContract> SetDraftOrderData(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new SendDraftOrderNotificationtoAdminContract
                {
                    FirstName = x["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(x["FirstName"]),
                    LastName = x["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(x["LastName"]),
                    InstituteName = x["IN_Name"] == DBNull.Value ? String.Empty : Convert.ToString(x["IN_Name"]),
                    BkgOrderID = x["BOR_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["BOR_ID"]),
                    MasterOrderID = x["BOR_MasterOrderID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["BOR_MasterOrderID"]),
                    OrderNumber = x["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(x["OrderNumber"]),
                    CreatedOn = x["BAEOD_CreatedOn"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(x["BAEOD_CreatedOn"]),
                    SelectedNodeID = x["SelectedNodeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["SelectedNodeID"]),
                    OrgUserId = x["OrgUserId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["OrgUserId"])
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<SendInvitationPendingOrderNotificationtoApplicantContract> GetInvitationPendingStatusOrderForApplicant(Int32 tenantID, Int32 subEventId, Int32 chunkSize, Int32 daysOld)
        {
            try
            {
                DataTable dt = BALUtils.GetAdminEntryPortalRepoInstance(tenantID).GetInvitationPendingStatusOrderForApplicant(chunkSize, daysOld, subEventId);

                return SetInvitationPendingOrders(dt);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        private static List<SendInvitationPendingOrderNotificationtoApplicantContract> SetInvitationPendingOrders(DataTable table)
        {


            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new SendInvitationPendingOrderNotificationtoApplicantContract
                {
                    FirstName = x["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(x["FirstName"]),
                    LastName = x["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(x["LastName"]),
                    hierarchyNodeName = x["IN_Name"] == DBNull.Value ? String.Empty : Convert.ToString(x["IN_Name"]),
                    BkgOrderID = x["BOR_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["BOR_ID"]),
                    SelectedNodeID = x["SelectedNodeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["SelectedNodeID"]),
                    OrgUserId = x["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["OrganizationUserID"]),
                    Email = x["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(x["PrimaryEmailAddress"]),
                    CreatedDate = x["CreatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(x["CreatedDate"]),
                    AdminEntryInvitationToken = x["InvitationToken"] == DBNull.Value ? String.Empty : Convert.ToString(x["InvitationToken"])
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static bool DeleteDraftOrder(Int32 daysOld, Int32 tenantID, Int32 backgroundProcessUserId)
        {
            try
            {

                return BALUtils.GetAdminEntryPortalRepoInstance(tenantID).DeleteDraftOrders(daysOld, backgroundProcessUserId);


            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static bool ChangeBkgOrdersStatusCompletedToArchived(Int32 BkgAdminEntryOrderId, Int32 DaysSettingValue, Int32 tenantID, Int32 backgroundProcessUserId)
        {
            try
            {

                List<lkpAdminEntryOrderStatu> lstlkpAdminEntryOrderStatus = LookupManager.GetLookUpData<lkpAdminEntryOrderStatu>(tenantID);
                List<lkpEventHistory> lstlkpEventHistory = LookupManager.GetLookUpData<lkpEventHistory>(tenantID);

                return BALUtils.GetAdminEntryPortalRepoInstance(tenantID).ChangeBkgOrdersStatusCompletedToArchived(DaysSettingValue, BkgAdminEntryOrderId, backgroundProcessUserId
                                                                              , lstlkpAdminEntryOrderStatus, lstlkpEventHistory);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static List<AutoArchivedTimeLineDays> GetAutoArchiveTimeLineDays(Int32 chunkSize, Int32 tenantID)
        {
            var dt = BALUtils.GetAdminEntryPortalRepoInstance(tenantID).GetAutoArchiveTimeLineDays(chunkSize);
            return GetSettingValueFromDT(dt);
        }

        public static List<AutoArchivedTimeLineDays> GetSettingValueFromDT(DataTable dt)
        {
            IEnumerable<DataRow> rows = dt.AsEnumerable();
            try
            {
                return rows.Select(x => new AutoArchivedTimeLineDays
                {
                    BkgAdminEntryOrderId = x["BAEOD_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["BAEOD_ID"]),
                    OrgUserId = x["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["OrganizationUserID"]),
                    OrderStatusId = x["BAEOD_OrderDraftStatusID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["BAEOD_OrderDraftStatusID"]),
                    FirstName = x["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(x["FirstName"]),
                    LastName = x["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(x["LastName"]),
                    Days = x["DPAEAS_SettingValue"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["DPAEAS_SettingValue"]),
                    BkgOrderId = x["BOR_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["BOR_ID"]),
                    OrderCompletedDate = x["OrderCompleteDate"] == DBNull.Value ? String.Empty : Convert.ToString(x["OrderCompleteDate"]),
                    InstitutionHierarchyName = x["IN_Name"] == DBNull.Value ? String.Empty : Convert.ToString(x["IN_Name"]),
                    SelectedNodeID = x["SelectedNodeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["SelectedNodeID"]),
                    OrderId = x["OrderId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["OrderId"]),
                    OrderNumber = x["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(x["OrderNumber"]),
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static List<CompleteStatusOrders> GetRecentCompletedOrders(Int32 chunkSize, string EntityName, Int32 tenantID, Int32 subEventId)
        {

            var dt = BALUtils.GetAdminEntryPortalRepoInstance(tenantID).GetRecentCompletedOrders(chunkSize, EntityName, subEventId);
            return GetCompletedOrdersFromDT(dt);

        }
        public static List<CompleteStatusOrders> GetCompletedOrdersFromDT(DataTable dt)
        {
            IEnumerable<DataRow> rows = dt.AsEnumerable();
            try
            {
                return rows.Select(x => new CompleteStatusOrders
                {

                    OrderID = x["OrderID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["OrderID"]),
                    BkgOrderID = x["BOR_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["BOR_ID"]),
                    //BkgOrderPackageID = x["BOP_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["BOP_ID"]),
                    BkgServGrpStatusType = x["BSGS_StatusType"] == DBNull.Value ? String.Empty : Convert.ToString(x["BSGS_StatusType"]),
                    BkgServGrpStatusCode = x["BSGS_StatusCode"] == DBNull.Value ? String.Empty : Convert.ToString(x["BSGS_StatusCode"]),
                    OrganizationUserID = x["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["OrganizationUserID"]),
                    FirstName = x["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(x["FirstName"]),
                    LastName = x["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(x["LastName"]),
                    InstitutionHierarchyName = x["IN_Name"] == DBNull.Value ? String.Empty : Convert.ToString(x["IN_Name"]),
                    SelectedNodeId = x["SelectedNodeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["SelectedNodeID"]),
                    OrderNumber = x["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(x["OrderNumber"]),
                    IsAdminOrder = x["IsAdminOrder"] == DBNull.Value ? false : Convert.ToBoolean(x["IsAdminOrder"]),
                    HierarchyNodeId = x["HierarchyNodeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["HierarchyNodeID"]),
                    // BkgOrderPkgServGrpID = x["OPSG_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["OPSG_ID"])


                    //Convert.ToInt32(x["OrderID"]),
                    //BkgOrderID = Convert.ToInt32(x["BOR_ID"]),
                    //BkgOrderPackageID = Convert.ToInt32(x["BOP_ID"]),
                    ////BkgOrderPkgServGrpID = Convert.ToInt32(x["OPSG_ID"]),
                    //BkgServGrpStatusType= x["BSGS_StatusType"].ToString(),
                    //BkgServGrpStatusCode= x["BSGS_StatusCode"].ToString(),
                    //OrganizationUserID =Convert.ToInt32(x["OrganizationUserID"]),
                    //FirstName = x["FirstName"].ToString(),
                    //LastName = x["LastName"].ToString(),
                    //InstituteName = x["IN_Name"].ToString(),
                    //SelectedNodeId = Convert.ToInt32(x["SelectedNodeID"]),
                    //OrderNumber = x["OrderNumber"].ToString(),
                    //IsAdminOrder= Convert.ToBoolean(x["IsAdminOrder"]),
                    //HierarchyNodeId= Convert.ToInt32(x["HierarchyNodeID"])
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static String GetApplicantInviteContent(Int32 selectedTenantId, Int32 orderId)
        {
            try
            {
                return BALUtils.GetAdminEntryPortalRepoInstance(selectedTenantId).GetApplicantInviteContent(orderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

    }
}