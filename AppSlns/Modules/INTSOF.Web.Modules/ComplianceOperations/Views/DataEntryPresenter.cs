using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public class DataEntryPresenter : Presenter<IDateEntryView>
    {
        Dictionary<Int32, List<Int32>> dicShotSeriesData = new Dictionary<Int32, List<Int32>>();

        /// <summary>
        /// Gets the Date requried to build the complete form, including the Data entered for it.
        /// </summary>
        /// <returns></returns>
        public List<AdminDataEntryUIContract> GetAdminDataEntrySubscription()
        {
            return StoredProcedureManagers.GetAdminDataEntrySubscription(View.PkgSubId, View.DocumentId, View.TenantId);
        }

        /// <summary>
        /// Save/Update the form data
        /// </summary>
        public Boolean SubmitAdminDataEntry()
        {
            if (ComplianceDataManager.SubmitAdminDataEntry(View.SaveContract, View.FDEQ_ID, View.DocumentStatus, View.CurrentUserId, View.TenantId))
            {
                View.ImpactedItemCnt = GetNoOfImpactedItemsFromDataEntry(View.SaveContract);

                //UAT-2618:
                ComplianceDataManager.UpdateIsDocAssociated(View.TenantId, View.PkgSubId, true, View.CurrentUserId);
                return true;
            }
            return false;
        }

        public void GetDataEntryNextRecord(DataEntryQueueFilterContract dataEntryQueueFilters)
        {
            String inputXml = null;
            DataEntryQueueContract dataEntryRecord;
            if (dataEntryQueueFilters.SelectedTenantIds != null && dataEntryQueueFilters.SelectedTenantIds.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Int32 item in dataEntryQueueFilters.SelectedTenantIds)
                {
                    sb.Append("<TenantId>" + item.ToString() + "</TenantId>");
                }

                inputXml = "<TenantIdList>" + sb + "</TenantIdList>";
            }

            if (dataEntryQueueFilters.QueueType.ToLower() == DataEntryQueueType.DATA_ENTRY_ASSIGNMENT_QUEUE.GetStringValue().ToLower())
            {
                if (dataEntryQueueFilters.SelectedTenantIds.Count == AppConsts.ONE)
                    dataEntryRecord = SecurityManager.GetDataEntryNextRecordData(inputXml, View.GridCustomPaging, null, View.FDEQ_ID, dataEntryQueueFilters.DepartmntPrgrmMppngIds, true, dataEntryQueueFilters.SelectedTenantIds.FirstOrDefault());
                else
                    dataEntryRecord = SecurityManager.GetDataEntryNextRecordData(inputXml, View.GridCustomPaging, null, View.FDEQ_ID,String.Empty,false,AppConsts.NONE);
            }
            else
            {
                if (dataEntryQueueFilters.SelectedTenantIds.Count == AppConsts.ONE)
                    dataEntryRecord = SecurityManager.GetDataEntryNextRecordData(inputXml, View.GridCustomPaging, View.CurrentUserId, View.FDEQ_ID, dataEntryQueueFilters.DepartmntPrgrmMppngIds, true, dataEntryQueueFilters.SelectedTenantIds.FirstOrDefault());
                else
                    dataEntryRecord = SecurityManager.GetDataEntryNextRecordData(inputXml, View.GridCustomPaging, View.CurrentUserId, View.FDEQ_ID, String.Empty, false, AppConsts.NONE);
            }
            //View.NextRecord = dataEntryRecord.SkipWhile(record => record.ApplicantDocumentID == View.DocumentId).Skip(1).FirstOrDefault();
            View.NextRecord = dataEntryRecord;
        }

        /// <summary>
        /// Method to get package subscription of applicant 
        /// </summary>
        /// <param name="selectedTenantID"></param>
        /// <param name="applicantID"></param>
        /// <returns></returns>
        public List<PackageSubscriptionForDataEntry> GetPackageSubscriptionOfApplicant(Int32 selectedTenantID, Int32 applicantID)
        {
            return ComplianceDataManager.GetPackageSubscriptionForDataEntry(applicantID, selectedTenantID);
        }

        public Int32 GetNoOfImpactedItemsFromDataEntry(AdminDataEntrySaveContract savedData)
        {
            Int32 NoOfItems = 0;
            foreach (ApplicantCmplncCategoryData applicantCmplncCategoryData in savedData.ApplicantCmplncCategoryData)
            {
                if (!applicantCmplncCategoryData.ApplicantCmplncItemData.IsNullOrEmpty())
                {
                    foreach (ApplicantCmplncItemData applicantCmplncItemData in applicantCmplncCategoryData.ApplicantCmplncItemData)
                    {
                        if (applicantCmplncItemData.IsDataChanged || applicantCmplncItemData.IsItemSwapped)
                        {
                            NoOfItems++;
                        }
                    }
                }
            }
            return NoOfItems;
        }

        public short GetDocumentStatusIdByCode(String documentStatusCode)
        {
            return ComplianceDataManager.GetDocumentStatusIdByCode(documentStatusCode);
        }

        /// <summary>
        /// Upadte the Document status to different types based on scenarios
        /// </summary>
        public void UpdateDocumentStatus()
        {
            ComplianceDataManager.UpdateDoccumentStatusAfterDataEntry(View.DocumentId, View.FDEQ_ID, View.DocumentStatus, View.CurrentUserId, View.TenantId);
        }

        /// <summary>
        /// Send Email for the Document rejected by Admin during Data Entry - UAT 1301 
        /// </summary>
        public void SendDocumentRejectionEmail()
        {
            FlatDataEntryQueue _currentDocument = SecurityManager.GetFlatDataEntryQueueRecord(View.FDEQ_ID);
            CommunicationManager.SendDocumentRejectionEmail(View.ApplicantId, _currentDocument.FDEQ_ApplicantDocumentName, View.TenantId);
        }

        #region UAT-1608:
        /// <summary>
        /// Save/Update the form data
        /// </summary>
        public Boolean SubmitAdminSeriesDataEntry()
        {
            Boolean isDataSaved = false;
            StringBuilder errorMessage = new StringBuilder();
            String successMessage = String.Empty;
            if (!View.ItemSeriesSaveContract.ApplicantCmplncCategoryData.IsNullOrEmpty())
            {
                foreach (var cat in View.ItemSeriesSaveContract.ApplicantCmplncCategoryData)
                {
                    if (!cat.ApplicantCmplncItemData.IsNullOrEmpty())
                    {
                        List<Int32> _lstItemIds = new List<Int32>();

                        foreach (var item in cat.ApplicantCmplncItemData)
                        {
                            if (!item.ApplicantCmplncAttrData.IsNullOrEmpty() && (item.IsDocAssociationReq || item.IsDataChanged))
                            {


                                List<ApplicantComplianceAttributeDataContract> lstSeriesAttributeData = new List<ApplicantComplianceAttributeDataContract>();
                                item.ApplicantCmplncAttrData.ForEach(x =>
                                {
                                    ApplicantComplianceAttributeDataContract seriesAttributeData = new ApplicantComplianceAttributeDataContract();
                                    seriesAttributeData.ComplianceItemAttributeId = x.AttrId;
                                    seriesAttributeData.AttributeValue = x.AttrValue;
                                    seriesAttributeData.AttributeTypeCode = x.AttrTypeCode;
                                    lstSeriesAttributeData.Add(seriesAttributeData);
                                });
                                String seriesAttributeXML = ComplianceDataManager.GetSeriesAttributeXML(lstSeriesAttributeData);
                                String attrDocumentXML = GetAttributeXML(lstSeriesAttributeData);
                                if (!seriesAttributeXML.IsNullOrEmpty())
                                {
                                    ShotSeriesSaveResponse saveResponse = ComplianceDataManager.SaveSeriesAttributeData(View.TenantId, View.ItemSeriesSaveContract.PackageSubscriptionId,
                                                                                               item.ItemSeriesID, View.CurrentUserId, seriesAttributeXML,
                                                                                               attrDocumentXML, ShotSeriesHandleCalledFrom.AdminDataEntry, 0);
                                    if (!saveResponse.IsNullOrEmpty() && saveResponse.StatusCode == AppConsts.NONE)
                                    {
                                        if (!saveResponse.lstItemData.IsNullOrEmpty())
                                        {
                                            _lstItemIds.AddRange(saveResponse.lstItemData.Select(id => id.ItemID).ToList());
                                        }

                                        View.ImpactedItemCnt = View.ImpactedItemCnt + AppConsts.ONE;
                                    }
                                }
                            }
                        }
                        dicShotSeriesData.Add(cat.CatId, _lstItemIds.Distinct().ToList());
                    }
                }
                //UAT-2618:
                ComplianceDataManager.UpdateIsDocAssociated(View.TenantId, View.PkgSubId, true, View.CurrentUserId);
            }
            return isDataSaved;
        }

        private String GetAttributeXML(List<ApplicantComplianceAttributeDataContract> lstApplicantCmplncAttrData)
        {
            String fileUploadAttributeTypeCode = ComplianceAttributeDatatypes.FileUpload.GetStringValue();
            String attrDocumentXML = null;
            var fileUploadAttribute = lstApplicantCmplncAttrData.FirstOrDefault(cnd => cnd.AttributeTypeCode == fileUploadAttributeTypeCode);
            if (!fileUploadAttribute.IsNullOrEmpty())
            {
                attrDocumentXML = "<Documents>";
                attrDocumentXML += "<AttributeDocuments>";
                attrDocumentXML += "<AttributeId>" + fileUploadAttribute.ComplianceItemAttributeId + "</AttributeId>";
                attrDocumentXML += "<Documents>";
                attrDocumentXML += "<Id>" + View.DocumentId + "</Id>";
                attrDocumentXML += "</Documents>";
                attrDocumentXML += "</AttributeDocuments>";
                attrDocumentXML += "</Documents>";
            }
            return attrDocumentXML;
        }
        #endregion

        /// <summary>
        /// Execute the 'usp_Rule_EvaluateAdjustItemSeriesRules' procedure
        /// </summary>
        public void ExecuteAdjustItemSeriesRuleProcedure()
        {
            Dictionary<Int32, List<Int32>> dic = new Dictionary<Int32, List<Int32>>();
            List<Int32> _lstItemIds = new List<Int32>();

            foreach (var category in View.SaveContract.ApplicantCmplncCategoryData)
            {
                if (!category.ApplicantCmplncItemData.IsNullOrEmpty())
                {
                    foreach (var item in category.ApplicantCmplncItemData)
                    {
                        _lstItemIds.Add(item.ItmId);
                    }

                    dic.Add(category.CatId, _lstItemIds);
                }
            }

            foreach (var category in dicShotSeriesData)
            {
                _lstItemIds = new List<Int32>();

                foreach (var itemId in category.Value)
                {
                    _lstItemIds.Add(itemId);
                }

                if (!_lstItemIds.IsNullOrEmpty())
                {
                    if (dic.ContainsKey(category.Key))
                    {
                        List<Int32> dicItemIds = new List<Int32>();
                        dic.TryGetValue(category.Key, out dicItemIds);
                        dicItemIds.AddRange(_lstItemIds);
                        dic.Remove(category.Key);
                        dic.Add(category.Key, dicItemIds);
                    }
                    else
                    {
                        dic.Add(category.Key, _lstItemIds);
                    }
                }
            }

            if (!dic.IsNullOrEmpty())
            {
                ComplianceDataManager.EvaluateAdjustItemSeriesRules(View.TenantId, dic, View.PkgSubId, View.CurrentUserId);
            }
        }

        public Dictionary<Boolean, String> EvaluateDataEntryUIRules(Int32 packageSubscriptionID, String nonSeriesData, String seriesData)
        {
            Tuple<Dictionary<Boolean, String>, Dictionary<Int32, Int32>> ruleResponse = RuleManager.EvaluateDataEntryUIRules(View.TenantId, packageSubscriptionID, nonSeriesData, seriesData);
            View.lstRuleVoilatedItem = ruleResponse.Item2;
            return ruleResponse.Item1;
        }

        #region UAT-2456:
        /// <summary>
        /// Upadte the Document status to different types based on scenarios
        /// </summary>
        public void UpdateDiscardDocumentCount(Boolean isDocumentFirstTimeDiscarded)
        {
            ComplianceDataManager.UpdateDocumentDiscardCount(View.FDEQ_ID, View.CurrentUserId, View.TenantId, isDocumentFirstTimeDiscarded);
        }
        #endregion


        #region Production Issue: Data Entry[26/12/2016]

        public void IsDiscardDocumentEmailNeedToSend()
        {
            View.IsDiscardDocumentEmailNeedToSend = ComplianceDataManager.IsDiscardDocumentEmailNeedToSend(View.TenantId, View.DocumentId, View.SelectedDiscardReasonId,
                                                                                                           View.ApplicantId, View.CurrentUserId);
        }

        public void GetDocumentDiscardReasonList()
        {
            var tempList = ComplianceDataManager.GetDocumentDiscardReasonList(View.TenantId);

            if (tempList != null)
            {
                tempList.Insert(0, new Entity.ClientEntity.lkpDocumentDiscardReason { DDR_Name = "--SELECT--", DDR_ID = 0 });
            }
            View.LstDocumentDiscradReason = tempList;
        }
        #endregion
        #region UAT 2695
        public void GetApplicantDocument()
        {
            ApplicantDocument DocumentData = ComplianceDataManager.GetApplicantDocument(View.DocumentId, View.TenantId);
            if (DocumentData != null && !String.IsNullOrEmpty(DocumentData.FileName))
            {
                View.DocumentName = DocumentData.FileName.Trim();
            }
        }
        #endregion
    }
}
