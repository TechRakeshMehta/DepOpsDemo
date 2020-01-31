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
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

namespace CoreWeb.ClinicalRotation.Views
{
    public class RequirementVerificationDetailsDocumentConrolPresenter : Presenter<IRequirementVerificationDetailsDocumentConrolView>
    {
        public Boolean IsDefaultTenant
        {
            get
            {
                return View.CurrentTenantId_Global.Equals(SecurityManager.DefaultTenantID);
            }
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {

            // TODO: Implement code that will be executed the first time the view loads

        }

        public void GetApplicantDocuments()
        {
            if (View.ApplicantId > 0 && View.SelectedTenantId_Global > 0)
            {
                View.lstApplicantDocument = ApplicantRequirementManager.GetApplicantDocument(View.ApplicantId, View.SelectedTenantId_Global);
            }
            else
            {
                View.lstApplicantDocument = new List<ApplicantDocumentContract>();
            }
        }

        /// <summary>
        /// Gets the specific data needed for the implementation of the page.
        /// </summary>
        public void GetData()
        {
            if (View.RequirementItemDataId > AppConsts.NONE && View.SelectedTenantId_Global > AppConsts.NONE)
            {
                ApplicantRequirementItemData applicantRequirementItemData = ApplicantRequirementManager.GetApplicantRequirementItemDataByID(View.SelectedTenantId_Global, View.RequirementItemDataId);
                if (applicantRequirementItemData != null)
                {
                    View.ApplicantRequirementItemData = applicantRequirementItemData;
                    View.lstApplicantRequirementFieldData = applicantRequirementItemData.ApplicantRequirementFieldDatas.Where(x => !x.ARFD_IsDeleted).ToList();
                }
            }
        }

        /// <summary>
        /// Get applicant documents by requirement package subscription id and category Id
        /// </summary>
        public void GetRequirementApplicantDocumentsByCategoryId()
        {
            if (View.SelectedTenantId_Global > AppConsts.NONE && View.RequirementPackageSubscriptionId > AppConsts.NONE && View.RequirementCategoryId > AppConsts.NONE)
            {
                List<ApplicantFieldDocumentMappingContract> _lstApplicantRequirementDocuments = RequirementVerificationManager.GetRequirementApplicantDocumentsByCategoryId(View.SelectedTenantId_Global, View.RequirementPackageSubscriptionId, View.RequirementCategoryId);
                if (!_lstApplicantRequirementDocuments.IsNullOrEmpty())
                {
                    View.lstApplicantRequirementDocuments = _lstApplicantRequirementDocuments;
                }
                else
                {
                    View.lstApplicantRequirementDocuments = new List<ApplicantFieldDocumentMappingContract>();
                }
            }
        }

        /// <summary>
        /// Updates the Mapping lists after Assign/Un-Assign & Remove Documents so that Checkboxes can be refreshed
        /// </summary>
        public void UpdateMappingList()
        {
            if (View.RequirementPackageSubscriptionId > AppConsts.NONE && View.RequirementItemId > AppConsts.NONE
                && View.RequirementCategoryId > AppConsts.NONE && View.SelectedTenantId_Global > AppConsts.NONE)
            {
                ApplicantRequirementParameterContract appParameterContract = new ApplicantRequirementParameterContract();
                appParameterContract.RequirementPkgSubscriptionId = View.RequirementPackageSubscriptionId;
                appParameterContract.RequirementItemId = View.RequirementItemId;
                appParameterContract.RequirementCategoryId = View.RequirementCategoryId;
                appParameterContract.TenantId = View.SelectedTenantId_Global;

                ApplicantRequirementItemDataContract applicantRequirementItemDataContract = ApplicantRequirementManager.GetApplicantRequirementItemData(appParameterContract, View.ApplicantId);

                if (applicantRequirementItemDataContract.IsNotNull() && !applicantRequirementItemDataContract.ApplicantRequirementFieldData.IsNullOrEmpty())
                {
                    var applicantRequirementFieldData = applicantRequirementItemDataContract.ApplicantRequirementFieldData;
                    var applicantFieldData = applicantRequirementFieldData.Where(x => x.FieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue()).FirstOrDefault();

                    if (applicantFieldData.IsNotNull() && !applicantFieldData.LstApplicantFieldDocumentMapping.IsNullOrEmpty())
                    {
                        View.lstApplicantRequirementDocumentMaps = applicantFieldData.LstApplicantFieldDocumentMapping;
                    }
                    else
                    {
                        View.lstApplicantRequirementDocumentMaps = new List<ApplicantFieldDocumentMappingContract>();
                    }
                }
            }
            else
            {
                View.lstApplicantRequirementDocumentMaps = new List<ApplicantFieldDocumentMappingContract>();
            }
        }

        /// <summary>
        /// Updates the new entries and deletes the old document mapping for Item
        /// </summary>
        /// <param name="toAddDocumentMapList">list to add ApplicantRequirementDocumentMap data</param>
        /// <param name="ToDeleteDocumentMapIDs">list to delete the mapping.</param>
        public void AssignUnAssignItemDocuments(List<ApplicantDocumentContract> toAddDocumentMapList, List<Int32> toDeleteDocumentMapIDs)
        {
            if (!toAddDocumentMapList.IsNullOrEmpty() || !toDeleteDocumentMapIDs.IsNullOrEmpty())
            {
                ApplicantRequirementManager.AssignUnAssignRequirementItemDocuments(View.SelectedTenantId_Global, toAddDocumentMapList, toDeleteDocumentMapIDs, View.RequirementItemDataId, View.CurrentLoggedInUserId);
            }
        }

        /// <summary>
        /// Assign/Un-Assign the documents for Incomplete Items. 
        /// Also handles the case when the only document has been assigned for an Item. In that case also, the Item Status remains as Incomplete.
        /// </summary>
        /// <param name="toAddDocumentMap"></param>
        /// <param name="toDeleteDocumentMap"></param>
        /// <param name="CurrentUserId"></param>
        public Tuple<Int32,Int32,Int32> AssignUnAssignIncompleteItemDocuments(List<ApplicantDocumentContract> toAddDocumentMapList, List<Int32> toDeleteApplicantRequirementDocumentMapIDs)
        {
            if (!toAddDocumentMapList.IsNullOrEmpty() || !toDeleteApplicantRequirementDocumentMapIDs.IsNullOrEmpty())
            {
                Int32 _itemDataId = 0;
                GenerateRequirementData(DateTime.Now);
                _fieldData.ARFD_FieldValue = Convert.ToString(toAddDocumentMapList.Count()); // Override the count of document

                ApplicantRequirementManager.AssignUnAssignIncompleteRequirementItemDocuments(View.SelectedTenantId_Global, toAddDocumentMapList, toDeleteApplicantRequirementDocumentMapIDs, _categoryData, _itemData, _fieldData, View.RequirementPackageSubscriptionId, View.ApplicantId, View.CurrentLoggedInUserId, out _itemDataId);

                if (_itemDataId > AppConsts.NONE || (View.RequirementItemDataId.IsNullOrEmpty() && View.RequirementItemDataId == AppConsts.NONE)) // Handle cases when the user simply clicks assign without any change in checkboxes
                {
                    View.RequirementItemDataId = _itemDataId;

                    return new Tuple<int, int, int>(_categoryData.ARCD_ID, View.RequirementItemDataId, this._fieldData.IsNotNull()?this._fieldData.ARFD_ID:AppConsts.NONE);
                }
            }
            return new Tuple<int, int, int>(AppConsts.NONE, AppConsts.NONE, AppConsts.NONE);
        }

        /// <summary>
        /// Removes the mapping from the table specific to the map id
        /// </summary>
        /// <param name="applicantMappingId">Id of which apping isa to be removed.</param>
        /// <param name="curentUserId"></param>
        public void RemoveMapping(Int32 applicantRequirementDocumentMapId)
        {
            ApplicantRequirementManager.RemoveMapping(View.SelectedTenantId_Global, applicantRequirementDocumentMapId, View.CurrentLoggedInUserId);
        }

        ApplicantRequirementCategoryData _categoryData;
        ApplicantRequirementItemData _itemData;
        ApplicantRequirementFieldData _fieldData;

        private void GenerateRequirementData(DateTime dtCreationDateTime)
        {
            String incompleteCategoryStatusCode = RequirementCategoryStatus.INCOMPLETE.GetStringValue();
            var lkpRequirementCategoryStatus = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementCategoryStatu>(View.SelectedTenantId_Global)
                                                                           .FirstOrDefault(cnd => cnd.RCS_Code == incompleteCategoryStatusCode && !cnd.RCS_IsDeleted);

            String incompleteItemStatusCode = RequirementItemStatus.INCOMPLETE.GetStringValue();
            var lkpRequirementItemStatus = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementItemStatu>(View.SelectedTenantId_Global)
                                                                           .FirstOrDefault(cnd => cnd.RIS_Code == incompleteItemStatusCode && !cnd.RIS_IsDeleted);

            _categoryData = new ApplicantRequirementCategoryData
            {
                ARCD_RequirementPackageSubscriptionID = View.RequirementPackageSubscriptionId,
                ARCD_RequirementCategoryID = View.RequirementCategoryId,
                ARCD_RequirementCategoryStatusID = lkpRequirementCategoryStatus.RCS_ID,
                ARCD_IsDeleted = false,
                ARCD_CreatedByID = View.CurrentLoggedInUserId,
                ARCD_CreatedOn = dtCreationDateTime
            };

            _itemData = new ApplicantRequirementItemData
            {
                ARID_RequirementCategoryDataID = _categoryData.ARCD_ID,
                ARID_RequirementItemID = View.RequirementItemId,
                ARID_RequirementItemStatusID = lkpRequirementItemStatus.RIS_ID,
                ARID_IsDeleted = false,
                ARID_CreatedByID = View.CurrentLoggedInUserId,
                ARID_CreatedOn = dtCreationDateTime,
                ARID_SubmissionDate = dtCreationDateTime
            };

            _fieldData = new ApplicantRequirementFieldData
            {
                ARFD_RequirementItemDataID = _itemData.ARID_ID,
                ARFD_FieldValue = "1",                                              // 1 Document is getting added on each call 
                ARFD_RequirementFieldID = View.RequirementFieldId,
                ARFD_IsDeleted = false,
                ARFD_CreatedByID = View.CurrentLoggedInUserId,
                ARFD_CreatedOn = dtCreationDateTime
            };
        }

        /// <summary>
        /// When the documents are being UPLOADED using BROWSE control.
        /// </summary>
        public void AddApplicantUploadedDocuments()
        {
            View.ToSaveApplicantUploadedDocuments = ApplicantRequirementManager.SaveApplicantUploadDocument(View.ToSaveApplicantUploadedDocuments, View.ApplicantId, View.SelectedTenantId_Global, View.CurrentLoggedInUserId);

            if (!View.IsIncompleteItem)
            {
                ApplicantRequirementManager.AddUpdateApplicantRequirementDocumentMappingData(View.SelectedTenantId_Global, View.ToSaveApplicantUploadedDocuments,
                    View.ApplicantRequirementItemData.ARID_ID, View.RequirementFieldId, View.CurrentLoggedInUserId);
            }
            else // Case of incomplete items
            {
                Int32 _itemDataId = 0;
                DateTime _dtCreationDateTime = DateTime.Now;
                GenerateRequirementData(_dtCreationDateTime);

                ApplicantRequirementManager.AddIncompleteApplicantRequirementDocumentMappingData(View.SelectedTenantId_Global, View.ToSaveApplicantUploadedDocuments, _categoryData, _itemData, _fieldData,
                    View.RequirementPackageSubscriptionId, View.ApplicantId, View.CurrentLoggedInUserId, out _itemDataId);

                if (_itemDataId > AppConsts.NONE || (View.RequirementItemDataId.IsNullOrEmpty() && View.RequirementItemDataId == AppConsts.NONE))
                    View.RequirementItemDataId = _itemDataId;
            }

        }

        /// <summary>
        /// To check if document is already uploaded
        /// </summary>
        /// <param name="documentName"></param>
        /// <param name="documentSize"></param>
        /// <param name="documentUploadedBytes"></param>
        /// <returns></returns>
        public String IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, byte[] documentUploadedBytes)
        {
            if (ApplicantRequirementManager.IsDocumentAlreadyUploaded(documentName, documentSize, View.ApplicantId, View.SelectedTenantId_Global))
            {
                return documentName;
            }

            List<ApplicantDocumentContract> applicantDocuments = ApplicantRequirementManager.GetApplicantDocument(View.ApplicantId, View.SelectedTenantId_Global);
            //Compare original document MD5Hash
            String md5Hash = CommonFileManager.GetMd5Hash(documentUploadedBytes);

            if (applicantDocuments.IsNotNull())
            {
                var applicantDocument = applicantDocuments.FirstOrDefault(x => x.OriginalDocMD5Hash == md5Hash);
                if (applicantDocument.IsNotNull())
                {
                    return applicantDocument.FileName;
                }
            }

            return null;
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
        /// This method is used to call the Parallel Task for Pdf conversion
        /// </summary>
        public void CallParallelTaskPdfConversion()
        {
            Dictionary<String, Object> conversionData = new Dictionary<String, Object>();
            //Use Poco class so that Entity will not get updated while running parallel tasks
            List<ApplicantDocumentPocoClass> lstApplicantDoc = new List<ApplicantDocumentPocoClass>();

            if (View.ToSaveApplicantUploadedDocuments.IsNotNull() && View.ToSaveApplicantUploadedDocuments.Count() >= 0)
            {
                foreach (var doc in View.ToSaveApplicantUploadedDocuments)
                {
                    ApplicantDocumentPocoClass appDoc = new ApplicantDocumentPocoClass();
                    appDoc.ApplicantDocumentID = doc.ApplicantDocumentId;
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
            conversionData.Add("CurrentLoggedUserID", View.CurrentLoggedInUserId);
            conversionData.Add("TenantID", View.SelectedTenantId_Global);

            Dictionary<String, Object> mergingData = null;

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            if (conversionData.IsNotNull() && conversionData.Count > 0)
            {
                Business.RepoManagers.DocumentManager.RunParallelTaskPdfConversionMerging(ConvertDocumentsIntoPdf, conversionData, MergeDocIntoUnifiedPdf, mergingData, LoggerService, ExceptiomService);
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
            }
        }

        public Dictionary<Boolean, String> ValidateDocumentMappingRules(Int32 noOfDocs)
        {
            if (View.IsFileUploadApplicable)
            {

                RequirementVerificationCategoryData reqVerificationData = new RequirementVerificationCategoryData();
                reqVerificationData.CatId = View.RequirementCategoryId;

                RequirementVerificationItemData itemData = new RequirementVerificationItemData();
                itemData.ItemId = View.RequirementItemId;

                RequirementVerificationFieldData fieldData = new RequirementVerificationFieldData();
                fieldData.FieldId = View.RequirementFieldId;
                fieldData.ApplicantFieldDataValue = noOfDocs.ToString();
                itemData.lstFieldData = new List<RequirementVerificationFieldData>();
                itemData.lstFieldData.Add(fieldData);

                reqVerificationData.lstItemData = new List<RequirementVerificationItemData>();
                reqVerificationData.lstItemData.Add(itemData);
                return RequirementVerificationManager.ValidateDocumentRules(reqVerificationData, View.RequirementPackageSubscriptionId, View.SelectedTenantId_Global);

            }
            return new Dictionary<Boolean, String>();
        }

    }
}




