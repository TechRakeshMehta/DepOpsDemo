using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.BkgOperations.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System.Configuration;
using System.IO;
using System.Text;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class RequiredDocumentationLoader : BaseUserControl, IRequiredDocumentationLoaderView
    {
        #region VARIABLES

        private RequiredDocumentationLoaderPresenter _presenter = new RequiredDocumentationLoaderPresenter();
        private ApplicantOrderCart _applicantOrderCart;
        private ApplicantOrderCart applicantOrderCart = new ApplicantOrderCart();
        private Boolean isSubmitted = false;
        #endregion

        #region PROPERTIES

        public int? HierarchyNodeID
        {
            get;
            set;
        }


        public int? ServiceID
        {
            get;
            set;
        }

        public List<Entity.SystemDocument> AdditionalDocuments
        {
            get;
            set;
        }


        public List<int> lstServiceIds
        {
            get;
            set;
        }

        public string BkgServiceIds
        {
            get;
            set;
        }


        public int TenantID
        {
            get;
            set;
        }

        public Dictionary<int, string> DictAttributeGroupIDs
        {
            get;
            set;
        }

        public List<SysDocumentFieldMappingContract> DocumentAttributeMappingList
        {
            get;
            set;
        }

        public List<SysDocumentFieldMappingContract> AdditionalDocumentList
        {
            get
            {
                if (ViewState["AdditionalDocumentList"].IsNotNull())
                {
                    return ViewState["AdditionalDocumentList"] as List<SysDocumentFieldMappingContract>;
                }
                else
                {
                    return new List<SysDocumentFieldMappingContract>();
                }
            }
            set
            {
                ViewState["AdditionalDocumentList"] = value;
            }
        }


        public List<SysDocumentFieldMappingContract> LstSpecialFields
        {
            get
            {
                if (ViewState["LstSpecialFields"].IsNotNull())
                {
                    return ViewState["LstSpecialFields"] as List<SysDocumentFieldMappingContract>;
                }
                else
                {
                    return new List<SysDocumentFieldMappingContract>();
                }
            }
            set
            {
                ViewState["LstSpecialFields"] = value;
            }
        }

        public String PackageName
        {
            get;
            set;
        }

        public Int32 CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Boolean IsDiclosureSubmitted
        {
            get
            {
                return isSubmitted;
            }
            set
            {
                isSubmitted = value;
            }
        }

        public Boolean IsErrorOccured
        {
            get;
            set;
        }

        public Boolean RestrictOrderFlow
        {
            get;
            set;
        }

        public bool LoadSignedPdfs
        {
            set
            {
                if (value)
                {
                    IsDiclosureSubmitted = value;
                    rptAdditionalDocuments.DataSource = AdditionalDocumentList;
                    rptAdditionalDocuments.DataBind();
                }
            }
        }

        //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
        public Int32 OrgUsrID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                else
                    return CurrentLoggedInUserID;
            }
        }

        //public Boolean IsSubscriptionExist
        //{
        //    get
        //    {
        //        if (ViewState["IsSubscriptionExist"].IsNotNull())
        //        {
        //            return Convert.ToBoolean(ViewState["IsSubscriptionExist"]);
        //        }
        //        return false;
        //    }
        //    set
        //    {
        //        ViewState["IsSubscriptionExist"] = value;
        //    }
        //}

        //UAT-3745
        public List<SystemDocBkgSvcMapping> lstServiceDocBkgSvcMapping
        {
            get
            {
                if (!ViewState["lstServiceDocBkgSvcMapping"].IsNullOrEmpty())
                    return (List<SystemDocBkgSvcMapping>)ViewState["lstServiceDocBkgSvcMapping"];
                return new List<SystemDocBkgSvcMapping>();
            }
            set
            {
                ViewState["lstServiceDocBkgSvcMapping"] = value;
            }
        }

        public List<SystemDocBkgSvcMapping> ServiceDocBkgSvcMappingList
        {
            get
            {
                if (!ViewState["ServiceDocBkgSvcMappingList"].IsNullOrEmpty())
                    return ViewState["ServiceDocBkgSvcMappingList"] as List<SystemDocBkgSvcMapping>;
                return new List<SystemDocBkgSvcMapping>();
            }
            set
            {
                ViewState["ServiceDocBkgSvcMappingList"] = value;
            }
        }

        #endregion

        #region PRESENTER
        public RequiredDocumentationLoaderPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }
        #endregion

        #region EVENTS

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                        TenantID = user.TenantId.HasValue ? user.TenantId.Value : 0;

                    if (_applicantOrderCart.IsNull())
                        _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);


                    //Clear previous disclosure document ids
                    _applicantOrderCart.ApplicantAdditionalDocumentIds = null;
                    //if (!_applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
                    //{
                    //Retrieved the profile of User
                    var userProfile = _applicantOrderCart.lstApplicantOrder.FirstOrDefault().OrganizationUserProfile;

                    //Create Full Name of the user
                    StringBuilder fullName = new StringBuilder();
                    fullName.Append(userProfile.FirstName + " ");
                    if (userProfile.MiddleName.IsNotNull())
                        fullName.Append(userProfile.MiddleName + " ");
                    fullName.Append(userProfile.LastName);

                    ////GetApplicant existing subscription
                    //Presenter.IsSubscriptionExistForApplicant(OrgUsrID);
                    //Fetch Additional Documents
                    FetchAdditionalDocuments();

                    //Fetch attributes for D and R Documents
                    Presenter.FetchAdditionalDocumentAttributes(_applicantOrderCart);

                    //Make the list of Documents IDs
                    List<Int32> lstDocIds = AdditionalDocuments.Select(cond => cond.SystemDocumentID).Distinct().ToList();

                    //Create a temporary list to store Special Fields
                    List<SysDocumentFieldMappingContract> _tmpLstSpecialFields = new List<SysDocumentFieldMappingContract>();

                    //Now iterate through list of D and R Documents
                    foreach (var id in lstDocIds)
                    {
                        //Filter the D and R Attributes for current Document
                        List<SysDocumentFieldMappingContract> tempMappings = DocumentAttributeMappingList.Where(cond => cond.ID == id).ToList();

                        foreach (var item in tempMappings)
                        {
                            //Retrive the Special Fields from the D and R Attributes
                            if (item.SpecialFieldTypeCode.IsNotNull())
                            {
                                SysDocumentFieldMappingContract _specialFields = new SysDocumentFieldMappingContract();
                                _specialFields.FieldName = item.FieldName;
                                _specialFields.SpecialFieldTypeName = item.SpecialFieldTypeName;
                                _specialFields.SpecialFieldTypeCode = item.SpecialFieldTypeCode;
                                _specialFields.ID = item.ID;
                                _tmpLstSpecialFields.Add(_specialFields);
                            }
                        }

                        //Assign the Special Fields to the property from temporary list of Special Fields
                        LstSpecialFields = _tmpLstSpecialFields;

                        //Retrieve current Document Path
                        Entity.SystemDocument sysDocument = AdditionalDocuments.Where(cond => cond.SystemDocumentID == id).FirstOrDefault();
                        String documentPath = sysDocument.DocumentPath;

                        //Fill the PDF with Attributes
                        byte[] fileBytes = Presenter.FillAttributesInPdf(documentPath, tempMappings, TenantID, fullName.ToString());

                        //Save the PDF filled with Attributes
                        if (fileBytes.IsNotNull())
                            StoreAttributesFilledPdf(fileBytes, userProfile.FirstName, userProfile.LastName, sysDocument);
                    }

                    //Set the visibility of the D and R Form (i.e Repeater)
                    if (AdditionalDocumentList.Count == AppConsts.NONE)
                    {
                        rptAdditionalDocuments.Visible = false;
                        //RestrictOrderFlow = true;
                    }

                    //Providing the datasource to repeater (BkgDisclosureDocumentList is filled in StoreAttributesFilledPdf method)
                    rptAdditionalDocuments.DataSource = AdditionalDocumentList;
                    rptAdditionalDocuments.DataBind();
                    //}
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                    IsErrorOccured = true;
                    base.ShowErrorMessage(ex.Message);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                    IsErrorOccured = true;
                    base.ShowErrorMessage(ex.Message);
                }
            }
        }

        protected void rptAdditionalDocuments_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Session["tmpDocPaths"] = null;
                //Session["retainFields"] = null;
                SysDocumentFieldMappingContract systemDoc = (SysDocumentFieldMappingContract)e.Item.DataItem;
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user.IsNotNull())
                    TenantID = user.TenantId.HasValue ? user.TenantId.Value : 0;

                System.Web.UI.HtmlControls.HtmlIframe _iframe = e.Item.FindControl("iframePdfDocViewer") as System.Web.UI.HtmlControls.HtmlIframe;
                if (_iframe.IsNotNull())
                {
                    _iframe.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?tenantId={0}&DocumentPath={1}&DocumentType={2}&DNR={3}&FileName={4}&IsDisclosureDocEdit={5}", TenantID, systemDoc.DocumentPath, "DisclosureDocument", "true", systemDoc.FieldName, "true");
                }
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "callDocumentViewerSaveButton();", true);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                IsErrorOccured = true;
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                IsErrorOccured = true;
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #region METHODS

        /// <summary>
        /// Method to fetch Additional Documents.
        /// </summary>
        private void FetchAdditionalDocuments()
        {
            try
            {
                List<Int32> BkgPackages = new List<Int32>();
                List<Int32> compPackageIdList = new List<Int32>();

                //Retrieve Order Cart from Session
                if (_applicantOrderCart.IsNull())
                {
                    _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                }

                //Retrieve TenantID from Session
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user.IsNotNull())
                {
                    TenantID = user.TenantId.HasValue ? user.TenantId.Value : 0;
                }

                var packagesList = _applicantOrderCart.lstApplicantOrder[0].lstPackages;
                if (!packagesList.IsNullOrEmpty())
                {
                    foreach (var item in packagesList)
                    {
                        BkgPackages.Add(item.BPAId);
                    }
                }
                HierarchyNodeID = _applicantOrderCart.SelectedHierarchyNodeID;
                //Get Compliance package Ids.
                if (_applicantOrderCart.IsCompliancePackageSelected)
                {
                    _applicantOrderCart.CompliancePackages.ForEach(cnd =>
                    {
                        if (!cnd.Value.IsNullOrEmpty())
                        {
                            compPackageIdList.Add(cnd.Value.CompliancePackageID);
                        }
                    });
                }
                //Get Additional Documents
                Presenter.GetAdditionalDocuments(BkgPackages, HierarchyNodeID.Value, TenantID, compPackageIdList);

                //UAT-3745 Get Which documents is/are mapped with which service and which package.
                if (!AdditionalDocuments.IsNullOrEmpty())
                {
                    String additionalDocIds = String.Join(",", (AdditionalDocuments.Where(c => !c.IsDeleted).Select(sel => sel.SystemDocumentID).ToList()));
                    if (!additionalDocIds.IsNullOrEmpty() && !BkgPackages.IsNullOrEmpty())
                        Presenter.GetAddtionalDocBkgSvcMapping(BkgPackages, additionalDocIds, TenantID);
                }
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                throw ex;
            }
        }


        /// <summary>
        /// This method read the signature and fill into the pdf
        /// </summary>
        /// <returns></returns>
        private bool StoreAttributesFilledPdf(byte[] fileBytes, String applicantFirstName, String applicantLastName, Entity.SystemDocument sysDocument = null)
        {
            try
            {
                Boolean aWSUseS3 = false;
                if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                {
                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                }
                SaveAdditionalPdfWithAttributes(fileBytes, aWSUseS3, applicantFirstName, applicantLastName, sysDocument);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                throw ex;
            }
            return false;
        }

        /// <summary>
        /// Save the ESigned disclosre document 
        /// </summary>
        /// <param name="pdfBytes">pdfBytes</param>
        /// <returns>Boolean</returns>
        private Boolean SaveAdditionalPdfWithAttributes(byte[] pdfBytes, Boolean aWSUseS3, String applicantFirstName, String applicantLastName, Entity.SystemDocument sysDocument = null)
        {
            String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
            String applicantFileLocation = String.Empty;
            String filename = String.Empty;

            if (tempFilePath.IsNullOrEmpty())
            {
                base.LogError("Please provide path for TemporaryFileLocation in config.", new SystemException());
                throw new SystemException("Please provide path for TemporaryFileLocation in config.");
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "Tenant(" + TenantID.ToString() + @")\";

            if (!Directory.Exists(tempFilePath))
                Directory.CreateDirectory(tempFilePath);

            //Check whether use AWS S3, true if need to use
            if (aWSUseS3 == false)
            {
                applicantFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"];
                if (!applicantFileLocation.EndsWith("\\"))
                {
                    applicantFileLocation += "\\";
                }
                applicantFileLocation += "Tenant(" + TenantID.ToString() + @")\";

                if (!Directory.Exists(applicantFileLocation))
                {
                    Directory.CreateDirectory(applicantFileLocation);
                }
            }

            _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("hhmmss") + DateTime.Now.Millisecond.ToString();
            String destFileName = "RD_" + TenantID.ToString() + "_" + CurrentLoggedInUserID + "_" + date + ".pdf";
            String newTempFilePath = Path.Combine(tempFilePath, destFileName);
            String newFinalFilePath = String.Empty;

            filename = GetFileName(sysDocument.FileName) + "_" + applicantFirstName + "_" + applicantLastName + "_" + date + ".pdf";

            FileStream _FileStream = null;
            try
            {
                _FileStream = new FileStream(newTempFilePath,
                            System.IO.FileMode.Create,
                            System.IO.FileAccess.Write);
                _FileStream.Write(pdfBytes, 0, pdfBytes.Length);
                long length = new System.IO.FileInfo(newTempFilePath).Length;
                Int32 filesize = 0;
                bool result = Int32.TryParse(length.ToString(), out filesize);
                try
                {
                    _FileStream.Close();
                }
                catch (Exception)
                {
                    base.LogError("Error while closing fileStream", new SystemException());
                }
                LogInfo("Saving the required documentation document");

                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    //Move file to other location
                    String pdfDocPathFileName = applicantFileLocation + destFileName;
                    File.Copy(newTempFilePath, pdfDocPathFileName);
                    newFinalFilePath = pdfDocPathFileName;

                }
                else
                {
                    applicantFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"];
                    if (!applicantFileLocation.EndsWith("//"))
                    {
                        applicantFileLocation += "//";
                    }
                    //AWS code to save document to S3 location
                    AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                    String destFolder = applicantFileLocation + "Tenant(" + TenantID.ToString() + @")/";
                    String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, destFileName, destFolder);
                    newFinalFilePath = returnFilePath; //Path.Combine(destFolder, destFileName);
                }

                if (!String.IsNullOrEmpty(newTempFilePath))
                    File.Delete(newTempFilePath);

                String documentTypeCode = INTSOF.Utils.DocumentType.ADDITIONAL_DOCUMENTS.GetStringValue();
                Boolean isSearchableOnly = false;
                Boolean isSetDataEntryDocStatusToCompleted = false;
                if (!sysDocument.IsOperational.IsNullOrEmpty() && sysDocument.IsOperational.Value)
                {
                    isSearchableOnly = true;
                    isSetDataEntryDocStatusToCompleted = true;
                }
                //if (_applicantOrderCart.IsNotNull() && !_applicantOrderCart.IsCompliancePackageSelected && !IsSubscriptionExist)
                //if (!IsSubscriptionExist)
                //{
                //    isSetDataEntryDocStatusToCompleted = true;
                //}

                ApplicantDocument applicantDocument = _presenter.SaveAditionalDocument(TenantID, newFinalFilePath, filename, filesize, documentTypeCode, CurrentLoggedInUserID,
                                                                                       OrgUsrID, isSetDataEntryDocStatusToCompleted, isSearchableOnly);

                if (applicantDocument.IsNotNull())
                {
                    LogInfo(String.Format("Saved the required documentation document {0} for applicant{1}", applicantDocument.ApplicantDocumentID, applicantDocument.OrganizationUserID));
                    SysDocumentFieldMappingContract savedDocInfo = new SysDocumentFieldMappingContract();
                    savedDocInfo.ID = applicantDocument.ApplicantDocumentID;
                    savedDocInfo.DocumentPath = applicantDocument.DocumentPath;
                    savedDocInfo.FieldName = applicantDocument.FileName;
                    savedDocInfo.SystemDocOldID = sysDocument.SystemDocumentID;
                    savedDocInfo.SendToStudent = sysDocument.SendToStudent;

                    //UAT-3745
                    if (!lstServiceDocBkgSvcMapping.IsNullOrEmpty())
                    {
                        savedDocInfo.SystemDocBkgSvcMapping = new SystemDocBkgSvcMapping();
                        savedDocInfo.SystemDocBkgSvcMapping = lstServiceDocBkgSvcMapping.Where(con => con.SystemDocumentID == sysDocument.SystemDocumentID).FirstOrDefault();
                        if (!savedDocInfo.SystemDocBkgSvcMapping.IsNullOrEmpty())
                        {
                            Int16 bkgSvcRecordTypeId = Presenter.GetlkpRecordTypeIdByCode(TenantID, RecordType.Background_Service.GetStringValue());
                            savedDocInfo.SystemDocBkgSvcMapping.ApplicantDocumentID = applicantDocument.ApplicantDocumentID;
                            savedDocInfo.SystemDocBkgSvcMapping.RecordTypeID = bkgSvcRecordTypeId;
                        }
                    }

                    //End UAT-3745

                    if (AdditionalDocumentList.IsNullOrEmpty())
                        AdditionalDocumentList = new List<SysDocumentFieldMappingContract>();
                    AdditionalDocumentList.Add(savedDocInfo);
                  
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                throw ex;
            }
            finally
            {
                try { _FileStream.Close(); }
                catch (Exception)
                {
                    base.LogError("Error while closing fileStream", new SystemException());
                }
            }
        }

        private String GetFileName(String fileNameWithExt)
        {
            String[] splitFileName = fileNameWithExt.Split('.');
            String tempFileName = String.Join(".", splitFileName.Take(splitFileName.Length - 1));
            tempFileName = tempFileName.Replace(@"\", @"-");
            return tempFileName;
        }
        #endregion
    }
}