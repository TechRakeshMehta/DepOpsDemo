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

namespace CoreWeb.BkgOperations.UserControl.Views
{
    public partial class DisclosureAndRelease : BaseUserControl, IDisclosureAndReleaseView
    {
        #region VARIABLES

        private DisclosureAndReleasePresenter _presenter = new DisclosureAndReleasePresenter();
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


        public List<InstHierarchyRegulatoryEntityMappingDetails> RegulatoryNodeType
        {
            get;
            set;
        }


        public string RegulatoryNodeIDs
        {
            get;
            set;
        }

        public List<Entity.SystemDocument> DandRDocuments
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

        public List<SysDocumentFieldMappingContract> BkgDisclosureDocumentList
        {
            get
            {
                if (ViewState["BkgDisclosureDocumentList"].IsNotNull())
                {
                    return ViewState["BkgDisclosureDocumentList"] as List<SysDocumentFieldMappingContract>;
                }
                else
                {
                    return new List<SysDocumentFieldMappingContract>();
                }
            }
            set
            {
                ViewState["BkgDisclosureDocumentList"] = value;
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
                    rptDisclosureAndRelease.DataSource = BkgDisclosureDocumentList;
                    rptDisclosureAndRelease.DataBind();
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

        #endregion

        #region PRESENTER
        public DisclosureAndReleasePresenter Presenter
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
                    _applicantOrderCart.ApplicantDisclosureDocumentIds = null;
                    if (!_applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
                    {
                        //Retrieved the profile of User
                        var userProfile = _applicantOrderCart.lstApplicantOrder.FirstOrDefault().OrganizationUserProfile;

                        //Create Full Name of the user
                        StringBuilder fullName = new StringBuilder();
                        fullName.Append(userProfile.FirstName + " ");
                        if (userProfile.MiddleName.IsNotNull())
                            fullName.Append(userProfile.MiddleName + " ");
                        fullName.Append(userProfile.LastName);

                        //Fetch D and R Documents
                        FetchDandRDocuments();

                        //Fetch attributes for D and R Documents
                        Presenter.FetchDandRAttributes(_applicantOrderCart);

                        //Make the list of Documents IDs
                        List<Int32> lstDocIds = DandRDocuments.Select(cond => cond.SystemDocumentID).Distinct().ToList();

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
                            String documentPath = DandRDocuments.Where(cond => cond.SystemDocumentID == id).FirstOrDefault().DocumentPath;

                            //Fill the PDF with Attributes
                            byte[] fileBytes = Presenter.FillAttributesInPdf(documentPath, tempMappings, TenantID, fullName.ToString());

                            //Save the PDF filled with Attributes
                            if (fileBytes.IsNotNull())
                                StoreAttributesFilledPdf(fileBytes, id);
                        }

                        //Set the visibility of the D and R Form (i.e Repeater)
                        if (BkgDisclosureDocumentList.Count == AppConsts.NONE)
                        {
                            rptDisclosureAndRelease.Visible = false;
                            RestrictOrderFlow = true;
                        }

                        //Providing the datasource to repeater (BkgDisclosureDocumentList is filled in StoreAttributesFilledPdf method)
                        rptDisclosureAndRelease.DataSource = BkgDisclosureDocumentList;
                        rptDisclosureAndRelease.DataBind();
                    }
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

        protected void rptDisclosureAndRelease_OnItemDataBound(object sender, RepeaterItemEventArgs e)
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

            // Uncomment for display the checkboxes after each Pdf.   16-July-2014
            //CheckBox checkBox = e.Item.FindControl("chkDisclosureAndRelease") as CheckBox;
            //if (checkBox.IsNotNull())
            //{
            //    checkBox.Checked = IsDiclosureSubmitted;
            //    checkBox.Enabled = !IsDiclosureSubmitted;
            //}

        }
        #endregion

        #region METHODS

        /// <summary>
        /// Method to fetch Disclosure and Release Documents.
        /// </summary>
        private void FetchDandRDocuments()
        {
            try
            {

                List<String> Country = new List<string>();
                List<String> State = new List<string>();
                List<Int32> BkgPackages = new List<Int32>();

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

                foreach (var prevAddresses in _applicantOrderCart.lstPrevAddresses)
                {
                    if (prevAddresses.CountryId > AppConsts.NONE && prevAddresses.isDeleted == false)
                        Country.Add(prevAddresses.CountryId.ToString());

                    if (prevAddresses.isDeleted == false)
                        State.Add(prevAddresses.StateName);
                }

                var packagesList = _applicantOrderCart.lstApplicantOrder[0].lstPackages;
                if (!packagesList.IsNullOrEmpty())
                {
                    foreach (var item in packagesList)
                    {
                        BkgPackages.Add(item.BPAId);
                    }
                }
                //Get details like HierarchyNodeID, RegulatoryType and Services
                Presenter.GetServicesList(BkgPackages);
                HierarchyNodeID = _applicantOrderCart.SelectedHierarchyNodeID;
                Presenter.GetRegulatoryTypeList(HierarchyNodeID);
                //Get D and R Documents
                Presenter.GetDandRDocuments(Country, State, TenantID, _applicantOrderCart.DisclosureAgeGroupType);
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
        private bool StoreAttributesFilledPdf(byte[] fileBytes, Int32? SysDocId = null)
        {
            try
            {
                Boolean aWSUseS3 = false;
                if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                {
                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                }
                SaveDisclosurerPdfWithAttributes(fileBytes, aWSUseS3, SysDocId);
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
        private Boolean SaveDisclosurerPdfWithAttributes(byte[] pdfBytes, Boolean aWSUseS3, Int32? SysDocId = null)
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
            if (_applicantOrderCart.IsNotNull())
            {
                _presenter.GetPackageName(Convert.ToInt32(_applicantOrderCart.DPP_Id), TenantID);
            }
            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
            String destFileName = "DC_" + TenantID.ToString() + "_" + CurrentLoggedInUserID + "_" + date + ".pdf";
            String newTempFilePath = Path.Combine(tempFilePath, destFileName);
            String newFinalFilePath = String.Empty;

            if (!String.IsNullOrEmpty(PackageName))
            {
                filename = "Disclosure Form" + "_" + PackageName + "_" + date + ".pdf";
            }
            else
            {
                filename = "Disclosure Form" + "_" + "Package" + "_" + date + ".pdf";
            }
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
                LogInfo("Saving the disclosure document");

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

                String documentTypeCode = INTSOF.Utils.DocumentType.Disclosure_n_Release.GetStringValue();
                ApplicantDocument applicantDocument = _presenter.SaveDisclosureReleaseDocument(TenantID, newFinalFilePath, filename, filesize, documentTypeCode, CurrentLoggedInUserID, OrgUsrID);

                if (applicantDocument.IsNotNull())
                {
                    LogInfo(String.Format("Saved the disclosure document {0} for applicant{1}", applicantDocument.ApplicantDocumentID, applicantDocument.OrganizationUserID));
                    SysDocumentFieldMappingContract savedDocInfo = new SysDocumentFieldMappingContract();
                    savedDocInfo.ID = applicantDocument.ApplicantDocumentID;
                    savedDocInfo.DocumentPath = applicantDocument.DocumentPath;
                    savedDocInfo.FieldName = applicantDocument.FileName;
                    savedDocInfo.SystemDocOldID = SysDocId;
                    if (BkgDisclosureDocumentList.IsNullOrEmpty())
                        BkgDisclosureDocumentList = new List<SysDocumentFieldMappingContract>();
                    BkgDisclosureDocumentList.Add(savedDocInfo);

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
        #endregion

    }
}