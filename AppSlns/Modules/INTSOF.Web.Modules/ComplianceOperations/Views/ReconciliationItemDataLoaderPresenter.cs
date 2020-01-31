using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Data;
using System.Linq;
using System.Web;
using INTSOF.Contracts;
using INTSOF.Utils.Consts;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.ComplianceRuleEngine;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ReconciliationItemDataLoaderPresenter : Presenter<IReconciliationItemDataLoaderView>
    {
        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public ItemDataLoaderPresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        StringBuilder queueFieldString = null;

        #region PUBLIC METHODS

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
            GetRejectionReasons();
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public String GetTenantType()
        {
            Entity.Tenant _tenant = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.Tenant;
            //View.SelectedTenantId = _tenant.TenantID;
            View.IsDefaultTenant = _tenant.TenantID.Equals(SecurityManager.DefaultTenantID);
            return _tenant.lkpTenantType.TenantTypeCode;
        }

        public Boolean IsThirdPartyTenant
        {
            get
            {
                return SecurityManager.IsTenantThirdPartyType(View.SelectedTenantId_Global, TenantType.Compliance_Reviewer.GetStringValue());
            }
        }

        public List<AssignmentHierarchyEditableByContract> GetCategoryLevelEditableBies()
        {
            return ComplianceSetupManager.GetEditableBies(View.SelectedCompliancePackageId_Global, View.SelectedComplianceCategoryId_Global, false, View.SelectedTenantId_Global);
        }

        public List<ListItemEditableBies> GetEditableBiesByCategoryId()
        {
            return ComplianceSetupManager.GetEditableBiesByCategoryId(View.SelectedCompliancePackageId_Global, View.SelectedComplianceCategoryId_Global, View.SelectedTenantId_Global);
        }

        public List<Entity.ClientEntity.ApplicantItemVerificationData> GetApplicantItemVerificationData()
        {
            return ComplianceDataManager.GetApplicantDataForVerification
                                        (View.SelectedComplianceCategoryId_Global, View.CurrentPackageSubscriptionID_Global, View.SelectedTenantId_Global);
        }

        public Entity.ClientEntity.PackageSubscription GetPackageSubscriptionByPackageID()
        {
            return ComplianceDataManager.GetPackageSubscriptionByID(View.SelectedTenantId_Global, View.CurrentPackageSubscriptionID_Global);
        }

        public List<ListItemAssignmentProperties> GetAssignmentPropertiesByCategoryId()
        {
            return ComplianceSetupManager.GetAssignmentPropertiesByCategoryId(View.SelectedCompliancePackageId_Global, View.SelectedComplianceCategoryId_Global, View.SelectedTenantId_Global);
        }
        public Dictionary<Int32, String> ValidateApplicantData(List<ApplicantComplianceItemData> lstItemData, List<ApplicantComplianceAttributeData> lstAttributeData)
        {
            if (View.IsUIValidationApplicable)
            {
                return ComplianceDataManager.ValidateUIRulesVerificationDetail(lstItemData, lstAttributeData, View.CurrentPackageSubscriptionID_Global,
              View.SelectedCompliancePackageId_Global, View.SelectedComplianceCategoryId_Global, View.SelectedApplicantId_Global, View.SelectedTenantId_Global);
            }
            return new Dictionary<Int32, String>();
        }

        public List<CatUpdatedByLatestInfo> GetCatUpdatedByLatestInfo(Int32 CategoryID)
        {
            return ComplianceDataManager.GetCatUpdatedByLatestInfo(CategoryID, View.CurrentPackageSubscriptionID_Global, View.SelectedTenantId_Global);
        }

        public List<ApplicantDocumentMappingData> GetApplicantDocumentsData(String xmlItemIds, String xmlAttributeIds)
        {
            return StoredProcedureManagers.GetDocumentMappings(xmlAttributeIds, xmlItemIds, View.SelectedTenantId_Global);
        }

        /// <summary>
        /// Gets the next possible actions for the Items, including the Exception & Incomplete items and distribute them into the different lists.
        /// </summary>
        /// <param name="lstItemData"></param>
        /// <param name="_lstNextReviewRecord"></param>
        /// <param name="_lstNextQueueRecord"></param>
        /// <param name="_lstEscalationRecord"></param>
        public void GetNextActionForItems(List<ApplicantComplianceItemData> lstItemData, List<ListItemAssignmentProperties> lstAssignmentProperties, out List<ApplicantComplianceItemData> _lstNextReviewRecord,
            out List<ApplicantComplianceItemData> _lstNextQueueRecord, out List<ApplicantComplianceItemData> _lstEscalationRecord,
            out List<ApplicantComplianceItemData> _lstIncompleteSameQueue)
        {
            Boolean _getNextActions = false;
            // Check for items which are having ApplicantComplianceItemId >  0, Exception Items and their status is getting changed
            List<ApplicantComplianceItemData> _lstItemsNotIncomplete = lstItemData.Where(itmData => itmData.ApplicantComplianceItemID > 0
                                                                       && itmData.AttemptedStatusCode != itmData.CurrentStatusCode
                                                                       && (!String.IsNullOrEmpty(itmData.CurrentStatusCode)
                                                                       && (itmData.CurrentStatusCode != ApplicantItemComplianceStatus.Incomplete.GetStringValue()))
                                                                       ) // Check added to avoid Incomplete items with Documents assigned to them
                                                                      .ToList();

            List<ApplicantComplianceItemData> _lstItemsIncomplete = lstItemData.Where(itmData => itmData.ApplicantComplianceItemID == 0
                                                                            || (String.IsNullOrEmpty(itmData.CurrentStatusCode)
                                                                            && itmData.ApplicantComplianceItemID > 0)
                                                                            || (!String.IsNullOrEmpty(itmData.CurrentStatusCode) // Check added to add Incomplete items with Documents assigned to them
                                                                            && itmData.CurrentStatusCode == ApplicantItemComplianceStatus.Incomplete.GetStringValue()
                                                                            && itmData.ApplicantComplianceItemID > 0)).ToList();


            List<ApplicantComplianceItemData> _lstIncompleteDifferentQueue = new List<ApplicantComplianceItemData>();
            _lstIncompleteSameQueue = new List<ApplicantComplianceItemData>();

            queueFieldString = new StringBuilder();
            queueFieldString.Append("<Queues>");

            foreach (var item in _lstItemsIncomplete)
            {
                if (!CheckIfSameQueue(item.CurrentTenantTypeCode, item.AttemptedStatusCode))
                {
                    _lstIncompleteDifferentQueue.Add(item);
                }
                else
                {
                    if (item.ApplicantComplianceItemID == 0)
                        _lstIncompleteSameQueue.Add(item);
                    else
                    {
                        item.IsIncompleteWithDocuments = true;
                        _lstIncompleteSameQueue.Add(GetItemWithCurrentStatus(item));
                    }
                }
            }
            ////Add expired items in list of IncompleteDifferentQueue.
            //foreach (var item in _lstItemsExpired)
            //{
            //    _lstIncompleteDifferentQueue.Add(item);
            //}

            if (!_lstIncompleteDifferentQueue.IsNullOrEmpty())
            {
                _getNextActions = true;
                GenerateInputXmlIncompleteRecords(_lstIncompleteDifferentQueue);
            }

            if (!_lstItemsNotIncomplete.IsNullOrEmpty())
            {
                _getNextActions = true;
                GenerateInputXmlOther(_lstItemsNotIncomplete, false);
            }

            queueFieldString.Append("</Queues>");

            List<NextQueueAction> _lstNextActions = new List<NextQueueAction>();
            if (_getNextActions)
                _lstNextActions = QueueManagementManager.GetNextQueueAction(Convert.ToString(queueFieldString), View.CurrentLoggedInUserId, View.SelectedTenantId_Global);

            _lstNextReviewRecord = new List<ApplicantComplianceItemData>();
            _lstNextQueueRecord = new List<ApplicantComplianceItemData>();
            _lstEscalationRecord = new List<ApplicantComplianceItemData>();

            foreach (var _nextAction in _lstNextActions)
            {
                ApplicantComplianceItemData _itemData = lstItemData.Where(itmData => itmData.ComplianceItemID == _nextAction.ReferenceId).FirstOrDefault();
                var _assignmentProperties = lstAssignmentProperties.Where(con => con.ComplianceItemId == _nextAction.ReferenceId).ToList();

                var adminReviewerTypeCode = ReviewerType.Admin.GetStringValue();
                var clientAdminReviewerTypeCode = ReviewerType.Client_Admin.GetStringValue();

                // ADD THE INCOMPLETE ITEMS WHICH WERE SENT TO DIFFERENT QUEUE, BUT THEY STILL NEED ANOTHER REVIEW
                if (_nextAction.RecordId == AppConsts.NONE && _nextAction.NextAction == lkpQueueActionType.Next_Level_Review_Required.GetStringValue())
                {
                    _itemData.ReSetInitialReview = true;
                    _itemData = GetItemWithCurrentStatus(_itemData);
                    // This list contains 2 types of Incomplete items, one which were attempted to be in 
                    // same queue and another which were attempted to 
                    // be in different queue, but need another review in same queue

                    //if Reviewer type is only Client Admin and admin is doing data entry or vice-versa and Reviewer Type count or Assignment Properties count is also equal to 1
                    //then move the record in next queue else record will be in same queue
                    if ((_assignmentProperties.Count == AppConsts.ONE) && (((_assignmentProperties.Where(x => x.ReviewerTypeCode == clientAdminReviewerTypeCode).Count() > 0) && View.IsDefaultTenant)
                            || ((_assignmentProperties.Where(x => x.ReviewerTypeCode == adminReviewerTypeCode).Count() > 0) && !View.IsDefaultTenant)))
                    {
                        _lstNextQueueRecord.Add(_itemData);
                    }
                    else
                    {
                        _lstIncompleteSameQueue.Add(_itemData);
                    }
                }
                else if (_nextAction.RecordId > 0 && (String.IsNullOrEmpty(_itemData.CurrentStatusCode) || _itemData.CurrentStatusCode == ApplicantItemComplianceStatus.Incomplete.GetStringValue())
                    && _nextAction.NextAction == lkpQueueActionType.Next_Level_Review_Required.GetStringValue())
                {
                    _itemData.ReSetInitialReview = true; // This is to handle these cases when they are [processed for CallHandleAssignments in NextQueue Listing
                    _itemData.IsIncompleteWithDocuments = true;
                    _itemData = GetItemWithCurrentStatus(_itemData);
                    //if Reviewer type is only Client Admin and admin is doing data entry or vice-versa and Reviewer Type count or Assignment Properties count is also equal to 1
                    //then move the record in next queue else record will be in same queue
                    if ((_assignmentProperties.Count == AppConsts.ONE) && (((_assignmentProperties.Where(x => x.ReviewerTypeCode == clientAdminReviewerTypeCode).Count() > 0) && View.IsDefaultTenant)
                            || ((_assignmentProperties.Where(x => x.ReviewerTypeCode == adminReviewerTypeCode).Count() > 0) && !View.IsDefaultTenant)))
                    {
                        _lstNextQueueRecord.Add(_itemData);
                    }
                    else
                    {
                        _lstIncompleteSameQueue.Add(_itemData);
                    }
                }
                else if (_nextAction.NextAction == lkpQueueActionType.Next_Level_Review_Required.GetStringValue()
                    || (_nextAction.NextAction == lkpQueueActionType.Proceed_To_Next_Queue.GetStringValue()
                       && _nextAction.MaxReviewLevels > AppConsts.NONE))
                {
                    //if Reviewer type is only Client Admin and admin is doing data entry or vice-versa and Reviewer Type count or Assignment Properties count is also equal to 1
                    //then move the record in next queue else record will be in same queue
                    if ((_assignmentProperties.Count == AppConsts.ONE) && (((_assignmentProperties.Where(x => x.ReviewerTypeCode == clientAdminReviewerTypeCode).Count() > 0) && View.IsDefaultTenant)
                            || ((_assignmentProperties.Where(x => x.ReviewerTypeCode == adminReviewerTypeCode).Count() > 0) && !View.IsDefaultTenant)))
                    {
                        _lstNextQueueRecord.Add(_itemData);
                    }
                    else
                    {
                        _lstNextReviewRecord.Add(_itemData);
                    }
                }
                else if (_nextAction.NextAction == lkpQueueActionType.Proceed_To_Next_Queue.GetStringValue())
                {
                    if (String.IsNullOrEmpty(_itemData.CurrentStatusCode) || _itemData.CurrentStatusCode == ApplicantItemComplianceStatus.Incomplete.GetStringValue()
                        )
                    {
                        _itemData.IsIncompleteWithDocuments = true;
                        //Set status for incomplete records
                        _itemData = GetItemWithCurrentStatus(_itemData);
                    }
                    _lstNextQueueRecord.Add(_itemData);
                }
                else if (_nextAction.NextAction == lkpQueueActionType.Escalation_Required.GetStringValue())
                {
                    _lstEscalationRecord.Add(_itemData);
                }
            }
        }

        private ApplicantComplianceItemData GetItemWithCurrentStatus(ApplicantComplianceItemData _itemData)
        {
            List<lkpItemComplianceStatu> _lstItemComplianceStatus = LookupManager.GetLookUpData<lkpItemComplianceStatu>(View.SelectedTenantId_Global);
            if (_itemData.CurrentTenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue())
            {
                _itemData.CurrentStatusCode = ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue();
                _itemData.StatusID = _lstItemComplianceStatus.FirstOrDefault(x => x.Code.Equals(_itemData.CurrentStatusCode)).ItemComplianceStatusID;
            }
            else if (_itemData.CurrentTenantTypeCode == TenantType.Institution.GetStringValue())
            {
                _itemData.CurrentStatusCode = ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue();
                _itemData.StatusID = _lstItemComplianceStatus.FirstOrDefault(x => x.Code.Equals(_itemData.CurrentStatusCode)).ItemComplianceStatusID;
            }
            else if (View.CurrentTenantId_Global == SecurityManager.DefaultTenantID)
            {
                _itemData.CurrentStatusCode = ApplicantItemComplianceStatus.Pending_Review.GetStringValue();
                _itemData.StatusID = _lstItemComplianceStatus.FirstOrDefault(x => x.Code.Equals(_itemData.CurrentStatusCode)).ItemComplianceStatusID;
            }
            return _itemData;
        }

        /// <summary>
        /// Get the speclalizations for the items, for the current logged in user.
        /// </summary>
        /// <param name="lstVerificationData"></param>
        /// <param name="currentTenantTypeCode"></param>
        /// <returns></returns>
        public List<UserSpecializationDetails> GetUserSpecializations(List<ApplicantItemVerificationData> lstVerificationData, String currentTenantTypeCode)
        {
            List<ApplicantComplianceItemData> _lstData = new List<ApplicantComplianceItemData>();

            foreach (var item in lstVerificationData)
            {

                _lstData.Add(new ApplicantComplianceItemData
                {
                    CurrentStatusCode = item.ItemComplianceStatusCode,
                    ApplicantComplianceItemID = Convert.ToInt32(item.ApplicantCompItemId),
                    ComplianceItemID = item.ComplianceItemId,
                    CurrentTenantTypeCode = currentTenantTypeCode
                });
            }
            if (!_lstData.IsNullOrEmpty())
            {
                queueFieldString = new StringBuilder();
                queueFieldString.Append("<Queues>");
                GenerateXMLUserSpecializations(_lstData);
                queueFieldString.Append("</Queues>");
                return QueueManagementManager.GetUserSpecializationDetails(View.CurrentLoggedInUserId, View.SelectedTenantId_Global, Convert.ToString(queueFieldString));
            }
            return new List<UserSpecializationDetails>();
        }

        /// <summary>
        ///  Call handle assignments for the all types of Items
        /// </summary>
        /// <param name="lstNextQueueRecords"></param>
        /// <param name="lstNextReviewRecords"></param>
        /// <param name="lstIncompleteSameQueue"></param>
        /// <param name="lstEscalateRecords"></param>
        /// <param name="lstAlreadyEscalated">Already escalated but status NOT EQUAL To Approved, rejected etc.</param>
        public void CallHandleAssignmentParallelTask(List<ApplicantComplianceItemData> lstNextQueueRecords)
        {
            StringBuilder _handleAssignmentXML = new StringBuilder();

            Boolean _shouldRunParallelTask = false;

            _handleAssignmentXML.Append("<Queues>");



            #region Generate XML for records that are to be moved to Next Queue

            /* Contains the record with data entry and need to move to next Queue + 
             * Attempt was different queue and should be moved to next queue */
            if (!lstNextQueueRecords.IsNullOrEmpty()) // Generate XML of Items which are to be moved to Next Queue, with New Status and QueueId
            {
                _shouldRunParallelTask = true;
                _handleAssignmentXML.Append(GenerateInputXmlHandleAssignment(lstNextQueueRecords, true, false, false));
            }

            //if (!lstVerificationCompleted.IsNullOrEmpty())
            //{
            //    _shouldRunParallelTask = true;
            //    _handleAssignmentXML.Append(GenerateInputXmlHandleAssignment(lstVerificationCompleted, true, false, false));
            //}

            #endregion

            _handleAssignmentXML.Append("</Queues>");

            var _loggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var _exceptionService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            Dictionary<String, Object> dicHandleAssignmentData = new Dictionary<String, Object>();

            dicHandleAssignmentData.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);
            dicHandleAssignmentData.Add("TenantId", View.SelectedTenantId_Global);
            dicHandleAssignmentData.Add("QueueRecordXML", Convert.ToString(_handleAssignmentXML));
            dicHandleAssignmentData.Add("ResetReviewLevel", false);

            if (_shouldRunParallelTask) // Run the Parallel task, only if XML is generated for any of the Item types
                QueueManagementManager.RunParallelTaskHandleAssignment(dicHandleAssignmentData, _loggerService, _exceptionService, View.SelectedTenantId_Global);
        }

        /// <summary>
        /// Call the ClearQueueRecords stored procedure.
        /// </summary>
        /// <param name="lstNextQueueRecords"></param>
        /// <param name="lstNextReviewRecords"></param>
        /// <param name="lstDeleteRecords"></param>
        /// <param name="lstReviewCompleted"></param>
        /// <param name="lstEscalationRecords">Records which are to be escalated</param>
        /// <param name="lstEscalationResolved"></param>
        /// <param name="lstFinalEscalationResolved">Escalated items which were approved, rejected etc.</param>
        /// <returns></returns>
        public void ClearQueueRecords()
        {
            QueueManagementManager.ResetReconciliationProcess(View.CurrentLoggedInUserId, View.ItemDataId_Global, true, View.SelectedTenantId_Global);
        }

        /// <summary>
        /// Escalate the records, which are neither to be moved to next queue or next review
        /// Change status of the Already escalated records
        /// </summary>
        /// <param name="lstEscalateQueueRecords"></param>
        /// <returns></returns>
        public void ApplyEscalationChanges(List<ApplicantComplianceItemData> lstRecordsToEscalate, List<ApplicantComplianceItemData> lstAlreadyEscalated)
        {

            if (!lstRecordsToEscalate.IsNullOrEmpty())
            {
                queueFieldString = new StringBuilder();
                queueFieldString.Append("<Queues>");

                GenerateInputXmlOther(lstRecordsToEscalate, false, true);
                queueFieldString.Append("</Queues>");
                QueueManagementManager.EscalateItems(View.CurrentLoggedInUserId, Convert.ToString(queueFieldString), View.SelectedTenantId_Global);
            }
            if (!lstAlreadyEscalated.IsNullOrEmpty())
            {
                queueFieldString = new StringBuilder();
                queueFieldString.Append("<Queues>");

                GenerateInputXmlOther(lstAlreadyEscalated, false, true);
                queueFieldString.Append("</Queues>");
                QueueManagementManager.EscalateItems(View.CurrentLoggedInUserId, Convert.ToString(queueFieldString), View.SelectedTenantId_Global);
            }

        }

        public String GetNodeHiearchy(Int32 tenantId, Int32 packageSubscriptionId)
        {
            return ComplianceDataManager.GetNodeHiearchy(tenantId, packageSubscriptionId);
        }

        public String GetInstitutionUrl(int tenantId)
        {
            return WebSiteManager.GetInstitutionUrl(tenantId);
            //var webSite = WebSiteManager.GetWebSiteDetail(tenantId);
            //String applicationUrl = String.Empty;
            //if (webSite.IsNotNull() && webSite.WebSiteID != SysXDBConsts.NONE)
            //{
            //    applicationUrl = webSite.URL;
            //}
            //else
            //{
            //    webSite = WebSiteManager.GetWebSiteDetail(1);
            //    applicationUrl = webSite.URL;
            //}

            //if (!(applicationUrl.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || applicationUrl.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            //{
            //    applicationUrl = string.Concat(HttpContext.Current.Request.Url.Scheme, "://", applicationUrl.Trim());
            //}

            //return applicationUrl;
        }

        public String GetInstitutionName(int tenantId)
        {
            return ClientSecurityManager.GetTenantName(tenantId);
        }

        public String GetApplicantPrimaryEmail(Int32 applicantId)
        {
            String primaryEmail = String.Empty;
            Entity.OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(applicantId);
            if (organizationUser.IsNotNull())
            {
                primaryEmail = organizationUser.PrimaryEmailAddress;
            }
            return primaryEmail;
        }

        public void CallParallelTaskForMail(List<ExceptionRejectionContract> lstExceptionRejectionContract, String primaryEmail, Int32 tenantID, Int32 CurrentLoggedInUserId)
        {
            Dictionary<String, Object> mailData = new Dictionary<String, Object>();

            mailData.Add("lstExceptionRejectionContract", lstExceptionRejectionContract);
            mailData.Add("primaryEmail", primaryEmail);
            mailData.Add("tenantID", tenantID);
            //UAT-2172
            mailData.Add("CurrentLoggedInUserId", CurrentLoggedInUserId);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            ParallelTaskContext.PerformParallelTask(SendRejectionMail, mailData, LoggerService, ExceptiomService);
        }


        public void SendRejectionMail(Dictionary<String, Object> data)
        {
            List<ExceptionRejectionContract> lstExceptionRejectionContract = data.GetValue("lstExceptionRejectionContract") as List<ExceptionRejectionContract>;
            String primaryEmail = data.GetValue("primaryEmail") as String;
            Int32 tenantID = Convert.ToInt32(data.GetValue("tenantID"));

            //UAT-2172
            Int32 currentLoggedInUserId = Convert.ToInt32(data.GetValue("CurrentLoggedInUserId"));
            var organisationuser = SecurityManager.GetOrganizationUser(currentLoggedInUserId);
            String adminName = String.Empty;
            if (organisationuser.Organization.TenantID == SecurityManager.DefaultTenantID)
            {
                adminName = "ADB Administrator";
            }
            else
            {
                String firstname = String.Empty;
                String lastname = String.Empty;
                String middleName = String.Empty;
                firstname = organisationuser.FirstName.IsNullOrEmpty() ? String.Empty : organisationuser.FirstName.Trim();
                lastname = organisationuser.LastName.IsNullOrEmpty() ? String.Empty : organisationuser.LastName.Trim();
                middleName = organisationuser.MiddleName.IsNullOrEmpty() ? String.Empty : organisationuser.MiddleName.Trim();
                adminName = firstname + " " + middleName + " " + lastname;
            }

            foreach (var excRejContract in lstExceptionRejectionContract)
            {
                Boolean isCategory = false;
                if (excRejContract.ItemID != null)
                {
                    isCategory = ComplianceDataManager.IsCategoryException(tenantID, excRejContract.ItemID.Value);
                }
                CommunicationSubEvents commSubEvents = CommunicationSubEvents.NOTIFICATION_FOR_ITEM_REJECTED;
                if (isCategory)
                    commSubEvents = CommunicationSubEvents.NOTIFICATION_FOR_CATEGORY_REJECTED;

                //Create Dictionary for Email Content
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, excRejContract.UserFullName);
                //dictMailData.Add(EmailFieldConstants.COMPLIANCE_ITEM_NAME, excRejContract.ComplianceItemName);
                //dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, excRejContract.PackageName);
                //dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, excRejContract.NodeHierarchy);
                //dictMailData.Add(EmailFieldConstants.CATEGORY_NAME, excRejContract.CategoryName);
                //dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, excRejContract.InstituteName);
                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, excRejContract.ApplicationUrl);
                if (!isCategory)
                {
                    //UAT-2172
                    dictMailData.Add(EmailFieldConstants.ADMINISTRATOR, adminName);

                    dictMailData.Add(EmailFieldConstants.REJECTION_REASON, excRejContract.RejectionReason);
                    //UAT-1519: Add the ability to include the Category Explanatory note in the Item rejected email notification.
                    dictMailData.Add(EmailFieldConstants.CATEGORY_EXPLANATORY_NOTES, excRejContract.CategoryExplanatoryNotes);
                    String categoryMoreInfoURL;
                    if (!excRejContract.CategoryMoreInfoURL.IsNullOrEmpty())
                    {
                        categoryMoreInfoURL = excRejContract.CategoryMoreInfoURL;
                    }
                    else
                    {
                        categoryMoreInfoURL = "Specific form is not applicable for this category.";
                    }
                    dictMailData.Add(EmailFieldConstants.CATEGORY_CPF_LINK, categoryMoreInfoURL);
                }

                //Create dictionary for MockUp Data
                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                mockData.UserName = string.Concat(excRejContract.UserFullName);
                mockData.EmailID = primaryEmail;
                mockData.ReceiverOrganizationUserID = excRejContract.ApplicantID;

                //Send mail
                CommunicationManager.SendPackageNotificationMail(commSubEvents, dictMailData, mockData, tenantID, excRejContract.HierarchyNodeID);

                //Send Message
                CommunicationManager.SaveMessageContent(commSubEvents, dictMailData, excRejContract.ApplicantID, tenantID);
            }
        }

        #endregion

        #region PRIVATE METHODS


        /// <summary>
        /// Returns the List of items, which are not in the List 'lstIncompleteDifferentQueue', 
        /// as they were added to the 'lstNextQueueRecord' AND 'lstNextReviewRecord' lists by the 'GetNextAction' method.
        /// </summary>
        /// <param name="lstQueueRecords"></param>
        /// <param name="lstIncompleteDifferentQueue"></param>
        /// <returns></returns>
        private List<ApplicantComplianceItemData> GetItemsNotInIncompleteDifferentQueue(List<ApplicantComplianceItemData> lstQueueRecords, List<ApplicantComplianceItemData> lstIncompleteDifferentQueue)
        {
            List<ApplicantComplianceItemData> _lstTemp = new List<ApplicantComplianceItemData>();
            foreach (var record in lstQueueRecords)
            {
                if (!lstIncompleteDifferentQueue.Where(itm => itm.ComplianceItemID == record.ComplianceItemID).Any())
                    _lstTemp.Add(record);
            }
            return _lstTemp;
        }

        /// <summary>
        ///  Generate Input XML for the cases for input to 'ClearQueueRecords' stored procedure.
        /// </summary>
        /// <param name="lstItemData"></param>
        /// <param name="lstQueueMetaData"></param>
        /// <param name="sameQueueRecords">Will be True if the Item is moved into the same queue.</param>
        /// <param name="nextQueueApplicable"></param>
        /// <param name="isEscalationTypeList">Will be true if the Method is called for the Escalation records list.</param>
        /// <returns></returns>
        private String GenerateInputXMLClearRecords(List<ApplicantComplianceItemData> lstItemData, List<QueueMetaData> lstQueueMetaData,
                                                    Boolean isEscalationTypeList, Boolean sameQueueRecords, Boolean nextQueueApplicable = true)
        {
            StringBuilder _sbInputXML = new StringBuilder();
            foreach (var itemData in lstItemData)
            {

                Int32 _oldQueueId = AppConsts.NONE;
                Int32? _newQueueId = AppConsts.NONE;
                String _queueTypeCodeAttempted = String.Empty;

                //String _queueTypeCodeCurrent = GetQueueTypeCodeByItemStatusCode(itemData.CurrentTenantTypeCode, itemData.CurrentStatusCode);
                String _queueTypeCodeCurrent = GetQueueTypeCodeByStatus(itemData.CurrentStatusCode);

                // Records are to be Deleted  will satisfy this condition in the 'lstDeleteRecords' List
                // OR already approved, rejected, approved with exception and exception rejected in the 'lstReviewCompleted' List
                if (!nextQueueApplicable)
                {
                    _oldQueueId = lstQueueMetaData.Where(qmd => qmd.QMD_Code == _queueTypeCodeCurrent && qmd.QMD_IsDeleted == false).FirstOrDefault().QMD_QueueID;
                    _newQueueId = null;
                }
                else
                {
                    if (!sameQueueRecords)
                    {
                        #region Commented
                        /// For items which are having following status, will always have New QueueId as null. 
                        /// Added for the 'lstDeleteItems' & 'lstReviewCompleted'
                        //if (itemData.AttemptedStatusCode == ApplicantItemComplianceStatus.Approved.GetStringValue() || itemData.AttemptedStatusCode == ApplicantItemComplianceStatus.Not_Approved.GetStringValue()
                        //    || itemData.AttemptedStatusCode == ApplicantItemComplianceStatus.Exception_Approved.GetStringValue() || itemData.AttemptedStatusCode == ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue())
                        //    _newQueueId = null; 
                        #endregion

                        _oldQueueId = lstQueueMetaData.Where(qmd => qmd.QMD_Code == _queueTypeCodeCurrent && qmd.QMD_IsDeleted == false).FirstOrDefault().QMD_QueueID;
                        if (isEscalationTypeList) // Executes for records that are to be Escalated
                            _queueTypeCodeAttempted = GetQueueTypeCodeByStatus(itemData.CurrentStatusCode, true);
                        else
                            _queueTypeCodeAttempted = GetQueueTypeCodeByStatus(itemData.AttemptedStatusCode); // NEED to verify this methods functionality

                        _newQueueId = lstQueueMetaData.Where(qmd => qmd.QMD_Code == _queueTypeCodeAttempted && qmd.QMD_IsDeleted == false).FirstOrDefault().QMD_QueueID;
                    }
                    else
                        _newQueueId = _oldQueueId = lstQueueMetaData.Where(qmd => qmd.QMD_Code == _queueTypeCodeCurrent && qmd.QMD_IsDeleted == false).FirstOrDefault().QMD_QueueID;
                }
                _sbInputXML.Append("<QueueDetail>");

                _sbInputXML.Append("<OldQueueID>" + _oldQueueId + "</OldQueueID>");
                if (_newQueueId.IsNotNull())
                    _sbInputXML.Append("<NewQueueID>" + _newQueueId + "</NewQueueID>");
                _sbInputXML.Append("<RecordID>" + itemData.ApplicantComplianceItemID + "</RecordID>");

                _sbInputXML.Append("</QueueDetail>");
            }
            return Convert.ToString(_sbInputXML);
        }

        /// <summary>
        /// Items already Escalated, are being saved.
        /// </summary>
        /// <param name="lstItemData"></param>
        /// <param name="lstQueueMetaData"></param>
        /// <param name="isNextQueueApplicable"></param>
        /// <returns></returns>
        private String GenerateInputXMLAlreadyEscalatedRecords(List<ApplicantComplianceItemData> lstItemData, List<QueueMetaData> lstQueueMetaData, Boolean isNextQueueApplicable)
        {
            StringBuilder _sbInputXML = new StringBuilder();
            foreach (var itemData in lstItemData)
            {
                Int32 _oldQueueId = AppConsts.NONE;
                Int32? _newQueueId = AppConsts.NONE;
                //String _queueTypeCodeCurrent = GetQueueTypeCodeByItemStatusCode(itemData.CurrentTenantTypeCode, itemData.CurrentStatusCode);
                String _queueTypeCodeCurrent = GetQueueTypeCodeByStatus(itemData.CurrentStatusCode, true);
                String _queueTypeCodeNew = String.Empty;

                _oldQueueId = lstQueueMetaData.Where(qmd => qmd.QMD_Code == _queueTypeCodeCurrent && qmd.QMD_IsDeleted == false).FirstOrDefault().QMD_QueueID;

                if (!isNextQueueApplicable)
                    _newQueueId = null;
                else
                {
                    _queueTypeCodeNew = GetQueueTypeCodeByStatus(itemData.CurrentStatusCode);
                    _newQueueId = lstQueueMetaData.Where(qmd => qmd.QMD_Code == _queueTypeCodeNew && qmd.QMD_IsDeleted == false).FirstOrDefault().QMD_QueueID;
                }
                _sbInputXML.Append("<QueueDetail>");
                _sbInputXML.Append("<OldQueueID>" + _oldQueueId + "</OldQueueID>");

                if (_newQueueId.IsNotNull())
                    _sbInputXML.Append("<NewQueueID>" + _newQueueId + "</NewQueueID>");

                _sbInputXML.Append("<RecordID>" + itemData.ApplicantComplianceItemID + "</RecordID>");

                _sbInputXML.Append("</QueueDetail>");
            }
            return Convert.ToString(_sbInputXML);
        }



        /// <summary>
        /// Generate XML for input of 'GetNextAction' and 'User Specializations'
        /// Generate XML for input of 'ApplyEscalationChanges' 
        /// </summary>
        /// <param name="lstItems"></param>
        /// <param name="useNewStatus">True for the items, which are getting mvoed to different queues.</param>
        private void GenerateInputXmlOther(List<ApplicantComplianceItemData> lstItems, Boolean useNewStatus, Boolean isEscalationRecords = false)
        {
            String _queueTypeCode = String.Empty;

            List<QueueMetaData> _lstQueueMetaData = LookupManager.GetLookUpData<QueueMetaData>(View.SelectedTenantId_Global);

            foreach (var itemData in lstItems)
            {
                //String _statusCode = useNewStatus ? itemData.AttemptedStatusCode : itemData.CurrentStatusCode;
                if (useNewStatus)
                    _queueTypeCode = GetQueueTypeCodeByStatus(itemData.AttemptedStatusCode);
                else
                {
                    if (isEscalationRecords)
                    {
                        if (itemData.IsEscalatedItem)
                            _queueTypeCode = GetQueueTypeCodeByStatus(itemData.CurrentStatusCode, true);
                        else
                            _queueTypeCode = GetQueueTypeCodeByStatus(itemData.CurrentStatusCode);
                    }
                    else
                        _queueTypeCode = GetQueueTypeCodeByStatus(itemData.CurrentStatusCode); // Need to check
                    // _queueTypeCode = GetQueueTypeCodeByItemStatusCode(itemData.CurrentTenantTypeCode, itemData.CurrentStatusCode); // Need to check
                }

                QueueMetaData _queueMetaData = _lstQueueMetaData.Where(qmd => qmd.QMD_Code == _queueTypeCode && qmd.QMD_IsDeleted == false).FirstOrDefault();

                Int32 _queueId = _queueMetaData.QMD_QueueID;
                Dictionary<String, Object> dicQueueFields = new Dictionary<String, Object>();
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ApplicantComplianceItemID, itemData.ApplicantComplianceItemID);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.PackageID, View.SelectedCompliancePackageId_Global);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.CategoryId, View.SelectedComplianceCategoryId_Global);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ComplianceItemId, itemData.ComplianceItemID);

                foreach (KeyValuePair<String, Object> dicQueueField in dicQueueFields)
                {
                    queueFieldString.Append("<QueueDetail>");
                    queueFieldString.Append("<QueueID>" + _queueId + "</QueueID>"); // Need to change when Moved to next Queue and NOT Same level
                    queueFieldString.Append("<RecordID>" + itemData.ApplicantComplianceItemID + "</RecordID>");
                    queueFieldString.Append("<QueueFieldName>" + dicQueueField.Key + "</QueueFieldName>");
                    queueFieldString.Append("<QueueFieldValue>" + dicQueueField.Value + "</QueueFieldValue>");
                    queueFieldString.Append("<ReferenceID>" + itemData.ComplianceItemID + "</ReferenceID>");
                    queueFieldString.Append("<LifeCycleFieldValue>" + itemData.AttemptedItemStatusId + "</LifeCycleFieldValue>");

                    if (isEscalationRecords)
                    {
                        if (itemData.IsEscalatedItem)
                            queueFieldString.Append("<EscalationResolved>" + true + "</EscalationResolved>");
                        else
                            queueFieldString.Append("<EscalationResolved>" + false + "</EscalationResolved>");
                    }

                    queueFieldString.Append("</QueueDetail>");
                }
                queueFieldString.ToString();
            }
        }

        /// <summary>
        /// Generate XML for input of 'GetNextAction' for the incomplete records'
        /// </summary>
        /// <param name="lstItems"></param>
        /// <param name="useNewStatus">True for the items, which are getting mvoed to different queues.</param>
        private void GenerateInputXmlIncompleteRecords(List<ApplicantComplianceItemData> lstItems)
        {
            String _queueTypeCode = String.Empty;

            List<QueueMetaData> _lstQueueMetaData = LookupManager.GetLookUpData<QueueMetaData>(View.SelectedTenantId_Global);
            foreach (var itemData in lstItems)
            {
                if (itemData.CurrentTenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue())
                {
                    _queueTypeCode = QueueMetaDataType.Verification_Queue_For_Third_Party.GetStringValue();
                }
                else if (itemData.CurrentTenantTypeCode == TenantType.Institution.GetStringValue())
                {
                    _queueTypeCode = QueueMetaDataType.Verification_Queue_For_ClientAdmin.GetStringValue();
                }
                else if (View.CurrentTenantId_Global == SecurityManager.DefaultTenantID)
                {
                    _queueTypeCode = QueueMetaDataType.Verification_Queue_For_Admin.GetStringValue();
                }

                QueueMetaData _queueMetaData = _lstQueueMetaData.Where(qmd => qmd.QMD_Code == _queueTypeCode && qmd.QMD_IsDeleted == false).FirstOrDefault();

                Int32 _queueId = _queueMetaData.QMD_QueueID;
                Dictionary<String, Object> dicQueueFields = new Dictionary<String, Object>();
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ApplicantComplianceItemID, itemData.ApplicantComplianceItemID);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.PackageID, View.SelectedCompliancePackageId_Global);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.CategoryId, View.SelectedComplianceCategoryId_Global);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ComplianceItemId, itemData.ComplianceItemID);

                foreach (KeyValuePair<String, Object> dicQueueField in dicQueueFields)
                {
                    queueFieldString.Append("<QueueDetail>");
                    queueFieldString.Append("<QueueID>" + _queueId + "</QueueID>"); // Need to change when Moved to next Queue and NOT Same level
                    queueFieldString.Append("<RecordID>" + itemData.ApplicantComplianceItemID + "</RecordID>");
                    queueFieldString.Append("<QueueFieldName>" + dicQueueField.Key + "</QueueFieldName>");
                    queueFieldString.Append("<QueueFieldValue>" + dicQueueField.Value + "</QueueFieldValue>");
                    queueFieldString.Append("<ReferenceID>" + itemData.ComplianceItemID + "</ReferenceID>");
                    queueFieldString.Append("<LifeCycleFieldValue>" + itemData.AttemptedItemStatusId + "</LifeCycleFieldValue>");
                    queueFieldString.Append("</QueueDetail>");
                }
                queueFieldString.ToString();
            }
        }

        private void GenerateXMLUserSpecializations(List<ApplicantComplianceItemData> lstItems)
        {
            String _queueTypeCode = String.Empty;
            List<QueueMetaData> _lstQueueMetaData = LookupManager.GetLookUpData<QueueMetaData>(View.SelectedTenantId_Global);

            foreach (var itemData in lstItems)
            {
                //String _statusCode = useNewStatus ? itemData.AttemptedStatusCode : itemData.CurrentStatusCode;

                _queueTypeCode = GetQueueTypeCodeByItemStatusCode(itemData.CurrentTenantTypeCode, itemData.CurrentStatusCode);
                QueueMetaData _queueMetaData = _lstQueueMetaData.Where(qmd => qmd.QMD_Code == _queueTypeCode && qmd.QMD_IsDeleted == false).FirstOrDefault();

                Int32 _queueId = _queueMetaData.QMD_QueueID;
                Dictionary<String, Object> dicQueueFields = new Dictionary<String, Object>();
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ApplicantComplianceItemID, itemData.ApplicantComplianceItemID);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.PackageID, View.SelectedCompliancePackageId_Global);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.CategoryId, View.SelectedComplianceCategoryId_Global);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ComplianceItemId, itemData.ComplianceItemID);

                foreach (KeyValuePair<String, Object> dicQueueField in dicQueueFields)
                {
                    queueFieldString.Append("<QueueDetail>");
                    queueFieldString.Append("<QueueID>" + _queueId + "</QueueID>"); // Need to change when Moved to next Queue and NOT Same level
                    queueFieldString.Append("<RecordID>" + itemData.ApplicantComplianceItemID + "</RecordID>");
                    queueFieldString.Append("<QueueFieldName>" + dicQueueField.Key + "</QueueFieldName>");
                    queueFieldString.Append("<QueueFieldValue>" + dicQueueField.Value + "</QueueFieldValue>");
                    queueFieldString.Append("<ReferenceID>" + itemData.ComplianceItemID + "</ReferenceID>");
                    queueFieldString.Append("<LifeCycleFieldValue>" + itemData.AttemptedItemStatusId + "</LifeCycleFieldValue>");

                    queueFieldString.Append("</QueueDetail>");
                }
                queueFieldString.ToString();
            }
        }
        /// <summary>
        /// Generate XML for input of 'HandleAssignments'
        /// </summary>
        /// <param name="lstItems"></param>
        /// <param name="useNewStatus"></param>
        /// <param name="skipInitialReview">True when the Incomplete item is directly moved to any other Queue rather then same queue.
        /// and Current Queue != User Queue</param>
        /// <param name="isResetReviewRequired">True when XML is sgenerated for any item moving within the same queue.</param>
        /// <param name="isNewEscalationList">Is true for the items that are to be escalated</param>
        /// <returns></returns>
        private String GenerateInputXmlHandleAssignment(List<ApplicantComplianceItemData> lstItems, Boolean useNewStatus, Boolean skipInitialReview, Boolean isResetReviewRequired
            , Boolean isNewEscalationList = false)
        {
            StringBuilder _sbXML = new StringBuilder();
            String _queueTypeCode = String.Empty;
            List<QueueMetaData> _lstQueueMetaData = LookupManager.GetLookUpData<QueueMetaData>(View.SelectedTenantId_Global);
            List<QueueFieldsMetaData> _lstQueueFieldsMetaData = LookupManager.GetLookUpData<QueueFieldsMetaData>(View.SelectedTenantId_Global);
            List<lkpItemComplianceStatu> _lstItemComplianceStatus = LookupManager.GetLookUpData<lkpItemComplianceStatu>(View.SelectedTenantId_Global);

            foreach (var itemData in lstItems)
            {
                if (isNewEscalationList)
                {
                    _queueTypeCode = GetQueueTypeCodeByStatus(itemData.CurrentStatusCode, true);
                    itemData.StatusID = _lstItemComplianceStatus.FirstOrDefault(x => x.Code.Equals(itemData.CurrentStatusCode)).ItemComplianceStatusID;
                }
                else
                {
                    if (useNewStatus)
                    {
                        _queueTypeCode = GetQueueTypeCodeByStatus(itemData.AttemptedStatusCode);
                        itemData.StatusID = _lstItemComplianceStatus.FirstOrDefault(x => x.Code.Equals(itemData.AttemptedStatusCode)).ItemComplianceStatusID;
                    }
                    else
                    {
                        _queueTypeCode = GetQueueTypeCodeByStatus(itemData.CurrentStatusCode);
                        itemData.StatusID = _lstItemComplianceStatus.FirstOrDefault(x => x.Code.Equals(itemData.CurrentStatusCode)).ItemComplianceStatusID;
                    }
                    //   _queueTypeCode = GetQueueTypeCodeByItemStatusCode(itemData.CurrentTenantTypeCode, itemData.CurrentStatusCode);
                }
                QueueMetaData _queueMetaData = _lstQueueMetaData.Where(qmd => qmd.QMD_Code == _queueTypeCode && qmd.QMD_IsDeleted == false).FirstOrDefault();
                Int32 _queueId = _queueMetaData.QMD_QueueID;

                //Added to get data related to XML of 'HandleAssignment' SP in 'Queue Framework'
                Dictionary<String, Object> dicQueueFields = new Dictionary<String, Object>();

                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ApplicantName, itemData.ApplicantName);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ComplianceItemId, itemData.ComplianceItemID);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.CategoryId, View.SelectedComplianceCategoryId_Global);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.PackageID, View.SelectedCompliancePackageId_Global);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.SubmissionDate, itemData.SubmissionDate);
                dicQueueFields.Add(_queueMetaData.QMD_LifeCycleFieldName, itemData.StatusID);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.System_Status, itemData.SystemStatusText);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Rush_Order_Status_Code, itemData.RushOrderStatusCode);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ApplicantComplianceItemID, itemData.ApplicantComplianceItemID);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.HierarchyNodeID, itemData.HierarchyNodeId);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ApplicantId, View.SelectedApplicantId_Global);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Item_Name, itemData.ComplianceItemName);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Category_Name, itemData.ComplianceCategoryName);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Package_Name, itemData.CompliancePackageName);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Verification_Status_Text, itemData.VerificationStatusText);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Rush_Order_Status_Text, itemData.RushOrderStatusText);

                //End 

                foreach (KeyValuePair<String, Object> dicQueueField in dicQueueFields)
                {
                    _sbXML.Append("<QueueDetail>");
                    _sbXML.Append("<QueueID>" + _queueId + "</QueueID>"); // Need to change when Moved to next Queue and NOT Same level
                    _sbXML.Append("<RecordID>" + itemData.ApplicantComplianceItemID + "</RecordID>");
                    _sbXML.Append("<QueueFieldName>" + dicQueueField.Key + "</QueueFieldName>");
                    _sbXML.Append("<QueueFieldValue>" + dicQueueField.Value + "</QueueFieldValue>");
                    _sbXML.Append("<Attempted_LifeCycleFieldValue>" + itemData.AttemptedItemStatusId + "</Attempted_LifeCycleFieldValue>");

                    if (skipInitialReview)
                        _sbXML.Append("<ByPassInitialReview>" + 1 + "</ByPassInitialReview>");
                    else
                        _sbXML.Append("<ByPassInitialReview>" + 0 + "</ByPassInitialReview>");

                    if (isResetReviewRequired)
                        _sbXML.Append("<ResetReviewProcess>" + 1 + "</ResetReviewProcess>");
                    else
                        _sbXML.Append("<ResetReviewProcess>" + 0 + "</ResetReviewProcess>");

                    _sbXML.Append("</QueueDetail>");
                }
            }
            return Convert.ToString(_sbXML);
        }

        /// <summary>
        /// Get the queue type code, based on the item status and current tenant type code.
        /// </summary>
        /// <param name="tenantTypeCode"></param>
        /// <param name="itemStatusCode"></param>
        /// <returns></returns>
        private String GetQueueTypeCodeByItemStatusCode(String tenantTypeCode, String itemStatusCode)
        {
            if (tenantTypeCode == TenantType.Institution.GetStringValue()) // Client Admin
            {
                if (itemStatusCode == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue()
                    || itemStatusCode == ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue()
                    || itemStatusCode == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue())
                {
                    return QueueMetaDataType.Exception_Queue_For_ClientAdmin.GetStringValue();
                }
                else
                {
                    return QueueMetaDataType.Verification_Queue_For_ClientAdmin.GetStringValue();
                }
            }
            else if (tenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue()) // Third Party
            {
                return QueueMetaDataType.Verification_Queue_For_Third_Party.GetStringValue();
            }
            else if (View.CurrentTenantId_Global == SecurityManager.DefaultTenantID) // ADB Admin
            {
                if (itemStatusCode == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue()
                   || itemStatusCode == ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue()
                   || itemStatusCode == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue())
                {
                    return QueueMetaDataType.Exception_Queue_For_Admin.GetStringValue();
                }
                else
                {
                    return QueueMetaDataType.Verification_Queue_For_Admin.GetStringValue();
                }
            }
            else
            {
                return String.Empty;
            }
        }

        private String GetQueueTypeCodeByStatus(String itemStatusCode, Boolean isEscalatedItem = false)
        {
            if (isEscalatedItem)
            {
                if (itemStatusCode == ApplicantItemComplianceStatus.Pending_Review.GetStringValue()) // Admin
                    return QueueMetaDataType.Escalated_Verification_Queue_For_Admin.GetStringValue();
                else if (itemStatusCode == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue()) // Client Admin
                    return QueueMetaDataType.Escalated_Verification_Queue_For_ClientAdmin.GetStringValue();
                else if (itemStatusCode == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue()) // Third Party
                    return QueueMetaDataType.Escalated_Verification_Queue_For_Third_Party.GetStringValue();
                else if (itemStatusCode == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue() && View.IsDefaultTenant) // Admin 
                    return QueueMetaDataType.Escalated_Exception_Queue_For_Admin.GetStringValue();
                else if (itemStatusCode == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue() && !View.IsDefaultTenant) // Client Admin
                    return QueueMetaDataType.Escalated_Exception_Queue_For_ClientAdmin.GetStringValue();
            }
            else
            {
                if (itemStatusCode == ApplicantItemComplianceStatus.Pending_Review.GetStringValue()) // Client Admin
                    return QueueMetaDataType.Verification_Queue_For_Admin.GetStringValue();
                else if (itemStatusCode == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue()) // Client Admin
                    return QueueMetaDataType.Verification_Queue_For_ClientAdmin.GetStringValue();
                else if (itemStatusCode == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue()) // Third Party
                    return QueueMetaDataType.Verification_Queue_For_Third_Party.GetStringValue();
                else if (itemStatusCode == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue() && View.IsDefaultTenant)
                    return QueueMetaDataType.Exception_Queue_For_Admin.GetStringValue();
                else if (itemStatusCode == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue() && !View.IsDefaultTenant)
                    return QueueMetaDataType.Exception_Queue_For_ClientAdmin.GetStringValue();
            }
            return String.Empty;
        }

        ///// <summary>
        ///// Gets the next Escalation queue type code, based on the current admin type and the item type
        ///// </summary>
        ///// <param name="tenantTypeCode"></param>
        ///// <param name="isExceptionItem">Is item an exception item or a normal item</param>
        ///// <returns></returns>
        //private String GetEscalatedQueueTypeCode(String tenantTypeCode, Boolean isExceptionItem)
        //{
        //    if (tenantTypeCode == TenantType.Institution.GetStringValue()) // Client Admin
        //    {
        //        if (isExceptionItem)
        //            return QueueMetaDataType.Escalated_Exception_Queue_For_ClientAdmin.GetStringValue();
        //        else
        //            return QueueMetaDataType.Escalated_Verification_Queue_For_ClientAdmin.GetStringValue();
        //    }
        //    else if (tenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue()) // Third Party
        //    {
        //        return QueueMetaDataType.Escalated_Verification_Queue_For_Third_Party.GetStringValue();
        //    }
        //    else if (View.CurrentTenantId_Global == SecurityManager.DefaultTenantID) // ADB Admin
        //    {
        //        if (isExceptionItem)
        //            return QueueMetaDataType.Escalated_Exception_Queue_For_Admin.GetStringValue();
        //        else
        //            return QueueMetaDataType.Escalated_Verification_Queue_For_Admin.GetStringValue();
        //    }
        //    else
        //    {
        //        return String.Empty;
        //    }
        //}

        //private String GetQueueTypeCodeByStatusCode(String itemStatusCode, Boolean isEscalatedItem = false)
        //{
        //    if (isEscalatedItem)
        //    {
        //        if (itemStatusCode == ApplicantItemComplianceStatus.Pending_Review.GetStringValue()) // Admin
        //            return QueueMetaDataType.Escalated_Verification_Queue_For_Admin.GetStringValue();
        //        else if (itemStatusCode == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue()) // Client Admin
        //            return QueueMetaDataType.Escalated_Verification_Queue_For_ClientAdmin.GetStringValue();
        //        else if (itemStatusCode == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue()) // Third Party
        //            return QueueMetaDataType.Escalated_Verification_Queue_For_Third_Party.GetStringValue();
        //        else if (itemStatusCode == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue() && View.IsDefaultTenant) // Admin 
        //            return QueueMetaDataType.Escalated_Exception_Queue_For_Admin.GetStringValue();
        //        else if (itemStatusCode == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue() && !View.IsDefaultTenant) // Client Admin
        //            return QueueMetaDataType.Escalated_Exception_Queue_For_ClientAdmin.GetStringValue();
        //    }
        //    else
        //    {
        //        if (itemStatusCode == ApplicantItemComplianceStatus.Pending_Review.GetStringValue()) // Client Admin
        //            return QueueMetaDataType.Verification_Queue_For_Admin.GetStringValue();
        //        else if (itemStatusCode == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue()) // Client Admin
        //            return QueueMetaDataType.Verification_Queue_For_ClientAdmin.GetStringValue();
        //        else if (itemStatusCode == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue()) // Third Party
        //            return QueueMetaDataType.Verification_Queue_For_Third_Party.GetStringValue();
        //    }
        //    return String.Empty;
        //}

        /// <summary>
        /// Check if the incomplete item is moved to different queue or same queue level.
        /// </summary>
        /// <param name="tenantTypeCode"></param>
        /// <param name="newItemStatus"></param>
        /// <returns></returns>
        private Boolean CheckIfSameQueue(String tenantTypeCode, String newItemStatusCode)
        {
            String _currentQueueCode = String.Empty;

            // Get the Current Queue type
            if (tenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue())
                _currentQueueCode = QueueMetaDataType.Verification_Queue_For_Third_Party.GetStringValue();
            else if (tenantTypeCode == TenantType.Institution.GetStringValue())
                _currentQueueCode = QueueMetaDataType.Verification_Queue_For_ClientAdmin.GetStringValue();
            else if (View.CurrentTenantId_Global == SecurityManager.DefaultTenantID)
                _currentQueueCode = QueueMetaDataType.Verification_Queue_For_Admin.GetStringValue();


            String _newQueueCode = String.Empty;

            if (newItemStatusCode != ApplicantItemComplianceStatus.Not_Approved.GetStringValue()
                && newItemStatusCode != ApplicantItemComplianceStatus.Approved.GetStringValue()
                && newItemStatusCode != ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue()
                && newItemStatusCode != ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue()
                && ((newItemStatusCode == ApplicantItemComplianceStatus.Pending_Review.GetStringValue()
                    && (View.CurrentTenantId_Global == SecurityManager.DefaultTenantID || tenantTypeCode == TenantType.Institution.GetStringValue()))
                || (newItemStatusCode == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue()
                    && (tenantTypeCode == TenantType.Institution.GetStringValue() || tenantTypeCode == TenantType.Company.GetStringValue()))
                || (newItemStatusCode == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue() && tenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue())))
                _newQueueCode = GetQueueTypeCodeByStatus(newItemStatusCode);

            if (_currentQueueCode == _newQueueCode)
                return true;
            else
                return false;
        }

        #endregion
        #region UAT-523 Get status id by status code
        public Int32 GetNewStatusId(String newStatusCode)
        {
            List<lkpItemComplianceStatu> _lstStatus = LookupManager.GetLookUpData<lkpItemComplianceStatu>(View.SelectedTenantId_Global);
            return _lstStatus.Where(sts => sts.Code == newStatusCode).FirstOrDefault().ItemComplianceStatusID;
        }

        /// <summary>
        /// Method to update the category and item data.
        /// </summary>
        /// <param name="categoryDataId"></param>
        /// <param name="expirationDate"></param>
        /// <param name="statuscode"></param>
        /// <param name="recordActionType"></param>
        /// <param name="applicantComplianceItemData"></param>
        /// <param name="lstAssignmentProperties"></param>
        /// <returns></returns>
        public Boolean UpdateCategoryLevelExceptionData(Int32 categoryDataId, DateTime? expirationDate, String statuscode, String recordActionType,
                                                        ApplicantComplianceItemData applicantComplianceItemData, List<ListItemAssignmentProperties> lstAssignmentProperties)
        {
            String catStatus = String.Empty;
            String catExceptionStatus = String.Empty;
            Int16? catExceptionStatusId = null;
            Int32 catStatusId = 0;
            Boolean _status = false;
            Boolean isComplianceRequired = true;
            PackageSubscription ps = ComplianceDataManager.GetPackageSubscriptionByID(View.SelectedTenantId_Global, View.CurrentPackageSubscriptionID_Global);
            //UAT-911: Category whose Compliance Required status is set to “No” changes from Approved to Incomplete at applicant data entry screen 
            //         when applicant applies category exception and then deletes the category exception.
            Entity.ClientEntity.ApplicantComplianceCategoryData appComplCatData = ps.ApplicantComplianceCategoryDatas.FirstOrDefault(cond => cond.ApplicantComplianceCategoryID == categoryDataId && cond.IsDeleted == false);

            //UAT-3805
            List<ApplicantComplianceAttributeData> applicantData = new List<ApplicantComplianceAttributeData>();
            List<Int32> approvedCategoryIDs = new List<Int32>();
            if (appComplCatData.IsNotNull())
            {
                approvedCategoryIDs.Add(appComplCatData.ComplianceCategoryID);
                approvedCategoryIDs = ProfileSharingManager.GetApprovedCategorIDs(View.CurrentTenantId_Global, View.CurrentPackageSubscriptionID_Global
                                                                                      , approvedCategoryIDs, lkpUseTypeEnum.COMPLIANCE.GetStringValue());
            }


            if (appComplCatData.IsNotNull() && appComplCatData.ComplianceCategory.CompliancePackageCategories.IsNotNull())
            {
                Entity.ClientEntity.CompliancePackageCategory compPackageCategory = appComplCatData.ComplianceCategory.CompliancePackageCategories.FirstOrDefault(slct => slct.CPC_PackageID == ps.CompliancePackageID && ps.IsDeleted == false);
                if (compPackageCategory.IsNotNull())
                {
                    //UAt-1209: As an application admin, I should be able to enter a date range for when a category should be compliance required/not required. 
                    DateTime? newStartDate = compPackageCategory.CPC_ComplianceRqdStartDate;
                    DateTime? newEndDate = compPackageCategory.CPC_ComplianceRqdEndDate;
                    DateTime currentDate = DateTime.Now;

                    //if ((compPackageCategory.CPC_ComplianceRequired == false
                    //            && ((newStartDate.IsNull() && newEndDate.IsNull())
                    //                || ((currentDate.Month > newStartDate.Value.Month || (currentDate.Month == newStartDate.Value.Month && currentDate.Day >= newStartDate.Value.Day))
                    //                && (currentDate.Month < newEndDate.Value.Month || (currentDate.Month == newEndDate.Value.Month && currentDate.Day <= newEndDate.Value.Day) || (currentDate.Month > newEndDate.Value.Month && newEndDate.Value.Month < newStartDate.Value.Month)))
                    //                ))
                    //       || (compPackageCategory.CPC_ComplianceRequired == true
                    //            && (newStartDate.IsNotNull() && newEndDate.IsNotNull())
                    //                && ((currentDate.Month < newStartDate.Value.Month || (currentDate.Month == newStartDate.Value.Month && currentDate.Day < newStartDate.Value.Day))
                    //                || (currentDate.Month > newEndDate.Value.Month || (currentDate.Month == newEndDate.Value.Month && currentDate.Day > newEndDate.Value.Day)))
                    //        ))
                    //{
                    //    isComplianceRequired = false;
                    //}

                    isComplianceRequired = ComplianceSetupManager.GetComplianceRqdByDateRange(compPackageCategory.CPC_ComplianceRequired, compPackageCategory.CPC_ComplianceRqdStartDate,
                      compPackageCategory.CPC_ComplianceRqdEndDate, View.SelectedTenantId_Global);
                }
            }

            Entity.OrganizationUser _organizationUser = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId);
            String _currentLoggedInUserName = _organizationUser.FirstName + " " + _organizationUser.LastName;
            String currentLoggedInUserInitials = _organizationUser.FirstName.Substring(0, 1) + (_organizationUser.MiddleName.IsNullOrEmpty() ? String.Empty : _organizationUser.MiddleName.Substring(0, 1)) + _organizationUser.LastName.Substring(0, 1);
            List<lkpItemComplianceStatu> _lstItemStatus = LookupManager.GetLookUpData<lkpItemComplianceStatu>(View.SelectedTenantId_Global);
            Int32 _statusId = _lstItemStatus.Where(sts => sts.Code == applicantComplianceItemData.AttemptedStatusCode).FirstOrDefault().ItemComplianceStatusID;

            List<lkpCategoryComplianceStatu> _lstCategoryStatus = LookupManager.GetLookUpData<lkpCategoryComplianceStatu>(View.SelectedTenantId_Global);

            List<lkpCategoryExceptionStatu> _lstCategoryExceptionStatus = LookupManager.GetLookUpData<lkpCategoryExceptionStatu>(View.SelectedTenantId_Global);

            if (statuscode == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue())
            {
                isComplianceRequired = true; //Set isComplianceRequired to true if admin approved category exception.
                catStatus = ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue();
                catStatusId = _lstCategoryStatus.Where(cnd => cnd.Code == catStatus).FirstOrDefault().CategoryComplianceStatusID;
                catExceptionStatus = lkpCategoryExceptionStatus.EXCEPTION_APPROVED.GetStringValue();
                catExceptionStatusId = _lstCategoryExceptionStatus.FirstOrDefault(cond => cond.CES_Code == catExceptionStatus).CES_ID;
            }
            else if (statuscode == ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue())
            {
                //1) UAT-911: Category whose Compliance Required status is set to “No” changes from Approved to Incomplete at applicant data entry screen 
                //         when applicant applies category exception and then deletes the category exception.
                //2) check compliance required or not, If compliance is not required for the category then category status should be approved on category exception rejected.
                if (!isComplianceRequired)
                {
                    catStatus = ApplicantCategoryComplianceStatus.Approved.GetStringValue();
                }
                else
                {
                    catStatus = ApplicantCategoryComplianceStatus.Incomplete.GetStringValue();
                }
                catStatusId = _lstCategoryStatus.Where(cnd => cnd.Code == catStatus).FirstOrDefault().CategoryComplianceStatusID;
                catExceptionStatus = lkpCategoryExceptionStatus.EXCEPTION_REJECTED.GetStringValue();
                catExceptionStatusId = _lstCategoryExceptionStatus.FirstOrDefault(cond => cond.CES_Code == catExceptionStatus).CES_ID;
            }

            ComplianceDataManager.UpdateCategoryLevelExceptionData(View.SelectedTenantId_Global, categoryDataId, expirationDate, catStatusId, catExceptionStatusId, applicantComplianceItemData.ApplicantComplianceItemID, applicantComplianceItemData.ComplianceItemID, applicantComplianceItemData.VerificationComments, _statusId, View.CurrentLoggedInUserId, _currentLoggedInUserName, null, lstAssignmentProperties, recordActionType, currentLoggedInUserInitials);

            //1) UAT-911: Category whose Compliance Required status is set to “No” changes from Approved to Incomplete at applicant data entry screen 
            //         when applicant applies category exception and then deletes the category exception.
            //2) check compliance required or not, If compliance is not required for the category then rules for category will not execute.
            if (isComplianceRequired)
            {
                EvaluatePostSubmitRules(applicantComplianceItemData.ComplianceItemID);
            }

            if (ps.ComplianceStatusID.Value != 0)
            {
                String tenantName = String.Empty;
                Entity.Tenant tenant = SecurityManager.GetTenant(View.SelectedTenantId_Global);
                if (!tenant.IsNullOrEmpty())
                    tenantName = tenant.TenantName;
                //Send Mail
                ComplianceDataManager.SendMailOnComplianceStatusChange(View.SelectedTenantId_Global, tenantName, ps.lkpPackageComplianceStatu.Name, ps.ComplianceStatusID.Value, View.CurrentPackageSubscriptionID_Global, ps.Order.DeptProgramPackage.DeptProgramMapping.DPM_ID);
            }

            //UAT-3805            
            SendItemDocNotificationToAgencyUser(approvedCategoryIDs, appComplCatData.ComplianceCategoryID, false);

            _status = true;
            return _status;
        }
        //UAT-845 Creation of admin override function (verification details).
        public Boolean UpdateCategoryOverrideData(DateTime? expirationDate, String catOverrideStatus, ApplicantItemVerificationData categoryOverrideData)
        {
            String catStatus = String.Empty;
            Int16? catOverrideStatusId = null;
            Int32 catStatusId = 0;
            Boolean _status = false;
            Int32 categoryDataId = 0;
            if (!categoryOverrideData.ApplicantCompCatId.IsNullOrEmpty())
            {
                categoryDataId = categoryOverrideData.ApplicantCompCatId.Value;
            }

            List<lkpCategoryComplianceStatu> _lstCategoryStatus = LookupManager.GetLookUpData<lkpCategoryComplianceStatu>(View.SelectedTenantId_Global);

            List<lkpCategoryExceptionStatu> _lstCategoryExceptionStatus = LookupManager.GetLookUpData<lkpCategoryExceptionStatu>(View.SelectedTenantId_Global);

            if (catOverrideStatus == lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue())
            {
                catStatus = ApplicantCategoryComplianceStatus.Approved.GetStringValue();
                catStatusId = _lstCategoryStatus.Where(cnd => cnd.Code == catStatus).FirstOrDefault().CategoryComplianceStatusID;
                catOverrideStatusId = _lstCategoryExceptionStatus.FirstOrDefault(cond => cond.CES_Code == catOverrideStatus).CES_ID;
            }
            else if (catOverrideStatus == lkpCategoryExceptionStatus.DEFAULT.GetStringValue())
            {
                catStatus = ApplicantCategoryComplianceStatus.Incomplete.GetStringValue();
                catStatusId = _lstCategoryStatus.Where(cnd => cnd.Code == catStatus).FirstOrDefault().CategoryComplianceStatusID;
                catOverrideStatusId = null;
                expirationDate = null;
            }
            //UAT-2547: added Category Override Notes
            _status = ComplianceDataManager.UpdateCategoryOverrideData(View.SelectedTenantId_Global, categoryDataId, expirationDate, catStatusId, catOverrideStatusId, View.CurrentLoggedInUserId, categoryOverrideData.ComplianceCatId, View.CurrentPackageSubscriptionID_Global, String.Empty);
            EvaluatePostSubmitRules(null);
            return _status;
        }

        public ApplicantComplianceItemData DeleteCategoryException()
        {
            //UPDATE Applicant Compliance Category data. Set [CategoryExceptionStatusID] and [ExpirationDate] to null.
            //Changes as per UAT-819 WB: Category Exception enhancements
            String catStatusApprovedCode = ApplicantCategoryComplianceStatus.Approved.GetStringValue();

            ApplicantComplianceItemData appCmpItmData = ComplianceDataManager.UpdateApplicantCmpCatData(View.SelectedTenantId_Global, View.CurrentViewContext.CurrentLoggedInUserId,
               View.SelectedComplianceCategoryId_Global, View.CurrentViewContext.CurrentPackageSubscriptionID_Global, catStatusApprovedCode);
            if (appCmpItmData.IsNotNull())
                EvaluatePostSubmitRules(appCmpItmData.ComplianceItemID);
            return appCmpItmData;
        }

        /// <summary>
        /// for executing buisness rules.
        /// </summary>
        public void EvaluatePostSubmitRules(Int32? currentItemId)
        {
            List<RuleObjectMapping> ruleObjectMappingList = new List<RuleObjectMapping>();
            RuleObjectMapping ruleObjectMappingForPackage = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Package.GetStringValue(), View.SelectedTenantId_Global).OT_ID),
                RuleObjectId = Convert.ToString(View.CurrentViewContext.SelectedCompliancePackageId_Global),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RuleObjectMapping ruleObjectMappingForCategory = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Category.GetStringValue(), View.SelectedTenantId_Global).OT_ID),
                RuleObjectId = Convert.ToString(View.CurrentViewContext.SelectedComplianceCategoryId_Global),
                RuleObjectParentId = Convert.ToString(View.CurrentViewContext.SelectedCompliancePackageId_Global)
            };

            ruleObjectMappingList.Add(ruleObjectMappingForPackage);
            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
            //Added this check for executing category level rules only.
            if (currentItemId.IsNotNull())
            {
                RuleObjectMapping ruleObjectMappingForItem = new RuleObjectMapping
                {
                    RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Item.GetStringValue(), View.SelectedTenantId_Global).OT_ID),
                    RuleObjectId = Convert.ToString(currentItemId),
                    RuleObjectParentId = Convert.ToString(View.CurrentViewContext.SelectedComplianceCategoryId_Global)
                };

                ruleObjectMappingList.Add(ruleObjectMappingForItem);
            }
            RuleManager.evaluatePostSubmitRules(ruleObjectMappingList, View.CurrentViewContext.SelectedApplicantId_Global, View.CurrentLoggedInUserId, View.SelectedTenantId_Global);
        }
        #endregion


        //UAT-718
        public void SetQueueImaging()
        {

            Dictionary<String, Object> dataDictionary = new Dictionary<String, Object>();
            dataDictionary.Add("TenantID", View.SelectedTenantId_Global);
            QueueImagingManager.AsignQueueImagingRepoInstance(dataDictionary);
        }

        #region UAT-1608
        public void ShufflingOfSeriesItemsData(List<ApplicantComplianceItemData> lstItemsToDelete, List<ApplicantComplianceItemData> lstItemData)
        {
            List<Int32> lstItemIdsToShuffle = new List<Int32>();
            if (!lstItemsToDelete.IsNullOrEmpty() || !lstItemData.IsNullOrEmpty())
            {
                lstItemsToDelete.ForEach(itmToDelete =>
                {
                    lstItemIdsToShuffle.Add(itmToDelete.ComplianceItemID);
                });


                lstItemData.ForEach(itmChng =>
                {
                    if ((itmChng.AttemptedStatusCode != itmChng.CurrentStatusCode) && (itmChng.AttemptedStatusCode == ApplicantItemComplianceStatus.Not_Approved.GetStringValue()))
                    {
                        lstItemIdsToShuffle.Add(itmChng.ComplianceItemID);
                    }
                });

                if (!lstItemIdsToShuffle.IsNullOrEmpty())
                {
                    List<ItemSery> lstItemSeriesOfCategory = new List<ItemSery>();
                    lstItemSeriesOfCategory = ComplianceDataManager.GetItemSeriesForCategory(View.SelectedTenantId_Global, View.SelectedComplianceCategoryId_Global);
                    if (!lstItemSeriesOfCategory.IsNullOrEmpty())
                    {
                        lstItemIdsToShuffle.Distinct().ForEach(itemId =>
                        {
                            ShuffleShotSeriesItemData(GetSeriesIDOfItem(lstItemSeriesOfCategory, itemId));
                        });
                    }
                }
            }
        }

        private Int32 GetSeriesIDOfItem(List<ItemSery> lstItemSeriesOfCategory, Int32 compItemId)
        {
            Int32 itemSeriesId = AppConsts.NONE;
            if (!lstItemSeriesOfCategory.IsNullOrEmpty())
            {
                foreach (var itemSeries in lstItemSeriesOfCategory)
                {
                    if (!itemSeries.ItemSeriesItems.IsNullOrEmpty())
                    {
                        Boolean isItemExist = itemSeries.ItemSeriesItems.Any(cnd => cnd.ISI_ItemID == compItemId && !cnd.ISI_IsDeleted);
                        if (isItemExist)
                        {
                            itemSeriesId = itemSeries.IS_ID;
                            break;
                        }
                    }
                }
            }
            return itemSeriesId;
        }

        private void ShuffleShotSeriesItemData(Int32 itemSeriesID)
        {
            if (itemSeriesID > AppConsts.NONE)
            {
                ShotSeriesSaveResponse saveResponse = ComplianceDataManager.SaveSeriesAttributeData(View.SelectedTenantId_Global, View.CurrentPackageSubscriptionID_Global,
                                                                          itemSeriesID, View.CurrentLoggedInUserId, null, null,
                                                                          ShotSeriesHandleCalledFrom.VerificationDetailScreen, View.SelectedApplicantId_Global);
            }
        }
        #endregion

        /// <summary>
        /// Onsite SP Updates 
        /// Update Shot Series SP so that it can accommodate situations like the OHSU Polio rule
        /// UAT - 1763
        /// </summary>
        public void EvaluateAdjustItemSeriesRules(Int32 packageSubscriptionID, Dictionary<Int32, List<Int32>> dicAdjustItems)
        {
            ComplianceDataManager.EvaluateAdjustItemSeriesRules(View.SelectedTenantId_Global, dicAdjustItems, packageSubscriptionID, View.CurrentLoggedInUserId);
        }

        #region UAT-3805
        public void SendItemDocNotificationToAgencyUser(List<Int32> approvedCategoryIDs, Int32 categoryID, Boolean callAfterApplicantDataSave)
        {
            String approvedCategoryIds = String.Empty;

            List<ItemDocNotificationRequestDataContract> lstItemDocNotificationData = new List<ItemDocNotificationRequestDataContract>();
            ItemDocNotificationRequestDataContract itemDocRequestData = new ItemDocNotificationRequestDataContract();

            PackageSubscription ps = View.PackageSubscriptionBeforeSaving;

            if (!ps.IsNullOrEmpty() && ps.PackageSubscriptionID > 0 && callAfterApplicantDataSave)
            {
                var approvedCat = ps.ApplicantComplianceCategoryDatas.FirstOrDefault(cnd => cnd.ComplianceCategoryID == View.SelectedComplianceCategoryId_Global
                                                                                      && !cnd.IsDeleted
                                                                                      &&
                                                                                      (cnd.lkpCategoryComplianceStatu.Code == ApplicantCategoryComplianceStatus.Approved.GetStringValue()
                                                                                      || cnd.lkpCategoryComplianceStatu.Code == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue()
                                                                                      )
                                                                                      );
                if (!approvedCat.IsNullOrEmpty())
                {
                    approvedCategoryIDs.Add(approvedCat.ComplianceCategoryID);
                }

            }
            approvedCategoryIds = approvedCategoryIDs.IsNullOrEmpty() ? String.Empty : String.Join(",", approvedCategoryIDs);

            itemDocRequestData.TenantID = View.SelectedTenantId_Global;
            itemDocRequestData.CategoryIds = Convert.ToString(categoryID);
            itemDocRequestData.ApplicantOrgUserID = View.SelectedApplicantId_Global;
            itemDocRequestData.ApprovedCategoryIds = approvedCategoryIds;
            itemDocRequestData.RequestTypeCode = lkpUseTypeEnum.COMPLIANCE.GetStringValue();
            itemDocRequestData.PackageSubscriptionID = View.CurrentPackageSubscriptionID_Global;
            itemDocRequestData.RPS_ID = null;
            itemDocRequestData.CurrentLoggedInUserID = View.CurrentLoggedInUserId;
            lstItemDocNotificationData.Add(itemDocRequestData);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
            Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
            dicParam.Add("CategoryData", lstItemDocNotificationData);
            ProfileSharingManager.RunParallelTaskItemDocNotificationOnCatApproval(dicParam, LoggerService, ExceptiomService);
        }
        #endregion

        #region UAT-3951:Addition of option to use preset ADB Admin rejection notes

        public void GetRejectionReasons()
        {
            List<Entity.RejectionReason> lstRejectionReasonsTemp = ComplianceSetupManager.GetRejectionReasons().ToList();

            if (!lstRejectionReasonsTemp.IsNullOrEmpty())
            {
                lstRejectionReasonsTemp.Insert(0, new RejectionReason { RR_ID = 0, RR_Name = "--SELECT--" });
            }

            View.ListRejectionReasons = lstRejectionReasonsTemp;
        }

        #endregion

    }
}





