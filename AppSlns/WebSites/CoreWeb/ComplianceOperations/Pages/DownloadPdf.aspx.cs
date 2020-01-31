using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using INTSOF.Utils;
using System.Web;
using System.IO;
using System.Threading;
using System.Web.UI;
using System.Net;
using System.Text;
using CoreWeb.Shell;
using System.Configuration;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.IntsofSecurityModel;
using System.Linq;


namespace CoreWeb.ComplianceOperations.Views
{
    public partial class DownloadPdf : BaseWebPage, IDownloadPdfView
    {
        #region Private Variables

        private DownloadPdfPresenter _presenter = new DownloadPdfPresenter();
        private Guid _fileIdentifier;
        private String _viewType;
        private Dictionary<String, String> queryString;

        #endregion

        #region Public Properties

        public String FilePath
        {
            get;
            set;
        }
        public String PdfFileName
        {
            get
            {
                return (String)ViewState["PdfFileName"];
            }
            set
            {
                ViewState["PdfFileName"] = value;
            }
        }

        public Int32 TenantID
        {
            get
            {
                return Convert.ToInt32(ViewState["TenantID"]);
            }
            set
            {
                ViewState["TenantID"] = value;
            }
        }
        public bool IsFromModifyShipping
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsFromModifyShipping"]);
            }
            set
            {
                ViewState["IsFromModifyShipping"] = value;
            }
        }

        public List<Int32> OrderIDs
        {
            get
            {
                return (List<Int32>)(ViewState["OrderIDs"]);
            }
            set
            {
                ViewState["OrderIDs"] = value;
            }
        }

        public Int32 CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
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

        public DownloadPdfPresenter Presenter
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
        #endregion

        #region Page Load Event

        protected void Page_Load(object sender, EventArgs e)
        {
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];

            try
            {
                if (Request.QueryString["fileIdentifier"] != null && !Request.QueryString["fileIdentifier"].Trim().Equals(""))
                {
                    _fileIdentifier = new Guid(Request.QueryString["fileIdentifier"]);

                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                     { "ID", Convert.ToString( _fileIdentifier)}
                                                                 };
                }
                Boolean _savePdfFile = false;
                if (Request.QueryString["IsSavePdfFile"].IsNotNull())
                {
                    _savePdfFile = Convert.ToBoolean(Request.QueryString["IsSavePdfFile"]);
                }

                if (!Request.QueryString["TenantID"].IsNullOrEmpty())
                {
                    TenantID = Convert.ToInt32(Request.QueryString["TenantID"]);
                }
                if (!Request.QueryString["IsFromModifyShipping"].IsNullOrEmpty())
                {
                    IsFromModifyShipping = Convert.ToBoolean(Request.QueryString["IsFromModifyShipping"]);
                }

                if (!Request.QueryString["OrderID"].IsNullOrEmpty())
                {
                    string inOrderID = Request.QueryString["OrderID"];
                    OrderIDs = new List<Int32>();

                    string[] arOrderId = inOrderID.Split(',');
                    if (arOrderId.IsNotNull() && arOrderId.Length > 0)
                    {
                        foreach (string orderId in arOrderId)
                            OrderIDs.Add(Convert.ToInt32(orderId.Trim()));
                    }
                    else
                        OrderIDs.Add(Convert.ToInt32(inOrderID));
                }

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                }
                Presenter.OnViewLoaded();

                if (_fileIdentifier.IsNotNull())
                {
                    GeneratePdf(_savePdfFile);
                }
            }

            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #region PrivateMethod
        /// <summary>
        /// This method generate the pdf of a html content by calling the PDFConversion.svc based on the url. 
        /// </summary>
        private void GeneratePdf(Boolean isSavePdfFile = false)
        {
            try
            {
                String host = String.Empty;
                String hostName = Request.Url.Host;
                if (!String.IsNullOrEmpty(hostName))
                {
                    Uri uri = HttpContext.Current.Request.Url;
                    host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;

                    /*Converted PDFConversion call to simple WCF Service*/
                    //create url for the pdf conversion service 
                    String url = String.Format(host + "/" + "TempFileViewer.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());


                    //Dictionary<String, String> contents = new Dictionary<String, String>
                    //                                              {
                    //                                                  {
                    //                                                      "tempFileConvertorUrl",
                    //                                                      url
                    //                                                  }                                                                    
                    //                                            };
                    //Call the service(PDFConversion.svc) to generate the pdfbytes based on the url             
                    byte[] pdfbytes = SysXEmailService.GetPdfBytes(url); //SysXEmailService.GetPdfBytes(contents, AppConsts.PDF_CONVERSION_SERVICE_CONTRACT_NAME, AppConsts.PDF_CONVERSION_SERVICE_OPERATION_CONTRACT);

                    if (pdfbytes.IsNotNull())
                    {
                        Presenter.GetFilePath(_fileIdentifier);
                        //Delete the temp file  
                        if (Presenter.DeleteTempFile(_fileIdentifier))
                        {
                            if (!String.IsNullOrEmpty(FilePath))
                            {
                                //Check whether use AWS S3, true if need to use
                                Boolean aWSUseS3 = false;
                                if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                                {
                                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                                }
                                if (aWSUseS3 == false)
                                {
                                    if (File.Exists(FilePath))
                                    {
                                        try
                                        {
                                            File.Delete(FilePath);
                                        }
                                        catch (Exception) { }
                                    }
                                }
                                else
                                {
                                    AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                                    objAmazonS3Documents.DeleteDocument(FilePath);
                                }
                            }
                        }
                        String fname = String.Empty;
                        if (PdfFileName.IsNotNull())
                        {
                            fname = PdfFileName + ".pdf";
                        }
                        else
                        {
                            fname = "Receipt.pdf";
                        }
                        //UAT-1035 WB: Capture and store order summary and add "Print Receipt" button to Order history for each order's order history. 
                        if (isSavePdfFile)
                        {
                            Boolean aWSUseS3 = false;
                            if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                            {
                                aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                            }
                            if (!Presenter.IsReciptAlreadySaved() || IsFromModifyShipping)
                            {
                                Boolean status = SaveRecieptDocument(pdfbytes, aWSUseS3, TenantID, PdfFileName);
                                ////UAT-2970
                                //if (status)
                                //{
                                //    SendOrderConfirmationWithDoc();
                                //}
                            }
                            //UAT-2970
                            SendOrderConfirmationWithDoc();
                        }
                        else
                        {
                            Response.Clear();
                            Response.Buffer = true;
                            Response.ClearContent();
                            Response.ClearHeaders();

                            // add the Content-Type and Content-Disposition HTTP headers
                            Response.AddHeader("Content-Type", "application/pdf");

                            //Response.AddHeader("Content-Disposition", String.Format("attachment; filename=ResultantPdf.pdf; size={0}",
                            //                pdfbytes.Length.ToString()));

                            Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}; size={1}", fname,
                                            pdfbytes.Length.ToString()));


                            // write the PDF document bytes as attachment to HTTP response 
                            Response.BinaryWrite(pdfbytes);
                        }

                        // Note: it is important to end the response, otherwise the ASP.NET
                        // web page will render its content to PDF document stream
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (ThreadAbortException thex)
            {

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }


        private Boolean SaveRecieptDocument(byte[] pdfBytes, Boolean aWSUseS3, Int32 TenantID, String PdfFileName)
        {
            FileStream _FileStream = null;
            try
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

                String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                String destFileName = PdfFileName + "_" + TenantID.ToString() + "_" + CurrentLoggedInUserID + "_" + date + ".pdf";
                String newTempFilePath = Path.Combine(tempFilePath, destFileName);
                String newFinalFilePath = String.Empty;
                filename = PdfFileName + ".pdf";
                _FileStream = new FileStream(newTempFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
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
                    base.LogError("Error while closing the _FileStream.", new SystemException());
                }

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
                try
                {
                    if (!String.IsNullOrEmpty(newTempFilePath))
                        File.Delete(newTempFilePath);
                }
                catch (Exception)
                {
                    base.LogError("Error while deleting the temporary file.", new SystemException());
                }
                String documentTypeCode = INTSOF.Utils.DocumentType.Reciept_Document.GetStringValue();
                //Save PDF and Update Order
                return _presenter.SaveRecieptDocument(TenantID, newFinalFilePath, filename, filesize, documentTypeCode, CurrentLoggedInUserID, OrgUsrID);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                throw ex;
            }
            finally
            {
                try { _FileStream.Close(); }
                catch (Exception) { }
            }
        }

        //UAT-2970
        private void SendOrderConfirmationWithDoc()
        {
            Int32 tenantId = AppConsts.NONE;
            List<OrderPaymentDetail> lstOrderPaymentDetail = new List<OrderPaymentDetail>();
            Boolean isOrderHasCreditCardPaymentType = false;
            OrderPaymentDetail opd = new OrderPaymentDetail();
            Int32 currentLoggedInUser = AppConsts.NONE;
            String orderNumber = String.Empty;

            ApplicantOrderCart _applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;
            if (!_applicantOrderCart.IsNullOrEmpty())
            {
                Boolean isChangeSubscription = (_applicantOrderCart.OrderRequestType == OrderRequestType.ChangeSubscriptionByAdmin.GetStringValue());
                String pendingSchoolApprovalStatusCode = ApplicantOrderStatus.Pending_School_Approval.GetStringValue();
                String pendingPaymentApproval = ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue();
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;

                List<Int32> lstOrderIds = _applicantOrderCart.AllOrderIDs.Split(',').ConvertIntoIntList();

                foreach (Int32 orderId in lstOrderIds)
                {
                    if (!tenantId.IsNullOrEmpty() && tenantId > AppConsts.NONE && !_applicantOrderCart.OrderId.IsNullOrEmpty() && _applicantOrderCart.OrderId > AppConsts.NONE)
                        //  lstOrderPaymentDetail = Presenter.GetAllPaymentDetailsOfOrderByOrderID(tenantId, _applicantOrderCart.OrderId);
                        lstOrderPaymentDetail = Presenter.GetAllPaymentDetailsOfOrderByOrderID(tenantId, orderId);

                    if (!lstOrderPaymentDetail.IsNullOrEmpty())
                    {
                        OrderPaymentDetail newOpd = lstOrderPaymentDetail.FirstOrDefault(con => con.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue() && !(con.lkpOrderStatu.Code == pendingSchoolApprovalStatusCode));

                        if (!newOpd.IsNullOrEmpty())
                        {
                            isOrderHasCreditCardPaymentType = true;
                            currentLoggedInUser = newOpd.Order.OrganizationUserProfile.OrganizationUserID;
                            orderNumber = orderNumber == String.Empty ? newOpd.Order.OrderNumber : (orderNumber + " , " + newOpd.Order.OrderNumber);
                            opd = newOpd;
                        }
                    }
                }
            }
            if (isOrderHasCreditCardPaymentType && !opd.IsNullOrEmpty() && tenantId > AppConsts.NONE)
            {
                if (!Session["IsNeedToSendOrderConfirmationDocument"].IsNullOrEmpty() && Convert.ToBoolean(Session["IsNeedToSendOrderConfirmationDocument"]))
                {
                    Presenter.SendOrderConfirmationDocEmail(tenantId, currentLoggedInUser, opd, orderNumber);
                    Session["IsNeedToSendOrderConfirmationDocument"] = null;
                }

            }
        }

        #endregion
    }
}

