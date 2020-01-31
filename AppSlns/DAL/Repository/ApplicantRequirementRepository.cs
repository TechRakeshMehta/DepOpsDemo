using System;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using System.Text;
using INTSOF.UI.Contract.RotationPackages;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Data.Entity.Core.Objects;

namespace DAL.Repository
{
    public class ApplicantRequirementRepository : ClientBaseRepository, IApplicantRequirementRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _dbContext;

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public ApplicantRequirementRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requirementPackageSubscriptionID"></param>
        /// <returns></returns>
        RequirementPackageSubscription IApplicantRequirementRepository.GetRequirementPackageSubscription(Int32 requirementPackageSubscriptionID)
        {

            //_dbContext.Refresh(RefreshMode.ClientWins, _dbContext.RequirementPackageSubscriptions.Include("ApplicantRequirementCategoryDatas")
            //                                                                                    .Include("ApplicantRequirementCategoryDatas.ApplicantRequirementItemDatas")
            //                                                                                    .Include("ApplicantRequirementCategoryDatas.ApplicantRequirementItemDatas.ApplicantRequirementFieldDatas"));


            // _dbContext.Refresh(RefreshMode.ClientWins, _dbContext.RequirementPackageSubscriptions);
            // _dbContext.Refresh(RefreshMode.ClientWins, _dbContext.ApplicantRequirementCategoryDatas);
            // _dbContext.Refresh(RefreshMode.ClientWins, _dbContext.ApplicantRequirementItemDatas);
            // _dbContext.Refresh(RefreshMode.ClientWins, _dbContext.ApplicantRequirementFieldDatas);

            return _dbContext.RequirementPackageSubscriptions
                                                   //.Include("ApplicantRequirementCategoryDatas")
                                                   //.Include("ApplicantRequirementCategoryDatas.lkpRequirementCategoryStatu")
                                                   //.Include("ApplicantRequirementCategoryDatas.ApplicantRequirementItemDatas")
                                                   //.Include("ApplicantRequirementCategoryDatas.ApplicantRequirementItemDatas.lkpRequirementItemStatu")
                                                   //.Include("ApplicantRequirementCategoryDatas.ApplicantRequirementItemDatas.ApplicantRequirementFieldDatas")
                                                   .FirstOrDefault(cond => cond.RPS_ID == requirementPackageSubscriptionID
                                                    && !cond.RPS_IsDeleted);


            //RequirementPackageSubscription rps = _dbContext.RequirementPackageSubscriptions
            //                                    .FirstOrDefault(cond => cond.RPS_ID == requirementPackageSubscriptionID
            //                                     && !cond.RPS_IsDeleted);

            // _dbContext.Refresh(RefreshMode.StoreWins, rps);

            //return rps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotationRequirementPackageID"></param>
        /// <returns></returns>
        RequirementPackage IApplicantRequirementRepository.GetRequirementPackageDetail(Int32 rotationRequirementPackageID)
        {
            return _dbContext.RequirementPackages.FirstOrDefault(cond => cond.RP_ID == rotationRequirementPackageID && !cond.RP_IsDeleted);
        }


        #region UAT-1316
        /// <summary>
        /// Get Requirement item data for data entry
        /// </summary>
        /// <param name="rotationRequirementPackageID"></param>
        /// <returns></returns>
        RequirementItem IApplicantRequirementRepository.GetDataEntryRequirementItem(Int32 requirementItemId)
        {
            return _dbContext.RequirementItems.FirstOrDefault(cond => cond.RI_ID == requirementItemId && !cond.RI_IsDeleted);
        }

        List<ApplicantDocument> IApplicantRequirementRepository.GetApplicantDocument(Int32 currentLoggedInUserID, List<Int32?> lstDocumentType)
        {
            return _dbContext.ApplicantDocuments.Where(ad => ad.OrganizationUserID == currentLoggedInUserID
                                                        && lstDocumentType.Contains(ad.DocumentType)
                                                        && (ad.IsSearchableOnly == null || ad.IsSearchableOnly == false)
                                                        && !ad.IsDeleted).OrderByDescending(ad => ad.ApplicantDocumentID).ToList();
        }

        ApplicantRequirementItemData IApplicantRequirementRepository.GetApplicantRequirementItemData(Int32 reqPkgSubscriptionId, Int32 reqItemId, Int32 reqCategoryId,
                                                                                                     Int32 currentLoggedInUserId)
        {
            return _dbContext.ApplicantRequirementCategoryDatas.Where(ARCD => ARCD.ARCD_RequirementPackageSubscriptionID == reqPkgSubscriptionId && !ARCD.ARCD_IsDeleted && ARCD.ARCD_RequirementCategoryID == reqCategoryId)
               .Join(_dbContext.ApplicantRequirementItemDatas, ARCD => ARCD.ARCD_ID, ARID => ARID.ARID_RequirementCategoryDataID, (ARCD, ARID) => new { ApplicantItemData = ARID, IsARIDDeleted = ARID.ARID_IsDeleted })
               .Where(itemData => itemData.ApplicantItemData.ARID_RequirementItemID == reqItemId && !itemData.IsARIDDeleted)
               .Select(i => i.ApplicantItemData).FirstOrDefault();
        }

        RequirementFieldVideo IApplicantRequirementRepository.GetRequirementFieldVideoData(Int32 rfVideoId)
        {
            return _dbContext.RequirementFieldVideos.FirstOrDefault(rfv => rfv.RFV_ID == rfVideoId && !rfv.RFV_IsDeleted);
        }

        RequirementObjectTreeProperty IApplicantRequirementRepository.GetObjectTreeProperty(Int32 rotId, Int32 lkpAttributeId)
        {
            return _dbContext.RequirementObjectTreeProperties.FirstOrDefault(rotp => rotp.ROTP_ObjectTreeID == rotId && rotp.ROTP_ObjectAttributeID == lkpAttributeId && !rotp.ROTP_IsDeleted);
        }

        ClientSystemDocument IApplicantRequirementRepository.GetClientSystemDocument(Int32 clientSysDocId)
        {
            return _dbContext.ClientSystemDocuments.FirstOrDefault(csd => csd.CSD_ID == clientSysDocId && !csd.CSD_IsDeleted);
        }

        List<RequirementObjectTreeProperty> IApplicantRequirementRepository.GetObjectTreeProperties(Int32 rotId)
        {
            return _dbContext.RequirementObjectTreeProperties.Where(rotp => rotp.ROTP_ObjectTreeID == rotId && !rotp.ROTP_IsDeleted).ToList();
        }

        #region Save Applicant Requirement Data

        /// <summary>
        /// Save/Update Applicant requirement data from Data Entry screen
        /// </summary>
        Dictionary<Boolean, String> IApplicantRequirementRepository.SaveApplicantRequirementData(ApplicantRequirementCategoryData applicantReqCategoryData, ApplicantRequirementItemData applicantReqItemData,
                                                                                                 List<ApplicantRequirementFieldData> lstApplicantFieldData, Int32 createdModifiedById, Dictionary<Int32, Int32> fieldDocuments,
                                                                                                 Int32 requirementPackageId, Int32 packageSubscriptionId, List<lkpObjectType> lstObjectTypes, Dictionary<Int32
                                                                                                , ApplicantDocument> signedAppDocuments, Int32 orgUsrID, Boolean isNewPackage, Boolean IsUploadDocUpdated)
        {
            // Get the Applicant Compliance Category Id to check if the category is already added or not. If added, then add the new item under this category. Used in both Add/Update
            Int32 applicantReqCategoryInitialId = applicantReqCategoryData.ARCD_ID = GetApplicantReqCategoryId(applicantReqCategoryData.
                                                                           ARCD_RequirementPackageSubscriptionID, applicantReqCategoryData.ARCD_RequirementCategoryID);

            DateTime creationModificationDateTime = DateTime.Now;

            //UAT-2366: Add the ability for "Item 1 must be dated before/after item 2" ui rules in rotation packages.	
            Dictionary<Boolean, String> dicResponse = new Dictionary<Boolean, String>();
            if (!isNewPackage)
                dicResponse = ValidateUiRules(applicantReqItemData, lstApplicantFieldData, lstObjectTypes, requirementPackageId, packageSubscriptionId, applicantReqCategoryData.ARCD_RequirementCategoryID);

            else
            {
                //ToDO: Commented as testing is pending.
                //dicResponse.Add(false, String.Empty);

                dicResponse = ValidateDynamicUiRules(orgUsrID, requirementPackageId, lstApplicantFieldData
                                             , applicantReqItemData.ARID_RequirementItemID, applicantReqCategoryData.ARCD_RequirementCategoryID
                                             , packageSubscriptionId, true, lstObjectTypes);
            }

            Boolean isSuccess = !dicResponse.Keys.FirstOrDefault();

            if (isSuccess)
            {
                if (applicantReqCategoryData.ARCD_ID > 0)
                {
                    #region UPDATE DATA

                    ApplicantRequirementCategoryData applicantReqCategoryDataToUpdate = _dbContext.ApplicantRequirementCategoryDatas.
                               Where(catData => catData.ARCD_ID == applicantReqCategoryData.ARCD_ID && !catData.ARCD_IsDeleted).FirstOrDefault();

                    applicantReqCategoryDataToUpdate.ARCD_ModifiedID = orgUsrID; //createdModifiedById; UAT 1261
                    applicantReqCategoryDataToUpdate.ARCD_ModifiedOn = creationModificationDateTime;

                    if (applicantReqItemData.ARID_ID > 0)
                    {

                        ApplicantRequirementItemData applicantReqItemDataToUpdate = _dbContext.ApplicantRequirementItemDatas.
                        Where(itemData => itemData.ARID_ID == applicantReqItemData.ARID_ID && !itemData.ARID_IsDeleted).FirstOrDefault();
                        //String itemStatuscode = applicantComplianceItemDataToUpdate.lkpItemComplianceStatu.Code;
                        applicantReqItemDataToUpdate.ARID_ModifiedByID = orgUsrID; //createdModifiedById; UAT 1261
                        applicantReqItemDataToUpdate.ARID_ModifiedOn = creationModificationDateTime;

                        applicantReqItemDataToUpdate.ARID_RequirementItemStatusID = applicantReqItemData.ARID_RequirementItemStatusID;
                        applicantReqItemDataToUpdate.ARID_SubmissionDate = creationModificationDateTime;

                        applicantReqItemData = applicantReqItemDataToUpdate;
                    }
                    else
                    {
                        applicantReqItemData.ARID_RequirementCategoryDataID = applicantReqCategoryData.ARCD_ID;

                        SaveAppRequirementItemData(applicantReqItemData, orgUsrID, creationModificationDateTime); //createdModifiedById; UAT 1261

                    }

                    foreach (var fieldData in lstApplicantFieldData)
                    {
                        if (fieldData.ARFD_ID > 0)
                        {
                            ApplicantRequirementFieldData fieldDataToUpdate = _dbContext.ApplicantRequirementFieldDatas.Where
                                (fldData => fldData.ARFD_ID == fieldData.ARFD_ID && !fldData.ARFD_IsDeleted).FirstOrDefault();

                            String dataTypeCode = GetFieldDataTypeCode(fieldData.ARFD_RequirementFieldID);

                            if (dataTypeCode == RequirementFieldDataType.SIGNATURE.GetStringValue().ToLower().Trim()
                                && fieldData.RequirementFieldDataLargeContents.IsNullOrEmpty())
                                continue;

                            fieldDataToUpdate.ARFD_ModifiedByID = orgUsrID; //createdModifiedById; UAT 1261
                            fieldDataToUpdate.ARFD_ModifiedOn = creationModificationDateTime;
                            fieldDataToUpdate.ARFD_FieldValue = fieldData.ARFD_FieldValue;

                            //fieldData.ARFD_CreatedOn = fieldDataToUpdate.ARFD_CreatedOn;
                            //fieldData.ARFD_CreatedByID = fieldDataToUpdate.ARFD_CreatedByID;

                            if (dataTypeCode == RequirementFieldDataType.SIGNATURE.GetStringValue().ToLower().Trim())
                            {
                                if (fieldDataToUpdate.RequirementFieldDataLargeContents.IsNullOrEmpty())
                                {
                                    if (!fieldData.RequirementFieldDataLargeContents.IsNullOrEmpty())
                                    {
                                        var signatureObj = fieldData.RequirementFieldDataLargeContents.FirstOrDefault();
                                        signatureObj.RFDLC_CreatedBy = orgUsrID;
                                        signatureObj.RFDLC_CreatedOn = DateTime.Now;
                                        signatureObj.RFDLC_ModifiedBy = null;
                                        signatureObj.RFDLC_ModifiedOn = null;
                                        fieldDataToUpdate.RequirementFieldDataLargeContents.Add(signatureObj);
                                    }
                                }
                                else
                                {
                                    //Need to Delete
                                    if (fieldData.RequirementFieldDataLargeContents.IsNullOrEmpty())
                                    {
                                        foreach (var item in fieldDataToUpdate.RequirementFieldDataLargeContents)
                                        {
                                            item.RFDLC_IsDeleted = true;
                                            item.RFDLC_ModifiedBy = orgUsrID;
                                            item.RFDLC_ModifiedOn = DateTime.Now;
                                        }
                                    }
                                    else
                                    {
                                        //Need to Update
                                        if (!fieldData.RequirementFieldDataLargeContents.IsNullOrEmpty())
                                        {
                                            var signatureRecord = fieldDataToUpdate.RequirementFieldDataLargeContents.FirstOrDefault();
                                            signatureRecord.RFDLC_Signature = fieldData.RequirementFieldDataLargeContents.FirstOrDefault().RFDLC_Signature;
                                            signatureRecord.RFDLC_ModifiedBy = orgUsrID;
                                            signatureRecord.RFDLC_ModifiedOn = DateTime.Now;
                                        }
                                    }
                                }
                            }

                            if (dataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue().ToLower().Trim())
                            {
                                List<ApplicantRequirementDocumentMap> lstMappedReqDocuments = _dbContext.ApplicantRequirementDocumentMaps.Where
                                 (documentMap => documentMap.ARDM_ApplicantRequirementFieldDataID == fieldData.ARFD_ID && documentMap.ARDM_IsDeleted == false).ToList();

                                List<ApplicantRequirementDocumentMap> lstDocumentToRemove = lstMappedReqDocuments.Where(cond => !fieldDocuments
                                                                                         .ContainsKey(cond.ARDM_ApplicantDocumentID)
                                                                                         && cond.ARDM_ApplicantRequirementFieldDataID == fieldData.ARFD_ID
                                                                                         ).ToList();
                                foreach (var mappedReqDoc in lstMappedReqDocuments)
                                {
                                    if (fieldDocuments.ContainsKey(mappedReqDoc.ARDM_ApplicantDocumentID))
                                        fieldDocuments.Remove(mappedReqDoc.ARDM_ApplicantDocumentID);
                                }
                                foreach (var mappedReqDocuments in lstDocumentToRemove)
                                {
                                    mappedReqDocuments.ARDM_IsDeleted = true;
                                    mappedReqDocuments.ARDM_ModifiedByID = orgUsrID; //createdModifiedById; UAT 1261
                                    mappedReqDocuments.ARDM_ModifiedOn = creationModificationDateTime;

                                }
                                SaveAppRequirementDocuments(orgUsrID, fieldDocuments, creationModificationDateTime, //signedAppDocuments; UAT-4900
                                                       fieldData, signedAppDocuments, IsUploadDocUpdated, fieldData.ARFD_ID); //createdModifiedById; UAT 1261
                            }
                            else if (dataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue().ToLower().Trim())
                            {
                                SaveAppSignedDocuments(orgUsrID, signedAppDocuments, creationModificationDateTime, fieldDataToUpdate, fieldData.ARFD_ID); //createdModifiedById; UAT 1261
                            }
                        }
                        else
                        {
                            //Save the attribute and its document type
                            SaveAppRequirementFields(applicantReqItemData, orgUsrID, fieldDocuments, creationModificationDateTime, fieldData, signedAppDocuments, IsUploadDocUpdated);
                            _dbContext.ApplicantRequirementFieldDatas.AddObject(fieldData); //createdModifiedById; UAT 1261
                        }
                    }
                    #endregion
                }
                else
                {
                    #region INSERT DATA

                    // Applicant requirement Category Data is not added, then no other data is entered. So insert all.
                    applicantReqCategoryData.ARCD_CreatedByID = orgUsrID; //createdModifiedById; UAT 1261
                    applicantReqCategoryData.ARCD_CreatedOn = creationModificationDateTime;
                    applicantReqCategoryData.ARCD_IsDeleted = false;
                    _dbContext.ApplicantRequirementCategoryDatas.AddObject(applicantReqCategoryData);

                    #region FOR SECOND APPLICANT REQUIREMENT ITEM DATA BEING SAVED, CHECK IF THE APPLICANTCATEGORY IS ALREADY CREATED OR NOT. IF YES,THEN GET THE APPLICANTRequirementCATEGORYID

                    if (applicantReqCategoryInitialId != 0)
                        applicantReqItemData.ARID_RequirementCategoryDataID = applicantReqCategoryInitialId;
                    else
                        applicantReqItemData.ApplicantRequirementCategoryData = applicantReqCategoryData;

                    #endregion

                    SaveAppRequirementItemData(applicantReqItemData, orgUsrID, creationModificationDateTime); //createdModifiedById; UAT 1261


                    foreach (var fieldData in lstApplicantFieldData)
                    {
                        SaveAppRequirementFields(applicantReqItemData, orgUsrID, fieldDocuments, creationModificationDateTime, fieldData, signedAppDocuments, IsUploadDocUpdated);
                        _dbContext.ApplicantRequirementFieldDatas.AddObject(fieldData);  //createdModifiedById; UAT 1261
                    }

                    #endregion
                }
                if (_dbContext.SaveChanges() > 0)
                {
                    ResetClientContext();

                    #region UAT-3532
                    dicResponse = new Dictionary<Boolean, String>();
                    List<String> docIds = new List<String>();
                    signedAppDocuments.ForEach(s => docIds.Add(Convert.ToString(s.Value.ApplicantDocumentID)));
                    dicResponse.Add(false, String.Join(",", docIds));
                    #endregion
                    return dicResponse;
                }
            }
            return dicResponse;

        }

        DataTable IApplicantRequirementRepository.GetPackageSubscriptionCategoryStatus(String requirementPackageSubscriptionIDs)
        {
            DataTable requirementPackageCategoryStatusList = new DataTable();

            DataColumn dc = new DataColumn("RequirementPackageSubscriptionID", typeof(Int32));
            requirementPackageCategoryStatusList.Columns.Add(dc);

            dc = new DataColumn("RequirementCategoryStatusCode", typeof(String));
            requirementPackageCategoryStatusList.Columns.Add(dc);

            dc = new DataColumn("ApplicantName", typeof(String));
            requirementPackageCategoryStatusList.Columns.Add(dc);

            dc = new DataColumn("RotationName", typeof(String));
            requirementPackageCategoryStatusList.Columns.Add(dc);

            dc = new DataColumn("PackageName", typeof(String));
            requirementPackageCategoryStatusList.Columns.Add(dc);

            dc = new DataColumn("OrganizationUserID", typeof(Int32));
            requirementPackageCategoryStatusList.Columns.Add(dc);

            dc = new DataColumn("Email", typeof(String));
            requirementPackageCategoryStatusList.Columns.Add(dc);

            dc = new DataColumn("UserName", typeof(String));
            requirementPackageCategoryStatusList.Columns.Add(dc);
            //UAt-3364
            dc = new DataColumn("RotationID", typeof(Int32));
            requirementPackageCategoryStatusList.Columns.Add(dc);

            DataRow requirementPackageCategoryStatusrow;
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@RPSI", requirementPackageSubscriptionIDs)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetPackageSubscriptionCategoryStatus", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            requirementPackageCategoryStatusrow = requirementPackageCategoryStatusList.NewRow();
                            requirementPackageCategoryStatusrow["RequirementPackageSubscriptionID"] = Convert.ToInt32(dr["RequirementPackageSubscriptionID"]);
                            requirementPackageCategoryStatusrow["RequirementCategoryStatusCode"] = Convert.ToString(dr["RequirementCategoryStatusCode"]);
                            requirementPackageCategoryStatusrow["ApplicantName"] = dr["ApplicantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ApplicantName"]);
                            requirementPackageCategoryStatusrow["RotationName"] = dr["RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationName"]);
                            requirementPackageCategoryStatusrow["PackageName"] = dr["PackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PackageName"]);
                            requirementPackageCategoryStatusrow["OrganizationUserID"] = dr["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrganizationUserID"]);
                            requirementPackageCategoryStatusrow["Email"] = dr["Email"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Email"]);
                            requirementPackageCategoryStatusrow["UserName"] = dr["UserName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UserName"]);
                            requirementPackageCategoryStatusrow["RotationID"] = Convert.ToInt32(dr["CR_ID"]); //UAT-3364
                            requirementPackageCategoryStatusList.Rows.Add(requirementPackageCategoryStatusrow);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return requirementPackageCategoryStatusList;
        }

        private Dictionary<Boolean, String> ValidateUiRules(ApplicantRequirementItemData applicantReqItemData, List<ApplicantRequirementFieldData> lstApplicantFieldData
                                            , List<lkpObjectType> lstObjectTypes, Int32 requirementPackageId, Int32 packageSubscriptionId, Int32 reqCatId)
        {
            #region GENERATE XML OF APPLICANT DATA ENTRY FORM OBJECTS AND GET OBJECT RULES BY EXECUTING STORED PROCEDURE usp_Rule_GetObjectsRules

            XmlDocument xmlObjectsRules = new XmlDocument();
            XmlElement elementObjectRules = (XmlElement)xmlObjectsRules.AppendChild(xmlObjectsRules.CreateElement("RuleObjects"));

            foreach (var applicantField in lstApplicantFieldData)
            {
                XmlNode exp = elementObjectRules.AppendChild(xmlObjectsRules.CreateElement("RuleObject"));
                exp.AppendChild(xmlObjectsRules.CreateElement("TypeId")).InnerText = Convert.ToString(GetObjectTypeId(LCObjectType.ComplianceATR.GetStringValue(), lstObjectTypes));
                exp.AppendChild(xmlObjectsRules.CreateElement("Id")).InnerText = Convert.ToString(applicantField.ARFD_RequirementFieldID);
                exp.AppendChild(xmlObjectsRules.CreateElement("ParentId")).InnerText = Convert.ToString(applicantReqItemData.ARID_RequirementItemID);
            }

            Int32 _ruleTypeId = GetRuleTypeId(ComplianceRuleType.UIRules.GetStringValue());
            List<usp_GetRequiremntUiRules_Result> lstObjectsRules = _dbContext.usp_GetRequiremntUiRules(packageSubscriptionId, requirementPackageId, reqCatId, Convert.ToString(xmlObjectsRules.OuterXml)).ToList();

            XmlDocument xmlRules = new XmlDocument();
            XmlElement elementRules = (XmlElement)xmlRules.AppendChild(xmlRules.CreateElement("Rules"));

            foreach (var objectRule in lstObjectsRules)
            {

                XmlElement elementRule = (XmlElement)elementRules.AppendChild(xmlRules.CreateElement("Rule"));
                elementRule.AppendChild(xmlRules.CreateElement("Id")).InnerText = Convert.ToString(objectRule.RuleId);
                XmlElement elementMappings = (XmlElement)elementRule.AppendChild(xmlRules.CreateElement("Mappings"));

                XmlNode nodeRules = elementMappings.AppendChild(xmlRules.CreateElement("Mapping"));
                nodeRules.AppendChild(xmlRules.CreateElement("Key")).InnerText = "[Object1]";
                ApplicantRequirementFieldData fieldData = lstApplicantFieldData.FirstOrDefault(sel => sel.ARFD_RequirementFieldID == objectRule.ObjectID);
                nodeRules.AppendChild(xmlRules.CreateElement("Value")).InnerText = fieldData.IsNullOrEmpty() ? String.Empty : fieldData.ARFD_FieldValue;

                XmlNode nodeRule2 = elementMappings.AppendChild(xmlRules.CreateElement("Mapping"));
                nodeRule2.AppendChild(xmlRules.CreateElement("Key")).InnerText = "[Object2]";
                nodeRule2.AppendChild(xmlRules.CreateElement("Value")).InnerText = objectRule.Object2Value.IsNullOrEmpty() ? String.Empty : objectRule.Object2Value.ToString();
            }

            String resultXML = _dbContext.usp_Rule_EvaluateFixedUiRules(xmlRules.OuterXml).FirstOrDefault();

            Dictionary<Boolean, String> dicResponse = new Dictionary<Boolean, String>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(resultXML);
            XmlNode nodeStatus = null;
            nodeStatus = xml.SelectSingleNode("Result/Status");
            XmlNodeList nodeMessages = xml.SelectNodes("Result/Messages/Message");
            Int32 statusCode = AppConsts.NONE;
            String message = String.Empty;
            if (nodeStatus.IsNullOrEmpty())
            {
                nodeStatus = xml.SelectSingleNode("Result");
            }
            List<String> lstMessage = new List<string>();
            if (!nodeMessages.IsNullOrEmpty())
            {
                foreach (XmlNode xmlNode in nodeMessages)
                {
                    if (xmlNode.IsNotNull())
                    {
                        lstMessage.Add(xmlNode.InnerText);
                    }
                }
            }
            message = String.Join("<br/>", lstMessage);
            statusCode = Convert.ToInt32(nodeStatus["Code"].InnerText);
            dicResponse.Add(Convert.ToBoolean(statusCode), message);
            return dicResponse;
            #endregion
        }

        public Dictionary<Boolean, String> ValidateDynamicUiRules(Int32 organizationUserId, Int32 reqPackageId, List<ApplicantRequirementFieldData> lstApplicantData
                                                            , Int32 complianceItemId, Int32 complianceCategoryId, Int32 packageSubscriptionId
                                                            , Boolean isDataEntryForm, List<lkpObjectType> lstObjectTypes)
        {
            List<GetRequirementObjectsRules> lstObjectsRules = GetObjectRulesByApplicantData(packageSubscriptionId, lstApplicantData, complianceItemId, complianceCategoryId, lstObjectTypes);

            Int32 _ruleTypeId = GetRuleTypeId(ComplianceRuleType.UIRules.GetStringValue());
            #region GENERATE XML OF APPLICANT INPUT VALUES & EXECUTE THE STORED PROCEDURE usp_Rule_EvaluatePreSubmitRules  AS PER THE RULE ID's

            XmlDocument xmlRules = new XmlDocument();
            XmlElement elementRules = (XmlElement)xmlRules.AppendChild(xmlRules.CreateElement("Rules"));

            Int32 initialRuleMappingId = 0;
            XmlElement elementMappings = null;

            foreach (var objectRule in lstObjectsRules)
            {
                if (initialRuleMappingId != objectRule.RuleMappingID)
                {
                    XmlElement elementRule = (XmlElement)elementRules.AppendChild(xmlRules.CreateElement("Rule"));
                    elementRule.AppendChild(xmlRules.CreateElement("Id")).InnerText = Convert.ToString(objectRule.RuleMappingID);
                    XmlElement elementObjectMapping = (XmlElement)elementRule.AppendChild(xmlRules.CreateElement("ObjectMapping"));
                    elementMappings = (XmlElement)elementObjectMapping.AppendChild(xmlRules.CreateElement("Mappings"));
                    initialRuleMappingId = objectRule.RuleMappingID;
                }

                XmlNode nodeRules = elementMappings.AppendChild(xmlRules.CreateElement("Mapping"));
                nodeRules.AppendChild(xmlRules.CreateElement("Key")).InnerText = objectRule.PlaceHolder;


                if (objectRule.IsRemoteAttribute.IsNotNull())
                {
                    if (objectRule.IsRemoteAttribute == AppConsts.NONE)
                    {
                        if (objectRule.RLMD_ConstantValue == AppConsts.SUBMISSION_DATE)
                            nodeRules.AppendChild(xmlRules.CreateElement("Value")).InnerText = DateTime.Now.ToString("MM/dd/yyyy");
                        else
                            nodeRules.AppendChild(xmlRules.CreateElement("Value")).InnerText = String.IsNullOrEmpty(Convert.ToString(objectRule.RLMD_ConstantValue)) ? GetObjectValueForDataEntry(lstApplicantData, Convert.ToInt32(objectRule.ObjectId), Convert.ToInt32(objectRule.ObjectTypeId)) : objectRule.RLMD_ConstantValue;

                    }
                    else if (objectRule.IsRemoteAttribute == AppConsts.ONE)
                    {
                        if (isDataEntryForm)
                            nodeRules.AppendChild(xmlRules.CreateElement("Value")).InnerText = objectRule.FieldValue;
                    }
                }
            }

            String resultXML = _dbContext.usp_Rule_EvaluateRequirementPreSubmitRules(xmlRules.OuterXml).FirstOrDefault();

            #endregion

            Dictionary<Boolean, String> dicResponse = new Dictionary<Boolean, String>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(resultXML);
            XmlNodeList nodeList = xml.SelectNodes("Results/Result");
            StringBuilder sbErrors = new StringBuilder();

            Int32 _errorCount = 0;
            Int32 statusCode = AppConsts.NONE;

            foreach (XmlNode xmlNode in nodeList)
            {
                if (xmlNode.IsNotNull())
                {
                    Boolean result;
                    Boolean.TryParse(xmlNode["Result"].InnerText, out result);

                    String nodeResultText = xmlNode["Result"].InnerText;
                    UIValidationResultType enumType = nodeResultText.ParseEnumbyCode<UIValidationResultType>();

                    if (enumType == UIValidationResultType.False)
                    {
                        sbErrors.Append(xmlNode["ErrorMessage"].InnerText + "<br />");
                        statusCode = AppConsts.ONE;
                    }
                    else if (enumType == UIValidationResultType.Error)
                    {
                        _errorCount++;
                        statusCode = AppConsts.ONE;
                    }
                }
            }

            if (_errorCount > 0)
                sbErrors.Append(String.Format("<br /> Some error(s) has occured and {0} rule(s) could not be validated. Please make sure that you have entered valid data.", _errorCount));

            dicResponse.Add(Convert.ToBoolean(statusCode), sbErrors.ToString());
            return dicResponse;
        }

        private String GetObjectValueForDataEntry(List<ApplicantRequirementFieldData> lstApplicantData, Int32 objectId, Int32 objectTypeId)
        {
            ApplicantRequirementFieldData applciantData = lstApplicantData.Where(data => data.ARFD_RequirementFieldID == objectId).FirstOrDefault();

            if (applciantData.IsNotNull())
            {
                return applciantData.ARFD_FieldValue;
            }
            else
                return String.Empty;
        }

        private List<GetRequirementObjectsRules> GetObjectRulesByApplicantData(Int32 packageSubscriptionId,
                                                                    List<ApplicantRequirementFieldData> lstApplicantData, Int32 reqItemId,
                                                                    Int32 reqCategoryId, List<lkpObjectType> lstObjectTypes)
        {
            #region GENERATE XML OF APPLICANT DATA ENTRY FORM OBJECTS AND GET OBJECT RULES BY EXECUTING STORED PROCEDURE usp_Rule_GetObjectsRules

            XmlDocument xmlObjectsRules = new XmlDocument();
            XmlElement elementObjectRules = (XmlElement)xmlObjectsRules.AppendChild(xmlObjectsRules.CreateElement("RuleObjects"));

            //added extra nodes for hierarchy for category
            XmlNode expCategoryNode = elementObjectRules.AppendChild(xmlObjectsRules.CreateElement("RuleObject"));
            expCategoryNode.AppendChild(xmlObjectsRules.CreateElement("TypeId")).InnerText = Convert.ToString(GetObjectTypeId(LCObjectType.ComplianceCategory.GetStringValue(), lstObjectTypes));
            expCategoryNode.AppendChild(xmlObjectsRules.CreateElement("Id")).InnerText = Convert.ToString(reqCategoryId);
            expCategoryNode.AppendChild(xmlObjectsRules.CreateElement("ParentId")).InnerText = Convert.ToString(AppConsts.NONE);

            XmlNode expItemNode = elementObjectRules.AppendChild(xmlObjectsRules.CreateElement("RuleObject"));
            expItemNode.AppendChild(xmlObjectsRules.CreateElement("TypeId")).InnerText = Convert.ToString(GetObjectTypeId(LCObjectType.ComplianceItem.GetStringValue(), lstObjectTypes));
            expItemNode.AppendChild(xmlObjectsRules.CreateElement("Id")).InnerText = Convert.ToString(reqItemId);
            expItemNode.AppendChild(xmlObjectsRules.CreateElement("ParentId")).InnerText = Convert.ToString(reqCategoryId);
            foreach (var attribute in lstApplicantData)
            {
                XmlNode exp = elementObjectRules.AppendChild(xmlObjectsRules.CreateElement("RuleObject"));
                exp.AppendChild(xmlObjectsRules.CreateElement("TypeId")).InnerText = Convert.ToString(GetObjectTypeId(LCObjectType.ComplianceATR.GetStringValue(), lstObjectTypes));
                exp.AppendChild(xmlObjectsRules.CreateElement("Id")).InnerText = Convert.ToString(attribute.ARFD_RequirementFieldID);
                exp.AppendChild(xmlObjectsRules.CreateElement("ParentId")).InnerText = Convert.ToString(reqItemId);
            }

            Int32 _ruleTypeId = GetRuleTypeId(ComplianceRuleType.UIRules.GetStringValue());
            List<GetRequirementObjectsRules> lstObjectsRules = _dbContext.usp_Rule_GetRequirementObjectsRules(packageSubscriptionId, _ruleTypeId, Convert.ToString(xmlObjectsRules.OuterXml)).ToList();

            #endregion
            return lstObjectsRules;
        }


        private Int32 GetObjectTypeId(String objectTypeCode, List<lkpObjectType> lstObjectTypes)
        {
            return lstObjectTypes.Where(oType => oType.OT_Code.ToLower() == objectTypeCode.ToLower()).FirstOrDefault().OT_ID;
        }

        private Int32 GetRuleTypeId(String ruleTypeCode)
        {
            return _dbContext.lkpRuleTypes.Where(rType => rType.RLT_Code.ToLower().Trim() == ruleTypeCode.ToLower().Trim()).FirstOrDefault().RLT_ID;
        }

        private Int32 GetApplicantReqCategoryId(Int32 packageSubscriptionId, Int32 reqCategoryId)
        {
            return _dbContext.ApplicantRequirementCategoryDatas.Where(cond => cond.ARCD_RequirementPackageSubscriptionID == packageSubscriptionId
                   && cond.ARCD_RequirementCategoryID == reqCategoryId && !cond.ARCD_IsDeleted).Select(slct => slct.ARCD_ID).FirstOrDefault();
        }

        private void SaveAppRequirementItemData(ApplicantRequirementItemData applicantReqItemData, Int32 createdModifiedById, DateTime creationDateTime)
        {
            applicantReqItemData.ARID_SubmissionDate = creationDateTime;
            applicantReqItemData.ARID_CreatedByID = createdModifiedById;
            applicantReqItemData.ARID_CreatedOn = creationDateTime;
            applicantReqItemData.ARID_IsDeleted = false;
            _dbContext.ApplicantRequirementItemDatas.AddObject(applicantReqItemData);
        }

        private void SaveAppRequirementFields(ApplicantRequirementItemData applicantReqItemData, Int32 createdModifiedById, Dictionary<Int32, Int32> fieldDocuments,
                                             DateTime creationDateTime, ApplicantRequirementFieldData fieldData, Dictionary<Int32, ApplicantDocument> appSignedDocument, Boolean IsUploadDocUpdated)
        {
            fieldData.ARFD_CreatedByID = createdModifiedById;
            fieldData.ARFD_CreatedOn = creationDateTime;
            fieldData.ARFD_IsDeleted = false;

            //28/08/2014 Sumit Sood - Resolved issue : CSU Fresno Error on entering Manual Expiration date
            //attributeData.ApplicantComplianceItemData = applicantItemData;
            if (applicantReqItemData.ARID_ID == 0)
            {
                fieldData.ApplicantRequirementItemData = applicantReqItemData;
            }
            else
            {
                fieldData.ARFD_RequirementItemDataID = applicantReqItemData.ARID_ID;
            }


            String fieldDataTypeCode;
            if (fieldData.RequirementField == null)
                fieldDataTypeCode = GetFieldDataTypeCode(fieldData.ARFD_RequirementFieldID);
            else
                fieldDataTypeCode = fieldData.RequirementField.lkpRequirementFieldDataType.RFDT_Code.ToLower().Trim();

            //if (attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower().Trim() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower().Trim())
            if (fieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue().ToLower().Trim())
            {
                SaveAppRequirementDocuments(createdModifiedById, fieldDocuments, creationDateTime, fieldData, appSignedDocument, IsUploadDocUpdated);//appSignedDocument; UAT-4900
            }
            else if (fieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue().ToLower().Trim())
            {
                SaveAppSignedDocuments(createdModifiedById, appSignedDocument, creationDateTime, fieldData);
            }
        }

        private void SaveAppRequirementDocuments(Int32 createdModifiedById, Dictionary<Int32, Int32> fieldDocuments, DateTime creationDateTime,
                                                 ApplicantRequirementFieldData fieldData, Dictionary<Int32, ApplicantDocument> signedAppDocuments, Boolean IsUploadDocUpdated, Int32 fieldDataId = 0) //signedAppDocuments; UAT-4900
        {
            if (!fieldDocuments.IsNullOrEmpty())
                foreach (var document in fieldDocuments)
                {
                    ApplicantRequirementDocumentMap appReqdocumentsMap = new ApplicantRequirementDocumentMap();
                    if (fieldDataId == 0)
                        appReqdocumentsMap.ApplicantRequirementFieldData = fieldData;
                    else
                        appReqdocumentsMap.ARDM_ApplicantRequirementFieldDataID = fieldDataId;

                    appReqdocumentsMap.ARDM_ApplicantDocumentID = document.Key;
                    appReqdocumentsMap.ARDM_IsDeleted = false;
                    appReqdocumentsMap.ARDM_CreatedByID = createdModifiedById;
                    appReqdocumentsMap.ARDM_CreatedOn = creationDateTime;
                    _dbContext.ApplicantRequirementDocumentMaps.AddObject(appReqdocumentsMap);
                }

            //Start Bug Id-24881
            //Start UAT-4900
            if (IsUploadDocUpdated && !signedAppDocuments.IsNullOrEmpty() && signedAppDocuments.Count > AppConsts.NONE)
            {
                List<ApplicantDocument> lstApplicantDocuments = signedAppDocuments.Values.ToList();
                foreach (ApplicantDocument appDocument in lstApplicantDocuments)
                {
                    ApplicantRequirementDocumentMap appReqdocumentsMap = new ApplicantRequirementDocumentMap();
                    if (fieldDataId == 0)
                    {
                        appReqdocumentsMap.ApplicantRequirementFieldData = fieldData;
                    }
                    else
                        appReqdocumentsMap.ARDM_ApplicantRequirementFieldDataID = fieldDataId;

                    appReqdocumentsMap.ApplicantDocument = appDocument;
                    appReqdocumentsMap.ARDM_IsDeleted = false;
                    appReqdocumentsMap.ARDM_CreatedByID = createdModifiedById;
                    appReqdocumentsMap.ARDM_CreatedOn = creationDateTime;
                    _dbContext.ApplicantRequirementDocumentMaps.AddObject(appReqdocumentsMap);
                }
            }
            //End UAT-4900
            //End Bug Id-24881
        }

        private void SaveAppSignedDocuments(Int32 createdModifiedById, Dictionary<Int32, ApplicantDocument> signedFieldDocuments, DateTime creationDateTime,
                                                 ApplicantRequirementFieldData fieldData, Int32 fieldDataId = 0)
        {
            if (!signedFieldDocuments.IsNullOrEmpty() && signedFieldDocuments.ContainsKey(fieldData.ARFD_RequirementFieldID))
            {
                ApplicantDocument signedAppDocument = signedFieldDocuments[fieldData.ARFD_RequirementFieldID];
                if (signedAppDocument.IsNotNull())
                {
                    ApplicantRequirementDocumentMap appReqdocumentsMap = new ApplicantRequirementDocumentMap();
                    if (fieldDataId != 0)
                    {
                        ApplicantRequirementDocumentMap appReqDocToRemove = _dbContext.ApplicantRequirementDocumentMaps.FirstOrDefault(cnd =>
                                                                                       cnd.ARDM_ApplicantRequirementFieldDataID == fieldDataId && !cnd.ARDM_IsDeleted);

                        if (!appReqDocToRemove.IsNullOrEmpty())
                        {
                            //Deleted mapping
                            appReqDocToRemove.ARDM_IsDeleted = true;
                            appReqDocToRemove.ARDM_ModifiedByID = createdModifiedById;
                            appReqDocToRemove.ARDM_ModifiedOn = creationDateTime;
                            //Delete Existing Signed Document.
                            if (!appReqDocToRemove.ApplicantDocument.IsNotNull())
                            {
                                appReqDocToRemove.ApplicantDocument.IsDeleted = true;
                                appReqDocToRemove.ApplicantDocument.ModifiedByID = createdModifiedById;
                                appReqDocToRemove.ApplicantDocument.ModifiedOn = creationDateTime;
                            }
                        }
                    }

                    appReqdocumentsMap.ApplicantRequirementFieldData = fieldData;
                    appReqdocumentsMap.ApplicantDocument = signedAppDocument;
                    appReqdocumentsMap.ARDM_IsDeleted = false;
                    appReqdocumentsMap.ARDM_CreatedByID = createdModifiedById;
                    appReqdocumentsMap.ARDM_CreatedOn = creationDateTime;
                    _dbContext.ApplicantRequirementDocumentMaps.AddObject(appReqdocumentsMap);
                    _dbContext.ApplicantDocuments.AddObject(signedAppDocument);
                    //signedAppDocument.ApplicantDocumentID;
                }
            }
        }
        private String GetFieldDataTypeCode(Int32 fieldId)
        {
            return _dbContext.RequirementFields.Where(field => field.RF_ID == fieldId).FirstOrDefault().lkpRequirementFieldDataType.RFDT_Code.ToLower().Trim();
        }

        List<ApplicantDocument> IApplicantRequirementRepository.SaveApplicantUploadDocument(List<ApplicantDocument> appUploadedDocumentList)
        {
            appUploadedDocumentList.ForEach(appDoc =>
            {
                _dbContext.ApplicantDocuments.AddObject(appDoc);
            });

            _dbContext.SaveChanges();
            return appUploadedDocumentList;
        }

        Boolean IApplicantRequirementRepository.DeleteAppRequirementItemFieldData(Int32 applicantReqItemDataId, Int32 currentUserId)
        {
            ApplicantRequirementItemData appReqItemData = _dbContext.ApplicantRequirementItemDatas.FirstOrDefault(cond => cond.ARID_ID == applicantReqItemDataId
                                                                                                                  && !cond.ARID_IsDeleted);

            if (appReqItemData.IsNotNull())
            {
                DateTime modifiedOnDateTime = DateTime.Now;
                if (appReqItemData.ApplicantRequirementFieldDatas.IsNotNull() && appReqItemData.ApplicantRequirementFieldDatas.Count() > 0)
                {
                    //Delete item fields data.
                    appReqItemData.ApplicantRequirementFieldDatas.ForEach(appReqFieldData =>
                    {
                        if (appReqFieldData.RequirementField.lkpRequirementFieldDataType.RFDT_Code.ToLower() ==
                                                                                                   RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue().ToLower())
                        {
                            // Delete mapped documents 
                            appReqFieldData.ApplicantRequirementDocumentMaps.ForEach(mapedDoc =>
                            {
                                mapedDoc.ARDM_IsDeleted = true;
                                mapedDoc.ARDM_ModifiedByID = currentUserId;
                                mapedDoc.ARDM_ModifiedOn = modifiedOnDateTime;

                            });
                        }
                        appReqFieldData.ARFD_IsDeleted = true;
                        appReqFieldData.ARFD_ModifiedByID = currentUserId;
                        appReqFieldData.ARFD_ModifiedOn = modifiedOnDateTime;

                    });
                }
                appReqItemData.ARID_ModifiedByID = currentUserId;
                appReqItemData.ARID_ModifiedOn = modifiedOnDateTime;
                appReqItemData.ARID_IsDeleted = true;

                if (_dbContext.SaveChanges() > 0)
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

        /// <summary>
        /// Check if the document with the same name and same size is already uploaded by applicant, in data entry
        /// </summary>
        /// <param name="documentName"></param>
        /// <param name="documentSize"></param>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        public Boolean IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, Int32 organizationUserId, List<lkpDocumentType> docType)
        {
            Int32 personalDocTypeID = docType.Where(cond => cond.DMT_Code.Equals(DocumentType.PERSONAL_DOCUMENT.GetStringValue())).Select(sel => sel.DMT_ID).FirstOrDefault();
            return _dbContext.ApplicantDocuments
                .Where(doc => doc.FileName.ToLower().Trim() == documentName.ToLower().Trim()
                    && doc.OriginalDocSize == documentSize
                    && doc.OrganizationUserID == organizationUserId
                    && doc.DocumentType != personalDocTypeID
                    && doc.IsDeleted == false).Any();
        }

        List<RequirementObjectTreeContract> IApplicantRequirementRepository.GetAttributeObjectTreeProperties(Int32 reqPackageId, Int32 reqItemId, Int32 reqCategoryId,
                                                                                                             Int32 currentLoggedInUserId)
        {
            List<RequirementObjectTreeContract> requirementObjectTreeList = new List<RequirementObjectTreeContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@PackageId", reqPackageId),
                    new SqlParameter("@CategoryId", reqCategoryId),
                    new SqlParameter("@ItemId", reqItemId)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetObjectTreePropertiesForAttribute", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementObjectTreeContract requirementObjectTree = new RequirementObjectTreeContract();

                            requirementObjectTree.ObjectID = Convert.ToInt32(dr["ObjectId"]);
                            requirementObjectTree.RequirementObjectTreeID = dr["RequirementObjectTreeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementObjectTreeID"]);
                            requirementObjectTree.ObjectTypeID = dr["ObjectTypeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ObjectTypeID"]);
                            requirementObjectTree.ObjectTypeCode = dr["ObjectTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ObjectTypeCode"]);
                            requirementObjectTree.ObjectAttributeValue = dr["ObjectAttributeValue"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ObjectAttributeValue"]);
                            requirementObjectTree.ObjectAttributeTypeId = dr["ObjectAttributeTypeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ObjectAttributeTypeID"]);
                            requirementObjectTree.ObjectAttributeTypeCode = dr["ObjectAttributeTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ObjectAttributeTypeCode"]);

                            requirementObjectTreeList.Add(requirementObjectTree);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }

            return requirementObjectTreeList;
        }

        ViewDocumentContract IApplicantRequirementRepository.GetViewDocumentData(Int32 applicantDocId, Int32 clientSysDocId, Int32 organizationUserId, Int32 reqFieldId, string signCode)
        {
            ViewDocumentContract viewDocContract = new ViewDocumentContract();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@ApplicantDocID", applicantDocId),
                    new SqlParameter("@ClientSysDocID", clientSysDocId),
                    new SqlParameter("@OrganizationUserID", organizationUserId),
                    new SqlParameter("@ReqFieldID", reqFieldId),
                    new SqlParameter("@SignCode",signCode)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetViewDocumentData", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementObjectTreeContract requirementObjectTree = new RequirementObjectTreeContract();

                            viewDocContract.DocumentPath = Convert.ToString(dr["DocumentPath"]);
                            viewDocContract.DocumentName = Convert.ToString(dr["FileName"]);
                            viewDocContract.FullName = Convert.ToString(dr["FullName"]);
                            viewDocContract.IsApplicantDoc = Convert.ToBoolean(dr["IsApplicantDoc"]);
                            viewDocContract.IsSignatureRequired = Convert.ToBoolean(dr["IsSignatureRequired"]);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return viewDocContract;
        }

        #endregion
        #endregion

        #region Rule's Execution.
        void IApplicantRequirementRepository.ExecuteRequirementObjectBuisnessRules(String ruleObjectXML, Int32 reqSubscriptionId, Int32 systemUserId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_Rule_EvaluateRequirementFixedBusinessRules", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RequirementSubscriptionID", reqSubscriptionId);
                command.Parameters.AddWithValue("@Objects", ruleObjectXML);
                command.Parameters.AddWithValue("@SystemUserID", systemUserId);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                command.ExecuteNonQuery();
                con.Close();
            }
        }


        #endregion

        #region GET EXPLANATORY NOTES
        String IApplicantRequirementRepository.GetExplanatoryNotes(Int32 objectId, Int32 objectTypeId, Int32 contentTypeId)
        {
            LargeContent largeContent = _dbContext.LargeContents.Where(obj => obj.LC_ObjectID == objectId && obj.LC_LargeContentTypeID == contentTypeId
                                                                          && obj.LC_ObjectTypeID == objectTypeId && obj.LC_IsDeleted == false).FirstOrDefault();
            if (largeContent.IsNotNull())
            {
                return largeContent.LC_Content;
            }
            else
            {
                return String.Empty;
            }
        }
        #endregion

        //UAT-1523: Addition a notes box for each rotation for the student to input information
        Boolean IApplicantRequirementRepository.SaveChanges()
        {
            if (_dbContext.SaveChanges() > 0)
                return true;
            return false;
        }

        #region UAT-2224: Admin access to upload/associate documents on rotation package items.

        /// <summary>
        /// Get Applicant Requirement Item Data by ID
        /// </summary>
        /// <param name="applicantRequirementItemDataID"></param>
        /// <returns></returns>
        ApplicantRequirementItemData IApplicantRequirementRepository.GetApplicantRequirementItemDataByID(Int32 applicantRequirementItemDataID)
        {
            return _dbContext.ApplicantRequirementItemDatas.FirstOrDefault(x => x.ARID_ID == applicantRequirementItemDataID && !x.ARID_IsDeleted);

        }

        /// <summary>
        /// Add/Update applicant Requirement document mapping data
        /// </summary>
        /// <param name="applicantUploadedDocuments"></param>
        /// <param name="applicantRequirementItemDataId"></param>
        /// <param name="requirementFieldId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean IApplicantRequirementRepository.AddUpdateApplicantRequirementDocumentMappingData(List<ApplicantDocumentContract> applicantUploadedDocuments, Int32 applicantRequirementItemDataId, Int32 requirementFieldId, Int32 currentUserId)
        {
            if (!applicantUploadedDocuments.IsNullOrEmpty())
            {
                String _uploadDocumentFieldTypeCode = RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue();
                ApplicantRequirementFieldData _fieldData = _dbContext.ApplicantRequirementFieldDatas
                                                                      .Where(x => x.ARFD_RequirementItemDataID == applicantRequirementItemDataId
                                                                      && x.RequirementField.lkpRequirementFieldDataType.RFDT_Code == _uploadDocumentFieldTypeCode)
                                                                      .FirstOrDefault();

                if (_fieldData.IsNullOrEmpty()) //field Data is to be added for first time
                {
                    _fieldData = new ApplicantRequirementFieldData();
                    _fieldData.ARFD_RequirementItemDataID = applicantRequirementItemDataId;
                    _fieldData.ARFD_RequirementFieldID = requirementFieldId;
                    _fieldData.ARFD_FieldValue = "0";
                    _fieldData.ARFD_IsDeleted = false;
                    _fieldData.ARFD_CreatedByID = currentUserId;
                    _fieldData.ARFD_CreatedOn = DateTime.Now;

                    _dbContext.ApplicantRequirementFieldDatas.AddObject(_fieldData);
                    _dbContext.SaveChanges();
                }

                applicantUploadedDocuments.ForEach(savedDoc =>
                {
                    ApplicantRequirementDocumentMap docMap = new ApplicantRequirementDocumentMap();
                    docMap.ARDM_ApplicantDocumentID = savedDoc.ApplicantDocumentId;
                    docMap.ARDM_ApplicantRequirementFieldDataID = _fieldData.ARFD_ID;
                    docMap.ARDM_IsDeleted = false;
                    docMap.ARDM_CreatedByID = currentUserId;
                    docMap.ARDM_CreatedOn = DateTime.Now;
                    _dbContext.ApplicantRequirementDocumentMaps.AddObject(docMap);
                });

                _fieldData.ARFD_FieldValue = Convert.ToString(Convert.ToInt32(_fieldData.ARFD_FieldValue) + applicantUploadedDocuments.Count());

                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Add Incomplete Applicant Requirement Document Mapping Data
        /// </summary>
        /// <param name="applicantUploadedDocuments"></param>
        /// <param name="categoryData"></param>
        /// <param name="itemData"></param>
        /// <param name="fieldData"></param>
        /// <param name="requirementPackageSubscriptionId"></param>
        /// <param name="applicantId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="itemDataId"></param>
        /// <returns></returns>
        Boolean IApplicantRequirementRepository.AddIncompleteApplicantRequirementDocumentMappingData(List<ApplicantDocumentContract> applicantUploadedDocuments, ApplicantRequirementCategoryData categoryData,
              ApplicantRequirementItemData itemData, ApplicantRequirementFieldData fieldData, Int32 requirementPackageSubscriptionId, Int32 applicantId, Int32 currentUserId, out Int32 itemDataId)
        {
            itemDataId = 0;
            Int32 fieldDataID = 0;

            if (!applicantUploadedDocuments.IsNullOrEmpty())
            {
                ApplicantRequirementCategoryData catdata = _dbContext.ApplicantRequirementCategoryDatas.FirstOrDefault(arcd => arcd.ARCD_RequirementCategoryID == categoryData.ARCD_RequirementCategoryID
                                                                                                        && arcd.ARCD_RequirementPackageSubscriptionID == categoryData.ARCD_RequirementPackageSubscriptionID && !arcd.ARCD_IsDeleted);
                // First time insertion
                if (catdata.IsNullOrEmpty())
                {
                    _dbContext.ApplicantRequirementCategoryDatas.AddObject(categoryData);
                    _dbContext.ApplicantRequirementItemDatas.AddObject(itemData);
                    _dbContext.ApplicantRequirementFieldDatas.AddObject(fieldData);

                    _dbContext.SaveChanges();
                    itemDataId = itemData.ARID_ID;
                    fieldDataID = fieldData.ARFD_ID;
                }
                else  // Data already added by another item of same category
                {
                    ApplicantRequirementItemData _itemData = _dbContext.ApplicantRequirementItemDatas.FirstOrDefault(itm => itm.ARID_RequirementCategoryDataID == catdata.ARCD_ID
                                                                                                     && itm.ARID_RequirementItemID == itemData.ARID_RequirementItemID
                                                                                                     && itm.ARID_IsDeleted == false);

                    if (_itemData.IsNullOrEmpty()) // Case when only category data is saved - Can occur when only category data is saved by admin from the top of items
                    {
                        itemData.ARID_RequirementCategoryDataID = catdata.ARCD_ID;
                        _dbContext.ApplicantRequirementItemDatas.AddObject(itemData); // If NO item exists, then add new item and field for it
                        _dbContext.ApplicantRequirementFieldDatas.AddObject(fieldData);
                        _dbContext.SaveChanges();

                        fieldDataID = fieldData.ARFD_ID;
                    }
                    else
                    {
                        String _fileUploadCode = RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue();

                        ApplicantRequirementFieldData _fieldData = _itemData.ApplicantRequirementFieldDatas.FirstOrDefault(att => att.ARFD_RequirementItemDataID == _itemData.ARID_ID
                                && att.RequirementField.lkpRequirementFieldDataType.RFDT_Code == _fileUploadCode
                            && att.ARFD_IsDeleted == false);

                        if (_fieldData.IsNullOrEmpty())
                        {
                            // Get Applicant Compliance Item Id if exists, else attach new instance id
                            fieldData.ARFD_RequirementItemDataID = _itemData.ARID_ID;
                            _dbContext.ApplicantRequirementFieldDatas.AddObject(fieldData);

                            _dbContext.SaveChanges();

                            fieldDataID = fieldData.ARFD_ID;
                        }
                        else
                        {
                            _fieldData.ARFD_FieldValue = Convert.ToString(Convert.ToInt32(_fieldData.ARFD_FieldValue) + Convert.ToInt32(applicantUploadedDocuments.Count()));
                            _fieldData.ARFD_ModifiedOn = DateTime.Now;
                            _fieldData.ARFD_ModifiedByID = currentUserId;

                            fieldDataID = _fieldData.ARFD_ID;
                        }
                    }

                    if (_itemData.IsNullOrEmpty())
                        itemDataId = itemData.ARID_ID;
                    else
                        itemDataId = _itemData.ARID_ID;
                }

                //Map saved applicant documents
                applicantUploadedDocuments.ForEach(savedDoc =>
                {
                    ApplicantRequirementDocumentMap docMap = new ApplicantRequirementDocumentMap();
                    docMap.ARDM_ApplicantDocumentID = savedDoc.ApplicantDocumentId;
                    docMap.ARDM_ApplicantRequirementFieldDataID = fieldDataID;
                    docMap.ARDM_IsDeleted = false;
                    docMap.ARDM_CreatedByID = currentUserId;
                    docMap.ARDM_CreatedOn = DateTime.Now;

                    _dbContext.ApplicantRequirementDocumentMaps.AddObject(docMap);
                });

                _dbContext.SaveChanges();

                return true;

            }

            return false;
        }

        /// <summary>
        /// Assign/UnAssign Requirement Item documents
        /// </summary>
        /// <param name="toAddDocumentMapList"></param>
        /// <param name="toDeleteApplicantRequirementDocumentMapIDs"></param>
        /// <param name="requirementItemDataId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean IApplicantRequirementRepository.AssignUnAssignRequirementItemDocuments(List<ApplicantDocumentContract> toAddDocumentMapList, List<Int32> toDeleteApplicantRequirementDocumentMapIDs, Int32 requirementItemDataId, Int32 currentUserId)
        {
            DateTime currentDateTime = DateTime.Now;
            String _uplodeDocumentCode = RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue();
            String _viewDocCode = RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue();

            ApplicantRequirementFieldData _fieldData = _dbContext.ApplicantRequirementFieldDatas.FirstOrDefault(x => x.ARFD_RequirementItemDataID == requirementItemDataId
                 && x.RequirementField.lkpRequirementFieldDataType.RFDT_Code == _uplodeDocumentCode && !x.ARFD_IsDeleted);

            ApplicantRequirementFieldData _viewDocFieldData = _dbContext.ApplicantRequirementFieldDatas.FirstOrDefault(x => x.ARFD_RequirementItemDataID == requirementItemDataId
                && x.RequirementField.lkpRequirementFieldDataType.RFDT_Code == _viewDocCode && !x.ARFD_IsDeleted);

            if (_fieldData.IsNotNull())
            {
                _fieldData.ARFD_FieldValue = Convert.ToString((Convert.ToInt32(_fieldData.ARFD_FieldValue.IsNullOrEmpty() ? AppConsts.ZERO : _fieldData.ARFD_FieldValue) + toAddDocumentMapList.Count()));
                _fieldData.ARFD_ModifiedByID = currentUserId;
                _fieldData.ARFD_ModifiedOn = currentDateTime;
            }

            Int32 viewDocMapIDs = 0;
            if (_viewDocFieldData.IsNotNull() && !_viewDocFieldData.ApplicantRequirementDocumentMaps.IsNullOrEmpty())
            {
                viewDocMapIDs = (_viewDocFieldData.ApplicantRequirementDocumentMaps.Where(cond => !cond.ARDM_IsDeleted).FirstOrDefault()).IsNull() ? AppConsts.NONE :
                    (_viewDocFieldData.ApplicantRequirementDocumentMaps.Where(cond => !cond.ARDM_IsDeleted).FirstOrDefault()).ARDM_ID;
            }

            if (toDeleteApplicantRequirementDocumentMapIDs.Contains(viewDocMapIDs))
            {
                _viewDocFieldData.ARFD_FieldValue = Convert.ToString(Convert.ToInt32(_viewDocFieldData.ARFD_FieldValue) - 1);
                if (_fieldData.IsNotNull())
                {
                    _fieldData.ARFD_FieldValue = Convert.ToString(Convert.ToInt32(_fieldData.ARFD_FieldValue) - (toDeleteApplicantRequirementDocumentMapIDs.Count() - 1));
                }
            }
            else if (_fieldData.IsNotNull())
            {
                _fieldData.ARFD_FieldValue = Convert.ToString(Convert.ToInt32(_fieldData.ARFD_FieldValue) - toDeleteApplicantRequirementDocumentMapIDs.Count());
            }

            //Map saved applicant documents
            if (!toAddDocumentMapList.IsNullOrEmpty() && _fieldData.IsNotNull())
            {
                //Map saved applicant documents
                toAddDocumentMapList.ForEach(savedDoc =>
                {
                    ApplicantRequirementDocumentMap docMap = new ApplicantRequirementDocumentMap();
                    docMap.ARDM_ApplicantDocumentID = savedDoc.ApplicantDocumentId;
                    docMap.ARDM_ApplicantRequirementFieldDataID = _fieldData.ARFD_ID;
                    docMap.ARDM_IsDeleted = false;
                    docMap.ARDM_CreatedByID = currentUserId;
                    docMap.ARDM_CreatedOn = DateTime.Now;

                    _dbContext.ApplicantRequirementDocumentMaps.AddObject(docMap);
                });
            }

            //Delete Applicant Requirement Document Map data
            if (!toDeleteApplicantRequirementDocumentMapIDs.IsNullOrEmpty())
            {
                List<ApplicantRequirementDocumentMap> applicantRequirementDocumentMapInDBList =
                   _dbContext.ApplicantRequirementDocumentMaps.Where(x => toDeleteApplicantRequirementDocumentMapIDs.Contains(x.ARDM_ID) && !x.ARDM_IsDeleted).ToList();

                applicantRequirementDocumentMapInDBList.ForEach(applicantRequirementDocumentMapInDB =>
                {
                    applicantRequirementDocumentMapInDB.ARDM_IsDeleted = true;
                    applicantRequirementDocumentMapInDB.ARDM_ModifiedByID = currentUserId;
                    applicantRequirementDocumentMapInDB.ARDM_ModifiedOn = currentDateTime;
                });
            }

            _dbContext.SaveChanges();


            return true;
        }

        /// <summary>
        /// Assign/UnAssign Incomplete Requirement Item documents
        /// </summary>
        /// <param name="toAddDocumentMapList"></param>
        /// <param name="toDeleteApplicantRequirementDocumentMapIDs"></param>
        /// <param name="categoryData"></param>
        /// <param name="itemData"></param>
        /// <param name="fieldData"></param>
        /// <param name="requirementPackageSubscriptionId"></param>
        /// <param name="applicantId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="itemDataId"></param>
        Boolean IApplicantRequirementRepository.AssignUnAssignIncompleteRequirementItemDocuments(List<ApplicantDocumentContract> toAddDocumentMapList,
            List<Int32> toDeleteApplicantRequirementDocumentMapIDs, ApplicantRequirementCategoryData categoryData,
                        ApplicantRequirementItemData itemData, ApplicantRequirementFieldData fieldData, Int32 requirementPackageSubscriptionId, Int32 applicantId, Int32 currentUserId, out Int32 itemDataId)
        {
            itemDataId = AppConsts.NONE;
            Int32 fieldDataID = 0;

            if (!toAddDocumentMapList.IsNullOrEmpty())
            {
                ApplicantRequirementCategoryData catdata = _dbContext.ApplicantRequirementCategoryDatas.FirstOrDefault(arcd => arcd.ARCD_RequirementCategoryID == categoryData.ARCD_RequirementCategoryID
                                                                                                        && arcd.ARCD_RequirementPackageSubscriptionID == categoryData.ARCD_RequirementPackageSubscriptionID && !arcd.ARCD_IsDeleted);
                // First time insertion
                if (catdata.IsNullOrEmpty())
                {
                    _dbContext.ApplicantRequirementCategoryDatas.AddObject(categoryData);
                    _dbContext.ApplicantRequirementItemDatas.AddObject(itemData);
                    _dbContext.ApplicantRequirementFieldDatas.AddObject(fieldData);

                    _dbContext.SaveChanges();

                    itemDataId = itemData.ARID_ID;
                    fieldDataID = fieldData.ARFD_ID;
                }
                else  // Data already added by another item of same category
                {
                    ApplicantRequirementItemData _itemData = _dbContext.ApplicantRequirementItemDatas.FirstOrDefault(itm => itm.ARID_RequirementCategoryDataID == catdata.ARCD_ID
                                                                                                     && itm.ARID_RequirementItemID == itemData.ARID_RequirementItemID
                                                                                                     && itm.ARID_IsDeleted == false);

                    categoryData = catdata;

                    if (_itemData.IsNullOrEmpty()) // Case when only category data is saved - Can occur when only category data is saved by admin from the top of items
                    {
                        itemData.ARID_RequirementCategoryDataID = catdata.ARCD_ID;
                        _dbContext.ApplicantRequirementItemDatas.AddObject(itemData); // If NO item exists, then add new item and field for it
                        _dbContext.ApplicantRequirementFieldDatas.AddObject(fieldData);

                        _dbContext.SaveChanges();

                        fieldDataID = fieldData.ARFD_ID;
                    }
                    else
                    {
                        String _uploadDocumentCode = RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue();

                        ApplicantRequirementFieldData _fieldData = _itemData.ApplicantRequirementFieldDatas.FirstOrDefault(att => att.ARFD_RequirementItemDataID == _itemData.ARID_ID
                                && att.RequirementField.lkpRequirementFieldDataType.RFDT_Code == _uploadDocumentCode
                            && att.ARFD_IsDeleted == false);

                        if (_fieldData.IsNullOrEmpty())
                        {
                            // Get Applicant Compliance Item Id if exists, else attach new instance id
                            fieldData.ARFD_RequirementItemDataID = _itemData.ARID_ID;
                            _dbContext.ApplicantRequirementFieldDatas.AddObject(fieldData);

                            _dbContext.SaveChanges();

                            fieldDataID = fieldData.ARFD_ID;
                        }
                        else
                        {
                            _fieldData.ARFD_FieldValue = Convert.ToString(Convert.ToInt32(_fieldData.ARFD_FieldValue) + Convert.ToInt32(toAddDocumentMapList.Count()) - toDeleteApplicantRequirementDocumentMapIDs.Count());
                            _fieldData.ARFD_ModifiedOn = DateTime.Now;
                            _fieldData.ARFD_ModifiedByID = currentUserId;

                            fieldDataID = _fieldData.ARFD_ID;
                        }
                    }

                    if (_itemData.IsNullOrEmpty())
                        itemDataId = itemData.ARID_ID;
                    else
                        itemDataId = _itemData.ARID_ID;
                }

                //Map saved applicant documents
                toAddDocumentMapList.ForEach(savedDoc =>
                {
                    ApplicantRequirementDocumentMap docMap = new ApplicantRequirementDocumentMap();
                    docMap.ARDM_ApplicantDocumentID = savedDoc.ApplicantDocumentId;
                    docMap.ARDM_ApplicantRequirementFieldDataID = fieldDataID;
                    docMap.ARDM_IsDeleted = false;
                    docMap.ARDM_CreatedByID = currentUserId;
                    docMap.ARDM_CreatedOn = DateTime.Now;

                    _dbContext.ApplicantRequirementDocumentMaps.AddObject(docMap);
                });

                _dbContext.SaveChanges();
            }

            //Delete Applicant Requirement Document Map data
            if (!toDeleteApplicantRequirementDocumentMapIDs.IsNullOrEmpty())
            {
                DateTime currentDateTime = DateTime.Now;
                List<ApplicantRequirementDocumentMap> applicantRequirementDocumentMapInDBList =
                   _dbContext.ApplicantRequirementDocumentMaps.Where(x => toDeleteApplicantRequirementDocumentMapIDs.Contains(x.ARDM_ID) && !x.ARDM_IsDeleted).ToList();

                applicantRequirementDocumentMapInDBList.ForEach(applicantRequirementDocumentMapInDB =>
                {
                    applicantRequirementDocumentMapInDB.ARDM_IsDeleted = true;
                    applicantRequirementDocumentMapInDB.ARDM_ModifiedByID = currentUserId;
                    applicantRequirementDocumentMapInDB.ARDM_ModifiedOn = currentDateTime;
                });

                _dbContext.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// Remove Applicant Requirement Document Mapping
        /// </summary>
        /// <param name="applicantRequirementDocumentMapId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean IApplicantRequirementRepository.RemoveMapping(Int32 applicantRequirementDocumentMapId, Int32 currentUserId)
        {
            ApplicantRequirementDocumentMap applicantRequirementDocumentMapInDB = _dbContext.ApplicantRequirementDocumentMaps.FirstOrDefault(x => x.ARDM_ID == applicantRequirementDocumentMapId && !x.ARDM_IsDeleted);

            if (applicantRequirementDocumentMapInDB != null)
            {
                DateTime _dtCurrentDateTime = DateTime.Now;

                applicantRequirementDocumentMapInDB.ARDM_IsDeleted = true;
                applicantRequirementDocumentMapInDB.ARDM_ModifiedByID = currentUserId;
                applicantRequirementDocumentMapInDB.ARDM_ModifiedOn = _dtCurrentDateTime;

                ApplicantRequirementFieldData _fieldData = applicantRequirementDocumentMapInDB.ApplicantRequirementFieldData;
                _fieldData.ARFD_ModifiedByID = currentUserId;
                _fieldData.ARFD_ModifiedOn = _dtCurrentDateTime;
                _fieldData.ARFD_FieldValue = Convert.ToString(Convert.ToInt32(_fieldData.ARFD_FieldValue) - 1);

                _dbContext.SaveChanges();
            }
            return true;
        }

        #endregion

        public Boolean CanDeleteRqmtFieldUploadDoc(Int32 applicantUploadedDocumentID)
        {
            Boolean canDelete = false;

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("ups_CanDeleteRqmtFieldUploadDoc", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ApplicantUploadedDocumentID", applicantUploadedDocumentID);
                canDelete = Convert.ToBoolean(cmd.ExecuteScalar());
                con.Close();
            }

            return canDelete;
        }

        //UAT-2905

        DataTable IApplicantRequirementRepository.GetMailDataForItemSubmitted(String rpsIds)
        {
            // ApplicantRequirementParameterContract mailDataForItemSubmitted = new ApplicantRequirementParameterContract();
            DataTable mailDataForItemSubmittedList = new DataTable();

            DataColumn dc = new DataColumn("RequirementPackageSubscriptionID", typeof(Int32));
            mailDataForItemSubmittedList.Columns.Add(dc);

            dc = new DataColumn("ApplicantName", typeof(String));
            mailDataForItemSubmittedList.Columns.Add(dc);

            dc = new DataColumn("RotationName", typeof(String));
            mailDataForItemSubmittedList.Columns.Add(dc);

            dc = new DataColumn("ItemName", typeof(String));
            mailDataForItemSubmittedList.Columns.Add(dc);

            dc = new DataColumn("ItemStatusCode", typeof(String));
            mailDataForItemSubmittedList.Columns.Add(dc);

            dc = new DataColumn("PackageName", typeof(String));
            mailDataForItemSubmittedList.Columns.Add(dc);

            dc = new DataColumn("UserName", typeof(String));
            mailDataForItemSubmittedList.Columns.Add(dc);

            dc = new DataColumn("ReceiverEmailAddress", typeof(String));
            mailDataForItemSubmittedList.Columns.Add(dc);

            dc = new DataColumn("ReceiverOrganizationUserID", typeof(Int32));
            mailDataForItemSubmittedList.Columns.Add(dc);

            dc = new DataColumn("RequirementItemDataID", typeof(Int32));
            mailDataForItemSubmittedList.Columns.Add(dc);

            dc = new DataColumn("RotationID", typeof(Int32));
            mailDataForItemSubmittedList.Columns.Add(dc);


            DataRow mailDataForItemSubmittedRow;


            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@RPSID", rpsIds),
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetMailDataForItemSubmitted", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            mailDataForItemSubmittedRow = mailDataForItemSubmittedList.NewRow();
                            mailDataForItemSubmittedRow["RequirementPackageSubscriptionID"] = Convert.ToInt32(dr["RequirementPackageSubscriptionID"]);
                            mailDataForItemSubmittedRow["ApplicantName"] = dr["ApplicantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ApplicantName"]);
                            mailDataForItemSubmittedRow["RotationName"] = dr["RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationName"]);
                            mailDataForItemSubmittedRow["ItemName"] = dr["ItemName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ItemName"]);
                            mailDataForItemSubmittedRow["ItemStatusCode"] = dr["ItemStatusCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ItemStatusCode"]);
                            mailDataForItemSubmittedRow["packageName"] = dr["PackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PackageName"]);
                            mailDataForItemSubmittedRow["UserName"] = dr["UserName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UserName"]);
                            mailDataForItemSubmittedRow["ReceiverEmailAddress"] = dr["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PrimaryEmailAddress"]);
                            mailDataForItemSubmittedRow["ReceiverOrganizationUserID"] = dr["RotationCreatedUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RotationCreatedUserID"]);
                            mailDataForItemSubmittedRow["RequirementItemDataID"] = dr["RequirementItemDataID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementItemDataID"]);
                            mailDataForItemSubmittedRow["RotationID"] = dr["RotationID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RotationID"]);
                            mailDataForItemSubmittedList.Rows.Add(mailDataForItemSubmittedRow);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);

                return mailDataForItemSubmittedList;
            }
        }

        List<RequirementItemField> IApplicantRequirementRepository.GetRequirementFieldList(Int32 reqItemId)
        {
            return ClientDBContext.RequirementItemFields.Include("RequirementField").Where(cond => cond.RIF_RequirementItemID == reqItemId && !cond.RIF_IsDeleted).ToList();
        }

        List<Int32> IApplicantRequirementRepository.FilterRequirementDataItemsByStatusCode(string dataIds, string statusCode)
        {
            List<Int32> lstItemDataIds = new List<int>();
            lstItemDataIds = dataIds.Split(',').Select(s => Convert.ToInt32(s)).ToList();

            if (!lstItemDataIds.IsNullOrEmpty())
            {
                return _dbContext.ApplicantRequirementItemDatas.Where(cond => lstItemDataIds.Contains(cond.ARID_ID)
                                                                && cond.lkpRequirementItemStatu.RIS_Code == statusCode
                                                                && !cond.ARID_IsDeleted)
                                                        .Select(s => s.ARID_ID)
                                                        .ToList();
            }
            else
            {
                return null;
            }

        }

        Boolean IApplicantRequirementRepository.SaveBadgeRequestFormData(List<BadgeFormNotificationDataContract> lstBadgeFormNotificationDataContract, Int32 compliancePackageTypeID, Int32 requirementPackageTypeID, string compItemCode, string reqItemCode, Int32 itemApprovalDateBagdeFormFieldTypeID, Int32 currentOrgUserID)
        {
            foreach (BadgeFormNotificationDataContract item in lstBadgeFormNotificationDataContract)
            {
                Int32 packageTypeID;
                if (item.ItemTypeCode == compItemCode)
                    packageTypeID = compliancePackageTypeID;
                else
                    packageTypeID = requirementPackageTypeID;


                if (item.IsRecordUpdated)
                {
                    //Updated Case
                    Int32 objectTypeID = _dbContext.lkpObjectTypes.Where(cond => cond.OT_Code == item.ItemTypeCode && !cond.OT_IsDeleted).Select(s => s.OT_ID).FirstOrDefault();
                    var existingRecord = _dbContext.BadgeRequestFormDatas.Where(cond => cond.BRFD_PackageTypeID == packageTypeID
                                                                                && cond.BRFD_RecordTypeID == itemApprovalDateBagdeFormFieldTypeID
                                                                                && cond.BRFD_RecordID == item.ItemDataId
                                                                                && !cond.BRFD_IsDeleted
                                                                                && cond.BRFD_OrgUserID == item.AppOrgUserID).FirstOrDefault();

                    if (!existingRecord.IsNullOrEmpty())
                    {
                        if (item.IsDataByItemApproval)
                        {
                            existingRecord.BRFD_RecordValue = DateTime.Now.ToString("dd MMM yyyy hh:mm tt");
                            existingRecord.BRFD_ModifiedBy = currentOrgUserID;
                            existingRecord.BRFD_ModifiedOn = DateTime.Now;

                            if (item.ProfileSharingInvitationGroupID != 0 && existingRecord.BRFD_PSIG_ID != item.ProfileSharingInvitationGroupID)
                                existingRecord.BRFD_PSIG_ID = item.ProfileSharingInvitationGroupID;
                        }
                        else
                        {
                            if (item.ProfileSharingInvitationGroupID != 0 && existingRecord.BRFD_PSIG_ID != item.ProfileSharingInvitationGroupID)
                            {
                                existingRecord.BRFD_ModifiedBy = currentOrgUserID;
                                existingRecord.BRFD_ModifiedOn = DateTime.Now;
                                existingRecord.BRFD_PSIG_ID = item.ProfileSharingInvitationGroupID;
                            }
                        }
                    }
                    else
                    {
                        if (!item.IsDataByItemApproval)
                        {
                            if (packageTypeID == compliancePackageTypeID)
                            {
                                var complianceItem = _dbContext.ApplicantComplianceItemDatas.Where(cond => cond.ApplicantComplianceItemID == item.ItemDataId).FirstOrDefault();

                                //Insert new row -- Of Item Modification Date
                                string recordValue = complianceItem.ModifiedOn.HasValue ? complianceItem.ModifiedOn.Value.ToString("dd MMM yyyy hh:mm tt") : complianceItem.CreatedOn.ToString("dd MMM yyyy hh:mm tt");
                                BadgeRequestFormData badgeRequestFormData = GetBadgeRequestFormDataObjectToAdd(itemApprovalDateBagdeFormFieldTypeID, currentOrgUserID, item, packageTypeID, recordValue);
                                _dbContext.BadgeRequestFormDatas.AddObject(badgeRequestFormData);
                            }
                            else if (packageTypeID == requirementPackageTypeID)
                            {
                                //Insert new row -- Of Item Modification Date
                                var reqItem = _dbContext.ApplicantRequirementItemDatas.Where(cond => cond.ARID_ID == item.ItemDataId).FirstOrDefault();
                                string recordValue = reqItem.ARID_ModifiedOn.HasValue ? reqItem.ARID_ModifiedOn.Value.ToString("dd MMM yyyy hh:mm tt") : reqItem.ARID_CreatedOn.ToString("dd MMM yyyy hh:mm tt");
                                BadgeRequestFormData badgeRequestFormData = GetBadgeRequestFormDataObjectToAdd(itemApprovalDateBagdeFormFieldTypeID, currentOrgUserID, item, packageTypeID, recordValue);
                                _dbContext.BadgeRequestFormDatas.AddObject(badgeRequestFormData);
                            }
                        }
                    }
                }
                else
                {
                    if (item.IsDataByItemApproval)
                    {
                        string recordValue = DateTime.Now.ToString("dd MMM yyyy hh:mm tt");
                        BadgeRequestFormData badgeRequestFormData = GetBadgeRequestFormDataObjectToAdd(itemApprovalDateBagdeFormFieldTypeID, currentOrgUserID, item, packageTypeID, recordValue);
                        _dbContext.BadgeRequestFormDatas.AddObject(badgeRequestFormData);
                    }
                    else
                    {
                        if (packageTypeID == compliancePackageTypeID)
                        {
                            var complianceItem = _dbContext.ApplicantComplianceItemDatas.Where(cond => cond.ApplicantComplianceItemID == item.ItemDataId).FirstOrDefault();

                            //Insert new row -- Of Item Modification Date
                            string recordValue = complianceItem.ModifiedOn.HasValue ? complianceItem.ModifiedOn.Value.ToString("dd MMM yyyy hh:mm tt") : complianceItem.CreatedOn.ToString("dd MMM yyyy hh:mm tt");
                            BadgeRequestFormData badgeRequestFormData = GetBadgeRequestFormDataObjectToAdd(itemApprovalDateBagdeFormFieldTypeID, currentOrgUserID, item, packageTypeID, recordValue);
                            _dbContext.BadgeRequestFormDatas.AddObject(badgeRequestFormData);
                        }
                        else if (packageTypeID == requirementPackageTypeID)
                        {
                            //Insert new row -- Of Item Modification Date
                            var reqItem = _dbContext.ApplicantRequirementItemDatas.Where(cond => cond.ARID_ID == item.ItemDataId).FirstOrDefault();
                            string recordValue = reqItem.ARID_ModifiedOn.HasValue ? reqItem.ARID_ModifiedOn.Value.ToString("dd MMM yyyy hh:mm tt") : reqItem.ARID_CreatedOn.ToString("dd MMM yyyy hh:mm tt");
                            BadgeRequestFormData badgeRequestFormData = GetBadgeRequestFormDataObjectToAdd(itemApprovalDateBagdeFormFieldTypeID, currentOrgUserID, item, packageTypeID, recordValue);
                            _dbContext.BadgeRequestFormDatas.AddObject(badgeRequestFormData);
                        }
                    }
                }
            }

            if (_dbContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        private static BadgeRequestFormData GetBadgeRequestFormDataObjectToAdd(Int32 itemApprovalDateBagdeFormFieldTypeID, Int32 currentOrgUserID, BadgeFormNotificationDataContract item, Int32 packageTypeID, string recordValue)
        {
            BadgeRequestFormData badgeRequestFormData = new BadgeRequestFormData();
            badgeRequestFormData.BRFD_RecordID = item.ItemDataId;
            badgeRequestFormData.BRFD_RecordValue = recordValue;
            badgeRequestFormData.BRFD_PackageTypeID = packageTypeID;
            badgeRequestFormData.BRFD_RecordTypeID = itemApprovalDateBagdeFormFieldTypeID;
            badgeRequestFormData.BRFD_CreatedBy = currentOrgUserID;
            badgeRequestFormData.BRFD_CreatedOn = DateTime.Now;
            badgeRequestFormData.BRFD_BadgeFormNotificationDataID = item.BadgeFormNotificationID;

            if (item.ProfileSharingInvitationGroupID != 0)
                badgeRequestFormData.BRFD_PSIG_ID = item.ProfileSharingInvitationGroupID;

            badgeRequestFormData.BRFD_OrgUserID = item.AppOrgUserID;
            return badgeRequestFormData;
        }

        //List<RequirementPackageSubscriptionApprovedContract> IApplicantRequirementRepository.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(String requirementPackageSubscriptionIDs)
        //{

        //    List<RequirementPackageSubscriptionApprovedContract> contractList = new List<RequirementPackageSubscriptionApprovedContract>();
        //    EntityConnection connection = _dbContext.Connection as EntityConnection;
        //    int agencyId;
        //    int clinicalRotationID;
        //    string hierarchyNodeIDs = string.Empty;
        //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    {
        //        SqlParameter[] sqlParameterCollection = new SqlParameter[]
        //        {
        //            new SqlParameter("@RPSID", requirementPackageSubscriptionIDs),
        //        };

        //        base.OpenSQLDataReaderConnection(con);
        //        using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetApprovedSubscrptionByRequirementPackageSubscriptionIDs", sqlParameterCollection))
        //        {
        //            if (dr.HasRows)
        //            {
        //                while (dr.Read())
        //                {
        //                    clinicalRotationID = Convert.ToInt32(dr["CR_ID"]);
        //                    agencyId = Convert.ToInt32(dr["CRA_AgencyID"]);
        //                    hierarchyNodeIDs = dr["HierarchyNodeIDs"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNodeIDs"]);

        //                    var contract = new RequirementPackageSubscriptionApprovedContract()
        //                    {
        //                        ClinicalRotationID = clinicalRotationID,
        //                        AgencyID = agencyId,
        //                        HierarchyNodeIDs = hierarchyNodeIDs
        //                    };
        //                    contractList.Add(contract);
        //                }
        //            }
        //        }

        //        base.CloseSQLDataReaderConnection(con);
        //    }
        //    return contractList;
        //}
        DataTable IApplicantRequirementRepository.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(String requirementPackageSubscriptionIDs)
        {
            int agencyId;
            int clinicalRotationID;
            string hierarchyNodeIDs = string.Empty;

            DataTable contractList = new DataTable();

            DataColumn dc = new DataColumn("CR_ID", typeof(Int32));
            contractList.Columns.Add(dc);

            dc = new DataColumn("CRA_AgencyID", typeof(Int32));
            contractList.Columns.Add(dc);

            dc = new DataColumn("HierarchyNodeIDs", typeof(String));
            contractList.Columns.Add(dc);

            DataRow contractRow;
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
               {
                    new SqlParameter("@RPSID", requirementPackageSubscriptionIDs),
               };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetApprovedSubscrptionByRequirementPackageSubscriptionIDs", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clinicalRotationID = Convert.ToInt32(dr["CR_ID"]);
                            agencyId = Convert.ToInt32(dr["CRA_AgencyID"]);
                            hierarchyNodeIDs = dr["HierarchyNodeIDs"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNodeIDs"]);

                            contractRow = contractList.NewRow();
                            contractRow["CR_ID"] = clinicalRotationID;
                            contractRow["CRA_AgencyID"] = agencyId;
                            contractRow["HierarchyNodeIDs"] = hierarchyNodeIDs;
                            contractList.Rows.Add(contractRow);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return contractList;
        }


        String IApplicantRequirementRepository.GetRequirementPackageSubscriptionIdsByPackageID(String requirementPackageId)
        {
            List<Int32> lstPackageIds = requirementPackageId.Split(',').Select(Int32.Parse).ToList();
            List<Int32> lstRequirementPackageSubscriptionIds = _dbContext.RequirementPackageSubscriptions.Where(cond => !cond.RPS_IsDeleted && cond.RPS_RequirementPackageID != null && lstPackageIds.Contains(cond.RPS_RequirementPackageID.Value) && !cond.RequirementPackage.RP_IsDeleted).Select(sel => sel.RPS_ID).ToList();

            if (lstRequirementPackageSubscriptionIds.IsNullOrEmpty())
                return String.Empty;
            return String.Join(",", lstRequirementPackageSubscriptionIds);
        }

        #region UAT-4015
        List<RequirementPackageSubscriptionStatusContract> IApplicantRequirementRepository.GetInstPrecepRPSData(Int32 rpsId)
        {
            List<RequirementPackageSubscriptionStatusContract> lstReqPackageSubscription = new List<RequirementPackageSubscriptionStatusContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@RPSID", rpsId),
                };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetInstPrecRPSData", sqlParameterCollection))
                {
                    RequirementPackageSubscriptionStatusContract reqPackageSubscription = null;
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            reqPackageSubscription = new RequirementPackageSubscriptionStatusContract();
                            reqPackageSubscription.RequirementPackageSubscriptionID = dr["RequirementPackageSubscriptionID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementPackageSubscriptionID"]);
                            reqPackageSubscription.PackageName = dr["PackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PackageName"]);
                            reqPackageSubscription.ApplicantName = dr["ApplicantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ApplicantName"]);
                            reqPackageSubscription.UserName = dr["UserName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UserName"]);
                            reqPackageSubscription.Email = dr["Email"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Email"]);
                            reqPackageSubscription.OrganizationUserID = dr["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrganizationUserID"]);
                            reqPackageSubscription.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                            reqPackageSubscription.ClinicalRotationSubscriptionID = dr["ClinicalRotationSubscriptionID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ClinicalRotationSubscriptionID"]);
                            lstReqPackageSubscription.Add(reqPackageSubscription);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstReqPackageSubscription;
        }

        #endregion
        Int32 IApplicantRequirementRepository.GetApplicantRequirementFieldData(Int32 RequirementItemDataID, Int32 RequirementFieldID)
        {
            //ApplicantRequirementFieldDataContract applicantRequirementFieldDataContract = new ApplicantRequirementFieldDataContract();

            // List<ApplicantRequirementFieldData> applicantRequirementFieldDatas = 

            return _dbContext.ApplicantRequirementFieldDatas.Where(cond => !cond.ARFD_IsDeleted && cond.ARFD_RequirementItemDataID == RequirementItemDataID && cond.ARFD_RequirementFieldID == RequirementFieldID).Select(x => x.ARFD_ID).FirstOrDefault();

            //applicantRequirementFieldDatas.ForEach(x =>
            //   {
            //       ApplicantRequirementFieldDataContract contractData = new ApplicantRequirementFieldDataContract();
            //       contractData.ApplicantReqFieldDataID = x.ARFD_ID;
            //       contractData.RequirementItemDataID = x.ARFD_RequirementItemDataID;
            //       contractData.RequirementFieldID = x.ARFD_RequirementFieldID;
            //       contractData.FieldValue = x.ARFD_FieldValue;
            //       applicantRequirementFieldDataContract = contractData;
            //   });

            //return applicantRequirementFieldDataContract;


        }

        #region UAT-4254

        List<RequirementCategoryDocLink> IApplicantRequirementRepository.GetRequirementCatDocUrls(Int32 requirementCategoryId)
        {
            return _dbContext.RequirementCategoryDocLinks.Where(c => c.RCDL_RequirementCategoryID == requirementCategoryId && !c.RCDL_IsDeleted).ToList();
        }
        #endregion
    }
}
