using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
//using ExternalVendors.ClearstarWebCCF;
using Business.RepoManagers;
using INTSOF.Utils;
using Entity.ClientEntity;
using System.IO;
using INTSOF.UI.Contract;
using INTSOF.ServiceUtil;
using Business.Interfaces;

namespace ExternalVendors.ClearStarVendor
{
    public class ClearStarCCF : IClearStarCCF
    {

        public Boolean IsUseClearstarTls_1_2 = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["UseClearstarTls_1_2"])
                                                            ? Convert.ToBoolean(ConfigurationManager.AppSettings["UseClearstarTls_1_2"]) : false;
        public ClearStarCCF()
        {
            if (IsUseClearstarTls_1_2)
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            }
        }

        #region Methods

        #region Public Methods
        /// <summary>
        /// Method that get the donor CCF data from clear star web CCF
        /// </summary>
        /// <param name="boID">boID</param>
        /// <param name="custID">custID</param>
        /// <param name="userName">userName</param>
        /// <param name="password">password</param>
        /// <param name="donorRegistrationID">donorRegistrationID</param>
        /// <returns>GetDonorCCF</returns>
        public GetDonorCCF GetClearstarDonorCCF(Int32 boID, String custID, String userName, String password, String donorRegistrationID)
        {
            //ClearstarWebCCF.DonorCCF donorCCFWS = new ClearstarWebCCF.DonorCCF();
            ClearstarClearMD.DonorCCF donorCCFWS = new ClearstarClearMD.DonorCCF();

            donorCCFWS.EnableDecompression = true;
            GetDonorCCF donorCCFData = null;
            XmlNode xmlNodeCreateResult = null;
            XmlNodeReader xmlNodeReaderCreateResult = null;
            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            XmlSerializer getDonorCCFSerializer = new XmlSerializer(typeof(GetDonorCCF));

            try
            {
                //Get Order from Clearstar Gateway
                xmlNodeCreateResult = donorCCFWS.GetDonorCCF(boID, custID, userName, password, donorRegistrationID);

                xmlNodeReaderCreateResult = new XmlNodeReader(xmlNodeCreateResult);
                donorCCFData = (GetDonorCCF)getDonorCCFSerializer.Deserialize(xmlNodeReaderCreateResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return donorCCFData;
        }

        /// <summary>
        /// Method that return the Clear Star Donor CCF PDF
        /// </summary>
        /// <param name="boID">boID</param>
        /// <param name="custID">custID</param>
        /// <param name="userName">userName</param>
        /// <param name="password">password</param>
        /// <param name="donorRegistrationID">donorRegistrationID</param>
        /// <returns>GetCCF</returns>
        public GetCCF GetClearstarDonorCCFPDF(Int32 boID, String custID, String userName, String password, String donorRegistrationID)
        {
            //ClearstarWebCCF.DonorCCF donorCCFWS = new ClearstarWebCCF.DonorCCF();
            ClearstarClearMD.DonorCCF donorCCFWS = new ClearstarClearMD.DonorCCF();

            donorCCFWS.EnableDecompression = true;
            GetCCF donorCCFPDF = null;
            XmlNode xmlNodeCreateResult = null;
            XmlNodeReader xmlNodeReaderCreateResult = null;
            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            XmlSerializer getDonorCCFSerializer = new XmlSerializer(typeof(GetCCF));

            try
            {
                //Get Order from Clearstar Gateway
                xmlNodeCreateResult = donorCCFWS.GetCCFPDF(boID, custID, userName, password, donorRegistrationID);

                xmlNodeReaderCreateResult = new XmlNodeReader(xmlNodeCreateResult);
                donorCCFPDF = (GetCCF)getDonorCCFSerializer.Deserialize(xmlNodeReaderCreateResult);
            }
            catch
            {
                throw;
            }

            return donorCCFPDF;
        }

        public void SaveCCFDataAndPDF(Dictionary<String, Object> dicParam)
        {
            Int32 BkgOrderId;
            Int32 tenantId;
            Int32 extVendorId;
            List<Int32> BPHMId_List;
            String registrationId;
            Int32 currentLoggedInUserId;
            Int32 organizationUserId;
            Int32 organizationUserProfileId;
            String applicantName;
            String primaryEmailAddress;
            Int32 hierarchyNodeId;
            if (dicParam.IsNotNull())
            {
                registrationId = dicParam.GetValue("RegistrationId") as String;
                dicParam.TryGetValue("CurrentLoggedInUserId", out currentLoggedInUserId);
                dicParam.TryGetValue("TenantId", out tenantId);
                dicParam.TryGetValue("ExtVendorId", out extVendorId);
                dicParam.TryGetValue("BkgOrderId", out BkgOrderId);
                dicParam.TryGetValue("OrganizationUserId", out organizationUserId);
                dicParam.TryGetValue("OrganizationUserProfileId", out organizationUserProfileId);
                dicParam.TryGetValue("ApplicantName", out applicantName);
                dicParam.TryGetValue("PrimaryEmailAddress", out primaryEmailAddress);
                dicParam.TryGetValue("HierarchyNodeId", out hierarchyNodeId);
                BPHMId_List = dicParam.GetValue("BPHMId_List") as List<Int32>;
                GetEDSCustomFormDataAndPDFFromCCF(BkgOrderId, tenantId, extVendorId, BPHMId_List, registrationId, currentLoggedInUserId, organizationUserId, organizationUserProfileId, applicantName, primaryEmailAddress, hierarchyNodeId);
            }

        }

        /// <summary>
        /// Method to get the custom form order data fields values from DonorCCF.
        /// </summary>
        public void GetEDSCustomFormDataAndPDFFromCCF(Int32 BkgOrderId, Int32 tenantId, Int32 extVendorId, List<Int32> BPHMIdList, String registrationId, Int32 currentLoggedInUserId, Int32 organizationUserId, Int32 organizationUserProfileId, String applicantName, String primaryEmailAddress, Int32 hierarchyNodeId)
        {
            ClearStarWebCCFContract clearStarWebCCFContract = new ClearStarWebCCFContract();
            String vendorAccountNumber = String.Empty;
            //Int32 BkgOrderId = BackgroundProcessOrderManager.GetBkgOrderIdByOrderID(tenantId, orderId);
            vendorAccountNumber = BackgroundProcessOrderManager.GetVendorAccountNumber(tenantId, extVendorId, hierarchyNodeId);
            String eDrugScrnAttributeGrpCode = AppConsts.ELECTRONIC_DRUG_SCREEN_ATT_GROUP_CODE;
            CustomFormDataGroup customFormDataFroup = BackgroundProcessOrderManager.GetCustomFormDataGroupForEDSData(tenantId, BkgOrderId, eDrugScrnAttributeGrpCode);
            if (registrationId.IsNullOrEmpty() || (String.Compare(registrationId, AppConsts.NONE.ToString()) == 0))
            {
                Guid attRegistrationIdNameCode = new Guid(AppConsts.DRUG_SCREEN_REGISTRATION_ID);
                registrationId = customFormDataFroup.CustomFormOrderDatas.FirstOrDefault(cond => cond.BkgAttributeGroupMapping.BAGM_Code == attRegistrationIdNameCode && cond.CFOD_IsDeleted == false).CFOD_Value;
            }
            List<BkgAttributeGroupMapping> lstBkgattributeGroupMapping = BackgroundProcessOrderManager.GetListBkgAttributeGroupMappingForEDrug(tenantId, eDrugScrnAttributeGrpCode);
            List<CustomFormOrderData> lstCustomFormOrderData = new List<CustomFormOrderData>();
            GetDonorCCF donorCCFData = null;
            ApplicantDocument applicantDocument = null;
            //Get ApplicantDocument
            applicantDocument = GetAppDocumentObJWithCCFPdf(tenantId, vendorAccountNumber, registrationId, currentLoggedInUserId, organizationUserId, organizationUserProfileId);

            //Get ClearStarCCF
            donorCCFData = GetClearstarDonorCCF(Convert.ToInt32(ConfigurationManager.AppSettings["ClearstarBOID"]), vendorAccountNumber,
                ConfigurationManager.AppSettings["ClearstarUserName"], ConfigurationManager.AppSettings["ClearstarPassword"], registrationId);
            //UAT-5022 
            Boolean sendInstructionAsAttachment = false;

            //UAT-5114
            Boolean isAdminEntryOrder = BackgroundProcessOrderManager.CheckIfOrderIsAdminEntryOrder(tenantId, BkgOrderId);

            if (donorCCFData != null)
            {
                //Default value for Distance From Lab attribute.
                String disFromLabDefaultVal = "35";
                String reasonCode = String.Empty;
                String state = String.Empty;
                String country = String.Empty;
                String county = String.Empty;
                Entity.ZipCode zipCodeObj = BackgroundProcessOrderManager.GetZipCodeObjByZipCode(tenantId, donorCCFData.SiteZipCode);
                if (zipCodeObj.IsNotNull())
                {
                    country = zipCodeObj.County.State.Country.FullName;
                    county = zipCodeObj.County.CountyName;
                }
                state = BackgroundProcessOrderManager.GetStateNameByAbbreviation(tenantId, donorCCFData.SiteState);
                if (donorCCFData.LabName == "Quest")
                {
                    reasonCode = "PRE";
                }
                else
                {
                    reasonCode = "PE";
                    //UAT-5022
                    if (String.Compare(donorCCFData.LabName, "LabCorp", true) != 0)
                    {
                        sendInstructionAsAttachment = true;
                    }
                }
                for (Int32 count = 0; count <= lstBkgattributeGroupMapping.Count() - 1; count++)
                {
                    switch (Convert.ToString(lstBkgattributeGroupMapping[count].BAGM_Code).ToUpper())
                    {
                        case AppConsts.LAB_NAME:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, donorCCFData.LabName, currentLoggedInUserId));
                            break;
                        case AppConsts.REGISTRATION_EXPIRATION_DATE:
                            if (!isAdminEntryOrder)
                            {
                                lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, donorCCFData.ExpirationDate, currentLoggedInUserId));
                            }                            
                            break;
                        case AppConsts.SITE_ADDRESS:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, donorCCFData.SiteAddress, currentLoggedInUserId));
                            break;
                        case AppConsts.SITE_NAME:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, donorCCFData.SiteName, currentLoggedInUserId));
                            break;
                        case AppConsts.SITE_PHONE_NUMBER:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, donorCCFData.SitePhone, currentLoggedInUserId));
                            break;
                        case AppConsts.POSTAL_CODE:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, donorCCFData.SiteZipCode, currentLoggedInUserId));
                            break;
                        //Not Geting from DonorCCF then set default value as 35
                        case AppConsts.DISTANCE_FROM_LAB:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, disFromLabDefaultVal, currentLoggedInUserId));
                            break;
                        case AppConsts.SITE_ID:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, donorCCFData.Id, currentLoggedInUserId));
                            break;
                        case AppConsts.LC_PANEL_ID:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, donorCCFData.PanelID, currentLoggedInUserId));
                            break;
                        case AppConsts.DONOR_HOME_PHONE_NUMBER:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, donorCCFData.DonorHomePhone, currentLoggedInUserId));
                            break;
                        case AppConsts.DONOR_WORK_PHONE_NUMBER:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, donorCCFData.DonorWorkPhone, currentLoggedInUserId));
                            break;
                        case AppConsts.REASON_CODE:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, reasonCode, currentLoggedInUserId));
                            break;
                        case AppConsts.REASON_DESCRIPTION:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, donorCCFData.TestReason, currentLoggedInUserId));
                            break;
                        case AppConsts.EXPIRATION_DATE:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, donorCCFData.ExpirationDate, currentLoggedInUserId));
                            break;
                        case AppConsts.COUNTRY_EDRUG:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, country, currentLoggedInUserId));
                            break;
                        case AppConsts.STATE_EDRUG:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, state, currentLoggedInUserId));
                            break;
                        case AppConsts.CITY_EDRUG:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, donorCCFData.SiteCity, currentLoggedInUserId));
                            break;
                        case AppConsts.COUNTY_EDRUG:
                            lstCustomFormOrderData.Add(GetCustomFormOrderDataObject(customFormDataFroup, lstBkgattributeGroupMapping[count].BAGM_ID, county, currentLoggedInUserId));
                            break;
                    }
                }
            }
            clearStarWebCCFContract.ListCustomFormOrderData = lstCustomFormOrderData;
            clearStarWebCCFContract.ApplicantDocumentCCFData = applicantDocument;
            //return clearStarWebCCFContract;
            BackgroundProcessOrderManager.SaveWebCCFPDFDocument(tenantId, clearStarWebCCFContract.ApplicantDocumentCCFData);
            if (BackgroundProcessOrderManager.SaveCustomFormOrderDataForEDrug(tenantId, clearStarWebCCFContract.ListCustomFormOrderData))
                BackgroundProcessOrderManager.UpdateBkgOrderSvcLineItem(tenantId, Convert.ToInt32(extVendorId), BkgOrderId, SvcLineItemDispatchStatus.NOT_DISPATCHED.GetStringValue(), currentLoggedInUserId);

            SendDefaultEDSForm(clearStarWebCCFContract.ApplicantDocumentCCFData, organizationUserId, applicantName, primaryEmailAddress, currentLoggedInUserId, tenantId, hierarchyNodeId, 
                               donorCCFData.HtmlBody,BkgOrderId, sendInstructionAsAttachment);
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Method that return custom form order data that is field by donor CCf data.
        /// </summary>
        /// <param name="customFormDataGrpObj">customFormDataGrpObj</param>
        /// <param name="bkgAttGroupMappingId">bkgAttGroupMappingId</param>
        /// <param name="value">value</param>
        /// <param name="currentLoggedInuserId">currentLoggedInuserId</param>
        /// <returns>CustomFormOrderData</returns>
        private CustomFormOrderData GetCustomFormOrderDataObject(CustomFormDataGroup customFormDataGrpObj, Int32 bkgAttGroupMappingId, String value, Int32 currentLoggedInuserId)
        {
            CustomFormOrderData _customFormOrderData = new CustomFormOrderData
            {
                CFOD_CustomFormDataGroupID = customFormDataGrpObj.CFDG_ID,
                CFOD_BkgAttributeGroupMappingID = bkgAttGroupMappingId,
                CFOD_Value = value.IsNullOrEmpty() ? null : value,
                CFOD_IsDeleted = false,
                CFOD_ModifiedBy = currentLoggedInuserId,
                CFOD_ModifiedOn = DateTime.Now
            };
            return _customFormOrderData;
        }

        /// <summary>
        /// method that used to get the ApplicantDocument object to save.
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="vendorAccountNumber">vendorAccountNumber</param>
        /// <param name="registrationId">registrationId</param>
        /// <param name="currentLoggedInUserId">currentLoggedInUserId</param>
        /// <param name="bkgOrderObj">bkgOrderObj</param>
        /// <returns>ApplicantDocument</returns>
        private ApplicantDocument GetAppDocumentObJWithCCFPdf(Int32 tenantId, String vendorAccountNumber, String registrationId, Int32 currentLoggedInUserId, Int32 organizationUserId, Int32 organizationUserProfileId)
        {
            GetCCF donorCCfPDF = null;
            String documentPath = String.Empty;
            Int32? fileSize = null;
            String EDSDocumentTypeCode = DocumentType.EDS_AuthorizationForm.GetStringValue();
            Entity.SystemDocument EDSDefaultAuthorizationForm = new Entity.SystemDocument();
            ApplicantDocument applicantDocument = null;
            //Get webCCFDonor PDF
            donorCCfPDF = GetClearstarDonorCCFPDF(Convert.ToInt32(ConfigurationManager.AppSettings["ClearstarBOID"]), vendorAccountNumber,
                ConfigurationManager.AppSettings["ClearstarUserName"], ConfigurationManager.AppSettings["ClearstarPassword"],
                registrationId);
            if (donorCCfPDF != null)
            {
                byte[] donorCCfPDFBytes;
                if (!String.IsNullOrWhiteSpace(donorCCfPDF.PdfFile))
                {
                    donorCCfPDFBytes = System.Convert.FromBase64String(donorCCfPDF.PdfFile);
                    //System.IO.File.WriteAllBytes(@"D:\DonorCCFPDF.pdf", donorCCfPDFBytes);
                    //documentPath = SaveDonorCCFPDF(donorCCfPDFBytes, tenantId, currentLoggedInUserId, Convert.ToInt32(registrationId));
                    documentPath = SaveDonorCCFPDF(donorCCfPDFBytes, tenantId, organizationUserId, registrationId);
                    fileSize = donorCCfPDFBytes.Length;
                }
                else
                {
                    //set the default eds registration form when document didn't come back from CCF
                    Int32 EDSDefaultdocTypeId = BackgroundSetupManager.GetDocumentTypeIDByCode(DislkpDocumentType.EDS_DEFAULT_AUTHORIZATION_FORM.GetStringValue());
                    EDSDefaultAuthorizationForm = BackgroundSetupManager.GetDisclosureTemplateDocuments(EDSDefaultdocTypeId).FirstOrDefault();
                    if (!EDSDefaultAuthorizationForm.IsNullOrEmpty())
                    {
                        documentPath = EDSDefaultAuthorizationForm.DocumentPath;
                        fileSize = 1291;//Default Size of EDS Authorization form.
                    }
                }

                applicantDocument = new ApplicantDocument();
                String recordTypeCode = RecordType.BackgroundProfile.GetStringValue();
                applicantDocument.FileName = "EDS_AuthorizationForm_" + registrationId.ToString()+".pdf";
                applicantDocument.OrganizationUserID = organizationUserId;
                applicantDocument.Description = "Electronic Drug Screening authorization form";
                applicantDocument.IsDeleted = false;
                applicantDocument.CreatedByID = currentLoggedInUserId;
                applicantDocument.CreatedOn = DateTime.Now;
                applicantDocument.DocumentType = ComplianceDataManager.GetlkpDocumentType(tenantId).FirstOrDefault(cond => cond.DMT_Code == EDSDocumentTypeCode).DMT_ID;
                applicantDocument.DocumentPath = documentPath;
                applicantDocument.Size = fileSize;

                GenericDocumentMapping genericDocumentMapping = new GenericDocumentMapping();
                genericDocumentMapping.GDM_RecordID = organizationUserProfileId;
                genericDocumentMapping.GDM_RecordTypeID = BackgroundProcessOrderManager.GetlkpRecordTypeIdByCode(tenantId, recordTypeCode);
                genericDocumentMapping.GDM_IsDeleted = false;
                genericDocumentMapping.GDM_CreatedBy = currentLoggedInUserId;
                genericDocumentMapping.GDM_CreatedOn = DateTime.Now;
                applicantDocument.GenericDocumentMappings.Add(genericDocumentMapping);
            }
            return applicantDocument;
        }

        /// <summary>
        /// Method that save the docor ccf pdf file and return the path where this file is saved.
        /// </summary>
        /// <param name="pdfBytes">Pdf byte array</param>
        /// <param name="tenantId">tenant id</param>
        /// <param name="currentLoggedInUserId">current loggedInuserID</param>
        /// <param name="registrationId">registration id for EDS</param>
        /// <returns>document path</returns>
        private String SaveDonorCCFPDF(byte[] pdfBytes, Int32 tenantId, Int32 currentLoggedInUserId, String registrationId)
        {
            String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
            String applicantFileLocation = String.Empty;

            if (tempFilePath.IsNullOrEmpty())
            {
                return String.Empty;
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "Tenant(" + tenantId.ToString() + @")\";

            if (!Directory.Exists(tempFilePath))
                Directory.CreateDirectory(tempFilePath);

            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
            String destFileName = "EDS_" + registrationId + "_" + tenantId.ToString() + "_" + currentLoggedInUserId + "_" + date + ".pdf";
            String newTempFilePath = Path.Combine(tempFilePath, destFileName);
            String desFilePath = "Tenant(" + tenantId.ToString() + @")\" + destFileName;
            String newFinalFilePath = String.Empty;
            FileStream _FileStream = null;
            try
            {
                _FileStream = new FileStream(newTempFilePath,
                            System.IO.FileMode.Create,
                            System.IO.FileAccess.Write);
                _FileStream.Write(pdfBytes, 0, pdfBytes.Length);
                try
                {
                    _FileStream.Close();
                    newFinalFilePath = CommonFileManager.SaveDocument(newTempFilePath, desFilePath, FileType.ApplicantFileLocation.GetStringValue());
                    File.Delete(newTempFilePath);
                }
                catch (Exception) { }
                return newFinalFilePath;
            }
            catch (Exception ex)
            {
                return newFinalFilePath;
            }
            finally
            {
                try { _FileStream.Close(); }
                catch (Exception) { }
            }
        }

        private Boolean SendDefaultEDSForm(ApplicantDocument applicantDocument, Int32 organizationUserId, String applicantName, String primaryEmailAddress, Int32 currentLoggedInUserId, 
                                           Int32 tenantId, Int32 hierarchyNodeID, String html_Body,Int32 bkgOrderID, Boolean sendInstructionAsAttachment)
        {
            //UAT-5022
            if (html_Body.IsNullOrEmpty() || sendInstructionAsAttachment)
            {
                html_Body = String.Empty;
            }
            //UAT-2154
            Int32 orderID = 0;
            BkgOrder bkgOrder = BackgroundProcessOrderManager.GetBkgOrderByBkgOrderId(tenantId, bkgOrderID);
            if (!bkgOrder.IsNullOrEmpty())
            {
                orderID = bkgOrder.BOR_MasterOrderID;
            }
            List<Entity.lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();

            String docAttachmentTypeCode = DocumentAttachmentType.APPLICANT_DOCUMENT.GetStringValue();
            Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);

            //Create Dictionary for Mail And Message Data
            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
            dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, applicantName);
            dictMailData.Add(EmailFieldConstants.EDS_HTML_BODY, html_Body);

            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
            mockData.UserName = applicantName;
            mockData.EmailID = primaryEmailAddress;
            mockData.ReceiverOrganizationUserID = organizationUserId;

            //Send mail
            Int32? systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_ELECTRONIC_DRUG_SCREENING_INSTRUCTIONS, dictMailData, mockData, tenantId, hierarchyNodeID, null);

            //Save Mail Attachment
            if (applicantDocument.IsNotNull() && systemCommunicationID.IsNotNull() && html_Body.IsNullOrEmpty())
            {
                Entity.SystemCommunicationAttachment sysCommAttachment = new Entity.SystemCommunicationAttachment();
                sysCommAttachment.SCA_OriginalDocumentID = applicantDocument.ApplicantDocumentID;
                sysCommAttachment.SCA_OriginalDocumentName = applicantDocument.FileName;
                sysCommAttachment.SCA_DocumentPath = applicantDocument.DocumentPath;
                if (applicantDocument.Size.HasValue)
                    sysCommAttachment.SCA_DocumentSize = applicantDocument.Size.Value;
                sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                sysCommAttachment.SCA_IsDeleted = false;
                sysCommAttachment.SCA_CreatedBy = currentLoggedInUserId;
                sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                sysCommAttachment.SCA_ModifiedBy = null;
                sysCommAttachment.SCA_ModifiedOn = null;

                Int32 sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
            }

            //UAT-2154:Add EDrug forms and their statuses to the order details tab for ADB Admins

            List<Entity.ClientEntity.lkpBusinessChannelType> businessChannelType = BackgroundProcessOrderManager.GetBusinessChannelType(tenantId);

            String amsBusinessChannelCode = BusinessChannelType.AMS.GetStringValue();
            Int16 businessChannelTypeID = businessChannelType.IsNotNull() && businessChannelType.Count > 0 ?
                Convert.ToInt16(businessChannelType.FirstOrDefault(cond => cond.Code == amsBusinessChannelCode).BusinessChannelTypeID) : Convert.ToInt16(0);

            List<Entity.ClientEntity.lkpOrderNotificationType> orderNotificationType = BackgroundProcessOrderManager.GetOrderNotificationType(tenantId);

            String EDSNotificationTypeCode = OrderNotificationType.EDS_NOTIFICATION.GetStringValue();
            Int32 EDSNotificationTypeID = orderNotificationType.IsNotNull() && orderNotificationType.Count > 0 ?
                Convert.ToInt32(orderNotificationType.FirstOrDefault(cond => cond.ONT_Code == EDSNotificationTypeCode).ONT_ID) : Convert.ToInt32(0);


            OrderNotification newOrdNotification = new OrderNotification();
            newOrdNotification.ONTF_OrderID = orderID;
            newOrdNotification.ONTF_MSG_SystemCommunicationID = systemCommunicationID;
            newOrdNotification.ONTF_MSG_MessageID = null;
            newOrdNotification.ONTF_BusinessChannelTypeID = businessChannelTypeID;
            newOrdNotification.ONTF_IsPostal = false;
            newOrdNotification.ONTF_CreatedByID = currentLoggedInUserId;
            newOrdNotification.ONTF_CreatedOn = DateTime.Now;
            newOrdNotification.ONTF_ModifiedByID = null;
            newOrdNotification.ONTF_ModifiedDate = null;
            newOrdNotification.ONTF_ParentNotificationID = null;
            newOrdNotification.ONTF_OrderNotificationTypeID = EDSNotificationTypeID;
            newOrdNotification.ONTF_NotificationDetail = "E-DRUG Notification";
            newOrdNotification.ONTF_ParentNotificationID = null;
            Int32 newOrdNotificationID = BackgroundProcessOrderManager.CreateOrderNotification(tenantId, newOrdNotification);
            return true;
        }

        #endregion

        #endregion
    }
}
