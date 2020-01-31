using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using INTSOF.Utils;
using Microsoft.Practices.ObjectBuilder;
//using INTSOF.SharedObjects;

using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceRuleEngine;
using INTSOF.UI.Contract.ComplianceManagement;
using System.IO;
using System.Web;
using INTSOF.ServiceUtil;
using CoreWeb.IntsofLoggerModel.Interface;
using CoreWeb.IntsofExceptionModel.Interface;
using INTSOF.ServiceUtil;
using INTSOF.Contracts;
using INTSOF.SharedObjects;
using INTSOF.Utils.CommonPocoClasses;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManageUploadDocumentPresenter : Presenter<IManageUploadDocumentView>
    {


        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetApplicantUploadedDocuments()
        {
            View.ApplicantUploadedDocuments = ComplianceDataManager.GetApplicantDocumentDetails(View.FromAdminApplicantID, View.IsAdminScreen ? View.FromAdminTenantID : View.TenantID);
        }

        public Boolean DeleteApplicantUploadedDocument()
        {
            Boolean canDocumentBeDeleted = false;
            //ApplicantDocument uploadedDocument = ComplianceDataManager.GetApplicantUploadedDocument(View.ApplicantUploadedDocumentID, View.TenantID);
            //var documentMapped = uploadedDocument.ApplicantComplianceDocumentMaps.Where(x => !x.IsDeleted);
            //var documentMappedWithException = uploadedDocument.ExceptionDocumentMappings.Where(x => !x.IsDeleted);
            //if ((documentMapped == null || documentMapped.Count() == 0) &&( documentMappedWithException == null || documentMappedWithException.Count() == 0))
            //{
            //    ComplianceDataManager.DeleteApplicantUploadedDocument(View.ApplicantUploadedDocumentID, View.TenantID);
            //    return true;
            //    // delete succesfully
            //}
            //else
            //{
            //    return false;
            //    // its mapped with item
            //}
            //check when operation performed from Admin side.
            canDocumentBeDeleted = SecurityManager.CheckDocumentDeletionAllowed(View.ApplicantUploadedDocumentID, View.IsAdminScreen ? View.FromAdminTenantID : View.TenantID);

            if (canDocumentBeDeleted)
            {
                //UAT-2214:- Check whether doc mapped with rotation item or not
                canDocumentBeDeleted = ApplicantRequirementManager.CanDeleteRqmtFieldUploadDoc(View.ApplicantUploadedDocumentID, View.IsAdminScreen ? View.FromAdminTenantID : View.TenantID);
               
            }

            if (canDocumentBeDeleted == true)
            {
                //UAT 1261
                if (ComplianceDataManager.DeleteApplicantUploadedDocument(View.ApplicantUploadedDocumentID, View.IsAdminScreen ? View.FromAdminTenantID : View.TenantID, View.OrgUsrID, View.FromAdminApplicantID))
                {
                    //UAT-2214
                    if (!View.IsRequirementFieldUploadDocument)
                    {
                        SecurityManager.DeleteDocumentFromFlatDataEntry(View.ApplicantUploadedDocumentID, View.IsAdminScreen ? View.FromAdminTenantID : View.TenantID,
                                                                        View.FromAdminApplicantID, View.OrgUsrID);

                        #region UAT-1564:delete a document from Unified also when deleting it from manage Document page
                        //CallParallelTaskDeleteAppDocFromUnifiedDoc();
                        DeleteApplicantDocFromUnifiedDoc();
                        #endregion
                    }
                }

                return true;
            }
            else
            {
                return false;
                // its mapped with item
            }
        }

        public Boolean UpdateApplicantUploadedDocument()
        {
            //Updation when operation performed from Admin Side Screen.
            ComplianceDataManager.UpdateApplicantUploadedDocument(View.ToUpdateUploadedDocument, View.IsAdminScreen ? View.FromAdminTenantID : View.TenantID);
            //UAT-2296 For schools where the document association at time of upload is turned on, edit document on manage documents screen should allow additional items/exceptions to be added to uploaded documents
            if (!View.ToUpdateUploadedDocument.DocItemAssociationForDataEntries.IsNullOrEmpty() && View.ToUpdateUploadedDocument.DocItemAssociationForDataEntries.Count > 0)
            {
                short InProgressDocumentStatus = View.lkpDataEntryDocumentStatus.FirstOrDefault(con => con.LDEDS_Code == DataEntryDocumentStatus.IN_PROGRESS.GetStringValue()).LDEDS_ID;
                SecurityManager.UpdateFlatDataEntryQueueRecord(View.ApplicantUploadedDocumentID, InProgressDocumentStatus,View.OrgUsrID,View.TenantID);

                List<DocItemAssociationForDataEntry> lstDocItemAssociationForDataEntry = View.ToUpdateUploadedDocument.DocItemAssociationForDataEntries.Where(cond => !cond.DAFD_IsDeleted).ToList();
                String instituteUrl = WebSiteManager.GetInstitutionUrl(View.TenantID);
                String instituteName = ClientSecurityManager.GetTenantName(View.TenantID);
                List<ExceptionRejectionContract> lstExceptionRejectionContract = new List<ExceptionRejectionContract>();
                foreach (DocItemAssociationForDataEntry docItemAssociationForDataEntry in lstDocItemAssociationForDataEntry)
                {
                    String IDMT_Code = View.lstItemDocMappingType.FirstOrDefault(con => con.IDMT_ID == docItemAssociationForDataEntry.DAFD_MappingType).IDMT_Code;
                    if (IDMT_Code == ItemDocMappingType.CATEGORY_EXCEPTION.GetStringValue())
                    {
                        List<ExceptionDocumentMapping> exceptionMapping = new List<ExceptionDocumentMapping>();
                        exceptionMapping.Add(new ExceptionDocumentMapping
                        {
                            ApplicantDocumentID = View.ApplicantUploadedDocumentID,
                        });
                        ApplyExceptionOnDocMapping(docItemAssociationForDataEntry.DAFD_ComplianceCategoryId, exceptionMapping, View.ApplicantUploadedDocumentID
                                                   , instituteUrl, instituteName, ref lstExceptionRejectionContract);
                    }
                }

                if (lstExceptionRejectionContract.IsNotNull() && lstExceptionRejectionContract.Count > 0)
                {
                    Dictionary<String, Object> mailData = new Dictionary<String, Object>();

                    mailData.Add("lstExceptionRejectionContract", lstExceptionRejectionContract);
                    mailData.Add("tenantId", View.TenantID);
                    mailData.Add("isCategory", true);

                    var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                    var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

                    ParallelTaskContext.PerformParallelTask(SendExceptionMail, mailData, LoggerService, ExceptiomService);
                }
            }

            return true;
        }

        public UnifiedPdfDocument GetPdfAsUnifiedDocument()
        {
            return DocumentManager.GetPdfAsUnifiedDocument(View.FromAdminTenantID, View.FromAdminApplicantID);
        }

        /// <summary>
        /// This method is used to call the Parallel Task for Pdf conversion and merging 
        /// </summary>
        //public void CallParallelTaskPdfConversionMerging()
        //{
        //    Dictionary<String, Object> conversionData = new Dictionary<String, Object>();

        //    conversionData.Add("ApplicantUploadedDocuments", null);
        //    conversionData.Add("CurrentLoggedUserID", View.CurrentUserID);
        //    conversionData.Add("TenantID", View.TenantID);

        //    Dictionary<String, Object> mergingData = new Dictionary<String, Object>();
        //    if (View.IsAdminScreen == true)
        //    {
        //        mergingData.Add("OrganizationUserId", View.FromAdminApplicantID);
        //        mergingData.Add("CurrentLoggedUserID", View.CurrentUserID);
        //        mergingData.Add("TenantID", View.FromAdminTenantID);
        //    }
        //    else
        //    {
        //        mergingData.Add("OrganizationUserId", View.CurrentUserID);
        //        mergingData.Add("CurrentLoggedUserID", View.CurrentUserID);
        //        mergingData.Add("TenantID", View.TenantID);
        //    }
        //    var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
        //    var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
        //    if (conversionData.IsNotNull() && conversionData.Count > 0 && mergingData.IsNotNull() && mergingData.Count > 0)
        //    {
        //        Business.RepoManagers.DocumentManager.RunParallelTaskPdfConversionMerging(ConvertDocumentsIntoPdf, conversionData, MergeDocIntoUnifiedPdf, mergingData, LoggerService, ExceptiomService);
        //    }
        //}

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

        ///// <summary>
        ///// This method is used to merge converted pdf documents into unified pdf document
        ///// </summary>
        ///// <param name="mergingData">mergingData (Data dictionary that conatins the organizationUserId,tenantId,CurrentLoggedUserIDk)</param>
        //private void MergeDocIntoUnifiedPdf(Dictionary<String, Object> mergingData)
        //{
        //    if (mergingData.IsNotNull() && mergingData.Count > 0)
        //    {
        //        Boolean isParallelTaskRunning = true;
        //        Int32 tenantId;
        //        Int32 CurrentLoggedUserID;
        //        Int32 OrganizationUserId;
        //        mergingData.TryGetValue("OrganizationUserId", out OrganizationUserId);
        //        mergingData.TryGetValue("TenantID", out tenantId);
        //        mergingData.TryGetValue("CurrentLoggedUserID", out CurrentLoggedUserID);

        //        List<lkpDocumentStatu> lkpDocumentStatus = Business.RepoManagers.DocumentManager.GetDocumentStatus(tenantId);
        //        //Get the document status id based on the Merging in progress statusc code
        //        Int32 documentStatusId = lkpDocumentStatus.Where(obj => obj.DMS_Code == DocumentStatus.MERGING_IN_PROGRESS.GetStringValue()).FirstOrDefault().DMS_ID;

        //        //Check status of the document for user who uploaded the document.Method return true if document status is Merging in progress  else false 
        //        isParallelTaskRunning = Business.RepoManagers.DocumentManager.IsMergingInProgress(OrganizationUserId, tenantId, documentStatusId);
        //        UnifiedPdfDocument unifiedPdfDocument = Business.RepoManagers.DocumentManager.GetAllMergingInProgressUnifiedDoc(OrganizationUserId, tenantId, documentStatusId);

        //        if (isParallelTaskRunning)
        //        {
        //            for (int i = 0; i < AppConsts.TWO; i++)
        //            {
        //                if (isParallelTaskRunning)
        //                {
        //                    if (i == AppConsts.TWO)
        //                    {
        //                        View.ErrorMessage = "Could not start fresh merging as pervious merging is still in progress for tenant: " + tenantId + "," + " appplicant: " + OrganizationUserId + " request is an abandon.";
        //                    }
        //                    else if (unifiedPdfDocument.IsNotNull())
        //                    {
        //                        Double totalTimeDiffernce = (DateTime.Now - unifiedPdfDocument.UPD_CreatedOn).TotalMinutes;
        //                        if (totalTimeDiffernce >= 10.0)
        //                        {
        //                            Business.RepoManagers.DocumentManager.MergeApplicantDocument(OrganizationUserId, tenantId, CurrentLoggedUserID);
        //                            break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //Wait for 1 seconds if document status is Merging in Progress
        //                        System.Threading.Thread.Sleep(1000);
        //                        isParallelTaskRunning = Business.RepoManagers.DocumentManager.IsMergingInProgress(OrganizationUserId, tenantId, documentStatusId);
        //                    }
        //                }
        //                else
        //                {
        //                    Business.RepoManagers.DocumentManager.MergeApplicantDocument(OrganizationUserId, tenantId, CurrentLoggedUserID);
        //                    break;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Business.RepoManagers.DocumentManager.MergeApplicantDocument(OrganizationUserId, tenantId, CurrentLoggedUserID);
        //        }

        //    }
        //}
        #region UAT-977: Additional work towards archive ability
        public void GetSubscriptionArchiveState()
        {
            if (!View.IsAdminScreen)
            {
                String archiveStateCode = ArchiveState.Archived.GetStringValue();
                String graduatedStateCode = SubscriptionState.Graduated.GetStringValue();
                String archiveGraduatedStateCode = SubscriptionState.Archived_Graduated.GetStringValue();
                List<Entity.ClientEntity.PackageSubscription> lstPackageSubscriptions = ComplianceDataManager.GetSubscribedPackagesForUser(View.TenantID, View.CurrentUserID);
                lstPackageSubscriptions = lstPackageSubscriptions.Where(cond => (cond.SubscriptionMobilityStatusID == null || (cond.lkpSubscriptionMobilityStatu != null && !cond.lkpSubscriptionMobilityStatu.Code.Equals("AAAB")))).ToList();
                View.IsActiveSubscription = lstPackageSubscriptions.Any(cnd => ((cnd.ArchiveStateID == null || (cnd.lkpArchiveState != null && !cnd.lkpArchiveState.AS_Code.Equals(archiveStateCode) &&
                                                                                                                !cnd.lkpArchiveState.AS_Code.Equals(graduatedStateCode) && !cnd.lkpArchiveState.AS_Code.Equals(archiveGraduatedStateCode)))
                                                                                                                 && cnd.ExpiryDate.Value.Date >= DateTime.Now.Date));
            }
        }


        public String GetUserNodePermission()
        {
            List<UserNodePermissionsContract> lstUserNodePermissionsContract = ComplianceSetupManager.GetUserNodePermissionForVerificationAndProfile(View.IsAdminScreen ? View.FromAdminTenantID : View.TenantID, View.CurrentUserID);
            var lstApplicantInstitutionHierarchyMapping = StoredProcedureManagers.GetApplicantInstitutionHierarchyMapping(View.IsAdminScreen ? View.FromAdminTenantID : View.TenantID, View.FromAdminApplicantID.ToString());
            List<Int32> lstLoggedInUserNodes = lstUserNodePermissionsContract.Select(col => col.DPM_ID).ToList();
            List<Int32> lstApplicantUserNodes = lstApplicantInstitutionHierarchyMapping.Select(col => col.DPM_ID).ToList();
            List<Int32> lstMatchingUserNodes = lstLoggedInUserNodes.Intersect(lstApplicantUserNodes).ToList();
            if (!lstMatchingUserNodes.IsNullOrEmpty())
            {
                Int32 profilePermissionIdForMinimumPermission = Convert.ToInt32(lstUserNodePermissionsContract.Where(cond => lstMatchingUserNodes.Contains(cond.DPM_ID)).Max(col => col.ProfilePermissionID));
                return lstUserNodePermissionsContract.Where(cond => cond.ProfilePermissionID == profilePermissionIdForMinimumPermission).Select(col => col.ProfilePermissionCode).FirstOrDefault();
            }
            else if (lstUserNodePermissionsContract.Any(cond => cond.ParentNodeID == null))
            {
                return lstUserNodePermissionsContract.Where(cond => cond.ParentNodeID == null).Select(col => col.ProfilePermissionCode).FirstOrDefault();
            }
            return LkpPermission.FullAccess.GetStringValue();
        }

        #endregion

        #region UAT-1544:WB: Create checkboxes for each document and a Print Button so that for each checked document, the student could print those documents.

        public String ConvertDocumentToPdfForPrint()
        {
            if (!View.DocumentIdsToPrint.IsNullOrEmpty())
            {
                return DocumentManager.ConvertDocumentToPDFForPrint(View.IsAdminScreen ? View.FromAdminTenantID : View.TenantID, View.DocumentIdsToPrint);
            }
            return String.Empty;
        }
        #endregion


        #region UAT-1564:delete a document from Unified also when deleting it from manage Document page

        ///// <summary>
        ///// This method is used to call the Parallel Task for deletion of applicant document from unified pdf document.
        ///// </summary>
        //public void CallParallelTaskDeleteAppDocFromUnifiedDoc()
        //{
        //    if (!View.ApplicantUploadedDocumentID.IsNullOrEmpty())
        //    {
        //        Dictionary<String, Object> deletionData = new Dictionary<String, Object>();
        //        if (View.IsAdminScreen == true)
        //        {
        //            deletionData.Add("OrganizationUserId", View.FromAdminApplicantID);
        //            deletionData.Add("CurrentLoggedUserID", View.CurrentUserID);
        //            deletionData.Add("TenantID", View.FromAdminTenantID);
        //        }
        //        else
        //        {
        //            deletionData.Add("OrganizationUserId", View.CurrentUserID);
        //            deletionData.Add("CurrentLoggedUserID", View.CurrentUserID);
        //            deletionData.Add("TenantID", View.TenantID);
        //        }
        //        deletionData.Add("ApplicantDocumentToDeleteID", View.ApplicantUploadedDocumentID);

        //        var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
        //        var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
        //        if (deletionData.IsNotNull() && deletionData.Count > 0)
        //        {
        //            Business.RepoManagers.DocumentManager.RunParallelTaskPdfDocumentDeletion(DeleteApplicantDocFromUnifiedDoc, deletionData, LoggerService, ExceptiomService);
        //        }
        //    }
        //}

        //public void DeleteApplicantDocFromUnifiedDoc(Dictionary<String, Object> deletionData)
        //{

        //    if (deletionData.IsNotNull() && deletionData.Count > 0)
        //    {
        //        Boolean isParallelTaskRunning = true;
        //        Int32 tenantId;
        //        Double totalTimeDiffernce = 0.0;
        //        Int32 CurrentLoggedUserID;
        //        Int32 OrganizationUserId;
        //        Int32 appDocumentIdToDelete;
        //        deletionData.TryGetValue("OrganizationUserId", out OrganizationUserId);
        //        deletionData.TryGetValue("TenantID", out tenantId);
        //        deletionData.TryGetValue("CurrentLoggedUserID", out CurrentLoggedUserID);
        //        deletionData.TryGetValue("ApplicantDocumentToDeleteID", out appDocumentIdToDelete);

        //        if (!appDocumentIdToDelete.IsNullOrEmpty())
        //        {
        //            List<lkpDocumentStatu> lkpDocumentStatus = Business.RepoManagers.DocumentManager.GetDocumentStatus(tenantId);
        //            //Get the document status id based on the Merging in progress statusc code
        //            Int32 documentStatusId = lkpDocumentStatus.Where(obj => obj.DMS_Code == DocumentStatus.MERGING_IN_PROGRESS.GetStringValue()).FirstOrDefault().DMS_ID;

        //            //Check status of the document for user who uploaded the document.Method return true if document status is Merging in progress  else false 
        //            isParallelTaskRunning = Business.RepoManagers.DocumentManager.IsMergingInProgress(OrganizationUserId, tenantId, documentStatusId);
        //            //UnifiedPdfDocument unifiedPdfDocument = Business.RepoManagers.DocumentManager.GetAllMergingInProgressUnifiedDoc(OrganizationUserId, tenantId, documentStatusId);

        //            if (isParallelTaskRunning)
        //            {
        //                for (int i = 0; i <= AppConsts.TEN; i++)
        //                {
        //                    if (isParallelTaskRunning)
        //                    {
        //                        //Wait for 10 seconds if document status is Merging in Progress
        //                        System.Threading.Thread.Sleep(10000);
        //                        isParallelTaskRunning = Business.RepoManagers.DocumentManager.IsMergingInProgress(OrganizationUserId, tenantId, documentStatusId);
        //                    }
        //                    else
        //                    {
        //                        Business.RepoManagers.DocumentManager.DeleteApplicantDocFromUnifiedDoc(OrganizationUserId, tenantId, CurrentLoggedUserID, appDocumentIdToDelete, true);
        //                        break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Business.RepoManagers.DocumentManager.DeleteApplicantDocFromUnifiedDoc(OrganizationUserId, tenantId, CurrentLoggedUserID, appDocumentIdToDelete, false);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// This method is used to call the Parallel Task for deletion of applicant document from unified pdf document.
        /// </summary>
        public void DeleteApplicantDocFromUnifiedDoc()
        {
            if (!View.ApplicantUploadedDocumentID.IsNullOrEmpty())
            {
                Int32 applicantID = 0;
                Int32 tenantId = 0;
                if (View.IsAdminScreen == true)
                {
                    applicantID = View.FromAdminApplicantID;
                    tenantId = View.FromAdminTenantID;
                }
                else
                {
                    applicantID = View.CurrentUserID;
                    tenantId = View.TenantID;
                }
                Business.RepoManagers.DocumentManager.DeleteApplicantDocFromUnifiedDoc(applicantID, tenantId, View.OrgUsrID, View.ApplicantUploadedDocumentID, false);
            }
        }
        #endregion

        #region UAT-2296 For schools where the document association at time of upload is turned on, edit document on manage documents screen should allow additional items/exceptions to be added to uploaded documents

        public List<ClientSetting> GetClientSetting()
        {
            List<String> lstCodes = new List<String>();
            lstCodes.Add(Setting.APPLICANT_DOCUMENT_ASSOCIATION.GetStringValue());

            return ComplianceDataManager.GetClientSettingsByCodes(View.TenantID, lstCodes);
        }

        public void GetSubscribedPackagesItems()
        {
            View.lstSubcribedItems = ComplianceDataManager.GetSubscribedItems(View.TenantID, View.CurrentUserID);
        }

        public void GetlkpItemDocMappingType()
        {
            View.lstItemDocMappingType = ComplianceDataManager.GetlkpItemDocMappingType(View.TenantID);
        }
        
        /// <summary>
        /// UAT-2296 : Get the Document status for changing document status to pending review after updation.
        /// </summary>
        public void GetlkpDataEntryDocumentStatus()
        {
            View.lkpDataEntryDocumentStatus = ComplianceDataManager.GetlkpDataEntryDocumentStatus(View.TenantID);
        }

        public void ApplyExceptionOnDocMapping(int complianceCategoryId, List<ExceptionDocumentMapping> exceptionMapping, Int32 applicationDocumentId
                                         , String instituteUrl, String instituteName, ref List<ExceptionRejectionContract> lstExceptionRejectionContract)
        {
            List<CompliancePackageCategory> lstCompliancePackageCategory = ComplianceDataManager.GetPackageListByCategoryId(complianceCategoryId, View.TenantID);
            string exceptionReason = "Please refer to attached document for exception reason";
            Guid wholeCatGUID = new Guid(AppConsts.WHOLE_CATEGORY_GUID);
            ComplianceItem cmpItem = ComplianceDataManager.GetWholeCategoryItemID(View.TenantID, View.CurrentUserID, wholeCatGUID, complianceCategoryId);
            foreach (var compliancePackageCategory in lstCompliancePackageCategory)
            {
                PackageSubscription subscriptionDetail = ComplianceDataManager.GetSubscriptionDetail(compliancePackageCategory.CPC_PackageID, View.CurrentUserID, View.TenantID);
                if (!subscriptionDetail.IsNullOrEmpty())
                {
                    ApplicantComplianceCategoryData existingCategoryData = subscriptionDetail.ApplicantComplianceCategoryDatas.FirstOrDefault(sel => sel.ComplianceCategoryID == complianceCategoryId
                                                                                                                                   && !sel.IsDeleted);

                    Boolean ifExceptionIsAlrdyApplied = false;
                    if (!existingCategoryData.IsNullOrEmpty() && existingCategoryData.CategoryExceptionStatusID.IsNotNull())
                        ifExceptionIsAlrdyApplied = true;
                    int complianceItemId = cmpItem.ComplianceItemID;

                    //inserting Into Compliance Category Data
                    var categoryData = new ApplicantComplianceCategoryData
                    {

                        PackageSubscriptionID = subscriptionDetail.PackageSubscriptionID,
                        ComplianceCategoryID = complianceCategoryId
                    };

                    //inserting Into Applicance Compliance Item Data
                    var applicantComplianceItemData = new ApplicantComplianceItemData
                    {
                        ApplicantComplianceCategoryID = complianceCategoryId,
                        ComplianceItemID = complianceItemId,
                        CreatedByID = View.OrgUsrID, //View.CurrentLoggedInUserId, UAT 1261
                        ExceptionReason = exceptionReason
                    };

                    List<ListItemAssignmentProperties> lstItemAssignmentProperties = ComplianceSetupManager.GetAssignmentPropertiesByCategoryId(compliancePackageCategory.CPC_PackageID, complianceCategoryId, View.TenantID);
                    applicantComplianceItemData = ComplianceDataManager.SetItemReviewerTypeProperties(applicantComplianceItemData, lstItemAssignmentProperties);
                    var itemCompliancestatus = ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue();
                    String categoryStatusCode = ApplicantCategoryComplianceStatus.Incomplete.GetStringValue();
                    String categoryExceptionStatusCode = ApplicantCategoryExceptionStatus.EXCEPTION_APPLIED.GetStringValue();
                    var currentItemAssignmentProperties = lstItemAssignmentProperties.Where(obj => obj.ComplianceItemId == applicantComplianceItemData.ComplianceItemID).ToList();
                    if (currentItemAssignmentProperties.IsNotNull() && currentItemAssignmentProperties.Any(x => x.ApprovalRequired == false))
                    {
                        itemCompliancestatus = ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue();
                        categoryStatusCode = ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue();
                        categoryExceptionStatusCode = ApplicantCategoryExceptionStatus.EXCEPTION_APPROVED.GetStringValue();
                    }
                    var reviewerTypeCode = LkpReviewerType.Admin;

                    //As an application admin, I should be able to enter a date range for when a category should be compliance required/not required. 
                    categoryStatusCode = CheckIfComplianceRequiredIsFalse(categoryStatusCode, compliancePackageCategory);
                    List<Int32> applicationDocumentIds = new List<Int32>();
                    applicationDocumentIds.Add(applicationDocumentId);

                    // Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
                    ComplianceSaveResponse complianceSaveResponse = ComplianceDataManager.SaveExceptionData(categoryData, applicantComplianceItemData, itemCompliancestatus, reviewerTypeCode,
                        categoryStatusCode, View.TenantID, exceptionMapping, applicationDocumentIds, true, categoryExceptionStatusCode, View.CurrentUserID);

                    EvaluatePostSubmitRules(complianceCategoryId, compliancePackageCategory.CPC_PackageID, subscriptionDetail);



                    ////Call Parallel task handle assignment method
                    Dictionary<String, Object> dicHandleAssignmentData = SetHandleAssignmentData(false, itemCompliancestatus, null
                                                                        , complianceSaveResponse.ItemData.ApplicantComplianceItemID, applicantComplianceItemData.ComplianceItemID,
                                                                            complianceSaveResponse.ItemData, false, subscriptionDetail, complianceCategoryId);
                    var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                    var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                    QueueManagementManager.RunParallelTaskHandleAssignment(dicHandleAssignmentData, LoggerService, ExceptiomService, View.TenantID);
                    SetQueueImaging();

                    if (!ifExceptionIsAlrdyApplied)
                    {
                        ExceptionRejectionContract excRejContract = new ExceptionRejectionContract();
                        excRejContract.ApplicantName = subscriptionDetail.OrganizationUser.FirstName + ' ' + subscriptionDetail.OrganizationUser.LastName;
                        excRejContract.ApplicationUrl = instituteUrl;
                        excRejContract.UserFullName = "Administrative User";

                        excRejContract.PackageName = subscriptionDetail.CompliancePackage.PackageLabel.IsNullOrEmpty() ?
                                                    subscriptionDetail.CompliancePackage.PackageName : subscriptionDetail.CompliancePackage.PackageLabel;
                        excRejContract.InstituteName = instituteName;
                        excRejContract.NodeHierarchy = ComplianceDataManager.GetNodeHiearchy(View.TenantID, subscriptionDetail.PackageSubscriptionID);
                        excRejContract.CategoryName = compliancePackageCategory.ComplianceCategory.CategoryLabel.IsNullOrEmpty() ?
                                                         compliancePackageCategory.ComplianceCategory.CategoryName : compliancePackageCategory.ComplianceCategory.CategoryLabel;
                        excRejContract.HierarchyNodeID = subscriptionDetail.Order.SelectedNodeID.HasValue ? subscriptionDetail.Order.SelectedNodeID.Value : 0;
                        lstExceptionRejectionContract.Add(excRejContract);
                    }
                    //UAT-2618:
                    ComplianceDataManager.UpdateIsDocAssociated(View.TenantID, subscriptionDetail.PackageSubscriptionID, true, View.CurrentUserID);
                }
            }
        }

        private string CheckIfComplianceRequiredIsFalse(String categoryStatusCode, CompliancePackageCategory compPackageCategory)
        {
            //UAt-1209: As an application admin, I should be able to enter a date range for when a category should be compliance required/not required. 
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

        public void EvaluatePostSubmitRules(Int32 complianceCategoryId, Int32 packageId, PackageSubscription subscriptionDetail)
        {

            List<RuleObjectMapping> ruleObjectMappingList = new List<RuleObjectMapping>();
            RuleObjectMapping ruleObjectMappingForPackage = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Package.GetStringValue(), View.TenantID).OT_ID),
                RuleObjectId = Convert.ToString(packageId),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RuleObjectMapping ruleObjectMappingForCategory = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Category.GetStringValue(), View.TenantID).OT_ID),
                RuleObjectId = Convert.ToString(complianceCategoryId),
                RuleObjectParentId = Convert.ToString(packageId)
            };

            ruleObjectMappingList.Add(ruleObjectMappingForPackage);
            ruleObjectMappingList.Add(ruleObjectMappingForCategory);

            RuleManager.evaluatePostSubmitRules(ruleObjectMappingList, View.CurrentUserID, View.OrgUsrID, View.TenantID);
            String tenantName = String.Empty;
            Entity.Tenant tenant = SecurityManager.GetTenant(View.TenantID);
            if (!tenant.IsNullOrEmpty())
                tenantName = tenant.TenantName;

            ComplianceDataManager.SendMailOnComplianceStatusChange(View.TenantID, tenantName, subscriptionDetail.lkpPackageComplianceStatu.Code,
                                                                   subscriptionDetail.lkpPackageComplianceStatu.PackageComplianceStatusID
                                                                   , subscriptionDetail.PackageSubscriptionID
                                                                   , subscriptionDetail.Order.SelectedNodeID.Value);

        }

        public Dictionary<String, Object> SetHandleAssignmentData(Boolean isResetBusinessProcess, String statusCode, Int32? statusId,
                                                               Int32 applicantComplianceItemnId, Int32 complianceItemId,
                                                               ApplicantComplianceItemData applicantItemData, Boolean notReviewed
                                                              , PackageSubscription subscriptionDetail, Int32 complianceCategoryId)
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
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ApplicantName, subscriptionDetail.OrganizationUser.FirstName);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ComplianceItemId, complianceItemId);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.CategoryId, complianceCategoryId);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.PackageID, subscriptionDetail.CompliancePackageID);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.SubmissionDate, applicantItemData.SubmissionDate);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.System_Status, String.Empty);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Rush_Order_Status, rushOrderStatusCode);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ApplicantComplianceItemID, applicantComplianceItemnId);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.HierarchyNodeID, subscriptionDetail.Order.SelectedNodeID);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ApplicantId, subscriptionDetail.OrganizationUser.OrganizationUserID);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Item_Name, complianceItemName);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Category_Name, complianceCategoryName);
                dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Package_Name, subscriptionDetail.CompliancePackage.PackageName);
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
                dicHandleAssignmentData.Add("CurrentLoggedInUserId", subscriptionDetail.OrganizationUser.OrganizationUserID); //View.CurrentLoggedInUserId UAT 1261
                dicHandleAssignmentData.Add("TenantId", View.TenantID);
                dicHandleAssignmentData.Add("QueueRecordXML", queueFieldsXML);
                return dicHandleAssignmentData;
            }
            return null;
        }

        private void SetQueueImaging()
        {

            Dictionary<String, Object> dataDictionary = new Dictionary<String, Object>();
            dataDictionary.Add("TenantID", View.TenantID);
            QueueImagingManager.AsignQueueImagingRepoInstance(dataDictionary);
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

        #endregion

        #region UAT-4687
        /// <summary>
        /// UAT-4687: "Upload Documents" should display for student accounts rather than "View Documents" whenever applicant has Clinical Rotations Tab active on their screen
        /// </summary>
        public void GetApplicantClinicalRotationMember()
        {
            View.IsApplicantClinicalRotationMember = ApplicantClinicalRotationManager.IsApplicantClinicalRotationMember(View.TenantID, View.CurrentUserID);
        }
        #endregion
    }
}



