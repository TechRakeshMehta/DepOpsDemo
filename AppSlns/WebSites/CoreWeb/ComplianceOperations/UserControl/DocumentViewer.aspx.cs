using Business.RepoManagers;
using Business.ReportExecutionService;
using CoreWeb.IntsofExceptionModel.Interface;
using CoreWeb.Shell;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Configuration;
using RadPdf.Data.Document;
using RadPdf.Data.Document.Objects.FormFields;
using System.Linq;
using iTextSharp.text.pdf;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class DocumentViewer : System.Web.UI.Page, IDocumentViewerView
    {
        private DocumentViewerPresenter _presenter = new DocumentViewerPresenter();
        private ISysXExceptionService _exceptionService = SysXWebSiteUtils.ExceptionService;
        // private ISysXExceptionService _exceptionService = SysXWebSiteUtils.ExceptionService;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Boolean aWSUseS3 = false;
                    if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                    {
                        aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                    }
                    Presenter.OnViewInitialized();
                    #region UAT-1544
                    GetDataFromArguments();
                    #endregion
                    if (Request.QueryString["documentId"] != null && !Request.QueryString["documentId"].Trim().Equals(""))
                        ApplicantDocumentId = Convert.ToInt32(Request.QueryString["documentId"]);
                    if (Request.QueryString["ClientSysDocID"] != null && !Request.QueryString["ClientSysDocID"].Trim().Equals(""))
                        ClientSysDocId = Convert.ToInt32(Request.QueryString["ClientSysDocID"]);
                    if (Request.QueryString["IsApplicantDocument"] != null && !Request.QueryString["IsApplicantDocument"].Trim().Equals(""))
                        IsApplicantDocument = Convert.ToBoolean(Request.QueryString["IsApplicantDocument"]);
                    if (Request.QueryString["tenantId"] != null && !Request.QueryString["tenantId"].Trim().Equals(""))
                        TenantId = Convert.ToInt32(Request.QueryString["tenantId"]);
                    if (Request.QueryString["UserId"] != null)
                        loggedinUserID = Convert.ToInt32(Request.QueryString["UserId"]);
                    if (Request.QueryString["DocumentType"] != null)
                        DocumentType = Convert.ToString(Request.QueryString["DocumentType"]);
                    if (Request.QueryString["WebsiteId"] != null && !Request.QueryString["WebsiteId"].Trim().Equals(""))
                        WebsiteId = Convert.ToInt32(Request.QueryString["WebsiteId"]);
                    if (Request.QueryString["UnifiedDocument"] != null)
                        IsUnifiedDocument = Convert.ToBoolean(Request.QueryString["UnifiedDocument"]);
                    if (Request.QueryString["systemDocumentId"] != null && !Request.QueryString["systemDocumentId"].Trim().Equals(""))
                        SystemDocumentID = Convert.ToInt32(Request.QueryString["systemDocumentId"]);
                    if (Request.QueryString["DNR"] != null && !Request.QueryString["DNR"].Trim().Equals(""))
                        DNR = Convert.ToBoolean(Request.QueryString["DNR"]);
                    //if (Request.QueryString["IsDisclosureDocEdit"] != null && !Request.QueryString["IsDisclosureDocEdit"].Trim().Equals(""))
                    //    IsDisclosureDocEdit = Convert.ToString(Request.QueryString["IsDisclosureDocEdit"]);
                    if (Request.QueryString["FileName"] != null && !Request.QueryString["FileName"].Trim().Equals(""))
                        FileName = Convert.ToString(Request.QueryString["FileName"]);
                    if (Request.QueryString["DocumentPath"] != null && !Request.QueryString["DocumentPath"].Trim().Equals(""))
                        FilePath = Convert.ToString(Request.QueryString["DocumentPath"]);
                    if (Request.QueryString["OrderID"] != null && !Request.QueryString["OrderID"].Trim().Equals(""))
                        OrderID = Convert.ToInt32((Request.QueryString["OrderID"]));
                    if (Request.QueryString["ServiceGroupID"] != null && !Request.QueryString["ServiceGroupID"].Trim().Equals(""))
                        ServiceGroupID = Convert.ToInt32((Request.QueryString["ServiceGroupID"]));
                    if (Request.QueryString["ReportType"] != null && !Request.QueryString["ReportType"].Trim().Equals(""))
                        ReportType = Convert.ToString(Request.QueryString["ReportType"]);
                    if (Request.QueryString["ReportName"] != null && !Request.QueryString["ReportName"].Trim().Equals(""))
                        ReportName = Convert.ToString(Request.QueryString["ReportName"]);
                    if (Request.QueryString["ApplicantID"] != null && !Request.QueryString["ApplicantID"].Trim().Equals(""))
                        ApplicantID = Convert.ToInt32(Request.QueryString["ApplicantID"]);
                    if (Request.QueryString["FromDate"] != null && !Request.QueryString["FromDate"].Trim().Equals(""))
                        FromDate = Convert.ToString(Request.QueryString["FromDate"]);
                    if (Request.QueryString["ToDate"] != null && !Request.QueryString["ToDate"].Trim().Equals(""))
                        ToDate = Convert.ToString(Request.QueryString["ToDate"]);
                    if (Request.QueryString["IsFromReport"] != null && !Request.QueryString["IsFromReport"].Trim().Equals(""))
                        IsFromReport = Convert.ToBoolean(Request.QueryString["IsFromReport"]);
                    if (Request.QueryString["UserIdForReport"] != null && !Request.QueryString["UserIdForReport"].Trim().Equals(""))
                        UserIdForReport = Convert.ToString(Request.QueryString["UserIdForReport"]);
                    if (Request.QueryString["Institute"] != null && !Request.QueryString["Institute"].Trim().Equals(""))
                        Institute = Convert.ToString(Request.QueryString["Institute"]);
                    if (Request.QueryString["Hierarchy"] != null && !Request.QueryString["Hierarchy"].Trim().Equals(""))
                        Hierarchy = Convert.ToString(Request.QueryString["Hierarchy"]);
                    if (Request.QueryString["HierarchyNodeID"] != null && !Request.QueryString["HierarchyNodeID"].Trim().Equals(""))
                        HierarchyNodeID = Convert.ToInt32(Request.QueryString["HierarchyNodeID"]);
                    if (Request.QueryString["IsReportSentToStudent"] != null && !Request.QueryString["IsReportSentToStudent"].Trim().Equals(""))
                        IsReportSentToStudent = Convert.ToBoolean(Request.QueryString["IsReportSentToStudent"]);
                    if (Request.QueryString["OrganizationUserID"] != null && !Request.QueryString["OrganizationUserID"].Trim().Equals(""))
                        OrganizationUserID = Convert.ToInt32(Request.QueryString["OrganizationUserID"]);
                    if (Request.QueryString["Psid"] != null && !Request.QueryString["Psid"].Trim().Equals(""))
                        PkgSubscriptionIds = Convert.ToInt32(Request.QueryString["Psid"]);
                    if (Request.QueryString["ShrdCatIds"] != null && !Request.QueryString["ShrdCatIds"].Trim().Equals(""))
                        ShrdCategoryIds = Convert.ToString(Request.QueryString["ShrdCatIds"]);
                    if (Request.QueryString["SnpShtId"] != null && !Request.QueryString["SnpShtId"].Trim().Equals(""))
                        SnpShtId = Convert.ToInt32(Request.QueryString["SnpShtId"]);
                    if (Request.QueryString["InvitationId"] != null && !Request.QueryString["InvitationId"].Trim().Equals(""))
                        InvitationId = Convert.ToInt32(Request.QueryString["InvitationId"]);
                    if (Request.QueryString["UserAttestationDocumentID"] != null && !Request.QueryString["UserAttestationDocumentID"].Trim().Equals(""))
                        UserAttestationDocumentID = Convert.ToInt32(Request.QueryString["UserAttestationDocumentID"]);
                    if (Request.QueryString["UAFDocumentStage"] != null && !Request.QueryString["UAFDocumentStage"].Trim().Equals(""))
                        UAFDocumentStage = Convert.ToString(Request.QueryString["UAFDocumentStage"]);
                    if (Request.QueryString["InvitationDocumentID"] != null && !Request.QueryString["InvitationDocumentID"].Trim().Equals(""))
                        InvitationDocumentID = Convert.ToInt32(Request.QueryString["InvitationDocumentID"]);
                    if (Request.QueryString["ProfileSharingInvitationID"] != null && !Request.QueryString["ProfileSharingInvitationID"].Trim().Equals(""))
                        ProfileSharingInvitationID = Convert.ToInt32(Request.QueryString["ProfileSharingInvitationID"]);
                    if (Request.QueryString["AttestationTypeCode"] != null && !Request.QueryString["AttestationTypeCode"].Trim().Equals(""))
                        AttestationTypeCode = Convert.ToString(Request.QueryString["AttestationTypeCode"]);
                    if (Request.QueryString["ContractIDs"] != null && !Request.QueryString["ContractIDs"].Trim().Equals(""))
                        ContractIDs = Convert.ToString(Request.QueryString["ContractIDs"]);
                    //UAT-2706
                    if (Request.QueryString["IsAgencyUserDocumentView"] != null && !Request.QueryString["IsAgencyUserDocumentView"].Trim().Equals(""))
                        IsAgencyUserDocumentView = Convert.ToBoolean(Request.QueryString["IsAgencyUserDocumentView"]);
                    //UAT-2774
                    if (Request.QueryString["ProfileSharingInvitationGroupId"] != null && !Request.QueryString["ProfileSharingInvitationGroupId"].Trim().IsNullOrEmpty())
                        ProfileSharingInvitationGroupId = Convert.ToInt32(Request.QueryString["ProfileSharingInvitationGroupId"]);
                    //UAT-3675
                    if (Request.QueryString["LocationImageFilePath"] != null && !Request.QueryString["LocationImageFilePath"].Trim().IsNullOrEmpty())
                        LocationImageFilePath = Convert.ToString(Request.QueryString["LocationImageFilePath"]);
                    if (Request.QueryString["LocationImageFileName"] != null && !Request.QueryString["LocationImageFileName"].Trim().IsNullOrEmpty())
                        LocationImageFileName = Convert.ToString(Request.QueryString["LocationImageFileName"]);
                    //Ticket Center Implementation UAT-3907
                    if (Request.QueryString["TicketDocumentID"] != null)
                        TicketDocumentID = Convert.ToInt32(Request.QueryString["TicketDocumentID"]);
                    #region UAT - 3866
                    if (Request.QueryString["BkgPkgSvcGrpID"] != null && !Request.QueryString["BkgPkgSvcGrpID"].Trim().Equals(""))
                        BkgPkgServiceGroupId = Convert.ToInt32((Request.QueryString["BkgPkgSvcGrpID"]));
                    #endregion

                    if (Request.QueryString["FingerPrintImageFilePath"] != null && !Request.QueryString["FingerPrintImageFilePath"].Trim().IsNullOrEmpty())
                        FingerPrintImageFilePath = Convert.ToString(Request.QueryString["FingerPrintImageFilePath"]);
                    if (Request.QueryString["FingerPrintImageFileName"] != null && !Request.QueryString["FingerPrintImageFileName"].Trim().IsNullOrEmpty())
                        FingerPrintImageFileName = Convert.ToString(Request.QueryString["FingerPrintImageFileName"]);

                    if (!ContractIDs.IsNullOrEmpty())
                    {
                        GetContractExportReport();
                    }

                    if (Request.QueryString["ReportType"] != null && Request.QueryString["ReportType"].Trim().Equals("ConsolidatedPassportReport"))
                    {
                        GetConsolidatedPassportReport();
                    }
                    if (Request.QueryString["ReportType"] != null && Request.QueryString["ReportType"].Trim().Equals("InstitutionConfigurationExport"))
                    {
                        GetInstitutionConfigurationExportReport();
                    }

                    if (DocumentType != null && DocumentType.ToLower() == "LoginImage".ToLower())
                    {
                        GetLoginImage();
                    }
                    else if (DocumentType != null && DocumentType.ToLower() == "RightLogoImage".ToLower())
                    {
                        GetRightLogoImage();
                    }
                    else if (DocumentType != null && DocumentType.ToLower() == "RecieptDocument".ToLower())
                    {
                        GetRecieptDocumentDataForOrderID(OrderID, aWSUseS3);
                    }

                    else if (DNR)
                    {
                        GetDNRDocument(aWSUseS3);
                    }
                    else if (IsUnifiedDocument)
                    {
                        GetUnifiedDocumentData(aWSUseS3);
                    }
                    else if (DocumentType != null && DocumentType.ToLower() == "DisclosureDocumentForTracking".ToLower())
                    {
                        GetDisclosureDocumentData(aWSUseS3);
                    }
                    else if (DocumentType != null && DocumentType.ToLower() == "ESignedDisclosureDocument".ToLower())
                    {
                        String disclosureDocumentTypeId = INTSOF.Utils.DocumentType.DisclosureDocument.GetStringValue();
                        GetESignedDocumentData(disclosureDocumentTypeId, aWSUseS3);
                    }
                    else if (DocumentType != null && DocumentType.ToLower() == "ESignedDisclaimerDocument".ToLower())
                    {
                        String disclaimerdocumentTypeId = INTSOF.Utils.DocumentType.DisclaimerDocument.GetStringValue();
                        GetESignedDocumentData(disclaimerdocumentTypeId, aWSUseS3);
                    }
                    else if (DocumentType != null && DocumentType.ToLower() == "DisclaimerDocument".ToLower())
                    {
                        try
                        {
                            String disclaimerDocumentpdfPath = string.Empty;                            
                            if (Presenter.IsOverrideDisclaimerDocument()) //UAT-4592 
                                GetDisclaimerDocumentData(aWSUseS3);
                            else
                                disclaimerDocumentpdfPath = Server.MapPath("~/App_Data/DisclaimerDocument.pdf");

                            if (!String.IsNullOrEmpty(disclaimerDocumentpdfPath))
                            {
                                String disclaimerDocumentFileName = Path.GetFileName(disclaimerDocumentpdfPath);
                                Initialize(disclaimerDocumentpdfPath, disclaimerDocumentFileName);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (DocumentType != null && DocumentType.ToLower() == "ClientSystemDocument".ToLower())
                    {
                        if (IsAgencyUserDocumentView)
                        {
                            GetDocumentPathForAgencyUser(aWSUseS3);
                        }
                        else if (FileName.IsNullOrEmpty())
                        {
                            GetClientApplicantDataByID(aWSUseS3);
                        }
                        else
                        {
                            GetApplicantDocDataByFileName(aWSUseS3);
                        }
                    }

                    else if (DocumentType != null && DocumentType.ToLower() == "FailedUnifiedDocument".ToLower())
                    {
                        GetFailedUnifiedDocumentData(aWSUseS3);
                    }
                    else if (DocumentType != null && DocumentType.ToLower() == "ServiceFormDocument".ToLower())
                    {
                        GetServiceFormDocumentData();
                    }
                    else if (DocumentType != null && (DocumentType.ToLower() == "DisclosureReleaseDocument".ToLower() || DocumentType.ToLower() == "AdditionalDocument".ToLower()))
                    {
                        GetDisclosureReleaseDocAndAdditionalDoc();
                    }
                    else if (DocumentType != null && DocumentType.ToLower() == "ReportDocument".ToLower())
                    {
                        GetReportDocumentData();
                    }
                    else if (IsFromReport)
                    {
                        //GetReportDocumentData();
                        GetBkgReportData();
                    }
                    else if (DocumentType != null && DocumentType.ToLower() == "EDS_AuthorizationForm".ToLower())
                    {
                        GetEdsDocument(aWSUseS3);
                    }
                    else if (DocumentType != null && DocumentType.ToLower() == AppConsts.VIEW_ATTESTATION.ToLower())
                    {
                        GetInvitationDocument(aWSUseS3);
                    }
                    else if (DocumentType != null && DocumentType.ToLower() == DislkpDocumentType.EMPLOYMENT_DISCLOSURE_FORM.GetStringValue().ToLower())
                    {
                        GetEmploymentDisclosureDocument(aWSUseS3);
                    }
                    else if (UserAttestationDocumentID > AppConsts.NONE)
                    {
                        GetPartiallyFilledUserAttestationDocument(aWSUseS3);
                    }
                    else if (DocumentType != null && DocumentType.ToLower() == AppConsts.VIEW_ATTESTATION_ADMIN.ToLower())
                    {
                        GetInvitationDocumentAdmin(aWSUseS3);
                    }
                    else if (DocumentType != null && DocumentType.ToLower() == AppConsts.APPLICANT_PRINT_DOCUMENT_TYPE.ToLower())
                    {
                        LoadApplicantPrintDocument();
                    }
                    else if (DocumentType != null && DocumentType.ToLower() == AppConsts.SHARED_USER_INVITATION_DOCUMENT.ToLower()) //UAT-2774
                    {
                        LoadSharedUserInvitationDocument(aWSUseS3);
                    }
                    //UAT-3321
                    else if (DocumentType != null && DocumentType.ToLower() == AppConsts.USER_GUIDE_FOR_AGENCY_USER.ToLower())
                    {
                        if (aWSUseS3 == false)
                        {
                            Initialize(FilePath, FileName);
                        }
                        else
                        {
                            InitializeS3Documents(FilePath, FileName);
                        }
                    }

                    //UAT-3675
                    else if (DocumentType != null && DocumentType.ToLower() == AppConsts.LOCATION_IMAGE_FILE.ToLower())
                    {
                        if (aWSUseS3 == false)
                        {
                            Initialize(LocationImageFilePath, LocationImageFileName);
                        }
                        else
                        {
                            InitializeS3Documents(LocationImageFilePath, LocationImageFileName);
                        }
                    }
                    //Imeplementation Related to Ticket Center
                    else if (DocumentType != null && DocumentType.ToLower() == AppConsts.DOCUMENT_TYPE_TICKET_CENTER.ToLower())
                    {
                        GetTicketDocument(aWSUseS3);
                    }
                    else if (DocumentType != null && DocumentType.ToLower() == AppConsts.ApplicantFingerPrintFile_IMAGE.ToLower())
                    {
                        if (aWSUseS3 == false)
                        {
                            Initialize(FingerPrintImageFilePath, FingerPrintImageFileName);
                        }
                        else
                        {
                            InitializeS3Documents(FingerPrintImageFilePath, FingerPrintImageFileName);
                        }
                    }
                    else
                    {
                        GetDocumentPath(aWSUseS3);
                    }
                }
                Presenter.OnViewLoaded();
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
            catch (Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }

        private void GetInvitationDocumentAdmin(bool aWSUseS3)
        {
            var invitationDocument = Presenter.GetInvitationDocumentByDocumentID();
            if (invitationDocument.IsNotNull())
            {
                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    Initialize(invitationDocument.IND_DocumentFilePath, "Attestation_Report");
                }
                else
                {
                    InitializeS3Documents(invitationDocument.IND_DocumentFilePath, "Attestation_Report");
                }
            }
        }


        //private void FillUserAttestationDocumentWithPrePopulatedData(Boolean isClient)
        //{
        //    Response.Clear();
        //    Response.Buffer = true;
        //    try
        //    {
        //        Response.BinaryWrite(Presenter.FillUserAttestationDocumentWithPrePopulatedData(isClient));
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError("", ex);
        //    }

        //    finally
        //    {
        //        Response.End();
        //    }
        //}


        private void GetRecieptDocumentDataForOrderID(int orderID, Boolean aWSUseS3)
        {
            Entity.ClientEntity.ApplicantDocument _recieptDoc = Presenter.GetRecieptDocumentDataForOrderID(orderID);
            //Initialize(FilePath, FileName);
            //Check whether use AWS S3, true if need to use
            if (aWSUseS3 == false)
            {
                Initialize(_recieptDoc.DocumentPath, _recieptDoc.FileName);
            }
            else
            {
                InitializeS3Documents(_recieptDoc.DocumentPath, _recieptDoc.FileName);
            }
        }


        #region UAT-1053
        /// <summary>
        /// Method to Get Bkg Report Data
        /// </summary>
        private void GetBkgReportData()
        {
            Response.Clear();
            Response.Buffer = true;
            try
            {
                ParameterValue[] parameters = new ParameterValue[2];

                if (ReportName.Equals(BackgroundReportType.ORDER_COMPLETION.GetStringValue()))
                {
                    if (BkgPkgServiceGroupId > 0)
                        parameters = new ParameterValue[4];
                    else
                        parameters = new ParameterValue[3];

                    parameters[0] = new ParameterValue();
                    parameters[0].Name = "OrderID";
                    parameters[0].Value = OrderID.ToString();//"5765";

                    parameters[1] = new ParameterValue();
                    parameters[1].Name = "TenantID";
                    parameters[1].Value = TenantId.ToString();//"4";

                    if (ServiceGroupID > 0)
                    {
                        parameters[2] = new ParameterValue();
                        parameters[2].Name = "PackageGroupID";
                        parameters[2].Value = ServiceGroupID.ToString();//"2"; 
                    }
                    if (BkgPkgServiceGroupId > 0)
                    {
                        parameters[3] = new ParameterValue();
                        parameters[3].Name = "BkgPkgSvcGrpID";
                        parameters[3].Value = BkgPkgServiceGroupId.ToString();//"2"; 
                    }
                }
                else if (ReportName.Equals(BackgroundReportType.DAILY_FLAGGED_ORDER_COMPLETION_REPORT.GetStringValue()) || ReportName.Equals(BackgroundReportType.DAILY_SERVICE_GROUP_COMPLETION_REPORT.GetStringValue()))
                {
                    parameters = new ParameterValue[6];
                    parameters[0] = new ParameterValue();
                    parameters[0].Name = "TenantID";
                    parameters[0].Value = TenantId.ToString();

                    parameters[1] = new ParameterValue();
                    parameters[1].Name = "UserID";
                    parameters[1].Value = UserIdForReport;

                    parameters[2] = new ParameterValue();
                    parameters[2].Name = "Institute";
                    parameters[2].Value = Institute.ToString();

                    parameters[3] = new ParameterValue();
                    parameters[3].Name = "Hierarchy";
                    parameters[3].Value = Hierarchy;

                    parameters[4] = new ParameterValue();
                    parameters[4].Name = "FromDate";
                    parameters[4].Value = FromDate;

                    parameters[5] = new ParameterValue();
                    parameters[5].Name = "ToDate";
                    parameters[5].Value = null;
                }

                byte[] reportContent = ReportManager.GetReportByteArray(ReportName, parameters);
                Response.ContentType = GetContentType(".pdf");
                Response.AddHeader("Content-Disposition", "inline;");
                if (reportContent.IsNotNull())
                {
                    Response.BinaryWrite(reportContent);
                }
            }
            catch (Exception ex)
            {
                LogError("", ex);
            }

            finally
            {
                Response.End();
            }

            //    ParameterValue[] parameters;
            //    if (ServiceGroupID > 0)
            //    {
            //        parameters = new ParameterValue[3];
            //        parameters[2] = new ParameterValue();
            //        parameters[2].Name = "PackageGroupID";
            //        parameters[2].Value = ServiceGroupID.ToString();//"2";
            //    }
            //    else
            //    {
            //        parameters = new ParameterValue[2];
            //    }
            //    parameters[0] = new ParameterValue();
            //    parameters[0].Name = "OrderID";
            //    parameters[0].Value = OrderID.ToString();//"5765";
            //    parameters[1] = new ParameterValue();
            //    parameters[1].Name = "TenantID";
            //    parameters[1].Value = TenantId.ToString();//"4";

        }
        #endregion

        #region UAT-1178
        private void GetPartiallyFilledUserAttestationDocument(Boolean aWSUseS3)
        {
            var userAttestationDocument = Presenter.GetPartiallyFilledUserAttestationDocument();
            if (userAttestationDocument.IsNotNull())
            {

                if (aWSUseS3 == false)
                {
                    Initialize(userAttestationDocument.UAD_DocumentPath, userAttestationDocument.UAD_FileName);
                }
                else
                {
                    if (UAFDocumentStage == AppConsts.UAF_PARTIALLY_FILLED_MODE || UAFDocumentStage == AppConsts.UAF_PREVIEW_MODE)
                    {
                        Initialize(userAttestationDocument.UAD_DocumentPath, userAttestationDocument.UAD_FileName);
                    }
                    else
                    {
                        InitializeS3Documents(userAttestationDocument.UAD_DocumentPath, userAttestationDocument.UAD_FileName);
                    }
                }
            }
        }
        #endregion

        public DocumentViewerPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public Int32 ApplicantDocumentId
        {
            get
            {
                if (ViewState["ApplicantDocumentId"] != null)
                    return Convert.ToInt32(ViewState["ApplicantDocumentId"]);
                return 0;
            }
            set
            {
                ViewState["ApplicantDocumentId"] = value;
            }
        }

        //Ticket Cernter
        public Int32 TicketDocumentID
        {
            get
            {
                if (ViewState["TicketDocumentID"] != null)
                    return Convert.ToInt32(ViewState["TicketDocumentID"]);
                return 0;
            }
            set
            {
                ViewState["TicketDocumentID"] = value;
            }
        }


        public Int32 ClientSysDocId
        {
            get
            {
                if (ViewState["ClientSysDocId"] != null)
                    return Convert.ToInt32(ViewState["ClientSysDocId"]);
                return 0;
            }
            set
            {
                ViewState["ClientSysDocId"] = value;
            }
        }

        public Boolean IsApplicantDocument
        {
            get
            {
                if (ViewState["IsApplicantDocument"] != null)
                    return Convert.ToBoolean(ViewState["IsApplicantDocument"]);
                return false;
            }
            set
            {
                ViewState["IsApplicantDocument"] = value;
            }
        }
        public Int32 TenantId
        {
            get
            {
                if (ViewState["TenantId"] != null)
                    return Convert.ToInt32(ViewState["TenantId"]);
                return 0;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        public Boolean DNR
        {
            get;
            set;
        }

        public Int32 loggedinUserID
        {
            get
            {
                //return SysXWebSiteUtils.SessionService.OrganizationUserId;
                if (ViewState["UserId"] != null)
                    return Convert.ToInt32(ViewState["UserId"]);
                return 0;
            }
            set
            {
                ViewState["UserId"] = value;
            }

        }

        public List<String> tmpDocPaths
        {
            get
            {
                //return SysXWebSiteUtils.SessionService.OrganizationUserId;
                if (Session["tmpDocPaths"] != null)
                    return Session["tmpDocPaths"] as List<String>;
                return new List<String>();
            }
            set
            {
                (Session["tmpDocPaths"]) = value;
            }

        }

        public List<String> tmpDocPathsforSave
        {
            get
            {
                //return SysXWebSiteUtils.SessionService.OrganizationUserId;
                if (Session["tmpDocPathsforSave"] != null)
                    return Session["tmpDocPathsforSave"] as List<String>;
                return new List<String>();
            }
            set
            {
                (Session["tmpDocPathsforSave"]) = value;
            }

        }

        public String DocumentType
        {
            get
            {
                if (ViewState["DocumentType"] != null)
                    return (ViewState["DocumentType"].ToString());
                return null;
            }
            set
            {
                ViewState["DocumentType"] = value;
            }

        }

        public String newTempFilePathForRadPDFSave
        {
            get
            {
                if (Session["newTempFilePathForRadPDFSave"] != null)
                    return (Session["newTempFilePathForRadPDFSave"].ToString());
                return null;
            }
            set
            {
                Session["newTempFilePathForRadPDFSave"] = value;
            }

        }

        public String tmpFilePathforTrackingDoc
        {
            get
            {
                if (Session["tmpFilePathforTrackingDoc"] != null)
                    return (Session["tmpFilePathforTrackingDoc"].ToString());
                return null;
            }
            set
            {
                Session["tmpFilePathforTrackingDoc"] = value;
            }

        }

        public String FileName
        {
            get;
            set;
        }

        public String AWSDocumentPath
        {
            get;
            set;
        }
        //public String IsDisclosureDocEdit
        //{
        //    get
        //    {
        //        if (Session["IsDisclosureDocEdit"] != null)
        //            return (Session["IsDisclosureDocEdit"].ToString());
        //        return null;
        //    }
        //    set
        //    {
        //        Session["IsDisclosureDocEdit"] = value;
        //    }

        //}
        public String FilePath
        {
            get;
            set;
        }

        public Int32 WebsiteId
        {
            get
            {
                if (ViewState["WebsiteId"] != null)
                    return Convert.ToInt32(ViewState["WebsiteId"]);
                return 0;
            }
            set
            {
                ViewState["WebsiteId"] = value;
            }
        }

        public Int32 SystemDocumentID
        {
            get
            {
                if (ViewState["SystemDocumentID"] != null)
                    return Convert.ToInt32(ViewState["SystemDocumentID"]);
                return 0;
            }
            set
            {
                ViewState["SystemDocumentID"] = value;
            }
        }

        #region UAT-4592
        public Int32 DisclaimerDocumentSystemDocumentID
        {
            get
            {
                if (ViewState["DisclaimerDocumentSystemDocumentID"] != null)
                    return Convert.ToInt32(ViewState["DisclaimerDocumentSystemDocumentID"]);
                return 0;
            }
            set
            {
                ViewState["DisclaimerDocumentSystemDocumentID"] = value;
            }
        }
        #endregion

        public Boolean IsUnifiedDocument
        {
            get
            {
                if (ViewState["UnifiedDocument"] != null)
                    return Convert.ToBoolean(ViewState["UnifiedDocument"]);
                return false;
            }
            set { ViewState["UnifiedDocument"] = value; }
        }

        public String LoginImageUrl
        {
            get;

            set;


        }

        public String RightLogoImageUrl
        {
            get;
            set;
        }

        public Int32 OrderID
        {
            get
            {
                if (ViewState["OrderID"] != null)
                    return Convert.ToInt32(ViewState["OrderID"]);
                return 0;
            }
            set
            {
                ViewState["OrderID"] = value;
            }
        }

        public Int32 ServiceGroupID
        {
            get
            {
                if (ViewState["ServiceGroupID"] != null)
                    return Convert.ToInt32(ViewState["ServiceGroupID"]);
                return 0;
            }
            set
            {
                ViewState["ServiceGroupID"] = value;
            }
        }
        #region UAT- 3886
        public Int32 BkgPkgServiceGroupId
        {
            get
            {
                if (ViewState["BkgPkgServiceGroupId"] != null)
                    return Convert.ToInt32(ViewState["BkgPkgServiceGroupId"]);
                return 0;
            }
            set
            {
                ViewState["BkgPkgServiceGroupId"] = value;
            }
        }
        #endregion

        public String ReportType
        {
            get
            {
                if (ViewState["ReportType"] != null)
                    return (ViewState["ReportType"].ToString());
                return null;
            }
            set
            {
                ViewState["ReportType"] = value;
            }

        }

        #region UAT-1053-PROPERTIES
        public String ReportName
        {
            get
            {
                if (ViewState["ReportName"] != null)
                    return (ViewState["ReportName"].ToString());
                return null;
            }
            set
            {
                ViewState["ReportName"] = value;
            }

        }

        public Int32 ApplicantID
        {
            get
            {
                if (ViewState["ApplicantID"] != null)
                    return Convert.ToInt32(ViewState["ApplicantID"]);
                return 0;
            }
            set
            {
                ViewState["ApplicantID"] = value;
            }

        }

        public String FromDate
        {
            get
            {
                if (ViewState["FromDate"] != null)
                    return (ViewState["FromDate"].ToString());
                return null;
            }
            set
            {
                ViewState["FromDate"] = value;
            }

        }

        public String ToDate
        {
            get
            {
                if (ViewState["ToDate"] != null)
                    return (ViewState["ToDate"].ToString());
                return null;
            }
            set
            {
                ViewState["ToDate"] = value;
            }
        }

        public Boolean IsFromReport
        {
            get
            {
                if (ViewState["IsFromReport"] != null)
                    return Convert.ToBoolean(ViewState["IsFromReport"]);
                return false;
            }
            set
            {
                ViewState["IsFromReport"] = value;
            }
        }

        public String UserIdForReport
        {
            get
            {
                if (ViewState["UserIdForReport"] != null)
                    return (ViewState["UserIdForReport"].ToString());
                return null;
            }
            set
            {
                ViewState["UserIdForReport"] = value;
            }
        }
        public String Institute
        {
            get
            {
                if (ViewState["Institute"] != null)
                    return (ViewState["Institute"].ToString());
                return null;
            }
            set
            {
                ViewState["Institute"] = value;
            }
        }
        public String Hierarchy
        {
            get
            {
                if (ViewState["Hierarchy"] != null)
                    return (ViewState["Hierarchy"].ToString());
                return null;
            }
            set
            {
                ViewState["Hierarchy"] = value;
            }
        }

        public Int32 HierarchyNodeID
        {
            get
            {
                if (ViewState["HierarchyNodeID"] != null)
                    return Convert.ToInt32(ViewState["HierarchyNodeID"]);
                return 0;
            }
            set
            {
                ViewState["HierarchyNodeID"] = value;
            }
        }

        public Boolean IsReportSentToStudent
        {
            get
            {
                if (ViewState["IsReportSentToStudent"] != null)
                    return Convert.ToBoolean(ViewState["IsReportSentToStudent"]);
                return false;
            }
            set
            {
                ViewState["IsReportSentToStudent"] = value;
            }
        }

        public Int32 OrganizationUserID
        {
            get
            {
                if (ViewState["OrganizationUserID"] != null)
                    return Convert.ToInt32(ViewState["OrganizationUserID"]);
                return 0;
            }
            set
            {
                ViewState["OrganizationUserID"] = value;
            }
        }


        public Int32 PkgSubscriptionIds
        {
            get
            {
                if (ViewState["PkgSubscriptionIds"] != null)
                    return Convert.ToInt32(ViewState["PkgSubscriptionIds"]);
                return 0;
            }
            set
            {
                ViewState["PkgSubscriptionIds"] = value;
            }
        }

        public String ShrdCategoryIds
        {
            get
            {
                if (ViewState["ShrdCategoryIds"] != null)
                    return Convert.ToString(ViewState["ShrdCategoryIds"]);
                return String.Empty;
            }
            set
            {
                ViewState["ShrdCategoryIds"] = value;
            }
        }

        public Int32 SnpShtId
        {
            get
            {
                if (ViewState["SnpShtId"] != null)
                    return Convert.ToInt32(ViewState["SnpShtId"]);
                return 0;
            }
            set
            {
                ViewState["SnpShtId"] = value;
            }
        }

        #region Profile Sharing Attestation Report
        public Int32 InvitationId
        {
            get
            {
                if (ViewState["InvitationId"] != null)
                    return Convert.ToInt32(ViewState["InvitationId"]);
                return 0;
            }
            set
            {
                ViewState["InvitationId"] = value;
            }
        }
        #endregion

        public Int32 UserAttestationDocumentID
        {
            get;
            set;
        }

        public string DocumentPath
        {
            get
            {
                if (ViewState["DocumentPath"] != null)
                    return (ViewState["DocumentPath"].ToString());
                return null;
            }
            set
            {
                ViewState["DocumentPath"] = value;
            }
        }

        public Int32 InvitationDocumentID
        {
            get
            {
                if (ViewState["InvitationDocumentID"] != null)
                    return Convert.ToInt32(ViewState["InvitationDocumentID"]);
                return 0;
            }
            set
            {
                ViewState["InvitationDocumentID"] = value;
            }
        }

        public Int32 ProfileSharingInvitationID
        {
            get
            {
                if (ViewState["ProfileSharingInvitationID"] != null)
                    return Convert.ToInt32(ViewState["ProfileSharingInvitationID"]);
                return 0;
            }
            set
            {
                ViewState["ProfileSharingInvitationID"] = value;
            }
        }

        public String AttestationTypeCode
        {
            get
            {
                if (ViewState["AttestationTypeCode"] != null)
                    return Convert.ToString(ViewState["AttestationTypeCode"]);
                return String.Empty;
            }
            set
            {
                ViewState["AttestationTypeCode"] = value;
            }
        }

        public String ContractIDs
        {
            get
            {
                if (ViewState["ContractIDs"] != null)
                    return Convert.ToString(ViewState["ContractIDs"]);
                return String.Empty;
            }
            set
            {
                ViewState["ContractIDs"] = value;
            }
        }

        #endregion

        #region UAT-2706
        public Boolean IsAgencyUserDocumentView { get; set; }
        #endregion

        #region UAT-2774
        public Int32 ProfileSharingInvitationGroupId
        {
            get
            {
                if (ViewState["ProfileSharingInvitationGroupId"] != null)
                    return Convert.ToInt32(ViewState["ProfileSharingInvitationGroupId"]);
                return 0;
            }
            set
            {
                ViewState["ProfileSharingInvitationGroupId"] = value;
            }
        }
        #endregion

        /// <summary>
        /// returns the Current Logged In User Id.
        /// </summary>
        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        #region UAT-3675
        public String LocationImageFilePath
        {
            get
            {
                if (ViewState["LocationImageFilePath"] != null)
                    return Convert.ToString(ViewState["LocationImageFilePath"]);
                return null;
            }
            set
            {
                ViewState["LocationImageFilePath"] = value;
            }
        }
        public String LocationImageFileName
        {
            get
            {
                if (ViewState["LocationImageFileName"] != null)
                    return Convert.ToString(ViewState["LocationImageFileName"]);
                return null;
            }
            set
            {
                ViewState["LocationImageFileName"] = value;
            }
        }

        public String FingerPrintImageFilePath
        {
            get
            {
                if (ViewState["FingerPrintImageFilePath"] != null)
                    return Convert.ToString(ViewState["FingerPrintImageFilePath"]);
                return null;
            }
            set
            {
                ViewState["FingerPrintImageFilePath"] = value;
            }
        }
        public String FingerPrintImageFileName
        {
            get
            {
                if (ViewState["FingerPrintImageFileName"] != null)
                    return Convert.ToString(ViewState["FingerPrintImageFileName"]);
                return null;
            }
            set
            {
                ViewState["FingerPrintImageFileName"] = value;
            }
        }

        #endregion
        public void GetDocumentPath(Boolean aWSUseS3)
        {
            String documentPath = string.Empty;
            String fileName = string.Empty;
            //String documentType = string.Empty;
            if (DocumentType != null && DocumentType == "ProfilePicture" && loggedinUserID > 0)
            {
                Entity.OrganizationUser orgUser = Presenter.getOrganisationUser(loggedinUserID);
                if (orgUser != null)
                {
                    documentPath = orgUser.PhotoName;
                    fileName = orgUser.OriginalPhotoName;
                }
                else
                {
                    LogError("ProfilePicture could not be loaded from database for UserId: " + loggedinUserID, null);
                }
            }
            else if (DocumentType == DocumentViewerDocType.ROTATION_DOCUMENT_PDF.GetStringValue())
            {
                Entity.ClientEntity.ApplicantDocument document = Presenter.GetApplicantDocument();
                if (document != null)
                {
                    documentPath = document.PdfDocPath;
                    fileName = document.PdfFileName;
                }
                else
                {
                    LogError("Applicant document could not be loaded. Document Id: " + ApplicantDocumentId + " Tenant Id: " + TenantId, null);
                }
                if (String.IsNullOrEmpty(documentPath))
                {
                    LogError("Document Path could not be found for document Id: " + ApplicantDocumentId + " Tenant Id: " + TenantId, null);
                }
            }
            else
            {
                Entity.ClientEntity.ApplicantDocument document = Presenter.GetApplicantDocument();
                if (document != null)
                {
                    documentPath = document.DocumentPath;
                    fileName = document.FileName;
                }
                else
                {
                    LogError("Applicant document could not be loaded. Document Id: " + ApplicantDocumentId + " Tenant Id: " + TenantId, null);
                }
                if (String.IsNullOrEmpty(documentPath))
                {
                    LogError("Document Path could not be found for document Id: " + ApplicantDocumentId + " Tenant Id: " + TenantId, null);
                }
            }
            //Check whether use AWS S3, true if need to use
            if (aWSUseS3 == false)
            {
                Initialize(documentPath, fileName);
            }
            else
            {
                InitializeS3Documents(documentPath, fileName);
            }
        }

        #region UAT-2706

        public void GetDocumentPathForAgencyUser(Boolean aWSUseS3)
        {
            String documentPath = string.Empty;
            String fileName = string.Empty;

            Entity.SharedDataEntity.ClientSystemDocument documentDetails = Presenter.GetSharedClientSystemDocument();

            if (documentDetails != null)
            {
                documentPath = documentDetails.CSD_DocumentPath;
                fileName = documentDetails.CSD_FileName;
            }
            else
            {
                LogError("ClientSystemDocument could not be loaded. ClientSystemDocument Id: " + ClientSysDocId, null);
            }

            if (String.IsNullOrEmpty(documentPath))
            {
                LogError("Document Path could not be found for ClientSystemDocument Id: " + ClientSysDocId, null);
            }

            //Check whether use AWS S3, true if need to use
            if (aWSUseS3 == false)
            {
                Initialize(documentPath, fileName);
            }
            else
            {
                InitializeS3Documents(documentPath, fileName);
            }
        }

        #endregion

        /// <summary>
        /// To initialize or get Document
        /// </summary>
        /// <param name="documentPath"></param>
        /// <param name="fileName"></param>
        public void Initialize(String documentPath, String fileName)
        {
            Response.Clear();
            Response.Buffer = true;
            try
            {
                String sPath = documentPath;
                if (!File.Exists(sPath) && DocumentType.IsNotNull() && (DocumentType.ToLower() == "LoginImage".ToLower()
                    || DocumentType.ToLower() == "RightLogoImage".ToLower()))
                {
                    sPath = AppConsts.DEFAULT_LOGIN_IMAGE_PATH;
                    sPath = Server.MapPath(sPath);
                }
                String extension = Path.GetExtension(sPath);
                if (File.Exists(sPath))
                {
                    FileStream myFileStream = new FileStream(sPath, FileMode.Open);
                    long FileSize = myFileStream.Length;
                    byte[] Buffer = new byte[(int)FileSize];
                    myFileStream.Read(Buffer, 0, (int)FileSize);
                    myFileStream.Close();
                    myFileStream.Dispose();
                    Response.AddHeader("Content-Length", FileSize.ToString());
                    Response.ContentType = GetContentType(extension);

                    if (IsUnifiedDocument)
                    {
                        // add the Content-Type and Content-Disposition HTTP headers
                        Response.AddHeader("Content-Type", "application/pdf");
                        Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}; size={1}", fileName,
                                        Buffer.Length.ToString()));
                    }
                    else if (Response.ContentType.Equals("application/octet-stream"))
                        Response.AddHeader("Content-disposition", "attachment; filename=" + fileName.Replace(" ", "_"));
                    else if (DocumentType != null && DocumentType.ToLower() == "FailedUnifiedDocument".ToLower())
                    {
                        //Add Content-Disposition HTTP header
                        Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}; size={1}", fileName,
                                            Buffer.Length.ToString()));
                    }
                    else if (DocumentType != null && (DocumentType.ToLower() == AppConsts.VIEW_ATTESTATION.ToLower() || DocumentType.ToLower() == AppConsts.VIEW_ATTESTATION_ADMIN.ToLower()
                    || DocumentType.ToLower() == AppConsts.USER_GUIDE_FOR_AGENCY_USER.ToLower()))//UAT-3321
                    {
                        //Add Content-Disposition HTTP header
                        Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}; size={1}", fileName + extension,
                                            Buffer.Length.ToString()));
                    }
                    else
                        Response.AddHeader("Content-Disposition", "inline;");


                    // Send the data to the browser
                    Response.BinaryWrite(Buffer);
                }
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
            catch (Exception ex)
            {
                LogError("", ex);
            }

            finally
            {
                Response.End();
            }
        }

        public void InitializeRadEditor(String documentPath)
        {
            //Response.Clear();
            //Response.Buffer = true;
            try
            {
                byte[] pdfData = System.IO.File.ReadAllBytes(documentPath);
                this.PdfWebControl1.CreateDocument("document", pdfData, PdfDocumentSettings.EnableSubmit);
                //PdfDocumentEditor DocumentEditor1 =
                //this.PdfWebControl1.EditDocument();
                //this.DocumentPath = documentPath;
                //DocumentEditor1.Save();

            }
            //{
            //    String sPath = documentPath;
            //    if (!File.Exists(sPath) && DocumentType.IsNotNull() && DocumentType.ToLower() == "LoginImage".ToLower())
            //    {
            //        sPath = AppConsts.DEFAULT_LOGIN_IMAGE_PATH;
            //        sPath = Server.MapPath(sPath);
            //    }
            //    String extension = Path.GetExtension(sPath);
            //    if (File.Exists(sPath))
            //    {
            //        FileStream myFileStream = new FileStream(sPath, FileMode.Open);
            //        long FileSize = myFileStream.Length;
            //        byte[] Buffer = new byte[(int)FileSize];
            //        myFileStream.Read(Buffer, 0, (int)FileSize);
            //        myFileStream.Close();
            //        myFileStream.Dispose();
            //        Response.AddHeader("Content-Length", FileSize.ToString());
            //        Response.ContentType = GetContentType(extension);

            //        if (IsUnifiedDocument)
            //        {
            //            // add the Content-Type and Content-Disposition HTTP headers
            //            Response.AddHeader("Content-Type", "application/pdf");
            //            Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}; size={1}", fileName,
            //                            Buffer.Length.ToString()));
            //        }
            //        else if (Response.ContentType.Equals("application/octet-stream"))
            //            Response.AddHeader("Content-disposition", "attachment; filename=" + fileName.Replace(" ", "_"));
            //        else if (DocumentType != null && DocumentType.ToLower() == "FailedUnifiedDocument".ToLower())
            //        {
            //            //Add Content-Disposition HTTP header
            //            Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}; size={1}", fileName,
            //                                Buffer.Length.ToString()));
            //        }
            //        else if (DocumentType != null && (DocumentType.ToLower() == AppConsts.VIEW_ATTESTATION.ToLower() || DocumentType.ToLower() == AppConsts.VIEW_ATTESTATION_ADMIN.ToLower()
            //        || DocumentType.ToLower() == AppConsts.USER_GUIDE_FOR_AGENCY_USER.ToLower()))//UAT-3321
            //        {
            //            //Add Content-Disposition HTTP header
            //            Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}; size={1}", fileName + extension,
            //                                Buffer.Length.ToString()));
            //        }
            //        else
            //            Response.AddHeader("Content-Disposition", "inline;");


            //        // Send the data to the browser
            //        Response.BinaryWrite(Buffer);
            //    }
            //}
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
            catch (Exception ex)
            {
                LogError("", ex);
            }

            finally
            {
                //Response.End();
            }
        }

        /// <summary>
        /// To initialize or get Document from Amazon S3 server
        /// </summary>
        /// <param name="documentPath"></param>
        /// <param name="fileName"></param>
        public void InitializeS3Documents(String documentPath, String fileName)
        {
            Response.Clear();
            Response.Buffer = true;
            try
            {
                AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                byte[] documentContent = objAmazonS3Documents.RetrieveDocument(documentPath);
                ViewState["AWSDocumentPath"] = documentPath;
                //IsDisclosureDocEdit = "true";
                //UAT-3587
                if (Convert.ToString(Session["IsSignatureStored"]) != "true" && (DocumentType == "DisclosureDocumentForTracking" || DocumentType == "DisclosureDocument" || DocumentType == "DisclaimerDocument"))
                {
                    FileStream _FileStream = null;
                    Boolean aWSUseS3 = false;
                    if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                    {
                        aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                    }
                    String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
                    String applicantFileLocation = String.Empty;
                    String filename = String.Empty;

                    if (tempFilePath.IsNullOrEmpty())
                    {
                        // base.LogError("Please provide path for TemporaryFileLocation in config.", new SystemException());
                        throw new SystemException("Please provide path for TemporaryFileLocation in config.");
                    }
                    if (!tempFilePath.EndsWith(@"\"))
                    {
                        tempFilePath += @"\";
                    }
                    tempFilePath += "Tenant(" + TenantId.ToString() + @")\";

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
                        applicantFileLocation += "Tenant(" + TenantId.ToString() + @")\";

                        if (!Directory.Exists(applicantFileLocation))
                        {
                            Directory.CreateDirectory(applicantFileLocation);
                        }
                    }
                    if (TenantId > AppConsts.NONE)
                    {
                        String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                        String destTempFileName = "DC_" + TenantId.ToString() + "_" + CurrentLoggedInUserId + date + ".pdf";
                        newTempFilePathForRadPDFSave = Path.Combine(tempFilePath, destTempFileName);
                        //Session["newTempFilePathForRadPDFSave"] = newTempFilePathForRadPDFSave;                        
                        if (tmpDocPaths.IsNullOrEmpty())
                            tmpDocPaths = new List<String>();
                        if (DocumentType != "DisclosureDocumentForTracking")
                        {
                            tmpDocPaths.Add(newTempFilePathForRadPDFSave);
                        }
                        tmpDocPathsforSave = tmpDocPaths;
                        if (DocumentType == "DisclosureDocumentForTracking" || DocumentType == "DisclaimerDocument")
                        {
                            tmpFilePathforTrackingDoc = newTempFilePathForRadPDFSave;
                        }
                        //Session["tempDocPaths"] = tmpDocPaths;

                        #region Disable Signature
                        byte[] updatedDocument = null;
                        PdfReader reader = new PdfReader(documentContent);
                        MemoryStream ms = new MemoryStream();
                        PdfStamper stamper = new PdfStamper(reader, ms);
                        AcroFields af = stamper.AcroFields;

                        if (af.GetField("SignatureImage").IsNotNull())
                        {
                            af.SetFieldProperty("SignatureImage", "setfflags", PdfFormField.FF_READ_ONLY, null);
                        }
                        if (af.GetField("Signature").IsNotNull())
                        {
                            af.SetFieldProperty("Signature", "setfflags", PdfFormField.FF_READ_ONLY, null);
                        }
                        if (af.GetField("SignatureImage2").IsNotNull())
                        {
                            af.SetFieldProperty("SignatureImage2", "setfflags", PdfFormField.FF_READ_ONLY, null);
                        }
                        stamper.Close();
                        updatedDocument = ms.ToArray();
                        ms.Close();
                        reader.Close();
                        #endregion

                        _FileStream = new FileStream(newTempFilePathForRadPDFSave,
                                System.IO.FileMode.Create,
                                System.IO.FileAccess.Write);
                        _FileStream.Write(updatedDocument, 0, updatedDocument.Length);
                        _FileStream.Close();
                        byte[] pdfData = System.IO.File.ReadAllBytes(newTempFilePathForRadPDFSave);
                        this.PdfWebControl1.CreateDocument("document", pdfData); //Opens the file in Rad Editor

                        if (DocumentType == "DisclosureDocumentForTracking" || DocumentType == "DisclaimerDocument")
                        {
                            pdfData = System.IO.File.ReadAllBytes(tmpFilePathforTrackingDoc);
                            this.PdfWebControl1.CreateDocument("document", pdfData);
                        }
                    }
                    this.DocumentPath = newTempFilePathForRadPDFSave;
                }
                else
                {
                    if (!documentContent.IsNullOrEmpty())
                    {
                        String extension = Path.GetExtension(documentPath);
                        Response.ClearHeaders();
                        Response.Clear();
                        Response.AddHeader("Content-Length", documentContent.Length.ToString());
                        Response.ContentType = GetContentType(extension);

                        if (IsUnifiedDocument)
                        {
                            // add the Content-Type and Content-Disposition HTTP headers
                            Response.AddHeader("Content-Type", "application/pdf");
                            Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}; size={1}", fileName,
                                            documentContent.Length.ToString()));
                        }
                        else if (Response.ContentType.Equals("application/octet-stream"))
                            Response.AddHeader("Content-disposition", "attachment; filename=" + fileName.Replace(" ", "_"));
                        else if (DocumentType != null && DocumentType.ToLower() == "FailedUnifiedDocument".ToLower())
                        {
                            //Add Content-Disposition HTTP header
                            Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}; size={1}", fileName,
                                                documentContent.Length.ToString()));
                        }
                        else if (DocumentType != null && (DocumentType.ToLower() == AppConsts.VIEW_ATTESTATION.ToLower() || DocumentType.ToLower() == AppConsts.VIEW_ATTESTATION_ADMIN.ToLower()
                        || DocumentType.ToLower() == AppConsts.USER_GUIDE_FOR_AGENCY_USER.ToLower()))//UAT-3321
                        {
                            //Add Content-Disposition HTTP header
                            Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}; size={1}", fileName + extension,
                                                documentContent.Length.ToString()));
                        }
                        else
                            Response.AddHeader("Content-Disposition", "inline;");

                        // Send the data to the browser
                        Response.BinaryWrite(documentContent);
                        //IsDisclosureDocEdit = "true";                        
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("", ex);
            }
        }

        private String GetContentType(String fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".pdf":
                    return "application/pdf";
                case ".swf":
                    return "application/x-shockwave-flash";
                case ".gif":
                    return "image/gif";
                case ".jpeg":
                case ".jpg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".txt":
                    return "text/plain";
                case ".xls":
                case ".xlsx":
                case ".csv":
                    return "application/vnd.ms-excel";
                default:
                    return "application/octet-stream";
            }
        }

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	

        public void GetLoginImage()
        {
            //String documentPath;
            String fileName = String.Empty;
            Presenter.GetLoginImageUrl();
            if (String.IsNullOrEmpty(LoginImageUrl))
            {
                LogError("Login Image could not be loaded for WebsiteId: " + WebsiteId, null);
            }
            Initialize(LoginImageUrl, fileName);

        }

        public void GetRightLogoImage()
        {
            //String documentPath;
            String fileName = String.Empty;
            Presenter.GetRightLogoImageUrl();
            if (String.IsNullOrEmpty(RightLogoImageUrl))
            {
                LogError("Right logo Image could not be loaded for WebsiteId: " + WebsiteId, null);
            }
            Initialize(RightLogoImageUrl, fileName);

        }

        protected void LogError(String errorMessage, System.Exception ex)
        {
            _exceptionService.HandleError(errorMessage, ex);
        }

        private void GetUnifiedDocumentData(Boolean aWSUseS3)
        {
            var unifiedDocument = Presenter.GetUnifiedDocumentData();
            if (unifiedDocument.IsNotNull())
            {
                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    Initialize(unifiedDocument.UPD_PdfDocPath, unifiedDocument.UPD_PdfFileName);
                }
                else
                {
                    InitializeS3Documents(unifiedDocument.UPD_PdfDocPath, unifiedDocument.UPD_PdfFileName);
                }
            }
        }

        private void GetFailedUnifiedDocumentData(Boolean aWSUseS3)
        {
            Entity.ClientEntity.ApplicantDocument applicantDocument = Presenter.GetFailedUnifiedApplicantDocument();
            if (applicantDocument.IsNotNull())
            {
                if (applicantDocument.PdfDocPath.IsNullOrEmpty())
                {
                    //Check whether use AWS S3, true if need to use
                    if (aWSUseS3 == false)
                    {
                        Initialize(applicantDocument.DocumentPath, applicantDocument.FileName);
                    }
                    else
                    {
                        InitializeS3Documents(applicantDocument.DocumentPath, applicantDocument.FileName);
                    }
                }
                else
                {
                    if (aWSUseS3 == false)
                    {
                        Initialize(applicantDocument.PdfDocPath, applicantDocument.PdfFileName);
                    }
                    else
                    {
                        InitializeS3Documents(applicantDocument.PdfDocPath, applicantDocument.PdfFileName);
                    }
                }
            }
        }

        /// <summary>
        /// Method to get Disclosure Document.
        /// </summary>
        private void GetDisclosureDocumentData(Boolean aWSUseS3)
        {
            var disclosureDocument = Presenter.GetDisclosureDocument();
            this.DocumentPath = disclosureDocument.DocumentPath;
            if (disclosureDocument.IsNotNull())
            {
                //Check whether use AWS S3, true if need to use
                if (Convert.ToString(Session["IsSignatureStored"]) != "true")
                {
                    if (aWSUseS3 == false)
                    {
                        Initialize(disclosureDocument.DocumentPath, disclosureDocument.FileName);
                    }
                    else
                    {
                        InitializeS3Documents(disclosureDocument.DocumentPath, disclosureDocument.FileName);
                    }
                }
                else
                {
                    if (aWSUseS3 == false)
                    {
                        InitializeRadEditor(disclosureDocument.DocumentPath);
                    }
                    else
                    {
                        InitializeS3Documents(disclosureDocument.DocumentPath, disclosureDocument.FileName);
                    }
                }
            }
        }

        #region UAT-1176 - EMPLOYMENT DISCLOSURE
        /// <summary>
        /// Method to Get Employment Disclosure Document 
        /// </summary>
        /// <param name="aWSUseS3"></param>
        private void GetEmploymentDisclosureDocument(Boolean aWSUseS3)
        {
            var employmentDisclosureDocument = Presenter.GetEmploymentDisclosureDocument();
            if (employmentDisclosureDocument.IsNotNull())
            {
                Response.Clear();
                Response.Buffer = true;
                DocumentPath = employmentDisclosureDocument.DocumentPath;
                Response.AddHeader("Content-Type", "application/pdf");
                Response.AddHeader("Content-Disposition", "inline;");
                try
                {
                    byte[] buffer = Presenter.GetFilledEmployementDisclosureDocument();
                    // Send the data to the browser
                    Response.BinaryWrite(buffer);
                }
                catch (Exception ex)
                {
                    LogError("Some Error Occurred.", ex);
                }
                finally
                {
                    Response.End();
                }


                //OLD FUNCTIONALITY
                //Check whether use AWS S3, true if need to use
                //if (aWSUseS3 == false)
                //{
                //Initialize(employmentDisclosureDocument.DocumentPath, employmentDisclosureDocument.FileName);
                //}
                //else
                //{
                //    InitializeS3Documents(employmentDisclosureDocument.DocumentPath, employmentDisclosureDocument.FileName);
                //}
            }
        }
        #endregion

        #region UAT-1178 - User Attest DISCLOSURE
        ///// <summary>
        ///// Method to Get User Attestation Disclosure Document 
        ///// </summary>
        ///// <param name="aWSUseS3"></param>
        //private void GetUserAttestationDisclosureDocument(bool aWSUseS3, bool isClientAdmin)
        //{
        //    var userAttestDisclosureDocument = Presenter.GetUserAttestationDisclosureDocument(isClientAdmin);
        //    if (userAttestDisclosureDocument.IsNotNull())
        //    {
        //        //Check whether use AWS S3, true if need to use
        //        if (aWSUseS3 == false)
        //        {
        //            Initialize(userAttestDisclosureDocument.DocumentPath, userAttestDisclosureDocument.FileName);
        //        }
        //        else
        //        {
        //            InitializeS3Documents(userAttestDisclosureDocument.DocumentPath, userAttestDisclosureDocument.FileName);
        //        }
        //        //TO-DO Pre Poulate the User Attestation Fomr with First Name, Last Name and School Name
        //    }

        //}
        #endregion

        /// <summary>
        /// Method to get Disclosure Document.
        /// </summary>
        private void GetDNRDocument(Boolean aWSUseS3)
        {
            //Initialize(FilePath, FileName);
            //Check whether use AWS S3, true if need to use
            if (aWSUseS3 == false)
            {
                InitializeRadEditor(FilePath);
            }
            else
            {
                InitializeS3Documents(FilePath, FileName);
            }
        }

        private void GetESignedDocumentData(String documentTypeCode, Boolean aWSUseS3)
        {
            var disclaimereDocument = Presenter.GetESignedeDocument(documentTypeCode);
            if (disclaimereDocument.IsNotNull())
            {
                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    Initialize(disclaimereDocument.DocumentPath, disclaimereDocument.FileName);
                }
                else
                {
                    InitializeS3Documents(disclaimereDocument.DocumentPath, disclaimereDocument.FileName);
                }
            }
        }

        private void GetServiceFormDocumentData()
        {
            Entity.ClientEntity.SystemDocument serviceFormDocument = Presenter.GetServiceFormDocumentData();
            if (serviceFormDocument.IsNotNull())
            {
                Response.Clear();
                Response.Buffer = true;
                try
                {
                    String sPath = serviceFormDocument.DocumentPath;
                    String extension = Path.GetExtension(sPath);
                    //FileStream myFileStream = new FileStream(sPath, FileMode.Open);
                    //long FileSize = myFileStream.Length;
                    //byte[] Buffer = new byte[(int)FileSize];
                    byte[] Buffer = CommonFileManager.RetrieveDocument(sPath, DocumentType);
                    //myFileStream.Close();
                    //myFileStream.Dispose();
                    //Response.AddHeader("Content-Length", FileSize.ToString());
                    Response.ContentType = GetContentType(extension);
                    Response.AddHeader("Content-Disposition", "inline;");
                    if (Buffer.IsNotNull())
                    {
                        Response.BinaryWrite(Buffer);
                    }
                }
                catch (Exception ex)
                {
                    LogError("", ex);
                }

                finally
                {
                    Response.End();
                }
            }
        }
        private void GetDisclosureReleaseDocAndAdditionalDoc()
        {
            Entity.ClientEntity.ApplicantDocument disclosureReleaseDoc = Presenter.GetApplicantDocument();
            if (disclosureReleaseDoc.IsNotNull())
            {
                Response.Clear();
                Response.Buffer = true;
                try
                {
                    String sPath = disclosureReleaseDoc.DocumentPath;
                    String extension = Path.GetExtension(sPath);
                    //FileStream myFileStream = new FileStream(sPath, FileMode.Open);
                    //long FileSize = myFileStream.Length;
                    //byte[] Buffer = new byte[(int)FileSize];
                    byte[] Buffer = CommonFileManager.RetrieveDocument(sPath, FileType.ApplicantFileLocation.GetStringValue());
                    //myFileStream.Close();
                    //myFileStream.Dispose();
                    //Response.AddHeader("Content-Length", FileSize.ToString());
                    Response.ContentType = GetContentType(extension);
                    Response.AddHeader("Content-Disposition", "inline;");
                    if (Buffer.IsNotNull())
                    {
                        Response.BinaryWrite(Buffer);
                    }
                }
                catch (Exception ex)
                {
                    LogError("", ex);
                }

                finally
                {
                    Response.End();
                }
            }
        }


        private void GetConsolidatedPassportReport()
        {
            Response.Clear();
            Response.Buffer = true;
            try
            {
                ParameterValue[] parameter;
                String reportName = ReportType;
                parameter = new ParameterValue[3];

                parameter[0] = new ParameterValue();
                parameter[0].Name = "Institute";
                parameter[0].Value = TenantId.ToString();
                parameter[1] = new ParameterValue();
                parameter[1].Name = "InvitationID";
                parameter[1].Value = InvitationId.ToString();

                parameter[2] = new ParameterValue();
                parameter[2].Name = "UserID";
                parameter[2].Value = loggedinUserID.ToString();

                byte[] reportContent = ReportManager.GetReportByteArrayFormat(reportName, parameter);
                Response.ContentType = GetContentType(".xlsx");
                Response.AddHeader("Content-Disposition", "attachment;filename=ConsolidatedPassportReport.xlsx");

                if (reportContent.IsNotNull())
                {
                    Response.BinaryWrite(reportContent);
                }
            }
            catch (Exception ex)
            {
                LogError("", ex);
            }

            finally
            {
                Response.End();
            }
        }

        private void GetContractExportReport()
        {
            Response.Clear();
            Response.Buffer = true;
            try
            {
                Boolean isSiteDetailsVisible = true;
                Boolean isSiteOnlyReport = false;
                String reportName = "ContractExportReport";
                String downloadedDocName = "ContractAndSiteDetailsExport.xlsx";
                if (ReportType == ReportTypeEnum.CONTRACT_EXPORT_CONTRACT_ONLY.GetStringValue())
                {
                    downloadedDocName = "MasterContractDetailsExport.xlsx";
                    isSiteDetailsVisible = false;
                }
                else if (ReportType == ReportTypeEnum.CONTRACT_EXPORT_SITE_ONLY.GetStringValue())
                {
                    downloadedDocName = "SiteDetailsExport.xlsx";
                    isSiteOnlyReport = true;
                }
                ParameterValue[] parameter;
                parameter = new ParameterValue[4];

                parameter[0] = new ParameterValue();
                parameter[0].Name = "Institute";
                parameter[0].Value = TenantId.ToString();
                parameter[1] = new ParameterValue();
                parameter[1].Name = "ContractIDs";
                parameter[1].Value = ContractIDs;
                parameter[2] = new ParameterValue();
                parameter[2].Name = "IsSiteDetailsVisible";
                parameter[2].Value = isSiteDetailsVisible.ToString();
                parameter[3] = new ParameterValue();
                parameter[3].Name = "IsSiteOnlyReport";
                parameter[3].Value = isSiteOnlyReport.ToString();
                byte[] reportContent = ReportManager.GetReportByteArrayFormat(reportName, parameter);
                Response.ContentType = GetContentType(".xlsx");
                Response.AddHeader("Content-Disposition", "attachment;filename=" + downloadedDocName);

                if (reportContent.IsNotNull())
                {
                    Response.BinaryWrite(reportContent);
                }
            }
            catch (Exception ex)
            {
                LogError("", ex);
            }

            finally
            {
                Response.End();
            }
        }

        /// <summary>
        /// Method to Export InstitutionConfiguration details (UAT-1748)
        /// </summary>
        private void GetInstitutionConfigurationExportReport()
        {
            Response.Clear();
            Response.Buffer = true;
            try
            {
                //Boolean isSiteDetailsVisible = true;
                //Boolean isSiteOnlyReport = false;
                String reportName = "InstitutionConfigurationNodeDetailsExport";
                String downloadedDocName = "InstitutionConfigurationDetailsExport.xlsx";

                ParameterValue[] parameter;
                parameter = new ParameterValue[1];

                parameter[0] = new ParameterValue();
                parameter[0].Name = "Institute";
                parameter[0].Value = TenantId.ToString();

                byte[] reportContent = ReportManager.GetReportByteArrayFormat(reportName, parameter);
                Response.ContentType = GetContentType(".xlsx");
                Response.AddHeader("Content-Disposition", "attachment;filename=" + downloadedDocName);

                if (reportContent.IsNotNull())
                {
                    Response.BinaryWrite(reportContent);
                }
            }
            catch (Exception ex)
            {
                LogError("", ex);
            }

            finally
            {
                Response.End();
            }
        }
        private void GetReportDocumentData()
        {
            Response.Clear();
            Response.Buffer = true;
            try
            {
                ParameterValue[] parameters;
                String reportName = ReportType;
                if (ReportType == "Status Report" || ReportType == "Data Report")
                {
                    if (loggedinUserID > AppConsts.NONE)
                        parameters = new ParameterValue[5];

                    else
                        parameters = new ParameterValue[4];


                    parameters[0] = new ParameterValue();
                    parameters[0].Name = "TenantID";
                    parameters[0].Value = TenantId.ToString();
                    parameters[1] = new ParameterValue();
                    parameters[1].Name = "SchoolID";
                    parameters[1].Value = TenantId.ToString();
                    parameters[2] = new ParameterValue();
                    parameters[2].Name = "SubscriptionIds";
                    parameters[2].Value = PkgSubscriptionIds.ToString();
                    if (ReportType == "Data Report")
                    {
                        parameters[3] = new ParameterValue();
                        parameters[3].Name = "CategoryIds";
                        parameters[3].Value = ShrdCategoryIds.ToString();
                        reportName = ReportTypeEnum.PASSPORT_REPORT_DATA_ONLY.GetStringValue();
                    }
                    else
                    {
                        parameters[3] = new ParameterValue();
                        parameters[3].Name = "SnapshotId";
                        parameters[3].Value = SnpShtId.ToString();
                        reportName = ReportTypeEnum.PASSPORT_REPORT_MULTI.GetStringValue();

                    }
                    if (loggedinUserID > AppConsts.NONE)
                    {
                        parameters[4] = new ParameterValue();
                        parameters[4].Name = "UserID";
                        parameters[4].Value = loggedinUserID.ToString();
                    }
                }

                else
                {
                    if (ServiceGroupID > 0)
                    {
                        if (BkgPkgServiceGroupId > 0)
                        {
                            parameters = new ParameterValue[5];
                        }
                        else
                        {
                            parameters = new ParameterValue[4];
                        }
                        parameters[3] = new ParameterValue();
                        parameters[3].Name = "PackageGroupID";
                        parameters[3].Value = ServiceGroupID.ToString();
                    }
                    else
                    {
                        if (BkgPkgServiceGroupId > 0)
                        {
                            parameters = new ParameterValue[4];
                        }
                        else
                        {
                            parameters = new ParameterValue[3];
                        }
                        //parameters = new ParameterValue[3];
                    }
                    if (BkgPkgServiceGroupId > 0)
                    {
                        var parmCount = parameters.Count();
                        parameters[parmCount - 1] = new ParameterValue();
                        parameters[parmCount - 1].Name = "BkgPkgSvcGrpID";
                        parameters[parmCount - 1].Value = BkgPkgServiceGroupId.ToString();
                    }

                    parameters[0] = new ParameterValue();
                    parameters[0].Name = "OrderID";
                    parameters[0].Value = OrderID.ToString();
                    parameters[1] = new ParameterValue();
                    parameters[1].Name = "TenantID";
                    parameters[1].Value = TenantId.ToString();
                    parameters[2] = new ParameterValue();
                    parameters[2].Name = "UserID";
                    parameters[2].Value = CurrentLoggedInUserId.ToString();


                }

                byte[] reportContent = ReportManager.GetReportByteArray(reportName, parameters);
                Response.ContentType = GetContentType(".pdf");
                Response.AddHeader("Content-Disposition", "inline;");
                if (reportContent.IsNotNull())
                {
                    Response.BinaryWrite(reportContent);
                }
            }
            catch (Exception ex)
            {
                LogError("", ex);
            }

            finally
            {
                Response.End();
            }
        }

        #region UAT-1316

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aWSUseS3"></param>
        private void GetClientApplicantDataByID(Boolean aWSUseS3)
        {
            string documentPath = String.Empty;
            string fileName = String.Empty;
            if (IsApplicantDocument)
            {
                var applicantDocData = Presenter.GetApplicantDocument();
                if (applicantDocData.IsNotNull())
                {
                    documentPath = applicantDocData.DocumentPath;
                    fileName = applicantDocData.FileName;
                }
            }
            else
            {
                var clientSysDocData = Presenter.GetClientSystemDocument();
                if (clientSysDocData.IsNotNull())
                {
                    documentPath = clientSysDocData.DocumentPath;
                    fileName = clientSysDocData.FileName;
                }
            }
            GetClientApplicantDocument(documentPath, fileName, aWSUseS3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aWSUseS3"></param>
        private void GetApplicantDocDataByFileName(Boolean aWSUseS3)
        {
            //String filePath = String.Empty;
            //String fileName = FileName;
            //String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
            //if (!tempFilePath.EndsWith(@"\"))
            //{
            //    tempFilePath += @"\";
            //}
            //tempFilePath += "Tenant(" + TenantId + @")\";
            //filePath = String.Format(tempFilePath + "{0}", FileName);
            ////GetClientApplicantDocument(filePath, fileName, aWSUseS3);
            Boolean isRadEditorEnabled = true;
            if (!ConfigurationManager.AppSettings["IsRadEditorEnabled"].IsNullOrEmpty())
            {
                isRadEditorEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsRadEditorEnabled"]);
            }
            if (!FilePath.IsNullOrEmpty() && isRadEditorEnabled)
            {
                InitializeRadEditor(FilePath);
            }
            else
            {
                String filePath = String.Empty;
                String fileName = FileName;
                String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                if (!tempFilePath.EndsWith(@"\"))
                {
                    tempFilePath += @"\";
                }
                tempFilePath += "Tenant(" + TenantId + @")\";
                filePath = String.Format(tempFilePath + "{0}", FileName);
                Initialize(filePath, fileName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <param name="aWSUseS3"></param>
        private void GetClientApplicantDocument(String filePath, String fileName, Boolean aWSUseS3)
        {
            if (aWSUseS3 == false)
            {
                Initialize(filePath, fileName);
            }
            else
            {
                InitializeS3Documents(filePath, fileName);
            }
        }

        #endregion

        /// <summary>
        /// Method to get the applicant document  for EDS service. 
        /// </summary>
        /// <param name="aWSUseS3"></param>
        private void GetEdsDocument(Boolean aWSUseS3)
        {
            var edsApplicantDocument = Presenter.GetApplicantDocumentForEds();
            if (edsApplicantDocument.IsNotNull())
            {
                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    Initialize(edsApplicantDocument.DocumentPath, edsApplicantDocument.FileName);
                }
                else
                {
                    InitializeS3Documents(edsApplicantDocument.DocumentPath, edsApplicantDocument.FileName);
                }
            }
        }

        #region Profile Sharing Attestion Document
        /// <summary>
        /// Method to get the Invitation document  for Profile Sharing
        /// </summary>
        /// <param name="aWSUseS3"></param>
        private void GetInvitationDocument(Boolean aWSUseS3)
        {
            var invitationDocument = Presenter.GetInvitationDocument();
            if (invitationDocument.IsNotNull())
            {
                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    Initialize(invitationDocument.IND_DocumentFilePath, "Attestation_Report");
                }
                else
                {
                    InitializeS3Documents(invitationDocument.IND_DocumentFilePath, "Attestation_Report");
                }
            }
        }
        #endregion



        public String UAFDocumentStage
        {
            get;
            set;
        }

        #region UAT-1544: WB: Create checkboxes for each document and a Print Button so that for each checked document, the student could print those documents.
        private void GetDataFromArguments()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT] != null)
            {
                args.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                if (args.ContainsKey("DocumentType") && args["DocumentType"].IsNotNull())
                {
                    DocumentType = args["DocumentType"];
                }
                if (args.ContainsKey("PrintDocumentPath") && args["PrintDocumentPath"].IsNotNull())
                {
                    DocumentPath = args["PrintDocumentPath"];
                }
            }
        }

        private void LoadApplicantPrintDocument()
        {
            try
            {
                Response.Clear();
                Response.Buffer = true;
                if (File.Exists(DocumentPath))
                {
                    String extension = Path.GetExtension(DocumentPath);
                    FileStream myFileStream = new FileStream(DocumentPath, FileMode.Open);
                    long FileSize = myFileStream.Length;
                    byte[] Buffer = new byte[(int)FileSize];
                    myFileStream.Read(Buffer, 0, (int)FileSize);
                    myFileStream.Close();
                    myFileStream.Dispose();
                    Response.ContentType = GetContentType(extension);
                    Response.AddHeader("Content-Disposition", "inline;");
                    if (Buffer.IsNotNull())
                    {
                        Response.BinaryWrite(Buffer);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("", ex);
            }

            finally
            {
                if (File.Exists(DocumentPath))
                {
                    File.Delete(DocumentPath);
                }
                Response.End();
            }
        }

        #region UAT-2774
        public void LoadSharedUserInvitationDocument(Boolean aWSUseS3)
        {
            var invitationDocument = Presenter.GetSharedUserInvitationDocumentByDocumentID();
            if (invitationDocument.IsNotNull())
            {
                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    Initialize(invitationDocument.IND_DocumentFilePath, invitationDocument.IND_FileName);
                }
                else
                {
                    InitializeS3Documents(invitationDocument.IND_DocumentFilePath, invitationDocument.IND_FileName);
                }
            }
        }
        #endregion

        #endregion
        /// <summary>
        /// Rad Editor's Save Event - UAT 3587
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PdfWebControl1_Saved(object sender, RadPdf.Integration.DocumentSavedEventArgs e)
        {
            try
            {
                var pathForSavingPDF = String.Empty;
                Boolean isRadEditorEnabled = false;
                if (!ConfigurationManager.AppSettings["IsRadEditorEnabled"].IsNullOrEmpty())
                {
                    isRadEditorEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsRadEditorEnabled"]);
                }
                if (isRadEditorEnabled == true)
                {
                    if (!Session["tmpDocPathsforSave"].IsNullOrEmpty() && DocumentType != "DisclosureDocumentForTracking")
                    {
                        List<String> lsttempDocPaths = new List<string>();
                        lsttempDocPaths = (List<String>)Session["tmpDocPathsforSave"];
                        var nonEmptyList = lsttempDocPaths.Where(x => !x.IsNullOrEmpty()).ToList();
                        var firstNonEmptyValue = nonEmptyList.FirstOrDefault();
                        pathForSavingPDF = firstNonEmptyValue;
                        lsttempDocPaths.Remove(firstNonEmptyValue);
                        Session["tmpDocPathsforSave"] = lsttempDocPaths;
                        //firstNonEmptyValue = null;
                    }
                    else
                    {
                        pathForSavingPDF = Convert.ToString(Session["tmpFilePathforTrackingDoc"]);
                    }
                    var docData = e.DocumentData;
                    if (!pathForSavingPDF.IsNullOrEmpty())
                    {
                        System.IO.File.WriteAllBytes(pathForSavingPDF, docData);
                    }

                    //For View Doc 
                    else
                    {
                        FilePath = Convert.ToString(Request.QueryString["DocumentPath"]);
                        System.IO.File.WriteAllBytes(FilePath, docData);
                    }

                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }

        #region Ticket Center
        /// <summary>
        /// Method to get the applicant document  for EDS service. 
        /// </summary>
        /// <param name="aWSUseS3"></param>
        private void GetTicketDocument(Boolean aWSUseS3)
        {
            var ticketDocument = Presenter.GetTicketDocument(TicketDocumentID);
            if (ticketDocument.IsNotNull())
            {
                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    Initialize(ticketDocument.FilePath, ticketDocument.Name);
                }
                else
                {
                    InitializeS3Documents(ticketDocument.FilePath, ticketDocument.Name);
                }
            }
        }
        #endregion

        #region UAT-4592
        /// <summary>
        /// Method to get Disclaimer Document
        /// </summary>
        private void GetDisclaimerDocumentData(Boolean aWSUseS3)
        {
            var disclaimerDocument = Presenter.GetDisclaimerDocumentData();
            this.DocumentPath = disclaimerDocument.DocumentPath;
            if (disclaimerDocument.IsNotNull())
            {
                //Check whether use AWS S3, true if need to use
                if (Convert.ToString(Session["IsSignatureStored"]) != "true")
                {
                    if (aWSUseS3 == false)
                    {
                        Initialize(disclaimerDocument.DocumentPath, disclaimerDocument.FileName);
                    }
                    else
                    {
                        InitializeS3Documents(disclaimerDocument.DocumentPath, disclaimerDocument.FileName);
                    }
                }
                else
                {
                    if (aWSUseS3 == false)
                    {
                        InitializeRadEditor(disclaimerDocument.DocumentPath);
                    }
                    else
                    {
                        InitializeS3Documents(disclaimerDocument.DocumentPath, disclaimerDocument.FileName);
                    }
                }
            }
        }
        #endregion
    }
}

