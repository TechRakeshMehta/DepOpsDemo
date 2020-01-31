using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using INTSOF.Utils;
using Microsoft.Practices.ObjectBuilder;
//using INTSOF.SharedObjects;
using INTSOF.Contracts;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceRuleEngine;
using INTSOF.UI.Contract.ComplianceManagement;
using System.IO;
using System.Web;
using INTSOF.ServiceUtil;
using CoreWeb.IntsofLoggerModel.Interface;
using CoreWeb.IntsofExceptionModel.Interface;
using INTSOF.SharedObjects;
using INTSOF.Utils.Consts;
using INTSOF.Utils.CommonPocoClasses;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.QueueManagement;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ComplianceOperations.Views
{
    public class CompliancePackageDetailsPresenter : Presenter<ICompliancePackageDetailsView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controllerr
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public CompliancePackageDetailsPresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            //  View.CurrentViewContext.PackageSubscriptionId = ClientComplianceManagementManager.GetPackageSubscriptionId(View.CurrentUserID, View.ClientCompliancePackageID, View.TenantID);
            GetDataEntryDocStatus();
        }

        /// <summary>
        /// Get Compliance & Subscription Package details
        /// </summary>
        public String GetClientCompliancePackage()
        {
            View.ClientPackage = ComplianceSetupManager.GetClientCompliancePackageByPackageID(View.TenantID, View.ClientCompliancePackageID, true);
            View.Subscription = ComplianceDataManager.GetPackageSubscriptionByPackageID(View.ClientCompliancePackageID, View.CurrentLoggedInUserId, View.TenantID);
            View.lstExpiringItem = ComplianceDataManager.GetItemsAboutToExpire(View.TenantID, View.Subscription.PackageSubscriptionID);
            View.LstComplncRqdMapping = ComplianceSetupManager.GetComplianceRqdForPackage(View.ClientCompliancePackageID, View.TenantID);
            //UAT-2159 : Show Category Explanatory note as a mouseover on the category name on the student data entry screen. 
            View.dicCatExplanatoryNotes = ComplianceSetupManager.GetExplanatoryNotesForCategory(View.ClientCompliancePackageID, View.TenantID);
            lkpCategoryComplianceStatu lkpCategoryComplianceStatus = ComplianceDataManager.GetCategoryComplianceStatusByCode(ApplicantCategoryComplianceStatus.Incomplete.GetStringValue(), View.TenantID);
            return lkpCategoryComplianceStatus.Name;
        }

        /// <summary>
        /// Get Subscription Package details
        /// </summary>
        public String GetClientCompliancePackageBySubscription()
        {
            View.ClientPackage = ComplianceSetupManager.GetClientCompliancePackageByPackageID(View.TenantID, View.ClientCompliancePackageID, true);
            View.Subscription = ComplianceDataManager.GetPackageSubscriptionByID(View.TenantID, View.PackageSubscriptionId);
            View.lstExpiringItem = ComplianceDataManager.GetItemsAboutToExpire(View.TenantID, View.PackageSubscriptionId);
            View.LstComplncRqdMapping = ComplianceSetupManager.GetComplianceRqdForPackage(View.ClientCompliancePackageID, View.TenantID);
            //UAT-2159 : Show Category Explanatory note as a mouseover on the category name on the student data entry screen. 
            View.dicCatExplanatoryNotes = ComplianceSetupManager.GetExplanatoryNotesForCategory(View.ClientCompliancePackageID, View.TenantID);
            lkpCategoryComplianceStatu lkpCategoryComplianceStatus = ComplianceDataManager.GetCategoryComplianceStatusByCode(ApplicantCategoryComplianceStatus.Incomplete.GetStringValue(), View.TenantID);
            return lkpCategoryComplianceStatus.Name;
        }

        public void GetClientComplianceItems(List<Int32> expiringItems)
        {
            if (View.CurrentViewContext.ComplianceCategoryId > 0)
            {
                //List<ComplianceItem> _lst = ComplianceDataManager.GetAvailableDataEntryItems(View.CurrentViewContext.ClientCompliancePackageID,
                //                                                  View.CurrentViewContext.ComplianceCategoryId, View.CurrentViewContext.CurrentLoggedInUserId, View.TenantID,
                //                                                  View.CurrentViewContext.ItemId, true, expiringItems);
                //UAT-2028
                List<ComplianceItem> _lst = new List<ComplianceItem>();
                List<Int32> lstMappedItems = new List<Int32>();
                List<Int32> lstExpireAndExpiredItems = new List<Int32>();

                Tuple<List<ComplianceItem>, List<Int32>, List<Int32>> tupleData = ComplianceDataManager.GetAllItemsForDataEntry(View.CurrentViewContext.ClientCompliancePackageID,
                                                               View.CurrentViewContext.ComplianceCategoryId, View.CurrentViewContext.CurrentLoggedInUserId, View.TenantID,
                                                               View.CurrentViewContext.ItemId, true, expiringItems);
                if (!tupleData.IsNullOrEmpty())
                {
                    _lst = tupleData.Item1;
                    View.lstMappedItems = lstMappedItems = tupleData.Item2;
                    lstExpireAndExpiredItems = tupleData.Item3;
                }
                if (!lstExpireAndExpiredItems.IsNullOrEmpty() && lstExpireAndExpiredItems.Count > 0)
                {
                    View.CurrentViewContext.IsItemExpired = AppConsts.ONE;
                }

                #region Get the Editable-by
                // Maintain the list of Ids the items, which can be edited by the applicant
                List<Int32> _lstEditableItemIds = new List<int>();
                //UAT-1137:Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
                List<Int32> lstExplanNotesItemIds = new List<int>();
                List<ListCategoryEditableBies> listEditableData = null;
                if (String.IsNullOrEmpty(View.WorkQueue)) // Screen is opened by applicant
                    listEditableData = ComplianceSetupManager.GetEditableBiesByPackageId(View.PackageId, View.TenantID);
                List<ListItemEditableBies> listItemEditableData = null;
                //UAT-3806
                if (String.IsNullOrEmpty(View.WorkQueue)) // Screen is opened by applicant
                    listItemEditableData = ComplianceSetupManager.GetEditableBiesByCategoryId(View.PackageId, View.CurrentViewContext.ComplianceCategoryId, View.TenantID);
                //if (!listItemEditableData.IsNullOrEmpty())
                List<Int32> lstAttributeEditableItems = listItemEditableData.Where(cond => cond.EditableByCode == LkpEditableBy.Applicant).Select(sel=>sel.ComplianceItemId).ToList();
                    if (!listEditableData.IsNullOrEmpty())
                    {
                        _lstEditableItemIds = listEditableData.Where(editableBy =>
                                                                   editableBy.CategoryId == View.CurrentViewContext.ComplianceCategoryId && editableBy.EditableByCode == LkpEditableBy.Applicant && editableBy.ItemDataEntry == true).Select(cnd => cnd.ComplianceItemId).ToList();
                        //UAT-1137:Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
                        lstExplanNotesItemIds = listEditableData.Where(editableBy =>
                                                                   editableBy.CategoryId == View.CurrentViewContext.ComplianceCategoryId
                                                                   && editableBy.ItemDataEntry == false).DistinctBy(x => x.ComplianceItemId)
                                                                   .Select(cnd => cnd.ComplianceItemId).ToList();
                    }
                #endregion

                // Add items only which can be edited by the applicant
                //  if (_lstEditableItemIds.IsNotNull() && _lstEditableItemIds.Count() > 0)
                List<ComplianceItem> compItems = new List<ComplianceItem>();

                //Added a check of "lstExpireAndExpiredItems" UAT-2028
                //UAT-2418

                //UAT-3806 :- Add Items for attribute level Editable By functionality is true.
                var cmpItems = _lst.Where(item => _lstEditableItemIds.Contains(item.ComplianceItemID)  || lstAttributeEditableItems.Contains(item.ComplianceItemID)  || (lstExpireAndExpiredItems.Contains(item.ComplianceItemID) && _lstEditableItemIds.Contains(item.ComplianceItemID))).ToList();
                //var cmpItems = _lst.Where(item => _lstEditableItemIds.Contains(item.ComplianceItemID)).ToList();
                //Added a check of "lstMappedItems" UAT-2028
                var cmpNotAllowedItems = _lst.Where(item => lstExplanNotesItemIds.Contains(item.ComplianceItemID) && !lstMappedItems.Contains(item.ComplianceItemID)).ToList();
                #region UAT-1607: Student Data Entry Screen changes
                //View.lstAvailableItems = GetOrderedComplianceItem(cmpItems);
                //View.lstNotAllowedDataEntryItems = GetOrderedComplianceItem(cmpNotAllowedItems);

                //Remove items those are already included in item series
                View.lstItemSeries = ComplianceDataManager.GetItemSeriesForCategory(View.TenantID, View.CurrentViewContext.ComplianceCategoryId);
                View.lstAvailableItems = RemoveSeriesItemsFromAvailableItems(cmpItems);
                View.lstNotAllowedDataEntryItems = RemoveSeriesItemsFromAvailableItems(cmpNotAllowedItems);

                View.lstAvailableItems = SetItemSeriesInAvailableItems(View.lstAvailableItems, View.lstItemSeries);
                #endregion
                //else
                //    View.lstAvailableItems = _lst;

                //UAT-1137:Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
                List<ComplianceItem> compItemsForExplanNotes = new List<ComplianceItem>();
                //Added a check of "lstMappedItems" UAT-2028
                compItemsForExplanNotes = _lst.Where(item => lstExplanNotesItemIds.Contains(item.ComplianceItemID) && !lstMappedItems.Contains(item.ComplianceItemID)).ToList();

                //UAT-1607:
                //Remove items those are already included in item series
                compItemsForExplanNotes = RemoveSeriesItemsFromAvailableItems(compItemsForExplanNotes);

                View.lstExplanatoryNotes = ComplianceSetupManager.GetExplanatoryNotesForItems(compItemsForExplanNotes,
                                                          LCObjectType.ComplianceItem.GetStringValue(), LCContentType.ExplanatoryNotes.GetStringValue(), View.TenantID);
            }
        }

        /// <summary>
        /// Get ComplianceCategoryId for the currently seelcted ComplianceItem
        /// </summary>
        public void GetComplianceItem()
        {
            // View.CurrentViewContext.ComplianceCategoryId = Convert.ToInt32(ClientComplianceManagementManager.GetClientComplianceItem(View.CurrentViewContext.ItemId, View.TenantID).ClientComplianceCategoryID);
        }

        /// <summary>
        /// Save/Updfate dynamic applicant form data
        /// </summary>
        public void SaveApplicantComplianceAttributeData(Boolean isResetBusinessProcess)
        {
            List<ApplicantComplianceAttributeData> applicantData = new List<ApplicantComplianceAttributeData>();

            //UAT-3805
            List<Int32> approvedCategoryIDs = new List<Int32>();
            if (View.CategoryDataContract.ApplicantComplianceCategoryId > 0)
            {
                approvedCategoryIDs.Add(View.CategoryDataContract.ComplianceCategoryId);
                approvedCategoryIDs = ProfileSharingManager.GetApprovedCategorIDs(View.TenantID, View.CategoryDataContract.PackageSubscriptionId
                                                                                      , approvedCategoryIDs, lkpUseTypeEnum.COMPLIANCE.GetStringValue());
            }

            ApplicantComplianceCategoryData categoryData = new ApplicantComplianceCategoryData
            {
                ApplicantComplianceCategoryID = View.CategoryDataContract.ApplicantComplianceCategoryId > 0 ? View.CategoryDataContract.ApplicantComplianceCategoryId : 0,
                PackageSubscriptionID = View.CategoryDataContract.PackageSubscriptionId,
                ComplianceCategoryID = View.CategoryDataContract.ComplianceCategoryId,
                Notes = View.CategoryDataContract.Notes
            };

            ApplicantComplianceItemData itemData = new ApplicantComplianceItemData
            {
                ApplicantComplianceItemID = View.ItemDataContract.ApplicantComplianceItemId > 0 ? View.ItemDataContract.ApplicantComplianceItemId : 0,
                ComplianceItemID = View.ItemDataContract.ComplianceItemId,
                Notes = View.ItemDataContract.Notes,
                IsUiRulesViolate = false
            };

            List<ListItemAssignmentProperties> lstItemAssignmentProperties = ComplianceSetupManager.GetAssignmentPropertiesByCategoryId(View.PackageId, View.CategoryDataContract.ComplianceCategoryId, View.TenantID);
            itemData = ComplianceDataManager.SetItemReviewerTypeProperties(itemData, lstItemAssignmentProperties);

            foreach (var attData in View.lstAttributesData)
            {
                var applicantComplianceAttributeData = new ApplicantComplianceAttributeData();
                applicantComplianceAttributeData.ApplicantComplianceAttributeID = attData.ApplicantComplianceAttributeId > 0 ? attData.ApplicantComplianceAttributeId : 0;
                applicantComplianceAttributeData.ApplicantComplianceItemID = attData.ComplianceItemAttributeId;
                applicantComplianceAttributeData.AttributeValue = attData.AttributeValue;
                applicantComplianceAttributeData.ComplianceAttributeID = attData.ComplianceItemAttributeId;
                applicantComplianceAttributeData.AttributeTypeCode = attData.AttributeTypeCode;

                if (!attData.Signature.IsNullOrEmpty())
                {
                    ComplianceAttributeDataLargeContent largeContent = new ComplianceAttributeDataLargeContent();
                    largeContent.CADLC_Signature = attData.Signature;
                    applicantComplianceAttributeData.ComplianceAttributeDataLargeContents.Add(largeContent);
                }

                applicantData.Add(applicantComplianceAttributeData);
            }
            Boolean notReviewed = false;
            if (itemData.ApplicantComplianceItemID > 0)
            {
                notReviewed = true;
            }

            //Changes related to UAT-1711.
            //UAT-1597
            //String oldItemStatus = GetOldItemStatus(itemData.ApplicantComplianceItemID);
            String oldItemStatus = String.Empty;
            Int32? oldReconciliationReviewCount = null;
            ApplicantComplianceItemData appCompItemData = ComplianceDataManager.GetApplicantComplianceItemDataByID(View.TenantID, itemData.ApplicantComplianceItemID);

            if (!appCompItemData.IsNullOrEmpty())
            {
                oldItemStatus = appCompItemData.lkpItemComplianceStatu.IsNullOrEmpty() ? String.Empty : appCompItemData.lkpItemComplianceStatu.Code;
                oldReconciliationReviewCount = appCompItemData.ReconciliationReviewCount;
            }

            ComplianceSaveResponse complianceSaveResponse = ComplianceDataManager.SaveApplicantData(categoryData, itemData, applicantData, View.CurrentLoggedInUserId, View.AttributeDocuments, View.CategoryDataContract.ReviewStatusTypeCode, View.ItemDataContract.ReviewStatusTypeCode, View.CurrentViewContext.PackageId, View.CurrentViewContext.IsUIValidationApplicable, View.PackageSubscriptionId, View.TenantID, View.ViewAttributeDocuments, View.OrgUsrID);
            View.CurrentViewContext.UIValidationErrors = complianceSaveResponse.UIValidationErrors;
            evaluatePostSubmitRules(View.ItemDataContract.ComplianceItemId);

            // Changes related to short series 'usp_Rule_EvaluateAdjustItemSeriesRules' SP.
            List<Int32> itemIds = new List<Int32>();
            itemIds.Add(View.ItemDataContract.ComplianceItemId);
            View.DicCategoryDataForItemSeriesRule = new Dictionary<Int32, List<Int32>>();
            View.DicCategoryDataForItemSeriesRule.Add(View.CategoryDataContract.ComplianceCategoryId, itemIds);

            //Send Mail for item status change UAT-1597
            SendItemStatusChangeNotification(View.ItemDataContract.ComplianceItemId, oldItemStatus);
            //Check that applicant data saved or not.
            if (complianceSaveResponse.SaveStatus)
            {
                //Call method that enter the entry in System Communication Delivery table 
                //to send the notification to applicant on first item submission.
                SaveSystComDeliverySettingForFirstItemSubmit(complianceSaveResponse.ItemData, false);
                SetQueueImaging();
            }
            //Call Parallel task handle assignment method
            String statusCode = complianceSaveResponse.StatusCode;
            Int32? statusId = complianceSaveResponse.StatusId;
            if (!statusCode.IsNullOrEmpty() && statusId != null && statusCode != ApplicantItemComplianceStatus.Approved.GetStringValue())
            {

                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

                if (oldReconciliationReviewCount != null && itemData.ApplicantComplianceItemID > AppConsts.NONE)
                {
                    QueueManagementManager.ResetReconciliationProcess(View.CurrentLoggedInUserId, itemData.ApplicantComplianceItemID, false, View.TenantID);
                }
                if (isResetBusinessProcess)
                {
                    String code = QueueMetaDataType.Verification_Queue_For_Admin.GetStringValue();
                    QueueMetaData queueMetaData = QueueManagementManager.GetQueueMetaDataByCode(View.TenantID, code);
                    Int32 businessProcessID = (queueMetaData.IsNotNull() && queueMetaData.QMD_BusinessProcessID.HasValue) ? queueMetaData.QMD_BusinessProcessID.Value : 0;
                    QueueManagementManager.ResetBusinessProcess(View.TenantID, businessProcessID, itemData.ApplicantComplianceItemID);
                }

                if (complianceSaveResponse.ItemData.ReconciliationReviewCount.IsNull())
                {
                    Dictionary<String, Object> dicHandleAssignmentData = SetHandleAssignmentData(false, statusCode, statusId, itemData.ApplicantComplianceItemID, itemData.ComplianceItemID, complianceSaveResponse.ItemData, notReviewed);
                    QueueManagementManager.RunParallelTaskHandleAssignment(dicHandleAssignmentData, LoggerService, ExceptiomService, View.TenantID);
                }
                else
                {
                    Dictionary<String, Object> dicHandleAssignmentData = SetHandleReconciliationAssignmentData(false, statusCode, statusId, itemData.ApplicantComplianceItemID, itemData.ComplianceItemID, complianceSaveResponse.ItemData, notReviewed);
                    QueueManagementManager.RunParallelTaskHandleReconciliationAssignment(dicHandleAssignmentData, LoggerService, ExceptiomService, View.TenantID);
                }
            }

            //UAT-2618:
            ComplianceDataManager.UpdateIsDocAssociated(View.TenantID, View.PackageSubscriptionId, true, View.CurrentLoggedInUserId);

            //UAT-3112:-
            if (!complianceSaveResponse.IsNullOrEmpty() && !complianceSaveResponse.ItemData.IsNullOrEmpty())
            {
                string oldReviewStatusCode = string.Empty;
                string newStatusCode = string.Empty;

                if (!complianceSaveResponse.ItemData.lkpItemComplianceStatu.IsNullOrEmpty())
                {
                    var result = ComplianceDataManager.GetApplicantComplianceItemDataByID(View.TenantID, complianceSaveResponse.ItemData.ApplicantComplianceItemID);
                    newStatusCode = result.lkpItemComplianceStatu.Code;
                }

                if (!View.ItemDataContract.IsNullOrEmpty())
                    oldReviewStatusCode = Convert.ToString(View.ItemDataContract.ReviewStatusTypeCode); ;

                if (newStatusCode == ApplicantItemComplianceStatus.Approved.GetStringValue()
                        && oldReviewStatusCode != newStatusCode
                        )
                {
                    string acid = Convert.ToString(complianceSaveResponse.ItemData.ApplicantComplianceItemID);
                    ComplianceDataManager.SaveBadgeFormNotificationData(View.TenantID, acid, null, null, View.CurrentLoggedInUserId);
                }
            }
            //UAT-3805
            SendItemDocNotificationToAgencyUser(approvedCategoryIDs);
        }

        /// <summary>
        /// Gets the name and the explanatory notes of the category, for which applicant wants to submit data of an item.
        /// </summary>
        public void GetComplianceCategoryDetails(Int32 complianceCategoryId = AppConsts.NONE)
        {
            //View.CurrentViewContext.SelectedCategory = ComplianceDataManager.GetComplianceCategoryDetails(View.CurrentViewContext.ComplianceCategoryId, View.CurrentViewContext.TenantID);
            Int32 categoryId = View.CurrentViewContext.ComplianceCategoryId;
            if (complianceCategoryId > AppConsts.NONE)
                categoryId = complianceCategoryId;
            View.CurrentViewContext.SelectedCategory = ComplianceDataManager.GetComplianceCategoryDetails(categoryId, View.CurrentViewContext.TenantID);
        }


        public List<ApplicantDocument> GetApplicantUploadedDocuments()
        {
            List<ApplicantDocument> applicantAllDocuments = ComplianceDataManager.GetApplicantDocuments(View.CurrentLoggedInUserId, View.TenantID).ToList();
            return ComplianceDataManager.GetApplicantDocumentsExceptEsigned(applicantAllDocuments, View.TenantID);
        }

        public void SaveExceptionData(int applicantComplianceItemId, string exceptionReason, int complianceItemId, List<ExceptionDocumentMapping> exceptionMapping, Boolean isCategory)
        {
            var applicantComplianceCategoryId = View.CategoryDataContract.ApplicantComplianceCategoryId > 0
                ? View.CategoryDataContract.ApplicantComplianceCategoryId
                : 0;
            //inserting Into Compliance Category Data
            var categoryData = new ApplicantComplianceCategoryData
            {
                ApplicantComplianceCategoryID = applicantComplianceCategoryId,
                PackageSubscriptionID = View.CategoryDataContract.PackageSubscriptionId,
                ComplianceCategoryID = View.CategoryDataContract.ComplianceCategoryId
            };

            //inserting Into Applicance Compliance Item Data
            var applicantComplianceItemData = new ApplicantComplianceItemData
            {
                ApplicantComplianceItemID = applicantComplianceItemId,
                ApplicantComplianceCategoryID = View.ComplianceCategoryId,
                ComplianceItemID = complianceItemId,
                CreatedByID = View.OrgUsrID, //View.CurrentLoggedInUserId, UAT 1261
                ExceptionReason = exceptionReason
            };

            List<ListItemAssignmentProperties> lstItemAssignmentProperties = ComplianceSetupManager.GetAssignmentPropertiesByCategoryId(View.PackageId, View.CategoryDataContract.ComplianceCategoryId, View.TenantID);
            applicantComplianceItemData = ComplianceDataManager.SetItemReviewerTypeProperties(applicantComplianceItemData, lstItemAssignmentProperties, true);
            var itemCompliancestatus = ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue();
            String categoryStatusCode = ApplicantCategoryComplianceStatus.Incomplete.GetStringValue();
            String categoryExceptionStatusCode = ApplicantCategoryExceptionStatus.EXCEPTION_APPLIED.GetStringValue();
            var currentItemAssignmentProperties = lstItemAssignmentProperties.Where(obj => obj.ComplianceItemId == applicantComplianceItemData.ComplianceItemID).ToList();
            if (currentItemAssignmentProperties.IsNotNull() && currentItemAssignmentProperties.Any(x => x.ApprovalRequired == false))
            {
                //Commented as UAT-3330
                //itemCompliancestatus = ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue();
                //categoryStatusCode = ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue();
                //categoryExceptionStatusCode = ApplicantCategoryExceptionStatus.EXCEPTION_APPROVED.GetStringValue();
            }
            var reviewerTypeCode = LkpReviewerType.Admin;

            //UAt-1209: As an application admin, I should be able to enter a date range for when a category should be compliance required/not required. 
            categoryStatusCode = CheckIfComplianceRequiredIsFalse(categoryStatusCode);
            List<Int32> applicationDocumentIds = new List<Int32>();
            if (!View.SavedApplicantDocuments.IsNull())
                applicationDocumentIds = View.SavedApplicantDocuments.Keys.ToList();

            //Changes as per UAT-523 WB: Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
            ComplianceSaveResponse complianceSaveResponse = ComplianceDataManager.SaveExceptionData(categoryData, applicantComplianceItemData, itemCompliancestatus, reviewerTypeCode,
                categoryStatusCode, View.TenantID, exceptionMapping, applicationDocumentIds, isCategory, categoryExceptionStatusCode, View.CurrentLoggedInUserId);
            evaluatePostSubmitRules(complianceItemId);

            // Changes related to short series 'usp_Rule_EvaluateAdjustItemSeriesRules' SP.
            List<Int32> itemIds = new List<Int32>();
            itemIds.Add(complianceItemId);
            if (!View.DicCategoryDataForItemSeriesRule.IsNullOrEmpty())
            {
                itemIds.AddRange(View.DicCategoryDataForItemSeriesRule.GetValue(View.CategoryDataContract.ComplianceCategoryId));
            }
            if (complianceItemId > AppConsts.NONE)
            {
                View.DicCategoryDataForItemSeriesRule = new Dictionary<Int32, List<Int32>>();
                View.DicCategoryDataForItemSeriesRule.Add(View.CategoryDataContract.ComplianceCategoryId, itemIds.Distinct().ToList());
            }

            //Call Parallel task handle assignment method
            Dictionary<String, Object> dicHandleAssignmentData = SetHandleAssignmentData(false, itemCompliancestatus, null, complianceSaveResponse.ItemData.ApplicantComplianceItemID, applicantComplianceItemData.ComplianceItemID, complianceSaveResponse.ItemData, false);
            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
            QueueManagementManager.RunParallelTaskHandleAssignment(dicHandleAssignmentData, LoggerService, ExceptiomService, View.TenantID);
            SetQueueImaging();
            //UAT-2618:
            ComplianceDataManager.UpdateIsDocAssociated(View.TenantID, View.PackageSubscriptionId, true, View.CurrentLoggedInUserId);
        }

        public void UpdateExceptionData(int applicantComplianceItemId, string exceptionReason, List<ExceptionDocumentMapping> exceptionMapping, Int32 complianceItemId)
        {
            //inserting Into Applicance Compliance Item Data
            var applicantComplianceItemData = new ApplicantComplianceItemData
            {
                ApplicantComplianceItemID = applicantComplianceItemId,
                ApplicantComplianceCategoryID = View.ComplianceCategoryId,
                CreatedByID = View.OrgUsrID, //View.CurrentLoggedInUserId, UAT 1261
                ComplianceItemID = complianceItemId,
                ExceptionReason = exceptionReason
            };
            List<ListItemAssignmentProperties> lstItemAssignmentProperties = ComplianceSetupManager.GetAssignmentPropertiesByCategoryId(View.PackageId, View.CategoryDataContract.ComplianceCategoryId, View.TenantID);
            applicantComplianceItemData = ComplianceDataManager.SetItemReviewerTypeProperties(applicantComplianceItemData, lstItemAssignmentProperties);
            List<Int32> applicationDocumentIds = new List<Int32>();
            if (!View.SavedApplicantDocuments.IsNull())
                applicationDocumentIds = View.SavedApplicantDocuments.Keys.ToList();

            string reviewerTypeCode = LkpReviewerType.Admin;
            string itemCompliancestatus = ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue();
            var currentItemAssignmentProperties = lstItemAssignmentProperties.Where(obj => obj.ComplianceItemId == applicantComplianceItemData.ComplianceItemID).ToList();
            String categoryStatusCode = ApplicantCategoryComplianceStatus.Incomplete.GetStringValue();
            String categoryExceptionStatusCode = ApplicantCategoryExceptionStatus.EXCEPTION_APPLIED.GetStringValue();

            if (currentItemAssignmentProperties.IsNotNull() && currentItemAssignmentProperties.Any(x => x.ApprovalRequired == false))
            {
                //UAT-3330
                //itemCompliancestatus = ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue();
                //categoryStatusCode = ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue();
                //categoryExceptionStatusCode = ApplicantCategoryExceptionStatus.EXCEPTION_APPROVED.GetStringValue();
            }

            //UAt-1209: As an application admin, I should be able to enter a date range for when a category should be compliance required/not required. 
            categoryStatusCode = CheckIfComplianceRequiredIsFalse(categoryStatusCode);
            ComplianceSaveResponse complianceSaveResponse = ComplianceDataManager.UpdateExceptionData(View.TenantID, applicantComplianceItemData, exceptionMapping, applicationDocumentIds, reviewerTypeCode, itemCompliancestatus
                                                                                                            , categoryStatusCode, categoryExceptionStatusCode);
            evaluatePostSubmitRules(View.CurrentViewContext.ItemId);

            // Changes related to short series 'usp_Rule_EvaluateAdjustItemSeriesRules' SP.
            List<Int32> itemIds = new List<Int32>();
            itemIds.Add(View.CurrentViewContext.ItemId);
            View.DicCategoryDataForItemSeriesRule = new Dictionary<Int32, List<Int32>>();
            if (View.CurrentViewContext.ItemId > AppConsts.NONE)
            {
                View.DicCategoryDataForItemSeriesRule.Add(View.CategoryDataContract.ComplianceCategoryId, itemIds);
            }
            //Call Parallel task handle assignment method
            complianceSaveResponse.ItemData = ComplianceDataManager.SetItemReviewerTypeProperties(complianceSaveResponse.ItemData, lstItemAssignmentProperties, true); //Commented as per UAT-3330
            Dictionary<String, Object> dicHandleAssignmentData = SetHandleAssignmentData(false, itemCompliancestatus, null, applicantComplianceItemId, View.CurrentViewContext.ItemId, complianceSaveResponse.ItemData, true);
            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
            QueueManagementManager.RunParallelTaskHandleAssignment(dicHandleAssignmentData, LoggerService, ExceptiomService, View.TenantID);
            SetQueueImaging();
            //UAT-2618:
            ComplianceDataManager.UpdateIsDocAssociated(View.TenantID, View.PackageSubscriptionId, true, View.CurrentLoggedInUserId);
        }

        //CHANGED
        /*public void RemoveExceptionByApplicantComplianceItem(int applicantComplianceItem)*/
        public void RemoveExceptionByApplicantComplianceItem(int applicantComplianceItem, Int32 complianceItemId, Int32 complianceCategoryId)
        {
            //Changes as per UAT-819 WB: Category Exception enhancements
            Guid wholeCatGUID = new Guid(AppConsts.WHOLE_CATEGORY_GUID);
            ApplicantComplianceItemData itemData = ComplianceDataManager.GetApplicantComplianceItemData(applicantComplianceItem, View.TenantID);
            if (itemData.ComplianceItem.Code == wholeCatGUID)
            {
                // CHANGED
                // DeleteCategoryException();
                DeleteCategoryException(complianceCategoryId);
            }
            else
            {
                ComplianceSaveResponse complianceSaveResponse = ComplianceDataManager.RemoveExceptionData(applicantComplianceItem, View.CurrentViewContext.CurrentLoggedInUserId, View.TenantID);
                //CHANGED
                //evaluatePostSubmitRules(View.CurrentViewContext.ItemId);
                evaluatePostSubmitRules(complianceItemId, complianceCategoryId);

                if (complianceSaveResponse.IsNotNull())
                {
                    ClearQueueRecords(applicantComplianceItem, complianceSaveResponse.StatusCode);
                }

                // Changes related to short series 'usp_Rule_EvaluateAdjustItemSeriesRules' SP.
                List<Int32> itemIds = new List<Int32>();
                itemIds.Add(View.CurrentViewContext.ItemId);
                View.DicCategoryDataForItemSeriesRule = new Dictionary<Int32, List<Int32>>();
                if (complianceItemId > AppConsts.NONE)
                {
                    View.DicCategoryDataForItemSeriesRule.Add(complianceCategoryId, itemIds);
                }
                EvaluateAdjustItemSeriesRules();
            }

            SetQueueImaging();
            //UAT-2618:
            ComplianceDataManager.UpdateIsDocAssociated(View.TenantID, View.PackageSubscriptionId, true, View.CurrentLoggedInUserId);
        }

        // CHanged
        //public Boolean DeleteApplicantItemAttributeData(Int32 applicantComplianceItemId)
        public Boolean DeleteApplicantItemAttributeData(Int32 applicantComplianceItemId, Int32 complianceCategoryId, Int32 complianceItemId)
        {
            Guid wholeCatGUID = new Guid(AppConsts.WHOLE_CATEGORY_GUID);
            ApplicantComplianceItemData itemData = ComplianceDataManager.GetApplicantComplianceItemData(applicantComplianceItemId, View.TenantID);
            if (itemData.ComplianceItem.Code == wholeCatGUID)
            {
                // CHANGED
                //DeleteCategoryException();
                DeleteCategoryException(complianceCategoryId);
                //UAT-2618:
                ComplianceDataManager.UpdateIsDocAssociated(View.TenantID, View.PackageSubscriptionId, true, View.CurrentLoggedInUserId);
                return true;
            }
            else
            {
                //UAT-2490
                String DeletedReasonCode = ApplicantComplianceItemDataDeletedfrom.STUDENT_DATA_ENTRY_SCREEN.GetStringValue();
                ComplianceSaveResponse complianceSaveResponse = ComplianceDataManager.DeleteApplicantItemAttributeData(applicantComplianceItemId, View.CurrentLoggedInUserId, View.CurrentViewContext.TenantID, DeletedReasonCode, View.OrganiztionUserID);
                if (complianceSaveResponse.IsNotNull() && complianceSaveResponse.SaveStatus)
                {
                    // CHANGED
                    //evaluatePostSubmitRules(View.CurrentViewContext.ItemId);
                    evaluatePostSubmitRules(complianceItemId, complianceCategoryId);

                    if (complianceSaveResponse.ItemData.IsNotNull()
                        && complianceSaveResponse.ItemData.ReconciliationReviewCount.IsNull())
                        ClearQueueRecords(applicantComplianceItemId, complianceSaveResponse.StatusCode);
                    else
                        QueueManagementManager.ResetReconciliationProcess(View.CurrentLoggedInUserId, itemData.ApplicantComplianceItemID, false, View.TenantID);

                    // Changes related to short series 'usp_Rule_EvaluateAdjustItemSeriesRules' SP.
                    List<Int32> itemIds = new List<Int32>();
                    itemIds.Add(complianceItemId);
                    View.DicCategoryDataForItemSeriesRule = new Dictionary<Int32, List<Int32>>();
                    if (complianceItemId > AppConsts.NONE)
                    {
                        View.DicCategoryDataForItemSeriesRule.Add(complianceCategoryId, itemIds);
                    }
                    SetQueueImaging();
                    //UAT-2618:
                    ComplianceDataManager.UpdateIsDocAssociated(View.TenantID, View.PackageSubscriptionId, true, View.CurrentLoggedInUserId);
                    return true;
                }
                return false;
            }
        }

        public ApplicantComplianceItemData GetApplicantComplianceItemData(int applicantComplianceItemId)
        {
            return ComplianceDataManager.GetApplicantComplianceItemData(applicantComplianceItemId, View.TenantID);
        }
        //UAT-3806
        public List<ListItemEditableBies> GetEditableBiesByCategoryId()
        {
            return ComplianceSetupManager.GetEditableBiesByCategoryId(View.PackageId, View.ComplianceCategoryId, View.TenantID);
        }
        public Int32 GetComplianceFileUploadDataTypeID()
        {
            return LookupManager.GetLookUpData<Entity.ClientEntity.lkpComplianceAttributeDatatype>(View.TenantID).Where(x => x.Code == ComplianceAttributeDatatypes.FileUpload.GetStringValue()).Select(select => select.ComplianceAttributeDatatypeID).FirstOrDefault();
        }
        public Int32 GetComplianceAttributeManualTypeID()
        {
            return LookupManager.GetLookUpData<Entity.ClientEntity.lkpComplianceAttributeType>(View.TenantID).Where(x => x.Code == ComplianceAttributeType.Manual.GetStringValue()).Select(x => x.ComplianceAttributeTypeID).FirstOrDefault();
        }
        //END UAT
        public void AddApplicantUploadedDocuments(String filePath)
        {
            if (View.SavedApplicantDocuments.IsNull())
            {
                View.SavedApplicantDocuments = new Dictionary<Int32, String>();
            }
            foreach (var applicantDocument in View.ToSaveApplicantUploadedDocuments)
            {
                var Id = ComplianceDataManager.SaveApplicantDocument(applicantDocument, View.TenantID);
                if (!View.SavedApplicantDocuments.ContainsKey(Id))
                {
                    View.SavedApplicantDocuments.Add(Id, applicantDocument.FileName);
                }
                String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                String newFileName = filePath + "UD_" + View.TenantID.ToString() + "_" + Id.ToString() + "_" + date + Path.GetExtension(applicantDocument.FileName);
                //String newFileName = applicantDocument.DocumentPath.Replace("xxx",Id.ToString());
                ComplianceDataManager.UpdateDocumentPath(newFileName, applicantDocument.DocumentPath, Id, View.TenantID, applicantDocument.OrganizationUserID.Value);
            }
        }

        public Int32 AddApplicantViewDocuments(String filePath)
        {
            Int32 id = 0;
            if (View.SavedApplicantDocuments.IsNull())
            {
                View.SavedApplicantDocuments = new Dictionary<Int32, String>();
            }
            foreach (var applicantDocument in View.ToSaveApplicantUploadedDocuments)
            {
                id = ComplianceDataManager.SaveApplicantDocument(applicantDocument, View.TenantID);
                if (!View.SavedApplicantDocuments.ContainsKey(id))
                {
                    View.SavedApplicantDocuments.Add(id, applicantDocument.FileName);
                }
                String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                String newFileName = filePath + "UD_" + View.TenantID.ToString() + "_" + id.ToString() + "_" + date + Path.GetExtension(applicantDocument.FileName);
                //String newFileName = applicantDocument.DocumentPath.Replace("xxx",Id.ToString());
                ComplianceDataManager.UpdateDocumentPath(newFileName, applicantDocument.DocumentPath, id, View.TenantID, applicantDocument.OrganizationUserID.Value);
            }
            View.AddedViewDocId = id;
            return id;
        }

        public void evaluatePostSubmitRules(Int32 complianceItemId, Int32 complianceCategoryId = AppConsts.NONE)
        {
            // Use category id as provided by the Deletion of Exception or Item methods
            var categoryId = complianceCategoryId != AppConsts.NONE ? complianceCategoryId : View.ComplianceCategoryId;

            List<RuleObjectMapping> ruleObjectMappingList = new List<RuleObjectMapping>();
            RuleObjectMapping ruleObjectMappingForPackage = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Package.GetStringValue(), View.TenantID).OT_ID),
                RuleObjectId = Convert.ToString(View.ClientCompliancePackageID),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RuleObjectMapping ruleObjectMappingForCategory = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Category.GetStringValue(), View.TenantID).OT_ID),
                //RuleObjectId = Convert.ToString(View.ComplianceCategoryId),
                RuleObjectId = Convert.ToString(categoryId),
                RuleObjectParentId = Convert.ToString(View.ClientCompliancePackageID)
            };

            RuleObjectMapping ruleObjectMappingForItem = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Item.GetStringValue(), View.TenantID).OT_ID),
                RuleObjectId = Convert.ToString(complianceItemId),
                //RuleObjectParentId = Convert.ToString(View.ComplianceCategoryId)
                RuleObjectParentId = Convert.ToString(categoryId)
            };


            ruleObjectMappingList.Add(ruleObjectMappingForPackage);
            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
            ruleObjectMappingList.Add(ruleObjectMappingForItem);
            if (View.lstAttributesData != null)
            {
                if (View.lstAttributesData.Count > 0)
                {
                    foreach (ApplicantComplianceAttributeDataContract attributeData in View.lstAttributesData)
                    {
                        RuleObjectMapping ruleObjectMappingForAttribute = new RuleObjectMapping
                        {
                            RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_ATR.GetStringValue(), View.TenantID).OT_ID),
                            RuleObjectId = Convert.ToString(attributeData.ComplianceItemAttributeId),
                            RuleObjectParentId = Convert.ToString(complianceItemId)
                        };
                        ruleObjectMappingList.Add(ruleObjectMappingForAttribute);
                    }
                }
            }
            //RuleManager.evaluatePostSubmitRules(ruleObjectMappingList, View.CurrentLoggedInUserId, View.CurrentLoggedInUserId, View.TenantID);
            RuleManager.evaluatePostSubmitRules(ruleObjectMappingList, View.CurrentLoggedInUserId, View.OrgUsrID, View.TenantID);

            if (View.ComplianceStatusID != 0)
            {
                String tenantName = String.Empty;
                Entity.Tenant tenant = SecurityManager.GetTenant(View.TenantID);
                if (!tenant.IsNullOrEmpty())
                    tenantName = tenant.TenantName;
                //Send Mail
                //ComplianceDataManager.SendMailOnComplianceStatusChange(View.TenantID, tenantName, View.ComplianceStatus, View.ComplianceStatusID, View.PackageSubscriptionId, View.HierarchyID.HasValue ? View.HierarchyID.Value : 0);
                //UAT - 1067 : Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on. 
                ComplianceDataManager.SendMailOnComplianceStatusChange(View.TenantID, tenantName, View.ComplianceStatus, View.ComplianceStatusID, View.PackageSubscriptionId, View.SelectedNodeID.HasValue ? View.SelectedNodeID.Value : 0);
            }
        }

        public Boolean IsItemStatusApproved(Int32 ItemId)
        {
            return ComplianceDataManager.IsItemStatusApproved(ItemId, View.TenantID);
        }

        public String GetItemDetails(Int32 itemId)
        {
            //UAT 401 -Add a field for Items to configure "Details" during student Order Process
            //return ComplianceDataManager.GetDataEntryComplianceItem(itemId, View.TenantID).Description;
            return ComplianceDataManager.GetDataEntryComplianceItem(itemId, View.TenantID).Details;
        }

        /// <summary>
        /// Get Editable bies by Package id
        /// </summary>
        /// <returns></returns>
        public List<ListCategoryEditableBies> GetEditableBiesByPackage()
        {
            if (String.IsNullOrEmpty(View.WorkQueue)) // Screen is opened by applicant
                return ComplianceSetupManager.GetEditableBiesByPackageId(View.PackageId, View.TenantID);
            else
                return new List<ListCategoryEditableBies>();
        }

        /// <summary>
        /// This method is used to call the Parallel Task for Pdf conversion and merging 
        /// </summary>
        public void CallParallelTaskPdfConversionMerging()
        {
            Dictionary<String, Object> conversionData = new Dictionary<String, Object>();
            //Use Poco class so that Entity will not get updated while running parallel tasks
            List<ApplicantDocumentPocoClass> lstApplicantDoc = new List<ApplicantDocumentPocoClass>();

            if (View.ToSaveApplicantUploadedDocuments.IsNotNull() && View.ToSaveApplicantUploadedDocuments.Count() >= 0)
            {
                foreach (var doc in View.ToSaveApplicantUploadedDocuments)
                {
                    ApplicantDocumentPocoClass appDoc = new ApplicantDocumentPocoClass();
                    appDoc.ApplicantDocumentID = doc.ApplicantDocumentID;
                    appDoc.FileName = doc.FileName;
                    appDoc.DocumentPath = doc.DocumentPath;
                    appDoc.PdfDocPath = doc.PdfDocPath;
                    appDoc.IsCompressed = doc.IsCompressed;
                    appDoc.Size = doc.Size;
                    lstApplicantDoc.Add(appDoc);
                }
                //conversionData.Add("ApplicantUploadedDocuments", View.ToSaveApplicantUploadedDocuments);
                conversionData.Add("ApplicantUploadedDocuments", lstApplicantDoc);
            }
            else
            {
                conversionData.Add("ApplicantUploadedDocuments", null);
            }
            conversionData.Add("OrganizationUserId", View.OrganiztionUserID);
            conversionData.Add("CurrentLoggedUserID", View.OrgUsrID); //View.CurrentLoggedInUserId UAT 1261
            conversionData.Add("TenantID", View.TenantID);

            Dictionary<String, Object> mergingData = new Dictionary<String, Object>();
            mergingData.Add("OrganizationUserId", View.OrganiztionUserID);
            mergingData.Add("CurrentLoggedUserID", View.OrgUsrID); //View.CurrentLoggedInUserId UAT 1261
            mergingData.Add("TenantID", View.TenantID);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            if (conversionData.IsNotNull() && conversionData.Count > 0 && mergingData.IsNotNull() && mergingData.Count > 0)
            {
                //ParallelTaskContext.ParallelTaskPdfConversionMerging(ConvertDocumentsIntoPdf, conversionData, MergeDocIntoUnifiedPdf, mergingData, LoggerService, ExceptiomService);
                Business.RepoManagers.DocumentManager.RunParallelTaskPdfConversionMerging(ConvertDocumentsIntoPdf, conversionData, MergeDocIntoUnifiedPdf, mergingData, LoggerService, ExceptiomService);
            }
        }

        /// <summary>
        /// This method is used to convert the list of documents into pdf document
        /// </summary>
        /// <param name="conversionData">conversionData (Data dictionary that conatins the applicantdocument table object ,tenantId,currentLoggedUserID)</param>
        private void ConvertDocumentsIntoPdf(Dictionary<String, Object> conversionData)
        {
            if (conversionData.IsNotNull() && conversionData.Count > 0)
            {
                Int32 tenantId;
                Int32 CurrentLoggedUserID;
                List<ApplicantDocumentPocoClass> applicantDocument = conversionData.GetValue("ApplicantUploadedDocuments") as List<ApplicantDocumentPocoClass>;
                conversionData.TryGetValue("TenantID", out tenantId);
                conversionData.TryGetValue("CurrentLoggedUserID", out CurrentLoggedUserID);
                Business.RepoManagers.DocumentManager.ConvertApplicantDocumentToPDF(applicantDocument, tenantId, CurrentLoggedUserID);
            }
        }

        /// <summary>
        /// This method is used to merge converted pdf documents into unified pdf document
        /// </summary>
        /// <param name="mergingData">mergingData (Data dictionary that conatins the organizationUserId,tenantId,CurrentLoggedUserIDk)</param>
        private void MergeDocIntoUnifiedPdf(Dictionary<String, Object> mergingData)
        {
            if (mergingData.IsNotNull() && mergingData.Count > 0)
            {
                Boolean isParallelTaskRunning = true;
                Int32 tenantId;
                Double totalTimeDiffernce = 0.0;
                Int32 CurrentLoggedUserID;
                Int32 OrganizationUserId;
                mergingData.TryGetValue("OrganizationUserId", out OrganizationUserId);
                mergingData.TryGetValue("TenantID", out tenantId);
                mergingData.TryGetValue("CurrentLoggedUserID", out CurrentLoggedUserID);

                List<lkpDocumentStatu> lkpDocumentStatus = Business.RepoManagers.DocumentManager.GetDocumentStatus(tenantId);
                //Get the document status id based on the Merging in progress statusc code
                Int32 documentStatusId = lkpDocumentStatus.Where(obj => obj.DMS_Code == DocumentStatus.MERGING_IN_PROGRESS.GetStringValue()).FirstOrDefault().DMS_ID;

                //Check status of the document for user who uploaded the document.Method return true if document status is Merging in progress  else false 
                isParallelTaskRunning = Business.RepoManagers.DocumentManager.IsMergingInProgress(OrganizationUserId, tenantId, documentStatusId);
                UnifiedPdfDocument unifiedPdfDocument = Business.RepoManagers.DocumentManager.GetAllMergingInProgressUnifiedDoc(OrganizationUserId, tenantId, documentStatusId);

                if (isParallelTaskRunning)
                {
                    for (int i = 0; i <= AppConsts.TEN; i++)
                    {
                        if (isParallelTaskRunning)
                        {
                            if (unifiedPdfDocument.IsNotNull())
                            {
                                totalTimeDiffernce = (DateTime.Now - unifiedPdfDocument.UPD_CreatedOn).TotalMinutes;
                            }

                            if (totalTimeDiffernce >= 10.0)
                            {
                                Business.RepoManagers.DocumentManager.AppendApplicantDocument(OrganizationUserId, tenantId, CurrentLoggedUserID);
                                break;
                            }
                            else if (i == AppConsts.TEN)
                            {
                                View.ErrorMessage = "Could not start fresh merging as pervious merging is still in progress for tenantId: " + tenantId + "," + " appplicantId: " + OrganizationUserId + "." + " Request is an abandoned.";
                            }
                            else
                            {
                                //Wait for 3 seconds if document status is Merging in Progress
                                System.Threading.Thread.Sleep(3000);
                                isParallelTaskRunning = Business.RepoManagers.DocumentManager.IsMergingInProgress(OrganizationUserId, tenantId, documentStatusId);
                            }
                        }
                        else
                        {
                            Business.RepoManagers.DocumentManager.AppendApplicantDocument(OrganizationUserId, tenantId, CurrentLoggedUserID);
                            break;
                        }
                    }
                }
                else
                {
                    Business.RepoManagers.DocumentManager.AppendApplicantDocument(OrganizationUserId, tenantId, CurrentLoggedUserID);

                }

            }
        }

        #region PARALLEL TASK CALLING FOR QUEUE MANAGEMENT

        /// <summary>
        /// Set handle assignment data in dictionary.
        /// </summary>
        /// <param name="isResetBusinessProcess">true=if item expired otherwise false.</param>
        /// <param name="statusCode">statusCode</param>
        /// <param name="statusId">StatusId</param>
        /// <param name="applicantComplianceItemnId">ApplicantComplianceItemId</param>
        /// <returns>dictionary</returns>
        public Dictionary<String, Object> SetHandleAssignmentData(Boolean isResetBusinessProcess, String statusCode, Int32? statusId, Int32 applicantComplianceItemnId, Int32 complianceItemId, ApplicantComplianceItemData applicantItemData, Boolean notReviewed)
        {
            Int32 queueId = 0;
            String queueCode;
            Dictionary<String, Object> dicHandleAssignmentData = new Dictionary<String, Object>();
            Dictionary<String, Object> dicQueueFields = new Dictionary<String, Object>();
            String queueFieldsXML = String.Empty;
            String rushOrderStatusCode = String.Empty;
            String rushOrderStatusText = String.Empty;
            String itemComplianceStatusText = String.Empty;
            String complianceCategoryName = String.Empty;
            String complianceItemName = String.Empty;
            if (applicantItemData.IsNotNull())
            {
                complianceItemName = !String.IsNullOrEmpty(applicantItemData.ComplianceItem.ItemLabel) ? applicantItemData.ComplianceItem.ItemLabel : applicantItemData.ComplianceItem.Name;
                if (applicantItemData.ApplicantComplianceCategoryData.ComplianceCategory.IsNotNull())
                {
                    complianceCategoryName = !String.IsNullOrEmpty(applicantItemData.ApplicantComplianceCategoryData.ComplianceCategory.CategoryLabel) ? applicantItemData.ApplicantComplianceCategoryData.ComplianceCategory.CategoryLabel : applicantItemData.ApplicantComplianceCategoryData.ComplianceCategory.CategoryName;
                }
            }
            if (View.RushOrderStatusId.IsNotNull())
            {
                lkpOrderStatu ordrStatus = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderStatu>(View.TenantID).Where(x => x.IsDeleted == false && x.OrderStatusID == View.RushOrderStatusId).FirstOrDefault();
                rushOrderStatusCode = ordrStatus.Code;
                rushOrderStatusText = ordrStatus.Name;
            }
            if (!statusCode.IsNullOrEmpty())
            {
                lkpItemComplianceStatu complianceStatus = LookupManager.GetLookUpData<Entity.ClientEntity.lkpItemComplianceStatu>(View.TenantID).Where(x => x.IsDeleted == false && x.Code == statusCode).FirstOrDefault();
                statusId = complianceStatus.ItemComplianceStatusID;
                itemComplianceStatusText = complianceStatus.Name;
            }
            //Get the queueid on the basis status code
            if (statusCode == ApplicantItemComplianceStatus.Pending_Review.GetStringValue())
            {
                queueCode = QueueMetaDataType.Verification_Queue_For_Admin.GetStringValue();
                queueId = LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(View.TenantID).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == queueCode).FirstOrDefault().QMD_QueueID;
            }
            else if (statusCode == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue())
            {
                queueCode = QueueMetaDataType.Verification_Queue_For_ClientAdmin.GetStringValue();
                queueId = LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(View.TenantID).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == queueCode).FirstOrDefault().QMD_QueueID;
            }
            else if ((statusCode == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue()) && applicantItemData.IsReviewerTypeAdmin == true)
            {
                queueCode = QueueMetaDataType.Exception_Queue_For_Admin.GetStringValue();
                queueId = LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(View.TenantID).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == queueCode).FirstOrDefault().QMD_QueueID;
            }
            else if ((statusCode == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue()) && (applicantItemData.IsReviewerTypeAdmin == false && applicantItemData.IsReviewerTypeClientAdmin == true))
            {
                queueCode = QueueMetaDataType.Exception_Queue_For_ClientAdmin.GetStringValue();
                queueId = LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(View.TenantID).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == queueCode).FirstOrDefault().QMD_QueueID;
            }

            if (queueId > 0)
            {
                //Create dictionary for QueueFields.
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ApplicantName, View.ApplicantName);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ComplianceItemId, complianceItemId);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.CategoryId, View.CategoryDataContract.ComplianceCategoryId);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.PackageID, View.PackageId);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.SubmissionDate, applicantItemData.SubmissionDate);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.System_Status, String.Empty);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Rush_Order_Status, rushOrderStatusCode);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ApplicantComplianceItemID, applicantComplianceItemnId);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.HierarchyNodeID, View.HierarchyID);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ApplicantId, View.ApplicantID);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Item_Name, complianceItemName);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Category_Name, complianceCategoryName);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Package_Name, View.PackageName);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Verification_Status_Text, itemComplianceStatusText);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Rush_Order_Status_Text, rushOrderStatusText);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Verification_Status, statusId);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Assigned_To_User, applicantItemData.AssignedToUser.IsNull() ? 0 : applicantItemData.AssignedToUser);
                dicQueueFields.Add("ResetReviewProcess", false);
                dicQueueFields.Add("BypassInitialReview", false);
                //dicQueueFields.Add("NotReviewed", notReviewed);

                //Get XML for queueFields.
                queueFieldsXML = "<Queues>" + QueueManagementManager.GetQueueFieldXMLString(dicQueueFields, queueId, applicantComplianceItemnId, notReviewed) + "</Queues>";

                //Create dictionary for handle assignment data.
                dicHandleAssignmentData.Add("CurrentLoggedInUserId", View.OrgUsrID); //View.CurrentLoggedInUserId UAT 1261
                dicHandleAssignmentData.Add("TenantId", View.TenantID);
                dicHandleAssignmentData.Add("QueueRecordXML", queueFieldsXML);
                return dicHandleAssignmentData;
            }
            return null;
        }

        /// <summary>
        /// Call the ClearQueueRecords stored procedure.
        /// </summary>
        /// <param name="_lstNextQueueItems"></param>
        /// <param name="_lstNextLevelItems"></param>
        public void ClearQueueRecords(Int32 applicantComplianceItemID, String statusCode)
        {
            String queueCode;
            Int32 queueId = 0;
            if (statusCode == ApplicantItemComplianceStatus.Pending_Review.GetStringValue())
            {
                queueCode = QueueMetaDataType.Verification_Queue_For_Admin.GetStringValue();
                queueId = LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(View.TenantID).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == queueCode).FirstOrDefault().QMD_QueueID;
            }
            else if (statusCode == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue())
            {
                queueCode = QueueMetaDataType.Verification_Queue_For_ClientAdmin.GetStringValue();
                queueId = LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(View.TenantID).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == queueCode).FirstOrDefault().QMD_QueueID;
            }
            else if (statusCode == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue())
            {
                queueCode = QueueMetaDataType.Exception_Queue_For_Admin.GetStringValue();
                queueId = LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(View.TenantID).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == queueCode).FirstOrDefault().QMD_QueueID;
            }
            StringBuilder queueFieldString = new StringBuilder();
            queueFieldString.Append("<Queues>");
            queueFieldString.Append("<QueueDetail>");
            queueFieldString.Append("<OldQueueID>" + queueId + "</OldQueueID>");
            queueFieldString.Append("<RecordID>" + applicantComplianceItemID + "</RecordID>");
            queueFieldString.Append("</QueueDetail>");
            queueFieldString.Append("</Queues>");
            QueueManagementManager.ClearQueueRecords(View.CurrentLoggedInUserId, Convert.ToString(queueFieldString), View.TenantID);

        }

        public void CallParallelTaskForMail(List<ExceptionRejectionContract> lstExceptionRejectionContract, Boolean isCategory)
        {
            Dictionary<String, Object> mailData = new Dictionary<String, Object>();

            mailData.Add("lstExceptionRejectionContract", lstExceptionRejectionContract);
            mailData.Add("tenantId", View.TenantID);
            mailData.Add("isCategory", isCategory);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            ParallelTaskContext.PerformParallelTask(SendExceptionMail, mailData, LoggerService, ExceptiomService);
        }

        #endregion

        public AssignmentProperty GetAssignmentProperty()
        {
            return ComplianceSetupManager.GetAssignmentPropertyDetails(View.CurrentViewContext.ComplianceCategoryId, -1, View.CurrentViewContext.PackageId, -1, View.CurrentViewContext.TenantID, RuleSetTreeNodeType.Category);
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

        public String GetNodeHiearchy(Int32 packageSubscriptionId)
        {
            return ComplianceDataManager.GetNodeHiearchy(View.TenantID, packageSubscriptionId);
        }

        public List<Entity.OrganizationUser> GetAdminUserByTenantId(Int32 tenantId)
        {
            List<Entity.OrganizationUser> lstAdminUsers = SecurityManager.GetClientAdminUsersByTanentId(tenantId).Where(cond => cond.IsActive == true
                                       && cond.IsDeleted == false).Select(x => new Entity.OrganizationUser
                                       {
                                           FirstName = x.FirstName + " " + x.LastName,
                                           OrganizationUserID = x.OrganizationUserID,
                                           PrimaryEmailAddress = x.aspnet_Users.aspnet_Membership.Email
                                       }).ToList(); ;

            return lstAdminUsers;
        }

        public void SendExceptionMail(Dictionary<String, Object> data)
        {
            List<ExceptionRejectionContract> lstExceptionRejectionContract = data.GetValue("lstExceptionRejectionContract") as List<ExceptionRejectionContract>;

            Int32 tenantId = Convert.ToInt32(data.GetValue("tenantId"));
            Boolean isCategory = Convert.ToBoolean(data.GetValue("isCategory"));
            foreach (var excRejContract in lstExceptionRejectionContract)
            {
                CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_EXCEPTION_APPLIED;
                if (isCategory)
                    commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_CATEGORY_EXCEPTION_APPLIED;

                //Create Dictionary for Email Content
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, excRejContract.UserFullName);
                dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, excRejContract.ApplicantName);
                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, excRejContract.ApplicationUrl);

                dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, excRejContract.PackageName);
                if (!isCategory)
                    dictMailData.Add(EmailFieldConstants.COMPLIANCE_ITEM_NAME, excRejContract.ComplianceItemName);
                dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, excRejContract.InstituteName);
                dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, excRejContract.NodeHierarchy);
                dictMailData.Add(EmailFieldConstants.CATEGORY_NAME, excRejContract.CategoryName);

                //Create dictionary for MockUp Data
                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                mockData.UserName = AppConsts.BACKGROUND_PROCESS_USER_NAME;
                mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                //Send mail
                CommunicationManager.SendPackageNotificationMail(commSubEvent, dictMailData, mockData, tenantId, excRejContract.HierarchyNodeID);

                //Send Message
                CommunicationManager.SaveMessageContent(commSubEvent, dictMailData, AppConsts.BACKGROUND_PROCESS_USER_VALUE, tenantId);
            }
        }

        #region SEND NOTIFICATION ON FIRST ITEM SUBMITT

        /// <summary>
        /// Save system communication delivert setting for first item submitt
        /// </summary>
        /// <param name="itemData">ApplicantComplianceItemData</param>
        public void SaveSystComDeliverySettingForFirstItemSubmit(ApplicantComplianceItemData itemData, Boolean isSendMailBySeriesDataSaved)
        {
            CommunicationSubEvents subEventFirstItemSubmission = CommunicationSubEvents.NOTIFICATION_FOR_FIRST_ITEM_SUBMISSION;
            if (!CommunicationManager.IsFirstItemNotificationExist(subEventFirstItemSubmission.GetStringValue(), View.CurrentLoggedInUserId))
            {
                #region UAT-1607
                if (isSendMailBySeriesDataSaved)
                {
                    itemData = GetItemDataAfterSeriesDataSaved();
                }
                #endregion
                String applicationUrl = WebSiteManager.GetInstitutionUrl(View.TenantID);
                String surveyMonkeyLink = ComplianceSetupManager.GetSurveyMonkeyLink(View.TenantID, View.CurrentLoggedInUserId, subEventFirstItemSubmission.GetStringValue(), View.PackageName, itemData.ApplicantComplianceCategoryData.ComplianceCategory.CategoryName, itemData.ComplianceItem.Name, itemData.ComplianceItem.ComplianceItemID, View.PackageId, View.CategoryDataContract.ComplianceCategoryId);
                //Create Dictionary
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(View.FirstName, " ", View.LastName));
                //dictMailData.Add(EmailFieldConstants.COMPLIANCE_ITEM_NAME, itemData.ComplianceItem.Name);
                dictMailData.Add(EmailFieldConstants.COMPLIANCE_ITEM_NAME, itemData.ComplianceItem.ItemLabel.IsNullOrEmpty() ? itemData.ComplianceItem.Name : itemData.ComplianceItem.ItemLabel);
                //dictMailData.Add(EmailFieldConstants.CATEGORY_NAME, itemData.ApplicantComplianceCategoryData.ComplianceCategory.CategoryName);
                dictMailData.Add(EmailFieldConstants.CATEGORY_NAME, itemData.ApplicantComplianceCategoryData.ComplianceCategory.CategoryLabel.IsNullOrEmpty() ? itemData.ApplicantComplianceCategoryData.ComplianceCategory.CategoryName : itemData.ApplicantComplianceCategoryData.ComplianceCategory.CategoryLabel);
                dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, View.PackageName);
                dictMailData.Add(EmailFieldConstants.SURVEY_MONKEY_LINK, surveyMonkeyLink);
                dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, GetInstitutionName(View.TenantID));
                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);

                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                mockData.UserName = string.Concat(View.FirstName, " ", View.LastName);
                mockData.EmailID = View.PrimaryEmailAddress;
                mockData.ReceiverOrganizationUserID = View.CurrentLoggedInUserId;

                //Send mail
                //CommunicationManager.SendPackageNotificationMail(subEventFirstItemSubmission, dictMailData, mockData, View.TenantID, View.HierarchyID.HasValue ? View.HierarchyID.Value : 0);
                //UAT - 1067 : Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on. 
                CommunicationManager.SendPackageNotificationMail(subEventFirstItemSubmission, dictMailData, mockData, View.TenantID, View.SelectedNodeID.HasValue ? View.SelectedNodeID.Value : 0);


                //Send Message
                CommunicationManager.SaveMessageContent(subEventFirstItemSubmission, dictMailData, View.CurrentLoggedInUserId, View.TenantID);
            }
        }
        #endregion

        /// <summary>
        /// To check if document is already uploaded
        /// </summary>
        /// <param name="documentName"></param>
        /// <param name="documentSize"></param>
        /// <param name="documentUploadedBytes"></param>
        /// <returns></returns>
        public String IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, byte[] documentUploadedBytes)
        {
            Int32 _organizationUserID = View.CurrentLoggedInUserId;
            Int32 _tenantId = View.TenantID;
            if (ComplianceDataManager.IsDocumentAlreadyUploaded(documentName, documentSize, _organizationUserID, _tenantId))
            {
                return documentName;
            }
            //Compare original document MD5Hash
            List<ApplicantDocument> applicantDocuments = ComplianceDataManager.GetApplicantDocuments(_organizationUserID, _tenantId);
            String md5Hash = CommonFileManager.GetMd5Hash(documentUploadedBytes);

            if (applicantDocuments.IsNotNull())
            {
                //UAT-2244
                Int32 ReqFieldUploadDocType = LookupManager.GetLookUpData<lkpDocumentType>(_tenantId).Where(cond => cond.DMT_Code == DocumentType.REQUIREMENT_FIELD_UPLOAD_DOCUMENT.GetStringValue()).FirstOrDefault().DMT_ID;
                Int32 personalDocType = LookupManager.GetLookUpData<lkpDocumentType>(_tenantId).Where(cond => cond.DMT_Code == DocumentType.PERSONAL_DOCUMENT.GetStringValue()).FirstOrDefault().DMT_ID;
                var applicantDocument = applicantDocuments.FirstOrDefault(x => x.OriginalDocMD5Hash == md5Hash
                                    && (x.DocumentType.IsNull() || (x.DocumentType != ReqFieldUploadDocType && x.DocumentType != personalDocType)));
                if (applicantDocument.IsNotNull())
                {
                    return applicantDocument.FileName;
                }
            }

            return null;
        }

        public void GetWholeCategoryItemID(Guid wholeCatGUID)
        {
            List<ComplianceItem> cmpItems = new List<ComplianceItem>();
            ComplianceItem cmpItem = ComplianceDataManager.GetWholeCategoryItemID(View.TenantID, View.OrgUsrID, wholeCatGUID, View.CurrentViewContext.ComplianceCategoryId); //View.CurrentViewContext.CurrentLoggedInUserId UAT 1261
            cmpItems.Add(cmpItem);
            View.lstAvailableItems = cmpItems;
        }

        public void DeleteCategoryException(Int32 complianceCategoryId)
        {
            //UPDATE Applicant Compliance Category data. Set [CategoryExceptionStatusID] and [ExpirationDate] to null.
            //Changes as per UAT-819 WB: Category Exception enhancements
            String catStatusApprovedCode = ApplicantCategoryComplianceStatus.Approved.GetStringValue();

            ApplicantComplianceItemData appCmpItmData = ComplianceDataManager.UpdateApplicantCmpCatData(View.TenantID, View.OrgUsrID,
               complianceCategoryId, View.CurrentViewContext.PackageSubscriptionId, catStatusApprovedCode); //View.CurrentViewContext.CurrentLoggedInUserId UAT 1261

            if (appCmpItmData.IsNotNull())
            {
                //Clear Queue
                ClearQueueRecords(appCmpItmData.ApplicantComplianceItemID, appCmpItmData.lkpItemComplianceStatu.Code);

                //Run rules 
                evaluatePostSubmitRules(appCmpItmData.ComplianceItemID, complianceCategoryId);

                // Changes related to short series 'usp_Rule_EvaluateAdjustItemSeriesRules' SP.
                List<Int32> itemIds = new List<Int32>();
                itemIds.Add(appCmpItmData.ComplianceItemID);
                View.DicCategoryDataForItemSeriesRule = new Dictionary<Int32, List<Int32>>();
                if (appCmpItmData.ComplianceItemID > AppConsts.NONE)
                {
                    View.DicCategoryDataForItemSeriesRule.Add(complianceCategoryId, itemIds);
                }
                EvaluateAdjustItemSeriesRules();


                //UAT-3112:-
                if (!appCmpItmData.ApplicantComplianceItemID.IsNullOrEmpty())
                {
                    string acid = Convert.ToString(appCmpItmData.ApplicantComplianceItemID);
                    ComplianceDataManager.SaveBadgeFormNotificationData(View.TenantID, acid, null, null, View.CurrentLoggedInUserId);
                }
            }
        }

        private void SetQueueImaging()
        {

            Dictionary<String, Object> dataDictionary = new Dictionary<String, Object>();
            dataDictionary.Add("TenantID", View.TenantID);
            QueueImagingManager.AsignQueueImagingRepoInstance(dataDictionary);
        }
        #region UAT-509: WB: Ability to limit admin access to read only on the ver details and applicant search details screen.
        /// <summary>
        /// Method to set the verification permission on selected node for the current logged in user.
        /// </summary>
        public void SetUserVerificationPermission()
        {
            View.IsFullPermissionForVerification = true;
            if (View.TenantID > 0 && View.PackageSubscriptionId > 0)
            {
                PackageSubscription packageSubscription = ComplianceDataManager.GetPackageSubscriptionByID(View.TenantID, View.PackageSubscriptionId);
                if (packageSubscription.IsNotNull() && packageSubscription.Order.IsNotNull())
                {
                    List<UserNodePermissionsContract> lstUserNodePermission = ComplianceSetupManager.GetUserNodePermissionForVerificationAndProfile(View.TenantID, View.CurrentLoggedInUserId).ToList();
                    UserNodePermissionsContract userNodePermission = lstUserNodePermission.FirstOrDefault(cond => cond.DPM_ID == packageSubscription.Order.HierarchyNodeID);
                    if (userNodePermission.IsNotNull() && userNodePermission.VerificationPermissionCode == LkpPermission.ReadOnly.GetStringValue())
                    {
                        View.IsFullPermissionForVerification = false;
                    }
                }
            }
        }
        #endregion

        #region UAT-1049:Admin Data Entry
        /// <summary>
        /// Method that set the data entry documnet status id for new status.  
        /// </summary>
        private void GetDataEntryDocStatus()
        {
            if (View.TenantID > AppConsts.NONE)
            {
                String newDataEntryDocStatus = DataEntryDocumentStatus.NEW.GetStringValue();
                lkpDataEntryDocumentStatu tempDataEntryDocStatus = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDataEntryDocumentStatu>(View.TenantID)
                                                                               .FirstOrDefault(cnd => cnd.LDEDS_Code == newDataEntryDocStatus && cnd.LDEDS_IsDeleted == false);
                if (!tempDataEntryDocStatus.IsNullOrEmpty())
                    View.DataEntryDocNewStatusId = tempDataEntryDocStatus.LDEDS_ID;

                String newDataEntryCompleteStatus = DataEntryDocumentStatus.COMPLETE.GetStringValue();
                lkpDataEntryDocumentStatu tempDataEntryDocStatusComplete = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDataEntryDocumentStatu>(View.TenantID)
                                                                               .FirstOrDefault(cnd => cnd.LDEDS_Code == newDataEntryCompleteStatus && cnd.LDEDS_IsDeleted == false);
                if (!tempDataEntryDocStatusComplete.IsNullOrEmpty())
                    View.DataEntryDocCompleteStatusId = tempDataEntryDocStatusComplete.LDEDS_ID;

                //String viewDocumentTypeCode = DocumentType.COMPLIANCE_VIEW_DOCUMENT.GetStringValue();
                //View.ViewDocumentTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentType>(View.TenantID)
                //                                                               .FirstOrDefault(cnd => cnd.DMT_Code == viewDocumentTypeCode && cnd.DMT_IsDeleted == false).DMT_ID;
            }
        }
        #endregion

        public List<ComplianceItem> GetOrderedComplianceItem(List<ComplianceItem> cmpItems)
        {
            List<ComplianceItem> compItems = new List<ComplianceItem>();
            if (cmpItems != null && cmpItems.Count > 0)
            {
                foreach (var item in cmpItems)
                {
                    var cmpItem = item;
                    Int32 itemID = item.ComplianceItemID;
                    //UAT-1607
                    cmpItem.CompItemID = Convert.ToString(item.ComplianceItemID);
                    Int32 displayOrder = item.ComplianceCategoryItems.FirstOrDefault(x => x.CCI_ItemID == itemID && x.CCI_CategoryID == View.CurrentViewContext.ComplianceCategoryId).CCI_DisplayOrder;
                    cmpItem.DisplayOrder = displayOrder;
                    compItems.Add(cmpItem);
                }
                compItems = compItems.OrderBy(x => x.DisplayOrder).ToList();
            }
            return compItems;
        }

        /// <summary>
        /// heck If ComplianceRequired Is False on current date
        /// </summary>
        /// <param name="categoryStatusCode"></param>
        /// <returns></returns>
        private string CheckIfComplianceRequiredIsFalse(String categoryStatusCode)
        {
            //UAt-1209: As an application admin, I should be able to enter a date range for when a category should be compliance required/not required. 
            CompliancePackageCategory compPackageCategory = ComplianceSetupManager.GetCompliancePackageCategory(View.TenantID, View.CategoryDataContract.ComplianceCategoryId, View.PackageId);
            if (compPackageCategory.IsNotNull())
            {
                DateTime? newStartDate = compPackageCategory.CPC_ComplianceRqdStartDate;
                DateTime? newEndDate = compPackageCategory.CPC_ComplianceRqdEndDate;
                DateTime currentDate = DateTime.Now;
                if ((compPackageCategory.CPC_ComplianceRequired == false
                            && ((newStartDate.IsNull() && newEndDate.IsNull())
                                || ((currentDate.Month > newStartDate.Value.Month || (currentDate.Month == newStartDate.Value.Month && currentDate.Day >= newStartDate.Value.Day))
                                && (currentDate.Month < newEndDate.Value.Month || (currentDate.Month == newEndDate.Value.Month && currentDate.Day <= newEndDate.Value.Day) || (currentDate.Month > newEndDate.Value.Month && newEndDate.Value.Month < newStartDate.Value.Month)))
                                ))
                       || (compPackageCategory.CPC_ComplianceRequired == true
                            && (newStartDate.IsNotNull() && newEndDate.IsNotNull())
                                && ((currentDate.Month < newStartDate.Value.Month || (currentDate.Month == newStartDate.Value.Month && currentDate.Day < newStartDate.Value.Day))
                                || (currentDate.Month > newEndDate.Value.Month || (currentDate.Month == newEndDate.Value.Month && currentDate.Day > newEndDate.Value.Day))))
                        )
                {
                    categoryStatusCode = ApplicantCategoryComplianceStatus.Approved.GetStringValue();
                }
            }
            return categoryStatusCode;
        }

        #region UAT-1607:Student Data Entry Screen changes
        private List<ComplianceItem> RemoveSeriesItemsFromAvailableItems(List<ComplianceItem> lstVailableItems)
        {
            List<Int32> lstSeriesItemIds = new List<Int32>();

            if (!View.lstItemSeries.IsNullOrEmpty())
            {
                View.lstItemSeries.ForEach(itemSeries =>
                {
                    var itemSeriesItemLst = itemSeries.ItemSeriesItems.Where(cnd => !cnd.ISI_IsDeleted);
                    if (!itemSeriesItemLst.IsNullOrEmpty())
                    {
                        itemSeriesItemLst.ForEach(seriesItem =>
                        {
                            lstSeriesItemIds.Add(seriesItem.ISI_ItemID);
                        });
                    }
                });
                lstVailableItems = lstVailableItems.Where(cnd => !lstSeriesItemIds.Contains(cnd.ComplianceItemID)).ToList();
            }
            return GetOrderedComplianceItem(lstVailableItems);
        }

        private List<ComplianceItem> SetItemSeriesInAvailableItems(List<ComplianceItem> lstVailableItems, List<ItemSery> lstItemSeries)
        {
            List<Int32> categoryApprovedItemIds = new List<Int32>();
            ApplicantComplianceCategoryData appCompCatData = View.Subscription.ApplicantComplianceCategoryDatas.FirstOrDefault(cnd =>
                                                                                cnd.ComplianceCategoryID == View.CurrentViewContext.ComplianceCategoryId);

            if (!appCompCatData.IsNullOrEmpty())
            {
                String approvedItemStatus = ApplicantItemComplianceStatus.Approved.GetStringValue();
                categoryApprovedItemIds = appCompCatData.ApplicantComplianceItemDatas.Where(cond => (cond.lkpItemComplianceStatu != null
                                                                                            && !View.ExpiringItemList.Contains(cond.ComplianceItemID)
                                                                                            && cond.lkpItemComplianceStatu.Code == approvedItemStatus && !cond.IsDeleted))
                                                                                          .Select(slct => slct.ComplianceItemID).Distinct().ToList();
            }
            if (!lstItemSeries.IsNullOrEmpty())
            {
                lstItemSeries.ForEach(itemSeries =>
                {
                    var seriesItems = itemSeries.ItemSeriesItems.Where(cnd => !cnd.ISI_IsDeleted).Select(x => x.ISI_ItemID);
                    Int32 totalSeriesItemCount = seriesItems.IsNullOrEmpty() ? AppConsts.NONE : seriesItems.Distinct().Count();
                    List<Int32> approvedseriesItems = new List<Int32>();
                    approvedseriesItems = categoryApprovedItemIds.Where(x => seriesItems.Contains(x)).ToList();
                    if ((approvedseriesItems.Count < totalSeriesItemCount) || (approvedseriesItems.Count == totalSeriesItemCount && itemSeries.IS_IsAvailablePostApproval))
                    {
                        ComplianceItem compItem = new ComplianceItem();
                        compItem.ComplianceItemID = itemSeries.IS_ID;
                        compItem.CompItemID = "IS_" + itemSeries.IS_ID;
                        compItem.Name = itemSeries.IS_Name;
                        compItem.ItemLabel = itemSeries.IS_Label;
                        compItem.IsItemSeries = true;
                        lstVailableItems.Add(compItem);
                    }
                });
            }
            return lstVailableItems;
        }

        public List<ItemSeriesItemContract> GetItemSeriesItemForCategories()
        {
            List<Int32> lstCategoryIds = new List<Int32>();
            if (!View.ClientPackage.IsNullOrEmpty())
            {
                lstCategoryIds = View.ClientPackage.CompliancePackageCategories.Where(cpc => !cpc.CPC_IsDeleted && cpc.ComplianceCategory.IsActive).Select(slct => slct.CPC_CategoryID).ToList();
                return ComplianceDataManager.GetItemSeriesItemForCategories(View.TenantID, lstCategoryIds);
            }
            return new List<ItemSeriesItemContract>();
        }

        public String GetItemSeriesDetail(Int32 itemSeriesId)
        {
            String itemSeriesDetail = String.Empty;
            ItemSery itemSeries = ComplianceDataManager.GetItemSeriesByID(View.TenantID, itemSeriesId);
            if (!itemSeries.IsNullOrEmpty())
            {
                itemSeriesDetail = itemSeries.IS_Details;
            }
            return itemSeriesDetail;
        }

        public List<Int32> SaveShotSeriesAttributeData()
        {
            List<Int32> catItemIds = new List<Int32>();
            String seriesAttributeXML = ComplianceDataManager.GetSeriesAttributeXML(View.lstAttributesData);
            String attrDocumentXML = ComplianceDataManager.GetAttributeDocumentXMLForSeries(View.AttributeDocuments, View.lstAttributesData, View.ViewAttributeDocuments);

            ShotSeriesSaveResponse saveResponse = ComplianceDataManager.SaveSeriesAttributeData(View.TenantID, View.CategoryDataContract.PackageSubscriptionId,
                                                                       View.ItemDataContract.ComplianceItemId, View.CurrentLoggedInUserId, seriesAttributeXML,
                                                                       attrDocumentXML, ShotSeriesHandleCalledFrom.StudentDataEntry, View.OrgUsrID, View.ItemDataContract.Notes);

            if (!saveResponse.IsNullOrEmpty() && saveResponse.StatusCode == AppConsts.ONE)
            {
                View.CurrentViewContext.UIValidationErrors = saveResponse.Message;
            }
            else if (!saveResponse.IsNullOrEmpty() && saveResponse.StatusCode == AppConsts.NONE)
            {
                if (!saveResponse.lstItemData.IsNullOrEmpty())
                {
                    List<ItemData> lstItemDataSaved = saveResponse.lstItemData.Where(cond => cond.IsRandomReview == AppConsts.ONE).ToList();
                    if (!lstItemDataSaved.IsNullOrEmpty())
                    {
                        String itemIds = String.Join(",", lstItemDataSaved.Select(sel => sel.ItemID));
                        List<ItemReconciliationAvailiblityContract> lstItemReconciliationAvailiblity = QueueManagementManager.GetItemReconciliationAvailiblityStatus(View.TenantID, itemIds, View.CategoryDataContract.PackageSubscriptionId);
                    }

                    // Changes related to short series 'usp_Rule_EvaluateAdjustItemSeriesRules' SP.
                    catItemIds = saveResponse.lstItemData.Select(slct => slct.ItemID).ToList();
                    View.DicCategoryDataForItemSeriesRule = new Dictionary<Int32, List<Int32>>();
                    if (!catItemIds.IsNullOrEmpty())
                    {
                        View.DicCategoryDataForItemSeriesRule.Add(View.CategoryDataContract.ComplianceCategoryId, catItemIds);
                    }
                }
                //Send mail for FirstItemSubmitted
                SaveSystComDeliverySettingForFirstItemSubmit(null, true);
                //Send mail for Compliance Status change.
                SendMailOfComplianceStatusChangeForSeries();
                //UAT-2618:
                ComplianceDataManager.UpdateIsDocAssociated(View.TenantID, View.PackageSubscriptionId, true, View.CurrentLoggedInUserId);
            }

            return catItemIds;
        }

        public Int32 GetItemSeriesIDForItem(Int32 compItemId, Int32 CompCategoryId)
        {
            return ComplianceDataManager.GetItemSeriesIDForItem(compItemId, CompCategoryId, View.TenantID);
        }

        public void SuffleShotSeriesItemData(Int32 itemSeriesID, Int32 complianceCategoryId)
        {
            if (itemSeriesID > AppConsts.NONE)
            {
                ShotSeriesSaveResponse saveResponse = ComplianceDataManager.SaveSeriesAttributeData(View.TenantID, View.PackageSubscriptionId,
                                                                          itemSeriesID, View.CurrentLoggedInUserId, null, null,
                                                                          ShotSeriesHandleCalledFrom.StudentDataEntry, View.OrgUsrID);
                if (!saveResponse.IsNullOrEmpty() && saveResponse.StatusCode == AppConsts.ONE)
                {
                    View.CurrentViewContext.UIValidationErrors = saveResponse.Message;
                }
                else if (!saveResponse.IsNullOrEmpty() && saveResponse.StatusCode == AppConsts.NONE && !saveResponse.lstItemData.IsNullOrEmpty())
                {
                    // Changes related to short series 'usp_Rule_EvaluateAdjustItemSeriesRules' SP.
                    List<Int32> catItemIds = new List<Int32>();
                    catItemIds = saveResponse.lstItemData.Select(slct => slct.ItemID).ToList();
                    if (!View.DicCategoryDataForItemSeriesRule.IsNullOrEmpty())
                    {
                        catItemIds.AddRange(View.DicCategoryDataForItemSeriesRule.GetValue(complianceCategoryId));
                    }
                    if (!catItemIds.IsNullOrEmpty())
                    {
                        View.DicCategoryDataForItemSeriesRule = new Dictionary<Int32, List<Int32>>();
                        View.DicCategoryDataForItemSeriesRule.Add(complianceCategoryId, catItemIds.Distinct().ToList());
                    }

                    //UAT-2618:
                    ComplianceDataManager.UpdateIsDocAssociated(View.TenantID, View.PackageSubscriptionId, true, View.CurrentLoggedInUserId);
                }
            }
        }

        private void SendMailOfComplianceStatusChangeForSeries()
        {
            if (View.ComplianceStatusID != 0)
            {
                String tenantName = String.Empty;
                Entity.Tenant tenant = SecurityManager.GetTenant(View.TenantID);
                if (!tenant.IsNullOrEmpty())
                    tenantName = tenant.TenantName;
                //Send Mail
                ComplianceDataManager.SendMailOnComplianceStatusChange(View.TenantID, tenantName, View.ComplianceStatus, View.ComplianceStatusID, View.PackageSubscriptionId, View.SelectedNodeID.HasValue ? View.SelectedNodeID.Value : 0);
            }
        }

        public ApplicantComplianceItemData GetItemDataAfterSeriesDataSaved()
        {
            ApplicantComplianceItemData appCompItemData = new ApplicantComplianceItemData();
            PackageSubscription subscription = ComplianceDataManager.GetPackageSubscriptionByID(View.TenantID, View.PackageSubscriptionId);
            if (!subscription.IsNullOrEmpty() && !subscription.ApplicantComplianceCategoryDatas.IsNullOrEmpty())
            {
                ApplicantComplianceCategoryData appCompCatData = subscription.ApplicantComplianceCategoryDatas.FirstOrDefault(x =>
                                                                              x.ComplianceCategoryID == View.ComplianceCategoryId && !x.IsDeleted);

                if (!appCompCatData.IsNullOrEmpty() && !appCompCatData.ApplicantComplianceItemDatas.IsNullOrEmpty())
                {
                    appCompItemData = appCompCatData.ApplicantComplianceItemDatas.Where(cnd => !cnd.IsDeleted).OrderByDescending(ord => ord.CreatedOn).First();
                }
            }
            return appCompItemData;
        }
        #endregion

        private void SendItemStatusChangeNotification(Int32 compItemID, String preItemCompStatus)
        {
            //Send Notification On Item Status Changed To Review Status
            ComplianceDataManager.SendNotificationOnItemStatusChangedToReviewStatus(false, View.TenantID, View.PackageSubscriptionId,
                                                                                    View.ClientCompliancePackageID, View.ComplianceCategoryId,
                                                                                    compItemID, View.CurrentLoggedInUserId, View.OrgUsrID,
                                                                                    preItemCompStatus);
        }

        private String GetOldItemStatus(Int32 applicantComplianceItemId)
        {
            ApplicantComplianceItemData appCompItemData = ComplianceDataManager.GetApplicantComplianceItemDataByID(View.TenantID, applicantComplianceItemId);

            if (!appCompItemData.IsNullOrEmpty())
            {
                return appCompItemData.lkpItemComplianceStatu.IsNullOrEmpty() ? String.Empty : appCompItemData.lkpItemComplianceStatu.Code;
            }
            return String.Empty;
        }

        public Dictionary<String, Object> SetHandleReconciliationAssignmentData(Boolean isResetBusinessProcess, String statusCode, Int32? statusId, Int32 applicantComplianceItemnId, Int32 complianceItemId, ApplicantComplianceItemData applicantItemData, Boolean notReviewed)
        {
            Int32 queueId = 0;
            String queueCode;
            Dictionary<String, Object> dicHandleAssignmentData = new Dictionary<String, Object>();
            Dictionary<String, Object> dicQueueFields = new Dictionary<String, Object>();
            String queueFieldsXML = String.Empty;


            //Get the queueid on the basis status code
            if (statusCode == ApplicantItemComplianceStatus.Pending_Review.GetStringValue())
            {
                queueCode = QueueMetaDataType.Verification_Queue_For_Admin.GetStringValue();
                queueId = LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(View.TenantID).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == queueCode).FirstOrDefault().QMD_QueueID;
            }
            else if (statusCode == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue())
            {
                queueCode = QueueMetaDataType.Verification_Queue_For_ClientAdmin.GetStringValue();
                queueId = LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(View.TenantID).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == queueCode).FirstOrDefault().QMD_QueueID;
            }

            if (queueId > 0)
            {
                //Create dictionary for QueueFields.

                dicQueueFields.Add("ResetReviewProcess", false);

                //Get XML for queueFields.
                queueFieldsXML = "<Queues>" + QueueManagementManager.GetQueueFieldXMLString(dicQueueFields, queueId, applicantComplianceItemnId, notReviewed) + "</Queues>";

                //Create dictionary for handle assignment data.
                dicHandleAssignmentData.Add("CurrentLoggedInUserId", View.OrgUsrID); //View.CurrentLoggedInUserId UAT 1261
                dicHandleAssignmentData.Add("TenantId", View.TenantID);
                dicHandleAssignmentData.Add("QueueRecordXML", queueFieldsXML);
                return dicHandleAssignmentData;
            }
            return null;
        }

        #region Changes related to short series 'usp_Rule_EvaluateAdjustItemSeriesRules' SP.
        /// <summary>
        /// Method to evaluate Item Series Rules SP.
        /// </summary>
        /// <param name="tenantId">tenant id</param>
        /// <param name="dictCategoryData">Combination of Category Id and its Items(includes the series item Id also)</param>
        /// <param name="packageSubscriptionId">Package Subscription Id</param>
        /// <param name="systemUserId">Syatem User Id</param>
        /// <returns></returns>
        public void EvaluateAdjustItemSeriesRules()
        {
            ComplianceDataManager.EvaluateAdjustItemSeriesRules(View.TenantID, View.DicCategoryDataForItemSeriesRule, View.PackageSubscriptionId, View.CurrentLoggedInUserId);
        }
        #endregion

        #region UAT-1811

        /// <summary>
        /// Gets the value of large content for the given object id and assigns the value in the corresponding field.
        /// </summary>
        public String GetLargeContent(Int32 complianceCategoryId)
        {
            LargeContent notesRecord = ComplianceSetupManager.getLargeContentRecord(complianceCategoryId, LCObjectType.ComplianceCategory.GetStringValue(), LCContentType.ExplanatoryNotes.GetStringValue(), View.TenantID);
            if (notesRecord.IsNotNull())
                return notesRecord.LC_Content;
            return null;
        }


        #endregion

        #region UAT-2028:

        public Boolean IsAnyItemPendingForDataEntry(List<ComplianceItem> lstNotAllowedItems, List<ComplianceItem> lstAvailableItems)
        {
            lstAvailableItems = lstAvailableItems.IsNullOrEmpty() ? new List<ComplianceItem>() : lstAvailableItems;
            lstNotAllowedItems = lstNotAllowedItems.IsNullOrEmpty() ? new List<ComplianceItem>() : lstNotAllowedItems;

            if (lstAvailableItems.Any(cnd => !View.lstMappedItems.Contains(cnd.ComplianceItemID) && cnd.ComplianceItemID != 0) || !lstNotAllowedItems.IsNullOrEmpty())
            {
                return true;
            }
            return false;
        }
        #endregion

        #region UAT-2413
        public String GetEnterRequirementsText()
        {
            List<lkpSetting> settings = LookupManager.GetLookUpData<Entity.ClientEntity.lkpSetting>(View.TenantID).Where(cnd => cnd.IsDeleted == false).ToList();
            List<ClientSetting> clientSettings = ComplianceDataManager.GetClientSetting(View.TenantID);

            Int32 appEnterRequirement = settings.Where(cond => cond.Code == Setting.APPLICANT_DATA_ENTRY_ENTER_REQUIREMENT_TEXT.GetStringValue()).First().SettingID;
            ClientSetting clientSetting = clientSettings.Where(cond => cond.CS_SettingID == appEnterRequirement).FirstOrDefault();

            if (!clientSetting.IsNullOrEmpty() && !clientSetting.CS_SettingValue.IsNullOrEmpty())
            {
                return clientSetting.CS_SettingValue;
            }
            else
            {
                return AppConsts.ENTER_REQUIREMENT_TEXT;
            }
        }
        #endregion

        /// <summary>
        /// Gets applicant Name by applicant Id
        /// </summary>
        public string GetApplicantNameByApplicantId(Int32 ApplicantId, Int32 tenantID)
        {
            return ComplianceDataManager.GetApplicantNameByApplicantId(ApplicantId, tenantID);
        }

        #region UAT-3077
        public List<ItemPaymentContract> GetItemPaymentDetail(Int32 subscriptionID)
        {
            return ComplianceDataManager.GetItemPaymentDetail(subscriptionID, View.TenantID, false);
        }
        public Tuple<Boolean, Int32> CheckItemPayment(Int32 applicantComplianceItemID, Int32 compItemId)
        {
            if (compItemId > AppConsts.NONE)
            {
                return ComplianceDataManager.CheckItemPayment(View.TenantID, applicantComplianceItemID, compItemId, false);
            }
            return new Tuple<bool, int>(true, 0);
        }

        public ComplianceItem ItemDetails(Int32 itemId)
        {
            return ComplianceDataManager.GetDataEntryComplianceItem(itemId, View.TenantID);
        }
        public ItemSery ItemSeriesDetails(Int32 itemSeriesId)
        {
            return ComplianceDataManager.GetItemSeriesByID(View.TenantID, itemSeriesId);
        }
        #endregion

        /// <summary>
        /// 3106
        /// </summary>
        /// <param name="ClientSettingCode"></param>
        /// <returns></returns>
        public String GetClientSettingsByCode(String ClientSettingCode)
        {
            ClientSetting OptionalCategorySetting = ComplianceDataManager.GetClientSetting(View.TenantID, ClientSettingCode);
            if (!OptionalCategorySetting.IsNullOrEmpty())
            {
                return OptionalCategorySetting.CS_SettingValue;
            }
            else
            {
                return AppConsts.STR_ONE;
            }
        }


        /// <summary>
        /// UAT-3240
        /// </summary>
        /// <returns></returns>
        public void IsDisabledBothCategoryAndItemExceptionsForTenant(Int32 tenantId)
        {
            if (tenantId > AppConsts.NONE)
            {
                Entity.ClientEntity.ClientSetting clientsettings = ComplianceDataManager.GetClientSetting(tenantId, Setting.DISABLE_CATEGORY_AND_ITEM_EXCEPTIONS.GetStringValue());
                if (clientsettings.IsNotNull())
                {
                    View.IsDisabledBothCategoryAndItemExceptionsForTenant = (!String.IsNullOrEmpty(clientsettings.CS_SettingValue) && clientsettings.CS_SettingValue == AppConsts.STR_ONE) ? true : false;
                }
                else
                {
                    View.IsDisabledBothCategoryAndItemExceptionsForTenant = false;
                }
            }
            else
            {
                View.IsDisabledBothCategoryAndItemExceptionsForTenant = false;
            }
        }

        /// <summary>
        /// UAT 3299
        /// </summary>
        public void GetQuizConfigurationSetting()
        {
            String _key = "LockSubmittedQuiz";
            Entity.ClientEntity.AppConfiguration QuizSetting = ComplianceDataManager.GetAppConfiguration(_key, View.TenantID);
            if (QuizSetting.IsNullOrEmpty())
            {
                Entity.AppConfiguration MasterQuizSetting = SecurityManager.GetAppConfiguration(_key);
                View.QuizConfigSetting = MasterQuizSetting.AC_Value.ToString();
            }
            else
            {
                View.QuizConfigSetting = QuizSetting.AC_Value.ToString();
            }
        }

        private void SendItemDocNotificationToAgencyUser(List<Int32> approvedCategoryIDs)
        {
            String approvedCategoryIds = String.Join(",", approvedCategoryIDs);

            List<ItemDocNotificationRequestDataContract> lstItemDocNotificationData = new List<ItemDocNotificationRequestDataContract>();
            ItemDocNotificationRequestDataContract itemDocRequestData = new ItemDocNotificationRequestDataContract();

            itemDocRequestData.TenantID = View.TenantID;
            itemDocRequestData.CategoryIds = Convert.ToString(View.CategoryDataContract.ComplianceCategoryId);
            itemDocRequestData.ApplicantOrgUserID = View.ApplicantID;
            itemDocRequestData.ApprovedCategoryIds = approvedCategoryIds;
            itemDocRequestData.RequestTypeCode = lkpUseTypeEnum.COMPLIANCE.GetStringValue();
            itemDocRequestData.PackageSubscriptionID = View.CategoryDataContract.PackageSubscriptionId;
            itemDocRequestData.RPS_ID = null;
            itemDocRequestData.CurrentLoggedInUserID = View.CurrentLoggedInUserId;
            lstItemDocNotificationData.Add(itemDocRequestData);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
            Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
            dicParam.Add("CategoryData", lstItemDocNotificationData);
            ProfileSharingManager.RunParallelTaskItemDocNotificationOnCatApproval(dicParam, LoggerService, ExceptiomService);

            //ProfileSharingManager.SendItemDocNotificationOnCategoryApproval(View.TenantID, Convert.ToString(View.CategoryDataContract.ComplianceCategoryId)
            //                                                               , View.ApplicantID, approvedCategoryIds, lkpUseTypeEnum.COMPLIANCE.GetStringValue()
            //                                                               , View.CategoryDataContract.PackageSubscriptionId, null);
        }

        public Int32 GetApplicantComplianceAttributeData(Int32 applicantItemDataId, Int32 complianceAttributeID)
        {
            if (applicantItemDataId > AppConsts.NONE && applicantItemDataId > AppConsts.NONE)
            {
                return ComplianceDataManager.GetApplicantComplianceAttributeData(applicantItemDataId, complianceAttributeID, View.TenantID).ApplicantComplianceAttributeID;
            }
            return AppConsts.NONE;
        }

        public String GetApplicantDocumentByApplAttrDataID(Int32 ApplAttributeDataId)
        {
            if (ApplAttributeDataId > AppConsts.NONE)
            {
                return ComplianceDataManager.GetApplicantDocumentByApplicantAttrDataID(View.TenantID, ApplAttributeDataId).FileName;
            }
            return String.Empty;
        }

    }
}




