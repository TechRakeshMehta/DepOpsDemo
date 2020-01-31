using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using System.Linq;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System.Data;
using System.Web;
using INTSOF.ServiceUtil;
using CoreWeb.IntsofLoggerModel.Interface;
using CoreWeb.IntsofExceptionModel.Interface;
using INTSOF.Contracts;
using INTSOF.Utils.CommonPocoClasses;
using System.Configuration;

namespace CoreWeb.ComplianceOperations.Views
{
    public class VerificationDetailsUnassignedDocumentControlPresenter  : Presenter<IVerificationDetailsUnassignedDocumentConrolView >
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
            GetDataEntryDocStatus();
        }

        public override void OnViewInitialized()
        {

            // TODO: Implement code that will be executed the first time the view loads

        }

        public void GetApplicantDocuments()
        {
            //if (View.OrganizationUserData != null)
            if (!View.ApplicantId.IsNullOrEmpty() && View.ApplicantId > AppConsts.NONE)
            {
                List<ApplicantDocuments> applicantDocuments = ComplianceDataManager.GetApplicantDocumentsData(View.ApplicantId, View.SelectedTenantId_Global);
                if (applicantDocuments.IsNotNull())
                {
                    View.lstApplicantDocument = ComplianceDataManager.GetDocumentRelatedToUserExceptEsigned(applicantDocuments, View.SelectedTenantId_Global);
                }
            }  // View.lstApplicantDocument = ComplianceDataManager.GetApplicantDocumentsData(View.ApplicantId, View.SelectedTenantId_Global);

        }

        /// <summary>
        /// Updates the Mapping lists after Assign/Un-Assign & Remove Documents so that Checkboxes can be refreshed
        /// </summary>
        public void UpdateMappingList()
        {
            if (!View.IsException)
            {
                View.lstApplicantComplianceDocumentMaps = new List<ApplicantComplianceDocumentMap>();
                View.lstApplicantComplianceDocumentMaps = StoredProcedureManagers.GetUpdatedDocumentItemMappings(View.ItemDataId, View.IsException, View.SelectedTenantId_Global);
            }
            else
            {
                View.lstExceptionDocumentDocumentMaps = new List<ExceptionDocumentMapping>();
                View.lstExceptionDocumentDocumentMaps = StoredProcedureManagers.GetUpdatedDocumentExceptionMappings(View.ItemDataId, View.IsException, View.SelectedTenantId_Global);
            }
        }

        /// <summary>
        /// Gets the specific data needed for the implementation of the page.
        /// </summary>
        public void getData()
        {
            if (View.ItemDataId != AppConsts.NONE) // Do not hit the database if Id is 0
            {
                ApplicantComplianceItemData applicantComplianceItemData = ComplianceDataManager.GetApplicantComplianceItemData(View.ItemDataId, View.SelectedTenantId_Global);
                if (applicantComplianceItemData != null)
                {
                    /* List<Int32?> organisationUserIds = new List<Int32?>();
                     organisationUserIds.Add(applicantComplianceItemData.ApplicantComplianceCategoryData.PackageSubscription.OrganizationUserID);
                 
                     * THIS IS BEING SET FROM ITEMDATAEDITMODE
                     * View.ApplicantId = applicantComplianceItemData.ApplicantComplianceCategoryData.PackageSubscription.OrganizationUserID.Value; */

                    View.ApplicantComplianceItemData = applicantComplianceItemData;
                    View.lstApplicantComplianceAttributeData = applicantComplianceItemData.ApplicantComplianceAttributeDatas.Where(x => !x.IsDeleted).ToList();

                    /* Fecthed using Stored procedure in Main screen.
                     * var applicantComplianceAttribute = View.lstApplicantComplianceAttributeData.Where(x => x.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower() && !x.IsDeleted).FirstOrDefault();

                    if (applicantComplianceAttribute == null)
                    {
                        View.lstApplicantComplianceDocumentMaps = null;
                    }
                    else
                    {
                        View.lstApplicantComplianceDocumentMaps = applicantComplianceAttribute.ApplicantComplianceDocumentMaps.Where(x => x.IsDeleted == false).ToList();
                    }

                    if (View.IsException)
                    {
                        View.lstExceptionDocumentDocumentMaps = applicantComplianceItemData.ExceptionDocumentMappings.Where(x => x.IsDeleted == false).ToList();
                    }*/
                }
            }
            /* else
             {
                 View.lstApplicantComplianceDocumentMaps = null;

                 if (View.IsException)
                 {
                     View.lstExceptionDocumentDocumentMaps = null;
                 }
             }*/
        }

        /// <summary>
        /// Get the "Screening Document" Attribute DocumentTypeID from lkpDocumentType
        /// </summary>
        public void GetScreeningDocumentTypeId()
        {
            var _screeningDocTypeCode = DocumentType.SCREENING_DOCUMENT_ATTRIBUTE_TYPE_DOCUMENT.GetStringValue();
            var _lstLkpDocumentTypes = LookupManager.GetLookUpData<lkpDocumentType>(View.SelectedTenantId_Global);
            View.ScreeningDocTypeId = _lstLkpDocumentTypes.First(dt => dt.DMT_Code == _screeningDocTypeCode).DMT_ID;
        }

        /// <summary>
        /// When the documents are being UPLOADED using BROWSE control.
        /// </summary>
        /// <param name="toAddDocumentMap"></param>
        /// <param name="ToAddDocumentMapException"></param>
        public void UpdateApplicantComplianceNewDocumentMaps(ApplicantComplianceDocumentMap toAddDocumentMap, ExceptionDocumentMapping toAddDocumentMapException)
        {
            // Handles the Case of All Items
            // 1. EXCEPT incomplete
            // 2. Data for other attributes has been already added but FileType attribute is being added for the first time
            // 3. Exception Documents are being UPLOADED 
            if (!View.IsIncompleteItem || !toAddDocumentMapException.IsNullOrEmpty())
                ComplianceDataManager.AddUpdateApplicantComplianceDocumentMappingData(toAddDocumentMap, toAddDocumentMapException
                    , View.ApplicantComplianceItemData.ApplicantComplianceItemID, View.ComplianceAttributeId, View.SelectedTenantId_Global);
            else // Case of incomplete items
            {
                Int32 _itemDataId = 0;
                DateTime _dtCreationDateTime = DateTime.Now;
                GenerateHierarchy(_dtCreationDateTime);
                ComplianceDataManager.AddIncompleteDocumentMapping(toAddDocumentMap, _categoryData, _itemData, _attributeData,
                    View.PackageSubscriptionId, View.ApplicantId, out _itemDataId, View.SelectedTenantId_Global);

                if (_itemDataId > AppConsts.NONE || (View.ItemDataId.IsNullOrEmpty() && View.ItemDataId == AppConsts.NONE))
                    View.ItemDataId = _itemDataId;
            }
        }

        /// <summary>
        /// Updates the new entries and deletes the old document mapping, for both Item and Exception case
        /// </summary>
        /// <param name="toAddDocumentMap">list to add ApplicantComplianceDocumentMap data</param>
        /// <param name="ToAddDocumentMapException">list to add ExceptionDocumentMapping data</param>
        /// <param name="ToDeleteApplicantComplianceDocumentMapIDs">list to delete the mapping.</param>
        /// <param name="IsException">Whether exception data or not.</param>
        /// <param name="CurrentUserId">Curent logged in user.</param>
        public void AssignUnAssignItemDocuments(List<ApplicantComplianceDocumentMap> toAddDocumentMap, List<ExceptionDocumentMapping> ToAddDocumentMapException, List<Int32> ToDeleteApplicantComplianceDocumentMapIDs, Boolean IsException, Int32 CurrentUserId)
        {
            ComplianceDataManager.AssignUnAssignItemDocuments(toAddDocumentMap, ToAddDocumentMapException, ToDeleteApplicantComplianceDocumentMapIDs, IsException, View.SelectedTenantId_Global, CurrentUserId, View.ItemDataId);
        }

        /// <summary>
        /// Assign/Un-Assign the documents for Incomplete Items. 
        /// Also handles the case when the only document has been assigned for an Item. In that case also, the Item Status remains as Incomplete.
        /// </summary>
        /// <param name="toAddDocumentMap"></param>
        /// <param name="toDeleteDocumentMap"></param>
        /// <param name="CurrentUserId"></param>
        public void AssignUnAssignIncompleteItemDocuments(List<ApplicantComplianceDocumentMap> toAddDocumentMap, List<Int32> toDeleteDocumentMap, Int32 CurrentUserId)
        {
            Int32 _itemDataId = 0;
            GenerateHierarchy(DateTime.Now);
            _attributeData.AttributeValue = Convert.ToString(toAddDocumentMap.Count()); // Override the count of document
            ComplianceDataManager.AssignUnAssignIncompleteItemDocuments(toAddDocumentMap, toDeleteDocumentMap, _categoryData, _itemData, _attributeData, View.PackageSubscriptionId, View.ApplicantId, out _itemDataId, CurrentUserId, View.SelectedTenantId_Global, View.lstAssignmentProperties);

            if (_itemDataId > AppConsts.NONE || (View.ItemDataId.IsNullOrEmpty() && View.ItemDataId == AppConsts.NONE)) // Handle cases when the user simply clicks assign without any change in checkboxes
                View.ItemDataId = _itemDataId;
        }

        /// <summary>
        /// Removes the mapping from the table specific to the map id
        /// </summary>
        /// <param name="applicantMappingId">Id of which apping isa to be removed.</param>
        /// <param name="curentUserId"></param>
        /// <param name="Isexception"></param>
        public void RemoveMapping(Int32 applicantMappingId, Int32 curentUserId, Boolean Isexception)
        {
            ComplianceDataManager.RemoveMapping(applicantMappingId, curentUserId, Isexception, View.SelectedTenantId_Global);
        }

        /// <summary>
        /// Gets the list of documen related to the specific item id.
        /// </summary>
        /// <returns></returns>
        public List<ApplicantDocuments> GetItemRelatedDocument()
        {
            if (!View.lstApplicantDocument.IsNullOrEmpty())
            {
                List<Int32> applicantComplianceItemIDs = GetApplicantComplianceItemIdList();

                if (applicantComplianceItemIDs.IsNotNull() && applicantComplianceItemIDs.Count > 0)
                {
                    List<ApplicantDocuments> assignedDocumentlist = View.lstApplicantDocument.Where(x => x.ItemID.HasValue && applicantComplianceItemIDs.Contains(x.ItemID.Value) && x.PackageId == View.PackageSubscriptionId).ToList();
                    return assignedDocumentlist;
                }
            }
            return new List<ApplicantDocuments>();
        }
        public List<Int32> GetApplicantComplianceItemIdList()
        {
            return ComplianceDataManager.GetApplicantComplianceItemIdList(View.PackageSubscriptionId, View.ComplianceCategoryId, View.SelectedTenantId_Global);
        }

        ///// <summary>
        ///// Gets the list of documen related to the specific item id.
        ///// </summary>
        ///// <returns></returns>
        //public List<ApplicantComplianceDocumentMap> GetDocumentsMapRelatedToAttribute()
        //{
        //    if (!View.ComplianceAttributeId.IsNullOrEmpty())
        //    {
        //        return ComplianceDataManager.GetDocumentsMapRelatedToAttribute(View.SelectedTenantId_Global, View.);
        //    }
        //    return new List<ApplicantComplianceDocumentMap>();
        //}

        public Boolean UpdateDocumentPath(String newFileName, String oldFileName, Int32 documentId, Int32 orgUserId)
        {
            return ComplianceDataManager.UpdateDocumentPath(newFileName, oldFileName, documentId, View.SelectedTenantId_Global, orgUserId);
        }

        /// <summary>
        /// This adds the new document loaded by the user and and return its id.
        /// </summary>
        /// <param name="applicantDocument"></param>
        /// <returns></returns>
        public Int32 AddApplicantDocument(ApplicantDocument applicantDocument)
        {
            Int32 applicantDocumentId = ComplianceDataManager.AddApplicantDocument(applicantDocument, View.SelectedTenantId_Global);
            return applicantDocumentId;
        }
        // TODO: Handle other view events and set state in the view

        public String ValidateDocumentMappingRules(Int32 newDocumentsToMap)
        {
            if (View.IsFileUploadApplicable)
            {
                #region COMMENTED CODE
                //if (!View.lstApplicantComplianceAttributeData.IsNullOrEmpty()) //Case when Attributes are already added by the applicant or ADMINs,
                //{
                //    var _fileUploadType = View.lstApplicantComplianceAttributeData.Where(att => att.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() ==
                //                                                 ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower()
                //                                                 && att.IsDeleted == false).FirstOrDefault();

                //    // Using this code will lead to Attribute value getting Updated, when SAVE Changes is fired in AddNewDocument()
                //    //// File upload already exists in the list of data already entered
                //    //if (!_fileUploadType.IsNullOrEmpty())
                //    //{
                //    //    _fileUploadType.AttributeValue = Convert.ToString(newDocumentsToMap);

                //    //    // To handle the case when validation passes, 
                //    //    //then further SaveChanges() will execute and update the
                //    //    //document count in AttributeValue column in ComplianceAttribute table
                //    //    _fileUploadType.ModifiedByID = View.CurrentLoggedInUserId;
                //    //    _fileUploadType.ModifiedOn = DateTime.Now;
                //    //}
                //    //else // File upload is getting added for the first time, then add to list for Validation
                //    //{
                //        //View.lstApplicantComplianceAttributeData.Add(new ApplicantComplianceAttributeData
                //        //{
                //        //    ComplianceAttributeID = View.ComplianceAttributeId,
                //        //    AttributeValue = Convert.ToString(newDocumentsToMap),
                //        //    CreatedByID = View.CurrentLoggedInUserId,
                //        //    CreatedOn = DateTime.Now,
                //        //    ApplicantComplianceItemID = View.lstApplicantComplianceAttributeData.FirstOrDefault().ApplicantComplianceItemID,
                //        //    IsDeleted = false
                //        //});
                //    //}
                //}
                //else // Case when admin is assigning new documents for an Item, for which NO attribute data exists.
                //{
                //    List<ApplicantComplianceAttributeData> _lstAttributeData = new List<ApplicantComplianceAttributeData>();
                //    _lstAttributeData.Add(
                //        new ApplicantComplianceAttributeData { ComplianceAttributeID = View.ComplianceAttributeId, AttributeValue = Convert.ToString(newDocumentsToMap) }
                //        );

                //    ApplicantComplianceItemData _itemData = new ApplicantComplianceItemData { ComplianceItemID = View.ComplianceItemId };

                //    return ComplianceDataManager.ValidateDocumentMappingRules(
                //      View.ApplicantId,
                //      View.CompliancePackageId,
                //     _lstAttributeData,
                //     _itemData,
                //      View.ComplianceCategoryId,
                //      View.PackageSubscriptionId,
                //      true,
                //      View.SelectedTenantId_Global);
                //}
                //return ComplianceDataManager.ValidateDocumentMappingRules(View.ApplicantId, View.ApplicantComplianceItemData.ApplicantComplianceCategoryData.PackageSubscription.CompliancePackageID,
                //      View.lstApplicantComplianceAttributeData, View.ApplicantComplianceItemData, View.ApplicantComplianceItemData.ApplicantComplianceCategoryData.ComplianceCategoryID, View.PackageSubscriptionId, true, View.SelectedTenantId_Global);

                #endregion

                List<ApplicantComplianceAttributeData> _lstAttributeData = new List<ApplicantComplianceAttributeData>();
                _lstAttributeData.Add(
                    new ApplicantComplianceAttributeData { ComplianceAttributeID = View.ComplianceAttributeId, AttributeValue = Convert.ToString(newDocumentsToMap) }
                    );

                ApplicantComplianceItemData _itemData = new ApplicantComplianceItemData { ComplianceItemID = View.ComplianceItemId };

                return ComplianceDataManager.ValidateDocumentMappingRules(
                  View.ApplicantId,
                  View.CompliancePackageId,
                 _lstAttributeData,
                 _itemData,
                  View.ComplianceCategoryId,
                  View.PackageSubscriptionId,
                  true,
                  View.SelectedTenantId_Global);
            }
            return String.Empty;
        }

        public Boolean ItemHasFileAttribute()
        {
            ComplianceItemAttribute _complianceItemAttribute = ComplianceDataManager.ItemHasFileAttribute(View.ComplianceItemId, View.SelectedTenantId_Global);
            View.IsFileUploadApplicable = _complianceItemAttribute.IsNullOrEmpty() ? false : true;
            View.ComplianceAttributeId = _complianceItemAttribute.IsNullOrEmpty() ? AppConsts.NONE : _complianceItemAttribute.CIA_AttributeID;

            return View.IsFileUploadApplicable;
        }

        ApplicantComplianceItemData _itemData;
        ApplicantComplianceCategoryData _categoryData;
        ApplicantComplianceAttributeData _attributeData;

        private void GenerateHierarchy(DateTime dtCreationDateTime)
        {
            _categoryData = new ApplicantComplianceCategoryData
        {
            PackageSubscriptionID = View.PackageSubscriptionId,
            ComplianceCategoryID = View.ComplianceCategoryId,
            IsDeleted = false,
            CreatedByID = View.CurrentLoggedInUserId,
            CreatedOn = dtCreationDateTime
        };

            _itemData = new ApplicantComplianceItemData
           {
               ApplicantComplianceCategoryID = _categoryData.ApplicantComplianceCategoryID,
               ComplianceItemID = View.ComplianceItemId,
               IsDeleted = false,
               CreatedByID = View.CurrentLoggedInUserId,
               CreatedOn = dtCreationDateTime
           };

            _attributeData = new ApplicantComplianceAttributeData
          {
              ApplicantComplianceItemID = _itemData.ApplicantComplianceItemID,
              AttributeValue = "1",                                              // 1 Document is getting added on each call 
              ComplianceAttributeID = View.ComplianceAttributeId,
              IsDeleted = false,
              CreatedByID = View.CurrentLoggedInUserId,
              CreatedOn = dtCreationDateTime
          };
        }

        /// <summary>
        /// This method is used to call the Parallel Task for Pdf conversion and merging 
        /// </summary>
        public void CallParallelTaskPdfConversionMerging()
        {
            Dictionary<String, Object> conversionData = new Dictionary<String, Object>();
            //Use Poco class so that Entity will not get updated while running parallel tasks
            List<ApplicantDocumentPocoClass> lstApplicantDoc = new List<ApplicantDocumentPocoClass>();
            if (View.ToSaveApplicantUploadedDocuments.IsNotNull() && View.ToSaveApplicantUploadedDocuments.Count() > 0)
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
            conversionData.Add("CurrentLoggedUserID", View.CurrentLoggedUserID);
            conversionData.Add("TenantID", View.SelectedTenantId_Global);
            conversionData.Add("OrganizationUserId", View.ApplicantId);

            Dictionary<String, Object> mergingData = new Dictionary<String, Object>();
            mergingData.Add("OrganizationUserId", View.ApplicantId);
            mergingData.Add("CurrentLoggedUserID", View.CurrentLoggedUserID);
            mergingData.Add("TenantID", View.SelectedTenantId_Global);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
            if (conversionData.IsNotNull() && conversionData.Count > 0 && mergingData.IsNotNull() && mergingData.Count > 0)
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
                                // Business.RepoManagers.DocumentManager.MergeApplicantDocument(OrganizationUserId, tenantId, CurrentLoggedUserID);
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
                            // Business.RepoManagers.DocumentManager.MergeApplicantDocument(OrganizationUserId, tenantId, CurrentLoggedUserID);
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

        public Int32 GetDocumentStatusID()
        {
            List<lkpDocumentStatu> lkpDocumentStatus = Business.RepoManagers.DocumentManager.GetDocumentStatus(View.SelectedTenantId_Global);
            var mergingCompletedCode = DocumentStatus.MERGING_COMPLETED.GetStringValue();
            //Get the document status id based on Merging Completed status code
            return lkpDocumentStatus.Where(obj => obj.DMS_Code == mergingCompletedCode).FirstOrDefault().DMS_ID;
        }

        #region UAT-1049:Admin Data Entry
        /// <summary>
        /// Method that set the data entry documnet status id for new status.  
        /// </summary>
        private void GetDataEntryDocStatus()
        {
            if (View.SelectedTenantId_Global > AppConsts.NONE)
            {
                String newDataEntryDocStatus = DataEntryDocumentStatus.NEW.GetStringValue();
                lkpDataEntryDocumentStatu tempDataEntryDocStatus = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDataEntryDocumentStatu>(View.SelectedTenantId_Global)
                                                                               .FirstOrDefault(cnd => cnd.LDEDS_Code == newDataEntryDocStatus && cnd.LDEDS_IsDeleted == false);
                if (!tempDataEntryDocStatus.IsNullOrEmpty())
                    View.DataEntryDocNewStatusId = tempDataEntryDocStatus.LDEDS_ID;
            }
        }
        #endregion

        /// <summary>
        /// UAT 1740: Move 604 notification from the time of login to when an admin attempts for view an employment result report. 
        /// </summary>
        /// <returns></returns>
        public Boolean IsEDFormPreviouslyAccepted()
        {
            Double employmentDisclosureIntervalHours = AppConsts.NONE;
            if (!ConfigurationManager.AppSettings["EmploymentDisclosureIntervalHours"].IsNullOrEmpty())
            {
                employmentDisclosureIntervalHours = Convert.ToDouble(ConfigurationManager.AppSettings["EmploymentDisclosureIntervalHours"]);
            }
            return SecurityManager.IsEDFormPreviouslyAccepted(View.CurrentLoggedInUserId, employmentDisclosureIntervalHours);
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                // return View.Tenant.TenantID.Equals(SecurityManager.DefaultTenantID);
                return View.CurrentTenantId_Global.Equals(SecurityManager.DefaultTenantID);
            }
        }

        #region UAT-2618:
        public void UpdateIsDocumentAssociated()
        {
            //UAT-2618:
            ComplianceDataManager.UpdateIsDocAssociated(View.SelectedTenantId_Global, View.PackageSubscriptionId, true, View.CurrentLoggedInUserId);
        }
        #endregion
    }
}




