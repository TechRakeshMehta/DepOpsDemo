using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using System.IO;
using System.Web;
using CoreWeb.IntsofLoggerModel.Interface;
using CoreWeb.IntsofExceptionModel.Interface;
using INTSOF.ServiceUtil;
using Entity.ClientEntity;
using System.Linq;

#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

#region UserDefined

using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.ServiceUtil;
using CoreWeb.IntsofLoggerModel.Interface;
using CoreWeb.IntsofExceptionModel.Interface;
using INTSOF.Contracts;
using INTSOF.Utils.CommonPocoClasses;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceRuleEngine;
#endregion

#endregion
namespace CoreWeb.ComplianceOperations.Views
{
    public class UploadDocumentsPresenter : Presenter<IUploadDocumentsView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public UploadDocumentsPresenter([CreateNew] IComplianceOperationsController controller)
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
            GetDataEntryDocStatus();
        }

        public Boolean AddApplicantUploadedDocuments(String filePath, Boolean isPersonalDoc)
        {
            foreach (var item in View.ToSaveApplicantUploadedDocuments)
            {
                if (isPersonalDoc)
                {
                    item.DocumentType = ComplianceSetupManager.GetDocumentTypeIDByCode(DocumentType.PERSONAL_DOCUMENT.GetStringValue(), View.TenantID);
                }
                Int32 applicantDocumentId = ComplianceDataManager.AddApplicantDocument(item, View.TenantID);
                String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                String fileName = filePath + "UD_" + View.TenantID.ToString() + "_" + applicantDocumentId + "_" + date + Path.GetExtension(item.FileName);
                ComplianceDataManager.UpdateDocumentPath(fileName, item.DocumentPath, applicantDocumentId, View.TenantID, item.OrganizationUserID.Value);
                if (!item.DocItemAssociationForDataEntries.IsNullOrEmpty())
                {
                    List<DocItemAssociationForDataEntry> lstDocItemAssociationForDataEntry = item.DocItemAssociationForDataEntries.Where(cond => !cond.DAFD_IsDeleted).ToList();
                    String instituteUrl = WebSiteManager.GetInstitutionUrl(View.TenantID);
                    String instituteName = ClientSecurityManager.GetTenantName(View.TenantID);
                    List<ExceptionRejectionContract> lstExceptionRejectionContract = new List<ExceptionRejectionContract>();
                    foreach (DocItemAssociationForDataEntry docItemAssociationForDataEntry in lstDocItemAssociationForDataEntry)
                    {
                        if (docItemAssociationForDataEntry.lkpItemDocMappingType.IDMT_Code == ItemDocMappingType.CATEGORY_EXCEPTION.GetStringValue())
                        {
                            List<ExceptionDocumentMapping> exceptionMapping = new List<ExceptionDocumentMapping>();
                            exceptionMapping.Add(new ExceptionDocumentMapping
                            {
                                ApplicantDocumentID = applicantDocumentId,
                            });
                            ApplyExceptionOnDocMapping(docItemAssociationForDataEntry.DAFD_ComplianceCategoryId, exceptionMapping, applicantDocumentId
                                                       , instituteUrl, instituteName, ref lstExceptionRejectionContract);
                        }
                        else if (docItemAssociationForDataEntry.lkpItemDocMappingType.IDMT_Code == ItemDocMappingType.ITEM_DATA.GetStringValue())
                        {
                            //UAT-2618
                            UpdateIsDocumentAssociated(docItemAssociationForDataEntry.DAFD_ComplianceCategoryId);
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
            }
            return true;
            //ComplianceDataManager.AddApplicantUploadedDocuments(View.ToSaveApplicantUploadedDocuments, View.TenantID);
        }

        /// <summary>
        /// This method is used to call the Parallel Task for Pdf conversion and merging 
        /// </summary>
        public void CallParallelTaskPdfConversionMerging()
        {
            Dictionary<String, Object> conversionData = new Dictionary<String, Object>();
            //Use Poco class so that Entity will not get updated while running parallel tasks
            List<ApplicantDocumentPocoClass> lstApplicantDoc = new List<ApplicantDocumentPocoClass>();
            foreach (var doc in View.ToSaveApplicantUploadedDocuments)
            {
                //doc.lkpDocumentType.DMT_Code;
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

            Dictionary<String, Object> mergingData = new Dictionary<String, Object>();
            if (View.IsAdminScreen == true)
            {
                conversionData.Add("OrganizationUserId", View.FromAdminApplicantID);
                conversionData.Add("CurrentLoggedUserID", View.CurrentLoggedUserID);
                conversionData.Add("TenantID", View.TenantID);

                mergingData.Add("OrganizationUserId", View.FromAdminApplicantID);
                mergingData.Add("CurrentLoggedUserID", View.CurrentLoggedUserID);
                mergingData.Add("TenantID", View.TenantID);
            }
            else
            {
                conversionData.Add("OrganizationUserId", View.OrganiztionUserID);
                conversionData.Add("CurrentLoggedUserID", View.CurrentLoggedUserID);
                conversionData.Add("TenantID", View.TenantID);

                mergingData.Add("OrganizationUserId", View.OrganiztionUserID);
                mergingData.Add("CurrentLoggedUserID", View.CurrentLoggedUserID);
                mergingData.Add("TenantID", View.TenantID);
            }

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
            if (conversionData.IsNotNull() && conversionData.Count > 0 && mergingData.IsNotNull() && mergingData.Count > 0)
            {
                Business.RepoManagers.DocumentManager.RunParallelTaskPdfConversionMerging(ConvertDocumentsIntoPdf, conversionData, MergeDocIntoUnifiedPdf, mergingData, LoggerService, ExceptiomService);
            }
        }

        public void CallParallelTaskPdfConversionWithoutMerging()
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
                    appDoc.PdfDocPath = String.Empty;
                    appDoc.IsCompressed = false;
                    appDoc.Size = doc.Size;
                    lstApplicantDoc.Add(appDoc);
                }
                conversionData.Add("ApplicantUploadedDocuments", lstApplicantDoc);
            }
            else
            {
                conversionData.Add("ApplicantUploadedDocuments", null);
            }
            conversionData.Add("OrganizationUserId", View.OrganiztionUserID);
            conversionData.Add("CurrentLoggedUserID", View.CurrentLoggedUserID);
            conversionData.Add("TenantID", View.TenantID);

            Dictionary<String, Object> mergingData = null;

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            if (conversionData.IsNotNull() && conversionData.Count > 0)
            {
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
                    // Business.RepoManagers.DocumentManager.MergeApplicantDocument(OrganizationUserId, tenantId, CurrentLoggedUserID);
                    Business.RepoManagers.DocumentManager.AppendApplicantDocument(OrganizationUserId, tenantId, CurrentLoggedUserID);
                }
            }
        }

        /// <summary>
        /// To check if document is already uploaded
        /// </summary>
        /// <param name="documentName"></param>
        /// <param name="documentSize"></param>
        /// <param name="documentUploadedBytes"></param>
        /// <returns></returns>
        public String IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, byte[] documentUploadedBytes, Boolean isPersonalDoc, Boolean IsInstructorPreceptorDocumentScreen)
        {
            Int32 _organizationUserID = View.CurrentLoggedUserID;
            Int32 _tenantId = View.TenantID;


            if (!IsInstructorPreceptorDocumentScreen)
            {
                //UAT-2298 : Disallow duplicate document upload on manage documents screen.
                if (View.IsAdminScreen)
                {
                    _organizationUserID = View.FromAdminApplicantID;
                }

                if (ComplianceDataManager.IsDocumentAlreadyUploaded(documentName, documentSize, _organizationUserID, _tenantId, isPersonalDoc))
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
                    /*
                        1) once a document is uplaoded into personal documents, it can not be uploaded again to personal documents.
                        2) once a document is uploaded in personal documents, it can still be uploaded in compliance documents
                        3) once a document is uploaded in personal documents, it can still be uploaded in rotation documents.
                        4) Once a document is uploaded into rotation documents, it can still be uploaded into personal dcouments.
                        4) Once a document is uploaded into compliance documents, it can still be uploaded into personal dcouments.
                    */
                    var applicantDocument = applicantDocuments.FirstOrDefault(x => x.OriginalDocMD5Hash == md5Hash
                                        && (
                                        (isPersonalDoc && x.DocumentType.IsNotNull() && x.DocumentType == personalDocType)
                                        || (!isPersonalDoc && ((x.DocumentType.IsNull()) || (x.DocumentType != ReqFieldUploadDocType && x.DocumentType != personalDocType)))
                                        ));
                    if (applicantDocument.IsNotNull())
                    {
                        return applicantDocument.FileName;
                    }
                }
            }
            //UAT-3593
            else if (IsInstructorPreceptorDocumentScreen)
            {
                if (ComplianceDataManager.IsReqDocumentAlreadyUploaded(documentName, documentSize, _organizationUserID, _tenantId))
                {
                    return documentName;
                }
                //Compare original document MD5Hash
                List<ApplicantDocument> applicantDocuments = ComplianceDataManager.GetApplicantDocuments(_organizationUserID, _tenantId);
                String md5Hash = CommonFileManager.GetMd5Hash(documentUploadedBytes);

                if (applicantDocuments.IsNotNull())
                {
                    var applicantDocument = applicantDocuments.FirstOrDefault(x => x.OriginalDocMD5Hash == md5Hash);

                    if (applicantDocument.IsNotNull())
                    {
                        return applicantDocument.FileName;
                    }
                }
            }

            return null;
        }

        public void GetlkpItemDocMappingType()
        {
            View.lstItemDocMappingType = ComplianceDataManager.GetlkpItemDocMappingType(View.TenantID);
        }

        #region UAT-1049:Admin Data Entry
        /// <summary>
        /// Method that set the data entry documnet status id for new status.  
        /// </summary>
        private void GetDataEntryDocStatus()
        {
            if (View.TenantID > AppConsts.NONE)
            {
                String newDataEntryDocStatus = DataEntryDocumentStatus.NEW.GetStringValue();
                lkpDataEntryDocumentStatu tempDataEntryDocStatusNew = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDataEntryDocumentStatu>(View.TenantID)
                                                                               .FirstOrDefault(cnd => cnd.LDEDS_Code == newDataEntryDocStatus && cnd.LDEDS_IsDeleted == false);
                if (!tempDataEntryDocStatusNew.IsNullOrEmpty())
                    View.DataEntryDocNewStatusId = tempDataEntryDocStatusNew.LDEDS_ID;

                String completedDataEntryDocStatus = DataEntryDocumentStatus.COMPLETE.GetStringValue();
                lkpDataEntryDocumentStatu tempDataEntryDocStatusComplete = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDataEntryDocumentStatu>(View.TenantID)
                                                                               .FirstOrDefault(cnd => cnd.LDEDS_Code == completedDataEntryDocStatus && cnd.LDEDS_IsDeleted == false);
                if (!completedDataEntryDocStatus.IsNullOrEmpty())
                    View.DataEntryDocCompleteStatusId = tempDataEntryDocStatusComplete.LDEDS_ID;

            }
        }

        public void GetSubscribedPackagesItems()
        {
            View.lstSubcribedItems = ComplianceDataManager.GetSubscribedItems(View.TenantID, View.OrganiztionUserID);
        }

        public List<ClientSetting> GetClientSetting()
        {
            List<String> lstCodes = new List<String>();
            lstCodes.Add(Setting.APPLICANT_DOCUMENT_ASSOCIATION.GetStringValue());

            //UAT-2431
            lstCodes.Add(Setting.DOCUMENT_UPLOAD_CONFIRMATION_MESSAGE_TEXT.GetStringValue());

            return ComplianceDataManager.GetClientSettingsByCodes(View.TenantID, lstCodes);
        }
        #endregion

        public void ApplyExceptionOnDocMapping(int complianceCategoryId, List<ExceptionDocumentMapping> exceptionMapping, Int32 applicationDocumentId
                                               , String instituteUrl, String instituteName, ref List<ExceptionRejectionContract> lstExceptionRejectionContract)
        {
            List<CompliancePackageCategory> lstCompliancePackageCategory = ComplianceDataManager.GetPackageListByCategoryId(complianceCategoryId, View.TenantID);
            string exceptionReason = "Please refer to attached document for exception reason";
            Guid wholeCatGUID = new Guid(AppConsts.WHOLE_CATEGORY_GUID);
            ComplianceItem cmpItem = ComplianceDataManager.GetWholeCategoryItemID(View.TenantID, View.OrganiztionUserID, wholeCatGUID, complianceCategoryId);
            foreach (var compliancePackageCategory in lstCompliancePackageCategory)
            {
                PackageSubscription subscriptionDetail = ComplianceDataManager.GetSubscriptionDetail(compliancePackageCategory.CPC_PackageID, View.OrganiztionUserID, View.TenantID);
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
                        CreatedByID = View.OrganiztionUserID, //View.CurrentLoggedInUserId, UAT 1261
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

                    //UAt-1209: As an application admin, I should be able to enter a date range for when a category should be compliance required/not required. 
                    categoryStatusCode = CheckIfComplianceRequiredIsFalse(categoryStatusCode, compliancePackageCategory);
                    List<Int32> applicationDocumentIds = new List<Int32>();
                    applicationDocumentIds.Add(applicationDocumentId);

                    //Changes as per UAT-523 WB: Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
                    ComplianceSaveResponse complianceSaveResponse = ComplianceDataManager.SaveExceptionData(categoryData, applicantComplianceItemData, itemCompliancestatus, reviewerTypeCode,
                        categoryStatusCode, View.TenantID, exceptionMapping, applicationDocumentIds, true, categoryExceptionStatusCode, View.OrganiztionUserID);

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
                    ComplianceDataManager.UpdateIsDocAssociated(View.TenantID, subscriptionDetail.PackageSubscriptionID, true, View.OrganiztionUserID);
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
                //RuleObjectId = Convert.ToString(View.ComplianceCategoryId),
                RuleObjectId = Convert.ToString(complianceCategoryId),
                RuleObjectParentId = Convert.ToString(packageId)
            };

            ruleObjectMappingList.Add(ruleObjectMappingForPackage);
            ruleObjectMappingList.Add(ruleObjectMappingForCategory);

            RuleManager.evaluatePostSubmitRules(ruleObjectMappingList, View.OrganiztionUserID, View.OrganiztionUserID, View.TenantID);
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

        #region UAT-2618:
        private void UpdateIsDocumentAssociated(Int32 complianceCategoryId)
        {
            List<CompliancePackageCategory> lstCompliancePackageCategory = ComplianceDataManager.GetPackageListByCategoryId(complianceCategoryId, View.TenantID);
            foreach (var compliancePackageCategory in lstCompliancePackageCategory)
            {
                PackageSubscription subscriptionDetail = ComplianceDataManager.GetSubscriptionDetail(compliancePackageCategory.CPC_PackageID, View.OrganiztionUserID, View.TenantID);
                if (!subscriptionDetail.IsNullOrEmpty())
                {
                    //UAT-2618:
                    ComplianceDataManager.UpdateIsDocAssociated(View.TenantID, subscriptionDetail.PackageSubscriptionID, true, View.OrganiztionUserID);
                }
            }
        }
        #endregion


        #region UAT-3675
        /// <summary>
        /// Method for saving the location images
        /// </summary>
        /// <returns></returns>
        public Boolean SaveLocationImages(Int32 Locationid)
        {
            if (Locationid > AppConsts.NONE)
                return FingerPrintSetUpManager.SaveLocationImages(View.AddedLocationImagesData, Locationid, View.CurrentLoggedUserID);
            return false;
        }
        #endregion


        #region UAT-4270
        public Boolean SaveManualFingerPrintFile(ApplicantDocument appDocument, Int32 FingerprintAppointmentId, Int32 TenantId, Boolean IsAbiReviewUpload)
        {
            if (TenantId > AppConsts.NONE)
            {
                return FingerPrintDataManager.SaveManualFingerPrintFile(appDocument, FingerprintAppointmentId, TenantId, View.CurrentLoggedUserID, IsAbiReviewUpload);
            }
            return false;
        }
        #endregion

    }
}




