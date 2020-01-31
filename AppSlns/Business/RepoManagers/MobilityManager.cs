using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.Mobility;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using INTSOF.UI.Contract.ComplianceOperation;

namespace Business.RepoManagers
{
    public static class MobilityManager
    {
        #region Variables

        #region public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static MobilityManager()
        {
            BALUtils.ClassModule = "MobilityManager";
        }

        #endregion

        #region Properties

        #region public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public static IQueryable<Entity.PkgMappingMaster> GetPkgMappingMasterData(Int32 pkgMappingMasterId)
        {
            try
            {
                return BALUtils.GetMobilityRepoInstance().GetPkgMappingMasterData(pkgMappingMasterId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static ObjectResult<GetRuleSetTree> GetRuleSetTreeForPackage(Int32 tenantId, String packageId)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantId).GetRuleSetTreeForPackage(packageId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {

                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int16? GetMobilityStatusIDByCode(Int32 tenantId, String statusCode)
        {
            try
            {
                var lkpSubscriptionMobilityStatus = LookupManager.GetLookUpData<Entity.ClientEntity.lkpSubscriptionMobilityStatu>(tenantId).FirstOrDefault(obj => obj.Code.Equals(statusCode) && obj.IsDeleted == false);

                if (lkpSubscriptionMobilityStatus.IsNotNull())
                {
                    return lkpSubscriptionMobilityStatus.SubscriptionMobilityStatusID;
                }
                return null;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static ObjectResult<Entity.GetComplianceItemMappingDetails_Result> GetMappedItemList(String complianceItemMappingXML)
        {
            try
            {
                return BALUtils.GetMobilityRepoInstance().GetMappedItemList(complianceItemMappingXML);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static ObjectResult<Entity.GetComplianceItemMappingDetails_Result> GetSavedMappedItemDetails(Int32 packageMappingMasterID)
        {
            try
            {
                return BALUtils.GetMobilityRepoInstance().GetSavedMappedItemDetails(packageMappingMasterID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static Boolean SaveComplianceItmMapping(List<Entity.ComplianceItmMappingDetail> cmplnceItemMappingDetailContractList)
        {
            try
            {
                return BALUtils.GetMobilityRepoInstance().SaveComplianceItmMapping(cmplnceItemMappingDetailContractList);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdateOrderSkippedMapping(Entity.PkgMappingMaster pkgMappingMaster, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantID).UpdateOrderSkippedMapping(pkgMappingMaster);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int16? GetPkgMappingTypeStatusIDByCode(String pkgMappingTypeCode)
        {
            try
            {
                var lkpPkgMappingTypes = LookupManager.GetLookUpData<Entity.lkpPkgMappingType>().FirstOrDefault(x => x.PMT_Code == pkgMappingTypeCode && x.PMT_IsDeleted == false);
                if (lkpPkgMappingTypes.IsNotNull())
                {
                    return lkpPkgMappingTypes.PMT_ID;
                }
                return null;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String GetNodesDetails(Int32 nodeID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantID).GetNodesDetails(nodeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static Entity.ComplianceItmMappingDetail GetComplianceItemMappingDetails(Int32 tenantID, Int32 pkgMappingMasterID, Int32 itemID, Int32 attributeID)
        {
            try
            {
                return BALUtils.GetMobilityRepoInstance().GetComplianceItemMappingDetails(tenantID, pkgMappingMasterID, itemID, attributeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //public static ObjectResult<PackageSubscriptionList> CopyPackageData(Int32 tenantID, List<SourceTargetSubscriptionID> subscriptionID)
        public static List<PackageSubscriptionList> CopyPackageData(Int32 tenantID, List<SourceTargetSubscriptionList> subscriptionID, Int32 currentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantID).CopyPackageData(subscriptionID, currentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //public static ObjectResult<PackageSubscriptionList> CopyPackageData(Int32 tenantID, List<SourceTargetSubscriptionID> subscriptionID)
        public static Int32 RollbackSubscriptions(Int32 tenantID, Int32 loginUserID, List<Int32> subscriptionID)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantID).RollbackSubscriptions(loginUserID, subscriptionID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #region "Get Lust of Active Subscriptions"
        public static List<AdminChangeSubscription> GetAdminChangeSubscriptionList(int ClientId, SearchItemDataContract searchDataContract, CustomPagingArgsContract gridCustomPaging)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(ClientId).GetAdminChangeSubscriptionList(searchDataContract, gridCustomPaging);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        public static List<ActiveSubscriptionsForRollback> GetActiveSubscriptionsForRollback(Int32 ClientId, string _applicantFirstName, string _applicantLastName, Int32? _userGroupID, Int32? _sourceNodeID, Int32? _targetNodeID, DateTime? _fromDate, DateTime? _toDate, CustomPagingArgsContract gridCustomPaging)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(ClientId).GetActiveSubscriptionsForRollback(_applicantFirstName, _applicantLastName, _userGroupID, _sourceNodeID, _targetNodeID, _fromDate, _toDate, gridCustomPaging);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region Institution Change Request Queue

        /// <summary>
        /// Get the Request Status ID as per LkpInstChangeRequestStatus Code from lkpInstChangeRequestStatu table.
        /// </summary>
        /// <param name="requestStatusCode">lkpInstChangeRequestStatu.Code</param>
        /// <returns>InstChangeRequestStatusID</returns>
        public static ObjectResult<GetPreviousValuesForSubscription> GetPreviousDataForSubscription(Int32 subscriptionnId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantId).GetPreviousDataForSubscription(subscriptionnId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Saves the Institute Change Request for an applicant.
        /// </summary>
        /// <param name="institutionChangeRequest">Object of InstitutionChangeRequest including Organization User ID, Tenant ID, Request Note, Source Subscription ID, Source Node ID,Request Status ID, Is Deleted, Created On and Created By ID </param>
        /// <returns>True if Institute Change Request is saved</returns>
        public static void SaveInstituteChangeRequest(Entity.InstitutionChangeRequest institutionChangeRequest)
        {
            try
            {
                BALUtils.GetMobilityRepoInstance().SaveInstituteChangeRequest(institutionChangeRequest);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get List of Instituion Change request Status
        /// </summary>
        public static List<Entity.lkpInstChangeRequestStatu> GetInstChangeRequestStatusList()
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpInstChangeRequestStatu>().Where(cond => !cond.IsDeleted).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Entity.GetInstitutionChangeRequests> GetInstitutionChangeRequestData(MobilitySearchDataContract mobilitySerachItemDataContract, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                string orderBy = QueueConstants.APPLICANT_SEARCH_DEFAULT_SORTING_FIELDS;
                string ordDirection = null;
                orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
                ordDirection = customPagingArgsContract.SortDirectionDescending == false ? null : "desc";

                return BALUtils.GetMobilityRepoInstance().GetInstitutionChangeRequestData(mobilitySerachItemDataContract, customPagingArgsContract, orderBy, ordDirection);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        //public static ObjectResult<GetTargetMobilityNodes> GetTargetMobilityNodes(Int32 subscriptionnId, Int32 tenantId)
        //{
        //    try
        //    {
        //        return BALUtils.GetClientMobilityRepoInstance(tenantId).GetTargetMobilityNodes(subscriptionnId);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        public static ObjectResult<GetSourceNodeDeatils> GetSourceNodeDeatils(Int32 tenantId, Int32 sourceNodeId, Int32 sourcePackageId)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantId).GetSourceNodeDeatils(sourceNodeId, sourcePackageId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static String GetTargetNodeHierarchyLabel(Int32 tenantId, Int32 departmentProgramMappingId)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantId).GetTargetNodeHierarchyLabel(departmentProgramMappingId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<PackageSubscription> GetSourceSubscriptionDetails(Int32 tenantId, List<Int32> sourceSubscriptionList)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantId).GetSourceSubscriptionDetails(sourceSubscriptionList);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        //public static List<usp_SubscriptionChange_Result> CreateNewSubscriptionForMobilityNode(Int32 tenantId, String xml)
        //{
        //    try
        //    {
        //        return BALUtils.GetClientMobilityRepoInstance(tenantId).CreateNewSubscriptionForMobilityNode(xml);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        public static void sendMailForChangeSubscription(Int32 tenantId, usp_SubscriptionChange_Result mobilitySubscriptionChangedData, String applicationUrl, String tenantName, String changedHierarchyNode, Int32 hierarchyNodeID)
        {
            try
            {
                //Create Dictionary
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(mobilitySubscriptionChangedData.FirstName, " ", mobilitySubscriptionChangedData.LastName));
                dictMailData.Add(EmailFieldConstants.CHANGED_HIERARCHY_NODE, changedHierarchyNode);
                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);

                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                mockData.UserName = string.Concat(mobilitySubscriptionChangedData.FirstName, " ", mobilitySubscriptionChangedData.LastName);
                mockData.EmailID = mobilitySubscriptionChangedData.PrimaryEmailAddress;
                mockData.ReceiverOrganizationUserID = Convert.ToInt32(mobilitySubscriptionChangedData.OranizationUserId);
                //UAT-2324 Dont send payment due email if the payment method is invoice
                //If the payment method associated with package or node is only invoice with approval or invoice without approval, no mail of balance payment will be sent,only notification for program change will be sent.
                int orderId = mobilitySubscriptionChangedData.NewOrderId ?? 0;
                Order objOrder = ComplianceDataManager.GetOrderById(tenantId, orderId);
                int deptProgramPackageId = 0;
                if (objOrder != null)
                {
                    deptProgramPackageId = objOrder.DeptProgramPackageID ?? 0;
                }
                List<PkgList> lstPkg = StoredProcedureManagers.GetPaymentOptions(deptProgramPackageId, "", mobilitySubscriptionChangedData.SelectedNodeId, tenantId);
                //Added condition of IfInvoiceIsOnlyPaymentOptions for UAT-2324
                if (mobilitySubscriptionChangedData.DuePayment == null || mobilitySubscriptionChangedData.DuePayment == AppConsts.NONE || IfInvoiceIsOnlyPaymentOptions(lstPkg))
                {
                    //Send mail
                    Int32? systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_PROGRAMME_CHANGE, dictMailData, mockData, tenantId, hierarchyNodeID);
                    #region UAT-3389
                    Dictionary<String, object> dicMessageParam = new Dictionary<String, object>();
                    //if (opd.Order.lkpOrderPackageType.OPT_Code.Equals(OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()) && systemCommunicationID.HasValue && systemCommunicationID > AppConsts.NONE)
                    if (
                        !objOrder.IsNullOrEmpty() &&
                          (
                          objOrder.lkpOrderPackageType.OPT_Code.Equals(OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue())
                          ||
                          objOrder.lkpOrderPackageType.OPT_Code.Equals(OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue())
                          )
                          &&
                          systemCommunicationID.HasValue && systemCommunicationID > AppConsts.NONE
                         )
                    {
                        var res = ComplianceDataManager.AttachOrderApprovalDocuments(tenantId, objOrder.OrderID, objOrder.OrganizationUserProfile.OrganizationUserID, systemCommunicationID.Value);
                        if (res.Item1)
                        {
                            dicMessageParam = res.Item2;
                        }
                    }

                    #endregion
                    //Send Message
                    CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_PROGRAMME_CHANGE, dictMailData, Convert.ToInt32(mobilitySubscriptionChangedData.OranizationUserId), tenantId, dicMessageParam);
                }
                else
                {
                    dictMailData.Add(EmailFieldConstants.AMOUNT_DUE, String.Format("{0}", Convert.ToDecimal(mobilitySubscriptionChangedData.DuePayment).ToString("0.00")));
                    //Send mail
                    CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_PROGRAMME_CHANGE_AND_BALANCE_PAYMENT, dictMailData, mockData, tenantId, hierarchyNodeID);
                    //Send Message
                    CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_PROGRAMME_CHANGE_AND_BALANCE_PAYMENT, dictMailData, Convert.ToInt32(mobilitySubscriptionChangedData.OranizationUserId), tenantId);
                }
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }
        #endregion


        #region Institute Change Request Detail

        public static Entity.InstitutionChangeRequest GetInstitutionChangeRequestById(Int32 institutionChangeRequestId)
        {
            try
            {
                return BALUtils.GetMobilityRepoInstance().GetInstitutionChangeRequestById(institutionChangeRequestId);
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

        public static PackageSubscription GetPackageSubscriptionById(Int32 packageSubscriptionId, Int32 tenantId, Boolean checkDeletedSubscriptions = true)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantId).GetPackageSubscriptionById(packageSubscriptionId, checkDeletedSubscriptions);
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

        public static DeptProgramMapping GetDeptProgramMappingById(Int32 deptProgramMappingId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantId).GetDeptProgramMappingById(deptProgramMappingId);
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

        public static Boolean SetInstnChangeReqStatus(Int32 institutionChangeRequestId, String statusCode, Int32 currentLoggedInUserId, Int32? newTenantId = null, String rejectnReason = null)
        {
            try
            {
                return BALUtils.GetMobilityRepoInstance().SetInstnChangeReqStatus(institutionChangeRequestId, statusCode, currentLoggedInUserId, newTenantId, rejectnReason);
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

        #endregion

        #region Institute Hierarchy Mobility

        public static Boolean CreateMobilityInstance(Int32 tenantID, Int32 backgroundProcessUserId, Int32 chunkSize)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantID).CreateMobilityInstance(backgroundProcessUserId, chunkSize);
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

        public static Boolean InsertNodeTranistionQueue(Int32 tenantID, Int32 backgroundProcessUserId, Int32 daysDueBeforeTransition)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantID).InsertNodeTranistionQueue(backgroundProcessUserId, daysDueBeforeTransition);
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
        /// To get InstHierarchyMobility data
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static InstHierarchyMobility GetInstHierarchyMobility(Int32 deptProgramMappingID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantID).GetInstHierarchyMobility(deptProgramMappingID);
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
        /// To save mobility data
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="firstStartDate"></param>
        /// <param name="durationTypeID"></param>
        /// <param name="duration"></param>
        /// <param name="instanceInterval"></param>
        /// <param name="successorNodeID"></param>
        /// <param name="listMobilityPackageRelation"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static Boolean SaveMobilityData(Int32 deptProgramMappingID, DateTime firstStartDate, Int16 durationTypeID, Int32 duration, Int32? instanceInterval, Int32? successorNodeID, List<MobilityPackageRelation> listMobilityPackageRelation, Int32 currentUserId, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantID).SaveMobilityData(deptProgramMappingID, firstStartDate, durationTypeID, duration, instanceInterval, successorNodeID, listMobilityPackageRelation, currentUserId);
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
        /// To delete Mobility Data
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static Boolean DeleteMobilityData(Int32 deptProgramMappingID, Int32 currentUserId, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantID).DeleteMobilityData(deptProgramMappingID, currentUserId);
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


        #endregion

        #region Mobility Mapping Queue

        public static List<Entity.ApplicantTransitionMappingList> GetApplicantMappingList(Int32 tenantID, CustomPagingArgsContract gridCustomPaging, String searchParameter)
        {
            try
            {
                return BALUtils.GetMobilityRepoInstance().GetApplicantTransitionMappingList(gridCustomPaging, searchParameter);
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



        public static Boolean CheckIfMappingAlreadyExist(Int32 tenantID, Int32 SelectedSourcePackageId, Int32 SelectedTargetPackageId, Int32 SourceNodeId, Int32 TargetNodeId)
        {
            try
            {
                return BALUtils.GetMobilityRepoInstance().CheckIfMappingAlreadyExist(tenantID, SelectedSourcePackageId, SelectedTargetPackageId, SourceNodeId, TargetNodeId);
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

        public static Boolean SaveMapping(Int32 tenantID, Entity.PkgMappingMaster pkgMappingMaster)
        {
            try
            {
                return BALUtils.GetMobilityRepoInstance().SaveMapping(pkgMappingMaster);
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

        public static Boolean HasNoReviewedInstance(Int32 pkgMappingMasterId)
        {
            try
            {
                return BALUtils.GetMobilityRepoInstance().HasNoReviewedInstance(pkgMappingMasterId);
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

        public static Boolean IsSubscriptionExist(Int32 tenantId, Int32 pkgMappingMasterId)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantId).IsSubscriptionExist(pkgMappingMasterId);
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

        public static Entity.PkgMappingMaster GetPkgMappingDtail(Int32 pkgMappingMasterId)
        {
            try
            {
                return BALUtils.GetMobilityRepoInstance().GetPkgMappingDtail(pkgMappingMasterId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdateChanges(Int32 pkgMappingMasterId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetMobilityRepoInstance().UpdateChanges(pkgMappingMasterId, currentLoggedInUserId);
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

        #endregion

        #region PkgMappingDependency

        public static List<Entity.ClientEntity.CompliancePkgMappingDependency> GetPkgMappingDependencyList(Int32 tenantID, CustomPagingArgsContract gridCustomPaging, Int32 packageMappingMasterId)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantID).GetPkgMappingDependencyList(gridCustomPaging, packageMappingMasterId);
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
        #endregion

        #region Applicant Node Transition Status
        /// <summary>
        /// Gets the most recent PackageSubscription Details for a Applicant.
        /// </summary>
        /// <returns>PackageSubscription</returns>
        public static List<ApplicantTransitionStatus> GetApplicantNodeTransitionStatus(Int32 tenantId, CustomPagingArgsContract gridCustomPaging, MobilitySearchDataContract mobilitySearchData)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantId).GetApplicantNodeTransitionStatus(gridCustomPaging, mobilitySearchData);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Gets the most recent PackageSubscription Details for a Applicant.
        /// </summary>
        /// <returns>PackageSubscription</returns>
        public static Boolean UpdateNodeTransitionStatus(Int32 tenantId, List<Int32> mobilityNodeTransitionIds, Int32 currentLoggedInUserId, Int16 approvalStatusID)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantId).UpdateNodeTransitionStatus(mobilityNodeTransitionIds, currentLoggedInUserId, approvalStatusID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }




        #endregion



        #region Common Methods

        #region Add Record In Mapping Queue

        public static List<Entity.MappingRequest> AddInMappingQueue(List<Entity.MappingRequest> mappingDetail, Int32 currentLoggedInUserId)
        {
            try
            {
                String mappingRequest = GetMappingRequestXMLString(mappingDetail, currentLoggedInUserId);
                return BALUtils.GetMobilityRepoInstance().AddInMappingQueue(mappingRequest);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get XML String For Status List
        /// </summary>
        /// <param name="listOfIds">mappingDetailContract</param>
        /// <param name="currentLoggedInUserId">currentLoggedInUserId</param>
        /// <returns>String</returns>
        public static String GetMappingRequestXMLString(List<Entity.MappingRequest> mappingDetail, Int32 currentLoggedInUserId)
        {
            if (mappingDetail.Count > 0 && mappingDetail.IsNotNull())
            {
                StringBuilder mappingData = new StringBuilder();
                mappingData.Append("<MappingRequest>");
                foreach (Entity.MappingRequest mappingItem in mappingDetail)
                {
                    #region Generate XML
                    String mappingName = mappingItem.MappingName.IsNullOrEmpty() ? mappingItem.FromPackageName + " > " + mappingItem.ToPackageName : mappingItem.MappingName;
                    mappingData.Append("<MappingRequest>");
                    mappingData.Append("<FromTenantID>" + mappingItem.FromTenantID.ToString() + "</FromTenantID>");
                    mappingData.Append("<ToTenantID>" + mappingItem.ToTenantID.ToString() + "</ToTenantID>");
                    mappingData.Append("<FromPackageID>" + mappingItem.FromPackageID.ToString() + "</FromPackageID>");
                    mappingData.Append("<ToPackageID>" + mappingItem.ToPackageID.ToString() + "</ToPackageID>");
                    mappingData.Append("<FromPackageName>" + mappingItem.FromPackageName.ToString() + "</FromPackageName>");
                    mappingData.Append("<ToPackageName>" + mappingItem.ToPackageName.ToString() + "</ToPackageName>");
                    mappingData.Append("<MappingName>" + mappingName + "</MappingName>");
                    mappingData.Append("<CreatedBy>" + currentLoggedInUserId.ToString() + "</CreatedBy>");
                    mappingData.Append("</MappingRequest>");
                    #endregion
                }
                mappingData.Append("</MappingRequest>");
                return mappingData.ToString();
            }
            return null;
        }

        public static List<Entity.MappingRequestData> AddRecordsInMappingQueue(List<Entity.MappingRequestData> mappingDetail, Int32 currentLoggedInUserId)
        {
            try
            {
                String mappingRequest = GetMappingRequestDataXMLString(mappingDetail, currentLoggedInUserId);
                return BALUtils.GetMobilityRepoInstance().AddRecordsInMappingQueue(mappingRequest);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get XML String For Status List
        /// </summary>
        /// <param name="listOfIds">mappingDetailContract</param>
        /// <param name="currentLoggedInUserId">currentLoggedInUserId</param>
        /// <returns>String</returns>
        public static String GetMappingRequestDataXMLString(List<Entity.MappingRequestData> mappingDetail, Int32 currentLoggedInUserId)
        {
            if (mappingDetail.Count > 0 && mappingDetail.IsNotNull())
            {
                StringBuilder mappingData = new StringBuilder();
                mappingData.Append("<MappingRequest>");
                foreach (Entity.MappingRequestData mappingItem in mappingDetail)
                {
                    #region Generate XML
                    String mappingName = mappingItem.MappingName.IsNullOrEmpty() ? mappingItem.FromPackageName + " > " + mappingItem.ToPackageName : mappingItem.MappingName;
                    mappingData.Append("<MappingRequest>");
                    mappingData.Append("<FromTenantID>" + mappingItem.FromTenantID.ToString() + "</FromTenantID>");
                    mappingData.Append("<ToTenantID>" + mappingItem.ToTenantID.ToString() + "</ToTenantID>");
                    mappingData.Append("<FromPackageID>" + mappingItem.FromPackageID.ToString() + "</FromPackageID>");
                    mappingData.Append("<ToPackageID>" + mappingItem.ToPackageID.ToString() + "</ToPackageID>");
                    mappingData.Append("<FromPackageName>" + mappingItem.FromPackageName.ToString() + "</FromPackageName>");
                    mappingData.Append("<ToPackageName>" + mappingItem.ToPackageName.ToString() + "</ToPackageName>");
                    mappingData.Append("<MappingName>" + mappingName + "</MappingName>");
                    mappingData.Append("<CreatedBy>" + currentLoggedInUserId.ToString() + "</CreatedBy>");
                    mappingData.Append("<IsMappingSkipped>" + mappingItem.IsMappingSkipped.ToString() + "</IsMappingSkipped>");
                    mappingData.Append("<FromNodeId>" + mappingItem.FromNodeId.ToString() + "</FromNodeId>");
                    mappingData.Append("<ToNodeId>" + mappingItem.ToNodeId.ToString() + "</ToNodeId>");
                    mappingData.Append("</MappingRequest>");
                    #endregion
                }
                mappingData.Append("</MappingRequest>");
                return mappingData.ToString();
            }
            return null;
        }
        #region Generate mapping instance and update in package subscription

        public static void generateMappingReviewInstanceAndUpdatePendingSubscription(Int32 mappingMasterId, Int32 tenantId, Int32 currentLoggedInUserId)
        {
            try
            {
                Int32? mobilityInstanceId = BALUtils.GetMobilityRepoInstance().generateMappingInstance(mappingMasterId, currentLoggedInUserId);
                if (mobilityInstanceId != null)
                    BALUtils.GetClientMobilityRepoInstance(tenantId).UpdateMappingInstanceforPendingSubscription(mappingMasterId, mobilityInstanceId.Value, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void UpdateMappingInstanceforPackageSubscription(Int32 mappingMasterId, Int32 mappingInstanceId, Int32 currentLoggedInUserId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetClientMobilityRepoInstance(tenantId).UpdateMappingInstanceforPendingSubscription(mappingMasterId, mappingInstanceId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int32 GetMappingInstanceId(Int32 mappingMasterId, Int32 currentLoggedInUserId)
        {
            try
            {
                Int32? _mappingChildId = BALUtils.GetMobilityRepoInstance().GetPossibleInstanceIdByCurrentInstanceId(mappingMasterId);

                if (_mappingChildId == AppConsts.NONE || _mappingChildId.IsNull())
                    _mappingChildId = BALUtils.GetMobilityRepoInstance().generateMappingInstance(mappingMasterId, currentLoggedInUserId);

                return Convert.ToInt32(_mappingChildId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion
        #endregion
        #region Applicant Balance Payment

        /// <summary>
        /// Get the Order for an Applicant whose Balance is due.
        /// </summary>
        /// <param name="tenantID">Tenant ID in which applicant is present.</param>
        /// <param name="applicantID">ID of applicant.</param>
        /// <returns>Order</returns>
        public static Order GetApplicantBalanceDueOrder(Int32 tenantID, Int32 applicantID)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantID).GetApplicantBalanceDueOrder(applicantID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Order GetApplicantBalanceDuePreviousOrder(Int32 tenantID, Int32 applicantID, Int32? orderID)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantID).GetApplicantBalanceDuePreviousOrder(applicantID, orderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsOrderPaymtDueAndChangeByAdmin(Int32 tenantID, Int32 orderID)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantID).IsOrderPaymtDueAndChangeByAdmin(orderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Get Current Mobility Instance for a Node.
        #endregion

        public static MobilityInstance GetNodeMobilityInstance(Int32 tenantID, Int32 dpmID)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantID).GetNodeMobilityInstance(dpmID);
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
        /// Check if the mapping exists for the source and target packages, between the two tenants
        /// </summary>
        /// <param name="sourcePackageId"></param>
        /// <param name="targetPackageId"></param>
        /// <param name="sourceTenantId"></param>
        /// <param name="targetTenantId"></param>
        /// <returns></returns>
        public static PackageMappingStatus IfMappingExists(Int32 sourcePackageId, Int32 targetPackageId, Int32 sourceTenantId, Int32 targetTenantId, Int32 sourceNodeId, Int32? targetNodeId, out Boolean IsReviewRequiredEveryTransaction, out Boolean IfReviewMappingCanBeSkipped)
        {
            try
            {
                IsReviewRequiredEveryTransaction = ComplianceDataManager.GetReviewPackageMappingEveryTransitionValue(sourceTenantId, Setting.REVIEW_PACKAGE_MAPPING_EVERY_TRANSITION.GetStringValue());
                //return BALUtils.GetMobilityRepoInstance().IfMappingExists(sourcePackageId, targetPackageId, sourceTenantId, targetTenantId);
                Entity.PkgMappingMaster pkgMappingMaster = BALUtils.GetMobilityRepoInstance().getMappingData(sourcePackageId, targetPackageId, sourceTenantId, targetTenantId, sourceNodeId, targetNodeId);
                if (pkgMappingMaster.IsNullOrEmpty())
                    IfReviewMappingCanBeSkipped = false;
                else
                    IfReviewMappingCanBeSkipped = pkgMappingMaster.PMM_IsMappingSkipped.Value;

                if (pkgMappingMaster.IsNullOrEmpty())
                    return PackageMappingStatus.MappingNotDefined;
                else if (pkgMappingMaster.lkpPkgMappingStatu.PMS_Code == PackageMappingStatus.Reviewed.GetStringValue())
                    return PackageMappingStatus.Reviewed;
                else if (pkgMappingMaster.lkpPkgMappingStatu.PMS_Code == PackageMappingStatus.Pending_Review.GetStringValue())
                    return PackageMappingStatus.Pending_Review;
                else if (pkgMappingMaster.lkpPkgMappingStatu.PMS_Code == PackageMappingStatus.Reviewed_Instance.GetStringValue())
                    return PackageMappingStatus.Reviewed_Instance;

                return PackageMappingStatus.MappingNotDefined;

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        public static Boolean CopyPackageDataExecuteBusinessRule(Int32 tenantID, Int32 chunkSize, Int32 backgroundProcessUserId)
        {
            try
            {
                List<SourceTargetSubscriptionList> lstSourceTargetList = BALUtils.GetClientMobilityRepoInstance(tenantID).GetSourceTargetSubscriptionList(chunkSize);
                if (lstSourceTargetList.IsNotNull() && lstSourceTargetList.Count > 0)
                {

                    List<Int32> lstSubLstForWhichDataIsCopied = new List<Int32>();
                    //Added foereach Loop for TimeOut Issue UAT-2276.
                    //CopyPackageData and rule exceution for each subscription one by one.
                    foreach (SourceTargetSubscriptionList SourceTargetSubscription in lstSourceTargetList)
                    {
                        List<PackageSubscriptionList> targetSubscriptionIds = new List<PackageSubscriptionList>();
                        List<SourceTargetSubscriptionList> lstSourceTargetData = new List<SourceTargetSubscriptionList>();
                        lstSourceTargetData.Add(new SourceTargetSubscriptionList
                        {
                            SourceSubscriptionID = SourceTargetSubscription.SourceSubscriptionID,
                            TargetSubscriptionID = SourceTargetSubscription.TargetSubscriptionID
                        });

                        //UAT-3805
                        List<Int32> approvedCategoryIDs = new List<Int32>();
                        approvedCategoryIDs = ProfileSharingManager.GetApprovedCategorIDs(tenantID, SourceTargetSubscription.TargetSubscriptionID
                                                                                              , new List<Int32>(), lkpUseTypeEnum.COMPLIANCE.GetStringValue());


                        targetSubscriptionIds.AddRange(MobilityManager.CopyPackageData(tenantID, lstSourceTargetData, backgroundProcessUserId));
                        String code = LkpSubscriptionMobilityStatus.DataMovementNotRequired;
                        Int16? subscriptionMobilityStatusID = MobilityManager.GetMobilityStatusIDByCode(tenantID, code);
                        Int16? DataMovedStatusId = MobilityManager.GetMobilityStatusIDByCode(tenantID, LkpSubscriptionMobilityStatus.DataMovementComplete);
                        List<Int32> targetIds = targetSubscriptionIds.Where(item => item.SubscriptionMobilityStatusID == subscriptionMobilityStatusID || item.SubscriptionMobilityStatusID == DataMovedStatusId).Select(item => item.TargetSubscriptionID).ToList();

                        if (targetIds.IsNotNull() && targetIds.Count > 0)
                        {
                            lstSubLstForWhichDataIsCopied.AddRange(targetIds);
                            /*UAT-1395:Change Subscription/Data sync bugs found by QA
                             ->Insert copied subscription record in data sync history
                             -> Background data will not sync for those subscriptions which are requested for change subscription and those package data were copied */

                            List<Int32> targetPkgSubIdsForChangeSubRequest = new List<Int32>();
                            targetPkgSubIdsForChangeSubRequest = MobilityManager.GetPackageSubscriptionIDForChangeSub(tenantID, targetIds);
                            if (!targetPkgSubIdsForChangeSubRequest.IsNullOrEmpty() && targetPkgSubIdsForChangeSubRequest.Count > AppConsts.NONE)
                            {
                                String subscriptionListXML = GetSubscriptsXMlForDatSyncHistory(targetPkgSubIdsForChangeSubRequest);
                                SaveDataSyncHistory(tenantID, subscriptionListXML, backgroundProcessUserId);

                                foreach (Int32 PkgSubscriptionId in targetIds)
                                {
                                    ComplianceDataManager.CopyData(PkgSubscriptionId, tenantID, backgroundProcessUserId, null);
                                }
                            }

                            //Check - Remove susbcriptions from targetIdsForDataMoved which are archived.
                            Int32 ArchivedID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpArchiveState>(tenantID).FirstOrDefault(cnd => cnd.AS_Code == ArchiveState.Archived.GetStringValue() && cnd.AS_IsDeleted == false).AS_ID;
                            List<Int32> targetIdsForDataMoved = targetSubscriptionIds.Where(item => item.SubscriptionMobilityStatusID != subscriptionMobilityStatusID && item.ArchiveStateID != ArchivedID).Select(item => item.TargetSubscriptionID).ToList();
                            RuleManager.ExecuteBusinessRules(targetIdsForDataMoved, tenantID, backgroundProcessUserId);

                            //UAT-2618
                            if (targetSubscriptionIds.IsNotNull() && targetSubscriptionIds.Count > 0)
                            {
                                foreach (PackageSubscriptionList packageSubscription in targetSubscriptionIds)
                                {
                                    ComplianceDataManager.UpdateIsDocAssociated(tenantID, packageSubscription.TargetSubscriptionID, true, backgroundProcessUserId);

                                    //UAT-3805
                                    PackageSubscription pkgSubscription = ComplianceDataManager.GetPackageSubscriptionByID(tenantID, packageSubscription.TargetSubscriptionID);

                                    if (!pkgSubscription.IsNullOrEmpty())
                                    {
                                        List<Int32> catIDs = pkgSubscription.ApplicantComplianceCategoryDatas.Where(cnd => !cnd.IsDeleted)
                                                                                                             .Select(slct => slct.ComplianceCategoryID).ToList();
                                        if (!catIDs.IsNullOrEmpty())
                                        {
                                            String categoryIds = String.Join(",", catIDs);
                                            String approvedCategoryIds = approvedCategoryIDs.IsNullOrEmpty() ? String.Empty : String.Join(",", approvedCategoryIDs);
                                            ProfileSharingManager.SendItemDocNotificationOnCategoryApproval(tenantID, categoryIds, pkgSubscription.OrganizationUserID.Value
                                                                                                            , approvedCategoryIds, lkpUseTypeEnum.COMPLIANCE.GetStringValue()
                                                                                                            , pkgSubscription.PackageSubscriptionID, null, backgroundProcessUserId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (lstSubLstForWhichDataIsCopied.Count > AppConsts.NONE)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean checkIfMappingIsDefinedForAttribute(Int32 attributeId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetMobilityRepoInstance().checkIfMappingIsDefinedForAttribute(attributeId, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<ApplicantsNodeTransitions> GetApplicantsNodeTransitionsDue(Int32 tenantID, Int32 chunkSize)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantID).GetApplicantsNodeTransitionsDue(chunkSize);
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

        public static List<AutomaticChangedSubscriptions> AutomaticChangeSubscription(Int32 tenantID, String sourceXML)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantID).AutomaticChangeSubscription(sourceXML);
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
        public static Int32 GetMappingData(Int32 sourcePackageId, Int32 targetPackageId, Int32 sourceTenantId, Int32 targetTenantId, Int32 sourceNodeId, Int32? targetNodeId)
        {
            try
            {
                Entity.PkgMappingMaster pkgMappingMaster = BALUtils.GetMobilityRepoInstance().getMappingData(sourcePackageId, targetPackageId, sourceTenantId, targetTenantId, sourceNodeId, targetNodeId);
                if (pkgMappingMaster.IsNotNull() && pkgMappingMaster.lkpPkgMappingStatu.PMS_Code == PackageMappingStatus.Reviewed.GetStringValue())
                    return pkgMappingMaster.PMM_ID;
                else
                    return AppConsts.NONE;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #region Private Methods
        #region UAT-2324 Dont send payment due email if the payment method is invoice
        private static Boolean IfInvoiceIsOnlyPaymentOptions(List<PkgList> lstPkg)
        {

            if (lstPkg != null && lstPkg.Count > 0)
            {
                List<PkgPaymentOptions> paymentOptions = lstPkg[0].lstPaymentOptions;
                if (paymentOptions.Count == 1)
                {
                    return paymentOptions.Any(t => t.PaymentOptionCode == PaymentOptions.InvoiceWithApproval.GetStringValue() || t.PaymentOptionCode == PaymentOptions.InvoiceWithOutApproval.GetStringValue());
                }
                else if (paymentOptions.Count == 2)
                {
                    return (paymentOptions.Any(t => t.PaymentOptionCode == PaymentOptions.InvoiceWithApproval.GetStringValue()) && paymentOptions.Any(t => t.PaymentOptionCode == PaymentOptions.InvoiceWithOutApproval.GetStringValue()));
                }
            }
            return false;
        }
        #endregion

        #region UAT-1395:Change Subscription/Data sync bugs found by QA
        private static String GetSubscriptsXMlForDatSyncHistory(List<Int32> subscriptionIDList)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Subscriptions>");
            foreach (Int32 subscriptionID in subscriptionIDList)
            {
                strBuilder.Append("<Subscription>");
                strBuilder.Append("<PackageSubscriptionID>" + subscriptionID.ToString() + "</PackageSubscriptionID>");
                strBuilder.Append("</Subscription>");
            }
            strBuilder.Append("</Subscriptions>");
            return strBuilder.ToString();
        }
        #endregion
        #endregion

        #endregion

        #region Order Package Type
        /// <summary>
        /// Get the Lookups of Order Package type
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Int32 GetOrderPackageTypes(Int32 tenantId, String orderPackageTypeCode)
        {
            List<lkpOrderPackageType> _lstOrderPackageType = LookupManager.GetLookUpData<lkpOrderPackageType>(tenantId);
            return _lstOrderPackageType.Where(opt => opt.OPT_Code == orderPackageTypeCode).FirstOrDefault().OPT_ID;
        }
        #endregion

        #region UAT-1395:Change Subscription/Data sync bugs found by QA
        //Insert Subscription Detail in Data Sunc History
        public static void SaveDataSyncHistory(Int32 tenantId, String subscriptionXml, Int32 currentLoggedInUSerID)
        {
            try
            {
                BALUtils.GetClientMobilityRepoInstance(tenantId).SaveDataSyncHistory(subscriptionXml, currentLoggedInUSerID, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Int32> GetPackageSubscriptionIDForChangeSub(Int32 tenantId, List<Int32> pkgSubscriptionIds)
        {
            try
            {
                String chngSubTypeCode = OrderRequestType.ChangeSubscription.GetStringValue();
                Int32 chngSubTypeID = LookupManager.GetLookUpData<lkpOrderRequestType>(tenantId).FirstOrDefault(cond => cond.ORT_Code == chngSubTypeCode).ORT_ID;
                return BALUtils.GetClientMobilityRepoInstance(tenantId).GetPackageSubscriptionIDForChangeSub(pkgSubscriptionIds, chngSubTypeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #endregion

        #region UAT-1476:WB: When a tracking package is ordered and there was already a previous package with entered data,
        //then there would be data movement as if there were a subscription change.
        public static void CopyCompPackageDataForNewOrder(Int32 tenantID, Int32 orderID, Int32 currentLoggedInUserID)
        {
            try
            {
                //Get newly purchased order which have compliance package 
                Order purchasedOrder = ComplianceDataManager.GetOrderById(tenantID, orderID);

                if (!purchasedOrder.IsNullOrEmpty() && purchasedOrder.lkpOrderRequestType.IsNotNull()
                     && purchasedOrder.lkpOrderRequestType.ORT_Code == OrderRequestType.NewOrder.GetStringValue()
                     && (purchasedOrder.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue()
                     || purchasedOrder.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()))
                {
                    String orderPaidStatusCode = ApplicantOrderStatus.Paid.GetStringValue();
                    String compliancePackageTypeCode = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();

                    //Check is compliance package payment is paind or not.
                    Boolean isOrderPaid = purchasedOrder.OrderPaymentDetails
                                            .Any(opd => opd.OrderPkgPaymentDetails
                        .Any(oppd => oppd.lkpOrderPackageType.OPT_Code == compliancePackageTypeCode && !oppd.OPPD_IsDeleted)
                          && !opd.OPD_IsDeleted && opd.lkpOrderStatu != null && opd.lkpOrderStatu.Code == orderPaidStatusCode);

                    //Purchased Package Subscription.
                    PackageSubscription purchasedSubs = purchasedOrder.PackageSubscriptions.FirstOrDefault();

                    if (isOrderPaid && !purchasedSubs.IsNullOrEmpty())
                    {
                        //Get Complinace Package Copy data mappings and Source subscription id.
                        CompliancePackageCopyDataMapping compPkgCopyDataMapping = BALUtils.GetClientMobilityRepoInstance(tenantID)
                                                                             .GetCompliancePackageCopyDataMapping(tenantID, purchasedSubs.CompliancePackageID,
                                                                               purchasedSubs.OrganizationUserID.Value, purchasedOrder.SelectedNodeID.Value);
                        if (!compPkgCopyDataMapping.IsNullOrEmpty())
                        {
                            //Update data in Package Subscription
                            /*1. Set Mapping Master ID: Its a master mapping Id of target and source package.
                              2. Set PkgMappingInstanceId: Its latest reviewed mapping instance id of Target and Source package mapping.
                              3. Set Subscription mobility status to data movement pending.
                             */
                            Int16? dataMovementSubMobilityStatusId = GetMobilityStatusIDByCode(tenantID, LkpSubscriptionMobilityStatus.DataMovementDue);
                            purchasedSubs.MappingMasterID = compPkgCopyDataMapping.MappingID;
                            purchasedSubs.PkgMappingInstanceID = compPkgCopyDataMapping.PkgMappingInstanceID;
                            purchasedSubs.SubscriptionMobilityStatusID = dataMovementSubMobilityStatusId;
                            purchasedSubs.ModifiedOn = DateTime.Now;
                            purchasedSubs.ModifiedByID = currentLoggedInUserID;
                            ComplianceDataManager.UpdateChanges(tenantID);

                            List<SourceTargetSubscriptionList> lstSourceTargetList = new List<SourceTargetSubscriptionList>();
                            lstSourceTargetList.Add(new SourceTargetSubscriptionList
                                                     {
                                                         SourceSubscriptionID = compPkgCopyDataMapping.SourceSubscriptionID,
                                                         TargetSubscriptionID = purchasedSubs.PackageSubscriptionID
                                                     });

                            //Copy Data From Previous expired subscription to newly purchased order subscription.
                            List<PackageSubscriptionList> targetSubscriptionIds = MobilityManager.CopyPackageData(tenantID, lstSourceTargetList, currentLoggedInUserID);
                            Int16? DataMovedStatusId = MobilityManager.GetMobilityStatusIDByCode(tenantID, LkpSubscriptionMobilityStatus.DataMovementComplete);
                            List<Int32> targetIds = targetSubscriptionIds.Where(item => item.SubscriptionMobilityStatusID == DataMovedStatusId)
                                                                               .Select(item => item.TargetSubscriptionID).ToList();

                            if (targetIds.IsNotNull() && targetIds.Count > 0)
                            {
                                /*Insert copied subscription record in data sync history
                                Background data will not sync for those subscriptions which are requested for change subscription and those package data were copied */
                                List<Int32> targetPkgSubIdsForChangeSubRequest = new List<Int32>();
                                targetPkgSubIdsForChangeSubRequest.Add(purchasedSubs.PackageSubscriptionID);
                                if (!targetPkgSubIdsForChangeSubRequest.IsNullOrEmpty() && targetPkgSubIdsForChangeSubRequest.Count > AppConsts.NONE)
                                {
                                    String subscriptionListXML = GetSubscriptsXMlForDatSyncHistory(targetPkgSubIdsForChangeSubRequest);
                                    SaveDataSyncHistory(tenantID, subscriptionListXML, currentLoggedInUserID);
                                }

                                //Check - Remove susbcriptions from targetIdsForDataMoved which are archived.
                                Int32 ArchivedID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpArchiveState>(tenantID)
                                                                .FirstOrDefault(cnd => cnd.AS_Code == ArchiveState.Archived.GetStringValue() && cnd.AS_IsDeleted == false).AS_ID;
                                List<Int32> targetIdsForDataMoved = targetSubscriptionIds.Where(item => item.ArchiveStateID != ArchivedID)
                                                                                          .Select(item => item.TargetSubscriptionID).ToList();
                                //Execute Rules on Newly Copied Subscription data.
                                RuleManager.ExecuteBusinessRules(targetIdsForDataMoved, tenantID, currentLoggedInUserID);

                                //UAT-2618
                                if (targetSubscriptionIds.IsNotNull() && targetSubscriptionIds.Count > 0)
                                {
                                    foreach (PackageSubscriptionList packageSubscription in targetSubscriptionIds)
                                    {
                                        ComplianceDataManager.UpdateIsDocAssociated(tenantID, packageSubscription.TargetSubscriptionID, true, currentLoggedInUserID);

                                        //UAT-3805

                                        PackageSubscription pkgSubscription = ComplianceDataManager.GetPackageSubscriptionByID(tenantID, packageSubscription.TargetSubscriptionID);

                                        if (!pkgSubscription.IsNullOrEmpty())
                                        {
                                            List<Int32> catIDs = pkgSubscription.ApplicantComplianceCategoryDatas.Where(cnd => !cnd.IsDeleted)
                                                                                                                 .Select(slct => slct.ComplianceCategoryID).ToList();
                                            if (!catIDs.IsNullOrEmpty())
                                            {
                                                String categoryIds = String.Join(",", catIDs);
                                                ProfileSharingManager.SendItemDocNotificationOnCategoryApproval(tenantID, categoryIds, pkgSubscription.OrganizationUserID.Value
                                                                                                                , String.Empty, lkpUseTypeEnum.COMPLIANCE.GetStringValue()
                                                                                                                , pkgSubscription.PackageSubscriptionID, null, currentLoggedInUserID);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                purchasedSubs.MappingMasterID = null;
                                purchasedSubs.PkgMappingInstanceID = null;
                                purchasedSubs.SubscriptionMobilityStatusID = null;
                                purchasedSubs.ModifiedOn = DateTime.Now;
                                purchasedSubs.ModifiedByID = currentLoggedInUserID;
                                ComplianceDataManager.UpdateChanges(tenantID);
                            }
                        }
                    }
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }

        }
        #endregion

        #region UAT-2387

        public static List<usp_SubscriptionChange_Result> ChangePackageAndSubscription(Int32 tenantId, String xml, Boolean isOnlyPackageChange)
        {
            try
            {
                return BALUtils.GetClientMobilityRepoInstance(tenantId).ChangePackageAndSubscription(xml, isOnlyPackageChange);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

    }
}
