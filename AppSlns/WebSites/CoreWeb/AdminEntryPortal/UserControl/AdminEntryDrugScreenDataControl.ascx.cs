using CoreWeb.BkgOperations.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using CoreWeb.Shell.Views;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace CoreWeb.AdminEntryPortal.Views
{
    public partial class AdminEntryDrugScreenDataControl : BaseUserControl, IAdminEntryDrugScreenDataControlView
    {
        #region Private Variables
        private AdminEntryDrugScreenDataControlPresenter _presenter = new AdminEntryDrugScreenDataControlPresenter();
        private Int32 _tenantId = 0;
        #endregion

        #region Properties
        public AdminEntryDrugScreenDataControlPresenter Presenter
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

        /// <summary>
        /// Gets and sets Logged In User TenantId
        /// </summary>
        Int32 IAdminEntryDrugScreenDataControlView.TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        public Int32 AttributeGroupId
        {
            get
            {
                if (!hdnAttributeGroupId.IsNullOrEmpty())
                    return Convert.ToInt32(hdnAttributeGroupId.Value);
                return AppConsts.NONE;
            }
            set
            {
                hdnAttributeGroupId.Value = value.ToString();
            }
        }

        public Int32 CustomFormId
        {
            get
            {
                if (!hdnCustomFormId.IsNullOrEmpty())
                    return Convert.ToInt32(hdnCustomFormId.Value);
                return AppConsts.NONE;
            }
            set
            {
                hdnCustomFormId.Value = value.ToString();
            }
        }

        List<BkgAttributeGroupMapping> IAdminEntryDrugScreenDataControlView.LstBkgAttributeGroupMapping { get; set; }
        public List<AttributesForCustomFormContract> LstAttributeForCustomFormContract
        {
            get
            {
                return (List<AttributesForCustomFormContract>)(ViewState["LstAttributeForCustomFormContract"]);
            }
            set
            {
                ViewState["LstAttributeForCustomFormContract"] = value;
            }
        }

        String IAdminEntryDrugScreenDataControlView.CustomHtml
        {
            set
            {
                divCustomHtml.InnerHtml = value;
            }

        }

        #region UAT-2842
        public Boolean IsAdminOrderScreen
        {
            get
            {
                if (ViewState["IsAdminOrderScreen"].IsNullOrEmpty())
                    return false;
                return Convert.ToBoolean(ViewState["IsAdminOrderScreen"]);
            }
            set
            {
                ViewState["IsAdminOrderScreen"] = value;
            }
        }
        #endregion

        #region Current View Context
        private IAdminEntryDrugScreenDataControlView CurrentViewContext
        {
            get { return this; }
        }
        #endregion

        public List<BackgroundOrderData> ListEDrugScreenData
        {
            get
            {
                //return (List<BackgroundOrderData>)(ViewState["ListEDrugScreenData"]);
                var drugScreeningData = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_DRUG_SCREENING) as List<BackgroundOrderData>;
                if (!drugScreeningData.IsNullOrEmpty())
                    return drugScreeningData;
                return new List<BackgroundOrderData>();
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_DRUG_SCREENING, value);
                // ViewState["ListEDrugScreenData"] = value;
            }
        }

        public Boolean IsReadOnly { get; set; }
        #endregion

        #region Events

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    Presenter.GetCustomHtmlContent();
                    divUCWebCCF.Visible = true;
                }
                if (IsAdminOrderScreen)
                {
                    CurrentViewContext.TenantId = GetSessionData().TenantId;
                }
                if (IsBackButtonNavigation())
                {
                    ManagePanelVisibility(true);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #region Web CCF Events
        protected void ucWebCCF_Success(object sender, WebCCFSuccessEventArgs e)
        {
            try
            {
                /* commented for UAT-5022*/
                //Int32 registraionId = AppConsts.NONE;
                //Int32.TryParse(e.RegistrationID, out registraionId);
                /* end UAT-5022*/
                Boolean isEDSOrderBlocked = false;

                if (CurrentViewContext.TenantId > AppConsts.NONE)
                {
                    isEDSOrderBlocked = Presenter.ProceedDrugScreeningOrderWithoutRegisterationId(CurrentViewContext.TenantId);
                }

                if ((!e.RegistrationID.IsNullOrEmpty() && (String.Compare(e.RegistrationID, AppConsts.NONE.ToString()) != 0)) || !isEDSOrderBlocked)
                {
                    List<BackgroundOrderData> lstResult = new List<BackgroundOrderData>();
                    BackgroundOrderData backgroundOrderData = new BackgroundOrderData();

                    backgroundOrderData.InstanceId = AppConsts.ONE;
                    backgroundOrderData.CustomFormId = CustomFormId;
                    backgroundOrderData.BkgSvcAttributeGroupId = AttributeGroupId;
                    backgroundOrderData.CustomFormData = GetDictionaryData(e);
                    lstResult.Add(backgroundOrderData);
                    ListEDrugScreenData = lstResult;
                    // count++;

                    //UAT-4251 
                    if (!e.ExpirationDate.IsNullOrEmpty())
                    {
                        lblDrugScreenExpiration.Visible = true;
                        lblExpiration.Visible = true;
                        lblExpiration.Text = e.ExpirationDate;
                    }
                    //END UAT

                    ApplicantOrderCart _applicantOrderCart = GetSessionData();

                    if (!_applicantOrderCart.IsNullOrEmpty())
                    {
                        _applicantOrderCart.EDrugScreeningRegistrationId = e.RegistrationID;
                        _applicantOrderCart.EDrugScreeningRegistrationExpirationDate = e.ExpirationDate;
                    }

                    //return lstResult;
                    if (!String.IsNullOrWhiteSpace(e.SuccessMessage))
                    {

                        ManagePanelVisibility(true);

                        if (Parent.IsNotNull())
                        {
                            CommandBar fsucCmdBar1 = Parent.FindControl("fsucCmdBar1") as CommandBar;
                            if (fsucCmdBar1.IsNotNull())
                            {
                                fsucCmdBar1.ClearButton.Enabled = true;
                            }
                        }
                        if (IsAdminOrderScreen)
                        {
                            Response.Redirect(String.Format("~/AdminEntryPortal/Pages/AdminEntryCustomFormPage.aspx?SelectedTenantId=" + Convert.ToString(CurrentViewContext.TenantId) + "&CustomFormId=" + Convert.ToString(CustomFormId) + "&IsPrevious=0"));
                        }
                    }
                }
                else
                {
                    ManageErrorVisibilityInCaseOfEmptyRegistrationID();
                }

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void ucWebCCF_Error(object sender, WebCCFErrorEventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(e.ErrorMessage))
                {
                    lblFailure.Visible = true;
                    lblFailure.Text = "Drug Screen Registration failed. Please try again in a few minutes. The error is: " + e.ErrorMessage;
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Button Events
        /// <summary>
        /// Load the WebCCF Widget.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdRefresh_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                ucWebCCF.Reload(true);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #endregion



        #region Methods

        #region Private Methods

        private Dictionary<int, String> GetDictionaryData(WebCCFSuccessEventArgs e)
        {
            Dictionary<int, String> dicAttributeData = new Dictionary<int, string>();
            foreach (AttributesForCustomFormContract bkgAttGrpMapp in CurrentViewContext.LstAttributeForCustomFormContract)
            {
                if (!dicAttributeData.ContainsKey(bkgAttGrpMapp.AtrributeGroupMappingId))
                {
                    switch (bkgAttGrpMapp.AttributeGroupMappingCode.ToUpper())
                    {
                        case AppConsts.DRUG_SCREEN_REGISTRATION_ID:
                            dicAttributeData.Add(bkgAttGrpMapp.AtrributeGroupMappingId, e.RegistrationID);
                            break;
                        case AppConsts.DONOR_GENDER:
                            dicAttributeData.Add(bkgAttGrpMapp.AtrributeGroupMappingId, e.DonorGender);
                            break;
                        case AppConsts.DONOR_EMAIL:
                            dicAttributeData.Add(bkgAttGrpMapp.AtrributeGroupMappingId, e.DonorEmail);
                            break;
                        case AppConsts.IS_US_CITIZEN:
                            dicAttributeData.Add(bkgAttGrpMapp.AtrributeGroupMappingId, e.IsUSCitizen);
                            break;
                        case AppConsts.LAB_NAME:
                            dicAttributeData.Add(bkgAttGrpMapp.AtrributeGroupMappingId, e.LabName);
                            break;
                        case AppConsts.REASON_DESCRIPTION:
                            dicAttributeData.Add(bkgAttGrpMapp.AtrributeGroupMappingId, e.CategoryReason);
                            break;
                        case AppConsts.REGISTRATION_EXPIRATION_DATE:
                            dicAttributeData.Add(bkgAttGrpMapp.AtrributeGroupMappingId, e.ExpirationDateCCF);
                            break;
                    }
                }
            }
            return dicAttributeData;
        }

        /// <summary>
        /// Gets the Session data
        /// </summary>
        /// <returns></returns>
        private ApplicantOrderCart GetSessionData()
        {
            return (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
        }

        /// <summary>
        /// Show/Hide the Panels of Location selection and Success message.
        /// Also used when the user selects browser back button after finishing the registration
        /// </summary>
        /// <param name="isRegistrationDone">
        /// Will be true when the RegistrationId is generated for the drug screening, after registration
        /// </param>
        private void ManagePanelVisibility(Boolean isRegistrationDone)
        {
            pnlReadOnly.Visible = isRegistrationDone;
            lblSuccess.Visible = isRegistrationDone;
            divUCWebCCF.Visible = !isRegistrationDone;
        }

        /// <summary>
        /// Checks if the user has navigated back to the screen by pressing the browser back button
        /// </summary>
        /// <returns></returns>
        private Boolean IsBackButtonNavigation()
        {
            ApplicantOrderCart _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            return String.IsNullOrEmpty(_applicantOrderCart.EDrugScreeningRegistrationId) ? false : true;
        }

        //UAT-3620
        private void ManageErrorVisibilityInCaseOfEmptyRegistrationID()
        {
            dvErrorMessageWithoutRegId.Visible = true;
            if (Parent.IsNotNull())
            {
                CommandBar fsucCmdBar1 = Parent.FindControl("fsucCmdBar1") as CommandBar;
                if (fsucCmdBar1.IsNotNull())
                {
                    fsucCmdBar1.ClearButton.Enabled = false;
                }
            }
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            if (!user.IsNullOrEmpty())
            {
                ApplicantOrderCart _applicantOrderCart = GetSessionData();

                if (!_applicantOrderCart.IsNullOrEmpty())
                {
                    Int32 selectedHierarchyNodeID = _applicantOrderCart.SelectedHierarchyNodeID.HasValue ? _applicantOrderCart.SelectedHierarchyNodeID.Value : AppConsts.NONE;

                    List<Int32> lstComplPkgIds = new List<Int32>();
                    if (!_applicantOrderCart.CompliancePackages.IsNullOrEmpty() && _applicantOrderCart.CompliancePackages.Count > AppConsts.NONE)
                    {
                        foreach (var item in _applicantOrderCart.CompliancePackages)
                        {
                            lstComplPkgIds.Add(item.Value.CompliancePackageID);
                        }
                    }

                    List<Int32> lstBkgPkgIds = new List<Int32>();
                    if (!_applicantOrderCart.lstApplicantOrder[0].IsNullOrEmpty()
                        && !_applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty()
                        && _applicantOrderCart.lstApplicantOrder[0].lstPackages.Count > AppConsts.NONE)
                    {
                        foreach (var item in _applicantOrderCart.lstApplicantOrder[0].lstPackages)
                        {
                            lstBkgPkgIds.Add(item.BPAId);
                        }
                    }

                    XmlDocument doc = new XmlDocument();
                    XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("SelectedPackageIds"));
                    el.AppendChild(doc.CreateElement("CompliancePackageIds")).InnerText = String.Join(",", lstComplPkgIds);
                    el.AppendChild(doc.CreateElement("BackgroundPackageIds")).InnerText = String.Join(",", lstBkgPkgIds);
                    String selectedPackageData = doc.OuterXml.ToString();

                    //UAT-3669: save entry  in security database
                    Presenter.UpdateBlockedOrdersHistoryData(user, selectedHierarchyNodeID, selectedPackageData);
                }
            }
        }

        #endregion

        #region Public Methods

        public List<BackgroundOrderData> GetEDrugData()
        {
            var _drugScreeningData = ListEDrugScreenData;
            //commented below code to reuse session data in case of next previous button click
            // Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);

            ApplicantOrderCart applicantOrderCart = GetSessionData();
            if (!applicantOrderCart.EDrugScreeningRegistrationExpirationDate.IsNullOrEmpty())
            {
                lblDrugScreenExpiration.Visible = true;
                lblExpiration.Visible = true;
                lblExpiration.Text = applicantOrderCart.EDrugScreeningRegistrationExpirationDate;
            }
            return _drugScreeningData;
        }

        #endregion

        #endregion
    }
}